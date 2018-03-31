namespace UltimateSpeakerTimer
{
    partial class ColorWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.Color1 = new System.Windows.Forms.Button();
            this.Color2 = new System.Windows.Forms.Button();
            this.LeftTop = new System.Windows.Forms.CheckBox();
            this.Top = new System.Windows.Forms.CheckBox();
            this.RightTop = new System.Windows.Forms.CheckBox();
            this.RightMid = new System.Windows.Forms.CheckBox();
            this.LeftMid = new System.Windows.Forms.CheckBox();
            this.RightBottom = new System.Windows.Forms.CheckBox();
            this.Bottom = new System.Windows.Forms.CheckBox();
            this.LeftBottom = new System.Windows.Forms.CheckBox();
            this.GhostEffect = new System.Windows.Forms.Label();
            this.ClearColor2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Color1
            // 
            this.Color1.Location = new System.Drawing.Point(12, 8);
            this.Color1.Name = "Color1";
            this.Color1.Size = new System.Drawing.Size(75, 23);
            this.Color1.TabIndex = 1;
            this.Color1.Text = "Color 1";
            this.Color1.UseVisualStyleBackColor = true;
            this.Color1.Click += new System.EventHandler(this.Color1_Click);
            // 
            // Color2
            // 
            this.Color2.Location = new System.Drawing.Point(12, 41);
            this.Color2.Name = "Color2";
            this.Color2.Size = new System.Drawing.Size(75, 23);
            this.Color2.TabIndex = 2;
            this.Color2.Text = "Color 2";
            this.Color2.UseVisualStyleBackColor = true;
            this.Color2.Click += new System.EventHandler(this.Color2_Click);
            // 
            // LeftTop
            // 
            this.LeftTop.AutoSize = true;
            this.LeftTop.Location = new System.Drawing.Point(15, 106);
            this.LeftTop.Name = "LeftTop";
            this.LeftTop.Size = new System.Drawing.Size(15, 14);
            this.LeftTop.TabIndex = 3;
            this.LeftTop.UseVisualStyleBackColor = true;
            this.LeftTop.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // Top
            // 
            this.Top.AutoSize = true;
            this.Top.Location = new System.Drawing.Point(36, 106);
            this.Top.Name = "Top";
            this.Top.Size = new System.Drawing.Size(15, 14);
            this.Top.TabIndex = 4;
            this.Top.UseVisualStyleBackColor = true;
            this.Top.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // RightTop
            // 
            this.RightTop.AutoSize = true;
            this.RightTop.ForeColor = System.Drawing.Color.Black;
            this.RightTop.Location = new System.Drawing.Point(57, 106);
            this.RightTop.Name = "RightTop";
            this.RightTop.Size = new System.Drawing.Size(15, 14);
            this.RightTop.TabIndex = 5;
            this.RightTop.UseVisualStyleBackColor = true;
            this.RightTop.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // RightMid
            // 
            this.RightMid.AutoSize = true;
            this.RightMid.ForeColor = System.Drawing.Color.Black;
            this.RightMid.Location = new System.Drawing.Point(57, 126);
            this.RightMid.Name = "RightMid";
            this.RightMid.Size = new System.Drawing.Size(15, 14);
            this.RightMid.TabIndex = 8;
            this.RightMid.UseVisualStyleBackColor = true;
            this.RightMid.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // LeftMid
            // 
            this.LeftMid.AutoSize = true;
            this.LeftMid.Location = new System.Drawing.Point(15, 126);
            this.LeftMid.Name = "LeftMid";
            this.LeftMid.Size = new System.Drawing.Size(15, 14);
            this.LeftMid.TabIndex = 6;
            this.LeftMid.UseVisualStyleBackColor = true;
            this.LeftMid.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // RightBottom
            // 
            this.RightBottom.AutoSize = true;
            this.RightBottom.ForeColor = System.Drawing.Color.Black;
            this.RightBottom.Location = new System.Drawing.Point(57, 146);
            this.RightBottom.Name = "RightBottom";
            this.RightBottom.Size = new System.Drawing.Size(15, 14);
            this.RightBottom.TabIndex = 11;
            this.RightBottom.UseVisualStyleBackColor = true;
            this.RightBottom.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // Bottom
            // 
            this.Bottom.AutoSize = true;
            this.Bottom.Location = new System.Drawing.Point(36, 146);
            this.Bottom.Name = "Bottom";
            this.Bottom.Size = new System.Drawing.Size(15, 14);
            this.Bottom.TabIndex = 10;
            this.Bottom.UseVisualStyleBackColor = true;
            this.Bottom.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // LeftBottom
            // 
            this.LeftBottom.AutoSize = true;
            this.LeftBottom.Location = new System.Drawing.Point(15, 146);
            this.LeftBottom.Name = "LeftBottom";
            this.LeftBottom.Size = new System.Drawing.Size(15, 14);
            this.LeftBottom.TabIndex = 9;
            this.LeftBottom.UseVisualStyleBackColor = true;
            this.LeftBottom.Click += new System.EventHandler(this.CheckBox_Click);
            // 
            // GhostEffect
            // 
            this.GhostEffect.AutoSize = true;
            this.GhostEffect.Location = new System.Drawing.Point(12, 79);
            this.GhostEffect.Name = "GhostEffect";
            this.GhostEffect.Size = new System.Drawing.Size(164, 13);
            this.GhostEffect.TabIndex = 12;
            this.GhostEffect.Text = "Checked == Gradient, else Ghost";
            // 
            // ClearColor2
            // 
            this.ClearColor2.Location = new System.Drawing.Point(101, 41);
            this.ClearColor2.Name = "ClearColor2";
            this.ClearColor2.Size = new System.Drawing.Size(75, 23);
            this.ClearColor2.TabIndex = 13;
            this.ClearColor2.Text = "Clear";
            this.ClearColor2.UseVisualStyleBackColor = true;
            this.ClearColor2.Click += new System.EventHandler(this.ClearColor2_Click);
            // 
            // FontColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 174);
            this.Controls.Add(this.ClearColor2);
            this.Controls.Add(this.GhostEffect);
            this.Controls.Add(this.RightBottom);
            this.Controls.Add(this.Bottom);
            this.Controls.Add(this.LeftBottom);
            this.Controls.Add(this.RightMid);
            this.Controls.Add(this.LeftMid);
            this.Controls.Add(this.RightTop);
            this.Controls.Add(this.Top);
            this.Controls.Add(this.LeftTop);
            this.Controls.Add(this.Color2);
            this.Controls.Add(this.Color1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FontColor";
            this.Text = "FontColor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button Color1;
        private System.Windows.Forms.Button Color2;
        private System.Windows.Forms.CheckBox LeftTop;
        private System.Windows.Forms.CheckBox Top;
        private System.Windows.Forms.CheckBox RightTop;
        private System.Windows.Forms.CheckBox RightMid;
        private System.Windows.Forms.CheckBox LeftMid;
        private System.Windows.Forms.CheckBox RightBottom;
        private System.Windows.Forms.CheckBox Bottom;
        private System.Windows.Forms.CheckBox LeftBottom;
        private System.Windows.Forms.Label GhostEffect;
        private System.Windows.Forms.Button ClearColor2;
    }
}