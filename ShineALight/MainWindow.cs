using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAL_Core;

namespace ShineALight
{
    public partial class MainWindow : Form
    {
        private readonly ArduinoCollection arduinoCollection;
        
        public MainWindow()
        {
            InitializeComponent();

            arduinoCollection = new ArduinoCollection();

            ModeSelect.SelectedIndex = 0;
            var eff = new UCEffects();
            Main.Panel2.Controls.Add(eff);
            eff.Dock = DockStyle.Fill;
            eff.Show();
        }

        private void ModeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddArduino_Click(object sender, EventArgs e)
        {
            using (AddArduino window = new AddArduino())
            {
                window.ShowDialog(this);
                if (window.DialogResult == DialogResult.OK)
                {
                    arduinoCollection.Add(window.arduino);
                    ArduinoList.Items.Add(window.arduino.Name, false);
                }
            }
        }
    }
}
