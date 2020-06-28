namespace ShineALight
{
    partial class Knob
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
            this.SuspendLayout();
            // 
            // Knob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Knob";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Knob_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Knob_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Knob_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Knob_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
