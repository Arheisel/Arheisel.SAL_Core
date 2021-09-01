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

namespace ShineALight.Controls
{
    public partial class EffectsControl : UserControl
    {
        private Effects _effects = null;

        public EffectsControl()
        {
            InitializeComponent();
        }

        public Effects Effects
        {
            get
            {
                return _effects;
            }
            set
            {
                if (value == null) return;
                _effects = value;

                foreach (string name in _effects.Settings.PresetList.Keys)
                {
                    currentSelect.Items.Add(name);
                }
                currentSelect.SelectedItem = _effects.Current;
            }
        }

        private void UpdateScrollbars()
        {
            var preset = _effects.Settings.CurrentPreset;

            speed.Value = preset.Speed;
            steps.Value = preset.TotalSteps;
            hold.Value = preset.HoldingSteps;
            reverse.Checked = preset.Reverse;
        }


        private void CurrentSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _effects.Current = currentSelect.Text;
                UpdateScrollbars();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "EffectsControl :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            using (PresetsEdit dialog = new PresetsEdit(_effects.Settings.PresetList))
            {
                dialog.ShowDialog(this);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    _effects.Settings.PresetList = dialog.PresetList;
                    if(!_effects.Settings.PresetList.ContainsKey(_effects.Current))
                        _effects.Current = _effects.Settings.PresetList.First().Key;
                    currentSelect.Items.Clear();
                    foreach (string name in _effects.Settings.PresetList.Keys)
                    {
                        currentSelect.Items.Add(name);
                    }
                    currentSelect.SelectedItem = _effects.Current;
                    UpdateScrollbars();
                    try
                    {
                        Program.settings.Save();
                    }
                    catch (Exception ex)
                    {
                        Log.Write(Log.TYPE_ERROR, "EEffectsControl :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                        MessageBox.Show(ex.Message, "ERROR");
                    }
                }
            }
        }

        private void Speed_ValueChanged(object sender, EventArgs e)
        {
            if (_effects.Settings.CurrentPreset != null)
                _effects.Speed = speed.Value;
        }

        private void Steps_ValueChanged(object sender, EventArgs e)
        {
            if (_effects.Settings.CurrentPreset != null)
                _effects.Steps = steps.Value;
        }

        private void Hold_ValueChanged(object sender, EventArgs e)
        {
            if (_effects.Settings.CurrentPreset != null)
                _effects.HoldSteps = hold.Value;
        }

        private void Reverse_CheckedStateChanged(object sender, EventArgs e)
        {
            if (_effects.Settings.CurrentPreset != null)
                _effects.Settings.CurrentPreset.Reverse = reverse.Checked;
        }
    }
}
