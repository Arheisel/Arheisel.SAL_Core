using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System;
using System.Collections.Generic;


namespace SAL_Core.Ambient
{
    class Effect
    {
        public EffectPreset Preset { get; set; }
        protected int ChannelCount { get; }
        protected int step = 0;
        protected int count = 0;
        protected readonly List<ChColor> colors = new List<ChColor>();

        public Effect(EffectPreset settings, int channelCount)
        {
            Preset = settings;
            ChannelCount = channelCount;
        }

        public virtual List<ChColor> Step()
        {
            return null;
        }

        public virtual void Reset()
        {
            count = 0;
            step = 0;
            colors.Clear();
        }
    }
}
