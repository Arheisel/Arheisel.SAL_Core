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
    public partial class AddArduino : Form
    {
        public Arduino arduino = null;
        private AddArduinoSerial serial;
        private AddArduinoUDP udp;
        public AddArduino()
        {
            InitializeComponent();
            serial = new AddArduinoSerial();
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
                    arduino = Program.COMArduinos[serial.port];
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("No device selected", "WARNING");
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
                        Log.Write(Log.TYPE_ERROR, "AddArduino :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                        MessageBox.Show(ex.Message, "ERROR");
                        //DialogResult = DialogResult.Cancel;
                        //Close();
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
                serial.StopDiscover();
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
            serial.StopDiscover();
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CancelButton_Click_1(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
