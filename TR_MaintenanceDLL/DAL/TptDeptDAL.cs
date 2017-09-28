using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using TR_LanguageResource.Resources;

namespace FM.TR_MaintenanceDLL.DAL
{
    internal class TptDeptDAL 
    {
        public const string tblTruckJobMain = "TRK_JOB_MAIN_Tbl";
        public const string tblHaulierJobMain = "TPT_JOB_MAIN_Tbl";

        internal static SortableList<TptDept> GetAllTptDept()
        {
            SortableList<TptDept> tptDepts = new SortableList<TptDept>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_DEPT_TBL";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tptDepts.Add(GetTptDept(reader));
            }
            cn.Close();
            return tptDepts;
        }

        internal static SortableList<TptDept> GetAllTptDept(DeptType deptType)
        {
            SortableList<TptDept> tptDepts = new SortableList<TptDept>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_DEPT_TBL WHERE TPT_DEPT_TYPE='{0}'";

            SqlCommand cmd = new SqlCommand(string.Format(SQLString, (int)deptType), cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                tptDepts.Add(GetTptDept(reader));
            }
            cn.Close();
            return tptDepts;
        }

        internal static SortableList<TptDept> GetDeptType()
        {
            SortableList<TptDept> tptDepts = new SortableList<TptDept>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT DISTINCT TPT_DEPT_TYPE FROM TPT_DEPT_TBL";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tptDepts.Add(GetDeptType(reader));
            }
            cn.Close();
            return tptDepts;
        }
        
        internal static SortableList<TptDept> GetTptDeptByCode(string deptCode)
        {
            SortableList<TptDept> tptDepts = new SortableList<TptDept>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_DEPT_TBL ";
            if (!deptCode.Equals(string.Empty))
                SQLString += @" Where TPT_DEPT_CODE ='" + CommonUtilities.FormatString(deptCode) + "'";
            else
                SQLString += "";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tptDepts.Add(GetTptDept(reader));
            }
            cn.Close();
            return tptDepts;
        }

        internal static TptDept GetDeptType(IDataReader reader)
        {
            TptDept tptDept = new TptDept("","",
                    (DeptType)reader["TPT_DEPT_TYPE"]);
            return tptDept;
        }

        internal static TptDept GetTptDept(IDataReader reader)
        {
            TptDept tptDept = new TptDept(
                    (string)reader["TPT_DEPT_CODE"],
                    (string)reader["TPT_DEPT_NAME"],
                    (DeptType)reader["TPT_DEPT_TYPE"]
                    );
            return tptDept;
        }
        
        internal static bool AddTptDept(TptDept tptDept, SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            try
            {
                //Gerry added 1 field (TPT_DEPT_TYPE column)
                string SQLInstertString = @"INSERT INTO TPT_DEPT_TBL (TPT_DEPT_CODE,TPT_DEPT_NAME,TPT_DEPT_TYPE) 
                                                VALUES ('{0}','{1}','{2}')";

                SqlCommand cmd = new SqlCommand(string.Format(SQLInstertString,
                                            CommonUtilities.FormatString(tptDept.TptDeptCode.Trim()),
                                            CommonUtilities.FormatString(tptDept.TptDeptName),
                                            (int)tptDept.TptDeptType), cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrInsertFailed + " Department. \n"+ex.ToString());
            }

            return true;

        }

        internal static bool EditTptDept(TptDept tptDept, SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            try
            {
                //Gerry added 1 field (TPT_DEPT_TYPE column)
                string SQLString = @"Update TPT_DEPT_TBL 
                                                    set TPT_DEPT_NAME ='{1}',
                                                        TPT_DEPT_TYPE ='{2}'
                                                    Where TPT_DEPT_CODE ='{0}'";

                SqlCommand cmd = new SqlCommand(string.Format(SQLString, 
                                                CommonUtilities.FormatString(tptDept.TptDeptCode.Trim()), 
                                                CommonUtilities.FormatString(tptDept.TptDeptName),
                                                (int)tptDept.TptDeptType), cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new FMException(TptResourceDAL.ErrEditFailed + "Department. \n" + ex.ToString());
            }

            return true;
        }

        internal static bool DeleteTptDept(TptDept tptDept, SqlConnection cn, SqlTransaction tran)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            try
            {
                string SQLString = @"Delete From TPT_DEPT_TBL                                                     
                                                    Where TPT_DEPT_CODE ='{0}'";

                SqlCommand cmd = new SqlCommand(string.Format(SQLString, CommonUtilities.FormatString(tptDept.TptDeptCode.Trim())), cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new FMException(TptResourceDAL.ErrDeleteFailed + "Department. \n" + ex.ToString());                
            }           
        }

        internal static bool IsTptDeptUsed(TptDept tptDept)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            string tableName ="";
            if (tptDept.TptDeptType == DeptType.Trucking)
                tableName = tblTruckJobMain;
            else
                tableName = tblHaulierJobMain;

            string SQLString = @"Select count(*) from {1}
                                    Where TPT_DEPT_CODE= '{0}'";

            SqlCommand cmd = new SqlCommand(string.Format(SQLString, CommonUtilities.FormatString(tptDept.TptDeptCode), tableName), con);
            int value= (int)cmd.ExecuteScalar();
            if (value > 0)
            {
                return true;
                throw new FMException(TptResourceDAL.ErrEditFailed + "Department. \n");
            }
            else
                return false;

        }

        internal static bool ValidateAddDept(TptDept tptDept)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            string SQLString = @"Select count(*) from TPT_DEPT_TBL
                                    Where TPT_DEPT_CODE= '{0}'";

            SqlCommand cmd = new SqlCommand(string.Format(SQLString, CommonUtilities.FormatString(tptDept.TptDeptCode.Trim())), con);
            int value = (int)cmd.ExecuteScalar();
            if (value > 0)
            {
                return false;
                throw new FMException(TptResourceDAL.ErrEditFailed + "Department. \n");
            }
            else
                return true;
        }

        
    }
}
