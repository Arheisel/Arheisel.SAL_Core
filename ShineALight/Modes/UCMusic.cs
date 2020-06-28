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
    public partial class UCMusic : CustomUserControl
    {
        private readonly Audio audio;
        private delegate void UpdateDelegate(MusicDataAvailableArgs e);

        private readonly ArduinoCollection arduinoCollection;

        public UCMusic(ArduinoCollection collection, AudioSettings settings)
        {
            InitializeComponent();
            arduinoCollection = collection;

            try
            {

                audio = new Audio(settings);
                audio.Channels = collection.ChannelCount * collection.Multiplier;
                audioUI.Audio = audio;
                audio.DataAvailable += Audio_DataAvailable;
                audio.StartCapture();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCMusic :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            audio.MaxFreq = settings.MaxFreq;
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            arduinoCollection.SetColor(Maps.EncodeRGB(e.Peak));
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
    }
}
