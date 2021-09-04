using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShineALight
{
    public partial class Knob : UserControl
    {
        public event EventHandler<EventArgs> ValueChanged;

        private bool drag = false;
        private int dragStartingX = 0;
        private int dragStartingVal = 0;

        private int _value = 0;
        private int _min = 0;
        private int _max = 100;

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == _value) return;
                if (value >= _min && value <= _max)
                {
                    _value = value;
                    Refresh();
                }
            }
        }

        public int Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
                if (_value < _min) Value = _min;
            }
        }

        public int Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                if (_value > _max) Value = _max;
            }
        }

        public int Step { get; set; } = 1;

        public string Title { get; set; } = string.Empty;

        public bool DrawTitle { get; set; } = true;
        public bool DrawValue { get; set; } = true;

        public Knob()
        {
            InitializeComponent();
            MouseWheel += Knob_MouseWheel;
            _value = Min;
        }

        

        protected override void OnPaint(PaintEventArgs e)
        {
            float angle = 270 * (float)(_value - _min) / (_max - _min);

            var pen = new Pen(ForeColor, 4);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            var g = e.Graphics;
            var size = e.ClipRectangle.Width < e.ClipRectangle.Height ? e.ClipRectangle.Width : e.ClipRectangle.Height;

            g.DrawEllipse(pen, new Rectangle(15, 15, size - 30, size - 30));
            try
            {
                g.DrawArc(pen, new Rectangle(4, 4, size - 8, size - 8), -225, angle);
            }
            catch { }


            var radius = size / 2 - 25;
            var center = size / 2;
            var rads = ((-225.0 + angle)) * Math.PI / 180.0;

            var lineX = center + (radius * Math.Cos(rads));
            var lineY = center + (radius * Math.Sin(rads));
            var lineX2 = center + ( (radius - 1) * Math.Cos(rads));    
            var lineY2 = center + ( (radius - 1) * Math.Sin(rads));

            g.DrawLine(pen, (float)lineX, (float)lineY, (float)lineX2, (float)lineY2);

            StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Center
            };

            if(DrawValue) g.DrawString(_value.ToString(), Font, new SolidBrush(ForeColor), new Point(center, e.ClipRectangle.Height - Font.Height * 2 - 5), sf);

            if (DrawTitle)
            {
                Font titleFont = new Font(Font, FontStyle.Bold);
                g.DrawString(Title, titleFont, new SolidBrush(ForeColor), new Point(center, e.ClipRectangle.Height - Font.Height), sf);
            }
        }

        private void Knob_MouseDown(object sender, MouseEventArgs e)
        {
            if (Enabled && e.Button == MouseButtons.Left)
            {
                drag = true;
                dragStartingX = e.X;
                dragStartingVal = _value;
            }
        }

        private void Knob_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) drag = false;
        }

        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (!drag) return;

            int delta = e.X - dragStartingX;
            if (delta == 0) return;

            Value = dragStartingVal + delta;
            ValueChanged?.Invoke(this, new EventArgs());
        }

        private void Knob_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!Enabled) return;

            Value += e.Delta / 120 * Step;
            ValueChanged?.Invoke(this, new EventArgs());
        }

        private void Knob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Down) Value -= Step;
            else if(e.KeyCode == Keys.Right || e.KeyCode == Keys.Up) Value += Step;
            ValueChanged?.Invoke(this, new EventArgs());
        }
    }
}
