using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SAL_Core
{
    class UDPServer
    {
        private readonly IPEndPoint ip;
        private readonly UdpClient sock;

        public UDPServer(int port)
        {
            ip = new IPEndPoint(IPAddress.Any, port);
            sock = new UdpClient(ip);
        }

        public byte Receive()
        {
            var endp = new IPEndPoint(IPAddress.Any, 0);
            var data = sock.Receive(ref endp);
            return data[0];
        }

        public void Dispose()
        {
            sock.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    class UDPCLient
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
            sock.Send(dgram, 1, dest);
        }

        public void Dispose()
        {
            sock.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
