using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSpeakerTimer
{
    public enum FontItem
    {
        Message = 2000,
        Time = 2001,
        Name = 2002,
        MainMessage = 2003,
        MainTime = 20004,
        MainName = 2005,
        None = 0,
    }

    public enum AnimMove
    {
        Move = 1990,
        Static= 1991,
    }

    public enum ScreenType
    {
        Unsupported = 0,
        WideScreen = 1,
        NarrowScreen = 2,
    }
}
