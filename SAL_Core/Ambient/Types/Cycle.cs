﻿using SAL_Core.Extensions;
using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System.Collections.Generic;


namespace SAL_Core.Ambient.Types
{
    class Cycle : Effect
    {
        private Transition transition;
        public Cycle(EffectPreset settings, int channelCount) : base(settings, channelCount) { }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (ChannelCount == 0) return colors;
            if (step == 0)
            {
                transition = new Transition(Preset.ColorList[count], Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)], Preset.TotalSteps);
                colors.Add(new ChColor(0, Preset.ColorList[count]));

                if (count >= Preset.ColorList.Count - 1) count = 0;
                else count++;
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return colors;
                }

                if (step < Preset.TotalSteps)
                    colors.Add(new ChColor(0, transition.getColor(step)));

                step++;
            }
            return colors;
        }
    }
}
