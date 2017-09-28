using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_MaintenanceDLL.DAL;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using TR_LanguageResource.Resources;


namespace FM.TR_MaintenanceDLL.BLL
{
    public class TptDept
    {
        private string tptDeptCode;
        private string tptDeptName;
        private DeptType tptDeptType;
        

        public string TptDeptCode
        {
            get { return tptDeptCode; }
            set {
                value = value.Trim();
                if (!value.Equals(string.Empty))
                    tptDeptCode = value;

                else
                    throw new FMException(TptResourceBLL.ErrDeptCodeEmpty);
            }
           
        }

        public string TptDeptName
        {
            get { return tptDeptName; }
            set
            {
                value = value.Trim();
                if (!value.Equals(string.Empty))
                    tptDeptName = value;

                else
                    throw new FMException(TptResourceBLL.ErrDeptNameEmpty);
            }
        }

        public DeptType TptDeptType
        {
            get { return tptDeptType; }
            set 
            {
                if (value >= 0)
                    tptDeptType = value;

                else
                    throw new FMException(TptResourceBLL.ErrDeptTypeEmpty);
            }
        }

        public TptDept()
        {
            this.tptDeptCode = "";
            this.tptDeptName = "";
            this.tptDeptType = TptDeptType;
        }
        
        public TptDept(string tptDeptCode, string tptDeptName, DeptType deptType)
        {
            this.TptDeptCode = tptDeptCode;
            this.TptDeptName = tptDeptName;
            this.TptDeptType = deptType;
        }

        public static SortableList<TptDept> GetAllTptDepts()
        {
            return TptDeptDAL.GetAllTptDept();
        }

        public static SortableList<TptDept> GetAllTptDeptsByCode(string deptCode)
        {
            return TptDeptDAL.GetTptDeptByCode(deptCode);
        }

        public static SortableList<TptDept> GetAllTptDepts(DeptType deptType)
        {
            return TptDeptDAL.GetAllTptDept(deptType);
        }

        public static SortableList<TptDept> GetDeptType()
        {
            return TptDeptDAL.GetDeptType();
        }

        public bool AddTptDept(out string message)
        {
            bool value = false;
            message = "";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            if (ValidateAddDept())
            {
                try
                {
                    value = TptDeptDAL.AddTptDept(this, con, tran);
                    message = "Save Successfully";
                    tran.Commit();
                }

                catch (FMException ex)
                {
                    tran.Rollback();
                    message = ex.ToString();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    message = ex.ToString();
                }
                finally
                {
                    con.Close();
                }
            }
            else
                message = TptResourceBLL.ErrDeptCodeExist;
            
            return value;
        }

        public bool EditTptDept(out string message)
        {
            message = "";
            bool value = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {  
                value = TptDeptDAL.EditTptDept(this, con, tran);
                tran.Commit();
                message = CommonResource.EditSuccess;
               
            }
            catch (FMException ex)
            {
                tran.Rollback();
                message = ex.ToString();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                message = ex.ToString();
            }
            finally
            {
                con.Close();
            }
            return value;
        }

        public bool DeleteTptDept(out string message)
        {
            bool value = false;
            message = "";
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            if (!IsTptDeptUsed())
            {
                try
                {
                    value = TptDeptDAL.DeleteTptDept(this, con, tran);
                    tran.Commit();
                    message = CommonResource.DeleteSuccess;

                }
                catch (FMException ex)
                {
                    value = false;
                    tran.Rollback();
                    message = ex.ToString();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    message = ex.ToString();
                }
                finally
                {
                    con.Close();
                }
            }
            else
                message = TptResourceBLL.ErrDeptDelete;

            con.Close();
            return value;           
            
        }

        public bool ValidateAddDept()
        {
            return TptDeptDAL.ValidateAddDept(this);
        }

        public bool IsTptDeptUsed()
        {
            bool value = false;
            try
            {
                value = TptDeptDAL.IsTptDeptUsed(this);

            }
            catch (FMException ex)
            {
                throw ex;
            }
            return value;
        } 
    }
}
