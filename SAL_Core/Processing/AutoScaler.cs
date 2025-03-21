﻿using SAL_Core.Settings;
using System;
using System.Timers;

namespace SAL_Core.Processing
{
    public class AutoScaler : IDisposable
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
                timer.Enabled = value;
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

        public int Amp
        {
            get
            {
                return Settings.Amp;
            }
            set
            {
                Settings.Amp = value;
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
            if (value <= 0) return;
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
                timer.Dispose();
            }

            _disposed = true;
        }
    }
}
