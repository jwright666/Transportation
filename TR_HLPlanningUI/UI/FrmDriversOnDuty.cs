using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TR_LanguageResource.Resources;
using TR_MessageDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_HLPlanDLL.BLL;
using TR_FormBaseLibrary;
using FM.TR_HLBookDLL.BLL;

namespace FM.TransportPlanning.UI
{
    public partial class FrmDriversOnDuty : Form
    {
        #region "Fields"
        public List<Driver> AllDriversOnDuty { get; set; }
        public Driver SelectedDriver { get; set; }
        public FrmHaulierPlanningEntry FrmPlanEntry { get; set; }
        public HaulierJobTrip SelectedTrip { get; set; }

        #endregion

        #region "Constructors"
        public FrmDriversOnDuty()
        {
            InitializeComponent();
        }

        public FrmDriversOnDuty(FrmHaulierPlanningEntry parent, HaulierJobTrip trip)
        {
            InitializeComponent();

            this.AllDriversOnDuty = parent.drivers.ToList();
            this.FrmPlanEntry = parent;
            this.SelectedTrip = trip;

            foreach (Driver d in AllDriversOnDuty)
            {
                clbDriversOnDuty.Items.Add(d.Name);
            }
            SelectedDriver = new Driver(); // .Name == String.Empty
        }

        #endregion

        #region "Events"
        /// <summary>
        /// If one driver is selected, and the button assign is clicked,
        /// try to append the selected(right-clicked) job trip to the
        /// driver as a plan sub trip. But before doing so, do some validation:
        /// the leg should always start and end in sequence of the same leg group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAssign_Click(object sender, EventArgs e)
        {
            switch (clbDriversOnDuty.CheckedItems.Count)
            {
                case 0:
                    {
                        MessageBox.Show("Please select one driver first.");
                        return;
                        break;
                    }
                case 1: {
                    SelectedDriver =
                        AllDriversOnDuty.FirstOrDefault(
                            x => x.Name == clbDriversOnDuty.CheckedItems[0].ToString());
                    // The core logic goes here:
                    // 1 Get the plan trip
                    // 2 Get all plan sub trips of the schedule date
                    // 3 If it's a leg of a leg group, it should start and end
                    //  in sequence among other legs of the same group(the other legs
                    //  may be carried out by other drivers)
                    // 4 If 3 is true, create a plan sub trip based on the job trip,
                    //   attach the job trip to the plan sub trip, and place the plan
                    //   sub trip next to the last plan sub trip(if any) of the 
                    //   assigned driver

                    // step 1
                    PlanHaulierTrip planTrip = FrmPlanEntry.planHaulierTrips.FirstOrDefault(
                        x=>x.Driver.Code == SelectedDriver.Code);
                    if (planTrip == null) 
                    {
                        // plan trip is not created / saved yet
                        planTrip = new PlanHaulierTrip("1", FrmPlanEntry.chosenDate,
                           Vehicle.GetVehicle(SelectedDriver.defaultVehicleNumber),
                           SelectedDriver,
                           new byte[8]);
                        // add the new plan trip to memory, if the whole operation failed,
                        // don't forget to remove it from memory
                        FrmPlanEntry.planHaulierTrips.Add(planTrip); 
                    } 
                    
                    // step 2
                    List<PlanHaulierSubTrip> allPlanSubTrips = 
                        new List<PlanHaulierSubTrip>();
                    foreach(PlanHaulierTrip pht in FrmPlanEntry.planHaulierTrips)
                    {
                        allPlanSubTrips.AddRange(pht.HaulierSubTrips.ToList());
                    }
                    
                    // step 3
                    // get the end time of the last plan sub trip of the selected driver
                    //List<PlanHaulierSubTrip> planSubTripsOfSelectedDriver = 
                    //    planTrip.HaulierSubTrips.ToList();
                    //DateTime tailEndTime = planSubTripsOfSelectedDriver.Count > 0 ?
                    //    planSubTripsOfSelectedDriver.Max(x => x.End) : 
                    //    planTrip.ScheduleDate.Date;

                    switch(SelectedTrip.LegType)
                    {
                        case Leg_Type.FirstOfTwoLeg:
                            {
                                // always valid
                                AddHaulierJobTripToPlanTrip(planTrip, SelectedTrip);
                                this.Close();
                                break;
                            }
                        case Leg_Type.SecondOfTwoLeg:
                            {
                                // should start and end after the first leg
                                // get the first leg and the plan sub trip containing it
                                HaulierJobTrip firstLeg = HaulierJobTrip.GetHaulierJobTrip(
                                    SelectedTrip.JobID,
                                    SelectedTrip.PartnerLeg);
                                PlanHaulierSubTrip planSubTripContainingFirstLeg =
                                    new PlanHaulierSubTrip();
                                foreach(PlanHaulierSubTrip pst in allPlanSubTrips)
                                {
                                    foreach(HaulierJobTrip jt in pst.JobTrips)
                                    {
                                        if (jt.JobID == firstLeg.JobID &&
                                            jt.Sequence == firstLeg.Sequence)
                                        {
                                            planSubTripContainingFirstLeg = pst;
                                            // if the driver of the plan sub trip
                                            // is the same with the Selected Driver, valid
                                            // or, need to compare the end time of the
                                            // plan sub trip of the first leg, and
                                            // the time when the selected driver finishing
                                            // his last trip.
                                            if (pst.DriverNumber == SelectedDriver.Code)
                                            {
                                                AddHaulierJobTripToPlanTrip(planTrip, SelectedTrip);
                                                this.Close();
                                                break;
                                            }
                                            else
                                            {
                                                MessageBox.Show("The previous leg of this leg is " +
                                                    "assigned to another driver, " +
                                                    "please drag & drop to assign this trip");
                                                // revert the status
                                                SelectedTrip.TripStatus = JobTripStatus.Booked;
                                                this.Close();
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case Leg_Type.OneLeg:
                            {
                                // if it's single leg one leg, always valid
                                if (!SelectedTrip.isMulti_leg)
                                {
                                    AddHaulierJobTripToPlanTrip(planTrip, SelectedTrip);
                                    this.Close();
                                    break;
                                }
                                else
                                {
                                    // get the previous leg first
                                    // and if its previous leg is under the same driver, valid
                                    // otherwise, invalid.
                                    foreach(PlanHaulierSubTrip pst in planTrip.HaulierSubTrips)
                                    {
                                        foreach(HaulierJobTrip jt in pst.JobTrips)
                                        {
                                            if (jt.JobID == SelectedTrip.JobID &&
                                                jt.Sequence == SelectedTrip.LegGroupMember - 1)
                                            {
                                                if (pst.DriverNumber == SelectedDriver.Code)
                                                {
                                                    AddHaulierJobTripToPlanTrip(planTrip, SelectedTrip);
                                                    this.Close();
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                    MessageBox.Show("The previous leg of this leg is " +
                                                    "assigned to another driver, " +
                                                    "please drag & drop to assign this trip");
                                    // revert the status
                                    SelectedTrip.TripStatus = JobTripStatus.Booked;
                                    this.Close();
                                    break;
                                }

                                break;
                            }
                    }

                    break;
                }
                default:
                    {
                        MessageBox.Show("Can only select one driver for a job trip. " +
                            "Please uncheck the other drivers before assigning.");
                        return;
                        break;
                    }
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void clbDriversOnDuty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clbDriversOnDuty.CheckedItems.Count > 1)
            {
                MessageBox.Show("Can only select one driver for a job trip. " +
                    "Please uncheck the other drivers before assigning.");
            }
            else if (clbDriversOnDuty.CheckedItems.Count == 1)
            {
                SelectedDriver = 
                    AllDriversOnDuty.FirstOrDefault(x=>x.Name == clbDriversOnDuty.CheckedItems[0]);
            }
            else if (clbDriversOnDuty.CheckedItems.Count == 0)
            {
                SelectedDriver = new Driver(); // an empty driver
            }
        }

        private void AddHaulierJobTripToPlanTrip(PlanHaulierTrip planTrip, 
            HaulierJobTrip jobTrip)
        {
            List<PlanHaulierSubTrip> planSubTrips =
                        planTrip.HaulierSubTrips.ToList();
            DateTime tailEndTime = planSubTrips.Count > 0 ? planSubTrips.Max(x => x.End) :
                planTrip.ScheduleDate.Date;
            jobTrip.TripStatus = JobTripStatus.Assigned;
            PlanHaulierSubTrip newPlanHaulierSubTrip =
                new PlanHaulierSubTrip(1, tailEndTime.AddSeconds(1.0),
                    tailEndTime.AddMinutes(30.0),
                    jobTrip.StartStop,
                    jobTrip.EndStop,
                    jobTrip.isBillable,
                    jobTrip.CargoDescription,
                    new SortableList<HaulierJobTrip>() { 
                                                        jobTrip
                                                        },
                    new Vehicle(),
                    planTrip,
                    JobTripStatus.Assigned
                    );
            planTrip.AddSubTrip(newPlanHaulierSubTrip);
            FrmPlanEntry.RefreshData();
        }

        #endregion

    }
}
