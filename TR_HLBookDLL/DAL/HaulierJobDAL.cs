using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FM.TR_HLBookDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using TR_LanguageResource.Resources;
using System.Collections;
using System.Diagnostics;
using FM.TR_SeaFreightDLL.BLL;
using FM.TR_HLBookDLL.Facade;

namespace FM.TR_HLBookDLL.DAL
{
    internal class HaulierJobDAL
    {
        const string LAST_MINUTE_OF_DAY = " 23:59:00";

        internal static bool AreThereInvoices(string jobNo)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                cn.Open();
                string SQLSearchString = "SELECT * FROM ACT_IV_Transport_Invoice_Master_Tbl with (NOLOCK) where Job_Number = '" + jobNo + "'";

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
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : AreThereInvoices. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : AreThereInvoices. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : AreThereInvoices. " + ex.Message); }
        }

        internal static bool FindDuplicateJobCharge(int jobID, string chargeCode)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                cn.Open();
                string SQLSearchString = "SELECT * FROM TPT_JOB_DETAIL_CHARGE_Tbl with (NOLOCK) where JOB_ID = " + jobID;
                SQLSearchString += " AND CHARGE_CODE = '" + chargeCode + "'";

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
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : FindDuplicateJobCharge. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : FindDuplicateJobCharge. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : FindDuplicateJobCharge. " + ex.Message); }
        }
        internal static SortableList<JobTripState> GetAllHaulierJobTripStates(HaulierJobTrip haulierjobtrip, SqlConnection cn, SqlTransaction tran)
        {
            SortableList<JobTripState> Operators = new SortableList<JobTripState>();
            try
            {
                string SQLString = "SELECT * FROM TPT_JOB_TRIP_STATE_Tbl with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + haulierjobtrip.JobID;
                SQLString += " AND SEQUENCE_NO = " + haulierjobtrip.Sequence;

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetHaulierJobTripState(reader));
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripStates. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripStates. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripStates. " + ex.Message); }
            return Operators;
        }
        internal static SortableList<JobTripState> GetAllHaulierJobTripStates(HaulierJobTrip haulierjobtrip)
        {
            SortableList<JobTripState> Operators = new SortableList<JobTripState>();
            try
            {
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT * FROM TPT_JOB_TRIP_STATE_Tbl with (NOLOCK) ";
                    SQLString += "WHERE JOB_ID = " + haulierjobtrip.JobID;
                    SQLString += " AND SEQUENCE_NO = " + haulierjobtrip.Sequence;

                    cn.Open();
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Operators.Add(GetHaulierJobTripState(reader));
                    }
                    cn.Close();
                }
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripStates. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripStates. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripStates. " + ex.Message); }
            return Operators;
        }

        internal static JobTripState GetHaulierJobTripState(IDataReader reader)
        {
            JobTripStatus jobstatus = JobTripStatus.Booked;
            int temp = Convert.ToInt32(reader["STATUS"]);
            switch (temp)
            {
                case 1:
                    jobstatus = JobTripStatus.Booked;
                    break;
                case 2:
                    jobstatus = JobTripStatus.Ready;
                    break;
                case 3:
                    jobstatus = JobTripStatus.Assigned;
                    break;
                case 4:
                    jobstatus = JobTripStatus.Completed;
                    break;
                case 5:
                    jobstatus = JobTripStatus.Invoiced;
                    break;

            }

            // First set IsNew = true first so that the properties validation can pass

            JobTripState jobTripState = new JobTripState(
                (int)reader["JOB_STATE_NO"],
                jobstatus,
                (DateTime)reader["STATUSDATE"],
                (string)reader["REMARKS"], true);

            // Now after object is constructed, set isNew = false

            jobTripState.IsNew = false;

            return jobTripState;

        }

        internal static SortableList<HaulierJobTrip> GetAllHaulierJobTrips(HaulierJob haulierJob)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            SortableList<HaulierJobTrip> Operators = new SortableList<HaulierJobTrip>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                cn.Open();
                SqlTransaction dbTran = cn.BeginTransaction();
                Operators = GetAllHaulierJobTrips(haulierJob, cn, dbTran);
                dbTran.Commit();
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            return Operators;
        }

        // 2014-12-01 Zhou Kai adds
        internal static SortableList<HaulierJobTrip> GetAllHaulierJobTrips(HaulierJob haulierJob,
            SqlConnection dbCon, SqlTransaction dbTran)
        {
            SortableList<HaulierJobTrip> Operators = new SortableList<HaulierJobTrip>();
            try
            {
                //20161215 - gerry modified query
                string SQLString = @"SELECT b.UPDATE_VERSION AS UPD_VER,
                                    convert(varchar, b.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = b.JOB_ID and a.LEG_GROUP = b.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = b.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = jm.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl b with (NOLOCK)
                                    LEFT JOIN TPT_JOB_MAIN_Tbl jm with (NOLOCK) ON b.JOB_ID=jm.JOB_ID  
                                    LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl c with (NOLOCK) ON jm.JOB_ID=c.JOB_ID 
                                    WHERE b.JOB_ID = {0}";
                SQLString = string.Format(SQLString, haulierJob.JobID);

                SqlCommand dbCmd = new SqlCommand(SQLString, dbCon);
                dbCmd.CommandTimeout = 0;
                dbCmd.Transaction = dbTran;
                IDataReader reader = dbCmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip trip = GetHaulierJobTrip(reader);
                    trip.JobTripStates = GetAllHaulierJobTripStates(trip);
                    Operators.Add(trip);

                    //Operators.Add(GetHaulierJobTrip(reader));
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            return Operators;
        }


        internal static SortableList<HaulierJobTrip> GetAllHaulierJobTrips()
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl

            SortableList<HaulierJobTrip> Operators = new SortableList<HaulierJobTrip>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_Tbl with (NOLOCK) ON TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID=TPT_JOB_MAIN_Tbl.JOB_ID ";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK) ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID ";

                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip trip = GetHaulierJobTrip(reader);
                    trip.JobTripStates = GetAllHaulierJobTripStates(trip);
                    Operators.Add(trip);

                    //Operators.Add(GetHaulierJobTrip(reader));
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTrips. " + ex.Message); }
            finally { cn.Close(); }
            return Operators;
        }

        internal static HaulierJobTrip GetHaulierJobTrip(int jobid, int sequence, SqlConnection cn, SqlTransaction tran)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            HaulierJobTrip Operator = new HaulierJobTrip();
            try
            {
                string SQLString = @"SELECT b.UPDATE_VERSION AS UPD_VER,
                                    convert(varchar, b.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a  with (NOLOCK)
																where (a.JOB_ID = b.JOB_ID and a.LEG_GROUP = b.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = b.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = jm.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl b with (NOLOCK)
                                    LEFT JOIN TPT_JOB_MAIN_Tbl jm with (NOLOCK) ON b.JOB_ID=jm.JOB_ID  
                                    LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl c with (NOLOCK) ON jm.JOB_ID=c.JOB_ID 
                                    WHERE b.JOB_ID = {0} AND b.SEQUENCE_NO = {1}";
                SQLString = string.Format(SQLString, jobid, sequence);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetHaulierJobTrip(reader);
                    Operator.LegTypeCustomized = reader["Leg"] == DBNull.Value ? string.Empty : (string)reader["Leg"];
                }
                reader.Close();
                Operator.JobTripStates = GetAllHaulierJobTripStates(Operator, cn, tran);
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }
            return Operator;
        }

        internal static HaulierJobTrip GetHaulierJobTrip(int jobid, int sequence)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            HaulierJobTrip Operator = new HaulierJobTrip();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT b.UPDATE_VERSION AS UPD_VER,
                                    convert(varchar, b.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a  with (NOLOCK) 
																where (a.JOB_ID = b.JOB_ID and a.LEG_GROUP = b.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = b.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = jm.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl b with (NOLOCK)
                                    LEFT JOIN TPT_JOB_MAIN_Tbl jm with (NOLOCK) ON b.JOB_ID=jm.JOB_ID  
                                    LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl c with (NOLOCK) ON jm.JOB_ID=c.JOB_ID
                                    WHERE b.JOB_ID = {0} AND b.SEQUENCE_NO = {1}";
                SQLString = string.Format(SQLString, jobid, sequence);
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetHaulierJobTrip(reader);
                    Operator.JobTripStates = GetAllHaulierJobTripStates(Operator);
                    Operator.LegTypeCustomized = reader["Leg"] == DBNull.Value ? string.Empty : (string)reader["Leg"];
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }
            return Operator;
        }

        internal static SortableList<HaulierJobTrip> GetAllReadyHaulierJobTrips()
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl

            SortableList<HaulierJobTrip> Operators = new SortableList<HaulierJobTrip>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_Tbl with (NOLOCK) ON TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID=TPT_JOB_MAIN_Tbl.JOB_ID ";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK) ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID ";
                SQLString += "WHERE STATUS = " + JobTripStatus.Ready.GetHashCode();
                //            SQLString += "WHERE TPT_JOB_DETAIL_CARGO_Tbl.STATUS = 2";

                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                    jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                    Operators.Add(jobTrip);

                    //Operators.Add(GetHaulierJobTrip(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllReadyHaulierJobTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllReadyHaulierJobTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllReadyHaulierJobTrips. " + ex.Message); }
            return Operators;
        }

        internal static SortableList<HaulierJobTrip> GetAllReadyHaulierJobTripsAndNotSubCon()
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl

            SortableList<HaulierJobTrip> Operators = new SortableList<HaulierJobTrip>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_Tbl with (NOLOCK) ON TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID=TPT_JOB_MAIN_Tbl.JOB_ID ";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK) ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID ";
                SQLString += "WHERE STATUS = " + JobTripStatus.Ready.GetHashCode();
                SQLString += " AND TPT_JOB_DETAIL_CARGO_Tbl.OWNTRANSPORT = 'T'";
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                    jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                    Operators.Add(jobTrip);

                    //Operators.Add(GetHaulierJobTrip(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllReadyHaulierJobTripsAndNotSubCon. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllReadyHaulierJobTripsAndNotSubCon. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllReadyHaulierJobTripsAndNotSubCon. " + ex.Message); }
            return Operators;
        }

        internal static SortableList<HaulierJobTrip> GetAllJobTripInReadyStatusByDept(string deptCode, DateTime scheduleDate, bool ownTransport)
        {
            SortableList<HaulierJobTrip> retValue = new SortableList<HaulierJobTrip>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT b.UPDATE_VERSION AS UPD_VER,
                                    convert(varchar, b.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = b.JOB_ID and a.LEG_GROUP = b.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = b.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = jm.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl  b with (NOLOCK)
                                    LEFT JOIN TPT_JOB_MAIN_Tbl jm with (NOLOCK) ON b.JOB_ID=jm.JOB_ID 
                                    LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl jsi with (NOLOCK) ON jm.JOB_ID=jsi.JOB_ID 
                                    WHERE b.STATUS = 2  
                                    AND jm.TPT_DEPT_CODE = '{0}' 
                                    AND b.OWNTRANSPORT = '{1}'
                                    AND b.FROM_DATE <= '{2}' 
                                    ORDER BY b.LEG_TYPE,b.FROM_DATE,  b.FROM_TIME, jm.JOB_NUMBER";
                SQLString = string.Format(SQLString, CommonUtilities.FormatString(deptCode), 
                                                    ownTransport ? "T" : "F",
                                                    DateUtility.ConvertDateForSQLPurpose(scheduleDate));

                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                    jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                    jobTrip.LegTypeCustomized = reader["Leg"] == DBNull.Value ? string.Empty : reader["Leg"].ToString();
                    retValue.Add(jobTrip);
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllJobTripInReadyStatusByDept. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllJobTripInReadyStatusByDept. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllJobTripInReadyStatusByDept. " + ex.Message); }
            return retValue;
        }
        //added filter jobNo, containerNo, source reference No
        internal static SortableList<HaulierJobTrip> GetAllJobTripInReadyStatusByDept(string deptCode, DateTime scheduleDate, bool ownTransport,
                            string jobNo, string containerNo, string sourceRefNo, string rowCount, DateTime? advanceJobDate = null)
        {
            SortableList<HaulierJobTrip> retValue = new SortableList<HaulierJobTrip>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT TOP {2} b.UPDATE_VERSION AS UPD_VER,
                                    convert(varchar, b.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = b.JOB_ID and a.LEG_GROUP = b.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = b.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = jm.JOB_NUMBER)) Leg,
                                    
                                    *
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl  b with (NOLOCK)
                                    LEFT JOIN TPT_JOB_MAIN_Tbl jm with (NOLOCK) ON b.JOB_ID=jm.JOB_ID 
                                    LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl jmsi with (NOLOCK) ON jm.JOB_ID=jmsi.JOB_ID 
                                    WHERE b.STATUS = 2 AND b.Is_Movement_Job = 'T'
                                    AND jm.TPT_DEPT_CODE = '{0}' 
                                    AND b.OWNTRANSPORT = '{1}' ";
                SQLString = string.Format(SQLString, CommonUtilities.FormatString(deptCode),
                                                    ownTransport ? "T" : "F",
                                                    rowCount == string.Empty ? "100" : rowCount);

                string filter = string.Empty;
                #region filter based on criteria
                if (advanceJobDate == null) { filter += " AND b.FROM_DATE <= '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'"; }
                else { filter += " AND b.FROM_DATE = '" + DateUtility.ConvertDateForSQLPurpose((DateTime)advanceJobDate) + "'"; }

                if (!jobNo.Equals(string.Empty)) { filter += " AND jm.JOB_NUMBER like '%" + CommonUtilities.FormatString(jobNo) + "%'"; }
                if (!containerNo.Equals(string.Empty)) { filter += " AND b.CONTAINER_NUMBER like '%" + CommonUtilities.FormatString(containerNo) + "%'"; }
                if (!sourceRefNo.Equals(string.Empty)) { filter += " AND jm.SOURCE_REF_NUMBER like '%" + CommonUtilities.FormatString(sourceRefNo) + "%'"; }
                #endregion

                //if (filter == string.Empty)
                //    filter += " AND b.FROM_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "' ";

                SQLString += filter;
                SQLString += @" ORDER BY b.LEG_TYPE,b.FROM_DATE,  b.FROM_TIME, jm.JOB_NUMBER";

                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                    jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);

                    jobTrip.LegTypeCustomized = reader["Leg"] == DBNull.Value ? string.Empty : reader["Leg"].ToString();
                    retValue.Add(jobTrip);
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllJobTripInReadyStatusByDept. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllJobTripInReadyStatusByDept. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllJobTripInReadyStatusByDept. " + ex.Message); }
            return retValue;
        }
        //20160815 - gerry added
        internal static HaulierJobCharge GetHaulierJobCharge(int jobID, int seqNo, SqlConnection cn, SqlTransaction tran)
        {
            HaulierJobCharge Operator = new HaulierJobCharge();
            try
            {
                string SQLString = "SELECT * FROM TPT_JOB_DETAIL_CHARGE_Tbl with (NOLOCK)";
                SQLString += "  WHERE JOB_ID = " + jobID;
                SQLString += "  AND JOB_ID = " + seqNo;
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetHaulierJobCharge(reader);
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            return Operator;
        }
        internal static SortableList<HaulierJobCharge> GetAllHaulierJobCharges(HaulierJob haulierJob)
        {
            SortableList<HaulierJobCharge> Operators = new SortableList<HaulierJobCharge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_JOB_DETAIL_CHARGE_Tbl with (NOLOCK)";
                SQLString += " WHERE JOB_ID = " + haulierJob.JobID;
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetHaulierJobCharge(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            return Operators;
        }

        /// <summary>
        /// 2014-08-12 Zhou Kai adds comments to this function:
        /// The function is to get all HaulierJobCharges, either container_movement_charges or
        /// service_charges(or non-movement charges)
        /// </summary>
        /// <param name="JobID">The id of the job the function is working on</param>
        /// <param name="isContainerMovement">True, to get container_movement_charges; False, to get service_charges</param>
        /// <returns>The SortableList of all the valid HaulierJobCharges</returns>
        internal static SortableList<HaulierJobCharge> GetAllHaulierJobCharges(int JobID, bool isContainerMovement)
        {
            SortableList<HaulierJobCharge> Operators = new SortableList<HaulierJobCharge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_JOB_DETAIL_CHARGE_Tbl,TPT_CHARGE_TBL with (NOLOCK)";
                SQLString += " WHERE JOB_ID = " + JobID;
                SQLString += " AND TPT_JOB_DETAIL_CHARGE_Tbl.CHARGE_CODE = TPT_CHARGE_TBL.CHARGE_CODE";
                if (isContainerMovement)
                {
                    SQLString += " AND TPT_CHARGE_TBL.IS_CONTAINER_MOVEMENT = 'T'";
                }
                else
                {
                    SQLString += " AND TPT_CHARGE_TBL.IS_CONTAINER_MOVEMENT = 'F'";
                }

                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetHaulierJobCharge(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobCharges. " + ex.Message); }
            return Operators;

        }

        internal static SortableList<HaulierJobCharge> GetAllHaulierJobChargesBasedOnType(HaulierJob haulierJob, string jobChargeType)
        {
            SortableList<HaulierJobCharge> Operators = new SortableList<HaulierJobCharge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_JOB_DETAIL_CHARGE_Tbl with (NOLOCK)";
                SQLString += " WHERE JOB_ID = " + haulierJob.JobID;
                SQLString += " AND JOB_CHARGE_TYPE = '" + jobChargeType + "'";
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetHaulierJobCharge(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobChargesBasedOnType. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobChargesBasedOnType. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobChargesBasedOnType. " + ex.Message); }
            return Operators;

        }

        internal static HaulierJobCharge GetHaulierJobCharge(IDataReader reader)
        {

            int temp = (int)reader["JOB_CHARGE_STATUS"];
            JobChargeStatus status = JobChargeStatus.Booked;
            if (temp == 1)
                status = JobChargeStatus.Booked;
            if (temp == 2)
                status = JobChargeStatus.Completed;
            if (temp == 3)
                status = JobChargeStatus.Invoiced;


            HaulierJobCharge haulierJobCharge = new HaulierJobCharge(
                (int)reader["INV_TRAN_NO"],
                Convert.ToInt32(reader["INV_ITEM_SEQ_NO"]),
                (int)reader["JOB_ID"],
                (int)reader["SEQUENCE_NO"],
                (string)reader["CHARGE_CODE"],
                (string)reader["DESCRIPTION"],
                (string)reader["UOM"],
                (decimal)reader["QUANTITY"],
                (decimal)reader["UNIT_RATE"],
                (decimal)reader["AMOUNT_HC"],
                (decimal)reader["AMOUNT_FC"],
                (string)reader["GST_TYPE"],
                (decimal)reader["GST_RATE"],
                //(decimal)reader["GST_AMOUNT_HC"],  //Gerry change, we will take GST_AMOUNT_FC instead of HC amount because it will recalculate in setter method
                (decimal)reader["GST_AMOUNT_FC"],
                (string)reader["CURRENCY"],
                (decimal)reader["EXCHANGE_RATE"],
                (string)reader["REMARKS"],
                (string)reader["QUOTATION_NO"],
                (int)reader["QUOTATION_ID"],
                (int)reader["SEQUENCE_NO_RATE"],
                (string)reader["JOB_CHARGE_TYPE"],
                (DateTime)reader["COMPLETED_DATE"],
                status,
                (string)reader["RATE_TYPE"],
                (byte[])reader["UPDATE_VERSION"],
                (string)reader["CONTAINER_NUMBER"]
                );

            haulierJobCharge.OldJobChargeStatus = status;
            return haulierJobCharge;
        }

        internal static SortableList<HaulierJob> GetAllHaulierJobs()
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            SortableList<HaulierJob> Operators = new SortableList<HaulierJob>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_JOB_MAIN_Tbl with (NOLOCK) LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID";
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetHaulierJob(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobs. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobs. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobs. " + ex.Message); }
            return Operators;
        }

        internal static HaulierJob GetHaulierJob(string JobNo)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            HaulierJob Operator = new HaulierJob();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_JOB_MAIN_Tbl with (NOLOCK) LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID";
                SQLString += " WHERE TPT_JOB_MAIN_Tbl.JOB_NUMBER='" + CommonUtilities.FormatString(JobNo) + "'";
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetHaulierJob(reader);
                }
                // 2014-10-04 Zhou Kai adds
                // Operator.PartnerJobTrips = GetPartnerTrips(Operator);
                // 2014-10-04 Zhou Kai ends

                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }
            return Operator;
        }
        internal static SortableList<HaulierJob> GetAllBookedHaulierJobsFromDatabase()
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            SortableList<HaulierJob> Operators = new SortableList<HaulierJob>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_JOB_MAIN_Tbl with (NOLOCK) LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID";
                SQLString += " WHERE TPT_JOB_MAIN_Tbl.JOB_STATUS=1";

                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetHaulierJob(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllBookedHaulierJobsFromDatabase. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllBookedHaulierJobsFromDatabase. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllBookedHaulierJobsFromDatabase. " + ex.Message); }
            return Operators;
        }

        internal static HaulierJob GetHaulierJob(IDataReader reader)
        {
            JobStatus jobstatus = JobStatus.Booked;
            int temp = Convert.ToInt32(reader["JOB_STATUS"]);
            switch (temp)
            {
                case 1:
                    jobstatus = JobStatus.Booked;
                    break;
                case 2:
                    jobstatus = JobStatus.Cancelled;
                    break;
                case 3:
                    jobstatus = JobStatus.Billed;
                    break;
                case 4:
                    jobstatus = JobStatus.Completed;
                    break;
                case 5:
                    jobstatus = JobStatus.Closed;
                    break;
            }
            JobUrgencyType jobUrgencyType = JobUrgencyType.Normal;
            temp = Convert.ToInt32(reader["JOB_URGENCY_TYPE"]);
            switch (temp)
            {
                case 1:
                    jobUrgencyType = JobUrgencyType.Normal;
                    break;
                case 2:
                    jobUrgencyType = JobUrgencyType.Important;
                    break;
                case 3:
                    jobUrgencyType = JobUrgencyType.Urgent;
                    break;
            }

            bool billbytpt = reader["BILL_BY_TRANSPORT"] == DBNull.Value ? false : ((string)reader["BILL_BY_TRANSPORT"] == "T" ? true : false);
            string remarks2 = reader["REMARKS2"] == DBNull.Value ? string.Empty : (string)reader["REMARKS2"];
            string remarks3 = reader["REMARKS3"] == DBNull.Value ? string.Empty : (string)reader["REMARKS3"];
            string remarks4 = reader["REMARKS4"] == DBNull.Value ? string.Empty : (string)reader["REMARKS4"];
            string remarks5 = reader["REMARKS5"] == DBNull.Value ? string.Empty : (string)reader["REMARKS5"];
            ChargeType hchargetype = ChargeType.NotStopDependent;
            temp = Convert.ToInt32(reader["HAULIER_CHARGE_TYPE"]);
            switch (temp)
            {
                case 1:
                    hchargetype = ChargeType.NotStopDependent;
                    break;

                case 2:
                    hchargetype = ChargeType.StopDependent;
                    break;
            }
            SortableList<HaulierJobTrip> haulierJobTrips = new SortableList<HaulierJobTrip>();

            #region "2014-06-18 Zhou Kai adds"
            Stop stuffingAt = new Stop(reader["Stuffing_At"] == DBNull.Value ? string.Empty : reader["Stuffing_At"].ToString(),
                reader["Stuffing_At_Name"] == DBNull.Value ? string.Empty : reader["Stuffing_At_Name"].ToString(),
                reader["Stuffing_At_Address1"] == DBNull.Value ? string.Empty : reader["Stuffing_At_Address1"].ToString(),
                reader["Stuffing_At_Address2"] == DBNull.Value ? string.Empty : reader["Stuffing_At_Address2"].ToString(),
                reader["Stuffing_At_Address3"] == DBNull.Value ? string.Empty : reader["Stuffing_At_Address3"].ToString(),
                reader["Stuffing_At_Address4"] == DBNull.Value ? string.Empty : reader["Stuffing_At_Address4"].ToString(),
                reader["Stuffing_At_City"] == DBNull.Value ? string.Empty : reader["Stuffing_At_City"].ToString());
            Stop unStuffingAt = new Stop(reader["unStuffing_At"] == DBNull.Value ? string.Empty : reader["unStuffing_At"].ToString(),
                reader["UnStuffing_At_Name"] == DBNull.Value ? string.Empty : reader["UnStuffing_At_Name"].ToString(),
                reader["unStuffing_At_Address1"] == DBNull.Value ? string.Empty : reader["unStuffing_At_Address1"].ToString(),
                reader["unStuffing_At_Address2"] == DBNull.Value ? string.Empty : reader["unStuffing_At_Address2"].ToString(),
                reader["unStuffing_At_Address3"] == DBNull.Value ? string.Empty : reader["unStuffing_At_Address3"].ToString(),
                reader["unStuffing_At_Address4"] == DBNull.Value ? string.Empty : reader["unStuffing_At_Address4"].ToString(),
                reader["unStuffing_At_City"] == DBNull.Value ? string.Empty : reader["unStuffing_At_City"].ToString());
            Stop portAt = new Stop(reader["Port_Code"] == DBNull.Value ? string.Empty : reader["Port_Code"].ToString(),
                reader["Port_Name"] == DBNull.Value ? string.Empty : reader["Port_Name"].ToString(),
                reader["Port_At_Address1"] == DBNull.Value ? string.Empty : reader["Port_At_Address1"].ToString(),
                reader["Port_At_Address2"] == DBNull.Value ? string.Empty : reader["Port_At_Address2"].ToString(),
                reader["Port_At_Address3"] == DBNull.Value ? string.Empty : reader["Port_At_Address3"].ToString(),
                reader["Port_At_Address4"] == DBNull.Value ? string.Empty : reader["Port_At_Address4"].ToString(),
                reader["Port_At_City"] == DBNull.Value ? string.Empty : reader["Port_At_City"].ToString());
            Stop containerYard = new Stop(reader["Container_Yard_Code"] == DBNull.Value ? string.Empty : reader["Container_Yard_Code"].ToString(),
                reader["Container_Yard_Name"] == DBNull.Value ? string.Empty : reader["Container_Yard_Name"].ToString(),
                reader["Container_Yard_Address1"] == DBNull.Value ? string.Empty : reader["Container_Yard_Address1"].ToString(),
                reader["Container_Yard_Address2"] == DBNull.Value ? string.Empty : reader["Container_Yard_Address2"].ToString(),
                reader["Container_Yard_Address3"] == DBNull.Value ? string.Empty : reader["Container_Yard_Address3"].ToString(),
                reader["Container_Yard_Address4"] == DBNull.Value ? string.Empty : reader["Container_Yard_Address4"].ToString(),
                reader["Container_Yard_City"] == DBNull.Value ? string.Empty : reader["Container_Yard_City"].ToString());
            #endregion

            // 2014-10-07 Zhou Kai
            HaulierJob haulierJob = null;
            string ptnJobNo = reader["PARTNER_JOB_NO"] == DBNull.Value ? String.Empty : reader["PARTNER_JOB_NO"].ToString();
            JobTransferType jtsfType = reader["JOB_TRANSFER_TYPE"] == DBNull.Value ?  0 : (JobTransferType)Enum.Parse(typeof(JobTransferType), reader["JOB_TRANSFER_TYPE"].ToString());
            try // 2014-10-07 Zhou Kai adds try
            {
                haulierJob = new HaulierJob(
                   haulierJobTrips,
                   (int)reader["JOB_ID"],
                   reader["JOB_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["JOB_NUMBER"],
                   reader["CUST_CODE"] == DBNull.Value ? string.Empty : (string)reader["CUST_CODE"],
                   reader["SOURCE_REF_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["SOURCE_REF_NUMBER"],
                   jobstatus,
                   reader["JOB_TYPE"] == DBNull.Value ? string.Empty : (string)reader["JOB_TYPE"],
                   reader["BOOKING_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["BOOKING_NUMBER"],
                   (DateTime)reader["BOOKING_DATE"],
                   reader["QUOTATION_NO"] == DBNull.Value ? string.Empty : (string)reader["QUOTATION_NO"],
                   jobUrgencyType,
                   billbytpt,
                   reader["TPT_DEPT_CODE"] == DBNull.Value ? string.Empty : (string)reader["TPT_DEPT_CODE"],
                   reader["TPT_DEPT_CODE"] == DBNull.Value ? string.Empty : (string)reader["TPT_DEPT_CODE"],
                   reader["SHIPPER_CODE"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_CODE"],
                   reader["SHIPPER_NAME"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_NAME"],
                   reader["SHIPPER_ADD1"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_ADD1"],
                   reader["SHIPPER_ADD2"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_ADD2"],
                   reader["SHIPPER_ADD3"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_ADD3"],
                   reader["SHIPPER_ADD4"] == DBNull.Value ? string.Empty : (string)reader["SHIPPER_ADD4"],
                   reader["CONSIGNEE_CODE"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_CODE"],
                   reader["CONSIGNEE_NAME"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_NAME"],
                   reader["CONSIGNEE_ADD1"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD1"],
                   reader["CONSIGNEE_ADD2"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD2"],
                   reader["CONSIGNEE_ADD3"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD3"],
                   reader["CONSIGNEE_ADD4"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD4"],
                   reader["BRANCH_CODE"] == DBNull.Value ? string.Empty : (string)reader["BRANCH_CODE"],
                   (byte[])reader["UPDATE_VERSION"],
                   true,
                   reader["PSA_ACCOUNT"] == DBNull.Value ? string.Empty : (string)reader["PSA_ACCOUNT"],
                   reader["SUBCONTRACTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CODE"],
                   reader["SUBCONTRACTOR_NAME"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_NAME"],
                   reader["SUBCONTRACTOR_ADD1"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD1"],
                   reader["SUBCONTRACTOR_ADD2"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD2"],
                   reader["SUBCONTRACTOR_ADD3"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD3"],
                   reader["SUBCONTRACTOR_ADD4"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD4"],
                    // 2014-05-22 Zhou Kai adds
                   reader["SUBCONTRACTOR_CITY_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CITY_CODE"],
                    // 2014-05-22 Zhou Kai ends
                   reader["WAREHOUSE_NO"] == DBNull.Value ? string.Empty : (string)reader["WAREHOUSE_NO"],
                   reader["YOUR_REF_NO"] == DBNull.Value ? string.Empty : (string)reader["YOUR_REF_NO"],
                   reader["UCR_NO"] == DBNull.Value ? string.Empty : (string)reader["UCR_NO"],
                   reader["PERMIT_NO1"] == DBNull.Value ? string.Empty : (string)reader["PERMIT_NO1"],
                   reader["DATE_PERMIT_NO1"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["DATE_PERMIT_NO1"],
                   reader["PERMIT_NO2"] == DBNull.Value ? string.Empty : (string)reader["PERMIT_NO2"],
                   reader["DATE_PERMIT_NO2"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["DATE_PERMIT_NO2"],
                   reader["REMARKS"] == DBNull.Value ? string.Empty : (string)reader["REMARKS"],
                   remarks2,
                   remarks3,
                   remarks4,
                   remarks5,
                   reader["CARRIER_AGENT"] == DBNull.Value ? string.Empty : (string)reader["CARRIER_AGENT"],
                   reader["OBL_NO"] == DBNull.Value ? string.Empty : (string)reader["OBL_NO"],
                   reader["HBL_NO"] == DBNull.Value ? string.Empty : (string)reader["HBL_NO"],
                   reader["VESSEL_NO"] == DBNull.Value ? string.Empty : (string)reader["VESSEL_NO"],
                   reader["VESSEL_Name"] == DBNull.Value ? string.Empty : reader["VESSEL_Name"].ToString(),
                   reader["VOYAGE_NO"] == DBNull.Value ? string.Empty : (string)reader["VOYAGE_NO"],
                   reader["POL"] == DBNull.Value ? string.Empty : (string)reader["POL"],
                   reader["POL_Name"] == DBNull.Value ? string.Empty : reader["POL_Name"].ToString(),
                   reader["POD"] == DBNull.Value ? string.Empty : (string)reader["POD"],
                   reader["POD_Name"] == DBNull.Value ? string.Empty : reader["POD_Name"].ToString(),
                   reader["ETA"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["ETA"],
                   reader["ETD"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["ETD"],
                   hchargetype,
                    // 2014-07-03 Zhou Kai rechecks
                   reader["IsBillable"] == DBNull.Value ? false : (bool)reader["IsBillable"].ToString().Equals("T") ? true : false,
                   reader["Cargo_Description"] == DBNull.Value ? string.Empty : reader["Cargo_Description"].ToString(),
                   reader["Shipping_line"] == DBNull.Value ? string.Empty : reader["Shipping_line"].ToString(),
                   reader["Shipping_Line_Name"] == DBNull.Value ? string.Empty : reader["Shipping_Line_Name"].ToString(),
                   reader["Customer_reference"] == DBNull.Value ? string.Empty : reader["Customer_reference"].ToString(),
                   reader["Booking_reference"] == DBNull.Value ? string.Empty : reader["Booking_reference"].ToString(),
                   stuffingAt,
                   unStuffingAt,
                  reader["PortNet_Ref"] == DBNull.Value ? string.Empty : reader["PortNet_Ref"].ToString(),
                   portAt,
                   containerYard
                    // 2014-07-03 Zhou Kai ends
                    , ptnJobNo
                    , jtsfType
                    // 2014-10-07 Zhou Kai adds
                   , new SortableList<HaulierJobTrip>()
                    // 2014-10-07 Zhou Kai ends
                   );

                haulierJob.JobCreatorId = reader["Added_By"] == DBNull.Value ? string.Empty : reader["Added_By"].ToString();
                haulierJob.JobCreatedDateTime = reader["Added_DateTime"] == DBNull.Value ? (DateTime)reader["BOOKING_DATE"] : (DateTime)reader["Added_DateTime"];
                haulierJob.LastModified_By = reader["Modified_By"] == DBNull.Value ? string.Empty : reader["Modified_By"].ToString();
                haulierJob.LastModifiedDateTime = reader["Modified_DateTime"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["Modified_DateTime"];
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }

            return haulierJob;
        }

        internal static SortableList<HaulierJob> GetHaulierJobs(HaulierJob startJob, HaulierJob endJob)
        {
            string sqlWhere = " Where ";
            sqlWhere += "TPT_JOB_MAIN_Tbl.JOB_NUMBER >= '" + startJob.JobNo + "' AND TPT_JOB_MAIN_Tbl.JOB_NUMBER <= '" + endJob.JobNo + "'";
            return GetHaulierJobs(sqlWhere);
        }

        internal static SortableList<HaulierJob> GetHaulierJobs(CustomerDTO startCust, CustomerDTO endCust)
        {
            string sqlWhere = " Where ";
            sqlWhere += "TPT_JOB_MAIN_Tbl.CUST_CODE >= '" + startCust.Code + "' AND TPT_JOB_MAIN_Tbl.CUST_CODE <= '" + endCust.Code + "'";
            return GetHaulierJobs(sqlWhere);
        }

        internal static SortableList<HaulierJob> GetHaulierJobs(HaulierJob startJob, HaulierJob endJob, CustomerDTO startCust, CustomerDTO endCust)
        {
            string sqlWhere = "  Where ";
            sqlWhere += "TPT_JOB_MAIN_Tbl.JOB_NUMBER >= '" + startJob.JobNo + "' AND TPT_JOB_MAIN_Tbl.JOB_NUMBER <= '" + endJob.JobNo + "'";
            sqlWhere += " AND TPT_JOB_MAIN_Tbl.CUST_CODE >= '" + startCust.Code + "' AND TPT_JOB_MAIN_Tbl.CUST_CODE <= '" + endCust.Code + "'";
            return GetHaulierJobs(sqlWhere);
        }

        private static SortableList<HaulierJob> GetHaulierJobs(string sqlWhere)
        {
            SortableList<HaulierJob> haulierJobs = new SortableList<HaulierJob>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_JOB_MAIN_Tbl with (NOLOCK) LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID";
                SQLString += sqlWhere;
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    haulierJobs.Add(GetHaulierJob(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
            return haulierJobs;
        }

        // creates HaulierJobTrip object from 1 row of the DataReader
        internal static HaulierJobTrip GetHaulierJobTrip(IDataReader reader)
        {
            bool isLaden = reader["IS_LADEN"] == DBNull.Value ? false :((string)reader["IS_LADEN"] == "T"? true : false);
            int temp2 = (int)reader["STATUS"];
            JobTripStatus jobtripstatus = JobTripStatus.Booked;
            if (temp2 == 1)
                jobtripstatus = JobTripStatus.Booked;
            if (temp2 == 2)
                jobtripstatus = JobTripStatus.Ready;
            if (temp2 == 3)
                jobtripstatus = JobTripStatus.Assigned;
            if (temp2 == 4)
                jobtripstatus = JobTripStatus.Completed;
            if (temp2 == 5)
                jobtripstatus = JobTripStatus.Invoiced;

            Stop stop = Stop.GetStop((string)reader["FROM_STOP"]);
            Stop startStop = new Stop(
                (string)reader["FROM_STOP"],
                stop.Description,    //20121116 - Gerry replaced to get Name or description base on the stop code
                (string)reader["FROM_ADD1"],
                (string)reader["FROM_ADD2"],
                (string)reader["FROM_ADD3"],
                (string)reader["FROM_ADD4"],
                stop.City);     //20121116 - Gerry replaced to get city base on the stop code
            startStop.operationType = stop.operationType;
            startStop.postalCode = stop.postalCode;
            startStop.countryName = stop.countryName;

                stop = Stop.GetStop((string)reader["TO_STOP"]);
            Stop endStop = new Stop(
                (string)reader["TO_STOP"],
                stop.Description,    //20121116 - Gerry replaced to get Name or description base on the stop code
                (string)reader["TO_ADD1"],
                (string)reader["TO_ADD2"],
                (string)reader["TO_ADD3"],
                (string)reader["TO_ADD4"],
                stop.City);     //20121116 - Gerry replaced to get city base on the stop code
            endStop.operationType = stop.operationType;
            endStop.postalCode = stop.postalCode;
            endStop.countryName = stop.countryName;

            //20140312 - gerry added
            //this is to be change when the final logic of haulier job trip for sub con
            //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
            //CustomerDTO subCon = CustomerDTO.GetSubContractorByCode(reader["SUBCONTRACTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CODE"]);
            OperatorDTO subCon = OperatorDTO.GetOperatorDTO(reader["SUBCONTRACTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CODE"]);
            //20140312 end
            //20140523 - gerry added because address might change
            subCon.Name = (reader["SUBCONTRACTOR_NAME"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_NAME"]);
            subCon.Add1 = reader["SUBCONTRACTOR_ADD1"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD1"];
            subCon.Add2 = reader["SUBCONTRACTOR_ADD2"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD2"];
            subCon.Add3 = reader["SUBCONTRACTOR_ADD3"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD3"];
            subCon.Add4 = reader["SUBCONTRACTOR_ADD4"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD4"];
            subCon.City = reader["SUBCONTRACTOR_CITY_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CITY_CODE"];
            //20140523 end

            SortableList<JobTripState> jobStates = new SortableList<JobTripState>();


            bool isDangerousGoods = reader["IS_DANGEROUS_GOODS"] == DBNull.Value ? false : (string)reader["IS_DANGEROUS_GOODS"] == "T" ? true : false;
            bool isOverSize = reader["IS_OVERSIZE"] == DBNull.Value ? false : (string)reader["IS_OVERSIZE"] == "T" ? true : false;
            bool isDirectDelivery = reader["IS_DIRECT_DELIVERY"] == DBNull.Value ? false : (string)reader["IS_DIRECT_DELIVERY"] == "T" ? true : false;
            bool ownTransport = reader["OWNTRANSPORT"] == DBNull.Value ? false : (string)reader["OWNTRANSPORT"] == "T" ? true : false;

            Leg_Type templeg = Leg_Type.OneLeg;
            int t = (int)reader["LEG_TYPE"];
            if (t == 1) templeg = Leg_Type.OneLeg;
            if (t == 21) templeg = Leg_Type.FirstOfTwoLeg;
            if (t == 22) templeg = Leg_Type.SecondOfTwoLeg;

            // 2014-10-07 Zhou Kai adds
            string ptnJobNo = reader["PARTNER_JOB_NO"] == DBNull.Value ? String.Empty : reader["PARTNER_JOB_NO"].ToString();

            int legGrpNo = reader["LEG_GROUP"] == DBNull.Value ?  -1 : Convert.ToInt32(reader["LEG_GROUP"]);
            int legGrpMbr = reader["LEG_GROUP_MEMBER"] == DBNull.Value ? 0 : Convert.ToInt32(reader["LEG_GROUP_MEMBER"]);
            // 2014-10-07 Zhou Kai ends

            // 2014-10-07 Zhou Kai adds try
            HaulierJobTrip haulierJobTrip = null;
            try
            {
                haulierJobTrip = new HaulierJobTrip(
                   (int)reader["JOB_ID"],
                   (int)reader["SEQUENCE_NO"],
                   (DateTime)reader["FROM_DATE"],
                   startStop,
                   (DateTime)reader["TO_DATE"],
                   endStop,
                   (string)reader["FROM_TIME"],
                   (string)reader["TO_TIME"],
                   jobtripstatus,
                   jobStates,
                   (byte[])reader["UPD_VER"],
                   true,
                   ownTransport,
                    /*(string)reader["SUBCONTRACTOR_CODE"]*/ subCon,    //20140312 - gerry replaced
                   (string)reader["SUBCONTRACTOR_REFERENCE"],
                   (string)reader["CHARGE_CODE"],
                   (string)reader["REMARKS"],
                   (string)reader["CONTAINER_NUMBER"],
                   (string)reader["SEAL_NUMBER"],
                   (decimal)reader["GROSS_WEIGHT"],
                   (decimal)reader["GROSS_CBM"],
                   isLaden,
                   (string)reader["CONTAINER_CODE"],
                   (string)reader["CARGO_DESCRIPTION"],
                   (decimal)reader["MAX_CBM"],
                   (decimal)reader["MAX_WEIGHT"],
                   isDangerousGoods,
                   (string)reader["DG_REMARKS"],
                   isOverSize,
                   (string)reader["OVERSIZE_REMARKS"],
                   isDirectDelivery,
                   templeg,
                   (int)reader["PARTNER_LEG"],
                   (string)reader["PORTREMARKS"],
                   (string)reader["YARD"]
                    /*2014-07-25 Zhou Kai adds*/, reader["IsBillable"].ToString().Equals("T") ? true : false

                   // 2014-10-07 Zhou Kai adds
                   , ptnJobNo,
                   legGrpNo,
                   legGrpMbr
                    // 2014-10-07 Zhou Kai ends
                   );


                string custCode = reader["CUST_CODE"] == DBNull.Value ? string.Empty : (string)reader["CUST_CODE"];
                // CustomerDTO cust = CustomerDTO.GetCustomerByCustCode(custCode);
                haulierJobTrip.CustomerCode = custCode;
                //haulierJobTrip.JobTripStates = GetAllHaulierJobTripStates(haulierJobTrip);
                haulierJobTrip.JobNo = reader["JOB_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["JOB_NUMBER"];

                //20140120 - Gerry modified to get haulierJob object once instead of calling 1 by 1
                //create HaulierJob for 3 field and future haulier job fields             PERMIT_NO1
                //HaulierJob job = HaulierJob.GetHaulierJob(haulierJobTrip.JobNo);
                //haulierJobTrip.permitNo = job.permitNo1.ToString();
                haulierJobTrip.permitNo = reader["PERMIT_NO1"] == DBNull.Value ? string.Empty : (string)reader["PERMIT_NO1"];
                //haulierJobTrip.vesselNo = job.VesselNo.ToString();
                haulierJobTrip.vesselNo = reader["VESSEL_NO"] == DBNull.Value ? string.Empty : (string)reader["VESSEL_NO"];
                haulierJobTrip.vesselName = reader["Vessel_Name"] == DBNull.Value ? string.Empty : (string)reader["Vessel_Name"];
                //haulierJobTrip.voyageNo = job.voyageNo.ToString();
                haulierJobTrip.voyageNo = reader["VOYAGE_NO"] == DBNull.Value ? string.Empty : (string)reader["VOYAGE_NO"];
                //haulierJobTrip.oblNo = job.oblNo.ToString();    //added 20140120      
                haulierJobTrip.oblNo = reader["OBL_NO"] == DBNull.Value ? string.Empty : (string)reader["OBL_NO"];
                //end  20140120
                //20120813 - Gerry added WYN2000 - update pertmit number from planning
                haulierJobTrip.oldPermitNo = haulierJobTrip.permitNo;
                //20121005 - Gerry added for WYN2000 multiple leg more than 2 legs
                haulierJobTrip.isMulti_leg = reader["IS_MULTI_LEG"] == DBNull.Value ? false : ((string)reader["IS_MULTI_LEG"] == "T") ? true : false;
                haulierJobTrip.jobType = reader["JOB_TYPE"] == DBNull.Value ? string.Empty : (string)reader["JOB_TYPE"];
                //20140708 -gerry modified to get customer either form transport settings or cust_vend_master  
                //if billable the take from customer table else from transport settings
                haulierJobTrip.CustomerName = CustomerDTO.GetCustomerNameByCustomerCode(custCode) == string.Empty ? TransportSettings.GetTransportSetting().Tpt_name : CustomerDTO.GetCustomerNameByCustomerCode(custCode);
                //20140708 end
                haulierJobTrip.SourceRefNo = reader["Source_Ref_Number"] == DBNull.Value ? string.Empty : reader["Source_Ref_Number"].ToString();
                haulierJobTrip.BookRefNo = reader["Booking_Reference"] == DBNull.Value ? string.Empty : reader["Booking_Reference"].ToString();
                haulierJobTrip.isMovementJob = reader["Is_Movement_Job"] == DBNull.Value ? false : ((string)reader["Is_Movement_Job"] == "T") ? true : false;
                //20161008 -gerry added
                haulierJobTrip.JobCreatorId = reader["Added_By"] == DBNull.Value ? string.Empty : reader["Added_By"].ToString();
                haulierJobTrip.JobCreatedDateTime = reader["Added_DateTime"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["Added_DateTime"];
                haulierJobTrip.LastModified_By = reader["Modified_By"] == DBNull.Value ? string.Empty : reader["Modified_By"].ToString();
                haulierJobTrip.LastModifiedDateTime = reader["Modified_DateTime"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["Modified_DateTime"];
                //20161017-gerry added
                haulierJobTrip.Processed = reader["Processed"] == DBNull.Value ? false : ((string)reader["Processed"] == "T") ? true : false;
                haulierJobTrip.Released = reader["Released"] == DBNull.Value ? false : ((string)reader["Released"] == "T") ? true : false;
                haulierJobTrip.ESN = reader["ESN"] == DBNull.Value ? false : ((string)reader["ESN"] == "T") ? true : false;

                //haulierJobTrip.LegTypeCustomized = reader["Leg"] == DBNull.Value ? string.Empty : (string)reader["Leg"];//20170111
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrip. " + ex.Message); }

            return haulierJobTrip;
        }

        internal static string GetPrefix()
        {
            string prefix = "";
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT JOB_NO_PREFIX FROM TPT_SPECIAL_DATA_Tbl with (NOLOCK)";

                cn.Open();
                SqlCommand cmd3 = new SqlCommand(SQLString, cn);
                cmd3.CommandTimeout = 0;
                IDataReader reader = cmd3.ExecuteReader();
                while (reader.Read())
                {
                    prefix = (string)reader["JOB_NO_PREFIX"];
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPrefix. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPrefix. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPrefix. " + ex.Message); }
            return prefix;
        }

        // 2014-05-22 Zhou Kai adds a parameter SUBCONTRACTOR_CITY_CODE to the stored procedure
        internal static string AddHaulierHeader(HaulierJob haulierJob, SqlConnection cn, SqlTransaction tran, string prefix, string userID)
        {
            try
            {
                //SqlCommand cmd2 = new SqlCommand("sp_Insert_AM_Special_Job_Number_Tbl_ForTR", cn);
                SqlCommand cmd2 = new SqlCommand("sp_Insert_TPT_Special_Job_Number", cn);
                cmd2.CommandTimeout = 0;
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Year_Code", haulierJob.JobDateTime.Year.ToString());
                cmd2.Parameters.AddWithValue("@Month", haulierJob.JobDateTime.Month);
                cmd2.Parameters.AddWithValue("@Job_Type_Code", prefix);

                cmd2.Transaction = tran;
                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd2.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd2.ExecuteNonQuery();
                int temprunningno = Convert.ToInt32(newReqNumber.Value);


                // form the bookingNo 
                haulierJob.JobNo = prefix +
                    haulierJob.BranchCode +
                    haulierJob.JobDateTime.Year.ToString().Substring(2, 2) +
                    haulierJob.JobDateTime.Month.ToString("D2") +
                    temprunningno.ToString("D4") +
                    "00";

                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_HAULIER_JOB_HEADER", cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@JOB_ID", haulierJob.JobID);
                cmd.Parameters.AddWithValue("@CUST_CODE", haulierJob.CustNo);
                cmd.Parameters.AddWithValue("@JOB_TYPE", haulierJob.JobType);
                cmd.Parameters.AddWithValue("@BOOKING_NUMBER", haulierJob.JobNo);
                cmd.Parameters.AddWithValue("@JOB_NUMBER", haulierJob.JobNo);
                cmd.Parameters.AddWithValue("@BOOKING_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.JobDateTime));
                cmd.Parameters.AddWithValue("@BRANCH_CODE", haulierJob.BranchCode);
                cmd.Parameters.AddWithValue("@SHIPPER_CODE", haulierJob.shipperCode);
                cmd.Parameters.AddWithValue("@SHIPPER_NAME", haulierJob.shipperName);
                cmd.Parameters.AddWithValue("@SHIPPER_ADD1", haulierJob.shipperAdd1);
                cmd.Parameters.AddWithValue("@SHIPPER_ADD2", haulierJob.shipperAdd2);
                cmd.Parameters.AddWithValue("@SHIPPER_ADD3", haulierJob.shipperAdd3);
                cmd.Parameters.AddWithValue("@SHIPPER_ADD4", haulierJob.shipperAdd4);
                cmd.Parameters.AddWithValue("@CONSIGNEE_CODE", haulierJob.consigneeCode);
                cmd.Parameters.AddWithValue("@CONSIGNEE_NAME", haulierJob.consigneeName);
                cmd.Parameters.AddWithValue("@CONSIGNEE_ADD1", haulierJob.consigneeAdd1);
                cmd.Parameters.AddWithValue("@CONSIGNEE_ADD2", haulierJob.consigneeAdd2);
                cmd.Parameters.AddWithValue("@CONSIGNEE_ADD3", haulierJob.consigneeAdd3);
                cmd.Parameters.AddWithValue("@CONSIGNEE_ADD4", haulierJob.consigneeAdd4);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", haulierJob.subcontractorCode);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_NAME", haulierJob.subcontractorName);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD1", haulierJob.subcontractorAdd1);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD2", haulierJob.subcontractorAdd2);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD3", haulierJob.subcontractorAdd3);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD4", haulierJob.subcontractorAdd4);
                // 2014-05-22 Zhou Kai adds
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CITY_CODE", haulierJob.subcontractorCityCode);
                // 2014-05-22 Zhou Kai ends
                cmd.Parameters.AddWithValue("@PSA_ACCOUNT", haulierJob.psaAccount);
                cmd.Parameters.AddWithValue("@WAREHOUSE_NO", haulierJob.warehouseNo);
                cmd.Parameters.AddWithValue("@YOUR_REF_NO", haulierJob.yourRefNo);
                cmd.Parameters.AddWithValue("@UCR_NO", haulierJob.ucrNo);
                cmd.Parameters.AddWithValue("@PERMIT_NO1", haulierJob.permitNo1);
                cmd.Parameters.AddWithValue("@DATE_PERMIT_NO1", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.datePermitNo1));
                cmd.Parameters.AddWithValue("@PERMIT_NO2", haulierJob.permitNo2);
                cmd.Parameters.AddWithValue("@DATE_PERMIT_NO2", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.datePermitNo2));
                cmd.Parameters.AddWithValue("@REMARKS", haulierJob.remarks);
                cmd.Parameters.AddWithValue("@REMARKS2", haulierJob.remarks2);
                cmd.Parameters.AddWithValue("@REMARKS3", haulierJob.remarks3);
                cmd.Parameters.AddWithValue("@REMARKS4", haulierJob.remarks4);
                cmd.Parameters.AddWithValue("@REMARKS5", haulierJob.remarks5);
                cmd.Parameters.AddWithValue("@JOB_STATUS", haulierJob.JobStatus);
                cmd.Parameters.AddWithValue("@CARRIER_AGENT", haulierJob.carrierAgent);
                cmd.Parameters.AddWithValue("@OBL_NO", haulierJob.oblNo);
                cmd.Parameters.AddWithValue("@HBL_NO", haulierJob.hblNo);
                cmd.Parameters.AddWithValue("@VESSEL_NO", haulierJob.VesselNo);
                cmd.Parameters.AddWithValue("@VESSEL_Name", haulierJob.VesselName); // 2014-07-02 ZK adds
                cmd.Parameters.AddWithValue("@VOYAGE_NO", haulierJob.voyageNo);
                cmd.Parameters.AddWithValue("@POL", haulierJob.POL);
                cmd.Parameters.AddWithValue("@POL_Name", haulierJob.PolName); // 2014-07-02 ZK adds
                cmd.Parameters.AddWithValue("@POD", haulierJob.POD);
                cmd.Parameters.AddWithValue("@POD_Name", haulierJob.PodName); // 2014-07-02 ZK adds
                cmd.Parameters.AddWithValue("@ETA", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.eta));
                cmd.Parameters.AddWithValue("@ETD", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.etd));
                cmd.Parameters.AddWithValue("@QUOTATION_NO", haulierJob.QuotationNo);
                cmd.Parameters.AddWithValue("@HAULIER_CHARGE_TYPE", haulierJob.HaulierChargeType.GetHashCode());

                int status = 0;
                if (haulierJob.JobUrgencyType == JobUrgencyType.Normal)
                    status = 1;
                if (haulierJob.JobUrgencyType == JobUrgencyType.Important)
                    status = 2;
                if (haulierJob.JobUrgencyType == JobUrgencyType.Urgent)
                    status = 3;
                cmd.Parameters.AddWithValue("@JOB_URGENCY_TYPE", status);
                cmd.Parameters.AddWithValue("@SOURCE_REF_NUMBER", haulierJob.SourceReferenceNo);
                if (haulierJob.BillByTransport == true)
                    cmd.Parameters.AddWithValue("@BILL_BY_TRANSPORT", "T");
                else
                    cmd.Parameters.AddWithValue("@BILL_BY_TRANSPORT", "F");
                cmd.Parameters.AddWithValue("@TPT_DEPT_CODE", haulierJob.TptDeptCode);

                SqlParameter newReqNumber2 = new SqlParameter("@Value", SqlDbType.Int);
                #region "2014-06-18 Zhou Kai adds parameters"
                // TPT_JOB_MAIN_Tbl
                cmd.Parameters.AddWithValue("@CUSTOMER_REFERENCE", haulierJob.CustomerRef);
                cmd.Parameters.AddWithValue("@PORTNET_REF", haulierJob.PortNetRef);
                cmd.Parameters.AddWithValue("@ISBILLABLE", haulierJob.IsBillable ? 'T' : 'F');
                // TPT_JOB_MAIN_DETAIL_CARGO_Tbl
                cmd.Parameters.AddWithValue("@SHIPPING_LINE", haulierJob.ShippingLine);
                cmd.Parameters.AddWithValue("@SHIPPING_LINE_NAME", haulierJob.ShippingLineName);
                cmd.Parameters.AddWithValue("@BOOKING_REFERENCE", haulierJob.BookingRef);
                cmd.Parameters.AddWithValue("@CARGO_DESCRIPTION", haulierJob.CargoDescription);
                cmd.Parameters.AddWithValue("@STUFFING_AT", haulierJob.StuffingAt.Code);
                cmd.Parameters.AddWithValue("@STUFFING_AT_Name", haulierJob.StuffingAt.Description);
                cmd.Parameters.AddWithValue("@STUFFING_AT_ADDRESS1", haulierJob.StuffingAt.Address1);
                cmd.Parameters.AddWithValue("@STUFFING_AT_ADDRESS2", haulierJob.StuffingAt.Address2);
                cmd.Parameters.AddWithValue("@STUFFING_AT_ADDRESS3", haulierJob.StuffingAt.Address3);
                cmd.Parameters.AddWithValue("@STUFFING_AT_ADDRESS4", haulierJob.StuffingAt.Address4);
                cmd.Parameters.AddWithValue("@STUFFING_AT_CITY", haulierJob.StuffingAt.City);
                cmd.Parameters.AddWithValue("@UNSTUFFING_AT", haulierJob.UnstuffingAt.Code);
                cmd.Parameters.AddWithValue("@UNSTUFFING_AT_Name", haulierJob.UnstuffingAt.Description);
                cmd.Parameters.AddWithValue("@UNSTUFFING_AT_ADDRESS1", haulierJob.UnstuffingAt.Address1);
                cmd.Parameters.AddWithValue("@UNSTUFFING_AT_ADDRESS2", haulierJob.UnstuffingAt.Address2);
                cmd.Parameters.AddWithValue("@UNSTUFFING_AT_ADDRESS3", haulierJob.UnstuffingAt.Address3);
                cmd.Parameters.AddWithValue("@UNSTUFFING_AT_ADDRESS4", haulierJob.UnstuffingAt.Address4);
                cmd.Parameters.AddWithValue("@UNSTUFFING_AT_CITY", haulierJob.UnstuffingAt.City);
                cmd.Parameters.AddWithValue("@PORT_CODE", haulierJob.Port.Code);
                cmd.Parameters.AddWithValue("@PORT_Name", haulierJob.Port.Description);
                cmd.Parameters.AddWithValue("@PORT_AT_ADDRESS1", haulierJob.Port.Address1);
                cmd.Parameters.AddWithValue("@PORT_AT_ADDRESS2", haulierJob.Port.Address2);
                cmd.Parameters.AddWithValue("@PORT_AT_ADDRESS3", haulierJob.Port.Address3);
                cmd.Parameters.AddWithValue("@PORT_AT_ADDRESS4", haulierJob.ContainerYard.Address4);
                cmd.Parameters.AddWithValue("@PORT_AT_CITY", haulierJob.ContainerYard.City);
                cmd.Parameters.AddWithValue("@CONTAINER_YARD_CODE", haulierJob.ContainerYard.Code);
                cmd.Parameters.AddWithValue("@CONTAINER_YARD_Name", haulierJob.ContainerYard.Description);
                cmd.Parameters.AddWithValue("@CONTAINER_YARD_ADDRESS1", haulierJob.ContainerYard.Address1);
                cmd.Parameters.AddWithValue("@CONTAINER_YARD_ADDRESS2", haulierJob.ContainerYard.Address2);
                cmd.Parameters.AddWithValue("@CONTAINER_YARD_ADDRESS3", haulierJob.ContainerYard.Address3);
                cmd.Parameters.AddWithValue("@CONTAINER_YARD_ADDRESS4", haulierJob.ContainerYard.Address4);
                cmd.Parameters.AddWithValue("@CONTAINER_YARD_CITY", haulierJob.ContainerYard.City);
                #endregion

                #region "2014-10-07 Zhou Kai"
                cmd.Parameters.AddWithValue("@Partner_job_number",
                    haulierJob.PartnerJobNo);
                cmd.Parameters.AddWithValue("@job_transfer_type",
                    haulierJob.JobTransferType);
                #endregion

                cmd.Parameters.AddWithValue("@Added_By", userID);
                cmd.Parameters.AddWithValue("@Added_DateTime", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(DateTime.Now));


                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                haulierJob.JobID = Convert.ToInt32(newReqNumber2.Value);
                return haulierJob.JobNo;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : AddHaulierHeader. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : AddHaulierHeader. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : AddHaulierHeader. " + ex.Message); }
        }
        internal static byte[] GetHeaderUpdateVersion(int jobId)
        {
            byte[] updateVersion = new byte[8];
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = "SELECT UPDATE_VERSION FROM TPT_JOB_MAIN_Tbl with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + jobId;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd1 = new SqlCommand(SQLString, con);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.Text;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    updateVersion = reader["UPDATE_VERSION"] == DBNull.Value ? new byte[8] : (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHeaderUpdateVersion. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHeaderUpdateVersion. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHeaderUpdateVersion. " + ex.Message); }
            //finally { con.Close(); }
            return updateVersion;

        }
        //20140818 - gerry added
        internal static byte[] GetHeaderUpdateVersion(int jobId, SqlConnection con, SqlTransaction tran)
        {
            byte[] updateVersion = new byte[8];
            try
            {
                string SQLString = "SELECT UPDATE_VERSION FROM TPT_JOB_MAIN_Tbl  with (NOLOCK)";
                SQLString += "WHERE JOB_ID = " + jobId;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd1 = new SqlCommand(SQLString, con);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    updateVersion = reader["UPDATE_VERSION"] == DBNull.Value ? new byte[8] : (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHeaderUpdateVersion. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHeaderUpdateVersion. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHeaderUpdateVersion. " + ex.Message); }
            //finally { con.Close(); }
            return updateVersion;
        }

        // 2014-05-22 Zhou Kai adds a parameter: bool isSubContractedByWholeJob, and the corresponding logic
        internal static bool EditHaulierHeader(HaulierJob haulierJob, bool isSubContractedByWholeJob, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = true;
            try
            {
                byte[] originalRowVersion = GetHeaderUpdateVersion(haulierJob.JobID);
                if (SqlBinary.Equals(haulierJob.UpdateVersion, originalRowVersion) == false)
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nHaulierJobDAL.EditHaulierJobHeader");
                }
                else
                {
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }

                    SqlCommand cmd = new SqlCommand("sp_Edit_TPT_HAULIER_JOB_HEADER", cn);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JOB_ID", haulierJob.JobID);
                    cmd.Parameters.AddWithValue("@CUST_CODE", haulierJob.CustNo);
                    cmd.Parameters.AddWithValue("@JOB_TYPE", haulierJob.JobType);
                    cmd.Parameters.AddWithValue("@BOOKING_NUMBER", haulierJob.JobNo);
                    cmd.Parameters.AddWithValue("@JOB_NUMBER", haulierJob.JobNo);
                    cmd.Parameters.AddWithValue("@BOOKING_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.JobDateTime));
                    cmd.Parameters.AddWithValue("@BRANCH_CODE", haulierJob.BranchCode);
                    cmd.Parameters.AddWithValue("@SHIPPER_CODE", haulierJob.shipperCode);
                    cmd.Parameters.AddWithValue("@SHIPPER_NAME", haulierJob.shipperName);
                    cmd.Parameters.AddWithValue("@SHIPPER_ADD1", haulierJob.shipperAdd1);
                    cmd.Parameters.AddWithValue("@SHIPPER_ADD2", haulierJob.shipperAdd2);
                    cmd.Parameters.AddWithValue("@SHIPPER_ADD3", haulierJob.shipperAdd3);
                    cmd.Parameters.AddWithValue("@SHIPPER_ADD4", haulierJob.shipperAdd4);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_CODE", haulierJob.consigneeCode);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_NAME", haulierJob.consigneeName);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_ADD1", haulierJob.consigneeAdd1);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_ADD2", haulierJob.consigneeAdd2);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_ADD3", haulierJob.consigneeAdd3);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_ADD4", haulierJob.consigneeAdd4);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", haulierJob.subcontractorCode);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_NAME", haulierJob.subcontractorName);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD1", haulierJob.subcontractorAdd1);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD2", haulierJob.subcontractorAdd2);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD3", haulierJob.subcontractorAdd3);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD4", haulierJob.subcontractorAdd4);
                    // 2014-05-22 Zhou Kai adds
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CITY_CODE", haulierJob.subcontractorCityCode);
                    // 2014-05-22 Zhou Kai ends
                    cmd.Parameters.AddWithValue("@PSA_ACCOUNT", haulierJob.psaAccount);
                    cmd.Parameters.AddWithValue("@WAREHOUSE_NO", haulierJob.warehouseNo);
                    cmd.Parameters.AddWithValue("@YOUR_REF_NO", haulierJob.yourRefNo);
                    cmd.Parameters.AddWithValue("@UCR_NO", haulierJob.ucrNo);
                    cmd.Parameters.AddWithValue("@PERMIT_NO1", haulierJob.permitNo1);
                    cmd.Parameters.AddWithValue("@DATE_PERMIT_NO1", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.datePermitNo1));
                    cmd.Parameters.AddWithValue("@PERMIT_NO2", haulierJob.permitNo2);
                    cmd.Parameters.AddWithValue("@DATE_PERMIT_NO2", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.datePermitNo2));
                    cmd.Parameters.AddWithValue("@REMARKS", haulierJob.remarks);
                    cmd.Parameters.AddWithValue("@REMARKS2", haulierJob.remarks2);
                    cmd.Parameters.AddWithValue("@REMARKS3", haulierJob.remarks3);
                    cmd.Parameters.AddWithValue("@REMARKS4", haulierJob.remarks4);
                    cmd.Parameters.AddWithValue("@REMARKS5", haulierJob.remarks5);
                    cmd.Parameters.AddWithValue("@JOB_STATUS", haulierJob.JobStatus);
                    cmd.Parameters.AddWithValue("@CARRIER_AGENT", haulierJob.carrierAgent);
                    cmd.Parameters.AddWithValue("@OBL_NO", haulierJob.oblNo);
                    cmd.Parameters.AddWithValue("@HBL_NO", haulierJob.hblNo);
                    cmd.Parameters.AddWithValue("@VESSEL_NO", haulierJob.VesselNo);
                    cmd.Parameters.AddWithValue("@VESSEL_Name", haulierJob.VesselName); // 2014-07-02 ZK adds
                    cmd.Parameters.AddWithValue("@VOYAGE_NO", haulierJob.voyageNo);
                    cmd.Parameters.AddWithValue("@POL", haulierJob.POL);
                    cmd.Parameters.AddWithValue("@POL_Name", haulierJob.PolName); // 2014-07-02 ZK adds
                    cmd.Parameters.AddWithValue("@POD", haulierJob.POD);
                    cmd.Parameters.AddWithValue("@POD_Name", haulierJob.PodName); // 2014-07-02 ZK adds
                    cmd.Parameters.AddWithValue("@ETA", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.eta));
                    cmd.Parameters.AddWithValue("@ETD", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.etd));
                    cmd.Parameters.AddWithValue("@QUOTATION_NO", haulierJob.QuotationNo);
                    cmd.Parameters.AddWithValue("@JOBCHARGETYPE", haulierJob.HaulierChargeType.GetHashCode());
                    int status = 0;
                    if (haulierJob.JobUrgencyType == JobUrgencyType.Normal)
                        status = 1;
                    if (haulierJob.JobUrgencyType == JobUrgencyType.Important)
                        status = 2;
                    if (haulierJob.JobUrgencyType == JobUrgencyType.Urgent)
                        status = 3;
                    cmd.Parameters.AddWithValue("@JOB_URGENCY_TYPE", status);
                    cmd.Parameters.AddWithValue("@SOURCE_REF_NUMBER", haulierJob.SourceReferenceNo);
                    if (haulierJob.BillByTransport == true)
                        cmd.Parameters.AddWithValue("@BILL_BY_TRANSPORT", "T");
                    else
                        cmd.Parameters.AddWithValue("@BILL_BY_TRANSPORT", "F");
                    cmd.Parameters.AddWithValue("@TPT_DEPT_CODE", haulierJob.TptDeptCode);
                    haulierJob.OldTptDeptCode = haulierJob.TptDeptCode;
                    #region "2014-06-18 Zhou Kai adds parameters"
                    // TPT_JOB_MAIN_Tbl
                    cmd.Parameters.AddWithValue("@CUSTOMER_REFERENCE", haulierJob.CustomerRef);
                    cmd.Parameters.AddWithValue("@PORTNET_REF", haulierJob.PortNetRef);
                    cmd.Parameters.AddWithValue("@ISBILLABLE", haulierJob.IsBillable ? 'T' : 'F');
                    // TPT_JOB_MAIN_DETAIL_CARGO_Tbl
                    cmd.Parameters.AddWithValue("@SHIPPING_LINE", haulierJob.ShippingLine);
                    cmd.Parameters.AddWithValue("@SHIPPING_LINE_Name", haulierJob.ShippingLineName);
                    cmd.Parameters.AddWithValue("@BOOKING_REFERENCE", haulierJob.BookingRef);
                    cmd.Parameters.AddWithValue("@CARGO_DESCRIPTION", haulierJob.CargoDescription);
                    cmd.Parameters.AddWithValue("@STUFFING_AT", haulierJob.StuffingAt.Code);
                    cmd.Parameters.AddWithValue("@STUFFING_AT_Name", haulierJob.StuffingAt.Description);
                    cmd.Parameters.AddWithValue("@STUFFING_AT_ADDRESS1", haulierJob.StuffingAt.Address1);
                    cmd.Parameters.AddWithValue("@STUFFING_AT_ADDRESS2", haulierJob.StuffingAt.Address2);
                    cmd.Parameters.AddWithValue("@STUFFING_AT_ADDRESS3", haulierJob.StuffingAt.Address3);
                    cmd.Parameters.AddWithValue("@STUFFING_AT_ADDRESS4", haulierJob.StuffingAt.Address4);
                    cmd.Parameters.AddWithValue("@STUFFING_AT_CITY", haulierJob.StuffingAt.City);
                    cmd.Parameters.AddWithValue("@UNSTUFFING_AT", haulierJob.UnstuffingAt.Code);
                    cmd.Parameters.AddWithValue("@UNSTUFFING_AT_Name", haulierJob.UnstuffingAt.Description);
                    cmd.Parameters.AddWithValue("@UNSTUFFING_AT_ADDRESS1", haulierJob.UnstuffingAt.Address1);
                    cmd.Parameters.AddWithValue("@UNSTUFFING_AT_ADDRESS2", haulierJob.UnstuffingAt.Address2);
                    cmd.Parameters.AddWithValue("@UNSTUFFING_AT_ADDRESS3", haulierJob.UnstuffingAt.Address3);
                    cmd.Parameters.AddWithValue("@UNSTUFFING_AT_ADDRESS4", haulierJob.UnstuffingAt.Address4);
                    cmd.Parameters.AddWithValue("@UNSTUFFING_AT_CITY", haulierJob.UnstuffingAt.City);
                    cmd.Parameters.AddWithValue("@PORT_CODE", haulierJob.Port.Code);
                    cmd.Parameters.AddWithValue("@PORT_Name", haulierJob.Port.Description);
                    cmd.Parameters.AddWithValue("@PORT_AT_ADDRESS1", haulierJob.Port.Address1);
                    cmd.Parameters.AddWithValue("@PORT_AT_ADDRESS2", haulierJob.Port.Address2);
                    cmd.Parameters.AddWithValue("@PORT_AT_ADDRESS3", haulierJob.Port.Address3);
                    cmd.Parameters.AddWithValue("@PORT_AT_ADDRESS4", haulierJob.Port.Address4);
                    cmd.Parameters.AddWithValue("@PORT_AT_CITY", haulierJob.Port.City);
                    cmd.Parameters.AddWithValue("@CONTAINER_YARD_CODE", haulierJob.ContainerYard.Code);
                    cmd.Parameters.AddWithValue("@CONTAINER_YARD_NAME", haulierJob.ContainerYard.Description);
                    cmd.Parameters.AddWithValue("@CONTAINER_YARD_ADDRESS1", haulierJob.ContainerYard.Address1);
                    cmd.Parameters.AddWithValue("@CONTAINER_YARD_ADDRESS2", haulierJob.ContainerYard.Address2);
                    cmd.Parameters.AddWithValue("@CONTAINER_YARD_ADDRESS3", haulierJob.ContainerYard.Address3);
                    cmd.Parameters.AddWithValue("@CONTAINER_YARD_ADDRESS4", haulierJob.ContainerYard.Address4);
                    cmd.Parameters.AddWithValue("@CONTAINER_YARD_CITY", haulierJob.ContainerYard.City);
                    #endregion
                    #region "2014-10-07 Zhou Kai"
                    cmd.Parameters.AddWithValue("@Partner_job_no",haulierJob.PartnerJobNo);
                    cmd.Parameters.AddWithValue("@job_transfer_type",haulierJob.JobTransferType);
                    #endregion
                    //20160630 - gerry added
                    cmd.Parameters.AddWithValue("@Modified_By", haulierJob.LastModified_By.ToString());
                    cmd.Parameters.AddWithValue("@Modified_DateTime", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJob.LastModifiedDateTime));

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    // 2014-05-22 Zhou Kai adds logic
                    if (isSubContractedByWholeJob) // if not subContracted by whole job, no job trips will be modified
                    {
                        cmd = new SqlCommand();
                        cmd.Connection = cn;
                        cmd.Transaction = tran;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Edit_TPT_JOB_DETAIL_CARGO_TBL_SUBCON_FIELDS";
                        cmd.Parameters.AddWithValue("@jobId", haulierJob.JobID);
                        cmd.Parameters.AddWithValue("@subCon_code", haulierJob.subcontractorCode);
                        cmd.Parameters.AddWithValue("@subCon_name", haulierJob.subcontractorName);
                        cmd.Parameters.AddWithValue("@subCon_add1", haulierJob.subcontractorAdd1);
                        cmd.Parameters.AddWithValue("@subCon_add2", haulierJob.subcontractorAdd2);
                        cmd.Parameters.AddWithValue("@subCon_add3", haulierJob.subcontractorAdd3);
                        cmd.Parameters.AddWithValue("@subCon_add4", haulierJob.subcontractorAdd4);
                        cmd.Parameters.AddWithValue("@subCon_city_code", haulierJob.subcontractorCityCode);
                        cmd.ExecuteNonQuery();
                    }
                    //2015082 - gerry added to update the stop of job trips for first 2 legs
                    if (haulierJob.JobTrips.Count > 0)
                    {
                        string warnMsg = string.Empty;
                        if (haulierJob.CanUpdateLocationFromHeader(out warnMsg))
                        {
                            foreach (HaulierJobTrip haulierJobTrip in haulierJob.JobTrips)
                            {
                                #region separate sp for subcon
                                //haulierJobTrip.subCon = new OperatorDTO();
                                //haulierJobTrip.subCon.Code = haulierJob.subcontractorCode;
                                //haulierJobTrip.subCon.Name = haulierJob.subcontractorName;
                                //haulierJobTrip.subCon.Add1 = haulierJob.subcontractorAdd1;
                                //haulierJobTrip.subCon.Add2 = haulierJob.subcontractorAdd2;
                                //haulierJobTrip.subCon.Add3 = haulierJob.subcontractorAdd3;
                                //haulierJobTrip.subCon.Add4 = haulierJob.subcontractorAdd4;
                                //haulierJobTrip.subCon.City = haulierJob.subcontractorCityCode;
                                #endregion
                                haulierJobTrip.UpdateVersion = GetHaulierJobTripUpdateVersion(haulierJobTrip, cn, tran);
                                if (haulierJob.JobType.Contains("SE")) //for sea export
                                {
                                    haulierJobTrip.StartStop = haulierJobTrip.LegGroupMember == 1 ? haulierJob.ContainerYard : haulierJob.StuffingAt;
                                    haulierJobTrip.EndStop = haulierJobTrip.LegGroupMember == 1 ? haulierJob.StuffingAt : haulierJob.Port;
                                }
                                else if (haulierJob.JobType.Contains("SI")) //for sea import
                                {
                                    haulierJobTrip.StartStop = haulierJobTrip.LegGroupMember == 1 ? haulierJob.Port : haulierJob.UnstuffingAt;
                                    haulierJobTrip.EndStop = haulierJobTrip.LegGroupMember == 1 ? haulierJob.UnstuffingAt : haulierJob.ContainerYard;
                                }
                                ////20151230 - gerry added validation to only update those status is booked or ready
                                //if (haulierJobTrip.TripStatus == JobTripStatus.Booked || haulierJobTrip.TripStatus == JobTripStatus.Ready)
                                //{
                                EditHaulierJobTrip(haulierJobTrip, cn, tran);
                                //}
                            }
                        }
                    }
                }
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : EditHaulierHeader. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : EditHaulierHeader. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : EditHaulierHeader. " + ex.Message); }
            return retValue;
        }

        internal static bool DeleteHaulierJob(HaulierJob haulierJob, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TPT_HAULIER_JOB_HEADER", cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", haulierJob.JobID);

                SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                if (Convert.ToInt32(newReqNumber2.Value) == 0)
                {
                    throw new FMException("Error - No Record Deleted");
                }

                if (Convert.ToInt32(newReqNumber2.Value) > 1)
                {
                    throw new FMException("Error - More than 1 record deleted");
                }

            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJob. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJob. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJob. " + ex.Message); }

            return true;
        }

        internal static bool AddHaulierJobTrip(HaulierJob haulierJob, HaulierJobTrip haulierJobTrip,
            SqlConnection cn, SqlTransaction tran)
        {
            /* In a transaction 
             1) Pass into a store procedure, parameters from both objects
            haulierJob.JobNo;
            haulierJobTrip.StartStop.Code;
            haulierJobTrip.StartDate and so on  
            Need to add exception handling
            Assume in this example to be OK*/

            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_JOB_DETAIL_CARGO_Tbl", cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                // 2014-10-31 Zhou Kai uses haulierJob.JobId instead of haulierJobTrip.JobId
                // cmd.Parameters.AddWithValue("@JOB_ID", haulierJobTrip.JobID);
                cmd.Parameters.AddWithValue("@JOB_ID", haulierJob.JobID);
                // 2014-10-31 Zhou Kai ends
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", 0);
                cmd.Parameters.AddWithValue("@FROM_STOP", haulierJobTrip.StartStop.Code);
                cmd.Parameters.AddWithValue("@FROM_ADD1", haulierJobTrip.StartStop.Address1);
                cmd.Parameters.AddWithValue("@FROM_ADD2", haulierJobTrip.StartStop.Address2);
                cmd.Parameters.AddWithValue("@FROM_ADD3", haulierJobTrip.StartStop.Address3);
                cmd.Parameters.AddWithValue("@FROM_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJobTrip.StartDate));
                cmd.Parameters.AddWithValue("@FROM_TIME", haulierJobTrip.StartTime);
                cmd.Parameters.AddWithValue("@TO_STOP", haulierJobTrip.EndStop.Code);
                cmd.Parameters.AddWithValue("@TO_ADD1", haulierJobTrip.EndStop.Address1);
                cmd.Parameters.AddWithValue("@TO_ADD2", haulierJobTrip.EndStop.Address2);
                cmd.Parameters.AddWithValue("@TO_ADD3", haulierJobTrip.EndStop.Address3);
                cmd.Parameters.AddWithValue("@TO_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJobTrip.EndDate));
                cmd.Parameters.AddWithValue("@TO_TIME", haulierJobTrip.EndTime);
                cmd.Parameters.AddWithValue("@CONTAINER_CODE", haulierJobTrip.ContainerCode);
                cmd.Parameters.AddWithValue("@CONTAINER_NUMBER", haulierJobTrip.ContainerNo);
                cmd.Parameters.AddWithValue("@SEAL_NUMBER", haulierJobTrip.SealNo);
                cmd.Parameters.AddWithValue("@GROSS_CBM", haulierJobTrip.GrossCBM);
                cmd.Parameters.AddWithValue("@GROSS_WEIGHT", haulierJobTrip.GrossWeight);
                cmd.Parameters.AddWithValue("@MAX_CBM", haulierJobTrip.maxCBM);
                cmd.Parameters.AddWithValue("@MAX_WEIGHT", haulierJobTrip.maxWeight);
                cmd.Parameters.AddWithValue("@CARGO_DESCRIPTION", haulierJobTrip.CargoDescription);
                // 2014-05-21 Zhou Kai adds
                // cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", haulierJobTrip.subCon.Code);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_NAME", haulierJobTrip.subCon.Name);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD1", haulierJobTrip.subCon.Add1);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD2", haulierJobTrip.subCon.Add2);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD3", haulierJobTrip.subCon.Add3);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD4", haulierJobTrip.subCon.Add4);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CITY_CODE", haulierJobTrip.subCon.City);
                cmd.Parameters.AddWithValue("@IsBillable", haulierJobTrip.isBillable ? "T" : "F");
                // 2014-07-25 Zhou Kai ends
                if (haulierJobTrip.IsLaden == true)
                {
                    cmd.Parameters.AddWithValue("@IS_LADEN", "T");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IS_LADEN", "F");
                }
                cmd.Parameters.AddWithValue("@IS_DANGEROUS_GOODS", haulierJobTrip.IsDangerousGoods == true ? "T" : "F");

                cmd.Parameters.AddWithValue("@DG_REMARKS", haulierJobTrip.DGRemarks);
                cmd.Parameters.AddWithValue("@IS_OVERSIZE", haulierJobTrip.IsOversize == true ? "T" : "F");
                cmd.Parameters.AddWithValue("@OVERSIZE_REMARKS", haulierJobTrip.OversizeRemarks);
                cmd.Parameters.AddWithValue("@IS_DIRECT_DELIVERY", haulierJobTrip.IsDirectDelivery == true ? "T" : "F");
                cmd.Parameters.AddWithValue("@OWNTRANSPORT", haulierJobTrip.OwnTransport == true ? "T" : "F");
                //20140312-gerry replaced
                //cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", haulierJobTrip.SubContractorCode);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", haulierJobTrip.subCon.Code.ToString());
                //20140312 end

                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_REFERENCE", haulierJobTrip.SubContractorReference);
                cmd.Parameters.AddWithValue("@CHARGE_CODE", haulierJobTrip.ChargeCode);
                cmd.Parameters.AddWithValue("@REMARKS", haulierJobTrip.Remarks);

                cmd.Parameters.AddWithValue("@LEG_TYPE", haulierJobTrip.LegType.GetHashCode());
                cmd.Parameters.AddWithValue("@PARTNER_LEG", haulierJobTrip.PartnerLeg);
                cmd.Parameters.AddWithValue("@PORT_REMARK", haulierJobTrip.PortRemark);
                cmd.Parameters.AddWithValue("@YARD", haulierJobTrip.Yard);
                //20121005 - Gerry added - for multiple leg more than 2 legs
                cmd.Parameters.AddWithValue("@IS_MULTI_LEG", haulierJobTrip.isMulti_leg ? "T" : "F");
                cmd.Parameters.AddWithValue("@IS_MOVEMENT_JOB", haulierJobTrip.isMovementJob ? "T" : "F");
                //20161008 - gerry added
                cmd.Parameters.AddWithValue("@Added_By", haulierJobTrip.JobCreatorId.ToString());
                cmd.Parameters.AddWithValue("@Added_DateTime", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(haulierJobTrip.JobCreatedDateTime));
                #region "LegGroup, LegGroupMember"
                // 2014-10-09 Zhou Kai adds logic to
                // assign legGroup / legGroupMember
                //if (haulierJobTrip.LegGroup == -1)
                //{
                //    if (haulierJobTrip.ContainerNo == String.Empty)
                //    {
                //        haulierJobTrip.LegGroup = HaulierJobDAL.GetNextLegGroup(
                //            haulierJobTrip.JobID, cn, tran);
                //    }
                //    else
                //    {
                //        haulierJobTrip.LegGroup = HaulierJobDAL.GetNextLegGroupByContainerNo(
                //            haulierJobTrip.JobID, haulierJobTrip.ContainerNo, cn, tran);
                //    }
                //}
                //if (haulierJobTrip.LegGroupMember == -1)
                //{
                //    haulierJobTrip.LegGroupMember = HaulierJobDAL.GetNextLegGroupMember(
                //                                            haulierJobTrip.JobID,
                //                                            haulierJobTrip.LegGroup,
                //                                            cn, tran
                //                                            );
                //}

                // 2014-10-09 Zhou Kai ends

                #region "2014-10-07 Zhou Kai"
                cmd.Parameters.AddWithValue("@LEG_GROUP", haulierJobTrip.LegGroup);
                cmd.Parameters.AddWithValue("@LEG_GROUP_MEMBER", haulierJobTrip.LegGroupMember);
                cmd.Parameters.AddWithValue("@PARTNER_JOB_NO", haulierJobTrip.PartnerJobNo);
                #endregion

                ////20170112 - gerry added
                //ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_SET_JOBTRIP_TO_READY);
                //if (option.setting_Value.ToString() == "T")
                //    haulierJobTrip.TripStatus = haulierJobTrip.LegGroupMember == 1 ? JobTripStatus.Ready : haulierJobTrip.TripStatus;//set to ready
                ////20170112 ends
                int status = 0;
                if (haulierJobTrip.TripStatus == JobTripStatus.Booked)
                    status = 1;
                if (haulierJobTrip.TripStatus == JobTripStatus.Ready)
                    status = 2;
                if (haulierJobTrip.TripStatus == JobTripStatus.Assigned)
                    status = 3;
                if (haulierJobTrip.TripStatus == JobTripStatus.Completed)
                    status = 4;
                if (haulierJobTrip.TripStatus == JobTripStatus.Invoiced)
                    status = 5;

                cmd.Parameters.AddWithValue("@STATUS", status);
                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;


                #endregion

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                haulierJobTrip.JobNo = haulierJob.JobNo;
                haulierJobTrip.Sequence = Convert.ToInt32(newReqNumber.Value);
                haulierJobTrip.UpdateVersion = GetHaulierJobTripUpdateVersion(haulierJobTrip, cn, tran);
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobTrip. " + ex.Message); }

            return true;
        }

        internal static bool EditHaulierJobTrip(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT * FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + haulierJobTrip.JobID;
                SQLString += " AND SEQUENCE_NO = " + haulierJobTrip.Sequence;
                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                if (SqlBinary.Equals(haulierJobTrip.UpdateVersion, originalRowVersion) == false)
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nHaulierJobDAL.EditHaulierJobTrip");
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("sp_Edit_TPT_JOB_DETAIL_CARGO_Tbl", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JOB_ID", haulierJobTrip.JobID);
                    cmd.Parameters.AddWithValue("@SEQUENCE_NO", haulierJobTrip.Sequence);
                    cmd.Parameters.AddWithValue("@FROM_STOP", haulierJobTrip.StartStop.Code);
                    cmd.Parameters.AddWithValue("@FROM_ADD1", haulierJobTrip.StartStop.Address1);
                    cmd.Parameters.AddWithValue("@FROM_ADD2", haulierJobTrip.StartStop.Address2);
                    cmd.Parameters.AddWithValue("@FROM_ADD3", haulierJobTrip.StartStop.Address3);
                    cmd.Parameters.AddWithValue("@FROM_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJobTrip.StartDate));
                    cmd.Parameters.AddWithValue("@FROM_TIME", haulierJobTrip.StartTime);
                    cmd.Parameters.AddWithValue("@TO_STOP", haulierJobTrip.EndStop.Code);
                    cmd.Parameters.AddWithValue("@TO_ADD1", haulierJobTrip.EndStop.Address1);
                    cmd.Parameters.AddWithValue("@TO_ADD2", haulierJobTrip.EndStop.Address2);
                    cmd.Parameters.AddWithValue("@TO_ADD3", haulierJobTrip.EndStop.Address3);
                    cmd.Parameters.AddWithValue("@TO_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJobTrip.EndDate));
                    cmd.Parameters.AddWithValue("@TO_TIME", haulierJobTrip.EndTime);
                    cmd.Parameters.AddWithValue("@CONTAINER_CODE", haulierJobTrip.ContainerCode);
                    cmd.Parameters.AddWithValue("@CONTAINER_NUMBER", haulierJobTrip.ContainerNo);
                    cmd.Parameters.AddWithValue("@SEAL_NUMBER", haulierJobTrip.SealNo);
                    cmd.Parameters.AddWithValue("@GROSS_CBM", haulierJobTrip.GrossCBM);
                    cmd.Parameters.AddWithValue("@GROSS_WEIGHT", haulierJobTrip.GrossWeight);
                    cmd.Parameters.AddWithValue("@MAX_CBM", haulierJobTrip.maxCBM);
                    cmd.Parameters.AddWithValue("@MAX_WEIGHT", haulierJobTrip.maxWeight);
                    cmd.Parameters.AddWithValue("@CARGO_DESCRIPTION", haulierJobTrip.CargoDescription);
                    // 2014-05-21 Zhou Kai adds
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", haulierJobTrip.subCon.Code);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_NAME", haulierJobTrip.subCon.Name);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD1", haulierJobTrip.subCon.Add1);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD2", haulierJobTrip.subCon.Add2);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD3", haulierJobTrip.subCon.Add3);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD4", haulierJobTrip.subCon.Add4);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CITY_CODE", haulierJobTrip.subCon.City);
                    cmd.Parameters.AddWithValue("@IsBillable", haulierJobTrip.isBillable ? "T" : "F");
                    // 2014-07-25 Zhou Kai ends
                    // 2014-10-27 Zhou Kai adds
                    cmd.Parameters.AddWithValue("@OWNTRANSPORT", haulierJobTrip.OwnTransport == true ? "T" : "F");
                    // 2014-10-27 Zhou Kai ends 
                    if (haulierJobTrip.IsLaden == true)
                    {
                        cmd.Parameters.AddWithValue("@IS_LADEN", "T");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IS_LADEN", "F");
                    }
                    int status = 0;
                    if (haulierJobTrip.TripStatus == JobTripStatus.Booked)
                        status = 1;
                    if (haulierJobTrip.TripStatus == JobTripStatus.Ready)
                        status = 2;
                    if (haulierJobTrip.TripStatus == JobTripStatus.Assigned)
                        status = 3;
                    if (haulierJobTrip.TripStatus == JobTripStatus.Completed)
                        status = 4;
                    if (haulierJobTrip.TripStatus == JobTripStatus.Invoiced)
                        status = 5;

                    cmd.Parameters.AddWithValue("@STATUS", status);
                    if (haulierJobTrip.IsDangerousGoods == true)
                        cmd.Parameters.AddWithValue("@IS_DANGEROUS_GOODS", "T");
                    else
                        cmd.Parameters.AddWithValue("@IS_DANGEROUS_GOODS", "F");
                    cmd.Parameters.AddWithValue("@DG_REMARKS", haulierJobTrip.DGRemarks);
                    if (haulierJobTrip.IsOversize == true)
                        cmd.Parameters.AddWithValue("@IS_OVERSIZE", "T");
                    else
                        cmd.Parameters.AddWithValue("@IS_OVERSIZE", "F");
                    cmd.Parameters.AddWithValue("@OVERSIZE_REMARKS", haulierJobTrip.OversizeRemarks);
                    if (haulierJobTrip.IsDirectDelivery == true)
                        cmd.Parameters.AddWithValue("@IS_DIRECT_DELIVERY", "T");
                    else
                        cmd.Parameters.AddWithValue("@IS_DIRECT_DELIVERY", "F");
                    cmd.Parameters.AddWithValue("@CHARGE_CODE", haulierJobTrip.ChargeCode);
                    cmd.Parameters.AddWithValue("@REMARKS", haulierJobTrip.Remarks);
                    cmd.Parameters.AddWithValue("@LEG_TYPE", haulierJobTrip.LegType.GetHashCode());
                    cmd.Parameters.AddWithValue("@PARTNER_LEG", haulierJobTrip.PartnerLeg);

                    cmd.Parameters.AddWithValue("@PORT_REMARK", haulierJobTrip.PortRemark);
                    cmd.Parameters.AddWithValue("@YARD", haulierJobTrip.Yard);
                    cmd.Parameters.AddWithValue("@IS_MULTI_LEG", haulierJobTrip.isMulti_leg ? "T" : "F");
                    //20161008 - gerry added
                    cmd.Parameters.AddWithValue("@Modified_By", haulierJobTrip.LastModified_By);
                    cmd.Parameters.AddWithValue("@Modified_DateTime", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(haulierJobTrip.LastModifiedDateTime));

                    #region "2014-10-07 Zhou Kai"
                    cmd.Parameters.AddWithValue("@LEG_GROUP",
                        haulierJobTrip.LegGroup);
                    cmd.Parameters.AddWithValue("@LEG_GROUP_MEMBER",
                        haulierJobTrip.LegGroupMember);
                    cmd.Parameters.AddWithValue("@PARTNER_JOB_NO",
                        haulierJobTrip.PartnerJobNo);
                    #endregion

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : EditHaulierJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : EditHaulierJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : EditHaulierJobTrip. " + ex.Message); }
        }

        internal static bool DeleteHaulierJobTrip(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TPT_JOB_DETAIL_CARGO_Tbl", cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", haulierJobTrip.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", haulierJobTrip.Sequence);

                cmd.Transaction = tran;

                // 2014-10-09 Zhou Kai adds
                ChangeLegGroupMember(haulierJobTrip, cn, tran);
                // 2014-10-09 Zhou Kai ends
                cmd.ExecuteNonQuery();

            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJobTrip. " + ex.Message); }
            return true;
        }

        private static void DisplayParameters(SqlCommand cmd)
        {
            string msg = "";

            for (int i = 0; i < cmd.Parameters.Count; i++)
            {
                msg += cmd.Parameters[i].ParameterName + ", " + cmd.Parameters[i].Value + "\n";
            }
            Console.WriteLine(msg);
            Console.ReadLine();
        }

        internal static bool AddJobTripState(JobTrip jobtrip, JobTripState jobTripState, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
                SQLString += " WHERE TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID = " + jobtrip.JobID;
                SQLString += " AND SEQUENCE_NO = " + jobtrip.Sequence;

                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPD_VER"];
                }
                reader.Close();
                if (SqlBinary.Equals(jobtrip.UpdateVersion, originalRowVersion) == false)
                {
                    // 2014-08-18 Zhou Kai use language resources instead hard-code string
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("sp_Insert_TPT_JOB_TRIP_STATE_Tbl", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JOB_ID", jobtrip.JobID);
                    cmd.Parameters.AddWithValue("@SEQUENCE_NO", jobtrip.Sequence);
                    cmd.Parameters.AddWithValue("@JOB_STATE_NO", jobTripState.Seq_No);
                    cmd.Parameters.AddWithValue("@STATUS", jobTripState.Status);
                    cmd.Parameters.AddWithValue("@STATUSDATE", DateUtility.ConvertDateAndTimeForSQLPurpose(jobTripState.StatusDate));
                    cmd.Parameters.AddWithValue("@REMARKS", jobTripState.Remarks);

                    SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                    cmd.Parameters.Add(newReqNumber);
                    newReqNumber.Direction = ParameterDirection.Output;

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    jobTripState.Seq_No = Convert.ToInt32(newReqNumber.Value);

                    return true;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : AddJobTripState. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : AddJobTripState. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : AddJobTripState. " + ex.Message); }
        }

        internal static bool EditJobTripState(JobTrip jobtrip, JobTripState jobTripState, SqlConnection cn, SqlTransaction tran)
        {
            byte[] originalRowVersion = new byte[8];
            string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
            SQLString += " WHERE TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID = " + jobtrip.JobID;
            SQLString += " AND SEQUENCE_NO = " + jobtrip.Sequence;
            SqlCommand cmd1 = new SqlCommand(SQLString, cn);
            cmd1.CommandTimeout = 0;
            cmd1.CommandType = CommandType.Text;
            cmd1.Transaction = tran;
            IDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
                originalRowVersion = (byte[])reader["UPD_VER"];
            }
            reader.Close();
            if (SqlBinary.Equals(jobtrip.UpdateVersion, originalRowVersion) == false)
            {
                throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nHaulierJobDAL.EditJobTripState");
                return false;
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_Edit_TPT_JOB_TRIP_STATE_Tbl", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JOB_ID", jobtrip.JobID);
                    cmd.Parameters.AddWithValue("@SEQUENCE_NO", jobtrip.Sequence);
                    cmd.Parameters.AddWithValue("@JOB_STATE_NO", jobTripState.Seq_No);
                    cmd.Parameters.AddWithValue("@STATUS", jobTripState.Status);
                    cmd.Parameters.AddWithValue("@STATUSDATE", DateUtility.ConvertDateAndTimeForSQLPurpose(jobTripState.StatusDate));
                    cmd.Parameters.AddWithValue("@REMARKS", jobTripState.Remarks);


                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : EditJobTripState. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : EditJobTripState. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : EditJobTripState. " + ex.Message); }
            }
            return true;

        }

        internal static bool DeleteJobTripState(JobTrip jobtrip, JobTripState jobTripState, SqlConnection cn, SqlTransaction tran)
        {

            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TPT_JOB_TRIP_STATE_Tbl", cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", jobtrip.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", jobtrip.Sequence);
                cmd.Parameters.AddWithValue("@STATUS", jobTripState.Status);

                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                int status = Convert.ToInt32(newReqNumber.Value);
                if (status == 1) { jobtrip.TripStatus = JobTripStatus.Booked; }
                if (status == 2) { jobtrip.TripStatus = JobTripStatus.Ready; }


                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : DeleteJobTripState. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : DeleteJobTripState. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : DeleteJobTripState. " + ex.Message); }
            return true;

        }

        internal static bool AddHaulierJobCharges(HaulierJobCharge haulierJobCharge, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_JOB_DETAIL_CHARGE_Tbl", con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@INV_TRX_NO", haulierJobCharge.InvTranNo);
                cmd.Parameters.AddWithValue("@INV_ITEM_SEQ_NO", haulierJobCharge.InvItemSeqNo);
                cmd.Parameters.AddWithValue("@JOB_ID", haulierJobCharge.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", haulierJobCharge.SequenceNo);
                cmd.Parameters.AddWithValue("@CHARGE_CODE", haulierJobCharge.ChargeCode);
                cmd.Parameters.AddWithValue("@DESCRIPTION", haulierJobCharge.ChargeDescription);
                cmd.Parameters.AddWithValue("@UOM", haulierJobCharge.Uom);
                cmd.Parameters.AddWithValue("@QUANTITY", haulierJobCharge.Quantity);
                cmd.Parameters.AddWithValue("@UNIT_RATE", haulierJobCharge.UnitRateFC);
                cmd.Parameters.AddWithValue("@CURRENCY", haulierJobCharge.Currency);
                cmd.Parameters.AddWithValue("@EXCHANGE_RATE", haulierJobCharge.ExchangeRate);
                cmd.Parameters.AddWithValue("@AMOUNT_FC", haulierJobCharge.TotalAmountFC);
                cmd.Parameters.AddWithValue("@AMOUNT_HC", haulierJobCharge.TotalAmountHC);
                cmd.Parameters.AddWithValue("@GST_TYPE", haulierJobCharge.GstType);
                cmd.Parameters.AddWithValue("@GST_RATE", haulierJobCharge.GstRate);
                cmd.Parameters.AddWithValue("@GST_AMOUNT_HC", haulierJobCharge.GstAmountHC);
                cmd.Parameters.AddWithValue("@QUOTATION_ID", haulierJobCharge.QuotationID);
                cmd.Parameters.AddWithValue("@QUOTATION_NO", haulierJobCharge.QuotationNo);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO_RATE", haulierJobCharge.SequenceNoRate);
                int status = 0;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;
                cmd.Parameters.AddWithValue("@JOB_CHARGE_STATUS", status);
                cmd.Parameters.AddWithValue("@REMARKS", haulierJobCharge.Remarks);
                cmd.Parameters.AddWithValue("@JOB_CHARGE_TYPE", haulierJobCharge.JobChargeType);
                cmd.Parameters.AddWithValue("@COMPLETED_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(haulierJobCharge.CompletedDate));
                cmd.Parameters.AddWithValue("@CONTAINER_NUMBER", haulierJobCharge.ContainerNo);
                cmd.Parameters.AddWithValue("@RATE_TYPE", haulierJobCharge.RateType);

                //Temporary assign value while waiting for the specs
                cmd.Parameters.AddWithValue("@GST_AMOUNT_FC", haulierJobCharge.GstAmountFC);

                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                haulierJobCharge.SequenceNo = Convert.ToInt32(newReqNumber.Value);

                return true;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobCharges. " + ex.Message); }

            return true;


        }

        internal static bool EditHaulierJobCharges(HaulierJobCharge haulierJobCharge, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Edit_TPT_JOB_DETAIL_CHARGE_Tbl", con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JOB_ID", haulierJobCharge.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", haulierJobCharge.SequenceNo);
                cmd.Parameters.AddWithValue("@DESCRIPTION", haulierJobCharge.ChargeDescription);
                cmd.Parameters.AddWithValue("@QUANTITY", haulierJobCharge.Quantity);
                cmd.Parameters.AddWithValue("@UNIT_RATE", haulierJobCharge.UnitRateFC);
                cmd.Parameters.AddWithValue("@CURRENCY", haulierJobCharge.Currency);
                cmd.Parameters.AddWithValue("@EXCHANGE_RATE", haulierJobCharge.ExchangeRate);
                cmd.Parameters.AddWithValue("@GST_TYPE", haulierJobCharge.GstType);
                cmd.Parameters.AddWithValue("@GST_RATE", haulierJobCharge.GstRate);
                cmd.Parameters.AddWithValue("@AMOUNT_FC", haulierJobCharge.TotalAmountFC);
                cmd.Parameters.AddWithValue("@AMOUNT_HC", haulierJobCharge.TotalAmountHC);
                cmd.Parameters.AddWithValue("@QUOTATION_NO", haulierJobCharge.QuotationNo);
                cmd.Parameters.AddWithValue("@RATE_TYPE", haulierJobCharge.RateType);
                cmd.Parameters.AddWithValue("@GST_AMOUNT_HC", haulierJobCharge.GstAmountHC);
                cmd.Parameters.AddWithValue("@CONTAINER_NO", haulierJobCharge.ContainerNo);

                //Temporary assign value while waiting for the specs
                cmd.Parameters.AddWithValue("@GST_AMOUNT_FC", haulierJobCharge.GstAmountFC);

                int status = 0;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;
                cmd.Parameters.AddWithValue("@JOB_CHARGE_STATUS", status);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : EditHaulierJobCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : EditHaulierJobCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : EditHaulierJobCharges. " + ex.Message); }
            return true;
        }

        internal static bool DeleteHaulierJobCharges(HaulierJobCharge haulierJobCharge, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TPT_JOB_DETAIL_CHARGE_Tbl", con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", haulierJobCharge.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", haulierJobCharge.SequenceNo);


                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJobCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJobCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : DeleteHaulierJobCharges. " + ex.Message); }

            return true;
        }

        /// <summary>
        /// 2014-08-08 Zhou Kai adds
        /// </summary>
        /// <param name="jobID"></param>
        /// <param name="con"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        internal static bool DeleteAllContainerMovementCharges(int jobID, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string query = @"DELETE FROM TPT_JOB_DETAIL_CHARGE_Tbl
 	                                      WHERE 
	                                      JOB_ID={0} AND
	                                      JOB_CHARGE_TYPE='T'";

                query = string.Format(query, jobID);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : DeleteAllContainerMovementCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : DeleteAllContainerMovementCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : DeleteAllContainerMovementCharges. " + ex.Message); }
            return true;
        }

        /// <summary>
        /// 2014-08-08 Zhou Kai adds, the same version with the above function, 
        /// with on dbCon and dbTran as params
        /// </summary>
        /// <returns></returns>
        internal static bool DeleteAllContainerMovementCharges(int jobID)
        {
            using (SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string query = @"DELETE FROM TPT_JOB_DETAIL_CHARGE_Tbl
 	                                      WHERE 
	                                      JOB_ID={0} AND
	                                      JOB_CHARGE_TYPE='T'";

                    query = string.Format(query, jobID);
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    SqlCommand cmd = new SqlCommand(query, dbCon);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : DeleteAllContainerMovementCharges. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : DeleteAllContainerMovementCharges. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : DeleteAllContainerMovementCharges. " + ex.Message); }
            }
            return true;
        }

        internal static bool UpdateHaulierJobTripFromPlanning(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
                SQLString += " WHERE TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID = " + haulierJobTrip.JobID;
                SQLString += " AND SEQUENCE_NO = " + haulierJobTrip.Sequence;
                //added incase of the 2nd leg was assigned first before the first leg
                SQLString += " AND LEG_TYPE = " + haulierJobTrip.LegType.GetHashCode();
                //end
                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPD_VER"];
                }
                reader.Close();
                //temporary fixed form update containNo from planning
                //to be fixed later
                if (!SqlBinary.Equals(haulierJobTrip.UpdateVersion, originalRowVersion) && haulierJobTrip.LegType != Leg_Type.SecondOfTwoLeg)
                {
                    //                        tran.Rollback();
                    throw new FMException("The data has been changed by someone else. Please refresh the data and update");
                    return false;
                }
                else
                {
                    //Change sp 
                    SqlCommand cmd = new SqlCommand("sp_Edit_TPT_JOBTRIP_ContNo_YRD_PRTRMK_Stop", cn);
                    cmd.CommandTimeout = 0;
                    //SqlCommand cmd = new SqlCommand("sp_Edit_TPT_JOBTRIP_ContNo_YRD_PRTRMK", cn);
                    //SqlCommand cmd = new SqlCommand("sp_UPDATE_TPT_JOBTRIP_ContNo_YRD_PRTRMK", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@JOB_ID", haulierJobTrip.JobID);
                    cmd.Parameters.AddWithValue("@SEQUENCE_NO", haulierJobTrip.Sequence);
                    cmd.Parameters.AddWithValue("@CONTAINER_NUMBER", haulierJobTrip.ContainerNo == null ? string.Empty : haulierJobTrip.ContainerNo);
                    //March 26, 2012 
                    cmd.Parameters.AddWithValue("@SEAL_NUMBER", haulierJobTrip.SealNo == null ? string.Empty : haulierJobTrip.SealNo);
                    cmd.Parameters.AddWithValue("@PORTREMARKS", haulierJobTrip.PortRemark == null ? string.Empty : haulierJobTrip.PortRemark);
                    cmd.Parameters.AddWithValue("@YARD", haulierJobTrip.Yard == null ? string.Empty : haulierJobTrip.Yard);
                    cmd.Parameters.AddWithValue("@LEGTYPE", haulierJobTrip.LegType.GetHashCode());
                    //end
                    //20130415 - gerry added 
                    cmd.Parameters.AddWithValue("@FROM_STOP", haulierJobTrip.StartStop.Code.ToString().Trim());
                    cmd.Parameters.AddWithValue("@FROM_ADD1", haulierJobTrip.StartStop.Address1.ToString().Trim());
                    cmd.Parameters.AddWithValue("@FROM_ADD2", haulierJobTrip.StartStop.Address2.ToString().Trim());
                    cmd.Parameters.AddWithValue("@FROM_ADD3", haulierJobTrip.StartStop.Address3.ToString().Trim());
                    cmd.Parameters.AddWithValue("@FROM_ADD4", haulierJobTrip.StartStop.Address4.ToString().Trim());
                    cmd.Parameters.AddWithValue("@TO_STOP", haulierJobTrip.EndStop.Code.ToString().Trim());
                    cmd.Parameters.AddWithValue("@TO_ADD1", haulierJobTrip.EndStop.Address1.ToString().Trim());
                    cmd.Parameters.AddWithValue("@TO_ADD2", haulierJobTrip.EndStop.Address2.ToString().Trim());
                    cmd.Parameters.AddWithValue("@TO_ADD3", haulierJobTrip.EndStop.Address3.ToString().Trim());
                    cmd.Parameters.AddWithValue("@TO_ADD4", haulierJobTrip.EndStop.Address4.ToString().Trim());
                    //20130415 end
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateHaulierJobTripFromPlanning. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateHaulierJobTripFromPlanning. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateHaulierJobTripFromPlanning. " + ex.Message); }

        }

        internal static bool UpdateSubContractor(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
                SQLString += " WHERE TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID = " + haulierJobTrip.JobID;
                SQLString += " AND SEQUENCE_NO = " + haulierJobTrip.Sequence;
                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPD_VER"];
                }
                reader.Close();
                if (SqlBinary.Equals(haulierJobTrip.UpdateVersion, originalRowVersion) == false)
                {
                    //                        tran.Rollback();
                    throw new FMException("The data has been changed by someone else. Please refresh the data and update");
                    return false;
                }
                else
                {
                    //SqlCommand cmd = new SqlCommand("sp_Update_Subcontractor", cn);
                    SqlCommand cmd = new SqlCommand("sp_Edit_TPT_Subcontractor", cn);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@JOB_ID", haulierJobTrip.JobID);
                    cmd.Parameters.AddWithValue("@SEQUENCE_NO", haulierJobTrip.Sequence);
                    //20140312-gerry replaced
                    //cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", haulierJobTrip.SubContractorCode);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", haulierJobTrip.subCon.Code.ToString());
                    //20140312 end

                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_REFERENCE", CommonUtilities.FormatString(haulierJobTrip.SubContractorReference));

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateSubContractor. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateSubContractor. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateSubContractor. " + ex.Message); }

        }

        internal static byte[] GetHaulierJobTripUpdateVersion(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            byte[] originalRowVersion = new byte[8];
            try
            {
                string SQLString = "SELECT * FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
                SQLString += " WHERE JOB_ID = " + haulierJobTrip.JobID;
                SQLString += " AND SEQUENCE_NO = " + haulierJobTrip.Sequence;
                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripUpdateVersion. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripUpdateVersion. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripUpdateVersion. " + ex.Message); }
            return originalRowVersion;

        }

        internal static SortableList<HaulierJobTrip> GetJobTrips(JobTripStatus status)
        {
            string sqlWhere = " WHERE STATUS = " + status.GetHashCode();
            sqlWhere += " ORDER BY ";
            return GetJobTrips(sqlWhere);
        }

        // Chong Chin 19 Dec 09, add 1 more parameter, planDate. Only HaulierJobTrips on or before
        // this planDate should be retrieved to planning

        internal static SortableList<HaulierJobTrip> GetJobTrips(JobTripStatus status, bool owntransport, string tptDeptCode, DateTime planDate, string jobType)
        {
            //   string sqlWhere = " WHERE STATUS = " + status.GetHashCode();
            string sqlWhere = " WHERE STATUS = " + status.GetHashCode();
            sqlWhere += " AND TPT_JOB_MAIN_Tbl.TPT_DEPT_CODE = '" + CommonUtilities.FormatString(tptDeptCode) + "'";
            if (owntransport == true)
            {
                sqlWhere += " AND TPT_JOB_DETAIL_CARGO_Tbl.OWNTRANSPORT = 'T'";
            }
            else
            {
                sqlWhere += " AND TPT_JOB_DETAIL_CARGO_Tbl.OWNTRANSPORT = 'F'";
            }
            //**** Chong Chin 19 Dec 09 Start ************************
            //**** Fuandi 26 July 10 Start - Should add in the last hour and minute of the day
            // because 1 leg trips were saved with time
            sqlWhere += " AND FROM_DATE <= '" + DateUtility.ConvertDateForSQLPurpose(planDate) + LAST_MINUTE_OF_DAY + "'";

            if (jobType != "")
            {
                sqlWhere += " AND JOB_TYPE = '" + jobType + "'";
            }

            sqlWhere += " ORDER BY TPT_JOB_DETAIL_CARGO_Tbl.LEG_TYPE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_TIME, TPT_JOB_MAIN_Tbl.JOB_NUMBER";

            // ********** 26 july end *******************
            // **********19 Dec 09  End ******************

            return GetJobTrips(sqlWhere);
        }

        internal static SortableList<HaulierJobTrip> GetJobTrips(int startJobID, int endJobID)
        {
            endJobID = endJobID + 1;
            string sqlWhere = " WHERE TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID >= " + startJobID;
            sqlWhere += " AND TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID < " + endJobID;
            sqlWhere += " ORDER BY TPT_JOB_DETAIL_CARGO_Tbl.LEG_TYPE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_TIME, TPT_JOB_MAIN_Tbl.JOB_NUMBER";

            return GetJobTrips(sqlWhere);
        }

        internal static SortableList<HaulierJobTrip> GetJobTrips(string startJobNo, string endJobNo, JobTripStatus status, string tptDept)
        {
            string sqlWhere = " WHERE TPT_JOB_MAIN_Tbl.JOB_NUMBER >= '" + startJobNo + "'";
            sqlWhere += " AND TPT_JOB_MAIN_Tbl.JOB_NUMBER <= '" + endJobNo + "'";
            sqlWhere += " AND TPT_JOB_MAIN_Tbl.TPT_DEPT_CODE = '" + CommonUtilities.FormatString(tptDept) + "'";
            sqlWhere += " AND STATUS = " + status.GetHashCode();
            sqlWhere += " ORDER BY TPT_JOB_DETAIL_CARGO_Tbl.LEG_TYPE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_TIME, TPT_JOB_MAIN_Tbl.JOB_NUMBER";

            return GetJobTrips(sqlWhere);
        }

        internal static SortableList<HaulierJobTrip> GetJobTrips(DateTime startJobDate, DateTime endJobDate)
        {
            string sqlWhere = " WHERE TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(startJobDate) + "'";
            sqlWhere += " AND TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE < '" + DateUtility.ConvertDateForSQLPurpose(endJobDate + TimeSpan.FromDays(1)) + "'";
            sqlWhere += " ORDER BY TPT_JOB_DETAIL_CARGO_Tbl.LEG_TYPE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_TIME, TPT_JOB_MAIN_Tbl.JOB_NUMBER";

            return GetJobTrips(sqlWhere);
        }

        internal static SortableList<HaulierJobTrip> GetJobTrips(DateTime startJobDate, DateTime endJobDate, JobTripStatus status, string tptDept)
        {
            string sqlWhere = " WHERE TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(startJobDate) + "'";
            sqlWhere += " AND TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE < '" + DateUtility.ConvertDateForSQLPurpose(endJobDate + TimeSpan.FromDays(1)) + "'";
            sqlWhere += " AND TPT_JOB_MAIN_Tbl.TPT_DEPT_CODE = '" + CommonUtilities.FormatString(tptDept) + "'";
            sqlWhere += " AND STATUS = " + status.GetHashCode();
            sqlWhere += " ORDER BY TPT_JOB_DETAIL_CARGO_Tbl.LEG_TYPE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_TIME, TPT_JOB_MAIN_Tbl.JOB_NUMBER";

            return GetJobTrips(sqlWhere);
        }

        internal static SortableList<HaulierJobTrip> GetJobTrips(bool ownTransport)
        {
            string sqlWhere = "";
            if (ownTransport == true)
            {
                sqlWhere = " WHERE OWNTRANSPORT = 'T'";
            }
            else
            {
                sqlWhere = " WHERE OWNTRANSPORT = 'F'";
            }

            sqlWhere += " ORDER BY TPT_JOB_DETAIL_CARGO_Tbl.LEG_TYPE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_DATE, TPT_JOB_DETAIL_CARGO_Tbl.FROM_TIME, TPT_JOB_MAIN_Tbl.JOB_NUMBER";

            return GetJobTrips(sqlWhere);
        }

        private static SortableList<HaulierJobTrip> GetJobTrips(string sqlWhere)
        {
            SortableList<HaulierJobTrip> Operators = new SortableList<HaulierJobTrip>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK) ";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_Tbl with (NOLOCK) ON TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID=TPT_JOB_MAIN_Tbl.JOB_ID";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK) ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID ";
                SQLString += sqlWhere;
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip trip = GetHaulierJobTrip(reader);
                    trip.JobTripStates = GetAllHaulierJobTripStates(trip);
                    Operators.Add(trip);

                    //Operators.Add(GetHaulierJobTrip(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetOwnBookJobTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetOwnBookJobTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetOwnBookJobTrips. " + ex.Message); }
            return Operators;
        }

        internal static SortableList<HaulierJobTrip> GetOwnBookJobTrips(JobTripStatus status, DateTime date1, string tptDeptCode)
        {
            int book = JobTripStatus.Booked.GetHashCode();
            int complete = JobTripStatus.Completed.GetHashCode();
            int SecondOfTwoLeg = Leg_Type.SecondOfTwoLeg.GetHashCode();

            SortableList<HaulierJobTrip> Operators = new SortableList<HaulierJobTrip>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = "SELECT A.UPDATE_VERSION AS UPD_VER,A.*,TPT_JOB_MAIN_Tbl.*";
                SQLString += " FROM TPT_JOB_DETAIL_CARGO_Tbl A with (NOLOCK)";
                SQLString += " INNER JOIN TPT_JOB_DETAIL_CARGO_Tbl B with (NOLOCK)";
                SQLString += " ON ( ((A.LEG_TYPE = " + SecondOfTwoLeg + " AND  (A.PARTNER_LEG = B.SEQUENCE_NO AND A.JOB_ID = B.JOB_ID AND B.[STATUS] = " + complete + ")))";
                // 10 Dec 09 by CC, remove condition "B.[STATUS] =4" for cases where job is not 2nd of 2 Legs
                // SQLString += " OR (A.LEG_TYPE <> 22 AND A.JOB_ID = B.JOB_ID AND A.SEQUENCE_NO = B.SEQUENCE_NO AND B.[STATUS] =4))";
                SQLString += " OR (A.LEG_TYPE <> " + SecondOfTwoLeg + " AND A.JOB_ID = B.JOB_ID AND A.SEQUENCE_NO = B.SEQUENCE_NO ))";

                SQLString += " LEFT JOIN TPT_JOB_MAIN_Tbl with (NOLOCK) ON A.JOB_ID=TPT_JOB_MAIN_Tbl.JOB_ID ";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON TPT_JOB_MAIN_Tbl.JOB_ID=msi.JOB_ID ";
                SQLString += " WHERE A.FROM_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date1) + "'";
                SQLString += " AND A.FROM_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date1 + TimeSpan.FromDays(1)) + "'";
                SQLString += " AND TPT_JOB_MAIN_Tbl.TPT_DEPT_CODE = '" + CommonUtilities.FormatString(tptDeptCode) + "'";
                SQLString += " AND A.[STATUS] = " + book;

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip trip = GetHaulierJobTrip(reader);
                    trip.JobTripStates = GetAllHaulierJobTripStates(trip);
                    Operators.Add(trip);

                    //Operators.Add(GetHaulierJobTrip(reader));
                }
                cn.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetOwnBookJobTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetOwnBookJobTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetOwnBookJobTrips. " + ex.Message); }
            return Operators;

        }

        // 2014-10-13 Zhou Kai modifies the function name:
        // from ForSecondLeg to ForLegsOfSameLegGroup
        internal static bool UpdateContainerSealNoForLegsOfSameLegGroup(HaulierJobTrip haulierJobTrip,
            SqlConnection cn, SqlTransaction tran)
        {
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string SQLUpdateDriverString =
                    " Update TPT_JOB_DETAIL_CARGO_Tbl set " +
                    " CONTAINER_NUMBER ='" + CommonUtilities.FormatString(haulierJobTrip.ContainerNo) + "'" +
                    ", SEAL_NUMBER ='" + CommonUtilities.FormatString(haulierJobTrip.SealNo) + "'" +
                    // 2014-08-27 Zhou Kai adds container code into the t-sql query
                    ", CONTAINER_CODE = '" + CommonUtilities.FormatString(haulierJobTrip.ContainerCode) + "'" +
                    // 2014-08-27 Zhou Kai ends
                    " Where JOB_ID = " + haulierJobTrip.JobID +
                    // 2014-10-13 Zhou Kai modifes the condition below, to update a whole leg-group
                     " AND LEG_GROUP = " + haulierJobTrip.LegGroup;
                // 2014-10-13 Zhou Kai ends

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForLegsOfSameLegGroup. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForLegsOfSameLegGroup. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForLegsOfSameLegGroup. " + ex.Message); }

            return true;
        }

        /// <summary>
        /// 2014-08-27 Zhou Kai adds
        /// </summary>
        /// <param name="frmName">The name of the form from which the operation is initialized</param>
        /// <param name="cn">The database connection object</param>
        /// <param name="tran">The database operation transaction</param>
        /// <returns>true, operation succeeded; false, operation failed</returns>
        internal static bool UpdateBillableNConCodeForPartnerLeg(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string SQLUpdateDriverString = " Update TPT_JOB_DETAIL_CARGO_Tbl set " +
                                               " CONTAINER_CODE = '" + CommonUtilities.FormatString(haulierJobTrip.ContainerCode) + "'" +
                                               ", IsBillable = " + (haulierJobTrip.isBillable ? "'T'" : "'F'") +
                                               " Where JOB_ID = " + haulierJobTrip.JobID +
                                               " AND SEQUENCE_NO = " + haulierJobTrip.PartnerLeg;

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateBillableNConCodeForPartnerLeg. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateBillableNConCodeForPartnerLeg. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateBillableNConCodeForPartnerLeg. " + ex.Message); }

            return true;
        }

        //20130417 - gerry added
        //use in planning incase the remaining leg is in different schedule date,
        //we dont know the remaining leg anymore unlike in booking we can see in the grid, so we must also update the containerNo and sealNo
        internal static bool UpdateContainerSealNoForAllRemainingLeg(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                //20141201 - gerry modify query - replaced container_number to leg_group
                // 2014-08-27 Zhou Kai adds container code into the t-sql query
                string SQLUpdateString = @"Update TPT_JOB_DETAIL_CARGO_Tbl set 
                                                CONTAINER_NUMBER ='{0}',
                                                SEAL_NUMBER = '{1}'
                                               , CONTAINER_CODE = '{2}'
                                                WHERE JOB_ID = {3}
                                                and LEG_GROUP = {4}   /*CONTAINER_NUMBER='{4}'*/ 
                                                and IS_MULTI_LEG = 'T'";
                // 20140922 = gerry reverted the codition from transport83 version JOB_ID, CONTAINER_NUMBER,SEAL_NUMBER,IS_MULTI_LEG
                SQLUpdateString = string.Format(SQLUpdateString,
                                                CommonUtilities.FormatString(haulierJobTrip.ContainerNo),
                                                CommonUtilities.FormatString(haulierJobTrip.SealNo),
                                                CommonUtilities.FormatString(haulierJobTrip.ContainerCode),
                                                haulierJobTrip.JobID,
                                                haulierJobTrip.LegGroup); //20141201 - gerry replaced based on leg group(new property); haulierJobTrip.oldContainerNo, // 20140922 = gerry added codition to container number
                                                //haulierJobTrip.SealNo);

                SqlCommand cmd = new SqlCommand(SQLUpdateString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForAllRemainingLeg. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForAllRemainingLeg. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForAllRemainingLeg. " + ex.Message); }

            return true;
        }

        internal static bool IsJobBilled(string jobNo)
        {
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = "SELECT * FROM ACT_IV_Transport_Invoice_Master_Tbl with (NOLOCK) ";
                    SQLString += " WHERE Job_Number='" + jobNo + "'";

                    SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
                    DataSet dsSearchResult = new DataSet();
                    daSearchCmd.Fill(dsSearchResult);
                    cn.Close();
                    return dsSearchResult.Tables[0].Rows.Count > 0;
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : IsJobBilled. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : IsJobBilled. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : IsJobBilled. " + ex.Message); }
            }
        }
        internal static bool IsJobPosted(string jobNo)
        {
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = "SELECT * FROM ACT_IV_Transport_Invoice_Master_Tbl with (NOLOCK) ";
                    SQLString += " WHERE Job_Number='" + jobNo + "'";
                    SQLString += " AND Posted_Y_N= 'T'";

                    SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
                    DataSet dsSearchResult = new DataSet();
                    daSearchCmd.Fill(dsSearchResult);
                    cn.Close();
                    return dsSearchResult.Tables[0].Rows.Count > 0;
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : IsJobPosted. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : IsJobPosted. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : IsJobPosted. " + ex.Message); }
            }
        }

        internal static SortableList<HaulierJobCharge> GetInvoiceHaulierJobCharges(HaulierJob haulierJob, SqlConnection cn, SqlTransaction tran)
        {
            SortableList<HaulierJobCharge> Operators = new SortableList<HaulierJobCharge>();
            try
            {
                string SQLString = @"SELECT * FROM TPT_JOB_DETAIL_CHARGE_Tbl with (NOLOCK) 
                                    WHERE JOB_ID = {0}
                                        AND JOB_CHARGE_STATUS = {1}";

                SQLString = string.Format(SQLString, haulierJob.JobID, JobChargeStatus.Invoiced.GetHashCode());
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                if (tran == null) { tran = cn.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;

                cmd.Transaction = tran;

                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetHaulierJobCharge(reader));
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetInvoiceHaulierJobCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetInvoiceHaulierJobCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetInvoiceHaulierJobCharges. " + ex.Message); }
            return Operators;

        }

        //use for adding invoice Item
        internal static bool SetHaulierJobChargeStatus(HaulierJobCharge haulierJobCharge, SqlConnection con, SqlTransaction tran)
        {
            bool success = true;
            try
            {
                int status = 0;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;

                string sqlString = @"Update TPT_JOB_DETAIL_CHARGE_Tbl 
                                        set JOB_CHARGE_STATUS = {0},
                                            INV_TRAN_NO = {1},
                                            INV_ITEM_SEQ_NO = {2}
                                        Where JOB_ID = {3}
                                            AND SEQUENCE_NO = {4}";

                sqlString = string.Format(sqlString, status, haulierJobCharge.InvTranNo, haulierJobCharge.InvItemSeqNo, haulierJobCharge.JobID, haulierJobCharge.SequenceNo);

                SqlCommand cmd = new SqlCommand(sqlString, con);
                cmd.CommandTimeout = 0;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobChargeStatus. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobChargeStatus. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobChargeStatus. " + ex.Message); }
            return success;
        }

        //use for deleting invoice Item
        internal static bool SetHaulierJobChargeStatus(HaulierJobCharge hauleirJobCharge, int invTrxNo, int invItemSeqNo, int jobID, bool deleteAll, SqlConnection con, SqlTransaction tran)
        {
            bool success = true;
            try
            {
                string sqlString = "";
                int status = 0;
                if (hauleirJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (hauleirJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (hauleirJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;

                if (!deleteAll)
                {
                    sqlString = @"Update TPT_JOB_DETAIL_CHARGE_Tbl 
                                        set JOB_CHARGE_STATUS = {0}
                                        Where JOB_ID = {1}
                                               AND INV_TRAN_NO = {2}
                                               AND INV_ITEM_SEQ_NO = {3}";
                }
                else
                {
                    sqlString = @"Update TPT_JOB_DETAIL_CHARGE_Tbl 
                                        set JOB_CHARGE_STATUS = {0}
                                        Where JOB_ID = {1}
                                            AND INV_TRAN_NO = {2}";
                }
                sqlString = string.Format(sqlString, status, jobID, invTrxNo, invItemSeqNo);

                SqlCommand cmd = new SqlCommand(sqlString, con);
                cmd.CommandTimeout = 0;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobChargeStatus. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobChargeStatus. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobChargeStatus. " + ex.Message); }
            return success;
        }



        //Filter Data from Criteria
        #region Method for Criteria
        //start for criteria 
        internal static SortableList<HaulierJobTrip> GetHaulierJobTrips(string sqlWhere)
        {
            SortableList<HaulierJobTrip> jobTrips = new SortableList<HaulierJobTrip>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = @"SELECT D.UPDATE_VERSION AS UPD_VER,
	                                    (select case M.JOB_TRANSFER_TYPE when '2' then
		                                    convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
								                                    where (a.JOB_ID = D.JOB_ID and a.CONTAINER_NUMBER = D.CONTAINER_NUMBER and a.LEG_GROUP = D.LEG_GROUP) 
										                                    OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER))
										                                    else  
		                                    convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
								                                    where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
										                                    OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) 
										                                    end) as Leg, 
                                        * 
                                        FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID ";
                    SQLString += sqlWhere;
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                        jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                        jobTrip.LegTypeCustomized = reader["Leg"] == DBNull.Value ? string.Empty : reader["Leg"].ToString();
                        jobTrips.Add(jobTrip);
                    }
                    cn.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrips. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrips. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTrips. " + ex.Message); }
                finally { cn.Close(); }
            }
            return jobTrips;
        }
        //20120914 - Gerry modify query to get job trip for SetBookedToReady 
        //for 2nd leg only if firstleg is completed
        //and for remaining leg only after 2nd leg is completed
        //20141119 - gerry modified the query to cater for transfer in job
        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsSetBookedToReady(string sqlWhere)
        {
            SortableList<HaulierJobTrip> jobTrips = new SortableList<HaulierJobTrip>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    #region old query
                    /*
                    string SQLString = @"SELECT D.UPDATE_VERSION AS UPD_VER,D.FROM_DATE,
                                    convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a 
									                            where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
											                            OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl as D
                                INNER JOIN TPT_JOB_MAIN_Tbl as M ON D.JOB_ID = M.JOB_ID 
                                INNER JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi ON M.JOB_ID=msi.JOB_ID 
                                    where (D.LEG_GROUP_MEMBER = 1 and D.IS_MULTI_LEG ='T')
                                    {0} --filter
                                     
                                    union all

                                SELECT D.UPDATE_VERSION AS UPD_VER,D.FROM_DATE,
                                    convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a 
									                            where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
											                            OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl as D
                                INNER JOIN TPT_JOB_MAIN_Tbl as M ON D.JOB_ID = M.JOB_ID 
                                INNER JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi ON M.JOB_ID=msi.JOB_ID 
                                    where (D.LEG_TYPE = 1 and D.IS_MULTI_LEG ='F')
                                    {0} --filter
                                     
                                    union all 
                                     
                                SELECT D.UPDATE_VERSION AS UPD_VER,D.FROM_DATE,
                                    convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a 
									                            where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
											                            OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl as D
                                INNER JOIN TPT_JOB_MAIN_Tbl as M ON D.JOB_ID = M.JOB_ID     
                                INNER JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi ON M.JOB_ID=msi.JOB_ID 
                                    where D.LEG_TYPE = 21 
                                    {0} --filter

                                    union all 
                                      
                                SELECT D.UPDATE_VERSION AS UPD_VER,D.FROM_DATE,
                                    convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a 
									                            where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
											                            OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl as D
                                INNER JOIN TPT_JOB_MAIN_Tbl as M ON D.JOB_ID = M.JOB_ID    
                                INNER JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi ON M.JOB_ID=msi.JOB_ID 
                                    where 
                                    D.LEG_TYPE = 22
                                    and D.sequence_no in (select distinct temp.partner_leg from TPT_JOB_DETAIL_CARGO_Tbl temp 
				                                where temp.LEG_TYPE = 21 AND temp.STATUS = 4 
                                                    and temp.JOB_ID = D.JOB_ID
					                                and temp.PARTNER_LEG = D.SEQUENCE_NO)
                                    		
                                    
                                    {0} --filter
			
                                    union all 
                                      
                                SELECT D.UPDATE_VERSION AS UPD_VER,D.FROM_DATE,
                                    convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a 
									                            where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
											                            OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                    * 
                                    FROM TPT_JOB_DETAIL_CARGO_Tbl as D
                                INNER JOIN TPT_JOB_MAIN_Tbl as M ON D.JOB_ID = M.JOB_ID    
                                INNER JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi ON M.JOB_ID=msi.JOB_ID 
                                    where D.LEG_TYPE = 1 and D.IS_MULTI_LEG = 'T'   
                                and D.LEG_GROUP in (select temp1.LEG_GROUP from TPT_JOB_DETAIL_CARGO_Tbl temp1
                                                        where (temp1.PARTNER_JOB_NO = M.JOB_NUMBER or temp1.JOB_ID = d.JOB_ID)
                                                           AND    temp1.LEG_GROUP = D.LEG_GROUP and STATUS = 4
			                                                            )				           
                                and D.LEG_GROUP_MEMBER in (select top 1 temp1.LEG_GROUP_MEMBER from TPT_JOB_DETAIL_CARGO_Tbl temp1
                                                        where (temp1.PARTNER_JOB_NO = M.JOB_NUMBER or temp1.JOB_ID = d.JOB_ID)
                                                           AND temp1.LEG_GROUP = D.LEG_GROUP and STATUS = 1 order by D.JOB_ID, D.LEG_GROUP, D.LEG_GROUP_MEMBER asc
		                                )  
                                    							
                                    {0}  --filter        

                                    ORDER BY D.LEG_TYPE, D.FROM_DATE, D.FROM_TIME, M.JOB_NUMBER, D.SEQUENCE_NO    ";
                    */
                    #endregion
                    #region new query
                    string SQLString = @"SELECT D.UPDATE_VERSION AS UPD_VER,D.FROM_DATE,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
							                                        where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
									                                        OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        * 
                                        FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        INNER JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID 
                                        INNER JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID 
                                        where D.SEQUENCE_NO in (select top 1 temp1.SEQUENCE_NO from TPT_JOB_DETAIL_CARGO_Tbl temp1 with (NOLOCK)
                                                            where (temp1.PARTNER_JOB_NO = M.JOB_NUMBER or temp1.JOB_ID = d.JOB_ID)
                                                                AND temp1.LEG_GROUP = D.LEG_GROUP and STATUS = 1 order by D.JOB_ID, D.LEG_GROUP, D.LEG_GROUP_MEMBER asc
	                                        )  
									
                                        {0} --filter

                                        ORDER BY D.LEG_TYPE, D.FROM_DATE, D.FROM_TIME, M.JOB_NUMBER, D.SEQUENCE_NO ";
                    #endregion
                    SQLString = string.Format(SQLString, sqlWhere);

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                        jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                        jobTrip.LegTypeCustomized = reader["Leg"] == DBNull.Value ? string.Empty : reader["Leg"].ToString();
                        jobTrips.Add(jobTrip);
                    }
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripsSetBookedToReady. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripsSetBookedToReady. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripsSetBookedToReady. " + ex.Message); }
                finally { cn.Close(); }
            }
            return jobTrips;
        }
        //Set Booked to Ready
        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsSetBookedToReady(string jobType, string deptCode, DateTime startDate, DateTime endDate)
        {
            string sqlwhere = @" AND D.STATUS={0} AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}' AND D.OWNTRANSPORT ='T'";

            if (!jobType.Equals("ALL"))
                sqlwhere += @" AND M.JOB_TYPE = '{3}'";

            if (!deptCode.Equals(string.Empty))
                sqlwhere += @" AND M.TPT_DEPT_CODE = '{4}'";

            //20141114 - gerry modified - add last minute of the day for end date to cater 00:00 - 23:59
            sqlwhere = string.Format(sqlwhere, JobTripStatus.Booked.GetHashCode(), DateUtility.ConvertDateForSQLPurpose(startDate),
                                        DateUtility.ConvertDateForSQLPurpose(endDate) + LAST_MINUTE_OF_DAY,
                                         jobType, deptCode);

            //sqlwhere += " ORDER BY D.LEG_TYPE, D.FROM_DATE, D.FROM_TIME, M.JOB_NUMBER";
            //return GetHaulierJobTrips(sqlwhere);
            return GetHaulierJobTripsSetBookedToReady(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByStatus(JobTripStatus status)
        {
            string sqlwhere;
            sqlwhere = @" WHERE D.STATUS={0}";
            sqlwhere = string.Format(sqlwhere, (int)status);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByTransport(bool ownTransport)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.OWNTRANSPORT='{0}'";
            sqlwhere = string.Format(sqlwhere, strOwnTransport);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDeptAndTransport(string deptCode, bool ownTransport)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND M.TPT_DEPT_CODE ='{1}'";
            sqlwhere = string.Format(sqlwhere, strOwnTransport, deptCode);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByStatusAndTransport(JobTripStatus status, bool ownTransport)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.STATUS='{0}' AND D.OWNTRANSPORT='{1}' ";
            sqlwhere = string.Format(sqlwhere, (int)status, strOwnTransport);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDept(string deptCode)
        {
            string sqlwhere;
            sqlwhere = "WHERE M.TPT_DEPT_CODE ='" + CommonUtilities.FormatString(deptCode) + "'";

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDeptAndStatus(JobTripStatus status, string deptCode)
        {
            string sqlwhere;
            sqlwhere = @"WHERE D.STATUS={0} AND M.TPT_DEPT_CODE ='{1}'";
            sqlwhere = string.Format(sqlwhere, (int)status, CommonUtilities.FormatString(deptCode));

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTrips(bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}'";
            sqlwhere = string.Format(sqlwhere, strOwnTransport,
                                        DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo, string deptCode)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.STATUS={0} AND D.OWNTRANSPORT='{1}' AND D.FROM_DATE >='{2}' AND D.TO_DATE <='{3}' AND M.TPT_DEPT_CODE ='{4}'";
            sqlwhere = string.Format(sqlwhere, (int)status, strOwnTransport,
                                        DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY,
                                        deptCode);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, DateTime jobDateFrom, DateTime jobDateTo, string deptCode)
        {
            string sqlwhere;
            sqlwhere = @"WHERE D.STATUS={0} AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}' AND M.TPT_DEPT_CODE ='{3}'";
            sqlwhere = string.Format(sqlwhere, (int)status, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY,
                                        deptCode);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, string deptCode, bool ownTransport)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.STATUS='{0}' AND M.TPT_DEPT_CODE ='{1}' AND D.OWNTRANSPORT='{2}'";
            sqlwhere = string.Format(sqlwhere, (int)status, CommonUtilities.FormatString(deptCode), strOwnTransport);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTrips(bool ownTransport, string jobNoFrom, string jobNoTo)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}'";
            sqlwhere = string.Format(sqlwhere, strOwnTransport, jobNoFrom, jobNoTo);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, bool ownTransport, string jobNoFrom, string jobNoTo, string deptCode)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.STATUS='{0}' AND D.OWNTRANSPORT='{1}' AND M.JOB_NUMBER >='{2}' AND M.JOB_NUMBER <='{3}' AND M.TPT_DEPT_CODE ='{4}'";
            sqlwhere = string.Format(sqlwhere, (int)status, strOwnTransport, jobNoFrom, jobNoTo, CommonUtilities.FormatString(deptCode));

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, string jobNoFrom, string jobNoTo, string deptCode)
        {
            string sqlwhere;
            sqlwhere = @"WHERE D.STATUS='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}' AND M.TPT_DEPT_CODE ='{3}'";
            sqlwhere = string.Format(sqlwhere, (int)status, jobNoFrom, jobNoTo, CommonUtilities.FormatString(deptCode));

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(DateTime jobDateFrom, DateTime jobDateTo)
        {
            string sqlwhere;
            sqlwhere = @"WHERE D.FROM_DATE >='{0}' AND D.TO_DATE <='{1}'";
            sqlwhere = string.Format(sqlwhere, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}'";
            sqlwhere = string.Format(sqlwhere, strOwnTransport, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(bool ownTransport, string deptCode, DateTime jobDateFrom, DateTime jobDateTo)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}' AND M.TPT_DEPT_CODE ='{3}'";
            sqlwhere = string.Format(sqlwhere, strOwnTransport, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY, deptCode);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(JobTripStatus status, bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.STATUS = {0} AND D.OWNTRANSPORT='{1}' AND D.FROM_DATE >='{2}' AND D.TO_DATE <='{3}'";
            sqlwhere = string.Format(sqlwhere, (int)status, strOwnTransport, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(JobTripStatus status, DateTime jobDateFrom, DateTime jobDateTo)
        {
            string sqlwhere;
            sqlwhere = @"WHERE D.STATUS={0} AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}'";
            // 2014-11-19 Zhou Kai add LAST_MINUTE_OF_DAY
            sqlwhere = string.Format(sqlwhere, (int)status, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(string deptCode, DateTime jobDateFrom, DateTime jobDateTo)
        {
            string sqlwhere;
            sqlwhere = @"WHERE M.TPT_DEPT_CODE ='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}'";
            sqlwhere = string.Format(sqlwhere, deptCode, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                        DateUtility.ConvertDateForSQLPurpose(jobDateTo) + LAST_MINUTE_OF_DAY);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByJob(string jobNoFrom, string jobNoTo)
        {
            string sqlwhere;
            sqlwhere = @"WHERE M.JOB_NUMBER >='{0}' AND M.JOB_NUMBER <='{1}'";
            sqlwhere = string.Format(sqlwhere, jobNoFrom, jobNoTo);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByJob(bool ownTransport, string jobNoFrom, string jobNoTo)
        {
            string sqlwhere;
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}'";
            sqlwhere = string.Format(sqlwhere, strOwnTransport, jobNoFrom, jobNoTo);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByJob(bool ownTransport, string jobNoFrom, string jobNoTo, string deptCode)
        {
            string sqlwhere = "";
            string strOwnTransport;

            if (ownTransport) strOwnTransport = "T";
            else strOwnTransport = "F";

            sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}' AND M.TPT_DEPT_CODE ='{3}'";
            sqlwhere = string.Format(sqlwhere, strOwnTransport, jobNoFrom, jobNoTo, deptCode);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByJob(string deptCode, string jobNoFrom, string jobNoTo)
        {
            string sqlwhere;
            sqlwhere = @"WHERE M.TPT_DEPT_CODE ='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}'";
            sqlwhere = string.Format(sqlwhere, deptCode, jobNoFrom, jobNoTo);

            return GetHaulierJobTrips(sqlwhere);
        }
        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsByContainerNo(int jobID, string contNo, bool sorted)
        {
            string sqlwhere;
            sqlwhere = @" WHERE D.JOB_ID={0} ";
            if (!contNo.Equals(string.Empty))
            {
                sqlwhere += " AND D.CONTAINER_NUMBER LIKE '%{1}%'";
            }
            if (sorted)
                sqlwhere += " ORDER BY D.SEQUENCE_NO DESC";
            else
                sqlwhere += " ORDER BY D.CONTAINER_NUMBER, D.SEQUENCE_NO";

            sqlwhere = string.Format(sqlwhere, jobID, contNo);

            return GetHaulierJobTrips(sqlwhere);
        }

        //not in used
        internal static List<HaulierJobTrip> GetHaulierJobTripsSortByContSeqNo(int jobID)
        {
            List<HaulierJobTrip> Operators = new List<HaulierJobTrip>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string SQLString = "SELECT D.UPDATE_VERSION AS UPD_VER,* FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID ";
                SQLString += " LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID ";
                SQLString += " WHERE D.JOB_ID = " + jobID;
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                        jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                        Operators.Add(jobTrip);

                        //Operators.Add(GetHaulierJobTrip(reader));
                    }
                    cn.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripsSortByContSeqNo. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripsSortByContSeqNo. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripsSortByContSeqNo. " + ex.Message); }
                finally { cn.Close(); }
            }
            return Operators;
        }

        #endregion


        internal static SortableList<HaulierJobTrip> GetTwoLegsHaulierJobTrips(int jobID)
        {

            string sqlwhere = "";

            sqlwhere = @"WHERE D.JOB_ID='{0}' AND D.LEG_TYPE <> 1 ";
            sqlwhere = string.Format(sqlwhere, jobID);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static SortableList<HaulierJobTrip> GetOneLegHaulierJobTrips(int jobID)
        {

            string sqlwhere = "";

            sqlwhere = @"WHERE D.JOB_ID='{0}' AND D.LEG_TYPE = 1 ";
            sqlwhere = string.Format(sqlwhere, jobID);

            return GetHaulierJobTrips(sqlwhere);
        }

        internal static List<string> GetAllHaulierJobNo()
        {
            List<string> jobNos = new List<string>();
            jobNos.Add(string.Empty);
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string SQLString = @"select DISTInCT JOB_NUMBER from TPT_JOB_MAIN_Tbl with (NOLOCK)";

                    SqlCommand cmd = new SqlCommand(SQLString, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobNos.Add(reader.GetString(0));
                    }
                    reader.Close();
                    con.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobNo. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobNo. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobNo. " + ex.Message); }
                finally { con.Close(); }
            }
            return jobNos;
        }
        internal static List<string> GetAllHaulierJobSourceRefNo()
        {
            List<string> jobNos = new List<string>();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string SQLString = @"select DISTINCT SOURCE_REF_NUMBER FROM TPT_JOB_MAIN_Tbl with (NOLOCK)";
                    SqlCommand cmd = new SqlCommand(SQLString, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobNos.Add(reader.GetString(0));
                    }
                    reader.Close();
                    con.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobSourceRefNo. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobSourceRefNo. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobSourceRefNo. " + ex.Message); }
                finally { con.Close(); }
            }
            return jobNos;
        }
        internal static List<string> GetAllHaulierCustCode()
        {
            List<string> custCodes = new List<string>();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string SQLString = @"select distinct CustVend_Code from act_custvend_master_tbl with (NOLOCK)
                                    inner join TPT_JOB_MAIN_Tbl with (NOLOCK)
                                    on TPT_JOB_MAIN_Tbl.CUST_CODE = act_custvend_master_tbl.CustVend_Code

                                    order by CustVend_Code";

                    SqlCommand cmd = new SqlCommand(SQLString, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        custCodes.Add(reader.GetString(0));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierCustCode. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierCustCode. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierCustCode. " + ex.Message); }
                finally { con.Close(); }
            }
            return custCodes;
        }
        internal static SortableList<HaulierJob> GetHaulierJobs(int rowCount)
        {
            SortableList<HaulierJob> Operators = new SortableList<HaulierJob>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = @"SELECT {0} * FROM TPT_JOB_MAIN_Tbl as HJM with (NOLOCK) LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl as HJD with (NOLOCK)
                                    ON HJM.JOB_ID = HJD.JOB_ID where 1 = 1";
                    SQLString = string.Format(SQLString, rowCount == 0 ? string.Empty : " TOP " + rowCount.ToString());

                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Operators.Add(GetHaulierJob(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                finally { cn.Close(); }
            }
            return Operators;
        }
        internal static SortableList<HaulierJob> GetHaulierJobs(string jobNoFrom, string jobNoTo, string custCodeFrom, string custCodeTo, string containerNo, string sourceRefNo, int rowCount)
        {
            SortableList<HaulierJob> Operators = new SortableList<HaulierJob>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = @"SELECT {0} * FROM TPT_JOB_MAIN_Tbl as HJM with (NOLOCK) LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl as HJD with (NOLOCK)
                                    ON HJM.JOB_ID = HJD.JOB_ID where 1 = 1";
                    SQLString = string.Format(SQLString, rowCount == 0 ? string.Empty : " TOP " + rowCount.ToString());

                    string filters = "";
                    if (containerNo.Equals(string.Empty))
                    {
                        #region Filter query

                        if (!jobNoFrom.Equals(string.Empty) && !jobNoTo.Equals(string.Empty)
                            && custCodeFrom.Equals(string.Empty) && custCodeTo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.JOB_NUMBER >= '{0}'  AND HJM.JOB_NUMBER <= '{1}' ";
                            filters = string.Format(filters, CommonUtilities.FormatString(jobNoFrom), CommonUtilities.FormatString(jobNoTo));
                        }
                        else if (!jobNoFrom.Equals(string.Empty) && jobNoTo.Equals(string.Empty)
                            && custCodeFrom.Equals(string.Empty) && custCodeTo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.JOB_NUMBER = '{0}'";
                            filters = string.Format(filters, CommonUtilities.FormatString(jobNoFrom));
                        }
                        else if (jobNoFrom.Equals(string.Empty) && !jobNoTo.Equals(string.Empty)
                            && custCodeFrom.Equals(string.Empty) && custCodeTo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.JOB_NUMBER = '{0}'";
                            filters = string.Format(filters, CommonUtilities.FormatString(jobNoTo));
                        }
                        else if (!custCodeFrom.Equals(string.Empty) && !custCodeTo.Equals(string.Empty)
                            && jobNoFrom.Equals(string.Empty) && jobNoTo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.CUST_CODE >= '{0}'  AND HJM.CUST_CODE <= '{1}' ";
                            filters = string.Format(filters, CommonUtilities.FormatString(custCodeFrom), CommonUtilities.FormatString(custCodeTo));
                        }
                        else if (!custCodeFrom.Equals(string.Empty) && custCodeTo.Equals(string.Empty)
                            && jobNoFrom.Equals(string.Empty) && jobNoTo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.CUST_CODE = '{0}' ";
                            filters = string.Format(filters, CommonUtilities.FormatString(custCodeFrom));
                        }
                        else if (custCodeFrom.Equals(string.Empty) && !custCodeTo.Equals(string.Empty)
                            && jobNoFrom.Equals(string.Empty) && jobNoTo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.CUST_CODE = '{0}' ";
                            filters = string.Format(filters, CommonUtilities.FormatString(custCodeTo));
                        }
                        else if (!custCodeFrom.Equals(string.Empty) && !jobNoFrom.Equals(string.Empty)
                            && custCodeTo.Equals(string.Empty) && jobNoTo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.JOB_NUMBER = '{0}' AND HJM.CUST_CODE = '{1}' ";
                            filters = string.Format(filters, CommonUtilities.FormatString(jobNoFrom), CommonUtilities.FormatString(custCodeFrom));
                        }
                        else if (!custCodeTo.Equals(string.Empty) && !jobNoTo.Equals(string.Empty)
                            && custCodeFrom.Equals(string.Empty) && jobNoFrom.Equals(string.Empty))
                        {
                            filters += @" AND HJM.JOB_NUMBER = '{0}' AND HJM.CUST_CODE = '{1}' ";
                            filters = string.Format(filters, CommonUtilities.FormatString(jobNoTo), CommonUtilities.FormatString(custCodeTo));
                        }
                        else if (!jobNoFrom.Equals(string.Empty) && !jobNoTo.Equals(string.Empty)
                                 && !custCodeFrom.Equals(string.Empty) && !custCodeTo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.JOB_NUMBER >= '{0}'
                                    AND HJM.JOB_NUMBER <= '{1}' 
                                    AND HJM.CUST_CODE >= '{2}'
                                    AND HJM.CUST_CODE <= '{3}' ";
                            filters = string.Format(filters, CommonUtilities.FormatString(jobNoFrom), CommonUtilities.FormatString(jobNoTo), CommonUtilities.FormatString(custCodeFrom), CommonUtilities.FormatString(custCodeTo));
                        }
                        else if (!sourceRefNo.Equals(string.Empty))
                        {
                            filters += @" AND HJM.SOURCE_REF_NUMBER  = '{0}'";
                            filters = string.Format(filters, CommonUtilities.FormatString(sourceRefNo));
                        }

                        #endregion
                    }
                    else
                    {
                        string tempQuery = @"select distinct JOB_ID from TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)
                                        where CONTAINER_NUMBER like '%{0}%'";
                        tempQuery = string.Format(tempQuery, CommonUtilities.FormatString(containerNo));

                        List<string> jobIDList = new List<string>();
                        SqlCommand cm = new SqlCommand(tempQuery, cn);
                        cm.CommandTimeout = 0;
                        if (cn.State == ConnectionState.Closed) { cn.Open(); }
                        IDataReader rdr = cm.ExecuteReader();
                        while (rdr.Read())
                        {
                            jobIDList.Add(Convert.ToString(rdr[0]));
                        }
                        rdr.Close();
                        string[] temp = new string[jobIDList.Count];
                        temp = jobIDList.ToArray();
                        string temp00 = string.Join("','", temp);

                        filters = " AND HJM.JOB_ID in ('" + temp00 + "')";
                    }

                    if (!filters.Equals(string.Empty))
                    {
                        SQLString += filters;
                    }
                    SQLString += @" ORDER by HJM.JOB_ID desc";

                    //SQLString = string.Format(SQLString, jobNoFrom, jobNoTo, custCodeFrom, custCodeTo);

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Operators.Add(GetHaulierJob(reader));
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                finally { cn.Close(); }
            }
            return Operators;

        }
        internal static SortableList<HaulierJob> GetHaulierJobs(string jobNo, string custCode, string custRefNo, string bookRefNo, string sourceRefNo, int rowCount, string containerNo, string shipperCode, string consigneeCode)
        {
            SortableList<HaulierJob> jobs = new SortableList<HaulierJob>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = @"SELECT {0} * FROM TPT_JOB_MAIN_Tbl as HJM with (NOLOCK) LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl as HJD with (NOLOCK)
                                    ON HJM.JOB_ID = HJD.JOB_ID where 1 = 1";
                    SQLString = string.Format(SQLString, rowCount == 0 ? string.Empty : " TOP " + rowCount.ToString());

                    string filters = "";
                    #region filter by Job number
                    if (!jobNo.Equals(string.Empty))
                        filters += @" AND JOB_NUMBER like '%" + CommonUtilities.FormatString(jobNo) + "%'";
                    #endregion
                    #region filter by Customer Code
                    if (!custCode.Equals(string.Empty))
                        filters += @" AND CUST_CODE like '%" + CommonUtilities.FormatString(custCode) + "%'";
                    #endregion
                    #region filter by Customer Reference Number
                    if (!custRefNo.Equals(string.Empty))
                        filters += @" AND CUSTOMER_REFERENCE like '%" + CommonUtilities.FormatString(custRefNo) + "%'";
                    #endregion
                    #region filter by Carrier Book Reference Number
                    if (!bookRefNo.Equals(string.Empty))
                        filters += @" AND BOOKING_REFERENCE like '%" + CommonUtilities.FormatString(bookRefNo) + "%'";
                    #endregion
                    #region filter by Shipper code
                    if (!shipperCode.Equals(string.Empty))
                        filters += @" AND SHIPPER_CODE like '%" + CommonUtilities.FormatString(shipperCode) + "%'";
                    #endregion
                    #region filter by Consignee code
                    if (!consigneeCode.Equals(string.Empty))
                        filters += @" AND CONSIGNEE_CODE like '%" + CommonUtilities.FormatString(consigneeCode) + "%'";
                    #endregion

                    #region filter by Sea Job Number
                    if (!sourceRefNo.Equals(string.Empty))
                        filters += @" AND SOURCE_REF_NUMBER like '%" + CommonUtilities.FormatString(sourceRefNo) + "%'";
                    #endregion
                    #region filter by Container
                    if (!containerNo.Equals(string.Empty))
                    {
                        string tempQuery = @"select distinct JOB_ID from TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)
                                        where CONTAINER_NUMBER like '%{0}%'";
                        tempQuery = string.Format(tempQuery, CommonUtilities.FormatString(containerNo));

                        List<string> jobIDList = new List<string>();
                        SqlCommand cm = new SqlCommand(tempQuery, cn);
                        if (cn.State == ConnectionState.Closed) { cn.Open(); }
                        IDataReader rdr = cm.ExecuteReader();
                        while (rdr.Read())
                        {
                            jobIDList.Add(Convert.ToString(rdr[0]));
                        }
                        rdr.Close();
                        string[] temp = new string[jobIDList.Count];
                        temp = jobIDList.ToArray();
                        string temp00 = string.Join("','", temp);

                        filters += " AND HJM.JOB_ID in ('" + temp00 + "')";
                    }
                    #endregion

                    if (!filters.Equals(string.Empty))
                    {
                        SQLString += filters;
                    }
                    SQLString += @" ORDER by HJM.JOB_ID desc";

                    //SQLString = string.Format(SQLString, jobNoFrom, jobNoTo, custCodeFrom, custCodeTo);

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        jobs.Add(GetHaulierJob(reader));
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobs. " + ex.Message); }
                finally { cn.Close(); }
            }
            return jobs;
        }

        //March 26, 2012 - Gerry Added to Update Port remark and YARD 
        internal static bool UpdatePortAndYard(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                string SQLUpdateString = @" Update TPT_JOB_DETAIL_CARGO_Tbl 
                                                    set PORTREMARKS ='{0}',
                                                        YARD ='{1}'
                                                    Where JOB_ID = '{2}'
                                                    AND SEQUENCE_NO = '{3}'";

                SQLUpdateString = string.Format(SQLUpdateString, CommonUtilities.FormatString(haulierJobTrip.PortRemark),
                                                                 CommonUtilities.FormatString(haulierJobTrip.Yard),
                                                                 haulierJobTrip.JobID,
                                                                 haulierJobTrip.Sequence);

                SqlCommand cmd = new SqlCommand(SQLUpdateString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                retValue = true;
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdatePortAndYard. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdatePortAndYard. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdatePortAndYard. " + ex.Message); }

            return retValue;
        }
        //20140218 - gerry added
        //20140920 - gerry added sql parameter, to be in a single connection with the calling method
        internal static ArrayList GetAllSealNoForJob(int jobId, SqlConnection con, SqlTransaction tran)
        {
            ArrayList list = new ArrayList();
            //20140922 - gerry added
            if (con == null)
                con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            ////20140922 - end
            string SQLString = @"SELECT DISTINCT D.SEAL_NUMBER FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK) 
                                 LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID   WHERE 1=1 and D.SEAL_NUMBER <>''
                                    AND M.JOB_ID =" + jobId;
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                //20140922 - gerry added
                if (tran != null) { cmd.Transaction = tran; }
                ////20140922 - end
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader.GetString(0).ToString().Trim());
                }
                reader.Close();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllSealNoForJob. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllSealNoForJob. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllSealNoForJob. " + ex.Message); }
            return list;
        }
        //20140920 - gerry added sql parameter, to be in a single connection with the calling method
        internal static ArrayList GetAllContainerNoForJob(int jobId, SqlConnection con, SqlTransaction tran)
        {
            ArrayList list = new ArrayList();
            string SQLString = @"SELECT DISTINCT D.CONTAINER_NUMBER FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK) 
                                 LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID WHERE 1=1 and D.CONTAINER_NUMBER <>''
                                    AND M.JOB_ID =" + jobId;
            try
            {
                //20140922 - gerry added
                if (con == null)
                    con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                ////20140922 - end
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                //20140922 - gerry added
                if (tran != null) { cmd.Transaction = tran; }
                ////20140922 - end
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader.GetString(0).ToString().Trim());
                }
                reader.Close();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerNoForJob. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerNoForJob. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerNoForJob. " + ex.Message); }
            return list;
        }
        internal static ArrayList GetAllContainerNoForJob(int jobId)
        {
            ArrayList list = new ArrayList();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string SQLString = @"SELECT DISTINCT D.CONTAINER_NUMBER FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK) 
                                 LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID WHERE 1=1 and D.CONTAINER_NUMBER <>''
                                    AND M.JOB_ID =" + jobId;
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, con);
                    cmd.CommandTimeout = 0;
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0).ToString().Trim());
                    }
                    reader.Close();
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerNoForJob. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerNoForJob. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerNoForJob. " + ex.Message); }
                finally { con.Close(); }
            }
            return list;
        }
        //20140731 - gerry added to get container codes
        internal static ArrayList GetAllContainerCodesForJob(int jobId)
        {
            ArrayList list = new ArrayList();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string SQLString = @"SELECT DISTINCT D.CONTAINER_CODE FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK) 
                                 LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID WHERE 1=1 and D.CONTAINER_CODE <>''
                                    AND M.JOB_ID =" + jobId;
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0).ToString().Trim());
                    }
                    cn.Close();
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerCodesForJob. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerCodesForJob. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllContainerCodesForJob. " + ex.Message); }
                finally { cn.Close(); }
            }
            return list;
        }
        //20150824 - gerry added to check if containerNo still being used by other job
        internal static bool IsContainerNoFreeToUse(string containerNo, int jobID, int legGroup)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string query = @"SELECT TOP 1 D.CONTAINER_NUMBER, M.JOB_NUMBER, D.SEQUENCE_NO FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK) 
                                 LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID 
                                    WHERE 1=1 AND D.CONTAINER_NUMBER = '{0}' 
                                            AND ((D.JOB_ID <> {1} AND D.STATUS <> 4) OR (D.JOB_ID = {1} AND D.LEG_GROUP <> {2}))";

                    query = string.Format(query, CommonUtilities.FormatString(containerNo), jobID, legGroup);

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        throw new FMException("Container number " + reader["CONTAINER_NUMBER"].ToString() + " is still being used by jobNo " + reader["JOB_NUMBER"].ToString() + " seqNo " + reader["SEQUENCE_NO"].ToString() + " and not yet completed. ");
                    }
                }
                catch (FMException fme) { throw new FMException("HaulierJobDAL Error : IsContainerNoFreeToUse. " + fme.Message); }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : IsContainerNoFreeToUse. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : IsContainerNoFreeToUse. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : IsContainerNoFreeToUse. " + ex.Message); }
                finally { con.Close(); }
            }
            return true;
        }
        internal static bool IsContainerNoFreeToUse(string containerNo, int jobID, int legGroup, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = true;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"SELECT TOP 1 D.CONTAINER_NUMBER, M.JOB_NUMBER, D.SEQUENCE_NO FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK) 
                                 LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID 
                                    WHERE 1=1 AND D.CONTAINER_NUMBER = '{0}' 
                                            AND ((D.JOB_ID <> {1} AND D.STATUS <> 4) 
		                                            OR (D.JOB_ID = {1} AND D.LEG_GROUP <> {2}))";

                query = string.Format(query, CommonUtilities.FormatString(containerNo), jobID, legGroup);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                string errMsg = @"Container number {0} is still being used by jobNo {1} seqNo {2} and not yet completed. ";
                while (reader.Read()) 
                {
                    errMsg = string.Format(errMsg, containerNo, reader["JOB_NUMBER"].ToString(), reader["SEQUENCE_NO"].ToString());
                    retValue = false; 
                }
                reader.Close();
                if (!retValue) { throw new FMException(errMsg); }
            }
            catch (FMException fme) { throw new FMException("HaulierJobDAL Error : IsContainerNoFreeToUse. " + fme.Message); }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : IsContainerNoFreeToUse. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : IsContainerNoFreeToUse. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : IsContainerNoFreeToUse. " + ex.Message); }
            return true;
        }
        //20150824 - gerry added to check if sealNo still being used by other job
        internal static bool IsSealNoFreeToUse(string sealNo, int jobID, int LegGroup)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string query = @"SELECT TOP 1 D.SEAL_NUMBER, M.JOB_NUMBER, D.SEQUENCE_NO FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK) 
                                 LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID 
                                    WHERE 1=1 AND D.SEAL_NUMBER = '{0}'
                                            AND ((D.JOB_ID <> {1} AND D.STATUS <> 4) 
		                                            OR (D.JOB_ID = {1}  AND D.LEG_GROUP <> {2}))";

                    query = string.Format(query, CommonUtilities.FormatString(sealNo), jobID, LegGroup);

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        throw new FMException(reader["SEAL_NUMBER"].ToString() + " is still being used by job number " + reader["JOB_NUMBER"].ToString() + " seqNo " + reader["SEQUENCE_NO"].ToString() + " and not yet completed. ");
                    }
                }
                catch (FMException fme) { throw new FMException("HaulierJobDAL Error : IsSealNoFreeToUse. " + fme.Message); }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : IsSealNoFreeToUse. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : IsSealNoFreeToUse. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : IsSealNoFreeToUse. " + ex.Message); }
                finally { con.Close(); }
            }
            return true;
        }
        internal static bool IsSealNoFreeToUse(string sealNo, int jobID, int LegGroup, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = true;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"SELECT TOP 1 D.SEAL_NUMBER, M.JOB_NUMBER, D.SEQUENCE_NO FROM TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK) 
                                 LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID 
                                    WHERE 1=1 AND D.SEAL_NUMBER = '{0}'
                                            AND ((D.JOB_ID <> {1} AND D.STATUS <> 4) 
		                                            OR (D.JOB_ID = {1}  AND D.LEG_GROUP <> {2}))";

                query = string.Format(query, CommonUtilities.FormatString(sealNo), jobID, LegGroup);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                string errMsg = @"Seal number {0} is still being used by jobNo {1} seqNo {2} and not yet completed. ";
                while (reader.Read())
                {
                    errMsg = string.Format(errMsg, sealNo, reader["JOB_NUMBER"].ToString(), reader["SEQUENCE_NO"].ToString());
                    retValue = false;
                }
                reader.Close();
                if (!retValue) { throw new FMException(errMsg); }
            }
            catch (FMException fme) { throw new FMException("HaulierJobDAL Error : IsSealNoFreeToUse. " + fme.Message); }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : IsSealNoFreeToUse. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : IsSealNoFreeToUse. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : IsSealNoFreeToUse. " + ex.Message); }

            return retValue;
        }
      
        //20120813 WYN
        //20140805 - gerry replaced parameter to haulier job because we need to check the update version of the haulier job header
        internal static bool UpdateHaulierJobFromPlanning(HaulierJob haulierJob, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //20140805 - gerry added to check update version of the job header
                byte[] originalRowVersion = GetHeaderUpdateVersion(haulierJob.JobID, cn, tran);
                if (SqlBinary.Equals(haulierJob.UpdateVersion, originalRowVersion) == false)
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nHaulierJobDAL.EditHaulierJobHeader");
                }
                string query = @"update TPT_JOB_MAIN_Tbl
                                    set PERMIT_NO1 ='{0}'
                                    where JOB_ID = {1}";
                query = string.Format(query, CommonUtilities.FormatString(haulierJob.permitNo1), haulierJob.JobID);
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                retValue = true;
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateHaulierJobFromPlanning. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateHaulierJobFromPlanning. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateHaulierJobFromPlanning. " + ex.Message); }
            return retValue;
        }

        internal static bool SetHaulierJobStatus(int jobId, JobStatus status, string completedRemark, string userID, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string query = @"update TPT_JOB_MAIN_Tbl
                                    set JOB_STATUS ='{1}'
                                        , Remarks5 = '{2}'
                                        , Modified_By = '{3}'
                                        , Modified_DateTime = '{4}'
                                    where JOB_ID = {0}";
                query = string.Format(query, jobId, (int)status.GetHashCode(), CommonUtilities.FormatString(completedRemark)
                                            ,userID, DateUtility.ConvertDateAndTimeForSQLPurpose(DateTime.Now));
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobStatus. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobStatus. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : SetHaulierJobStatus. " + ex.Message); }

            return true;
        }

        internal static bool IsHaulierJobTripHasPendingPreviousLeg(HaulierJobTrip jobTrip)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                //20141126 -gerry modified query - replace to legroup instead of containerNo 
                string SQLString = @"select * from TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)
                                        where JOB_ID = {0}
	                                        and	LEG_TYPE = 1
                                            and status <> 4
                                            and IS_MULTI_LEG = 'T'
	                                        and sequence_no < {1}
	                                        and LEG_GROUP = {2} /*container_number  = '{2}' --20141126 -gerry replace to legroup instead of containerNo */
	                                        ";

                SQLString = string.Format(SQLString, jobTrip.JobID, jobTrip.Sequence, /*jobTrip.ContainerNo*/jobTrip.LegGroup);
                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, con);
                DataTable dsSearchResult = new DataTable();
                daSearchCmd.Fill(dsSearchResult);
                con.Close();
                if (dsSearchResult.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : IsHaulierJobTripHasPendingPreviousLeg. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : IsHaulierJobTripHasPendingPreviousLeg. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : IsHaulierJobTripHasPendingPreviousLeg. " + ex.Message); }
            finally { con.Close(); }
        }

        // 2013-12-02 Zhou Kai adds function to get and sort the status history by job trip
        public static List<JobTripState> GetHaulierJobTripStatusHistory(int jobId, int jobTripSeqNo)
        {
            List<JobTripState> lstHaulierJobTripStatusHistory = new List<JobTripState>();
            using (SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string strQuery = "SELECT * FROM TPT_JOB_TRIP_STATE_Tbl with (NOLOCK) WHERE JOB_ID = @jobId " +
                    "AND SEQUENCE_NO = @jobtrip_seqNo ORDER BY JOB_STATE_NO";
                try
                {
                    if (dbCon.State != ConnectionState.Open) { dbCon.Open(); }
                    SqlCommand dbCmd = new SqlCommand(strQuery, dbCon);
                    dbCmd.CommandTimeout = 0;
                    dbCmd.Parameters.AddWithValue("jobId", jobId);
                    dbCmd.Parameters.AddWithValue("jobtrip_seqNo", jobTripSeqNo);
                    SqlDataReader sr = dbCmd.ExecuteReader();
                    while (sr.Read())
                    {
                        JobTripState tjtsr = new JobTripState();
                        // tjtsr.jobId = jobId;
                        // tjtsr.jobTripSeqNo = jobTripSeqNo;
                        tjtsr.Status = (JobTripStatus)(int)sr["STATUS"];
                        tjtsr.Seq_No = (int)sr["JOB_STATE_NO"];
                        tjtsr.StatusDate = (DateTime)sr["STATUSDATE"];
                        tjtsr.Remarks = (string)sr["REMARKS"];

                        lstHaulierJobTripStatusHistory.Add(tjtsr);
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripStatusHistory. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripStatusHistory. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobTripStatusHistory. " + ex.Message); }
                finally { dbCon.Close(); }
            }
            return lstHaulierJobTripStatusHistory;
        }

        // 2013-12-02 Zhou Kai ends

        // 2013-12-24 Zhou Kai adds
        public static List<string> GetAllHaulierJobChargeSeqNos(string jobNumber)
        {
            List<string> lstHaulierJobChargeSeqNo = new List<string>();
            string sqlQuery = "SELECT SEQUENCE_NO FROM dbo.TPT_JOB_DETAIL_CHARGE_Tbl with (NOLOCK) " +
                    "INNER JOIN dbo.TPT_JOB_MAIN_Tbl with (NOLOCK) ON dbo.TPT_JOB_DETAIL_CHARGE_Tbl.JOB_ID = " +
                    "dbo.TPT_JOB_MAIN_Tbl.JOB_ID WHERE JOB_NUMBER = '{0}'";
            sqlQuery = String.Format(sqlQuery, jobNumber);
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lstHaulierJobChargeSeqNo.Add(reader["SEQUENCE_NO"].ToString());
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobChargeSeqNos. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobChargeSeqNos. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobChargeSeqNos. " + ex.Message); }
                finally { con.Close(); }
            }
            return lstHaulierJobChargeSeqNo;
        }
        // 2013-12-24 Zhou Kai ends        

        //20140124 - Gerry added to get the previous job trip
        internal static HaulierJobTrip GetPreviousLegJobTrip(int jobID, string containerNo)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        *
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID  
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where M.JOB_ID = {0} AND D.CONTAINER_NUMBER = '{1}'
                                        order by D.SEQUENCE_NO desc";
                    sqlQuery = string.Format(sqlQuery, jobID, containerNo);
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                        jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                        retValue = jobTrip;
                        //retValue = GetHaulierJobTrip(reader);
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                finally { con.Close(); }
            }
            return retValue;
        }
        //20141124 - Gerry added to get the previous job trip including the transfer out job
        internal static HaulierJobTrip GetPreviousLegJobTrip(int jobID, string containerNo, int legGroupMember)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        *
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID  
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where M.JOB_ID = {0} AND D.CONTAINER_NUMBER = '{1}' and leg_group_member < {2}
                                        order by D.JOB_ID, D.Leg_Group, D.leg_group_member desc";
                    sqlQuery = string.Format(sqlQuery, jobID, containerNo, legGroupMember);
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                        jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                        retValue = jobTrip;

                        //retValue = GetHaulierJobTrip(reader);
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                finally { con.Close(); }
            }
            return retValue;
        }
        //20150909 - Gerry overload
        internal static HaulierJobTrip GetPreviousLegJobTrip(int jobID, int legGroupMember, int legGroup, SqlConnection con, SqlTransaction tran)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            try
            {
                string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        *
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID  
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where M.JOB_ID = {0}   
                                        AND D.LEG_GROUP_MEMBER < {1}
                                        AND D.LEG_GROUP = {2}
                                        order by D.SEQUENCE_NO desc";
                sqlQuery = string.Format(sqlQuery, jobID, legGroupMember, legGroup);
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                    retValue = jobTrip;

                    //retValue = GetHaulierJobTrip(reader);
                }
                reader.Close();
                retValue.JobTripStates = GetAllHaulierJobTripStates(retValue, con, tran);
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }

            return retValue;
        }
        //20151201 - gerry modified to fix error - transfer In job Can't set to complete
        //parameter seqNo was replaced by leg group member
        //query also modified to cater to hubbing jobs
        internal static HaulierJobTrip GetPreviousLegJobTrip(int jobID, int legGroupMember, int legGroup)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        *
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID  
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where M.JOB_ID = {0}   
                                        AND D.LEG_GROUP_MEMBER < {1}
                                        AND D.LEG_GROUP = {2}
                                        order by D.SEQUENCE_NO desc";
                    sqlQuery = string.Format(sqlQuery, jobID, legGroupMember, legGroup);
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                        jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                        retValue = jobTrip;
                        //retValue = GetHaulierJobTrip(reader);
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                finally { con.Close(); }
            }
            return retValue;
        }
        //20151201 - gerry added to cater the previous leg of the hubbing job
        internal static HaulierJobTrip GetPreviousLegJobTrip(HaulierJobTrip jobTrip)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER, 
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        *
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID  
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where (M.JOB_ID in (select JOB_ID from TPT_JOB_MAIN_Tbl with (NOLOCK) where JOB_NUMBER in ('{0}','{1}')))
	                                        AND D.LEG_GROUP_MEMBER < {2} 
	                                        AND D.LEG_GROUP = {3}
                                        order by D.LEG_GROUP,D.LEG_GROUP_MEMBER   desc";

                    sqlQuery = string.Format(sqlQuery, jobTrip.JobNo, jobTrip.PartnerJobNo, jobTrip.LegGroupMember, jobTrip.LegGroup);
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HaulierJobTrip tempTrip = GetHaulierJobTrip(reader);
                        tempTrip.JobTripStates = GetAllHaulierJobTripStates(tempTrip);
                        retValue = tempTrip;
                        //retValue = GetHaulierJobTrip(reader);
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
                finally { con.Close(); }
            }
            return retValue;
        }
        internal static HaulierJobTrip GetPreviousLegJobTrip(HaulierJobTrip jobTrip, SqlConnection con, SqlTransaction tran)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            try
            {
                string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        * 
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID  
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where (M.JOB_ID in (select JOB_ID from TPT_JOB_MAIN_Tbl with (NOLOCK) where JOB_NUMBER in ('{0}','{1}')))
	                                        AND D.LEG_GROUP_MEMBER < {2} 
	                                        AND D.LEG_GROUP = {3}
                                        order by D.LEG_GROUP,D.LEG_GROUP_MEMBER   desc";

                sqlQuery = string.Format(sqlQuery, jobTrip.JobNo, jobTrip.PartnerJobNo, jobTrip.LegGroupMember, jobTrip.LegGroup);
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip tempTrip = GetHaulierJobTrip(reader);
                    retValue = tempTrip;
                    //retValue = GetHaulierJobTrip(reader);
                }
                reader.Close();
                retValue.JobTripStates = GetAllHaulierJobTripStates(retValue, con, tran);
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPreviousLegJobTrip. " + ex.Message); }

            return retValue;
        }


        #region   Sub contractor changed start

        //20140516- Gerry overload
        internal static HaulierJobTrip GetPreviousLegJobTrip(int jobID, int seqNo, string containerNo)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        * from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID  
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where M.JOB_ID = {0}   
                                        AND D.SEQUENCE_NO < {1}
                                        AND D.CONTAINER_NUMBER = '{2}'
                                        order by D.SEQUENCE_NO desc";
                sqlQuery = string.Format(sqlQuery, jobID, seqNo, containerNo);
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                    jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                    retValue = jobTrip;

                    //retValue = GetHaulierJobTrip(reader);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { con.Close(); }

            return retValue;
        }
        //20140520 - gerry added to get jobNos for subcontractor jobs
        internal static ArrayList GetSubContractedHaulierTripJobNo(int status)
        {
            ArrayList list = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT DISTINCT JOB_NUMBER FROM TPT_JOB_MAIN_Tbl with (NOLOCK)
                                        INNER JOIN TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)
                                        ON TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID=TPT_JOB_MAIN_Tbl.JOB_ID
                                        WHERE 1=1   
                                        AND TPT_JOB_DETAIL_CARGO_Tbl.STATUS ={0}
                                        AND TPT_JOB_DETAIL_CARGO_Tbl.OWNTRANSPORT ='F'";
                SQLString = string.Format(SQLString, status);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((string)reader["JOB_NUMBER"]);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return list;
        }
        //20140520 - gerry added to get seqNos for subcontractor jobs
        internal static ArrayList GetSubContractedHaulierTripSeqNo(int jobId, int status)
        {
            ArrayList list = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT DISTINCT SEQUENCE_NO FROM TPT_JOB_MAIN_Tbl with (NOLOCK)
                                        INNER JOIN TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)
                                        ON TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID=TPT_JOB_MAIN_Tbl.JOB_ID
                                        WHERE 1=1      
                                        AND TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID = {0}
                                        AND TPT_JOB_DETAIL_CARGO_Tbl.STATUS ={1}
                                        AND TPT_JOB_DETAIL_CARGO_Tbl.OWNTRANSPORT ='F'";
                SQLString = string.Format(SQLString, jobId, status);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((Int32)reader["SEQUENCE_NO"]);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return list;
        }
        //20140521 - gerry add for subcon
        internal static bool AddorChangeSubContractorFromPlanning(HaulierJobTrip haulierJobTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                byte[] origUpdateVersion = GetHaulierJobTripUpdateVersion(haulierJobTrip, con, tran);
                if (!SqlBinary.Equals(haulierJobTrip.UpdateVersion, origUpdateVersion))
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict);
                }
                string query = @"UPDATE TPT_JOB_DETAIL_CARGO_Tbl
                                set SUBCONTRACTOR_CODE = '{0}',
                                    SUBCONTRACTOR_NAME ='{1}',
                                    SUBCONTRACTOR_ADD1 ='{2}',
                                    SUBCONTRACTOR_ADD2 ='{3}',
                                    SUBCONTRACTOR_ADD3 ='{4}',
                                    SUBCONTRACTOR_ADD4 ='{5}',
                                    SUBCONTRACTOR_CITY_CODE ='{6}',
                                    Status =  {7}, /*set to not only Assigned but can be set to any status*/
                                    OWNTRANSPORT = '{8}' /* set own transport value */

                                where JOB_ID = {9} and SEQUENCE_NO ={10}
                                ";

                query = string.Format(query, CommonUtilities.FormatString(haulierJobTrip.subCon.Code.ToString().Trim()),
                                             CommonUtilities.FormatString(haulierJobTrip.subCon.Name.ToString().Trim()),
                                             CommonUtilities.FormatString(haulierJobTrip.subCon.Add1.ToString().Trim()),
                                             CommonUtilities.FormatString(haulierJobTrip.subCon.Add2.ToString().Trim()),
                                             CommonUtilities.FormatString(haulierJobTrip.subCon.Add3.ToString().Trim()),
                                             CommonUtilities.FormatString(haulierJobTrip.subCon.Add4.ToString().Trim()),
                                             CommonUtilities.FormatString(haulierJobTrip.subCon.City.ToString().Trim()),
                                             haulierJobTrip.TripStatus.GetHashCode(),
                                             haulierJobTrip.subCon.Code.Trim() == string.Empty ? "T" : "F",
                                             haulierJobTrip.JobID,
                                             haulierJobTrip.Sequence);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
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
        #endregion


        #region "2014-03-14 Zhou Kai adds the code bolck for haulage logging related functions"
        /*
         * Get all haulage job numbers those are already been deleted
         */
        public static List<string> GetAllDeletedHaulageJobNumbers(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = "SELECT PARENT_IDENTIFIER AS JOB_NO FROM " +
                                       "TPT_LOG_HEADER_Tbl WHERE with (NOLOCK) " +
                                       "MODULE = 5 AND CHILD_IDENTIFIER = '' AND FORM_ACTION = 3 " +
                                       LoggerDAL.FormatFormNamesForSql(dict);
            SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand dbCmd = new SqlCommand(sqlQuery, dbCon);
            dbCmd.CommandTimeout = 0;
            try
            {
                if (dbCon.State != ConnectionState.Open) { dbCon.Open(); }
                SqlDataReader sr = dbCmd.ExecuteReader();
                while (sr.Read())
                {
                    tmpList.Add(sr["JOB_NO"].ToString());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedHaulageJobNumbers. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedHaulageJobNumbers. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedHaulageJobNumbers. " + ex.Message); }
            finally { dbCon.Close(); }

            return tmpList;
        }

        /*
         * Get all job numbers that have deleted job trips.
         */
        public static List<string> GetAllJobNoWhichHasDeletedJobTrips(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = "SELECT DISTINCT PARENT_IDENTIFIER FROM TPT_LOG_HEADER_Tbl with (NOLOCK) " +
                                       "WHERE FORM_ACTION = 3 AND " +
                                       "CHILD_IDENTIFIER <> '' AND CHARINDEX('&', PARENT_IDENTIFIER) = 0 " +
                                        "AND MODULE = 5 " + LogHeader.FormatFormNames(dict) +
                                        " AND PARENT_IDENTIFIER IN (SELECT JOB_NUMBER FROM TPT_JOB_MAIN_Tbl with (NOLOCK)) " +
                                        "AND CHILD_IDENTIFIER NOT IN (SELECT DISTINCT [SEQUENCE_NO] FROM " +
                                        " TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK) INNER JOIN TPT_JOB_MAIN_Tbl with (NOLOCK) ON " +
                                        "TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID = TPT_JOB_MAIN_Tbl.JOB_ID" +
                                        " AND TPT_JOB_MAIN_Tbl.JOB_NUMBER = PARENT_IDENTIFIER);";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                cmd.CommandTimeout = 0;
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    tmpList.Add(sr["PARENT_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllJobNoWhichHasDeletedJobTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllJobNoWhichHasDeletedJobTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllJobNoWhichHasDeletedJobTrips. " + ex.Message); }
            finally { cn.Close(); }

            return tmpList;
        }

        public static List<string> GetAllDeletedJobTripSeqNos(Dictionary<string, string> dict)
        {
            string jobNo = dict[LogHeader.TOP_LEVEL_NO];
            List<string> tmpList = new List<string>();
            List<string> tmpSeqList = new List<string>();
            string tripSeqNos = String.Empty;
            tmpSeqList = GetAllHaulierJobTripSeqNos(jobNo);
            foreach (string seqNo in tmpSeqList)
            {
                tripSeqNos = tripSeqNos + ",'" + seqNo + "'";
            }
            if (tripSeqNos.Equals(String.Empty)) // no deleted job trips 
            {
                tripSeqNos = "('')";
            }
            else
            {
                tripSeqNos = "(" + tripSeqNos.Substring(1, tripSeqNos.Length - 1) + ")";
            }
            string sqlQuery = "SELECT DISTINCT CHILD_IDENTIFIER, FORM_NAME FROM TPT_LOG_HEADER_Tbl  with (NOLOCK) " +
                                        "WHERE PARENT_IDENTIFIER = '{0}' AND MODULE = 5 " +
                                        "AND CHILD_IDENTIFIER <> '' AND FORM_ACTION = 3 AND " +
                                        "CHILD_IDENTIFIER NOT IN " + tripSeqNos + LogHeader.FormatFormNames(dict) +
                                        "AND PARENT_IDENTIFIER IN (SELECT JOB_NUMBER FROM TPT_JOB_MAIN_Tbl with (NOLOCK))";
            sqlQuery = String.Format(sqlQuery, dict[LogHeader.TOP_LEVEL_NO]);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                cmd.CommandTimeout = 0;
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tmpList.Add(r["CHILD_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedJobTripSeqNos. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedJobTripSeqNos. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedJobTripSeqNos. " + ex.Message); }
            finally { cn.Close(); }

            return tmpList;
        }

        public static List<string> GetAllDeletedJobChargeSeqNos(Dictionary<string, string> dict)
        {
            string jobNo = dict[LogHeader.TOP_LEVEL_NO];
            List<string> tmpList = new List<string>();
            List<string> tmpSeqList = new List<string>();
            string chargeSeqNos = String.Empty;
            tmpSeqList = GetAllHaulierJobChargeSeqNos(jobNo);
            foreach (string seqNo in tmpSeqList)
            {
                chargeSeqNos = chargeSeqNos + ",'" + seqNo + "'";
            }
            // 2014-02-10 Zhou Kai adds
            if (chargeSeqNos.Equals(String.Empty)) // no deleted job trips 
            {
                chargeSeqNos = "('')";
            }
            else
            {
                chargeSeqNos = "(" + chargeSeqNos.Substring(1, chargeSeqNos.Length - 1) + ")";
            }
            // 2014-02-10 Zhou Kai ends
            string sqlQuery = "SELECT CHILD_IDENTIFIER, FORM_NAME FROM TPT_LOG_HEADER_Tbl with (NOLOCK) " +
                                        "WHERE PARENT_IDENTIFIER = '{0}' AND MODULE = 5 " +
                                        "AND CHILD_IDENTIFIER <> '' AND FORM_ACTION = 3 AND " +
                                        "CHILD_IDENTIFIER NOT IN " + chargeSeqNos + LogHeader.FormatFormNames(dict);
            sqlQuery = String.Format(sqlQuery, dict[LogHeader.TOP_LEVEL_NO]);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                cmd.CommandTimeout = 0;
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tmpList.Add(r["CHILD_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedJobChargeSeqNos. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedJobChargeSeqNos. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllDeletedJobChargeSeqNos. " + ex.Message); }
            finally { cn.Close(); }

            return tmpList;
        }

        internal static List<string> GetAllHaulierJobTripSeqNos(string jobNumber)
        {
            try
            {
                List<string> tmpList = new List<string>();
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT TPT_JOB_DETAIL_CARGO_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK)";
                    SQLString += " INNER JOIN TPT_JOB_MAIN_Tbl with (NOLOCK) ON TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID=TPT_JOB_MAIN_Tbl.JOB_ID  AND " +
                        "Job_Number = '" + jobNumber + "'";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //tmpList.Add(GetHaulierJobTrip(reader).Sequence.ToString()); 
                        // 2014-09-12 Zhou Kai modifies the one line below:
                        // tmpList.Add((string)reader["SEQUENCE_NO"]);
                        tmpList.Add(reader["SEQUENCE_NO"].ToString());
                    }
                    cn.Close();
                }
                return tmpList;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripSeqNos. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripSeqNos. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllHaulierJobTripSeqNos. " + ex.Message); }
        }

        public static List<string> GetAllJobNoWhichHasDeletedJobCharges(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = "SELECT DISTINCT PARENT_IDENTIFIER FROM TPT_LOG_HEADER_Tbl with (NOLOCK) " +
                                       "WHERE FORM_ACTION = 3 AND " +
                                       "CHILD_IDENTIFIER <> '' AND CHARINDEX('&', PARENT_IDENTIFIER) = 0 " +
                                        "AND MODULE = 5 " + LogHeader.FormatFormNames(dict) +
                                        " AND PARENT_IDENTIFIER IN (SELECT JOB_NUMBER FROM TPT_JOB_MAIN_Tbl with (NOLOCK)) " +
                                        "AND CHILD_IDENTIFIER NOT IN (SELECT SEQUENCE_NO FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK) " +
                                        "INNER JOIN TPT_JOB_MAIN_Tbl with (NOLOCK) ON  TPT_JOB_MAIN_Tbl.JOB_ID = TPT_JOB_DETAIL_CARGO_Tbl.JOB_ID " +
                                        "AND JOB_NUMBER = '{0}')";
            sqlQuery = string.Format(sqlQuery, dict[LogHeader.TOP_LEVEL_NO]);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                cmd.CommandTimeout = 0;
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    tmpList.Add(sr["PARENT_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllJobNoWhichHasDeletedJobCharges. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllJobNoWhichHasDeletedJobCharges. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllJobNoWhichHasDeletedJobCharges. " + ex.Message); }
            finally { cn.Close(); }

            return tmpList;
        }

        #endregion

        //20140620 - Gerry added to get the next job trip
        //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
        //AND CHANGE PARAMETER
        internal static HaulierJobTrip GetNextLegJobTrip(int jobID, int seqNo, int legGroup/*string containerNo*/, SqlConnection con, SqlTransaction tran)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            try
            {   //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
                string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        * 
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where D.JOB_ID = {0} AND D.SEQUENCE_NO > {1} AND LEG_GROUP = {2}"; //AND D.CONTAINER_NO = '{2}'";
                sqlQuery = string.Format(sqlQuery, jobID, seqNo, legGroup); // containerNo);
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                    retValue = jobTrip;

                    //retValue = GetHaulierJobTrip(reader);
                }
                reader.Close();
                retValue.JobTripStates = GetAllHaulierJobTripStates(retValue, con, tran);
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            return retValue;
        }
        internal static HaulierJobTrip GetNextLegJobTrip(HaulierJobTrip jobTrip, SqlConnection con, SqlTransaction tran)
        {
            HaulierJobTrip retValue = new HaulierJobTrip();
            try
            {   //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
                string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        * 
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where (M.JOB_ID in (select JOB_ID from TPT_JOB_MAIN_Tbl with (NOLOCK) where JOB_NUMBER in ('{0}','{1}')))
	                                        AND D.LEG_GROUP_MEMBER > {2} 
	                                        AND D.LEG_GROUP = {3}
                                        order by D.LEG_GROUP,D.LEG_GROUP_MEMBER asc";
                sqlQuery = string.Format(sqlQuery, jobTrip.JobNo, jobTrip.PartnerJobNo, jobTrip.LegGroupMember, jobTrip.LegGroup);

                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip temTrip = GetHaulierJobTrip(reader);
                    temTrip.JobTripStates = GetAllHaulierJobTripStates(temTrip, con, tran);
                    retValue = temTrip;

                    //retValue = GetHaulierJobTrip(reader);
                }
                reader.Close();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            return retValue;
        }
      
  
        //20140620 - gerry added to set next leg status to ready after completing the previous leg
        #region   gerry removed this method and combined to UpdateNextLegStartStopAndStatus
        /*
         * //
        internal static void SetNextLegToReady(HaulierJobTrip jobTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string sqlQuery = @"Update TPT_JOB_DETAIL_CARGO_Tbl set 
                                        STATUS = 2
                                        WHERE JOB_ID = {0}
                                        and SEQUENCE_NO = {1}
                                        and CONTAINER_NUMBER='{2}'";
                sqlQuery = string.Format(sqlQuery, jobTrip.JobID, jobTrip.Sequence, CommonUtilities.FormatString(jobTrip.ContainerNo));
                if (con.State != ConnectionState.Open) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
         */
        #endregion

        //20140729 - gerry added to set next leg trip start stop based on previous leg end stop
        //in this method the set next leg status and start will updated
        internal static void UpdateNextLegStartStopAndStatus(JobTripStatus prevLegStatus, HaulierJobTrip prevJobTrip, HaulierJobTrip nextJobTrip, bool autoSetNextLegToReady, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //{3} and {4} are based of the previous leg status and endStop
                string sqlQuery = @"Update TPT_JOB_DETAIL_CARGO_Tbl set 
                                        {3}{4}        
                                    WHERE JOB_ID = {0}
                                    and SEQUENCE_NO = {1}
                                    AND LEG_GROUP = {2}";
                //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
                //and CONTAINER_NUMBER='{2}'";
                string setStop = string.Empty;
                #region Query when end stop of the previous leg is changed
                if (prevJobTrip.EndStop.Code != nextJobTrip.StartStop.Code)
                {
                    setStop = @"FROM_STOP = '{0}',               
                                        FROM_ADD1 = '{1}',
                                        FROM_ADD2 = '{2}',
                                        FROM_ADD3 = '{3}',
                                        FROM_ADD4 = '{4}'";
                    setStop = string.Format(setStop, CommonUtilities.FormatString(prevJobTrip.EndStop.Code),
                                            CommonUtilities.FormatString(prevJobTrip.EndStop.Address1),
                                            CommonUtilities.FormatString(prevJobTrip.EndStop.Address2),
                                            CommonUtilities.FormatString(prevJobTrip.EndStop.Address3),
                                            CommonUtilities.FormatString(prevJobTrip.EndStop.Address4));
                }
                #endregion
                //this will be use to set ready status if prevLegStatus is completed, else empty
                string setToReady = string.Empty;
                #region Query when status of the previous leg is completed
                if (autoSetNextLegToReady)
                {
                    if (prevLegStatus == JobTripStatus.Completed && nextJobTrip.TripStatus == JobTripStatus.Booked)
                    {
                        setToReady = setStop == string.Empty ? "STATUS = 2" : ",STATUS = 2";
                    }
                }
                #endregion

                //to minimized the sql writings
                if (!setStop.Equals(string.Empty) || !setToReady.Equals(string.Empty))
                {
                    //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
                    sqlQuery = string.Format(sqlQuery, nextJobTrip.JobID, nextJobTrip.Sequence, nextJobTrip.LegGroup/*CommonUtilities.FormatString(nextJobTrip.ContainerNo)*/,
                                                setStop, setToReady);
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandTimeout = 0;
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    //get the update version of the next trip because it was updated
                    nextJobTrip.UpdateVersion = GetHaulierJobTripUpdateVersion(nextJobTrip, con, tran);
                    //add job trip state of the next job trip if set to ready
                    if (!setToReady.Equals(string.Empty))
                    {
                        JobTripState jobTripState = new JobTripState(1, JobTripStatus.Ready, prevJobTrip.StartDate, "auto set to ready", true);
                        nextJobTrip.AddJobTripState(jobTripState, con, tran);
                    }
                }
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateNextLegStartStopAndStatus. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateNextLegStartStopAndStatus. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateNextLegStartStopAndStatus. " + ex.Message); }
        }
        internal static void UpdateNextLegToReadyStatus(HaulierJobTrip currentJobTrip, HaulierJobTrip nextJobTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string sqlQuery = @"Update TPT_JOB_DETAIL_CARGO_Tbl set 
                                      status = 2      
                                    WHERE JOB_ID = {0}
                                    and SEQUENCE_NO = {1}
                                    AND LEG_GROUP = {2}";
                sqlQuery = string.Format(sqlQuery, nextJobTrip.JobID, nextJobTrip.Sequence, nextJobTrip.LegGroup);
                if (con.State != ConnectionState.Open) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateNextLegToReadyStatus. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateNextLegToReadyStatus. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateNextLegToReadyStatus. " + ex.Message); }
        }
        internal static void UpdatePreviousLegEndStop(HaulierJobTrip currentJobTrip, HaulierJobTrip prevJobTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //{3} and {4} are based of the previous leg status and endStop
                string sqlQuery = @"Update TPT_JOB_DETAIL_CARGO_Tbl set 
                                        {3}      
                                    WHERE JOB_ID = {0}
                                    and SEQUENCE_NO = {1}
                                    AND LEG_GROUP = {2}";
                //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
                //and CONTAINER_NUMBER='{2}'";
                string setStop = string.Empty;
                #region Query when end stop of the previous leg is changed
                if (currentJobTrip.StartStop.Code != prevJobTrip.StartStop.Code)
                {
                    setStop = @"TO_STOP = '{0}',               
                                        TO_ADD1 = '{1}',
                                        TO_ADD2 = '{2}',
                                        TO_ADD3 = '{3}',
                                        TO_ADD4 = '{4}'";
                    setStop = string.Format(setStop, CommonUtilities.FormatString(currentJobTrip.StartStop.Code),
                                            CommonUtilities.FormatString(currentJobTrip.StartStop.Address1),
                                            CommonUtilities.FormatString(currentJobTrip.StartStop.Address2),
                                            CommonUtilities.FormatString(currentJobTrip.StartStop.Address3),
                                            CommonUtilities.FormatString(currentJobTrip.StartStop.Address4));
                }
                #endregion
                //to minimized the sql writings
                if (!setStop.Equals(string.Empty))
                {
                    //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
                    sqlQuery = string.Format(sqlQuery, prevJobTrip.JobID, prevJobTrip.Sequence, prevJobTrip.LegGroup, setStop);
                    if (con.State != ConnectionState.Open) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandTimeout = 0;
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    //get the update version of the next trip because it was updated
                    prevJobTrip.UpdateVersion = GetHaulierJobTripUpdateVersion(prevJobTrip, con, tran);
                }
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdatePreviousLegEndStop. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdatePreviousLegEndStop. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdatePreviousLegEndStop. " + ex.Message); }
        }
    
        
        // 2014-07-07 Zhou Kai adds 
        public static Dictionary<string, string> GetDefaultPortForSeaImportJob()
        {
            Dictionary<string, string> dictDefaultSeaImportPort = new Dictionary<string, string>();
            string sqlQuery = "SELECT * FROM [TPT_SPECIAL_DATA_Tbl] with (NOLOCK) INNER JOIN [CRT_Operation_Database_Tbl] with (NOLOCK) " +
                "ON [TPT_SPECIAL_DATA_Tbl].[DEFAULT_IMPORT_PORT_CODE] = " +
                "[CRT_Operation_Database_Tbl].[Operation_Code]";
            using (var dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            using (var dbCmd = new SqlCommand(sqlQuery, dbCon))
            {
                try
                {
                    if (dbCon.State != ConnectionState.Open) { dbCon.Open(); }
                    IDataReader dbReader = dbCmd.ExecuteReader();
                    int iCount = 0;
                    while (dbReader.Read())
                    {
                        dictDefaultSeaImportPort.Add("Operation_Code", dbReader["Operation_Code"].ToString());
                        dictDefaultSeaImportPort.Add("Operation_Name", dbReader["Operation_Name"].ToString());
                        dictDefaultSeaImportPort.Add("Op_Add1", dbReader["Op_Add1"].ToString());
                        dictDefaultSeaImportPort.Add("Op_Add2", dbReader["Op_Add2"].ToString());
                        dictDefaultSeaImportPort.Add("Op_Add3", dbReader["Op_Add3"].ToString());
                        dictDefaultSeaImportPort.Add("Op_Add4", dbReader["Op_Add4"].ToString());
                        dictDefaultSeaImportPort.Add("City_Code", dbReader["City_Code"].ToString());
                        iCount++;
                    }
                    if (iCount == 0)
                    {
                        throw new FMException(TptResourceUI.TheDefaultImPortCodeIsNotSetCorrectly);
                    }
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetDefaultPortForSeaImportJob. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetDefaultPortForSeaImportJob. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetDefaultPortForSeaImportJob. " + ex.Message); }
            }

            return dictDefaultSeaImportPort;
        }

        /// <summary>
        /// 2014-07-29 Zhou Kai adds, to get the quantity of containers for each type
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetContainerQtyByJobID(int jobId)
        {
            Dictionary<string, string> dictContainerQty = new Dictionary<string, string>();
            int n20 = 0, n40 = 0, n45 = 0, nOTH = 0;
            string sqlQuery = String.Empty;
            sqlQuery = @"SELECT jt.JOB_ID ,jt.SEQUENCE_NO, jt.LEG_GROUP,jt.CONTAINER_NUMBER, jt.LEG_TYPE,cont.CONTAINER_CODE
                            FROM TPT_JOB_DETAIL_CARGO_Tbl jt with (NOLOCK)
                            inner JOIN SRT_Container_Tbl cont with (NOLOCK)
                            ON jt.CONTAINER_CODE =cont.Container_Code 
                            where jt.JOB_ID = {0}
                             ORDER BY JOB_ID, SEQUENCE_NO ASC";
            sqlQuery = String.Format(sqlQuery, jobId);

            using (SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    SqlCommand dbCmd = new SqlCommand(sqlQuery, dbCon);
                    dbCmd.CommandTimeout = 0;
                    if (dbCon.State != ConnectionState.Open) { dbCon.Open(); }
                    List<int> visitedLegGroup = new List<int>();

                    SqlDataAdapter dbAdpt = new SqlDataAdapter();
                    dbAdpt.SelectCommand = dbCmd;
                    DataTable tbl = new DataTable();
                    dbAdpt.Fill(tbl);

                    //20150708 - gerry modified the pulling of ContainerTypesDTO size
                    foreach (DataRow dr in tbl.Rows)
                    {
                        if (dr["Container_Code"].ToString().Contains(SeaJob.TWENTYFOOT.Substring(0, 2))) //.Equals(SeaFreight.BLL.SeaJob.TWENTYFOOT))
                        {
                            if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                            {
                                n20++;
                                visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                            }
                        }
                        if (dr["Container_Code"].ToString().Contains(SeaJob.FORTYFOOT.Substring(0, 2))) //.Equals(SeaFreight.BLL.SeaJob.FORTYFOOT))
                        {
                            if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                            {
                                n40++;
                                visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                            }
                        }
                        if (dr["Container_Code"].ToString().Contains(SeaJob.FORTYFIVEFOOT.Substring(0, 2))) //.Equals(SeaFreight.BLL.SeaJob.FORTYFIVEFOOT))
                        {
                            if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                            {
                                n45++;
                                visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                            }
                        }
                        if (dr["Container_Code"].ToString().Contains(SeaJob.OTHERSIZE.Substring(0, 2))) //.Equals(SeaFreight.BLL.SeaJob.OTHERSIZE))
                        {
                            if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                            {
                                nOTH++;
                                visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                            }
                        }
                    }

                    dictContainerQty.Add(SeaJob.TWENTYFOOT, n20.ToString());
                    dictContainerQty.Add(SeaJob.FORTYFOOT, n40.ToString());
                    dictContainerQty.Add(SeaJob.FORTYFIVEFOOT, n45.ToString());
                    dictContainerQty.Add(SeaJob.OTHERSIZE, nOTH.ToString());
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetContainerQtyByJobID. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetContainerQtyByJobID. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetContainerQtyByJobID. " + ex.Message); }
            }

            return dictContainerQty;
        }
        //20140818 - gerry added    
        public static Dictionary<string, Decimal> GetContainerQtyByJobID_New(int jobId, SqlConnection con, SqlTransaction tran)
        {
            Dictionary<string, Decimal> dictContainerQty = new Dictionary<string, Decimal>();
            int n20 = 0, n40 = 0, n45 = 0, nOTH = 0;
            string sqlQuery = String.Empty;
            sqlQuery = @"SELECT jt.JOB_ID ,jt.SEQUENCE_NO, jt.LEG_GROUP,jt.CONTAINER_NUMBER, jt.LEG_TYPE,cont.CONTAINER_CODE
                            FROM TPT_JOB_DETAIL_CARGO_Tbl jt with (NOLOCK)
                            inner JOIN SRT_Container_Tbl cont with (NOLOCK)
                            ON jt.CONTAINER_CODE =cont.Container_Code 
                            where jt.JOB_ID = {0}
                             ORDER BY JOB_ID, SEQUENCE_NO ASC";
            sqlQuery = String.Format(sqlQuery, jobId);

            try
            {
                SqlCommand dbCmd = new SqlCommand(sqlQuery, con);
                dbCmd.CommandTimeout = 0;
                dbCmd.Transaction = tran;
                List<int> visitedLegGroup = new List<int>();
                SqlDataAdapter dbAdpt = new SqlDataAdapter();
                dbAdpt.SelectCommand = dbCmd;
                DataTable tbl = new DataTable();
                dbAdpt.Fill(tbl);

                //20150708 - gerry modified the pulling of ContainerTypesDTO size
                foreach (DataRow dr in tbl.Rows)
                {
                    if (dr["Container_Code"].ToString().Contains(SeaJob.TWENTYFOOT.Substring(0, 2))) //.Equals(SeaFreight.BLL.SeaJob.TWENTYFOOT))
                    {
                        if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                        {
                            n20++;
                            visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                        }
                    }
                    if (dr["Container_Code"].ToString().Contains(SeaJob.FORTYFOOT.Substring(0, 2))) //.Equals(SeaFreight.BLL.SeaJob.FORTYFOOT))
                    {
                        if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                        {
                            n40++;
                            visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                        }
                    }
                    if (dr["Container_Code"].ToString().Contains(SeaJob.FORTYFIVEFOOT.Substring(0, 2))) //.Equals(SeaFreight.BLL.SeaJob.FORTYFIVEFOOT))
                    {
                        if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                        {
                            n45++;
                            visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                        }
                    }
                    if (dr["Container_Code"].ToString().Contains(SeaJob.OTHERSIZE.Substring(0, 2))) //.Equals(SeaFreight.BLL.SeaJob.OTHERSIZE))
                    {
                        if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                        {
                            nOTH++;
                            visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                        }
                    } 
                    if (dr["Container_Code"].ToString().Contains("40HC")) //.Equals(SeaFreight.BLL.SeaJob.OTHERSIZE))
                    {
                        if (!visitedLegGroup.Contains(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"])))
                        {
                            nOTH++;
                            visitedLegGroup.Add(dr["LEG_GROUP"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LEG_GROUP"]));
                        }
                    }
                }

                dictContainerQty.Add(SeaJob.TWENTYFOOT, n20);
                dictContainerQty.Add(SeaJob.FORTYFOOT, n40);
                dictContainerQty.Add(SeaJob.FORTYFIVEFOOT, n45);
                dictContainerQty.Add(SeaJob.OTHERSIZE, nOTH);
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetContainerQtyByJobID. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetContainerQtyByJobID. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetContainerQtyByJobID. " + ex.Message); }
            
            return dictContainerQty;
        }
     
        internal static HaulierJob GetHaulierJob(string JobNo, SqlConnection cn, SqlTransaction tran)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl  
            HaulierJob Operator = new HaulierJob();
            try
            {
                string SQLString = "SELECT * FROM TPT_JOB_MAIN_Tbl with (NOLOCK) LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TPT_JOB_MAIN_Tbl.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID";
                SQLString += " WHERE TPT_JOB_MAIN_Tbl.JOB_NUMBER='" + JobNo + "'";
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetHaulierJob(reader);
                }
                reader.Close();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJob. " + ex.Message); }
            return Operator;
        }
        //20140829 - gerry overload the methid to get the next job trip
        //20150824 - gerry cahnge parameter containerNo to leggroup
        internal static HaulierJobTrip GetNextLegJobTrip(int jobID, int seqNo, /*string containerNo*/ int legGroup)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            HaulierJobTrip retValue = new HaulierJobTrip();
            try
            {
                string sqlQuery = @"SELECT TOP 1 D.UPDATE_VERSION AS UPD_VER,
                                        convert(varchar, D.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
																where (a.JOB_ID = D.JOB_ID and a.LEG_GROUP = D.LEG_GROUP) 
																		OR (a.CONTAINER_NUMBER = D.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = M.JOB_NUMBER)) Leg, 
                                        * 
                                        from TPT_JOB_DETAIL_CARGO_Tbl as D with (NOLOCK)
                                        LEFT JOIN TPT_JOB_MAIN_Tbl as M with (NOLOCK) ON D.JOB_ID = M.JOB_ID
                                        LEFT JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl msi with (NOLOCK) ON M.JOB_ID=msi.JOB_ID
                                        where D.JOB_ID = {0} AND D.SEQUENCE_NO > {1} AND D.LEG_GROUP = '{2}'
                                        ORDER BY D.SEQUENCE_NO";
                sqlQuery = string.Format(sqlQuery, jobID, seqNo, legGroup);
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HaulierJobTrip jobTrip = GetHaulierJobTrip(reader);
                    jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                    retValue = jobTrip;
                }
                reader.Close();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegJobTrip. " + ex.Message); }
            return retValue;
        }

        /// <summary>
        /// 2014-10-04 Zhou Kai.
        /// Get the partnerJobTrips of thisJob
        /// </summary>
        /// <param name="thisJob">The haulier job</param>
        /// <returns>The partnerTrips of thisJob</returns>
        public static SortableList<HaulierJobTrip> GetPartnerTrips(HaulierJob thisJob)
        {
            try
            {
                if (thisJob.JobTransferType != JobTransferType.TransferIn)
                {
                    thisJob.PartnerJobTrips.Clear();
                    return thisJob.PartnerJobTrips;
                }
                else
                {
                    return
                        GetPartnerTripsForInJobFromOutJob(/*thisJob.PartnerJobNo,*/ thisJob.JobNo);
                }
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPartnerTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPartnerTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPartnerTrips. " + ex.Message); }
        }

        /// <summary>
        /// 2014-10-04 Zhou Kai. Get trips transferred
        /// in to inJobNo from outJobNo
        /// </summary>
        /// <param name="outJobNo">
        /// The job number of the transfer out job.
        /// </param>
        /// <param name="inJobNo">
        /// The job number of the transfer in job.
        /// </param>
        /// <returns>
        /// The trips transferred out to inJobNo
        /// </returns> 
        /// 
        //20141027 - gerry's note paremeter is not in use at the moment
        public static SortableList<HaulierJobTrip>
           GetPartnerTripsForInJobFromOutJob(/*string outJobNo,*/ string inJobNo)
        {
            // select all the trips whoes partnerJobNo ptnrJobNo of job.
            SortableList<HaulierJobTrip> rtn = new SortableList<HaulierJobTrip>();
            using (SqlConnection dbCon = new SqlConnection(
                FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction dbTran = null;
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbTran = dbCon.BeginTransaction();

                    return GetPartnerTripsForInJobFromOutJob(inJobNo, dbCon, dbTran);
                }
                catch (FMException fmEx)
                { if (dbTran != null) { dbTran.Rollback(); } throw fmEx; }
                catch (SqlException ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.ToString()); }
                catch (InvalidOperationException ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.ToString()); }
            }
        }

        public static SortableList<HaulierJobTrip> GetPartnerTripsForInJobFromOutJob(string inJobNo, SqlConnection dbCon, SqlTransaction dbTran)
        {
            // select all the trips whoes partnerJobNo = ptnrJobNo of job.
            SortableList<HaulierJobTrip> rtn = new SortableList<HaulierJobTrip>();
            //20141103 - gerry modified query to display partner jobNo of the old trips, using TPT_JOB_DETAIL_CARGO_Tbl.PARTNER_JOB_NO field(same field name both header and detail)
            string sqlQry = @"SELECT jt.UPDATE_VERSION AS UPD_VER, jt.PARTNER_JOB_NO, 
	                                    (select case jm.JOB_TRANSFER_TYPE when '2' then
		                                    convert(varchar, jt.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
								                                    where (a.JOB_ID = jt.JOB_ID and a.CONTAINER_NUMBER = jt.CONTAINER_NUMBER) -- and a.LEG_GROUP = jt.LEG_GROUP) 
										                                    OR (a.CONTAINER_NUMBER = jt.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = jm.JOB_NUMBER))
										                                    else  
		                                    convert(varchar, jt.LEG_GROUP_MEMBER) + '/' + (select convert(varchar, cOUNT(*)) from TPT_JOB_DETAIL_CARGO_Tbl a with (NOLOCK) 
								                                    where (a.JOB_ID = jt.JOB_ID and a.LEG_GROUP = jt.LEG_GROUP) 
										                                    OR (a.CONTAINER_NUMBER = jt.CONTAINER_NUMBER AND a.PARTNER_JOB_NO = jm.JOB_NUMBER)) 
										                                    end) as Leg, 
                                *
                                FROM TPT_JOB_DETAIL_CARGO_Tbl jt with (NOLOCK)
                                INNER JOIN  TPT_JOB_MAIN_TBL jm with (NOLOCK) ON 
                                jm.JOB_ID = jt.JOB_ID AND
                                jt.PARTNER_JOB_NO = '{0}'
                                INNER JOIN TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK) ON jm.JOB_ID=TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID";
            sqlQry = String.Format(sqlQry, inJobNo);
            using (SqlCommand dbCmd = new SqlCommand(sqlQry, dbCon))
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbCmd.CommandTimeout = 0;
                    dbCmd.Transaction = dbTran;
                    SqlDataReader dbRdr = dbCmd.ExecuteReader();
                    while (dbRdr.Read())
                    {
                        HaulierJobTrip jobTrip = GetHaulierJobTrip(dbRdr);
                        jobTrip.JobTripStates = GetAllHaulierJobTripStates(jobTrip);
                        jobTrip.LegTypeCustomized = dbRdr["Leg"] == DBNull.Value ? string.Empty : dbRdr["Leg"].ToString();
                        rtn.Add(jobTrip);

                        //rtn.Add(GetHaulierJobTrip(dbRdr));
                    }
                    dbRdr.Close();
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetPartnerTripsForInJobFromOutJob. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetPartnerTripsForInJobFromOutJob. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetPartnerTripsForInJobFromOutJob. " + ex.Message); }

            return rtn;
        }

        /// <summary>
        /// 2014-10-09 Zhou Kai.
        /// Get the next legGroup id from db
        /// </summary>
        /// <param name="jobId">The id of the haulier
        /// job this function works on</param>
        /// <returns>The next leg group id</returns>
        internal static int GetNextLegGroup(int jobId)
        {
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();

            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    SqlTransaction dbTran = dbCon.BeginTransaction();
                    int n = GetNextLegGroup(jobId, dbCon, dbTran);
                    dbTran.Commit();

                    return n;
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroup. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroup. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroup. " + ex.Message); }
            }

        }

        /// <summary>
        /// 2014-10-09 Zhou Kai.
        /// Get the next legGroup id from db
        /// </summary>
        /// <param name="jobId">The id of the haulier job this function works on</param>
        /// <returns>The next leg group id</returns>
        internal static int GetNextLegGroup(int jobId, SqlConnection dbCon, SqlTransaction dbTran)
        {
            string sqlQuery = "SELECT COUNT(JOB_ID) FROM " +
                                       "TPT_JOB_MAIN_Tbl WHERE JOB_ID = {0}";
            sqlQuery = String.Format(sqlQuery, jobId);

            try
            {
                SqlCommand dbCmd = new SqlCommand();
                dbCmd.CommandTimeout = 0;
                dbCmd.Transaction = dbTran;
                dbCmd.Connection = dbCon;
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = sqlQuery;
                int n = (int)dbCmd.ExecuteScalar();

                if (n == 0)
                { /*return 1;*/
                    throw new FMException("GetNextLegGroup(): " + " the jobId: " +
                        jobId.ToString() + " does not exist.");
                }
                if (n == 1)
                {
                    sqlQuery = "SELECT MAX(LEG_GROUP) FROM " +
                                      "TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK) WHERE JOB_ID = {0}";
                    sqlQuery = String.Format(sqlQuery, jobId);
                    dbCmd.CommandText = sqlQuery;

                    n = dbCmd.ExecuteScalar() == DBNull.Value ?
                                                                                1 :
                                                                                Convert.ToInt32(dbCmd.ExecuteScalar()) + 1;

                    return n;
                }

                throw new FMException("GetNextLegGroup(), " + "duplicated Job_Id = " + jobId);

            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroup. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroup. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroup. " + ex.Message); }

        }

        /// <summary>
        /// 2014-10-09 Zhou Kai
        /// Get the next legGroup id from db
        /// </summary>
        /// <param name="job">The haulier job trip this
        /// function works on</param>
        /// <returns>The next leg group id</returns>
        internal static int GetNextLegGroupMember(int jobId, int legGroup)
        {
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    SqlTransaction dbTran = dbCon.BeginTransaction();
                    int nRtn = GetNextLegGroupMember(jobId, legGroup, dbCon, dbTran);

                    dbTran.Commit();
                    return nRtn;
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupMember. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupMember. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupMember. " + ex.Message); }
            }

        }

        /// <summary>
        /// 2014-10-09 Zhou Kai
        /// Get the next legGroup id from db
        /// </summary>
        /// <param name="job">The haulier job trip this function works on</param>
        /// <returns>The next leg group id</returns>
        internal static int GetNextLegGroupMember(int jobId,
                                                                           int legGroup,
                                                                           SqlConnection dbCon,
                                                                           SqlTransaction dbTran
                                                                            )
        {
            // legGroupMember depends on legGroup
            if (legGroup == -1)
            {
                throw new FMException("GetNextLegGroupMember()" +
                                                        ": the legGroup is not assigned yet.");
            }

            string sqlQuery = "SELECT MAX(LEG_GROUP_MEMBER)" +
                                        " FROM TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK) " +
                                        " WHERE JOB_ID = {0} AND LEG_GROUP = {1}";
            sqlQuery = String.Format(sqlQuery, jobId, legGroup);
            using (SqlCommand dbCmd = new SqlCommand())
            {
                try
                {
                    dbCmd.CommandTimeout = 0;
                    dbCmd.Connection = dbCon;
                    dbCmd.Transaction = dbTran;
                    dbCmd.CommandType = CommandType.Text;
                    dbCmd.CommandText = sqlQuery;

                    return dbCmd.ExecuteScalar() == DBNull.Value ?
                                                                         1 :
                                                                         Convert.ToInt32(dbCmd.ExecuteScalar()) + 1;
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupMember. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupMember. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupMember. " + ex.Message); }
            }

        }

        /// <summary>
        /// 2014-10-09 Zhou Kai. 
        /// The function gets the legGroup by jobId and containerNo
        /// </summary>
        /// <param name="jobId">The job id </param>
        /// <param name="containerNo">The container number </param>
        /// <returns>The next legGroup of the trip with the containerNo</returns>
        public static int GetNextLegGroupByContainerNo(int jobId, string containerNo,
            SqlConnection dbCon, SqlTransaction dbTran)
        {
            if (containerNo == String.Empty)
            {
                throw new FMException("GetLegGroupByContainerNo(): " +
                  "please provide container number.");
            }

            string sqlQuery = "SELECT COUNT(JOB_ID) FROM " +
                                       "TPT_JOB_MAIN_Tbl with (NOLOCK) WHERE JOB_ID = {0}";
            sqlQuery = String.Format(sqlQuery, jobId);

            try
            {
                SqlCommand dbCmd = new SqlCommand();
                dbCmd.CommandTimeout = 0;
                dbCmd.Transaction = dbTran;
                dbCmd.Connection = dbCon;
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = sqlQuery;
                int n = (int)dbCmd.ExecuteScalar();

                if (n == 0)
                { /*return 1;*/
                    throw new FMException("GetNextLegGroupByContainerNo(): " + " the jobId: " +
                        jobId.ToString() + " does not exist.");
                }
                if (n == 1)
                {
                    sqlQuery = "SELECT MAX(LEG_GROUP) FROM " +
                                      "TPT_JOB_DETAIL_CARGO_Tbl with (NOLOCK) WHERE JOB_ID = {0} " +
                                      "AND CONTAINER_NUMBER = '{1}'";
                    sqlQuery = String.Format(sqlQuery, jobId, containerNo);
                    dbCmd.CommandText = sqlQuery;

                    n = dbCmd.ExecuteScalar() == DBNull.Value ?
                                                                                GetNextLegGroup(jobId, dbCon, dbTran) :
                                                                                Convert.ToInt32(dbCmd.ExecuteScalar());

                    return n;
                }

                throw new FMException("GetNextLegGroup(), " + "duplicated Job_Id = " + jobId);
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupByContainerNo. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupByContainerNo. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupByContainerNo. " + ex.Message); }
        }

        /// <summary>
        /// 2014-10-09 Zhou Kai. 
        /// The function gets the legGroup by jobId and containerNo
        /// </summary>
        /// <param name="jobId">The job id </param>
        /// <param name="containerNo">The container number </param>
        /// <returns>The next legGroup of the trip with the containerNo</returns>
        public static int GetNextLegGroupByContainerNo(int jobId, string containerNo)
        {
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    SqlTransaction dbTran = dbCon.BeginTransaction();
                    return GetNextLegGroupByContainerNo(jobId, containerNo, dbCon, dbTran);
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupByContainerNo. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupByContainerNo. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetNextLegGroupByContainerNo. " + ex.Message); }
            }
        }

        /// <summary>
        /// 2014-10-09 Zhou Kai adds.
        /// Update the legGroupMember of the following
        /// trips when one of the trip in a legGroup is deleted.
        /// </summary>
        /// <param name="trip">The trip to be deleted</param>
        /// <returns>true: successfull; false: failed</returns>
        internal static bool ChangeLegGroupMember(HaulierJobTrip trip)
        {
            // this function only provides valid database connection, and starts transaction,
            // the real-logic is inside the other version of:
            // ChangeLegGroupMember() function with dbCon, dbTran as parameters

            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            bool rtn = false;
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    SqlTransaction dbTran = dbCon.BeginTransaction();

                    rtn = ChangeLegGroupMember(trip, dbCon, dbTran);

                    dbTran.Commit();
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : ChangeLegGroupMember. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : ChangeLegGroupMember. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : ChangeLegGroupMember. " + ex.Message); }
            }

            return rtn;
        }

        /// <summary>
        /// 2014-10-09 Zhou Kai adds.
        /// Update the legGroupMember of the following
        /// trips when one of the trip in a legGroup is deleted.
        /// </summary>
        /// <param name="trips">The trip to be deleted</param>
        /// <returns>true: successfull; false: failed</returns>
        internal static bool ChangeLegGroupMember(HaulierJobTrip trip,
                                                                             SqlConnection dbCon,
                                                                             SqlTransaction dbTran
                                                                            )
        {
            int jId = trip.JobID;
            int legGroup = trip.LegGroup;
            int legGM = trip.LegGroupMember;
            string sqlQuery = "UPDATE TPT_JOB_DETAIL_CARGO_Tbl " +
                "SET LEG_GROUP_MEMBER = LEG_GROUP_MEMBER - 1 " +
                "WHERE JOB_ID = {0} AND LEG_GROUP = {1} AND " +
                "LEG_GROUP_MEMBER >= {2};";
            sqlQuery = String.Format(sqlQuery, jId, legGroup, legGM);

            using (SqlCommand dbCmd = new SqlCommand())
            {
                try
                {
                    dbCmd.CommandTimeout = 0;
                    dbCmd.Connection = dbCon;
                    dbCmd.Transaction = dbTran;
                    dbCmd.CommandType = CommandType.Text;
                    dbCmd.CommandText = sqlQuery;

                    dbCmd.ExecuteNonQuery();

                    return true;
                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : ChangeLegGroupMember. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : ChangeLegGroupMember. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : ChangeLegGroupMember. " + ex.Message); }

            }

        }

        /// <summary>
        /// 2014-10-10 Zhou Kai.
        /// Add a collection of trips to the existing job. Notice that the existing job may have existing
        /// trips.
        /// </summary>
        /// <param name="job">The job the trips collection will be added to.</param>
        /// <param name="trips">The collection of trips</param>
        /// <returns>true: succeeded; false: failed</returns>
        internal static bool AddHaulierJobTrips(HaulierJob job,
                                                                    SortableList<HaulierJobTrip> trips,
                                                                    string formName,
                                                                    string usrId)
        {
            // inserting the collection of trips in one transaction
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                SqlTransaction dbTran = null;
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbTran = dbCon.BeginTransaction();

                    // calculate the charges:
                    SortableList<HaulierJobTrip> allTrips = job.JobTrips;
                    foreach (HaulierJobTrip t in trips) 
                    {
                        allTrips.Add(t); 
                    }
                    job.JobTrips = allTrips;
                    HaulierJob.ChangeContainerMovementCharge(job, dbCon, dbTran);
                    // end, calculate charges

                    // add trips into database, and write logs
                    bool rtn = AddHaulierJobTrips(job, trips, formName, usrId, dbCon, dbTran);

                    dbTran.Commit();

                    return rtn;
                }
                catch (FMException fmEx)
                { if (dbTran != null) { dbTran.Rollback(); } throw fmEx; }
                catch (SqlException ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                catch (InvalidOperationException ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                catch (Exception ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
            }

        }

        internal static bool AddHaulierJobTrips(HaulierJob job, SortableList<HaulierJobTrip> trips, string formName,
                                                 string usrId, SqlConnection dbCon, SqlTransaction dbTran)
        {
            try
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_SET_JOBTRIP_TO_READY);
                foreach (HaulierJobTrip t in trips)
                {
                    //20170112 - gerry added
                    if (t.LegGroup == -1)
                    {
                        if (t.ContainerNo == String.Empty)
                        {
                            t.LegGroup = HaulierJobDAL.GetNextLegGroup( t.JobID, dbCon, dbTran);
                        }
                        else
                        {
                            t.LegGroup = HaulierJobDAL.GetNextLegGroupByContainerNo(t.JobID, t.ContainerNo, dbCon, dbTran);
                        }
                    }
                    if (t.LegGroupMember == -1)
                    {
                        t.LegGroupMember = HaulierJobDAL.GetNextLegGroupMember(t.JobID, t.LegGroup, dbCon, dbTran);
                    }
                    if (option.setting_Value.ToString() == "T")
                        t.TripStatus = t.LegGroupMember == 1 ? JobTripStatus.Ready : t.TripStatus;//set to ready
                    //20170112 ends

                    // 2014-10-16 Zhou Kai uses DAL function instead, 
                    // so no logs written, no charges calculated, to avoid transaction conflict
                    //job.AddHaulierJobTrip(t, formName, usrId, dbCon, dbTran);
                    AddHaulierJobTrip(job, t, dbCon, dbTran);
                    //if (t.PartnerJobNo != string.Empty)
                    //{
                    //    HaulierJob hubbingJob = GetHaulierJob(t.PartnerJobNo, dbCon, dbTran);
                    //    //TODO update hubbingjob leg group
                    //    UpdatePartnerTripsLegGroup(t.LegGroup, hubbingJob.JobID, t.ContainerNo, dbCon, dbTran);
                    //}

                    Trace.WriteLine("JID: " + job.JobID + " Seq: " + t.Sequence + ", jobTripStates.Count: " + t.JobTripStates.Count +
                        "ContainerCode: " + t.ContainerCode);
                    // add states
                    JobTripState jobTripState = new JobTripState(1, t.TripStatus, job.JobDateTime, "", true);
                    t.AddJobTripState(jobTripState, dbCon, dbTran);
					#region removed
                    // write logs in the seperate step:
                    //DateTime serverDateTime = Logger.GetServerDateTime();
                    //LogHeader lh = new LogHeader(FMModule.Transport, formName, serverDateTime,
                    //                                                job.JobNo, t.Sequence.ToString(), FormMode.Add, usrId);

                    //LogDetail ld1 = new LogDetail(HaulierJob.LOGDETAIL_CONTAINER_CODE,
                    //    t.ContainerCode);
                    //LogDetail ld2 = new LogDetail(HaulierJob.LOGDETAIL_START_STOP, t.StartStop.Code);
                    //LogDetail ld3 = new LogDetail(HaulierJob.LOGDETAIL_END_STOP, t.EndStop.Code);
                    //LogDetail ld4 = new LogDetail(HaulierJob.LOGDETAIL_DATE_FROM, t.StartDate.ToShortDateString());
                    //LogDetail ld5 = new LogDetail(HaulierJob.LOGDETAIL_DATE_TO, t.EndDate.ToShortDateString());
                    //LogDetail ld6 = new LogDetail(HaulierJob.LOGDETAIL_IS_MULTI_LEG, t.isMulti_leg ? "Yes" : "No");
                    //LogDetail ld7 = new LogDetail(HaulierJob.LOGDETAIL_CONTAINER_NO, t.ContainerNo);
                    //LogDetail ld8 = new LogDetail(HaulierJob.LOGDETAIL_SEAL_NO, t.SealNo);
                    //LogDetail ld9 = new LogDetail(HaulierJob.LOGDETAIL_LEGTYPE, t.LegType.ToString());
                    //LogDetail ld10 = new LogDetail(HaulierJob.LOGDETAIL_PARTNER_LEG, t.PartnerLeg.ToString());

                    //lh.LogDetails.Add(ld1);
                    //lh.LogDetails.Add(ld2);
                    //lh.LogDetails.Add(ld3);
                    //lh.LogDetails.Add(ld4);
                    //lh.LogDetails.Add(ld5);
                    //lh.LogDetails.Add(ld6);
                    //lh.LogDetails.Add(ld7);
                    //lh.LogDetails.Add(ld8);
                    //lh.LogDetails.Add(ld9);
                    //lh.LogDetails.Add(ld10);

                    //Logger.WriteLog(lh, dbCon, dbTran);

                    // 2014-10-16 Zhou Kai ends
                    #endregion

                    Dictionary<string, Decimal> dictContainerQty = GetContainerQtyByJobID_New(job.JobID, dbCon, dbTran);
                    TransportFacadeOut.EditTptJobToSpecialJobDetail(job, dbCon, dbTran, HaulierJobDAL.GetPrefix(), dictContainerQty);
                    ////20151028 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(t.JobNo, "HL", "BK", t.JobID, usrId, DateTime.Now, formName, t.Sequence, FormMode.Add.ToString());
                    auditLog.WriteAuditLog("JobNumber", t.JobNo.ToString(), string.Empty, dbCon, dbTran);
                    auditLog.WriteAuditLog("SeqNo", t.Sequence.ToString(), string.Empty, dbCon, dbTran);
                }
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : AddHaulierJobTrips. " + ex.Message); }
            return true;
        }

        //20141010 - gerry added to update the partnerJobNo
        internal static bool UpdatePartnerTrips(int jobId, List<string> containerNos, string partnerJobNo, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string contNos = string.Join("','", containerNos.ToArray());
                string SQLUpdateString = @"UPDATE TPT_JOB_DETAIL_CARGO_Tbl 
                                        SET PARTNER_JOB_NO = '{0}'
                                    Where JOB_ID = {1}
                                        AND CONTAINER_NUMBER in ('{2}')";
                SQLUpdateString = string.Format(SQLUpdateString, CommonUtilities.FormatString(partnerJobNo),
                                                                jobId, contNos);

                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(SQLUpdateString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdatePartnerTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdatePartnerTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdatePartnerTrips. " + ex.Message); }
            return true;
        } 
        //20160531 - gerry added to update the partnerJobNo
        internal static bool UpdatePartnerTripsLegGroup(int legGroup, int jobId, string containerNo,  SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLUpdateString = @"UPDATE TPT_JOB_DETAIL_CARGO_Tbl 
                                        SET LEG_GROUP = '{0}'
                                    Where JOB_ID = {1}
                                        AND CONTAINER_NUMBER = '{2}' ";
                SQLUpdateString = string.Format(SQLUpdateString, legGroup, jobId, containerNo);

                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(SQLUpdateString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdatePartnerTrips. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdatePartnerTrips. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdatePartnerTrips. " + ex.Message); }
            return true;
        }
        #region "2014-10-30 Zhou Kai"
        internal static void TransferServiceCharges(int outJobId, int inJobId)
        {
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                SqlTransaction dbTran = null;
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbTran = dbCon.BeginTransaction();
                    TransferServiceCharges(outJobId, inJobId, dbCon, dbTran);

                    dbTran.Commit();
                }
                catch (FMException fmEx)
                { if (dbTran != null) { dbTran.Rollback(); } throw fmEx; }
                catch (SqlException ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                catch (InvalidOperationException ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                catch (Exception ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }

            }

        }

        internal static void TransferServiceCharges(int outJobId, int inJobId, SqlConnection dbCon,
            SqlTransaction dbTran)
        {
            // 2014-10-31 Zhou Kai comments out the function
            //string strSql = "UPDATE TPT_JOB_DETAIL_CHARGE_Tbl " +
            //    "SET JOB_ID = {0} WHERE JOB_ID = {1} AND JOB_CHARGE_TYPE = 'S'";
            //strSql = String.Format(strSql, inJobId, outJobId);

            //using (SqlCommand dbCmd = new SqlCommand(strSql, dbCon))
            //{
            //    try
            //    {
            //        dbCmd.Transaction = dbTran;
            //        dbCmd.ExecuteNonQuery();
            //    }
            //    catch (FMException fmEx) { throw fmEx; }
            //    catch (SqlException ex) { throw new FMException(ex.Message); }
            //    catch (InvalidOperationException ex) { throw new FMException(ex.Message); }
            //    catch (Exception ex) { throw new FMException(ex.Message); }

            //}

        }

        internal static bool ErasePartnerJobNo(string inJobNo, string outJobNo)
        {
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                SqlTransaction dbTran = null;
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbTran = dbCon.BeginTransaction();
                    HaulierJobDAL.ErasePartnerJobNo(inJobNo, outJobNo, dbCon, dbTran);

                    dbTran.Commit();
                    return true;
                }
                catch (FMException fmEx)
                { if (dbTran != null) { dbTran.Rollback(); } throw fmEx; }
                catch (SqlException ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                catch (InvalidOperationException ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                catch (Exception ex)
                { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }

            }

        }

        internal static bool ErasePartnerJobNo(string inJobNo, string outJobNo, SqlConnection dbCon,
            SqlTransaction dbTran)
        {
            string sqlQuery =
                "UPDATE TPT_JOB_DETAIL_CARGO_Tbl SET PARTNER_JOB_NO = '' " +
                "WHERE JOB_ID = (SELECT JOB_ID FROM TPT_JOB_MAIN_Tbl with (NOLOCK) " +
                "WHERE  JOB_Number = '{0}') AND PARTNER_JOB_NO = '{1}';";
            sqlQuery = String.Format(sqlQuery, inJobNo, outJobNo);
            using (SqlCommand dbCmd = new SqlCommand(sqlQuery, dbCon))
            {
                try
                {
                    dbCmd.CommandTimeout = 0;
                    dbCmd.Transaction = dbTran;
                    dbCmd.ExecuteNonQuery();

                    return true;
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : ErasePartnerJobNo. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : ErasePartnerJobNo. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : ErasePartnerJobNo. " + ex.Message); }
            }

        }
        #endregion

        /// <summary>
        /// 2014-11-11 Zhou Kai
        /// Get all TO jobs whose customer_code is cusCode.
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetAllTransferOutJobNo()
        {
            // this query can be optimized to select only the transfer out job number only when whose
            // job has in-complete containers
            string sqlQuery = "SELECT JOB_NUMBER FROM TPT_JOB_MAIN_Tbl with (NOLOCK) " +
                "WHERE JOB_TRANSFER_TYPE = 3 AND JOB_STATUS = 1 ";
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            List<string> rtn = new List<string>();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            using (SqlCommand dbCmd = new SqlCommand())
            {
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbCmd.CommandTimeout = 0;
                    dbCmd.Connection = dbCon;
                    dbCmd.CommandType = CommandType.Text;
                    dbCmd.CommandText = sqlQuery;
                    SqlDataReader dbRdr = dbCmd.ExecuteReader();

                    while (dbRdr.Read())
                    {
                        rtn.Add(Convert.ToString(dbRdr[0]));
                    }

                }
                catch (FMException fme) { throw fme; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllTransferOutJobNo. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllTransferOutJobNo. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllTransferOutJobNo. " + ex.Message); }

                return rtn;
            }
        }

        /// <summary>
        /// 2014-11-19 Zhou Kai.
        /// Get all TO jobs whose customer_code is cusCode.
        /// </summary>
        /// <param name="cusCode"></param>
        /// <returns></returns>
        internal static List<string> GetAllTransferOutJobNo(string cusCode)
        {
            string sqlQuery = "SELECT JOB_NUMBER FROM TPT_JOB_MAIN_Tbl with (NOLOCK) " +
                "WHERE JOB_TRANSFER_TYPE = 3 AND JOB_STATUS = 1 AND " +
                " CUST_CODE = '{0}'";
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            sqlQuery = String.Format(sqlQuery, cusCode);
            List<string> rtn = new List<string>();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            using (SqlCommand dbCmd = new SqlCommand())
            {
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbCmd.CommandTimeout = 0;
                    dbCmd.Connection = dbCon;
                    dbCmd.CommandType = CommandType.Text;
                    dbCmd.CommandText = sqlQuery;
                    SqlDataReader dbRdr = dbCmd.ExecuteReader();

                    while (dbRdr.Read())
                    {
                        rtn.Add(Convert.ToString(dbRdr[0]));
                    }

                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetAllTransferOutJobNo. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetAllTransferOutJobNo. " + ex.Message); }
                catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetAllTransferOutJobNo. " + ex.Message); }

                return rtn;
            }

        }

        #region "2014-12-04 Zhou Kai"
        internal static List<HaulierJob> GetHaulierJobsByDate(DateTime startDate, DateTime endDate)
        {
            List<HaulierJob> rtn = new List<HaulierJob>();
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                SqlTransaction dbTran = null;
                try
                {
                    if (dbCon.State == ConnectionState.Closed) {dbCon.Open();}
                    dbTran = dbCon.BeginTransaction();
                    rtn = GetHaulierJobsByDate(startDate, endDate, dbCon, dbTran);
                    
                    dbTran.Commit();
                }
                catch (FMException fmEx) { if (dbTran != null) { dbTran.Rollback(); } throw fmEx; }
                catch (SqlException ex) { if (dbTran != null) { dbTran.Rollback(); } throw new FMException("HaulierJobDAL Error : GetHaulierJobsByDate. " + ex.Message); }
                catch (InvalidOperationException ex) { if (dbTran != null) { dbTran.Rollback(); } throw new FMException("HaulierJobDAL Error : GetHaulierJobsByDate. " + ex.Message); }
                catch (Exception ex) { if (dbTran != null) { dbTran.Rollback(); } throw new FMException("HaulierJobDAL Error : GetHaulierJobsByDate. " + ex.Message); }
            }

            return rtn;
        }

        internal static List<HaulierJob> GetHaulierJobsByDate(DateTime startDate, DateTime endDate,
            SqlConnection dbCon, SqlTransaction dbTran)
        {
            List<HaulierJob> rtn = new List<HaulierJob>();
            string strStartDate = DateUtility.ConvertDateForSQLPurpose(startDate);
            string strEndDate = DateUtility.ConvertDateForSQLPurpose(endDate) + 
                LAST_MINUTE_OF_DAY;
            string strSqlQuery = "SELECT * FROM TPT_JOB_MAIN_Tbl with (NOLOCK)  INNER JOIN " +
            " TPT_JOB_MAIN_SHIP_INFO_Tbl with (NOLOCK) ON TPT_JOB_MAIN_Tbl.JOB_ID = " +
            "  TPT_JOB_MAIN_SHIP_INFO_Tbl.JOB_ID AND " +
                "BOOKING_DATE >= '" + strStartDate + "' AND BOOKING_DATE <= '" +
                strEndDate + "'";
            try
            {
                using (SqlCommand dbCmd = new SqlCommand(strSqlQuery, dbCon))
                {
                    dbCmd.CommandTimeout = 0;
                    dbCmd.Transaction = dbTran;
                    SqlDataReader dbRdr = dbCmd.ExecuteReader();
                    while(dbRdr.Read())
                    {
                        rtn.Add(GetHaulierJob(dbRdr));
                    }
                    dbRdr.Close();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobsByDate. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobsByDate. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHaulierJobsByDate. " + ex.Message); }

            return rtn;
        }

#endregion
        
        internal static ContainerMovementInfo GetContainerMovementInfo(int jobId, int seqNo)
        {
            ContainerMovementInfo retValue = null;
            try
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string strSqlQuery = @"select * from TPT_PLAN_SUBTRIP_JOB_TBL pstj with (NOLOCK)
                                        inner join TPT_PLAN_SUBTRIP_TBL pst  with (NOLOCK)
                                        on pst.PLANTRIP_NO = pstj.PLANTRIP_NO and pst.SEQ_NO = pstj.SEQ_NO 
                                        inner join TPT_PLAN_TRIP_TBL pt with (NOLOCK)
                                        on pt.PLANTRIP_NO = pst.PLANTRIP_NO
                                        inner join TPT_Driver_Tbl drv with (NOLOCK)
                                        on drv.Driver_Code = pt.DRIVER_NO

                                        where pstj.JOB_ID = {0}
                                        and pstj.SEQUENCE_NO = {1}";

                strSqlQuery = string.Format(strSqlQuery, jobId, seqNo);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand dbCmd = new SqlCommand();
                dbCmd.CommandTimeout = 0;
                dbCmd.Connection = con;
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = strSqlQuery;
                SqlDataReader dbRdr = dbCmd.ExecuteReader();
                while (dbRdr.Read())
                {
                    retValue = new ContainerMovementInfo();
                    retValue.JobID = (int)dbRdr["JOB_ID"];
                    retValue.SeqNo = (int)dbRdr["SEQUENCE_NO"];
                    retValue.DriverName = (string)dbRdr["DRIVER_NAME"];
                    retValue.PrimeMover = (string)dbRdr["DEFAULT_VEHICLE"];
                    retValue.TrailerNo = (string)dbRdr["TRAILER_NO"];
                    retValue.ScheduleStarted = (DateTime)dbRdr["START_TIME"];
                    retValue.ScheduleFinished = (DateTime)dbRdr["END_TIME"];
                    return retValue;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetContainerMovementInfo. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetContainerMovementInfo. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetContainerMovementInfo. " + ex.Message); }
            return retValue;
        }
        //20160602 - gerry created method to get latest hubbing job based on customercode and container number
        internal static HaulierJob GetHubbingJob(string customerCode, string containerNo)
        {
            string sqlQuery = @"select TOP 1 * from TPT_JOB_MAIN_Tbl M with (NOLOCK)
                                inner join TPT_JOB_DETAIL_CARGO_Tbl D with (NOLOCK)
                                on M.JOB_ID = D.JOB_ID
                                inner join TPT_JOB_MAIN_SHIP_INFO_Tbl MSD with (NOLOCK)
                                on M.JOB_ID = MSD.JOB_ID
                                where M.CUST_CODE ='{0}'
                                and D.CONTAINER_NUMBER ='{1}'
                                order by M.Added_DateTime desc ";
            sqlQuery = string.Format(sqlQuery, customerCode, containerNo);
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                using (SqlCommand dbCmd = new SqlCommand())
                {
                    try
                    {
                        if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                        dbCmd.CommandTimeout = 0;
                        dbCmd.Connection = dbCon;
                        dbCmd.CommandType = CommandType.Text;
                        dbCmd.CommandText = sqlQuery;
                        SqlDataReader dbRdr = dbCmd.ExecuteReader();
                        while (dbRdr.Read())
                        {
                            return GetHaulierJob(dbRdr);
                        }
                    }
                    catch (FMException fme) { throw fme; }
                    catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHubbingJob. " + ex.Message); }
                    catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHubbingJob. " + ex.Message); }
                    catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHubbingJob. " + ex.Message); }
                    finally { dbCon.Close(); }
                    return null;
                }
            }
        }
        internal static string GetHubbingJobNo(string customerCode, string containerNo)
        {
            string hubbingJobNo = string.Empty;
            string sqlQuery = @"select TOP 1 M.JOB_NUMBER from TPT_JOB_MAIN_Tbl M with (NOLOCK)
                                inner join TPT_JOB_DETAIL_CARGO_Tbl D with (NOLOCK)
                                on M.JOB_ID = D.JOB_ID
                                inner join TPT_JOB_MAIN_SHIP_INFO_Tbl MSD with (NOLOCK)
                                on M.JOB_ID = MSD.JOB_ID
                                where M.JOB_TRANSFER_TYPE = 3
								AND M.CUST_CODE ='{0}'
                                and D.CONTAINER_NUMBER ='{1}'
                                order by M.JOB_ID desc"; // M.JOB_NUMBER, M.Added_DateTime desc "; //20160914 - gerry modified
            sqlQuery = string.Format(sqlQuery, customerCode, containerNo);
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                using (SqlCommand dbCmd = new SqlCommand())
                {
                    try
                    {
                        if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                        dbCmd.CommandTimeout = 0;
                        dbCmd.Connection = dbCon;
                        dbCmd.CommandType = CommandType.Text;
                        dbCmd.CommandText = sqlQuery;
                        SqlDataReader dbRdr = dbCmd.ExecuteReader();
                        while (dbRdr.Read())
                        {
                            hubbingJobNo = dbRdr["JOB_NUMBER"] == DBNull.Value ? String.Empty : dbRdr["JOB_NUMBER"].ToString();
                            return hubbingJobNo;
                        }
                    }
                    catch (FMException fme) { throw fme; }
                    catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : GetHubbingJobNo. " + ex.Message); }
                    catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : GetHubbingJobNo. " + ex.Message); }
                    catch (Exception ex) { throw new FMException("HaulierJobDAL Error : GetHubbingJobNo. " + ex.Message); }
                    finally { dbCon.Close(); }
                    return hubbingJobNo;
                }
            }
        }
        //20161017 - gerry added
        internal static bool UpdateProcessedReleasedESNForAllLegs(HaulierJobTrip haulierJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string SQLUpdateString = @"Update TPT_JOB_DETAIL_CARGO_Tbl set 
                                                Processed ='{0}',
                                                Released = '{1}' ,
                                                ESN = '{2}'
                                                WHERE JOB_ID = {3}
                                                and LEG_GROUP = {4}";
                SQLUpdateString = string.Format(SQLUpdateString,
                                                haulierJobTrip.Processed ? "T" : "F",
                                                haulierJobTrip.Released ? "T" : "F",
                                                haulierJobTrip.ESN ? "T" : "F",
                                                haulierJobTrip.JobID,
                                                haulierJobTrip.LegGroup);

                SqlCommand cmd = new SqlCommand(SQLUpdateString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForAllRemainingLeg. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForAllRemainingLeg. " + ex.Message); }
            catch (Exception ex) { throw new FMException("HaulierJobDAL Error : UpdateContainerSealNoForAllRemainingLeg. " + ex.Message); }

            return true;
        }

    }

}
 