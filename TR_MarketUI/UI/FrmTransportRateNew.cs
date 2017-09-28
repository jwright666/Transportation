using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.TransportMarket.BLL;
using FM.FMSystem.BLL;
using FM.TransportMaintenanceDLL.BLL;
using TR_LanguageResource.Resources;
using FormBaseLibrary;
using FM.Facade;
using System.Globalization;

namespace FM.TransportMarket.UI
{
    public partial class FrmTransportRateNew : AbstractForm
    {
        private const string frmTransportRate = "TPT_HAU_E_TRANSPORT_RATE";
        private Charge charge = new Charge();
        private frmQuotationEntry parent;
        public TransportRate transportrate = new TransportRate();
        private QuotationType quotationType;
        private string custCode;
        private CustomerDTO cust;
        private CustomerGroup custgroup;
        private BranchDTO branch;
        private FormMode frmMode;
        private Quotation quotation;
        //private BindingSource bdsStartStops = new BindingSource();
        //private BindingSource bdsEndStops = new BindingSource();

        public FrmTransportRateNew()
        {
            InitializeComponent();
            Localize();
        }

        public FrmTransportRateNew(frmQuotationEntry parent, FormMode frmMode, QuotationType quotationType,CustomerDTO cust,CustomerGroup custgroup, BranchDTO branch)
        {
            InitializeComponent();
            this.parent = parent;
            this.quotationType = quotationType;
            this.custCode = cust.Code;
            this.cust = cust;
            this.custgroup = custgroup;
            this.branch = branch;
            this.transportrate = parent.transportrate;
            this.frmMode = frmMode;
            this.quotation = parent.quotation;
            Localize();
        }

        public void Localize()
        {
            //Labels
            lblChargeCode.Text = CommonResource.ChargeCode + ":";
            lblDescription.Text = CommonResource.Description + ":";
            lblChargeType.Text = CommonResource.RateType + ":";
            lblUOM.Text = CommonResource.UOM + ":";
            lblMinimumValue.Text = TptResourceUI.MinValue + ":";
            lblCurrencyCode.Text = CommonResource.Currency + ":";
            lblRemarks.Text = CommonResource.Remarks + ":";
            lblContainerCode.Text = CommonResource.ContainerCode + ":";
            lblPriceBreak.Text = TptResourceUI.PriceBreak;

            //CheckBox
            chkboxContainerMovement.Text = TptResourceUI.IsContainerMovement;
            chkboxOverweight.Text = TptResourceUI.IsOverWeight;
            chkboxTruckMovement.Text = TptResourceUI.IsTruckMovement;
           
            //RadioButtons
            rdbNone.Text = CommonResource.None;
            rdbStopRelated.Text = TptResourceUI.StopRelated;
            rdbSectorRelated.Text = TptResourceUI.SectorRelated;
           
            //Button
            btnAdd.Text = CommonResource.Add;
            btnDelete.Text = CommonResource.Delete;
            btnSave.Text = CommonResource.Save;
            btnCancel.Text = CommonResource.Cancel;

            //DataGrid
            dgvPriceBreaks.Columns["seqNoDataGridViewTextBoxColumn"].HeaderText = CommonResource.No;
            dgvPriceBreaks.Columns["endDataGridViewTextBoxColumn"].HeaderText = CommonResource.End;
            dgvPriceBreaks.Columns["isLumpSumDataGridViewCheckBoxColumn"].HeaderText = TptResourceUI.IsLumpSum;
            dgvPriceBreaks.Columns["lumpSumValueDataGridViewTextBoxColumn"].HeaderText = TptResourceUI.LumpSumValue;
            dgvPriceBreaks.Columns["rateDataGridViewTextBoxColumn"].HeaderText = CommonResource.Rate;
            dgvPriceBreaks.Columns["remarksDataGridViewTextBoxColumn"].HeaderText = CommonResource.Remarks;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cboChargeCode.Text == "")
            {
                MessageBox.Show(TptResourceBLL.ErrMissingChargeCode);
            }
            else
            {
                FrmPriceBreakNew FrmTransportPriceBreak = new FrmPriceBreakNew(this);
                DialogResult result = FrmTransportPriceBreak.ShowDialog();
            }
            bdsPriceBreaks.DataSource = new BindingList<PriceBreaks>(transportrate.PriceBreaks);
           
            //dgvPriceBreaks.DataSource = bdsPriceBreaks;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            PriceBreaks priceBreaks = (PriceBreaks)bdsPriceBreaks.Current;
            //bdsPriceBreaks.Remove(priceBreaks);
            transportrate.PriceBreaks.Remove(priceBreaks);
            if (transportrate.PriceBreaks.Count > 0)
            {
                for (int i = 0; i < transportrate.PriceBreaks.Count; i++)
                {
                    if (transportrate.PriceBreaks[i].Seq_No > priceBreaks.Seq_No)
                    {
                        transportrate.PriceBreaks[i].Seq_No = transportrate.PriceBreaks[i].Seq_No - 1;
                    }
                }
            }
            bdsPriceBreaks.DataSource = transportrate.PriceBreaks;
        }

        private void FillTransportRateProperties()
        {
            transportrate.ChargeID = cboChargeCode.Text;
            transportrate.ChargeType = cboChargeType.Text;
            transportrate.UOM = cboUOM.Text;
            transportrate.Minimum = Convert.ToDecimal(txtMinimum.Text);
            transportrate.Currency = cboCurrencyCode.Text;
            transportrate.Remarks = txtRemarks.Text;
            transportrate.Description = txtDescription.Text;
            //Stop or Sector Dependent
            if (rdbNone.Checked)
            {
                transportrate.IsStopDependent = false;
                transportrate.StartStop = "";
                transportrate.EndStop = "";
                transportrate.IsSectorDependent = false;
                transportrate.StartSectorCode = "";
                transportrate.EndSectorCode = "";
            }
            else
            {
                if (rdbStopRelated.Checked)
                {
                    transportrate.IsStopDependent = true;
                    transportrate.StartStop = cboStartStop_Sector.Text;
                    transportrate.EndStop = cboEndStop_Sector.Text;
                    transportrate.IsSectorDependent = false;
                    transportrate.StartSectorCode = "";
                    transportrate.EndSectorCode = "";
                }
                else if(rdbSectorRelated.Checked)
                {
                    transportrate.IsSectorDependent = true;
                    transportrate.StartSectorCode = cboStartStop_Sector.Text;
                    transportrate.EndSectorCode = cboEndStop_Sector.Text;
                    transportrate.IsStopDependent = false;
                    transportrate.StartStop = "";
                    transportrate.EndStop = "";
                    
                }
            }
            //isContainerMovement
            if (chkboxContainerMovement.Checked == true)
            {
                transportrate.IsContainerMovement = true;
                transportrate.ContainerCode = txtContainerCode.Text;
            }
            else
            {
                transportrate.IsContainerMovement = false;
                transportrate.ContainerCode = "";
            }
            //isTruckMovement
            if (chkboxTruckMovement.Checked == true)
                transportrate.IsTruckMovement = true;
            else
                transportrate.IsTruckMovement = false;
            //isOverweight
            if (chkboxOverweight.Checked)
                transportrate.IsOverweight = true;
            else
                transportrate.IsOverweight = false;
            //number of legs
            if (rdoRoundTrip.Checked)
                transportrate.NoOfLeg = 2;
            else if (rdoOneLeg.Checked)
                transportrate.NoOfLeg = 1;
            else
                transportrate.NoOfLeg = 0;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                FillTransportRateProperties();
                switch (frmMode)
                {
                    case FormMode.Add:
                        transportrate.IsNew = true;
                        this.parent.quotation.AddTransportRate(parent.fmModule, transportrate, frmTransportRate, parent.user.UserID);
                        MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                        break;
                    case FormMode.Edit:
                        transportrate.IsNew = false;
                        //this.parent.quotation.TransportRates.Remove(this.parent.transportrate);
                        this.parent.quotation.EditTransportRate(parent.fmModule, transportrate, frmTransportRate, parent.user.UserID);
                        MessageBox.Show(CommonResource.EditSuccess, CommonResource.Alert);
                        
                        break;
                    default:
                        break;
                }
                this.Close();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(TptResourceBLL.ErrNotNumeric, CommonResource.Error);
                //this.parent.quotation.TransportRates.Add(this.parent.transportrate);
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                //this.parent.quotation.TransportRates.Add(this.parent.transportrate); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                this.parent.quotation.TransportRates.Add(this.parent.transportrate);
            }
        }

        private void DisplayData(TransportRate transportRate)
        {
            cboChargeCode.Text = transportrate.ChargeID;
            txtDescription.Text = transportrate.Description;
            cboChargeType.Text = transportrate.ChargeType;
            cboUOM.Text = transportrate.UOM;
            txtMinimum.Text = transportrate.Minimum.ToString("N2", NumberFormatInfo.CurrentInfo);
            cboCurrencyCode.Text = transportrate.Currency;
            txtRemarks.Text = transportrate.Remarks;
            if (transportrate.IsStopDependent)
                rdbStopRelated.Checked = true;
            else
                rdbNone.Checked = true;

            if (transportrate.IsSectorDependent)
                rdbSectorRelated.Checked = true;
            else if (transportrate.IsStopDependent)
                rdbStopRelated.Checked = true;
            else
                rdbNone.Checked = true;

            if (transportrate.IsOverweight)
                chkboxOverweight.Checked = true;
            else
                chkboxOverweight.Checked = false;



            if (transportrate.NoOfLeg == 2)
                rdoRoundTrip.Checked = true;
            else if (transportrate.NoOfLeg == 1)
                rdoOneLeg.Checked = true;
            else
            {
                rdoRoundTrip.Checked = false;
                rdoOneLeg.Checked = false;
                grbNoOfLegs.Visible = false;
            }
        }                                        
        private void SetDataSources()
        {
            cboChargeType.Items.Clear();                         
            cboChargeType.Items.Add(TransportRate.RateType_S);
            cboChargeType.Items.Add(TransportRate.RateType_R);
            cboChargeType.Text = TransportRate.RateType_S;
            chkboxContainerMovement.Checked = true;
            cboUOM.DataSource = Uom.GetUOMs();

            #region Removed
            //cboStartSector.DataSource = Sector.GetAllSectors();
            //cboStartSector.DisplayMember = "SectorCode";
            //cboStartSector.SelectedIndex = -1;
            //cboEndSector.DataSource = Sector.GetAllSectors();
            //cboEndSector.DisplayMember = "SectorCode";
            //cboEndSector.SelectedIndex = -1;
            //cboStartStopSector.DataSource = Stop.GetStopCodes();
            //cboStartStopSector.SelectedIndex = -1;
            //cboEndStopSector.DataSource = Stop.GetStopCodes();
            //cboEndStopSector.SelectedIndex = -1;
            #endregion

            if (quotationType == QuotationType.Customer)
            {
                cboCurrencyCode.DataSource = cust.CurrencyRates;
                cboChargeCode.DataSource = Charge.GetAllCharges(branch.Code, cust.CustVendTypeCode);
            }
            else
            {
                cboCurrencyCode.DataSource = CurrencyRate.GetAllCurrencyRates(DateTime.Today);
                cboChargeCode.DataSource = Charge.GetAllCharges(branch.Code, custgroup.CustVendType);
            }

            string temp = (frmMode == FormMode.Add ? CommonResource.CantAdd : CommonResource.CantEdit);
            temp += " TransportRate. ";
            if (cboChargeCode.Items.Count < 1)
                throw new FMException(temp + TptResourceBLL.ErrNoChargeCreated);
            if (cboCurrencyCode.Items.Count < 1)
                throw new FMException(temp + TptResourceBLL.ErrNoCurrencyFound);

            cboChargeCode.DisplayMember = "chargeCode";
            cboChargeCode.SelectedIndex = -1;
            cboCurrencyCode.DisplayMember = "currencyCode";
            //cboCurrencyCode.SelectedIndex = cboCurrencyCode.FindString("SGD");

        }

        private void FrmTransportRateNew_Load(object sender, EventArgs e)
        {
            try
            {
                SetDataSources();
                txtDescription.Text = "";
                switch (frmMode)
                {
                    case FormMode.Add:
                        transportrate = new TransportRate();
                        cboChargeCode.SelectedIndex = -1;
                        break;
                    case FormMode.Edit:
                        DisplayData(transportrate);
                        break;

                    default:
                        break;
                }
                bdsPriceBreaks.DataSource = transportrate.PriceBreaks;
            }
            catch (FMException fmEx)
            {
                MessageBox.Show(fmEx.ToString(), CommonResource.Alert);
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Alert);
                this.Dispose();
            }
        }

        private void cboChargeCode_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboChargeCode.SelectedItem != null)
                {
                    charge = (Charge)cboChargeCode.SelectedItem;
                    txtDescription.Text = charge.ChargeDescription;
                    txtContainerCode.Text = charge.ContainerCode;
                    rdbNone.Checked = true;  
                    chkboxTruckMovement.Checked = charge.IsTruckMovement;
                    chkboxContainerMovement.Checked = charge.IsContainerMovement; 
                    if (charge.IsContainerMovement)
                    {
                        cboUOM.DataSource = TransportFacadeIn.GetContainerSizes();
                        lblUOM.Text = "Container Size :";
                        grbNoOfLegs.Visible = true;
                        rdoRoundTrip.Checked = true;
                    }
                    //20130405 Gerry Added for truck valid uom
                    else if (charge.IsTruckMovement)
                    {
                        grbNoOfLegs.Visible = false;
                        rdoRoundTrip.Checked = false;
                        #region 20130903 - Gerry Replaced
                        /*
                        if (charge.validUoms.Count <= 0)
                        {
                            cboUOM.DataSource = new List<string>();
                            cboUOM.Text = string.Empty;
                            throw new FMException("No valid UOM for the selected truck movement charge, please edit charge and add valid uom. ");
                        }
                        else
                        {
                            cboUOM.DataSource = charge.validUoms;
                            cboUOM.DisplayMember = "uomCode";
                            lblUOM.Text = "UOM :";
                        }
                         * */
                        #endregion
                        PopulateTruckMovementUOM();
                    }
                    //end 20130405
                    else
                    {
                        cboUOM.DataSource = Uom.GetUOMs();//.GetAllUoms();
                        cboUOM.DisplayMember = "UomCode";
                        lblUOM.Text = "UOM :";
                        chkboxContainerMovement.Checked = false;
                        grbNoOfLegs.Visible = false;
                        rdoRoundTrip.Checked = false;
                        rdoOneLeg.Checked = false;
                    }

                    cboCurrencyCode.DisplayMember = "currencyCode";
                    cboUOM.Text = charge.UOM;
                    chkboxOverweight.Checked = charge.IsOverweight;

                    if (charge.IsTruckMovement || charge.IsContainerMovement)
                        grpDistanceRelated.Enabled = true;
                    else
                        grpDistanceRelated.Enabled = false;
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

        private void cboStartStop_SelectedValueChanged(object sender, EventArgs e)
        {
            if (rdbStopRelated.Checked)
            {
                Stop stop = new Stop();
                stop = Stop.GetStop(cboStartStop_Sector.Text.Trim());
                if (stop != null)
                    txtStartStop.Text = stop.Description;
            }
            else if (rdbSectorRelated.Checked)
            {
                Sector sector = new Sector();
                sector = Sector.GetSectorByCode(cboStartStop_Sector.Text);
                if (sector != null)
                    txtStartStop.Text = sector.SectorDescription;
            }
        }

        private void cboEndStop_SelectedValueChanged(object sender, EventArgs e)
        {
            if (rdbStopRelated.Checked)
            {
                Stop stop = new Stop();
                stop = Stop.GetStop(cboEndStop_Sector.Text.Trim());
                if (stop != null)
                    txtEndStop.Text = stop.Description;
            }
            else if (rdbSectorRelated.Checked)
            {
                Sector sector = new Sector();
                sector = Sector.GetSectorByCode(cboEndStop_Sector.Text);
                if (sector != null)
                    txtEndStop.Text = sector.SectorDescription;
                
            }
        }

        private void EnableSectorandStopGroup()
        {
            cboEndStop_Sector.DataSource = Sector.GetSectors(); 
            if (rdbNone.Checked)
            {
                grpStops.Visible = false;
            }
            else
            {
                grpStops.Visible = true;                
                if (rdbSectorRelated.Checked)
                {
                    grpStops.Text = TptResourceUI.Sector;
                    lblStartStop.Text = "Start Sector";
                    lblendStop.Text = "End Sector";
                    cboStartStop_Sector.DataSource = Sector.GetSectors();
                    cboEndStop_Sector.DataSource = Sector.GetSectors();                    
                    if(transportrate.StartSectorCode != "")
                    {
                        cboStartStop_Sector.Text = transportrate.StartSectorCode;
                        cboEndStop_Sector.Text = transportrate.EndSectorCode;
                    }
                }
                else if (rdbStopRelated.Checked)
                {
                    grpStops.Text = "Stops";
                    lblStartStop.Text = TptResourceUI.StartStop; ;
                    lblendStop.Text = TptResourceUI.EndStop;
                    cboStartStop_Sector.DataSource = Stop.GetStopCodes();
                    cboEndStop_Sector.DataSource = Stop.GetStopCodes();
                    cboStartStop_Sector.Text = quotation.CustNo;
                    cboEndStop_Sector.Text = quotation.CustNo;
                    if (transportrate.StartStop != "")
                    {
                        cboStartStop_Sector.Text = transportrate.StartStop;
                        cboEndStop_Sector.Text = transportrate.EndStop;
                    }
                }

                if (transportrate.StartSectorCode.Equals(string.Empty) && transportrate.StartStop.Equals(string.Empty))
                {
                    cboStartStop_Sector.Text = quotation.CustNo;
                    cboEndStop_Sector.Text = quotation.CustNo;
                }

            }
        }

        private void rdbNone_Click(object sender, EventArgs e)
        {
            EnableSectorandStopGroup();
        }

        private void rdbSectorRelated_Click(object sender, EventArgs e)
        {
            EnableSectorandStopGroup();
        }

        private void rdbStopRelated_Click(object sender, EventArgs e)
        {
            EnableSectorandStopGroup();
        }

        private void cboChargeCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.AutoCompleteComboBox(sender, e, true);
        }

        private void chkboxContainerMovement_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxContainerMovement.Checked)
            {
                grbNoOfLegs.Visible = true;
                rdoRoundTrip.Checked = true;
            }
            else
            {
                grbNoOfLegs.Visible = false;
                rdoRoundTrip.Checked = false;
                rdoOneLeg.Checked = false;
            }
        }

        private void txtMinimum_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.NumbersOnlyTextBox(sender, e);
            }
            catch { }
        }

        //20130903 - Gerry added to populate uom for truck movement charges
        private void PopulateTruckMovementUOM()
        {
            switch (charge.invoiceChargeType)
            { 
                case InvoiceChargeType.DependOnPackagetype:
                    Package pack = Package.GetPackageByType(charge.packType);
                    pack.PackageUOMs = PackageUOM.GetAllPackageUOM(pack);
                    if (pack.PackageUOMs.Count <= 0)
                    {
                        cboUOM.DataSource = new List<string>();
                        cboUOM.Text = string.Empty;
                        throw new FMException("No package uom for the selected truck movement charge, please edit charge and add package uom. ");
                    }
                    else
                    {
                        cboUOM.DataSource = pack.PackageUOMs;
                        cboUOM.DisplayMember = "packUOMCode";
                        lblUOM.Text = "UOM :";
                    }
                    break;
                default:
                    if (charge.validUoms.Count <= 0)
                    {
                        cboUOM.DataSource = new List<string>();
                        cboUOM.Text = string.Empty;
                        throw new FMException("No valid UOM for the selected truck movement charge, please edit charge and add valid uom. ");
                    }
                    else
                    {
                        cboUOM.DataSource = charge.validUoms;
                        cboUOM.DisplayMember = "uomCode";
                        lblUOM.Text = "UOM :";
                    }
                    break;
            }


        }
    }
}
