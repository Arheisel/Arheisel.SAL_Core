using SAL_Core.Ambient;
using SAL_Core.Extensions;
using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.RGB;
using SAL_Core.Settings;

namespace SAL_Core.Modes
{
    public class EffectsVisualizerMode : BaseMode
    {
        private readonly Color[] EffColors;
        public Effects Effects { get; }
        public EffectsVisualizerMode(IChannelGroup group, AudioSettings settings, EffectSettings effectSettings) : base(group, settings)
        {
            EffColors = new Color[group.ChannelCount];
            Effects = new Effects(group, effectSettings);
            Effects.DataAvailable += Effects_DataAvailable;
        }

        private void Effects_DataAvailable(object sender, EffectDataAvailableArgs e)
        {
            foreach (var chColor in e.Colors)
            {
                if (chColor.Channel == 0)
                {
                    EffColors.Populate(chColor.Color);
                }
                else
                {
                    EffColors[chColor.Channel - 1] = chColor.Color;
                }
            }
        }

        protected override void OnDataAvailable(AudioDataAvailableArgs e)
        {
            var colors = new Color[e.ChannelMagnitudes.Length];
            for (int i = 0; i < e.ChannelMagnitudes.Length; i++)
            {
                colors[i] = EffColors[i] * e.ChannelMagnitudes[i];
            }
            Group.SetColor(colors);
        }
        public override void Dispose()
        {
            base.Dispose();
            Effects.DataAvailable -= Effects_DataAvailable;
            Effects.Stop();
            Effects.Dispose();
        }
    }
}
