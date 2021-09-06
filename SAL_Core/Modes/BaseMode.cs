using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.Settings;

namespace SAL_Core.Modes
{
    public abstract class BaseMode : IAudioMode
    {
        protected readonly ArduinoCollection Collection;

        public string Name { get { return GetType().Name; } }
        public Audio Audio { get; }

        public BaseMode(ArduinoCollection collection, AudioSettings settings)
        {
            Collection = collection;
            Audio = new Audio(settings);
            Audio.Channels = collection.ChannelCount * collection.Multiplier;
            Audio.DataAvailable += Audio_DataAvailable; ;
            Audio.StartCapture();
        }

        protected virtual void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            double[] magnitudes = new double[Collection.ChannelCount];

            for(int i = 0; i < magnitudes.Length; i++)
            {
                double max = 0;
                for (int j = 0; j < Collection.Multiplier; j++)
                {
                    if (e.ChannelMagnitudes[i * Collection.Multiplier + j] > max) max = e.ChannelMagnitudes[i * Collection.Multiplier + j];
                }
                magnitudes[i] = max;
            }

            OnDataAvailable(new AudioDataAvailableArgs(e.Peak, magnitudes));
        }

        protected abstract void OnDataAvailable(AudioDataAvailableArgs e);

        public virtual void Dispose()
        {
            Audio.DataAvailable -= Audio_DataAvailable;
            Audio.StopCapture();
            Audio.autoScaler.Stop();
            Audio.Dispose();
        }
    }
}
