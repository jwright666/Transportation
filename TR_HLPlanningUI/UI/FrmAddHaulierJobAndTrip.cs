using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using TR_LanguageResource.Resources;
using TR_MessageDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_HLPlanDLL.BLL;
using TR_FormBaseLibrary;
using FM.TR_SeaFreightDLL.BLL;
using FM.TR_HLBookDLL.BLL;
using FM.TR_MarketDLL.BLL;
using FM.TR_HLBookDLL.Facade;

namespace FM.TransportPlanning.UI
{
    public partial class FrmAddHaulierJobAndTrip : AbstractForm
    {
        public const string FORM_NAME = "TPT_HAU_E_HAULIER_JOB_FRM_PLAN";
        private HaulierJob haulierJob;
        private CustomerDTO customer;
        private Stop origin;
        private Stop destination;
        private ContainerTypesDTO containerDTO;
        private FrmHaulierPlanningEntry parent;
        private string quotationNo = string.Empty;
        private string warnMsg = string.Empty;
        TransportSettings TRSettings;

        public FrmAddHaulierJobAndTrip(FrmHaulierPlanningEntry parent)
        {
            InitializeComponent();
            this.parent = parent;
            this.TRSettings = TransportSettings.GetTransportSetting();
        }

        private void FrmAddHaulierJobAndTrip_Load(object sender, EventArgs e)
        {
            try
            {
                chkExistingJob.Checked = true;
                chkIsBillable.Checked = false;   //set default to not billable  
                List<string> jobList = HaulierJob.GetAllHaulierJobNo();
                if(jobList.Count > 0)
                    jobList = jobList.OrderByDescending(a => a).ToList();   //sort in descending order
                cboJobNo.DataSource = jobList;
                cboJobNo.Text = string.Empty;
                //population customer datasource add company code to the list of customer,
                //because some jobs are from company(ex. PrimeMover will be send to shop, to get driver incentive must create job)
                TransportSettings transporSettings = TransportSettings.GetTransportSetting();
                List<string> customerList = CustomerDTO.GetAllCustomerCodes();
                if (!customerList.Contains(transporSettings.Tpt_code))
                    customerList.Insert(0, transporSettings.Tpt_code);
                cboCustCode.DataSource = customerList;
                cboCustCode.SelectedIndex = -1; // string.Empty;   

                cboStartStop.DataSource = Stop.GetStopCodes();
                cboStartStop.SelectedIndex = -1;
                cboEndStop.DataSource = Stop.GetStopCodes();
                cboEndStop.SelectedIndex = -1;
                cboContainerCode.DataSource = ContainerTypesDTO.GetAllContainerCodes();
                cboBranchCode.DataSource = BranchDTO.GetBranchCodeByUser(parent.user.UserID.ToString());
                cboBranchCode.DisplayMember = "Code";
                //20140723 - gerry modified
                cboDriverName.DataSource = parent.drivers;
                cboDriverName.DisplayMember = "DescriptionForPlanningPurpose";
                cboDriverName.Text = parent.currentdriver.DescriptionForPlanningPurpose;

                cboJobType.DataSource = HaulierJob.GetHaulierJobTypes();
                cboJobType.SelectedItem = HaulierJob.HAULIER_LOCAL;
                txtDept.Text = parent.deptcode;
                cboChargeJobType.Items.Add(ChargeType.NotStopDependent);
                cboChargeJobType.Items.Add(ChargeType.StopDependent);
                cboChargeJobType.SelectedItem = ChargeType.NotStopDependent;

                dtpBookingDate.Value = parent.chosenDate;
                dtpStartDate.Value = parent.chosenDate;
                txtStartTime.Text = parent.selectedInterval.Start.ToString("HHmm");//.Replace(":","").Substring(0,3);
                dtpEndDate.Value = parent.chosenDate;
                txtEndTime.Text = parent.selectedInterval.End.ToString("HHmm");//.Replace(":", "").Substring(0, 3);
                dtpBookingDate.Enabled = false;
                dtpStartDate.Enabled = false;
                dtpEndDate.Enabled = false;
                this.gbxJobHeader.Enabled = parent.formMode == FormMode.Edit ? true : false;
                this.gbxJobTrip.Enabled = parent.formMode == FormMode.Edit ? true : false;
                this.btnSave.Enabled = parent.formMode == FormMode.Edit ? true : false;

                
                pnlIncentive.Visible = chkExistingJob.Checked ? false : true;
                
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckDriverTimeAvailability()
        {
            try
            {
                int startHr = Convert.ToInt32(txtStartTime.Text.ToString().Substring(0, 2));
                int startMin = Convert.ToInt32(txtStartTime.Text.ToString().Substring(2, 2));
                int endHr = Convert.ToInt32(txtEndTime.Text.ToString().Substring(0, 2));
                int endMin = Convert.ToInt32(txtEndTime.Text.ToString().Substring(2, 2));
                parent.selectedInterval.Start = dtpStartDate.Value.Date.AddHours(startHr).AddMinutes(startMin);
                parent.selectedInterval.End = dtpEndDate.Value.Date.AddHours(endHr).AddMinutes(endMin);
                if (parent.currentdriver != null)
                {
                    if (parent.currentdriver.unAvailableDates.Count > 0)
                    {
                        if (!parent.IsIntervalAllowed(parent.selectedInterval))
                            throw new FMException("Driver is not available with in the selected time");
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                parent.currentdriver = (Driver)cboDriverName.SelectedItem;
                FillHaulierJob();
                //check the availability time of driver
                CheckDriverTimeAvailability();

                if (haulierJob.ValidateAddHaulierJobHeader() && haulierJob.JobTrips[haulierJob.JobTrips.Count - 1].ValidateAddHaulierJobTrip(chkIsShiftingJob.Checked)) //20141108 - gerry added to optional parameter
                {
                    if(chkExistingJob.Checked) //validate for existing job
                        haulierJob.CanAddTripToExistingFromPlanning(haulierJob.JobTrips[haulierJob.JobTrips.Count -1]);
                    //20140923 - gerry added to validate charge code
                    if (haulierJob.IsBillable && haulierJob.QuotationNo != string.Empty
                        && haulierJob.JobTrips[haulierJob.JobTrips.Count - 1].isBillable
                        && haulierJob.JobTrips[haulierJob.JobTrips.Count - 1].ContainerCode != string.Empty
                        && haulierJob.JobTrips[haulierJob.JobTrips.Count - 1].ChargeCode == string.Empty)
                    {
                        throw new FMException(TptResourceUI.NoValidChargeForThisContainer);
                    }
                    //20140923 end

                    //add job trip to the list of jobtrip from scheduler
                    //parent.haulierJobTrips.Add(haulierJob.JobTrips[haulierJob.JobTrips.Count - 1]); //20140908 - gerry removed because duplication job trip will be created
                    //add planSubTrip
                    //Incentive incentive = new Incentive();
                    //incentive.Code = cboIncentiveCode.Text.ToString().Trim();
                    //incentive.
                    PlanHaulierTrip.AddPlanHaulierSubTripInMemory(parent.planHaulierTrips, parent.currentdriver, haulierJob.JobTrips, parent.trailerlocations, cboIncentiveCode.Text.ToString().Trim(), out parent.planHaulierSubTrip1);
                    //add job trip 
                    if (haulierJob.AddHaulierJobAndTripFromPlanningOrSeaFreightSide(FORM_NAME, parent.user.UserID.ToString(), true, chkExistingJob.Checked, chkIsShiftingJob.Checked))
                    {    
                        MessageBox.Show(CommonResource.SaveSuccess, CommonResource.Alert);
                        //TODO set to read only then populate job number
                        txtJobNo.Text = haulierJob.JobNo.ToString();
                        gbxJobHeader.Enabled = false;
                        gbxJobTrip.Enabled = false;
                        btnSave.Enabled = false;
                        btnCancel.Text = CommonResource.Close;
                    }
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void FillHaulierJob()
        {
            try
            {
                //TODO fill up haulier job properties    
                if (!chkExistingJob.Checked) { haulierJob = new HaulierJob(); }
                haulierJob.JobType = cboJobType.Text.ToString().Trim();  //not in specs but needed
                haulierJob.TptDeptCode = parent.deptcode;   //not in specs but needed  
                haulierJob.CustNo = cboCustCode.Text.ToString();
                haulierJob.QuotationNo = rdbCustomer.Checked ? txtQuotationNo.Text.ToString() : string.Empty;
                haulierJob.JobDateTime = dtpBookingDate.Value;
                haulierJob.BranchCode = cboBranchCode.Text.ToString();
                haulierJob.IsBillable = chkIsBillable.Checked;
                haulierJob.HaulierChargeType = (ChargeType)cboChargeJobType.SelectedItem;
                //20141029 - gerry added
                //haulierJob.JobTransferType = JobTransferType.None; 


                //20140818 - gerry modified to get the collection of job trips if existing job
                haulierJob.JobTrips = chkExistingJob.Checked ? HaulierJobTrip.GetAllHaulierJobTrips(haulierJob) : new SortableList<HaulierJobTrip>();
                //haulierJob.JobTrips = new SortableList<HaulierJobTrip>();
                //fill job trip
                haulierJob.JobTrips.Add(FillHaulierJobTrip());
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

        }
        private HaulierJobTrip FillHaulierJobTrip()
        {
            HaulierJobTrip jobTrip = new HaulierJobTrip();
            try
            {
                if (haulierJob != null)
                {
                    //TODO fill up haulier job trip properties
                    jobTrip.JobID = haulierJob.JobID;
                    jobTrip.JobNo = haulierJob.JobNo;
                    jobTrip.StartDate = dtpStartDate.Value;
                    jobTrip.StartTime = txtStartTime.Text.ToString().Trim();
                    jobTrip.EndDate = dtpEndDate.Value;
                    jobTrip.EndTime = txtEndTime.Text.ToString().Trim();
                    jobTrip.CustomerCode = cboCustCode.Text.Trim().ToString();
                    jobTrip.CustomerName = txtCustName.Text.Trim().ToString();
                    #region Origin and Destination
                    //origin
                    jobTrip.StartStop.Code = cboStartStop.Text.ToString().Trim();
                    jobTrip.StartStop.Description = txtDescriptionFrom.Text.ToString().Trim();
                    jobTrip.StartStop.Address1 = txtAddress1From.Text.ToString().Trim();
                    jobTrip.StartStop.Address2 = txtAddress2From.Text.ToString().Trim();
                    jobTrip.StartStop.Address3 = txtAddress3From.Text.ToString().Trim();
                    jobTrip.StartStop.Address4 = txtAddress4From.Text.ToString().Trim();
                    jobTrip.StartStop.City = txtCityFrom.Text.ToString().Trim();
                    //destination
                    jobTrip.EndStop.Code = cboEndStop.Text.ToString().Trim();
                    jobTrip.EndStop.Description = txtDescriptionTo.Text.ToString().Trim();
                    jobTrip.EndStop.Address1 = txtAddress1To.Text.ToString().Trim();
                    jobTrip.EndStop.Address2 = txtAddress2To.Text.ToString().Trim();
                    jobTrip.EndStop.Address3 = txtAddress3To.Text.ToString().Trim();
                    jobTrip.EndStop.Address4 = txtAddress4To.Text.ToString().Trim();
                    jobTrip.EndStop.City = txtCityTo.Text.ToString().Trim();
                    #endregion

                    jobTrip.LegType = Leg_Type.OneLeg;
                    jobTrip.isMulti_leg = chkMultiLeg.Checked ? true : false;
                    jobTrip.isBillable = chkTripBillable.Checked;
                    jobTrip.ContainerCode = cboContainerCode.Text.ToString();
                    jobTrip.ContainerNo = chkMultiLeg.Checked ? cboContainerNo.Text.ToString() : txtContainerNo.Text.ToString().Trim();
                    jobTrip.SealNo = chkMultiLeg.Checked ? cboSealNo.Text.ToString() : txtSealNo.Text.ToString().Trim();
                    jobTrip.GrossWeight = Decimal.Parse(txtGrossWeight.Text);
                    jobTrip.maxWeight = Decimal.Parse(txtMaxWeight.Text);
                    //20140623 - gery added the Reamrk ang cargo description
                    jobTrip.CargoDescription = txtCargoDescription.Text.ToString().Trim();
                    jobTrip.Remarks = txtRemarks.Text.ToString().Trim();
                    //20140623 end
                    //20140822 - gerry added
                    jobTrip.ChargeCode = tbxChargeCode.Text.ToString().Trim();
                    jobTrip.TripStatus = JobTripStatus.Assigned;

                    //20141009 - gerry added
                    int legGroup = chkExistingJob.Checked && chkMultiLeg.Checked ? HaulierJob.GetNextLegGroup(haulierJob.JobID) : 1; //haulierJob.JobTrips.Where(jt => jt.ContainerNo == jobTrip.ContainerNo).ToList()[0].LegGroup;
                    jobTrip.SetLegGroup(legGroup);
                    int legGroupMember = chkExistingJob.Checked && chkMultiLeg.Checked ? HaulierJob.GetNextLegGroupMember(haulierJob.JobID, jobTrip.LegGroup) : -1; //haulierJob.JobTrips.Where(jt => jt.ContainerNo == jobTrip.ContainerNo).ToList()[0].LegGroupMember + 1;
                    jobTrip.SetLegGroupMember(legGroupMember);
                    //20141009 end                    
                    jobTrip.isMovementJob = chkExistingJob.Checked ? true : false;//20150805 - gerry added
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return jobTrip;
        }
        private void CheckQuotation()
        { 
            try
            {
                rdbNoQuotation.Checked = true;
                if (chkIsBillable.Checked)
                {
                    if (cboCustCode.SelectedValue != null)
                    {
                        quotationNo = (Quotation.GetValidQuotationNo(cboCustCode.SelectedValue.ToString(), dtpBookingDate.Value, out warnMsg)).ToString();
                        if (!quotationNo.Equals(string.Empty))
                        {
                            rdbCustomer.Checked = true;
                            rdbCustomer.Enabled = true;
                            txtQuotationNo.Text = quotationNo;
                        }
                        else     // 20140721 gerry modified - don;'t show messagebox because it will the combox to lost focus and fires the event again                            
                        {
                            //MessageBox.Show(TptResourceBLL.WarnCustHasNoQuotation, CommonResource.Information);
                            warnMsg = TptResourceBLL.WarnCustHasNoQuotation;
                            rdbNoQuotation.Checked = true;
                        }
                    }
                }
                else
                    warnMsg = string.Empty;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }      
        private void cboStartStop_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                origin = Stop.GetStop(cboStartStop.Text.ToString().Trim());
                if (origin != null)
                {
                    txtDescriptionFrom.Text = origin.Description;
                    txtAddress1From.Text = origin.Address1;
                    txtAddress2From.Text = origin.Address2;
                    txtAddress3From.Text = origin.Address3;
                    txtAddress4From.Text = origin.Address4;
                    txtCityFrom.Text = origin.City;
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void cboEndStop_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                destination = Stop.GetStop(cboEndStop.Text.ToString().Trim());
                if (destination != null)
                {
                    txtDescriptionTo.Text = destination.Description;
                    txtAddress1To.Text = destination.Address1;
                    txtAddress2To.Text = destination.Address2;
                    txtAddress3To.Text = destination.Address3;
                    txtAddress4To.Text = destination.Address4;
                    txtCityTo.Text = destination.City;
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }

        }

        private void cbo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.AutoCompleteComboBox(sender, e, true);
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void cboContainerCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tbxChargeCode.Text = string.Empty;
                containerDTO = ContainerTypesDTO.GetContainerDTO(cboContainerCode.SelectedValue.ToString().Trim());
                if (containerDTO != null)
                {
                    txtContainerSize.Text = containerDTO.Size.ToString() + " / " + containerDTO.TareWeight.ToString("N2", NumberFormatInfo.CurrentInfo);
                    txtMaxWeight.Text = (containerDTO.TareWeight + Convert.ToDecimal(txtGrossWeight.Text)).ToString("N2", NumberFormatInfo.CurrentInfo);
                    //20140924 - gerry transferred this method and modified
                    // try to get the quotation(if any) for this job
                    SortableList<TransportRate> transportRates = Quotation.GetTransportRatesBasedOnQuotationNo(txtQuotationNo.Text.ToString().Trim());
                    int noOfLeg = chkMultiLeg.Checked ? 2 : 1;
                    // try to find a rate(if any) in the quotation for this uom(containerCode-->containerSize-->UOM)
                    foreach (TransportRate r in transportRates)
                    {
                        if (r.UOM.ToUpper().Equals(containerDTO.Size.ToUpper()) && r.NoOfLeg == noOfLeg)
                        {
                            tbxChargeCode.Text = r.ChargeID;
                        }
                    }
                    if (chkTripBillable.Checked && transportRates.Count > 0 
                        && cboContainerCode.SelectedValue.ToString() != string.Empty 
                        && tbxChargeCode.Text == string.Empty) 
                    {
                        throw new FMException(TptResourceUI.NoValidChargeForThisContainer); 
                    }
                    //20140924 end
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }

        private void txtGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                base.NumbersOnlyTextBox(sender, e);
            }
            catch { }
        }

        private void txtGrossWeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                containerDTO = ContainerTypesDTO.GetContainerDTO(cboContainerCode.Text.ToString().Trim());
                if (containerDTO != null)
                    txtMaxWeight.Text = (containerDTO.TareWeight + Convert.ToDecimal(txtGrossWeight.Text)).ToString("N2", NumberFormatInfo.CurrentInfo);
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }           

        private void txtTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                base.ValidTimeOnly(sender, e);
            }
            catch { }
        }
        //20140828 - gerry modified logic to populate containercodes based on quotation
        private void PopulateContainerCodesFromQuotation()
        {
            try
            {
                List<string> containerCodes = new List<string>();
                switch (quotationNo.ToString().Equals(string.Empty))
                {
                    case false:
                        Quotation quotation = Quotation.GetAllQuotationHeader(quotationNo.ToString());
                        if (haulierJob.HaulierChargeType == ChargeType.StopDependent)  //20140124 - gerry modified  for stop dependent
                            containerCodes = TransportFacadeIn.GetContainerCodesBySizeUsedInQuotationAndJobTrips(quotation.QuotationID, true, 1); //TransportFacadeIn.GetContainerCodesBySizeUsedInQuotationAndJobTrips(quotation.QuotationID, true);
                        else    //20140124 - gerry modified
                            containerCodes = TransportFacadeIn.GetContainerCodesBySizeUsedInQuotationAndJobTrips(quotation.QuotationID, false, 1); //TransportFacadeIn.GetContainerCodesBySizeUsedInQuotationAndJobTrips(quotation.QuotationID, false);

                        if (chkExistingJob.Checked)
                            containerCodes = haulierJob.GetAllContainerCodesForJob().Cast<string>().ToList();

                        break;
                    default:
                        containerCodes = TransportFacadeIn.GetAllContainerCodes();
                        break;
                }
                if (containerCodes.Count > 0)
                    btnSave.Enabled = true;
                else
                {
                    cboContainerCode.Text = string.Empty;
                    warnMsg = TptResourceBLL.ErrNoContainerForQuotation;
                    btnSave.Enabled = false;
                }
                cboContainerCode.DataSource = containerCodes;
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }
        private void rdbNoQuotation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbNoQuotation.Checked)
                {
                    txtQuotationNo.Text = string.Empty;
                    lblQuotationNo.Hide();
                    txtQuotationNo.Hide();
                    chkTripBillable.Checked = false;
                }
                else if (rdbCustomer.Checked)
                {
                    lblQuotationNo.Show();
                    txtQuotationNo.Show();
                    txtQuotationNo.Text = quotationNo;
                    chkTripBillable.Checked = chkIsBillable.Checked ? true : false;
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
            //PopulateContainerCodesFromQuotation();
        }
        //2014024 - gerry added
        private void chkExistingJob_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkExistingJob.Checked)
                {
                    cboJobNo.BringToFront();
                    chkMultiLeg.Checked = true;
                    chkMultiLeg.Enabled = true;
                    cboCustCode.Enabled = false;
                    chkIsBillable.Enabled = false;
                    cboJobType.Enabled = false;   //20140818 - gerry added
                    grpQuotation.Enabled = false;   //20140818 - gerry added
                    chkIsShiftingJob.Checked = false; // 20141117 - gerry added
                    cboIncentiveCode.Text = string.Empty;
                }
                else
                {
                    cboJobNo.SendToBack();
                    chkMultiLeg.Checked = false;
                    chkMultiLeg.Enabled = false;
                    cboCustCode.Enabled = true;
                    cboJobType.SelectedItem = HaulierJob.HAULIER_LOCAL;
                    chkIsBillable.Enabled = !chkIsShiftingJob.Checked; 
                    dtpBookingDate.Value = parent.chosenDate;
                    cboJobType.Enabled = true;  //20140818 - gerry added   
                    grpQuotation.Enabled = true;   //20140818 - gerry added

                    cboIncentiveCode.DataSource = Incentive.GetIncentiveCodes("MN");
                    cboIncentiveCode.SelectedIndex = -1;
                }
                pnlIncentive.Visible = chkExistingJob.Checked ? false : true;
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
            //PopulateContainerCodesFromQuotation();
        }
        //2014024 - gerry added
        private void cboJobNo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkExistingJob.Checked)
                {
                    if ((string)cboJobNo.SelectedValue.ToString() != string.Empty)
                    {
                        haulierJob = HaulierJob.GetHaulierJob(cboJobNo.SelectedValue.ToString());
                        if (haulierJob != null)
                        {
                            PopulateExistingJob();
                            chkTripBillable.Checked = haulierJob.IsBillable;
                            CheckContainerForExistingJob();
                            cboCustCode.SelectedIndex = cboCustCode.FindString(haulierJob.CustNo);
                            CheckQuotation();
                            //set default container code
                            SortableList<HaulierJobTrip> tripList =HaulierJobTrip.GetHaulierJobTripsByContainerNo(haulierJob.JobID, cboContainerNo.Text, false);
                            if (tripList.Count > 0)
                                cboContainerCode.Text = tripList[0].ContainerCode;
                        }
                    }
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void cboJobNo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //CheckContainerForExistingJob();

        }
        //2014024 - gery added
        private void chkMultiLeg_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkMultiLeg.Checked)
                {
                    cboContainerNo.BringToFront();
                    cboSealNo.BringToFront();
                    cboContainerCode.Enabled = false;  //20140818 - gerry added
                }
                else
                {
                    cboContainerNo.SendToBack();
                    cboSealNo.SendToBack();
                    cboContainerCode.Enabled = true;  //20140818 - gerry added
                    txtContainerNo.Text = "";
                    txtSealNo.Text = "";
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }
        //2014024 - gerry added
        private void PopulateExistingJob()
        {
            try
            {
                if (haulierJob != null)
                {
                    dtpBookingDate.Value = haulierJob.JobDateTime;
                    chkIsBillable.Checked = haulierJob.IsBillable;
                    cboJobType.Text = haulierJob.JobType;
                    cboChargeJobType.Text = haulierJob.HaulierChargeType.ToString();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        private void chkTripBillable_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if(rdbNoQuotation.Checked)
                PopulateContainerCodesFromQuotation();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        private void cboCustCode_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboCustCode.SelectedValue != null)
                {
                    customer = CustomerDTO.GetCustomerByCustCode(cboCustCode.SelectedValue.ToString().Trim());
                    if (customer.Code != string.Empty)
                    {
                        rdbCustomer.Enabled = false;
                        txtCustName.Text = customer.Name;
                        txtCustAddress1.Text = customer.address1;
                        txtCustAddress2.Text = customer.address2;
                        txtCustAddress3.Text = customer.address3;
                        txtCustAddress4.Text = customer.address4;
                        txtCityCustomer.Text = customer.city;
                    }
                    else if (TRSettings != null)
                    {
                        if (TRSettings.Tpt_code == cboCustCode.SelectedValue.ToString())
                        {
                            txtCustName.Text = TRSettings.Tpt_name;
                            txtCustAddress1.Text = TRSettings.Address1;
                            txtCustAddress2.Text = TRSettings.Address2;
                            txtCustAddress3.Text = TRSettings.Address3;
                            txtCustAddress4.Text = TRSettings.Address4;
                            txtCityCustomer.Text = "";  // no city code in transport settings  
                        }
                    }
                    CheckQuotation();
                }
                else
                {
                    txtCustName.Text = "";
                    txtCustAddress1.Text = "";
                    txtCustAddress2.Text = "";
                    txtCustAddress3.Text = "";
                    txtCustAddress4.Text = "";
                    txtCityCustomer.Text = "";
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void chkIsBillable_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkTripBillable.Enabled = chkIsBillable.Checked ? true : false;
                if (!chkIsBillable.Checked)
                    chkTripBillable.Checked = false;
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }
        //20140731 -gerry added
        private void cboContainerNo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (haulierJob != null)
                {

                    if (!cboContainerNo.Text.ToString().Equals(string.Empty))
                    {
                        HaulierJobTrip prevTrip = HaulierJobTrip.GetPreviousLegJobTrip(haulierJob.JobID, cboContainerNo.SelectedValue.ToString());
                        //assign start stop to end stop of the previous leg
                        cboStartStop.Text = prevTrip.EndStop.Code.ToString();
                        //20140818 - gerry added
                        //cboContainerCode.SelectedIndex = cboContainerCode.FindString(
                        if (chkMultiLeg.Checked && chkExistingJob.Checked)
                            cboContainerCode.Enabled = false;  //20140818 - gerry added
                        else
                            cboContainerCode.Enabled = true;
                        cboContainerCode.Text = prevTrip.ContainerCode;   //20140818 - gerry added
                    }
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }
        //20140731 -gerry added
        private void CheckContainerForExistingJob()
        {
            try
            {
                cboContainerNo.Text = "";   //initialize to blank
                cboSealNo.Text = "";     //initialize to blank
                cboContainerNo.DataSource = haulierJob.GetAllContainerNoForJob(null, null);
                cboContainerNo.SelectedIndex = haulierJob.GetAllContainerNoForJob(null, null).Count > 0 ? 0 : -1;
                cboSealNo.DataSource = haulierJob.GetAllSealNoForJob(null, null);
                cboSealNo.SelectedIndex = haulierJob.GetAllSealNoForJob(null, null).Count > 0 ? 0 : -1;

                if (cboContainerNo.Items.Count <= 0 && cboJobNo.SelectedValue.ToString() != "")
                {
                    cboContainerNo.Text = string.Empty;
                    warnMsg = TptResourceBLL.ErrNoValidContainerNo;
                }
                else
                    warnMsg = string.Empty;
                

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        private void ShowWarnMessage()
        {
            if (!warnMsg.Equals(string.Empty))
            {
                if (chkIsBillable.Checked || chkExistingJob.Checked)
                    MessageBox.Show(warnMsg, cboCustCode.SelectedValue.ToString());
            }
        }
        private void cboCustCode_TextChanged(object sender, EventArgs e)
        {
            if (chkExistingJob.Checked && cboJobNo.Text != string.Empty) { PopulateContainerCodesFromQuotation(); ShowWarnMessage(); }
        }

        private void cboCustCode_DropDownClosed(object sender, EventArgs e)
        {
            ShowWarnMessage();
        }

        private void cboCustCode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                CheckQuotation();
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void chkIsShiftingJob_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkIsShiftingJob.Checked)
                {
                    chkExistingJob.Checked = false;
                    chkIsBillable.Checked = false;
                    cboCustCode.SelectedIndex = -1;
                    cboStartStop.SelectedIndex = -1;
                    cboEndStop.SelectedIndex = -1;
                    cboIncentiveCode.SelectedIndex = -1;
                    cboContainerCode.SelectedIndex = 0;
                }
                chkExistingJob.Enabled = !chkIsShiftingJob.Checked; // 20141117 - gerry added
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.Message.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), CommonResource.Error); }
        }

        private void cboIncentiveCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboIncentiveCode.Text != string.Empty)
            {
                Incentive inc = Incentive.GetIncentiveWithRateDetails(cboIncentiveCode.SelectedItem.ToString(), parent.chosenDate);
                txtIncentiveDesc.Text = inc.Desc + " @" + inc.Rate + " " + inc.Currency;
            }
            else txtIncentiveDesc.Text = string.Empty;
        }
    }
}
