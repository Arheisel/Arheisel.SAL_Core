using System;

namespace SAL_Core.Settings
{
    [Serializable]
    public class AutoscalerSettings
    {
        public bool Enabled { get; set; } = true;

        public double Scale { get; set; } = 1.0;

        public int Amp { get; set; } = 1;
    }
}
