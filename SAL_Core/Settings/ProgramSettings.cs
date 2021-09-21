using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arheisel.Log;
using SAL_Core.IO;
using SAL_Core.IO.Connection;

namespace SAL_Core.Settings
{
    [Serializable]
    public sealed class ProgramSettings
    {
        public ArduinoCollectionSettings ArduinoCollectionSettings { get; } = new ArduinoCollectionSettings();

        public string CurrentProfileName { get; set; } = "Default";

        public EffectSettings Effects { get; } = new EffectSettings();

        public Dictionary<string, Profile> Profiles { get; } = new Dictionary<string, Profile>() { {"Default", new Profile()} };

        public Profile CurrentProfile
        {
            get
            {
                if (!Profiles.ContainsKey(CurrentProfileName))
                {
                    CurrentProfileName = Profiles.First().Key;
                }
                return Profiles[CurrentProfileName];
            }
        }

        // ------ TOGO -------
        public int CurrentMode { get; set; } = 0;
        public AudioSettings Music { get; } = new AudioSettings();

        public AudioSettings RGBVisualizer { get; } = new AudioSettings();

        public AudioSettings Visualizer { get; } = new AudioSettings();

        public AudioSettings Musicbar { get; } = new AudioSettings();

        public AudioSettings Musicbar2 { get; } = new AudioSettings();

        public AudioSettings EffectsVisualizer { get; } = new AudioSettings();

        // ------ /TOGO -------

        private static void WriteToFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        private static T ReadFromFile<T>(string filePath)
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
                Log.Error("Settings :: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        public static ProgramSettings Load()
        {
            try
            {
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.bin")))
                    try
                    {
                        return ReadFromFile<ProgramSettings>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.bin"));
                    }
                    catch (Exception e)
                    {
                        Log.Exception(e);
                        return new ProgramSettings();
                    }
                else
                    return new ProgramSettings();
            }
            catch (Exception e)
            {
                Log.Error("Settings :: " + e.Message + Environment.NewLine + e.StackTrace);
                return new ProgramSettings();
            }
        }
    }
}
