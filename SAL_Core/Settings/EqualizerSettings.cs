using System;

namespace SAL_Core.Settings
{
    [Serializable]
    public class EqualizerSettings
    {
        public double High { get; set; } = 1;
        public double Mid { get; set; } = 1;
        public double Low { get; set; } = 1;
    }
}
