using System;
using System.Collections.Generic;
using System.IO;
using Arheisel.Log;
using SAL_Core.IO;
using SAL_Core.IO.Connection;

namespace SAL_Core.Settings
{
    [Serializable]
    public class ProgramSettings
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

        public static ProgramSettings Load()
        {
            try
            {
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.bin")))
                    return ReadFromFile<ProgramSettings>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.bin"));
                else
                    return new ProgramSettings();
            }
            catch (Exception e)
            {
                Log.Write(Log.TYPE_ERROR, "Settings :: " + e.Message + Environment.NewLine + e.StackTrace);
                return new ProgramSettings();
            }
        }
    }
}
