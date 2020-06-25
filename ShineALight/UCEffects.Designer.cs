namespace ShineALight
{
    partial class UCEffects
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.currentSelect = new System.Windows.Forms.ComboBox();
            this.speedTrackbar = new System.Windows.Forms.TrackBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.speedLabel = new System.Windows.Forms.Label();
            this.stepsLabel = new System.Windows.Forms.Label();
            this.stepsTrackbar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.editBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.holdingKnob = new ShineALight.Knob();
            this.revCheckBox = new ShineALight.RoundedCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepsTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Speed:";
            // 
            // currentSelect
            // 
            this.currentSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentSelect.FormattingEnabled = true;
            this.currentSelect.Location = new System.Drawing.Point(72, 16);
            this.currentSelect.Name = "currentSelect";
            this.currentSelect.Size = new System.Drawing.Size(196, 21);
            this.currentSelect.TabIndex = 2;
            this.currentSelect.SelectedIndexChanged += new System.EventHandler(this.CurrentSelect_SelectedIndexChanged);
            // 
            // speedTrackbar
            // 
            this.speedTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.speedTrackbar.Location = new System.Drawing.Point(73, 52);
            this.speedTrackbar.Maximum = 100;
            this.speedTrackbar.Minimum = 1;
            this.speedTrackbar.Name = "speedTrackbar";
            this.speedTrackbar.Size = new System.Drawing.Size(276, 45);
            this.speedTrackbar.TabIndex = 3;
            this.speedTrackbar.Value = 1;
            this.speedTrackbar.Scroll += new System.EventHandler(this.SpeedTrackbar_Scroll);
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Location = new System.Drawing.Point(50, 75);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(13, 13);
            this.speedLabel.TabIndex = 4;
            this.speedLabel.Text = "0";
            // 
            // stepsLabel
            // 
            this.stepsLabel.AutoSize = true;
            this.stepsLabel.Location = new System.Drawing.Point(49, 126);
            this.stepsLabel.Name = "stepsLabel";
            this.stepsLabel.Size = new System.Drawing.Size(25, 13);
            this.stepsLabel.TabIndex = 7;
            this.stepsLabel.Text = "255";
            // 
            // stepsTrackbar
            // 
            this.stepsTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stepsTrackbar.Location = new System.Drawing.Point(72, 103);
            this.stepsTrackbar.Maximum = 255;
            this.stepsTrackbar.Minimum = 1;
            this.stepsTrackbar.Name = "stepsTrackbar";
            this.stepsTrackbar.Size = new System.Drawing.Size(276, 45);
            this.stepsTrackbar.TabIndex = 6;
            this.stepsTrackbar.Value = 255;
            this.stepsTrackbar.Scroll += new System.EventHandler(this.StepsTrackbar_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Steps:";
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editBtn.Location = new System.Drawing.Point(274, 16);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(75, 23);
            this.editBtn.TabIndex = 11;
            this.editBtn.Text = "Edit";
            this.editBtn.UseVisualStyleBackColor = true;
            this.editBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Reverse:";
            // 
            // holdingKnob
            // 
            this.holdingKnob.BackColor = System.Drawing.SystemColors.Control;
            this.holdingKnob.DrawTitle = true;
            this.holdingKnob.DrawValue = true;
            this.holdingKnob.ForeColor = System.Drawing.SystemColors.ControlText;
            this.holdingKnob.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.holdingKnob.Location = new System.Drawing.Point(198, 152);
            this.holdingKnob.Max = 100;
            this.holdingKnob.Min = 0;
            this.holdingKnob.Name = "holdingKnob";
            this.holdingKnob.Size = new System.Drawing.Size(101, 124);
            this.holdingKnob.Step = 1;
            this.holdingKnob.TabIndex = 14;
            this.holdingKnob.Title = "Hold Steps";
            this.holdingKnob.Value = 0;
            this.holdingKnob.ValueChanged += new System.EventHandler<System.EventArgs>(this.HoldingKnob_ValueChanged);
            // 
            // revCheckBox
            // 
            this.revCheckBox.Checked = false;
            this.revCheckBox.Location = new System.Drawing.Point(77, 193);
            this.revCheckBox.Name = "revCheckBox";
            this.revCheckBox.Size = new System.Drawing.Size(27, 44);
            this.revCheckBox.TabIndex = 15;
            this.revCheckBox.CheckedStateChanged += new System.EventHandler<System.EventArgs>(this.RevCheckBox_CheckedStateChanged);
            // 
            // UCEffects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.revCheckBox);
            this.Controls.Add(this.holdingKnob);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.stepsLabel);
            this.Controls.Add(this.stepsTrackbar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.speedTrackbar);
            this.Controls.Add(this.currentSelect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UCEffects";
            this.Size = new System.Drawing.Size(372, 347);
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepsTrackbar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox currentSelect;
        private System.Windows.Forms.TrackBar speedTrackbar;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.Label stepsLabel;
        private System.Windows.Forms.TrackBar stepsTrackbar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.Label label3;
        private Knob holdingKnob;
        private RoundedCheckBox revCheckBox;
    }
}
