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
        private readonly Music music;
        private delegate void UpdateDelegate(MusicDataAvailableArgs e);

        private readonly ArduinoCollection arduinoCollection;

        public UCMusicbar2(ArduinoCollection collection, MusicSettings settings)
        {
            InitializeComponent();
            arduinoCollection = collection;

            try
            {
                music = new Music(settings);
                autoscalerControl1.AutoScaler = music.AutoScaler;
                autoscalerControl1.UpdateValues();
                music.DataAvailable += Music_DataAvailable;
                music.Run();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCMusic :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            curvePlot1.Function = music.Curve;
            slopeTrackbar.Value = music.Settings.Slope;
            slopeLabel.Text = slopeTrackbar.Value.ToString();
            curvePlot1.Refresh();
        }

        public override void DisposeDeferred()
        {
            try
            {
                music.DataAvailable -= Music_DataAvailable;
                music.Stop();
                autoscalerControl1.AutoScaler.Stop();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCMusic :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            Dispose();
        }

        private void Music_DataAvailable(object sender, MusicDataAvailableArgs e)
        {
            double div = 1.0 / (double)arduinoCollection.ChannelCount;
            for(int i = 0; i < arduinoCollection.ChannelCount; i++)
            {
                if(e.Sample > div*i) arduinoCollection.SetColor(i + 1, Maps.EncodeRGB(e.Sample >= div * (i + 1) ? div*(i + 1):e.Sample));
                else arduinoCollection.SetColor(i + 1, Colors.OFF);
            }
            UIUpdate(e);
        }

        private void UIUpdate(MusicDataAvailableArgs e)
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
                vuMeter1.Value = e.Sample;
                autoscalerControl1.UpdateValues();
            }
        }

        private void UCMusic_Load(object sender, EventArgs e)
        {

        }

        private void slopeTrackbar_Scroll(object sender, EventArgs e)
        {
            music.Settings.Slope = slopeTrackbar.Value;
            slopeLabel.Text = slopeTrackbar.Value.ToString();
            curvePlot1.Refresh();
        }
    }
}
