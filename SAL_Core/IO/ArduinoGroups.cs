using System.Collections;
using System.Collections.Generic;

namespace SAL_Core.IO
{
    class ArduinoGroups : IEnumerable<ArduinoGroup>
    {
        private List<ArduinoGroup> groups;
        private ArduinoCollection collection;

        public int Count
        {
            get
            {
                return groups.Count;
            }
        }

        public ArduinoGroups(ArduinoCollection arduinoCollection)
        {
            groups = new List<ArduinoGroup>();
            collection = arduinoCollection;

            
        }

        public ArduinoGroup this[int index]
        {
            get
            {
                return groups[index];
            }
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
