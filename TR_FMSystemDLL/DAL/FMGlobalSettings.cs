using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Security.Principal;  
using FM.TR_FMSystemDLL.BLL;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using TR_LanguageResource.Resources;
using System.Collections;

namespace FM.TR_FMSystemDLL.DAL
{
    public class FMGlobalSettings
    {
        private string conString;
        private string pubConString;

        string iniPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString() + @"\System_Manager.ini";
        public string sqlUser = "";  //20140320 - gerry change private to pulic
        public string sqlPassword = ""; //20140320 - gerry change private to pulic
        public string dbName = "";//20170206 - gerry added
        public string serverName = ""; //20170206 - gerry added
        public string loginUserName = ""; //20170206 - gerry added
        public string loginPassword = "";//20170206 - gerry added
        //20150515 - gerry added for web service address
        public static string WS_Address = ""; //
        //201506 50 - gerry added for data grid columns settings
        string settingsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString() + @"\GridSettings.ini";
        private static ArrayList headerColumns;
        private static ArrayList detailColumns;
        private static ArrayList planJobTripColumns;

        //Imported from kernel32 dll might be used to read from .ini file
        #region kernel dll imported
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
                int size, string filePath);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int Section, string Key, string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
               int Size, string FileName);

        //static string fmkgPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString() + @"\FMKGConvert.dll";
        [DllImport("FMKGConvert.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        //Directory.SetCurrentDirectory(fmkgPath);
        static extern int EncPassConvert(string str1, string str2, out string str);
        #endregion

        public FMGlobalSettings() { }

        public static readonly FMGlobalSettings TheInstance = new FMGlobalSettings();

        // string pubsConnection = "Data Source=" + server + ";Initial Catalog=pubs;"  + "User Id=ipl;" + "Password=support;";
        public void SetConnectionString(string user, string server)
        {
            //get sql username and password from System_Manager.ini
            GetSQLUserPassword();  
            //string pubsConnection = "Data Source=" + server + ";Initial Catalog=pubs;" + "Integrated Security=SSPI;";
            
            //10 Jan. 2012 - Gerry modify connectionstring, not to used windows authentication to login sql server.
            string pubsConnection = @"Data Source= {0}; Trusted_Connection = false; Initial Catalog=pubs;User Id = {1}; password ={2};";
            pubsConnection = string.Format(pubsConnection, server, sqlUser, sqlPassword);
            SqlConnection con = new SqlConnection(pubsConnection);
            try
            {  
                string strSQL = "SELECT * FROM FM_Login WHERE UserID = '" + user + "'";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                con.Open();
                IDataReader reader = cmd.ExecuteReader();
                string databaseName = "";
                while (reader.Read())
                {
                    databaseName = (string)reader["DatabaseName"];
                }
                if (databaseName.Length == 0 || databaseName.Equals(string.Empty))
                {
                    throw new FMException(string.Format(TptResourceDAL.ErrAccessingFMLogin, user)); //UserId '{0}' has been logout from the server. Please restart FM and login again.
                }
                else
                {
                    //connection = "Data Source=" + server + ";Initial Catalog=" + databaseName + ";User Id=ipl;" + "Password=support;";
                     //connection = "Data Source=" + server + ";Initial Catalog=" + databaseName +";Integrated Security=SSPI;";

                    conString = @"Data Source= {0}; Initial Catalog={1}; Trusted_Connection = false; User Id = {2}; password ={3};";
                    conString = string.Format(conString, server, databaseName, sqlUser, sqlPassword);
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public bool InsertLoginInfo(string strUser,string strServer,string strDatabase)
        {
            string pubsConnection = "Data Source=" + strServer + ";Initial Catalog=pubs;" + "User Id=ipl;" + "Password=support;";
            SqlConnection con = new SqlConnection(pubsConnection);
            try
            {
                string strSQL = "Insert into FM_Login (UserID,PC_Name,ServerName,DatabaseName)values(@UserID, @PCName,@Servername,@DatabaseName)";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@UserID", strUser.Trim());
                cmd.Parameters.AddWithValue("@PCName",System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString());
                cmd.Parameters.AddWithValue("@Servername", strServer.Trim());
                cmd.Parameters.AddWithValue("@DatabaseName", strDatabase.Trim());
                con.Open();
                cmd.ExecuteNonQuery(); 
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing FM_Login table" + ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return false; 
        }

        public bool DeleteLoginInfo(string strUser,string strServer)
        {
            string pubsConnection = "Data Source=" + strServer + ";Initial Catalog=pubs;" + "User Id=ipl;" + "Password=support;";
            SqlConnection con = new SqlConnection(pubsConnection);
            string strSQL = "Delete from FM_Login WHERE UserID =@UserID "; 
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.AddWithValue("@UserID", strUser.Trim());
            try
            {
                con.Open();
                IDataReader reader = cmd.ExecuteReader();
                int count = 0;
                string databaseName = "";
                while (reader.Read())
                {
                    databaseName = (string)reader["DatabaseName"];

                    count += 1;
                }
                if (count == 0 || databaseName.Length == 0)
                {
                    throw new FMException("Database not set correctly");
                }
                else
                {
                    conString = "Data Source=" + strServer + ";Initial Catalog=" + databaseName + ";User Id=ipl;" + "Password=support;";
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing FM_Login table" + ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return false; 
        }

        public bool UpdateLoginInfo(string strUser, string strServer, string strDatabase)
        {
            string pubsConnection = "Data Source=" + strServer + ";Initial Catalog=pubs;" + "User Id=ipl;" + "Password=support;";
            SqlConnection con = new SqlConnection(pubsConnection);
            try
            {
                string strSQL = "update FM_Login set PC_Name = @PCName, ServerName=@ServerName,DatabaseName=@Database where UserID =@UserID ";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@UserID", strUser.Trim());
                cmd.Parameters.AddWithValue("@PCName", System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString());
                cmd.Parameters.AddWithValue("@ServerName", strServer.Trim());
                cmd.Parameters.AddWithValue("@Database", strDatabase.Trim());
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing FM_Login table" + ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return false; 
        }
       
        public bool CheckExistLoginInfo(string strUser, string strServer,string strDatabase)
        {
            string pubsConnection = "Data Source=" + strServer + ";Initial Catalog=pubs;" + "User Id=ipl;" + "Password=support;";
            SqlConnection con = new SqlConnection(pubsConnection);
            try
            {
                string strSQL = "select count(*) from FM_Login where UserID =@UserID";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.Parameters.AddWithValue("@UserID", strUser.Trim()); 
                con.Open();
                int iFoundRecord = (int)cmd.ExecuteScalar();
                if (iFoundRecord >= 1)
                {
                    this.UpdateLoginInfo(strUser, strServer, strDatabase);
                }
                else
                {
                    this.InsertLoginInfo(strUser, strServer, strDatabase); 
                }

            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing FM_Login table" + ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return false; 
        }

        public string getConnectionString()
        {
            try
            {
                if (conString.Length == 0)
                {
                    throw new FMException("Connection has not been set");
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error Connection String" + ex.ToString());
            }
            return conString;

        }    

        public string GetPubsConnectionString()
        {    
            try
            {
                if (this.pubConString.Length == 0)
                {
                    throw new FMException("Pubs Connection has not been set");
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error Connection String" + ex.ToString());
            }
            return this.pubConString;            
        }
        //20130701 - Gerry Added - This method used to test the sql connection if have valid server or database name
        //mostly use for licensing (to access pubs database) user or innosys staff will provide the server name.
        public void TestSQLConnection(string paramConString)
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(paramConString);
                con.Open();
            }
            catch (InvalidOperationException)
            {
                throw new FMException("Server was not found or not accessible. \n" + paramConString);
            }
            catch (SqlException)
            {
                throw new FMException("Server was not found or not accessible. \n" + paramConString);
            }
            catch (Exception ex)
            {
                throw new FMException("Error Connection String" + ex.ToString());
            }
            finally { if (con.State == ConnectionState.Open) { con.Close(); } }
            
        }

        //31 Aug. 2011 - Gerry Added for NUNIT Test - valid conString will be pass this parameter
        //may use connection string from App.config file or pass 
        public void SetConnectionString(string conString)
        {
            try
            {
                if(!conString.Equals(string.Empty))
                    this.conString = conString;
                else
                    throw new FMException("Database not set correctly. ");
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing database.\n" + ex.ToString());
            }
        }
        //20121123 - Gerry overload 1 more method to use in webservice
        public void SetConnectionString(string server, string dbName, string sqlUserName, string sqlUserPassword)
        {
            try
            {
                conString = "Data Source= '{0}'; Initial Catalog='{1}';  User Id = '{2}'; password = '{3}';";
                this.serverName = server;
                this.dbName = dbName;
                conString = string.Format(conString, server, dbName, sqlUserName, sqlUserPassword);

                //20130821 - Gerry added to set pub connection string use for webservice
                pubConString = "Data Source= '{0}'; Initial Catalog=Pubs;  User Id = '{1}'; password = '{2}';Connection Timeout=10";
                pubConString = string.Format(pubConString, server, sqlUserName, sqlUserPassword);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing database.\n" + ex.ToString());
            }
        }
        //20131521 - Gerry overload 1 more method for new parameter dbName passed from FM
        public void SetConnectionString(string[] param, bool takePassword = false)
        {
            try
            {
                //param[0] = userlogin //not used here
                //param[1] = server
                //param[2] = databaseName
                GetSQLUserPassword();//read from SystemManager.ini file to get sql user id and password
                this.serverName = param[1];
                this.dbName = param[2];
                conString = "Data Source= '{0}'; Initial Catalog='{1}';  User Id = '{2}'; password = '{3}';";
                conString = string.Format(conString, serverName.ToString(), dbName.ToString(), this.sqlUser, this.sqlPassword);

                if (takePassword)
                {
                    this.loginUserName = param[0];
                    string GetPassString = @"select User_Password from SEC_users_tbl where User_ID = '" + this.loginUserName + "'";
                    SqlConnection con = new SqlConnection(conString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand(GetPassString, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        this.loginPassword = reader.GetString(0);
                    }
                    con.Close();
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing database.\n" + ex.ToString());
            }
        }

        public void SetConnectionStringForMS(string [] args)
        {
            try
            {
                // "Persist Security Info=False;User ID=*****;Password=*****;Initial Catalog=AdventureWorks;Server=MySqlServer"
                // args[0]: database_login_id
                // args[1]: database_login_password
                // args[2]: database_name
                // args[3]: sql_server_name
                conString = "Persist Security Info=False; User ID = {0}; Password = {1}; Initial Catalog = {2};  Server='{3}';";

                this.serverName = args[1];
                this.dbName = args[2];
                conString = string.Format(conString, args[0], args[1], args[2], args[3]);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing database.\n" + ex.ToString());
            }
        }

        /// <summary>
        /// 2015-02-07 Zhou Kai.
        /// Set the connection string purely from args.
        /// </summary>
        /// <param name="param"></param>
        public void SetConnectionStringFromArgs(string[] param)
        {
            try
            {
                //param[0] = userlogin //not used here
                //param[1] = server
                //param[2] = databaseName
                GetSQLUserPassword();//read from SystemManager.ini file to get sql user id and password
                conString = "User Id = '{0}'; password = '{1}'; Data Source= '{2}'; Initial Catalog='{3}';";
                this.serverName = param[1];
                this.dbName = param[2];
                conString = string.Format(conString, param[0], param[1], param[2], param[3]);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing database.\n" + ex.ToString());
            }
        }

        //20130619 - gerry added for pub connection string
        public void SetPubsConnectionString(string server)
        {
            try
            {
                GetSQLUserPassword();
                this.serverName = server;
                this.pubConString = "Data Source= '{0}'; Initial Catalog=Pubs;  User Id = '{1}'; password = '{2}';Connection Timeout=10";
                this.pubConString = string.Format(pubConString, server, this.sqlUser, this.sqlPassword);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex) 
            {
                throw ex;
            }  
        }

        //10 Jan. 2012 - Gerry Added this method to get sql username and password from System_Manager.ini
        public void GetSQLUserPassword()
        {
            if (File.Exists(iniPath))
            {
                StringBuilder user = new StringBuilder(255);
                StringBuilder pass = new StringBuilder(255);
                int i = GetPrivateProfileString("User", "UserName", "", user, 255, iniPath);
                int p = GetPrivateProfileString("User", "Password", "", pass,  255, iniPath);

                this.sqlUser = user.ToString();
                this.sqlPassword = pass.ToString();
            }
            else
            {
                throw new FMException("INI file not exist in " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString());
            }
        }
        public ArrayList GetDatabaseToLogin()
        {
            ArrayList retValue = new ArrayList();
            if (File.Exists(iniPath))
            {
                foreach (string section in GetSectionNames())
                {
                    if (section.Contains("Company"))
                    {
                        CompanyProfile companyProfile = new CompanyProfile()
                        {
                            CompanyName = IniReadValue(section, "CompanyName"),
                            DisplayCompanyName = IniReadValue(section, "DisplayCompanyName"),
                            ServerApplicationName = IniReadValue(section, "ServerApplicationName"),
                            IPAddress = IniReadValue(section, "IPAddress"),
                            DBName = IniReadValue(section, "DBName"),
                            DBServer = IniReadValue(section, "DBServer"),
                            NT_Security = IniReadValue(section, "NT_Security"),
                            BINARYKEY = IniReadValue(section, "BINARYKEY")
                        };
                        retValue.Add(companyProfile);
                    }
                }
            }
            else
            {
                throw new FMException("INI file not exist in " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString());
            }
            return retValue;
        }
        // The Function called to obtain the SectionHeaders,
        // and returns them in an Dynamic Array.
        public string[] GetSectionNames()
        {
            //    Sets the maxsize buffer to 500, if the more
            //    is required then doubles the size each time.
            for (int maxsize = 500; true; maxsize *= 2)
            {
                //    Obtains the information in bytes and stores
                //    them in the maxsize buffer (Bytes array)
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(0, "", "", bytes, maxsize, iniPath);

                // Check the information obtained is not bigger
                // than the allocated maxsize buffer - 2 bytes.
                // if it is, then skip over the next section
                // so that the maxsize buffer can be doubled.
                if (size < maxsize - 2)
                {
                    // Converts the bytes value into an ASCII char. This is one long string.
                    string Selected = Encoding.ASCII.GetString(bytes, 0,
                                               size - (size > 0 ? 1 : 0));
                    // Splits the Long string into an array based on the "\0"
                    // or null (Newline) value and returns the value(s) in an array
                    return Selected.Split(new char[] { '\0' });
                }
            }
        }
        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.iniPath);
            return temp.ToString();

        }
        //set first 20 columns of the data grid
        public void SetHeaderGridColumns()
        {
            headerColumns = new ArrayList();
            try
            {
                if (File.Exists(settingsPath))
                {
                    for (int i = 1; i <= 15; i++)
                    {
                        StringBuilder col = new StringBuilder(255);
                        GetPrivateProfileString("HL_HeaderColumns", i.ToString(), "", col, 255, settingsPath);
                        headerColumns.Add(col);
                    }
                }
                //else
                //{
                //    throw new FMException("GridSettings.ini file not exist in " + settingsPath);
                //}

            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        public ArrayList GetHeaderGridColumns()
        {
            if (headerColumns.Count < 1) { throw new FMException("Header grid columns was not setup. "); }
            return headerColumns;
        }
        public void SetDetailGridColumns()
        {
            detailColumns = new ArrayList();
            try
            {
                if (File.Exists(settingsPath))
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        StringBuilder col = new StringBuilder(255);
                        GetPrivateProfileString("HL_DetailColumns", i.ToString(), "", col, 255, settingsPath);
                        detailColumns.Add(col);
                    }
                }
                //else
                //{
                //    throw new FMException("GridSettings.ini file not exist in " + settingsPath);
                //}

            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        public ArrayList GetDetailGridColumns()
        {
            if (detailColumns.Count < 1) { throw new FMException("detail grid columns was not setup. "); }
            return detailColumns;
        }
        public void SetPlanJobTripColumns()
        {
            planJobTripColumns = new ArrayList();
            try
            {
                if (File.Exists(settingsPath))
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        StringBuilder col = new StringBuilder(255);
                        GetPrivateProfileString("HL_PlanJobTripColumns", i.ToString(), "", col, 255, settingsPath);
                        planJobTripColumns.Add(col);
                    }
                }
                //else
                //{
                //    throw new FMException("GridSettings.ini file not exist in " + settingsPath);
                //}

            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        
        }
        public ArrayList GetPlanJobTripColumns()
        {
            if (planJobTripColumns.Count < 1) { throw new FMException("plan job trip grid columns was not setup. "); }
            return planJobTripColumns;
        }
        //
    }
    public class CompanyProfile
    {
        public string CompanyName { get; set; }
        public string DisplayCompanyName { get; set; }
        public string ServerApplicationName { get; set; }
        public string IPAddress { get; set; }
        public string DBName { get; set; }
        public string DBServer { get; set; }
        public string NT_Security { get; set; }
        public string BINARYKEY { get; set; }

        public CompanyProfile()
        {
        }
    }
}
