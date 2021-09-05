using SAL_Core.RGB;


namespace SAL_Core.IO
{
    class ArduinoGroup
    {
        private ArduinoCollection collection;
        private int start;
        private int length;

        public ArduinoGroup(ArduinoCollection arduinoCollection, int start, int length)
        {
            collection = arduinoCollection;
            this.start = start;
            this.length = length;
        }

        public void SetColor(int channel, Color color)
        {

        }

        public void SetColor(Color color)
        {

        }

        public void SetColor(Color[] colors)
        {

        }
    }
}
