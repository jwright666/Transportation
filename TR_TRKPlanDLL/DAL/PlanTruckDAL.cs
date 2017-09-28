using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_HLPlanDLL.BLL;
using FM.TR_HLBookDLL.BLL;
using FM.TR_TRKPlanDLL.BLL;
using TR_LanguageResource.Resources;
using System.Text.RegularExpressions;
using FM.TruckPlanDLL.BLL;

namespace FM.TR_TRKPlanDLL.DAL
{
    public class PlanTruckDAL
    {
        internal static SortableList<PlanTruckTrip> GetAllPlanTruckTripByDayAndDept(DateTime date, string tptDept)
        {
            SortableList<PlanTruckTrip> planTruckTrips = new SortableList<PlanTruckTrip>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TRK_PLAN_TRIP_TBL with (NOLOCK)  ";
            SQLString += " WHERE SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";//date.ToShortDateString();
            SQLString += " AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";//date.ToShortDateString();
            SQLString += " and DEPT='" + tptDept + "'";

            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckTrips.Add(GetPlanTruckTrip(reader));
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }
            return planTruckTrips;

        }

        internal static PlanTruckTrip GetPlanTruckTrip(string planTripNo)
        {
            PlanTruckTrip planTruckTrip = new PlanTruckTrip();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_PLAN_TRIP_TBL with (NOLOCK)  ";
                SQLString += " WHERE PLANTRIP_NO = '" + planTripNo + "'";


                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckTrip = GetPlanTruckTrip(reader);
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckTrip;
        }

        #region "2014-05-09 Zhou Kai adds function"
        internal static PlanTruckTrip GetPlanTruckTrip(string planTripNo, SqlConnection cn, SqlTransaction tran)
        {
            PlanTruckTrip planTruckTrip = new PlanTruckTrip();
            try
            {
                string SQLString = "SELECT * FROM TRK_PLAN_TRIP_TBL with (NOLOCK)  ";
                SQLString += " WHERE PLANTRIP_NO = '" + planTripNo + "'";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckTrip = GetPlanTruckTrip(reader);
                }
                reader.Close();
            }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return planTruckTrip;
        }
        #endregion

        internal static PlanTruckTrip GetAllPlanTruckTripByDayAndDept(DateTime date, string tptDept, SqlConnection con, SqlTransaction tran)
        {
            PlanTruckTrip planTruckTrip = new PlanTruckTrip();
            try
            {
                string SQLString = "SELECT * FROM TRK_PLAN_TRIP_TBL with (NOLOCK)  ";
                SQLString += " WHERE SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";//date.ToShortDateString();
                SQLString += " AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";//date.ToShortDateString();
                SQLString += " and DEPT='" + tptDept + "'";

                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckTrip = GetPlanTruckTrip(reader);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckTrip;

        }

        internal static PlanTruckTrip GetPlanTruckTrip(IDataReader reader)
        {
            //System.Globalization.CultureInfo american = new System.Globalization.CultureInfo("en-US");
            try
            {
                PlanTruckTrip planTruckTrip = new PlanTruckTrip(
                    (string)reader["DEPT"],
                   (string)reader["PLANTRIP_NO"],
                   (DateTime)reader["SCHEDULE_DATE"],
                   Vehicle.GetVehicle((string)reader["VEHICLE_NO"]),
                   Driver.GetDriver((string)reader["DRIVER_NO"]),
                   (byte[])reader["UPDATE_VERSION"]);

                planTruckTrip.pPlanTruckSubTrips = GetPlanTruckSubTrips(planTruckTrip);
                planTruckTrip.CopyPlanSubTripsToOldPlanSubTrips();//.CopyPlanTruckSubTripsToOldPlanHaulierSubTrips();
                //20161012 - gerry added
                //planTruckTrip.centerPoint = reader["GEO_CENTER_PLACE"] == DBNull.Value ? 0 : (double)reader["GEO_CENTER_PLACE"];
                //planTruckTrip.radius = reader["GEO_RADIUS"] == DBNull.Value ? 0 : (double)reader["GEO_RADIUS"];


                return planTruckTrip;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static PlanTruckSubTrips GetPlanTruckSubTrips(PlanTruckTrip planTruckTrip)
        {
            PlanTruckSubTrips planTruckSubTrips = new PlanTruckSubTrips();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_PLAN_SUB_TRIP_TBL with (NOLOCK)  ";
                SQLString += "WHERE PLANTRIP_NO = '" + planTruckTrip.PlanTripNo + "'";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckSubTrips.Add(GetPlanTruckSubTrip(reader, planTruckTrip));
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckSubTrips;
        }

        internal static PlanTruckSubTrip GetPlanTruckSubTrip(PlanTruckTrip planTruckTrip, int planTripSeq)
        {
            PlanTruckSubTrip planTruckSubTrip = new PlanTruckSubTrip();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_PLAN_SUB_TRIP_TBL with (NOLOCK)  ";
                SQLString += "WHERE PLANTRIP_NO = '" + planTruckTrip.PlanTripNo + "'";
                SQLString += " AND SEQ_NO = '" + planTripSeq + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckSubTrip = GetPlanTruckSubTrip(reader, planTruckTrip);
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckSubTrip;
        }

        internal static PlanTruckSubTrip GetPlanTruckSubTrip(PlanTruckTrip planTruckTrip, int planTripSeq, SqlConnection con, SqlTransaction tran)
        {
            PlanTruckSubTrip planTruckSubTrip = new PlanTruckSubTrip();
            try
            {
                string SQLString = "SELECT * FROM TRK_PLAN_SUB_TRIP_TBL with (NOLOCK)  ";
                SQLString += "WHERE PLANTRIP_NO = '" + planTruckTrip.PlanTripNo + "'";
                SQLString += " AND SEQ_NO = '" + planTripSeq + "'";
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckSubTrip = GetPlanTruckSubTrip(reader, planTruckTrip, con, tran);
                }
                reader.Close();
                planTruckSubTrip.planTruckSubTripJobs = GetPlanTruckSubTripJobs(planTruckTrip.PlanTripNo, planTruckSubTrip, con, tran);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckSubTrip;
        }
  
        internal static PlanTruckSubTrip GetPlanTruckSubTrip(IDataReader reader, PlanTruckTrip planTruckTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                bool isbillable = false;
                string temp = (string)reader["IS_BILLABLE"];
                if (temp == "T")
                { isbillable = true; }
                else { isbillable = false; }

                SortableList<PlanTruckSubTripJob> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();

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


                PlanTruckSubTrip planTruckSubTrip = new PlanTruckSubTrip(
                   (int)reader["SEQ_NO"],
                   (DateTime)reader["START_TIME"],
                   (DateTime)reader["END_TIME"],
                   Stop.GetStop((string)reader["START_STOP"]),
                   Stop.GetStop((string)reader["END_STOP"]),
                   isbillable,
                   (string)reader["DESCRIPTION"],
                   planTruckSubTripJobs,
                   planTruckTrip,
                   jobtripstatus,
                   (byte[])reader["UPDATE_VERSION"]
                   );

                planTruckSubTrip.PlanTripNo = reader["PLANTRIP_NO"] == DBNull.Value ? string.Empty : (string)reader["PLANTRIP_NO"];
                //20160825 -gerry added
                planTruckSubTrip.shipment_id = reader["SHIPMENT_ID"] == DBNull.Value ? string.Empty : (string)reader["SHIPMENT_ID"];
                //2016090 -gerry added
                planTruckSubTrip.route_point_id = reader["CT_LOCATION_POINT_ID"] == DBNull.Value ? string.Empty : (Convert.ToInt32(reader["CT_LOCATION_POINT_ID"])).ToString();
                planTruckSubTrip.Task_ID = reader["CT_Task_ID"] == DBNull.Value ? string.Empty : (Convert.ToInt32(reader["CT_Task_ID"])).ToString();

                return planTruckSubTrip;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static PlanTruckSubTrip GetPlanTruckSubTrip(IDataReader reader, PlanTruckTrip planTruckTrip)
        {
            try
            {
                bool isbillable = false;
                string temp = (string)reader["IS_BILLABLE"];
                if (temp == "T")
                { isbillable = true; }
                else { isbillable = false; }

                SortableList<PlanTruckSubTripJob> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();

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

                PlanTruckSubTrip planTruckSubTrip = new PlanTruckSubTrip(
                   (int)reader["SEQ_NO"],
                   (DateTime)reader["START_TIME"],
                   (DateTime)reader["END_TIME"],
                   Stop.GetStop((string)reader["START_STOP"]),
                   Stop.GetStop((string)reader["END_STOP"]),
                   isbillable,
                   (string)reader["DESCRIPTION"],
                   planTruckSubTripJobs,
                   planTruckTrip,
                   jobtripstatus,
                   (byte[])reader["UPDATE_VERSION"]
                   );

                planTruckSubTrip.PlanTripNo = reader["PLANTRIP_NO"] == DBNull.Value ? string.Empty : (string)reader["PLANTRIP_NO"];
                //20160825 -gerry added
                planTruckSubTrip.shipment_id = reader["SHIPMENT_ID"] == DBNull.Value ? string.Empty : (string)reader["SHIPMENT_ID"];
                //2016090 -gerry added
                planTruckSubTrip.route_point_id = reader["CT_LOCATION_POINT_ID"] == DBNull.Value ? string.Empty : (Convert.ToInt32(reader["CT_LOCATION_POINT_ID"])).ToString();
                planTruckSubTrip.Task_ID = reader["CT_Task_ID"] == DBNull.Value ? string.Empty : (Convert.ToInt32(reader["CT_Task_ID"])).ToString();
                //20161228
                planTruckSubTrip.travelTimeFromVehicleToPickup = reader["TRAVEL_TIME_FROM_VEHICLE_LOC_TO_PICKUP"] == DBNull.Value ? 0 : Convert.ToDouble(reader["TRAVEL_TIME_FROM_VEHICLE_LOC_TO_PICKUP"]);
                planTruckSubTrip.loadingTime = reader["LOADING_TIME"] == DBNull.Value ? 0 : Convert.ToDouble(reader["LOADING_TIME"]);
                planTruckSubTrip.travelTimeFromPickupToDelivery = reader["TRAVEL_TIME_FROM_PICKUP_TO_DELIVERY"] == DBNull.Value ? 0 : Convert.ToDouble(reader["TRAVEL_TIME_FROM_PICKUP_TO_DELIVERY"]);
                planTruckSubTrip.unloadingTime = reader["UNLOADING"] == DBNull.Value ? 0 : Convert.ToDouble(reader["UNLOADING"]);

                planTruckSubTrip.planTruckSubTripJobs = GetPlanTruckSubTripJobsForGridDisplay(planTruckTrip.PlanTripNo, planTruckSubTrip);
                return planTruckSubTrip;
            }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        /*
         * 2013-09-10 Zhou Kai adds comments:
         * This method is to get planTruckSubTripJobs from database,
         * to bind to dgvTrcukJob on FrmPlanTruckSubTrip for displan 
         * and modification by user manually, such as weight/volume,
         * start/end time
         */
        internal static SortableList<PlanTruckSubTripJob> GetPlanTruckSubTripJobsForGridDisplay(string planTripNo,
            PlanTruckSubTrip planTruckSubTrip)
        {
            SortableList<PlanTruckSubTripJob> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());

            //20121017 - Gerry Replaced the query
            #region REmoved OLD query

            string SQLString = @"select * from TRK_PLAN_SUB_TRIP_JOB_TBL with (NOLOCK)  
                                inner join TRK_JOB_MAIN_Tbl with (NOLOCK)  on TRK_PLAN_SUB_TRIP_JOB_TBL.job_id = TRK_JOB_MAIN_Tbl.job_id 
                                inner join TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  on TRK_PLAN_SUB_TRIP_JOB_TBL.job_id = TRK_JOB_DETAIL_TRIP_Tbl.job_id 
		                                and TRK_PLAN_SUB_TRIP_JOB_TBL.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO 
                                inner join TRK_PLAN_SUB_TRIP_TBL with (NOLOCK)  on TRK_PLAN_SUB_TRIP_TBL.PLANTRIP_NO = TRK_PLAN_SUB_TRIP_JOB_TBL.PLANTRIP_NO
			                            and TRK_PLAN_SUB_TRIP_TBL.SEQ_NO = TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIP_SEQNO
                                left join TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  on TRK_PLAN_SUB_TRIP_JOB_TBL.job_id = TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOB_ID 
                                        and TRK_PLAN_SUB_TRIP_JOB_TBL.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOBTRIP_SEQ_NO 
                                        and TRK_PLAN_SUB_TRIP_JOB_TBL.PLANTRIP_NO = TRK_JOB_DETAIL_TRIP_PLAN_TBL.PLANTRIP_NO 
		                                and TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIP_SEQNO = TRK_JOB_DETAIL_TRIP_PLAN_TBL.PLANSUBTRIP_SEQNO
                                        and TRK_PLAN_SUB_TRIP_JOB_TBL.VOLUME = TRK_JOB_DETAIL_TRIP_PLAN_TBL.VOLUME
                                        and TRK_PLAN_SUB_TRIP_JOB_TBL.WEIGHT = TRK_JOB_DETAIL_TRIP_PLAN_TBL.WEIGHT ";

            SQLString += " WHERE TRK_PLAN_SUB_TRIP_JOB_TBL.PLANTRIP_NO = '" + planTripNo + "'";
            SQLString += " AND TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIP_SEQNO = " + planTruckSubTrip.SeqNo;
            //20161206 - gerry added
            SQLString += " ORDER BY TRK_PLAN_SUB_TRIP_JOB_TBL.PLANTRIP_NO, TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIP_SEQNO, TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIPJOB_SEQNO";

            #endregion
            //end 20121017
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PlanTruckSubTripJob planTruckSubTripJob = GetPlanTruckSubTripJob(reader);
                    planTruckSubTripJob.planTruckSubTripJobDetails = GetPlanTruckSubTripJobDetails(planTruckSubTripJob);
                    planTruckSubTripJobs.Add(planTruckSubTripJob);
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckSubTripJobs;
        }

        internal static SortableList<PlanTruckSubTripJob> GetPlanTruckSubTripJobs(string planTripNo,
                                                                                                                              PlanTruckSubTrip planTruckSubTrip,
                                                                                                                              SqlConnection con,
                                                                                                                             SqlTransaction tran)
        {
            SortableList<PlanTruckSubTripJob> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJob>();
            try
            {
                string SQLString = "select * from TRK_PLAN_SUB_TRIP_JOB_TBL with (NOLOCK)  inner join TRK_JOB_MAIN_Tbl with (NOLOCK)  ";
                SQLString += "on TRK_PLAN_SUB_TRIP_JOB_TBL.job_id = TRK_JOB_MAIN_Tbl.job_id ";
                SQLString += "inner join TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  ";
                SQLString += "on TRK_PLAN_SUB_TRIP_JOB_TBL.job_id = TRK_JOB_DETAIL_TRIP_Tbl.job_id ";
                SQLString += "and TRK_PLAN_SUB_TRIP_JOB_TBL.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO ";
                SQLString += "WHERE TRK_PLAN_SUB_TRIP_JOB_TBL.PLANTRIP_NO = '" + planTripNo + "'";
                SQLString += " AND PLANSUBTRIP_SEQNO = " + planTruckSubTrip.SeqNo;
                //20161206 - gerry added
                SQLString += " ORDER BY TRK_PLAN_SUB_TRIP_JOB_TBL.PLANTRIP_NO, TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIP_SEQNO, TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIPJOB_SEQNO";
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();

                // 2014-03-27 Zhou Kai adds a datatable to store the results and close the outter reader first
                DataTable dtPlanTruckSubTripJob = new DataTable();
                dtPlanTruckSubTripJob.Columns.Add("PLANTRIP_NO");
                dtPlanTruckSubTripJob.Columns.Add("SEQ_NO");
                dtPlanTruckSubTripJob.Columns.Add("JOB_ID");
                dtPlanTruckSubTripJob.Columns.Add("JOB_SEQ_NO");
                dtPlanTruckSubTripJob.Columns.Add("JOBTRIP_PLAN_SEQ");

                while (reader.Read())
                {
                    //planTruckSubTripJobs.Add(GetPlanTruckSubTripJob(reader));
                    dtPlanTruckSubTripJob.Rows.Add((string)reader["PLANTRIP_NO"],
                                                    (int)reader["PLANSUBTRIP_SEQNO"],//20160202 - gerry changed
                                                    (int)reader["JOB_ID"],
                                                    (int)reader["JOBTRIP_SEQ_NO"],//20160202 - gerry changed
                                                    (int)reader["PLANSUBTRIPJOB_SEQNO"]);//20160202 - gerry changed
                }
                reader.Close();
                foreach (DataRow dr in dtPlanTruckSubTripJob.Rows)
                {
                    planTruckSubTripJobs.Add(GetPlanTruckSubTripJob(dr["PLANTRIP_NO"].ToString(),
                                                                                                      Convert.ToInt32(dr["SEQ_NO"]),
                                                                                                      Convert.ToInt32(dr["JOB_ID"]),
                                                                                                      Convert.ToInt32(dr["JOB_SEQ_NO"]),
                                                                                                      Convert.ToInt32(dr["JOBTRIP_PLAN_SEQ"]),
                                                                                                      con, tran));

                }

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckSubTripJobs;
        }

        internal static PlanTruckSubTripJob GetPlanTruckSubTripJob(string planTripNo,
                                                                                                     int planTripSeq,
                                                                                                     int jobID,
                                                                                                     int jobSeq,
                                                                                                     int jobTripPlanSeq,
                                                                                                     SqlConnection cn,
                                                                                                     SqlTransaction tran)
        {
            PlanTruckSubTripJob planTruckSubTripJob = new PlanTruckSubTripJob();
            try
            {
                string SQLString = "select * from TRK_PLAN_SUB_TRIP_JOB_TBL with (NOLOCK)  ";
                SQLString += "INNER JOIN TRK_JOB_MAIN_Tbl with (NOLOCK)  ";
                SQLString += "ON TRK_PLAN_SUB_TRIP_JOB_TBL.JOB_ID=TRK_JOB_MAIN_Tbl.JOB_ID ";
                SQLString += "INNER JOIN TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  ";
                SQLString += "ON TRK_PLAN_SUB_TRIP_JOB_TBL.JOB_ID=TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID ";
                SQLString += "AND TRK_PLAN_SUB_TRIP_JOB_TBL.JOBTRIP_SEQ_NO=TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO ";
                ////
                SQLString += "inner join TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  ";
                SQLString += "on TRK_PLAN_SUB_TRIP_JOB_TBL.job_id = TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOB_ID ";
                SQLString += "and TRK_PLAN_SUB_TRIP_JOB_TBL.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOBTRIP_SEQ_NO ";
                SQLString += "and TRK_PLAN_SUB_TRIP_JOB_TBL.VOLUME = TRK_JOB_DETAIL_TRIP_PLAN_TBL.VOLUME ";
                SQLString += "and TRK_PLAN_SUB_TRIP_JOB_TBL.WEIGHT = TRK_JOB_DETAIL_TRIP_PLAN_TBL.WEIGHT ";
                ////
                SQLString += "WHERE TRK_PLAN_SUB_TRIP_JOB_TBL.PLANTRIP_NO = '" + planTripNo + "'";
                SQLString += " AND TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIP_SEQNO = " + planTripSeq;
                SQLString += " AND TRK_PLAN_SUB_TRIP_JOB_TBL.JOB_ID = " + jobID;
                SQLString += " AND TRK_PLAN_SUB_TRIP_JOB_TBL.JOBTRIP_SEQ_NO = " + jobSeq;
                SQLString += " AND TRK_PLAN_SUB_TRIP_JOB_TBL.PLANSUBTRIPJOB_SEQNO = " + jobTripPlanSeq;

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckSubTripJob = GetPlanTruckSubTripJob(reader);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckSubTripJob;
        }

        internal static PlanTruckSubTripJob GetPlanTruckSubTripJob(IDataReader reader)
        {
            try
            {
                // 2013-11-13 Zhou Kai fixes the bug, modify the reader["Start_Time"] --> reader["FROM_DATE"]
                // reader["End_Time"] --> reader["TO_DATE"]
                //DateTime startDate = ((DateTime)reader["FROM_DATE"]).Date;
                //DateTime endDate = ((DateTime)reader["TO_DATE"]).Date;
                //string startTime = ((String)reader["START_TIME"]);
                //string endTime = ((String)reader["END_TIME"]);
                // 2013-11-13 Zhou Kai ends


                //20131206 - Gerry Replaced above code
                DateTime startDate = ((DateTime)reader["START_TIME"]).Date;
                DateTime endDate = ((DateTime)reader["END_TIME"]).Date;
                string startTime = ((DateTime)reader["START_TIME"]).ToString("HHmm");
                string endTime = ((DateTime)reader["END_TIME"]).ToString("HHmm");
                //20131206 


                Stop startStop = new Stop(
                    (string)reader["FROM_STOP"],
                    "",
                    (string)reader["FROM_ADD1"],
                    (string)reader["FROM_ADD2"],
                    (string)reader["FROM_ADD3"],
                    (string)reader["FROM_ADD4"],
                    "Singapore");

                Stop endStop = new Stop(
                    (string)reader["TO_STOP"],
                    "",
                    (string)reader["TO_ADD1"],
                    (string)reader["TO_ADD2"],
                    (string)reader["TO_ADD3"],
                    (string)reader["TO_ADD4"],
                    "Singapore");

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

                PlanTruckSubTripJob planTruckSubTripJob = new PlanTruckSubTripJob(
                   (string)reader["PLANTRIP_NO"],
                   (int)reader["PLANSUBTRIP_SEQNO"],
                   (int)reader["JOB_ID"],
                   (int)reader["JOBTRIP_SEQ_NO"],
                   (string)reader["CUST_CODE"],
                   "",
                   startStop,
                   endStop,
                   startDate,//(DateTime)reader["FROM_DATE"],
                   endDate,//(DateTime)reader["TO_DATE"],
                   startTime,//(string)reader["FROM_TIME"],
                   endTime,//(string)reader["TO_TIME"],
                   (decimal)reader["VOLUME"],
                   (decimal)reader["WEIGHT"],
                   (byte[])reader["UPDATE_VERSION"],
                   jobtripstatus,
                   (int)reader["PLANSUBTRIPJOB_SEQNO"]
                   );

                planTruckSubTripJob.custName = CustomerDTO.GetCustomerNameByCustomerCode(planTruckSubTripJob.custCode);
                //20160920 - gerry added
                planTruckSubTripJob.cloudTrack_JobId = reader["CT_Task_ID"] == DBNull.Value ? string.Empty : (Convert.ToInt32(reader["CT_Task_ID"])).ToString();
                return planTruckSubTripJob;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        public static bool DeleteAllPlanTruckTripDatabase(PlanTruckTrip planTruckTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT * FROM TRK_PLAN_TRIP_TBL with (NOLOCK)  ";
                SQLString += "WHERE PLANTRIP_NO = '" + planTruckTrip.PlanTripNo + "'";

                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                if (SqlBinary.Equals(planTruckTrip.UpdateVersion, originalRowVersion) == false)
                {
                    retValue = false;
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nPlanTruckDAL.DeleteAllPlanTruckTripDatabase");
                }

                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_PLAN", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTruckTrip.PlanTripNo);

                SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                if (Convert.ToInt32(newReqNumber2.Value) > 1)
                {
                    throw new FMException("Error - More than 1 record deleted");
                }
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        public static bool ChangeDriver(PlanTruckTrip planTruckTrip, Driver newDriver, SqlConnection cn, SqlTransaction tran, out PlanTruckTrip outPlanTruckTrip)
        {
            bool changed = false;
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT * FROM TRK_PLAN_TRIP_TBL with (NOLOCK)  ";
                SQLString += "WHERE PLANTRIP_NO = '" + planTruckTrip.PlanTripNo + "'";

                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                if (!SqlBinary.Equals(planTruckTrip.UpdateVersion, originalRowVersion))
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nPlanTruckDAL.ChangeDriver");

                }

                // 2014-05-15 Zhou Kai adds logic:
                // when the new driver already has his / her planTruckTrip,
                // need to update the driver of the planTruckTrip to the 
                // driver with whom the new driver transfers his planSubTrips to
                SQLString = "SELECT [PLANTRIP_NO] FROM [dbo].[TRK_PLAN_TRIP_TBL] with (NOLOCK)  WHERE [DRIVER_NO] = '{0}' AND " +
                    "[SCHEDULE_DATE] = '{1}'";
                string strDate = planTruckTrip.ScheduleDate.ToString("yyyy-MM-dd");
                SQLString = String.Format(SQLString, newDriver.Code, strDate);
                SqlCommand cmd = new SqlCommand("sp_Edit_TRK_CHANGE_DRIVER_AND_VEHICLE_PLAN", cn);
                cmd.Transaction = tran;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = SQLString;
                if (object.ReferenceEquals(cmd.ExecuteScalar(), DBNull.Value))
                {
                    // do nothing
                }
                else
                {
                    // perform update
                    string orignalPlanTripNo = Convert.ToString(cmd.ExecuteScalar());
                    cmd = new SqlCommand("sp_Edit_TRK_CHANGE_DRIVER_AND_VEHICLE_PLAN", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = tran;

                    cmd.Parameters.AddWithValue("@PLANTRIP_NO", orignalPlanTripNo);
                    cmd.Parameters.AddWithValue("@DRIVER_NO", planTruckTrip.Driver.Code);
                    cmd.Parameters.AddWithValue("@VEHICLE_NO", planTruckTrip.Driver.defaultVehicleNumber);
                    if (cmd.ExecuteNonQuery() != 1) { }
                    //throw new FMException(CommonResource.AffactedRowCountDoesNotMatchExpected);

                }
                // 2014-05-15 Zhou Kai ends

                //SqlCommand cmd = new SqlCommand("sp_CHANGE_DRIVER_AND_VEHICLE_TRK_PLAN", cn);
                cmd = new SqlCommand("sp_Edit_TRK_CHANGE_DRIVER_AND_VEHICLE_PLAN", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTruckTrip.PlanTripNo);
                cmd.Parameters.AddWithValue("@DRIVER_NO", newDriver.Code);
                cmd.Parameters.AddWithValue("@VEHICLE_NO", newDriver.defaultVehicleNumber);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                // outPlanTruckTrip = GetPlanTruckTrip(planTruckTrip.PlanTripNo);
                // 2014-05-09 Zhou Kai uses this function instead
                outPlanTruckTrip = GetPlanTruckTrip(planTruckTrip.PlanTripNo, cn, tran);
                // 2014-05-09 Zhou Kai ends

                changed = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return changed;
        }

        public static bool AddPlanTruckTripDatabase(PlanTruckTrip planTruckTrip, SqlConnection cn, SqlTransaction tran, out PlanTruckTrip outPlanTruckTrip)
        {
            bool retValue = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_PLAN_TRIP_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                string templanno = "";
                string year = planTruckTrip.ScheduleDate.Year.ToString();
                string month = planTruckTrip.ScheduleDate.Month.ToString();
                string day = planTruckTrip.ScheduleDate.Day.ToString();

                year = year[2].ToString() + year[3].ToString();
                if (month.Length == 1)
                    month = "0" + month;
                if (day.Length == 1)
                    day = "0" + day;
                templanno = TruckJob.GetPrefix() + year + month + day + planTruckTrip.dept;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", templanno);
                cmd.Parameters.AddWithValue("@SCHEDULE_DATE", DateUtility.ConvertDateForSQLPurpose(planTruckTrip.ScheduleDate.Date));
                cmd.Parameters.AddWithValue("@DRIVER_NO", planTruckTrip.Driver.Code);
                cmd.Parameters.AddWithValue("@VEHICLE_NO", planTruckTrip.Vehicle.Number);
                cmd.Parameters.AddWithValue("@DEPT", planTruckTrip.dept);

                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.NVarChar);
                newReqNumber.Size = 256;
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                planTruckTrip.PlanTripNo = newReqNumber.Value.ToString();

                for (int i = 0; i < planTruckTrip.pPlanTruckSubTrips.Count; i++)
                {
                    PlanTruckSubTrip outPlanTruckSubTrip = new PlanTruckSubTrip();
                    AddPlanTruckSubTripDatabase(planTruckTrip, planTruckTrip.pPlanTruckSubTrips[i], cn, tran, out outPlanTruckSubTrip);
                }

                outPlanTruckTrip = GetAllPlanTruckTripByDayAndDept(planTruckTrip.ScheduleDate, planTruckTrip.dept, cn, tran);

                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        //27 Jan 2012 - Gerry Start add - will get the next planTruckSubTripSeqNo
        public static Int32 GetNextPlanTruckSubTripSeqNo(string planTruckTripNo, SqlConnection cn, SqlTransaction tran)
        {
            int planTruckSubTripSeq_No = 0;
            string SQLString = @"select ISNULL(max(seq_no), 0) as LAST_SEQ_NO from TRK_PLAN_SUB_TRIP_TBL with (NOLOCK) 
                                            where PLANTRIP_NO = '{0}'";

            SQLString = string.Format(SQLString, planTruckTripNo);
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    planTruckSubTripSeq_No = (int)reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckSubTripSeq_No + 1;
        }
        public static Int32 GetNextPlanTruckSubTripSeqNo(string planTruckTripNo)
        {
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                int planTruckSubTripSeq_No = 0;
                string SQLString = @"select ISNULL(max(seq_no), 0) as LAST_SEQ_NO from TRK_PLAN_SUB_TRIP_TBL with (NOLOCK) 
                                            where PLANTRIP_NO = '{0}'";

                SQLString = string.Format(SQLString, planTruckTripNo);
                try
                {
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        planTruckSubTripSeq_No = (int)reader.GetInt32(0);
                    }
                    reader.Close();
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                return planTruckSubTripSeq_No + 1;
            }
        }

        public static bool AddPlanTruckSubTripDatabase(PlanTruckTrip planTruckTrip, PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran, out PlanTruckSubTrip outPlanTruckSubTrip,
            string userID = "", string frmName = "", bool isNextPlanSubTrip = false)
        {
            bool retValue = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_PLAN_SUBTRIP_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTruckTrip.PlanTripNo);
                cmd.Parameters.AddWithValue("@SEQ_NO", planTruckSubTrip.SeqNo);
                cmd.Parameters.AddWithValue("@START_TIME", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planTruckSubTrip.Start));
                cmd.Parameters.AddWithValue("@END_TIME", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planTruckSubTrip.End));
                cmd.Parameters.AddWithValue("@START_STOP", planTruckSubTrip.StartStop.Code);
                cmd.Parameters.AddWithValue("@END_STOP", planTruckSubTrip.EndStop.Code);
                if (planTruckSubTrip.IsBillableTrip == true)
                    cmd.Parameters.AddWithValue("@IS_BILLABLE", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_BILLABLE", "F");
                cmd.Parameters.AddWithValue("@DESCRIPTION", planTruckSubTrip.Description);
                cmd.Parameters.AddWithValue("@STATUS", planTruckSubTrip.Status);
                //20161228
                cmd.Parameters.AddWithValue("@TRAVELTIME_FROMVEHICLE_TOPICKUP", planTruckSubTrip.travelTimeFromVehicleToPickup);
                cmd.Parameters.AddWithValue("@LOADINGTIME", planTruckSubTrip.loadingTime);
                cmd.Parameters.AddWithValue("@TRAVELTIME_FROMPICKUP_TODELIVERY", planTruckSubTrip.travelTimeFromPickupToDelivery);
                cmd.Parameters.AddWithValue("@UNLOADINGTIME", planTruckSubTrip.unloadingTime);
                //20161228 end
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                for (int i = 0; i < planTruckSubTrip.planTruckSubTripJobs.Count; i++)
                {
                    PlanTruckSubTripJob outPlanTruckSubTripJob = new PlanTruckSubTripJob();
                    planTruckSubTrip.planTruckSubTripJobs[i].planSubTripSeqNo = planTruckSubTrip.SeqNo;
                    AddPlanTruckSubTripJobDatabase(planTruckTrip, planTruckSubTrip, planTruckSubTrip.planTruckSubTripJobs[i], cn, tran, out outPlanTruckSubTripJob, userID, frmName, isNextPlanSubTrip);
                }
                outPlanTruckSubTrip = new PlanTruckSubTrip();
                outPlanTruckSubTrip = GetPlanTruckSubTrip(planTruckTrip, planTruckSubTrip.SeqNo, cn, tran);
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        public static bool AddPlanTruckSubTripJobDatabase(PlanTruckTrip planTruckTrip, PlanTruckSubTrip planTruckSubTrip, PlanTruckSubTripJob planTruckSubTripJob, SqlConnection cn, SqlTransaction tran, out PlanTruckSubTripJob outPlanTruckSubTripJob,
            string userID = "", string frmName = "", bool isNextPlanSubTrip = false)
        {
            bool retValue = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_PLAN_SUB_TRIP_JOB_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTruckTrip.PlanTripNo);
                cmd.Parameters.AddWithValue("@PLANSUBTRIP_SEQNO", planTruckSubTripJob.planSubTripSeqNo);
                cmd.Parameters.AddWithValue("@JOB_ID", planTruckSubTripJob.jobID);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQNO", planTruckSubTripJob.jobSeq);
                cmd.Parameters.AddWithValue("@VOLUME", planTruckSubTripJob.volume);
                cmd.Parameters.AddWithValue("@WEIGHT", planTruckSubTripJob.weight);

                //cmd.Parameters.AddWithValue("@ACTUAL_START_TIME", DateUtility.ConvertDateAndTimeForSQLPurpose(planTruckSubTrip.Start));//gerry added 20160223
                //cmd.Parameters.AddWithValue("@ACTUAL_END_TIME", DateUtility.ConvertDateAndTimeForSQLPurpose(planTruckSubTrip.End));//gerry added 20160223
                if (planTruckSubTripJob.status == JobTripStatus.Booked)
                    cmd.Parameters.AddWithValue("@STATUS", 1);
                if (planTruckSubTripJob.status == JobTripStatus.Ready)
                    cmd.Parameters.AddWithValue("@STATUS", 2);
                if (planTruckSubTripJob.status == JobTripStatus.Assigned)
                    cmd.Parameters.AddWithValue("@STATUS", 3);
                if (planTruckSubTripJob.status == JobTripStatus.Completed)
                    cmd.Parameters.AddWithValue("@STATUS", 4);
                if (planTruckSubTripJob.status == JobTripStatus.Invoiced)
                    cmd.Parameters.AddWithValue("@STATUS", 5);

                SqlParameter newReqNumber = new SqlParameter("@PLANSUBTRIPJOB_SEQNO", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                planTruckSubTripJob.planSubTripJobSeqNo = Convert.ToInt32(newReqNumber.Value);

                //20160201 - gerry added
                //if (planTruckSubTrip.StartStop.Code.Trim() == planTruckSubTripJob.startStop.Code.Trim())
                //{
                //add PlanTruckSubTripJobDetail
                if (planTruckSubTripJob.planTruckSubTripJobDetails != null)
                {
                    foreach (PlanTruckSubTripJobDetail planTruckSubTripJobDetail in planTruckSubTripJob.planTruckSubTripJobDetails)
                    {
                        planTruckSubTripJobDetail.planTripNo = planTruckSubTripJob.planTripNo;
                        planTruckSubTripJobDetail.planSubTripSeqNo = planTruckSubTripJob.planSubTripSeqNo;
                        planTruckSubTripJobDetail.planSubTripJobSeqNo = planTruckSubTripJob.planSubTripJobSeqNo;
                        if (planTruckSubTripJobDetail.qty > 0)
                            planTruckSubTripJobDetail.AddPlanTruckSubTripJobDetail(cn, tran);
                    }
                }
                //}
                //job trip to be updated
                TruckJobTrip jobTrip = null;
                if (!isNextPlanSubTrip)
                    jobTrip = TruckJobTrip.GetTruckJobTrip(planTruckSubTripJob.jobID, planTruckSubTripJob.jobSeq, cn, tran);
                else
                {
                    jobTrip = new TruckJobTrip()
                    {
                        JobID = planTruckSubTripJob.jobID,
                        Sequence = planTruckSubTripJob.jobSeq
                    };
                    jobTrip.JobTripStates = TruckJobTrip.GetAllTruckJobTripStates(jobTrip, cn, tran);
                    jobTrip.TripStatus = jobTrip.JobTripStates[jobTrip.JobTripStates.Count - 1].Status;
                    jobTrip.UpdateVersion = jobTrip.GetTruckJobTripUpdatedVersion(cn, tran);
                }
                //add truckJobTripPlan
                if (planTruckSubTrip.StartStop.Code == planTruckSubTripJob.startStop.Code)
                {
                    //add TruckJobTripPlan
                    TruckJobTripPlan truckJobTripPlan = planTruckTrip.CreateTruckJobTripPlan(planTruckSubTripJob, planTruckSubTrip.Start, planTruckSubTrip.End);
                    truckJobTripPlan.planTripNo = planTruckTrip.PlanTripNo;
                    truckJobTripPlan.planSubTripSeqNo = planTruckSubTripJob.planSubTripSeqNo;
                    truckJobTripPlan.planSubTripJobSeqNo = planTruckSubTripJob.planSubTripJobSeqNo;
                    TruckJobTripPlan outTruckJobTripPlan = new TruckJobTripPlan();
                    jobTrip.AddTruckJobTripPlan(truckJobTripPlan, cn, tran, out outTruckJobTripPlan, userID, frmName);
                    TruckJobTrip.UpdateAcutalWeightVolumeforTruckJobTrip(jobTrip.JobID, jobTrip.Sequence, cn, tran);
                }

                //get output
                outPlanTruckSubTripJob = GetPlanTruckSubTripJob(planTruckTrip.PlanTripNo,
                                            planTruckSubTripJob.planSubTripSeqNo,
                                            planTruckSubTripJob.jobID,
                                            planTruckSubTripJob.jobSeq,
                                            planTruckSubTripJob.planSubTripJobSeqNo, cn, tran);
                outPlanTruckSubTripJob.planTruckSubTripJobDetails = GetPlanTruckSubTripJobDetails(outPlanTruckSubTripJob, cn, tran);
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue; ;
        }

        public static bool DeletePlanTruckSubTripForTransferDriverDatabase(PlanTruckTrip planTruckTrip, PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_JOB_TRIP_PLAN_AND_PLAN_SUBTRIP_JOB", cn);
                cmd.CommandTimeout = 0;
                for (int i = 0; i < planTruckSubTrip.planTruckSubTripJobs.Count; i++)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTruckSubTrip.planTruckSubTripJobs[i].planTripNo);
                    cmd.Parameters.AddWithValue("@PLANSUBTRIP_SEQNO", planTruckSubTrip.planTruckSubTripJobs[i].planSubTripSeqNo);
                    cmd.Parameters.AddWithValue("@PLANSUBTRIPJOB_SEQNO", planTruckSubTrip.planTruckSubTripJobs[i].planSubTripJobSeqNo);
                    cmd.Parameters.AddWithValue("@JOB_ID", planTruckSubTrip.planTruckSubTripJobs[i].jobID);
                    cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", planTruckSubTrip.planTruckSubTripJobs[i].jobSeq);

                    SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                    cmd.Parameters.Add(newReqNumber2);
                    newReqNumber2.Direction = ParameterDirection.Output;

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    if (Convert.ToInt32(newReqNumber2.Value) == 0)
                    {
                        throw new FMException(TptResourceDAL.ErrNoRecordDeleted);
                    }
                }

                cmd = new SqlCommand("sp_Delete_TRK_PLAN_SUB_TRIP_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTruckTrip.PlanTripNo);
                cmd.Parameters.AddWithValue("@SEQ_NO", planTruckSubTrip.SeqNo);

                SqlParameter newReqNumber = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                if (Convert.ToInt32(newReqNumber.Value) == 0)
                {
                    throw new FMException(TptResourceDAL.ErrNoRecordDeleted);
                }
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        public static bool DeletePlanTruckSubTrip(PlanTruckTrip planTruckTrip, PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_PLAN_SUB_TRIP_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTruckTrip.PlanTripNo);
                cmd.Parameters.AddWithValue("@SEQ_NO", planTruckSubTrip.SeqNo);


                SqlParameter newReqNumber = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;


                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                if (Convert.ToInt32(newReqNumber.Value) == 0)
                {
                    throw new FMException(TptResourceDAL.ErrNoRecordDeleted);
                }
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue; ;
        }

        public static bool DeletePlanTruckSubTripJobDatabase(PlanTruckSubTrip planTruckSubTrip, PlanTruckSubTripJob planTruckSubTripJob, SqlConnection cn, SqlTransaction tran, out PlanTruckSubTrip outPlanTruckSubTrip)
        {
            bool retValue = false;
            try
            {
                //20160329 Delete PlanTruckSubTripJobDetail
                DeletePlanTruckSubTripJobDetail(planTruckSubTripJob, cn, tran);

                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_PLAN_SUB_TRIP_JOB_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTruckSubTripJob.planTripNo);
                cmd.Parameters.AddWithValue("@PLANSUBTRIP_SEQNO", planTruckSubTripJob.planSubTripSeqNo);
                cmd.Parameters.AddWithValue("@PLANSUBTRIPJOB_SEQNO", planTruckSubTripJob.planSubTripJobSeqNo);//20160201
                cmd.Parameters.AddWithValue("@JOB_ID", planTruckSubTripJob.jobID);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", planTruckSubTripJob.jobSeq);

                SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                // 2014-03-28 Zhou Kai adds comments: check the affacted row count.
                if (Convert.ToInt32(newReqNumber2.Value) == 0)
                {
                    throw new FMException(TptResourceDAL.ErrNoRecordDeleted);
                }


                //13 March - Gerry Commented out the checking below. 
                //it is posible to removed multiple rows in case of 1 jobtrip is assigned to different planTruckSubTrip
                //if (Convert.ToInt32(newReqNumber2.Value) > 1)
                //{
                //    throw new FMException(TptResourceDAL.ErrMoreThan1RecordDeleted);
                //}

                outPlanTruckSubTrip = new PlanTruckSubTrip();
                outPlanTruckSubTrip = (PlanTruckSubTrip)planTruckSubTrip.Clone();
                //outPlanTruckSubTrip.planTruckSubTripJobs = GetPlanTruckSubTripJobs(planTruckSubTripJob.planTripNo, planTruckSubTrip);

                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrDeleteFailed + " PlanTruckSubTripJob. \n" + ex.Message.ToString());
            }
            return retValue; ;
        }
        public static bool DeletePlanTruckSubTripJobDatabaseWithPreceeding(PlanTruckSubTrip planTruckSubTrip, PlanTruckSubTripJob planTruckSubTripJob, SqlConnection cn, SqlTransaction tran, out PlanTruckSubTrip outPlanTruckSubTrip)
        {
            bool retValue = false;
            try
            {
                //delete details first
                DeletePlanTruckSubTripJobDetailPreceeding(planTruckSubTripJob, cn, tran);
                string query = @"DELETE FROM TRK_PLAN_SUB_TRIP_JOB_TBL
	                                WHERE PLANTRIP_NO = '{0}'
	                                AND JOB_ID = '{1}'
	                                AND JOBTRIP_SEQ_NO = '{2}'";
                query = string.Format(query, planTruckSubTripJob.planTripNo,
                                             planTruckSubTripJob.jobID,
                                             planTruckSubTripJob.jobSeq);
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                outPlanTruckSubTrip = (PlanTruckSubTrip)planTruckSubTrip.Clone();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrDeleteFailed + " PlanTruckSubTripJob. \n" + ex.Message.ToString());
            }
            return retValue; ;
        }

        internal static Boolean IsPlanDateDuplicate(DateTime date, string tptDept)
        {
            bool retValue = true;
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT DISTINCT TRK_PLAN_TRIP_TBL.SCHEDULE_DATE FROM TRK_PLAN_TRIP_TBL with (NOLOCK)  ";
                SQLString += " WHERE TRK_PLAN_TRIP_TBL.SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
                SQLString += " AND TRK_PLAN_TRIP_TBL.SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";
                SQLString += " and TRK_PLAN_TRIP_TBL.DEPT='" + tptDept + "'";


                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
                DataSet dsSearchResult = new DataSet();
                daSearchCmd.Fill(dsSearchResult);
                cn.Close();
                if (dsSearchResult.Tables[0].Rows.Count > 0)
                {
                    retValue = false;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return retValue;

        }

        internal static SortableList<DateTime> GetPlanTripDates(DateTime fromDate, DateTime toDate, string tptDept)
        {
            SortableList<DateTime> dates = new SortableList<DateTime>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT DISTINCT SCHEDULE_DATE FROM TRK_PLAN_TRIP_TBL with (NOLOCK)  ";
                SQLString += "WHERE SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(fromDate) + "'";
                SQLString += " AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(toDate + TimeSpan.FromDays(1)) + "'";
                SQLString += " AND DRIVER_NO IN";
                SQLString += " (SELECT DRIVER_CODE FROM TPT_PLAN_DEPT_DRIVER_TBL with (NOLOCK)  WHERE ( SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(fromDate);
                SQLString += "' AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(toDate + TimeSpan.FromDays(1));
                SQLString += "')AND TPT_DEPT_CODE = '" + tptDept + "')";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                DateTime date;
                while (reader.Read())
                {
                    date = (DateTime)reader["SCHEDULE_DATE"];
                    date = date.Date;
                    if (dates.Find("Date", date) == -1)
                    {
                        dates.Add(date.Date);
                    }
                }
                reader.Close();
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return dates;
        }

        internal static bool SetPlanTruckSubTripJobStatus(PlanTruckSubTripJob planTruckSubTripJob, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //02 March 2012 - Gerry Added to if the date is not used by other user
                byte[] origVersion = new byte[8];
                #region CheckVersion
                string sqlCheckVersion = @"Select * from TRK_PLAN_SUB_TRIP_JOB_TBL with (NOLOCK) 
                                                 WHERE PLANTRIP_NO = '{0}'
                                                 AND PLANSUBTRIP_SEQNO = '{1}'
                                                 AND PLANSUBTRIPJOB_SEQNO = '{2}'
                                                 AND JOB_ID = {3}
                                                 AND JOBTRIP_SEQ_NO = {4}
                                            ";
                sqlCheckVersion = string.Format(sqlCheckVersion, planTruckSubTripJob.planTripNo,
                                                                planTruckSubTripJob.planSubTripSeqNo,
                                                                planTruckSubTripJob.planSubTripJobSeqNo,
                                                                planTruckSubTripJob.jobID,
                                                                planTruckSubTripJob.jobSeq
                                                                );

                SqlCommand cmd = new SqlCommand(sqlCheckVersion, con);
                cmd.CommandType = CommandType.Text;

                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    origVersion = (byte[])reader["UPDATE_VERSION"];
                    if (!SqlBinary.Equals(origVersion, planTruckSubTripJob.updateVersion))
                    {
                        throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nPlanTruckDAL.SetPlanTruckSubTripJobStatus");
                    }
                }
                reader.Close();
                #endregion
                //end
                //20160312 - gerry modified to update including previous plan subtrip where job trip was loaded
                string SQLString = @"UPDATE TRK_PLAN_SUB_TRIP_JOB_TBL
                                        SET STATUS = {0}
                                     WHERE PLANTRIP_NO = '{1}'
                                     AND VOLUME = '{2}'
                                     AND WEIGHT = '{3}'
                                     AND JOB_ID = {4}
                                     AND JOBTRIP_SEQ_NO = {5}
                                    ";
                SQLString = string.Format(SQLString, planTruckSubTripJob.status.GetHashCode(),
                                            planTruckSubTripJob.planTripNo,
                                            planTruckSubTripJob.volume,
                                            planTruckSubTripJob.weight,
                                            planTruckSubTripJob.jobID,
                                            planTruckSubTripJob.jobSeq);

                #region 20160217 - gerry removed
                /*
                if (planTruckSubTripJob.status == JobTripStatus.Booked)
                {
                    SQLString += " SET STATUS = 1";
                }
                if (planTruckSubTripJob.status == JobTripStatus.Ready)
                {
                    SQLString += " SET STATUS = 2";
                }
                if (planTruckSubTripJob.status == JobTripStatus.Assigned)
                {
                    SQLString += " SET STATUS = 3";
                }
                if (planTruckSubTripJob.status == JobTripStatus.Completed)
                {
                    SQLString += " SET STATUS = 4";
                }
                SQLString += " WHERE PLANTRIP_NO = '" + planTruckSubTripJob.planTripNo + "'";
                //Removed to update the same jobtripjob from other planTruckSubTrip 
                //SQLString += " AND SEQ_NO = " + planTruckSubTripJob.planTripSeq;
                SQLString += " AND JOB_ID = " + planTruckSubTripJob.jobID;
                SQLString += " AND JOB_SEQ_NO = " + planTruckSubTripJob.jobSeq;
                */
                #endregion

                cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                #region 20160312 - gerry removed
                // all preceeding plan subtrip also set to complete, not only 1 row will be updated
                /*
                // 2014-03-28 Zhou Kai adds affactedRow
                int affactedRow = 0;
                affactedRow = cmd.ExecuteNonQuery();
                if (affactedRow != 1) { throw new FMException(CommonResource.AffectedRowCountDoesNotMatchExpected); }
                // 2014-03-28 Zhou Kai ends
                */
                #endregion

                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return retValue;
        }

        internal static bool SetPlanTruckSubTripStatus(JobTripStatus tripStatus, string planTripNo, PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                int status = 0;
                if (tripStatus == JobTripStatus.Booked)
                    status = 1;
                if (tripStatus == JobTripStatus.Ready)
                    status = 2;
                if (tripStatus == JobTripStatus.Assigned)
                    status = 3;
                if (tripStatus == JobTripStatus.Completed)
                    status = 4;
                if (tripStatus == JobTripStatus.Invoiced)
                    status = 5;

                byte[] origVersion = new byte[8];
                #region Check Version
                string sqlCheckVersion = @"Select * from TRK_PLAN_SUB_TRIP_TBL with (NOLOCK) 
                                        where PLANTRIP_NO = '{0}'
                                                AND SEQ_NO = {1}";

                sqlCheckVersion = string.Format(sqlCheckVersion, planTripNo, planTruckSubTrip.SeqNo);

                SqlCommand cmd = new SqlCommand(sqlCheckVersion, cn);
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    origVersion = (byte[])reader["UPDATE_VERSION"];
                    if (!SqlBinary.Equals(origVersion, planTruckSubTrip.updateVersion))
                    {
                        throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nPlanTruckDAL.SetPlanTruckSubTripStatus");
                    }
                }
                reader.Close();
                #endregion
                string SQLUpdateString = " Update TRK_PLAN_SUB_TRIP_TBL set " +
                                               " STATUS = " + status +
                                               " Where PLANTRIP_NO = '" + planTripNo + "'" +
                                               " and SEQ_NO = " + planTruckSubTrip.SeqNo;

                cmd = new SqlCommand(SQLUpdateString, cn);
                cmd.Transaction = tran;
                int rowaffected = 0;
                rowaffected = cmd.ExecuteNonQuery();

                if (rowaffected <= 0)
                {
                    throw new FMException(TptResourceDAL.ErrNoRowUpdated);
                }
                else
                {
                    retValue = true;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        internal static bool UpdatePlanTruckSubTripStopsAndTime(PlanTruckTrip planTruckTrip, PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TRK_PLAN_SUB_TRIP_TBL
                                            set 
                                            START_STOP = '{0}'    
                                            ,END_STOP = '{1}'  
                                            ,START_TIME = '{2}'    
                                            ,END_TIME = '{3}' 
                                        WHERE
                                            PLANTRIP_NO ='{4}'
                                           AND SEQ_NO = {5}   
                                         ";

                updateQuery = string.Format(updateQuery,
                                            planTruckSubTrip.StartStop.Code.ToString().Trim(),
                                            planTruckSubTrip.EndStop.Code.ToString().Trim(),
                                            DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planTruckSubTrip.Start),
                                            DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planTruckSubTrip.End),
                                            planTruckTrip.PlanTripNo.ToString().Trim(),
                                            planTruckSubTrip.SeqNo);

                SqlCommand cmd = new SqlCommand(updateQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        internal static bool DeletePlanTruckTrip(string planTripNo, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            if (con.State == ConnectionState.Closed) { con.Open(); }
            try
            {
                string SQLString = @"delete from TRK_PLAN_TRIP_TBL
                                        where PLANTRIP_NO ='{0}'";

                SQLString = string.Format(SQLString, planTripNo);

                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        internal static bool UpdateWeightAndVolume(PlanTruckSubTripJob planTruckSubTripJob, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                SqlCommand cmd;
                byte[] origVersion = new byte[8];
                #region CheckVersion
                string sqlCheckVersion = @"Select * from TRK_PLAN_SUB_TRIP_JOB_TBL with (NOLOCK) 
                                        where PLANTRIP_NO = '{0}'
                                                AND PLANSUBTRIP_SEQNO = {1}  
                                                AND JOB_ID = {2}
                                                AND JOBTRIP_SEQ_NO = {3}
                                                AND PLANSUBTRIPJOB_SEQNO = {4}";

                sqlCheckVersion = string.Format(sqlCheckVersion, planTruckSubTripJob.planTripNo,
                                                                planTruckSubTripJob.planSubTripSeqNo,
                                                                planTruckSubTripJob.jobID,
                                                                planTruckSubTripJob.jobSeq,
                                                                planTruckSubTripJob.planSubTripJobSeqNo
                                                                );

                cmd = new SqlCommand(sqlCheckVersion, con);
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    origVersion = (byte[])reader["UPDATE_VERSION"];
                    if (!SqlBinary.Equals(origVersion, planTruckSubTripJob.updateVersion))
                    {
                        throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nPlanTruckDAL.UpdateWeightAndVolume");
                    }
                }
                reader.Close();
                #endregion
                string SQLString = @"UPDATE TRK_PLAN_SUB_TRIP_JOB_TBL
                                    set VOLUME = '{0}', 
                                    WEIGHT ='{1}'
                                    Where PLANTRIP_NO = '{2}'
                                    and PLANSUBTRIP_SEQNO ={3}
                                    and JOBTRIP_PLAN_SEQ={4}
                                    and JOB_ID ={5}
                                    and JOBTRIP_SEQ_NO={6}";
                SQLString = string.Format(SQLString, planTruckSubTripJob.volume,
                                                     planTruckSubTripJob.weight,
                                                     planTruckSubTripJob.planTripNo,
                                                     planTruckSubTripJob.planSubTripSeqNo,
                                                     planTruckSubTripJob.planSubTripJobSeqNo,
                                                     planTruckSubTripJob.jobID,
                                                     planTruckSubTripJob.jobSeq);

                cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                planTruckSubTripJob = GetPlanTruckSubTripJob(planTruckSubTripJob.planTripNo,
                                                             planTruckSubTripJob.planSubTripSeqNo,
                                                             planTruckSubTripJob.jobID,
                                                             planTruckSubTripJob.jobSeq,
                                                             planTruckSubTripJob.planSubTripJobSeqNo, con, tran);
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        // 2013-11-11 Zhou Kai adds
        internal static bool UpdateActualStartEndTimeforTruckJobDetailTripPlan(int jobID, int jobTripSeqNo,
                                                                               string planTripNo, int jobTripPlanSeqNo,
                                                                               DateTime start_time, DateTime end_time,
                                                                               SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            string sqlUpdateActualStartEndTime = @"UPDATE TRK_JOB_DETAIL_TRIP_PLAN_Tbl 
                                                    SET START_TIME = @start_time, 
                                                        END_TIME = @end_time 

                                                        WHERE JOB_ID = @job_id 
                                                        AND JOBTRIP_SEQ_NO = @JobTripSeqNo 
                                                        AND PLANTRIP_NO = @plantrip_no 
                                                        AND PLANSUBTRIPJOB_SEQNO = @jobtrip_plan_seq;";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlUpdateActualStartEndTime, con);
                cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("start_time", start_time);
                cmd.Parameters.AddWithValue("end_time", end_time);
                cmd.Parameters.AddWithValue("job_id", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                cmd.Parameters.AddWithValue("plantrip_no", planTripNo);
                cmd.Parameters.AddWithValue("jobtrip_plan_seq", jobTripPlanSeqNo);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return retValue;
        }



        internal static bool UpdateActualStartEndTimeforTruckJobDetailTripPlan(int jobID, int jobTripSeqNo,
                                                                       string planTripNo, int jobTripPlanSeqNo,
                                                                       DateTime start_time, DateTime end_time)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string sqlUpdateActualStartEndTime = @"UPDATE TRK_JOB_DETAIL_TRIP_PLAN_Tbl 
                                                    SET START_TIME = @start_time, 
                                                        END_TIME = @end_time 

                                                        WHERE JOB_ID = @job_id 
                                                        AND JOBTRIP_SEQ_NO = @JobTripSeqNo 
                                                        AND PLANTRIP_NO = @plantrip_no 
                                                        AND PLANSUBTRIPJOB_SEQNO = @jobtrip_plan_seq;";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlUpdateActualStartEndTime, con);
                if (con.State != ConnectionState.Open) { con.Open(); }
                cmd.Parameters.AddWithValue("start_time", start_time);
                cmd.Parameters.AddWithValue("end_time", end_time);
                cmd.Parameters.AddWithValue("job_id", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                cmd.Parameters.AddWithValue("plantrip_no", planTripNo);
                cmd.Parameters.AddWithValue("jobtrip_plan_seq", jobTripPlanSeqNo);

                cmd.ExecuteNonQuery();
                retValue = true;
            }  //gerry replace catch clause
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { con.Close(); }

            return retValue;
        }
        // 2013-11-11 Zhou Kai ends

        // 2013-11-14 Zhou Kai adds
        internal static bool UpdateAcutalStartEndTimeforTruckJobDetailTrip(int jobID, int jobTripSeqNo,
                                                                       DateTime StartDateTime, DateTime EndDateTime,
                                                                       SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            string startDate = DateUtility.ConvertDateForSQLPurpose(StartDateTime);
            string endDate = DateUtility.ConvertDateForSQLPurpose(EndDateTime);
            string start_time = StartDateTime.ToString("HHmm");
            string end_time = EndDateTime.ToString("HHmm");

            string sqlUpdateActualStartEndTime = "UPDATE TRK_JOB_DETAIL_TRIP_Tbl SET FROM_DATE = @from_date, " +
                "TO_DATE = @to_date, FROM_TIME = @from_time, TO_TIME = @to_time " +
                "WHERE JOB_ID = @job_id AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";

            SqlCommand cmd = new SqlCommand(sqlUpdateActualStartEndTime, con);
            cmd.Transaction = tran;
            try
            {
                cmd.Parameters.AddWithValue("from_date", startDate);
                cmd.Parameters.AddWithValue("to_date", endDate);
                cmd.Parameters.AddWithValue("from_time", start_time);
                cmd.Parameters.AddWithValue("to_time", end_time);
                cmd.Parameters.AddWithValue("job_id", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return retValue;
        }

        internal static bool UpdateAcutalStartEndTimeforTruckJobDetailTrip(int jobID, int jobTripSeqNo,
                                                                           DateTime StartDateTime, DateTime EndDateTime)
        {
            bool retValue = false;
            string startDate = DateUtility.ConvertDateForSQLPurpose(StartDateTime);
            string endDate = DateUtility.ConvertDateForSQLPurpose(EndDateTime);
            string start_time = StartDateTime.ToString("HHmm");
            string end_time = EndDateTime.ToString("HHmm");

            string sqlUpdateActualStartEndTime = "UPDATE TRK_JOB_DETAIL_TRIP_Tbl SET FROM_DATE = @from_date, " +
                "TO_DATE = @to_date, FROM_TIME = @from_time, TO_TIME = @to_time " +
                "WHERE JOB_ID = @job_id AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand cmd = new SqlCommand(sqlUpdateActualStartEndTime, con);
            try
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                cmd.Parameters.AddWithValue("FROM_DATE", startDate);
                cmd.Parameters.AddWithValue("TO_DATE", endDate);
                cmd.Parameters.AddWithValue("FROM_TIME", start_time);
                cmd.Parameters.AddWithValue("TO_TIME", end_time);
                cmd.Parameters.AddWithValue("job_id", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);

                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return retValue;
        }
        // 2013-11-14 Zhou Kai ends

        // 2013-12-06 Zhou Kai adds function to update 
        // Update TRK_JOB_DETAIL_TRIP_PLAN_Tbl and
        // Update TRK_JOB_DETAIL_TRIP_Tbl
        public static bool UpdateActualStartEndTime(PlanTruckSubTripJob newPTSTJ, string frmName, string usrId,
             DateTime new_start_date_time, DateTime new_end_date_time, SqlConnection con, SqlTransaction tran)
        {
            bool retVal = true;
            retVal = retVal && UpdateActualStartEndTimeforTruckJobDetailTripPlan(newPTSTJ.jobID, newPTSTJ.jobSeq, newPTSTJ.planTripNo, newPTSTJ.planSubTripJobSeqNo,
                new_start_date_time, new_end_date_time, con, tran);
            // 2013-12-10 Zhou Kai comments
            //retVal = retVal && UpdateAcutalStartEndTimeforTruckJobDetailTrip(newPTSTJ.jobID, newPTSTJ.jobSeq, new_start_date_time, new_end_date_time,
            //    con, tran);
            // 2013-12-10 Zhou Kai ends
            return retVal;
        }

        // 2013-12-06 Zhou Kai ends

        //20160204 -gerry added
        internal static bool HasPlanTruckSubTripChanged(PlanTruckSubTrip planSubTrip)
        {
            bool retValue = false;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string query = @"select UPDATE_VERSION from TRK_PLAN_SUB_TRIP_TBL with (NOLOCK) 
                                    where PLANTRIP_NO = '{0}'
                                    and SEQ_NO = {1}";
                    query = string.Format(query, planSubTrip.PlanTripNo.Trim(), planSubTrip.SeqNo);

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    IDataReader reader = cmd.ExecuteReader();
                    byte[] origVersion = null;
                    while (reader.Read()) { origVersion = (byte[])reader["UPDATE_VERSION"]; }
                    reader.Close();
                    //if some drag/drop or transfer to other driver
                    if (!SqlBinary.Equals(planSubTrip.updateVersion, origVersion))
                    {
                        string msg = @"Plan Sub trip was modified by someone. Please refresh your screen before doing any changes. ";
                        throw new FMException(msg);
                    }
                    //if someone added or deleted some job inside
                    int count = 0;
                    if (planSubTrip.planTruckSubTripJobs.Count > 0)
                        count = GetPlanTruckSubTripJobs(planSubTrip.planTruckSubTripJobs[0].planTripNo, planSubTrip, con, con.BeginTransaction()).Count;

                    if (count != planSubTrip.planTruckSubTripJobs.Count)
                    {
                        string msg = @"Plan Sub trip was modified by someone, some job(s) inside were added or deleted. Please refresh your screen before doing any changes. ";
                        throw new FMException(msg);
                    }
                }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (FMException ex) { throw ex; }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            }
            return retValue;
        }
        internal static bool HasPlanTruckSubTripChanged(PlanTruckSubTrip planSubTrip, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"select UPDATE_VERSION from TRK_PLAN_SUB_TRIP_TBL with (NOLOCK) 
                                    where PLANTRIP_NO = '{0}'
                                    and SEQ_NO = {1}";
                query = string.Format(query, planSubTrip.PlanTripNo.Trim(), planSubTrip.SeqNo);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                byte[] origVersion = null;
                while (reader.Read()) { origVersion = (byte[])reader["UPDATE_VERSION"]; }
                reader.Close();
                //if some drag/drop or transfer to other driver
                if (!SqlBinary.Equals(planSubTrip.updateVersion, origVersion))
                {
                    string msg = @"Plan Sub trip was modified by someone. Please refresh your screen before doing any changes. ";
                    throw new FMException(msg);
                }
                //if someone added or deleted some job inside
                int count = 0;
                if (planSubTrip.planTruckSubTripJobs.Count > 0)
                    count = GetPlanTruckSubTripJobs(planSubTrip.planTruckSubTripJobs[0].planTripNo, planSubTrip, con, tran).Count;

                if (count != planSubTrip.planTruckSubTripJobs.Count)
                {
                    string msg = @"Plan Sub trip was modified by someone, some job(s) inside were added or deleted. Please refresh your screen before doing any changes. ";
                    throw new FMException(msg);
                }
            }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static byte[] GetPlanTruckTripUpdateVersion(PlanTruckTrip planTrip)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string query = @"select UPDATE_VERSION from TRK_PLAN_TRIP_TBL with (NOLOCK) 
                                    where PLANTRIP_NO = '{0}'";
                    query = string.Format(query, planTrip.PlanTripNo.Trim());

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    IDataReader reader = cmd.ExecuteReader();
                    byte[] origVersion = null;
                    while (reader.Read()) { origVersion = (byte[])reader["UPDATE_VERSION"]; }
                    reader.Close();
                    return origVersion;
                }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (FMException ex) { throw ex; }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            }
        }
        internal static byte[] GetPlanTruckTripUpdateVersion(PlanTruckTrip planTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"select UPDATE_VERSION from TRK_PLAN_TRIP_TBL with (NOLOCK) 
                                    where PLANTRIP_NO = '{0}'";
                query = string.Format(query, planTrip.PlanTripNo.Trim());

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                byte[] origVersion = null;
                while (reader.Read()) { origVersion = (byte[])reader["UPDATE_VERSION"]; }
                reader.Close();
                return origVersion;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static byte[] GetPlanTruckSubTripUpdateVersion(PlanTruckSubTrip planSubTrip)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string query = @"select UPDATE_VERSION from TRK_PLAN_SUB_TRIP_TBL with (NOLOCK) 
                                    where PLANTRIP_NO = '{0}'
                                    and SEQ_NO = {1}";
                    query = string.Format(query, planSubTrip.PlanTripNo.Trim(), planSubTrip.SeqNo);

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    IDataReader reader = cmd.ExecuteReader();
                    byte[] origVersion = null;
                    while (reader.Read()) { origVersion = (byte[])reader["UPDATE_VERSION"]; }
                    reader.Close();
                    return origVersion;
                }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (FMException ex) { throw ex; }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            }
        }
        internal static byte[] GetPlanTruckSubTripUpdateVersion(PlanTruckSubTrip planSubTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"select UPDATE_VERSION from TRK_PLAN_SUB_TRIP_TBL with (NOLOCK) 
                                    where PLANTRIP_NO = '{0}'
                                    and SEQ_NO = {1}";
                query = string.Format(query, planSubTrip.PlanTripNo.Trim(), planSubTrip.SeqNo);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                byte[] origVersion = null;
                while (reader.Read()) { origVersion = (byte[])reader["UPDATE_VERSION"]; }
                reader.Close();
                return origVersion;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static SortableList<PlanTruckSubTripJobDetail> GetPlanTruckSubTripJobDetails(PlanTruckSubTripJob planTruckSubTripJob)
        {
            SortableList<PlanTruckSubTripJobDetail> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJobDetail>();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = @"SELECT pjtd.*, jtd.MARKING as ITEM_NAME from [TRK_PLAN_SUB_TRIP_JOB_DETAIL_TBL] pjtd with (NOLOCK) 
                                        inner join TRK_JOB_DETAIL_TRIP_DETAIL_Tbl jtd with (NOLOCK) 
                                        on  pjtd.[JOB_ID] = jtd.JOB_ID
	                                        and pjtd.[JOBTRIP_SEQ_NO] = jtd.JOBTRIP_SEQ_NO
	                                        and pjtd.DETAIL_SEQNO = jtd.SEQ_NO
                                        where pjtd.[PLANTRIP_NO] = '{0}'
                                            and pjtd.[PLANSUBTRIP_SEQNO] = {1}
                                            and pjtd.[JOB_ID] = {2}
                                            and pjtd.[JOBTRIP_SEQ_NO] = {3}
                                            and pjtd.[PLANSUBTRIPJOB_SEQNO] = {4}";
                    SQLString = string.Format(SQLString, planTruckSubTripJob.planTripNo,
                                                         planTruckSubTripJob.planSubTripSeqNo,
                                                         planTruckSubTripJob.jobID,
                                                         planTruckSubTripJob.jobSeq,
                                                         planTruckSubTripJob.planSubTripJobSeqNo);

                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    SqlCommand cmd = new SqlCommand(SQLString, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        PlanTruckSubTripJobDetail temp = new PlanTruckSubTripJobDetail();
                        temp.planTripNo = reader["PLANTRIP_NO"] == DBNull.Value ? string.Empty : (string)reader["PLANTRIP_NO"];
                        temp.planSubTripSeqNo = reader["PLANSUBTRIP_SEQNO"] == DBNull.Value ? 0 : (int)reader["PLANSUBTRIP_SEQNO"];
                        temp.jobID = reader["JOB_ID"] == DBNull.Value ? 0 : (int)reader["JOB_ID"];
                        temp.jobSeq = reader["JOBTRIP_SEQ_NO"] == DBNull.Value ? 0 : (int)reader["JOBTRIP_SEQ_NO"];
                        temp.planSubTripJobSeqNo = reader["PLANSUBTRIPJOB_SEQNO"] == DBNull.Value ? 0 : (int)reader["PLANSUBTRIPJOB_SEQNO"];
                        temp.planSubTripJobDetailSeqNo = reader["DETAIL_SEQNO"] == DBNull.Value ? 0 : (int)reader["DETAIL_SEQNO"];
                        temp.marking = reader["ITEM_NAME"] == DBNull.Value ? string.Empty : (string)reader["ITEM_NAME"];
                        temp.uom = reader["UOM"] == DBNull.Value ? string.Empty : (string)reader["UOM"];
                        temp.qty = reader["QTY"] == DBNull.Value ? 0 : Convert.ToInt32(reader["QTY"]);
                        temp.length = reader["LENGTH"] == DBNull.Value ? 0 : (decimal)reader["LENGTH"];
                        temp.width = reader["WIDTH"] == DBNull.Value ? 0 : (decimal)reader["WIDTH"];
                        temp.height = reader["HEIGHT"] == DBNull.Value ? 0 : (decimal)reader["HEIGHT"];
                        temp.unitWeight = reader["UNIT_WEIGHT"] == DBNull.Value ? 0 : (decimal)reader["UNIT_WEIGHT"];

                        //20170616
                        temp.actQty = reader["ActQty"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ActQty"]);
                        temp.ref_No = reader["Ref_No"] == DBNull.Value ? string.Empty : (string)reader["Ref_No"];

                        planTruckSubTripJobs.Add(temp);
                    }
                    reader.Close();
                }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { if (con.State != ConnectionState.Closed) { con.Close(); } }
            }
            return planTruckSubTripJobs;
        }
        internal static SortableList<PlanTruckSubTripJobDetail> GetPlanTruckSubTripJobDetails(PlanTruckSubTripJob planTruckSubTripJob, SqlConnection con, SqlTransaction tran)
        {
            SortableList<PlanTruckSubTripJobDetail> planTruckSubTripJobs = new SortableList<PlanTruckSubTripJobDetail>();
            try
            {
                string SQLString = @"SELECT * from [TRK_PLAN_SUB_TRIP_JOB_DETAIL_TBL] with (NOLOCK) 
                                      where [PLANTRIP_NO] = '{0}'
                                           and [PLANSUBTRIP_SEQNO] = {1}
                                           and [JOB_ID] = {2}
                                           and [JOBTRIP_SEQ_NO] = {3}
                                           and [PLANSUBTRIPJOB_SEQNO] = {4}";
                SQLString = string.Format(SQLString, planTruckSubTripJob.planTripNo,
                                                     planTruckSubTripJob.planSubTripSeqNo,
                                                     planTruckSubTripJob.jobID,
                                                     planTruckSubTripJob.jobSeq,
                                                     planTruckSubTripJob.planSubTripJobSeqNo);

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PlanTruckSubTripJobDetail temp = new PlanTruckSubTripJobDetail();
                    temp.planTripNo = reader["PLANTRIP_NO"] == DBNull.Value ? string.Empty : (string)reader["PLANTRIP_NO"];
                    temp.planSubTripSeqNo = reader["PLANSUBTRIP_SEQNO"] == DBNull.Value ? 0 : (int)reader["PLANSUBTRIP_SEQNO"];
                    temp.jobID = reader["JOB_ID"] == DBNull.Value ? 0 : (int)reader["JOB_ID"];
                    temp.jobSeq = reader["JOBTRIP_SEQ_NO"] == DBNull.Value ? 0 : (int)reader["JOBTRIP_SEQ_NO"];
                    temp.planSubTripJobSeqNo = reader["PLANSUBTRIPJOB_SEQNO"] == DBNull.Value ? 0 : (int)reader["PLANSUBTRIPJOB_SEQNO"];
                    temp.planSubTripJobDetailSeqNo = reader["DETAIL_SEQNO"] == DBNull.Value ? 0 : (int)reader["DETAIL_SEQNO"];
                    temp.marking = reader["Marking"] == DBNull.Value ? string.Empty : (string)reader["Marking"];
                    temp.uom = reader["UOM"] == DBNull.Value ? string.Empty : (string)reader["UOM"];
                    temp.qty = reader["QTY"] == DBNull.Value ? 0 : (int)reader["QTY"];
                    temp.length = reader["LENGTH"] == DBNull.Value ? 0 : (decimal)reader["LENGTH"];
                    temp.width = reader["WIDTH"] == DBNull.Value ? 0 : (decimal)reader["WIDTH"];
                    temp.height = reader["HEIGHT"] == DBNull.Value ? 0 : (decimal)reader["HEIGHT"];
                    temp.unitWeight = reader["UNIT_WEIGHT"] == DBNull.Value ? 0 : (decimal)reader["UNIT_WEIGHT"];

                    //20170616
                    temp.actQty = reader["ActQty"] == DBNull.Value ? 0 : (int)reader["ActQty"];
                    temp.ref_No = reader["Ref_No"] == DBNull.Value ? string.Empty : (string)reader["Ref_No"];

                    planTruckSubTripJobs.Add(temp);
                }
                reader.Close();
            }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planTruckSubTripJobs;
        }

        internal static bool AddPlanTruckSubTripJobDetail(PlanTruckSubTripJobDetail planTruckSubTripJobDetail, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                #region SQL string
                string insertString = @"INSERT INTO [TRK_PLAN_SUB_TRIP_JOB_DETAIL_TBL](
	                                                [PLANTRIP_NO],
	                                                [PLANSUBTRIP_SEQNO],
	                                                [JOB_ID],
	                                                [JOBTRIP_SEQ_NO],
	                                                [PLANSUBTRIPJOB_SEQNO],
                                                    [DETAIL_SEQNO],
	                                                [Marking],
	                                                [UOM],
	                                                [QTY],
	                                                [LENGTH],
	                                                [WIDTH],
	                                                [HEIGHT],
	                                                [UNIT_WEIGHT],
                                                    [Ref_No] )
                                                VALUES('{0}', {1}, {2}, {3}, {4}, {5}, '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}')";
                insertString = string.Format(insertString, CommonUtilities.FormatString(planTruckSubTripJobDetail.planTripNo),
                                                            planTruckSubTripJobDetail.planSubTripSeqNo,
                                                            planTruckSubTripJobDetail.jobID,
                                                            planTruckSubTripJobDetail.jobSeq,
                                                            planTruckSubTripJobDetail.planSubTripJobSeqNo,
                                                            planTruckSubTripJobDetail.planSubTripJobDetailSeqNo,
                                                            CommonUtilities.FormatString(planTruckSubTripJobDetail.marking),
                                                            CommonUtilities.FormatString(planTruckSubTripJobDetail.uom),
                                                            planTruckSubTripJobDetail.qty,
                                                            planTruckSubTripJobDetail.length,
                                                            planTruckSubTripJobDetail.width,
                                                            planTruckSubTripJobDetail.height,
                                                            planTruckSubTripJobDetail.unitWeight,
                                                            planTruckSubTripJobDetail.ref_No);
                #endregion
                SqlCommand cmd = new SqlCommand(insertString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }

        public static bool DeletePlanTruckSubTripJobDetail(PlanTruckSubTripJob planTruckSubTripJob, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                string query = @"Delete from TRK_PLAN_SUB_TRIP_JOB_DETAIL_TBL
                                    where PLANTRIP_NO = '{0}'
                                    and PLANSUBTRIP_SEQNO ='{1}'
                                    and PLANSUBTRIPJOB_SEQNO ='{2}'
                                    and JOB_ID ='{3}'
                                    and JOBTRIP_SEQ_NO ='{4}'";
                query = string.Format(query, planTruckSubTripJob.planTripNo,
                                             planTruckSubTripJob.planSubTripSeqNo,
                                             planTruckSubTripJob.planSubTripJobSeqNo,
                                             planTruckSubTripJob.jobID,
                                             planTruckSubTripJob.jobSeq);
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                retValue = true;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrDeleteFailed + " PlanTruckSubTripJob. \n" + ex.ToString());
            }
            return retValue; ;
        }
        public static bool DeletePlanTruckSubTripJobDetailPreceeding(PlanTruckSubTripJob planTruckSubTripJob, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                string query = @"Delete from TRK_PLAN_SUB_TRIP_JOB_DETAIL_TBL
                                    where PLANTRIP_NO = '{0}'
                                    and JOB_ID ='{1}'
                                    and JOBTRIP_SEQ_NO ='{2}'";
                query = string.Format(query, planTruckSubTripJob.planTripNo,
                                             planTruckSubTripJob.jobID,
                                             planTruckSubTripJob.jobSeq);
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                retValue = true;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrDeleteFailed + " PlanTruckSubTripJob. \n" + ex.ToString());
            }
            return retValue; ;
        }
        //20160820 - gerry added to update shipment id given from 3D app
        internal static bool UpdatePlanTruckSubTripShipmentId(PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TRK_PLAN_SUB_TRIP_TBL
                                            set SHIPMENT_ID = '{2}' 
                                        WHERE
                                            PLANTRIP_NO ='{0}'
                                           AND SEQ_NO = {1}   
                                         ";

                updateQuery = string.Format(updateQuery,
                                            planTruckSubTrip.PlanTripNo.ToString().Trim(),
                                            planTruckSubTrip.SeqNo,
                                            CommonUtilities.FormatString(planTruckSubTrip.shipment_id));

                SqlCommand cmd = new SqlCommand(updateQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        //20160908 - gerry added method to capture the cloudtrack route_point_id and jobid
        internal static bool UpdatePlanTruckSubTripCloudtrack_Job_id(PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TRK_PLAN_SUB_TRIP_TBL
                                            set CT_Task_ID = '{2}'
                                        WHERE
                                            PLANTRIP_NO ='{0}'
                                           AND SEQ_NO = {1}   
                                         ";

                updateQuery = string.Format(updateQuery,
                                            CommonUtilities.FormatString(planTruckSubTrip.PlanTripNo.ToString().Trim()),
                                            planTruckSubTrip.SeqNo,
                                            CommonUtilities.FormatString(planTruckSubTrip.Task_ID));

                SqlCommand cmd = new SqlCommand(updateQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static bool UpdatePlanTruckSubTripJobCloudtrack_Job_id(PlanTruckSubTrip planTruckSubTrip, PlanTruckSubTripJob planTruckSubTripJob, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TRK_PLAN_SUB_TRIP_JOB_TBL
                                            set CT_Task_ID = '{4}'
                                        WHERE
                                            PLANTRIP_NO ='{0}'
                                           AND PLANSUBTRIP_SEQNO = {1}   
                                           AND JOB_ID ='{2}'
                                           AND JOBTRIP_SEQ_NO = {3}   
                                         ";

                updateQuery = string.Format(updateQuery,
                                            CommonUtilities.FormatString(planTruckSubTrip.PlanTripNo.ToString().Trim()),
                                            planTruckSubTrip.SeqNo,
                                            planTruckSubTripJob.jobID,
                                            planTruckSubTripJob.jobSeq,
                                            CommonUtilities.FormatString(planTruckSubTripJob.cloudTrack_JobId));

                SqlCommand cmd = new SqlCommand(updateQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static bool UpdatePlanTruckSubTripCloudtrack_RoutePoint_id(PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TRK_PLAN_SUB_TRIP_TBL
                                            set CT_LOCATION_POINT_ID = '{2}'
                                        WHERE
                                            PLANTRIP_NO ='{0}'
                                           AND SEQ_NO = {1}   
                                         ";

                updateQuery = string.Format(updateQuery,
                                            CommonUtilities.FormatString(planTruckSubTrip.PlanTripNo.ToString().Trim()),
                                            planTruckSubTrip.SeqNo,
                                            CommonUtilities.FormatString(planTruckSubTrip.route_point_id));

                SqlCommand cmd = new SqlCommand(updateQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static bool InsertRouteLocation(TaskLocation taskLocation, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                #region insert Query
                string insertQuery = @"INSERT INTO [dbo].[TPT_ROUTE_LOCATION_TBL]
                                       ([ID]
                                       ,[NAME]
                                       ,[DESCRIPTION]
                                       ,[ADDRESS]
                                       ,[PHONE]
                                       ,[EMAIL]
                                       ,[RADIUS]
                                       ,[EXTERNAL_ID]
                                       ,[LATITUDE]
                                       ,[LONGITUDE]
                                       ,[KIND]
                                       ,[RND])
                                 VALUES
                                       ({0}
                                       ,'{1}'
                                       ,'{2}'
                                       ,'{3}'
                                       ,'{4}'
                                       ,'{5}'
                                       ,'{6}'
                                       ,'{7}'
                                       ,'{8}'
                                       ,'{9}'
                                       ,'{10}'
                                       ,'{11}')  
                                         ";
                insertQuery = string.Format(insertQuery,
                                            taskLocation.loc_id,
                                            CommonUtilities.FormatString(taskLocation.name),
                                            CommonUtilities.FormatString(taskLocation.description),
                                            CommonUtilities.FormatString(taskLocation.address),
                                            CommonUtilities.FormatString(taskLocation.phone),
                                            CommonUtilities.FormatString(taskLocation.email),
                                            taskLocation.radius,
                                            CommonUtilities.FormatString(taskLocation.external_id),
                                            taskLocation.lat,
                                            taskLocation.lon,
                                            CommonUtilities.FormatString(taskLocation.kind),
                                            CommonUtilities.FormatString(taskLocation.rnd));
                #endregion
                SqlCommand cmd = new SqlCommand(insertQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static bool isRouteLocationAdded()
        {
            return false;
        }
        //currently no use no need to show the square in the MAP if want see the route
        internal static bool UpdatePlanTripGeoLocation(PlanTruckTrip planTruckTrip)
        {
            bool retValue = false;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string SQLString = @"UPDATE TRK_PLAN_TRIP_TBL
                                    set GEO_CENTER_PLACE = '{0}', 
                                    GEO_RADIUS ='{1}'
                                    Where PLANTRIP_NO = '{2}'";
                    SQLString = string.Format(SQLString, planTruckTrip.centerPoint, planTruckTrip.radius, planTruckTrip.PlanTripNo);

                    SqlCommand cmd = new SqlCommand(SQLString, con);
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                    retValue = true;
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally{con.Close();}
                return retValue;
            }
        }
        //201061206 update planTruckSubTrip start/endTime
        internal static bool UpdatePlanTruckSubTripStartAndEndTime(PlanTruckSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TRK_PLAN_SUB_TRIP_TBL
                                            set Start_Time = '{2}',
                                                End_Time = '{3}'
                                        WHERE
                                            PLANTRIP_NO ='{0}'
                                           AND SEQ_NO = {1}   
                                         ";

                updateQuery = string.Format(updateQuery,
                                            CommonUtilities.FormatString(planTruckSubTrip.PlanTripNo.ToString().Trim()),
                                            planTruckSubTrip.SeqNo,
                                            DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planTruckSubTrip.Start),
                                            DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planTruckSubTrip.End));

                SqlCommand cmd = new SqlCommand(updateQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        //20170220 - truckcomm
        internal static bool IsTaskAssignedInDriver(PlanTruckSubTrip planTruckSubTrip)
        {
            bool retValue = false;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                try
                {
                    string selectQuery = @"SELECT * FROM TPT_TRUCK_COM_TRACKING_TBL  with (NOLOCK) 
                                            where TASK_ID = '{0}' 
	                                            and (status not in ('A','R')) 
                                            ";

                    selectQuery = string.Format(selectQuery, planTruckSubTrip.Task_ID);

                    SqlCommand cmd = new SqlCommand(selectQuery, con);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                        retValue = true;
                    else
                        retValue = false;
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            }
            return retValue;
        }
        //20170221
        internal static PlanTruckSubTrip GetPlanTruckSubTrip(PlanTruckTrip planTruckTrip, string message_TaskID)
        {
            PlanTruckSubTrip planTruckSubTrip = null;
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = "SELECT * FROM TRK_PLAN_SUB_TRIP_TBL with (NOLOCK)  ";
                    SQLString += "WHERE CT_Task_ID = '" + message_TaskID + "'";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        planTruckSubTrip = GetPlanTruckSubTrip(reader, planTruckTrip);
                    }
                    reader.Close();
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { cn.Close(); }
            }
            return planTruckSubTrip;
        }
    }
}
