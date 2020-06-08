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
        public ConectionType ConectionType { get; set; } = ConectionType.Serial;

        public string COM { get; set; } = string.Empty;

        public string IP { get; set; } = string.Empty;

        public int Port { get; set; } = 0;
    }

    public enum ConectionType
    {
        Serial = 0,
        UDP = 1
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
            {"Flash", EffectPresetDefaults.Flash }
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

    public enum EffectTypes
    {
        Rainbow = 0,
        Cycle = 1,
        Breathing = 2,
        Flash = 3
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
                    Speed = 0,
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
                    TotalSteps = 20,
                    HoldingSteps = 2,
                    ColorList = new List<Color> { Colors.WHITE }
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
