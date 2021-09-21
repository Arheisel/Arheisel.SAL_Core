using System;
using System.Linq;
using System.Timers;
using Arheisel.Log;
using SAL_Core.Settings;
using SAL_Core.Ambient.Types;
using System.Collections.Generic;
using SAL_Core.RGB;

namespace SAL_Core.Ambient
{
    public class Effects : IDisposable
    {
        private readonly Timer timer;
        private Effect effect;
        

        public event EventHandler<EffectDataAvailableArgs> DataAvailable;
        public EffectSettings Settings { get; }

        private int _channels = 0;
        public int ChannelCount
        {
            get
            {
                return _channels;
            }
            set
            {
                if(value >= 0)
                {
                    _channels = value;
                    InitializeEffect();
                }
            }
        }


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
                    Time = 10.1 - value/10.0;
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

        public double Time { get; private set; } = 100;

        public Effects(EffectSettings settings, int channelCount)
        {
            Settings = settings;
            timer = new Timer()
            {
                Enabled = false,
                Interval = Time,
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;

            ChannelCount = channelCount;
        }

        public void InitializeEffect()
        {
            try
            {
                timer.Stop();
                var preset = Settings.CurrentPreset;
                switch (preset.Type)
                {
                    case EffectTypes.Rainbow:
                        effect = new Rainbow(preset, _channels);
                        break;
                    case EffectTypes.Cycle:
                        effect = new Cycle(preset, _channels);
                        break;
                    case EffectTypes.Breathing:
                        effect = new Breathing(preset, _channels);
                        break;
                    case EffectTypes.Flash:
                        effect = new Flash(preset, _channels);
                        break;
                    case EffectTypes.Fire:
                        effect = new Fire(preset, _channels);
                        break;
                    case EffectTypes.Static:
                        effect = new Static(preset, _channels);
                        break;
                    case EffectTypes.Sweep:
                        effect = new Sweep(preset, _channels);
                        break;
                    case EffectTypes.Load:
                        effect = new Load(preset, _channels);
                        break;
                    case EffectTypes.Beam:
                        effect = new Beam(preset, _channels);
                        break;
                }
                DataAvailable?.Invoke(this, new EffectDataAvailableArgs(new List<ChColor>() { new ChColor(0, Colors.OFF) }));
                Speed = preset.Speed;
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
                if (_channels == 0) return;

                var colors = effect.Step();
                if(colors.Count > 0)
                    DataAvailable?.Invoke(this, new EffectDataAvailableArgs(colors));
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "Effects :: " + Current +  " :: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }  
        }

        public void Stop()
        {
            timer.Stop();
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
                timer.Stop();
                timer.Close();
                timer.Dispose();
            }

            _disposed = true;
        }
    }
}
