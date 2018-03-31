using System;
using System.Threading;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX;
using SharpDX.DirectWrite;
using Font = System.Drawing.Font;
using Color = SharpDX.Color;
using Rectangle = System.Drawing.Rectangle;
using RectangleF = SharpDX.RectangleF;
using Factory = SharpDX.Direct2D1.Factory;
using FontFactory = SharpDX.DirectWrite.Factory;
using Format = SharpDX.DXGI.Format;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using System.Drawing.Printing;
using UltimateSpeakerTimer;

namespace UltimateSpeakerTimer
{
    public partial class timerForm : Form
    {
        public string StartSound { get; set; }
        public static string EndSound { get; set; }
        public string Picture { get; set; }
        public static int Hours;
        public static int Minutes;
        public static int Seconds;

        public Brush LGradientBrushTimer = null;
        public Bitmap bitmap;
        public static Size2 bitmapSize;
        public string Message = null;

        public bool Static_Message = true;
        public static int MessageTime;
        public float MessageLoopPosX;
        public string SpeakerName;
        public RectangleF SpeakerNameRect;

        public Color4 ClearColor = new Color4(0, 0, 0, 0);
        public Brush LGradientBrushBG = null;
        public bool TransperantBG;


        public WindowRenderTarget device;
        private HwndRenderTargetProperties renderProperties;
        private static SolidColorBrush solidColorBrush;
        private Factory factory;
        private FontFactory fontFactory;
        SettingsForm parent = null;
        public Rectangle deviceRect;

        public Thread TextThread = null;

        public bool ShowName;

        [DllImport("dwmapi.dll")]
        static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margins);

        private void timerForm_Load()
        {
            // Init factory
            factory = new Factory();
            fontFactory = new FontFactory();

            // Render settings
            renderProperties = new HwndRenderTargetProperties()
            {
                Hwnd = this.Handle,
                PixelSize = new Size2(1920, 1080),
                PresentOptions = PresentOptions.None
            };
            // Init device
            device = new WindowRenderTarget(factory, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderProperties);

            // Init brush
            solidColorBrush = new SolidColorBrush(device, new Color4(255, 255, 255, 0));
            deviceRect = new Rectangle(0, 0, (int)device.Size.Width, (int)device.Size.Height);
        }

        public void GetBrush(ColorType Type)
        {
            if (Type == ColorType.Font)
            {
                foreach (DxFont Font in Shared.Fonts)
                {
                    if (Font.TypeID == FontItem.MainTime)
                    {
                        if (TimerEffects.GradientEffectTimer != "")
                        {
                            GradientStop[] GS = new GradientStop[2];
                            GS[0].Color = TimerColors.FontColor1;
                            GS[0].Position = 0.0f;
                            GS[1].Color = TimerColors.FontColor2;
                            GS[1].Position = 1.0f;
                            GradientStopCollection GSC = new GradientStopCollection(device, GS);

                            if (LGradientBrushTimer != null)
                                LGradientBrushTimer.Dispose();
                            if (TimerEffects.GradientEffectTimer == "LeftTop")
                            {
                                LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                {
                                    StartPoint = new Vector2(Font.txtRect.X, Font.txtRect.Y),
                                    EndPoint = new Vector2(Font.txtRect.Right, Font.txtRect.Bottom)
                                }, GSC);
                            }
                            if (TimerEffects.GradientEffectTimer == "Top")
                            {
                                LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                {
                                    StartPoint = new Vector2(Font.txtRect.X + (Font.txtRect.Width / 2), Font.txtRect.Top),
                                    EndPoint = new Vector2(Font.txtRect.X + (Font.txtRect.Width / 2), Font.txtRect.Bottom)
                                }, GSC);
                            }
                            if (TimerEffects.GradientEffectTimer == "RightTop")
                            {
                                LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                {
                                    StartPoint = new Vector2(Font.txtRect.Right, Font.txtRect.Top),
                                    EndPoint = new Vector2(Font.txtRect.X, Font.txtRect.Bottom)
                                }, GSC);
                            }
                            if (TimerEffects.GradientEffectTimer == "LeftMid")
                            {
                                LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                {
                                    StartPoint = new Vector2(Font.txtRect.X, Font.txtRect.Y + (Font.txtRect.Height / 2)),
                                    EndPoint = new Vector2(Font.txtRect.Right, Font.txtRect.Y + (Font.txtRect.Height / 2))
                                }, GSC);
                                return;
                            }
                            if (TimerEffects.GradientEffectTimer == "RightMid")
                            {
                                LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                {
                                    StartPoint = new Vector2(Font.txtRect.Right, Font.txtRect.Y + (Font.txtRect.Height / 2)),
                                    EndPoint = new Vector2(Font.txtRect.X, Font.txtRect.Y + (Font.txtRect.Height / 2))
                                }, GSC);
                                return;
                            }
                            if (TimerEffects.GradientEffectTimer == "LeftBottom")
                            {
                                LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                {
                                    StartPoint = new Vector2(Font.txtRect.X, Font.txtRect.Bottom),
                                    EndPoint = new Vector2(Font.txtRect.Right, Font.txtRect.Top)
                                }, GSC);
                            }
                            if (TimerEffects.GradientEffectTimer == "Bottom")
                            {;
                                LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                {
                                    StartPoint = new Vector2(Font.txtRect.X + (Font.txtRect.Width / 2), Font.txtRect.Bottom),
                                    EndPoint = new Vector2(Font.txtRect.X + (Font.txtRect.Width / 2), Font.txtRect.Top)
                                }, GSC);
                            }
                            if (TimerEffects.GradientEffectTimer == "RightBottom")
                            {
                                LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                {
                                    StartPoint = new Vector2(Font.txtRect.Right, Font.txtRect.Bottom),
                                    EndPoint = new Vector2(Font.txtRect.X, Font.txtRect.Top)
                                }, GSC);
                            }
                            GSC.Dispose();
                        }
                        else
                            LGradientBrushTimer = null;
                    }
                }
            }

            if (Type == ColorType.Background)
            {
                GradientStop[] GS = new GradientStop[2];
                GS[0].Color = TimerColors.BGColor1;
                GS[0].Position = 0.0f;
                GS[1].Color = TimerColors.BGColor2;
                GS[1].Position = 1.0f;
                GradientStopCollection GSC = new GradientStopCollection(device, GS);

                if (LGradientBrushBG != null) 
                    LGradientBrushBG.Dispose();
                if (TimerEffects.GradientEffectBG != "")
                {
                    if (TimerEffects.GradientEffectBG == "LeftTop")
                    {
                        LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                        {
                            StartPoint = new Vector2(deviceRect.X, deviceRect.Y),
                            EndPoint = new Vector2(deviceRect.Right, deviceRect.Bottom)
                        }, GSC);
                    }
                    if (TimerEffects.GradientEffectBG == "Top")
                    {
                        LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                        {
                            StartPoint = new Vector2(deviceRect.X + (deviceRect.Width / 2), deviceRect.Top),
                            EndPoint = new Vector2(deviceRect.X + (deviceRect.Width / 2), deviceRect.Bottom)
                        }, GSC);
                    }
                    if (TimerEffects.GradientEffectBG == "RightTop")
                    {
                        LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                        {
                            StartPoint = new Vector2(deviceRect.Right, deviceRect.Top),
                            EndPoint = new Vector2(deviceRect.X, deviceRect.Bottom)
                        }, GSC);
                    }
                    if (TimerEffects.GradientEffectBG == "LeftMid")
                    {
                        LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                        {
                            StartPoint = new Vector2(deviceRect.X, deviceRect.Y + (deviceRect.Height / 2)),
                            EndPoint = new Vector2(deviceRect.Right, deviceRect.Y + (deviceRect.Height / 2))
                        }, GSC);
                    }
                    if (TimerEffects.GradientEffectBG == "RightMid")
                    {
                        LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                        {
                            StartPoint = new Vector2(deviceRect.Right, deviceRect.Y + (deviceRect.Height / 2)),
                            EndPoint = new Vector2(deviceRect.X, deviceRect.Y + (deviceRect.Height / 2))
                        }, GSC);
                    }
                    if (TimerEffects.GradientEffectBG == "LeftBottom")
                    {
                        LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                        {
                            StartPoint = new Vector2(deviceRect.X, deviceRect.Bottom),
                            EndPoint = new Vector2(deviceRect.Right, deviceRect.Top)
                        }, GSC);
                    }
                    if (TimerEffects.GradientEffectBG == "Bottom")
                    {
                        LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                        {
                            StartPoint = new Vector2(deviceRect.X + (deviceRect.Width / 2), deviceRect.Bottom),
                            EndPoint = new Vector2(deviceRect.X + (deviceRect.Width / 2), deviceRect.Top)
                        }, GSC);
                    }
                    if (TimerEffects.GradientEffectBG == "RightBottom")
                    {
                        LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                        {
                            StartPoint = new Vector2(deviceRect.Right, deviceRect.Bottom),
                            EndPoint = new Vector2(deviceRect.X, deviceRect.Top)
                        }, GSC);
                    }
                    GSC.Dispose();
                }
                else
                    LGradientBrushBG = null;
            }
        }

        public timerForm(SettingsForm parent)
        {
            this.parent = parent;

            InitializeComponent();

            timerForm_Load();
        }

        private void timerForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Margins mrg = new Margins(Math.Abs(this.Location.X), Location.Y, Size.Width, Size.Height);
                DwmExtendFrameIntoClientArea(this.Handle, ref mrg);

                MessageLoopPosX = (int)device.Size.Width;
                GetBrush(ColorType.Font);
                GetBrush(ColorType.Background);
                CountDownEffectClass.ShowTimerText = true;
                if (TextThread == null)
                {
                    TextThread = new Thread(new ParameterizedThreadStart(TimerTextThread));
                    TextThread.Start();
                }
            }
            else
            {
                TextThread = null;
            }
        }

        public void TimerTextThread(object sender)
        {
            while (Visible)
            {
                device.BeginDraw();

                GetBrush(ColorType.Background);
                if (LGradientBrushBG == null)
                    DrawFillRect(0, 0, device.Size.Width, device.Size.Height, TimerColors.BGColor1);
                else
                    DrawFillRect(0, 0, device.Size.Width, device.Size.Height, LGradientBrushBG);

                if (TransperantBG)
                    device.Clear(ClearColor);
                if (Picture != null && !TransperantBG)
                    DrawImage(0, 0, (int)device.Size.Width, (int)device.Size.Height, bitmap);

                foreach (DxFont Font in Shared.Fonts)
                {
                    if (Font.TypeID == FontItem.MainTime)
                    {
                        Font.SetText(Countdown.GetTime());
                        if (CountDownEffectClass.ShowTimerText)
                        {
                            if (LGradientBrushTimer == null)
                            {
                                Color Pricolor = TimerColors.FontColor1;
                                if (((CountDownEffectClass.CDC1M * 100) + CountDownEffectClass.CDC1S) > Countdown.GetTimeLeftDec())
                                {
                                    if (CountDownEffectClass.CDC1.A != 0)
                                        Pricolor = CountDownEffectClass.CDC1;
                                    if (((CountDownEffectClass.CDC2M * 100) + CountDownEffectClass.CDC2S) > Countdown.GetTimeLeftDec())
                                    {
                                        if (CountDownEffectClass.CDC2.A != 0)
                                            Pricolor = CountDownEffectClass.CDC2;
                                    }
                                }
                                DrawTextCenter((Font.txtRect.X), (Font.txtRect.Y), ((Font.txtRect.Width)), (Font.txtRect.Height), Font.txt, Pricolor, Font.GetFormat(), TimerColors.FontColor2);
                            }
                            else
                                DrawTextCenter((Font.txtRect.X), (Font.txtRect.Y), ((Font.txtRect.Width)), (Font.txtRect.Height), Font.txt, LGradientBrushTimer, Font.GetFormat());
                        }
                    }

                    if (Font.TypeID == FontItem.MainName)
                    {
                        if (Font.txt != "" && Font.txt != null && ShowName)
                        {
                            if (!Font.txtRect.TopLeft.IsZero)
                            {
                                DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, TimerColors.FontColor1, Font.GetFormat());
                            }
                        }
                    }
                    if (Font.TypeID == FontItem.MainMessage)
                    {
                        if (AnnimationMessage.running)
                            DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, TimerColors.FontColor1, Font.GetFormat());
                    }
                }
                device.EndDraw();
            }
        }

        private void DrawRect(float X, float Y, float W, float H, Color color)
        {
            solidColorBrush.Color = color;
            device.DrawRectangle(new RectangleF(X, Y, W, H), solidColorBrush);
        }

        private void DrawRect(float X, float Y, float W, float H, Color color, float stroke)
        {
            solidColorBrush.Color = color;
            device.DrawRectangle(new RectangleF(X, Y, W, H), solidColorBrush, stroke);
        }

        private void DrawFillRect(float X, float Y, float W, float H, Color color)
        {
            solidColorBrush.Color = color;
            device.FillRectangle(new RectangleF(X, Y, W, H), solidColorBrush);
        }

        private void DrawFillRect(float X, float Y, float W, float H, Brush brush)
        {
            device.FillRectangle(new RectangleF(X, Y, W, H), brush);
        }

        private void DrawText(float X, float Y, string text, Color color, TextFormat font)
        {
            solidColorBrush.Color = color;
            device.DrawText(text, font, new RectangleF(X, Y, font.FontSize * text.Length, font.FontSize), solidColorBrush);
        }

        private void DrawText(float X, float Y, string text, Color color, bool outline, TextFormat font)
        {
            if (outline)
            {
                solidColorBrush.Color = Color.Black;
                device.DrawText(text, font, new RectangleF(X + 1, Y + 1, font.FontSize * text.Length, font.FontSize), solidColorBrush);
            }

            solidColorBrush.Color = color;
            device.DrawText(text, font, new RectangleF(X, Y, font.FontSize * text.Length, font.FontSize), solidColorBrush);
        }

        private void DrawTextCenter(float X, float Y, float W, float H, string text, Color color, TextFormat font) //Modded
        {
            TextLayout layout = new TextLayout(fontFactory, text, font, W, H);
            layout.TextAlignment = TextAlignment.Center;
            layout.ParagraphAlignment = ParagraphAlignment.Center;
            solidColorBrush.Color = color;
            device.DrawTextLayout(new Vector2(X, Y), layout, solidColorBrush);
            layout.Dispose();
        }

        private void DrawTextCenter(float X, float Y, float W, float H, string text, Brush brush, TextFormat font) //Modded
        {
            TextLayout layout = new TextLayout(fontFactory, text, font, W, H);
            layout.TextAlignment = TextAlignment.Center;
            layout.ParagraphAlignment = ParagraphAlignment.Center;
            device.DrawTextLayout(new Vector2(X, Y), layout, brush);
            layout.Dispose();
        }

        private void DrawTextCenter(float X, float Y, float W, float H, string text, Color color, TextFormat font, Color GhostColor) //Modded
        {
            TextLayout layout = new TextLayout(fontFactory, text, font, W, H);
            layout.TextAlignment = TextAlignment.Center;

            solidColorBrush.Color = GhostColor;
            device.DrawTextLayout(new Vector2(X + 3, Y + 3), layout, solidColorBrush);

            solidColorBrush.Color = color;
            device.DrawTextLayout(new Vector2(X, Y), layout, solidColorBrush);
            layout.Dispose();
        }
        private void DrawLine(int X, int Y, int XX, int YY, Color color, float Width = 1)
        {
            solidColorBrush.Color = color;
            device.DrawLine(new Vector2(X, Y), new Vector2(XX, YY), solidColorBrush, Width);
        }

        private void DrawLine(float X, float Y, float XX, float YY, Color color, float Width = 1)
        {
            solidColorBrush.Color = color;
            device.DrawLine(new Vector2(X, Y), new Vector2(XX, YY), solidColorBrush, Width);
        }

        private void DrawLine(Vector3 w2s, Vector3 _w2s, Color color, float Width = 1)
        {
            solidColorBrush.Color = color;
            device.DrawLine(new Vector2(w2s.X, w2s.Y), new Vector2(_w2s.X, _w2s.Y), solidColorBrush, Width);
        }

        private void DrawCircle(int X, int Y, int W, int H, int Rx, int Ry, Color color)
        {
            RoundedRectangle rect = new RoundedRectangle();
            rect.RadiusX = Rx;
            rect.RadiusY = Ry;
            rect.Rect = new RectangleF(X, Y, W, H);
            solidColorBrush.Color = color;
            float[] Dashes = { };
            StrokeStyleProperties styleProperties = default(StrokeStyleProperties);
            device.DrawRoundedRectangle(ref rect, solidColorBrush, 1F, new StrokeStyle(factory, styleProperties, Dashes));
        }

        private void DrawCircle(int X, int Y, int W, Color color)
        {
            solidColorBrush.Color = color;
            device.DrawEllipse(new Ellipse(new Vector2(X, Y), W, W), solidColorBrush);
        }

        private void DrawFillCircle(int X, int Y, int W, Color color)
        {
            solidColorBrush.Color = color;
            device.FillEllipse(new Ellipse(new Vector2(X, Y), W, W), solidColorBrush);
        }

        public void DrawImage(int X, int Y, int W, int H, Bitmap bitmap)
        {
            device.DrawBitmap(bitmap, new RectangleF(X, Y, W, H), 1.0f, BitmapInterpolationMode.Linear);
        }

        public void DrawImage(int X, int Y, int W, int H, Bitmap bitmap, float angle)
        {
            device.Transform = Matrix3x2.Rotation(angle, new SharpDX.Vector2(X + (H / 2), Y + (H / 2)));
            device.DrawBitmap(bitmap, new RectangleF(X, Y, W, H), 1.0f, BitmapInterpolationMode.Linear);
            device.Transform = Matrix3x2.Rotation(0);
        }

        public void DrawSprite(RectangleF destinationRectangle, Bitmap bitmap, RectangleF sourceRectangle)
        {
            device.DrawBitmap(bitmap, destinationRectangle, 1.0f, BitmapInterpolationMode.Linear, sourceRectangle);
        }

        public Bitmap LoadFromFile(string file, out Size2 size)
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            if (B == 0xFF && G == 0xFF && R == 0xFF && A >= 0x00)
                            {
                                B = 0x00;
                                G = 0x00;
                                R = 0x00;
                                A = 0x00;
                            }
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(device, size, tempStream, stride, bitmapProperties);
                }
            }
        }
    }
}
