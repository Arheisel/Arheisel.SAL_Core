using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAL_Core
{
    [Serializable]
    public class Settings
    {
        public EffectPreset Effects { get; set; } = new EffectPreset();
    }

    [Serializable]
    public class EffectSettings
    {
        public string Current { get; set; } = "Rainbow";

        public Dictionary<string, EffectPreset> PresetList { get; } = new Dictionary<string, EffectPreset>()
        {
            {"Rainbow", EffectPresetDefaults.Rainbow },
            {"Cycle", EffectPresetDefaults.Cycle },
            {"Breathing", EffectPresetDefaults.Breathing },
            {"Flash", EffectPresetDefaults.Flash }
        };

        public EffectPreset CurrentPreset
        {
            get
            {
                return PresetList[Current];
            }
        }

    }

    [Serializable]
    public class EffectPreset
    {
        public EffectTypes Type { get; set; } = EffectTypes.Rainbow;
        public int Speed { get; set; } = 0;
        public int TotalSteps { get; set; } = 255;
        public int HoldingSteps { get; set; } = 50;
        public List<Color> ColorList { get; set; } = new List<Color>();

    }

    public enum EffectTypes
    {
        Rainbow = 0,
        Cycle = 1,
        Breathing = 2,
        Flash = 3
    }

    public static class EffectPresetDefaults
    {
        public static EffectPreset Rainbow
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Rainbow,
                    Speed = 0,
                    TotalSteps = 20,
                    HoldingSteps = 0,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
        public static EffectPreset Cycle
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Cycle,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
        public static EffectPreset Breathing
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Breathing,
                    ColorList = new List<Color> { Colors.RED, Colors.GREEN, Colors.BLUE, Colors.YELLOW, Colors.MAGENTA, Colors.CYAN }
                };
            }
        }
        public static EffectPreset Flash
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Flash,
                    TotalSteps = 20,
                    HoldingSteps = 2,
                    ColorList = new List<Color> { Colors.WHITE }
                };
            }
        }

    }
}
