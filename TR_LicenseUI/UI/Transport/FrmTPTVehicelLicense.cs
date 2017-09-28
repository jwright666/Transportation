using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.FMSystem.BLL;
using TR_LanguageResource.Resources;
using FMLicense.BLL.Transport;
using FMLicenseUI.Common;
using FMLicenseUI.Common.Transport;

namespace FMLicenseUI.Common
{
    public partial class FrmTPTVehicelLicense : FrmBaseValidLicense
    {
        private FrmTPTCompanyLicense parent;
        private FormMode formMode;
        private TPTVehicleNoOfLicense license;

        public FrmTPTVehicelLicense() : base() { }

        public FrmTPTVehicelLicense(FrmTPTCompanyLicense parent, FormMode formMode)
        {
            InitializeComponent();
            this.parent = parent;
            this.license = parent.license;
            this.formMode = formMode;
        }

        private void btnSaveLicense_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO Save license to memory
                FillLicense();
                if (formMode == FormMode.Add)
                    parent.company.AddVehicleLicense(this.license);
                else
                    parent.company.EditVehicleLicense(this.license);

                this.Close();
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
            }
        }

        private void btnCancelLicense_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void DisplayLicense()
        {
            try
            {
                txtCompanyName.Text = parent.company.CompanyName;
                int licenseCount = parent.company.vehicleNoLicenses.Count;
                switch (formMode)
                {
                    case FormMode.Add:
                        txtNoOfLicense.Text = "0";
                        dtpValidFrom.Value = DateTime.Today;

                        txtOldEffectiveDate.Text = licenseCount < 1 ? string.Empty : parent.company.vehicleNoLicenses[0].validFrom.ToShortDateString();
                        //txtOldExpiryDate.Text = licenseCount < 1 ? string.Empty : parent.company.vehicleNoLicenses[0].validTo.Value.ToShortDateString();
                        txtOldNoOfLicense.Text = licenseCount < 1 ? string.Empty : parent.company.vehicleNoLicenses[0].numOfLicense.ToString();
                        break;
                    case FormMode.Edit:
                        txtNoOfLicense.Text = this.license.numOfLicense.ToString();
                        dtpValidFrom.Value = this.license.validFrom;

                        txtOldEffectiveDate.Text = this.license.validFrom.ToShortDateString();
                        txtOldNoOfLicense.Text = this.license.numOfLicense.ToString();             
                        break;
                }
            }
            catch { }
        }
        private void FillLicense()
        {
            try
            {
                if (txtNoOfLicense.Text.Length == 0 || txtNoOfLicense.Text.Trim() == "0")
                    throw new FMException("Number of license cannot be 0 or empty. ");
                else
                {
                    this.license = new TPTVehicleNoOfLicense();
                    this.license.CompanyName = parent.company.CompanyName.Trim();
                    this.license.GroupName = parent.company.GroupName.Trim();
                    this.license.numOfLicense = Convert.ToInt32(txtNoOfLicense.Text.Trim());
                    this.license.validFrom = dtpValidFrom.Value;
                    this.license.validTo = null;
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
        }

        private void FrmTPTVehicelLicense_Load(object sender, EventArgs e)
        {
            DisplayLicense();
        }

        private void txtNoOfLicense_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar)&& !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch { }
        }

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
            // txtNoOfLicense
            // 
            this.txtNoOfLicense.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNoOfLicense_KeyPress);
            // 
            // FrmTPTVehicelLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(365, 195);
            this.Name = "FrmTPTVehicelLicense";
            this.Shown += new System.EventHandler(this.FrmTPTVehicelLicense_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxLicenseInfo.ResumeLayout(false);
            this.gbxLicenseInfo.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
