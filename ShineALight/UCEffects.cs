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

namespace ShineALight
{
    public partial class UCEffects : CustomUserControl
    {
        private readonly Effects effects;
        public UCEffects(ArduinoCollection collection)
        {
            InitializeComponent();
            effects = new Effects(collection);

            foreach(string name in effects.list.Keys)
            {
                currentSelect.Items.Add(name);
            }
            currentSelect.Text = effects.Current;
        }

        public override void DisposeDeferred()
        {
            effects.Speed = 0;
            Dispose();
        }

        private void CurrentSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            effects.Current = currentSelect.Text;
        }

        private void SpeedTrackbar_Scroll(object sender, EventArgs e)
        {
            effects.Speed = speedTrackbar.Value;
            speedLabel.Text = speedTrackbar.Value.ToString();
        }

        private void StepsTrackbar_Scroll(object sender, EventArgs e)
        {
            effects.Steps = stepsTrackbar.Value;
            stepsLabel.Text = stepsTrackbar.Value.ToString();
        }
    }
}
