using FMLicense.BLL.WmsMobile;
namespace FMLicenseUI.Common.WmsMobile
{
    partial class FrmWmsTabletRegistration
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
            this.gbxTabletInfo = new System.Windows.Forms.GroupBox();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBrand = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAndroidID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gbxTabletInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxTabletInfo
            // 
            this.gbxTabletInfo.Controls.Add(this.txtCompanyName);
            this.gbxTabletInfo.Controls.Add(this.btnSave);
            this.gbxTabletInfo.Controls.Add(this.btnCancel);
            this.gbxTabletInfo.Controls.Add(this.lblCompanyName);
            this.gbxTabletInfo.Controls.Add(this.txtDeviceName);
            this.gbxTabletInfo.Controls.Add(this.label7);
            this.gbxTabletInfo.Controls.Add(this.txtBrand);
            this.gbxTabletInfo.Controls.Add(this.label3);
            this.gbxTabletInfo.Controls.Add(this.txtModel);
            this.gbxTabletInfo.Controls.Add(this.label4);
            this.gbxTabletInfo.Controls.Add(this.txtAndroidID);
            this.gbxTabletInfo.Controls.Add(this.label5);
            this.gbxTabletInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxTabletInfo.Location = new System.Drawing.Point(0, 0);
            this.gbxTabletInfo.Name = "gbxTabletInfo";
            this.gbxTabletInfo.Size = new System.Drawing.Size(401, 151);
            this.gbxTabletInfo.TabIndex = 1;
            this.gbxTabletInfo.TabStop = false;
            this.gbxTabletInfo.Text = "Table Information";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtCompanyName.Location = new System.Drawing.Point(141, 18);
            this.txtCompanyName.MaxLength = 50;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.ReadOnly = true;
            this.txtCompanyName.Size = new System.Drawing.Size(251, 20);
            this.txtCompanyName.TabIndex = 30;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(238, 122);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(318, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Location = new System.Drawing.Point(27, 22);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(110, 13);
            this.lblCompanyName.TabIndex = 29;
            this.lblCompanyName.Text = "Company Name :";
            this.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtDeviceName.Location = new System.Drawing.Point(141, 99);
            this.txtDeviceName.MaxLength = 20;
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(251, 20);
            this.txtDeviceName.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(4, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Device Name :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtBrand.Location = new System.Drawing.Point(142, 59);
            this.txtBrand.MaxLength = 20;
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(251, 20);
            this.txtBrand.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Brand :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtModel
            // 
            this.txtModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtModel.Location = new System.Drawing.Point(142, 79);
            this.txtModel.MaxLength = 20;
            this.txtModel.Name = "txtModel";
            this.txtModel.Size = new System.Drawing.Size(251, 20);
            this.txtModel.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Model :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAndroidID
            // 
            this.txtAndroidID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtAndroidID.Location = new System.Drawing.Point(142, 39);
            this.txtAndroidID.MaxLength = 50;
            this.txtAndroidID.Name = "txtAndroidID";
            this.txtAndroidID.Size = new System.Drawing.Size(251, 20);
            this.txtAndroidID.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(5, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Android ID :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmWmsTabletRegistration
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 150);
            this.Controls.Add(this.gbxTabletInfo);
            this.Name = "FrmWmsTabletRegistration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tablet Registration Form";
            this.Load += new System.EventHandler(this.FrmTabletRegistration_Load);
            this.gbxTabletInfo.ResumeLayout(false);
            this.gbxTabletInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxTabletInfo;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtAndroidID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.TextBox txtCompanyName;
    }
}