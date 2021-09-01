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
using Arheisel.Log;

namespace ShineALight
{

    public partial class UCRGBVisualizer : CustomUserControl
    {
        private readonly Audio audio;
        private delegate void UpdateDelegate(AudioDataAvailableArgs e);
        private readonly ArduinoCollection collection;
        public UCRGBVisualizer(ArduinoCollection collection, AudioSettings settings)
        {
            InitializeComponent();
            try
            {
                audio = new Audio(settings);
                audio.Channels = 3;
                autoscalerControl.AutoScaler = audio.autoScaler;
                autoscalerControl.UpdateValues();
                audio.DataAvailable += Audio_DataAvailable;
                audio.StartCapture();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCRGBVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            
            this.collection = collection;
            chLabel.Text = audio.AudioChannels.ToString();
            vuMeterR.Color = Brushes.Red;
            vuMeterR.PeakColor = Brushes.Red;
            vuMeterB.Color = Brushes.Blue;
            vuMeterB.PeakColor = Brushes.Blue;
            vuMeterG.Color = Brushes.Green;
            vuMeterG.PeakColor = Brushes.Green;

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
            try
            {
                audio.DataAvailable -= Audio_DataAvailable;
                audio.StopCapture();
                audio.autoScaler.Stop();
                audio.Dispose();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCRGBVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            Dispose();
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            int r = (int) Math.Round(e.ChannelMagnitudes[0] * 255);
            int b = (int) Math.Round(e.ChannelMagnitudes[1] * 255);
            int g = (int) Math.Round(e.ChannelMagnitudes[2] * 255);
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
