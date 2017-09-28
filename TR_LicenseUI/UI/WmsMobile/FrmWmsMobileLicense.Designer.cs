namespace FMLicenseUI.Common.WmsMobile
{
    partial class FrmWmsMobileLicense
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
            this.groupBox1.SuspendLayout();
            this.gbxLicenseInfo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancelLicense
            // 
            this.btnCancelLicense.Click += new System.EventHandler(this.btnCancelLicense_Click);
            // 
            // btnSaveLicense
            // 
            this.btnSaveLicense.Click += new System.EventHandler(this.btnSaveLicense_Click);
            // 
            // FrmWmsMobileLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 195);
            this.Name = "FrmWmsMobileLicense";
            this.Text = "FrmWmsMobileLicense";
            this.Load += new System.EventHandler(this.FrmTPTVehicelLicense_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxLicenseInfo.ResumeLayout(false);
            this.gbxLicenseInfo.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}