using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using SAL_Core;
using Damez.Log;
using System.Threading;

namespace ShineALight
{
    public partial class AddArduinoSerial : UserControl
    {
        public string port = "";

        private readonly BackgroundWorker background;
        public AddArduinoSerial()
        {
            InitializeComponent();
            Design.Apply(this);
            background = new BackgroundWorker();
            background.WorkerReportsProgress = true;
            background.WorkerSupportsCancellation = true;
            background.DoWork += new DoWorkEventHandler(Background_DoWork);
            background.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Background_RunWorkerCompleted);
            background.ProgressChanged += new ProgressChangedEventHandler(Background_ProgressChanged);
            StartDiscover();
        }

        public void StartDiscover()
        {
            statusLabel.Text = "Scanning for devices...";
            progressBar1.Visible = true;
            comboBox1.Enabled = false;
            background.RunWorkerAsync();
        }

        public void StopDiscover()
        {
            if (background.IsBusy)
            {
                background.CancelAsync();
                statusLabel.Text = "Stopping...";
                while (background.IsBusy) Thread.Sleep(1);
            }
        }

        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = DiscoverArduinos(worker, e);
        }

        private void Background_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //progressBar1.Visible = false;
            if(e.Error != null)
            {
                Log.Write(Log.TYPE_ERROR, "AddArduinoSerial :: " + e.Error.Message + Environment.NewLine + e.Error.StackTrace);
                statusLabel.Text = e.Error.Message;
            }
            else if (e.Cancelled)
            {
                statusLabel.Text = "Cancelled";
            }
            else
            {
                var result = (List<string>)e.Result;
                if (result.Count > 0)
                {
                    comboBox1.DataSource = result;
                    comboBox1.SelectedIndex = 0;
                    port = comboBox1.Text;
                    statusLabel.Text = "Ready";
                    comboBox1.Enabled = true;
                    wifiSetupBtn.Enabled = true;
                }
                else
                {
                    statusLabel.Text = "No COM Ports";
                }
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            port = comboBox1.Text;
        }

        private void AddArduinoSerial_Load(object sender, EventArgs e)
        {
            
        }

        private List<string> DiscoverArduinos(BackgroundWorker worker, DoWorkEventArgs e)
        {
            var list = new List<string>();
            int count = 0;
            Log.Write(Log.TYPE_INFO, "AddArduinoSerial :: Starting serial  discovery");
            var names = SerialPort.GetPortNames();
            var taskList = new List<Task<Arduino>>();
            var completedTasks = new List<Task<Arduino>>();
            foreach (string name in names)
            {
                if (!Program.COMArduinos.ContainsKey(name))
                {
                    taskList.Add(Task.Run(() => DiscoverPort(name)));
                }
                else if (!Program.arduinoCollection.Contains(name))
                {
                    list.Add(name);
                }
            }

            while (true)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                foreach (Task<Arduino> task in taskList)
                {
                    if (task.IsCompleted)
                    {
                        if (task.Exception != null)
                        {
                            Log.Write(Log.TYPE_ERROR, "AddArduinoSerial :: " + task.Exception.Message + Environment.NewLine + task.Exception.StackTrace);
                        }
                        else if (task.Result != null)
                        {
                            Program.COMArduinos.Add(task.Result.Name, task.Result);
                            list.Add(task.Result.Name);
                        }
                        completedTasks.Add(task);
                        worker.ReportProgress((++count * 100) / names.Length);
                    }
                }
                foreach (Task<Arduino> task in completedTasks)
                {
                    taskList.Remove(task);
                }
                completedTasks.Clear();
                if (taskList.Count == 0) break;
                Thread.Sleep(10);
            }
            
            Log.Write(Log.TYPE_INFO, "AddArduinoSerial :: Serial discovery ended");
            return list;
        }

        private Arduino DiscoverPort(string port)
        {
            try
            {
                return new Arduino(port);
            }
            catch
            {
                return null;
            }
        }

        private void WifiSetupBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new WiFiSetup(Program.COMArduinos[port]))
            {
                dialog.ShowDialog(this);
            }
        }
    }
}
