using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAL_Core.Settings
{
    [Serializable]
    public class ArduinoCollectionSettings
    {
        public List<ArduinoSettings> Arduinos { get; } = new List<ArduinoSettings>();
        public List<ArduinoGroupSettings> Groups { get; } = new List<ArduinoGroupSettings>();
    }
}
