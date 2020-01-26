namespace ShineALight
{
    partial class UCMusic
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
            this.SuspendLayout();
            // 
            // vuMeter1
            // 
            this.vuMeter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vuMeter1.Location = new System.Drawing.Point(3, 3);
            this.vuMeter1.Name = "vuMeter1";
            this.vuMeter1.Size = new System.Drawing.Size(30, 176);
            this.vuMeter1.TabIndex = 1;
            this.vuMeter1.Value = 0D;
            // 
            // autoscalerControl1
            // 
            this.autoscalerControl1.AutoScaler = null;
            this.autoscalerControl1.Location = new System.Drawing.Point(3, 185);
            this.autoscalerControl1.Name = "autoscalerControl1";
            this.autoscalerControl1.Size = new System.Drawing.Size(418, 96);
            this.autoscalerControl1.TabIndex = 2;
            // 
            // UCMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoscalerControl1);
            this.Controls.Add(this.vuMeter1);
            this.Name = "UCMusic";
            this.Size = new System.Drawing.Size(425, 381);
            this.ResumeLayout(false);

        }

        #endregion
        private VUMeter vuMeter1;
        private AutoscalerControl autoscalerControl1;
    }
}
