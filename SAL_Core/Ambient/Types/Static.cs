using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System.Collections.Generic;


namespace SAL_Core.Ambient.Types
{
    class Static : Effect
    {
        public Static(IChannelGroup group, EffectPreset settings) : base(group, settings) { }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (Group.ChannelCount == 0) return colors;
            if (step == 0)
            {
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

                step++;
            }
            return colors;
        }
    }
}
