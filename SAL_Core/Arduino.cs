using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Damez.Log;

namespace SAL_Core
{
    public class Arduino : IDisposable
    {
        private const int arduinobuffersize = 1024;
        private Color[] colorCache;

        private SerialPort serial;
        private TcpClient tcp = null;
        private NetworkStream stream = null;

        public event EventHandler<ArduinoExceptionArgs> OnError;

        public string Name { get; private set; }

        public bool Online { get; private set; } = false;

        private int _channels = 0;
        public int Channels {
            get
            {
                return _channels;
            }
            private set
            {
                if(value > 0 && value <= 255)
                {
                    _channels = value;
                    colorCache = new Color[value];
                    colorCache.Populate(Colors.OFF);
                }
            }
        }

        public ArduinoSettings Settings { get; private set; } = null;

        public Arduino(ArduinoSettings settings, bool muteExceptions = false)
        {
            Settings = settings;
            try
            {
                if (settings.ConnectionType == ConnectionType.Serial)
                {
                    Name = settings.COM;
                    StartSerial(settings.COM);
                }
                else
                {
                    Name = Settings.IP + ":" + Settings.Port;
                    StartTCPClient(Settings.IP, Settings.Port);
                }
                SetColor(Colors.RED);
                Show();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                if (!muteExceptions) throw;
            }
        }

        public void StartSerial(string com, bool retry = false, int retryCount = 0)
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
                Send(1, true); //get Model
                var data = Receive();
                if(data.Length == 2 && data[0] == 250)
                {
                    Channels = data[1];
                }     
                else throw new Exception("Failed at getting device Model");
                Online = true;
            }
            catch (TimeoutException)
            {
                if (retryCount < 10 && retry)
                {
                    serial.Close();
                    Thread.Sleep(250);
                    StartSerial(com, true, ++retryCount);
                }
                else
                {
                    Online = false;
                    throw new Exception("Failed at getting device Model");
                }
            }
            catch (Exception)
            {
                Online = false;
                throw;
            }
        }

        private void StartTCPClient(string ip, int dstPort, bool retry = false, int retryCount = 0)
        {
            try
            {
                tcp = new TcpClient(ip, dstPort);
                tcp.NoDelay = true;
                tcp.SendTimeout = 500;
                tcp.ReceiveTimeout = 500;
                if (tcp.Connected)
                {
                    stream = tcp.GetStream();
                    Send(1, true); //get Model
                    var data = Receive();
                    if (data.Length == 2 && data[0] == 250)
                    {
                        Channels = data[1];
                    }         
                    else throw new Exception("Failed at getting device Model");
                }
                else
                    throw new Exception("Connection failed.");
                Online = true;
            }
            catch
            {
                if(retry && retryCount < 10)
                {
                    StartTCPClient(ip, dstPort, retry, ++retryCount);
                }
                else
                {
                    Online = false;
                    throw;
                }
            }
        }

        public void StopTCPClient()
        {
            try
            {
                if(stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if(tcp != null)
                {
                    tcp.Close();
                    tcp.Dispose();
                }
                tcp = null;
                stream = null;
                Online = false;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public override string ToString()
        {
            if (Online)
            {
                if(Settings.Reverse) return Name + " - " + Channels + "CH - Reversed";
                else return Name + " - " + Channels + "CH";
            }
            else return Name + " - Offline";
        }

        public void SetColor(Color color)
        {
            colorCache.Populate(color);
        }

        public void SetColor(int channel, Color color)
        {
            if (channel < 0 || channel > Channels)
            {
                return;
            }

            colorCache[channel - 1] = color;
        }

        public void SetColor(Color[] colors)
        {
            if (colors.Length != Channels) return;

            colorCache = colors;
        }

        /// <summary>
        /// Sends the colorCache to the arduino
        /// </summary>
        public void Show()
        {
            byte[] data = new byte[Channels * 3 + 1];
            data[0] = 99;

            if (Settings.Reverse)
            {
                for (int i = 0; i < Channels; i++)
                {
                    data[i * 3 + 1] = (byte)NormalizeColor(colorCache[Channels - i - 1].R);
                    data[i * 3 + 2] = (byte)NormalizeColor(colorCache[Channels - i - 1].G);
                    data[i * 3 + 3] = (byte)NormalizeColor(colorCache[Channels - i - 1].B);
                }
            }
            else
            {
                for (int i = 0; i < Channels; i++)
                {
                    data[i * 3 + 1] = (byte)NormalizeColor(colorCache[i].R);
                    data[i * 3 + 2] = (byte)NormalizeColor(colorCache[i].G);
                    data[i * 3 + 3] = (byte)NormalizeColor(colorCache[i].B);
                }
            }

            Send(data);
        }

        private int NormalizeColor(int c)
        {
            if (c < 0) c = 0;
            else if (c > 255) c = 255;
            return c;
        }


        private void Send(int command, bool force = false)
        {
            if (command < 0 || command > 255) return;
            byte[] data = new byte[] { (byte)command };
            Send(data, force);
        }

        private void Send(byte[] data, bool force = false)
        {
            if (!(Online || force)) return;
            if (data.Length > arduinobuffersize) return;

            byte[] header = { 252, (byte)(data.Length/256), (byte)(data.Length%256)}; //not pretty but endian independent
            data = header.Concat(data);
            if (Settings.ConnectionType == ConnectionType.TCP)
                SendTCP(data);
            else
                SendSerial(data);
        }

        private void SendSerial(byte[] data)
        {
            try
            {
                serial.Write(data, 0, data.Length);
            }
            catch (TimeoutException e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);

                Log.Write(Log.TYPE_INFO, "Arduino :: " + Name + " :: Attempting to reopen port");
                try
                {
                    serial.Close();
                    Thread.Sleep(250);
                    StartSerial(Settings.COM, true);
                }
                catch (Exception ex)
                {
                    Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                    Online = false;
                    throw;
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                Online = false;
                throw;
            }
        }

        private void SendTCP(byte[] data)
        {
            if(stream == null)
            {
                Online = false;
                return;
            }
            try
            {
                stream.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);

                Log.Write(Log.TYPE_INFO, "Arduino :: " + Name + " :: Attempting to reopen connection");
                try
                {
                    StopTCPClient();
                    StartTCPClient(Settings.IP, Settings.Port, true);
                }
                catch (Exception ex)
                {
                    Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                    OnError?.Invoke(this, new ArduinoExceptionArgs(this, ex));
                }
            }
        }

        private byte[] Receive(bool wait = false)
        {
            if (Settings.ConnectionType == ConnectionType.TCP)
                return ReceiveTCP(wait);
            else
                return ReceiveSerial(wait);
        }

        private byte[] ReceiveSerial(bool wait = false)
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
                    for(int i = 0; i < len; i++)
                    {
                        data[i] = (byte)serial.ReadByte();
                    }
                    return data;
                }
                else
                {
                    serial.DiscardInBuffer();
                    return new byte[0];
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        private byte[] ReceiveTCP(bool wait = false) //This function is absolutely atrocious, but for what is needed it's suitable
        {
            try
            {
                if (ReceiveTCPByte(wait) == 252)
                {
                    var len = ReceiveTCPByte(wait);
                    byte[] data = new byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        data[i] = ReceiveTCPByte(wait);
                    }
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
            catch(Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        private byte ReceiveTCPByte(bool wait = false)
        {
            /// Getto timeout
            var time = DateTime.Now;
            var diff = new TimeSpan(0, 0, 10);
            while (!stream.DataAvailable)
            {
                if ((DateTime.Now - time) > diff && !wait)
                {
                    throw new Exception("E_TCP_TIMEOUT: La conexion TCP excedio el tiempo de espera.");
                }
                Thread.Sleep(20);
            }

            var msg = new byte[1];
            stream.Read(msg, 0, 1);
            return msg[0];
        }

        private string ReceiveString(bool wait = false)
        {
            return Encoding.ASCII.GetString(Receive(wait));
        }

        public List<string> ScanNetworks()
        {
            var result = new List<string>();
            if(Settings.ConnectionType == ConnectionType.Serial) serial.DiscardInBuffer();
            Send(2); //scan command
            int len = Receive(true)[0];
            for(int i = 0; i < len; i++)
            {
                result.Add(ReceiveString(true));
            }

            return result;
        }

        public string GetIPAddress()
        {
            if (Settings.ConnectionType == ConnectionType.Serial) serial.DiscardInBuffer();
            Send(4);
            return ReceiveString(true);
        }

        public string GetMACAddress()
        {
            if (Settings.ConnectionType == ConnectionType.Serial) serial.DiscardInBuffer();
            Send(5);
            return ReceiveString(true);
        }

        public void SetWifi(string SSID, string password, byte[] ip, byte[] gateway, byte[] mask)
        {
            var data = new byte[2];
            data[0] = 3;
            data[1] = (byte)SSID.Length;
            data = data.Concat(Encoding.ASCII.GetBytes(SSID));
            data = data.Concat(new byte[] { (byte)password.Length });
            data = data.Concat(Encoding.ASCII.GetBytes(password));
            data = data.Concat(ip);
            data = data.Concat(gateway);
            data = data.Concat(mask);
            Send(data);
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
                else
                {
                    StopTCPClient();
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

        public int ChannelCount { get; private set; } = 0;

        public bool Enabled { get; set; } = true;

        public event EventHandler<ArduinoExceptionArgs> OnError;

        public ArduinoCollection()
        {
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
                    while (queue.TryDequeue(out ChColor chColor))
                    {
                        if(chColor.Colors != null)
                        {
                            int i = 0;
                            foreach (Arduino arduino in collection)
                            {
                                if (arduino.Online)
                                {
                                    arduino.SetColor(chColor.Colors.Splice(i, arduino.Channels));
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
                                        arduino.SetColor(chColor.Color);
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
                                            arduino.SetColor(channel, chColor.Color);
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
                    Parallel.ForEach(collection, arduino => 
                    {
                        try
                        {
                            arduino.Show();
                        }
                        catch (Exception e)
                        {
                            OnError?.Invoke(this, new ArduinoExceptionArgs(arduino, e));
                            throw;
                        }
                    });
                    Thread.Sleep(15);
                }
                catch (Exception e)
                {
                    Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
                }   
            }
        }

        
        public void TurnOff()
        {
            Enabled = false;
            while (!queue.IsEmpty) queue.TryDequeue(out _);

            foreach (Arduino arduino in collection)
            {
                if (arduino.Online)
                {
                    try
                    {
                        arduino.SetColor(Colors.OFF);
                        arduino.Show();
                    }
                    catch (Exception e)
                    {
                        Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
                    }
                }
            }

            Thread.Sleep(50);
            Enabled = true;
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
                CalculateChannels();
                arduino.OnError += Arduino_OnError;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "ArduinoCollection :: " + e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private void Arduino_OnError(object sender, ArduinoExceptionArgs e)
        {
            CalculateChannels();
            OnError?.Invoke(this, new ArduinoExceptionArgs(e.Arduino, e.Exception));
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
                if(arduino.Online) count += arduino.Channels;
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
            if (!Enabled) return;

            queue.Enqueue(new ChColor(0, color));
        }

        public void SetColor(int channel, Color color)
        {
            if (!Enabled) return;
            if (channel >= 0 && channel <= ChannelCount)
            {
                queue.Enqueue(new ChColor(channel, color));
            }
        }

        public void SetColor(Color[] colors)
        {
            if (!Enabled) return;
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
                thread.Abort();
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
        public static Color ORANGE { get; } = new Color(255, 65, 0);
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
        TCP = 1
    }
}
