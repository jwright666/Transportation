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
    /*
     * 2013-06-28 zhou kai adds class
     * to perform the "Add Routed Plan Truck Sub Trips" function
     * 2013-07-09 ends
     */

    public partial class FrmAddRoute : Form
    {
        #region "Member variables"
        //from parent windown form
        public const string frmAddRoutedPlanTruckSubTrips = "TPT_TRK_E_ADD_PLANNED_ROUTE";
        public FrmTruckPlanningEntry parent;
        public DateTime chosenDate;
        public string deptCode;
        public User user; //current FM System user
        public SortableList<Driver> drivers = new SortableList<Driver>();
        public SortableList<Vehicle> vehicles = new SortableList<Vehicle>();

        //all existing plan (sub) trips, plan trip jobs on the chosen date
        public SortableList<PlanTruckTrip> planTruckTripsOnChosenDate = new SortableList<PlanTruckTrip>();
        public PlanTruckTrip planTruckTrip = new PlanTruckTrip();

        //plan route
        private List<Stop> startStops = new List<Stop>();
        private List<Stop> endStops = new List<Stop>();
        private List<DateTime> startTimes = new List<DateTime>();
        private List<DateTime> endTimes = new List<DateTime>();
        private Driver planRoutedDriver; //driver for this planned route
        private Vehicle planRoutedVehicle; //vehicle for this planned route
        private List<PlanTruckSubTrip> unSortedRoutedPlanSubTrips; //represents for the whole plan route
        private List<PlanTruckSubTrip> sortedRoutedPlanSubTrips;

        //plan routed sub trip
        private OperatorDTO subTripStartStop;
        private OperatorDTO subTripEndStop;

        //existing system resources
        private List<OperatorDTO> startStopList = new List<OperatorDTO>();
        private List<OperatorDTO> endStopList = new List<OperatorDTO>();

        //datasource binding
        private BindingSource bindSourceDrivers;
        private BindingSource bindSourceVehicles;

        private BindingSource bindSourceStartStops;
        private BindingSource bindSourceEndStops;

        //UI
        private DataGridView lastFocusedDatagridview; //to represent for which datagridview is clicked most recently
        private Int32 nSeqNo = 0; //serve as the uid for the sorted / unsorted plan sub trips
        private bool bRouteSavedSuccessfully; //it will be set to true if a route is saved successfully after clicking button "Save Route" 
        private bool bIsAddMode; //If bIsAddMode is true, then when click button "Add Plan SubTrip",
                                 //a new plan subtrip will be add to bindingsource; 
                                 //if bIsAddMode is false, then when click button "Add Plan SubTrip",
                                 //all the fields of middle panel will be earased for input, (no new plan subtrip created)
        #endregion

        #region "Constructors"

        public FrmAddRoute()
        {
            InitializeComponent();
        }

        public FrmAddRoute(FrmTruckPlanningEntry parent, User user)
        {
            InitializeComponent();

            this.parent = parent;
            this.deptCode = parent.deptcode;
            this.chosenDate = parent.chosenDate;
            this.drivers = parent.drivers;
            this.vehicles = null; //we have to get all the avaliable vehicles from parent window

            this.user = user;
            this.planTruckTripsOnChosenDate = parent.planTruckTrips;

            this.planRoutedDriver = null;
            this.planRoutedVehicle = null;
            this.unSortedRoutedPlanSubTrips = null;

            bindSourceDrivers = new BindingSource();
            bindSourceVehicles = new BindingSource();

            bindSourceStartStops = new BindingSource();
            bindSourceEndStops = new BindingSource();

            unSortedRoutedPlanSubTrips = new List<PlanTruckSubTrip>();
            sortedRoutedPlanSubTrips = new List<PlanTruckSubTrip>();

            bindSourceUnSortedRoutedPlanSubTrips.DataSource = unSortedRoutedPlanSubTrips;
            bindSourceSortedRoutedPlanSubTrips.DataSource = sortedRoutedPlanSubTrips;
            
            bRouteSavedSuccessfully = false;
            bIsAddMode = true;
        }

        #endregion

        #region "Methods"

        private void Localize()
        {
            //do the localizations here
        }

        private void OnOffBottomPanel(bool enable)
        {
            if (enable)
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;

                btnSaveRoute.Enabled = true;
                btnCancelRoute.Enabled = true;
            }
            else
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;

                btnSaveRoute.Enabled = false;
                btnCancelRoute.Enabled = false;
            }
            return;
        }

        // 2013-09-02 Zhou Kai adds this logic
        private void OnOffButtons(bool bIsAddMode)
        {
            if (bIsAddMode)
            {
                btnAddSubTrip.Enabled = true;
                btnSaveEditSubTrip.Enabled = false;
                btnCancelEdit.Enabled = false;
                btnDeleteSubTrip.Enabled = false;
            }
            else
            {
                btnAddSubTrip.Enabled = false;
                btnSaveEditSubTrip.Enabled = true;
                btnCancelEdit.Enabled = true;
                btnDeleteSubTrip.Enabled = true;
            }
        }
        // ends

        private void OnOffMiddlePanel(bool enable, bool keepContent)
        {
            #region Gerry removed unnecessary codes
            /*
            if (enable)
            {
                iplcboStartCode.Enabled = true;
                iplcboEndCode.Enabled = true;
                dtpStartTime.Enabled = true;
                dtpEndTime.Enabled = true;

                txtStartAddress.Enabled = true;
                txtStartAddress2.Enabled = true;
                txtStartAddress3.Enabled = true;
                txtStartAddress4.Enabled = true;
                txtEndAddress.Enabled = true;
                txtEndAddress2.Enabled = true;
                txtEndAddress3.Enabled = true;
                txtEndAddress4.Enabled = true;

                txtStartCity.Enabled = true;
                txtEndCity.Enabled = true;
            }
            else
            {
                iplcboStartCode.Enabled = false;
                iplcboEndCode.Enabled = false;
                dtpStartTime.Enabled = false;
                dtpEndTime.Enabled = false;

                txtStartAddress.Enabled = false;
                txtStartAddress2.Enabled = false;
                txtStartAddress3.Enabled = false;
                txtStartAddress4.Enabled = false;
                txtEndAddress.Enabled = false;
                txtEndAddress2.Enabled = false;
                txtEndAddress3.Enabled = false;
                txtEndAddress4.Enabled = false;

                txtStartCity.Enabled = false;
                txtEndCity.Enabled = false;
            }
            */
            #endregion
            gbxStart.Enabled = enable;
            gbxEnd.Enabled = enable;

            if (!keepContent)
            {
                iplcboStartCode.Text = String.Empty;
                txtStartName.Text = String.Empty;
                txtStartAddress.Text = String.Empty;
                txtStartAddress2.Text = String.Empty;
                txtStartAddress3.Text = String.Empty;
                txtStartAddress4.Text = String.Empty;
                iplcboEndCode.Text = String.Empty;
                txtEndName.Text = String.Empty;
                txtEndAddress.Text = String.Empty;
                txtEndAddress2.Text = String.Empty;
                txtEndAddress3.Text = String.Empty;
                txtEndAddress4.Text = String.Empty;

                txtStartCity.Text = String.Empty;
                txtEndCity.Text = String.Empty;
            }
        }

        private bool IsDriverVehicleSelected()
        {
            if (cboDriver.SelectedIndex != -1 &&
                cboVehicle.SelectedIndex != -1 &&
                planRoutedDriver != null &&
                planRoutedVehicle != null &&
                (!planRoutedVehicle.Number.Equals(String.Empty)))
            {
                return true;
            }

            return false;
        }

        private void CustomizeDateTimePicker()
        {
            //make the date time picker control
            //to display time only(without date)
            dtpStartTime.Format = DateTimePickerFormat.Time;
            dtpStartTime.ShowUpDown = true;
            dtpEndTime.Format = DateTimePickerFormat.Time;
            dtpEndTime.ShowUpDown = true;

            return;
        }

        private void FillinDrivers()
        {
            bindSourceDrivers.DataSource = drivers;
            cboDriver.DataSource = bindSourceDrivers;
            cboDriver.DisplayMember = @"DescriptionForPlanningPurpose";
        }

        private void FillVehicle()
        {
            // use the driver's default vehicle
            Driver dr = planRoutedDriver;
            planRoutedVehicle = Vehicle.GetVehicle(dr.defaultVehicleNumber);
            bindSourceVehicles = new BindingSource();
            bindSourceVehicles.DataSource = planRoutedVehicle;
            cboVehicle.DataSource = bindSourceVehicles;
            cboVehicle.DisplayMember = @"Number";
        }

        private void FillStopsComboBox()
        {
            try
            {
                startStopList = OperatorDTO.GetAllOperators();
                endStopList = OperatorDTO.GetAllOperators();
                startStopList.Insert(0, new OperatorDTO());
                endStopList.Insert(0, new OperatorDTO());
                bindSourceStartStops = new BindingSource();
                bindSourceEndStops = new BindingSource();
                bindSourceStartStops.DataSource = startStopList;
                bindSourceEndStops.DataSource = endStopList;
                iplcboStartCode.DataSource = bindSourceStartStops;
                iplcboStartCode.DisplayMember = @"Code";
                iplcboEndCode.DataSource = bindSourceEndStops;
                iplcboEndCode.DisplayMember = @"Code";
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }
        }

        private bool IsSelectedRowContinuous(DataGridViewSelectedRowCollection dgvsr)
        {
            if (dgvsr.Count == 0)
            {
                MessageBox.Show(TptResourceUI.SelectPTSTBeforeMove);
                return false;
            }
         
            if (dgvsr.Count == 1)
            {
                return true;
            }

            Int32[] indexs = new Int32[dgvsr.Count];
            Int32 i = 0;
            foreach (DataGridViewRow dgr in dgvsr)
            {
                indexs[i] = dgr.Index;
                i++;
            }

            //sort the indexs first
            i = 0;
            Int32 tmp = 0;
            for (i = 0; i < indexs.Length; i++)
            {
                for (int j = i; j < indexs.Length; j++)
                {
                    if (indexs[i] > indexs[j])
                    {
                        tmp = indexs[i];
                        indexs[i] = indexs[j];
                        indexs[j] = tmp;
                    }
                }
            }

            for (i = 0; i < dgvsr.Count - 1; i++)
            {
                if (indexs[i] - indexs[i + 1] != -1)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsStartAndEndTimeValid()
        {
            string startDate, endDate;
            DateTime dtStart, dtEnd;
            startDate = "1900-01-01 " + dtpStartTime.Value.ToShortTimeString();
            endDate = "1900-01-01 " + dtpEndTime.Value.ToShortTimeString();
            dtStart = Convert.ToDateTime(startDate);
            dtEnd = Convert.ToDateTime(endDate);

            if (dtStart >= dtEnd)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsStopsComplete()
        {
            if (iplcboStartCode.Text.Equals(String.Empty) ||
                iplcboEndCode.Text.Equals(String.Empty))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsStartEndStopDifferent()
        {
            if (iplcboStartCode.Text.Equals(iplcboEndCode.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SavePlanSubTripToUnsortedList()
        {
            //add plan routed sub trip
            string strStartTime = dtpStartTime.Value.ToShortTimeString();
            strStartTime = chosenDate.ToShortDateString() + " " + strStartTime;
            string strEndTime = dtpEndTime.Value.ToShortTimeString();
            strEndTime = chosenDate.ToShortDateString() + " " + strEndTime;

            DateTime startTime = Convert.ToDateTime(strStartTime);
            DateTime endTime = Convert.ToDateTime(strEndTime);
            Stop startStop = Stop.GetStop(((OperatorDTO)iplcboStartCode.SelectedItem).Code);
            Stop endStop = Stop.GetStop(((OperatorDTO)iplcboEndCode.SelectedItem).Code);

            PlanTruckSubTrip ptst = new PlanTruckSubTrip(nSeqNo, startTime, endTime, startStop, endStop, false,
                                                         String.Empty, new SortableList<PlanTruckSubTripJob>(),
                                                         planTruckTrip, JobTripStatus.Assigned,
                                                         new byte[8]);
            nSeqNo++;
            ptst.VehicleNumber = planRoutedDriver.defaultVehicleNumber;
            ptst.DriverNumber = planRoutedDriver.Code;
            unSortedRoutedPlanSubTrips.Add(ptst);
            bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);

            return;
        }


        private void GetPlanRouteSubTripArgs(List<PlanTruckSubTrip> sortedPlanTruckSubTrips,
                                             out List<Stop> startStops, out List<Stop> endStops,
                                             out List<DateTime> startTimes, out List<DateTime> endTimes)
        {
            startStops = new List<Stop>();
            endStops = new List<Stop>();
            startTimes = new List<DateTime>();
            endTimes = new List<DateTime>();

            //sort the plan truck sub trips for the customer by start time first
            PlanTruckTrip.SortPlanTruckSubTripsByStartDateTime(sortedPlanTruckSubTrips);

            foreach (PlanTruckSubTrip ptst in sortedPlanTruckSubTrips)
            {
                startStops.Add(ptst.StartStop);
                endStops.Add(ptst.EndStop);
                startTimes.Add(ptst.Start);
                endTimes.Add(ptst.End);
            }

            return;
        }

        private void ChangeDriver(Driver newDriver, Vehicle newVehicle)
        {
            foreach (PlanTruckSubTrip ptst in sortedRoutedPlanSubTrips)
            {
                ptst.DriverNumber = newDriver.Code;
                ptst.VehicleNumber = newVehicle.Number;

            }
            foreach (PlanTruckSubTrip ptst in unSortedRoutedPlanSubTrips)
            {
                ptst.DriverNumber = newDriver.Code;
                ptst.VehicleNumber = newVehicle.Number;
            }
        }

        private void EnableMiddlePanelForEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell dgvc = dgv.SelectedCells[0];
            Int32 selectedRowIndex = dgvc.RowIndex; ;
            dgv.Rows[selectedRowIndex].Selected = true;
            PlanTruckSubTrip ptst = new PlanTruckSubTrip();
            if (dgv.Name.Equals("dgvSortedPlanRoutedSubTrips"))
            {
                ptst = sortedRoutedPlanSubTrips[selectedRowIndex];
            }
            else if (dgv.Name.Equals("dgvUnSortedPlanRoutedSubTrips"))
            {
                ptst = unSortedRoutedPlanSubTrips[selectedRowIndex];
            }
            
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;
            
            OnOffMiddlePanel(true, true);
            bIsAddMode = false;
            OnOffButtons(bIsAddMode);

            return;
        }

        #endregion

        #region "Event handlers"

        private void FrmAddRoutedPlanTruckSubTrips_Load(object sender, EventArgs e)
        {
            //when loads the windows form,
            //we don't fill the combobox here, to
            //reduce the problems caused by it
            lblDate.Text = chosenDate.Date.ToShortDateString();
            Localize();
            FillinDrivers();
            FillStopsComboBox();
            OnOffMiddlePanel(false, false);
            OnOffBottomPanel(false);
            // 2013-09-02 Zhou Kai adds:
            OnOffButtons(bIsAddMode);
            // 2013-09-02 ends
            CustomizeDateTimePicker();
            if (parent.currentdriver.DescriptionForPlanningPurpose != string.Empty)
                cboDriver.SelectedIndex = cboDriver.FindStringExact(parent.currentdriver.DescriptionForPlanningPurpose);
            else
                cboDriver.SelectedIndex = 0;
            dtpStartTime.Value = DateTime.Now;
        }

        void selec(object sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void cboDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            //after selects a dirver code
            planRoutedDriver = new Driver();
            planRoutedDriver = (Driver)cboDriver.SelectedItem;

            FillVehicle();
            OnOffMiddlePanel(IsDriverVehicleSelected(), false);
            ChangeDriver(planRoutedDriver, (Vehicle)cboVehicle.SelectedItem);

            planTruckTrip = PlanTruckTrip.FindPlanTruckTripByDriver(parent.planTruckTrips, planRoutedDriver);

            //If the user change the dirver
            //at halfway of creating the routed plan sub trips,
            //we need to modify the driver of all the newly
            //created routed plan sub trips.
            //This logic is not implemented yet.

            return;
        }

        private void cboVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //after selecting a vehicle.
            //Will need to complete the logic.

        }

        private void cboStartStop_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                subTripStartStop = new OperatorDTO();
                subTripStartStop = OperatorDTO.GetOperatorDTO(iplcboStartCode.Text.Trim());
                if (!iplcboStartCode.Text.Trim().ToUpper().Equals(String.Empty))
                {
                    txtStartName.Text = subTripStartStop.Name;
                    txtStartAddress.Text = subTripStartStop.Add1;
                    txtStartAddress2.Text = subTripStartStop.Add2;
                    txtStartAddress3.Text = subTripStartStop.Add3;
                    txtStartAddress4.Text = subTripStartStop.Add4;
                    txtStartCity.Text = subTripStartStop.City;
                }
                else
                {
                    return;
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }

            return;
        }

        private void cboEndStop_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                subTripEndStop = new OperatorDTO();
                subTripEndStop = OperatorDTO.GetOperatorDTO(iplcboEndCode.Text.Trim());
                if (!iplcboEndCode.Text.Trim().ToUpper().Equals(String.Empty))
                {
                    txtEndName.Text = subTripEndStop.Name;
                    txtEndAddress.Text = subTripEndStop.Add1;
                    txtEndAddress2.Text = subTripEndStop.Add2;
                    txtEndAddress3.Text = subTripEndStop.Add3;
                    txtEndAddress4.Text = subTripEndStop.Add4;
                    txtEndCity.Text = subTripEndStop.City;
                }
                else
                {
                    return;
                }
            }
            catch (FMException fmEx) { MessageBox.Show(fmEx.ToString(), CommonResource.Error); }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), CommonResource.Error); }

            return;
        }

        private void btnAddSubTrip_Click(object sender, EventArgs e)
        {
            if (!bIsAddMode)
            {
                OnOffMiddlePanel(true, false);
                bIsAddMode = true;
                return;
            }

            if (!IsStopsComplete())
            {
                MessageBox.Show(TptResourceUI.ToCompleteStartEndStop, CommonResource.Information, MessageBoxButtons.OK);
                return;
            }

            if (!IsStartAndEndTimeValid())
            {
                MessageBox.Show(TptResourceUI.MakeSureStartEarlyThanEndTime, CommonResource.Information, MessageBoxButtons.OK);
                return;
            }

            if (!IsStartEndStopDifferent())
            {
                MessageBox.Show(TptResourceUI.MakeSureStartEndStopDiff, CommonResource.Information, MessageBoxButtons.OK);
                return;
            }

            SavePlanSubTripToUnsortedList();

            //clear all the fields in middle panel
            OnOffMiddlePanel(true, false);
            OnOffButtons(true);
            OnOffBottomPanel(true);
        }

        private void btnCancelRoute_Click(object sender, EventArgs e)
        {
            if (unSortedRoutedPlanSubTrips.Count > 0 ||
                sortedRoutedPlanSubTrips.Count > 0)
            {
                DialogResult dr = MessageBox.Show(TptResourceUI.PromptToDelAllPTST,
                                              CommonResource.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr != DialogResult.Yes)
                {
                    return;
                }

                unSortedRoutedPlanSubTrips.Clear();
                sortedRoutedPlanSubTrips.Clear();
                bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
                bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            }

            return;
        }

        private void FrmAddRoutedPlanTruckSubTrips_FormClosing(object sender, FormClosingEventArgs e)
        {
            //now we don't store the temporary info,
            //so when the user close the windows form
            //before click "Save Route", the data will lose
            DialogResult dr = new DialogResult();

            if (!bRouteSavedSuccessfully)
            {
                dr = MessageBox.Show(TptResourceUI.PromptPTSTNotSaved,
                                     CommonResource.Warning,
                                     MessageBoxButtons.OKCancel,
                                     MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {
                    //go on closing the window form
                }
                else
                {
                    //stay
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void btnAddToSorted_Click(object sender, EventArgs e)
        {
            //move selected one or more plan sub trips to the list to the right
            if (dgvUnSortedPlanRoutedSubTrips.SelectedRows.Count == 0)
            {
                if (dgvUnSortedPlanRoutedSubTrips.SelectedCells.Count == 0)
                {
                    MessageBox.Show(TptResourceUI.SelectUnSortedPTSTBeforeMove);
                }
                else if (dgvUnSortedPlanRoutedSubTrips.SelectedCells.Count == 1)
                {
                    //get the selected un sorted plan sub trip
                    DataGridViewCell dgvc = dgvUnSortedPlanRoutedSubTrips.SelectedCells[0];
                    Int32 selectedRowIndex = dgvc.RowIndex;
                    PlanTruckSubTrip ptst = unSortedRoutedPlanSubTrips[selectedRowIndex];
                    //add it to the sorted list
                    sortedRoutedPlanSubTrips.Add(ptst);
                    //remove it from the unsorted list
                    PlanTruckSubTrip tmp = new PlanTruckSubTrip();
                    tmp = unSortedRoutedPlanSubTrips.Find(planTruckSubTrip =>
                                                          planTruckSubTrip.SeqNo.Equals(ptst.SeqNo));
                    unSortedRoutedPlanSubTrips.Remove(tmp);
                }
                else
                {
                    //do nothing
                }
            }
            else if (dgvUnSortedPlanRoutedSubTrips.SelectedRows.Count >= 1)
            {
                List<Int32> selectedIndexes = new List<int>();
                foreach (DataGridViewRow rowSelected in dgvUnSortedPlanRoutedSubTrips.SelectedRows)
                {
                    selectedIndexes.Add(rowSelected.Index);
                }

                selectedIndexes.Sort();
                Int32 j = 0; 
                PlanTruckSubTrip ptst = new PlanTruckSubTrip();
                
                foreach (Int32 i in selectedIndexes)
                {
                    ptst = unSortedRoutedPlanSubTrips[i - j];
                    sortedRoutedPlanSubTrips.Add(ptst);
                    unSortedRoutedPlanSubTrips.RemoveAt(i - j);
                    j++;
                }    
            }

            bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
            bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            dgvUnSortedPlanRoutedSubTrips.DataSource = bindSourceUnSortedRoutedPlanSubTrips;
            dgvSortedPlanRoutedSubTrips.DataSource = bindSourceSortedRoutedPlanSubTrips; 

            OnOffMiddlePanel(true, false);
            btnSaveEditSubTrip.Enabled = false;
            btnDeleteSubTrip.Enabled = false;
        }

        private void btnAddToUnsorted_Click(object sender, EventArgs e)
        {
            //move selected one or more plan sub trips to the list to the left
            if (dgvSortedPlanRoutedSubTrips.SelectedRows.Count == 0)
            {
                if (dgvSortedPlanRoutedSubTrips.SelectedCells.Count == 0)
                {
                    MessageBox.Show(TptResourceUI.SelectSortedPTSTBeforeMove);
                }
                else if (dgvSortedPlanRoutedSubTrips.SelectedCells.Count == 1)
                {
                    //get the selected un sorted plan sub trip
                    DataGridViewCell dgvc = dgvSortedPlanRoutedSubTrips.SelectedCells[0];
                    Int32 selectedRowIndex = dgvc.RowIndex;
                    PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[selectedRowIndex];
                    //add it to the sorted list
                    unSortedRoutedPlanSubTrips.Add(ptst);
                    //remove it from the unsorted list
                    PlanTruckSubTrip tmp = new PlanTruckSubTrip();
                    tmp = unSortedRoutedPlanSubTrips.Find(planTruckSubTrip =>
                                                         planTruckSubTrip.SeqNo.Equals(ptst.SeqNo));
                    unSortedRoutedPlanSubTrips.Remove(tmp);
                }
                else
                {
                    //do nothing
                }
            }
            else if (dgvSortedPlanRoutedSubTrips.SelectedRows.Count >= 1)
            {
                List<Int32> selectedIndexes = new List<int>();
                foreach (DataGridViewRow rowSelected in dgvSortedPlanRoutedSubTrips.SelectedRows)
                {
                    selectedIndexes.Add(rowSelected.Index);
                }

                selectedIndexes.Sort();

                Int32 j = 0;
                PlanTruckSubTrip ptst = new PlanTruckSubTrip();
                foreach (Int32 i in selectedIndexes)
                {
                    ptst = sortedRoutedPlanSubTrips[i - j];
                    unSortedRoutedPlanSubTrips.Add(ptst);
                    sortedRoutedPlanSubTrips.RemoveAt(i - j);
                    j++;
                }
            }

            bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
            bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            dgvUnSortedPlanRoutedSubTrips.DataSource = bindSourceUnSortedRoutedPlanSubTrips;
            dgvSortedPlanRoutedSubTrips.DataSource = bindSourceSortedRoutedPlanSubTrips;

            OnOffMiddlePanel(true, false);
            btnSaveEditSubTrip.Enabled = false;
            btnDeleteSubTrip.Enabled = false;
        }

                    private Int32 FindPlanSubTripWintinListBySeqNo(List<PlanTruckSubTrip> planTruckSubTrips, Int32 seqNo)
        {
            Int32 index = 0;
            foreach (PlanTruckSubTrip ptst in planTruckSubTrips)
            {
                if (ptst.SeqNo == seqNo)
                {
                    return index;
                }
                index++;
            }

            return index;
        }

        private void btnAddAllToSorted_Click(object sender, EventArgs e)
        {
            //get all the unsorted plan sub trips,
            //and add all of them to the sorted list,
            //and clear the unsorted plan sub trips
            dgvUnSortedPlanRoutedSubTrips.ClearSelection();
            dgvUnSortedPlanRoutedSubTrips.SelectAll();

            foreach (DataGridViewRow rowSelected in dgvUnSortedPlanRoutedSubTrips.SelectedRows)
            {
                //add the selected plan sub trips(s) to the right list first

                PlanTruckSubTrip ptst = (PlanTruckSubTrip)rowSelected.DataBoundItem;
                sortedRoutedPlanSubTrips.Insert(0, ptst);

                //then remvoe the selected student(s) on the left list
                PlanTruckSubTrip tmp = new PlanTruckSubTrip();
                tmp = unSortedRoutedPlanSubTrips.Find(planTruckSubTrip =>
                                                      planTruckSubTrip.SeqNo.Equals(ptst.SeqNo));
                unSortedRoutedPlanSubTrips.Remove(tmp);
            }

            bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
            bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            dgvUnSortedPlanRoutedSubTrips.DataSource = bindSourceUnSortedRoutedPlanSubTrips;
            dgvSortedPlanRoutedSubTrips.DataSource = bindSourceSortedRoutedPlanSubTrips; 

            OnOffMiddlePanel(true, false);
            btnSaveEditSubTrip.Enabled = false;
            btnDeleteSubTrip.Enabled = false;

            return;
        }

        private void btnAddAllToUnsorted_Click(object sender, EventArgs e)
        {
            //get all the sorted plan sub trips,
            //and add all of them to the unsorted list,
            //and clear the sorted plan sub trips
            dgvSortedPlanRoutedSubTrips.ClearSelection();
            dgvSortedPlanRoutedSubTrips.SelectAll();

            foreach (DataGridViewRow rowSelected in dgvSortedPlanRoutedSubTrips.SelectedRows)
            {
                //add the selected plan sub trips(s) to the right list first
                PlanTruckSubTrip ptst = (PlanTruckSubTrip)rowSelected.DataBoundItem;
                unSortedRoutedPlanSubTrips.Insert(0, ptst); 

                //then remvoe the selected student(s) on the left list
                PlanTruckSubTrip tmp = new PlanTruckSubTrip();
                tmp = unSortedRoutedPlanSubTrips.Find(planTruckSubTrip =>
                                                      planTruckSubTrip.SeqNo.Equals(ptst.SeqNo));
                sortedRoutedPlanSubTrips.Remove(tmp);
            }

            bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
            bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            dgvUnSortedPlanRoutedSubTrips.DataSource = bindSourceUnSortedRoutedPlanSubTrips;
            dgvSortedPlanRoutedSubTrips.DataSource = bindSourceSortedRoutedPlanSubTrips; 

            OnOffMiddlePanel(true, false);
            btnSaveEditSubTrip.Enabled = false;
            btnDeleteSubTrip.Enabled = false;
        }

        private void btnSaveEditSubTrip_Click(object sender, EventArgs e)
        {
            //get the selected plan sub trip, display its 
            //details and enable editing.
            //The user can only edit one plan sub trip at one time.

            if (!IsStopsComplete())
            {
                MessageBox.Show(TptResourceUI.FinishPTSTBeforeSave);
                return;
            }

            if (lastFocusedDatagridview.Name.Equals("dgvSortedPlanRoutedSubTrips"))
            {
                if (dgvSortedPlanRoutedSubTrips.SelectedCells.Count >= 1)
                {
                    //delete the selected sorted plan sub trip and replace the edited new one
                    //to the sorted list, and make sure the position of the new added
                    //is the same with the one be edited
                    DataGridViewCell dgvc = dgvSortedPlanRoutedSubTrips.SelectedCells[0];
                    Int32 selectedRowIndex = dgvc.RowIndex; ;
                    PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[selectedRowIndex];

                    string strStartTime = dtpStartTime.Value.ToShortTimeString();
                    strStartTime = chosenDate.ToShortDateString() + " " + strStartTime;
                    string strEndTime = dtpEndTime.Value.ToShortTimeString();
                    strEndTime = chosenDate.ToShortDateString() + " " + strEndTime;

                    DateTime startTime = Convert.ToDateTime(strStartTime);
                    DateTime endTime = Convert.ToDateTime(strEndTime);
                    Stop startStop = Stop.GetStop(((OperatorDTO)iplcboStartCode.SelectedItem).Code);
                    Stop endStop = Stop.GetStop(((OperatorDTO)iplcboEndCode.SelectedItem).Code);

                    ptst = new PlanTruckSubTrip(0, startTime, endTime, startStop, endStop, false,
                                                                 String.Empty, new SortableList<PlanTruckSubTripJob>(),
                                                                 planTruckTrip, JobTripStatus.Assigned,
                                                                 new byte[8]);
                    sortedRoutedPlanSubTrips[selectedRowIndex] = ptst; //replacement happens here
                    bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
                }
            }
            else if (lastFocusedDatagridview.Name.Equals("dgvUnSortedPlanRoutedSubTrips"))
            {

                if (dgvUnSortedPlanRoutedSubTrips.SelectedCells.Count >= 1)
                {
                    DataGridViewCell dgvc = dgvUnSortedPlanRoutedSubTrips.SelectedCells[0];
                    Int32 selectedRowIndex = dgvc.RowIndex; ;
                    PlanTruckSubTrip ptst = unSortedRoutedPlanSubTrips[selectedRowIndex];

                    string strStartTime = dtpStartTime.Value.ToShortTimeString();
                    strStartTime = chosenDate.ToShortDateString() + " " + strStartTime;
                    string strEndTime = dtpEndTime.Value.ToShortTimeString();
                    strEndTime = chosenDate.ToShortDateString() + " " + strEndTime;

                    DateTime startTime = Convert.ToDateTime(strStartTime);
                    DateTime endTime = Convert.ToDateTime(strEndTime);
                    Stop startStop = Stop.GetStop(((OperatorDTO)iplcboStartCode.SelectedItem).Code);
                    Stop endStop = Stop.GetStop(((OperatorDTO)iplcboEndCode.SelectedItem).Code);

                    ptst = new PlanTruckSubTrip(0, startTime, endTime, startStop, endStop, false,
                                                                 String.Empty, new SortableList<PlanTruckSubTripJob>(),
                                                                 planTruckTrip, JobTripStatus.Assigned,
                                                                 new byte[8]);
                    unSortedRoutedPlanSubTrips[selectedRowIndex] = ptst; //replacement happens here
                    bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
                }
            }

            bIsAddMode = true;
            OnOffMiddlePanel(true, false);
            OnOffButtons(bIsAddMode);

            return;
        }

        private void btnDeleteSubTrip_Click(object sender, EventArgs e)
        {
            //get the selected plan sub trip, and 
            //delete it from the unsorted list

            if (lastFocusedDatagridview.Name.Equals("dgvSortedPlanRoutedSubTrips"))
            {
                if (dgvSortedPlanRoutedSubTrips.SelectedCells.Count >= 1)
                {
                    //delete the selected sorted plan sub trip and replace the edited new one
                    //to the sorted list, and make sure the position of the new added
                    //is the same with the one be edited

                    DialogResult dr = MessageBox.Show(TptResourceUI.PromptToDelPTST, CommonResource.Warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dr != DialogResult.OK)
                    {
                        return;
                    }

                    DataGridViewCell dgvc = dgvSortedPlanRoutedSubTrips.SelectedCells[0];
                    Int32 selectedRowIndex = dgvc.RowIndex; ;
                    PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[selectedRowIndex];
                    sortedRoutedPlanSubTrips.RemoveAt(selectedRowIndex); //removement happens here
                    bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);

                }
            }
            else if (lastFocusedDatagridview.Name.Equals("dgvUnSortedPlanRoutedSubTrips"))
            {

                if (dgvUnSortedPlanRoutedSubTrips.SelectedCells.Count >= 1)
                {
                    DialogResult dr = MessageBox.Show(TptResourceUI.PromptToDelPTST, CommonResource.Warning, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dr != DialogResult.OK)
                    {
                        return;
                    }

                    DataGridViewCell dgvc = dgvUnSortedPlanRoutedSubTrips.SelectedCells[0];
                    Int32 selectedRowIndex = dgvc.RowIndex; ;
                    PlanTruckSubTrip ptst = unSortedRoutedPlanSubTrips[selectedRowIndex];

                    unSortedRoutedPlanSubTrips.RemoveAt(selectedRowIndex); //removement happens here
                    bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
                }
            }

            // 2013-09-02 Zhou Kai adds
            bIsAddMode = true;
            OnOffMiddlePanel(true, false);
            OnOffButtons(bIsAddMode);
            // 2013-09-02 ends

            return;
        }

        private void btnSaveRoute_Click(object sender, EventArgs e)
        {
            //save the whole plan routed truck sub trips

            if (unSortedRoutedPlanSubTrips.Count > 0)
            {
                MessageBox.Show(TptResourceUI.UnSortedPTSTRemain);
                return;
            }
            if (sortedRoutedPlanSubTrips.Count == 0)
            {
                MessageBox.Show(TptResourceUI.NoSortedPTSTYet);
                return;
            }

            List<Stop> startStops, endStops;
            List<DateTime> startTimes, endTimes;

            //auto sort by start time for the user
            PlanTruckTrip.SortPlanTruckSubTripsByStartDateTime(sortedRoutedPlanSubTrips);
            bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            GetPlanRouteSubTripArgs(sortedRoutedPlanSubTrips, out startStops, out endStops, out startTimes, out endTimes);
            string feedback = String.Empty;
            if (planTruckTrip.CanAddPlanSubTripByRoute(startStops, endStops, startTimes, endTimes, out feedback))
            {
                SortableList<PlanTruckSubTrip> planTruckSubTrips = new SortableList<PlanTruckSubTrip>();
                foreach (PlanTruckSubTrip ptst in sortedRoutedPlanSubTrips)
                {
                    planTruckSubTrips.Add(ptst);
                }

                try
                {
                    planTruckTrip.AddRoutedPlanTruckSubTrips(planTruckSubTrips, user.UserID);
                    sortedRoutedPlanSubTrips.Clear();
                    bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
                    bRouteSavedSuccessfully = true;
                    parent.RefreshData();
                    parent.schedulerControl1.Refresh();

                    MessageBox.Show(CommonResource.SaveSuccess, CommonResource.SaveSuccess, MessageBoxButtons.OK);
                    bRouteSavedSuccessfully = true;
                    btnSaveEditSubTrip.Enabled = false;
                    btnDeleteSubTrip.Enabled = false;
                }
                catch(FMException fe)
                {
                    MessageBox.Show(TptResourceUI.FollowingPlanTruckSubTrips + fe.ToString() +
                                    "\n" + TptResourceUI.FailedUpdateDB);
                    bRouteSavedSuccessfully = false;
                    //feedback is from FMException.Message
                    MessageBox.Show(feedback, CommonResource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                bRouteSavedSuccessfully = false;
                MessageBox.Show(TptResourceUI.ErrorInRoutSetting + Environment.NewLine + feedback);
            }

            bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
            bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            return;
        }

        #region "Display the details of the plan sub trip when selecting one"

        private void dgvUnSortedPlanRoutedSubTrips_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell dgvc = dgv.SelectedCells[0];
            Int32 selectedRowIndex = dgvc.RowIndex;
            dgvUnSortedPlanRoutedSubTrips.Rows[selectedRowIndex].Selected = true;
            PlanTruckSubTrip ptst = unSortedRoutedPlanSubTrips[selectedRowIndex];
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;

            btnSaveEditSubTrip.Enabled = true;
            btnDeleteSubTrip.Enabled = true;

            OnOffMiddlePanel(false, true);
            bIsAddMode = false;
        }

        private void dgvSortedPlanRoutedSubTrips_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell dgvc = dgv.SelectedCells[0];
            Int32 selectedRowIndex = dgvc.RowIndex;
            dgvSortedPlanRoutedSubTrips.Rows[selectedRowIndex].Selected = true;
            PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[selectedRowIndex];
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;

            OnOffMiddlePanel(false, true);
            bIsAddMode = false;
        }

        #endregion

        #region "move selected rows up or down within the sorted list"

        private void btnUp_Click(object sender, EventArgs e)
        {
            //move the selected sorted plan sub trip(s) up for 1 position.
            //if more than 1 plan sub trips are selected, 
            //they have to be continuous.

            List<PlanTruckSubTrip> selectedPlanSubTrips = new List<PlanTruckSubTrip>();
            Int32 position = 0;
            List<Int32> selectedIndex = new List<int>(); //to remember the indexes of the selected rows.

            if (dgvSortedPlanRoutedSubTrips.SelectedRows.Count == 0)
            {
                if (dgvSortedPlanRoutedSubTrips.SelectedCells.Count == 0)
                {
                    MessageBox.Show(TptResourceUI.SelectSortedPTSTToMoveUp);
                    return;
                }
                else if (dgvSortedPlanRoutedSubTrips.SelectedCells.Count == 1)
                {
                    //get the selected un sorted plan sub trip
                    DataGridViewCell dgvc = dgvSortedPlanRoutedSubTrips.SelectedCells[0];
                    position = dgvc.RowIndex;
                    PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[position];
                    selectedPlanSubTrips.Add(ptst);
                }
                else
                {
                    //no cells, no rows are selected, so do nothing here.
                }
            }
            else if (dgvSortedPlanRoutedSubTrips.SelectedRows.Count >= 1)
            {
                foreach (DataGridViewRow rowSelected in dgvSortedPlanRoutedSubTrips.SelectedRows)
                {
                    //add the selected plan sub trips(s) to the right list first
                    PlanTruckSubTrip ptst = (PlanTruckSubTrip)rowSelected.DataBoundItem;
                    selectedPlanSubTrips.Add(ptst);

                    if (position < rowSelected.Index)
                    {
                        position = rowSelected.Index;
                    }
                }
            }

            MoveUpWithinList(sortedRoutedPlanSubTrips, selectedPlanSubTrips, position, selectedIndex);
            bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            dgvSortedPlanRoutedSubTrips.DataSource = bindSourceSortedRoutedPlanSubTrips;
            foreach (Int32 i in selectedIndex)
            {
                if (i < dgvSortedPlanRoutedSubTrips.Rows.Count &&
                    i >= 0)
                {
                    dgvSortedPlanRoutedSubTrips.Rows[i].Selected = true;
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            //move the selected sorted plan sub trip(s) down 
            //get the selected rows, then get the proceeding row.
            //swap the selected rows with the their proceeding row.

            List<PlanTruckSubTrip> selectedPlanSubTrips = new List<PlanTruckSubTrip>();
            Int32 position = 0;
            List<Int32> selectedIndex = new List<int>();

            if (dgvSortedPlanRoutedSubTrips.SelectedRows.Count == 0)
            {
                if (dgvSortedPlanRoutedSubTrips.SelectedCells.Count == 0)
                {
                    MessageBox.Show(TptResourceUI.SelectSortedPTSTToMoveDown);
                    return;
                }
                else if (dgvSortedPlanRoutedSubTrips.SelectedCells.Count == 1)
                {
                    //get the selected un sorted plan sub trip
                    DataGridViewCell dgvc = dgvSortedPlanRoutedSubTrips.SelectedCells[0];
                    position = dgvc.RowIndex;
                    PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[position];
                    selectedPlanSubTrips.Add(ptst);
                }
                else
                {
                    //no cells, no rows are selected, so do nothing here.
                }
            }
            else if (dgvSortedPlanRoutedSubTrips.SelectedRows.Count >= 1)
            {

                foreach (DataGridViewRow rowSelected in dgvSortedPlanRoutedSubTrips.SelectedRows)
                {
                    //add the selected plan sub trips(s) to the right list first
                    PlanTruckSubTrip ptst = (PlanTruckSubTrip)rowSelected.DataBoundItem;
                    selectedPlanSubTrips.Add(ptst);

                    if (position < rowSelected.Index)
                    {
                        position = rowSelected.Index;
                    }
                }
            }

            MoveDownWithinList(sortedRoutedPlanSubTrips, selectedPlanSubTrips, position, selectedIndex);
            bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
            dgvSortedPlanRoutedSubTrips.DataSource = bindSourceSortedRoutedPlanSubTrips;

            foreach (Int32 i in selectedIndex)
            {
                if (i < dgvSortedPlanRoutedSubTrips.Rows.Count && 
                    i >= 0)
                {
                    dgvSortedPlanRoutedSubTrips.Rows[i].Selected = true;
                }
            }
        }

        private void MoveUpWithinList(List<PlanTruckSubTrip> planTruckSubTripList,
                                      List<PlanTruckSubTrip> selectedPlanSubTruckTrips,
                                      Int32 position,
                                      List<Int32> selectedIndex)
        {
            //notice if the user select more than one row to
            //move up, then these rows should be continuous, can't be discrete

            if (position - selectedPlanSubTruckTrips.Count == -1)
            {
                MessageBox.Show(TptResourceUI.PTSTAlreadyAtTop);
                return;
            }

            //first to check if the selected rows are continuous
            Int32 gap = selectedPlanSubTruckTrips.Count;
            if (IsSelectedRowContinuous(dgvSortedPlanRoutedSubTrips.SelectedRows))
            {
                Int32 i = 0;

                for (Int32 j = gap; j > 0; j--)
                {
                    PlanTruckSubTrip pts = (PlanTruckSubTrip)dgvSortedPlanRoutedSubTrips.SelectedRows[j - 1].DataBoundItem;
                    planTruckSubTripList.Insert(position - gap + i, pts);
                    planTruckSubTripList.RemoveAt(position - gap + i + 2);
                    i++;
                }

                foreach (DataGridViewRow dgvr in dgvSortedPlanRoutedSubTrips.SelectedRows)
                {
                    selectedIndex.Add(dgvr.Index - 1);
                }
            }
            return;
        }

        private void MoveDownWithinList(List<PlanTruckSubTrip> planTruckSubTripList,
                                        List<PlanTruckSubTrip> selectedPlanSubTruckTrips,
                                        Int32 position,
                                        List<Int32> selectedIndex)
        {
            //notice if the user select more than one row to
            //move down, then these rows should be continuous, can't be discrete

            Int32 gap = selectedPlanSubTruckTrips.Count;
            if (position + 1 == planTruckSubTripList.Count)
            {
                MessageBox.Show(TptResourceUI.PTSTAlreadyAtBottom);
                return;
            }

            //first to check if the selected rows are continuous
            if (IsSelectedRowContinuous(dgvSortedPlanRoutedSubTrips.SelectedRows))
            {
                for (Int32 j = gap; j > 0; j--)
                {
                    PlanTruckSubTrip pts = selectedPlanSubTruckTrips[j - 1];
                    planTruckSubTripList.Insert(position + 2, pts);
                    planTruckSubTripList.RemoveAt(position - gap + 1);
                }

                foreach (DataGridViewRow dgvr in dgvSortedPlanRoutedSubTrips.SelectedRows)
                {
                    selectedIndex.Add(dgvr.Index + 1);
                }
            }

            return;
        }

        #endregion

        private void dgvUnSortedPlanRoutedSubTrips_Click(object sender, EventArgs e)
        {
            //set the lastFocusedControl to it
            lastFocusedDatagridview = (DataGridView)sender;
        }

        private void dgvSortedPlanRoutedSubTrips_Click(object sender, EventArgs e)
        {
            //set the lastFocusedControl to it
            lastFocusedDatagridview = (DataGridView)sender;
        }

        private void dtpStartTime_ValueChanged(object sender, EventArgs e)
        {
            dtpEndTime.Value = dtpStartTime.Value.AddHours(1);
        }

        private void dgvUnSortedPlanRoutedSubTrips_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            Int32 selectedIndex = dgv.SelectedRows[0].Index;
            PlanTruckSubTrip ptst = unSortedRoutedPlanSubTrips[selectedIndex];
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;

            OnOffMiddlePanel(false, true);

            return;
        }

        private void dgvSortedPlanRoutedSubTrips_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            Int32 selectedIndex = dgv.SelectedRows[0].Index;
            PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[selectedIndex];
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;

            OnOffMiddlePanel(false, true);

            return;
        }

        private void dgvUnSortedPlanRoutedSubTrips_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //double click a cell will make the clicked plan truck sub trip into edit mode
            EnableMiddlePanelForEdit(sender, e);

            return;
        }

        private void dgvSortedPlanRoutedSubTrips_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //double click a cell will make the clicked plan truck sub trip into edit mode
            EnableMiddlePanelForEdit(sender, e);

            return;
        }

        private void dgvUnSortedPlanRoutedSubTrips_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if it's the StartTime column clicked, sort the plan sub trips by start time,
            //otherwise do nothing
            DataGridView dgv = (DataGridView)sender;
            Int32 colIndex = e.ColumnIndex;

            if (dgv.Columns[colIndex].HeaderText.Equals("StartTime"))
            {
                PlanTruckTrip.SortPlanTruckSubTripsByStartDateTime(unSortedRoutedPlanSubTrips);
                bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
                return;
            }

            if (dgv.Columns[colIndex].HeaderText.Equals("EndTime"))
            {
                PlanTruckTrip.SortPlanTruckSubTripsByEndDateTime(unSortedRoutedPlanSubTrips);
                bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
                return;
            }

            if (dgv.Columns[colIndex].HeaderText.Equals("StartStop"))
            {
                PlanTruckTrip.SortPlanTruckSubTripsByStartStop(unSortedRoutedPlanSubTrips);
                bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
                return;
            }

            if (dgv.Columns[colIndex].HeaderText.Equals("EndStop"))
            {
                PlanTruckTrip.SortPlanTruckSubTripsByEndStop(unSortedRoutedPlanSubTrips);
                bindSourceUnSortedRoutedPlanSubTrips.ResetBindings(false);
                return;
            }
        }

        private void dgvSortedPlanRoutedSubTrips_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if it's the StartTime column clicked, sort the plan sub trips by start time,
            //otherwise do nothing
            DataGridView dgv = (DataGridView)sender;
            Int32 colIndex = e.ColumnIndex;
            if (dgv.Columns[colIndex].HeaderText.Equals("StartTime"))
            {
                PlanTruckTrip.SortPlanTruckSubTripsByStartDateTime(sortedRoutedPlanSubTrips);
                bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
                return;
            }

            if (dgv.Columns[colIndex].HeaderText.Equals("EndTime"))
            {
                PlanTruckTrip.SortPlanTruckSubTripsByEndDateTime(sortedRoutedPlanSubTrips);
                bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
                return;
            }

            if (dgv.Columns[colIndex].HeaderText.Equals("StartStop"))
            {
                PlanTruckTrip.SortPlanTruckSubTripsByStartStop(sortedRoutedPlanSubTrips);
                bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
                return;
            }

            if (dgv.Columns[colIndex].HeaderText.Equals("EndStop"))
            {
                PlanTruckTrip.SortPlanTruckSubTripsByEndStop(sortedRoutedPlanSubTrips);
                bindSourceSortedRoutedPlanSubTrips.ResetBindings(false);
                return;
            }
        }

        private void dgvUnSortedPlanRoutedSubTrips_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            Int32 selectedIndex = dgv.SelectedRows[0].Index;
            PlanTruckSubTrip ptst = unSortedRoutedPlanSubTrips[selectedIndex];
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;

            OnOffMiddlePanel(false, true);
            btnAddSubTrip.Enabled = true;
            bIsAddMode = false;

            return;
        }

        private void dgvSortedPlanRoutedSubTrips_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            Int32 selectedIndex = dgv.SelectedRows[0].Index;
            PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[selectedIndex];
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;

            OnOffMiddlePanel(false, true);
            btnAddSubTrip.Enabled = true;
            bIsAddMode = false;

            return;
        }

        // 2013-09-02 Zhou Kai adds:
        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            bIsAddMode = true;
            OnOffMiddlePanel(true, false);
            OnOffButtons(bIsAddMode);

        }

        private void dgvUnSortedPlanRoutedSubTrips_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell dgvc = dgv.SelectedCells[0];
            Int32 selectedRowIndex = dgvc.RowIndex;
            dgvUnSortedPlanRoutedSubTrips.Rows[selectedRowIndex].Selected = true;
            PlanTruckSubTrip ptst = unSortedRoutedPlanSubTrips[selectedRowIndex];
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;

            btnSaveEditSubTrip.Enabled = true;
            btnDeleteSubTrip.Enabled = true;

            OnOffMiddlePanel(false, true);
            bIsAddMode = false;
        }

        private void dgvUnSortedPlanRoutedSubTrips_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //double click a cell will make the clicked plan truck sub trip into edit mode
            EnableMiddlePanelForEdit(sender, e);
        }

        private void dgvSortedPlanRoutedSubTrips_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell dgvc = dgv.SelectedCells[0];
            Int32 selectedRowIndex = dgvc.RowIndex;
            dgvSortedPlanRoutedSubTrips.Rows[selectedRowIndex].Selected = true;
            PlanTruckSubTrip ptst = sortedRoutedPlanSubTrips[selectedRowIndex];
            iplcboStartCode.Text = ptst.StartStop.Code;
            txtStartAddress.Text = ptst.StartStop.Address1;
            txtStartAddress2.Text = ptst.StartStop.Address2;
            txtStartAddress3.Text = ptst.StartStop.Address3;
            txtStartAddress4.Text = ptst.StartStop.Address4;

            iplcboEndCode.Text = ptst.EndStop.Code;
            txtEndAddress.Text = ptst.EndStop.Address1;
            txtEndAddress2.Text = ptst.EndStop.Address2;
            txtEndAddress3.Text = ptst.EndStop.Address3;
            txtEndAddress4.Text = ptst.EndStop.Address4;

            dtpStartTime.Value = ptst.Start;
            dtpEndTime.Value = ptst.End;

            OnOffMiddlePanel(false, true);
            bIsAddMode = false;
        }

        private void dgvSortedPlanRoutedSubTrips_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //double click a cell will make the clicked plan truck sub trip into edit mode
            EnableMiddlePanelForEdit(sender, e);
        }

        // ends

        #endregion

        
    }
}
