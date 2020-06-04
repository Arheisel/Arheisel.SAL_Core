using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Damez.Log;

namespace SAL_Core
{
    public class Music
    {
        //public delegate void DataAvailableEventHandler(double sample);
        public event EventHandler<MusicDataAvailableArgs> DataAvailable;

        public readonly AutoScaler AutoScaler;
        public readonly MusicSettings Settings;

        //private readonly Audio audio;
        //private readonly Stopwatch timer;
        private readonly Thread thread;


        public Music(MusicSettings settings)
        {
            try
            {
                Settings = settings;
                AutoScaler = new AutoScaler(settings.Autoscaler);
                thread = new Thread(new ThreadStart(ProcessAudioData));
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Music :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        public void Run()
        {
            try
            {
                thread.Start();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Music :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        public void Stop()
        {
            try
            {
                thread.Abort();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Music :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        public double Curve(double x)
        {
            return (1.0 / ((1.1 - x) * Settings.Slope)) - 0.1;
        }

        private void ProcessAudioData()
        {
            try
            {
                var audio = new Audio();
                var timer = new Stopwatch();
                var values = new List<float>();
            

                timer.Start();

                while (true)
                {
                    values.Add(audio.Peak);

                    if(timer.ElapsedMilliseconds > 5)
                    {
                        float avg = 0;
                        foreach (float i in values)
                        {
                            avg += i;
                        }
                        avg /= values.Count;

                        if (avg == 0)
                        {
                            DataAvailable?.Invoke(this, new MusicDataAvailableArgs(0));
                            values.Clear();
                            Thread.Sleep(5);
                            continue;
                        }
                        double x = Convert.ToDouble(avg * AutoScaler.Scale);
                        if (x > 1.09) x = 1.09;
                        double res = Curve(x);
                        AutoScaler.Sample(res);
                        if (res > 1) res = 1;
                        else if (res < 0) res = 0;

                        DataAvailable?.Invoke(this, new MusicDataAvailableArgs(res));

                        values.Clear();
                        timer.Restart();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Music :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }
    }

    public class MusicDataAvailableArgs : EventArgs
    {
        public MusicDataAvailableArgs(double sample)
        {
            Sample = sample;
        }

        public double Sample { get; }
    }
}
