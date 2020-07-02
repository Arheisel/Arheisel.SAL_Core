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
    public partial class AutoscalerControl : UserControl
    {
        public AutoscalerControl()
        {
            InitializeComponent();
        }

        public AutoScaler AutoScaler { get; set; } = null;

        public void UpdateValues()
        {
            amp.Value = AutoScaler.Amp;
            scale.Value = (int)(AutoScaler.Scale * 100.0 / amp.Value);
            enabled.Checked = AutoScaler.Enabled;
        }

        private void Scale_ValueChanged(object sender, EventArgs e)
        {
            if (AutoScaler == null) return;
            AutoScaler.Scale = (double)scale.Value / 100.0 * amp.Value;
        }

        private void RoundedCheckBox1_CheckedStateChanged(object sender, EventArgs e)
        {
            if (AutoScaler == null) return;
            AutoScaler.Enabled = enabled.Checked;
        }

        private void Amp_ValueChanged(object sender, EventArgs e)
        {
            if (AutoScaler == null) return;
            AutoScaler.Amp = amp.Value;
            AutoScaler.Scale = (double)scale.Value / 100.0 * amp.Value;
        }
    }
}
