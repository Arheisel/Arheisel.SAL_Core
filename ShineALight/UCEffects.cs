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
        public UCEffects(ArduinoCollection collection, EffectSettings settings)
        {
            InitializeComponent();
            try
            {
                effects = new Effects(collection, settings);
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCEffects :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            foreach(string name in effects.Settings.PresetList.Keys)
            {
                currentSelect.Items.Add(name);
            }
            currentSelect.Text = effects.Current;
            UpdateScrollbars();
        }

        public override void DisposeDeferred()
        {
            effects.Stop();
            Dispose();
        }

        private void CurrentSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                effects.Current = currentSelect.Text;
                UpdateScrollbars();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCEffects :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
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

        private void HoldTrackbar_Scroll(object sender, EventArgs e)
        {
            effects.HoldSteps = holdTrackbar.Value;
            holdLabel.Text = holdTrackbar.Value.ToString();
        }

        private void UpdateScrollbars()
        {
            var preset = effects.Settings.CurrentPreset;

            speedTrackbar.Value = preset.Speed;
            speedLabel.Text = speedTrackbar.Value.ToString();

            stepsTrackbar.Value = preset.TotalSteps;
            stepsLabel.Text = stepsTrackbar.Value.ToString();

            holdTrackbar.Value = preset.HoldingSteps;
            holdLabel.Text = holdTrackbar.Value.ToString();
        }
    }
}
