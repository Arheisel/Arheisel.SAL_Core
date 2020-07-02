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
    public partial class UCEffects : CustomUserControl
    {
        private readonly Effects effects;
        private readonly ArduinoCollection arduinoCollection;
        public UCEffects(ArduinoCollection collection, EffectSettings settings)
        {
            InitializeComponent();
            try
            {
                arduinoCollection = collection;
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
                arduinoCollection.SetColor(color.Channel, color.Color);
            }
        }

        public override void DisposeDeferred()
        {
            effects.Stop();
            effects.Dispose();
            Dispose();
        }
    }
}
