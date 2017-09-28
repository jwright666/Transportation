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
using FM.FMSystem.DAL;
using System.Globalization;
using FMLicense.BLL.Transport;
using FMLicenseUI.Common;

namespace FMLicenseUI.Common.Transport
{
    public partial class FrmTPTCompanyLicense : Form//FrmBaseCompanyLicense
    {
        private FormMode companyMode = FormMode.Delete;
        private FormMode licenseMode = FormMode.Delete;
        public TPTCompany company;
        public TPTVehicleNoOfLicense license;
        private FrmTPTVehicelLicense frmVehicleLicense;
        public string serverName;

        public FrmTPTCompanyLicense()
        {
            InitializeComponent();
        }
        public FrmTPTCompanyLicense(string serverName)
        {
            InitializeComponent();
            this.serverName = serverName.Trim();
        }
        #region Button Events
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                companyMode = FormMode.Add;
                EnableCompanyButtons();
                company = new TPTCompany();
                BindCompany();
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
                EnableCompanyButtons();
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
                //if (company.DeleteCompany())
                //    MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);

                RefreshCompanyGrid();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();
                FillCompany();
                if (companyMode == FormMode.Add)
                    company.AddNewCompany();

                companyMode = FormMode.Delete;
                EnableCompanyButtons();
                RefreshCompanyGrid();
                RefreshLicenseGrid();
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
            try
            {
                companyMode = FormMode.Delete;
                EnableCompanyButtons();
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
                    if (company.vehicleNoLicenses.Count > 0)
                    {
                        foreach (TPTVehicleNoOfLicense tempLicense in company.vehicleNoLicenses)
                        {
                            if (tempLicense.validFrom >= DateTime.Today.Date)
                            {
                                throw new FMException("There is already a valid license for today. ");
                            }
                        }
                    }
                    licenseMode = FormMode.Add;
                    frmVehicleLicense = new FrmTPTVehicelLicense(this, licenseMode);
                    frmVehicleLicense.ShowDialog();
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
                if (license != null)
                {
                    if (license.validTo == null)
                    {
                        //if (license.validFrom < DateTime.Today)
                        //{
                        //    throw new FMException("Cannot edit effective date which is before today's date. ");
                        //}
                        //TODO Edit license 
                        licenseMode = FormMode.Edit;
                        frmVehicleLicense = new FrmTPTVehicelLicense(this, licenseMode);
                        frmVehicleLicense.ShowDialog();
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
        #endregion

        private void RefreshCompanyGrid()
        {
            try
            {
                bdsCompany.DataSource = TPTCompany.GetAllCompany();
                dgvCompanyList.DataSource = bdsCompany;

                splitContainer1.Panel2.Enabled = bdsCompany.Count == 0 ? false : true;
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
                    company.vehicleNoLicenses = company.GetVehicleNoLicenses();
                    bdsLicense.DataSource = company.vehicleNoLicenses;
                    dgvLicenseList.DataSource = bdsLicense;
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
        private void ValidateFields()
        {
            string errMsg = string.Empty;
            if (txtDatabaseName.Text.ToString().Trim() == string.Empty)
                errMsg += "\nDatabase name cannot be empty. ";
            if (txtCompanyName.Text.Trim() == string.Empty)
                errMsg += "\nCompany name cannot be empty. ";
            if (txtGroupName.Text == string.Empty)
                errMsg += "\nGroup name cannot be empty. ";

            if (errMsg != string.Empty)
                throw new FMException(errMsg);
        }
        private void BindCompany()
        {
            if (company != null)
            {
                txtDatabaseName.Text = company.DatabaseName;
                txtCompanyName.Text = company.CompanyName;
                txtGroupName.Text = company.GroupName;
                txtCompanyLocalName.Text = company.CompanyNameLocal;
                txtGroupLocalName.Text = company.GroupNameLocal;
                chkShared.Checked = company.ShareWithGoup;
            }
        }

        private void bdsLicense_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (bdsLicense.Count > 0)
                {
                    license = (TPTVehicleNoOfLicense)bdsLicense.Current;
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
        private void bdsCompany_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (bdsCompany.Count > 0)
                {
                    company = (TPTCompany)bdsCompany.Current;
                    BindCompany();
                    company.vehicleNoLicenses = company.GetVehicleNoLicenses();
                    RefreshLicenseGrid();
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

        private void EnableCompanyFields(bool set)
        {
            txtCompanyName.Enabled = set;
            txtGroupName.Enabled = set;
            txtCompanyLocalName.Enabled = set;
            txtGroupLocalName.Enabled = set;
            txtDatabaseName.Enabled = set;
            chkShared.Enabled = set;
        }

        private void EnableCompanyButtons()
        {
            switch (companyMode)
            {
                case FormMode.Delete:
                    btnNew.Enabled = true;
                    btnEdit.Enabled = true;
                    btnCancel.Enabled = true;
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = true;
                    gbxCompanyInfo.Enabled = false;
                    dgvCompanyList.Enabled = true;

                    EnableCompanyFields(false);
                    splitContainer1.Panel2.Enabled = true;
                    chkExistingGroup.Checked = false;
                    break;
                case FormMode.Add:
                    btnNew.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    gbxCompanyInfo.Enabled = true;
                    dgvCompanyList.Enabled = false;

                    EnableCompanyFields(true);
                    splitContainer1.Panel2.Enabled = false;
                    chkExistingGroup.Checked = true;
                    break;
                case FormMode.Edit:
                    btnNew.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = false;
                    gbxCompanyInfo.Enabled = true;
                    dgvCompanyList.Enabled = false;

                    EnableCompanyFields(false);
                    splitContainer1.Panel2.Enabled = false;
                    chkShared.Enabled = true;
                    chkExistingGroup.Checked = false;
                    break;
            }
            btnEdit.Enabled = false;//disable edit for this moment
        }

        private void FillCompany()
        {
            company = new TPTCompany();
            company.CompanyName = txtCompanyName.Text.ToString().Trim().ToUpper();
            company.GroupName = chkExistingGroup.Checked ? cboExistingGroups.Text.ToString().Trim().ToUpper() : txtGroupName.Text.ToString().Trim().ToUpper();
            company.CompanyNameLocal = txtCompanyLocalName.Text.ToString().Trim().ToUpper();
            company.GroupNameLocal = txtGroupLocalName.Text.ToString().Trim().ToUpper();
            company.DatabaseName = txtDatabaseName.Text.ToString().Trim().ToUpper();
            company.SQLServer = this.serverName;
            company.ShareWithGoup = chkShared.Checked;
        }

        private void FrmTPTCompanyLicense_Shown(object sender, EventArgs e)
        {                                                     
            this.StartPosition = FormStartPosition.CenterParent;
            companyMode = FormMode.Delete;
            company = new TPTCompany();
            EnableCompanyButtons();
            RefreshCompanyGrid();
            SetAllTextBoxToUppercase(this);
            cboExistingGroups.DataSource = company.GetGroupNames();
            cboExistingGroups.SelectedIndex = -1;
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshCompanyGrid();
        }

        protected void SetAllTextBoxToUppercase(Control control)
        {
            if (control.HasChildren && (control as DataGridView) == null)
            {
                foreach (Control childControl in control.Controls)
                {
                    SetAllTextBoxToUppercase(childControl);
                }
            }
            else
            {
                if (control is TextBox)
                {
                    TextBox obj = control as TextBox;
                    if (obj.PasswordChar.Equals('\0') && !obj.Name.Equals("txtDatabaseName"))
                        (control as TextBox).CharacterCasing = CharacterCasing.Upper;
                }
                if (control is DataGridView)
                {
                    (control as DataGridView).SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    foreach (DataGridViewColumn col in (control as DataGridView).Columns)
                    {
                        if (col.ValueType == typeof(DateTime))
                        {
                            col.DefaultCellStyle.Format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;//"dd/MM/yyyy";
                        }
                    }
                }
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
