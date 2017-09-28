namespace FM.TR_TRKPlanningUI.UI
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
            this.lblOldDriver = new System.Windows.Forms.Label();
            this.lblNewDriver = new System.Windows.Forms.Label();
            this.cboNewDriverCode = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtOldDriverCode
            // 
            this.txtOldDriverCode.Enabled = false;
            this.txtOldDriverCode.Location = new System.Drawing.Point(95, 12);
            this.txtOldDriverCode.Name = "txtOldDriverCode";
            this.txtOldDriverCode.Size = new System.Drawing.Size(322, 20);
            this.txtOldDriverCode.TabIndex = 0;
            // 
            // lblOldDriver
            // 
            this.lblOldDriver.Location = new System.Drawing.Point(12, 15);
            this.lblOldDriver.Name = "lblOldDriver";
            this.lblOldDriver.Size = new System.Drawing.Size(77, 13);
            this.lblOldDriver.TabIndex = 1;
            this.lblOldDriver.Text = "Old Driver :";
            this.lblOldDriver.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNewDriver
            // 
            this.lblNewDriver.Location = new System.Drawing.Point(6, 43);
            this.lblNewDriver.Name = "lblNewDriver";
            this.lblNewDriver.Size = new System.Drawing.Size(83, 13);
            this.lblNewDriver.TabIndex = 3;
            this.lblNewDriver.Text = "New Driver :";
            this.lblNewDriver.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboNewDriverCode
            // 
            this.cboNewDriverCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewDriverCode.FormattingEnabled = true;
            this.cboNewDriverCode.Location = new System.Drawing.Point(95, 39);
            this.cboNewDriverCode.Name = "cboNewDriverCode";
            this.cboNewDriverCode.Size = new System.Drawing.Size(322, 21);
            this.cboNewDriverCode.TabIndex = 5;
            this.cboNewDriverCode.TextChanged += new System.EventHandler(this.cboNewDriverCode_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 33);
            this.panel1.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.Location = new System.Drawing.Point(254, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSave.Location = new System.Drawing.Point(95, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmChangeDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(429, 100);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cboNewDriverCode);
            this.Controls.Add(this.lblNewDriver);
            this.Controls.Add(this.lblOldDriver);
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
        private System.Windows.Forms.Label lblOldDriver;
        private System.Windows.Forms.Label lblNewDriver;
        private System.Windows.Forms.ComboBox cboNewDriverCode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}