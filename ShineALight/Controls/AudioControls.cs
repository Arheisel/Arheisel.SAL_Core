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

namespace ShineALight.Controls
{
    public partial class AudioControls : UserControl
    {
        private Audio _audio = null;
        public AudioControls()
        {
            InitializeComponent();
        }

        public Audio Audio
        {
            get
            {
                return _audio;
            }
            set
            {
                _audio = value;
                if (_audio == null) return;
                minFreq.Value = _audio.MinFreq;
                maxFreq.Value = _audio.MaxFreq;
                minFreq.Max = _audio.MaxFreq / 2;
                maxFreq.Min = _audio.MinFreq * 2;
                slope.Value = _audio.Slope;
            }
        }

        private void MinFreq_ValueChanged(object sender, EventArgs e)
        {
            if (_audio == null) return;
            _audio.MinFreq = minFreq.Value;
            maxFreq.Min = _audio.MinFreq * 2;
        }

        private void MaxFreq_ValueChanged(object sender, EventArgs e)
        {
            if (_audio == null) return;
            _audio.MaxFreq = maxFreq.Value;
            minFreq.Max = _audio.MaxFreq / 2;
        }

        private void Slope_ValueChanged(object sender, EventArgs e)
        {
            if (_audio == null) return;
            _audio.Slope = slope.Value;
        }
    }
}
