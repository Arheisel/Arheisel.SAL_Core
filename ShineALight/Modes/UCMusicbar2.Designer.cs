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
            this.audioUI1 = new ShineALight.Controls.AudioUI();
            this.SuspendLayout();
            // 
            // audioUI1
            // 
            this.audioUI1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.audioUI1.Audio = null;
            this.audioUI1.Location = new System.Drawing.Point(29, 36);
            this.audioUI1.Name = "audioUI1";
            this.audioUI1.Size = new System.Drawing.Size(448, 400);
            this.audioUI1.TabIndex = 0;
            // 
            // UCMusicbar2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.audioUI1);
            this.Name = "UCMusicbar2";
            this.Size = new System.Drawing.Size(527, 472);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.AudioUI audioUI1;
    }
}
