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

    public partial class UCVisualizer : CustomUserControl
    {
        private readonly Audio audio;
        private delegate void UpdateDelegate(AudioDataAvailableArgs e);
        private readonly ArduinoCollection collection;
        public UCVisualizer(ArduinoCollection collection, VSettings settings)
        {
            InitializeComponent();
            audio = new Audio(settings);
            audio.Channels = 4;
            autoscalerControl.AutoScaler = audio.autoScaler;
            autoscalerControl.UpdateValues();
            audio.DataAvailable += Audio_DataAvailable;
            audio.StartCapture();
            //curvePlot1.Function = audio.Curve;
            this.collection = collection;
            chLabel.Text = audio.AudioChannels.ToString();
            audio.MaxFreq = 4100;

            slopeTrackbar.Value = audio.Slope;
            slopeLabel.Text = slopeTrackbar.Value.ToString();

            minFreqTrackbar.Value = audio.MinFreq;
            minFreqLabel.Text = minFreqTrackbar.Value.ToString();
            maxFreqTrackbar.Minimum = audio.MinFreq * 2;

            maxFreqTrackbar.Value = audio.MaxFreq;
            maxFreqLabel.Text = maxFreqTrackbar.Value.ToString();
            minFreqTrackbar.Maximum = audio.MaxFreq / 2;
        }

        public override void DisposeDeferred()
        {
            audio.DataAvailable -= Audio_DataAvailable;
            audio.StopCapture();
            autoscalerControl.AutoScaler.Stop();
            Dispose();
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            for(int i = 0; i < e.ChannelMagnitudes.Length; i++)
            {
                collection.SetColor(i + 1, Maps.EncodeRGB(e.ChannelMagnitudes[i]));
            }
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
                vuMeter1.Value = e.ChannelMagnitudes[0];
                vuMeter2.Value = e.ChannelMagnitudes[1];
                vuMeter3.Value = e.ChannelMagnitudes[2];
                vuMeter4.Value = e.ChannelMagnitudes[3];
                autoscalerControl.UpdateValues();
            }
        }

        private void slopeTrackbar_Scroll(object sender, EventArgs e)
        {
            audio.Slope = slopeTrackbar.Value;
            slopeLabel.Text = slopeTrackbar.Value.ToString();
            //curvePlot1.Refresh();
        }

        private void MinFreqTrackbar_Scroll(object sender, EventArgs e)
        {
            audio.MinFreq = minFreqTrackbar.Value;
            minFreqLabel.Text = minFreqTrackbar.Value.ToString();
            maxFreqTrackbar.Minimum = audio.MinFreq * 2;
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            audio.MaxFreq = maxFreqTrackbar.Value;
            maxFreqLabel.Text = maxFreqTrackbar.Value.ToString();
            minFreqTrackbar.Maximum = audio.MaxFreq / 2;
        }
    }
}
