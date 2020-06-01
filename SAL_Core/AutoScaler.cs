using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SAL_Core
{
    public class AutoScaler
    {
        private readonly Timer timer;

        public double Peak = 0;

        private readonly AutoscalerSettings Settings;

        private bool newSamples = false;

        public bool Enabled
        {
            get
            {
                return Settings.Enabled;
            }
            set
            {
                Settings.Enabled = value;
                if (value)
                {
                    timer.Start();
                }
                else
                {
                    timer.Stop();
                }
            }
        }

        public double Scale
        {
            get
            {
                return Settings.Scale;
            }
            set
            {
                Settings.Scale = value;
            }
        }

        public void Stop()
        {
            timer.Stop();
        }

        public AutoScaler(AutoscalerSettings settings)
        {
            Settings = settings;
            timer = new Timer()
            {
                AutoReset = true,
                Enabled = false,
                Interval = 100,
            };
            timer.Elapsed += Autoscale;

            if (settings.Enabled) timer.Start();
        }

        public void Sample(double value)
        {
            if(value > Peak)
            {
                Peak = value;
            }
            newSamples = true;
        }

        public void Autoscale(object sender, ElapsedEventArgs e)
        {
            if (newSamples) newSamples = false;
            else return;

            if (Peak > 1)
            {
                Scale -= (float)0.02;
                Peak = 0.9;
            }
            if (Peak < 0.8)
            {
                Scale += (float)0.01;
                Peak = 0.85;
            }
            Peak -= 0.01;
        }
    }
}
