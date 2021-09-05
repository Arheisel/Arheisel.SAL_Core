using System;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using SAL_Core.IO.Connection;

namespace ShineALight
{
    public partial class AddArduinoTCP : UserControl
    {
        
        public string ip = "";
        public int port = 7990;

        private delegate void UpdateDelegate(ArduinoDetectedArgs e);
        private Thread thread;
        private UDPServer udpServer;
        public AddArduinoTCP()
        {
            InitializeComponent();
            Design.Apply(this);
            udpServer = new UDPServer(port);
            thread = new Thread(new ThreadStart(Worker));
            thread.Start();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            ip = textBox1.Text;
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            port = (int)numericUpDown1.Value;
        }

        private void detectedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = (string)detectedList.SelectedItem;
        }

        private void UIUpdate(ArduinoDetectedArgs e)
        {
            if (InvokeRequired)
            {
                var d = new UpdateDelegate(UIUpdate);
                try
                {
                    Invoke(d, new object[] { e });
                }
                catch { }; //Raises an exception when I close the program because *of course* the target doesn't fucking exist anymore.
            }
            else
            {
                detectedList.Items.Add(e.IP);
            }
        }

        private void Worker()
        {
            var data = udpServer.Receive(out IPEndPoint endPoint, true);
            if(data.Length == 2 && data[0] == 252)
            {
                if(data[1] == 0xFA)
                {
                    UIUpdate(new ArduinoDetectedArgs(endPoint.Address.ToString()));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            
            if (disposing)
            {
                if(components != null) components.Dispose();
                thread.Abort();
                udpServer.Dispose();
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
