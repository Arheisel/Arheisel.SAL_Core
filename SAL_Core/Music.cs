using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SAL_Core
{
    public class Music
    {
        public delegate void DataAvailableEventHandler(double sample);
        public event DataAvailableEventHandler DataAvailable;

        public readonly AutoScaler AutoScaler;
        public double Slope = 10.0;

        private readonly ArduinoCollection arduinoCollection;
        private readonly Audio audio;
        private readonly Timer timer;
        

        public Music(ArduinoCollection arduino)
        {
            arduinoCollection = arduino;
            audio = new Audio();
            timer = new Timer()
            {
                AutoReset = true,
                Enabled = false,
                Interval = 5
            };
            timer.Elapsed += Timer_Elapsed;
            AutoScaler = new AutoScaler();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            new Task(ProcessAudioData).Start();
        }

        public void Run()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private void ProcessAudioData()
        {
            /*
            var values = new List<float>();
            values.Add(audio.Peak);

            float avg = 0;
            foreach (float i in values)
            {
                avg += i;
            }
            avg /= values.Count;*/

            var avg = audio.Peak;
            if (avg == 0)
            {
                arduinoCollection.SetColor(Colors.OFF);
                return;
            }
            double x = Convert.ToDouble(avg * AutoScaler.Scale);
            if (x > 1.09) x = 1.09;
            double res = (1.0 / ((1.1 - x) * Slope)) - 0.1;
            AutoScaler.Sample(res);
            if (res > 1) res = 1;
            else if (res < 0) res = 0;

            int index = Convert.ToInt32(Math.Floor(res * Maps.MaxIndex + 0.99));
            if (index < 0) index = 0;
            else if (index > Maps.MaxIndex) index = Maps.MaxIndex;
            arduinoCollection.SetColor(Maps.Map[index]);

            DataAvailable?.Invoke(res);

            //values.Clear();
        }

    }
}
