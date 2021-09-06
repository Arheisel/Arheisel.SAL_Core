using System.Collections;
using System.Collections.Generic;

namespace SAL_Core.IO
{
    public class ArduinoGroups : IEnumerable<ArduinoGroup>
    {
        private List<ArduinoGroup> groups;

        public int Count
        {
            get
            {
                return groups.Count;
            }
        }

        public ArduinoGroups(ArduinoCollection arduinoCollection)
        {
            groups = new List<ArduinoGroup>() //TODO: Implement actual logic to populate the list
            {
                new ArduinoGroup(arduinoCollection, 1, 10),
                new ArduinoGroup(arduinoCollection, 11, 10),
                new ArduinoGroup(arduinoCollection, 21, 10),
                new ArduinoGroup(arduinoCollection, 31, 9)
            };
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
