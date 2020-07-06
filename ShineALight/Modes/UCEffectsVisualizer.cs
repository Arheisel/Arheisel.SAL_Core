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
        private readonly ArduinoCollection collection;
        private readonly SAL_Core.Color[] effColors;

        public UCEffectsVisualizer(ArduinoCollection collection, AudioSettings settings, EffectSettings effectSettings)
        {
            InitializeComponent();

            var channels = collection.ChannelCount;
            effColors = new SAL_Core.Color[channels];

            try
            {
                effects = new Effects(collection, effectSettings);
                effects.DataAvailable += Effects_DataAvailable;

                audio = new Audio(settings);
                audio.Channels = channels * collection.Multiplier;
                audioUI.Audio = audio;
                audio.DataAvailable += Audio_DataAvailable;
                audio.StartCapture();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
            effectsControl1.Effects = effects;

            this.collection = collection;
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
                for(int j = 0; j < collection.Multiplier; j++)
                {
                    if (e.ChannelMagnitudes[i * collection.Multiplier + j] > peak) peak = e.ChannelMagnitudes[i * collection.Multiplier + j];
                }
                colors[i] = effColors[i] * peak;
            }
            collection.SetColor(colors);
        }
    }
}
