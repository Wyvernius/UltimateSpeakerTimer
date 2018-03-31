using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Mathematics;

namespace UltimateSpeakerTimer
{
    public static class AnnimationMessage
    {
        public static bool static_Message = true;
        public static Vector2 Pos = new Vector2();
        public static Vector2 PosMain = new Vector2();
        public static int time = -1;
        public static int BaseTime = -1;
        public static bool Send = false;
        public static bool running = false;
        public static bool stopped = true;
        public static float speed = 5.0f;
        public static bool MessageFocus = false;

        public static void Start()
        {
            time = BaseTime;
            stopped = false;
            TimerClass.newTimer(1000, TimerType.MessageAnnimation, Timer);
            TimerClass.Start(TimerType.MessageAnnimation);
            running = true;
        }

        public static void Stop()
        {
            TimerClass.Stop(TimerType.MessageAnnimation);
            stopped = true;
            Send = false;
        }

        public static bool Active()
        {
            return TimerClass.Active(TimerType.MessageAnnimation);
        }

        public static int Timer()
        {
            time--;
            if (time < 0)
                time = 0;
            if (time + BaseTime == BaseTime)
            {
                Stop();
            }
            Console.WriteLine("Timer {0}   {1}", time + BaseTime, BaseTime);
            return 0;
        }
    }
}
