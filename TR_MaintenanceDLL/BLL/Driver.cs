using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_MaintenanceDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using System.Data;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class Driver
    {
        private string code;
        private string name;
        public string nirc { get; set; }
        public string nationality { get; set; }
        private string drivingLicence;
        private string drivingClass;
        public DateTime licenceExpiryDate { get; set; }
        public string defaultVehicleNumber { get; set; }
        private EmployeeStatus employeeStatus;  
        public bool isAvailable { get; set; }
        public string DescriptionForPlanningPurpose { get; set; }
        //20140626 - gerry added
        public SortableList<DriverUnavailable> unAvailableDates { get; set; }
        public int priority { get; set; }
        public string unAvailableDisplayTime { get; set; }

        public Driver()
        {
            this.code = "";
            this.name = "";
            this.nirc = "";
            this.nationality = "";
            this.drivingLicence = "";
            this.drivingClass = "";
            this.licenceExpiryDate = DateTime.Today;
            this.defaultVehicleNumber = "";
            this.isAvailable = true;
            this.DescriptionForPlanningPurpose = "";
            this.employeeStatus = EmployeeStatus.Available;
            this.unAvailableDates = new SortableList<DriverUnavailable>(); //20140626 gerry added
            this.unAvailableDisplayTime = string.Empty;
        }

        public Driver(string code, string name, string nirc, string nationality, string drivingLicence,
           string drivingClass, DateTime licenceExpiryDate, string defaultVehicleNumber,
            bool isAvailable, EmployeeStatus employeeStatus)
        {
            this.Code = code;
            this.Name = name;
            this.nirc = nirc;
            this.nationality = nationality;
            this.DrivingLicence = drivingLicence;
            this.DrivingClass = drivingClass;
            this.licenceExpiryDate = licenceExpiryDate;
            this.defaultVehicleNumber = defaultVehicleNumber;
            this.isAvailable = isAvailable;
            this.DescriptionForPlanningPurpose = name + " (" + (defaultVehicleNumber == string.Empty ? "None" : defaultVehicleNumber) + ")";
            this.EmployeeStatus = employeeStatus;
            this.unAvailableDisplayTime = string.Empty;
            this.unAvailableDates = new SortableList<DriverUnavailable>(); //20140626 gerry added
        }

        public string Code
        {
            get { return code; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrDriverCode);
                }
                else
                {
                    code = value;
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrDriverName);
                }
                else
                {
                    name = value;
                }
            }
        }

        public string DrivingLicence
        {
            get { return drivingLicence; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrDriverLicence);
                }
                else
                {
                    drivingLicence = value;
                }
            }
        }

        public string DrivingClass
        {
            get { return drivingClass; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrDrivingClass);
                }
                else
                {
                    drivingClass = value;
                }
            }
        }

        public EmployeeStatus EmployeeStatus
        {
            get { return employeeStatus;  }
            set
            {
                employeeStatus = value;  
            }
        }

        public static SortableList<Driver> GetAllDrivers()
        {
            return DriverDAL.GetAllDrivers();
        }

        public static SortableList<Driver> GetAllAvailableDrivers()
        {
            return DriverDAL.GetAllAvailableDrivers();
        }

        public static SortableList<Driver> GetAllAvailableDriversForParticularDate(DateTime scheduleDate)
        {
            return DriverDAL.GetAllAvailableDriversForParticularDateForHaulage(scheduleDate);
        }
        //20160226 - gerry added
        public static SortableList<Driver> GetAllAvailableDriversForParticularDateForTrucking(DateTime scheduleDate)
        {
            return DriverDAL.GetAllAvailableDriversForParticularDateForTrucking(scheduleDate);
        }
        public static SortableList<Driver> GetDrivers(string driverCode)
        {
            return DriverDAL.GetDrivers(driverCode);
        }

        public static Driver GetDriver(string driverCode)
        {
            return DriverDAL.GetDriver(driverCode);
        }
        //20170105
        public static Driver GetDriverByVehicle(string vehicleNo)
        {
            return DriverDAL.GetDriverByVehicle(vehicleNo);
        }
        public static string GetDriverName(string driverCode)
        {
            return DriverDAL.GetDriverName(driverCode);
        }

        public bool AddDriver(Driver driver)
        {
            bool isAddedDriver = true;
            bool isVehicleUsed = false;
            try
            {
                if (this.VerifyExixtingDriver(driver.Code) == true)
                {
                    isAddedDriver = true;
                }
                else
                {
                    throw new FMException(TptResourceBLL.ErrDriveCodeExist);  
                }

                if (this.IsVehicleAssigned(driver.defaultVehicleNumber) == true)
                {
                    isVehicleUsed = true;
                    throw new FMException(TptResourceBLL.ErrDriverNoDefaultVehicle);
                }
                else
                {
                    isVehicleUsed = false;
                }
                if ((isAddedDriver) & (isVehicleUsed == false))
                {
                    DriverDAL.AddDriver(driver);
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
        public bool AddDriver(Driver driver, string formName, string userID)
        {
            bool isVehicleUsed = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (!this.VerifyExixtingDriver(driver.Code))
                    throw new FMException(TptResourceBLL.ErrDriveCodeExist);

                if (this.IsVehicleAssigned(driver.defaultVehicleNumber))
                {
                    isVehicleUsed = true;
                    throw new FMException(TptResourceBLL.ErrDriverNoDefaultVehicle);
                }
                if (!isVehicleUsed)
                {
                    DriverDAL.AddDriver(driver, con, tran);
                }
                //audit log
                AuditLog auditLog = new AuditLog(driver.code.ToString(), "TR", "SETUP", 0, userID, DateTime.UtcNow, "DRIVER_SETUP", 0, FormMode.Add.ToString());
                auditLog.WriteAuditLog(driver, null, con, tran);

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
                throw new FMException(ex.Message.ToString());
            }
            return true;
        }
        public bool DeleteDriver(string driverCode, string formName, string userID)
        {
            bool isDeleteDriver = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (this.ValidateDeleteDriver(driverCode))
                    throw new FMException(TptResourceBLL.ErrDriverExistAtSched);
                //if (this.IsDriverHasAssignedPriority(driverCode))
                //    throw new FMException("Can't delete driver with assigned priority. ");

                isDeleteDriver = DriverDAL.DeleteDriver(driverCode, con, tran);

                //audit log
                AuditLog auditLog = new AuditLog(driverCode, "TR", "SETUP", 0, userID, DateTime.UtcNow, "DRIVER_SETUP", 0, FormMode.Delete.ToString());
                auditLog.WriteAuditLog(null, null, con, tran);

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
                throw ex;
            }
            return isDeleteDriver;

            //return DriverDAL.DeleteDriver(driverCode);
        }
        public bool DeleteDriver(string driverCode)
        {
            bool isDeleteDriver = false;
            try
            {
                if (this.ValidateDeleteDriver(driverCode))
                    throw new FMException(TptResourceBLL.ErrDriverExistAtSched);
                //if (this.IsDriverHasAssignedPriority(driverCode))
                //    throw new FMException("Can't delete driver with assigned priority. ");

                isDeleteDriver = DriverDAL.DeleteDriver(driverCode);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleteDriver;

            //return DriverDAL.DeleteDriver(driverCode);
        }
        public static bool EditDriver(Driver driver, string formName, string userID)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Driver oldDriver = DriverDAL.GetDriver(driver.code, con, tran);
                if (driver.IsVehicleAssignedForEditMode(driver.defaultVehicleNumber, driver.code) == true)
                {
                    throw new FMException(TptResourceBLL.ErrDriverNoDefaultVehicle);
                }
                DriverDAL.EditDriver(driver, con, tran);
                //audit log
                AuditLog auditLog = new AuditLog(driver.code.ToString(), "TR", "SETUP", 0, userID, DateTime.Now, "DRIVER_SETUP", 0, FormMode.Edit.ToString());
                auditLog.WriteAuditLog(driver, oldDriver, con, tran);

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
                throw ex;
            }
            return true;
        }
        public static bool EditDriver(Driver driver)
        {
            bool retValue = true;

            try
            {
                if (driver.IsVehicleAssignedForEditMode(driver.defaultVehicleNumber, driver.code) == true)
                {
                    retValue = false;
                    throw new FMException(TptResourceBLL.ErrDriverNoDefaultVehicle);
                }
                else
                {
                    DriverDAL.EditDriver(driver);
                }
            }
            catch (FMException ex)
            {
                retValue =false;
                throw ex;
            }
            catch (Exception ex)
            {
                retValue = false;
                throw ex;
            }

            return retValue;
        }

        public bool VerifyExixtingDriver(string driverCode)
        {
            bool isExistingDriver = false;
            try
            {
                isExistingDriver = DriverDAL.VerifyExixtingDriver(driverCode.Trim());
            }
            catch (FMException ex)
            {
                throw ex; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isExistingDriver; 
        }

        public static SortableList<Driver> GetAllDriverThatNotInBothTruckAndHaulierPlan(DateTime date)
        {
            return DriverDAL.GetAllDriverThatNotInBothTruckAndHaulierPlan(date);
        }

        public static SortableList<Driver> GetAllDriverThatNotInThePlan(DateTime date)
        {
            return DriverDAL.GetAllDriverThatNotInThePlan(date);
        }

        public bool ValidateDeleteDriver(string driverCode)
        {
            bool isAllowDeleteDriver = false; 
            try
            {
                isAllowDeleteDriver = DriverDAL.IsJobTripUsedByDriver(driverCode);                 
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAllowDeleteDriver; 
        }

        public bool IsVehicleAssigned(string vehicleno)
        {
            bool isok = false;
            try
            {
                if (!vehicleno.Equals(string.Empty))
                {
                    isok = DriverDAL.IsVehicleAssigned(vehicleno);
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

            return isok;
        }

        public bool IsVehicleAssignedForEditMode(string vehicleno, string drcode)
        {
            bool isok = false;
            try
            {
                if (!vehicleno.Equals(string.Empty))
                {
                    isok = DriverDAL.IsVehicleAssignedForEditMode(vehicleno, drcode);
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

            return isok;
        }

        public static List<Driver> GetDriversInPlanning(DateTime scheduleDate, string deptCode)
        {
            return DriverDAL.GetDriversInPlanning(scheduleDate, deptCode);
        }

        #region "2014-05-14 Zhou Kai adds functions"
        /// <summary>
        /// Get all drivers those who:
        /// (1) are in tpt_plan_dept_driver_tbl but not has planTruckTrips yet, OR
        /// (2) have planTruckTrips but don't have planSubTrips
        /// </summary>
        /// <param name="date">That "certain date"</param>
        /// <returns>The list of those drivers</returns>
        public static List<Driver> GetUnAllocatedDriverForDate(DateTime date)
        {
            return DriverDAL.GetUnAllocatedDriverForDate(date);
        }

        /// <summary>
        /// 2014-05-13 Zhou Kai writes this function to get all the drivers that
        /// are in TPT_PLAN_DEPT_DRIVER_TBL but not in TRK_PLAN_TRIP_TBL
        /// nor TPT_PLAN_TRIP_TBL on a ceratin date 
        /// </summary>
        /// <param name="date">It's the "certain date"</param>
        /// <returns>The list of those drivers</returns>
        internal static List<Driver> GetAllDriversInScheduleButHasNoPlanTripsByDate(DateTime date)
        {
            return DriverDAL.GetAllDriversInScheduleButHasNoPlanTripsByDate(date);
        }

        /// <summary>
        /// 2014-05-15 Zhou Kai writes this function to get all the drivers those
        /// are in TRK_PLAN_TRIP_TBL but have no planTruckSubTrips
        /// on a certain date.
        /// </summary>
        /// <param name="date">That "certain date"</param>
        /// <returns>A list of those drivers</returns>
        internal static List<Driver> GetAllDriversInScheduleButHasNoPlanSubTripsByDate(DateTime date)
        {
            return DriverDAL.GetAllDriversInScheduleButHasNoPlanSubTripsByDate(date);
        }

        #endregion

        //20140625 - gerry added to get unavailable dates for drivers based on schedule date
        public SortableList<DriverUnavailable> GetDriverUnavailableDates(DateTime scheduleDate, SqlConnection con, SqlTransaction tran)
        {
            return DriverDAL.GetDriverUnavailableDates(this.code, scheduleDate,con,tran);
        }
        //20140625 - gerry added to get unavailable drivers from the range
        public SortableList<DriverUnavailable> GetDriverUnavailableDates(DateTime dateStartFrom, DateTime dateStartTo)
        {
            return DriverDAL.GetDriverUnavailableDates(this.code, dateStartFrom, dateStartTo); 
        }
        public DriverUnavailable GetDriverUnavailableDate(DriverUnavailable driverUnavailable)
        {
            return DriverDAL.GetDriverUnavailableDate(this.code, driverUnavailable.seqNo);
        }
        public bool DoDatesOverlapped(DriverUnavailable driverUnavailable)
        {
            return DriverDAL.DoDatesOverlapped(driverUnavailable);
        }
        public bool ValidateAddDriverUnavailable(DriverUnavailable driverUnavailable)
        {
            //20140707 - gerry modified to validate from SQL instead of object collections 
            if (!DoDatesOverlapped(driverUnavailable))
            {
                if (driverUnavailable.startDateTime > driverUnavailable.endDateTime)
                    throw new FMException(TptResourceBLL.ErrUnavailableStartDateTimeLaterThanEndDateTim);

            }
            return true;
        }
        public bool ValidateEditDriverUnavailable(DriverUnavailable driverUnavailable)
        {
            //20140707 - gerry modified to validate from SQL instead of object collections 
            if (!DoDatesOverlapped(driverUnavailable))
            {
                if (driverUnavailable.startDateTime > driverUnavailable.endDateTime)
                    throw new FMException(TptResourceBLL.ErrUnavailableStartDateTimeLaterThanEndDateTim); 
            }
            return true;
        }  
        public bool ValidateDeleteDriverUnavailable(DriverUnavailable driverUnavailable)
        {
            if (driverUnavailable.startDateTime < DateTime.Today)
                throw new FMException(TptResourceBLL.ErrCantDeleteUnavailableDateTimeEarlyThanToday);
            return true;
        }   
        public bool AddDriverUnavailable(DriverUnavailable driverUnavailable)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (ValidateAddDriverUnavailable(driverUnavailable))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    DriverDAL.AddDriverUnavailable(driverUnavailable,con,tran);
                    this.unAvailableDates.Add(driverUnavailable);

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
                throw ex;
            }
            finally { con.Close(); }
            return true;         
        }
        public bool EditDriverUnavailable(DriverUnavailable driverUnavailable)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (ValidateEditDriverUnavailable(driverUnavailable))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    DriverDAL.EditDriverUnavailable(driverUnavailable, con, tran);

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
                throw ex;
            }
            finally { con.Close(); }
            return true;
        }
        public bool DeleteDriverUnavailable(DriverUnavailable driverUnavailable)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (ValidateDeleteDriverUnavailable(driverUnavailable))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    DriverDAL.DeleteDriverUnavailable(driverUnavailable, con, tran);

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
                throw ex;
            }
            finally { con.Close(); }
            return true;
        }

        public static SortableList<Driver> GetDriversUnavailableForTheDay(DateTime scheduleDate)
        {
            return DriverDAL.GetDriversUnavailableForTheDay(scheduleDate);
        }
        public bool IsDriverHasAssignedPriority(string driverCode)
        {
            return DriverDAL.IsDriverHasAssignedPriority(driverCode);
        }
        public static bool IsDriverAssignJob(string DriverCode, DateTime ScheduleDate, DateTime StartTime, DateTime EndTime)
        {
            return DriverDAL.IsDriverAssignJob(DriverCode, ScheduleDate, StartTime, EndTime);
        }
    }
}
