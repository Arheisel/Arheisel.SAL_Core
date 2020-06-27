using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SAL_Core;
using Damez.Log;

namespace ShineALight
{
    public partial class UCMusicbar2 : CustomUserControl
    {
        private readonly Audio audio;
        private delegate void UpdateDelegate(AudioDataAvailableArgs e);

        private readonly ArduinoCollection arduinoCollection;

        public UCMusicbar2(ArduinoCollection collection, VSettings settings)
        {
            InitializeComponent();
            arduinoCollection = collection;

            try
            {
                audio = new Audio(settings);
                audio.Channels = 1;
                autoscalerControl1.AutoScaler = audio.autoScaler;
                autoscalerControl1.UpdateValues();
                audio.DataAvailable += Audio_DataAvailable;
                audio.StartCapture();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCMusicBar2 :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            slopeTrackbar.Value = audio.Slope;
            slopeLabel.Text = slopeTrackbar.Value.ToString();
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            double div = 1.0 / (double)arduinoCollection.ChannelCount;
            for (int i = 0; i < arduinoCollection.ChannelCount; i++)
            {
                if (e.Peak > div * i) arduinoCollection.SetColor(i + 1, Maps.EncodeRGB(e.Peak >= div * (i + 1) ? div * (i + 1) : e.Peak));
                else arduinoCollection.SetColor(i + 1, Colors.OFF);
            }
            UIUpdate(e);
        }

        public override void DisposeDeferred()
        {
            try
            {
                audio.DataAvailable -= Audio_DataAvailable;
                audio.StopCapture();
                autoscalerControl1.AutoScaler.Stop();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCMusic :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            Dispose();
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
                vuMeter1.Value = e.Peak;
                autoscalerControl1.UpdateValues();
            }
        }

        private void UCMusic_Load(object sender, EventArgs e)
        {

        }

        private void slopeTrackbar_Scroll(object sender, EventArgs e)
        {
            audio.Slope = slopeTrackbar.Value;
            slopeLabel.Text = slopeTrackbar.Value.ToString();
        }
    }
}
