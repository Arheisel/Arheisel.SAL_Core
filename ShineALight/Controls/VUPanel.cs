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
    public partial class VUPanel : UserControl
    {
        private VUMeter[] meters;
        private int _channels = 0;
        public VUPanel()
        {
            InitializeComponent();
        }

        public int Channels
        {
            get
            {
                return _channels;
            }
            set
            {
                if (value <= 0) return;
                _channels = value;
                panel.Controls.Clear();
                meters = new VUMeter[_channels];
                var margin = panel.Size.Width / _channels > 10 ? 5:0;
                var channelWidth = (panel.Size.Width - margin) / _channels;
                var meterWidth = channelWidth - margin > 0 ? channelWidth - margin:1;
                var height = panel.Size.Height;
                for (int i = 0; i < _channels; i++)
                {
                    var meter = new VUMeter
                    {
                        Size = new Size(meterWidth, height),
                        Location = new Point((channelWidth * i) + margin, 0)
                    };
                    panel.Controls.Add(meter);
                    meters[i] = meter;
                }
            }
        }

        public void UIUpdate(AudioDataAvailableArgs e)
        {
            if (_channels != e.ChannelMagnitudes.Length) return;
            for (int i = 0; i < e.ChannelMagnitudes.Length; i++)
            {
                meters[i].Value = e.ChannelMagnitudes[i];
            }
        }

        private void VUPanel_Resize(object sender, EventArgs e)
        {
            Channels = _channels;
        }
    }
}
