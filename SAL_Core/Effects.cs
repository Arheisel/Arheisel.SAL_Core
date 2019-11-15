using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAL_Core
{
    class Effects
    {
        private static int _current = 0;

        public static int Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (value >= 0 && value <= list.Count - 1)
                {
                    _current = value;
                    count = 0;
                }
            }
        }

        public static int count = 0;

        private static readonly List<Colors[]> list = new List<Colors[]>()
        {
            new Colors[] { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK },
            new Colors[]
            {
                Colors.RED,
                Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,
                Colors.GREEN,
                Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,
                Colors.BLUE,
                Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,
                Colors.YELLOW,
                Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,
                Colors.MAGENTA,
                Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,
                Colors.CYAN,
                Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,Colors.OFF,
            },
            new Colors[] { Colors.WHITE, Colors.OFF, Colors.OFF, Colors.OFF, Colors.OFF }
        };

        private static int _speed = 0;

        public static int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _speed = value;
                    Time = 1515 - _speed * 15;
                }
            }
        }

        public static int Time { get; private set; } = 1515;

        public static Colors[] Effect
        {
            get
            {
                return list[_current];
            }
        }
    }
}
