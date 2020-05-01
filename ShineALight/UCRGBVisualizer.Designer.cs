namespace ShineALight
{
    partial class UCRGBVisualizer
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
            this.vuMeterR = new ShineALight.VUMeter();
            this.vuMeterB = new ShineALight.VUMeter();
            this.vuMeterG = new ShineALight.VUMeter();
            this.autoscalerControl = new ShineALight.AutoscalerControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.slopeLabel = new System.Windows.Forms.Label();
            this.slopeTrackbar = new System.Windows.Forms.TrackBar();
            this.chLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maxFreqTrackbar = new System.Windows.Forms.TrackBar();
            this.minFreqTrackbar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.maxFreqLabel = new System.Windows.Forms.Label();
            this.minFreqLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slopeTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxFreqTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minFreqTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // vuMeterR
            // 
            this.vuMeterR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vuMeterR.Location = new System.Drawing.Point(3, 3);
            this.vuMeterR.Name = "vuMeterR";
            this.vuMeterR.Size = new System.Drawing.Size(40, 263);
            this.vuMeterR.TabIndex = 0;
            this.vuMeterR.Value = 0D;
            // 
            // vuMeterB
            // 
            this.vuMeterB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vuMeterB.Location = new System.Drawing.Point(49, 3);
            this.vuMeterB.Name = "vuMeterB";
            this.vuMeterB.Size = new System.Drawing.Size(40, 263);
            this.vuMeterB.TabIndex = 1;
            this.vuMeterB.Value = 0D;
            // 
            // vuMeterG
            // 
            this.vuMeterG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vuMeterG.Location = new System.Drawing.Point(95, 3);
            this.vuMeterG.Name = "vuMeterG";
            this.vuMeterG.Size = new System.Drawing.Size(40, 263);
            this.vuMeterG.TabIndex = 2;
            this.vuMeterG.Value = 0D;
            // 
            // autoscalerControl
            // 
            this.autoscalerControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoscalerControl.AutoScaler = null;
            this.autoscalerControl.Location = new System.Drawing.Point(3, 272);
            this.autoscalerControl.Name = "autoscalerControl";
            this.autoscalerControl.Size = new System.Drawing.Size(419, 96);
            this.autoscalerControl.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.minFreqLabel);
            this.groupBox1.Controls.Add(this.maxFreqLabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.minFreqTrackbar);
            this.groupBox1.Controls.Add(this.maxFreqTrackbar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chLabel);
            this.groupBox1.Controls.Add(this.slopeLabel);
            this.groupBox1.Controls.Add(this.slopeTrackbar);
            this.groupBox1.Location = new System.Drawing.Point(141, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 263);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // slopeLabel
            // 
            this.slopeLabel.AutoSize = true;
            this.slopeLabel.Location = new System.Drawing.Point(6, 230);
            this.slopeLabel.Name = "slopeLabel";
            this.slopeLabel.Size = new System.Drawing.Size(19, 13);
            this.slopeLabel.TabIndex = 5;
            this.slopeLabel.Text = "20";
            // 
            // slopeTrackbar
            // 
            this.slopeTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.slopeTrackbar.Location = new System.Drawing.Point(27, 212);
            this.slopeTrackbar.Maximum = 50;
            this.slopeTrackbar.Name = "slopeTrackbar";
            this.slopeTrackbar.Size = new System.Drawing.Size(248, 45);
            this.slopeTrackbar.TabIndex = 4;
            this.slopeTrackbar.Value = 20;
            this.slopeTrackbar.Scroll += new System.EventHandler(this.TrackBar1_Scroll);
            // 
            // chLabel
            // 
            this.chLabel.AutoSize = true;
            this.chLabel.Location = new System.Drawing.Point(84, 29);
            this.chLabel.Name = "chLabel";
            this.chLabel.Size = new System.Drawing.Size(13, 13);
            this.chLabel.TabIndex = 6;
            this.chLabel.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Channels:";
            // 
            // maxFreqTrackbar
            // 
            this.maxFreqTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxFreqTrackbar.Location = new System.Drawing.Point(27, 139);
            this.maxFreqTrackbar.Maximum = 10000;
            this.maxFreqTrackbar.Minimum = 100;
            this.maxFreqTrackbar.Name = "maxFreqTrackbar";
            this.maxFreqTrackbar.Size = new System.Drawing.Size(248, 45);
            this.maxFreqTrackbar.TabIndex = 8;
            this.maxFreqTrackbar.Value = 2050;
            this.maxFreqTrackbar.Scroll += new System.EventHandler(this.TrackBar2_Scroll);
            // 
            // minFreqTrackbar
            // 
            this.minFreqTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.minFreqTrackbar.Location = new System.Drawing.Point(27, 74);
            this.minFreqTrackbar.Maximum = 1000;
            this.minFreqTrackbar.Minimum = 1;
            this.minFreqTrackbar.Name = "minFreqTrackbar";
            this.minFreqTrackbar.Size = new System.Drawing.Size(248, 45);
            this.minFreqTrackbar.TabIndex = 9;
            this.minFreqTrackbar.Value = 50;
            this.minFreqTrackbar.Scroll += new System.EventHandler(this.MinFreqTrackbar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Max Frequency (Hz)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Slope";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Min Frequency (Hz)";
            // 
            // maxFreqLabel
            // 
            this.maxFreqLabel.AutoSize = true;
            this.maxFreqLabel.Location = new System.Drawing.Point(6, 156);
            this.maxFreqLabel.Name = "maxFreqLabel";
            this.maxFreqLabel.Size = new System.Drawing.Size(31, 13);
            this.maxFreqLabel.TabIndex = 13;
            this.maxFreqLabel.Text = "2050";
            // 
            // minFreqLabel
            // 
            this.minFreqLabel.AutoSize = true;
            this.minFreqLabel.Location = new System.Drawing.Point(6, 92);
            this.minFreqLabel.Name = "minFreqLabel";
            this.minFreqLabel.Size = new System.Drawing.Size(19, 13);
            this.minFreqLabel.TabIndex = 14;
            this.minFreqLabel.Text = "50";
            // 
            // UCRGBVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.autoscalerControl);
            this.Controls.Add(this.vuMeterG);
            this.Controls.Add(this.vuMeterB);
            this.Controls.Add(this.vuMeterR);
            this.Name = "UCRGBVisualizer";
            this.Size = new System.Drawing.Size(425, 371);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slopeTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxFreqTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minFreqTrackbar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VUMeter vuMeterR;
        private VUMeter vuMeterB;
        private VUMeter vuMeterG;
        private AutoscalerControl autoscalerControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label slopeLabel;
        private System.Windows.Forms.TrackBar slopeTrackbar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label chLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar minFreqTrackbar;
        private System.Windows.Forms.TrackBar maxFreqTrackbar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label minFreqLabel;
        private System.Windows.Forms.Label maxFreqLabel;
    }
}
