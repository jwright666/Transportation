
namespace FM.TransportMarket.UI
{
    partial class FrmPackages
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

        private System.Windows.Forms.GroupBox gbxPackageInfo;
        private System.Windows.Forms.TextBox txtPackageDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPackageType;
        private System.Windows.Forms.Label lblPackageType;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvPackageList;
        private System.Windows.Forms.Panel pnlCompanyButtons;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlLicenseButtons;
        private System.Windows.Forms.Label lblPackageUOM;
        private System.Windows.Forms.Button btnNewPackageUOM;
        private System.Windows.Forms.Button btnEditPackageUOM;

       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbxPackageInfo = new System.Windows.Forms.GroupBox();
            this.txtPackageDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPackageType = new System.Windows.Forms.TextBox();
            this.lblPackageType = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvPackageList = new System.Windows.Forms.DataGridView();
            this.bdsPackage = new System.Windows.Forms.BindingSource(this.components);
            this.pnlCompanyButtons = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvPackageUOMList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bdsPackageUOM = new System.Windows.Forms.BindingSource(this.components);
            this.pnlLicenseButtons = new System.Windows.Forms.Panel();
            this.btnDeletePackageUOM = new System.Windows.Forms.Button();
            this.lblPackageUOM = new System.Windows.Forms.Label();
            this.btnNewPackageUOM = new System.Windows.Forms.Button();
            this.btnEditPackageUOM = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbxPackageInfo.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPackage)).BeginInit();
            this.pnlCompanyButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageUOMList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPackageUOM)).BeginInit();
            this.pnlLicenseButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxPackageInfo
            // 
            this.gbxPackageInfo.Controls.Add(this.txtPackageDescription);
            this.gbxPackageInfo.Controls.Add(this.label3);
            this.gbxPackageInfo.Controls.Add(this.txtPackageType);
            this.gbxPackageInfo.Controls.Add(this.lblPackageType);
            this.gbxPackageInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxPackageInfo.Location = new System.Drawing.Point(0, 0);
            this.gbxPackageInfo.Name = "gbxPackageInfo";
            this.gbxPackageInfo.Size = new System.Drawing.Size(535, 68);
            this.gbxPackageInfo.TabIndex = 3;
            this.gbxPackageInfo.TabStop = false;
            this.gbxPackageInfo.Text = "Package Information";
            // 
            // txtPackageDescription
            // 
            this.txtPackageDescription.BackColor = System.Drawing.Color.White;
            this.txtPackageDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPackageDescription.Location = new System.Drawing.Point(141, 38);
            this.txtPackageDescription.MaxLength = 50;
            this.txtPackageDescription.Name = "txtPackageDescription";
            this.txtPackageDescription.Size = new System.Drawing.Size(326, 20);
            this.txtPackageDescription.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Package Description :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPackageType
            // 
            this.txtPackageType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.txtPackageType.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPackageType.Location = new System.Drawing.Point(141, 15);
            this.txtPackageType.MaxLength = 50;
            this.txtPackageType.Name = "txtPackageType";
            this.txtPackageType.Size = new System.Drawing.Size(112, 20);
            this.txtPackageType.TabIndex = 0;
            // 
            // lblPackageType
            // 
            this.lblPackageType.Location = new System.Drawing.Point(4, 19);
            this.lblPackageType.Name = "lblPackageType";
            this.lblPackageType.Size = new System.Drawing.Size(131, 13);
            this.lblPackageType.TabIndex = 17;
            this.lblPackageType.Text = "Package Type :";
            this.lblPackageType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 68);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvPackageList);
            this.splitContainer1.Panel1.Controls.Add(this.pnlCompanyButtons);
            this.splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvPackageUOMList);
            this.splitContainer1.Panel2.Controls.Add(this.pnlLicenseButtons);
            this.splitContainer1.Panel2MinSize = 145;
            this.splitContainer1.Size = new System.Drawing.Size(535, 404);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 4;
            // 
            // dgvPackageList
            // 
            this.dgvPackageList.AllowUserToAddRows = false;
            this.dgvPackageList.AutoGenerateColumns = false;
            this.dgvPackageList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPackageList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvPackageList.DataSource = this.bdsPackage;
            this.dgvPackageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackageList.Location = new System.Drawing.Point(0, 28);
            this.dgvPackageList.Name = "dgvPackageList";
            this.dgvPackageList.ReadOnly = true;
            this.dgvPackageList.RowHeadersVisible = false;
            this.dgvPackageList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPackageList.Size = new System.Drawing.Size(535, 171);
            this.dgvPackageList.TabIndex = 0;
            // 
            // bdsPackage
            // 
            this.bdsPackage.DataSource = typeof(FM.TransportMarket.BLL.Package);
            this.bdsPackage.CurrentChanged += new System.EventHandler(this.bdsPackage_CurrentChanged);
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
            // dgvPackageUOMList
            // 
            this.dgvPackageUOMList.AllowUserToAddRows = false;
            this.dgvPackageUOMList.AutoGenerateColumns = false;
            this.dgvPackageUOMList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPackageUOMList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            this.dgvPackageUOMList.DataSource = this.bdsPackageUOM;
            this.dgvPackageUOMList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPackageUOMList.Location = new System.Drawing.Point(0, 28);
            this.dgvPackageUOMList.Name = "dgvPackageUOMList";
            this.dgvPackageUOMList.ReadOnly = true;
            this.dgvPackageUOMList.RowHeadersVisible = false;
            this.dgvPackageUOMList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPackageUOMList.Size = new System.Drawing.Size(535, 173);
            this.dgvPackageUOMList.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "packType";
            this.dataGridViewTextBoxColumn4.HeaderText = "PackType";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "packUOMCode";
            this.dataGridViewTextBoxColumn3.HeaderText = "UOMCode";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "packUOMDescription";
            this.dataGridViewTextBoxColumn5.HeaderText = "UOMDescription";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "length";
            this.dataGridViewTextBoxColumn6.HeaderText = "Length";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "width";
            this.dataGridViewTextBoxColumn7.HeaderText = "Width";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "height";
            this.dataGridViewTextBoxColumn8.HeaderText = "Height";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "unitWeight";
            this.dataGridViewTextBoxColumn9.HeaderText = "UnitWeight";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // bdsPackageUOM
            // 
            this.bdsPackageUOM.DataSource = typeof(FM.TransportMarket.BLL.PackageUOM);
            this.bdsPackageUOM.CurrentChanged += new System.EventHandler(this.bdsPackageUOM_CurrentChanged);
            // 
            // pnlLicenseButtons
            // 
            this.pnlLicenseButtons.Controls.Add(this.btnDeletePackageUOM);
            this.pnlLicenseButtons.Controls.Add(this.lblPackageUOM);
            this.pnlLicenseButtons.Controls.Add(this.btnNewPackageUOM);
            this.pnlLicenseButtons.Controls.Add(this.btnEditPackageUOM);
            this.pnlLicenseButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLicenseButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlLicenseButtons.Name = "pnlLicenseButtons";
            this.pnlLicenseButtons.Size = new System.Drawing.Size(535, 28);
            this.pnlLicenseButtons.TabIndex = 4;
            // 
            // btnDeletePackageUOM
            // 
            this.btnDeletePackageUOM.Location = new System.Drawing.Point(444, 2);
            this.btnDeletePackageUOM.Name = "btnDeletePackageUOM";
            this.btnDeletePackageUOM.Size = new System.Drawing.Size(75, 23);
            this.btnDeletePackageUOM.TabIndex = 12;
            this.btnDeletePackageUOM.Text = "Delete";
            this.btnDeletePackageUOM.UseVisualStyleBackColor = true;
            this.btnDeletePackageUOM.Click += new System.EventHandler(this.btnDeletePackageUOM_Click);
            // 
            // lblPackageUOM
            // 
            this.lblPackageUOM.AutoSize = true;
            this.lblPackageUOM.Location = new System.Drawing.Point(12, 7);
            this.lblPackageUOM.Name = "lblPackageUOM";
            this.lblPackageUOM.Size = new System.Drawing.Size(93, 13);
            this.lblPackageUOM.TabIndex = 11;
            this.lblPackageUOM.Text = "Package UOM list";
            this.lblPackageUOM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnNewPackageUOM
            // 
            this.btnNewPackageUOM.Location = new System.Drawing.Point(283, 2);
            this.btnNewPackageUOM.Name = "btnNewPackageUOM";
            this.btnNewPackageUOM.Size = new System.Drawing.Size(75, 23);
            this.btnNewPackageUOM.TabIndex = 0;
            this.btnNewPackageUOM.Text = "New";
            this.btnNewPackageUOM.UseVisualStyleBackColor = true;
            this.btnNewPackageUOM.Click += new System.EventHandler(this.btnNewPackageUOM_Click);
            // 
            // btnEditPackageUOM
            // 
            this.btnEditPackageUOM.Location = new System.Drawing.Point(363, 2);
            this.btnEditPackageUOM.Name = "btnEditPackageUOM";
            this.btnEditPackageUOM.Size = new System.Drawing.Size(75, 23);
            this.btnEditPackageUOM.TabIndex = 1;
            this.btnEditPackageUOM.Text = "Edit";
            this.btnEditPackageUOM.UseVisualStyleBackColor = true;
            this.btnEditPackageUOM.Click += new System.EventHandler(this.btnEditPackageUOM_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "PackType";
            this.dataGridViewTextBoxColumn1.HeaderText = "PackType";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 81;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "PackDescription";
            this.dataGridViewTextBoxColumn2.HeaderText = "PackDescription";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 110;
            // 
            // FrmPackages
            // 
            this.ClientSize = new System.Drawing.Size(535, 472);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.gbxPackageInfo);
            this.Name = "FrmPackages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transport Vehicle License";
            this.Shown += new System.EventHandler(this.FrmPackages_Shown);
            this.gbxPackageInfo.ResumeLayout(false);
            this.gbxPackageInfo.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPackage)).EndInit();
            this.pnlCompanyButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPackageUOMList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsPackageUOM)).EndInit();
            this.pnlLicenseButtons.ResumeLayout(false);
            this.pnlLicenseButtons.PerformLayout();
            this.ResumeLayout(false);

        }       


        #endregion

        private System.Windows.Forms.DataGridView dgvPackageUOMList;
        private System.Windows.Forms.Button btnDeletePackageUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn packTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn packDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn packTypeDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn packUOMCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn widthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn heightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitWeightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn packUOMDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource bdsPackage;
        private System.Windows.Forms.BindingSource bdsPackageUOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;



    }
}