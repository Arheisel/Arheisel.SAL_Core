using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System.Collections.Generic;


namespace SAL_Core.Ambient.Types
{
    class Breathing : Effect
    {
        private Transition transition;
        private bool mode = true;
        public Breathing(IChannelGroup group, EffectPreset settings) : base(group, settings) { }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (Group.ChannelCount == 0) return colors;
            if (step == 0)
            {
                if (mode)
                {
                    transition = new Transition(Colors.OFF, Preset.ColorList[count], Preset.TotalSteps);
                    colors.Add(new ChColor(0, Colors.OFF));

                    mode = false;
                }
                else
                {
                    transition = new Transition(Preset.ColorList[count], Colors.OFF, Preset.TotalSteps);
                    colors.Add(new ChColor(0, Preset.ColorList[count]));

                    if (count >= Preset.ColorList.Count - 1) count = 0;
                    else count++;

                    mode = true;
                }
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
