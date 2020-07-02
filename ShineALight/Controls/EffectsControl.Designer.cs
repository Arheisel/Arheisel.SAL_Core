namespace ShineALight.Controls
{
    partial class EffectsControl
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
            this.editBtn = new System.Windows.Forms.Button();
            this.currentSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.speed = new ShineALight.Knob();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.reverse = new ShineALight.RoundedCheckBox();
            this.hold = new ShineALight.Knob();
            this.steps = new ShineALight.Knob();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editBtn.Location = new System.Drawing.Point(268, 3);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(75, 23);
            this.editBtn.TabIndex = 14;
            this.editBtn.Text = "Edit";
            this.editBtn.UseVisualStyleBackColor = true;
            this.editBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // currentSelect
            // 
            this.currentSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentSelect.FormattingEnabled = true;
            this.currentSelect.Location = new System.Drawing.Point(56, 3);
            this.currentSelect.Name = "currentSelect";
            this.currentSelect.Size = new System.Drawing.Size(206, 21);
            this.currentSelect.TabIndex = 13;
            this.currentSelect.SelectedIndexChanged += new System.EventHandler(this.CurrentSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Current:";
            // 
            // speed
            // 
            this.speed.DrawTitle = true;
            this.speed.DrawValue = true;
            this.speed.Location = new System.Drawing.Point(6, 15);
            this.speed.Max = 100;
            this.speed.Min = 1;
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(81, 109);
            this.speed.Step = 5;
            this.speed.TabIndex = 15;
            this.speed.Title = "Speed";
            this.speed.Value = 50;
            this.speed.ValueChanged += new System.EventHandler<System.EventArgs>(this.Speed_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.reverse);
            this.groupBox1.Controls.Add(this.hold);
            this.groupBox1.Controls.Add(this.steps);
            this.groupBox1.Controls.Add(this.speed);
            this.groupBox1.Location = new System.Drawing.Point(9, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 130);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(267, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Reverse";
            // 
            // reverse
            // 
            this.reverse.Checked = false;
            this.reverse.Location = new System.Drawing.Point(279, 35);
            this.reverse.Name = "reverse";
            this.reverse.Size = new System.Drawing.Size(27, 44);
            this.reverse.TabIndex = 18;
            this.reverse.CheckedStateChanged += new System.EventHandler<System.EventArgs>(this.Reverse_CheckedStateChanged);
            // 
            // hold
            // 
            this.hold.DrawTitle = true;
            this.hold.DrawValue = true;
            this.hold.Location = new System.Drawing.Point(180, 15);
            this.hold.Max = 100;
            this.hold.Min = 0;
            this.hold.Name = "hold";
            this.hold.Size = new System.Drawing.Size(81, 109);
            this.hold.Step = 5;
            this.hold.TabIndex = 17;
            this.hold.Title = "Hold Steps";
            this.hold.Value = 10;
            this.hold.ValueChanged += new System.EventHandler<System.EventArgs>(this.Hold_ValueChanged);
            // 
            // steps
            // 
            this.steps.DrawTitle = true;
            this.steps.DrawValue = true;
            this.steps.Location = new System.Drawing.Point(93, 15);
            this.steps.Max = 255;
            this.steps.Min = 2;
            this.steps.Name = "steps";
            this.steps.Size = new System.Drawing.Size(81, 109);
            this.steps.Step = 5;
            this.steps.TabIndex = 16;
            this.steps.Title = "Steps";
            this.steps.Value = 255;
            this.steps.ValueChanged += new System.EventHandler<System.EventArgs>(this.Steps_ValueChanged);
            // 
            // EffectsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.currentSelect);
            this.Controls.Add(this.label1);
            this.Name = "EffectsControl";
            this.Size = new System.Drawing.Size(346, 166);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.ComboBox currentSelect;
        private System.Windows.Forms.Label label1;
        private Knob speed;
        private System.Windows.Forms.GroupBox groupBox1;
        private Knob hold;
        private Knob steps;
        private RoundedCheckBox reverse;
        private System.Windows.Forms.Label label2;
    }
}
