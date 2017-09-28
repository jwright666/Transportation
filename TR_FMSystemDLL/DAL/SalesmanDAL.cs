using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class SalesmanDAL
    {
        internal static SortableList<Salesman> GetAllSalesman()
        {
            SortableList<Salesman> Salesmen = new SortableList<Salesman>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM ACT_Salesman_Tbl ";
                   SQLString += "WHERE Current_Status = 'A'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Salesmen.Add(GetSalesman(reader));
            }
            cn.Close();
            return Salesmen;

        }

        // creates OperatorDTO object from 1 row of the DataReader
        internal static Salesman GetSalesman(IDataReader reader)
        {
            return new Salesman(
               (string)reader["Salesman_Code"],
               (string)reader["Salesman_Name"]);
        }

    }
}
