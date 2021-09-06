using System.Text;
using SAL_Core.Settings;
using SAL_Core.Extensions;
using System;

namespace SAL_Core.IO.Connection
{
    class ConnectionHandler
    {
        private const int arduinobuffersize = 1024;
        private readonly ArduinoSettings Settings;
        private readonly IConnection Connection;

        public bool Online { get; private set; } = false;
        public int Channels { get; private set; }

        public ConnectionHandler(ArduinoSettings settings)
        {
            Settings = settings;

            try
            {
                if (settings.ConnectionType == ConnectionType.Serial)
                {
                    Connection = new Serial(settings.COM);
                }
                else
                {
                    Connection =  new TCP(Settings.IP, Settings.Port);
                }

                Send(1, true); //get Model
                var data = Receive(false, true);
                if (data.Length == 2 && data[0] == 250)
                {
                    Channels = data[1];
                    Online = true;
                }
                else throw new Exception("Failed at getting device Model");
            }
            catch
            {
                throw;
            }
        }

        public void Close()
        {
            Connection.Close();
        }

        public void Send(int command)
        {
            Send(command, false);
        }

        private void Send(int command, bool startup)
        {
            if (command < 0 || command > 255) return;
            byte[] data = new byte[] { (byte)command };
            Send(data, startup);
        }

        public void Send(byte[] data)
        {
            Send(data, false);
        }

        private void Send(byte[] data, bool startup)
        {
            if (!(Online || startup)) return;
            if (data.Length > arduinobuffersize) return;

            try
            {
                byte[] header = { 252, (byte)(data.Length / 256), (byte)(data.Length % 256) }; //not pretty but endian independent
                data = header.Concat(data);
                Connection.Send(data);
            }
            catch
            {
                Online = false;
                throw;
            }
            
        }

        public byte[] Receive(bool wait = false)
        {
            return Receive(wait, false);
        }

        private byte[] Receive(bool wait, bool startup)
        {
            if (!(Online || startup)) return null;

            try
            {
                return Connection.Receive(wait);
            }
            catch
            {
                Online = false;
                throw;
            }
        }

        public string ReceiveString(bool wait = false)
        {
            return Encoding.ASCII.GetString(Receive(wait));
        }
    }
}
