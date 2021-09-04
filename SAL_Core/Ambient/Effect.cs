using SAL_Core.IO;
using SAL_Core.RGB;
using SAL_Core.Settings;
using System;
using System.Collections.Generic;


namespace SAL_Core.Ambient
{
    class Effect : IDisposable
    {
        protected readonly ArduinoCollection arduino;
        public EffectPreset Preset { get; set; }
        protected int step = 0;
        protected int count = 0;
        protected readonly List<ChColor> colors = new List<ChColor>();

        public Effect(ArduinoCollection collection, EffectPreset settings)
        {
            Preset = settings;
            arduino = collection;
            arduino.SetColor(Colors.OFF);
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

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }
    }
}
