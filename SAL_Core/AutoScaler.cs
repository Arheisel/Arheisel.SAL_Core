using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SAL_Core
{
    class AutoScaler
    {
        private readonly Timer timer;
        private bool _enabled = false;

        public double Scale = 1;
        public double Peak = 0;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
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

        public AutoScaler()
        {
            timer = new Timer()
            {
                AutoReset = true,
                Enabled = false,
                Interval = 100,
            };
            timer.Elapsed += Autoscale;
        }

        public void Sample(double value)
        {
            if(value > Peak)
            {
                Peak = value;
            }
        }

        public void Autoscale(object sender, ElapsedEventArgs e)
        {
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
