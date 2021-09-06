using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System.Collections.Generic;

namespace SAL_Core.Modes
{
    public class MusicbarMode : BaseMode
    {
        private readonly List<Color> ColorList;
        private int current = 0;
        private bool currentSet = false;

        public MusicbarMode(ArduinoCollection collection, AudioSettings settings) : base(collection, settings)
        {
            ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.GREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK };
        }

        protected override void OnDataAvailable(AudioDataAvailableArgs e)
        {
            if (e.Peak >= 0.95 && !currentSet)
            {
                if (current >= ColorList.Count - 1) current = 0;
                else current++;
                currentSet = true;
            }
            if (e.Peak < 0.90 && currentSet) currentSet = false;

            double div = 1.0 / (double)Collection.ChannelCount;
            var colors = new Color[Collection.ChannelCount];
            for (int i = 0; i < Collection.ChannelCount; i++)
            {
                if (e.Peak > div * i) colors[i] = ColorList[current] * e.Peak;
                else colors[i] = Colors.OFF;
            }
            Collection.SetColor(colors);
        }
    }
}
