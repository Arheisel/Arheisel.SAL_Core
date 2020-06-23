namespace ShineALight
{
    partial class UCEffectsVisualizer
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
            this.autoscalerControl = new ShineALight.AutoscalerControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.minFreqLabel = new System.Windows.Forms.Label();
            this.maxFreqLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.minFreqTrackbar = new System.Windows.Forms.TrackBar();
            this.maxFreqTrackbar = new System.Windows.Forms.TrackBar();
            this.slopeLabel = new System.Windows.Forms.Label();
            this.slopeTrackbar = new System.Windows.Forms.TrackBar();
            this.vuPanel = new System.Windows.Forms.Panel();
            this.currentSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minFreqTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxFreqTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slopeTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // autoscalerControl
            // 
            this.autoscalerControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoscalerControl.AutoScaler = null;
            this.autoscalerControl.Location = new System.Drawing.Point(187, 204);
            this.autoscalerControl.Name = "autoscalerControl";
            this.autoscalerControl.Size = new System.Drawing.Size(235, 95);
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
            this.groupBox1.Controls.Add(this.slopeLabel);
            this.groupBox1.Controls.Add(this.slopeTrackbar);
            this.groupBox1.Location = new System.Drawing.Point(187, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 195);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // minFreqLabel
            // 
            this.minFreqLabel.AutoSize = true;
            this.minFreqLabel.Location = new System.Drawing.Point(6, 56);
            this.minFreqLabel.Name = "minFreqLabel";
            this.minFreqLabel.Size = new System.Drawing.Size(19, 13);
            this.minFreqLabel.TabIndex = 14;
            this.minFreqLabel.Text = "50";
            // 
            // maxFreqLabel
            // 
            this.maxFreqLabel.AutoSize = true;
            this.maxFreqLabel.Location = new System.Drawing.Point(6, 106);
            this.maxFreqLabel.Name = "maxFreqLabel";
            this.maxFreqLabel.Size = new System.Drawing.Size(31, 13);
            this.maxFreqLabel.TabIndex = 13;
            this.maxFreqLabel.Text = "4100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Min Frequency (Hz)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Slope";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Max Frequency (Hz)";
            // 
            // minFreqTrackbar
            // 
            this.minFreqTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.minFreqTrackbar.LargeChange = 10;
            this.minFreqTrackbar.Location = new System.Drawing.Point(27, 38);
            this.minFreqTrackbar.Maximum = 1000;
            this.minFreqTrackbar.Minimum = 1;
            this.minFreqTrackbar.Name = "minFreqTrackbar";
            this.minFreqTrackbar.Size = new System.Drawing.Size(202, 45);
            this.minFreqTrackbar.SmallChange = 5;
            this.minFreqTrackbar.TabIndex = 9;
            this.minFreqTrackbar.Value = 50;
            this.minFreqTrackbar.Scroll += new System.EventHandler(this.MinFreqTrackbar_Scroll);
            // 
            // maxFreqTrackbar
            // 
            this.maxFreqTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.maxFreqTrackbar.LargeChange = 100;
            this.maxFreqTrackbar.Location = new System.Drawing.Point(27, 89);
            this.maxFreqTrackbar.Maximum = 10000;
            this.maxFreqTrackbar.Minimum = 100;
            this.maxFreqTrackbar.Name = "maxFreqTrackbar";
            this.maxFreqTrackbar.Size = new System.Drawing.Size(202, 45);
            this.maxFreqTrackbar.SmallChange = 50;
            this.maxFreqTrackbar.TabIndex = 8;
            this.maxFreqTrackbar.Value = 4100;
            this.maxFreqTrackbar.Scroll += new System.EventHandler(this.TrackBar2_Scroll);
            // 
            // slopeLabel
            // 
            this.slopeLabel.AutoSize = true;
            this.slopeLabel.Location = new System.Drawing.Point(6, 158);
            this.slopeLabel.Name = "slopeLabel";
            this.slopeLabel.Size = new System.Drawing.Size(19, 13);
            this.slopeLabel.TabIndex = 5;
            this.slopeLabel.Text = "10";
            // 
            // slopeTrackbar
            // 
            this.slopeTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.slopeTrackbar.Location = new System.Drawing.Point(27, 140);
            this.slopeTrackbar.Maximum = 50;
            this.slopeTrackbar.Name = "slopeTrackbar";
            this.slopeTrackbar.Size = new System.Drawing.Size(202, 45);
            this.slopeTrackbar.TabIndex = 4;
            this.slopeTrackbar.Value = 10;
            this.slopeTrackbar.Scroll += new System.EventHandler(this.slopeTrackbar_Scroll);
            // 
            // vuPanel
            // 
            this.vuPanel.BackColor = System.Drawing.Color.White;
            this.vuPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vuPanel.Location = new System.Drawing.Point(3, 3);
            this.vuPanel.Name = "vuPanel";
            this.vuPanel.Size = new System.Drawing.Size(178, 296);
            this.vuPanel.TabIndex = 7;
            // 
            // currentSelect
            // 
            this.currentSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentSelect.FormattingEnabled = true;
            this.currentSelect.Location = new System.Drawing.Point(55, 305);
            this.currentSelect.Name = "currentSelect";
            this.currentSelect.Size = new System.Drawing.Size(237, 21);
            this.currentSelect.TabIndex = 9;
            this.currentSelect.SelectedIndexChanged += new System.EventHandler(this.CurrentSelect_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Current:";
            // 
            // UCEffectsVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.currentSelect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.vuPanel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.autoscalerControl);
            this.Name = "UCEffectsVisualizer";
            this.Size = new System.Drawing.Size(425, 371);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minFreqTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxFreqTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slopeTrackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private AutoscalerControl autoscalerControl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label slopeLabel;
        private System.Windows.Forms.TrackBar slopeTrackbar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar minFreqTrackbar;
        private System.Windows.Forms.TrackBar maxFreqTrackbar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label minFreqLabel;
        private System.Windows.Forms.Label maxFreqLabel;
        private System.Windows.Forms.Panel vuPanel;
        private System.Windows.Forms.ComboBox currentSelect;
        private System.Windows.Forms.Label label2;
    }
}
