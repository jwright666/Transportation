using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using FM.TR_HLPlanDLL.BLL;
using FM.TR_HLBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using TR_LanguageResource.Resources;
using TR_MessageDLL.BLL;
using FM.TR_HLBookDLL.Facade;

namespace FM.TR_HLPlanDLL.DAL
{
    internal class PlanTransportDAL
    {
        internal static SortableList<DateTime> GetPlanTripDates(DateTime fromDate, DateTime toDate, string tptDept)
        {
            SortableList<DateTime> dates = new SortableList<DateTime>();
            try
            {
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT DISTINCT SCHEDULE_DATE FROM TPT_PLAN_TRIP_TBL with (NOLOCK) ";
                    SQLString += "WHERE SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(fromDate) + "'";
                    SQLString += " AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(toDate + TimeSpan.FromDays(1)) + "'";
                    SQLString += " AND DRIVER_NO IN";
                    SQLString += " (SELECT DRIVER_CODE FROM TPT_PLAN_DEPT_DRIVER_TBL with (NOLOCK) WHERE ( SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(fromDate);
                    SQLString += "' AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(toDate + TimeSpan.FromDays(1));
                    SQLString += "')AND TPT_DEPT_CODE = '" + CommonUtilities.FormatString(tptDept) + "')";
                    SQLString += " ORDER BY SCHEDULE_DATE DESC ";

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    //DateTime date;
                    //int a;
                    while (reader.Read())
                    {
                        dates.Add((DateTime)reader["SCHEDULE_DATE"]);
                        //date = (DateTime)reader["SCHEDULE_DATE"];
                        //date = date.Date;
                        //if (dates.Find("Date", date) == -1)
                        //{
                        //    dates.Add(date.Date);
                        //}
                    }
                    cn.Close();
                }
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanTripDates method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return dates;
        }

        internal static Boolean IsPlanDateDuplicate(DateTime date, string tptDept)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT DISTINCT TPT_PLAN_TRIP_TBL.SCHEDULE_DATE FROM TPT_PLAN_TRIP_TBL with (NOLOCK) ";
                    SQLString += " inner join TPT_PLAN_DEPT_DRIVER_TBL with (NOLOCK)";
                    SQLString += " on TPT_PLAN_TRIP_TBL.DRIVER_NO=TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE";
                    SQLString += " and TPT_PLAN_TRIP_TBL.SCHEDULE_DATE=TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE";
                    SQLString += " WHERE TPT_PLAN_TRIP_TBL.SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
                    SQLString += " AND TPT_PLAN_TRIP_TBL.SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";
                    SQLString += " and TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE='" + CommonUtilities.FormatString(tptDept) + "'";


                    SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
                    DataSet dsSearchResult = new DataSet();
                    daSearchCmd.Fill(dsSearchResult);
                    cn.Close();
                    if (dsSearchResult.Tables[0].Rows.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside IsPlanDateDuplicate(DateTime date, string tptDept) method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }


        internal static SortableList<HaulierJobTrip> GetHaulierJobTripsForPlan(PlanHaulierTrip planHaulierTrip,
            PlanHaulierSubTrip planHaulierSubTrip)
        {
            SortableList<HaulierJobTrip> haulierTrips = new SortableList<HaulierJobTrip>();
            try
            {
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT * FROM TPT_PLAN_SUBTRIP_JOB_TBL with (NOLOCK) ";
                    SQLString += "WHERE PLANTRIP_NO = '" + planHaulierTrip.PlanTripNo + "'";
                    SQLString += " AND SEQ_NO = " + planHaulierSubTrip.SeqNo;

                    SQLString += " ORDER BY JOB_ID, SEQUENCE_NO ";//20160201 - gerry added

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        haulierTrips.Add(HaulierJobTrip.GetHaulierJobTrip((int)reader["JOB_ID"], (int)reader["SEQUENCE_NO"]));
                    }
                    cn.Close();
                }
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetHaulierJobTripsForPlan method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return haulierTrips;

        }

        internal static SortableList<PlanHaulierTrip> GetAllPlanHaulierTripsByDay(DateTime date)
        {
            SortableList<PlanHaulierTrip> planHaulierTrips = new SortableList<PlanHaulierTrip>();
            try
            {
                using(SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT TPT_PLAN_TRIP_TBL.* FROM TPT_PLAN_TRIP_TBL with (NOLOCK) ";
                    SQLString += " WHERE TPT_PLAN_TRIP_TBL.SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";//date.ToShortDateString();
                    SQLString += " AND TPT_PLAN_TRIP_TBL.SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";//date.ToShortDateString();

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout =0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        planHaulierTrips.Add(GetPlanHaulierTrip(reader));
                    }
                    cn.Close();
                }
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetAllPlanHaulierTripsByDay(DateTime date) method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planHaulierTrips;
            #region Removed
            /*
                        //instead of using Database, data will be created for testing
                        // create 2 HaulierJobTrips
                        HaulierJobTrip haulierJobTrip1 = new HaulierJobTrip();
                        haulierJobTrip1.ContainerNo = "C1234567890";
                        HaulierJobTrip haulierJobTrip2 = new HaulierJobTrip();
                        haulierJobTrip2.ContainerNo = "D1234567890";

                        // create collections to hold them for each PlanHaulierSubTrip

                        SortableList<HaulierJobTrip> haulierJobTrips1 = new SortableList<HaulierJobTrip>();
                        haulierJobTrips1.Add(haulierJobTrip1);
                        haulierJobTrips1.Add(haulierJobTrip2);

                        SortableList<HaulierJobTrip> haulierJobTrips2 = new SortableList<HaulierJobTrip>();
                        haulierJobTrips2.Add(haulierJobTrip2);


                        //get collection of vehicles

                        SortableList<Vehicle> vehicles = Vehicle.GetAllVehicles();
                        Vehicle vehicle1 = vehicles.ElementAt(0);
                        Vehicle vehicle2 = vehicles.ElementAt(1);
                        Vehicle vehicle3 = vehicles.ElementAt(2);
                        Vehicle vehicle4 = vehicles.ElementAt(3);
                        Vehicle vehicle5 = vehicles.ElementAt(4);

                        //get collection of drivers

                        SortableList<Driver> drivers = Driver.GetAllDrivers();
                        Driver driver1 = drivers.ElementAt(0);
                        Driver driver2 = drivers.ElementAt(1);


                        //Get collection of stops
                        List<Stop> stops = Stop.GetAllStops();
                        Stop stop1 = stops.ElementAt(0);
                        Stop stop2 = stops.ElementAt(1);
                        Stop stop3 = stops.ElementAt(2);

                        // Create 2 PlanHaulierTrips

                        PlanHaulierTrip haulierTrip1 = new PlanHaulierTrip(DateTime.Now, vehicle1, driver1);
                        PlanHaulierTrip haulierTrip2 = new PlanHaulierTrip(DateTime.Now, vehicle3, driver2);

                        //Create 2 PlanHaulierSubTrips


                        PlanHaulierSubTrip planHaulierSubTrip1 = new PlanHaulierSubTrip(1, DateTime.Today + TimeSpan.FromHours(9), DateTime.Today + TimeSpan.FromHours(11),
                          stop1, stop2, true, "Stop1 - first Sub Trip", haulierJobTrips1,  vehicle4, haulierTrip1, JobTripStatus.Assigned);
                        PlanHaulierSubTrip planHaulierSubTrip2 = new PlanHaulierSubTrip(2, DateTime.Today + TimeSpan.FromHours(12), DateTime.Today+TimeSpan.FromHours(15),
                            stop2, stop1, true, "Stop2 - second Sub Trip", haulierJobTrips2, vehicle4, haulierTrip1, JobTripStatus.Assigned);
                        PlanHaulierSubTrip planHaulierSubTrip3 = new PlanHaulierSubTrip(1, DateTime.Today + TimeSpan.FromHours(10), DateTime.Today + TimeSpan.FromHours(13),
                            stop3, stop2, true, "Stop3, first Sub Trip", haulierJobTrips1, vehicle4, haulierTrip2, JobTripStatus.Assigned);
                        PlanHaulierSubTrip planHaulierSubTrip4 = new PlanHaulierSubTrip(2, DateTime.Today + TimeSpan.FromHours(14), DateTime.Today + TimeSpan.FromHours(16),
                            stop3, stop1, true, "Stop3, Second Sub Trip", haulierJobTrips2, vehicle4, haulierTrip2, JobTripStatus.Assigned);


                        // Assign the subTrips to the parent trip

                        haulierTrip1.HaulierSubTrips.Add(planHaulierSubTrip1);
                        haulierTrip1.HaulierSubTrips.Add(planHaulierSubTrip2);


                        haulierTrip2.HaulierSubTrips.Add(planHaulierSubTrip3);
                        haulierTrip2.HaulierSubTrips.Add(planHaulierSubTrip4);

                        SortableList<PlanHaulierTrip> planHaulierTrips = new SortableList<PlanHaulierTrip>();
                        planHaulierTrips.Add(haulierTrip1);
                        planHaulierTrips.Add(haulierTrip2);

                        return planHaulierTrips;
            */
            #endregion
        }

        internal static SortableList<PlanHaulierTrip> GetAllPlanHaulierTripsByDayAndDept(DateTime date, string tptDept)
        {
            SortableList<PlanHaulierTrip> planHaulierTrips = new SortableList<PlanHaulierTrip>();
            try
            {
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT TPT_PLAN_TRIP_TBL.* FROM TPT_PLAN_TRIP_TBL with (NOLOCK) ";
                    SQLString += " inner join TPT_PLAN_DEPT_DRIVER_TBL with (NOLOCK) ";
                    SQLString += " on TPT_PLAN_TRIP_TBL.DRIVER_NO=TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE";
                    SQLString += " and TPT_PLAN_TRIP_TBL.SCHEDULE_DATE=TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE";
                    SQLString += " WHERE TPT_PLAN_TRIP_TBL.SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";//date.ToShortDateString();
                    SQLString += " AND TPT_PLAN_TRIP_TBL.SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";//date.ToShortDateString();
                    SQLString += " and TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE='" + CommonUtilities.FormatString(tptDept) + "'";

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        planHaulierTrips.Add(GetPlanHaulierTrip(reader));
                    }
                    cn.Close();
                }
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetAllPlanHaulierTripsByDayAndDept method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planHaulierTrips;
        }
        //20151130 - gerry added to get specific driver being updated
        internal static PlanHaulierTrip GetPlanHaulierTripByDateAndDriver(string driverCode, DateTime date, string tptDept)
        {
            PlanHaulierTrip planHaulierTrip = new PlanHaulierTrip();
            try
            {
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT TPT_PLAN_TRIP_TBL.* FROM TPT_PLAN_TRIP_TBL with (NOLOCK) ";
                    SQLString += " inner join TPT_PLAN_DEPT_DRIVER_TBL with (NOLOCK) ";
                    SQLString += " on TPT_PLAN_TRIP_TBL.DRIVER_NO=TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE";
                    SQLString += " and TPT_PLAN_TRIP_TBL.SCHEDULE_DATE=TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE";
                    SQLString += " WHERE TPT_PLAN_TRIP_TBL.SCHEDULE_DATE = '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
                    SQLString += " and TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE='" + CommonUtilities.FormatString(tptDept) + "'";
                    SQLString += " and TPT_PLAN_TRIP_TBL.DRIVER_NO='" + CommonUtilities.FormatString(driverCode) + "'";

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        planHaulierTrip = GetPlanHaulierTrip(reader);
                    }
                    cn.Close();
                }
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanHaulierTripByDateAndDriver method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planHaulierTrip;
        }
        //20151130 - gerry added to get specific vehicle being updated
        internal static PlanHaulierTrip GetPlanHaulierTripByDateAndVehicle(string vehicleNo, DateTime date, string tptDept)
        {
            PlanHaulierTrip planHaulierTrip = new PlanHaulierTrip();
            try
            {
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT TPT_PLAN_TRIP_TBL.* FROM TPT_PLAN_TRIP_TBL with (NOLOCK) ";
                    SQLString += " inner join TPT_PLAN_DEPT_DRIVER_TBL with (NOLOCK) ";
                    SQLString += " on TPT_PLAN_TRIP_TBL.DRIVER_NO=TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE";
                    SQLString += " and TPT_PLAN_TRIP_TBL.SCHEDULE_DATE=TPT_PLAN_DEPT_DRIVER_TBL.SCHEDULE_DATE";
                    SQLString += " WHERE TPT_PLAN_TRIP_TBL.SCHEDULE_DATE = '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
                    SQLString += " and TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE='" + CommonUtilities.FormatString(tptDept) + "'";
                    SQLString += " and TPT_PLAN_TRIP_TBL.VEHICLE_NO='" + CommonUtilities.FormatString(vehicleNo) + "'";

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        planHaulierTrip = GetPlanHaulierTrip(reader);
                    }
                    cn.Close();
                }
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanHaulierTripByDateAndVehicle method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planHaulierTrip;
        }
        internal static PlanHaulierTrip GetPlanHaulierTrip(IDataReader reader)
        {
            //System.Globalization.CultureInfo american = new System.Globalization.CultureInfo("en-US");

            PlanHaulierTrip planHaulierTrip = new PlanHaulierTrip(
               (string)reader["PLANTRIP_NO"],
               (DateTime)reader["SCHEDULE_DATE"],
               Vehicle.GetVehicle((string)reader["VEHICLE_NO"]),
               Driver.GetDriver((string)reader["DRIVER_NO"]),
               (byte[])reader["UPDATE_VERSION"]);

            planHaulierTrip.HaulierSubTrips = GetPlanHaulierSubTrips(planHaulierTrip);
            planHaulierTrip.CopyPlanHaulierSubTripsToOldPlanHaulierSubTrips();

            return planHaulierTrip;
        }

        internal static PlanHaulierSubTrips GetPlanHaulierSubTrips(PlanHaulierTrip planHaulierTrip)
        {
            PlanHaulierSubTrips planHaulierSubTrips = new PlanHaulierSubTrips();
            try
            {
                using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    string SQLString = "SELECT * FROM TPT_PLAN_SUBTRIP_TBL with (NOLOCK) ";
                    SQLString += "WHERE PLANTRIP_NO = '" + planHaulierTrip.PlanTripNo + "'";

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        PlanHaulierSubTrip subTrip = GetPlanHaulierSubTrip(reader, planHaulierTrip);
                        if(!(subTrip.Task_ID == "" || subTrip.Task_ID == "0"))
                            subTrip.messageTripStatus = subTrip.IsTripStillInVehicle() ? MessageTripStatus.InProgress : MessageTripStatus.None;
                        planHaulierSubTrips.Add(subTrip);
                    }
                    cn.Close();
                }
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanHaulierSubTrips(PlanHaulierTrip planHaulierTrip) method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return planHaulierSubTrips;
        }

        internal static PlanHaulierSubTrip GetPlanHaulierSubTrip(IDataReader reader, PlanHaulierTrip planHaulierTrip)
        {
            bool isbillable = false;
            string temp = (string)reader["IS_BILLABLE"];
            if (temp == "T")
            { isbillable = true; }
            else { isbillable = false; }

            SortableList<HaulierJobTrip> jobTrips = new SortableList<HaulierJobTrip>();

            try
            {
                PlanHaulierSubTrip planHaulierSubTrip = new PlanHaulierSubTrip(
                   (int)reader["SEQ_NO"],
                   (DateTime)reader["START_TIME"],
                   (DateTime)reader["END_TIME"],
                   Stop.GetStop((string)reader["START_STOP"]),
                   Stop.GetStop((string)reader["END_STOP"]),
                   isbillable,
                   (string)reader["DESCRIPTION"],
                   jobTrips,
                   Vehicle.GetVehicle((string)reader["TRAILER_NO"]),
                   planHaulierTrip,
                   JobTripStatus.Ready
                   );
                //20150708 - gerry added for driver incentives
                planHaulierSubTrip.PlanTripNo = reader["PLANTRIP_NO"] == DBNull.Value ? string.Empty : (string)reader["PLANTRIP_NO"];
                planHaulierSubTrip.IncentiveCode = reader["INCENTIVE_CODE"] == DBNull.Value ? string.Empty : (string)reader["INCENTIVE_CODE"];
                planHaulierSubTrip.HasOT = reader["HAS_OT"] == DBNull.Value ? false : ((string)reader["HAS_OT"] == "F" ? false : true);
                planHaulierSubTrip.IncentiveQty = reader["INCENTIVE_QTY"] == DBNull.Value ? 1 : (decimal)reader["INCENTIVE_QTY"];
                planHaulierSubTrip.IncentiveRemarks = reader["INCENTIVE_REMARKS"] == DBNull.Value ? string.Empty : (string)reader["INCENTIVE_REMARKS"];
                //20170303 - for TruckComm
                planHaulierSubTrip.Task_ID = reader["Task_ID"] == DBNull.Value ? string.Empty : reader["Task_ID"].ToString();

                #region REmoved
                //int msgStatus = reader["MESSAGE_STATUS"] == DBNull.Value ? 10 : Convert.ToInt32(reader["MESSAGE_STATUS"]);
                //switch (msgStatus)
                //{
                //    case 0:
                //        planHaulierSubTrip.messageTripStatus = MessageTripStatus.Received;
                //        break;
                //    case 1:
                //        planHaulierSubTrip.messageTripStatus = MessageTripStatus.Started;
                //        break;
                //    case 2:
                //        planHaulierSubTrip.messageTripStatus = MessageTripStatus.OnGoing;
                //        break;
                //    case 3:
                //        planHaulierSubTrip.messageTripStatus = MessageTripStatus.OnHold;
                //        break;
                //    case 4:
                //        planHaulierSubTrip.messageTripStatus = MessageTripStatus.Cancelled;
                //        break;
                //    case 5:
                //        planHaulierSubTrip.messageTripStatus = MessageTripStatus.InvalidMessage;
                //        break;
                //    case 7:
                //        planHaulierSubTrip.messageTripStatus = MessageTripStatus.Completed;
                //        break;
                //    case 10:
                //        planHaulierSubTrip.messageTripStatus = MessageTripStatus.None;
                //        break;
                //}
                #endregion
                planHaulierSubTrip.JobTrips = GetHaulierJobTripsForPlan(planHaulierTrip, planHaulierSubTrip);
                if (planHaulierSubTrip.JobTrips.Count > 0)
                {
                    planHaulierSubTrip.Status = planHaulierSubTrip.JobTrips[0].TripStatus;
                    //20130415 - gerry added to assign stops to first job trip
                    planHaulierSubTrip.StartStop = planHaulierSubTrip.JobTrips[0].StartStop;
                    planHaulierSubTrip.EndStop = planHaulierSubTrip.JobTrips[0].EndStop;
                    //20130415 end
                    planHaulierSubTrip.containerNos = ((planHaulierSubTrip.JobTrips[0].ContainerNo == string.Empty) ? " - " : planHaulierSubTrip.JobTrips[0].ContainerNo);
                    if (planHaulierSubTrip.JobTrips.Count == 2)
                        planHaulierSubTrip.containerNos += "+" + ((planHaulierSubTrip.JobTrips[1].ContainerNo == string.Empty) ? " - " : planHaulierSubTrip.JobTrips[1].ContainerNo);
                }

                return planHaulierSubTrip;
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanHaulierSubTrip method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool DeletePlanHaulierTripDatabase(PlanHaulierTrip planHaulierTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                byte[] originalRowVersion = new byte[8];

                //20151217 - gerry modified
                string SQLString = "SELECT * FROM TPT_PLAN_TRIP_TBL with (NOLOCK) ";
                SQLString += "WHERE PLANTRIP_NO = '" + planHaulierTrip.PlanTripNo + "'";

                //                string SQLString = @"SELECT * FROM TPT_PLAN_TRIP_TBL
                //                                        where DRIVER_NO = '{0}' AND SCHEDULE_DATE = '{1}'";
                //                SQLString = string.Format(SQLString, CommonUtilities.FormatString(planHaulierTrip.Driver.Code.ToString()),
                //                                                        DateUtility.ConvertDateForSQLPurpose(planHaulierTrip.ScheduleDate.Date));        

                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                if (!SqlBinary.Equals(planHaulierTrip.UpdateVersion, originalRowVersion))
                {
                    string msg = @"Data from driver : ({0}) were updated by someone, please don't do any changes for this driver or else please refresh your screen by pressing refresh button."; //TptResourceUI.WarnPlanModifiedByOtherUser;
                    throw new FMException(string.Format(msg, planHaulierTrip.Driver.DescriptionForPlanningPurpose));//(TptResourceDAL.ErrMultiUserConflict + "\nPlanTransportDAL.DeletePlanHaulierTripDatabase");
                }

                SqlCommand cmd = new SqlCommand("sp_Delete_TPT_PLAN", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planHaulierTrip.PlanTripNo);

                SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;


                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                if (Convert.ToInt32(newReqNumber2.Value) > 1)
                {
                    throw new FMException("Error - More than 1 record deleted");
                }
                //20151027 - gerry added
                if ((!SqlBinary.Equals(new byte[8], planHaulierTrip.UpdateVersion)) && (Convert.ToInt32(newReqNumber2.Value) < 1))
                {   //plantripNo will be gone if someone change the data first
                    string msg = @"Data from driver : ({0}) were updated by someone, please don't do any changes for this driver or else please referesh your screen by pressing refresh button.";
                    throw new FMException(string.Format(msg, planHaulierTrip.Driver.Name));
                }
                retValue = true;
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        internal static bool AddPlanHaulierTripDatabase(PlanHaulierTrip planHaulierTrip, SqlConnection cn, SqlTransaction tran, string tptDept)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_PLAN_TRIP_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                string templanno = "";
                string year = planHaulierTrip.ScheduleDate.Year.ToString();
                string month = planHaulierTrip.ScheduleDate.Month.ToString();
                string day = planHaulierTrip.ScheduleDate.Day.ToString();

                year = year[2].ToString() + year[3].ToString();
                if (month.Length == 1)
                    month = "0" + month;
                if (day.Length == 1)
                    day = "0" + day;
                templanno = "HL" + year + month + day + tptDept;

                //if (planHaulierTrip.PlanTripNo == string.Empty)
                //    planHaulierTrip.PlanTripNo = templanno;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planHaulierTrip.PlanTripNo);
                cmd.Parameters.AddWithValue("@SCHEDULE_DATE", DateUtility.ConvertDateForSQLPurpose(planHaulierTrip.ScheduleDate));
                cmd.Parameters.AddWithValue("@DRIVER_NO", planHaulierTrip.Driver.Code);
                cmd.Parameters.AddWithValue("@VEHICLE_NO", planHaulierTrip.Vehicle.Number);
                cmd.Parameters.AddWithValue("@DEPTCODE", tptDept);

                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.NVarChar);
                newReqNumber.Size = 256;
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                planHaulierTrip.PlanTripNo = newReqNumber.Value.ToString();
                //20150506 - gerry added
                //planHaulierTrip.UpdateVersion = GetPlanHaulierTripUpdateVersion(planHaulierTrip, cn, tran);

                for (int i = 0; i < planHaulierTrip.HaulierSubTrips.Count; i++)
                {
                    AddPlanHaulierSubTripDatabase(planHaulierTrip, planHaulierTrip.HaulierSubTrips[i], cn, tran);
                    AddPlanHaulierSubTripJobDatabase(planHaulierTrip, planHaulierTrip.HaulierSubTrips[i], cn, tran);
                }

            }
            catch (FMException ex) { throw new FMException(ex.Message + " Error PlanTranspotDAL inside AddPlanHaulierTripDatabase method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }
        #region REmoved
        /*
                internal static void SetPlanHaulierTripNo(PlanHaulierTrip planHaulierTrip, SqlConnection cn, SqlTransaction tran)
                {
                    string SQLString = "SELECT count(*) as plantripcount FROM TPT_PLAN_TRIP_TBL ";
                    SQLString += "WHERE SCHEDULE_DATE = '" + DateUtility.ConvertDateForSQLPurpose(planHaulierTrip.ScheduleDate) + "'"; ;

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    IDataReader reader = cmd.ExecuteReader();
                    int plantripcount = 0;
                    while (reader.Read())
                    {
                        plantripcount = (int)reader["plantripcount"];
                    }

                    string templanno = "";
                    string year = planHaulierTrip.ScheduleDate.Year.ToString();
                    string month = planHaulierTrip.ScheduleDate.Month.ToString();
                    string day = planHaulierTrip.ScheduleDate.Day.ToString();

                    year = year[2].ToString() + year[3].ToString();
                    if (month.Length == 1)
                        month = "0" + month;
                    if (day.Length == 1)
                        day = "0" + day;
                    templanno = "HL" + year + month + day;
                    plantripcount = plantripcount + 1;
                    planHaulierTrip.PlanTripNo = templanno + plantripcount.ToString();
                }
        */
        #endregion
        internal static bool AddPlanHaulierSubTripDatabase(PlanHaulierTrip planHaulierTrip, PlanHaulierSubTrip planHaulierSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_PLAN_SUBTRIP_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planHaulierTrip.PlanTripNo);
                cmd.Parameters.AddWithValue("@SEQ_NO", planHaulierSubTrip.SeqNo);
                cmd.Parameters.AddWithValue("@START_TIME", DateUtility.ConvertDateAndTimeForSQLPurpose(planHaulierSubTrip.Start));
                cmd.Parameters.AddWithValue("@END_TIME", DateUtility.ConvertDateAndTimeForSQLPurpose(planHaulierSubTrip.End));
                cmd.Parameters.AddWithValue("@START_STOP", planHaulierSubTrip.StartStop.Code);
                cmd.Parameters.AddWithValue("@END_STOP", planHaulierSubTrip.EndStop.Code);
                cmd.Parameters.AddWithValue("@IS_BILLABLE", planHaulierSubTrip.IsBillableTrip);
                cmd.Parameters.AddWithValue("@DESCRIPTION", planHaulierSubTrip.Description);
                cmd.Parameters.AddWithValue("@TRAILER_NO", planHaulierSubTrip.Trailer.Number);
                cmd.Parameters.AddWithValue("@INCENTIVECODE", planHaulierSubTrip.IncentiveCode);
                cmd.Parameters.AddWithValue("@HAS_OT", planHaulierSubTrip.HasOT ? "T" : "F");
                cmd.Parameters.AddWithValue("@INCENTIVE_QTY", planHaulierSubTrip.IncentiveQty);
                cmd.Parameters.AddWithValue("@INCENTIVE_REMARKS", planHaulierSubTrip.IncentiveRemarks);
                cmd.Parameters.AddWithValue("@TASK_ID", planHaulierSubTrip.Task_ID);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

            }
            catch (FMException ex) { throw new FMException(ex.Message + " Error PlanTranspotDAL inside AddPlanHaulierSubTripDatabase method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }

        internal static bool AddPlanHaulierSubTripJobDatabase(PlanHaulierTrip planHaulierTrip, PlanHaulierSubTrip planHaulierSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                for (int i = 0; i < planHaulierSubTrip.JobTrips.Count; i++)
                {

                    SqlCommand cmd = new SqlCommand("sp_Insert_TPT_PLAN_SUBTRIP_JOB_TBL", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@PLANTRIP_NO", planHaulierTrip.PlanTripNo);
                    cmd.Parameters.AddWithValue("@SEQ_NO", planHaulierSubTrip.SeqNo);
                    cmd.Parameters.AddWithValue("@JOB_ID", planHaulierSubTrip.JobTrips[i].JobID);
                    cmd.Parameters.AddWithValue("@SEQUENCE_NO", planHaulierSubTrip.JobTrips[i].Sequence);
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (FMException ex) { throw new FMException(ex.Message + " Error PlanTranspotDAL inside AddPlanHaulierSubTripJobDatabase method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }

        internal static List<string> GetDriverNamesInPlan(string codes)
        {
            List<string> names = new List<string>();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                string query = @"select distinct tblDRV.Driver_Name from TPT_PLAN_DEPT_DRIVER_TBL as planDRV with (NOLOCK)
                                inner join TPT_Driver_Tbl as tblDRV with (NOLOCK)
                                on tblDRV.Driver_Code = planDRV.DRIVER_CODE
                                where planDRV.DRIVER_CODE in {0}
                                ";
                query = string.Format(query, codes);
                try
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        names.Add(reader.GetString(0).ToString());
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetDriverNamesInPlan method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return names;

        }

        //20151219
        internal static bool HasPlanTripsChanged(Driver driver, DateTime scheduleDate)
        {
            bool retValue = false;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string query = @"select UPDATE_VERSION from TPT_PLAN_TRIP_TBL with (NOLOCK)
                                    where DRIVER_NO = '{0}'
                                    and SCHEDULE_DATE = '{1}'";
                    query = string.Format(query, driver.Code.ToString().Trim()
                                            , DateUtility.ConvertDateForSQLPurpose(scheduleDate));

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    IDataReader reader = cmd.ExecuteReader();
                    byte[] origVersion = null;
                    while (reader.Read())
                    {
                        string msg = @"Data from driver : ({0}) were updated by someone, please don't do any changes for this driver or else please refresh your screen by pressing refresh button."; //TptResourceUI.WarnPlanModifiedByOtherUser;
                        throw new FMException(string.Format(msg, driver.DescriptionForPlanningPurpose));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside HasPlanTripsChanged method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            }
            return retValue;
        }
        //20151219
        internal static bool HasPlanTripsChanged(PlanTrip planTrip)
        {
            bool retValue = false;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    string query = @"select UPDATE_VERSION from TPT_PLAN_TRIP_TBL with (NOLOCK)
                                    where DRIVER_NO = '{0}'
                                    and SCHEDULE_DATE = '{1}'";
                    query = string.Format(query, planTrip.Driver.Code.ToString().Trim()
                                            , DateUtility.ConvertDateForSQLPurpose(planTrip.ScheduleDate));

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;
                    IDataReader reader = cmd.ExecuteReader();
                    byte[] origVersion = null;
                    while (reader.Read())
                    {
                        origVersion = (byte[])reader["UPDATE_VERSION"];
                    }
                    reader.Close();
                    if (!SqlBinary.Equals(planTrip.UpdateVersion, origVersion))
                    {
                        string msg = @"Data from driver : ({0}) were updated by someone, please don't do any changes for this driver or else please refresh your screen by pressing refresh button."; //TptResourceUI.WarnPlanModifiedByOtherUser;
                        throw new FMException(string.Format(msg, planTrip.Driver.DescriptionForPlanningPurpose));
                    }
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            }
            return retValue;
        }
        //20120911
        internal static bool HasPlanTripsChanged(PlanHaulierTrip planHaulierTrip, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                if (con.State == ConnectionState.Closed)
                { con.Open(); }
                string query = @"select UPDATE_VERSION from TPT_PLAN_TRIP_TBL with (NOLOCK)
                                    where DRIVER_NO = '{0}'
                                    and SCHEDULE_DATE = '{1}'";
                query = string.Format(query, planHaulierTrip.Driver.Code.ToString().Trim()
                                        , DateUtility.ConvertDateForSQLPurpose(planHaulierTrip.ScheduleDate));

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                byte[] origVersion = null;
                while (reader.Read())
                {
                    origVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                if (!SqlBinary.Equals(planHaulierTrip.UpdateVersion, origVersion))
                {
                    string msg = @"Data from driver : ({0}) were updated by someone, please don't do any changes for this driver or else please refresh your screen by pressing refresh button."; //TptResourceUI.WarnPlanModifiedByOtherUser;
                    throw new FMException(string.Format(msg, planHaulierTrip.Driver.DescriptionForPlanningPurpose));
                }
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        internal static bool IsNewPlanTrip(PlanHaulierTrip planHaulierTrip, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"select * from TPT_PLAN_TRIP_TBL with (NOLOCK)
                                    where DRIVER_NO = '{0}'
                                    and SCHEDULE_DATE = '{1}'";
                query = string.Format(query, planHaulierTrip.Driver.Code.ToString().Trim()
                                        , DateUtility.ConvertDateForSQLPurpose(planHaulierTrip.ScheduleDate));
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                    retValue = false;
                else
                    retValue = true;
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside IsNewPlanTrip method"); }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        //end 20120911

        //20140516 - gerry added to get the PlanHaulierSubTrip to be use in getting trailer location for sub con
        //20141125 - gerry modified - it is use to determine if how many trip inside
        internal static PlanHaulierSubTrip GetPlanHaulierSubTripFromPreviousLeg(int JobId, int seqNo, out string planTripNo)
        {
            PlanHaulierSubTrip planHaulierSubTrip = null;
            planTripNo = string.Empty;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = @"declare @planTripNo varchar(23), @planTripSeqNo int
                                    Begin
	                                    select @planTripNo = PLANTRIP_NO,
		                                       @planTripSeqNo = SEQ_NO 
	                                    from TPT_PLAN_SUBTRIP_JOB_TBL with (NOLOCK)
		                                    where JOB_ID = {0} and SEQUENCE_NO = {1}
                                    END
                                    BEGIN	
                                    select * from TPT_PLAN_SUBTRIP_TBL with (NOLOCK)
					                    where PLANTRIP_NO = @planTripNo 
                                            and SEQ_NO = @planTripSeqNo
				                      
                                    END";

                    SQLString = string.Format(SQLString, JobId, seqNo);
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    SqlCommand cmd = new SqlCommand(SQLString, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        //assign planTripNo as output parameter because it is not a property of plansubTrip object
                        planTripNo = reader["PLANTRIP_NO"] == DBNull.Value ? string.Empty : (string)reader["PLANTRIP_NO"];
                        PlanHaulierTrip planTrip = new PlanHaulierTrip();
                        planTrip.PlanTripNo = planTripNo; // only plantripNo is needed based on the current method to get GetPlanHaulierSubTrip and GetHaulierJobTripsForPlan
                        planHaulierSubTrip = GetPlanHaulierSubTrip(reader, planTrip);
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanHaulierSubTripFromPreviousLeg method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { planTripNo = string.Empty; throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            }
            return planHaulierSubTrip;
        }

        //20150506 - gerry added to get the lastest updateversion   
        internal static byte[] GetPlanHaulierTripUpdateVersion(PlanHaulierTrip planHaulierTrip)
        {
            byte[] originalRowVersion = new byte[8];
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                try
                {
                    string SQLString = @"SELECT UPDATE_VERSION FROM TPT_PLAN_TRIP_TBL with (NOLOCK)
                                    WHERE PLANTRIP_NO = '{0}'
                                    AND DRIVER_NO = '{1}'";
                    SQLString = string.Format(SQLString, CommonUtilities.FormatString(planHaulierTrip.PlanTripNo), CommonUtilities.FormatString(planHaulierTrip.Driver.Code));
                    SqlCommand cmd1 = new SqlCommand(SQLString, con);
                    cmd1.CommandTimeout = 0;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Transaction = con.BeginTransaction();
                    IDataReader reader = cmd1.ExecuteReader();
                    while (reader.Read())
                    {
                        originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanHaulierTripUpdateVersion method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return originalRowVersion;
        }      
        internal static byte[] GetPlanHaulierTripUpdateVersion(PlanHaulierTrip planHaulierTrip, SqlConnection cn, SqlTransaction tran)
        {
            byte[] originalRowVersion = new byte[8];
            try
            {
                string SQLString = @"SELECT UPDATE_VERSION FROM TPT_PLAN_TRIP_TBL with (NOLOCK)
                                    WHERE PLANTRIP_NO = '{0}'
                                    AND DRIVER_NO = '{1}'";
                SQLString = string.Format(SQLString, CommonUtilities.FormatString(planHaulierTrip.PlanTripNo), CommonUtilities.FormatString(planHaulierTrip.Driver.Code));
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
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanHaulierTripUpdateVersion method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return originalRowVersion;
        }        
        internal static byte[] GetPlanHaulierSubTripUpdateVersion(string planTripNo, PlanHaulierSubTrip planHaulierSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            byte[] originalRowVersion = new byte[8];
            try
            {
                string SQLString = "SELECT UPDATE_VERSION FROM TPT_PLAN_SUBTRIP_TBL with (NOLOCK)";
                SQLString += " WHERE PLANTRIP_NO = '" + CommonUtilities.FormatString(planTripNo) + "'";
                SQLString += " AND SEQ_NO = '" + planHaulierSubTrip.SeqNo + "'";
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
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanHaulierSubTripUpdateVersion method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return originalRowVersion;
        }
        internal static bool IsPlanHaulierTripUpdated(PlanHaulierTrip planHaulierTrip)
        {
            byte[] originalRowVersion = new byte[8];
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                try
                {
                    string SQLString = @"SELECT UPDATE_VERSION FROM TPT_PLAN_TRIP_TBL with (NOLOCK)
                                    WHERE SCHEDULE_DATE = '{0}'
                                    AND DRIVER_NO = '{1}'";
                    SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(planHaulierTrip.ScheduleDate), CommonUtilities.FormatString(planHaulierTrip.Driver.Code));
                    SqlCommand cmd1 = new SqlCommand(SQLString, con);
                    cmd1.CommandTimeout = 0;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Transaction = con.BeginTransaction();
                    IDataReader reader = cmd1.ExecuteReader();
                    while (reader.Read())
                    {
                        originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                        if (!SqlBinary.Equals(planHaulierTrip.UpdateVersion, originalRowVersion))
                        {
                            return true;
                        }
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside IsPlanHaulierTripUpdated method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return false;
        }

        internal static int GetPlanSubTripJobByJobID(int JobID, SqlConnection cn, SqlTransaction tran)
        {
            int result = -1;
            try
            {
                string SQLString = @" SELECT COUNT(*) AS DataCount FROM TPT_PLAN_SUBTRIP_JOB_TBL with (NOLOCK) WHERE JOB_ID = {0} ";

                SQLString = string.Format(SQLString, JobID);
                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                var dr = cmd1.ExecuteReader();
                if (dr.Read())
                    result = dr.GetInt32(0);
                dr.Close();
                return result;
            }
            catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetPlanSubTripJobByJobIDSeq method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        #region Incentives
        //20150710 - gerry added for generate incentive and display when plan sub trip was completed
        internal static List<string> GetIncentiveCodes(string incentiveType)
        {
            List<string> codes = new List<string>();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                string query = @"SELECT distinct d.Incentive_Code from  TRI_Incentive_Rate_Details_Tbl d with (NOLOCK)
                                inner join TRI_Incentive_Rate_Master_Tbl m with (NOLOCK)
                                on m.Incentive_Code = d.Incentive_Code";
                if (!incentiveType.Equals(string.Empty))
                {
                    query += "  where m.Incentive_Type = '{0}'";
                    query = string.Format(query, incentiveType);
                }
                try
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        codes.Add(reader.GetString(0).ToString());
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw new FMException("Error PlanTranspotDAL inside GetIncentiveCodes method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return codes;
        }
        internal static string GenerateDriverIncentive(string planTripNo, PlanHaulierSubTrip planSubTrip, string deptCode, string frmName, string userID)
        {
            string retValue = string.Empty;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    string startLocType = OperationDetail.GetOperationDetail(planSubTrip.StartStop.Code).operationTypeCode;
                    string endLocType = OperationDetail.GetOperationDetail(planSubTrip.EndStop.Code).operationTypeCode;
                    #region Query
                    string query = @" DECLARE @IsDoubleMounted char(1), @TotalAmountValue decimal(10, 2)
                                    ,@IncentiveCode varchar(20) 
	                                ,@FromPremiumZone varchar(20) =''
	                                ,@ToPremiumZone varchar(20)=''
	                                ,@CurrenyCode varchar(20)
	                                ,@IncentiveRate decimal(10, 2)
	                                ,@Qty decimal(10, 2)
	                                ,@IncentiveAmount decimal(10, 2) = 0
	                                ,@FromPremiumRate decimal(10, 2) = 0
	                                ,@FromPremiumType varchar(20) =''
	                                ,@FromPremiumAmount decimal(10, 2) = 0
	                                ,@ToPremiumRate decimal(10, 2) = 0
	                                ,@ToPremiumType varchar(20)=''
	                                ,@ToPremiumAmount decimal(10, 2) = 0
	                                ,@ProcessesDatTime datetime

                                BEGIN
                                    SET @IsDoubleMounted = '{5}'    
                                    SET @IncentiveCode = '{21}'
                                    SET @Qty = '{22}'
                                    IF @IncentiveCode <> ''
                                        BEGIN
                                            select top 1 @IncentiveCode = id.Incentive_Code, @IncentiveRate = id.Rate, @CurrenyCode = id.Currency_Code
	                                        from TRI_Incentive_Rate_Master_Tbl im ,TRI_Incentive_Rate_Details_Tbl id
	                                        where im.Incentive_Code=id.Incentive_Code
	                                        and im.Is_Laden = '{0}'
	                                        and im.Incentive_Type= '{1}'
	                                        and im.Container_Size_Code= '{2}'
												and im.Incentive_Code =  @IncentiveCode		
	                                        and im.Is_DoubleMounting= '{5}'
	                                        and id.Effective_Date =(select max(Effective_Date) from  TRI_Incentive_Rate_Details_Tbl a with (NOLOCK)
							                                        where a.Incentive_Code=im.Incentive_Code and Effective_Date <=getdate())
	                                        and id.Effective_Date<= '{6}'
                                        END
                                    ELSE
                                        BEGIN
                                            select top 1 @IncentiveCode = id.Incentive_Code, @IncentiveRate = id.Rate, @CurrenyCode = id.Currency_Code
	                                        from TRI_Incentive_Rate_Master_Tbl im ,TRI_Incentive_Rate_Details_Tbl id
	                                        where im.Incentive_Code=id.Incentive_Code
	                                        and im.Is_Laden = '{0}'
	                                        and im.Incentive_Type= '{1}'
	                                        and im.Container_Size_Code= '{2}'
	                                        and ((im.Location_Type1 = '{3}' and im.Location_Type2 = '{4}') or
	                                             (im.Location_Type2 = '{3}' and im.Location_Type1 = '{4}'))		
	                                        and im.Is_DoubleMounting= '{5}'
	                                        and id.Effective_Date =(select max(Effective_Date) from  TRI_Incentive_Rate_Details_Tbl a with (NOLOCK)
							                                        where a.Incentive_Code=im.Incentive_Code and Effective_Date <=getdate())
	                                        and id.Effective_Date<= '{6}'
                                        END
                                END
                                BEGIN
                                    select Top 1 @FromPremiumZone = a.Premium_Zone_Code ,@FromPremiumType =a.Premium_Type, @FromPremiumRate = a.Premium_Rate, @FromPremiumAmount = @Qty * a.Premium_Rate 
                                 from TRI_Premium_Zone_Rate_Tbl a,
	                                TRI_Premium_Zone_Master_Tbl b, TRI_Premium_Zone_Location_Tbl d
	                                where a.Premium_Zone_Code=b.Premium_Zone_Code
	                                and a.Premium_Zone_Code = d.Premium_Zone_Code
	                                and d.Location_Code='{7}'
	                                and ((b.Location_Type1 = '{3}' and b.Location_Type2 = '{4}') or
	                                         (b.Location_Type2 = '{3}' and b.Location_Type1 = '{4}'))		  
	                                and a.Effective_Date =(select max(Effective_Date) from  TRI_Premium_Zone_Rate_Tbl c with (NOLOCK)
								                                 where c.Premium_Zone_Code=a.Premium_Zone_Code
								                                  and c.Effective_Date<='{6}')  
                                END
                                BEGIN 
                                  select Top 1  @ToPremiumZone = a.Premium_Zone_Code, @ToPremiumType =a.Premium_Type, @ToPremiumRate = a.Premium_Rate, @ToPremiumAmount = @Qty * a.Premium_Rate 
                                from TRI_Premium_Zone_Rate_Tbl a,
	                                TRI_Premium_Zone_Master_Tbl b, TRI_Premium_Zone_Location_Tbl d
	                                where a.Premium_Zone_Code=b.Premium_Zone_Code
	                                and a.Premium_Zone_Code = d.Premium_Zone_Code
	                                and d.Location_Code='{8}'
	                                and ((b.Location_Type1 = '{3}' and b.Location_Type2 = '{4}') or
	                                         (b.Location_Type2 = '{3}' and b.Location_Type1 = '{4}'))		  
	                                and a.Effective_Date =(select max(Effective_Date) from  TRI_Premium_Zone_Rate_Tbl c with (NOLOCK)
								                                 where c.Premium_Zone_Code=a.Premium_Zone_Code
								                                  and c.Effective_Date<='{6}')  
                                END

							if @IsDoubleMounted = 'T'
							BEGIN 
								SET @FromPremiumAmount = @FromPremiumAmount * 2
								SET @ToPremiumAmount = @ToPremiumAmount * 2
							END

                            if @IncentiveCode <> NULL OR @IncentiveCode <> ''
                            BEGIN
                                BEGIN
                                    delete from TRI_Driver_Incentive_Tbl
                                    where Incentive_Date = '{6}'
                                    and Incentive_Code = @IncentiveCode
                                    and Driver_Code = '{9}'
                                    --and PlanTrip_SeqNo = '{11}'
                                    and Job_ID = '{12}'
                                    and Sequence_No = '{13}'
                                END
                                BEGIN
                                --save date if needed to save; if not then will be remove
                                INSERT INTO [dbo].[TRI_Driver_Incentive_Tbl]
                                            ([Incentive_Date]
                                            ,[Driver_Code]
                                            ,[PlanTrip_No]
                                            ,[PlanTrip_SeqNo]
                                            ,[Job_ID]
                                            ,[Sequence_No]
                                            ,[Incentive_Code]
                                            ,[Incentive_Type]
                                            ,[From_Premium_Zone]
                                            ,[To_Premium_Zone]
                                            ,[Currency_Code]
                                            ,[Incentive_Rate]
                                            ,[Qty]
                                            ,[Incentive_Amount]
                                            ,[From_Premium_Rate]
                                            ,[From_Premium_Type]
                                            ,[From_Premium_Amount]
                                            ,[To_Premium_Rate]
                                            ,[To_Premium_Type]
                                            ,[To_Premium_Amount]
                                            ,[Process_DateTime]
                                            ,[Remarks]
                                            ,[Container_Number]
                                            ,[From_Location]
                                            ,[To_Location]
                                            ,[Start_Time]
                                            ,[End_Time]
                                            ,[TPT_Dept_Code]
                                            ,[Vehicle_No]
                                            ,[Trailer_No]
                                            ,[Job_Number])
                                        VALUES
                                            ('{6}'
                                            ,'{9}'
                                            ,'{10}'
                                            ,'{11}'
                                            ,'{12}'
                                            ,'{13}'
                                            ,@IncentiveCode
                                            ,'{1}'
                                            ,@FromPremiumZone
                                            ,@ToPremiumZone
                                            ,@CurrenyCode
                                            ,@IncentiveRate
                                            ,@Qty
                                            ,@IncentiveRate * @Qty
                                            ,ISNULL(@FromPremiumRate,0)
                                            ,@FromPremiumType
                                            ,ISNULL(@FromPremiumAmount,0)
                                            ,ISNULL(@ToPremiumRate,0)
                                            ,@ToPremiumType
                                            ,ISNULL(@ToPremiumAmount,0)
                                            ,GETDATE()
                                            ,'{14}'
                                            ,'{15}'
                                            ,'{7}'
                                            ,'{8}'
                                            ,'{16}'
                                            ,'{17}'
                                            ,'{18}'
                                            ,'{19}'
                                            ,'{20}'
                                            ,'{23}')
                                    END
                                END
                                ";
                    #endregion

                    bool isLaden = false;
                    foreach (HaulierJobTrip trip in planSubTrip.JobTrips)
                    {
                        isLaden = trip.IsLaden;
                    }
                    //contSize = planSubTrip.JobTrips.Count > 1 ? "20''" : CommonUtilities.FormatString(planSubTrip.JobTrips[0].ContainerCode.Substring(0, 2) + "'");
                    //string contSize = TransportFacadeIn.GetContainerSizeByContainerCode(planSubTrip.JobTrips[0].ContainerCode);
                    string incentiveType = planSubTrip.IncentiveCode != string.Empty ? "MN" : (planSubTrip.Start.DayOfWeek == DayOfWeek.Sunday ? "AS" : "AN");
                    string contSize = incentiveType == "MN" ? "-" : TransportFacadeIn.GetContainerSizeByContainerCode(planSubTrip.JobTrips[0].ContainerCode);
                    if (incentiveType != "MN")
                        contSize = planSubTrip.JobTrips.Count > 1 ? "20''" : CommonUtilities.FormatString(contSize);
                    string jobNo = planSubTrip.JobTrips[0].JobNo;
                    string contNo = planSubTrip.JobTrips[0].ContainerNo;
                    if (planSubTrip.JobTrips.Count > 1)
                    {
                        contNo += contNo.Equals(string.Empty) ? planSubTrip.JobTrips[1].ContainerNo : "," + planSubTrip.JobTrips[1].ContainerNo;
                        if (planSubTrip.JobTrips[0].JobNo != planSubTrip.JobTrips[1].JobNo)
                            jobNo += "," + planSubTrip.JobTrips[1].JobNo.Substring(planSubTrip.JobTrips[1].JobNo.Length - 10, 10);
                    }
                    #region Parameters
                    query = string.Format(query, isLaden ? "T" : "F", //{0}
                                                 incentiveType,//{1}
                                                 contSize, //{2}
                                                 planSubTrip.IncentiveCode == string.Empty ? CommonUtilities.FormatString(startLocType) : string.Empty,//{3}
                                                 planSubTrip.IncentiveCode == string.Empty ? CommonUtilities.FormatString(endLocType) : string.Empty,//{4}
                                                 planSubTrip.JobTrips.Count > 1 ? "T" : "F",//{5}
                                                 DateUtility.ConvertDateForSQLPurpose(planSubTrip.Start),//{6}
                                                 CommonUtilities.FormatString(planSubTrip.StartStop.Code),//{7}
                                                 CommonUtilities.FormatString(planSubTrip.EndStop.Code),//{8}
                                                 CommonUtilities.FormatString(planSubTrip.DriverNumber),//{9}
                                                 planTripNo,//{10}
                                                 planSubTrip.SeqNo,//{11}
                                                 planSubTrip.JobTrips[0].JobID,  ////{12}
                                                 planSubTrip.JobTrips[0].Sequence, ////13}
                                                 planSubTrip.IncentiveRemarks == string.Empty ? CommonUtilities.FormatString("Incentive from " + planSubTrip.StartStop.Code + " - " + planSubTrip.EndStop.Code)
                                                                                                : CommonUtilities.FormatString(planSubTrip.IncentiveRemarks), //{14}
                                                 CommonUtilities.FormatString(contNo),//{15}
                                                 DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planSubTrip.Start),//{16}
                                                 DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planSubTrip.End),//{17}
                                                 deptCode, //{18}
                                                 planSubTrip.VehicleNumber, //{19}
                                                 planSubTrip.Trailer.Number,//{20}
                                                 planSubTrip.IncentiveCode,//{21}
                                                 planSubTrip.IncentiveQty,//{22}
                                                 CommonUtilities.FormatString(jobNo));//{23}
                    #endregion
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    cmd.Transaction = tran;
                    int obj = cmd.ExecuteNonQuery();
                    if (obj == -1)
                        retValue = "No incentive setup detected for this route! ";
                    else
                    {
                        //Incentive incentive = GetActualIncentive(planSubTrip, con, tran);
                        //20151109 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(planTripNo, "TR", "PL", planSubTrip.SeqNo, userID, DateTime.Now, frmName, planSubTrip.SeqNo, FormMode.Add.ToString());
                        auditLog.WriteAuditLog(planSubTrip, null, con, tran);
                    }

                    tran.Commit();
                }
                catch (FMException fmEx) { if (tran != null) { tran.Rollback(); } throw new FMException("Error PlanTranspotDAL inside GenerateDriverIncentive method"); }
                catch (SqlException ex) { if (tran != null) { tran.Rollback(); } throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { if (tran != null) { tran.Rollback(); } throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { if (tran != null) { tran.Rollback(); } throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return retValue;
        }
        internal static bool GenerateDriverOtherIncentive(string planTripNo, PlanHaulierSubTrip planSubTrip, Incentive otherIncentive, string deptCode, string frmName, string userID)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    #region Query
                    string query = @" BEGIN
                                    delete from TRI_Driver_Incentive_Tbl
                                    where Incentive_Code = '{0}'
                                    and Incentive_Date = '{1}'
                                    and Driver_Code = '{2}'
                                    --and PlanTrip_SeqNo = '{4}'
                                    and Job_ID = '{5}'
                                    and Sequence_No = '{6}'

                                --save date if needed to save; if not then will be remove
                                INSERT INTO [dbo].[TRI_Driver_Incentive_Tbl]
                                            ([Incentive_Date]
                                            ,[Driver_Code]
                                            ,[PlanTrip_No]
                                            ,[PlanTrip_SeqNo]
                                            ,[Job_ID]
                                            ,[Sequence_No]
                                            ,[Incentive_Code]
                                            ,[Incentive_Type]
                                            ,[From_Premium_Zone]
                                            ,[To_Premium_Zone]
                                            ,[Currency_Code]
                                            ,[Incentive_Rate]
                                            ,[Qty]
                                            ,[Incentive_Amount]
                                            ,[From_Premium_Rate]
                                            ,[From_Premium_Type]
                                            ,[From_Premium_Amount]
                                            ,[To_Premium_Rate]
                                            ,[To_Premium_Type]
                                            ,[To_Premium_Amount]
                                            ,[Process_DateTime]
                                            ,[Remarks]
                                            ,[Container_Number]
                                            ,[From_Location]
                                            ,[To_Location]
                                            ,[Start_Time]
                                            ,[End_Time]
                                            ,[TPT_Dept_Code]
                                            ,[Vehicle_No]
                                            ,[Trailer_No]
                                            ,[Job_Number])

                                        VALUES
                                            ('{1}'
                                            ,'{2}'
                                            ,'{3}'
                                            ,'{4}'
                                            ,'{5}'
                                            ,'{6}'
                                            ,'{0}'
                                            ,'MN'
                                            ,''
                                            ,''
                                            ,'{17}'
                                            ,'{18}'
                                            ,'{7}'
                                            ,'{19}'
                                            ,0
                                            ,''
                                            ,0
                                            ,0
                                            ,''
                                            ,0
                                            ,GETDATE()
                                            ,'{8}'
                                            ,'{9}'
                                            ,'{10}'
                                            ,'{11}'
                                            ,'{12}'
                                            ,'{13}'
                                            ,'{14}'
                                            ,'{15}'
                                            ,'{16}'
                                            ,'{20}')
                                    END
                                ";
                    #endregion
                    #region Parameters
                    query = string.Format(query, CommonUtilities.FormatString(otherIncentive.Code), //{0}
                                                 DateUtility.ConvertDateForSQLPurpose(planSubTrip.Start),//{1}
                                                 planSubTrip.DriverNumber, //{2}
                                                 planTripNo,//{3}
                                                 planSubTrip.SeqNo,//{4}
                                                 planSubTrip.JobTrips[0].JobID,  //{5}
                                                 planSubTrip.JobTrips[0].Sequence, //6}
                                                 otherIncentive.PayableQty,//{7}
                                                 CommonUtilities.FormatString(otherIncentive.Remarks),//{8}
                                                 planSubTrip.JobTrips[0].ContainerNo,//{9}
                                                 planSubTrip.StartStop.Code,//{10}
                                                 planSubTrip.EndStop.Code,//{11}
                                                 DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planSubTrip.Start),//{12}
                                                 DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planSubTrip.End),//{13}
                                                 deptCode, //{14}
                                                 planSubTrip.VehicleNumber,//{15}
                                                 planSubTrip.Trailer.Number,//{16}
                                                 otherIncentive.Currency,//{17}
                                                 otherIncentive.Rate,//{18}
                                                 otherIncentive.IncentiveAmount,//{19}
                                                 planSubTrip.JobTrips[0].JobNo);//{20}
                    #endregion
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    //20151109 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(planTripNo, "TR", "PL", planSubTrip.SeqNo, userID, DateTime.Now, frmName, planSubTrip.SeqNo, FormMode.Add.ToString());
                    auditLog.WriteAuditLog(planSubTrip, null, con, tran);

                    tran.Commit();
                }
                catch (FMException fmEx) { if (tran != null) { tran.Rollback(); } throw fmEx; }
                catch (SqlException ex) { if (tran != null) { tran.Rollback(); } throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { if (tran != null) { tran.Rollback(); } throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { if (tran != null) { tran.Rollback(); } throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return true;
        }
        internal static string GenerateDriverOtherIncentive(string planTripNo, PlanHaulierSubTrip planSubTrip, Incentive otherIncentive, string deptCode, SqlConnection con, SqlTransaction tran, string frmName, string userID)
        {
            string retValue = string.Empty;
            try
            {
                #region Query
                string query = @"  DECLARE @TotalAmountValue decimal(10, 2), @Qty decimal(10,2)
                                    ,@IncentiveCode varchar(20) 
	                                ,@CurrenyCode varchar(20)
	                                ,@IncentiveRate decimal(10, 2)

                                BEGIN   
									SET @IncentiveCode = '{0}'
									SET @Qty = '{7}'
								    SET @TotalAmountValue = @IncentiveRate * @Qty
								    SELECT @IncentiveRate as rate, @Qty as qty, @TotalAmountValue as Tamount
                                END
								BEGIN
                                    delete from TRI_Driver_Incentive_Tbl
                                    where Incentive_Code = @IncentiveCode
                                    and Incentive_Date = '{1}'
                                    and Driver_Code = '{2}'
                                    --and PlanTrip_SeqNo = '{4}'
                                    and Job_ID = '{5}'
                                    and Sequence_No = '{6}'
                                END
								 BEGIN
                                --save date if needed to save; if not then will be remove
                                INSERT INTO [dbo].[TRI_Driver_Incentive_Tbl]
                                            ([Incentive_Date]
                                            ,[Driver_Code]
                                            ,[PlanTrip_No]
                                            ,[PlanTrip_SeqNo]
                                            ,[Job_ID]
                                            ,[Sequence_No]
                                            ,[Incentive_Code]
                                            ,[Incentive_Type]
                                            ,[From_Premium_Zone]
                                            ,[To_Premium_Zone]
                                            ,[Currency_Code]
                                            ,[Incentive_Rate]
                                            ,[Qty]
                                            ,[Incentive_Amount]
                                            ,[From_Premium_Rate]
                                            ,[From_Premium_Type]
                                            ,[From_Premium_Amount]
                                            ,[To_Premium_Rate]
                                            ,[To_Premium_Type]
                                            ,[To_Premium_Amount]
                                            ,[Process_DateTime]
                                            ,[Remarks]
                                            ,[Container_Number]
                                            ,[From_Location]
                                            ,[To_Location]
                                            ,[Start_Time]
                                            ,[End_Time]
                                            ,[TPT_Dept_Code]
                                            ,[Vehicle_No]
                                            ,[Trailer_No]
                                            ,[Job_Number])

                                        VALUES
                                            ('{1}'
                                            ,'{2}'
                                            ,'{3}'
                                            ,'{4}'
                                            ,'{5}'
                                            ,'{6}'
                                            ,@IncentiveCode
                                            ,'MN'
                                            ,''
                                            ,''
                                            ,@CurrenyCode
                                            ,@IncentiveRate
                                            ,@Qty
                                            ,@TotalAmountValue
                                            ,0
                                            ,''
                                            ,0
                                            ,0
                                            ,''
                                            ,0
                                            ,GETDATE()
                                            ,'{8}'
                                            ,'{9}'
                                            ,'{10}'
                                            ,'{11}'
                                            ,'{12}'
                                            ,'{13}'
                                            ,'{14}'
                                            ,'{15}'
                                            ,'{16}'
                                            ,'{17}')
                                    END
                                ";
                #endregion
                #region Parameters
                query = string.Format(query, CommonUtilities.FormatString(otherIncentive.Code), //{0}
                                             DateUtility.ConvertDateForSQLPurpose(planSubTrip.Start),//{1}
                                             planSubTrip.DriverNumber, //{2}
                                             planTripNo,//{3}
                                             planSubTrip.SeqNo,//{4}
                                             planSubTrip.JobTrips[0].JobID,  //{5}
                                             planSubTrip.JobTrips[0].Sequence, //6}
                                             otherIncentive.PayableQty,//{7}
                                             CommonUtilities.FormatString(otherIncentive.Remarks),//{8}
                                             planSubTrip.JobTrips[0].ContainerNo,//{9}
                                             CommonUtilities.FormatString(planSubTrip.StartStop.Code),//{10}
                                             CommonUtilities.FormatString(planSubTrip.EndStop.Code),//{11}
                                             DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planSubTrip.Start),//{12}
                                             DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(planSubTrip.End),//{13}
                                             deptCode, //{14}
                                             planSubTrip.VehicleNumber,//{15}
                                             planSubTrip.Trailer.Number,//{16}
                                             planSubTrip.JobTrips[0].JobNo);//{17}
                #endregion
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                int obj = cmd.ExecuteNonQuery();
                if (obj == -1)
                    retValue = "No incentive setup detected for this route! ";
                else
                {
                    //20151109 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(planTripNo, "TR", "PL", planSubTrip.SeqNo, userID, DateTime.Now, frmName, planSubTrip.SeqNo, FormMode.Add.ToString());
                    auditLog.WriteAuditLog(planSubTrip, null, con, tran);
                }
            }
            catch (FMException fmEx) { throw new FMException("Error PlanTranspotDAL inside GenerateDriverOtherIncentive method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static Incentive GetActualIncentive(PlanHaulierSubTrip planSubTrip)
        {
            Incentive incentive = new Incentive();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    #region query
                    string query = @"    SELECT inc.Incentive_Code, im.Description, inc.Incentive_Type, inc.Incentive_Date, inc.Incentive_Rate, inc.Qty, inc.Incentive_Amount,inc.Remarks,
	                                         inc.From_Premium_Amount,inc.To_Premium_Amount, inc.Currency_Code
                                     FROM TRI_Driver_Incentive_Tbl inc with (NOLOCK)
                                     inner join TRI_Incentive_Rate_Master_Tbl im with (NOLOCK) on im.Incentive_Code = inc.Incentive_Code 
                                      where inc.incentive_Date ='{0}'
                                      and inc.Driver_Code = '{1}'
                                ";
                    query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(planSubTrip.Start),
                                                planSubTrip.DriverNumber);
                    if (planSubTrip.IncentiveCode == string.Empty)
                    {
                        query += @" and inc.Job_Id = '{0}' AND inc.Sequence_No = '{1}' AND inc.Incentive_Type = '{2}' ";
                        query = string.Format(query, planSubTrip.JobTrips[0].JobID, planSubTrip.JobTrips[0].Sequence,
                                                 planSubTrip.Start.DayOfWeek == DayOfWeek.Sunday ? "AS" : "AN");
                    }
                    else
                    {
                        query += @" and inc.Incentive_Code = '{0}' AND inc.Incentive_Type = '{1}' ";
                        query = string.Format(query, planSubTrip.IncentiveCode, "MN");
                    }
                    #endregion

                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        incentive = new Incentive();
                        incentive.Code = reader["Incentive_Code"] == DBNull.Value ? string.Empty : (string)reader["Incentive_Code"];
                        incentive.Desc = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"];
                        incentive.Type = reader["Incentive_Type"] == DBNull.Value ? string.Empty : (string)reader["Incentive_Type"];
                        incentive.Currency = reader["Currency_Code"] == DBNull.Value ? string.Empty : (string)reader["Currency_Code"];
                        incentive.EffectiveDate = reader["Incentive_Date"] == DBNull.Value ? DateTime.Today : (DateTime)reader["Incentive_Date"];
                        incentive.Rate = reader["Incentive_Rate"] == DBNull.Value ? 0 : (decimal)reader["Incentive_Rate"];
                        incentive.PayableQty = reader["Qty"] == DBNull.Value ? 0 : (decimal)reader["Qty"];
                        incentive.IncentiveAmount = reader["Incentive_Amount"] == DBNull.Value ? 0 : (decimal)reader["Incentive_Amount"];
                        incentive.FromPremiumAmount = reader["From_Premium_Amount"] == DBNull.Value ? 0 : (decimal)reader["From_Premium_Amount"];
                        incentive.ToPremiumAmount = reader["To_Premium_Amount"] == DBNull.Value ? 0 : (decimal)reader["To_Premium_Amount"];
                        incentive.Remarks = reader["Remarks"] == DBNull.Value ? string.Empty : (string)reader["Remarks"];
                    }
                    reader.Close();
                }
                catch (FMException fmEx) { throw new FMException("Error PlanTranspotDAL inside GetActualIncentive method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return incentive;
        
        }
        internal static Incentive GetActualIncentive(PlanHaulierSubTrip planSubTrip, SqlConnection con, SqlTransaction tran)
        {
            Incentive incentive = new Incentive();
            try
            {
                #region query
                string query = @"    SELECT inc.Incentive_Code, im.Description, inc.Incentive_Type, inc.Incentive_Date, inc.Incentive_Rate, inc.Qty, inc.Incentive_Amount,inc.Remarks,
	                                         inc.From_Premium_Amount,inc.To_Premium_Amount, inc.Currency_Code
                                     FROM TRI_Driver_Incentive_Tbl inc with (NOLOCK)
                                     inner join TRI_Incentive_Rate_Master_Tbl im with (NOLOCK) on im.Incentive_Code = inc.Incentive_Code 
                                      where inc.incentive_Date ='{0}'
                                      and inc.Driver_Code = '{1}'
                                ";
                query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(planSubTrip.Start),
                                            planSubTrip.DriverNumber);
                if (planSubTrip.IncentiveCode == string.Empty)
                {
                    query += @" and inc.Job_Id = '{0}' AND inc.Sequence_No = '{1}' AND inc.Incentive_Type = '{2}' ";
                    query = string.Format(query, planSubTrip.JobTrips[0].JobID, planSubTrip.JobTrips[0].Sequence,
                                             planSubTrip.Start.DayOfWeek == DayOfWeek.Sunday ? "AS" : "AN");
                }
                else
                {
                    query += @" and inc.Incentive_Code = '{0}' AND inc.Incentive_Type = '{1}' ";
                    query = string.Format(query, planSubTrip.IncentiveCode, "MN");
                }
                #endregion

                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    incentive = new Incentive();
                    incentive.Code = reader["Incentive_Code"] == DBNull.Value ? string.Empty : (string)reader["Incentive_Code"];
                    incentive.Desc = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"];
                    incentive.Type = reader["Incentive_Type"] == DBNull.Value ? string.Empty : (string)reader["Incentive_Type"];
                    incentive.Currency = reader["Currency_Code"] == DBNull.Value ? string.Empty : (string)reader["Currency_Code"];
                    incentive.EffectiveDate = reader["Incentive_Date"] == DBNull.Value ? DateTime.Today : (DateTime)reader["Incentive_Date"];
                    incentive.Rate = reader["Incentive_Rate"] == DBNull.Value ? 0 : (decimal)reader["Incentive_Rate"];
                    incentive.PayableQty = reader["Qty"] == DBNull.Value ? 0 : (decimal)reader["Qty"];
                    incentive.IncentiveAmount = reader["Incentive_Amount"] == DBNull.Value ? 0 : (decimal)reader["Incentive_Amount"];
                    incentive.FromPremiumAmount = reader["From_Premium_Amount"] == DBNull.Value ? 0 : (decimal)reader["From_Premium_Amount"];
                    incentive.ToPremiumAmount = reader["To_Premium_Amount"] == DBNull.Value ? 0 : (decimal)reader["To_Premium_Amount"];
                    incentive.Remarks = reader["Remarks"] == DBNull.Value ? string.Empty : (string)reader["Remarks"];
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw new FMException("Error PlanTranspotDAL inside GetActualIncentive method"); }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return incentive;

        }
        internal static Incentive GetIncentiveWithRateDetails(string incentiveCode, DateTime scheduleDate)
        {
            Incentive incentive = new Incentive();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    #region query
                    string query = @"  SELECT * FROM TRI_Incentive_Rate_Details_Tbl id with (NOLOCK)
                                    inner join TRI_Incentive_Rate_Master_Tbl im with (NOLOCK)
                                    on im.Incentive_Code = id.Incentive_Code
                                        where id.Incentive_Code = '{0}'
                                        and id.Effective_Date <= '{1}'
                                ";
                    query = string.Format(query, CommonUtilities.FormatString(incentiveCode), DateUtility.ConvertDateForSQLPurpose(scheduleDate));
                    #endregion

                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        incentive = new Incentive();
                        incentive.Code = reader["Incentive_Code"] == DBNull.Value ? string.Empty : (string)reader["Incentive_Code"];
                        incentive.Desc = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"];
                        incentive.Type = reader["Incentive_Type"] == DBNull.Value ? string.Empty : (string)reader["Incentive_Type"];
                        incentive.Currency = reader["Currency_Code"] == DBNull.Value ? string.Empty : (string)reader["Currency_Code"];
                        incentive.Rate = reader["Rate"] == DBNull.Value ? 0 : (decimal)reader["Rate"];
                    }
                    reader.Close();
                }
                catch (FMException fmEx) { throw new FMException("Error PlanTranspotDAL inside GetIncentiveWithRateDetails method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return incentive;
        }   
        internal static List<Incentive> GetOtherIncentives(PlanHaulierSubTrip planSubTrip)
        {
            List<Incentive> incentives = new List<Incentive>();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    #region query
                    string query = @"  SELECT inc.Incentive_Code, im.Description, inc.Incentive_Type, inc.Incentive_Date, inc.Incentive_Rate, inc.Qty, inc.Incentive_Amount,inc.Remarks,
	                                         inc.From_Premium_Amount,inc.To_Premium_Amount, inc.Currency_Code
                                     FROM TRI_Driver_Incentive_Tbl inc with (NOLOCK)
                                     inner join TRI_Incentive_Rate_Master_Tbl im with (NOLOCK) on im.Incentive_Code = inc.Incentive_Code 
                                      where inc.incentive_Date ='{0}'
                                      and inc.Driver_Code = '{1}'
                                      and inc.Job_Id = '{2}'
                                      and inc.Sequence_No = '{3}' 
                                      and inc.Incentive_Type = 'MN' 
                                ";
                    query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(planSubTrip.Start),
                                                planSubTrip.DriverNumber, planSubTrip.JobTrips[0].JobID, planSubTrip.JobTrips[0].Sequence);
                    #endregion

                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Incentive otherIncentive = new Incentive();
                        otherIncentive.Code = reader["Incentive_Code"] == DBNull.Value ? string.Empty : (string)reader["Incentive_Code"];
                        otherIncentive.Desc = reader["Description"] == DBNull.Value ? string.Empty : (string)reader["Description"];
                        otherIncentive.Type = reader["Incentive_Type"] == DBNull.Value ? string.Empty : (string)reader["Incentive_Type"];
                        otherIncentive.Currency = reader["Currency_Code"] == DBNull.Value ? string.Empty : (string)reader["Currency_Code"];
                        otherIncentive.EffectiveDate = reader["Incentive_Date"] == DBNull.Value ? DateTime.Today : (DateTime)reader["Incentive_Date"];
                        otherIncentive.Rate = reader["Incentive_Rate"] == DBNull.Value ? 0 : (decimal)reader["Incentive_Rate"];
                        otherIncentive.PayableQty = reader["Qty"] == DBNull.Value ? 0 : (decimal)reader["Qty"];
                        otherIncentive.IncentiveAmount = reader["Incentive_Amount"] == DBNull.Value ? 0 : (decimal)reader["Incentive_Amount"];
                        otherIncentive.FromPremiumAmount = reader["From_Premium_Amount"] == DBNull.Value ? 0 : (decimal)reader["From_Premium_Amount"];
                        otherIncentive.ToPremiumAmount = reader["To_Premium_Amount"] == DBNull.Value ? 0 : (decimal)reader["To_Premium_Amount"];
                        otherIncentive.Remarks = reader["Remarks"] == DBNull.Value ? string.Empty : (string)reader["Remarks"];

                        incentives.Add(otherIncentive);
                    }
                    reader.Close();
                }
                catch (FMException fmEx) { throw new FMException("Error PlanTranspotDAL inside GetOtherIncentives method"); }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return incentives;

        }
        internal static bool DeleteIncentive(PlanHaulierSubTrip planSubTrip, string frmName, string userID)
        {
            using(SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    #region query
                    string query = @" DELETE  FROM TRI_Driver_Incentive_Tbl 
                                      where incentive_Date ='{0}'
                                      and Driver_Code = '{1}'
                                      and Job_Id = '{2}'
                                      and Sequence_No = '{3}'  
                                ";
                    query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(planSubTrip.Start),
                                                planSubTrip.DriverNumber, planSubTrip.JobTrips[0].JobID, planSubTrip.JobTrips[0].Sequence);
                    #endregion

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    //20151109 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(planSubTrip.IncentiveCode, "TR", "PL", planSubTrip.SeqNo, userID, DateTime.Now, frmName, planSubTrip.SeqNo, FormMode.Delete.ToString());
                    auditLog.WriteAuditLog(null, null, con, tran);

                    tran.Commit();
                }
                catch (FMException fmEx) { tran.Rollback(); throw new FMException("Error PlanTranspotDAL inside DeleteIncentive method"); }
                catch (SqlException ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return true;
            
        }
        internal static bool DeleteIncentiveByCode(PlanHaulierSubTrip planSubTrip, Incentive otherIncentive, string frmName, string userID)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    #region query
                    string query = @" DELETE  FROM TRI_Driver_Incentive_Tbl 
                                      where incentive_Date ='{0}'
                                      and Driver_Code = '{1}'
                                      and Job_Id = '{2}'
                                      and Sequence_No = '{3}'  
                                      and Incentive_Code = '{4}'
                                ";
                    query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(planSubTrip.Start),
                                                 planSubTrip.DriverNumber, planSubTrip.JobTrips[0].JobID, planSubTrip.JobTrips[0].Sequence,
                                                 CommonUtilities.FormatString(otherIncentive.Code));
                    #endregion

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandTimeout = 0;
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                    //20151109 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(otherIncentive.Code, "TR", "PL", planSubTrip.SeqNo, userID, DateTime.Now, frmName, planSubTrip.SeqNo, FormMode.Delete.ToString());
                    auditLog.WriteAuditLog(null, null, con, tran);

                    tran.Commit();
                }
                catch (FMException fmEx) { tran.Rollback(); throw new FMException("Error PlanTranspotDAL inside DeleteIncentiveByCode method"); }
                catch (SqlException ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                finally { con.Close(); }
            }
            return true;
        }
        #endregion

        //20170303 - gerry added method to capture the task id used in TruckComm
        internal static bool UpdatePlanHaulierSubTripTaskID(PlanHaulierSubTrip planTruckSubTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TPT_PLAN_SUBTRIP_TBL
                                            set Task_ID = '{2}'
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
    }
}
