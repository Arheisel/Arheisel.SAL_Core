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

        

        public readonly Dictionary<string, Colors[]> list = new Dictionary<string, Colors[]>()
        {
            {"Cycle", new Colors[] { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK } },
            {"Color Flash", new Colors[]
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
            {"Flash", new Colors[] { Colors.WHITE, Colors.OFF, Colors.OFF, Colors.OFF, Colors.OFF } }
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
                    Time = 1515 - value * 15;
                    timer.Enabled = value != 0;
                    timer.Interval = Time;
                }
            }
        }

        public int Time { get; private set; } = 1515;

        public Colors[] Effect
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

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            arduinoCollection.SetColor(Effect[Count]);
            if (Count == Effect.Length - 1) Count = 0;
            else Count++;
        }
    }
}
