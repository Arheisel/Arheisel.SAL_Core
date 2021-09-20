using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System;
using System.Collections.Generic;

namespace SAL_Core.Ambient.Types
{
    class Fire : Effect
    {
        private Transition transition;
        private Random random;
        public Fire(EffectPreset settings, int channelCount) : base(settings, channelCount)
        {
            random = new Random();
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (ChannelCount == 0) return colors;
            if (step == 0)
            {
                transition = new Transition(Preset.ColorList[0], Preset.ColorList[1], Preset.TotalSteps); ;

                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return colors;
                }

                if (step > Preset.TotalSteps)
                    colors.Add(new ChColor(0, transition.getColor(random.Next(0, Preset.TotalSteps))));
                else
                    colors.Add(new ChColor(0, transition.getColor(random.Next(0, Preset.TotalSteps / 2))));

                step++;
            }
            return colors;
        }
    }
}
