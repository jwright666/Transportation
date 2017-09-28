namespace FM.TR_TRKBookUI.UI
{
    partial class FrmConsignmentNote
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboTrx_Id = new System.Windows.Forms.ComboBox();
            this.lblTrx_Id = new System.Windows.Forms.Label();
            this.cboCustomerCode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRetreive = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboTrx_Id);
            this.panel1.Controls.Add(this.lblTrx_Id);
            this.panel1.Controls.Add(this.cboCustomerCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 58);
            this.panel1.TabIndex = 13;
            // 
            // cboTrx_Id
            // 
            this.cboTrx_Id.FormattingEnabled = true;
            this.cboTrx_Id.Location = new System.Drawing.Point(120, 31);
            this.cboTrx_Id.Name = "cboTrx_Id";
            this.cboTrx_Id.Size = new System.Drawing.Size(124, 21);
            this.cboTrx_Id.TabIndex = 12;
            this.cboTrx_Id.SelectedIndexChanged += new System.EventHandler(this.cboTrx_Id_SelectedIndexChanged);
            this.cboTrx_Id.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCustomerCode_KeyPress);
            // 
            // lblTrx_Id
            // 
            this.lblTrx_Id.Location = new System.Drawing.Point(7, 31);
            this.lblTrx_Id.Name = "lblTrx_Id";
            this.lblTrx_Id.Size = new System.Drawing.Size(107, 21);
            this.lblTrx_Id.TabIndex = 11;
            this.lblTrx_Id.Text = "Trx ID :";
            this.lblTrx_Id.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboCustomerCode
            // 
            this.cboCustomerCode.Enabled = false;
            this.cboCustomerCode.FormattingEnabled = true;
            this.cboCustomerCode.Location = new System.Drawing.Point(120, 6);
            this.cboCustomerCode.Name = "cboCustomerCode";
            this.cboCustomerCode.Size = new System.Drawing.Size(124, 21);
            this.cboCustomerCode.TabIndex = 10;
            this.cboCustomerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCustomerCode_KeyPress);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 21);
            this.label1.TabIndex = 9;
            this.label1.Text = "Customer Code :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRetreive
            // 
            this.btnRetreive.Location = new System.Drawing.Point(36, 2);
            this.btnRetreive.Name = "btnRetreive";
            this.btnRetreive.Size = new System.Drawing.Size(89, 25);
            this.btnRetreive.TabIndex = 8;
            this.btnRetreive.Text = "Retrieve";
            this.btnRetreive.UseVisualStyleBackColor = true;
            this.btnRetreive.Click += new System.EventHandler(this.btnRetrieveJob_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnRetreive);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 58);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(269, 30);
            this.panel2.TabIndex = 14;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(131, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmConsignmentNote
            // 
            this.AcceptButton = this.btnRetreive;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(269, 88);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MaximumSize = new System.Drawing.Size(285, 127);
            this.MinimumSize = new System.Drawing.Size(285, 127);
            this.Name = "FrmConsignmentNote";
            this.Text = "Pull Consolidated Jobs";
            this.Load += new System.EventHandler(this.FrmConsignmentNote_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRetreive;
        private System.Windows.Forms.ComboBox cboCustomerCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTrx_Id;
        private System.Windows.Forms.Label lblTrx_Id;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
    }
}