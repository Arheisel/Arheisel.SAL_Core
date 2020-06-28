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
            this.currentSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.audioUI = new ShineALight.Controls.AudioUI();
            this.SuspendLayout();
            // 
            // currentSelect
            // 
            this.currentSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentSelect.FormattingEnabled = true;
            this.currentSelect.Location = new System.Drawing.Point(115, 342);
            this.currentSelect.Name = "currentSelect";
            this.currentSelect.Size = new System.Drawing.Size(266, 21);
            this.currentSelect.TabIndex = 9;
            this.currentSelect.SelectedIndexChanged += new System.EventHandler(this.CurrentSelect_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 345);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Current:";
            // 
            // audioUI
            // 
            this.audioUI.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.audioUI.Audio = null;
            this.audioUI.Location = new System.Drawing.Point(0, 0);
            this.audioUI.Name = "audioUI";
            this.audioUI.Size = new System.Drawing.Size(451, 336);
            this.audioUI.TabIndex = 10;
            // 
            // UCEffectsVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.audioUI);
            this.Controls.Add(this.currentSelect);
            this.Controls.Add(this.label2);
            this.Name = "UCEffectsVisualizer";
            this.Size = new System.Drawing.Size(454, 384);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox currentSelect;
        private System.Windows.Forms.Label label2;
        private Controls.AudioUI audioUI;
    }
}
