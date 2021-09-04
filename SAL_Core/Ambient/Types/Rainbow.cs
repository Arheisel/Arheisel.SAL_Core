using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Extensions;
using SAL_Core.Settings;
using System.Collections.Generic;


namespace SAL_Core.Ambient.Types
{
    class Rainbow : Effect
    {
        private int channels = 0;
        private readonly Transition[] transitions = new Transition[100];
        private int stage = 0;
        private int startPoint = 0;
        public Rainbow(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;

            if (step == 0)
            {
                channels = arduino.ChannelCount;
                count = startPoint;
                for (int i = 0; i < channels; i++)
                {
                    if ((i.Even() && stage == 0) || (i.Odd() && stage == 1))
                    {
                        if (Preset.Reverse)
                            colors.Add(new ChColor(channels - i, Preset.ColorList[count.Mod(Preset.ColorList.Count)]));
                        else
                            colors.Add(new ChColor(i + 1, Preset.ColorList[count.Mod(Preset.ColorList.Count)]));
                    }
                    else
                    {
                        transitions[i] = new Transition(Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)], Preset.ColorList[count.Mod(Preset.ColorList.Count)], Preset.TotalSteps);
                        if (Preset.Reverse)
                            colors.Add(new ChColor(channels - i, Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)]));
                        else
                            colors.Add(new ChColor(i + 1, Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)]));
                        if (count >= Preset.ColorList.Count - 1) count = 0;
                        else count++;
                    }
                }
                if (stage == 0)
                {
                    if (startPoint <= 0) startPoint = Preset.ColorList.Count - 1;
                    else startPoint--;
                }
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    if (stage == 1) stage = 0;
                    else stage = 1;
                    return colors;
                }

                if (step < Preset.TotalSteps)
                {
                    for (int i = 0; i < channels; i++)
                    {
                        if ((i.Even() && stage == 1) || (i.Odd() && stage == 0))
                        {
                            if (Preset.Reverse)
                                colors.Add(new ChColor(channels - i, transitions[i].getColor(step)));
                            else
                                colors.Add(new ChColor(i + 1, transitions[i].getColor(step)));
                        }
                    }
                }

                step++;
            }
            return colors;
        }

        public override void Reset()
        {
            startPoint = 0;
            stage = 0;
            base.Reset();
        }
    }
}
