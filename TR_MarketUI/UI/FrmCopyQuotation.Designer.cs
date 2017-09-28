namespace FM.TransportMarket.UI
{
    partial class FrmCopyQuotation
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
            this.lblQuotationNo = new System.Windows.Forms.Label();
            this.cboQuotationNo = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.txtQuotationPeriod = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblQuotationNo
            // 
            this.lblQuotationNo.Location = new System.Drawing.Point(10, 14);
            this.lblQuotationNo.Name = "lblQuotationNo";
            this.lblQuotationNo.Size = new System.Drawing.Size(92, 13);
            this.lblQuotationNo.TabIndex = 0;
            this.lblQuotationNo.Text = "Quotation No:";
            this.lblQuotationNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboQuotationNo
            // 
            this.cboQuotationNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboQuotationNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboQuotationNo.FormattingEnabled = true;
            this.cboQuotationNo.Location = new System.Drawing.Point(108, 10);
            this.cboQuotationNo.Name = "cboQuotationNo";
            this.cboQuotationNo.Size = new System.Drawing.Size(146, 21);
            this.cboQuotationNo.TabIndex = 1;
            this.cboQuotationNo.SelectedIndexChanged += new System.EventHandler(this.cboQuotationNo_SelectedIndexChanged);
            this.cboQuotationNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboQuotationNo_KeyPress);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 33);
            this.panel1.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(182, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(101, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(65, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Save";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Location = new System.Drawing.Point(10, 38);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(92, 13);
            this.lblCustomerName.TabIndex = 3;
            this.lblCustomerName.Text = "Customer Name:";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtCustomerName.Location = new System.Drawing.Point(108, 34);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.ReadOnly = true;
            this.txtCustomerName.Size = new System.Drawing.Size(204, 20);
            this.txtCustomerName.TabIndex = 4;
            // 
            // txtQuotationPeriod
            // 
            this.txtQuotationPeriod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtQuotationPeriod.Location = new System.Drawing.Point(108, 58);
            this.txtQuotationPeriod.Name = "txtQuotationPeriod";
            this.txtQuotationPeriod.ReadOnly = true;
            this.txtQuotationPeriod.Size = new System.Drawing.Size(204, 20);
            this.txtQuotationPeriod.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Quotation Period:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmCopyQuotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(312, 113);
            this.Controls.Add(this.txtQuotationPeriod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustomerName);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cboQuotationNo);
            this.Controls.Add(this.lblQuotationNo);
            this.MaximumSize = new System.Drawing.Size(328, 151);
            this.MinimumSize = new System.Drawing.Size(328, 151);
            this.Name = "FrmCopyQuotation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Quotation No";
            this.Load += new System.EventHandler(this.FrmCopyQuotation_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblQuotationNo;
        private System.Windows.Forms.ComboBox cboQuotationNo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.TextBox txtQuotationPeriod;
        private System.Windows.Forms.Label label1;
    }
}