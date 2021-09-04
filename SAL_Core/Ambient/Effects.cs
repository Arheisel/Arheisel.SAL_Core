using System;
using System.Linq;
using System.Timers;
using Arheisel.Log;
using SAL_Core.IO;
using SAL_Core.Settings;
using SAL_Core.Ambient.Types;

namespace SAL_Core.Ambient
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
}
