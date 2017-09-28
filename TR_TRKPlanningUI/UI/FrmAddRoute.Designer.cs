using FM.TR_TRKPlanDLL.BLL;
namespace FM.TR_TRKPlanningUI.UI
{
    partial class FrmAddRoute
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboVehicle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboDriver = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelEdit = new System.Windows.Forms.Button();
            this.btnDeleteSubTrip = new System.Windows.Forms.Button();
            this.btnSaveEditSubTrip = new System.Windows.Forms.Button();
            this.btnAddSubTrip = new System.Windows.Forms.Button();
            this.gbxEnd = new System.Windows.Forms.GroupBox();
            this.txtEndName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.txtEndCity = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEndAddress4 = new System.Windows.Forms.TextBox();
            this.txtEndAddress3 = new System.Windows.Forms.TextBox();
            this.txtEndAddress2 = new System.Windows.Forms.TextBox();
            this.txtEndAddress = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.gbxStart = new System.Windows.Forms.GroupBox();
            this.txtStartName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.txtStartCity = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStartAddress4 = new System.Windows.Forms.TextBox();
            this.txtStartAddress3 = new System.Windows.Forms.TextBox();
            this.txtStartAddress2 = new System.Windows.Forms.TextBox();
            this.txtStartAddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnUp = new System.Windows.Forms.Button();
            this.dgvSortedPlanRoutedSubTrips = new System.Windows.Forms.DataGridView();
            this.dgvUnSortedPlanRoutedSubTrips = new System.Windows.Forms.DataGridView();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnAddAllToUnsorted = new System.Windows.Forms.Button();
            this.btnAddAllToSorted = new System.Windows.Forms.Button();
            this.btnAddToUnsorted = new System.Windows.Forms.Button();
            this.btnAddToSorted = new System.Windows.Forms.Button();
            this.btnSaveRoute = new System.Windows.Forms.Button();
            this.btnCancelRoute = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.iplcboEndCode = new FormBaseLibrary.CommonControl.IPLComboBox();
            this.iplcboStartCode = new FormBaseLibrary.CommonControl.IPLComboBox();
            this.startDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startStopDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endStopDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vehicleNumberDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverNumberDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindSourceSortedRoutedPlanSubTrips = new System.Windows.Forms.BindingSource(this.components);
            this.startDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startStopDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endStopDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vehicleNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.driverNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindSourceUnSortedRoutedPlanSubTrips = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.gbxEnd.SuspendLayout();
            this.gbxStart.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSortedPlanRoutedSubTrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnSortedPlanRoutedSubTrips)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindSourceSortedRoutedPlanSubTrips)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindSourceUnSortedRoutedPlanSubTrips)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.lblDate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboVehicle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboDriver);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(995, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Driver and Vehicle Select";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(343, 19);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(133, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "(Please select a driver first)";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(630, 19);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(28, 13);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(538, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "This plan is for: ";
            // 
            // cboVehicle
            // 
            this.cboVehicle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.cboVehicle.Enabled = false;
            this.cboVehicle.FormattingEnabled = true;
            this.cboVehicle.Location = new System.Drawing.Point(75, 37);
            this.cboVehicle.Name = "cboVehicle";
            this.cboVehicle.Size = new System.Drawing.Size(156, 21);
            this.cboVehicle.TabIndex = 3;
            this.cboVehicle.SelectedIndexChanged += new System.EventHandler(this.cboVehicle_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Vehicle:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDriver
            // 
            this.cboDriver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboDriver.FormattingEnabled = true;
            this.cboDriver.Location = new System.Drawing.Point(75, 15);
            this.cboDriver.Name = "cboDriver";
            this.cboDriver.Size = new System.Drawing.Size(237, 21);
            this.cboDriver.TabIndex = 1;
            this.cboDriver.SelectedIndexChanged += new System.EventHandler(this.cboDriver_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Driver:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.Location = new System.Drawing.Point(235, 230);
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.Size = new System.Drawing.Size(87, 23);
            this.btnCancelEdit.TabIndex = 2;
            this.btnCancelEdit.Text = "Cancel Editing";
            this.btnCancelEdit.UseVisualStyleBackColor = true;
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            // 
            // btnDeleteSubTrip
            // 
            this.btnDeleteSubTrip.Location = new System.Drawing.Point(328, 231);
            this.btnDeleteSubTrip.Name = "btnDeleteSubTrip";
            this.btnDeleteSubTrip.Size = new System.Drawing.Size(147, 22);
            this.btnDeleteSubTrip.TabIndex = 1;
            this.btnDeleteSubTrip.Text = "Delete This Plan Sub Trip";
            this.btnDeleteSubTrip.UseVisualStyleBackColor = true;
            this.btnDeleteSubTrip.Click += new System.EventHandler(this.btnDeleteSubTrip_Click);
            // 
            // btnSaveEditSubTrip
            // 
            this.btnSaveEditSubTrip.Location = new System.Drawing.Point(132, 230);
            this.btnSaveEditSubTrip.Name = "btnSaveEditSubTrip";
            this.btnSaveEditSubTrip.Size = new System.Drawing.Size(97, 22);
            this.btnSaveEditSubTrip.TabIndex = 1;
            this.btnSaveEditSubTrip.Text = "Save Edited Plan SubTrip";
            this.btnSaveEditSubTrip.UseVisualStyleBackColor = true;
            this.btnSaveEditSubTrip.Click += new System.EventHandler(this.btnSaveEditSubTrip_Click);
            // 
            // btnAddSubTrip
            // 
            this.btnAddSubTrip.Location = new System.Drawing.Point(5, 230);
            this.btnAddSubTrip.Name = "btnAddSubTrip";
            this.btnAddSubTrip.Size = new System.Drawing.Size(121, 22);
            this.btnAddSubTrip.TabIndex = 1;
            this.btnAddSubTrip.Text = "Add Plan SubTrip";
            this.btnAddSubTrip.UseVisualStyleBackColor = true;
            this.btnAddSubTrip.Click += new System.EventHandler(this.btnAddSubTrip_Click);
            // 
            // gbxEnd
            // 
            this.gbxEnd.Controls.Add(this.txtEndName);
            this.gbxEnd.Controls.Add(this.label15);
            this.gbxEnd.Controls.Add(this.iplcboEndCode);
            this.gbxEnd.Controls.Add(this.label8);
            this.gbxEnd.Controls.Add(this.dtpEndTime);
            this.gbxEnd.Controls.Add(this.txtEndCity);
            this.gbxEnd.Controls.Add(this.label9);
            this.gbxEnd.Controls.Add(this.txtEndAddress4);
            this.gbxEnd.Controls.Add(this.txtEndAddress3);
            this.gbxEnd.Controls.Add(this.txtEndAddress2);
            this.gbxEnd.Controls.Add(this.txtEndAddress);
            this.gbxEnd.Controls.Add(this.label10);
            this.gbxEnd.Controls.Add(this.label11);
            this.gbxEnd.Location = new System.Drawing.Point(462, 9);
            this.gbxEnd.Name = "gbxEnd";
            this.gbxEnd.Size = new System.Drawing.Size(425, 203);
            this.gbxEnd.TabIndex = 0;
            this.gbxEnd.TabStop = false;
            this.gbxEnd.Text = "End";
            // 
            // txtEndName
            // 
            this.txtEndName.Location = new System.Drawing.Point(92, 41);
            this.txtEndName.Name = "txtEndName";
            this.txtEndName.Size = new System.Drawing.Size(325, 20);
            this.txtEndName.TabIndex = 11;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(27, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Name:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(27, 177);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "End Time:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Location = new System.Drawing.Point(92, 173);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(156, 20);
            this.dtpEndTime.TabIndex = 7;
            // 
            // txtEndCity
            // 
            this.txtEndCity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtEndCity.Location = new System.Drawing.Point(92, 151);
            this.txtEndCity.Name = "txtEndCity";
            this.txtEndCity.ReadOnly = true;
            this.txtEndCity.Size = new System.Drawing.Size(156, 20);
            this.txtEndCity.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(27, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "City:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEndAddress4
            // 
            this.txtEndAddress4.Location = new System.Drawing.Point(92, 129);
            this.txtEndAddress4.Name = "txtEndAddress4";
            this.txtEndAddress4.Size = new System.Drawing.Size(325, 20);
            this.txtEndAddress4.TabIndex = 4;
            // 
            // txtEndAddress3
            // 
            this.txtEndAddress3.Location = new System.Drawing.Point(92, 107);
            this.txtEndAddress3.Name = "txtEndAddress3";
            this.txtEndAddress3.Size = new System.Drawing.Size(325, 20);
            this.txtEndAddress3.TabIndex = 4;
            // 
            // txtEndAddress2
            // 
            this.txtEndAddress2.Location = new System.Drawing.Point(92, 85);
            this.txtEndAddress2.Name = "txtEndAddress2";
            this.txtEndAddress2.Size = new System.Drawing.Size(325, 20);
            this.txtEndAddress2.TabIndex = 4;
            // 
            // txtEndAddress
            // 
            this.txtEndAddress.Location = new System.Drawing.Point(92, 63);
            this.txtEndAddress.Name = "txtEndAddress";
            this.txtEndAddress.Size = new System.Drawing.Size(325, 20);
            this.txtEndAddress.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(27, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Address:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(27, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Code:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxStart
            // 
            this.gbxStart.Controls.Add(this.txtStartName);
            this.gbxStart.Controls.Add(this.label16);
            this.gbxStart.Controls.Add(this.iplcboStartCode);
            this.gbxStart.Controls.Add(this.label7);
            this.gbxStart.Controls.Add(this.dtpStartTime);
            this.gbxStart.Controls.Add(this.txtStartCity);
            this.gbxStart.Controls.Add(this.label6);
            this.gbxStart.Controls.Add(this.txtStartAddress4);
            this.gbxStart.Controls.Add(this.txtStartAddress3);
            this.gbxStart.Controls.Add(this.txtStartAddress2);
            this.gbxStart.Controls.Add(this.txtStartAddress);
            this.gbxStart.Controls.Add(this.label5);
            this.gbxStart.Controls.Add(this.label4);
            this.gbxStart.Location = new System.Drawing.Point(7, 9);
            this.gbxStart.Name = "gbxStart";
            this.gbxStart.Size = new System.Drawing.Size(441, 203);
            this.gbxStart.TabIndex = 0;
            this.gbxStart.TabStop = false;
            this.gbxStart.Text = "Start";
            // 
            // txtStartName
            // 
            this.txtStartName.Location = new System.Drawing.Point(68, 41);
            this.txtStartName.Name = "txtStartName";
            this.txtStartName.Size = new System.Drawing.Size(325, 20);
            this.txtStartName.TabIndex = 11;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(3, 45);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 13);
            this.label16.TabIndex = 10;
            this.label16.Text = "Name:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(4, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Start Time:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(68, 173);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(156, 20);
            this.dtpStartTime.TabIndex = 7;
            this.dtpStartTime.ValueChanged += new System.EventHandler(this.dtpStartTime_ValueChanged);
            // 
            // txtStartCity
            // 
            this.txtStartCity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtStartCity.Location = new System.Drawing.Point(68, 151);
            this.txtStartCity.Name = "txtStartCity";
            this.txtStartCity.ReadOnly = true;
            this.txtStartCity.Size = new System.Drawing.Size(156, 20);
            this.txtStartCity.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(4, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "City:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStartAddress4
            // 
            this.txtStartAddress4.Location = new System.Drawing.Point(68, 129);
            this.txtStartAddress4.Name = "txtStartAddress4";
            this.txtStartAddress4.Size = new System.Drawing.Size(325, 20);
            this.txtStartAddress4.TabIndex = 4;
            // 
            // txtStartAddress3
            // 
            this.txtStartAddress3.Location = new System.Drawing.Point(68, 107);
            this.txtStartAddress3.Name = "txtStartAddress3";
            this.txtStartAddress3.Size = new System.Drawing.Size(325, 20);
            this.txtStartAddress3.TabIndex = 4;
            // 
            // txtStartAddress2
            // 
            this.txtStartAddress2.Location = new System.Drawing.Point(68, 85);
            this.txtStartAddress2.Name = "txtStartAddress2";
            this.txtStartAddress2.Size = new System.Drawing.Size(325, 20);
            this.txtStartAddress2.TabIndex = 4;
            // 
            // txtStartAddress
            // 
            this.txtStartAddress.Location = new System.Drawing.Point(68, 63);
            this.txtStartAddress.Name = "txtStartAddress";
            this.txtStartAddress.Size = new System.Drawing.Size(325, 20);
            this.txtStartAddress.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(4, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Address:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Code:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnDown);
            this.groupBox3.Controls.Add(this.btnCancelRoute);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.btnSaveRoute);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.btnUp);
            this.groupBox3.Controls.Add(this.dgvSortedPlanRoutedSubTrips);
            this.groupBox3.Controls.Add(this.dgvUnSortedPlanRoutedSubTrips);
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 325);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(995, 285);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Edit Route Plan";
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(622, 252);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(713, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Sorted Sub Trips";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(186, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "UnSorted Sub Trips";
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(541, 252);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 4;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // dgvSortedPlanRoutedSubTrips
            // 
            this.dgvSortedPlanRoutedSubTrips.AllowUserToAddRows = false;
            this.dgvSortedPlanRoutedSubTrips.AllowUserToDeleteRows = false;
            this.dgvSortedPlanRoutedSubTrips.AutoGenerateColumns = false;
            this.dgvSortedPlanRoutedSubTrips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSortedPlanRoutedSubTrips.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.startDataGridViewTextBoxColumn1,
            this.endDataGridViewTextBoxColumn1,
            this.startStopDataGridViewTextBoxColumn1,
            this.endStopDataGridViewTextBoxColumn1,
            this.vehicleNumberDataGridViewTextBoxColumn1,
            this.driverNumberDataGridViewTextBoxColumn1});
            this.dgvSortedPlanRoutedSubTrips.DataSource = this.bindSourceSortedRoutedPlanSubTrips;
            this.dgvSortedPlanRoutedSubTrips.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSortedPlanRoutedSubTrips.Location = new System.Drawing.Point(541, 31);
            this.dgvSortedPlanRoutedSubTrips.Name = "dgvSortedPlanRoutedSubTrips";
            this.dgvSortedPlanRoutedSubTrips.ReadOnly = true;
            this.dgvSortedPlanRoutedSubTrips.Size = new System.Drawing.Size(430, 216);
            this.dgvSortedPlanRoutedSubTrips.TabIndex = 0;
            this.dgvSortedPlanRoutedSubTrips.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSortedPlanRoutedSubTrips_CellClick);
            this.dgvSortedPlanRoutedSubTrips.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSortedPlanRoutedSubTrips_CellContentClick);
            this.dgvSortedPlanRoutedSubTrips.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSortedPlanRoutedSubTrips_CellContentDoubleClick);
            this.dgvSortedPlanRoutedSubTrips.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSortedPlanRoutedSubTrips_CellDoubleClick);
            this.dgvSortedPlanRoutedSubTrips.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSortedPlanRoutedSubTrips_ColumnHeaderMouseClick);
            this.dgvSortedPlanRoutedSubTrips.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSortedPlanRoutedSubTrips_RowHeaderMouseClick);
            this.dgvSortedPlanRoutedSubTrips.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSortedPlanRoutedSubTrips_RowHeaderMouseDoubleClick);
            this.dgvSortedPlanRoutedSubTrips.Click += new System.EventHandler(this.dgvSortedPlanRoutedSubTrips_Click);
            // 
            // dgvUnSortedPlanRoutedSubTrips
            // 
            this.dgvUnSortedPlanRoutedSubTrips.AllowUserToAddRows = false;
            this.dgvUnSortedPlanRoutedSubTrips.AllowUserToDeleteRows = false;
            this.dgvUnSortedPlanRoutedSubTrips.AutoGenerateColumns = false;
            this.dgvUnSortedPlanRoutedSubTrips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnSortedPlanRoutedSubTrips.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.startDataGridViewTextBoxColumn,
            this.endDataGridViewTextBoxColumn,
            this.startStopDataGridViewTextBoxColumn,
            this.endStopDataGridViewTextBoxColumn,
            this.vehicleNumberDataGridViewTextBoxColumn,
            this.driverNumberDataGridViewTextBoxColumn});
            this.dgvUnSortedPlanRoutedSubTrips.DataSource = this.bindSourceUnSortedRoutedPlanSubTrips;
            this.dgvUnSortedPlanRoutedSubTrips.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvUnSortedPlanRoutedSubTrips.Location = new System.Drawing.Point(6, 31);
            this.dgvUnSortedPlanRoutedSubTrips.Name = "dgvUnSortedPlanRoutedSubTrips";
            this.dgvUnSortedPlanRoutedSubTrips.ReadOnly = true;
            this.dgvUnSortedPlanRoutedSubTrips.Size = new System.Drawing.Size(460, 216);
            this.dgvUnSortedPlanRoutedSubTrips.TabIndex = 2;
            this.dgvUnSortedPlanRoutedSubTrips.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnSortedPlanRoutedSubTrips_CellClick);
            this.dgvUnSortedPlanRoutedSubTrips.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnSortedPlanRoutedSubTrips_CellContentClick);
            this.dgvUnSortedPlanRoutedSubTrips.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnSortedPlanRoutedSubTrips_CellContentDoubleClick);
            this.dgvUnSortedPlanRoutedSubTrips.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnSortedPlanRoutedSubTrips_CellDoubleClick);
            this.dgvUnSortedPlanRoutedSubTrips.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUnSortedPlanRoutedSubTrips_ColumnHeaderMouseClick);
            this.dgvUnSortedPlanRoutedSubTrips.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUnSortedPlanRoutedSubTrips_RowHeaderMouseClick);
            this.dgvUnSortedPlanRoutedSubTrips.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvUnSortedPlanRoutedSubTrips_RowHeaderMouseDoubleClick);
            this.dgvUnSortedPlanRoutedSubTrips.Click += new System.EventHandler(this.dgvUnSortedPlanRoutedSubTrips_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnAddAllToUnsorted);
            this.groupBox6.Controls.Add(this.btnAddAllToSorted);
            this.groupBox6.Controls.Add(this.btnAddToUnsorted);
            this.groupBox6.Controls.Add(this.btnAddToSorted);
            this.groupBox6.Location = new System.Drawing.Point(472, 42);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(62, 191);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            // 
            // btnAddAllToUnsorted
            // 
            this.btnAddAllToUnsorted.Location = new System.Drawing.Point(9, 147);
            this.btnAddAllToUnsorted.Name = "btnAddAllToUnsorted";
            this.btnAddAllToUnsorted.Size = new System.Drawing.Size(46, 23);
            this.btnAddAllToUnsorted.TabIndex = 0;
            this.btnAddAllToUnsorted.Text = "<<";
            this.btnAddAllToUnsorted.UseVisualStyleBackColor = true;
            this.btnAddAllToUnsorted.Click += new System.EventHandler(this.btnAddAllToUnsorted_Click);
            // 
            // btnAddAllToSorted
            // 
            this.btnAddAllToSorted.Location = new System.Drawing.Point(9, 107);
            this.btnAddAllToSorted.Name = "btnAddAllToSorted";
            this.btnAddAllToSorted.Size = new System.Drawing.Size(46, 23);
            this.btnAddAllToSorted.TabIndex = 0;
            this.btnAddAllToSorted.Text = ">>";
            this.btnAddAllToSorted.UseVisualStyleBackColor = true;
            this.btnAddAllToSorted.Click += new System.EventHandler(this.btnAddAllToSorted_Click);
            // 
            // btnAddToUnsorted
            // 
            this.btnAddToUnsorted.Location = new System.Drawing.Point(9, 67);
            this.btnAddToUnsorted.Name = "btnAddToUnsorted";
            this.btnAddToUnsorted.Size = new System.Drawing.Size(46, 23);
            this.btnAddToUnsorted.TabIndex = 0;
            this.btnAddToUnsorted.Text = "<";
            this.btnAddToUnsorted.UseVisualStyleBackColor = true;
            this.btnAddToUnsorted.Click += new System.EventHandler(this.btnAddToUnsorted_Click);
            // 
            // btnAddToSorted
            // 
            this.btnAddToSorted.Location = new System.Drawing.Point(9, 29);
            this.btnAddToSorted.Name = "btnAddToSorted";
            this.btnAddToSorted.Size = new System.Drawing.Size(46, 23);
            this.btnAddToSorted.TabIndex = 0;
            this.btnAddToSorted.Text = ">";
            this.btnAddToSorted.UseVisualStyleBackColor = true;
            this.btnAddToSorted.Click += new System.EventHandler(this.btnAddToSorted_Click);
            // 
            // btnSaveRoute
            // 
            this.btnSaveRoute.Location = new System.Drawing.Point(814, 252);
            this.btnSaveRoute.Name = "btnSaveRoute";
            this.btnSaveRoute.Size = new System.Drawing.Size(73, 23);
            this.btnSaveRoute.TabIndex = 3;
            this.btnSaveRoute.Text = "Save Route";
            this.btnSaveRoute.UseVisualStyleBackColor = true;
            this.btnSaveRoute.Click += new System.EventHandler(this.btnSaveRoute_Click);
            // 
            // btnCancelRoute
            // 
            this.btnCancelRoute.Location = new System.Drawing.Point(895, 252);
            this.btnCancelRoute.Name = "btnCancelRoute";
            this.btnCancelRoute.Size = new System.Drawing.Size(75, 23);
            this.btnCancelRoute.TabIndex = 4;
            this.btnCancelRoute.Text = "Clear Route";
            this.btnCancelRoute.UseVisualStyleBackColor = true;
            this.btnCancelRoute.Click += new System.EventHandler(this.btnCancelRoute_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbxEnd);
            this.panel1.Controls.Add(this.btnCancelEdit);
            this.panel1.Controls.Add(this.gbxStart);
            this.panel1.Controls.Add(this.btnDeleteSubTrip);
            this.panel1.Controls.Add(this.btnAddSubTrip);
            this.panel1.Controls.Add(this.btnSaveEditSubTrip);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(995, 260);
            this.panel1.TabIndex = 5;
            // 
            // iplcboEndCode
            // 
            this.iplcboEndCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.iplcboEndCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.iplcboEndCode.DisplayColumns = new string[] {
        "Code",
        "Name"};
            this.iplcboEndCode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.iplcboEndCode.DropDownHeight = 100;
            this.iplcboEndCode.DropDownWidth = 300;
            this.iplcboEndCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iplcboEndCode.FormattingEnabled = true;
            this.iplcboEndCode.IntegralHeight = false;
            this.iplcboEndCode.Location = new System.Drawing.Point(92, 17);
            this.iplcboEndCode.Name = "iplcboEndCode";
            this.iplcboEndCode.Size = new System.Drawing.Size(156, 21);
            this.iplcboEndCode.TabIndex = 9;
            this.iplcboEndCode.SelectedIndexChanged += new System.EventHandler(this.cboEndStop_SelectedIndexChanged);
            // 
            // iplcboStartCode
            // 
            this.iplcboStartCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.iplcboStartCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.iplcboStartCode.DisplayColumns = new string[] {
        "Code",
        "Name"};
            this.iplcboStartCode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.iplcboStartCode.DropDownHeight = 100;
            this.iplcboStartCode.DropDownWidth = 300;
            this.iplcboStartCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iplcboStartCode.FormattingEnabled = true;
            this.iplcboStartCode.IntegralHeight = false;
            this.iplcboStartCode.Location = new System.Drawing.Point(68, 17);
            this.iplcboStartCode.Name = "iplcboStartCode";
            this.iplcboStartCode.Size = new System.Drawing.Size(156, 21);
            this.iplcboStartCode.TabIndex = 9;
            this.iplcboStartCode.SelectedIndexChanged += new System.EventHandler(this.cboStartStop_SelectedIndexChanged);
            // 
            // startDataGridViewTextBoxColumn1
            // 
            this.startDataGridViewTextBoxColumn1.DataPropertyName = "Start";
            this.startDataGridViewTextBoxColumn1.HeaderText = "StartTime";
            this.startDataGridViewTextBoxColumn1.Name = "startDataGridViewTextBoxColumn1";
            this.startDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // endDataGridViewTextBoxColumn1
            // 
            this.endDataGridViewTextBoxColumn1.DataPropertyName = "End";
            this.endDataGridViewTextBoxColumn1.HeaderText = "EndTime";
            this.endDataGridViewTextBoxColumn1.Name = "endDataGridViewTextBoxColumn1";
            this.endDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // startStopDataGridViewTextBoxColumn1
            // 
            this.startStopDataGridViewTextBoxColumn1.DataPropertyName = "StartStop";
            this.startStopDataGridViewTextBoxColumn1.HeaderText = "StartStop";
            this.startStopDataGridViewTextBoxColumn1.Name = "startStopDataGridViewTextBoxColumn1";
            this.startStopDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // endStopDataGridViewTextBoxColumn1
            // 
            this.endStopDataGridViewTextBoxColumn1.DataPropertyName = "EndStop";
            this.endStopDataGridViewTextBoxColumn1.HeaderText = "EndStop";
            this.endStopDataGridViewTextBoxColumn1.Name = "endStopDataGridViewTextBoxColumn1";
            this.endStopDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // vehicleNumberDataGridViewTextBoxColumn1
            // 
            this.vehicleNumberDataGridViewTextBoxColumn1.DataPropertyName = "VehicleNumber";
            this.vehicleNumberDataGridViewTextBoxColumn1.HeaderText = "VehicleNumber";
            this.vehicleNumberDataGridViewTextBoxColumn1.Name = "vehicleNumberDataGridViewTextBoxColumn1";
            this.vehicleNumberDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // driverNumberDataGridViewTextBoxColumn1
            // 
            this.driverNumberDataGridViewTextBoxColumn1.DataPropertyName = "DriverNumber";
            this.driverNumberDataGridViewTextBoxColumn1.HeaderText = "DriverNumber";
            this.driverNumberDataGridViewTextBoxColumn1.Name = "driverNumberDataGridViewTextBoxColumn1";
            this.driverNumberDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // bindSourceSortedRoutedPlanSubTrips
            // 
            this.bindSourceSortedRoutedPlanSubTrips.DataSource = typeof(PlanTruckSubTrips);
            // 
            // startDataGridViewTextBoxColumn
            // 
            this.startDataGridViewTextBoxColumn.DataPropertyName = "Start";
            this.startDataGridViewTextBoxColumn.HeaderText = "StartTime";
            this.startDataGridViewTextBoxColumn.Name = "startDataGridViewTextBoxColumn";
            this.startDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // endDataGridViewTextBoxColumn
            // 
            this.endDataGridViewTextBoxColumn.DataPropertyName = "End";
            this.endDataGridViewTextBoxColumn.HeaderText = "EndTime";
            this.endDataGridViewTextBoxColumn.Name = "endDataGridViewTextBoxColumn";
            this.endDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // startStopDataGridViewTextBoxColumn
            // 
            this.startStopDataGridViewTextBoxColumn.DataPropertyName = "StartStop";
            this.startStopDataGridViewTextBoxColumn.HeaderText = "StartStop";
            this.startStopDataGridViewTextBoxColumn.Name = "startStopDataGridViewTextBoxColumn";
            this.startStopDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // endStopDataGridViewTextBoxColumn
            // 
            this.endStopDataGridViewTextBoxColumn.DataPropertyName = "EndStop";
            this.endStopDataGridViewTextBoxColumn.HeaderText = "EndStop";
            this.endStopDataGridViewTextBoxColumn.Name = "endStopDataGridViewTextBoxColumn";
            this.endStopDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // vehicleNumberDataGridViewTextBoxColumn
            // 
            this.vehicleNumberDataGridViewTextBoxColumn.DataPropertyName = "VehicleNumber";
            this.vehicleNumberDataGridViewTextBoxColumn.HeaderText = "VehicleNumber";
            this.vehicleNumberDataGridViewTextBoxColumn.Name = "vehicleNumberDataGridViewTextBoxColumn";
            this.vehicleNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // driverNumberDataGridViewTextBoxColumn
            // 
            this.driverNumberDataGridViewTextBoxColumn.DataPropertyName = "DriverNumber";
            this.driverNumberDataGridViewTextBoxColumn.HeaderText = "DriverNumber";
            this.driverNumberDataGridViewTextBoxColumn.Name = "driverNumberDataGridViewTextBoxColumn";
            this.driverNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bindSourceUnSortedRoutedPlanSubTrips
            // 
            this.bindSourceUnSortedRoutedPlanSubTrips.DataSource = typeof(PlanTruckSubTrips);
            // 
            // FrmAddRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(995, 610);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmAddRoute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddRoute";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAddRoutedPlanTruckSubTrips_FormClosing);
            this.Load += new System.EventHandler(this.FrmAddRoutedPlanTruckSubTrips_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxEnd.ResumeLayout(false);
            this.gbxEnd.PerformLayout();
            this.gbxStart.ResumeLayout(false);
            this.gbxStart.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSortedPlanRoutedSubTrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnSortedPlanRoutedSubTrips)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindSourceSortedRoutedPlanSubTrips)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindSourceUnSortedRoutedPlanSubTrips)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboVehicle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboDriver;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveEditSubTrip;
        private System.Windows.Forms.Button btnAddSubTrip;
        private System.Windows.Forms.GroupBox gbxStart;
        private System.Windows.Forms.Button btnDeleteSubTrip;
        private System.Windows.Forms.TextBox txtStartAddress2;
        private System.Windows.Forms.TextBox txtStartAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbxEnd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.TextBox txtEndCity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEndAddress4;
        private System.Windows.Forms.TextBox txtEndAddress3;
        private System.Windows.Forms.TextBox txtEndAddress2;
        private System.Windows.Forms.TextBox txtEndAddress;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.TextBox txtStartCity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStartAddress4;
        private System.Windows.Forms.TextBox txtStartAddress3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView dgvUnSortedPlanRoutedSubTrips;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnSaveRoute;
        private System.Windows.Forms.Button btnCancelRoute;
        private System.Windows.Forms.Button btnAddAllToSorted;
        private System.Windows.Forms.Button btnAddToUnsorted;
        private System.Windows.Forms.Button btnAddToSorted;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnAddAllToUnsorted;
        private System.Windows.Forms.DataGridView dgvSortedPlanRoutedSubTrips;
        private FormBaseLibrary.CommonControl.IPLComboBox iplcboStartCode;
        private FormBaseLibrary.CommonControl.IPLComboBox iplcboEndCode;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.BindingSource bindSourceUnSortedRoutedPlanSubTrips;
        private System.Windows.Forms.BindingSource bindSourceSortedRoutedPlanSubTrips;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn startStopDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn endStopDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn vehicleNumberDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverNumberDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startStopDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn endStopDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vehicleNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn driverNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnCancelEdit;
        private System.Windows.Forms.TextBox txtEndName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtStartName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel1;
    }
}