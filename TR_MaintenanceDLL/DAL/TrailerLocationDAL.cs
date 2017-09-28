using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using TR_LanguageResource.Resources;

namespace FM.TR_MaintenanceDLL.DAL
{
    internal class TrailerLocationDAL
    {
        internal static SortableList<TrailerLocation> GetAllPreviousTrailerLocations(DateTime scheduleDate, string dept)
        {
            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                SortableList<Vehicle> trailers = new SortableList<Vehicle>();
                trailers = Vehicle.GetAllAvailableTrailersBasedOnDepartment(scheduleDate, dept);
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_PLAN_DEPT_VEHICLE_TBL.VEHICLE_NO";
                    SQLString += " left outer join TPT_Driver_Tbl";
                    SQLString += " on TPT_Driver_Tbl.Driver_Code=TPT_TRAILER_LOCATION_TBL.DriverNo";
                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    SQLString += " AND ScheduleDate <= '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    SQLString += " ORDER BY TrailerNo, StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TrailerLocation temp = GetTrailerLocation(reader);
                        if (reader["Driver_Name"] == DBNull.Value)
                            temp.DriverName = string.Empty;
                        else
                            temp.DriverName = Convert.ToString(reader["Driver_Name"]);

                        trailerlocations.Add(temp);
                    }
                    reader.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return trailerlocations;
        }
        internal static SortableList<TrailerLocation> GetAllPreviousTrailerLocationsTEST(DateTime scheduleDate, string dept)
        {
            SortableList<Vehicle> trailers = new SortableList<Vehicle>();
          
            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {    
                trailers = Vehicle.GetAllAvailableTrailersBasedOnDepartment(scheduleDate, dept);
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_PLAN_DEPT_VEHICLE_TBL.VEHICLE_NO";
                    SQLString += " left outer join TPT_Driver_Tbl";
                    SQLString += " on TPT_Driver_Tbl.Driver_Code=TPT_TRAILER_LOCATION_TBL.DriverNo";
                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    SQLString += " AND ScheduleDate <= '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    SQLString += " ORDER BY TrailerNo, StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); } 
                    IDataReader reader = cmd.ExecuteReader();    
                    while (reader.Read())
                    {
                        TrailerLocation temp = GetTrailerLocation(reader);
                        if (reader["Driver_Name"] == DBNull.Value)
                            temp.DriverName = string.Empty;
                        else
                            temp.DriverName = Convert.ToString(reader["Driver_Name"]);

                        trailerlocations.Add(temp);
                    }
                    reader.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }
            return trailerlocations;
        }

        internal static SortableList<TrailerLocation> GetAllTrailerLocations(DateTime scheduleDate, string dept)
        {
            SortableList<Vehicle> trailers = new SortableList<Vehicle>();     
            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                trailers = Vehicle.GetAllAvailableTrailersBasedOnDepartment(scheduleDate, dept);   
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT DISTINCT TPT_TRAILER_LOCATION_TBL.*, TPT_Driver_Tbl.Driver_Name FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_PLAN_DEPT_VEHICLE_TBL.VEHICLE_NO";
                    SQLString += " left outer join TPT_Driver_Tbl";
                    SQLString += " on TPT_Driver_Tbl.Driver_Code=TPT_TRAILER_LOCATION_TBL.DriverNo";
                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    SQLString += " AND ScheduleDate = '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    SQLString += " ORDER BY TrailerNo, StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();          
                    int counting = 0;
                    while (reader.Read())
                    {
                        counting = counting + 1;
                        TrailerLocation temp = GetTrailerLocation(reader);
                        if (reader["Driver_Name"] == DBNull.Value)
                            temp.DriverName = string.Empty;
                        else
                            temp.DriverName = Convert.ToString(reader["Driver_Name"]);

                        trailerlocations.Add(temp);
                    }
                    reader.Close();

                    if (counting == 0)
                    {
                        SQLString = "SELECT DISTINCT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                        SQLString += " inner join TPT_PLAN_DEPT_VEHICLE_TBL";
                        SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_PLAN_DEPT_VEHICLE_TBL.VEHICLE_NO";
                        SQLString += " left outer join TPT_Driver_Tbl";
                        SQLString += " on TPT_Driver_Tbl.Driver_Code=TPT_TRAILER_LOCATION_TBL.DriverNo";
                        SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                        SQLString += " AND SCHEDULE_DATE <= '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                        SQLString += " ORDER BY TrailerNo, StartTime DESC";
                        cmd = new SqlCommand(SQLString, cn); 
                        if (cn.State == ConnectionState.Closed) { cn.Open(); }
                        cmd.CommandTimeout = 0;
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TrailerLocation temp = GetTrailerLocation(reader);
                            if (reader["Driver_Name"] == DBNull.Value)
                                temp.DriverName = string.Empty;
                            else
                                temp.DriverName = Convert.ToString(reader["Driver_Name"]);

                            trailerlocations.Add(temp);
                        }
                        reader.Close();
                    }

                    if (counting == 1)
                    {
                        SQLString = "SELECT DISTINCT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                        SQLString += " inner join TPT_PLAN_DEPT_VEHICLE_TBL";
                        SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_PLAN_DEPT_VEHICLE_TBL.VEHICLE_NO";
                        SQLString += " left outer join TPT_Driver_Tbl";
                        SQLString += " on TPT_Driver_Tbl.Driver_Code=TPT_TRAILER_LOCATION_TBL.DriverNo";
                        SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                        SQLString += " AND ScheduleDate < '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                        SQLString += " ORDER BY TrailerNo, StartTime DESC";
                        cmd = new SqlCommand(SQLString, cn);
                        if (cn.State == ConnectionState.Closed) { cn.Open(); }
                        cmd.CommandTimeout = 0;
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TrailerLocation temp = GetTrailerLocation(reader);
                            if (reader["Driver_Name"] == DBNull.Value)
                                temp.DriverName = string.Empty;
                            else
                                temp.DriverName = Convert.ToString(reader["Driver_Name"]);

                            trailerlocations.Add(temp);
                        }
                        reader.Close();  
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) 
            {
                if (ex.Number == -2) 
                    throw new FMException("Timeout occurred");
                else
                    throw new FMException(ex.ToString()); 
            }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }
            return trailerlocations;
        }

        internal static SortableList<TrailerLocation> GetAllTrailerLocationsForThisDayOnly(DateTime scheduleDate, string dept)
        {
            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                SortableList<Vehicle> trailers = new SortableList<Vehicle>();
                trailers = Vehicle.GetAllAvailableTrailersBasedOnDepartment(scheduleDate, dept); 
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT DISTINCT TPT_TRAILER_LOCATION_TBL.* FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_PLAN_DEPT_VEHICLE_TBL.VEHICLE_NO";
                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    SQLString += " AND ScheduleDate = '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    SQLString += " ORDER BY StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        trailerlocations.Add(GetTrailerLocation(reader));
                    }
                    reader.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return trailerlocations;
        }
        internal static SortableList<TrailerLocation> GetAllTrailerLocationsForThisDayOnlyUsedInPlanning(DateTime scheduleDate, string dept)
        {   
            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());  
            SortableList<Vehicle> trailers = new SortableList<Vehicle>();
            trailers = Vehicle.GetAllAvailableTrailersBasedOnDepartment(scheduleDate, dept);
            try
            {
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT DISTINCT TPT_TRAILER_LOCATION_TBL.* FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_PLAN_DEPT_VEHICLE_TBL.VEHICLE_NO";

                    SQLString += " inner join TPT_PLAN_SUBTRIP_TBL";
                    SQLString += " ON TPT_PLAN_SUBTRIP_TBL.TRAILER_NO = TPT_TRAILER_LOCATION_TBL.TrailerNo";

                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    SQLString += " AND ScheduleDate = '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    SQLString += " ORDER BY StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader(); 
                    while (reader.Read())
                    {
                        trailerlocations.Add(GetTrailerLocation(reader));
                    }
                    reader.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return trailerlocations;
        }
        //20151123 - gerry added to get the affected trailer location based on driver and plan date
        internal static SortableList<TrailerLocation> GetAllAffectedTrailerLocationsForThisDay(string driverCode, DateTime scheduleDate, string dept)
        {
            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SortableList<Vehicle> trailers = new SortableList<Vehicle>();
            trailers = Vehicle.GetAllAvailableTrailersBasedOnDepartment(scheduleDate, dept);
            try
            {
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT DISTINCT TPT_TRAILER_LOCATION_TBL.* FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " on TPT_TRAILER_LOCATION_TBL.TrailerNo=TPT_PLAN_DEPT_VEHICLE_TBL.VEHICLE_NO";

                    SQLString += " inner join TPT_PLAN_SUBTRIP_TBL";
                    SQLString += " ON TPT_PLAN_SUBTRIP_TBL.TRAILER_NO = TPT_TRAILER_LOCATION_TBL.TrailerNo";

                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    SQLString += " AND ScheduleDate = '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    SQLString += " ORDER BY StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        trailerlocations.Add(GetTrailerLocation(reader));
                    }
                    reader.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return trailerlocations;
        }


        internal static SortableList<TrailerLocation> GetAllLatestTrailerLocations(string status)
        { 
            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {                    
            SortableList<Vehicle> trailers = new SortableList<Vehicle>();
            trailers = Vehicle.GetAllTrailers();
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    //SQLString += " AND ScheduleDate = '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    if (status != "")
                    {
                        SQLString += " AND TrailerStatus = " + status;
                    }
                    SQLString += " ORDER BY StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();            
                    while (reader.Read())
                    {
                        trailerlocations.Add(GetTrailerLocation(reader));
                    }
                    reader.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }
            return trailerlocations;
        }

        internal static SortableList<TrailerLocation> GetAllTrailerHistory(string trailerno, DateTime startdate, DateTime enddate)
        {
            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                SortableList<Vehicle> trailers = new SortableList<Vehicle>();
                if (trailerno == "")
                    trailers = Vehicle.GetAllTrailers();
                else
                {
                    Vehicle t = Vehicle.GetVehicle(trailerno);
                    trailers.Add(t);
                }
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT * FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    SQLString += " AND ScheduleDate >= '" + DateUtility.ConvertDateForSQLPurpose(startdate) + "'";
                    SQLString += " AND ScheduleDate < '" + DateUtility.ConvertDateForSQLPurpose(enddate.AddDays(1)) + "'";
                    SQLString += " ORDER BY ScheduleDate Desc,StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();  
                    while (reader.Read())
                    {
                        trailerlocations.Add(GetTrailerLocation(reader));
                    }
                    reader.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return trailerlocations;
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
            try
            {
                TrailerLocation trailerlocation = new TrailerLocation(
                        (string)reader["TrailerNo"],
                        (DateTime)reader["ScheduleDate"],
                        (DateTime)reader["DateChange"],
                        (DateTime)reader["StartTime"],
                        (DateTime)reader["EndTime"],
                        t,
                        reader["StartStop"] == DBNull.Value ? string.Empty : (string)reader["StartStop"],
                        reader["StopCode"] == DBNull.Value ? string.Empty : (string)reader["StopCode"],
                        reader["ParkStopCode"] == DBNull.Value ? string.Empty : (string)reader["ParkStopCode"],
                        reader["PlanTripNo"] == DBNull.Value ? string.Empty : (string)reader["PlanTripNo"],
                        reader["DriverNo"] == DBNull.Value ? string.Empty : (string)reader["DriverNo"],
                        reader["Remarks"] == DBNull.Value ? string.Empty : (string)reader["Remarks"],
                        (byte[])reader["UpdateVersion"]
                        );
                return trailerlocation;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static bool DeleteTrailerLocations(DateTime planDate, string tptDept, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string SQLDeleteString = "DELETE FROM TPT_TRAILER_LOCATION_TBL where trailerno in ";
                SQLDeleteString += "(select vehicle_no from TPT_PLAN_DEPT_VEHICLE_TBL";
                SQLDeleteString += " Where Schedule_Date = '" + DateUtility.ConvertDateForSQLPurpose(planDate) + "'" +
                                   " AND TPT_DEPT_CODE = '" + CommonUtilities.FormatString(tptDept) + "')";
                SQLDeleteString += " AND ScheduleDate ='" + DateUtility.ConvertDateForSQLPurpose(planDate) + "'";
                SqlCommand cmd = new SqlCommand(SQLDeleteString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }     

        internal static bool HasTrailerLocationUpdateVersionChange(TrailerLocation trailerlocation, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT UpdateVersion FROM TPT_TRAILER_LOCATION_TBL";
                SQLString += " Where StartTime = '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.StartTime) + "'" +
                             " AND TrailerNo = '" + CommonUtilities.FormatString(trailerlocation.TrailerNo) + "'";
                SqlCommand cmd2 = new SqlCommand(SQLString, cn);
                cmd2.CommandType = CommandType.Text;
                cmd2.Transaction = tran;
                IDataReader reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UpdateVersion"];
                }
                reader.Close();
                if (SqlBinary.Equals(trailerlocation.UpdateVersion, originalRowVersion) == false)
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + " TrailerLocatationDAL.HasTrailerLocationUpdateVersionChange");
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }

        internal static bool AddTrailerLocations(TrailerLocation trailerlocation, SqlConnection cn, SqlTransaction tran)
        {
            string SQLInstertString = "";
            try
            {
                string SQLDeleteString = "DELETE FROM TPT_TRAILER_LOCATION_TBL";
                SQLDeleteString += " Where StartTime = '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.StartTime) + "'" +
                                   " AND TrailerNo = '" + CommonUtilities.FormatString(trailerlocation.TrailerNo) + "'";
                SqlCommand cmd = new SqlCommand(SQLDeleteString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                SQLInstertString = "INSERT INTO TPT_TRAILER_LOCATION_TBL (ScheduleDate,TrailerNo,StartTime,EndTime,DateChange,TrailerStatus,StartStop,StopCode,ParkStopCode,DriverNo,PlanTripNo,Remarks) VALUES ('" 
                + DateUtility.ConvertDateForSQLPurpose(trailerlocation.ScheduleDate.Date);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.TrailerNo);
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.StartTime);
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.EndTime);
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(DateTime.Now);
                SQLInstertString += "'" + "," + "'" + trailerlocation.TrailerStatus.GetHashCode().ToString();
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.StartStop);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.StopCode);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.ParkStop);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.DriverNo);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.PlanTripNo);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.Remarks) + "')";
                SqlCommand cmd1 = new SqlCommand(SQLInstertString, cn);
                cmd1.Transaction = tran;
                cmd1.ExecuteNonQuery();


                string SQLString = "SELECT UpdateVersion FROM TPT_TRAILER_LOCATION_TBL";
                SQLString += " Where StartTime = '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.StartTime) + "'" +
                             " AND TrailerNo = '" + CommonUtilities.FormatString(trailerlocation.TrailerNo) + "'";

                SqlCommand cmd2 = new SqlCommand(SQLString, cn);
                cmd2.Transaction = tran;
                IDataReader reader = cmd2.ExecuteReader();

                while (reader.Read())
                {
                    trailerlocation.UpdateVersion = (byte[])reader["UPDATEVERSION"];
                }
                reader.Close(); 
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            
            return true;
        }

        internal static DateTime GetLatestPlanDate()
        {
            DateTime scheduledate = DateTime.Today.Date;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                SQLString += " ORDER BY ScheduleDate DESC";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    scheduledate = (DateTime)reader["ScheduleDate"];
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }
            return scheduledate;
        }

        internal static bool AddTrailerLocation(TrailerLocation trailerlocation, out TrailerLocation trailerLoactionOut, SqlConnection cn, SqlTransaction tran, out string msg)
        {
            msg = "";
            string SQLInstertString = "";
            trailerLoactionOut = new TrailerLocation();
            try
            {
                SQLInstertString = "INSERT INTO TPT_TRAILER_LOCATION_TBL (ScheduleDate,TrailerNo,StartTime,EndTime,DateChange,TrailerStatus,StartStop,StopCode,ParkStopCode,DriverNo,PlanTripNo,Remarks) VALUES ('"
                + DateUtility.ConvertDateAndTimeForSQLPurpose(trailerlocation.ScheduleDate.Date);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.TrailerNo.Trim());
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.StartTime);
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.EndTime);
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(DateTime.Now);
                SQLInstertString += "'" + "," + "'" + trailerlocation.TrailerStatus.GetHashCode().ToString();
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.StartStop.Trim());
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.StopCode.Trim());
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.ParkStop.Trim());
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.DriverNo);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.PlanTripNo.Trim());
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.Remarks) + "')";
                SqlCommand cmd1 = new SqlCommand(SQLInstertString, cn);
                cmd1.Transaction = tran;
                cmd1.ExecuteNonQuery();

                trailerLoactionOut = GetTrailerLocation(trailerlocation.ScheduleDate, trailerlocation.TrailerNo, trailerlocation.StartTime, cn, tran);

                msg = CommonResource.SaveSuccess;
                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static TrailerLocation GetTrailerLocation(DateTime scheduleDate, string trailerNo, DateTime startTime, SqlConnection con, SqlTransaction trans)
        {
            TrailerLocation trailerLocation = new TrailerLocation();
            try
            {
                string SQLString = @"Select * from TPT_TRAILER_LOCATION_TBL
                                   left outer join TPT_Driver_Tbl
                                     on TPT_Driver_Tbl.Driver_Code=TPT_TRAILER_LOCATION_TBL.DriverNo
                                Where ScheduleDate = '{0}' AND TrailerNo = '{1}' and StartTime ='{2}'";

                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), CommonUtilities.FormatString(trailerNo), DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(startTime));
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = trans;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    trailerLocation = GetTrailerLocation(reader);
                    if (reader["Driver_Name"] == DBNull.Value)
                        trailerLocation.DriverName = string.Empty;
                    else
                        trailerLocation.DriverName = Convert.ToString(reader["Driver_Name"]);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return trailerLocation;
        }

        internal static SortableList<TrailerLocation> GetTrailersForTrailerLocationStatus(DateTime scheduleDate)
        {
            SortableList<TrailerLocation> availableTrailers = new SortableList<TrailerLocation>(); 
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                SortableList<Vehicle> trailers = new SortableList<Vehicle>();
                trailers = Vehicle.GetAllTrailersForParticularDate(scheduleDate);
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_Vehicle_Tbl";
                    SQLString += " on TPT_Vehicle_Tbl.Vehicle_No = TPT_TRAILER_LOCATION_TBL.TrailerNo";
                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    //SQLString += " AND ScheduleDate = '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    //SQLString += " AND TrailerStatus = " + trailerStatus.GetHashCode();
                    SQLString += " ORDER BY StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if ((int)reader["TrailerStatus"] != 1 && (int)reader["TrailerStatus"] != 5)
                        {
                            #region Remove
                            //    int temp = (int)reader["TrailerStatus"];
                            //    TrailerStatus status = TrailerStatus.TrailerPark;
                            //    if (temp == 1)
                            //        status = TrailerStatus.Assigned;
                            //    else if (temp == 2)
                            //        status = TrailerStatus.CustomerStuff;
                            //    else if (temp == 3)
                            //        status = TrailerStatus.CustomerUnstuff;
                            //    else if (temp == 4)
                            //        status = TrailerStatus.TrailerPark;
                            //    else if (temp == 5)
                            //        status = TrailerStatus.Unavailable;
                            //    else if (temp == 6)
                            //        status = TrailerStatus.CompleteStuffUnstuff;

                            //    trailers[i].LocationStatus = status;
                            #endregion
                            TrailerLocation temp = GetTrailerLocation(reader);
                            temp.MaximumLadenWeight = (decimal)reader["Max_Weight"];
                            temp.TrailerType = (string)reader["Trailer_Type"];
                            availableTrailers.Add(temp);
                        }
                    }
                    cn.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }
            return availableTrailers;
        }

        internal static SortableList<TrailerLocation> GetAllTrailersAssigned(DateTime scheduleDate)
        {
            SortableList<TrailerLocation> vehicles = new SortableList<TrailerLocation>();  
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string sqlByvehicleNo = string.Empty;
                string sqlByvehicleType = string.Empty;
                string SQLString = "SELECT * FROM TPT_Vehicle_Tbl";
                SQLString += " inner join TPT_TRAILER_LOCATION_TBL";
                SQLString += " on TPT_Vehicle_Tbl.Vehicle_No = TPT_TRAILER_LOCATION_TBL.TrailerNo";
                SQLString += " WHERE Vehicle_Type=" + VehicleType.Trailer.GetHashCode().ToString();
                SQLString += " AND Is_Available= 'T'";
                SQLString += " AND vehicle_no in (select vehicle_no from TPT_PLAN_DEPT_VEHICLE_TBL";
                SQLString += " WHERE SCHEDULE_DATE='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "')";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if ((int)reader["TrailerStatus"] != TrailerStatus.Assigned.GetHashCode()
                            && (int)reader["TrailerStatus"] != TrailerStatus.Unavailable.GetHashCode())
                    {
                        TrailerLocation temp = GetTrailerLocation(reader);
                        temp.MaximumLadenWeight = (decimal)reader["Max_Weight"];
                        temp.TrailerType = (string)reader["Trailer_Type"];
                        vehicles.Add(temp);
                    }
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }
            return vehicles;
        }

        internal static SortableList<TrailerLocation> GetAllTrailersForParticularDate(DateTime scheduleDate)
        {
            SortableList<TrailerLocation> availableTrailers = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
                vehicles = VehicleDAL.GetTrailersInParticularDate(scheduleDate);
                for (int i = 0; i < vehicles.Count; i++)
                {
                    string SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_Vehicle_Tbl";
                    SQLString += " on TPT_Vehicle_Tbl.Vehicle_No = TPT_TRAILER_LOCATION_TBL.TrailerNo";
                    SQLString += " WHERE TPT_TRAILER_LOCATION_TBL.TrailerNo = '" + CommonUtilities.FormatString(vehicles[i].Number) + "'";
                    SQLString += " ORDER BY StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if ((int)reader["TrailerStatus"] != TrailerStatus.Assigned.GetHashCode()
                            && (int)reader["TrailerStatus"] != TrailerStatus.Unavailable.GetHashCode())
                        {
                            TrailerLocation temp = GetTrailerLocation(reader);
                            temp.MaximumLadenWeight = (decimal)reader["Max_Weight"];
                            temp.TrailerType = (string)reader["Trailer_Type"];
                            availableTrailers.Add(temp);
                        }
                    }
                    cn.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return availableTrailers;
        }

        //20140519 - gerry added to get the trailer location to be use in planning for assigning of sub contractor validation
        internal static TrailerLocation GetPreviousLegTrailerLocationForPlanning(string planTripNo, string trailerNo, DateTime startTime)
        {
            TrailerLocation trailerLocation = new TrailerLocation();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = @"Select * from TPT_TRAILER_LOCATION_TBL
                                   left outer join TPT_Driver_Tbl
                                     on TPT_Driver_Tbl.Driver_Code=TPT_TRAILER_LOCATION_TBL.DriverNo
                                Where PlanTripNo = '{0}' AND TrailerNo = '{1}' and StartTime ='{2}'";

                SQLString = string.Format(SQLString, CommonUtilities.FormatString(planTripNo), CommonUtilities.FormatString(trailerNo), DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(startTime));
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {      
                    trailerLocation = GetTrailerLocation(reader);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }  
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return trailerLocation;
        }
        internal static SortableList<TrailerLocation> GetAllTrailerInParkForTheDay(DateTime scheduleDate)
        {
            SortableList<TrailerLocation> availableTrailers = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = @" SELECT * FROM TPT_TRAILER_LOCATION_TBL tl1
                                    inner join TPT_Vehicle_Tbl v on v.Vehicle_No = tl1.TrailerNo and v.Is_Available = 'T'
                                    inner join 
                                        (
                                            Select max(StartTime) as LatestDate, TrailerNo
                                            from TPT_TRAILER_LOCATION_TBL 
                                            Group by TrailerNo
                                        ) SubMax 
                                        on tl1.StartTime = SubMax.LatestDate
                                        and tl1.TrailerNo = SubMax.TrailerNo
                                    where tl1.TrailerStatus = 4
                                    and tl1.TrailerNo not in (select vehicle_no from TPT_PLAN_DEPT_VEHICLE_TBL WHERE SCHEDULE_DATE='{0}')";

                SQLString = string.Format(SQLString, DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(scheduleDate));
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TrailerLocation tl = GetTrailerLocation(reader);
                    tl.MaximumLadenWeight = (decimal)reader["Max_Weight"];
                    tl.TrailerType = (string)reader["Trailer_Type"];
                    availableTrailers.Add(tl);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return availableTrailers;
            
        }

        internal static bool IsTrailerBeingAssigned(string trailerNo)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            try
            {
                string SQLSearchString = "SELECT * FROM TPT_TRAILER_LOCATION_TBL where TrailerStatus <> 4 AND TrailerNo = '" + CommonUtilities.FormatString(trailerNo) + "'";
                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
                DataSet dsSearchResult = new DataSet();
                daSearchCmd.Fill(dsSearchResult);
                cn.Close();
                if (dsSearchResult.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }
        }
        //20151123 - gerry added to delete trailerLocation base on driver and scheduledate
        internal static bool DeleteTrailerLocations(string driverCode, DateTime planDate, string tptDept, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string SQLDeleteString = @"DELETE FROM TPT_TRAILER_LOCATION_TBL where 1 =1 
                                            and DriverNo = '{0}'
                                            and ScheduleDate ='{1}' ";

                SQLDeleteString = string.Format(SQLDeleteString, CommonUtilities.FormatString(driverCode), DateUtility.ConvertDateForSQLPurpose(planDate));
                SqlCommand cmd = new SqlCommand(SQLDeleteString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }
        //20160104 - gerry added to delete trailerLocation base on driver and start date time
        internal static bool DeleteTrailersForParticularDateAndDriver(string driverCode, DateTime scheduleDate, string tptDept, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string SQLDeleteString = @"DELETE FROM TPT_TRAILER_LOCATION_TBL where 1 =1 
                                            and DriverNo = '{0}'
                                            and ScheduleDate ='{1}' ";

                SQLDeleteString = string.Format(SQLDeleteString, CommonUtilities.FormatString(driverCode), DateUtility.ConvertDateForSQLPurpose(scheduleDate));
                SqlCommand cmd = new SqlCommand(SQLDeleteString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }
        internal static bool DeleteTrailersForParticularStartDateTimeAndDriver(string driverCode, DateTime startDateTime, string tptDept, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string SQLDeleteString = @"DELETE FROM TPT_TRAILER_LOCATION_TBL where 1 =1 
                                            and DriverNo = '{0}'
                                            and StartTime ='{1}' ";

                SQLDeleteString = string.Format(SQLDeleteString, CommonUtilities.FormatString(driverCode), DateUtility.ConvertDateAndTimeForSQLPurpose(startDateTime));
                SqlCommand cmd = new SqlCommand(SQLDeleteString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }
        internal static SortableList<TrailerLocation> GetAllTrailersForParticularDateAndDriver(string driverCode, DateTime scheduleDate, SqlConnection cn, SqlTransaction tran)
        {
            SortableList<TrailerLocation> retValue = new SortableList<TrailerLocation>();
            using (cn)
            {
                try
                {
                    SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
                    vehicles = VehicleDAL.GetTrailersInParticularDate(scheduleDate);
                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        string SQLString = @"SELECT * FROM TPT_TRAILER_LOCATION_TBL
                                                inner join TPT_Vehicle_Tbl
                                                on TPT_Vehicle_Tbl.Vehicle_No = TPT_TRAILER_LOCATION_TBL.TrailerNo
                                                WHERE TPT_TRAILER_LOCATION_TBL.DriverNo = '{0}'
                                                and TPT_TRAILER_LOCATION_TBL.ScheduleDate = '{1}'
                                                ORDER BY TPT_TRAILER_LOCATION_TBL.StartTime Desc";

                        SQLString = string.Format(SQLString, CommonUtilities.FormatString(driverCode), DateUtility.ConvertDateForSQLPurpose(scheduleDate));
                        SqlCommand cmd = new SqlCommand(SQLString, cn);
                        cmd.CommandTimeout = 0;
                        if (cn.State == ConnectionState.Closed) { cn.Open(); }
                        IDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TrailerLocation temp = GetTrailerLocation(reader);
                            if (reader["Driver_Name"] == DBNull.Value)
                                temp.DriverName = string.Empty;
                            else
                                temp.DriverName = Convert.ToString(reader["Driver_Name"]);

                            retValue.Add(temp);
                        }
                        cn.Close();
                    }
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                catch (Exception ex) { throw new FMException(ex.ToString()); }
                finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }
            }
            return retValue;
        }
        internal static TrailerLocation GetLastTrailerLocation(string trailerNo)
        {
            TrailerLocation lastTrailerlocation = new TrailerLocation();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = @"SELECT DISTINCT tl.* FROM TPT_TRAILER_LOCATION_TBL tl
                                        where tl.TrailerNo ='{0}'
                                        AND (select count(*) from TPT_TRAILER_LOCATION_TBL
                                                where TrailerNo = tl.TrailerNo
	                                                and EndTime > tl.EndTime) < 1";
                SQLString = string.Format(SQLString, CommonUtilities.FormatString(trailerNo));
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lastTrailerlocation = GetTrailerLocation(reader);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { if (cn.State == ConnectionState.Open) { cn.Close(); } }

            return lastTrailerlocation;
        }

        #region methods to fixed missing trailer if happen
        internal static void DeleteAllTrailerLocations(DateTime dateStart, DateTime dateEnd, SqlConnection con, SqlTransaction tran, out int deletedCount)
        {
            try
            {
                string SQLDeleteString = @"delete from TPT_TRAILER_LOCATION_TBL
                                            where ScheduleDate in (select pdv.SCHEDULE_DATE from TPT_PLAN_DEPT_VEHICLE_TBL pdv
							                                            inner join TPT_Vehicle_Tbl v on pdv.VEHICLE_NO = v.Vehicle_No            
			                                                            and pdv.SCHEDULE_DATE >='{0}'
			                                                            and pdv.SCHEDULE_DATE <='{1}'
                                                    )";
                SQLDeleteString = string.Format(SQLDeleteString, DateUtility.ConvertDateForSQLPurpose(dateStart), DateUtility.ConvertDateForSQLPurpose(dateEnd));
                SqlCommand cmd = new SqlCommand(SQLDeleteString, con);
                cmd.Transaction = tran;
                deletedCount = cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static void InsertTrailerLocation(TrailerLocation trailerlocation, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = "INSERT INTO TPT_TRAILER_LOCATION_TBL (ScheduleDate,TrailerNo,StartTime,EndTime,DateChange,TrailerStatus,StartStop,StopCode,ParkStopCode,DriverNo,PlanTripNo,Remarks) VALUES ('"
                   + DateUtility.ConvertDateAndTimeForSQLPurpose(trailerlocation.ScheduleDate.Date);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.TrailerNo.Trim());
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.StartTime);
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(trailerlocation.EndTime);
                SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(DateTime.Now);
                SQLInstertString += "'" + "," + "'" + trailerlocation.TrailerStatus.GetHashCode().ToString();
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.StartStop.Trim());
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.StopCode.Trim());
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.ParkStop.Trim());
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.DriverNo);
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.PlanTripNo.Trim());
                SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.Remarks) + "')";
                SqlCommand cmd1 = new SqlCommand(SQLInstertString, con);
                cmd1.Transaction = tran;
                cmd1.CommandTimeout = 0;
                cmd1.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException sqlEx) { throw new FMException(sqlEx.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static List<TrailerLocation> GetAllTrailersBeingUsedInPlanning(DateTime dateStart, DateTime dateEnd, SqlConnection con, SqlTransaction tran)
        {
            List<TrailerLocation> trailerlocations = new List<TrailerLocation>();
            try
            {
                string SQLSelectString = @"select distinct pt.SCHEDULE_DATE, 
                                            pst.START_TIME, 
                                            pst.END_TIME, 
                                            pst.TRAILER_NO, 
                                            pst.START_STOP, 
                                            pst.END_STOP, 
                                            pt.DRIVER_NO, 
                                            pst.PLANTRIP_NO
                                            , (case (select top 1 jt.STATUS from TPT_JOB_DETAIL_CARGO_Tbl jt
				                                            inner join TPT_PLAN_SUBTRIP_JOB_TBL pstj on pstj.JOB_ID = jt.JOB_ID and pstj.SEQUENCE_NO = jt.SEQUENCE_NO 
				                                            where pstj.PLANTRIP_NO = pst.PLANTRIP_NO and pstj.SEQ_NO = pst.SEQ_NO)
	                                            when 4 then 4 else 1
	                                            end) as TRAILER_STATUS 
                                            from TPT_PLAN_SUBTRIP_TBL pst
                                            inner join TPT_PLAN_TRIP_TBL pt on pt.PLANTRIP_NO = pst.PLANTRIP_NO
                                            inner join TPT_PLAN_DEPT_VEHICLE_TBL pdv on pdv.SCHEDULE_DATE = pt.SCHEDULE_DATE and pdv.VEHICLE_NO = pst.TRAILER_NO
                                            inner join TPT_PLAN_DEPT_TBL pd on pd.SCHEDULE_DATE = pt.SCHEDULE_DATE
                                            where pst.TRAILER_NO <> ''
			                                and pt.SCHEDULE_DATE >='{0}'
			                                and pt.SCHEDULE_DATE <='{1}'
                                            ";

                SQLSelectString = string.Format(SQLSelectString, DateUtility.ConvertDateForSQLPurpose(dateStart), DateUtility.ConvertDateForSQLPurpose(dateEnd));
                SqlCommand cmd = new SqlCommand(SQLSelectString, con);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    trailerlocations.Add(GetTrailerLocationNew(reader));
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return trailerlocations;
        }
        internal static TrailerLocation GetTrailerLocationNew(IDataReader reader)
        {
            TrailerStatus t = new TrailerStatus();
            int status = (int)reader["TRAILER_STATUS"];
            if (status == 1) { t = TrailerStatus.Assigned; }
            if (status == 2) { t = TrailerStatus.CustomerStuff; }
            if (status == 3) { t = TrailerStatus.CustomerUnstuff; }
            if (status == 4) { t = TrailerStatus.TrailerPark; }
            if (status == 5) { t = TrailerStatus.Unavailable; }
            if (status == 6) { t = TrailerStatus.CompleteStuffUnstuff; }
            try
            {
                TrailerLocation trailerlocation = new TrailerLocation();
                trailerlocation.TrailerNo = (string)reader["TRAILER_NO"];
                trailerlocation.ScheduleDate = (DateTime)reader["SCHEDULE_DATE"];
                trailerlocation.ChangeDate = (DateTime)reader["START_TIME"];
                trailerlocation.StartTime = (DateTime)reader["START_TIME"];
                trailerlocation.EndTime = (DateTime)reader["END_TIME"];
                trailerlocation.TrailerStatus = t;
                trailerlocation.StartStop = reader["START_STOP"] == DBNull.Value ? string.Empty : (string)reader["START_STOP"];
                trailerlocation.StopCode = reader["END_STOP"] == DBNull.Value ? string.Empty : (string)reader["END_STOP"];
                trailerlocation.ParkStop = t == TrailerStatus.Assigned ? string.Empty : (string)reader["END_STOP"];
                trailerlocation.PlanTripNo = reader["PLANTRIP_NO"] == DBNull.Value ? string.Empty : (string)reader["PLANTRIP_NO"];
                trailerlocation.DriverNo = reader["DRIVER_NO"] == DBNull.Value ? string.Empty : (string)reader["DRIVER_NO"];
                return trailerlocation;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static bool IsTrailerLocationAlreadyInserted(string driverCode, DateTime startDateTime, string trailerNo, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLSelectString = @"SELECT count(*) from TPT_TRAILER_LOCATION_TBL where TrailerNo = '{0}' and StartTime ='{1}'";

                SQLSelectString = string.Format(SQLSelectString, CommonUtilities.FormatString(trailerNo), DateUtility.ConvertDateAndTimeForSQLPurpose(startDateTime));

                SqlCommand cmd = new SqlCommand(SQLSelectString, con);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                object temp = cmd.ExecuteScalar();
                if (temp != null)
                {
                    if ((int)temp == 0) { return false; }
                    else return true;
                }
            }
            catch (FMException fmEx)
            {
                Console.WriteLine(fmEx.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            return false;

        }
        #endregion
    }
}
