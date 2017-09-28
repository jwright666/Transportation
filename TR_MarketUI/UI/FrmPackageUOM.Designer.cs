namespace FM.TransportMarket.UI
{
    partial class FrmPackageUOM
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
            this.lblPackageType = new System.Windows.Forms.Label();
            this.txtPackDescription = new System.Windows.Forms.TextBox();
            this.lblPackDescription = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboUOMCode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPackageType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPackageHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPackageWeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPackageLength = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPackageWidth = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPackageType
            // 
            this.lblPackageType.Location = new System.Drawing.Point(12, 27);
            this.lblPackageType.Name = "lblPackageType";
            this.lblPackageType.Size = new System.Drawing.Size(118, 13);
            this.lblPackageType.TabIndex = 0;
            this.lblPackageType.Text = "UOM Code :";
            this.lblPackageType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPackDescription
            // 
            this.txtPackDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPackDescription.Location = new System.Drawing.Point(134, 47);
            this.txtPackDescription.MaxLength = 50;
            this.txtPackDescription.Name = "txtPackDescription";
            this.txtPackDescription.Size = new System.Drawing.Size(230, 20);
            this.txtPackDescription.TabIndex = 3;
            // 
            // lblPackDescription
            // 
            this.lblPackDescription.Location = new System.Drawing.Point(10, 51);
            this.lblPackDescription.Name = "lblPackDescription";
            this.lblPackDescription.Size = new System.Drawing.Size(118, 13);
            this.lblPackDescription.TabIndex = 2;
            this.lblPackDescription.Text = "UOM Description :";
            this.lblPackDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(106, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(202, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboUOMCode);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtPackageType);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtPackageHeight);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtPackageWeight);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtPackageLength);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtPackageWidth);
            this.panel1.Controls.Add(this.lblPackageType);
            this.panel1.Controls.Add(this.lblPackDescription);
            this.panel1.Controls.Add(this.txtPackDescription);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(371, 191);
            this.panel1.TabIndex = 6;
            // 
            // cboUOMCode
            // 
            this.cboUOMCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboUOMCode.FormattingEnabled = true;
            this.cboUOMCode.Location = new System.Drawing.Point(134, 24);
            this.cboUOMCode.Name = "cboUOMCode";
            this.cboUOMCode.Size = new System.Drawing.Size(105, 21);
            this.cboUOMCode.TabIndex = 14;
            this.cboUOMCode.SelectedIndexChanged += new System.EventHandler(this.cboUOMCode_SelectedIndexChanged);
            this.cboUOMCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboUOMCode_KeyPress);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Package Type :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPackageType
            // 
            this.txtPackageType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtPackageType.Location = new System.Drawing.Point(134, 3);
            this.txtPackageType.MaxLength = 10;
            this.txtPackageType.Name = "txtPackageType";
            this.txtPackageType.ReadOnly = true;
            this.txtPackageType.Size = new System.Drawing.Size(105, 20);
            this.txtPackageType.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Height :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPackageHeight
            // 
            this.txtPackageHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtPackageHeight.Location = new System.Drawing.Point(134, 109);
            this.txtPackageHeight.MaxLength = 16;
            this.txtPackageHeight.Name = "txtPackageHeight";
            this.txtPackageHeight.Size = new System.Drawing.Size(105, 20);
            this.txtPackageHeight.TabIndex = 9;
            this.txtPackageHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Measurement_KeyPress);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Unit Weight :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPackageWeight
            // 
            this.txtPackageWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtPackageWeight.Location = new System.Drawing.Point(134, 130);
            this.txtPackageWeight.MaxLength = 16;
            this.txtPackageWeight.Name = "txtPackageWeight";
            this.txtPackageWeight.Size = new System.Drawing.Size(105, 20);
            this.txtPackageWeight.TabIndex = 11;
            this.txtPackageWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Measurement_KeyPress);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Length :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPackageLength
            // 
            this.txtPackageLength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtPackageLength.Location = new System.Drawing.Point(134, 68);
            this.txtPackageLength.MaxLength = 16;
            this.txtPackageLength.Name = "txtPackageLength";
            this.txtPackageLength.Size = new System.Drawing.Size(105, 20);
            this.txtPackageLength.TabIndex = 5;
            this.txtPackageLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Measurement_KeyPress);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Width :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPackageWidth
            // 
            this.txtPackageWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtPackageWidth.Location = new System.Drawing.Point(134, 88);
            this.txtPackageWidth.MaxLength = 16;
            this.txtPackageWidth.Name = "txtPackageWidth";
            this.txtPackageWidth.Size = new System.Drawing.Size(105, 20);
            this.txtPackageWidth.TabIndex = 7;
            this.txtPackageWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Measurement_KeyPress);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 156);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(371, 35);
            this.panel3.TabIndex = 8;
            // 
            // FrmPackageUOM
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(371, 191);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "FrmPackageUOM";
            this.Text = "Package Type";
            this.Shown += new System.EventHandler(this.FrmPackageType_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPackageType;
        private System.Windows.Forms.TextBox txtPackDescription;
        private System.Windows.Forms.Label lblPackDescription;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPackageType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPackageHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPackageWeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPackageLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPackageWidth;
        private System.Windows.Forms.ComboBox cboUOMCode;
    }
}