using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Damez.Log;
using SAL_Core;
using System.Net;

namespace ShineALight
{
    public partial class WiFiSetup : Form
    {
        private Arduino arduino;
        private BackgroundWorker background;
        public WiFiSetup(Arduino arduino)
        {
            InitializeComponent();
            Design.Apply(this);

            this.arduino = arduino;

            background = new BackgroundWorker();
            background.WorkerReportsProgress = false;
            background.WorkerSupportsCancellation = false;
            background.DoWork += new DoWorkEventHandler(Background_DoWork);
            background.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Background_RunWorkerCompleted);

            macTB.Text = arduino.GetMACAddress();
        }

        private void ScanBtn_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("Scanning...");
            scanBtn.Enabled = false;
            background.RunWorkerAsync();
        }

        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = ScanNetworks(worker, e);
        }

        private void Background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listBox1.Items.Clear();
            if (e.Error != null)
            {
                Log.Write(Log.TYPE_ERROR, "WiFiSetup :: " + e.Error.Message + Environment.NewLine + e.Error.StackTrace);
                listBox1.Items.Add(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                listBox1.Items.Add("Cancelled");
            }
            else
            {
                var result = (List<string>)e.Result;
                if (result.Count > 0)
                {
                    foreach(string ssid in result)
                    {
                        listBox1.Items.Add(ssid);
                    }
                }
                else
                {
                    listBox1.Items.Add("No SSIDs Found");
                }
                scanBtn.Enabled = true;
            }
        }

        private List<string> ScanNetworks(BackgroundWorker worker, DoWorkEventArgs e)
        {
            return arduino.ScanNetworks();
        }

        private void SetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var ip = IPAddress.Parse(ipTB.Text);
                var gw = IPAddress.Parse(gwTB.Text);
                var mask = IPAddress.Parse(maskTB.Text);

                arduino.SetWifi(
                    ssidTB.Text,
                    passwdTB.Text,
                    ip.GetAddressBytes(),
                    gw.GetAddressBytes(),
                    mask.GetAddressBytes()
                );
            }
            catch(Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "WiFiSetup :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private void ListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ssidTB.Text = (string)listBox1.SelectedItem;
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            devipTB.Text = arduino.GetIPAddress();
        }
    }
}
