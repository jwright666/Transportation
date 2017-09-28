using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TR_WMSDLL.BLL;

namespace TR_WMSDLL.DAL
{
    public class GINDAL
    {
        internal static List<string> GetOutstandingWMS_GINNos(string transporterCode)
        {
            List<string> ginNos = new List<string>();

            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string SQLString = @"select distinct gin.GIN_No from WMS_GIN_Master_Tbl gin
	                                inner join TRK_JOB_FROM_WMS_Tbl trk
	                                ON trk.TRX_KEY = gin.Trx_Key
                                    where gin.GIN_No in (select GIN_No from TRK_JOB_FROM_WMS_Tbl where IS_CONFIRMED ='F') 
                                    ORDER BY gin.GIN_No desc";
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ginNos.Add((string)reader["GIN_No"]);
                    }
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                catch (Exception ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                finally { cn.Close(); }
            }
            return ginNos;
        }

        internal static GINHeader GetGINHeader(IDataReader reader)
        {
            GINHeader tempGINHeader = new GINHeader();
            tempGINHeader.delivery_type = reader["Delivery_Type"] == DBNull.Value ? string.Empty : (string)reader["Delivery_Type"];
            tempGINHeader.gin_no = reader["GIN_No"] == DBNull.Value ? string.Empty : (string)reader["GIN_No"];
            tempGINHeader.custCode = reader["Customer_Code"] == DBNull.Value ? string.Empty : (string)reader["Customer_Code"];
            tempGINHeader.branchCode = reader["Branch_Code"] == DBNull.Value? string.Empty : (string)reader["Branch_Code"];
            tempGINHeader.shipperCode = reader["Shipper_Code"] == DBNull.Value  ? string.Empty : (string)reader["Shipper_Code"];
            tempGINHeader.shipperName = reader["Shipper_Name"] == DBNull.Value  ? string.Empty : (string)reader["Shipper_Name"];
            tempGINHeader.shipperAdd1 = reader["Shipper_Add1"] == DBNull.Value ? string.Empty : (string)reader["Shipper_Add1"];
            tempGINHeader.shipperAdd2 = reader["Shipper_Add2"] == DBNull.Value  ? string.Empty : (string)reader["Shipper_Add2"];
            tempGINHeader.shipperAdd3 = reader["Shipper_Add3"] == DBNull.Value  ? string.Empty : (string)reader["Shipper_Add3"];
            tempGINHeader.shipperAdd4 = reader["Shipper_Add4"] == DBNull.Value  ? string.Empty : (string)reader["Shipper_Add4"];
            tempGINHeader.consigneeCode = reader["Consignee_Code"] == DBNull.Value  ? string.Empty : (string)reader["Consignee_Code"];
            tempGINHeader.consigneeName = reader["Consignee_Name"] == DBNull.Value  ? string.Empty : (string)reader["Consignee_Name"];
            tempGINHeader.consigneeAdd1 = reader["Consignee_Add1"] == DBNull.Value  ? string.Empty : (string)reader["Consignee_Add1"];
            tempGINHeader.consigneeAdd2 = reader["Consignee_Add2"] == DBNull.Value  ? string.Empty : (string)reader["Consignee_Add2"];
            tempGINHeader.consigneeAdd3 = reader["Consignee_Add3"] == DBNull.Value  ? string.Empty : (string)reader["Consignee_Add3"];
            tempGINHeader.consigneeAdd4 = reader["Consignee_Add4"] == DBNull.Value  ? string.Empty : (string)reader["Consignee_Add4"];
            tempGINHeader.hawb = reader["House_No"] == DBNull.Value  ? string.Empty : (string)reader["House_No"];
            tempGINHeader.mawb = reader["Master_No"] == DBNull.Value ? string.Empty : (string)reader["Master_No"];
            //tempGINHeader.awb = reader["Branch_Code"] == DBNull.Value  ? string.Empty : (string)reader["Branch_Code"];
            //tempGINHeader.oblNumber = reader["Branch_Code"] == DBNull.Value ? string.Empty : (string)reader["Branch_Code"];
            //tempGINHeader.hblNumber = reader["Branch_Code"] == DBNull.Value  ? string.Empty : (string)reader["Branch_Code"];
            tempGINHeader.shippingLine = reader["Shipping_Line"] == DBNull.Value  ? string.Empty : (string)reader["Shipping_Line"];
            tempGINHeader.mVesselName = reader["Vessel_Name"] == DBNull.Value  ? string.Empty : (string)reader["Vessel_Name"];
            tempGINHeader.mVoyage = reader["Voyage_No"] == DBNull.Value  ? string.Empty : (string)reader["Voyage_No"];
            tempGINHeader.pol = reader["POL"] == DBNull.Value  ? string.Empty : (string)reader["POL"];
            //tempGINHeader.polName = reader["Branch_Code"] == DBNull.Value ? string.Empty : (string)reader["Branch_Code"];
            tempGINHeader.pod = reader["POD"] == DBNull.Value ? string.Empty : (string)reader["POD"];
            //tempGINHeader.podName = reader["Branch_Code"] == DBNull.Value ? string.Empty : (string)reader["Branch_Code"];
            tempGINHeader.warehouseNo = reader["Warehouse_No"] == DBNull.Value  ? string.Empty : (string)reader["Warehouse_No"];
            //tempGINHeader.yourRefNo = reader["Branch_Code"] == DBNull.Value  ? string.Empty : (string)reader["Branch_Code"];
            tempGINHeader.issue_date = reader["Issue_Date"] == DBNull.Value ? DateTime.Today : (DateTime)reader["Issue_Date"];
            tempGINHeader.wh_staff_id = reader["Confirmed_By"] == DBNull.Value ? string.Empty : (string)reader["Confirmed_By"];

            return tempGINHeader;
        }
        internal static SortableList<GINHeader> GetOutStandingGINFromWMS()
        {
            SortableList<GINHeader> ginHeaders = new SortableList<GINHeader>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string SQLString = @"select distinct gin.GIN_No, opDB.Customer_Code,  gin.Issue_Date, gin.Confirmed_By,gin.Branch_Code, gin.Delivery_Type,
                                        ship.Shipper_Code, ship.Shipper_Name, ship.Shipper_Add1, ship.Shipper_Add2, ship.Shipper_Add3, ship.Shipper_Add4, 
                                        ship.Consignee_Code, ship.Consignee_Name, ship.Consignee_Add1, ship.Consignee_Add2, ship.Consignee_Add3, ship.Consignee_Add4, 
                                        ship.House_No, ship.Master_No, ship.Shipping_Line, ship.Vessel_Name, ship.Voyage_No, ship.POL, ship.POD,
                                        loc_out.Warehouse_No 
                                        from WMS_GIN_Master_Tbl gin
	                                    inner join CRT_Operation_Database_Tbl opDB
	                                    ON opDB.Operation_Code = gin.Owner_Code
                                        inner join WMS_Movement_Location_Out_Details_Tbl loc_out 
                                        ON loc_out.Master_Trx_Key = gin.Trx_Key
                                        left join WMS_GIN_Shipping_Tbl ship
                                        ON ship.Trx_key = gin.Trx_Key
	                                    inner join TRK_JOB_FROM_WMS_Tbl trk
	                                    ON trk.TRX_KEY = gin.Trx_Key
                                        where gin.GIN_No in (select GIN_No from TRK_JOB_FROM_WMS_Tbl where IS_CONFIRMED ='F') 
                                        ORDER BY gin.GIN_No desc";
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ginHeaders.Add(GetGINHeader(reader));
                    }
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                catch (Exception ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                finally { cn.Close(); }            
            }
            return ginHeaders;
        }
        internal static GINDetail GetGINDetail(IDataReader reader)
        {
            GINDetail tempGINDetail = new GINDetail();
            tempGINDetail.gin_no = reader["GIN_No"] == DBNull.Value ? string.Empty : (string)reader["GIN_No"];
            tempGINDetail.itemTrxKey = reader["Item_Trx_Key"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Item_Trx_Key"]);
            tempGINDetail.productCode = reader["Product_Code"] == DBNull.Value ? string.Empty : (string)reader["Product_Code"];
            tempGINDetail.productName = reader["Product_Name"] == DBNull.Value ? string.Empty : (string)reader["Product_Name"];
            tempGINDetail.item_uom = reader["Base_UOM"] == DBNull.Value ? string.Empty : (string)reader["Base_UOM"];
            tempGINDetail.qty = reader["Base_Qty"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Base_Qty"]);
            tempGINDetail.unitWeight = reader["GW"] == DBNull.Value ? 0 : (Convert.ToDecimal(reader["GW"]) / Convert.ToDecimal(tempGINDetail.qty));
            tempGINDetail.dimension_uom = reader["Base_UOM"] == DBNull.Value ? string.Empty : (string)reader["Base_UOM"];
            tempGINDetail.length = reader["Length"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Length"]);
            tempGINDetail.width = reader["Width"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Width"]);
            tempGINDetail.height = reader["Height"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Height"]);
            tempGINDetail.totalVolume = reader["CBM"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["CBM"]);
            tempGINDetail.totalWeight = reader["GW"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["GW"]);
            tempGINDetail.originCode = reader["WH_CODE"] == DBNull.Value ? string.Empty : (string)reader["WH_CODE"];
            tempGINDetail.originName = reader["WH_NAME"] == DBNull.Value ? string.Empty : (string)reader["WH_NAME"];
            tempGINDetail.originAdd1 = reader["WH_ADD1"] == DBNull.Value ? string.Empty : (string)reader["WH_ADD1"];
            tempGINDetail.originAdd2 = reader["WH_ADD2"] == DBNull.Value ? string.Empty : (string)reader["WH_ADD2"];
            tempGINDetail.originAdd3 = reader["WH_ADD3"] == DBNull.Value ? string.Empty : (string)reader["WH_ADD3"];
            tempGINDetail.originAdd4 = reader["WH_ADD4"] == DBNull.Value ? string.Empty : (string)reader["WH_ADD4"];
            string delivery_type = reader["Delivery_Type"] == DBNull.Value ? string.Empty : (string)reader["Delivery_Type"];
            if (delivery_type == "S")
            {
                tempGINDetail.destinationCode = reader["Single_Delivery_Code"] == DBNull.Value ? string.Empty : (string)reader["Single_Delivery_Code"];
                tempGINDetail.destinationName = reader["Single_Delivery_Name"] == DBNull.Value ? string.Empty : (string)reader["Single_Delivery_Name"];
                tempGINDetail.destinationAdd1 = reader["Single_Delivery_Add1"] == DBNull.Value ? string.Empty : (string)reader["Single_Delivery_Add1"];
                tempGINDetail.destinationAdd2 = reader["Single_Delivery_Add2"] == DBNull.Value ? string.Empty : (string)reader["Single_Delivery_Add2"];
                tempGINDetail.destinationAdd3 = reader["Single_Delivery_Add3"] == DBNull.Value ? string.Empty : (string)reader["Single_Delivery_Add3"];
                tempGINDetail.destinationAdd4 = reader["Single_Delivery_Add4"] == DBNull.Value ? string.Empty : (string)reader["Single_Delivery_Add4"];
            }
            else
            { 
                tempGINDetail.destinationCode = reader["Delivery_Code"] == DBNull.Value ? string.Empty : (string)reader["Delivery_Code"];
                tempGINDetail.destinationName = reader["Delivery_Name"] == DBNull.Value ? string.Empty : (string)reader["Delivery_Name"];
                tempGINDetail.destinationAdd1 = reader["Delivery_Add1"] == DBNull.Value ? string.Empty : (string)reader["Delivery_Add1"];
                tempGINDetail.destinationAdd2 = reader["Delivery_Add2"] == DBNull.Value ? string.Empty : (string)reader["Delivery_Add2"];
                tempGINDetail.destinationAdd3 = reader["Delivery_Add3"] == DBNull.Value ? string.Empty : (string)reader["Delivery_Add3"];
                tempGINDetail.destinationAdd4 = reader["Delivery_Add4"] == DBNull.Value ? string.Empty : (string)reader["Delivery_Add4"];
            }




            return tempGINDetail;
        }
        internal static SortableList<GINDetail> GetOutStandingGINDetailsFromWMS(string ginNo)
        {
            SortableList<GINDetail> ginDetails = new SortableList<GINDetail>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string SQLString = @"select gin.Delivery_Type, gin.GIN_No, loc_out.Item_Trx_Key, gin.Owner_Code, gin.Owner_Name, gin.Issue_Date, gin.Confirmed_By,
                                        item.Product_Code, item.Product_Name, item.Length, item.Width, item.Height,  item.Dimension_UOM,
                                        loc_out.Base_Qty, loc_out.Base_UOM, loc_out.GW, loc_out.CBM,
                                        gin.Delivery_Code as Single_Delivery_Code,gin.Delivery_Name as Single_Delivery_Name, gin.Delivery_Add1 as Single_Delivery_add1, gin.Delivery_Add2 as Single_Delivery_add2,
                                        gin.Delivery_Add3 as Single_Delivery_add3, gin.Delivery_Add4 as Single_Delivery_add4,
                                        item.Delivery_Code,item.Delivery_Name, item.Delivery_Add1, item.Delivery_Add2, item.Delivery_Add3, item.Delivery_Add4,
                                        wh_info.Warehouse_No [WH_CODE], wh_info.Name [WH_NAME], wh_info.Address1 [WH_ADD1], wh_info.Address2 [WH_ADD2], wh_info.Address3 [WH_ADD3], wh_info.Address4 [WH_ADD4]

                                        from WMS_Movement_Location_Out_Details_Tbl loc_out 
                                        inner join WMS_GIN_Master_Tbl gin
                                        ON loc_out.Master_Trx_key = gin.Trx_Key
                                        inner join Wms_ref_warehouse_tbl wh_info
                                        ON wh_info.Warehouse_No = loc_out.Warehouse_No
                                        inner join WMS_Movement_Items_Out_Details_Tbl item
                                        ON item.Master_Trx_key = loc_out.Master_Trx_Key 
                                        and item.Trx_key = loc_out.Item_Trx_Key
                                        where gin.GIN_No = '{0}'	
                                        ORDER BY gin.GIN_No desc";
                SQLString = string.Format(SQLString, ginNo);
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ginDetails.Add(GetGINDetail(reader));
                    }
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                catch (Exception ex) { throw new FMException("GINDAL Error : GetOutstandingWMS_GINNos. " + ex.Message); }
                finally { cn.Close(); }
            }
            return ginDetails;
        }
    }
}
