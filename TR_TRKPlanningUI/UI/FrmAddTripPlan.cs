using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_TRKPlanDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using TR_FormBaseLibrary;

namespace FM.TR_TRKPlanningUI.UI
{
    public partial class FrmAddTripPlan : AbstractForm
    {
        public const string formName = "TPT_TRK_E_ADD_TRIP_PLAN";
        private FrmTruckPlanningEntry frmMainPlan;
        private FrmTruckJobTrips frmTruckJobTrips;
        public TruckJobTrip currentTruckJobTrip;
        public PlanTruckTrip currentPlanTruckTrip;
        public PlanTruckSubTrip planTruckSubTrip;
        public PlanTruckSubTripJob planTruckSubTripJob;
        public TruckJobTripDetail truckJobTripDetail;
        SortableList<TruckJobTrip> jobTrips = new SortableList<TruckJobTrip>();
        public DateTime startDateTime;
        public DateTime endDateTime;
        Driver selectedDriver;
        Vehicle truck;
        bool loaded = false;

        public FrmAddTripPlan()
        {
            InitializeComponent();
        }

        //to be called from frmTruckJobTrips
        public FrmAddTripPlan(FrmTruckJobTrips frmTruckJobTrips)
        {
            InitializeComponent();
            this.frmTruckJobTrips = frmTruckJobTrips;
            this.frmMainPlan = frmTruckJobTrips.parent;
            this.currentTruckJobTrip = frmTruckJobTrips.truckJobTrip;
        }
        //tobe called from main planning form when drag/drop
        public FrmAddTripPlan(FrmTruckPlanningEntry frmMainPlan, PlanTruckTrip currentPlanTruckTrip, TruckJobTrip currentTruckJobTrip, Driver selectedDriver, DateTime startDateTime, DateTime endDateTime, out PlanTruckSubTrip planTruckSubTrip)
        {
            InitializeComponent();
            this.frmMainPlan = frmMainPlan;
            this.currentPlanTruckTrip = currentPlanTruckTrip;
            this.currentTruckJobTrip = currentTruckJobTrip;
            this.selectedDriver = selectedDriver;
            this.startDateTime = startDateTime;
            this.endDateTime = endDateTime;
            this.planTruckSubTrip = planTruckSubTrip = new PlanTruckSubTrip();
        }

        private void Localize()
        {
            this.Text = "Add Plan Truck SubTrip";//TptResourceUI.AddTripPlan;
            lblStart.Text = CommonResource.Start + ":";
            lblEnd.Text = CommonResource.End + ":";
            lblDriver.Text = CommonResource.Driver + ":";
            lblWeight.Text = CommonResource.Weight + ":";
            lblVolume.Text = CommonResource.Volume + ":";
            btnCancel.Text = CommonResource.Cancel;
            btnSave.Text = CommonResource.Save;
        }

        private void FrmAddTripPlan_Load(object sender, EventArgs e)
        {
            Localize();
            bdsTruckJobTripDetail.DataSource = currentTruckJobTrip.truckJobTripDetail;
            //if (currentTruckJobTrip.truckJobTripDetail.Count == 0)
            //{
                //txtWeight.Text = currentTruckJobTrip.GetBalanceWeightForPlan().ToString("#0.00");
                //txtVolume.Text = currentTruckJobTrip.GetBalanceVolForPlan().ToString("#0.00");
            //}
            RefreshData();
        }
        public void AutoCalculateWeightAndVolumeCanLoadToTruck(bool isTruckEmpty)
        {
            try
            {
                if (isTruckEmpty)
                {
                    if (truck == null)
                    {
                        truck = Vehicle.GetVehicle(selectedDriver.defaultVehicleNumber.Trim());
                    }
                    CreatePlanTruckSubTrip();
                    //get exceeded wt and vol
                    decimal exceededWt = 0;
                    decimal exceededVol = 0;
                    //get the max weight and volume to be loaded
                    decimal tempWtTobeLoaded = 0;
                    decimal tempVolTobeLoaded = 0;
                    decimal truckAvailableWt = truck.MaximumLadenWeight;
                    decimal truckAvailableVol = truck.VolCapacity;
                    int qtyToBeLoaded = 0;
                    if (loaded)
                    {
                        foreach (TruckJobTripDetail tjtd in currentTruckJobTrip.truckJobTripDetail)
                        {
                            PlanTruckSubTripJobDetail planTruckSubTripJobDetail = PlanTruckSubTripJobDetail.CreatePlanTruckSubTripJobDetail(0, planTruckSubTripJob, truckJobTripDetail);
                            decimal tempTotalWt = tjtd.balQty * tjtd.unitWeight;
                            if (tempTotalWt <= truckAvailableWt)
                            {
                                qtyToBeLoaded = tjtd.balQty;
                                tempWtTobeLoaded += tempTotalWt;
                                truckAvailableWt -= tempTotalWt;
                                //plansubtripjobdetail
                                //planTruckSubTripJobDetail.qty = tjtd.balQty;
                                //tjtd.balQty = 0;
                            }
                            else //calculate the balance wt can load to truck
                            {
                                qtyToBeLoaded = Convert.ToInt32(truckAvailableWt / tjtd.unitWeight);
                                tempWtTobeLoaded += (qtyToBeLoaded * tjtd.unitWeight);
                                planTruckSubTripJobDetail.qty = Convert.ToInt32(qtyToBeLoaded);
                                //tjtd.balQty -= Convert.ToInt32(qtyToBeLoaded);
                            }
                            exceededWt = currentTruckJobTrip.GetBalanceWeightForPlan() - tempWtTobeLoaded;
                            //}
                            //calculate the balance volume can load to truck
                            if (qtyToBeLoaded < tjtd.quantity)
                            {
                                tempVolTobeLoaded += (qtyToBeLoaded * ((tjtd.length * tjtd.width * tjtd.height) / 1000000));
                                truckAvailableVol -= (qtyToBeLoaded * ((tjtd.length * tjtd.width * tjtd.height) / 1000000));
                            }
                            else
                            {
                                tempVolTobeLoaded += tjtd.volume;
                                truckAvailableVol -= tjtd.volume;
                            }
                            exceededVol = currentTruckJobTrip.GetBalanceVolForPlan() - tempVolTobeLoaded;
                            //}
                            //else { tempVolTobeLoaded = tjtd.volume; }
                            //planSubTripJob
                            planTruckSubTripJob.weight = tempWtTobeLoaded;
                            planTruckSubTripJob.volume = tempVolTobeLoaded;
                            planTruckSubTripJob.planTruckSubTripJobDetails.Add(planTruckSubTripJobDetail);
                            txtWeight.Text = tempWtTobeLoaded.ToString("#0.00");
                            txtVolume.Text = tempVolTobeLoaded.ToString("#0.00");
                            if (tempTotalWt <= truckAvailableWt && tempVolTobeLoaded <= truckAvailableVol)
                            {
                                planTruckSubTripJobDetail.qty = tjtd.balQty; 
                                tjtd.balQty = 0;
                            }
                        }
                    }
                }
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
        }

        public void InializeDisplayFromFromTruckJobTrips()
        {
            if (frmTruckJobTrips != null)
            {
                dtpStartDate.Value = frmTruckJobTrips.chosenDate;
                dtpEndDate.Value = frmTruckJobTrips.chosenDate;
                dtpStartTime.Value = DateTime.Now;
                dtpEndTime.Value = dtpStartTime.Value.AddHours(1);
                cboDriverCode.DataSource = frmTruckJobTrips.drivers;
                cboDriverCode.DisplayMember = "DescriptionForPlanningPurpose";
                //txtWeight.Text = "0.00";
                //txtVolume.Text = "0.00";
            }
        }
        public void InializeDisplayFromFromMainPlan()
        {
            if (currentPlanTruckTrip != null)
            {
                dtpStartDate.Value = currentPlanTruckTrip.ScheduleDate;
                dtpEndDate.Value = currentPlanTruckTrip.ScheduleDate;
                dtpStartTime.Value = startDateTime;
                dtpEndTime.Value = endDateTime;
                cboDriverCode.DataSource = frmMainPlan.drivers;
                cboDriverCode.DisplayMember = "DescriptionForPlanningPurpose";
                cboDriverCode.Enabled = false;
                //txtWeight.Text = "0.00";
                //txtVolume.Text = "0.00";
                AutoCalculateWeightAndVolumeCanLoadToTruck(true);
                bdsTruckJobTripDetail.DataSource = currentTruckJobTrip.truckJobTripDetail;
                cboDriverCode.SelectedIndex = cboDriverCode.FindStringExact(currentPlanTruckTrip.Driver.DescriptionForPlanningPurpose.ToString());
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshData();
            }
            catch (FMException ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }
        private void RefreshData()
        {
            try
            {
                planTruckSubTrip = null;
                planTruckSubTripJob = null;
                currentTruckJobTrip = TruckJobTrip.GetTruckJobTrip(currentTruckJobTrip.JobID, currentTruckJobTrip.Sequence);
                bdsTruckJobTripDetail.DataSource = currentTruckJobTrip.truckJobTripDetail;
                txtWeight.Text = currentTruckJobTrip.GetBalanceWeightForPlan().ToString("#0.00");
                txtVolume.Text = currentTruckJobTrip.GetBalanceVolForPlan().ToString("#0.00");
                if (frmTruckJobTrips != null)
                    InializeDisplayFromFromTruckJobTrips();
                else
                    InializeDisplayFromFromMainPlan();
            }
            catch (FMException ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region 20160223 - gerry Replaced
                /*
                 * 2013-08-20 Zhou Kai adds comments:
                 * This function is to create:
                 * (a) a truckJobTripPlan
                 * (b) use CreatePlanTruckSubTrip() to create:
                 *       1. a planTruckSubTrip 
                 *       2. a planTrukcSubTripJob, which attached to the created planTruckSubTrip
                 * (c) CreatePlanTruckSubTrip() will also update the truckJobTripPlan,
                 *     planTruckSubTrip, planTruckSubTripJob to database
                 * (d) // 2013-09-13 Zhou Kai adds
                 *     As a truckJobTripPlan is added to planning, we
                 *     also need to update the actual weight/volume to
                 *     truckJobTrip and TRK_JOB_DETAIL_TRIP
                 */
                /*
                #region "Create a new truckJobTripPlan with the values that the user input from UI"
               
                Driver selectedDriver = (Driver)cboDriverCode.SelectedItem;
                TruckJobTripPlan truckJobTripPlan = new TruckJobTripPlan();
                truckJobTripPlan.scheduleDate = parent.chosenDate;
                truckJobTripPlan.driver = selectedDriver.Code;
                truckJobTripPlan.truckNo = selectedDriver.defaultVehicleNumber;
                truckJobTripPlan.volume = Decimal.Parse(txtVolume.Text);
                truckJobTripPlan.weight = Decimal.Parse(txtWeight.Text);
                DateTime start = dtpStartDate.Value.Date + dtpStartTime.Value.TimeOfDay;
                DateTime end = dtpEndDate.Value.Date + dtpEndTime.Value.TimeOfDay;
                truckJobTripPlan.start = start;
                truckJobTripPlan.end = end;
                truckJobTripPlan.jobID = parent.truckJobTrip.JobID;
                truckJobTripPlan.sequence = parent.truckJobTrip.Sequence;
             
                #endregion

                #region "Check if there's an existing planTruckTrip with the chosen driver for this truckJobTripPlan"
                bool add = false;
                for (int i = 0; i < parent.planTruckTrips.Count; i++)
                {
                    bool foundDriver = false;
                    if (parent.planTruckTrips[i].Driver.Code == selectedDriver.Code)
                    {
                        foundDriver = true;
                        // 2014-04-16 Zhou Kai adds codes to synchronize planTruckTrips 
                        // between FrmTruckJobTrips and FrmTruckPlanningEntry
                        parent.planTruckTrips = parent.parent.planTruckTrips;
                        // 2014-04-16 Zhou Kai ends
                        truckJobTripPlan.planTripNo = parent.planTruckTrips[i].PlanTripNo;
                        string message = "";
                        bool overload = false;
                        if (parent.planTruckTrips[i].CanAddPlanTruckSubTrip(truckJobTripPlan, out message, out overload))
                        {
                            try
                            {
                                PlanTruckSubTrip.CreatePlanTruckSubTrip(parent.planTruckTrips[i], parent.truckJobTrip,
                                                                        truckJobTripPlan, formName, parent.user.UserID);
                                // 2013-11-18 Zhou Kai moves this function into the one above
                                // TruckJobTrip.UpdateAcutalWeightVolumeforTruckJobTrip(truckJobTripPlan.jobID, truckJobTripPlan.sequence);
                                // 2013-11-18 Zhou Kai ends
                                add = true;
                            }
                            catch (FMException ex)
                            {
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        if (message != "")
                        {
                            MessageBox.Show(message, CommonResource.Alert);
                        }
                    }
                    if (foundDriver)
                    {
                        break;
                    }
                }
                #endregion

                #region "If there's no existing planTruckTrip with the same driver as the new truckJobTripPlan (logic unfinished)"
                if (add)
                {
                    for (int i = 0; i < parent.dgvTruckJobTripPlans.Rows.Count; i++)
                    {
                        //parent.dgvTruckJobTripPlans.Rows[i].Cells["DriverName"].Value = Driver.GetDriverName(parent.dgvTruckJobTripPlans.Rows[i].Cells["DriverCode"].Value.ToString());
                    }
                    if (parent != null)
                    {
                        //parent.RefreshJobTripGridDataAndColor(); //20140319 -gerry remove to refresh frmTruckJobTrips when form is activated
                        //parent.RefreshChildGridData();
                    }
                    this.Dispose();
                }
                #endregion
                */
                #endregion
                jobTrips = new SortableList<TruckJobTrip>();
                jobTrips.Add(currentTruckJobTrip);
                string outMsg = string.Empty;
                bool isOverLoad = false;
                decimal assignedVolume = Decimal.Parse(txtVolume.Text);
                decimal assignWeight = Decimal.Parse(txtWeight.Text);
                startDateTime = dtpStartDate.Value.Date + dtpStartTime.Value.TimeOfDay;
                endDateTime = dtpEndDate.Value.Date + dtpEndTime.Value.TimeOfDay;
                DevExpress.XtraScheduler.TimeInterval newTimeInterval = new DevExpress.XtraScheduler.TimeInterval(startDateTime, endDateTime);
                frmMainPlan.dragType = 1; //newly created plan truck sub trip
                currentPlanTruckTrip = PlanTruckTrip.FindPlanTruckTripByDriver(frmMainPlan.planTruckTrips, selectedDriver);
                if (frmMainPlan.IsIntervalAllowed(selectedDriver, newTimeInterval))
                {
                    int index = 0;
                    if (currentPlanTruckTrip == null)
                        currentPlanTruckTrip = new PlanTruckTrip(frmMainPlan.deptcode, "", startDateTime.Date, Vehicle.GetVehicle(selectedDriver.defaultVehicleNumber), selectedDriver, new byte[8]);

                    currentPlanTruckTrip.CheckTruckJobTripsBeforeAssignToVehicle(jobTrips, startDateTime, endDateTime, assignWeight, assignedVolume, out outMsg, out isOverLoad);
                    if (isOverLoad)
                    {
                        if (outMsg.Contains("Volume") || outMsg.Contains("volume"))
                            throw new FMException(TptResourceBLL.ErrExceededVolume);
                        if (outMsg.Contains("Weight") || outMsg.Contains("weight"))
                            throw new FMException(TptResourceBLL.ErrExceededWeight);
                    }
                    if (frmTruckJobTrips != null)
                    {
                        planTruckSubTrip = currentPlanTruckTrip.CreatePlanTruckSubTripForOTO(currentTruckJobTrip, assignWeight, assignedVolume, newTimeInterval.Start, newTimeInterval.End, frmTruckJobTrips.Name, frmMainPlan.user.UserID, frmMainPlan.deptcode, planTruckSubTripJob);
                        frmTruckJobTrips.RefreshJobTripGridDataAndColor();
                    }
                    else
                    {  //TODO with split
                        //currentPlanTruckTrip.AddPlanTruckSubTrip(planTruckSubTrip, formName, frmMainPlan.user.UserID.ToString());
                        planTruckSubTrip = currentPlanTruckTrip.CreatePlanTruckSubTripForOTO(currentTruckJobTrip, assignWeight, assignedVolume, newTimeInterval.Start, newTimeInterval.End, "ApptCreated - Split", frmMainPlan.user.UserID, frmMainPlan.deptcode, planTruckSubTripJob);
                    }
                    currentPlanTruckTrip.planTruckSubTrips.SortByStartTime();
                    AuditLog.EventLog("ApptCreated - Manual", planTruckSubTrip, planTruckSubTrip.Start, planTruckSubTrip.End, frmMainPlan.user.UserID, true);
                }
                //currentTruckJobTrip = TruckJobTrip.GetTruckJobTrip(currentTruckJobTrip.JobID, currentTruckJobTrip.Sequence);
                //txtWeight.Text = currentTruckJobTrip.GetBalanceWeightForPlan().ToString("#0.00");
                //txtVolume.Text = currentTruckJobTrip.GetBalanceVolForPlan().ToString("#0.00");
                //bdsTruckJobTripDetail.DataSource = currentTruckJobTrip.truckJobTripDetail;
               
                //20170203 -gerry added to send send to vehicle
                frmMainPlan.SendJobToVehicle(frmMainPlan.GetCT_AccessTokenCodeForCloudTrack(), false);//send job to cloud track

                //refresh parents
                frmMainPlan.RefreshData();
                frmMainPlan.currentPlanTruckTrip = currentPlanTruckTrip;
                //recalculate balance weight and volume
                //reset display weight and volume
                RefreshData();
                if ((Convert.ToDecimal(txtWeight.Text.ToString().Trim())) <= 0 && (Convert.ToDecimal(txtVolume.Text.ToString().Trim())) <= 0)
                {
                    this.Close();
                }
            }
            catch (FMException ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), CommonResource.Error);
                RefreshData();
            }
        }
  
        private void ValidatebeforeSavePlanTrukcSubTrip()
        {
            try
            {
                frmMainPlan.dragType = 1; // newly created
                startDateTime = dtpStartDate.Value.Date + dtpStartTime.Value.TimeOfDay;
                endDateTime = dtpEndDate.Value.Date + dtpEndTime.Value.TimeOfDay;
                DevExpress.XtraScheduler.TimeInterval newTimeInterval = new DevExpress.XtraScheduler.TimeInterval(startDateTime, endDateTime);
                currentPlanTruckTrip = PlanTruckTrip.FindPlanTruckTripByDriver(frmMainPlan.planTruckTrips, selectedDriver);
                if (frmMainPlan.IsIntervalAllowed(selectedDriver, newTimeInterval))
                {
                    int index = 0;
                    if (currentPlanTruckTrip == null)
                        currentPlanTruckTrip = new PlanTruckTrip(frmMainPlan.deptcode, "", startDateTime.Date, Vehicle.GetVehicle(selectedDriver.defaultVehicleNumber), selectedDriver, new byte[8]);
                }
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }            
        }
        private void CreatePlanTruckSubTrip()
        {
            if (planTruckSubTrip == null)
            {
                planTruckSubTrip = currentPlanTruckTrip.CreatePlanTruckSubTripForSplitPlanTruckSubTripJob(currentTruckJobTrip.StartStop, currentTruckJobTrip.EndStop,startDateTime, 
                                                                                        endDateTime, this.Name.ToString(), frmMainPlan.user.UserID, frmMainPlan.deptcode);
            }
            if (planTruckSubTripJob == null)
            {  //create plan truckSubTripJob
                planTruckSubTripJob = PlanTruckSubTripJob.CreatePlanTruckSubTripJob(planTruckSubTrip.PlanTripNo, planTruckSubTrip.SeqNo, currentTruckJobTrip,startDateTime, endDateTime);
            }
            if (planTruckSubTrip.planTruckSubTripJobs.Count == 0)
                planTruckSubTrip.planTruckSubTripJobs.Add(planTruckSubTripJob);
            
        }
        private void dgvTruckJobTripDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvTruckJobTripDetails.ColumnCount - 1 && e.RowIndex >= 0)
                {
                    ValidatebeforeSavePlanTrukcSubTrip();
                    CreatePlanTruckSubTrip();
                    //show quantity form
                    loaded = false;
                    FrmActualQuantity frmActualQuantity = new FrmActualQuantity(this);
                    frmActualQuantity.ShowDialog();
                    //recalculate the weight and volume
                    planTruckSubTripJob.weight = 0;
                    planTruckSubTripJob.volume = 0;
                    foreach (PlanTruckSubTripJobDetail planTruckSubTripJobDeatil in planTruckSubTripJob.planTruckSubTripJobDetails)
                    {
                        planTruckSubTripJob.weight += planTruckSubTripJobDeatil.qty * planTruckSubTripJobDeatil.unitWeight;
                        planTruckSubTripJob.volume += planTruckSubTripJobDeatil.qty * ((planTruckSubTripJobDeatil.length * planTruckSubTripJobDeatil.width * planTruckSubTripJobDeatil.height) / 1000000);
                    }
                    txtVolume.Text = planTruckSubTripJob.volume.ToString("#0.00");
                    txtWeight.Text = planTruckSubTripJob.weight.ToString("#0.00");
                    bdsTruckJobTripDetail.ResetBindings(false);
                    loaded = true;
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }

        private void cboDriverCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDriver = (Driver)cboDriverCode.SelectedItem;
            if (selectedDriver != null)
            {
                Vehicle truck = Vehicle.GetVehicle(selectedDriver.defaultVehicleNumber.Trim());
                txtTruckCapacity.Text = truck.NumberAndCapacity;
            }
        }

        private void bdsTruckJobTripDetail_CurrentChanged(object sender, EventArgs e)
        {
            truckJobTripDetail = (TruckJobTripDetail)bdsTruckJobTripDetail.Current;
        }

    }
}
