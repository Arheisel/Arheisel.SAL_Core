using System;
using System.Collections.Generic;
using System.Text;
using Arheisel.Log;
using SAL_Core.Extensions;
using SAL_Core.IO.Connection;
using SAL_Core.RGB;
using SAL_Core.Settings;

namespace SAL_Core.IO
{
    public class Arduino : IDisposable, IEquatable<Arduino>
    {
        private Color[] colorCache;
        private readonly ConnectionHandler Connection;

        public string Name { 
            get
            {
                return Settings.Name;
            }
        }

        public bool Online { 
            get 
            {
                if (Connection != null)
                    return Connection.Online;
                else
                    return false;
            }
        }

        private int _channels = 0;
        public int Channels {
            get
            {
                if (Online)
                    return _channels;
                else
                    return 0;
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

        public ArduinoSettings Settings { get; }

        public Arduino(ArduinoSettings settings, bool muteExceptions = false)
        {
            Settings = settings;
            try
            {
                Connection = new ConnectionHandler(settings);
                Channels = Connection.Channels;
                SetColor(Colors.RED);
                Show();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Arduino :: " + Name + " :: " + e.Message + Environment.NewLine + e.StackTrace);
                if (!muteExceptions)
                    throw;
                else
                    Connection = null;
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
            if (channel <= 0 || channel > Channels)
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

        public void SetColor(Color[] colors, int start)
        {
            if (start < 1 || start + colors.Length - 1 > Channels) return;

            Array.Copy(colors, 0, colorCache, start - 1, colors.Length);
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

            Connection.Send(data);
        }

        private int NormalizeColor(int c)
        {
            if (c < 0) c = 0;
            else if (c > 255) c = 255;
            return c;
        }


        public List<string> ScanNetworks()
        {
            var result = new List<string>();
            //if(Settings.ConnectionType == ConnectionType.Serial) serial.DiscardInBuffer();
            Connection.Send(2); //scan command
            int len = Connection.Receive(true)[0];
            for(int i = 0; i < len; i++)
            {
                result.Add(Connection.ReceiveString(true));
            }

            return result;
        }

        public string GetIPAddress()
        {
            //if (Settings.ConnectionType == ConnectionType.Serial) serial.DiscardInBuffer();
            Connection.Send(4);
            return Connection.ReceiveString(true);
        }

        public string GetMACAddress()
        {
            //if (Settings.ConnectionType == ConnectionType.Serial) serial.DiscardInBuffer();
            Connection.Send(5);
            return Connection.ReceiveString(true);
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
            Connection.Send(data);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Arduino);
        }

        public bool Equals(Arduino other)
        {
            return other != null &&
                   EqualityComparer<ArduinoSettings>.Default.Equals(Settings, other.Settings);
        }

        public override int GetHashCode()
        {
            return -2113213080 + EqualityComparer<ArduinoSettings>.Default.GetHashCode(Settings);
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
                if(Connection != null) Connection.Close();
            }

            _disposed = true;
        }
    }    
}
