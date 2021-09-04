using System;


namespace SAL_Core.RGB
{
    [Serializable]
    public struct Color
    {
        public readonly int R;
        public readonly int G;
        public readonly int B;

        public Color(int R, int G, int B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public Color(System.Drawing.Color color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public static bool operator ==(Color c1, Color c2)
        {
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }

        public static bool operator !=(Color c1, Color c2)
        {
            return !(c1 == c2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return R + ", " + G + ", " + B;
        }

        public System.Drawing.Color ToSystemColor()
        {
            return System.Drawing.Color.FromArgb(R, G, B);
        }

        public static Color operator *(Color left, double right)
        {
            return new Color((int)(left.R * right), (int)(left.G * right), (int)(left.B * right));
        }
    }
}
