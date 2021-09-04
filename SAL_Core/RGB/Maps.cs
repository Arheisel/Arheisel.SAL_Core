using System;
using System.Collections.Generic;


namespace SAL_Core.RGB
{
    public static class Maps
    {
        private static readonly List<Color[]> list = new List<Color[]>()
        {
            new Color[]
            {
                Colors.OFF,
                Colors.LYME,
                Colors.AQGREEN,
                Colors.CYAN,
                Colors.EBLUE,
                Colors.BLUE,
                Colors.PURPLE,
                Colors.MAGENTA,
                Colors.PINK,
                Colors.YELLOW,
                Colors.ORANGE,
                Colors.RED
            },
            new Color[]
            {
                Colors.OFF,
                Colors.WHITE
            }
        };

        private static uint _Current = 0;

        public static uint Current
        {
            get
            {
                return _Current;
            }
            set
            {
                if (value >= 0 && value <= list.Count - 1)
                {
                    _Current = value;
                    MaxIndex = list[(int)value].Length - 1;
                }
            }
        }

        /// <summary>
        /// Maximum index of the Map property
        /// </summary>
        public static int MaxIndex { get; private set; } = list[0].Length - 1;

        public static Color[] Map
        {
            get
            {
                return list[(int)_Current];
            }
        }

        public static Color EncodeRGB(double num)
        {
            if (num <= 0) return new Color(0, 0, 0);
            else if (num >= 1) return new Color(255, 0, 0);

            double y = 0;

            int R;
            if (num < 0.5) R = 0;
            else
            {
                y = 2.0 * num - 1.0;
                R = (int)Math.Round(y * 255.0);
            }

            int G;
            if (num < 0.4)
            {
                y = 2.5 * num;
            }
            else
            {
                y = -5 * num + 3;
            }
            G = (int)Math.Round(y * 255.0);

            int B;
            if (num < 0.5)
            {
                y = 2.5 * num - 0.25;
            }
            else
            {
                y = -2.5 * num + 2.25;
            }
            B = (int)Math.Round(y * 255.0);

            return new Color(R, G, B);
        }
    }
}
