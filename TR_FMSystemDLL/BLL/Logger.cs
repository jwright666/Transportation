using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class Logger
    {
        // Delegates call to LoggerDAL. LoggerDAL will write both LogHeader and LogDetail
        public static bool WriteLog(LogHeader logHeader, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                LoggerDAL.WriteLog(logHeader, con, tran);
                return true;
            }
            catch (FMException Ex)
            {
                throw Ex;
            }
        }

        public static DateTime GetServerDateTime()
        {
            return LoggerDAL.GetServerDateTime();
        }
    }
}
