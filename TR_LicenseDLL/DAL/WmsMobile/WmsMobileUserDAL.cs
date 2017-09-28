using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using System.Data;
using TR_LanguageResource.Resources;
using TR_LicenseDLL.BLL.WmsMobile;
using Encryption;

namespace TR_LicenseDLL.DAL.WmsMobile
{
    public class WmsMobileUserDAL
    {
        internal static bool IsMobileUserValid(string userId, string password)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                //TODO validate userid and password
                string encryptedPassword = FMEncryption.Encrypt(password);
                string query = @"Select * from WH_Mobile_Users_Tbl
                                    where UserID ='{0}' and UserPassword ='{1}'";
                query = string.Format(query, CommonUtilities.FormatString(userId)
                                            , CommonUtilities.FormatString(encryptedPassword.Trim()));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (encryptedPassword.Trim() == (string)reader["UserPassword"].ToString().Trim()
                        || userId.Trim() == (string)reader["UserID"].ToString().Trim())
                    {
                        retValue = true;
                        break;
                    }
                }
                reader.Close();
                if (!retValue)
                {
                    throw new FMException(WmsResourceBLL.ErrInvalidUser);
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
        internal static bool AddMobileUser(WmsMobileUser mobileUser, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO add mobile user
                string query = @"insert into WH_Mobile_Users_Tbl
                                    values('{0}','{1}','{2}','{3}','{4}','{5}')";

                query = string.Format(query, CommonUtilities.FormatString(mobileUser.UserID),
                                             CommonUtilities.FormatString(FMEncryption.Encrypt("password")), //set default userPassword to 'password' then allow user to edit
                                            CommonUtilities.FormatString(mobileUser.FirstName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(mobileUser.LastName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(mobileUser.Phone),
                                            CommonUtilities.FormatString(mobileUser.DefaultBranch.Code));


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
        internal static bool EditMobileUser(WmsMobileUser mobileUser, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO edit mobile user
                string query = @"update WH_Mobile_Users_Tbl
                                     set FirstName ='{2}'
                                        , LastName ='{3}'
                                        , Phone = '{4}'
                                        , DefaultBranch = '{5}'
                                     where UserID = '{0}' and UserPassword = '{1}'";

                query = string.Format(query, CommonUtilities.FormatString(mobileUser.UserID),
                                            CommonUtilities.FormatString(FMEncryption.Encrypt(mobileUser.Password.Trim())),
                                            CommonUtilities.FormatString(mobileUser.FirstName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(mobileUser.LastName.Trim().ToUpper()),
                                            CommonUtilities.FormatString(mobileUser.Phone),
                                            CommonUtilities.FormatString(mobileUser.DefaultBranch.Code));


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
        internal static bool DeleteMobileUser(WmsMobileUser mobileUser, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO edit mobile user
                string query = @"delete from WH_Mobile_Users_Tbl
                                    where userID = '{0}'";

                query = string.Format(query, CommonUtilities.FormatString(mobileUser.UserID));

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
        internal static bool ChangePassword(WmsMobileUser mobileUser, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                //TODO change password mobile user
                string query = @"update WH_Mobile_Users_Tbl
                                     set UserPassword = '{1}'
                                     where UserID = '{0}'";

                query = string.Format(query, CommonUtilities.FormatString(mobileUser.UserID),
                                            CommonUtilities.FormatString(FMEncryption.Encrypt(mobileUser.Password.Trim())));


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
        internal static bool IsUserStillUsing(WmsMobileUser mobileUser)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                //TODO validate user still using
                string query = @"select distinct *, 'GRN' TrxType from WH_MOBILE_GRN_DOWNLOAD
                                        where User_ID = '{0}' and status = 'DO'

                                        union all

                                select distinct *, 'GIN' TrxType from WH_MOBILE_GIN_DOWNLOAD
                                        where User_ID = '{0}' and status = 'DO'";

                query = string.Format(query, CommonUtilities.FormatString(mobileUser.UserID));
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
                    throw new FMException(WmsResourceBLL.ErrUserHavePendingTransaction + errMsg);
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


        internal static SortableList<WmsMobileUser> GetAllMobileUsers()
        {
            SortableList<WmsMobileUser> list = new SortableList<WmsMobileUser>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string query = @"select * from WH_Mobile_Users_Tbl order by UserID";
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    BranchDTO branch = new BranchDTO();
                    if (reader["DefaultBranch"] != DBNull.Value)
                    {
                        branch = BranchDTO.GetBranch((string)reader["DefaultBranch"]);
                    }
                    string phone = "";
                    if (reader["Phone"] != DBNull.Value)
                    {
                        phone = (string)reader["Phone"];
                    }
                    string password = "";
                    if (reader["UserPassword"] != DBNull.Value)
                    {
                        password = FMEncryption.Decrypt((string)reader["UserPassword"]);
                    }
                    WmsMobileUser mobileUser = new WmsMobileUser(
                    (string)reader["UserID"],
                    password,
                    branch,
                    (string)reader["FirstName"],
                    (string)reader["LastName"],
                    phone
                    );
                    list.Add(mobileUser);
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
        internal static bool IsMobileUserExist(string userId, string password)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string encryptedPassword = FMEncryption.Encrypt(password.Trim());
                //TODO validate userid and password
                string query = @"Select * from WH_Mobile_Users_Tbl
                                    where UserID ='{0}' and UserPassword = '{1}'";
                query = string.Format(query, CommonUtilities.FormatString(userId.Trim())
                                            , CommonUtilities.FormatString(encryptedPassword.Trim()));
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (encryptedPassword.Trim().Equals((string)reader["UserPassword"].ToString().Trim())
                        && userId.Trim().Equals((string)reader["UserID"].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new FMException(WmsResourceBLL.ErrMobileUserExist);
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
    }
}
