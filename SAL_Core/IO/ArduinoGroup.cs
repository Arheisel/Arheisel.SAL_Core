using SAL_Core.RGB;
using SAL_Core.Extensions;

namespace SAL_Core.IO
{
    public class ArduinoGroup : IChannelGroup
    {
        private ArduinoCollection collection;
        private int Start;
        public int ChannelCount { get; private set; }
        public int Multiplier { get; private set; }

        public ArduinoGroup(ArduinoCollection arduinoCollection, int start, int length)
        {
            collection = arduinoCollection;
            Start = start;
            ChannelCount = length;

            if (ChannelCount <= 1) Multiplier = 12;
            else if (ChannelCount == 2) Multiplier = 6;
            else if (ChannelCount == 3) Multiplier = 4;
            else if (ChannelCount == 4) Multiplier = 3;
            else if (ChannelCount >= 5 && ChannelCount <= 8) Multiplier = 2;
            else Multiplier = 1;
        }

        public void SetColor(int channel, Color color)
        {
            if (channel <= 0 || channel > ChannelCount) return;

            if(channel == 0)
            {
                SetColor(color);
                return;
            }

            collection.SetColor(Start + channel - 1, color);
        }

        public void SetColor(Color color)
        {
            var colors = new Color[ChannelCount];
            colors.Populate(color);
            SetColor(colors);
        }

        public void SetColor(Color[] colors)
        {
            if (colors.Length != ChannelCount) return;
            collection.SetColor(colors, Start);
        }
    }
}
