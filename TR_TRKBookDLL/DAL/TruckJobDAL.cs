using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_HLBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using System.Collections;
using TR_LanguageResource.Resources;
// 2013-10-08 Zhou Kai adds
using FM.TR_MarketDLL.BLL;

namespace FM.TR_TRKBookDLL.DAL
{
    class TruckJobDAL
    {
        const string LAST_MINUTE_OF_DAY = " 23:59:00";

        internal static string GetPrefix()
        {
            string prefix = "";
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());

                string SQLString = "SELECT TRUCK_JOB_NO_PREFIX FROM TPT_SPECIAL_DATA_Tbl with (NOLOCK)";

                SqlCommand cmd3 = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd3.ExecuteReader();
                while (reader.Read())
                {
                    prefix = (string)reader["TRUCK_JOB_NO_PREFIX"];
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return prefix;
        }

        // Chong Chin 27 Aug 10. Add out TruckJob parameter for NUNIT test

        internal static string AddTruckJobHeader(TruckJob truckJob, SqlConnection cn, SqlTransaction tran, string prefix, out TruckJob truckJobOut)
        {
            try
            {
                //SqlCommand cmd2 = new SqlCommand("sp_Insert_AM_Special_Job_Number_Tbl_ForTR", cn);
                SqlCommand cmd2 = new SqlCommand("sp_Insert_TPT_Special_Job_Number", cn);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Year_Code", truckJob.JobDateTime.Year.ToString());
                cmd2.Parameters.AddWithValue("@Month", truckJob.JobDateTime.Month);
                cmd2.Parameters.AddWithValue("@Job_Type_Code", prefix);

                cmd2.Transaction = tran;
                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd2.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd2.ExecuteNonQuery();
                int temprunningno = Convert.ToInt32(newReqNumber.Value);


                // form the bookingNo 
                truckJob.JobNo = prefix +
                    truckJob.BranchCode +
                    truckJob.JobDateTime.Year.ToString().Substring(2, 2) +
                    truckJob.JobDateTime.Month.ToString("D2") +
                    temprunningno.ToString("D4") +
                    "00";

                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_JOB_HEADER", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", truckJob.JobID);
                cmd.Parameters.AddWithValue("@CUST_CODE", truckJob.CustNo);
                cmd.Parameters.AddWithValue("@TRIP_TYPE", truckJob.TripType.ToString());
                cmd.Parameters.AddWithValue("@BOOKING_NUMBER", truckJob.JobNo);
                cmd.Parameters.AddWithValue("@JOB_NUMBER", truckJob.JobNo);
                cmd.Parameters.AddWithValue("@JOB_DATETIME", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.JobDateTime.Date));
                cmd.Parameters.AddWithValue("@BRANCH_CODE", truckJob.BranchCode);
                cmd.Parameters.AddWithValue("@SHIPPER_CODE", truckJob.shipperCode);
                cmd.Parameters.AddWithValue("@SHIPPER_NAME", truckJob.shipperName);
                cmd.Parameters.AddWithValue("@SHIPPER_ADD1", truckJob.shipperAdd1);
                cmd.Parameters.AddWithValue("@SHIPPER_ADD2", truckJob.shipperAdd2);
                cmd.Parameters.AddWithValue("@SHIPPER_ADD3", truckJob.shipperAdd3);
                cmd.Parameters.AddWithValue("@SHIPPER_ADD4", truckJob.shipperAdd4);
                cmd.Parameters.AddWithValue("@CONSIGNEE_CODE", truckJob.consigneeCode);
                cmd.Parameters.AddWithValue("@CONSIGNEE_NAME", truckJob.consigneeName);
                cmd.Parameters.AddWithValue("@CONSIGNEE_ADD1", truckJob.consigneeAdd1);
                cmd.Parameters.AddWithValue("@CONSIGNEE_ADD2", truckJob.consigneeAdd2);
                cmd.Parameters.AddWithValue("@CONSIGNEE_ADD3", truckJob.consigneeAdd3);
                cmd.Parameters.AddWithValue("@CONSIGNEE_ADD4", truckJob.consigneeAdd4);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", truckJob.subcontractorCode);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_NAME", truckJob.subcontractorName);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD1", truckJob.subcontractorAdd1);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD2", truckJob.subcontractorAdd2);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD3", truckJob.subcontractorAdd3);
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD4", truckJob.subcontractorAdd4);
                cmd.Parameters.AddWithValue("@PSA_ACCOUNT", truckJob.psaAccount);
                cmd.Parameters.AddWithValue("@WAREHOUSE_NO", truckJob.warehouseNo);
                cmd.Parameters.AddWithValue("@YOUR_REF_NO", truckJob.yourRefNo);
                cmd.Parameters.AddWithValue("@UCR_NO", truckJob.ucrNo);
                cmd.Parameters.AddWithValue("@PERMIT_NO1", truckJob.permitNo1);
                cmd.Parameters.AddWithValue("@DATE_PERMIT_NO1", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.datePermitNo1));
                cmd.Parameters.AddWithValue("@PERMIT_NO2", truckJob.permitNo2);
                cmd.Parameters.AddWithValue("@DATE_PERMIT_NO2", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.datePermitNo2));
                cmd.Parameters.AddWithValue("@REMARKS", truckJob.remarks);
                cmd.Parameters.AddWithValue("@REMARKS2", truckJob.remarks2);
                cmd.Parameters.AddWithValue("@REMARKS3", truckJob.remarks3);
                cmd.Parameters.AddWithValue("@REMARKS4", truckJob.remarks4);
                cmd.Parameters.AddWithValue("@REMARKS5", truckJob.remarks5);
                cmd.Parameters.AddWithValue("@JOB_STATUS", truckJob.JobStatus);
                cmd.Parameters.AddWithValue("@CARRIER_AGENT", truckJob.carrierAgent);
                cmd.Parameters.AddWithValue("@OBL_NO", truckJob.oblNo);
                cmd.Parameters.AddWithValue("@HBL_NO", truckJob.hblNo);
                cmd.Parameters.AddWithValue("@VESSEL_NO", truckJob.vesselNo);
                cmd.Parameters.AddWithValue("@VOYAGE_NO", truckJob.voyageNo);
                cmd.Parameters.AddWithValue("@POL", truckJob.pol);
                cmd.Parameters.AddWithValue("@POD", truckJob.pod);
                cmd.Parameters.AddWithValue("@ETA", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.eta));
                cmd.Parameters.AddWithValue("@ETD", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.etd));
                cmd.Parameters.AddWithValue("@QUOTATION_NO", truckJob.QuotationNo);
                int status = 0;
                if (truckJob.JobUrgencyType == JobUrgencyType.Normal)
                    status = 1;
                if (truckJob.JobUrgencyType == JobUrgencyType.Important)
                    status = 2;
                if (truckJob.JobUrgencyType == JobUrgencyType.Urgent)
                    status = 3;
                cmd.Parameters.AddWithValue("@JOB_URGENCY_TYPE", status);
                //status = 0;
                //if (truckJob.TruckJobChargeType == ChargeType.StopDependent)
                //    status = 1;
                ////if (truckJob.truckChargeType == TruckChargeType.SectorDependent)
                ////    status = 2;
                //if (truckJob.TruckJobChargeType == ChargeType.None)
                //    status = 2;
                cmd.Parameters.AddWithValue("@TRUCK_CHARGE_TYPE", truckJob.TruckJobChargeType.GetHashCode());

                cmd.Parameters.AddWithValue("@SOURCE_REF_NUMBER", truckJob.SourceReferenceNo);
                if (truckJob.BillByTransport == true)
                    cmd.Parameters.AddWithValue("@BILL_BY_TRANSPORT", "T");
                else
                    cmd.Parameters.AddWithValue("@BILL_BY_TRANSPORT", "F");
                cmd.Parameters.AddWithValue("@TPT_DEPT_CODE", truckJob.TptDeptCode);

                cmd.Parameters.AddWithValue("@ORIGIN", truckJob.origin);
                cmd.Parameters.AddWithValue("@DESTINATION", truckJob.destination);
                cmd.Parameters.AddWithValue("@VIA", truckJob.via);
                cmd.Parameters.AddWithValue("@FLIGHTNO", truckJob.flightNo);
                cmd.Parameters.AddWithValue("@HAWB", truckJob.HAWB);
                cmd.Parameters.AddWithValue("@MAWB", truckJob.MAWB);
                cmd.Parameters.AddWithValue("@AWB_NO", truckJob.AWBNo);
                cmd.Parameters.AddWithValue("@FLIGHT_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.flightDate));
                //20130923 -Gerry added new fields for JLV invoice and custom declaration
                cmd.Parameters.AddWithValue("@COMMERCIAL_INVOICE_NO", truckJob.commercialInvoice);
                cmd.Parameters.AddWithValue("@CUSTOM_DECLARATION_NO", truckJob.customsDeclaration);
                // 2014-04-10 Zhou Kai adds
                //cmd.Parameters.AddWithValue("@FREIGHT_TYPE", truckJob.Freight_Type.ToString());
                cmd.Parameters.AddWithValue("@ISBILLABLE", truckJob.IsBillable ? 'T' : 'F');
                // 2014-06-18 Zhou Kai ends
                cmd.Parameters.AddWithValue("@JOB_TYPE", truckJob.JobType);
                //20160304 - gerry added
                cmd.Parameters.AddWithValue("@Added_By", truckJob.JobCreatorId.ToString());
                cmd.Parameters.AddWithValue("@Booked_DateTime", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.JobCreatedDateTime));
                cmd.Parameters.AddWithValue("@IsFlexibleTime", truckJob.isFlexibleTime ? 'T' : 'F');

                SqlParameter newReqNumber2 = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                truckJob.JobID = Convert.ToInt32(newReqNumber2.Value);

                // Chong Chin 27 Aug 10 Now retrieve and assign to output parameter
                //truckJobOut = null;
                truckJobOut = GetTruckJob(truckJob.JobNo, cn, tran);
                return truckJob.JobNo;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static byte[] GetHeaderUpdateVersion(int jobId)
        {
            byte[] updateVersion = new byte[8];
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = "SELECT UPDATE_VERSION FROM TRK_JOB_MAIN_Tbl with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + jobId;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd1 = new SqlCommand(SQLString, con);
                cmd1.CommandType = CommandType.Text;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    updateVersion = reader["UPDATE_VERSION"] == DBNull.Value ? new byte[8] : (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { con.Close(); }
            return updateVersion;

        }
        internal static bool EditTruckHeader(TruckJob truckJob, SqlConnection cn, SqlTransaction tran, out TruckJob truckJobOut)
        {
            try
            {
                byte[] originalRowVersion = GetHeaderUpdateVersion(truckJob.JobID);
                if (SqlBinary.Equals(truckJob.UpdateVersion, originalRowVersion) == false)
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nTruckJobDAL.EditTruckHeader");
                }
                else
                {
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    SqlCommand cmd = new SqlCommand("sp_Edit_TRK_JOB_HEADER", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JOB_ID", truckJob.JobID);
                    cmd.Parameters.AddWithValue("@CUST_CODE", truckJob.CustNo);
                    cmd.Parameters.AddWithValue("@TRIP_TYPE", truckJob.TripType.ToString()); //--20160408 - gerry renamed from @JOB_TYPE
                    cmd.Parameters.AddWithValue("@BOOKING_NUMBER", truckJob.JobNo);
                    cmd.Parameters.AddWithValue("@JOB_NUMBER", truckJob.JobNo);
                    cmd.Parameters.AddWithValue("@JOB_DATETIME", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.JobDateTime));
                    cmd.Parameters.AddWithValue("@BRANCH_CODE", truckJob.BranchCode);
                    cmd.Parameters.AddWithValue("@SHIPPER_CODE", truckJob.shipperCode);
                    cmd.Parameters.AddWithValue("@SHIPPER_NAME", truckJob.shipperName);
                    cmd.Parameters.AddWithValue("@SHIPPER_ADD1", truckJob.shipperAdd1);
                    cmd.Parameters.AddWithValue("@SHIPPER_ADD2", truckJob.shipperAdd2);
                    cmd.Parameters.AddWithValue("@SHIPPER_ADD3", truckJob.shipperAdd3);
                    cmd.Parameters.AddWithValue("@SHIPPER_ADD4", truckJob.shipperAdd4);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_CODE", truckJob.consigneeCode);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_NAME", truckJob.consigneeName);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_ADD1", truckJob.consigneeAdd1);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_ADD2", truckJob.consigneeAdd2);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_ADD3", truckJob.consigneeAdd3);
                    cmd.Parameters.AddWithValue("@CONSIGNEE_ADD4", truckJob.consigneeAdd4);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", truckJob.subcontractorCode);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_NAME", truckJob.subcontractorName);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD1", truckJob.subcontractorAdd1);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD2", truckJob.subcontractorAdd2);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD3", truckJob.subcontractorAdd3);
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD4", truckJob.subcontractorAdd4);
                    cmd.Parameters.AddWithValue("@PSA_ACCOUNT", truckJob.psaAccount);
                    cmd.Parameters.AddWithValue("@WAREHOUSE_NO", truckJob.warehouseNo);
                    cmd.Parameters.AddWithValue("@YOUR_REF_NO", truckJob.yourRefNo);
                    cmd.Parameters.AddWithValue("@UCR_NO", truckJob.ucrNo);
                    cmd.Parameters.AddWithValue("@PERMIT_NO1", truckJob.permitNo1);
                    cmd.Parameters.AddWithValue("@DATE_PERMIT_NO1", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.datePermitNo1));
                    cmd.Parameters.AddWithValue("@PERMIT_NO2", truckJob.permitNo2);
                    cmd.Parameters.AddWithValue("@DATE_PERMIT_NO2", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.datePermitNo2));
                    cmd.Parameters.AddWithValue("@REMARKS", truckJob.remarks);
                    cmd.Parameters.AddWithValue("@REMARKS2", truckJob.remarks2);
                    cmd.Parameters.AddWithValue("@REMARKS3", truckJob.remarks3);
                    cmd.Parameters.AddWithValue("@REMARKS4", truckJob.remarks4);
                    cmd.Parameters.AddWithValue("@REMARKS5", truckJob.remarks5);
                    cmd.Parameters.AddWithValue("@JOB_STATUS", truckJob.JobStatus);
                    cmd.Parameters.AddWithValue("@CARRIER_AGENT", truckJob.carrierAgent);
                    cmd.Parameters.AddWithValue("@OBL_NO", truckJob.oblNo);
                    cmd.Parameters.AddWithValue("@HBL_NO", truckJob.hblNo);
                    cmd.Parameters.AddWithValue("@VESSEL_NO", truckJob.vesselNo);
                    cmd.Parameters.AddWithValue("@VOYAGE_NO", truckJob.voyageNo);
                    cmd.Parameters.AddWithValue("@POL", truckJob.pol);
                    cmd.Parameters.AddWithValue("@POD", truckJob.pod);
                    cmd.Parameters.AddWithValue("@ETA", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.eta));
                    cmd.Parameters.AddWithValue("@ETD", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJob.etd));
                    cmd.Parameters.AddWithValue("@QUOTATION_NO", truckJob.QuotationNo);

                    int status = 0;
                    if (truckJob.JobUrgencyType == JobUrgencyType.Normal)
                        status = 1;
                    if (truckJob.JobUrgencyType == JobUrgencyType.Important)
                        status = 2;
                    if (truckJob.JobUrgencyType == JobUrgencyType.Urgent)
                        status = 3;
                    cmd.Parameters.AddWithValue("@JOB_URGENCY_TYPE", status);
                    //status = 0;
                    //if (truckJob.TruckJobChargeType == ChargeType.StopDependent)
                    //    status = 2;
                    ////if (truckJob.truckChargeType == TruckChargeType.SectorDependent)
                    ////    status = 2;
                    //if (truckJob.TruckJobChargeType == ChargeType.None)
                    //    status = 1;
                    cmd.Parameters.AddWithValue("@TRUCK_CHARGE_TYPE", truckJob.TruckJobChargeType.GetHashCode());

                    cmd.Parameters.AddWithValue("@SOURCE_REF_NUMBER", truckJob.SourceReferenceNo);
                    if (truckJob.BillByTransport == true)
                        cmd.Parameters.AddWithValue("@BILL_BY_TRANSPORT", "T");
                    else
                        cmd.Parameters.AddWithValue("@BILL_BY_TRANSPORT", "F");
                    cmd.Parameters.AddWithValue("@TPT_DEPT_CODE", truckJob.TptDeptCode);
                    truckJob.OldTptDeptCode = truckJob.TptDeptCode;

                    cmd.Parameters.AddWithValue("@ORIGIN", truckJob.origin);
                    cmd.Parameters.AddWithValue("@DESTINATION", truckJob.destination);
                    cmd.Parameters.AddWithValue("@VIA", truckJob.via);
                    cmd.Parameters.AddWithValue("@FLIGHTNO", truckJob.flightNo);
                    cmd.Parameters.AddWithValue("@HAWB", truckJob.HAWB);
                    cmd.Parameters.AddWithValue("@MAWB", truckJob.MAWB);
                    cmd.Parameters.AddWithValue("@AWB_NO", truckJob.AWBNo);
                    cmd.Parameters.AddWithValue("@FLIGHT_DATE", truckJob.flightDate);
                    //20130923 -Gerry added new fields for JLV invoice and custom declaration
                    cmd.Parameters.AddWithValue("@COMMERCIAL_INVOICE_NO", truckJob.commercialInvoice);
                    cmd.Parameters.AddWithValue("@CUSTOM_DECLARATION_NO", truckJob.customsDeclaration);
                    // 2014-04-10 Zhou Kai adds
                    //cmd.Parameters.AddWithValue("@FREIGHT_TYPE", truckJob.Freight_Type.ToString());
                    cmd.Parameters.AddWithValue("@ISBILLABLE", truckJob.IsBillable ? 'T' : 'F');
                    // 2014-06-18 Zhou Kai ends
                    cmd.Parameters.AddWithValue("@JOB_TYPE", truckJob.JobType);
                    cmd.Parameters.AddWithValue("@IsFlexibleTime", truckJob.isFlexibleTime ? 'T' : 'F');
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();


                    // Chong Chin 31 Aug Now retrieve and assign to output parameter
                    //truckJobOut = null;
                    truckJobOut = GetTruckJob(truckJob.JobNo, cn, tran);

                    return true;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool DeleteTruckJob(TruckJob truckJob, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_JOB_HEADER", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", truckJob.JobID);

                SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool ChangeDriver(string planTripNo, int jobId, int jobSeq, int jobtripPlanSeq, string newPlanTripNo, Driver newDriver, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                //SqlCommand cmd = new SqlCommand("sp_CHANGE_DRIVER_AND_VEHICLE_FOR_1_TRK_JOBTRIP", cn);
                SqlCommand cmd = new SqlCommand("sp_Edit_TRK_CHANGE_DRIVER_AND_VEHICLE_FOR_1_JOBTRIP", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTripNo);
                cmd.Parameters.AddWithValue("@JOB_ID", jobId);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", jobSeq);
                cmd.Parameters.AddWithValue("@JOBTRIP_PLAN_SEQ", jobtripPlanSeq);
                cmd.Parameters.AddWithValue("@DRIVER_NO", newDriver.Code);
                cmd.Parameters.AddWithValue("@VEHICLE_NO", newDriver.defaultVehicleNumber);
                cmd.Parameters.AddWithValue("@NEWPLANTRIP_NO", newPlanTripNo);

                SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;


                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                //27 Jan 2012 - Gerry Remove, not all PlanTruckSubTripJobs are to be inserted into TRK_JOB_DETAIL_TRIP_PLAN_TBL
                //we got error when transfer planTruckSubTripJob from preceeding planTruckSubTrip to newPlanTruckSubTrip
                //if (Convert.ToInt32(newReqNumber2.Value) == 0)
                //{
                //    throw new FMException("Error - No record updated on TRK_JOB_DETAIL_TRIP_PLAN_TBL");
                //}


                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool ChangeDrivers(string planTripNo, Driver newDriver, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                //SqlCommand cmd = new SqlCommand("sp_CHANGE_DRIVER_AND_VEHICLE_TRK_JOBTRIP", cn);
                SqlCommand cmd = new SqlCommand("sp_Edit_TRK_CHANGE_DRIVER_AND_VEHICLE_JOBTRIP", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", planTripNo);
                cmd.Parameters.AddWithValue("@DRIVER_NO", newDriver.Code);
                cmd.Parameters.AddWithValue("@VEHICLE_NO", newDriver.defaultVehicleNumber);


                SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;


                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                if (Convert.ToInt32(newReqNumber2.Value) == 0)
                {
                    throw new FMException("Error - No record updated on TRK_JOB_DETAIL_TRIP_PLAN_TBL");
                }


                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }


        internal static bool DeleteTruckJobTripPlan(TruckJobTripPlan truckJobTripPlan, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_JOB_DETAIL_TRIP_PLAN_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PLANTRIP_NO", truckJobTripPlan.planTripNo);
                cmd.Parameters.AddWithValue("@JOB_ID", truckJobTripPlan.jobID);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", truckJobTripPlan.sequence);
                cmd.Parameters.AddWithValue("@PLANSUBTRIP_SEQ_NO", truckJobTripPlan.planSubTripSeqNo);
                cmd.Parameters.AddWithValue("@PLANSUBTRIPJOB_SEQ_NO", truckJobTripPlan.planSubTripJobSeqNo);

                SqlParameter newReqNumber2 = new SqlParameter("@RowCount", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                // 2014-03-28 Zhou Kai adds
                if (Convert.ToInt32(newReqNumber2.Value) == 0)
                {
                    throw new FMException("Error - No Record Deleted");
                }

                if (Convert.ToInt32(newReqNumber2.Value) > 1)
                {
                    throw new FMException("Error - More than 1 record deleted");
                }
                // 2014-03-28 Zhou Kai ends
                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }


        internal static bool AddTruckJobTripDetail(TruckJobTrip truckJobTrip, TruckJobTripDetail truckJobTripDetail, SqlConnection cn, SqlTransaction tran, out TruckJobTripDetail truckJobTripDetailOut)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_JOB_DETAIL_TRIP_DETAIL_Tbl", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@JOB_ID", truckJobTrip.JobID);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", truckJobTrip.Sequence);
                cmd.Parameters.AddWithValue("@SEQ_NO", 0);
                cmd.Parameters.AddWithValue("@UOM", truckJobTripDetail.uom);
                cmd.Parameters.AddWithValue("@QTY", truckJobTripDetail.quantity);
                cmd.Parameters.AddWithValue("@UNIT_WEIGHT", truckJobTripDetail.unitWeight);
                cmd.Parameters.AddWithValue("@TOT_WEIGHT", truckJobTripDetail.totalWeight);
                cmd.Parameters.AddWithValue("@LENGTH", truckJobTripDetail.length);
                cmd.Parameters.AddWithValue("@WIDTH", truckJobTripDetail.width);
                cmd.Parameters.AddWithValue("@HEIGHT", truckJobTripDetail.height);
                cmd.Parameters.AddWithValue("@VOLUME", truckJobTripDetail.volume);
                cmd.Parameters.AddWithValue("@MARKING", truckJobTripDetail.marking);
                cmd.Parameters.AddWithValue("@CARGO_DESC", truckJobTripDetail.cargoDescription);
                cmd.Parameters.AddWithValue("@REMARKS", truckJobTripDetail.remarks);
                cmd.Parameters.AddWithValue("@REF_NO", truckJobTripDetail.ref_No);//20170616

                SqlParameter newReqNumber2 = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber2);
                newReqNumber2.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                truckJobTripDetail.detailSequence = Convert.ToInt32(newReqNumber2.Value);

                truckJobTripDetailOut = GetTruckJobTripDetail(truckJobTrip.JobID, truckJobTrip.Sequence, truckJobTripDetail.detailSequence, cn, tran);

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool AddTruckJobTripPlan(TruckJobTripPlan truckJobTripPlan, SqlConnection cn, SqlTransaction tran, out TruckJobTripPlan outTruckJobTripPlan)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_JOB_DETAIL_TRIP_PLAN_TBL", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SCHEDULE_DATE", DateUtility.ConvertDateForSQLPurpose(truckJobTripPlan.scheduleDate));
                cmd.Parameters.AddWithValue("@JOB_ID", truckJobTripPlan.jobID);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", truckJobTripPlan.sequence);
                cmd.Parameters.AddWithValue("@PLANTRIP_NO", truckJobTripPlan.planTripNo);
                cmd.Parameters.AddWithValue("@PLANSUBTRIP_SEQ_NO", truckJobTripPlan.planSubTripSeqNo);
                cmd.Parameters.AddWithValue("@PLANSUBTRIPJOB_SEQ_NO", truckJobTripPlan.planSubTripJobSeqNo);
                cmd.Parameters.AddWithValue("@DRIVER_NO", truckJobTripPlan.driver);
                cmd.Parameters.AddWithValue("@VEHICLE_NO", truckJobTripPlan.truckNo);
                cmd.Parameters.AddWithValue("@VOLUME", truckJobTripPlan.volume);
                cmd.Parameters.AddWithValue("@WEIGHT", truckJobTripPlan.weight);
                cmd.Parameters.AddWithValue("@START_TIME", truckJobTripPlan.start);
                cmd.Parameters.AddWithValue("@END_TIME", truckJobTripPlan.end);
                cmd.Parameters.AddWithValue("@REMARKS", "");
                int status = 0;
                if (truckJobTripPlan.status == JobTripStatus.Booked)
                    status = 1;
                if (truckJobTripPlan.status == JobTripStatus.Ready)
                    status = 2;
                if (truckJobTripPlan.status == JobTripStatus.Assigned)
                    status = 3;
                if (truckJobTripPlan.status == JobTripStatus.Completed)
                    status = 4;
                if (truckJobTripPlan.status == JobTripStatus.Invoiced)
                    status = 5;
                cmd.Parameters.AddWithValue("@STATUS", status);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                //truckJobTripPlan.planSubTripJobSeqNo = Convert.ToInt32(newReqNumber.Value);

                // 2014-04-02 Zhou Kai adds comments:
                // the function belwo gets the latest truckJobTripPlan from database including the update_version as the
                // outTruckJobTripPlan, but, outTruckJobTripPlan is for testing only!
                // The original truckJobTripPlan, which will be used in memory in future is not updated! Including update_version.
                // That caused  multiuser-conflicts in the logic hereafter.
                outTruckJobTripPlan = GetTruckJobTripPlan(truckJobTripPlan.jobID, truckJobTripPlan.sequence,
                    truckJobTripPlan.planTripNo, truckJobTripPlan.planSubTripSeqNo, truckJobTripPlan.planSubTripJobSeqNo, truckJobTripPlan.driver, truckJobTripPlan.truckNo, cn, tran);
                // 2014-04-02 Zhou Kai adds this line below.
                truckJobTripPlan.updateVersion = outTruckJobTripPlan.updateVersion;
                // 2014-04-02 Zhou Kai ends comments
                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool EditTruckJobTripDetail(TruckJobTrip truckJobTrip, TruckJobTripDetail truckJobTripDetail, SqlConnection cn, SqlTransaction tran, out TruckJobTripDetail truckJobTripDetailOut)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("sp_Edit_TRK_JOB_DETAIL_TRIP_DETAIL_Tbl", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@JOB_ID", truckJobTrip.JobID);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", truckJobTrip.Sequence);
                cmd.Parameters.AddWithValue("@SEQ_NO", truckJobTripDetail.detailSequence);
                cmd.Parameters.AddWithValue("@UOM", truckJobTripDetail.uom);
                cmd.Parameters.AddWithValue("@QTY", truckJobTripDetail.quantity);
                cmd.Parameters.AddWithValue("@UNIT_WEIGHT", truckJobTripDetail.unitWeight);
                cmd.Parameters.AddWithValue("@TOT_WEIGHT", truckJobTripDetail.totalWeight);
                cmd.Parameters.AddWithValue("@LENGTH", truckJobTripDetail.length);
                cmd.Parameters.AddWithValue("@WIDTH", truckJobTripDetail.width);
                cmd.Parameters.AddWithValue("@HEIGHT", truckJobTripDetail.height);
                cmd.Parameters.AddWithValue("@VOLUME", truckJobTripDetail.volume);
                cmd.Parameters.AddWithValue("@MARKING", truckJobTripDetail.marking);
                cmd.Parameters.AddWithValue("@CARGO_DESC", truckJobTripDetail.cargoDescription);
                cmd.Parameters.AddWithValue("@REMARKS", truckJobTripDetail.remarks);


                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                truckJobTripDetailOut = GetTruckJobTripDetail(truckJobTrip.JobID, truckJobTrip.Sequence, truckJobTripDetail.detailSequence, cn, tran);


                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool DeleteTruckJobTripDetails(TruckJobTrip truckJobTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string query = @" DELETE FROM TRK_JOB_DETAIL_TRIP_DETAIL_Tbl
                                  WHERE JOB_ID = {0}
                                  AND JOBTRIP_SEQ_NO = {1} ";
                query = string.Format(query, truckJobTrip.JobID, truckJobTrip.Sequence);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException("TruckJobDAL Error : DeleteTruckJobTripDetails. " + ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException("TruckJobDAL Error : DeleteTruckJobTripDetails. " + ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException("TruckJobDAL Error : DeleteTruckJobTripDetails. " + ex.Message.ToString()); }
            return true;
        }

        internal static bool DeleteTruckJobTripDetail(TruckJobTrip truckJobTrip, TruckJobTripDetail truckJobTripDetail, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_JOB_DETAIL_TRIP_DETAIL_Tbl", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", truckJobTrip.JobID);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", truckJobTrip.Sequence);
                cmd.Parameters.AddWithValue("@SEQ_NO", truckJobTripDetail.detailSequence);

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
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }

        internal static bool AddTruckJobTrip(TruckJob truckJob, TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran, out TruckJobTrip truckJobTripOut)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_JOB_DETAIL_TRIP_Tbl", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", truckJob.JobID);
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", 0);
                cmd.Parameters.AddWithValue("@FROM_STOP", truckJobTrip.StartStop.Code);
                cmd.Parameters.AddWithValue("@FROM_ADD1", truckJobTrip.StartStop.Address1);
                cmd.Parameters.AddWithValue("@FROM_ADD2", truckJobTrip.StartStop.Address2);
                cmd.Parameters.AddWithValue("@FROM_ADD3", truckJobTrip.StartStop.Address3);
                cmd.Parameters.AddWithValue("@FROM_ADD4", truckJobTrip.StartStop.Address4);

                cmd.Parameters.AddWithValue("@FROM_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJobTrip.StartDate));
                cmd.Parameters.AddWithValue("@FROM_TIME", truckJobTrip.StartTime);
                cmd.Parameters.AddWithValue("@TO_STOP", truckJobTrip.EndStop.Code);
                cmd.Parameters.AddWithValue("@TO_ADD1", truckJobTrip.EndStop.Address1);
                cmd.Parameters.AddWithValue("@TO_ADD2", truckJobTrip.EndStop.Address2);
                cmd.Parameters.AddWithValue("@TO_ADD3", truckJobTrip.EndStop.Address3);
                cmd.Parameters.AddWithValue("@TO_ADD4", truckJobTrip.EndStop.Address4);

                cmd.Parameters.AddWithValue("@TO_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJobTrip.EndDate));
                cmd.Parameters.AddWithValue("@TO_TIME", truckJobTrip.EndTime);
                cmd.Parameters.AddWithValue("@BOOKED_VOLUME", truckJobTrip.BookedVol);
                cmd.Parameters.AddWithValue("@BOOKED_WEIGHT", truckJobTrip.BookedWeight);
                //@MAX_CBM decimal(24, 8),
                //@MAX_WEIGHT decimal(24, 8),
                cmd.Parameters.AddWithValue("@CARGO_DESCRIPTION", truckJobTrip.CargoDescription);
                if (truckJobTrip.IsLaden == true)
                {
                    cmd.Parameters.AddWithValue("@IS_LADEN", "T");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IS_LADEN", "F");
                }
                int status = 0;
                if (truckJobTrip.TripStatus == JobTripStatus.Booked)
                    status = 1;
                if (truckJobTrip.TripStatus == JobTripStatus.Ready)
                    status = 2;
                if (truckJobTrip.TripStatus == JobTripStatus.Assigned)
                    status = 3;
                if (truckJobTrip.TripStatus == JobTripStatus.Completed)
                    status = 4;
                if (truckJobTrip.TripStatus == JobTripStatus.Invoiced)
                    status = 5;
                cmd.Parameters.AddWithValue("@STATUS", status);

                if (truckJobTrip.IsDangerousGoods == true)
                    cmd.Parameters.AddWithValue("@IS_DANGEROUS_GOODS", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_DANGEROUS_GOODS", "F");
                cmd.Parameters.AddWithValue("@DG_REMARKS", truckJobTrip.DGRemarks);
                if (truckJobTrip.IsOversize == true)
                    cmd.Parameters.AddWithValue("@IS_OVERSIZE", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_OVERSIZE", "F");
                cmd.Parameters.AddWithValue("@OVERSIZE_REMARKS", truckJobTrip.OversizeRemarks);
                if (truckJobTrip.IsDirectDelivery == true)
                    cmd.Parameters.AddWithValue("@IS_DIRECT_DELIVERY", "T");
                else
                    cmd.Parameters.AddWithValue("@IS_DIRECT_DELIVERY", "F");
                if (truckJobTrip.OwnTransport == true)
                    cmd.Parameters.AddWithValue("@OWNTRANSPORT", "T");
                else
                    cmd.Parameters.AddWithValue("@OWNTRANSPORT", "F");
                //20140312-gerry replaced
                //cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", truckJobTrip.SubContractorCode);
                //201040502 - gerry change subCon from CustomerDTO to operatorDTO
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", truckJobTrip.subCon.Code.ToString());
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_NAME", truckJobTrip.subCon.Name.ToString());
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD1", truckJobTrip.subCon.Add1.ToString());
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD2", truckJobTrip.subCon.Add2.ToString());
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD3", truckJobTrip.subCon.Add3.ToString());
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD4", truckJobTrip.subCon.Add4.ToString());
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CITY_CODE", truckJobTrip.subCon.City.ToString());
                //20140312 end
                cmd.Parameters.AddWithValue("@SUBCONTRACTOR_REFERENCE", truckJobTrip.SubContractorReference);
                cmd.Parameters.AddWithValue("@CHARGE_CODE", truckJobTrip.ChargeCode);
                cmd.Parameters.AddWithValue("@REMARKS", truckJobTrip.Remarks);
                cmd.Parameters.AddWithValue("@BILLING_QTY", truckJobTrip.BillingQty);
                cmd.Parameters.AddWithValue("@TOTAL_WEIGHT", truckJobTrip.TotalWeight);
                cmd.Parameters.AddWithValue("@ACTUAL_VOLUME", truckJobTrip.actualVol);
                cmd.Parameters.AddWithValue("@ACTUAL_WEIGHT", truckJobTrip.actualWeight);
                cmd.Parameters.AddWithValue("@BILLING_UOM", truckJobTrip.billing_UOM);
                cmd.Parameters.AddWithValue("@WEIGHT_REQUIRED", truckJobTrip.weightRequired);
                if (truckJobTrip.wtVolFrDetails == true)
                {
                    cmd.Parameters.AddWithValue("@WT_VOL_FR_DETAILS", "T");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@WT_VOL_FR_DETAILS", "F");
                }
                //201309 - Gerry Added
                cmd.Parameters.AddWithValue("@IS_PACKAGE_DEPENDENT", truckJobTrip.isPackageDependent ? "T" : "F");
                cmd.Parameters.AddWithValue("@IS_MULTI_PACKAGE", truckJobTrip.isMultiplePackage ? "T" : "F");
                cmd.Parameters.AddWithValue("@PACK_TYPE", truckJobTrip.packType.ToString().Trim());
                cmd.Parameters.AddWithValue("@LEG_TYPE", truckJobTrip.legType.GetHashCode());
                cmd.Parameters.AddWithValue("@PARTNER_LEG", truckJobTrip.partnerLeg);
                cmd.Parameters.AddWithValue("@IS_FIXED_TIME", truckJobTrip.isFixedTime ? "T" : "F");
                cmd.Parameters.AddWithValue("@TIME_BOUND", truckJobTrip.timeBound.GetHashCode());
                cmd.Parameters.AddWithValue("@IS_REFRIGERATED", truckJobTrip.isRefrigeratedGoods ? "T" : "F");

                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                truckJobTrip.Sequence = Convert.ToInt32(newReqNumber.Value);
                truckJobTrip.UpdateVersion = GetTruckJobTripUpdateVersion(truckJobTrip, cn, tran);

                truckJobTripOut = GetTruckJobTrip(truckJobTrip.JobID, truckJobTrip.Sequence, cn, tran);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return true;
        }

        internal static byte[] GetTruckJobTripUpdateVersion(TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran)
        {

            byte[] originalRowVersion = new byte[8];
            try
            {
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)";
                SQLString += " WHERE JOB_ID = " + truckJobTrip.JobID;
                SQLString += " AND JOBTRIP_SEQ_NO = " + truckJobTrip.Sequence;
                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return originalRowVersion;
        }

        internal static SortableList<TruckJob> GetAllTruckJobs()
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl

            SortableList<TruckJob> Operators = new SortableList<TruckJob>();

            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_JOB_MAIN_Tbl with (NOLOCK) INNER JOIN TRK_JOB_SHIP_AIR_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TRK_JOB_MAIN_Tbl.JOB_ID=TRK_JOB_SHIP_AIR_INFO_Tbl.JOB_ID";
                SQLString += " Order by TRK_JOB_MAIN_Tbl.JOB_ID desc";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJob(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operators;
        }

        internal static TruckJob GetTruckJob(string JobNo)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl  
            TruckJob Operator = new TruckJob();

            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                // Chong Chin27 Aug 2010 - Change table names 
                string SQLString = "SELECT * FROM TRK_JOB_MAIN_Tbl with (NOLOCK) INNER JOIN TRK_JOB_SHIP_AIR_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TRK_JOB_MAIN_Tbl.JOB_ID=TRK_JOB_SHIP_AIR_INFO_Tbl.JOB_ID";
                SQLString += " WHERE TRK_JOB_MAIN_Tbl.JOB_NUMBER='" + JobNo + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetTruckJob(reader);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operator;
        }
        // Chong Chin - 25 Aug 2010, new method, includes SqlConnection parameter, so as to able to use NUNIT
        // to test Add/Edit/Delete TruckHeader

        internal static TruckJob GetTruckJob(string JobNo, SqlConnection con, SqlTransaction tran)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl

            TruckJob truckJob = new TruckJob();
            //gerry added try catch
            try
            {
                // Chong Chin 27 Aug 2010 - Change table names 
                string SQLString = "SELECT * FROM TRK_JOB_MAIN_Tbl with (NOLOCK) INNER JOIN TRK_JOB_SHIP_AIR_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TRK_JOB_MAIN_Tbl.JOB_ID=TRK_JOB_SHIP_AIR_INFO_Tbl.JOB_ID";
                SQLString += " WHERE TRK_JOB_MAIN_Tbl.JOB_NUMBER='" + JobNo + "'";
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    truckJob = GetTruckJob(reader);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return truckJob;
        }

        internal static TruckJob GetTruckJob(int JobId)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl

            TruckJob Operator = new TruckJob();
            //gerry added try catch
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_JOB_MAIN_Tbl with (NOLOCK) INNER JOIN TRK_JOB_SHIP_AIR_INFO_Tbl with (NOLOCK)";
                SQLString += " ON TRK_JOB_MAIN_Tbl.JOB_ID=TRK_JOB_SHIP_AIR_INFO_Tbl.JOB_ID";
                SQLString += " WHERE TRK_JOB_MAIN_Tbl.JOB_ID='" + JobId + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetTruckJob(reader);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operator;
        }


        internal static TruckJob GetTruckJob(IDataReader reader)
        {
            SortableList<TruckJobTrip> truckJobTrips = new SortableList<TruckJobTrip>();
            //gerry added try catch
            try
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
                    // 2013-10-24 Zhou Kai adds
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
                ChargeType tchargetype = ChargeType.NotStopDependent;
                temp = Convert.ToInt32(reader["TRUCK_CHARGE_TYPE"]);
                switch (temp)
                {
                    case 1:
                        tchargetype = ChargeType.NotStopDependent;
                        break;
                    //case 2:
                    //    tchargetype = TruckChargeType.SectorDependent;
                    //    break;
                    case 2:
                        tchargetype = ChargeType.StopDependent;
                        break;
                }

                bool billbytpt = true;
                string tempbill = (string)reader["BILL_BY_TRANSPORT"];
                if (tempbill == "F")
                    billbytpt = false;

                string remarks2 = "";
                if (reader["REMARKS2"] is System.DBNull)
                {
                    remarks2 = "";
                }
                else
                {
                    remarks2 = (string)reader["REMARKS2"];
                }

                string remarks3 = "";
                if (reader["REMARKS3"] is System.DBNull)
                {
                    remarks3 = "";
                }
                else
                {
                    remarks3 = (string)reader["REMARKS3"];
                }

                string remarks4 = "";
                if (reader["REMARKS4"] is System.DBNull)
                {
                    remarks4 = "";
                }
                else
                {
                    remarks4 = (string)reader["REMARKS4"];
                }

                string remarks5 = "";
                if (reader["REMARKS5"] is System.DBNull)
                {
                    remarks5 = "";
                }
                else
                {
                    remarks5 = (string)reader["REMARKS5"];
                }
                #region 20160408 - gerry replaced
                /*
                // 2014-04-09 Zhou Kai adds
                string freightType = reader["FREIGHT_TYPE"] is DBNull ? String.Empty : (string)reader["FREIGHT_TYPE"];
                FreightType freight_type = FreightType.LO;
                if (!String.IsNullOrEmpty(freightType))
                {
                    if (freightType.Equals(FreightType.AF.ToString()))
                    {
                        freight_type = FreightType.AF;
                    }
                    else if (freightType.Equals(FreightType.SF.ToString()))
                    {
                        freight_type = FreightType.SF;
                    }
                    else if (freightType.Equals(FreightType.LO.ToString()))
                    {
                        freight_type = FreightType.LO;
                    }
                    else
                    {
                        // freight_type is blank, which should be not allowed,
                        // but temporary we allow it
                        // throw new FMException("");
                    }
                }
                // 2014-04-09 Zhou Kai ends
                */
                #endregion
                string tempTripType = reader["TRIP_TYPE"] == DBNull.Value ? Trip_Type.OTO.ToString() : (string)reader["TRIP_TYPE"];
                Trip_Type tripType = Trip_Type.OTO;
                switch (tempTripType)
                { 
                    case "OTM":
                        tripType = Trip_Type.OTM;
                        break;
                    case "MTO":
                        tripType = Trip_Type.MTO;
                        break;
                }

                TruckJob truckJob = new TruckJob(
                    (int)reader["JOB_ID"],
                    reader["JOB_NUMBER"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["JOB_NUMBER"],
                    reader["CUST_CODE"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["CUST_CODE"],
                    reader["SOURCE_REF_NUMBER"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["SOURCE_REF_NUMBER"],
                    jobstatus,
                    reader["JOB_TYPE"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["JOB_TYPE"],
                    reader["BOOKING_NUMBER"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["BOOKING_NUMBER"],
                    (DateTime)reader["JOB_DATETIME"],
                    reader["QUOTATION_NO"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["QUOTATION_NO"],
                    jobUrgencyType,
                    billbytpt,
                    reader["TPT_DEPT_CODE"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["TPT_DEPT_CODE"],
                    reader["TPT_DEPT_CODE"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["TPT_DEPT_CODE"],
                    reader["BRANCH_CODE"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["BRANCH_CODE"],
                    (byte[])reader["UPDATE_VERSION"],
                    true,
                    tchargetype,
                    reader["SHIPPER_CODE"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["SHIPPER_CODE"],
                    reader["SHIPPER_NAME"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["SHIPPER_NAME"],
                    reader["SHIPPER_ADD1"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["SHIPPER_ADD1"],
                    reader["SHIPPER_ADD2"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["SHIPPER_ADD2"],
                    reader["SHIPPER_ADD3"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["SHIPPER_ADD3"],
                    reader["SHIPPER_ADD4"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["SHIPPER_ADD4"],
                    reader["CONSIGNEE_CODE"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["CONSIGNEE_CODE"],
                    reader["CONSIGNEE_NAME"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["CONSIGNEE_NAME"],
                    reader["CONSIGNEE_ADD1"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["CONSIGNEE_ADD1"],
                    reader["CONSIGNEE_ADD2"] == DBNull.Value ? Job_Type.LO.ToString() : (string)reader["CONSIGNEE_ADD2"],
                    reader["CONSIGNEE_ADD3"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD3"],
                    reader["CONSIGNEE_ADD4"] == DBNull.Value ? string.Empty : (string)reader["CONSIGNEE_ADD4"],
                    reader["PSA_ACCOUNT"] == DBNull.Value ? string.Empty : (string)reader["PSA_ACCOUNT"],
                    reader["SUBCONTRACTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CODE"],
                    reader["SUBCONTRACTOR_NAME"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_NAME"],
                    reader["SUBCONTRACTOR_ADD1"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD1"],
                    reader["SUBCONTRACTOR_ADD2"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD2"],
                    reader["SUBCONTRACTOR_ADD3"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD3"],
                    reader["SUBCONTRACTOR_ADD4"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD4"],
                    reader["WAREHOUSE_NO"] == DBNull.Value ? string.Empty : (string)reader["WAREHOUSE_NO"],
                    reader["YOUR_REF_NO"] == DBNull.Value ? string.Empty : (string)reader["YOUR_REF_NO"],
                    reader["UCR_NO"] == DBNull.Value ? string.Empty : (string)reader["UCR_NO"],
                    reader["PERMIT_NO1"] == DBNull.Value ? string.Empty : (string)reader["PERMIT_NO1"],
                    (DateTime)reader["DATE_PERMIT_NO1"],
                    reader["PERMIT_NO2"] == DBNull.Value ? string.Empty : (string)reader["PERMIT_NO2"],
                    (DateTime)reader["DATE_PERMIT_NO2"],
                    reader["REMARKS"] == DBNull.Value ? string.Empty : (string)reader["REMARKS"],
                    remarks2,
                    remarks3,
                    remarks4,
                    remarks5,
                    reader["CARRIER_AGENT"] == DBNull.Value ? string.Empty : (string)reader["CARRIER_AGENT"],
                    reader["OBL_NO"] == DBNull.Value ? string.Empty : (string)reader["OBL_NO"],
                    reader["HBL_NO"] == DBNull.Value ? string.Empty : (string)reader["HBL_NO"],
                    reader["VESSEL_NO"] == DBNull.Value ? string.Empty : (string)reader["VESSEL_NO"],
                    reader["VOYAGE_NO"] == DBNull.Value ? string.Empty : (string)reader["VOYAGE_NO"],
                    reader["POL"] == DBNull.Value ? string.Empty : (string)reader["POL"],
                    reader["POD"] == DBNull.Value ? string.Empty : (string)reader["POD"],
                    (DateTime)reader["ETA"],
                    (DateTime)reader["ETD"],
                    reader["ORIGIN"] == DBNull.Value ? string.Empty : (string)reader["ORIGIN"],
                    reader["DESTINATION"] == DBNull.Value ? string.Empty : (string)reader["DESTINATION"],
                    reader["VIA"] == DBNull.Value ? string.Empty : (string)reader["VIA"],
                    reader["FLIGHTNO"] == DBNull.Value ? string.Empty : (string)reader["FLIGHTNO"],
                    reader["HAWB"] == DBNull.Value ? string.Empty : (string)reader["HAWB"],
                    reader["MAWB"] == DBNull.Value ? string.Empty : (string)reader["MAWB"],
                    reader["AWB_NO"] == DBNull.Value ? string.Empty : (string)reader["AWB_NO"],
                    (DateTime)reader["FLIGHT_DATE"],
                    truckJobTrips
                    /*2014-04-09 Zhou Kai adds*/
                    , /*JobType*/ tripType
                    , reader["IsBillable"].ToString().Equals("T") ? true : false);
                //20130923 - Gerry added for JLV commercial invoice and customer declaration
                truckJob.commercialInvoice = reader["COMMERCIAL_INVOICE_NO"] == DBNull.Value ? string.Empty : (string)reader["COMMERCIAL_INVOICE_NO"];
                truckJob.customsDeclaration = reader["CUSTOM_DECLARATION_NO"] == DBNull.Value ? string.Empty : (string)reader["CUSTOM_DECLARATION_NO"];

                truckJob.JobCreatorId = reader["Added_By"] == DBNull.Value ? string.Empty : reader["Added_By"].ToString();
                truckJob.JobCreatedDateTime = reader["Booked_DateTime"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["Booked_DateTime"];
                truckJob.isFlexibleTime = reader["IsFlexibleTime"] == DBNull.Value ? true : ((bool)reader["IsFlexibleTime"].ToString().Equals("T") ? true : false);
                return truckJob;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }


        internal static bool EditTruckJobTrip(TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran, out TruckJobTrip truckJobTripOut)
        {
            //gerry added try catch
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_Tbl  with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + truckJobTrip.JobID;
                SQLString += " AND JOBTRIP_SEQ_NO = " + truckJobTrip.Sequence;
                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                if (SqlBinary.Equals(truckJobTrip.UpdateVersion, originalRowVersion) == false)
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nTruckJobDAL.EditTruckJobTrip");

                }
                else
                {
                    SqlCommand cmd = new SqlCommand("sp_Edit_TRK_JOB_DETAIL_TRIP_Tbl", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JOB_ID", truckJobTrip.JobID);
                    cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", truckJobTrip.Sequence);
                    cmd.Parameters.AddWithValue("@FROM_STOP", truckJobTrip.StartStop.Code);
                    cmd.Parameters.AddWithValue("@FROM_ADD1", truckJobTrip.StartStop.Address1);
                    cmd.Parameters.AddWithValue("@FROM_ADD2", truckJobTrip.StartStop.Address2);
                    cmd.Parameters.AddWithValue("@FROM_ADD3", truckJobTrip.StartStop.Address3);
                    cmd.Parameters.AddWithValue("@FROM_ADD4", truckJobTrip.StartStop.Address4);
                    cmd.Parameters.AddWithValue("@FROM_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJobTrip.StartDate));
                    cmd.Parameters.AddWithValue("@FROM_TIME", truckJobTrip.StartTime);
                    cmd.Parameters.AddWithValue("@TO_STOP", truckJobTrip.EndStop.Code);
                    cmd.Parameters.AddWithValue("@TO_ADD1", truckJobTrip.EndStop.Address1);
                    cmd.Parameters.AddWithValue("@TO_ADD2", truckJobTrip.EndStop.Address2);
                    cmd.Parameters.AddWithValue("@TO_ADD3", truckJobTrip.EndStop.Address3);
                    cmd.Parameters.AddWithValue("@TO_ADD4", truckJobTrip.EndStop.Address4);
                    cmd.Parameters.AddWithValue("@TO_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJobTrip.EndDate));
                    cmd.Parameters.AddWithValue("@TO_TIME", truckJobTrip.EndTime);
                    cmd.Parameters.AddWithValue("@BOOKED_VOLUME", truckJobTrip.BookedVol);
                    cmd.Parameters.AddWithValue("@BOOKED_WEIGHT", truckJobTrip.BookedWeight);
                    cmd.Parameters.AddWithValue("@ACTUAL_VOLUME", truckJobTrip.actualVol);
                    cmd.Parameters.AddWithValue("@ACTUAL_WEIGHT", truckJobTrip.actualWeight);
                    cmd.Parameters.AddWithValue("@CARGO_DESCRIPTION", truckJobTrip.CargoDescription);
                    cmd.Parameters.AddWithValue("@IS_LADEN", truckJobTrip.IsLaden ? "T" : "F");

                    int status = 0;
                    if (truckJobTrip.TripStatus == JobTripStatus.Booked)
                        status = 1;
                    if (truckJobTrip.TripStatus == JobTripStatus.Ready)
                        status = 2;
                    if (truckJobTrip.TripStatus == JobTripStatus.Assigned)
                        status = 3;
                    if (truckJobTrip.TripStatus == JobTripStatus.Completed)
                        status = 4;
                    if (truckJobTrip.TripStatus == JobTripStatus.Invoiced)
                        status = 5;

                    cmd.Parameters.AddWithValue("@STATUS", status);
                    cmd.Parameters.AddWithValue("@IS_DANGEROUS_GOODS", truckJobTrip.IsDangerousGoods ? "T" : "F");
                    cmd.Parameters.AddWithValue("@DG_REMARKS", truckJobTrip.DGRemarks);
                    cmd.Parameters.AddWithValue("@IS_OVERSIZE", truckJobTrip.IsOversize ? "T" : "F");
                    cmd.Parameters.AddWithValue("@OVERSIZE_REMARKS", truckJobTrip.OversizeRemarks);
                    cmd.Parameters.AddWithValue("@IS_DIRECT_DELIVERY", truckJobTrip.IsDirectDelivery ? "T" : "F");
                    cmd.Parameters.AddWithValue("@OWNTRANSPORT", truckJobTrip.OwnTransport ? "T" : "F");
                    //20140312-gerry replaced
                    //cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", truckJobTrip.SubContractorCode);  
                    //201040502 - gerry change subCon from CustomerDTO to operatorDTO
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CODE", truckJobTrip.subCon.Code.ToString());
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_NAME", truckJobTrip.subCon.Name.ToString());
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD1", truckJobTrip.subCon.Add1.ToString());
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD2", truckJobTrip.subCon.Add2.ToString());
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD3", truckJobTrip.subCon.Add3.ToString());
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_ADD4", truckJobTrip.subCon.Add4.ToString());
                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_CITY_CODE", truckJobTrip.subCon.City.ToString());
                    //20140312 end

                    cmd.Parameters.AddWithValue("@SUBCONTRACTOR_REFERENCE", truckJobTrip.SubContractorReference);
                    cmd.Parameters.AddWithValue("@CHARGE_CODE", truckJobTrip.ChargeCode);
                    cmd.Parameters.AddWithValue("@REMARKS", truckJobTrip.Remarks);
                    cmd.Parameters.AddWithValue("@BILLING_QTY", truckJobTrip.BillingQty);
                    cmd.Parameters.AddWithValue("@TOTAL_WEIGHT", truckJobTrip.TotalWeight);
                    cmd.Parameters.AddWithValue("@BILLING_UOM", truckJobTrip.billing_UOM);
                    cmd.Parameters.AddWithValue("@WEIGHT_REQUIRED", truckJobTrip.weightRequired);
                    cmd.Parameters.AddWithValue("@WT_VOL_FR_DETAILS", truckJobTrip.wtVolFrDetails ? "T" : "F");

                    //201309 - Gerry Added
                    cmd.Parameters.AddWithValue("@IS_PACKAGE_DEPENDENT", truckJobTrip.isPackageDependent ? "T" : "F");
                    cmd.Parameters.AddWithValue("@IS_MULTI_PACKAGE", truckJobTrip.isMultiplePackage ? "T" : "F");
                    cmd.Parameters.AddWithValue("@PACK_TYPE", truckJobTrip.packType.ToString().Trim());
                    cmd.Parameters.AddWithValue("@IS_FIXED_TIME", truckJobTrip.isFixedTime ? "T" : "F");
                    cmd.Parameters.AddWithValue("@TIME_BOUND", truckJobTrip.timeBound.GetHashCode());
                    cmd.Parameters.AddWithValue("@IS_REFRIGERATED", truckJobTrip.isRefrigeratedGoods ? "T" : "F");//20170801

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();


                    truckJobTripOut = GetTruckJobTrip(truckJobTrip.JobID, truckJobTrip.Sequence, cn, tran);

                    return true;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

        }

        internal static bool DeleteTruckJobTrip(TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_JOB_DETAIL_TRIP_Tbl", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", truckJobTrip.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", truckJobTrip.Sequence);

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

            }//gerry replaced catch clause
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return true;
        }

        // creates TruckJobTrip object from 1 row of the DataReader
        internal static TruckJobTrip GetTruckJobTrip(IDataReader reader)
        {
            //gerry added try catch
            try
            {
                bool isLaden = true;
                string temp = (string)reader["IS_LADEN"];
                switch (temp)
                {
                    case "T":
                        isLaden = true;
                        break;
                    case "F":
                        isLaden = false;
                        break;
                }

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
                    reader["FROM_STOP"] == DBNull.Value ? string.Empty : (string)reader["FROM_STOP"],
                    stop.Description, //20121116 - Gerry replaced to get Name or description base on the stop code
                    reader["FROM_ADD1"] == DBNull.Value ? string.Empty : (string)reader["FROM_ADD1"],
                    reader["FROM_ADD2"] == DBNull.Value ? string.Empty : (string)reader["FROM_ADD2"],
                    reader["FROM_ADD3"] == DBNull.Value ? string.Empty : (string)reader["FROM_ADD3"],
                    reader["FROM_ADD4"] == DBNull.Value ? string.Empty : (string)reader["FROM_ADD4"],
                    stop.City);   //20121116 - Gerry replaced to get city base on the stop code
                startStop.operationType = stop.operationType;
                startStop.postalCode = stop.postalCode;
                startStop.countryName = stop.countryName;

                stop = Stop.GetStop((string)reader["TO_STOP"]);                
                Stop endStop = new Stop(
                    reader["FROM_STOP"] == DBNull.Value ? string.Empty : (string)reader["TO_STOP"],
                    stop.Description,    //20121116 - Gerry replaced to get Name or description base on the stop code
                    reader["TO_ADD1"] == DBNull.Value ? string.Empty : (string)reader["TO_ADD1"],
                    reader["TO_ADD2"] == DBNull.Value ? string.Empty : (string)reader["TO_ADD2"],
                    reader["TO_ADD3"] == DBNull.Value ? string.Empty : (string)reader["TO_ADD3"],
                    reader["TO_ADD4"] == DBNull.Value ? string.Empty : (string)reader["TO_ADD4"],
                    stop.City);     //20121116 - Gerry replaced to get city base on the stop code
                endStop.operationType = stop.operationType;
                endStop.postalCode = stop.postalCode;
                endStop.countryName = stop.countryName;

                //20140312 - gerry added   
                //201040502 - gerry change subCon from CustomerDTO to operatorDTO
                //CustomerDTO subCon = new CustomerDTO();
                OperatorDTO subCon = new OperatorDTO();
                subCon.Code = (reader["SUBCONTRACTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CODE"]);
                subCon.Name = (reader["SUBCONTRACTOR_NAME"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_NAME"]);
                subCon.Add1 = reader["SUBCONTRACTOR_ADD1"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD1"];
                subCon.Add2 = reader["SUBCONTRACTOR_ADD2"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD2"];
                subCon.Add3 = reader["SUBCONTRACTOR_ADD3"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD3"];
                subCon.Add4 = reader["SUBCONTRACTOR_ADD4"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_ADD4"];
                subCon.City = reader["SUBCONTRACTOR_CITY_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CITY_CODE"];
                //20140312 end

                SortableList<JobTripState> jobStates = new SortableList<JobTripState>();


                bool isDangerousGoods = (string)reader["IS_DANGEROUS_GOODS"] == "T" ? true : false;
                bool isOverSize = (string)reader["IS_OVERSIZE"] == "T" ? true : false;
                bool isDirectDelivery = (string)reader["IS_DIRECT_DELIVERY"] == "T" ? true : false;
                bool ownTransport = (string)reader["OWNTRANSPORT"] == "T" ? true : false;
                bool wtVolFrDetails = (string)reader["WT_VOL_FR_DETAILS"] == "T" ? true : false;

                SortableList<TruckJobTripDetail> truckJobTripDetail = new SortableList<TruckJobTripDetail>();
                #region 20130406 - Gerry replaced
                /*
            TruckJobTrip truckJobTrip = new TruckJobTrip(
                (int)reader["JOB_ID"],
                (int)reader["JOBTRIP_SEQ_NO"],
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
                (string)reader["SUBCONTRACTOR_CODE"],
                (string)reader["SUBCONTRACTOR_REFERENCE"],
                (string)reader["CHARGE_CODE"],
                (string)reader["REMARKS"],
                (decimal)reader["GROSS_WEIGHT"],
                (decimal)reader["GROSS_CBM"],
                (decimal)reader["MAX_WEIGHT"],
                (decimal)reader["MAX_CBM"],
                isLaden,
                (string)reader["CARGO_DESCRIPTION"],
                isDangerousGoods,
                (string)reader["DG_REMARKS"],
                isOverSize,
                (string)reader["OVERSIZE_REMARKS"],
                isDirectDelivery,
                (decimal)reader["TOTAL_CBM"],
                (decimal)reader["TOTAL_WEIGHT"],
                truckJobTripDetail,
                new SortableList<TruckJobTripPlan>(),
                (string)reader["BILLING_UOM"],
                (decimal)reader["WEIGHT_REQUIRED"],
                wtVolFrDetails);
                 */
                #endregion
                TruckJobTrip truckJobTrip = new TruckJobTrip(
                  (int)reader["JOB_ID"],
                  (int)reader["JOBTRIP_SEQ_NO"],
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
                    //20140312 - gerry replaced
                    //reader["SUBCONTRACTOR_CODE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_CODE"],
                  subCon,
                    //20140312 end
                  reader["SUBCONTRACTOR_REFERENCE"] == DBNull.Value ? string.Empty : (string)reader["SUBCONTRACTOR_REFERENCE"],
                  reader["CHARGE_CODE"] == DBNull.Value ? string.Empty : (string)reader["CHARGE_CODE"],
                  reader["REMARKS"] == DBNull.Value ? string.Empty : (string)reader["REMARKS"],
                  (decimal)reader["BOOK_WEIGHT"],
                  (decimal)reader["BOOK_VOLUME"],
                  (decimal)reader["ACT_WEIGHT"],
                  (decimal)reader["ACT_VOLUME"],
                  isLaden,
                  reader["CARGO_DESCRIPTION"] == DBNull.Value ? string.Empty : (string)reader["CARGO_DESCRIPTION"],
                  isDangerousGoods,
                  reader["DG_REMARKS"] == DBNull.Value ? string.Empty : (string)reader["DG_REMARKS"],
                  isOverSize,
                  reader["OVERSIZE_REMARKS"] == DBNull.Value ? string.Empty : (string)reader["OVERSIZE_REMARKS"],
                  isDirectDelivery,
                  (decimal)reader["BILLING_QTY"],
                  (decimal)reader["TOTAL_WEIGHT"],
                  truckJobTripDetail,
                  new SortableList<TruckJobTripPlan>(),
                  reader["BILLING_UOM"] == DBNull.Value ? string.Empty : (string)reader["BILLING_UOM"],
                  (decimal)reader["WEIGHT_REQUIRED"],
                  wtVolFrDetails);
                string custCode = reader["CUST_CODE"] == DBNull.Value ? string.Empty : (string)reader["CUST_CODE"];
                // CustomerDTO cust = CustomerDTO.GetCustomerByCustCode(custCode);
                truckJobTrip.CustomerCode = custCode;
                truckJobTrip.CustomerName = CustomerDTO.GetCustomerNameByCustomerCode(custCode);
                truckJobTrip.JobTripStates = GetAllTruckJobTripStates(truckJobTrip);
                truckJobTrip.JobNo = reader["JOB_NUMBER"] == DBNull.Value ? string.Empty : (string)reader["JOB_NUMBER"];

                //truckJobTrip.GetTruckJobTripPlans(); //20160203 - gerry removed from here
                truckJobTrip.truckJobTripDetail = GetTruckJobTripDetails(truckJobTrip.JobID, truckJobTrip.Sequence);

                //20130903 - Gerry Added
                truckJobTrip.isPackageDependent = (string)reader["IS_PACKAGE_DEPENDENT"] == "T" ? true : false;
                truckJobTrip.isMultiplePackage = (string)reader["IS_MULTI_PACKAGE"] == "T" ? true : false;
                truckJobTrip.packType = reader["PACK_TYPE"] == DBNull.Value ? string.Empty : (string)reader["PACK_TYPE"];
                //20131002 - Gerry added for multi leg
                int lType = reader["LEG_TYPE"] == DBNull.Value ? 0 : (int)reader["LEG_TYPE"];
                switch (lType)
                {
                    case 1:
                        truckJobTrip.legType = Leg_Type.OneLeg;
                        break;
                    case 21:
                        truckJobTrip.legType = Leg_Type.FirstOfTwoLeg;
                        break;
                    case 22:
                        truckJobTrip.legType = Leg_Type.SecondOfTwoLeg;
                        break;
                }
                truckJobTrip.partnerLeg = reader["PARTNER_LEG"] == DBNull.Value ? 0 : (int)reader["PARTNER_LEG"];
                //20161014 - gerry added
                string tempTripType = reader["TRIP_TYPE"] == DBNull.Value ? Trip_Type.OTO.ToString() : (string)reader["TRIP_TYPE"];
                truckJobTrip.tripType = Trip_Type.OTO;
                switch (tempTripType)
                {
                    case "OTM":
                        truckJobTrip.tripType = Trip_Type.OTM;
                        break;
                    case "MTO":
                        truckJobTrip.tripType = Trip_Type.MTO;
                        break;
                }
                //20161209 - gerry added
                truckJobTrip.isFixedTime = reader["IS_FIXED_TIME"] == DBNull.Value ? false : ((string)reader["IS_FIXED_TIME"] == "T" ? true : false);
                var timebound = reader["TIME_BOUND"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TIME_BOUND"].ToString());
                switch (timebound)
                { 
                    case 1:
                        truckJobTrip.timeBound = TIME_BOUND.FIXED_PICKUP;
                        break;
                    case 2:
                        truckJobTrip.timeBound = TIME_BOUND.FIXED_DELIVERY;
                        break;
                    case 3:
                        truckJobTrip.timeBound = TIME_BOUND.AM_PICKUP;
                        break;
                    case 4:
                        truckJobTrip.timeBound = TIME_BOUND.AM_DELIVERY;
                        break;
                    case 5:
                        truckJobTrip.timeBound = TIME_BOUND.PM_PICKUP;
                        break;
                    case 6:
                        truckJobTrip.timeBound = TIME_BOUND.PM_DELIVERY;
                        break;
                    default:
                        truckJobTrip.timeBound = TIME_BOUND.NONE;
                        break;
                }
                truckJobTrip.isRefrigeratedGoods = reader["IsRefrigerated"] == DBNull.Value ? false : ((string)reader["IsRefrigerated"] == "T" ? true : false); //20170801

                return truckJobTrip;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

        }
        internal static SortableList<JobTripState> GetAllTruckJobTripStates(TruckJobTrip truckJobTrip)
        {
            SortableList<JobTripState> Operators = new SortableList<JobTripState>();
            //gerry added try catch
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_JOB_TRIP_STATE_Tbl with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + truckJobTrip.JobID;
                SQLString += " AND SEQUENCE_NO = " + truckJobTrip.Sequence;

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJobTripState(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operators;
        }
        internal static SortableList<JobTripState> GetAllTruckJobTripStates(TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            SortableList<JobTripState> Operators = new SortableList<JobTripState>();
            //gerry added try catch
            try
            {
                string SQLString = "SELECT * FROM TRK_JOB_TRIP_STATE_Tbl with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + truckJobTrip.JobID;
                SQLString += " AND SEQUENCE_NO = " + truckJobTrip.Sequence;

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJobTripState(reader));
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operators;
        }

        internal static JobTripState GetTruckJobTripState(IDataReader reader)
        {
            //gerry addded try catch
            try
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
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

        }

        internal static SortableList<TruckJobTrip> GetAllTruckJobTrips()
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            SortableList<TruckJobTrip> Operators = new SortableList<TruckJobTrip>();
            //gerry added try catch
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT TRK_JOB_DETAIL_TRIP_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)";
                SQLString += " LEFT JOIN TRK_JOB_MAIN_Tbl with (NOLOCK) ON TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID=TRK_JOB_MAIN_Tbl.JOB_ID ";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TruckJobTrip truckJobTrip = GetTruckJobTrip(reader);
                    truckJobTrip.truckJobTripPlans = truckJobTrip.GetTruckJobTripPlans();
                    Operators.Add(truckJobTrip);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operators;
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTrips(int jobId)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl   
            SortableList<TruckJobTrip> truckJobTrips = new SortableList<TruckJobTrip>();
            //gerry added try catch
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                //gerry added try catch
                try
                {
                    string SQLString = "SELECT TRK_JOB_DETAIL_TRIP_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)";
                    SQLString += " LEFT JOIN TRK_JOB_MAIN_Tbl with (NOLOCK) ON TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID=TRK_JOB_MAIN_Tbl.JOB_ID ";
                    SQLString += " WHERE TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID=" + jobId;
                    SQLString += " ORDER BY TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO DESC";

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TruckJobTrip truckJobTrip = GetTruckJobTrip(reader);
                        truckJobTrip.truckJobTripPlans = truckJobTrip.GetTruckJobTripPlans();
                        truckJobTrips.Add(truckJobTrip);
                    }
                    reader.Close();
                    foreach (TruckJobTrip tempTrip in truckJobTrips)
                    {
                        foreach (TruckJobTripDetail tempDet in tempTrip.truckJobTripDetail)
                        {
                            tempDet.balQty = GetAvailableBalQtyForCargoDetail(tempDet);
                        }
                    }
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { cn.Close(); }
            }
            return truckJobTrips;
        }

        internal static TruckJobTrip GetTruckJobTrip(int jobid, int sequence)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            TruckJobTrip truckJobTrip = new TruckJobTrip();
            //gerry added try catch
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = "SELECT TRK_JOB_DETAIL_TRIP_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK) ";
                    SQLString += " LEFT JOIN TRK_JOB_MAIN_Tbl with (NOLOCK) ON TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID=TRK_JOB_MAIN_Tbl.JOB_ID ";
                    SQLString += "WHERE TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = " + jobid;
                    SQLString += " AND TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO = " + sequence;
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        truckJobTrip = GetTruckJobTrip(reader);
                        truckJobTrip.truckJobTripPlans = truckJobTrip.GetTruckJobTripPlans();
                        //truckJobTrip.truckJobTripDetail = GetTruckJobTripDetails(jobid, sequence);
                    }
                    reader.Close();
                    foreach (TruckJobTripDetail tempDet in truckJobTrip.truckJobTripDetail)
                    {
                        tempDet.balQty = GetAvailableBalQtyForCargoDetail(tempDet);
                    }
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { cn.Close(); }
                return truckJobTrip;
            }
        }

        internal static TruckJobTrip GetTruckJobTrip(int jobid, int sequence, SqlConnection con, SqlTransaction tran)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            TruckJobTrip Operator = new TruckJobTrip();
            //gerry added try catch
            try
            {
                string SQLString = "SELECT TRK_JOB_DETAIL_TRIP_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK) ";
                SQLString += " LEFT JOIN TRK_JOB_MAIN_Tbl with (NOLOCK) ON TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID=TRK_JOB_MAIN_Tbl.JOB_ID ";
                SQLString += "WHERE TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = " + jobid;
                SQLString += " AND TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO = " + sequence;
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetTruckJobTrip(reader);
                }
                reader.Close();
                Operator.truckJobTripPlans = Operator.GetTruckJobTripPlans(con, tran);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operator;
        }

        internal static TruckJobTripDetail GetTruckJobTripDetail(int jobid, int jobtripseq, int jobtripdetailseq, SqlConnection con, SqlTransaction tran)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            TruckJobTripDetail Operator = new TruckJobTripDetail();
            //gerry added try catch
            try
            {
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_DETAIL_Tbl with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + jobid;
                SQLString += " AND JOBTRIP_SEQ_NO = " + jobtripseq;
                SQLString += " AND SEQ_NO = " + jobtripdetailseq;
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetTruckJobTripDetail(reader);
                }
                reader.Close();

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operator;
        }
        //20160202 - gerry added parameter planSubTripSeqNo
        internal static TruckJobTripPlan GetTruckJobTripPlan(int jobid, int jobtripseq, string plantripNo, int planSubTripSeqNo, int planSubTripJobSeqNo,
            string driverNo, string vehicleNo, SqlConnection con, SqlTransaction tran)
        {
            TruckJobTripPlan Operator = new TruckJobTripPlan();
            //gerry added try catch
            try
            {
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK) ";
                SQLString += "WHERE JOB_ID = " + jobid;
                SQLString += " AND JOBTRIP_SEQ_NO = " + jobtripseq;
                SQLString += " AND PLANTRIP_NO = '" + plantripNo + "'";
                SQLString += " AND PLANSUBTRIP_SEQNO = " + planSubTripSeqNo; //20160202 - gerry added
                SQLString += " AND PLANSUBTRIPJOB_SEQNO = " + planSubTripJobSeqNo;
                SQLString += " AND DRIVER_NO = '" + driverNo + "'";
                SQLString += " AND VEHICLE_NO = '" + vehicleNo + "'";
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetTruckJobTripPlan(reader);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operator;


        }

        internal static SortableList<TruckJobTripDetail> GetTruckJobTripDetails(int jobid, int jobtripseq)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            SortableList<TruckJobTripDetail> truckJobTripDetails = new SortableList<TruckJobTripDetail>();
            //gerry added try catch
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_DETAIL_Tbl with (NOLOCK) ";
                    SQLString += "WHERE JOB_ID = " + jobid;
                    SQLString += " AND JOBTRIP_SEQ_NO = " + jobtripseq;
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TruckJobTripDetail tempCargoDetail = GetTruckJobTripDetail(reader);
                        truckJobTripDetails.Add(tempCargoDetail);
                    }
                    reader.Close();
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { cn.Close(); }
            }
            return truckJobTripDetails;
        }

        internal static SortableList<TruckJobTripPlan> GetTruckJobTripPlans(int jobid, int jobtripseq)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            SortableList<TruckJobTripPlan> Operators = new SortableList<TruckJobTripPlan>();
            //gerry added try catch
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  ";
                SQLString += "WHERE JOB_ID = " + jobid;
                SQLString += " AND JOBTRIP_SEQ_NO = " + jobtripseq;
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJobTripPlan(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operators;
        }
        internal static SortableList<TruckJobTripPlan> GetTruckJobTripPlans(int jobid, int jobtripseq, SqlConnection con, SqlTransaction tran)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            SortableList<TruckJobTripPlan> Operators = new SortableList<TruckJobTripPlan>();
            //gerry added try catch
            try
            {
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  ";
                SQLString += "WHERE JOB_ID = " + jobid;
                SQLString += " AND JOBTRIP_SEQ_NO = " + jobtripseq;
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJobTripPlan(reader));
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operators;
        }


        internal static TruckJobTripDetail GetTruckJobTripDetail(IDataReader reader)
        {
            //gerry added try catch
            try
            {
                TruckJobTripDetail truckJobTripDetail = new TruckJobTripDetail(
                    (int)reader["JOB_ID"],
                    (int)reader["JOBTRIP_SEQ_NO"],
                    (int)reader["SEQ_NO"],
                    (string)reader["UOM"],
                    (int)reader["QTY"],
                    (decimal)reader["UNIT_WEIGHT"],
                    (decimal)reader["TOT_WEIGHT"],
                    (decimal)reader["LENGTH"],
                    (decimal)reader["WIDTH"],
                    (decimal)reader["HEIGHT"],
                    (decimal)reader["VOLUME"],
                    (string)reader["MARKING"],
                    (string)reader["CARGO_DESC"],
                    (string)reader["REMARKS"]
                );
                truckJobTripDetail.ref_No = reader["Ref_No"] == DBNull.Value ? string.Empty : (string)reader["Ref_No"]; //20170613
                return truckJobTripDetail;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static TruckJobTripPlan GetTruckJobTripPlan(IDataReader reader)
        {
            //gerry added try catch
            try
            {
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

                TruckJobTripPlan truckJobTripPlan = new TruckJobTripPlan(
                    (DateTime)reader["SCHEDULE_DATE"],
                    (string)reader["PLANTRIP_NO"],
                    (string)reader["DRIVER_NO"],
                    (string)reader["VEHICLE_NO"],
                    (decimal)reader["VOLUME"],
                    (decimal)reader["WEIGHT"],
                    (byte[])reader["UPDATE_VERSION"],
                    (DateTime)reader["START_TIME"],
                    (DateTime)reader["END_TIME"],
                    jobtripstatus,
                    (int)reader["PLANSUBTRIPJOB_SEQNO"]
                );
                truckJobTripPlan.jobID = (int)reader["JOB_ID"];
                truckJobTripPlan.sequence = (int)reader["JOBTRIP_SEQ_NO"];
                //20160201
                truckJobTripPlan.planSubTripSeqNo = reader["PLANSUBTRIP_SEQNO"] == DBNull.Value ? 0 : (int)reader["PLANSUBTRIP_SEQNO"];
                truckJobTripPlan.planSubTripJobSeqNo = (int)reader["PLANSUBTRIPJOB_SEQNO"];

                return truckJobTripPlan;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static ArrayList GetAllTruckJobNo()
        {
            ArrayList list = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK)  INNER JOIN TRK_JOB_SHIP_AIR_INFO_Tbl with (NOLOCK) ";
                SQLString += " ON TRK_JOB_MAIN_Tbl.JOB_ID=TRK_JOB_SHIP_AIR_INFO_Tbl.JOB_ID";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((string)reader["JOB_NUMBER"]);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return list;
        }

        internal static List<string> GetAllTruckJobNoListString()
        {
            List<string> list = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK)  INNER JOIN TRK_JOB_SHIP_AIR_INFO_Tbl with (NOLOCK) ";
                SQLString += " ON TRK_JOB_MAIN_Tbl.JOB_ID=TRK_JOB_SHIP_AIR_INFO_Tbl.JOB_ID";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((string)reader["JOB_NUMBER"]);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return list;
        }

        internal static SortableList<TruckJob> GetTruckJobsWithRangedCustNo(string StartCustNo, string EndCustNo)
        {
            try
            {
                string sqlWhere = " Where ";
                sqlWhere += "TRK_JOB_MAIN_Tbl.CUST_CODE >= '" + StartCustNo + "' AND TRK_JOB_MAIN_Tbl.CUST_CODE <= '" + EndCustNo + "'";
                //sqlWhere += " Order by TRK_JOB_MAIN_Tbl.JOB_ID desc";
                return GetTruckJobs(sqlWhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static SortableList<TruckJob> GetTruckJobsWithRangedJobNo(string StartJobNo, string EndJobNo)
        {
            try
            {
                string sqlWhere = " Where ";
                sqlWhere += "TRK_JOB_MAIN_Tbl.JOB_NUMBER >= '" + StartJobNo + "' AND TRK_JOB_MAIN_Tbl.JOB_NUMBER <= '" + EndJobNo + "'";
                //sqlWhere += " Order by TRK_JOB_MAIN_Tbl.JOB_ID desc";
                return GetTruckJobs(sqlWhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static SortableList<TruckJob> GetTruckJobs(string StartJobNo, string EndJobNo, string StartCustNo, string EndCustNo)
        {
            try
            {
                string sqlWhere = " Where ";
                sqlWhere += "TRK_JOB_MAIN_Tbl.JOB_NUMBER >= '" + StartJobNo + "' AND TRK_JOB_MAIN_Tbl.JOB_NUMBER <= '" + EndJobNo + "'";
                sqlWhere += " AND TRK_JOB_MAIN_Tbl.CUST_CODE >= '" + StartCustNo + "' AND TRK_JOB_MAIN_Tbl.CUST_CODE <= '" + EndCustNo + "'";

                return GetTruckJobs(sqlWhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        private static SortableList<TruckJob> GetTruckJobs(string sqlWhere)
        {
            SortableList<TruckJob> truckJobs = new SortableList<TruckJob>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_JOB_MAIN_Tbl with (NOLOCK)  INNER JOIN TRK_JOB_SHIP_AIR_INFO_Tbl with (NOLOCK) ";
                SQLString += " ON TRK_JOB_MAIN_Tbl.JOB_ID=TRK_JOB_SHIP_AIR_INFO_Tbl.JOB_ID";
                SQLString += sqlWhere;
                SQLString += " ORDER BY TRK_JOB_MAIN_Tbl.JOB_ID DESC";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    truckJobs.Add(GetTruckJob(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return truckJobs;
        }

        internal static bool UpdateWeightAndVolume(TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                byte[] originalRowVersion = new byte[8];
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  ";
                SQLString += "WHERE JOB_ID = " + truckJobTrip.JobID;
                SQLString += "AND JOBTRIP_SEQ_NO = " + truckJobTrip.Sequence;
                SqlCommand cmd1 = new SqlCommand(SQLString, cn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Transaction = tran;
                IDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    originalRowVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                if (SqlBinary.Equals(truckJobTrip.UpdateVersion, originalRowVersion) == false)
                {
                    throw new FMException("The data has been changed by someone else. Please refresh the data and update");

                }
                else
                {
                    //SqlCommand cmd = new SqlCommand("sp_Edit_TRK_GROSS_WEIGHT_VOLUME_JOB_DETAIL_TRIP_Tbl", cn);
                    //SqlCommand cmd = new SqlCommand("sp_Edit_TPT_GROSS_WEIGHT_VOLUME_TRK_JOB_DETAIL_TRIP_Tbl", cn);
                    //20130408 - GERRY modify and renamed sp
                    SqlCommand cmd = new SqlCommand("sp_Edit_TRK_JOB_DETAIL_TRIP_BOOKED_WEIGHT_VOLUME_Tbl", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@JOB_ID", truckJobTrip.JobID);
                    cmd.Parameters.AddWithValue("@JOBTRIP_SEQ_NO", truckJobTrip.Sequence);
                    cmd.Parameters.AddWithValue("@BOOKED_WEIGHT", truckJobTrip.BookedWeight);
                    cmd.Parameters.AddWithValue("@BOOKED_VOL", truckJobTrip.BookedVol);
                    cmd.Parameters.AddWithValue("@ACTUAL_WEIGHT", truckJobTrip.actualWeight);
                    cmd.Parameters.AddWithValue("@ACTUAL_VOL", truckJobTrip.actualVol);
                    cmd.Parameters.AddWithValue("@BILLING_QTY", truckJobTrip.BillingQty);

                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }


        internal static bool AddTruckJobCharges(TruckJobCharge truckJobCharge, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_JOB_DETAIL_CHARGE_Tbl", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@INV_TRX_NO", truckJobCharge.InvTranNo);
                cmd.Parameters.AddWithValue("@INV_ITEM_SEQ_NO", truckJobCharge.InvItemSeqNo);
                cmd.Parameters.AddWithValue("@JOB_ID", truckJobCharge.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", truckJobCharge.SequenceNo);
                cmd.Parameters.AddWithValue("@CHARGE_CODE", truckJobCharge.ChargeCode);
                cmd.Parameters.AddWithValue("@DESCRIPTION", truckJobCharge.ChargeDescription);
                cmd.Parameters.AddWithValue("@UOM", truckJobCharge.Uom);
                cmd.Parameters.AddWithValue("@QUANTITY", truckJobCharge.Quantity);
                cmd.Parameters.AddWithValue("@UNIT_RATE", truckJobCharge.UnitRateFC);
                cmd.Parameters.AddWithValue("@CURRENCY", truckJobCharge.Currency);
                cmd.Parameters.AddWithValue("@EXCHANGE_RATE", truckJobCharge.ExchangeRate);
                cmd.Parameters.AddWithValue("@AMOUNT_FC", truckJobCharge.TotalAmountFC);
                cmd.Parameters.AddWithValue("@AMOUNT_HC", truckJobCharge.TotalAmountHC);
                cmd.Parameters.AddWithValue("@GST_TYPE", truckJobCharge.GstType);
                cmd.Parameters.AddWithValue("@GST_RATE", truckJobCharge.GstRate);
                cmd.Parameters.AddWithValue("@GST_AMOUNT_HC", truckJobCharge.GstAmountHC);
                cmd.Parameters.AddWithValue("@QUOTATION_ID", truckJobCharge.QuotationID);
                cmd.Parameters.AddWithValue("@QUOTATION_NO", truckJobCharge.QuotationNo);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO_RATE", truckJobCharge.SequenceNoRate);
                int status = 0;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;
                cmd.Parameters.AddWithValue("@JOB_CHARGE_STATUS", status);
                cmd.Parameters.AddWithValue("@REMARKS", truckJobCharge.Remarks);
                cmd.Parameters.AddWithValue("@JOB_CHARGE_TYPE", truckJobCharge.JobChargeType);
                cmd.Parameters.AddWithValue("@COMPLETED_DATE", DateUtility.ConvertDateAndTimeForSQLPurpose(truckJobCharge.CompletedDate));
                cmd.Parameters.AddWithValue("@RATE_TYPE", truckJobCharge.RateType);
                //        cmd.Parameters.AddWithValue("@CONTAINER_NUMBER", truckJobCharge.ContainerNo);

                //Temporary assign value while waiting for the specs
                cmd.Parameters.AddWithValue("@GST_AMOUNT_FC", truckJobCharge.GstAmountFC);
                //201310 -gerry added for jobtrip seqNo
                cmd.Parameters.AddWithValue("@JOBTRIP_SEQNO", truckJobCharge.JobTripSeqNo);

                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                truckJobCharge.SequenceNo = Convert.ToInt32(newReqNumber.Value);

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }


        }

        internal static bool EditTruckJobCharges(TruckJobCharge truckJobCharge, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Edit_TRK_JOB_DETAIL_CHARGE_Tbl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JOB_ID", truckJobCharge.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", truckJobCharge.SequenceNo);
                cmd.Parameters.AddWithValue("@GST_AMOUNT_HC", truckJobCharge.GstAmountHC);
                cmd.Parameters.AddWithValue("@DESCRIPTION", truckJobCharge.ChargeDescription);
                cmd.Parameters.AddWithValue("@QUANTITY", truckJobCharge.Quantity);
                cmd.Parameters.AddWithValue("@UNIT_RATE", truckJobCharge.UnitRateFC);
                cmd.Parameters.AddWithValue("@CURRENCY", truckJobCharge.Currency);
                cmd.Parameters.AddWithValue("@EXCHANGE_RATE", truckJobCharge.ExchangeRate);
                cmd.Parameters.AddWithValue("@GST_TYPE", truckJobCharge.GstType);
                cmd.Parameters.AddWithValue("@GST_RATE", truckJobCharge.GstRate);
                cmd.Parameters.AddWithValue("@AMOUNT_FC", truckJobCharge.TotalAmountFC);
                cmd.Parameters.AddWithValue("@AMOUNT_HC", truckJobCharge.TotalAmountHC);
                cmd.Parameters.AddWithValue("@QUOTATION_NO", truckJobCharge.QuotationNo);
                cmd.Parameters.AddWithValue("@RATE_TYPE", truckJobCharge.RateType);
                cmd.Parameters.AddWithValue("@UOM", truckJobCharge.Uom);

                //Temporary assign value while waiting for the specs
                cmd.Parameters.AddWithValue("@GST_AMOUNT_FC", truckJobCharge.GstAmountFC);


                int status = 0;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;
                cmd.Parameters.AddWithValue("@JOB_CHARGE_STATUS", status);
                #region REmoved
                //START****** 5 Apr 2011 - Gerry Added to set back INVTrxNo and INVITEMSEQNO to 0 when status is booked
                //if (status == 1)
                //{
                //    cmd.Parameters.AddWithValue("@INV_TRX_NO", 0);
                //    cmd.Parameters.AddWithValue("@INV_ITEM_SEQ_NO", 0);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@INV_TRX_NO", truckJobCharge.InvTranNo);
                //    cmd.Parameters.AddWithValue("@INV_ITEM_SEQ_NO", truckJobCharge.InvItemSeqNo);
                //}
                //END****************
                #endregion

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool EditTruckJobCharges(TruckJobCharge truckJobCharge, out TruckJobCharge outTruckJobCharge, SqlConnection con, SqlTransaction tran)
        {
            string sqlString = "";
            try
            {
                #region Queries
                if (truckJobCharge.QuotationNo == "")
                {
                    sqlString = @"UPDATE TRK_JOB_DETAIL_CHARGE_Tbl
	                                    SET
	                                      DESCRIPTION='{2}',
	                                      QUANTITY='{3}',
	                                      UNIT_RATE='{4}',
	                                      CURRENCY='{5}',
	                                      EXCHANGE_RATE='{6}',
	                                      GST_TYPE='{7}',
	                                      GST_RATE='{8}',
	                                      AMOUNT_FC='{9}',
	                                      AMOUNT_HC='{10}',
	                                      JOB_CHARGE_STATUS={11},
	                                      RATE_TYPE='{12}',
                                          Charge_CODE='{13}'";
                }
                else
                {
                    sqlString = @"UPDATE TRK_JOB_DETAIL_CHARGE_Tbl
                                        SET
                                          DESCRIPTION='{2}',
                                          QUANTITY='{3}',
                                          UNIT_RATE='{4}',
                                          AMOUNT_FC='{9}',
                                          AMOUNT_HC='{10}',
                                          JOB_CHARGE_STATUS={11},
                                          RATE_TYPE='{12}',
                                          Charge_CODE='{13}'";

                }

                sqlString += @"WHERE JOB_ID={0} AND SEQUENCE_NO={1}";
                #endregion

                #region Parameters
                int status = 0;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;

                sqlString = string.Format(sqlString,
                    truckJobCharge.JobID,
                    truckJobCharge.SequenceNo,
                    truckJobCharge.ChargeDescription,
                    truckJobCharge.Quantity,
                    truckJobCharge.UnitRateFC.ToString().Replace(",", "."),
                    truckJobCharge.Currency.ToString().Replace(",", "."),
                    truckJobCharge.ExchangeRate.ToString().Replace(",", "."),
                    truckJobCharge.GstType.ToString().Replace(",", "."),
                    truckJobCharge.GstRate.ToString().Replace(",", "."),
                    truckJobCharge.TotalAmountFC.ToString().Replace(",", "."),
                    truckJobCharge.TotalAmountHC.ToString().Replace(",", "."),
                    status,
                    truckJobCharge.RateType,
                    truckJobCharge.ChargeCode);
                #endregion

                SqlCommand cmd = new SqlCommand(sqlString, con);
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                outTruckJobCharge = GetTruckJobCharge(truckJobCharge);
                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static TruckJobCharge GetTruckJobCharge(TruckJobCharge truckJobCharge)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                truckJobCharge = new TruckJobCharge();
                string sqlSelect = @"Select * from TRK_JOB_DETAIL_CHARGE_Tbl with (NOLOCK)  Where JOB_ID = {0} and SEQUENCE_NO = {1}";

                sqlSelect = string.Format(sqlSelect, truckJobCharge.JobID, truckJobCharge.SequenceNo);

                SqlCommand cmd = new SqlCommand(sqlSelect, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    truckJobCharge = GetTruckJobCharge(reader);
                }
                reader.Close();

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return truckJobCharge;

        }

        internal static bool DeleteTruckJobCharges(TruckJobCharge truckJobCharge, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TRK_JOB_DETAIL_CHARGE_Tbl", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JOB_ID", truckJobCharge.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", truckJobCharge.SequenceNo);


                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        //20130423 - gerry added parameter for job charge type "T"(truckmovement) or "S"(other charges)
        internal static SortableList<TruckJobCharge> GetAllTruckJobChargesByType(TruckJob truckJob, string jobChargeType)
        {
            SortableList<TruckJobCharge> Operators = new SortableList<TruckJobCharge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_CHARGE_Tbl with (NOLOCK) ";
                SQLString += " WHERE JOB_ID = " + truckJob.JobID;
                SQLString += " AND JOB_CHARGE_TYPE = '" + jobChargeType + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJobCharge(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operators;

        }

        internal static TruckJobCharge GetTruckJobCharge(IDataReader reader)
        {
            //gerry added try catch
            try
            {
                int temp = (int)reader["JOB_CHARGE_STATUS"];
                JobChargeStatus status = JobChargeStatus.Booked;
                if (temp == 1)
                    status = JobChargeStatus.Booked;
                if (temp == 2)
                    status = JobChargeStatus.Completed;
                if (temp == 3)
                    status = JobChargeStatus.Invoiced;


                TruckJobCharge truckJobCharge = new TruckJobCharge(
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
                    (byte[])reader["UPDATE_VERSION"]);

                truckJobCharge.OldJobChargeStatus = status;
                //20131010 - Gerry added to get job trip seqNo
                truckJobCharge.JobTripSeqNo = reader["JOBTRIP_SEQNO"] == DBNull.Value ? 0 : (int)reader["JOBTRIP_SEQNO"];

                return truckJobCharge;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool FindDuplicateJobCharge(int jobID, string chargeCode)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLSearchString = "SELECT * FROM TRK_JOB_DETAIL_CHARGE_Tbl with (NOLOCK)  where JOB_ID = " + jobID;
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
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetJobTrips(JobTripStatus status, bool owntransport, string tptDeptCode, DateTime planDate)
        {
            try
            {
                string sqlWhere = " WHERE STATUS = " + status.GetHashCode();
                sqlWhere += " AND TRK_JOB_MAIN_Tbl.TPT_DEPT_CODE = '" + tptDeptCode + "'";
                if (owntransport == true)
                {
                    sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = 'T'";
                }
                else
                {
                    sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = 'F'";
                }
                sqlWhere += " AND FROM_DATE <= '" + DateUtility.ConvertDateForSQLPurpose(planDate) + LAST_MINUTE_OF_DAY + "'";

                return GetJobTrips(sqlWhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetJobTripsForPlanning(bool owntransport, string tptDeptCode, DateTime planDate, string jobNo)
        {
            //string sqlWhere = " WHERE (STATUS = 2 ";
            //            string sqlWhere = @" LEFT OUTER JOIN TRK_JOB_DETAIL_TRIP_PLAN_TBL
            //                                    ON TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOB_ID = TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID
            //                                    and TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO
            //                        
            //20121017 - Gerry modify query not to display duplicate job trip

            try
            {
                #region 20160315 - gerry replaced the code
                /*
                string sqlWhere = @" WHERE ((TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 2 OR TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 3) ";
                sqlWhere += " AND TRK_JOB_MAIN_Tbl.TPT_DEPT_CODE = '" + tptDeptCode + "'";
                if (owntransport == true)
                    sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = 'T'";
                else
                    sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = 'F'";

                sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE <= '" + DateUtility.ConvertDateForSQLPurpose(planDate) + LAST_MINUTE_OF_DAY + "')";

                //sqlWhere += " OR ((TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 3 OR TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 4)";

                sqlWhere += " OR (TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 4 ";
                sqlWhere += " AND TRK_JOB_MAIN_Tbl.TPT_DEPT_CODE = '" + tptDeptCode + "'";
                if (owntransport == true)
                    sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = 'T' ";
                else
                    sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = 'F' ";

                sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(planDate) + "'";
                sqlWhere += " AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE <= '" + DateUtility.ConvertDateForSQLPurpose(planDate) + LAST_MINUTE_OF_DAY + "')";
                if(!jobNo.Equals(string.Empty))
                    sqlWhere += " AND TRK_JOB_MAIN_Tbl.JOB_NUMBER like '%" + jobNo + "%'"; //20160315 - gerry added
                */
                #endregion
                string tempJobNoFilter = " AND TRK_JOB_MAIN_Tbl.JOB_NUMBER like '%" + jobNo + "%' ";
                string sqlWhere = @"WHERE ((TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 2 OR TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 3)  
			                                    AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE >= '{0}' 
                                                AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE <= '{1}') 
                                    AND TRK_JOB_MAIN_Tbl.TPT_DEPT_CODE = '{2}' 
                                    AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = '{3}' 
                                     {4} ";

                sqlWhere = string.Format(sqlWhere, DateUtility.ConvertDateForSQLPurpose(planDate),
                                                    DateUtility.ConvertDateForSQLPurpose(planDate) + LAST_MINUTE_OF_DAY,
                                                    tptDeptCode,
                                                    (owntransport ? "T" : "F"),
                                                    (jobNo == string.Empty ? string.Empty : tempJobNoFilter));
                #region Replaced query
                    /*
                    @"WHERE ((TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 2 OR TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 3)  
			                                    AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE <= '{0}' {1})
                                    OR (TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 4  AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE >= '{2}' 
                                                AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE <= '{0}' {1}) 
                                    AND TRK_JOB_MAIN_Tbl.TPT_DEPT_CODE = '{3}' 
                                    AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = '{4}' ";
                
                sqlWhere = string.Format(sqlWhere, DateUtility.ConvertDateForSQLPurpose(planDate) + LAST_MINUTE_OF_DAY,
                                                    (jobNo == string.Empty ? string.Empty : tempJobNoFilter),
                                                    DateUtility.ConvertDateForSQLPurpose(planDate), tptDeptCode,
                                                    (owntransport ? "T" : "F"));
                */
                #endregion
                return GetJobTrips(sqlWhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        //20161006 - jana - add date range filter
        internal static SortableList<TruckJobTrip> GetJobTripsForPlanningFilterDateRange(bool owntransport, string tptDeptCode, DateTime planDate, DateTime ToDate, string jobNo)
        {
            try
            {
                string tempJobNoFilter = " AND TRK_JOB_MAIN_Tbl.JOB_NUMBER like '%" + jobNo + "%' ";
                string sqlWhere = @"WHERE ((TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 2 OR TRK_JOB_DETAIL_TRIP_Tbl.STATUS = 3)  
                                                                                    AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE >= '{0}' 
                                                AND TRK_JOB_DETAIL_TRIP_Tbl.FROM_DATE <= '{1}') 
                                    AND TRK_JOB_MAIN_Tbl.TPT_DEPT_CODE = '{2}' 
                                    AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT = '{3}' 
                                     {4} ";

                sqlWhere = string.Format(sqlWhere, DateUtility.ConvertDateForSQLPurpose(planDate.Date),
                                                    DateUtility.ConvertDateForSQLPurpose(ToDate.Date),
                                                    tptDeptCode,
                                                    (owntransport ? "T" : "F"),
                                                    (jobNo == string.Empty ? string.Empty : tempJobNoFilter));
                return GetJobTrips(sqlWhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }


        private static SortableList<TruckJobTrip> GetJobTrips(string sqlWhere)
        {
            SortableList<TruckJobTrip> truckJobTrips = new SortableList<TruckJobTrip>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = "SELECT TRK_JOB_DETAIL_TRIP_Tbl.UPDATE_VERSION AS UPD_VER,* FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK) ";
                    SQLString += " LEFT JOIN TRK_JOB_MAIN_Tbl with (NOLOCK)  ON TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID=TRK_JOB_MAIN_Tbl.JOB_ID ";
                    SQLString += sqlWhere;
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TruckJobTrip truckJobTrip = GetTruckJobTrip(reader);
                        truckJobTrip.truckJobTripPlans = truckJobTrip.GetTruckJobTripPlans();
                        truckJobTrips.Add(truckJobTrip);
                        //Operators.Add(GetTruckJobTrip(reader));
                    }
                    reader.Close();
                    foreach (TruckJobTrip tempTrip in truckJobTrips)
                    {
                        foreach (TruckJobTripDetail tempDet in tempTrip.truckJobTripDetail)
                        {
                            tempDet.balQty = GetAvailableBalQtyForCargoDetail(tempDet);
                        }
                    }
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { cn.Close(); }
            }
            return truckJobTrips;
        }

        internal static bool AddTruckJobTripState(TruckJobTrip jobtrip, JobTripState jobTripState, SqlConnection cn, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                byte[] originalRowVersion = GetTruckJobTripUpdateVersion(jobtrip, cn, tran);
                if (SqlBinary.Equals(jobtrip.UpdateVersion, originalRowVersion) == false)
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nTruckJobDAL.AddJobTripState");
                }
                SqlCommand cmd = new SqlCommand("sp_Insert_TRK_JOB_TRIP_STATE_Tbl", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                int status = 0;
                if (jobTripState.Status == JobTripStatus.Booked)
                    status = 1;
                if (jobTripState.Status == JobTripStatus.Ready)
                    status = 2;
                if (jobTripState.Status == JobTripStatus.Assigned)
                    status = 3;
                if (jobTripState.Status == JobTripStatus.Completed)
                    status = 4;
                if (jobTripState.Status == JobTripStatus.Invoiced)
                    status = 5;

                cmd.Parameters.AddWithValue("@JOB_ID", jobtrip.JobID);
                cmd.Parameters.AddWithValue("@SEQUENCE_NO", jobtrip.Sequence);
                cmd.Parameters.AddWithValue("@JOB_STATE_NO", jobTripState.Seq_No);
                cmd.Parameters.AddWithValue("@STATUS", status);
                cmd.Parameters.AddWithValue("@STATUSDATE", DateUtility.ConvertDateAndTimeForSQLPurpose(DateTime.Now));
                cmd.Parameters.AddWithValue("@REMARKS", jobTripState.Remarks);

                SqlParameter newReqNumber = new SqlParameter("@Value", SqlDbType.Int);
                cmd.Parameters.Add(newReqNumber);
                newReqNumber.Direction = ParameterDirection.Output;

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                jobTripState.Seq_No = Convert.ToInt32(newReqNumber.Value);

                temp = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return temp;
        }

        internal static bool ChangeWeightVolume(decimal newWeight, decimal newVolume, TruckJobTripPlan tp, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string SQLUpdateDriverString = " Update TRK_JOB_DETAIL_TRIP_PLAN_TBL set " +
                                               " VOLUME = " + newVolume + "," +
                                               " WEIGHT  = " + newWeight +
                                               " Where JOB_ID = " + tp.jobID +
                                               " and JOBTRIP_SEQ_NO = " + tp.sequence +
                                               " and PLANTRIP_NO = '" + tp.planTripNo + "'";

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();


                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static bool SetJobTripStatusForTruck(JobTripStatus tripStatus, TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool temp = false;
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

                //update version cannot be equal anymore because of jobtripstate

                string SQLUpdateDriverString = " Update TRK_JOB_DETAIL_TRIP_Tbl set " +
                                               " STATUS = " + status +
                                               " Where JOB_ID = " + truckJobTrip.JobID +
                                               " and JOBTRIP_SEQ_NO = " + truckJobTrip.Sequence;   

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                // Chong Chin 13 March 11, start
                // Get the latest UpdateVersion property - object in memory in Truck Planning
                truckJobTrip.UpdateVersion = GetTruckJobTripUpdateVersion(truckJobTrip, cn, tran);
                // End 

                //return true;

                temp = true;

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return temp;
        }

        internal static bool SetJobTripPlanStatus(JobTripStatus tripStatus, TruckJobTripPlan truckJobTripPlan, SqlConnection cn, SqlTransaction tran)
        {
            bool temp = false;
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
                string sqlCheckVersion = @"Select * from TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK) 
                                        where PLANTRIP_NO = '{0}'
                                                AND PLANSUBTRIP_SEQNO = {1}
                                                AND PLANSUBTRIPJOB_SEQNO = {2}
                                                AND JOB_ID = {3}
                                                AND JOBTRIP_SEQ_NO = {4}";

                sqlCheckVersion = string.Format(sqlCheckVersion, truckJobTripPlan.planTripNo,
                                                                truckJobTripPlan.planSubTripSeqNo,
                                                                truckJobTripPlan.planSubTripJobSeqNo,
                                                                truckJobTripPlan.jobID,
                                                                truckJobTripPlan.sequence);

                SqlCommand cmd = new SqlCommand(sqlCheckVersion, cn);
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    origVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                #endregion
                if (!SqlBinary.Equals(origVersion, truckJobTripPlan.updateVersion))
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nTruckJobDAL.SetJobTripPlanStatus");
                }
                else
                {
                    string SQLUpdateDriverString = @"Update TRK_JOB_DETAIL_TRIP_PLAN_TBL 
                                                                set STATUS = {0}
                                                                where PLANTRIP_NO = '{1}'
                                                                        AND PLANSUBTRIP_SEQNO = {2}
                                                                        AND PLANSUBTRIPJOB_SEQNO = {3}
                                                                        AND JOB_ID = {4}
                                                                        AND JOBTRIP_SEQ_NO = {5}";

                    SQLUpdateDriverString = string.Format(SQLUpdateDriverString, status,
                                                                truckJobTripPlan.planTripNo,
                                                                truckJobTripPlan.planSubTripSeqNo,
                                                                truckJobTripPlan.planSubTripJobSeqNo,
                                                                truckJobTripPlan.jobID,
                                                                truckJobTripPlan.sequence);

                    cmd = new SqlCommand(SQLUpdateDriverString, cn);
                    cmd.Transaction = tran;
                    int rowaffected = 0;
                    rowaffected = cmd.ExecuteNonQuery();

                    if (rowaffected <= 0)
                    {
                        throw new FMException("No Row Updated");
                    }
                    else
                    {
                        temp = true;
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return temp;
        }

        internal static bool SetTruckJobChargeStatus(TruckJobCharge truckJobCharge, int invTrxNo, int jobID, SqlConnection con, SqlTransaction tran)
        {
            bool success = true;
            try
            {
                int status = 0;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;

                string sqlString = @"Update TRK_JOB_DETAIL_CHARGE_Tbl 
                                        set JOB_CHARGE_STATUS = {0}
                                        Where JOB_ID = {1}
                                            AND INV_TRAN_NO = {2}";

                sqlString = string.Format(sqlString, status, jobID, invTrxNo);

                SqlCommand cmd = new SqlCommand(sqlString, con);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return success;
        }

        internal static bool SetTruckJobChargeStatus(TruckJobCharge truckJobCharge, int invTrxNo, int invItemSeqNo, int jobID, SqlConnection con, SqlTransaction tran)
        {
            bool success = true;
            try
            {
                int status = 0;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;

                string sqlString = @"Update TRK_JOB_DETAIL_CHARGE_Tbl 
                                        set JOB_CHARGE_STATUS = {0}
                                        Where JOB_ID = {1}
                                               AND INV_TRAN_NO = {2}
                                               AND INV_ITEM_SEQ_NO = {3}";

                sqlString = string.Format(sqlString, status, jobID, invTrxNo, invItemSeqNo);

                SqlCommand cmd = new SqlCommand(sqlString, con);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return success;
        }

        internal static bool SetTruckJobChargeStatus(TruckJobCharge truckJobCharge, SqlConnection con, SqlTransaction tran)
        {
            bool success = true;
            try
            {
                int status = 0;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    status = 1;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    status = 2;
                if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    status = 3;

                string sqlString = @"Update TRK_JOB_DETAIL_CHARGE_Tbl 
                                        set JOB_CHARGE_STATUS = {0},
                                            INV_TRAN_NO = {1},
                                            INV_ITEM_SEQ_NO = {2}
                                        Where JOB_ID = {3}
                                            AND SEQUENCE_NO = {4}";

                sqlString = string.Format(sqlString, status, truckJobCharge.InvTranNo, truckJobCharge.InvItemSeqNo, truckJobCharge.JobID, truckJobCharge.SequenceNo);

                SqlCommand cmd = new SqlCommand(sqlString, con);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return success;
        }

        internal static SortableList<TruckJobCharge> GetInvoiceTruckJobCharges(TruckJob truckJob, JobChargeStatus jobChargeStatus, SqlConnection cn, SqlTransaction tran)
        {
            int status = 0;
            if (jobChargeStatus == JobChargeStatus.Booked)
                status = 1;
            if (jobChargeStatus == JobChargeStatus.Completed)
                status = 2;
            if (jobChargeStatus == JobChargeStatus.Invoiced)
                status = 3;

            SortableList<TruckJobCharge> Operators = new SortableList<TruckJobCharge>();
            try
            {
                string SQLString = @"SELECT * FROM TRK_JOB_DETAIL_CHARGE_Tbl  with (NOLOCK) 
                                    WHERE JOB_ID = {0}
                                        AND JOB_CHARGE_STATUS = {1}";

                SQLString = string.Format(SQLString, truckJob.JobID, status);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                if (tran == null) { tran = cn.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(SQLString, cn);

                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJobCharge(reader));
                }
                reader.Close();
            }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operators;
        }

        //Filter Data from Criteria
        #region Method for Criteria

        private static SortableList<TruckJobTrip> GetTruckJobTrips(string sqlWhere)
        {
            SortableList<TruckJobTrip> Operators = new SortableList<TruckJobTrip>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT D.UPDATE_VERSION AS UPD_VER,* FROM TRK_JOB_DETAIL_TRIP_Tbl as D with (NOLOCK) ";
                SQLString += " LEFT JOIN TRK_JOB_MAIN_Tbl as M with (NOLOCK)  ON D.JOB_ID = M.JOB_ID ";
                SQLString += sqlWhere;
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TruckJobTrip truckJobTrip = GetTruckJobTrip(reader);
                    truckJobTrip.truckJobTripPlans = truckJobTrip.GetTruckJobTripPlans();
                    Operators.Add(truckJobTrip);
                    //Operators.Add(GetTruckJobTrip(reader));
                }
                reader.Close();

            }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return Operators;
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByStatus(JobTripStatus status)
        {
            try
            {
                string sqlwhere;
                sqlwhere = @"WHERE D.STATUS={0}";
                sqlwhere = string.Format(sqlwhere, (int)status);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTripsByTransport(bool ownTransport)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.OWNTRANSPORT='{0}'";
                sqlwhere = string.Format(sqlwhere, strOwnTransport);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDeptAndTransport(string deptCode, bool ownTransport)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND M.TPT_DEPT_CODE ='{1}'";
                sqlwhere = string.Format(sqlwhere, strOwnTransport, deptCode);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTripsByStatusAndTransport(JobTripStatus status, bool ownTransport)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.STATUS='{0}' AND D.OWNTRANSPORT='{1}' ";
                sqlwhere = string.Format(sqlwhere, (int)status, strOwnTransport);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDept(string deptCode)
        {
            try
            {
                string sqlwhere;
                sqlwhere = "WHERE M.TPT_DEPT_CODE ='" + deptCode + "'";

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDeptAndStatus(JobTripStatus status, string deptCode)
        {
            try
            {
                string sqlwhere;
                sqlwhere = @"WHERE D.STATUS={0} AND M.TPT_DEPT_CODE ='{1}'";
                sqlwhere = string.Format(sqlwhere, (int)status, deptCode);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, string deptCode, bool ownTransport)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.STATUS='{0}' AND M.TPT_DEPT_CODE ='{1}' AND D.OWNTRANSPORT='{2}'";
                sqlwhere = string.Format(sqlwhere, (int)status, deptCode, strOwnTransport);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTrips(bool ownTransport, string jobNoFrom, string jobNoTo)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}'";
                sqlwhere = string.Format(sqlwhere, strOwnTransport, jobNoFrom, jobNoTo);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, bool ownTransport, string jobNoFrom, string jobNoTo, string deptCode)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.STATUS='{0}' AND D.OWNTRANSPORT='{1}' AND M.JOB_NUMBER >='{2}' AND M.JOB_NUMBER <='{3}' AND M.TPT_DEPT_CODE ='{4}'";
                sqlwhere = string.Format(sqlwhere, (int)status, strOwnTransport, jobNoFrom, jobNoTo, deptCode);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, string jobNoFrom, string jobNoTo, string deptCode)
        {
            try
            {
                string sqlwhere;
                sqlwhere = @"WHERE D.STATUS='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}' AND M.TPT_DEPT_CODE ='{3}'";
                sqlwhere = string.Format(sqlwhere, (int)status, jobNoFrom, jobNoTo, deptCode);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDate(DateTime jobDateFrom, DateTime jobDateTo)
        {
            try
            {
                string sqlwhere;
                sqlwhere = @"WHERE D.FROM_DATE >='{0}' AND D.TO_DATE <='{1}'";
                sqlwhere = string.Format(sqlwhere, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo));

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDate(bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}'";
                sqlwhere = string.Format(sqlwhere, strOwnTransport, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo));

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDate(bool ownTransport, string deptCode, DateTime jobDateFrom, DateTime jobDateTo)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}' AND M.TPT_DEPT_CODE ='{3}'";
                sqlwhere = string.Format(sqlwhere, strOwnTransport, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo), deptCode);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsForSetBookedToReady(bool hasSubCon, string deptCode, DateTime jobDateFrom, DateTime jobDateTo)
        {
            try
            {
                string strOwnTransport;
                string sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}' AND D.STATUS = 1";

                if (hasSubCon) strOwnTransport = "F";
                else strOwnTransport = "T";

                if (!deptCode.Equals(string.Empty))
                {
                    sqlwhere += " AND M.TPT_DEPT_CODE ='{3}'";
                    sqlwhere = string.Format(sqlwhere, strOwnTransport, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                                DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(jobDateTo), deptCode);
                }
                else
                {
                    sqlwhere = string.Format(sqlwhere, strOwnTransport, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                                DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(jobDateTo));
                }

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDate(JobTripStatus status, bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.STATUS = {0} AND D.OWNTRANSPORT='{1}' AND D.FROM_DATE >='{2}' AND D.TO_DATE <='{3}'";
                sqlwhere = string.Format(sqlwhere, (int)status, strOwnTransport, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo));

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDate(JobTripStatus status, DateTime jobDateFrom, DateTime jobDateTo)
        {
            try
            {
                string sqlwhere;
                sqlwhere = @"WHERE D.STATUS={0} AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}'";
                sqlwhere = string.Format(sqlwhere, (int)status, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo));

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByDate(string deptCode, DateTime jobDateFrom, DateTime jobDateTo)
        {
            try
            {
                string sqlwhere;
                sqlwhere = @"WHERE M.TPT_DEPT_CODE ='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}'";
                sqlwhere = string.Format(sqlwhere, deptCode, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo));

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByJob(string jobNoFrom, string jobNoTo)
        {
            try
            {
                string sqlwhere;
                sqlwhere = @"WHERE M.JOB_NUMBER >='{0}' AND M.JOB_NUMBER <='{1}'";
                sqlwhere = string.Format(sqlwhere, jobNoFrom, jobNoTo);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByJob(bool ownTransport, string jobNoFrom, string jobNoTo)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}'";
                sqlwhere = string.Format(sqlwhere, strOwnTransport, jobNoFrom, jobNoTo);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByJob(bool ownTransport, string jobNoFrom, string jobNoTo, string deptCode)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}' AND M.TPT_DEPT_CODE ='{3}'";
                sqlwhere = string.Format(sqlwhere, strOwnTransport, jobNoFrom, jobNoTo, deptCode);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTripsByJob(string deptCode, string jobNoFrom, string jobNoTo)
        {
            try
            {
                string sqlwhere;
                sqlwhere = @"WHERE M.TPT_DEPT_CODE ='{0}' AND M.JOB_NUMBER >='{1}' AND M.JOB_NUMBER <='{2}'";
                sqlwhere = string.Format(sqlwhere, deptCode, jobNoFrom, jobNoTo);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTrips(bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            try
            {
                string sqlwhere;
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @"WHERE D.OWNTRANSPORT='{0}' AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}'";
                sqlwhere = string.Format(sqlwhere, strOwnTransport,
                                            DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo));

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo, string deptCode)
        {
            try
            {
                string sqlwhere = " WHERE 1=1 ";
                string strOwnTransport;

                if (ownTransport) strOwnTransport = "T";
                else strOwnTransport = "F";

                sqlwhere = @" AND D.STATUS={0} AND D.OWNTRANSPORT='{1}' AND D.FROM_DATE >='{2}' AND D.TO_DATE <='{3}' AND M.TPT_DEPT_CODE ='{4}'";
                sqlwhere = string.Format(sqlwhere, (int)status, strOwnTransport,
                                            DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo),
                                            deptCode);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, DateTime jobDateFrom, DateTime jobDateTo, string deptCode)
        {
            string sqlwhere = " WHERE 1=1 ";
            try
            {
                sqlwhere = @" AND D.STATUS={0} AND D.FROM_DATE >='{1}' AND D.TO_DATE <='{2}' AND M.TPT_DEPT_CODE ='{3}'";
                sqlwhere = string.Format(sqlwhere, (int)status, DateUtility.ConvertDateForSQLPurpose(jobDateFrom),
                                            DateUtility.ConvertDateForSQLPurpose(jobDateTo),
                                            deptCode);

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        //20131009 - Gerry added
        internal static SortableList<TruckJobTrip> GetTruckJobTripsToChangeStatus(int currentStatus, bool? ownTransport, DateTime? jobDateFrom, DateTime? jobDateTo, string jobNoFrom, string jobNoTo, string deptCode)
        {
            try
            {
                string sqlwhere = " WHERE 1=1 ";
                sqlwhere += @" AND M.TPT_DEPT_CODE ='{0}' ";
                sqlwhere = string.Format(sqlwhere, deptCode);

                #region Status Criteria
                if (currentStatus != 0)
                {
                    sqlwhere += " AND D.STATUS={0} ";
                    sqlwhere = string.Format(sqlwhere, (int)currentStatus);
                }
                #endregion
                #region Date Criteria
                if (jobDateFrom != null || jobDateTo != null)
                {
                    sqlwhere += "AND D.FROM_DATE >='{0}' AND D.TO_DATE <='{1}'  ";
                    if (jobDateFrom != null && jobDateTo != null)
                    {
                        sqlwhere = string.Format(sqlwhere, DateUtility.ConvertDateForSQLPurpose((DateTime)jobDateFrom),
                                                 DateUtility.ConvertDateForSQLPurpose((DateTime)jobDateTo));
                    }
                    else if (jobDateFrom != null && jobDateTo == null)
                    {
                        sqlwhere = string.Format(sqlwhere, DateUtility.ConvertDateForSQLPurpose((DateTime)jobDateFrom),
                                                 DateUtility.ConvertDateForSQLPurpose((DateTime)jobDateFrom));
                    }
                    else if (jobDateFrom == null && jobDateTo != null)
                    {
                        sqlwhere = string.Format(sqlwhere, DateUtility.ConvertDateForSQLPurpose((DateTime)jobDateTo),
                                                 DateUtility.ConvertDateForSQLPurpose((DateTime)jobDateTo));
                    }
                }
                #endregion
                #region JobNo Criteria
                if (!jobNoFrom.Equals(string.Empty) || !jobNoTo.Equals(string.Empty))
                {
                    sqlwhere += " AND M.JOB_NUMBER >='{0}' AND M.JOB_NUMBER <='{1}' ";
                    if (!jobNoFrom.Equals(string.Empty) && jobNoTo.Equals(string.Empty))
                    {
                        sqlwhere = string.Format(sqlwhere, CommonUtilities.FormatString(jobNoFrom), CommonUtilities.FormatString(jobNoFrom));
                    }
                    else if (jobNoFrom.Equals(string.Empty) && !jobNoTo.Equals(string.Empty))
                    {
                        sqlwhere = string.Format(sqlwhere, CommonUtilities.FormatString(jobNoTo), CommonUtilities.FormatString(jobNoTo));
                    }
                    else if (!jobNoFrom.Equals(string.Empty) && !jobNoTo.Equals(string.Empty))
                    {
                        sqlwhere = string.Format(sqlwhere, CommonUtilities.FormatString(jobNoFrom), CommonUtilities.FormatString(jobNoTo));
                    }
                }
                #endregion
                #region Owntransport Criteria
                if (ownTransport != null)
                {
                    sqlwhere += " AND D.OWNTRANSPORT='{0}' ";
                    sqlwhere = string.Format(sqlwhere, (bool)ownTransport ? "T" : "F");

                }
                #endregion

                return GetTruckJobTrips(sqlwhere);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }


        #endregion

        internal static bool IsJobBilled(string jobNo)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM ACT_IV_Transport_Invoice_Master_Tbl with (NOLOCK)  ";
                SQLString += " WHERE ACT_IV_Transport_Invoice_Master_Tbl.Job_Number='" + jobNo + "'";


                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
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
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static List<string> GetTruckJobNos()
        {
            List<string> list = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK) INNER JOIN TRK_JOB_SHIP_AIR_INFO_Tbl with (NOLOCK) ";
                SQLString += " ON TRK_JOB_MAIN_Tbl.JOB_ID=TRK_JOB_SHIP_AIR_INFO_Tbl.JOB_ID";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return list;

        }

        internal static ArrayList GetChargeUOMs(int jobID, string chargeCode)
        {
            ArrayList temp = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());

                string sqlSelect = @"Select UOM from TRK_JOB_DETAIL_CHARGE_Tbl with (NOLOCK)  Where JOB_ID = {0} and CHARGE_CODE = '{1}'";

                sqlSelect = string.Format(sqlSelect, jobID, chargeCode);

                SqlCommand cmd = new SqlCommand(sqlSelect, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp.Add(reader.GetString(0).ToString());
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return temp;
        }

        internal static TruckJobTripPlan GetTruckJobTripPlan(SqlDataReader reader)
        {
            JobTripStatus tempStatus = new JobTripStatus();
            try
            {
                switch ((int)reader["STATUS"])
                {
                    case 1:
                        tempStatus = JobTripStatus.Booked;
                        break;
                    case 2:
                        tempStatus = JobTripStatus.Ready;
                        break;
                    case 3:
                        tempStatus = JobTripStatus.Assigned;
                        break;
                    case 4:
                        tempStatus = JobTripStatus.Completed;
                        break;
                    case 5:
                        tempStatus = JobTripStatus.Invoiced;
                        break;
                }
                TruckJobTripPlan tempTruckJobTripPlan = new TruckJobTripPlan(
                    (DateTime)reader["SCHEDULE_DATE"],
                    (string)reader["PLANTRIP_NO"],
                    (string)reader["DRIVER_NO"],
                    (string)reader["VEHICLE_NO"],
                    (Decimal)reader["VOLUME"],
                    (Decimal)reader["WEIGHT"],
                    (byte[])reader["UPDATE_VERSION"],
                    (DateTime)reader["START_TIME"],
                    (DateTime)reader["END_TIME"],
                    tempStatus,
                    (int)reader["JOBTRIP_PLAN_SEQ"]
                    );
                return tempTruckJobTripPlan;

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        internal static SortableList<TruckJobTripPlan> GetTruckJobTripPlanByDriver(string driveCode, DateTime schedDate, string deptCode)
        {
            SortableList<TruckJobTripPlan> tempTruckJobTripPlanList = new SortableList<TruckJobTripPlan>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                con.Open();
                string SQLQuery = @"select * from TRK_JOB_DETAIL_TRIP_PLAN_TBL as a with (NOLOCK) 
                                        inner join TRK_PLAN_TRIP_TBL as b with (NOLOCK) 
                                        ON b.SCHEDULE_DATE = a.SCHEDULE_DATE
                                        and b.DRIVER_NO = a.DRIVER_NO 

                                        where a.DRIVER_NO = '{0}'
                                            and a.SCHEDULE_DATE = '{1}'
                                            and b.DEPT = '{2}'";


                SQLQuery = string.Format(SQLQuery, driveCode, DateUtility.ConvertDateForSQLPurpose(schedDate), deptCode);

                SqlCommand comm = new SqlCommand(SQLQuery, con);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    tempTruckJobTripPlanList.Add(GetTruckJobTripPlan(reader));
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return tempTruckJobTripPlanList;
        }

        internal static ArrayList GetJobNosForPlanningReport(string driver, DateTime scheduleDate, string deptCode)
        {
            ArrayList jobNos = new ArrayList();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            #region Query
            string SQLString = @"select distinct jobMain.JOB_NUMBER
                                     from TRK_PLAN_SUB_TRIP_JOB_TBL as plSubTripJob with (NOLOCK) 
                                     inner join TRK_PLAN_SUB_TRIP_TBL as plSubTrip with (NOLOCK) 
                                    on plSubTrip.PLANTRIP_NO = plSubTripJob.PLANTRIP_NO 
                                    and plSubTrip.SEQ_NO = plSubTripJob.PLANSUBTRIP_SEQNO
                                    inner join TRK_PLAN_TRIP_TBL as plTrip with (NOLOCK) 
                                    on plTrip.PLANTRIP_NO = plSubTripJob.PLANTRIP_NO
                                    inner join TRK_JOB_MAIN_Tbl as jobMain with (NOLOCK) 
                                    on jobMain.JOB_ID = plSubTripJob.JOB_ID

                                    where plTrip.DRIVER_NO = '{0}'
                                    and plTrip.SCHEDULE_DATE ='{1}'   
                                    and plTrip.DEPT = '{2}'                                 
                                 ";
            SQLString = string.Format(SQLString, driver, DateUtility.ConvertDateForSQLPurpose(scheduleDate), deptCode);

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
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return jobNos;
        }

        internal static ArrayList GetJobTripSeqNosForPlanningReport(string driver, DateTime scheduleDate, int jobID, string deptCode)
        {
            ArrayList jobSeqNos = new ArrayList();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            #region Query
            string SQLString = @"SELECT jtp.JOBTRIP_SEQ_NO 
                                    FROM TRK_JOB_DETAIL_TRIP_Tbl as jt with (NOLOCK) 
                                    LEFT JOIN TRK_JOB_MAIN_Tbl jobMain with (NOLOCK) 
                                    ON jt.JOB_ID=jobMain.JOB_ID  
                                    left JOIN TRK_JOB_DETAIL_TRIP_PLAN_TBL as jtp with (NOLOCK) 
                                    ON jtp.JOB_ID = jt.JOB_ID
                                    and jtp.JOBTRIP_SEQ_NO = jt.JOBTRIP_SEQ_NO

                                    WHERE 
                                    jtp.DRIVER_NO = '{0}'
                                    and jtp.SCHEDULE_DATE ='{1}'
                                    and jtp.JOB_ID = '{2}'
                                    and jobMain.TPT_DEPT_CODE = '{3}'                                     
                                 ";
            SQLString = string.Format(SQLString, driver, DateUtility.ConvertDateForSQLPurpose(scheduleDate), jobID, deptCode);

            #endregion
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobSeqNos.Add(reader.GetInt32(0).ToString());
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return jobSeqNos;
        }

        internal static bool UpdateTruckJobTripPlanWeightVol(TruckJobTripPlan truckJobTripPlan, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                SqlCommand cmd;
                byte[] origVersion = new byte[8];
                #region Check Version
                string sqlCheckVersion = @"Select * from TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK) 
                                        where PLANTRIP_NO = '{0}'
                                                AND JOBTRIP_PLAN_SEQ = {1}
                                                AND JOB_ID = {2}
                                                AND JOBTRIP_SEQ_NO = {3}";

                sqlCheckVersion = string.Format(sqlCheckVersion, truckJobTripPlan.planTripNo,
                                                                truckJobTripPlan.planSubTripJobSeqNo,
                                                                truckJobTripPlan.jobID,
                                                                truckJobTripPlan.sequence);

                cmd = new SqlCommand(sqlCheckVersion, con);
                cmd.Transaction = tran;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    origVersion = (byte[])reader["UPDATE_VERSION"];
                }
                reader.Close();
                #endregion
                if (!SqlBinary.Equals(origVersion, truckJobTripPlan.updateVersion))
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict + "\nTruckJobDAL.UpdateTruckJobTripPlanWeightVol");
                }
                else
                {
                    #region SQLUpdate
                    string SQLUpdate = @" Update TRK_JOB_DETAIL_TRIP_PLAN_TBL
                                        set VOLUME = '{0}',
                                            WEIGHT = '{1}'
                                        Where JOB_ID = {2}
                                            and JOBTRIP_SEQ_NO = {3}
                                            and PLANTRIP_NO ='{4}'
                                            and JOBTRIP_PLAN_SEQ ={5}";
                    SQLUpdate = string.Format(SQLUpdate, truckJobTripPlan.volume,
                                                    truckJobTripPlan.weight,
                                                    truckJobTripPlan.jobID,
                                                    truckJobTripPlan.sequence,
                                                    truckJobTripPlan.planTripNo,
                                                    truckJobTripPlan.planSubTripJobSeqNo);
                    #endregion

                    cmd = new SqlCommand(SQLUpdate, con);
                    cmd.Transaction = tran;
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();

                    retValue = true;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        public static TruckJobTripPlan GetTruckJobTripPlan(int jobid, int jobtripseq, string plantripNo, int planSubTripSeqNo, int jobTripPlanSeq,
            SqlConnection con, SqlTransaction tran)
        {
            TruckJobTripPlan Operator = new TruckJobTripPlan();
            try
            {
                //20160129 - gerry modified query add parameter planSubTripSeqNo
                string SQLString = @"SELECT * FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL  with (NOLOCK) 
                                    WHERE JOB_ID = {0}
                                    AND JOBTRIP_SEQ_NO = {1}
                                    AND PLANTRIP_NO = '{2}'
                                    AND PLANSUBTRIP_SEQNO = '{3}'
                                    AND PLANSUBTRIPJOB_SEQNO = {4}
                                    ";
                SQLString = string.Format(SQLString, jobid, jobtripseq, plantripNo, planSubTripSeqNo, jobTripPlanSeq);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetTruckJobTripPlan(reader);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return Operator;
        }
        //Check AirJob already exist
        internal static bool DoesAirJobNumberExist(string airJobNo, out string outTruckJobNo)
        {
            bool retValue = false;
            outTruckJobNo = string.Empty;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string SQLString = "SELECT * FROM TRK_JOB_MAIN_Tbl with (NOLOCK)  where SOURCE_REF_NUMBER = '" + airJobNo + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    outTruckJobNo = (string)reader["JOB_NUMBER"];
                    retValue = true;
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }
            return retValue;
        }

        internal static bool DeleteAllTruckMovementCharges(int jobID, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string query = @"DELETE FROM TRK_JOB_DETAIL_CHARGE_Tbl
 	                                WHERE 
	                                  JOB_ID={0} AND
	                                  JOB_CHARGE_TYPE='T'";

                query = string.Format(query, jobID);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }

        #region "2013-09-09 Zhou Kai starts to add"
        /*
         * 2013-09-09 Zhou Kai adds function, to update actual 
         * weight/volume from Planning side to Booking side
         */
        internal static bool UpdateAcutalWeightVolumeforTruckJobTrip(int jobID, int jobTripSeqNo, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            #region 20131206 Gerry Removed
            /*
            decimal dAcutalWeightSum = 0M; // actual truck job trip weight
            decimal dActualVolumeSum = 0M; // actual truck job trip volume
            string sqlSelectWeightSum = "SELECT SUM(WEIGHT) FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL WHERE " +
                                        "JOB_ID = @JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            string sqlSelectVolumeSum = "SELECT SUM(VOLUME) FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL WHERE " +
                                        "JOB_ID = @JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            SqlCommand cmd = new SqlCommand(sqlSelectWeightSum, con);
            cmd.Transaction = tran;
             */
            #endregion
            try
            {
                #region 20131206 - Removed
                /*
                cmd.Parameters.AddWithValue("JobId", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                dAcutalWeightSum = Convert.ToDecimal(cmd.ExecuteScalar());
                cmd = new SqlCommand(sqlSelectVolumeSum, con);
                cmd.Parameters.AddWithValue("JobId", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                cmd.Transaction = tran;
                dActualVolumeSum = Convert.ToDecimal(cmd.ExecuteScalar());
                
                string sqlUpdateActualWeightVolume = "UPDATE TRK_JOB_DETAIL_TRIP_TBL SET ACT_WEIGHT = " +
                                        "@ActWeightSum, ACT_VOLUME = @ActVolumeSum WHERE JOB_ID = " +
                                        "@JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
                cmd = new SqlCommand(sqlUpdateActualWeightVolume, con);
                cmd.Parameters.AddWithValue("ActWeightSum", dAcutalWeightSum);
                cmd.Parameters.AddWithValue("ActVolumeSum", dActualVolumeSum);
                cmd.Parameters.AddWithValue("JobId", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                 */
                #endregion
                string updateQuery = @"UPDATE TRK_JOB_DETAIL_TRIP_TBL 
                                        SET ACT_WEIGHT = ISNULL((SELECT SUM(WEIGHT) FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  WHERE
						                                        JOB_ID = {0} AND JOBTRIP_SEQ_NO = {1}), BOOK_WEIGHT), 
                                        ACT_VOLUME = ISNULL((SELECT SUM(VOLUME) FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  WHERE
						                                        JOB_ID = {0} AND JOBTRIP_SEQ_NO = {1}), BOOK_VOLUME)
                                         
                                        WHERE JOB_ID = {0} 
                                        AND JOBTRIP_SEQ_NO = {1}";
                updateQuery = string.Format(updateQuery, jobID, jobTripSeqNo);
                SqlCommand cmd = new SqlCommand(updateQuery, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }  //gerry replace catch clause
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }



        internal static bool UpdateAcutalWeightVolumeforTruckJobTrip(int jobID, int jobTripSeqNo)
        {
            bool retValue = false;
            // Select sum of weight/volume of all truckJobTripPlans belongs
            // to the truckJobTrip
            decimal dAcutalWeightSum = 0M; // actual truck job trip weight
            decimal dActualVolumeSum = 0M; // actual truck job trip volume
            string sqlSelectWeightSum = "SELECT SUM(WEIGHT) FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  WHERE " +
                                        "JOB_ID = @JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            string sqlSelectVolumeSum = "SELECT SUM(VOLUME) FROM TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  WHERE " +
                                        "JOB_ID = @JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand cmd = new SqlCommand(sqlSelectWeightSum, con);
            try
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlTransaction tran = con.BeginTransaction();
                cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("JobId", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                dAcutalWeightSum = Convert.ToDecimal(cmd.ExecuteScalar());
                cmd = new SqlCommand(sqlSelectVolumeSum, con);
                cmd.Parameters.AddWithValue("JobId", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                cmd.Transaction = tran;
                dActualVolumeSum = Convert.ToDecimal(cmd.ExecuteScalar());

                string sqlUpdateActualWeightVolume = "UPDATE TRK_JOB_DETAIL_TRIP_TBL SET ACT_WEIGHT = " +
                                        "@ActWeightSum, ACT_VOLUME = @ActVolumeSum WHERE JOB_ID = " +
                                        "@JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
                cmd = new SqlCommand(sqlUpdateActualWeightVolume, con);
                cmd.Parameters.AddWithValue("ActWeightSum", dAcutalWeightSum);
                cmd.Parameters.AddWithValue("ActVolumeSum", dActualVolumeSum);
                cmd.Parameters.AddWithValue("JobId", jobID);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                // 2013-11-18 Zhou Kai adds
                tran.Commit();
                // 2013-11-18 Zhou Kai ends
                retValue = true;
            }  //gerry replace catch clause
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            // 2013-11-18 Zhou Kai starts
            finally { con.Close(); }
            // 2013-11-18 Zhou Kai ends

            return retValue;
        }

        /*
         * 2013-09-10 Zhou Kai adds this function, 
         * to convert a DateTime to a 4 bit shour time format,
         * example 2013-09-10 23:00:00 --> 2300
         */
        internal static string To4BitTimeFormat(DateTime origDateTime)
        {
            string str4BitTime = String.Empty;
            if (origDateTime.Hour < 10)
            {
                str4BitTime = "0" + origDateTime.Hour.ToString();
            }
            else
            {
                str4BitTime = origDateTime.Hour.ToString();
            }
            if (origDateTime.Minute < 10)
            {
                str4BitTime = str4BitTime + "0" + origDateTime.Minute.ToString();
            }
            else
            {
                str4BitTime = str4BitTime + origDateTime.Minute.ToString();
            }

            return str4BitTime;
        }

        // 2013-10-24 Zhou Kai modifies
        internal static bool GetActualStartEndTime(int jobID, int jobTripSeqNo,
                                                   string earliestStartTime, string latestEndTime,
                                                   SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            string sqlGetEarliestStartTime = "SELECT MIN(START_TIME) FROM " +
                                            " TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  WHERE " +
                                            " TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOB_ID = " +
                                            jobID + " AND TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOBTRIP_SEQ_NO = " +
                                            jobTripSeqNo + ";";
            string sqlGetLatestEndTime = "SELECT MAX(END_TIME) FROM " +
                                            " TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  WHERE " +
                                            " TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOB_ID = " +
                                            jobID + " AND TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOBTRIP_SEQ_NO = " +
                                            jobTripSeqNo + ";";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlGetEarliestStartTime, con);
                cmd.Transaction = tran;

                if (ReferenceEquals(cmd.ExecuteScalar(), System.DBNull.Value)) { earliestStartTime = String.Empty; }
                else { earliestStartTime = Convert.ToDateTime(cmd.ExecuteScalar()).ToString("HH:mm:ss"); }
                cmd = new SqlCommand(sqlGetLatestEndTime, con);
                if (ReferenceEquals(cmd.ExecuteScalar(), System.DBNull.Value)) { latestEndTime = String.Empty; }
                else { latestEndTime = Convert.ToDateTime(cmd.ExecuteScalar()).ToString("HH:mm:ss"); }
                if (earliestStartTime.Length * latestEndTime.Length == 0) { return false; }


                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }
        internal static bool GetActualStartEndTime(int jobID, int jobTripSeqNo,
                                                   out string earliestStartTime, out string latestEndTime)
        {
            bool retValue = false;
            string sqlGetEarliestStartTime = "SELECT MIN(START_TIME) FROM " +
                                            " TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  WHERE " +
                                            " TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOB_ID = " +
                                            jobID + " AND TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOBTRIP_SEQ_NO = " +
                                            jobTripSeqNo + ";";
            string sqlGetLatestEndTime = "SELECT MAX(END_TIME) FROM " +
                                            " TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK)  WHERE " +
                                            " TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOB_ID = " +
                                            jobID + " AND TRK_JOB_DETAIL_TRIP_PLAN_TBL.JOBTRIP_SEQ_NO = " +
                                            jobTripSeqNo + ";";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand(sqlGetEarliestStartTime, con);

                if (ReferenceEquals(cmd.ExecuteScalar(), System.DBNull.Value)) { earliestStartTime = String.Empty; }
                else { earliestStartTime = Convert.ToDateTime(cmd.ExecuteScalar()).ToString("yyyy-MM-dd HH:mm:ss"); }

                cmd = new SqlCommand(sqlGetLatestEndTime, con);
                if (ReferenceEquals(cmd.ExecuteScalar(), System.DBNull.Value)) { latestEndTime = String.Empty; }
                else { latestEndTime = Convert.ToDateTime(cmd.ExecuteScalar()).ToString("yyyy-MM-dd HH:mm:ss"); }

                if (earliestStartTime.Length * latestEndTime.Length == 0) { return false; }
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { earliestStartTime = String.Empty; latestEndTime = String.Empty; return false; }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }
        // 2013-10-24 ends

        // 2013-10-24 Zhou Kai starts to modify
        internal static bool AreAllJobTripsCompleteForOwnTransportJob(int jobId, out string message)
        {
            int nJobTrips = 0;
            message = string.Empty;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand cmd = new SqlCommand();
            string sqlQuery = "SELECT JOB_ID, JOBTRIP_SEQ_NO, STATUS FROM " +
                          "TRK_JOB_DETAIL_TRIP_TBL with (NOLOCK)  WHERE JOB_ID = @JobId;";
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd = new SqlCommand(sqlQuery, cn);
                cmd.Parameters.AddWithValue("JobId", jobId);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    if (Convert.ToInt32(sr["Status"].ToString()) != (int)JobTripStatus.Completed)
                    { message = message + sr["JOBTRIP_SEQ_NO"].ToString() + ","; }
                    nJobTrips++;
                }
                sr.Close();
                if (nJobTrips == 0) { message = TptResourceUI.NoJobTripsYet; return false; }
                if (!message.Equals(String.Empty))
                {
                    message = TptResourceDAL.TheFollowingJobTripsNotComplete
                              + "\nJobID: " + jobId.ToString() + ", SeqNo: "
                              + message;
                    return false;
                }
                return true;
            }
            catch (IndexOutOfRangeException ie) { return false; }
            finally { cn.Close(); }
        }

        internal static bool AreAllJobTripsCompleteForSubConJob(int jobId, out string message)
        {
            return (AreAllJobTripsCompleteForOwnTransportJob(jobId, out message) &&
                AreAllSubConDataComplete(jobId, out message));
        }
        // 2013-10-25 Zhou Kai ends modifying


        //2013-09-25 Zhou Kai adds this function
        internal static SortableList<TruckJobTripSubCon> GetAllTruckJobTripSubCon(int jobId)
        {
            SortableList<TruckJobTripSubCon> tmp = new SortableList<TruckJobTripSubCon>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            TruckJobTripSubCon Operator = new TruckJobTripSubCon();
            string strSql = "SELECT TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.UPDATE_VERSION AS time_stamp, * " +
                            "FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  INNER JOIN " +
                            "TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  ON " +
                            "(TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.JOB_ID =  TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID " +
                            " AND TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO)" +
                            " WHERE TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = @JobId" +
                            " ORDER BY TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO DESC";
            SqlCommand cmd = new SqlCommand(strSql, cn);
            cmd.Parameters.AddWithValue("JobId", jobId);
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetTruckJobTripSubCon(reader);
                    tmp.Add(Operator);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }
            return tmp;
        }

        internal static TruckJobTripSubCon GetTruckJobTripSubCon(int jobId, int jobTripSeqNo)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            TruckJobTripSubCon Operator = new TruckJobTripSubCon();
            string strSql = "SELECT TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.UPDATE_VERSION AS time_stamp, * " +
                            "FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  INNER JOIN " +
                            "TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  ON " +
                            "(TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.JOB_ID =  TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID " +
                            " AND TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO)" +
                            " WHERE TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = @JobId AND TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO = @JobTripSeqNo" +
                            " ORDER BY TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO DESC";

            SqlCommand cmd = new SqlCommand(strSql, cn);
            cmd.Parameters.AddWithValue("JobId", jobId);
            cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                int i = 0;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    i++;
                    Operator = GetTruckJobTripSubCon(reader);
                }
                if (i == 0)
                {
                    Operator = null;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }
            return Operator;
        }

        internal static SortableList<TruckJobTripSubCon> GetAllSubConDataWithTruckJobTripCompleted(int jobId)
        {
            SortableList<TruckJobTripSubCon> tmp = new SortableList<TruckJobTripSubCon>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            TruckJobTripSubCon Operator = new TruckJobTripSubCon();
            string strSql = "SELECT TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.UPDATE_VERSION AS time_stamp, * " +
                            "FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  INNER JOIN " +
                            "TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  ON " +
                            "(TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.JOB_ID =  TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID " +
                            " AND TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO)" +
                            " WHERE TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = @JobId AND STATUS = 4 " +
                            " ORDER BY TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO DESC";
            SqlCommand cmd = new SqlCommand(strSql, cn);
            cmd.Parameters.AddWithValue("JobId", jobId);
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operator = GetTruckJobTripSubCon(reader);
                    tmp.Add(Operator);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }
            return tmp;
        }
        #endregion

        #region "2013-10-08 Zhou Kai adds"
        internal static TruckJobTripSubCon GetTruckJobTripSubCon(IDataReader reader)
        {
            TruckJobTripSubCon Operator = new TruckJobTripSubCon();
            int jobID = Convert.ToInt32(reader["JOB_ID"]);
            int jobTripSeqNo = Convert.ToInt32(reader["JOBTRIP_SEQ_NO"]);
            // 2013-11-04 Zhou Kai modifies to format the date-time string
            string waitTimeFrom = ReferenceEquals(reader["WAIT_TIME_FR_ACT"], DBNull.Value) ?
                String.Empty : DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(Convert.ToDateTime(reader["WAIT_TIME_FR_ACT"]));
            string waitTimeTo = ReferenceEquals(reader["WAIT_TIME_TO_ACT"], DBNull.Value) ?
                String.Empty : DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(Convert.ToDateTime(reader["WAIT_TIME_TO_ACT"]));
            DateTime dateTimeFromAct = Convert.ToDateTime(reader["FR_DATE_ACT"]);
            DateTime dateTimeToAct = Convert.ToDateTime(reader["TO_DATE_ACT"]);
            // 2013-11-04 Zhou Kai ends
            string driver = Convert.ToString(reader["DRIVER"]);
            string vehicle = Convert.ToString(reader["VEHICLE"]);
            int noOfPackage = GetNoOfPackageWhenUOMIsNotPkg(jobID, jobTripSeqNo);
            int noOfPallet = GetNoOfPackageWhenUOMIsPkg(jobID, jobTripSeqNo); ; // for Jupiter Vietnam only
            string operatorTruckCapacity = Convert.ToString(reader["OPERATORTRUCKCAPACITY"]);
            string reqCusTruckCapacity = GetReqCusomerTruckCapacity(jobID, jobTripSeqNo);
            string ChargeCode = Convert.ToString(reader["CHARGE_CODE"]);
            string remarks1 = Convert.ToString(reader["REMARKS_1"]);
            string remarks2 = Convert.ToString(reader["REMARKS_2"]);
            string remarks3 = Convert.ToString(reader["REMARKS_3"]);
            string remarks4 = Convert.ToString(reader["REMARKS_4"]);
            string remarks5 = Convert.ToString(reader["REMARKS_5"]);

            // 2014-03-21 Zhou Kai adds
            string subConCode = Convert.ToString(reader["SUBCONTRACTOR_CODE"]);
            byte[] UpdateVersion = (byte[])(reader["time_stamp"]);
            decimal actVolume = Convert.ToDecimal(reader["ACT_VOLUME"]);
            decimal actWeight = Convert.ToDecimal(reader["ACT_WEIGHT"]);


            Operator = new TruckJobTripSubCon(jobID, jobTripSeqNo, waitTimeFrom, waitTimeTo, dateTimeFromAct,
                                              dateTimeToAct, driver, vehicle, actVolume, actWeight, noOfPackage, noOfPallet, reqCusTruckCapacity,
                                              operatorTruckCapacity, remarks1, remarks2, remarks3, remarks4, remarks5, subConCode, UpdateVersion);


            return Operator;
        }
        /*2013-10-08 Zhou Kai ends*/

        /*2013-10-08 Zhou Kai adds:
         * Get the quantity of packages while a truck job trip charge UOM 
         * is not Package Type. Logic:
         * (1) ChargeCode --> Charge --> IS_PACKAGE_DEPENDENT
         */
        internal static int GetNoOfPackageWhenUOMIsNotPkg(int jobId, int jobSeqNo)
        {
            int retValue = 0;
            Charge charge;
            bool hasDetails = false;
            int nBillingQty = 0;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string strSql = "SELECT WT_VOL_FR_DETAILS, BRANCH_CODE, CHARGE_CODE, IS_MULTI_PACKAGE, PACK_TYPE, BILLING_QTY " +
                            "FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  " +
                            "INNER JOIN TRK_JOB_MAIN_Tbl with (NOLOCK)  ON TRK_JOB_MAIN_Tbl.JOB_ID = TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID " +
                            "AND TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = @JobId" + " AND JOBTRIP_SEQ_NO = @JobTripSeqNo";
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(strSql, cn);
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobSeqNo);
                SqlDataReader sr = cmd.ExecuteReader();
                if (sr.Read())
                {
                    // check if it's charged by package
                    charge = Charge.GetCharge(Convert.ToString(sr["CHARGE_CODE"]), Convert.ToString(sr["BRANCH_CODE"]));
                    hasDetails = Convert.ToChar(sr["WT_VOL_FR_DETAILS"]) == 'T' ? true : false;
                    if (!hasDetails) { nBillingQty = Convert.ToInt32(sr["BILLING_QTY"]); }
                    sr.Close();
                    if (charge.invoiceChargeType == InvoiceChargeType.DependOnPackagetype) { return 0; }

                    /* 2013-10-28 Zhou Kai modifies the logic */
                    if (hasDetails)
                    {
                        strSql = "SELECT SUM(QTY) FROM TRK_JOB_DETAIL_TRIP_DETAIL_Tbl with (NOLOCK)  WHERE " +
                         "JOB_ID = @JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
                        cmd = new SqlCommand(strSql, cn);
                        cmd.Parameters.AddWithValue("JobId", jobId);
                        cmd.Parameters.AddWithValue("JobTripSeqNo", jobSeqNo);
                        retValue = ReferenceEquals(cmd.ExecuteScalar(), DBNull.Value) ? 0 :
                            Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    else { return 0; }
                    // 2013-10-28 Zhou Kai ends
                }
                else
                {
                    throw new FMException(TptResourceDAL.NoSuchJobTrip + " JobID: " + jobId.ToString() +
                                          " JobTripSeqNo: " + jobSeqNo.ToString());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return retValue;
        }
        internal static int GetNoOfPackageWhenUOMIsPkg(int jobId, int jobSeqNo)
        {
            int retValue = 0;
            bool isMultiPackageBased = false;
            Charge charge;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string strSql = "SELECT BRANCH_CODE, CHARGE_CODE, IS_MULTI_PACKAGE, PACK_TYPE, BILLING_QTY FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  " +
                            "INNER JOIN TRK_JOB_MAIN_Tbl with (NOLOCK)  ON TRK_JOB_MAIN_Tbl.JOB_ID = TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID " +
                            " AND TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = @JobId" + " AND JOBTRIP_SEQ_NO = @JobTripSeqNo";
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(strSql, cn);
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobSeqNo);
                SqlDataReader sr = cmd.ExecuteReader();
                if (sr.Read())
                {
                    // check if it's charged by package
                    charge = Charge.GetCharge(Convert.ToString(sr["CHARGE_CODE"]), Convert.ToString(sr["BRANCH_CODE"]));
                    if (charge.invoiceChargeType != InvoiceChargeType.DependOnPackagetype) { return 0; }
                    // check if it's charged by multipul package type
                    isMultiPackageBased = Convert.ToString(sr["IS_MULTI_PACKAGE"]) == "T" ? true : false;
                    if (!isMultiPackageBased) // single package type
                    {
                        retValue = Convert.ToInt32(sr["BILLING_QTY"]);
                    }
                    // 2013-10-28 Zhou Kai adds logic
                    if (isMultiPackageBased) // multipackage type or not by package
                    {
                        sr.Close();
                        strSql = "SELECT SUM(QTY) FROM TRK_JOB_DETAIL_TRIP_DETAIL_Tbl with (NOLOCK)  WHERE " +
                                 "JOB_ID = '" + jobId.ToString() + "' AND JOBTRIP_SEQ_NO = '" +
                                 jobSeqNo.ToString() + "';";
                        cmd = new SqlCommand(strSql, cn);
                        retValue = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // 2013-10-28 Zhou Kai ends
                }
                else
                {
                    throw new FMException(TptResourceDAL.NoSuchJobTrip + " JobID: " + jobId.ToString() +
                                          " JobTripSeqNo: " + jobSeqNo.ToString());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return retValue;
        }
        /*2013-10-08 Zhou Kai ends*/

        /*2013-10-08 Zhou Kai adds*/
        internal static string GetReqCusomerTruckCapacity(int jobId, int jobTripSeqNo)
        {
            string billing_uom = String.Empty;
            Charge charge;
            string strSql = "SELECT TRK_JOB_MAIN_Tbl.BRANCH_CODE, CHARGE_CODE, BILLING_UOM FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  " +
                            "INNER JOIN TRK_JOB_MAIN_Tbl with (NOLOCK)  ON TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = TRK_JOB_MAIN_Tbl.JOB_ID " +
                            "AND TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = @JobId " +
                            "AND TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(strSql, cn);
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                SqlDataReader sr = cmd.ExecuteReader();
                if (sr.Read())
                {
                    charge = Charge.GetCharge(Convert.ToString(sr["CHARGE_CODE"]), Convert.ToString(sr["BRANCH_CODE"]));
                    if (charge.invoiceChargeType == InvoiceChargeType.DependOnTruckCapacity)
                    {
                        return Convert.ToString(sr["BILLING_UOM"]);
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                else
                {
                    throw new FMException(TptResourceDAL.NoSuchJobTrip + " JobID: " + jobId.ToString() +
                                          " JobTripSeqNo: " + jobTripSeqNo.ToString());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        /*2013-10-28 Zhou Kai ends*/

        /*2013-10-08 Zhou Kai adds*/
        internal static bool AddTruckJobTripSubCon(TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            // add a truck job trip sub con according to the truck job trip
            if (ReferenceEquals(truckJobTrip, null)) { throw new FMException(CommonResource.NULLReference); }

            string startTime = " " + truckJobTrip.StartTime.Insert(2, ":");
            string endTime = " " + truckJobTrip.EndTime.Insert(2, ":");
            string pickUptime = DateUtility.ConvertDateForSQLPurpose(truckJobTrip.StartDate) + startTime;
            string deliverTime = DateUtility.ConvertDateForSQLPurpose(truckJobTrip.EndDate) + endTime;
            string strSql = "INSERT INTO TRK_JOB_DETAIL_TRIP_SUBCON_Tbl (" +
                            "JOB_ID, JOBTRIP_SEQ_NO, FR_DATE_ACT, " +
                            "TO_DATE_ACT) VALUES (@JobId, @JobTripSeqNo, @pickUpTime, @deliverTime);";
            SqlCommand cmd = new SqlCommand(strSql, cn);
            cmd.Parameters.AddWithValue("JobId", truckJobTrip.JobID);
            cmd.Parameters.AddWithValue("JobTripSeqNo", truckJobTrip.Sequence);
            cmd.Parameters.AddWithValue("pickUpTime", pickUptime);
            cmd.Parameters.AddWithValue("deliverTime", deliverTime);
            cmd.Transaction = tran;
            try
            {
                // 2014-03-24 Zhou Kai adds checking to the affected row count
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new FMException(TR_LanguageResource.Resources.CommonResource.AffectedRowCountDoesNotMatchExpected); // new user message
                }
                // 2014-03-24 Zhou Kai ends

                // 2014-03-24 Zhou Kai adds logic to get update_version from database when adding the object to database
                truckJobTrip.currentTruckJobTripSubCon.UpdateVersion = GetSubConTripUpdateVersionFromDb(truckJobTrip.JobID, truckJobTrip.Sequence, cn, tran);
                // 2014-03-24 Zhou Kai ends
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }
        /*2013-10-22 Zhou Kai ends*/

        /*2013-10-08 Zhou Kai adds*/
        internal static bool EditTruckJobTripSubConData(int jobId, int jobTripSeqNo,
                                                          string driver, string vehicle,
                                                          string pickUpTime, string deliverTime,
                                                          string waitTimeFrom, string waitTimeTo,
                                                          string operatorTruckCapacity,
                                                          string rm1, string rm2, string rm3,
                                                          string rm4, string rm5)
        {
            bool retValue = false;
            DateTime fr_date_act = Convert.ToDateTime(pickUpTime);
            DateTime to_date_act = Convert.ToDateTime(deliverTime);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            // Get the time stamp
            string strSql = "UPDATE TRK_JOB_DETAIL_TRIP_SUBCON_TBL SET DRIVER = @Driver, VEHICLE = @Vehicle, " +
                            "FR_DATE_ACT = @Fr_date_act, TO_DATE_ACT = @To_date_act, " +
                            "WAIT_TIME_FR_ACT = @Wait_time_fr_act, WAIT_TIME_TO_ACT = @Wait_time_to_act, " +
                            "OPERATORTRUCKCAPACITY = @OperatorTruckCapacity, " +
                            "REMARKS_1 = @Rm1, REMARKS_2 = @Rm2, REMARKS_3 = @Rm3," +
                            "REMARKS_4 = @Rm4, REMARKS_5 = @Rm5 " +
                            "WHERE JOB_ID = @JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            SqlCommand cmd = new SqlCommand(strSql, cn);
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd.Parameters.AddWithValue("Driver", driver);
                cmd.Parameters.AddWithValue("Vehicle", vehicle);
                cmd.Parameters.AddWithValue("Fr_date_act", fr_date_act);
                cmd.Parameters.AddWithValue("To_date_act", to_date_act);
                cmd.Parameters.AddWithValue("Wait_time_fr_act", waitTimeFrom);
                cmd.Parameters.AddWithValue("Wait_time_to_act", waitTimeTo);
                cmd.Parameters.AddWithValue("OperatorTruckCapacity", operatorTruckCapacity);
                cmd.Parameters.AddWithValue("Rm1", rm1);
                cmd.Parameters.AddWithValue("Rm2", rm2);
                cmd.Parameters.AddWithValue("Rm3", rm3);
                cmd.Parameters.AddWithValue("Rm4", rm4);
                cmd.Parameters.AddWithValue("Rm5", rm5);
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return retValue;
        }

        internal static bool DeleteTruckJobTripSubCon(int jobId, int jobTripSeqNo, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            string strSql = "DELETE FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl WHERE JOB_ID = @JobId" +
                            " AND JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            SqlCommand cmd = new SqlCommand(strSql, cn);
            cmd.Parameters.AddWithValue("JobId", jobId);
            cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
            cmd.Transaction = tran;
            try
            {
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;
        }

        #endregion


        //20130930 - gerry added for multi trips
        //
        internal static bool UpdatePartnerLeg(int jobId, int seqNo, int legType, int partnerLeg, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //TODO logic set the partnerLeg type
                string SQLQuery = @"UPDATE TRK_JOB_DETAIL_TRIP_Tbl
                                        SET LEG_TYPE = {2},
                                            PARTNER_LEG = {3}
                                        where JOB_ID ={0} and JOBTRIP_SEQ_NO={1}";
                SQLQuery = string.Format(SQLQuery, jobId, seqNo, legType, partnerLeg);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(SQLQuery, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }
        //20131009 - gerry added to change Job trips status
        //20140314 - gerry remove sql parameter, to allows update for individual job trip status
        //and change parameter to truckJobTrip object
        internal static bool UpdateJobTripStatus(TruckJobTrip jobTrip)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                //check update version before updating
                byte[] origUpdateVersion = GetTruckJobTripUpdateVersion(jobTrip, con, tran);
                if (!SqlBinary.Equals(origUpdateVersion, jobTrip.UpdateVersion))
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict);
                }
                string SQLQuery = @"UPDATE TRK_JOB_DETAIL_TRIP_Tbl
                                        SET STATUS = {2}
                                        where JOB_ID ={0} and JOBTRIP_SEQ_NO={1}";
                SQLQuery = string.Format(SQLQuery, jobTrip.JobID, jobTrip.Sequence, (int)jobTrip.TripStatus);

                SqlCommand cmd = new SqlCommand(SQLQuery, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                tran.Commit(); //20140314 - gerry added to commit every job trip transaction
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (SqlException ex)
            {
                tran.Rollback();
                throw new FMException(ex.Message.ToString());
            }
            catch (InvalidOperationException ex)
            {
                tran.Rollback();
                throw new FMException(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.Message.ToString());
            }

            return true;
        }

        #region "2013-10-16 Zhou Kai adds"
        internal static bool SetJobStatusToComplete(int jobId)
        {
            bool retValue = false;
            string message = String.Empty;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand cmd = new SqlCommand();
            string sqlQuery = "UPDATE TRK_JOB_MAIN_Tbl SET JOB_STATUS = @JobStatus" +
                              " WHERE JOB_ID = @JobId;";
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd = new SqlCommand(sqlQuery, cn);
                cmd.Parameters.AddWithValue("JobStatus", (int)JobStatus.Completed);
                cmd.Parameters.AddWithValue("JobId", jobId);
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new FMException(CommonResource.AffectedRowCountDoesNotMatchExpected);
                }
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return retValue;
        }

        internal static bool SetJobStatusToComplete(int jobId, FMModule fmModule, string formName, DateTime logTime,
            string parentId, string childId, string user, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            string message = String.Empty;
            SqlCommand cmd = new SqlCommand();
            string sqlQuery = "UPDATE TRK_JOB_MAIN_Tbl SET JOB_STATUS = @JobStatus" +
                              " WHERE JOB_ID = @JobId;";
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd = new SqlCommand(sqlQuery, cn);
                cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("JobStatus", (int)JobStatus.Completed);
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }

        internal static bool DoTruckJobTripSubConExist(int jobId)
        {
            bool retValue = false;
            int nCount = 0;
            string sqlQuery = "SELECT COUNT (JOB_ID) FROM TRK_JOB_DETAI_TRIP_SUBCON_Tbl with (NOLOCK)  " +
                              " WHERE JOB_ID = @JobId;";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                cmd.Parameters.AddWithValue("JobId", jobId);
                nCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (nCount > 0) { retValue = true; }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return retValue;
        }

        static internal bool DoTruckJobTripSubConExist(int jobId, int jobTripSeqNo)
        {
            bool retValue = false;
            //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
            ApplicationOption appop =
                ApplicationOption.GetApplicationOption(ApplicationOption.TRUCKING_SETTINGS_ID,
                ApplicationOption.SETTINGS_TRUKC_JOB_BY_SUBCON);
            TruckJobTrip truckJobTrip = TruckJobDAL.GetTruckJobTrip(jobId, jobTripSeqNo);
            bool isSubCon = appop.setting_Value == "T" ? true : false;
            //if (appOption.setting_Value && !truckJobTrip.OwnTransport)
            if (isSubCon && !truckJobTrip.OwnTransport)
            {
                string sqlQuery = "SELECT COUNT (*) FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  " +
                                  "WHERE JOB_ID = @JobId AND " +
                                  "JOBTRIP_SEQ_NO = @JobTripSeqNo;";
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                try
                {
                    if (cn.State != ConnectionState.Open) { cn.Open(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                    cmd.Parameters.AddWithValue("JobId", jobId);
                    cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                    int nSubConTrip = Convert.ToInt32(cmd.ExecuteScalar());
                    if (nSubConTrip > 1) { throw new FMException(TptResourceDAL.MultiJobTripSubCon); }
                    if (nSubConTrip == 1) { retValue = true; } // Only return true in this case
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                catch (Exception ex) { throw new FMException(ex.ToString()); }
                finally { cn.Close(); }
            }

            return retValue;
        }

        static internal bool DoTruckJobTripSubConExist(int jobId, int jobTripSeqNo,
                                                       SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
            ApplicationOption appop =
                ApplicationOption.GetApplicationOption(ApplicationOption.TRUCKING_SETTINGS_ID,
                ApplicationOption.SETTINGS_TRUKC_JOB_BY_SUBCON);
            TruckJobTrip truckJobTrip = TruckJobDAL.GetTruckJobTrip(jobId, jobTripSeqNo);
            bool isSubCon = appop.setting_Value == "T" ? true : false;
            if (isSubCon && !truckJobTrip.OwnTransport)
            {
                string sqlQuery = "SELECT COUNT (*) FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  " +
                                  "WHERE JOB_ID = @JobId AND " +
                                  "JOBTRIP_SEQ_NO = @JobTripSeqNo;";
                try
                {
                    if (cn.State != ConnectionState.Open) { cn.Open(); }
                    SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                    cmd.Parameters.AddWithValue("JobId", jobId);
                    cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                    cmd.Transaction = tran;
                    int nSubConTrip = Convert.ToInt32(cmd.ExecuteScalar());
                    if (nSubConTrip > 1) { throw new FMException(TptResourceDAL.MultiJobTripSubCon); }
                    if (nSubConTrip == 1) { retValue = true; } // Only return true in this case
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                catch (Exception ex) { throw new FMException(ex.ToString()); }
                finally { cn.Close(); }
            }

            return retValue;
        }

        static internal bool IsSubConDataComplete(int jobId, int jobTripSeqNo,
                                                  out string message)
        {
            string sqlQuery = "SELECT * FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  " +
                              "WHERE JOB_ID = @JobId AND " +
                              "JOBTRIP_SEQ_NO = @JobTripSeqNo;";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                SqlDataReader sr = cmd.ExecuteReader();
                message = String.Empty;
                if (sr.Read())
                {
                    string driver = Convert.ToString(sr["DRIVER"]);
                    string vehicle = Convert.ToString(sr["VEHICLE"]);
                    string oprTruckCap = Convert.ToString(sr["OPERATORTRUCKCAPACITY"]);
                    if (driver.Equals(String.Empty)) { message = TptResourceDAL.DriverHasntSet + " "; }
                    if (vehicle.Equals(String.Empty)) { message = message + TptResourceDAL.VehicleHasntSet + " "; }
                    if (oprTruckCap.Equals(String.Empty)) { message = message + TptResourceDAL.OprTruckCapHasntSet; }
                    if (message.Equals(String.Empty)) { return true; }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return false;
        }

        static internal bool IsSubConDataComplete(int jobId, int jobTripSeqNo,
                                                  SqlConnection cn,
                                                  SqlTransaction tran, out string message)
        {
            string sqlQuery = "SELECT * FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  " +
                              "WHERE JOB_ID = @JobId AND JOBTRIP_SEQ_NO = @JobTripSeqNo";
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Parameters.AddWithValue("JobTripSeqNo", jobTripSeqNo);
                cmd.Transaction = tran;
                SqlDataReader sr = cmd.ExecuteReader();
                message = String.Empty;
                if (sr.Read())
                {
                    string driver = Convert.ToString(sr["DRIVER"]);
                    string vehicle = Convert.ToString(sr["VEHICLE"]);
                    string oprTruckCap = Convert.ToString(sr["OPERATORTRUCKCAPACITY"]);
                    if (driver.Equals(String.Empty)) { message = TptResourceDAL.DriverHasntSet + " "; }
                    if (vehicle.Equals(String.Empty)) { message = message + TptResourceDAL.VehicleHasntSet + " "; }
                    if (oprTruckCap.Equals(String.Empty)) { message = message + TptResourceDAL.OprTruckCapHasntSet; }
                    if (message.Equals(String.Empty)) { return true; }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return false;
        }

        static internal bool DeleteAllTruckJobTripsSubCon(int jobId)
        {
            bool retValue = false;
            string sqlQuery = "DELETE FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl WHERE " +
                              "JOB_ID = @JobId;";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand cmd = new SqlCommand(sqlQuery, cn);
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }

        static internal bool DeleteAllTruckJobTripsSubCon(int jobId, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            string sqlQuery = "DELETE FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl WHERE " +
                              "JOB_ID = @JobId;";
            SqlCommand cmd = new SqlCommand(sqlQuery, cn);
            cmd.Transaction = tran;
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }
        // 2013-10-16 Zhou Kai ends

        // 2013-10-23 Zhou Kai adds
        static internal bool SetOWNTRANSPORTByJob(int jobId, char b, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            string sqlQuery = "UPDATE TRK_JOB_DETAIL_TRIP_Tbl SET OWNTRANSPORT = @ot " +
                              "WHERE JOB_ID = @JobId;";
            SqlCommand cmd = new SqlCommand(sqlQuery, cn);
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd.Parameters.AddWithValue("ot", b);
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }

        static internal bool UpdateSubConCodeByJobId(int jobId, string subConCode, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            string sqlQuery = "UPDATE TRK_JOB_DETAIL_TRIP_Tbl SET SUBCONTRACTOR_CODE = @subConCode " +
                              "WHERE JOB_ID = @JobId;";
            SqlCommand cmd = new SqlCommand(sqlQuery, cn);
            try
            {
                // if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd.Parameters.AddWithValue("subConCode", subConCode);
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }

        // Erase the sub-con data, but don't delete the rows
        static internal bool EraseAllTruckJobTripsSubConData(int jobId, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            string sqlQuery = "UPDATE TRK_JOB_DETAIL_TRIP_SUBCON_Tbl SET " +
                              "WAIT_TIME_FR_ACT = @wFrom, WAIT_TIME_TO_ACT = @wTo, " +
                              "DRIVER = @Driver, VEHICLE = @Vehicle, " +
                              "OPERATORTRUCKCAPACITY = @otc, REMARKS_1 = @rm1, " +
                              "REMARKS_2 = @rm2, REMARKS_3 = @rm3, REMARKS_4 = @rm4," +
                              "REMARKS_5 = @rm5 WHERE JOB_ID = @JobId;";
            SqlCommand cmd = new SqlCommand(sqlQuery, cn);
            cmd.Transaction = tran;
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                cmd.Parameters.AddWithValue("JobId", jobId);
                cmd.Parameters.AddWithValue("wFrom", DBNull.Value);
                cmd.Parameters.AddWithValue("wTo", DBNull.Value);
                cmd.Parameters.AddWithValue("Driver", String.Empty);
                cmd.Parameters.AddWithValue("Vehicle", String.Empty);
                cmd.Parameters.AddWithValue("otc", string.Empty);
                cmd.Parameters.AddWithValue("rm1", String.Empty);
                cmd.Parameters.AddWithValue("rm2", String.Empty);
                cmd.Parameters.AddWithValue("rm3", String.Empty);
                cmd.Parameters.AddWithValue("rm4", String.Empty);
                cmd.Parameters.AddWithValue("rm5", String.Empty);
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return retValue;
        }

        static internal bool AreAllSubConDataComplete(int jobId, out string message)
        {
            bool retValue = true;
            message = String.Empty;
            string sqlQuery = "SELECT * FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  " +
                              "WHERE JOB_ID = @JobId";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand cmd = new SqlCommand(sqlQuery, cn);
            cmd.Parameters.AddWithValue("JobId", jobId);
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read()) { if (!IsSubConDataComplete(sr)) { message = message + sr["JOBTRIP_SEQ_NO"].ToString() + " "; } }
                if (!message.Equals(String.Empty))
                {
                    message = TptResourceUI.SubConDataInComplete + "\nTruck JobID: " +
                              jobId + ". Trip SeqNo: " + message;
                    return false;
                }
            }
            catch (FMException fmEx) { retValue = false; throw fmEx; }
            catch (SqlException ex) { retValue = false; throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { retValue = false; throw new FMException(ex.ToString()); }
            catch (Exception ex) { retValue = false; throw new FMException(ex.ToString()); }
            finally { cn.Close(); }

            return retValue;
        }

        private static bool IsSubConDataComplete(IDataReader reader)
        {
            try
            {
                if (reader["DRIVER"].ToString().Equals(String.Empty))
                { return false; }
                if (reader["VEHICLE"].ToString().Equals(String.Empty))
                { return false; }
                if (reader["OPERATORTRUCKCAPACITY"].ToString().Equals(String.Empty))
                { return false; }
            }
            catch (IndexOutOfRangeException ie) { return false; }
            return true;
        }

        internal static bool EditTruckJobTripSubConRelatedData(TruckJobTrip truckJobTrip, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            int jobId = truckJobTrip.JobID;
            int jobTripSeqNo = truckJobTrip.Sequence;
            byte[] dbUpdateVersion = new byte[8];
            dbUpdateVersion = GetSubConTripUpdateVersionFromDb(jobId, jobTripSeqNo);
            if (!SqlBinary.Equals(dbUpdateVersion, truckJobTrip.currentTruckJobTripSubCon.UpdateVersion))
            {
                throw new FMException(TptResourceDAL.ErrMultiUserConflict);
            }
            DateTime dateTimeFromAct = GetDateTime(truckJobTrip.StartDate, truckJobTrip.StartTime);
            DateTime dateTimeToAct = GetDateTime(truckJobTrip.EndDate, truckJobTrip.EndTime);
            string strSql = "UPDATE TRK_JOB_DETAIL_TRIP_SUBCON_Tbl SET " +
                            "FR_DATE_ACT = @fr_date_act, TO_DATE_ACT = @to_date_act " +
                            "WHERE JOB_ID = @job_id AND JOBTRIP_SEQ_NO = @jobtrip_seq_no;";
            SqlCommand cmd = new SqlCommand(strSql, cn);
            try
            {
                cmd.Transaction = tran;
                cmd.Parameters.AddWithValue("fr_date_act", dateTimeFromAct);
                cmd.Parameters.AddWithValue("to_date_act", dateTimeToAct);
                cmd.Parameters.AddWithValue("job_id", truckJobTrip.JobID);
                cmd.Parameters.AddWithValue("jobtrip_seq_no", truckJobTrip.Sequence);
                if (cmd.ExecuteNonQuery() != 1)
                {
                    throw new FMException(TR_LanguageResource.Resources.CommonResource.AffectedRowCountDoesNotMatchExpected);
                }
                // 2014-03-25 Zhou Kai adds, update the time stamp
                truckJobTrip.currentTruckJobTripSubCon.UpdateVersion = GetSubConTripUpdateVersionFromDb(jobId, jobTripSeqNo, cn, tran);
                // 2014-03-25 Zhou Kai ends
            }
            catch (FMException fmEx) { retValue = false; throw fmEx; }
            catch (SqlException ex) { retValue = false; throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { retValue = false; throw new FMException(ex.ToString()); }
            catch (Exception ex) { retValue = false; throw new FMException(ex.ToString()); }
            return retValue;
        }

        private static string GetDateTimeToString(DateTime datePart, string timePart)
        {
            return DateUtility.ConvertDateForSQLPurpose(datePart) + " " + timePart.Insert(2, ":");
        }
        private static DateTime GetDateTime(DateTime datePart, string timePart)
        {
            return Convert.ToDateTime(DateUtility.ConvertDateForSQLPurpose(datePart)
                                      + " " + timePart.Insert(2, ":"));
        }
        // 2013-10-24 Zhou Kai ends

        // 2013-11-29 Zhou Kai adds to get and sort the job trip status history
        public static List<JobTripState> GetTruckJobTripStatusHistory(int jobId, int jobTripSeqNo)
        {
            List<JobTripState> lstTruckJobTripStatusHistory = new List<JobTripState>();
            SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string strQuery = "SELECT * FROM TRK_JOB_TRIP_STATE_Tbl with (NOLOCK)  WHERE JOB_ID = @jobId " +
                "AND SEQUENCE_NO = @jobtrip_seqNo ORDER BY JOB_STATE_NO";
            try
            {
                if (dbCon.State != ConnectionState.Open) { dbCon.Open(); }
                SqlCommand dbCmd = new SqlCommand(strQuery, dbCon);
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

                    lstTruckJobTripStatusHistory.Add(tjtsr);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { dbCon.Close(); }
            return lstTruckJobTripStatusHistory;
        }

        // 2013-12-02 Zhou Kai ends
        #endregion

        #region "2013-12-18 Zou Kai adds function to get truck job trip details from job_no and job_trip_seqNo"
        public SortableList<TruckJobTripDetail> GetTruckJobTripDetails(string jobNo, int jobTripSeqNo)
        {
            //Following code is an example based on a proposed table, TPT_XXX_Tbl
            SortableList<TruckJobTripDetail> Operators = new SortableList<TruckJobTripDetail>();
            //gerry added try catch
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_DETAIL_Tbl with (NOLOCK)  " +
                    "INNER JOIN TRK_JOB_MAIN_Tbl with (NOLOCK)  ON TRK_JOB_DETAIL_TRIP_DETAIL_Tbl.JOB_ID =TRK_JOB_MAIN_Tbl.JOB_ID ";
                SQLString += "AND JOB_NUMBER = " + jobNo;
                SQLString += " AND JOBTRIP_SEQ_NO = " + jobTripSeqNo;
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJobTripDetail(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return Operators;
        }

        public static SortableList<TruckJobTripDetail> GetTruckJobTripDetailsStatic(string jobNo, int jobTripSeqNo)
        {
            SortableList<TruckJobTripDetail> Operators = new SortableList<TruckJobTripDetail>();
            //gerry added try catch
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TRK_JOB_DETAIL_TRIP_DETAIL_Tbl with (NOLOCK)  " +
                    "INNER JOIN TRK_JOB_MAIN_Tbl with (NOLOCK)  ON TRK_JOB_MAIN_Tbl.JOB_ID = " +
                    "TRK_JOB_DETAIL_TRIP_DETAIL_Tbl.JOB_ID ";
                SQLString += "AND JOB_NUMBER = '" + jobNo;
                SQLString += "' AND JOBTRIP_SEQ_NO = " + jobTripSeqNo;
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Operators.Add(GetTruckJobTripDetail(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return Operators;
        }

        // 2013-12-24 Zhou Kai adds
        public static List<string> GetAllTruckJobChargeSeqNos(string jobNumber)
        {
            List<string> lstTruckJobChargeSeqNo = new List<string>();
            string sqlQuery = "SELECT SEQUENCE_NO FROM dbo.TRK_JOB_DETAIL_CHARGE_Tbl with (NOLOCK)  " +
                    "INNER JOIN dbo.TRK_JOB_MAIN_Tbl with (NOLOCK)  ON dbo.TRK_JOB_DETAIL_CHARGE_Tbl.JOB_ID = " +
                    "dbo.TRK_JOB_MAIN_Tbl.JOB_ID WHERE JOB_NUMBER = '{0}' AND JOB_CHARGE_TYPE = 'S'";
            sqlQuery = String.Format(sqlQuery, jobNumber);
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lstTruckJobChargeSeqNo.Add(reader["SEQUENCE_NO"].ToString());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { con.Close(); }

            return lstTruckJobChargeSeqNo;
        }
        // 2013-12-24 Zhou Kai ends

        #endregion

        // 2014-01-30 Zhou Kai adds
        public static List<string> GetAllDeletedTruckJobNumbers(Dictionary<string, string> dictCriteria)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = @"SELECT DISTINCT JobNo, FORM_NAME FROM 

                                           (SELECT PARENT_IDENTIFIER As JobNo, FORM_NAME FROM TPT_LOG_HEADER_Tbl with (NOLOCK)  WHERE 
                                           MODULE = 7 AND CHILD_IDENTIFIER = '')  AS JobNo

                                          WHERE  (JobNo NOT IN (SELECT  Job_Number FROM TRK_JOB_MAIN_Tbl)" +
                                           LoggerDAL.FormatFormNamesForSql(dictCriteria) + ")";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tmpList.Add(r["JobNo"].ToString());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        public static List<string> GetAllDeletedTruckJobTripIDs(Dictionary<string, string> dict)
        {
            string jobNo = dict[LogHeader.TOP_LEVEL_NO];
            List<string> tmpList = new List<string>();
            List<string> tmpSeqList = new List<string>();
            string tripSeqNos = String.Empty;
            tmpSeqList = TruckJobTrip.GetAllTruckJobTripSeqNoByJobNumber(jobNo);
            foreach (string seqNo in tmpSeqList)
            {
                tripSeqNos = tripSeqNos + ",'" + seqNo + "'";
            }
            // 2014-02-10 Zhou Kai adds
            if (tripSeqNos.Equals(String.Empty)) // no deleted job trips 
            {
                tripSeqNos = "('')";
            }
            else
            {
                tripSeqNos = "(" + tripSeqNos.Substring(1, tripSeqNos.Length - 1) + ")";
            }
            string sqlQuery = "SELECT CHILD_IDENTIFIER, FORM_NAME FROM TPT_LOG_HEADER_Tbl " +
                                        "WHERE PARENT_IDENTIFIER = '{0}' AND MODULE = 7 " +
                                        "AND CHILD_IDENTIFIER <> '' AND FORM_ACTION = 3 AND " +
                                        "CHILD_IDENTIFIER NOT IN " + tripSeqNos +
                                        " AND PARENT_IDENTIFIER IN (SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK) )" +
                                         LogHeader.FormatFormNames(dict);
            sqlQuery = String.Format(sqlQuery, dict[LogHeader.TOP_LEVEL_NO]);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tmpList.Add(r["CHILD_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        public static List<string> GetAllDeletedTruckJobChargeSeqNos(Dictionary<string, string> dict)
        {
            string jobNo = dict[LogHeader.TOP_LEVEL_NO];
            List<string> tmpList = new List<string>();
            List<string> existingSeqList = new List<string>();
            string existingSeqs = String.Empty;
            existingSeqList = TruckJobCharge.GetAllTruckJobChargeSeqNos(jobNo);
            foreach (string temp in existingSeqList)
            {
                existingSeqs = existingSeqs + ",'" + temp + "'";
            }
            if (existingSeqs.Equals(String.Empty)) // no deleted job trips 
            {
                existingSeqs = "('')";
            }
            else
            {
                existingSeqs = "(" + existingSeqs.Substring(1, existingSeqs.Length - 1) + ")";
            }
            string sqlQuery = "SELECT DISTINCT CHILD_IDENTIFIER FROM TPT_LOG_HEADER_Tbl " +
                                        "WHERE PARENT_IDENTIFIER = '{0}' AND FORM_ACTION = 3 " +
                                         " AND CHILD_IDENTIFIER NOT IN " + existingSeqs +
                                          LoggerDAL.FormatFormNamesForSql(dict);
            sqlQuery = String.Format(sqlQuery, jobNo);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tmpList.Add(r["CHILD_IDENTIFIER"].ToString());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }
            return tmpList;
        }

        public static List<string> GetAllTruckJobNoWhichHasDeletedJobTrips(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = "SELECT DISTINCT PARENT_IDENTIFIER FROM TPT_LOG_HEADER_Tbl " +
                                       "WHERE FORM_ACTION = 3 AND " + // be a deletion
                                       "CHILD_IDENTIFIER <> '' AND CHARINDEX('&', PARENT_IDENTIFIER) = 0 " + // it's job trip been deleted
                                        "AND MODULE = 7 " + LogHeader.FormatFormNames(dict) + // truck job trip related form names
                                        " AND PARENT_IDENTIFIER IN (SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK) ) " + // job is still there
                                        "AND CHILD_IDENTIFIER NOT IN (SELECT DISTINCT JOBTRIP_SEQ_NO FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK) " +
                                        " INNER JOIN TRK_JOB_MAIN_Tbl with (NOLOCK) " +
                                        " ON PARENT_IDENTIFIER = TRK_JOB_MAIN_Tbl.JOB_NUMBER" +
                                         " AND TRK_JOB_MAIN_Tbl.JOB_ID = TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID)"; // job trip is not there
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    tmpList.Add(sr["PARENT_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        public static List<string> GetAllTruckJobNoWhichHasDeletedJobCharges(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = "SELECT DISTINCT PARENT_IDENTIFIER FROM TPT_LOG_HEADER_Tbl " +
                                       "WHERE FORM_ACTION = 3 AND " +
                                       "CHILD_IDENTIFIER <> '' AND CHARINDEX('&', PARENT_IDENTIFIER) = 0 " +
                                        "AND MODULE = 7 AND PARENT_IDENTIFIER IN (" +
                                        "SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK) )" + LogHeader.FormatFormNames(dict);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    tmpList.Add(sr["PARENT_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        //20140304 - gerry added for Subcon changes
        //20140324 - gerry modified query to use select count
        internal static bool AreThereAnyTruckJobTripPlan(TruckJobTrip jobtrip)
        {
            bool retValue = false;
            try
            {
                string query = @"select Count(*) from TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK) 
                                    where JOB_ID = {0} and JOBTRIP_SEQ_NO ={1}";
                query = string.Format(query, jobtrip.JobID, jobtrip.Sequence);
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(query, cn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = reader.GetInt32(0) > 0 ? true : false;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        //20140314 - gerry add for subcon
        internal static bool AddorChangeSubContractorFromPlanning(TruckJobTrip truckJobTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                byte[] origUpdateVersion = GetTruckJobTripUpdateVersion(truckJobTrip, con, tran);
                if (!SqlBinary.Equals(truckJobTrip.UpdateVersion, origUpdateVersion))
                {
                    throw new FMException(TptResourceDAL.ErrMultiUserConflict);
                }
                //20140317 - gerry modified the query to change owntansport field
                string query = @"UPDATE TRK_JOB_DETAIL_TRIP_Tbl
                                set SUBCONTRACTOR_CODE = '{0}',
                                    SUBCONTRACTOR_NAME ='{1}',
                                    SUBCONTRACTOR_ADD1 ='{2}',
                                    SUBCONTRACTOR_ADD2 ='{3}',
                                    SUBCONTRACTOR_ADD3 ='{4}',
                                    SUBCONTRACTOR_ADD4 ='{5}',
                                    SUBCONTRACTOR_CITY_CODE ='{6}',
                                    Status =  {7}, /*set to not only Assigned but can be set to booked or ready*/
                                    OWNTRANSPORT = '{8}' /* set own transport value */

                                where job_id = {9} and jobtrip_seq_no ={10}
                                ";
                //201040502 - gerry change subCon from CustomerDTO to operatorDTO
                query = string.Format(query, CommonUtilities.FormatString(truckJobTrip.subCon.Code.ToString().Trim()),
                                             CommonUtilities.FormatString(truckJobTrip.subCon.Name.ToString().Trim()),
                                             CommonUtilities.FormatString(truckJobTrip.subCon.Add1.ToString().Trim()),
                                             CommonUtilities.FormatString(truckJobTrip.subCon.Add2.ToString().Trim()),
                                             CommonUtilities.FormatString(truckJobTrip.subCon.Add3.ToString().Trim()),
                                             CommonUtilities.FormatString(truckJobTrip.subCon.Add4.ToString().Trim()),
                                             CommonUtilities.FormatString(truckJobTrip.subCon.City.ToString().Trim()),
                                             truckJobTrip.TripStatus.GetHashCode(),   //20140324 - gerry added
                                             truckJobTrip.subCon.Code.Trim() == string.Empty ? "T" : "F", /*20140318 - gerry added*/
                                             truckJobTrip.JobID,/*20140318 - gerry added*/
                                             truckJobTrip.Sequence/*20140318 - gerry added*/);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }

        // 2014-03-17 Zhou Kai adds
        /*
         *Get deleted truck job trip detail number by truck job number and truck job trip sequence no.
        */
        internal static List<string> GetAllDeletedDetailSeqNoByJobNoAndTripSeqNo(Dictionary<string, string> dict)
        {
            string jobNumber = dict[LogHeader.TOP_LEVEL_NO];
            string jobTripSeqNo = dict[LogHeader.SEQ_NO];
            List<string> tmpList = new List<string>();
            SortableList<string> tmpDetailSeqNoList = new SortableList<string>();
            string detailSeqNos = String.Empty;
            tmpDetailSeqNoList = TruckJobTrip.GetTruckJobTripDetailSeqNo(jobNumber, jobTripSeqNo);
            foreach (string seqNo in tmpDetailSeqNoList)
            {
                detailSeqNos = detailSeqNos + ",'" + seqNo + "'";
            }
            if (detailSeqNos.Equals(String.Empty)) // no deleted job trips 
            {
                detailSeqNos = "('')";
            }
            else
            {
                detailSeqNos = "(" + detailSeqNos.Substring(1, detailSeqNos.Length - 1) + ")";
            }


            string sqlQuery = "SELECT CHILD_IDENTIFIER, FORM_NAME FROM TPT_LOG_HEADER_Tbl " +
                                        "WHERE PARENT_IDENTIFIER = '{0}&{1}' AND MODULE = 7 " +
                                        "AND CHILD_IDENTIFIER <> '' AND FORM_ACTION = 3 AND " +
                                        "CHILD_IDENTIFIER NOT IN " + detailSeqNos + LogHeader.FormatFormNames(dict);
            sqlQuery = String.Format(sqlQuery, dict[LogHeader.TOP_LEVEL_NO], dict[LogHeader.SEQ_NO]);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    tmpList.Add(r["CHILD_IDENTIFIER"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        internal static List<string> GetAllJobNoWhichHasDeletedDetails(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = "SELECT DISTINCT PARENT_IDENTIFIER FROM TPT_LOG_HEADER_Tbl " +
                                       "WHERE FORM_ACTION = 3 AND " +
                                       "CHILD_IDENTIFIER <> '' AND CHARINDEX('&', PARENT_IDENTIFIER) <> 0 " +
                                        "AND MODULE = 7 " + LogHeader.FormatFormNames(dict) +
                                        " AND SUBSTRING(PARENT_IDENTIFIER, 0, CHARINDEX('&', PARENT_IDENTIFIER))" +
                                        " IN (SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK) )";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    tmpList.Add(sr["PARENT_IDENTIFIER"].ToString().Trim());
                }

                tmpList = GetTopLevelNoFromParentIdentifier(tmpList);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        internal static List<string> GetAllTripSeqNosWhichHasDeletedDetails(Dictionary<string, string> dict)
        {
            List<string> tmpList = new List<string>();
            string sqlQuery = "SELECT DISTINCT PARENT_IDENTIFIER FROM TPT_LOG_HEADER_Tbl " +
                                       "WHERE FORM_ACTION = 3 AND " +
                                       "CHILD_IDENTIFIER <> '' AND CHARINDEX('&', PARENT_IDENTIFIER) <> 0 " +
                                        "AND MODULE = 7 " + LogHeader.FormatFormNames(dict) +
                                        " AND SUBSTRING(PARENT_IDENTIFIER, 0, CHARINDEX('&', PARENT_IDENTIFIER)) IN " +
                                        " (SELECT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK) )" +
                                        " AND CHILD_IDENTIFIER IN (SELECT SEQUENCE_NO FROM TRK_JOB_DETAIL_Tbl with (NOLOCK) )";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                SqlDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    tmpList.Add(sr["PARENT_IDENTIFIER"].ToString().Trim());
                }

                tmpList = GetSeqlNoFromParentIdentifier(dict[LogHeader.TOP_LEVEL_NO], tmpList);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { cn.Close(); }

            return tmpList;
        }

        /*
         * The parent_identifier is like: TRK20140314&1, the topLevelNo is: TRK20140314, 
         * and the seqNo is: 1.
         * The function is to get all seqNos which has the given topLevel from a list of 
         * parent identifiers: lstParentIds
         */
        private static List<string> GetSeqlNoFromParentIdentifier(string topLevelNo, List<string> lstParentIds)
        {
            List<string> tmpList = new List<string>();
            foreach (string parentId in lstParentIds)
            {
                if (parentId.Contains(topLevelNo)) // if the parent id has the same topLevelNo with the given topLevelNo
                {
                    tmpList.Add(parentId.Split('&')[1]); // get the seqNo from the parentId
                }
            }

            return tmpList;
        }

        private static List<string> GetTopLevelNoFromParentIdentifier(List<string> lstParentIds)
        {
            List<string> tmpList = new List<string>();
            foreach (string parentId in lstParentIds)
            {
                tmpList.Add(parentId.Split('&')[0]); // get the topLevelNo from the parentId
            }

            return tmpList;
        }
        // 2014-03-17 Zhou Kai ends

        //20140319 - gerry added to get jobNos for subcontractor jobs
        internal static ArrayList GetSubContractedTruckTripJobNo(int status)
        {
            ArrayList list = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT DISTINCT JOB_NUMBER FROM TRK_JOB_MAIN_Tbl with (NOLOCK) 
                                        INNER JOIN TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK) 
                                        ON TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID=TRK_JOB_MAIN_Tbl.JOB_ID
                                        WHERE 1=1
                                        AND TRK_JOB_DETAIL_TRIP_Tbl.STATUS ={0}
                                        AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT ='F'";
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
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return list;
        }
        //20140319 - gerry added to get jobNos for subcontractor jobs
        internal static ArrayList GetSubContractedTruckTripSeqNo(int jobId, int status)
        {
            ArrayList list = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT DISTINCT JOBTRIP_SEQ_NO FROM TRK_JOB_MAIN_Tbl with (NOLOCK) 
                                        INNER JOIN TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK) 
                                        ON TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID=TRK_JOB_MAIN_Tbl.JOB_ID 
                                        WHERE 1=1
                                        AND JOB_ID = {0}
                                        AND TRK_JOB_DETAIL_TRIP_Tbl.STATUS ={1}
                                        AND TRK_JOB_DETAIL_TRIP_Tbl.OWNTRANSPORT ='F'";
                SQLString = string.Format(SQLString, jobId, status);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((Int32)reader["JOBTRIP_SEQ_NO"]);
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return list;
        }

        internal static bool DeleteTruckJobTripBySubCon(int jobId, int jobTripSeqNo, SqlConnection con, SqlTransaction tran)
        {
            string sqlQuery = "DELETE FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl WHERE " +
                "JOB_ID = {0} AND [JOBTRIP_SEQ_NO] = {1}";
            sqlQuery = string.Format(sqlQuery, jobId, jobTripSeqNo);

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.Transaction = tran;
                if (cmd.ExecuteNonQuery() != 1) // 2014-03-25 Zhou Kai adds comments, to check affected row count
                {
                    throw new FMException(TR_LanguageResource.Resources.CommonResource.AffectedRowCountDoesNotMatchExpected);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return true;
        }

        // 2014-03-23 Zhou Kai adds
        public static byte[] GetSubConTripUpdateVersionFromDb(int jobId, int jobTripSeqNo)
        {
            byte[] retUpdateVersion = new byte[8];
            string sqlQuery = "SELECT UPDATE_VERSION FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  " +
                "WHERE JOB_ID = {0} AND [JOBTRIP_SEQ_NO] = {1}";
            sqlQuery = String.Format(sqlQuery, jobId, jobTripSeqNo);
            SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());

            try
            {
                if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                SqlCommand dbCmd = new SqlCommand(sqlQuery, dbCon);
                retUpdateVersion = (byte[])dbCmd.ExecuteScalar();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { dbCon.Close(); }

            return retUpdateVersion;
        }

        public static byte[] GetSubConTripUpdateVersionFromDb(int jobId, int jobTripSeqNo, SqlConnection dbCon, SqlTransaction tran)
        {
            byte[] retUpdateVersion = new byte[8];
            string sqlQuery = "SELECT UPDATE_VERSION FROM TRK_JOB_DETAIL_TRIP_SUBCON_Tbl with (NOLOCK)  " +
                "WHERE JOB_ID = {0} AND [JOBTRIP_SEQ_NO] = {1}";
            sqlQuery = String.Format(sqlQuery, jobId, jobTripSeqNo);

            try
            {
                SqlCommand dbCmd = new SqlCommand(sqlQuery, dbCon);
                dbCmd.Transaction = tran;
                retUpdateVersion = (byte[])dbCmd.ExecuteScalar();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return retUpdateVersion;
        }

        // 2014-03-23 Zhou Kai ends

        //20151222 - gerry start
        internal static Decimal GetUnAssignedTruckJobTripWeight(TruckJobTrip jobTrip)
        {
            decimal retValue = 0;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"select ISNULL(SUM(weight), 0) as AssignedWeight from TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK) 
                                    where JOB_ID = {0} 
                                        and JOBTRIP_SEQ_NO = {1}";

                query = string.Format(query, jobTrip.JobID, jobTrip.Sequence);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                IDataReader reader = cmd.ExecuteReader();
                byte[] origVersion = null;
                while (reader.Read())
                {
                    decimal AssignedWeight = reader.GetDecimal(0);
                    retValue = jobTrip.BookedWeight - AssignedWeight;
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static Decimal GetUnAssignedTruckJobTripWeight(TruckJobTrip jobTrip, SqlConnection con, SqlTransaction tran)
        {
            decimal retValue = 0;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"select ISNULL(SUM(weight), 0) as AssignedWeight from TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK) 
                                    where JOB_ID = {0} 
                                        and JOBTRIP_SEQ_NO = {1}";

                query = string.Format(query, jobTrip.JobID, jobTrip.Sequence);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                byte[] origVersion = null;
                while (reader.Read())
                {
                    decimal AssignedWeight = reader.GetDecimal(0);
                    retValue = jobTrip.BookedWeight - AssignedWeight;
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        internal static Decimal GetUnassignedTruckJobTripVolume(TruckJobTrip jobTrip)
        {
            decimal retValue = 0;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"select ISNULL(SUM(VOLUME), 0) as AssignedVolume from TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK) 
                                    where JOB_ID = {0} 
                                        and JOBTRIP_SEQ_NO = {1}";

                query = string.Format(query, jobTrip.JobID, jobTrip.Sequence);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                IDataReader reader = cmd.ExecuteReader();
                byte[] origVersion = null;
                while (reader.Read())
                {
                    decimal AssignedVolume = reader.GetDecimal(0);
                    retValue = jobTrip.BookedVol - AssignedVolume;
                }
                reader.Close();
            }
            catch (FMException ex) { throw; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            return retValue;
        }
        internal static Decimal GetUnassignedTruckJobTripVolume(TruckJobTrip jobTrip, SqlConnection con, SqlTransaction tran)
        {
            decimal retValue = 0;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                string query = @"select ISNULL(SUM(VOLUME), 0) as AssignedVolume from TRK_JOB_DETAIL_TRIP_PLAN_TBL with (NOLOCK) 
                                    where JOB_ID = {0} 
                                        and JOBTRIP_SEQ_NO = {1}";

                query = string.Format(query, jobTrip.JobID, jobTrip.Sequence);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                byte[] origVersion = null;
                while (reader.Read())
                {
                    decimal AssignedVolume = reader.GetDecimal(0);
                    retValue = jobTrip.BookedVol - AssignedVolume;
                }
                reader.Close();
            }
            catch (FMException ex) { throw; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        //20160224 - gerry added
        internal static JobTripStatus GetTruckJobTripStatus(int jobID, int SeqNo, SqlConnection con, SqlTransaction tran)
        {
            JobTripStatus retValue = JobTripStatus.Booked;
            //gerry added try catch
            try
            {
                string SQLString = "SELECT STATUS FROM TRK_JOB_DETAIL_TRIP_Tbl with (NOLOCK)  ";
                SQLString += "WHERE JOB_ID = " + jobID;
                SQLString += " AND JOBTRIP_SEQ_NO = " + SeqNo;

                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int status = reader["STATUS"] == DBNull.Value ? 1 : (int)reader["STATUS"];
                    switch (status)
                    {
                        case 1:
                            retValue = JobTripStatus.Booked;
                            break;
                        case 2:
                            retValue = JobTripStatus.Ready;
                            break;
                        case 3:
                            retValue = JobTripStatus.Assigned;
                            break;
                        case 4:
                            retValue = JobTripStatus.Completed;
                            break;
                        case 5:
                            retValue = JobTripStatus.Invoiced;
                            break;
                    }
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }

        internal static int GetAvailableBalQtyForCargoDetail(TruckJobTripDetail truckJobTripDetail)
        {
            int retValue = 0;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string query = @"select (a.QTY - ISNULL((select SUM(b.QTY) from TRK_PLAN_SUB_TRIP_JOB_DETAIL_TBL b with (NOLOCK) 
				                                        where b.JOB_ID ={0}
				                                        and b.JOBTRIP_SEQ_NO ={1}
				                                        and b.DETAIL_SEQNO = {2}),0)) as balQty  
                                        from TRK_JOB_DETAIL_TRIP_DETAIL_Tbl  a with (NOLOCK) 
                                        where a.JOB_ID ={0}
                                        and a.JOBTRIP_SEQ_NO ={1}
                                        and a.SEQ_NO = {2}";

                    query = string.Format(query, truckJobTripDetail.jobID, truckJobTripDetail.jobTripSequence, truckJobTripDetail.detailSequence);
                    SqlCommand cmd = new SqlCommand(query, con);
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        retValue = (Int32)reader["balQty"];
                    }
                    con.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
                finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            }
            return retValue;
        }
        internal static int GetAvailableBalQtyForCargoDetail(TruckJobTripDetail truckJobTripDetail, SqlConnection con, SqlTransaction tran)
        {
            int retValue = 0;
            try
            {
                string query = @"select (a.QTY - ISNULL((select SUM(b.QTY) from TRK_PLAN_SUB_TRIP_JOB_DETAIL_TBL b with (NOLOCK) 
				                                        where b.JOB_ID ={0}
				                                        and b.JOBTRIP_SEQ_NO ={1}
				                                        and b.DETAIL_SEQNO = {2}),0)) as balQty  
                                        from TRK_JOB_DETAIL_TRIP_DETAIL_Tbl  a with (NOLOCK) 
                                        where a.JOB_ID ={0}
                                        and a.JOBTRIP_SEQ_NO ={1}
                                        and a.SEQ_NO = {2}";

                query = string.Format(query, truckJobTripDetail.jobID, truckJobTripDetail.jobTripSequence, truckJobTripDetail.detailSequence);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                if (con.State == ConnectionState.Closed) { con.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = (Int32)reader["balQty"];
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        //20160902 - gerry added to updated wms temp table
        internal static int UpdateTRKJOB_FROM_WMS_Tbl(string ginNo, bool is_Confirmed, SqlConnection con, SqlTransaction tran)
        {
            int retValue = 0;
            try
            {
                string query = @"update TRK_JOB_FROM_WMS_Tbl
		                            set IS_CONFIRMED = '{0}'
		                            where GIN_NO = '{1}'";

                query = string.Format(query, is_Confirmed ? "T" : "F", ginNo);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }


    }
}
