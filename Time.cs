using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace UltimateSpeakerTimer
{

    enum TimerType
    {
        TimerAnnimation = 1,
        CountDown = 2,
        MessageAnnimation = 3,
        Keys = 4,

    }

    class TimerClassItem
    {
        public int CallbackResult;
        public Func<int> Callback;
        public TimerType Type;
        public int Interval;
        public bool Active;
        public long lastTimeCount = 0;
    }
    static class TimerClass
    {
        public static List<TimerClassItem> TimersList = new List<TimerClassItem>();
        static Timer MainTimerThread = new Timer();
        static int PastTime = 0;
        static int tickInterval = 100;

        public static void MainThreadTimerStart()
        {
            MainTimerThread.Interval = tickInterval;
            MainTimerThread.Tick += TimerThread;
            MainTimerThread.Start();
        }

        public static void MainThreadTimerStop()
        {
            if (MainTimerThread.Enabled)
                MainTimerThread.Stop();
        }

        public static bool Active(TimerType _Type)
        {
            for (int i = 0; i < TimersList.Count; i++)
            {
                if (TimersList[i].Type == _Type)
                    return TimersList[i].Active;
            }
            return false;
        }

        public static void Start(TimerType _Type)
        {
            for (int i = 0; i < TimersList.Count; i++)
            {
                if (TimersList[i].Type == _Type)
                {
                    TimersList[i].lastTimeCount = PastTime;
                    TimersList[i].Active = true;
                }
            }
        }

        public static void Stop(TimerType _Type)
        {
            for (int i = 0; i < TimersList.Count; i++)
            {
                if (TimersList[i].Type == _Type)
                    TimersList[i].Active = false;
            }
        }

        public static void newTimer(int _Interval,TimerType _Type,Func<int> _Callback)
        {
            bool Inlist = false;
            for (int i = 0;i < TimersList.Count;i++)
            {
                if (TimersList[i].Type == _Type)
                    Inlist = true;
            }
            if (!Inlist)
            {
                TimerClassItem Item = new TimerClassItem();
                Item.Type = _Type;
                Item.Interval = _Interval;
                Item.Callback = _Callback;
                Item.Active = false;
                TimersList.Add(Item);
            }
        }

        static void TimerThread(object sender, EventArgs e)
        {
            PastTime += tickInterval;
            foreach (TimerClassItem Item in TimersList)
            {
                if (Item.Active)
                {
                    if (PastTime - Item.lastTimeCount > Item.Interval)
                    {
                        Item.Callback?.Invoke(); // Invoke Callback.
                        Item.lastTimeCount = PastTime; // Update LastCalled Time;
                    }
                }
            }
        }
    }
}
