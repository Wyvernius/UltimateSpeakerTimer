﻿using System;
using System.Windows.Forms;
using OrigColor = System.Drawing.Color;
using Color = SharpDX.Color;

namespace UltimateSpeakerTimer
{


    class CountDownEffectClass
    {
        public static Color CDC1;
        public static Color CDC2;
        public static int CDC1M = 0;
        public static int CDC2M;
        public static int CDC1S = 0;
        public static int CDC2S;
        public static bool BlinkZero;
        public static bool CDC1Blink;
        public static bool CDC2Blink;
        public static bool ShowTimerText = true;

        public static int ShowTimer()
        {
            if (((CDC1M * 100) + CDC1S) > Countdown.GetTimeLeftDec() && CDC1Blink)
                ShowTimerText = !ShowTimerText;
            else if (((CDC2M * 100) + CDC2S) > Countdown.GetTimeLeftDec() && CDC2Blink)
                ShowTimerText = !ShowTimerText;
            else if (Countdown.GetTimeLeftDec() == 0 && BlinkZero)
                ShowTimerText = !ShowTimerText;
            else
                ShowTimerText = true;
            return 0;
        }

    }
    public partial class CountDownEffects : Form
    {
        SettingsForm parent = null;
        public CountDownEffects(SettingsForm parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void CountDownEffects_Load(object sender, EventArgs e)
        {
            Color1.ForeColor = GetOrigColor(CountDownEffectClass.CDC1);
            Color2.ForeColor = GetOrigColor(CountDownEffectClass.CDC2);
            numericUpDown1.Value = CountDownEffectClass.CDC1M;
            numericUpDown2.Value = CountDownEffectClass.CDC2M;
            numericUpDown3.Value = CountDownEffectClass.CDC1S;
            numericUpDown4.Value = CountDownEffectClass.CDC2S;
            blinkZero.Checked = CountDownEffectClass.BlinkZero;
            CDC1Blink.Checked = CountDownEffectClass.CDC1Blink;
            CDC2Blink.Checked = CountDownEffectClass.CDC2Blink;
        }

        public OrigColor GetOrigColor(Color color)
        {
            return OrigColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void Color1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                CountDownEffectClass.CDC1 = new Color(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B, colorDialog1.Color.A);
                Color1.ForeColor = GetOrigColor(CountDownEffectClass.CDC1);
            }
        }

        private void Color2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                CountDownEffectClass.CDC2 = new Color(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B, colorDialog1.Color.A);
                Color2.ForeColor = GetOrigColor(CountDownEffectClass.CDC2);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CountDownEffectClass.CDC1M = (int)numericUpDown1.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            CountDownEffectClass.CDC1S = (int)numericUpDown3.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            CountDownEffectClass.CDC2M = (int)numericUpDown2.Value;
        }

        private void blinkZero_Click(object sender, EventArgs e)
        {
            CountDownEffectClass.BlinkZero = blinkZero.Checked;
            if (blinkZero.Checked)
            {
                CDC2Blink.Checked = false;
                CountDownEffectClass.CDC2Blink = false;
                CDC1Blink.Checked = false;
                CountDownEffectClass.CDC1Blink = false;
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            CountDownEffectClass.CDC2S = (int)numericUpDown4.Value;
        }

        private void CDC1Blink_CheckedChanged(object sender, EventArgs e)
        {
            CountDownEffectClass.CDC1Blink = CDC1Blink.Checked;
            if (CDC1Blink.Checked)
            {
                CDC2Blink.Checked = false;
                CountDownEffectClass.CDC2Blink = false;
                CountDownEffectClass.BlinkZero = false;
                blinkZero.Checked = false;
            }
        }

        private void CDC2Blink_CheckedChanged(object sender, EventArgs e)
        {
            CountDownEffectClass.CDC2Blink = CDC2Blink.Checked;
            if (CDC2Blink.Checked)
            {
                CDC1Blink.Checked = false;
                CountDownEffectClass.CDC1Blink = false;
                CountDownEffectClass.BlinkZero = false;
                blinkZero.Checked = false;
            }
        }
    }
}