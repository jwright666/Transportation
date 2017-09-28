using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_SeaFreightDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_SeaFreightDLL.DAL
{
    internal class SeaFreightDAL
    {
        internal static string GetTransportCode()
        {
            string code = "";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT TPT_CODE FROM TPT_SPECIAL_DATA_Tbl";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                code = (string)reader["TPT_CODE"];
            }
            cn.Close();
            return code;
        }

        internal static void GetSI_Job_Detail_Delivery_Tbl(SeaImportJob sijob, int import_no,string tcode)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT TOP 1 * FROM SI_Job_Detail_Delivery_Tbl";
                SQLString += " WHERE Transporter_Code='" + tcode +"'";
                SQLString += " AND Import_Number=" + import_no ;
                SQLString += " ORDER BY Delivery_PK Asc";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sijob.DeliveryCode = (string)reader["Delivery_To_Code"];
                    sijob.DeliveryName = (string)reader["Delivery_To_Name"];
                    sijob.DeliveryAdd1 = (string)reader["Delivery_To_Add1"];
                    sijob.DeliveryAdd1 = (string)reader["Delivery_To_Add2"];
                    sijob.DeliveryAdd1 = (string)reader["Delivery_To_Add3"];
                    sijob.DeliveryAdd1 = (string)reader["Delivery_To_Add4"];
                    sijob.TransportCode = (string)reader["Transporter_Code"];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool IsTransportCodeInSI_Job_Detail_Delivery_Tbl(int import_no, string tcode)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLString = "SELECT TOP 1 * FROM SI_Job_Detail_Delivery_Tbl";
                SQLString += " WHERE Transporter_Code='" + tcode + "'";
                SQLString += " AND Import_Number=" + import_no;
                SQLString += " ORDER BY Delivery_PK Asc";
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static List<string> GetOutstandingSeaExportJobNos(string transporterCode, string shipmentType)
        {
            List<string> jobnos = new List<string>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            //string SQLString = "SELECT DISTINCT SE_Job_Master_Tbl.Master_Job_Number ";
            string SQLString = "SELECT DISTINCT SE_Job_Master_Tbl.Job_Number ";
            SQLString += " FROM SE_Job_Master_Tbl,SE_Job_Detail_Party_Tbl,TPT_SPECIAL_DATA_Tbl,SE_Job_Detail_Shipment_Tbl";
            SQLString += " where SE_Job_Master_Tbl.Export_Number=SE_Job_Detail_Party_Tbl.Export_Number";
            // 2014-07-10 Zhou Kai comments out the one line below
            //SQLString += " AND SE_Job_Detail_Party_Tbl.Transport_Code = TPT_SPECIAL_DATA_Tbl.TPT_CODE";
            // 2014-07-10 Zhou Kai ends
            SQLString += " AND SE_Job_Master_Tbl.Export_Number=SE_Job_Detail_Shipment_Tbl.Export_Number";

            //SQLString += " AND (SE_Job_Master_Tbl.Export_Type='MBL' OR SE_Job_Master_Tbl.Export_Type= 'DBL')";
            SQLString += " AND SE_Job_Master_Tbl.Shipment_Term='" + shipmentType + "'";
            if (shipmentType == "FCL")
            {
                SQLString += " AND SE_Job_Master_Tbl.Job_Number not in";
                SQLString += " (select SOURCE_REF_NUMBER from TPT_JOB_MAIN_Tbl where SOURCE_REF_NUMBER=SE_Job_Master_Tbl.Job_Number)";
            }
            else if (shipmentType == "LCL")
            {
                SQLString += " AND SE_Job_Master_Tbl.Job_Number not in";
                SQLString += " (select SOURCE_REF_NUMBER from TRK_JOB_MAIN_Tbl where SOURCE_REF_NUMBER=SE_Job_Master_Tbl.Job_Number)";
            }
            //20150703 -gerry added to show only those job in specific tranporter
            if (transporterCode != string.Empty)
                SQLString += " AND SE_Job_Detail_Party_Tbl.Transport_Code = '" + transporterCode + "'";
            //20150703 -gerry added
            SQLString += " ORDER BY SE_Job_Master_Tbl.Job_Number";//20161229

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                jobnos.Add((string)reader["Job_Number"]);
            }
            cn.Close();
            return jobnos;
        }

        // 2014-07-02 Zhou Kai modifies this function to inner join SRT_Shipping_Line_Tbl
        // to get the Shipping_Line_Name by Shipping_Line_Code
        internal static SeaExportJob GetSeaExportJob(string jobno)
        {
            SeaExportJob seaexport = new SeaExportJob();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            // 2014-07-01 Zhou Kai adds table SE_JOB_DETAIL_OTHER_Tbl 
            // to this sql query for BookingRefNo field.
            #region 20150803 - gerry replaced
            /*
            string SQLString = "SELECT *,  SRT_Shipping_Line_Tbl.Shipping_Line_Name AS " +
                                         "ShippingLineName " +
                                         "FROM SE_Job_Master_Tbl,SE_Job_Detail_Party_Tbl," + 
                                         "TPT_SPECIAL_DATA_Tbl,SE_Job_Detail_Shipment_Tbl, " +
                                         "SE_JOB_DETAIL_CARGO_Tbl" +
                                         ", SE_Job_Detail_Other_Tbl, SRT_Shipping_Line_Tbl"; // 2014-07-01 Zhou Kai adds
            SQLString += " where SE_Job_Master_Tbl.Export_Number=SE_Job_Detail_Party_Tbl.Export_Number";
            // 2014-07-10 Zhou Kai comments out the one line below:
            // SQLString += " AND SE_Job_Detail_Party_Tbl.Transport_Code = TPT_SPECIAL_DATA_Tbl.TPT_CODE";
            // 2014-07-10 Zhou Kai ends
            SQLString += " AND SE_Job_Master_Tbl.Export_Number=SE_Job_Detail_Shipment_Tbl.Export_Number";
            // 2014-07-01 Zhou Kai adds
            SQLString += " AND SE_Job_Master_Tbl.Export_Number=SE_Job_Detail_Other_Tbl.Export_Number";
            SQLString += " AND SE_JOB_DETAIL_CARGO_Tbl.Export_Number = SE_JOB_MASTER_Tbl.Export_Number";
            // 2014-07-01 Zhou Kai ends
            SQLString += " AND SE_Job_Master_Tbl.Master_Job_Number='" + jobno + "'";
            // 2014-07-05 Zhou Kai comments out
            // SQLString += " AND (SE_Job_Master_Tbl.Export_Type='MBL'";
            // SQLString += " OR SE_Job_Master_Tbl.Export_Type='DBL')";
            // SQLString += " AND SE_Job_Master_Tbl.Shipment_Term='FCL'";
            // 2014-07-05 Zhou Kai ends
            SQLString += " AND SE_Job_Detail_Shipment_Tbl.Shipping_Line_Code = ";
            SQLString += " SRT_Shipping_Line_Tbl.Shipping_Line_Code";
            */
            #endregion
            string SQLString = @"SELECT *,  SRT_Shipping_Line_Tbl.Shipping_Line_Name AS ShippingLineName
                                            FROM SE_Job_Master_Tbl,SE_Job_Detail_Party_Tbl,
                                            TPT_SPECIAL_DATA_Tbl,SE_Job_Detail_Shipment_Tbl, 
                                            SE_JOB_DETAIL_CARGO_Tbl
                                            , SE_Job_Detail_Other_Tbl, SRT_Shipping_Line_Tbl
                                            where SE_Job_Master_Tbl.Export_Number=SE_Job_Detail_Party_Tbl.Export_Number
                                            --AND SE_Job_Detail_Party_Tbl.Transport_Code = TPT_SPECIAL_DATA_Tbl.TPT_CODE
                                            AND SE_Job_Master_Tbl.Export_Number=SE_Job_Detail_Shipment_Tbl.Export_Number
                                            AND SE_Job_Master_Tbl.Export_Number=SE_Job_Detail_Other_Tbl.Export_Number
                                            AND SE_JOB_DETAIL_CARGO_Tbl.Export_Number = SE_JOB_MASTER_Tbl.Export_Number
                                            AND SE_Job_Master_Tbl.Job_Number='{0}'
                                            --AND (SE_Job_Master_Tbl.Export_Type='MBL'
                                            --     OR SE_Job_Master_Tbl.Export_Type='DBL')
                                            --     AND SE_Job_Master_Tbl.Shipment_Term='FCL'
                                            AND SE_Job_Detail_Shipment_Tbl.Shipping_Line_Code = SRT_Shipping_Line_Tbl.Shipping_Line_Code";
            SQLString = string.Format(SQLString, jobno);
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                seaexport = GetSeaExportJob(reader);
            }
            cn.Close();
            return seaexport;
        }

        internal static List<string> GetOutstandingSeaImportJobNos(string transporterCode, string shipmentType)
        {
            string tcode = "";
            tcode = GetTransportCode();

            List<string> jobnos = new List<string>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            //string SQLString = "SELECT DISTINCT SI_Job_Master_Tbl.Master_Job_Number, SI_Job_Master_Tbl.Import_Number ";
            string SQLString = "SELECT DISTINCT SI_Job_Master_Tbl.Job_Number, SI_Job_Master_Tbl.Import_Number ";
            SQLString += " FROM SI_Job_Master_Tbl,SI_Job_Detail_Party_Tbl,SI_Job_Detail_Shipment_Tbl, SI_Job_Detail_Delivery_Tbl";
            SQLString += " where SI_Job_Master_Tbl.Import_Number=SI_Job_Detail_Party_Tbl.Import_Number";
            SQLString += " AND SI_Job_Master_Tbl.Import_Number=SI_Job_Detail_Shipment_Tbl.Import_Number";  
            //20150704 - geery added
            SQLString += " AND SI_Job_Detail_Delivery_Tbl.Import_Number = SI_Job_Master_Tbl.Import_Number"; 
            SQLString += " AND SI_Job_Master_Tbl.Shipment_Term='" + shipmentType + "'";
            if (shipmentType == "FCL")
            {
                SQLString += " AND SI_Job_Master_Tbl.Job_Number not in";
                SQLString += " (select SOURCE_REF_NUMBER from TPT_JOB_MAIN_Tbl where SOURCE_REF_NUMBER=SI_Job_Master_Tbl.Job_Number)";
            }
            else if (shipmentType == "LCL")
            {
                SQLString += " AND SI_Job_Master_Tbl.Job_Number not in";
                SQLString += " (select SOURCE_REF_NUMBER from TRK_JOB_MAIN_Tbl where SOURCE_REF_NUMBER=SI_Job_Master_Tbl.Job_Number)";
            }
            // // 2014-07-05 comments out this condition below
            // SQLString += " AND (SI_Job_Master_Tbl.Import_Type='MAA'";
            // SQLString += " OR SI_Job_Master_Tbl.Import_Type='DOA')";
            // 2014-07-05 Zhou Kai ends

            //20150703 -gerry added to show only those job in specific tranporter
            if (transporterCode != string.Empty)
            {
                SQLString += " AND SI_Job_Detail_Delivery_Tbl.Transporter_Code = '" + transporterCode + "'";
            }
            SQLString += " ORDER BY SI_Job_Master_Tbl.Job_Number";//20161229
            //20150703 -gerry added

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // 2014-07-10 Zhou Kai comments out the if condition below:
                //if (IsTransportCodeInSI_Job_Detail_Delivery_Tbl((int)reader["Import_Number"],tcode) == true)
                //{
                    jobnos.Add((string)reader["Job_Number"]);
                //}
                // 2014-07-10 Zhou Kai ends
            }
            cn.Close();
            return jobnos;
        }

        internal static SeaImportJob GetSeaImportJob(string jobno)
        {
            string tcode = "";
            // tcode = GetTransportCode();

            SeaImportJob sijob = new SeaImportJob();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT *, SRT_Shipping_Line_Tbl.Shipping_Line_Name AS ShippingLineName 
                                    FROM SI_Job_Master_Tbl, SI_Job_Detail_Party_Tbl, SI_Job_Detail_Shipment_Tbl, SRT_Shipping_Line_Tbl, SI_Job_Detail_Cargo_Tbl,SI_Job_Detail_Other_Tbl, SI_Job_Detail_Delivery_Tbl

                                    where SI_Job_Master_Tbl.Import_Number=SI_Job_Detail_Party_Tbl.Import_Number 
                                    AND SI_Job_Master_Tbl.Import_Number=SI_Job_Detail_Shipment_Tbl.Import_Number 
                                    AND SI_Job_Master_Tbl.Job_Number='{0}' 
                                    AND SI_Job_Master_Tbl.Import_Number = SI_Job_Detail_Cargo_Tbl.Import_Number  
                                    AND SI_Job_Master_Tbl.Import_Number = SI_Job_Detail_Delivery_Tbl.Import_Number  
                                    AND SRT_Shipping_Line_Tbl.Shipping_Line_Code = SI_Job_Detail_Shipment_Tbl.Shipping_Line_Code 
                                    AND SI_Job_Detail_Other_Tbl.Import_Number = SI_Job_Master_Tbl.Import_Number";
            SQLString = string.Format(SQLString, jobno);
            #region Gerry replaced the query
            /*
                "SELECT *, SRT_Shipping_Line_Tbl.Shipping_Line_Name AS ShippingLineName " +
                "FROM SI_Job_Master_Tbl, " +
                "SI_Job_Detail_Party_Tbl, " + 
                "SI_Job_Detail_Shipment_Tbl, " +
                "SRT_Shipping_Line_Tbl, " +
                "SI_Job_Detail_Cargo_Tbl," +
                "SI_Job_Detail_Other_Tbl"; 
            SQLString += " where SI_Job_Master_Tbl.Import_Number=SI_Job_Detail_Party_Tbl.Import_Number";
            SQLString += " AND SI_Job_Master_Tbl.Import_Number=SI_Job_Detail_Shipment_Tbl.Import_Number";
            SQLString += " AND SI_Job_Master_Tbl.Master_Job_Number='" + jobno+"'";
            // // 2014-07-05 Zhou Kai comments out this condition below
            // SQLString += " AND (SI_Job_Master_Tbl.Import_Type='MAA'";
            // SQLString += " OR SI_Job_Master_Tbl.Import_Type='DOA')";
            // SQLString += " AND SI_Job_Master_Tbl.Shipment_Term='FCL' ";
            // 2014-07-05 Zhou Kai ends
            SQLString += " AND SI_Job_Master_Tbl.Import_Number = SI_Job_Detail_Cargo_Tbl.Import_Number ";
            SQLString += " AND SRT_Shipping_Line_Tbl.Shipping_Line_Code =" +
                                  " SI_Job_Detail_Shipment_Tbl.Shipping_Line_Code";
            SQLString += " AND SI_Job_Detail_Other_Tbl.Import_Number = SI_Job_Master_Tbl.Import_Number";
            */
            #endregion
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // 2014-07-10 Zhou Kai comments out the if condition
                //if (IsTransportCodeInSI_Job_Detail_Delivery_Tbl((int)reader["Import_Number"], tcode) == true)
                //{
                    sijob = GetSeaImportJob(reader,tcode);
                // }
                // 2014-07-10 Zhou Kai ends
            }
            cn.Close();
            return sijob;
        }

        internal static string GetSeaExportCargoDesc(int export_number)
        {

            string desc = "";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM SE_Job_Detail_Cargo_Tbl";
            SQLString += " WHERE Export_number=" + export_number;

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                desc = (string)reader["Cargo_Desc"];
            }
            cn.Close();
            return desc;
        }

        internal static string GetSeaImportCargoDesc(int import_number)
        {

            string desc = "";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM SI_Job_Detail_Cargo_Tbl";
            SQLString += " WHERE Import_number=" + import_number;

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                desc = (string)reader["Cargo_Desc"];
            }
            cn.Close();
            return desc;
        }

        internal static SortableList<SeaJobContainer> GetSeaImportContainer(int import_number)
        {
            SortableList<SeaJobContainer> sicontainers = new SortableList<SeaJobContainer>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM SI_Job_Detail_Container_Tbl";
            SQLString += " WHERE Import_Number=" + import_number;

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sicontainers.Add(GetSeaImportContainer(reader));
            }
            cn.Close();
            return sicontainers;
        }

        internal static SortableList<SeaJobContainer> GetSeaExportContainer(int export_number)
        {
            SortableList<SeaJobContainer> secontainers = new SortableList<SeaJobContainer>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM SE_Job_Detail_Cargo_Info_Tbl";
            SQLString += " WHERE Export_Number=" + export_number;

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                secontainers.Add(GetSeaExportContainer(reader));
            }
            cn.Close();
            return secontainers;
        } 

        //20170807
        internal static List<JobDimension> GetExportDimension(int export_Number, int seqNo)
        {
            List<JobDimension> retValue = new List<JobDimension>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT *  FROM SE_Job_Detail_Dimension_Tbl dimen
                                        inner join SE_Job_Master_Tbl jm
                                        on jm.Export_Number=dimen.Export_Number
                                        inner join SE_Job_Detail_Cargo_Info_Tbl det
                                        on det.Export_Number=dimen.Export_Number and det.seqNo=dimen.seqNo
                                        where dimen.export_number ={0}
                                        and dimen.SeqNo ={1}";
                SQLString = string.Format(SQLString, export_Number, seqNo);                
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    JobDimension dimen = new JobDimension();
                    dimen.JobID = reader["export_number"] == DBNull.Value ? 0 : Convert.ToInt32(reader["export_number"].ToString());
                    dimen.SeqNo = reader["SeqNo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SeqNo"].ToString());
                    dimen.DimenSeqNo = reader["Sub_Seq_No"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Sub_Seq_No"].ToString());
                    dimen.Qty = reader["PCS"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PCS"].ToString());
                    dimen.Length = reader["Cargo_Length"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Cargo_Length"].ToString());
                    dimen.Width = reader["Cargo_Width"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Cargo_Width"].ToString());
                    dimen.Height = reader["Cargo_Height"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Cargo_Height"].ToString());
                    retValue.Add(dimen);
                }
                cn.Close();
            }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        //20170807
        internal static List<JobDimension> GetImportDimension(int import_Number, int seqNo)
        {
            List<JobDimension> retValue = new List<JobDimension>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT  *  FROM SI_Job_Detail_Dimension_Tbl dimen
                                        inner join SI_Job_Master_Tbl jm
                                        on jm.Import_Number=dimen.Import_Number
                                        inner join SI_Job_Detail_Cargo_Item_Tbl det
                                        on det.import_Number=dimen.Import_Number and det.Cargo_PK=dimen.Container_PK
                                        where dimen.import_number ={0}
                                        and dimen.Container_PK ={1}";
                SQLString = string.Format(SQLString, import_Number, seqNo);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    JobDimension dimen = new JobDimension();
                    dimen.JobID = reader["import_number"] == DBNull.Value ? 0 : Convert.ToInt32(reader["import_number"].ToString());
                    dimen.SeqNo = reader["Container_PK"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Container_PK"].ToString());
                    dimen.DimenSeqNo = reader["Sub_Seq_No"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Sub_Seq_No"].ToString());
                    dimen.Qty = reader["PCS"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PCS"].ToString());
                    dimen.Length = reader["Cargo_Length"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Cargo_Length"].ToString());
                    dimen.Width = reader["Cargo_Width"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Cargo_Width"].ToString());
                    dimen.Height = reader["Cargo_Height"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Cargo_Height"].ToString());
                    retValue.Add(dimen);
                }
                cn.Close();
            }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }


        // returns a collection of ContainerTypesDTO 
        internal static List<ContainerTypesDTO> GetAllContainerTypes()
        {
            List<ContainerTypesDTO> containerTypes = new List<ContainerTypesDTO>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM SRT_Container_Tbl";
            SqlCommand cmd = new SqlCommand(SQLString,cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                containerTypes.Add(GetContainerType(reader));
            }
            cn.Close();

            return containerTypes;
            // 2014-07-04 Zhou Kai moves the one line below to above, 
            // it's unreachable here.
            // cn.Close();
         }
        
        internal static ContainerTypesDTO GetContainerType(string code)
        {
            ContainerTypesDTO containerType = new ContainerTypesDTO();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM SRT_Container_Tbl";
            SQLString += " Where Container_Code='" + code + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                containerType = GetContainerType(reader);
            }
            cn.Close();

            return containerType;
            // 2014-07-04 Zhou Kai moves the one line below to above, 
            // it's unreachable here.
            // cn.Close();
        }
        
        // creates ContainerTypesDTO object from 1 row of the DataReader
         internal static ContainerTypesDTO GetContainerType(IDataReader reader)
         {
             return new ContainerTypesDTO(
                (string) reader["Container_Code"],
                (string) reader["Container_Desc"],
                (string) reader["Container_Size_Code"],
                (decimal) reader["Allowable_Max_Ton"],
                (decimal) reader["Gross_Weight"],
                (decimal) reader["Gross_CBM"],
                (decimal) reader["Length"],
                (decimal) reader["Width"],
                (decimal) reader["Height"]);
         }

         internal static SeaJobContainer GetSeaImportContainer(IDataReader reader)
         {
             SeaJobContainer retValue = new SeaJobContainer(
                (string)reader["Container_Code"],
                (string)reader["Container_Number"],
                (string)reader["Seal_Number"],
                 // 2014-07-14 Zhou Kai adds
                  (decimal)reader["Gross_CBM"],
                //(decimal)reader["Std_CBM"],
                  (decimal)reader["Gross_Weight"]
                //(decimal)reader["Std_Weight"]
                , string.Empty
                , reader["Container_Size"].ToString()
                 // 2014-07-14 Zhou Kai ends
                );
             retValue.JobID = retValue.JobID = reader["import_number"] == DBNull.Value ? 0 : Convert.ToInt32(reader["import_number"].ToString());
             retValue.SeqNo = reader["Container_PK"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Container_PK"].ToString());
             return retValue;
         }

         internal static SeaJobContainer GetSeaExportContainer(IDataReader reader)
         {
             string contcode = "";
             if (reader["Container_Code"] is System.DBNull)
                 contcode = "";
             else
                 contcode = (string)reader["Container_Code"];
             SeaJobContainer retValue = new SeaJobContainer(
                contcode,
                (string)reader["Container_No"],
                (string)reader["Seal_No"],
                // 2014-07-14 Zhou Kai adds
                 (decimal)reader["Gross_CBM"],
                //(decimal)reader["Std_CBM"],
                 (decimal)reader["Gross_Weight"]
                //(decimal)reader["Std_Weight"]
                , reader["Goods_Desc"].ToString() // notice this is [Product_Desc] column instead
                , reader["Container_Size"].ToString()
                // 2014-07-14 Zhou Kai ends
                );
             retValue.JobID = retValue.JobID = reader["export_number"] == DBNull.Value ? 0 : Convert.ToInt32(reader["export_number"].ToString());
             retValue.SeqNo = reader["SeqNo"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SeqNo"].ToString());
             return retValue;
         }

         internal static SeaExportJob GetSeaExportJob(IDataReader reader)
         {
             //string deliveryToCityCode = OperatorDTO.GetOperatorDTO(reader["Delivery_To_Code"].ToString()).City;
             //string emptyContainerToCityCode = OperatorDTO.GetOperatorDTO(reader["Empty_Cont_To_Code"].ToString()).City;
             //string emptyContainerFromCityCode = OperatorDTO.GetOperatorDTO(reader["Empty_Cont_From_Code"].ToString()).City;

             SeaExportJob tempSE = new SeaExportJob(
                (string)reader["Branch_Code"],
                (string)reader["Job_Number"],
                (string)reader["Shipper_Code"],
                (string)reader["Shipper_Name"],
                (string)reader["Shipper_Add1"],
                (string)reader["Shipper_Add2"],
                (string)reader["Shipper_Add3"],
                (string)reader["Shipper_Add4"],
                (string)reader["Consignee_Code"],
                (string)reader["Consignee_Name"],
                (string)reader["Consignee_Add1"],
                (string)reader["Consignee_Add2"],
                (string)reader["Consignee_Add3"],
                (string)reader["Consignee_Add4"],
                (string)reader["OBL_Number"],
                (string)reader["HBL_Number"],
                (string)reader["M_Vessel"],
                reader["Mother_Vessel_Name"].ToString(), // 2014-07-11 Zhou Kai adds
                (string)reader["M_Voyage"],
                (string)reader["POL"],
                reader["POL_Name"].ToString(), // 2014-07-04 Zhou Kai adds
                (string)reader["POD"],
                reader["POD_Name"].ToString(), // 2014-07-04 Zhou Kai adds
                (DateTime)reader["ETA"],
                (DateTime)reader["ETD"],
                // 2014-06-30 Zhou Kai uses .ToString() instead                
                reader["Transport_Code"].ToString(),
                reader["Cargo_Desc"].ToString(), // cargoDescription is empty here, it'll be assigned at the end of this function
                // 2014-07-01 Zhou Kai ends            
                // 2014-06-30 Zhou Kai adds
                reader["Shipping_Line_Code"].ToString(),
                reader["shippingLineName"].ToString(),
                reader["Customer_Invoice_Number"].ToString(), // 2014-07-24 Zhou Kai adds
                new SortableList<SeaJobContainer>(),    
                // 2014-06-30 Zhou Kai ends
                /*2014-09-01 Zhou Kai adds*/
                reader["BILL_TO_CODE"].ToString(),
                reader["BILL_TO_NAME"].ToString()
                // 2014-09-01 Zhou Kai ends
                );
             // 2014-07-17 Zhou Kai modifies below:
             //tempSE.CargoDescription = GetSeaExportCargoDesc((int)reader["Export_Number"]);
             //tempSE.SeaJobContainers = GetSeaExportContainer((int)reader["Export_Number"]);
             //tempSE.CargoDescription = reader["Cargo_Desc"].ToString();
             tempSE.SeaJobContainers = GetSeaExportContainer((int)reader["Export_Number"]);
             // 2014-07-17 Zhou Kai ends


             //20150803 - gerry added
             //pickup place normally from Container yard
             tempSE.PickUpPlaceCode = reader["Empty_Cont_From_Code"] == DBNull.Value ? string.Empty : reader["Empty_Cont_From_Code"].ToString();
             tempSE.PickUpPlaceName = reader["Empty_Cont_From_Name"] == DBNull.Value ? string.Empty : reader["Empty_Cont_From_Name"].ToString();
             tempSE.PickUpPlaceAdd1 = reader["Empty_Cont_From_Add1"] == DBNull.Value ? string.Empty : reader["Empty_Cont_From_Add1"].ToString();
             tempSE.PickUpPlaceAdd2 = reader["Empty_Cont_From_Add2"] == DBNull.Value ? string.Empty : reader["Empty_Cont_From_Add2"].ToString();
             tempSE.PickUpPlaceAdd3 = reader["Empty_Cont_From_Add3"] == DBNull.Value ? string.Empty : reader["Empty_Cont_From_Add3"].ToString();
             tempSE.PickUpPlaceAdd4 = reader["Empty_Cont_From_Add4"] == DBNull.Value ? string.Empty : reader["Empty_Cont_From_Add4"].ToString();
             tempSE.PickUpPlaceCityCode = OperatorDTO.GetOperatorDTO(reader["Empty_Cont_From_Code"].ToString()).City;
             //stuffing which customer to go from container yard
             tempSE.StuffingCode = reader["Empty_Cont_To_Code"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Code"].ToString();
             tempSE.StuffingName = reader["Empty_Cont_To_Name1"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Name1"].ToString();
             tempSE.StuffingAdd1 = reader["Empty_Cont_To_Add11"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Add11"].ToString();
             tempSE.StuffingAdd2 = reader["Empty_Cont_To_Add21"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Add21"].ToString();
             tempSE.StuffingAdd3 = reader["Empty_Cont_To_Add31"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Add31"].ToString();
             tempSE.StuffingAdd4 = reader["Empty_Cont_To_Add41"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Add41"].ToString();
             tempSE.StuffingCityCode = OperatorDTO.GetOperatorDTO(reader["Empty_Cont_To_Code"].ToString()).City;
             //delivery place from customer place normally to port
             tempSE.DeliveryCode = reader["Delivery_To_Code"] == DBNull.Value ? string.Empty : reader["Delivery_To_Code"].ToString();
             tempSE.DeliveryName = reader["Delivery_To_Name"] == DBNull.Value ? string.Empty : reader["Delivery_To_Name"].ToString();
             tempSE.DeliveryAdd1 = reader["Delivery_To_Add1"] == DBNull.Value ? string.Empty : reader["Delivery_To_Add1"].ToString();
             tempSE.DeliveryAdd2 = reader["Delivery_To_Add2"] == DBNull.Value ? string.Empty : reader["Delivery_To_Add2"].ToString();
             tempSE.DeliveryAdd3 = reader["Delivery_To_Add3"] == DBNull.Value ? string.Empty : reader["Delivery_To_Add3"].ToString();
             tempSE.DeliveryAdd4 = reader["Delivery_To_Add4"] == DBNull.Value ? string.Empty : reader["Delivery_To_Add4"].ToString();
             tempSE.DeliveryCityCode = OperatorDTO.GetOperatorDTO(reader["Delivery_To_Code"].ToString()).City;
             tempSE.BookingRefNo = reader["Booking_Ref_Number"] == DBNull.Value ? string.Empty : reader["Booking_Ref_Number"].ToString();

             tempSE.JobCreatorId = reader["Added_By"] == DBNull.Value ? string.Empty : reader["Added_By"].ToString();
             tempSE.JobCreatedDateTime = reader["Added_DateTime"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["Added_DateTime"];

             tempSE.Shipment_Type = reader["Shipment_Term"] == DBNull.Value ? string.Empty : reader["Shipment_Term"].ToString();//20170805

             return tempSE;
         }

         internal static SeaImportJob GetSeaImportJob(IDataReader reader,string transport_code)
         {
             // 2014-07-09 Zhou Kai adds
             string pol_name = OperatorDTO.GetOperatorDTO(reader["POL"].ToString()).Name;
             string pod_name = OperatorDTO.GetOperatorDTO(reader["POD"].ToString()).Name;
             // 2014-07-09 Zhou Kai ends

             //string pickUpPlaceCityCode = OperatorDTO.GetOperatorDTO(reader["Pickup_Place_Code"].ToString()).City;
             //string emptyContainerToCityCode = OperatorDTO.GetOperatorDTO(reader["Empty_Cont_To_Code"].ToString()).City;
             SeaImportJob tempSI = new SeaImportJob(
                (string)reader["Branch_Code"],
                (string)reader["Job_Number"],
                (string)reader["Shipper_Code"],
                (string)reader["Shipper_Name"],
                (string)reader["Shipper_Add1"],
                (string)reader["Shipper_Add2"],
                (string)reader["Shipper_Add3"],
                (string)reader["Shipper_Add4"],
                (string)reader["Consignee_Code"],
                (string)reader["Consignee_Name"],
                (string)reader["Consignee_Add1"],
                (string)reader["Consignee_Add2"],
                (string)reader["Consignee_Add3"],
                (string)reader["Consignee_Add4"],
                (string)reader["OBL_Number"],
                (string)reader["HBL_Number"],
                (string)reader["M_Vessel"],
                reader["Mother_Vessel_Name"].ToString(), // 2014-07-11 Zhou Kai adds
                (string)reader["M_Voyage"],
                (string)reader["POL"],
                pol_name,// reader["POL_Name"].ToString(), // // 2014-07-04 Zhou Kai adds
                (string)reader["POD"],
                pod_name, // reader["POD_Name"].ToString(), // 2014-07-04 Zhou Kai adds
                (DateTime)reader["ETA"],
                (DateTime)reader["ETD"],
                 // the emptyContainerTo for SI should be a singapore container yard
             #region "2014-07-01 Zhou Kai adds codes"
             #endregion
             #region "the orignal codes, set all to blank"
                 //String.Empty, // emptyContainerToCode
                //String.Empty, // emptyContainerToName
                //String.Empty, // emptyContainerToAdd1
                //String.Empty, // emptyContainerToAdd2
                //String.Empty, // emptyContainerToAdd3
                //String.Empty, // emptyContainerToAdd4
                //String.Empty, // shippingLine
                 //String.Empty, // transportCode
             #endregion
                transport_code,
                reader["Cargo_Desc"].ToString(), 
                reader["Shipping_Line_Code"].ToString(),
                reader["ShippingLineName"].ToString(),
                reader["Customer_Invoice_Number"] == DBNull.Value ? string.Empty : reader["Customer_Invoice_Number"].ToString(),
                new SortableList<SeaJobContainer>(),
                 // 2014-07-24 Zhou Kai adds
                 /*2014-09-01 Zhou Kai adds*/
                reader["BILL_TO_CODE"].ToString(),
                reader["BILL_TO_NAME"].ToString()
                 // 2014-09-01 Zhou Kai ends
                );

             // 2014-07-17 Zhou Kai comments below:
             // tempSI.CargoDescription=GetSeaImportCargoDesc((int)reader["Import_Number"]);
             // GetSI_Job_Detail_Delivery_Tbl(tempSI, (int)reader["Import_Number"], transport_code);
             // 2014-07-17 Zhou Kai ends
             tempSI.SeaJobContainers = GetSeaImportContainer((int)reader["Import_Number"]);
    
             //20150803 - gerry added
             //pickup place normally from Port
             tempSI.PickUpPlaceCode = reader["Pickup_Place_Code"] == DBNull.Value ? string.Empty : reader["Pickup_Place_Code"].ToString();
             tempSI.PickUpPlaceName = reader["Pickup_Place_Name"] == DBNull.Value ? string.Empty : reader["Pickup_Place_Name"].ToString();
             tempSI.PickUpPlaceAdd1 = reader["Pickup_Place_Add1"] == DBNull.Value ? string.Empty : reader["Pickup_Place_Add1"].ToString();
             tempSI.PickUpPlaceAdd2 = reader["Pickup_Place_Add2"] == DBNull.Value ? string.Empty : reader["Pickup_Place_Add2"].ToString();
             tempSI.PickUpPlaceAdd3 = reader["Pickup_Place_Add3"] == DBNull.Value ? string.Empty : reader["Pickup_Place_Add3"].ToString();
             tempSI.PickUpPlaceAdd4 = reader["Pickup_Place_Add4"] == DBNull.Value ? string.Empty : reader["Pickup_Place_Add4"].ToString();
             tempSI.PickUpPlaceCityCode = OperatorDTO.GetOperatorDTO(reader["Pickup_Place_Code"].ToString()).City;
             //stuffing which customer to go from port
             tempSI.StuffingCode = reader["Delivery_To_Code"] == DBNull.Value ? string.Empty : reader["Delivery_To_Code"].ToString();
             tempSI.StuffingName = reader["Delivery_To_Name"] == DBNull.Value ? string.Empty : reader["Delivery_To_Name"].ToString();
             tempSI.StuffingAdd1 = reader["Delivery_To_Add1"] == DBNull.Value ? string.Empty : reader["Delivery_To_Add1"].ToString();
             tempSI.StuffingAdd2 = reader["Delivery_To_Add2"] == DBNull.Value ? string.Empty : reader["Delivery_To_Add2"].ToString();
             tempSI.StuffingAdd3 = reader["Delivery_To_Add3"] == DBNull.Value ? string.Empty : reader["Delivery_To_Add3"].ToString();
             tempSI.StuffingAdd4 = reader["Delivery_To_Add4"] == DBNull.Value ? string.Empty : reader["Delivery_To_Add4"].ToString();
             tempSI.StuffingCityCode = OperatorDTO.GetOperatorDTO(reader["Delivery_To_Code"].ToString()).City;
             //delivery place from customer normally to container yard
             tempSI.DeliveryCode = reader["Empty_Cont_To_Code"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Code"].ToString();
             tempSI.DeliveryName = reader["Empty_Cont_To_Name"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Name"].ToString();
             tempSI.DeliveryAdd1 = reader["Empty_Cont_To_Add1"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Add1"].ToString();
             tempSI.DeliveryAdd2 = reader["Empty_Cont_To_Add2"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Add2"].ToString();
             tempSI.DeliveryAdd3 = reader["Empty_Cont_To_Add3"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Add3"].ToString();
             tempSI.DeliveryAdd4 = reader["Empty_Cont_To_Add4"] == DBNull.Value ? string.Empty : reader["Empty_Cont_To_Add4"].ToString();
             tempSI.DeliveryCityCode = OperatorDTO.GetOperatorDTO(reader["Empty_Cont_To_Code"].ToString()).City;
             tempSI.BookingRefNo = reader["Booking_Ref_Number"] == DBNull.Value ? string.Empty : reader["Booking_Ref_Number"].ToString();

             tempSI.JobCreatorId = reader["Added_By"] == DBNull.Value ? string.Empty : reader["Added_By"].ToString();
             tempSI.JobCreatedDateTime = reader["Added_DateTime"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["Added_DateTime"];
         
             return tempSI;
         }

        //-----16 Aug 2011 - Gerry Added

         // 2014-07-30 Zhou Kai adds parameter noOfLegs to this function
         internal static List<string> GetContainerSizesFromTransportRates(int quotationID, bool stopDependent, int noOfLegs)
         {
             SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
             List<string> sizesList = new List<string>();
             string temp = "";
             if (stopDependent)
                 temp = "T";
             else
                 temp = "F";
             string where = @"select uom from dbo.TPT_MKT_QUOTATION_DETAIL_RATE_TBL 
                                            where QUOTATION_ID = '{0}' AND IS_CONTAINER_MOVEMENT ='T' 
                                                AND IS_STOP_DEPENDENT = '{1}' ";
             // 2014-07-30 Zhou Kai adds
             where += " AND [NoOfLeg] = {2}";
             // 2014-07-30 Zhou Kai ends
             where = string.Format(where, quotationID, temp, noOfLegs);
             SqlCommand cmd1 = new SqlCommand(where, cn);
             if (cn.State == ConnectionState.Closed) { cn.Open(); }
             IDataReader reader1 = cmd1.ExecuteReader();
             while (reader1.Read())
             {
                 string size = reader1.GetString(0);                  
                 sizesList.Add(CommonUtilities.FormatString(size));
             }
             reader1.Close();
             cn.Close();

             return sizesList;
         }
        
        // 2014-07-30 Zhou Kai adds the parameter noOfLegs
         internal static List<string> GetContainerCodesBySizeUsedInQuotationAndJobTrips(int quotationID, bool stopDependent, int noOfLegs)
         {
             List<string> containerCodes = new List<string>();
             List<string> sizesList = new List<string>();
             sizesList = GetContainerSizesFromTransportRates(quotationID, stopDependent, noOfLegs);
             string sizes = string.Join("','", sizesList.ToArray());
             
             SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
             string SQLString = @"SELECT Container_Code FROM SRT_Container_Tbl 
                                    Where Container_Size_Code in ('{0}')";

             SQLString = string.Format(SQLString, sizes);
             SqlCommand cmd = new SqlCommand(SQLString, cn);
             if (cn.State == ConnectionState.Closed) { cn.Open(); }
             IDataReader reader = cmd.ExecuteReader();
             while (reader.Read())
             {
                 containerCodes.Add(reader.GetString(0));
             }
             reader.Close();
             cn.Close();

             return containerCodes;
         }
       
         internal static List<string> GetContainerSizes()
         {
             SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
             List<string> sizesList = new List<string>();

             string SQLString = @"SELECT distinct Container_Size_Code FROM SRT_Container_Tbl ";

             //SQLString = string.Format(SQLString, SQLString);
             SqlCommand cmd = new SqlCommand(SQLString, cn);
             if (cn.State == ConnectionState.Closed) { cn.Open(); }
             IDataReader reader = cmd.ExecuteReader();
             while (reader.Read())
             {
                 sizesList.Add(reader.GetString(0));
             }
             reader.Close();
             cn.Close();

             return sizesList;
         }

         internal static List<string> GetAllContainerCodes()
         {
             List<string> containerTypes = new List<string>();
             SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
             string SQLString = "SELECT distinct Container_Code FROM SRT_Container_Tbl";
             SqlCommand cmd = new SqlCommand(SQLString, cn);
             cn.Open();
             IDataReader reader = cmd.ExecuteReader();
             while (reader.Read())
             {
                 containerTypes.Add(reader.GetString(0));
             }
             cn.Close();
             return containerTypes;
         }

         // 2014-07-30 Zhou Kai adds parameter noOfLegs to this function
         //20140124 - gerry added
         internal static List<ContainerTypesDTO> GetContainerDTOBySizeUsedInQuotationAndJobTrips(int quotationID, bool stopDependent, int noOfLegs)
         {
             List<ContainerTypesDTO> retValue = new List<ContainerTypesDTO>();
             List<string> sizesList = new List<string>();
             sizesList = GetContainerSizesFromTransportRates(quotationID, stopDependent, noOfLegs);
             string sizes = string.Join("','", sizesList.ToArray());

             SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
             string SQLString = @"SELECT * FROM SRT_Container_Tbl 
                                    Where Container_Size_Code in ('{0}')";

             SQLString = string.Format(SQLString, sizes);
             SqlCommand cmd = new SqlCommand(SQLString, cn);
             if (cn.State == ConnectionState.Closed) { cn.Open(); }
             IDataReader reader = cmd.ExecuteReader();
             while (reader.Read())
             {
                 retValue.Add(GetContainerType(reader));
             }
             reader.Close();
             cn.Close();

             return retValue;
         }

        #region "2014-06-23 Zhou Kai adds Get functions"
         internal static SortableList<Port> GetAllPorts()
         {
             SortableList<Port> allPorts = new SortableList<Port>();
             allPorts.Add(new Port()); // add an empty item
             string strSqlQuery = "SELECT [Port_Code], [Port_Name] FROM CRT_Port_Tbl";
             using (SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
             {
                 SqlCommand dbCmd = new SqlCommand(strSqlQuery, dbCon);
                 try
                 {
                     if (dbCon.State == ConnectionState.Closed) {dbCon.Open();}
                     SqlDataReader dbReader = dbCmd.ExecuteReader();
                     while (dbReader.Read())
                     {
                         Port port = new Port(dbReader["Port_Code"].ToString(), dbReader["Port_Name"].ToString());
                         allPorts.Add(port);
                     }
                 }
                 catch (FMException fmEx) { throw fmEx; }
                 catch (SqlException ex) { throw new FMException(ex.ToString()); }
                 catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                 catch (Exception ex) { throw new FMException(ex.ToString()); }
             }

             return allPorts;
         }

         internal static SortableList<Vessel> GetAllVessels()
         {
             SortableList<Vessel> allVessels = new SortableList<Vessel>();
             allVessels.Add(new Vessel()); // add an empty item
             string strSqlQuery = "SELECT [Vessel_Alpha], [Vessel_Name] FROM SRT_Vessel_Master_Tbl";
             using (SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
             {
                 SqlCommand dbCmd = new SqlCommand(strSqlQuery, dbCon);
                 try
                 {
                     if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                     SqlDataReader dbReader = dbCmd.ExecuteReader();
                     while (dbReader.Read())
                     {
                         Vessel vessel = new Vessel(dbReader["Vessel_Alpha"].ToString(), dbReader["Vessel_Name"].ToString());
                         allVessels.Add(vessel);
                     }
                 }
                 catch (FMException fmEx) { throw fmEx; }
                 catch (SqlException ex) { throw new FMException(ex.ToString()); }
                 catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                 catch (Exception ex) { throw new FMException(ex.ToString()); }
             }

             return allVessels;
         }

         internal static SortableList<Shipping> GetAllShippings()
         {
             SortableList<Shipping> allShippings = new SortableList<Shipping>();
             allShippings.Add(new Shipping()); // add an empty item
             string strSqlQuery = "SELECT [Shipping_Line_Code], [Shipping_Line_Name] FROM SRT_Shipping_Line_Tbl";
             using (SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
             {
                 SqlCommand dbCmd = new SqlCommand(strSqlQuery, dbCon);
                 try
                 {
                     if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                     SqlDataReader dbReader = dbCmd.ExecuteReader();
                     while (dbReader.Read())
                     {
                         Shipping shipping = new Shipping(dbReader["Shipping_Line_Code"].ToString(), dbReader["Shipping_Line_Name"].ToString());
                         allShippings.Add(shipping);
                     }
                 }
                 catch (FMException fmEx) { throw fmEx; }
                 catch (SqlException ex) { throw new FMException(ex.ToString()); }
                 catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                 catch (Exception ex) { throw new FMException(ex.ToString()); }
             }

             return allShippings;
         }

        #endregion

         /// <summary>
         /// 2014-08-06 Zhou Kai adds this function to get container size
         /// from container code
         /// </summary>
         /// <param name="code">The container code</param>
         /// <returns>The container size</returns>
         internal static string GetContainerSizeFromContainerCode(string code)
         {
             string rtn = String.Empty;
             string sqlQuery = "SELECT [Container_Size_Code] FROM " +
                 "[dbo].[SRT_Container_Tbl] WHERE [dbo].[SRT_Container_Tbl].[Container_Code] "+
                 " = '{0}';";
             sqlQuery = String.Format(sqlQuery, code);
             using (SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
             using (SqlCommand dbCmd = new SqlCommand())
             {
                 try
                 {
                     if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                     dbCmd.Connection = dbCon;
                     dbCmd.CommandType = CommandType.Text;
                     dbCmd.CommandText = sqlQuery;
                     rtn = dbCmd.ExecuteScalar().ToString();

                     if (rtn.Equals(String.Empty)) { throw new FMException("Container size for this container code not defined."); }
                 }
                 catch (FMException fmEx) { throw fmEx; }
                 catch (SqlException ex) { throw new FMException(ex.ToString()); }
                 catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                 catch (Exception ex) { throw new FMException(ex.ToString()); }
                 
             }

             return rtn;
         }

        /// <summary>
        /// 2014-08-13 Zhou Kai adds this function to initialize the container code to 
        /// size mapping from SRT_Container_Tbl
        /// </summary>
        /// <returns>The initialized dictionary object, the key is the container code
        /// and the value is the container size</returns>
         internal static Dictionary<string, string> GetContainerCodeSizeMapping()
         {
             Dictionary<string, string> tmp = new Dictionary<string, string>();
             string sqlQuery = 
                 "SELECT [CONTAINER_CODE], [CONTAINER_SIZE_CODE]"
                 + " FROM [SRT_Container_Tbl]";
             using (SqlConnection dbCon = new
              SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
             using (SqlCommand dbCmd = new SqlCommand(sqlQuery, dbCon))
             {
                 try
                 {
                     if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                     SqlDataReader dbRdr = dbCmd.ExecuteReader();
                     while (dbRdr.Read())
                     {
                         tmp.Add(dbRdr["CONTAINER_CODE"].ToString(), 
                             dbRdr["CONTAINER_SIZE_CODE"].ToString());
                     }

                     if (tmp.Count == 0)
                     { 
                         throw new FMException
                             ("No container code / size, please check database."); 
                     }
                 }
                 catch (FMException fmEx) { throw fmEx; }
                 catch (SqlException ex) { throw new FMException(ex.ToString()); }
                 catch (InvalidOperationException ex) 
                 { throw new FMException(ex.ToString()); }
                 catch (Exception ex) { throw new FMException(ex.ToString()); }
             }
             tmp.Add(String.Empty, String.Empty); // map blank to blank

             return tmp;
         }

    }
}