using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System.Collections.Generic;

namespace SAL_Core.Ambient.Types
{
    class Beam : Effect
    {
        private int currentChannel = 1;
        private Transition transition;
        private Transition transitionOff;
        public Beam(EffectPreset settings, int channelCount) : base(settings, channelCount) { }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (ChannelCount == 0) return colors;
            if (step == 0)
            {
                if (currentChannel == 1)
                {
                    transition = new Transition(Colors.OFF, Preset.ColorList[count], Preset.TotalSteps);
                    transitionOff = new Transition(Preset.ColorList[count], Colors.OFF, Preset.TotalSteps);
                    colors.Add(new ChColor(0, Colors.OFF));
                    if (count >= Preset.ColorList.Count - 1) count = 0;
                    else count++;
                }

                step++;
            }
            else
            {
                if (currentChannel >= ChannelCount + 1)
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
                {
                    if (Preset.Reverse)
                    {
                        if (currentChannel <= ChannelCount)
                            colors.Add(new ChColor((ChannelCount + 1) - currentChannel, transition.getColor(step)));
                        if (currentChannel > 1)
                            colors.Add(new ChColor((ChannelCount + 2) - currentChannel, transitionOff.getColor(step)));
                    }
                    else
                    {
                        if (currentChannel <= ChannelCount)
                            colors.Add(new ChColor(currentChannel, transition.getColor(step)));
                        if (currentChannel > 1)
                            colors.Add(new ChColor(currentChannel - 1, transitionOff.getColor(step)));

                    }
                }


                step++;
            }
            return colors;
        }
    }
}
