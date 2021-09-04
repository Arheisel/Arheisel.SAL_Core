using System;
using System.Collections.Generic;
using System.Linq;

namespace SAL_Core.Settings
{

    [Serializable]
    public class EffectSettings
    {
        public string Current { get; set; } = "Rainbow";

        public Dictionary<string, EffectPreset> PresetList { get; set; } = new Dictionary<string, EffectPreset>()
        {
            {"Rainbow", EffectPresetDefaults.Rainbow },
            {"Cycle", EffectPresetDefaults.Cycle },
            {"Breathing", EffectPresetDefaults.Breathing },
            {"Flash", EffectPresetDefaults.Flash },
            {"Fire", EffectPresetDefaults.Fire },
            {"Static", EffectPresetDefaults.Static },
            {"Sweep", EffectPresetDefaults.Sweep },
            {"Load", EffectPresetDefaults.Load },
            {"Beam", EffectPresetDefaults.Beam }
        };

        public EffectPreset CurrentPreset
        {
            get
            {
                if (!PresetList.ContainsKey(Current))
                {
                    Current = PresetList.First().Key;
                }
                return PresetList[Current];
            }
        }

    }
}
