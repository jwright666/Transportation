using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using System.Collections;
using FM.TR_MarketDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_MarketDLL.DAL
{
    internal class ChargeDAL
    {
        internal static string GetTaxCode(string ChargeCode)
        {    
            string tax_code = "";
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT Tax_Code FROM ACT_Sales_Cost_Item_Tbl ";
                SQLString += "WHERE Sales_Cost_Item_Code = '" + ChargeCode + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tax_code = (string)reader["Tax_Code"];
                }
                cn.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return tax_code;
        }

        internal static string GetUOM(string ChargeCode)
        {
            string uom = "";
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT UOM FROM TPT_CHARGE_TBL ";
                SQLString += "WHERE CHARGE_CODE = '" + ChargeCode + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    uom = (string)reader["UOM"];
                }
                cn.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return uom;
        }

        internal static SortableList<Charge> GetAllCharges(string branchCode, string custVendType)
        {
            SortableList<Charge> Charges = new SortableList<Charge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_CHARGE_TBL ";

                if (branchCode != "" && custVendType == "")
                    SQLString += " WHERE BRANCH_CODE ='" + branchCode + "' ";
                else if (branchCode != "" && custVendType == "")
                    SQLString += " WHERE CUST_VEND_TYPE_CODE ='" + custVendType + "' ";
                else if (branchCode != "" && custVendType != "")
                {
                    SQLString += " WHERE BRANCH_CODE ='" + branchCode + "' ";
                    SQLString += " AND CUST_VEND_TYPE_CODE ='" + custVendType + "' ";
                }

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charges.Add(GetCharge(reader));
                }
                cn.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return Charges;
        }

        internal static SortableList<Charge> GetAllChargesFromSalesCostItem()
        {                                                   
            SortableList<Charge> Charges = new SortableList<Charge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM ACT_Sales_Cost_Item_Tbl ";
                SQLString += "Where TR_Y_N ='T'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charges.Add(GetChargeFromSalesCostItem(reader));
                }
                cn.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return Charges;
        }

        internal static SortableList<Charge> GetAllChargesFromSalesCostItem(string branch, string custvendtype)
        {
            SortableList<Charge> Charges = new SortableList<Charge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM ACT_Sales_Cost_Item_Tbl ";
                SQLString += "Where TR_Y_N ='T' ";
                SQLString += "AND Branch_Code ='" + branch + "' ";
                SQLString += "AND CustVend_Type_Code ='" + custvendtype + "' ";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charges.Add(GetChargeFromSalesCostItem(reader));
                }
                cn.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return Charges;
        }

        internal static Charge GetCharge(string chargeCode, string branchCode, string custVend_Type)
        {
            Charge charge = new Charge();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_CHARGE_TBL ";
                SQLString += "WHERE CHARGE_CODE = '" + chargeCode + "'";
                SQLString += " AND BRANCH_CODE = '" + branchCode + "'";
                SQLString += " AND CUST_VEND_TYPE_CODE = '" + custVend_Type + "'";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    charge = GetCharge(reader);
                }
                cn.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return charge;
        }

        internal static SortableList<Charge> GetAllChargesWhichNotContainerMovement()
        {
            SortableList<Charge> Charges = new SortableList<Charge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_CHARGE_TBL ";
                SQLString += "WHERE IS_CONTAINER_MOVEMENT = 'F'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charges.Add(GetCharge(reader));
                }
                cn.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return Charges;
        }


        // creates OperatorDTO object from 1 row of the DataReader
        internal static Charge GetChargeFromSalesCostItem(IDataReader reader)
        {                
            string taxcode = (string)reader["Tax_Code"];    
            string uom;
            if (reader["UOM"] is System.DBNull)
            {
                uom = "";
            }
            else
            {
                uom = (string)reader["UOM"];
            }

            Charge retValue = new Charge(
               reader["Sales_Cost_Item_Code"] == DBNull.Value ? string.Empty : (string)reader["Sales_Cost_Item_Code"],
               reader["Item_Description"] == DBNull.Value ? string.Empty : (string)reader["Item_Description"],
               uom,
               reader["Sales_Accnt_Code"] == DBNull.Value ? string.Empty : (string)reader["Sales_Accnt_Code"],
               false,
               "",
               false,
               false,
               taxcode,
               reader["Branch_Code"] == DBNull.Value ? string.Empty : (string)reader["Branch_Code"],
               reader["CustVend_Type_Code"] == DBNull.Value ? string.Empty : (string)reader["CustVend_Type_Code"]
               );         
            retValue.validUoms = new SortableList<Uom>();//20130402 Gerry added  
            //retValue.IsPAckageDependent = (int)reader["IS_PACKAGE_DEPENDENT"] == 1 ? true : false; //20130827 Gerry added 
            //retValue.PackType = reader["PACK_TYPE"] == DBNull.Value ? string.Empty : (string)reader["PACK_TYPE"];  //20130827 Gerry added 

            
            return retValue;
        }


        internal static Charge GetCharge(IDataReader reader)
        {
            #region 20130402 Gerry removed and replaced
            /*
            bool iscontainermovement = true;
            string temp = "";
            temp = (string)reader["IS_CONTAINER_MOVEMENT"];
            if (temp == "T") iscontainermovement = true;
            else iscontainermovement = false;

            bool isoverweight = true;
            temp = (string)reader["IS_OVERWEIGHT"];
            if (temp == "T") isoverweight = true;
            else isoverweight = false;

            bool istruckmovement = true;
            temp = (string)reader["IS_TRUCK_MOVEMENT"];
            if (temp == "T") istruckmovement = true;
            else istruckmovement = false;

            string taxcode = (string)reader["TAX_CODE"];

            return new Charge(
               (string)reader["CHARGE_CODE"],
               (string)reader["CHARGE_DESCRIPTION"],
               (string)reader["UOM"],
               (string)reader["SALES_ACCNT_CODE"],
               iscontainermovement,
               (string)reader["CONTAINERCODE"],
               isoverweight,
               istruckmovement,
               taxcode,
               (string)reader["BRANCH_CODE"],
               (string)reader["CUST_VEND_TYPE_CODE"]);
            */
            #endregion
            string chargeCode = reader["CHARGE_CODE"] == DBNull.Value ? string.Empty : (string)reader["CHARGE_CODE"];
            string custVendType = reader["Cust_Vend_Type_Code"] == DBNull.Value ? string.Empty : (string)reader["Cust_Vend_Type_Code"];
            Charge retValue = new Charge(
                       chargeCode,
                       reader["CHARGE_DESCRIPTION"] == DBNull.Value ? string.Empty : (string)reader["CHARGE_DESCRIPTION"],
                       reader["UOM"] == DBNull.Value ? string.Empty : (string)reader["UOM"],
                       reader["SALES_ACCNT_CODE"] == DBNull.Value ? string.Empty : (string)reader["SALES_ACCNT_CODE"],
                      (reader["IS_CONTAINER_MOVEMENT"] == DBNull.Value ? string.Empty : (string)reader["IS_CONTAINER_MOVEMENT"]) == "T" ? true : false,
                       reader["CONTAINERCODE"] == DBNull.Value ? string.Empty : (string)reader["CONTAINERCODE"],
                      (reader["IS_OVERWEIGHT"] == DBNull.Value ? string.Empty : (string)reader["IS_OVERWEIGHT"]) == "T" ? true : false,
                      (reader["IS_TRUCK_MOVEMENT"] == DBNull.Value ? string.Empty : (string)reader["IS_TRUCK_MOVEMENT"]) == "T" ? true : false,
                       reader["Tax_Code"] == DBNull.Value ? string.Empty : (string)reader["Tax_Code"],
                       reader["Branch_Code"] == DBNull.Value ? string.Empty : (string)reader["Branch_Code"],
                       custVendType
                        );
            //retValue.validUoms = Uom.GetValidTRUomsByChargeCode(chargeCode);retValue
            //20130603 - gerry modify to pass the charge object
            retValue.validUoms = Uom.GetValidTRUomsByChargeCode(retValue);
            retValue.packType = reader["PACK_TYPE"] == DBNull.Value ? string.Empty : (string)reader["PACK_TYPE"];
            int tempChargeType = reader["INVOICE_CHARGE_TYPE"] == DBNull.Value ? 1 : (int)reader["INVOICE_CHARGE_TYPE"];
            switch (tempChargeType)
            { 
                case 2:
                    retValue.invoiceChargeType = InvoiceChargeType.DependOnWeightVolume;
                    break;
                case 3:
                    retValue.invoiceChargeType = InvoiceChargeType.DependOnTruckCapacity;
                    break;
                case 4:
                    retValue.invoiceChargeType = InvoiceChargeType.DependOnPackagetype;
                    break;
                case 5:
                    retValue.invoiceChargeType = InvoiceChargeType.DependOnHigherWeightOrVolume;
                    break;
                default: 
                    retValue.invoiceChargeType = InvoiceChargeType.Others;
                    break;
            }


            return retValue;
        }

        internal static bool AddCharge(Charge charge, SqlConnection cn, SqlTransaction tran)
        {
            bool temp = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
           
            try
            {    
                con.Open();
                string uom = charge.UOM.ToString().Trim();                
                //if (uom.Contains("'"))
                //{
                //    int index = uom.IndexOf("'");
                //    uom = uom.Insert(index, "'");
                //}

                #region SQL query
                string SQLInstertString = @"INSERT INTO TPT_CHARGE_TBL(   
                                                CHARGE_CODE,
                                                CHARGE_DESCRIPTION,
                                                SALES_ACCNT_CODE,
                                                UOM,
                                                IS_CONTAINER_MOVEMENT,
                                                CONTAINERCODE,
                                                IS_OVERWEIGHT,
                                                IS_TRUCK_MOVEMENT,
                                                TAX_CODE,
                                                BRANCH_CODE,
                                                CUST_VEND_TYPE_CODE,
                                                PACK_TYPE,
                                                INVOICE_CHARGE_TYPE)   
                                            VALUES (
                                                '{0}',
                                                '{1}',
                                                '{2}',
                                                '{3}',
                                                '{4}',
                                                '{5}',
                                                '{6}',
                                                '{7}',
                                                '{8}',
                                                '{9}',
                                                '{10}',
                                                '{11}',
                                                '{12}')
                                                ";
                #endregion
                #region SQL Parameters
                SQLInstertString = string.Format(SQLInstertString,
                                        CommonUtilities.FormatString(charge.ChargeCode.Trim()),
                                        CommonUtilities.FormatString(charge.ChargeDescription),
                                        charge.SalesAccountCode,
                                        CommonUtilities.FormatString(uom),
                                        (charge.IsContainerMovement?"T":"F"),
                                        CommonUtilities.FormatString(charge.ContainerCode),
                                        (charge.IsOverweight?"T":"F"),
                                        (charge.IsTruckMovement?"T":"F"),
                                        charge.TaxCode,
                                        charge.BranchCode,
                                        charge.CustVendorType,
                                        charge.packType,
                                        charge.invoiceChargeType.GetHashCode());
                #endregion
                
                //Removed
                #region Gerry Removed
                /*
                if (charge.UOM == "20'")
                {
                    SQLInstertString += "'" + "," + "'20''";
                }
                else
                {
                    if (charge.UOM == "40'")
                    {
                        SQLInstertString += "'" + "," + "'40''";
                    }
                    else
                    {
                        if (charge.UOM == "40'H")
                        {
                            SQLInstertString += "'" + "," + "'40''H";
                        }
                        else
                        {
                            SQLInstertString += "'" + "," + "'" + charge.UOM;
                        }
                    }
                }
                
                if (charge.IsOverweight == true)
                {
                    SQLInstertString += "'" + "," + "'T";
                }
                else
                {
                    SQLInstertString += "'" + "," + "'F";
                }
                if (charge.IsTruckMovement == true)
                {
                    SQLInstertString += "'" + "," + "'T";
                }
                else
                {
                    SQLInstertString += "'" + "," + "'F";
                }
                 * */
                #endregion                        
               
                SqlCommand cmd = new SqlCommand(SQLInstertString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }

            return temp;
        }

        internal static bool EditCharge(Charge charge, SqlConnection cn, SqlTransaction tran)
        {
            bool temp = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            try
            {
                string SQLUpdateDriverString = " Update TPT_CHARGE_TBL set " +
                                               " CHARGE_DESCRIPTION ='" + CommonUtilities.FormatString(charge.ChargeDescription) + "'," +
                                               " PACK_TYPE ='" + charge.packType + "'," +
                                               " INVOICE_CHARGE_TYPE ='" + charge.invoiceChargeType.GetHashCode() + "'," +
                                               " UOM ='" + CommonUtilities.FormatString(charge.UOM) + "'" +
                                               " Where CHARGE_CODE = '" + charge.ChargeCode + "'" +
                                               " AND BRANCH_CODE = '" + charge.BranchCode + "'" +
                                               " AND CUST_VEND_TYPE_CODE = '" + charge.CustVendorType + "'"; 

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return temp;
        }

        internal static bool DeleteCharge(Charge charge, SqlConnection cn, SqlTransaction tran)
        {
            bool temp = true;
            try
            {
                if (!IsChargeUsed(charge))
                {
                    SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    con.Open();
                    string SQLDeleteChargeString = " Delete From TPT_CHARGE_TBL " +
                                                   " Where CHARGE_CODE = '" + charge.ChargeCode + "'" +
                                                   " AND BRANCH_CODE = '" + charge.BranchCode + "'" +
                                                   " AND CUST_VEND_TYPE_CODE = '" + charge.CustVendorType + "'";

                    SqlCommand cmd = new SqlCommand(SQLDeleteChargeString, cn);
                    cmd.Transaction = tran;
                    cmd.ExecuteNonQuery();

                }
                else
                {
                    throw new FMException("Fail to delete in DeleteCharge");
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }     
            return temp;
        }

        internal static bool IsChargeUsed(Charge charge)
        {
            bool temp = true;
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLString = "select CustVend_Type_Code,BRANCH_CODE,CHARGE_CODE from TPT_JOB_DETAIL_CHARGE_TBL,TPT_JOB_MAIN_TBL,ACT_CustVend_Master_Tbl ";
                SQLString += "Where TPT_JOB_MAIN_TBL.CUST_CODE = ACT_CustVend_Master_Tbl.CustVend_Code ";
                SQLString += "AND TPT_JOB_DETAIL_CHARGE_TBL.JOB_ID = TPT_JOB_MAIN_TBL.JOB_ID ";
                SQLString += "AND CustVend_Type_Code='" + charge.CustVendorType + "' ";
                SQLString += "AND CHARGE_CODE='" + charge.ChargeCode + "' ";
                SQLString += "AND BRANCH_CODE='" + charge.BranchCode + "' ";

                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
                DataSet dsSearchResult = new DataSet();
                daSearchCmd.Fill(dsSearchResult);
                cn.Close();
                if (dsSearchResult.Tables[0].Rows.Count <= 0)
                {
                    cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    cn.Open();
                    SQLString = "select * from ACT_IV_Transport_Invoice_Item_Tbl,ACT_IV_Transport_Invoice_Master_Tbl,ACT_CustVend_Master_Tbl ";
                    SQLString += "Where ACT_IV_Transport_Invoice_Master_Tbl.Customer_Code = ACT_CustVend_Master_Tbl.CustVend_Code ";
                    SQLString += "AND ACT_IV_Transport_Invoice_Item_Tbl.Transaction_Number = ACT_IV_Transport_Invoice_Master_Tbl.Transaction_Number ";
                    SQLString += "AND CustVend_Type_Code='" + charge.CustVendorType + "' ";
                    SQLString += "AND Item_Code='" + charge.ChargeCode + "' ";
                    SQLString += "AND ACT_IV_Transport_Invoice_Master_Tbl.BRANCH_CODE='" + charge.BranchCode + "' ";

                    SqlDataAdapter daSearchCmd2 = new SqlDataAdapter(SQLString, cn);
                    DataSet dsSearchResult2 = new DataSet();
                    daSearchCmd2.Fill(dsSearchResult2);
                    cn.Close();
                    if (dsSearchResult2.Tables[0].Rows.Count > 0)
                    {
                        temp = true;
                        throw new FMException("Fail to delete Charge is used in invoice");
                    }
                    else
                    {
                        temp = false;
                    }
                }
                else
                    throw new FMException("Fail to delete Charge is used in booking");
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return temp;
        }

        internal static SortableList<Charge> GetAllTransportCharges()
        {
            SortableList<Charge> Charges = new SortableList<Charge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT * FROM TPT_CHARGE_TBL as tc
                                    inner join ACT_GL_Account_Tbl as gla
                                    ON gla.Account_Code = tc.Sales_Accnt_Code";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charges.Add(GetCharge(reader));
                }
                reader.Close();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return Charges;
        }
        internal static SortableList<Charge> GetAllTransportNoneMovementCharges()
        {
            SortableList<Charge> Charges = new SortableList<Charge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT * FROM TPT_CHARGE_TBL  as tc
                                    inner join ACT_GL_Account_Tbl as gla
                                    ON gla.Account_Code = tc.Sales_Accnt_Code
                                 WHERE tc.IS_CONTAINER_MOVEMENT = 'F'
                                   AND tc.IS_TRUCK_MOVEMENT = 'F'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charges.Add(GetCharge(reader));
                }
                reader.Close();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return Charges;
        }

        //20130605 - gerry added new parameters to cater multiple branch code
        internal static SortableList<Charge> GetAllTruckMovementCharges(string branchCode, string custVendType)
        {
            SortableList<Charge> Charges = new SortableList<Charge>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT * FROM TPT_CHARGE_TBL  as tc
                                    inner join ACT_GL_Account_Tbl as gla
                                    ON gla.Account_Code = tc.Sales_Accnt_Code
                                 WHERE tc.IS_TRUCK_MOVEMENT = 'T' AND tc.BRANCH_CODE='{0}' AND CUST_VEND_TYPE_CODE ='{1}'";

                SQLString = string.Format(SQLString, CommonUtilities.FormatString(branchCode), CommonUtilities.FormatString(custVendType));
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Charges.Add(GetCharge(reader));
                }
                reader.Close();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return Charges;
        }

        internal static ArrayList ChargeUOMs(string code)
        {
            ArrayList uoms = new ArrayList();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT UOM FROM TPT_CHARGE_TBL 
                                 WHERE IS_TRUCK_MOVEMENT = 'T'
                                    AND CHARGE_CODE = '{0}' ";

                SQLString = string.Format(SQLString, code);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    uoms.Add(reader.GetString(0));
                }
                reader.Close();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return uoms;
        }

        internal static Charge GetCharge(string chargeCode, string branchCode)
        {
            Charge charge = new Charge();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT * FROM TPT_CHARGE_TBL
                                        WHERE CHARGE_CODE = '{0}'
                                        AND BRANCH_CODE = '{1}'";

                SQLString = string.Format(SQLString, chargeCode, branchCode);

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    charge = GetCharge(reader);
                }
                cn.Close();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return charge;
        }


    
    }


}
