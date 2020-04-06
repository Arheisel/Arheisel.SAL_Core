using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SAL_Core
{
    public class Effects
    {
        private string _current = "Cycle";
        private Timer timer;
        private ArduinoCollection arduinoCollection;

        public int Count = 0;

        public string Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (list.ContainsKey(value))
                {
                    _current = value;
                    Count = 0;
                }
            }
        }

        

        public readonly Dictionary<string, Color[]> list = new Dictionary<string, Color[]>()
        {
            {"Cycle", new Color[] { Colors.RED, Colors.GREEN, Colors.BLUE } },
            {"Extended Cycle", new Color[] { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK } },
            {"Color Flash", new Color[]
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
            } },
            {"Flash", new Color[] { Colors.WHITE, Colors.OFF, Colors.OFF, Colors.OFF, Colors.OFF } }
        };

        private int _speed = 0;

        public int Speed
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
                    Time = 101 - value;
                    timer.Enabled = value != 0;
                    timer.Interval = Time;
                }
            }
        }

        public int Time { get; private set; } = 100;

        public Color[] Effect
        {
            get
            {
                return list[_current];
            }
        }

        public Effects(ArduinoCollection arduino)
        {
            timer = new Timer()
            {
                Enabled = false,
                Interval = Time,
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;
            arduinoCollection = arduino;
        }

        private Color _currentColor = Colors.OFF;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var target = Effect[Count];

            if(target == _currentColor)
            {
                if (Count == Effect.Length - 1) Count = 0;
                else Count++;
                target = Effect[Count];
            }
            var R = _currentColor.R;
            var G = _currentColor.G;
            var B = _currentColor.B;

            if (target.R > R) R++;
            else if (target.R < R) R--;
            if (target.G > G) G++;
            else if (target.G < G) G--;
            if (target.B > B) B++;
            else if (target.B < B) B--;

            _currentColor = new Color(R, G, B);

            arduinoCollection.SetColor(_currentColor);
            
        }
    }
}
