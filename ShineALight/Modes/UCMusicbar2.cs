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

        private readonly ArduinoCollection arduinoCollection;

        public UCMusicbar2(ArduinoCollection collection, AudioSettings settings)
        {
            InitializeComponent();
            arduinoCollection = collection;

            try
            {
                audio = new Audio(settings);
                audio.Channels = collection.ChannelCount * collection.Multiplier;
                audioUI1.Audio = audio;
                audio.DataAvailable += Audio_DataAvailable;
                audio.StartCapture();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCMusicBar2 :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            double div = 1.0 / (double)arduinoCollection.ChannelCount;
            for (int i = 0; i < arduinoCollection.ChannelCount; i++)
            {
                if (e.Peak > div * i) arduinoCollection.SetColor(i + 1, Maps.EncodeRGB(e.Peak >= div * (i + 1) ? div * (i + 1) : e.Peak));
                else arduinoCollection.SetColor(i + 1, Colors.OFF);
            }
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
                Log.Write(Log.TYPE_ERROR, "UCMusic :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            Dispose();
        }

        private void UCMusic_Load(object sender, EventArgs e)
        {

        }

    }
}
