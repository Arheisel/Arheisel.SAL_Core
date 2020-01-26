using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SAL_Core;

namespace ShineALight
{
    public partial class UCMusic : CustomUserControl
    {
        private readonly Music music;
        private delegate void UpdateDelegate(DataAvailableArgs e);

        public UCMusic(ArduinoCollection collection)
        {
            InitializeComponent();
            music = new Music(collection);
            music.DataAvailable += Music_DataAvailable;
            music.Run();

        }

        public override void DisposeDeferred()
        {
            music.DataAvailable -= Music_DataAvailable;
            music.Stop();
            Dispose();
        }

        private void Music_DataAvailable(object sender, DataAvailableArgs e)
        {
            //volumeBar.Value = (int)e.Sample * 100; //This is not working at all
            UIUpdate(e);
        }

        private void UIUpdate(DataAvailableArgs e)
        {
            if (InvokeRequired)
            {
                var d = new UpdateDelegate(UIUpdate);
                try
                {
                    Invoke(d, new object[] { e });
                }
                catch { }; //Raises an exception when I close the program because *of course* the target doesn't fucking exist anymore.
            }
            else
            {
                vuMeter1.Value = e.Sample;
            }
        }
    }
}
