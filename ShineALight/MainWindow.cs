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
using Damez.Log;

namespace ShineALight
{
    public partial class MainWindow : Form
    {
        private readonly ArduinoCollection arduinoCollection;
        private readonly Settings settings;
        private delegate void UpdateDelegate();

        public MainWindow()
        {
            InitializeComponent();

            arduinoCollection = Program.arduinoCollection;
            arduinoCollection.OnError += ArduinoCollection_OnError;
            settings = Program.settings;
            ModeSelect.SelectedIndex = settings.CurrentMode;

            FormClosed += MainWindow_FormClosed;
        }

        private void ArduinoCollection_OnError(object sender, ArduinoExceptionArgs e)
        {
            MessageBox.Show("Connection lost with " + e.Arduino.Name + ": " + e.Exception.Message, "COM ERROR");
            UIUpdate();
        }

        private void UIUpdate()
        {
            if (InvokeRequired)
            {
                var d = new UpdateDelegate(UIUpdate);
                try
                {
                    Invoke(d);
                }
                catch { }; //Raises an exception when I close the program because *of course* the target doesn't fucking exist anymore.
            }
            else
            {
                ArduinoList.Refresh();
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ModeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
                        control = new UCEffects(arduinoCollection, settings.Effects);
                        Main.Panel2.Controls.Add(control);
                        control.Dock = DockStyle.Fill;
                        control.Show();
                        break;
                    case "Music":
                        control = new UCMusic(arduinoCollection, settings.Music);
                        Main.Panel2.Controls.Add(control);
                        control.Dock = DockStyle.Fill;
                        control.Show();
                        break;
                    case "RGB Visualizer":
                        control = new UCRGBVisualizer(arduinoCollection, settings.RGBVisualizer);
                        Main.Panel2.Controls.Add(control);
                        control.Dock = DockStyle.Fill;
                        control.Show();
                        break;
                    case "Visualizer":
                        control = new UCVisualizer(arduinoCollection, settings.Visualizer);
                        Main.Panel2.Controls.Add(control);
                        control.Dock = DockStyle.Fill;
                        control.Show();
                        break;
                }
                settings.CurrentMode = ModeSelect.SelectedIndex;
            }
            catch(Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "MainWindow :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message, "ERROR");
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
                   ArduinoList.Items.Add(window.arduino, false);
                }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                settings.Save();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "MainWindow :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            using (AddArduino window = new AddArduino())
            {
                window.ShowDialog(this);
                if (window.DialogResult == DialogResult.OK)
                {
                    arduinoCollection.Add(window.arduino);
                    ArduinoList.Items.Add(window.arduino, false);
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
