using System;
using System.Collections.Generic;
using System.Text;
using System.Data; 
using System.Data.SqlClient; 
using FM.TR_FMSystemDLL.BLL; 
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class RateTypeDAL
    {
        internal static SortableList<RateType> GetAllRateType()
        {
            try
            {
                SortableList<RateType> RateTypies = new SortableList<RateType>();  
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string sqlGetRateType = " select * from CRT_Rate_Type_Tbl ";
                sqlGetRateType += " where rate_type_code= 'R' or rate_type_code= 'S'"; 
                SqlCommand cmd = new SqlCommand(sqlGetRateType, cn);
                cn.Open(); 
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RateTypies.Add(GetAllRateType(reader)); 
                }
                cn.Close();
                return RateTypies;
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        internal static RateType GetAllRateType(IDataReader reader)
        {
            try
            {
                RateType ratetype = new RateType((string)reader["Rate_Type_Code"], (string)reader["Rate_Type_Desc"]);
                return ratetype; 
            }
            catch(Exception ex) 
            {
                throw ex; 
            }
        }
    }
}
