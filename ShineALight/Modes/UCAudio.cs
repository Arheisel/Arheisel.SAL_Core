using System;
using System.Windows.Forms;
using Arheisel.Log;
using SAL_Core.Modes;

namespace ShineALight
{

    public partial class UCAudio : CustomUserControl
    {
        private readonly IAudioMode Mode;
        public UCAudio(IAudioMode mode)
        {
            InitializeComponent();

            try
            {
                Mode = mode;
                audioUI1.Audio = mode.Audio;
            }
            catch (Exception ex)
            {
                Log.Error($"UCVisualizer :: {mode.Name} :: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                MessageBox.Show("ERROR: " + ex.Message);
            }

        }

        public override void DisposeDeferred()
        {
            try
            {
                Mode.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error($"UCVisualizer :: {Mode.Name} :: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                MessageBox.Show("ERROR: " + ex.Message);
            }
            Dispose();
        }
    }
}
