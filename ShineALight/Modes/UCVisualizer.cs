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

    public partial class UCVisualizer : CustomUserControl
    {
        private readonly Audio audio;
        private readonly ArduinoCollection collection;
        public UCVisualizer(ArduinoCollection collection, AudioSettings settings)
        {
            InitializeComponent();


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
                Log.Write(Log.TYPE_ERROR, "UCVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            this.collection = collection;

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
                Log.Write(Log.TYPE_ERROR, "UCVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
            Dispose();
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            var colors = new SAL_Core.Color[collection.ChannelCount];
            for (int i = 0; i < collection.ChannelCount; i++)
            {
                double peak = 0;
                for (int j = 0; j < collection.Multiplier; j++)
                {
                    if (e.ChannelMagnitudes[i * collection.Multiplier + j] > peak) peak = e.ChannelMagnitudes[i * collection.Multiplier + j];
                }
                colors[i] = Maps.EncodeRGB(peak);
                //collection.SetColor(i + 1, Maps.EncodeRGB(peak));
            }
            collection.SetColor(colors);
        }
    }
}
