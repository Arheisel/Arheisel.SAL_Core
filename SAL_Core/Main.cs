using SAL_Core.Modes;
using SAL_Core.Settings;
using SAL_Core.IO;
using System.Collections.ObjectModel;

namespace SAL_Core
{
    public static class Main
    {
        public static ReadOnlyCollection<ModeListItem> Modes { get; }
        public static ProgramSettings Settings { get; }
        public static ArduinoCollection ArduinoCollection { get; }

        static Main()
        {
            Modes = new ReadOnlyCollection<ModeListItem>(new ModeListItem[] {
                new ModeListItem { ID = ModeEnum.Effects, Name = "Effects" },
                new ModeListItem { ID = ModeEnum.Music, Name = "Music" },
                new ModeListItem { ID = ModeEnum.RGBVisualizer, Name = "RGB Visualizer" },
                new ModeListItem { ID = ModeEnum.Visualizer, Name = "Visualizer" },
                new ModeListItem { ID = ModeEnum.Musicbar, Name = "Musicbar" },
                new ModeListItem { ID = ModeEnum.Musicbar2, Name = "Musicbar 2" },
                new ModeListItem { ID = ModeEnum.Musicbar3, Name = "Musicbar 3" },
                new ModeListItem { ID = ModeEnum.EffectsVisualizer, Name = "Effects Visualizer" },
            });

            Settings = ProgramSettings.Load();
            ArduinoCollection = new ArduinoCollection(Settings.ArduinoCollectionSettings);
        }
    }
}
