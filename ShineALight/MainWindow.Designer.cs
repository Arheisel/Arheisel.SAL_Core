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
            this.arduinoList = new System.Windows.Forms.ListBox();
            this.Reverse = new System.Windows.Forms.Button();
            this.MoveDown = new System.Windows.Forms.Button();
            this.MoveUp = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.RemoveArduino = new System.Windows.Forms.Button();
            this.AddArduino = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ModeSelect = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.Main.Panel1.Controls.Add(this.arduinoList);
            this.Main.Panel1.Controls.Add(this.Reverse);
            this.Main.Panel1.Controls.Add(this.MoveDown);
            this.Main.Panel1.Controls.Add(this.MoveUp);
            this.Main.Panel1.Controls.Add(this.progressBar1);
            this.Main.Panel1.Controls.Add(this.label2);
            this.Main.Panel1.Controls.Add(this.RemoveArduino);
            this.Main.Panel1.Controls.Add(this.AddArduino);
            this.Main.Panel1.Controls.Add(this.label1);
            this.Main.Panel1.Controls.Add(this.ModeSelect);
            this.Main.Panel1MinSize = 200;
            this.Main.Size = new System.Drawing.Size(800, 426);
            this.Main.SplitterDistance = 200;
            this.Main.TabIndex = 0;
            // 
            // arduinoList
            // 
            this.arduinoList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.arduinoList.FormattingEnabled = true;
            this.arduinoList.Location = new System.Drawing.Point(15, 82);
            this.arduinoList.Name = "arduinoList";
            this.arduinoList.Size = new System.Drawing.Size(163, 264);
            this.arduinoList.TabIndex = 10;
            // 
            // Reverse
            // 
            this.Reverse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Reverse.Location = new System.Drawing.Point(102, 352);
            this.Reverse.Name = "Reverse";
            this.Reverse.Size = new System.Drawing.Size(75, 23);
            this.Reverse.TabIndex = 9;
            this.Reverse.Text = "Reverse";
            this.Reverse.UseVisualStyleBackColor = true;
            this.Reverse.Click += new System.EventHandler(this.Reverse_Click);
            // 
            // MoveDown
            // 
            this.MoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MoveDown.Location = new System.Drawing.Point(55, 352);
            this.MoveDown.Name = "MoveDown";
            this.MoveDown.Size = new System.Drawing.Size(35, 23);
            this.MoveDown.TabIndex = 8;
            this.MoveDown.Text = "▼";
            this.MoveDown.UseVisualStyleBackColor = true;
            this.MoveDown.Click += new System.EventHandler(this.MoveDown_Click);
            // 
            // MoveUp
            // 
            this.MoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MoveUp.Location = new System.Drawing.Point(15, 352);
            this.MoveUp.Name = "MoveUp";
            this.MoveUp.Size = new System.Drawing.Size(34, 23);
            this.MoveUp.TabIndex = 7;
            this.MoveUp.Text = "▲";
            this.MoveUp.UseVisualStyleBackColor = true;
            this.MoveUp.Click += new System.EventHandler(this.MoveUp_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 66);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(163, 10);
            this.progressBar1.TabIndex = 6;
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
            this.RemoveArduino.Click += new System.EventHandler(this.RemoveArduino_Click);
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
            "Visualizer",
            "Musicbar",
            "Musicbar 2",
            "Musicbar 3",
            "Effect Visualizer"});
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
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Shine a Light";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button Reverse;
        private System.Windows.Forms.Button MoveDown;
        private System.Windows.Forms.Button MoveUp;
        private System.Windows.Forms.ListBox arduinoList;
    }
}

