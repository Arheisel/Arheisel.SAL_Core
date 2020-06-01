﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using AForge.Math;

namespace SAL_Core
{
    public class Audio
    {
        private const int minLength = 2;
        private const int maxLength = 16384;

        public event EventHandler<AudioDataAvailableArgs> DataAvailable;

        private readonly MMDeviceEnumerator enumerator;
        private readonly MMDevice device;

        private WasapiLoopbackCapture capture = null;
        private BufferedWaveProvider waveBuffer = null;
        private WaveFormat waveFormat = null;
        private int bytesPerFrame = 1;
        private readonly VSettings Settings;

        public readonly AutoScaler autoScaler;

        private int _channels = 8;

        private int divider = 1;
        private double _octave = 0;
        private double _octaveDist = 1.5;

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
                Settings.Slope = value;
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
            /*Thread thread = new Thread(new ThreadStart(Capture));
            thread.Start();*/
            enumerator = new MMDeviceEnumerator();
            device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        }

        public Audio(VSettings settings)
        {
            Settings = settings;
            autoScaler = new AutoScaler(settings.Autoscaler);
        }

        public void StartCapture()
        {
            if (capture != null) StopCapture();

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

        public void StopCapture()
        {
            if (capture == null) return;

            capture.StopRecording();
            capture.Dispose();
            capture = null;
        }

        public double Curve(double x)
        {
            return Math.Log10(x) + (Settings.Slope / 10);//Math.Log10(x) + (Slope / 10); //Math.Log10(x*10) * (10/slope); //Math.Log10(x) + (slope / 10); //(Math.Log(x, 2) + slope) / 10;
        }

        private void Capture_DataAvailable(object sender, WaveInEventArgs e)
        {
            Task.Run(() => ProcessAudioData(e));
        }

        private void ProcessAudioData(WaveInEventArgs e)
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
                    for(int j = 0; j < waveFormat.Channels; j++)
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

            double octave = _octave;
            double avgPeak = 0;

            for (int i = 0; i < _channels; i++)
            {
                double max = 0;
                for (int j = startingPoint; j * step + offset < octave && j < transform.Length; j++) //startingPoint + nFreqs
                {
                    if (transform[j] > max) max = transform[j];
                    startingPoint++;
                }
                octave *= _octaveDist;
                //channelMagnitudes[i] = max;
                avgPeak += max;

                double x = Convert.ToDouble(max * autoScaler.Scale);
                double res = Curve(x);
                autoScaler.Sample(res);
                if (res > 1) res = 1;
                else if (res < 0) res = 0;
                channelMagnitudes[i] = res;
            }
            avgPeak /= _channels;

            DataAvailable?.Invoke(this, new AudioDataAvailableArgs(avgPeak, channelMagnitudes));
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
            if(num < 0.2)
            {
                y = 5.0 * num;
            }
            else
            {
                y = -1.81 * num + 1.36;
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
        public AudioDataAvailableArgs(double avgPeak, double[] channelMagnitudes)
        {
            AvgPeak = avgPeak;
            ChannelMagnitudes = channelMagnitudes;
        }

        public double AvgPeak { get; }
        public double[] ChannelMagnitudes { get; }
    }
}
