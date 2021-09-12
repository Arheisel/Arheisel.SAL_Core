using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAL_Core.IO;
using Arheisel.Log;
using SAL_Core.Settings;

namespace ShineALight
{
    static class Program
    {
        public static Dictionary<string, Arduino> COMArduinos = new Dictionary<string, Arduino>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}
