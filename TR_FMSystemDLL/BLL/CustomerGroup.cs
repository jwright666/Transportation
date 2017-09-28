using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using TR_LanguageResource.Resources;
//using FM.FMSystem.Resources;

namespace FM.TR_FMSystemDLL.BLL
{
    public class CustomerGroup
    {
        private string customerGroupCode;
        private string customerGroupName;
        private string branchCode;
        private string custVendType;

        public CustomerGroup()
        {
            this.customerGroupCode = "";
            this.customerGroupName = "";
            this.branchCode = "";
            this.custVendType = "";
        }

        public CustomerGroup(string customerGroupCode, string customerGroupName, 
            string branchCode, string custVendType)
        {
            this.CustomerGroupCode = customerGroupCode;
            this.CustomerGroupName = customerGroupName;
            this.branchCode = branchCode;
            this.custVendType = custVendType;
        }

        public string CustomerGroupCode
        {
            get { return customerGroupCode; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrorCustomerGroupCodeBlank);
                }
                else
                {
                    customerGroupCode = value;
                }
            }
        }

        public string CustomerGroupName
        {
            get { return customerGroupName; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrorCustomerGroupNameBlank);
                }
                else
                {
                    customerGroupName = value;
                }
            }
        }

        public string BranchCode
        {
            get { return branchCode; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrBranchCodeBlank);
                }
                else
                {
                    branchCode = value;
                }
            }
        }

        public string CustVendType
        {
            get { return custVendType; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrorCustVendBlank);
                }
                else
                {
                    custVendType = value;
                }
            }
        }


        public static SortableList<CustomerGroup> GetAllCustomerGroups()
        {
            return CustomerGroupDAL.GetAllCustomerGroups();
        }

        public static string GetCustomerGroup(string customerCode)
        {
            return CustomerGroupDAL.GetCustomerGroup(customerCode);
        }

        public bool AddCustomerGroup(string frmName, string user)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                CustomerGroupDAL.AddCustomerGroup(this, con, tran);

                // now form the log entry
                DateTime serverDateTime = Logger.GetServerDateTime();
                FMModule module = FMModule.Transport;

                // form the header
                LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
                    customerGroupCode, customerGroupCode, FormMode.Add, user);

                // This is an example of create the 2 logDetail objects
                LogDetail logDetail1 = new LogDetail("Group Code", customerGroupCode);

                // add the logDetails object to the List collection of logHeader
                logHeader.LogDetails.Add(logDetail1);

                // now call the Logger class to write

                Logger.WriteLog(logHeader, con, tran);


                tran.Commit();
                temp = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                temp = false;
            }
            finally
            {
                con.Close();
            }
            return temp;

        }

        public bool EditCustomerGroup(string frmName, string user)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                CustomerGroupDAL.EditCustomerGroup(this, con, tran);

                // now form the log entry
                DateTime serverDateTime = Logger.GetServerDateTime();
                FMModule module = FMModule.Transport;

                // form the header
                LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
                    customerGroupCode, customerGroupCode, FormMode.Edit, user);

                // This is an example of create the 2 logDetail objects
                LogDetail logDetail1 = new LogDetail("Group Code", customerGroupCode);

                // add the logDetails object to the List collection of logHeader
                logHeader.LogDetails.Add(logDetail1);

                // now call the Logger class to write

                Logger.WriteLog(logHeader, con, tran);


                tran.Commit();
                temp = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                temp = false;
            }
            finally
            {
                con.Close();
            }
            return temp;
        }

        public bool DeleteCustomerGroup(string frmName, string user)
        {
            bool temp = false;
            if (CanDeleteCustomerGroup() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    CustomerGroupDAL.DeleteCustomerGroup(this, con, tran);


                    // now form the log entry
                    DateTime serverDateTime = Logger.GetServerDateTime();
                    FMModule module = FMModule.Transport;

                    // form the header
                    LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
                        customerGroupCode, customerGroupCode, FormMode.Delete, user);

                    // This is an example of create the 2 logDetail objects
                    LogDetail logDetail1 = new LogDetail("Group Code", customerGroupCode);

                    // add the logDetails object to the List collection of logHeader
                    logHeader.LogDetails.Add(logDetail1);

                    // now call the Logger class to write

                    Logger.WriteLog(logHeader, con, tran);


                    tran.Commit();
                    temp = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    temp = false;
                }
                finally
                {
                    con.Close();
                }
            }
            return temp;
        }

        internal static bool FindCustomerGroupInsideQuotation(CustomerGroup customerGroup)
        {
            if (CustomerGroupDAL.FindCustomerGroupInsideQuotation(customerGroup) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanDeleteCustomerGroup()
        {
            try
            {
                if (CustomerGroupDAL.FindCustomerGroupInCustomerDTO(this) == true)
                {
                    throw new FMException(TptResourceBLL.ErrorCannotDeleteGroupUsedByCustomer);
                }
                else
                {
                    if (FindCustomerGroupInsideQuotation(this) == true)
                    {
                        throw new FMException(TptResourceBLL.ErrorCannotDeleteGroupUsedInQuotation);
                    }
                    else
                    {
                        //check inside job, if it's not billed, based on quotation no
                        //if there is customer group used this
                        //then cant edit or delete
                        return true;
                    }
                }
            }
            catch (FMException ex)
            {
                throw;
            }
        }


    }
}
