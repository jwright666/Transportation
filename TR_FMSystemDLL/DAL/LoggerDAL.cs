using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.DAL
{
    public class LoggerDAL
    {
        //20151028 - gerry added for the new auditlog format
        internal static bool WriteAuditLog(AuditLog auditLog, object field, object newValue, object oldValue, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                IncrementLogID(con, tran);
                auditLog.LogNo = GetNextLogID(con, tran);
                SqlCommand cmd = new SqlCommand("sp_Insert_SEC_Audit_Log_Transport_Tbl", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Trx_Type", CommonUtilities.FormatString(auditLog.TrxType));
                cmd.Parameters.AddWithValue("@Entry_Form", CommonUtilities.FormatString(auditLog.EntryForm));
                cmd.Parameters.AddWithValue("@Trx_No", auditLog.TrxNo);
                cmd.Parameters.AddWithValue("@Log_UserID", CommonUtilities.FormatString(auditLog.LogUserID));
                cmd.Parameters.AddWithValue("@Log_DateTime", DateUtility.ConvertDateAndTimeForSQLPurpose(auditLog.LogDateTime));
                cmd.Parameters.AddWithValue("@TLevel", CommonUtilities.FormatString(auditLog.TLevel));
                cmd.Parameters.AddWithValue("@Item_Key", auditLog.ItemKey);
                cmd.Parameters.AddWithValue("@Event_Mode", CommonUtilities.FormatString(auditLog.EventMode));
                cmd.Parameters.AddWithValue("@Field", CommonUtilities.FormatString(field.ToString()));
                cmd.Parameters.AddWithValue("@New_Value", CommonUtilities.FormatString(newValue.ToString()));
                cmd.Parameters.AddWithValue("@Old_Value", CommonUtilities.FormatString(oldValue.ToString()));
                cmd.Parameters.AddWithValue("@PM_Key", CommonUtilities.FormatString(auditLog.PMKey));
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException e) { throw e; }
            catch (SqlException e) { throw new FMException(e.Message.ToString()); }
            catch (OverflowException e) { throw new FMException(e.Message.ToString()); }
            catch (FormatException e) { throw new FMException(e.Message.ToString()); }
            catch (ArgumentNullException e) { throw new FMException(e.Message.ToString()); }
            catch (Exception e) { throw new FMException(e.Message.ToString()); }

            return true;
        }


        internal static bool WriteLog(LogHeader logHeader, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                IncrementLogID(con, tran);
                logHeader.LogID = GetNextLogID(con, tran);
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_LOG", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LOG_ID", logHeader.LogID);
                cmd.Parameters.AddWithValue("@MODULE", logHeader.Module);
                cmd.Parameters.AddWithValue("@FORM_NAME", logHeader.FormName);
                cmd.Parameters.AddWithValue("@DATE_LOG", DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(logHeader.DateLog));
                cmd.Parameters.AddWithValue("@PARENT_IDENTIFIER", logHeader.ParentIdentifier);
                // 2014-01-02 Gerry modifies this line
                cmd.Parameters.AddWithValue("@CHILD_IDENTIFIER", logHeader.ChildIdentifier.Equals(string.Empty) ? " " : logHeader.ChildIdentifier);
                // 2014-01-02 Gerry ends
                cmd.Parameters.AddWithValue("@FORM_ACTION", logHeader.FormAction);
                cmd.Parameters.AddWithValue("@USER", logHeader.User);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                // if there are logDetails, insert the record
                if (logHeader.LogDetails.Count > 0)
                {
                    foreach (LogDetail logdetail in logHeader.LogDetails)
                    {
                        WriteLogDetail(logHeader.LogID, logdetail, con, tran);
                    }
                }
                return true;
            }
            catch (FMException e) { throw e; }
            catch (SqlException e) { throw new FMException(e.Message.ToString()); }
            catch (Exception e) { throw new FMException(e.Message.ToString()); }
        }

        private static bool WriteLogDetail(int logID, LogDetail logDetail, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_TPT_LOG_DETAIL", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LOG_ID", logID);
                cmd.Parameters.AddWithValue("@PROPERTY_NAME", logDetail.PropertyName);
                cmd.Parameters.AddWithValue("@PROPERTY_VALUE", logDetail.PropertyValue);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new FMException("Error at WriteLogDetail in LoggerDAL class " + ex.ToString());
            }
        }

        internal static bool DeleteLog(LogHeader logHeader, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                // The following stored procedure deletes both records from
                // TPT_LOG_HEADER_Tbl and TPT_LOG_DETAIL_Tbl

                SqlCommand cmd = new SqlCommand("sp_Delete_TPT_Log", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LOG_ID", logHeader.LogID);

                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception Ex)
            {
                throw new FMException("Error at DeleteLog in LoggerDAl class " +
                    Ex.ToString());

            }
        }

        // creates LogHeader object from 1 row of the DataReader
        internal static LogHeader GetLogHeader(IDataReader reader)
        {
            int module = (int)reader["MODULE"];
            FMModule fmModule;

            switch (module)
            {
                // only transport module currently uses this table
                case 5:
                    fmModule = FMModule.Transport;
                    break;
                case 7:
                    fmModule = FMModule.Trucking;
                    break;

                default:
                    throw new FMException("Invalid Module in TPT_LOG_HEADER_Tbl");
            }

            int action = (int)reader["FORM_ACTION"];
            FormMode formMode;

            switch (action)
            {
                case 1:
                    formMode = FormMode.Add;
                    break;
                case 2:
                    formMode = FormMode.Edit;
                    break;
                case 3:
                    formMode = FormMode.Delete;
                    break;
                default:
                    throw new FMException("No such form action in TPT_LOG_HEADER_Tbl ");
            }

            DateTime date = reader.GetDateTime(reader.GetOrdinal("DATE_LOG"));


            LogHeader logHeader = new LogHeader(
               (int)reader["LOG_ID"],
               fmModule,
               ((string)reader["FORM_NAME"]).Trim(),
                //Convert.ToDateTime(reader["DATE_LOG"]),
                date,
               ((string)reader["PARENT_IDENTIFIER"]).Trim(),
               ((string)reader["CHILD_IDENTIFIER"]).Trim(),
                formMode,
               ((string)reader["FMUSER"]).Trim());

            // Add the LogDetail objects to the logHeader object
            int logID = (int)reader["LOG_ID"];
            logHeader.LogDetails = GetLogDetails(logID);

            //DateTime date = Convert.ToDateTime("2010-03-05 10:02:00");

            return logHeader;
        }

        public static List<LogDetail> GetLogDetails(int logID)
        {
            List<LogDetail> logDetails = new List<LogDetail>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_LOG_DETAIL_Tbl WHERE LOG_ID = ";
            SQLString += logID;
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    logDetails.Add(GetLogDetail(reader));
                }
            }
            cn.Close();
            return logDetails;
        }

        private static LogDetail GetLogDetail(SqlDataReader reader)
        {
            LogDetail logDetail = new LogDetail((int)reader["Log_ID"],
               ((string)reader["PROPERTY_NAME"]).Trim(),
               ((string)reader["PROPERTY_VALUE"]).Trim());
            return logDetail;
        }

        internal static List<LogHeader> GetLogHeaders(string parentIdentifier)
        {
            List<LogHeader> logHeaders = new List<LogHeader>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_LOG_HEADER_Tbl WHERE PARENT_IDENTIFIER = ";
            SQLString += "'" + parentIdentifier + "'";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                logHeaders.Add(GetLogHeader(reader));
            }
            cn.Close();
            return logHeaders;

        }

        internal static DateTime GetServerDateTime()
        {
            // For testing, will use current DateTime
            return DateTime.Now;
        }

        internal static void IncrementLogID(SqlConnection cn, SqlTransaction tran)
        {
            // increment the Log_NO by 1
            string strSQL = "UPDATE TPT_SPECIAL_DATA_Tbl SET LOG_NO = LOG_NO + 1 WHERE RECORD_KEY = '1' ";
            SqlCommand cmd1 = new SqlCommand(strSQL, cn);
            cmd1.CommandType = CommandType.Text;
            cmd1.Transaction = tran;
            cmd1.ExecuteNonQuery();

        }

        internal static int GetNextLogID(SqlConnection cn, SqlTransaction tran)
        {
            int nextLogId = 0;
            string strSQL = "";
            strSQL = "SELECT LOG_NO FROM TPT_SPECIAL_DATA_Tbl WHERE RECORD_KEY = '1' ";
            SqlCommand cmd2 = new SqlCommand(strSQL, cn);
            cmd2.CommandType = CommandType.Text;
            cmd2.Transaction = tran;
            IDataReader reader = cmd2.ExecuteReader();
            while (reader.Read())
            {
                nextLogId = (int)reader["LOG_NO"];

            }
            reader.Close();
            return nextLogId;
        }

        //June 15, 2011 - Gerry add, it is used to LogForm maintenance
        public static SortableList<LogHeader> GetLogHeaders(string formName, string parentIdentifier, string childIdentifier)
        {
            SortableList<LogHeader> logHeaders = new SortableList<LogHeader>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_LOG_HEADER_Tbl";
                if (childIdentifier == "")
                {
                    if (formName == "" && parentIdentifier != "")
                    {
                        SQLString += " WHERE PARENT_IDENTIFIER = '" + parentIdentifier + "'";
                    }
                    else if (formName != "" && parentIdentifier == "")
                    {
                        SQLString += " WHERE FORM_NAME = '" + formName + "'";
                    }
                    else if (formName != "" && parentIdentifier != "")
                    {
                        SQLString += " WHERE FORM_NAME = '" + formName + "'";
                        SQLString += " AND PARENT_IDENTIFIER = '" + parentIdentifier + "'";
                    }
                }
                else
                {
                    if (formName == "" && parentIdentifier != "")
                    {
                        SQLString += " WHERE PARENT_IDENTIFIER = '" + parentIdentifier + "'";
                        SQLString += " AND CHILD_IDENTIFIER = '" + childIdentifier + "'";
                    }
                    else if (formName != "" && parentIdentifier == "")
                    {
                        SQLString += " WHERE FORM_NAME = '" + formName + "'";
                        SQLString += " AND CHILD_IDENTIFIER = '" + childIdentifier + "'";
                    }
                    else if (formName != "" && parentIdentifier != "")
                    {
                        SQLString += " WHERE FORM_NAME = '" + formName + "'";
                        SQLString += " AND PARENT_IDENTIFIER = '" + parentIdentifier + "'";
                        SQLString += " AND CHILD_IDENTIFIER = '" + childIdentifier + "'";
                    }
                }

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    logHeaders.Add(GetLogHeader(reader));
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return logHeaders;
        }

        public static List<string> GetParentIdentifier(string formName)
        {
            List<string> trxNos = new List<string>();
            trxNos.Add("");
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT distinct(PARENT_IDENTIFIER) FROM TPT_LOG_HEADER_Tbl";
                if (formName != "")
                {
                    SQLString += " WHERE FORM_NAME = '" + formName + "'";
                }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    trxNos.Add(reader.GetString(0).Trim());
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trxNos;
        }

        public static List<string> GetChildIdentifier(string formName)
        {
            List<string> trxNos = new List<string>();
            trxNos.Add("");
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT distinct(CHILD_IDENTIFIER) FROM TPT_LOG_HEADER_Tbl";
                if (formName != "")
                {
                    SQLString += " WHERE FORM_NAME = '" + formName + "'";
                }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    trxNos.Add(reader.GetString(0).Trim());
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trxNos;
        }

        public static List<LogDetail> GetLogDetails(List<string> logID)
        {
            List<LogDetail> logDetails = new List<LogDetail>();
            string ids = string.Join(",", logID.ToArray());

            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_LOG_DETAIL_Tbl WHERE LOG_ID in ( {0} ) ";
            SQLString = string.Format(SQLString, ids);

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    logDetails.Add(GetLogDetail(reader));
                }
            }
            cn.Close();
            return logDetails;
        }

        #region "2013-12-18 Zhou Kai adds code block for feature, Log by Property Name"
        /*
         * These public const strings are shared between UI and BLL
         * as the "keys" in key-value pair.
         */
        public static SortableList<string> GetLogPropertiesForDisplay(string sqlForProperties)
        {
            SortableList<string> lstLoggedProperties = new SortableList<string>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand cmd = new SqlCommand(sqlForProperties, con);
            try
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lstLoggedProperties.Add((string)reader[0]);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { con.Close(); }

            return lstLoggedProperties;
        }

        // 2013-12-24 Zhou Kai modifies this function, take away the logDetails parameter and add the propertyNames parameter
        public static void GetLogHeaderAndDetails(string sqlQuery, string propertyNames,
            out SortableList<LogHeader> logHeaders)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            logHeaders = new SortableList<LogHeader>();
            try
            {
                if (con.State != ConnectionState.Open) { con.Open(); }
                Dictionary<string, string> dictFormNameMappings = new Dictionary<string, string>();
                dictFormNameMappings = InitializeFormNameMappings(dictFormNameMappings);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    logHeaders.Add(GetLogHeader(reader, propertyNames, dictFormNameMappings));
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { con.Close(); }
        }

        public static bool GetLogHeadersFromPropertyForDisplay(Dictionary<string, string> dict, string propertyNames,
             out SortableList<LogHeader> logHeaders)
        {
            bool retValue = false;
            string sql = String.Empty;
            string jobOrQuotationNo = dict[LogHeader.TOP_LEVEL_NO];
            string seqNo = dict[LogHeader.SEQ_NO];
            string detailNo = dict[LogHeader.DETAIL_NO];
            string propertyLevel = dict[LogHeader.PROPERTY_LEVEL];
            string existingTransaction = dict[LogHeader.IS_EXISTING_TRANS].ToString().Trim();
            bool bExistingTransaction = existingTransaction.Equals(true.ToString());

            logHeaders = new SortableList<LogHeader>();

            if (propertyLevel == PropertyLevel.NotSet.ToString())
            {
                throw new FMException(TR_LanguageResource.Resources.TptResourceUI.InputDataNotCompleteForLog);
            }
            if (propertyLevel == PropertyLevel.JobLevel.ToString()) // job level
            {
                if (bExistingTransaction)
                {
                    // select the logs from the last time when it was deleted
                    sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                      "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                      "AND PARENT_IDENTIFIER = '{0}' AND PROPERTY_NAME IN ({1}) " +
                      "AND TPT_LOG_HEADER_Tbl.LOG_ID >=  (SELECT TOP (1) LOG_ID FROM TPT_LOG_HEADER_Tbl " +
                        // 2014-09-12 Zhou Kai adds condition: "AND CHILD_IDENTIFIER = ''"
                      "WHERE PARENT_IDENTIFIER = '{0}'  AND CHILD_IDENTIFIER = '' AND FORM_ACTION = 1" +
                      LogHeader.FormatFormNames(dict) + " ORDER BY TPT_LOG_HEADER_Tbl.LOG_ID DESC)" +
                        // 2014-01-17 Zhou Kai adds "order by" to order the results
                      " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
                }
                else if (!bExistingTransaction)
                {
                    // select all deleted dates
                    sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                      "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                      "AND PARENT_IDENTIFIER = '{0}'  AND CHILD_IDENTIFIER = '' " +
                      "AND FORM_ACTION = 3 AND PROPERTY_NAME IN ({1}) " +
                        // 2014-01-17 Zhou Kai adds order by to order the results
                      " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
                }
                sql = String.Format(sql, jobOrQuotationNo, propertyNames);
                LoggerDAL.GetLogHeaderAndDetails(sql, propertyNames, out logHeaders);
            }
            else if (propertyLevel == PropertyLevel.JobTripLevel.ToString()) // job trip level
            {
                if (bExistingTransaction)
                {
                    // select the logs with log_id later than the last time the job trip was added
                    sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                              "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                              "AND PARENT_IDENTIFIER = '{0}'  AND  CHILD_IDENTIFIER = '{1}' AND " +
                              "PROPERTY_NAME IN ({2}) " + FormatFormNamesForSql(dict) +
                              "AND TPT_LOG_HEADER_Tbl.LOG_ID >=  (SELECT TOP (1) LOG_ID FROM TPT_LOG_HEADER_Tbl " +
                              "WHERE PARENT_IDENTIFIER = '{0}' AND CHILD_IDENTIFIER = '{1}'  " +
                              "AND FORM_ACTION = 1 " +  FormatFormNamesForSql(dict) + " ORDER BY TPT_LOG_HEADER_Tbl.LOG_ID DESC)" +
                              "AND FORM_ACTION <> 3";
                }
                else
                {
                    // only select logs of transaction deletion
                    sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                              "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                              "AND PARENT_IDENTIFIER = '{0}'  AND  CHILD_IDENTIFIER = '{1}' AND " +
                              "PROPERTY_NAME IN ({2})  AND FORM_ACTION = 3" + FormatFormNamesForSql(dict);
                }

                sql = String.Format(sql, jobOrQuotationNo, seqNo, propertyNames) +
                    // 2014-01-17 Zhou Kai adds order by to order the results
                    " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
                LoggerDAL.GetLogHeaderAndDetails(sql, propertyNames, out logHeaders);
            }
            else if (propertyLevel == PropertyLevel.JobTripDetailLevel.ToString()) // job trip detail level
            {
                // 2014-03-17 Zhou Kai modifies the codes to complete the logging for details
                if (bExistingTransaction)
                {
                    sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                                         "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                                         "AND PARENT_IDENTIFIER = '{0}&{1}'  AND  CHILD_IDENTIFIER = '{2}' AND " +
                                         "PROPERTY_NAME IN ({3}) AND TPT_LOG_HEADER_Tbl.LOG_ID > " +
                                         "(SELECT TOP(1) TPT_LOG_HEADER_Tbl.LOG_ID FROM TPT_LOG_HEADER_Tbl " +
                                         "WHERE PARENT_IDENTIFIER = '{0}&{1}' AND CHILD_IDENTIFIER = '{2}' AND " +
                                         "FORM_ACTION = 1)" +
                                          // 2014-01-17 Zhou Kai adds order by to order the results
                                         " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
                }
                else
                {
                    // 2014-03-17 Zhou Kai adds logic for detail 
                    sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                    "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                    "AND PARENT_IDENTIFIER = '{0}&{1}'  AND  CHILD_IDENTIFIER = '{2}' AND " +
                    "PROPERTY_NAME IN ({3})  AND FORM_ACTION = 3" + FormatFormNamesForSql(dict);
                }
                sql = String.Format(sql, jobOrQuotationNo, seqNo, detailNo, propertyNames);
                LoggerDAL.GetLogHeaderAndDetails(sql, propertyNames, out logHeaders);
            }
            else if (propertyLevel == PropertyLevel.JobChargeLevel.ToString()) // job charge level
            {
                if (bExistingTransaction)
                {
                    sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                             "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                            "AND PARENT_IDENTIFIER = '{0}'  AND  CHILD_IDENTIFIER = '{1}' AND " +
                            "PROPERTY_NAME IN ({2})" + FormatFormNamesForSql(dict) +
                            " AND TPT_LOG_HEADER_Tbl.LOG_ID >=  (SELECT TOP (1) LOG_ID FROM TPT_LOG_HEADER_Tbl " +
                            "WHERE PARENT_IDENTIFIER = '{0}' AND CHILD_IDENTIFIER = '{1}' " +
                            " AND FORM_ACTION = 1" + FormatFormNamesForSql(dict) + " ORDER BY TPT_LOG_HEADER_Tbl.LOG_ID DESC)" +
                          // 2014-01-17 Zhou Kai adds order by to order the results
                          " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
                }
                else
                {
                    sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                    "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                    "AND PARENT_IDENTIFIER = '{0}'  AND CHILD_IDENTIFIER = '{1}' " +
                    "AND FORM_ACTION = 3 AND PROPERTY_NAME IN ({2}) " + FormatFormNamesForSql(dict) +
                        // 2014-01-17 Zhou Kai adds order by to order the results
                     " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
                }

                sql = String.Format(sql, jobOrQuotationNo, seqNo, propertyNames);
                LoggerDAL.GetLogHeaderAndDetails(sql, propertyNames, out logHeaders);
                // 2014-01-02 Zhou Kai ends
            }
            else if (propertyLevel == PropertyLevel.QuotationLevel.ToString()) // 2014-01-03 Zhou Kai adds case for Marketing
            {
                sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                      "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                      "AND PARENT_IDENTIFIER = '{0}'  AND  CHILD_IDENTIFIER = ' ' AND " +
                      "PROPERTY_NAME IN ({1}) " +
                    // 2014-01-17 Zhou Kai adds order by to order the results
                      " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
                sql = String.Format(sql, jobOrQuotationNo, propertyNames);
                LoggerDAL.GetLogHeaderAndDetails(sql, propertyNames, out logHeaders);
            }
            else if (propertyLevel == PropertyLevel.TransportRateLevel.ToString())
            {
                sql = "SELECT * FROM TPT_LOG_HEADER_Tbl INNER JOIN TPT_LOG_DETAIL_Tbl ON " +
                      "TPT_LOG_HEADER_Tbl.LOG_ID = TPT_LOG_DETAIL_Tbl.LOG_ID " +
                      "AND PARENT_IDENTIFIER = '{0}'  AND  CHILD_IDENTIFIER = '{1}' AND " +
                      "PROPERTY_NAME IN ({2}) " +
                    // 2014-01-17 Zhou Kai adds order by to order the results
                      " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
                sql = String.Format(sql, jobOrQuotationNo, seqNo, propertyNames);
                LoggerDAL.GetLogHeaderAndDetails(sql, propertyNames, out logHeaders);
            }

            return retValue;
        }

        public static string GetSqlStringFromUIControlStatus(Dictionary<string, string> dict)
        {
            string sql = String.Empty;
            try
            {
                int mainModule = dict[LogHeader.MAIN_MODULE].Equals(LogHeader.HAULAGE) ? (int)FMModule.Transport : (int)FMModule.Trucking;
                string subModule = dict[LogHeader.SUB_MODULE];
                string propertyLevel = dict[LogHeader.PROPERTY_LEVEL];
                string jobNo = dict[LogHeader.TOP_LEVEL_NO];
                string jobTripSeq = dict[LogHeader.SEQ_NO];
                string jobChargeSeq = dict[LogHeader.SEQ_NO];
                string jobTripDetailSeq = dict[LogHeader.DETAIL_NO];
                string existingTrans = dict[LogHeader.IS_EXISTING_TRANS];
                string quotationNo = dict[LogHeader.TOP_LEVEL_NO];
                string rateSeqNo = dict[LogHeader.SEQ_NO];
                bool bExistingTrans = existingTrans.Equals(true.ToString());

                if (subModule.Equals(LogHeader.BOOKING)) // booking
                {
                    if (propertyLevel == PropertyLevel.JobLevel.ToString()) // job level
                    {
                        sql = "SELECT DISTINCT PROPERTY_NAME FROM TPT_LOG_DETAIL_Tbl INNER JOIN TPT_LOG_HEADER_Tbl " +
                               " ON TPT_LOG_DETAIL_Tbl.LOG_ID = TPT_LOG_HEADER_Tbl.LOG_ID AND " +
                               " MODULE = {0} AND PARENT_IDENTIFIER = '{1}' AND CHILD_IDENTIFIER = ''" +
                                (bExistingTrans ? "AND FORM_ACTION <> 3" : "AND FORM_ACTION = 3");
                        sql = String.Format(sql, mainModule, jobNo) + FormatFormNamesForSql(dict);
                    }
                    if (propertyLevel == PropertyLevel.JobTripLevel.ToString()) // job trip level
                    {
                        sql = "SELECT DISTINCT PROPERTY_NAME FROM TPT_LOG_DETAIL_Tbl INNER JOIN TPT_LOG_HEADER_Tbl " +
                            " ON TPT_LOG_DETAIL_Tbl.LOG_ID = TPT_LOG_HEADER_Tbl.LOG_ID AND " +
                            " MODULE = {0} AND PARENT_IDENTIFIER = '{1}' AND CHILD_IDENTIFIER = '{2}' " +
                            (bExistingTrans ? "AND FORM_ACTION <> 3" : "AND FORM_ACTION = 3");
                        sql = String.Format(sql, mainModule, jobNo, jobTripSeq) + FormatFormNamesForSql(dict);
                    }
                    if (propertyLevel == PropertyLevel.JobTripDetailLevel.ToString()) // job trip detail level
                    {
                        sql = "SELECT DISTINCT PROPERTY_NAME FROM TPT_LOG_DETAIL_Tbl INNER JOIN TPT_LOG_HEADER_Tbl " +
                           " ON TPT_LOG_DETAIL_Tbl.LOG_ID = TPT_LOG_HEADER_Tbl.LOG_ID AND " +
                           " MODULE = {0} AND PARENT_IDENTIFIER = '{1}' AND CHILD_IDENTIFIER = '{2}' "
                           + (bExistingTrans ? "AND FORM_ACTION <> 3" : "AND FORM_ACTION = 3");

                        sql = String.Format(sql, mainModule, jobNo + "&" + jobTripSeq, jobTripDetailSeq) + FormatFormNamesForSql(dict);
                    }
                    if (propertyLevel == PropertyLevel.JobChargeLevel.ToString()) // job charge level
                    {
                        // Need to check form name also
                        sql = "SELECT DISTINCT PROPERTY_NAME FROM TPT_LOG_DETAIL_Tbl INNER JOIN TPT_LOG_HEADER_Tbl " +
                            " ON TPT_LOG_DETAIL_Tbl.LOG_ID = TPT_LOG_HEADER_Tbl.LOG_ID AND " +
                            " MODULE = {0} AND PARENT_IDENTIFIER = '{1}' AND CHILD_IDENTIFIER = '{2}' " +
                            (bExistingTrans ? "AND FORM_ACTION <> 3" : "AND FORM_ACTION = 3");
                        sql = String.Format(sql, mainModule, jobNo, jobChargeSeq) + FormatFormNamesForSql(dict);
                    }
                }
                else if (dict[LogHeader.SUB_MODULE].Equals(LogHeader.MARKETING)) // marketing
                {
                    // 2014-01-03 Zhou Kai continues with Marketing
                    if (propertyLevel == PropertyLevel.QuotationLevel.ToString()) // quotation level
                    {
                        sql = "SELECT DISTINCT PROPERTY_NAME FROM TPT_LOG_DETAIL_Tbl INNER JOIN TPT_LOG_HEADER_Tbl " +
                           " ON TPT_LOG_DETAIL_Tbl.LOG_ID = TPT_LOG_HEADER_Tbl.LOG_ID AND " +
                           " MODULE = 7 AND PARENT_IDENTIFIER = '{1}' AND CHILD_IDENTIFIER = ' '";
                        sql = String.Format(sql, mainModule, quotationNo) +
                            (bExistingTrans ? "AND FORM_ACTION <> 3" : "AND FORM_ACTION = 3");
                    }
                    else if (propertyLevel == PropertyLevel.TransportRateLevel.ToString()) // transport rate level
                    {
                        sql = "SELECT DISTINCT PROPERTY_NAME FROM TPT_LOG_DETAIL_Tbl INNER JOIN TPT_LOG_HEADER_Tbl " +
                           " ON TPT_LOG_DETAIL_Tbl.LOG_ID = TPT_LOG_HEADER_Tbl.LOG_ID AND " +
                           " MODULE = 7 AND PARENT_IDENTIFIER = '{1}' AND CHILD_IDENTIFIER = '{2}'";
                        sql = String.Format(sql, mainModule, quotationNo, rateSeqNo) + 
                            (bExistingTrans ? "AND FORM_ACTION <> 3" : "AND FORM_ACTION = 3");
                    }
                }
                else if (dict[LogHeader.SUB_MODULE].Equals(LogHeader.PLANNING)) // planning
                {
                    throw (new NotImplementedException());
                }
                else
                {
                    sql = String.Empty;
                    throw (new Exception("Sql-Query String is empty."));
                }
            }
            catch (KeyNotFoundException knfe)
            {
                throw knfe;
            }

            return sql;
        }

        // creates LogHeader object from 1 row of the DataReader
        internal static LogHeader GetLogHeader(IDataReader reader, string propertyNames, Dictionary<string, string> dictFormNameMappings)
        {
            int module = (int)reader["MODULE"];
            FMModule fmModule;

            switch (module)
            {
                // only transport module currently uses this table
                case 5:
                    fmModule = FMModule.Transport;
                    break;
                case 7:
                    fmModule = FMModule.Trucking;
                    break;

                default:
                    throw new FMException("Invalid Module in TPT_LOG_HEADER_Tbl");
            }

            int action = (int)reader["FORM_ACTION"];
            FormMode formMode;

            switch (action)
            {
                case 1:
                    formMode = FormMode.Add;
                    break;
                case 2:
                    formMode = FormMode.Edit;
                    break;
                case 3:
                    formMode = FormMode.Delete;
                    break;
                default:
                    throw new FMException("No such form action in TPT_LOG_HEADER_Tbl ");
            }

            DateTime date = reader.GetDateTime(reader.GetOrdinal("DATE_LOG"));

            LogHeader logHeader = new LogHeader(
               (int)reader["LOG_ID"],
               fmModule,
               (dictFormNameMappings[((string)reader["FORM_NAME"]).Trim()]), // 2014-02-14 Zhou Kai modifies
                //Convert.ToDateTime(reader["DATE_LOG"]),
                date,
               ((string)reader["PARENT_IDENTIFIER"]).Trim(),
               ((string)reader["CHILD_IDENTIFIER"]).Trim(),
                formMode,
               ((string)reader["FMUSER"]).Trim());

            // Add the LogDetail objects to the logHeader object,
            // from LOG_ID and PropertyNames
            int logID = (int)reader["LOG_ID"];
            logHeader.LogDetails = GetLogDetails(logID, propertyNames);
            // set the customized string for logging display on propertyNames
            logHeader.SetCusFieldForDisplay();

            return logHeader;
        }

        public static List<LogDetail> GetLogDetails(int logID, string propertyNames)
        {
            List<LogDetail> logDetails = new List<LogDetail>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "SELECT * FROM TPT_LOG_DETAIL_Tbl WHERE LOG_ID = ";
            SQLString += logID + " AND PROPERTY_NAME IN (" + propertyNames + ")" +
                // 2014-01-17 Zhou Kai adds order by to order the results
            " ORDER BY SUBSTRING(PROPERTY_NAME, CHARINDEX(' ', PROPERTY_NAME), LEN(PROPERTY_NAME))";
            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    logDetails.Add(GetLogDetail(reader));
                }
            }
            cn.Close();
            return logDetails;
        }

        public static string FormatFormNamesForSql(Dictionary<string, string> dicCriteriasForLogPropertyName)
        {
            string formNames = String.Empty;
            string mainModule = dicCriteriasForLogPropertyName[LogHeader.MAIN_MODULE];
            string subModule = dicCriteriasForLogPropertyName[LogHeader.SUB_MODULE];
            PropertyLevel propertyLevel =
                (PropertyLevel)Enum.Parse(typeof(PropertyLevel), dicCriteriasForLogPropertyName[LogHeader.PROPERTY_LEVEL]);
            switch (propertyLevel)
            {
                case PropertyLevel.JobLevel:
                    {
                        foreach (string frmName in LogHeader.FRM_NAMES_TRK_JOB)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        foreach (string frmName in LogHeader.FRM_NAMES_HAU_JOB)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        break;
                    }
                case PropertyLevel.JobTripLevel:
                    {
                        foreach (string frmName in LogHeader.FRM_NAMES_TRK_JOB_TRIP)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        foreach (string frmName in LogHeader.FRM_NAMES_HAU_JOB_TRIP)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        break;
                    }
                case PropertyLevel.JobChargeLevel:
                    {
                        foreach (string frmName in LogHeader.FRM_NAMES_TRK_JOB_CHARGE)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        foreach (string frmName in LogHeader.FRM_NAMES_HAU_JOB_CHARGE)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        break;
                    }
                case PropertyLevel.JobTripDetailLevel:
                    {

                        break;
                    }
                case PropertyLevel.QuotationLevel:
                    {
                        foreach (string frmName in LogHeader.FRM_NAMES_QUOTATION)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        break;
                    }
                default:
                    {
                        foreach (string frmName in LogHeader.FRM_NAMES_TRK_JOB)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        foreach (string frmName in LogHeader.FRM_NAMES_HAU_JOB)
                        {
                            formNames += "'" + frmName + "',";
                        }
                        break;
                    }
            }

            if (!formNames.Equals(String.Empty))
            {
                formNames = " AND FORM_NAME IN (" + formNames;
                formNames = formNames.Substring(0, formNames.Length - 1) + ")";
            }
            return formNames;
        }

        // 2014-02-14 Zhou Kai adds function to
        // get the mappings between FM_FORM_NAME to USER_FORM_NAME from TPT_LOG_FORMS
        public static Dictionary<string, string> InitializeFormNameMappings(Dictionary<string, string> dictFormNamesMapping)
        {
            string sqlString = "SELECT * FROM TPT_LOG_FORMS";
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlString, cn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // 2014-04-01 Zhou Kai adds .Trim() to the string values read from database
                    dictFormNamesMapping.Add(reader["FM_FORM_NAME"].ToString().Trim(), reader["USER_FORM_NAME"].ToString().Trim());
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }
            return dictFormNamesMapping;
        }
        // 2014-02-14 Zhou Kai ends

        #endregion
    }
}
