using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Arheisel.Log;
using SAL_Core.Extensions;
using SAL_Core.RGB;
using SAL_Core.Settings;

namespace SAL_Core.IO
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


        private void Send(int command, bool startup = false)
        {
            if (command < 0 || command > 255) return;
            byte[] data = new byte[] { (byte)command };
            Send(data, startup);
        }

        private void Send(byte[] data, bool startup = false)
        {
            if (!(Online || startup)) return;
            if (data.Length > arduinobuffersize) return;

            byte[] header = { 252, (byte)(data.Length/256), (byte)(data.Length%256)}; //not pretty but endian independent
            data = header.Concat(data);
            if (Settings.ConnectionType == ConnectionType.TCP)
                SendTCP(data, startup);
            else
                SendSerial(data, startup);
        }

        private void SendSerial(byte[] data, bool startup = false)
        {
            try
            {
                serial.Write(data, 0, data.Length);
            }
            catch (TimeoutException e)
            {
                if (startup) throw;

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
                if (startup) throw;

                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                Online = false;
                throw;
            }
        }

        private void SendTCP(byte[] data, bool startup = false)
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
                if (startup) throw;

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
}
