using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using System.Data;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class ApplicationOptionDAL
    {

        public static ApplicationOption GetApplicationOption(string setting_ID, string setting_Name)
        {
            ApplicationOption retValue  = new ApplicationOption();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * from CRT_System_Special_Setup_Detail_Tbl where Setting_ID = '{0}' and Setting_Name ='{1}'";
                SQLString = string.Format(SQLString,CommonUtilities.FormatString(setting_ID), CommonUtilities.FormatString(setting_Name));
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                cmd.Transaction = cn.BeginTransaction();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue.setting_ID = reader["Setting_ID"].Equals(DBNull.Value) ? string.Empty : (string)reader["Setting_ID"];
                    retValue.setting_Name = reader["Setting_Name"].Equals(DBNull.Value) ? string.Empty : (string)reader["Setting_Name"];
                    retValue.setting_Desc = reader["Setting_Desc"].Equals(DBNull.Value) ? string.Empty : (string)reader["Setting_Desc"];

                    retValue.setting_Value = reader["Setting_Value"].Equals(DBNull.Value) ? "F" : (string)reader["Setting_Value"];
                   // retValue.setting_Value = temp.Equals("T") ? true : false;
                }
                reader.Close();
                cn.Close();
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
            return retValue;
        }
    }
}
