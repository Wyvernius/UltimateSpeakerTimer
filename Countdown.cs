﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UltimateSpeakerTimer
{
    static class Countdown 
    {
        public static int MessageTime = -1;
        public static int Hours = 0;
        public static int Minutes = 1;
        public static int Seconds = 0;

        public static string GetTime()
        {
            string TimerText = "0";
            if (Hours != 0)
            {
                TimerText = Hours + ":" + ((Minutes < 10) ? ("0" + Minutes).ToString() : Minutes.ToString()) + ":" + ((Seconds < 10) ? ("0" + Seconds).ToString() : Seconds.ToString());
            }
            else if (Minutes != 0)
            {
                TimerText = Minutes + ":" + ((Seconds < 10) ? ("0" + Seconds).ToString() : Seconds.ToString());
            }
            else if (Seconds != 0)
            {
                TimerText = ((Seconds < 10) ? ("0" + Seconds.ToString()) : Seconds.ToString());
            }
            return TimerText;
        }

        public static float GetTimeLeftDec()
        {
            float timerDec = 0f;
            timerDec += (float)Hours * 1000;
            timerDec += (float)Minutes * 100;
            timerDec += (float)Seconds;
            return timerDec;
        }

        public static int TimeCountDown()
        {
            if (Hours + Minutes + Seconds != 0)
            {
                Seconds--;
                if (Seconds < 0)
                {
                    Seconds = 59;
                    Minutes--;
                    if (Minutes < 0 && Hours != 0)
                    {
                        Minutes = 59;
                        Hours--;
                    }
                }

                if (Hours + Minutes + Seconds == 0)
                {
                    UltimateSpeakerTimer.PlaySound.PlayendSound();
                }
            }
            if (MessageTime > 0)
                MessageTime--;
            return 0;
        }
    }
}
