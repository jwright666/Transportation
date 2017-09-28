using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FM.TransportMarket.BLL;
using FM.FMSystem.BLL;
using FM.Facade;
using TR_LanguageResource.Resources;
using System.Collections;
using System.Globalization;



namespace FM.TransportMarket.UI
{
    public partial class frmQuotationEntry : FormBaseLibrary.frmEntryBase
    {
        private const string frmEntryQuotation = "TPT_HAU_E_QUOTATION";
        private const string frmTransportRate = "TPT_HAU_E_TRANSPORT_RATE";

        public FMModule fmModule;
        private FrmRptQuotation frmRptQuotation;
        private SortableList<Quotation> quotations = new SortableList<Quotation>();
        public Quotation quotation = new Quotation();
        public TransportRate transportrate = new TransportRate();
        private string moduleItem;
        private List<string> startQuotationNoList = new List<string>();
        private List<string> endQuotationNoList = new List<string>();
        //private BindingList<string> bindingList;
        CustomerDTO custDTO = new CustomerDTO();
        int counter = 0;
        bool latest = true;

        public frmQuotationEntry(User user, string moduleItem, FMModule fmModule)
        {
            InitializeComponent();           
            Quotation quotation = new Quotation();
            base.user = user;
            this.moduleItem = moduleItem;
            this.fmModule = fmModule;
            FillComboBox();

            cboCustomerGroup.Visible = false;
            rdbtnCustomerGroup.Visible = false;
            grpQuotationType.Enabled = false;
            counter = 1;
        }

        public void DisableComponentForEditingPurpose()
        {
            dtpValidTo.Enabled = true;
            cboSalesman.Enabled = true;
            txtConditions.Enabled = true;
            txtRemarks.Enabled = true;
            txtRemarks2.Enabled = true;
            txtRemarks3.Enabled = true;
            txtRemarks4.Enabled = true;
            txtRemarks5.Enabled = true;
            txtPaymentTerms.Enabled = true;

            cboBranch.Enabled = false;
            txtQuotationNo.Enabled = false;
            grpQuotationType.Enabled = false;
            dtpQuotationDate.Enabled = false;
            dtpValidFrom.Enabled = false;
            txtCreditDAys.Enabled = false;
        }

        protected override void LocalizeOtherControls()
        {
            #region Buttons
            btnAddTransportRate.Text = CommonResource.Add;
            btnDeleteTransportRate.Text = CommonResource.Delete;
            btnCopyTransportRate.Text = TptResourceUI.CopyTransportRate;
            rdoLatestOnly.Text = CommonResource.LatestQuotationOnly;
            rdoAll.Text = CommonResource.AllQuotation;

            #endregion

            #region Labels
            tabPageMaster.Text = CommonResource.Master;
            tabPageSearch.Text = CommonResource.Search;
            tabPageDetails.Text = CommonResource.Details;
            //Search
            lblSearchQuotationNo.Text = CommonResource.QuotationNo + " :";
            //Master
            lblQuotationNo.Text = CommonResource.QuotationNo + " :";
            grpQuotationType.Text = CommonResource.QuotationType;
            rdbtnCustomerGroup.Text = CommonResource.CustomerGroup;
            rdbtnCustomer.Text = CommonResource.Customer;
            lblBranch.Text = CommonResource.BranchCode + " :";
            lblQuotationDate.Text = CommonResource.QuotationDate + " :";
            lblCreditDays.Text = CommonResource.CreditDays + " :";
            lblValidFrom.Text = CommonResource.ValidFrom + " :";
            lblValidTo.Text = CommonResource.ValidTo + " :";
            lblSalesman.Text = CommonResource.Salesman + " :";
            lblConditions.Text = TptResourceUI.Conditions + " :";
            lblCreditLimit.Text = CommonResource.CreditLimit + " :";
            lblRemarks.Text = CommonResource.Remarks;
            //Details
            lblTransportRate.Text = TptResourceUI.TransportRate;
            lblPriceBreak.Text = TptResourceUI.PriceBreak;
            #endregion

            #region Data Grid
            dgvMaster.Columns["updateVersionDataGridViewImageColumn"].Visible = false;
            dgvTransportRate.Columns["IsSectorDependent"].Visible = false;
            dgvTransportRate.Columns["StartSectorCode"].Visible = false;
            dgvTransportRate.Columns["EndSectorCode"].Visible = false;
            //dvgMaster
            dgvMaster.Columns["quotationIDDataGridViewTextBoxColumn"].HeaderText = CommonResource.QuotationID;
            dgvMaster.Columns["quotationNoDataGridViewTextBoxColumn"].HeaderText = CommonResource.QuotationNo;
            //dgvMaster.Columns["quotationTypeDataGridViewTextBoxColumn"].HeaderText = CommonResource.QuotationType;
            dgvMaster.Columns["custNoDataGridViewTextBoxColumn"].HeaderText = CommonResource.CustNo;
            //dgvMaster.Columns["customerGroupDataGridViewTextBoxColumn"].HeaderText = CommonResource.CustomerGroup;
            dgvMaster.Columns["quotationDateDataGridViewTextBoxColumn"].HeaderText = CommonResource.QuotationDate;
            dgvMaster.Columns["validFromDataGridViewTextBoxColumn"].HeaderText = CommonResource.ValidFrom;
            dgvMaster.Columns["validToDataGridViewTextBoxColumn"].HeaderText = CommonResource.ValidTo;
            dgvMaster.Columns["creditDaysDataGridViewTextBoxColumn"].HeaderText = CommonResource.CreditDays;
            dgvMaster.Columns["salesmanDataGridViewTextBoxColumn"].HeaderText = CommonResource.Salesman;
            dgvMaster.Columns["conditionsDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.Conditions;
            dgvMaster.Columns["paymentTermsDataGridViewTextBoxColumn"].HeaderText = CommonResource.PaymentTerms;
            dgvMaster.Columns["remarksDataGridViewTextBoxColumn"].HeaderText = CommonResource.Remarks;
            dgvMaster.Columns["remarks2DataGridViewTextBoxColumn"].HeaderText = CommonResource.Remarks + "2";
            dgvMaster.Columns["remarks3DataGridViewTextBoxColumn"].HeaderText = CommonResource.Remarks + "3";
            dgvMaster.Columns["remarks4DataGridViewTextBoxColumn"].HeaderText = CommonResource.Remarks + "4";
            dgvMaster.Columns["remarks5DataGridViewTextBoxColumn"].HeaderText = CommonResource.Remarks + "5";
            dgvMaster.Columns["isValidDataGridViewCheckBoxColumn"].HeaderText = TptResourceUI.IsValid;
            dgvMaster.Columns["branchCodeDataGridViewTextBoxColumn"].HeaderText = CommonResource.BranchCode;
            //dgvTransportRate 
            dgvTransportRate.Columns["SequenceNo"].HeaderText = CommonResource.No;
            dgvTransportRate.Columns["chargeIDDataGridViewTextBoxColumn"].HeaderText = CommonResource.ChargeID;
            dgvTransportRate.Columns["chargeTypeDataGridViewTextBoxColumn"].HeaderText = CommonResource.ChargeType;
            dgvTransportRate.Columns["uOMDataGridViewTextBoxColumn"].HeaderText = CommonResource.UOM;
            dgvTransportRate.Columns["minimumDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.MinValue;
            dgvTransportRate.Columns["currencyDataGridViewTextBoxColumn"].HeaderText = CommonResource.Currency;
            dgvTransportRate.Columns["remarksDataGridViewTextBoxColumn1"].HeaderText = CommonResource.Remarks;
            dgvTransportRate.Columns["IsSectorDependent"].HeaderText = CommonResource.IsSectorDependent;
            dgvTransportRate.Columns["StartSectorCode"].HeaderText = CommonResource.Start + CommonResource.SectorCode;
            dgvTransportRate.Columns["EndSectorCode"].HeaderText = CommonResource.End + CommonResource.SectorCode;
            dgvTransportRate.Columns["IsStopDependent"].HeaderText = TptResourceUI.IsStopDependent;
            dgvTransportRate.Columns["StartStop"].HeaderText = TptResourceUI.StartStop;
            dgvTransportRate.Columns["EndStop"].HeaderText = TptResourceUI.EndStop;
            dgvTransportRate.Columns["IsContainerMovement"].HeaderText = TptResourceUI.IsContainerMovement;
            dgvTransportRate.Columns["ContainerCode"].HeaderText = CommonResource.ContainerCode;
            dgvTransportRate.Columns["IsOverweight"].HeaderText = TptResourceUI.IsOverWeight;
            dgvTransportRate.Columns["IsTruckMovement"].HeaderText = TptResourceUI.IsTruckMovement;
            //dgvPriceBreaks 
            dgvPriceBreaks.Columns["seqNoDataGridViewTextBoxColumn"].HeaderText = CommonResource.No;
            dgvPriceBreaks.Columns["endDataGridViewTextBoxColumn"].HeaderText = CommonResource.End;
            dgvPriceBreaks.Columns["isLumpSumDataGridViewCheckBoxColumn"].HeaderText = TptResourceUI.IsLumpSum;
            dgvPriceBreaks.Columns["lumpSumValueDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.LumpSumValue;
            dgvPriceBreaks.Columns["rateDataGridViewTextBoxColumn"].HeaderText = CommonResource.Rate;
            dgvPriceBreaks.Columns["remarksDataGridViewTextBoxColumn2"].HeaderText = CommonResource.Remarks;

            #endregion           

        }

        protected override void SetSecurityControl()
        {
            base.ManageUserAccess(moduleItem, user);
        }

        public void EnableAllComponent()
        {
            dtpValidTo.Enabled = true;
            cboSalesman.Enabled = true;
            txtConditions.Enabled = true;
            txtRemarks.Enabled = true;
            txtRemarks2.Enabled = true;
            txtRemarks3.Enabled = true;
            txtRemarks4.Enabled = true;
            txtRemarks5.Enabled = true;

            cboBranch.Enabled = true;
            txtQuotationNo.Enabled = true;
            grpQuotationType.Enabled = true;
            dtpQuotationDate.Enabled = true;
            dtpValidFrom.Enabled = true;
            txtCreditDAys.Enabled = true;
            txtPaymentTerms.Enabled = true;

        }

        private void UnbindComponent()
        {
            dtpValidTo.DataBindings.Clear();
            dtpValidTo.DataBindings.Clear();
            cboSalesman.DataBindings.Clear();
            txtConditions.DataBindings.Clear();
            txtRemarks.DataBindings.Clear();
            txtRemarks2.DataBindings.Clear();
            txtRemarks3.DataBindings.Clear();
            txtRemarks4.DataBindings.Clear();
            txtRemarks5.DataBindings.Clear();

            cboBranch.DataBindings.Clear();
            txtQuotationNo.DataBindings.Clear();
            grpQuotationType.DataBindings.Clear();
            dtpQuotationDate.DataBindings.Clear();
            dtpValidFrom.DataBindings.Clear();
            txtCreditDAys.DataBindings.Clear();
            txtPaymentTerms.DataBindings.Clear();
            cboCustomerGroup.DataBindings.Clear();
            cboCustomer.DataBindings.Clear();


            bdsTransportRate.DataSource = null;
            bdsPriceBreak.DataSource = null;
        }

        private void BindComponent()
        {
            #region Removed
            /*
            dtpValidTo.DataBindings.Add("Text", bdsMaster, "ValidTo");
            cboSalesman.DataBindings.Add("Text", bdsMaster, "Salesman");
            txtConditions.DataBindings.Add("Text", bdsMaster, "Conditions");
            txtRemarks.DataBindings.Add("Text", bdsMaster, "Remarks");
            txtRemarks2.DataBindings.Add("Text", bdsMaster, "Remarks2");
            txtRemarks3.DataBindings.Add("Text", bdsMaster, "Remarks3");
            txtRemarks4.DataBindings.Add("Text", bdsMaster, "Remarks4");
            txtRemarks5.DataBindings.Add("Text", bdsMaster, "Remarks5");

            cboBranch.DataBindings.Add("Text", bdsMaster, "BranchCode");
            txtQuotationNo.DataBindings.Add("Text", bdsMaster, "QuotationNo");
            grpQuotationType.DataBindings.Add("Text", bdsMaster, "QuotationType");
            dtpQuotationDate.DataBindings.Add("Text", bdsMaster, "QuotationDate");
            dtpValidFrom.DataBindings.Add("Text", bdsMaster, "ValidFrom");
            //txtCreditDAys.DataBindings.Add("Text", bdsMaster, "CreditDAys");
            //txtPaymentTerms.DataBindings.Add("Text", bdsMaster, "PaymentTerms");
            cboCustomerGroup.DataBindings.Add("Text", bdsMaster, "CustomerGroup");
            cboCustomer.DataBindings.Add("Text", bdsMaster, "CustNo");
            */
            #endregion
            if (quotation != null)
            {
                cboCustomerGroup.Text = quotation.CustomerGroup.ToString();
                cboCustomer.Text = quotation.CustNo.ToString();
                //cboCustomerGroup.SelectedIndex = cboCustomerGroup.FindString(quotation.CustomerGroup.ToString());
                //cboCustomer.SelectedIndex = cboCustomerGroup.FindString(quotation.CustNo.ToString());
                dtpValidTo.Value = quotation.ValidTo;
                cboSalesman.Text = quotation.Salesman;
                txtConditions.Text = quotation.Conditions;
                txtRemarks.Text = quotation.Remarks;
                txtRemarks2.Text = quotation.Remarks2;
                txtRemarks3.Text = quotation.Remarks3;
                txtRemarks4.Text = quotation.Remarks4;
                txtRemarks5.Text = quotation.Remarks5;

                cboBranch.Text = quotation.BranchCode;
                txtQuotationNo.Text = quotation.QuotationNo;
                grpQuotationType.Text = quotation.QuotationType.ToString();
                dtpQuotationDate.Value = quotation.QuotationDate;
                dtpValidFrom.Value = quotation.ValidFrom;
                txtCreditDAys.Text = quotation.CreditDays.ToString();
                txtPaymentTerms.Text = quotation.PaymentTerms;
                if (quotation.QuotationType == QuotationType.CustomerGroup)
                {
                    rdbtnCustomerGroup.Checked = true;
                    cboCustomerGroup.Visible = true;
                    cboCustomer.Visible = false;

                }
                else
                {
                    rdbtnCustomer.Checked = true;
                    cboCustomerGroup.Visible = false;
                    cboCustomer.Visible = true;
                }
                bdsTransportRate.DataSource = new BindingList<TransportRate>(quotation.TransportRates);
            }

            dgvTransportRate.DataSource = bdsTransportRate;
            bdsPriceBreak.DataSource = bdsTransportRate;
            bdsPriceBreak.DataMember = "PriceBreaks";
            dgvPriceBreaks.DataSource = bdsPriceBreak;

        }

        private void InitializeComponentForAdd()
        {
            grpQuotationType.Enabled = true;
            dtpValidTo.Value = DateTime.Today;
            //cboCustomer.SelectedIndex = 0;
            //cboSalesman.SelectedIndex = 0;
            cboCustomer.Text  = "";
            cboSalesman.Text = "";
            txtConditions.Text = "";
            txtRemarks.Text = "";
            txtRemarks2.Text = "";
            txtRemarks3.Text = "";
            txtRemarks4.Text = "";
            txtRemarks5.Text = "";

            cboBranch.SelectedIndex = cboBranch.FindString("SIN");
            txtQuotationNo.Text = "";
            grpQuotationType.Enabled = true;
            dtpQuotationDate.Value = DateTime.Today;
            dtpValidFrom.Value = DateTime.Today;
            txtCreditDAys.Text = "";
            txtPaymentTerms.Text = "";
        }

        private void FillComboBoxesForSearchPurpose()
        {

            // Chong  Chin 23 April 2010 - remove 2 lines above - not used
           // bdsQuotationNoFrom.DataSource = Quotation.GetAllQuotationHeader();
           //bdsQuotationNoTo.DataSource = Quotation.GetAllQuotationHeader();
            // Chong Chin 23 April End


            //Feb. 25, 2011 - Gerry Added to load whether latest quotation or not 
            startQuotationNoList = new List<string>();
            startQuotationNoList = Quotation.GetLatestQuotationNo(latest);
            startQuotationNoList.Insert(0, "");

            endQuotationNoList = Quotation.GetLatestQuotationNo(latest);
            endQuotationNoList.Insert(0, "");

            cboStartQuotationNo.DataSource = startQuotationNoList;   
            cboEndQuotationNo.DataSource = endQuotationNoList;
            //end
            
        }

        protected override bool AfterQueryClicked()
        {
            //DeleteQuotationWithNoTransportRate();
            quotations = Quotation.GetLatestQuotationHeader(cboStartQuotationNo.Text.ToString(), cboEndQuotationNo.Text.ToString(), latest);
            bdsMaster.DataSource = quotations;
            lblNoOfRows.Text = "Total Row(s): " + quotations.Count.ToString();
            if (bdsMaster.Count > 0)
            {
                btnPrint.Visible = true;
                btnDeleteMaster.Enabled = true;
                pnlTransportRate.Enabled = true;
            }
            else
            {
                btnPrint.Visible = false;
                btnDeleteMaster.Enabled = false;
                pnlTransportRate.Enabled = false;
            }
            return true;
        }

        protected override void FillComboBox()
        {
            FillComboBoxesForSearchPurpose();
            cboCustomer.DataSource = TransportFacadeIn.GetAllCustomerCodes();
            cboCustomer.SelectedIndex = -1;

            cboCustomerGroup.DataSource = CustomerGroup.GetAllCustomerGroups();
            cboCustomerGroup.DisplayMember = "customerGroupCode";

            cboSalesman.DataSource = Salesman.GetAllSalesman();
            cboSalesman.DisplayMember = "salesmanCode";

            cboBranch.DataSource = BranchDTO.GetAllBranches();
            cboBranch.DisplayMember = "Code";
        }

        protected override bool AfterCancelClicked()
        {
            grpQuotationType.Enabled = false;
            return true;
        }
        protected override bool AfterNewClicked()
        {
            EnableAllComponent();
            UnbindComponent();
            InitializeComponentForAdd();
            //rdbtnCustomerGroup.Checked = true;
            txtCustomerName.Text = "";

            return true;
        }

        protected override bool AfterModifyClicked()
        {
            if (quotation.IsValid)
            {
                DisableComponentForEditingPurpose();
                UnbindComponent();
                BindComponent();
                return true;
            }
            else
            {
                MessageBox.Show("This quotation is not the latest one, you can only Edit the latest quotation. ");
                return false;
            }
        }

        protected override bool AfterDeleteClicked()
        {
            bool clicked = false;
            try
            {
                if (bdsMaster.Count > 0)
                {
                    if (MessageBox.Show(CommonResource.DeleteConfirmation + CommonResource.Quotation, CommonResource.ConfirmToDeleteHeaderName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        quotation = (Quotation)bdsMaster.Current;
                        quotation.DeleteQuotationHeader(fmModule, frmEntryQuotation, user.UserID);
                        clicked = true;
                        formMode = FormMode.Delete;
                    }
                }
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString());
                clicked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                clicked = false;
            }

            return clicked;
        }

        protected override bool AfterSaveClicked()
        {
            bool Clicked = false;
            try
            {
                switch (formMode)
                {
                    case FormMode.Add:
                        quotation = new Quotation();                        
                        FillQuotationProperties();
                        if (quotation.AddQuotationHeader(fmModule, frmEntryQuotation, user.UserID))
                        {
                            txtQuotationNo.Text = quotation.QuotationNo;
                            MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                            formMode = FormMode.Delete;
                            grpQuotationType.Enabled = false;
                            pnlTransportRate.Enabled = true;
                            Clicked = true;
                            //add to search items no need to read from database
                            startQuotationNoList.Add(quotation.QuotationNo);
                            endQuotationNoList.Add(quotation.QuotationNo);
                        }
                        break;

                    case FormMode.Edit:
                        quotation = (Quotation)bdsMaster.Current;
                        FillQuotationProperties();
                        if (quotation.EditQuotationHeader(fmModule, frmEntryQuotation, user.UserID))
                        {
                            MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                            formMode = FormMode.Delete;
                            grpQuotationType.Enabled = false;
                            pnlTransportRate.Enabled = true; 
                            Clicked = true;
                        }

                        break;

                    default:
                        break;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(TptResourceBLL.ErrNotNumeric + " \n" + ex.ToString(), CommonResource.Alert);
                Clicked = false;
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
                Clicked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
                Clicked = false;
            }
            if (Clicked)
            {
                pnlDetails.Enabled = true;
                btnAddTransportRate.Enabled = true;
                btnDeleteTransportRate.Enabled = true;
            }
                return Clicked;
        }

        private void FillQuotationProperties()
        {
            quotation.QuotationNo = txtQuotationNo.Text;
            if (rdbtnCustomerGroup.Checked == true)
            {
                quotation.QuotationType = QuotationType.CustomerGroup;
                quotation.CustomerGroup = cboCustomerGroup.Text;
                quotation.CustNo = "";
            }
            else
            {
                quotation.QuotationType = QuotationType.Customer;
                quotation.CustomerGroup = "";
                quotation.CustNo = cboCustomer.Text;
            }
            if (!custDTO.Equals(null))
            {
                quotation.CreditDays = custDTO.Terms;
                quotation.PaymentTerms = custDTO.CreditLimit.ToString();
            }
            quotation.QuotationDate = dtpQuotationDate.Value;
            quotation.ValidFrom = dtpValidFrom.Value;
            quotation.ValidTo = dtpValidTo.Value;
            quotation.Salesman = cboSalesman.Text;
            quotation.Conditions = txtConditions.Text;

            quotation.Remarks = txtRemarks.Text;
            quotation.Remarks2 = txtRemarks2.Text;
            quotation.Remarks3 = txtRemarks3.Text;
            quotation.Remarks4 = txtRemarks4.Text;
            quotation.Remarks5 = txtRemarks5.Text;
            quotation.BranchCode = cboBranch.Text;
            quotation.IsValid = true;
        }

        private void DeleteQuotationWithNoTransportRate(Quotation objQuotation)
        {
            try
            {
                if (objQuotation.QuotationID != 0)
                {
                    if (objQuotation.TransportRates.Count == 0)
                    {
                        //MessageBox.Show(TptResourceUI.HeaderWillBeDeletedBecauseNoTransportRate,
                        //      CommonResource.QuotationNo + ":" + objQuotation.QuotationNo.ToString());



                        objQuotation.DeleteQuotationHeader(fmModule, frmEntryQuotation, user.UserID);

                    }
                }
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //protected override void CloseForm()
        //{
        //    this.WhenClosing();
        //}

        private void btnAddTransportRate_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdbtnCustomerGroup.Checked == true)
                {
                    FrmTransportRateNew FrmTransportRate = new FrmTransportRateNew(this, FormMode.Add, QuotationType.CustomerGroup, new CustomerDTO(), (CustomerGroup)cboCustomerGroup.SelectedItem, (BranchDTO)cboBranch.SelectedItem);
                    DialogResult result = FrmTransportRate.ShowDialog();
                }
                else
                {
                    // Chong Chin 23 April 2010 
                    CustomerDTO customer = CustomerDTO.GetCustomerByCustCode(cboCustomer.SelectedItem.ToString());
                    FrmTransportRateNew FrmTransportRate = new FrmTransportRateNew(this, FormMode.Add, QuotationType.Customer, customer, new CustomerGroup(), (BranchDTO)cboBranch.SelectedItem);
                    // Chong Chin 23 April 2010 End 
                    DialogResult result = FrmTransportRate.ShowDialog();
                }

                bdsTransportRate.DataSource = new BindingList<TransportRate>(quotation.GetTransportRatesForThisQuotation());
                dgvTransportRate.DataSource = bdsTransportRate;
                bdsPriceBreak.DataSource = bdsTransportRate;
                bdsPriceBreak.DataMember = "PriceBreaks";
                dgvPriceBreaks.DataSource = bdsPriceBreak;
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        private void dgvMaster_Click(object sender, EventArgs e)
        {
            try
            {
                if (bdsMaster.Count != 0)
                {
                    UnbindComponent();
                    BindComponent();
                    if (bdsTransportRate.Count == 0)
                    {
                        btnCopyTransportRate.Enabled = true;
                    }
                }
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bdsMaster_CurrentChanged(object sender, EventArgs e)
        {
            quotation = (Quotation)bdsMaster.Current;
            if (quotation != null)
            {
                BindComponent();
                try
                {
                    quotation.TransportRates = quotation.GetTransportRatesForThisQuotation();
                }
                catch (FMException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                bdsTransportRate.DataSource = new BindingList<TransportRate>(quotation.TransportRates);
                dgvTransportRate.DataSource = bdsTransportRate;

                bdsPriceBreak.DataSource = bdsTransportRate;
                bdsPriceBreak.DataMember = "PriceBreaks";
                dgvPriceBreaks.DataSource = bdsPriceBreak;

                if (quotation.QuotationID != 0)
                {
                    pnlDetails.Enabled = true;
                }
            }
        }

        private void rdbtnCustomerGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbtnCustomerGroup.Checked == true)
            {
                quotation.QuotationType = QuotationType.CustomerGroup;
                cboCustomerGroup.Visible = true;
                cboCustomer.Visible = false;
                txtCustomerName.Visible = false;
            }
            else
            {
                quotation.QuotationType = QuotationType.Customer;
                cboCustomerGroup.Visible = false;
                cboCustomer.Visible = true;
                txtCustomerName.Visible = true;
                showCreditDaysandSalesman();
            }
        }       

        private void btnExitMaster_Click(object sender, EventArgs e)
        {
        }

        
        private void btnCopyTransportRate_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdbtnCustomerGroup.Checked == true)
                {
                    FrmCopyQuotation frmCopyQuotation = new FrmCopyQuotation(this, QuotationType.CustomerGroup, new CustomerDTO(), quotation);
                    DialogResult result = frmCopyQuotation.ShowDialog();
                }
                else
                {
                    // Chong Chin 23 April 2010, Start - Fetch object

                    CustomerDTO customer = CustomerDTO.GetCustomerByCustCode(cboCustomer.SelectedItem.ToString());
                    FrmCopyQuotation frmCopyQuotation = new FrmCopyQuotation(this, QuotationType.Customer, customer, quotation);
                    // Chong Chin 23 April End 
                    DialogResult result = frmCopyQuotation.ShowDialog();
                }
                bdsTransportRate.DataSource = new BindingList<TransportRate>(quotation.GetTransportRatesForThisQuotation());
                dgvTransportRate.DataSource = bdsTransportRate;

                bdsPriceBreak.DataSource = bdsTransportRate;
                bdsPriceBreak.DataMember = "PriceBreaks";
                dgvPriceBreaks.DataSource = bdsPriceBreak;
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void showCreditDaysandSalesman()
        {
            try
            {
            txtCreditDAys.Text = "0";
            if ((cboCustomer.Text != "") && (counter == 1))
            {
             // Chong Chin - 22 April 2010 Fetch object from selection
             //   custDTO = (CustomerDTO)cboCustomer.SelectedItem;
                custDTO = CustomerDTO.GetCustomerByCustCode(cboCustomer.SelectedItem.ToString());
                txtCustomerName.Text = custDTO.Name;
                if (custDTO != null)
                {
                    int index = cboSalesman.FindString(custDTO.Salesman);
                    cboSalesman.SelectedIndex = index;

                    txtCreditDAys.Text = custDTO.Terms.ToString();
                    txtPaymentTerms.Text = custDTO.CurrCode.ToString() + custDTO.CreditLimit.ToString("N2", NumberFormatInfo.CurrentInfo);
                    txtPaymentTerms.ReadOnly = true;
                    dtpValidTo.Value = dtpValidFrom.Value.AddDays(custDTO.Terms);
                }
            }
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cboCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            showCreditDaysandSalesman();
        }

        private void btnDeleteTransportRate_Click(object sender, EventArgs e)
        {
            try
            {
                //20130406 Gerry Added validation
                if (quotation.ValidateEditDeleteRate())
                {

                    if (MessageBox.Show(CommonResource.ConfirmToDeleteHeaderName + TptResourceUI.TransportRate, CommonResource.Confirmation,
                             MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                             == DialogResult.Yes)
                    {
                        quotation.DeleteTransportRate(fmModule, (TransportRate)bdsTransportRate.Current, frmTransportRate, user.UserID);
                    }
                }
                //20130406 end
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            bdsTransportRate.DataSource = new BindingList<TransportRate>(quotation.GetTransportRatesForThisQuotation());
            dgvTransportRate.DataSource = bdsTransportRate;

            bdsPriceBreak.DataSource = bdsTransportRate;
            bdsPriceBreak.DataMember = "PriceBreaks";
            dgvPriceBreaks.DataSource = bdsPriceBreak;
        }      

        //Mar 01, 2011 - Gerry Added - to print a single quotation and display latest quotation
        private void rdoLatestOnly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoLatestOnly.Checked) latest = true;
                else latest = false;

                startQuotationNoList = Quotation.GetLatestQuotationNo(latest);
                startQuotationNoList.Insert(0, "");

                endQuotationNoList = Quotation.GetLatestQuotationNo(latest);
                endQuotationNoList.Insert(0, "");

                cboStartQuotationNo.DataSource = startQuotationNoList;
                cboEndQuotationNo.DataSource = endQuotationNoList;
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (!quotation.Equals(null))
                {
                    if (frmRptQuotation == null || frmRptQuotation.IsDisposed)
                    {
                        frmRptQuotation = new FrmRptQuotation(quotation);
                        frmRptQuotation.WindowState = FormWindowState.Maximized;
                        frmRptQuotation.user = user;
                        frmRptQuotation.module = "TPT_HAU_R_QUOTATION";
                        frmRptQuotation.Show();
                    }
                    else
                        frmRptQuotation.BringToFront();
                }
                SetSecurityControl();
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void cboStartQuotationNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboEndQuotationNo.SelectedItem = cboStartQuotationNo.SelectedItem;
        }
        private void ComboBoxTextChange(object sender, EventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            try
            {
                if (cbo.Text.Length > 0)
                {
                    string temp = cbo.Text.ToString();
                    if (cbo.Items.Count > 0)
                    {
                        for (int item = 0; item < cbo.Items.Count; item++)
                        {
                            if (cbo.Items[item].ToString().StartsWith(temp))
                            {
                                cbo.SelectedIndex = item;
                                return;
                            }
                        }
                    }
                }
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEditTransportRate_Click(object sender, EventArgs e)
        {
            transportrate = (TransportRate)bdsTransportRate.Current;
            try
            {
                if (rdbtnCustomerGroup.Checked == true)
                {
                    FrmTransportRateNew FrmTransportRate = new FrmTransportRateNew(this, FormMode.Edit, QuotationType.CustomerGroup, new CustomerDTO(), (CustomerGroup)cboCustomerGroup.SelectedItem, (BranchDTO)cboBranch.SelectedItem);
                    DialogResult result = FrmTransportRate.ShowDialog();
                }
                else
                {
                    if (quotation.ValidateEditDeleteRate())
                    {
                        CustomerDTO customer = CustomerDTO.GetCustomerByCustCode(cboCustomer.SelectedItem.ToString());
                        FrmTransportRateNew FrmTransportRate = new FrmTransportRateNew(this, FormMode.Edit, QuotationType.Customer, customer, new CustomerGroup(), (BranchDTO)cboBranch.SelectedItem);
                        DialogResult result = FrmTransportRate.ShowDialog();
                    }
                }
                bdsTransportRate.DataSource = new BindingList<TransportRate>(quotation.GetTransportRatesForThisQuotation());
                dgvTransportRate.DataSource = bdsTransportRate;
                bdsPriceBreak.DataSource = bdsTransportRate;
                bdsPriceBreak.DataMember = "PriceBreaks";
                dgvPriceBreaks.DataSource = bdsPriceBreak;
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ValidateValidFromAndValidToFields()
        {
            string msg = "";
            quotation.ValidateNewValidFromAndValidTo(dtpValidFrom.Value, dtpValidTo.Value, out msg);
            if (!msg.Equals(string.Empty))
                throw new FMException(msg);

            ArrayList quotedJobs = new ArrayList();
            quotedJobs = quotation.GetJobsAndInvoicesForQuotation(dtpValidTo.Value);
            if (quotedJobs.Count > 0)
            {
                string jobs = "There are jobs or invoices used the previous quotation with in date range. They are \n";// string.Join(" - ", quotedJobs.ToArray);
                jobs = string.Format(jobs);
                for (int i = 0; i < quotedJobs.Count; i++)
                {
                    jobs += quotedJobs[i].ToString() + "\n";
                }
                jobs = string.Format(jobs);
                jobs += "\n If you change, then those jobs and invoices will used the old quotation. \n Do you wish to proceed?";


                DialogResult dr = MessageBox.Show(string.Format(jobs), CommonResource.Warning, MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                    dtpValidTo.Value = quotation.ValidTo;
            }
            
        }

        private void dtpValidTo_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (formMode == FormMode.Edit || formMode == FormMode.Add)
                {
                    ValidateValidFromAndValidToFields();
                }
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString());
                dtpValidTo.Value = quotation.ValidTo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                dtpValidTo.Value = quotation.ValidTo;
            }
                
            
        }

        private void tbcEntry_Click(object sender, EventArgs e)
        {
            SetTabControl(formMode);
        }

        private void SetTabControl(FormMode formMode)
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

        private void WhenClosing()
        {
            //quotations = Quotation.GetLatestQuotationHeader("", "", false);
            //if (quotations.Count > 0)
            //{
                for (int i = 0; i < quotations.Count; i++)
                {
                    quotation = quotations[i];
                    DeleteQuotationWithNoTransportRate(quotation);
                }
            //}
        }

        protected override void frmEntryBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MessageBox.Show(CommonResource.ConfirmToCloseForm, CommonResource.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    quotations = Quotation.GetLatestQuotationHeader("", "", false);
                    string msg = "{0} \n" + CommonResource.DeleteConfirmation;
                    if (quotations.Count != 0)
                    {
                        string noRatesList = "";
                        //first loop to get quotations id for no transport rates
                        foreach (Quotation tempQuotation in quotations)
                        {
                            if (tempQuotation.TransportRates.Count == 0)
                            {
                                noRatesList += "\n" + tempQuotation.QuotationNo.ToString();
                            }
                        }
                        if (!noRatesList.Equals(""))
                        {
                            msg = string.Format(msg, TptResourceUI.HeaderWillBeDeletedBecauseNoTransportRate + noRatesList);
                            if (MessageBox.Show(msg, CommonResource.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            { 
                                foreach (Quotation tempQuotation in quotations)
                                {
                                    if (tempQuotation.TransportRates.Count == 0)
                                    {
                                        tempQuotation.DeleteQuotationHeader(fmModule, frmEntryQuotation, user.UserID);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CBO_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.AutoCompleteComboBox(sender, e, true);
            }
            catch { }
        }


    }
}
