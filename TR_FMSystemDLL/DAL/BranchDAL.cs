using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class BranchDAL
    {
        // gets a list of Branches from database
        internal static List<BranchDTO> GetAllBranches()
        {
            List<BranchDTO> branches = new List<BranchDTO>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM CRT_Branch_Tbl ";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                branches.Add(GetBranch(reader));
            }
            cn.Close();
            return branches;
        }

        // creates BranchDTO object from 1 row of the DataReader
        internal static  BranchDTO GetBranch(IDataReader reader)
        {
            return new BranchDTO(
               (string)reader["Branch_Code"],
               (string)reader["Branch_Name"]);
        }

        internal static SortableList<BranchDTO> GetBranchsByUserName(string userID)
        {
            SortableList<BranchDTO> branches = new SortableList<BranchDTO>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            #region  Raplaced query 
            /*
            string SQLString = " SELECT Default_Branch as Branch_Code,Branch_Name FROM SEC_Users_Tbl SU , CRT_Branch_Tbl CB " + 
                               " where SU.User_ID ='" + userID.Trim() + "'" +
                               " and SU.Default_Branch = CB.Branch_Code " ;
             */
            #endregion
            //20120907 - Gerry Replaced the query because wrong table for user-branch combination used. 
            string SQLString = @"SELECT * FROM SEC_User_Branch_Tbl SU
                                    inner join CRT_Branch_Tbl CB
                                    ON CB.Branch_Code = SU.Branch_Code
                                    where User_ID ='{0}'";
            SQLString = string.Format(SQLString, userID);
            
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                branches.Add(GetBranch(reader));
            }
            return branches;
            cn.Close();
        }

        internal static BranchDTO GetBranch(string Branch_Code)
        {
            BranchDTO branch = new BranchDTO();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM CRT_Branch_Tbl ";
            SQLString += " WHERE Branch_Code = '" + Branch_Code + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                branch = GetBranch(reader);
            }
            return branch;
            cn.Close();
        }

    }
}
