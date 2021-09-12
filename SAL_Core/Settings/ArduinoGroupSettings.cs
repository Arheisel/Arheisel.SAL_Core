using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAL_Core.Settings
{
    [Serializable]
    public class ArduinoGroupSettings
    {
        public int Start { get; set; } = 0;
        public int Length { get; set; } = 0;
    }
}
