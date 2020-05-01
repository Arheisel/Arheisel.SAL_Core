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
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
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
            this.groupBox1.Controls.Add(this.slopeLabel);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Location = new System.Drawing.Point(141, 187);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 79);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Slope";
            // 
            // slopeLabel
            // 
            this.slopeLabel.AutoSize = true;
            this.slopeLabel.Location = new System.Drawing.Point(6, 45);
            this.slopeLabel.Name = "slopeLabel";
            this.slopeLabel.Size = new System.Drawing.Size(19, 13);
            this.slopeLabel.TabIndex = 5;
            this.slopeLabel.Text = "20";
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(27, 28);
            this.trackBar1.Maximum = 50;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(248, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Value = 20;
            this.trackBar1.Scroll += new System.EventHandler(this.TrackBar1_Scroll);
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
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VUMeter vuMeterR;
        private VUMeter vuMeterB;
        private VUMeter vuMeterG;
        private AutoscalerControl autoscalerControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label slopeLabel;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}
