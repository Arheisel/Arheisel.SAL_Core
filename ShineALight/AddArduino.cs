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
    public partial class AddArduino : Form
    {
        public Arduino arduino = null;
        private AddArduinoSerial serial;
        private AddArduinoUDP udp;
        public AddArduino()
        {
            InitializeComponent();
            var serial = new AddArduinoSerial();
            tableLayoutPanel1.Controls.Add(serial);
            serial.Dock = DockStyle.Fill;
            serial.Show();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (SerialRadio.Checked)
            {
                if (!String.IsNullOrWhiteSpace(serial.port))
                {
                    try
                    {
                        arduino = new Arduino(serial.port);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }
                }
            }
            else if (UDPRadio.Checked)
            {
                if (!String.IsNullOrWhiteSpace(udp.ip))
                {
                    try
                    {
                        arduino = new Arduino(udp.ip, udp.port);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }
                }
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void UDPRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UDPRadio.Checked)
            {
                udp = new AddArduinoUDP();
                tableLayoutPanel1.Controls[0].Dispose();
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.Controls.Add(udp);
                udp.Dock = DockStyle.Fill;
                udp.Show();
            }
        }

        private void SerialRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (SerialRadio.Checked)
            {
                serial = new AddArduinoSerial();
                tableLayoutPanel1.Controls[0].Dispose();
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.Controls.Add(serial);
                serial.Dock = DockStyle.Fill;
                serial.Show();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
