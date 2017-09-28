namespace FM.TR_TRKPlanningUI.UI
{
    partial class FrmAddPlanTruckSubTrip
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
            this.lblAddressEnd = new System.Windows.Forms.Label();
            this.lblCityEnd = new System.Windows.Forms.Label();
            this.lblCodeEnd = new System.Windows.Forms.Label();
            this.lblAddressStart = new System.Windows.Forms.Label();
            this.lblCityStart = new System.Windows.Forms.Label();
            this.lblCodeStart = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.grpEnd = new System.Windows.Forms.GroupBox();
            this.txtAddress4To = new System.Windows.Forms.TextBox();
            this.txtAddress3To = new System.Windows.Forms.TextBox();
            this.txtAddress2To = new System.Windows.Forms.TextBox();
            this.txtAddress1To = new System.Windows.Forms.TextBox();
            this.cboEndStop = new System.Windows.Forms.ComboBox();
            this.txtDescriptionTo = new System.Windows.Forms.TextBox();
            this.txtCityTo = new System.Windows.Forms.TextBox();
            this.grpStart = new System.Windows.Forms.GroupBox();
            this.txtAddress4From = new System.Windows.Forms.TextBox();
            this.txtAddress3From = new System.Windows.Forms.TextBox();
            this.txtAddress1From = new System.Windows.Forms.TextBox();
            this.txtAddress2From = new System.Windows.Forms.TextBox();
            this.cboStartStop = new System.Windows.Forms.ComboBox();
            this.txtCityFrom = new System.Windows.Forms.TextBox();
            this.txtDescriptionFrom = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboDriverCode = new System.Windows.Forms.ComboBox();
            this.lblDriver = new System.Windows.Forms.Label();
            this.grpEnd.SuspendLayout();
            this.grpStart.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAddressEnd
            // 
            this.lblAddressEnd.Location = new System.Drawing.Point(6, 44);
            this.lblAddressEnd.Name = "lblAddressEnd";
            this.lblAddressEnd.Size = new System.Drawing.Size(62, 13);
            this.lblAddressEnd.TabIndex = 42;
            this.lblAddressEnd.Text = "Address:";
            this.lblAddressEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCityEnd
            // 
            this.lblCityEnd.Location = new System.Drawing.Point(6, 148);
            this.lblCityEnd.Name = "lblCityEnd";
            this.lblCityEnd.Size = new System.Drawing.Size(62, 13);
            this.lblCityEnd.TabIndex = 37;
            this.lblCityEnd.Text = "City:";
            this.lblCityEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCodeEnd
            // 
            this.lblCodeEnd.Location = new System.Drawing.Point(6, 17);
            this.lblCodeEnd.Name = "lblCodeEnd";
            this.lblCodeEnd.Size = new System.Drawing.Size(62, 13);
            this.lblCodeEnd.TabIndex = 39;
            this.lblCodeEnd.Text = "Code:";
            this.lblCodeEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAddressStart
            // 
            this.lblAddressStart.Location = new System.Drawing.Point(6, 50);
            this.lblAddressStart.Name = "lblAddressStart";
            this.lblAddressStart.Size = new System.Drawing.Size(62, 13);
            this.lblAddressStart.TabIndex = 60;
            this.lblAddressStart.Text = "Address:";
            this.lblAddressStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCityStart
            // 
            this.lblCityStart.Location = new System.Drawing.Point(6, 153);
            this.lblCityStart.Name = "lblCityStart";
            this.lblCityStart.Size = new System.Drawing.Size(62, 13);
            this.lblCityStart.TabIndex = 55;
            this.lblCityStart.Text = "City:";
            this.lblCityStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCodeStart
            // 
            this.lblCodeStart.Location = new System.Drawing.Point(6, 23);
            this.lblCodeStart.Name = "lblCodeStart";
            this.lblCodeStart.Size = new System.Drawing.Size(62, 13);
            this.lblCodeStart.TabIndex = 57;
            this.lblCodeStart.Text = "Code:";
            this.lblCodeStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTime.Location = new System.Drawing.Point(210, 220);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Size = new System.Drawing.Size(100, 20);
            this.dtpEndTime.TabIndex = 82;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpStartTime.Location = new System.Drawing.Point(210, 197);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(100, 20);
            this.dtpStartTime.TabIndex = 81;
            // 
            // lblEnd
            // 
            this.lblEnd.Location = new System.Drawing.Point(8, 224);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(90, 13);
            this.lblEnd.TabIndex = 80;
            this.lblEnd.Text = "End :";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStart
            // 
            this.lblStart.Location = new System.Drawing.Point(8, 201);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(90, 13);
            this.lblStart.TabIndex = 79;
            this.lblStart.Text = "Start :";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Enabled = false;
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(104, 220);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(100, 20);
            this.dtpEndDate.TabIndex = 78;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Enabled = false;
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(104, 197);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(100, 20);
            this.dtpStartDate.TabIndex = 77;
            // 
            // grpEnd
            // 
            this.grpEnd.Controls.Add(this.txtAddress4To);
            this.grpEnd.Controls.Add(this.txtAddress3To);
            this.grpEnd.Controls.Add(this.txtAddress2To);
            this.grpEnd.Controls.Add(this.lblAddressEnd);
            this.grpEnd.Controls.Add(this.txtAddress1To);
            this.grpEnd.Controls.Add(this.cboEndStop);
            this.grpEnd.Controls.Add(this.txtDescriptionTo);
            this.grpEnd.Controls.Add(this.lblCityEnd);
            this.grpEnd.Controls.Add(this.lblCodeEnd);
            this.grpEnd.Controls.Add(this.txtCityTo);
            this.grpEnd.Location = new System.Drawing.Point(319, 12);
            this.grpEnd.Name = "grpEnd";
            this.grpEnd.Size = new System.Drawing.Size(305, 179);
            this.grpEnd.TabIndex = 76;
            this.grpEnd.TabStop = false;
            this.grpEnd.Text = "End";
            // 
            // txtAddress4To
            // 
            this.txtAddress4To.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtAddress4To.Location = new System.Drawing.Point(74, 119);
            this.txtAddress4To.Name = "txtAddress4To";
            this.txtAddress4To.ReadOnly = true;
            this.txtAddress4To.Size = new System.Drawing.Size(222, 20);
            this.txtAddress4To.TabIndex = 46;
            // 
            // txtAddress3To
            // 
            this.txtAddress3To.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtAddress3To.Location = new System.Drawing.Point(74, 93);
            this.txtAddress3To.Name = "txtAddress3To";
            this.txtAddress3To.ReadOnly = true;
            this.txtAddress3To.Size = new System.Drawing.Size(222, 20);
            this.txtAddress3To.TabIndex = 45;
            // 
            // txtAddress2To
            // 
            this.txtAddress2To.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtAddress2To.Location = new System.Drawing.Point(74, 67);
            this.txtAddress2To.Name = "txtAddress2To";
            this.txtAddress2To.ReadOnly = true;
            this.txtAddress2To.Size = new System.Drawing.Size(222, 20);
            this.txtAddress2To.TabIndex = 44;
            // 
            // txtAddress1To
            // 
            this.txtAddress1To.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtAddress1To.Location = new System.Drawing.Point(74, 41);
            this.txtAddress1To.Name = "txtAddress1To";
            this.txtAddress1To.ReadOnly = true;
            this.txtAddress1To.Size = new System.Drawing.Size(222, 20);
            this.txtAddress1To.TabIndex = 43;
            // 
            // cboEndStop
            // 
            this.cboEndStop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboEndStop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboEndStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboEndStop.FormattingEnabled = true;
            this.cboEndStop.Location = new System.Drawing.Point(74, 14);
            this.cboEndStop.Name = "cboEndStop";
            this.cboEndStop.Size = new System.Drawing.Size(80, 21);
            this.cboEndStop.TabIndex = 41;
            this.cboEndStop.TextChanged += new System.EventHandler(this.cboEndStop_TextChanged_1);
            // 
            // txtDescriptionTo
            // 
            this.txtDescriptionTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtDescriptionTo.Enabled = false;
            this.txtDescriptionTo.Location = new System.Drawing.Point(160, 15);
            this.txtDescriptionTo.Name = "txtDescriptionTo";
            this.txtDescriptionTo.ReadOnly = true;
            this.txtDescriptionTo.Size = new System.Drawing.Size(136, 20);
            this.txtDescriptionTo.TabIndex = 40;
            // 
            // txtCityTo
            // 
            this.txtCityTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtCityTo.Enabled = false;
            this.txtCityTo.Location = new System.Drawing.Point(74, 145);
            this.txtCityTo.Name = "txtCityTo";
            this.txtCityTo.ReadOnly = true;
            this.txtCityTo.Size = new System.Drawing.Size(100, 20);
            this.txtCityTo.TabIndex = 38;
            // 
            // grpStart
            // 
            this.grpStart.Controls.Add(this.txtAddress4From);
            this.grpStart.Controls.Add(this.txtAddress3From);
            this.grpStart.Controls.Add(this.lblAddressStart);
            this.grpStart.Controls.Add(this.txtAddress1From);
            this.grpStart.Controls.Add(this.txtAddress2From);
            this.grpStart.Controls.Add(this.cboStartStop);
            this.grpStart.Controls.Add(this.lblCityStart);
            this.grpStart.Controls.Add(this.txtCityFrom);
            this.grpStart.Controls.Add(this.lblCodeStart);
            this.grpStart.Controls.Add(this.txtDescriptionFrom);
            this.grpStart.Location = new System.Drawing.Point(8, 12);
            this.grpStart.Name = "grpStart";
            this.grpStart.Size = new System.Drawing.Size(305, 179);
            this.grpStart.TabIndex = 75;
            this.grpStart.TabStop = false;
            this.grpStart.Text = "Start";
            // 
            // txtAddress4From
            // 
            this.txtAddress4From.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtAddress4From.Location = new System.Drawing.Point(74, 124);
            this.txtAddress4From.Name = "txtAddress4From";
            this.txtAddress4From.ReadOnly = true;
            this.txtAddress4From.Size = new System.Drawing.Size(222, 20);
            this.txtAddress4From.TabIndex = 64;
            // 
            // txtAddress3From
            // 
            this.txtAddress3From.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtAddress3From.Location = new System.Drawing.Point(74, 99);
            this.txtAddress3From.Name = "txtAddress3From";
            this.txtAddress3From.ReadOnly = true;
            this.txtAddress3From.Size = new System.Drawing.Size(222, 20);
            this.txtAddress3From.TabIndex = 63;
            // 
            // txtAddress1From
            // 
            this.txtAddress1From.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtAddress1From.Location = new System.Drawing.Point(74, 47);
            this.txtAddress1From.Name = "txtAddress1From";
            this.txtAddress1From.ReadOnly = true;
            this.txtAddress1From.Size = new System.Drawing.Size(222, 20);
            this.txtAddress1From.TabIndex = 62;
            // 
            // txtAddress2From
            // 
            this.txtAddress2From.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtAddress2From.Location = new System.Drawing.Point(74, 73);
            this.txtAddress2From.Name = "txtAddress2From";
            this.txtAddress2From.ReadOnly = true;
            this.txtAddress2From.Size = new System.Drawing.Size(222, 20);
            this.txtAddress2From.TabIndex = 61;
            // 
            // cboStartStop
            // 
            this.cboStartStop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStartStop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStartStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
            this.cboStartStop.FormattingEnabled = true;
            this.cboStartStop.Location = new System.Drawing.Point(74, 20);
            this.cboStartStop.Name = "cboStartStop";
            this.cboStartStop.Size = new System.Drawing.Size(80, 21);
            this.cboStartStop.TabIndex = 59;
            this.cboStartStop.TextChanged += new System.EventHandler(this.cboStartStop_TextChanged_1);
            // 
            // txtCityFrom
            // 
            this.txtCityFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtCityFrom.Enabled = false;
            this.txtCityFrom.Location = new System.Drawing.Point(74, 150);
            this.txtCityFrom.Name = "txtCityFrom";
            this.txtCityFrom.ReadOnly = true;
            this.txtCityFrom.Size = new System.Drawing.Size(100, 20);
            this.txtCityFrom.TabIndex = 56;
            // 
            // txtDescriptionFrom
            // 
            this.txtDescriptionFrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(180)))));
            this.txtDescriptionFrom.Enabled = false;
            this.txtDescriptionFrom.Location = new System.Drawing.Point(158, 20);
            this.txtDescriptionFrom.Name = "txtDescriptionFrom";
            this.txtDescriptionFrom.ReadOnly = true;
            this.txtDescriptionFrom.Size = new System.Drawing.Size(136, 20);
            this.txtDescriptionFrom.TabIndex = 58;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 271);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(635, 47);
            this.panel1.TabIndex = 83;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(336, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(187, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboDriverCode
            // 
            this.cboDriverCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDriverCode.FormattingEnabled = true;
            this.cboDriverCode.Location = new System.Drawing.Point(104, 244);
            this.cboDriverCode.Name = "cboDriverCode";
            this.cboDriverCode.Size = new System.Drawing.Size(206, 21);
            this.cboDriverCode.TabIndex = 85;
            // 
            // lblDriver
            // 
            this.lblDriver.Location = new System.Drawing.Point(8, 247);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(90, 13);
            this.lblDriver.TabIndex = 84;
            this.lblDriver.Text = "Driver :";
            this.lblDriver.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmAddPlanTruckSubTrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(635, 318);
            this.Controls.Add(this.cboDriverCode);
            this.Controls.Add(this.lblDriver);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dtpEndTime);
            this.Controls.Add(this.dtpStartTime);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.grpEnd);
            this.Controls.Add(this.grpStart);
            this.Name = "FrmAddPlanTruckSubTrip";
            this.Text = "Add Plan Truck Sub Trip";
            this.Load += new System.EventHandler(this.FrmAddPlanTruckSubTrip_Load_1);
            this.grpEnd.ResumeLayout(false);
            this.grpEnd.PerformLayout();
            this.grpStart.ResumeLayout(false);
            this.grpStart.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.GroupBox grpEnd;
        private System.Windows.Forms.TextBox txtAddress4To;
        private System.Windows.Forms.TextBox txtAddress3To;
        private System.Windows.Forms.TextBox txtAddress2To;
        private System.Windows.Forms.TextBox txtAddress1To;
        private System.Windows.Forms.ComboBox cboEndStop;
        private System.Windows.Forms.TextBox txtDescriptionTo;
        private System.Windows.Forms.TextBox txtCityTo;
        private System.Windows.Forms.GroupBox grpStart;
        private System.Windows.Forms.TextBox txtAddress4From;
        private System.Windows.Forms.TextBox txtAddress3From;
        private System.Windows.Forms.TextBox txtAddress1From;
        private System.Windows.Forms.TextBox txtAddress2From;
        private System.Windows.Forms.ComboBox cboStartStop;
        private System.Windows.Forms.TextBox txtCityFrom;
        private System.Windows.Forms.TextBox txtDescriptionFrom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboDriverCode;
        private System.Windows.Forms.Label lblDriver;
        private System.Windows.Forms.Label lblAddressEnd;
        private System.Windows.Forms.Label lblCityEnd;
        private System.Windows.Forms.Label lblCodeEnd;
        private System.Windows.Forms.Label lblAddressStart;
        private System.Windows.Forms.Label lblCityStart;
        private System.Windows.Forms.Label lblCodeStart;
    }
}