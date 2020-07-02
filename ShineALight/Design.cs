using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ShineALight
{
    static class Design
    {
        public static Color BgColor { get; } = Color.FromArgb(43, 48, 54);
        public static Color BtnColor { get; } = Color.FromArgb(53, 59, 66);
        public static Color TextColor { get; } = Color.FromArgb(173, 193, 217);
        public static Color DetailColor { get; } = Color.FromArgb(237, 99, 40);

        public static void Apply(Control control)
        {
            if (control is Button)
            {
                var c = control as Button;
                c.BackColor = BtnColor;
                c.ForeColor = TextColor;
                c.FlatStyle = FlatStyle.Flat;
                c.FlatAppearance.BorderColor = DetailColor;
                c.FlatAppearance.BorderSize = 1;
            }
            else if (control is TrackBar)
            {
                var c = control as TrackBar;
                c.BackColor = BgColor;
            }
            else if (control is ComboBox)
            {
                var c = control as ComboBox;
                c.BackColor = BtnColor;
                c.ForeColor = TextColor;
                c.FlatStyle = FlatStyle.Flat;
            }
            else
            {
                control.BackColor = BgColor;
                control.ForeColor = TextColor;
            }
            foreach (Control ctrl in control.Controls)
            {
                Apply(ctrl);
            }
        }
    }
}
