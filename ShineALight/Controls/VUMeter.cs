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
    public partial class VUMeter : UserControl
    {
        private double _val;
        Bitmap bitmap;
        public VUMeter()
        {
            InitializeComponent();
        }

        public Brush Color { get; set; } = Brushes.Lime;
        public Brush PeakColor { get; set; } = Brushes.Red;

        public double Value
        {
            get
            {
                return _val;
            }

            set
            {
                if(value <= 0)
                {
                    pictureBox1.Image = null;
                    _val = 0;
                }
                else
                {
                    bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
                    pictureBox1.Image = bitmap;
                    var g = Graphics.FromImage(bitmap);
                    if (value < 0.9)
                    {
                        var h = (float)(pictureBox1.Size.Height * value);
                        g.FillRectangle(Color, 0, pictureBox1.Size.Height - h, pictureBox1.Size.Width, h);
                    }
                    else
                    {
                        g.FillRectangle(PeakColor, 0, 0, pictureBox1.Size.Width, pictureBox1.Size.Height);
                        _val = 1;
                    }
                }
            }
        }
    }
}
