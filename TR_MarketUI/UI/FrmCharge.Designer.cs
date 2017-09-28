namespace FM.TransportMarket.UI
{
    partial class FrmCharge
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
            this.cboSearchBranch = new System.Windows.Forms.ComboBox();
            this.cboSearchCustVendor = new System.Windows.Forms.ComboBox();
            this.lblBranchCode = new System.Windows.Forms.Label();
            this.lblVendorType = new System.Windows.Forms.Label();
            this.dgvMaster = new System.Windows.Forms.DataGridView();
            this.lblAddVendorType = new System.Windows.Forms.Label();
            this.cboCustVend = new System.Windows.Forms.ComboBox();
            this.lblAddBranchCode = new System.Windows.Forms.Label();
            this.cboBranch = new System.Windows.Forms.ComboBox();
            this.chxIsTruckMov = new System.Windows.Forms.CheckBox();
            this.chxIsOverWeight = new System.Windows.Forms.CheckBox();
            this.cboContainerCode = new System.Windows.Forms.ComboBox();
            this.lblContainerCode = new System.Windows.Forms.Label();
            this.txtTaxCode = new System.Windows.Forms.TextBox();
            this.lblTaxCode = new System.Windows.Forms.Label();
            this.chxIsContainerMov = new System.Windows.Forms.CheckBox();
            this.lblSalesAcctCode = new System.Windows.Forms.Label();
            this.txtSalesAccCode = new System.Windows.Forms.TextBox();
            this.lblUOM = new System.Windows.Forms.Label();
            this.cboUOM = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.lblChargeCode = new System.Windows.Forms.Label();
            this.cboChargeCode = new System.Windows.Forms.ComboBox();
            this.pnlTruckMovementDetails = new System.Windows.Forms.Panel();
            this.gbxTruckMovementOption = new System.Windows.Forms.GroupBox();
            this.lblPackageType = new System.Windows.Forms.Label();
            this.cboPackageType = new System.Windows.Forms.ComboBox();
            this.lblWarnInvAmtDependent = new System.Windows.Forms.Label();
            this.rdbInvoiceDependsOnPackage = new System.Windows.Forms.RadioButton();
            this.rdbDependsOnCapacity = new System.Windows.Forms.RadioButton();
            this.rdbInvoiceDependsOnWtVol = new System.Windows.Forms.RadioButton();
            this.gbxValidUoms = new System.Windows.Forms.GroupBox();
            this.dgvValidUoms = new System.Windows.Forms.DataGridView();
            this.pnlValidUOMTransactions = new System.Windows.Forms.Panel();
            this.btnDeleteValidUom = new System.Windows.Forms.Button();
            this.btnAddValidUom = new System.Windows.Forms.Button();
            this.lblTruckMovementWarning = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chargeCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chargeDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uOMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.salesAccountCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isContainerMovementDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isOverweightDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.containerCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isTruckMovementDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.taxCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branchCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custVendorTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4.SuspendLayout();
            this.tbcEntry.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlQuery.SuspendLayout();
            this.tabPageMaster.SuspendLayout();
            this.pnlMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
            this.pnlTruckMovementDetails.SuspendLayout();
            this.gbxTruckMovementOption.SuspendLayout();
            this.gbxValidUoms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValidUoms)).BeginInit();
            this.pnlValidUOMTransactions.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Size = new System.Drawing.Size(742, 44);
            // 
            // btnDeleteMaster
            // 
            this.btnDeleteMaster.Text = "Delete ";
            // 
            // btnEditMaster
            // 
            this.btnEditMaster.Text = "Edit ";
            // 
            // btnNewMaster
            // 
            this.btnNewMaster.Text = "New ";
            // 
            // btnCancel
            // 
            this.btnCancel.Text = "Cancel ";
            // 
            // btnSave
            // 
            this.btnSave.Text = "Save ";
            // 
            // tbcEntry
            // 
            this.tbcEntry.Size = new System.Drawing.Size(742, 421);
            this.tbcEntry.Click += new System.EventHandler(this.tbcEntry_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(280, 10);
            this.btnQuery.Text = "Query ";
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.Controls.Add(this.dgvMaster);
            this.tabPageSearch.Size = new System.Drawing.Size(734, 392);
            this.tabPageSearch.Text = "Search ";
            this.tabPageSearch.UseVisualStyleBackColor = true;
            this.tabPageSearch.Controls.SetChildIndex(this.pnlQuery, 0);
            this.tabPageSearch.Controls.SetChildIndex(this.dgvMaster, 0);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Location = new System.Drawing.Point(0, 465);
            this.pnlBottom.Size = new System.Drawing.Size(742, 47);
            // 
            // btnPrint
            // 
            this.btnPrint.Text = "Print ";
            this.btnPrint.Visible = false;
            // 
            // pnlQuery
            // 
            this.pnlQuery.Controls.Add(this.cboSearchBranch);
            this.pnlQuery.Controls.Add(this.cboSearchCustVendor);
            this.pnlQuery.Controls.Add(this.lblBranchCode);
            this.pnlQuery.Controls.Add(this.lblVendorType);
            this.pnlQuery.Size = new System.Drawing.Size(728, 65);
            this.pnlQuery.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlQuery.Controls.SetChildIndex(this.lblVendorType, 0);
            this.pnlQuery.Controls.SetChildIndex(this.lblBranchCode, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboSearchCustVendor, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboSearchBranch, 0);
            // 
            // tabPageMaster
            // 
            this.tabPageMaster.Size = new System.Drawing.Size(734, 392);
            this.tabPageMaster.Text = "Master ";
            this.tabPageMaster.UseVisualStyleBackColor = true;
            // 
            // pnlMaster
            // 
            this.pnlMaster.Controls.Add(this.panel1);
            this.pnlMaster.Controls.Add(this.pnlTruckMovementDetails);
            this.pnlMaster.Size = new System.Drawing.Size(728, 386);
            // 
            // bdsMaster
            // 
            this.bdsMaster.DataSource = typeof(FM.TransportMarket.BLL.Charge);
            this.bdsMaster.CurrentChanged += new System.EventHandler(this.bdsMaster_CurrentChanged);
            // 
            // btnClose
            // 
            this.btnClose.Text = "Close ";
            // 
            // cboSearchBranch
            // 
            this.cboSearchBranch.FormattingEnabled = true;
            this.cboSearchBranch.Location = new System.Drawing.Point(132, 10);
            this.cboSearchBranch.Name = "cboSearchBranch";
            this.cboSearchBranch.Size = new System.Drawing.Size(121, 21);
            this.cboSearchBranch.TabIndex = 6;
            // 
            // cboSearchCustVendor
            // 
            this.cboSearchCustVendor.FormattingEnabled = true;
            this.cboSearchCustVendor.Location = new System.Drawing.Point(132, 36);
            this.cboSearchCustVendor.Name = "cboSearchCustVendor";
            this.cboSearchCustVendor.Size = new System.Drawing.Size(121, 21);
            this.cboSearchCustVendor.TabIndex = 9;
            // 
            // lblBranchCode
            // 
            this.lblBranchCode.AutoSize = true;
            this.lblBranchCode.Location = new System.Drawing.Point(51, 13);
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Size = new System.Drawing.Size(75, 13);
            this.lblBranchCode.TabIndex = 7;
            this.lblBranchCode.Text = "Branch Code :";
            // 
            // lblVendorType
            // 
            this.lblVendorType.AutoSize = true;
            this.lblVendorType.Location = new System.Drawing.Point(28, 39);
            this.lblVendorType.Name = "lblVendorType";
            this.lblVendorType.Size = new System.Drawing.Size(98, 13);
            this.lblVendorType.TabIndex = 8;
            this.lblVendorType.Text = "Cust Vendor Type :";
            // 
            // dgvMaster
            // 
            this.dgvMaster.AllowUserToAddRows = false;
            this.dgvMaster.AllowUserToDeleteRows = false;
            this.dgvMaster.AutoGenerateColumns = false;
            this.dgvMaster.BackgroundColor = System.Drawing.Color.DodgerBlue;
            this.dgvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chargeCodeDataGridViewTextBoxColumn,
            this.chargeDescriptionDataGridViewTextBoxColumn,
            this.uOMDataGridViewTextBoxColumn,
            this.salesAccountCodeDataGridViewTextBoxColumn,
            this.isContainerMovementDataGridViewCheckBoxColumn,
            this.isOverweightDataGridViewCheckBoxColumn,
            this.containerCodeDataGridViewTextBoxColumn,
            this.isTruckMovementDataGridViewCheckBoxColumn,
            this.taxCodeDataGridViewTextBoxColumn,
            this.branchCodeDataGridViewTextBoxColumn,
            this.custVendorTypeDataGridViewTextBoxColumn});
            this.dgvMaster.DataSource = this.bdsMaster;
            this.dgvMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMaster.Location = new System.Drawing.Point(3, 68);
            this.dgvMaster.Name = "dgvMaster";
            this.dgvMaster.ReadOnly = true;
            this.dgvMaster.Size = new System.Drawing.Size(728, 321);
            this.dgvMaster.TabIndex = 5;
            // 
            // lblAddVendorType
            // 
            this.lblAddVendorType.AutoSize = true;
            this.lblAddVendorType.Location = new System.Drawing.Point(3, 30);
            this.lblAddVendorType.Name = "lblAddVendorType";
            this.lblAddVendorType.Size = new System.Drawing.Size(98, 13);
            this.lblAddVendorType.TabIndex = 7;
            this.lblAddVendorType.Text = "Cust Vendor Type :";
            this.lblAddVendorType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboCustVend
            // 
            this.cboCustVend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboCustVend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCustVend.FormattingEnabled = true;
            this.cboCustVend.Location = new System.Drawing.Point(105, 26);
            this.cboCustVend.Name = "cboCustVend";
            this.cboCustVend.Size = new System.Drawing.Size(121, 21);
            this.cboCustVend.TabIndex = 1;
            this.cboCustVend.SelectedIndexChanged += new System.EventHandler(this.cboCustVend_SelectedIndexChanged);
            // 
            // lblAddBranchCode
            // 
            this.lblAddBranchCode.AutoSize = true;
            this.lblAddBranchCode.Location = new System.Drawing.Point(26, 8);
            this.lblAddBranchCode.Name = "lblAddBranchCode";
            this.lblAddBranchCode.Size = new System.Drawing.Size(75, 13);
            this.lblAddBranchCode.TabIndex = 5;
            this.lblAddBranchCode.Text = "Branch Code :";
            this.lblAddBranchCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboBranch
            // 
            this.cboBranch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBranch.FormattingEnabled = true;
            this.cboBranch.Location = new System.Drawing.Point(105, 4);
            this.cboBranch.Name = "cboBranch";
            this.cboBranch.Size = new System.Drawing.Size(121, 21);
            this.cboBranch.TabIndex = 0;
            // 
            // chxIsTruckMov
            // 
            this.chxIsTruckMov.AutoSize = true;
            this.chxIsTruckMov.Location = new System.Drawing.Point(416, 139);
            this.chxIsTruckMov.Name = "chxIsTruckMov";
            this.chxIsTruckMov.Size = new System.Drawing.Size(118, 17);
            this.chxIsTruckMov.TabIndex = 11;
            this.chxIsTruckMov.Text = "Is Truck Movement";
            this.chxIsTruckMov.UseVisualStyleBackColor = true;
            this.chxIsTruckMov.CheckedChanged += new System.EventHandler(this.chxIsTruckMov_CheckedChanged);
            // 
            // chxIsOverWeight
            // 
            this.chxIsOverWeight.AutoSize = true;
            this.chxIsOverWeight.Location = new System.Drawing.Point(416, 116);
            this.chxIsOverWeight.Name = "chxIsOverWeight";
            this.chxIsOverWeight.Size = new System.Drawing.Size(91, 17);
            this.chxIsOverWeight.TabIndex = 10;
            this.chxIsOverWeight.Text = "Is Overweight";
            this.chxIsOverWeight.UseVisualStyleBackColor = true;
            // 
            // cboContainerCode
            // 
            this.cboContainerCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboContainerCode.FormattingEnabled = true;
            this.cboContainerCode.Location = new System.Drawing.Point(416, 92);
            this.cboContainerCode.Name = "cboContainerCode";
            this.cboContainerCode.Size = new System.Drawing.Size(121, 21);
            this.cboContainerCode.TabIndex = 9;
            // 
            // lblContainerCode
            // 
            this.lblContainerCode.AutoSize = true;
            this.lblContainerCode.Location = new System.Drawing.Point(327, 95);
            this.lblContainerCode.Name = "lblContainerCode";
            this.lblContainerCode.Size = new System.Drawing.Size(86, 13);
            this.lblContainerCode.TabIndex = 27;
            this.lblContainerCode.Text = "Container Code :";
            // 
            // txtTaxCode
            // 
            this.txtTaxCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtTaxCode.Location = new System.Drawing.Point(416, 48);
            this.txtTaxCode.Name = "txtTaxCode";
            this.txtTaxCode.ReadOnly = true;
            this.txtTaxCode.Size = new System.Drawing.Size(121, 20);
            this.txtTaxCode.TabIndex = 7;
            // 
            // lblTaxCode
            // 
            this.lblTaxCode.AutoSize = true;
            this.lblTaxCode.Location = new System.Drawing.Point(354, 52);
            this.lblTaxCode.Name = "lblTaxCode";
            this.lblTaxCode.Size = new System.Drawing.Size(59, 13);
            this.lblTaxCode.TabIndex = 25;
            this.lblTaxCode.Text = "Tax Code :";
            // 
            // chxIsContainerMov
            // 
            this.chxIsContainerMov.AutoSize = true;
            this.chxIsContainerMov.Checked = true;
            this.chxIsContainerMov.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chxIsContainerMov.Location = new System.Drawing.Point(416, 71);
            this.chxIsContainerMov.Name = "chxIsContainerMov";
            this.chxIsContainerMov.Size = new System.Drawing.Size(135, 17);
            this.chxIsContainerMov.TabIndex = 8;
            this.chxIsContainerMov.Text = "Is Container Movement";
            this.chxIsContainerMov.UseVisualStyleBackColor = true;
            this.chxIsContainerMov.CheckedChanged += new System.EventHandler(this.chxIsContainerMov_CheckedChanged);
            // 
            // lblSalesAcctCode
            // 
            this.lblSalesAcctCode.AutoSize = true;
            this.lblSalesAcctCode.Location = new System.Drawing.Point(303, 30);
            this.lblSalesAcctCode.Name = "lblSalesAcctCode";
            this.lblSalesAcctCode.Size = new System.Drawing.Size(110, 13);
            this.lblSalesAcctCode.TabIndex = 23;
            this.lblSalesAcctCode.Text = "Sales Account Code :";
            // 
            // txtSalesAccCode
            // 
            this.txtSalesAccCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtSalesAccCode.Location = new System.Drawing.Point(416, 26);
            this.txtSalesAccCode.Name = "txtSalesAccCode";
            this.txtSalesAccCode.ReadOnly = true;
            this.txtSalesAccCode.Size = new System.Drawing.Size(121, 20);
            this.txtSalesAccCode.TabIndex = 6;
            // 
            // lblUOM
            // 
            this.lblUOM.AutoSize = true;
            this.lblUOM.Location = new System.Drawing.Point(375, 8);
            this.lblUOM.Name = "lblUOM";
            this.lblUOM.Size = new System.Drawing.Size(38, 13);
            this.lblUOM.TabIndex = 21;
            this.lblUOM.Text = "UOM :";
            // 
            // cboUOM
            // 
            this.cboUOM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboUOM.FormattingEnabled = true;
            this.cboUOM.Location = new System.Drawing.Point(416, 4);
            this.cboUOM.Name = "cboUOM";
            this.cboUOM.Size = new System.Drawing.Size(121, 21);
            this.cboUOM.TabIndex = 5;
            this.cboUOM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboUOM_KeyPress);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(35, 71);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(66, 13);
            this.lblDescription.TabIndex = 19;
            this.lblDescription.Text = "Description :";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(105, 71);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(182, 83);
            this.txtDesc.TabIndex = 4;
            // 
            // lblChargeCode
            // 
            this.lblChargeCode.AutoSize = true;
            this.lblChargeCode.Location = new System.Drawing.Point(26, 52);
            this.lblChargeCode.Name = "lblChargeCode";
            this.lblChargeCode.Size = new System.Drawing.Size(75, 13);
            this.lblChargeCode.TabIndex = 17;
            this.lblChargeCode.Text = "Charge Code :";
            // 
            // cboChargeCode
            // 
            this.cboChargeCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboChargeCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChargeCode.FormattingEnabled = true;
            this.cboChargeCode.Location = new System.Drawing.Point(105, 48);
            this.cboChargeCode.Name = "cboChargeCode";
            this.cboChargeCode.Size = new System.Drawing.Size(121, 21);
            this.cboChargeCode.TabIndex = 3;
            this.cboChargeCode.SelectedIndexChanged += new System.EventHandler(this.cboChargeCode_SelectedIndexChanged);
            // 
            // pnlTruckMovementDetails
            // 
            this.pnlTruckMovementDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pnlTruckMovementDetails.Controls.Add(this.gbxTruckMovementOption);
            this.pnlTruckMovementDetails.Controls.Add(this.gbxValidUoms);
            this.pnlTruckMovementDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTruckMovementDetails.Location = new System.Drawing.Point(0, 214);
            this.pnlTruckMovementDetails.Name = "pnlTruckMovementDetails";
            this.pnlTruckMovementDetails.Size = new System.Drawing.Size(728, 172);
            this.pnlTruckMovementDetails.TabIndex = 1;
            this.pnlTruckMovementDetails.Visible = false;
            // 
            // gbxTruckMovementOption
            // 
            this.gbxTruckMovementOption.Controls.Add(this.lblPackageType);
            this.gbxTruckMovementOption.Controls.Add(this.cboPackageType);
            this.gbxTruckMovementOption.Controls.Add(this.lblWarnInvAmtDependent);
            this.gbxTruckMovementOption.Controls.Add(this.rdbInvoiceDependsOnPackage);
            this.gbxTruckMovementOption.Controls.Add(this.rdbDependsOnCapacity);
            this.gbxTruckMovementOption.Controls.Add(this.rdbInvoiceDependsOnWtVol);
            this.gbxTruckMovementOption.Location = new System.Drawing.Point(0, 4);
            this.gbxTruckMovementOption.Name = "gbxTruckMovementOption";
            this.gbxTruckMovementOption.Size = new System.Drawing.Size(373, 164);
            this.gbxTruckMovementOption.TabIndex = 33;
            this.gbxTruckMovementOption.TabStop = false;
            this.gbxTruckMovementOption.Text = "Truck Movement Options";
            // 
            // lblPackageType
            // 
            this.lblPackageType.AutoSize = true;
            this.lblPackageType.Location = new System.Drawing.Point(29, 89);
            this.lblPackageType.Name = "lblPackageType";
            this.lblPackageType.Size = new System.Drawing.Size(83, 13);
            this.lblPackageType.TabIndex = 15;
            this.lblPackageType.Text = "Package Type :";
            // 
            // cboPackageType
            // 
            this.cboPackageType.FormattingEnabled = true;
            this.cboPackageType.Location = new System.Drawing.Point(119, 85);
            this.cboPackageType.Name = "cboPackageType";
            this.cboPackageType.Size = new System.Drawing.Size(107, 21);
            this.cboPackageType.TabIndex = 14;
            // 
            // lblWarnInvAmtDependent
            // 
            this.lblWarnInvAmtDependent.ForeColor = System.Drawing.Color.Red;
            this.lblWarnInvAmtDependent.Location = new System.Drawing.Point(6, 122);
            this.lblWarnInvAmtDependent.Name = "lblWarnInvAmtDependent";
            this.lblWarnInvAmtDependent.Size = new System.Drawing.Size(336, 39);
            this.lblWarnInvAmtDependent.TabIndex = 13;
            this.lblWarnInvAmtDependent.Text = "Note :";
            // 
            // rdbInvoiceDependsOnPackage
            // 
            this.rdbInvoiceDependsOnPackage.Checked = true;
            this.rdbInvoiceDependsOnPackage.Location = new System.Drawing.Point(8, 63);
            this.rdbInvoiceDependsOnPackage.Name = "rdbInvoiceDependsOnPackage";
            this.rdbInvoiceDependsOnPackage.Size = new System.Drawing.Size(359, 17);
            this.rdbInvoiceDependsOnPackage.TabIndex = 4;
            this.rdbInvoiceDependsOnPackage.TabStop = true;
            this.rdbInvoiceDependsOnPackage.Text = "Invoice amount depends on package type of container";
            this.rdbInvoiceDependsOnPackage.UseVisualStyleBackColor = true;
            this.rdbInvoiceDependsOnPackage.CheckedChanged += new System.EventHandler(this.rdbInvoiceDependsOnPackage_CheckedChanged);
            // 
            // rdbDependsOnCapacity
            // 
            this.rdbDependsOnCapacity.Location = new System.Drawing.Point(8, 40);
            this.rdbDependsOnCapacity.Name = "rdbDependsOnCapacity";
            this.rdbDependsOnCapacity.Size = new System.Drawing.Size(359, 17);
            this.rdbDependsOnCapacity.TabIndex = 3;
            this.rdbDependsOnCapacity.TabStop = true;
            this.rdbDependsOnCapacity.Text = "Invoice amount depends on truck capacity";
            this.rdbDependsOnCapacity.UseVisualStyleBackColor = true;
            // 
            // rdbInvoiceDependsOnWtVol
            // 
            this.rdbInvoiceDependsOnWtVol.Location = new System.Drawing.Point(8, 18);
            this.rdbInvoiceDependsOnWtVol.Name = "rdbInvoiceDependsOnWtVol";
            this.rdbInvoiceDependsOnWtVol.Size = new System.Drawing.Size(359, 17);
            this.rdbInvoiceDependsOnWtVol.TabIndex = 2;
            this.rdbInvoiceDependsOnWtVol.TabStop = true;
            this.rdbInvoiceDependsOnWtVol.Text = "Invoice amount depends on weight or volume for each job trip";
            this.rdbInvoiceDependsOnWtVol.UseVisualStyleBackColor = true;
            // 
            // gbxValidUoms
            // 
            this.gbxValidUoms.Controls.Add(this.dgvValidUoms);
            this.gbxValidUoms.Controls.Add(this.pnlValidUOMTransactions);
            this.gbxValidUoms.Location = new System.Drawing.Point(373, 4);
            this.gbxValidUoms.Name = "gbxValidUoms";
            this.gbxValidUoms.Size = new System.Drawing.Size(355, 164);
            this.gbxValidUoms.TabIndex = 34;
            this.gbxValidUoms.TabStop = false;
            this.gbxValidUoms.Text = "Valid Uom for Truck Movement Charges";
            // 
            // dgvValidUoms
            // 
            this.dgvValidUoms.AllowUserToAddRows = false;
            this.dgvValidUoms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvValidUoms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvValidUoms.Location = new System.Drawing.Point(3, 44);
            this.dgvValidUoms.Name = "dgvValidUoms";
            this.dgvValidUoms.Size = new System.Drawing.Size(349, 117);
            this.dgvValidUoms.TabIndex = 0;
            // 
            // pnlValidUOMTransactions
            // 
            this.pnlValidUOMTransactions.Controls.Add(this.btnDeleteValidUom);
            this.pnlValidUOMTransactions.Controls.Add(this.btnAddValidUom);
            this.pnlValidUOMTransactions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlValidUOMTransactions.Location = new System.Drawing.Point(3, 16);
            this.pnlValidUOMTransactions.Name = "pnlValidUOMTransactions";
            this.pnlValidUOMTransactions.Size = new System.Drawing.Size(349, 28);
            this.pnlValidUOMTransactions.TabIndex = 1;
            // 
            // btnDeleteValidUom
            // 
            this.btnDeleteValidUom.Location = new System.Drawing.Point(124, 3);
            this.btnDeleteValidUom.Name = "btnDeleteValidUom";
            this.btnDeleteValidUom.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteValidUom.TabIndex = 2;
            this.btnDeleteValidUom.Text = "Delete";
            this.btnDeleteValidUom.UseVisualStyleBackColor = true;
            this.btnDeleteValidUom.Click += new System.EventHandler(this.btnDeleteValidUom_Click);
            // 
            // btnAddValidUom
            // 
            this.btnAddValidUom.Location = new System.Drawing.Point(43, 3);
            this.btnAddValidUom.Name = "btnAddValidUom";
            this.btnAddValidUom.Size = new System.Drawing.Size(75, 23);
            this.btnAddValidUom.TabIndex = 0;
            this.btnAddValidUom.Text = "Add";
            this.btnAddValidUom.UseVisualStyleBackColor = true;
            this.btnAddValidUom.Click += new System.EventHandler(this.btnAddValidUom_Click);
            // 
            // lblTruckMovementWarning
            // 
            this.lblTruckMovementWarning.AutoSize = true;
            this.lblTruckMovementWarning.ForeColor = System.Drawing.Color.Red;
            this.lblTruckMovementWarning.Location = new System.Drawing.Point(413, 159);
            this.lblTruckMovementWarning.Name = "lblTruckMovementWarning";
            this.lblTruckMovementWarning.Size = new System.Drawing.Size(0, 13);
            this.lblTruckMovementWarning.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.lblAddBranchCode);
            this.panel1.Controls.Add(this.lblTruckMovementWarning);
            this.panel1.Controls.Add(this.cboBranch);
            this.panel1.Controls.Add(this.cboCustVend);
            this.panel1.Controls.Add(this.chxIsTruckMov);
            this.panel1.Controls.Add(this.lblAddVendorType);
            this.panel1.Controls.Add(this.chxIsOverWeight);
            this.panel1.Controls.Add(this.cboChargeCode);
            this.panel1.Controls.Add(this.cboContainerCode);
            this.panel1.Controls.Add(this.lblChargeCode);
            this.panel1.Controls.Add(this.lblContainerCode);
            this.panel1.Controls.Add(this.txtDesc);
            this.panel1.Controls.Add(this.txtTaxCode);
            this.panel1.Controls.Add(this.lblDescription);
            this.panel1.Controls.Add(this.lblTaxCode);
            this.panel1.Controls.Add(this.cboUOM);
            this.panel1.Controls.Add(this.chxIsContainerMov);
            this.panel1.Controls.Add(this.lblUOM);
            this.panel1.Controls.Add(this.lblSalesAcctCode);
            this.panel1.Controls.Add(this.txtSalesAccCode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(728, 214);
            this.panel1.TabIndex = 32;
            // 
            // chargeCodeDataGridViewTextBoxColumn
            // 
            this.chargeCodeDataGridViewTextBoxColumn.DataPropertyName = "ChargeCode";
            this.chargeCodeDataGridViewTextBoxColumn.HeaderText = "ChargeCode";
            this.chargeCodeDataGridViewTextBoxColumn.Name = "chargeCodeDataGridViewTextBoxColumn";
            this.chargeCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // chargeDescriptionDataGridViewTextBoxColumn
            // 
            this.chargeDescriptionDataGridViewTextBoxColumn.DataPropertyName = "ChargeDescription";
            this.chargeDescriptionDataGridViewTextBoxColumn.HeaderText = "ChargeDescription";
            this.chargeDescriptionDataGridViewTextBoxColumn.Name = "chargeDescriptionDataGridViewTextBoxColumn";
            this.chargeDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // uOMDataGridViewTextBoxColumn
            // 
            this.uOMDataGridViewTextBoxColumn.DataPropertyName = "UOM";
            this.uOMDataGridViewTextBoxColumn.HeaderText = "UOM";
            this.uOMDataGridViewTextBoxColumn.Name = "uOMDataGridViewTextBoxColumn";
            this.uOMDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // salesAccountCodeDataGridViewTextBoxColumn
            // 
            this.salesAccountCodeDataGridViewTextBoxColumn.DataPropertyName = "SalesAccountCode";
            this.salesAccountCodeDataGridViewTextBoxColumn.HeaderText = "SalesAccountCode";
            this.salesAccountCodeDataGridViewTextBoxColumn.Name = "salesAccountCodeDataGridViewTextBoxColumn";
            this.salesAccountCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isContainerMovementDataGridViewCheckBoxColumn
            // 
            this.isContainerMovementDataGridViewCheckBoxColumn.DataPropertyName = "IsContainerMovement";
            this.isContainerMovementDataGridViewCheckBoxColumn.HeaderText = "IsContainerMovement";
            this.isContainerMovementDataGridViewCheckBoxColumn.Name = "isContainerMovementDataGridViewCheckBoxColumn";
            this.isContainerMovementDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // isOverweightDataGridViewCheckBoxColumn
            // 
            this.isOverweightDataGridViewCheckBoxColumn.DataPropertyName = "IsOverweight";
            this.isOverweightDataGridViewCheckBoxColumn.HeaderText = "IsOverweight";
            this.isOverweightDataGridViewCheckBoxColumn.Name = "isOverweightDataGridViewCheckBoxColumn";
            this.isOverweightDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // containerCodeDataGridViewTextBoxColumn
            // 
            this.containerCodeDataGridViewTextBoxColumn.DataPropertyName = "ContainerCode";
            this.containerCodeDataGridViewTextBoxColumn.HeaderText = "ContainerCode";
            this.containerCodeDataGridViewTextBoxColumn.Name = "containerCodeDataGridViewTextBoxColumn";
            this.containerCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isTruckMovementDataGridViewCheckBoxColumn
            // 
            this.isTruckMovementDataGridViewCheckBoxColumn.DataPropertyName = "IsTruckMovement";
            this.isTruckMovementDataGridViewCheckBoxColumn.HeaderText = "IsTruckMovement";
            this.isTruckMovementDataGridViewCheckBoxColumn.Name = "isTruckMovementDataGridViewCheckBoxColumn";
            this.isTruckMovementDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // taxCodeDataGridViewTextBoxColumn
            // 
            this.taxCodeDataGridViewTextBoxColumn.DataPropertyName = "TaxCode";
            this.taxCodeDataGridViewTextBoxColumn.HeaderText = "TaxCode";
            this.taxCodeDataGridViewTextBoxColumn.Name = "taxCodeDataGridViewTextBoxColumn";
            this.taxCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // branchCodeDataGridViewTextBoxColumn
            // 
            this.branchCodeDataGridViewTextBoxColumn.DataPropertyName = "BranchCode";
            this.branchCodeDataGridViewTextBoxColumn.HeaderText = "BranchCode";
            this.branchCodeDataGridViewTextBoxColumn.Name = "branchCodeDataGridViewTextBoxColumn";
            this.branchCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // custVendorTypeDataGridViewTextBoxColumn
            // 
            this.custVendorTypeDataGridViewTextBoxColumn.DataPropertyName = "CustVendorType";
            this.custVendorTypeDataGridViewTextBoxColumn.HeaderText = "CustVendorType";
            this.custVendorTypeDataGridViewTextBoxColumn.Name = "custVendorTypeDataGridViewTextBoxColumn";
            this.custVendorTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FrmCharge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 512);
            this.Name = "FrmCharge";
            this.Text = "Transport Charge";
            this.groupBox4.ResumeLayout(false);
            this.tbcEntry.ResumeLayout(false);
            this.tabPageSearch.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlQuery.ResumeLayout(false);
            this.pnlQuery.PerformLayout();
            this.tabPageMaster.ResumeLayout(false);
            this.pnlMaster.ResumeLayout(false);
            this.pnlMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
            this.pnlTruckMovementDetails.ResumeLayout(false);
            this.gbxTruckMovementOption.ResumeLayout(false);
            this.gbxTruckMovementOption.PerformLayout();
            this.gbxValidUoms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValidUoms)).EndInit();
            this.pnlValidUOMTransactions.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSearchBranch;
        private System.Windows.Forms.ComboBox cboSearchCustVendor;
        private System.Windows.Forms.Label lblBranchCode;
        private System.Windows.Forms.Label lblVendorType;
        private System.Windows.Forms.DataGridView dgvMaster;
        private System.Windows.Forms.Label lblAddVendorType;
        private System.Windows.Forms.ComboBox cboCustVend;
        private System.Windows.Forms.Label lblAddBranchCode;
        private System.Windows.Forms.ComboBox cboBranch;
        private System.Windows.Forms.CheckBox chxIsTruckMov;
        private System.Windows.Forms.CheckBox chxIsOverWeight;
        private System.Windows.Forms.ComboBox cboContainerCode;
        private System.Windows.Forms.Label lblContainerCode;
        private System.Windows.Forms.TextBox txtTaxCode;
        private System.Windows.Forms.Label lblTaxCode;
        private System.Windows.Forms.CheckBox chxIsContainerMov;
        private System.Windows.Forms.Label lblSalesAcctCode;
        private System.Windows.Forms.TextBox txtSalesAccCode;
        private System.Windows.Forms.Label lblUOM;
        private System.Windows.Forms.ComboBox cboUOM;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblChargeCode;
        private System.Windows.Forms.ComboBox cboChargeCode;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isHigherOfAmtWtVolDataGridViewCheckBoxColumn;
        private System.Windows.Forms.Panel pnlTruckMovementDetails;
        private System.Windows.Forms.GroupBox gbxTruckMovementOption;
        private System.Windows.Forms.GroupBox gbxValidUoms;
        private System.Windows.Forms.DataGridView dgvValidUoms;
        private System.Windows.Forms.Panel pnlValidUOMTransactions;
        private System.Windows.Forms.Button btnDeleteValidUom;
        private System.Windows.Forms.Button btnAddValidUom;
        private System.Windows.Forms.Label lblTruckMovementWarning;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblWarnInvAmtDependent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn chargeTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isInvoiceAmtDependentOnWtVolDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isHigherOfActWtVolDataGridViewCheckBoxColumn;
        private System.Windows.Forms.RadioButton rdbInvoiceDependsOnPackage;
        private System.Windows.Forms.RadioButton rdbDependsOnCapacity;
        private System.Windows.Forms.RadioButton rdbInvoiceDependsOnWtVol;
        private System.Windows.Forms.Label lblPackageType;
        private System.Windows.Forms.ComboBox cboPackageType;
        private System.Windows.Forms.DataGridViewTextBoxColumn chargeCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chargeDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uOMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn salesAccountCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isContainerMovementDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOverweightDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn containerCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isTruckMovementDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn branchCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn custVendorTypeDataGridViewTextBoxColumn;
    }
}