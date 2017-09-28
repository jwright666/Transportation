using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using Encryption;
using System.Data;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using TR_LicenseDLL.BLL.Transport;
using System.Collections;

namespace TR_LicenseDLL.DAL.Transport
{
    public class TPTCompanyLicenseDAL
    {

        internal static bool AddNewCompany(TPTCompany company, SqlConnection paramCon, SqlTransaction paramTran)
        { 
            bool retValue = false;
            try
            {
                string query = @"insert into TPT_COMPANY_Tbl
                                    values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";

                query = string.Format(query,
                                            CommonUtilities.FormatString(company.GroupName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(company.CompanyName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(FMEncryption.Encrypt(company.GroupName.Trim().ToUpper())),
                                            CommonUtilities.FormatString(FMEncryption.Encrypt(company.CompanyName.Trim().ToUpper())),
                                            CommonUtilities.FormatString(company.GroupNameLocal.Trim().ToUpper()),
                                            CommonUtilities.FormatString(company.CompanyNameLocal.Trim().ToUpper()),
                                            CommonUtilities.FormatString(company.DatabaseName.Trim()),
                                            CommonUtilities.FormatString(company.SQLServer.Trim()),
                                            company.ShareWithGoup ? "T" : "F");

                if (paramCon.State == ConnectionState.Closed) { paramCon.Open(); }
                if (paramTran == null) { paramTran = paramCon.BeginTransaction(); }
                SqlCommand cmd = paramCon.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = paramTran;

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
        //get available License in the company
        internal static int GetBalNoOfLicenseForCompany(TPTCompany company, DateTime appDate)
        {
            int retValue = 0;
            //TODO get balance license for each company 
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"SELECT (select NoOfLicense from TPT_VEHICLE_LICENSE_TBL 
                                                where GroupName ='{0}' and CompanyName = '{1}'
                                                    and DateFrom <= '{2}' and DateTo is NULL)
		                                         as NoOfLicenses";
                query = string.Format(query, CommonUtilities.FormatString(company.GroupName)
                                            , CommonUtilities.FormatString(company.CompanyName)
                                            , DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(appDate));
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
                //return the number of license minus the available vehicles
                retValue = retValue - GetAvailableVehiclesInCompany(company, appDate);
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
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
            }

            return retValue;
        }
        //get number of available vehicles in the company  
        //20140326 - gerry added parameter appDate
        internal static int GetAvailableVehiclesInCompany(TPTCompany company, DateTime appDate)
        {
            int retValue = 0;
            //20140320 - gerry modified
            //using different databases, so connection string will be different based on the company
            FMGlobalSettings.TheInstance.GetSQLUserPassword();
            string conString = @"Data Source= '{0}'; Initial Catalog='{1}';  User Id = '{2}'; password = '{3}'";
            conString = string.Format(conString, company.SQLServer, company.DatabaseName, FMGlobalSettings.TheInstance.sqlUser, FMGlobalSettings.TheInstance.sqlPassword);

            SqlConnection con = new SqlConnection(conString);
            try
            {
                //20140324 - gerry change the query to count from planning instead of maintenance table
                //string query = @"SELECT Count(*) as AvailableVehicles FROM TPT_Vehicle_Tbl
                //                  where Is_Available = 'T'
                //                    and Vehicle_Type <> 2";
                string query = @"select COUNT(*) ASSIGNED_DRIVER from TPT_PLAN_DEPT_DRIVER_TBL
                                        where SCHEDULE_DATE = '{0}' ";
                query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(appDate));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {           //20140326 - gerry replaced
                    retValue = (int)reader["ASSIGNED_DRIVER"];// (int)reader["AvailableVehicles"];
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
        //get available License in the group of companies 
        //20140326 - gerry added parameter appDate
        internal static int GetBalNoOfLicenseForGroup(TPTCompany company, DateTime appDate)
        {
            int retValue = 0;
            //TODO get balance license for the group 
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"SELECT NoOfLicense as NoOfLicenses from TPT_VEHICLE_LICENSE_TBL 
                                                where GroupName ='{0}' and DateFrom <= '{1}' and DateTo is NULL
		                                         ";
                query = string.Format(query, CommonUtilities.FormatString(company.GroupName)
                                            , DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(appDate));
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
                //return the number of license in group minus the available vehicles in all companies
                retValue = retValue - GetAvailableVehiclesInGroupOfCompanies(company, appDate);
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
            finally
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
            }

            return retValue;
        }
        //get number of available vehicles in all companies within the group
        //20140326 - gerry added parameter appDate
        internal static int GetAvailableVehiclesInGroupOfCompanies(TPTCompany company, DateTime appDate)
        {
            int retValue = 0;
            try
            {
                SortableList<TPTCompany> companies = GetAllCompany(company.GroupName);
                //get total available vehicles for all companies in the group
                foreach (TPTCompany tempCompany in companies)
                {
                    retValue += GetAvailableVehiclesInCompany(tempCompany, appDate);
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

        internal static bool DeleteCompany(TPTCompany company, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO edit no of licenses
                string query = @"delete from TPT_Company_Tbl
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
        //this method will be called during license registration
        internal static bool IsCompanyValid(TPTCompany company)
        {
            bool retValue = true;
            string[] param = new string[3];
            param[0] = "ipl";
            param[1] = company.SQLServer.ToString().Trim();
            param[2] = company.DatabaseName.ToString().Trim();
            FMGlobalSettings.TheInstance.SetConnectionString(param, false);
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
        //this method will be called from vehicle class in transport maintenance
        internal static bool IsCompanyValid(string companyName, string server)
        {
            bool retValue = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"Select * from TPT_Company_Tbl";

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (companyName.Trim().ToUpper() != (string)reader["CompanyName"].ToString().Trim().ToUpper())
                    {
                        retValue = false;
                        break;
                    }
                }
                reader.Close();
                if (!retValue)
                {
                    throw new FMException("Sorry! your company have no valid license to use vehicle(s). Please contact support@innosys.com.sg to register or renew your license.  ");
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
            finally { con.Close(); }
            return retValue;
        }

        internal static TPTCompany GetCompanyByName(string companyName, string databaseName)
        {
            TPTCompany company = null;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
             string query = string.Empty;
            try
            {
                query = @"select * from TPT_Company_Tbl where CompanyName ='{0}' and DatabaseName = '{1}'";
                query = string.Format(query, companyName, databaseName);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    company = new TPTCompany();
                    company.GroupName = (string)reader["GroupName"];
                    company.CompanyName = (string)reader["CompanyName"];
                    company.GroupNameLocal = (string)reader["GroupNameLocal"];
                    company.CompanyNameLocal = (string)reader["CompanyNameLocal"];
                    company.DatabaseName = (string)reader["DatabaseName"];
                    company.SQLServer = (string)reader["SQLServer"];
                    company.ShareWithGoup = ((string)reader["ShareWithGroup"] == "T") ? true : false;
                }
                if (company != null) //20131120 gerry added validation in case use other database.
                {
                    company.vehicleNoLicenses = GetVehicleNoLicenses(company);
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
        internal static SortableList<TPTCompany> GetAllCompany(string groupName)
        {
            SortableList<TPTCompany> list = new SortableList<TPTCompany>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            string query = string.Empty;
            try
            {
                if (groupName.Equals(string.Empty))
                    query = @"select * from TPT_Company_Tbl order by CompanyName";
                else
                {
                    query = @"select * from TPT_Company_Tbl where GroupName ='{0}' order by CompanyName";
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
                    TPTCompany company = new TPTCompany();
                    company.GroupName = (string)reader["GroupName"];
                    company.CompanyName = (string)reader["CompanyName"];
                    company.GroupNameLocal = (string)reader["GroupNameLocal"];
                    company.CompanyNameLocal = (string)reader["CompanyNameLocal"];
                    company.DatabaseName = (string)reader["DatabaseName"];
                    company.SQLServer = (string)reader["SQLServer"];
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

        internal static bool AddVehicleNoLicense(TPTVehicleNoOfLicense vehicleNoLicense, SqlConnection paramCon, SqlTransaction paramTran)
        {
            bool retValue = false;
            try
            {
                string query = @"insert into TPT_VEHICLE_LICENSE_Tbl(GroupName,CompanyName, NoOfLicense, DateAdded, DateFrom)
                                    values('{0}','{1}','{2}','{3}','{4}')";

                query = string.Format(query, CommonUtilities.FormatString(vehicleNoLicense.GroupName.Trim()),
                                            CommonUtilities.FormatString(vehicleNoLicense.CompanyName.Trim()),
                                            CommonUtilities.FormatString(FMEncryption.EncryptNumberOfLicense(vehicleNoLicense.numOfLicense)),
                                            DateUtility.ConvertDateAndTimeForSQLPurpose(DateTime.Now),
                                            DateUtility.ConvertDateForSQLPurpose(vehicleNoLicense.validFrom.Date));


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
        internal static bool EditVehicleNoLicense(TPTVehicleNoOfLicense vehicleNoLicense, SqlConnection paramCon, SqlTransaction paramTran)
        {
            bool retValue = false;
            try
            {
                //TODO edit no of licenses
                string query = @"update TPT_VEHICLE_LICENSE_Tbl
                                     set NoOfLicense ='{3}', DateFrom ='{2}'        
                                     where GroupName = '{0}' and CompanyName = '{1}' and DateTo is null";

                query = string.Format(query, CommonUtilities.FormatString(vehicleNoLicense.GroupName.Trim()),
                                            CommonUtilities.FormatString(vehicleNoLicense.CompanyName.Trim()),
                                            DateUtility.ConvertDateForSQLPurpose(vehicleNoLicense.validFrom.Date),
                                            CommonUtilities.FormatString(FMEncryption.EncryptNumberOfLicense(vehicleNoLicense.numOfLicense)));


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
        internal static bool EditExpiryOfLicense(TPTVehicleNoOfLicense vehicleNoLicense, DateTime newExpiry, SqlConnection paramCon, SqlTransaction paramTran)
        {
            bool retValue = false;
            try
            {
                string query = @"update TPT_VEHICLE_LICENSE_Tbl
                                     set DateTo ='{3}'
                                     where GroupName = '{0}' and CompanyName = '{1}' and DateFrom ='{2}'";

                query = string.Format(query, CommonUtilities.FormatString(vehicleNoLicense.GroupName.Trim()),
                                            CommonUtilities.FormatString(vehicleNoLicense.CompanyName.Trim()),
                                            DateUtility.ConvertDateForSQLPurpose(vehicleNoLicense.validFrom.Date),
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
        internal static bool DeleteVehicleNoLicense(TPTCompany company, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO edit no of licenses
                string query = @"delete from TPT_VEHICLE_LICENSE_Tbl
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

        internal static TPTVehicleNoOfLicense GetLatestLicenseForCompany(TPTCompany company)
        {
            TPTVehicleNoOfLicense retValue = null;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"Select * from TPT_VEHICLE_LICENSE_TBL as a
                                    where a.GroupName ='{0}' and a.CompanyName ='{1}'
                                           and (select count(*) from TPT_VEHICLE_LICENSE_TBL
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

                    retValue = new TPTVehicleNoOfLicense();
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

        internal static SortableList<TPTVehicleNoOfLicense> GetVehicleNoLicenses(TPTCompany company)
        {
            SortableList<TPTVehicleNoOfLicense> list = new SortableList<TPTVehicleNoOfLicense>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"select * from TPT_VEHICLE_LICENSE_TBL 
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

                    TPTVehicleNoOfLicense vehicleNoLicense = new TPTVehicleNoOfLicense();
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
        internal static ArrayList GetAllCompanyGroup()
        {
            ArrayList list = new ArrayList();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            string query = string.Empty;
            try
            {
                query = @"select distinct GroupName from TPT_Company_Tbl";
              

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


    }
}
