using System;
using System.Net.Sockets;
using System.Threading;
using Arheisel.Log;

namespace SAL_Core.IO.Connection
{
    class TCP : IConnection
    {
        private readonly string IP;
        private readonly int Port;

        private TcpClient tcp = null;
        private NetworkStream stream = null;

        public TCP(string ip, int port)
        {
            IP = ip;
            Port = port;
            StartTCPClient();
        }

        private void StartTCPClient(bool retry = false, int retryCount = 0)
        {
            try
            {
                tcp = new TcpClient(IP, Port);
                tcp.NoDelay = true;
                tcp.SendTimeout = 500;
                tcp.ReceiveTimeout = 500;
                if (tcp.Connected)
                {
                    stream = tcp.GetStream();
                    return;
                }
                else
                    throw new Exception("Connection failed.");
            }
            catch
            {
                if (retry && retryCount < 10)
                {
                    StartTCPClient(retry, ++retryCount);
                }
                else
                {
                    throw;
                }
            }
        }

        public void Close()
        {
            try
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (tcp != null)
                {
                    tcp.Close();
                    tcp.Dispose();
                }
                tcp = null;
                stream = null;
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
        }

        public void Send(byte[] data)
        {
            if (stream == null)
            {
                throw new Exception("stream is null");
            }
            try
            {
                stream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Log.Exception(e);

                Log.Info("Attempting to reopen connection");
                try
                {
                    Close();
                    StartTCPClient(true);
                }
                catch
                {
                    throw;
                }
            }
        }

        public byte[] Receive(bool wait = false) //This function is absolutely atrocious, but for what is needed it's suitable
        {
            try
            {
                DelayTimeout(tcp, 1, wait);
                if (stream.ReadByte() == 252)
                {
                    DelayTimeout(tcp, 1);
                    var len = stream.ReadByte();
                    byte[] data = new byte[len];
                    DelayTimeout(tcp, len);
                    stream.Read(data, 0, len);
                    return data;
                }
                else
                {
                    //discard all data
                    var buffer = new byte[4096];
                    while (stream.DataAvailable)
                    {
                        stream.Read(buffer, 0, buffer.Length);
                    }
                    return new byte[0];
                }
            }
            catch
            {
                throw;
            }
        }

        private void DelayTimeout(TcpClient client, int length, bool wait = false)
        {
            var time = DateTime.Now;
            var diff = new TimeSpan(0, 0, 10);
            while (client.Available < length)
            {
                if ((DateTime.Now - time) > diff && !wait)
                {
                    throw new Exception("E_TCP_TIMEOUT: La conexion TCP excedio el tiempo de espera.");
                }
                Thread.Sleep(20);
            }
        }
    }
}
