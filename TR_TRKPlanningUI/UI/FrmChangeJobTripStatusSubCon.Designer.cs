using FM.TR_TRKBookDLL.BLL;
namespace FM.TR_TRKPlanningUI.UI
{
    partial class FrmChangeJobTripStatusSubCon
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.gbxCriteria = new System.Windows.Forms.GroupBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.pnlByJobTripDate = new System.Windows.Forms.Panel();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.pnlByJobNo = new System.Windows.Forms.Panel();
            this.cboToJobNo = new System.Windows.Forms.ComboBox();
            this.lblJobNoTo = new System.Windows.Forms.Label();
            this.cboFromJobNo = new System.Windows.Forms.ComboBox();
            this.lblFromJobNo = new System.Windows.Forms.Label();
            this.rdbByJobTripDate = new System.Windows.Forms.RadioButton();
            this.rdbByJobNo = new System.Windows.Forms.RadioButton();
            this.lblDept = new System.Windows.Forms.Label();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.gbxStatus = new System.Windows.Forms.GroupBox();
            this.cboNewStatus = new System.Windows.Forms.ComboBox();
            this.lblNewStatus = new System.Windows.Forms.Label();
            this.cboCurrentStatus = new System.Windows.Forms.ComboBox();
            this.lblCurrentStatus = new System.Windows.Forms.Label();
            this.gbxListOfJobTrips = new System.Windows.Forms.GroupBox();
            this.dgvJobTrips = new System.Windows.Forms.DataGridView();
            this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.jobNoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartStop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndStop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subCon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bdsJobTrips = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSaveStatus = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            this.gbxCriteria.SuspendLayout();
            this.pnlByJobTripDate.SuspendLayout();
            this.pnlByJobNo.SuspendLayout();
            this.gbxStatus.SuspendLayout();
            this.gbxListOfJobTrips.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobTrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsJobTrips)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.gbxCriteria);
            this.pnlTop.Controls.Add(this.gbxStatus);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(763, 178);
            this.pnlTop.TabIndex = 0;
            // 
            // gbxCriteria
            // 
            this.gbxCriteria.Controls.Add(this.btnQuery);
            this.gbxCriteria.Controls.Add(this.pnlByJobTripDate);
            this.gbxCriteria.Controls.Add(this.pnlByJobNo);
            this.gbxCriteria.Controls.Add(this.rdbByJobTripDate);
            this.gbxCriteria.Controls.Add(this.rdbByJobNo);
            this.gbxCriteria.Controls.Add(this.lblDept);
            this.gbxCriteria.Controls.Add(this.cboDept);
            this.gbxCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxCriteria.Location = new System.Drawing.Point(0, 41);
            this.gbxCriteria.Name = "gbxCriteria";
            this.gbxCriteria.Size = new System.Drawing.Size(763, 137);
            this.gbxCriteria.TabIndex = 1;
            this.gbxCriteria.TabStop = false;
            this.gbxCriteria.Text = "Criteria";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(143, 111);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // pnlByJobTripDate
            // 
            this.pnlByJobTripDate.Controls.Add(this.dtpDateTo);
            this.pnlByJobTripDate.Controls.Add(this.lblDateTo);
            this.pnlByJobTripDate.Controls.Add(this.dtpDateFrom);
            this.pnlByJobTripDate.Controls.Add(this.lblDateFrom);
            this.pnlByJobTripDate.Location = new System.Drawing.Point(143, 73);
            this.pnlByJobTripDate.Name = "pnlByJobTripDate";
            this.pnlByJobTripDate.Size = new System.Drawing.Size(559, 28);
            this.pnlByJobTripDate.TabIndex = 5;
            this.pnlByJobTripDate.Visible = false;
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateTo.Location = new System.Drawing.Point(354, 4);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(121, 20);
            this.dtpDateTo.TabIndex = 9;
            // 
            // lblDateTo
            // 
            this.lblDateTo.Location = new System.Drawing.Point(267, 7);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(80, 15);
            this.lblDateTo.TabIndex = 8;
            this.lblDateTo.Text = "To :";
            this.lblDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateFrom.Location = new System.Drawing.Point(91, 4);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(121, 20);
            this.dtpDateFrom.TabIndex = 7;
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.Location = new System.Drawing.Point(4, 7);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(80, 15);
            this.lblDateFrom.TabIndex = 6;
            this.lblDateFrom.Text = "From :";
            this.lblDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlByJobNo
            // 
            this.pnlByJobNo.Controls.Add(this.cboToJobNo);
            this.pnlByJobNo.Controls.Add(this.lblJobNoTo);
            this.pnlByJobNo.Controls.Add(this.cboFromJobNo);
            this.pnlByJobNo.Controls.Add(this.lblFromJobNo);
            this.pnlByJobNo.Location = new System.Drawing.Point(143, 41);
            this.pnlByJobNo.Name = "pnlByJobNo";
            this.pnlByJobNo.Size = new System.Drawing.Size(559, 28);
            this.pnlByJobNo.TabIndex = 4;
            // 
            // cboToJobNo
            // 
            this.cboToJobNo.FormattingEnabled = true;
            this.cboToJobNo.Location = new System.Drawing.Point(354, 4);
            this.cboToJobNo.Name = "cboToJobNo";
            this.cboToJobNo.Size = new System.Drawing.Size(145, 21);
            this.cboToJobNo.TabIndex = 7;
            this.cboToJobNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCurrentStatus_KeyPress);
            // 
            // lblJobNoTo
            // 
            this.lblJobNoTo.Location = new System.Drawing.Point(267, 7);
            this.lblJobNoTo.Name = "lblJobNoTo";
            this.lblJobNoTo.Size = new System.Drawing.Size(80, 15);
            this.lblJobNoTo.TabIndex = 6;
            this.lblJobNoTo.Text = "To :";
            this.lblJobNoTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboFromJobNo
            // 
            this.cboFromJobNo.FormattingEnabled = true;
            this.cboFromJobNo.Location = new System.Drawing.Point(91, 4);
            this.cboFromJobNo.Name = "cboFromJobNo";
            this.cboFromJobNo.Size = new System.Drawing.Size(145, 21);
            this.cboFromJobNo.TabIndex = 5;
            this.cboFromJobNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCurrentStatus_KeyPress);
            // 
            // lblFromJobNo
            // 
            this.lblFromJobNo.Location = new System.Drawing.Point(4, 7);
            this.lblFromJobNo.Name = "lblFromJobNo";
            this.lblFromJobNo.Size = new System.Drawing.Size(80, 15);
            this.lblFromJobNo.TabIndex = 4;
            this.lblFromJobNo.Text = "From :";
            this.lblFromJobNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rdbByJobTripDate
            // 
            this.rdbByJobTripDate.AutoSize = true;
            this.rdbByJobTripDate.Location = new System.Drawing.Point(23, 79);
            this.rdbByJobTripDate.Name = "rdbByJobTripDate";
            this.rdbByJobTripDate.Size = new System.Drawing.Size(104, 17);
            this.rdbByJobTripDate.TabIndex = 3;
            this.rdbByJobTripDate.TabStop = true;
            this.rdbByJobTripDate.Text = "By Job Trip Date";
            this.rdbByJobTripDate.UseVisualStyleBackColor = true;
            this.rdbByJobTripDate.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // rdbByJobNo
            // 
            this.rdbByJobNo.AutoSize = true;
            this.rdbByJobNo.Location = new System.Drawing.Point(23, 47);
            this.rdbByJobNo.Name = "rdbByJobNo";
            this.rdbByJobNo.Size = new System.Drawing.Size(97, 17);
            this.rdbByJobNo.TabIndex = 2;
            this.rdbByJobNo.Text = "By Job Number";
            this.rdbByJobNo.UseVisualStyleBackColor = true;
            this.rdbByJobNo.CheckedChanged += new System.EventHandler(this.rdb_CheckedChanged);
            // 
            // lblDept
            // 
            this.lblDept.Location = new System.Drawing.Point(8, 17);
            this.lblDept.Name = "lblDept";
            this.lblDept.Size = new System.Drawing.Size(128, 15);
            this.lblDept.TabIndex = 0;
            this.lblDept.Text = "Department :";
            this.lblDept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDept
            // 
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(143, 14);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(121, 21);
            this.cboDept.TabIndex = 1;
            this.cboDept.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCurrentStatus_KeyPress);
            // 
            // gbxStatus
            // 
            this.gbxStatus.Controls.Add(this.cboNewStatus);
            this.gbxStatus.Controls.Add(this.lblNewStatus);
            this.gbxStatus.Controls.Add(this.cboCurrentStatus);
            this.gbxStatus.Controls.Add(this.lblCurrentStatus);
            this.gbxStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxStatus.Location = new System.Drawing.Point(0, 0);
            this.gbxStatus.Name = "gbxStatus";
            this.gbxStatus.Size = new System.Drawing.Size(763, 41);
            this.gbxStatus.TabIndex = 0;
            this.gbxStatus.TabStop = false;
            this.gbxStatus.Text = "Status";
            // 
            // cboNewStatus
            // 
            this.cboNewStatus.FormattingEnabled = true;
            this.cboNewStatus.Location = new System.Drawing.Point(497, 14);
            this.cboNewStatus.Name = "cboNewStatus";
            this.cboNewStatus.Size = new System.Drawing.Size(121, 21);
            this.cboNewStatus.TabIndex = 5;
            this.cboNewStatus.SelectedIndexChanged += new System.EventHandler(this.cboNewStatus_SelectedIndexChanged);
            this.cboNewStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCurrentStatus_KeyPress);
            // 
            // lblNewStatus
            // 
            this.lblNewStatus.Location = new System.Drawing.Point(362, 17);
            this.lblNewStatus.Name = "lblNewStatus";
            this.lblNewStatus.Size = new System.Drawing.Size(128, 15);
            this.lblNewStatus.TabIndex = 4;
            this.lblNewStatus.Text = "To new  Status :";
            this.lblNewStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboCurrentStatus
            // 
            this.cboCurrentStatus.FormattingEnabled = true;
            this.cboCurrentStatus.Location = new System.Drawing.Point(143, 14);
            this.cboCurrentStatus.Name = "cboCurrentStatus";
            this.cboCurrentStatus.Size = new System.Drawing.Size(121, 21);
            this.cboCurrentStatus.TabIndex = 3;
            this.cboCurrentStatus.SelectedIndexChanged += new System.EventHandler(this.cboCurrentStatus_SelectedIndexChanged);
            this.cboCurrentStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCurrentStatus_KeyPress);
            // 
            // lblCurrentStatus
            // 
            this.lblCurrentStatus.Location = new System.Drawing.Point(8, 17);
            this.lblCurrentStatus.Name = "lblCurrentStatus";
            this.lblCurrentStatus.Size = new System.Drawing.Size(128, 15);
            this.lblCurrentStatus.TabIndex = 2;
            this.lblCurrentStatus.Text = "Current Status :";
            this.lblCurrentStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxListOfJobTrips
            // 
            this.gbxListOfJobTrips.Controls.Add(this.dgvJobTrips);
            this.gbxListOfJobTrips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxListOfJobTrips.Location = new System.Drawing.Point(0, 178);
            this.gbxListOfJobTrips.Name = "gbxListOfJobTrips";
            this.gbxListOfJobTrips.Size = new System.Drawing.Size(763, 205);
            this.gbxListOfJobTrips.TabIndex = 1;
            this.gbxListOfJobTrips.TabStop = false;
            this.gbxListOfJobTrips.Text = "List of Job Trips";
            // 
            // dgvJobTrips
            // 
            this.dgvJobTrips.AllowUserToAddRows = false;
            this.dgvJobTrips.AllowUserToOrderColumns = true;
            this.dgvJobTrips.AllowUserToResizeRows = false;
            this.dgvJobTrips.AutoGenerateColumns = false;
            this.dgvJobTrips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvJobTrips.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Select,
            this.jobNoDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.StartStop,
            this.EndStop,
            this.StartDate,
            this.EndDate,
            this.subCon});
            this.dgvJobTrips.DataSource = this.bdsJobTrips;
            this.dgvJobTrips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvJobTrips.Location = new System.Drawing.Point(3, 16);
            this.dgvJobTrips.Name = "dgvJobTrips";
            this.dgvJobTrips.RowHeadersVisible = false;
            this.dgvJobTrips.RowHeadersWidth = 10;
            this.dgvJobTrips.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvJobTrips.Size = new System.Drawing.Size(757, 186);
            this.dgvJobTrips.TabIndex = 0;
            // 
            // Select
            // 
            this.Select.FalseValue = "F";
            this.Select.Frozen = true;
            this.Select.HeaderText = "";
            this.Select.Name = "Select";
            this.Select.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Select.TrueValue = "T";
            this.Select.Width = 50;
            // 
            // jobNoDataGridViewTextBoxColumn
            // 
            this.jobNoDataGridViewTextBoxColumn.DataPropertyName = "JobNo";
            this.jobNoDataGridViewTextBoxColumn.HeaderText = "JobNo";
            this.jobNoDataGridViewTextBoxColumn.Name = "jobNoDataGridViewTextBoxColumn";
            this.jobNoDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // StartStop
            // 
            this.StartStop.DataPropertyName = "StartStop";
            this.StartStop.HeaderText = "Origin";
            this.StartStop.Name = "StartStop";
            this.StartStop.ReadOnly = true;
            // 
            // EndStop
            // 
            this.EndStop.DataPropertyName = "EndStop";
            this.EndStop.HeaderText = "Destination";
            this.EndStop.Name = "EndStop";
            this.EndStop.ReadOnly = true;
            // 
            // StartDate
            // 
            this.StartDate.DataPropertyName = "StartDate";
            this.StartDate.HeaderText = "Start Date";
            this.StartDate.Name = "StartDate";
            this.StartDate.ReadOnly = true;
            // 
            // EndDate
            // 
            this.EndDate.DataPropertyName = "EndDate";
            this.EndDate.HeaderText = "End Date";
            this.EndDate.Name = "EndDate";
            this.EndDate.ReadOnly = true;
            // 
            // subCon
            // 
            this.subCon.DataPropertyName = "subCon";
            this.subCon.HeaderText = "Sub Con";
            this.subCon.Name = "subCon";
            // 
            // bdsJobTrips
            // 
            this.bdsJobTrips.DataSource = typeof(TruckJobTrip);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSaveStatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 383);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 29);
            this.panel1.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(161, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSaveStatus
            // 
            this.btnSaveStatus.Location = new System.Drawing.Point(41, 2);
            this.btnSaveStatus.Name = "btnSaveStatus";
            this.btnSaveStatus.Size = new System.Drawing.Size(113, 23);
            this.btnSaveStatus.TabIndex = 0;
            this.btnSaveStatus.Text = "Save Status";
            this.btnSaveStatus.UseVisualStyleBackColor = true;
            this.btnSaveStatus.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmChangeJobTripStatusSubCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 412);
            this.Controls.Add(this.gbxListOfJobTrips);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.panel1);
            this.Name = "FrmChangeJobTripStatusSubCon";
            this.Text = "Change JobTrip Status for SubCon";
            this.Load += new System.EventHandler(this.FrmChangeJobTripStatusSubCon_Load);
            this.pnlTop.ResumeLayout(false);
            this.gbxCriteria.ResumeLayout(false);
            this.gbxCriteria.PerformLayout();
            this.pnlByJobTripDate.ResumeLayout(false);
            this.pnlByJobNo.ResumeLayout(false);
            this.gbxStatus.ResumeLayout(false);
            this.gbxListOfJobTrips.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobTrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsJobTrips)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.GroupBox gbxStatus;
        private System.Windows.Forms.GroupBox gbxListOfJobTrips;
        private System.Windows.Forms.GroupBox gbxCriteria;
        private System.Windows.Forms.ComboBox cboNewStatus;
        private System.Windows.Forms.Label lblNewStatus;
        private System.Windows.Forms.ComboBox cboCurrentStatus;
        private System.Windows.Forms.Label lblCurrentStatus;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.Label lblDept;
        private System.Windows.Forms.DataGridView dgvJobTrips;
        private System.Windows.Forms.RadioButton rdbByJobTripDate;
        private System.Windows.Forms.RadioButton rdbByJobNo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSaveStatus;
        private System.Windows.Forms.Panel pnlByJobTripDate;
        private System.Windows.Forms.Panel pnlByJobNo;
        private System.Windows.Forms.ComboBox cboFromJobNo;
        private System.Windows.Forms.Label lblFromJobNo;
        private System.Windows.Forms.ComboBox cboToJobNo;
        private System.Windows.Forms.Label lblJobNoTo;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.BindingSource bdsJobTrips;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Select;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartStop;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndStop;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn subCon;
    }
}