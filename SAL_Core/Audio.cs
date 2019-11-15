using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using AForge.Math;

namespace SAL_Core
{
    class Audio
    {
        private const int minLength = 2;
        private const int maxLength = 16384;

        public delegate void DataAvailableEventHandler(double avgPeak, double[] channelMagnitudes);
        public event DataAvailableEventHandler DataAvailable;

        private readonly MMDeviceEnumerator enumerator;
        private readonly MMDevice device;

        private WasapiLoopbackCapture capture = null;
        private BufferedWaveProvider waveBuffer = null;
        private WaveFormat waveFormat = null;
        private int bytesPerFrame = 1;

        private int _channels = 8;
        private int _minFreq = 50; //Hz
        private int _maxFreq = 2050;

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

        public int MinFreq
        {
            get
            {
                return _minFreq;
            }
            set
            {
                if (value > 0 && value < _maxFreq)
                {
                    _minFreq = value;
                    UpdateOctaveDist();

                }
            }
        }

        public int MaxFreq
        {
            get
            {
                return _maxFreq * 2;
            }
            set
            {
                value = value / 2; //I don't know why this works and I'm too afraid to ask
                if (value > _minFreq && waveFormat != null && value <= waveFormat.SampleRate / 2)
                {
                    _maxFreq = value;
                    divider = (waveFormat.SampleRate / _maxFreq);
                    UpdateOctaveDist();
                }
            }
        }

        private void UpdateOctaveDist()
        {
            _octaveDist = Math.Pow((double)_maxFreq / (double)_minFreq, 1 / (double)_channels);
            _octave = _minFreq * _octaveDist;
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

        public void StartCapture()
        {
            if (capture != null) StopCapture();

            capture = new WasapiLoopbackCapture();
            waveFormat = capture.WaveFormat;
            bytesPerFrame = (capture.WaveFormat.BitsPerSample / 8) * capture.WaveFormat.Channels;
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

        private void Capture_DataAvailable(object sender, WaveInEventArgs e)
        {
            waveBuffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
            ISampleProvider samples = waveBuffer.ToSampleProvider();

            int count = waveBuffer.BufferedBytes / bytesPerFrame;

            if (count < minLength) return;

            if (count > maxLength)
            {
                count = maxLength;
            }

            var frames = new float[count];
            samples.Read(frames, 0, count);

            int n = count;
            if (!Tools.IsPowerOf2(n))
            {
                while (!Tools.IsPowerOf2(n))
                {
                    n &= n - 1;
                }
                //n <<= 1;
            }

            Complex[] complexBuffer = new Complex[n];

            for (int i = 0; i < n; i++)
                complexBuffer[i] = new Complex(frames[i], 0);

            FourierTransform.FFT(complexBuffer, FourierTransform.Direction.Forward);

            double[] transform = new double[Convert.ToInt32(Math.Floor((double)n / divider))];

            double step = (double)waveFormat.SampleRate / (double)n;
            int offset = Convert.ToInt32(_minFreq / step);

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
                channelMagnitudes[i] = max;
                avgPeak += max;
            }
            avgPeak /= _channels;

            DataAvailable?.Invoke(avgPeak, channelMagnitudes);
        }

    }

    class Maps
    {
        private static readonly List<Colors[]> list = new List<Colors[]>()
        {
            new Colors[]
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
            new Colors[]
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

        public static Colors[] Map
        {
            get
            {
                return list[(int)_Current];
            }
        }
    }
}
