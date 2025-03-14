﻿using System;
using System.Windows.Forms;
using SAL_Core.Processing;
using SAL_Core.Ambient;
using SAL_Core.Settings;
using SAL_Core.IO;
using Arheisel.Log;
using SAL_Core.RGB;
using SAL_Core.Modes;

namespace ShineALight
{

    public partial class UCEffectsVisualizer : CustomUserControl
    {
        private readonly EffectsVisualizerMode Mode;

        public UCEffectsVisualizer(EffectsVisualizerMode mode)
        {
            InitializeComponent();

            try
            {
                Mode = mode;
                audioUI.Audio = mode.Audio;
                effectsControl1.Effects = mode.Effects;
            }
            catch (Exception ex)
            {
                Log.Write(Log.TYPE_ERROR, "UCEffectsVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
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
                Log.Write(Log.TYPE_ERROR, "UCEffectsVisualizer :: " + ex.Message + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("ERROR: " + ex.Message);
            }
            Dispose();
        }
    }
}
