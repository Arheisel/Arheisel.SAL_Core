using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAL_Core.Settings
{
    [Serializable]
    public class Profile
    {
        private readonly List<ModeSettings> modes;

        public Profile()
        {
            modes = new List<ModeSettings>();
        }

        public ModeSettings GetModeSettings(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException("index Must be greater than 0");

            if(index > modes.Count - 1)
            {
                for(int i = modes.Count; i <= index; i++)
                {
                    modes.Add(new ModeSettings());
                }
            }

            return modes[index];
        }
    }
}
