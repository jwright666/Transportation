using FMLicense.BLL.Transport;
namespace FMLicenseUI.Common.Transport
{
    partial class FrmTPTCompanyLicense
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

        private System.Windows.Forms.GroupBox gbxCompanyInfo;
        private System.Windows.Forms.CheckBox chkExistingGroup;
        private System.Windows.Forms.TextBox txtCompanyLocalName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkShared;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGroupLocalName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.ComboBox cboExistingGroups;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvCompanyList;
        private System.Windows.Forms.Panel pnlCompanyButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvLicenseList;
        private System.Windows.Forms.Panel pnlLicenseButtons;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnNewLicense;
        private System.Windows.Forms.Button btnEditLicense;

       

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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvCompanyList = new System.Windows.Forms.DataGridView();
            this.bdsCompany = new System.Windows.Forms.BindingSource(this.components);
            this.pnlCompanyButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvLicenseList = new System.Windows.Forms.DataGridView();
            this.numOfLicenseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateAddedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.validFromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.validToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bdsLicense = new System.Windows.Forms.BindingSource(this.components);
            this.pnlLicenseButtons = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.btnNewLicense = new System.Windows.Forms.Button();
            this.btnEditLicense = new System.Windows.Forms.Button();
            this.groupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.databaseNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sQLServerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shareWithGoupDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gbxCompanyInfo.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompanyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsCompany)).BeginInit();
            this.pnlCompanyButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLicenseList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsLicense)).BeginInit();
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
            this.gbxCompanyInfo.Size = new System.Drawing.Size(535, 128);
            this.gbxCompanyInfo.TabIndex = 3;
            this.gbxCompanyInfo.TabStop = false;
            this.gbxCompanyInfo.Text = "Company Information";
            // 
            // chkExistingGroup
            // 
            this.chkExistingGroup.AutoSize = true;
            this.chkExistingGroup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExistingGroup.Location = new System.Drawing.Point(352, 60);
            this.chkExistingGroup.Name = "chkExistingGroup";
            this.chkExistingGroup.Size = new System.Drawing.Size(115, 17);
            this.chkExistingGroup.TabIndex = 2;
            this.chkExistingGroup.Text = "Use existing Group";
            this.chkExistingGroup.UseVisualStyleBackColor = true;
            this.chkExistingGroup.CheckedChanged += new System.EventHandler(this.chkExistingGroup_CheckedChanged);
            // 
            // txtCompanyLocalName
            // 
            this.txtCompanyLocalName.BackColor = System.Drawing.Color.White;
            this.txtCompanyLocalName.Location = new System.Drawing.Point(141, 100);
            this.txtCompanyLocalName.MaxLength = 50;
            this.txtCompanyLocalName.Name = "txtCompanyLocalName";
            this.txtCompanyLocalName.Size = new System.Drawing.Size(326, 20);
            this.txtCompanyLocalName.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 104);
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
            this.txtDatabaseName.Size = new System.Drawing.Size(326, 20);
            this.txtDatabaseName.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 13);
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
            this.chkShared.Location = new System.Drawing.Point(353, 81);
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
            this.txtCompanyName.Size = new System.Drawing.Size(326, 20);
            this.txtCompanyName.TabIndex = 1;
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
            this.txtGroupLocalName.Location = new System.Drawing.Point(141, 79);
            this.txtGroupLocalName.MaxLength = 50;
            this.txtGroupLocalName.Name = "txtGroupLocalName";
            this.txtGroupLocalName.Size = new System.Drawing.Size(192, 20);
            this.txtGroupLocalName.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 83);
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
            this.txtGroupName.MaxLength = 20;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(192, 20);
            this.txtGroupName.TabIndex = 1;
            // 
            // cboExistingGroups
            // 
            this.cboExistingGroups.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboExistingGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExistingGroups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboExistingGroups.FormattingEnabled = true;
            this.cboExistingGroups.Location = new System.Drawing.Point(141, 58);
            this.cboExistingGroups.Name = "cboExistingGroups";
            this.cboExistingGroups.Size = new System.Drawing.Size(192, 21);
            this.cboExistingGroups.TabIndex = 3;
            this.cboExistingGroups.SelectedIndexChanged += new System.EventHandler(this.cboExistingGroups_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 128);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvCompanyList);
            this.splitContainer1.Panel1.Controls.Add(this.pnlCompanyButtons);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvLicenseList);
            this.splitContainer1.Panel2.Controls.Add(this.pnlLicenseButtons);
            this.splitContainer1.Panel2MinSize = 145;
            this.splitContainer1.Size = new System.Drawing.Size(535, 344);
            this.splitContainer1.SplitterDistance = 170;
            this.splitContainer1.TabIndex = 4;
            // 
            // dgvCompanyList
            // 
            this.dgvCompanyList.AllowUserToAddRows = false;
            this.dgvCompanyList.AutoGenerateColumns = false;
            this.dgvCompanyList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCompanyList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupNameDataGridViewTextBoxColumn,
            this.companyNameDataGridViewTextBoxColumn,
            this.databaseNameDataGridViewTextBoxColumn,
            this.sQLServerDataGridViewTextBoxColumn,
            this.shareWithGoupDataGridViewCheckBoxColumn});
            this.dgvCompanyList.DataSource = this.bdsCompany;
            this.dgvCompanyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCompanyList.Location = new System.Drawing.Point(0, 28);
            this.dgvCompanyList.Name = "dgvCompanyList";
            this.dgvCompanyList.ReadOnly = true;
            this.dgvCompanyList.RowHeadersVisible = false;
            this.dgvCompanyList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCompanyList.Size = new System.Drawing.Size(535, 142);
            this.dgvCompanyList.TabIndex = 0;
            // 
            // bdsCompany
            // 
            this.bdsCompany.DataSource = typeof(FMLicense.BLL.Transport.TPTCompany);
            this.bdsCompany.CurrentChanged += new System.EventHandler(this.bdsCompany_CurrentChanged);
            // 
            // pnlCompanyButtons
            // 
            this.pnlCompanyButtons.Controls.Add(this.btnRefresh);
            this.pnlCompanyButtons.Controls.Add(this.btnDelete);
            this.pnlCompanyButtons.Controls.Add(this.btnNew);
            this.pnlCompanyButtons.Controls.Add(this.btnCancel);
            this.pnlCompanyButtons.Controls.Add(this.btnEdit);
            this.pnlCompanyButtons.Controls.Add(this.btnSave);
            this.pnlCompanyButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCompanyButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlCompanyButtons.Name = "pnlCompanyButtons";
            this.pnlCompanyButtons.Size = new System.Drawing.Size(535, 28);
            this.pnlCompanyButtons.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(17, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(258, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(98, 2);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(418, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(178, 2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(338, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvLicenseList
            // 
            this.dgvLicenseList.AllowUserToAddRows = false;
            this.dgvLicenseList.AutoGenerateColumns = false;
            this.dgvLicenseList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLicenseList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numOfLicenseDataGridViewTextBoxColumn,
            this.dateAddedDataGridViewTextBoxColumn,
            this.validFromDataGridViewTextBoxColumn,
            this.validToDataGridViewTextBoxColumn});
            this.dgvLicenseList.DataSource = this.bdsLicense;
            this.dgvLicenseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLicenseList.Location = new System.Drawing.Point(0, 28);
            this.dgvLicenseList.Name = "dgvLicenseList";
            this.dgvLicenseList.ReadOnly = true;
            this.dgvLicenseList.RowHeadersVisible = false;
            this.dgvLicenseList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLicenseList.Size = new System.Drawing.Size(535, 142);
            this.dgvLicenseList.TabIndex = 5;
            // 
            // numOfLicenseDataGridViewTextBoxColumn
            // 
            this.numOfLicenseDataGridViewTextBoxColumn.DataPropertyName = "numOfLicense";
            this.numOfLicenseDataGridViewTextBoxColumn.HeaderText = "numOfLicense";
            this.numOfLicenseDataGridViewTextBoxColumn.Name = "numOfLicenseDataGridViewTextBoxColumn";
            this.numOfLicenseDataGridViewTextBoxColumn.ReadOnly = true;
            this.numOfLicenseDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // dateAddedDataGridViewTextBoxColumn
            // 
            this.dateAddedDataGridViewTextBoxColumn.DataPropertyName = "dateAdded";
            this.dateAddedDataGridViewTextBoxColumn.HeaderText = "dateAdded";
            this.dateAddedDataGridViewTextBoxColumn.Name = "dateAddedDataGridViewTextBoxColumn";
            this.dateAddedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // validFromDataGridViewTextBoxColumn
            // 
            this.validFromDataGridViewTextBoxColumn.DataPropertyName = "validFrom";
            this.validFromDataGridViewTextBoxColumn.HeaderText = "validFrom";
            this.validFromDataGridViewTextBoxColumn.Name = "validFromDataGridViewTextBoxColumn";
            this.validFromDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // validToDataGridViewTextBoxColumn
            // 
            this.validToDataGridViewTextBoxColumn.DataPropertyName = "validTo";
            this.validToDataGridViewTextBoxColumn.HeaderText = "validTo";
            this.validToDataGridViewTextBoxColumn.Name = "validToDataGridViewTextBoxColumn";
            this.validToDataGridViewTextBoxColumn.ReadOnly = true;
            this.validToDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // bdsLicense
            // 
            this.bdsLicense.DataSource = typeof(FMLicense.BLL.Transport.TPTVehicleNoOfLicense);
            this.bdsLicense.CurrentChanged += new System.EventHandler(this.bdsLicense_CurrentChanged);
            // 
            // pnlLicenseButtons
            // 
            this.pnlLicenseButtons.Controls.Add(this.label7);
            this.pnlLicenseButtons.Controls.Add(this.btnNewLicense);
            this.pnlLicenseButtons.Controls.Add(this.btnEditLicense);
            this.pnlLicenseButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLicenseButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlLicenseButtons.Name = "pnlLicenseButtons";
            this.pnlLicenseButtons.Size = new System.Drawing.Size(535, 28);
            this.pnlLicenseButtons.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "List of license(s) for each company";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // groupNameDataGridViewTextBoxColumn
            // 
            this.groupNameDataGridViewTextBoxColumn.DataPropertyName = "GroupName";
            this.groupNameDataGridViewTextBoxColumn.HeaderText = "GroupName";
            this.groupNameDataGridViewTextBoxColumn.Name = "groupNameDataGridViewTextBoxColumn";
            this.groupNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // companyNameDataGridViewTextBoxColumn
            // 
            this.companyNameDataGridViewTextBoxColumn.DataPropertyName = "CompanyName";
            this.companyNameDataGridViewTextBoxColumn.HeaderText = "CompanyName";
            this.companyNameDataGridViewTextBoxColumn.Name = "companyNameDataGridViewTextBoxColumn";
            this.companyNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // databaseNameDataGridViewTextBoxColumn
            // 
            this.databaseNameDataGridViewTextBoxColumn.DataPropertyName = "DatabaseName";
            this.databaseNameDataGridViewTextBoxColumn.HeaderText = "DatabaseName";
            this.databaseNameDataGridViewTextBoxColumn.Name = "databaseNameDataGridViewTextBoxColumn";
            this.databaseNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sQLServerDataGridViewTextBoxColumn
            // 
            this.sQLServerDataGridViewTextBoxColumn.DataPropertyName = "SQLServer";
            this.sQLServerDataGridViewTextBoxColumn.HeaderText = "SQLServer";
            this.sQLServerDataGridViewTextBoxColumn.Name = "sQLServerDataGridViewTextBoxColumn";
            this.sQLServerDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // shareWithGoupDataGridViewCheckBoxColumn
            // 
            this.shareWithGoupDataGridViewCheckBoxColumn.DataPropertyName = "ShareWithGoup";
            this.shareWithGoupDataGridViewCheckBoxColumn.HeaderText = "ShareWithGoup";
            this.shareWithGoupDataGridViewCheckBoxColumn.Name = "shareWithGoupDataGridViewCheckBoxColumn";
            this.shareWithGoupDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // FrmTPTCompanyLicense
            // 
            this.ClientSize = new System.Drawing.Size(535, 472);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.gbxCompanyInfo);
            this.Name = "FrmTPTCompanyLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transport Vehicle License";
            this.Shown += new System.EventHandler(this.FrmTPTCompanyLicense_Shown);
            this.gbxCompanyInfo.ResumeLayout(false);
            this.gbxCompanyInfo.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompanyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsCompany)).EndInit();
            this.pnlCompanyButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLicenseList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsLicense)).EndInit();
            this.pnlLicenseButtons.ResumeLayout(false);
            this.pnlLicenseButtons.PerformLayout();
            this.ResumeLayout(false);

        }       


        #endregion

        private System.Windows.Forms.BindingSource bdsCompany;
        private System.Windows.Forms.BindingSource bdsLicense;
        private System.Windows.Forms.DataGridViewTextBoxColumn numOfLicenseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateAddedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn validFromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn validToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn databaseNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sQLServerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn shareWithGoupDataGridViewCheckBoxColumn;


    }
}