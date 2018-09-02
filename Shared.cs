using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drawing = System.Drawing;
using SharpDX.Mathematics;
using RectangleF = SharpDX.RectangleF;
using SharpDX;
using SharpDX.Direct2D1;
using RenderTarget = SharpDX.Direct2D1.RenderTarget;
using SolidColorBrush = SharpDX.Direct2D1.SolidColorBrush;
using Brush = SharpDX.Direct2D1.Brush;
using FontFactory = SharpDX.DirectWrite.Factory;
using Bitmap = SharpDX.Direct2D1.Bitmap;

namespace UltimateSpeakerTimer
{
    public static class Shared
    {
        public static List<DxFont> Fonts = new List<DxFont>();

    }
    public class DxFont
    {
        TextFormat txtFormat;
        public RectangleF txtRect;
        public string txt = "";
        public FontItem TypeID;
        RectangleF OldRect;
        float scale = 1;
        Drawing.Graphics g;
        public DxFont() { }
        Drawing.Font OrigFont;
        public DxFont(WindowRenderTarget device,Drawing.Font TimerFont,FontItem _ID,float Div = 1, float scalar = 1)
        {
            TypeID = _ID;
            scale = scalar;
            OrigFont = TimerFont;
            FontWeight fontw = FontWeight.Normal;
            FontStyle fonts = FontStyle.Normal;
            txt = "";
            using (FontFactory fontFactory = new FontFactory())
            {
                if (TimerFont.Bold)
                    fontw = FontWeight.Bold;

                if (TimerFont.Italic)
                    fonts = FontStyle.Italic;
                
                    txtFormat = new TextFormat(fontFactory, TimerFont.Name, fontw, fonts, (TimerFont.Size  * Div)  * scalar);
            }
            g = Drawing.Graphics.FromImage(new Drawing.Bitmap((int)device.Size.Width,(int)device.Size.Height)); // create graphics to measure string later.
        }
        public TextFormat GetFormat()
        {
            return txtFormat;
        }

        public void SetText(string _txt)
        {
            txt = _txt;
            SetRect();
        }

        public void SetRect()
        {
            Drawing.SizeF txtSize = g.MeasureString(txt, new Drawing.Font(txtFormat.FontFamilyName,txtFormat.FontSize));
            txtRect = new RectangleF(((txtRect.X > 0) ? txtRect.X  : (txtRect.X < 0) ? 0 : 0),((txtRect.Y > 0) ? txtRect.Y : (txtRect.Y < 0) ? 0 : 0),  txtSize.Width, CalcLineSpace(txt).Y * txtFormat.FontSize);
            if (OldRect == null)
            {
                OldRect = txtRect;
            }
            else
            {
                OldRect.Location = txtRect.Location;
                if (!txtRect.Center.IsZero)
                {
                    Vector2 newCenter = OldRect.Center - txtRect.Center;
                    txtRect.Location += newCenter;
                    OldRect = txtRect;
                }
            }
            
        }

        private Vector2 CalcLineSpace(string String)
        {
            int LongestMessageLine = 0;
            int Lines = 0;
            char[] Line = String.ToCharArray();
            char[] Next = new char[] { '\r', '\n' };
            int Found = 0;
            int FoundBackup = 0;
            List<int> Pos = new List<int>();
            for (int c = 0; c < Line.Count(); c++)
            {
                for (int i = 0; i < Next.Count(); i++)
                {
                    if (!(Line[c + i] == Next[i]))
                    {
                        break;
                    }
                    else
                    {
                        Found++;
                        if (Found - FoundBackup == 2)
                        {
                            FoundBackup = Found;
                            Pos.Add(c + i);
                        }
                    }
                }
            }
            Pos.Add(Line.Length);
            int PositionOld = 0;
            int Longest = 0;
            foreach (int Position in Pos)
            {
                if ((Position - 1) - PositionOld > Longest)
                {
                    Longest = Position - PositionOld;
                    PositionOld = Position;
                }
            }
            if (Longest == 0)
                Longest = String.Length;
            LongestMessageLine = Longest;
            Lines = (Found / 2) + 1;
            return new Vector2(LongestMessageLine, Lines);
        }
    }

}
