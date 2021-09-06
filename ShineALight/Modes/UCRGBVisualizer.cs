using System;
using System.Drawing;
using System.Windows.Forms;
using Arheisel.Log;
using SAL_Core.IO;
using SAL_Core.Processing;
using SAL_Core.Settings;
using SAL_Core.Modes;

namespace ShineALight
{

    public partial class UCRGBVisualizer : CustomUserControl
    {
        private readonly RGBVisualizerMode Mode;
        private readonly Audio Audio;
        private delegate void UpdateDelegate(AudioDataAvailableArgs e);
        public UCRGBVisualizer(RGBVisualizerMode mode)
        {
            InitializeComponent();
            try
            {
                Mode = mode;
                Audio = mode.Audio;
                autoscalerControl.AutoScaler = Audio.autoScaler;
                autoscalerControl.UpdateValues();
                Audio.DataAvailable += Audio_DataAvailable;
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCRGBVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            chLabel.Text = Audio.AudioChannels.ToString();
            vuMeterR.Color = Brushes.Red;
            vuMeterR.PeakColor = Brushes.Red;
            vuMeterB.Color = Brushes.Blue;
            vuMeterB.PeakColor = Brushes.Blue;
            vuMeterG.Color = Brushes.Green;
            vuMeterG.PeakColor = Brushes.Green;

            slopeTrackbar.Value = Audio.Slope;
            slopeLabel.Text = slopeTrackbar.Value.ToString();

            minFreqTrackbar.Value = Audio.MinFreq;
            minFreqLabel.Text = minFreqTrackbar.Value.ToString();
            maxFreqTrackbar.Minimum = Audio.MinFreq * 2;

            maxFreqTrackbar.Value = Audio.MaxFreq;
            maxFreqLabel.Text = maxFreqTrackbar.Value.ToString();
            minFreqTrackbar.Maximum = Audio.MaxFreq / 2;
        }

        public override void DisposeDeferred()
        {
            try
            {
                Mode.Dispose();
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
            Audio.Slope = slopeTrackbar.Value;
            slopeLabel.Text = slopeTrackbar.Value.ToString();
            //curvePlot1.Refresh();
        }

        private void MinFreqTrackbar_Scroll(object sender, EventArgs e)
        {
            Audio.MinFreq = minFreqTrackbar.Value;
            minFreqLabel.Text = minFreqTrackbar.Value.ToString();
            maxFreqTrackbar.Minimum = Audio.MinFreq * 2;
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            Audio.MaxFreq = maxFreqTrackbar.Value;
            maxFreqLabel.Text = maxFreqTrackbar.Value.ToString();
            minFreqTrackbar.Maximum = Audio.MaxFreq / 2;
        }
    }
}
