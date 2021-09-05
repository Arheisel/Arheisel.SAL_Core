using SAL_Core.IO.Connection;
using System;

namespace SAL_Core.Settings
{
    [Serializable]
    public class ArduinoSettings
    {
        public ConnectionType ConnectionType { get; set; } = ConnectionType.Serial;

        public string COM { get; set; } = string.Empty;

        public string IP { get; set; } = string.Empty;

        public int Port { get; set; } = 0;

        public bool Reverse { get; set; } = false;

        public ArduinoSettings(string com)
        {
            ConnectionType = ConnectionType.Serial;
            COM = com;
        }

        public ArduinoSettings(string ip, int port)
        {
            ConnectionType = ConnectionType.TCP;
            IP = ip;
            Port = port;
        }
    }
}
