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
using FM.SeaFreight.BLL;
using FM.Facade;
using FM.TransportMarket.BLL;

namespace FM.TransportMarket.UI
{
    public partial class FrmCharge : frmEntryBase
    {
        public Charge objCharge = new Charge();

        public FrmCharge(User user)
        {
            InitializeComponent();
            base.module = "TPT_HAU_E_CHARGE";
            base.user = user;          
            //chkUseHigherActVolWt.Visible = false;  
            cboChargeCode.DataSource = Charge.GetAllChargesFromSalesCostItem();
        }


        private void InitializeDisplay()
        {
            if (objCharge != null)
            {
                cboBranch.Text = objCharge.BranchCode.ToString();
                cboCustVend.Text = objCharge.CustVendorType.ToString();
                txtDesc.Text = objCharge.ChargeDescription;
                cboUOM.Text = objCharge.UOM.ToString();
                txtSalesAccCode.Text = objCharge.SalesAccountCode;
                chxIsContainerMov.Checked = objCharge.IsContainerMovement;
                cboContainerCode.Visible = objCharge.IsContainerMovement;
                cboContainerCode.Text = objCharge.ContainerCode.ToString();
                chxIsOverWeight.Checked = objCharge.IsOverweight;
                txtTaxCode.Text = objCharge.TaxCode;
                chxIsTruckMov.Checked = objCharge.IsTruckMovement;       
                cboChargeCode.SelectedIndex = cboChargeCode.FindString(objCharge.ChargeCode.ToString());

                //chkIAD.Checked = objCharge.IsInvoiceAmtDependentOnWtVol;
                //chkUseHigherActVolWt.Checked = objCharge.IsHigherOfActWtVol;
                switch (objCharge.invoiceChargeType)
                {
                    case InvoiceChargeType.DependOnWeightVolume:
                        rdbInvoiceDependsOnWtVol.Checked = true;
                        break;
                    case InvoiceChargeType.DependOnTruckCapacity:
                        rdbDependsOnCapacity.Checked = true;
                        break;
                    case InvoiceChargeType.DependOnPackagetype:
                        rdbInvoiceDependsOnPackage.Checked = true;
                        cboPackageType.Text = objCharge.packType.ToString();
                        break;
                    default:
                        rdbInvoiceDependsOnWtVol.Checked = true;
                        break;

                }

                Refresh_VaUOMGrid();
            }

        }


        protected override void LocalizeOtherControls()
        {
            #region Buttons
            btnNewMaster.Text = CommonResource.New;
            btnEditMaster.Text = CommonResource.Edit;
            btnDeleteMaster.Text = CommonResource.Delete;
            btnQuery.Text = CommonResource.Query;
            btnSave.Text = CommonResource.Save;
            btnCancel.Text = CommonResource.Cancel;
            btnClose.Text = CommonResource.Close;
            btnPrint.Text = CommonResource.Print;

            #endregion

            #region Labels
            tabPageMaster.Text = CommonResource.Master;
            tabPageSearch.Text = CommonResource.Search;
            //Search
            lblBranchCode.Text = CommonResource.BranchCode + ":";
            lblVendorType.Text = TptResourceUI.VendorType + ":";
            //master
            lblAddBranchCode.Text = CommonResource.BranchCode + ":";
            lblAddVendorType.Text = TptResourceUI.VendorType + ":";
            lblChargeCode.Text = CommonResource.ChargeCode + ":";
            lblDescription.Text = CommonResource.Description + ":";
            lblUOM.Text = CommonResource.UOM + ":";
            lblSalesAcctCode.Text = CommonResource.SalesAcctCode + ":";
            lblTaxCode.Text = CommonResource.TaxCode + ":";
            lblContainerCode.Text = CommonResource.ContainerCode + ":";
            chxIsContainerMov.Text = TptResourceUI.IsContainerMovement;
            chxIsOverWeight.Text = TptResourceUI.IsOverWeight;
            chxIsTruckMov.Text = TptResourceUI.IsTruckMovement;
            #endregion

            #region DataGrid
            dgvMaster.AutoGenerateColumns = false;
            //dgvMaster
            dgvMaster.Columns["chargeCodeDataGridViewTextBoxColumn"].HeaderText = CommonResource.ChargeCode;
            dgvMaster.Columns["chargeDescriptionDataGridViewTextBoxColumn"].HeaderText = CommonResource.ChargeDescription;
            dgvMaster.Columns["uOMDataGridViewTextBoxColumn"].HeaderText = CommonResource.UOM;
            dgvMaster.Columns["salesAccountCodeDataGridViewTextBoxColumn"].HeaderText = CommonResource.SalesAcctCode;
            dgvMaster.Columns["isContainerMovementDataGridViewCheckBoxColumn"].HeaderText = TptResourceUI.IsContainerMovement; ;
            dgvMaster.Columns["isOverweightDataGridViewCheckBoxColumn"].HeaderText = TptResourceUI.IsOverWeight;
            dgvMaster.Columns["containerCodeDataGridViewTextBoxColumn"].HeaderText = CommonResource.ContainerCode;
            dgvMaster.Columns["isTruckMovementDataGridViewCheckBoxColumn"].HeaderText = TptResourceUI.IsTruckMovement;
            dgvMaster.Columns["taxCodeDataGridViewTextBoxColumn"].HeaderText = CommonResource.TaxCode;
            dgvMaster.Columns["branchCodeDataGridViewTextBoxColumn"].HeaderText = CommonResource.BranchCode;
            dgvMaster.Columns["custVendorTypeDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.VendorType;


            #endregion
        }

        public void ControlComponent(bool set)
        {
            cboBranch.Enabled = set;
            cboCustVend.Enabled =set;
            cboChargeCode.Enabled = set;
            //txtDesc.Enabled = set;
            cboUOM.Enabled = set;
            txtSalesAccCode.Enabled = set;
            chxIsContainerMov.Enabled = set;
            cboContainerCode.Enabled = set;
            chxIsOverWeight.Enabled = set;
            txtTaxCode.Enabled = set;
            chxIsTruckMov.Enabled = set;
            //lblContainerCode.Visible = set;
            //cboContainerCode.Visible = set;
            //chxIsOverWeight.Visible = set;
            pnlTruckMovementDetails.Enabled = set;
            gbxTruckMovementOption.Enabled = set;
        }

        protected override void FillComboBox()
        {
            try
            {
                //201506 - gerry modify get the list uoms codes intead on objects
                //SortableList<Uom> Uoms = Uom.GetAllUoms();
                List<string> uoms = Uom.GetUOMs();
                uoms.Insert(0, string.Empty);
                cboUOM.DataSource = uoms;
                //cboUOM.DisplayMember = "UomCode";

                ContainerTypesDTO blankconttype = new ContainerTypesDTO();
                List<ContainerTypesDTO> containertypes = new List<ContainerTypesDTO>();
                containertypes = SeaFreightFacadeOut.GetAllContainerTypes();
                containertypes.Insert(0, blankconttype);
                cboContainerCode.DataSource = containertypes;
                cboContainerCode.DisplayMember = "Code";

                List<BranchDTO> branches = new List<BranchDTO>();
                branches = BranchDTO.GetAllBranches();
                branches.Insert(0, new BranchDTO());
                cboBranch.DataSource = branches;
                cboBranch.DisplayMember = "Code";

                cboSearchBranch.DataSource = branches;
                cboSearchBranch.DisplayMember = "Code";

                BindingSource bs = new BindingSource();
                bs.DataSource =CustomerDTO.GetCustVendType();
                cboCustVend.DataSource = bs;
                cboCustVend.DisplayMember = "Key";

                cboSearchCustVendor.DataSource = bs;
                cboSearchCustVendor.DisplayMember = "Key";

                //20130903 -Gerry added
                cboPackageType.DataSource = Package.GetAllPackageTypes();
                cboPackageType.DisplayMember = "PackType";
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Alert); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Alert); }
        }

        protected override bool AfterQueryClicked()
        {
            try
            {
                if (cboSearchBranch.Text == "" && cboSearchCustVendor.Text == "")
                {
                    bdsMaster.DataSource = Charge.GetAllTransportCharges();
                }
                else
                    bdsMaster.DataSource = Charge.GetAllCharges(cboSearchBranch.Text, cboSearchCustVendor.Text);

                return true;
            }
            catch { return false; }
        }
        protected override bool AfterNewClicked()
        {
            try
            {
                objCharge = new Charge();
                InitializeDisplay();
                ControlComponent(true);

                return true;
            }
            catch { return false; }
        }
        protected override bool AfterModifyClicked()
        {
            try
            {
                InitializeDisplay();
                ControlComponent(false);
                if (objCharge.IsTruckMovement)
                {
                    //cboUOM.Enabled = true;
                    pnlTruckMovementDetails.Enabled = true;
                    gbxTruckMovementOption.Enabled = false;
                }
                return true;
            }
            catch { return false; }
        }
        protected override bool AfterDeleteClicked()
        {
            try
            {
                DialogResult dr = MessageBox.Show(CommonResource.ConfirmToDeleteHeaderName + TptResourceUI.Charge, CommonResource.Confirmation, MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    Charge tempcharge = new Charge();
                    tempcharge = (Charge)bdsMaster.Current;
                    tempcharge.DeleteCharge();
                    MessageBox.Show(CommonResource.DeleteSuccess, CommonResource.Alert);
                    btnQuery.PerformClick();
                    return true;
                }
                else
                    return false;
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                return false;
            }
        }
        protected override bool AfterSaveClicked()
        {
            try
            {
                FillChargeProperties();
                if (formMode == FormMode.Add)
                {
                    if (objCharge.AddCharge())
                        MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);  
                }
                if (formMode == FormMode.Edit)
                {
                    if (objCharge.EditCharge())
                        MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                }
                formMode = FormMode.Delete;
                return true;
            }
            catch (FMException fmEx)
            { 
                MessageBox.Show(fmEx.ToString(), CommonResource.Alert);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
                return false;
            }
        }


        private void chxIsContainerMov_CheckedChanged(object sender, EventArgs e)
        {
            if (chxIsContainerMov.Checked)
            {
                chxIsTruckMov.Checked = false;
                chxIsTruckMov.Visible  = false;
                lblContainerCode.Visible = true;
                cboContainerCode.Visible = true;
                chxIsOverWeight.Visible = true;
            }
            else
            {
                chxIsTruckMov.Visible = true;
                lblContainerCode.Visible = false;
                cboContainerCode.Visible = false;
                chxIsOverWeight.Checked = false;
                chxIsOverWeight.Visible = false;

            }
        }

        private void FillChargeProperties()
        {
            //objCharge = new Charge();
            objCharge.BranchCode = cboBranch.Text.ToString().Trim();
            objCharge.CustVendorType = cboCustVend.Text.ToString().Trim();
            objCharge.ChargeCode = cboChargeCode.Text.ToString().Trim();
            objCharge.ChargeDescription = txtDesc.Text.ToString();
            objCharge.UOM = cboUOM.Text.ToString().Trim();
            objCharge.SalesAccountCode = txtSalesAccCode.Text.ToString().Trim();
            objCharge.TaxCode = txtTaxCode.Text.ToString().Trim();
            objCharge.IsContainerMovement = chxIsContainerMov.Checked;
            objCharge.ContainerCode = cboContainerCode.Text.ToString().Trim();
            objCharge.IsOverweight = chxIsOverWeight.Checked;
            objCharge.IsTruckMovement = chxIsTruckMov.Checked;

            //objCharge.IsInvoiceAmtDependentOnWtVol = chkIAD.Checked;
            //objCharge.IsHigherOfActWtVol = chkUseHigherActVolWt.Checked;

            //20130903 - added features
            if (rdbInvoiceDependsOnWtVol.Checked)
            {
                objCharge.invoiceChargeType = InvoiceChargeType.DependOnWeightVolume;
                objCharge.packType = string.Empty;                
            }
            else if (rdbDependsOnCapacity.Checked)
            {
                objCharge.invoiceChargeType = InvoiceChargeType.DependOnTruckCapacity;
                objCharge.packType = string.Empty;
            }
            else if (rdbInvoiceDependsOnPackage.Checked)
            {
                objCharge.invoiceChargeType = InvoiceChargeType.DependOnPackagetype;
                objCharge.packType = cboPackageType.Text.ToString();
            }
        }

        private void cboCustVend_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SortableList<Charge> Charges = Charge.GetAllChargesFromSalesCostItem(cboBranch.Text.ToString().Trim(), cboCustVend.Text.ToString().Trim());
                //SortableList<Charge> Charges = Charge.GetAllTransportCharges();//(cboBranch.Text.ToString().Trim(), cboCustVend.Text.ToString().Trim());
                Charges.Insert(0, new Charge());
                cboChargeCode.DataSource = Charges;
                cboChargeCode.DisplayMember = "ChargeCode";
            }
            catch { }
        }

        private void cboChargeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Charge tempCharge = (Charge)cboChargeCode.SelectedItem;
                if (tempCharge != null)
                {
                    if (formMode == FormMode.Add)
                    {
                        txtDesc.Text = tempCharge.ChargeDescription;
                        txtSalesAccCode.Text = tempCharge.SalesAccountCode;
                        txtTaxCode.Text = tempCharge.TaxCode;
                        //cboUOM.Text = tempCharge.UOM;
                    }
                }
            }
            catch { }
        }

        private void chxIsTruckMov_CheckedChanged(object sender, EventArgs e)
        {  
            lblWarnInvAmtDependent.Text = "Note :\n" + TptResourceBLL.WarnValidUOMForJobTripDependentCharge;     
            if (chxIsTruckMov.Checked)
            {      
                chxIsContainerMov.Checked = false;
                pnlTruckMovementDetails.Visible = true;
                lblTruckMovementWarning.Text = TptResourceBLL.WarnTruckMovementCharge;// "Truck movement charge(s) must have at least 1 valid UOM. ";
                cboUOM.Visible = false;
                lblUOM.Visible = false;
                cboUOM.Text = "";
            }
            else
            {
                cboUOM.BackColor = System.Drawing.Color.FromArgb(150, 190, 255);
                pnlTruckMovementDetails.Visible = false;
                lblTruckMovementWarning.Text = "";
                List<string> Uoms = Uom.GetUOMs();
                Uoms.Insert(0, "");
                cboUOM.DataSource = Uoms;
                cboUOM.Visible = true;
                lblUOM.Visible = true;
            }
        }

        private void bdsMaster_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (bdsMaster.Count > 0)
                {
                    objCharge = (Charge)bdsMaster.Current;
                    InitializeDisplay();
                }
            }
            catch { }
        }

        private void tbcEntry_Click(object sender, EventArgs e)
        {
            switch (formMode)
            {
                case FormMode.Delete:
                    if (tbcEntry.SelectedIndex == 0)
                        btnQuery.PerformClick();
                    break;

                default:
                    if (tbcEntry.SelectedIndex == 0)
                        tbcEntry.SelectedTab = tabPageMaster;
                    break;
            }

        }

        public void Refresh_VaUOMGrid()
        {
            if (cboChargeCode.SelectedItem != null)
            {
                dgvValidUoms.DataSource = objCharge.validUoms;
                if (objCharge.validUoms.Count > 0 && chxIsTruckMov.Checked)
                {
                    cboUOM.Text = objCharge.validUoms[0].UomCode.ToString();
                }
                //20131002 - gerry fix to disable truckmovement option if valid uom count > 0
                gbxTruckMovementOption.Enabled = objCharge.validUoms.Count > 0 ? false : true;
            }
        } 

        private void btnAddValidUom_Click(object sender, EventArgs e)
        {
            try
            {
                FillChargeProperties();
                if (objCharge != null)
                {
                    FrmAddTruckMovementUOM frmUom = new FrmAddTruckMovementUOM(this);
                    frmUom.ShowDialog();
                    Refresh_VaUOMGrid();
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

        private void btnDeleteValidUom_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboChargeCode.SelectedItem != null)
                {

                    if (dgvValidUoms.Rows.Count > 0)
                    {
                        if (formMode == FormMode.Edit && dgvValidUoms.Rows.Count == 1)
                        { 
                            throw new FMException(TptResourceBLL.ErrCantREmoveAllUomWhenEdit);
                        }
                        DialogResult dr = MessageBox.Show(CommonResource.ConfirmToDeleteHeaderName + CommonResource.UOM, CommonResource.Confirmation, MessageBoxButtons.YesNo);
                        string selectedUOMCode = dgvValidUoms.CurrentRow.Cells[0].Value.ToString();
                        if (dr == DialogResult.Yes)
                        {
                            if (objCharge.DeleteValidUOM(selectedUOMCode))
                            {
                                Refresh_VaUOMGrid();
                            }
                        }
                    }                        
                }
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString(), CommonResource.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
            }
        }

        private void cboUOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.AutoCompleteComboBox(sender, e, true);
            }
            catch { }
        }

        private InvoiceChargeType GetInvoiceChargeType()
        {
            return InvoiceChargeType.Others;
        }

        private void rdbInvoiceDependsOnPackage_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbInvoiceDependsOnPackage.Checked)
            {
                lblPackageType.Show();
                cboPackageType.Show();
            }
            else
            { 
                lblPackageType.Hide();
                cboPackageType.Hide();                
            }
        }
      
    }
}
