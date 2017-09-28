using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TR_LicenseDLL.DAL.WmsMobile;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using System.Data;
using TR_LanguageResource.Resources;

namespace TR_LicenseDLL.BLL.WmsMobile
{
    public class WmsMobileDevice
    {         
        public string CompanyName { get; set; }
        public string AndroidID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string DeviceName { get; set; }

        public WmsMobileDevice()
        {
            this.CompanyName = "";
            this.AndroidID = "";
            this.Brand = "";
            this.Model = "";
            this.DeviceName = "";            
        }

        public static bool IsAndroidIDValid(string androidID, string companyName, out string outMsg)
        {
            bool retValue = false;
            outMsg = "";
            try
            {
                retValue = WmsMobileDeviceDAL.IsAndroidIDValid(androidID, companyName, out outMsg);
            }
            catch (Exception ex)
            {
                outMsg = ex.Message.ToString();
                throw new FMException(ex.Message.ToString());
            }

            return retValue;
        }
        public bool AddMobileDevice()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (IsLicenseAvailable() && IsMobileDeviceCompanyValid() && !IsAndroidIDExist(this))
                {
                    retValue = WmsMobileDeviceDAL.AddMobileDevice(this, con, tran);

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
        public bool DeleteMobileDevice()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (!IsAndroidIDBeingUsed())
                {
                    retValue = WmsMobileDeviceDAL.DeleteMobileDevice(this, con, tran);

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
        public bool EditMobileDevice()
        {
            bool retValue = false;
            string outMsg = "";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (IsAndroidIDValid(this.AndroidID, this.CompanyName, out outMsg))
                {
                    retValue = WmsMobileDeviceDAL.EditMobileDevice(this, con, tran);

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

        public bool IsAndroidIDBeingUsed()
        {
            bool retValue = false;
            try
            {
                retValue = WmsMobileDeviceDAL.IsMobileDeviceBeingUsed(this);
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
        public static SortableList<WmsMobileDevice> GetAllMobileDevicesByCompany(string companyName)
        {
            return WmsMobileDeviceDAL.GetAllMobileDevicesByCompany(companyName);
        }
        public static SortableList<WmsMobileDevice> GetAllMobileDevices()
        {
            return WmsMobileDeviceDAL.GetAllMobileDevices();
        }
        public static bool IsAndroidIDExist(WmsMobileDevice mobileDevice)
        {
            bool retValue = false;
            try
            {
                retValue = WmsMobileDeviceDAL.IsMobileDeviceExist(mobileDevice);
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
        public bool IsMobileDeviceCompanyValid()
        {
            bool retValue = false;
            try
            {
                retValue = WmsMobileDeviceDAL.IsMobileDeviceCompanyValid(this);
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

        public bool IsLicenseAvailable()
        {
            bool retValue = true;
            try
            {
                WmsMobileCompany company = WmsMobileCompany.GetCompanyByName(this.CompanyName);
                int licenseBalance = 0;
                if (company.ShareWithGoup)
                {
                    licenseBalance = company.GetBalLicenseForGroup();
                    if (licenseBalance <= WmsMobileCompanyDAL.GetAvailableVehiclesInGroupOfCompanies(company))
                    {
                        throw new FMException("Sorry! You have been exceed the number of license allowed in the group. Please contact support@innosys.com.sg to acquire additional license. ");
                    } 

                }
                else
                {
                    licenseBalance = company.GetBalLicenseForCompany();
                    if (licenseBalance <= 0)
                    {
                        throw new FMException("Sorry! You have been exceed the number of license allowed in the company. Please contact support@innosys.com.sg to acquire additional license. ");
                    }
                }

            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //call getBalLicenseForGroup else
            //call getBalLicenseForCompany()
            //If balance > 0 return true else return false
            return retValue;

        }
    }
}
