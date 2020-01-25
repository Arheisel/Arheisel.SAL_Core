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

namespace ShineALight
{
    public partial class AddArduinoSerial : UserControl
    {
        public string port = "";
        public AddArduinoSerial()
        {
            InitializeComponent();
            var names = SerialPort.GetPortNames();
            foreach(string name in names)
            {
                comboBox1.Items.Add(name);
            }

            if(comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
                port = comboBox1.Text;
            }
            else
            {
                comboBox1.Enabled = false;
                comboBox1.Text = "No COM Ports";
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            port = comboBox1.Text;
        }
    }
}
