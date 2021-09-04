using System;

namespace SAL_Core.Settings
{
    [Serializable]
    public class AudioSettings
    {
        public int Slope { get; set; } = 10;

        public AutoscalerSettings Autoscaler { get; set; } = new AutoscalerSettings();

        public int MinFreq { get; set; } = 50;

        public int MaxFreq { get; set; } = 4100;

    }
}
