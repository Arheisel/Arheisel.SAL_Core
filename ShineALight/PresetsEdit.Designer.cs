namespace ShineALight
{
    partial class PresetsEdit
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
            this.presetListBox = new System.Windows.Forms.ListBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.delBtn = new System.Windows.Forms.Button();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.typeSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.colorAddBtn = new System.Windows.Forms.Button();
            this.colorDelBtn = new System.Windows.Forms.Button();
            this.colorUpBtn = new System.Windows.Forms.Button();
            this.colorDownBtn = new System.Windows.Forms.Button();
            this.colorsPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // presetListBox
            // 
            this.presetListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.presetListBox.FormattingEnabled = true;
            this.presetListBox.Location = new System.Drawing.Point(12, 12);
            this.presetListBox.Name = "presetListBox";
            this.presetListBox.Size = new System.Drawing.Size(156, 394);
            this.presetListBox.TabIndex = 0;
            this.presetListBox.SelectedIndexChanged += new System.EventHandler(this.PresetListBox_SelectedIndexChanged);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addBtn.Location = new System.Drawing.Point(12, 415);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 1;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // delBtn
            // 
            this.delBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.delBtn.Location = new System.Drawing.Point(93, 415);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(75, 23);
            this.delBtn.TabIndex = 2;
            this.delBtn.Text = "Remove";
            this.delBtn.UseVisualStyleBackColor = true;
            this.delBtn.Click += new System.EventHandler(this.DelBtn_Click);
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(351, 415);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 9;
            this.okBtn.Text = "Save";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(432, 415);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 10;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Type:";
            // 
            // typeSelect
            // 
            this.typeSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.typeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeSelect.FormattingEnabled = true;
            this.typeSelect.Location = new System.Drawing.Point(231, 41);
            this.typeSelect.Name = "typeSelect";
            this.typeSelect.Size = new System.Drawing.Size(276, 21);
            this.typeSelect.TabIndex = 4;
            this.typeSelect.SelectedValueChanged += new System.EventHandler(this.TypeSelect_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Colors:";
            // 
            // colorAddBtn
            // 
            this.colorAddBtn.Location = new System.Drawing.Point(189, 83);
            this.colorAddBtn.Name = "colorAddBtn";
            this.colorAddBtn.Size = new System.Drawing.Size(75, 23);
            this.colorAddBtn.TabIndex = 5;
            this.colorAddBtn.Text = "Add";
            this.colorAddBtn.UseVisualStyleBackColor = true;
            this.colorAddBtn.Click += new System.EventHandler(this.ColorAddBtn_Click);
            // 
            // colorDelBtn
            // 
            this.colorDelBtn.Location = new System.Drawing.Point(270, 83);
            this.colorDelBtn.Name = "colorDelBtn";
            this.colorDelBtn.Size = new System.Drawing.Size(75, 23);
            this.colorDelBtn.TabIndex = 6;
            this.colorDelBtn.Text = "Remove";
            this.colorDelBtn.UseVisualStyleBackColor = true;
            this.colorDelBtn.Click += new System.EventHandler(this.ColorDelBtn_Click);
            // 
            // colorUpBtn
            // 
            this.colorUpBtn.Location = new System.Drawing.Point(351, 83);
            this.colorUpBtn.Name = "colorUpBtn";
            this.colorUpBtn.Size = new System.Drawing.Size(75, 23);
            this.colorUpBtn.TabIndex = 7;
            this.colorUpBtn.Text = "Up";
            this.colorUpBtn.UseVisualStyleBackColor = true;
            // 
            // colorDownBtn
            // 
            this.colorDownBtn.Location = new System.Drawing.Point(432, 83);
            this.colorDownBtn.Name = "colorDownBtn";
            this.colorDownBtn.Size = new System.Drawing.Size(75, 23);
            this.colorDownBtn.TabIndex = 8;
            this.colorDownBtn.Text = "Down";
            this.colorDownBtn.UseVisualStyleBackColor = true;
            // 
            // colorsPanel
            // 
            this.colorsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.colorsPanel.AutoScroll = true;
            this.colorsPanel.BackColor = System.Drawing.Color.White;
            this.colorsPanel.Location = new System.Drawing.Point(189, 112);
            this.colorsPanel.Name = "colorsPanel";
            this.colorsPanel.Size = new System.Drawing.Size(318, 294);
            this.colorsPanel.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(191, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Name:";
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.Location = new System.Drawing.Point(231, 12);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(276, 20);
            this.nameBox.TabIndex = 3;
            this.nameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
            this.nameBox.Leave += new System.EventHandler(this.NameBox_Leave);
            // 
            // PresetsEdit
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(522, 450);
            this.ControlBox = false;
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.colorsPanel);
            this.Controls.Add(this.colorDownBtn);
            this.Controls.Add(this.colorUpBtn);
            this.Controls.Add(this.colorDelBtn);
            this.Controls.Add(this.colorAddBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.typeSelect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.presetListBox);
            this.MaximizeBox = false;
            this.Name = "PresetsEdit";
            this.Text = "PresetsEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox presetListBox;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button delBtn;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox typeSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button colorAddBtn;
        private System.Windows.Forms.Button colorDelBtn;
        private System.Windows.Forms.Button colorUpBtn;
        private System.Windows.Forms.Button colorDownBtn;
        private System.Windows.Forms.Panel colorsPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}