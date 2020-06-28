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
    public partial class RoundedCheckBox : UserControl
    {
        public event EventHandler<EventArgs> CheckedStateChanged;

        private bool _checked = false;

        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                if (value == _checked) return;
                _checked = value;
                CheckedStateChanged?.Invoke(this, new EventArgs());
                Refresh();
            }
        }
        public RoundedCheckBox()
        {
            InitializeComponent();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            var pen = new Pen(ForeColor, 2);
            var g = e.Graphics;
            try
            {
                g.DrawArc(pen, new Rectangle(2, 2, 20, 20), -180, 180);
                g.DrawArc(pen, new Rectangle(2, 22, 20, 20), 0, 180);
                g.DrawLine(pen, 3, 11, 3, 33);
                g.DrawLine(pen, 22, 11, 22, 33);

                if (_checked)
                {
                    var b = new SolidBrush(ForeColor);
                    g.FillEllipse(b, new Rectangle(6, 6, 12, 12));
                }
                else
                {
                    pen.Width = 1;
                    g.DrawEllipse(pen, new Rectangle(6, 26, 12, 12));
                }
            }
            catch { }
        }

        private void RoundedCheckBox_Click(object sender, EventArgs e)
        {
            Checked = !_checked;
        }
    }
}
