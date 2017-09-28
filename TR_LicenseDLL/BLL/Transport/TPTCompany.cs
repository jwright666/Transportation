using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TR_LicenseDLL.Base;
using FM.TR_FMSystemDLL.BLL;
using System.Collections;
using System.Data.SqlClient;
using TR_LicenseDLL.DAL.Transport;
using FM.TR_FMSystemDLL.DAL;
using System.Data;
using TR_LanguageResource.Resources;

namespace TR_LicenseDLL.BLL.Transport
{
    public class TPTCompany  : Company
    { 
        public SortableList<TPTVehicleNoOfLicense> vehicleNoLicenses { get; set; }
        public SortableList<TPTVehicleNoOfLicense> oldVehicleNoLicenses { get; set; }

        public TPTCompany()
            : base()
        {
            this.vehicleNoLicenses = new SortableList<TPTVehicleNoOfLicense>();
            this.oldVehicleNoLicenses = new SortableList<TPTVehicleNoOfLicense>();
        }


        public bool AddNewCompany()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                ValidateFields();//base class method
                if (TPTCompanyLicenseDAL.IsCompanyValid(this))
                {
                    retValue = TPTCompanyLicenseDAL.AddNewCompany(this, con, tran);
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
        public int GetBalLicense(DateTime appDate)
        {
            int balLicense = 0;
            try
            {
                switch (this.ShareWithGoup)
                {
                    case true:
                        balLicense = GetBalLicenseForGroup(appDate);
                        break;

                    default:
                        balLicense = GetBalLicenseForCompany(appDate);
                        break;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return balLicense;
        }
        public int GetBalLicenseForGroup(DateTime appDate)
        {
            return TPTCompanyLicenseDAL.GetBalNoOfLicenseForGroup(this, appDate);
        }
        public int GetBalLicenseForCompany(DateTime appDate)
        {
            return TPTCompanyLicenseDAL.GetBalNoOfLicenseForCompany(this, appDate);
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
                retValue = TPTCompanyLicenseDAL.DeleteCompany(this, con, tran);

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
        public static TPTCompany GetCompanyByName(string companyName, string databaseName)
        {
            TPTCompany retValue = null;
            try
            {
                retValue = TPTCompanyLicenseDAL.GetCompanyByName(companyName, databaseName);
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
        //add license to database
        public bool AddVehicleLicense(TPTVehicleNoOfLicense vehicleLicense)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (this.IsValidToAddLicense(vehicleLicense))
                {
                    if (this.vehicleNoLicenses.Count > 0)
                    {
                        TPTVehicleNoOfLicense previousLicense = TPTCompanyLicenseDAL.GetLatestLicenseForCompany(this);
                        TPTCompanyLicenseDAL.EditExpiryOfLicense(previousLicense, vehicleLicense.validFrom.Date.AddDays(-1), con, tran);
                    }
                    //TODO add license 
                    retValue = TPTCompanyLicenseDAL.AddVehicleNoLicense(vehicleLicense, con, tran);
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
        public bool EditVehicleLicense(TPTVehicleNoOfLicense vehicleLicense)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (this.IsValidToEditLicense(vehicleLicense))
                {
                    if (this.vehicleNoLicenses.Count > 1)
                    {
                        TPTCompanyLicenseDAL.EditExpiryOfLicense(this.vehicleNoLicenses[1], vehicleLicense.validFrom.Date.AddDays(-1), con, tran);
                    }

                    retValue = TPTCompanyLicenseDAL.EditVehicleNoLicense(vehicleLicense, con, tran);
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
    
        public bool HasValidLicense(DateTime appDate)
        {
            bool retValue = false;
            try
            {
                if (this.vehicleNoLicenses.Count > 0)
                {
                    foreach (TPTVehicleNoOfLicense tempLicense in this.vehicleNoLicenses)
                    {
                        if (tempLicense.validFrom <= appDate && tempLicense.validTo == null)
                        {
                            retValue = true;
                        }
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
            return retValue;
        }
        //not final logic
        public bool IsValidToEditLicense(TPTVehicleNoOfLicense vehicleLicense)
        {
            try
            {
                if (this.vehicleNoLicenses.Count > 1)
                {
                    vehicleLicense.IsValidToEditLicense(this.vehicleNoLicenses[1]);  //base class
                    //if (vehicleLicense.validFrom.Date <= this.vehicleNoLicenses[1].validFrom.Date)
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

        public bool IsValidToAddLicense(TPTVehicleNoOfLicense vehicleLicense)
        {
            try
            {
                if (this.vehicleNoLicenses.Count > 0)
                {
                    foreach (TPTVehicleNoOfLicense tempLicense in this.vehicleNoLicenses)
                    {
                        tempLicense.IsValidToAddLicense(vehicleLicense);//base class
                        //if (tempLicense.validFrom >= vehicleLicense.validFrom.Date)
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

        public static SortableList<TPTCompany> GetAllCompany()
        {
            return TPTCompanyLicenseDAL.GetAllCompany(string.Empty);
        }
        public SortableList<TPTVehicleNoOfLicense> GetVehicleNoLicenses()
        {
            return TPTCompanyLicenseDAL.GetVehicleNoLicenses(this);
        }
        public ArrayList GetGroupNames()
        {
            ArrayList retValue = new ArrayList();
            try
            {
                retValue = TPTCompanyLicenseDAL.GetAllCompanyGroup();
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
