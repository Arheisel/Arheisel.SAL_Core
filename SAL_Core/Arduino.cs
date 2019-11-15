using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAL_Core
{
    class Arduino
    {
        private SerialPort serial;
        private readonly byte[] dataArr = new byte[1];
        private UDPCLient udp = null;
        private static int port = 9050;
        private bool usingUDP = false;
        public readonly string name;

        /// <summary>
        /// Initializes a Serial Arduino Connection
        /// </summary>
        /// <param name="com">COM Port Name</param>
        public Arduino(string com)
        {
            name = com;

            serial = new SerialPort
            {
                PortName = com,
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,

                WriteTimeout = 5,
                ReadTimeout = 1000
            };

            serial.Open();
            SetColor(Colors.RED);
        }

        /// <summary>
        /// Initializes an IP Arduino Connection
        /// </summary>
        /// <param name="ip">IP of the Receiver</param>
        /// <param name="dstPort">Receiver Port</param>
        public Arduino(string ip, int dstPort)
        {
            name = ip + ":" + port;
            StartUDPClient(ip, dstPort);
        }

        private Colors _color = 0;

        public void SetColor(Colors color)
        {
            if (color != _color && color != Colors.NONE)
            {
                _color = color;
                if (usingUDP) udp.Send((byte)color);
                else Send((byte)color);
            }
        }

        private readonly Colors[] _chColor = new Colors[8];

        public void SetColor(int channel, Colors color)
        {
            if (channel >= 0 && channel <= 7 && (color != _chColor[channel] || _color != Colors.NONE) && color != Colors.NONE)
            {
                _chColor[channel] = color;
                _color = Colors.NONE;
                uint data = (uint)channel + 1;
                data <<= 4;
                data += (uint)color;
                if (usingUDP) udp.Send((byte)data);
                else Send((byte)data);
            }
        }

        public void WriteToSerial(byte data)
        {
            if (usingUDP) return;
            else Send(data);
        }

        public bool UsingUDP
        {
            get
            {
                return usingUDP;
            }
        }
        private void Send(byte data)
        {
            dataArr[0] = data;
            serial.Write(dataArr, 0, 1);
        }

        private int Receive()
        {
            return serial.ReadByte();
        }

        public bool TryReceive(out int data)
        {
            data = 0;
            if (!usingUDP && serial.BytesToRead > 0)
            {
                data = Receive();
                return true;
            }
            return false;
        }

        public void StartUDPClient(string ip, int dstPort)
        {
            udp = new UDPCLient(ip, dstPort, port++);
            usingUDP = true;
            SetColor(Colors.RED);
        }

        public void StopUDPClient()
        {
            udp.Dispose();
            udp = null;
            usingUDP = false;
            port--;
        }


    }

    class ArduinoCollection
    {
        private readonly List<Arduino> collection = new List<Arduino>();

        public Arduino this[int index]
        {
            get
            {
                return collection[index];
            }
        }

        public void Add(Arduino arduino)
        {
            collection.Add(arduino);
        }

        public int Count
        {
            get
            {
                return collection.Count;
            }
        }

        private Colors _color = 0;

        public void SetColor(Colors color)
        {
            if (color != _color)
            {
                _color = color;
                foreach (Arduino arduino in collection)
                {
                    arduino.SetColor(color);
                }
            }
        }

        public int ChannelCount
        {
            get
            {
                return collection.Count * 8;
            }
        }

        private readonly Colors[] _chColor = new Colors[1024];

        public void SetColor(int channel, Colors color)
        {
            if (channel >= 0 && channel <= ChannelCount - 1 && (color != _chColor[channel] || _color != Colors.NONE))
            {
                _chColor[channel] = color;
                _color = Colors.NONE;
                collection[Convert.ToInt32(Math.Floor((double)channel / 8))].SetColor(channel % 8, color);
            }
        }

    }

    enum Colors
    {
        NONE = -1,
        OFF = 0,
        RED = 1,
        GREEN = 2,
        BLUE = 3,
        YELLOW = 4,
        MAGENTA = 5,
        CYAN = 6,
        ORANGE = 7,
        LYME = 8,
        PURPLE = 9,
        PINK = 10,
        AQGREEN = 11,
        EBLUE = 12,
        WHITE = 13
    }
}
