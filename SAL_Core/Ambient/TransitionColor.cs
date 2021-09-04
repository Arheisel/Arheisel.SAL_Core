using System;


namespace SAL_Core.Ambient
{
    class TransitionColor
    {
        private double m;
        private double b;

        public TransitionColor(double oldValue, double newValue, double steps)
        {
            m = (newValue - oldValue) / (steps - 1.0);
            b = oldValue;
        }

        public int getColorValue(double step)
        {
            return (int)Math.Round(m * step + b);
        }
    }
}
