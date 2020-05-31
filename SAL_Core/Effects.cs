﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SAL_Core
{
    public class Effects
    {

        private Timer timer;
        private ArduinoCollection arduinoCollection;

        private Effect effect;

        public EffectSettings Settings { get; } = new EffectSettings();

        public string Current
        {
            get
            {
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
                }
            }
        }

        public int Time { get; private set; } = 100;


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

            InitializeEffect();
        }

        public void InitializeEffect()
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
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            effect.Step();
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

    class Effect
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
}
