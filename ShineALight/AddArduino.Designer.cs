using System.Windows.Forms;

namespace ShineALight
{
    partial class AddArduino
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SerialRadio = new System.Windows.Forms.RadioButton();
            this.UDPRadio = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SerialRadio
            // 
            this.SerialRadio.AutoSize = true;
            this.SerialRadio.Checked = true;
            this.SerialRadio.Location = new System.Drawing.Point(13, 13);
            this.SerialRadio.Name = "SerialRadio";
            this.SerialRadio.Size = new System.Drawing.Size(51, 17);
            this.SerialRadio.TabIndex = 0;
            this.SerialRadio.TabStop = true;
            this.SerialRadio.Text = "Serial";
            this.SerialRadio.UseVisualStyleBackColor = true;
            this.SerialRadio.CheckedChanged += new System.EventHandler(this.SerialRadio_CheckedChanged);
            // 
            // UDPRadio
            // 
            this.UDPRadio.AutoSize = true;
            this.UDPRadio.Location = new System.Drawing.Point(102, 13);
            this.UDPRadio.Name = "UDPRadio";
            this.UDPRadio.Size = new System.Drawing.Size(35, 17);
            this.UDPRadio.TabIndex = 1;
            this.UDPRadio.Text = "IP";
            this.UDPRadio.UseVisualStyleBackColor = true;
            this.UDPRadio.CheckedChanged += new System.EventHandler(this.UDPRadio_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 37);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 61);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(57, 104);
            this.OKButton.Name = "OKButton";
            this.OKButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 3;
            this.OKButton.Text = "Add";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // AddArduino
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(222, 134);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.UDPRadio);
            this.Controls.Add(this.SerialRadio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddArduino";
            this.Text = "AddArduino";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton SerialRadio;
        private System.Windows.Forms.RadioButton UDPRadio;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button PopupCancelButton;

    }
}