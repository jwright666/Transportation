using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.DAL;
using TR_LanguageResource.Resources;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class TrailerLocation
    {
        private string trailerNo;
        private DateTime scheduleDate;
        private TrailerStatus trailerStatus;

        private DateTime startTime;
        private DateTime endTime;

        private string startStop;
        private string stopCode;
        private string parkStop;
        private string driverNo;
        private string planTripNo;
        private string remarks;
        private DateTime changeDate;
        private byte[] updateVersion;

        private decimal maximumLadenWeight = 0;
        private string trailerType = "";
        private string driverName = "";

        public string DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }

        public decimal MaximumLadenWeight
        {
            get { return maximumLadenWeight; }
            set { maximumLadenWeight = value; }
        }

        public string TrailerType
        {
            get { return trailerType; }
            set { trailerType = value; }
        }

        public string TrailerNo
        {
            get { return trailerNo; }
            set {trailerNo = value; }
        }

        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
        }

        public TrailerStatus TrailerStatus
        {
            get { return trailerStatus; }
            set { trailerStatus = value; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public string StartStop
        {
            get { return startStop; }
            set{startStop = value;}                
        }

        public string StopCode
        {
            get { return stopCode; }
            set{stopCode = value;}                
        }

        public string ParkStop
        {
            get { return parkStop; }
            set{parkStop = value;}
        }

        public string DriverNo
        {
            get { return driverNo; }
            set {driverNo = value;}
        }

        public string PlanTripNo
        {
            get { return planTripNo; }
            set { planTripNo = value;}               
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public DateTime ChangeDate
        {
            get { return changeDate; }
            set { changeDate = value; }
        }

        public byte[] UpdateVersion
        {
            get { return updateVersion; }
            set { updateVersion = value; }
        }

        public TrailerLocation()
        {
            this.trailerNo = "";
            this.scheduleDate = DateTime.Today.Date;
            this.changeDate = DateTime.Today.Date;

            this.startTime = DateTime.Today;
            this.endTime = DateTime.Today;

            this.startStop = "";
            this.stopCode = "";
            this.parkStop = "";
            this.planTripNo = "";
            this.driverNo = "";

            this.trailerStatus = TrailerStatus.TrailerPark;
            this.remarks = "";

        }

        public TrailerLocation(string trailerNo, DateTime scheduleDate, DateTime changeDate,
            DateTime startTime, DateTime endTime,
            TrailerStatus trailerStatus, string startStop, string stopCode, string parkStop,
            string planTripNo, string driverNo, 
            string remarks, byte[] updateversion)
        {
            this.TrailerNo = trailerNo;
            this.ScheduleDate = scheduleDate;
            this.ChangeDate = changeDate;
            this.StartTime = startTime;
            this.EndTime = endTime;

            this.TrailerStatus = trailerStatus;
            this.StartStop = startStop;
            this.StopCode = stopCode;
            this.ParkStop = parkStop;
            this.PlanTripNo = planTripNo;
            this.DriverNo = driverNo;
            this.Remarks = remarks;
                            
            this.UpdateVersion = updateversion;



        }

        public static SortableList<TrailerLocation> GetAllPreviousTrailerLocations(DateTime scheduleDate, string dept)
        {
            return TrailerLocationDAL.GetAllPreviousTrailerLocations(scheduleDate,dept);
        }

        public static SortableList<TrailerLocation> GetAllTrailerLocations(DateTime scheduleDate,string dept)
        {
            return TrailerLocationDAL.GetAllTrailerLocations(scheduleDate,dept);
        }

        public static SortableList<TrailerLocation> GetAllTrailerLocationsForThisDayOnly(DateTime scheduleDate, string dept)
        {
            //return TrailerLocationDAL.GetAllTrailerLocationsForThisDayOnly(scheduleDate, dept);
            return TrailerLocationDAL.GetAllTrailerLocationsForThisDayOnlyUsedInPlanning(scheduleDate, dept);           
        }        

        public static SortableList<TrailerLocation> GetAllLatestTrailerLocations(string status)
        {
                return TrailerLocationDAL.GetAllLatestTrailerLocations(status);
        }

        public static SortableList<TrailerLocation> GetAllTrailerHistory(string trailerno, DateTime startdate, DateTime enddate)
        {
            return TrailerLocationDAL.GetAllTrailerHistory(trailerno, startdate, enddate);
        }

        public static DateTime GetLatestPlanDate()
        {
            return TrailerLocationDAL.GetLatestPlanDate();
        }

        public static void GetUpdateVersionFromHashTable(SortableList<TrailerLocation> trailerlocations, Hashtable trailerLocationHashTable)
        {
            for (int i = 0; i < trailerlocations.Count; i++)
            {

                foreach (DictionaryEntry trailerhash in trailerLocationHashTable)
                {
                    TrailerLocation temp = new TrailerLocation();
                    temp = (TrailerLocation)trailerhash.Key;

                    if ((temp.trailerNo == trailerlocations[i].trailerNo) && (temp.startTime == trailerlocations[i].startTime))
                    {
                        if ((trailerlocations[i].trailerStatus == TrailerStatus.CustomerStuff) || (trailerlocations[i].trailerStatus == TrailerStatus.CustomerUnstuff))
                        {
                            trailerlocations[i].updateVersion = (byte[])trailerhash.Value;
                        }
                    }

                }
            }
        }


        public static bool AddTrailerLocations(SortableList<TrailerLocation> trailerlocations, SqlConnection con, SqlTransaction tran, DateTime plandate, string tptDept, Hashtable trailerlocationhashtbl)
        {
            try
            {
                TrailerLocation.GetUpdateVersionFromHashTable(trailerlocations, trailerlocationhashtbl);
                ///20151123 - gerry modified this method
                ///
                ///get the actual trailer lcoations collections because it may be updated by other planner
                SortableList<TrailerLocation> DBTrailerLocations = GetAllPreviousTrailerLocations(plandate, tptDept);
                for (int i = 0; i < trailerlocations.Count; i++)
                {
                    var tempTLlist = DBTrailerLocations.Where(tl => tl.trailerNo != trailerlocations[i].trailerNo
                        && tl.driverNo == trailerlocations[i].driverNo
                        && tl.startTime == trailerlocations[i].startTime).ToList();

                    if (tempTLlist.Count > 0) { /*do nothing */ }
                    else
                        TrailerLocationDAL.AddTrailerLocations(trailerlocations[i], con, tran);
                }
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
        //20151118 - gerry modified the checking if trailer is overlapped
        public static bool IsTrailerTimeOverlapping(SortableList<TrailerLocation> trailerLocations,
            string trailerNo, DateTime existingStartTime, DateTime existingEndTime, DateTime newStartTime, DateTime newEndTime, bool isNewTrailerLocation)
        {
            bool test = false;
            for (int i = 0; i < trailerLocations.Count; i++)
            {
                if (isNewTrailerLocation)
                {
                    if (trailerNo == trailerLocations[i].trailerNo) //&& (trailerLocations[i].trailerStatus != TrailerStatus.TrailerPark))
                    {
                        if ((newStartTime == trailerLocations[i].startTime) && (newStartTime == trailerLocations[i].endTime))
                        {
                            throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                 + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                        }
                        if ((newStartTime > trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime)
                            && (newEndTime > trailerLocations[i].startTime) && (newEndTime < trailerLocations[i].endTime))
                        {
                            throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                 + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                        }
                        if ((newStartTime <= trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime) && (newEndTime >= trailerLocations[i].endTime))
                        {
                            throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                 + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                        }
                        if ((newStartTime > trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime) && (newEndTime >= trailerLocations[i].endTime))
                        {
                            throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                 + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                        }
                        if ((newStartTime <= trailerLocations[i].startTime) && (newEndTime > trailerLocations[i].endTime) && (newEndTime < trailerLocations[i].endTime))
                        {
                            throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                 + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                        }
                    }
                }
                else
                {
                    if (trailerNo == trailerLocations[i].trailerNo) // && (trailerLocations[i].trailerStatus != TrailerStatus.TrailerPark))
                    {
                        if (existingStartTime != trailerLocations[i].startTime && existingEndTime != trailerLocations[i].endTime)
                        {
                            if ((newStartTime == trailerLocations[i].startTime) && (newStartTime == trailerLocations[i].endTime))
                            {
                                throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                     + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                            }
                            if ((newStartTime > trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime)
                                && (newEndTime > trailerLocations[i].startTime) && (newEndTime < trailerLocations[i].endTime))
                            {
                                throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                     + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                            }
                            if ((newStartTime <= trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime) && (newEndTime >= trailerLocations[i].endTime))
                            {
                                throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                     + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                            }
                            if ((newStartTime > trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime) && (newEndTime >= trailerLocations[i].endTime))
                            {
                                throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                     + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                            }
                            if ((newStartTime <= trailerLocations[i].startTime) && (newEndTime > trailerLocations[i].endTime) && (newEndTime < trailerLocations[i].endTime))
                            {
                                throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                     + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                            }
                            if ((newEndTime > trailerLocations[i].startTime) && (newEndTime < trailerLocations[i].endTime))
                            {
                                throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                                     + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                            }
                        }
                    }
                }
            }
            return test;
        }

        public static bool IsTrailerTimeOverlapping(SortableList<TrailerLocation> trailerLocations, string trailerNo, DateTime newStartTime, DateTime newEndTime)
        {
            bool test = false;
            for (int i = 0; i < trailerLocations.Count; i++)
            {
                if ((trailerNo == trailerLocations[i].trailerNo)) // && (trailerLocations[i].trailerStatus != TrailerStatus.TrailerPark))
                {
                    if ((newStartTime == trailerLocations[i].startTime) && (newStartTime == trailerLocations[i].endTime))
                    {
                        throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                             + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                    }
                    if ((newStartTime > trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime)
                        && (newEndTime > trailerLocations[i].startTime) && (newEndTime < trailerLocations[i].endTime))
                    {
                        throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                             + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                    }
                    if ((newStartTime <= trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime) && (newEndTime >= trailerLocations[i].endTime))
                    {
                        throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                             + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                    }
                    if ((newStartTime > trailerLocations[i].startTime) && (newStartTime < trailerLocations[i].endTime) && (newEndTime >= trailerLocations[i].endTime))
                    {
                        throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                             + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                    }
                    if ((newStartTime <= trailerLocations[i].startTime) && (newEndTime > trailerLocations[i].endTime) && (newEndTime < trailerLocations[i].endTime))
                    {
                        throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                             + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                    }
                    if ((newEndTime > trailerLocations[i].startTime) && (newEndTime < trailerLocations[i].endTime))
                    {
                        throw new FMException("TrailerNo " + trailerLocations[i].trailerNo + " has time recorded from " + trailerLocations[i].startTime.ToShortTimeString()
                                             + " - " + trailerLocations[i].endTime.ToShortTimeString() + " under " + Driver.GetDriver(trailerLocations[i].driverNo).DescriptionForPlanningPurpose);
                    }
                }
            }
            return test;
        }

        public static SortableList<Vehicle> GetAvailableTrailersForPeriod(
            SortableList<TrailerLocation> trailerLocations,
            DateTime requiredStartTime, DateTime requiredEndTime, string currentTrailerNo, string deptCode)
        {
            SortableList<Vehicle> availableTrailers = Vehicle.GetAllAvailableTrailersBasedOnDepartment(requiredStartTime, deptCode);
            SortableList<Vehicle> returnTrailers = new SortableList<Vehicle>();
            

            //20130516 - Gerry Removed and replaced below
            #region Gerry Removed
            /*
            for (int i = 0; i < availableTrailers.Count; i++)
            {
                isAvailable = true;
                for (int j = 0; j < trailerLocations.Count; j++)
                {
                    if (trailerLocations[j].trailerNo != currentTrailerNo)
                    {
                        if ((availableTrailers[i].Number == trailerLocations[j].TrailerNo) )
                        {
                            if (trailerLocations[j].trailerStatus != TrailerStatus.TrailerPark)
                            {
                                if ((requiredStartTime >= trailerLocations[j].startTime) && (requiredStartTime < trailerLocations[j].endTime))
                                {
                                    isAvailable = false;
                                }
                                if ((requiredStartTime < trailerLocations[j].startTime) && (requiredEndTime > trailerLocations[j].endTime))
                                {
                                    isAvailable = false;
                                }
                                if ((requiredEndTime > trailerLocations[j].startTime) && (requiredEndTime < trailerLocations[j].endTime))
                                {
                                    isAvailable = false; 
                                }
                            }
                        }
                    }
                }
                if (isAvailable == true)
                {
                    returnTrailers.Add(availableTrailers[i]);
                }
            }
            */
            #endregion  

            //Now loop through the available trailers from database
            foreach (Vehicle tempVehicle in availableTrailers)
            {
                bool isAvailable = true;
                if (!tempVehicle.Number.Equals(currentTrailerNo))
                {  //loop through the trailerlocation in the plan date(parameter)
                    foreach (TrailerLocation tempTrailerLoc in trailerLocations)
                    {
                        if (tempTrailerLoc.trailerNo.Equals(tempVehicle.Number))
                        {   //we filter from assigned because there is assigned status in memory when not yet saved the whole plan
                            //20140520 - added gerry
                            if (tempTrailerLoc.trailerStatus == TrailerStatus.Assigned)
                            {
                                isAvailable = false;
                                break;
                            }
                            //20140520 end
                            else
                            {
                                if ((requiredStartTime >= tempTrailerLoc.startTime) && (requiredStartTime < tempTrailerLoc.endTime))
                                {
                                    isAvailable = false;
                                    break;
                                }
                                if ((requiredStartTime < tempTrailerLoc.startTime) && (requiredEndTime > tempTrailerLoc.endTime))
                                {
                                    isAvailable = false;
                                    break;
                                }
                                if ((requiredEndTime > tempTrailerLoc.startTime) && (requiredEndTime < tempTrailerLoc.endTime))
                                {
                                    isAvailable = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (isAvailable)
                        returnTrailers.Add(tempVehicle);
                }
            }
            //form the currentTrailer as Vehicle object, then add to the return list if not found
            if (!currentTrailerNo.Equals(string.Empty))
            {
                Vehicle currentTrailer = Vehicle.GetVehicle(currentTrailerNo);
                if (!returnTrailers.Contains(currentTrailer))
                    returnTrailers.Insert(0, currentTrailer);
            }
            return returnTrailers;
        }

        public static TrailerLocation GetTrailerLocationFromCollection(
             SortableList<TrailerLocation> trailerLocations, string trailerNo, DateTime startTime)
        {
            TrailerLocation newTrailerLocations = new TrailerLocation();
            if (trailerNo.Contains("-"))
            {
                trailerNo = trailerNo.Substring(0, trailerNo.IndexOf("-")).ToString().Trim();
            }
            for (int i = 0; i < trailerLocations.Count; i++)
            {
                // Should use & operator - this means both conditions must be be true
                // && means the second condition will not be tested if the first one is false
                if ((trailerLocations[i].TrailerNo == trailerNo 
                    & (trailerLocations[i].startTime == startTime)))
                {
                    newTrailerLocations = trailerLocations[i];
                    break;
                }
            }
            return newTrailerLocations;
        }
        //20130918 - Gerry added to get the latest trailer location from
        public static TrailerLocation GetLatestTrailerLocationFromCollection(
            SortableList<TrailerLocation> trailerLocations, string trailerNo)
        {
            SortableList<TrailerLocation> newTrailerLocations = new SortableList<TrailerLocation>();
            for (int i = 0; i < trailerLocations.Count; i++)
            {
                if ((trailerLocations[i].TrailerNo == trailerNo))
                {
                    newTrailerLocations.Add(trailerLocations[i]);
                }  
            }
            //now get the latest and loop through it 
            TrailerLocation newTrailerLocation = null;
            for (int i = 0; i < newTrailerLocations.Count; i++)
            {
                if (newTrailerLocation == null)
                {
                    newTrailerLocation = newTrailerLocations[i];
                }
                else
                {
                    if (newTrailerLocation.startTime < newTrailerLocations[i].startTime)
                    {   //replace the old with the latest 1
                        newTrailerLocation = newTrailerLocations[i];
                    }
                }
            }       
            return newTrailerLocation;
        }


        public static SortableList<TrailerLocation> DeleteTrailerLocationFromCollection(
             SortableList<TrailerLocation> trailerLocations, string trailerNo, DateTime startTime)
        {
            SortableList<TrailerLocation> newTrailerLocations = new SortableList<TrailerLocation>();
            for (int i = 0; i < trailerLocations.Count; i++)
            {
                // Should use & operator - this means both conditions must be be true
                // && means the second condition will not be tested if the first one is false
                if ((trailerLocations[i].TrailerNo == trailerNo) & (trailerLocations[i].startTime == startTime))
                {
                    // do nothing 
                }
                else
                {
                    newTrailerLocations.Add(trailerLocations[i]);
                }
            }
            return newTrailerLocations;
        }
       
        public static void SetComponentDefault(TrailerLocation trailerLocation, TrailerStatus status)
        {
            if (status == TrailerStatus.TrailerPark)
            { 
              //trailerLocation.TrailerStatus   
            }
        
        }

        public static bool CanAddTrailerLocation(TrailerLocation oldTrailerLocation, TrailerLocation newTrailerLocation, out string message)
        {
            message = "";
            bool pass = false;
            switch (newTrailerLocation.trailerStatus)
            {
                case TrailerStatus.Assigned:
                    if (oldTrailerLocation.trailerStatus == TrailerStatus.TrailerPark ||
                        oldTrailerLocation.TrailerStatus == TrailerStatus.CompleteStuffUnstuff)
                    {
                        pass = true;
                    }
                    else
                    {
                        message = TptResourceBLL.ErrCantChangeTrailerStatusToAssigned;
                        pass = false;
                    }
                    break;

                case TrailerStatus.CompleteStuffUnstuff:
                    if (oldTrailerLocation.TrailerStatus == TrailerStatus.CustomerStuff ||
                        oldTrailerLocation.TrailerStatus == TrailerStatus.CustomerUnstuff)
                    {
                        pass = true;
                    }
                    else
                    {
                        message = TptResourceBLL.ErrCantChangeTrailerStatusToCompleteStuffUnstuff;
                        pass = false;
                    }
                    break;

                case TrailerStatus.CustomerStuff:
                    if (oldTrailerLocation.TrailerStatus == TrailerStatus.Assigned)
                    {
                        pass = true;
                    }
                    else
                    {
                        message = TptResourceBLL.ErrCantChangeTrailerStatusToStuffUnstuffTraialerPark;
                        pass = false;
                    }
                    break;

                case TrailerStatus.CustomerUnstuff:
                    if (oldTrailerLocation.TrailerStatus == TrailerStatus.Assigned)
                    {
                        pass = true;
                    }
                    else
                    {
                        message = TptResourceBLL.ErrCantChangeTrailerStatusToStuffUnstuffTraialerPark;
                        pass = false;
                    }
                    break;
                case TrailerStatus.TrailerPark:
                    if (oldTrailerLocation.TrailerStatus == TrailerStatus.Assigned)
                    {
                        pass = true;
                    }
                    else
                    {
                        message = TptResourceBLL.ErrCantChangeTrailerStatusToStuffUnstuffTraialerPark;
                        pass = false;
                    }
                    break;
            }
            return pass;
        }

        public static bool AddTrailerLocation(SortableList<TrailerLocation> trailerLocations, TrailerLocation oldTrailerLocation, 
            TrailerLocation newTrailerLocation, out string message, string user, string frmName, FMModule fmModule)
        {
            message = "";
            bool added = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            if (!newTrailerLocation.driverNo.Equals(string.Empty))
            {
                if (CanAddTrailerLocation(oldTrailerLocation, newTrailerLocation, out message))
                {
                    try
                    {
                        added = TrailerLocationDAL.AddTrailerLocation(newTrailerLocation, out newTrailerLocation, con, trans, out message);
                        if (added)
                        {
                            //Form Log
                            DateTime serverDate = Logger.GetServerDateTime();

                            LogHeader logHeader = new LogHeader(fmModule, frmName, serverDate, newTrailerLocation.planTripNo, newTrailerLocation.trailerNo, FormMode.Add, user);
                            LogDetail logDetail = new LogDetail("Plan Trip No", newTrailerLocation.planTripNo);
                            LogDetail logDetail1 = new LogDetail("Trailer No", newTrailerLocation.trailerNo);
                            LogDetail logDetail2 = new LogDetail("Trailer Trailer Status", newTrailerLocation.trailerStatus.ToString());

                            logHeader.LogDetails.Add(logDetail);
                            logHeader.LogDetails.Add(logDetail1); 
                            logHeader.LogDetails.Add(logDetail2);

                            Logger.WriteLog(logHeader, con, trans);

                            trans.Commit();
                            trailerLocations.Add(newTrailerLocation);
                        }
                    }
                    catch (FMException ex)
                    {
                        trans.Rollback();
                        added = false;
                        message = ex.ToString();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        added = false;
                        message = ex.ToString();
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                added = false;
                message = TptResourceBLL.ErrDriverCode;
            }
            return added;
        }

        //This method is used for NUNIT Test only if can add empty driver.
        public static bool AddTrailerLocation(SortableList<TrailerLocation> trailerLocations, TrailerLocation oldTrailerLocation,
            TrailerLocation newTrailerLocation, SqlConnection con,SqlTransaction trans, out string message)
        {
            message = "";
            bool added = true;
            if (!newTrailerLocation.driverNo.Equals(string.Empty))
            {
                if (CanAddTrailerLocation(oldTrailerLocation, newTrailerLocation, out message))
                {
                    try
                    {
                        added = TrailerLocationDAL.AddTrailerLocation(newTrailerLocation, out newTrailerLocation, con, trans, out message);
                        if (added)
                        {
                            trailerLocations.Add(newTrailerLocation);
                        }
                    }
                    catch (FMException ex)
                    {
                        added = false;
                        message = ex.ToString();
                    }
                    catch (Exception ex)
                    {
                        added = false;
                        message = ex.ToString();
                    }
                }
            }
            else
            {
                added = false;
                message = TptResourceBLL.ErrDriverCode;
            }
            return added;
        }


        public static SortableList<TrailerLocation> GetTrailersForNewTrailerLocationStatus(DateTime scheduleDate)
        {
            SortableList<TrailerLocation> trailersavailable = new SortableList<TrailerLocation>();
            SortableList<TrailerLocation> a = new SortableList<TrailerLocation>();
            a = TrailerLocationDAL.GetTrailersForTrailerLocationStatus(scheduleDate);


            if (scheduleDate.Date == DateTime.Today.Date)
            {
                trailersavailable = a;
            }
            else
            {
                SortableList<TrailerLocation> b = new SortableList<TrailerLocation>();
                b = GetAllTrailersAssigned(DateTime.Today.Date);
                //b = TrailerLocationDAL.GetTrailersForTrailerLocationStatus(DateTime.Today.Date);
                for (int i = 0; i < a.Count; i++)
                {
                    bool find = false;
                    for (int j = 0; j < b.Count; j++)
                    {
                        if (a[i].trailerNo == b[j].trailerNo)
                        {
                            find = true;
                        }
                    }
                    if (find == false)
                    {
                        trailersavailable.Add(a[i]);
                    }

                }

            }
            return trailersavailable;
        
        }

        public static SortableList<TrailerLocation> GetAllTrailersAssigned(DateTime scheduleDate)
        {
            return TrailerLocationDAL.GetAllTrailersAssigned(scheduleDate);
        }

        public static SortableList<TrailerLocation> GetAllTrailersForEditTptPlanDeptDate(DateTime scheduleDate)
        {
            SortableList<TrailerLocation> trailersavailable = new SortableList<TrailerLocation>();

            SortableList<TrailerLocation> a = new SortableList<TrailerLocation>();
            a = TrailerLocationDAL.GetAllTrailersForParticularDate(scheduleDate);

            if (scheduleDate.Date == DateTime.Today.Date)
            {
                trailersavailable = a;
            }
            else
            {
                SortableList<TrailerLocation> b = new SortableList<TrailerLocation>();
                b = GetAllTrailersAssigned(DateTime.Today.Date);
                //b = TrailerLocationDAL.GetAllTrailersForParticularDate(DateTime.Today.Date);
                for (int i = 0; i < a.Count; i++)
                {
                    bool find = false;
                    for (int j = 0; j < b.Count; j++)
                    {
                        if (a[i].trailerNo == b[j].trailerNo)
                        {
                            find = true;
                        }
                    }
                    if (find == false)
                    {
                        trailersavailable.Add(a[i]);
                    }

                }
            }
            return trailersavailable;
        }
        public static TrailerLocation PopulateComponentsAndControls(TrailerLocation trailerLocation, out SortableList<TrailerStatus> trailerStatusList, out string message)
        {
            message = "";
            trailerStatusList = new SortableList<TrailerStatus>();
            TrailerLocation temp = new TrailerLocation();
            switch (trailerLocation.trailerStatus)
            {
                case TrailerStatus.Assigned:
                    trailerStatusList.Add(TrailerStatus.TrailerPark);
                    trailerStatusList.Add(TrailerStatus.CustomerStuff);
                    trailerStatusList.Add(TrailerStatus.CustomerUnstuff);
                    temp.trailerNo = trailerLocation.trailerNo;
                    temp.trailerStatus = TrailerStatus.TrailerPark;
                    temp.startStop = trailerLocation.startStop;
                    temp.stopCode = trailerLocation.stopCode;
                    temp.parkStop = trailerLocation.stopCode;
                    temp.startTime = trailerLocation.endTime;
                    temp.DriverName = trailerLocation.DriverName;
                    temp.driverNo = trailerLocation.driverNo;
                    message = TptResourceBLL.WarnCantChangeStartTime;


                    break;

                case TrailerStatus.CompleteStuffUnstuff:
                    trailerStatusList.Add(TrailerStatus.Assigned);
                    temp.trailerNo = trailerLocation.trailerNo;
                    temp.trailerStatus = TrailerStatus.Assigned;
                    temp.startStop = trailerLocation.startStop;
                    temp.stopCode = trailerLocation.stopCode;
                    temp.startTime = trailerLocation.endTime;
                    temp.parkStop = string.Empty;
                    temp.DriverName = trailerLocation.DriverName;
                    temp.driverNo = trailerLocation.driverNo;
                    break;

                case TrailerStatus.CustomerStuff:
                    trailerStatusList.Add(TrailerStatus.CompleteStuffUnstuff);
                    temp.trailerNo = trailerLocation.trailerNo;
                    temp.trailerStatus = TrailerStatus.CompleteStuffUnstuff;
                    temp.startStop = trailerLocation.startStop;
                    temp.stopCode = trailerLocation.stopCode;
                    temp.parkStop = trailerLocation.stopCode;
                    temp.startTime = trailerLocation.endTime;
                    temp.DriverName = trailerLocation.DriverName;
                    temp.driverNo = trailerLocation.driverNo;
                    message = TptResourceBLL.WarnSetStartTime;
                    break;

                case TrailerStatus.CustomerUnstuff:
                    trailerStatusList.Add(TrailerStatus.CompleteStuffUnstuff);
                    temp.trailerNo = trailerLocation.trailerNo;
                    temp.trailerStatus = TrailerStatus.CompleteStuffUnstuff;
                    temp.startStop = trailerLocation.startStop;
                    temp.stopCode = trailerLocation.stopCode;
                    temp.parkStop = trailerLocation.stopCode;
                    temp.startTime = trailerLocation.endTime;
                    temp.DriverName = trailerLocation.DriverName;
                    temp.driverNo = trailerLocation.driverNo;
                    message = TptResourceBLL.WarnSetStartTime;
                    break;

                case TrailerStatus.TrailerPark:
                    trailerStatusList.Add(TrailerStatus.Assigned);
                    temp.trailerNo = trailerLocation.trailerNo;
                    temp.trailerStatus = TrailerStatus.Assigned;
                    temp.parkStop = string.Empty;
                    temp.startStop = trailerLocation.startStop;
                    temp.stopCode = trailerLocation.stopCode;
                    temp.DriverName = trailerLocation.DriverName;
                    temp.driverNo = trailerLocation.driverNo;
                    temp.startTime = trailerLocation.endTime;//.Date.AddMinutes(1);
                    break;
            }
            return temp;
        }
        public static SortableList<Vehicle> GetNot20FtTrailersOnly(SortableList<Vehicle> avlTrailer)
        {
            SortableList<Vehicle> retValue = new SortableList<Vehicle>();
            if (avlTrailer.Count > 0)
            {
                foreach (Vehicle vh in avlTrailer)
                {
                    string contCode = vh.TrailerType.ToString();
                    if (contCode.Substring(0, contCode.Length).Contains("4"))
                    {
                        retValue.Add(vh);
                    }
                }
            }              
            return retValue;
        }
        //20130517 - gerry added to get trailer location object from collection base on trailer number
        public static TrailerLocation GetTrailerLocationObject(SortableList<TrailerLocation> trailerLocations, string currentTrailerNo)
        {
            TrailerLocation trailerLocation = new TrailerLocation();
            foreach (TrailerLocation tempTrailerLocation in trailerLocations)
            {
                if (tempTrailerLocation.TrailerNo.Equals(currentTrailerNo))
                {
                    return tempTrailerLocation;
                }
            }
            return trailerLocation;
        }

        //20140519 - gerry added to get the previous trailer location to be use in planning for assigning sub contractor validation
        public static TrailerLocation GetPreviousLegTrailerLocationForPlanning(string planTripNo, string trailerNo, DateTime startTime)
        {
            return TrailerLocationDAL.GetPreviousLegTrailerLocationForPlanning(planTripNo, trailerNo, startTime);
        }
        //20150904 - gerry added to get all trailer in trailerPark for particular date
        public static SortableList<TrailerLocation> GetAllTrailerInParkForTheDay(DateTime scheduleDate)
        {
            return TrailerLocationDAL.GetAllTrailerInParkForTheDay(scheduleDate);
        }
        //20131123 - gerry added to get the affected trailer based on driver and plan date
        public static void GetAffectedTrailerLocations(ref SortableList<TrailerLocation> affectedTrailerLocations, SortableList<TrailerLocation> trailerLocations, string driverCode, DateTime startTime, string dept)
        {
            try
            {
                foreach (TrailerLocation tl in trailerLocations)
                {
                    if (tl.driverNo == driverCode && tl.startTime == startTime)
                    {
                        affectedTrailerLocations.Add(tl);
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        public static bool AddAffectedTrailerLocations(ref SortableList<TrailerLocation> affectedTrailerLocations, string tptDept, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                if (affectedTrailerLocations.Count > 0)
                {
                    string outMsg = string.Empty;
                    foreach (TrailerLocation tl in affectedTrailerLocations)
                    {
                        TrailerLocationDAL.DeleteTrailerLocations(tl.driverNo, tl.scheduleDate, tptDept, cn, tran);
                        TrailerLocation outTrailerLocation = null;
                        TrailerLocationDAL.AddTrailerLocation(tl, out outTrailerLocation, cn, tran, out outMsg);
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }
        public static bool DeleteTrailersForParticularDateAndDriver(string driverCode, DateTime scheduleDate, string tptDept, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                //TrailerLocationDAL.DeleteTrailerLocationByDriverAndStartTime(driverCode, startDateTime, tptDept, cn, tran);
                TrailerLocationDAL.DeleteTrailersForParticularDateAndDriver(driverCode, scheduleDate, tptDept, cn, tran);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }
        public static void AddAffectedTrailerLocationsForParticularDateAndDriver(ref SortableList<TrailerLocation> trailerLocations, string driverCode, DateTime scheduleDate, string tptDept, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                //delete from db
                DeleteTrailersForParticularDateAndDriver(driverCode, scheduleDate, tptDept, cn, tran);
                //get the affected trailer locations
                var affectedTrailerLocations = trailerLocations.Where(tl => tl.driverNo == driverCode && tl.scheduleDate.Date == scheduleDate.Date).ToList();
                foreach (TrailerLocation tl in affectedTrailerLocations)
                {
                    //TrailerLocationDAL.DeleteTrailersForParticularStartDateTimeAndDriver(tl.driverNo, tl.startTime, tptDept, cn, tran);
                    TrailerLocationDAL.AddTrailerLocations(tl, cn, tran);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        //20161007 - gerry added
        public static TrailerLocation GetLastTrailerLocation(string trailerNo)
        {
            return TrailerLocationDAL.GetLastTrailerLocation(trailerNo);
        }

        #region methods to fixed missing trailer if happen
        public static void UpdateData(DateTime dateStart, DateTime dateEnd)
        {
            List<TrailerLocation> tempTrailerLocations = new List<TrailerLocation>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //DateTime dateStart = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
                //DateTime dateEnd = new DateTime(dateStart.Year, dateStart.Month, DateTime.DaysInMonth(dateStart.Year, dateStart.Month));

                //Console.WriteLine("\nDeleting old Trailer Location data. Please wait...");
                if (tempTrailerLocations.Count == 0)
                    tempTrailerLocations = TrailerLocationDAL.GetAllTrailersBeingUsedInPlanning(dateStart, dateEnd, con, tran);
                //TODO delete all trailer locations which being use in planning
                int deletedCount = 0;
                TrailerLocationDAL.DeleteAllTrailerLocations(dateStart, dateEnd, con, tran, out deletedCount);
                //Console.WriteLine("\nTotal rows deleted : " + deletedCount.ToString());

                //Console.WriteLine("\nRewriting new Trailer Location data. Please wait...");
                foreach (TrailerLocation tl in tempTrailerLocations)
                {
                    if (!TrailerLocationDAL.IsTrailerLocationAlreadyInserted(tl.DriverNo, tl.StartTime, tl.TrailerNo, con, tran))
                    {
                        TrailerLocationDAL.InsertTrailerLocation(tl, con, tran);
                        //Console.Write("\nRow added : " + tl.TrailerNo.ToString() + " " + tl.StartTime.ToShortDateString());
                    }
                }
                //Console.Write("\nTotal rows added : " + tempTrailerLocations.Count.ToString());
                tran.Commit();
            }
            catch (SqlException SqlEx) { throw new FMException(SqlEx.Message.ToString()); }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally {  con.Close(); }
        }
        public static List<TrailerLocation> GetAllTrailersBeingUsedInPlanning(DateTime dateStart, DateTime dateEnd)
        {
            List<TrailerLocation> trailerlocations = new List<TrailerLocation>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                trailerlocations = TrailerLocationDAL.GetAllTrailersBeingUsedInPlanning(dateStart, dateEnd, con, tran);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { tran.Commit(); con.Close(); }
            return trailerlocations;
        }
        #endregion
    }
}
