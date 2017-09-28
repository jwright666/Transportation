namespace FM.TR_MaintenanceUI.UI
{
    partial class FrmDriver
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblDriverCode = new System.Windows.Forms.Label();
            this.dtgrdSeach = new System.Windows.Forms.DataGridView();
            this.cboDriverCode = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.gbxUnavailableDates = new System.Windows.Forms.GroupBox();
            this.dgvUnavailableDates = new System.Windows.Forms.DataGridView();
            this.driverCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seqNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remarksDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bdsUnavailableDates = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteUnavailableDate = new System.Windows.Forms.Button();
            this.btnEditUnvailableDate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUnavailableDateQuery = new System.Windows.Forms.Button();
            this.dtpUnavailableEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpUnavailableEndDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDateTimeTo = new System.Windows.Forms.Label();
            this.dtpUnavailableStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtpUnavailableStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDateTimeFrom = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.cboDefaultVehicle = new System.Windows.Forms.ComboBox();
            this.lblNIRC = new System.Windows.Forms.Label();
            this.cboEmployeeStatus = new System.Windows.Forms.ComboBox();
            this.lblNationality = new System.Windows.Forms.Label();
            this.dtpLicenceExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.lblDrivingLicence = new System.Windows.Forms.Label();
            this.txtDrivingClass = new System.Windows.Forms.TextBox();
            this.lblDrivingClass = new System.Windows.Forms.Label();
            this.txtDrivingLicence = new System.Windows.Forms.TextBox();
            this.lblLicenceExpiryDate = new System.Windows.Forms.Label();
            this.txtNationality = new System.Windows.Forms.TextBox();
            this.lblDefaultVehicle = new System.Windows.Forms.Label();
            this.txtNIRC = new System.Windows.Forms.TextBox();
            this.lblEmployStatus = new System.Windows.Forms.Label();
            this.txtDriverName = new System.Windows.Forms.TextBox();
            this.txtDriverCode = new System.Windows.Forms.TextBox();
            this.lblWarnMsg = new System.Windows.Forms.Label();
            this.tabPageJobAssignmentByDriver = new System.Windows.Forms.TabPage();
            this.dgvAssignmentsByDriver = new System.Windows.Forms.DataGridView();
            this.driverCodeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefaultVehicle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prior_Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bdsAssignmentByDriver = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddAssignments = new System.Windows.Forms.Button();
            this.dtpAssignmentFrom = new System.Windows.Forms.DateTimePicker();
            this.btnGetJobAssignments = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpAssignmentTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPageAssignmentSummary = new System.Windows.Forms.TabPage();
            this.dgvAssignmentSummary = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bdsAssignmentSummary = new System.Windows.Forms.BindingSource(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtpJobSummaryDateFrom = new System.Windows.Forms.DateTimePicker();
            this.btnAssignmentSummary = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpJobSummaryDateTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPriorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nircDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nationalityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.licenceExpiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defaultVehicleNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isAvailableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.descriptionForPlanningPurposeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unAvailableDisplayTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.drivingLicenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.drivingClassDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.employeeStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4.SuspendLayout();
            this.tbcEntry.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlQuery.SuspendLayout();
            this.tabPageMaster.SuspendLayout();
            this.pnlMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdSeach)).BeginInit();
            this.gbxUnavailableDates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnavailableDates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsUnavailableDates)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPageJobAssignmentByDriver.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignmentsByDriver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsAssignmentByDriver)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabPageAssignmentSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignmentSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsAssignmentSummary)).BeginInit();
            this.panel3.SuspendLayout();
            this.dgvContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblWarnMsg);
            this.groupBox4.Size = new System.Drawing.Size(837, 44);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.Controls.SetChildIndex(this.lblWarnMsg, 0);
            this.groupBox4.Controls.SetChildIndex(this.btnNewMaster, 0);
            this.groupBox4.Controls.SetChildIndex(this.btnEditMaster, 0);
            this.groupBox4.Controls.SetChildIndex(this.btnDeleteMaster, 0);
            // 
            // btnDeleteMaster
            // 
            this.btnDeleteMaster.TabIndex = 2;
            this.btnDeleteMaster.Text = "Delete ";
            // 
            // btnEditMaster
            // 
            this.btnEditMaster.TabIndex = 1;
            this.btnEditMaster.Text = "Edit ";
            // 
            // btnNewMaster
            // 
            this.btnNewMaster.TabIndex = 0;
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
            this.tbcEntry.Controls.Add(this.tabPageJobAssignmentByDriver);
            this.tbcEntry.Controls.Add(this.tabPageAssignmentSummary);
            this.tbcEntry.Size = new System.Drawing.Size(837, 440);
            this.tbcEntry.TabIndex = 0;
            this.tbcEntry.Click += new System.EventHandler(this.tbcEntry_Click);
            this.tbcEntry.DoubleClick += new System.EventHandler(this.tbcEntry_Click);
            this.tbcEntry.Controls.SetChildIndex(this.tabPageAssignmentSummary, 0);
            this.tbcEntry.Controls.SetChildIndex(this.tabPageJobAssignmentByDriver, 0);
            this.tbcEntry.Controls.SetChildIndex(this.tabPageMaster, 0);
            this.tbcEntry.Controls.SetChildIndex(this.tabPageSearch, 0);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(242, 6);
            this.btnQuery.Text = "Query ";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.Controls.Add(this.dtgrdSeach);
            this.tabPageSearch.Text = "Search ";
            this.tabPageSearch.Controls.SetChildIndex(this.pnlQuery, 0);
            this.tabPageSearch.Controls.SetChildIndex(this.dtgrdSeach, 0);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Location = new System.Drawing.Point(0, 484);
            this.pnlBottom.Size = new System.Drawing.Size(837, 47);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnPrint
            // 
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "Print ";
            this.btnPrint.Visible = false;
            // 
            // pnlQuery
            // 
            this.pnlQuery.Controls.Add(this.btnClear);
            this.pnlQuery.Controls.Add(this.cboDriverCode);
            this.pnlQuery.Controls.Add(this.lblDriverCode);
            this.pnlQuery.Size = new System.Drawing.Size(649, 36);
            this.pnlQuery.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlQuery.Controls.SetChildIndex(this.lblDriverCode, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboDriverCode, 0);
            this.pnlQuery.Controls.SetChildIndex(this.btnClear, 0);
            // 
            // tabPageMaster
            // 
            this.tabPageMaster.Controls.Add(this.gbxUnavailableDates);
            this.tabPageMaster.Size = new System.Drawing.Size(829, 411);
            this.tabPageMaster.Text = "Master ";
            this.tabPageMaster.Controls.SetChildIndex(this.pnlMaster, 0);
            this.tabPageMaster.Controls.SetChildIndex(this.gbxUnavailableDates, 0);
            // 
            // pnlMaster
            // 
            this.pnlMaster.Controls.Add(this.lblCode);
            this.pnlMaster.Controls.Add(this.lblName);
            this.pnlMaster.Controls.Add(this.cboDefaultVehicle);
            this.pnlMaster.Controls.Add(this.lblNIRC);
            this.pnlMaster.Controls.Add(this.cboEmployeeStatus);
            this.pnlMaster.Controls.Add(this.lblNationality);
            this.pnlMaster.Controls.Add(this.dtpLicenceExpiryDate);
            this.pnlMaster.Controls.Add(this.lblDrivingLicence);
            this.pnlMaster.Controls.Add(this.txtDrivingClass);
            this.pnlMaster.Controls.Add(this.lblDrivingClass);
            this.pnlMaster.Controls.Add(this.txtDrivingLicence);
            this.pnlMaster.Controls.Add(this.lblLicenceExpiryDate);
            this.pnlMaster.Controls.Add(this.txtNationality);
            this.pnlMaster.Controls.Add(this.lblDefaultVehicle);
            this.pnlMaster.Controls.Add(this.txtNIRC);
            this.pnlMaster.Controls.Add(this.lblEmployStatus);
            this.pnlMaster.Controls.Add(this.txtDriverName);
            this.pnlMaster.Controls.Add(this.txtDriverCode);
            this.pnlMaster.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMaster.Size = new System.Drawing.Size(823, 129);
            // 
            // bdsMaster
            // 
            this.bdsMaster.DataSource = typeof(FM.TR_MaintenanceDLL.BLL.Driver);
            this.bdsMaster.CurrentChanged += new System.EventHandler(this.bdsMaster_CurrentChanged);
            // 
            // btnClose
            // 
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close ";
            // 
            // lblDriverCode
            // 
            this.lblDriverCode.AutoSize = true;
            this.lblDriverCode.Location = new System.Drawing.Point(19, 11);
            this.lblDriverCode.Name = "lblDriverCode";
            this.lblDriverCode.Size = new System.Drawing.Size(66, 13);
            this.lblDriverCode.TabIndex = 2;
            this.lblDriverCode.Text = "Driver Code:";
            // 
            // dtgrdSeach
            // 
            this.dtgrdSeach.AllowUserToAddRows = false;
            this.dtgrdSeach.AllowUserToDeleteRows = false;
            this.dtgrdSeach.AllowUserToOrderColumns = true;
            this.dtgrdSeach.AutoGenerateColumns = false;
            this.dtgrdSeach.BackgroundColor = System.Drawing.Color.DodgerBlue;
            this.dtgrdSeach.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dtgrdSeach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgrdSeach.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nircDataGridViewTextBoxColumn,
            this.nationalityDataGridViewTextBoxColumn,
            this.licenceExpiryDateDataGridViewTextBoxColumn,
            this.defaultVehicleNumberDataGridViewTextBoxColumn,
            this.isAvailableDataGridViewCheckBoxColumn,
            this.descriptionForPlanningPurposeDataGridViewTextBoxColumn,
            this.priorityDataGridViewTextBoxColumn,
            this.unAvailableDisplayTimeDataGridViewTextBoxColumn,
            this.codeDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.drivingLicenceDataGridViewTextBoxColumn,
            this.drivingClassDataGridViewTextBoxColumn,
            this.employeeStatusDataGridViewTextBoxColumn});
            this.dtgrdSeach.DataSource = this.bdsMaster;
            this.dtgrdSeach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgrdSeach.Location = new System.Drawing.Point(3, 39);
            this.dtgrdSeach.Name = "dtgrdSeach";
            this.dtgrdSeach.ReadOnly = true;
            this.dtgrdSeach.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgrdSeach.Size = new System.Drawing.Size(649, 320);
            this.dtgrdSeach.TabIndex = 9;
            this.dtgrdSeach.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dtgrdSeach_CellMouseDown);
            // 
            // cboDriverCode
            // 
            this.cboDriverCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDriverCode.BackColor = System.Drawing.Color.White;
            this.cboDriverCode.FormattingEnabled = true;
            this.cboDriverCode.Location = new System.Drawing.Point(91, 8);
            this.cboDriverCode.Name = "cboDriverCode";
            this.cboDriverCode.Size = new System.Drawing.Size(121, 21);
            this.cboDriverCode.TabIndex = 3;
            this.cboDriverCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDriverCode_KeyPress);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(323, 7);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // gbxUnavailableDates
            // 
            this.gbxUnavailableDates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.gbxUnavailableDates.Controls.Add(this.dgvUnavailableDates);
            this.gbxUnavailableDates.Controls.Add(this.panel1);
            this.gbxUnavailableDates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxUnavailableDates.Enabled = false;
            this.gbxUnavailableDates.Location = new System.Drawing.Point(3, 132);
            this.gbxUnavailableDates.Name = "gbxUnavailableDates";
            this.gbxUnavailableDates.Size = new System.Drawing.Size(823, 276);
            this.gbxUnavailableDates.TabIndex = 10;
            this.gbxUnavailableDates.TabStop = false;
            this.gbxUnavailableDates.Text = "Unavailable Dates";
            // 
            // dgvUnavailableDates
            // 
            this.dgvUnavailableDates.AllowUserToAddRows = false;
            this.dgvUnavailableDates.AllowUserToDeleteRows = false;
            this.dgvUnavailableDates.AllowUserToOrderColumns = true;
            this.dgvUnavailableDates.AutoGenerateColumns = false;
            this.dgvUnavailableDates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvUnavailableDates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnavailableDates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.driverCodeDataGridViewTextBoxColumn,
            this.seqNoDataGridViewTextBoxColumn,
            this.startDateDataGridViewTextBoxColumn,
            this.startTime,
            this.endDateDataGridViewTextBoxColumn,
            this.endTime,
            this.remarksDataGridViewTextBoxColumn});
            this.dgvUnavailableDates.DataSource = this.bdsUnavailableDates;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.Format = "t";
            dataGridViewCellStyle8.NullValue = null;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUnavailableDates.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvUnavailableDates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUnavailableDates.Location = new System.Drawing.Point(3, 76);
            this.dgvUnavailableDates.Name = "dgvUnavailableDates";
            this.dgvUnavailableDates.ReadOnly = true;
            this.dgvUnavailableDates.Size = new System.Drawing.Size(817, 197);
            this.dgvUnavailableDates.TabIndex = 1;
            // 
            // driverCodeDataGridViewTextBoxColumn
            // 
            this.driverCodeDataGridViewTextBoxColumn.DataPropertyName = "driverCode";
            this.driverCodeDataGridViewTextBoxColumn.HeaderText = "Driver Code";
            this.driverCodeDataGridViewTextBoxColumn.Name = "driverCodeDataGridViewTextBoxColumn";
            this.driverCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.driverCodeDataGridViewTextBoxColumn.Width = 88;
            // 
            // seqNoDataGridViewTextBoxColumn
            // 
            this.seqNoDataGridViewTextBoxColumn.DataPropertyName = "seqNo";
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.seqNoDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.seqNoDataGridViewTextBoxColumn.HeaderText = "SeqNo";
            this.seqNoDataGridViewTextBoxColumn.Name = "seqNoDataGridViewTextBoxColumn";
            this.seqNoDataGridViewTextBoxColumn.ReadOnly = true;
            this.seqNoDataGridViewTextBoxColumn.Width = 65;
            // 
            // startDateDataGridViewTextBoxColumn
            // 
            this.startDateDataGridViewTextBoxColumn.DataPropertyName = "startDateTime";
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.startDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.startDateDataGridViewTextBoxColumn.HeaderText = "Start Date";
            this.startDateDataGridViewTextBoxColumn.Name = "startDateDataGridViewTextBoxColumn";
            this.startDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.startDateDataGridViewTextBoxColumn.Width = 80;
            // 
            // startTime
            // 
            this.startTime.DataPropertyName = "startDateTime";
            dataGridViewCellStyle5.Format = "HH:mm";
            dataGridViewCellStyle5.NullValue = null;
            this.startTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.startTime.HeaderText = "Start Time";
            this.startTime.Name = "startTime";
            this.startTime.ReadOnly = true;
            this.startTime.Width = 80;
            // 
            // endDateDataGridViewTextBoxColumn
            // 
            this.endDateDataGridViewTextBoxColumn.DataPropertyName = "endDateTime";
            dataGridViewCellStyle6.Format = "d";
            this.endDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.endDateDataGridViewTextBoxColumn.HeaderText = "End Date ";
            this.endDateDataGridViewTextBoxColumn.Name = "endDateDataGridViewTextBoxColumn";
            this.endDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.endDateDataGridViewTextBoxColumn.Width = 80;
            // 
            // endTime
            // 
            this.endTime.DataPropertyName = "endDateTime";
            dataGridViewCellStyle7.Format = "t";
            this.endTime.DefaultCellStyle = dataGridViewCellStyle7;
            this.endTime.HeaderText = "End Time";
            this.endTime.Name = "endTime";
            this.endTime.ReadOnly = true;
            this.endTime.Width = 77;
            // 
            // remarksDataGridViewTextBoxColumn
            // 
            this.remarksDataGridViewTextBoxColumn.DataPropertyName = "remarks";
            this.remarksDataGridViewTextBoxColumn.HeaderText = "Remarks";
            this.remarksDataGridViewTextBoxColumn.Name = "remarksDataGridViewTextBoxColumn";
            this.remarksDataGridViewTextBoxColumn.ReadOnly = true;
            this.remarksDataGridViewTextBoxColumn.Width = 74;
            // 
            // bdsUnavailableDates
            // 
            this.bdsUnavailableDates.DataSource = typeof(FM.TR_MaintenanceDLL.BLL.DriverUnavailable);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.panel1.Controls.Add(this.btnDeleteUnavailableDate);
            this.panel1.Controls.Add(this.btnEditUnvailableDate);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnUnavailableDateQuery);
            this.panel1.Controls.Add(this.dtpUnavailableEndTime);
            this.panel1.Controls.Add(this.dtpUnavailableEndDate);
            this.panel1.Controls.Add(this.lblStartDateTimeTo);
            this.panel1.Controls.Add(this.dtpUnavailableStartTime);
            this.panel1.Controls.Add(this.dtpUnavailableStartDate);
            this.panel1.Controls.Add(this.lblStartDateTimeFrom);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(817, 60);
            this.panel1.TabIndex = 0;
            // 
            // btnDeleteUnavailableDate
            // 
            this.btnDeleteUnavailableDate.Location = new System.Drawing.Point(157, 34);
            this.btnDeleteUnavailableDate.Name = "btnDeleteUnavailableDate";
            this.btnDeleteUnavailableDate.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteUnavailableDate.TabIndex = 16;
            this.btnDeleteUnavailableDate.Text = "Delete";
            this.btnDeleteUnavailableDate.UseVisualStyleBackColor = true;
            this.btnDeleteUnavailableDate.Click += new System.EventHandler(this.btnDeleteUnavailableDate_Click);
            // 
            // btnEditUnvailableDate
            // 
            this.btnEditUnvailableDate.Location = new System.Drawing.Point(81, 34);
            this.btnEditUnvailableDate.Name = "btnEditUnvailableDate";
            this.btnEditUnvailableDate.Size = new System.Drawing.Size(75, 23);
            this.btnEditUnvailableDate.TabIndex = 15;
            this.btnEditUnvailableDate.Text = "Edit";
            this.btnEditUnvailableDate.UseVisualStyleBackColor = true;
            this.btnEditUnvailableDate.Click += new System.EventHandler(this.btnEditUnavailableDate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(5, 34);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUnavailableDateQuery
            // 
            this.btnUnavailableDateQuery.Location = new System.Drawing.Point(595, 5);
            this.btnUnavailableDateQuery.Name = "btnUnavailableDateQuery";
            this.btnUnavailableDateQuery.Size = new System.Drawing.Size(75, 23);
            this.btnUnavailableDateQuery.TabIndex = 13;
            this.btnUnavailableDateQuery.Text = "Search";
            this.btnUnavailableDateQuery.UseVisualStyleBackColor = true;
            this.btnUnavailableDateQuery.Click += new System.EventHandler(this.btnUnavailableDateQuery_Click);
            // 
            // dtpUnavailableEndTime
            // 
            this.dtpUnavailableEndTime.CustomFormat = "HH:mm";
            this.dtpUnavailableEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpUnavailableEndTime.Location = new System.Drawing.Point(503, 6);
            this.dtpUnavailableEndTime.Name = "dtpUnavailableEndTime";
            this.dtpUnavailableEndTime.ShowUpDown = true;
            this.dtpUnavailableEndTime.Size = new System.Drawing.Size(56, 20);
            this.dtpUnavailableEndTime.TabIndex = 12;
            this.dtpUnavailableEndTime.Value = new System.DateTime(2014, 6, 27, 0, 0, 0, 0);
            this.dtpUnavailableEndTime.FormatChanged += new System.EventHandler(this.dtpStartTimeTo_FormatChanged);
            // 
            // dtpUnavailableEndDate
            // 
            this.dtpUnavailableEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpUnavailableEndDate.Location = new System.Drawing.Point(408, 6);
            this.dtpUnavailableEndDate.Name = "dtpUnavailableEndDate";
            this.dtpUnavailableEndDate.Size = new System.Drawing.Size(90, 20);
            this.dtpUnavailableEndDate.TabIndex = 11;
            this.dtpUnavailableEndDate.Value = new System.DateTime(2014, 6, 27, 0, 0, 0, 0);
            this.dtpUnavailableEndDate.ValueChanged += new System.EventHandler(this.dtpUnavailableEndDate_ValueChanged);
            // 
            // lblStartDateTimeTo
            // 
            this.lblStartDateTimeTo.Location = new System.Drawing.Point(297, 10);
            this.lblStartDateTimeTo.Name = "lblStartDateTimeTo";
            this.lblStartDateTimeTo.Size = new System.Drawing.Size(107, 13);
            this.lblStartDateTimeTo.TabIndex = 10;
            this.lblStartDateTimeTo.Text = "End Date :";
            this.lblStartDateTimeTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpUnavailableStartTime
            // 
            this.dtpUnavailableStartTime.CustomFormat = "HH:mm";
            this.dtpUnavailableStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpUnavailableStartTime.Location = new System.Drawing.Point(209, 6);
            this.dtpUnavailableStartTime.Name = "dtpUnavailableStartTime";
            this.dtpUnavailableStartTime.ShowUpDown = true;
            this.dtpUnavailableStartTime.Size = new System.Drawing.Size(56, 20);
            this.dtpUnavailableStartTime.TabIndex = 9;
            this.dtpUnavailableStartTime.Value = new System.DateTime(2014, 6, 27, 0, 0, 0, 0);
            this.dtpUnavailableStartTime.FormatChanged += new System.EventHandler(this.dtpStartTimeTo_FormatChanged);
            // 
            // dtpUnavailableStartDate
            // 
            this.dtpUnavailableStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpUnavailableStartDate.Location = new System.Drawing.Point(118, 6);
            this.dtpUnavailableStartDate.Name = "dtpUnavailableStartDate";
            this.dtpUnavailableStartDate.Size = new System.Drawing.Size(90, 20);
            this.dtpUnavailableStartDate.TabIndex = 8;
            this.dtpUnavailableStartDate.Value = new System.DateTime(2014, 6, 27, 0, 0, 0, 0);
            this.dtpUnavailableStartDate.ValueChanged += new System.EventHandler(this.dtpUnavailableStartDate_ValueChanged);
            // 
            // lblStartDateTimeFrom
            // 
            this.lblStartDateTimeFrom.Location = new System.Drawing.Point(7, 10);
            this.lblStartDateTimeFrom.Name = "lblStartDateTimeFrom";
            this.lblStartDateTimeFrom.Size = new System.Drawing.Size(107, 13);
            this.lblStartDateTimeFrom.TabIndex = 7;
            this.lblStartDateTimeFrom.Text = "Start Date :";
            this.lblStartDateTimeFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCode
            // 
            this.lblCode.Location = new System.Drawing.Point(8, 7);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(127, 13);
            this.lblCode.TabIndex = 10;
            this.lblCode.Text = "Driver Code :";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(7, 28);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(127, 13);
            this.lblName.TabIndex = 12;
            this.lblName.Text = "Name :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDefaultVehicle
            // 
            this.cboDefaultVehicle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDefaultVehicle.BackColor = System.Drawing.Color.White;
            this.cboDefaultVehicle.FormattingEnabled = true;
            this.cboDefaultVehicle.Location = new System.Drawing.Point(478, 44);
            this.cboDefaultVehicle.Name = "cboDefaultVehicle";
            this.cboDefaultVehicle.Size = new System.Drawing.Size(139, 21);
            this.cboDefaultVehicle.TabIndex = 23;
            this.cboDefaultVehicle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDriverCode_KeyPress);
            // 
            // lblNIRC
            // 
            this.lblNIRC.Location = new System.Drawing.Point(7, 48);
            this.lblNIRC.Name = "lblNIRC";
            this.lblNIRC.Size = new System.Drawing.Size(127, 13);
            this.lblNIRC.TabIndex = 14;
            this.lblNIRC.Text = "NIRC :";
            this.lblNIRC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboEmployeeStatus
            // 
            this.cboEmployeeStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboEmployeeStatus.FormattingEnabled = true;
            this.cboEmployeeStatus.Location = new System.Drawing.Point(478, 65);
            this.cboEmployeeStatus.Name = "cboEmployeeStatus";
            this.cboEmployeeStatus.Size = new System.Drawing.Size(138, 21);
            this.cboEmployeeStatus.TabIndex = 26;
            this.cboEmployeeStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDriverCode_KeyPress);
            // 
            // lblNationality
            // 
            this.lblNationality.Location = new System.Drawing.Point(7, 68);
            this.lblNationality.Name = "lblNationality";
            this.lblNationality.Size = new System.Drawing.Size(127, 13);
            this.lblNationality.TabIndex = 16;
            this.lblNationality.Text = "Nationality :";
            this.lblNationality.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpLicenceExpiryDate
            // 
            this.dtpLicenceExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpLicenceExpiryDate.Location = new System.Drawing.Point(478, 24);
            this.dtpLicenceExpiryDate.Name = "dtpLicenceExpiryDate";
            this.dtpLicenceExpiryDate.Size = new System.Drawing.Size(139, 20);
            this.dtpLicenceExpiryDate.TabIndex = 21;
            this.dtpLicenceExpiryDate.Value = new System.DateTime(2009, 3, 5, 0, 0, 0, 0);
            // 
            // lblDrivingLicence
            // 
            this.lblDrivingLicence.Location = new System.Drawing.Point(7, 87);
            this.lblDrivingLicence.Name = "lblDrivingLicence";
            this.lblDrivingLicence.Size = new System.Drawing.Size(127, 13);
            this.lblDrivingLicence.TabIndex = 18;
            this.lblDrivingLicence.Text = "Driving Licence :";
            this.lblDrivingLicence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDrivingClass
            // 
            this.txtDrivingClass.BackColor = System.Drawing.Color.SkyBlue;
            this.txtDrivingClass.Location = new System.Drawing.Point(135, 102);
            this.txtDrivingClass.Name = "txtDrivingClass";
            this.txtDrivingClass.Size = new System.Drawing.Size(152, 20);
            this.txtDrivingClass.TabIndex = 19;
            // 
            // lblDrivingClass
            // 
            this.lblDrivingClass.Location = new System.Drawing.Point(7, 106);
            this.lblDrivingClass.Name = "lblDrivingClass";
            this.lblDrivingClass.Size = new System.Drawing.Size(127, 13);
            this.lblDrivingClass.TabIndex = 20;
            this.lblDrivingClass.Text = "Driving Class  :";
            this.lblDrivingClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDrivingLicence
            // 
            this.txtDrivingLicence.BackColor = System.Drawing.Color.SkyBlue;
            this.txtDrivingLicence.Location = new System.Drawing.Point(135, 83);
            this.txtDrivingLicence.Name = "txtDrivingLicence";
            this.txtDrivingLicence.Size = new System.Drawing.Size(152, 20);
            this.txtDrivingLicence.TabIndex = 17;
            // 
            // lblLicenceExpiryDate
            // 
            this.lblLicenceExpiryDate.Location = new System.Drawing.Point(341, 28);
            this.lblLicenceExpiryDate.Name = "lblLicenceExpiryDate";
            this.lblLicenceExpiryDate.Size = new System.Drawing.Size(130, 13);
            this.lblLicenceExpiryDate.TabIndex = 22;
            this.lblLicenceExpiryDate.Text = "Licence Expiry Date  :";
            this.lblLicenceExpiryDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNationality
            // 
            this.txtNationality.BackColor = System.Drawing.Color.SkyBlue;
            this.txtNationality.Location = new System.Drawing.Point(135, 64);
            this.txtNationality.Name = "txtNationality";
            this.txtNationality.Size = new System.Drawing.Size(203, 20);
            this.txtNationality.TabIndex = 15;
            // 
            // lblDefaultVehicle
            // 
            this.lblDefaultVehicle.Location = new System.Drawing.Point(341, 48);
            this.lblDefaultVehicle.Name = "lblDefaultVehicle";
            this.lblDefaultVehicle.Size = new System.Drawing.Size(130, 13);
            this.lblDefaultVehicle.TabIndex = 24;
            this.lblDefaultVehicle.Text = "Default Vehicle :";
            this.lblDefaultVehicle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNIRC
            // 
            this.txtNIRC.BackColor = System.Drawing.Color.SkyBlue;
            this.txtNIRC.Location = new System.Drawing.Point(135, 44);
            this.txtNIRC.Name = "txtNIRC";
            this.txtNIRC.Size = new System.Drawing.Size(203, 20);
            this.txtNIRC.TabIndex = 13;
            // 
            // lblEmployStatus
            // 
            this.lblEmployStatus.Location = new System.Drawing.Point(342, 69);
            this.lblEmployStatus.Name = "lblEmployStatus";
            this.lblEmployStatus.Size = new System.Drawing.Size(130, 13);
            this.lblEmployStatus.TabIndex = 25;
            this.lblEmployStatus.Text = "Employee Status:";
            this.lblEmployStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDriverName
            // 
            this.txtDriverName.BackColor = System.Drawing.Color.SkyBlue;
            this.txtDriverName.Location = new System.Drawing.Point(135, 24);
            this.txtDriverName.Name = "txtDriverName";
            this.txtDriverName.Size = new System.Drawing.Size(203, 20);
            this.txtDriverName.TabIndex = 11;
            // 
            // txtDriverCode
            // 
            this.txtDriverCode.BackColor = System.Drawing.Color.SkyBlue;
            this.txtDriverCode.Location = new System.Drawing.Point(135, 4);
            this.txtDriverCode.Name = "txtDriverCode";
            this.txtDriverCode.Size = new System.Drawing.Size(89, 20);
            this.txtDriverCode.TabIndex = 9;
            // 
            // lblWarnMsg
            // 
            this.lblWarnMsg.AutoSize = true;
            this.lblWarnMsg.Location = new System.Drawing.Point(253, 18);
            this.lblWarnMsg.Name = "lblWarnMsg";
            this.lblWarnMsg.Size = new System.Drawing.Size(35, 13);
            this.lblWarnMsg.TabIndex = 3;
            this.lblWarnMsg.Text = "label1";
            // 
            // tabPageJobAssignmentByDriver
            // 
            this.tabPageJobAssignmentByDriver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.tabPageJobAssignmentByDriver.Controls.Add(this.dgvAssignmentsByDriver);
            this.tabPageJobAssignmentByDriver.Controls.Add(this.panel2);
            this.tabPageJobAssignmentByDriver.Location = new System.Drawing.Point(4, 25);
            this.tabPageJobAssignmentByDriver.Name = "tabPageJobAssignmentByDriver";
            this.tabPageJobAssignmentByDriver.Size = new System.Drawing.Size(655, 362);
            this.tabPageJobAssignmentByDriver.TabIndex = 2;
            this.tabPageJobAssignmentByDriver.Text = "Job Assignment By Driver";
            // 
            // dgvAssignmentsByDriver
            // 
            this.dgvAssignmentsByDriver.AllowUserToAddRows = false;
            this.dgvAssignmentsByDriver.AllowUserToDeleteRows = false;
            this.dgvAssignmentsByDriver.AllowUserToOrderColumns = true;
            this.dgvAssignmentsByDriver.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAssignmentsByDriver.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAssignmentsByDriver.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssignmentsByDriver.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.driverCodeDataGridViewTextBoxColumn1,
            this.DefaultVehicle,
            this.ScheduleDate,
            this.Prior_Desc});
            this.dgvAssignmentsByDriver.DataSource = this.bdsAssignmentByDriver;
            this.dgvAssignmentsByDriver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAssignmentsByDriver.Location = new System.Drawing.Point(0, 39);
            this.dgvAssignmentsByDriver.Name = "dgvAssignmentsByDriver";
            this.dgvAssignmentsByDriver.ReadOnly = true;
            this.dgvAssignmentsByDriver.RowHeadersWidth = 20;
            this.dgvAssignmentsByDriver.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAssignmentsByDriver.Size = new System.Drawing.Size(655, 323);
            this.dgvAssignmentsByDriver.TabIndex = 34;
            this.dgvAssignmentsByDriver.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssignments_CellDoubleClick);
            // 
            // driverCodeDataGridViewTextBoxColumn1
            // 
            this.driverCodeDataGridViewTextBoxColumn1.DataPropertyName = "DriverCode";
            this.driverCodeDataGridViewTextBoxColumn1.HeaderText = "DriverCode";
            this.driverCodeDataGridViewTextBoxColumn1.Name = "driverCodeDataGridViewTextBoxColumn1";
            this.driverCodeDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // DefaultVehicle
            // 
            this.DefaultVehicle.DataPropertyName = "DefaultVehicle";
            this.DefaultVehicle.HeaderText = "Prime Mover";
            this.DefaultVehicle.Name = "DefaultVehicle";
            this.DefaultVehicle.ReadOnly = true;
            // 
            // ScheduleDate
            // 
            this.ScheduleDate.DataPropertyName = "ScheduleDate";
            this.ScheduleDate.HeaderText = "ScheduleDate";
            this.ScheduleDate.Name = "ScheduleDate";
            this.ScheduleDate.ReadOnly = true;
            // 
            // Prior_Desc
            // 
            this.Prior_Desc.DataPropertyName = "Prior_Desc";
            this.Prior_Desc.HeaderText = "Assignment";
            this.Prior_Desc.Name = "Prior_Desc";
            this.Prior_Desc.ReadOnly = true;
            // 
            // bdsAssignmentByDriver
            // 
            this.bdsAssignmentByDriver.DataSource = typeof(FM.TR_MaintenanceDLL.BLL.DriverPriority);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAddAssignments);
            this.panel2.Controls.Add(this.dtpAssignmentFrom);
            this.panel2.Controls.Add(this.btnGetJobAssignments);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dtpAssignmentTo);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(655, 39);
            this.panel2.TabIndex = 33;
            // 
            // btnAddAssignments
            // 
            this.btnAddAssignments.Location = new System.Drawing.Point(599, 10);
            this.btnAddAssignments.Name = "btnAddAssignments";
            this.btnAddAssignments.Size = new System.Drawing.Size(127, 23);
            this.btnAddAssignments.TabIndex = 35;
            this.btnAddAssignments.Text = "Add Job Assignments";
            this.btnAddAssignments.UseVisualStyleBackColor = true;
            this.btnAddAssignments.Click += new System.EventHandler(this.btnAddAssignments_Click);
            // 
            // dtpAssignmentFrom
            // 
            this.dtpAssignmentFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAssignmentFrom.Location = new System.Drawing.Point(76, 11);
            this.dtpAssignmentFrom.Name = "dtpAssignmentFrom";
            this.dtpAssignmentFrom.Size = new System.Drawing.Size(111, 20);
            this.dtpAssignmentFrom.TabIndex = 29;
            this.dtpAssignmentFrom.ValueChanged += new System.EventHandler(this.dtpAssignmentFrom_ValueChanged);
            // 
            // btnGetJobAssignments
            // 
            this.btnGetJobAssignments.Location = new System.Drawing.Point(396, 10);
            this.btnGetJobAssignments.Name = "btnGetJobAssignments";
            this.btnGetJobAssignments.Size = new System.Drawing.Size(185, 23);
            this.btnGetJobAssignments.TabIndex = 32;
            this.btnGetJobAssignments.Text = "Get Job Assignments by Driver";
            this.btnGetJobAssignments.UseVisualStyleBackColor = true;
            this.btnGetJobAssignments.Click += new System.EventHandler(this.btnGetJobAssignments_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Date From :";
            // 
            // dtpAssignmentTo
            // 
            this.dtpAssignmentTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAssignmentTo.Location = new System.Drawing.Point(279, 11);
            this.dtpAssignmentTo.Name = "dtpAssignmentTo";
            this.dtpAssignmentTo.Size = new System.Drawing.Size(111, 20);
            this.dtpAssignmentTo.TabIndex = 31;
            this.dtpAssignmentTo.ValueChanged += new System.EventHandler(this.dtpAssignmentTo_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(211, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Date To :";
            // 
            // tabPageAssignmentSummary
            // 
            this.tabPageAssignmentSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.tabPageAssignmentSummary.Controls.Add(this.dgvAssignmentSummary);
            this.tabPageAssignmentSummary.Controls.Add(this.panel3);
            this.tabPageAssignmentSummary.Location = new System.Drawing.Point(4, 25);
            this.tabPageAssignmentSummary.Name = "tabPageAssignmentSummary";
            this.tabPageAssignmentSummary.Size = new System.Drawing.Size(655, 362);
            this.tabPageAssignmentSummary.TabIndex = 3;
            this.tabPageAssignmentSummary.Text = "Job Assignment Summary";
            // 
            // dgvAssignmentSummary
            // 
            this.dgvAssignmentSummary.AllowUserToAddRows = false;
            this.dgvAssignmentSummary.AllowUserToDeleteRows = false;
            this.dgvAssignmentSummary.AllowUserToOrderColumns = true;
            this.dgvAssignmentSummary.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAssignmentSummary.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAssignmentSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssignmentSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn5});
            this.dgvAssignmentSummary.DataSource = this.bdsAssignmentSummary;
            this.dgvAssignmentSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAssignmentSummary.Location = new System.Drawing.Point(0, 39);
            this.dgvAssignmentSummary.Name = "dgvAssignmentSummary";
            this.dgvAssignmentSummary.ReadOnly = true;
            this.dgvAssignmentSummary.RowHeadersWidth = 20;
            this.dgvAssignmentSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAssignmentSummary.Size = new System.Drawing.Size(655, 323);
            this.dgvAssignmentSummary.TabIndex = 36;
            this.dgvAssignmentSummary.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssignmentSummary_CellDoubleClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "DriverCode";
            this.dataGridViewTextBoxColumn1.HeaderText = "DriverCode";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "DefaultVehicle";
            this.dataGridViewTextBoxColumn2.HeaderText = "Prime Mover";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ScheduleDate";
            this.dataGridViewTextBoxColumn3.HeaderText = "ScheduleDate";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Prior_Desc";
            this.dataGridViewTextBoxColumn5.HeaderText = "Assignment";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // bdsAssignmentSummary
            // 
            this.bdsAssignmentSummary.DataSource = typeof(FM.TR_MaintenanceDLL.BLL.DriverPriority);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtpJobSummaryDateFrom);
            this.panel3.Controls.Add(this.btnAssignmentSummary);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.dtpJobSummaryDateTo);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(655, 39);
            this.panel3.TabIndex = 35;
            // 
            // dtpJobSummaryDateFrom
            // 
            this.dtpJobSummaryDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpJobSummaryDateFrom.Location = new System.Drawing.Point(76, 11);
            this.dtpJobSummaryDateFrom.Name = "dtpJobSummaryDateFrom";
            this.dtpJobSummaryDateFrom.Size = new System.Drawing.Size(111, 20);
            this.dtpJobSummaryDateFrom.TabIndex = 29;
            this.dtpJobSummaryDateFrom.ValueChanged += new System.EventHandler(this.dtpJobSummaryDateFrom_ValueChanged);
            // 
            // btnAssignmentSummary
            // 
            this.btnAssignmentSummary.Location = new System.Drawing.Point(396, 10);
            this.btnAssignmentSummary.Name = "btnAssignmentSummary";
            this.btnAssignmentSummary.Size = new System.Drawing.Size(214, 23);
            this.btnAssignmentSummary.TabIndex = 32;
            this.btnAssignmentSummary.Text = "Get Job Assignments Summary";
            this.btnAssignmentSummary.UseVisualStyleBackColor = true;
            this.btnAssignmentSummary.Click += new System.EventHandler(this.btnAssignmentSummary_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Date From :";
            // 
            // dtpJobSummaryDateTo
            // 
            this.dtpJobSummaryDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpJobSummaryDateTo.Location = new System.Drawing.Point(279, 11);
            this.dtpJobSummaryDateTo.Name = "dtpJobSummaryDateTo";
            this.dtpJobSummaryDateTo.Size = new System.Drawing.Size(111, 20);
            this.dtpJobSummaryDateTo.TabIndex = 31;
            this.dtpJobSummaryDateTo.ValueChanged += new System.EventHandler(this.dtpJobSummaryDateTo_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(211, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Date To :";
            // 
            // dgvContextMenu
            // 
            this.dgvContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPriorityToolStripMenuItem});
            this.dgvContextMenu.Name = "dgvContextMenu";
            this.dgvContextMenu.Size = new System.Drawing.Size(138, 26);
            // 
            // addPriorityToolStripMenuItem
            // 
            this.addPriorityToolStripMenuItem.Name = "addPriorityToolStripMenuItem";
            this.addPriorityToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.addPriorityToolStripMenuItem.Text = "Add Priority";
            this.addPriorityToolStripMenuItem.Click += new System.EventHandler(this.addPriorityToolStripMenuItem_Click);
            // 
            // nircDataGridViewTextBoxColumn
            // 
            this.nircDataGridViewTextBoxColumn.DataPropertyName = "nirc";
            this.nircDataGridViewTextBoxColumn.HeaderText = "nirc";
            this.nircDataGridViewTextBoxColumn.Name = "nircDataGridViewTextBoxColumn";
            this.nircDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nationalityDataGridViewTextBoxColumn
            // 
            this.nationalityDataGridViewTextBoxColumn.DataPropertyName = "nationality";
            this.nationalityDataGridViewTextBoxColumn.HeaderText = "nationality";
            this.nationalityDataGridViewTextBoxColumn.Name = "nationalityDataGridViewTextBoxColumn";
            this.nationalityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // licenceExpiryDateDataGridViewTextBoxColumn
            // 
            this.licenceExpiryDateDataGridViewTextBoxColumn.DataPropertyName = "licenceExpiryDate";
            this.licenceExpiryDateDataGridViewTextBoxColumn.HeaderText = "licenceExpiryDate";
            this.licenceExpiryDateDataGridViewTextBoxColumn.Name = "licenceExpiryDateDataGridViewTextBoxColumn";
            this.licenceExpiryDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // defaultVehicleNumberDataGridViewTextBoxColumn
            // 
            this.defaultVehicleNumberDataGridViewTextBoxColumn.DataPropertyName = "defaultVehicleNumber";
            this.defaultVehicleNumberDataGridViewTextBoxColumn.HeaderText = "defaultVehicleNumber";
            this.defaultVehicleNumberDataGridViewTextBoxColumn.Name = "defaultVehicleNumberDataGridViewTextBoxColumn";
            this.defaultVehicleNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isAvailableDataGridViewCheckBoxColumn
            // 
            this.isAvailableDataGridViewCheckBoxColumn.DataPropertyName = "isAvailable";
            this.isAvailableDataGridViewCheckBoxColumn.HeaderText = "isAvailable";
            this.isAvailableDataGridViewCheckBoxColumn.Name = "isAvailableDataGridViewCheckBoxColumn";
            this.isAvailableDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // descriptionForPlanningPurposeDataGridViewTextBoxColumn
            // 
            this.descriptionForPlanningPurposeDataGridViewTextBoxColumn.DataPropertyName = "DescriptionForPlanningPurpose";
            this.descriptionForPlanningPurposeDataGridViewTextBoxColumn.HeaderText = "DescriptionForPlanningPurpose";
            this.descriptionForPlanningPurposeDataGridViewTextBoxColumn.Name = "descriptionForPlanningPurposeDataGridViewTextBoxColumn";
            this.descriptionForPlanningPurposeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // priorityDataGridViewTextBoxColumn
            // 
            this.priorityDataGridViewTextBoxColumn.DataPropertyName = "priority";
            this.priorityDataGridViewTextBoxColumn.HeaderText = "priority";
            this.priorityDataGridViewTextBoxColumn.Name = "priorityDataGridViewTextBoxColumn";
            this.priorityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // unAvailableDisplayTimeDataGridViewTextBoxColumn
            // 
            this.unAvailableDisplayTimeDataGridViewTextBoxColumn.DataPropertyName = "unAvailableDisplayTime";
            this.unAvailableDisplayTimeDataGridViewTextBoxColumn.HeaderText = "unAvailableDisplayTime";
            this.unAvailableDisplayTimeDataGridViewTextBoxColumn.Name = "unAvailableDisplayTimeDataGridViewTextBoxColumn";
            this.unAvailableDisplayTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // drivingLicenceDataGridViewTextBoxColumn
            // 
            this.drivingLicenceDataGridViewTextBoxColumn.DataPropertyName = "DrivingLicence";
            this.drivingLicenceDataGridViewTextBoxColumn.HeaderText = "DrivingLicence";
            this.drivingLicenceDataGridViewTextBoxColumn.Name = "drivingLicenceDataGridViewTextBoxColumn";
            this.drivingLicenceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // drivingClassDataGridViewTextBoxColumn
            // 
            this.drivingClassDataGridViewTextBoxColumn.DataPropertyName = "DrivingClass";
            this.drivingClassDataGridViewTextBoxColumn.HeaderText = "DrivingClass";
            this.drivingClassDataGridViewTextBoxColumn.Name = "drivingClassDataGridViewTextBoxColumn";
            this.drivingClassDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // employeeStatusDataGridViewTextBoxColumn
            // 
            this.employeeStatusDataGridViewTextBoxColumn.DataPropertyName = "EmployeeStatus";
            this.employeeStatusDataGridViewTextBoxColumn.HeaderText = "EmployeeStatus";
            this.employeeStatusDataGridViewTextBoxColumn.Name = "employeeStatusDataGridViewTextBoxColumn";
            this.employeeStatusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FrmDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 531);
            this.Name = "FrmDriver";
            this.Text = "Driver Maintenance";
            this.Load += new System.EventHandler(this.FrmDriver_Load);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.Controls.SetChildIndex(this.pnlBottom, 0);
            this.Controls.SetChildIndex(this.tbcEntry, 0);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tbcEntry.ResumeLayout(false);
            this.tabPageSearch.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlQuery.ResumeLayout(false);
            this.pnlQuery.PerformLayout();
            this.tabPageMaster.ResumeLayout(false);
            this.pnlMaster.ResumeLayout(false);
            this.pnlMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdSeach)).EndInit();
            this.gbxUnavailableDates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnavailableDates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsUnavailableDates)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabPageJobAssignmentByDriver.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignmentsByDriver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsAssignmentByDriver)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPageAssignmentSummary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssignmentSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsAssignmentSummary)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.dgvContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDriverCode;
        private System.Windows.Forms.DataGridView dtgrdSeach;
        private System.Windows.Forms.ComboBox cboDriverCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ratePerLadenTripDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ratePerUnladenTripDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox gbxUnavailableDates;
        private System.Windows.Forms.DataGridView dgvUnavailableDates;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDeleteUnavailableDate;
        private System.Windows.Forms.Button btnEditUnvailableDate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUnavailableDateQuery;
        private System.Windows.Forms.DateTimePicker dtpUnavailableEndTime;
        private System.Windows.Forms.DateTimePicker dtpUnavailableEndDate;
        private System.Windows.Forms.Label lblStartDateTimeTo;
        private System.Windows.Forms.DateTimePicker dtpUnavailableStartTime;
        private System.Windows.Forms.DateTimePicker dtpUnavailableStartDate;
        private System.Windows.Forms.Label lblStartDateTimeFrom;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cboDefaultVehicle;
        private System.Windows.Forms.Label lblNIRC;
        private System.Windows.Forms.ComboBox cboEmployeeStatus;
        private System.Windows.Forms.Label lblNationality;
        private System.Windows.Forms.DateTimePicker dtpLicenceExpiryDate;
        private System.Windows.Forms.Label lblDrivingLicence;
        private System.Windows.Forms.TextBox txtDrivingClass;
        private System.Windows.Forms.Label lblDrivingClass;
        private System.Windows.Forms.TextBox txtDrivingLicence;
        private System.Windows.Forms.Label lblLicenceExpiryDate;
        private System.Windows.Forms.TextBox txtNationality;
        private System.Windows.Forms.Label lblDefaultVehicle;
        private System.Windows.Forms.TextBox txtNIRC;
        private System.Windows.Forms.Label lblEmployStatus;
        private System.Windows.Forms.TextBox txtDriverName;
        private System.Windows.Forms.TextBox txtDriverCode;
        private System.Windows.Forms.BindingSource bdsUnavailableDates;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seqNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isWholeUnavailableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn remarksDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblWarnMsg;
        private System.Windows.Forms.TabPage tabPageJobAssignmentByDriver;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpAssignmentFrom;
        private System.Windows.Forms.Button btnGetJobAssignments;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpAssignmentTo;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.DataGridView dgvAssignmentsByDriver;
        private System.Windows.Forms.DataGridViewTextBoxColumn defaultVehicleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn schedPriorityDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource bdsAssignmentByDriver;
        private System.Windows.Forms.Button btnAddAssignments;
        private System.Windows.Forms.TabPage tabPageAssignmentSummary;
        public System.Windows.Forms.DataGridView dgvAssignmentSummary;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker dtpJobSummaryDateFrom;
        private System.Windows.Forms.Button btnAssignmentSummary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpJobSummaryDateTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bdsAssignmentSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverCodeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultVehicle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScheduleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prior_Desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.ContextMenuStrip dgvContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addPriorityToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn priority;
        private System.Windows.Forms.DataGridViewTextBoxColumn nircDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nationalityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn licenceExpiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn defaultVehicleNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isAvailableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionForPlanningPurposeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn drivingLicenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn drivingClassDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn employeeStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unAvailableDisplayTimeDataGridViewTextBoxColumn;
    }
}