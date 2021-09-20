using System;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using SAL_Core.IO.Connection;
using System.Linq;
using SAL_Core;

namespace ShineALight
{
    public partial class AddArduinoTCP : UserControl
    {
        
        public string ip = "";
        public int port = 7990;

        private delegate void UpdateDelegate();
        public AddArduinoTCP()
        {
            InitializeComponent();
            Design.Apply(this);
            Main.UDPServer.OnNewArduinoDetected += UdpServer_OnNewArduinoDetected;
            var ipList = Main.UDPServer.DetectedArduinos.Values.Select(p => p.Address.ToString()).ToList();
            detectedList.DataSource = ipList;
            Main.UDPServer.Start();
        }

        private void UdpServer_OnNewArduinoDetected(object sender, EventArgs e)
        {
            UIUpdate();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            ip = textBox1.Text;
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            port = (int)numericUpDown1.Value;
        }

        private void DetectedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = (string)detectedList.SelectedItem;
        }

        private void UIUpdate()
        {
            if (InvokeRequired)
            {
                var d = new UpdateDelegate(UIUpdate);
                try
                {
                    Invoke(d, new object[] { });
                }
                catch { }; //Raises an exception when I close the program because *of course* the target doesn't fucking exist anymore.
            }
            else
            {
                var ipList = Main.UDPServer.DetectedArduinos.Values.Select(p => p.Address.ToString()).ToList();
                detectedList.DataSource = ipList;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Main.UDPServer.Stop();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

    }

    public class ArduinoDetectedArgs : EventArgs
    {
        public string IP { get; set; }

        public ArduinoDetectedArgs(string ip)
        {
            IP = ip;
        }
    }
}
