using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TR_LanguageResource.Resources;
using System.Globalization;
using FM.FMSystem.BLL;
using FM.TransportMarket.BLL;

namespace FM.TransportMarket.UI
{
    public partial class FrmPackages : Form//FrmBaseCompanyLicense
    {
        private FormMode packageTypeMode = FormMode.Delete;
        public FormMode packagUOMMode = FormMode.Delete;
        private User user;
        public FM.TransportMarket.BLL.Package package;
        public FM.TransportMarket.BLL.PackageUOM packUOM;

        public FrmPackages()
        {
            InitializeComponent();
        }
        public FrmPackages(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        #region Packages Methods
        private void RefreshPackageGrid()
        {
            try
            {
                bdsPackage.DataSource = Package.GetAllPackageTypes();
                dgvPackageList.DataSource = bdsPackage;

                splitContainer1.Panel2.Enabled = bdsPackage.Count == 0 ? false : true;
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
        private void BindPackage()
        {
            if (package != null)
            {
                txtPackageType.Text = package.PackType;
                txtPackageDescription.Text = package.PackDescription;
            }
            else
            {
                txtPackageType.Text = string.Empty;
                txtPackageDescription.Text = string.Empty;                
            }
        }    
        private void EnableCompanyButtons()
        {
            switch (packageTypeMode)
            {
                case FormMode.Delete:
                    btnNew.Enabled = true;
                    btnEdit.Enabled = true;
                    btnCancel.Enabled = true;
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = true;
                    gbxPackageInfo.Enabled = false;
                    dgvPackageList.Enabled = true;

                    txtPackageType.Enabled = false;
                    txtPackageDescription.Enabled = false;
                    splitContainer1.Panel2.Enabled = true;
                    break;
                case FormMode.Add:
                    btnNew.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;
                    btnDelete.Enabled = false;
                    btnRefresh.Enabled = false;
                    gbxPackageInfo.Enabled = true;
                    dgvPackageList.Enabled = false;

                    txtPackageType.Enabled = true;
                    txtPackageDescription.Enabled = true;
                    splitContainer1.Panel2.Enabled = false;
                    break;
                case FormMode.Edit:
                    btnNew.Enabled = false;
                    btnEdit.Enabled = false;
                    btnCancel.Enabled = true;
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                    btnRefresh.Enabled = false;
                    gbxPackageInfo.Enabled = true;
                    dgvPackageList.Enabled = false;

                    txtPackageType.Enabled = false;
                    txtPackageDescription.Enabled = false;
                    splitContainer1.Panel2.Enabled = false;
                    break;
            }
            btnEdit.Enabled = false;//disable edit for this moment
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshPackageGrid();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                packageTypeMode = FormMode.Add;
                EnableCompanyButtons();
                package = new Package();
                BindPackage();
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
                packageTypeMode = FormMode.Edit;
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
                //TODO Delete package

                RefreshPackageGrid();
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
                if (!txtPackageType.Text.ToString().Equals(string.Empty))
                {
                    package.PackType = txtPackageType.Text.ToString();
                    package.PackDescription = txtPackageDescription.Text.ToString();

                    if (packageTypeMode == FormMode.Add)
                    {
                        package.AddPackageType();
                        MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                    }
                    else if (packageTypeMode == FormMode.Edit)
                    {
                        package.EditPackageType();
                        MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                    }

                    packageTypeMode = FormMode.Delete;
                    EnableCompanyButtons();
                    RefreshPackageGrid();
                    RefreshPackUOMGrid();
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
            try
            {
                packageTypeMode = FormMode.Delete;
                EnableCompanyButtons();
                RefreshPackageGrid();                
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

        private void FrmPackages_Shown(object sender, EventArgs e)
        {
            try
            {
                this.StartPosition = FormStartPosition.CenterParent;
                packageTypeMode = FormMode.Delete;
                package = new Package();
                EnableCompanyButtons();
                RefreshPackageGrid();
            }
            catch { }
        }

        private void bdsPackage_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (bdsPackage.Count > 0)
                {
                    package = (Package)bdsPackage.Current;
                    BindPackage();
                    RefreshPackUOMGrid();
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

        #endregion


        public void RefreshPackUOMGrid()
        {
            try
            {
                if (package != null)
                {
                    package.PackageUOMs = PackageUOM.GetAllPackageUOM(package);
                    bdsPackageUOM.DataSource = package.PackageUOMs;
                }
                else
                    bdsPackageUOM.DataSource = new SortableList<PackageUOM>();

                dgvPackageUOMList.DataSource = bdsPackageUOM;
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
        private void bdsPackageUOM_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (bdsPackageUOM.Count > 0)
                {
                    packUOM = (PackageUOM)bdsPackageUOM.Current;
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

        private void btnNewPackageUOM_Click(object sender, EventArgs e)
        {
            try
            {
                packUOM = new PackageUOM();
                FrmPackageUOM frmPackUOM = new FrmPackageUOM(this, FormMode.Add);
                frmPackUOM.ShowDialog();  

                RefreshPackUOMGrid();

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

        private void btnEditPackageUOM_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO Edit  package UOM 
                if (packUOM != null)
                {
                    //check if in used
                    if (!packUOM.IsPackageUOMInUsed())
                    {
                        FrmPackageUOM frmPackUOM = new FrmPackageUOM(this, FormMode.Edit);
                        frmPackUOM.ShowDialog();
                    }
                }
                RefreshPackUOMGrid();
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

        private void btnDeletePackageUOM_Click(object sender, EventArgs e)
        {
            try
            {
               //TODO delete package UOM  
                if (packUOM != null)
                {
                    DialogResult dr = MessageBox.Show(CommonResource.ConfirmToDeleteHeaderName + CommonResource.UOM, CommonResource.Confirmation, MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        if (packUOM.DeletePackageUOM())
                        {
                            MessageBox.Show(CommonResource.DeleteSuccess, CommonResource.Alert);
                            RefreshPackUOMGrid();
                        }
                    }
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
    }
}
