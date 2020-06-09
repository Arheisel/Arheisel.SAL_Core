using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Damez.Log;

namespace SAL_Core
{
    [Serializable]
    public class Settings
    {
        public List<ArduinoSettings> Arduinos { get; set; } = new List<ArduinoSettings>();
        public int CurrentMode { get; set; } = 0;
        public EffectSettings Effects { get; set; } = new EffectSettings();

        public MusicSettings Music { get; set; } = new MusicSettings();

        public VSettings RGBVisualizer { get; set; } = new VSettings();

        public VSettings Visualizer { get; set; } = new VSettings();

        public MusicSettings Musicbar { get; set; } = new MusicSettings();

        public MusicSettings Musicbar2 { get; set; } = new MusicSettings();

        public static void WriteToFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        public void Save()
        {
            WriteToFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.bin"), this);
        }

        public static Settings Load()
        {
            try
            {
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.bin")))
                    return ReadFromFile<Settings>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.bin"));
                else
                    return new Settings();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Settings :: " + e.Message + Environment.NewLine + e.StackTrace);
                return new Settings();
            }
        }
    }

    [Serializable]
    public class ArduinoSettings
    {
        public ConnectionType ConnectionType { get; set; } = ConnectionType.Serial;

        public string COM { get; set; } = string.Empty;

        public string IP { get; set; } = string.Empty;

        public int Port { get; set; } = 0;

        public ArduinoSettings(string com)
        {
            ConnectionType = ConnectionType.Serial;
            COM = com;
        }

        public ArduinoSettings(string ip, int port)
        {
            ConnectionType = ConnectionType.UDP;
            IP = ip;
            Port = port;
        }
    }

    [Serializable]
    public class EffectSettings
    {
        public string Current { get; set; } = "Rainbow";

        public Dictionary<string, EffectPreset> PresetList { get; } = new Dictionary<string, EffectPreset>()
        {
            {"Rainbow", EffectPresetDefaults.Rainbow },
            {"Cycle", EffectPresetDefaults.Cycle },
            {"Breathing", EffectPresetDefaults.Breathing },
            {"Flash", EffectPresetDefaults.Flash },
            {"Fire", EffectPresetDefaults.Fire },
            {"Static", EffectPresetDefaults.Static }
        };

        public EffectPreset CurrentPreset
        {
            get
            {
                return PresetList[Current];
            }
        }

    }

    [Serializable]
    public class EffectPreset
    {
        public EffectTypes Type { get; set; } = EffectTypes.Rainbow;
        public int Speed { get; set; } = 0;
        public int TotalSteps { get; set; } = 255;
        public int HoldingSteps { get; set; } = 50;
        public List<Color> ColorList { get; set; } = new List<Color>();

    }

    public static class EffectPresetDefaults
    {
        public static EffectPreset Rainbow
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Rainbow,
                    Speed = 50,
                    TotalSteps = 20,
                    HoldingSteps = 0,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
        public static EffectPreset Cycle
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Cycle,
                    Speed = 50,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
        public static EffectPreset Breathing
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Breathing,
                    Speed = 50,
                    ColorList = new List<Color> { Colors.RED, Colors.GREEN, Colors.BLUE, Colors.YELLOW, Colors.MAGENTA, Colors.CYAN }
                };
            }
        }
        public static EffectPreset Flash
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Flash,
                    Speed = 50,
                    TotalSteps = 20,
                    HoldingSteps = 2,
                    ColorList = new List<Color> { Colors.WHITE }
                };
            }
        }

        public static EffectPreset Fire
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Fire,
                    Speed = 50,
                    TotalSteps = 255,
                    HoldingSteps = 100,
                    ColorList = new List<Color> { Colors.ORANGE, Colors.RED }
                };
            }
        }

        public static EffectPreset Static
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Static,
                    Speed = 0,
                    ColorList = new List<Color> { Colors.PURPLE }
                };
            }
        }
    }

    [Serializable]
    public class MusicSettings
    {
        public int Slope { get; set; } = 10;

        public AutoscalerSettings Autoscaler { get; set; } = new AutoscalerSettings();
    }

    [Serializable]
    public class VSettings
    {
        public int Slope { get; set; } = 10;

        public AutoscalerSettings Autoscaler { get; set; } = new AutoscalerSettings();

        public int MinFreq { get; set; } = 50;

        public int MaxFreq { get; set; } = 4100;

    }

    [Serializable]
    public class AutoscalerSettings
    {
        public bool Enabled { get; set; } = false;

        public double Scale { get; set; } = 1.0;
    }
}
