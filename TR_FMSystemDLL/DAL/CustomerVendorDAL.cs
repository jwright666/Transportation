using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using System.Collections;

namespace FM.TR_FMSystemDLL.DAL
{
    internal class CustomerVendorDAL
    {
        // gets a list of customers from database
        internal static List<CustomerDTO> GetAllCustomers()
        {
            try
            {
                List<CustomerDTO> customers = new List<CustomerDTO>();
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM ACT_CustVend_Master_Tbl WHERE Customer_Y_N = 'T'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(GetCustomer(reader));
                }
                cn.Close();
                return customers;
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL "+ex.ToString());
            }
        }

        // creates customerDTO object from 1 row of the DataReader
        internal static CustomerDTO GetCustomer(IDataReader reader)
        {
            try
            {
                #region Gerry removed 20130325
                /*
                string address1 = "";
                if (reader["Address1"].Equals(System.DBNull.Value))
                {
                    address1 = "";
                }
                else
                {
                    address1 = (string)reader["Address1"];
                }

                string address2 = "";
                if (reader["Address2"].Equals(System.DBNull.Value))
                {
                    address2 = "";
                }
                else
                {
                    address2 = (string)reader["Address2"];
                }

                string address3 = "";
                if (reader["Address3"].Equals(System.DBNull.Value))
                {
                    address3 = "";
                }
                else
                {
                    address3 = (string)reader["Address3"];
                }

                string address4 = "";
                if (reader["Address4"].Equals(System.DBNull.Value))
                {
                    address4 = "";
                }
                else
                {
                    address4 = (string)reader["Address4"];
                }

                //string customergroup = CustomerGroup.GetCustomerGroup((string)reader["CustVend_Code"]);
                string customergroup = "ABC";
                bool taxable_flag = false;
                if (reader["Taxable_Flag"].Equals(System.DBNull.Value))
                {
                    taxable_flag = false;
                }
                else
                {
                    string temp = (string)reader["Taxable_Flag"];
                    if (temp == "T")
                    {
                        taxable_flag = true;
                    }
                    else
                    {
                        taxable_flag = false;
                    }
                }

                decimal creditLimit = 0;
                if (reader["Credit_Limit"].Equals(System.DBNull.Value))
                {
                    creditLimit = 0;
                }
                else
                {
                    creditLimit = (decimal)reader["Credit_Limit"];
                }
                string equiCurrCode = string.Empty;
                if (!reader["Equi_Curr_Code"].Equals(System.DBNull.Value))
                {
                    equiCurrCode = (string)reader["Equi_Curr_Code"];
                }
                else
                {
                    equiCurrCode = string.Empty;
                }
                bool equivCurr_Y_N = false;
                if (reader["Equivalent_Y_N"].Equals("T"))
                {
                    equivCurr_Y_N = true;
                }
                string countryCode = string.Empty;
                if (!reader["Country_Code"].Equals(System.DBNull.Value))
                {
                    countryCode = (string)reader["Country_Code"];
                }
                SortableList<CurrencyRate> currencyRates = new SortableList<CurrencyRate>();
                currencyRates = CurrencyRate.GetAllCurrencyRatesForCustomer((string)reader["CustVend_Code"], DateTime.Today);

                return new CustomerDTO(
                   (string)reader["CustVend_Code"],
                   (string)reader["CustVend_Name"],
                   address1,
                   address2,
                   address3,
                   address4,
                   (string)reader["City_Code"],
                   customergroup,
                   taxable_flag,
                   (string)reader["Zip_Code"],
                  string.Empty,
                    //(string)reader["Salesman_Code"],
                   (byte)reader["Term_Code"],
                   creditLimit,
                   (string)reader["Country_Code"],
                   currencyRates,
                   countryCode, //(string)reader["Currency_Code"],
                   (string)reader["CustVend_Type_Code"],
                   equiCurrCode,
                   equivCurr_Y_N);
                */
                #endregion
                string custCode =  reader["CustVend_Code"] == DBNull.Value ? string.Empty : (string)reader["CustVend_Code"];
    
                SortableList<CurrencyRate> currencyRates = new SortableList<CurrencyRate>();
                currencyRates = CurrencyRate.GetAllCurrencyRatesForCustomer(custCode, DateTime.Today);

                return new CustomerDTO(
                 custCode,
                 reader["CustVend_Name"] == DBNull.Value ? string.Empty : (string)reader["CustVend_Name"],
                 reader["Address1"] == DBNull.Value ? string.Empty : (string)reader["Address1"],
                 reader["Address2"] == DBNull.Value ? string.Empty : (string)reader["Address2"],
                 reader["Address3"] == DBNull.Value ? string.Empty : (string)reader["Address3"],
                 reader["Address4"] == DBNull.Value ? string.Empty : (string)reader["Address4"],
                 reader["City_Code"] == DBNull.Value ? string.Empty : (string)reader["City_Code"],
                 "ABC",   //hardcoded group code
                 (reader["Taxable_Flag"] == DBNull.Value ? string.Empty : (string)reader["Taxable_Flag"]) == "T" ? true : false,
                 reader["Zip_Code"] == DBNull.Value ? string.Empty : (string)reader["Zip_Code"],
                 reader["Salesman_Code"] == DBNull.Value ? string.Empty : (string)reader["Salesman_Code"],
                 reader["Term_Code"] == DBNull.Value ? 0 : (byte)reader["Term_Code"],
                 reader["Credit_Limit"] == DBNull.Value ? 0 : (decimal)reader["Credit_Limit"],
                 reader["Country_Code"] == DBNull.Value ? string.Empty : (string)reader["Country_Code"],
                  currencyRates,
                 reader["Currency_Code"] == DBNull.Value ? string.Empty : (string)reader["Currency_Code"],
                 reader["CustVend_Type_Code"] == DBNull.Value ? string.Empty : (string)reader["CustVend_Type_Code"],
                 reader["Equi_Curr_Code"] == DBNull.Value ? string.Empty : (string)reader["Equi_Curr_Code"],
                 (reader["Equivalent_Y_N"] == DBNull.Value ? string.Empty : (string)reader["Equivalent_Y_N"]) == "T" ? true : false);
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
        }

        internal static CustomerDTO GetCustomerByCustCode(string custCode)
        {
            CustomerDTO customer = new CustomerDTO();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM ACT_CustVend_Master_Tbl WHERE Customer_Y_N = 'T'";
                SQLString += " AND CustVend_Code = '" + custCode + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer = GetCustomer(reader);
                }
                cn.Close();
                return customer;
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
        }


        internal static string GetCustomerNameByCustomerCode(string custCode)
        {
            string customer = "";
            try
            {
                custCode = CommonUtilities.FormatString(custCode);
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT CustVend_Name FROM ACT_CustVend_Master_Tbl WHERE Customer_Y_N = 'T'";
                SQLString += " AND CustVend_Code = '" + custCode + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer = (string)reader["CustVend_Name"];
                }
                reader.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return customer;
        }

        internal static CustomerDTO GetCustomerByCustName(string custName)
        {
            CustomerDTO customer = new CustomerDTO();
            try
            {
                custName = CommonUtilities.FormatString(custName);
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM ACT_CustVend_Master_Tbl WHERE Customer_Y_N = 'T'";
                SQLString += " AND CustVend_Name = '" + custName + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer = GetCustomer(reader);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return customer;
        }


        internal static CustomerDTO GetCustomerByNonCustomerCustomerCode(string custCode)
        {
            CustomerDTO customer = new CustomerDTO();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM ACT_CustVend_Master_Tbl WHERE ";
                SQLString += " AND CustVend_Code = '" + custCode + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customer = GetCustomer(reader);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return customer;
        }

        internal static bool AddCustomerGroupCode(CustomerDTO customer, SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_CUSTOMER__CUSTOMER_GROUP_Tbl", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CUSTOMER_CODE", customer.Code);
                cmd.Parameters.AddWithValue("@CUSTOMER_GROUP_CODE", customer.CustomerGroupCode);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                temp = true;
            }
            catch (Exception Ex)
            {
                temp = false;
                throw new FMException("Error at AddCustomerGroupCode in CustomerVendorDAL class" + Ex.ToString());

            }
            return temp;
        }

        internal static bool EditCustomerGroupCode(CustomerDTO customer, SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Edit_TPT_CUSTOMER__CUSTOMER_GROUP_Tbl", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CUSTOMER_CODE", customer.Code);
                cmd.Parameters.AddWithValue("@CUSTOMER_GROUP_CODE", customer.CustomerGroupCode);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                temp = true;
            }
            catch (Exception Ex)
            {
                temp = false;
                throw new FMException("Error at EditCustomerGroupCode in CustomerVendorDAL class" + Ex.ToString());

            }
            return temp;
        }

        internal static bool DeleteCustomerGroupCode(CustomerDTO customer, SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_TPT_CUSTOMER__CUSTOMER_GROUP_Tbl", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CUSTOMER_CODE", customer.Code);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                temp = true;
            }
            catch (Exception Ex)
            {
                temp = false;
                throw new FMException("Error at DeleteCustomerGroupCode in CustomerVendorDAL class" + Ex.ToString());

            }
            return temp;
        }

        internal static bool ValidateEditCustomerGroup(CustomerDTO customer)
        {
            bool temp = true;
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_JOB_MAIN_Tbl WHERE CUST_CODE = " + "'" + customer.Code + "'";
                SQLString += " AND JOB_STATUS = '4'";
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
                    temp = false;
                }
                else
                {
                    temp = true;
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return temp;
        }

        internal static List<string> GetAllValidCurrencyCode(CustomerDTO customer)
        {
            List<string> currCode = new List<string>();
            try
            {
                currCode.Add(customer.CurrCode);
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT Currency_Code FROM ACT_CustVend_MultiCurr_Tbl WHERE CustVend_Code = '" + customer.Code + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    currCode.Add((string)reader["Currency_Code"]);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return currCode;
        }


        // Chong Chin 22 April 2010 - Start - return only cust code - to improve speed
        internal static List<string> GetAllCustomerCodes()
        {
            List<string> customerCodes = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT CustVend_Code FROM ACT_CustVend_Master_Tbl WHERE Customer_Y_N = 'T' order by CustVend_Code asc";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customerCodes.Add((string)reader["CustVend_Code"]);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return customerCodes;

        }
        // Chong Chin 22 April 2010 - End
        internal static List<string> GetAllCustomerNames()
        {
            List<string> customerCodes = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT CustVend_Name FROM ACT_CustVend_Master_Tbl WHERE Customer_Y_N = 'T'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customerCodes.Add((string)reader["CustVend_Name"]);
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return customerCodes;

        }
        internal static SortedDictionary<string,string> GetAllCustCodeAndNames()
        {
            SortedDictionary<string, string> customers = new SortedDictionary<string, string>();
            try
            {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM ACT_CustVend_Master_Tbl WHERE Customer_Y_N = 'T'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add((string)reader["CustVend_Code"],(string)reader["CustVend_Code"]+"\t"+(string)reader["CustVend_Name"]);
            }
            cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return customers;

        }


        internal static ArrayList GetCustVendType()
        {
            ArrayList temp = new ArrayList();
            temp.Add("");
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT * FROM ACT_CustVend_Type_Tbl";

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //temp.Add((string)reader["CustVend_Type_Code"], (string)reader["Description"]);
                    temp.Add((string)reader["CustVend_Type_Code"]);
                }
                cn.Close();
            }
            catch { }

            return temp;
        }

        //20130322 - Gerry added to populate sub contractors
        internal static List<string> GetAllSubContractorCodes()
        {
            List<string> customerCodes = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT CustVend_Code FROM ACT_CustVend_Master_Tbl WHERE Vendor_Y_N = 'T'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customerCodes.Add(reader.GetString(0).ToString());
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return customerCodes;

        }
        internal static CustomerDTO GetSubContractorByCode(string code)
        {
            CustomerDTO subContractor = new CustomerDTO();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM ACT_CustVend_Master_Tbl WHERE Vendor_Y_N = 'T'";
                SQLString += " AND CustVend_Code ='" + code + "'";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    subContractor = GetCustomer(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new FMException("CustVendorDAL " + ex.ToString());
            }
            return subContractor;

        }


    }
}
