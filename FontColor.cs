using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SharpDX.Mathematics.Interop;
using SharpDX;
using OrigColor = System.Drawing.Color;

namespace UltimateSpeakerTimer
{
    public partial class ColorWindow : Form
    {
        ColorType Type = ColorType.None;
        Color color1;
        Color color2;
        public ColorWindow(ColorType Type)
        {
            this.Type = Type;
            InitializeComponent();

            if (Type == ColorType.Font)
            {
                color1 = TimerColors.FontColor1;
                Color1.ForeColor = GetOrigColor(color1);

                color2 = TimerColors.FontColor2;
                Color2.ForeColor = GetOrigColor(color2);
            }
            if (Type == ColorType.Background)
            {
                color1 = TimerColors.BGColor1;
                Color1.ForeColor = GetOrigColor(color1);
                color2 = TimerColors.BGColor2;
                Color2.ForeColor = GetOrigColor(color2);
            }

            foreach (Control con in this.Controls)
            {
                if (con is CheckBox)
                {
                    if (!((CheckBox)con).Checked && int.Parse((string)((CheckBox)con).Tag) == (int)(Type == ColorType.Font ? TimerEffects.GradientEffectTimer : TimerEffects.GradientEffectBG))
                    {
                        ((CheckBox)con).Checked = true;
                    }
                }
            }
        }

        public OrigColor GetOrigColor(Color color)
        {
            return OrigColor.FromArgb(color.A, color.R, color.G, color.B);
        }
        private void Color1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color1.ForeColor = colorDialog1.Color;
                if (Type == ColorType.Font)
                    TimerColors.FontColor1 = new SharpDX.Color(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B, colorDialog1.Color.A);
                if (Type == ColorType.Background)
                    TimerColors.BGColor1 = new SharpDX.Color(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B, colorDialog1.Color.A);
            }
        }

        private void Color2_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = GetOrigColor(color1);
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color2.ForeColor = colorDialog1.Color;
                if (Type == ColorType.Font)
                    TimerColors.FontColor2 = new SharpDX.Color(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B, colorDialog1.Color.A);
                if (Type == ColorType.Background)
                    TimerColors.BGColor2 = new SharpDX.Color(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B, colorDialog1.Color.A);
            }
        }

        private void CheckBox_Click(object sender, EventArgs e)
        {
            int a = 0;
            int d = 0;
            foreach (Control con in this.Controls)
            {
                if (con is CheckBox)
                {
                    if (((CheckBox)con).Checked && int.Parse((string)((CheckBox)con).Tag) != (int)(Type == ColorType.Font ? TimerEffects.GradientEffectTimer : TimerEffects.GradientEffectBG))
                    {
                        a++;
                        if (Type == ColorType.Font)
                            TimerEffects.GradientEffectTimer = (GradientDir)int.Parse((string)((CheckBox)con).Tag);
                        if (Type == ColorType.Background)
                            TimerEffects.GradientEffectBG = (GradientDir)int.Parse((string)((CheckBox)con).Tag);
                        break;
                    }

                }
            }
            foreach (Control con in this.Controls)
            {
                if (con is CheckBox)
                {
                    if (((CheckBox)con).Checked && int.Parse((string)((CheckBox)con).Tag) != (int)(Type == ColorType.Font ? TimerEffects.GradientEffectTimer : TimerEffects.GradientEffectBG))
                    {
                        d++;
                        ((CheckBox)con).Checked = false;
                    }
                }
            }
            if (a == 0 && d == 0)
            {
                if (Type == ColorType.Font)
                    TimerEffects.GradientEffectTimer = GradientDir.None;
                if (Type == ColorType.Background)
                    TimerEffects.GradientEffectBG = GradientDir.None;
            }
        }

        private void ClearColor2_Click(object sender, EventArgs e)
        {
            if (Type == ColorType.Font)
                TimerColors.FontColor2 = new Color(0,0,0,0);
            if (Type == ColorType.Background)
                TimerColors.BGColor2 = new Color(0,0,0,0);
            Color2.ForeColor = OrigColor.Black;
        }
    }



    static class TimerColors
    {
        static public Color FontColor1 = new Color(255, 255, 255, 255);
        static public Color FontColor2 = new Color(0, 0, 0, 0);
        static public Color BGColor1 = new Color(0, 0, 0, 255);
        static public Color BGColor2 = new Color(0, 0, 0, 0);
    }

    static class TimerEffects
    {
        static public GradientDir GradientEffectTimer = 0;
        static public GradientDir GradientEffectBG = 0;
    }

    public enum ColorType
    {
        Font = 1000,
        Background = 1001,
        None = 1002,
    }

    public enum GradientDir
    {
        None = 0,
        LeftTop = 1,
        Top = 2,
        RightTop = 3,
        Right = 4,
        RightBottom = 5,
        Bottom = 6,
        LeftBottom = 7,
        Left = 8,
    }
}
