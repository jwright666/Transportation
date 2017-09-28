using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TR_LicenseDLL.Base;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using System.Data;
using TR_LicenseDLL.DAL.WmsMobile;
using TR_LanguageResource.Resources;
using System.Collections;

namespace TR_LicenseDLL.BLL.WmsMobile
{
    public class WmsMobileCompany : Company
    {      
        //public string WebServiceIP { get; set; }
        //public string ServerIP { get; set; }
        public SortableList<WmsMobileNoOfLicense> wmsMobileNoOfLicenses { get; set; }
        public SortableList<WmsMobileNoOfLicense> oldWmsMobileNoOfLicenses { get; set; }

        public WmsMobileCompany()
            : base()
        {
            //this.WebServiceIP = string.Empty;
            //this.ServerIP = string.Empty;
        }

        public bool AddNewCompany()
        {
            bool retValue = false;
            string outMsg = "";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                ValidateFields();//call base class method
                if (GetAllCompanyNames().Contains(this.CompanyName))
                    throw new FMException("Company name already exist");
                if (WmsMobileCompanyDAL.IsCompanyValid(this)
                    && IsCompanyValid(this.CompanyName.Trim().ToUpper(), this.DatabaseName.Trim().ToUpper(), out outMsg))
                {
                    retValue = WmsMobileCompanyDAL.AddNewCompany(this, con, tran);
                }


                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }

            return retValue;
        }
        public int GetBalLicenseForGroup()
        {
            int retValue = 0;
            try
            {
                retValue = WmsMobileCompanyDAL.GetBalLicenseForGroup(this);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValue;
        }
        public int GetBalLicenseForCompany()
        {
            int retValue = 0;
            try
            {
                retValue = WmsMobileCompanyDAL.GetBalLicenseForCompany(this);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValue;
        }
        public bool DeleteCompany()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                retValue = WmsMobileCompanyDAL.DeleteCompany(this, con, tran);

                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }

            return retValue;
        }

        public static bool IsCompanyValid(string companyName, string dbName, out string outMsg)
        {
            bool retValue = false;
            outMsg = "";
            try
            {             
                retValue = WmsMobileCompanyDAL.IsCompanyValid(companyName, dbName, out outMsg);
            }
            catch (FMException ex)
            {
                outMsg = ex.Message.ToString();
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                outMsg = ex.Message.ToString();
                throw new FMException(ex.ToString());
            }

            return retValue;
        }

        public static SortableList<WmsMobileCompany> GetAllCompany()
        {
            return WmsMobileCompanyDAL.GetAllCompany(string.Empty);
        }
        public SortableList<WmsMobileNoOfLicense> GetMobileLicenses()
        {
            return WmsMobileCompanyDAL.GetVehicleNoLicenses(this);
        }
        public static WmsMobileCompany GetCompanyByName(string companyName)
        {
            WmsMobileCompany retValue = new WmsMobileCompany();
            try
            {
                if (GetAllCompany().Count > 0)
                {
                    foreach (WmsMobileCompany tempCompany in GetAllCompany())
                    {
                        if (tempCompany.CompanyName.Equals(companyName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            retValue = tempCompany;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }

            return retValue;
        }

        //user to display in combobox 
        public static ArrayList GetAllCompanyNames()
        {
            ArrayList retValue = new ArrayList();
            try
            {
                retValue = WmsMobileCompanyDAL.GetAllCompanyNames();
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValue;
        }

        public bool IsValidNoOfLicenses()
        {
            bool retValue = true;
            try
            {
                int mobileDevicesCount = WmsMobileDevice.GetAllMobileDevicesByCompany(this.CompanyName).Count;
                //if (mobileDevicesCount > this.NumberOfLicense)
                //{
                //    throw new FMException(WmsResourceBLL.MsgMobileDeviceCount + mobileDevicesCount + WmsResourceBLL.ErrCantChangeNoOfLicense);
                //}
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValue;
        }

        //add license to database
        public bool AddMobileLicense(WmsMobileNoOfLicense mobileLicense)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (this.IsValidToAddLicense(mobileLicense))
                {
                    if (this.wmsMobileNoOfLicenses.Count > 0)
                    {
                        WmsMobileNoOfLicense previousLicense = WmsMobileCompanyDAL.GetLatestLicenseForCompany(this);
                        WmsMobileCompanyDAL.EditExpiryOfLicense(previousLicense, mobileLicense.validFrom.Date.AddDays(-1), con, tran);
                    }
                    //TODO add license 
                    retValue = WmsMobileCompanyDAL.AddMobileLicense(mobileLicense, con, tran);
                    tran.Commit();
                }
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }

            return retValue;
        }
        //edit license from database
        public bool EditMobileLicense(WmsMobileNoOfLicense mobileLicense)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (this.IsValidToEditLicense(mobileLicense))
                {
                    if (this.wmsMobileNoOfLicenses.Count > 1)
                    {
                        WmsMobileCompanyDAL.EditExpiryOfLicense(this.wmsMobileNoOfLicenses[1], mobileLicense.validFrom.Date.AddDays(-1), con, tran);
                    }

                    retValue = WmsMobileCompanyDAL.EditMobileLicense(mobileLicense, con, tran);
                }

                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }

            return retValue;
        }

        //not final logic
        public bool IsValidToEditLicense(WmsMobileNoOfLicense mobileLicense)
        {
            try
            {
                if (this.wmsMobileNoOfLicenses.Count > 1)
                {
                    mobileLicense.IsValidToEditLicense(this.wmsMobileNoOfLicenses[1]); //base class
                    //if (mobileLicense.validFrom.Date <= this.wmsMobileNoOfLicenses[1].validFrom.Date)
                    //{
                    //    throw new FMException("New effective date must be after the effective date of the previous license. ");
                    //}
                }
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return true;
        }

        public bool IsValidToAddLicense(WmsMobileNoOfLicense mobileLicense)
        {
            try
            {
                if (this.wmsMobileNoOfLicenses.Count > 0)
                {
                    foreach (WmsMobileNoOfLicense tempLicense in this.wmsMobileNoOfLicenses)
                    {
                        tempLicense.IsValidToAddLicense(mobileLicense);//base class
                        //if (tempLicense.validFrom >= mobileLicense.validFrom.Date)
                        //{
                        //    throw new FMException("Effective date must be after the effective date of the previous license. ");
                        //}
                    }
                }

            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return true;
        }
        public ArrayList GetGroupNames()
        {
            ArrayList retValue = new ArrayList();
            try
            {
                retValue = WmsMobileCompanyDAL.GetAllCompanyGroups();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }
       
    }
}
