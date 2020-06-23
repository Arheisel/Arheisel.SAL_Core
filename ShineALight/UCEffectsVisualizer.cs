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
using Damez.Log;

namespace ShineALight
{

    public partial class UCEffectsVisualizer : CustomUserControl
    {
        private readonly Audio audio;
        private readonly Effects effects;
        private delegate void UpdateDelegate(AudioDataAvailableArgs e);
        private readonly ArduinoCollection collection;
        private readonly VUMeter[] meters;
        private readonly SAL_Core.Color[] effColors;

        public UCEffectsVisualizer(ArduinoCollection collection, VSettings settings, EffectSettings effectSettings)
        {
            InitializeComponent();

            var channels = collection.ChannelCount;
            meters = new VUMeter[channels];
            var margin = 5;
            var totalWidth = vuPanel.Size.Width - margin;
            var channelWidth = totalWidth / channels;
            var meterWidth = channelWidth - margin;
            var height = vuPanel.Size.Height;
            for(int i = 0; i < channels; i++)
            {
                var meter = new VUMeter
                {
                    Size = new Size(meterWidth, height),
                    Location = new Point((channelWidth * i) + margin, 0)
                };
                vuPanel.Controls.Add(meter);
                meters[i] = meter;
            }

            effColors = new SAL_Core.Color[channels];

            try
            {
                effects = new Effects(collection, effectSettings);
                effects.DataAvailable += Effects_DataAvailable;

                audio = new Audio(settings);
                audio.Channels = channels;
                autoscalerControl.AutoScaler = audio.autoScaler;
                autoscalerControl.UpdateValues();
                audio.DataAvailable += Audio_DataAvailable;
                audio.StartCapture();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            foreach (string name in effects.Settings.PresetList.Keys)
            {
                currentSelect.Items.Add(name);
            }
            currentSelect.SelectedItem = effects.Current;

            //curvePlot1.Function = audio.Curve;
            this.collection = collection;
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

        private void Effects_DataAvailable(object sender, EffectDataAvailableArgs e)
        {
            foreach(var chColor in e.Colors)
            {
                if(chColor.Channel == 0)
                {
                    for (int i = 0; i < effColors.Length; i++) effColors[i] = chColor.Color;
                }
                else
                {
                    effColors[chColor.Channel - 1] = chColor.Color;
                }
            }
        }

        public override void DisposeDeferred()
        {
            try
            {
                audio.DataAvailable -= Audio_DataAvailable;
                audio.StopCapture();
                autoscalerControl.AutoScaler.Stop();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
            Dispose();
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            for(int i = 0; i < e.ChannelMagnitudes.Length; i++)
            {
                collection.SetColor(i + 1, effColors[i] * e.ChannelMagnitudes[i]);
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
                for (int i = 0; i < e.ChannelMagnitudes.Length; i++)
                {
                    meters[i].Value = e.ChannelMagnitudes[i];
                }
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

        private void CurrentSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                effects.Current = currentSelect.Text;
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCEffectsVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }
    }
}
