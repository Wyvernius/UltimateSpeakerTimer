using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSpeakerTimer
{
    static class PlaySound
    {
        static WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();
        public static string EndSound;
        public static string StartSound;

        public static void PlayendSound()
        {
            if (EndSound != null)
            {
                WMP.URL = EndSound;
                WMP.controls.play();
            }
        }

        public static void PlayStartSound()
        {
            if (StartSound != null)
            {
                WMP.URL = StartSound;
                WMP.controls.play();
            }
        }
    }
}
