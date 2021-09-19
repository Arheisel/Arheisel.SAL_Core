using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.RGB;
using SAL_Core.Settings;

namespace SAL_Core.Modes
{
    public class Musicbar3Mode : BaseMode
    {
        public Musicbar3Mode(ArduinoCollection collection, AudioSettings settings) : base(collection, settings) { }

        protected override void OnDataAvailable(AudioDataAvailableArgs e)
        {
            /*
            int start = 0;
            foreach(var group in Group.Groups)
            {
                double peak = 0;
                for(int i = 0; i < group.ChannelCount; i++)
                {
                    if (e.ChannelMagnitudes[i + start] > peak) peak = e.ChannelMagnitudes[i + start];
                }

                double div = 1.0 / (double)group.ChannelCount;
                var colors = new Color[group.ChannelCount];
                for (int i = 0; i < group.ChannelCount; i++)
                {
                    if (peak > div * i) colors[i] = Maps.EncodeRGB(peak >= div * (i + 1) ? div * (i + 1) : peak);
                    else colors[i] = Colors.OFF;
                }
                group.SetColor(colors);
                start += group.ChannelCount;
            }
            */
        }
    }
}
