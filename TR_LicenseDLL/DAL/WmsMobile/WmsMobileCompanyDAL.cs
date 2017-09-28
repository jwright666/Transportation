using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using Encryption;
using System.Data.SqlClient;
using System.Data;
using FM.TR_FMSystemDLL.DAL;
using TR_LicenseDLL.BLL.WmsMobile;
using System.Collections;

namespace TR_LicenseDLL.DAL.WmsMobile
{
    public class WmsMobileCompanyDAL
    {
        internal static bool AddNewCompany(WmsMobileCompany company, SqlConnection con, SqlTransaction tran)
        {       //TODO add company 
            bool retValue = false;
            try
            {
                string query = @"insert into WH_Mobile_Company_Tbl
                                    values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";

                query = string.Format(query,
                                            CommonUtilities.FormatString(company.GroupName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(company.CompanyName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(FMEncryption.Encrypt(company.GroupName.Trim())),
                                            CommonUtilities.FormatString(FMEncryption.Encrypt(company.CompanyName.Trim())),
                                            CommonUtilities.FormatString(company.GroupNameLocal.Trim().ToUpper()),
                                            CommonUtilities.FormatString(company.CompanyNameLocal.Trim().ToUpper()),
                                            CommonUtilities.FormatString(company.DatabaseName.Trim()),
                                            CommonUtilities.FormatString(company.SQLServer.Trim()),
                                            company.ShareWithGoup ? "T" : "F");


                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();
                //tran.Commit();
                retValue = true;
            }
            catch (FMException fmEx)
            {
                throw new FMException(fmEx.ToString());
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }

            return retValue;
        }
        //this method will be called during license registration
        internal static bool IsCompanyValid(WmsMobileCompany company)
        {
            bool retValue = true;
            string[] param = new string[3];
            param[0] = "ipl";
            param[1] = company.SQLServer.ToString().Trim();
            param[2] = company.DatabaseName.ToString().Trim();
            FMGlobalSettings.TheInstance.SetConnectionString(param);
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string query = @"Select top 1 Company_Name from CRT_Special_Data_Tbl";

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (company.CompanyName.Trim() != reader.GetString(0).Trim())
                    {
                        throw new FMException("Sorry! the company you have entered is not valid. Please make sure that you are connecting to the correct server and database name. ");
                    }
                }
                reader.Close();
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return retValue;
        }

        internal static bool DeleteCompany(WmsMobileCompany company, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO edit no of licenses
                string query = @"delete from WH_Mobile_Company_Tbl
                                     where GroupName = '{0}' and CompanyName = '{1}'";

                query = string.Format(query, CommonUtilities.FormatString(company.GroupName.Trim()),
                                            CommonUtilities.FormatString(company.CompanyName.Trim()));

                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();
                //tran.Commit();
                retValue = true;
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }  
        internal static bool IsCompanyValid(string companyName, string dbName, out string errMsg)
        {
            bool retValue = true;
            errMsg = "";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                //TODO call WmsMobileEcryptLogic.getEncrypt then compare   
                string encryptedCompanyName = FMEncryption.Encrypt(companyName);
                string query = @"Select * from WH_Mobile_Company_Tbl
                                    where CompanyNameEC ='{0}' and DatabaseName ='{1}'";
                query = string.Format(query, CommonUtilities.FormatString(encryptedCompanyName.Trim())
                                            , CommonUtilities.FormatString(dbName));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (encryptedCompanyName.Trim() != (string)reader["CompanyNameEC"].ToString().Trim()
                        || dbName.Trim().ToUpper() != (string)reader["DatabaseName"].ToString().Trim().ToUpper())
                    {
                        retValue = false;
                        break;
                    }
                }
                reader.Close();
                if (!retValue)
                {
                    errMsg = "Sorry! your company is not registered. Please contact innosys staff to register your company. ";
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return retValue;
        }

        internal static int GetBalLicenseForCompany(WmsMobileCompany company)
        {
            int retValue = 0;
            //TODO get balance license for that company
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"SELECT (select NoOfLicense from WH_MOBILE_LICENSE_TBL 
                                                where GroupName ='{0}' and CompanyName = '{1}'
                                                    and DateFrom <= '{2}' and DateTo is NULL)
		                                         as NoOfLicenses";
                query = string.Format(query, CommonUtilities.FormatString(company.GroupName)
                                            , CommonUtilities.FormatString(company.CompanyName)
                                            , DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(DateTime.Today));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["NoOfLicenses"] == DBNull.Value)
                        throw new FMException("There is no valid license yet. Please add valid license. ");
                    else
                    {
                        int dbValue = Convert.ToInt32(FMEncryption.Decrypt((string)reader["NoOfLicenses"]));
                        retValue = Convert.ToInt32(FMEncryption.DecryptNumberOfLicense(dbValue));
                    }
                }
                reader.Close();    
                //return the number of license minus the registered mobile devices
                retValue = retValue - GetNoOfMobileDeviceInCompany(company);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }

            return retValue;
        }
        internal static int GetBalLicenseForGroup(WmsMobileCompany company)
        {
            int retValue = 0;
            //TODO get balance license for the group
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query =@"SELECT NoOfLicense as NoOfLicenses from WH_MOBILE_LICENSE_TBL 
                                                where GroupName ='{0}' and DateFrom <= '{1}' and DateTo is NULL";
                query = string.Format(query, CommonUtilities.FormatString(company.GroupName)
                                            , DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(DateTime.Today));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["NoOfLicenses"] == DBNull.Value)
                        throw new FMException("There is no valid license yet. Please add valid license. ");
                    else
                    {
                        int dbValue = Convert.ToInt32(FMEncryption.Decrypt((string)reader["NoOfLicenses"]));
                        retValue += Convert.ToInt32(FMEncryption.DecryptNumberOfLicense(dbValue));
                    }
                }
                reader.Close();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }

            return retValue;
        }
        internal static int GetNoOfMobileDeviceInCompany(WmsMobileCompany company)
        {
            int retValue = 0;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"select Count(*) NoOfMobileDevice from WH_Mobile_ID_Tbl where CompanyName ='" + company.CompanyName.Trim() + "'";
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = (int)reader["NoOfMobileDevice"];
                }
                reader.Close();
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }

            return retValue;
        }
        internal static int GetAvailableVehiclesInGroupOfCompanies(WmsMobileCompany company)
        {
            int retValue = 0;
            try
            {
                SortableList<WmsMobileCompany> companies = GetAllCompany(company.GroupName);
                //get total available vehicles for all companies in the group
                foreach (WmsMobileCompany tempCompany in companies)
                {
                    retValue += GetNoOfMobileDeviceInCompany(tempCompany);
                }
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }

            return retValue;
        }
        internal static bool AddMobileLicense(WmsMobileNoOfLicense mobileLicense, SqlConnection paramCon, SqlTransaction paramTran)
        {
            bool retValue = false;
            try
            {
                string query = @"insert into WH_MOBILE_LICENSE_TBL(GroupName,CompanyName, NoOfLicense, DateAdded, DateFrom)
                                    values('{0}','{1}','{2}','{3}','{4}')";

                query = string.Format(query, CommonUtilities.FormatString(mobileLicense.GroupName.Trim()),
                                            CommonUtilities.FormatString(mobileLicense.CompanyName.Trim()),
                                            CommonUtilities.FormatString(FMEncryption.EncryptNumberOfLicense(mobileLicense.numOfLicense)),
                                            DateUtility.ConvertDateAndTimeForSQLPurpose(DateTime.Now),
                                            DateUtility.ConvertDateForSQLPurpose(mobileLicense.validFrom.Date));


                if (paramCon.State == ConnectionState.Closed) { paramCon.Open(); }
                if (paramTran == null) { paramTran = paramCon.BeginTransaction(); }
                SqlCommand cmd = paramCon.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = paramTran;

                cmd.ExecuteNonQuery();
                //tran.Commit();
                retValue = true;
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            } 
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }
        internal static bool EditMobileLicense(WmsMobileNoOfLicense mobileLicense, SqlConnection paramCon, SqlTransaction paramTran)
        {
            bool retValue = false;
            try
            {
                //TODO edit no of licenses
                string query = @"update WH_MOBILE_LICENSE_TBL
                                     set NoOfLicense ='{3}', DateFrom ='{2}'        
                                     where GroupName = '{0}' and CompanyName = '{1}' and DateTo is null";

                query = string.Format(query, CommonUtilities.FormatString(mobileLicense.GroupName.Trim()),
                                            CommonUtilities.FormatString(mobileLicense.CompanyName.Trim()),
                                            DateUtility.ConvertDateForSQLPurpose(mobileLicense.validFrom.Date),
                                            CommonUtilities.FormatString(FMEncryption.EncryptNumberOfLicense(mobileLicense.numOfLicense)));


                if (paramCon.State == ConnectionState.Closed) { paramCon.Open(); }
                if (paramTran == null) { paramTran = paramCon.BeginTransaction(); }
                SqlCommand cmd = paramCon.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = paramTran;

                cmd.ExecuteNonQuery();
                //tran.Commit();
                retValue = true;
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            } 
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }  
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }
        internal static bool EditExpiryOfLicense(WmsMobileNoOfLicense mobileLicense, DateTime newExpiry, SqlConnection paramCon, SqlTransaction paramTran)
        {
            bool retValue = false;
            try
            {
                string query = @"update WH_MOBILE_LICENSE_TBL
                                     set DateTo ='{3}'
                                     where GroupName = '{0}' and CompanyName = '{1}' and DateFrom ='{2}'";

                query = string.Format(query, CommonUtilities.FormatString(mobileLicense.GroupName.Trim()),
                                            CommonUtilities.FormatString(mobileLicense.CompanyName.Trim()),
                                            DateUtility.ConvertDateForSQLPurpose(mobileLicense.validFrom.Date),
                                            DateUtility.ConvertDateForSQLPurpose(newExpiry));

                if (paramCon.State == ConnectionState.Closed) { paramCon.Open(); }
                if (paramTran == null) { paramTran = paramCon.BeginTransaction(); }
                SqlCommand cmd = paramCon.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = paramTran;

                cmd.ExecuteNonQuery();
                //tran.Commit();
                retValue = true;
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }
        internal static bool DeleteMobileLicense(WmsMobileCompany company, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO edit no of licenses
                string query = @"delete from WH_MOBILE_LICENSE_TBL
                                     where GroupName = '{0}' and CompanyName = '{1}'";

                query = string.Format(query, CommonUtilities.FormatString(company.GroupName.Trim()),
                                            CommonUtilities.FormatString(company.CompanyName.Trim()));

                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;

                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }

        internal static SortableList<WmsMobileCompany> GetAllCompany(string groupName)
        {
            SortableList<WmsMobileCompany> list = new SortableList<WmsMobileCompany>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            string query = string.Empty;
            try
            {
                if (groupName.Equals(string.Empty))
                    query = @"select * from WH_Mobile_Company_Tbl order by CompanyName";
                else
                {
                    query = @"select * from WH_Mobile_Company_Tbl where GroupName ='{0}' order by CompanyName";
                    query = string.Format(query, groupName);
                }

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    WmsMobileCompany company = new WmsMobileCompany();
                    company.GroupName = (string)reader["GroupName"];
                    company.CompanyName = (string)reader["CompanyName"];
                    company.GroupNameLocal = (string)reader["GroupName_Local"];
                    company.CompanyNameLocal = (string)reader["CompanyName_Local"];
                    company.DatabaseName = (string)reader["DatabaseName"];
                    company.SQLServer = (string)reader["ServerName"];
                    company.ShareWithGoup = ((string)reader["ShareWithGroup"] == "T") ? true : false;
                  
                    list.Add(company);
                }
                reader.Close();

            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return list;
        }
        internal static WmsMobileCompany GetCompanyByName(string companyName)
        {
            WmsMobileCompany company = null;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            string query = string.Empty;
            try
            {
                query = @"select * from TPT_Company_Tbl where CompanyName ='" + companyName + "'";//{0}'";

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    company = new WmsMobileCompany();
                    company.GroupName = (string)reader["GroupName"];
                    company.CompanyName = (string)reader["CompanyName"];
                    company.GroupNameLocal = (string)reader["GroupName_Local"];
                    company.CompanyNameLocal = (string)reader["CompanyName_Local"];
                    company.DatabaseName = (string)reader["DatabaseName"];
                    company.SQLServer = (string)reader["ServerName"];
                    company.ShareWithGoup = ((string)reader["ShareWithGroup"] == "T") ? true : false;
                   
                }
                reader.Close();
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return company;
        } 
        internal static WmsMobileNoOfLicense GetLatestLicenseForCompany(WmsMobileCompany company)
        {
            WmsMobileNoOfLicense retValue = null;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"Select * from WH_MOBILE_LICENSE_TBL as a
                                    where a.GroupName ='{0}' and a.CompanyName ='{1}'
                                           and (select count(*) from WH_MOBILE_LICENSE_TBL
                                                   where GroupName = a.GroupName
                                                        and CompanyName = a.CompanyName
	                                                    and DateFrom > a.DateFrom) < 1";
                query = string.Format(query, CommonUtilities.FormatString(company.GroupName)
                                            , CommonUtilities.FormatString(company.CompanyName));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int dbValue = Convert.ToInt32(FMEncryption.Decrypt((string)reader["NoOfLicense"]));

                    retValue = new WmsMobileNoOfLicense();
                    retValue.GroupName = (string)reader["GroupName"];
                    retValue.CompanyName = (string)reader["CompanyName"];
                    retValue.numOfLicense = Convert.ToInt32(FMEncryption.DecryptNumberOfLicense(dbValue));
                    retValue.validFrom = (DateTime)reader["DateFrom"];
                    retValue.validTo = reader["DateTo"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["DateTo"];
                }
                reader.Close();
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return retValue;
        }
        internal static SortableList<WmsMobileNoOfLicense> GetVehicleNoLicenses(WmsMobileCompany company)
        {
            SortableList<WmsMobileNoOfLicense> list = new SortableList<WmsMobileNoOfLicense>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"select * from WH_MOBILE_LICENSE_TBL 
                                    where GroupName ='{0}' and CompanyName ='{1}' 
                                    order by DateFrom desc";
                query = string.Format(query, CommonUtilities.FormatString(company.GroupName), CommonUtilities.FormatString(company.CompanyName));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int dbValue = Convert.ToInt32(FMEncryption.Decrypt((string)reader["NoOfLicense"]));

                    WmsMobileNoOfLicense vehicleNoLicense = new WmsMobileNoOfLicense();
                    vehicleNoLicense.GroupName = (string)reader["GroupName"];
                    vehicleNoLicense.CompanyName = (string)reader["CompanyName"];
                    vehicleNoLicense.numOfLicense = Convert.ToInt32(FMEncryption.DecryptNumberOfLicense(dbValue));
                    vehicleNoLicense.dateAdded = (DateTime)reader["DateAdded"];
                    vehicleNoLicense.validFrom = (DateTime)reader["DateFrom"];
                    vehicleNoLicense.validTo = reader["DateTo"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["DateTo"];
                    list.Add(vehicleNoLicense);
                }
                reader.Close();
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.Message.ToString().Substring(0, ex.Message.ToString().IndexOf(".")) + ". Please double check the database name");//.Substring(0, ex.Message.ToString().IndexOf(".")));
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }

            return list;
        }
        internal static ArrayList GetAllCompanyGroups()
        {
            ArrayList list = new ArrayList();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            string query = string.Empty;
            try
            {
                query = @"select distinct GroupName from WH_Mobile_Company_Tbl";


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((string)reader["GroupName"]);
                }
                reader.Close();

            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return list;
        }
        internal static ArrayList GetAllCompanyNames()
        {
            ArrayList list = new ArrayList();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            string query = string.Empty;
            try
            {
                query = @"select distinct CompanyName from WH_Mobile_Company_Tbl";


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((string)reader["CompanyName"]);
                }
                reader.Close();

            }
            catch (InvalidOperationException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (SqlException ex)
            {
                throw new FMException(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return list;
        }
     

    }
}
