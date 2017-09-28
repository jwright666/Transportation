namespace FM.TransportMarket.UI

{
    partial class FrmRptQuotation
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.dataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dstQuotation = new FM.TransportMarket.Report.DstQuotation();
            this.dtbSpecialDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dstSpecialData = new FM.TransportMarket.Report.DstSpecialData();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblStartQuotDate = new System.Windows.Forms.Label();
            this.dtpStartQuotDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndQuotDate = new System.Windows.Forms.Label();
            this.dtpEndQuotDate = new System.Windows.Forms.DateTimePicker();
            this.grbOptions = new System.Windows.Forms.GroupBox();
            this.rdoAllQuotations = new System.Windows.Forms.RadioButton();
            this.rdoLatestQuotation = new System.Windows.Forms.RadioButton();
            this.lblEndCustCode = new System.Windows.Forms.Label();
            this.lblStartCustCode = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.cboEndCustNo = new System.Windows.Forms.ComboBox();
            this.cboStartCustNo = new System.Windows.Forms.ComboBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.dataTable1TableAdapter = new FM.TransportMarket.Report.DstQuotationTableAdapters.DataTable1TableAdapter();
            this.dtbSpecialDataTableAdapter = new FM.TransportMarket.Report.DstSpecialDataTableAdapters.DtbSpecialDataTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dstQuotation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtbSpecialDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dstSpecialData)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.grbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataTable1BindingSource
            // 
            this.dataTable1BindingSource.DataMember = "DataTable1";
            this.dataTable1BindingSource.DataSource = this.dstQuotation;
            // 
            // dstQuotation
            // 
            this.dstQuotation.DataSetName = "DstQuotation";
            this.dstQuotation.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dtbSpecialDataBindingSource
            // 
            this.dtbSpecialDataBindingSource.DataMember = "DtbSpecialData";
            this.dtbSpecialDataBindingSource.DataSource = this.dstSpecialData;
            // 
            // dstSpecialData
            // 
            this.dstSpecialData.DataSetName = "DstSpecialData";
            this.dstSpecialData.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pnlHeader.Controls.Add(this.lblStartQuotDate);
            this.pnlHeader.Controls.Add(this.dtpStartQuotDate);
            this.pnlHeader.Controls.Add(this.lblEndQuotDate);
            this.pnlHeader.Controls.Add(this.dtpEndQuotDate);
            this.pnlHeader.Controls.Add(this.grbOptions);
            this.pnlHeader.Controls.Add(this.lblEndCustCode);
            this.pnlHeader.Controls.Add(this.lblStartCustCode);
            this.pnlHeader.Controls.Add(this.btnPreview);
            this.pnlHeader.Controls.Add(this.cboEndCustNo);
            this.pnlHeader.Controls.Add(this.cboStartCustNo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(767, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblStartQuotDate
            // 
            this.lblStartQuotDate.Location = new System.Drawing.Point(5, 38);
            this.lblStartQuotDate.Name = "lblStartQuotDate";
            this.lblStartQuotDate.Size = new System.Drawing.Size(139, 16);
            this.lblStartQuotDate.TabIndex = 9;
            this.lblStartQuotDate.Text = "Start Quotation Date";
            this.lblStartQuotDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpStartQuotDate
            // 
            this.dtpStartQuotDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartQuotDate.Location = new System.Drawing.Point(150, 36);
            this.dtpStartQuotDate.Name = "dtpStartQuotDate";
            this.dtpStartQuotDate.Size = new System.Drawing.Size(121, 20);
            this.dtpStartQuotDate.TabIndex = 8;
            this.dtpStartQuotDate.CloseUp += new System.EventHandler(this.dtpStartQuotDate_CloseUp);
            // 
            // lblEndQuotDate
            // 
            this.lblEndQuotDate.Location = new System.Drawing.Point(275, 38);
            this.lblEndQuotDate.Name = "lblEndQuotDate";
            this.lblEndQuotDate.Size = new System.Drawing.Size(139, 16);
            this.lblEndQuotDate.TabIndex = 7;
            this.lblEndQuotDate.Text = "End Quotation Date";
            this.lblEndQuotDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndQuotDate
            // 
            this.dtpEndQuotDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndQuotDate.Location = new System.Drawing.Point(420, 36);
            this.dtpEndQuotDate.Name = "dtpEndQuotDate";
            this.dtpEndQuotDate.Size = new System.Drawing.Size(121, 20);
            this.dtpEndQuotDate.TabIndex = 6;
            // 
            // grbOptions
            // 
            this.grbOptions.Controls.Add(this.rdoAllQuotations);
            this.grbOptions.Controls.Add(this.rdoLatestQuotation);
            this.grbOptions.Location = new System.Drawing.Point(546, 3);
            this.grbOptions.Name = "grbOptions";
            this.grbOptions.Size = new System.Drawing.Size(138, 53);
            this.grbOptions.TabIndex = 5;
            this.grbOptions.TabStop = false;
            this.grbOptions.Text = "Options";
            // 
            // rdoAllQuotations
            // 
            this.rdoAllQuotations.AutoSize = true;
            this.rdoAllQuotations.Location = new System.Drawing.Point(6, 15);
            this.rdoAllQuotations.Name = "rdoAllQuotations";
            this.rdoAllQuotations.Size = new System.Drawing.Size(90, 17);
            this.rdoAllQuotations.TabIndex = 1;
            this.rdoAllQuotations.Text = "All Quotations";
            this.rdoAllQuotations.UseVisualStyleBackColor = true;
            // 
            // rdoLatestQuotation
            // 
            this.rdoLatestQuotation.AutoSize = true;
            this.rdoLatestQuotation.Checked = true;
            this.rdoLatestQuotation.Location = new System.Drawing.Point(6, 34);
            this.rdoLatestQuotation.Name = "rdoLatestQuotation";
            this.rdoLatestQuotation.Size = new System.Drawing.Size(127, 17);
            this.rdoLatestQuotation.TabIndex = 0;
            this.rdoLatestQuotation.TabStop = true;
            this.rdoLatestQuotation.Text = "Latest Quotation Only";
            this.rdoLatestQuotation.UseVisualStyleBackColor = true;
            // 
            // lblEndCustCode
            // 
            this.lblEndCustCode.Location = new System.Drawing.Point(275, 12);
            this.lblEndCustCode.Name = "lblEndCustCode";
            this.lblEndCustCode.Size = new System.Drawing.Size(139, 16);
            this.lblEndCustCode.TabIndex = 4;
            this.lblEndCustCode.Text = "End Customer Code";
            this.lblEndCustCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStartCustCode
            // 
            this.lblStartCustCode.Location = new System.Drawing.Point(5, 14);
            this.lblStartCustCode.Name = "lblStartCustCode";
            this.lblStartCustCode.Size = new System.Drawing.Size(139, 13);
            this.lblStartCustCode.TabIndex = 3;
            this.lblStartCustCode.Text = "Start Customer Code";
            this.lblStartCustCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPreview
            // 
            this.btnPreview.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnPreview.Location = new System.Drawing.Point(687, 10);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = false;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // cboEndCustNo
            // 
            this.cboEndCustNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEndCustNo.FormattingEnabled = true;
            this.cboEndCustNo.Location = new System.Drawing.Point(420, 10);
            this.cboEndCustNo.Name = "cboEndCustNo";
            this.cboEndCustNo.Size = new System.Drawing.Size(121, 21);
            this.cboEndCustNo.TabIndex = 1;
            this.cboEndCustNo.SelectedIndexChanged += new System.EventHandler(this.cboEndCustNo_SelectedIndexChanged);
            // 
            // cboStartCustNo
            // 
            this.cboStartCustNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartCustNo.FormattingEnabled = true;
            this.cboStartCustNo.Location = new System.Drawing.Point(150, 10);
            this.cboStartCustNo.Name = "cboStartCustNo";
            this.cboStartCustNo.Size = new System.Drawing.Size(121, 21);
            this.cboStartCustNo.TabIndex = 0;
            this.cboStartCustNo.SelectedIndexChanged += new System.EventHandler(this.cboStartCustNo_SelectedIndexChanged);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DstQuotation_DataTable1";
            reportDataSource1.Value = this.dataTable1BindingSource;
            reportDataSource2.Name = "DstSpecialData_DtbSpecialData";
            reportDataSource2.Value = this.dtbSpecialDataBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "FM.TransportMarket.Report.RptQuotation.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 70);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(767, 399);
            this.reportViewer1.TabIndex = 1;
            // 
            // dataTable1TableAdapter
            // 
            this.dataTable1TableAdapter.ClearBeforeFill = true;
            // 
            // dtbSpecialDataTableAdapter
            // 
            this.dtbSpecialDataTableAdapter.ClearBeforeFill = true;
            // 
            // FrmRptQuotation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 469);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FrmRptQuotation";
            this.Text = "Quotation Report";
            this.Load += new System.EventHandler(this.FrmRptQuotation_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmRptQuotation_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dstQuotation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtbSpecialDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dstSpecialData)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.grbOptions.ResumeLayout(false);
            this.grbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private FM.TransportMarket.Report.DstQuotation dstQuotation;
        private System.Windows.Forms.BindingSource dataTable1BindingSource;
        private FM.TransportMarket.Report.DstQuotationTableAdapters.DataTable1TableAdapter dataTable1TableAdapter;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.ComboBox cboEndCustNo;
        private System.Windows.Forms.ComboBox cboStartCustNo;
        private System.Windows.Forms.Label lblEndCustCode;
        private System.Windows.Forms.Label lblStartCustCode;
        private FM.TransportMarket.Report.DstSpecialData dstSpecialData;
        private System.Windows.Forms.BindingSource dtbSpecialDataBindingSource;
        private FM.TransportMarket.Report.DstSpecialDataTableAdapters.DtbSpecialDataTableAdapter dtbSpecialDataTableAdapter;
        private System.Windows.Forms.GroupBox grbOptions;
        private System.Windows.Forms.RadioButton rdoLatestQuotation;
        private System.Windows.Forms.RadioButton rdoAllQuotations;
        private System.Windows.Forms.DateTimePicker dtpEndQuotDate;
        private System.Windows.Forms.Label lblEndQuotDate;
        private System.Windows.Forms.Label lblStartQuotDate;
        private System.Windows.Forms.DateTimePicker dtpStartQuotDate;
    }
}