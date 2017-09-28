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
using FM.TransportMarket.BLL;
using System.Collections;
using FormBaseLibrary;

namespace FM.TransportMarket.UI
{
    public partial class FrmAddTruckMovementUOM : AbstractForm
    {
        BindingSource bdsUOM = new BindingSource();
        FrmCharge parent;
        public FrmAddTruckMovementUOM(FrmCharge parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void FrmAddTruckMovementUOM_Shown(object sender, EventArgs e)
        {
            ArrayList uomTypes = new ArrayList();
            cboUOMType.Items.Clear();
            if (parent.objCharge.invoiceChargeType == InvoiceChargeType.DependOnWeightVolume)
            {
                //show only weight or volume
                Array uoms = Enum.GetValues(typeof(TruckMovementUOM_WtVol));
                cboUOMCode.DataSource = uoms;
                cboUOMType.Enabled = true;
                uomTypes.Add(UOM_TYPE.Volume);
                uomTypes.Add(UOM_TYPE.Weight);
            }
            else if (parent.objCharge.invoiceChargeType == InvoiceChargeType.DependOnPackagetype)
            {
                //show only package uom      
                Package pack = Package.GetPackageByType(parent.objCharge.packType);
                cboUOMCode.DataSource = PackageUOM.GetAllPackageUOM(pack);
                cboUOMCode.DisplayMember = "packUOMCode";
                uomTypes.Add(UOM_TYPE.Others);
                cboUOMType.Enabled = false;                
            }
            else
            {
                //allow user to select from FM uom
                cboUOMCode.DataSource = Uom.GetUOMs();
                uomTypes.Add(UOM_TYPE.Others);
                cboUOMType.Enabled = false;
            } 
            cboUOMType.DataSource = uomTypes;
        }

        private void cboUOMCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!cboUOMCode.Text.Equals(string.Empty))
                {
                    txtUOMDesc.Text = Uom.GetUomFromFMbyCode(cboUOMCode.Text.ToString()).UomDescription;
                }
            }
            catch { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateField())
                {
                    Uom validUOM = new Uom(cboUOMCode.Text.ToString(), txtUOMDesc.Text.ToString(),
                                            (UOM_TYPE)cboUOMType.SelectedItem);
                    bool uomExist = false;
                    foreach(Uom tempUom in parent.objCharge.validUoms) 
                    {
                        if(tempUom.UomCode.Trim().Equals(validUOM.UomCode.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            uomExist = true;
                            break;
                        }
                    }
                    if (uomExist)
                    {
                        throw new FMException(TptResourceBLL.ErrUOMExist);
                    }
                    else
                    {
                        parent.objCharge.validUoms.Add(validUOM);  //add to memory
                        MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                        //this.Close();
                        parent.Refresh_VaUOMGrid();
                    }
                }
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),CommonResource.Error);
            } 
        }
        private bool ValidateField()
        {
            if (cboUOMCode.Text.Equals(string.Empty))
                throw new FMException(TptResourceBLL.ErrUOMBlank);
            else if (txtUOMDesc.Text.Equals(string.Empty))
                throw new FMException(TptResourceBLL.ErrDescriptionBlank);
            else if (cboUOMType.Text.Equals(string.Empty))
                throw new FMException(TptResourceBLL.ErrEmptyUomType);

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
