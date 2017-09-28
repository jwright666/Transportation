using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_TRKPlanDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using TR_FormBaseLibrary;

namespace FM.TR_TRKPlanningUI.UI
{
    public partial class FrmAddPlanTruckSubTrip : Form
    {
        private const string formName = "TPT_TRK_E_ADD_PLAN_SUB_TRIP";
        private User user;
        private FrmTruckPlanningEntry parent;
        string drivernumber = "";
        private Stop startStop;
        private Stop endStop;
        private Appointment apt;

        public FrmAddPlanTruckSubTrip()
        {
            InitializeComponent();
        }

        public FrmAddPlanTruckSubTrip(FrmTruckPlanningEntry parent,User user)
        {
            this.user = user;
            this.parent = parent;
            InitializeComponent();
        }
        //20130513 - Gerry Added new constractor for new schedule option create new appointment
        public FrmAddPlanTruckSubTrip(FrmTruckPlanningEntry parent, Appointment apt, User user)
        {
            this.user = user;
            this.parent = parent;
            this.apt = apt;
            InitializeComponent();
        }
        //20130514 - Gerry Added, use when right from scheduler -> new truck plansubtrip
        private void DisplayDefaultStartStop()
        {
            try
            {
                if (parent.currentPlanTruckTrip.planTruckSubTrips.Count > 0)
                {
                    //int preceedingIndex = parent.currentPlanTruckTrip.GetPrecedingPlanTruckSubTripIndex(apt.Start, apt.End);
                    if (apt != null)
                    {
                        if (parent.currentPlanTruckTrip != null)
                        {
                            int preceedingIndex = parent.currentPlanTruckTrip.GetPrecedingPlanTruckSubTripIndex(apt.Start, apt.End);
                            if (preceedingIndex >= 0)
                            {
                                cboStartStop.Text = parent.currentPlanTruckTrip.planTruckSubTrips[preceedingIndex].EndStop.Code.ToString();
                                grpStart.Enabled = false;
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
        public void Localize()
        {
            this.Text = TptResourceUI.AddPlanSubTrip;
            grpStart.Text = CommonResource.Start;
            grpEnd.Text = CommonResource.End;
            lblCodeStart.Text = CommonResource.Code + ":"; ;
            lblCodeEnd.Text = CommonResource.Code + ":"; ;
            lblAddressStart.Text = CommonResource.Address + ":"; ;
            lblAddressEnd.Text = CommonResource.Address + ":"; ;
            lblCodeStart.Text = CommonResource.City + ":"; ;
            lblCityEnd.Text = CommonResource.City + ":"; ;
            lblStart.Text = CommonResource.Start + ":"; ;
            lblEnd.Text = CommonResource.End + ":"; ;
            lblDriver.Text = CommonResource.Driver + ":"; ;
            btnCancel.Text = CommonResource.Cancel;
            btnSave.Text = CommonResource.Save;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        //This method is used when creating a new planTruckSubTrip from UI, without any job trips
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Driver tempDriver = new Driver();
                tempDriver = (Driver)cboDriverCode.SelectedItem;
                drivernumber = tempDriver.Code;
                DateTime start = dtpStartDate.Value.Date + dtpStartTime.Value.TimeOfDay;  
                DateTime end = dtpEndDate.Value.Date + dtpEndTime.Value.TimeOfDay;
                for (int i = 0; i < parent.planTruckTrips.Count; i++)
                {
                    if (parent.planTruckTrips[i].Driver.Code == drivernumber)
                    {
                        string message = "";
                        if (parent.planTruckTrips[i].ValidateTime(/*drivernumber,*/ start, end, out message))
                        {
                            PlanTruckSubTrip planTruckSubTrip = new PlanTruckSubTrip();
                            //We got wrong seq no, if we use this.
                            //we can set this to BLL
                            //planTruckSubTrip.SeqNo = parent.planTruckTrips[i].TruckSubTrips.Count + 1;
                        
                            planTruckSubTrip.Start = start;
                            planTruckSubTrip.End = end;
                            planTruckSubTrip.StartStop = startStop; // (Stop)cboStartStop.SelectedItem;
                            planTruckSubTrip.EndStop = endStop; // (Stop)cboEndStop.SelectedItem;
                            planTruckSubTrip.IsBillableTrip = true;
                            planTruckSubTrip.Description = "";
                            planTruckSubTrip.VehicleNumber = Driver.GetDriver(drivernumber).defaultVehicleNumber;
                            planTruckSubTrip.DriverNumber = drivernumber;
                            planTruckSubTrip.Status = JobTripStatus.Assigned;

                            parent.planTruckTrips[i].AddPlanTruckSubTrip(planTruckSubTrip, formName, user.UserID);
                            
                        }
                        else
                        {
                            //MessageBox.Show(message);
                            throw new FMException(message);
                        }
                        break;
                    }
                }
                //parent.affectedDrivers.Add(tempDriver);
                parent.RefreshData();
                parent.schedulerControl1.Refresh(); 
                this.Dispose();
                this.Close();
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

        private void FrmAddPlanTruckSubTrip_Load(object sender, EventArgs e)
        {
            Localize();
        }

        private void cboStartStop_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboEndStop_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmAddPlanTruckSubTrip_Load_1(object sender, EventArgs e)
        {
            Localize();
            cboDriverCode.DataSource = parent.drivers;
            cboDriverCode.DisplayMember = "Name";

            //20130514 - Gerry Replaced
            //cboStartStop.DataSource = Stop.GetAllStops();
            cboStartStop.DataSource = Stop.GetStopCodes();
            cboStartStop.DisplayMember = "Code";

            //20130514 - Gerry Replaced
            //cboEndStop.DataSource = Stop.GetAllStops();
            cboEndStop.DataSource = Stop.GetStopCodes();
            cboEndStop.DisplayMember = "Code";

            dtpStartDate.Value = parent.chosenDate;
            dtpEndDate.Value = parent.chosenDate;

            dtpStartTime.Value = DateTime.Today.Date.AddHours(6);
            dtpEndTime.Value = dtpStartTime.Value.AddHours(1);

            //201305 14- Gerry added
            if (apt != null)
            {
                dtpStartTime.Value = apt.Start;
                dtpEndTime.Value = apt.End;
                cboDriverCode.SelectedItem = parent.currentdriver;
                DisplayDefaultStartStop();
            }
        }

        private void cboStartStop_TextChanged_1(object sender, EventArgs e)
        {
            //20130514 - Gerry Replaced
            //Stop stop = (Stop)cboStartStop.SelectedItem;
            startStop = Stop.GetStop(cboStartStop.Text.ToString());
            if (startStop != null)
            {
                txtAddress1From.Text = startStop.Address1;
                txtAddress2From.Text = startStop.Address2;
                txtAddress3From.Text = startStop.Address3;
                txtCityFrom.Text = startStop.City;
                txtDescriptionFrom.Text = startStop.Description;
            }
        }

        private void cboEndStop_TextChanged_1(object sender, EventArgs e)
        {
            //20130514 - Gerry Replaced
            //Stop stop = (Stop)cboEndStop.SelectedItem;
            endStop = Stop.GetStop(cboEndStop.Text.ToString());
            if (endStop != null)
            {
                txtAddress1To.Text = endStop.Address1;
                txtAddress2To.Text = endStop.Address2;
                txtAddress3To.Text = endStop.Address3;
                txtCityTo.Text = endStop.City;
                txtDescriptionTo.Text = endStop.Description;
            }
        }
    }
}
