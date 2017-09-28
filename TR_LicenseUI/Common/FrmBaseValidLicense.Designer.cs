namespace FMLicenseUI.Common
{
    partial class FrmBaseValidLicense
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
        protected void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxLicenseInfo = new System.Windows.Forms.GroupBox();
            this.btnCancelLicense = new System.Windows.Forms.Button();
            this.btnSaveLicense = new System.Windows.Forms.Button();
            this.dtpValidFrom = new System.Windows.Forms.DateTimePicker();
            this.txtNoOfLicense = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtOldExpiryDate = new System.Windows.Forms.TextBox();
            this.txtOldEffectiveDate = new System.Windows.Forms.TextBox();
            this.txtOldNoOfLicense = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbxLicenseInfo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCompanyName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 37);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtCompanyName.Location = new System.Drawing.Point(102, 12);
            this.txtCompanyName.MaxLength = 50;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.ReadOnly = true;
            this.txtCompanyName.Size = new System.Drawing.Size(249, 20);
            this.txtCompanyName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Company Name :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxLicenseInfo
            // 
            this.gbxLicenseInfo.Controls.Add(this.btnCancelLicense);
            this.gbxLicenseInfo.Controls.Add(this.btnSaveLicense);
            this.gbxLicenseInfo.Controls.Add(this.dtpValidFrom);
            this.gbxLicenseInfo.Controls.Add(this.txtNoOfLicense);
            this.gbxLicenseInfo.Controls.Add(this.label12);
            this.gbxLicenseInfo.Controls.Add(this.label14);
            this.gbxLicenseInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbxLicenseInfo.Location = new System.Drawing.Point(0, 102);
            this.gbxLicenseInfo.Name = "gbxLicenseInfo";
            this.gbxLicenseInfo.Size = new System.Drawing.Size(365, 93);
            this.gbxLicenseInfo.TabIndex = 16;
            this.gbxLicenseInfo.TabStop = false;
            this.gbxLicenseInfo.Text = "New License";
            // 
            // btnCancelLicense
            // 
            this.btnCancelLicense.Location = new System.Drawing.Point(145, 65);
            this.btnCancelLicense.Name = "btnCancelLicense";
            this.btnCancelLicense.Size = new System.Drawing.Size(75, 23);
            this.btnCancelLicense.TabIndex = 13;
            this.btnCancelLicense.Text = "Cancel";
            this.btnCancelLicense.UseVisualStyleBackColor = true;
            // 
            // btnSaveLicense
            // 
            this.btnSaveLicense.Location = new System.Drawing.Point(65, 65);
            this.btnSaveLicense.Name = "btnSaveLicense";
            this.btnSaveLicense.Size = new System.Drawing.Size(75, 23);
            this.btnSaveLicense.TabIndex = 4;
            this.btnSaveLicense.Text = "Save";
            this.btnSaveLicense.UseVisualStyleBackColor = true;
            // 
            // dtpValidFrom
            // 
            this.dtpValidFrom.CalendarMonthBackground = System.Drawing.Color.White;
            this.dtpValidFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpValidFrom.Location = new System.Drawing.Point(103, 41);
            this.dtpValidFrom.Name = "dtpValidFrom";
            this.dtpValidFrom.Size = new System.Drawing.Size(121, 20);
            this.dtpValidFrom.TabIndex = 11;
            // 
            // txtNoOfLicense
            // 
            this.txtNoOfLicense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtNoOfLicense.Location = new System.Drawing.Point(102, 19);
            this.txtNoOfLicense.MaxLength = 5;
            this.txtNoOfLicense.Name = "txtNoOfLicense";
            this.txtNoOfLicense.Size = new System.Drawing.Size(74, 20);
            this.txtNoOfLicense.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(8, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(84, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "No Of License :";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 44);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(84, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Effective Date :";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtOldExpiryDate);
            this.groupBox2.Controls.Add(this.txtOldEffectiveDate);
            this.groupBox2.Controls.Add(this.txtOldNoOfLicense);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 37);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(365, 65);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Previous License";
            // 
            // txtOldExpiryDate
            // 
            this.txtOldExpiryDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtOldExpiryDate.Location = new System.Drawing.Point(102, 37);
            this.txtOldExpiryDate.MaxLength = 50;
            this.txtOldExpiryDate.Name = "txtOldExpiryDate";
            this.txtOldExpiryDate.ReadOnly = true;
            this.txtOldExpiryDate.Size = new System.Drawing.Size(88, 20);
            this.txtOldExpiryDate.TabIndex = 12;
            // 
            // txtOldEffectiveDate
            // 
            this.txtOldEffectiveDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtOldEffectiveDate.Location = new System.Drawing.Point(102, 16);
            this.txtOldEffectiveDate.MaxLength = 50;
            this.txtOldEffectiveDate.Name = "txtOldEffectiveDate";
            this.txtOldEffectiveDate.ReadOnly = true;
            this.txtOldEffectiveDate.Size = new System.Drawing.Size(88, 20);
            this.txtOldEffectiveDate.TabIndex = 11;
            // 
            // txtOldNoOfLicense
            // 
            this.txtOldNoOfLicense.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtOldNoOfLicense.Location = new System.Drawing.Point(285, 16);
            this.txtOldNoOfLicense.MaxLength = 50;
            this.txtOldNoOfLicense.Name = "txtOldNoOfLicense";
            this.txtOldNoOfLicense.ReadOnly = true;
            this.txtOldNoOfLicense.Size = new System.Drawing.Size(66, 20);
            this.txtOldNoOfLicense.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(195, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "No Of License :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Valid To :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Valid From :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmBaseValidLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 195);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbxLicenseInfo);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmBaseValidLicense";
            this.Text = "Valid License";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxLicenseInfo.ResumeLayout(false);
            this.gbxLicenseInfo.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TextBox txtCompanyName;
        protected System.Windows.Forms.Button btnCancelLicense;
        protected System.Windows.Forms.Button btnSaveLicense;
        protected System.Windows.Forms.DateTimePicker dtpValidFrom;
        protected System.Windows.Forms.TextBox txtNoOfLicense;
        protected System.Windows.Forms.TextBox txtOldExpiryDate;
        protected System.Windows.Forms.TextBox txtOldEffectiveDate;
        protected System.Windows.Forms.TextBox txtOldNoOfLicense;
        protected System.Windows.Forms.GroupBox groupBox1;
        protected System.Windows.Forms.GroupBox gbxLicenseInfo;
        protected System.Windows.Forms.GroupBox groupBox2;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.Label label12;
        protected System.Windows.Forms.Label label14;
        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label3;
        protected System.Windows.Forms.Label label4;
    }
}