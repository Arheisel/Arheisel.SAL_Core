using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using AForge.Math;
using Arheisel.Log;

namespace SAL_Core
{
    public class Audio : IDisposable
    {
        private const int minLength = 2;
        private const int maxLength = 16384;

        public event EventHandler<AudioDataAvailableArgs> DataAvailable;

        private readonly MMDeviceEnumerator enumerator = null;
        private readonly MMDevice device = null;

        private WasapiLoopbackCapture capture = null;
        private BufferedWaveProvider waveBuffer = null;
        private WaveFormat waveFormat = null;
        private int bytesPerFrame = 1;
        private readonly AudioSettings Settings;

        public readonly AutoScaler autoScaler;
        private readonly EqualizerSettings Equalizer = new EqualizerSettings() { High = 10, Mid = 2, Low = 1 };

        private int _channels = 8;

        private int divider = 1;
        private double _octave = 0;
        private double _octaveDist = 1.5;

        private double _curve_cutoff;

        public int Channels
        {
            get
            {
                return _channels;
            }
            set
            {
                if (value > 0)
                {
                    _channels = value;
                    UpdateOctaveDist();
                }
            }
        }

        public int AudioChannels
        {
            get
            {
                if (capture != null) return capture.WaveFormat.Channels;
                else return 0;
            }
        }

        public int MinFreq
        {
            get
            {
                return Settings.MinFreq;
            }
            set
            {
                if (value > 0 && value < Settings.MaxFreq)
                {
                    Settings.MinFreq = value;
                    UpdateOctaveDist();

                }
            }
        }

        public int MaxFreq
        {
            get
            {
                return Settings.MaxFreq; // * capture.WaveFormat.Channels;
            }
            set
            {
                //value = value / capture.WaveFormat.Channels; //I don't know why this works and I'm too afraid to ask
                if (value > Settings.MinFreq && waveFormat != null && value <= waveFormat.SampleRate)
                {
                    Settings.MaxFreq = value;
                    divider = (waveFormat.SampleRate / Settings.MaxFreq);
                    UpdateOctaveDist();
                }
            }
        }

        public int Slope
        {
            get
            {
                return Settings.Slope;
            }
            set
            {
                if(value >= 0 && value <= 50)
                {
                    Settings.Slope = value;
                    _curve_cutoff = value / 1000.0;
                }
            }
        }

        private void UpdateOctaveDist()
        {
            _octaveDist = Math.Pow((double)Settings.MaxFreq / (double)Settings.MinFreq, 1 / (double)_channels);
            _octave = Settings.MinFreq * _octaveDist;
        }


        public float Peak
        {
            get
            {
                return device.AudioMeterInformation.MasterPeakValue;
            }
        }

        public Audio()
        {
            try
            {
                enumerator = new MMDeviceEnumerator();
                device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Audio :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        public Audio(AudioSettings settings)
        {
            try
            {
                Settings = settings;
                Slope = settings.Slope; //calculate curve variables
                autoScaler = new AutoScaler(settings.Autoscaler);
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Audio :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        public void StartCapture()
        {
            if (capture != null) StopCapture();
            try
            {
                capture = new WasapiLoopbackCapture();
                waveFormat = capture.WaveFormat;
                bytesPerFrame = (capture.WaveFormat.BitsPerSample / 8); // * capture.WaveFormat.Channels;
                MaxFreq = MaxFreq; // Calculate divider and Update Octave Distance
                waveBuffer = new BufferedWaveProvider(waveFormat)
                {
                    DiscardOnBufferOverflow = true
                };
                capture.StartRecording();
                capture.DataAvailable += Capture_DataAvailable;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Audio :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        public void StopCapture()
        {
            if (capture == null) return;
            try
            {
                capture.StopRecording();
                capture.Dispose();
                capture = null;
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Audio :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }      
        }

        public double Curve(double x)
        {

            return x > _curve_cutoff ? x : 0;

            /*
            if(x < _curve_cutoff)
            {
                return Math.Tan(x * _curve_tan_multiplier);
            }
            else
            {
                return Math.Sqrt(x);
            }
            */

            /*Eq Log
             * (Math.Log10(x) + _curve_cutoff) / _curve_cutoff;
             * Math.Sqrt(1 - Math.Pow(x - 1, 2));
             * Math.Log10(x) + (Settings.Slope / 10);
             * Math.Log10(x) + (Slope / 10); 
             * Math.Log10(x*10) * (10/slope); 
             * Math.Log10(x) + (slope / 10); 
             * (Math.Log(x, 2) + slope) / 10;
            */
        }

        private void Capture_DataAvailable(object sender, WaveInEventArgs e)
        {
            Task.Run(() => ProcessAudioData(e));
        }

        private void ProcessAudioData(WaveInEventArgs e)
        {
            try
            {
                waveBuffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
                ISampleProvider samples = waveBuffer.ToSampleProvider();

                int count = (waveBuffer.BufferedBytes / bytesPerFrame) / waveFormat.Channels;

                int n = 0;
                for (int i = 0; i < 30; i++)
                {
                    int curr = 1 << i;
                    if (curr > count) break;
                    n = curr;
                }
                count = n * waveFormat.Channels;

                if (n < minLength) return;

                if (n > maxLength)
                {
                    n = maxLength;
                }

                var frames = new float[count];
                samples.Read(frames, 0, count);
                waveBuffer.ClearBuffer();

                Complex[] complexBuffer = new Complex[n];

                if (waveFormat.Channels > 1)
                {
                    var bufferIndex = 0;
                    for (int i = 0; i < count; i += waveFormat.Channels)
                    {
                        float f = 0;
                        for (int j = 0; j < waveFormat.Channels; j++)
                        {
                            f += frames[i + j];
                        }
                        complexBuffer[bufferIndex++] = new Complex(f, 0);
                    }
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        complexBuffer[i] = new Complex(frames[i], 0);
                    }
                }

                FourierTransform.FFT(complexBuffer, FourierTransform.Direction.Forward);

                double[] transform = new double[Convert.ToInt32(Math.Floor((double)n / divider))];

                double step = (double)waveFormat.SampleRate / (double)n;
                int offset = Convert.ToInt32(Settings.MinFreq / step);

                for (int i = 0; i < transform.Length && i + offset < complexBuffer.Length; i++)
                {
                    transform[i] = complexBuffer[i + offset].Magnitude * 2;
                }

                double[] channelMagnitudes = new double[_channels];

                int startingPoint = 0;
                int startingPointLast = 0;

                double startingFreq = step * offset; //theoretical frecuency of transform[0]
                double octave = _octave;
                double peak = 0;

                // EQUALIZER
                for(int j = 0; j < transform.Length; j++)
                {
                    var freq = j * step + startingFreq;

                    if(freq < 300)
                    {
                        transform[j] *= Equalizer.Low;
                    }
                    else
                    {
                        if(freq < 5000)
                        {
                            transform[j] *= Equalizer.Mid;
                        }
                        else
                        {
                            transform[j] *= Equalizer.High;
                        }
                    }
                }

                for (int i = 0; i < _channels; i++)
                {
                    double max = 0;
                    for (int j = startingPoint; j * step + startingFreq < octave && j < transform.Length; j++, startingPoint++) //startingPoint + nFreqs
                    {
                        if (transform[j] > max) max = transform[j];
                    }
                    octave *= _octaveDist;

                    if (startingPoint == startingPointLast && startingPoint < transform.Length - 1)
                    {
                        max = transform[startingPoint++];
                    }

                    startingPointLast = startingPoint;

                    double x = max * autoScaler.Scale;
                    double res = Curve(x);
                    autoScaler.Sample(res);
                    if (res > 1)
                    {
                        res = 1;
                    }
                    else if (res < 0) res = 0;
                    channelMagnitudes[i] = res;
                    if (res > peak) peak = res;
                }
                DataAvailable?.Invoke(this, new AudioDataAvailableArgs(peak, channelMagnitudes));
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "Audio :: " + ex.Message + Environment.NewLine + ex.StackTrace);
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
                if (enumerator != null) enumerator.Dispose();
                if (device != null) device.Dispose();
                if (capture != null) capture.Dispose();
            }

            _disposed = true;
        }
    }

    public static class Maps
    {
        private static readonly List<Color[]> list = new List<Color[]>()
        {
            new Color[]
            {
                Colors.OFF,
                Colors.LYME,
                Colors.AQGREEN,
                Colors.CYAN,
                Colors.EBLUE,
                Colors.BLUE,
                Colors.PURPLE,
                Colors.MAGENTA,
                Colors.PINK,
                Colors.YELLOW,
                Colors.ORANGE,
                Colors.RED
            },
            new Color[]
            {
                Colors.OFF,
                Colors.WHITE
            }
        };

        private static uint _Current = 0;

        public static uint Current
        {
            get
            {
                return _Current;
            }
            set
            {
                if (value >= 0 && value <= list.Count - 1)
                {
                    _Current = value;
                    MaxIndex = list[(int)value].Length - 1;
                }
            }
        }

        /// <summary>
        /// Maximum index of the Map property
        /// </summary>
        public static int MaxIndex { get; private set; } = list[0].Length - 1;

        public static Color[] Map
        {
            get
            {
                return list[(int)_Current];
            }
        }

        public static Color EncodeRGB(double num)
        {
            if (num <= 0) return new Color(0, 0, 0);
            else if (num >= 1) return new Color(255, 0, 0);

            double y = 0;

            int R;
            if (num < 0.5) R = 0;
            else
            {
                y = 2.0 * num - 1.0;
                R = (int)Math.Round(y * 255.0);
            }

            int G;
            if(num < 0.4)
            {
                y = 2.5 * num;
            }
            else
            {
                y = -5 * num + 3;
            }
            G = (int)Math.Round(y * 255.0);

            int B;
            if(num < 0.5)
            {
                y = 2.5 * num - 0.25;
            }
            else
            {
                y = -2.5 * num + 2.25;
            }
            B = (int)Math.Round(y * 255.0);

            return new Color(R, G, B);
        }
    }

    public class AudioDataAvailableArgs : EventArgs
    {
        public AudioDataAvailableArgs(double peak, double[] channelMagnitudes)
        {
            Peak = peak;
            ChannelMagnitudes = channelMagnitudes;
        }

        public double Peak { get; }
        public double[] ChannelMagnitudes { get; }
    }
}
