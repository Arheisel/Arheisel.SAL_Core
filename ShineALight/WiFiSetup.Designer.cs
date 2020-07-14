namespace ShineALight
{
    partial class WiFiSetup
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
            this.label1 = new System.Windows.Forms.Label();
            this.macTB = new System.Windows.Forms.TextBox();
            this.ssidTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.passwdTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ipTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gwTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.maskTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.setBtn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.scanBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.devipTB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "MAC:";
            // 
            // macTB
            // 
            this.macTB.Location = new System.Drawing.Point(65, 15);
            this.macTB.Name = "macTB";
            this.macTB.ReadOnly = true;
            this.macTB.Size = new System.Drawing.Size(302, 20);
            this.macTB.TabIndex = 1;
            // 
            // ssidTB
            // 
            this.ssidTB.Location = new System.Drawing.Point(464, 15);
            this.ssidTB.Name = "ssidTB";
            this.ssidTB.Size = new System.Drawing.Size(302, 20);
            this.ssidTB.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(425, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "SSID:";
            // 
            // passwdTB
            // 
            this.passwdTB.Location = new System.Drawing.Point(464, 41);
            this.passwdTB.Name = "passwdTB";
            this.passwdTB.Size = new System.Drawing.Size(302, 20);
            this.passwdTB.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(404, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // ipTB
            // 
            this.ipTB.Location = new System.Drawing.Point(464, 67);
            this.ipTB.Name = "ipTB";
            this.ipTB.Size = new System.Drawing.Size(302, 20);
            this.ipTB.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(438, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "IP:";
            // 
            // gwTB
            // 
            this.gwTB.Location = new System.Drawing.Point(464, 93);
            this.gwTB.Name = "gwTB";
            this.gwTB.Size = new System.Drawing.Size(302, 20);
            this.gwTB.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(411, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Gateway";
            // 
            // maskTB
            // 
            this.maskTB.Location = new System.Drawing.Point(464, 119);
            this.maskTB.Name = "maskTB";
            this.maskTB.Size = new System.Drawing.Size(302, 20);
            this.maskTB.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(425, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Mask:";
            // 
            // setBtn
            // 
            this.setBtn.Location = new System.Drawing.Point(691, 145);
            this.setBtn.Name = "setBtn";
            this.setBtn.Size = new System.Drawing.Size(75, 23);
            this.setBtn.TabIndex = 12;
            this.setBtn.Text = "Set";
            this.setBtn.UseVisualStyleBackColor = true;
            this.setBtn.Click += new System.EventHandler(this.SetBtn_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(29, 44);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(338, 329);
            this.listBox1.TabIndex = 13;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.ListBox1_SelectedValueChanged);
            // 
            // scanBtn
            // 
            this.scanBtn.Location = new System.Drawing.Point(292, 379);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(75, 23);
            this.scanBtn.TabIndex = 14;
            this.scanBtn.Text = "Scan";
            this.scanBtn.UseVisualStyleBackColor = true;
            this.scanBtn.Click += new System.EventHandler(this.ScanBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(713, 415);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 15;
            this.closeBtn.Text = "Close";
            this.closeBtn.UseVisualStyleBackColor = true;
            // 
            // devipTB
            // 
            this.devipTB.Location = new System.Drawing.Point(464, 211);
            this.devipTB.Name = "devipTB";
            this.devipTB.ReadOnly = true;
            this.devipTB.Size = new System.Drawing.Size(302, 20);
            this.devipTB.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(401, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Device IP:";
            // 
            // refreshBtn
            // 
            this.refreshBtn.Location = new System.Drawing.Point(691, 237);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(75, 23);
            this.refreshBtn.TabIndex = 18;
            this.refreshBtn.Text = "Refresh";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // WiFiSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.devipTB);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.scanBtn);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.setBtn);
            this.Controls.Add(this.maskTB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.gwTB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ipTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.passwdTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ssidTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.macTB);
            this.Controls.Add(this.label1);
            this.Name = "WiFiSetup";
            this.Text = "WiFiSetup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox macTB;
        private System.Windows.Forms.TextBox ssidTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox passwdTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ipTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox gwTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox maskTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button setBtn;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button scanBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.TextBox devipTB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button refreshBtn;
    }
}