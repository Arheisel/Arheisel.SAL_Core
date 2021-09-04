using SAL_Core.RGB;
using SAL_Core.Ambient;
using System.Collections.Generic;


namespace SAL_Core.Settings
{
    public static class EffectPresetDefaults
    {
        public static EffectPreset Rainbow
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Rainbow,
                    Speed = 50,
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
                    Speed = 50,
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
                    Speed = 50,
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
                    Speed = 50,
                    TotalSteps = 20,
                    HoldingSteps = 2,
                    ColorList = new List<Color> { Colors.WHITE }
                };
            }
        }

        public static EffectPreset Fire
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Fire,
                    Speed = 50,
                    TotalSteps = 255,
                    HoldingSteps = 100,
                    ColorList = new List<Color> { Colors.ORANGE, Colors.RED }
                };
            }
        }

        public static EffectPreset Static
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Static,
                    Speed = 1,
                    ColorList = new List<Color> { Colors.PURPLE }
                };
            }
        }

        public static EffectPreset Sweep
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Sweep,
                    Speed = 50,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
        public static EffectPreset Load
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Load,
                    Speed = 50,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
        public static EffectPreset Beam
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Beam,
                    Speed = 50,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
    }
}
