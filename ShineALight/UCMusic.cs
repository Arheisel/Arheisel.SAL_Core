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

namespace ShineALight
{
    public partial class UCMusic : CustomUserControl
    {
        private readonly Music music;
        private delegate void UpdateDelegate(MusicDataAvailableArgs e);

        public UCMusic(ArduinoCollection collection)
        {
            InitializeComponent();
            music = new Music(collection);
            autoscalerControl1.AutoScaler = music.AutoScaler;
            music.DataAvailable += Music_DataAvailable;
            music.Run();
            curvePlot1.Function = music.Curve;
        }

        public override void DisposeDeferred()
        {
            music.DataAvailable -= Music_DataAvailable;
            music.Stop();
            Dispose();
        }

        private void Music_DataAvailable(object sender, MusicDataAvailableArgs e)
        {
            //volumeBar.Value = (int)e.Sample * 100; //This is not working at all
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

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            music.Data.Slope = trackBar1.Value;
            slopeLabel.Text = trackBar1.Value.ToString();
            curvePlot1.Refresh();
        }
    }
}
