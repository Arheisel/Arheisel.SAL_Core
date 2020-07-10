using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Damez.Log;

namespace SAL_Core
{
    public class Arduino : IDisposable
    {
        private SerialPort serial;
        //private readonly byte[] dataArr = new byte[1];
        private UDPCLient udp = null;
        private static int port = 9050;

        public string Name { get; private set; }

        public bool Online { get; private set; } = true;

        public ArduinoSettings Settings { get; private set; } = null;

        public Arduino(ArduinoSettings settings, bool muteExceptions = false)
        {
            Settings = settings;
            if(settings.ConnectionType == ConnectionType.Serial)
            {
                Name = settings.COM;
                try
                {
                    StartSerial(settings.COM);
                    SetColor(Colors.RED);
                }
                catch (Exception e)
                {
                    serial.Close();
                    Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                    if (!muteExceptions) throw;
                }
            }
            else
            {
                Name = Settings.IP + ":" + Settings.Port;
                try
                {
                    StartUDPClient(Settings.IP, Settings.Port);
                }
                catch (Exception e)
                {
                    Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                    if (!muteExceptions) throw;
                }
            }
        }

        /// <summary>
        /// Initializes a Serial Arduino Connection
        /// </summary>
        /// <param name="com">COM Port Name</param>
        public Arduino(string com, bool reverse = false, bool muteExceptions = false)
        {
            Name = com;
            Settings = new ArduinoSettings(com);
            Settings.Reverse = reverse;
            try
            {
                StartSerial(com);
                SetColor(Colors.RED);
            }
            catch(Exception e)
            {
                serial.Close();
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                if(!muteExceptions) throw;
            }
        }

        public void StartSerial(string com, int retryCount = 0)
        {
            try
            {
                serial = new SerialPort
                {
                    PortName = Name,
                    BaudRate = 115200,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Handshake = Handshake.None,
                    //DtrEnable = true,

                    WriteTimeout = 100,
                    ReadTimeout = 200
                };

                serial.Open();
                byte[] dataArr = { 252, 1, 1 }; //get model
                serial.Write(dataArr, 0, dataArr.Length);
                var id = Receive();
                if(id.Length == 2 && id[0] == 250)
                    Channels = id[1];
                else throw new Exception("Failed at getting device Model");
            }
            catch (TimeoutException)
            {
                if (retryCount < 10 && Online)
                {
                    serial.Close();
                    Thread.Sleep(250);
                    StartSerial(com, ++retryCount);
                }
                else
                {
                    Online = false;
                    throw new Exception("Failed at getting device Model");
                }
            }
        }

        /// <summary>
        /// Initializes an IP Arduino Connection
        /// </summary>
        /// <param name="ip">IP of the Receiver</param>
        /// <param name="dstPort">Receiver Port</param>
        public Arduino(string ip, int dstPort, bool reverse = false, bool muteExceptions = false)
        {
            Name = ip + ":" + dstPort;
            Settings = new ArduinoSettings(ip, dstPort);
            Settings.Reverse = reverse;
            try
            {
                StartUDPClient(ip, dstPort);
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                if(!muteExceptions) throw;
            }
        }

        public override string ToString()
        {
            if (Online)
            {
                if(Settings.Reverse) return Name + " - Reversed";
                else return Name;
            }
            else return Name + " - Offline";
        }

        public int Channels { get; private set; } = 1;

        public void SetColor(Color color)
        {
            SetColor(0, color);
        }

        public void SetColor(int channel, Color color)
        {
            if (channel < 0 || channel > Channels)
            {
                return;
            }

            int ch = 100;
            if (Channels != 1 && channel > 0)
            {
                if (Settings.Reverse)
                    ch += Channels - channel + 1;
                else
                    ch += channel;
            }

            var R = NormalizeColor(color.R);
            var G = NormalizeColor(color.G);
            var B = NormalizeColor(color.B);

            byte[] data = { (byte)ch, (byte)R, (byte)G, (byte)B };
            Send(data);
        }

        public void SetColor(Color[] colors)
        {
            if (colors.Length != Channels) return;

            byte[] data = new byte[Channels * 3 + 1];
            data[0] = 99;

            if (Settings.Reverse)
            {
                for (int i = 0; i < Channels; i++)
                {
                    data[i * 3 + 1] = (byte)NormalizeColor(colors[Channels - i - 1].R);
                    data[i * 3 + 2] = (byte)NormalizeColor(colors[Channels - i - 1].G);
                    data[i * 3 + 3] = (byte)NormalizeColor(colors[Channels - i - 1].B);
                }
            }
            else
            {
                for (int i = 0; i < Channels; i++)
                {
                    data[i * 3 + 1] = (byte)NormalizeColor(colors[i].R);
                    data[i * 3 + 2] = (byte)NormalizeColor(colors[i].G);
                    data[i * 3 + 3] = (byte)NormalizeColor(colors[i].B);
                }
            }
            
            Send(data);
        }

        private int NormalizeColor(int c)
        {
            if(c < 0) c = 0;
            else if (c > 255) c = 255;
            /*else
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
            }*/
            return c;
        }

        private void Send(byte[] data)
        {
            try
            {
                if (data.Length > 255) return;
                byte[] header = { 252, (byte)data.Length };
                data = header.Concat(data);
                if (Settings.ConnectionType == ConnectionType.UDP) udp.Send(data);
                else
                {
                    serial.Write(data, 0, data.Length);
                }
            }
            catch (TimeoutException e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                if(Settings.ConnectionType == ConnectionType.Serial)
                {
                    Log.Write(Log.TYPE_INFO, "Arduino :: " + Name + " :: Attempting to reopen port");
                    try
                    {
                        serial.Close();
                        Thread.Sleep(250);
                        StartSerial(Name);
                        Send(data);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                        Online = false;
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                Online = false;
                throw;
            }
        }

        private byte[] Receive(bool wait = false)
        {
            try
            { 
                if (wait)
                {
                    while (serial.BytesToRead == 0) Thread.Sleep(10);
                }

                if(serial.ReadByte() == 252)
                {
                    var len = serial.ReadByte();
                    byte[] data = new byte[len];
                    serial.Read(data, 0, len);
                    return data;
                }
                else
                {
                    return new byte[0];
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        /*public bool TryReceive(out byte[] data)
        {
            try
            {
                data = new byte[0];
                if (Settings.ConnectionType == ConnectionType.Serial && serial.BytesToRead > 0)
                {
                    data = Receive();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }*/

        private void StartUDPClient(string ip, int dstPort)
        {
            udp = new UDPCLient(ip, dstPort, port++);
            SetColor(Colors.RED);
        }

        public void StopUDPClient()
        {
            try
            {
                udp.Dispose();
                udp = null;
                Online = false;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
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
                if (Settings.ConnectionType == ConnectionType.Serial)
                {
                    serial.Close();
                    serial.Dispose();
                }
            }

            _disposed = true;
        }
    }

    public class ArduinoCollection : IDisposable, IList<Arduino>
    {
        private readonly List<Arduino> collection = new List<Arduino>();
        private readonly ConcurrentQueue<ChColor> queue;
        private readonly Thread thread;
        //private Color _color = Colors.NONE;
        private readonly Color[] _chColor = new Color[100];
        public int ChannelCount { get; private set; } = 0;

        public event EventHandler<ArduinoExceptionArgs> OnError;

        public ArduinoCollection()
        {
            for (int i = 0; i < _chColor.Length; i++) _chColor[i] = Colors.NONE;

            try
            {
                thread = new Thread(new ThreadStart(Worker));
                queue = new ConcurrentQueue<ChColor>();
                thread.Start();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private void Worker()
        {
            while (true)
            {
                try
                {
                    if (queue.TryDequeue(out ChColor chColor))
                    {
                        if(chColor.Colors != null)
                        {
                            int i = 0;
                            foreach (Arduino arduino in collection)
                            {
                                if (arduino.Online)
                                {
                                    try
                                    {
                                        arduino.SetColor(chColor.Colors.Splice(i, arduino.Channels));
                                    }
                                    catch (Exception e)
                                    {
                                        OnError?.Invoke(this, new ArduinoExceptionArgs(arduino, e));
                                        throw;
                                    }
                                }
                                i += arduino.Channels;
                            }
                        }
                        else
                        {
                            int channel = chColor.Channel;
                            if (channel == 0)
                            {
                                foreach (Arduino arduino in collection)
                                {
                                    if (arduino.Online)
                                    {
                                        try
                                        {
                                            arduino.SetColor(chColor.Color);
                                        }
                                        catch (Exception e)
                                        {
                                            OnError?.Invoke(this, new ArduinoExceptionArgs(arduino, e));
                                            throw;
                                        }
                                    }

                                }
                            }
                            else
                            {

                                foreach (Arduino arduino in collection)
                                {
                                    if (channel - arduino.Channels <= 0)
                                    {
                                        if (arduino.Online)
                                        {
                                            try
                                            {
                                                arduino.SetColor(channel, chColor.Color);
                                            }
                                            catch (Exception e)
                                            {
                                                OnError?.Invoke(this, new ArduinoExceptionArgs(arduino, e));
                                                throw;
                                            }
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        channel -= arduino.Channels;
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(1);
                }
                catch (Exception e)
                {
                    Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
                }   
            }
        }

        public Arduino this[int index]
        {
            get
            {
                return collection[index];
            }
            set
            {
                collection[index] = value;
            }
        }

        public Arduino IndexOf(string name)
        {
            for(int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Name == name) return collection[i];
            }
            return null;
        }

        public int IndexOf(Arduino arduino)
        {
            return collection.IndexOf(arduino);
        }

        public bool Contains(string name)
        {
            return IndexOf(name) != null;
        }

        public bool Contains(Arduino arduino)
        {
            return collection.Contains(arduino);
        }

        public void Add(Arduino arduino)
        {
            try
            {
                if (Contains(arduino)) return;
                collection.Add(arduino);
                ChannelCount += arduino.Channels;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public void Insert(int i, Arduino arduino)
        {
            try
            {
                if (Contains(arduino)) return;
                collection.Insert(i, arduino);
                ChannelCount += arduino.Channels;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public void ShiftUp(Arduino arduino)
        {
            collection.Shift(arduino, -1);
        }

        public void ShiftDown(Arduino arduino)
        {
            collection.Shift(arduino, 1);
        }

        public bool Remove(Arduino arduino)
        {
            if (Contains(arduino))
            {
                collection.Remove(arduino);
                arduino.Dispose();
                CalculateChannels();
                return true;
            }
            return false;
        }

        public void RemoveAt(int i)
        {
            if (i < collection.Count)
            {
                var arduino = collection[i];
                collection.RemoveAt(i);
                arduino.Dispose();
                CalculateChannels();
            }
        }

        public void Clear()
        {
            foreach (Arduino arduino in collection) Remove(arduino);
        }

        private void CalculateChannels()
        {
            var count = 0;
            foreach(var arduino in collection)
            {
                count += arduino.Channels;
            }
            ChannelCount = count;
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

        public void SetColor(int channel, Color color)
        {
            if (channel >= 0 && channel <= ChannelCount && (color != _chColor[channel] || (_chColor[0] != Colors.NONE && channel != 0)))
            {
                _chColor[channel] = color;
                if (channel != 0) _chColor[0] = Colors.NONE;
                queue.Enqueue(new ChColor(channel, color));
            }
        }

        public void SetColor(Color[] colors)
        {
            if (colors.Length != ChannelCount) return;
            queue.Enqueue(new ChColor(colors));
        }

        public int Multiplier
        {
            get
            {
                if (ChannelCount <= 1) return 12;
                else if (ChannelCount == 2) return 6;
                else if (ChannelCount == 3) return 4;
                else if (ChannelCount == 4) return 3;
                else if (ChannelCount >= 5 && ChannelCount <= 8) return 2;
                else return 1;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<Arduino> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        public void CopyTo(Arduino[] array)
        {
            collection.CopyTo(array);
        }

        public void CopyTo(Arduino[] array, int i)
        {
            collection.CopyTo(array, i);
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
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
                foreach (Arduino arduino in collection) arduino.Dispose();
            }

            _disposed = true;
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

    [Serializable]
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

        public Color(System.Drawing.Color color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
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

        public override string ToString()
        {
            return R + ", " + G + ", " + B;
        }

        public System.Drawing.Color ToSystemColor()
        {
            return System.Drawing.Color.FromArgb(R, G, B);
        }

        public static Color operator * (Color left, double right)
        {
            return new Color((int)(left.R * right), (int)(left.G * right), (int)(left.B * right));
        }
    }

    public struct ChColor
    {
        public readonly Color Color;
        public readonly int Channel;
        public readonly Color[] Colors;

        public ChColor(int channel, Color color)
        {
            Color = color;
            Channel = channel;
            Colors = null;
        }

        public ChColor(Color[] colors)
        {
            Color = SAL_Core.Colors.NONE;
            Channel = 0;
            Colors = colors;
        }
    }

    public class ArduinoExceptionArgs
    {
        public Arduino Arduino { get; }
        public Exception Exception { get; }
        public ArduinoExceptionArgs(Arduino arduino, Exception exception)
        {
            Arduino = arduino;
            Exception = exception;
        }
    }

    public enum ConnectionType
    {
        Serial = 0,
        UDP = 1
    }
}
