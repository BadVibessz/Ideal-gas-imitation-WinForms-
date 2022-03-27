namespace animationTry2
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.accelBtn = new System.Windows.Forms.Button();
            this.slowBtn = new System.Windows.Forms.Button();
            this.ballCountNumeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.statBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ballCountNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.Location = new System.Drawing.Point(1, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1103, 518);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // accelBtn
            // 
            this.accelBtn.Location = new System.Drawing.Point(12, 10);
            this.accelBtn.Name = "accelBtn";
            this.accelBtn.Size = new System.Drawing.Size(62, 24);
            this.accelBtn.TabIndex = 1;
            this.accelBtn.Text = "x2";
            this.accelBtn.UseVisualStyleBackColor = true;
            this.accelBtn.Click += new System.EventHandler(this.AccelBtn_Click);
            // 
            // slowBtn
            // 
            this.slowBtn.Location = new System.Drawing.Point(12, 38);
            this.slowBtn.Name = "slowBtn";
            this.slowBtn.Size = new System.Drawing.Size(62, 24);
            this.slowBtn.TabIndex = 2;
            this.slowBtn.Text = "x0.5";
            this.slowBtn.UseVisualStyleBackColor = true;
            this.slowBtn.Click += new System.EventHandler(this.slowBtn_Click);
            // 
            // ballCountNumeric
            // 
            this.ballCountNumeric.BackColor = System.Drawing.Color.White;
            this.ballCountNumeric.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ballCountNumeric.Location = new System.Drawing.Point(177, 40);
            this.ballCountNumeric.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.ballCountNumeric.Name = "ballCountNumeric";
            this.ballCountNumeric.ReadOnly = true;
            this.ballCountNumeric.Size = new System.Drawing.Size(39, 18);
            this.ballCountNumeric.TabIndex = 3;
            this.ballCountNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ballCountNumeric.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(95, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Balls alive:";
            // 
            // statBtn
            // 
            this.statBtn.Location = new System.Drawing.Point(236, 35);
            this.statBtn.Name = "statBtn";
            this.statBtn.Size = new System.Drawing.Size(69, 23);
            this.statBtn.TabIndex = 5;
            this.statBtn.Text = "statistic";
            this.statBtn.UseVisualStyleBackColor = true;
            this.statBtn.Click += new System.EventHandler(this.statBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 586);
            this.Controls.Add(this.statBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ballCountNumeric);
            this.Controls.Add(this.slowBtn);
            this.Controls.Add(this.accelBtn);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.ballCountNumeric)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button statBtn;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.NumericUpDown ballCountNumeric;

        private System.Windows.Forms.Button slowBtn;

        private System.Windows.Forms.Button accelBtn;

        private System.Windows.Forms.Panel panel1;

        #endregion
    }
}