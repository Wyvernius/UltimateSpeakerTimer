namespace UltimateSpeakerTimer
{
    partial class CountDownEffect
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
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.blinkZero = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.CDC1Blink = new System.Windows.Forms.CheckBox();
            this.CDC2Blink = new System.Windows.Forms.CheckBox();
            this.CountOvertime = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.SuspendLayout();
            // 
            // Color1
            // 
            this.Color1.Location = new System.Drawing.Point(12, 31);
            this.Color1.Name = "Color1";
            this.Color1.Size = new System.Drawing.Size(75, 23);
            this.Color1.TabIndex = 0;
            this.Color1.Text = "Color < ";
            this.Color1.UseVisualStyleBackColor = true;
            this.Color1.Click += new System.EventHandler(this.Color1_Click);
            // 
            // Color2
            // 
            this.Color2.Enabled = false;
            this.Color2.Location = new System.Drawing.Point(12, 60);
            this.Color2.Name = "Color2";
            this.Color2.Size = new System.Drawing.Size(75, 23);
            this.Color2.TabIndex = 1;
            this.Color2.Text = "Color <";
            this.Color2.UseVisualStyleBackColor = true;
            this.Color2.Click += new System.EventHandler(this.Color2_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(93, 34);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(36, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.ThousandsSeparator = true;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Enabled = false;
            this.numericUpDown2.Location = new System.Drawing.Point(93, 63);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(36, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // blinkZero
            // 
            this.blinkZero.AutoSize = true;
            this.blinkZero.Location = new System.Drawing.Point(12, 89);
            this.blinkZero.Name = "blinkZero";
            this.blinkZero.Size = new System.Drawing.Size(87, 17);
            this.blinkZero.TabIndex = 6;
            this.blinkZero.Text = "Blink if 00:00";
            this.blinkZero.UseVisualStyleBackColor = true;
            this.blinkZero.Click += new System.EventHandler(this.blinkZero_Click);
            // 
            // label1
            // 
            this.label1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Minutes : Seconds";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(144, 34);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(36, 20);
            this.numericUpDown3.TabIndex = 9;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Enabled = false;
            this.numericUpDown4.Location = new System.Drawing.Point(144, 63);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(36, 20);
            this.numericUpDown4.TabIndex = 8;
            this.numericUpDown4.ThousandsSeparator = true;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // CDC1Blink
            // 
            this.CDC1Blink.AutoSize = true;
            this.CDC1Blink.Location = new System.Drawing.Point(192, 37);
            this.CDC1Blink.Name = "CDC1Blink";
            this.CDC1Blink.Size = new System.Drawing.Size(49, 17);
            this.CDC1Blink.TabIndex = 10;
            this.CDC1Blink.Text = "Blink";
            this.CDC1Blink.UseVisualStyleBackColor = true;
            this.CDC1Blink.CheckedChanged += new System.EventHandler(this.CDC1Blink_CheckedChanged);
            // 
            // CDC2Blink
            // 
            this.CDC2Blink.AutoSize = true;
            this.CDC2Blink.Enabled = false;
            this.CDC2Blink.Location = new System.Drawing.Point(192, 66);
            this.CDC2Blink.Name = "CDC2Blink";
            this.CDC2Blink.Size = new System.Drawing.Size(49, 17);
            this.CDC2Blink.TabIndex = 11;
            this.CDC2Blink.Text = "Blink";
            this.CDC2Blink.UseVisualStyleBackColor = true;
            this.CDC2Blink.CheckedChanged += new System.EventHandler(this.CDC2Blink_CheckedChanged);
            // 
            // CountOvertime
            // 
            this.CountOvertime.AutoSize = true;
            this.CountOvertime.Location = new System.Drawing.Point(105, 89);
            this.CountOvertime.Name = "CountOvertime";
            this.CountOvertime.Size = new System.Drawing.Size(99, 17);
            this.CountOvertime.TabIndex = 12;
            this.CountOvertime.Text = "Count Overtime";
            this.CountOvertime.UseVisualStyleBackColor = true;
            this.CountOvertime.CheckedChanged += new System.EventHandler(this.CountOvertime_CheckedChanged);
            // 
            // CountDownEffect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 116);
            this.Controls.Add(this.CountOvertime);
            this.Controls.Add(this.CDC2Blink);
            this.Controls.Add(this.CDC1Blink);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.blinkZero);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.Color2);
            this.Controls.Add(this.Color1);
            this.Name = "CountDownEffect";
            this.Text = "CountDownEffects";
            this.Load += new System.EventHandler(this.CountDownEffects_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button Color1;
        private System.Windows.Forms.Button Color2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.CheckBox blinkZero;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.CheckBox CDC1Blink;
        private System.Windows.Forms.CheckBox CDC2Blink;
        private System.Windows.Forms.CheckBox CountOvertime;
    }
}