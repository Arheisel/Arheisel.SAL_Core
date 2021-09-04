using System;


namespace SAL_Core.Settings
{
    [Serializable]
    public class MusicSettings
    {
        public int Slope { get; set; } = 10;

        public AutoscalerSettings Autoscaler { get; set; } = new AutoscalerSettings();
    }
}
