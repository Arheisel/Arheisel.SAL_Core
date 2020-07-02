namespace ShineALight
{
    partial class AutoscalerControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.amp = new ShineALight.Knob();
            this.enabled = new ShineALight.RoundedCheckBox();
            this.scale = new ShineALight.Knob();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.amp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.enabled);
            this.groupBox1.Controls.Add(this.scale);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 130);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Autoscaler";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enabled";
            // 
            // amp
            // 
            this.amp.DrawTitle = true;
            this.amp.DrawValue = true;
            this.amp.Location = new System.Drawing.Point(162, 14);
            this.amp.Max = 10;
            this.amp.Min = 1;
            this.amp.Name = "amp";
            this.amp.Size = new System.Drawing.Size(81, 109);
            this.amp.Step = 1;
            this.amp.TabIndex = 5;
            this.amp.Title = "Amp";
            this.amp.Value = 1;
            this.amp.ValueChanged += new System.EventHandler<System.EventArgs>(this.Amp_ValueChanged);
            // 
            // enabled
            // 
            this.enabled.Checked = false;
            this.enabled.Location = new System.Drawing.Point(31, 35);
            this.enabled.Name = "enabled";
            this.enabled.Size = new System.Drawing.Size(25, 48);
            this.enabled.TabIndex = 3;
            this.enabled.CheckedStateChanged += new System.EventHandler<System.EventArgs>(this.RoundedCheckBox1_CheckedStateChanged);
            // 
            // scale
            // 
            this.scale.DrawTitle = true;
            this.scale.DrawValue = true;
            this.scale.Location = new System.Drawing.Point(75, 14);
            this.scale.Max = 1000;
            this.scale.Min = 0;
            this.scale.Name = "scale";
            this.scale.Size = new System.Drawing.Size(81, 109);
            this.scale.Step = 10;
            this.scale.TabIndex = 2;
            this.scale.Title = "Scale";
            this.scale.Value = 10;
            this.scale.ValueChanged += new System.EventHandler<System.EventArgs>(this.Scale_ValueChanged);
            // 
            // AutoscalerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "AutoscalerControl";
            this.Size = new System.Drawing.Size(244, 130);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Knob scale;
        private System.Windows.Forms.Label label1;
        private RoundedCheckBox enabled;
        private Knob amp;
    }
}
