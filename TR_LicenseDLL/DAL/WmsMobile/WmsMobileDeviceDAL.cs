using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using Encryption;
using FM.TR_FMSystemDLL.BLL;
using System.Data;
using TR_LicenseDLL.BLL.WmsMobile;
using TR_LanguageResource.Resources;

namespace TR_LicenseDLL.DAL.WmsMobile
{
    public class WmsMobileDeviceDAL
    {
        internal static bool IsAndroidIDValid(string androidID, string companyName, out string errMsg)
        {
            errMsg = "";
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string encryptedAndroidID = FMEncryption.Encrypt(androidID.Trim().ToUpper());
                //TODO validate userid and password
                string query = @"Select * from WH_Mobile_ID_Tbl
                                    where Android_ID_EC ='{0}' and CompanyName = '{1}'";
                query = string.Format(query, CommonUtilities.FormatString(encryptedAndroidID.Trim()), CommonUtilities.FormatString(companyName.Trim()));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (encryptedAndroidID.Trim() == (string)reader["Android_ID_EC"].ToString().Trim()
                        && companyName.Trim() == (string)reader["CompanyName"].ToString().Trim())
                    {
                        retValue = true; break;
                    }
                }
                reader.Close();
                if (!retValue)
                {
                    errMsg = "Sorry! Cannot use unregistered device. Please contact innosys staff to register your device. ";
                }
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return retValue;
        }
        internal static bool AddMobileDevice(WmsMobileDevice mobileDevice, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO add mobile device
                string query = @"insert into WH_Mobile_ID_Tbl
                                    values('{0}','{1}','{2}','{3}','{4}','{5}')";

                query = string.Format(query, CommonUtilities.FormatString(mobileDevice.CompanyName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(mobileDevice.AndroidID.Trim().ToUpper()),
                                            CommonUtilities.FormatString(FMEncryption.Encrypt(mobileDevice.AndroidID.Trim())),
                                            CommonUtilities.FormatString(mobileDevice.Brand.Trim().ToUpper()),
                                            CommonUtilities.FormatString(mobileDevice.Model.Trim().ToUpper()),
                                            CommonUtilities.FormatString(mobileDevice.DeviceName.Trim().ToUpper()));


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
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }
        internal static bool DeleteMobileDevice(WmsMobileDevice mobileDevice, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO edit mobile device
                string encryptedDeviceID = FMEncryption.Encrypt(mobileDevice.AndroidID);
                string query = @"delete from WH_Mobile_ID_Tbl
                                    where CompanyName = '{0}' and Android_ID_EC ='{1}'";

                query = string.Format(query, CommonUtilities.FormatString(mobileDevice.CompanyName.Trim()), CommonUtilities.FormatString(encryptedDeviceID.Trim()));

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
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }
        internal static bool EditMobileDevice(WmsMobileDevice mobileDevice, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO edit mobile device
                string encryptedDeviceID = FMEncryption.Encrypt(mobileDevice.AndroidID);
                string query = @"update WH_Mobile_ID_Tbl
                    set Brand ='{2}', Model ='{3}', DeviceName ='{4}'
                                    where CompanyName = '{0}' and Android_ID_EC ='{1}'";

                query = string.Format(query, CommonUtilities.FormatString(mobileDevice.CompanyName.Trim())
                                            , CommonUtilities.FormatString(encryptedDeviceID.Trim())
                                            , CommonUtilities.FormatString(mobileDevice.Brand)
                                            , CommonUtilities.FormatString(mobileDevice.Model)
                                            , CommonUtilities.FormatString(mobileDevice.DeviceName));

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
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }


        internal static bool IsMobileDeviceBeingUsed(WmsMobileDevice mobileDevice)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                //TODO validate user still using
                string query = @"select distinct *, 'GRN' TrxType from WH_MOBILE_GRN_DOWNLOAD
                                        where Android_ID = '{0}' and status = 'DO'

                                        union all

                                        select distinct *, 'GIN' TrxType from WH_MOBILE_GIN_DOWNLOAD
                                        where Android_ID = '{0}' and status = 'DO'";

                query = string.Format(query, CommonUtilities.FormatString(mobileDevice.AndroidID.Trim()));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;

                SqlDataReader reader = cmd.ExecuteReader();
                string errMsg = "";
                while (reader.Read())
                {
                    errMsg += "\n" + (string)reader["TrxType"] + "\tTRX NO : " + (int)reader["TrxKey"] + "\tDATE DOWNLOAD : " + (DateTime)reader["DateTime_Download"];
                }
                reader.Close();
                if (!errMsg.Trim().Equals(""))
                {
                    throw new FMException(WmsResourceBLL.ErrDeviceHavePendingTransaction + errMsg);
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
            finally { con.Close(); }
            return retValue;
        }

        internal static SortableList<WmsMobileDevice> GetAllMobileDevices()
        {
            SortableList<WmsMobileDevice> list = new SortableList<WmsMobileDevice>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"select * from WH_Mobile_ID_Tbl order by Android_ID,CompanyName";
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    WmsMobileDevice mobileDevice = new WmsMobileDevice();
                    mobileDevice.CompanyName = (string)reader["CompanyName"];
                    mobileDevice.AndroidID = (string)reader["Android_ID"];
                    mobileDevice.Brand = (string)reader["Brand"];
                    mobileDevice.Model = (string)reader["Model"];
                    mobileDevice.DeviceName = (string)reader["DeviceName"];

                    list.Add(mobileDevice);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return list;
        }
        internal static SortableList<WmsMobileDevice> GetAllMobileDevicesByCompany(string companyName)
        {
            SortableList<WmsMobileDevice> list = new SortableList<WmsMobileDevice>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string query = @"select * from WH_Mobile_ID_Tbl where CompanyName = '{0}' order by Android_ID";
                query = string.Format(query, CommonUtilities.FormatString(companyName));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    WmsMobileDevice mobileDevice = new WmsMobileDevice();
                    mobileDevice.CompanyName = (string)reader["CompanyName"];
                    mobileDevice.AndroidID = (string)reader["Android_ID"];
                    mobileDevice.Brand = (string)reader["Brand"];
                    mobileDevice.Model = (string)reader["Model"];
                    mobileDevice.DeviceName = (string)reader["DeviceName"];

                    list.Add(mobileDevice);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return list;
        }
        internal static bool IsMobileDeviceExist(WmsMobileDevice mobileDevice)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string encryptedAndroidID = FMEncryption.Encrypt(mobileDevice.AndroidID.Trim());
                //TODO validate userid and password
                string query = @"Select * from WH_Mobile_ID_Tbl
                                    where Android_ID_EC ='{0}' and CompanyName = '{1}'";
                query = string.Format(query, CommonUtilities.FormatString(encryptedAndroidID.Trim()), CommonUtilities.FormatString(mobileDevice.CompanyName.Trim()));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (encryptedAndroidID.Trim().Equals((string)reader["Android_ID_EC"].ToString().Trim())
                        && mobileDevice.CompanyName.Trim().Equals((string)reader["CompanyName"].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new FMException(WmsResourceBLL.ErrDeviceAlreadyExist);
                    }
                }
                reader.Close();
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }
            return retValue;
        }
        internal static bool IsMobileDeviceCompanyValid(WmsMobileDevice mobileDevice)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.GetPubsConnectionString());
            try
            {
                string encryptedAndroidID = FMEncryption.Encrypt(mobileDevice.AndroidID.Trim());
                //TODO validate userid and password
                string query = @"Select * from WH_Mobile_Company_Tbl
                                    where CompanyName = '{0}'";
                query = string.Format(query, CommonUtilities.FormatString(mobileDevice.CompanyName.Trim()));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (mobileDevice.CompanyName.Trim().Equals((string)reader["CompanyName"].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                }
                reader.Close();
                if (!retValue)
                {
                    throw new FMException(WmsResourceBLL.ErrInvalidCompanyName);
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
            finally { con.Close(); }
            return retValue;
        }
    }
}
