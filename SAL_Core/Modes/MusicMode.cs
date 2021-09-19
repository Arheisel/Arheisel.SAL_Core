using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.RGB;
using SAL_Core.Settings;

namespace SAL_Core.Modes
{
    public class MusicMode : BaseMode
    {
        public MusicMode(IChannelGroup group, AudioSettings settings) : base(group, settings) { }

        protected override void OnDataAvailable(AudioDataAvailableArgs e)
        {
            Group.SetColor(Maps.EncodeRGB(e.Peak));
        }
    }
}
