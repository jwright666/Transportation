using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MarketDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_MarketDLL.DAL
{
    internal class UomDAL
    {
        internal static SortableList<Uom> GetAllUoms()
        {
            SortableList<Uom> Uoms = new SortableList<Uom>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM CRT_Measurement_Tbl ";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Uoms.Add(GetUomFromFM(reader));
            }
            cn.Close();
            return Uoms;

        }
        internal static List<string> GetAllUOMs()
        {
            List<string> Uoms = new List<string>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM CRT_Measurement_Tbl ";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Uoms.Add(reader.GetString(0));
            }
            Uoms.Remove("-");
            cn.Close();
            return Uoms;

        }

        internal static Uom GetUomFromFM(IDataReader reader)
        {
            return new Uom(
               reader["Measurement_Code"] == DBNull.Value ? string.Empty : (string)reader["Measurement_Code"],
               reader["Measurement_Desc"] == DBNull.Value ? string.Empty : (string)reader["Measurement_Desc"],
               UOM_TYPE.Others);
        }

        internal static Uom GetUomFromFMbyCode(string code)
        {
            Uom retValue = new Uom();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM CRT_Measurement_Tbl where Measurement_Code = '" + CommonUtilities.FormatString(code) + "'";
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = GetUomFromFM(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { cn.Close(); }
            return retValue;
        }
        internal static Uom GetUomFromCharge(IDataReader reader)
        {
            int temp = reader["UOM_TYPE"] == DBNull.Value ? 0 : (int)reader["UOM_TYPE"];
            UOM_TYPE tempUomType = UOM_TYPE.Others;
            switch (temp)
            { 
                case 1:
                    tempUomType = UOM_TYPE.Volume;
                    break;
                case 2:
                    tempUomType = UOM_TYPE.Weight;
                    break;
                default:
                    break;
            }
            Uom retValue =  new Uom(
               reader["UOM"] == DBNull.Value ? string.Empty : (string)reader["UOM"],
               reader["UOM_DESCRIPTION"] == DBNull.Value ? string.Empty : (string)reader["UOM_DESCRIPTION"],
               tempUomType);

            return retValue;
        }
        //20130603 - Gerry modified filter by chargecode, branch and custvend_type
        internal static SortableList<Uom> GetValidTRUomForChargeCode(Charge charge)
        {
            SortableList<Uom> Uoms = new SortableList<Uom>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT * FROM TPT_CHARGE_UOM_Tbl 
                                    where CHARGE_CODE = '{0}' 
                                        and BRANCH_CODE ='{1}' 
                                        and CUST_VEND_TYPE_CODE ='{2}'";
            try
            {
                SQLString = string.Format(SQLString, charge.ChargeCode, charge.BranchCode, charge.CustVendorType);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Uoms.Add(GetUomFromCharge(reader));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { cn.Close(); }
            return Uoms;
        }
        //20130603 - gerry modify to pass parameter charge object instead of chargecode, to use the branchcode and custvend_type
        internal static bool AddValidUOM(Charge charge, Uom validUOM, SqlConnection con, SqlTransaction tran)
        {
            bool temp = true;
            if (con.State == ConnectionState.Closed) { con.Open(); }
            try
            {
                #region SQLInstertString
                string SQLInstertString = @"INSERT INTO TPT_CHARGE_UOM_Tbl(CHARGE_CODE,UOM,UOM_DESCRIPTION,
                                                                        UOM_TYPE,BRANCH_CODE,CUST_VEND_TYPE_CODE)   
                                            VALUES ('{0}','{1}','{2}',{3},'{4}', '{5}')
                                                ";
                #endregion
                #region SQL Parameters
                SQLInstertString = string.Format(SQLInstertString,
                                        CommonUtilities.FormatString(charge.ChargeCode.Trim()),
                                        CommonUtilities.FormatString(validUOM.UomCode),
                                        CommonUtilities.FormatString(validUOM.UomDescription),
                                        (int)validUOM.UOM_Type.GetHashCode(),
                                        CommonUtilities.FormatString(charge.BranchCode),
                                        CommonUtilities.FormatString(charge.CustVendorType));
                #endregion  
   

                SqlCommand cmd = new SqlCommand(SQLInstertString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();  
            }
            catch(SqlException sqlEx)
            {
                temp = false;
                throw new FMException("Fail to insert in valid UOM \n" + sqlEx.ToString());
            }
            catch (Exception ex)
            {
                temp = false;
                throw new FMException("Fail to insert in valid UOM \n" + ex.ToString());
            }
            return temp;
        }

        internal static bool DeleteValidUOM(Charge charge, string uomCode, SqlConnection con, SqlTransaction tran)
        {
            bool temp = true;
            if (con.State == ConnectionState.Closed) { con.Open(); }
            try
            {
                string SQLUpdateDriverString = @" DELETE FROM TPT_CHARGE_UOM_Tbl 
                                             Where CHARGE_CODE = '{0}' AND UOM = '{1}' 
                                                    AND BRANCH_CODE = '{2}'  AND CUST_VEND_TYPE_CODE ='{3}'
                                               ";

                #region SQL Parameters
                SQLUpdateDriverString = string.Format(SQLUpdateDriverString,
                                        CommonUtilities.FormatString(charge.ChargeCode.Trim()),
                                        CommonUtilities.FormatString(uomCode),
                                        CommonUtilities.FormatString(charge.BranchCode.Trim()),
                                        CommonUtilities.FormatString(charge.CustVendorType.Trim()));
                #endregion  
                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                temp = false;
                throw new FMException("Fail to DELETE UOM \n" + ex.ToString());
            }
            return temp;
        }
        
        internal static bool IsUomValidToDelete(Charge charge, string uomCode)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"SELECT * FROM TPT_MKT_QUOTATION_DETAIL_RATE_TBL as rate
                                    INNER JOIN TPT_MKT_QUOTATION_HEADER_TBL as header
                                    ON header.QUOTATION_ID = rate.QUOTATION_ID
                                    INNER JOIN ACT_CustVend_Master_Tbl as custVend
                                    On custVend.CustVend_Code = header.Cust_Code

                                    where rate.CHARGE_CODE = '{0}' and rate.UOM = '{1}'
                                        AND header.BRANCH_CODE = '{2}'  AND custVend.CustVend_Type_Code ='{3}'
                                            ";
            try
            {
                SQLString = string.Format(SQLString, CommonUtilities.FormatString(charge.ChargeCode.Trim()),
                                                        CommonUtilities.FormatString(uomCode),
                                                        CommonUtilities.FormatString(charge.BranchCode.Trim()),
                                                        CommonUtilities.FormatString(charge.CustVendorType.Trim()));
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                object temp = (object)cmd.ExecuteScalar();
                if (temp == null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
        }
    
    }
}
