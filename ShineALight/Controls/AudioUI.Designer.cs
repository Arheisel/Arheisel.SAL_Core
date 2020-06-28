namespace ShineALight.Controls
{
    partial class AudioUI
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
            this.audioControls = new ShineALight.Controls.AudioControls();
            this.vuPanel = new ShineALight.Controls.VUPanel();
            this.SuspendLayout();
            // 
            // autoscalerControl
            // 
            this.autoscalerControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoscalerControl.AutoScaler = null;
            this.autoscalerControl.Location = new System.Drawing.Point(283, 267);
            this.autoscalerControl.Name = "autoscalerControl";
            this.autoscalerControl.Size = new System.Drawing.Size(161, 130);
            this.autoscalerControl.TabIndex = 2;
            // 
            // audioControls
            // 
            this.audioControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.audioControls.Audio = null;
            this.audioControls.Location = new System.Drawing.Point(3, 267);
            this.audioControls.Name = "audioControls";
            this.audioControls.Size = new System.Drawing.Size(274, 130);
            this.audioControls.TabIndex = 1;
            // 
            // vuPanel
            // 
            this.vuPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vuPanel.Channels = 0;
            this.vuPanel.Location = new System.Drawing.Point(3, 3);
            this.vuPanel.Name = "vuPanel";
            this.vuPanel.Size = new System.Drawing.Size(441, 258);
            this.vuPanel.TabIndex = 0;
            // 
            // AudioUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoscalerControl);
            this.Controls.Add(this.audioControls);
            this.Controls.Add(this.vuPanel);
            this.Name = "AudioUI";
            this.Size = new System.Drawing.Size(448, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private VUPanel vuPanel;
        private AudioControls audioControls;
        private AutoscalerControl autoscalerControl;
    }
}
