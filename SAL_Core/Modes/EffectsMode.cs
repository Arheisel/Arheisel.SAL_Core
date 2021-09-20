using SAL_Core.Ambient;
using SAL_Core.IO;
using SAL_Core.Settings;

namespace SAL_Core.Modes
{
    public class EffectsMode
    {
        protected readonly IChannelGroup Group;

        public string Name { get { return GetType().Name; } }
        public Effects Effects { get; }

        public EffectsMode(IChannelGroup group, EffectSettings settings)
        {
            Group = group;
            Effects = new Effects(settings, group.ChannelCount);
            Effects.DataAvailable += Effects_DataAvailable;
        }

        private void Effects_DataAvailable(object sender, EffectDataAvailableArgs e)
        {
            OnDataAvailable(e);
        }

        protected virtual void OnDataAvailable(EffectDataAvailableArgs e)
        {
            foreach (var color in e.Colors)
            {
                Group.SetColor(color.Channel, color.Color);
            }
        }
        public void Dispose()
        {
            Effects.DataAvailable -= Effects_DataAvailable;
            Effects.Stop();
            Effects.Dispose();
        }
    }
}
