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

        public double Value
        {
            get
            {
                return _val;
            }

            set
            {
                if(value > 0 && value < 1)
                {
                    _val = value;
                    bitmap = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
                    pictureBox1.Image = bitmap;
                    var g = Graphics.FromImage(bitmap);
                    var h = (float)(pictureBox1.Size.Height * value);
                    g.FillRectangle(Brushes.Lime, 0, pictureBox1.Size.Height - h, pictureBox1.Size.Width, h);
                }
            }
        }
    }
}
