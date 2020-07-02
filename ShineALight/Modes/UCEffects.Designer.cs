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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.effectsControl = new ShineALight.Controls.EffectsControl();
            this.SuspendLayout();
            // 
            // effectsControl
            // 
            this.effectsControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.effectsControl.Effects = null;
            this.effectsControl.Location = new System.Drawing.Point(3, 3);
            this.effectsControl.Name = "effectsControl";
            this.effectsControl.Size = new System.Drawing.Size(346, 166);
            this.effectsControl.TabIndex = 0;
            // 
            // UCEffects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.effectsControl);
            this.Name = "UCEffects";
            this.Size = new System.Drawing.Size(372, 347);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Controls.EffectsControl effectsControl;
    }
}
