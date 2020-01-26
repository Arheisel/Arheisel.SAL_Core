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
            this.speedSelect = new System.Windows.Forms.TrackBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.speedSelect)).BeginInit();
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
            this.currentSelect.Size = new System.Drawing.Size(277, 21);
            this.currentSelect.TabIndex = 2;
            this.currentSelect.SelectedIndexChanged += new System.EventHandler(this.CurrentSelect_SelectedIndexChanged);
            // 
            // speedSelect
            // 
            this.speedSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.speedSelect.Location = new System.Drawing.Point(73, 52);
            this.speedSelect.Maximum = 100;
            this.speedSelect.Name = "speedSelect";
            this.speedSelect.Size = new System.Drawing.Size(276, 45);
            this.speedSelect.TabIndex = 3;
            this.speedSelect.Scroll += new System.EventHandler(this.SpeedSelect_Scroll);
            // 
            // UCEffects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.speedSelect);
            this.Controls.Add(this.currentSelect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UCEffects";
            this.Size = new System.Drawing.Size(372, 347);
            ((System.ComponentModel.ISupportInitialize)(this.speedSelect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox currentSelect;
        private System.Windows.Forms.TrackBar speedSelect;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
