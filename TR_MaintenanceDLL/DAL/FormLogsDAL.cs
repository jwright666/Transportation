using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_MaintenanceDLL.DAL
{
    class FormLogsDAL
    {

        internal static SortableList<FormLogs> GetAllHeaderForms()
        {
            SortableList<FormLogs> formLogs = new SortableList<FormLogs>();
            string sqlQuery = @"select distinct(PARENT_FORM) as PARENT_FORM, PARENT_FORM_DESCRIPTION
                                         from TPT_LOG_FORMS ";

            try
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                con.Open();
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    formLogs.Add(GetParentFormLog(true, reader));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return formLogs;
        }
        internal static FormLogs GetParentFormLog(bool isHeader, IDataReader reader)
        {
            FormLogs formLog = new FormLogs();
            try
            {
                if (isHeader)
                {
                    formLog = new FormLogs((string)reader["PARENT_FORM"],
                                           (string)reader["Parent_Form_Description"],
                                           string.Empty, string.Empty);
                }
                else
                {
                    formLog = new FormLogs(string.Empty, 
                                            string.Empty,
                                            (string)reader["CHILD_FORM"],
                                            (string)reader["CHILD_FORM_DESCRIPTION"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return formLog;
        }

        internal static SortableList<FormLogs> GetChildForms(string parentForm)
        {
            SortableList<FormLogs> childForms = new SortableList<FormLogs>();
            FormLogs formLog = new FormLogs();
            childForms.Add(formLog);
            try
            {
                string sqlQuery = @"SELECT * FROM TPT_LOG_FORMS
                                        WHERE PARENT_FORM = '{0}'";

                sqlQuery = string.Format(sqlQuery, parentForm);

                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                con.Open();
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    childForms.Add(GetParentFormLog(false, reader));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return childForms;
        }
        
    }
}
