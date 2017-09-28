using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data; 
using FM.TR_FMSystemDLL.BLL; 
using FM.TR_FMSystemDLL.DAL; 

namespace FM.TR_FMSystemDLL.DAL
{
    internal class AccountDAL
    {

        internal static SortableList<Account> GetAccountInfo()
        {
            try
            {
                SortableList<Account> accountsInfo = new SortableList<Account>(); 
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string sqlGetAccountInfo = " Select Account_Code , Account_Desc  from Act_GL_Account_tbl where Bank_Recon_Y_N = 'T'";
                SqlCommand cmd = new SqlCommand(sqlGetAccountInfo, cn);
                cn.Open(); 
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    accountsInfo.Add(GetAccountInfo(reader)); 
                }
                cn.Close();
                return accountsInfo;
                
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        internal static Account GetAccountInfo(IDataReader reader)
        {
            Account acc = new Account((string)reader["Account_Code"], (string)reader["Account_Desc"]); 
            return acc; 
        }
    }
}
