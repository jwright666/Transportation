using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FMLicenseUI.Common;
using FM.FMSystem.BLL;
using FMLicenseUI.Common.WmsMobile;
using FMLicense.BLL.WmsMobile;
using TR_LanguageResource.Resources;

namespace FMLicenseUI.Common.WmsMobile
{
    public partial class FrmWmsMobileLicense : FrmBaseValidLicense
    {          
        private FrmWmsCompanyRegistration parent;
        private FormMode formMode;
        private WmsMobileNoOfLicense license;

        public FrmWmsMobileLicense()
        {
            InitializeComponent();
        }
        public FrmWmsMobileLicense(FrmWmsCompanyRegistration parent, FormMode formMode)
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
                    parent.company.AddMobileLicense(this.license);
                else
                    parent.company.EditMobileLicense(this.license);

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
                int licenseCount = parent.company.wmsMobileNoOfLicenses.Count;
                switch (formMode)
                {
                    case FormMode.Add:
                        txtNoOfLicense.Text = "0";
                        dtpValidFrom.Value = DateTime.Today;

                        txtOldEffectiveDate.Text = licenseCount < 1 ? string.Empty : parent.company.wmsMobileNoOfLicenses[0].validFrom.ToShortDateString();
                        //txtOldExpiryDate.Text = licenseCount < 1 ? string.Empty : parent.company.vehicleNoLicenses[0].validTo.Value.ToShortDateString();
                        txtOldNoOfLicense.Text = licenseCount < 1 ? string.Empty : parent.company.wmsMobileNoOfLicenses[0].numOfLicense.ToString();
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
                    this.license = new WmsMobileNoOfLicense();
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
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch { }
        }

    }
}
