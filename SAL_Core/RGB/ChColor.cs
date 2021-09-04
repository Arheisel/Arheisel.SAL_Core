

namespace SAL_Core.RGB
{
    public struct ChColor
    {
        public readonly Color Color;
        public readonly int Channel;
        public readonly Color[] Colors;

        public ChColor(int channel, Color color)
        {
            Color = color;
            Channel = channel;
            Colors = null;
        }

        public ChColor(Color[] colors)
        {
            Color = RGB.Colors.NONE;
            Channel = 0;
            Colors = colors;
        }
    }
}
