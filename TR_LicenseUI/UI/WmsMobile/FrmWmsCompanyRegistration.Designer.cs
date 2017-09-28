namespace FMLicenseUI.Common.WmsMobile
{
    partial class FrmWmsCompanyRegistration
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
            this.gbxCompanyInfo = new System.Windows.Forms.GroupBox();
            this.chkExistingGroup = new System.Windows.Forms.CheckBox();
            this.txtCompanyLocalName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkShared = new System.Windows.Forms.CheckBox();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGroupLocalName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.cboExistingGroups = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnlCompanyGrid = new System.Windows.Forms.Panel();
            this.dgvCompanyList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sQLServerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dsCompany = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbcMobileDeviceLicense = new System.Windows.Forms.TabControl();
            this.tpRegisteredMobile = new System.Windows.Forms.TabPage();
            this.dgvDeviceList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AndroidID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dsDevices = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDeleteDevice = new System.Windows.Forms.Button();
            this.btnNewDevice = new System.Windows.Forms.Button();
            this.btnEditDevice = new System.Windows.Forms.Button();
            this.tpValidLicense = new System.Windows.Forms.TabPage();
            this.dgvLicenseList = new System.Windows.Forms.DataGridView();
            this.numOfLicenseDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateAddedDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.validFromDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.validToDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dsLicense = new System.Windows.Forms.BindingSource(this.components);
            this.pnlLicenseButtons = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.btnNewLicense = new System.Windows.Forms.Button();
            this.btnEditLicense = new System.Windows.Forms.Button();
            this.gbxCompanyInfo.SuspendLayout();
            this.pnlCompanyGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompanyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsCompany)).BeginInit();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tbcMobileDeviceLicense.SuspendLayout();
            this.tpRegisteredMobile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsDevices)).BeginInit();
            this.panel2.SuspendLayout();
            this.tpValidLicense.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLicenseList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsLicense)).BeginInit();
            this.pnlLicenseButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxCompanyInfo
            // 
            this.gbxCompanyInfo.Controls.Add(this.chkExistingGroup);
            this.gbxCompanyInfo.Controls.Add(this.txtCompanyLocalName);
            this.gbxCompanyInfo.Controls.Add(this.label3);
            this.gbxCompanyInfo.Controls.Add(this.txtDatabaseName);
            this.gbxCompanyInfo.Controls.Add(this.label4);
            this.gbxCompanyInfo.Controls.Add(this.chkShared);
            this.gbxCompanyInfo.Controls.Add(this.txtCompanyName);
            this.gbxCompanyInfo.Controls.Add(this.label6);
            this.gbxCompanyInfo.Controls.Add(this.txtGroupLocalName);
            this.gbxCompanyInfo.Controls.Add(this.label5);
            this.gbxCompanyInfo.Controls.Add(this.label1);
            this.gbxCompanyInfo.Controls.Add(this.txtGroupName);
            this.gbxCompanyInfo.Controls.Add(this.cboExistingGroups);
            this.gbxCompanyInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxCompanyInfo.Location = new System.Drawing.Point(0, 0);
            this.gbxCompanyInfo.Name = "gbxCompanyInfo";
            this.gbxCompanyInfo.Size = new System.Drawing.Size(572, 130);
            this.gbxCompanyInfo.TabIndex = 0;
            this.gbxCompanyInfo.TabStop = false;
            this.gbxCompanyInfo.Text = "Company Information";
            // 
            // chkExistingGroup
            // 
            this.chkExistingGroup.AutoSize = true;
            this.chkExistingGroup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExistingGroup.Location = new System.Drawing.Point(451, 60);
            this.chkExistingGroup.Name = "chkExistingGroup";
            this.chkExistingGroup.Size = new System.Drawing.Size(115, 17);
            this.chkExistingGroup.TabIndex = 20;
            this.chkExistingGroup.Text = "Use existing Group";
            this.chkExistingGroup.UseVisualStyleBackColor = true;
            this.chkExistingGroup.CheckedChanged += new System.EventHandler(this.chkExistingGroup_CheckedChanged);
            // 
            // txtCompanyLocalName
            // 
            this.txtCompanyLocalName.BackColor = System.Drawing.Color.White;
            this.txtCompanyLocalName.Location = new System.Drawing.Point(141, 102);
            this.txtCompanyLocalName.MaxLength = 50;
            this.txtCompanyLocalName.Name = "txtCompanyLocalName";
            this.txtCompanyLocalName.Size = new System.Drawing.Size(306, 20);
            this.txtCompanyLocalName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Company Name(Local) :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtDatabaseName.Location = new System.Drawing.Point(141, 15);
            this.txtDatabaseName.MaxLength = 50;
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(306, 20);
            this.txtDatabaseName.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(22, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Database Name :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkShared
            // 
            this.chkShared.AutoSize = true;
            this.chkShared.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShared.Checked = true;
            this.chkShared.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShared.Location = new System.Drawing.Point(451, 81);
            this.chkShared.Name = "chkShared";
            this.chkShared.Size = new System.Drawing.Size(114, 17);
            this.chkShared.TabIndex = 9;
            this.chkShared.Text = "Share with Group :";
            this.chkShared.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.chkShared.UseVisualStyleBackColor = true;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtCompanyName.Location = new System.Drawing.Point(141, 36);
            this.txtCompanyName.MaxLength = 50;
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(306, 20);
            this.txtCompanyName.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(4, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Company Name :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGroupLocalName
            // 
            this.txtGroupLocalName.BackColor = System.Drawing.Color.White;
            this.txtGroupLocalName.Location = new System.Drawing.Point(141, 81);
            this.txtGroupLocalName.MaxLength = 50;
            this.txtGroupLocalName.Name = "txtGroupLocalName";
            this.txtGroupLocalName.Size = new System.Drawing.Size(306, 20);
            this.txtGroupLocalName.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Group Name(Local) :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Group Name :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGroupName
            // 
            this.txtGroupName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtGroupName.Location = new System.Drawing.Point(141, 58);
            this.txtGroupName.MaxLength = 50;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(306, 20);
            this.txtGroupName.TabIndex = 1;
            // 
            // cboExistingGroups
            // 
            this.cboExistingGroups.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboExistingGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExistingGroups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboExistingGroups.FormattingEnabled = true;
            this.cboExistingGroups.Location = new System.Drawing.Point(142, 58);
            this.cboExistingGroups.Name = "cboExistingGroups";
            this.cboExistingGroups.Size = new System.Drawing.Size(306, 21);
            this.cboExistingGroups.TabIndex = 21;
            this.cboExistingGroups.SelectedIndexChanged += new System.EventHandler(this.cboExistingGroups_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(335, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(255, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(95, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(15, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(175, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlCompanyGrid
            // 
            this.pnlCompanyGrid.Controls.Add(this.dgvCompanyList);
            this.pnlCompanyGrid.Controls.Add(this.panel1);
            this.pnlCompanyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCompanyGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlCompanyGrid.Name = "pnlCompanyGrid";
            this.pnlCompanyGrid.Size = new System.Drawing.Size(572, 145);
            this.pnlCompanyGrid.TabIndex = 1;
            // 
            // dgvCompanyList
            // 
            this.dgvCompanyList.AllowUserToAddRows = false;
            this.dgvCompanyList.AllowUserToDeleteRows = false;
            this.dgvCompanyList.AutoGenerateColumns = false;
            this.dgvCompanyList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCompanyList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn6,
            this.sQLServerDataGridViewTextBoxColumn,
            this.dataGridViewCheckBoxColumn1});
            this.dgvCompanyList.DataSource = this.dsCompany;
            this.dgvCompanyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompanyList.Location = new System.Drawing.Point(0, 28);
            this.dgvCompanyList.Name = "dgvCompanyList";
            this.dgvCompanyList.ReadOnly = true;
            this.dgvCompanyList.RowHeadersWidth = 20;
            this.dgvCompanyList.Size = new System.Drawing.Size(572, 117);
            this.dgvCompanyList.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "GroupName";
            this.dataGridViewTextBoxColumn2.HeaderText = "GroupName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "CompanyName";
            this.dataGridViewTextBoxColumn3.HeaderText = "CompanyName";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "DatabaseName";
            this.dataGridViewTextBoxColumn6.HeaderText = "DatabaseName";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // sQLServerDataGridViewTextBoxColumn
            // 
            this.sQLServerDataGridViewTextBoxColumn.DataPropertyName = "SQLServer";
            this.sQLServerDataGridViewTextBoxColumn.HeaderText = "SQLServer";
            this.sQLServerDataGridViewTextBoxColumn.Name = "sQLServerDataGridViewTextBoxColumn";
            this.sQLServerDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "ShareWithGoup";
            this.dataGridViewCheckBoxColumn1.HeaderText = "ShareWithGoup";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // dsCompany
            // 
            this.dsCompany.DataSource = typeof(FMLicense.BLL.WmsMobile.WmsMobileCompany);
            this.dsCompany.CurrentChanged += new System.EventHandler(this.dsCompany_CurrentChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 28);
            this.panel1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 130);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlCompanyGrid);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbcMobileDeviceLicense);
            this.splitContainer1.Panel2MinSize = 100;
            this.splitContainer1.Size = new System.Drawing.Size(572, 310);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 2;
            // 
            // tbcMobileDeviceLicense
            // 
            this.tbcMobileDeviceLicense.Controls.Add(this.tpValidLicense);
            this.tbcMobileDeviceLicense.Controls.Add(this.tpRegisteredMobile);
            this.tbcMobileDeviceLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcMobileDeviceLicense.Location = new System.Drawing.Point(0, 0);
            this.tbcMobileDeviceLicense.Name = "tbcMobileDeviceLicense";
            this.tbcMobileDeviceLicense.SelectedIndex = 0;
            this.tbcMobileDeviceLicense.Size = new System.Drawing.Size(572, 161);
            this.tbcMobileDeviceLicense.TabIndex = 0;
            // 
            // tpRegisteredMobile
            // 
            this.tpRegisteredMobile.BackColor = System.Drawing.Color.Transparent;
            this.tpRegisteredMobile.Controls.Add(this.dgvDeviceList);
            this.tpRegisteredMobile.Controls.Add(this.panel2);
            this.tpRegisteredMobile.Location = new System.Drawing.Point(4, 22);
            this.tpRegisteredMobile.Name = "tpRegisteredMobile";
            this.tpRegisteredMobile.Padding = new System.Windows.Forms.Padding(3);
            this.tpRegisteredMobile.Size = new System.Drawing.Size(564, 135);
            this.tpRegisteredMobile.TabIndex = 0;
            this.tpRegisteredMobile.Text = "Mobile Device";
            this.tpRegisteredMobile.UseVisualStyleBackColor = true;
            // 
            // dgvDeviceList
            // 
            this.dgvDeviceList.AllowUserToAddRows = false;
            this.dgvDeviceList.AllowUserToDeleteRows = false;
            this.dgvDeviceList.AutoGenerateColumns = false;
            this.dgvDeviceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeviceList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn8,
            this.AndroidID,
            this.Brand,
            this.Model,
            this.DeviceName});
            this.dgvDeviceList.DataSource = this.dsDevices;
            this.dgvDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDeviceList.Location = new System.Drawing.Point(3, 32);
            this.dgvDeviceList.Name = "dgvDeviceList";
            this.dgvDeviceList.ReadOnly = true;
            this.dgvDeviceList.RowHeadersWidth = 20;
            this.dgvDeviceList.Size = new System.Drawing.Size(558, 100);
            this.dgvDeviceList.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "CompanyName";
            this.dataGridViewTextBoxColumn8.HeaderText = "CompanyName";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // AndroidID
            // 
            this.AndroidID.DataPropertyName = "AndroidID";
            this.AndroidID.HeaderText = "AndroidID";
            this.AndroidID.Name = "AndroidID";
            this.AndroidID.ReadOnly = true;
            // 
            // Brand
            // 
            this.Brand.DataPropertyName = "Brand";
            this.Brand.HeaderText = "Brand";
            this.Brand.Name = "Brand";
            this.Brand.ReadOnly = true;
            // 
            // Model
            // 
            this.Model.DataPropertyName = "Model";
            this.Model.HeaderText = "Model";
            this.Model.Name = "Model";
            this.Model.ReadOnly = true;
            // 
            // DeviceName
            // 
            this.DeviceName.DataPropertyName = "DeviceName";
            this.DeviceName.HeaderText = "DeviceName";
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.ReadOnly = true;
            // 
            // dsDevices
            // 
            this.dsDevices.DataSource = typeof(FMLicense.BLL.WmsMobile.WmsMobileDevice);
            this.dsDevices.CurrentChanged += new System.EventHandler(this.dsDevices_CurrentChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDeleteDevice);
            this.panel2.Controls.Add(this.btnNewDevice);
            this.panel2.Controls.Add(this.btnEditDevice);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(558, 29);
            this.panel2.TabIndex = 4;
            // 
            // btnDeleteDevice
            // 
            this.btnDeleteDevice.Location = new System.Drawing.Point(188, 3);
            this.btnDeleteDevice.Name = "btnDeleteDevice";
            this.btnDeleteDevice.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteDevice.TabIndex = 24;
            this.btnDeleteDevice.Text = "Delete";
            this.btnDeleteDevice.UseVisualStyleBackColor = true;
            this.btnDeleteDevice.Click += new System.EventHandler(this.btnDeleteDevice_Click);
            // 
            // btnNewDevice
            // 
            this.btnNewDevice.Location = new System.Drawing.Point(28, 3);
            this.btnNewDevice.Name = "btnNewDevice";
            this.btnNewDevice.Size = new System.Drawing.Size(75, 23);
            this.btnNewDevice.TabIndex = 22;
            this.btnNewDevice.Text = "New";
            this.btnNewDevice.UseVisualStyleBackColor = true;
            this.btnNewDevice.Click += new System.EventHandler(this.btnNewDevice_Click);
            // 
            // btnEditDevice
            // 
            this.btnEditDevice.Location = new System.Drawing.Point(108, 3);
            this.btnEditDevice.Name = "btnEditDevice";
            this.btnEditDevice.Size = new System.Drawing.Size(75, 23);
            this.btnEditDevice.TabIndex = 23;
            this.btnEditDevice.Text = "Edit";
            this.btnEditDevice.UseVisualStyleBackColor = true;
            this.btnEditDevice.Click += new System.EventHandler(this.btnEditDevice_Click);
            // 
            // tpValidLicense
            // 
            this.tpValidLicense.BackColor = System.Drawing.Color.Transparent;
            this.tpValidLicense.Controls.Add(this.dgvLicenseList);
            this.tpValidLicense.Controls.Add(this.pnlLicenseButtons);
            this.tpValidLicense.Location = new System.Drawing.Point(4, 22);
            this.tpValidLicense.Name = "tpValidLicense";
            this.tpValidLicense.Padding = new System.Windows.Forms.Padding(3);
            this.tpValidLicense.Size = new System.Drawing.Size(564, 135);
            this.tpValidLicense.TabIndex = 1;
            this.tpValidLicense.Text = "Valid License";
            this.tpValidLicense.UseVisualStyleBackColor = true;
            // 
            // dgvLicenseList
            // 
            this.dgvLicenseList.AllowUserToAddRows = false;
            this.dgvLicenseList.AutoGenerateColumns = false;
            this.dgvLicenseList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLicenseList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numOfLicenseDataGridViewTextBoxColumn1,
            this.dateAddedDataGridViewTextBoxColumn1,
            this.validFromDataGridViewTextBoxColumn1,
            this.validToDataGridViewTextBoxColumn1});
            this.dgvLicenseList.DataSource = this.dsLicense;
            this.dgvLicenseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLicenseList.Location = new System.Drawing.Point(3, 31);
            this.dgvLicenseList.Name = "dgvLicenseList";
            this.dgvLicenseList.ReadOnly = true;
            this.dgvLicenseList.RowHeadersVisible = false;
            this.dgvLicenseList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLicenseList.Size = new System.Drawing.Size(558, 101);
            this.dgvLicenseList.TabIndex = 6;
            // 
            // numOfLicenseDataGridViewTextBoxColumn1
            // 
            this.numOfLicenseDataGridViewTextBoxColumn1.DataPropertyName = "numOfLicense";
            this.numOfLicenseDataGridViewTextBoxColumn1.HeaderText = "numOfLicense";
            this.numOfLicenseDataGridViewTextBoxColumn1.Name = "numOfLicenseDataGridViewTextBoxColumn1";
            this.numOfLicenseDataGridViewTextBoxColumn1.ReadOnly = true;
            this.numOfLicenseDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dateAddedDataGridViewTextBoxColumn1
            // 
            this.dateAddedDataGridViewTextBoxColumn1.DataPropertyName = "dateAdded";
            this.dateAddedDataGridViewTextBoxColumn1.HeaderText = "dateAdded";
            this.dateAddedDataGridViewTextBoxColumn1.Name = "dateAddedDataGridViewTextBoxColumn1";
            this.dateAddedDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // validFromDataGridViewTextBoxColumn1
            // 
            this.validFromDataGridViewTextBoxColumn1.DataPropertyName = "validFrom";
            this.validFromDataGridViewTextBoxColumn1.HeaderText = "validFrom";
            this.validFromDataGridViewTextBoxColumn1.Name = "validFromDataGridViewTextBoxColumn1";
            this.validFromDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // validToDataGridViewTextBoxColumn1
            // 
            this.validToDataGridViewTextBoxColumn1.DataPropertyName = "validTo";
            this.validToDataGridViewTextBoxColumn1.HeaderText = "validTo";
            this.validToDataGridViewTextBoxColumn1.Name = "validToDataGridViewTextBoxColumn1";
            this.validToDataGridViewTextBoxColumn1.ReadOnly = true;
            this.validToDataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dsLicense
            // 
            this.dsLicense.DataSource = typeof(FMLicense.BLL.WmsMobile.WmsMobileNoOfLicense);
            this.dsLicense.CurrentChanged += new System.EventHandler(this.dsLicense_CurrentChanged);
            // 
            // pnlLicenseButtons
            // 
            this.pnlLicenseButtons.Controls.Add(this.label13);
            this.pnlLicenseButtons.Controls.Add(this.btnNewLicense);
            this.pnlLicenseButtons.Controls.Add(this.btnEditLicense);
            this.pnlLicenseButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLicenseButtons.Location = new System.Drawing.Point(3, 3);
            this.pnlLicenseButtons.Name = "pnlLicenseButtons";
            this.pnlLicenseButtons.Size = new System.Drawing.Size(558, 28);
            this.pnlLicenseButtons.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 7);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(170, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "List of license(s) for each company";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNewLicense
            // 
            this.btnNewLicense.Location = new System.Drawing.Point(338, 2);
            this.btnNewLicense.Name = "btnNewLicense";
            this.btnNewLicense.Size = new System.Drawing.Size(75, 23);
            this.btnNewLicense.TabIndex = 0;
            this.btnNewLicense.Text = "New";
            this.btnNewLicense.UseVisualStyleBackColor = true;
            this.btnNewLicense.Click += new System.EventHandler(this.btnNewLicense_Click);
            // 
            // btnEditLicense
            // 
            this.btnEditLicense.Location = new System.Drawing.Point(418, 2);
            this.btnEditLicense.Name = "btnEditLicense";
            this.btnEditLicense.Size = new System.Drawing.Size(75, 23);
            this.btnEditLicense.TabIndex = 1;
            this.btnEditLicense.Text = "Edit";
            this.btnEditLicense.UseVisualStyleBackColor = true;
            this.btnEditLicense.Click += new System.EventHandler(this.btnEditLicense_Click);
            // 
            // FrmWmsCompanyRegistration
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 440);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.gbxCompanyInfo);
            this.Name = "FrmWmsCompanyRegistration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Company Registration Form";
            this.Load += new System.EventHandler(this.FrmCompanyRegistration_Load);
            this.gbxCompanyInfo.ResumeLayout(false);
            this.gbxCompanyInfo.PerformLayout();
            this.pnlCompanyGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompanyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsCompany)).EndInit();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tbcMobileDeviceLicense.ResumeLayout(false);
            this.tpRegisteredMobile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsDevices)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tpValidLicense.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLicenseList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsLicense)).EndInit();
            this.pnlLicenseButtons.ResumeLayout(false);
            this.pnlLicenseButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxCompanyInfo;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGroupLocalName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkShared;
        private System.Windows.Forms.TextBox txtCompanyLocalName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel pnlCompanyGrid;
        private System.Windows.Forms.DataGridView dgvCompanyList;
        private System.Windows.Forms.BindingSource dsCompany;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameLocalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyNameLocalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn databaseNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfLicenseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn shareWithGoupDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn webServiceIPDataGridViewTextBoxColumn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tbcMobileDeviceLicense;
        private System.Windows.Forms.TabPage tpRegisteredMobile;
        private System.Windows.Forms.TabPage tpValidLicense;
        private System.Windows.Forms.Panel pnlLicenseButtons;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnNewLicense;
        private System.Windows.Forms.Button btnEditLicense;
        private System.Windows.Forms.DataGridView dgvLicenseList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDeleteDevice;
        private System.Windows.Forms.Button btnNewDevice;
        private System.Windows.Forms.Button btnEditDevice;
        private System.Windows.Forms.DataGridView dgvDeviceList;
        private System.Windows.Forms.BindingSource dsDevices;
        private System.Windows.Forms.BindingSource dsLicense;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn AndroidID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn numOfLicenseDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateAddedDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn validFromDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn validToDataGridViewTextBoxColumn1;
        private System.Windows.Forms.CheckBox chkExistingGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn sQLServerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.ComboBox cboExistingGroups;
    }
}