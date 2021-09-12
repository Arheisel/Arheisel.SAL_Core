using System;
using System.Windows.Forms;
using Arheisel.Log;
using SAL_Core.IO;
using SAL_Core.Settings;

namespace ShineALight
{
    public partial class AddArduino : Form
    {
        public Arduino Arduino { get; private set; } = null;
        private AddArduinoSerial serial;
        private AddArduinoTCP tcp;
        public AddArduino()
        {
            InitializeComponent();
            Design.Apply(this);
            serial = new AddArduinoSerial();
            tableLayoutPanel1.Controls.Add(serial);
            serial.Dock = DockStyle.Fill;
            serial.Show();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (SerialRadio.Checked)
            {
                if (!string.IsNullOrWhiteSpace(serial.Port))
                {
                    Arduino = Program.COMArduinos[serial.Port];
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
                if (!string.IsNullOrWhiteSpace(tcp.ip))
                {
                    try
                    {
                        Arduino = new Arduino(new ArduinoSettings(tcp.ip, tcp.port));
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
                tcp = new AddArduinoTCP();
                tableLayoutPanel1.Controls[0].Dispose();
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.Controls.Add(tcp);
                tcp.Dock = DockStyle.Fill;
                tcp.Show();
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
