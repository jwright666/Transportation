namespace FM.TR_TRKBookUI.UI
{
    partial class FrmPrintOption
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
            this.btnBookingConfirmation = new System.Windows.Forms.Button();
            this.btnDeliveryNote = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gboOption = new System.Windows.Forms.GroupBox();
            this.rdbSingle = new System.Windows.Forms.RadioButton();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.pnlJobTripSeqNo = new System.Windows.Forms.Panel();
            this.txtEndStop = new System.Windows.Forms.TextBox();
            this.lblEndStop = new System.Windows.Forms.Label();
            this.txtStartStop = new System.Windows.Forms.TextBox();
            this.lblStartStop = new System.Windows.Forms.Label();
            this.cboJobTripSeqNo = new System.Windows.Forms.ComboBox();
            this.lblSeqNo = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.gboOption.SuspendLayout();
            this.pnlJobTripSeqNo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBookingConfirmation
            // 
            this.btnBookingConfirmation.AutoSize = true;
            this.btnBookingConfirmation.Location = new System.Drawing.Point(12, 12);
            this.btnBookingConfirmation.Name = "btnBookingConfirmation";
            this.btnBookingConfirmation.Size = new System.Drawing.Size(117, 23);
            this.btnBookingConfirmation.TabIndex = 0;
            this.btnBookingConfirmation.Text = "Booking Confirmation";
            this.btnBookingConfirmation.UseVisualStyleBackColor = true;
            this.btnBookingConfirmation.Click += new System.EventHandler(this.btnBookingConfirmation_Click);
            // 
            // btnDeliveryNote
            // 
            this.btnDeliveryNote.AutoSize = true;
            this.btnDeliveryNote.Location = new System.Drawing.Point(12, 45);
            this.btnDeliveryNote.Name = "btnDeliveryNote";
            this.btnDeliveryNote.Size = new System.Drawing.Size(117, 23);
            this.btnDeliveryNote.TabIndex = 1;
            this.btnDeliveryNote.Text = "Delivery Note";
            this.btnDeliveryNote.UseVisualStyleBackColor = true;
            this.btnDeliveryNote.Click += new System.EventHandler(this.btnDeliveryNote_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(12, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gboOption
            // 
            this.gboOption.Controls.Add(this.rdbSingle);
            this.gboOption.Controls.Add(this.rdbAll);
            this.gboOption.Location = new System.Drawing.Point(148, 3);
            this.gboOption.Name = "gboOption";
            this.gboOption.Size = new System.Drawing.Size(170, 43);
            this.gboOption.TabIndex = 3;
            this.gboOption.TabStop = false;
            this.gboOption.Text = "Print Option";
            // 
            // rdbSingle
            // 
            this.rdbSingle.AutoSize = true;
            this.rdbSingle.Location = new System.Drawing.Point(100, 19);
            this.rdbSingle.Name = "rdbSingle";
            this.rdbSingle.Size = new System.Drawing.Size(54, 17);
            this.rdbSingle.TabIndex = 1;
            this.rdbSingle.TabStop = true;
            this.rdbSingle.Text = "Single";
            this.rdbSingle.UseVisualStyleBackColor = true;
            this.rdbSingle.CheckedChanged += new System.EventHandler(this.rdbSingle_CheckedChanged);
            // 
            // rdbAll
            // 
            this.rdbAll.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.rdbAll.AutoSize = true;
            this.rdbAll.Checked = true;
            this.rdbAll.Location = new System.Drawing.Point(9, 19);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(60, 17);
            this.rdbAll.TabIndex = 0;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "Print All";
            this.rdbAll.UseVisualStyleBackColor = true;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.rdbSingle_CheckedChanged);
            // 
            // pnlJobTripSeqNo
            // 
            this.pnlJobTripSeqNo.Controls.Add(this.txtEndStop);
            this.pnlJobTripSeqNo.Controls.Add(this.lblEndStop);
            this.pnlJobTripSeqNo.Controls.Add(this.txtStartStop);
            this.pnlJobTripSeqNo.Controls.Add(this.lblStartStop);
            this.pnlJobTripSeqNo.Controls.Add(this.cboJobTripSeqNo);
            this.pnlJobTripSeqNo.Controls.Add(this.lblSeqNo);
            this.pnlJobTripSeqNo.Location = new System.Drawing.Point(148, 53);
            this.pnlJobTripSeqNo.Name = "pnlJobTripSeqNo";
            this.pnlJobTripSeqNo.Size = new System.Drawing.Size(170, 75);
            this.pnlJobTripSeqNo.TabIndex = 3;
            this.pnlJobTripSeqNo.Visible = false;
            // 
            // txtEndStop
            // 
            this.txtEndStop.Location = new System.Drawing.Point(98, 50);
            this.txtEndStop.Name = "txtEndStop";
            this.txtEndStop.ReadOnly = true;
            this.txtEndStop.Size = new System.Drawing.Size(66, 20);
            this.txtEndStop.TabIndex = 9;
            // 
            // lblEndStop
            // 
            this.lblEndStop.Location = new System.Drawing.Point(5, 54);
            this.lblEndStop.Name = "lblEndStop";
            this.lblEndStop.Size = new System.Drawing.Size(90, 13);
            this.lblEndStop.TabIndex = 8;
            this.lblEndStop.Text = "End Stop :";
            this.lblEndStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStartStop
            // 
            this.txtStartStop.Location = new System.Drawing.Point(98, 28);
            this.txtStartStop.Name = "txtStartStop";
            this.txtStartStop.ReadOnly = true;
            this.txtStartStop.Size = new System.Drawing.Size(66, 20);
            this.txtStartStop.TabIndex = 7;
            // 
            // lblStartStop
            // 
            this.lblStartStop.Location = new System.Drawing.Point(5, 32);
            this.lblStartStop.Name = "lblStartStop";
            this.lblStartStop.Size = new System.Drawing.Size(90, 13);
            this.lblStartStop.TabIndex = 6;
            this.lblStartStop.Text = "Start Stop :";
            this.lblStartStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboJobTripSeqNo
            // 
            this.cboJobTripSeqNo.FormattingEnabled = true;
            this.cboJobTripSeqNo.Location = new System.Drawing.Point(98, 5);
            this.cboJobTripSeqNo.Name = "cboJobTripSeqNo";
            this.cboJobTripSeqNo.Size = new System.Drawing.Size(66, 21);
            this.cboJobTripSeqNo.TabIndex = 5;
            this.cboJobTripSeqNo.SelectedIndexChanged += new System.EventHandler(this.cboJobTripSeqNo_SelectedIndexChanged);
            // 
            // lblSeqNo
            // 
            this.lblSeqNo.Location = new System.Drawing.Point(5, 9);
            this.lblSeqNo.Name = "lblSeqNo";
            this.lblSeqNo.Size = new System.Drawing.Size(90, 13);
            this.lblSeqNo.TabIndex = 4;
            this.lblSeqNo.Text = "Seq. No :";
            this.lblSeqNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSize = true;
            this.btnPrint.Location = new System.Drawing.Point(75, 104);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(54, 23);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // FrmPrintOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 130);
            this.Controls.Add(this.gboOption);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.pnlJobTripSeqNo);
            this.Controls.Add(this.btnDeliveryNote);
            this.Controls.Add(this.btnBookingConfirmation);
            this.Controls.Add(this.btnCancel);
            this.Name = "FrmPrintOption";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Option";
            this.Load += new System.EventHandler(this.FrmPrintOption_Load);
            this.gboOption.ResumeLayout(false);
            this.gboOption.PerformLayout();
            this.pnlJobTripSeqNo.ResumeLayout(false);
            this.pnlJobTripSeqNo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBookingConfirmation;
        private System.Windows.Forms.Button btnDeliveryNote;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gboOption;
        private System.Windows.Forms.RadioButton rdbSingle;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.Panel pnlJobTripSeqNo;
        private System.Windows.Forms.Label lblSeqNo;
        private System.Windows.Forms.ComboBox cboJobTripSeqNo;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtEndStop;
        private System.Windows.Forms.Label lblEndStop;
        private System.Windows.Forms.TextBox txtStartStop;
        private System.Windows.Forms.Label lblStartStop;
    }
}