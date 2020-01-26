using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAL_Core;

namespace ShineALight
{
    public partial class UCEffects : CustomUserControl
    {
        private readonly Effects effects;
        public UCEffects(ArduinoCollection collection)
        {
            InitializeComponent();
            effects = new Effects(collection);

            foreach(string name in effects.list.Keys)
            {
                currentSelect.Items.Add(name);
            }
            currentSelect.Text = effects.Current;
            this.Disposed += UCEffects_Disposed;
        }

        private void UCEffects_Disposed(object sender, EventArgs e)
        {
            effects.Speed = 0;
        }

        private void CurrentSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            effects.Current = currentSelect.Text;
        }

        private void SpeedSelect_Scroll(object sender, EventArgs e)
        {
            effects.Speed = speedSelect.Value;
        }
    }
}
