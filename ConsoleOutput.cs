using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSpeakerTimer
{
    class ConsoleOutput
    {
        static FileStream ostrm;
        static StreamWriter writer;
        static TextWriter oldOut = Console.Out;

        public static void InitOutPut()
        {
            try
            {
                ostrm = new FileStream("LOG" + DateTime.Now.ToString().Replace(':','-') + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
                Console.SetOut(writer);
                TestOut();
               // Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open Redirect.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }
        }
        public static void TestOut()
        {

            Console.WriteLine("This is a line of text");
            Console.WriteLine("Everything written to Console.Write() or");
            Console.WriteLine("Console.WriteLine() will be written to a file");


        }
        public static void Dispose()
        { 
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Console OutPut Restored");
        }
    }
}
