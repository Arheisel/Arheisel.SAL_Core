using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using SAL_Core.IO;
using SAL_Core.Settings;
using Arheisel.Log;
using System.Threading;
using SAL_Core.Extensions;
using SAL_Core.IO.Connection;
using SAL_Core.Modes;
using System.Threading.Tasks;

namespace ShineALight
{
    public partial class MainWindow : Form
    {
        private readonly ArduinoCollection arduinoCollection;
        private readonly ProgramSettings settings;
        private delegate void UpdateDelegate();
        private readonly BackgroundWorker background;

        public MainWindow()
        {
            InitializeComponent();
            Design.Apply(this);

            arduinoCollection = SAL_Core.Main.ArduinoCollection;
            arduinoCollection.OnError += ArduinoCollection_OnError;
            arduinoCollection.OnChannelCountChange += ArduinoCollection_OnChannelCountChange;
            settings = SAL_Core.Main.Settings;
            //ModeSelect.SelectedIndex = settings.CurrentMode;

            FormClosed += MainWindow_FormClosed;

            background = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            background.DoWork += new DoWorkEventHandler(Background_DoWork);
            background.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Background_RunWorkerCompleted);
            progressBar1.Style = ProgressBarStyle.Marquee;
            Reverse.Enabled = false;
            MoveUp.Enabled = false;
            MoveDown.Enabled = false;
            RemoveArduino.Enabled = false;
            AddArduino.Enabled = false;
            arduinoList.Items.Add("Connecting...");
            background.RunWorkerAsync();
        }

        private void ArduinoCollection_OnChannelCountChange(object sender, EventArgs e)
        {
            UIUpdate();
        }

        private void ArduinoCollection_OnError(object sender, ArduinoExceptionArgs e)
        {
            MessageBox.Show("Connection lost with " + e.Arduino.Name + ": " + e.Exception.Message, "COM ERROR");
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
                arduinoList.DataSource = arduinoCollection.ToList();
                ModeSelect.SelectedIndex = settings.CurrentMode;
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (background.IsBusy)
                {
                    background.CancelAsync();
                    while (background.IsBusy) Task.Delay(10).Wait();
                }

                if (Main.Panel2.Controls.Count > 0)
                {
                    CustomUserControl cuc = (CustomUserControl)Main.Panel2.Controls[0];
                    cuc.DisposeDeferred();
                }
                Main.Panel2.Controls.Clear();

                SAL_Core.Main.Dispose();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "MainWindow :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message, "ERROR");
            }
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
                arduinoCollection.TurnOff();
                Control control;
                switch (ModeSelect.Text)
                {
                    case "Effects":
                        control = new UCEffects(new EffectsMode(arduinoCollection, settings.Effects));
                        break;
                    case "Music":
                        control = new UCAudio(new MusicMode(arduinoCollection, settings.Music));
                        break;
                    case "RGB Visualizer":
                        control = new UCRGBVisualizer(new RGBVisualizerMode(arduinoCollection, settings.RGBVisualizer));
                        break;
                    case "Visualizer":
                        control = new UCAudio(new VisualizerMode(arduinoCollection, settings.Visualizer));
                        break;
                    case "Musicbar":
                        control = new UCAudio(new MusicbarMode(arduinoCollection, settings.Musicbar));
                        break;
                    case "Musicbar 2":
                        control = new UCAudio(new Musicbar2Mode(arduinoCollection, settings.Musicbar2));
                        break;
                    case "Musicbar 3":
                        control = new UCAudio(new Musicbar3Mode(arduinoCollection, settings.Musicbar2));
                        break;
                    case "Effect Visualizer":
                        control = new UCEffectsVisualizer(new EffectsVisualizerMode(arduinoCollection, settings.EffectsVisualizer, settings.Effects));
                        break;
                    default:
                        control = new UCAudio(new VisualizerMode(arduinoCollection, settings.Visualizer));
                        break;
                }
                Main.Panel2.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                control.Show();
                settings.CurrentMode = ModeSelect.SelectedIndex;
                Design.Apply(Main.Panel2.Controls[0]);
            }
            catch(Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "MainWindow :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void AddArduino_Click(object sender, EventArgs e)
        {
            using (AddArduino dialog = new AddArduino())
            {
                dialog.ShowDialog(this);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    arduinoCollection.Add(dialog.Arduino);
                    arduinoList.DataSource = arduinoCollection.ToList();
                }
            }
        }


        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWindow_FormClosed(this, new FormClosedEventArgs(CloseReason.UserClosing));
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

        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            ConnectToArduinos();
        }

        private void ConnectToArduinos()
        {
            Log.Write(Log.TYPE_INFO, "MainWindow :: Starting initial connect");
            arduinoCollection.InitializeArduinos();
            Log.Write(Log.TYPE_INFO, "MainWindow :: Initial connect ended");
        }

        private void Background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //progressBar1.Visible = false;
            if (e.Error != null)
            {
                Log.Write(Log.TYPE_ERROR, "MainWindow :: " + e.Error.Message + Environment.NewLine + e.Error.StackTrace);
                MessageBox.Show(e.Error.Message, "ERROR");
                arduinoList.Items.Clear();
                arduinoList.Items.Add(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                arduinoList.Items.Clear();
                arduinoList.Items.Add("Cancelled");
            }
            else
            {
                //arduinoList.Items.Clear();
                arduinoList.DataSource = arduinoCollection.ToList();
                Reverse.Enabled = true;
                MoveUp.Enabled = true;
                MoveDown.Enabled = true;
                RemoveArduino.Enabled = true;
                AddArduino.Enabled = true;
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Value = 100;
            }
            //ModeSelect.SelectedIndex = settings.CurrentMode;
        }

        private void Reverse_Click(object sender, EventArgs e)
        {
            if (arduinoList.SelectedItem == null) return;
            Arduino arduino = arduinoList.SelectedItem as Arduino;
            arduino.Settings.Reverse = !arduino.Settings.Reverse;
            arduinoList.DataSource = arduinoCollection.ToList();
            arduinoList.SelectedItem = arduino;
            settings.Save();
        }

        private void RemoveArduino_Click(object sender, EventArgs e)
        {
            if (arduinoList.SelectedItem == null) return;
            Arduino arduino = arduinoList.SelectedItem as Arduino;

            var dialog = MessageBox.Show("Are you sure you want to delete " + arduino.Name + "?", "Confirm Delete", MessageBoxButtons.YesNo);

            if(dialog == DialogResult.Yes)
            {
                arduinoList.Items.Remove(arduino);
                if (arduino.Settings.ConnectionType == ConnectionType.Serial) Program.COMArduinos.Remove(arduino.Settings.COM);
                arduinoCollection.Remove(arduino);
                arduinoList.DataSource = arduinoCollection.ToList();
                settings.Save();
            }
            
        }

        private void MoveUp_Click(object sender, EventArgs e)
        {
            if (arduinoList.SelectedItem == null) return;
            Arduino arduino = arduinoList.SelectedItem as Arduino;
            arduinoCollection.ShiftUp(arduino);
            arduinoList.DataSource = arduinoCollection.ToList();
            arduinoList.SelectedItem = arduino;
            settings.Save();
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
            if (arduinoList.SelectedItem == null) return;
            Arduino arduino = arduinoList.SelectedItem as Arduino;
            arduinoCollection.ShiftDown(arduino);
            arduinoList.DataSource = arduinoCollection.ToList();
            arduinoList.SelectedItem = arduino;
            settings.Save();
        }
    }

    public class CustomUserControl : UserControl
    {
        public virtual void DisposeDeferred()
        {
            
        }

    }
}
