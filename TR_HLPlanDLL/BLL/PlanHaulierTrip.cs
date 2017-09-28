using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_HLBookDLL.BLL;
using FM.TR_HLPlanDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using TR_LanguageResource.Resources;
using System.ComponentModel;
using TR_MessageDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_HLBookDLL.Facade;

namespace FM.TR_HLPlanDLL.BLL
{
    public class PlanHaulierTrip : PlanTrip
    {
        private PlanHaulierSubTrips haulierSubTrips = new PlanHaulierSubTrips();
        private PlanHaulierSubTrips oldhaulierSubTrips = new PlanHaulierSubTrips();


        public PlanHaulierTrip()
            : base()
        {
            this.haulierSubTrips = new PlanHaulierSubTrips();
            this.oldhaulierSubTrips = new PlanHaulierSubTrips();
        }

        public PlanHaulierTrip(string planTripNo, DateTime scheduleDate, Vehicle vehicle, Driver driver, byte[] updateversion)
            : base(planTripNo, scheduleDate, vehicle, driver, updateversion)
        {
            this.haulierSubTrips = new PlanHaulierSubTrips();
            this.oldhaulierSubTrips = new PlanHaulierSubTrips();
        }

        public PlanHaulierSubTrips HaulierSubTrips
        {
            get { return haulierSubTrips; }
            set { haulierSubTrips = value; }
        }

        public PlanHaulierSubTrips OldHaulierSubTrips
        {
            get { return oldhaulierSubTrips; }
            set { oldhaulierSubTrips = value; }
        }

        public static SortableList<DateTime> GetPlanTripDates(DateTime fromDate, DateTime toDate, string tptDept)
        {
            return PlanTransportDAL.GetPlanTripDates(fromDate, toDate, tptDept);
        }

        public static SortableList<PlanHaulierTrip> GetAllPlanHaulierTripsByDay(DateTime date)
        {

            return PlanTransportDAL.GetAllPlanHaulierTripsByDay(date);
        }

        public static SortableList<PlanHaulierTrip> GetAllPlanHaulierTripsByDayAndDept(DateTime date, string tptDept)
        {

            return PlanTransportDAL.GetAllPlanHaulierTripsByDayAndDept(date, tptDept);
        }

        public static bool FindCurrentHaulierTrip(SortableList<PlanHaulierTrip> planHaulierTrips, Vehicle vehicle)
        {
            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                if (planHaulierTrips[i].Vehicle.Number == vehicle.Number)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetHaulierTripIndex(SortableList<PlanHaulierTrip> planHaulierTrips, Vehicle vehicle)
        {
            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                if (planHaulierTrips[i].Vehicle.Number == vehicle.Number)
                {
                    return i;
                }
            }
            return 0;
        }

        public static PlanHaulierTrip GetCurrentHaulierTrip(SortableList<PlanHaulierTrip> planHaulierTrips,
            string planTripNo, DateTime scheduleDate, Vehicle vehicle,
            Driver driver, Vehicle trailer)
        {
            PlanHaulierTrip tempPlanHaulierTrip = new PlanHaulierTrip();

            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                if (planHaulierTrips[i].Vehicle.Number == vehicle.Number)
                {
                    tempPlanHaulierTrip = planHaulierTrips[i];
                }
                else
                {
                    PlanHaulierTrip planHaulierTrip1 = new PlanHaulierTrip(planTripNo, scheduleDate, vehicle, driver, new byte[8]);
                    tempPlanHaulierTrip = planHaulierTrip1;
                }
            }
            return tempPlanHaulierTrip;
        }

        public static void SwapUp(SortableList<PlanHaulierSubTrip> haulierSubTrips, int planHaulierTripindex, int planHaulierSubTripindex)
        {
            PlanHaulierSubTrip tempPlanHaulierSubTrip;
            int i = planHaulierTripindex;
            int j = planHaulierSubTripindex;
            if (j != 0)
            {

                haulierSubTrips[j - 1].SeqNo = 0;
                haulierSubTrips[j].SeqNo = haulierSubTrips[j].SeqNo - 1;
                haulierSubTrips[j - 1].SeqNo = haulierSubTrips[j].SeqNo + 1;

                tempPlanHaulierSubTrip = haulierSubTrips[j - 1];
                haulierSubTrips[j - 1] = haulierSubTrips[j];
                haulierSubTrips[j] = tempPlanHaulierSubTrip;

            }
        }

        public static void SwapDown(SortableList<PlanHaulierSubTrip> haulierSubTrips, int planHaulierTripindex, int planHaulierSubTripindex)
        {
            PlanHaulierSubTrip tempPlanHaulierSubTrip;
            int i = planHaulierTripindex;
            int j = planHaulierSubTripindex;
            if (j != haulierSubTrips.Count - 1)
            {
                haulierSubTrips[j + 1].SeqNo = 0;
                haulierSubTrips[j].SeqNo = haulierSubTrips[j].SeqNo + 1;
                haulierSubTrips[j + 1].SeqNo = haulierSubTrips[j].SeqNo - 1;

                tempPlanHaulierSubTrip = haulierSubTrips[j + 1];
                haulierSubTrips[j + 1] = haulierSubTrips[j];
                haulierSubTrips[j] = tempPlanHaulierSubTrip;

            }
        }

        public static string SubTripValidation(SortableList<PlanHaulierSubTrip> planHaulierSubTrips)
        {
            string message = "";
            int index = 0;
            for (int i = 0; i < planHaulierSubTrips.Count; i++)
            {
                if (i != 0)
                {
                    index = i + 1;
                    if (planHaulierSubTrips[i].StartStop.Code != planHaulierSubTrips[i - 1].EndStop.Code)
                    {
                        message = message + "Error at line " + index.ToString() + " Start Stop must be the same with prev End stop \n";
                    }
                    if (DateTime.Compare(planHaulierSubTrips[i].Start, planHaulierSubTrips[i].End) > 0)
                    {
                        message = message + "Error at line " + index.ToString() + " Start Time must be earlier than End Time \n";
                    }
                }
            }
            return message;


        }

        public static PlanHaulierTrip FindPlanHaulierTripByVehicle(SortableList<PlanHaulierTrip> planHaulierTrips, Vehicle vehicle)
        {
            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                if (planHaulierTrips[i].Driver.defaultVehicleNumber.Trim() == vehicle.Number.Trim())
                {
                    return planHaulierTrips[i];
                    break;
                }
            }
            return null;
        }

        public static PlanHaulierTrip FindPlanHaulierTripByDriver(SortableList<PlanHaulierTrip> planHaulierTrips, Driver driver)
        {
            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                if (planHaulierTrips[i].Driver.Code == driver.Code)
                {
                    return planHaulierTrips[i];
                    break;
                }
            }
            return null;
        }


        public void AddSubTrip(PlanHaulierSubTrip planHaulierSubTrip)
        {
            planHaulierSubTrip.VehicleNumber = this.Vehicle.Number;
            planHaulierSubTrip.SeqNo = this.HaulierSubTrips.Count + 1;
            this.HaulierSubTrips.Add(planHaulierSubTrip);

        }

        public void DeleteSubTrip(PlanHaulierSubTrip planHaulierSubTrip)
        {
            this.HaulierSubTrips.Remove(planHaulierSubTrip);
        }


        public void AddPlanHaulierTripDatabase(SqlConnection cn, SqlTransaction tran, string tptDept)
        {
            PlanTransportDAL.AddPlanHaulierTripDatabase(this, cn, tran, tptDept);
        }

        public void DeletePlanHaulierTripDatabase(SqlConnection cn, SqlTransaction tran)
        {
            PlanTransportDAL.DeletePlanHaulierTripDatabase(this, cn, tran);
        }

        public static byte[] GetPlanHaulierTripLatestUpdateVersion(PlanHaulierTrip planHaulierTrip)
        {
            return PlanTransportDAL.GetPlanHaulierTripUpdateVersion(planHaulierTrip);
        }
        public static bool HasPlanTripsChanged(PlanHaulierTrip planHaulierTrip)
        {
            return PlanTransportDAL.HasPlanTripsChanged(planHaulierTrip);
        }
        public static void AddAffectedPlanTrip(SortableList<PlanHaulierTrip> memPlanHaulierTrips, SortableList<PlanHaulierTrip> affectedPlanHaulierTrips, PlanHaulierTrip currentPlanTrip)
        {
            try
            {
                foreach (PlanHaulierTrip memPlanTrip in memPlanHaulierTrips)//loop the entire collection of plan trips
                {
                    if (currentPlanTrip.Driver == memPlanTrip.Driver)//compare the currentPlanTrip base on driver
                    {   //filter to add current plan trip
                        if ((affectedPlanHaulierTrips.Where(pt => pt.Driver == memPlanTrip.Driver)).ToList().Count == 0)
                        {
                            affectedPlanHaulierTrips.Add(memPlanTrip);
                            break;
                        }
                    }
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        //public static SortableList<PlanHaulierTrip> GetAffectedPlanTrip()
        //20150505 - gerry new method of saving the planning
        public static void UpdateAllAffectedHaulierPlanTrips(SortableList<PlanHaulierTrip> affectedPlanTrips, SortableList<TrailerLocation> trailerlocations, string tptDept, Hashtable trailerlocationhashtbl
            , string userID, string frmName, SortableList<HaulierJobTrip> subConJobTrips, TripUpdate02 tripUpdate, MessageTripStatus status)
        {
            bool autoSetNextLegToReady = ApplicationOption.GetApplicationOption(ApplicationOption.HAULAGE_SETTINGS_ID,
                ApplicationOption.SETTINGS_AUTO_UPDATE_STATUS_NEXT_LEG).setting_Value == "T" ? true : false;            
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            using (con)
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                DateTime plandate;
                try
                {
                    //20150805 - gerry modified if jobTripsNotAssignedToDriver is not null
                    //TODO  update jobtrips which is assigned to a sub contractor
                    if (subConJobTrips != null)
                    {
                        foreach (HaulierJobTrip jobTrip in subConJobTrips)
                        {
                            if (jobTrip.subCon.Code != string.Empty)
                            {       ///20140609 - gerry changed the method name and parameters from HaulierJobTrip class
                                jobTrip.AddSubContractorFromPlanning(con, tran);

                                //20151109 - gerry added new audit log
                                AuditLog auditLog = new AuditLog(jobTrip.JobNo, "TR", "PL", jobTrip.JobID, userID, DateTime.Now, frmName, jobTrip.Sequence, FormMode.Edit.ToString());
                                auditLog.WriteAuditLog("Subcon", jobTrip.subCon.Code.ToString(), "", con, tran);
                            }
                            if (!jobTrip.isMovementJob)
                            {
                                HaulierJob haulierJob = HaulierJob.GetHaulierJob(jobTrip.JobNo);
                            }
                        }
                    }
                    //TODO delete affected plan trip, each plan subtrip and each job trip inside
                    if (affectedPlanTrips.Count > 0)
                    { //TODO code here delete affected plan trip
                        foreach (PlanHaulierTrip affectedPT in affectedPlanTrips)
                        {
                            affectedPT.DeletePlanHaulierTripDatabase(con, tran);
                        }
                    }
                    //TODO set updated job trips properties if any changes
                    SetPlanHaulierJobTripsPropertiesIfChanged(affectedPlanTrips, userID, frmName, autoSetNextLegToReady, con, tran);
                    //affected trailation used to saved only those affected trailers
                    SortableList<TrailerLocation> affectedTrailerLocations = new SortableList<TrailerLocation>();
                    //TODO Add affected plan trip, each plan subtrip and each job trip inside
                    if (affectedPlanTrips.Count > 0)
                    { //TODO code here delete affected plan trip
                        foreach (PlanHaulierTrip affectedPT in affectedPlanTrips)
                        {
                            foreach (PlanHaulierSubTrip subTrip in affectedPT.HaulierSubTrips)
                            {
                                foreach (HaulierJobTrip trip in subTrip.JobTrips)
                                {//get update version of each job trip
                                    trip.UpdateVersion = trip.GetHaulierJobTripUpdateVersion(con, tran);
                                }
                                //get update version of each PlanHaulierSubTrip
                                //subTrip.updateVersion = PlanTransportDAL.GetPlanHaulierSubTripUpdateVersion(affectedPT.PlanTripNo, subTrip, con, tran);

                                //20151123 - gerry added to get the affected trailerlocation
                                //TrailerLocation.DeleteTrailerLocationByDriverAndStartTime(subTrip.DriverNumber, subTrip.Start, tptDept, con, tran);
                                //TrailerLocation.GetAffectedTrailerLocations(affectedTrailerLocations, trailerlocations, affectedPT.Driver.Code, subTrip.Start, tptDept);
                                
                            }
                            //save to db
                            affectedPT.AddPlanHaulierTripDatabase(con, tran, tptDept);
                            //20160104 - gerry added here the update of trailer locations
                            TrailerLocation.AddAffectedTrailerLocationsForParticularDateAndDriver(ref trailerlocations, affectedPT.Driver.Code, affectedPT.ScheduleDate, tptDept, con, tran);
                            //get update version of each affected driver or plantrips
                            affectedPT.UpdateVersion = PlanTransportDAL.GetPlanHaulierTripUpdateVersion(affectedPT, con, tran);
                            //Clone
                            affectedPT.CopyPlanHaulierSubTripsToOldPlanHaulierSubTrips();
                            //TODO write back affected trailer location to DB
                        }
                    }
                    ////TODO SAVE TRAILER LOCATION
                    //if (trailerlocations.Count > 0)
                    //{
                    //    TrailerLocation.AddTrailerLocations(trailerlocations, con, tran, affectedPlanTrips[0].ScheduleDate.Date, tptDept, trailerlocationhashtbl);
                    //}
                    //if from message then update message table
                    if (tripUpdate != null)
                    {
                        //TripUpdate01.EditTripUpdate(tripUpdate, status, con, tran);
                        TripUpdate02.SetTripUpdateStatus(tripUpdate, status, frmName, userID, con, tran);
                        //update instruction status
                        TripInstruction02.SetTripInstructionStatus(status, tripUpdate.PrimeMover, tripUpdate.HeadNo, tripUpdate.SeqNo, tripUpdate.TaskID, con, tran);
                    }
                    //if (affectedTrailerLocations.Count > 0)
                    //{
                    //    TrailerLocation.AddAffectedTrailerLocations(affectedTrailerLocations, tptDept, con, tran);                       
                    //}
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
       
        //20160107 -gerry added to create plan trip each driver being assigned for that day
        public static void AddPlanTripsForAllDrivers(SortableList<Driver> assignedDrivers, DateTime scheduleDate, string userID, string frmName, string tptDept)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                foreach (Driver driver in assignedDrivers)
                {
                    Vehicle vehicle = Vehicle.GetVehicle(driver.defaultVehicleNumber);
                    PlanHaulierTrip planTrip = new PlanHaulierTrip("", scheduleDate, vehicle, driver, new byte[8]);
                    planTrip.AddPlanHaulierTripDatabase(con, tran, tptDept);

                    //added new audit log
                    AuditLog auditLog = new AuditLog(driver.defaultVehicleNumber, "TR", "PL", 0, userID, DateTime.Now, frmName, 0, FormMode.Add.ToString());
                    auditLog.WriteAuditLog(planTrip, null, con, tran);
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
            
        }

        //20140521 - gerry adde 1 parameter jobTrips
        //reason : some jobtrip might be assigned to a sub contractor
        public static bool AddAllPlanHaulierTrips(SortableList<PlanHaulierTrip> planHaulierTrips, SortableList<TrailerLocation> trailerlocations, string tptDept, Hashtable trailerlocationhashtbl
            , string userID, string frmName, SortableList<HaulierJobTrip> subConJobTrips)
        {

            bool autoSetNextLegToReady = ApplicationOption.GetApplicationOption(ApplicationOption.HAULAGE_SETTINGS_ID,
                ApplicationOption.SETTINGS_AUTO_UPDATE_STATUS_NEXT_LEG).setting_Value == "T" ? true : false;
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //03 March 2012 - Gerry replaced this code
                //because an error will uccor when saving without jobtrip assigned.
                //DateTime plandate = planHaulierTrips[0].ScheduleDate;

                DateTime plandate;
                if (planHaulierTrips.Count > 0)
                    plandate = planHaulierTrips[0].ScheduleDate;
                else
                    throw new FMException(TptResourceBLL.ErrCantSavePlanWOjobtrip);

                //end 03 March 2012

                //20120911 - to prevent multi user conflict                 
                //TODO Check if the plantrip update version is changed
                foreach (PlanHaulierTrip planTrip in planHaulierTrips)
                {
                    //this case only check if not newly added driver
                    if (!PlanTransportDAL.IsNewPlanTrip(planTrip, con, tran))
                    {
                        if (!PlanTransportDAL.HasPlanTripsChanged(planTrip, con, tran))
                        {
                            break;
                        }
                    }
                }
                //20120911  
                for (int i = 0; i < planHaulierTrips.Count; i++)
                {
                    planHaulierTrips[i].DeletePlanHaulierTripDatabase(con, tran);
                }     
          
                //20140521 - gerry added to update jobtrips which is assigned to a sub contractor
                foreach (HaulierJobTrip jobTrip in subConJobTrips)
                {
                    if (jobTrip.subCon.Code != string.Empty)
                    {       ///20140609 - gerry changed the method name and parameters from HaulierJobTrip class
                        jobTrip.AddSubContractorFromPlanning(con, tran);  //jobTrip.ChangeSubContractorFromPlanning(con, tran, true);
                    }
                }
                #region 20140804 - gerry removed and replaced
                /*
                //20140521 end
                       //20120324 - Renamed From SetContainerNo
                //UpdateHaulierJbTripFromPlanning(frmName,userID, con, tran, planHaulierTrips);

                // Create 2 new Lists to pass in SetHaulierJobTripStatusObject
                SortableList<HaulierJobTrip> tempNewJobTripsForTripStates = new SortableList<HaulierJobTrip>();
                SortableList<HaulierJobTrip> tempOldJobTripsForTripStates = new SortableList<HaulierJobTrip>();
                // Pass in 2 Lists by reference to the method below, which will populate the 2 Lists
                SetHaulierJobTripStatusObject(planHaulierTrips, tempNewJobTripsForTripStates, tempOldJobTripsForTripStates);
                // Write the changes for HaulierJobTrip
                SetHaulierJobTripStatusDatabase(con, tran, tempNewJobTripsForTripStates, tempOldJobTripsForTripStates);
                */
                #endregion
                SetPlanHaulierJobTripsPropertiesIfChanged(planHaulierTrips, userID, frmName, autoSetNextLegToReady, con, tran);

                #region 20140415 - gerry removed
                /*
                SortableList<PlanHaulierTrip> temp = new SortableList<PlanHaulierTrip>();
                for (int i = 0; i < planHaulierTrips.Count; i++)
                {
                    if (planHaulierTrips[i].HaulierSubTrips.Count != 0)
                    {
                        temp.Add(planHaulierTrips[i]);
                    }
                }  
                planHaulierTrips = temp;
                */
                #endregion

                for (int i = 0; i < planHaulierTrips.Count; i++)
                {
                    if (planHaulierTrips[i].HaulierSubTrips.Count != 0)
                        planHaulierTrips[i].AddPlanHaulierTripDatabase(con, tran, tptDept);
                }                                            
                // After writing to database, for each PlanHaulierTrip - copy planHaulierSubTrips 
                // to the oldPlanHaulierSubTrips. This will invoke the clone methods of PlanHaulierSubTrip 
                // and HaulierJobTrip. Note that only properties necessary for SetHaulierJobTripStatusObject are copied. 
                // The other properties are not copied for performance reasons

                foreach (PlanHaulierTrip planHaulierTrip in planHaulierTrips)
                {
                    planHaulierTrip.CopyPlanHaulierSubTripsToOldPlanHaulierSubTrips();
                }

                //SAVE TRAILER LOCATION

                if (trailerlocations.Count > 0)
                {
                    TrailerLocation.AddTrailerLocations(trailerlocations, con, tran, plandate, tptDept, trailerlocationhashtbl);
                }
                retValue = true;
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
            return retValue;
        }

        #region Gerry Removed and replaced by SetPlanHaulierJobTripsPropertiesIfChanged method
        /*
        //Renamed From SetContainerNo
        //20130417 added parameter fr logging
        public static void UpdateHaulierJbTripFromPlanning(string frmName, string userID, SqlConnection con, SqlTransaction tran,
            SortableList<PlanHaulierTrip> planHaulierTrips)
        {
            try
            {
                SortableList<HaulierJobTrip> oldjobtrips = new SortableList<HaulierJobTrip>();
                SortableList<HaulierJobTrip> newjobtrips = new SortableList<HaulierJobTrip>();
                bool appSettingValue = ApplicationOption.GetApplicationOption(ApplicationOption.HAULAGE_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_UPDATE_STATUS_NEXT_LEG).setting_Value;

                //pull all new and old jobtrips
                for (int i = 0; i < planHaulierTrips.Count; i++)
                {
                    for (int j = 0; j < planHaulierTrips[i].haulierSubTrips.Count; j++)
                    {
                        for (int k = 0; k < planHaulierTrips[i].haulierSubTrips[j].JobTrips.Count; k++)
                        {
                            newjobtrips.Add(planHaulierTrips[i].haulierSubTrips[j].JobTrips[k]);
                        }
                    }
                    #region 20130418 - Gerry removed- no use at all
                    
                for (int j = 0; j < planHaulierTrips[i].oldhaulierSubTrips.Count; j++)
                {
                    for (int k = 0; k < planHaulierTrips[i].oldhaulierSubTrips[j].JobTrips.Count; k++)
                    {
                        oldjobtrips.Add(planHaulierTrips[i].oldhaulierSubTrips[j].JobTrips[k]);
                    }
                } 
                 
                    #endregion
                }

                SortableList<HaulierJobTrip> jobtripsFL = new SortableList<HaulierJobTrip>();  //create a collection of first leg jobtrips
                for (int i = 0; i < newjobtrips.Count; i++)
                {
                    //20120813 check And Update haulier job if permit no. changed
                    if (newjobtrips[i].permitNo != newjobtrips[i].oldPermitNo)
                    {
                        HaulierJob.UpdatehaulierJobFromPlanning(newjobtrips[i].permitNo, newjobtrips[i].JobID, con, tran);
                    }
                    //end 20120813  

                    //20130415 - gerry replaced
                    //separate method was created in haulierJobTrip class 
                    //to update jobtrip from planning, it will not automatic update the 2nd leg    
                    //Update all job trips  
                    newjobtrips[i].UpdateHaulierJobTripFromPlanning(frmName, userID, con, tran);
                    newjobtrips[i].UpdateVersion = newjobtrips[i].GetHaulierJobTripUpdateVersion(con, tran);
                    //Get all first legs
                    if (newjobtrips[i].LegType == LegType.FirstOfTwoLeg)
                    {   //add to collection of first legs
                        jobtripsFL.Add(newjobtrips[i]);
                    }
                 }
                //for (int j = 0; j < jobTripsLastLeg.Count; j++)
                //{
                //    if (newjobtrips[i].ContainerNo == jobTripsLastLeg[0].ContainerNo)
                //    {
                //        jobTripsLastLeg[0] = newjobtrips[i];
                //    }
                //    else
                //        jobTripsLastLeg.Add(newjobtrips[i]);
                //}
                //loop to the collection of first legs, 
                //if has partner leg in the collect update separately 
                //if no partner leg in the collection update ContainerNo and SealNo where ever it is.
                if (jobtripsFL.Count > 0)
                {
           
                    UpdateContainerNoForNextLegs(jobtripsFL, newjobtrips, frmName, con, tran);
                }
                if (appSettingValue)
                { //TODO update next leg
                    SetStatusAndStartStopOfNextLeg(newjobtrips, con, tran);
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message); }
        }
        private static void SetStatusAndStartStopOfNextLeg(SortableList<HaulierJobTrip> newJobTrips, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                foreach (HaulierJobTrip lastLegTrip in newJobTrips)
                {
                    lastLegTrip.UpdateNextLegStartStopAndStatus(con, tran);
                }
            }        
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message); }  
        }
        //20140731- gerry refactor to this method
        //since only first legs can be change containerNo, then need parameter
        private static void UpdateContainerNoForNextLegs(SortableList<HaulierJobTrip> firstLegTrips, SortableList<HaulierJobTrip> allTripsInPlanDate, string frmName, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                foreach (HaulierJobTrip trip in firstLegTrips)
                {
                    bool hasPartnerLeg = false;
                    bool hasRemainingLeg = false;
                    //get the remaining legs inside the plan date
                    List<HaulierJobTrip> remainingLegsInsidePlanDate = allTripsInPlanDate.Where(jt => jt.JobID == trip.JobID).ToList();
                    foreach (HaulierJobTrip nextLeg in remainingLegsInsidePlanDate)
                    {
                        if (trip.Sequence == nextLeg.PartnerLeg && nextLeg.LegType == LegType.SecondOfTwoLeg)
                            hasPartnerLeg = true;
                        if (nextLeg.isMulti_leg == true && nextLeg.LegType == LegType.OneLeg && nextLeg.ContainerNo == trip.ContainerNo)
                            hasRemainingLeg = true;
                    }
                    //update partner leg and remaining leg are separate because 
                    //partner leg has the partnerLegSequenceNo while the remaining legs don't have
                    //and there also for the case of 2 legs without containerNo  or containerNo was changed
                    if (!hasPartnerLeg)
                    {
                        trip.UpdateContainerAndSealNoForSecondLeg(frmName, con, tran);
                        trip.UpdateVersion = trip.GetHaulierJobTripUpdateVersion(con, tran);
                    }
                    if (!hasRemainingLeg)
                    {
                        trip.UpdateContainerSealNoForRemainingLeg(frmName, trip.oldContainerNo, trip.oldSealNo, con, tran);
                        trip.UpdateVersion = trip.GetHaulierJobTripUpdateVersion(con, tran);
                    }
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message); }
        }

        public static bool SetHaulierJobTripStatusObject(SortableList<PlanHaulierTrip> planHaulierTrips,
            SortableList<HaulierJobTrip> newjobtrips, SortableList<HaulierJobTrip> oldjobtrips)
        {
            //20130418 - gerry removed,
            //bool firstcase = false;
            //bool secondcase = false;

            //HaulierJobTrip deletenewjobtrip = new HaulierJobTrip();
            //HaulierJobTrip deleteoldjobtrip = new HaulierJobTrip();          


            //we need this temp object for looping purpose. not to affect the return objects
            SortableList<HaulierJobTrip> tempoldjobtrips = new SortableList<HaulierJobTrip>();
            SortableList<HaulierJobTrip> tempnewjobtrips = new SortableList<HaulierJobTrip>();
            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                for (int j = 0; j < planHaulierTrips[i].haulierSubTrips.Count; j++)
                {
                    for (int k = 0; k < planHaulierTrips[i].haulierSubTrips[j].JobTrips.Count; k++)
                    {
                        newjobtrips.Add(planHaulierTrips[i].haulierSubTrips[j].JobTrips[k]);
                        tempnewjobtrips.Add(planHaulierTrips[i].haulierSubTrips[j].JobTrips[k]);
                    }
                }
                for (int j = 0; j < planHaulierTrips[i].oldhaulierSubTrips.Count; j++)
                {
                    for (int k = 0; k < planHaulierTrips[i].oldhaulierSubTrips[j].JobTrips.Count; k++)
                    {
                        oldjobtrips.Add(planHaulierTrips[i].oldhaulierSubTrips[j].JobTrips[k]);
                        tempoldjobtrips.Add(planHaulierTrips[i].oldhaulierSubTrips[j].JobTrips[k]);
                    }
                }
            }
            for (int i = 0; i < tempnewjobtrips.Count; i++)
            {
                for (int j = 0; j < tempoldjobtrips.Count; j++)
                {
                    //search if the 1st and 2nd trip is same
                    //if the same, then remove from collection of new and old job trips
                    //we only add job trip state if - new newly assigned job trips OR status changed
                    if ((tempnewjobtrips[i].JobID == tempoldjobtrips[j].JobID) &&
                        (tempnewjobtrips[i].Sequence == tempoldjobtrips[j].Sequence))
                    {
                        //retain all job trips which are not changed
                        if (tempnewjobtrips[i].TripStatus == tempoldjobtrips[j].TripStatus)
                        {
                            newjobtrips.Remove(tempnewjobtrips[i]);
                            oldjobtrips.Remove(tempoldjobtrips[j]);
                        }
                        else //case if status was changed, need to retain the previous job trip state
                        {
                            oldjobtrips.Remove(tempoldjobtrips[j]);
                        }
                    }
                }
            }
            return true;
        }

        public static bool SetHaulierJobTripStatusDatabase(SqlConnection con, SqlTransaction tran,
            SortableList<HaulierJobTrip> newjobtrips, SortableList<HaulierJobTrip> oldjobtrips)
        {
            try
            {
                //for each item in remaining element in new job trips
                for (int i = 0; i < newjobtrips.Count; i++)
                {
                    JobTripState jobTripState = new JobTripState(1, newjobtrips[i].TripStatus, newjobtrips[i].StartDate, "", true);
                    newjobtrips[i].AddJobTripState(jobTripState, con, tran);
                }
                //for each item in remaining element in old job trips
                for (int i = 0; i < oldjobtrips.Count; i++)
                {
                    oldjobtrips[i].TripStatus = JobTripStatus.Assigned;
                    JobTripState jobTripState = new JobTripState(1, JobTripStatus.Assigned, oldjobtrips[i].StartDate, "", true);
                    oldjobtrips[i].DeleteJobTripState(jobTripState, con, tran);
                }
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
          
        */
        #endregion

        private static void SetPlanHaulierJobTripsPropertiesIfChanged(SortableList<PlanHaulierTrip> planHaulierTrips, string userID, string frmName, bool autoSetNextLegToReady, SqlConnection con, SqlTransaction tran)
        {
            //this collections are those will be updated
            SortableList<HaulierJobTrip> oldjobtrips = new SortableList<HaulierJobTrip>();
            SortableList<HaulierJobTrip> newjobtrips = new SortableList<HaulierJobTrip>();
            //temporary collection for comparison, because we cant remove items during looping
            SortableList<HaulierJobTrip> tempoldjobtrips = new SortableList<HaulierJobTrip>();
            SortableList<HaulierJobTrip> tempnewjobtrips = new SortableList<HaulierJobTrip>();
            //get the collections of new and old job trips
            GetNewAndOldHaulierJobTrips(planHaulierTrips, newjobtrips, oldjobtrips, tempnewjobtrips, tempoldjobtrips);
            //remove from collection all those are not changed
            RemoveUnChangeJobTrip(newjobtrips, oldjobtrips, tempnewjobtrips, tempoldjobtrips);
            //update job trip, include update of HaulierJob permitNo
            foreach (HaulierJobTrip trip in newjobtrips)
            {
                //start update haulier job permit no
                HaulierJob haulierJob = HaulierJob.GetHaulierJob(trip.JobNo, con, tran);
                //Get original job trip
                HaulierJobTrip origJobTrip = HaulierJobTrip.GetHaulierJobTrip(trip.JobID, trip.Sequence, con, tran);
                //20140920_1230 end
                if (trip.permitNo != trip.oldPermitNo)
                {
                    haulierJob.permitNo1 = trip.permitNo;
                    haulierJob.UpdatehaulierJobFromPlanning(con, tran);
                }
                //check duplicate container number
                haulierJob.JobTrips.Add(trip);
                if (!trip.ContainerNo.Equals(string.Empty))
                    haulierJob.CheckDoubleContainerSealNos(trip, con, tran);
                //Assuming that next leg trip is not yet added in the planning date
                //most cases, the first leg must completed first before we can add the next leg 
                //and we cannot set to complete if no containerNo
                //if (trip.LegType == LegType.FirstOfTwoLeg) // && trip.ContainerNo != trip.oldContainerNo)

                //20160513 - gerry allow to update container and seal number in any legs
                //if (trip.LegGroupMember == 1 && trip.isMulti_leg)
                //{
                #region Update container/seal number of the next leg(s)
                //if (trip.oldContainerNo.Equals(string.Empty))   //case for containerNo was not set in booking
                //trip.UpdateContainerAndSealNoForSecondLeg("PL", userID, frmName, con, tran);
                //else  //case for container numbers are set in booking and have more than 2 legs
                trip.UpdateContainerSealNoForRemainingLeg("PL", userID, frmName, con, tran);//update the other legs
                #endregion
                //}
                //update version get from the called method
                //update job trip(containerNo, sealNo, yard, portRemark, startStop, endStop)
                trip.UpdateHaulierJobTripFromPlanning(frmName, userID, con, tran);
                //update next leg status and start stop

                trip.UpdateNextLegStartStopAndStatus(autoSetNextLegToReady, "PL", userID, frmName, con, tran);
                //20161010
                AuditLog auditLog = new AuditLog(trip.JobNo, "HL", "PL", trip.JobID, userID, DateTime.Now, frmName, trip.Sequence, FormMode.Edit.ToString());
                auditLog.WriteAuditLog("SeqNo", trip.Sequence.ToString(), "", con, tran);
                auditLog.WriteAuditLog(trip, origJobTrip, con, tran);
            }
            //update job trip states
            UpdateJobTripsStates(newjobtrips, oldjobtrips, planHaulierTrips[0].ScheduleDate, userID, frmName, con, tran);
        }
        //20140808 - gerry added get the new and old job trips
        //temporary collection for comparison, because we cant remove items during looping
        private static void GetNewAndOldHaulierJobTrips(SortableList<PlanHaulierTrip> planHaulierTrips, SortableList<HaulierJobTrip> newjobtrips, SortableList<HaulierJobTrip> oldjobtrips
                                 , SortableList<HaulierJobTrip> tempnewjobtrips, SortableList<HaulierJobTrip> tempoldjobtrips)
         {
             foreach (PlanHaulierTrip planTrip in planHaulierTrips)
             {   //new job trips here
                 foreach (PlanHaulierSubTrip newPlanSubTrip in planTrip.haulierSubTrips)
                 {
                     foreach (HaulierJobTrip jobTrip in newPlanSubTrip.JobTrips)
                     {
                         newjobtrips.Add(jobTrip);
                         tempnewjobtrips.Add(jobTrip);//temporary collection
                     }
                 }
                 //old job trips here
                 foreach (PlanHaulierSubTrip oldPlanSubTrip in planTrip.oldhaulierSubTrips)
                 {
                     foreach (HaulierJobTrip jobTrip in oldPlanSubTrip.JobTrips)
                     {
                         oldjobtrips.Add(jobTrip);
                         tempoldjobtrips.Add(jobTrip);//temporary collection
                     }
                 }
             }
         }
        //20140808 - gerry added remove from collection all those are not changed
        //temporary collection for comparison, because we cant remove items during looping
        private static void RemoveUnChangeJobTrip(SortableList<HaulierJobTrip> newjobtrips, SortableList<HaulierJobTrip> oldjobtrips
                                 , SortableList<HaulierJobTrip> tempnewjobtrips, SortableList<HaulierJobTrip> tempoldjobtrips)
         {
             foreach (HaulierJobTrip newJobTrip in tempnewjobtrips)
             {
                 foreach (HaulierJobTrip oldJobTrip in tempoldjobtrips)
                 {   //if the same, then remove from collection of new and old job trips
                     //we only add job trip state if - new newly assigned job trips OR status changed
                     if (newJobTrip.JobID == oldJobTrip.JobID && newJobTrip.Sequence == oldJobTrip.Sequence)
                     {
                         switch (newJobTrip.TripStatus == oldJobTrip.TripStatus)
                         {
                             case true:
                                 if (newJobTrip.ContainerNo == oldJobTrip.ContainerNo
                                     && newJobTrip.SealNo == oldJobTrip.SealNo
                                     && newJobTrip.permitNo == oldJobTrip.permitNo
                                     && newJobTrip.Yard == oldJobTrip.Yard
                                     && newJobTrip.PortRemark == oldJobTrip.PortRemark
                                     && newJobTrip.StartStop.Code == oldJobTrip.StartStop.Code
                                     && newJobTrip.EndStop.Code == oldJobTrip.EndStop.Code)
                                 {
                                     newjobtrips.Remove(newJobTrip);
                                     oldjobtrips.Remove(oldJobTrip);
                                 }
                                 else //if any of those properties were changed then remove from oldtrip collection
                                     oldjobtrips.Remove(oldJobTrip);
                                 break;
                             default:
                                 oldjobtrips.Remove(oldJobTrip);
                                 break;
                         }
                     }
                 }
             }
         }       
        //20140808 - gerry added update job trip states
        private static void UpdateJobTripsStates(SortableList<HaulierJobTrip> newjobtrips, SortableList<HaulierJobTrip> oldjobtrips, DateTime scheduleDate, string userID, string frmName, SqlConnection con, SqlTransaction tran)
        {
            //for each item in remaining element in new job trips
            foreach (HaulierJobTrip newTrip in newjobtrips)
            {   //update new jobtrips trip state
                
                JobTripStatus tripLastStatus = newTrip.JobTripStates[newTrip.JobTripStates.Count - 1].Status;

                //20140818 - gerry added for case Ready to complete
                if ((tripLastStatus == JobTripStatus.Ready && (newTrip.TripStatus == JobTripStatus.Assigned || newTrip.TripStatus == JobTripStatus.Completed))
                    || (tripLastStatus == JobTripStatus.Assigned && newTrip.TripStatus == JobTripStatus.Completed)
                    || (tripLastStatus == JobTripStatus.Completed && newTrip.TripStatus == JobTripStatus.Assigned))
                {
                //if(newTrip.TripStatus!= tripLastStatus)
                //{
                    JobTripState jobTripState = new JobTripState(1, newTrip.TripStatus, scheduleDate, "Updated from Planning by " + userID, true);
                    newTrip.AddJobTripState(jobTripState,"PL", userID, frmName, con, tran);
                } 
            }
            //for each item in remaining element in old job trips
            foreach (HaulierJobTrip oldTrip in oldjobtrips)
            {
                bool isPass = true;
                if (!oldTrip.isMovementJob)
                {
                    HaulierJob haulierJob = HaulierJob.GetHaulierJob(oldTrip.JobNo, con, tran);
                    //20160531 Jana, check if Job trip = 1;
                    haulierJob.JobTrips = HaulierJobTrip.GetAllHaulierJobTrips(haulierJob, con, tran);
                    if (haulierJob.JobTrips.Count == 1)
                    {
                        //remove Job.JobTrips, to pass validation of Job Haulier
                        haulierJob.JobTrips = new SortableList<HaulierJobTrip>();
                        //Check if Job ID have more than 1 data in TPT_PLAN_SUBTRIP_JOB_TBL
                        int DataCount = PlanTransportDAL.GetPlanSubTripJobByJobID(haulierJob.JobID, con, tran);
                        if (DataCount == 0)
                        {
                            isPass = false;
                            //haulierJob.JobTrips = HaulierJobTrip.GetAllHaulierJobTrips(haulierJob); // no need JOBID is enough to delete
                            //delete from db those non movement job
                            haulierJob.DeleteHaulierJobFromPlanning("TPT_HAU_E_HAULIER_PLANNING", userID, con, tran);
                            //20151109 - gerry added new audit log
                            AuditLog auditLog = new AuditLog(oldTrip.JobNo, "TR", "PL", oldTrip.JobID, userID, DateTime.Now, frmName, oldTrip.Sequence, FormMode.Delete.ToString());
                            auditLog.WriteAuditLog(null, null, con, tran);
                        }
                    }
                }
                if (isPass)
                {
                    //case when job trip was deleted from planning; 
                    //instead of delete the previous status, we add the new status which is ready to job trip states
                    JobTripState jobTripState = new JobTripState(1, JobTripStatus.Ready, scheduleDate, "Updated from Planning by " + userID, true);
                    oldTrip.AddJobTripState(jobTripState, "PL", userID, frmName, con, tran);
                }
            }  
  
        }

        public void CopyPlanHaulierSubTripsToOldPlanHaulierSubTrips()
        {
            oldhaulierSubTrips = null;
            oldhaulierSubTrips = new PlanHaulierSubTrips();
            foreach (PlanHaulierSubTrip planHaulierSubTrip in haulierSubTrips)
            {
                PlanHaulierSubTrip oldPlanHaulierSubTrip = (PlanHaulierSubTrip)planHaulierSubTrip.Clone();
                oldhaulierSubTrips.Add(oldPlanHaulierSubTrip);
            }
        }

        public static bool IsPlanDateDuplicate(DateTime date, string tptDept)
        {
            return PlanTransportDAL.IsPlanDateDuplicate(date, tptDept);
        }

        public static bool AddTrailerLocationToCollection(SortableList<TrailerLocation> trailerLocations, string trailerNo,
            PlanHaulierSubTrip planSubTrip, string planTripNo, TrailerStatus status)
        {
            if (!TrailerLocation.IsTrailerTimeOverlapping(trailerLocations, trailerNo, planSubTrip.Start, planSubTrip.End,
                planSubTrip.Start, planSubTrip.End, true))
            {
                Vehicle tempvechicle = Vehicle.GetVehicle(trailerNo);
                planSubTrip.Trailer = tempvechicle;
                TrailerLocation t = new TrailerLocation();
                t.TrailerNo = trailerNo;
                t.ScheduleDate = planSubTrip.Start;
                t.ChangeDate = DateTime.Today;
                t.StartTime = planSubTrip.Start;
                t.EndTime = planSubTrip.End;
                t.StartStop = planSubTrip.StartStop.Code;
                t.StopCode = planSubTrip.EndStop.Code;
                t.PlanTripNo = planTripNo;
                t.DriverNo = planSubTrip.DriverNumber;
                t.TrailerStatus = status;
                t.Remarks = "";
                trailerLocations.Add(t);
                return true;
            }
            else
            {
                return false;
            }
        }


        public int FindNextIndexPlanHaulierSubTrip(DateTime endtime)
        {
            int nextsubtrip = 99;
            DateTime tempstarttime = endtime;
            int find = 0;
            for (int i = 0; i < haulierSubTrips.Count; i++)
            {
                if (haulierSubTrips[i].End > endtime)
                {
                    if (find == 0)
                    {
                        tempstarttime = haulierSubTrips[i].End;
                        nextsubtrip = i;
                        find = 1;
                    }
                    else
                    {
                        if (haulierSubTrips[i].End < tempstarttime)
                        {
                            tempstarttime = haulierSubTrips[i].End;
                            nextsubtrip = i;
                        }
                    }
                }
            }
            return nextsubtrip;
        }
   
        public Vehicle GetTrailerFromFirstLegJobTrip(int jobid, int seqno)
        {
            Vehicle temp = null;
            for (int i = 0; i < this.HaulierSubTrips.Count; i++)
            {
                if ((this.HaulierSubTrips[i].JobTrips[0].JobID == jobid)
                    & (this.HaulierSubTrips[i].JobTrips[0].Sequence == seqno)
                    & (this.HaulierSubTrips[i].JobTrips[0].LegType == Leg_Type.FirstOfTwoLeg))
                {
                    temp = this.HaulierSubTrips[i].Trailer;
                    break;
                }
            }
            return temp;
        }
        //20141117 - Gerry Added 
        //to be use as default for 3rd, 4th leg and so on..
        public static Vehicle GetTrailerFromPreviousLeg(SortableList<PlanHaulierTrip> planHaulierTrips, PlanHaulierSubTrip planHaulierSubTrip)
        {
            Vehicle previousLegTrailer = new Vehicle();
            try
            {
                if (planHaulierSubTrip.JobTrips[0].ContainerNo != "")
                {   //loop through all planTrip in that day 
                    foreach (PlanHaulierTrip tempPlanHaulierTrip in planHaulierTrips)
                    {
                        if (tempPlanHaulierTrip.Driver.Code == planHaulierSubTrip.DriverNumber)
                        { //loop through all planSubTrip in that tempPlanHaulierTrip 
                            foreach (PlanHaulierSubTrip tempPlanSubTrip in tempPlanHaulierTrip.HaulierSubTrips)
                            {
                                if (tempPlanSubTrip.Start == planHaulierSubTrip.Start && tempPlanHaulierTrip.Driver.Code == planHaulierSubTrip.DriverNumber)
                                {
                                    //do nothing
                                }
                                else
                                {
                                    //get jobtrips based on legGroup and jobNumber
                                    //or if transferredIN jobtrip jobNumber based on the partner jobNo
                                    List<HaulierJobTrip> tempList = tempPlanSubTrip.JobTrips.Where(jt => (jt.JobNo == planHaulierSubTrip.JobTrips[0].JobNo && jt.ContainerNo == planHaulierSubTrip.JobTrips[0].ContainerNo)
                                                                                                || (jt.JobNo == planHaulierSubTrip.JobTrips[0].PartnerJobNo && jt.ContainerNo == planHaulierSubTrip.JobTrips[0].ContainerNo)).ToList();
                                    if (tempList.Count > 0)
                                    {
                                        return tempPlanSubTrip.Trailer;
                                    }
                                }
                            }
                        }
                    }
                    if (previousLegTrailer.Number == string.Empty)
                    {   
                        //get previous leg trailer from database
                        //first Get HaulierJob
                        previousLegTrailer = Vehicle.GetTrailerFromPreviousLeg(planHaulierSubTrip.JobTrips[0].JobID, planHaulierSubTrip.JobTrips[0].ContainerNo);
                        
                        //if previous trailer is not in the current job; it might a transferIn job, so get the partnerJob ID
                        if (previousLegTrailer.Number == string.Empty)
                        { 
                            //get haulier job based on the partner job number
                            HaulierJob partnerJob = HaulierJob.GetHaulierJob(planHaulierSubTrip.JobTrips[0].PartnerJobNo);
                            previousLegTrailer = Vehicle.GetTrailerFromPreviousLeg(partnerJob.JobID, planHaulierSubTrip.JobTrips[0].ContainerNo);
                        }       
                        //Note: if trailer being pulled means your data don't have either legGroup or legGroupMember
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return previousLegTrailer;
        }
        //end 20120926
        public static List<string> GetDriverNamesInPlan(string code)
        {
            return PlanTransportDAL.GetDriverNamesInPlan(code);
        }

        //20121015 - Gerry Added this method to validate 2nd leg not assign before First leg
        private static bool ValidateTimeBy2ndLegType(SortableList<PlanHaulierTrip> planHaulierTrips, HaulierJobTrip newJobTrip, DateTime newStartTime)
        {
            //loop through all PlanHaulierTrip in that planHaulierTrips
            foreach (PlanHaulierTrip tempPlanTrip in planHaulierTrips)
            {
                //loop through all PlanHaulierSubTrip in that tempPlanTrip
                foreach (PlanHaulierSubTrip tempPlanHaulierSubTrip in tempPlanTrip.haulierSubTrips)
                {
                    //loop through all jobHaulierTrip in that tempPlanHaulierSubTrip
                    foreach (HaulierJobTrip tempJobTrip in tempPlanHaulierSubTrip.JobTrips)
                    {
                        //if partner leg found
                        if (tempJobTrip.LegType == Leg_Type.FirstOfTwoLeg
                            & tempJobTrip.JobID == newJobTrip.JobID
                            & tempJobTrip.LegGroup == newJobTrip.LegGroup)
                        {
                            //compare planHaulierSubTrip time             
                            if (tempPlanHaulierSubTrip.End > newStartTime)
                            {
                                throw new FMException(TptResourceBLL.ErrSecondLegEarlierThanFirstLeg + "\n" +TptResourceBLL.WarnFirstLegStart+ tempPlanHaulierSubTrip.Start.ToShortTimeString() + " - " + tempPlanHaulierSubTrip.End.ToShortTimeString());
                            }
                        }  
                    }
                }
            }
            return true;
        }
        //20121015 - Gerry Added this method to validate first leg not assign after 2nd leg and remaining legs
        private static bool ValidateTimeByFirstLegType(SortableList<PlanHaulierTrip> planHaulierTrips, HaulierJobTrip newJobTrip, DateTime newEndTime)
        {
            //loop through all PlanHaulierTrip in that planHaulierTrips
            foreach (PlanHaulierTrip tempPlanTrip in planHaulierTrips)
            {
                //loop through all PlanHaulierSubTrip in that tempPlanTrip
                foreach (PlanHaulierSubTrip tempPlanHaulierSubTrip in tempPlanTrip.haulierSubTrips)
                {
                    //loop through all jobHaulierTrip in that tempPlanHaulierSubTrip
                    foreach (HaulierJobTrip tempJobTrip in tempPlanHaulierSubTrip.JobTrips)
                    {
                        //if partner leg found
                        if (tempJobTrip.LegType == Leg_Type.SecondOfTwoLeg
                            & tempJobTrip.JobID == newJobTrip.JobID
                            & tempJobTrip.LegGroup == newJobTrip.LegGroup)
                        {
                            //compare planHaulierSubTrip time             
                            if (tempPlanHaulierSubTrip.Start < newEndTime)
                            {
                                throw new FMException(TptResourceBLL.ErrSecondLegEarlierThanFirstLeg + "\n"+TptResourceBLL.Warn2ndLegStart 
                                                        + tempPlanHaulierSubTrip.Start.ToShortTimeString() + " - " + tempPlanHaulierSubTrip.End.ToShortTimeString());
                            }
                        }
                    }
                }
            }
            return true;
        }
        //20121015 - Gerry Added this method to validate remaining legs must assign after 2nd leg or other previous legs
        private static bool ValidateTimeByRemainingLegs(SortableList<PlanHaulierTrip> planHaulierTrips, HaulierJobTrip newJobTrip, DateTime newStartTime, out Vehicle outTrailer)
        {
            outTrailer = null;
            //loop through all PlanHaulierTrip in that planHaulierTrips
            foreach (PlanHaulierTrip tempPlanTrip in planHaulierTrips)
            {
                //loop through all PlanHaulierSubTrip in that tempPlanTrip
                foreach (PlanHaulierSubTrip tempPlanHaulierSubTrip in tempPlanTrip.haulierSubTrips)
                {
                    //loop through all jobHaulierTrip in that tempPlanHaulierSubTrip
                    foreach (HaulierJobTrip tempJobTrip in tempPlanHaulierSubTrip.JobTrips)
                    {
                        //for case 4th leg and above, will check the two conditions
                        //for case 3rd leg, will check only the 2nd condition

                        //first, must check if completed (One Leg/multi-leg) found with the same containerNo
                        if (tempJobTrip.LegType == Leg_Type.OneLeg & tempJobTrip.isMulti_leg
                            & tempJobTrip.TripStatus == JobTripStatus.Completed
                            & tempJobTrip.JobID == newJobTrip.JobID
                            & tempJobTrip.ContainerNo == newJobTrip.ContainerNo)
                        {
                            //compare planHaulierSubTrip time             
                            if (tempPlanHaulierSubTrip.End > newStartTime)
                                throw new FMException(TptResourceBLL.ErrPreviousLegMustEarlier + "\n" + TptResourceBLL.WarnPreviousLegStart + tempPlanHaulierSubTrip.Start.ToShortTimeString() + " To " + tempPlanHaulierSubTrip.End.ToShortTimeString());
                            else
                            {
                                outTrailer = tempPlanHaulierSubTrip.Trailer;                                 
                            }
                        } 
                        //2nd, check if completed 2nd leg found
                        else if ( tempJobTrip.LegType == Leg_Type.SecondOfTwoLeg 
                            & tempJobTrip.TripStatus == JobTripStatus.Completed
                            & tempJobTrip.JobID == newJobTrip.JobID
                            & tempJobTrip.ContainerNo == newJobTrip.ContainerNo)
                        {
                            //compare planHaulierSubTrip time             
                            if (tempPlanHaulierSubTrip.End > newStartTime)
                                throw new FMException(TptResourceBLL.ErrPreviousLegMustEarlier + "\n"+TptResourceBLL.WarnPreviousLegStart + tempPlanHaulierSubTrip.Start.ToShortTimeString() + " To " + tempPlanHaulierSubTrip.End.ToShortTimeString());
                            else
                            {
                                outTrailer = tempPlanHaulierSubTrip.Trailer;
                            }
                        }
                    }
                }
            }
            if (outTrailer == null)
            { 
                //TODO look from database;
                Vehicle vh = new Vehicle();
                vh = Vehicle.GetTrailerFromPreviousLeg(newJobTrip.JobID, newJobTrip.ContainerNo);
                outTrailer = vh;                
            }

            return true;
        }
        //20121015 - Gerry Added this method to validate first leg not assign after 2nd leg and remaining legs
        //parameter DateTime newCheckStartTime used to check if leg type is 2ndleg or oneleg/multi_leg
        //parameter DateTime newCheckEndTime used to check if leg type is 1st leg
        public static bool CheckStartTimAndGetTrailerForOneLegAndMultiLeg(SortableList<PlanHaulierTrip> planHaulierTrips, SortableList<HaulierJobTrip> newJobTrips, DateTime newCheckStartTime, DateTime newCheckEndTime, out Vehicle outTrailer)
        {
            outTrailer =new Vehicle();
            try
            {
                //loop each jobtrips
                foreach (HaulierJobTrip newJobTrip in newJobTrips)
                {
                    //TODO validate time for 2nd leg
                    if (newJobTrip.LegType == Leg_Type.SecondOfTwoLeg)
                        ValidateTimeBy2ndLegType(planHaulierTrips, newJobTrip, newCheckStartTime);
                    //TODO validate time for first leg
                    else if (newJobTrip.LegType == Leg_Type.FirstOfTwoLeg)
                        ValidateTimeByFirstLegType(planHaulierTrips, newJobTrip, newCheckEndTime);
                    //TODO validate time and get trailer for remaining legs
                    else if (newJobTrip.LegType == Leg_Type.OneLeg && newJobTrip.isMulti_leg)
                        ValidateTimeByRemainingLegs(planHaulierTrips, newJobTrip, newCheckStartTime, out outTrailer);
    
                    
                    return false;                  
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            return false;
        }

        public static bool IsValidTrailerForPlanSubTrip(SortableList<PlanHaulierTrip> planHaulierTrips, SortableList<TrailerLocation> trailerLocations, PlanHaulierSubTrip currentPlanSubTrip, string trailerNo, out string msg)
        {
            bool retValue = true;
            try
            {
                //201319 - Gerry added to get the latest trailer location from the memory collection
                TrailerLocation latestTrailerLoc = TrailerLocation.GetLatestTrailerLocationFromCollection(trailerLocations, trailerNo);
                           
                msg = TptResourceBLL.ErrTrailerIsAssignedToOtherJob; // "Trailer ({0}) is already assigned to {1} container('{2}') from {3} - {4}, {5}, {6}.\n You can only use this trailer after completion of the assigned job and set status to 'Trailer Park' ";
                //First loop through whole plan date
                foreach (PlanHaulierTrip tempPlanTrip in planHaulierTrips)
                { 
                    bool exit1 = false;
                    //Second loop through each plansubtrip
                    foreach (PlanHaulierSubTrip tempPlanSubTrip in tempPlanTrip.HaulierSubTrips)
                    {
                        bool exit2 = false;
                        if (!tempPlanSubTrip.Equals(currentPlanSubTrip))
                        {
                            //20130919 - Gerry Change the condition to compare the latest trailer location 
                            //get trailerLocation object of the tempPlanSubTrip 
                            //to minimize the looping scope from all planHaulierTrip with the same trailerNo
                            TrailerLocation tempTrailerLoc = TrailerLocation.GetTrailerLocationFromCollection(trailerLocations, tempPlanSubTrip.Trailer.Number.ToString(), tempPlanSubTrip.Start);

                            //20130919 - Gerry Changed the condition to compare the latest trailer location and the trailer used by tmpPlanSubtrip
                            //if (tempPlanSubTrip.Trailer.Number.ToString() == trailerNo
                            //    && latestTrailerLoc.TrailerStatus != TrailerStatus.TrailerPark)                              
                            if(tempTrailerLoc == latestTrailerLoc
                                && latestTrailerLoc.TrailerStatus != TrailerStatus.TrailerPark)
                            {
                                bool sameContainer = false;
                                //Third loop through each jobtrip
                                foreach (HaulierJobTrip tempJobTrip in tempPlanSubTrip.JobTrips)
                                {
                                    string driver = tempPlanSubTrip.DriverNumber.Equals(latestTrailerLoc.DriverNo) ? string.Empty : ("Under driver : " + tempPlanSubTrip.DriverNumber);
                                    string customer = tempJobTrip.CustomerName;
                                    string startTime = latestTrailerLoc.StartTime.ToShortTimeString();
                                    string endTime = latestTrailerLoc.EndTime.ToShortTimeString();
                                    string planDate = tempPlanSubTrip.Start == latestTrailerLoc.StartTime ? string.Empty : tempPlanSubTrip.Start.ToShortDateString();
                                    msg = string.Format(msg, trailerNo, latestTrailerLoc.TrailerStatus.ToString(),customer, tempJobTrip.ContainerNo.ToString(), startTime, endTime, driver, planDate);
                                    bool exit3 = false;
                                    foreach (HaulierJobTrip currentJobTrip in currentPlanSubTrip.JobTrips)
                                    {   //check container
                                        if (tempJobTrip.ContainerNo.Equals(currentJobTrip.ContainerNo))
                                        {
                                            sameContainer = true;
                                            exit1 = true;
                                            exit2 = true;
                                            exit3 = true;
                                            retValue = true;
                                            break;
                                        }
                                    }
                                    if (exit3)//exit Third loop
                                        break;
                                }
                                if (!sameContainer)
                                    retValue = false;
                            }
                            if (exit2) //exit Second loop
                                break;
                        }
                    }
                    if (exit1)//exit First loop
                        break;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            return retValue;
        }

        //20140519 - gerry added to validate if jobTrip can be assign to sub contractor
        //note the parameter jobid and seqNo are from the selected job trip in planning
        public static bool ValidateTrailerGetJobTrip(HaulierJobTrip jobTrip)
        {
            try
            {
                if (jobTrip.isMulti_leg && jobTrip.LegType != Leg_Type.FirstOfTwoLeg)
                {
                    //get the previous leg job trip
                    HaulierJobTrip prevJobTrip = HaulierJobTrip.GetPreviousLegJobTrip(jobTrip.JobID, jobTrip.Sequence, jobTrip.ContainerNo);
                    //20141125 - gerry added to get from transfered out job
                    HaulierJob haulierJob = HaulierJob.GetHaulierJob(jobTrip.JobNo);
                    if(haulierJob.JobTransferType == JobTransferType.TransferIn && !jobTrip.PartnerJobNo.Equals(string.Empty))
                    {
                        HaulierJob partnerJob = HaulierJob.GetHaulierJob(jobTrip.PartnerJobNo);
                        prevJobTrip = HaulierJobTrip.GetPreviousLegJobTrip(partnerJob.JobID, jobTrip.ContainerNo, jobTrip.LegGroupMember);
                    }
                    //20141125 -end gerry
                    //get planHaulierSubTrip
                    string planTripNo = string.Empty;
                    PlanHaulierSubTrip prePlanSubTrip = PlanTransportDAL.GetPlanHaulierSubTripFromPreviousLeg(prevJobTrip.JobID, prevJobTrip.Sequence, out planTripNo);
                    if (prePlanSubTrip != null)
                    {   //get TrailerLocation of the previous leg
                        TrailerLocation prevTrailerLocation = TrailerLocation.GetPreviousLegTrailerLocationForPlanning(planTripNo, prePlanSubTrip.Trailer.Number, prePlanSubTrip.Start);
                        if (prevTrailerLocation != null)
                        {
                            if (prevTrailerLocation.TrailerStatus != TrailerStatus.TrailerPark)
                            {
                                throw new FMException(TptResourceBLL.ErrCantAssigToSubNotTrailerPark); // "Cannot assign to sub contractor because the trailer of the previous leg is not in trailer park. ");
                            }
                        }
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }  
            return true;
            
        }

        //20140724 - gerry added
        //20150714 - gerry added parameter incentivecode
        public static bool AddPlanHaulierSubTripInMemory(SortableList<PlanHaulierTrip> planTrips, Driver driver, SortableList<HaulierJobTrip> jobTrips,
            SortableList<TrailerLocation> trailerLocations, string incentiveCode, out PlanHaulierSubTrip newlyCreatedPlanSubTrip)
        {
            PlanHaulierTrip planTrip = new PlanHaulierTrip();
            newlyCreatedPlanSubTrip = new PlanHaulierSubTrip();
            try
            {

                Vehicle vehicle = new Vehicle();   //use as a trailer
                //set job trip status to Assigned
                //20140819 - gerry modified because of the collection of job trips for the case of existing job
                jobTrips[jobTrips.Count - 1].TripStatus = JobTripStatus.Assigned;
                //jobTrips[0].UpdateVersion = jobTrips[0].GetHaulierJobTripUpdateVersion();

                int sHr = Convert.ToInt32(jobTrips[jobTrips.Count - 1].StartTime.Substring(0, 2));
                int sMin = Convert.ToInt32(jobTrips[jobTrips.Count - 1].StartTime.Substring(2, 2));
                int eHr = Convert.ToInt32(jobTrips[jobTrips.Count - 1].EndTime.Substring(0, 2));
                int eMin = Convert.ToInt32(jobTrips[jobTrips.Count - 1].EndTime.Substring(2, 2));
                DateTime startDateTime = jobTrips[jobTrips.Count - 1].StartDate.AddHours(sHr).AddMinutes(sMin);
                DateTime endDateTime = jobTrips[jobTrips.Count - 1].EndDate.AddHours(eHr).AddMinutes(eMin);

                //create plan trip
                planTrip = FindPlanHaulierTripByDriver(planTrips, driver);
                if (planTrip == null)
                {
                    planTrip = new PlanHaulierTrip("", startDateTime.Date, Vehicle.GetVehicle(driver.defaultVehicleNumber), driver, new byte[8]);
                    //add plantrip to collection of plantrips
                    planTrips.Add(planTrip);
                }
                //20151221 - gerry added validation
                if (!PlanHaulierTrip.HasPlanTripsChanged(planTrip))
                {
                    //create PlanHaulierSubtripJobs collection to assigned in plansubtrip
                    SortableList<HaulierJobTrip> planSubTripJobs = new SortableList<HaulierJobTrip>();
                    planSubTripJobs.Add(jobTrips[jobTrips.Count - 1]); //assigned the last collection which is the newly added trip
                    //check time if previous leg is not after the next leg
                    CheckStartTimAndGetTrailerForOneLegAndMultiLeg(planTrips, planSubTripJobs, startDateTime, endDateTime, out vehicle);
                    //TODO check overlapping
                    if (!TrailerLocation.IsTrailerTimeOverlapping(trailerLocations, vehicle.Number, startDateTime, endDateTime))
                    {
                        //create plan sub trip
                        newlyCreatedPlanSubTrip = new PlanHaulierSubTrip(1, startDateTime, endDateTime, jobTrips[jobTrips.Count - 1].StartStop,
                            jobTrips[jobTrips.Count - 1].EndStop, jobTrips[jobTrips.Count - 1].isBillable, "Auto generated!", planSubTripJobs, vehicle, planTrip, JobTripStatus.Assigned);
                        newlyCreatedPlanSubTrip.IncentiveCode = incentiveCode;
                        if (planTrip.ValidateAddPlanSubTrip(newlyCreatedPlanSubTrip))
                        {
                            if (!planTrip.haulierSubTrips.Contains(newlyCreatedPlanSubTrip))
                            {  //add plasubtrip to plantrip
                                planTrip.AddSubTrip(newlyCreatedPlanSubTrip);
                            }
                        }
                        else
                        {
                            if (planTrip.haulierSubTrips.Count == 0)
                                planTrips.Remove(planTrip);
                            throw new FMException(TptResourceBLL.ErrPlanHaulierSubTripOverlapped);
                        }
                        //form trailer location if vehicle is not null
                        if (!vehicle.Number.Equals(string.Empty))
                        {
                            if (TrailerLocation.IsTrailerTimeOverlapping(trailerLocations, vehicle.Number, startDateTime, endDateTime))
                                throw new FMException(TptResourceBLL.ErrTrailerOverlapping);
                            #region Add trailer location and set status to CustomerStuff
                            TrailerLocation tl1 = new TrailerLocation();
                            tl1.TrailerNo = vehicle.Number;
                            tl1.ScheduleDate = startDateTime.Date;
                            tl1.ChangeDate = DateTime.Now;
                            tl1.StartTime = startDateTime;
                            tl1.EndTime = endDateTime;
                            tl1.TrailerStatus = TrailerStatus.CustomerStuff;//default to customer Stuff
                            tl1.StartStop = jobTrips[0].StartStop.Code.ToString();
                            tl1.StopCode = jobTrips[0].EndStop.Code.ToString();
                            tl1.ParkStop = jobTrips[0].EndStop.Code.ToString();
                            tl1.PlanTripNo = planTrip.PlanTripNo.ToString();
                            tl1.DriverNo = driver.Code.ToString();
                            tl1.Remarks = "Auto Assigned!";
                            #endregion
                            trailerLocations.Add(tl1);
                        }
                    }
                }
            }
            catch (FMException ex)
            {
                if (planTrip != null)
                {
                    if (newlyCreatedPlanSubTrip != null)
                    {
                        planTrip.DeleteSubTrip(newlyCreatedPlanSubTrip);
                    }
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (planTrip != null)
                {
                    if (newlyCreatedPlanSubTrip != null)
                    {
                        planTrip.DeleteSubTrip(newlyCreatedPlanSubTrip);
                    }
                }
                throw new FMException(ex.Message);
            }
            return true;
        }
        //not yet complete
        private bool ValidateAddPlanSubTrip(PlanHaulierSubTrip planSubTrip)
        {
            foreach (PlanHaulierSubTrip subtrip in this.haulierSubTrips)
            {
                if (subtrip.Start < planSubTrip.Start)
                {
                    if (subtrip.End > planSubTrip.Start)
                        return false; 
                }                                        
                else if (subtrip.Start > planSubTrip.Start)
                {
                    if (subtrip.Start < planSubTrip.End) 
                        return false;                     
                }
                else if (subtrip.Start == planSubTrip.Start)
                    return false;
            }
            return true;
        }

        //20141124 - gerry added for 20' conatiner to check previous leg of the trip when drag from jobtrip form to planning screen
        //parameter job trips are the selected trip to assign in planning
        public static bool Validate20ftIfNot1stLeg(SortableList<HaulierJobTrip> jobTrips, out string outMsg)
        {
            outMsg = string.Empty;
            try
            {
                foreach (HaulierJobTrip trip in jobTrips)
                {
                    if (trip.LegType != Leg_Type.FirstOfTwoLeg && trip.isMulti_leg)
                    {
                        HaulierJobTrip prvTrip = HaulierJobTrip.GetPreviousLegJobTrip(trip.JobID, trip.Sequence, trip.ContainerNo);
                        HaulierJob haulierJob = HaulierJob.GetHaulierJob(trip.JobNo);
                        if (haulierJob.JobTransferType == JobTransferType.TransferIn && !trip.PartnerJobNo.Equals(string.Empty))
                        {
                            HaulierJob partnerJob = HaulierJob.GetHaulierJob(trip.PartnerJobNo);
                            prvTrip = HaulierJobTrip.GetPreviousLegJobTrip(partnerJob.JobID, trip.ContainerNo, trip.LegGroupMember);
                        }
                        string outPlanTripNo = string.Empty; //just initialize to reused method
                        PlanHaulierSubTrip subTrip = PlanTransportDAL.GetPlanHaulierSubTripFromPreviousLeg(prvTrip.JobID, prvTrip.Sequence, out outPlanTripNo);
                        if (subTrip != null)
                        {
                            if (jobTrips.Count == 1 && subTrip.JobTrips.Count == 2)
                                outMsg = string.Format(TptResourceBLL.Warn1TripAndPrevLegDoubleMount, jobTrips[0].ContainerNo == subTrip.JobTrips[0].ContainerNo ? subTrip.JobTrips[1].ContainerNo : subTrip.JobTrips[0].ContainerNo); //"There is a second container no: {0} in the previous leg. "; //{0} contNo double mount of previous leg;
                            else if (jobTrips.Count == 2 && subTrip.JobTrips.Count == 1)
                                outMsg = TptResourceBLL.Warn2TripAndPrevLeg1Trip; //"There is only 1 container in the previous leg. ";
                            else if (jobTrips.Count == 2 && subTrip.JobTrips.Count == 2)
                            {
                                //check if 2nd container  is inside the trips of previous plansubtrip
                                int cont2ndcount = subTrip.JobTrips.Where(jt => jt.ContainerNo == jobTrips[1].ContainerNo).ToList().Count;
                                if (cont2ndcount == 0)
                                {
                                    string Cont2ndNo = subTrip.JobTrips.Where(jt => jt.ContainerNo != trip.ContainerNo).ToList()[0].ContainerNo;
                                    outMsg = string.Format(TptResourceBLL.Warn2TripAndPrevLegDoubleMount, Cont2ndNo, jobTrips[1].ContainerNo); // "In the previous leg, the double mount conatiner was {0} and not {1}. ";// {0} contNo double mount of previous leg; {1} contNo double of current leg
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }

        public byte[] GetPlanHaulierTripUpdateVersion()
        {
            byte[] retValue = new byte[8];
            try
            {
                retValue = PlanTransportDAL.GetPlanHaulierTripUpdateVersion(this);
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
        //20151130 - gerry added to check if driver was being update
        public bool IsPlanHaulierTripUpdated()
        {
            return PlanTransportDAL.IsPlanHaulierTripUpdated(this);
        }
        //20151130 - gerry added to get specific driver being updated
        public static PlanHaulierTrip GetPlanHaulierTripByDateAndDriver(string driverCode, DateTime date, string tptDept)
        {
            return PlanTransportDAL.GetPlanHaulierTripByDateAndDriver(driverCode, date, tptDept);
        }
        //20151130 - gerry added to get specific vehicle being updated
        public static PlanHaulierTrip GetPlanHaulierTripByDateAndVehicle(string vehicleNo, DateTime date, string tptDept)
        {
            return PlanTransportDAL.GetPlanHaulierTripByDateAndVehicle(vehicleNo, date, tptDept);            
        }

        //start of messaging logic
        //20150523 - gerry added to update affected plan trip based the the message sent from vehicle/driver
        //this method will form the current plantrip, assign trailer to plan sub trip or set contain or sealNo to job trip.
        public static void PerformUpdateMessage(TripUpdate02 tripUpdate, SortableList<PlanHaulierTrip> planTrips, SortableList<TrailerLocation> trailerlocations,
            string tptDept, Hashtable trailerlocationhashtbl, string userID, string frmName, SortableList<HaulierJobTrip> subConJobTrips)
        {
            string errMsg = string.Empty;
            try
            {

                //get TripInstruct based on headNo and trip seqNo
                //TripInstruction01 tripInstruction = TripInstruction01.GetTripInstruction(tripUpdate.HeadNo, tripUpdate.SeqNo);
                //get the vehicle object
                Vehicle primeMover = Vehicle.GetVehicle(tripUpdate.PrimeMover.Trim());
                //Driver driver = Driver.GetDriver(tripInstruction.DriverCode);
                //initialize an affectedPlanTrip
                SortableList<PlanHaulierTrip> affectedPlanTrips = new SortableList<PlanHaulierTrip>();
                //get the affected haulierPlanSubTrip
                PlanHaulierSubTrip affectedPlanSubTrip = null;//get the current planTrip
                PlanHaulierTrip currentPlanTrip = FindPlanHaulierTripByVehicle(planTrips, primeMover);//FindPlanHaulierTripByDriver(planTrips, driver); //
                if (currentPlanTrip == null)
                    throw new FMException("This job is not assigned to this vehicle(" + tripUpdate.PrimeMover + ") for this scheduled plan date, please double check your sent messages. ");
                else
                {
                    //20151221 -gerry added checking if driver/vehicle was being updated
                    if (!HasPlanTripsChanged(currentPlanTrip))
                    {
                        currentPlanTrip.HaulierSubTrips.SortByStartTime();
                        affectedPlanSubTrip = PlanHaulierSubTrip.GetHaulierPlanSubTripBaseOnMessage(currentPlanTrip, tripUpdate);
                        //if status is complete then do nothing
                        if (affectedPlanSubTrip.Status == JobTripStatus.Completed)
                            throw new FMException("Appointment was being COMPLETED manually. This message cannot be process unless you set the appointment back to ASSIGNED. ");
                        else
                        {
                            //process message body
                            MessageTripStatus status = 0;
                            switch (tripUpdate.MessageBody)
                            {
                                case "00":
                                    status = MessageTripStatus.Received;
                                    if (affectedPlanSubTrip != null)
                                    {
                                        if (affectedPlanSubTrip.Status == JobTripStatus.Completed)
                                            status = MessageTripStatus.Completed;
                                    }
                                    break;
                                case "01":
                                    status = MessageTripStatus.InProgress;
                                    if (affectedPlanSubTrip != null)
                                    {
                                        if (affectedPlanSubTrip.Status == JobTripStatus.Completed)
                                            status = MessageTripStatus.Completed;
                                    }
                                    break;
                                case "07":
                                    if (affectedPlanSubTrip != null)
                                    {
                                        if (affectedPlanSubTrip.Trailer.Number == string.Empty) { throw new FMException(TptResourceBLL.ErrNoTrailer); }
                                        foreach (HaulierJobTrip jt in affectedPlanSubTrip.JobTrips)
                                        {
                                            if (jt.ContainerNo == string.Empty) { throw new FMException(TptResourceBLL.ErrContainerNoBlank); }
                                            jt.TripStatus = JobTripStatus.Completed;
                                            jt.oldContainerNo = jt.ContainerNo;
                                            jt.oldSealNo = jt.SealNo;
                                        }
                                        //TRAILER default to Trailer Park status when completed
                                        trailerlocations = TrailerLocation.DeleteTrailerLocationFromCollection(trailerlocations, affectedPlanSubTrip.Trailer.Number, affectedPlanSubTrip.Start);
                                        AddTrailerLocationToCollection(trailerlocations, affectedPlanSubTrip.Trailer.Number, affectedPlanSubTrip, currentPlanTrip.PlanTripNo, TrailerStatus.TrailerPark);

                                        affectedPlanSubTrip.Status = JobTripStatus.Completed;
                                        affectedPlanSubTrip.messageTripStatus = MessageTripStatus.Completed;
                                    }
                                    status = MessageTripStatus.Completed;
                                    break;
                                case "11":
                                    status = MessageTripStatus.Rejected;
                                    break;
                                case "12":
                                    status = MessageTripStatus.OnHold;
                                    break;
                                default:
                                    //CheckMessageBodyValues(tripUpdate, out errMsg);
                                    //get the properties and values from message body
                                    if(AssignedMessageBodyValues(tripUpdate, currentPlanTrip.PlanTripNo, affectedPlanSubTrip, ref trailerlocations, tptDept))
                                        status = MessageTripStatus.InProgress;
                                    break;
                            }
                            //TODO update plan trip
                            affectedPlanTrips.Add(currentPlanTrip);
                            UpdateAllAffectedHaulierPlanTrips(affectedPlanTrips, trailerlocations, tptDept, trailerlocationhashtbl, userID, frmName, subConJobTrips, tripUpdate, status);
                            //generate incentives
                            if (affectedPlanSubTrip.Status == JobTripStatus.Completed && affectedPlanSubTrip.JobTrips[0].isMovementJob)
                            {
                                Incentive.GenerateDriverIncentive(currentPlanTrip.PlanTripNo, affectedPlanSubTrip, tptDept, frmName, userID);
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message); }
        }
        public static List<PropertyNameValuePair> ConvertTripUpdateIntoPropertyNameValuePairs(TripUpdate02 tripUpdate)
        {
            string[] propertyNameValuePair = tripUpdate.MessageBody.Split('~');
            List<PropertyNameValuePair> pairs = new List<PropertyNameValuePair>();
            try
            {
                foreach (string tmp in propertyNameValuePair)
                {
                    if (tmp == "00" || tmp == "01" || tmp == "07" || tmp == "11" || tmp == "12" || tmp == "A")
                    {
                        pairs.Add(new PropertyNameValuePair(tmp, "")); //DONOT capture
                    }
                    else
                    {
                        string[] pair = tmp.Split('=');
                        pairs.Add(new PropertyNameValuePair(pair[0], pair[1]));
                    }
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException e) { throw e; }
            catch (Exception e) { throw new FMException(e.Message.ToString()); }

            return pairs;
        }
        private static bool AssignedMessageBodyValues(TripUpdate02 tripUpdate, string planTripNo, PlanHaulierSubTrip affectedPlanSubTrip, ref SortableList<TrailerLocation> trailerlocations, string deptCode)
        {
            try
            {
                List<PropertyNameValuePair> pairs = ConvertTripUpdateIntoPropertyNameValuePairs(tripUpdate);
                bool isDoubleMount = affectedPlanSubTrip.JobTrips.Count > 1;
                foreach (PropertyNameValuePair nameValuePair in pairs)
                {
                    if (nameValuePair.pValue.Contains("\0"))
                    {
                        int indx1 = nameValuePair.pValue.IndexOf("\0");
                        nameValuePair.pValue = nameValuePair.pValue.Substring(0, indx1);
                    }
                    switch (nameValuePair.pName)
                    {
                        case "02": //update job trip containerNo
                            if (affectedPlanSubTrip != null)
                            {
                                affectedPlanSubTrip.JobTrips[0].ContainerNo = nameValuePair.pValue.Trim();
                                if (isDoubleMount)
                                {
                                    if (nameValuePair.pValue.Contains('+'))
                                    {
                                        string[] contNos = nameValuePair.pValue.Split('+');
                                        affectedPlanSubTrip.JobTrips[0].ContainerNo = contNos[0];
                                        affectedPlanSubTrip.JobTrips[1].ContainerNo = contNos[1];
                                    }
                                }
                            }
                            break;
                        case "03": //update job trip sealNo
                            if (affectedPlanSubTrip != null)
                            {
                                affectedPlanSubTrip.JobTrips[0].SealNo = nameValuePair.pValue;
                                if (isDoubleMount)
                                {
                                    if (nameValuePair.pValue.Contains('+'))
                                    {
                                        string[] sealNos = nameValuePair.pValue.Split('+');
                                        affectedPlanSubTrip.JobTrips[0].SealNo = sealNos[0];
                                        affectedPlanSubTrip.JobTrips[1].SealNo = sealNos[1];
                                    }
                                }
                            }
                            break;
                        case "04": //update job trip trailer No
                            if (affectedPlanSubTrip != null)
                            {
                                trailerlocations = TrailerLocation.DeleteTrailerLocationFromCollection(trailerlocations, affectedPlanSubTrip.Trailer.Number, affectedPlanSubTrip.Start);
                                //validate trailer
                                var validTrailers = TrailerLocation.GetAvailableTrailersForPeriod(trailerlocations,
                                                        affectedPlanSubTrip.Start, affectedPlanSubTrip.End, affectedPlanSubTrip.Trailer.Number, deptCode);

                                string contCode = affectedPlanSubTrip.JobTrips[0].ContainerCode.ToString();

                                if (isDoubleMount || contCode.Substring(0, contCode.Length).Contains("4"))
                                    validTrailers = TrailerLocation.GetNot20FtTrailersOnly(validTrailers);
                                //get previous plan sub trip trailer
                                //Vehicle previousLegTrailer = PlanHaulierTrip.GetTrailerFromPreviousLeg(parent.planHaulierTrips, affectedPlanSubTrip);

                                int count = validTrailers.Where(t => t.Number == nameValuePair.pValue).ToList().Count();
                                if (count > 0)
                                {
                                    affectedPlanSubTrip.Trailer = Vehicle.GetVehicle(nameValuePair.pValue);
                                    AddTrailerLocationToCollection(trailerlocations, nameValuePair.pValue, affectedPlanSubTrip, planTripNo, TrailerStatus.Assigned);
                                }
                                else
                                    throw new FMException(string.Format("Oops! Trailer was not in the list of the available trailer(s) for this plan date ({0}).", affectedPlanSubTrip.Start.ToShortDateString()));
                            }
                            break;
                        //05 = DO
                        //06 = Remark
                    }
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException e) { throw e; }
            catch (Exception e) { throw new FMException(e.Message.ToString()); }
            return true;
        }
        public static void CheckMessageBodyValues(TripUpdate02 tripUpdate, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;
                List<PropertyNameValuePair> pairs = ConvertTripUpdateIntoPropertyNameValuePairs(tripUpdate);

                foreach (PropertyNameValuePair nameValuePair in pairs)
                {
                    if (nameValuePair.pValue.Contains("\0"))
                    {
                        int indx1 = nameValuePair.pValue.IndexOf("\0");
                        nameValuePair.pValue = nameValuePair.pValue.Substring(0, indx1);
                    }
                    string jobNo = tripUpdate.HeadNo;
                    string seqNo = tripUpdate.SeqNo;
                    string[] jobNos = null;
                    string[] seqNos = null;
                    if (tripUpdate.HeadNo.Contains("+"))
                    {
                        jobNos = tripUpdate.HeadNo.Split('+');
                        jobNo = jobNos[0].ToString().Trim();
                    }
                    if (tripUpdate.SeqNo.Contains("+"))
                    {
                        seqNos = tripUpdate.SeqNo.Split('+');
                        seqNo = seqNos[0].ToString().Trim();
                    }
                    if (!(jobNo == "" || seqNo == ""))
                    {
                        int jobID = HaulierJob.GetHaulierJob(jobNo).JobID;
                        HaulierJobTrip firstJobTrip = HaulierJobTrip.GetHaulierJobTrip(jobID, Convert.ToInt32(seqNo));

                        //string firstContNo = HaulierJobTrip.GetHaulierJobTrip(jobID, Convert.ToInt32(seqNo)).ContainerNo;
                        //string firstSealNo = HaulierJobTrip.GetHaulierJobTrip(jobID, Convert.ToInt32(seqNo)).SealNo;

                        if (nameValuePair.pValue.Contains("\0"))
                        {
                            int indx1 = nameValuePair.pValue.IndexOf("\0");
                            nameValuePair.pValue = nameValuePair.pValue.Substring(0, indx1);
                        }
                        switch (nameValuePair.pName)
                        {
                            case "02": //check container number
                                CheckContainerNo(firstJobTrip, out errMsg, jobNos, seqNos, nameValuePair);
                                break;
                            case "03": //check seal number
                                CheckSealNo(firstJobTrip, out errMsg, jobNos, seqNos, nameValuePair);
                                break;
                            case "04": // check trailer number
                                break;
                            case "08": // check book reference number
                                CheckRefNo(firstJobTrip, out errMsg, jobNos, seqNos, nameValuePair);
                                break;
                        }
                    }
                    else throw new FMException(TptResourceBLL.ErrorJobNoEmpty);
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException e) { throw e; }
            catch (Exception e) { throw new FMException(e.Message.ToString()); }
        }
        public static void CheckContainerNo(HaulierJobTrip firstJobTrip, out string errMsg, string[] jobNos, string[] seqNos, PropertyNameValuePair nameValuePair)
        {
            errMsg = string.Empty;
            try
            {
                if (nameValuePair.pValue.Contains("+"))
                {
                    string[] contNos = new string[2];
                    contNos = nameValuePair.pValue.Split('+');
                    //check first container number
                    if (HaulierJobTrip.IsContainerNoFreeToUse(contNos[0], firstJobTrip.JobID, firstJobTrip.LegGroup))
                    {
                        if (firstJobTrip.ContainerNo.Equals(string.Empty) && firstJobTrip.LegGroupMember == 1)
                        {
                            //no need to check for first leg blank container number else need to check
                        }
                        else
                        {
                            if (!firstJobTrip.ContainerNo.Equals(contNos[0]))
                                throw new FMException("Opps! Container number " + contNos[0].ToString() + " sent from vehicle is different from the current data which is " + (firstJobTrip.ContainerNo == string.Empty ? "BLANK" : firstJobTrip.ContainerNo) + ". Please double check with the driver. ");
                        }
                    }
                    //check 2nd container        
                    //get 2nd container
                    HaulierJobTrip secondTrip = null;
                    if (jobNos != null)
                    {
                        int jobID = HaulierJob.GetHaulierJob(jobNos[1]).JobID;
                        secondTrip = HaulierJobTrip.GetHaulierJobTrip(jobID, Convert.ToInt32(seqNos[1]));
                        HaulierJobTrip.IsContainerNoFreeToUse(contNos[1], secondTrip.JobID, secondTrip.LegGroup);
                    }
                    else
                    {
                        secondTrip = HaulierJobTrip.GetHaulierJobTrip(firstJobTrip.JobID, Convert.ToInt32(seqNos[1]));
                        HaulierJobTrip.IsContainerNoFreeToUse(contNos[1], secondTrip.JobID, secondTrip.LegGroup);
                    }
                    if (secondTrip != null)
                    {
                        if (secondTrip.ContainerNo.Equals(string.Empty) && secondTrip.LegGroupMember == 1)
                        {
                            //no need to check for first leg blank container number else need to check
                        }
                        else
                        {
                            if (!secondTrip.ContainerNo.Equals(contNos[1]))
                                throw new FMException("Opps! Container number " + contNos[1].ToString() + " sent from vehicle is different from the current data which is " + (secondTrip.ContainerNo == string.Empty ? "BLANK" : secondTrip.ContainerNo) + ". Please double check with the driver. ");
                        }
                    }
                    //check both container for valid ISO standard
                    int errCount = 0;
                    foreach (string contNo in contNos)
                    {
                        if (!TransportFacadeIn.IsValidContainerNoBasedOnISO(contNo))
                        {
                            errCount++;
                            errMsg = (errCount == 1 ? "1 container number is" : "Both container numbers are") + " not valid for ISO standard. ";
                        }
                    }
                    #region REplaced
                    /*
                    string sencondCont = HaulierJobTrip.GetHaulierJobTrip(jobID, Convert.ToInt32(seqNos[1])).ContainerNo;
                    if (HaulierJobTrip.IsContainerNoFreeToUse(contNos[0].ToString(), jobID, Convert.ToInt32(seqNos[0])) && HaulierJobTrip.IsContainerNoFreeToUse(contNos[1].ToString(), jobID, Convert.ToInt32(seqNos[0])))
                    {
                        if ((!firstCont.Equals(contNos[0]) && !firstCont.Equals(string.Empty)) ||
                            (!sencondCont.Equals(contNos[1]) && !sencondCont.Equals(string.Empty)))
                        {
                            throw new FMException("Opps! Container number sent from vehicle is different. Please double check with the driver. ");
                        }
                        else
                        {
                            foreach (string contNo in contNos)
                            {
                                if (!TransportFacadeIn.IsValidContainerNoBasedOnISO(nameValuePair.pValue))
                                {
                                    errCount++;
                                    errMsg = (errCount == 1 ? "1 container number is" : "Both container numbers are") + " not valid for ISO standard. ";
                                }
                            }
                        }
                    }
                     */
                    #endregion
                }
                else
                {

                    if (HaulierJobTrip.IsContainerNoFreeToUse(nameValuePair.pValue.ToString(), firstJobTrip.JobID, firstJobTrip.LegGroup))
                    {
                        if (firstJobTrip.ContainerNo.Equals(string.Empty) && firstJobTrip.LegGroupMember == 1)
                        {
                            //no need to check for first leg blank container number else need to check
                        }
                        else
                        {
                            if (!firstJobTrip.ContainerNo.Equals(nameValuePair.pValue.ToString().Trim()))
                                throw new FMException("Opps! Container number " + nameValuePair.pValue.ToString() + " sent from vehicle is different from the current data which is " + (firstJobTrip.ContainerNo == string.Empty ? "BLANK" : firstJobTrip.ContainerNo) + ". Please double check with the driver. ");
                        }
                        if (!TransportFacadeIn.IsValidContainerNoBasedOnISO(nameValuePair.pValue))
                            errMsg += "Container number is not valid for ISO standard. ";
                    }
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException e) { throw e; }
            catch (Exception e) { throw new FMException(e.Message.ToString()); }
        }
        public static void CheckSealNo(HaulierJobTrip firstJobTrip, out string errMsg, string[] jobNos, string[] seqNos, PropertyNameValuePair nameValuePair)
        {
            errMsg = string.Empty;
            try
            {
                if (nameValuePair.pValue.Contains("+"))
                {
                    string[] sealNos = new string[2];
                    sealNos = nameValuePair.pValue.Split('+');
                    int errCount = 0;
                    //check first container number
                    if (HaulierJobTrip.IsSealNoFreeToUse(sealNos[0], firstJobTrip.JobID, firstJobTrip.LegGroup))
                    {
                        if (firstJobTrip.SealNo.Equals(string.Empty) && firstJobTrip.LegGroupMember == 1)
                        {
                            //no need to check for first leg blank container number else need to check
                        }
                        else
                        {
                            if (!firstJobTrip.SealNo.Equals(sealNos[0]))
                                throw new FMException("Opps! Seal number " + sealNos[0].ToString() + " sent from vehicle is different from the current data which is " + (firstJobTrip.SealNo == string.Empty ? "BLANK" : firstJobTrip.SealNo) + ". Please double check with the driver. ");
                        }
                    }
                    HaulierJobTrip secondJobTrip = null;
                    if (jobNos != null)
                    {
                        int jobID = HaulierJob.GetHaulierJob(jobNos[1]).JobID;
                        secondJobTrip = HaulierJobTrip.GetHaulierJobTrip(jobID, Convert.ToInt32(seqNos[1]));
                        HaulierJobTrip.IsSealNoFreeToUse(sealNos[1], secondJobTrip.JobID, secondJobTrip.LegGroup);
                    }
                    else
                    {
                        secondJobTrip = HaulierJobTrip.GetHaulierJobTrip(firstJobTrip.JobID, Convert.ToInt32(seqNos[1]));
                        HaulierJobTrip.IsSealNoFreeToUse(sealNos[1], secondJobTrip.JobID, secondJobTrip.LegGroup);
                    }

                    if (secondJobTrip != null)
                    {
                        if (secondJobTrip.SealNo.Equals(string.Empty) && secondJobTrip.LegGroupMember == 1)
                        {
                            //no need to check for first leg blank container number else need to check
                        }
                        else
                        {
                            if (!secondJobTrip.SealNo.Equals(sealNos[1]))
                                throw new FMException("Opps! Seal number " + sealNos[1].ToString() + " sent from vehicle is different from the current data which is " + (secondJobTrip.SealNo == string.Empty ? "BLANK" : secondJobTrip.SealNo) + ". Please double check with the driver. ");
                        }
                    }
                    #region Replaced
                    /*
                    //get 2nd container
                    if (jobNos != null)
                        jobID = HaulierJob.GetHaulierJob(jobNos[1]).JobID;

                    string sencondSeal = HaulierJobTrip.GetHaulierJobTrip(jobID, Convert.ToInt32(seqNos[1])).ContainerNo;
                    if (HaulierJobTrip.IsSealNoFreeToUse(sealNos[0].ToString(), jobID, Convert.ToInt32(seqNos[0])) && HaulierJobTrip.IsSealNoFreeToUse(sealNos[1].ToString(), jobID, Convert.ToInt32(seqNos[0])))
                    {

                        if ((!firstSeal.Equals(sealNos[0]) && !firstSeal.Equals(string.Empty)) ||
                            (!sencondSeal.Equals(sealNos[1]) && !sencondSeal.Equals(string.Empty)))
                        {
                            throw new FMException("Opps! Seal number sent from vehicle is different. Please double check with the driver. ");
                        }
                    }
                     */
                    #endregion
                }
                else
                {
                    if (HaulierJobTrip.IsSealNoFreeToUse(nameValuePair.pValue, firstJobTrip.JobID, firstJobTrip.LegGroup))
                    {
                        if (firstJobTrip.SealNo.Equals(string.Empty) && firstJobTrip.LegGroupMember == 1)
                        {
                            //no need to check for first leg blank container number else need to check
                        }
                        else
                        {
                            if (!firstJobTrip.SealNo.Equals(nameValuePair.pValue))
                                throw new FMException("Opps! Seal number " + nameValuePair.pValue.ToString() + " sent from vehicle is different from the current data which is " + (firstJobTrip.SealNo == string.Empty ? "BLANK" : firstJobTrip.SealNo) + ". Please double check with the driver. ");
                        }
                    }
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException e) { throw e; }
            catch (Exception e) { throw new FMException(e.Message.ToString()); }
        }
        public static void CheckRefNo(HaulierJobTrip firstJobTrip, out string errMsg, string[] jobNos, string[] seqNos, PropertyNameValuePair nameValuePair)
        {
            errMsg = string.Empty;
            try
            {
                if (nameValuePair.pValue.Contains("+"))
                {
                    string[] RefNos = new string[2];
                    RefNos = nameValuePair.pValue.Split('+');
                    int errCount = 0;
                    //check first book reference number    
                    string last4DbookRefNo = string.Empty;
                    if (!firstJobTrip.BookRefNo.Equals(string.Empty) && firstJobTrip.BookRefNo.Length >= 4)
                    {
                        last4DbookRefNo = firstJobTrip.BookRefNo.Substring(firstJobTrip.BookRefNo.Length - 4, 4);
                        if (last4DbookRefNo != RefNos[0].ToString())
                            throw new FMException("Opps! Book reference number " + RefNos[0].ToString() + " sent from vehicle is different from the current data which is " + (firstJobTrip.BookRefNo == string.Empty ? "BLANK" : firstJobTrip.BookRefNo) + ". Please double check with the driver. ");
                    }
                    else
                        throw new FMException("Opps! Book reference number " + RefNos[0].ToString() + " sent from vehicle is different from the current data which is BLANK. Please double check with the driver. ");

                    string secondBookRefNo = string.Empty;
                    if (jobNos != null)
                    {
                        secondBookRefNo = HaulierJob.GetHaulierJob(jobNos[1]).BookingRef;
                        if (!secondBookRefNo.Equals(string.Empty) && secondBookRefNo.Length >= 4)
                        {
                            last4DbookRefNo = secondBookRefNo.Substring(secondBookRefNo.Length - 4, 4);
                            if (last4DbookRefNo != RefNos[1].ToString())
                                throw new FMException("Opps! Book reference number " + RefNos[1].ToString() + " sent from vehicle is different from the current data which is " + (secondBookRefNo == string.Empty ? "BLANK" : secondBookRefNo) + ". Please double check with the driver. ");
                        }
                        else
                            throw new FMException("Opps! Book reference number " + RefNos[1].ToString() + " sent from vehicle is different from the current data which is BLANK. Please double check with the driver. ");
                    }
                    else // if job number is the same with the first job trip then referenceNo should be the same
                    {
                        if (last4DbookRefNo != RefNos[1].ToString())
                            throw new FMException("Opps! Book reference number " + RefNos[1].ToString() + " sent from vehicle is different from the current data which is " + (firstJobTrip.BookRefNo == string.Empty ? "BLANK" : firstJobTrip.BookRefNo) + ". Please double check with the driver. ");
                    }
                }
                else
                {
                    if (!firstJobTrip.BookRefNo.Equals(string.Empty))
                    {
                        string last4DbookRefNo = firstJobTrip.BookRefNo.Substring(firstJobTrip.BookRefNo.Length - 4, 4);
                        if (last4DbookRefNo != nameValuePair.pValue.ToString())
                            throw new FMException("Opps! Book reference number " + nameValuePair.pValue.ToString() + " sent from vehicle is different from the current data which is " + (firstJobTrip.BookRefNo == string.Empty ? "BLANK" : firstJobTrip.BookRefNo) + ". Please double check with the driver. ");
                    }
                    else
                        throw new FMException("Opps! Book reference number " + nameValuePair.pValue.ToString() + " sent from vehicle is different from the current data which is " + (firstJobTrip.BookRefNo == string.Empty ? "BLANK" : firstJobTrip.BookRefNo) + ". Please double check with the driver. ");
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException e) { throw e; }
            catch (Exception e) { throw new FMException(e.Message.ToString()); }
        }
        //20161130 - gerry added to get new data for affect driver/ plantrip
        public static void GetDataForAffectedPlanTrip(ref SortableList<PlanHaulierTrip> planTrips, List<Driver> affectedDrivers)
        {
            try
            {
                foreach (PlanHaulierTrip planTrip in planTrips)
                {
                    var tempDriver = affectedDrivers.FirstOrDefault(dr => dr.Code.Trim() == planTrip.Driver.Code.Trim());
                    if (tempDriver != null && !tempDriver.Code.Trim().Equals(string.Empty))
                    {
                        if (planTrip.Driver.Code.Trim() == tempDriver.Code.Trim())
                        {
                            planTrip.HaulierSubTrips = PlanTransportDAL.GetPlanHaulierSubTrips(planTrip);
                            planTrip.CopyPlanHaulierSubTripsToOldPlanHaulierSubTrips();
                            planTrip.UpdateVersion = PlanTransportDAL.GetPlanHaulierTripUpdateVersion(planTrip);
                        }
                    }
                }
            }
            catch (NullReferenceException ex) { throw new FMException(ex.Message); }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message); }
        }
    }
}

