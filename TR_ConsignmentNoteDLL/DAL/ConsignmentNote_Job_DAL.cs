using FM.FMSystem.BLL;
using FM.FMSystem.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TR_ConsignmentNote.BLL;

namespace TR_ConsignmentNote.DAL
{
    internal class ConsignmentNote_Job_DAL
    {
        internal static ConsignmentNote_Job_Master FillConsignmentNoteJobMasterProperties(IDataReader reader)
        {
            ConsignmentNote_Job_Master jobMaster = new ConsignmentNote_Job_Master();
            try
            {
                jobMaster.Job_ID = reader["Job_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Job_ID"].ToString());
                jobMaster.Job_No = reader["Job_No"] == DBNull.Value ? string.Empty : reader["Job_No"].ToString();
                jobMaster.Customer_Code = reader["Company_Id"] == DBNull.Value ? string.Empty : reader["Company_Id"].ToString();
                jobMaster.Branch_Code = reader["Branch_Code"] == DBNull.Value ? string.Empty : reader["Branch_Code"].ToString();
                jobMaster.Master_Job_No = reader["Master_Job_No"] == DBNull.Value ? string.Empty : reader["Master_Job_No"].ToString();
                jobMaster.Booking_No = reader["Booking_No"] == DBNull.Value ? string.Empty : reader["Booking_No"].ToString();
                jobMaster.Tracking_No = reader["Tracking_No"] == DBNull.Value ? string.Empty : reader["Tracking_No"].ToString();
                jobMaster.Consignment_Note_No = reader["Consignment_Note_No"] == DBNull.Value ? string.Empty : reader["Consignment_Note_No"].ToString();
                jobMaster.Shipment_Term = reader["Shipment_Term"] == DBNull.Value ? string.Empty : reader["Shipment_Term"].ToString();
                jobMaster.Shipment_Type = reader["Shipment_Type"] == DBNull.Value ? string.Empty : reader["Shipment_Type"].ToString();
                jobMaster.Mode_Of_Transport = reader["Mode_Of_Transport"] == DBNull.Value ? string.Empty : reader["Mode_Of_Transport"].ToString();
                jobMaster.Service_Term = reader["Service_Term"] == DBNull.Value ? string.Empty : reader["Service_Term"].ToString();
                jobMaster.Delivery_Term = reader["Delivery_Term"] == DBNull.Value ? string.Empty : reader["Delivery_Term"].ToString();
                jobMaster.Master_No = reader["Master_No"] == DBNull.Value ? string.Empty : reader["Master_No"].ToString();
                jobMaster.SubMaster_No = reader["SubMaster_No"] == DBNull.Value ? string.Empty : reader["SubMaster_No"].ToString();
                jobMaster.House_No = reader["House_No"] == DBNull.Value ? string.Empty : reader["House_No"].ToString();
                jobMaster.Origin_Code = reader["Origin_Code"] == DBNull.Value ? string.Empty : reader["Origin_Code"].ToString();
                jobMaster.Origin_Name = reader["Origin_Name"] == DBNull.Value ? string.Empty : reader["Origin_Name"].ToString();
                jobMaster.Origin_Country_Code = reader["Origin_Country_Code"] == DBNull.Value ? string.Empty : reader["Origin_Country_Code"].ToString();
                jobMaster.VIA_City_Code = reader["VIA_City_Code"] == DBNull.Value ? string.Empty : reader["VIA_City_Code"].ToString();
                jobMaster.VIA_City_Name = reader["VIA_City_Name"] == DBNull.Value ? string.Empty : reader["VIA_City_Name"].ToString();
                jobMaster.VIA_Country_Code = reader["VIA_Country_Code"] == DBNull.Value ? string.Empty : reader["VIA_Country_Code"].ToString();
                jobMaster.Dest_Code = reader["Dest_Code"] == DBNull.Value ? string.Empty : reader["Dest_Code"].ToString();
                jobMaster.Dest_Name = reader["Dest_Name"] == DBNull.Value ? string.Empty : reader["Dest_Name"].ToString();
                DateTime eta = reader["ETA"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["ETA"].ToString());
                string eta_time = reader["ETA_Time"] == DBNull.Value ? "0000" : reader["ETA_Time"].ToString();
                int eta_hr = Convert.ToInt32(eta_time.Substring(0, 2).ToString());
                int eta_min = Convert.ToInt32(eta_time.Substring(2, 2).ToString());
                jobMaster.ETA_Time = eta.AddHours(eta_hr).AddMinutes(eta_min);
                DateTime etd = reader["ETD"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["ETD"].ToString());
                string etd_time = reader["ETD_Time"] == DBNull.Value ? "0000" : reader["ETD_Time"].ToString();
                int etd_hr = Convert.ToInt32(etd_time.Substring(0, 2).ToString());
                int etd_min = Convert.ToInt32(etd_time.Substring(2, 2).ToString());
                jobMaster.ETD_Time = etd.AddHours(etd_hr).AddMinutes(etd_min);
                jobMaster.POL_Code = reader["POL_Code"] == DBNull.Value ? string.Empty : reader["POL_Code"].ToString();
                jobMaster.POL_Name = reader["POL_Name"] == DBNull.Value ? string.Empty : reader["POL_Name"].ToString();
                jobMaster.VIA_Port_Code = reader["VIA_Port_Code"] == DBNull.Value ? string.Empty : reader["VIA_Port_Code"].ToString();
                jobMaster.VIA_Port_Name = reader["VIA_Port_Name"] == DBNull.Value ? string.Empty : reader["VIA_Port_Name"].ToString();
                jobMaster.POD_Code = reader["POD_Code"] == DBNull.Value ? string.Empty : reader["POD_Code"].ToString();
                jobMaster.POD_Name = reader["POD_Name"] == DBNull.Value ? string.Empty : reader["POD_Name"].ToString();
                jobMaster.POI_Code = reader["POI_Code"] == DBNull.Value ? string.Empty : reader["POI_Code"].ToString();
                jobMaster.POI_Name = reader["POI_Name"] == DBNull.Value ? string.Empty : reader["POI_Name"].ToString();
                jobMaster.Date_Of_Issue = reader["Date_Of_Issue"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["Date_Of_Issue"].ToString());
                jobMaster.Delivery_Place_Code = reader["Delivery_Place_Code"] == DBNull.Value ? string.Empty : reader["Delivery_Place_Code"].ToString();
                jobMaster.Delivery_Place_Name = reader["Delivery_Place_Name"] == DBNull.Value ? string.Empty : reader["Delivery_Place_Name"].ToString();
                jobMaster.Receipt_Place_Code = reader["Receipt_Place_Code"] == DBNull.Value ? string.Empty : reader["Receipt_Place_Code"].ToString();
                jobMaster.Receipt_Place_Name = reader["Receipt_Place_Name"] == DBNull.Value ? string.Empty : reader["Receipt_Place_Name"].ToString();
                jobMaster.PO_No1 = reader["PO_No1"] == DBNull.Value ? string.Empty : reader["PO_No1"].ToString();
                jobMaster.PO_No2 = reader["PO_No2"] == DBNull.Value ? string.Empty : reader["PO_No2"].ToString();
                jobMaster.Permit_No1 = reader["Permit_No1"] == DBNull.Value ? string.Empty : reader["Permit_No1"].ToString();
                jobMaster.Permit_No2 = reader["Permit_No2"] == DBNull.Value ? string.Empty : reader["Permit_No2"].ToString();
                jobMaster.Cost_Centre = reader["Cost_Centre"] == DBNull.Value ? string.Empty : reader["Cost_Centre"].ToString();
                jobMaster.Added_By = reader["Added_By"] == DBNull.Value ? string.Empty : reader["Added_By"].ToString();
                jobMaster.Added_DateTime = reader["Added_DateTime"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["Added_DateTime"].ToString());
                jobMaster.Edited_By = reader["Edited_By"] == DBNull.Value ? string.Empty : reader["Edited_By"].ToString();
                jobMaster.Editing_Datetime = reader["Editing_Datetime"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["Editing_Datetime"].ToString());
            }
            catch (Exception ex) { throw ex; }
            return jobMaster;
        }
        internal static ConsignmentNote_Job_Master GetConsignmentNoteJobMaster(string consignment_Note_No)
        {
            ConsignmentNote_Job_Master retValue = null;
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"select * from Job_Master WHERE Consignment_Note_No='" + consignment_Note_No + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = FillConsignmentNoteJobMasterProperties(reader);
                }
                reader.Close();
            }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
        internal static List<string> GetConsignmentNoteNos(string house_No)
        {
            List<string> retValue = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"select distinct Consignment_Note_No from Job_Master WHERE Job_Type ='CN' AND Consignment_Note_No <> '''' ";
                if (house_No != string.Empty)
                    SQLString += " AND House_No ='" + house_No + "'";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue.Add(reader.GetString(0));
                }
                reader.Close();
            }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
        internal static List<string> GetConsignmentNoteHouseNos()
        {
            List<string> retValue = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"select distinct House_No from Job_Master WHERE Job_Type ='CN' AND House_No <> '''' ";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue.Add(reader.GetString(0));
                }
                reader.Close();
            }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
        internal static ConsignmentNote_Job_Goods_Details FillConsignmentNoteJobGoodsDetailsProperties(IDataReader reader)
        {
            ConsignmentNote_Job_Goods_Details jobDetail = new ConsignmentNote_Job_Goods_Details();
            try
            {
                jobDetail.Job_ID = reader["Job_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Job_ID"].ToString());
                jobDetail.Seq_No = reader["Seq_No"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Seq_No"].ToString());
                jobDetail.Qty = reader["Qty"] == DBNull.Value ? 1 : Convert.ToDecimal(reader["Qty"].ToString());
                jobDetail.CBM = reader["CBM"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CBM"].ToString());
                jobDetail.Gross_Weight = reader["Gross_Weight"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Gross_Weight"].ToString());
                jobDetail.Goods_Desc = reader["Goods_Desc"] == DBNull.Value ? string.Empty : reader["Goods_Desc"].ToString();
                jobDetail.UOM = reader["UOM"] == DBNull.Value ? string.Empty : reader["UOM"].ToString();
                jobDetail.Container_Code = reader["Container_Code"] == DBNull.Value ? string.Empty : reader["Container_Code"].ToString();
                jobDetail.Container_No = reader["Container_No"] == DBNull.Value ? string.Empty : reader["Container_No"].ToString();
                jobDetail.Seal_No = reader["Seal_No"] == DBNull.Value ? string.Empty : reader["Seal_No"].ToString();
                jobDetail.Marks_No = reader["Marks_No"] == DBNull.Value ? string.Empty : reader["Marks_No"].ToString();
                jobDetail.Package_Desc = reader["Package_Desc"] == DBNull.Value ? string.Empty : reader["Package_Desc"].ToString();
                jobDetail.Cargo_Desc = reader["Cargo_Desc"] == DBNull.Value ? string.Empty : reader["Cargo_Desc"].ToString();
                jobDetail.Packing_Type = reader["Packing_Type"] == DBNull.Value ? string.Empty : reader["Packing_Type"].ToString();
                jobDetail.Commodity_Code = reader["Commodity_Code"] == DBNull.Value ? string.Empty : reader["Commodity_Code"].ToString();
                jobDetail.Customer_Product_No = reader["Customer_Product_No"] == DBNull.Value ? string.Empty : reader["Customer_Product_No"].ToString();
                jobDetail.Product_No = reader["Job_No"] == DBNull.Value ? string.Empty : reader["Product_No"].ToString();

                DateTime depot_In = reader["Depot_In_Date"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["Depot_In_Date"].ToString());
                string depot_In_time = reader["Depot_In_Time"] == DBNull.Value ? "0000" : reader["Depot_In_Time"].ToString();
                int depot_In_hr = Convert.ToInt32(depot_In_time.Substring(0, 2).ToString());
                int depot_In_min = Convert.ToInt32(depot_In_time.Substring(2, 2).ToString());
                jobDetail.Depot_In_DateTime = depot_In.AddHours(depot_In_hr).AddMinutes(depot_In_min);
                DateTime depot_Out = reader["Depot_Out_Date"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["Depot_Out_Date"].ToString());
                string depot_Out_time = reader["Depot_Out_Time"] == DBNull.Value ? "0000" : reader["Depot_Out_Time"].ToString();
                int depot_Out_hr = Convert.ToInt32(depot_Out_time.Substring(0, 2).ToString());
                int depot_Out_min = Convert.ToInt32(depot_Out_time.Substring(2, 2).ToString());
                jobDetail.Depot_Out_DateTime = depot_Out.AddHours(depot_Out_hr).AddMinutes(depot_Out_min);

                jobDetail.OriginCode = reader["OriginCode"] == DBNull.Value ? string.Empty : reader["OriginCode"].ToString();
                jobDetail.OriginName = reader["OriginName"] == DBNull.Value ? string.Empty : reader["OriginName"].ToString();
                jobDetail.OriginAdd1 = reader["OriginAdd1"] == DBNull.Value ? string.Empty : reader["OriginAdd1"].ToString();
                jobDetail.OriginAdd2 = reader["OriginAdd2"] == DBNull.Value ? string.Empty : reader["OriginAdd2"].ToString();
                jobDetail.OriginAdd3 = reader["OriginAdd3"] == DBNull.Value ? string.Empty : reader["OriginAdd3"].ToString();
                jobDetail.OriginAdd4 = reader["OriginAdd4"] == DBNull.Value ? string.Empty : reader["OriginAdd4"].ToString();
                jobDetail.DestinationCode = reader["DestinationCode"] == DBNull.Value ? string.Empty : reader["DestinationCode"].ToString();
                jobDetail.DestinationName = reader["DestinationName"] == DBNull.Value ? string.Empty : reader["DestinationName"].ToString();
                jobDetail.DestinationAdd1 = reader["DestinationAdd1"] == DBNull.Value ? string.Empty : reader["DestinationAdd1"].ToString();
                jobDetail.DestinationAdd2 = reader["DestinationAdd2"] == DBNull.Value ? string.Empty : reader["DestinationAdd2"].ToString();
                jobDetail.DestinationAdd3 = reader["DestinationAdd3"] == DBNull.Value ? string.Empty : reader["DestinationAdd3"].ToString();
                jobDetail.DestinationAdd4 = reader["DestinationAdd4"] == DBNull.Value ? string.Empty : reader["DestinationAdd4"].ToString();

                jobDetail.Job_Goods_Dimensions = GetConsignmentNoteJobGoodsDimensions(jobDetail.Job_ID, jobDetail.Seq_No);
            }
            catch (Exception ex) { throw ex; }
            return jobDetail;
        }
        internal static SortableList<ConsignmentNote_Job_Goods_Details> GetConsignmentNoteJobGoodsDetails(string consignment_Note_No)
        {
            SortableList<ConsignmentNote_Job_Goods_Details> retValue = new SortableList<ConsignmentNote_Job_Goods_Details>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"select * from job_goods_details as jobdetails
                                        inner join job_master as jobmaster
                                        on jobmaster.Job_ID = jobdetails.Job_ID
                                        where jobmaster.Consignment_Note_No = '" + consignment_Note_No + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ConsignmentNote_Job_Goods_Details jobDetail = FillConsignmentNoteJobGoodsDetailsProperties(reader);
                    retValue.Add(jobDetail);
                }
                reader.Close();
                cn.Close();
            }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
        internal static ConsignmentNote_Job_Goods_Dimension FillConsignmentNoteJobGoodsDimensionsProperties(IDataReader reader)
        {
            ConsignmentNote_Job_Goods_Dimension jobGoodsDimension = new ConsignmentNote_Job_Goods_Dimension();
            try
            {
                jobGoodsDimension.Job_ID = reader["Job_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Job_ID"].ToString());
                jobGoodsDimension.Seq_No = reader["Seq_No"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Seq_No"].ToString());
                jobGoodsDimension.Seq_No = reader["Sub_Seq_No"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Sub_Seq_No"].ToString());
                jobGoodsDimension.Qty = reader["Qty"] == DBNull.Value ? 1 : Convert.ToDecimal(reader["Qty"].ToString());
                jobGoodsDimension.CBM = reader["CBM"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CBM"].ToString());
                jobGoodsDimension.Gross_Weight = reader["Gross_Weight"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Gross_Weight"].ToString());
                jobGoodsDimension.Nett_Weight = reader["Nett_Weight"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Nett_Weight"].ToString());
                jobGoodsDimension.Width = reader["Width"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Width"].ToString());
                jobGoodsDimension.Length = reader["Length"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Length"].ToString());
                jobGoodsDimension.Height = reader["Height"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Height"].ToString());
            }
            catch (Exception ex) { throw ex; }
            return jobGoodsDimension;
        }
        internal static SortableList<ConsignmentNote_Job_Goods_Dimension> GetConsignmentNoteJobGoodsDimensions(int jobID, int seqNo)
        {
            SortableList<ConsignmentNote_Job_Goods_Dimension> retValue = new SortableList<ConsignmentNote_Job_Goods_Dimension>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"select * from Job_Goods_Dimensions as jobdetails
                                        inner join job_master as jobmaster
                                        on jobmaster.Job_ID = jobdetails.Job_ID
                                        where jobmaster.job_id = {0} and jobdetails.Seq_No={1}";

                SQLString = string.Format(SQLString, jobID, seqNo);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue.Add(FillConsignmentNoteJobGoodsDimensionsProperties(reader));
                }
                reader.Close();
                cn.Close();
            }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
        internal static SortableList<ConsignmentNote_Job_Goods_Dimension> GetConsignmentNoteJobGoodsDimensions(int jobID, int seqNo, SqlConnection con, SqlTransaction tran)
        {
            SortableList<ConsignmentNote_Job_Goods_Dimension> retValue = new SortableList<ConsignmentNote_Job_Goods_Dimension>();
            try
            {
                string SQLString = @"select * from job_goods_details as jobdetails
                                        inner join job_master as jobmaster
                                        on jobmaster.Job_ID = jobdetails.Job_ID
                                        where jobmaster.job_id = {0} and jobdetails.Seq_No={1}";

                SQLString = string.Format(SQLString, jobID, seqNo);
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue.Add(FillConsignmentNoteJobGoodsDimensionsProperties(reader));
                }
                reader.Close();
            }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
    }
}
