using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using TR_LicenseDLL.DAL.WmsMobile;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using System.Data;

namespace TR_LicenseDLL.BLL.WmsMobile
{
    public class WmsMobileUser  : User
    {               
        public string Password { get; set; }
        public string Phone { get; set; }


        public WmsMobileUser():base()
        {
            this.Password = "";
            this.Phone = "";
        }

        public WmsMobileUser(string userId, string password, BranchDTO branch, string firsName, string lastName, string phone)
            : base(userId, firsName, lastName, branch, 1)
        {
            this.Password = password;
            this.Phone = phone;
        }

        public static bool IsMobilerUserValid(String userId, string password, out string outMsg)
        {
            bool retValue = false;
            outMsg = "";
            try
            {
               retValue = WmsMobileUserDAL.IsMobileUserValid(userId, password);
            }
            catch (FMException fmEx)
            {
                outMsg = fmEx.ToString();
                throw fmEx;
            }
            catch (Exception ex)
            {
                outMsg = ex.ToString();
                throw ex;
            }
            return retValue;
        }
        public bool AddMobilerUser()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (!IsMobileUserExist(this.UserID, this.Password))
                {
                    retValue = WmsMobileUserDAL.AddMobileUser(this, con, tran);

                    tran.Commit();
                }
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }
            return retValue;
        }
        public bool ChangePassword()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (!IsUserStillUsing())
                {
                    retValue = WmsMobileUserDAL.ChangePassword(this, con, tran);

                    tran.Commit();
                }
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }
            return retValue;
        }
        public bool EditMobilerUser()
        {
            bool retValue = false;
            string outMsg ="";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (IsMobilerUserValid(this.UserID,this.Password, out outMsg))
                {
                    retValue = WmsMobileUserDAL.EditMobileUser(this, con, tran);

                    tran.Commit();
                }
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }
            return retValue;
        }
        public bool DeleteMobilerUser()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (!IsUserStillUsing())
                {
                    retValue = WmsMobileUserDAL.DeleteMobileUser(this, con, tran);

                    tran.Commit();
                }
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally { con.Close(); }
            return retValue;
        }
        public bool IsUserStillUsing()
        {
            bool retValue = false;
            try
            {
                retValue = WmsMobileUserDAL.IsUserStillUsing(this);
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

        public static SortableList<WmsMobileUser> GetAllMobileUsers()
        {
            try
            {
                return WmsMobileUserDAL.GetAllMobileUsers();
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool IsMobileUserExist(string userId, string password)
        {
            bool retValue = false;
            try
            {
                retValue = WmsMobileUserDAL.IsMobileUserExist(userId, password);
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
    }
}
