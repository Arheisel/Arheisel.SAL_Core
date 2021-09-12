using System;

namespace SAL_Core.Settings
{
    [Serializable]
    public class AudioSettings
    {
        public int Slope { get; set; } = 25;

        public AutoscalerSettings Autoscaler { get; } = new AutoscalerSettings();

        public EqualizerSettings Equalizer { get; } = new EqualizerSettings();

        public int MinFreq { get; set; } = 20;

        public int MaxFreq { get; set; } = 10000;

    }
}
