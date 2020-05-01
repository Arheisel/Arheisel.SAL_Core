namespace ShineALight
{
    partial class MainWindow
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
            this.Main = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.RemoveArduino = new System.Windows.Forms.Button();
            this.AddArduino = new System.Windows.Forms.Button();
            this.ArduinoList = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ModeSelect = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.Main)).BeginInit();
            this.Main.Panel1.SuspendLayout();
            this.Main.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Main
            // 
            this.Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.Main.Location = new System.Drawing.Point(0, 24);
            this.Main.Name = "Main";
            // 
            // Main.Panel1
            // 
            this.Main.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.Main.Panel1.Controls.Add(this.label2);
            this.Main.Panel1.Controls.Add(this.RemoveArduino);
            this.Main.Panel1.Controls.Add(this.AddArduino);
            this.Main.Panel1.Controls.Add(this.ArduinoList);
            this.Main.Panel1.Controls.Add(this.label1);
            this.Main.Panel1.Controls.Add(this.ModeSelect);
            this.Main.Panel1MinSize = 200;
            this.Main.Size = new System.Drawing.Size(800, 426);
            this.Main.SplitterDistance = 200;
            this.Main.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Arduinos:";
            // 
            // RemoveArduino
            // 
            this.RemoveArduino.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveArduino.Location = new System.Drawing.Point(102, 380);
            this.RemoveArduino.Name = "RemoveArduino";
            this.RemoveArduino.Size = new System.Drawing.Size(75, 23);
            this.RemoveArduino.TabIndex = 4;
            this.RemoveArduino.Text = "Remove";
            this.RemoveArduino.UseVisualStyleBackColor = true;
            // 
            // AddArduino
            // 
            this.AddArduino.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddArduino.Location = new System.Drawing.Point(15, 381);
            this.AddArduino.Name = "AddArduino";
            this.AddArduino.Size = new System.Drawing.Size(75, 23);
            this.AddArduino.TabIndex = 3;
            this.AddArduino.Text = "Add";
            this.AddArduino.UseVisualStyleBackColor = true;
            this.AddArduino.Click += new System.EventHandler(this.AddArduino_Click);
            // 
            // ArduinoList
            // 
            this.ArduinoList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArduinoList.FormattingEnabled = true;
            this.ArduinoList.Location = new System.Drawing.Point(15, 67);
            this.ArduinoList.Name = "ArduinoList";
            this.ArduinoList.Size = new System.Drawing.Size(163, 304);
            this.ArduinoList.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mode:";
            // 
            // ModeSelect
            // 
            this.ModeSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeSelect.FormattingEnabled = true;
            this.ModeSelect.Items.AddRange(new object[] {
            "Effects",
            "Music",
            "RGB Visualizer",
            "Receiver"});
            this.ModeSelect.Location = new System.Drawing.Point(55, 12);
            this.ModeSelect.Name = "ModeSelect";
            this.ModeSelect.Size = new System.Drawing.Size(123, 21);
            this.ModeSelect.TabIndex = 1;
            this.ModeSelect.SelectedIndexChanged += new System.EventHandler(this.ModeSelect_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Main);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainWindow";
            this.Text = "Shine a Light";
            this.Main.Panel1.ResumeLayout(false);
            this.Main.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Main)).EndInit();
            this.Main.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer Main;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ModeSelect;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button RemoveArduino;
        private System.Windows.Forms.Button AddArduino;
        private System.Windows.Forms.CheckedListBox ArduinoList;
        private System.Windows.Forms.Label label2;
    }
}

