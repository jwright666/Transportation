using System;
using System.Data;
using System.Data.SqlClient; 
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL; 
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class TermsDAL
    {
        internal static SortableList<Terms> GetALLTerms()
        {
            try
            {
                SortableList<Terms> terms = new SortableList<Terms>(); 
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()); 
                string sqlGetTerms = "Select Term_Code,Term_Desc from ACT_Terms_Tbl ";
                SqlCommand cmd = new SqlCommand(sqlGetTerms, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    terms.Add(GetTerms(reader));
                }
                cn.Close();
                return terms;

            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
        internal static Terms GetTerms(IDataReader reader)
        {
            Terms term = new Terms( Convert.ToInt16(reader["Term_Code"]),
                                    (string)reader["Term_Desc"]);
         return term;
        }
    }
}
