namespace FM.TransportPlanning.UI
{
    partial class FrmChangeDriver
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
            this.txtOldDriverCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOldDriverName = new System.Windows.Forms.TextBox();
            this.txtNewDriverName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboNewDriverCode = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtOldDriverCode
            // 
            this.txtOldDriverCode.Enabled = false;
            this.txtOldDriverCode.Location = new System.Drawing.Point(82, 11);
            this.txtOldDriverCode.Name = "txtOldDriverCode";
            this.txtOldDriverCode.Size = new System.Drawing.Size(196, 20);
            this.txtOldDriverCode.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Old Driver :";
            // 
            // txtOldDriverName
            // 
            this.txtOldDriverName.Enabled = false;
            this.txtOldDriverName.Location = new System.Drawing.Point(331, 11);
            this.txtOldDriverName.Name = "txtOldDriverName";
            this.txtOldDriverName.Size = new System.Drawing.Size(81, 20);
            this.txtOldDriverName.TabIndex = 2;
            // 
            // txtNewDriverName
            // 
            this.txtNewDriverName.Enabled = false;
            this.txtNewDriverName.Location = new System.Drawing.Point(331, 39);
            this.txtNewDriverName.Name = "txtNewDriverName";
            this.txtNewDriverName.Size = new System.Drawing.Size(81, 20);
            this.txtNewDriverName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "New Driver :";
            // 
            // cboNewDriverCode
            // 
            this.cboNewDriverCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewDriverCode.FormattingEnabled = true;
            this.cboNewDriverCode.Location = new System.Drawing.Point(82, 39);
            this.cboNewDriverCode.Name = "cboNewDriverCode";
            this.cboNewDriverCode.Size = new System.Drawing.Size(196, 21);
            this.cboNewDriverCode.TabIndex = 5;
            this.cboNewDriverCode.TextChanged += new System.EventHandler(this.cboNewDriverCode_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 43);
            this.panel1.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.Location = new System.Drawing.Point(254, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSave.Location = new System.Drawing.Point(95, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(286, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Code :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(286, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Code :";
            // 
            // FrmChangeDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(429, 129);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cboNewDriverCode);
            this.Controls.Add(this.txtNewDriverName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOldDriverName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOldDriverCode);
            this.Name = "FrmChangeDriver";
            this.Text = "Change Driver";
            this.Load += new System.EventHandler(this.FrmChooseDriver_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOldDriverCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOldDriverName;
        private System.Windows.Forms.TextBox txtNewDriverName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboNewDriverCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}