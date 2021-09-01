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
    public partial class UCEffects : CustomUserControl
    {
        private readonly Effects effects;
        private readonly ArduinoCollection arduinoCollection;
        private readonly SAL_Core.Color[] colorBuffer;
        public UCEffects(ArduinoCollection collection, EffectSettings settings)
        {
            InitializeComponent();
            try
            {
                arduinoCollection = collection;
                colorBuffer = new SAL_Core.Color[collection.ChannelCount];
                for (int i = 0; i < collection.ChannelCount; i++){
                    colorBuffer[i] = new SAL_Core.Color(0, 0, 0);
                }
                effects = new Effects(collection, settings);
                effects.DataAvailable += Effects_DataAvailable;
                effectsControl.Effects = effects;
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCEffects :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void Effects_DataAvailable(object sender, EffectDataAvailableArgs e)
        {
            foreach (var color in e.Colors)
            {
                arduinoCollection.SetColor(color.Channel, color.Color); //colorBuffer[color.Channel - 1] = color.Color;
            }

            /*if (e.Colors.Count == 1)
            {
                arduinoCollection.SetColor(e.Colors[0].Channel, e.Colors[0].Color);
            }
            else
            {
                foreach (var color in e.Colors)
                {
                    colorBuffer[color.Channel - 1] = color.Color;
                }
                //arduinoCollection.SetColor(colorBuffer);
            }*/
        }

        public override void DisposeDeferred()
        {
            effects.Stop();
            effects.Dispose();
            Dispose();
        }
    }
}
