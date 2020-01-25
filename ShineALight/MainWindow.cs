using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShineALight
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            ModeSelect.SelectedIndex = 0;
            var eff = new EffectsForm();
            eff.TopLevel = false;
            Main.Panel2.Controls.Add(eff);
            eff.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            eff.Dock = DockStyle.Fill;
            eff.Show();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
