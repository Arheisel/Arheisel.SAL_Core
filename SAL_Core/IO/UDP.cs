using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SAL_Core.IO
{
    public class UDPServer : IDisposable
    {
        private readonly IPEndPoint ip;
        private readonly UdpClient sock;

        public UDPServer(int port)
        {
            ip = new IPEndPoint(IPAddress.Any, port);
            sock = new UdpClient(ip);
            sock.EnableBroadcast = true;
        }

        public byte[] Receive(out IPEndPoint endPoint, bool wait = false)
        {
            var time = DateTime.Now;
            var diff = new TimeSpan(0, 0, 10);
            while (sock.Available == 0)
            {
                if ((DateTime.Now - time) > diff && !wait)
                {
                    throw new Exception("E_UDP_TIMEOUT: No se han recibido datos.");
                }
                Thread.Sleep(20);
            }

            endPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = sock.Receive(ref endPoint);
            return data;
        }

        public void Dispose()
        {
            sock.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public class UDPCLient
    {
        private readonly IPEndPoint ip;
        private readonly UdpClient sock;
        private readonly IPEndPoint dest;

        public UDPCLient(string ipAddr, int dstPort, int srcPort)
        {
            ip = new IPEndPoint(IPAddress.Any, srcPort);
            sock = new UdpClient(ip);
            dest = new IPEndPoint(IPAddress.Parse(ipAddr), dstPort);
        }

        public void Send(byte data)
        {
            var dgram = new byte[] { data };
            Send(dgram);
        }

        public void Send(byte[] data)
        {
            sock.Send(data, data.Length, dest);
        }

        public void Dispose()
        {
            sock.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
