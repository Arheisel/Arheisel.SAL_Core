using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Damez.Log;

namespace SAL_Core
{
    public class Effects : IDisposable
    {

        private Timer timer;
        private ArduinoCollection arduinoCollection;

        private Effect effect;

        public readonly EffectSettings Settings;

        public string Current
        {
            get
            {
                if (!Settings.PresetList.ContainsKey(Settings.Current))
                {
                    Settings.Current = Settings.PresetList.First().Key;
                }
                return Settings.Current;
            }
            set
            {
                if (Settings.PresetList.ContainsKey(value))
                {
                    Settings.Current = value;
                    InitializeEffect();
                }
            }
        }

        public int Speed
        {
            get
            {
                return Settings.CurrentPreset.Speed;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    Settings.CurrentPreset.Speed = value;
                    Time = 101 - value;
                    timer.Enabled = value != 0;
                    timer.Interval = Time;
                }
            }
        }

        public void Stop()
        {
            timer.Enabled = false;
        }

        public int Steps
        {
            get
            {
                return Settings.CurrentPreset.TotalSteps;
            }
            set
            {
                if (value > 0 && value <= 255)
                {
                    Settings.CurrentPreset.TotalSteps = value;
                    effect.Reset();
                }
            }
        }

        public int HoldSteps
        {
            get
            {
                return Settings.CurrentPreset.HoldingSteps;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    Settings.CurrentPreset.HoldingSteps = value;
                    effect.Reset();
                }
            }
        }

        public int Time { get; private set; } = 100;


        public Effects(ArduinoCollection arduino, EffectSettings settings)
        {
            Settings = settings;
            timer = new Timer()
            {
                Enabled = false,
                Interval = Time,
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;
            arduinoCollection = arduino;

            InitializeEffect();
        }

        public void InitializeEffect()
        {
            try
            {
                var preset = Settings.CurrentPreset;
                Speed = preset.Speed;
                switch (preset.Type)
                {
                    case EffectTypes.Rainbow:
                        effect = new Rainbow(arduinoCollection, preset);
                        break;
                    case EffectTypes.Cycle:
                        effect = new Cycle(arduinoCollection, preset);
                        break;
                    case EffectTypes.Breathing:
                        effect = new Breathing(arduinoCollection, preset);
                        break;
                    case EffectTypes.Flash:
                        effect = new Flash(arduinoCollection, preset);
                        break;
                    case EffectTypes.Fire:
                        effect = new Fire(arduinoCollection, preset);
                        break;
                    case EffectTypes.Static:
                        effect = new Static(arduinoCollection, preset);
                        break;
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Effects :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }  
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                effect.Step();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "Effects :: " + Current +  " :: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }  
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                effect.Dispose();
                timer.Stop();
                timer.Dispose();
            }

            _disposed = true;
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

    public enum EffectTypes
    {
        Rainbow = 0,
        Cycle = 1,
        Breathing = 2,
        Flash = 3,
        Fire = 4,
        Static = 5
    }

    class Effect : IDisposable
    {
        protected readonly ArduinoCollection arduino;
        public EffectPreset Preset { get; set; }
        protected int step = 0;
        protected int count = 0;

        public Effect(ArduinoCollection collection, EffectPreset settings)
        {
            Preset = settings;
            arduino = collection;
        }

        public virtual void Step()
        {

        }

        public void Reset()
        {
            count = 0;
            step = 0;
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }
    }

    class Rainbow : Effect
    {
        private int channels = 0;
        private readonly Transition[] transitions = new Transition[100];
        public Rainbow(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override void Step()
        {
            if (arduino.ChannelCount == 0) return;
            if(step == 0)
            {
                channels = arduino.ChannelCount;
                for(int i = 0; i < channels; i++)
                {
                    transitions[i] = new Transition(Preset.ColorList[MathH.Mod(count - i, Preset.ColorList.Count)], Preset.ColorList[MathH.Mod(count - i + 1, Preset.ColorList.Count)], Preset.TotalSteps);
                    arduino.SetColor(i + 1, Preset.ColorList[MathH.Mod(count - i, Preset.ColorList.Count)]);
                }
                if (count >= Preset.ColorList.Count - 1) count = 0;
                else count++;
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return;
                }

                if (step < Preset.TotalSteps)
                    for (int i = 0; i < channels; i++)
                        arduino.SetColor(i + 1, transitions[i].getColor(step));

                step++;
            }
        }
    }

    class Cycle : Effect
    {
        private Transition transition;
        public Cycle(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override void Step()
        {
            if (arduino.ChannelCount == 0) return;
            if (step == 0)
            {
                transition = new Transition(Preset.ColorList[count], Preset.ColorList[MathH.Mod(count + 1, Preset.ColorList.Count)], Preset.TotalSteps);
                arduino.SetColor(Preset.ColorList[count]);

                if (count >= Preset.ColorList.Count - 1) count = 0;
                else count++;
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return;
                }

                if (step < Preset.TotalSteps)
                        arduino.SetColor(transition.getColor(step));

                step++;
            }
        }
    }

    class Breathing : Effect
    {
        private Transition transition;
        private bool mode = true;
        public Breathing(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override void Step()
        {
            if (arduino.ChannelCount == 0) return;
            if (step == 0)
            {
                if (mode)
                {
                    transition = new Transition(Colors.OFF, Preset.ColorList[count], Preset.TotalSteps);
                    arduino.SetColor(Colors.OFF);

                    mode = false;
                }
                else
                {
                    transition = new Transition(Preset.ColorList[count], Colors.OFF, Preset.TotalSteps);
                    arduino.SetColor(Preset.ColorList[count]);

                    if (count >= Preset.ColorList.Count - 1) count = 0;
                    else count++;

                    mode = true;
                }
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return;
                }

                if (step < Preset.TotalSteps)
                    arduino.SetColor(transition.getColor(step));

                step++;
            }
        }
    }

    class Flash : Effect
    {
        private Color color;
        public Flash(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override void Step()
        {
            if (arduino.ChannelCount == 0) return;
            if (step == 0)
            {
                color = Preset.ColorList[count];
                arduino.SetColor(Colors.OFF);

                if (count >= Preset.ColorList.Count - 1) count = 0;
                else count++;
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return;
                }

                if (step > Preset.TotalSteps)
                    arduino.SetColor(color);

                step++;
            }
        }
    }

    class Fire : Effect
    {
        private Transition transition;
        private Random random;
        public Fire(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
            random = new Random();
        }

        public override void Step()
        {
            if (arduino.ChannelCount == 0) return;
            if (step == 0)
            {
                transition = new Transition(Preset.ColorList[0], Preset.ColorList[1], Preset.TotalSteps); ;

                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return;
                }

                if(step > Preset.TotalSteps)
                    arduino.SetColor(transition.getColor(random.Next(0, Preset.TotalSteps)));
                else
                    arduino.SetColor(transition.getColor(random.Next(0, Preset.TotalSteps/2)));

                step++;
            }
        }
    }

    class Static : Effect
    {
        public Static(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
            
        }

        public override void Step()
        {
            if (arduino.ChannelCount == 0) return;
            if (step == 0)
            {
                arduino.SetColor(Preset.ColorList[count]);

                if (count >= Preset.ColorList.Count - 1) count = 0;
                else count++;
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return;
                }

                step++;
            }
        }
    }
}
