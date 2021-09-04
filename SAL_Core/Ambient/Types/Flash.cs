using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System.Collections.Generic;


namespace SAL_Core.Ambient.Types
{
    class Flash : Effect
    {
        private Color color;
        public Flash(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            if (step == 0)
            {
                color = Preset.ColorList[count];
                colors.Add(new ChColor(0, Colors.OFF));

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

                if (step > Preset.TotalSteps)
                    colors.Add(new ChColor(0, color));

                step++;
            }
            return colors;
        }
    }
}
