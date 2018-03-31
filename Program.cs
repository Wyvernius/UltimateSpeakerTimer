using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UltimateSpeakerTimer
{
    static class Program
    {

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();
           // ConsoleOutput.InitOutPut();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SettingsForm());
        }
    }

    class SpeakerList
    {
        public List<Speaker> List = new List<Speaker>();
        public class Speaker
        {
            public Speaker()
            {

            }

            public Speaker(string _Name,int _Hours,int _Minutes, int _Seconds)
            {
                Name = _Name;
                Hours = _Hours;
                Minutes = _Minutes;
                Seconds = _Seconds;
            }
            public string Name;
            public int Hours;
            public int Minutes;
            public int Seconds;
        }
    }
}
