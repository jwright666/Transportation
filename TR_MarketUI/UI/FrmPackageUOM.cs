using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using FM.FMSystem.BLL;
using TR_LanguageResource.Resources;
using FM.TransportMarket.BLL;
using FormBaseLibrary;

namespace FM.TransportMarket.UI
{
    public partial class FrmPackageUOM : AbstractForm
    {
        FrmPackages parent;
        FormMode frmMode;
        //BindingSource bdsUOMCodes = new BindingSource();

        public FrmPackageUOM(FrmPackages parent, FormMode frmMode)
        {
            InitializeComponent();
            this.parent = parent;
            this.frmMode = frmMode;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                FillPackageUOMProperties();
                if (frmMode == FormMode.Add)
                {
                    parent.packUOM.AddPackageUOM(parent.package);
                }
                //TODO edit function
                if (frmMode == FormMode.Edit)
                {
                    parent.packUOM.EditPackageUOM();
                }
                this.Close();
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
            parent.RefreshPackUOMGrid();
            this.Close();
        }

        private void FrmPackageType_Shown(object sender, EventArgs e)
        {
            try
            {   
                cboUOMCode.DataSource = Uom.GetUOMs();        
                IntializeDisplay();
                EnableControlByMode();
                //bdsUOMCodes.DataSource = Uom.get.GetAllUoms();           
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
        private void IntializeDisplay()
        {
            if (parent.package != null && parent.packUOM != null)
            {
                cboUOMCode.Text = parent.packUOM.packUOMCode;
                txtPackageType.Text = parent.package.PackType;
                txtPackDescription.Text = parent.packUOM.packUOMDescription;
                txtPackageLength.Text = parent.packUOM.length.ToString(CultureInfo.CurrentCulture);
                txtPackageWidth.Text = parent.packUOM.width.ToString(CultureInfo.CurrentCulture);
                txtPackageHeight.Text = parent.packUOM.height.ToString(CultureInfo.CurrentCulture);
                txtPackageWeight.Text = parent.packUOM.unitWeight.ToString(CultureInfo.CurrentCulture);
            }
            else
            { 
                foreach(Control ctrl in this.Controls)
                {
                    if (ctrl is TextBox)
                        ctrl.Text = string.Empty;
                }
            }               
        }
        private void EnableControlByMode()
        {
            switch (frmMode)
            { 
                case FormMode.Add:
                    cboUOMCode.Enabled = true;
                    cboUOMCode.BackColor = Color.FromArgb(150, 190, 255);//180, 220, 180
                    break;
                case FormMode.Edit:
                    cboUOMCode.Enabled = false;
                    cboUOMCode.BackColor = Color.FromArgb(180, 220, 180);       
                    break;
            }
        }
        private void FillPackageUOMProperties()
        {
            parent.packUOM.packUOMCode = cboUOMCode.Text.ToString().Trim();
            parent.packUOM.packType = txtPackageType.Text.ToString().Trim();
            parent.packUOM.packUOMDescription = txtPackDescription.Text.ToString().Trim();
            parent.packUOM.length = Decimal.Parse(txtPackageLength.Text.ToString(CultureInfo.CurrentCulture));
            parent.packUOM.width = Decimal.Parse(txtPackageWidth.Text.ToString(CultureInfo.CurrentCulture));
            parent.packUOM.height = Decimal.Parse(txtPackageHeight.Text.ToString(CultureInfo.CurrentCulture));
            parent.packUOM.unitWeight = Decimal.Parse(txtPackageWeight.Text.ToString(CultureInfo.CurrentCulture));
        }

        private void cboUOMCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!cboUOMCode.Text.ToString().Equals(string.Empty))
                {
                    Uom selectedUom = Uom.GetUomFromFMbyCode(cboUOMCode.Text.ToString().Trim());
                    txtPackDescription.Text = selectedUom.UomDescription;
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

        private void Measurement_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.NumbersOnlyTextBox(sender, e);
            }
            catch { }
        }

        private void cboUOMCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.AutoCompleteComboBox(sender, e, true);
            }
            catch { }

        }

    }
}
