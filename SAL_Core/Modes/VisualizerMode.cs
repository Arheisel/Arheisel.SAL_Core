using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.RGB;
using SAL_Core.Settings;

namespace SAL_Core.Modes
{
    public class VisualizerMode : BaseMode
    {
        public VisualizerMode(IChannelGroup group, AudioSettings settings) : base(group, settings) { }

        protected override void OnDataAvailable(AudioDataAvailableArgs e)
        {
            var colors = new Color[e.ChannelMagnitudes.Length];
            for (int i = 0; i < e.ChannelMagnitudes.Length; i++)
            {
                colors[i] = Maps.EncodeRGB(e.ChannelMagnitudes[i]);
            }
            Group.SetColor(colors);
        }
    }
}
