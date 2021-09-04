using System;
using System.Windows.Forms;
using SAL_Core.Ambient;
using Arheisel.Log;
using SAL_Core.IO;
using SAL_Core.Settings;
using SAL_Core.RGB;

namespace ShineALight
{
    public partial class UCEffects : CustomUserControl
    {
        private readonly Effects effects;
        private readonly ArduinoCollection arduinoCollection;
        private readonly Color[] colorBuffer;
        public UCEffects(ArduinoCollection collection, EffectSettings settings)
        {
            InitializeComponent();
            try
            {
                arduinoCollection = collection;
                colorBuffer = new Color[collection.ChannelCount];
                for (int i = 0; i < collection.ChannelCount; i++){
                    colorBuffer[i] = new Color(0, 0, 0);
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
