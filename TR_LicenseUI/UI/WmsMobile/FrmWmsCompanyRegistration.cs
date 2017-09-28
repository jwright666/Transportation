using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FormBaseLibrary;
using TR_LanguageResource.Resources;
using FM.FMSystem.BLL;
using System.Net;
using System.Net.Sockets;
using FMLicense.BLL.WmsMobile;
using FM.FMSystem.DAL;
using System.Collections;

namespace FMLicenseUI.Common.WmsMobile
{
    public partial class FrmWmsCompanyRegistration : AbstractForm
    {
        FrmWmsMobileLicense frmWmsMobileLicense;
        FrmWmsTabletRegistration frmWmsTabletRegistration;
        public WmsMobileCompany company = new WmsMobileCompany();
        public FormMode companyMode = FormMode.Delete;
        public FormMode licenseMode = FormMode.Delete;
        public FormMode mobileMode = FormMode.Delete;
        public string serverName;       
        public WmsMobileNoOfLicense license;
        public WmsMobileDevice mobileDevice;
        public SortableList<WmsMobileDevice> mobileDevices;

        public FrmWmsCompanyRegistration()
        {
            InitializeComponent();
        }           
        public FrmWmsCompanyRegistration(string serverName)
        {
            InitializeComponent();
            this.serverName = serverName;
        }    
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string outMsg = "";
                FillCompany();
                switch (companyMode)
                {
                    case FormMode.Edit:
                        //TODO edit company
                        //company.EditNoOfLicense();
                        MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                        break;
                    case FormMode.Add:
                        company.AddNewCompany();
                        MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);                        
                        break;
                }
                RefreshCompanyGrid();
                companyMode = FormMode.Delete;
                EnableButtons();
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
        private void FillCompany()
        {
            company = new WmsMobileCompany();
            company.CompanyName = txtCompanyName.Text.ToString().Trim();
            company.GroupName = txtGroupName.Text.ToString().Trim();
            company.CompanyNameLocal = txtCompanyLocalName.Text.ToString().Trim();
            company.GroupNameLocal = txtGroupLocalName.Text.ToString().Trim();
            company.DatabaseName = txtDatabaseName.Text.ToString().Trim();
            company.SQLServer = this.serverName;// txtServerName.Text.ToString().Trim();
            //company.ServerIP = txtServerIP.Text.ToString().Trim();
            //company.NumberOfLicense = Convert.ToInt32(txtLicenseCount.Text.ToString());
            company.ShareWithGoup = chkShared.Checked;
            //company.WebServiceIP = txtWebserviceIP.Text.ToString().Trim();
        }   
        private void FrmCompanyRegistration_Load(object sender, EventArgs e)
        {
            companyMode = FormMode.Delete;
            //base.setAllTextBoxToUppercase(this);
            RefreshCompanyGrid();
            EnableButtons();
            cboExistingGroups.DataSource = company.GetGroupNames();
        }   
        private void btnCancel_Click(object sender, EventArgs e)
        {
            RefreshCompanyGrid();
            companyMode = FormMode.Delete;
            EnableButtons();
            chkExistingGroup.Checked = false;
        }

        private void txtWebserviceIP_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (sender as TextBox); 
            try
            {
                IPAddress unused;
                if (!IPAddress.TryParse(txtbox.Text.ToString().Trim(), out unused))
                {
                    throw new FMException(txtbox.Text.ToString().Trim() + "\n" + WmsResourceBLL.ErrInvalidIPAddress);
                }
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                txtbox.Focus();
                txtbox.SelectAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                txtbox.Focus();
                txtbox.SelectAll();
            }
        }

        private void dsCompany_CurrentChanged(object sender, EventArgs e)
        {
            if (dsCompany.Count > 0)
            {
                company = (WmsMobileCompany)dsCompany.Current;
                license = null;
                mobileDevice = null;   
            }
            //else
            //    company = new WmsMobileCompany();

            BindComponent();
            gbxCompanyInfo.Enabled = false;
            //company.wmsMobileNoOfLicenses = company.GetMobileLicenses();
            RefreshLicenseGrid();
            RefreshMobileDeviceGrid();
        }
        private void BindComponent()
        {
            if (company != null)
            {
                txtCompanyName.Text = company.CompanyName;
                txtGroupName.Text = company.GroupName;
                txtCompanyLocalName.Text = company.CompanyNameLocal;
                txtGroupLocalName.Text = company.GroupNameLocal;
                txtDatabaseName.Text = company.DatabaseName;
                //txtServerName.Text = company.ServerName;
                //txtServerIP.Text = company.ServerIP;
                //txtLicenseCount.Text = company.NumberOfLicense.ToString();
                chkShared.Checked = company.ShareWithGoup;
                //txtWebserviceIP.Text = company.WebServiceIP;
            }
        }
        private void EnableButtons()
        {
            switch (companyMode)
            {
                case FormMode.Delete:
                        btnNew.Enabled = true;
                        btnEdit.Enabled = false;
                        btnCancel.Enabled = true;
                        btnSave.Enabled = false;
                        btnDelete.Enabled = false;
                        gbxCompanyInfo.Enabled = false;
                        dgvCompanyList.Enabled = true;

                        EnableFields(false);
                        chkExistingGroup.Checked = false;
                        splitContainer1.Panel2.Enabled = true;
                        break;
                case FormMode.Add:
                        btnNew.Enabled = false;
                        btnEdit.Enabled = false;
                        btnCancel.Enabled = true;
                        btnSave.Enabled = true;
                        btnDelete.Enabled = false;
                        gbxCompanyInfo.Enabled = true;
                        dgvCompanyList.Enabled = false;

                        EnableFields(true);
                        chkExistingGroup.Checked = true;
                        splitContainer1.Panel2.Enabled = false;
                        break;
                case FormMode.Edit:
                        btnNew.Enabled = false;
                        btnEdit.Enabled = false;
                        btnCancel.Enabled = true;
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;
                        gbxCompanyInfo.Enabled = true;
                        dgvCompanyList.Enabled = false;

                        EnableFields(false);
                        chkExistingGroup.Checked = false;
                        splitContainer1.Panel2.Enabled = false;
                        break;
            }
        }
        private void RefreshCompanyGrid()
        {
            try
            {
                dsCompany.DataSource = WmsMobileCompany.GetAllCompany();
                dgvCompanyList.DataSource = dsCompany;                    
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


        private void EnableFields(bool set)
        {
            txtCompanyName.Enabled = set;
            txtGroupName.Enabled = set;
            txtCompanyLocalName.Enabled = set;
            txtGroupLocalName.Enabled = set;
            txtDatabaseName.Enabled = set;
            //txtServerName.Enabled = set;
            //txtServerIP.Enabled = set;
            //txtLicenseCount.Enabled = set;
            chkShared.Enabled = set;
            //txtWebserviceIP.Enabled = set;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                company = new WmsMobileCompany();
                BindComponent();
                companyMode = FormMode.Add;
                EnableButtons();
                txtDatabaseName.Focus();
                cboExistingGroups.DataSource = company.GetGroupNames();
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
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                companyMode = FormMode.Edit;
                EnableButtons();
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
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
               // throw new FMException("This is not yet implemented");
                if (company.DeleteCompany())
                {
                    MessageBox.Show(CommonResource.DeleteSuccess, CommonResource.Alert);
                    RefreshCompanyGrid();
                    companyMode = FormMode.Delete;
                    EnableButtons();
                    if (dsCompany.Count <= 0)
                    {
                        btnEdit.Enabled = false;
                    }
                }
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







        private void btnNewLicense_Click(object sender, EventArgs e)
        {
            try
            {
                if (company != null)
                {
                    if (company.wmsMobileNoOfLicenses.Count > 0)
                    {
                        foreach (WmsMobileNoOfLicense tempLicense in company.wmsMobileNoOfLicenses)
                        {
                            if (tempLicense.validFrom >= DateTime.Today.Date)
                            {
                                throw new FMException("There is already a valid license for today. ");
                            }
                        }
                    }
                    licenseMode = FormMode.Add;
                    license = new WmsMobileNoOfLicense();
                    frmWmsMobileLicense = new FrmWmsMobileLicense(this, licenseMode);
                    frmWmsMobileLicense.ShowDialog();
                }
                RefreshLicenseGrid();
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
        private void btnEditLicense_Click(object sender, EventArgs e)
        {
            try
            {
                if (company != null && license != null)
                {
                    if (license.validTo == null)
                    {
                        licenseMode = FormMode.Edit;
                        frmWmsMobileLicense = new FrmWmsMobileLicense(this, licenseMode);
                        frmWmsMobileLicense.ShowDialog();
                    }
                    else
                        throw new FMException("Can only edit the latest license. ");
                }
                RefreshLicenseGrid();
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
        private void RefreshLicenseGrid()
        {
            try
            {
                if (company != null)
                {
                    company.wmsMobileNoOfLicenses = company.GetMobileLicenses();
                    dsLicense.DataSource = company.wmsMobileNoOfLicenses;
                    dgvLicenseList.DataSource = dsLicense;
                }
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

        private void dsLicense_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (dsLicense.Count > 0)
                {
                    license = (WmsMobileNoOfLicense)dsLicense.Current;
                }
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


        private void RefreshMobileDeviceGrid()
        {
            try
            {
                if (company != null)
                {
                    mobileDevices = WmsMobileDevice.GetAllMobileDevicesByCompany(company.CompanyName.Trim());
                    dsDevices.DataSource = mobileDevices;
                    dgvDeviceList.DataSource = dsDevices;
                }
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

        private void dsDevices_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (dsDevices.Count > 0)
                {
                    mobileDevice = (WmsMobileDevice)dsDevices.Current;
                }
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

        private void btnNewDevice_Click(object sender, EventArgs e)
        {
            try
            {
                if (company != null)
                {
                    mobileMode = FormMode.Add;
                    mobileDevice = new WmsMobileDevice();
                    frmWmsTabletRegistration = new FrmWmsTabletRegistration(this, mobileMode);
                    frmWmsTabletRegistration.ShowDialog();
                }
                RefreshMobileDeviceGrid();    
                mobileMode = FormMode.Delete;
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

        private void btnEditDevice_Click(object sender, EventArgs e)
        {
            try
            {
                if (company != null && mobileDevice != null)
                {
                    mobileMode = FormMode.Edit;
                    frmWmsTabletRegistration = new FrmWmsTabletRegistration(this, mobileMode);
                    frmWmsTabletRegistration.ShowDialog();
                }
                RefreshMobileDeviceGrid();     
                mobileMode = FormMode.Delete;
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

        private void btnDeleteDevice_Click(object sender, EventArgs e)
        {
            try
            {
                if (dsDevices.Count > 0)
                {
                    //set connection when delete device
                    //because it is use to valid if the device is in used or not
                    string[] args = new string[3];
                    args[0] = "ipl";
                    args[1] = this.serverName;
                    args[2] = company.DatabaseName;
                    FMGlobalSettings.TheInstance.SetConnectionString(args);
                    //end connection string

                    mobileDevice = (WmsMobileDevice)dsDevices.Current;
                    if (mobileDevice.DeleteMobileDevice())
                    {
                        MessageBox.Show(CommonResource.DeleteSuccess, CommonResource.Alert);
                        mobileMode = FormMode.Delete;
                    }
                    RefreshMobileDeviceGrid();
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

        private void chkExistingGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExistingGroup.Checked)
            {
                cboExistingGroups.SelectedIndex = -1;
                cboExistingGroups.BringToFront();
            }
            else
                txtGroupName.BringToFront();

        }

        private void cboExistingGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboExistingGroups.SelectedItem != null)
                txtGroupName.Text = cboExistingGroups.Text.ToString().Trim().ToUpper();

        }




    }
}
