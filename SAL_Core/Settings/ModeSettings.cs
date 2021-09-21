using System;
using SAL_Core.Modes;

namespace SAL_Core.Settings
{
    [Serializable]
    public class ModeSettings
    {
        public ModeEnum Mode = ModeEnum.Effects;

        public string CurrentEffect { get; set; } = EffectPresetDefaults.DEFAULTPRESET;
        public AudioSettings AudioSettings { get; } = new AudioSettings();
    }
}
