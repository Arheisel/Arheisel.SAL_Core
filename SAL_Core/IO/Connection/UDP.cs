using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SAL_Core.IO.Connection
{
    public class UDPServer : IDisposable
    {
        private readonly IPEndPoint ip;
        private UdpClient sock;
        private Thread thread;
        private int consumers = 0;

        public readonly ConcurrentDictionary<uint, IPEndPoint> DetectedArduinos;
        

        public event EventHandler OnNewArduinoDetected;

        public UDPServer(int port)
        {
            ip = new IPEndPoint(IPAddress.Any, port);
            DetectedArduinos = new ConcurrentDictionary<uint, IPEndPoint>();
        }

        public void Start()
        {
            if (Interlocked.Increment(ref consumers) == 1)
            {
                sock = new UdpClient(ip)
                {
                    EnableBroadcast = true
                };
                thread = new Thread(new ThreadStart(Worker)) { IsBackground = true };
                thread.Start();
            }
        }

        public void Stop()
        {
            if(Interlocked.Decrement(ref consumers) == 0)
            {
                thread.Abort();
                sock?.Dispose();
            }
        }


        private void Worker()
        {
            while (true)
            {
                var data = Receive(out IPEndPoint endPoint, true);
                if (data.Length == 5 && data[0] == 252)
                {
                    if (data[1] == 0xFA)
                    {
                        uint id = 0;
                        id = (id | data[2]) << 8;
                        id = (id | data[3]) << 8;
                        id |= data[4];
                        if(DetectedArduinos.TryAdd(id, endPoint))
                        {
                            OnNewArduinoDetected?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
            }
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
                Task.Delay(20).Wait();
            }

            endPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = sock.Receive(ref endPoint);
            return data;
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                thread.Abort();
                sock?.Dispose();
            }

            _disposed = true;
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

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                sock?.Dispose();
            }

            _disposed = true;
        }
    }
}
