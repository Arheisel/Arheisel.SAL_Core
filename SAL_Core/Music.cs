using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace SAL_Core
{
    public class Music
    {
        //public delegate void DataAvailableEventHandler(double sample);
        public event EventHandler<DataAvailableArgs> DataAvailable;

        public readonly AutoScaler AutoScaler;
        public readonly MusicData Data;

        private readonly ArduinoCollection arduinoCollection;
        //private readonly Audio audio;
        //private readonly Stopwatch timer;
        private readonly Thread thread;

        public Music(ArduinoCollection arduino)
        {
            arduinoCollection = arduino;
            AutoScaler = new AutoScaler();
            thread = new Thread(new ThreadStart(ProcessAudioData));
            Data = new MusicData();
        }

        public void Run()
        {
            thread.Start();
        }

        public void Stop()
        {
            thread.Abort();
        }

        private void ProcessAudioData()
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
                        arduinoCollection.SetColor(Colors.OFF);
                        Thread.Sleep(5);
                        values.Clear();
                    }
                    double x = Convert.ToDouble(avg * AutoScaler.Scale);
                    if (x > 1.09) x = 1.09;
                    double res = (1.0 / ((1.1 - x) * Data.Slope)) - 0.1;
                    AutoScaler.Sample(res);
                    if (res > 1) res = 1;
                    else if (res < 0) res = 0;

                    int index = Convert.ToInt32(Math.Floor(res * Maps.MaxIndex + 0.99));
                    if (index < 0) index = 0;
                    else if (index > Maps.MaxIndex) index = Maps.MaxIndex;
                    arduinoCollection.SetColor(Maps.Map[index]);
                    Data.Peak = res;
                    DataAvailable?.Invoke(this, new DataAvailableArgs(res));

                    values.Clear();
                    timer.Restart();
                }
            }
        }

    }

    public class DataAvailableArgs : EventArgs
    {
        public DataAvailableArgs(double sample)
        {
            Sample = sample;
        }

        public double Sample { get; }
    }

    public class MusicData
    {
        public double Peak;
        public double Slope = 10;
    }
}
