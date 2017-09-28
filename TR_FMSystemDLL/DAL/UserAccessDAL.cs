using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class UserAccessDAL
    {
        public static UserAccess GetUserAccess(User user)
        {
            UserAccess TempUserAcc = new UserAccess();
            SortableList<ModuleItemAccess> ModuleItemAccess = new SortableList<ModuleItemAccess>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM SEC_User_Rights_Control_Tbl";
            SQLString += " LEFT JOIN SEC_Module_Items_Code_Tbl ";
            SQLString += " ON SEC_User_Rights_Control_Tbl.Module_Item_Code=SEC_Module_Items_Code_Tbl.Module_Item_Code";
            SQLString += " WHERE user_id = '"+ user.UserID +"'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            bool found = false;
            while (reader.Read())
            {
                found = true;
                ModuleItemAccess.Add(GetModuleItemAccess(reader));
            }
            if (found)
            {
                TempUserAcc.User = user;
            }
            cn.Close();
            return TempUserAcc;

        }

        public static ModuleItemAccess GetUserAccessRightsForModuleItem(User user,string moduleItem)
        {
            ModuleItemAccess ModuleItemAccess = new ModuleItemAccess();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM SEC_User_Rights_Control_Tbl";
            SQLString += " WHERE user_id = '" + user.UserID + "'";
            SQLString += " AND Module_Item_code = '" + moduleItem + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ModuleItemAccess = GetModuleItemAccess(reader);
            }
            cn.Close();
            return ModuleItemAccess;

        }



        public static bool IsModuleItemAllowed(User user, string moduleItem)
        {

            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            string SQLSearchString = "SELECT * FROM SEC_User_Rights_Control_Tbl";
            SQLSearchString += " WHERE user_id = '" + user.UserID + "'";
            SQLSearchString += " AND Module_Item_code = '" + moduleItem + "'";
            SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
            DataSet dsSearchResult = new DataSet();
            daSearchCmd.Fill(dsSearchResult);
            cn.Close();
            if (dsSearchResult.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        internal static ModuleItemAccess GetModuleItemAccess(IDataReader reader)
        {
            string temp = "";
            bool add = false;
            temp = (string)reader["New_Right"];
            switch (temp)
            {
                case "T":
                    add = true;
                    break;
                case "F":
                    add = false;
                    break;
            }

            bool edit = false;
            temp = (string)reader["Edit_Right"];
            switch (temp)
            {
                case "T":
                    edit = true;
                    break;
                case "F":
                    edit = false;
                    break;
            }

            bool delete = false;
            temp = (string)reader["Delete_Right"];
            switch (temp)
            {
                case "T":
                    delete = true;
                    break;
                case "F":
                    delete = false;
                    break;
            }

            bool browse = false;
            temp = (string)reader["Browse_Right"];
            switch (temp)
            {
                case "T":
                    browse = true;
                    break;
                case "F":
                    browse = false;
                    break;
            }

            bool print = false;
            temp = (string)reader["Print_Right"];
            switch (temp)
            {
                case "T":
                    print = true;
                    break;
                case "F":
                    print = false;
                    break;
            }

            bool post = false;
            temp = (string)reader["Posting_Right"];
            switch (temp)
            {
                case "T":
                    post = true;
                    break;
                case "F":
                    post = false;
                    break;
            }

            return new ModuleItemAccess(
               (string)reader["Module_Item_Code"],
               add,
               edit,
               delete,
               browse,
               print,
               post
            );
        }


    }
}


