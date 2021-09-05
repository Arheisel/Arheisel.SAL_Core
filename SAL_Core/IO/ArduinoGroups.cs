using System.Collections;
using System.Collections.Generic;

namespace SAL_Core.IO
{
    class ArduinoGroups : IEnumerable<ArduinoGroup>
    {
        private List<ArduinoGroup> groups;
        private ArduinoCollection collection;

        public ArduinoGroups(ArduinoCollection arduinoCollection)
        {
            groups = new List<ArduinoGroup>();
            collection = arduinoCollection;


        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<ArduinoGroup> GetEnumerator()
        {
            return groups.GetEnumerator();
        }
    }
}
