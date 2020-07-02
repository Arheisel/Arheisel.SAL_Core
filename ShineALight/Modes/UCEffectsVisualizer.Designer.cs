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
            this.audioUI = new ShineALight.Controls.AudioUI();
            this.effectsControl1 = new ShineALight.Controls.EffectsControl();
            this.SuspendLayout();
            // 
            // audioUI
            // 
            this.audioUI.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.audioUI.Audio = null;
            this.audioUI.Location = new System.Drawing.Point(1, 3);
            this.audioUI.Name = "audioUI";
            this.audioUI.Size = new System.Drawing.Size(534, 225);
            this.audioUI.TabIndex = 10;
            // 
            // effectsControl1
            // 
            this.effectsControl1.Effects = null;
            this.effectsControl1.Location = new System.Drawing.Point(100, 231);
            this.effectsControl1.Name = "effectsControl1";
            this.effectsControl1.Size = new System.Drawing.Size(342, 166);
            this.effectsControl1.TabIndex = 11;
            // 
            // UCEffectsVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.effectsControl1);
            this.Controls.Add(this.audioUI);
            this.Name = "UCEffectsVisualizer";
            this.Size = new System.Drawing.Size(534, 400);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.AudioUI audioUI;
        private Controls.EffectsControl effectsControl1;
    }
}
