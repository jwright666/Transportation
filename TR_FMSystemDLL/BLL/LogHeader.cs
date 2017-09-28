using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class LogHeader
    {
        private int logID;
        private FMModule module;
        private string formName;
        private DateTime dateLog;
        private string parentIdentifier;
        private string childIdentifier;
        private FormMode formAction;
        private string user;
        private List<LogDetail> logDetails = new List<LogDetail>();

        // constructor use for reading i.e. logID is known
        public LogHeader(int logID, FMModule module, string formName, DateTime datetime,
            string parentIdentifier, string childIdentifier, FormMode formAction, string user)
        {
            this.LogID = logID;
            this.Module = module;
            this.FormName = formName;
            this.DateLog = datetime;
            this.ParentIdentifier = parentIdentifier;
            this.ChildIdentifier = childIdentifier;
            this.FormAction = formAction;
            this.User = user;

        }

        // constructor use for writing i.e. logID is unknown yet
        public LogHeader(FMModule module, string formName, DateTime datetime,
            string parentIdentifier, string childIdentifier, FormMode formAction, string user)
        {
            this.Module = module;
            this.FormName = formName;
            this.DateLog = datetime;
            this.ParentIdentifier = parentIdentifier;
            this.ChildIdentifier = childIdentifier;
            this.FormAction = formAction;
            this.User = user;
        }

        public int LogID
        {
            get { return logID; }
            set { logID = value; }
        }

        public FMModule Module
        {
            get { return module; }
            set
            {
                // Limit now to only Transport Module
                //if (value != FMModule.Transport)
                //{
                //    throw new FMException(FMException.WRONG_VALUE + " Module enum");
                //}
                module = value;
            }
        }
        public string FormName
        {
            get { return formName; }
            set
            {
                value = value.Trim();
                // Limit now to only Transport Module
                //if (value.Length == 0)
                //{
                //    throw new FMException(FMException.REQUIRED_FIELD + " Form Name");
                //}
                formName = value;
            }
        }


        public DateTime DateLog
        {
            get { return dateLog; }
            set { dateLog = value; }
        }

        public string ParentIdentifier
        {
            get { return parentIdentifier; }
            set
            {
                value = value.Trim();
                // Limit now to only Transport Module
                if (value.Length == 0)
                {
                    throw new FMException(FMException.REQUIRED_FIELD + " Parent Identifier");
                }
                parentIdentifier = value;
            }
        }

        public string ChildIdentifier
        {
            get { return childIdentifier; }
            set
            {
                value = value.Trim();
                // 2013-12-23 Zhou Kai comments out to allow child_id is blank " "
                //if (value.Length == 0)
                //{
                //    throw new FMException(FMException.REQUIRED_FIELD + " Child Identifier");
                //}
                childIdentifier = value;
            }
        }

        public FormMode FormAction
        {
            get { return formAction; }
            set
            {
                // Limit now to only Transport Module
                if (value != FormMode.Add & value != FormMode.Edit & value != FormMode.Delete)
                {
                    throw new FMException(FMException.WRONG_VALUE + " FormMode enum");
                }
                formAction = value;
            }
        }

        public string User
        {
            get { return user; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(FMException.REQUIRED_FIELD + " User");
                }
                user = value;
            }
        }

        public List<LogDetail> LogDetails
        {
            get { return logDetails; }
            set { logDetails = value; }
        }


        public static SortableList<LogHeader> GetLogHeader(string formName, string parentIdentifier, string childIdentifier)
        {
            return LoggerDAL.GetLogHeaders(formName, parentIdentifier, childIdentifier);
        }

        public static List<LogDetail> GetLogDetails(int logId)
        {
            return LoggerDAL.GetLogDetails(logId);
        }
        public static List<string> GetParentIdentifier(string formName)
        {
            return LoggerDAL.GetParentIdentifier(formName);
        }

        public static List<string> GetChildIdentifier(string formName)
        {
            return LoggerDAL.GetChildIdentifier(formName);
        }

        public static List<LogDetail> GetLogDetails(List<string> logId)
        {
            return LoggerDAL.GetLogDetails(logId);
        }

        #region "2013-12-16 Zhou Kai adds codes for Log by Property for FM82"
        public const string MAIN_MODULE = "MAIN_MODULE";
        public const string SUB_MODULE = "SUB_MODULE";
        public const string PROPERTY_LEVEL = "PROPERTY_LEVEL";
        public const string TOP_LEVEL_NO = "JOB_OR_QUOTATION_NO";
        public const string SEQ_NO = "JOBTRIP_OR_CHARGE_SEQNO";
        public const string DETAIL_NO = "JOBTRIP_DETAIL_NO";
        // 2014-02-02 Zhou Kai adds
        public const string IS_EXISTING_TRANS = "IS_EXISTING_TRANS";
        // 2014-02-02 Zhou Kai ends

        // 2014-02-14 Zhou Kai adds the mappings of FM_Form_Name and User_Form_Name
        // When writing to database, use the key, when displaying on logs, use the value
        public static readonly Dictionary<string, string> DictFormNames = new Dictionary<string, string>()
            {
                {"TPT_HAU_E_HAULIER_JOB","Haulage - Book Header"},
                {"TPT_HAU_E_HAULIER_MULT_2CONT","Haulage - Book Multi-containers - 2 trips"},
                {"TPT_HAU_E_HAULIER_JOB_TRIP","Haulage - Book Job Trip"},
                {"TPT_HAU_E_HAULIER_MULT_CONT","Haulage - Book Multi-containers - single trip"},
                {"TPT_HAU_E_CHARGE","Haulage - Book Job charge"},
                {"TPT_HAU_E_QUOTATION","Transport Marketing - Header"},
                {"TPT_HAU_E_COPY_QUOTATION","Transport Marketing - Copy Transport Rates"},
                {"TPT_HAU_E_TRANSPORT_RATE","Transport Marketing - Transport Rate"},
                {"TPT_TRK_E_TRUCK_JOB","Trucking - Book Header"},
                {"TPT_TRK_E_TRUCK_JOB_TRIP","Trucking - Book Job Trip"},
                {"TPT_TRK_E_TRUCK_JOB_TRIP_DET","Trucking - Book Job Trip detail"},
                {"TPT_TRK_E_TRUCK_JOB_CHARGE","Trucking - Book Job Charge"},
            };
        // 2014-02-14 Zhou Kai ends

        // 2014-01-07 Zhou Kai adds
        // These array recorde the form_names involved according to different property levels.
        // add the form names into the right array if new forms added into system
        public static readonly string[] FRM_NAMES_TRK_JOB = new string[] { "TPT_TRK_E_TRUCK_JOB" };
        public static readonly string[] FRM_NAMES_TRK_JOB_TRIP = new string[] { "TPT_TRK_E_TRUCK_JOB_TRIP" };
        public static readonly string[] FRM_NAMES_TRK_JOB_CHARGE = new string[] { "TPT_TRK_E_TRUCK_JOB_CHARGE" };
        // 2014-09-12 Zhou Kai adds new form names to FRM_NAMES_HAU_JOB and FRM_NAMES_HAU_JOB_TRIP
        // as now job / job trip may be created from new forms(Import from SF)
        public static readonly string[] FRM_NAMES_HAU_JOB = new string[] 
        { "TPT_HAU_E_HAULIER_JOB",
            "TPT_HAU_IMPORT_FROM_SEAFREIGHT", 
            "TPT_HAU_E_HAULIER_JOB_FRM_PLAN"
        };
        public static readonly string[] FRM_NAMES_HAU_JOB_TRIP = new string[] 
        { "TPT_HAU_E_HAULIER_JOB_TRIP", 
            "TPT_HAU_E_HAULIER_MULT_2CONT", 
            "TPT_HAU_E_HAULIER_MULT_CONT" ,
            "TPT_HAU_IMPORT_FROM_SEAFREIGHT",
            "TPT_HAU_E_HAULIER_JOB_FRM_PLAN"
        };
        public static readonly string[] FRM_NAMES_HAU_JOB_CHARGE = new string[] { "TPT_HAU_E_CHARGE" };
        public static readonly string[] FRM_NAMES_QUOTATION = new string[] { "TPT_HAU_E_QUOTATION" };
        public static readonly string[] FRM_NAMES_RATE = new string[] { "TPT_HAU_E_TRANSPORT_RATE" };

        /*
         * These public const strings are hardcoded "values" in
         * the key-value pairs for main and sub module.
         */
        public static string HAULAGE = "Haulage";
        public static string TRUCKING = "Trucking";
        public static string MARKETING = "Marketing";
        public static string BOOKING = "Booking";
        public static string PLANNING = "Planning";

        // New field for log displaying on FrmLogFormByProp
        private string cusFieldForDisplay;
        public string CusFieldForDisplay
        {
            get { return cusFieldForDisplay; }
            set { cusFieldForDisplay = value; }
        }

        public void SetCusFieldForDisplay()
        {
            string tmp = String.Empty;
            foreach (LogDetail ld in logDetails)
            {
                tmp += ld.PropertyName + ":" + ld.PropertyValue + ",";
            }
            cusFieldForDisplay = tmp;
        }

        public static SortableList<string> GetLogPropertiesForDisplay(string sqlForProperties)
        {
            return LoggerDAL.GetLogPropertiesForDisplay(sqlForProperties);
        }

        public static void DeleteDuplicatedLogHeaders(SortableList<LogHeader> logHeaders)
        {
            Dictionary<int, LogHeader> dictTmp = new Dictionary<int, LogHeader>();
            foreach (LogHeader lh in logHeaders)
            {
                try
                {
                    dictTmp.Add(lh.LogID, lh);
                }
                catch (ArgumentException ae) { /* ignore and do nothing */}
            }
            logHeaders.Clear();
            foreach (KeyValuePair<int, LogHeader> p in dictTmp)
            {
                logHeaders.Add(p.Value);
            }
        }

        public static void GetLogHeadersFromPerpotyForDisplay(Dictionary<string, string> dict, string propertyNames,
            out SortableList<LogHeader> logHeaders)
        {
            LoggerDAL.GetLogHeadersFromPropertyForDisplay(dict, propertyNames, out logHeaders);
        }

        public static string GetSqlStringFromUIControlStatus(Dictionary<string, string> dicCriteriasForLogPropertyName)
        {
            return LoggerDAL.GetSqlStringFromUIControlStatus(dicCriteriasForLogPropertyName);
        }

        // 2014-01-08 Zhou Kai adds for generating the form names for sql-query string
        public static string FormatFormNames(Dictionary<string, string> dicCriteriasForLogPropertyName)
        {
            return LoggerDAL.FormatFormNamesForSql(dicCriteriasForLogPropertyName);
        }

        #endregion
    }



}
