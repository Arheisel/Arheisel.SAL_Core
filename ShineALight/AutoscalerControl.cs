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

        public AutoScaler AutoScaler { get; set; }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            AutoScaler.Enabled = checkBox1.Checked;
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            AutoScaler.Scale = (double)trackBar1.Value / 100.0;
            label1.Text = AutoScaler.Scale.ToString();
        }

        public void UpdateValues()
        {
            trackBar1.Value = (int)(AutoScaler.Scale * 100.0);
            label1.Text = AutoScaler.Scale.ToString("0.00");
            checkBox1.Checked = AutoScaler.Enabled;
        }
    }
}
