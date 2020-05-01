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

            FormClosed += MainWindow_FormClosed;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ModeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Main.Panel2.Controls.Count > 0)
            {
                CustomUserControl cuc = (CustomUserControl)Main.Panel2.Controls[0];
                cuc.DisposeDeferred();
            }
            Main.Panel2.Controls.Clear();

            Control control;
            switch (ModeSelect.Text)
            {
                case "Effects":
                    control = new UCEffects(arduinoCollection);
                    Main.Panel2.Controls.Add(control);
                    control.Dock = DockStyle.Fill;
                    control.Show();
                    break;
                case "Music":
                    control = new UCMusic(arduinoCollection);
                    Main.Panel2.Controls.Add(control);
                    control.Dock = DockStyle.Fill;
                    control.Show();
                    break;
                case "RGB Visualizer":
                    control = new UCRGBVisualizer(arduinoCollection);
                    Main.Panel2.Controls.Add(control);
                    control.Dock = DockStyle.Fill;
                    control.Show();
                    break;
            }
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

    public class CustomUserControl : UserControl
    {
        public virtual void DisposeDeferred()
        {
            
        }

    }
}
