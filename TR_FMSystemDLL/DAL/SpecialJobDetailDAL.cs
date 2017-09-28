using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class SpecialJobDetailDAL
    {
        const string ConstSystemJobRelation = "TR";

        internal static SpecialJobDetail GetSpecialJobDetail(string jobNumber, string branchCode)
        {
            try
            {
                SpecialJobDetail specialJobDetail = new SpecialJobDetail(); 
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string sqlGetJobTypeCode = " Select JobDetail.*,JobMaster.* From AM_Special_Job_Detail_Tbl JobDetail,AM_Job_Master_Tbl JobMaster " +
                                           " where JobDetail.Job_Number = '" + jobNumber.Trim() + "'" +
                                           " and JobDetail.Branch_Code = '" + branchCode.Trim() + "'" +
                                           " and JobDetail.Job_Number =JobMaster.Job_Number ";
                SqlDataAdapter adapter = new SqlDataAdapter(sqlGetJobTypeCode, cn);
                cn.Open();
                DataSet dsGetJobDetail = new DataSet();
                adapter.Fill(dsGetJobDetail);
                cn.Close(); 
                foreach(DataRow drTemp in dsGetJobDetail.Tables[0].Rows) 
                {

                    specialJobDetail.jobNo =drTemp["Job_Number"].ToString();
                    specialJobDetail.jobTypeCode =drTemp["Job_Type_Code"].ToString(); 
                    specialJobDetail.branchCode =drTemp["Branch_Code"].ToString(); 
                    specialJobDetail.shipperCode=drTemp["Shipper_Code"].ToString(); 
                    specialJobDetail.shipperName=drTemp["Shipper_Name"].ToString();
                    specialJobDetail.consigneeCode=drTemp["Consignee_Code"].ToString(); 
                    specialJobDetail.consigneeName =drTemp["Consignee_Name"].ToString(); 
                    specialJobDetail.overseaAgentName =drTemp["Oversea_Agent_Name"].ToString(); 
                    specialJobDetail.overseaAgentCode =drTemp["Oversea_Agent_Code"].ToString();
                    specialJobDetail.routingAgentCode =drTemp["Routing_Agent_Code"].ToString();
                    specialJobDetail.routingAgentName =drTemp["Routing_Agent_Name"].ToString(); 
                    specialJobDetail.origin =drTemp["Origin"].ToString();
                    specialJobDetail.destination =drTemp["Destination"].ToString(); 
                    specialJobDetail.viaCity =drTemp["Via_City"].ToString(); 
                    specialJobDetail.portOfDischarge =drTemp["Port_of_Discharge"].ToString(); 
                    specialJobDetail.portOfLoading =drTemp["Port_of_Loading"].ToString();
                    if (!Convert.IsDBNull(drTemp["Job_Month"]))
                    {
                        specialJobDetail.jobMonth = Convert.ToInt32(drTemp["Job_Month"]);
                    }
                    if (!Convert.IsDBNull(drTemp["Job_Year"]))
                    {
                        specialJobDetail.jobMonth = Convert.ToInt32(drTemp["Job_Year"]);
                    }
                    if (!Convert.IsDBNull(drTemp["Change_Date_Time"]))
                    {
                        specialJobDetail.changeDate = Convert.ToDateTime(drTemp["Change_Date_Time"]);
                    }
                    if (!Convert.IsDBNull(drTemp["Flight_Date"]))
                    {
                    specialJobDetail.flightDate =Convert.ToDateTime(drTemp["Flight_Date"]);
                    }
                     if (!Convert.IsDBNull(drTemp["Arrival_Date"]))
                    {
                    specialJobDetail.arrivalDate =Convert.ToDateTime(drTemp["Arrival_Date"]);
                    }
                    if (!Convert.IsDBNull(drTemp["PCS"]))
                    {
                        specialJobDetail.PCS =Convert.ToInt16(drTemp["PCS"]) ;
                    }
                    if (!Convert.IsDBNull(drTemp["Gross_Weight"]))
                    {
                        specialJobDetail.grossWeight =Convert.ToDecimal(drTemp["Gross_Weight"]); 
                    }
                    if (!Convert.IsDBNull(drTemp["Charge_Weight"]))
                    {
                        specialJobDetail.chargeWeight =Convert.ToDecimal(drTemp["Charge_Weight"]);
                    }
                    if (!Convert.IsDBNull(drTemp["CBM"]))
                    {
                        specialJobDetail.cbm =Convert.ToDecimal(drTemp["CBM"]); 
                    }
                    if (!Convert.IsDBNull(drTemp["Container_20"]))
                    {
                        specialJobDetail.container20 =Convert.ToDecimal(drTemp["Container_20"]); 
                    }
                    if (!Convert.IsDBNull(drTemp["Container_40"]))
                    {
                        specialJobDetail.container40 =Convert.ToDecimal(drTemp["Container_40"]);
                    }
                    if (!Convert.IsDBNull(drTemp["Container_45"]))
                    {
                        specialJobDetail.container45 =Convert.ToDecimal(drTemp["Container_45"]);
                    }
                    if(!Convert.IsDBNull(drTemp["Airline"]))
                    {
                        specialJobDetail.airLine =drTemp["Airline"].ToString(); 
                    }
                    if(!Convert.IsDBNull(drTemp["Coloader"]))
                    {
                        specialJobDetail.coloader =drTemp["Coloader"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Remarks1"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Remarks1"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Remarks2"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Remarks2"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Remarks3"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Remarks3"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Remarks4"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Remarks4"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Remarks5"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Remarks5"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Content1"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Content1"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Content2"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Content2"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Content3"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Content3"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Content4"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Content4"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Content5"]))
                    {
                      specialJobDetail.remarks1 =drTemp["Content5"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Feeder_Vessel_Alpha"]))
                    {
                      specialJobDetail.feederVesselAlpha =drTemp["Feeder_Vessel_Alpha"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Feeder_Voyage_Number"]))
                    {
                        specialJobDetail.feederVoyageNumber =drTemp["Feeder_Voyage_Number"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Mother_vessel_Alpha"]))
                    {
                    specialJobDetail.motherVesselAlpha =drTemp["Mother_vessel_Alpha"].ToString();
                    }
                    if (!Convert.IsDBNull(drTemp["Mother_Voyage_Number"]))
                    {
                        specialJobDetail.motherVoyageNumber =drTemp["Mother_Voyage_Number"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["Shipping_Ling_Code"]))
                    {
                        specialJobDetail.shippingLingCode =drTemp["Shipping_Ling_Code"].ToString();
                    }
                    if (!Convert.IsDBNull(drTemp["Term_Code"]))
                    {
                    specialJobDetail.termCode =drTemp["Term_Code"].ToString(); 
                    }
                    if (!Convert.IsDBNull(drTemp["UOM_Code"]))
                    {
                        specialJobDetail.uomCode =drTemp["UOM_Code"].ToString(); 
                    }
                    specialJobDetail.systemJobRelation = drTemp["System_Job_Relation"].ToString(); 
                }
                return specialJobDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       // 2014-04-10 Zhou Kai comments out this function     
        //internal static SpecialJobDetail GetSpecialJobDetail(IDataReader reader)
        //{
        //    decimal container_other,Container_20,Container_40,Container_45 ;

        //    //if (reader["Container_Other"] != null)
        //    //{
        //    //    container_other = Convert.ToDecimal(reader["Container_Other"]);
        //    //}
        //    //else
        //    //{
        //    //    container_other = 0;
        //    //}
        //    //if (reader["Container_20"] != null)
        //    //{
        //    //    Container_20 = Convert.ToDecimal(reader["Container_20"]);
        //    //}
        //    //else
        //    //{
        //    //    Container_20 = 0; 
        //    //}
        //    SpecialJobDetail jobDetail = new SpecialJobDetail(
        //                                                      (string)reader["Job_Number"],(string)reader["Job_Type_Code"],
        //                                                      (string)reader["Branch_Code"],(string)reader["Shipper_Code"],
        //                                                      (string)reader["Shipper_Name"],(string)reader["Consignee_Code"],
        //                                                      (string)reader["Consignee_Name"],(string)reader["Oversea_Agent_Name"],
        //                                                      (string)reader["Oversea_Agent_Code"],(string)reader["Routing_Agent_Code"],
        //                                                      (string)reader["Routing_Agent_Name"],(string)reader["Origin"],
        //                                                      (string)reader["Destination"],(string)reader["Via_City"],
        //                                                      (string)reader["Port_of_Loading"],(string)reader["Port_of_Discharge"],
        //    (DateTime)reader["Flight_Date"],(DateTime)reader["Arrival_Date"],
        //   // Convert.ToInt16(reader["PCS"]),Convert.ToDecimal(reader["Gross_Weight"]),
        //   // Convert.ToDecimal(reader["Charge_Weight"]),Convert.ToDecimal(reader["CBM"]),
        //   //Convert.ToDecimal(reader["Container_20"]), Convert.ToDecimal(reader["Container_40"]),
        //   // Convert.ToDecimal(reader["Container_45"])
        //   0,0,0,0,0,0,
        //    0,(string)reader["Airline"],
        //    (string)reader["Coloader"],(string)reader["Remarks1"],
        //    (string)reader["Remarks2"],(string)reader["Content1"],
        //    (string)reader["Content2"], (string)reader["Content3"],
        //    (string)reader["Content4"],(string)reader["Content5"],
        //    (string)reader["Feeder_Vessel_Alpha"],(string)reader["Feeder_Voyage_Number"],
        //    (string)reader["Mother_vessel_Alpha"],(string)reader["Mother_Voyage_Number"],
        //    (string)reader["Shipping_Ling_Code"],(string)reader["Remarks3"],
        //    (string)reader["Remarks4"],(string)reader["Remarks5"],
        //    //Convert.ToDecimal(reader["Container_Other"])
        //    0, string.Empty,
        //    string.Empty, string.Empty, (int)reader["Job_Month"], (int)reader["Job_Year"],
        //    (DateTime)reader["Change_Date_Time"], (DateTime)reader["Open_Date_Time"]);
           

        //    return  jobDetail; 

                    
                                       
        //}
        // 2014-04-10 Zhou Kai ends

        internal static string GetJobTypeCode(string jobNumber,string branchCode)
        {
            try
            {
                string sqlGetJobTypeCode = " Select Job_Type_Code from AM_Special_Job_Detail_Tbl " +
                                           " Where Job_Number ='" + jobNumber + "'" +
                                           " and Branch_Code ='" + branchCode + "'";
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                SqlCommand cmd = new SqlCommand(sqlGetJobTypeCode, cn);
                cn.Open();
                string JobTypeCode = cmd.ExecuteScalar().ToString(); 
                cn.Close();
                return JobTypeCode; 
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        // 2014-04-09 Zhou Kai adds another two parameters to the stored procedure:
        //Master_Number and House_Number
        public static bool AddTptJobToSpecialJobDetail(SpecialJobDetail specialJobDetail, SqlConnection con,
            SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_SPECIAL_JOB_DETAIL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Open_Date_Time", specialJobDetail.openDateTime);//20130322 - gerry change value from DateTime.Today);
                cmd.Parameters.AddWithValue("@Change_Date_Time", specialJobDetail.changeDate);
                cmd.Parameters.AddWithValue("@Job_Month", specialJobDetail.jobMonth);
                cmd.Parameters.AddWithValue("@Job_Year", specialJobDetail.jobYear);
                cmd.Parameters.AddWithValue("@System_Job_Relation", ConstSystemJobRelation);
                cmd.Parameters.AddWithValue("@MISC_Flag", "F");
                cmd.Parameters.AddWithValue("@Job_Number", specialJobDetail.jobNo);
                cmd.Parameters.AddWithValue("@Job_Type_Code", specialJobDetail.jobTypeCode);
                cmd.Parameters.AddWithValue("@Branch_Code", specialJobDetail.branchCode);
                cmd.Parameters.AddWithValue("@Shipper_Code", specialJobDetail.shipperCode);
                cmd.Parameters.AddWithValue("@Shipper_Name", specialJobDetail.shipperName);
                cmd.Parameters.AddWithValue("@Consignee_Code", specialJobDetail.consigneeCode);
                cmd.Parameters.AddWithValue("@Consignee_Name", specialJobDetail.consigneeName);
                cmd.Parameters.AddWithValue("@Oversea_Agent_Code", specialJobDetail.overseaAgentCode);
                cmd.Parameters.AddWithValue("@Oversea_Agent_Name", specialJobDetail.overseaAgentName);
                cmd.Parameters.AddWithValue("@Routing_Agent_Code", specialJobDetail.routingAgentCode);
                cmd.Parameters.AddWithValue("@Routing_Agent_Name", specialJobDetail.routingAgentName);
                cmd.Parameters.AddWithValue("@Origin", specialJobDetail.origin);
                cmd.Parameters.AddWithValue("@Destination", specialJobDetail.destination);
                cmd.Parameters.AddWithValue("@Via_City", specialJobDetail.viaCity);
                cmd.Parameters.AddWithValue("@Port_of_Loading", specialJobDetail.portOfLoading);
                cmd.Parameters.AddWithValue("@Port_of_Discharge", specialJobDetail.portOfDischarge);
                cmd.Parameters.AddWithValue("@Flight_Date", specialJobDetail.flightDate == DateUtility.GetSQLDateTimeMinimumValue() ? DateUtility.GetSQLDateTimeMinimumValue() : specialJobDetail.flightDate); //20130322 - gerry change set sql minimum date if not change by the user
                cmd.Parameters.AddWithValue("@Arrival_Date", specialJobDetail.arrivalDate == DateUtility.GetSQLDateTimeMinimumValue() ? DateUtility.GetSQLDateTimeMinimumValue() : specialJobDetail.arrivalDate);   //20130322 - gerry change set sql minimum date if not change by the user
                cmd.Parameters.AddWithValue("@PCS", specialJobDetail.PCS);
                cmd.Parameters.AddWithValue("@Gross_Weight", specialJobDetail.grossWeight);
                cmd.Parameters.AddWithValue("@Charge_Weight", specialJobDetail.chargeWeight);
                cmd.Parameters.AddWithValue("@CBM", specialJobDetail.cbm);
                cmd.Parameters.AddWithValue("@Container_20", specialJobDetail.container20);
                cmd.Parameters.AddWithValue("@Container_40", specialJobDetail.container40);
                cmd.Parameters.AddWithValue("@Container_45", specialJobDetail.container45);
                cmd.Parameters.AddWithValue("@Airline", specialJobDetail.airLine);
                cmd.Parameters.AddWithValue("@Coloader", specialJobDetail.coloader);
                cmd.Parameters.AddWithValue("@Remarks1", specialJobDetail.remarks1);
                cmd.Parameters.AddWithValue("@Remarks2", specialJobDetail.remarks2);
                cmd.Parameters.AddWithValue("@Content1", specialJobDetail.content1);
                cmd.Parameters.AddWithValue("@Content2", specialJobDetail.content2);
                cmd.Parameters.AddWithValue("@Content3", specialJobDetail.content3);
                cmd.Parameters.AddWithValue("@Content4", specialJobDetail.content4);
                cmd.Parameters.AddWithValue("@Content5", specialJobDetail.content5);
                cmd.Parameters.AddWithValue("@Feeder_Vessel_Alpha", specialJobDetail.feederVesselAlpha);
                cmd.Parameters.AddWithValue("@Feeder_Voyage_Number", specialJobDetail.feederVoyageNumber);
                cmd.Parameters.AddWithValue("@Mother_Vessel_Alpha", specialJobDetail.motherVesselAlpha);
                cmd.Parameters.AddWithValue("@Mother_Voyage_Number", specialJobDetail.motherVoyageNumber);
                cmd.Parameters.AddWithValue("@Shipping_Ling_Code", specialJobDetail.shippingLingCode);
                cmd.Parameters.AddWithValue("@Remarks3", specialJobDetail.remarks3);
                cmd.Parameters.AddWithValue("@Remarks4", specialJobDetail.remarks4);
                cmd.Parameters.AddWithValue("@Remarks5", specialJobDetail.remarks5);
                cmd.Parameters.AddWithValue("@Container_Other", specialJobDetail.containerOther);
                cmd.Parameters.AddWithValue("@Term_Code", specialJobDetail.termCode);
                cmd.Parameters.AddWithValue("@UOM_Code", specialJobDetail.uomCode);

                // 2014-04-09 Zhou Kai adds
                cmd.Parameters.AddWithValue("@Master_Number", specialJobDetail.MasterNo);
                cmd.Parameters.AddWithValue("@House_Number", specialJobDetail.HouseNo);
                // 2014-04-09 Zhou Kai ends

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new FMException("Fail to insert Special Data in AddTptJobToSpecialJobDetail in SpecialJobDetailDAL class" + ex.Message);
            }

            return true;
        }

        public static bool EditTptJobToSpecialJobDetail(SpecialJobDetail specialJobDetail, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Edit_TPT_SPECIAL_JOB_DETAIL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Open_Date_Time", specialJobDetail.openDateTime);//20130322 - gerry added change the job master open datetime;
                cmd.Parameters.AddWithValue("@Job_Number", specialJobDetail.jobNo);
                cmd.Parameters.AddWithValue("@Shipper_Code", specialJobDetail.shipperCode);
                cmd.Parameters.AddWithValue("@Shipper_Name", specialJobDetail.shipperName);
                cmd.Parameters.AddWithValue("@Consignee_Code", specialJobDetail.consigneeCode);
                cmd.Parameters.AddWithValue("@Consignee_Name", specialJobDetail.consigneeName);
                cmd.Parameters.AddWithValue("@Port_of_Loading", specialJobDetail.portOfLoading);
                cmd.Parameters.AddWithValue("@Port_of_Discharge", specialJobDetail.portOfDischarge);
                cmd.Parameters.AddWithValue("@Content1", specialJobDetail.content1);
                cmd.Parameters.AddWithValue("@Content2", specialJobDetail.content2);
                cmd.Parameters.AddWithValue("@Content3", specialJobDetail.content3);
                cmd.Parameters.AddWithValue("@Content4", specialJobDetail.content4);
                cmd.Parameters.AddWithValue("@Content5", specialJobDetail.content5);
                cmd.Parameters.AddWithValue("@Remarks1", specialJobDetail.remarks1);
                cmd.Parameters.AddWithValue("@Remarks2", specialJobDetail.remarks2);
                cmd.Parameters.AddWithValue("@Remarks3", specialJobDetail.remarks3);
                cmd.Parameters.AddWithValue("@Remarks4", specialJobDetail.remarks4);
                cmd.Parameters.AddWithValue("@Remarks5", specialJobDetail.remarks5);
                cmd.Parameters.AddWithValue("@Change_Date_Time", specialJobDetail.changeDate);
                cmd.Parameters.AddWithValue("@Job_Month", specialJobDetail.jobMonth);
                cmd.Parameters.AddWithValue("@Job_Year", specialJobDetail.jobYear);
                //added 20121122
                cmd.Parameters.AddWithValue("@Flight_Date", specialJobDetail.flightDate == DateUtility.GetSQLDateTimeMinimumValue() ? DateUtility.GetSQLDateTimeMinimumValue() : specialJobDetail.flightDate); //20130322 - gerry change set sql minimum date if not change by the user
                cmd.Parameters.AddWithValue("@Arrival_Date", specialJobDetail.arrivalDate == DateUtility.GetSQLDateTimeMinimumValue() ? DateUtility.GetSQLDateTimeMinimumValue() : specialJobDetail.arrivalDate);   //20130322 - gerry change set sql minimum date if not change by the user

                cmd.Parameters.AddWithValue("@Mother_Vessel_Alpha", specialJobDetail.motherVesselAlpha);
                cmd.Parameters.AddWithValue("@Mother_Voyage_Number", specialJobDetail.motherVoyageNumber);
                //20130406 - gerry added
                cmd.Parameters.AddWithValue("@Booked_Weight", specialJobDetail.grossWeight);
                cmd.Parameters.AddWithValue("@Booked_Volume", specialJobDetail.cbm);
                //20130406 end

                // 2014-04-09 Zhou Kai adds
                cmd.Parameters.AddWithValue("@Master_Number", specialJobDetail.MasterNo);
                cmd.Parameters.AddWithValue("@House_Number", specialJobDetail.HouseNo);
                // 2014-04-09 Zhou Kai ends
                cmd.Parameters.AddWithValue("@Container20ft", specialJobDetail.container20);
                cmd.Parameters.AddWithValue("@Container40ft", specialJobDetail.container40);
                cmd.Parameters.AddWithValue("@Container45ft", specialJobDetail.container45);
                cmd.Parameters.AddWithValue("@ContainerOthers", specialJobDetail.containerOther);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new FMException("Fail to Edit Special Data in EditTptJobToSpecialJobDetail in SpecialJobDetailDAL class" + ex.Message);
            }

            return true;
        }


        public static bool DeleteTptJobToSpecialJobDetail(string jobNo, SqlConnection con,
            SqlTransaction tran)
        {
            try
            {
                string SQLDeleteString = "DELETE FROM AM_Job_Master_Tbl where Job_Number='"+jobNo+"'";
                SqlCommand cmd = new SqlCommand(SQLDeleteString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                SQLDeleteString = "DELETE FROM AM_Special_Job_Detail_Tbl where Job_Number='" + jobNo + "'";
                cmd = new SqlCommand(SQLDeleteString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException("Fail to delete in DeleteTptJobToSpecialJobDetail Error :" + ex.ToString());
                return false;
            }
            return true;
        }

        internal static bool EditSpecialJobDetailGrossWeightVolume(string jobNo, decimal newGrossWeight, decimal newGrossCBM, SqlConnection con,
            SqlTransaction tran)
        {
            try
            {
                string SQLEditString = @"update AM_Special_Job_Detail_Tbl
                                            set Gross_Weight = {1},
	                                            CBM= {2}
                                            where Job_Number = '{0}'";
                SQLEditString = string.Format(SQLEditString, jobNo, newGrossWeight, newGrossCBM);
                SqlCommand cmd = new SqlCommand(SQLEditString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

    }
}
