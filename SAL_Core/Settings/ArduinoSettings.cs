using SAL_Core.IO.Connection;
using System;
using System.Collections.Generic;

namespace SAL_Core.Settings
{
    [Serializable]
    public class ArduinoSettings : IEquatable<ArduinoSettings>
    {
        public string Name { get; set; } = "Glow";

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

        public override bool Equals(object obj)
        {
            return Equals(obj as ArduinoSettings);
        }

        public bool Equals(ArduinoSettings other)
        {
            return other != null &&
                   ConnectionType == other.ConnectionType &&
                   COM == other.COM &&
                   IP == other.IP &&
                   Port == other.Port;
        }

        public override int GetHashCode()
        {
            int hashCode = -373179799;
            hashCode = hashCode * -1521134295 + ConnectionType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(COM);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(IP);
            hashCode = hashCode * -1521134295 + Port.GetHashCode();
            return hashCode;
        }
    }
}
