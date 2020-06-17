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
using System.Threading;

namespace ShineALight
{
    public partial class MainWindow : Form
    {
        private readonly ArduinoCollection arduinoCollection;
        private readonly Settings settings;
        private delegate void UpdateDelegate();
        private readonly BackgroundWorker background;

        public MainWindow()
        {
            InitializeComponent();

            arduinoCollection = Program.arduinoCollection;
            arduinoCollection.OnError += ArduinoCollection_OnError;
            settings = Program.settings;
            ModeSelect.SelectedIndex = settings.CurrentMode;

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
            ArduinoList.Items.Add("Connecting...", false);
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
                ArduinoList.Refresh();
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
                arduinoCollection.SetColor(Colors.OFF);
                Thread.Sleep(10);
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
                arduinoCollection.SetColor(Colors.OFF);
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
                    case "Musicbar":
                        control = new UCMusicbar(arduinoCollection, settings.Musicbar);
                        Main.Panel2.Controls.Add(control);
                        control.Dock = DockStyle.Fill;
                        control.Show();
                        break;
                    case "Musicbar 2":
                        control = new UCMusicbar2(arduinoCollection, settings.Musicbar2);
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
            using (AddArduino dialog = new AddArduino())
            {
                dialog.ShowDialog(this);
                if (dialog.DialogResult == DialogResult.OK)
                {
                    arduinoCollection.Add(dialog.arduino);
                    ArduinoList.Items.Add(dialog.arduino, false);
                    settings.AddArduino(dialog.arduino);
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
                ArduinoList.Items.Clear();
                ArduinoList.Items.Add(e.Error.Message, false);
            }
            else if (e.Cancelled)
            {
                ArduinoList.Items.Clear();
                ArduinoList.Items.Add("Cancelled", false);
            }
            else
            {
                ArduinoList.Items.Clear();
                var result = (List<Arduino>)e.Result;
                foreach (Arduino arduino in result)
                {
                    ArduinoList.Items.Add(arduino);
                }
                Reverse.Enabled = true;
                MoveUp.Enabled = true;
                MoveDown.Enabled = true;
                RemoveArduino.Enabled = true;
                AddArduino.Enabled = true;
            }
        }

        private List<Arduino> ConnectToArduinos(BackgroundWorker worker, DoWorkEventArgs e)
        {
            var list = new List<Arduino>();
            int count = 0;
            Log.Write(Log.TYPE_INFO, "MainWindow :: Starting serial connect");
            foreach (ArduinoSettings arduino in settings.Arduinos)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                if (arduino.ConnectionType == ConnectionType.UDP) continue;

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
                else if (!Program.arduinoCollection.ContainsArduino(arduino.COM))
                {
                    arduinoCollection.Add(Program.COMArduinos[arduino.COM]);
                }

                worker.ReportProgress((++count * 100) / settings.Arduinos.Count);
            }

            Log.Write(Log.TYPE_INFO, "MainWindow :: Serial connect ended");
            return list;
        }

        private void Reverse_Click(object sender, EventArgs e)
        {
            foreach(Arduino arduino in ArduinoList.CheckedItems.OfType<Arduino>())
            {
                arduino.Settings.Reverse = !arduino.Settings.Reverse;
            }
            ArduinoList.Refresh();
        }

        private void RemoveArduino_Click(object sender, EventArgs e)
        {
            foreach (Arduino arduino in ArduinoList.CheckedItems.OfType<Arduino>().ToList())
            {
                ArduinoList.Items.Remove(arduino);
                if(arduino.Settings.ConnectionType == ConnectionType.Serial) Program.COMArduinos.Remove(arduino.Settings.COM);
                arduinoCollection.Remove(arduino);
                settings.Arduinos.Remove(arduino.Settings);
            }
            ArduinoList.Refresh();
        }
    }

    public class CustomUserControl : UserControl
    {
        public virtual void DisposeDeferred()
        {
            
        }

    }
}
