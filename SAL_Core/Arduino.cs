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
                ReadTimeout = 200
            };

            serial.Open();
            GetChannels();
            SetColor(Colors.RED);
        }

        public void GetChannels()
        { 
            try
            {
                dataArr[0] = 242;
                Send(dataArr);
                Channels = Receive();
            }
            catch(TimeoutException)
            {
                GetChannels(0);
            }
        }

        public void GetChannels(int retryCount)
        {
            try
            {
                dataArr[0] = 242;
                Send(dataArr);
                Channels = Receive();
            }
            catch (TimeoutException)
            {
                if (retryCount < 10) GetChannels(++retryCount);
                else throw new Exception("Failed at getting device Model");
            }
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

        /*public void SetColor(int R, int G, int B)
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

        }*/

        public int Channels { get; private set; } = 1;

        public void SetColor(int R, int G, int B)
        {
            SetColor(0, R, G, B);
        }

        public void SetColor(int channel, int R, int G, int B)
        {
            int ch = 252;
            if(Channels != 1 && channel > 0 && channel <= 4)
            {
                ch -= channel * 2;
            }

            R = NormalizeColor(R);
            G = NormalizeColor(G);
            B = NormalizeColor(B);

            byte[] data = { (byte)ch, (byte)R, (byte)G, (byte)B };
            Send(data);
        }

        private int NormalizeColor(int c)
        {
            if(c < 0) c = 0;
            else if (c > 255) c = 255;
            else
            {
                switch (c)
                {
                    case 252:
                    case 250:
                    case 248:
                    case 246:
                    case 244:
                    case 242:
                        c++;
                        break;
                }
            }
            return c;
        }

        private Color _color = Colors.NONE;
        public void SetColor(Color color)
        {
            if (color == _color) return;
            _color = color;
            SetColor(0, color.R, color.G, color.B);
        }

        private Color[] _colors = { Colors.NONE, Colors.NONE, Colors.NONE, Colors.NONE };
        public void SetColor(int channel, Color color)
        {
            if(channel <= 0 || channel > 4)
            {
                SetColor(color);
            }
            else
            {
                if (color == _colors[channel - 1]) return;
                _colors[channel - 1] = color;
                SetColor(channel, color.R, color.G, color.B);
            }
        }


        private void Send(byte[] data)
        {
            if (usingUDP) udp.Send(data);
            else
            {
                serial.Write(data, 0, data.Length);
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
        private readonly ConcurrentQueue<ChColor> queue;
        private readonly Thread thread;
        //private Color _color = Colors.NONE;
        private readonly Color[] _chColor = new Color[100];

        public ArduinoCollection()
        {
            for (int i = 0; i < _chColor.Length; i++) _chColor[i] = Colors.NONE;

            thread = new Thread(new ThreadStart(Worker));
            queue = new ConcurrentQueue<ChColor>();
            thread.Start();
        }

        private void Worker()
        {
            while (true)
            {
                if(queue.TryDequeue(out ChColor chColor))
                {
                    int channel = chColor.Channel;
                    if (channel == 0)
                    {
                        foreach (Arduino arduino in collection)
                        {
                            arduino.SetColor(chColor.Color);
                        }
                    }
                    else
                    {
                        
                        foreach (Arduino arduino in collection)
                        {
                            if (channel - arduino.Channels <= 0)
                            {
                                arduino.SetColor(channel, chColor.Color);
                                break;
                            }
                            else
                            {
                                channel -= arduino.Channels;
                            }
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
            ChannelCount += arduino.Channels;
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
            if (_chColor[0] != color)
            {
                _chColor[0] = color;
                queue.Enqueue(new ChColor(0, color));
            } 
        }

        public int ChannelCount { get; private set; } = 0;

        

        public void SetColor(int channel, Color color)
        {
            if (channel >= 0 && channel <= ChannelCount && color != _chColor[channel])
            {
                _chColor[channel] = color;
                queue.Enqueue(new ChColor(channel, color));
            }
        }

    }

    public static class Colors
    {
        public static Color NONE { get; } = new Color(-1, -1, -1);
        public static Color OFF { get; } = new Color(0, 0, 0);
        public static Color RED { get; } = new Color(255, 0, 0);
        public static Color GREEN { get; } = new Color(0, 255, 0);
        public static Color BLUE { get; } = new Color(0, 0, 255);
        public static Color YELLOW { get; } = new Color(255, 255, 0);
        public static Color MAGENTA { get; } = new Color(255, 0, 255);
        public static Color CYAN { get; } = new Color(0, 255, 255);
        public static Color ORANGE { get; } = new Color(255, 127, 0);
        public static Color LYME { get; } = new Color(127, 255, 0);
        public static Color PURPLE { get; } = new Color(127, 0, 255);
        public static Color PINK { get; } = new Color(255, 0, 127);
        public static Color AQGREEN { get; } = new Color(0, 255, 127);
        public static Color EBLUE { get; } = new Color(0, 127, 255);
        public static Color WHITE { get; } = new Color(255, 255, 255);
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

    public struct ChColor
    {
        public readonly Color Color;
        public readonly int Channel;

        public ChColor(int channel, Color color)
        {
            Color = color;
            Channel = channel;
        }
    }
}
