using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TruckPlanDLL.BLL;
using FM.TR_HLPlanDLL.BLL;
using FM.TR_HLBookDLL.BLL;
using TR_LanguageResource.Resources;
using System.Data.SqlTypes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using FM.TR_TRKPlanDLL.DAL;

namespace FM.TR_TRKPlanDLL.BLL
{
    public class PlanTruckTrip : PlanTrip
    {
        //private Vehicle vehicle;
        //private Driver driver;
        public PlanTruckSubTrips planTruckSubTrips = new PlanTruckSubTrips();
        public PlanTruckSubTrips oldPlanTruckSubTrips = new PlanTruckSubTrips();
        //20161012 - gerry adde for geo location
        public double centerPoint { get; set; }
        public double radius { get; set; }


        public PlanTruckTrip()
            : base()
        {
            this.dept = "";
            this.planTruckSubTrips = new PlanTruckSubTrips();
            this.oldPlanTruckSubTrips = new PlanTruckSubTrips();
            //this.vehicle = new Vehicle();
            //this.driver = new Driver();
            this.centerPoint = 0;
            this.radius = 0;
        }

        public PlanTruckTrip(string dept, string planTripNo, DateTime scheduleDate, Vehicle vehicle, Driver driver, byte[] updateversion)
            : base(planTripNo, scheduleDate, vehicle, driver, updateversion)
        {
            this.dept = dept;
            this.planTruckSubTrips = new PlanTruckSubTrips();
            this.oldPlanTruckSubTrips = new PlanTruckSubTrips();
            //this.vehicle = vehicle;
            //this.driver = driver;
            this.VehicleNumber = vehicle.Number;
            this.DisplayForPlanning_PrimeMover = driver.defaultVehicleNumber;
            this.DisplayForPlanning_DriverCode = driver.Code;
            this.DisplayForPlanning_Both = driver.defaultVehicleNumber + "(" + driver.Code + ")";
            this.centerPoint = 0;
            this.radius = 0;
        }

        // 2014-03-27 Zhou Kai renames the property from
        // TruckSubTrips to pPlanTruckSubTrips
        public PlanTruckSubTrips pPlanTruckSubTrips
        {
            get { return planTruckSubTrips; }
            set { planTruckSubTrips = value; }
        }

        public PlanTruckSubTrips OldtruckSubTrips
        {
            get { return oldPlanTruckSubTrips; }
            set { oldPlanTruckSubTrips = value; }
        }

        //public Vehicle Vehicle
        //{
        //    get { return vehicle; }
        //    set { vehicle = value; }
        //}

        //public Driver Driver
        //{
        //    get { return driver; }
        //    set { driver = value; }
        //}
        // This method is called from FrmTruckJobTrip in the Planning UI
        // when a TruckJobTrip is allocated to several trucks
        // In the UI a) A TruckJobTripPlan is created b) This method is called to
        // validate c) If true, then CreatePlanTruckSubTrip in PlanTruckSubTrip is called

        public bool CanAddPlanTruckSubTrip(TruckJobTripPlan truckJobTripPlan, out string message, out bool overload)
        {
            message = "";
            overload = false;
            bool flag = true;
            Vehicle truck = new Vehicle();
            truck = Vehicle.GetVehicle(truckJobTripPlan.truckNo);

            //20130521 - Gerry Replaced the error message
            #region Removed
            /*
            if (truckJobTripPlan.weight == 0)
            {
                flag = false;
                message += string.Format(TptResourceBLL.ErrZeroWtVol, CommonResource.Weight, CommonResource.Weight) + "\n";
            }
            if (truckJobTripPlan.volume == 0)
            {
                flag = false;
                message += string.Format(TptResourceBLL.ErrZeroWtVol, CommonResource.Volume, CommonResource.Volume) + "\n";

            }
             */
            #endregion

            if (truckJobTripPlan.weight <= 0 || truckJobTripPlan.volume <= 0)
            {
                flag = false;
                message += TptResourceBLL.ErrTotalWeightOrVolZero + "\n";
            }
            //20130521 end

            if (truckJobTripPlan.volume > truck.VolCapacity)
            {
                overload = true;
                message = message + TptResourceBLL.ErrExceededVolume + "\n";
            }
            if (truckJobTripPlan.weight > truck.MaximumLadenWeight)
            {
                overload = true;
                message = message + TptResourceBLL.ErrExceededWeight + "\n";
            }
            //if (IsPlanSubTripOverlapping(truckJobTripPlan))
            //{
            //    flag = false;
            //    message = message + TptResourceBLL.ErrStartTimeEndTimeOverlapWithOthers;
            //}

            return flag;
        }

        // 2014-03-27 Zhou Kai adds comments:
        /* match the time-gap of [startTime, endTime] with each of the existing planTruckSubTrips
         * inside the current planTruckTrip to see if there're overlaps.
         */
        public bool ValidateTime(/*string drivercode,*/ DateTime startTime, DateTime endTime, out string message)
        {
            bool test = true;

            for (int i = 0; i < this.planTruckSubTrips.Count; i++)
            {
                // eauals
                if ((startTime == planTruckSubTrips[i].Start) && (endTime == planTruckSubTrips[i].End))
                {
                    test = false;
                    break;
                }
                // partly overlapped
                else if ((startTime > planTruckSubTrips[i].Start) && (startTime < planTruckSubTrips[i].End))
                {
                    test = false;
                    break;
                }
                // partly overlapped
                else if ((startTime <= planTruckSubTrips[i].Start) && (endTime >= planTruckSubTrips[i].End))
                {
                    test = false;
                    break;
                }
                // contains
                else if ((startTime <= planTruckSubTrips[i].Start) && (endTime >= planTruckSubTrips[i].Start))
                {
                    test = false;
                    break;
                }
                // be contained
                else if ((endTime >= planTruckSubTrips[i].Start) && (endTime <= planTruckSubTrips[i].End))
                {
                    test = false;
                    break;
                }
            }
            message = "";
            if (test == false)
            {
                message = TptResourceBLL.ErrTimeIsOverLapped;
            }
            return test;
        }

        public bool CanAddPlanTruckSubTrip(PlanTruckSubTrip planTruckSubTrip)
        {
            bool retValue = true;
            if (planTruckSubTrip.StartStop.Code.ToString().Trim() == planTruckSubTrip.EndStop.Code.ToString().Trim())
            {
                throw new FMException(TptResourceBLL.ErrCantSavePlanSubTripWithSameStop);
            }
            if (planTruckSubTrip.Start > planTruckSubTrip.End)
            {
                throw new FMException(TptResourceBLL.ErrStartTimenEndTime);
            }
            return retValue;
        }

        // 2014-03-27 Zhou Kai renames the parameter from truckJobTripPlan to newTruckJobTripPlan
        public bool IsPlanSubTripOverlapping(TruckJobTripPlan newTruckJobTripPlan)
        {
            bool test = false;
            for (int i = 0; i < planTruckSubTrips.Count; i++)
            {
                if (newTruckJobTripPlan.truckNo == planTruckSubTrips[i].VehicleNumber)
                {
                    if ((newTruckJobTripPlan.start == planTruckSubTrips[i].Start) && (newTruckJobTripPlan.end == planTruckSubTrips[i].End))
                    {
                        test = true;
                        break;
                    }
                    if ((newTruckJobTripPlan.start > planTruckSubTrips[i].Start) && (newTruckJobTripPlan.start < planTruckSubTrips[i].End))
                    {
                        test = true;
                        break;
                    }
                    if ((newTruckJobTripPlan.start <= planTruckSubTrips[i].Start) && (newTruckJobTripPlan.end >= planTruckSubTrips[i].End))
                    {
                        test = true;
                        break;
                    }
                    if ((newTruckJobTripPlan.start <= planTruckSubTrips[i].Start) && (newTruckJobTripPlan.end > planTruckSubTrips[i].Start))
                    {
                        test = true;
                        break;
                    }
                    if ((newTruckJobTripPlan.end > planTruckSubTrips[i].Start) && (newTruckJobTripPlan.end <= planTruckSubTrips[i].End))
                    {
                        test = true;
                        break;
                    }
                }

            }
            return test;
        }

        /*
         * 2014-04-01 Zhou Kai adds comments to this function:
         * This function is to: delete the planTruckSubTripJob from the planTruckSubTrip.
         * Inputs: 
         *     planTruckSubTripJob, the planTruckSubTripJob to be deleted
         *     planTruckSubTrip, the planTruckSbuTrip from which the planTruckSubTripJob is deleted
         *     truckJobTrips, all the truckJobTrips listed in the form: FrmTruckJobTrips
         *     user, the current logged in user
         *     frmName, the Form Name on which this operation is performed
         * Outputs:
         *     (1) the planTruckSubTripJob is deleted from both the planTruckSubTrip in memory and from 
         *           TRK_PLAN_TRUCK_DETAIL_TRIP_JOB_Tbl
         *     (2) a corresponding truckJobTripPlan for the deleted planTruckSubTripJob is deleted from truckJobTrip 
         *           and from TRK_JOB_DETAIL_TRIP_PLAN_Tbl
         *     (3) if after deleting, there're no truckJobTripPlans under the truckJobTrip, set the truckJobTrip to "Ready", 
         *           otherwise, set to "Assigned", and also add the new state to TRK_JOB_TRIP_STATE_Tbl,
         *           and update the STATUS column in TRK_JOB_DETAIL_TRIP_Tbl
         */
        public bool DeletePlanTruckSubTripJob(PlanTruckSubTripJob planTruckSubTripJob, PlanTruckSubTrip planTruckSubTrip,
                                                SortableList<TruckJobTrip> truckJobTrips, string user, string frmName)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {
                string message = "";
                string hour = String.Concat(planTruckSubTripJob.startTime[0], planTruckSubTripJob.startTime[1]);
                string minute = String.Concat(planTruckSubTripJob.startTime[2], planTruckSubTripJob.startTime[3]);

                if (planTruckSubTrip.CanDeletePlanTruckSubTripJob(planTruckSubTripJob, out message))
                {
                    bool bPlanTruckSubTripJobDeleted = false;

                    #region "2014-04-01 Zhou Ka comments out this block"
                    // 201404-01 Zhou Kai:
                    // We already has the planTruckSubTrip where the planTruckSubTripJob is deleted from,
                    // why do we still loop through all the planTruckSubTrips in the loop below? 
                    // 2014-04-01 Zhou Kai comments out the loop below
                    /*
                    foreach (PlanTruckSubTrip tempPlanSubTrip in this.planTruckSubTrips)
                    {
                        // 2014-01-21 Zhou Kai moves deleted out of this loop
                        // bool deleted = false;
                        //loop through plaTruckSubtripJobs
                        foreach (PlanTruckSubTripJob tempPlanSubTripJob in tempPlanSubTrip.planTruckSubTripJobs)
                        {
                            // 2014-01-21 Zhou Kai adds
                            if (deleted) break;
                            // 2014-01-21 Zhou Kai ends
                            if (tempPlanSubTripJob.planTripNo == planTruckSubTripJob.planTripNo
                                && tempPlanSubTripJob.jobID == planTruckSubTripJob.jobID
                                && tempPlanSubTripJob.jobSeq == planTruckSubTripJob.jobSeq)
                            {
                                planTruckSubTrip.DeletePlanTruckSubTripJob(planTruckSubTripJob, con, tran);
                                // 2014-01-21 Zhou Kai moves this line from the end of this function to here:
                                tempPlanSubTrip.planTruckSubTripJobs.Remove(planTruckSubTripJob);
                                // 2014-01-21 Zhou Kai ends
                                deleted = true;
                                break;
                            }
                        }
                    }*/
                    #endregion

                    #region "2014-04-01 Zhou Kai writes new codes to delete the planTruckTripJob from the planTruckSubTrip"
                    foreach (PlanTruckSubTripJob iPlanTruckSubTripJob in planTruckSubTrip.planTruckSubTripJobs)
                    {
                        if (bPlanTruckSubTripJobDeleted) break;
                        if (iPlanTruckSubTripJob.planTripNo == planTruckSubTripJob.planTripNo
                            && iPlanTruckSubTripJob.jobID == planTruckSubTripJob.jobID
                            && iPlanTruckSubTripJob.jobSeq == planTruckSubTripJob.jobSeq)
                        {
                            planTruckSubTrip.DeletePlanTruckSubTripJob(planTruckSubTripJob, con, tran);
                            planTruckSubTrip.planTruckSubTripJobs.Remove(planTruckSubTripJob);
                            // I think when all the planTruckSubTripJobs are deleted, the planTruckSubTrip
                            // should be set to "Ready", but we don't have the "Ready" status for a planTruckSubTrip
                            bPlanTruckSubTripJobDeleted = true;
                            break;
                        }
                    }// end of loop
                    // one and only one rwo must be deleted.
                    if (!bPlanTruckSubTripJobDeleted) { throw new FMException(TptResourceBLL.NoDataIsDeletedAsExpected); }
                    #endregion

                    #region "Delete truckJobTripPlan"
                    //loop throug all truckJobtrips
                    bool bTruckJobTripPlanDeleted = false;
                    foreach (TruckJobTrip iTruckJobTrip in truckJobTrips)
                    {
                        if (iTruckJobTrip.JobID == planTruckSubTripJob.jobID && iTruckJobTrip.Sequence == planTruckSubTripJob.jobSeq)
                        {
                            //loop  throug truckJobtripPlans
                            foreach (TruckJobTripPlan iJobTripPlan in iTruckJobTrip.truckJobTripPlans)
                            {
                                if ((iJobTripPlan.planTripNo == planTruckSubTripJob.planTripNo) &&
                                    (iJobTripPlan.jobID == planTruckSubTripJob.jobID) &&
                                    (iJobTripPlan.sequence == planTruckSubTripJob.jobSeq) &&
                                    (iJobTripPlan.planSubTripSeqNo == planTruckSubTripJob.planSubTripSeqNo) &&
                                    (iJobTripPlan.planSubTripJobSeqNo == planTruckSubTripJob.planSubTripJobSeqNo))
                                {
                                    //delete from TRK_JOB_DETAIL_TRIP_PLAN_TBL and from Memory
                                    iTruckJobTrip.DeleteTruckJobTripPlan(iJobTripPlan, con, tran);
                                    iTruckJobTrip.truckJobTripPlans.Remove(iJobTripPlan);
                                    bTruckJobTripPlanDeleted = true;
                                    //update TRK_JOB_DETAIL_TRIP_Tbl
                                    TruckJobTrip.UpdateAcutalWeightVolumeforTruckJobTrip(iTruckJobTrip.JobID, iTruckJobTrip.Sequence, con, tran);
                                    //get the update version to synchronize when status will be updated
                                    iTruckJobTrip.UpdateVersion = iTruckJobTrip.GetTruckJobTripUpdatedVersion(con, tran);
                                    break;
                                }
                            }// end of inner loop
                            if (bTruckJobTripPlanDeleted)
                            {
                                if (iTruckJobTrip.truckJobTripPlans.Count == 0)
                                {
                                    //update tempJobTrip status back to ready - added 20131206
                                    iTruckJobTrip.TripStatus = JobTripStatus.Ready;
                                    // Create a JobTripState
                                    JobTripState jobTripState = new JobTripState();
                                    jobTripState.Status = JobTripStatus.Ready;
                                    jobTripState.IsNew = true;
                                    jobTripState.StatusDate = DateTime.Today;
                                    // 2014-03-28 Zhou Kai modifies the line below: 
                                    // change the status from Assigned to Ready
                                    // tempJobTrip.SetJobTripStatusForTruck(jobTripState, con, tran, user, frmName, JobTripStatus.Assigned.ToString());
                                    iTruckJobTrip.SetJobTripStatusForTruck(jobTripState, con, tran, user, frmName, JobTripStatus.Ready.ToString());
                                }
                                break;
                            }
                        }
                        #region "Removed"
                        // 2014-01-21 Zhou Kai comments out the if block below
                        //if (deleted)
                        //{
                        //remove from memory
                        // 2014-01-21 Zhou Kai comments out
                        // tempPlanSubTrip.planTruckSubTripJobs.Remove(planTruckSubTripJob);
                        // 2014-01-21 Zhou Kai ends
                        // 2014-01-14 Zhou Kai comments out the line below, 
                        // When the planTruckSubTripJob is not in the first planTruckSubTrip,
                        // this "break" will lead to "bug of planTruckSubTripJob is left not deleted" 
                        //    break;
                        //}
                        #endregion
                    }// end of loop
                    // One and only one truckJobTripPlan must be deleted
                    if (!bTruckJobTripPlanDeleted) { throw new FMException(TptResourceBLL.NoDataIsDeletedAsExpected); }
                } // end of if
                    #endregion

                tran.Commit();
            }
            catch (FMException ex) { tran.Rollback(); throw ex; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
            finally { con.Close(); }

            return true;
        }
        //20131204 - gerry replaced by the method above
        public bool DeletePlanTruckSubTripJobOLD(PlanTruckSubTripJob planTruckSubTripJob,
            PlanTruckSubTrip planTruckSubTrip,
            SortableList<TruckJobTrip> truckJobTrips, string user, string frmName)
        {
            string message = "";
            if (planTruckSubTrip.CanDeletePlanTruckSubTripJob(planTruckSubTripJob, out message))
            {

                string hour = String.Concat(planTruckSubTripJob.startTime[0], planTruckSubTripJob.startTime[1]);
                string minute = String.Concat(planTruckSubTripJob.startTime[2], planTruckSubTripJob.startTime[3]);

                //string hour = planTruckSubTripJob.startTime.Substring(0, planTruckSubTripJob.startTime.IndexOf(":"));
                //hour = (hour.Length < 2) ? hour.Insert(0, "0") : hour;
                //string minute = planTruckSubTripJob.startTime.Substring(planTruckSubTripJob.startTime.IndexOf(":")+1,2);


                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    for (int j = 0; j < this.planTruckSubTrips.Count; j++)
                    {
                        bool deleted = false;
                        if (this.planTruckSubTrips[j].Start >= planTruckSubTripJob.startDate.AddHours(Convert.ToInt32(hour)).AddMinutes(Convert.ToInt32(minute)))
                        {
                            if (this.planTruckSubTrips[j].planTruckSubTripJobs.Count > 0)
                            {
                                for (int i = 0; i < this.planTruckSubTrips[j].planTruckSubTripJobs.Count; i++)
                                {
                                    if ((this.planTruckSubTrips[j].planTruckSubTripJobs[i].jobID == planTruckSubTripJob.jobID)
                                        && (this.planTruckSubTrips[j].planTruckSubTripJobs[i].jobSeq == planTruckSubTripJob.jobSeq))
                                    {
                                        planTruckSubTrip.DeletePlanTruckSubTripJob(planTruckSubTripJob, con, tran);

                                        if (this.planTruckSubTrips[j].planTruckSubTripJobs[i].SetJobTripStatus(JobTripStatus.Ready, con, tran))
                                        {
                                            this.planTruckSubTrips[j].planTruckSubTripJobs.Remove(this.planTruckSubTrips[j].planTruckSubTripJobs[i]);
                                        }
                                        //this.truckSubTrips[j].planTruckSubTripJobs = PlanTruckDAL.GetPlanTruckSubTripJobs(planTruckSubTripJob.planTripNo, planTruckSubTrip);
                                        deleted = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (deleted)
                            break;
                    }

                    //Gerry Removed not in use
                    //double starthour = double.Parse(string.Concat(planTruckSubTripJob.startTime[0], planTruckSubTripJob.startTime[1]));
                    //double startminute = double.Parse(string.Concat(planTruckSubTripJob.startTime[2], planTruckSubTripJob.startTime[3]));
                    //double endhour = double.Parse(string.Concat(planTruckSubTripJob.endTime[0], planTruckSubTripJob.endTime[1]));
                    //double endminute = double.Parse(string.Concat(planTruckSubTripJob.endTime[2], planTruckSubTripJob.endTime[3]));
                    //end

                    for (int j = 0; j < truckJobTrips.Count; j++)
                    {
                        if ((truckJobTrips[j].JobID == planTruckSubTripJob.jobID) &&
                            (truckJobTrips[j].Sequence == planTruckSubTripJob.jobSeq))
                        {

                            for (int i = 0; i < truckJobTrips[j].truckJobTripPlans.Count; i++)
                            {
                                if ((truckJobTrips[j].truckJobTripPlans[i].jobID == planTruckSubTripJob.jobID) &&
                                    (truckJobTrips[j].truckJobTripPlans[i].planTripNo == planTruckSubTripJob.planTripNo) &&
                                    (truckJobTrips[j].truckJobTripPlans[i].planSubTripJobSeqNo == planTruckSubTripJob.planSubTripJobSeqNo))

                                //  Line below is wrong - the 2 sequences are not equal 
                                //  (truckJobTrips[j].truckJobTripPlans[i].sequence == planTruckSubTripJob.jobSeq))
                                {
                                    truckJobTrips[j].DeleteTruckJobTripPlan(truckJobTrips[j].truckJobTripPlans[i], con, tran);
                                    truckJobTrips[j].truckJobTripPlans.Remove(truckJobTrips[j].truckJobTripPlans[i]);
                                    // 2013-11-18 Zhou  Kai adds, also update actual weight/volume while deleting a truck job trip job.
                                    TruckJobTrip.UpdateAcutalWeightVolumeforTruckJobTrip(truckJobTrips[j].JobID, truckJobTrips[j].Sequence, con, tran);
                                    // 2013-11-18 Zhou Kai ends

                                    if (truckJobTrips[j].truckJobTripPlans.Count == 0)
                                    {
                                        JobTripState newJobTripState = new JobTripState();
                                        newJobTripState.Status = JobTripStatus.Ready;
                                        newJobTripState.IsNew = true;
                                        newJobTripState.StatusDate = DateTime.Today;

                                        truckJobTrips[j].SetJobTripStatusForTruck(newJobTripState, con, tran, user, frmName, truckJobTrips[j].TripStatus.ToString());
                                    }

                                    break;
                                }
                            }
                            break;
                        }
                    }

                    // Not sure what is this for loop
                    // There is only 1 instance of TruckJobTripPlan we need to delete from the collection in TruckJobTrip
                    /* 
                    for (int i = 0; i < truckJobTrips.Count; i++)
                    {
                        if ((truckJobTrips[i].JobID == planTruckSubTripJob.jobID) &&
                            (truckJobTrips[i].Sequence == planTruckSubTripJob.jobSeq))
                        {
                            for (int j = 0; j < truckJobTrips[i].truckJobTripPlans.Count; j++)
                            {
                                if ((planTruckSubTripJob.planTripNo == truckJobTrips[i].truckJobTripPlans[j].planTripNo) &&
                                    (planTruckSubTrip.DriverNumber == truckJobTrips[i].truckJobTripPlans[j].driver) &&
                                    (planTruckSubTrip.VehicleNumber == truckJobTrips[i].truckJobTripPlans[j].truckNo))
                                {
                                    truckJobTrips[i].truckJobTripPlans.Remove(truckJobTrips[i].truckJobTripPlans[j]);

                                }
                            }
                        }
                    }
                    */
                    tran.Commit();
                }
                catch (FMException ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
                return true;
            }
            else
            {
                return false;
                throw new FMException(message);
            }
        }

        public bool CanDeletePlanTruckSubTrip(PlanTruckSubTrip planTruckSubTrip, out string message)
        {
            bool flag = true;
            message = "";
            if (planTruckSubTrip.planTruckSubTripJobs.Count > 0)
            {
                flag = false;
                message = message + TptResourceBLL.ErrCantDeleteBecauseThereareJobsAllocated;
            }

            if (planTruckSubTrip.Status == JobTripStatus.Completed)
            {
                flag = false;
                message = message + TptResourceBLL.ErrCantDeleteCompletedOrInvoicedSubTrip;
            }
            return flag;
        }

        public bool CanDeletePlanTruckSubTripForTransferDriver(PlanTruckSubTrip planTruckSubTrip, out string message)
        {
            bool flag = true;
            message = "";

            if (planTruckSubTrip.Status == JobTripStatus.Completed)
            {
                flag = false;
                message = message + TptResourceBLL.ErrCantDeleteCompletedOrInvoicedSubTrip;
            }
            return flag;
        }

        /*
        // Logic in this method is wrong - delete
        public PlanTruckSubTrip GetPlanTruckSubTripForTransfer(DateTime startTime,DateTime endTime)
        {
            PlanTruckSubTrip p = new PlanTruckSubTrip();

            DateTime tempEndTime = endTime.Date;

            for (int i = 0; i < this.truckSubTrips.Count; i++)
            {
                if (DateTime.Compare(this.truckSubTrips[i].End, startTime) < 0)
                {
                    if (DateTime.Compare(this.truckSubTrips[i].End, tempEndTime) > 0)
                    {
                        tempEndTime = this.truckSubTrips[i].End;
                        p = this.truckSubTrips[i];
                    }
                }
            }

            return p;
        }
        */
        public int GetPrecedingPlanTruckSubTripIndex(DateTime startTime, DateTime endTime)
        {
            int planTruckSubTripIndex = -1;

            // first sort the collection by start time
            this.pPlanTruckSubTrips.SortByStartTime();

            // if startTime is earlier than the first PlanTruckSubTrip return -1
            if (DateTime.Compare(startTime, pPlanTruckSubTrips[0].Start) < 0)
            {
                return -1;
            }
            // if startTime is after the last PlanTruckSubTrip return Count -1
            if (DateTime.Compare(startTime, pPlanTruckSubTrips[pPlanTruckSubTrips.Count - 1].End) > 0)
            {
                return (pPlanTruckSubTrips.Count - 1);
            }
            // if the startTime is in between the first and last planTruckSubTrip
            // get the index by comparing with two consecutive planTruckSubTrips
            bool found = false;

            //24 Feb - Gerry Replaced the loop
            //for (int i = 0; i < this.TruckSubTrips.Count; i++)
            for (int i = 0; i < this.pPlanTruckSubTrips.Count; i++)
            {
                if (DateTime.Compare(startTime, pPlanTruckSubTrips[i].End) > 0)
                {
                    if (DateTime.Compare(endTime, pPlanTruckSubTrips[i + 1].Start) < 0)
                    {
                        planTruckSubTripIndex = i;
                        break;
                    }
                }
            }
            return planTruckSubTripIndex;
        }

        // 2014-03-27 Zhou Kai adds comments to this function:
        /*
         * inputs: 
         *        planTruckTrips, all the existing planTruckTrips on the scheduler
         *        oldPlanTruckTrip, the planTruckTrip which is going to changing driver
         *        newDriver, the driver which the oldPlanTruckTrip is going to change to
         *        startTime, the new startTime after the oldPlanTruckTrip is changed to another planTruckTrip
         *        endTime, the new endTime after the oldPlanTruckTrip is changed to another planTruckTrip
         *        message, error message
         *        frmName, the form name where this operation is performed on
         *        user, the current loggin user 
         */
        public static bool ChangeDriver(SortableList<PlanTruckTrip> planTruckTrips,
                                                        PlanTruckTrip oldPlanTruckTrip,
                                                        PlanTruckSubTrip oldPlanTruckSubTrip,
                                                        Driver newDriver,
                                                        DateTime startTime,
                                                        DateTime endTime,
                                                        out string message,
                                                       string frmName,
                                                       string user)
        {
            PlanTruckTrip newPlanTruckTrip = new PlanTruckTrip();
            bool change = false;
            if (oldPlanTruckSubTrip.CanChangeDriver(startTime, endTime, out message) /*== true*/)
            {
                if (planTruckTrips.Count > 0)
                {
                    for (int i = 0; i < planTruckTrips.Count; i++)
                    {
                        if (planTruckTrips[i].Driver.Code == newDriver.Code)
                        {
                            newPlanTruckTrip = planTruckTrips[i];
                            break;
                        }
                    }
                }
                if (newPlanTruckTrip.ValidateTime(/*newDriver.Code,*/ startTime, endTime, out message) /*== true*/)
                {
                    SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    try
                    {
                        PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
                        #region Removed
                        /*
                        newPlanTruckSubTrip = (PlanTruckSubTrip)oldPlanTruckSubTrip.Clone();
                        // Chong Chin 25 Jan 12, Start, Should use startTime, endTime instead
                        //newPlanTruckSubTrip.Start = oldPlanTruckSubTrip.Start;
                        //newPlanTruckSubTrip.End = oldPlanTruckSubTrip.End;
                        newPlanTruckSubTrip.Start = startTime;
                        newPlanTruckSubTrip.End = endTime;
                        
                        //Chong Chin 25 Jan 12, End
                        newPlanTruckSubTrip.StartStop = oldPlanTruckSubTrip.StartStop;
                        newPlanTruckSubTrip.EndStop = oldPlanTruckSubTrip.EndStop;
                        newPlanTruckSubTrip.IsBillableTrip = oldPlanTruckSubTrip.IsBillableTrip;
                        newPlanTruckSubTrip.Description = oldPlanTruckSubTrip.Description;
                        newPlanTruckSubTrip.VehicleNumber = newDriver.defaultVehicleNumber;
                        newPlanTruckSubTrip.DriverNumber = newDriver.Code;
                        newPlanTruckSubTrip.Status = oldPlanTruckSubTrip.Status;

                        //newPlanTruckSubTrip.SeqNo = newPlanTruckTrip.truckSubTrips.Count + 1;
                        newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(newPlanTruckTrip.PlanTripNo.ToString().Trim());

                        // Chong Chin 27 Jan 12. Start. Wrong logic. Instead add the collection of TruckJobs
                        // from oldPlanTruckSubTrip to newPlanTruckSubTrip
                        //
                        for (int i = 0; i < newPlanTruckSubTrip.planTruckSubTripJobs.Count; i++)
                        {
                            newPlanTruckSubTrip.planTruckSubTripJobs[i].planTripNo = newPlanTruckTrip.PlanTripNo;
                        }
                        //
                        for (int i = 0; i < oldPlanTruckSubTrip.planTruckSubTripJobs.Count; i++)
                        {
                            //no need to add because the collection of planTruckSubTripJobs ware carried out from the oldPlanTruckSubTrip
                            //if we add it create duplicate planTruckSubTripJob
                            //newPlanTruckSubTrip.planTruckSubTripJobs.Add(oldPlanTruckSubTrip.planTruckSubTripJobs[i]);
                            newPlanTruckSubTrip.planTruckSubTripJobs[i].planTripNo = newPlanTruckTrip.PlanTripNo;
                            newPlanTruckSubTrip.planTruckSubTripJobs[i].planTripSeq = newPlanTruckSubTrip.SeqNo;
                        }   
                        // Chong Chin 27 

                        // Chong Chin 24 Jan 12 Start - move the following 2 lines to after getting the index of the
                        // preceding planTruckSubTrip
                        //newPlanTruckTrip.AddPlanTruckSubTrip(newPlanTruckSubTrip, frmName, user);
                        //newPlanTruckTrip.truckSubTrips.Add(newPlanTruckSubTrip);
                                                
                        PlanTruckSubTrip preceedingPlanTruckSubTrip = new PlanTruckSubTrip();
                        // 24 Jan 12 method called in following line has wrong logic
                       // preceedingPlanTruckSubTrip = newPlanTruckTrip.GetPlanTruckSubTripForTransfer(startTime, endTime);
                        // 24 Jan 12, instead use the new method to get the new index
                        int indexPrecedingTruckSubTrip = -1;
                        if (newPlanTruckTrip.TruckSubTrips.Count != 0)
                        {
                            indexPrecedingTruckSubTrip = newPlanTruckTrip.GetPrecedingPlanTruckSubTripIndex(startTime, endTime);
                            // Chong Chin 25 Jan 12, Start  - following line is wrong. Should not assign for case of -1
                            //preceedingPlanTruckSubTrip = newPlanTruckTrip.TruckSubTrips[indexPrecedingTruckSubTrip];
                            if (indexPrecedingTruckSubTrip >= 0)
                            {
                                preceedingPlanTruckSubTrip = newPlanTruckTrip.TruckSubTrips[indexPrecedingTruckSubTrip];
                                
                            
                            }
                            // Chong Chin 25 Jan 12, End
                        }
                       // Now after getting the index of existing collection, add newPlanTruckTrip into database and collection
                       // AddPlanTruckSubTrip method should pass in connection and transaction
                        newPlanTruckTrip.AddPlanTruckSubTrip(newPlanTruckSubTrip, frmName, user,con,tran);
                        newPlanTruckTrip.truckSubTrips.Add(newPlanTruckSubTrip);
                        // Use the new name of the method and call this only if index <> -1
                        if (newPlanTruckTrip.TruckSubTrips.Count !=0 & indexPrecedingTruckSubTrip != -1)
                            {
                            PlanTruckSubTrip.TransferJobsFromPrecedingPlanTruckSubTrip(preceedingPlanTruckSubTrip, newPlanTruckTrip, newPlanTruckSubTrip, con, tran);
                            }
                        // Chong Chin 24 Jan 12 End of Change


                        */
                        #endregion
                        newPlanTruckSubTrip = (PlanTruckSubTrip)oldPlanTruckSubTrip.Clone();
                        //Change the 4 properties which is set in change driver form
                        newPlanTruckSubTrip.Start = startTime;
                        newPlanTruckSubTrip.End = endTime;
                        newPlanTruckSubTrip.VehicleNumber = newDriver.defaultVehicleNumber;
                        newPlanTruckSubTrip.DriverNumber = newDriver.Code;
                        //make sure to use next planTruckSubTrip seqNo 
                        newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(newPlanTruckTrip.PlanTripNo.ToString().Trim(), con, tran);

                        TruckJobTripPlan truckJobTripPlan = new TruckJobTripPlan();
                        for (int i = 0; i < oldPlanTruckSubTrip.planTruckSubTripJobs.Count; i++) // don't understand this loop, Zhou Kai, 2014-03-27
                        {
                            #region Get TruckJobTripPlan
                            truckJobTripPlan = TruckJobTripPlan.GetTruckJobTripPlan(
                                                                    oldPlanTruckSubTrip.planTruckSubTripJobs[i].jobID,
                                                                    oldPlanTruckSubTrip.planTruckSubTripJobs[i].jobSeq,
                                                                    oldPlanTruckSubTrip.planTruckSubTripJobs[i].planTripNo,
                                                                    oldPlanTruckSubTrip.planTruckSubTripJobs[i].planSubTripSeqNo,
                                                                    oldPlanTruckSubTrip.planTruckSubTripJobs[i].planSubTripJobSeqNo,
                                                                    con, tran);
                            #endregion
                            //assign planTruckSubTripJob planTripNo and planTruckSubTripJob planTripSeq
                            newPlanTruckSubTrip.planTruckSubTripJobs[i].planTripNo = newPlanTruckTrip.PlanTripNo;
                            newPlanTruckSubTrip.planTruckSubTripJobs[i].planSubTripSeqNo = newPlanTruckSubTrip.SeqNo;
                            //newPlanTruckSubTrip.planTruckSubTripJobs[i].volume = oldPlanTruckSubTrip.planTruckSubTripJobs[i].volume;
                            //newPlanTruckSubTrip.planTruckSubTripJobs[i].weight = oldPlanTruckSubTrip.planTruckSubTripJobs[i].weight;
                        }

                        #region "Delete and Add TruckJobTripPlan"
                        // 2014-03-27 Zhou Kai adds comments:
                        // 2014-03-27, since this loop is to delete each TruckJobTripPlan from database,
                        // and then add each TruckJobTripPlan back into database, why not using
                        // "UPDATE" statement instead?
                        if (newPlanTruckSubTrip.planTruckSubTripJobs.Count > 0)
                        {
                            for (int i = 0; i < oldPlanTruckSubTrip.planTruckSubTripJobs.Count; i++)
                            {
                                // 2014-03-27 Zhou Kai: the line below reads database only for getting a TruckJobTrip object
                                // and thus to use its DeleteTruckJobTripPlan() function.
                                // However, we can use JobId and JobTripSeqNo to delete a truckJobTripPlan.
                                TruckJobTrip jt = TruckJobTrip.GetTruckJobTrip(oldPlanTruckSubTrip.planTruckSubTripJobs[i].jobID,
                                                                                                           oldPlanTruckSubTrip.planTruckSubTripJobs[i].jobSeq);
                                //delete TruckJobTripPlan
                                jt.DeleteTruckJobTripPlan(truckJobTripPlan, con, tran);

                                //add TruckJobTripPlan
                                truckJobTripPlan.planTripNo = newPlanTruckSubTrip.planTruckSubTripJobs[i].planTripNo;
                                truckJobTripPlan.driver = newPlanTruckTrip.Driver.Code.ToString();
                                truckJobTripPlan.start = newPlanTruckSubTrip.planTruckSubTripJobs[i].startDate.AddHours(startTime.Hour).AddMinutes(startTime.Minute);
                                truckJobTripPlan.end = newPlanTruckSubTrip.planTruckSubTripJobs[i].endDate.AddHours(endTime.Hour).AddMinutes(endTime.Minute);
                                truckJobTripPlan.volume = newPlanTruckSubTrip.planTruckSubTripJobs[i].volume;
                                truckJobTripPlan.weight = newPlanTruckSubTrip.planTruckSubTripJobs[i].weight;
                                // 2014-03-27 Zhou Kai adds
                                truckJobTripPlan.truckNo = newPlanTruckSubTrip.VehicleNumber;
                                // 2014-03-27 Zhou Kai ends

                                jt.truckJobTripPlans.Add(truckJobTripPlan);
                                TruckJobTripPlan outTruckJobTripPlan = new TruckJobTripPlan();
                                // 2014-03-27 Zhou Kai adds comments: add the truckJobTripPlan with updated value into database again.
                                jt.AddTruckJobTripPlan(truckJobTripPlan, con, tran, out outTruckJobTripPlan, user, frmName);

                                //assign planTruckSubTripJob jobTripPlanSeq
                                newPlanTruckSubTrip.planTruckSubTripJobs[i].planSubTripJobSeqNo = outTruckJobTripPlan.planSubTripJobSeqNo;
                            }
                        }
                        #endregion

                        //I'm not sure what is this for    
                        #region Removed
                        //for (int i = 0; i < newPlanTruckSubTrip.planTruckSubTripJobs.Count; i++)
                        //{
                        //    TruckJobTripPlan.ChangeDriver(oldPlanTruckTrip.PlanTripNo, newPlanTruckSubTrip.planTruckSubTripJobs[i].jobID, newPlanTruckSubTrip.planTruckSubTripJobs[i].jobSeq,
                        //        newPlanTruckSubTrip.planTruckSubTripJobs[i].jobTripPlanSeq, newPlanTruckTrip.PlanTripNo, newDriver, con, tran);
                        //}
                        #endregion

                        //here we delete oldPlanTruckSubTrip, planTruckSubTripJob and truckJobTripPlan 
                        oldPlanTruckTrip.DeletePlanTruckSubTripForTransferDriver(oldPlanTruckSubTrip, con, tran);

                        //Now add new planTruckSubTrip
                        //and cater preceeding planTruckSubTrip
                        newPlanTruckSubTrip.AddPlanTruckSubTrip(newPlanTruckTrip, con, tran, frmName, user);

                        change = true;
                        tran.Commit();
                    }
                    catch (FMException ex)
                    {
                        change = false;
                        tran.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        change = false;
                        tran.Rollback();
                        throw new FMException(ex.ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                    //return true;

                    //else
                    //{
                    //    return false;
                    //}
                }
            }
            //else
            //{
            //    return false;
            //}
            return change;
        }

        public bool DeletePlanTruckSubTrip(PlanTruckSubTrip planTruckSubTrip)
        {

            string message = "";
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    if (CanDeletePlanTruckSubTrip(planTruckSubTrip, out message))
                    {
                        PlanTruckDAL.DeletePlanTruckSubTrip(this, planTruckSubTrip, con, tran);
                        this.pPlanTruckSubTrips.Remove(planTruckSubTrip);
                        tran.Commit();
                        return true;
                    }
                    else
                    {
                        throw new FMException(message);
                        return false;
                    }
                }
                catch (FMException ex)
                {
                    tran.Rollback();
                    throw;
                    return false;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                    return false;
                }
            }
        }
        public bool DeletePlanTruckSubTrip(PlanTruckSubTrip planTruckSubTrip, SqlConnection con, SqlTransaction tran)
        {

            string message = "";
            try
            {
                if (CanDeletePlanTruckSubTrip(planTruckSubTrip, out message))
                {
                    PlanTruckDAL.DeletePlanTruckSubTrip(this, planTruckSubTrip, con, tran);
                    this.pPlanTruckSubTrips.Remove(planTruckSubTrip);

                    return true;
                }
                else
                {
                    throw new FMException(message);
                    return false;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        public bool DeletePlanTruckSubTripForTransferDriver(PlanTruckSubTrip planTruckSubTrip, SqlConnection con, SqlTransaction tran)
        {
            string message = "";
            if (CanDeletePlanTruckSubTripForTransferDriver(planTruckSubTrip, out message))
            {
                try
                {
                    PlanTruckDAL.DeletePlanTruckSubTripForTransferDriverDatabase(this, planTruckSubTrip, con, tran);
                    this.pPlanTruckSubTrips.Remove(planTruckSubTrip);
                }
                catch (FMException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
            else
            {
                throw new FMException(message);
                return false;
            }
        }


        public void CopyPlanSubTripsToOldPlanSubTrips()
        {
            oldPlanTruckSubTrips = null;
            oldPlanTruckSubTrips = new PlanTruckSubTrips();
            foreach (PlanTruckSubTrip planTruckSubTrip in planTruckSubTrips)
            {
                PlanTruckSubTrip oldPlanTruckSubTrip = (PlanTruckSubTrip)planTruckSubTrip.Clone();
                oldPlanTruckSubTrips.Add(oldPlanTruckSubTrip);
            }
        }

        public void DeleteAllPlanTruckTripDatabase(SqlConnection cn, SqlTransaction tran)
        {
            PlanTruckDAL.DeleteAllPlanTruckTripDatabase(this, cn, tran);
        }

        public void AddPlanTruckTripDatabase(SqlConnection cn, SqlTransaction tran)
        {

            PlanTruckTrip outPlanTruckTrip = new PlanTruckTrip();

            PlanTruckDAL.AddPlanTruckTripDatabase(this, cn, tran, out outPlanTruckTrip);
        }

        public static SortableList<PlanTruckTrip> GetAllPlanTruckTripByDayAndDept(DateTime date, string tptDept)
        {
            return PlanTruckDAL.GetAllPlanTruckTripByDayAndDept(date, tptDept);
        }
        public SortableList<PlanTruckTrip> GetAllPlanTruckTrips(DateTime date, string tptDept)
        {
            return PlanTruckDAL.GetAllPlanTruckTripByDayAndDept(date, tptDept);
        }
        public static bool IsPlanDateDuplicate(DateTime date, string tptDept)
        {
            return PlanTruckDAL.IsPlanDateDuplicate(date, tptDept);
        }

        public static bool AddAllPlanTruckTrips(SortableList<PlanTruckTrip> planTruckTrips)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            bool added = true;
            //23 Mar 2011 - Gerry Added if planTruckTrips.Count > 0
            //because when you cancel new Plan the planTruckTrips returns 0 collection and it causes error.
            if (planTruckTrips.Count > 0)
            {
                try
                {
                    DateTime plandate = planTruckTrips[0].ScheduleDate;

                    for (int i = 0; i < planTruckTrips.Count; i++)
                    {
                        planTruckTrips[i].DeleteAllPlanTruckTripDatabase(con, tran);
                    }


                    for (int i = 0; i < planTruckTrips.Count; i++)
                    {
                        planTruckTrips[i].AddPlanTruckTripDatabase(con, tran);
                    }

                    foreach (PlanTruckTrip planTruckTrip in planTruckTrips)
                    {
                        planTruckTrip.CopyPlanSubTripsToOldPlanSubTrips();
                    }

                    tran.Commit();
                    added = true;
                }
                catch (FMException ex)
                {
                    added = false;
                    tran.Rollback();
                    throw ex;

                }
                catch (Exception ex)
                {
                    added = false;
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    con.Close();
                }
            }
            return added;

        }

        #region "2013-06-11 new logic for adding routed plan sub trips to existing plan sub trips"

        public static bool IsTimeValid(DateTime planStart, DateTime planEnd,
                                         DateTime existStart, DateTime existEnd,
                                         out string feedback)
        {
            //2013-06-06 zhou kai adds this function
            //to verify if two duration is overlapped.
            //Return true if there're no time over laps
            //Return false if there're time over lpas
            if (planStart >= planEnd)
            {
                //invalid input
                feedback = @"invalid input, start time is later than end time.";
                return false;
            }
            if (planStart >= existStart && planStart < existEnd)
            {
                //intersection
                feedback = @"time overlaps with existing time gap";
                return false;
            }

            if (planEnd > existStart && planEnd <= existEnd)
            {
                //intersection
                feedback = @"time overlaps with existing time gap";
                return false;
            }
            if (planStart <= existStart && planEnd >= existEnd)
            {
                //contains
                feedback = @"existing time gap is contained by plan time gap";
                return false;
            }
            if (planStart >= existStart && planEnd <= existEnd)
            {
                //contains
                feedback = @"plan time gap is contained by existing time gap";
                return false;
            }

            feedback = @"there's no time overlaps.";
            return true;
            //2013-07-01 ends
        }

        public static bool IsPlanRoutedTimesValid(List<DateTime> startTimes,
                                         List<DateTime> endTimes,
                                         out string errMsg)
        {
            //2013-06-07 zhou kai adds function to 
            //check if a combination of start time list
            //and end time list is valid.
            //return true means there're no time overlaps,
            //return false means there're time overlaps.
            //errMsg will contain the overlap information
            //if there is.
            //2013-06-24 ends

            errMsg = String.Empty;

            if ((startTimes.Count == endTimes.Count) && (startTimes.Count != 0))
            {
                //check if each start time is earlier than end time
                for (int i = 0; i < startTimes.Count; i++)
                {
                    if (startTimes[i] >= endTimes[i])
                    {

                        errMsg += TptResourceUI.PlanSubTrip + " #" + (i + 1).ToString() + " start time: " + startTimes[i].ToString()
                                 + ", is later than end time: " + endTimes[i].ToString() + ".\n";
                    }
                }

                //check if there's overlap within plan durations
                for (int i = 0; i < startTimes.Count - 1; i++)
                {
                    if (Convert.ToDateTime(endTimes[i]) >=
                        Convert.ToDateTime(startTimes[i + 1]))
                    {
                        errMsg += TptResourceUI.TimeOverLapBetween + " #" + (i + 1).ToString() +
                                  TptResourceUI.And + " #" + (i + 2).ToString() + " " + TptResourceUI.PlanSubTrip + "\n";
                    }
                }
            }

            // Chong Chin 30/6/13, return false only if there are errMsg.Length = 0
            if (errMsg.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Int32 GetTimeGap(List<DateTime> startTimes, List<DateTime> endTimes,
                                        out DateTime gapStart, out DateTime gapEnd)
        {
            //2013-06-06 zhou kai adds this function to
            //get time gap from a list of start times
            //and end times

            string errMsg = String.Empty;
            gapStart = Convert.ToDateTime("3000-01-01 00:00:00");
            gapEnd = Convert.ToDateTime("1900-01-01 00:00:00");

            for (int i = 0; i < startTimes.Count; i++)
            {
                if (gapStart > startTimes[i])
                {
                    gapStart = startTimes[i];
                }
                if (gapEnd < endTimes[i])
                {
                    gapEnd = endTimes[i];
                }
            }

            return startTimes.Count; //return positive means we get the time gap successfully

        }

        public bool CanAddPlanSubTripByRoute(List<Stop> startStops, List<Stop> endStops,
                                             List<DateTime> startTimes, List<DateTime> endTimes,
                                             out string feedback)
        {
            //2013-06-06 zhou kai adds function to 
            //verify if a routed plan sub trips is
            //valid to add to the existing plan sub trips.
            string feedbackByTimes = String.Empty;
            string feedbackByStops = String.Empty;
            feedback = String.Empty;
            bool bPlanRoutedTimesValid = false;
            bool bPlanRoutedStopsValid = false;

            bPlanRoutedTimesValid = IsPlanRoutedTimesValid(startTimes, endTimes, out feedbackByTimes);
            if (bPlanRoutedTimesValid)
            {
                bPlanRoutedStopsValid = IsRoutedPlanSubTripStopsValid(startStops, endStops, out feedbackByStops);
                if (!bPlanRoutedStopsValid)
                {
                    feedback = feedbackByStops;
                    return false;
                }
            }
            else
            {
                feedback = feedbackByTimes;
                return false;
            }

            int position = 0;
            DateTime routedPlanDurStart = new DateTime();
            DateTime routedPlanDurEnd = new DateTime();
            bool bAreInsertableByTime = false;

            //sort all the existing plan truck sub trips by plan start date time

            List<PlanTruckSubTrip> sortedPlanTruckTrips = GetSortedPlanTruckSubTripByStartDateTime();
            //get the earliest start time and latest end time of the plan route trip
            GetTimeGap(startTimes, endTimes, out routedPlanDurStart, out routedPlanDurEnd);
            List<PlanTruckSubTrip> existingPlanTruckSubTrips = new List<PlanTruckSubTrip>();
            foreach (PlanTruckSubTrip ptst in planTruckSubTrips)
            {
                existingPlanTruckSubTrips.Add(ptst);
            }
            bAreInsertableByTime = IsPlanRoutedSubTripsInsertableByTime(routedPlanDurStart, routedPlanDurEnd,
                                                      existingPlanTruckSubTrips, out position, out feedbackByTimes);

            feedback += feedbackByTimes;

            if (bAreInsertableByTime)
            {
                return true;
            }
            else
            {
                return false;
            }
            //2013-07-01 ends
        }

        //2013-06-11 zhou kai adds function to check if we can
        //insert this plan routed truck sub trips into the 
        //time span intervals of the existing plan truck sub trips.
        //position is the index in the sortedPlanTruckSubTrips
        //where we insert the routed plan sub trip
        public static bool IsPlanRoutedSubTripsInsertableByTime(DateTime durRoutedPlanStart, DateTime durRoutedPlanEnd,
                                                                 List<PlanTruckSubTrip> sortedPlanTruckSubTrips,
                                                                 out Int32 position, out string feedback)
        {
            Int32 nPlanTruckSubTripsCount = sortedPlanTruckSubTrips.Count;
            if (nPlanTruckSubTripsCount == 0)
            {
                //there're no existing plan sub trips
                position = -1;
                feedback = String.Empty;
                return true;
            }
            if (sortedPlanTruckSubTrips[0].Start >= durRoutedPlanEnd)
            {
                //insert before all the existing plan sub trips
                position = 0;
                feedback = String.Empty;
                return true;
            }
            if (sortedPlanTruckSubTrips[nPlanTruckSubTripsCount - 1].End <= durRoutedPlanStart)
            {
                //insert after all the existing plan sub trips
                position = nPlanTruckSubTripsCount;
                feedback = String.Empty;
                return true;
            }

            for (Int32 i = 0; i < nPlanTruckSubTripsCount - 1; i++)
            {
                //insert between two existing plan sub trips
                if (sortedPlanTruckSubTrips[i].End <= durRoutedPlanStart &&
                    sortedPlanTruckSubTrips[i + 1].Start >= durRoutedPlanEnd)
                {
                    position = i;
                    feedback = String.Empty;
                    return true;
                }
            }

            position = -1; //means we can't insert
            feedback = TptResourceBLL.ErrCantInsertRoutedTrip + ", the plan route overlaps with an existing plan sub trip." +
                " Please correct.";
            return false;  //means we can't insert
            //2013-06-11 ends
        }

        //2013-06-11 zhou kai adds function to check if we can
        //insert this plan routed truck sub trips into the 
        //the existing plan truck sub trips, considering 
        //about the stops.
        //
        //This function is NOT USED NOW
        private static bool CheckInsertableByStops(List<Stop> routedStartStops,
                                                   List<Stop> routedEndStops,
                                                   List<PlanTruckSubTrip> sortedPlanTruckSubTrips,
                                                   Int32 position,
                                                   out string feedback)
        {
            Int32 nExistPlanTruckSubTrips = sortedPlanTruckSubTrips.Count;
            Int32 nPlanTruckSubTripStops = routedStartStops.Count;

            if (position == 0)
            {
                if (routedEndStops[nPlanTruckSubTripStops - 1].Equals(
                    sortedPlanTruckSubTrips[0].StartStop.ToString()))
                {
                    //insert before all the existing plan sub trips
                    feedback = String.Empty;
                    return true;
                }
            }
            if (position == sortedPlanTruckSubTrips.Count)
            {
                //insert after all the existing plan sub trips
                if (routedStartStops[0].Equals(
                    sortedPlanTruckSubTrips[nExistPlanTruckSubTrips - 1].EndStop.ToString()))
                {
                    feedback = String.Empty;
                    return true;
                }
            }
            if (routedStartStops[0].Equals(sortedPlanTruckSubTrips[position - 1].EndStop.ToString()) &&
                routedEndStops[nPlanTruckSubTripStops - 1].Equals(sortedPlanTruckSubTrips[position].StartStop.ToString()))
            {
                //insert between two existing plan sub trips
                feedback = String.Empty;
                return true;
            }

            feedback = @"We can't insert this routed plan sub trip because its start and end stop do not match any of " +
                        "the existing plan sub trips.";

            return false;
            //2013-06-11 ends
        }

        //2013-06-13 zhou kai adds function to check if the the preceding
        //plan sub trip have plan sub trip jobs whose time gap 
        //overlaps with the routed plan sub trips.
        //This function is NOT USED NOW, because the start
        //time and end time of the plan sub trip jobs is from
        //booking module, they're not as accurate, so we can't
        //depend on it. 
        //Instead, we just transit the unfinished
        //plan sub trip jobs of the preceding plan trip to 
        //every plan sub trip in the new routed plan sub trips.
        //We perform this transit in another function, not here.
        private static bool CheckInsertableByExistSubTripJobs(List<PlanTruckSubTrip> sortedPlanTruckSubTrips,
                                                              Int32 position,
                                                              DateTime routedTimeGapStart,
                                                              DateTime routedTimeGapEnd,
                                                              out string feedback)
        {
            bool bRtn = false;
            bool tmp = false;
            DateTime jobsTimeGapStart = new DateTime();
            DateTime jobsTimeGapEnd = new DateTime();
            feedback = String.Empty;
            SortableList<PlanTruckSubTripJob> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();
            PlanTruckSubTrip precedingPlanSubTrip = sortedPlanTruckSubTrips[position - 1];
            planTruckSubTripJobs = precedingPlanSubTrip.planTruckSubTripJobs;

            tmp = GetTimeGapFromPlanTruckSubTripJobs(planTruckSubTripJobs, out jobsTimeGapStart, out jobsTimeGapEnd);
            bRtn = IsTimeValid(routedTimeGapStart, routedTimeGapEnd, jobsTimeGapStart, jobsTimeGapEnd, out feedback);

            if (!bRtn)
            {
                return false;
            }

            return bRtn;

            /*
             * 2013-06-18 zhou kai comments out this loop
             * we don't need to loop here.
             * we only need to check the plan sub trip jobs
             * of the preceding plan sub trip whoes
             * position is position - 1
             * 
            for (int i = 0; i < position; i++)
            {
                planTruckSubTripJobs = sortedPlanTruckSubTrips[i].planTruckSubTripJobs;
                tmp = GetTimeGapFromPlanTruckSubTripJobs(planTruckSubTripJobs, out jobsTimeGapStart, out jobsTimeGapEnd);
                bRtn = IsTimeOverLap(routedTimeGapStart, routedTimeGapEnd, jobsTimeGapStart, jobsTimeGapEnd, out feedback);
                if (!bRtn)
                {
                    return false;
                }
            }
            
            return bRtn;
            */

            //2013-06-18 ends
        }

        private static bool GetTimeGapFromPlanTruckSubTripJobs(SortableList<PlanTruckSubTripJob> planTruckSubTripJobs,
                                                               out DateTime gapStart, out DateTime gapEnd)
        {
            //2013-06-13 zhou kai adds function to 
            //get the time gap of the plan sub trip jobs
            //of a plan sub trip.
            //This function is NOT USED NOW.
            //And it's NOT TESTED.
            //The time gap is the earliest start time
            //and the latest end time of all the plan
            //sub trip jobs in a plan sub trip.

            bool bRtn = false;
            Int32 nCount = 0;
            List<DateTime> startTimes = new List<DateTime>();
            List<DateTime> endTimes = new List<DateTime>();

            foreach (PlanTruckSubTripJob ptst in planTruckSubTripJobs)
            {
                startTimes.Add(ptst.startDate);
                endTimes.Add(ptst.endDate);
            }
            nCount = GetTimeGap(startTimes, endTimes, out gapStart, out gapEnd);
            if (nCount > 0)
            {
                bRtn = true;
            }
            else
            {
                bRtn = false;
            }

            return bRtn;
            //2013-06-13 ends
        }


        public static bool IsRoutedPlanSubTripStopsValid(List<Stop> routedStartStops,
                                                              List<Stop> routedEndStops,
                                                              out string feedback)
        {
            //2013-06-13 zhou kai adds function to check within routed plan
            //sub trip stops

            //Return true means all stops can matche
            //Return false means there're stops that do not match
            Int32 nRoutedPlanSubTripStopsCount = routedStartStops.Count;
            feedback = String.Empty;

            if (routedStartStops.Count != routedEndStops.Count)
            {
                feedback = TptResourceBLL.ErrStopsInEqual;
                return false;
            }

            for (Int32 i = 0; i < nRoutedPlanSubTripStopsCount; i++)
            {
                if (routedStartStops[i].ToString().Equals(
                                   routedEndStops[i].ToString()))
                {
                    feedback += "start stop: " + routedStartStops[i].Code + ".\n";
                }
            }

            if (feedback.Length == 0)
            {
                return true;
            }
            else
            {
                feedback = TptResourceBLL.ErrStartStopnEndStop + "\n" + feedback;
                return false;
            }
            //2013-07-01 ends
        }

        //2013-06-11 zhou kai adds function to create plan truck sub trips by a given route
        public SortableList<PlanTruckSubTrip> CreatePlanSubTruckTrips(List<Stop> startStops,
                                                                      List<Stop> endStops,
                                                                      List<DateTime> startTimes,
                                                                      List<DateTime> endTimes,
                                                                      Driver driver,
                                                                      string frmName,
                                                                      string usrID)
        {
            Int32 nCount = startStops.Count;
            Int32 nJobTripSeqNo = 0;
            SortableList<PlanTruckSubTripJob> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();
            SortableList<PlanTruckSubTrip> planSubTruckTripsByRoute = new SortableList<PlanTruckSubTrip>();

            for (int i = 0; i < nCount; i++)
            {
                PlanTruckSubTrip ptst = new PlanTruckSubTrip(nJobTripSeqNo, startTimes[i], endTimes[i], startStops[i],
                                                             endStops[i], false, String.Empty,
                                                             planTruckSubTripJobs,
                                                             this, JobTripStatus.Assigned, new byte[8]);
                ptst.DriverNumber = driver.Code;
                ptst.VehicleNumber = driver.defaultVehicleNumber;

                planSubTruckTripsByRoute.Add(ptst);
            }

            return planSubTruckTripsByRoute;
            //2013-06-18 ends
        }

        //2013-06-12 zhou kai adds function to add the routed plan truck sub trips 
        //into the database
        public void AddRoutedPlanTruckSubTrips(SortableList<PlanTruckSubTrip> routedPlanTruckSubTrips, string usrID)
        {
            int indexPlanSubTrip = 0;
            string feedback = String.Empty;

            foreach (PlanTruckSubTrip ptst in routedPlanTruckSubTrips)
            {
                try
                {
                    AddPlanTruckSubTrip(ptst, @"ADD_ROUTED_PLAN_TRUCK_SUB_TRIPS", usrID);
                }
                catch (FMException fe)
                {
                    indexPlanSubTrip++;
                    feedback = feedback + "the plan sub trip #" + indexPlanSubTrip.ToString() + "\n";
                }
                catch (Exception e)
                {
                    indexPlanSubTrip++;
                    feedback = feedback + "the plan sub trip #" + indexPlanSubTrip.ToString() + "\n";
                }
            }

            if (!feedback.Equals(String.Empty))
            {
                throw new FMException(feedback);
            }

            //2013-08-22 ends
        }

        #endregion

        public static SortableList<DateTime> GetPlanTripDates(DateTime fromDate, DateTime toDate, string tptDept)
        {
            return PlanTruckDAL.GetPlanTripDates(fromDate, toDate, tptDept);
        }

        public bool CanChangeDriver()
        {
            bool flag = true;

            for (int i = 0; i < this.planTruckSubTrips.Count; i++)
            {
                for (int j = 0; j < this.planTruckSubTrips[i].planTruckSubTripJobs.Count; j++)
                {
                    if (this.planTruckSubTrips[i].planTruckSubTripJobs[j].status == JobTripStatus.Completed)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 2014-05-10 Zhou Kai adds comments to this function:
        /// The function  is used to change the whole planTruckTrip(this) including
        /// its planTruckSubTrips and truckJobTripPlans to a new driver.
        /// </summary>
        /// <param name="newDriver">The new driver which the planTruckTrip is shift to</param>
        /// <param name="truckJobTrips">All the truckJobTrips the planTruckTrip(this class) owns</param>
        /// <returns>boolean</returns>
        public bool ChangeDriver(Driver newDriver, SortableList<TruckJobTrip> truckJobTrips)
        {
            if (CanChangeDriver())
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    PlanTruckTrip outPlanTruckTrip = new PlanTruckTrip();
                    PlanTruckDAL.ChangeDriver(this, newDriver, con, tran, out outPlanTruckTrip);

                    this.Driver = newDriver;
                    this.Vehicle = Vehicle.GetVehicle(newDriver.defaultVehicleNumber);
                    this.VehicleNumber = newDriver.defaultVehicleNumber;

                    for (int i = 0; i < this.planTruckSubTrips.Count; i++)
                    {
                        this.planTruckSubTrips[i].DriverNumber = newDriver.Code;
                        this.planTruckSubTrips[i].VehicleNumber = newDriver.defaultVehicleNumber;
                    }

                    TruckJobTripPlan.ChangeDrivers(this.PlanTripNo, newDriver, con, tran);

                    // 2014-05-10 Zhou Kai modifies the double loop below:
                    for (int i = 0; i < truckJobTrips.Count; i++)
                    {
                        // 2014-05-10 Zhou Kai adds line
                        int nTruckJobTripPlansCount = truckJobTrips[i].truckJobTripPlans.Count;
                        // 2014-05-10 Zhou Kai ends

                        for (int j = 0; j < nTruckJobTripPlansCount; j++)
                        {
                            if (truckJobTrips[i].truckJobTripPlans[j].planTripNo == this.PlanTripNo)
                            {
                                truckJobTrips[i].truckJobTripPlans[j].driver = newDriver.Code;
                                truckJobTrips[i].truckJobTripPlans[j].truckNo = newDriver.defaultVehicleNumber;
                            }
                        }
                    }
                    // 2014-05-10 Zhou Kai ends

                    tran.Commit();
                }
                catch (FMException ex)
                {
                    tran.Rollback();
                    return false;
                    throw ex;

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return false;
                    throw ex;

                }
                finally
                {
                    con.Close();
                }
                return true;
            }
            else
            {
                return false;
                throw new FMException(TptResourceBLL.ErroFailedToChangeDriver);

            }
        }

        //This method is used when creating a new planTruckSubTrip from UI, without any job trips
        public bool AddPlanTruckSubTrip(PlanTruckSubTrip planTruckSubTrip, string frmName, string user)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            bool retValue = false;
            try
            {
                //17 Jan 2012 - Gerry  Added validation, not allow to the same start Stop/Time and End Stop/Time
                planTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                if (CanAddPlanTruckSubTrip(planTruckSubTrip))
                {
                    PlanTruckSubTrip outPlanTruckSubTrip = new PlanTruckSubTrip();
                    //Replaced - inorder to cater the preeceding PlanTruckSubTrip
                    //PlanTruckDAL.AddPlanTruckSubTripDatabase(this, planTruckSubTrip, con, tran, out outPlanTruckSubTrip);
                    planTruckSubTrip.AddPlanTruckSubTrip(this, con, tran, frmName, user);

                    tran.Commit();
                    retValue = true;
                }
            }
            catch (FMException ex)
            {
                retValue = false;
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                retValue = false;
                tran.Rollback();
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return retValue;
        }

        //This method is used when when called from another BLL method which initiates the transaction
        // a) ChangeDriver 

        public bool AddPlanTruckSubTrip(PlanTruckSubTrip planTruckSubTrip, string frmName, string user, SqlConnection con, SqlTransaction tran, bool isNextPlanSubTrip = false)
        {
            bool retValue = false;
            try
            {
                //17 Jan 2012 - Gerry  Added validation, not allow to the same startStop and EndStop
                if (CanAddPlanTruckSubTrip(planTruckSubTrip))
                {
                    PlanTruckSubTrip outPlanTruckSubTrip = new PlanTruckSubTrip();
                    PlanTruckDAL.AddPlanTruckSubTripDatabase(this, planTruckSubTrip, con, tran, out outPlanTruckSubTrip, frmName, user, isNextPlanSubTrip);
                    planTruckSubTrip = outPlanTruckSubTrip;
                    retValue = true;
                }
            }
            catch (FMException ex)
            {
                retValue = false;
                throw ex;
            }
            catch (Exception ex)
            {
                retValue = false;
                throw ex;
            }
            return retValue;
        }

        public static PlanTruckTrip FindPlanTruckTripByDriver(SortableList<PlanTruckTrip> planTruckTrips, Driver driver)
        {
            for (int i = 0; i < planTruckTrips.Count; i++)
            {
                if (planTruckTrips[i].Driver.Code == driver.Code)
                {
                    return planTruckTrips[i];
                    break;
                }
            }
            return null;
        }

        public static PlanTruckTrip FindPlanTruckTripByVehicle(SortableList<PlanTruckTrip> planTruckTrips, Vehicle vehicle)
        {
            for (int i = 0; i < planTruckTrips.Count; i++)
            {
                if (planTruckTrips[i].Vehicle.Number == vehicle.Number)
                {
                    return planTruckTrips[i];
                    break;
                }
            }
            return null;
        }
        public bool SetJobStatusToComplete(PlanTruckSubTripJob currentPlanTruckSubTripJob, SortableList<TruckJobTrip> truckJobTrips)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                currentPlanTruckSubTripJob.SetJobTripStatus(JobTripStatus.Completed, con, tran);
                //20160223 - gerry modified
                TruckJobTrip jobTrip = null;
                //use linq instead of looping manually
                var tempTruckJobTrips = truckJobTrips.Where(jt => jt.JobID == currentPlanTruckSubTripJob.jobID && jt.Sequence == currentPlanTruckSubTripJob.jobSeq).ToList();
                if (tempTruckJobTrips != null)
                {
                    if (tempTruckJobTrips.Count > 0)
                    {
                        jobTrip = tempTruckJobTrips[0] as TruckJobTrip;
                        //use linq instead of looping manually              
                        var tempJobTripPlans = jobTrip.truckJobTripPlans.Where(jtp => (jtp.jobID == currentPlanTruckSubTripJob.jobID) &&
                                                                                        (jtp.sequence == currentPlanTruckSubTripJob.jobSeq) &&
                                                                                        (jtp.planTripNo == currentPlanTruckSubTripJob.planTripNo) &&
                                                                                        (jtp.planSubTripSeqNo == currentPlanTruckSubTripJob.planSubTripSeqNo) &&
                                                                                        (jtp.planSubTripJobSeqNo == currentPlanTruckSubTripJob.planSubTripJobSeqNo)).ToList();
                                                                                        //&& //modified 20161208
                                                                                        //(jtp.volume == currentPlanTruckSubTripJob.volume) &&
                                                                                        //(jtp.weight == currentPlanTruckSubTripJob.weight)).ToList();
                        if (tempJobTripPlans.Count > 0)
                        {
                            TruckJobTripPlan truckJobTripPlan = tempJobTripPlans[0] as TruckJobTripPlan;
                            truckJobTripPlan.SetJobTripPlanStatus(JobTripStatus.Completed, con, tran);
                        }
                    }
                }
                //20160311 - gerry added to auto set job trip to complete if weight and volume are fully  allocated
                foreach (PlanTruckSubTrip planSubTrip in this.planTruckSubTrips)
                {
                    planSubTrip.planTruckSubTripJobs = PlanTruckDAL.GetPlanTruckSubTripJobs(this.PlanTripNo, planSubTrip, con, tran);
                    if (planSubTrip.CanSetToComplete())
                    {
                        if (planSubTrip.SetPlanTruckSubTrip(JobTripStatus.Completed, this.PlanTripNo, con, tran))
                        {
                            if (jobTrip.GetBalanceVolForPlan() <= 0 && jobTrip.GetBalanceWeightForPlan() <= 0)
                            {
                                JobTripState jobTripState = new JobTripState(1, JobTripStatus.Completed, DateTime.Now, "Auto set to complete.", true);
                                jobTrip.SetJobTripStatusForTruck(jobTripState, con, tran, "system", "system", "Assigned");
                            }
                        }
                    }
                }

                #region 20160223 - Gerry Removed and replaced code above
                /*
                for (int i = 0; i < tempTruckJobTrips.Count; i++)
                {
                    //17 Jan Gerry added flag if set complete
                    bool set = false;
                    if (tempTruckJobTrips[i].truckJobTripPlans.Count > 0)
                    {
                        for (int j = 0; j < tempTruckJobTrips[i].truckJobTripPlans.Count; j++)
                        {
                            if ((tempTruckJobTrips[i].truckJobTripPlans[j].jobID == currentPlanTruckSubTripJob.jobID) &&
                                (tempTruckJobTrips[i].truckJobTripPlans[j].sequence == currentPlanTruckSubTripJob.jobSeq) &&
                                (tempTruckJobTrips[i].truckJobTripPlans[j].planTripNo == currentPlanTruckSubTripJob.planTripNo) &&
                                (tempTruckJobTrips[i].truckJobTripPlans[j].planSubTripJobSeqNo == currentPlanTruckSubTripJob.planSubTripJobSeqNo))
                            {
                                tempTruckJobTrips[i].truckJobTripPlans[j].SetJobTripPlanStatus(JobTripStatus.Completed, con, tran);
                                set = true;
                                break;
                            }
                        }
                    }
                    if (set)//if set complete break from loop
                        break;
                }
                */
                #endregion
                //loop is not needed
                #region 20131206 Gerry Removed
                /*
                for (int i = 0; i < this.truckSubTrips.Count; i++)
                {
                    bool set = false;
                    if (truckJobTrips[i].truckJobTripPlans.Count > 0)
                    {
                        for (int j = 0; j < this.truckSubTrips[i].planTruckSubTripJobs.Count; j++)
                        {
                            if ((this.truckSubTrips[i].planTruckSubTripJobs[j].jobID == currentPlanTruckSubTripJob.jobID) &&
                                (this.truckSubTrips[i].planTruckSubTripJobs[j].jobSeq == currentPlanTruckSubTripJob.jobSeq) &&
                                (this.truckSubTrips[i].planTruckSubTripJobs[j].planTripNo == currentPlanTruckSubTripJob.planTripNo) &&
                                (this.truckSubTrips[i].planTruckSubTripJobs[j].jobTripPlanSeq == currentPlanTruckSubTripJob.jobTripPlanSeq))
                            {
                                this.truckSubTrips[i].planTruckSubTripJobs[j].SetJobTripStatus(JobTripStatus.Completed, con, tran);
                                set = true;
                                break;
                            }
                        }
                    }
                    if (set)//if set complete break from loop
                        break;  
                }
                 */
                #endregion

                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return retValue;
        }
        //update from driver message
        public bool SetPlanTruckSubTripStatusToComplete(PlanTruckSubTrip currentPlanTruckSubTrip)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //currentPlanTruckSubTrip = PlanTruckDAL.GetPlanTruckSubTrip(this, currentPlanTruckSubTrip.SeqNo, con, tran);
                foreach (PlanTruckSubTripJob pstj in currentPlanTruckSubTrip.planTruckSubTripJobs)
                {
                    if (pstj.endStop.Code.Trim().ToString() == currentPlanTruckSubTrip.EndStop.Code.Trim().ToString())
                    {
                        pstj.SetJobTripStatus(JobTripStatus.Completed, con, tran);
                        TruckJobTrip jobTrip = TruckJobTrip.GetTruckJobTrip(pstj.jobID, pstj.jobSeq, con, tran);
                        if (jobTrip != null)
                        {
                            //use linq instead of looping manually
                            var tempJobTripPlans = jobTrip.truckJobTripPlans.Where(jtp => (jtp.jobID == pstj.jobID) &&
                                                                                            (jtp.sequence == pstj.jobSeq) &&
                                                                                            (jtp.planTripNo == pstj.planTripNo) &&
                                                                                            (jtp.volume == pstj.volume) &&
                                                                                            (jtp.weight == pstj.weight)).ToList();
                            if (tempJobTripPlans.Count > 0)
                            {
                                TruckJobTripPlan truckJobTripPlan = tempJobTripPlans[0] as TruckJobTripPlan;
                                truckJobTripPlan.SetJobTripPlanStatus(JobTripStatus.Completed, con, tran);
                            }
                        }
                        //20160311 - gerry added to auto set job trip to complete if weight and volume are fully  allocated
                        foreach (PlanTruckSubTrip planSubTrip in this.planTruckSubTrips)
                        {
                            planSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(planSubTrip, con, tran);
                            planSubTrip.planTruckSubTripJobs = PlanTruckDAL.GetPlanTruckSubTripJobs(this.PlanTripNo, planSubTrip, con, tran);
                            if (planSubTrip.CanSetToComplete())
                            {
                                if (planSubTrip.SetPlanTruckSubTrip(JobTripStatus.Completed, this.PlanTripNo, con, tran))
                                {
                                    if (jobTrip.GetBalanceVolForPlan() <= 0 && jobTrip.GetBalanceWeightForPlan() <= 0)
                                    {
                                        JobTripState jobTripState = new JobTripState(1, JobTripStatus.Completed, DateTime.Now, "Auto set to complete.", true);
                                        jobTrip.SetJobTripStatusForTruck(jobTripState, con, tran, "system", "system", "Assigned");
                                    }
                                }
                            }
                        }
                    }
                }
                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return retValue;
        }
        //set complete manually
        public bool SetJobStatusToComplete(PlanTruckSubTripJob currentPlanTruckSubTripJob)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                currentPlanTruckSubTripJob.SetJobTripStatus(JobTripStatus.Completed, con, tran);
                //20160223 - gerry modified
                TruckJobTrip jobTrip = TruckJobTrip.GetTruckJobTrip(currentPlanTruckSubTripJob.jobID, currentPlanTruckSubTripJob.jobSeq);
                if (jobTrip != null)
                {
                    //use linq instead of looping manually
                    var tempJobTripPlans = jobTrip.truckJobTripPlans.Where(jtp => (jtp.jobID == currentPlanTruckSubTripJob.jobID) &&
                                                                                    (jtp.sequence == currentPlanTruckSubTripJob.jobSeq) &&
                                                                                    (jtp.planTripNo == currentPlanTruckSubTripJob.planTripNo) &&
                                                                                    (jtp.volume == currentPlanTruckSubTripJob.volume) &&
                                                                                    (jtp.weight == currentPlanTruckSubTripJob.weight)).ToList();
                    if (tempJobTripPlans.Count > 0)
                    {
                        TruckJobTripPlan truckJobTripPlan = tempJobTripPlans[0] as TruckJobTripPlan;
                        truckJobTripPlan.SetJobTripPlanStatus(JobTripStatus.Completed, con, tran);
                    }
                }
                //20160311 - gerry added to auto set job trip to complete if weight and volume are fully  allocated
                foreach (PlanTruckSubTrip planSubTrip in this.planTruckSubTrips)
                {
                    planSubTrip.planTruckSubTripJobs = PlanTruckDAL.GetPlanTruckSubTripJobs(this.PlanTripNo, planSubTrip, con, tran);
                    if (planSubTrip.CanSetToComplete())
                    {
                        if (planSubTrip.SetPlanTruckSubTrip(JobTripStatus.Completed, this.PlanTripNo, con, tran))
                        {
                            if (jobTrip.GetBalanceVolForPlan() <= 0 && jobTrip.GetBalanceWeightForPlan() <= 0)
                            {
                                JobTripState jobTripState = new JobTripState(1, JobTripStatus.Completed, DateTime.Now, "Auto set to complete.", true);
                                jobTrip.SetJobTripStatusForTruck(jobTripState, con, tran, "system", "system", "Assigned");
                            }
                        }
                    }
                }
                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return retValue;
        }


        //Not impletemented, Sort PlanTruckSubTrip by DateTime instead of plantripseq_no
        //19 Jan 2012 - Gerry Added this method to sort planTruckSubTrips by startTime
        //can not be use because SortableList<T> which we used for the collections of PlanTrip is not supported .Sort() method

        public List<PlanTruckSubTrip> GetSortedPlanTruckSubTripByStartDateTime()
        {
            List<PlanTruckSubTrip> retValues = new List<PlanTruckSubTrip>();
            if (this.planTruckSubTrips.Count > 0)
            {
                foreach (PlanTruckSubTrip temp in this.planTruckSubTrips)
                {
                    retValues.Add(temp);
                }
            }
            retValues.Sort(delegate(PlanTruckSubTrip ptst1, PlanTruckSubTrip ptst2)
                            { return (ptst1.Start.CompareTo(ptst2.Start)); });

            return retValues;
        }

        #region "Sort a list of plan sub trips by Start / End time, Start / End Stop, used by FrmAddRoutedPlanTruckSubTrips.cs"
        //2013-07-15 zhou kai adds function to sort plan sub trip by start time
        public static void SortPlanTruckSubTripsByStartDateTime(List<PlanTruckSubTrip> planTruckSubTrips)
        {
            planTruckSubTrips.Sort(delegate(PlanTruckSubTrip ptst1, PlanTruckSubTrip ptst2)
            { return (ptst1.Start.CompareTo(ptst2.Start)); });

            return;
            //2013-07-15 ends
        }

        //2013-07-15 zhou kai adds function to sort plan sub trip by end time
        public static void SortPlanTruckSubTripsByEndDateTime(List<PlanTruckSubTrip> planTruckSubTrips)
        {
            planTruckSubTrips.Sort(delegate(PlanTruckSubTrip ptst1, PlanTruckSubTrip ptst2)
            { return (ptst1.End.CompareTo(ptst2.End)); });

            return;
            //2013-07-15 ends
        }

        //2013-07-15 zhou kai adds function to sort plan sub trip by start stop
        public static void SortPlanTruckSubTripsByStartStop(List<PlanTruckSubTrip> planTruckSubTrips)
        {
            planTruckSubTrips.Sort(delegate(PlanTruckSubTrip ptst1, PlanTruckSubTrip ptst2)
            { return (ptst1.StartStop.CompareTo(ptst2.StartStop)); });

            return;
            //2013-07-15 ends
        }

        //2013-07-15 zhou kai adds function to sort plan sub trip by end stop
        public static void SortPlanTruckSubTripsByEndStop(List<PlanTruckSubTrip> planTruckSubTrips)
        {
            planTruckSubTrips.Sort(delegate(PlanTruckSubTrip ptst1, PlanTruckSubTrip ptst2)
            { return (ptst1.EndStop.CompareTo(ptst2.EndStop)); });

            return;
            //2013-07-15 ends
        }
        #endregion

        public static bool ModifyPlanTruckTrips(SortableList<PlanTruckTrip> existingPlanTruckTrips,
                            SortableList<Driver> existingDrivers, DateTime scheduleDate, string decpCode, string formName, string useID)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //loop to add new planTruckTrip if new Driver is added to schedule date
                for (int i = 0; i < existingDrivers.Count; i++)
                {
                    bool driverExist = false;
                    for (int j = 0; j < existingPlanTruckTrips.Count; j++)
                    {
                        if (existingDrivers[i].Code.Trim() == existingPlanTruckTrips[j].Driver.Code.Trim())
                        {
                            driverExist = true;
                            break;
                        }
                    }
                    if (!driverExist)
                    {
                        PlanTruckTrip newPlanTruckTrip = new PlanTruckTrip();
                        newPlanTruckTrip.Driver = existingDrivers[i];
                        newPlanTruckTrip.ScheduleDate = existingPlanTruckTrips[0].ScheduleDate;
                        newPlanTruckTrip.dept = existingPlanTruckTrips[0].dept;
                        newPlanTruckTrip.VehicleNumber = existingDrivers[0].defaultVehicleNumber;

                        newPlanTruckTrip.AddPlanTruckTripDatabase(con, tran);
                        existingPlanTruckTrips.Add(newPlanTruckTrip);
                    }
                }

                //loop to delete plantrip if driver was deleted from scheduleDate or unable
                for (int i = 0; i < existingPlanTruckTrips.Count; i++)
                {
                    bool driverExist = false;
                    for (int j = 0; j < existingDrivers.Count; j++)
                    {
                        if (existingPlanTruckTrips[i].Driver.Code.Trim() == existingDrivers[j].Code.Trim())
                        {
                            driverExist = true;
                            break;
                        }
                    }

                    if (!driverExist)
                    {
                        existingPlanTruckTrips[i].DeletePlanTruckTrip(con, tran, formName, useID);
                        existingPlanTruckTrips.Remove(existingPlanTruckTrips[i]);
                    }

                }
                //logging here..

                tran.Commit();
                retValue = true;
            }
            catch (FMException fmEx)
            {
                retValue = false;
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                retValue = false;
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }

            return retValue;
        }

        public bool DeletePlanTruckTrip(SqlConnection con, SqlTransaction tran, string formName, string userID)
        {
            bool retValue = false;
            try
            {
                if (this.pPlanTruckSubTrips.Count <= 0)
                {
                    retValue = PlanTruckDAL.DeletePlanTruckTrip(this.PlanTripNo, con, tran);
                }
                else
                {
                    throw new FMException(TptResourceBLL.ErrCantDeletePlanTripSomePlanSubTripExist);
                }
            }
            catch (FMException fmEx) { throw fmEx; }

            return retValue;
        }

        public bool UpdatePlaTruckSubTripTime()
        {
            return true;
        }

        //20151221 - gerry start modify trucking

        public static void CreatePlanTruckTrip()
        {
            try
            {

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

        }
        public static bool ValidateDropTruckJobTrip()
        {
            try
            {

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
            return true;
        }
        public void UpdatePlanTruckSubTripStopsAndTimeInThesameDriver(PlanTruckSubTrip planTruckSubTrip, DateTime startTime, DateTime endTime, string userID, string formName, string deptCode)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    int indx = 0;


                    UpdatePlanTruckSubTripStopsAndTimeInThesameDriver(planTruckSubTrip, startTime, endTime, userID, formName, deptCode, con, tran);


                    tran.Commit();
                }
                catch (FMException fmEx)
                {
                    tran.Rollback();
                    throw fmEx;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally { con.Close(); }
            }
        }
        public void UpdatePlanTruckSubTripStopsAndTimeInThesameDriver(PlanTruckSubTrip planTruckSubTrip, DateTime startTime, DateTime endTime,
            string userID, string formName, string deptCode, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string outMsg = string.Empty;
                if (!PlanTruckSubTrip.HasPlanTruckSubTripChanged(planTruckSubTrip, con, tran))
                {
                    planTruckSubTrip.Start = startTime;
                    planTruckSubTrip.End = endTime;
                    if (planTruckSubTrip.planTruckSubTripJobs.Count > 0)
                    {
                        for (int i = 0; i < planTruckSubTrip.planTruckSubTripJobs.Count; i++)
                        {
                            //update truckjobtripPlan
                            PlanTruckDAL.UpdateActualStartEndTimeforTruckJobDetailTripPlan(
                                                                    planTruckSubTrip.planTruckSubTripJobs[i].jobID,
                                                                    planTruckSubTrip.planTruckSubTripJobs[i].jobSeq,
                                                                    planTruckSubTrip.planTruckSubTripJobs[i].planTripNo,
                                                                    planTruckSubTrip.planTruckSubTripJobs[i].planSubTripJobSeqNo,
                                                                    startTime, endTime, con, tran);
                        }
                    }
                    //now update PlanTruckSubTrip Time
                    PlanTruckDAL.UpdatePlanTruckSubTripStopsAndTime(this, planTruckSubTrip, con, tran);
                    planTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(planTruckSubTrip, con, tran);

                }
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePlanTruckSubTripStartAndEndTime(PlanTruckSubTrip planTruckSubTrip, SqlConnection con, SqlTransaction tran)
        {
            PlanTruckDAL.UpdatePlanTruckSubTripStartAndEndTime(planTruckSubTrip, con, tran);
        }
        //Create TruckJobTripPlan
        public TruckJobTripPlan CreateTruckJobTripPlan(PlanTruckSubTripJob planTruckSubTripJob, DateTime startTime, DateTime endTime)
        {
            TruckJobTripPlan truckJobTripPlan = null;
            try
            {
                //if (planTruckSubTripJob.status != JobTripStatus.Ready || planTruckSubTripJob.status != JobTripStatus.Assigned)
                //{
                //    throw new FMException("Opps! Can only assign Job trip which READY or ASSIGNED status. ");
                //}
                truckJobTripPlan = new TruckJobTripPlan();
                truckJobTripPlan.scheduleDate = planTruckSubTripJob.startDate.Date;
                truckJobTripPlan.jobID = planTruckSubTripJob.jobID;
                truckJobTripPlan.planSubTripSeqNo = planTruckSubTripJob.planSubTripSeqNo;
                truckJobTripPlan.planSubTripJobSeqNo = planTruckSubTripJob.planSubTripJobSeqNo;
                truckJobTripPlan.sequence = planTruckSubTripJob.jobSeq;
                truckJobTripPlan.planTripNo = planTruckSubTripJob.planTripNo;
                truckJobTripPlan.driver = this.Driver.Code; ;
                truckJobTripPlan.truckNo = this.Driver.defaultVehicleNumber;
                truckJobTripPlan.volume = planTruckSubTripJob.volume;
                truckJobTripPlan.weight = planTruckSubTripJob.weight;
                truckJobTripPlan.start = startTime;
                truckJobTripPlan.end = endTime;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
            return truckJobTripPlan;
        }
        public TruckJobTripPlan CreateTruckJobTripPlan(TruckJobTrip truckJobTrip, DateTime startTime, DateTime endTime)
        {
            TruckJobTripPlan truckJobTripPlan = null;
            try
            {
                byte[] latestversion = truckJobTrip.GetTruckJobTripUpdatedVersion();

                if (!SqlBinary.Equals(latestversion, truckJobTrip.UpdateVersion))
                {
                    throw new FMException("Opps! JobID: " + truckJobTrip.JobID.ToString() + " SeqNo: " + truckJobTrip.Sequence + " was updated by someone. Please refresh your screen to get the latest data. ");
                }
                if (!(truckJobTrip.TripStatus == JobTripStatus.Ready || truckJobTrip.TripStatus == JobTripStatus.Assigned))
                {
                    throw new FMException("Opps! Can only assign Job trip which READY or ASSIGNED status. ");
                }
                truckJobTripPlan = new TruckJobTripPlan();
                truckJobTripPlan.scheduleDate = this.ScheduleDate;
                truckJobTripPlan.jobID = truckJobTrip.JobID;
                truckJobTripPlan.sequence = truckJobTrip.Sequence;
                truckJobTripPlan.planSubTripSeqNo = 0; //assign later
                truckJobTripPlan.planSubTripJobSeqNo = 0; //assign later
                truckJobTripPlan.planTripNo = this.PlanTripNo;
                truckJobTripPlan.driver = this.Driver.Code; ;
                truckJobTripPlan.truckNo = this.Driver.defaultVehicleNumber;
                truckJobTripPlan.volume = TruckJobTripPlan.GetUnassignedTruckJobTripVolume(truckJobTrip);
                truckJobTripPlan.weight = TruckJobTripPlan.GetUnAssignedTruckJobTripWeight(truckJobTrip);
                truckJobTripPlan.start = startTime;
                truckJobTripPlan.end = endTime;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
            return truckJobTripPlan;
        }
        public SortableList<TruckJobTripPlan> CreateTruckJobTripPlans(SortableList<TruckJobTrip> truckJobTrips, DateTime startTime, DateTime endTime)
        {
            SortableList<TruckJobTripPlan> truckJobTripPlans = new SortableList<TruckJobTripPlan>();
            try
            {
                foreach (TruckJobTrip jobTrip in truckJobTrips)
                {
                    truckJobTripPlans.Add(CreateTruckJobTripPlan(jobTrip, startTime, endTime));
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return truckJobTripPlans;
        }
        public void CheckTruckJobTripsBeforeAssignToVehicle(SortableList<TruckJobTrip> truckJobTrips, DateTime startTime, DateTime endTime, decimal actualWeight, decimal actualVolume, out string outMsg, out bool overload)
        {
            try
            {
                outMsg = "";
                overload = false;
                Vehicle truck = Vehicle.GetVehicle(this.Driver.defaultVehicleNumber);
                var tempTruckJobTrips = truckJobTrips.OrderBy(jt => jt.StartTime).ToList(); // sort by time
                string firstStartLocation = tempTruckJobTrips[0].StartStop.Code.ToString().Trim();
                string firstEndLocation = tempTruckJobTrips[0].EndStop.Code.ToString().Trim();
                foreach (TruckJobTrip truckJobTrip in tempTruckJobTrips)
                {   //20170421 -gerry modified the validation to drag/drop together
                    if (truckJobTrip.StartStop.Code.Trim() != firstStartLocation && truckJobTrip.tripType != Trip_Type.MTO)
                        throw new FMException("Only job trip with the same start location were allowed to drag/drop together.");
                    else if (truckJobTrip.EndStop.Code.Trim() != firstEndLocation && truckJobTrip.tripType == Trip_Type.MTO)
                        throw new FMException("Only job trip with the same end location were allowed to drag/drop together.");

                    TruckJobTripPlan truckJobTripPlan = CreateTruckJobTripPlan(truckJobTrip, startTime, endTime);
                    truckJobTripPlan.volume = actualVolume;
                    truckJobTripPlan.weight = actualWeight;
                    if (truckJobTripPlan.weight <= 0 || truckJobTripPlan.volume <= 0)
                        throw new FMException("Opps! JobID: " + truckJobTripPlan.jobID.ToString() + " SeqNo: " + truckJobTripPlan.sequence.ToString() + " has no available weight or volume to assign. ");
                    if (IsPlanSubTripOverlapping(truckJobTripPlan))
                        throw new FMException(TptResourceBLL.ErrStartTimeEndTimeOverlapWithOthers);

                    overload = CheckTruckCapacityIsOverload(truckJobTripPlan.volume, truckJobTripPlan.weight, truck, out outMsg);
                }

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
        }
        public bool CheckTruckCapacityIsOverload(decimal volume, decimal weight, Vehicle selectedTruck, out string outOverLoadMsg, bool createNew = true)
        {
            outOverLoadMsg = string.Empty;
            bool retValue = false;
            try
            {
                if (volume > selectedTruck.VolCapacity)
                {
                    retValue = true;
                    outOverLoadMsg = outOverLoadMsg + TptResourceBLL.ErrExceededVolume;
                }
                if (weight > selectedTruck.MaximumLadenWeight)
                {
                    retValue = true;
                    outOverLoadMsg = outOverLoadMsg.Equals(string.Empty) ? TptResourceBLL.ErrExceededWeight : ("Volume and " + TptResourceBLL.ErrExceededWeight);
                }
                if (!outOverLoadMsg.Equals(string.Empty))
                {
                    outOverLoadMsg = outOverLoadMsg + ". Please assign this plan sub trip to a truck with bigger weight capacity ";
                    if (createNew)
                    {
                        outOverLoadMsg = outOverLoadMsg + "or split into multiple trucks. \n";
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        ///20170426 - gerry added to check if the time can catchup the flight date time
        ///only applicable for MTO or Air Export jobs
        public void CheckCutOffTimeForTheFlight(SortableList<TruckJobTrip> truckJobTrips, DateTime startTime, DateTime endTime)
        {
            try
            {
                PlanTruckSubTrips planTruckSubTrips = CreatePlanTruckSubTripsForMTOInMemory(truckJobTrips, startTime, endTime);
                var lastPlanSubTrip = planTruckSubTrips[planTruckSubTrips.Count -1];
                var lastPlanSubTripJob = lastPlanSubTrip.planTruckSubTripJobs.FirstOrDefault(pstj => pstj.endStop.Code.Trim() == lastPlanSubTrip.EndStop.Code.Trim());
                DateTime flightDateTime = TruckJob.GetTruckJob(lastPlanSubTripJob.jobID).flightDate;
                int cutOffTime = 2;
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.TRUCKING_SETTINGS_ID, ApplicationOption.SETTINGS_TRK_PLAN_CUTOFF_TIME_BEFORE_FLIGHT);
                if (option.setting_Value != "") { cutOffTime = Convert.ToInt32(option.setting_Value); }

                if (lastPlanSubTrip.End > flightDateTime.AddHours((-1)*cutOffTime))
                    throw new Exception("The last job cannot catch up the flight checkin cut-off which is " + cutOffTime
                        + " hours before flight time(" + flightDateTime.ToShortTimeString() + "), please let the driver start this job earlier. ");
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
        }

        public bool IsPlanSubTripOverlapping(PlanTruckSubTrip newPlanTruckSubTrip)
        {
            bool test = false;
            for (int i = 0; i < planTruckSubTrips.Count; i++)
            {
                if (newPlanTruckSubTrip.VehicleNumber == planTruckSubTrips[i].VehicleNumber)
                {
                    if ((newPlanTruckSubTrip.Start == planTruckSubTrips[i].Start) && (newPlanTruckSubTrip.End == planTruckSubTrips[i].End))
                    {
                        test = true;
                        break;
                    }
                    if ((newPlanTruckSubTrip.Start > planTruckSubTrips[i].Start) && (newPlanTruckSubTrip.Start < planTruckSubTrips[i].End))
                    {
                        test = true;
                        break;
                    }
                    if ((newPlanTruckSubTrip.Start <= planTruckSubTrips[i].Start) && (newPlanTruckSubTrip.End >= planTruckSubTrips[i].End))
                    {
                        test = true;
                        break;
                    }
                    if ((newPlanTruckSubTrip.Start <= planTruckSubTrips[i].Start) && (newPlanTruckSubTrip.End > planTruckSubTrips[i].Start))
                    {
                        test = true;
                        break;
                    }
                    if ((newPlanTruckSubTrip.End > planTruckSubTrips[i].Start) && (newPlanTruckSubTrip.End <= planTruckSubTrips[i].End))
                    {
                        test = true;
                        break;
                    }
                }
            }
            return test;
        }
        /// <summary>
        /// 
        /// only check time if estimated start is specified in booking
        /// 
        /// </summary>
        /// <param name="truckJobTrip"></param>
        /// <param name="selectedStartTime"></param>
        /// <param name="outMsg"></param>
        public void CheckBookedAndPlanTime(TruckJobTrip truckJobTrip, DateTime selectedStartTime, DateTime selectedEndTime, out string outMsg)
        {
            outMsg = string.Empty;
            try
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_IMPLEMENT_JOB_TRIP_FIXEDTIME);
                if (option.setting_Value != "")
                {
                    Stop truckLocation = Stop.GetStop(this.Driver.defaultVehicleNumber.ToString());
                    if(this.planTruckSubTrips.Count > 0)
                        truckLocation = this.planTruckSubTrips[this.planTruckSubTrips.Count -1].EndStop;

                    int startHour = Convert.ToInt32(truckJobTrip.StartTime.Substring(0, 2));
                    int startMinute = Convert.ToInt32(truckJobTrip.StartTime.Substring(2, 2));
                    DateTime expectedStartTime = selectedStartTime.Date.AddHours(startHour).AddMinutes(startMinute);
                    int endHour = Convert.ToInt32(truckJobTrip.EndTime.Substring(0, 2));
                    int endMinute = Convert.ToInt32(truckJobTrip.EndTime.Substring(2, 2));
                    DateTime expectedEndTime = selectedEndTime.Date.AddHours(endHour).AddMinutes(endMinute);

                    if (truckJobTrip.isFixedTime)
                    {
                        #region Fixed Time
                        if (truckJobTrip.timeBound == TIME_BOUND.FIXED_PICKUP)
                        {
                            if (expectedStartTime < selectedStartTime || expectedStartTime > selectedEndTime)
                            {
                                //DateTime estimatedStartTime = GetStartTimeForFixedPickup(truckLocation, truckJobTrip);
                                outMsg = string.Format(this.Driver.defaultVehicleNumber + " expected pickup time is at {0}, please assign to the correct vehicle's timeslot. ",
                                                        expectedStartTime.ToShortTimeString());
                            }
                        }
                        if (truckJobTrip.timeBound == TIME_BOUND.FIXED_DELIVERY)
                        {
                            if (expectedEndTime < selectedStartTime || expectedEndTime > selectedEndTime)
                            {
                                //DateTime estimatedEndTime = GetStartTimeForFixedDelivery(truckLocation, truckJobTrip);
                                outMsg = string.Format(this.Driver.defaultVehicleNumber + " expected delivery time is at {0}, please assign to the correct vehicle's timeslot. ",
                                                        expectedEndTime.ToShortTimeString());
                            }
                        }
                        if (outMsg != string.Empty)
                            throw new FMException(outMsg);
                        #endregion
                    }
                    else
                    {
                        #region Flexible Time - AM/PM Pickup or AM/PM Delivery
                        string errMsg = "Expected time bound is {0}, please set start time to morning between {1}";
                        switch (truckJobTrip.timeBound)
                        {
                            case TIME_BOUND.AM_PICKUP:
                                if (selectedStartTime > expectedStartTime.Date.AddHours(11).AddMinutes(59))
                                    throw new FMException(string.Format(errMsg, truckJobTrip.timeBound.ToString(), "00:00 - 11:59"));
                                break;
                            case TIME_BOUND.AM_DELIVERY:
                                if (selectedEndTime >= expectedEndTime.Date.AddHours(11).AddMinutes(59))
                                    throw new FMException(string.Format(errMsg, truckJobTrip.timeBound.ToString(), "00:00 - 11:59"));
                                break;
                            case TIME_BOUND.PM_PICKUP:
                                if (selectedStartTime < expectedStartTime.Date.AddHours(12))
                                    throw new FMException(string.Format(errMsg, truckJobTrip.timeBound.ToString(), "12:00 - 23:59"));
                                break;
                            case TIME_BOUND.PM_DELIVERY:
                                if (selectedEndTime <= expectedEndTime.Date.AddHours(12))
                                    throw new FMException(string.Format(errMsg, truckJobTrip.timeBound.ToString(), "12:00 - 23:59"));
                                break;
                        }
                        #endregion
                    }

                    #region removed old codes
                    //if (truckJobTrip.StartTime != "0000") // onl
                        //{
                        //    int hour = Convert.ToInt32(truckJobTrip.StartTime.Substring(0, 2));
                        //    int minute = Convert.ToInt32(truckJobTrip.StartTime.Substring(2, 2));
                        //    DateTime estimatedStartTime = truckJobTrip.StartDate.Date.AddHours(hour).AddMinutes(minute);
                        //    if (DateTime.Compare(estimatedStartTime, selectedStartTime) != 0)
                        //    {
                        //        outMsg = string.Format("Expected start time in booking is at {0} {1} but you assign at {2} {3}. ",
                        //                                                estimatedStartTime.ToShortDateString(), estimatedStartTime.ToShortTimeString(),
                        //                                                selectedStartTime.ToShortDateString(), selectedStartTime.ToShortTimeString());
                        //    }
                        //}
                    #endregion
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        //this method will be use for OTM jobs when drag/drop from selected Jobtrips or assign from the MAP
        public PlanTruckSubTrip CreatePlanTruckSubTripForOTM1(SortableList<TruckJobTrip> truckJobTrips, decimal actualWeight, decimal actualVolume, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                using (con)
                {
                    //Vehicle truck = Vehicle.GetVehicle(this.Driver.defaultVehicleNumber);
                    if (con.State != ConnectionState.Open) { con.Open(); };
                    tran = con.BeginTransaction();
                    //var tempTruckJobTrips = truckJobTrips.OrderBy(jt => jt.StartTime).ToList(); // sort by time
                    //string firstEndLocation = tempTruckJobTrips[0].StartStop.Code.ToString().Trim(); // get the first destination
                    SortableList<PlanTruckSubTrip> tempPlanSubTrips = new SortableList<PlanTruckSubTrip>();
                    Stop lastEndStop = null;
                    DateTime lastEndDateTime = endTime;
                    int firstPlanTruckSubTripSeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                    int addedPlanSubTrip = 0;
                    foreach (TruckJobTrip jobTrip in truckJobTrips)
                    {
                        if (jobTrip.TripStatus == JobTripStatus.Completed)
                            throw new FMException("Opps! Job trip already completed. ");
                        if (actualVolume > TruckJobTripPlan.GetUnassignedTruckJobTripVolume(jobTrip, con, tran))
                            throw new FMException("Opps! you keyed in volume more than the unassigned cargo volume. ");
                        if (actualWeight > TruckJobTripPlan.GetUnAssignedTruckJobTripWeight(jobTrip, con, tran))
                            throw new FMException("Opps! you keyed in weight more than the unassigned cargo weight. ");

                        //create plan trucksubtrip
                        #region PlanTruckSubTrip
                        newPlanTruckSubTrip = new PlanTruckSubTrip();
                        newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                        newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                        newPlanTruckSubTrip.Start = startTime;
                        newPlanTruckSubTrip.End = endTime;
                        newPlanTruckSubTrip.StartStop = jobTrip.StartStop;
                        newPlanTruckSubTrip.EndStop = jobTrip.EndStop;
                        newPlanTruckSubTrip.IsBillableTrip = true;
                        newPlanTruckSubTrip.Description = "";
                        newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                        newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                        newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                        #endregion
                        if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                        {
                            //create plantrucksubtripjob
                            PlanTruckSubTripJob newPlanTruckSubTripJob = newPlanTruckSubTrip.CreatePlanTruckSubTripJob(jobTrip, actualVolume, actualWeight);
                            //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                            newPlanTruckSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);
                            if (addedPlanSubTrip == 0)
                            {
                                Stop truckLocation = Stop.GetStop(this.Vehicle.DefaultStop.Trim());
                                if (this.planTruckSubTrips.Count > 0)
                                {
                                    truckLocation = this.planTruckSubTrips[this.planTruckSubTrips.Count - 1].EndStop;
                                }
                                newPlanTruckSubTrip.End = newPlanTruckSubTrip.Start.AddMinutes(CalculateEstimatedTimeForJobTrip(truckLocation, newPlanTruckSubTrip, con, tran));
                                //add new PlanTruckSubTrip to database
                                AddPlanTruckSubTrip(newPlanTruckSubTrip, frmName, userID, con, tran);
                                newPlanTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(newPlanTruckSubTrip, con, tran);
                                //add plan sub trip to collection in memory
                                this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                            }
                            else
                            {
                                //int sameDestinationCount = this.planTruckSubTrips.Where(ptst => ptst.EndStop == jobTrip.EndStop).Count();
                                if (lastEndStop.Code.Trim() != jobTrip.EndStop.Code.Trim())
                                    CreateNextPlanTruckSubTrip(startTime, lastEndStop, newPlanTruckSubTripJob, newPlanTruckSubTrip.End, newPlanTruckSubTrip.End.AddHours(1), frmName, userID, deptCode, con, tran);
                                else
                                {
                                    this.planTruckSubTrips.SortByStartTime();
                                    //transfer preceeding
                                    foreach (PlanTruckSubTrip previousSubTrip in this.pPlanTruckSubTrips)
                                    {
                                        //if (previousSubTrip.StartStop.Code != newPlanTruckSubTripJob.startStop.Code)
                                        //if (previousSubTrip.Status != JobTripStatus.Completed && previousSubTrip.Start >= startTime)
                                        //{
                                        PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                                        newPlanTruckSubTripJob.planSubTripSeqNo = previousSubTrip.SeqNo;
                                        PlanTruckDAL.AddPlanTruckSubTripJobDatabase(this, previousSubTrip, newPlanTruckSubTripJob, con, tran, out outPlanTruckSubTripJob, userID, frmName, true);
                                        previousSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);
                                        //}
                                        ////update endtime
                                        //previousSubTrip.End = 
                                        //PlanTruckDAL.UpdatePlanTruckSubTripStartAndEndTime(previousSubTrip, con, tran);
                                    }
                                }
                            }
                        }
                        lastEndStop = jobTrip.EndStop.Clone() as Stop;
                        addedPlanSubTrip++;
                    }
                    this.planTruckSubTrips.SortByStartTime();
                    tran.Commit();
                }
            }
            catch (FMException ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw ex;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return newPlanTruckSubTrip;
        }
        //this method will be use for OTM jobs when drag/drop from selected Jobtrips or assign from the MAP
        public PlanTruckSubTrip CreatePlanTruckSubTripForOTM(SortableList<TruckJobTrip> truckJobTrips, decimal actualWeight, decimal actualVolume, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                using (con)
                {
                    //Vehicle truck = Vehicle.GetVehicle(this.Driver.defaultVehicleNumber);
                    if (con.State != ConnectionState.Open) { con.Open(); };
                    tran = con.BeginTransaction();
                    List<PlanTruckSubTrip> tempPlanSubTrips = new List<PlanTruckSubTrip>();
                    Stop lastEndStop = null;
                    DateTime lastEndDateTime = endTime;
                    int firstPlanTruckSubTripSeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);

                    //create plan trucksubtrip
                    #region PlanTruckSubTrip
                    newPlanTruckSubTrip = new PlanTruckSubTrip();
                    newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                    newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                    newPlanTruckSubTrip.Start = startTime;
                    newPlanTruckSubTrip.End = endTime;
                    newPlanTruckSubTrip.StartStop = truckJobTrips[0].StartStop;
                    newPlanTruckSubTrip.EndStop = truckJobTrips[0].EndStop;
                    newPlanTruckSubTrip.IsBillableTrip = true;
                    newPlanTruckSubTrip.Description = "";
                    newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                    newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                    newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                    #endregion

                    foreach (TruckJobTrip jobTrip in truckJobTrips)
                    {
                        if (jobTrip.TripStatus == JobTripStatus.Completed)
                            throw new FMException("Opps! Job trip already completed. ");
                        if (actualVolume > TruckJobTripPlan.GetUnassignedTruckJobTripVolume(jobTrip, con, tran))
                            throw new FMException("Opps! you keyed in volume more than the unassigned cargo volume. ");
                        if (actualWeight > TruckJobTripPlan.GetUnAssignedTruckJobTripWeight(jobTrip, con, tran))
                            throw new FMException("Opps! you keyed in weight more than the unassigned cargo weight. ");

                        //create plantrucksubtripjob
                        PlanTruckSubTripJob newPlanTruckSubTripJob = newPlanTruckSubTrip.CreatePlanTruckSubTripJob(jobTrip, actualVolume, actualWeight);
                        newPlanTruckSubTripJob.planSubTripJobSeqNo = newPlanTruckSubTrip.planTruckSubTripJobs.Count + 1;
                        //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                        newPlanTruckSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);
                    }
                    Stop truckLocation = Stop.GetStop(this.Vehicle.DefaultStop.Trim());
                    if (this.planTruckSubTrips.Count > 0)
                    {
                        truckLocation = this.planTruckSubTrips[this.planTruckSubTrips.Count - 1].EndStop;
                    }
                    //20170425 - gerry replaced
                    //newPlanTruckSubTrip.End = newPlanTruckSubTrip.Start.AddMinutes(CalculateEstimatedTimeForJobTrip(truckLocation, newPlanTruckSubTrip));
                    newPlanTruckSubTrip.Start = GetStartTimeForFixedDelivery(newPlanTruckSubTrip, truckLocation, truckJobTrips[0], endTime, con, tran);
                    newPlanTruckSubTrip.End = GetEndTimeForFixedPickup(newPlanTruckSubTrip, truckJobTrips[0], newPlanTruckSubTrip.Start, con, tran); 
                    //20170425 end

                    tempPlanSubTrips.Add(newPlanTruckSubTrip);
                    if (newPlanTruckSubTrip.planTruckSubTripJobs.Count > 1)
                    {
                        var remainingPlanTruckSubTripJobs = newPlanTruckSubTrip.planTruckSubTripJobs.Where(pstj => pstj.endStop.Code != newPlanTruckSubTrip.EndStop.Code).ToList();
                        CreateNextPlanTruckSubTripInMemory(ref tempPlanSubTrips, newPlanTruckSubTrip.SeqNo, newPlanTruckSubTrip.EndStop, remainingPlanTruckSubTripJobs, newPlanTruckSubTrip.End, newPlanTruckSubTrip.End.AddHours(1));
                    }
                    //add to database
                    foreach (PlanTruckSubTrip tempSubTrip in tempPlanSubTrips)
                    {
                        //add new PlanTruckSubTrip to database
                        AddPlanTruckSubTrip(tempSubTrip, frmName, userID, con, tran, true);
                        tempSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(newPlanTruckSubTrip, con, tran);
                        this.planTruckSubTrips.Add(tempSubTrip);
                    }
                    this.planTruckSubTrips.SortByStartTime();
                    tran.Commit();
                }
            }
            catch (FMException ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw ex;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return newPlanTruckSubTrip;
        }
        //this method will create plan sub trips from drag/drop
        public PlanTruckSubTrip CreatePlanTruckSubTripForMTO(SortableList<TruckJobTrip> truckJobTrips, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                using (con)
                {
                    Vehicle truck = Vehicle.GetVehicle(this.Driver.defaultVehicleNumber);
                    Stop truckLocation = Stop.GetStop(this.Vehicle.DefaultStop.Trim());
                    tran = con.BeginTransaction();
                    SortableList<PlanTruckSubTrip> tempPlanSubTrips = new SortableList<PlanTruckSubTrip>();
                    Stop lastEndStop = null;
                    DateTime lastEndDateTime = endTime;
                    int addedPlanSubTrip = 0;
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    foreach (TruckJobTrip jobTrip in truckJobTrips)
                    {
                        //create plan trucksubtrip
                        #region PlanTruckSubTrip
                        newPlanTruckSubTrip = new PlanTruckSubTrip();
                        newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                        newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                        newPlanTruckSubTrip.Start = startTime;
                        newPlanTruckSubTrip.End = endTime;
                        newPlanTruckSubTrip.StartStop = jobTrip.StartStop;
                        newPlanTruckSubTrip.EndStop = jobTrip.EndStop;
                        newPlanTruckSubTrip.IsBillableTrip = true;
                        newPlanTruckSubTrip.Description = "";
                        newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                        newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                        newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                        #endregion
                        if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                        {
                            string overloadMsg = string.Empty;
                            //create plantrucksubtripjob
                            PlanTruckSubTripJob newPlanTruckSubTripJob = newPlanTruckSubTrip.CreatePlanTruckSubTripJob(jobTrip, 0, 0);
                            if (!newPlanTruckSubTrip.IsPlanTruckSubTripOverload(newPlanTruckSubTripJob, truck))
                            {
                                //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                                newPlanTruckSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);
                                if (this.planTruckSubTrips.Count > 0)
                                {
                                    //set newPlanTruckSubTrip time
                                    newPlanTruckSubTrip.Start = this.planTruckSubTrips[this.planTruckSubTrips.Count - 1].End;
                                    //newPlanTruckSubTrip.End = newPlanTruckSubTrip.Start.AddHours(1);//20170425
                                    newPlanTruckSubTrip.End = GetEndTimeForFixedPickup(newPlanTruckSubTrip, jobTrip, newPlanTruckSubTrip.Start, con, tran);
                                }
                                else///20170425 - auto calculate time
                                {
                                    newPlanTruckSubTrip.Start = GetStartTimeForFixedDelivery(newPlanTruckSubTrip, truckLocation, jobTrip, endTime, con, tran);
                                    newPlanTruckSubTrip.End = GetEndTimeForFixedDelivery(newPlanTruckSubTrip, jobTrip, endTime, con, tran);
                                }
                                //add new PlanTruckSubTrip to database                                    
                                AddPlanTruckSubTrip(newPlanTruckSubTrip, frmName, userID, con, tran);
                                newPlanTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(newPlanTruckSubTrip, con, tran);
                                //get preceeding plan subtrip jobs
                                if (this.planTruckSubTrips.Count > 0)
                                {
                                    var preceedingPlanSubTrip = this.planTruckSubTrips[this.planTruckSubTrips.Count - 1];
                                    if (preceedingPlanSubTrip != null && preceedingPlanSubTrip.Status != JobTripStatus.Completed)
                                    {
                                        //update preceeding plan subtrip endstop
                                        preceedingPlanSubTrip.EndStop = jobTrip.StartStop;
                                        UpdatePlanTruckSubTripStopsAndTimeInThesameDriver(preceedingPlanSubTrip, preceedingPlanSubTrip.Start, preceedingPlanSubTrip.End, userID, frmName, dept, con, tran);

                                        foreach (var pstj in preceedingPlanSubTrip.planTruckSubTripJobs)
                                        {
                                            if (pstj.endStop.Code.Trim() != preceedingPlanSubTrip.EndStop.Code.Trim())
                                            {
                                                PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                                                PlanTruckSubTripJob previousPlanTruckSubTripJob = pstj;
                                                previousPlanTruckSubTripJob.planSubTripSeqNo = newPlanTruckSubTrip.SeqNo;
                                                PlanTruckDAL.AddPlanTruckSubTripJobDatabase(this, newPlanTruckSubTrip, previousPlanTruckSubTripJob, con, tran, out outPlanTruckSubTripJob, userID, frmName, true);
                                                newPlanTruckSubTrip.planTruckSubTripJobs.Add(outPlanTruckSubTripJob);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //add plan sub trip to collection in memory
                        this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                    }
                    this.planTruckSubTrips.SortByStartTime();
                    tran.Commit();
                }
            }
            catch (FMException ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw ex;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return newPlanTruckSubTrip;
        }
        //this method will create plan sub trips from drag/drop
        public PlanTruckSubTrips CreatePlanTruckSubTripsForMTOInMemory(SortableList<TruckJobTrip> truckJobTrips, DateTime startTime, DateTime endTime)
        {
            PlanTruckSubTrips planTruckSubTrips = new PlanTruckSubTrips();
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                using (con)
                {
                    Vehicle truck = Vehicle.GetVehicle(this.Driver.defaultVehicleNumber);
                    if (con.State != ConnectionState.Open) { con.Open(); };
                    tran = con.BeginTransaction();
                    Stop lastEndStop = null;
                    DateTime lastEndDateTime = endTime;
                    int addedPlanSubTrip = 0;
                    Stop truckLocation = Stop.GetStop(this.Vehicle.DefaultStop.Trim());
                    foreach (TruckJobTrip jobTrip in truckJobTrips)
                    {
                        //create plan trucksubtrip
                        #region PlanTruckSubTrip
                        newPlanTruckSubTrip = new PlanTruckSubTrip();
                        newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                        newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                        newPlanTruckSubTrip.Start = startTime;
                        newPlanTruckSubTrip.End = endTime;
                        newPlanTruckSubTrip.StartStop = jobTrip.StartStop;
                        newPlanTruckSubTrip.EndStop = jobTrip.EndStop;
                        newPlanTruckSubTrip.IsBillableTrip = true;
                        newPlanTruckSubTrip.Description = "";
                        newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                        newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                        newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                        #endregion
                        if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                        {
                            string overloadMsg = string.Empty;
                            //create plantrucksubtripjob
                            PlanTruckSubTripJob newPlanTruckSubTripJob = newPlanTruckSubTrip.CreatePlanTruckSubTripJob(jobTrip, 0, 0);
                            if (!newPlanTruckSubTrip.IsPlanTruckSubTripOverload(newPlanTruckSubTripJob, truck))
                            {
                                //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                                newPlanTruckSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);
                                if (planTruckSubTrips.Count > 0)
                                {
                                    //set newPlanTruckSubTrip time
                                    newPlanTruckSubTrip.Start = planTruckSubTrips[planTruckSubTrips.Count - 1].End;
                                    //newPlanTruckSubTrip.End = newPlanTruckSubTrip.Start.AddHours(1);//20170425
                                    newPlanTruckSubTrip.End = GetEndTimeForFixedPickup(newPlanTruckSubTrip, jobTrip, newPlanTruckSubTrip.Start, con, tran);
                                }
                                else///20170425 - auto calculate time
                                {
                                    newPlanTruckSubTrip.Start = GetStartTimeForFixedDelivery(newPlanTruckSubTrip, truckLocation, jobTrip, endTime, con, tran);
                                    newPlanTruckSubTrip.End = GetEndTimeForFixedDelivery(newPlanTruckSubTrip, jobTrip, endTime, con, tran);
                                }
                                //add new PlanTruckSubTrip to database                                    
                                //AddPlanTruckSubTrip(newPlanTruckSubTrip, frmName, userID, con, tran);
                                //newPlanTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(newPlanTruckSubTrip, con, tran);
                                //get preceeding plan subtrip jobs
                                if (planTruckSubTrips.Count > 0)
                                {
                                    var preceedingPlanSubTrip = planTruckSubTrips[planTruckSubTrips.Count - 1];
                                    if (preceedingPlanSubTrip != null && preceedingPlanSubTrip.Status != JobTripStatus.Completed)
                                    {
                                        //update preceeding plan subtrip endstop
                                        preceedingPlanSubTrip.EndStop = jobTrip.StartStop;
                                        //UpdatePlanTruckSubTripStopsAndTimeInThesameDriver(preceedingPlanSubTrip, preceedingPlanSubTrip.Start, preceedingPlanSubTrip.End, userID, frmName, dept, con, tran);
                                        foreach (var pstj in preceedingPlanSubTrip.planTruckSubTripJobs)
                                        {
                                            if (pstj.endStop.Code.Trim() != preceedingPlanSubTrip.EndStop.Code.Trim())
                                            {
                                                PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                                                PlanTruckSubTripJob previousPlanTruckSubTripJob = pstj;
                                                previousPlanTruckSubTripJob.planSubTripSeqNo = newPlanTruckSubTrip.SeqNo;
                                                //PlanTruckDAL.AddPlanTruckSubTripJobDatabase(this, newPlanTruckSubTrip, previousPlanTruckSubTripJob, con, tran, out outPlanTruckSubTripJob, userID, frmName, true);
                                                newPlanTruckSubTrip.planTruckSubTripJobs.Add(outPlanTruckSubTripJob);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //add plan sub trip to collection in memory
                        planTruckSubTrips.Add(newPlanTruckSubTrip);
                    }
                    planTruckSubTrips.SortByStartTime();
                    tran.Commit();
                }
            }
            catch (FMException ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw ex;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return planTruckSubTrips;
        }

        //this method will be use in drag/drop from chart
        public PlanTruckSubTrip CreatePlanTruckSubTrip(SortableList<TruckJobTrip> truckJobTrips, decimal actualWeight, decimal actualVolume, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode, SqlConnection con, SqlTransaction tran)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            try
            {
                var tempTruckJobTrips = truckJobTrips.OrderBy(jt => jt.StartTime).ToList(); // sort by time
                string firstEndLocation = tempTruckJobTrips[0].StartStop.Code.ToString().Trim(); // get the first destination
                SortableList<PlanTruckSubTrip> tempPlanSubTrips = new SortableList<PlanTruckSubTrip>();
                foreach (TruckJobTrip jobTrip in tempTruckJobTrips)
                {
                    if (jobTrip.TripStatus == JobTripStatus.Completed)
                        throw new FMException("Opps! Job trip already completed. ");
                    if (actualVolume > TruckJobTripPlan.GetUnassignedTruckJobTripVolume(jobTrip, con, tran))
                        throw new FMException("Opps! you keyed in volume more than the unassigned cargo volume. ");
                    if (actualWeight > TruckJobTripPlan.GetUnAssignedTruckJobTripWeight(jobTrip, con, tran))
                        throw new FMException("Opps! you keyed in weight more than the unassigned cargo weight. ");

                    //create plan trucksubtrip
                    #region PlanTruckSubTrip
                    newPlanTruckSubTrip = new PlanTruckSubTrip();
                    newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                    newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                    newPlanTruckSubTrip.Start = startTime;
                    newPlanTruckSubTrip.End = endTime;
                    newPlanTruckSubTrip.StartStop = jobTrip.StartStop;
                    newPlanTruckSubTrip.EndStop = jobTrip.EndStop;
                    newPlanTruckSubTrip.IsBillableTrip = true;
                    newPlanTruckSubTrip.Description = "";
                    newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                    newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                    newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                    #endregion
                    if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                    {
                        //add plan sub trip to collection in memory
                        if (!this.planTruckSubTrips.Contains(newPlanTruckSubTrip))
                        {
                            this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                            this.planTruckSubTrips.SortByStartTime();
                        }
                        //create plantrucksubtripjob
                        PlanTruckSubTripJob newPlanTruckSubTripJob = newPlanTruckSubTrip.CreatePlanTruckSubTripJob(jobTrip, actualVolume, actualWeight);

                        //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                        newPlanTruckSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);
                    }
                }
                //add new PlanTruckSubTrip to database
                AddPlanTruckSubTrip(newPlanTruckSubTrip, frmName, userID, con, tran);
                newPlanTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(newPlanTruckSubTrip, con, tran);
                //this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                this.planTruckSubTrips.SortByStartTime();

            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
            return newPlanTruckSubTrip;
        }
        //this method will be use in when adding job trip inside plantrucksubtrip with different end stop
        public PlanTruckSubTrip CreateNextPlanTruckSubTrip(DateTime firsStartTime, Stop lastStop, PlanTruckSubTripJob planTruckSubTripJob, DateTime startTime, DateTime endTime, string frmName, string userID,
            string deptCode, SqlConnection con, SqlTransaction tran)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            try
            {
                this.planTruckSubTrips.SortByStartTime();
                //create plan trucksubtrip
                #region PlanTruckSubTrip
                newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran) + 1;
                newPlanTruckSubTrip.Start = startTime;
                newPlanTruckSubTrip.End = endTime;
                newPlanTruckSubTrip.StartStop = lastStop;
                newPlanTruckSubTrip.EndStop = planTruckSubTripJob.endStop;
                newPlanTruckSubTrip.IsBillableTrip = true;
                newPlanTruckSubTrip.Description = "";
                newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                #endregion
                if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                {
                    //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                    if (planTruckSubTripJob != null)
                        newPlanTruckSubTrip.planTruckSubTripJobs.Add(planTruckSubTripJob);
                    //add plan sub trip to collection in memory
                    if (!this.planTruckSubTrips.Contains(newPlanTruckSubTrip))
                    {
                        if (this.planTruckSubTrips.Count > 0)
                        {
                            newPlanTruckSubTrip.Start = this.planTruckSubTrips[this.planTruckSubTrips.Count - 1].End;
                            //20161205 - gerry modified
                            newPlanTruckSubTrip.End = newPlanTruckSubTrip.Start.AddMinutes(CalculateEstimatedTimeForJobTrip(lastStop,newPlanTruckSubTrip, con, tran)); // newPlanTruckSubTrip.Start.AddHours(1);
                        }
                        //add to previous sub trip
                        foreach (PlanTruckSubTrip previousSubTrip in this.pPlanTruckSubTrips)
                        {
                            //if (previousSubTrip.StartStop.Code != planTruckSubTripJob.startStop.Code)
                            if (previousSubTrip.Status != JobTripStatus.Completed && previousSubTrip.Start >= firsStartTime)
                            {
                                PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                                planTruckSubTripJob.planSubTripSeqNo = previousSubTrip.SeqNo;
                                PlanTruckDAL.AddPlanTruckSubTripJobDatabase(this, previousSubTrip, planTruckSubTripJob, con, tran, out outPlanTruckSubTripJob, userID, frmName, true);
                                previousSubTrip.planTruckSubTripJobs.Add(outPlanTruckSubTripJob);
                            }
                        }
                        //add to database
                        AddPlanTruckSubTrip(newPlanTruckSubTrip, frmName, userID, con, tran, true);
                        newPlanTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(newPlanTruckSubTrip, con, tran);
                        this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                    }
                }
                //this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                //this.planTruckSubTrips.SortByStartTime();
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
            return newPlanTruckSubTrip;
        }
        //this method will be use in GMap to check the selected Jobtrips
        public bool CreatePlanTruckSubTripInMemoryForOTM(List<TruckJobTrip> truckJobTrips, decimal actualWeight, decimal actualVolume, DateTime startTime, DateTime endTime)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = null;
            try
            {
                //var tempTruckJobTrips = truckJobTrips.OrderBy(jt => jt.StartTime).ToList(); // sort by time
                //string firstEndLocation = tempTruckJobTrips[0].StartStop.Code.ToString().Trim(); // get the first destination
                SortableList<PlanTruckSubTrip> tempPlanSubTrips = new SortableList<PlanTruckSubTrip>();
                Stop lastEndStop = null;
                DateTime lastEndDateTime = endTime;
                int firstPlanTruckSubTripSeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo);
                int addedPlanSubTrip = 0;
                foreach (TruckJobTrip jobTrip in truckJobTrips)
                {
                    if (jobTrip.TripStatus == JobTripStatus.Completed)
                        throw new FMException("Opps! Job trip already completed. ");
                    if (actualVolume > TruckJobTripPlan.GetUnassignedTruckJobTripVolume(jobTrip))
                        throw new FMException("Opps! you keyed in volume more than the unassigned cargo volume. ");
                    if (actualWeight > TruckJobTripPlan.GetUnAssignedTruckJobTripWeight(jobTrip))
                        throw new FMException("Opps! you keyed in weight more than the unassigned cargo weight. ");

                    //create plan trucksubtrip
                    #region PlanTruckSubTrip
                    newPlanTruckSubTrip = new PlanTruckSubTrip();
                    newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                    newPlanTruckSubTrip.SeqNo = firstPlanTruckSubTripSeqNo;
                    newPlanTruckSubTrip.Start = startTime;
                    newPlanTruckSubTrip.End = endTime;
                    newPlanTruckSubTrip.StartStop = jobTrip.StartStop; //truckJobTrips[0].StartStop;//
                    newPlanTruckSubTrip.EndStop = jobTrip.EndStop; //truckJobTrips[0].EndStop; //
                    newPlanTruckSubTrip.IsBillableTrip = true;
                    newPlanTruckSubTrip.Description = "";
                    newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                    newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                    newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                    #endregion
                    if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                    {
                        //create plantrucksubtripjob
                        PlanTruckSubTripJob newPlanTruckSubTripJob = newPlanTruckSubTrip.CreatePlanTruckSubTripJob(jobTrip, actualVolume, actualWeight);
                        //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                        newPlanTruckSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);
                        if (addedPlanSubTrip == 0)
                        {   //add plan sub trip to collection in memory
                            this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                            this.planTruckSubTrips.SortByStartTime();
                        }
                        else
                        {
                            //int sameDestinationCount = this.planTruckSubTrips.Where(ptst => ptst.EndStop == jobTrip.EndStop).Count();
                            if (lastEndStop.Code.Trim() != jobTrip.EndStop.Code.Trim())
                                CreateNextPlanTruckSubTripInMemory(firstPlanTruckSubTripSeqNo, lastEndStop, newPlanTruckSubTripJob, lastEndDateTime, lastEndDateTime.AddHours(1));
                            else
                            {
                                this.planTruckSubTrips.SortByStartTime();
                                //add to previous sub trip
                                foreach (PlanTruckSubTrip previousSubTrip in this.pPlanTruckSubTrips)
                                {
                                    //if (previousSubTrip.StartStop.Code != newPlanTruckSubTripJob.startStop.Code)
                                    //if (previousSubTrip.Status != JobTripStatus.Completed && previousSubTrip.Start >= startTime)
                                    //{
                                    PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                                    newPlanTruckSubTripJob.planSubTripSeqNo = previousSubTrip.SeqNo;
                                    newPlanTruckSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);
                                    //}
                                }
                            }
                        }
                    }
                    lastEndStop = (Stop)jobTrip.EndStop.Clone();
                    addedPlanSubTrip++;
                }
                this.planTruckSubTrips.SortByStartTime();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return true;
        }
        //this method will be use in when adding job trip inside plantrucksubtrip with different end stop
        public PlanTruckSubTrip CreateNextPlanTruckSubTripInMemory(int lastSeqNo, Stop lastStop, PlanTruckSubTripJob planTruckSubTripJob, DateTime startTime, DateTime endTime)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            try
            {
                //this.planTruckSubTrips.SortByStartTime();
                //create plan trucksubtrip
                #region PlanTruckSubTrip
                newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                newPlanTruckSubTrip.SeqNo = lastSeqNo + 1;
                newPlanTruckSubTrip.Start = startTime;
                newPlanTruckSubTrip.End = endTime;
                newPlanTruckSubTrip.StartStop = lastStop;
                newPlanTruckSubTrip.EndStop = planTruckSubTripJob.endStop;
                newPlanTruckSubTrip.IsBillableTrip = true;
                newPlanTruckSubTrip.Description = "";
                newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                #endregion
                if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                {
                    //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                    if (planTruckSubTripJob != null)
                        newPlanTruckSubTrip.planTruckSubTripJobs.Add(planTruckSubTripJob);
                    //add plan sub trip to collection in memory
                    if (!this.planTruckSubTrips.Contains(newPlanTruckSubTrip))
                    {
                        if (this.planTruckSubTrips.Count > 0)
                        {
                            newPlanTruckSubTrip.Start = this.planTruckSubTrips[this.planTruckSubTrips.Count - 1].End;
                            newPlanTruckSubTrip.End = newPlanTruckSubTrip.Start.AddHours(1);
                        }
                        //insert to previous sub trip
                        foreach (PlanTruckSubTrip previousSubTrip in this.pPlanTruckSubTrips)
                        {
                            if (previousSubTrip.Status != JobTripStatus.Completed && previousSubTrip.Start >= startTime)
                                previousSubTrip.planTruckSubTripJobs.Add(planTruckSubTripJob);
                        }
                        this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                    }
                }
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
            return newPlanTruckSubTrip;
        }
        //20161206 - this method will be use in when adding job trip inside plantrucksubtrip with different end stop
        public void CreateNextPlanTruckSubTripInMemory(ref List<PlanTruckSubTrip> tempPlanSubTrips, int lastSeqNo, Stop lastStop, List<PlanTruckSubTripJob> planTruckSubTripJobs, DateTime startTime, DateTime endTime)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            try
            {
                //this.planTruckSubTrips.SortByStartTime();
                //create plan trucksubtrip
                #region PlanTruckSubTrip
                newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                newPlanTruckSubTrip.SeqNo = lastSeqNo + 1;
                newPlanTruckSubTrip.Start = startTime;
                newPlanTruckSubTrip.End = endTime;
                newPlanTruckSubTrip.StartStop = lastStop;
                newPlanTruckSubTrip.EndStop = planTruckSubTripJobs[0].endStop;
                newPlanTruckSubTrip.IsBillableTrip = true;
                newPlanTruckSubTrip.Description = "";
                newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                #endregion
                int pstj_seqNo = 1;
                if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                {
                    foreach (PlanTruckSubTripJob pstj in planTruckSubTripJobs)
                    {
                        pstj.planSubTripSeqNo = newPlanTruckSubTrip.SeqNo;
                        pstj.planSubTripJobSeqNo = pstj_seqNo;

                        newPlanTruckSubTrip.planTruckSubTripJobs.Add(pstj);
                        pstj_seqNo++;
                    }
                    newPlanTruckSubTrip.End = newPlanTruckSubTrip.Start.AddMinutes(CalculateEstimatedTimeForJobTrip(lastStop, newPlanTruckSubTrip));
                    tempPlanSubTrips.Add(newPlanTruckSubTrip);
                    if (newPlanTruckSubTrip.planTruckSubTripJobs.Count > 1)
                    {
                        var temp = newPlanTruckSubTrip.planTruckSubTripJobs.Where(pstj => pstj.endStop.Code != newPlanTruckSubTrip.EndStop.Code).ToList();
                        CreateNextPlanTruckSubTripInMemory(ref tempPlanSubTrips, newPlanTruckSubTrip.SeqNo, newPlanTruckSubTrip.EndStop, temp, newPlanTruckSubTrip.End, newPlanTruckSubTrip.End.AddHours(1));
                    }
                }
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw; }
        }
        //this method will create plan sub trips from GMap
        public PlanTruckSubTrip CreatePlanTruckSubTripForOTO(TruckJobTrip truckJobTrip, decimal actualWeight, decimal actualVolume, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode, PlanTruckSubTripJob planTruckSubTripJob = null)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    using (con)
                    {
                        //Vehicle truck = Vehicle.GetVehicle(this.Driver.defaultVehicleNumber);
                        if (con.State != ConnectionState.Open) { con.Open(); };
                        tran = con.BeginTransaction();

                        newPlanTruckSubTrip = CreatePlanTruckSubTrip(truckJobTrip, actualWeight, actualVolume, startTime, endTime, frmName, userID, dept, con, tran, planTruckSubTripJob);
                        tran.Commit();
                    }
                }
                catch (FMException ex)
                {
                    if (tran != null) { tran.Rollback(); }
                    throw ex;
                }
                catch (Exception ex)
                {
                    if (tran != null) { tran.Rollback(); }
                    throw new FMException(ex.ToString());
                }
                finally { con.Close(); }
            }
            return newPlanTruckSubTrip;
        }
        
        //this method will be use in drag/drop from chart
        public PlanTruckSubTrip CreatePlanTruckSubTrip(TruckJobTrip truckJobTrip, decimal actualWeight, decimal actualVolume, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode, SqlConnection con, SqlTransaction tran, PlanTruckSubTripJob planTruckSubTripJob = null)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            try
            {
                SortableList<PlanTruckSubTrip> tempPlanSubTrips = new SortableList<PlanTruckSubTrip>();
                if (truckJobTrip.TripStatus == JobTripStatus.Completed)
                    throw new FMException("Opps! Job trip already completed. ");
                if (actualVolume > TruckJobTripPlan.GetUnassignedTruckJobTripVolume(truckJobTrip, con, tran))
                    throw new FMException("Opps! you keyed in volume more than the unassigned cargo volume. ");
                if (actualWeight > TruckJobTripPlan.GetUnAssignedTruckJobTripWeight(truckJobTrip, con, tran))
                    throw new FMException("Opps! you keyed in weight more than the unassigned cargo weight. ");

                //create plan trucksubtrip
                #region PlanTruckSubTrip
                newPlanTruckSubTrip = new PlanTruckSubTrip();
                newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                newPlanTruckSubTrip.Start = startTime;
                newPlanTruckSubTrip.End = endTime;
                newPlanTruckSubTrip.StartStop = truckJobTrip.StartStop;
                newPlanTruckSubTrip.EndStop = truckJobTrip.EndStop;
                newPlanTruckSubTrip.IsBillableTrip = true;
                newPlanTruckSubTrip.Description = "";
                newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                #endregion
                //create plantrucksubtripjob
                PlanTruckSubTripJob newPlanTruckSubTripJob = newPlanTruckSubTrip.CreatePlanTruckSubTripJob(truckJobTrip, actualVolume, actualWeight);
                if(planTruckSubTripJob == null)
                    planTruckSubTripJob = newPlanTruckSubTrip.CreatePlanTruckSubTripJob(truckJobTrip, actualVolume, actualWeight);
                
                //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection 
                newPlanTruckSubTrip.planTruckSubTripJobs.Add(planTruckSubTripJob);

                #region 20161206  - gerry added to calculate estimated time to finish the job
                Stop truckLocation = Stop.GetStop(this.Vehicle.DefaultStop.Trim());
                if (this.planTruckSubTrips.Count > 0)
                {
                    truckLocation = this.planTruckSubTrips[this.planTruckSubTrips.Count - 1].EndStop;
                }
                //get the office starting hour
                //ApplicationOption startWorkOption = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_START_WORKING_HOUR);
                //if (startWorkOption.setting_Value == "")
                //    newPlanTruckSubTrip.Start = newPlanTruckSubTrip.Start.AddHours(Convert.ToInt32(startWorkOption.setting_Value.ToString()));

                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_CALCULATE_JOB_TIME);
                //20161213
                if (option.setting_Value == "T")
                {
                    if (truckJobTrip.isFixedTime)
                    {
                        int startHr = Convert.ToInt32(truckJobTrip.StartTime.ToString().Substring(0, 2).ToString());
                        int startMin = Convert.ToInt32(truckJobTrip.StartTime.ToString().Substring(2, 2).ToString());
                        DateTime pickupTime = newPlanTruckSubTrip.Start.Date.AddHours(startHr).AddMinutes(startMin);
                        int endHr = Convert.ToInt32(truckJobTrip.EndTime.ToString().Substring(0, 2).ToString());
                        int endMin = Convert.ToInt32(truckJobTrip.EndTime.ToString().Substring(2, 2).ToString());
                        DateTime deliveryTime = newPlanTruckSubTrip.End.Date.AddHours(endHr).AddMinutes(endMin);
                        if (truckJobTrip.timeBound == TIME_BOUND.FIXED_PICKUP)
                        {
                            newPlanTruckSubTrip.Start = GetStartTimeForFixedPickup(newPlanTruckSubTrip, truckLocation, truckJobTrip, pickupTime, con, tran);
                            newPlanTruckSubTrip.End = GetEndTimeForFixedPickup(newPlanTruckSubTrip, truckJobTrip, pickupTime, con, tran);
                        }
                        else if (truckJobTrip.timeBound == TIME_BOUND.FIXED_DELIVERY)
                        {
                            newPlanTruckSubTrip.Start = GetStartTimeForFixedDelivery(newPlanTruckSubTrip, truckLocation, truckJobTrip, deliveryTime, con, tran);
                            newPlanTruckSubTrip.End = GetEndTimeForFixedDelivery(newPlanTruckSubTrip, truckJobTrip, deliveryTime, con, tran);
                        }
                    }
                    else
                    {
                        if (truckJobTrip.timeBound.ToString().Contains("PICKUP"))
                        {
                            newPlanTruckSubTrip.Start = GetStartTimeForFixedPickup(newPlanTruckSubTrip, truckLocation, truckJobTrip, startTime, con, tran);
                            newPlanTruckSubTrip.End = GetEndTimeForFixedPickup(newPlanTruckSubTrip, truckJobTrip, startTime, con, tran);
                        }
                        else if (truckJobTrip.timeBound.ToString().Contains("DELIVERY"))
                        {
                            newPlanTruckSubTrip.Start = GetStartTimeForFixedDelivery(newPlanTruckSubTrip, truckLocation, truckJobTrip, endTime, con, tran);
                            newPlanTruckSubTrip.End = GetEndTimeForFixedDelivery(newPlanTruckSubTrip, truckJobTrip, endTime, con, tran);
                        }
                    }
                }
                else
                {
                    newPlanTruckSubTrip.End = newPlanTruckSubTrip.Start.AddMinutes(CalculateEstimatedTimeForJobTrip(truckLocation, newPlanTruckSubTrip, con, tran));
                }
                #endregion

                //20170103 - gerry added validation
                if (IsPlanSubTripOverlapping(newPlanTruckSubTrip))
                        throw new FMException(TptResourceBLL.ErrStartTimeEndTimeOverlapWithOthers);

                if (CanAddPlanTruckSubTrip(newPlanTruckSubTrip))
                {
                    //add new PlanTruckSubTrip to database
                    AddPlanTruckSubTrip(newPlanTruckSubTrip, frmName, userID, con, tran);
                    //add plan sub trip to collection in memory
                    if (!this.planTruckSubTrips.Contains(newPlanTruckSubTrip))
                    {
                        this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                        this.planTruckSubTrips.SortByStartTime();
                    }
                    newPlanTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(newPlanTruckSubTrip, con, tran);
                    //this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                    this.planTruckSubTrips.SortByStartTime();
                }
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return newPlanTruckSubTrip;
        }
    

        //this method will be use when split Jobtrip into separate planTruckSubTripJob
        public PlanTruckSubTrip CreatePlanTruckSubTripForSplitPlanTruckSubTripJob(Stop startStop, Stop endStop, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            try
            {
                newPlanTruckSubTrip.PlanTripNo = this.PlanTripNo;
                newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo);
                newPlanTruckSubTrip.Start = startTime;
                newPlanTruckSubTrip.End = endTime;
                newPlanTruckSubTrip.StartStop = startStop;
                newPlanTruckSubTrip.EndStop = endStop;
                newPlanTruckSubTrip.IsBillableTrip = true;
                newPlanTruckSubTrip.Description = "";
                newPlanTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber;
                newPlanTruckSubTrip.DriverNumber = this.Driver.Code;
                newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                //add plan sub trip to collection in memory
                //if (!this.planTruckSubTrips.Contains(newPlanTruckSubTrip))
                //{
                //    this.planTruckSubTrips.Add(newPlanTruckSubTrip);
                //    this.planTruckSubTrips.SortByStartTime();
                //}
                newPlanTruckSubTrip.planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();
            }
            catch (FMException ex) { throw; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return newPlanTruckSubTrip;
        }

        //validate if can Drag Drop Appointment
        public bool CanDragDropAppointment(PlanTruckSubTrip planTruckSubTrip, DateTime startTime, DateTime endTime)
        {
            try
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_CALCULATE_JOB_TIME);
                
                this.planTruckSubTrips.SortByStartTime();
                // if planTruckSubTrip is completed
                if (planTruckSubTrip.Status == JobTripStatus.Completed)
                    throw new FMException(TptResourceBLL.ErrCantChangeDriverBecauseItIsCompleted);

                //check the current driver if their is the same start stop OR end stop
                if (this.Driver.Code.Trim() != planTruckSubTrip.DriverNumber.Trim())
                {
                    foreach (PlanTruckSubTrip planSubTrip in this.planTruckSubTrips)
                    {
                        if (planTruckSubTrip.StartStop.Code.Trim() == planSubTrip.StartStop.Code.Trim())
                            throw new FMException(string.Format("Driver {0} has already assigned job to this location({1})", planTruckSubTrip.DriverNumber, planTruckSubTrip.StartStop.Code.Trim()));
                        if (planTruckSubTrip.EndStop.Code.Trim() == planSubTrip.EndStop.Code.Trim())
                            throw new FMException(string.Format("Driver {0} has already assigned job to this location({1})", planTruckSubTrip.DriverNumber, planTruckSubTrip.EndStop.Code.Trim()));
                    }
                }
                //check it plan truck subtrip job inside
                for (int i = 0; i < planTruckSubTrip.planTruckSubTripJobs.Count; i++)
                {
                    // if any of the planTruckSubTripJob is completed
                    if (planTruckSubTrip.planTruckSubTripJobs[i].status == JobTripStatus.Completed)
                        throw new FMException(TptResourceBLL.ErrCantChangeDriverBecauseItIsCompleted);
                    // if any of the planTruckSubTripJob is not the start and end stop
                    if (planTruckSubTrip.planTruckSubTripJobs[i].startStop.Code != planTruckSubTrip.StartStop.Code || planTruckSubTrip.planTruckSubTripJobs[i].endStop.Code != planTruckSubTrip.EndStop.Code)
                        throw new FMException("Unable to drag/drop because there is a preceeding job inside. ");

                    // when changing driver for a planTruckSubTrip,
                    // all its planTruckSubTripJobs should start / end at the same stop of the planTruckSubTrip
                    if (planTruckSubTrip.planTruckSubTripJobs[i].startStop.Code != planTruckSubTrip.StartStop.Code && planTruckSubTrip.planTruckSubTripJobs[i].endStop.Code == planTruckSubTrip.EndStop.Code)
                    {    //if different driver throw exception
                        if (this.Driver.Code.Trim() != planTruckSubTrip.DriverNumber.Trim())
                            throw new FMException(TptResourceBLL.ErrCantChangeDriverBecauseDifferentStartStop);
                        else
                            ValidateDragAndDropTimeInSameDriver(planTruckSubTrip);
                    }
                    if (planTruckSubTrip.planTruckSubTripJobs[i].startStop.Code == planTruckSubTrip.StartStop.Code && planTruckSubTrip.planTruckSubTripJobs[i].endStop.Code != planTruckSubTrip.EndStop.Code)
                    {  //if different driver
                        if (this.Driver.Code.Trim() != planTruckSubTrip.DriverNumber.Trim())
                            throw new FMException(TptResourceBLL.ErrCantChangeDriverBecauseDifferentEndStop);
                        else
                            ValidateDragAndDropTimeInSameDriver(planTruckSubTrip);
                    }
                    //check for time
                    if (option.setting_Value == "T")
                    {
                        string outMsg = string.Empty;
                        TruckJobTrip jobTrip = TruckJobTrip.GetTruckJobTrip(planTruckSubTrip.planTruckSubTripJobs[i].jobID, planTruckSubTrip.planTruckSubTripJobs[i].jobSeq);
                        CheckBookedAndPlanTime(jobTrip, startTime, endTime, out outMsg);
                    }
                }
            }
            catch (FMException fmEx) { throw; }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }
        //validate if the same driver don't allow to swap time with other plan sub trip
        public void ValidateDragAndDropTimeInSameDriver(PlanTruckSubTrip planTruckSubTrip)
        {
            int indx = this.planTruckSubTrips.IndexOf(planTruckSubTrip);
            if (indx != 0)
            {
                if (this.planTruckSubTrips[indx - 1] != null)
                {
                    if (planTruckSubTrip.Start < this.planTruckSubTrips[indx - 1].End)
                        throw new FMException("Some preceeding job inside, you are not allowed to swap timing across other PlanSubTrip. ");
                }
            }
            if (indx != this.planTruckSubTrips.Count - 1)
            {
                if (this.planTruckSubTrips[indx + 1] != null)
                {
                    if (planTruckSubTrip.End < this.planTruckSubTrips[indx + 1].Start)
                        throw new FMException("Some preceeding job inside, you are not allowed to swap timing across other PlanSubTrip. ");
                }
            }
        }
        //this method will be use in drag/drop from planning appointment
        public void DragDropAppointment(SortableList<PlanTruckTrip> planTruckTrips, PlanTruckSubTrip planTruckSubTrip, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    //CheckTruckCapacity()
                    string errMsg = string.Empty;
                    Driver previousDriver = Driver.GetDriver(planTruckSubTrip.DriverNumber);
                    //get the previous planTruckTrip based on previous driver
                    PlanTruckTrip previousTruckTrip = FindPlanTruckTripByDriver(planTruckTrips, previousDriver);
                    if (previousTruckTrip.CanDragDropAppointment(planTruckSubTrip, startTime, endTime))
                    {
                        AuditLog.EventLog(frmName, planTruckSubTrip, planTruckSubTrip.Start, planTruckSubTrip.End, userID);
                        PlanTruckSubTrip outPlanSubTrip = new PlanTruckSubTrip();
                        #region delete subtripjob, TruckJobTripPlan, planSubtrip
                        foreach (PlanTruckSubTripJob pstj in planTruckSubTrip.planTruckSubTripJobs)
                        {
                            //TruckJobTrip
                            TruckJobTrip jobTrip = TruckJobTrip.GetTruckJobTrip(pstj.jobID, pstj.jobSeq, con, tran);
                            //delete plan subtripjob
                            planTruckSubTrip.DeletePlanTruckSubTripJob(pstj, con, tran);
                            #region 20160420 - gerry removed
                            //if (jobTrip.EndStop.Code.Trim() == planTruckSubTrip.EndStop.Code.Trim())
                            //    PlanTruckDAL.DeletePlanTruckSubTripJobDatabase(planTruckSubTrip, pstj, con, tran, out outPlanSubTrip);
                            //else //delete planSubTripJob from other planSubTrip
                            //    PlanTruckDAL.DeletePlanTruckSubTripJobDatabaseWithPreceeding(planTruckSubTrip, pstj, con, tran, out outPlanSubTrip);

                            ////TruckJobTripPlan
                            //TruckJobTripPlan truckJobTripPlan = CreateTruckJobTripPlan(pstj, startTime, endTime);
                            ////delete truckJobTripPlan
                            //jobTrip.DeleteTruckJobTripPlan(truckJobTripPlan, con, tran);
                            #endregion
                        }
                        //delete planSubtrip
                        if (this.Driver.Code.Trim() == previousDriver.Code.Trim())
                        {
                            PlanTruckDAL.DeletePlanTruckSubTrip(this, planTruckSubTrip, con, tran);
                            this.planTruckSubTrips.Remove(planTruckSubTrip);
                        }
                        else
                        {
                            PlanTruckDAL.DeletePlanTruckSubTrip(previousTruckTrip, planTruckSubTrip, con, tran);
                            previousTruckTrip.planTruckSubTrips.Remove(planTruckSubTrip);
                        }
                        #endregion

                        #region Add back subtripjob, TruckJobTripPlan, planSubtrip
                        //set new planTruckSubTrip
                        planTruckSubTrip.PlanTripNo = this.PlanTripNo;
                        planTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                        planTruckSubTrip.Start = startTime;
                        planTruckSubTrip.End = endTime;
                        planTruckSubTrip.DriverNumber = this.Driver.Code.Trim();
                        planTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber.Trim();
                        foreach (PlanTruckSubTripJob pstj in planTruckSubTrip.planTruckSubTripJobs)
                        {
                            pstj.planTripNo = this.PlanTripNo;
                            pstj.planSubTripSeqNo = planTruckSubTrip.SeqNo;
                        }
                        //add to database
                        AddPlanTruckSubTrip(planTruckSubTrip, frmName, userID, con, tran);
                        planTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(planTruckSubTrip, con, tran);
                        this.planTruckSubTrips.Add(planTruckSubTrip);
                        #endregion

                    }
                    tran.Commit();
                }
                catch (FMException ex)
                {
                    tran.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new FMException(ex.Message.ToString()); 
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public void DragDropAppointment(SortableList<PlanTruckTrip> planTruckTrips, PlanTruckSubTrip planTruckSubTrip, DateTime startTime, DateTime endTime, string frmName, string userID, string deptCode,
                                        SqlConnection con, SqlTransaction tran)
        {
            using (con)
            {
                try
                {
                    string errMsg = string.Empty;
                    Driver previousDriver = Driver.GetDriver(planTruckSubTrip.DriverNumber);
                    //get the previous planTruckTrip based on previous driver
                    PlanTruckTrip previousTruckTrip = FindPlanTruckTripByDriver(planTruckTrips, previousDriver);
                    if (previousTruckTrip.CanDragDropAppointment(planTruckSubTrip, startTime, endTime))
                    {
                        AuditLog.EventLog(frmName, planTruckSubTrip, planTruckSubTrip.Start, planTruckSubTrip.End, userID);
                        PlanTruckSubTrip outPlanSubTrip = new PlanTruckSubTrip();
                        #region delete subtripjob, TruckJobTripPlan, planSubtrip
                        foreach (PlanTruckSubTripJob pstj in planTruckSubTrip.planTruckSubTripJobs)
                        {
                            //delete plan subtripjob
                            PlanTruckDAL.DeletePlanTruckSubTripJobDatabase(planTruckSubTrip, pstj, con, tran, out outPlanSubTrip);
                            //TruckJobTripPlan
                            TruckJobTripPlan truckJobTripPlan = CreateTruckJobTripPlan(pstj, startTime, endTime);
                            //TruckJobTrip
                            TruckJobTrip jobTrip = TruckJobTrip.GetTruckJobTrip(pstj.jobID, pstj.jobSeq, con, tran);
                            //delete truckJobTripPlan
                            jobTrip.DeleteTruckJobTripPlan(truckJobTripPlan, con, tran);
                        }
                        //delete planSubtrip
                        if (this.Driver.Code.Trim() == previousDriver.Code.Trim())
                        {
                            PlanTruckDAL.DeletePlanTruckSubTrip(this, planTruckSubTrip, con, tran);
                            this.planTruckSubTrips.Remove(planTruckSubTrip);
                        }
                        else
                        {
                            PlanTruckDAL.DeletePlanTruckSubTrip(previousTruckTrip, planTruckSubTrip, con, tran);
                            previousTruckTrip.planTruckSubTrips.Remove(planTruckSubTrip);
                        }
                        #endregion

                        #region Add back subtripjob, TruckJobTripPlan, planSubtrip
                        //set new planTruckSubTrip
                        planTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(this.PlanTripNo, con, tran);
                        planTruckSubTrip.Start = startTime;
                        planTruckSubTrip.End = endTime;
                        planTruckSubTrip.DriverNumber = this.Driver.Code.Trim();
                        planTruckSubTrip.VehicleNumber = this.Driver.defaultVehicleNumber.Trim();
                        //add to database
                        AddPlanTruckSubTrip(planTruckSubTrip, frmName, userID, con, tran);
                        foreach (PlanTruckSubTripJob pstj in planTruckSubTrip.planTruckSubTripJobs)
                        {
                            pstj.planTripNo = this.PlanTripNo;
                            pstj.planSubTripSeqNo = planTruckSubTrip.SeqNo;
                        }
                        planTruckSubTrip.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(planTruckSubTrip, con, tran);
                        this.planTruckSubTrips.Add(planTruckSubTrip);
                        #endregion

                    }
                    tran.Commit();
                }
                catch (FMException ex)
                {
                    tran.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void AutoAdjustNextPlanSubTrip()
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            using (con)
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    //TODO some logic here            
                    //adjust start and end time of the remaing plansubtrips if any
                    tran.Commit();
                }
                catch (FMException ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
        }
        public void AutoAdjustNextPlanSubTrip(PlanTruckSubTrip currentPlanTruckSubTrip, int adjustedTime, string userID, string formName, string deptCode, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //TODO some logic here      
                //adjust start and end time of the remaing plansubtrips if any        
                foreach (PlanTruckSubTrip planTempTruckSubTrip in this.planTruckSubTrips)
                {
                    if (planTempTruckSubTrip.End > currentPlanTruckSubTrip.End)
                    {
                        UpdatePlanTruckSubTripStopsAndTimeInThesameDriver(planTempTruckSubTrip, planTempTruckSubTrip.Start.AddMinutes(adjustedTime),
                            planTempTruckSubTrip.End.AddMinutes(adjustedTime), userID, formName, deptCode, con, tran);
                    }
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.Message.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public PlanTruckSubTrip FindPlanTruckSubTrip(DateTime startTime, DateTime endTime)
        {
            foreach (PlanTruckSubTrip subTrip in this.planTruckSubTrips)
            {
                if (subTrip.Start == startTime && subTrip.End == endTime)
                {
                    return subTrip;
                }
            }
            return null;
        }
        public static bool UpdatePlanTripGeoLocation(PlanTruckTrip planTruckTrip)
        {
            return PlanTruckDAL.UpdatePlanTripGeoLocation(planTruckTrip);
        }
        //20160219 - gerry added
        public decimal GetTotalVolumeCarried()
        {
            decimal retValue = 0;
            foreach (PlanTruckSubTrip pts in this.planTruckSubTrips)
            {
                if (pts.Status != JobTripStatus.Completed)
                    retValue += pts.GetTotalVolumeCarried();
            }
            return retValue;
        }
        public decimal GetTotalWeightCarried()
        {
            decimal retValue = 0;
            foreach (PlanTruckSubTrip pts in this.planTruckSubTrips)
            {
                if (pts.Status != JobTripStatus.Completed)
                    retValue += pts.GetTotalWeightCarried();
            }
            return retValue;
        }


        #region Mapping functions
        public static void AssignJobsForOTO(SortableList<PlanTruckTrip> planTruckTrips, Dictionary<string, List<TruckJobTrip>> assignedTrucks)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            DateTime startTime = planTruckTrips[0].ScheduleDate.Date.AddHours(7);//default
            DateTime endTime = startTime.AddHours(2);//default time to finish job is 2hrs
            try
            {
                using (con)
                {
                    foreach (KeyValuePair<string, List<TruckJobTrip>> pair in assignedTrucks)
                    {
                        //get driver
                        var driver = Driver.GetDriverByVehicle(pair.Key.ToString());
                        //get plantrucktrip
                        var planTruckTrip = FindPlanTruckTripByDriver(planTruckTrips, driver);
                        //use special setting to get start working hour
                        ApplicationOption appOption = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_START_WORKING_HOUR);
                        
                        int startWorkingHour = appOption.setting_Value.ToString() == ""? 0 : Convert.ToInt32(appOption.setting_Value.ToString());
                        if (startWorkingHour > 0)
                        {
                            startTime = planTruckTrips[0].ScheduleDate.Date.AddHours(startWorkingHour);
                            endTime = startTime.AddHours(2);
                        }
                        foreach (TruckJobTrip tempJobTrip in pair.Value)
                        {
                        }
                    }
                }
            }
            catch (FMException ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw ex;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw new FMException(ex.ToString());
            }
        }
        #endregion


        #region CalculateEstimatedTimeForJobTrip
        /// <summary>
        /// Job Time = time to travel from location of truck to Job pick point 
        ///             + job loading time
        ///             + time to travel from pickup point to delivery point
        ///             + job unloading time
        /// </summary>
        /// <returns></returns>
        public static double CalculateEstimatedTimeForJobTrip(Stop truckLocation, PlanTruckSubTrip subTrip)
        {
            double totalTime;
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    totalTime = CalculateEstimatedTimeForJobTrip(truckLocation, subTrip, con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return totalTime;
        }
        public static double CalculateEstimatedTimeForJobTrip(Stop truckLocation, PlanTruckSubTrip subTrip, SqlConnection con, SqlTransaction tran)
        {
            double totalTime = 60; //default 1 hour
            try
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_CALCULATE_JOB_TIME);
                if (option == null) { return totalTime; }
                if (option.setting_Value == "T")
                {
                    double loadingTime = 0;
                    double unloadingTime = 0;
                    //get travel time from truck location to pickup point
                    double timeTravelFromTruckLocationToPickupPoint = truckLocation.Code.Trim() == subTrip.StartStop.Code.Trim() ? 0 : GetTravelTimeBetweenTwoPlaces(subTrip.Start, truckLocation, subTrip.StartStop).TotalMinutes;
                    
                    foreach (PlanTruckSubTripJob subTripJob in subTrip.planTruckSubTripJobs)
                    {
                        //get estimated loading time
                        if (subTrip.StartStop.Code == subTripJob.startStop.Code)
                        {
                            loadingTime += GetTotalLoadingUnloadingTime(subTripJob, con, tran);
                        }
                        //get the estimated unloading time
                        if (subTrip.EndStop.Code == subTripJob.endStop.Code)
                        {
                            unloadingTime += GetTotalLoadingUnloadingTime(subTripJob, con, tran);
                        }
                    }
                    //get travel time from pickup point to delivery point
                    double timeTravelFromPickToDeliveryPoint = GetTravelTimeBetweenTwoPlaces(subTrip.Start, subTrip.StartStop, subTrip.EndStop).TotalMinutes;
                    //now get the total time
                    totalTime = timeTravelFromTruckLocationToPickupPoint + loadingTime + timeTravelFromPickToDeliveryPoint + unloadingTime;
                    //capture the data to be save
                    subTrip.travelTimeFromVehicleToPickup = timeTravelFromTruckLocationToPickupPoint;
                    subTrip.loadingTime = loadingTime;
                    subTrip.travelTimeFromPickupToDelivery = timeTravelFromPickToDeliveryPoint;
                    subTrip.unloadingTime = unloadingTime;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return totalTime;
        }

        //20161206 - gerry added to get time travel bet two points using yournavigation API
        public static async Task<double> GetYN_RouteTimeDurationAsync(PointLatLng StartPoint, PointLatLng EndPoint)
        {
            double duration = 0;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //GET
                    string sURL = @"http://yournavigation.org/api/dev/route.php?flat=" + StartPoint.Lat.ToString() + "&flon=" + StartPoint.Lng.ToString() + "&tlat=" + EndPoint.Lat.ToString() + "&tlon=" + EndPoint.Lng.ToString() + "&v=goods&fast=1&layer=mapnik";
                    HttpResponseMessage response = await client.GetAsync(sURL);
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var jObject = JObject.Parse(responseString);
                        var routes = JArray.Parse(jObject.GetValue("routes").ToString());
                        var route = JObject.Parse(routes[0].ToString());

                        duration = Convert.ToDouble(route.GetValue("duration").ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return duration;
        }
        //20161106 - gerry added to get time travel bet two points using yournavigation API
        public static double GetYN_RouteDistance(PointLatLng StartPoint, PointLatLng EndPoint)
        {
            double distance = 0;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    //GET
                    string sURL = @"http://yournavigation.org/api/dev/route.php?flat=" + StartPoint.Lat.ToString() + "&flon=" + StartPoint.Lng.ToString() + "&tlat=" + EndPoint.Lat.ToString() + "&tlon=" + EndPoint.Lng.ToString() + "&v=goods&fast=1&layer=mapnik";
                    HttpResponseMessage response = client.GetAsync(sURL).Result;
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var xmlDocument = new XmlDocument();
                        int start = responseString.IndexOf('X') + 1;
                        string t = responseString.Substring(start, responseString.Length - start);
                        xmlDocument.LoadXml(t);
                        //Document
                        XmlNode MotherNode = xmlDocument.DocumentElement.ChildNodes[0];
                        foreach (XmlNode ChildNode in MotherNode.ChildNodes)
                        {
                            if (ChildNode.Name == "distance")
                            {
                                distance =Convert.ToDouble(ChildNode.InnerText.ToString());
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return distance;
        }
        public static Double GetDistanceBetweenTwoPlaces(Stop lastEndStop, Stop nextStartStop)
        {
            double distance = 0;
            PointLatLng startpoint = new PointLatLng();
            PointLatLng endpoint = new PointLatLng();
            GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;
            PointLatLng? startPointPosition = GMapProviders.GoogleMap.GetPoint(lastEndStop.countryName +","+lastEndStop.Address1, out status);
            if (startPointPosition != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                startpoint = startPointPosition.Value;
            }
            PointLatLng? endPointPosition = GMapProviders.GoogleMap.GetPoint(lastEndStop.countryName + "," + nextStartStop.Address1, out status);
            if (endPointPosition != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                endpoint = endPointPosition.Value;
            }
            //now calculate duration using the map route
            //GMapRoute route = new GMapRoute("Estimate");
            //route.Points.Add(startpoint);
            //route.Points.Add(endpoint);
            distance =  GetYN_RouteDistance(startpoint, endpoint);

            return distance;
        }
        public static TimeSpan GetTravelTimeBetweenTwoPlaces(DateTime time, Stop lastEndStop, Stop nextStartStop)
        {
            DateTime actualTime = DateUtility.GetSQLDateTimeMaximumValue().Date.Add(time.TimeOfDay);
            double averageSpeed = VehicleAverageSpeed.GetAveSpeed(actualTime);

            double routeDistance = GetDistanceBetweenTwoPlaces(lastEndStop, nextStartStop);
            double duration = routeDistance / averageSpeed;
            //get time span
            TimeSpan timeTaken = TimeSpan.FromHours(duration);
            return timeTaken;
        }
        public static double GetTotalLoadingUnloadingTime(TruckJobTrip jobTrip, SqlConnection con, SqlTransaction tran)
        {
            double loadingTime = 0;
            foreach (TruckJobTripDetail jobTripDetail in jobTrip.truckJobTripDetail)
            {
                decimal vol = (jobTripDetail.length * jobTripDetail.width * jobTripDetail.height) / 1000000;
                LoadOrUnloadingTime estimatedTime = LoadOrUnloadingTime.GetLoadingTime(Convert.ToDouble(jobTripDetail.unitWeight), Convert.ToDouble(vol), con, tran);
                estimatedTime.estimatedTime = estimatedTime.estimatedTime == 0 ? 1 : estimatedTime.estimatedTime; //if weight/volume is not found in the table; minimum to 1 mins
                loadingTime += estimatedTime.estimatedTime * jobTripDetail.quantity;
            }
            return loadingTime;
        }
        public static double GetTotalLoadingUnloadingTime(PlanTruckSubTripJob subTripJob, SqlConnection con, SqlTransaction tran)
        {
            double loadingTime = 0;
            foreach (PlanTruckSubTripJobDetail subTripJobDetail in subTripJob.planTruckSubTripJobDetails)
            {
                decimal vol = (subTripJobDetail.length * subTripJobDetail.width * subTripJobDetail.height) / 1000000;
                LoadOrUnloadingTime estimatedTime = LoadOrUnloadingTime.GetLoadingTime(Convert.ToDouble(subTripJobDetail.unitWeight), Convert.ToDouble(vol), con, tran);
                estimatedTime.estimatedTime = estimatedTime.estimatedTime == 0 ? 1 : estimatedTime.estimatedTime; //if weight/volume is not found in the table; minimum to 1 mins
                loadingTime += estimatedTime.estimatedTime * subTripJobDetail.qty;
            }
            return loadingTime;
        }
        #endregion

        #region Get Start and End Time for fixed pickup
        //get the estimated time when the driver start working to reach the pickup point on time based on customer requirements
        public static DateTime GetStartTimeForFixedPickup(PlanTruckSubTrip subTrip, Stop truckLocation, TruckJobTrip jobTrip)
        {
            int startHr = Convert.ToInt32(jobTrip.StartTime.Substring(0, 2).ToString());
            int startMin = Convert.ToInt32(jobTrip.StartTime.Substring(2, 2).ToString());
            DateTime pickupTime = jobTrip.StartDate.Date.AddHours(startHr).AddMinutes(startMin);
            DateTime startTime = pickupTime;
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    startTime = GetStartTimeForFixedPickup(subTrip, truckLocation, jobTrip, pickupTime, con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return startTime;        
        }
        public static DateTime GetStartTimeForFixedPickup(PlanTruckSubTrip subTrip, Stop truckLocation, TruckJobTrip jobTrip, DateTime fixedTime, SqlConnection con, SqlTransaction tran)
        {
            DateTime startTime = fixedTime; //default to the booked pickup time
            try
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_CALCULATE_JOB_TIME);
                if (option == null) { return fixedTime; }
                if (option.setting_Value == "T")
                {
                    //get travel time from truck location to pickup point
                    double timeTravelFromTruckLocationToPickupPoint = GetTravelTimeBetweenTwoPlaces(fixedTime, truckLocation, jobTrip.StartStop).TotalMinutes;
                    //calculate time when the driver start to reach to the pickup point on specific time
                    startTime = fixedTime.AddMinutes(-1 * timeTravelFromTruckLocationToPickupPoint);
                    //capture the data for subtrip to be save
                    subTrip.Start = startTime;
                    subTrip.travelTimeFromVehicleToPickup = timeTravelFromTruckLocationToPickupPoint;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return startTime;
        }
        //get the estimated time when the driver finish the job
        public static DateTime GetEndTimeForFixedPickup(PlanTruckSubTrip subTrip, TruckJobTrip jobTrip)
        {
            int startHr = Convert.ToInt32(jobTrip.StartTime.Substring(0, 2).ToString());
            int startMin = Convert.ToInt32(jobTrip.StartTime.Substring(2, 2).ToString());
            DateTime startTime = jobTrip.StartDate.Date.AddHours(startHr).AddMinutes(startMin);
            DateTime endTime = startTime.AddHours(1);
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    endTime = GetEndTimeForFixedPickup(subTrip, jobTrip, startTime, con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return endTime;
        }
        public static DateTime GetEndTimeForFixedPickup(PlanTruckSubTrip subTrip, TruckJobTrip jobTrip, DateTime fixedTime, SqlConnection con, SqlTransaction tran)
        {
            DateTime endTime = fixedTime; //default to the booked pickup time
            try
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_CALCULATE_JOB_TIME);
                if (option == null) { return fixedTime; }
                if (option.setting_Value == "T")
                {
                    //get travel time from pickup to delivery point
                    double timeTravelFromPickupToDeliveryPoint = GetTravelTimeBetweenTwoPlaces(fixedTime, jobTrip.StartStop, jobTrip.EndStop).TotalMinutes;
                    //get unloading time
                    double unloadingTime = GetTotalLoadingUnloadingTime(jobTrip, con, tran);

                    //calculate time when the driver finish the job from specific time
                    //specifi time + loading time + travel time from pickup to delivery + Unloading time
                    endTime = fixedTime.AddMinutes(timeTravelFromPickupToDeliveryPoint + (unloadingTime * 2));
                    
                    subTrip.End = endTime;
                    subTrip.loadingTime = unloadingTime;
                    subTrip.travelTimeFromPickupToDelivery = timeTravelFromPickupToDeliveryPoint;
                    subTrip.unloadingTime = unloadingTime;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return endTime;
        }
        #endregion

        #region Get Start and End Time for fixed delivery
        //get the estimated time when the driver start working to reach the delivery point on time based on customer requirements
        public static DateTime GetStartTimeForFixedDelivery(PlanTruckSubTrip subTrip, Stop truckLocation, TruckJobTrip jobTrip)
        {
            int endHr = Convert.ToInt32(jobTrip.EndTime.Substring(0, 2).ToString());
            int endMin = Convert.ToInt32(jobTrip.EndTime.Substring(2, 2).ToString());
            DateTime deliveryTime = jobTrip.EndDate.Date.AddHours(endHr).AddMinutes(endMin);
            DateTime startTime = deliveryTime;
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    startTime = GetStartTimeForFixedDelivery(subTrip, truckLocation, jobTrip, deliveryTime, con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return startTime;
        }
        public static DateTime GetStartTimeForFixedDelivery(PlanTruckSubTrip subTrip, Stop truckLocation, TruckJobTrip jobTrip, DateTime fixedTime, SqlConnection con, SqlTransaction tran)
        {
            DateTime startTime = fixedTime; //default to the booked pickup time
            try
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_CALCULATE_JOB_TIME);
                if (option == null) { return fixedTime; }
                if (option.setting_Value == "T")
                {
                    //get travel time from truck location to pickup point
                    double timeTravelFromTruckLocationToPickupPoint = GetTravelTimeBetweenTwoPlaces(fixedTime, truckLocation, jobTrip.StartStop).TotalMinutes;
                    //get travel time from pickup to delivery point
                    double timeTravelFromPickupToDeliveryPoint = GetTravelTimeBetweenTwoPlaces(fixedTime, jobTrip.StartStop, jobTrip.EndStop).TotalMinutes;
                    //get loading time
                    double loadingTime = GetTotalLoadingUnloadingTime(jobTrip, con, tran);
                    //calculate time when the driver start to reach to the pickup point on specific time
                    startTime = fixedTime.AddMinutes(-1 * (timeTravelFromTruckLocationToPickupPoint + timeTravelFromPickupToDeliveryPoint + loadingTime));
                    //capture the data for subtrip to be save
                    subTrip.Start = startTime;
                    subTrip.travelTimeFromVehicleToPickup = timeTravelFromTruckLocationToPickupPoint;
                    subTrip.loadingTime = loadingTime;
                    subTrip.travelTimeFromPickupToDelivery = timeTravelFromPickupToDeliveryPoint;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return startTime;
        }
        //get the estimated time when the driver finish the job
        public static DateTime GetEndTimeForFixedDelivery(PlanTruckSubTrip subTrip, TruckJobTrip jobTrip)
        {
            int endHr = Convert.ToInt32(jobTrip.EndTime.Substring(0, 2).ToString());
            int endMin = Convert.ToInt32(jobTrip.EndTime.Substring(2, 2).ToString());
            DateTime deliveryTime = jobTrip.EndDate.Date.AddHours(endHr).AddMinutes(endMin);
            DateTime endTime = deliveryTime;
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    endTime = GetEndTimeForFixedDelivery(subTrip, jobTrip, deliveryTime, con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return endTime;
        }
        public static DateTime GetEndTimeForFixedDelivery(PlanTruckSubTrip subTrip, TruckJobTrip jobTrip, DateTime fixedTime, SqlConnection con, SqlTransaction tran)
        {
            DateTime endTime = fixedTime; //default to the booked pickup time
            try
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_CALCULATE_JOB_TIME);
                if (option == null) { return fixedTime; }
                if (option.setting_Value == "T")
                {
                    //get unloading time
                    double unloadingTime = GetTotalLoadingUnloadingTime(jobTrip, con, tran);
                    //calculate time when to finish the job trip
                    //specific + Unloading time
                    endTime = fixedTime.AddMinutes(unloadingTime);
                    subTrip.End = endTime;
                    subTrip.unloadingTime = unloadingTime;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return endTime;
        }
        #endregion

        #region OPtimized Plan Chart
        //20170105 - gerry added to get new data for affect driver/ plantrip
        public static void GetDataForAffectedPlanTrip(ref SortableList<PlanTruckTrip> planTrips, List<Driver> affectedDrivers)
        {
            try
            {
                foreach (PlanTruckTrip planTrip in planTrips)
                {
                    var tempDriver = affectedDrivers.FirstOrDefault(dr => dr.Code.Trim() == planTrip.Driver.Code.Trim());
                    if (tempDriver != null && !tempDriver.Code.Trim().Equals(string.Empty))
                    {
                        if (planTrip.Driver.Code.Trim() == tempDriver.Code.Trim())
                        {
                            planTrip.planTruckSubTrips = PlanTruckDAL.GetPlanTruckSubTrips(planTrip);
                            planTrip.CopyPlanSubTripsToOldPlanSubTrips();
                            planTrip.UpdateVersion = PlanTruckDAL.GetPlanTruckTripUpdateVersion(planTrip);
                        }
                    }
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message); }
        }       
        #endregion

        #region OTO Auto assign jobs to driver/vehicle
        /// <summary>
        /// Auto assignment of OTO job tips currently work for vehicles always park to default parking place.
        /// </summary>
        /// <param name="jobTripOTOList"></param>
        /// <param name="availableTrucks"></param>
        public static void AutoAssignOTOJobTripsToVehicle(ref SortableList<PlanTruckSubTrip> planTruckSubTrips, SortableList<PlanTruckTrip> planTruckTrips, List<TruckJobTrip> jobTripOTOList, string frmName, string userID, string deptCode, out int jobAddCount)
        {
            jobAddCount = 0;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    //Sort job trips by fixed time and the earliest.
                    //var jobTrips = jobTripOTOList.Where(jt => (!jt.isFixedTime || (jt.isFixedTime && jt.StartDate.Date == planTruckTrips[0].ScheduleDate.Date))).OrderBy(jt => jt.isFixedTime).ThenBy(jt => jt.StartTime).ToList();
                    var jobTrips = jobTripOTOList.OrderByDescending(jt => jt.isFixedTime).ThenBy(jt => jt.StartTime).ToList();
                    //get available lorries
                    List<Vehicle> availableLorries = new List<Vehicle>();
                    foreach (PlanTruckTrip pt in planTruckTrips)
                    {
                        availableLorries.Add(pt.Vehicle);
                    }
                    //Loop through the sorting job trips.
                    foreach (TruckJobTrip jt in jobTrips)
                    {
                        GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;
                        PointLatLng? pos = GMapProviders.GoogleMap.GetPoint(GetCompleteAddress(jt.StartStop), out status);
                        PointLatLng jobTripPosition = pos.Value;
                        //take the vehicle with shortest distance and the smallest capacity that capable to do the job trip
                        if (availableLorries.Count() > 0)
                        {
                            var suitableNearestLorry = GetNearestSuitableVehiclesFromJobTrip(planTruckTrips, jobTripPosition, ref availableLorries);
                            if (suitableNearestLorry != null)
                            {
                                //get Plan Truck Trip based on vehicle and Assign suitable lorry to job trip
                                PlanTruckTrip planTruckTrip = FindPlanTruckTripByVehicle(planTruckTrips, suitableNearestLorry);
                                if (planTruckTrip != null)
                                {
                                    int shour = 0;
                                    int sminute = 0;
                                    int ehour = 0;
                                    int eminute = 0;
                                    if (jt.isFixedTime)
                                    {
                                        shour = Convert.ToInt32(String.Concat(jt.StartTime[0], jt.StartTime[1]));
                                        sminute = Convert.ToInt32(String.Concat(jt.StartTime[2], jt.StartTime[3]));
                                        ehour = Convert.ToInt32(String.Concat(jt.EndTime[0], jt.EndTime[1]));
                                        eminute = Convert.ToInt32(String.Concat(jt.EndTime[2], jt.EndTime[3]));
                                    }
                                    else
                                    {
                                        if (planTruckTrip.planTruckSubTrips.Count() > 0)
                                            shour = planTruckTrip.planTruckSubTrips.Last().End.Hour;
                                        else
                                            shour = Convert.ToInt32(ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_START_WORKING_HOUR).setting_Value);

                                        ehour = shour + 1;                                        
                                    }
                                    DateTime startTime = planTruckTrips[0].ScheduleDate.Date.AddHours(shour).AddMinutes(sminute);
                                    DateTime endTime = planTruckTrips[0].ScheduleDate.Date.AddHours(ehour).AddMinutes(eminute);

                                    //Create plan truck sub trip
                                    SortableList<TruckJobTrip> jobTripList = new SortableList<TruckJobTrip>();
                                    jobTripList.Add(jt);
                                    string outMsg = string.Empty;
                                    bool overload = false;
                                    //check if there is still not completed plan sub trip
                                    bool hasIncompleteSubTrip = planTruckTrip.planTruckSubTrips.Where(pst => pst.Status != JobTripStatus.Completed).Count() > 0;
                                    if (!hasIncompleteSubTrip && !jt.isFixedTime)
                                    {
                                        if (!planTruckTrip.CheckTruckCapacityIsOverload(planTruckTrip.GetTotalVolumeCarried(), planTruckTrip.GetTotalWeightCarried(), suitableNearestLorry, out outMsg, false))
                                        {
                                            planTruckTrip.CheckTruckJobTripsBeforeAssignToVehicle(jobTripList, startTime, endTime, jt.GetBalanceWeightForPlan(), jt.GetBalanceVolForPlan(), out outMsg, out overload);

                                            PlanTruckSubTrip planSubTrip = planTruckTrip.CreatePlanTruckSubTrip(jt, jt.GetBalanceWeightForPlan(), jt.GetBalanceVolForPlan(), startTime, endTime, frmName, userID, planTruckTrip.dept, con, tran);
                                            if (planTruckSubTrips == null)
                                                planTruckSubTrips = new SortableList<PlanTruckSubTrip>();

                                            planTruckSubTrips.Add(planSubTrip);//add to the collection of plan sub trip for the day, it will return as reference object
                                            jobAddCount++;
                                        }
                                    }
                                }
                                availableLorries.Remove(suitableNearestLorry);//remove lorry from the list after assigned
                            }
                        }
                    }
                    tran.Commit();
                }
                catch (FMException ex)
                {
                    if (tran != null) { tran.Rollback(); } throw ex;
                }
                catch (Exception ex)
                {
                    if (tran != null) { tran.Rollback(); } throw new FMException(ex.Message.ToString());
                }
                finally { con.Close(); }
            }
        }

        public static Vehicle GetNearestSuitableVehiclesFromJobTrip(SortableList<PlanTruckTrip> planTruckTrips, PointLatLng jobTripPosition, ref List<Vehicle> availableLorries)
        {
            try
            {
                //SortedDictionary<Vehicle, double> lorriesWithDisctance = new SortedDictionary<Vehicle, double>();
                Dictionary<Vehicle, double> lorriesWithDisctance = new Dictionary<Vehicle, double>();
                //Sort the vehicle by capacity.
                var lorries = availableLorries.OrderBy(tr => tr.VolCapacity).ThenBy(tr => tr.MaximumLadenWeight).ToList();
                foreach (Vehicle lorry in lorries)
                {
                    Stop lorryStop = Stop.GetStop(lorry.DefaultStop);
                    PlanTruckTrip planTrip = FindPlanTruckTripByVehicle(planTruckTrips, lorry);
                    if (planTrip != null)
                    { //get last stop location of the lorry
                        if (planTrip.planTruckSubTrips.Count() > 0)
                            lorryStop = planTrip.planTruckSubTrips.Last().EndStop;
                    }
                    GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;
                    PointLatLng? pos = GMapProviders.GoogleMap.GetPoint(GetCompleteAddress(lorryStop), out status);
                    PointLatLng lorryPoint = pos.Value;
                    double NDis = GMapProviders.EmptyProvider.Projection.GetDistance(jobTripPosition, lorryPoint);
                    lorriesWithDisctance.Add(lorry, NDis);
                }
                foreach (var lorry in lorriesWithDisctance.OrderBy(i => i.Value))
                {
                    Vehicle closestLorry = availableLorries.FirstOrDefault(l=> l.Number == lorry.Key.Number);
                    return closestLorry;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return null;
        }
        public static string GetCompleteAddress(Stop location)
        {
            string completeAddress = location.Address1;
            //if (location.Address2.Trim() != "")
            //    completeAddress += "," + location.Address2;
            //if (location.Address3.Trim() != "")
            //    completeAddress += "," + location.Address3;
            //if (location.Address4.Trim() != "")
            //    completeAddress += "," + location.Address4;
            if (location.countryName.Trim() != "")
                completeAddress += "," + location.countryName;

            return completeAddress;
        }
        #endregion
    }
}
