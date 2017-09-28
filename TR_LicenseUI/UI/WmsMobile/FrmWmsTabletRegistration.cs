using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FormBaseLibrary;
using FM.FMSystem.BLL;
using TR_LanguageResource.Resources;
using FMLicense.BLL.WmsMobile;
using FMLicenseUI.Common.WmsMobile;

namespace FMLicenseUI.Common.WmsMobile
{
    public partial class FrmWmsTabletRegistration : AbstractForm
    {
        FrmWmsCompanyRegistration parent;
        private WmsMobileDevice mobileDevice = new WmsMobileDevice();
        private FormMode frmMode = FormMode.Delete;

        public FrmWmsTabletRegistration()
        {
            InitializeComponent();
        }
        public FrmWmsTabletRegistration(FrmWmsCompanyRegistration parent, FormMode frmMode)
        {
            InitializeComponent();
            this.parent = parent;
            this.mobileDevice = parent.mobileDevice;
            this.frmMode = frmMode;
        }

        private void BindComponent()
        {
            if (mobileDevice != null)
            {
                txtCompanyName.Text = parent.company.CompanyName;
                txtAndroidID.Text = mobileDevice.AndroidID;
                txtBrand.Text = mobileDevice.Brand;
                txtModel.Text = mobileDevice.Model;
                txtDeviceName.Text = mobileDevice.DeviceName;
            }
        }
        private void EnableControl()
        {
            switch (frmMode)
            {
                case FormMode.Add:
                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;
                    gbxTabletInfo.Enabled = true;
                    txtAndroidID.Enabled = true;
                    break;
                case FormMode.Edit:
                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;
                    gbxTabletInfo.Enabled = true;
                    txtAndroidID.Enabled = false;
                    break;
            }
        }

        private bool validateFields(out string outMsg)
        {
            bool retValue = true;
            outMsg = WmsResourceBLL.MsgCompulsoryFields + " :";
            if (txtAndroidID.Text.ToString().Trim() == "")
            {
                outMsg += "\n*" + WmsResourceBLL.ErrEmptyAndroidID;
                retValue = false;
            }
            if (txtBrand.Text.ToString().Trim() == "")
            {
                outMsg += "\n*" + WmsResourceBLL.ErrEmptyDeviceBrand;
                retValue = false;
            }
            if (txtModel.Text.ToString().Trim() == "")
            {
                outMsg += "\n*" + WmsResourceBLL.ErrEmptyDeviceModel;
                retValue = false;
            }
            if (txtDeviceName.Text.ToString().Trim() == "")
            {
                outMsg += "\n*" + WmsResourceBLL.ErrEmptyDeviceName;
                retValue = false;
            }   
            return retValue;
        }
        private void FillMobileDevice()
        {
            mobileDevice = new WmsMobileDevice();
            mobileDevice.CompanyName = txtCompanyName.Text.ToString().Trim();
            mobileDevice.AndroidID = txtAndroidID.Text.ToString().Trim();
            mobileDevice.Brand = txtBrand.Text.ToString().Trim();
            mobileDevice.Model = txtModel.Text.ToString().Trim();
            mobileDevice.DeviceName = txtDeviceName.Text.ToString().Trim();
        }

      

        private void FrmTabletRegistration_Load(object sender, EventArgs e)
        {
            txtCompanyName.Text = parent.company.CompanyName;
            base.setAllTextBoxToUppercase(this);
            BindComponent();                   
            EnableControl();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string outMsg = "";
                if (validateFields(out outMsg))
                {
                    FillMobileDevice();
                    switch (frmMode)
                    { 
                        case FormMode.Edit:
                            //TODO edit mobileDevice
                            mobileDevice.EditMobileDevice();
                            MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                            break;
                        case FormMode.Add:
                            mobileDevice.AddMobileDevice();
                            MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                            break;  
                    }
                    this.Close();
                }
                else
                {
                    throw new FMException(outMsg.ToString());
                }
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }

        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

 


    }
}
