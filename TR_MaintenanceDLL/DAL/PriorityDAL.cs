using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_MaintenanceDLL.DAL
{
    internal class PriorityDAL
    {
        internal static SortableList<JobAssignmentPriority> GetAllJobAssignmentPriority()
        {
            SortableList<JobAssignmentPriority> retValue = new SortableList<JobAssignmentPriority>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL ORDER BY Priority_No";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    JobAssignmentPriority prior = new JobAssignmentPriority();
                    prior.Code = reader["Code"] == DBNull.Value ? string.Empty : (string)reader["Code"];
                    prior.Description = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"];
                    prior.sched_Priority = reader["Priority_No"] == DBNull.Value ? 0 : (int)reader["Priority_No"];
                    switch (prior.sched_Priority)
                    { 
                        case 1 :
                            prior.priorityColor = Color.Tan;
                            break;
                        case 2:
                            prior.priorityColor = Color.LightSkyBlue;
                            break;
                        case 3:
                            prior.priorityColor = Color.LightGray;
                            break;
                        case 4:
                            prior.priorityColor = Color.LightYellow;
                            break;
                        case 5:
                            prior.priorityColor = Color.Coral;
                            break;
                    }
                    retValue.Add(prior);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;
        }
        internal static bool AddJobAssignmentPriority(JobAssignmentPriority jobAssignmentPriority, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"Insert into TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL(Code, Description, Priority_No)
                                    VALUES('{0}', '{1}', {2})";

                query = string.Format(query, jobAssignmentPriority.Code, jobAssignmentPriority.Description, jobAssignmentPriority.sched_Priority);
                
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction =tran;
                
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static bool DeleteJobAssignmentPriority(JobAssignmentPriority jobAssignmentPriority, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"DELETE FROM TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL WHERE Code ='{0}'
                                DELETE FROM TPT_PLAN_DRIVER_PRIORITY_Tbl WHERE Priority_Code ='{0}'";

                query = string.Format(query, jobAssignmentPriority.Code);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static bool EditJobAssignmentPriority(JobAssignmentPriority jobAssignmentPriority, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"UPDATE TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL SET Description='{1}' WHERE Code ='{0}'";

                query = string.Format(query, jobAssignmentPriority.Code, 
                                            CommonUtilities.FormatString(jobAssignmentPriority.Description));

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static bool ReArrangePriorities(JobAssignmentPriority jobAssignmentPriority, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"UPDATE TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL SET Priority_No = {1} WHERE Code ='{0}'";

                query = string.Format(query, jobAssignmentPriority.Code, jobAssignmentPriority.sched_Priority);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }


        private static DriverPriority GetDriverPriority(IDataReader reader)
        {
            DriverPriority prior = new DriverPriority();
            try
            {
                prior.DriverCode = reader["DriverCode"] == DBNull.Value ? string.Empty : (string)reader["DriverCode"];
                prior.DriverName = reader["Driver_Name"] == DBNull.Value ? string.Empty : (string)reader["Driver_Name"];
                //prior.DefaultVehicle = reader["VehicleNo"] == DBNull.Value ? string.Empty : (string)reader["VehicleNo"];
                prior.DefaultVehicle = reader["Default_Vehicle"] == DBNull.Value ? string.Empty : (string)reader["Default_Vehicle"];
                prior.ScheduleDate = reader["PrioritySchedule_Date"] == DBNull.Value ? DateTime.Now : (DateTime)reader["PrioritySchedule_Date"];
                prior.Prior_Code = reader["Priority_Code"] == DBNull.Value ? string.Empty : (string)reader["Priority_Code"];
                prior.Prior_Desc = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"];
            }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return prior;
        }
        internal static SortableList<DriverPriority> GetJobAssignmentPriorityByDate(DateTime scheduleDateFrom, DateTime scheduleDateTo)
        {
            SortableList<DriverPriority> retValue = new SortableList<DriverPriority>();
            try
            {
                string query = @"SELECT a.*,b.*, d.Driver_Name, d.Default_Vehicle FROM TPT_PLAN_DRIVER_PRIORITY_Tbl a
                                    inner join TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL b
                                    on a.Priority_Code = b.Code 
                                    inner join TPT_Driver_Tbl d
                                    on d.Driver_Code = a.DriverCode
                                        WHERE a.PrioritySchedule_Date >= '{0}' AND a.PrioritySchedule_Date <='{1}'
                                        Order by b.Priority_No, a.VehicleNo";
                query = string.Format(query, 
                            DateUtility.ConvertDateForSQLPurpose(scheduleDateFrom),
                            DateUtility.ConvertDateForSQLPurpose(scheduleDateTo));

                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                SqlCommand cmd = new SqlCommand(query, cn);
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue.Add(GetDriverPriority(reader));
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
     
        internal static SortableList<DriverPriority> GetAllJobAssignmentPriority(string driverCode, DateTime scheduleDate)
        {
            SortableList<DriverPriority> retValue = new SortableList<DriverPriority>();
            try
            {
                string query = @"SELECT a.*,b.*, d.Driver_Name, d.Default_Vehicle FROM TPT_PLAN_DRIVER_PRIORITY_Tbl a
                                    inner join TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL b
                                    on a.Priority_Code = b.Code 
                                    inner join TPT_Driver_Tbl d
                                    on d.Driver_Code = a.DriverCode
                                    WHERE DriverCode ='" + driverCode + "'";
                if (scheduleDate != DateUtility.GetSQLDateTimeMinimumValue())
                    query += " AND a.PrioritySchedule_Date =" + DateUtility.ConvertDateForSQLPurpose(scheduleDate);

                query += " Order by PrioritySchedule_Date desc";
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                SqlCommand cmd = new SqlCommand(query, cn);
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue.Add(GetDriverPriority(reader));
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static SortableList<DriverPriority> GetAllJobAssignmentPriority(string driverCode, DateTime scheduleDateFrom, DateTime scheduleDateTo)
        {
            SortableList<DriverPriority> retValue = new SortableList<DriverPriority>();
            try
            {
                string query = @"SELECT a.*,b.*, d.Driver_Name, d.Default_Vehicle FROM TPT_PLAN_DRIVER_PRIORITY_Tbl a
                                            inner join TPT_DRIVER_JOB_ASSIGNMENT_PRIORITY_TBL b
                                            on a.Priority_Code = b.Code 
                                            inner join TPT_Driver_Tbl d
                                            on d.Driver_Code = a.DriverCode
                                        WHERE a.DriverCode ='{0}'
                                            AND a.PrioritySchedule_Date >= '{1}' AND a.PrioritySchedule_Date <='{2}'
                                        Order by a.PrioritySchedule_Date desc"; // +DateUtility.ConvertDateForSQLPurpose(scheduleDate);

                query = string.Format(query,driverCode,
                                            DateUtility.ConvertDateForSQLPurpose(scheduleDateFrom), 
                                            DateUtility.ConvertDateForSQLPurpose(scheduleDateTo));
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue.Add(GetDriverPriority(reader));
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;
        }
        internal static bool AddDriverPriority(DriverPriority driverPriority, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"Insert into TPT_PLAN_DRIVER_PRIORITY_Tbl(DriverCode, VehicleNo, PrioritySchedule_Date, Priority_Code)
                                    VALUES('{0}', '{1}', '{2}', '{3}')";

                query = string.Format(query, driverPriority.DriverCode,
                                        driverPriority.DefaultVehicle, 
                                        DateUtility.ConvertDateForSQLPurpose(driverPriority.ScheduleDate),
                                        driverPriority.Prior_Code);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static bool EditDriverPriority(DriverPriority driverPriority, string newPriorCode, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"update TPT_PLAN_DRIVER_PRIORITY_Tbl
                                    set Priority_Code = '{2}'
                                    where DriverCode = '{0}'
                                    AND PrioritySchedule_Date = '{1}'";

                query = string.Format(query, driverPriority.DriverCode,
                                        DateUtility.ConvertDateForSQLPurpose(driverPriority.ScheduleDate),
                                        newPriorCode);

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static bool DeleteDriverPriority(DriverPriority driverPriority, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"DELETE FROM TPT_PLAN_DRIVER_PRIORITY_Tbl WHERE DriverCode = '{0}' AND PrioritySchedule_Date ='{1}'";

                query = string.Format(query, driverPriority.DriverCode,
                                            DateUtility.ConvertDateForSQLPurpose(driverPriority.ScheduleDate));

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
             
        internal static bool DeleteDriverPriority(DateTime scheduleDate, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"DELETE FROM TPT_PLAN_DRIVER_PRIORITY_Tbl WHERE PrioritySchedule_Date = '{0}'";

                query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(scheduleDate));

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static bool HasDriverPriorityForTheDay(DriverPriority driverPriority)
        {
            bool retValue = false;
            try
            {
                string query = @"SELECT * FROM TPT_PLAN_DRIVER_PRIORITY_Tbl
                                     WHERE DriverCode = '{0}' AND PrioritySchedule_Date ='{1}'"; 

                query = string.Format(query, driverPriority.DriverCode,
                                            DateUtility.ConvertDateForSQLPurpose(driverPriority.ScheduleDate));
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                SqlCommand cmd = new SqlCommand(query, cn);     
                object obj = cmd.ExecuteScalar();
                if (obj == null)
                    retValue = false;
                else
                    retValue = true;

                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue; 
        }

    }
}
