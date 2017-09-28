namespace FM.TR_TRKBookUI.UI
{
    partial class FrmAirJob
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnRetrieveAirJob = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblCustName = new System.Windows.Forms.Label();
            this.txtCustName = new System.Windows.Forms.TextBox();
            this.gbxJobType = new System.Windows.Forms.GroupBox();
            this.rdbAirImport = new System.Windows.Forms.RadioButton();
            this.rdbAirExport = new System.Windows.Forms.RadioButton();
            this.gbxJobDetail = new System.Windows.Forms.GroupBox();
            this.cboAWBNo = new System.Windows.Forms.ComboBox();
            this.lblAWBNo = new System.Windows.Forms.Label();
            this.cboJobNo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHAWB = new System.Windows.Forms.TextBox();
            this.lblMAWB = new System.Windows.Forms.Label();
            this.txtMAWB = new System.Windows.Forms.TextBox();
            this.lblJobNo = new System.Windows.Forms.Label();
            this.gbxBillingOption = new System.Windows.Forms.GroupBox();
            this.rdbBillByTrucking = new System.Windows.Forms.RadioButton();
            this.rdbBillByAirFreight = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbxJobType.SuspendLayout();
            this.gbxJobDetail.SuspendLayout();
            this.gbxBillingOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 339);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblStatus);
            this.panel3.Controls.Add(this.txtStatus);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 147);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(564, 161);
            this.panel3.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(5, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status :";
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtStatus.Location = new System.Drawing.Point(53, 5);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(508, 134);
            this.txtStatus.TabIndex = 3;
            this.txtStatus.Text = "";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnRetrieveAirJob);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 308);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(564, 31);
            this.panel4.TabIndex = 11;
            // 
            // btnRetrieveAirJob
            // 
            this.btnRetrieveAirJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRetrieveAirJob.Location = new System.Drawing.Point(45, 3);
            this.btnRetrieveAirJob.Name = "btnRetrieveAirJob";
            this.btnRetrieveAirJob.Size = new System.Drawing.Size(124, 23);
            this.btnRetrieveAirJob.TabIndex = 5;
            this.btnRetrieveAirJob.Text = "Retrieve Air Job";
            this.btnRetrieveAirJob.UseVisualStyleBackColor = true;
            this.btnRetrieveAirJob.Click += new System.EventHandler(this.btnRetreiveAirJob_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(171, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(81, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(254, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(81, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblCustName);
            this.panel2.Controls.Add(this.txtCustName);
            this.panel2.Controls.Add(this.gbxJobType);
            this.panel2.Controls.Add(this.gbxJobDetail);
            this.panel2.Controls.Add(this.gbxBillingOption);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(564, 147);
            this.panel2.TabIndex = 9;
            // 
            // lblCustName
            // 
            this.lblCustName.Location = new System.Drawing.Point(12, 121);
            this.lblCustName.Name = "lblCustName";
            this.lblCustName.Size = new System.Drawing.Size(80, 20);
            this.lblCustName.TabIndex = 7;
            this.lblCustName.Text = "Cust. Name.";
            this.lblCustName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCustName
            // 
            this.txtCustName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtCustName.Location = new System.Drawing.Point(98, 121);
            this.txtCustName.MaxLength = 14;
            this.txtCustName.Name = "txtCustName";
            this.txtCustName.ReadOnly = true;
            this.txtCustName.Size = new System.Drawing.Size(306, 20);
            this.txtCustName.TabIndex = 6;
            // 
            // gbxJobType
            // 
            this.gbxJobType.Controls.Add(this.rdbAirImport);
            this.gbxJobType.Controls.Add(this.rdbAirExport);
            this.gbxJobType.Location = new System.Drawing.Point(13, 12);
            this.gbxJobType.Name = "gbxJobType";
            this.gbxJobType.Size = new System.Drawing.Size(132, 72);
            this.gbxJobType.TabIndex = 0;
            this.gbxJobType.TabStop = false;
            this.gbxJobType.Text = "Air Job Type";
            // 
            // rdbAirImport
            // 
            this.rdbAirImport.AutoSize = true;
            this.rdbAirImport.Location = new System.Drawing.Point(17, 43);
            this.rdbAirImport.Name = "rdbAirImport";
            this.rdbAirImport.Size = new System.Drawing.Size(69, 17);
            this.rdbAirImport.TabIndex = 1;
            this.rdbAirImport.Text = "Air Import";
            this.rdbAirImport.UseVisualStyleBackColor = true;
            this.rdbAirImport.CheckedChanged += new System.EventHandler(this.rdbAirJobType_CheckedChanged);
            // 
            // rdbAirExport
            // 
            this.rdbAirExport.AutoSize = true;
            this.rdbAirExport.Location = new System.Drawing.Point(17, 20);
            this.rdbAirExport.Name = "rdbAirExport";
            this.rdbAirExport.Size = new System.Drawing.Size(70, 17);
            this.rdbAirExport.TabIndex = 0;
            this.rdbAirExport.Text = "Air Export";
            this.rdbAirExport.UseVisualStyleBackColor = true;
            this.rdbAirExport.CheckedChanged += new System.EventHandler(this.rdbAirJobType_CheckedChanged);
            // 
            // gbxJobDetail
            // 
            this.gbxJobDetail.Controls.Add(this.cboAWBNo);
            this.gbxJobDetail.Controls.Add(this.lblAWBNo);
            this.gbxJobDetail.Controls.Add(this.cboJobNo);
            this.gbxJobDetail.Controls.Add(this.label2);
            this.gbxJobDetail.Controls.Add(this.txtHAWB);
            this.gbxJobDetail.Controls.Add(this.lblMAWB);
            this.gbxJobDetail.Controls.Add(this.txtMAWB);
            this.gbxJobDetail.Controls.Add(this.lblJobNo);
            this.gbxJobDetail.Location = new System.Drawing.Point(147, 12);
            this.gbxJobDetail.Name = "gbxJobDetail";
            this.gbxJobDetail.Size = new System.Drawing.Size(257, 105);
            this.gbxJobDetail.TabIndex = 1;
            this.gbxJobDetail.TabStop = false;
            this.gbxJobDetail.Text = "Air Job Details";
            // 
            // cboAWBNo
            // 
            this.cboAWBNo.FormattingEnabled = true;
            this.cboAWBNo.Location = new System.Drawing.Point(87, 16);
            this.cboAWBNo.Name = "cboAWBNo";
            this.cboAWBNo.Size = new System.Drawing.Size(161, 21);
            this.cboAWBNo.TabIndex = 7;
            this.cboAWBNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CBO_KeyPress);
            this.cboAWBNo.Leave += new System.EventHandler(this.cboAWBNo_Leave);
            // 
            // lblAWBNo
            // 
            this.lblAWBNo.Location = new System.Drawing.Point(6, 16);
            this.lblAWBNo.Name = "lblAWBNo";
            this.lblAWBNo.Size = new System.Drawing.Size(80, 20);
            this.lblAWBNo.TabIndex = 6;
            this.lblAWBNo.Text = "AWB No.";
            this.lblAWBNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboJobNo
            // 
            this.cboJobNo.FormattingEnabled = true;
            this.cboJobNo.Location = new System.Drawing.Point(87, 38);
            this.cboJobNo.Name = "cboJobNo";
            this.cboJobNo.Size = new System.Drawing.Size(161, 21);
            this.cboJobNo.TabIndex = 1;
            this.cboJobNo.SelectedValueChanged += new System.EventHandler(this.cboJobNo_SelectedValueChanged);
            this.cboJobNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CBO_KeyPress);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "HAWB No.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtHAWB
            // 
            this.txtHAWB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtHAWB.Location = new System.Drawing.Point(87, 81);
            this.txtHAWB.MaxLength = 14;
            this.txtHAWB.Name = "txtHAWB";
            this.txtHAWB.ReadOnly = true;
            this.txtHAWB.Size = new System.Drawing.Size(161, 20);
            this.txtHAWB.TabIndex = 4;
            // 
            // lblMAWB
            // 
            this.lblMAWB.Location = new System.Drawing.Point(6, 60);
            this.lblMAWB.Name = "lblMAWB";
            this.lblMAWB.Size = new System.Drawing.Size(80, 20);
            this.lblMAWB.TabIndex = 3;
            this.lblMAWB.Text = "MAWB No.";
            this.lblMAWB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMAWB
            // 
            this.txtMAWB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtMAWB.Location = new System.Drawing.Point(87, 60);
            this.txtMAWB.MaxLength = 14;
            this.txtMAWB.Name = "txtMAWB";
            this.txtMAWB.ReadOnly = true;
            this.txtMAWB.Size = new System.Drawing.Size(161, 20);
            this.txtMAWB.TabIndex = 2;
            // 
            // lblJobNo
            // 
            this.lblJobNo.Location = new System.Drawing.Point(6, 38);
            this.lblJobNo.Name = "lblJobNo";
            this.lblJobNo.Size = new System.Drawing.Size(80, 20);
            this.lblJobNo.TabIndex = 1;
            this.lblJobNo.Text = "Job No.";
            this.lblJobNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxBillingOption
            // 
            this.gbxBillingOption.Controls.Add(this.rdbBillByTrucking);
            this.gbxBillingOption.Controls.Add(this.rdbBillByAirFreight);
            this.gbxBillingOption.Location = new System.Drawing.Point(410, 13);
            this.gbxBillingOption.Name = "gbxBillingOption";
            this.gbxBillingOption.Size = new System.Drawing.Size(151, 71);
            this.gbxBillingOption.TabIndex = 2;
            this.gbxBillingOption.TabStop = false;
            this.gbxBillingOption.Text = "Billing Option";
            // 
            // rdbBillByTrucking
            // 
            this.rdbBillByTrucking.AutoSize = true;
            this.rdbBillByTrucking.Location = new System.Drawing.Point(17, 43);
            this.rdbBillByTrucking.Name = "rdbBillByTrucking";
            this.rdbBillByTrucking.Size = new System.Drawing.Size(97, 17);
            this.rdbBillByTrucking.TabIndex = 1;
            this.rdbBillByTrucking.TabStop = true;
            this.rdbBillByTrucking.Text = "Bill by Trucking";
            this.rdbBillByTrucking.UseVisualStyleBackColor = true;
            // 
            // rdbBillByAirFreight
            // 
            this.rdbBillByAirFreight.AutoSize = true;
            this.rdbBillByAirFreight.Location = new System.Drawing.Point(17, 20);
            this.rdbBillByAirFreight.Name = "rdbBillByAirFreight";
            this.rdbBillByAirFreight.Size = new System.Drawing.Size(102, 17);
            this.rdbBillByAirFreight.TabIndex = 0;
            this.rdbBillByAirFreight.TabStop = true;
            this.rdbBillByAirFreight.Text = "Bill by Air Freight";
            this.rdbBillByAirFreight.UseVisualStyleBackColor = true;
            this.rdbBillByAirFreight.CheckedChanged += new System.EventHandler(this.ChkBillingOption_CheckChanged);
            // 
            // FrmAirJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 339);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(580, 331);
            this.Name = "FrmAirJob";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Air Job";
            this.Load += new System.EventHandler(this.FrmAirJob_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gbxJobType.ResumeLayout(false);
            this.gbxJobType.PerformLayout();
            this.gbxJobDetail.ResumeLayout(false);
            this.gbxJobDetail.PerformLayout();
            this.gbxBillingOption.ResumeLayout(false);
            this.gbxBillingOption.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbxJobDetail;
        private System.Windows.Forms.GroupBox gbxJobType;
        private System.Windows.Forms.RadioButton rdbAirImport;
        private System.Windows.Forms.RadioButton rdbAirExport;
        private System.Windows.Forms.Label lblJobNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHAWB;
        private System.Windows.Forms.Label lblMAWB;
        private System.Windows.Forms.TextBox txtMAWB;
        private System.Windows.Forms.RichTextBox txtStatus;
        private System.Windows.Forms.GroupBox gbxBillingOption;
        private System.Windows.Forms.RadioButton rdbBillByTrucking;
        private System.Windows.Forms.RadioButton rdbBillByAirFreight;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRetrieveAirJob;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cboJobNo;
        private System.Windows.Forms.Label lblCustName;
        private System.Windows.Forms.TextBox txtCustName;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cboAWBNo;
        private System.Windows.Forms.Label lblAWBNo;
    }
}