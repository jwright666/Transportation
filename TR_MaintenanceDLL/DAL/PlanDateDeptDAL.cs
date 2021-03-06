﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
//using FM.TransportPlanDLL.BLL;

namespace FM.TR_MaintenanceDLL.DAL
{
    public class PlanDateDeptDAL
    {

        internal static SortableList<PlanDateDept> GetAllPlanDateDept(DateTime scheduleDate)
        {
            SortableList<PlanDateDept> planDateDepts = new SortableList<PlanDateDept>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_PLAN_DEPT_TBL";
            SQLString += " WHERE SCHEDULE_DATE='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                planDateDepts.Add(GetPlanDateDept(reader));
            }
            reader.Close();

            for (int i = 0; i < planDateDepts.Count; i++)
            {
                planDateDepts[i].Drivers = GetDriversForPlanDateDeptForHaulage(scheduleDate, planDateDepts[i].TptDeptCode);
                planDateDepts[i].Trailers = GetTrailersForPlanDateDept(scheduleDate, planDateDepts[i].TptDeptCode);
            }

            return planDateDepts;
        }

        internal static SortableList<PlanDateDept> GetPlanDateAndDept(DateTime scheduleDate, string dept)
        {
            SortableList<PlanDateDept> planDateDepts = new SortableList<PlanDateDept>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT * FROM TPT_PLAN_DEPT_TBL";
            SQLString += " WHERE SCHEDULE_DATE='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";

            if (!dept.Equals(string.Empty))
            {
                SQLString += " AND TPT_DEPT_CODE='" + dept + "'";
            }

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                planDateDepts.Add(GetPlanDateDept(reader));
            }
            cn.Close();
            for (int i = 0; i < planDateDepts.Count; i++)
            {
                planDateDepts[i].Drivers = GetDriversForPlanDateDeptForHaulage(scheduleDate, planDateDepts[i].TptDeptCode);
                planDateDepts[i].Trailers = GetTrailersForPlanDateDept(scheduleDate, planDateDepts[i].TptDeptCode);
            }
            return planDateDepts;
        }

        internal static PlanDateDept GetPlanDateDept(DateTime scheduleDate, string tptDeptCode)
        {
            PlanDateDept planDateDept = new PlanDateDept();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_PLAN_DEPT_TBL";
            SQLString += " WHERE SCHEDULE_DATE='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
            SQLString += " AND TPT_DEPT_CODE='" + tptDeptCode + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                planDateDept = GetPlanDateDept(reader);
            }
            cn.Close();


            planDateDept.Drivers = GetDriversForPlanDateDeptForHaulage(scheduleDate, planDateDept.TptDeptCode);
            planDateDept.Trailers = GetTrailersForPlanDateDept(scheduleDate, planDateDept.TptDeptCode);

            return planDateDept;
        }

        internal static PlanDateDept GetPlanDateDept(IDataReader reader)
        {
            PlanDateDept planDateDept = new PlanDateDept(
                    (DateTime)reader["SCHEDULE_DATE"],
                    (string)reader["TPT_DEPT_CODE"],
                    new SortableList<Driver>(),
                    new SortableList<TrailerLocation>()
                    );
            return planDateDept;
        }

   
        public static SortableList<TrailerLocation> GetTrailersForPlanDateDept(DateTime scheduleDate, string dept)
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();

            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_PLAN_DEPT_VEHICLE_TBL";
            SQLString += " inner join TPT_Vehicle_Tbl";
            //SQLString += " inner join TPT_TRAILER_LOCATION_TBL";
            SQLString += " on TPT_PLAN_DEPT_VEHICLE_TBL.Vehicle_No=TPT_Vehicle_Tbl.Vehicle_No";
            SQLString += " WHERE SCHEDULE_DATE='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
            SQLString += " AND TPT_DEPT_CODE='" + CommonUtilities.FormatString(dept) + "'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vehicles.Add(GetVehicles(reader));
            }
            reader.Close();
            SortableList<TrailerLocation> trailers = new SortableList<TrailerLocation>();
            for (int i = 0; i < vehicles.Count; i++)
            {
                SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                SQLString += " inner join TPT_Vehicle_Tbl";
                SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_Vehicle_Tbl.Vehicle_No";
                SQLString += " Where TPT_TRAILER_LOCATION_TBL.TrailerNo = '{0}'";
                SQLString += " ORDER BY StartTime Desc";

                SQLString = string.Format(SQLString, CommonUtilities.FormatString(vehicles[i].Number));
                cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); };
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TrailerLocation temp = GetTrailerLocation(reader);
                    temp.TrailerType = (string)reader["Trailer_Type"];
                    temp.MaximumLadenWeight = (decimal)reader["Max_Weight"];
                    trailers.Add(temp);
                }
                reader.Close();
            }
            return trailers;
        }

        internal static Driver GetDriver(IDataReader reader)
        {
            bool isavailable = true;
            EmployeeStatus driverEmployeeStatus;
            string temp = (string)reader["Is_Available"];
            if (temp == "F")
            {
                isavailable = false;
            }

            string tempEmployeeStatus = (string)reader["Employ_Status"];
            if (tempEmployeeStatus == "A")
            {
                driverEmployeeStatus = EmployeeStatus.Available;
            }
            else
            {
                driverEmployeeStatus = EmployeeStatus.Resigned;
            }
            Driver driver = new Driver(
                (string)reader["Driver_Code"],
                (string)reader["Driver_Name"],
                (string)reader["NRIC_No"],
                (string)reader["Nationality"],
                (string)reader["Driving_Licence_No"],
                (string)reader["Driving_Class"],
                (DateTime)reader["Licence_Expiry_Date"],
                (string)reader["Default_Vehicle"],
                isavailable,
                driverEmployeeStatus
                );
            return driver;
        }

        internal static Vehicle GetVehicles(IDataReader reader)
        {
            bool isAvaiable;
            if ((string)reader["Is_Available"] == "T")
            {
                isAvaiable = true;
            }
            else
            {
                isAvaiable = false;
            }

            Vehicle vehicle = new Vehicle(
                    (VehicleType)reader["Vehicle_Type"],
                    (string)reader["Make"],
                    (string)reader["Model"],
                    (string)reader["Vehicle_No"],
                    (decimal)reader["Max_weight"],
                    isAvaiable,
                    (decimal)reader["Tare_Weight"],
                    (string)reader["Trailer_Type"],
                    (string)reader["Default_Stop"],
                    (string)reader["Remarks"],
                    (byte[])reader["Update_Version"],
                    (decimal)reader["Vol_Capacity"]);
            return vehicle;
        }

        internal static TrailerLocation GetTrailerLocation(IDataReader reader)
        {
            TrailerStatus t = new TrailerStatus();
            int status = (int)reader["TrailerStatus"];
            if (status == 1) { t = TrailerStatus.Assigned; }
            if (status == 2) { t = TrailerStatus.CustomerStuff; }
            if (status == 3) { t = TrailerStatus.CustomerUnstuff; }
            if (status == 4) { t = TrailerStatus.TrailerPark; }
            if (status == 5) { t = TrailerStatus.Unavailable; }
            if (status == 6) { t = TrailerStatus.CompleteStuffUnstuff; }

            TrailerLocation trailerlocation = new TrailerLocation(
                    (string)reader["TrailerNo"],
                    (DateTime)reader["ScheduleDate"],
                    (DateTime)reader["DateChange"],
                    (DateTime)reader["StartTime"],
                    (DateTime)reader["EndTime"],
                    t,
                    (string)reader["StartStop"],
                    (string)reader["StopCode"],
                    (string)reader["ParkStopCode"],
                    (string)reader["PlanTripNo"],
                    (string)reader["DriverNo"],
                    (string)reader["Remarks"],
                    (byte[])reader["UpdateVersion"]
                    );

            return trailerlocation;
        }

        internal static bool DeleteDriver(PlanDateDept p, Driver driver, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string sqlDeleteString = " Delete TPT_PLAN_DEPT_DRIVER_TBL where SCHEDULE_DATE ='" + DateUtility.ConvertDateForSQLPurpose(p.ScheduleDate) + "'";
                sqlDeleteString += " AND TPT_DEPT_CODE='" + CommonUtilities.FormatString(p.TptDeptCode) + "'";
                sqlDeleteString += " AND DRIVER_CODE='" + CommonUtilities.FormatString(driver.Code) + "'";
                SqlCommand cmd = new SqlCommand(sqlDeleteString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                //cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool DeleteTrailer(PlanDateDept p, TrailerLocation trailer, SqlConnection cn, SqlTransaction tran)
        {
            string sqlDeleteString = "";
            try
            {
                sqlDeleteString = " Delete TPT_PLAN_DEPT_VEHICLE_TBL where SCHEDULE_DATE ='" + DateUtility.ConvertDateForSQLPurpose(p.ScheduleDate) + "'";
                sqlDeleteString += " AND TPT_DEPT_CODE='" + CommonUtilities.FormatString(p.TptDeptCode) + "'";
                sqlDeleteString += " AND VEHICLE_NO='" + CommonUtilities.FormatString(trailer.TrailerNo) + "'";
                SqlCommand cmd = new SqlCommand(sqlDeleteString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                //cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new FMException("Fail to delete in DeleteTrailer" + ex.ToString());
            }
        }

        public static bool AddDriverToPlanDateDept(PlanDateDept p, Driver d, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = "INSERT INTO TPT_PLAN_DEPT_DRIVER_TBL (SCHEDULE_DATE,TPT_DEPT_CODE,DRIVER_CODE) VALUES ('" + DateUtility.ConvertDateForSQLPurpose(p.ScheduleDate);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(p.TptDeptCode);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(d.Code) + "')";
                SqlCommand cmd = new SqlCommand(SQLInstertString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException("Fail to insert in AddDriverToPlanDateDept \n" + ex.ToString());
            }

            return true;

        }

        internal static bool AddVehicleToPlanDateDept(PlanDateDept p, TrailerLocation v, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                string SQLInstertString = "INSERT INTO TPT_PLAN_DEPT_VEHICLE_TBL (SCHEDULE_DATE,TPT_DEPT_CODE,VEHICLE_NO) VALUES ('" + DateUtility.ConvertDateForSQLPurpose(p.ScheduleDate);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(p.TptDeptCode);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(v.TrailerNo) + "')";
                SqlCommand cmd = new SqlCommand(SQLInstertString, cn);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (Exception ex)
            {
                retValue = false;
                throw new FMException("Fail to insert in AddVehicleToPlanDateDept \n" + ex.ToString());
            }

            return retValue;
        }

        public static bool AddPlanDateDept(PlanDateDept p, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = "INSERT INTO TPT_PLAN_DEPT_TBL (SCHEDULE_DATE,TPT_DEPT_CODE) VALUES ('" + DateUtility.ConvertDateForSQLPurpose(p.ScheduleDate);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(p.TptDeptCode) + "')";
                SqlCommand cmd = new SqlCommand(SQLInstertString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException("Fail to insert in AddPlanDateDept " + ex.ToString());
            }

            return true;
        }

        public static bool IsDriverUsed(DateTime scheduleDate, string driver, DeptType deptyType)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "";
            string planTripNo = "";
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                if (deptyType == DeptType.Trucking)
                {
                    SQLString = @"select PLANTRIP_NO from TRK_PLAN_TRIP_TBL
                                where schedule_date = '{0}' AND driver_no = '{1}'";
                }
                else if (deptyType == DeptType.Haulage)
                {
                    SQLString = @"select PLANTRIP_NO from TPT_PLAN_TRIP_TBL
                                where schedule_date = '{0}' AND driver_no = '{1}'";
                }

                SQLString = string.Format(SQLString, DateUtility.ConvertDateAndTimeForSQLPurpose(scheduleDate), CommonUtilities.FormatString(driver));
                adapter = new SqlDataAdapter(SQLString, con);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (dt.Rows.Count > 0)
            {
                DataTable dt1 = new DataTable();
                planTripNo = dt.Rows[0][0].ToString();

                if (deptyType == DeptType.Trucking)
                    SQLString = @"select * from TRK_PLAN_SUB_TRIP_TBL where PLANTRIP_NO ='" + CommonUtilities.FormatString(planTripNo) + "'";
                else if (deptyType == DeptType.Haulage)
                    SQLString = @"select * from TPT_PLAN_SUBTRIP_TBL where PLANTRIP_NO ='" + CommonUtilities.FormatString(planTripNo) + "'";

                adapter = new SqlDataAdapter(SQLString, con);
                adapter.Fill(dt1);
                if (dt1.Rows.Count > 0)
                    return true;

                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        internal static List<string> GetJobNosForPlanDateDept(string driver, DateTime scheduleDate)
        {
            List<string> jobNos = new List<string>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            #region Query
            string SQLString = @"select distinct jobMain.JOB_NUMBER from TPT_PLAN_SUBTRIP_JOB_TBL as plSubTripJob
                                    inner join TPT_PLAN_SUBTRIP_TBL as plSubTrip
                                    on plSubTrip.PLANTRIP_NO = plSubTripJob.PLANTRIP_NO 
                                        and plSubTrip.SEQ_NO = plSubTripJob.SEQ_NO
                                    inner join TPT_PLAN_TRIP_TBL as plTrip
                                    on plTrip.PLANTRIP_NO = plSubTripJob.PLANTRIP_NO
                                    inner join TPT_JOB_MAIN_Tbl as jobMain
                                    on jobMain.JOB_ID = plSubTripJob.JOB_ID

                                    where plTrip.DRIVER_NO = '{0}'
                                    and
                                    plTrip.SCHEDULE_DATE ='{1}'                                    
                                 ";
            SQLString = string.Format(SQLString, CommonUtilities.FormatString(driver), DateUtility.ConvertDateForSQLPurpose(scheduleDate));

            #endregion
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobNos.Add(reader.GetString(0).ToString());
                }
                cn.Close();
            }
            catch { }

            return jobNos;
        }


        //20140624 - gerry added to delete TPT_PLAN_DEPT_TBL and TPT_PLAN_DEPT_DRIVER_TBL and TPT_PLAN_DEPT_VEHICLE_TBL 
        //which added during creation of planDateDept auto selectdrivers and trailers
        internal static void DeletePlanDeptDriversTrailersForScheduleDate(DateTime scheduleDate, string deptCode, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //delete from 3 tables
                string deleteQuery = @" Delete from TPT_PLAN_DEPT_VEHICLE_TBL where SCHEDULE_DATE ='{0}' and TPT_DEPT_CODE ='{1}' 
                                        Delete from TPT_PLAN_DEPT_DRIVER_TBL where SCHEDULE_DATE ='{0}' and TPT_DEPT_CODE ='{1}'
                                        Delete from TPT_PLAN_DEPT_TBL where SCHEDULE_DATE ='{0}' and TPT_DEPT_CODE ='{1}' ";  
                deleteQuery = string.Format(deleteQuery, DateUtility.ConvertDateForSQLPurpose(scheduleDate), CommonUtilities.FormatString(deptCode));

                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(deleteQuery, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (con.State == ConnectionState.Open) { con.Close(); } }
        }

        internal static bool IsPlanDateDeptExist(PlanDateDept planDateDept)
        { 
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLSearchString = @"SELECT count(*) FROM TPT_PLAN_DEPT_TBL
                                                where schedule_date ='{0}' 
                                                    and TPT_DEPT_CODE ='{1}'";
                SQLSearchString = string.Format(SQLSearchString, DateUtility.ConvertDateForSQLPurpose(planDateDept.ScheduleDate), CommonUtilities.FormatString(planDateDept.TptDeptCode));
                SqlCommand cmd = new SqlCommand(SQLSearchString, cn);
                object obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    if ((int)obj > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return false;
        }

        internal static SortableList<JobAssignmentPriority> GetDriversForTheDayByPriority(DateTime scheduleDate, string dept)
        {
            SortableList<JobAssignmentPriority> drivers = new SortableList<JobAssignmentPriority>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_PLAN_DEPT_DRIVER_TBL";
            SQLString += " inner join TPT_Driver_Tbl";
            SQLString += " on TPT_PLAN_DEPT_DRIVER_TBL.Driver_Code=TPT_Driver_Tbl.Driver_Code";
            SQLString += " WHERE SCHEDULE_DATE='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
            SQLString += " AND TPT_DEPT_CODE='" + CommonUtilities.FormatString(dept) + "'";
            SQLString += " ORDER BY TPT_PLAN_DEPT_DRIVER_TBL.Schedule_Priority, TPT_Driver_Tbl.Driver_Name";
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                   // drivers.Add(GetDriver(reader));
                }
                reader.Close();
                
                cn.Close();
                //20140701 end
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }
        //for haulage
        internal static SortableList<Driver> GetDriversForPlanDateDeptForHaulage(DateTime scheduleDate, string dept)
        {
            SortableList<Driver> drivers = new SortableList<Driver>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            //20150628 - gerr modified the query
            string SQLString = @"SELECT * FROM TPT_PLAN_DEPT_DRIVER_TBL
                                    inner join TPT_Driver_Tbl
                                    on TPT_PLAN_DEPT_DRIVER_TBL.Driver_Code=TPT_Driver_Tbl.Driver_Code

                                    inner join TPT_Vehicle_Tbl
                                    on TPT_Vehicle_Tbl.Vehicle_No = TPT_Driver_Tbl.Default_Vehicle
                                    AND TPT_Vehicle_Tbl.Vehicle_Type = 1 -- prime mover only

                                    left join TPT_PLAN_DRIVER_PRIORITY_Tbl	
                                    on TPT_PLAN_DRIVER_PRIORITY_Tbl.DriverCode = TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE
                                    and TPT_PLAN_DRIVER_PRIORITY_Tbl.PrioritySchedule_Date = TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE

                                    Left join TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL
                                    on TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL.Code = TPT_PLAN_DRIVER_PRIORITY_Tbl.Priority_Code
    
                                    WHERE TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE='{0}'
                                    AND TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE='{1}'
                                    AND TPT_Driver_Tbl.Default_Vehicle <> ''

                                    ORDER BY ISNULL(TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL.Priority_No, 999), TPT_Driver_Tbl.Default_Vehicle";

            SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), CommonUtilities.FormatString(dept));

            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Driver drv = GetDriver(reader);
                    //20150704 - gerry added for driver priority
                    drv.priority = reader["Priority_No"] == DBNull.Value ? 999 : (int)reader["Priority_No"]; //Priority_No
                    drivers.Add(drv);
                }
                reader.Close();
                //20140701 - gerry added to get the partial unavailable dates for drivers
                if (drivers.Count > 0)
                {
                    foreach (Driver dr in drivers)
                    {
                        //20140701 - gerry to get the partial unavailable date for the driver
                        dr.unAvailableDates = dr.GetDriverUnavailableDates(scheduleDate, cn, tran);
                        if (dr.unAvailableDates.Count > 0)
                        {
                            if (dr.unAvailableDates[0].startDateTime.Date == dr.unAvailableDates[0].endDateTime.Date)
                                dr.DescriptionForPlanningPurpose += " Off(" + dr.unAvailableDates[0].startDateTime.TimeOfDay.ToString().Substring(0, 5) + "-" + dr.unAvailableDates[0].endDateTime.TimeOfDay.ToString().Substring(0, 5) + ")";
                            else if ((dr.unAvailableDates[0].endDateTime.Date > scheduleDate.Date))
                            {
                                if (dr.unAvailableDates[0].startDateTime.TimeOfDay == scheduleDate.TimeOfDay)
                                {
                                    dr.DescriptionForPlanningPurpose += " Off(" + dr.unAvailableDates[0].remarks + ")";
                                    //dr.priority = 999;
                                }
                                else
                                    dr.DescriptionForPlanningPurpose += " Off(" + dr.unAvailableDates[0].startDateTime.ToString("dd/MM/yyyy HH:mm") + " - " + dr.unAvailableDates[0].endDateTime.ToString("dd/MM/yyyy HH:mm") + ")";
                            }
                        }
                    }
                }
                cn.Close();
                //20140701 end
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }
        //20160226 - gerry added
        internal static SortableList<Driver> GetDriversForPlanDateDeptForTrucking(DateTime scheduleDate, string dept)
        {
            SortableList<Driver> drivers = new SortableList<Driver>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            //20150628 - gerr modified the query
            string SQLString = @"SELECT * FROM TPT_PLAN_DEPT_DRIVER_TBL
                                    inner join TPT_Driver_Tbl
                                    on TPT_PLAN_DEPT_DRIVER_TBL.Driver_Code=TPT_Driver_Tbl.Driver_Code

                                    inner join TPT_Vehicle_Tbl
                                    on TPT_Vehicle_Tbl.Vehicle_No = TPT_Driver_Tbl.Default_Vehicle
                                    AND TPT_Vehicle_Tbl.Vehicle_Type = 3 -- truck only

                                    left join TPT_PLAN_DRIVER_PRIORITY_Tbl	
                                    on TPT_PLAN_DRIVER_PRIORITY_Tbl.DriverCode = TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE
                                    and TPT_PLAN_DRIVER_PRIORITY_Tbl.PrioritySchedule_Date = TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE

                                    Left join TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL
                                    on TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL.Code = TPT_PLAN_DRIVER_PRIORITY_Tbl.Priority_Code
    
                                    WHERE TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE='{0}'
                                    AND TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE='{1}'
                                    AND TPT_Driver_Tbl.Default_Vehicle <> ''

                                    ORDER BY ISNULL(TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL.Priority_No, 999), TPT_Driver_Tbl.Default_Vehicle";

            SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), CommonUtilities.FormatString(dept));

            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Driver drv = GetDriver(reader);
                    //20150704 - gerry added for driver priority
                    drv.priority = reader["Priority_No"] == DBNull.Value ? 999 : (int)reader["Priority_No"]; //Priority_No
                    drivers.Add(drv);
                }
                reader.Close();
                //20140701 - gerry added to get the partial unavailable dates for drivers
                if (drivers.Count > 0)
                {
                    foreach (Driver dr in drivers)
                    {
                        //20140701 - gerry to get the partial unavailable date for the driver
                        dr.unAvailableDates = dr.GetDriverUnavailableDates(scheduleDate, cn, tran);
                        if (dr.unAvailableDates.Count > 0)
                        {
                            if (dr.unAvailableDates[0].startDateTime.Date == dr.unAvailableDates[0].endDateTime.Date)
                                dr.DescriptionForPlanningPurpose += " Off(" + dr.unAvailableDates[0].startDateTime.TimeOfDay.ToString().Substring(0, 5) + "-" + dr.unAvailableDates[0].endDateTime.TimeOfDay.ToString().Substring(0, 5) + ")";
                            else if ((dr.unAvailableDates[0].endDateTime.Date > scheduleDate.Date))
                            {
                                if (dr.unAvailableDates[0].startDateTime.TimeOfDay == scheduleDate.TimeOfDay)
                                {
                                    dr.DescriptionForPlanningPurpose += " Off(" + dr.unAvailableDates[0].remarks + ")";
                                    //dr.priority = 999;
                                }
                                else
                                    dr.DescriptionForPlanningPurpose += " Off(" + dr.unAvailableDates[0].startDateTime.ToString("dd/MM/yyyy HH:mm") + " - " + dr.unAvailableDates[0].endDateTime.ToString("dd/MM/yyyy HH:mm") + ")";
                            }
                        }
                    }
                }
                cn.Close();
                //20140701 end
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }

        internal static Driver GetDriverWithPriority(string driverCode, DateTime scheduleDate)
        {
            Driver driver = new Driver();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT * FROM TPT_Driver_Tbl drv
                                    left join TPT_PLAN_DRIVER_PRIORITY_Tbl	drvp
                                    on drvp.DriverCode = drv.Driver_Code
                                    Left join TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL drva
                                    on drva.Code = drvp.Priority_Code
                                    WHERE drv.Driver_Code= '{0}'
                                    AND drvp.PrioritySchedule_Date= '{1}'";

            SQLString = string.Format(SQLString, CommonUtilities.FormatString(driverCode), DateUtility.ConvertDateForSQLPurpose(scheduleDate));

            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    driver = GetDriver(reader);
                    driver.priority = reader["Priority_No"] == DBNull.Value ? 999 : (int)reader["Priority_No"]; //Priority_No
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return driver;
            
        }

        internal static SortableList<Vehicle> GetTrucksForPlanDate(DateTime scheduleDate, string dept)
        {
            SortableList<Vehicle> trucks = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                //20150628 - gerr modified the query
                string SQLString = @"SELECT * FROM TPT_PLAN_DEPT_DRIVER_TBL
                                    inner join TPT_Driver_Tbl
                                    on TPT_PLAN_DEPT_DRIVER_TBL.Driver_Code=TPT_Driver_Tbl.Driver_Code
                                    left join TPT_PLAN_DRIVER_PRIORITY_Tbl	
                                    on TPT_PLAN_DRIVER_PRIORITY_Tbl.DriverCode = TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE
                                    and TPT_PLAN_DRIVER_PRIORITY_Tbl.PrioritySchedule_Date = TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE
                                    Left join TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL
                                    on TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL.Code = TPT_PLAN_DRIVER_PRIORITY_Tbl.Priority_Code
                                     WHERE TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE='{0}'
                                    AND TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE='{1}'
                                    ORDER BY ISNULL(TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL.Priority_No, 999), TPT_Driver_Tbl.Default_Vehicle";

                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), CommonUtilities.FormatString(dept));

                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    SqlTransaction tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Vehicle truck = GetVehicles(reader);
                        trucks.Add(truck);
                    }
                    reader.Close();
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                catch (Exception ex) { throw new FMException(ex.ToString()); }
            }
            return trucks;
        }

   
    }
}
