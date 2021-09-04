using SAL_Core.RGB;

namespace SAL_Core.Ambient
{
    class Transition
    {
        private TransitionColor R;
        private TransitionColor G;
        private TransitionColor B;

        public Transition(Color oldColor, Color newColor, int steps)
        {
            R = new TransitionColor(oldColor.R, newColor.R, steps);
            G = new TransitionColor(oldColor.G, newColor.G, steps);
            B = new TransitionColor(oldColor.B, newColor.B, steps);
        }

        public Color getColor(int step)
        {
            return new Color(R.getColorValue(step), G.getColorValue(step), B.getColorValue(step));
        }
    }
}
