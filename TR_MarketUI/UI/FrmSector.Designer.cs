namespace FM.TransportMarket.UI
{
    partial class FrmSector
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
            this.dgvMaster = new System.Windows.Forms.DataGridView();
            this.txtSectorCode = new System.Windows.Forms.TextBox();
            this.lblSectorCode = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.sectorCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sectorDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4.SuspendLayout();
            this.tbcEntry.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlQuery.SuspendLayout();
            this.tabPageMaster.SuspendLayout();
            this.pnlMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // btnDeleteMaster
            // 
            this.btnDeleteMaster.Click += new System.EventHandler(this.btnDeleteMaster_Click);
            // 
            // btnEditMaster
            // 
            this.btnEditMaster.Click += new System.EventHandler(this.btnEditMaster_Click);
            // 
            // btnNewMaster
            // 
            this.btnNewMaster.Click += new System.EventHandler(this.btnNewMaster_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(224, 19);
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.Controls.Add(this.dgvMaster);
            this.tabPageSearch.Controls.SetChildIndex(this.pnlQuery, 0);
            this.tabPageSearch.Controls.SetChildIndex(this.dgvMaster, 0);
            // 
            // pnlQuery
            // 
            this.pnlQuery.Size = new System.Drawing.Size(649, 57);
            // 
            // pnlMaster
            // 
            this.pnlMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.pnlMaster.Controls.Add(this.lblDescription);
            this.pnlMaster.Controls.Add(this.txtDesc);
            this.pnlMaster.Controls.Add(this.lblSectorCode);
            this.pnlMaster.Controls.Add(this.txtSectorCode);
            // 
            // bdsMaster
            // 
            this.bdsMaster.DataSource = typeof(FM.TransportMaintenanceDLL.BLL.Sector);
            this.bdsMaster.CurrentChanged += new System.EventHandler(this.bdsMaster_CurrentChanged);
            // 
            // dgvMaster
            // 
            this.dgvMaster.AllowUserToAddRows = false;
            this.dgvMaster.AllowUserToDeleteRows = false;
            this.dgvMaster.AutoGenerateColumns = false;
            this.dgvMaster.BackgroundColor = System.Drawing.Color.DodgerBlue;
            this.dgvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sectorCodeDataGridViewTextBoxColumn,
            this.sectorDescriptionDataGridViewTextBoxColumn});
            this.dgvMaster.DataSource = this.bdsMaster;
            this.dgvMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMaster.Location = new System.Drawing.Point(3, 60);
            this.dgvMaster.Name = "dgvMaster";
            this.dgvMaster.ReadOnly = true;
            this.dgvMaster.Size = new System.Drawing.Size(649, 299);
            this.dgvMaster.TabIndex = 4;
            // 
            // txtSectorCode
            // 
            this.txtSectorCode.Location = new System.Drawing.Point(135, 36);
            this.txtSectorCode.Name = "txtSectorCode";
            this.txtSectorCode.Size = new System.Drawing.Size(135, 20);
            this.txtSectorCode.TabIndex = 0;
            // 
            // lblSectorCode
            // 
            this.lblSectorCode.Location = new System.Drawing.Point(9, 39);
            this.lblSectorCode.Name = "lblSectorCode";
            this.lblSectorCode.Size = new System.Drawing.Size(120, 13);
            this.lblSectorCode.TabIndex = 1;
            this.lblSectorCode.Text = "Sector Code :";
            this.lblSectorCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(9, 65);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(120, 13);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description :";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(135, 62);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(194, 84);
            this.txtDesc.TabIndex = 2;
            // 
            // sectorCodeDataGridViewTextBoxColumn
            // 
            this.sectorCodeDataGridViewTextBoxColumn.DataPropertyName = "SectorCode";
            this.sectorCodeDataGridViewTextBoxColumn.HeaderText = "SectorCode";
            this.sectorCodeDataGridViewTextBoxColumn.Name = "sectorCodeDataGridViewTextBoxColumn";
            this.sectorCodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sectorDescriptionDataGridViewTextBoxColumn
            // 
            this.sectorDescriptionDataGridViewTextBoxColumn.DataPropertyName = "SectorDescription";
            this.sectorDescriptionDataGridViewTextBoxColumn.HeaderText = "SectorDescription";
            this.sectorDescriptionDataGridViewTextBoxColumn.Name = "sectorDescriptionDataGridViewTextBoxColumn";
            this.sectorDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FrmSector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(663, 482);
            this.Name = "FrmSector";
            this.Text = "Sector Entry";
            this.Load += new System.EventHandler(this.FrmSector_Load);
            this.groupBox4.ResumeLayout(false);
            this.tbcEntry.ResumeLayout(false);
            this.tabPageSearch.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlQuery.ResumeLayout(false);
            this.tabPageMaster.ResumeLayout(false);
            this.pnlMaster.ResumeLayout(false);
            this.pnlMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectorCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectorDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblSectorCode;
        private System.Windows.Forms.TextBox txtSectorCode;

    }
}
