using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAL_Core;

namespace ShineALight
{

    public partial class UCRGBVisualizer : CustomUserControl
    {
        private readonly Audio audio;
        private delegate void UpdateDelegate(AudioDataAvailableArgs e);
        private readonly ArduinoCollection collection;
        public UCRGBVisualizer(ArduinoCollection collection)
        {
            InitializeComponent();
            audio = new Audio(collection);
            audio.Channels = 3;
            autoscalerControl.AutoScaler = audio.autoScaler;
            audio.DataAvailable += Audio_DataAvailable;
            audio.StartCapture();
            //curvePlot1.Function = audio.Curve;
            this.collection = collection;
            chLabel.Text = audio.Channels.ToString();
            audio.MaxFreq = 2050;
        }

        public override void DisposeDeferred()
        {
            audio.DataAvailable -= Audio_DataAvailable;
            audio.StopCapture();
            Dispose();
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            int r = (int) Math.Round(e.ChannelMagnitudes[0] * 14);
            int b = (int) Math.Round(e.ChannelMagnitudes[1] * 14);
            int g = (int) Math.Round(e.ChannelMagnitudes[2] * 14);
            collection.SetColor(new SAL_Core.Color(r, g, b));
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
                vuMeterR.Value = e.ChannelMagnitudes[0];
                vuMeterB.Value = e.ChannelMagnitudes[1];
                vuMeterG.Value = e.ChannelMagnitudes[2];
                autoscalerControl.UpdateValues();
            }
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            audio.Slope = slopeTrackbar.Value;
            slopeLabel.Text = slopeTrackbar.Value.ToString();
            //curvePlot1.Refresh();
        }

        private void MinFreqTrackbar_Scroll(object sender, EventArgs e)
        {
            audio.MinFreq = minFreqTrackbar.Value;
            minFreqLabel.Text = minFreqTrackbar.Value.ToString();
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            audio.MaxFreq = maxFreqTrackbar.Value;
            maxFreqLabel.Text = maxFreqTrackbar.Value.ToString();
        }
    }
}
