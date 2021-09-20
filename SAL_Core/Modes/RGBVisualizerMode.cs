using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System;

namespace SAL_Core.Modes
{
    public class RGBVisualizerMode : BaseMode
    {
        public RGBVisualizerMode(IChannelGroup group, AudioSettings settings) : base(group, settings)
        {
            Audio.ChannelCount = 3;
        }

        protected override void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            OnDataAvailable(e);
        }

        protected override void OnDataAvailable(AudioDataAvailableArgs e)
        {
            int r = (int)Math.Round(e.ChannelMagnitudes[0] * 255);
            int b = (int)Math.Round(e.ChannelMagnitudes[1] * 255);
            int g = (int)Math.Round(e.ChannelMagnitudes[2] * 255);
            Group.SetColor(new Color(r, g, b));
        }
    }
}
