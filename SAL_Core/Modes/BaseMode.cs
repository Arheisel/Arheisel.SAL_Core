using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.Settings;

namespace SAL_Core.Modes
{
    public abstract class BaseMode : IAudioMode
    {
        protected readonly IChannelGroup Group;

        public string Name { get { return GetType().Name; } }
        public Audio Audio { get; }

        public BaseMode(IChannelGroup group, AudioSettings settings)
        {
            Group = group;
            Audio = new Audio(settings, group.ChannelCount * group.Multiplier);
            Audio.DataAvailable += Audio_DataAvailable; ;
            Audio.StartCapture();
        }

        protected virtual void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            double[] magnitudes = new double[Group.ChannelCount];

            for(int i = 0; i < magnitudes.Length; i++)
            {
                double max = 0;
                for (int j = 0; j < Group.Multiplier; j++)
                {
                    if (e.ChannelMagnitudes[i * Group.Multiplier + j] > max) max = e.ChannelMagnitudes[i * Group.Multiplier + j];
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
