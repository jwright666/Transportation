using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;
using System.Data.SqlClient;
//using FM.FMSystem.Resources;
using System.Collections;
using TR_LanguageResource.Resources;




namespace FM.TR_FMSystemDLL.BLL
{
    public class CustomerDTO
    {
        private string code;
        private string name;
        private string Address1;
        private string Address2;
        private string Address3;
        private string Address4;
        private string City;
        private string customerGroupCode;
        private bool taxable;
        private string zipcode;
        private string salesman;
        private int terms;
        private decimal creditLimit;
        private string countryCode;
        private SortableList<CurrencyRate> currencyRates;
        private string currCode;
        private string custVendTypeCode ; 
        private string equiCurrCode;
        //2011 May 23 - Gerry Added this property
        private bool equivCurr_Y_N;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool Taxable
        {
            get { return taxable; }
        }

        public string Zipcode
        {
            get { return zipcode; }
        }

        public string Salesman
        {
            get { return salesman; }
        }

        public int Terms
        {
            get { return terms; }
        }

        public decimal CreditLimit
        {
            get { return creditLimit; }
        }

        public string CountryCode
        {
            get { return countryCode; }
        }

        public SortableList<CurrencyRate> CurrencyRates
        {
            get { return currencyRates; }
        }

        public string CurrCode
        {
            get { return currCode; }
        }

        public string CustomerGroupCode
        {
            get { return customerGroupCode; }
            set { customerGroupCode = value; }
        }

        public string address1
        {
            get { return Address1; }
            set { Address1 = value; }
        }

        public string address2
        {
            get { return Address2; }
            set { Address2 = value; }
        }

        public string address3
        {
            get { return Address3; }
            set { Address3 = value; }
        }

        public string address4
        {
            get { return Address4; }
            set { Address4 = value; }
        }
        //20140314 - gerry added
        public string city
        {
            get { return City; }
            set { City = value; }
        }
        public string CustVendTypeCode
        {
            get { return custVendTypeCode; }
        }

        public string EquiCurrCode
        {
            get { return equiCurrCode; }
        }

        public CustomerDTO()
        {
            this.code = "";
            this.name = "";
            this.Address1="";
            this.Address2 = "";
            this.Address3 = "";
            this.Address4 = "";
            this.City = "";
            this.customerGroupCode = "";
            this.taxable = true;
            this.zipcode = "";
            this.salesman = "";
            this.terms = 0;
            this.creditLimit = 0;
            this.countryCode = "";
            this.currencyRates = new SortableList<CurrencyRate>();
            this.currCode = "";
            this.custVendTypeCode = "";
            this.equiCurrCode = "";
        }

        //2011 May 23 - Gerry Added this property
        public bool EquivCurr_Y_N
        {
            get { return equivCurr_Y_N; }
            set { equivCurr_Y_N = value; }
        }

        public CustomerDTO(string code, string name, string address1, string address2,
            string address3, string address4, string city, string customerGroupCode,
            bool taxable,string zipcode, string salesman, int terms, decimal creditLimit,
            string countryCode, SortableList<CurrencyRate> currencyRates, string currCode,string custvendtypecode,
            string equicurrcode, bool equivCurr_Y_N)
        {
            this.code = code;
            this.name = name;
            this.Address1 = address1;
            this.Address2 = address2;
            this.Address3 = address3;
            this.Address4 = address4;
            this.City = city;
            this.customerGroupCode = customerGroupCode;
            this.taxable = taxable;
            this.zipcode = zipcode;
            this.salesman = salesman;
            this.terms = terms;
            this.creditLimit = creditLimit;
            this.countryCode = countryCode;
            this.currencyRates = currencyRates;
            this.currCode = currCode;
            this.custVendTypeCode = custvendtypecode;
            this.equiCurrCode = equicurrcode;
            this.EquivCurr_Y_N = equivCurr_Y_N;
        }

        // static methods

        public static List<CustomerDTO> GetAllCustomers()
        {
            // delegates method call to customerVendorDAL
            return CustomerVendorDAL.GetAllCustomers();
        }

        public static CustomerDTO GetCustomerByCustCode(string custCode)
        {
            CustomerDTO c = new CustomerDTO();
            try
            {
                c = CustomerVendorDAL.GetCustomerByCustCode(custCode);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return c;
        }

        public static string GetCustomerNameByCustomerCode(string custCode)
        {
            string c = "";
            try
            {
                c = CustomerVendorDAL.GetCustomerNameByCustomerCode(custCode);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return c;
        }


        public static CustomerDTO GetCustomerByCustName(string custName)
        {
            CustomerDTO c = new CustomerDTO();
            try
            {
                c = CustomerVendorDAL.GetCustomerByCustName(custName);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return c;
        }



        public bool AddCustomerGroupCode()
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                temp = CustomerVendorDAL.AddCustomerGroupCode(this, con, tran);
                tran.Commit();
            }
            catch (Exception ex)
            {
                temp = false;
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return temp;
        }

        public bool EditCustomerGroupCode()
        {
            bool temp = false;
            if (ValidateEditCustomerGroup() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    temp = CustomerVendorDAL.EditCustomerGroupCode(this, con, tran);
                    tran.Commit();
                }
                catch (FMException e)
                {
                    throw new FMException(e.ToString());
                }
                catch (Exception ex)
                {
                    temp = false;
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return temp;
        }

        public bool DeleteCustomerGroupCode()
        {
            bool temp = false;
            if (ValidateEditCustomerGroup() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    temp = CustomerVendorDAL.DeleteCustomerGroupCode(this, con, tran);
                    tran.Commit();
                }
                catch (FMException e)
                {
                    throw new FMException(e.ToString());
                }
                catch (Exception ex)
                {
                    temp = false;
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return temp;
        }


        public bool ValidateEditCustomerGroup()
        {
            if (CustomerVendorDAL.ValidateEditCustomerGroup(this) == false)
            {
                throw new FMException(TptResourceBLL.ErrorExistingJobWithCustomerGroup);
            }
            else
            {
                return true;
            }
        }

        public List<string> GetAllValidCurrencyCode()
        {
            return CustomerVendorDAL.GetAllValidCurrencyCode(this);
        }

        //Chong Chin 22 April 2010 -Start -  Return string collection for faster loading 

        public static List<string> GetAllCustomerCodes()
        {
            return CustomerVendorDAL.GetAllCustomerCodes();
        }
        // Chong Chin 22 April 2010 End

        public static List<string> GetAllCustomerNames()
        {
            return CustomerVendorDAL.GetAllCustomerNames();
        }

        public static ArrayList GetCustVendType()
        {
            return CustomerVendorDAL.GetCustVendType();
        }

        //20130322 - Gerry added to populate sub contractors
        public static List<string> GetAllSubContractorCodes()
        {
            return CustomerVendorDAL.GetAllSubContractorCodes();
        }
        public static CustomerDTO GetSubContractorByCode(string code)
        {
            return CustomerVendorDAL.GetSubContractorByCode(code);
        }

        //20140314 - gerry added 
        public object Clone()
        {
            CustomerDTO retValue = new CustomerDTO();
            retValue.code = this.code;
            retValue.name = this.name;
            return retValue;
        }
        //20140314 - gerry added 
        public override string ToString()
        {
            return this.code + (this.name == string.Empty ? string.Empty : (", " + this.name));
        }
    }

}
