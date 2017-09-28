using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;

using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.DAL
{
    class CustomerGroupDAL
    {
        internal static bool AddCustomerGroup(CustomerGroup customerGroup,
            SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_CUSTOMER_GROUP_Tbl", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GROUP_CODE", customerGroup.CustomerGroupCode);
                cmd.Parameters.AddWithValue("@GROUP_NAME", customerGroup.CustomerGroupName);
                cmd.Parameters.AddWithValue("@BRANCH_CODE", customerGroup.BranchCode);
                cmd.Parameters.AddWithValue("@CUST_VEND_TYPE_CODE", customerGroup.CustVendType);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new FMException("Fail to insert new customergroup in AddCustomerGroup in CustomerGroupDAL class");
            }
            return true;
        }

        internal static bool EditCustomerGroup(CustomerGroup customerGroup,
            SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Edit_TPT_CUSTOMER_GROUP_Tbl", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GROUP_CODE", customerGroup.CustomerGroupCode);
                cmd.Parameters.AddWithValue("@GROUP_NAME", customerGroup.CustomerGroupName);
                cmd.Parameters.AddWithValue("@BRANCH_CODE", customerGroup.BranchCode);
                cmd.Parameters.AddWithValue("@CUST_VEND_TYPE_CODE", customerGroup.CustVendType);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException("Fail to edit customergroup in EditCustomerGroup in CustomerGroupDAL class");
            }
            return true;
        }


        internal static bool DeleteCustomerGroup(CustomerGroup customerGroup,
            SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TPT_CUSTOMER_GROUP_Tbl", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GROUP_CODE", customerGroup.CustomerGroupCode);
                cmd.Parameters.AddWithValue("@BRANCH_CODE", customerGroup.BranchCode);
                cmd.Parameters.AddWithValue("@CUST_VEND_TYPE_CODE", customerGroup.CustVendType);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException("Fail to delete customergroup in DeleteCustomerGroup in CustomerGroupDAL class");
            }
            return true;
        }

        internal static bool FindCustomerGroupInCustomerDTO(CustomerGroup customerGroup)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_CUSTOMER_CUSTOMER_GROUP_Tbl ";
            SQLString += " WHERE CUSTOMER_GROUP_CODE = " + " '" + customerGroup.CustomerGroupCode + "'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();


            IDataReader reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                count++;
            }
            if (count > 0)
            {
                cn.Close();
                return true;
            }
            else
            {
                cn.Close();
                return false;
            }

        }

        internal static SortableList<CustomerGroup> GetAllCustomerGroups()
        {
            SortableList<CustomerGroup> customerGroups = new SortableList<CustomerGroup>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_CUSTOMER_GROUP_TBL ";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customerGroups.Add(GetCustomerGroup(reader));
            }
            cn.Close();
            return customerGroups;
        }

        internal static string GetCustomerGroup(string customerCode)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_CUSTOMER_CUSTOMER_GROUP_Tbl ";
            SQLString += " WHERE CUSTOMER_CODE = " + " '" + customerCode + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            string temp = "";
            while (reader.Read())
            {
                temp = (string)reader["CUSTOMER_GROUP_CODE"];
            }
            cn.Close();
            return temp;
        }


        internal static CustomerGroup GetCustomerGroup(IDataReader reader)
        {
            return new CustomerGroup(
               (string)reader["GROUP_CODE"],
               (string)reader["GROUP_NAME"],
               (string)reader["BRANCH_CODE"],
               (string)reader["CUST_VEND_TYPE_CODE"]
            );
        }

        internal static bool FindCustomerGroupInsideQuotation(CustomerGroup customerGroup)
        {

            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_MKT_QUOTATION_HEADER_TBL ";
            SQLString += "WHERE CUSTOMER_GROUP_NO = " + " '" + customerGroup.CustomerGroupCode + "'";
            SQLString += " AND BRANCH_CODE = " + " '" + customerGroup.BranchCode + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                count++;
                if (count > 0)
                {
                    break;
                }
            }
            cn.Close();
            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
