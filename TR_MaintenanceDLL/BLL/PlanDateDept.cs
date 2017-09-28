using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_MaintenanceDLL.DAL;
using TR_LanguageResource.Resources;
//using FM.TransportPlanDLL.BLL;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class PlanDateDept
    {
        private DateTime scheduleDate;
        private string tptDeptCode;
        private SortableList<Driver> drivers;
        private SortableList<TrailerLocation> trailers;

        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
        }

        public string TptDeptCode
        {
            get { return tptDeptCode; }
            set { tptDeptCode = value; }
        }

        public SortableList<Driver> Drivers
        {
            get { return drivers; }
            set { drivers = value; }
        }

        public SortableList<TrailerLocation> Trailers
        {
            get { return trailers; }
            set { trailers = value; }
        }

        public PlanDateDept()
        {
            this.scheduleDate = DateTime.Today;
            this.tptDeptCode = "";
            this.drivers = new SortableList<Driver>();
            this.trailers = new SortableList<TrailerLocation>();
        }

        public PlanDateDept(DateTime scheduleDate, string tptDeptCode, SortableList<Driver> drivers, SortableList<TrailerLocation> trailers)
        {
            this.scheduleDate = scheduleDate;
            this.tptDeptCode = tptDeptCode;
            this.drivers = drivers;
            this.trailers = trailers;

        }

        public static SortableList<PlanDateDept> GetAllPlanDateDept(DateTime scheduleDate)
        {
            return PlanDateDeptDAL.GetAllPlanDateDept(scheduleDate);
        }

        public static SortableList<PlanDateDept> GetPlanDateAndDept(DateTime scheduleDate, string dept)
        {
            return PlanDateDeptDAL.GetPlanDateAndDept(scheduleDate, dept);
        }
        public static PlanDateDept GetPlanDateDept(DateTime scheduleDate,string tptDeptCode)
        {
            return PlanDateDeptDAL.GetPlanDateDept(scheduleDate, tptDeptCode);
        }


        public bool CanDeleteDriver(DateTime scheduleDate,Driver driver)
        {
            return true;
            //bool temp = true;
            //SortableList<PlanHaulierTrip> p = new SortableList<PlanHaulierTrip>();
            //p = PlanHaulierTrip.GetAllPlanHaulierTripsByDay(scheduleDate);
            
            //for (int i = 0; i < p.Count; i++)
            //{
            //    if (p[i].Driver.Code == driver.Code)
            //    {
            //        temp = false;
            //        break;
            //    }
            //}

            //return temp;
        }

        public bool IsDriverInUsed(DateTime scheduleDate, Driver driver, DeptType deptType)
        {
            return PlanDateDeptDAL.IsDriverUsed(scheduleDate, driver.Code, deptType);
        }

        public bool CanAddDriver(DateTime scheduleDate, Driver driver)
        {
            bool temp = true;
            try
            {
                //20140619 - gerry removed not neccessary codes
                //all available drivers are means not assigned in the particular date regradless the department, 
                //so only need check the license
                #region Replaced
                /*
                SortableList<PlanDateDept> p = new SortableList<PlanDateDept>();

                p = PlanDateDept.GetAllPlanDateDept(scheduleDate);

                for (int i = 0; i < p.Count; i++)
                {
                    for (int j = 0; j < p[i].drivers.Count; j++)
                    {
                        if (p[i].drivers[j].Code == driver.Code)
                        {
                            temp = false;
                            break;
                        }
                    }
                }
              */
                #endregion
                foreach (Driver drv in this.drivers)
                {
                    if (drv.Code == driver.Code)
                        throw new FMException(TptResourceBLL.ErrDriverExistAtSched);
                }

                //20140324 - gerry added validation for license
                if (driver.defaultVehicleNumber.Equals(string.Empty))
                    throw new FMException(TptResourceBLL.ErrDriverNoDefaultVehicle + TptResourceBLL.WarnSetupDriver);
                else
                {
                    Vehicle defaultVehicle = Vehicle.GetVehicle(driver.defaultVehicleNumber);
                    //check if vehicle is available
                    if (!defaultVehicle.IsAvailable)
                        throw new FMException(TptResourceBLL.ErrDriversVehicleIsNotAvailable + TptResourceBLL.WarnSetupDriver);
                    else //check license if exceed
                        Vehicle.HasExceedLicense(scheduleDate);
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return temp;
        }

        public bool CanDeleteTrailer(DateTime scheduleDate, TrailerLocation trailerNo)
        {
            bool temp = true;         
            SortableList<TrailerLocation> p = new SortableList<TrailerLocation>();
            //p = TrailerLocation.GetAllTrailerLocations(scheduleDate, this.tptDeptCode);
            p = TrailerLocation.GetAllTrailerLocationsForThisDayOnly(scheduleDate, this.tptDeptCode);

            for (int i = 0; i < p.Count; i++)
            {
                if (p[i].TrailerNo == trailerNo.TrailerNo)
                {
                    throw new FMException(TptResourceBLL.ErrCantDeleteTrailerUsedInPlanning);
                }
            }                       
            return temp;
        }

        public bool CanAddTrailer(DateTime scheduleDate, TrailerLocation trailer)
        {
            bool temp = true;
            #region 20130414 - gerry Removed and replaced
            /*
            SortableList<PlanDateDept> p = new SortableList<PlanDateDept>();
            p = GetAllPlanDateDept(scheduleDate);

            for (int i = 0; i < p.Count; i++)
            {
                for (int j = 0; j < p[i].trailers.Count; j++)
                {
                    if (p[i].trailers[j].TrailerNo == trailer.TrailerNo)
                    {
                        throw new FMException(TptResourceBLL.ErrCantAddTrailerUsedInPlanning);
                    }                
                }
            }
             */ 
            #endregion
            if (this.trailers.Count > 0)
            {
                for (int i = 0; i < this.trailers.Count; i++)
                {
                    if (this.trailers[i].TrailerNo == trailer.TrailerNo)
                    {
                        throw new FMException(TptResourceBLL.ErrCantAddTrailerUsedInPlanning);
                    }
                }
            }                         
            return temp;
        }

        public bool AddPlanDateDept()
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (!PlanDateDeptDAL.AddPlanDateDept(this, con, tran))
                {
                    throw new FMException(TptResourceDAL.ErrInsertFailed + TptResourceUI.PlanDept +"/"+ CommonResource.Date);
                }
                tran.Commit();
            }
            catch (FMException ex)
            {
                temp = false;
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                temp = false;
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            //return true;
            return temp;
        }
         
        public bool DeleteDriver(Driver driver, DeptType deptyType, string user, string frmName, FMModule fmModule)
        {
            bool temp = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            SqlTransaction tran = cn.BeginTransaction();
            if (!IsDriverInUsed(scheduleDate, driver, deptyType))
            {
                try
                {
                    if (!PlanDateDeptDAL.DeleteDriver(this, driver, cn, tran))
                    {
                        throw new FMException(TptResourceDAL.ErrDeleteFailed + CommonResource.Driver);
                    }

                    //audit log
                    AuditLog auditLog = new AuditLog(driver.Code, "TR", "PL", 0, user, DateTime.Now, frmName, 0, FormMode.Delete.ToString());
                    auditLog.WriteAuditLog(string.Empty, string.Empty, driver.Code + " " + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(this.scheduleDate), cn, tran);

                    temp = true;
                    this.drivers.Remove(driver);

                    tran.Commit();
                }
                catch (FMException ex)
                {
                    temp = false;
                    tran.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    temp = false;
                    tran.Rollback();
                    throw ex;
                }
                finally { if(cn.State == ConnectionState.Open){ cn.Close();} }
            }
            else
            {
                throw new FMException(TptResourceBLL.ErrCantDeleteDriverInUsed);
            }
            //return true;
            return temp;
        }

        public bool AddDriver(Driver driver, string user, string frmName, FMModule fmModule)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if(this.CanAddDriver(this.scheduleDate, driver))
                {
                   temp = PlanDateDeptDAL.AddDriverToPlanDateDept(this, driver, con, tran);

                   //audit log
                   AuditLog auditLog = new AuditLog(driver.Code, "TR", "PL", 0, user, DateTime.Now, frmName, 0, FormMode.Add.ToString());
                   auditLog.WriteAuditLog("DriverCode", driver.Code, string.Empty, con, tran);
                   auditLog.WriteAuditLog("ScheduleDate", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(this.scheduleDate), string.Empty, con, tran);

                   this.drivers.Add(driver);
                }
                temp = true;

                tran.Commit();
            }
            catch (FMException ex)
            {
                temp = false;
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
            //return true;
            return temp;
        }

        public bool AddTrailer(TrailerLocation selectTrailer, string user, string frmName, FMModule fmModule)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (CanAddTrailer(this.scheduleDate, selectTrailer))
                {
                    temp = PlanDateDeptDAL.AddVehicleToPlanDateDept(this, selectTrailer, con, tran);

                    //audit log
                    AuditLog auditLog = new AuditLog(selectTrailer.TrailerNo, "TR", "PL", 0, user, DateTime.Now, frmName, 0, FormMode.Add.ToString());
                    auditLog.WriteAuditLog("TrailerNo", selectTrailer.TrailerNo, string.Empty, con, tran);
                    auditLog.WriteAuditLog("ScheduleDate", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(this.scheduleDate), string.Empty, con, tran);

                    this.trailers.Add(selectTrailer);                    
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
            finally
            {
                con.Close();
            }
            return temp;
        }
        public bool DeleteTrailer(TrailerLocation trailer, string user, string frmName, FMModule fmModule)
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                if (CanDeleteTrailer(scheduleDate, trailer))
                {
                    if (PlanDateDeptDAL.DeleteTrailer(this, trailer, cn, tran))
                    {

                        //audit log
                        AuditLog auditLog = new AuditLog(trailer.TrailerNo, "TR", "PL", 0, user, DateTime.Now, frmName, 0, FormMode.Delete.ToString());
                        auditLog.WriteAuditLog(string.Empty, string.Empty, trailer.TrailerNo +" "+ DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(this.scheduleDate), cn, tran);

                        retValue = true;
                        this.Trailers.Remove(trailer);
                        tran.Commit();
                    }
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
                throw new FMException(TptResourceDAL.ErrDeleteFailed + TptResourceUI.Trailer + " \n" + ex.ToString());
            }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return retValue;
        }
        public static SortableList<Driver> GetDriversForPlanDateDept(DateTime scheduleDate, string dept)
        {
            return PlanDateDeptDAL.GetDriversForPlanDateDeptForHaulage(scheduleDate, dept);
        }
        public static SortableList<Driver> GetDriversForPlanDateDeptForTrucking(DateTime scheduleDate, string dept)
        {
            return PlanDateDeptDAL.GetDriversForPlanDateDeptForTrucking(scheduleDate, dept);
        }

        public static SortableList<TrailerLocation> GetTrailersForPlanDateDept(DateTime scheduleDate, string dept)
        {
            return PlanDateDeptDAL.GetTrailersForPlanDateDept(scheduleDate, dept);
        }

        public static List<string> GetJobNosForPlanDateDept(string driver, DateTime scheduleDate)
        {
            return PlanDateDeptDAL.GetJobNosForPlanDateDept(driver, scheduleDate);
        }

        public bool AddAllTrailer(SortableList<TrailerLocation> unAssignedTrailers, string user, string frmName, FMModule fmModule)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (unAssignedTrailers.Count > 0)
                {
                    //for (int i = 0; i < unAssignedTrailers.Count; i++)
                    foreach(TrailerLocation tempTrailer in unAssignedTrailers)
                    {
                        if (CanAddTrailer(this.scheduleDate, tempTrailer))
                        {
                            temp = PlanDateDeptDAL.AddVehicleToPlanDateDept(this, tempTrailer, con, tran);
                            this.trailers.Add(tempTrailer);
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
            finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            return temp;
        }
        //20140619 = gerry added to create PlanDateDept for auto select available drivers and trailers option
        public static bool CreatePlanDate(DateTime scheduleDate, string dept, string user, string frmName)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                SortableList<Driver> availableDrivers = Driver.GetAllAvailableDriversForParticularDate(scheduleDate);
                //201904 - gerry modified to get available trailer in trailerpark for the day
                SortableList<TrailerLocation> availableTrailerLocations = TrailerLocation.GetAllTrailerInParkForTheDay(scheduleDate); //TrailerLocation.GetTrailersForNewTrailerLocationStatus(scheduleDate);
                if (!Vehicle.HasExceedLicenseForTotalVehicles(scheduleDate, availableDrivers.Count))
                {   
                    PlanDateDept planDateDept = new PlanDateDept();
                    planDateDept.scheduleDate = scheduleDate;
                    planDateDept.tptDeptCode = dept;  
                    //20140702 - gerry added validation if plan Date exist
                    if (PlanDateDeptDAL.IsPlanDateDeptExist(planDateDept))
                    {                                                       
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        if (tran == null) { tran = con.BeginTransaction(); }
                        if (GetDriversForPlanDateDept(scheduleDate, dept).Count == 0)
                            AddAllAvailableDrivers(planDateDept, availableDrivers, frmName, user, con, tran); //add all available drivers                           
                        if (GetTrailersForPlanDateDept(scheduleDate, dept).Count == 0)
                            AddAllAvailableTrailers(planDateDept, availableTrailerLocations, frmName, user, con, tran);  //add all available trailers
                    }
                    else
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        if (tran == null) { tran = con.BeginTransaction(); }
                        //add PlanDateDept              
                        PlanDateDeptDAL.AddPlanDateDept(planDateDept, con, tran);
                        AddAllAvailableDrivers(planDateDept, availableDrivers, frmName, user, con, tran); //add all available drivers  
                        AddAllAvailableTrailers(planDateDept, availableTrailerLocations, frmName, user, con, tran);  //add all available trailers
                    }
                    if (tran != null) { tran.Commit(); }
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
            finally { if (con.State == ConnectionState.Open) { con.Close(); } }   
            return true;
        }
        //20140619 = gerry added to create PlanDateDept for auto select available drivers and trailers option
        public static bool CreatePlanDateForTrucking(DateTime scheduleDate, string dept, string user, string frmName)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    SortableList<Driver> availableDrivers = Driver.GetAllAvailableDriversForParticularDateForTrucking(scheduleDate);
                    if(availableDrivers.Count == 0)
                        throw new FMException("No Driver with Truck is available. Please go to Booking > Setup > Driver then assign a available driver to available truck. ");
                    
                    if (!Vehicle.HasExceedLicenseForTotalVehicles(scheduleDate, availableDrivers.Count))
                    {
                        PlanDateDept planDateDept = new PlanDateDept();
                        planDateDept.scheduleDate = scheduleDate;
                        planDateDept.tptDeptCode = dept;
                        //20140702 - gerry added validation if plan Date exist
                        if (PlanDateDeptDAL.IsPlanDateDeptExist(planDateDept))
                        {
                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            if (tran == null) { tran = con.BeginTransaction(); }
                            if (GetDriversForPlanDateDept(scheduleDate, dept).Count == 0)
                                AddAllAvailableDrivers(planDateDept, availableDrivers, frmName, user, con, tran); //add all available drivers              
                        }
                        else
                        {
                            if (con.State == ConnectionState.Closed) { con.Open(); }
                            if (tran == null) { tran = con.BeginTransaction(); }
                            //add PlanDateDept              
                            PlanDateDeptDAL.AddPlanDateDept(planDateDept, con, tran);
                            AddAllAvailableDrivers(planDateDept, availableDrivers, frmName, user, con, tran); //add all available drivers  
                        }
                        if (tran != null) { tran.Commit(); }
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
                finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            }
            return true;
        }
        //20140624 - gerry added to delete TPT_PLAN_DEPT_TBL and TPT_PLAN_DEPT_DRIVER_TBL and TPT_PLAN_DEPT_VEHICLE_TBL 
        //which added during creation of planDateDept auto selectdrivers and trailers
        public static void DeletePlanDeptDriversTrailersForScheduleDate(DateTime scheduleDate, string deptCode)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                PlanDateDeptDAL.DeletePlanDeptDriversTrailersForScheduleDate(scheduleDate, deptCode, con, tran);
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
            finally { if (con.State == ConnectionState.Open) { con.Close(); } }
        }
        public static void AddAllAvailableDrivers(PlanDateDept planDateDept, SortableList<Driver> availableDrivers, string frmName, string userID, SqlConnection con, SqlTransaction tran)
        {

            try
            {//add all available drivers
                foreach (Driver drv in availableDrivers)
                {   //1. use instance to call non static methods
                    //2. validation is done in license, no need to validate 1 by 1
                    if(!drv.defaultVehicleNumber.Equals(string.Empty))
                        PlanDateDeptDAL.AddDriverToPlanDateDept(planDateDept, drv, con, tran);

                    //audit log
                    AuditLog auditLog = new AuditLog(drv.Code, "TR", "PL", 0, userID, DateTime.Now, frmName, 0, FormMode.Add.ToString());
                    auditLog.WriteAuditLog("DriverCode", drv.Code, string.Empty, con, tran);
                    auditLog.WriteAuditLog("ScheduleDate",DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planDateDept.scheduleDate), string.Empty, con, tran);
                    planDateDept.drivers.Add(drv);
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        public static void AddAllAvailableTrailers(PlanDateDept planDateDept, SortableList<TrailerLocation> availableTrailerLocations, string frmName, string userID, SqlConnection con, SqlTransaction tran)
        {   //add all available trailers
            try
            {
                foreach (TrailerLocation trailerLocation in availableTrailerLocations)
                {   //use instance to call non static methods
                    if (planDateDept.CanAddTrailer(planDateDept.scheduleDate, trailerLocation))
                    {
                        PlanDateDeptDAL.AddVehicleToPlanDateDept(planDateDept, trailerLocation, con, tran);

                        //audit log
                        AuditLog auditLog = new AuditLog(trailerLocation.TrailerNo, "TR", "PL", 0, userID, DateTime.Now, frmName, 0, FormMode.Add.ToString());
                        auditLog.WriteAuditLog("TrailerNo", trailerLocation.TrailerNo, string.Empty, con, tran);
                        auditLog.WriteAuditLog("ScheduleDate", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planDateDept.scheduleDate), string.Empty, con, tran);
                        planDateDept.trailers.Add(trailerLocation);
                    }
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        public static SortableList<JobAssignmentPriority> GetAllDriversForTheDayByPriority(DateTime scheduleDate)
        {
            return null;
        }
        public static Driver GetDriverWithPriority(string driverCode, DateTime scheduleDate)
        {
            return PlanDateDeptDAL.GetDriverWithPriority(driverCode, scheduleDate);
        }

    }
}
