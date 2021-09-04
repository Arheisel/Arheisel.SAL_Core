using SAL_Core.RGB;
using System;
using System.Collections.Generic;


namespace SAL_Core.Ambient
{
    public class EffectDataAvailableArgs : EventArgs
    {
        public readonly List<ChColor> Colors;

        public EffectDataAvailableArgs(List<ChColor> chColors)
        {
            Colors = chColors;
        }
    }
}
