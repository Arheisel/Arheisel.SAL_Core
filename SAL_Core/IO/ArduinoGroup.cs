using SAL_Core.RGB;
using SAL_Core.Extensions;

namespace SAL_Core.IO
{
    public class ArduinoGroup
    {
        private ArduinoCollection collection;
        private int Start;
        public int Length { get; private set; }

        public ArduinoGroup(ArduinoCollection arduinoCollection, int start, int length)
        {
            collection = arduinoCollection;
            Start = start;
            Length = length;
        }

        public void SetColor(int channel, Color color)
        {
            if (channel <= 0 || channel > Length) return;

            if(channel == 0)
            {
                SetColor(color);
                return;
            }

            collection.SetColor(Start + channel - 1, color);
        }

        public void SetColor(Color color)
        {
            var colors = new Color[Length];
            colors.Populate(color);
            SetColor(colors);
        }

        public void SetColor(Color[] colors)
        {
            if (colors.Length != Length) return;
            collection.SetColor(colors, Start);
        }
    }
}
