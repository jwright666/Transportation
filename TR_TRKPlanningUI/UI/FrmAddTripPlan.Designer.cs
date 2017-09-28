using FM.TR_TRKBookDLL.BLL;
namespace FM.TR_TRKPlanningUI.UI
{
    partial class FrmAddTripPlan
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbxCargoDetails = new System.Windows.Forms.GroupBox();
            this.dgvTruckJobTripDetails = new System.Windows.Forms.DataGridView();
            this.uomDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitWeightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.widthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.heightDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.markingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Add_Remove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.bdsTruckJobTripDetail = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtTruckCapacity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.lblStart = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblWeight = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.cboDriverCode = new System.Windows.Forms.ComboBox();
            this.lblDriver = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxCargoDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTruckJobTripDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsTruckJobTripDetail)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(165, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gbxCargoDetails
            // 
            this.gbxCargoDetails.Controls.Add(this.dgvTruckJobTripDetails);
            this.gbxCargoDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxCargoDetails.Location = new System.Drawing.Point(0, 122);
            this.gbxCargoDetails.Name = "gbxCargoDetails";
            this.gbxCargoDetails.Size = new System.Drawing.Size(522, 136);
            this.gbxCargoDetails.TabIndex = 101;
            this.gbxCargoDetails.TabStop = false;
            this.gbxCargoDetails.Text = "Cargo Details";
            // 
            // dgvTruckJobTripDetails
            // 
            this.dgvTruckJobTripDetails.AllowUserToAddRows = false;
            this.dgvTruckJobTripDetails.AllowUserToDeleteRows = false;
            this.dgvTruckJobTripDetails.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTruckJobTripDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTruckJobTripDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTruckJobTripDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.uomDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.unitWeightDataGridViewTextBoxColumn,
            this.lengthDataGridViewTextBoxColumn,
            this.widthDataGridViewTextBoxColumn,
            this.heightDataGridViewTextBoxColumn,
            this.markingDataGridViewTextBoxColumn,
            this.Add_Remove});
            this.dgvTruckJobTripDetails.DataSource = this.bdsTruckJobTripDetail;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTruckJobTripDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTruckJobTripDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTruckJobTripDetails.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTruckJobTripDetails.Location = new System.Drawing.Point(3, 16);
            this.dgvTruckJobTripDetails.Name = "dgvTruckJobTripDetails";
            this.dgvTruckJobTripDetails.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTruckJobTripDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTruckJobTripDetails.RowHeadersVisible = false;
            this.dgvTruckJobTripDetails.RowHeadersWidth = 20;
            this.dgvTruckJobTripDetails.Size = new System.Drawing.Size(516, 117);
            this.dgvTruckJobTripDetails.TabIndex = 100;
            this.dgvTruckJobTripDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTruckJobTripDetails_CellContentClick);
            // 
            // uomDataGridViewTextBoxColumn
            // 
            this.uomDataGridViewTextBoxColumn.DataPropertyName = "uom";
            this.uomDataGridViewTextBoxColumn.HeaderText = "Uom";
            this.uomDataGridViewTextBoxColumn.Name = "uomDataGridViewTextBoxColumn";
            this.uomDataGridViewTextBoxColumn.ReadOnly = true;
            this.uomDataGridViewTextBoxColumn.Width = 40;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "balQty";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Bal. Qty";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.quantityDataGridViewTextBoxColumn.Width = 50;
            // 
            // unitWeightDataGridViewTextBoxColumn
            // 
            this.unitWeightDataGridViewTextBoxColumn.DataPropertyName = "unitWeight";
            this.unitWeightDataGridViewTextBoxColumn.HeaderText = "unit Wt.(kg)";
            this.unitWeightDataGridViewTextBoxColumn.Name = "unitWeightDataGridViewTextBoxColumn";
            this.unitWeightDataGridViewTextBoxColumn.ReadOnly = true;
            this.unitWeightDataGridViewTextBoxColumn.Width = 60;
            // 
            // lengthDataGridViewTextBoxColumn
            // 
            this.lengthDataGridViewTextBoxColumn.DataPropertyName = "length";
            this.lengthDataGridViewTextBoxColumn.HeaderText = "length(cm)";
            this.lengthDataGridViewTextBoxColumn.Name = "lengthDataGridViewTextBoxColumn";
            this.lengthDataGridViewTextBoxColumn.ReadOnly = true;
            this.lengthDataGridViewTextBoxColumn.Width = 60;
            // 
            // widthDataGridViewTextBoxColumn
            // 
            this.widthDataGridViewTextBoxColumn.DataPropertyName = "width";
            this.widthDataGridViewTextBoxColumn.HeaderText = "width(cm)";
            this.widthDataGridViewTextBoxColumn.Name = "widthDataGridViewTextBoxColumn";
            this.widthDataGridViewTextBoxColumn.ReadOnly = true;
            this.widthDataGridViewTextBoxColumn.Width = 60;
            // 
            // heightDataGridViewTextBoxColumn
            // 
            this.heightDataGridViewTextBoxColumn.DataPropertyName = "height";
            this.heightDataGridViewTextBoxColumn.HeaderText = "height(cm)";
            this.heightDataGridViewTextBoxColumn.Name = "heightDataGridViewTextBoxColumn";
            this.heightDataGridViewTextBoxColumn.ReadOnly = true;
            this.heightDataGridViewTextBoxColumn.Width = 60;
            // 
            // markingDataGridViewTextBoxColumn
            // 
            this.markingDataGridViewTextBoxColumn.DataPropertyName = "marking";
            this.markingDataGridViewTextBoxColumn.HeaderText = "marking";
            this.markingDataGridViewTextBoxColumn.Name = "markingDataGridViewTextBoxColumn";
            this.markingDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Add_Remove
            // 
            this.Add_Remove.HeaderText = "Add/Remove";
            this.Add_Remove.Name = "Add_Remove";
            this.Add_Remove.ReadOnly = true;
            this.Add_Remove.Text = "Load";
            this.Add_Remove.UseColumnTextForButtonValue = true;
            this.Add_Remove.Width = 80;
            // 
            // bdsTruckJobTripDetail
            // 
            this.bdsTruckJobTripDetail.DataSource = typeof(TruckJobTripDetail);
            this.bdsTruckJobTripDetail.CurrentChanged += new System.EventHandler(this.bdsTruckJobTripDetail_CurrentChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtTruckCapacity);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.dtpStartDate);
            this.panel2.Controls.Add(this.dtpEndDate);
            this.panel2.Controls.Add(this.txtVolume);
            this.panel2.Controls.Add(this.lblStart);
            this.panel2.Controls.Add(this.txtWeight);
            this.panel2.Controls.Add(this.lblEnd);
            this.panel2.Controls.Add(this.lblVolume);
            this.panel2.Controls.Add(this.dtpStartTime);
            this.panel2.Controls.Add(this.lblWeight);
            this.panel2.Controls.Add(this.dtpEndTime);
            this.panel2.Controls.Add(this.cboDriverCode);
            this.panel2.Controls.Add(this.lblDriver);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(522, 122);
            this.panel2.TabIndex = 99;
            // 
            // txtTruckCapacity
            // 
            this.txtTruckCapacity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtTruckCapacity.Location = new System.Drawing.Point(72, 70);
            this.txtTruckCapacity.MaxLength = 12;
            this.txtTruckCapacity.Name = "txtTruckCapacity";
            this.txtTruckCapacity.ReadOnly = true;
            this.txtTruckCapacity.Size = new System.Drawing.Size(256, 20);
            this.txtTruckCapacity.TabIndex = 99;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 98;
            this.label1.Text = "Truck :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Enabled = false;
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(72, 3);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(127, 20);
            this.dtpStartDate.TabIndex = 86;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Enabled = false;
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(72, 26);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(127, 20);
            this.dtpEndDate.TabIndex = 87;
            // 
            // txtVolume
            // 
            this.txtVolume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtVolume.Location = new System.Drawing.Point(244, 92);
            this.txtVolume.MaxLength = 12;
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.ReadOnly = true;
            this.txtVolume.Size = new System.Drawing.Size(84, 20);
            this.txtVolume.TabIndex = 97;
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point(2, 7);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(68, 13);
            this.lblStart.TabIndex = 88;
            this.lblStart.Text = "Start :";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWeight
            // 
            this.txtWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtWeight.Location = new System.Drawing.Point(72, 92);
            this.txtWeight.MaxLength = 12;
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.ReadOnly = true;
            this.txtWeight.Size = new System.Drawing.Size(84, 20);
            this.txtWeight.TabIndex = 96;
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point(2, 30);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(68, 13);
            this.lblEnd.TabIndex = 89;
            this.lblEnd.Text = "End :";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVolume
            // 
            this.lblVolume.Location = new System.Drawing.Point(172, 96);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(68, 13);
            this.lblVolume.TabIndex = 95;
            this.lblVolume.Text = "Volume :";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTime.Location = new System.Drawing.Point(201, 3);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(127, 20);
            this.dtpStartTime.TabIndex = 90;
            // 
            // lblWeight
            // 
            this.lblWeight.Location = new System.Drawing.Point(2, 96);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(68, 13);
            this.lblWeight.TabIndex = 94;
            this.lblWeight.Text = "Weight :";
            this.lblWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTime.Location = new System.Drawing.Point(201, 26);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Size = new System.Drawing.Size(127, 20);
            this.dtpEndTime.TabIndex = 91;
            // 
            // cboDriverCode
            // 
            this.cboDriverCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDriverCode.FormattingEnabled = true;
            this.cboDriverCode.Location = new System.Drawing.Point(72, 48);
            this.cboDriverCode.Name = "cboDriverCode";
            this.cboDriverCode.Size = new System.Drawing.Size(256, 21);
            this.cboDriverCode.TabIndex = 93;
            this.cboDriverCode.SelectedIndexChanged += new System.EventHandler(this.cboDriverCode_SelectedIndexChanged);
            // 
            // lblDriver
            // 
            this.lblDriver.Location = new System.Drawing.Point(2, 52);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(68, 13);
            this.lblDriver.TabIndex = 92;
            this.lblDriver.Text = "Driver :";
            this.lblDriver.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 258);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(522, 33);
            this.panel1.TabIndex = 98;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(84, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmAddTripPlan
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(522, 291);
            this.Controls.Add(this.gbxCargoDetails);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmAddTripPlan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Plan Truck SubTrip";
            this.Shown += new System.EventHandler(this.FrmAddTripPlan_Load);
            this.gbxCargoDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTruckJobTripDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsTruckJobTripDetail)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboDriverCode;
        private System.Windows.Forms.Label lblDriver;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgvTruckJobTripDetails;
        public System.Windows.Forms.BindingSource bdsTruckJobTripDetail;
        private System.Windows.Forms.GroupBox gbxCargoDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn uomDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitWeightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn widthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn heightDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn markingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn Add_Remove;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtTruckCapacity;
        private System.Windows.Forms.Label label1;
    }
}