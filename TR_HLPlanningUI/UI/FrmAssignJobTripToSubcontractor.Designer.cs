using FM.TR_HLBookDLL.BLL;
namespace FM.TransportPlanning.UI
{
    partial class FrmAssignJobTripToSubcontractor
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblJobID = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.cboStartJobID = new System.Windows.Forms.ComboBox();
            this.cboEndJobID = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.grpSubCon = new System.Windows.Forms.GroupBox();
            this.rdbBoth = new System.Windows.Forms.RadioButton();
            this.rdbNo = new System.Windows.Forms.RadioButton();
            this.rdbYes = new System.Windows.Forms.RadioButton();
            this.dgvJobTrip = new System.Windows.Forms.DataGridView();
            this.ContainerNoGrid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtEditJobID = new System.Windows.Forms.TextBox();
            this.txtEditSequence = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEditContainerNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEditContainerCode = new System.Windows.Forms.TextBox();
            this.chkOwnTransport = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEditStatus = new System.Windows.Forms.TextBox();
            this.cboEditSubContractor = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtEditSubContractorRef = new System.Windows.Forms.TextBox();
            this.maxCBMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxWeightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cargoDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.containerCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isLadenDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.grossCBMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grossWeightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sealNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.containerNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isDangerousGoodsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dGRemarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isOversizeDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.oversizeRemarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isDirectDeliveryDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.legTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partnerLegDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jobIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jobNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sequenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tripStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endStopDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startStopDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.updateVersionDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.isNewDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.customerCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ownTransportDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.subContractorCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subContractorReferenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chargeCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4.SuspendLayout();
            this.tbcEntry.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlQuery.SuspendLayout();
            this.tabPageMaster.SuspendLayout();
            this.pnlMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).BeginInit();
            this.grpSubCon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobTrip)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDeleteMaster
            // 
            this.btnDeleteMaster.Text = "Delete ";
            this.btnDeleteMaster.Visible = false;
            // 
            // btnEditMaster
            // 
            this.btnEditMaster.Text = "Assign";
            this.btnEditMaster.Click += new System.EventHandler(this.btnEditMaster_Click);
            // 
            // btnNewMaster
            // 
            this.btnNewMaster.Text = "New ";
            this.btnNewMaster.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Text = "Cancel ";
            // 
            // btnSave
            // 
            this.btnSave.Text = "Save ";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Text = "Query ";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.Controls.Add(this.dgvJobTrip);
            this.tabPageSearch.Text = "Search ";
            this.tabPageSearch.Controls.SetChildIndex(this.pnlQuery, 0);
            this.tabPageSearch.Controls.SetChildIndex(this.dgvJobTrip, 0);
            // 
            // btnPrint
            // 
            this.btnPrint.Text = "Print ";
            // 
            // pnlQuery
            // 
            this.pnlQuery.Controls.Add(this.grpSubCon);
            this.pnlQuery.Controls.Add(this.dtpEndDate);
            this.pnlQuery.Controls.Add(this.dtpStartDate);
            this.pnlQuery.Controls.Add(this.lblDate);
            this.pnlQuery.Controls.Add(this.cboEndJobID);
            this.pnlQuery.Controls.Add(this.cboStartJobID);
            this.pnlQuery.Controls.Add(this.cboStatus);
            this.pnlQuery.Controls.Add(this.lblJobID);
            this.pnlQuery.Controls.Add(this.lblStatus);
            this.pnlQuery.Size = new System.Drawing.Size(649, 156);
            this.pnlQuery.Controls.SetChildIndex(this.lblStatus, 0);
            this.pnlQuery.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlQuery.Controls.SetChildIndex(this.lblJobID, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboStatus, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboStartJobID, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboEndJobID, 0);
            this.pnlQuery.Controls.SetChildIndex(this.lblDate, 0);
            this.pnlQuery.Controls.SetChildIndex(this.dtpStartDate, 0);
            this.pnlQuery.Controls.SetChildIndex(this.dtpEndDate, 0);
            this.pnlQuery.Controls.SetChildIndex(this.grpSubCon, 0);
            // 
            // tabPageMaster
            // 
            this.tabPageMaster.Text = "Master ";
            // 
            // pnlMaster
            // 
            this.pnlMaster.Controls.Add(this.label7);
            this.pnlMaster.Controls.Add(this.txtEditSubContractorRef);
            this.pnlMaster.Controls.Add(this.label6);
            this.pnlMaster.Controls.Add(this.cboEditSubContractor);
            this.pnlMaster.Controls.Add(this.label5);
            this.pnlMaster.Controls.Add(this.txtEditStatus);
            this.pnlMaster.Controls.Add(this.chkOwnTransport);
            this.pnlMaster.Controls.Add(this.label4);
            this.pnlMaster.Controls.Add(this.txtEditContainerCode);
            this.pnlMaster.Controls.Add(this.label3);
            this.pnlMaster.Controls.Add(this.txtEditContainerNo);
            this.pnlMaster.Controls.Add(this.label2);
            this.pnlMaster.Controls.Add(this.label1);
            this.pnlMaster.Controls.Add(this.txtEditSequence);
            this.pnlMaster.Controls.Add(this.txtEditJobID);
            // 
            // bdsMaster
            // 
            this.bdsMaster.DataSource = typeof(HaulierJobTrip);
            // 
            // btnClose
            // 
            this.btnClose.Text = "Close ";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(44, 21);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status :";
            // 
            // lblJobID
            // 
            this.lblJobID.AutoSize = true;
            this.lblJobID.Location = new System.Drawing.Point(44, 47);
            this.lblJobID.Name = "lblJobID";
            this.lblJobID.Size = new System.Drawing.Size(44, 13);
            this.lblJobID.TabIndex = 5;
            this.lblJobID.Text = "Job ID :";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(93, 18);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(121, 21);
            this.cboStatus.TabIndex = 6;
            // 
            // cboStartJobID
            // 
            this.cboStartJobID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartJobID.FormattingEnabled = true;
            this.cboStartJobID.Location = new System.Drawing.Point(93, 44);
            this.cboStartJobID.Name = "cboStartJobID";
            this.cboStartJobID.Size = new System.Drawing.Size(121, 21);
            this.cboStartJobID.TabIndex = 7;
            this.cboStartJobID.SelectedIndexChanged += new System.EventHandler(this.cboStartJobID_SelectedIndexChanged);
            // 
            // cboEndJobID
            // 
            this.cboEndJobID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEndJobID.FormattingEnabled = true;
            this.cboEndJobID.Location = new System.Drawing.Point(220, 44);
            this.cboEndJobID.Name = "cboEndJobID";
            this.cboEndJobID.Size = new System.Drawing.Size(121, 21);
            this.cboEndJobID.TabIndex = 8;
            this.cboEndJobID.SelectedIndexChanged += new System.EventHandler(this.cboEndJobID_SelectedIndexChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(51, 74);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(36, 13);
            this.lblDate.TabIndex = 9;
            this.lblDate.Text = "Date :";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(93, 70);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(121, 20);
            this.dtpStartDate.TabIndex = 10;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(220, 70);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(121, 20);
            this.dtpEndDate.TabIndex = 11;
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.dtpEndDate_ValueChanged);
            // 
            // grpSubCon
            // 
            this.grpSubCon.Controls.Add(this.rdbBoth);
            this.grpSubCon.Controls.Add(this.rdbNo);
            this.grpSubCon.Controls.Add(this.rdbYes);
            this.grpSubCon.Location = new System.Drawing.Point(24, 96);
            this.grpSubCon.Name = "grpSubCon";
            this.grpSubCon.Size = new System.Drawing.Size(317, 44);
            this.grpSubCon.TabIndex = 13;
            this.grpSubCon.TabStop = false;
            this.grpSubCon.Text = "Own Transport";
            // 
            // rdbBoth
            // 
            this.rdbBoth.AutoSize = true;
            this.rdbBoth.Location = new System.Drawing.Point(196, 19);
            this.rdbBoth.Name = "rdbBoth";
            this.rdbBoth.Size = new System.Drawing.Size(47, 17);
            this.rdbBoth.TabIndex = 2;
            this.rdbBoth.Text = "Both";
            this.rdbBoth.UseVisualStyleBackColor = true;
            // 
            // rdbNo
            // 
            this.rdbNo.AutoSize = true;
            this.rdbNo.Checked = true;
            this.rdbNo.Location = new System.Drawing.Point(136, 19);
            this.rdbNo.Name = "rdbNo";
            this.rdbNo.Size = new System.Drawing.Size(39, 17);
            this.rdbNo.TabIndex = 1;
            this.rdbNo.TabStop = true;
            this.rdbNo.Text = "No";
            this.rdbNo.UseVisualStyleBackColor = true;
            // 
            // rdbYes
            // 
            this.rdbYes.AutoSize = true;
            this.rdbYes.Location = new System.Drawing.Point(69, 19);
            this.rdbYes.Name = "rdbYes";
            this.rdbYes.Size = new System.Drawing.Size(43, 17);
            this.rdbYes.TabIndex = 0;
            this.rdbYes.Text = "Yes";
            this.rdbYes.UseVisualStyleBackColor = true;
            // 
            // dgvJobTrip
            // 
            this.dgvJobTrip.AllowUserToAddRows = false;
            this.dgvJobTrip.AllowUserToDeleteRows = false;
            this.dgvJobTrip.AutoGenerateColumns = false;
            this.dgvJobTrip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJobTrip.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ContainerNoGrid,
            this.maxCBMDataGridViewTextBoxColumn,
            this.maxWeightDataGridViewTextBoxColumn,
            this.cargoDescriptionDataGridViewTextBoxColumn,
            this.containerCodeDataGridViewTextBoxColumn,
            this.isLadenDataGridViewCheckBoxColumn,
            this.grossCBMDataGridViewTextBoxColumn,
            this.grossWeightDataGridViewTextBoxColumn,
            this.sealNoDataGridViewTextBoxColumn,
            this.containerNoDataGridViewTextBoxColumn,
            this.isDangerousGoodsDataGridViewCheckBoxColumn,
            this.dGRemarksDataGridViewTextBoxColumn,
            this.isOversizeDataGridViewCheckBoxColumn,
            this.oversizeRemarksDataGridViewTextBoxColumn,
            this.isDirectDeliveryDataGridViewCheckBoxColumn,
            this.legTypeDataGridViewTextBoxColumn,
            this.partnerLegDataGridViewTextBoxColumn,
            this.jobIDDataGridViewTextBoxColumn,
            this.jobNoDataGridViewTextBoxColumn,
            this.sequenceDataGridViewTextBoxColumn,
            this.tripStatusDataGridViewTextBoxColumn,
            this.endDateDataGridViewTextBoxColumn,
            this.startDateDataGridViewTextBoxColumn,
            this.endTimeDataGridViewTextBoxColumn,
            this.startTimeDataGridViewTextBoxColumn,
            this.endStopDataGridViewTextBoxColumn,
            this.startStopDataGridViewTextBoxColumn,
            this.updateVersionDataGridViewImageColumn,
            this.isNewDataGridViewCheckBoxColumn,
            this.customerCodeDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.ownTransportDataGridViewCheckBoxColumn,
            this.subContractorCodeDataGridViewTextBoxColumn,
            this.subContractorReferenceDataGridViewTextBoxColumn,
            this.chargeCodeDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn});
            this.dgvJobTrip.DataSource = this.bdsMaster;
            this.dgvJobTrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvJobTrip.Location = new System.Drawing.Point(3, 159);
            this.dgvJobTrip.Name = "dgvJobTrip";
            this.dgvJobTrip.ReadOnly = true;
            this.dgvJobTrip.Size = new System.Drawing.Size(649, 200);
            this.dgvJobTrip.TabIndex = 4;
            // 
            // ContainerNoGrid
            // 
            this.ContainerNoGrid.DataPropertyName = "ContainerNo";
            this.ContainerNoGrid.HeaderText = "ContainerNo";
            this.ContainerNoGrid.Name = "ContainerNoGrid";
            this.ContainerNoGrid.ReadOnly = true;
            // 
            // txtEditJobID
            // 
            this.txtEditJobID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditJobID.Location = new System.Drawing.Point(148, 25);
            this.txtEditJobID.Name = "txtEditJobID";
            this.txtEditJobID.ReadOnly = true;
            this.txtEditJobID.Size = new System.Drawing.Size(100, 20);
            this.txtEditJobID.TabIndex = 0;
            // 
            // txtEditSequence
            // 
            this.txtEditSequence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditSequence.Location = new System.Drawing.Point(148, 51);
            this.txtEditSequence.Name = "txtEditSequence";
            this.txtEditSequence.ReadOnly = true;
            this.txtEditSequence.Size = new System.Drawing.Size(100, 20);
            this.txtEditSequence.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "JobID :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sequence :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(67, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Container No :";
            // 
            // txtEditContainerNo
            // 
            this.txtEditContainerNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditContainerNo.Location = new System.Drawing.Point(148, 106);
            this.txtEditContainerNo.Name = "txtEditContainerNo";
            this.txtEditContainerNo.ReadOnly = true;
            this.txtEditContainerNo.Size = new System.Drawing.Size(100, 20);
            this.txtEditContainerNo.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Container Code :";
            // 
            // txtEditContainerCode
            // 
            this.txtEditContainerCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditContainerCode.Location = new System.Drawing.Point(148, 132);
            this.txtEditContainerCode.Name = "txtEditContainerCode";
            this.txtEditContainerCode.ReadOnly = true;
            this.txtEditContainerCode.Size = new System.Drawing.Size(100, 20);
            this.txtEditContainerCode.TabIndex = 6;
            // 
            // chkOwnTransport
            // 
            this.chkOwnTransport.AutoSize = true;
            this.chkOwnTransport.Enabled = false;
            this.chkOwnTransport.Location = new System.Drawing.Point(128, 157);
            this.chkOwnTransport.Name = "chkOwnTransport";
            this.chkOwnTransport.Size = new System.Drawing.Size(96, 17);
            this.chkOwnTransport.TabIndex = 10;
            this.chkOwnTransport.Text = "Own Transport";
            this.chkOwnTransport.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(99, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Status :";
            // 
            // txtEditStatus
            // 
            this.txtEditStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditStatus.Location = new System.Drawing.Point(148, 77);
            this.txtEditStatus.Name = "txtEditStatus";
            this.txtEditStatus.ReadOnly = true;
            this.txtEditStatus.Size = new System.Drawing.Size(100, 20);
            this.txtEditStatus.TabIndex = 11;
            // 
            // cboEditSubContractor
            // 
            this.cboEditSubContractor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEditSubContractor.FormattingEnabled = true;
            this.cboEditSubContractor.Location = new System.Drawing.Point(148, 180);
            this.cboEditSubContractor.Name = "cboEditSubContractor";
            this.cboEditSubContractor.Size = new System.Drawing.Size(121, 21);
            this.cboEditSubContractor.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Sub Contractor :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Sub Contractor Reference :";
            // 
            // txtEditSubContractorRef
            // 
            this.txtEditSubContractorRef.BackColor = System.Drawing.Color.White;
            this.txtEditSubContractorRef.Location = new System.Drawing.Point(148, 207);
            this.txtEditSubContractorRef.Name = "txtEditSubContractorRef";
            this.txtEditSubContractorRef.Size = new System.Drawing.Size(100, 20);
            this.txtEditSubContractorRef.TabIndex = 15;
            // 
            // maxCBMDataGridViewTextBoxColumn
            // 
            this.maxCBMDataGridViewTextBoxColumn.DataPropertyName = "maxCBM";
            this.maxCBMDataGridViewTextBoxColumn.HeaderText = "maxCBM";
            this.maxCBMDataGridViewTextBoxColumn.Name = "maxCBMDataGridViewTextBoxColumn";
            this.maxCBMDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // maxWeightDataGridViewTextBoxColumn
            // 
            this.maxWeightDataGridViewTextBoxColumn.DataPropertyName = "maxWeight";
            this.maxWeightDataGridViewTextBoxColumn.HeaderText = "maxWeight";
            this.maxWeightDataGridViewTextBoxColumn.Name = "maxWeightDataGridViewTextBoxColumn";
            this.maxWeightDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cargoDescriptionDataGridViewTextBoxColumn
            // 
            this.cargoDescriptionDataGridViewTextBoxColumn.DataPropertyName = "CargoDescription";
            this.cargoDescriptionDataGridViewTextBoxColumn.HeaderText = "CargoDescription";
            this.cargoDescriptionDataGridViewTextBoxColumn.Name = "cargoDescriptionDataGridViewTextBoxColumn";
            this.cargoDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // containerCodeDataGridViewTextBoxColumn
            // 
            this.containerCodeDataGridViewTextBoxColumn.DataPropertyName = "ContainerCode";
            this.containerCodeDataGridViewTextBoxColumn.HeaderText = "ContainerCode";
            this.containerCodeDataGridViewTextBoxColumn.Name = "containerCodeDataGridViewTextBoxColumn";
            this.containerCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isLadenDataGridViewCheckBoxColumn
            // 
            this.isLadenDataGridViewCheckBoxColumn.DataPropertyName = "IsLaden";
            this.isLadenDataGridViewCheckBoxColumn.HeaderText = "IsLaden";
            this.isLadenDataGridViewCheckBoxColumn.Name = "isLadenDataGridViewCheckBoxColumn";
            this.isLadenDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // grossCBMDataGridViewTextBoxColumn
            // 
            this.grossCBMDataGridViewTextBoxColumn.DataPropertyName = "GrossCBM";
            this.grossCBMDataGridViewTextBoxColumn.HeaderText = "GrossCBM";
            this.grossCBMDataGridViewTextBoxColumn.Name = "grossCBMDataGridViewTextBoxColumn";
            this.grossCBMDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // grossWeightDataGridViewTextBoxColumn
            // 
            this.grossWeightDataGridViewTextBoxColumn.DataPropertyName = "GrossWeight";
            this.grossWeightDataGridViewTextBoxColumn.HeaderText = "GrossWeight";
            this.grossWeightDataGridViewTextBoxColumn.Name = "grossWeightDataGridViewTextBoxColumn";
            this.grossWeightDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sealNoDataGridViewTextBoxColumn
            // 
            this.sealNoDataGridViewTextBoxColumn.DataPropertyName = "SealNo";
            this.sealNoDataGridViewTextBoxColumn.HeaderText = "SealNo";
            this.sealNoDataGridViewTextBoxColumn.Name = "sealNoDataGridViewTextBoxColumn";
            this.sealNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // containerNoDataGridViewTextBoxColumn
            // 
            this.containerNoDataGridViewTextBoxColumn.DataPropertyName = "ContainerNo";
            this.containerNoDataGridViewTextBoxColumn.HeaderText = "ContainerNo";
            this.containerNoDataGridViewTextBoxColumn.Name = "containerNoDataGridViewTextBoxColumn";
            this.containerNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isDangerousGoodsDataGridViewCheckBoxColumn
            // 
            this.isDangerousGoodsDataGridViewCheckBoxColumn.DataPropertyName = "IsDangerousGoods";
            this.isDangerousGoodsDataGridViewCheckBoxColumn.HeaderText = "IsDangerousGoods";
            this.isDangerousGoodsDataGridViewCheckBoxColumn.Name = "isDangerousGoodsDataGridViewCheckBoxColumn";
            this.isDangerousGoodsDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // dGRemarksDataGridViewTextBoxColumn
            // 
            this.dGRemarksDataGridViewTextBoxColumn.DataPropertyName = "DGRemarks";
            this.dGRemarksDataGridViewTextBoxColumn.HeaderText = "DGRemarks";
            this.dGRemarksDataGridViewTextBoxColumn.Name = "dGRemarksDataGridViewTextBoxColumn";
            this.dGRemarksDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isOversizeDataGridViewCheckBoxColumn
            // 
            this.isOversizeDataGridViewCheckBoxColumn.DataPropertyName = "IsOversize";
            this.isOversizeDataGridViewCheckBoxColumn.HeaderText = "IsOversize";
            this.isOversizeDataGridViewCheckBoxColumn.Name = "isOversizeDataGridViewCheckBoxColumn";
            this.isOversizeDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // oversizeRemarksDataGridViewTextBoxColumn
            // 
            this.oversizeRemarksDataGridViewTextBoxColumn.DataPropertyName = "OversizeRemarks";
            this.oversizeRemarksDataGridViewTextBoxColumn.HeaderText = "OversizeRemarks";
            this.oversizeRemarksDataGridViewTextBoxColumn.Name = "oversizeRemarksDataGridViewTextBoxColumn";
            this.oversizeRemarksDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isDirectDeliveryDataGridViewCheckBoxColumn
            // 
            this.isDirectDeliveryDataGridViewCheckBoxColumn.DataPropertyName = "IsDirectDelivery";
            this.isDirectDeliveryDataGridViewCheckBoxColumn.HeaderText = "IsDirectDelivery";
            this.isDirectDeliveryDataGridViewCheckBoxColumn.Name = "isDirectDeliveryDataGridViewCheckBoxColumn";
            this.isDirectDeliveryDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // legTypeDataGridViewTextBoxColumn
            // 
            this.legTypeDataGridViewTextBoxColumn.DataPropertyName = "LegType";
            this.legTypeDataGridViewTextBoxColumn.HeaderText = "LegType";
            this.legTypeDataGridViewTextBoxColumn.Name = "legTypeDataGridViewTextBoxColumn";
            this.legTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // partnerLegDataGridViewTextBoxColumn
            // 
            this.partnerLegDataGridViewTextBoxColumn.DataPropertyName = "PartnerLeg";
            this.partnerLegDataGridViewTextBoxColumn.HeaderText = "PartnerLeg";
            this.partnerLegDataGridViewTextBoxColumn.Name = "partnerLegDataGridViewTextBoxColumn";
            this.partnerLegDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // jobIDDataGridViewTextBoxColumn
            // 
            this.jobIDDataGridViewTextBoxColumn.DataPropertyName = "JobID";
            this.jobIDDataGridViewTextBoxColumn.HeaderText = "JobID";
            this.jobIDDataGridViewTextBoxColumn.Name = "jobIDDataGridViewTextBoxColumn";
            this.jobIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // jobNoDataGridViewTextBoxColumn
            // 
            this.jobNoDataGridViewTextBoxColumn.DataPropertyName = "JobNo";
            this.jobNoDataGridViewTextBoxColumn.HeaderText = "JobNo";
            this.jobNoDataGridViewTextBoxColumn.Name = "jobNoDataGridViewTextBoxColumn";
            this.jobNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sequenceDataGridViewTextBoxColumn
            // 
            this.sequenceDataGridViewTextBoxColumn.DataPropertyName = "Sequence";
            this.sequenceDataGridViewTextBoxColumn.HeaderText = "Sequence";
            this.sequenceDataGridViewTextBoxColumn.Name = "sequenceDataGridViewTextBoxColumn";
            this.sequenceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tripStatusDataGridViewTextBoxColumn
            // 
            this.tripStatusDataGridViewTextBoxColumn.DataPropertyName = "TripStatus";
            this.tripStatusDataGridViewTextBoxColumn.HeaderText = "TripStatus";
            this.tripStatusDataGridViewTextBoxColumn.Name = "tripStatusDataGridViewTextBoxColumn";
            this.tripStatusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // endDateDataGridViewTextBoxColumn
            // 
            this.endDateDataGridViewTextBoxColumn.DataPropertyName = "EndDate";
            this.endDateDataGridViewTextBoxColumn.HeaderText = "EndDate";
            this.endDateDataGridViewTextBoxColumn.Name = "endDateDataGridViewTextBoxColumn";
            this.endDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // startDateDataGridViewTextBoxColumn
            // 
            this.startDateDataGridViewTextBoxColumn.DataPropertyName = "StartDate";
            this.startDateDataGridViewTextBoxColumn.HeaderText = "StartDate";
            this.startDateDataGridViewTextBoxColumn.Name = "startDateDataGridViewTextBoxColumn";
            this.startDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // endTimeDataGridViewTextBoxColumn
            // 
            this.endTimeDataGridViewTextBoxColumn.DataPropertyName = "EndTime";
            this.endTimeDataGridViewTextBoxColumn.HeaderText = "EndTime";
            this.endTimeDataGridViewTextBoxColumn.Name = "endTimeDataGridViewTextBoxColumn";
            this.endTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // startTimeDataGridViewTextBoxColumn
            // 
            this.startTimeDataGridViewTextBoxColumn.DataPropertyName = "StartTime";
            this.startTimeDataGridViewTextBoxColumn.HeaderText = "StartTime";
            this.startTimeDataGridViewTextBoxColumn.Name = "startTimeDataGridViewTextBoxColumn";
            this.startTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // endStopDataGridViewTextBoxColumn
            // 
            this.endStopDataGridViewTextBoxColumn.DataPropertyName = "EndStop";
            this.endStopDataGridViewTextBoxColumn.HeaderText = "EndStop";
            this.endStopDataGridViewTextBoxColumn.Name = "endStopDataGridViewTextBoxColumn";
            this.endStopDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // startStopDataGridViewTextBoxColumn
            // 
            this.startStopDataGridViewTextBoxColumn.DataPropertyName = "StartStop";
            this.startStopDataGridViewTextBoxColumn.HeaderText = "StartStop";
            this.startStopDataGridViewTextBoxColumn.Name = "startStopDataGridViewTextBoxColumn";
            this.startStopDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // updateVersionDataGridViewImageColumn
            // 
            this.updateVersionDataGridViewImageColumn.DataPropertyName = "UpdateVersion";
            this.updateVersionDataGridViewImageColumn.HeaderText = "UpdateVersion";
            this.updateVersionDataGridViewImageColumn.Name = "updateVersionDataGridViewImageColumn";
            this.updateVersionDataGridViewImageColumn.ReadOnly = true;
            // 
            // isNewDataGridViewCheckBoxColumn
            // 
            this.isNewDataGridViewCheckBoxColumn.DataPropertyName = "IsNew";
            this.isNewDataGridViewCheckBoxColumn.HeaderText = "IsNew";
            this.isNewDataGridViewCheckBoxColumn.Name = "isNewDataGridViewCheckBoxColumn";
            this.isNewDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // customerCodeDataGridViewTextBoxColumn
            // 
            this.customerCodeDataGridViewTextBoxColumn.DataPropertyName = "CustomerCode";
            this.customerCodeDataGridViewTextBoxColumn.HeaderText = "CustomerCode";
            this.customerCodeDataGridViewTextBoxColumn.Name = "customerCodeDataGridViewTextBoxColumn";
            this.customerCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ownTransportDataGridViewCheckBoxColumn
            // 
            this.ownTransportDataGridViewCheckBoxColumn.DataPropertyName = "OwnTransport";
            this.ownTransportDataGridViewCheckBoxColumn.HeaderText = "OwnTransport";
            this.ownTransportDataGridViewCheckBoxColumn.Name = "ownTransportDataGridViewCheckBoxColumn";
            this.ownTransportDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // subContractorCodeDataGridViewTextBoxColumn
            // 
            this.subContractorCodeDataGridViewTextBoxColumn.DataPropertyName = "SubContractorCode";
            this.subContractorCodeDataGridViewTextBoxColumn.HeaderText = "SubContractorCode";
            this.subContractorCodeDataGridViewTextBoxColumn.Name = "subContractorCodeDataGridViewTextBoxColumn";
            this.subContractorCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // subContractorReferenceDataGridViewTextBoxColumn
            // 
            this.subContractorReferenceDataGridViewTextBoxColumn.DataPropertyName = "SubContractorReference";
            this.subContractorReferenceDataGridViewTextBoxColumn.HeaderText = "SubContractorReference";
            this.subContractorReferenceDataGridViewTextBoxColumn.Name = "subContractorReferenceDataGridViewTextBoxColumn";
            this.subContractorReferenceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // chargeCodeDataGridViewTextBoxColumn
            // 
            this.chargeCodeDataGridViewTextBoxColumn.DataPropertyName = "ChargeCode";
            this.chargeCodeDataGridViewTextBoxColumn.HeaderText = "ChargeCode";
            this.chargeCodeDataGridViewTextBoxColumn.Name = "chargeCodeDataGridViewTextBoxColumn";
            this.chargeCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "Remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FrmAssignJobTripToSubcontractor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(663, 482);
            this.Name = "FrmAssignJobTripToSubcontractor";
            this.Text = "Assign Job Trip To Sub Contractor";
            this.Load += new System.EventHandler(this.FrmChangeTripStatus_Load);
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
            this.grpSubCon.ResumeLayout(false);
            this.grpSubCon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobTrip)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblJobID;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.ComboBox cboEndJobID;
        private System.Windows.Forms.ComboBox cboStartJobID;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.GroupBox grpSubCon;
        private System.Windows.Forms.RadioButton rdbBoth;
        private System.Windows.Forms.RadioButton rdbNo;
        private System.Windows.Forms.RadioButton rdbYes;
        private System.Windows.Forms.DataGridView dgvJobTrip;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEditContainerNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEditSequence;
        private System.Windows.Forms.TextBox txtEditJobID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEditContainerCode;
        private System.Windows.Forms.CheckBox chkOwnTransport;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEditStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtEditSubContractorRef;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboEditSubContractor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContainerNoGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxCBMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxWeightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cargoDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContainerCodeGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isLadenDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossCBMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossWeightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sealNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDangerousGoodsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dGRemarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOversizeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oversizeRemarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDirectDeliveryDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn JobIDGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn SequenceGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn TripStatusGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endStopDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startStopDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn UpdateVersion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isNewDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerCodeGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerNameGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn OwnTransportGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubContractorCodeGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn subContractorReferenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn containerCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn containerNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn legTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnerLegDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tripStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn updateVersionDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ownTransportDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subContractorCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chargeCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobNoDataGridViewTextBoxColumn;
    }
}
