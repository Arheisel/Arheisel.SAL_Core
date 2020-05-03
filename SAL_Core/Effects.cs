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
        private string _current = "Rainbow";
        private Timer timer;
        private ArduinoCollection arduinoCollection;

        public int Count = 0;
        private Transition transition;
        private int step = 0;
        private int totalSteps = 255;
        private int holdingSteps = 50;

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
            {"Rainbow", new Color[] { Colors.MAGENTA, Colors.CYAN, Colors.YELLOW } },
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

        public int Steps
        {
            get
            {
                return totalSteps;
            }
            set
            {
                if (value > 0 && value <= 255)
                {
                    totalSteps = value;
                }
            }
        }

        public int HoldSteps
        {
            get
            {
                return holdingSteps;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    holdingSteps = value;
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

        public Color CurrentColor
        {
            get
            {
                return Effect[Count];
            }
        }

        public Color NextColor
        {
            get
            {
                if (Count == Effect.Length - 1) return Effect[0];
                else return Effect[Count + 1];
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

            transition = new Transition(CurrentColor, NextColor, totalSteps);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(step >= totalSteps + holdingSteps)
            {
                if (Count == Effect.Length - 1) Count = 0;
                else Count++;
                transition = new Transition(CurrentColor, NextColor, totalSteps);
                step = 0;
            }

            if(step < totalSteps)
                arduinoCollection.SetColor(transition.getColor(step));

            step++;
        }
    }

    class Transition
    {
        private TransitionColor R;
        private TransitionColor G;
        private TransitionColor B;

        public Transition(Color oldColor, Color newColor, int steps)
        {
            R = new TransitionColor(oldColor.R, newColor.R, steps);
            G = new TransitionColor(oldColor.G, newColor.G, steps);
            B = new TransitionColor(oldColor.B, newColor.B, steps);
        }

        public Color getColor(int step)
        {
            return new Color(R.getColorValue(step), G.getColorValue(step), B.getColorValue(step));
        }
    }

    class TransitionColor
    {
        private double m;
        private double b;

        public TransitionColor(double oldValue, double newValue, double steps)
        {
            m = (newValue - oldValue) / (steps - 1.0);
            b = oldValue;
        }

        public int getColorValue(double step)
        {
            return (int)Math.Round(m * step + b);
        }
    }
}
