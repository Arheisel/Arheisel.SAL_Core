using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShineALight
{
    public partial class AddArduinoUDP : UserControl
    {
        public string ip = "";
        public int port = 9090;
        public AddArduinoUDP()
        {
            InitializeComponent();
            Design.Apply(this);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            ip = textBox1.Text;
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            port = (int)numericUpDown1.Value;
        }
    }
}
