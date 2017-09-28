namespace FM.TR_MaintenanceUI.UI
{
    partial class FrmAuditLog
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
            this.pnlCriteria = new System.Windows.Forms.Panel();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlCriteria
            // 
            this.pnlCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCriteria.Location = new System.Drawing.Point(0, 0);
            this.pnlCriteria.Name = "pnlCriteria";
            this.pnlCriteria.Size = new System.Drawing.Size(796, 127);
            this.pnlCriteria.TabIndex = 0;
            // 
            // pnlDetails
            // 
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 127);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(796, 272);
            this.pnlDetails.TabIndex = 1;
            // 
            // FrmAuditLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 399);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.pnlCriteria);
            this.Name = "FrmAuditLog";
            this.Text = "Audit Log";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCriteria;
        private System.Windows.Forms.Panel pnlDetails;
    }
}