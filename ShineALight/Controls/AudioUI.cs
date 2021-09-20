using System.Windows.Forms;
using SAL_Core.Processing;

namespace ShineALight.Controls
{
    public partial class AudioUI : UserControl
    {
        private Audio _audio = null;
        private delegate void UpdateDelegate(AudioDataAvailableArgs e);
        public AudioUI()
        {
            InitializeComponent();
        }

        public Audio Audio
        {
            get
            {
                return _audio;
            }
            set
            {
                if (value == null) return;
                _audio = value;

                autoscalerControl.AutoScaler = _audio.autoScaler;
                autoscalerControl.UpdateValues();
                audioControls.Audio = _audio;
                vuPanel.Channels = _audio.ChannelCount;
                _audio.DataAvailable += _audio_DataAvailable;
            }
        }

        private void _audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            UIUpdate(e);
        }

        private void UIUpdate(AudioDataAvailableArgs e)
        {
            if (InvokeRequired)
            {
                var d = new UpdateDelegate(UIUpdate);
                try
                {
                    Invoke(d, new object[] { e });
                }
                catch { }; //Raises an exception when I close the program because *of course* the target doesn't fucking exist anymore.
            }
            else
            {
                vuPanel.UIUpdate(e);
                autoscalerControl.UpdateValues();
            }
        }
    }
}
