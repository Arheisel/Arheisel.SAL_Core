using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Extensions;
using SAL_Core.Settings;
using System.Collections.Generic;


namespace SAL_Core.Ambient.Types
{
    class Sweep : Effect
    {
        private int currentChannel = 1;
        private Transition transition;
        public Sweep(EffectPreset settings, int channelCount) : base(settings, channelCount) { }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (ChannelCount == 0) return colors;
            if (step == 0)
            {
                if (currentChannel == 1)
                {
                    transition = new Transition(Preset.ColorList[count], Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)], Preset.TotalSteps);
                    colors.Add(new ChColor(0, Preset.ColorList[count]));
                    if (count >= Preset.ColorList.Count - 1) count = 0;
                    else count++;
                }

                step++;
            }
            else
            {
                if (currentChannel >= ChannelCount)
                {
                    if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                    {
                        currentChannel = 1;
                        step = 0;
                        return colors;
                    }
                }
                else
                {
                    if (step >= Preset.TotalSteps)
                    {
                        currentChannel++;
                        step = 0;
                        return colors;
                    }
                }


                if (step < Preset.TotalSteps)
                    if (Preset.Reverse)
                        colors.Add(new ChColor((ChannelCount + 1) - currentChannel, transition.getColor(step)));
                    else
                        colors.Add(new ChColor(currentChannel, transition.getColor(step)));

                step++;
            }
            return colors;
        }
    }
}
