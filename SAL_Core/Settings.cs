﻿using System;
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

        public AudioSettings Music { get; set; } = new AudioSettings();

        public AudioSettings RGBVisualizer { get; set; } = new AudioSettings();

        public AudioSettings Visualizer { get; set; } = new AudioSettings();

        public AudioSettings Musicbar { get; set; } = new AudioSettings();

        public AudioSettings Musicbar2 { get; set; } = new AudioSettings();

        public AudioSettings EffectsVisualizer { get; set; } = new AudioSettings();

        public void AddArduino(Arduino arduino)
        {
            for (int i = 0; i < Arduinos.Count; i++)
            {
                if (Arduinos[i].ConnectionType == arduino.Settings.ConnectionType)
                {
                    if (arduino.Settings.ConnectionType == ConnectionType.Serial)
                    {
                        if (arduino.Settings.COM == Arduinos[i].COM)
                        {
                            Arduinos.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (arduino.Settings.IP == Arduinos[i].IP && arduino.Settings.Port == Arduinos[i].Port)
                        {
                            Arduinos.RemoveAt(i);
                        }
                    }
                }
            }
            Arduinos.Add(arduino.Settings);
        }

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
            try
            {
                WriteToFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.bin"), this);
            }
            catch(Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Settings :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
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

        public bool Reverse { get; set; } = false;

        public ArduinoSettings(string com)
        {
            ConnectionType = ConnectionType.Serial;
            COM = com;
        }

        public ArduinoSettings(string ip, int port)
        {
            ConnectionType = ConnectionType.TCP;
            IP = ip;
            Port = port;
        }
    }

    [Serializable]
    public class EffectSettings
    {
        public string Current { get; set; } = "Rainbow";

        public Dictionary<string, EffectPreset> PresetList { get; set; } = new Dictionary<string, EffectPreset>()
        {
            {"Rainbow", EffectPresetDefaults.Rainbow },
            {"Cycle", EffectPresetDefaults.Cycle },
            {"Breathing", EffectPresetDefaults.Breathing },
            {"Flash", EffectPresetDefaults.Flash },
            {"Fire", EffectPresetDefaults.Fire },
            {"Static", EffectPresetDefaults.Static },
            {"Sweep", EffectPresetDefaults.Sweep },
            {"Load", EffectPresetDefaults.Load },
            {"Beam", EffectPresetDefaults.Beam }
        };

        public EffectPreset CurrentPreset
        {
            get
            {
                if (!PresetList.ContainsKey(Current))
                {
                    Current = PresetList.First().Key;
                }
                return PresetList[Current];
            }
        }

    }

    [Serializable]
    public class EffectPreset
    {
        public EffectTypes Type { get; set; } = EffectTypes.Static;
        public int Speed { get; set; } = 1;
        public int TotalSteps { get; set; } = 255;
        public int HoldingSteps { get; set; } = 50;
        public bool Reverse { get; set; } = false;
        public List<Color> ColorList { get; set; } = new List<Color>() { Colors.RED };

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
                    Speed = 1,
                    ColorList = new List<Color> { Colors.PURPLE }
                };
            }
        }

        public static EffectPreset Sweep
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Sweep,
                    Speed = 50,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
        public static EffectPreset Load
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Load,
                    Speed = 50,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
                };
            }
        }
        public static EffectPreset Beam
        {
            get
            {
                return new EffectPreset()
                {
                    Type = EffectTypes.Beam,
                    Speed = 50,
                    ColorList = new List<Color> { Colors.RED, Colors.ORANGE, Colors.YELLOW, Colors.LYME, Colors.GREEN, Colors.AQGREEN, Colors.CYAN, Colors.EBLUE, Colors.BLUE, Colors.PURPLE, Colors.MAGENTA, Colors.PINK }
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
    public class AudioSettings
    {
        public int Slope { get; set; } = 10;

        public AutoscalerSettings Autoscaler { get; set; } = new AutoscalerSettings();

        public int MinFreq { get; set; } = 50;

        public int MaxFreq { get; set; } = 4100;

    }

    [Serializable]
    public class AutoscalerSettings
    {
        public bool Enabled { get; set; } = true;

        public double Scale { get; set; } = 1.0;

        public int Amp { get; set; } = 1;
    }

    [Serializable]
    public class EqualizerSettings
    {
        public double High { get; set; } = 1;
        public double Mid { get; set; } = 1;
        public double Low { get; set; } = 1;
    }

}
