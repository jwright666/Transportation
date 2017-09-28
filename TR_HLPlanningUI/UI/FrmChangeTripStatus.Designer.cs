using FM.TR_HLBookDLL.BLL;
namespace FM.TransportPlanning.UI
{
    partial class FrmChangeTripStatus
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
            this.dgvJobTrip = new System.Windows.Forms.DataGridView();
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
            this.txtEditJobID = new System.Windows.Forms.TextBox();
            this.txtEditSequence = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboEditStatus = new System.Windows.Forms.ComboBox();
            this.chkOwnTransport = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblEditContainerNo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEditContainerCode = new System.Windows.Forms.TextBox();
            this.txtEditContainerNo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbDate = new System.Windows.Forms.RadioButton();
            this.rdbJobNo = new System.Windows.Forms.RadioButton();
            this.cboDepartment = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCurrentStatus = new System.Windows.Forms.TextBox();
            this.cboToStatus = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblToStatus = new System.Windows.Forms.Label();
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.permitNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vesselNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vesselNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voyageNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oldPermitNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxCBMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxWeightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isMultilegDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.oldContainerNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oldSealNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oblNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isBillableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bookRefNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceRefNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.legGroupDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.legGroupMemberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partnerJobNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.legTypeCustomizedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portRemarkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yardDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.jobTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subConDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isMovementJobDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobTrip)).BeginInit();
            this.grpSubCon.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Size = new System.Drawing.Size(1046, 44);
            // 
            // btnDeleteMaster
            // 
            this.btnDeleteMaster.Text = "Cancel";
            this.btnDeleteMaster.Visible = false;
            // 
            // btnEditMaster
            // 
            this.btnEditMaster.Text = "Edit ";
            // 
            // btnNewMaster
            // 
            this.btnNewMaster.Text = "Save";
            this.btnNewMaster.Visible = false;
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
            this.tbcEntry.Size = new System.Drawing.Size(1046, 386);
            this.tbcEntry.Click += new System.EventHandler(this.tbcEntry_Click);
            this.tbcEntry.DoubleClick += new System.EventHandler(this.tbcEntry_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(465, 104);
            this.btnQuery.Text = "Query ";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.Controls.Add(this.dgvJobTrip);
            this.tabPageSearch.Controls.Add(this.panel1);
            this.tabPageSearch.Size = new System.Drawing.Size(1038, 357);
            this.tabPageSearch.Text = "Search ";
            this.tabPageSearch.Controls.SetChildIndex(this.pnlQuery, 0);
            this.tabPageSearch.Controls.SetChildIndex(this.panel1, 0);
            this.tabPageSearch.Controls.SetChildIndex(this.dgvJobTrip, 0);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Location = new System.Drawing.Point(0, 430);
            this.pnlBottom.Size = new System.Drawing.Size(1046, 52);
            // 
            // btnPrint
            // 
            this.btnPrint.Text = "Print ";
            this.btnPrint.Visible = false;
            // 
            // pnlQuery
            // 
            this.pnlQuery.Controls.Add(this.lblToStatus);
            this.pnlQuery.Controls.Add(this.cboToStatus);
            this.pnlQuery.Controls.Add(this.cboDepartment);
            this.pnlQuery.Controls.Add(this.label3);
            this.pnlQuery.Controls.Add(this.groupBox1);
            this.pnlQuery.Controls.Add(this.grpSubCon);
            this.pnlQuery.Controls.Add(this.cboStatus);
            this.pnlQuery.Controls.Add(this.lblStatus);
            this.pnlQuery.Size = new System.Drawing.Size(1032, 132);
            this.pnlQuery.Controls.SetChildIndex(this.lblStatus, 0);
            this.pnlQuery.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboStatus, 0);
            this.pnlQuery.Controls.SetChildIndex(this.grpSubCon, 0);
            this.pnlQuery.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlQuery.Controls.SetChildIndex(this.label3, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboDepartment, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboToStatus, 0);
            this.pnlQuery.Controls.SetChildIndex(this.lblToStatus, 0);
            // 
            // tabPageMaster
            // 
            this.tabPageMaster.Size = new System.Drawing.Size(1038, 357);
            this.tabPageMaster.Text = "Master ";
            // 
            // pnlMaster
            // 
            this.pnlMaster.Controls.Add(this.label7);
            this.pnlMaster.Controls.Add(this.txtCurrentStatus);
            this.pnlMaster.Controls.Add(this.txtEditContainerNo);
            this.pnlMaster.Controls.Add(this.label4);
            this.pnlMaster.Controls.Add(this.txtEditContainerCode);
            this.pnlMaster.Controls.Add(this.lblEditContainerNo);
            this.pnlMaster.Controls.Add(this.txtRemarks);
            this.pnlMaster.Controls.Add(this.label6);
            this.pnlMaster.Controls.Add(this.chkOwnTransport);
            this.pnlMaster.Controls.Add(this.cboEditStatus);
            this.pnlMaster.Controls.Add(this.label5);
            this.pnlMaster.Controls.Add(this.label2);
            this.pnlMaster.Controls.Add(this.label1);
            this.pnlMaster.Controls.Add(this.txtEditSequence);
            this.pnlMaster.Controls.Add(this.txtEditJobID);
            this.pnlMaster.Size = new System.Drawing.Size(1032, 351);
            // 
            // bdsMaster
            // 
            this.bdsMaster.DataSource = typeof(HaulierJobTrip);
            this.bdsMaster.CurrentChanged += new System.EventHandler(this.bdsMaster_CurrentChanged);
            // 
            // btnClose
            // 
            this.btnClose.Text = "Close ";
            // 
            // dgvJobTrip
            // 
            this.dgvJobTrip.AllowUserToAddRows = false;
            this.dgvJobTrip.AllowUserToDeleteRows = false;
            this.dgvJobTrip.AllowUserToOrderColumns = true;
            this.dgvJobTrip.AutoGenerateColumns = false;
            this.dgvJobTrip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJobTrip.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.permitNoDataGridViewTextBoxColumn,
            this.vesselNoDataGridViewTextBoxColumn,
            this.vesselNameDataGridViewTextBoxColumn,
            this.voyageNoDataGridViewTextBoxColumn,
            this.oldPermitNoDataGridViewTextBoxColumn,
            this.maxCBMDataGridViewTextBoxColumn,
            this.maxWeightDataGridViewTextBoxColumn,
            this.isMultilegDataGridViewCheckBoxColumn,
            this.oldContainerNoDataGridViewTextBoxColumn,
            this.oldSealNoDataGridViewTextBoxColumn,
            this.oblNoDataGridViewTextBoxColumn,
            this.isBillableDataGridViewCheckBoxColumn,
            this.bookRefNoDataGridViewTextBoxColumn,
            this.sourceRefNoDataGridViewTextBoxColumn,
            this.legGroupDataGridViewTextBoxColumn,
            this.legGroupMemberDataGridViewTextBoxColumn,
            this.partnerJobNoDataGridViewTextBoxColumn,
            this.legTypeCustomizedDataGridViewTextBoxColumn,
            this.portRemarkDataGridViewTextBoxColumn,
            this.yardDataGridViewTextBoxColumn,
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
            this.jobTypeDataGridViewTextBoxColumn,
            this.subConDataGridViewTextBoxColumn,
            this.isMovementJobDataGridViewCheckBoxColumn,
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
            this.subContractorReferenceDataGridViewTextBoxColumn,
            this.chargeCodeDataGridViewTextBoxColumn,
            this.remarksDataGridViewTextBoxColumn});
            this.dgvJobTrip.DataSource = this.bdsMaster;
            this.dgvJobTrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvJobTrip.Location = new System.Drawing.Point(3, 135);
            this.dgvJobTrip.Name = "dgvJobTrip";
            this.dgvJobTrip.ReadOnly = true;
            this.dgvJobTrip.Size = new System.Drawing.Size(1032, 184);
            this.dgvJobTrip.TabIndex = 4;
            this.dgvJobTrip.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJobTrip_CellClick);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(44, 11);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(43, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status :";
            // 
            // lblJobID
            // 
            this.lblJobID.AutoSize = true;
            this.lblJobID.Location = new System.Drawing.Point(109, 44);
            this.lblJobID.Name = "lblJobID";
            this.lblJobID.Size = new System.Drawing.Size(47, 13);
            this.lblJobID.TabIndex = 5;
            this.lblJobID.Text = "Job No :";
            this.lblJobID.Visible = false;
            // 
            // cboStatus
            // 
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Booked",
            "Ready",
            "Assigned",
            "Completed",
            "Invoiced"});
            this.cboStatus.Location = new System.Drawing.Point(93, 8);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(142, 21);
            this.cboStatus.TabIndex = 6;
            this.cboStatus.TextChanged += new System.EventHandler(this.cboStatus_TextChanged);
            this.cboStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCBO_KeyPress);
            // 
            // cboStartJobID
            // 
            this.cboStartJobID.FormattingEnabled = true;
            this.cboStartJobID.Location = new System.Drawing.Point(162, 41);
            this.cboStartJobID.Name = "cboStartJobID";
            this.cboStartJobID.Size = new System.Drawing.Size(121, 21);
            this.cboStartJobID.TabIndex = 7;
            this.cboStartJobID.Visible = false;
            this.cboStartJobID.SelectedIndexChanged += new System.EventHandler(this.cboStartJobID_SelectedIndexChanged);
            this.cboStartJobID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCBO_KeyPress);
            // 
            // cboEndJobID
            // 
            this.cboEndJobID.FormattingEnabled = true;
            this.cboEndJobID.Location = new System.Drawing.Point(289, 41);
            this.cboEndJobID.Name = "cboEndJobID";
            this.cboEndJobID.Size = new System.Drawing.Size(121, 21);
            this.cboEndJobID.TabIndex = 8;
            this.cboEndJobID.Visible = false;
            this.cboEndJobID.SelectedIndexChanged += new System.EventHandler(this.cboEndJobID_SelectedIndexChanged);
            this.cboEndJobID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCBO_KeyPress);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(120, 21);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(36, 13);
            this.lblDate.TabIndex = 9;
            this.lblDate.Text = "Date :";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(162, 17);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(121, 20);
            this.dtpStartDate.TabIndex = 10;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(289, 17);
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
            this.grpSubCon.Location = new System.Drawing.Point(241, 8);
            this.grpSubCon.Name = "grpSubCon";
            this.grpSubCon.Size = new System.Drawing.Size(194, 44);
            this.grpSubCon.TabIndex = 13;
            this.grpSubCon.TabStop = false;
            this.grpSubCon.Text = "Own Transport";
            // 
            // rdbBoth
            // 
            this.rdbBoth.AutoSize = true;
            this.rdbBoth.Location = new System.Drawing.Point(137, 19);
            this.rdbBoth.Name = "rdbBoth";
            this.rdbBoth.Size = new System.Drawing.Size(47, 17);
            this.rdbBoth.TabIndex = 2;
            this.rdbBoth.Text = "Both";
            this.rdbBoth.UseVisualStyleBackColor = true;
            // 
            // rdbNo
            // 
            this.rdbNo.AutoSize = true;
            this.rdbNo.Location = new System.Drawing.Point(77, 19);
            this.rdbNo.Name = "rdbNo";
            this.rdbNo.Size = new System.Drawing.Size(39, 17);
            this.rdbNo.TabIndex = 1;
            this.rdbNo.Text = "No";
            this.rdbNo.UseVisualStyleBackColor = true;
            this.rdbNo.CheckedChanged += new System.EventHandler(this.rdbOwnTransportOption_CheckedChanged);
            // 
            // rdbYes
            // 
            this.rdbYes.AutoSize = true;
            this.rdbYes.Checked = true;
            this.rdbYes.Location = new System.Drawing.Point(10, 19);
            this.rdbYes.Name = "rdbYes";
            this.rdbYes.Size = new System.Drawing.Size(43, 17);
            this.rdbYes.TabIndex = 0;
            this.rdbYes.TabStop = true;
            this.rdbYes.Text = "Yes";
            this.rdbYes.UseVisualStyleBackColor = true;
            // 
            // txtEditJobID
            // 
            this.txtEditJobID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditJobID.Location = new System.Drawing.Point(114, 12);
            this.txtEditJobID.Name = "txtEditJobID";
            this.txtEditJobID.ReadOnly = true;
            this.txtEditJobID.Size = new System.Drawing.Size(121, 20);
            this.txtEditJobID.TabIndex = 0;
            // 
            // txtEditSequence
            // 
            this.txtEditSequence.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditSequence.Location = new System.Drawing.Point(114, 35);
            this.txtEditSequence.Name = "txtEditSequence";
            this.txtEditSequence.ReadOnly = true;
            this.txtEditSequence.Size = new System.Drawing.Size(121, 20);
            this.txtEditSequence.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Job No :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sequence :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "New Status :";
            // 
            // cboEditStatus
            // 
            this.cboEditStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEditStatus.FormattingEnabled = true;
            this.cboEditStatus.Location = new System.Drawing.Point(114, 148);
            this.cboEditStatus.Name = "cboEditStatus";
            this.cboEditStatus.Size = new System.Drawing.Size(121, 21);
            this.cboEditStatus.TabIndex = 9;
            // 
            // chkOwnTransport
            // 
            this.chkOwnTransport.AutoSize = true;
            this.chkOwnTransport.Enabled = false;
            this.chkOwnTransport.Location = new System.Drawing.Point(93, 125);
            this.chkOwnTransport.Name = "chkOwnTransport";
            this.chkOwnTransport.Size = new System.Drawing.Size(96, 17);
            this.chkOwnTransport.TabIndex = 10;
            this.chkOwnTransport.Text = "Own Transport";
            this.chkOwnTransport.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(53, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Remarks :";
            // 
            // txtRemarks
            // 
            this.txtRemarks.BackColor = System.Drawing.Color.White;
            this.txtRemarks.Location = new System.Drawing.Point(114, 172);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(162, 80);
            this.txtRemarks.TabIndex = 12;
            // 
            // lblEditContainerNo
            // 
            this.lblEditContainerNo.AutoSize = true;
            this.lblEditContainerNo.Location = new System.Drawing.Point(33, 61);
            this.lblEditContainerNo.Name = "lblEditContainerNo";
            this.lblEditContainerNo.Size = new System.Drawing.Size(75, 13);
            this.lblEditContainerNo.TabIndex = 14;
            this.lblEditContainerNo.Text = "Container No :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Container Code :";
            // 
            // txtEditContainerCode
            // 
            this.txtEditContainerCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditContainerCode.Location = new System.Drawing.Point(114, 80);
            this.txtEditContainerCode.Name = "txtEditContainerCode";
            this.txtEditContainerCode.ReadOnly = true;
            this.txtEditContainerCode.Size = new System.Drawing.Size(121, 20);
            this.txtEditContainerCode.TabIndex = 15;
            // 
            // txtEditContainerNo
            // 
            this.txtEditContainerNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEditContainerNo.Location = new System.Drawing.Point(114, 58);
            this.txtEditContainerNo.Name = "txtEditContainerNo";
            this.txtEditContainerNo.ReadOnly = true;
            this.txtEditContainerNo.Size = new System.Drawing.Size(121, 20);
            this.txtEditContainerNo.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbDate);
            this.groupBox1.Controls.Add(this.rdbJobNo);
            this.groupBox1.Controls.Add(this.cboEndJobID);
            this.groupBox1.Controls.Add(this.lblJobID);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.cboStartJobID);
            this.groupBox1.Controls.Add(this.lblDate);
            this.groupBox1.Location = new System.Drawing.Point(16, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 70);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // rdbDate
            // 
            this.rdbDate.AutoSize = true;
            this.rdbDate.Checked = true;
            this.rdbDate.Location = new System.Drawing.Point(23, 19);
            this.rdbDate.Name = "rdbDate";
            this.rdbDate.Size = new System.Drawing.Size(62, 17);
            this.rdbDate.TabIndex = 13;
            this.rdbDate.TabStop = true;
            this.rdbDate.Text = "by Date";
            this.rdbDate.UseVisualStyleBackColor = true;
            this.rdbDate.CheckedChanged += new System.EventHandler(this.rdbDate_CheckedChanged);
            // 
            // rdbJobNo
            // 
            this.rdbJobNo.AutoSize = true;
            this.rdbJobNo.Location = new System.Drawing.Point(23, 42);
            this.rdbJobNo.Name = "rdbJobNo";
            this.rdbJobNo.Size = new System.Drawing.Size(73, 17);
            this.rdbJobNo.TabIndex = 12;
            this.rdbJobNo.Text = "by Job No";
            this.rdbJobNo.UseVisualStyleBackColor = true;
            this.rdbJobNo.CheckedChanged += new System.EventHandler(this.rdbJobNo_CheckedChanged);
            // 
            // cboDepartment
            // 
            this.cboDepartment.FormattingEnabled = true;
            this.cboDepartment.Items.AddRange(new object[] {
            "",
            "Booked",
            "Ready",
            "Assigned",
            "Completed",
            "Invoiced"});
            this.cboDepartment.Location = new System.Drawing.Point(93, 35);
            this.cboDepartment.Name = "cboDepartment";
            this.cboDepartment.Size = new System.Drawing.Size(142, 21);
            this.cboDepartment.TabIndex = 16;
            this.cboDepartment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCBO_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Department :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Current Status :";
            // 
            // txtCurrentStatus
            // 
            this.txtCurrentStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtCurrentStatus.Location = new System.Drawing.Point(114, 102);
            this.txtCurrentStatus.Name = "txtCurrentStatus";
            this.txtCurrentStatus.ReadOnly = true;
            this.txtCurrentStatus.Size = new System.Drawing.Size(121, 20);
            this.txtCurrentStatus.TabIndex = 18;
            // 
            // cboToStatus
            // 
            this.cboToStatus.FormattingEnabled = true;
            this.cboToStatus.Items.AddRange(new object[] {
            "Ready",
            "Assigned",
            "Completed"});
            this.cboToStatus.Location = new System.Drawing.Point(465, 30);
            this.cboToStatus.Name = "cboToStatus";
            this.cboToStatus.Size = new System.Drawing.Size(121, 21);
            this.cboToStatus.TabIndex = 17;
            this.cboToStatus.Visible = false;
            this.cboToStatus.TextChanged += new System.EventHandler(this.cboToStatus_TextChanged);
            this.cboToStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCBO_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUnSelectAll);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 319);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1032, 35);
            this.panel1.TabIndex = 5;
            // 
            // btnUnSelectAll
            // 
            this.btnUnSelectAll.Location = new System.Drawing.Point(97, 9);
            this.btnUnSelectAll.Name = "btnUnSelectAll";
            this.btnUnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnUnSelectAll.TabIndex = 1;
            this.btnUnSelectAll.Text = "UnSelect_All";
            this.btnUnSelectAll.UseVisualStyleBackColor = true;
            this.btnUnSelectAll.Click += new System.EventHandler(this.btnUnSelectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(12, 9);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "Select_All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lblToStatus
            // 
            this.lblToStatus.AutoSize = true;
            this.lblToStatus.Location = new System.Drawing.Point(462, 12);
            this.lblToStatus.Name = "lblToStatus";
            this.lblToStatus.Size = new System.Drawing.Size(155, 13);
            this.lblToStatus.TabIndex = 18;
            this.lblToStatus.Text = "Change SubConTrip Status To:";
            this.lblToStatus.Visible = false;
            // 
            // colSelected
            // 
            this.colSelected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colSelected.FalseValue = "F";
            this.colSelected.HeaderText = "Selected";
            this.colSelected.IndeterminateValue = "F";
            this.colSelected.Name = "colSelected";
            this.colSelected.ReadOnly = true;
            this.colSelected.TrueValue = "T";
            this.colSelected.Width = 55;
            // 
            // permitNoDataGridViewTextBoxColumn
            // 
            this.permitNoDataGridViewTextBoxColumn.DataPropertyName = "permitNo";
            this.permitNoDataGridViewTextBoxColumn.HeaderText = "permitNo";
            this.permitNoDataGridViewTextBoxColumn.Name = "permitNoDataGridViewTextBoxColumn";
            this.permitNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // vesselNoDataGridViewTextBoxColumn
            // 
            this.vesselNoDataGridViewTextBoxColumn.DataPropertyName = "vesselNo";
            this.vesselNoDataGridViewTextBoxColumn.HeaderText = "vesselNo";
            this.vesselNoDataGridViewTextBoxColumn.Name = "vesselNoDataGridViewTextBoxColumn";
            this.vesselNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // vesselNameDataGridViewTextBoxColumn
            // 
            this.vesselNameDataGridViewTextBoxColumn.DataPropertyName = "vesselName";
            this.vesselNameDataGridViewTextBoxColumn.HeaderText = "vesselName";
            this.vesselNameDataGridViewTextBoxColumn.Name = "vesselNameDataGridViewTextBoxColumn";
            this.vesselNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // voyageNoDataGridViewTextBoxColumn
            // 
            this.voyageNoDataGridViewTextBoxColumn.DataPropertyName = "voyageNo";
            this.voyageNoDataGridViewTextBoxColumn.HeaderText = "voyageNo";
            this.voyageNoDataGridViewTextBoxColumn.Name = "voyageNoDataGridViewTextBoxColumn";
            this.voyageNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oldPermitNoDataGridViewTextBoxColumn
            // 
            this.oldPermitNoDataGridViewTextBoxColumn.DataPropertyName = "oldPermitNo";
            this.oldPermitNoDataGridViewTextBoxColumn.HeaderText = "oldPermitNo";
            this.oldPermitNoDataGridViewTextBoxColumn.Name = "oldPermitNoDataGridViewTextBoxColumn";
            this.oldPermitNoDataGridViewTextBoxColumn.ReadOnly = true;
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
            // isMultilegDataGridViewCheckBoxColumn
            // 
            this.isMultilegDataGridViewCheckBoxColumn.DataPropertyName = "isMulti_leg";
            this.isMultilegDataGridViewCheckBoxColumn.HeaderText = "isMulti_leg";
            this.isMultilegDataGridViewCheckBoxColumn.Name = "isMultilegDataGridViewCheckBoxColumn";
            this.isMultilegDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // oldContainerNoDataGridViewTextBoxColumn
            // 
            this.oldContainerNoDataGridViewTextBoxColumn.DataPropertyName = "oldContainerNo";
            this.oldContainerNoDataGridViewTextBoxColumn.HeaderText = "oldContainerNo";
            this.oldContainerNoDataGridViewTextBoxColumn.Name = "oldContainerNoDataGridViewTextBoxColumn";
            this.oldContainerNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oldSealNoDataGridViewTextBoxColumn
            // 
            this.oldSealNoDataGridViewTextBoxColumn.DataPropertyName = "oldSealNo";
            this.oldSealNoDataGridViewTextBoxColumn.HeaderText = "oldSealNo";
            this.oldSealNoDataGridViewTextBoxColumn.Name = "oldSealNoDataGridViewTextBoxColumn";
            this.oldSealNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // oblNoDataGridViewTextBoxColumn
            // 
            this.oblNoDataGridViewTextBoxColumn.DataPropertyName = "oblNo";
            this.oblNoDataGridViewTextBoxColumn.HeaderText = "oblNo";
            this.oblNoDataGridViewTextBoxColumn.Name = "oblNoDataGridViewTextBoxColumn";
            this.oblNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isBillableDataGridViewCheckBoxColumn
            // 
            this.isBillableDataGridViewCheckBoxColumn.DataPropertyName = "isBillable";
            this.isBillableDataGridViewCheckBoxColumn.HeaderText = "isBillable";
            this.isBillableDataGridViewCheckBoxColumn.Name = "isBillableDataGridViewCheckBoxColumn";
            this.isBillableDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // bookRefNoDataGridViewTextBoxColumn
            // 
            this.bookRefNoDataGridViewTextBoxColumn.DataPropertyName = "BookRefNo";
            this.bookRefNoDataGridViewTextBoxColumn.HeaderText = "BookRefNo";
            this.bookRefNoDataGridViewTextBoxColumn.Name = "bookRefNoDataGridViewTextBoxColumn";
            this.bookRefNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sourceRefNoDataGridViewTextBoxColumn
            // 
            this.sourceRefNoDataGridViewTextBoxColumn.DataPropertyName = "SourceRefNo";
            this.sourceRefNoDataGridViewTextBoxColumn.HeaderText = "SourceRefNo";
            this.sourceRefNoDataGridViewTextBoxColumn.Name = "sourceRefNoDataGridViewTextBoxColumn";
            this.sourceRefNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // legGroupDataGridViewTextBoxColumn
            // 
            this.legGroupDataGridViewTextBoxColumn.DataPropertyName = "LegGroup";
            this.legGroupDataGridViewTextBoxColumn.HeaderText = "LegGroup";
            this.legGroupDataGridViewTextBoxColumn.Name = "legGroupDataGridViewTextBoxColumn";
            this.legGroupDataGridViewTextBoxColumn.ReadOnly = true;
            this.legGroupDataGridViewTextBoxColumn.Width = 50;
            // 
            // legGroupMemberDataGridViewTextBoxColumn
            // 
            this.legGroupMemberDataGridViewTextBoxColumn.DataPropertyName = "LegGroupMember";
            this.legGroupMemberDataGridViewTextBoxColumn.HeaderText = "LegGroupMember";
            this.legGroupMemberDataGridViewTextBoxColumn.Name = "legGroupMemberDataGridViewTextBoxColumn";
            this.legGroupMemberDataGridViewTextBoxColumn.ReadOnly = true;
            this.legGroupMemberDataGridViewTextBoxColumn.Width = 50;
            // 
            // partnerJobNoDataGridViewTextBoxColumn
            // 
            this.partnerJobNoDataGridViewTextBoxColumn.DataPropertyName = "PartnerJobNo";
            this.partnerJobNoDataGridViewTextBoxColumn.HeaderText = "PartnerJobNo";
            this.partnerJobNoDataGridViewTextBoxColumn.Name = "partnerJobNoDataGridViewTextBoxColumn";
            this.partnerJobNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // legTypeCustomizedDataGridViewTextBoxColumn
            // 
            this.legTypeCustomizedDataGridViewTextBoxColumn.DataPropertyName = "LegTypeCustomized";
            this.legTypeCustomizedDataGridViewTextBoxColumn.HeaderText = "Leg";
            this.legTypeCustomizedDataGridViewTextBoxColumn.Name = "legTypeCustomizedDataGridViewTextBoxColumn";
            this.legTypeCustomizedDataGridViewTextBoxColumn.ReadOnly = true;
            this.legTypeCustomizedDataGridViewTextBoxColumn.Width = 50;
            // 
            // portRemarkDataGridViewTextBoxColumn
            // 
            this.portRemarkDataGridViewTextBoxColumn.DataPropertyName = "PortRemark";
            this.portRemarkDataGridViewTextBoxColumn.HeaderText = "PortRemark";
            this.portRemarkDataGridViewTextBoxColumn.Name = "portRemarkDataGridViewTextBoxColumn";
            this.portRemarkDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // yardDataGridViewTextBoxColumn
            // 
            this.yardDataGridViewTextBoxColumn.DataPropertyName = "Yard";
            this.yardDataGridViewTextBoxColumn.HeaderText = "Yard";
            this.yardDataGridViewTextBoxColumn.Name = "yardDataGridViewTextBoxColumn";
            this.yardDataGridViewTextBoxColumn.ReadOnly = true;
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
            // jobTypeDataGridViewTextBoxColumn
            // 
            this.jobTypeDataGridViewTextBoxColumn.DataPropertyName = "jobType";
            this.jobTypeDataGridViewTextBoxColumn.HeaderText = "jobType";
            this.jobTypeDataGridViewTextBoxColumn.Name = "jobTypeDataGridViewTextBoxColumn";
            this.jobTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // subConDataGridViewTextBoxColumn
            // 
            this.subConDataGridViewTextBoxColumn.DataPropertyName = "subCon";
            this.subConDataGridViewTextBoxColumn.HeaderText = "subCon";
            this.subConDataGridViewTextBoxColumn.Name = "subConDataGridViewTextBoxColumn";
            this.subConDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isMovementJobDataGridViewCheckBoxColumn
            // 
            this.isMovementJobDataGridViewCheckBoxColumn.DataPropertyName = "isMovementJob";
            this.isMovementJobDataGridViewCheckBoxColumn.HeaderText = "isMovementJob";
            this.isMovementJobDataGridViewCheckBoxColumn.Name = "isMovementJobDataGridViewCheckBoxColumn";
            this.isMovementJobDataGridViewCheckBoxColumn.ReadOnly = true;
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
            // FrmChangeTripStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1046, 482);
            this.Name = "FrmChangeTripStatus";
            this.Text = "Change Trip Status for Individual Job Trips";
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobTrip)).EndInit();
            this.grpSubCon.ResumeLayout(false);
            this.grpSubCon.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEditSequence;
        private System.Windows.Forms.TextBox txtEditJobID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboEditStatus;
        private System.Windows.Forms.CheckBox chkOwnTransport;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblEditContainerNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEditContainerCode;
        private System.Windows.Forms.TextBox txtEditContainerNo;
        private System.Windows.Forms.DataGridView dgvJobTrip;
        private System.Windows.Forms.ComboBox cboDepartment;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbDate;
        private System.Windows.Forms.RadioButton rdbJobNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCurrentStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLegGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn collegGroupMember;
        private System.Windows.Forms.ComboBox cboToStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUnSelectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label lblToStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn permitNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vesselNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vesselNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn voyageNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldPermitNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxCBMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxWeightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMultilegDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldContainerNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldSealNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oblNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isBillableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookRefNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceRefNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn legGroupDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn legGroupMemberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnerJobNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn legTypeCustomizedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn portRemarkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yardDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cargoDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn containerCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isLadenDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossCBMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossWeightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sealNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn containerNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDangerousGoodsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dGRemarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isOversizeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oversizeRemarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isDirectDeliveryDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn legTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnerLegDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subConDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMovementJobDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tripStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endStopDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startStopDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn updateVersionDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isNewDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ownTransportDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subContractorReferenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chargeCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
    }
}
