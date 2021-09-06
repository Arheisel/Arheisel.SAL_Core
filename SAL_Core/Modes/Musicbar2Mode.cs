using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.RGB;
using SAL_Core.Settings;

namespace SAL_Core.Modes
{
    public class Musicbar2Mode : BaseMode
    {
        public Musicbar2Mode(ArduinoCollection collection, AudioSettings settings) : base(collection, settings) { }

        protected override void OnDataAvailable(AudioDataAvailableArgs e)
        {
            double div = 1.0 / (double)Collection.ChannelCount;
            var colors = new Color[Collection.ChannelCount];
            for (int i = 0; i < Collection.ChannelCount; i++)
            {
                if (e.Peak > div * i) colors[i] = Maps.EncodeRGB(e.Peak >= div * (i + 1) ? div * (i + 1) : e.Peak);
                else colors[i] = Colors.OFF;
            }
            Collection.SetColor(colors);
        }
    }
}
