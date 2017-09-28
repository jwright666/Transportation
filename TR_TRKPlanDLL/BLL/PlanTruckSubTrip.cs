using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_HLBookDLL.BLL;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_TRKPlanDLL.DAL;
using TR_LanguageResource.Resources;
using FM.TR_HLPlanDLL.BLL;
using FM.TruckPlanDLL.BLL;

namespace FM.TR_TRKPlanDLL.BLL
{
    public class PlanTruckSubTrip : PlanSubTrip, ICloneable
    {
        public SortableList<PlanTruckSubTripJob> planTruckSubTripJobs { get; set; }
        // 2014-03-27 Zhou Kai renames the variable from truckTrip to planTruckTrip
        private PlanTruckTrip planTruckTrip;        
        public string shipment_id { get; set; }//20160820 - gerry added for visual loading(3D app)
        public string route_point_id { get; set; }//20160908 - gerry added for cloud track
        //public string cloudTrack_JobId { get; set; }//20160908 - gerry added for cloud track
        //20161228
        public double travelTimeFromVehicleToPickup { get; set; }
        public double loadingTime { get; set; }
        public double travelTimeFromPickupToDelivery { get; set; }
        public double unloadingTime { get; set; }
        public bool isRefrigerated { get; set; } //20170801

        public PlanTruckSubTrip() : base()
        {
            this.planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();
        }

        public PlanTruckSubTrip(int seqNo, DateTime start, DateTime end,
            Stop startStop, Stop endStop, bool isBillableTrip, string description,
            SortableList<PlanTruckSubTripJob> planTruckSubTripJobs,
            PlanTruckTrip trip, JobTripStatus status, byte[] updateVersion)
            : base(seqNo, start, end, startStop, endStop, isBillableTrip, description, trip, status)
        {
            // Constructor of parent's is used
            this.planTruckSubTripJobs = planTruckSubTripJobs;
            this.planTruckTrip = trip;
            base.updateVersion = updateVersion;
        }

        // 2014-02-18 Zhou Kai edits the comments:
        // This method is called from a. FrmAddTripPlan, btnSave_Click event. b. Drag-drop event
        // In this function:
        // a) CanAddPlanTruckSubTrip is called to check if the inputs from FrmAddTripPlan are valid 
        // b) A TruckJobTripPlan is created based on the inputs from FrmAddTripPlan
        // c) A PlanTruckSubTrip is created 
        // d) A PlanTruckSubTripJob is then added to that PlanTruckSubTrip
        // e) That TruckJobTripPlan is then added to the collection of TruckJobTripPlans in the TruckJobTrip
        // TruckJobTrip --> TruckJobTripPlan --> PlanTruckSubTrip <-- PlanTruckSubTripJobs
        // This function updates both UI and Database Tables
        // 2014-02-18 Zhou Kai ends

        public static PlanTruckSubTrip CreatePlanTruckSubTrip(PlanTruckTrip planTrucktrip,
                                                                                                  TruckJobTrip truckJobTrip,
                                                                                                  TruckJobTripPlan truckJobTripPlan,
                                                                                                  string formName, 
                                                                                                  string user)
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            string message = "";
            bool overload = false;
            string exceptionMsg = String.Empty;

            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State != ConnectionState.Open) { con.Open(); };
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (planTrucktrip.CanAddPlanTruckSubTrip(truckJobTripPlan, out message, out overload))
                {
                    #region "Create newPlanTruckSubTrip"
                    // create new plan truck sub trip based on the truck job trip plan
                    // 27 Jan 2012 - Gerr Replaced this line, we are not sure with the last planTruckSubTripSeq_No
                    // newPlanTruckSubTrip.SeqNo = planTrucktrip.TruckSubTrips.Count + 1; 
                    // end
                    newPlanTruckSubTrip.SeqNo = PlanTruckDAL.GetNextPlanTruckSubTripSeqNo(planTrucktrip.PlanTripNo, con, tran);
                    newPlanTruckSubTrip.Start = truckJobTripPlan.start;
                    newPlanTruckSubTrip.End = truckJobTripPlan.end;
                    newPlanTruckSubTrip.StartStop = truckJobTrip.StartStop;
                    newPlanTruckSubTrip.EndStop = truckJobTrip.EndStop;
                    newPlanTruckSubTrip.IsBillableTrip = true;
                    newPlanTruckSubTrip.Description = "";
                    newPlanTruckSubTrip.VehicleNumber = truckJobTripPlan.truckNo;
                    newPlanTruckSubTrip.DriverNumber = truckJobTripPlan.driver;
                    newPlanTruckSubTrip.Status = JobTripStatus.Assigned;
                    #endregion

                    #region "Create newPlanTruckSubTripJob"
                    // a new planTruckSubTripJob is created on
                    // the new planTruckSubTrip and the existing
                    // truckJobTrip
                    PlanTruckSubTripJob newPlanTruckSubTripJob = new PlanTruckSubTripJob();
                    //27 Jan 2012 - Gerry Replaced this line, planTruckSubTripJobPlanSeq_No is base on outPlanTruckSubTrip Seq_No
                    //newPlanTruckSubTripJob.planTripSeq = planTrucktrip.TruckSubTrips.Count + 1;
                    newPlanTruckSubTripJob.planTripNo = planTrucktrip.PlanTripNo;
                    newPlanTruckSubTripJob.planSubTripSeqNo = newPlanTruckSubTrip.SeqNo;
                    //end
                    newPlanTruckSubTripJob.jobID = truckJobTrip.JobID;
                    newPlanTruckSubTripJob.jobSeq = truckJobTrip.Sequence;
                    newPlanTruckSubTripJob.custCode = truckJobTrip.CustomerCode;
                    newPlanTruckSubTripJob.custName = truckJobTrip.CustomerName;
                    newPlanTruckSubTripJob.startStop = truckJobTrip.StartStop;
                    newPlanTruckSubTripJob.endStop = truckJobTrip.EndStop;
                    newPlanTruckSubTripJob.startDate = truckJobTrip.StartDate;
                    newPlanTruckSubTripJob.endDate = truckJobTrip.EndDate;
                    string startTime = truckJobTripPlan.start.Hour.ToString() + truckJobTripPlan.start.Minute.ToString();//.Substring(0, 5);
                    if (startTime.Length < 4)
                        startTime = startTime.Insert(0, "0");
                    string endTime = truckJobTripPlan.end.Hour.ToString() + truckJobTripPlan.end.Minute.ToString();
                    if (endTime.Length < 4)
                        endTime = endTime.Insert(0, "0");

                    newPlanTruckSubTripJob.startTime = startTime;//truckJobTripPlan.start.ToShortTimeString().Substring(0,5).Replace(":","");
                    newPlanTruckSubTripJob.endTime = endTime; //truckJobTripPlan.end.ToShortTimeString().Substring(0, 5).Replace(":", "");
                    newPlanTruckSubTripJob.volume = truckJobTripPlan.volume;
                    newPlanTruckSubTripJob.weight = truckJobTripPlan.weight;
                    newPlanTruckSubTripJob.updateVersion = new byte[8];
                    newPlanTruckSubTripJob.status = JobTripStatus.Assigned;
                    #endregion

                    //Add newTruckSubtripJob to newPlanTruckSubTrip.planTruckSubTripJobs collection
                    newPlanTruckSubTrip.planTruckSubTripJobs.Add(newPlanTruckSubTripJob);

                    TruckJobTripPlan outTruckJobTripPlan = new TruckJobTripPlan();
                    truckJobTrip.AddTruckJobTripPlan(truckJobTripPlan, con, tran, out outTruckJobTripPlan, user, formName);
                    // 2013-11-18 Zhou Kai adds
                    // 2014-03-31 Zhou Kai modifies the code below: replace outTruckJobTripPlan with truckJobTripPlan
                    truckJobTrip.truckJobTripPlans.Add(/*outTruckJobTripPlan*/ truckJobTripPlan);
                    // 2013-11-18 Zhou Kai ends
                    newPlanTruckSubTripJob.planSubTripJobSeqNo = outTruckJobTripPlan.planSubTripJobSeqNo;
                    // 2014-02-18 Zhou Kai use below function and comment out the old function
                    newPlanTruckSubTrip.AddPlanTruckSubTrip(planTrucktrip, con, tran, formName, user);
                    // planTrucktrip.AddPlanTruckSubTrip(newPlanTruckSubTrip, formName, user, con, tran);
                    // 2014-02-18 Zhou Kai ends

                    if (truckJobTrip.TripStatus != JobTripStatus.Assigned)
                    {
                        JobTripState newJobTripState = new JobTripState();
                        newJobTripState.Status = JobTripStatus.Assigned;
                        newJobTripState.IsNew = true;
                        newJobTripState.StatusDate = DateTime.Today;
                        truckJobTrip.SetJobTripStatusForTruck(newJobTripState, con, tran, user, formName, truckJobTrip.TripStatus.ToString());
                    }
                    
                    // 2013-11-18 Zhou Kai adds
                    TruckJobTrip.UpdateAcutalWeightVolumeforTruckJobTrip(truckJobTripPlan.jobID, truckJobTripPlan.sequence, con, tran);
                    // 2013-11-18 Zhou Kai ends

                    // truckJobTrip.truckJobTripPlans.Add(truckJobTripPlan);
                    // planTrucktrip.pPlanTruckSubTrips.Add(newPlanTruckSubTrip);

                    #region "2014-04-01 Zhou Kai comments out the logging block"
                    /*
                    DateTime serverDateTime = Logger.GetServerDateTime();
                    LogHeader logHeader = new LogHeader(FMModule.Trucking, formName, serverDateTime,
                    planTrucktrip.PlanTripNo, truckJobTrip.JobID.ToString(), FormMode.Add, user);

                    LogDetail logDetail1 = new LogDetail("Starttime", truckJobTrip.StartTime.ToString());
                    logHeader.LogDetails.Add(logDetail1);
                    // now call the Logger class to write
                    Logger.WriteLog(logHeader, con, tran);
                    */
                    #endregion

                    tran.Commit();
                }
                else
                {
                    throw new FMException(message);
                }
            }
            catch (FMException ex)
            {
                exceptionMsg = ex.ToString();
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
                // 2014-01-15 Zhou Kai add codes to keep the original exception message
                if (!exceptionMsg.Equals(String.Empty))
                {
                    throw new FMException(exceptionMsg);
                }
                // 2014-01-15 Zhou Kai ends
            }
            return newPlanTruckSubTrip;
        }

        // 2014-03-28 Zhou Kai adds comments to the function below:
        public bool CanAddPlanTruckSubTripJobToPlanTruckSubTrip(TruckJobTrip truckJobTrip,
                                                                                                        PlanTruckTrip planTruckTrip,
                                                                                                        PlanTruckSubTripJob planTruckSubTripJob)
        {
            bool flag = true;
            try
            {
                if (this.planTruckSubTripJobs.Count > 0)
                {
                    // don't understand this codes below, I think we should check:
                    // if (truckJobTrip.StartStop.Code != this.StartStop.Code) instead
                    if (truckJobTrip.StartStop.Code != this.planTruckSubTripJobs[0].startStop.Code)
                    {
                        flag = false;
                        throw new FMException(TptResourceBLL.ErrStartStopNotSame);
                    }
                }
                bool find = false;
                // don't understand the loop below, we want to add a truckJobTrip to a known planTruckSubTrip,
                // why do we check all other planTruckSubTrips?
                /*
                for (int i = 0; i < planTruckTrip.pPlanTruckSubTrips.Count; i++)
                {
                    string hour = String.Concat(planTruckSubTripJob.startTime[0], planTruckSubTripJob.startTime[1]);
                    string minute = String.Concat(planTruckSubTripJob.startTime[2], planTruckSubTripJob.startTime[3]);

                    //Do not allow jobtrip which is already in planTruckSubTrip
                    if (planTruckTrip.pPlanTruckSubTrips[i].planTruckSubTripJobs.Count > 0)
                    {
                        for (int j = 0; j < planTruckTrip.pPlanTruckSubTrips[i].planTruckSubTripJobs.Count; j++)
                        {
                            if (planTruckSubTripJob.planTripSeq == planTruckTrip.pPlanTruckSubTrips[i].planTruckSubTripJobs[j].planTripSeq
                                && planTruckSubTripJob.jobID == planTruckTrip.pPlanTruckSubTrips[i].planTruckSubTripJobs[j].jobID
                                && planTruckSubTripJob.jobSeq == planTruckTrip.pPlanTruckSubTrips[i].planTruckSubTripJobs[j].jobSeq)
                            {
                                flag = false;
                                throw new FMException(TptResourceBLL.ErrCantAddJobTripAlreadyInPlanSubTrip);
                            }
                            #region Removed Temp
                            ////17 Jan 2012 - Compare new TruckSubTripJob starttime and endstop to current planTruckSubTrip starttime and endstop
                            //if (((DateTime)this.Start >= (DateTime)planTruckSubTripJob.startDate.AddHours(Convert.ToInt32(hour)).AddMinutes(Convert.ToInt32(minute)))
                            //    && (planTruckTrip.TruckSubTrips[i].planTruckSubTripJobs[j].endStop.Code == planTruckSubTripJob.endStop.Code))
                            //{
                            //    find = true;
                            //}
                           #endregion
                 */
                // 2014-03-28 Zhou Kai writes new codes:
                string hour = String.Concat(planTruckSubTripJob.startTime[0], planTruckSubTripJob.startTime[1]);
                string minute = String.Concat(planTruckSubTripJob.startTime[2], planTruckSubTripJob.startTime[3]);
                for (int i = 0; i < planTruckTrip.pPlanTruckSubTrips.Count; i++)
                {
                    // The codes below is to check if the new truckJobTrip will stop at any following planTruckSubTrip's end stop
                    if (planTruckTrip.pPlanTruckSubTrips[i].EndStop.Code == planTruckSubTripJob.endStop.Code)
                    {

                        if ((DateTime)planTruckTrip.pPlanTruckSubTrips[i].Start >= (DateTime)planTruckSubTripJob.startDate) //.AddHours(Convert.ToInt32(hour)).AddMinutes(Convert.ToInt32(minute))))
                        {
                            find = true;
                        }
                    }
                }
                // 2014-03-28 Zhou Kai ends

                if (find == false)
                {
                    flag = false;
                    throw new FMException(TptResourceBLL.ErrCantFindEndStop);
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return flag;
        }
        public bool IsPlanTruckSubTripOverload(PlanTruckSubTripJob planTruckSubTripJob, Vehicle truck)
        {
            string message = string.Empty;
            decimal currentWeightLoad = 0;
            decimal currentVolLoad = 0;
            foreach (PlanTruckSubTripJob plstj in this.planTruckSubTripJobs)
            {
                currentWeightLoad += plstj.weight;
                currentVolLoad += plstj.volume;
            }
            if ((currentVolLoad + planTruckSubTripJob.volume) > truck.VolCapacity)
            {
                message = message + TptResourceBLL.ErrCBM + "\n";
            }

            if ((currentWeightLoad + planTruckSubTripJob.weight) > truck.MaximumLadenWeight)
            {
                message = message + TptResourceBLL.ErrWeight + "\n";
            }
            if(!message.Equals(string.Empty))
                throw new FMException(message);

            return false;
        }

        public bool IsTruckJobTripOverload(TruckJobTrip truckJobTrip, PlanTruckTrip planTruckTrip, PlanTruckSubTripJob planTruckSubTripJob, out string message)
        {
            bool flag = true;
            message = "";
            decimal totalAllocatedVol = 0;
            decimal totalAllocatedWeight = 0;
            totalAllocatedVol = truckJobTrip.GetAllocatedVolToPlan();
            totalAllocatedWeight = truckJobTrip.GetAllocatedWeightToPlan();

            if ((totalAllocatedVol + planTruckSubTripJob.volume) > truckJobTrip.BookedVol)
            {
                flag = false;
                message = message + TptResourceBLL.ErrCBM + "\n";
            }

            if ((totalAllocatedWeight + planTruckSubTripJob.weight) > truckJobTrip.BookedWeight)
            {
                flag = false;
                message = message + TptResourceBLL.ErrWeight + "\n";
            }

            return flag;
        }

        /*
         * 2013-08-21 Zhou Kai edits comments:
         * 
         * This method is called from the Planning UI when a planTruckSubTripJob is added to
         * a planTruckSubTrip by clicking button "Add Trip Job" on FrmPlanTruckSubTrip, or
         * by drag-drop operation.
         * 
         * In the method, these things happen:
         * (a) CanAddPlanTruckSubTripJobToPlanSubTrip is called to validate
         * (b) the planTruckSubTripJob is added to current PlanTruckSubTrip
         * (c) adds to subsequent PlanTruckSubTrips until the endStop is reached
         * (d) finally a TruckJobTripPlan is created and added to collection in TruckJobTrip 
         * 
         * 2013-08-21 ends
         */
        public bool AddPlanTruckSubTripJobToPlanTruckSubTrips(PlanTruckTrip planTruckTrip,
            TruckJobTrip truckJobTrip, PlanTruckSubTripJob planTruckSubTripJob, string frmName, string user)
        {
            bool flag = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //string message = "";
                TruckJobTripPlan newTruckJobTripPlan = new TruckJobTripPlan();
                //planTruckTrip.TruckSubTrips.SortByStartTime();
                if (this.CanAddPlanTruckSubTripJobToPlanTruckSubTrip(truckJobTrip, planTruckTrip, planTruckSubTripJob))
                {
                    bool addtosubsequence = false;
                    DateTime starttime = DateTime.Now;
                    string driverno = planTruckTrip.Driver.Code;
                    string truckno = planTruckTrip.VehicleNumber;

                    #region 20160311 - Gerry removed
                    /*
                    newTruckJobTripPlan.scheduleDate = planTruckTrip.ScheduleDate;
                    newTruckJobTripPlan.planTripNo = planTruckTrip.PlanTripNo;
                    newTruckJobTripPlan.driver = driverno;
                    newTruckJobTripPlan.truckNo = truckno;
                    newTruckJobTripPlan.volume = planTruckSubTripJob.volume;
                    newTruckJobTripPlan.weight = planTruckSubTripJob.weight;
                    double starthour = double.Parse(string.Concat(planTruckSubTripJob.startTime[0], planTruckSubTripJob.startTime[1]));
                    double startminute = double.Parse(string.Concat(planTruckSubTripJob.startTime[2], planTruckSubTripJob.startTime[3]));
                    double endhour = double.Parse(string.Concat(planTruckSubTripJob.endTime[0], planTruckSubTripJob.endTime[1]));
                    double endminute = double.Parse(string.Concat(planTruckSubTripJob.endTime[2], planTruckSubTripJob.endTime[3]));
                    newTruckJobTripPlan.start = planTruckSubTripJob.startDate.AddHours(starthour).AddMinutes(startminute); ;
                    newTruckJobTripPlan.end = planTruckSubTripJob.endDate.AddHours(endhour).AddMinutes(endminute); ;
                    newTruckJobTripPlan.jobID = planTruckSubTripJob.jobID;
                    newTruckJobTripPlan.sequence = planTruckSubTripJob.jobSeq;

                    truckJobTrip.truckJobTripPlans.Add(newTruckJobTripPlan);

                    TruckJobTripPlan outTruckJobTripPlan = new TruckJobTripPlan();
                    truckJobTrip.AddTruckJobTripPlan(newTruckJobTripPlan, con, tran, out outTruckJobTripPlan, user, frmName);
                   

                    //set planTripSeq and jobTripPlanSeq
                    planTruckSubTripJob.planSubTripSeqNo = this.SeqNo;
                    planTruckSubTripJob.planSubTripJobSeqNo = outTruckJobTripPlan.planSubTripJobSeqNo;
                     */
                     #endregion
                    PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                    PlanTruckDAL.AddPlanTruckSubTripJobDatabase(planTruckTrip, this, planTruckSubTripJob, con, tran, out outPlanTruckSubTripJob, user, frmName);
                    planTruckSubTripJob.planSubTripSeqNo = this.SeqNo;
                    this.planTruckSubTripJobs.Add(outPlanTruckSubTripJob);

                    if (this.EndStop != planTruckSubTripJob.endStop)
                    {
                        addtosubsequence = true;
                    }
                    //Need to sort the start time of the planTruckSubTrips with in the same plantrip
                    planTruckTrip.planTruckSubTrips.SortByStartTime();

                    int start = planTruckTrip.pPlanTruckSubTrips.IndexOf(this);

                    if (addtosubsequence)
                    {
                        DateTime endtime = DateTime.Now;
                        for (int i = 0; i < planTruckTrip.pPlanTruckSubTrips.Count; i++)
                        {
                            if (planTruckTrip.pPlanTruckSubTrips[i].EndStop.Code == planTruckSubTripJob.endStop.Code)
                            {
                                endtime = planTruckTrip.pPlanTruckSubTrips[i].End;
                                break;
                            }
                        }
                        for (int i = 0; i < planTruckTrip.pPlanTruckSubTrips.Count; i++)
                        {
                            if ((planTruckTrip.pPlanTruckSubTrips[i].Start > this.Start) && (planTruckTrip.pPlanTruckSubTrips[i].End <= endtime))
                            {
                                //20160311 - gerry modified
                                planTruckSubTripJob.planSubTripSeqNo = planTruckTrip.pPlanTruckSubTrips[i].SeqNo;
                                planTruckSubTripJob.startStop = planTruckTrip.pPlanTruckSubTrips[i].StartStop;                                
                                string strStartTime = planTruckTrip.pPlanTruckSubTrips[i].Start.Hour.ToString() + planTruckTrip.pPlanTruckSubTrips[i].Start.Minute.ToString();//.Substring(0, 5);
                                if (strStartTime.Length < 4)
                                    strStartTime = strStartTime.Insert(0, "0");
                                string strEndTime = planTruckTrip.pPlanTruckSubTrips[i].End.Hour.ToString() + planTruckTrip.pPlanTruckSubTrips[i].End.Minute.ToString();
                                if (strEndTime.Length < 4)
                                    strEndTime = strEndTime.Insert(0, "0");

                                planTruckSubTripJob.startTime = strStartTime;
                                planTruckSubTripJob.endTime = strEndTime;

                                outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                                PlanTruckDAL.AddPlanTruckSubTripJobDatabase(planTruckTrip, this, planTruckSubTripJob, con, tran, out outPlanTruckSubTripJob, user, frmName);

                                planTruckTrip.pPlanTruckSubTrips[i].planTruckSubTripJobs.Add(outPlanTruckSubTripJob);
                                //20160311 - gerry modified end
                            }
                        }
                    }

                    #region "2014-04-01 Zhou Kai comments out the logging block"
                    /*
                    DateTime serverDateTime = Logger.GetServerDateTime();

                    LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime, planTruckTrip.PlanTripNo, this.SeqNo.ToString(), FormMode.Add, user);
                    LogDetail logDetail1 = new LogDetail("TruckTripNo", planTruckSubTripJob.jobID.ToString());
                    LogDetail logDetail2 = new LogDetail("TruckTripNoSeq", planTruckSubTripJob.jobSeq.ToString());

                    logHeader.LogDetails.Add(logDetail1);
                    logHeader.LogDetails.Add(logDetail2);

                    Logger.WriteLog(logHeader, con, tran);
                     */ 
                    #endregion 

                    tran.Commit();
                }
            }
            catch (FMException ex)
            {
                flag = false;
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                flag = false;
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return flag;
        }

        // 2014-04-01 Zhou Kai adds comments:
        // You can only delete a planTruckSubTripJob 
        // (1) from the planTruckSubTrip where it is started
        // (2) and when it's not completed yet.
        public bool CanDeletePlanTruckSubTripJob(PlanTruckSubTripJob planTruckSubTripJob, out string message)
        {
            bool flag = true;
            message = "";

            if (this.StartStop.Code != planTruckSubTripJob.startStop.Code)
            {
                flag = false;
                message = TptResourceBLL.ErrCantDeleteBecauseStartStopNotValid + "\n";
            }

            if (planTruckSubTripJob.status == JobTripStatus.Completed)
            {
                flag = false;
                message = TptResourceBLL.ErrCantDeleteCompletedJob;
            }

            return flag;
        }

        public bool DeletePlanTruckSubTripJob(PlanTruckSubTripJob planTruckSubTripJob, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                #region 20160420 - gerry repplaced code below
                //PlanTruckSubTrip outPlanTruckSubTrip = new PlanTruckSubTrip();
                //if (PlanTruckDAL.DeletePlanTruckSubTripJobDatabase(this, planTruckSubTripJob, con, tran, out outPlanTruckSubTrip))
                //{
                //    TruckJobTrip tempTRuckJobTrip = new TruckJobTrip();
                //    tempTRuckJobTrip.JobID = planTruckSubTripJob.jobID;
                //    tempTRuckJobTrip.Sequence = planTruckSubTripJob.jobSeq;

                //}
                //retValue = true;
                #endregion
                TruckJobTrip tempTRuckJobTrip = TruckJobTrip.GetTruckJobTrip(planTruckSubTripJob.jobID, planTruckSubTripJob.jobSeq, con, tran);
                PlanTruckSubTrip outPlanTruckSubTrip = new PlanTruckSubTrip();
                if (this.EndStop.Code.Trim() != tempTRuckJobTrip.EndStop.Code.Trim())
                    PlanTruckDAL.DeletePlanTruckSubTripJobDatabaseWithPreceeding(this, planTruckSubTripJob, con, tran, out outPlanTruckSubTrip);
                else
                    PlanTruckDAL.DeletePlanTruckSubTripJobDatabase(this, planTruckSubTripJob, con, tran, out outPlanTruckSubTrip);

                //now delete TruckJobTripPlan if any
                var list = tempTRuckJobTrip.truckJobTripPlans.Where(jtp => jtp.planTripNo == planTruckSubTripJob.planTripNo
                                                                        && jtp.planSubTripSeqNo == planTruckSubTripJob.planSubTripSeqNo
                                                                        && jtp.planSubTripJobSeqNo == planTruckSubTripJob.planSubTripJobSeqNo
                                                                        && jtp.jobID == planTruckSubTripJob.jobID
                                                                        && jtp.sequence == planTruckSubTripJob.jobSeq).ToList();
                if (list.Count > 0)
                {
                    TruckJobTripPlan tempTruckJobTripPlan = list[0];
                    tempTRuckJobTrip.DeleteTruckJobTripPlan(tempTruckJobTripPlan, con, tran);
                }

                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw; }
            return retValue;
        }

        public static SortableList<PlanTruckSubTrip> GetAllPlanTruckSubTrips(SortableList<PlanTruckTrip> planTruckTrips, DateTime date)
        {
            SortableList<PlanTruckSubTrip> planTruckSubTrips = new SortableList<PlanTruckSubTrip>();

            for (int i = 0; i < planTruckTrips.Count; i++)
            {
                SortableList<PlanTruckSubTrip> subTrips = planTruckTrips[i].pPlanTruckSubTrips;
                for (int j = 0; j < subTrips.Count; j++)
                {
                    planTruckSubTrips.Add(subTrips[j]);
                }
            }
            return planTruckSubTrips;
        }

        public SortableList<PlanTruckSubTripJob> GetAllPlanTruckSubTripJobs(string planTripNo)
        {
            SortableList<PlanTruckSubTripJob> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();

            planTruckSubTripJobs = PlanTruckDAL.GetPlanTruckSubTripJobsForGridDisplay(planTripNo, this);

            return planTruckSubTripJobs;
        }

        // Method is used to copy this to another PlanTruckSubTrip
        public object Clone()
        {
            PlanTruckSubTrip newPlanTruckSubTrip = new PlanTruckSubTrip();
            newPlanTruckSubTrip.SeqNo = this.SeqNo;

            //this is incomplete, it's only copy the seqNo not the whole object
            //other properties of the original PlanTruckSubTrip was not cloned
            //instead its creates a new PlanTruckSubTrip object not a cloned

            //24 Feb 2012 - Gerry Added
            //we need also these properties
            newPlanTruckSubTrip.Start = this.Start;
            newPlanTruckSubTrip.End = this.End;
            // 2014-02-27 Zhou Kai uses Clone method instead of simply 
            // assign a reference of the stop to the newPlanTruckSubTrip
            newPlanTruckSubTrip.StartStop = (Stop)this.StartStop.Clone();
            newPlanTruckSubTrip.EndStop = (Stop)this.EndStop.Clone();
            // 2014-02-27 Zhou Kai ends
            newPlanTruckSubTrip.IsBillableTrip = this.IsBillableTrip;
            newPlanTruckSubTrip.Description = this.Description;
            newPlanTruckSubTrip.DriverNumber = this.DriverNumber;
            newPlanTruckSubTrip.VehicleNumber = this.VehicleNumber;
            newPlanTruckSubTrip.Status = this.Status;
            //newPlanTruckSubTrip.planTruckSubTripJobs = this.planTruckSubTripJobs;

            //end
            // #region Removed
            ///*
            foreach (PlanTruckSubTripJob job in this.planTruckSubTripJobs)
            {
                PlanTruckSubTripJob newJob = new PlanTruckSubTripJob();
                newJob = (PlanTruckSubTripJob)job.Clone();
                newJob.planSubTripSeqNo = this.SeqNo;
                newPlanTruckSubTrip.planTruckSubTripJobs.Add(newJob);
            }
            //*/
            //  #endregion

            return newPlanTruckSubTrip;
        }

        // 2014-03-27 Zhou Kai adds comments:
        /* When draging a planTruckSubTrip from one planTruckTrip to another(changing drivers), 
            (1) The planTruckSubTrip cannot be completed already
            (2) None of the planTruckSubTripJobs should be completed inside the planTruckSubTrip
            (3) All the planTruckSubTripJobs should have the same start / end stop with the planTruckSubTrip
        */ 
        public bool CanChangeDriver(DateTime startTime, DateTime endTime, out string message)
        {
            bool flag = true;
            message = "";
            // if planTruckSubTrip is completed
            if (this.Status == JobTripStatus.Completed)
            {
                message = TptResourceBLL.ErrCantChangeDriverBecauseItIsCompleted + "\n";
                flag = false;
            }
            
            for (int i = 0; i < this.planTruckSubTripJobs.Count; i++)
            {
                // if any of the planTruckSubTripJob is completed
                if (this.planTruckSubTripJobs[i].status == JobTripStatus.Completed)
                {
                    message = TptResourceBLL.ErrCantChangeDriverBecauseItIsCompleted + "\n";
                    flag = false;
                    break;
                }
                // when changing driver for a planTruckSubTrip,
                // all its planTruckSubTripJobs should start / end at the same stop of the planTruckSubTrip
                if (this.planTruckSubTripJobs[i].startStop.Code != this.StartStop.Code)
                {
                    message = TptResourceBLL.ErrCantChangeDriverBecauseDifferentStartStop + "\n";
                    flag = false;
                    break;
                }
                if (this.planTruckSubTripJobs[i].endStop.Code != this.EndStop.Code)
                {
                    message = TptResourceBLL.ErrCantChangeDriverBecauseDifferentEndStop + "\n";
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        // Chong Chin 24 Jan 12 This method has been renamed it can also be used by other 
        // cases besides Change Driver case 
        public static bool TransferJobsFromPrecedingPlanTruckSubTrip(PlanTruckSubTrip preceding, PlanTruckTrip newPlanTruckTrip, PlanTruckSubTrip newPlanTruckSubTrip, SqlConnection con, SqlTransaction tran, string userID, string formName)
        {
            for (int i = 0; i < preceding.planTruckSubTripJobs.Count; i++)
            {
                if ((preceding.planTruckSubTripJobs[i].status != JobTripStatus.Completed) &&
                    (preceding.EndStop.Code != preceding.planTruckSubTripJobs[i].endStop.Code))
                {
                    //PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                    // Chong Chin 27 Jan 12, Start Add into in memory collection first
                    // and assign newPlanTruckSubTrip SeqNo to each planTruckSubTripJobs planTripSeq 
                    preceding.planTruckSubTripJobs[i].planSubTripSeqNo = newPlanTruckSubTrip.SeqNo;
                    //PlanTruckDAL.AddPlanTruckSubTripJobDatabase(newPlanTruckTrip, preceding.planTruckSubTripJobs[i], con, tran, out outPlanTruckSubTripJob);
                    // Chong Chin 27 Jan 12, End

                    //set preceeding planTruckSubTripJobs start and end time
                    string startTime = newPlanTruckSubTrip.Start.Hour.ToString() + newPlanTruckSubTrip.Start.Minute.ToString();
                    startTime = (startTime.Length < 4) ? startTime.Insert(0, "0") : startTime;
                    string endTime = newPlanTruckSubTrip.End.Hour.ToString() + newPlanTruckSubTrip.End.Minute.ToString();
                    endTime = (endTime.Length < 4) ? endTime.Insert(0, "0") : endTime;

                    preceding.planTruckSubTripJobs[i].startTime = startTime;
                    preceding.planTruckSubTripJobs[i].startTime = startTime;
                    //Don't write in to database yet we add first to the collection of planSubTripJobs
                    //we save to database when saving the planTruckSubTrip
                    newPlanTruckSubTrip.planTruckSubTripJobs.Add(preceding.planTruckSubTripJobs[i]);
                }
            }
            return true;
        }

        public bool CanSetToComplete()
        {
            bool flag = true;

            if (planTruckSubTripJobs.Count == 0)
            {
                flag = false;
            }

            for (int i = 0; i < this.planTruckSubTripJobs.Count; i++)
            {
                //20160907 - gerry modified the condition to complete plansubtrip if all job trip inside with same end stop are all completed
                if (this.planTruckSubTripJobs[i].status != JobTripStatus.Completed && this.planTruckSubTripJobs[i].endStop.Code == this.EndStop.Code)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }
        public bool SetPlanTruckSubTrip(JobTripStatus status, string planTripNo, SqlConnection con, SqlTransaction tran)
        {
            if (CanSetToComplete())
            {
                try
                {
                    PlanTruckDAL.SetPlanTruckSubTripStatus(JobTripStatus.Completed, planTripNo, this, con, tran);
                    this.Status = JobTripStatus.Completed;
                    this.updateVersion = PlanTruckDAL.GetPlanTruckSubTripUpdateVersion(this, con, tran);
                }
                catch (FMException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new FMException(ex.Message.ToString());
                }
            }
            else
            {
                throw new FMException(TptResourceBLL.ErrNotAllJobsCompleted);
            }
            return true;
        }

        public bool SetPlanTruckSubTrip(JobTripStatus status, string planTripNo)
        {
            bool retValue = false;
            if (CanSetToComplete())
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    PlanTruckDAL.SetPlanTruckSubTripStatus(JobTripStatus.Completed, planTripNo, this, con, tran);
                    this.Status = JobTripStatus.Completed;
                    tran.Commit();
                    retValue = true;
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
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                retValue = false;
                throw new FMException(TptResourceBLL.ErrNotAllJobsCompleted);
            }
            return retValue;
        }

        /*
         * 2014-04-07 Zhou Kai reads this function and adds comments:
         * 
         */
        // 16/1/12 Get TruckJobTrips belongs to this planTruckSubTrip out from all truckJobTrips belongs to a TruckJob
        public SortableList<TruckJobTrip> GetTruckJobTripForPlanSubTrip(SortableList<TruckJobTrip> parentTruckJobTrips /*, PlanTruckSubTrip planTruckSubTrip*/)
        {
            SortableList<TruckJobTrip> truckJobTrips = new SortableList<TruckJobTrip>();

            for (int i = 0; i < parentTruckJobTrips.Count; i++)
            {
                //1st condition - start stop must be the same and status must not be complete
                if (parentTruckJobTrips[i].StartStop.Code.ToString().Trim() == this.StartStop.Code.ToString().Trim()//planTruckSubTrip.StartStop 
                    && parentTruckJobTrips[i].TripStatus != JobTripStatus.Completed)
                {
                    //2nd condition - the jobID && seq No must not be in this planSubTrip,
                    //because the truckJobTrips with the same jobID and SeqNo are already
                    //there in the truckJobTrips list.
                    if (this.planTruckSubTripJobs.Count != 0)
                    {
                        // complete the code here  
                        for (int j = 0; j < this.planTruckSubTripJobs.Count; j++)
                        {
                            if ((int)parentTruckJobTrips[i].JobID != (int)this.planTruckSubTripJobs[j].jobID)
                            {
                                truckJobTrips.Add(parentTruckJobTrips[i]);
                            }
                            else
                            {
                                if ((int)parentTruckJobTrips[i].Sequence != (int)this.planTruckSubTripJobs[j].jobSeq)
                                    truckJobTrips.Add(parentTruckJobTrips[i]);
                            }
                        }
                    }
                     // 2014-04-07 Zhou Kai adds comments:
                    // When the planTruckSubTrip is empty, the criterial is: 
                    // both start and end stop of the truckJobTrip equal to
                    // those of this planTruckSubTrip
                    // since this plan sub trip has not truck job trips
                    else
                    {
                        if (parentTruckJobTrips[i].EndStop.Code.ToString().Trim() == this.EndStop.Code.ToString().Trim())
                        {
                            truckJobTrips.Add(parentTruckJobTrips[i]);
                        }
                    }
                }
            }
            return truckJobTrips;
        }       

        //This method is to be use when 
        //add new PlanTruckSubTrip or Change to other driver
        //to cater preceeding planTruckSubTrip
        public bool AddPlanTruckSubTrip(PlanTruckTrip planTruckTrip, SqlConnection con, SqlTransaction tran, string frmName, string userID)
        {
            bool retValue = false;
            try
            {
                PlanTruckSubTrip preceedingPlanTruckSubTrip = new PlanTruckSubTrip();
                int indexPrecedingTruckSubTrip = -1;
                if (planTruckTrip.pPlanTruckSubTrips.Count != 0)
                {
                    planTruckTrip.planTruckSubTrips.SortByStartTime();
                    indexPrecedingTruckSubTrip = planTruckTrip.GetPrecedingPlanTruckSubTripIndex(this.Start, this.End);
                    if (indexPrecedingTruckSubTrip >= 0)
                    {
                        preceedingPlanTruckSubTrip = planTruckTrip.pPlanTruckSubTrips[indexPrecedingTruckSubTrip];
                        PlanTruckSubTrip.TransferJobsFromPrecedingPlanTruckSubTrip(preceedingPlanTruckSubTrip, planTruckTrip, this, con, tran, userID, frmName);
                    }
                }
                // Now after getting the index of existing collection, add newPlanTruckTrip into database and collection
                // AddPlanTruckSubTrip method should pass in connection and transaction
                planTruckTrip.AddPlanTruckSubTrip(this, frmName, userID, con, tran);
                planTruckTrip.planTruckSubTrips.Add(this);

                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }

            return retValue;
        }
        public bool AddPlanTruckSubTrip(PlanTruckTrip planTruckTrip, string frmName, string userID)
        {
            bool retValue = false;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    PlanTruckSubTrip preceedingPlanTruckSubTrip = new PlanTruckSubTrip();
                    int indexPrecedingTruckSubTrip = -1;
                    if (planTruckTrip.pPlanTruckSubTrips.Count != 0)
                    {
                        planTruckTrip.planTruckSubTrips.SortByStartTime();
                        indexPrecedingTruckSubTrip = planTruckTrip.GetPrecedingPlanTruckSubTripIndex(this.Start, this.End);
                        if (indexPrecedingTruckSubTrip >= 0)
                        {
                            preceedingPlanTruckSubTrip = planTruckTrip.pPlanTruckSubTrips[indexPrecedingTruckSubTrip];
                            PlanTruckSubTrip.TransferJobsFromPrecedingPlanTruckSubTrip(preceedingPlanTruckSubTrip, planTruckTrip, this, con, tran, userID, frmName);
                        }
                    }
                    // Now after getting the index of existing collection, add newPlanTruckTrip into database and collection
                    // AddPlanTruckSubTrip method should pass in connection and transaction
                    planTruckTrip.AddPlanTruckSubTrip(this, frmName, userID, con, tran);
                    planTruckTrip.planTruckSubTrips.Add(this);

                    retValue = true;
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (Exception ex) { throw ex; }
                finally { if (con.State != ConnectionState.Closed) con.Close(); }
            }
            return retValue;
        }

        /*
         * This function update these database tables and objects:
         * (1) planTruckSubTripJob and TRK_PLAN_TRUCK_SUB_TRIP_JOB_TBL
         * (2) truckJobTripPlan    and TRK_TRUCK_DETAIL_TRIP_PLAN_TBL
         * (3) truckJobTrip        and TRK_TRUCK_DETAIL_TRIP
         * 
         * Notes:
         * (1) updating those three database tables in one transaction and using one same database connection
         * (2) check the values of update_version in object and that in database before updating
         * (3) if one step failed, roll back the whole transaction
         * (4) The logic is: 
         *     a. Actual Weight/Volume changed from dataGridView on UI
         *     b. Get current planTruckSubTripJob from that dataGridView, it's binding to dataGridView
         *     c. Update the edited planTruckSubTripJob to database, 
         *        and get planTruckSubTripJob from database(not necessary)
         *     d. Get trcukJobTripPlan object from database, update its acutal weight/volume, and
         *        update it to database table 
         *     e. Select and add sum of all the weight/volume in truckJobTripPlans which belong
         *        to the truckJobTrip, and update that weight/volume to TRK_TRUCK_DETAIL_TRIP as
         *        actual weight/volume
         */
        public bool UpdateWeightAndVolume(PlanTruckSubTripJob planTruckSubTripJob, string frmName, string userID)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (this.StartStop.Code.ToString() == planTruckSubTripJob.startStop.Code.ToString())
                {
                    // UPDATE TRK_PLAN_SUB_TRIP_JOB_TBL
                    PlanTruckDAL.UpdateWeightAndVolume(planTruckSubTripJob, con, tran);

                    // Update truckJobTripPlan
                    TruckJobTripPlan truckJobTripPlan = TruckJobTripPlan.GetTruckJobTripPlan(planTruckSubTripJob.jobID,
                                                            planTruckSubTripJob.jobSeq,
                                                            planTruckSubTripJob.planTripNo,
                                                            planTruckSubTripJob.planSubTripSeqNo,
                                                            planTruckSubTripJob.planSubTripJobSeqNo,
                                                            con, tran);
                    truckJobTripPlan.volume = planTruckSubTripJob.volume;
                    truckJobTripPlan.weight = planTruckSubTripJob.weight;
                    // 2013-09-11 Zhou Kai adds logic to update actual weight/volume
                    string newStartDateTime = String.Empty;
                    string newEndDateTime = String.Empty;
                    newStartDateTime = planTruckSubTripJob.startDate.ToShortDateString();
                    newEndDateTime = planTruckSubTripJob.endDate.ToShortDateString();
                    newStartDateTime = newStartDateTime + " " + planTruckSubTripJob.startTime.Insert(2, ":");
                    newEndDateTime = newEndDateTime + " " + planTruckSubTripJob.endTime.Insert(2, ":");
                    truckJobTripPlan.start = Convert.ToDateTime(newStartDateTime);
                    truckJobTripPlan.end = Convert.ToDateTime(newEndDateTime);
                    // Update TRK_JOB_DETAIL_TRIP_PLAN_TBL with weight/volume, one truck job detail trip
                    // contains one or more truck job detail trip detail
                    TruckJobTripPlan.UpdateWeightAndVol(truckJobTripPlan, con, tran);

                    // UPDATE TRK_JOB_DETAIL_TRIP_TBL, one truck job detail trip contains one
                    // or some truck job deatil trip plans.
                    TruckJobTrip.UpdateAcutalWeightVolumeforTruckJobTrip(planTruckSubTripJob.jobID,
                                                                         planTruckSubTripJob.jobSeq, con, tran);
                    retValue = true;
                    tran.Commit();
                }
                else
                {
                    throw new FMException(TptResourceBLL.ErrDifferentStartStop);
                }
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); con.Dispose(); }

            return retValue;
        }

        // 2013-12-06 Zhou Kai modifies these two functions to make them in one transaction
        public bool UpdateActualStartEndTime(PlanTruckSubTripJob newPlanTruckSubTripJob, string frmName, string usrId,
             DateTime new_start_date_time, DateTime new_end_date_time)
        {
            bool retVal = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //20131209 - gerry removed no such method in previous DAL version
                // 2013-12-10 Zhou Kai uncomment this function
                PlanTruckDAL.UpdateActualStartEndTime(newPlanTruckSubTripJob, frmName, usrId,
                                new_start_date_time, new_end_date_time, con, tran);


                retVal = true;
                tran.Commit();
                // add log here:
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (SqlException ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
            finally { con.Close(); }

            return retVal;
        }

        /*
         *2013-09-13 Zhou Kai adds this function
         */
        public bool UpdateWeightAndVolumeWhenAddNewTruckJobTripPlan(PlanTruckSubTripJob planTruckSubTripJob,
                                                                    TruckJobTripPlan truckJobTripPlan,
                                                                    string frmName, string userID,
                                                                    SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                if (this.StartStop.Code.ToString() == planTruckSubTripJob.startStop.Code.ToString())
                {
                    // Update planTruckSubTripJob
                    PlanTruckDAL.UpdateWeightAndVolume(planTruckSubTripJob, con, tran);
                    // Update truckJobTripPlan
                    TruckJobTripPlan.UpdateWeightAndVol(truckJobTripPlan, con, tran);
                    // Update truckJobTrip
                    TruckJobTrip.UpdateAcutalWeightVolumeforTruckJobTrip(planTruckSubTripJob.jobID,
                                                                         planTruckSubTripJob.jobSeq, con, tran);
                    retValue = true;
                }
                else
                {
                    throw new FMException(TptResourceBLL.ErrDifferentStartStop);
                }
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }

            return retValue;
        }
       
        //20151223 - gerry start modify

        public static bool HasPlanTruckSubTripChanged(PlanTruckSubTrip planTruckSubTrip)
        {
            return PlanTruckDAL.HasPlanTruckSubTripChanged(planTruckSubTrip);
        }
        public static bool HasPlanTruckSubTripChanged(PlanTruckSubTrip planTruckSubTrip, SqlConnection con, SqlTransaction tran)
        {
            return PlanTruckDAL.HasPlanTruckSubTripChanged(planTruckSubTrip, con, tran);
        }
        //20160219 - gerry added
        public decimal GetTotalVolumeCarried()
        {
            decimal retValue = 0;
            foreach (PlanTruckSubTripJob ptsj in this.planTruckSubTripJobs)
            {
                if (ptsj.status != JobTripStatus.Completed)
                {
                    retValue += ptsj.volume;
                }
            }
            return retValue;
        }
        public decimal GetTotalWeightCarried()
        {
            decimal retValue = 0;
            foreach (PlanTruckSubTripJob ptsj in this.planTruckSubTripJobs)
            {
                if (ptsj.status != JobTripStatus.Completed)
                {
                    retValue += ptsj.weight;
                }
            }
            return retValue;
        }
        //20160405 - gery added to create plan truck sub trip job
        public PlanTruckSubTripJob CreatePlanTruckSubTripJob(TruckJobTrip jobTrip, decimal volume, decimal weight)
        {
            PlanTruckSubTripJob newPlanTruckSubTripJob = null;
            try
            {
                newPlanTruckSubTripJob = new PlanTruckSubTripJob();
                newPlanTruckSubTripJob.planTripNo = this.PlanTripNo;
                newPlanTruckSubTripJob.planSubTripSeqNo = this.SeqNo;
                newPlanTruckSubTripJob.jobID = jobTrip.JobID;
                newPlanTruckSubTripJob.jobSeq = jobTrip.Sequence;
                newPlanTruckSubTripJob.custCode = jobTrip.CustomerCode;
                newPlanTruckSubTripJob.custName = jobTrip.CustomerName;
                newPlanTruckSubTripJob.startStop = jobTrip.StartStop;
                newPlanTruckSubTripJob.endStop = jobTrip.EndStop;
                newPlanTruckSubTripJob.startDate = this.Start;
                newPlanTruckSubTripJob.endDate = this.End;
                string startHr = this.Start.Hour.ToString().Length == 1 ? "0" + this.Start.Hour.ToString() : this.Start.Hour.ToString();
                string startMin = this.Start.Minute.ToString().Length == 1 ? "0" + this.Start.Minute.ToString() : this.Start.Minute.ToString();
                string strStartTime = string.Concat(startHr, startMin);

                string endHr = this.End.Hour.ToString().Length == 1 ? "0" + this.End.Hour.ToString() : this.End.Hour.ToString();
                string endMin = this.End.Minute.ToString().Length == 1 ? "0" + this.End.Minute.ToString() : this.End.Minute.ToString();
                string strEndTime = string.Concat(endHr, endMin);

                newPlanTruckSubTripJob.startTime = strStartTime;
                newPlanTruckSubTripJob.endTime = strEndTime;
                newPlanTruckSubTripJob.volume = (volume == 0 ? jobTrip.GetBalanceVolForPlan() : volume);
                newPlanTruckSubTripJob.weight = (weight == 0 ? jobTrip.GetBalanceWeightForPlan() : weight);
                newPlanTruckSubTripJob.status = JobTripStatus.Assigned;
                foreach (TruckJobTripDetail cargoDetail in jobTrip.truckJobTripDetail)
                {
                    //20170201 - gerry added
                    //decimal tempTotalWt = cargoDetail.balQty * cargoDetail.unitWeight;
                    //int neededQty = Convert.ToInt32(tempTotalWt / cargoDetail.unitWeight);
                    //int neededQty = loadedQty == 0 ? cargoDetail.quantity : loadedQty;//20170811

                    PlanTruckSubTripJobDetail tempPlanCargoDeatil = PlanTruckSubTripJobDetail.CreatePlanTruckSubTripJobDetail(cargoDetail.balQty,
                                                    newPlanTruckSubTripJob, cargoDetail);
                    newPlanTruckSubTripJob.planTruckSubTripJobDetails.Add(tempPlanCargoDeatil);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) {  throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return newPlanTruckSubTripJob;
        }
        //20160419 - gerry added to add plantrucksubtripJob inside plantrucksubtrip
        public bool AddPlanTruckSubTripJobToPlanTruckSubTrips(PlanTruckTrip planTruckTrip, TruckJobTrip truckJobTrip, decimal volume, decimal weight, string frmName, string user, string deptCode)
        {
            bool flag = true;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    //check if the job trip is the same start with plan sub trip
                    if (this.StartStop.Code.Trim() != truckJobTrip.StartStop.Code.Trim())
                        throw new FMException(TptResourceBLL.ErrStartStopNotSame);
                    string msgOverLoad = string.Empty;
                    decimal newWeight = GetTotalWeightCarried() + weight;
                    decimal newVolume = GetTotalVolumeCarried() + volume;
                    PlanTruckSubTripJob planTruckSubTripJob = CreatePlanTruckSubTripJob(truckJobTrip, volume, weight);
                    Vehicle currentTruck = Vehicle.GetVehicle(planTruckTrip.Driver.defaultVehicleNumber);
                    planTruckTrip.CheckTruckCapacityIsOverload(newVolume, newWeight, currentTruck, out msgOverLoad, false);
                    if (!msgOverLoad.Equals(string.Empty))
                        throw new FMException(msgOverLoad);
                    //check if there is plan plan subtrip with the same end stop; if not then auto create plan subtrip
                    bool foundSameEndStop = false;
                    PlanTruckSubTrip nextPlanTruckSubTrip = null;
                    for (int i = 0; i < planTruckTrip.pPlanTruckSubTrips.Count; i++)
                    {   // The codes below is to check if the new truckJobTrip will stop at any following planTruckSubTrip's end stop
                        if (planTruckTrip.pPlanTruckSubTrips[i].EndStop.Code == planTruckSubTripJob.endStop.Code)
                        {
                            if ((DateTime)planTruckTrip.pPlanTruckSubTrips[i].Start >= (DateTime)planTruckSubTripJob.startDate)
                            {
                                foundSameEndStop = true;
                                nextPlanTruckSubTrip = planTruckTrip.pPlanTruckSubTrips[i];
                            }
                        }
                    }
                    //now add plantrucksubtripJob tp databaase
                    PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                    PlanTruckDAL.AddPlanTruckSubTripJobDatabase(planTruckTrip, this, planTruckSubTripJob, con, tran, out outPlanTruckSubTripJob, user, frmName);

                    if (!foundSameEndStop)
                    { //auto create plansubtrip with 30min time by default user can modify time 
                        SortableList<TruckJobTrip> jobTrips = new SortableList<TruckJobTrip>();
                        jobTrips.Add(truckJobTrip);
                        nextPlanTruckSubTrip = planTruckTrip.CreateNextPlanTruckSubTrip(this.Start, this.EndStop, outPlanTruckSubTripJob, this.End, this.End.AddMinutes(30), frmName, user, deptCode, con, tran);
                    }
                    else
                    { //if nextPlanTruckSubTrip found
                        PlanTruckSubTripJob outPlanTruckSubTripJob1 = new PlanTruckSubTripJob();
                        planTruckSubTripJob.planSubTripSeqNo = nextPlanTruckSubTrip.SeqNo;
                        PlanTruckDAL.AddPlanTruckSubTripJobDatabase(planTruckTrip, this, planTruckSubTripJob, con, tran, out outPlanTruckSubTripJob1, user, frmName);
                        nextPlanTruckSubTrip.planTruckSubTripJobs.Add(outPlanTruckSubTripJob1);
                    }
                    //now add plantrucksubtripJob to memory
                    this.planTruckSubTripJobs.Add(outPlanTruckSubTripJob);

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
                return flag;
            }
        }
        public bool DeletePlanTruckSubTripJob(PlanTruckSubTripJob planTruckSubTripJob)
        {
            bool retValue = false;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    retValue = DeletePlanTruckSubTripJob(planTruckSubTripJob, con, tran);
                    //if truckJobTripPlan count is equal to 0 then set back the status to ready
                    TruckJobTrip tempTRuckJobTrip = TruckJobTrip.GetTruckJobTrip(planTruckSubTripJob.jobID, planTruckSubTripJob.jobSeq, con, tran);
                    if (tempTRuckJobTrip.truckJobTripPlans.Count == 0)
                    {
                        JobTripState truckJobTripState = new JobTripState(1, JobTripStatus.Ready, DateTime.Now.Date,"system setback to ready after deleted from planning",true);

                        tempTRuckJobTrip.SetJobTripStatusForTruck(truckJobTripState, con, tran, "", "", "Assigned");
                    }


                    tran.Commit();
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (Exception ex) { throw ex; }
                finally { con.Close(); }
            }
            return retValue;
        }
        //20160820 - gerry added for easy cargo
        public static void UpdatePlanTruckSubTripShipmentId(PlanTruckSubTrip planSubTrip)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    tran = con.BeginTransaction();

                    PlanTruckDAL.UpdatePlanTruckSubTripShipmentId(planSubTrip, con, tran);

                    tran.Commit();
                }
                catch (FMException fmEx) { if (tran != null) { tran.Rollback(); } throw fmEx; }
                catch (Exception ex) { if (tran != null) { tran.Rollback(); } throw ex; }
                finally { con.Close(); }
            }
        }
        //20160908 - gerry added for cloud track
        public static bool UpdatePlanTruckSubTripCloudtrack_Job_id(PlanTruckSubTrip planSubTrip)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    PlanTruckDAL.UpdatePlanTruckSubTripCloudtrack_Job_id(planSubTrip, con, tran);
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
            }
            return true;
        }
        public static bool UpdatePlanTruckSubTripCloudtrack_RoutePoint_id(PlanTruckSubTrip planSubTrip)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    PlanTruckDAL.UpdatePlanTruckSubTripCloudtrack_RoutePoint_id(planSubTrip, con, tran);
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
            }
            return true;
        }
        public static bool InsertRouteLocation(TaskLocation taskLocation)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    PlanTruckDAL.InsertRouteLocation(taskLocation, con, tran);
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
            }
            return true;
        }
        //20170220
        public bool IsTaskAssignedInDriver()
        {
            return PlanTruckDAL.IsTaskAssignedInDriver(this);
        }
        public PlanTruckSubTrip GetPlanTruckSubTrip(PlanTruckTrip planTruckTrip, string message_TaskID)
        {
            return PlanTruckDAL.GetPlanTruckSubTrip(planTruckTrip, message_TaskID);
        }
    }
}
