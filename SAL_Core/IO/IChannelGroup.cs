using SAL_Core.RGB;
using System;
using System.Collections.Generic;


namespace SAL_Core.IO
{
    public interface IChannelGroup
    {
        int ChannelCount { get; }
        int Multiplier { get; }
        void SetColor(Color color);
        void SetColor(int channel, Color color);
        void SetColor(Color[] colors);
    }
}
