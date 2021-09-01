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
using Arheisel.Log;

namespace ShineALight
{
    public partial class UCMusicbar : CustomUserControl
    {
        private readonly Audio audio;

        private readonly ArduinoCollection arduinoCollection;

        private List<SAL_Core.Color> colorList;
        private int current = 0;
        private bool currentSet = false;

        public UCMusicbar(ArduinoCollection collection, AudioSettings settings)
        {
            InitializeComponent();
            arduinoCollection = collection;
            colorList = new List<SAL_Core.Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.GREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK };
            try
            {
                audio = new Audio(settings);
                audio.Channels = collection.ChannelCount * collection.Multiplier;
                audioUI1.Audio = audio;
                audio.DataAvailable += Audio_DataAvailable;
                audio.StartCapture();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCMusicBar :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void Audio_DataAvailable(object sender, AudioDataAvailableArgs e)
        {
            if (e.Peak >= 0.95 && !currentSet)
            {
                if (current >= colorList.Count - 1) current = 0;
                else current++;
                currentSet = true;
            }
            if (e.Peak < 0.90 && currentSet) currentSet = false;

            double div = 1.0 / (double)arduinoCollection.ChannelCount;
            var colors = new SAL_Core.Color[arduinoCollection.ChannelCount];
            for (int i = 0; i < arduinoCollection.ChannelCount; i++)
            {
                if (e.Peak > div * i) colors[i] = colorList[current] * e.Peak;
                else colors[i] = Colors.OFF;
            }
            arduinoCollection.SetColor(colors);
        }

        public override void DisposeDeferred()
        {
            try
            {
                audio.DataAvailable -= Audio_DataAvailable;
                audio.StopCapture();
                audio.autoScaler.Stop();
                audio.Dispose();
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCMusic :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }

            Dispose();
        }
    }
}
