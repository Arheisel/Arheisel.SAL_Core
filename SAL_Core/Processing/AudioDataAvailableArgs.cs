using System;


namespace SAL_Core.Processing
{
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
