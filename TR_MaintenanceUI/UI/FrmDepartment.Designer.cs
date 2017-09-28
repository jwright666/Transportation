namespace FM.TR_MaintenanceUI.UI
{
    partial class FrmDepartment
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
            this.dgvSearch = new System.Windows.Forms.DataGridView();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblDeptCode = new System.Windows.Forms.Label();
            this.lblDeptName = new System.Windows.Forms.Label();
            this.lblDeptType = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lblErrorEdit = new System.Windows.Forms.Label();
            this.lblDeptCodeCriteria = new System.Windows.Forms.Label();
            this.cboDepartmentCode = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tptDeptCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tptDeptNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tptDeptTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4.SuspendLayout();
            this.tbcEntry.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlQuery.SuspendLayout();
            this.tabPageMaster.SuspendLayout();
            this.pnlMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.TabIndex = 1;
            // 
            // btnDeleteMaster
            // 
            this.btnDeleteMaster.Enabled = false;
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
            this.tbcEntry.TabIndex = 2;
            this.tbcEntry.Click += new System.EventHandler(this.tbcEntry_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(271, 9);
            this.btnQuery.Text = "Query ";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.Controls.Add(this.dgvSearch);
            this.tabPageSearch.Text = "Search ";
            this.tabPageSearch.Controls.SetChildIndex(this.pnlQuery, 0);
            this.tabPageSearch.Controls.SetChildIndex(this.dgvSearch, 0);
            // 
            // groupBox5
            // 
            this.pnlBottom.TabIndex = 0;
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
            this.pnlQuery.Controls.Add(this.lblDeptCodeCriteria);
            this.pnlQuery.Controls.Add(this.cboDepartmentCode);
            this.pnlQuery.Size = new System.Drawing.Size(649, 38);
            this.pnlQuery.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlQuery.Controls.SetChildIndex(this.cboDepartmentCode, 0);
            this.pnlQuery.Controls.SetChildIndex(this.lblDeptCodeCriteria, 0);
            this.pnlQuery.Controls.SetChildIndex(this.btnClear, 0);
            // 
            // tabPageMaster
            // 
            this.tabPageMaster.Text = "Master ";
            // 
            // pnlMaster
            // 
            this.pnlMaster.Controls.Add(this.lblErrorEdit);
            this.pnlMaster.Controls.Add(this.cboType);
            this.pnlMaster.Controls.Add(this.lblDeptType);
            this.pnlMaster.Controls.Add(this.lblDeptName);
            this.pnlMaster.Controls.Add(this.lblDeptCode);
            this.pnlMaster.Controls.Add(this.txtName);
            this.pnlMaster.Controls.Add(this.txtCode);
            // 
            // bdsMaster
            // 
            this.bdsMaster.DataSource = typeof(FM.TR_MaintenanceDLL.BLL.TptDept);
            this.bdsMaster.CurrentChanged += new System.EventHandler(this.bdsMaster_CurrentChanged);
            // 
            // btnClose
            // 
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close ";
            // 
            // dgvSearch
            // 
            this.dgvSearch.AllowUserToAddRows = false;
            this.dgvSearch.AllowUserToDeleteRows = false;
            this.dgvSearch.AutoGenerateColumns = false;
            this.dgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tptDeptCodeDataGridViewTextBoxColumn,
            this.tptDeptNameDataGridViewTextBoxColumn,
            this.tptDeptTypeDataGridViewTextBoxColumn});
            this.dgvSearch.DataSource = this.bdsMaster;
            this.dgvSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSearch.Location = new System.Drawing.Point(3, 41);
            this.dgvSearch.Name = "dgvSearch";
            this.dgvSearch.ReadOnly = true;
            this.dgvSearch.Size = new System.Drawing.Size(649, 318);
            this.dgvSearch.TabIndex = 4;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtCode.Location = new System.Drawing.Point(113, 29);
            this.txtCode.MaxLength = 12;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(122, 20);
            this.txtCode.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.txtName.Location = new System.Drawing.Point(113, 52);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(260, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblDeptCode
            // 
            this.lblDeptCode.AutoSize = true;
            this.lblDeptCode.Location = new System.Drawing.Point(20, 32);
            this.lblDeptCode.Name = "lblDeptCode";
            this.lblDeptCode.Size = new System.Drawing.Size(90, 13);
            this.lblDeptCode.TabIndex = 2;
            this.lblDeptCode.Text = "Department Code";
            // 
            // lblDeptName
            // 
            this.lblDeptName.AutoSize = true;
            this.lblDeptName.Location = new System.Drawing.Point(17, 55);
            this.lblDeptName.Name = "lblDeptName";
            this.lblDeptName.Size = new System.Drawing.Size(93, 13);
            this.lblDeptName.TabIndex = 3;
            this.lblDeptName.Text = "Department Name";
            // 
            // lblDeptType
            // 
            this.lblDeptType.AutoSize = true;
            this.lblDeptType.Location = new System.Drawing.Point(21, 80);
            this.lblDeptType.Name = "lblDeptType";
            this.lblDeptType.Size = new System.Drawing.Size(89, 13);
            this.lblDeptType.TabIndex = 4;
            this.lblDeptType.Text = "Department Type";
            // 
            // cboType
            // 
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(113, 76);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(121, 21);
            this.cboType.TabIndex = 2;
            // 
            // lblErrorEdit
            // 
            this.lblErrorEdit.AutoSize = true;
            this.lblErrorEdit.ForeColor = System.Drawing.Color.Red;
            this.lblErrorEdit.Location = new System.Drawing.Point(21, 136);
            this.lblErrorEdit.Name = "lblErrorEdit";
            this.lblErrorEdit.Size = new System.Drawing.Size(0, 13);
            this.lblErrorEdit.TabIndex = 6;
            // 
            // lblDeptCodeCriteria
            // 
            this.lblDeptCodeCriteria.AutoSize = true;
            this.lblDeptCodeCriteria.Location = new System.Drawing.Point(27, 14);
            this.lblDeptCodeCriteria.Name = "lblDeptCodeCriteria";
            this.lblDeptCodeCriteria.Size = new System.Drawing.Size(96, 13);
            this.lblDeptCodeCriteria.TabIndex = 7;
            this.lblDeptCodeCriteria.Text = "Department Code :";
            this.lblDeptCodeCriteria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboDepartmentCode
            // 
            this.cboDepartmentCode.BackColor = System.Drawing.Color.White;
            this.cboDepartmentCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDepartmentCode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cboDepartmentCode.FormattingEnabled = true;
            this.cboDepartmentCode.Location = new System.Drawing.Point(125, 11);
            this.cboDepartmentCode.Name = "cboDepartmentCode";
            this.cboDepartmentCode.Size = new System.Drawing.Size(121, 21);
            this.cboDepartmentCode.TabIndex = 6;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(352, 9);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tptDeptCodeDataGridViewTextBoxColumn
            // 
            this.tptDeptCodeDataGridViewTextBoxColumn.DataPropertyName = "TptDeptCode";
            this.tptDeptCodeDataGridViewTextBoxColumn.HeaderText = "TptDeptCode";
            this.tptDeptCodeDataGridViewTextBoxColumn.Name = "tptDeptCodeDataGridViewTextBoxColumn";
            this.tptDeptCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tptDeptNameDataGridViewTextBoxColumn
            // 
            this.tptDeptNameDataGridViewTextBoxColumn.DataPropertyName = "TptDeptName";
            this.tptDeptNameDataGridViewTextBoxColumn.HeaderText = "TptDeptName";
            this.tptDeptNameDataGridViewTextBoxColumn.Name = "tptDeptNameDataGridViewTextBoxColumn";
            this.tptDeptNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tptDeptTypeDataGridViewTextBoxColumn
            // 
            this.tptDeptTypeDataGridViewTextBoxColumn.DataPropertyName = "TptDeptType";
            this.tptDeptTypeDataGridViewTextBoxColumn.HeaderText = "TptDeptType";
            this.tptDeptTypeDataGridViewTextBoxColumn.Name = "tptDeptTypeDataGridViewTextBoxColumn";
            this.tptDeptTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FrmDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(663, 482);
            this.Name = "FrmDepartment";
            this.Text = "Department Maintenance";
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn tptDeptCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tptDeptNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblDeptName;
        private System.Windows.Forms.Label lblDeptCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblDeptType;
        private System.Windows.Forms.DataGridViewTextBoxColumn tptDeptTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblErrorEdit;
        private System.Windows.Forms.Label lblDeptCodeCriteria;
        private System.Windows.Forms.ComboBox cboDepartmentCode;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Button btnClear;
    }
}
