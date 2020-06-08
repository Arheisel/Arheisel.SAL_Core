using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAL_Core;
using Damez.Log;

namespace ShineALight
{
    static class Program
    {
        public static Settings settings;
        public static ArduinoCollection arduinoCollection;
        public static Dictionary<string, Arduino> COMArduinos = new Dictionary<string, Arduino>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.Start();
            settings = Settings.Load();
            arduinoCollection = new ArduinoCollection();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
