namespace ShineALight
{
    partial class UCMusicbar2
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
            this.vuMeter1 = new ShineALight.VUMeter();
            this.autoscalerControl1 = new ShineALight.AutoscalerControl();
            this.curvePlot1 = new ShineALight.CurvePlot();
            this.slopeTrackbar = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.slopeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.slopeTrackbar)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // vuMeter1
            // 
            this.vuMeter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vuMeter1.Location = new System.Drawing.Point(3, 3);
            this.vuMeter1.Name = "vuMeter1";
            this.vuMeter1.Size = new System.Drawing.Size(30, 260);
            this.vuMeter1.TabIndex = 1;
            this.vuMeter1.Value = 0D;
            // 
            // autoscalerControl1
            // 
            this.autoscalerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoscalerControl1.AutoScaler = null;
            this.autoscalerControl1.Location = new System.Drawing.Point(3, 269);
            this.autoscalerControl1.Name = "autoscalerControl1";
            this.autoscalerControl1.Size = new System.Drawing.Size(418, 96);
            this.autoscalerControl1.TabIndex = 2;
            // 
            // curvePlot1
            // 
            this.curvePlot1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.curvePlot1.Function = null;
            this.curvePlot1.Location = new System.Drawing.Point(40, 4);
            this.curvePlot1.Name = "curvePlot1";
            this.curvePlot1.Size = new System.Drawing.Size(381, 173);
            this.curvePlot1.TabIndex = 3;
            // 
            // trackBar1
            // 
            this.slopeTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.slopeTrackbar.Location = new System.Drawing.Point(27, 28);
            this.slopeTrackbar.Maximum = 40;
            this.slopeTrackbar.Name = "trackBar1";
            this.slopeTrackbar.Size = new System.Drawing.Size(348, 45);
            this.slopeTrackbar.TabIndex = 4;
            this.slopeTrackbar.Value = 10;
            this.slopeTrackbar.Scroll += new System.EventHandler(this.slopeTrackbar_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.slopeLabel);
            this.groupBox1.Controls.Add(this.slopeTrackbar);
            this.groupBox1.Location = new System.Drawing.Point(40, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 79);
            this.groupBox1.TabIndex = 5;
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
            this.slopeLabel.Text = "10";
            // 
            // UCMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.curvePlot1);
            this.Controls.Add(this.autoscalerControl1);
            this.Controls.Add(this.vuMeter1);
            this.Name = "UCMusic";
            this.Size = new System.Drawing.Size(425, 371);
            this.Load += new System.EventHandler(this.UCMusic_Load);
            ((System.ComponentModel.ISupportInitialize)(this.slopeTrackbar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private VUMeter vuMeter1;
        private AutoscalerControl autoscalerControl1;
        private CurvePlot curvePlot1;
        private System.Windows.Forms.TrackBar slopeTrackbar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label slopeLabel;
    }
}
