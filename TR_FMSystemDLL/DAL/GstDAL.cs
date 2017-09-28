using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class GstDAL
    {
        internal static SortableList<Gst> GetAllGst(DateTime requiredDate)
        {
            SortableList<Gst> Gsts = new SortableList<Gst>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT DISTINCT * FROM ACT_AR_TAX_CODE_Tbl ";
//            SQLString += "WHERE (((Effective_Date_Time <= '" + DateUtility.ConvertDateForSQLPurpose(requiredDate) + "') AND (Expiry_Date_Time > '" + DateUtility.ConvertDateForSQLPurpose(requiredDate) + "')) ";
//            SQLString += "OR ((Effective_Date_Time <= '" + DateUtility.ConvertDateForSQLPurpose(requiredDate) + "') AND (Expiry_Date_Time is NULL)))";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Gsts.Add(GetGst(reader));
            }
            cn.Close();

            if (Gsts.Count > 0)
            {
                for (int i = 0; i < Gsts.Count; i++)
                {
                    cn.Open();

                    string SQLSearchString = "SELECT Tax_Rate FROM ACT_AR_TAX_CODE_Tbl ";
                    SQLSearchString += "WHERE (((Effective_Date_Time <= '" + DateUtility.ConvertDateForSQLPurpose(requiredDate) + "') AND (Expiry_Date_Time > '" + DateUtility.ConvertDateForSQLPurpose(requiredDate) + "')) ";
                    SQLSearchString += "OR ((Effective_Date_Time <= '" + DateUtility.ConvertDateForSQLPurpose(requiredDate) + "') AND (Expiry_Date_Time is NULL)))";
                    SQLSearchString += "AND Tax_Code='" + Gsts[i].GstType+ "'";

                    SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
                    DataSet dsSearchResult = new DataSet();
                    daSearchCmd.Fill(dsSearchResult);
                    cn.Close();
                    if (dsSearchResult.Tables[0].Rows.Count > 0)
                    {
                        Gsts[i].GstRate = (byte)dsSearchResult.Tables[0].Rows[0]["Tax_Rate"];
                    }
                }
            }

            return Gsts;
        }

        internal static Gst GetGst(IDataReader reader)
        {
            return new Gst(
               (string)reader["Tax_Code"],
               0,
               (DateTime)reader["Effective_Date_Time"]);
        }

        internal static Gst GetGstRate(string GstType,DateTime invoiceDate)
        {
            Gst Gst = new Gst();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT DISTINCT * FROM ACT_AR_TAX_CODE_Tbl ";
            SQLString += "WHERE Tax_Code = '" + GstType + "'";
            SQLString += "AND (((Effective_Date_Time <= '" + DateUtility.ConvertDateForSQLPurpose(invoiceDate) + "') AND (Expiry_Date_Time > '" + DateUtility.ConvertDateForSQLPurpose(invoiceDate) + "')) ";
            SQLString += "OR ((Effective_Date_Time <= '" + DateUtility.ConvertDateForSQLPurpose(invoiceDate) + "') AND (Expiry_Date_Time is NULL)))";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Gst = GetGst(reader);
            }
            cn.Close();

            cn.Open();
            string SQLSearchString = "SELECT Tax_Rate FROM ACT_AR_TAX_CODE_Tbl ";
            SQLSearchString += "WHERE (((Effective_Date_Time <= '" + DateUtility.ConvertDateForSQLPurpose(invoiceDate) + "') AND (Expiry_Date_Time > '" + DateUtility.ConvertDateForSQLPurpose(invoiceDate) + "')) ";
            SQLSearchString += "OR ((Effective_Date_Time <= '" + DateUtility.ConvertDateForSQLPurpose(invoiceDate) + "') AND (Expiry_Date_Time is NULL)))";
            SQLSearchString += "AND Tax_Code='" + Gst.GstType + "'";

            SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
            DataSet dsSearchResult = new DataSet();
            daSearchCmd.Fill(dsSearchResult);
            cn.Close();
            if (dsSearchResult.Tables[0].Rows.Count > 0)
            {
                Gst.GstRate = (byte)dsSearchResult.Tables[0].Rows[0]["Tax_Rate"];
            }



            return Gst;
        }


    }
}
