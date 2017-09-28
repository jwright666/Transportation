using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_TRKBookDLL.Facade
{
    public class JSONParameter
    {
        public ParamInfo paramInfo { get; set; }
        public Filter filter { get; set; }
    }
    public class ParamInfo
    {
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string ServerIP { get; set; }
        public string ServerPort { get; set; }
        public string Customer_Code { get; set; }
        public string Trx_ID { get; set; }
        public string Job_Id { get; set; }
        public string Consignment_Note_No { get; set; }
        public string Job_No { get; set; }
        public string Seq_No { get; set; }
        public string JobID_List { get; set; }
        public Filter filter { get; set; }

        public void SetParamInfo()
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string SQLString = @" declare @dbName varchar(20), @serverName varchar(50), @serverIP varchar(20), @port varchar(6)

                                        select @dbName = SETTING_VALUE from CM_EDI_DETAIL_SETUP_TBL where EDI_NAME = 'FREIGHTX_API' and SETTING_NAME ='DATABASE_NAME'
                                        select @serverName = SETTING_VALUE from CM_EDI_DETAIL_SETUP_TBL where EDI_NAME = 'FREIGHTX_API' and SETTING_NAME ='Server_NAME'
                                        select @serverIP = SETTING_VALUE from CM_EDI_DETAIL_SETUP_TBL where EDI_NAME = 'FREIGHTX_API' and SETTING_NAME ='SERVER_IP'
                                        select @port = SETTING_VALUE from CM_EDI_DETAIL_SETUP_TBL where EDI_NAME = 'FREIGHTX_API' and SETTING_NAME ='SERVER_PORT'

                                        select @dbName dbName, @ServerName serverName, @serverIP serverIP, @port port";

                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    SqlCommand cmd = new SqlCommand(SQLString, con);
                    cmd.CommandTimeout = 0;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        this.ServerName = reader["serverName"] == DBNull.Value ? string.Empty : (string)reader["serverName"];
                        this.DatabaseName = reader["dbName"] == DBNull.Value ? string.Empty : (string)reader["dbName"];
                        this.ServerIP = reader["serverIP"] == DBNull.Value ? string.Empty : (string)reader["serverIP"];
                        this.ServerPort = reader["port"] == DBNull.Value ? string.Empty : (string)reader["port"];
                    }
                    reader.Close();
                }
                catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
                catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            }
        }
    }
    public class Filter
    {
        public string Trx_ID { get; set; }
        public string Job_Id { get; set; }
        public string Consignment_Note_No { get; set; }
        public string Job_No { get; set; }
        public string Seq_No { get; set; }
        public string Customer_Code { get; set; }
        public string JobID_List { get; set; }
        public Filter()
        {
            this.Trx_ID = string.Empty;
            this.Job_Id = string.Empty;
            this.Consignment_Note_No = string.Empty;
            this.Job_No = string.Empty;
            this.Seq_No = string.Empty;
            this.Customer_Code = string.Empty;
            this.JobID_List = string.Empty;
        }
    }

    public class AccessToken
    {
        public string Auth_Token { get; set; }
        public string Auth_Expire { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string User_Name_LC { get; set; }
        public string Company_ID { get; set; }     
    }
}
