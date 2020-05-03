using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAL_Core
{
    public class Arduino
    {
        private SerialPort serial;
        private readonly byte[] dataArr = new byte[1];
        private UDPCLient udp = null;
        private static int port = 9050;
        private bool usingUDP = false;
        public readonly string Name;


        /// <summary>
        /// Initializes a Serial Arduino Connection
        /// </summary>
        /// <param name="com">COM Port Name</param>
        public Arduino(string com)
        {
            Name = com;

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
            Name = ip + ":" + dstPort;
            StartUDPClient(ip, dstPort);
        }

        /*public void SetColor(Colors color)
        {
            if (color != _color && color != Colors.NONE)
            {
                _color = color;
                Send((byte)color);
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
                Send((byte)data);
            }
        }*/

        public void SetColor(int R, int G, int B)
        {
            if (R < 0) R = 0;
            else if (R > 14) R = 14;
            if (G < 0) G = 0;
            else if (G > 14) G = 14;
            if (B < 0) B = 0;
            if (B > 14) B = 14;

            byte data = 0xF0; //sync nibble
            data += (byte)R;
            Send(data);

            data = (byte)G;
            data <<= 4;
            data += (byte)B;
            Send(data);

        }

        private Color _color = Colors.NONE;
        public void SetColor(Color color)
        {
            if (color == _color) return;
            _color = color;
            SetColor(color.R, color.G, color.B);
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
            if (usingUDP) udp.Send(data);
            else
            {
                dataArr[0] = data;
                serial.Write(dataArr, 0, 1);
            }
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

    public class ArduinoCollection
    {
        private readonly List<Arduino> collection = new List<Arduino>();
        private readonly ConcurrentQueue<Color> queue;
        private readonly Thread thread;
        private Color _color = Colors.NONE;

        public ArduinoCollection()
        {
            thread = new Thread(new ThreadStart(Worker));
            queue = new ConcurrentQueue<Color>();
            thread.Start();
        }

        private void Worker()
        {
            while (true)
            {
                if(queue.TryDequeue(out Color color))
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
                Thread.Sleep(1);
            }
        }

        public Arduino this[int index]
        {
            get
            {
                return collection[index];
            }
        }

        public int FindByName(string name)
        {
            for(int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Name == name) return i;
            }
            return -1;
        }

        public void Add(Arduino arduino)
        {
            if (FindByName(arduino.Name) != -1) return;
            collection.Add(arduino);
        }

        public int Count
        {
            get
            {
                return collection.Count;
            }
        }

        public void SetColor(Color color)
        {
            queue.Enqueue(color);
        }

        public void SetColor(int R, int G, int B)
        {
            foreach (Arduino arduino in collection)
            {
                arduino.SetColor(R, G, B);
            }
        }

        public int ChannelCount
        {
            get
            {
                return collection.Count * 8;
            }
        }

        /*private readonly Color[] _chColor = new Color[1024];

        public void SetColor(int channel, Color color)
        {
            if (channel >= 0 && channel <= ChannelCount - 1 && (color != _chColor[channel] || _color != Colors.NONE))
            {
                _chColor[channel] = color;
                _color = Colors.NONE;
                collection[Convert.ToInt32(Math.Floor((double)channel / 8))].SetColor(channel % 8, color);
            }
        }*/

    }

    public static class Colors
    {
        public static Color NONE { get; } = new Color(-1, -1, -1);
        public static Color OFF { get; } = new Color(0, 0, 0);
        public static Color RED { get; } = new Color(14, 0, 0);
        public static Color GREEN { get; } = new Color(0, 14, 0);
        public static Color BLUE { get; } = new Color(0, 0, 14);
        public static Color YELLOW { get; } = new Color(14, 14, 0);
        public static Color MAGENTA { get; } = new Color(14, 0, 14);
        public static Color CYAN { get; } = new Color(0, 14, 14);
        public static Color ORANGE { get; } = new Color(14, 6, 0);
        public static Color LYME { get; } = new Color(6, 14, 0);
        public static Color PURPLE { get; } = new Color(6, 0, 14);
        public static Color PINK { get; } = new Color(14, 0, 6);
        public static Color AQGREEN { get; } = new Color(0, 14, 6);
        public static Color EBLUE { get; } = new Color(0, 6, 14);
        public static Color WHITE { get; } = new Color(14, 14, 14);
    }

    public struct Color
    {
        public readonly int R;
        public readonly int G;
        public readonly int B;

        public Color(int R, int G, int B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public static bool operator == (Color c1, Color c2)
        {
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }

        public static bool operator != (Color c1, Color c2)
        {
            return !(c1 == c2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
