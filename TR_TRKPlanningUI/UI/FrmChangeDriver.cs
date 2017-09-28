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
    public partial class FrmChangeDriver : Form
    {
        Driver oldDriver;
        FrmTruckPlanningEntry parent;

        public FrmChangeDriver()
        {
            InitializeComponent();
        }

        public FrmChangeDriver(FrmTruckPlanningEntry parent, Driver oldDriver)
        {
            InitializeComponent();
            this.oldDriver = oldDriver;
            this.parent = parent;

        }
        private void Localize()
        {
            this.Text = TptResourceUI.ChangeDriver;
            lblNewDriver.Text = CommonResource.New + CommonResource.Driver + ":";
            lblOldDriver.Text = CommonResource.Old + CommonResource.Driver + ":";
            btnSave.Text = CommonResource.Save;
            btnCancel.Text = CommonResource.Cancel;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 2014-05-15 Zhou Kai adds
                parent.choosedriver = true;
                Driver newDriver = (Driver)cboNewDriverCode.SelectedItem;
                //newDriver.unAvailableDates = newDriver.GetDriverUnavailableDates(parent.chosenDate.Date, parent.chosenDate.Date.AddDays(1).AddMinutes(-1));
                if (newDriver.Code.Equals(oldDriver.Code))
                {
                    MessageBox.Show(TptResourceUI.UseDifferentDriver);
                }
                else
                {
                    //if(parent.IsIntervalAllowed(parent.newdriver,))
                    PlanTruckTrip tmp = PlanTruckTrip.FindPlanTruckTripByDriver(parent.planTruckTrips, newDriver);
                    if (tmp != null)
                    {
                        // notice: this check is only for logic completeness, this case
                        // is prevented on the function: GetUnAllocatedDriverForDate()
                        // drivers with existing planSubTrips will not be available to chose from.
                        if (tmp.planTruckSubTrips.Count > 0)
                            throw new FMException(TptResourceUI.DriverAlreadyHasPlanSubTrips);
                    }
                    //check time slot of new selected driver if available
                    foreach (PlanTruckSubTrip subTrip in parent.currentPlanTruckTrip.planTruckSubTrips)
                    {
                        parent.planTruckSubTrip = subTrip;
                        parent.IsIntervalAllowed(newDriver, new TimeInterval(subTrip.Start, subTrip.End));
                    }


                    parent.currentPlanTruckTrip.ChangeDriver(newDriver, parent.truckJobTrips);
                    parent.RefreshData();
                    parent.PopulateScheduleDate();
                    Close();
                }
            }
            catch (FMException fmex) { MessageBox.Show(fmex.Message); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void FrmChooseDriver_Load(object sender, EventArgs e)
        {
            Localize();
            txtOldDriverCode.Text = oldDriver.Code;
            //txtOldDriverName.Text = oldDriver.Name;
            // 2014-05-15 Zhou Kai comments out the one line below, and replace it with the line below it
            // cboNewDriverCode.DataSource = Driver.GetAllDriverThatNotInBothTruckAndHaulierPlan(parent.chosenDate);
            PlanTruckTrip tmp = PlanTruckTrip.FindPlanTruckTripByDriver(parent.planTruckTrips, oldDriver);
            foreach(PlanTruckSubTrip ptst in tmp.planTruckSubTrips)// 2014-05-23 Zhou Kai adds logic here
            {
                if (ptst.Status > JobTripStatus.Assigned)
                {
                    MessageBox.Show(TptResourceUI.CannotReplaceDriverHasCompletedTrips);
                    this.Close();
                    return;
                }
                foreach (PlanTruckSubTripJob ptstj in ptst.planTruckSubTripJobs)
                {
                    if (ptstj.status > JobTripStatus.Assigned)
                    {
                        MessageBox.Show(TptResourceUI.CannotReplaceDriverHasCompletedTrips);
                        this.Close();
                        return;
                    }
                }
            }

            cboNewDriverCode.DataSource = Driver.GetUnAllocatedDriverForDate(parent.chosenDate);
            // 2014-05-15 Zhou Kai ends
            cboNewDriverCode.DisplayMember = "DescriptionForPlanningPurpose";
        }

        private void cboNewDriverCode_TextChanged(object sender, EventArgs e)
        {
            //Driver a = (Driver)cboNewDriverCode.SelectedItem;
            //txtNewDriverName.Text = a.Name;
        }
    }
}
