namespace ShineALight.Controls
{
    partial class AudioControls
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.minFreq = new ShineALight.Knob();
            this.slope = new ShineALight.Knob();
            this.maxFreq = new ShineALight.Knob();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.minFreq);
            this.groupBox1.Controls.Add(this.slope);
            this.groupBox1.Controls.Add(this.maxFreq);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 130);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // minFreq
            // 
            this.minFreq.DrawTitle = true;
            this.minFreq.DrawValue = true;
            this.minFreq.Location = new System.Drawing.Point(6, 14);
            this.minFreq.Max = 1000;
            this.minFreq.Min = 20;
            this.minFreq.Name = "minFreq";
            this.minFreq.Size = new System.Drawing.Size(81, 109);
            this.minFreq.Step = 10;
            this.minFreq.TabIndex = 0;
            this.minFreq.Title = "Min Freq (Hz)";
            this.minFreq.Value = 50;
            this.minFreq.ValueChanged += new System.EventHandler<System.EventArgs>(this.MinFreq_ValueChanged);
            // 
            // slope
            // 
            this.slope.DrawTitle = true;
            this.slope.DrawValue = true;
            this.slope.Location = new System.Drawing.Point(180, 14);
            this.slope.Max = 50;
            this.slope.Min = 1;
            this.slope.Name = "slope";
            this.slope.Size = new System.Drawing.Size(81, 109);
            this.slope.Step = 1;
            this.slope.TabIndex = 1;
            this.slope.Title = "Slope";
            this.slope.Value = 10;
            this.slope.ValueChanged += new System.EventHandler<System.EventArgs>(this.Slope_ValueChanged);
            // 
            // maxFreq
            // 
            this.maxFreq.DrawTitle = true;
            this.maxFreq.DrawValue = true;
            this.maxFreq.Location = new System.Drawing.Point(93, 14);
            this.maxFreq.Max = 10000;
            this.maxFreq.Min = 100;
            this.maxFreq.Name = "maxFreq";
            this.maxFreq.Size = new System.Drawing.Size(81, 109);
            this.maxFreq.Step = 50;
            this.maxFreq.TabIndex = 2;
            this.maxFreq.Title = "Max Freq (Hz)";
            this.maxFreq.Value = 4100;
            this.maxFreq.ValueChanged += new System.EventHandler<System.EventArgs>(this.MaxFreq_ValueChanged);
            // 
            // AudioControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "AudioControls";
            this.Size = new System.Drawing.Size(274, 130);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Knob minFreq;
        private Knob slope;
        private Knob maxFreq;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
