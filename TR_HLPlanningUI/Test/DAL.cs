using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.Data;
using System.Data.SqlClient;

namespace FM.TransportPlanDLL.Test
{
    // 2015-03-21 Zhou Kai adds this class, for accessing database
    public class DAL
    {
        #region "perform data access operations"
        public static void InsertAValue(Values theValue)
        {
            string strSqlCon = "server=zhoukai\\sql2012; user id = ipl;" +
                "password = support; database = webapi_crud;Trusted_Connection = false;";
            string strQuery = "insert into myvalues(id, value, remark, " +
                " additional_remark) values ({0}, '{1}', '{2}', '{3}');";
            strQuery = String.Format(strQuery, theValue.ID, theValue.Value, 
                theValue.Remark, "NA");
            using (SqlConnection dbCon = new SqlConnection(strSqlCon))
            using (SqlCommand dbCmd = new SqlCommand())
            {
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbCmd.CommandType = CommandType.Text;
                    dbCmd.CommandText = strQuery;
                    dbCmd.Connection = dbCon;

                    dbCmd.ExecuteNonQuery();

                }
                catch (InvalidOperationException ioe) { throw new Exception(ioe.Message); }
                catch (InvalidCastException ice) { throw new Exception(ice.Message); }
                catch (SqlException se) { throw new Exception(se.Message); }
                catch (Exception e) { throw new Exception(e.Message); }
            }

            return;
        }


        #endregion
    }
}