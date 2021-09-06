using SAL_Core.Processing;

namespace SAL_Core.Modes
{
    public interface IAudioMode
    {
        string Name { get; }
        Audio Audio { get; }
        void Dispose();
    }
}
