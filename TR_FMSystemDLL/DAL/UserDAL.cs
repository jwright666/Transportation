using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class UserDAL
    {
        public static User GetUser(string userID)
        {
            User user = new User();

            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM SEC_Users_Tbl";
                SQLString += " WHERE user_id = '" + userID + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = GetUser(reader);
                }
                cn.Close();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing User table for user " + userID + ex.ToString());
            }

            return user;

        }

        internal static User GetUser(IDataReader reader)
        {
            //6 January 2012 - Gerry modified, if user lastname is empty or space system cannot read as null
            int temp = Convert.ToInt16(reader["Language_ID"]);
            string firstName = "";
            if (reader["First_Name"] is System.DBNull || (string)reader["First_Name"] == "")
                firstName = "";
            else
                firstName = (string)reader["First_Name"];

            string lastname = "";
            if (reader["Last_Name"] is System.DBNull || (string)reader["Last_Name"] == "")
                lastname = "";            
            else
                lastname = (string)reader["Last_Name"];
                        
            string defaultBranch = "";
            if (reader["Default_Branch"] is System.DBNull || (string)reader["Default_Branch"] == "")
                defaultBranch = "";
            else
                defaultBranch = (string)reader["Default_Branch"];

            BranchDTO branchDTO = BranchDTO.GetBranch(defaultBranch);

            string userID = "";
            if (reader["User_ID"] is System.DBNull || (string)reader["User_ID"] == "")
                userID = "";
            else
                userID = (string)reader["User_ID"];

            return new User(
               //(string)reader["User_ID"],
               //(string)reader["First_Name"],
               userID,
               firstName,
               lastname,
               branchDTO,
               temp
            );
        }


    }
}
