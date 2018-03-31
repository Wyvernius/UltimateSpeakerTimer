using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Factory = SharpDX.Direct2D1.Factory;
using FontFactory = SharpDX.DirectWrite.Factory;
using Format = SharpDX.DXGI.Format;
using Font = System.Drawing.Font;
using OrigColor = System.Drawing.Color;
using Color = SharpDX.Color;
using Rectangle = System.Drawing.Rectangle;
using RectangleF = SharpDX.RectangleF;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using Brush = SharpDX.Direct2D1.Brush;

namespace UltimateSpeakerTimer
{
    public partial class SettingsForm : Form
    {
        private Thread PreviewThread = null, KeyThread = null;
        public WindowRenderTarget device;
        private HwndRenderTargetProperties renderProperties;
        private static SolidColorBrush solidColorBrush;
        private Factory factory;
        private FontFactory fontFactory;
        public static timerForm timerForm;
        public int PreviewMessageLoopPosX;
        public Bitmap bitmap;
        public Size2 bitmapSize;
        public Font SettingsFont;
        public int MessageTime;
        public Color4 SettingsClearColor = new Color4(100, 100, 100, 100);
        public Vector2 MouseLoc;
        public Font PickedFont;
        public Font PickedMessageFont;
        public DxFont SpeakerFont;
        public Size TitelBarSize;

        public static Brush LGradientBrushTimer;
        public Brush LGradientBrushBG;

        SpeakerList speakerList = new SpeakerList();

        public bool Pauzed = false;

        public RectangleF pictureBoxRect;

        public FontItem TextFocus = FontItem.None;
        bool keyDown = false;

        GradientStopCollection GSC = null; 

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (!TimerClass.Active(TimerType.CountDown) && !Pauzed)
            {
                Countdown.Hours = (int)HoursUpDown.Value;
                Countdown.Minutes = (int)MinuteUpDown.Value;
                Countdown.Seconds = (int)secondUpDown.Value;
                timerForm.Show();
                PlaySound.PlayStartSound();
                TimerClass.Start(TimerType.CountDown);
                TimerClass.Start(TimerType.TimerAnnimation);
            }
            else if (TimerClass.Active(TimerType.CountDown) && !Pauzed)
            {
                Pauzed = true;
                TimerClass.Stop(TimerType.CountDown);
            }
            else if (!TimerClass.Active(TimerType.CountDown) && Pauzed)
            {
                Pauzed = false;
                TimerClass.Start(TimerType.CountDown);
            }
        }

        private void Start_Sound_Select_Button_Click(object sender, EventArgs e)
        {
            //  SelectSoundFileDialog.Filter = "(*.mp3)|*mp3|(*.wav)|*.wav";
            if (SelectSoundFileDialog.ShowDialog() == DialogResult.OK)
            {
                PlaySound.StartSound = SelectSoundFileDialog.FileName;
                Selected_Start_Sound.Text = SelectSoundFileDialog.SafeFileName;
            }
        }

        private void Select_End_Sound_Click(object sender, EventArgs e)
        {
            //  SelectSoundFileDialog.Filter = "(*.mp3)|*mp3|(*.wav)|*.wav";
            if (SelectSoundFileDialog.ShowDialog() == DialogResult.OK)
            {
                PlaySound.EndSound = SelectSoundFileDialog.FileName;
                Selected_End_Sound.Text = SelectSoundFileDialog.SafeFileName; // label
            }
        }

        public SettingsForm()
        {
            InitializeComponent();
            TimerClass.MainThreadTimerStart();
            TimerClass.newTimer(1000, TimerType.CountDown, Countdown.TimeCountDown); // CountDownTimer.
            TimerClass.newTimer(100, TimerType.Keys, Keytimer_Tick);
            TimerClass.Start(TimerType.Keys);
            TimerClass.newTimer(500, TimerType.TimerAnnimation, CountDownEffectClass.ShowTimer);
        }

        private void Font_Select_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = PickedFont;
            fontDialog1.AllowSimulations = true;
            fontDialog1.ShowEffects = false;

            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                // Preview selected font.
                FontName.Font = new Font(fontDialog1.Font.FontFamily, 18f, fontDialog1.Font.Style);
                FontName.Text = fontDialog1.Font.Name;

                PickedFont = new Font(fontDialog1.Font.FontFamily, fontDialog1.Font.Size, fontDialog1.Font.Style);

                DxFont PreviewFont = new DxFont(PickedFont, FontItem.Time);
                DxFont MainFont = new DxFont(PickedFont, FontItem.MainTime,1.0f,GetRatioTimerForm());

                DxFont PreviewNameFont = new DxFont(PickedFont, FontItem.Name, 1.0f / 3.0f);
                DxFont MainNameFont = new DxFont(PickedFont, FontItem.MainName, 1.0f / 3.0f, GetRatioTimerForm());

                DxFont PreviewMessageFont = new DxFont(PickedFont, FontItem.Message, 1.0f / 3.0f);
                DxFont MainMessageFont = new DxFont(PickedFont, FontItem.MainMessage, 1.0f / 3.0f, GetRatioTimerForm());

                PreviewFont.SetText(Countdown.GetTime());
                MainFont.SetText(Countdown.GetTime());

                // Update Fonts
                if (Shared.Fonts.Count > 0)
                {
                    for (int i = 0; i < Shared.Fonts.Count; i++)
                    {
                        if (Shared.Fonts[i].TypeID == FontItem.Time)
                            Shared.Fonts[i] = PreviewFont;
                        if (Shared.Fonts[i].TypeID == FontItem.Name)
                            Shared.Fonts[i] = PreviewNameFont;
                        if (Shared.Fonts[i].TypeID == FontItem.Message)
                            Shared.Fonts[i] = PreviewMessageFont;

                        if (Shared.Fonts[i].TypeID == FontItem.MainTime)
                            Shared.Fonts[i] = MainFont;
                        if (Shared.Fonts[i].TypeID == FontItem.MainName)
                            Shared.Fonts[i] = MainNameFont;
                        if (Shared.Fonts[i].TypeID == FontItem.MainMessage)
                            Shared.Fonts[i] = MainMessageFont;
                    }
                    return;
                }
                else
                {
                    Shared.Fonts.Add(PreviewFont);
                    Shared.Fonts.Add(MainFont);
                    Shared.Fonts.Add(PreviewNameFont);
                    Shared.Fonts.Add(MainNameFont);
                    Shared.Fonts.Add(PreviewMessageFont);
                    Shared.Fonts.Add(MainMessageFont);
                }
            }
        }

        private void Stop_Timer_Click(object sender, EventArgs e)
        {
            if (timerForm != null)
            {
                Pauzed = false;
                timerForm.Hide();
                TimerClass.Stop(TimerType.CountDown);
                // Reset Time;
                Countdown.Hours = (int)HoursUpDown.Value;
                Countdown.Minutes = (int)MinuteUpDown.Value;
                Countdown.Seconds = (int)secondUpDown.Value;

            }
        }


        private void UpDownValue_Changed(object sender,EventArgs e)
        {
            if (!TimerClass.Active(TimerType.CountDown))
            {
                Countdown.Hours = (int)HoursUpDown.Value;
                Countdown.Minutes = (int)MinuteUpDown.Value;
                Countdown.Seconds = (int)secondUpDown.Value;
            }
        }

        private void Picture_Select_Click(object sender, EventArgs e)
        {
            if (SelectSoundFileDialog.ShowDialog() == DialogResult.OK)
            {
                timerForm.Picture = SelectSoundFileDialog.FileName;
                timerForm.bitmap = timerForm.LoadFromFile(timerForm.Picture, out timerForm.bitmapSize);
                bitmap = LoadFromFile(timerForm.Picture, out bitmapSize);
            }
        }

        private void FontColor_Click(object sender, EventArgs e)
        {
            ColorWindow ColorWindow = new ColorWindow(ColorType.Font);
            if (ColorWindow.ShowDialog() == DialogResult.Cancel)
            {
                GetBrush(ColorType.Font);
                timerForm.GetBrush(ColorType.Font);
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            pictureBoxRect = new RectangleF(pictureBox1.Location.X, pictureBox1.Location.Y, pictureBox1.Size.Width, pictureBox1.Size.Height);
            timerForm = new timerForm(this);
            Screen[] Screens = Screen.AllScreens;
            int i = 0;
            foreach (Screen screen in Screens)
            {
                string[] Row = { i.ToString(), screen.DeviceName, screen.Primary.ToString(), screen.Bounds.Width + "/" + screen.Bounds.Height };
                ScreenListView.Items.Add(new ListViewItem(Row));
                i++;
            }

            ScreenListView.Items[0].Focused = true;
            foreach (Screen screen in Screens)
            {
                if (ScreenListView.FocusedItem.SubItems[1].Text == screen.DeviceName)
                {
                    ScreenListView.FocusedItem.Selected = true;
                }
            }
            InitPreviewWindow(timerForm.device.Size);
            PreviewThread = new Thread(new ParameterizedThreadStart(PreviewLoop));
            PreviewThread.Start();
            MessageTime = (int)MessageTimeSetter.Value;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                if (ScreenListView.FocusedItem.SubItems[1].Text == screen.DeviceName)
                {
                    timerForm.StartPosition = FormStartPosition.Manual;
                    timerForm.Location = screen.Bounds.Location;
                    timerForm.device.Resize(new SharpDX.Size2(screen.Bounds.Width, screen.Bounds.Height));
                    resizePreviewRect();
                }

            }
        }

        private void resizePreviewRect()
        {
            pictureBoxRect.Height = ((float)timerForm.device.Size.Height / (float)timerForm.device.Size.Width) * (float)pictureBoxRect.Width;
            Console.WriteLine(((float)pictureBoxRect.Size.Width / (float)pictureBoxRect.Size.Height) + "," + ((float)timerForm.device.Size.Width / (float)timerForm.device.Size.Height));
        }

        private void BG_Color_Click(object sender, EventArgs e)
        {
            ColorWindow ColorWindow = new ColorWindow(ColorType.Background);
            if (ColorWindow.ShowDialog() == DialogResult.Cancel)
            {

            }
        }

        private void Send_Message_Button_Click(object sender, EventArgs e)
        {
            AnnimationMessage.BaseTime = (int)MessageTimeSetter.Value;
            AnnimationMessage.Send = true;
        }

        private void Static_Message_CheckedChanged(object sender, EventArgs e)
        {
            AnnimationMessage.static_Message = Static_Message.Checked;
        }

        private void InitPreviewWindow(Size2F Size)
        {
            // Init factory
            factory = new Factory();
            fontFactory = new FontFactory();

            // Render settings
            renderProperties = new HwndRenderTargetProperties()
            {
                Hwnd = this.Handle,
                PixelSize = new Size2((int)Size.Width, (int)Size.Height),
                PresentOptions = PresentOptions.None
            };

            // Init device
            device = new WindowRenderTarget(factory, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderProperties);

            // Init brush
            solidColorBrush = new SolidColorBrush(device, new Color4(255, 255, 255, 0));
        }

        public float GetRatioTimerForm()
        {
            return timerForm.device.Size.Width / pictureBoxRect.Width;
        }

        public void PreviewLoop(object sender)
        {
            while (true)
            {
                try
                {
                    device.Resize(new Size2(this.Width,this.Height));
                    device.BeginDraw();
                    device.Clear(SettingsClearColor);

                    GetBrush(ColorType.Background);
                    // Background Rendering
                    DrawRect(pictureBoxRect.Location.X, pictureBoxRect.Location.Y, pictureBoxRect.Size.Width, pictureBoxRect.Size.Height, Color.Red);

                    if (LGradientBrushBG != null)
                        DrawFillRect(pictureBoxRect.Location.X + 1, pictureBoxRect.Location.Y + 1, pictureBoxRect.Size.Width - 1, pictureBoxRect.Height - 2, LGradientBrushBG);
                    else
                        DrawFillRect(pictureBoxRect.Location.X + 1, pictureBoxRect.Location.Y + 1, pictureBoxRect.Size.Width - 1, pictureBoxRect.Height - 2, TimerColors.BGColor1);

                    // Picture Rendering
                    if (bitmap != null)
                        DrawImage((int)pictureBoxRect.Location.X, (int)pictureBoxRect.Location.Y, (int)pictureBoxRect.Size.Width, (int)pictureBoxRect.Size.Height, bitmap);

                    // Time Rendering
                    for (int  i = 0; i < Shared.Fonts.Count;i++)
                    {
                        DxFont Font = Shared.Fonts[i];
                        if (Font.TypeID == FontItem.Time)
                        {
                            if (CountDownEffectClass.ShowTimerText)
                            {
                                Font.SetText(Countdown.GetTime());
                                DrawRect(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Color.Blue);
                                
                                GetBrush(ColorType.Font);
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
                                    DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, Pricolor, Font.GetFormat(), TimerColors.FontColor2);
                                }
                                else
                                    DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, LGradientBrushTimer, Font.GetFormat());
                            }
                        }
                        if (Font.TypeID == FontItem.Name)
                        {
                            DrawRect(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Color.Blue);
                            DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, TimerColors.FontColor1, Font.GetFormat());
                        }
                        if (Font.TypeID == FontItem.Message)
                        {
                            if (AnnimationMessage.static_Message)
                            {
                                AnnimationMessage.MessageFocus = false;
                                if (!AnnimationMessage.Pos.IsZero && Font.txtRect.Location != AnnimationMessage.Pos)
                                {
                                    // restore backup;
                                    Font.txtRect.Location = AnnimationMessage.Pos; // backup pos;
                                    Shared.Fonts[i + 1].txtRect.Location = AnnimationMessage.PosMain;
                                    AnnimationMessage.Pos = new Vector2(); // zero
                                }

                                if (AnnimationMessage.Send)
                                {
                                    if (!AnnimationMessage.running)
                                        AnnimationMessage.Start();
                                }
                                else
                                {
                                    DrawRect(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Color.Blue);
                                    DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, TimerColors.FontColor1, Font.GetFormat());
                                }
                                if (AnnimationMessage.Active())
                                {
                                    DrawRect(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Color.Red);
                                    DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, TimerColors.FontColor1, Font.GetFormat());
                                }
                                if (AnnimationMessage.stopped && AnnimationMessage.running)
                                {
                                    AnnimationMessage.Send = false;
                                    AnnimationMessage.running = false;
                                }

                            }
                            else
                            {
                                if (AnnimationMessage.Pos.IsZero)
                                {
                                    AnnimationMessage.Pos = Font.txtRect.Location;
                                    AnnimationMessage.PosMain = Shared.Fonts[i + 1].txtRect.Location;
                                }
                                AnnimationMessage.MessageFocus = true;

                                if (AnnimationMessage.Send)
                                {
                                    if (!AnnimationMessage.running)
                                    {
                                        AnnimationMessage.Start();
                                        Font.txtRect.X = pictureBox1.Right;

                                    }
                                }
                                if (!AnnimationMessage.Active() && !AnnimationMessage.running)
                                {
                                    if (!Font.txtRect.Intersects(pictureBoxRect))
                                    {
                                        Font.txtRect.X = pictureBoxRect.Right;
                                    }
                                    if (Font.txtRect.Right > 0)
                                    {
                                        SettingsForm_MouseMove(AnimMove.Move, new MouseEventArgs(MouseButtons.None, 0, (int)-AnnimationMessage.speed, 0, 0));
                                    }
                                    DrawRect(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Color.Blue);
                                    DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, TimerColors.FontColor1, Font.GetFormat());
                                }
                                if (AnnimationMessage.running)
                                {
                                    if (!Font.txtRect.Intersects(pictureBoxRect))
                                    {
                                        Font.txtRect.X = pictureBoxRect.Right;
                                        if (AnnimationMessage.stopped && AnnimationMessage.running)
                                        {
                                            AnnimationMessage.running = false;
                                            AnnimationMessage.Send = false;
                                        }
                                    }
                                    if (Font.txtRect.Right > 0)
                                        SettingsForm_MouseMove(AnimMove.Move, new MouseEventArgs(MouseButtons.None, 0, (int)-AnnimationMessage.speed, 0, 0));

                                    DrawRect(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Color.Red);
                                    DrawTextCenter(Font.txtRect.X, Font.txtRect.Y, Font.txtRect.Width, Font.txtRect.Height, Font.txt, TimerColors.FontColor1, Font.GetFormat());
                                }
                            }
                        }
                                
                    }
                    device.EndDraw();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                   // ConsoleOutput.Dispose();
                }
            }
        }

        public void GetBrush(ColorType Type)
        {
            //Console.WriteLine("Type = {0} , EffectTimer = {1} , EffectBG = {2}", Type, TimerEffects.GradientEffectTimer, TimerEffects.GradientEffectBG);
            try
            {
                if (Type == ColorType.Font)
                {
                    if (LGradientBrushTimer != null)
                        LGradientBrushTimer.Dispose();
                    if (TimerEffects.GradientEffectTimer != "")
                    {
                        foreach (DxFont Font in Shared.Fonts)
                        {
                            if (Font.TypeID == FontItem.Time)
                            {
                                GradientStop[] GS = new GradientStop[2];
                                GS[0].Color = TimerColors.FontColor1;
                                GS[0].Position = 0.0f;
                                GS[1].Color = TimerColors.FontColor2;
                                GS[1].Position = 1.0f;
                                GradientStopCollection GSC = new GradientStopCollection(device, GS);

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
                                }
                                if (TimerEffects.GradientEffectTimer == "RightMid")
                                {
                                    LGradientBrushTimer = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                                    {
                                        StartPoint = new Vector2(Font.txtRect.Right, Font.txtRect.Y + (Font.txtRect.Height / 2)),
                                        EndPoint = new Vector2(Font.txtRect.X, Font.txtRect.Y + (Font.txtRect.Height / 2))
                                    }, GSC);
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
                                {
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
                        }
                    }
                    else
                        LGradientBrushTimer = null;
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
                                StartPoint = new Vector2(pictureBoxRect.X, pictureBoxRect.Y),
                                EndPoint = new Vector2(pictureBoxRect.Right, pictureBoxRect.Bottom)
                            }, GSC);
                        }
                        if (TimerEffects.GradientEffectBG == "Top")
                        {
                            LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                            {
                                StartPoint = new Vector2(pictureBoxRect.X + (pictureBoxRect.Width / 2), pictureBoxRect.Top),
                                EndPoint = new Vector2(pictureBoxRect.X + (pictureBoxRect.Width / 2), pictureBoxRect.Bottom)
                            }, GSC);
                        }
                        if (TimerEffects.GradientEffectBG == "RightTop")
                        {
                            LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                            {
                                StartPoint = new Vector2(pictureBoxRect.Right, pictureBoxRect.Top),
                                EndPoint = new Vector2(pictureBoxRect.X, pictureBoxRect.Bottom)
                            }, GSC);
                        }
                        if (TimerEffects.GradientEffectBG == "LeftMid")
                        {
                            LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                            {
                                StartPoint = new Vector2(pictureBoxRect.X, pictureBoxRect.Y + (pictureBoxRect.Height / 2)),
                                EndPoint = new Vector2(pictureBoxRect.Right, pictureBoxRect.Y + (pictureBoxRect.Height / 2))
                            }, GSC);
                        }
                        if (TimerEffects.GradientEffectBG == "RightMid")
                        {
                            LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                            {
                                StartPoint = new Vector2(pictureBoxRect.Right, pictureBoxRect.Y + (pictureBoxRect.Height / 2)),
                                EndPoint = new Vector2(pictureBoxRect.X, pictureBoxRect.Y + (pictureBoxRect.Height / 2))
                            }, GSC);
                        }
                        if (TimerEffects.GradientEffectBG == "LeftBottom")
                        {
                            LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                            {
                                StartPoint = new Vector2(pictureBoxRect.X, pictureBoxRect.Bottom),
                                EndPoint = new Vector2(pictureBoxRect.Right, pictureBoxRect.Top)
                            }, GSC);
                        }
                        if (TimerEffects.GradientEffectBG == "Bottom")
                        {
                            LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                            {
                                StartPoint = new Vector2(pictureBoxRect.X + (pictureBoxRect.Width / 2), pictureBoxRect.Bottom),
                                EndPoint = new Vector2(pictureBoxRect.X + (pictureBoxRect.Width / 2), pictureBoxRect.Top)
                            }, GSC);
                        }
                        if (TimerEffects.GradientEffectBG == "RightBottom")
                        {
                            LGradientBrushBG = new LinearGradientBrush(device, new LinearGradientBrushProperties()
                            {
                                StartPoint = new Vector2(pictureBoxRect.Right, pictureBoxRect.Bottom),
                                EndPoint = new Vector2(pictureBoxRect.X, pictureBoxRect.Top)
                            }, GSC);
                        }
                        GSC.Dispose();
                    }
                    else
                        LGradientBrushBG = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #region Drawing
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
            // solidColorBrush.Color = color;
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

                    progressBar1.Show();
                    progressBar1.Value = 0;
                    progressBar1.Maximum = bitmap.Width * bitmap.Height;
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
                            /*
                            if (B == 0xFF && G == 0xFF && R == 0xFF && A >= 0x00)
                            {
                                B = 0x00;
                                G = 0x00;
                                R = 0x00;
                                A = 0x00;
                            }
                            */
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }
                        progressBar1.Value += bitmap.Width;
                        progressBar1.Update();
                    }
                    progressBar1.Hide();
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(device, size, tempStream, stride, bitmapProperties);
                }
            }
        }

        #endregion

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PreviewThread.Abort();
        }

        private void Message_TextBox_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Shared.Fonts.Count; i++)
            {
                if (Shared.Fonts[i].TypeID == FontItem.Message)
                {
                    Shared.Fonts[i].SetText(Message_TextBox.Text);
                }
                if (Shared.Fonts[i].TypeID == FontItem.MainMessage)
                {
                    Shared.Fonts[i].SetText(Message_TextBox.Text);
                }
            }
        }



      

        private void MessageTimeSetter_ValueChanged(object sender, EventArgs e)
        {
            AnnimationMessage.BaseTime = (int)MessageTimeSetter.Value;
        }

        private void SettingsForm_MouseMove(object sender, MouseEventArgs e)
        {
            float DifX;
            float DifY;
            if (sender != this)
            {
                DifX = e.X;
                DifY = e.Y;
            }
            else
            {
                Vector2 m = new Vector2(e.Location.X, e.Y);
                DifX = (m.X > MouseLoc.X) ? m.X - MouseLoc.X : (m.X < MouseLoc.X) ? m.X - MouseLoc.X : 0;
                DifY = (m.Y > MouseLoc.Y) ? m.Y - MouseLoc.Y : (m.Y < MouseLoc.Y) ? m.Y - MouseLoc.Y : 0;
                MouseLoc = m;
            }

            Vector2 DifCanvas = new Vector2(timerForm.device.Size.Width / pictureBoxRect.Size.Width,
            timerForm.device.Size.Height / pictureBoxRect.Size.Height);
            for (int i = 0; i < Shared.Fonts.Count; i++)
            {
                if (TextFocus == FontItem.Time && Shared.Fonts[i].TypeID == FontItem.Time)
                {
                    if (InPictureBoundsX(Shared.Fonts[i].txtRect, DifX))
                        Shared.Fonts[i].txtRect.X += DifX;
                    if (InPictureBoundsY(Shared.Fonts[i].txtRect, DifY))
                        Shared.Fonts[i].txtRect.Y += DifY;
                    Shared.Fonts[i + 1].txtRect.Location = Shared.Fonts[i].txtRect.Location * DifCanvas;
                    break;

                }

                if ((TextFocus == FontItem.Message || sender  != this)  && Shared.Fonts[i].TypeID == FontItem.Message)
                {
                    if (InPictureBoundsX(Shared.Fonts[i].txtRect, DifX) && sender == this)
                        Shared.Fonts[i].txtRect.X += DifX;
                    else
                        Shared.Fonts[i].txtRect.X += DifX;

                    if (InPictureBoundsY(Shared.Fonts[i].txtRect, DifY))
                        Shared.Fonts[i].txtRect.Y += DifY;
                    Shared.Fonts[i + 1].txtRect.Location = Shared.Fonts[i].txtRect.Location * DifCanvas;
                    break;

                }

                if (TextFocus == FontItem.Name && Shared.Fonts[i].TypeID == FontItem.Name)
                {
                    if (InPictureBoundsX(Shared.Fonts[i].txtRect, DifX))
                        Shared.Fonts[i].txtRect.X += DifX;
                    if (InPictureBoundsY(Shared.Fonts[i].txtRect, DifY))
                        Shared.Fonts[i].txtRect.Y += DifY;
                    Shared.Fonts[i + 1].txtRect.Location = Shared.Fonts[i].txtRect.Location * DifCanvas;
                    break;

                }
            }
        }

        public bool InPictureBoundsX(RectangleF b, float i)
        {
            if (b.Left + i > pictureBoxRect.Left && b.Right + i < pictureBoxRect.Right)
                return true;
            return false;
        }

        public bool InPictureBoundsY(RectangleF b, float i)
        {
            if (b.Top + i > pictureBoxRect.Top && b.Bottom + i < pictureBoxRect.Bottom)
                return true;
            return false;
        }

        private void SettingsForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Drawing.Point m = new System.Drawing.Point(e.X, e.Y);
                foreach (DxFont Font in Shared.Fonts)
                {
                    if (Font.TypeID == FontItem.Time)
                    {
                        if (TextFocus == FontItem.Time)
                            TextFocus = FontItem.None;
                        else if (Font.txtRect.Intersects(new RectangleF(e.X,e.Y,1,1)) && TextFocus == FontItem.None)
                            TextFocus = FontItem.Time;
                    }

                    if (Font.TypeID == FontItem.Message)
                    {
                        if (TextFocus == FontItem.Message)
                            TextFocus = FontItem.None;
                        else if (Font.txtRect.Intersects(new RectangleF(e.X, e.Y, 1, 1)) && TextFocus == FontItem.None)
                            TextFocus = FontItem.Message;
                    }

                    if (Font.TypeID == FontItem.Name)
                    {
                        if (TextFocus == FontItem.Name)
                            TextFocus = FontItem.None;
                        else if (Font.txtRect.Intersects(new RectangleF(e.X, e.Y, 1, 1)) && TextFocus == FontItem.None)
                            TextFocus = FontItem.Name;
                    }
                }
            }
        }

        private void MessageFontButton_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = PickedMessageFont == null ? PickedFont : PickedMessageFont;
            fontDialog1.ShowEffects = false;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                PickedMessageFont = new Font(fontDialog1.Font.FontFamily, fontDialog1.Font.Size, fontDialog1.Font.Style);
                DxFont PreviewMessageFont = new DxFont(PickedMessageFont, FontItem.Message);
                DxFont MainMessageFont = new DxFont(PickedMessageFont, FontItem.MainMessage, 1.0f, GetRatioTimerForm());
                DxFont Backup;
                if (Shared.Fonts.Count > 0)
                {
                    for (int i = 0; i < Shared.Fonts.Count; i++)
                    {
                        if (Shared.Fonts[i].TypeID == FontItem.Message)
                        {
                            Backup = Shared.Fonts[i];
                            Shared.Fonts[i] = PreviewMessageFont;
                            Shared.Fonts[i].SetText(Backup.txt);
                        }
                        if (Shared.Fonts[i].TypeID == FontItem.MainMessage)
                        {
                            Backup = Shared.Fonts[i];
                            Shared.Fonts[i] = MainMessageFont;
                            Shared.Fonts[i].SetText(Backup.txt);
                        }
                    }
                }
                else
                {
                    Shared.Fonts.Add(PreviewMessageFont);
                    Shared.Fonts.Add(MainMessageFont);
                }
            }
        }

        private void TransperantcheckBox_Click(object sender, EventArgs e)
        {
            timerForm.TransperantBG = TransperantcheckBox.Checked;
        }

        private void CountDownEffectButton_Click(object sender, EventArgs e)
        {
            CountDownEffects CDE = new CountDownEffects(this);
            CDE.Show();
        }

        private void Save_Speaker_Click(object sender, EventArgs e)
        {
            if (SpeakerName.Text == "" || SpeakerName.Text == null)
            {
                MessageBox.Show("Enter name First!");
                return;
            }

            SpeakerList.Speaker spkr = new SpeakerList.Speaker();
            spkr.Name = SpeakerName.Text;
            spkr.Hours = (int)HoursUpDown.Value;
            spkr.Minutes = (int)MinuteUpDown.Value;
            spkr.Seconds = (int)secondUpDown.Value;

            bool InList = false;
            foreach (SpeakerList.Speaker speaker in speakerList.List)
            {
                if (speaker.Name == SpeakerName.Text)
                    InList = true;

            }
            if (!InList)
                speakerList.List.Add(spkr);
            else
                MessageBox.Show("Name already exists in list!");

            UpdateSpeakerList(0);
        }

        private void SpeakerUp_Click(object sender, EventArgs e)
        {
            if (speakerList.List.Count > 0)
            {
                ListViewItem SelectedFromList = SpeakerListView.Items[SpeakerListView.SelectedIndices[0]];
                SpeakerList.Speaker NewSpeaker = new SpeakerList.Speaker(SelectedFromList.SubItems[1].Text, int.Parse(SelectedFromList.SubItems[2].Text), int.Parse(SelectedFromList.SubItems[3].Text), int.Parse(SelectedFromList.SubItems[4].Text));
                if (SpeakerListView.SelectedIndices[0] != 0)
                {
                    speakerList.List.RemoveAt(SpeakerListView.SelectedIndices[0]);
                    speakerList.List.Insert(SpeakerListView.SelectedIndices[0] - 1, NewSpeaker);
                    UpdateSpeakerList(SpeakerListView.SelectedIndices[0] - 1);
                }
            }
        }

        private void SpeakerDown_Click(object sender, EventArgs e)
        {
            if (speakerList.List.Count > 0)
            {
                ListViewItem SelectedFromList = SpeakerListView.Items[SpeakerListView.SelectedIndices[0]];
                SpeakerList.Speaker NewSpeaker = new SpeakerList.Speaker(SelectedFromList.SubItems[1].Text, int.Parse(SelectedFromList.SubItems[2].Text),int.Parse(SelectedFromList.SubItems[3].Text), int.Parse(SelectedFromList.SubItems[4].Text));
                if (SpeakerListView.SelectedIndices[0] != speakerList.List.Count - 1)
                {
                    speakerList.List.RemoveAt(SpeakerListView.SelectedIndices[0]);
                    speakerList.List.Insert(SpeakerListView.SelectedIndices[0] + 1, NewSpeaker);
                    UpdateSpeakerList(SpeakerListView.SelectedIndices[0] + 1);
                }
            }
        }

        public void UpdateSpeakerList(int Index = -1)
        {
            SpeakerListView.Items.Clear();
            int i = 1;
            foreach (SpeakerList.Speaker Speaker in speakerList.List)
            {
                string[] item = { i.ToString(), Speaker.Name,Speaker.Hours.ToString(), Speaker.Minutes.ToString(), Speaker.Seconds.ToString()};
                SpeakerListView.Items.Add(new ListViewItem(item));
                i++;
            }
            if (Index != -1)
                SpeakerListView.Items[Index].Selected = true;

        }

        private void NextSpeaker_Click(object sender, EventArgs e)
        {
            if (speakerList.List.Count > 0)
            {
                if (timerForm.Visible)
                {
                    TimerClass.Stop(TimerType.CountDown);
                    int SelIndex = SpeakerListView.SelectedIndices[0];
                    if (SelIndex + 1 != SpeakerListView.Items.Count)
                    {
                        SpeakerListView.Items[SelIndex + 1].Selected = true;
                        SpeakerList.Speaker spkr = speakerList.List[SpeakerListView.SelectedIndices[0]];
                        Countdown.Hours = spkr.Hours;
                        Countdown.Minutes = spkr.Minutes;
                        Countdown.Seconds = spkr.Seconds;
                        timerForm.SpeakerName = spkr.Name;
                    }
                    TimerClass.Start(TimerType.CountDown);
                }
                else
                {
                    int SelIndex = SpeakerListView.SelectedIndices[0];
                    SpeakerList.Speaker spkr = speakerList.List[SpeakerListView.SelectedIndices[0]];
                    HoursUpDown.Value = spkr.Hours;
                    MinuteUpDown.Value = spkr.Minutes;
                    secondUpDown.Value = spkr.Seconds;
                    SpeakerName.Text = spkr.Name;

                    StartButton_Click(null, null);
                }
            }
        }

        private void ClearBitmap_Click(object sender, EventArgs e)
        {
            bitmap = null;
            timerForm.Picture = null;
            timerForm.bitmap = null;
            timerForm.bitmapSize = new Size2();
        }

        private void SettingsForm_Click(object sender, EventArgs e)
        {
            this.Focus();
            this.Activate();
        }

        private void SpeakerName_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Shared.Fonts.Count;i++)
            {
                if (Shared.Fonts[i].TypeID == FontItem.Name)
                {
                    Shared.Fonts[i].SetText(SpeakerName.Text);
                }
                if (Shared.Fonts[i].TypeID == FontItem.MainName)
                {
                    Shared.Fonts[i].SetText(SpeakerName.Text);
                }
            }
        }

        private void startsoundClear_Click(object sender, EventArgs e)
        {
            PlaySound.StartSound = "";
        }

        private void EndSoundClear_Click(object sender, EventArgs e)
        {
            PlaySound.EndSound = "";
        }

        private void showNameChBx_CheckedChanged(object sender, EventArgs e)
        {
            timerForm.ShowName = showNameChBx.Checked;
        }

        private void Delete_Speaker_Click(object sender, EventArgs e)
        {
            if (SpeakerListView.SelectedIndices.Count > 0)
            {
                if (speakerList.List.Count > 0)
                {
                    speakerList.List.RemoveAt(SpeakerListView.SelectedIndices[0]);
                    Console.WriteLine("Removed Speaker At Index " + SpeakerListView.SelectedIndices[0] +  ", List Contains " + speakerList.List.Count + " Items");
                    if (speakerList.List.Count >= 1)
                    {
                        if (speakerList.List.Count - 1 < SpeakerListView.SelectedIndices[0])
                        {
                            UpdateSpeakerList(SpeakerListView.SelectedIndices[0] - 1);
                        }
                        else
                            UpdateSpeakerList(SpeakerListView.SelectedIndices[0]);
                    }
                    else
                        UpdateSpeakerList();
                }
            }
        }

        private void SettingsForm_ResizeBegin(object sender, EventArgs e)
        {
            var test = 0;
        }

        private int Keytimer_Tick()
        {
           if (TimerClass.Active(TimerType.CountDown) || Pauzed)
            {
                int Add = 0;
                if (MouseInput.IsKeyDown((int)MouseInput.VirtualKeysShort.Shift))
                    Add = -1;
                else
                    Add = 1;

                if (MouseInput.IsKeyDown((int)MouseInput.VirtualKeysShort.S))
                {
                    Countdown.Seconds += Add;
                    keyDown = true;
                    if (Countdown.Seconds > 59)
                    {
                        Countdown.Seconds = 0;
                        Countdown.Minutes += 1;
                        if (Countdown.Minutes > 59)
                        {
                            Countdown.Minutes = 0;
                            Countdown.Hours++;
                        }
                    }
                    if (Countdown.Seconds < 0)
                    {
                        Countdown.Seconds = 59;
                        Countdown.Minutes -= 1;
                        if (Countdown.Minutes < 0)
                        {
                            Countdown.Minutes = 59;
                            if (Countdown.Hours > 0)
                                Countdown.Hours--;
                        }
                    }
                }
                if (MouseInput.IsKeyDown((int)MouseInput.VirtualKeysShort.M))
                {
                    Countdown.Minutes += Add;
                    keyDown = true;
                    if (Countdown.Minutes > 59)
                    {
                        Countdown.Minutes = 0;
                        Countdown.Hours++;
                    }
                    if (Countdown.Minutes < 0)
                    {
                        Countdown.Minutes = 59;
                        if (Countdown.Hours > 0)
                            Countdown.Hours--;
                    }
                }

                if (MouseInput.IsKeyDown((int)MouseInput.VirtualKeysShort.H))
                {
                    keyDown = true;
                    if (Countdown.Hours < 23 && Add > 0)
                        Countdown.Hours += Add;
                    if (Countdown.Hours > 0 && Add < 0)
                        Countdown.Hours += Add;
                }

                if (!MouseInput.IsKeyDown((int)MouseInput.VirtualKeysShort.M) && !MouseInput.IsKeyDown((int)MouseInput.VirtualKeysShort.S))
                    keyDown = false;
            }
            return 0;
        }
    }
}
