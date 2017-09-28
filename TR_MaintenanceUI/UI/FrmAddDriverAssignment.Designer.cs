namespace FM.TR_MaintenanceUI.UI
{
    partial class FrmAddDriverAssignment
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
            this.label2 = new System.Windows.Forms.Label();
            this.cboAssignment = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpAssignmentDateFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpAssignmentDateTo = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblAssignTo = new System.Windows.Forms.Label();
            this.lblDriver = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Job Assignment :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboAssignment
            // 
            this.cboAssignment.FormattingEnabled = true;
            this.cboAssignment.Location = new System.Drawing.Point(110, 86);
            this.cboAssignment.Name = "cboAssignment";
            this.cboAssignment.Size = new System.Drawing.Size(108, 21);
            this.cboAssignment.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Date From :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpAssignmentDateFrom
            // 
            this.dtpAssignmentDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAssignmentDateFrom.Location = new System.Drawing.Point(110, 34);
            this.dtpAssignmentDateFrom.Name = "dtpAssignmentDateFrom";
            this.dtpAssignmentDateFrom.Size = new System.Drawing.Size(108, 20);
            this.dtpAssignmentDateFrom.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Date To :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpAssignmentDateTo
            // 
            this.dtpAssignmentDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAssignmentDateTo.Location = new System.Drawing.Point(110, 60);
            this.dtpAssignmentDateTo.Name = "dtpAssignmentDateTo";
            this.dtpAssignmentDateTo.Size = new System.Drawing.Size(108, 20);
            this.dtpAssignmentDateTo.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(29, 119);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(110, 119);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblAssignTo
            // 
            this.lblAssignTo.Location = new System.Drawing.Point(9, 9);
            this.lblAssignTo.Name = "lblAssignTo";
            this.lblAssignTo.Size = new System.Drawing.Size(95, 13);
            this.lblAssignTo.TabIndex = 13;
            this.lblAssignTo.Text = "Assign Priority to :";
            this.lblAssignTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDriver
            // 
            this.lblDriver.Font = new System.Drawing.Font("Calibri Light", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDriver.Location = new System.Drawing.Point(107, 9);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(111, 13);
            this.lblDriver.TabIndex = 14;
            this.lblDriver.Text = "Driver Code";
            // 
            // FrmAddDriverAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 144);
            this.Controls.Add(this.lblDriver);
            this.Controls.Add(this.lblAssignTo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpAssignmentDateTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboAssignment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpAssignmentDateFrom);
            this.Name = "FrmAddDriverAssignment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Assignment";
            this.Load += new System.EventHandler(this.FrmAddDriverAssignment_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboAssignment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpAssignmentDateFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpAssignmentDateTo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblAssignTo;
        private System.Windows.Forms.Label lblDriver;
    }
}