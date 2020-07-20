using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Damez.Log;

namespace SAL_Core
{
    public class Effects : IDisposable
    {
        public event EventHandler<EffectDataAvailableArgs> DataAvailable;
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
                    case EffectTypes.Sweep:
                        effect = new Sweep(arduinoCollection, preset);
                        break;
                    case EffectTypes.Load:
                        effect = new Load(arduinoCollection, preset);
                        break;
                    case EffectTypes.Beam:
                        effect = new Beam(arduinoCollection, preset);
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
                var colors = effect.Step();
                if(colors.Count > 0)
                    DataAvailable?.Invoke(this, new EffectDataAvailableArgs(colors));
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

    public class EffectDataAvailableArgs : EventArgs
    {
        public readonly List<ChColor> Colors;

        public EffectDataAvailableArgs(List<ChColor> chColors)
        {
            Colors = chColors;
        }
    }

    public enum EffectTypes
    {
        Rainbow = 0,
        Cycle = 1,
        Breathing = 2,
        Flash = 3,
        Fire = 4,
        Static = 5,
        Sweep = 6,
        Load = 7,
        Beam = 8
    }

    class Effect : IDisposable
    {
        protected readonly ArduinoCollection arduino;
        public EffectPreset Preset { get; set; }
        protected int step = 0;
        protected int count = 0;
        protected readonly List<ChColor> colors = new List<ChColor>();

        public Effect(ArduinoCollection collection, EffectPreset settings)
        {
            Preset = settings;
            arduino = collection;
            arduino.SetColor(Colors.OFF);
        }

        public virtual List<ChColor> Step()
        {
            return null;
        }

        public virtual void Reset()
        {
            count = 0;
            step = 0;
            colors.Clear();
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
        private int stage = 0;
        private int startPoint = 0;
        public Rainbow(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            
            if(step == 0)
            {
                channels = arduino.ChannelCount;
                count = startPoint;
                for (int i = 0; i < channels; i++)
                {
                    if ((i.Even() && stage == 0) || (i.Odd() && stage == 1))
                    {
                        if (Preset.Reverse)
                            colors.Add(new ChColor(channels - i, Preset.ColorList[count.Mod(Preset.ColorList.Count)]));
                        else
                            colors.Add(new ChColor(i + 1, Preset.ColorList[count.Mod(Preset.ColorList.Count)]));
                    }
                    else
                    {
                        transitions[i] = new Transition(Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)], Preset.ColorList[count.Mod(Preset.ColorList.Count)], Preset.TotalSteps);
                        if (Preset.Reverse)
                            colors.Add(new ChColor(channels - i, Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)]));
                        else
                            colors.Add(new ChColor(i + 1, Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)]));
                        if (count >= Preset.ColorList.Count - 1) count = 0;
                        else count++;
                    }
                }
                if(stage == 0)
                {
                    if (startPoint <= 0) startPoint = Preset.ColorList.Count - 1;
                    else startPoint--;
                }
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    if (stage == 1) stage = 0;
                    else stage = 1;
                    return colors;
                }

                if (step < Preset.TotalSteps)
                {
                    for (int i = 0; i < channels; i++)
                    {
                        if ((i.Even() && stage == 1) || (i.Odd() && stage == 0))
                        {
                            if (Preset.Reverse)
                                colors.Add(new ChColor(channels - i, transitions[i].getColor(step)));
                            else
                                colors.Add(new ChColor(i + 1, transitions[i].getColor(step)));
                        }
                    }  
                }
                    
                step++;
            }
            return colors;
        }

        public override void Reset()
        {
            startPoint = 0;
            stage = 0;
            base.Reset();
        }
    }

    class Cycle : Effect
    {
        private Transition transition;
        public Cycle(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            if (step == 0)
            {
                transition = new Transition(Preset.ColorList[count], Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)], Preset.TotalSteps);
                colors.Add(new ChColor(0, Preset.ColorList[count]));

                if (count >= Preset.ColorList.Count - 1) count = 0;
                else count++;
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return colors;
                }

                if (step < Preset.TotalSteps)
                    colors.Add(new ChColor(0, transition.getColor(step)));

                step++;
            }
            return colors;
        }
    }

    class Breathing : Effect
    {
        private Transition transition;
        private bool mode = true;
        public Breathing(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            if (step == 0)
            {
                if (mode)
                {
                    transition = new Transition(Colors.OFF, Preset.ColorList[count], Preset.TotalSteps);
                    colors.Add(new ChColor(0, Colors.OFF));

                    mode = false;
                }
                else
                {
                    transition = new Transition(Preset.ColorList[count], Colors.OFF, Preset.TotalSteps);
                    colors.Add(new ChColor(0, Preset.ColorList[count]));

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
                    return colors;
                }

                if (step < Preset.TotalSteps)
                    colors.Add(new ChColor(0, transition.getColor(step)));

                step++;
            }
            return colors;
        }
    }

    class Flash : Effect
    {
        private Color color;
        public Flash(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            if (step == 0)
            {
                color = Preset.ColorList[count];
                colors.Add(new ChColor(0, Colors.OFF));

                if (count >= Preset.ColorList.Count - 1) count = 0;
                else count++;
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return colors;
                }

                if (step > Preset.TotalSteps)
                    colors.Add(new ChColor(0, color));

                step++;
            }
            return colors;
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

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
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
                    return colors;
                }

                if(step > Preset.TotalSteps)
                    colors.Add(new ChColor(0, transition.getColor(random.Next(0, Preset.TotalSteps))));
                else
                    colors.Add(new ChColor(0, transition.getColor(random.Next(0, Preset.TotalSteps/2))));

                step++;
            }
            return colors;
        }
    }

    class Static : Effect
    {
        public Static(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
            
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            if (step == 0)
            {
                colors.Add(new ChColor(0, Preset.ColorList[count]));

                if (count >= Preset.ColorList.Count - 1) count = 0;
                else count++;
                step++;
            }
            else
            {
                if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                {
                    step = 0;
                    return colors;
                }

                step++;
            }
            return colors;
        }
    }

    class Sweep : Effect
    {
        private int channels = 0;
        private int currentChannel = 1;
        private Transition transition;
        public Sweep(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            if (step == 0)
            {
                channels = arduino.ChannelCount;
                if(currentChannel == 1)
                {
                    transition = new Transition(Preset.ColorList[count], Preset.ColorList[(count + 1).Mod(Preset.ColorList.Count)], Preset.TotalSteps);
                    colors.Add(new ChColor(0, Preset.ColorList[count]));
                    if (count >= Preset.ColorList.Count - 1) count = 0;
                    else count++;
                }
                
                step++;
            }
            else
            {
                if (currentChannel >= channels)
                {
                    if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                    {
                        currentChannel = 1;
                        step = 0;
                        return colors;
                    }
                }
                else
                {
                    if (step >= Preset.TotalSteps)
                    { 
                        currentChannel++;
                        step = 0;
                        return colors;
                    }
                }
                

                if (step < Preset.TotalSteps)
                    if (Preset.Reverse)
                        colors.Add(new ChColor((channels + 1) - currentChannel, transition.getColor(step)));
                    else
                        colors.Add(new ChColor(currentChannel, transition.getColor(step)));

                step++;
            }
            return colors;
        }
    }

    class Load : Effect
    {
        private int channels = 0;
        private int currentChannel = 1;
        private Transition transition;
        public Load(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            if (step == 0)
            {
                channels = arduino.ChannelCount;
                if (currentChannel == 1)
                {
                    transition = new Transition(Colors.OFF, Preset.ColorList[count], Preset.TotalSteps);
                    colors.Add(new ChColor(0, Colors.OFF));
                    if (count >= Preset.ColorList.Count - 1) count = 0;
                    else count++;
                }

                step++;
            }
            else
            {
                if (currentChannel >= channels)
                {
                    if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                    {
                        currentChannel = 1;
                        step = 0;
                        return colors;
                    }
                }
                else
                {
                    if (step >= Preset.TotalSteps)
                    {
                        currentChannel++;
                        step = 0;
                        return colors;
                    }
                }

                if (step < Preset.TotalSteps)
                    if (Preset.Reverse)
                        colors.Add(new ChColor((channels + 1) - currentChannel, transition.getColor(step)));
                    else
                        colors.Add(new ChColor(currentChannel, transition.getColor(step)));

                step++;
            }
            return colors;
        }
    }

    class Beam : Effect
    {
        private int channels = 0;
        private int currentChannel = 1;
        private Transition transition;
        private Transition transitionOff;
        public Beam(ArduinoCollection collection, EffectPreset settings) : base(collection, settings)
        {
        }

        public override List<ChColor> Step()
        {
            colors.Clear();
            if (arduino.ChannelCount == 0) return colors;
            if (step == 0)
            {
                channels = arduino.ChannelCount;
                if (currentChannel == 1)
                {
                    transition = new Transition(Colors.OFF, Preset.ColorList[count], Preset.TotalSteps);
                    transitionOff = new Transition(Preset.ColorList[count], Colors.OFF, Preset.TotalSteps);
                    colors.Add(new ChColor(0, Colors.OFF));
                    if (count >= Preset.ColorList.Count - 1) count = 0;
                    else count++;
                }

                step++;
            }
            else
            {
                if (currentChannel >= channels + 1)
                {
                    if (step >= Preset.TotalSteps + Preset.HoldingSteps)
                    {
                        currentChannel = 1;
                        step = 0;
                        return colors;
                    }
                }
                else
                {
                    if (step >= Preset.TotalSteps)
                    {
                        currentChannel++;
                        step = 0;
                        return colors;
                    }
                }

                if (step < Preset.TotalSteps)
                {
                    if (Preset.Reverse)
                    {
                        if (currentChannel <= channels)
                            colors.Add(new ChColor((channels + 1) - currentChannel, transition.getColor(step)));
                        if (currentChannel > 1)
                            colors.Add(new ChColor((channels + 2) - currentChannel, transitionOff.getColor(step)));
                    }
                    else
                    {
                        if (currentChannel <= channels)
                            colors.Add(new ChColor(currentChannel, transition.getColor(step)));
                        if (currentChannel > 1)
                            colors.Add(new ChColor(currentChannel - 1, transitionOff.getColor(step)));

                    }
                }
                    

                step++;
            }
            return colors;
        }
    }
}
