using System;
using System.Windows.Forms;
using SAL_Core.Ambient;
using Arheisel.Log;
using SAL_Core.IO;
using SAL_Core.Settings;
using SAL_Core.RGB;
using SAL_Core.Modes;

namespace ShineALight
{
    public partial class UCEffects : CustomUserControl
    {
        private readonly EffectsMode Mode;
        public UCEffects(EffectsMode mode)
        {
            InitializeComponent();
            try
            {
                Mode = mode;
                effectsControl.Effects = mode.Effects;
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCEffects :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        public override void DisposeDeferred()
        {
            Mode.Dispose();
            Dispose();
        }
    }
}
