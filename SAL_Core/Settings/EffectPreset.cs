using SAL_Core.RGB;
using SAL_Core.Ambient;
using System;
using System.Collections.Generic;


namespace SAL_Core.Settings
{
    [Serializable]
    public class EffectPreset
    {
        public EffectTypes Type { get; set; } = EffectTypes.Static;
        public int Speed { get; set; } = 1;
        public int TotalSteps { get; set; } = 255;
        public int HoldingSteps { get; set; } = 50;
        public bool Reverse { get; set; } = false;
        public List<Color> ColorList { get; set; } = new List<Color>() { Colors.RED };

    }
}
