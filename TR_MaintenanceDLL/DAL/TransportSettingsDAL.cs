using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using TR_LanguageResource.Resources;

namespace FM.TR_MaintenanceDLL.DAL
{
    internal class TransportSettingsDAL
    {
        internal static bool GetArePricesVisibleInJob()
        {
            string temp = "";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_SPECIAL_DATA_Tbl";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                temp = (string)reader["PRICES_VISIBLE_IN_JOB"];
            }
            cn.Close();
            if (temp == "T")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        internal static TransportSettings GetTransportSetting(IDataReader reader)
        {
            TransportSettings t;
            try
            {
                int qty_decimal = reader["QUANTITY_DECIMAL"].Equals(System.DBNull.Value) ? 0 : (int)reader["QUANTITY_DECIMAL"];
                int foreign_curr_decimal = reader["FOREIGN_CURRENCY_DECIMAL"].Equals(System.DBNull.Value) ? 0 : (int)reader["FOREIGN_CURRENCY_DECIMAL"];
                int total_amount_decimal = reader["TOTAL_AMOUNT_DECIMAL"].Equals(System.DBNull.Value) ? 0 : (int)reader["TOTAL_AMOUNT_DECIMAL"];
                int unit_amount_decimal = reader["UNIT_AMOUNT_DECIMAL"].Equals(System.DBNull.Value) ? 0 : (int)reader["UNIT_AMOUNT_DECIMAL"];
                string localEqvCurr = reader["Local_Eqv_Curr"].Equals(System.DBNull.Value) ? string.Empty : (string)reader["Local_Eqv_Curr"];
                bool pricevisible = (string)reader["PRICES_VISIBLE_IN_JOB"] == "T" ? true : false;
                t = new TransportSettings(
                (string)reader["TPT_CODE"],
                (string)reader["TPT_NAME"],
                (string)reader["ADDRESS1"],
                (string)reader["ADDRESS2"],
                (string)reader["ADDRESS3"],
                (string)reader["ADDRESS4"],
                (int)reader["BOOKING_NO"],
                (int)reader["JOB_NO"],
                (string)reader["BOOKING_NO_PREFIX"],
                (string)reader["JOB_NO_PREFIX"],
                (int)reader["LOG_NO"],
                qty_decimal,
                foreign_curr_decimal,
                total_amount_decimal,
                unit_amount_decimal,
                pricevisible,
                (int)reader["QUOTATION_NO"],
                (string)reader["COMPANY_REG_NO"],
                (string)reader["GST_REG_NO"],
                (string)reader["Home_Currency_Code"],
                localEqvCurr,
                (string)reader["CULTURE"]
                    // 2014-07-11 Zhou Kai adds
                , reader["DEFAULT_IMPORT_PORT_CODE"].ToString(),
                String.Empty
                    // 2014-07-11 Zhou Kai ends
                );

                t.AirportOfficeCode = reader["AirportOfficeCode"] == DBNull.Value ? string.Empty : (string)reader["AirportOfficeCode"];
                //20141031 - gerry added for planning checking time, note timer measure miliseconds
                //default is 30 sec or 30000 miliseconds
                string timeCheck = (string)(reader["PLANNING_UPDATECHECK_TIME"] == DBNull.Value ? "30" : (string)reader["PLANNING_UPDATECHECK_TIME"]);
                t.planningCheckTime = Convert.ToInt32(timeCheck);//(reader["PLANNING_UPDATECHECK_TIME"] == DBNull.Value ? 30 : (int)reader["PLANNING_UPDATECHECK_TIME"]);// * 1000;
                t.location = (string)(reader["LOCATION"] == DBNull.Value ? string.Empty : (string)reader["LOCATION"]);
                t.DefaultWarehouseCode = (string)(reader["DEFAULT_WAREHOUSE_CODE"] == DBNull.Value ? string.Empty : (string)reader["DEFAULT_WAREHOUSE_CODE"]);
            }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return t;
        }
        internal static TransportSettings GetTransportSettingForInvoice(SqlConnection cn)
        {
            TransportSettings transportSettings = new TransportSettings();
            try
            {
                string SQLString = "SELECT * FROM TPT_Special_Data_Tbl, ACT_GL_Special_Data_Tbl";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                cmd.Transaction = cn.BeginTransaction();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    transportSettings = GetTransportSetting(reader);
                }
                reader.Close();
            }
            catch (FMException fmEx)
            {
                throw new FMException(fmEx.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return transportSettings;
        }

        /// <summary>
        /// 2014-08-21 Zhou Kai modifies this function, get rid of the database
        /// transaction object and use the using clause to ensure the database
        /// connection object is always closed
        /// </summary>
        /// <returns>the TransportSettings object, which contains the setting
        /// values for the transport modules</returns>
        internal static TransportSettings GetTransportSetting()
        {
            TransportSettings transportSettings = new TransportSettings();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = "SELECT * FROM TPT_Special_Data_Tbl, ACT_GL_Special_Data_Tbl, CRT_Special_Data_Tbl";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    // cmd.Transaction = cn.BeginTransaction();
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        transportSettings = GetTransportSetting(reader);
                    }
                    reader.Close();
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                catch (Exception ex) { throw new FMException(ex.ToString()); }
                finally { cn.Close(); }
            }
            
            return transportSettings;
        }
        
        internal static bool AddTransportSetting(TransportSettings t)
        {
            try
            {
                string visi = "T";
                if (t.ArePricesVisibleInJob==true)
                    visi="T";
                else
                    visi="F";


                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLInsertDriverString = " Insert into TPT_SPECIAL_DATA_Tbl(RECORD_KEY,TPT_CODE,TPT_NAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,BOOKING_NO,JOB_NO,BOOKING_NO_PREFIX,JOB_NO_PREFIX,LOG_NO,PRICES_VISIBLE_IN_JOB,QUANTITY_DECIMAL,FOREIGN_CURRENCY_DECIMAL,TOTAL_AMOUNT_DECIMAL,UNIT_AMOUNT_DECIMAL,QUOTATION_NO,COMPANY_REG_NO,GST_REG_NO, TRUCK_JOB_NO_PREFIX,CULTURE, AirportOfficeCode, PLANNING_UPDATECHECK_TIME, DEFAULT_IMPORT_PORT_CODE, DEFAULT_WAREHOUSE_CODE) " +
                                               " Values (1,'" +
                                               CommonUtilities.FormatString(t.Tpt_code) + "','" +
                                               CommonUtilities.FormatString(t.Tpt_name) + "','" +
                                               CommonUtilities.FormatString(t.Address1) + "','" +
                                               CommonUtilities.FormatString(t.Address2) + "','" +
                                               CommonUtilities.FormatString(t.Address3) + "','" +
                                               CommonUtilities.FormatString(t.Address4) + "'," +
                                               +t.Booking_no + "," +
                                               +t.Job_no + ",'" +
                                               CommonUtilities.FormatString(t.Booking_no_prefix) + "','" +
                                               CommonUtilities.FormatString(t.Job_no_prefix) + "'," +
                                               t.Log_no + ",'" +
                                               CommonUtilities.FormatString(visi) + "'," +
                                               +t.QuantityDecimals + "," +
                                               +t.ForeignCurrencyDecimals + "," +
                                               +t.TotalAmountDecimals + "," +
                                               +t.UnitAmountDecimals + "," +
                                               +t.Quotation_no + ",'" +
                                               CommonUtilities.FormatString(t.Company_reg_no) + "','" +
                                               CommonUtilities.FormatString(t.Gst_reg_no) + "'," +
                                               "'TR','" +
                                               CommonUtilities.FormatString(t.Culture.Trim()) + "','" +
                                               CommonUtilities.FormatString(t.AirportOfficeCode.Trim()) + "'," +
                                               t.planningCheckTime+ "," +
                                               CommonUtilities.FormatString(t.DefaultPortCode.Trim()) + "','" +
                                               CommonUtilities.FormatString(t.DefaultWarehouseCode.Trim()) + "')";                                               

                SqlCommand cmd = new SqlCommand(SQLInsertDriverString, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrInsertFailed + " Transport Settings. \n" + ex.ToString());
            }
        }

        internal static bool EditTransportSetting(TransportSettings t)
        {
            try
            {
                string visi = "T";
                if (t.ArePricesVisibleInJob == true)
                    visi = "T";
                else
                    visi = "F";
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLUpdateDriverString = " Update TPT_SPECIAL_DATA_Tbl set " +
                                               " ADDRESS1 ='" + CommonUtilities.FormatString(t.Address1) + "'," +
                                               " ADDRESS2 ='" + CommonUtilities.FormatString(t.Address2) + "'," +
                                               " ADDRESS3 ='" + CommonUtilities.FormatString(t.Address3) + "'," +
                                               " ADDRESS4 ='" + CommonUtilities.FormatString(t.Address4) + "'," +
                                               " AirportOfficeCode ='" + CommonUtilities.FormatString(t.AirportOfficeCode) + "'" +
                                                // 2014-07-11 Zhou Kai adds
                                               " ,DEFAULT_IMPORT_PORT_CODE = '" + CommonUtilities.FormatString(t.DefaultPortCode) + "'" +
                                               // 2014-07-11 Zhou Kai ends
                                                ",PLANNING_UPDATECHECK_TIME = " + t.planningCheckTime +
                                               " ,DEFAULT_WAREHOUSE_CODE = '" + CommonUtilities.FormatString(t.DefaultWarehouseCode) + "'" +
                                               " Where RECORD_KEY = '1'";

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrEditFailed + " Transport Settings. \n" + ex.ToString());
            }      
        }

     


    }
}
