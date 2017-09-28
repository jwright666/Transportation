using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class CurrencyRateDAL
    {
        internal static CurrencyRate GetCurrencyRate(string currencyCode,DateTime requiredDate)
        {
            //6 Jan 2012 - Gerry Modified query to include time and minutes for the effectivity and expiry of currency exchange rate
            CurrencyRate CurrencyRate = new CurrencyRate();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM ACT_Currency_Exch_Rate_Tbl ";
            SQLString += "INNER JOIN ACT_Currency_Tbl ";
            SQLString += "on ACT_Currency_Exch_Rate_Tbl.Currency_Code = ACT_Currency_Tbl.Currency_Code ";
            SQLString += "AND (((Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date.AddHours(23).AddMinutes(59)) + "') AND (Expiry_Date_Time > '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate) + "')) ";
            SQLString += "OR ((Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date.AddHours(23).AddMinutes(59)) + "') AND (Expiry_Date_Time is NULL)))";
            SQLString += " WHERE ACT_Currency_Tbl.Currency_Code='" + currencyCode+"'";
            
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CurrencyRate=GetCurrencyRate(reader);
            }
            // 2014-08-21 Zhou Kai adds
            reader.Close();
            // 2014-08-21 Zhou Kai ends
            cn.Close();

            return CurrencyRate;
        }

        
        internal static SortableList<CurrencyRate> GetAllCurrencyRates(DateTime requiredDate)
        {
            SortableList<CurrencyRate> CurrencyRates = new SortableList<CurrencyRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = " SELECT * FROM ACT_Currency_Exch_Rate_Tbl ";
                   SQLString += "INNER JOIN ACT_Currency_Tbl ";
                   SQLString += "on ACT_Currency_Exch_Rate_Tbl.Currency_Code = ACT_Currency_Tbl.Currency_Code ";
                   SQLString += "AND (((Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date.AddHours(23).AddMinutes(59)) + "') AND (Expiry_Date_Time > '" + DateUtility.ConvertDateForSQLPurpose(requiredDate) + "')) ";
                   SQLString += "OR ((Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date.AddHours(23).AddMinutes(59)) + "') AND (Expiry_Date_Time is NULL)))";
                   //requiredDate.ToString("MM/dd/yyyy")
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CurrencyRates.Add(GetCurrencyRate(reader));
            }
            cn.Close();

            return CurrencyRates;
        }

        internal static SortableList<CurrencyRate> GetAllCurrencyRatesForCustomer(string custCode,DateTime requiredDate)
        {
            //6 Jan 2012 - Gerry Modified query to include time and minutes for the effectivity and expiry of currency exchange rate
            SortableList<CurrencyRate> CurrencyRates = new SortableList<CurrencyRate>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT c.Currency_Code, c.Currency_Name, c.Curr_Factor,  c.No_Decimal_Flag, c.Round_Up_Flag,";
            SQLString += "b.Effective_Date_Time, b.Expiry_Date_time, ";
            SQLString += "b.Exchange_Rate, b.Variance, a.CustVend_Code ";
            SQLString += "FROM ACT_CustVend_Master_Tbl a,ACT_Currency_Exch_Rate_Tbl b ";
            SQLString += "INNER JOIN ACT_Currency_Tbl c ";
            SQLString += "on b.Currency_Code = c.Currency_Code ";
            //Gerry added 23hrs and 23minutes for required date to cater all currency exchange rate for that day.
            SQLString += "AND (((b.Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date.AddHours(23).AddMinutes(59))/*.ToString("MM/dd/yyyy")*/ + "') AND (b.Expiry_Date_Time > '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date) + "')) ";
            SQLString += "OR ((b.Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date.AddHours(23).AddMinutes(59)) + "') AND (b.Expiry_Date_Time is NULL)))";
            //end
            SQLString += " Where a.Currency_Code = b.Currency_Code";
            SQLString += " AND a.CustVend_Code='" + custCode + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CurrencyRates.Add(GetCurrencyRate(reader));
            }
            cn.Close();

            SQLString = "SELECT c.Currency_Code, c.Currency_Name, c.Curr_Factor,  c.No_Decimal_Flag, c.Round_Up_Flag,";//Gerry Modify this line
            SQLString += "b.Effective_Date_Time, b.Expiry_Date_time, ";
            SQLString += "b.Exchange_Rate, b.Variance, a.CustVend_Code, a.Default_Y_N ";
            SQLString += "FROM ACT_CustVend_MultiCurr_Tbl a, ACT_Currency_Exch_Rate_Tbl b ";
            SQLString += "INNER JOIN ACT_Currency_Tbl c ";
            SQLString += "on b.Currency_Code = c.Currency_Code ";
            //Gerry added 23hrs and 23minutes for required date to cater all currency exchange rate created for that day.
            SQLString += "AND (((b.Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date.AddHours(23).AddMinutes(59)) + "') AND (b.Expiry_Date_Time > '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date) + "')) ";
            SQLString += "OR ((b.Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(requiredDate.Date.AddHours(23).AddMinutes(59)) + "') AND (b.Expiry_Date_Time is NULL)))";
            //end
            SQLString += " Where a.Currency_Code = b.Currency_Code";
            SQLString += " AND a.CustVend_Code='" + custCode + "'";
            
            if (CurrencyRates.Count == 1)
            {
                SQLString += " AND a.Currency_Code<>'" + CurrencyRates[0].CurrencyCode + "'";
            }
            cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            reader = cmd.ExecuteReader();
            //Gerry Removed - we need to add to the CurrencyRates collection
            //SortableList<CurrencyRate> CurrencyRatesValues = new SortableList<CurrencyRate>();
            
            while (reader.Read())
            {
                //CurrencyRatesValues.Add(GetCurrencyRateForCustomer(reader));
                CurrencyRates.Add(GetCurrencyRateForCustomer(reader));
            }
            cn.Close();            
            
            return CurrencyRates;
        }

        // creates OperatorDTO object from 1 row of the DataReader
        internal static CurrencyRate GetCurrencyRate(IDataReader reader)
        {
            //20130325 - Gerry modify to set string empty when value is null
            bool showDecimal = (reader["No_Decimal_Flag"] == DBNull.Value ? string.Empty : (string)reader["No_Decimal_Flag"]) == "F" ? true : false;
            bool roundFlag = (reader["Round_Up_Flag"] == DBNull.Value ? string.Empty : (string)reader["Round_Up_Flag"]) == "T" ? true : false;
            string currCode = reader["Currency_Code"] == DBNull.Value ? string.Empty : (string)reader["Currency_Code"];
            string currName = reader["Currency_Name"] == DBNull.Value ? string.Empty : (string)reader["Currency_Name"];
            return new CurrencyRate(
               currCode,
               currName,
            reader["Exchange_Rate"] == DBNull.Value ? 0 : (decimal)reader["Exchange_Rate"],
            reader["Variance"] == DBNull.Value ? 0 : (decimal)reader["Variance"],
                true,
            reader["Curr_Factor"] == DBNull.Value ? 0 : (int)reader["Curr_Factor"],
               showDecimal,
               roundFlag);
        }

        internal static CurrencyRate GetCurrencyRateForCustomer(IDataReader reader)
        {
            #region 20130325 Gerry removed and replaced
            /*
            string temp = (string)reader["Default_Y_N"];
            bool default_Y_N = false;
            if (temp == "T")
            {
                default_Y_N = true;
            }

            bool showDecimal = false;
            if ((string)reader["No_Decimal_Flag"] == "F")
            {
                showDecimal = true;
            }

            bool roundFlag = false;
            if ((string)reader["Round_Up_Flag"] == "T")
            {
                roundFlag = true; ;
            }
            return new CurrencyRate(
               (string)reader["Currency_Code"],
               (string)reader["Currency_Name"],
               (decimal)reader["Exchange_Rate"],
               (decimal)reader["Variance"],
               default_Y_N,

               (int)reader["Curr_Factor"],
               showDecimal,
               roundFlag);
            */
            #endregion
            return new CurrencyRate(
                reader["Currency_Code"] == DBNull.Value ? string.Empty : (string)reader["Currency_Code"],
                reader["Currency_Name"] == DBNull.Value ? string.Empty : (string)reader["Currency_Name"],
                reader["Exchange_Rate"] == DBNull.Value ? 0 : (decimal)reader["Exchange_Rate"],
                reader["Variance"] == DBNull.Value ? 0 : (decimal)reader["Variance"],
                (reader["Default_Y_N"] == DBNull.Value ? string.Empty : (string)reader["Default_Y_N"]) == "T" ? true : false,

                reader["Curr_Factor"] == DBNull.Value ? 0 : (int)reader["Curr_Factor"],
                (reader["No_Decimal_Flag"] == DBNull.Value ? string.Empty : (string)reader["No_Decimal_Flag"]) == "F" ? true : false,
                (reader["Round_Up_Flag"] == DBNull.Value ? string.Empty : (string)reader["Round_Up_Flag"]) == "T" ? true : false);
        }

        internal static bool GetValidCurrencyRateVarianceRange(string currencyCode, DateTime invoicedate, decimal compareCurrencyRate)
        {
            try
            {
                decimal minusCurrencyRate = 0;
                decimal addCurrencyRate = 0;
                bool isValid = true; 
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLString = " SELECT ACT_Currency_Exch_Rate_Tbl.Currency_code,ACT_Currency_Exch_Rate_Tbl.Variance,(Exchange_rate +(Exchange_rate *(variance /-100))) as Less, Exchange_Rate , (Exchange_rate +(Exchange_rate*(variance /100))) as More " + 
                                   " FROM ACT_Currency_Exch_Rate_Tbl " + 
                                   " INNER JOIN ACT_Currency_Tbl " +
                                   " on ACT_Currency_Exch_Rate_Tbl.Currency_Code = ACT_Currency_Tbl.Currency_Code " +
                                   " AND ACT_Currency_Tbl.Currency_Code ='" +  currencyCode.ToString() + "'" +
                                   " AND (((Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(invoicedate.Date.AddHours(23).AddMinutes(59)) + "') AND (Expiry_Date_Time > '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(invoicedate) + "')) " +
                                   " OR ((Effective_Date_Time <= '" + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(invoicedate.Date.AddHours(23).AddMinutes(59)) + "') AND (Expiry_Date_Time is NULL)))";

                SqlCommand comm = new SqlCommand(SQLString, cn);

                #region Removed
                /*
                DataSet ds = new DataSet(); 
                SqlDataAdapter adapter = new SqlDataAdapter(comm);                
                adapter.Fill(ds);
                cn.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    minusCurrencyRate =  Convert.ToDecimal(dr["Less"]);
                    addCurrencyRate = Convert.ToDecimal(dr["More"]);                     
                }
                */
                #endregion

                IDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    minusCurrencyRate = Convert.ToDecimal(reader["Less"]);
                    addCurrencyRate = Convert.ToDecimal(reader["More"]);
                }
                if (!((compareCurrencyRate >= minusCurrencyRate) && (compareCurrencyRate <= addCurrencyRate)))
                {
                    isValid = false; 
                }

                return isValid;
                //(out)minusCurrencyRate;
                
               //deductedCurrencyRate= minusCurrencyRate ;
               //addedCurrencyRate= addCurrencyRate  ;
                 
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }

        internal static string GetCurrencyName(string code)
        {
            string name = "";
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                string query = @"select Currency_Name from ACT_Currency_Tbl
                                    where Currency_Code ='{0}'";

                query = string.Format(query, code);

                SqlCommand comm = new SqlCommand(query, cn);
                IDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    name = reader.GetString(0).ToString();
                }
                reader.Close();
            }
            catch { }

            return name;
        }
    }
}
