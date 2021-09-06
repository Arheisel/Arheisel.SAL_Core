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

            arduinoCollection = Program.arduinoCollection;
            arduinoCollection.OnError += ArduinoCollection_OnError;
            settings = Program.settings;
            //ModeSelect.SelectedIndex = settings.CurrentMode;

            FormClosed += MainWindow_FormClosed;

            background = new BackgroundWorker();
            background.WorkerReportsProgress = true;
            background.WorkerSupportsCancellation = true;
            background.DoWork += new DoWorkEventHandler(Background_DoWork);
            background.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Background_RunWorkerCompleted);
            background.ProgressChanged += new ProgressChangedEventHandler(Background_ProgressChanged);
            progressBar1.Visible = true;
            Reverse.Enabled = false;
            MoveUp.Enabled = false;
            MoveDown.Enabled = false;
            RemoveArduino.Enabled = false;
            AddArduino.Enabled = false;
            arduinoList.Items.Add("Connecting...");
            background.RunWorkerAsync();
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
                arduinoList.Items.Clear();
                foreach (Arduino a in arduinoCollection) arduinoList.Items.Add(a);
                ModeSelect_SelectedIndexChanged(this, new EventArgs());
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (background.IsBusy)
                {
                    background.CancelAsync();
                    while (background.IsBusy) Thread.Sleep(1);
                }

                settings.Save();
                if (Main.Panel2.Controls.Count > 0)
                {
                    CustomUserControl cuc = (CustomUserControl)Main.Panel2.Controls[0];
                    cuc.DisposeDeferred();
                }
                Main.Panel2.Controls.Clear();
                arduinoCollection.TurnOff();
                arduinoCollection.Dispose();
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
                    case "Effect Visualizer":
                        control = new UCEffectsVisualizer(new EffectsVisualizerMode(arduinoCollection, settings.EffectsVisualizer, settings.Effects));
                        break;
                    default:
                        {
                            var mode = new VisualizerMode(arduinoCollection, settings.Visualizer);
                            control = new UCAudio(mode);
                            break;
                        }
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
                    arduinoList.Items.Add(dialog.Arduino);
                    settings.AddArduino(dialog.Arduino);
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

        private void MainWindow_Load(object sender, EventArgs e)
        {
            
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {

        }

        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = ConnectToArduinos(worker, e);
        }

        private void Background_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
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
                arduinoList.Items.Clear();
                var result = (List<Arduino>)e.Result;
                foreach (Arduino arduino in result)
                {
                    arduinoList.Items.Add(arduino);
                }
                Reverse.Enabled = true;
                MoveUp.Enabled = true;
                MoveDown.Enabled = true;
                RemoveArduino.Enabled = true;
                AddArduino.Enabled = true;
            }
            ModeSelect.SelectedIndex = settings.CurrentMode;
        }

        private List<Arduino> ConnectToArduinos(BackgroundWorker worker, DoWorkEventArgs e)
        {
            var list = new List<Arduino>();
            int count = 0;
            Log.Write(Log.TYPE_INFO, "MainWindow :: Starting initial connect");
            foreach (ArduinoSettings arduino in settings.Arduinos)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                if (arduino.ConnectionType == ConnectionType.TCP)
                {
                    try
                    {
                        var dev = new Arduino(arduino, true);
                        if (dev.Online)
                        {
                            arduinoCollection.Add(dev);
                        }
                        list.Add(dev);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(Log.TYPE_ERROR, "MainWindow :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
                else
                {
                    if (!Program.COMArduinos.ContainsKey(arduino.COM))
                    {
                        try
                        {
                            var dev = new Arduino(arduino, true);
                            if (dev.Online)
                            {
                                Program.COMArduinos.Add(dev.Name, dev);
                                arduinoCollection.Add(dev);
                            }
                            list.Add(dev);
                        }
                        catch (Exception ex)
                        {
                            Log.Write(Log.TYPE_ERROR, "MainWindow :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                    else if (!Program.arduinoCollection.Contains(arduino.COM))
                    {
                        arduinoCollection.Add(Program.COMArduinos[arduino.COM]);
                    }
                }
                
                worker.ReportProgress((++count * 100) / settings.Arduinos.Count);
            }

            Log.Write(Log.TYPE_INFO, "MainWindow :: Initial connect ended");
            return list;
        }

        private void Reverse_Click(object sender, EventArgs e)
        {
            if (arduinoList.SelectedItem == null) return;
            Arduino arduino = arduinoList.SelectedItem as Arduino;
            arduino.Settings.Reverse = !arduino.Settings.Reverse;
            arduinoList.Items.Clear();
            foreach (Arduino a in arduinoCollection) arduinoList.Items.Add(a);
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
                settings.Arduinos.Remove(arduino.Settings);
                settings.Save();
            }
            
        }

        private void MoveUp_Click(object sender, EventArgs e)
        {
            if (arduinoList.SelectedItem == null) return;
            Arduino arduino = arduinoList.SelectedItem as Arduino;
            arduinoCollection.ShiftUp(arduino);
            settings.Arduinos.Shift(arduino.Settings, -1);
            arduinoList.Items.Clear();
            foreach(Arduino a in arduinoCollection) arduinoList.Items.Add(a);
            arduinoList.SelectedItem = arduino;
            settings.Save();
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
            if (arduinoList.SelectedItem == null) return;
            Arduino arduino = arduinoList.SelectedItem as Arduino;
            arduinoCollection.ShiftDown(arduino);
            settings.Arduinos.Shift(arduino.Settings, 1);
            arduinoList.Items.Clear();
            foreach (Arduino a in arduinoCollection) arduinoList.Items.Add(a);
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
