using FM.TR_FMSystemDLL.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_FMSystemDLL.BLL
{
    /// <summary>
    /// 20151210 - Gerry create this new audit log
    /// sstructure will be same in SF, AF audit log
    /// </summary>
    public class AuditLog
    {
        // temp write log
        static int logCount = 0;

        public int LogNo { get; set; }
        public string PMKey { get; set; }
        public string TrxType { get; set; }
        public string EntryForm { get; set; }
        public int TrxNo { get; set; }
        public string LogUserID { get; set; }
        public DateTime LogDateTime { get; set; }
        public string TLevel { get; set; }
        public int ItemKey { get; set; }
        public string EventMode { get; set; }
        
        List<string> logMessages = new List<string>(); 

        public AuditLog()
        {
            this.LogNo = 0;
            this.PMKey = string.Empty;
            this.TrxType = string.Empty;
            this.EntryForm = string.Empty;
            this.TrxNo = 0;
            this.LogUserID = string.Empty;
            this.LogDateTime = DateTime.UtcNow;
            this.TLevel = string.Empty;
            this.ItemKey = 0;
            this.EventMode = string.Empty;
        }
        public AuditLog(string pmKey, string trxType, string entryForm, int trxNo, string logUserID, DateTime logDateTime, string TLevel, int itemKey, string eventMode)
        {
            this.LogNo = 0;
            this.PMKey = pmKey;
            this.TrxType = trxType;
            this.EntryForm = entryForm;
            this.TrxNo = trxNo;
            this.LogUserID = logUserID;
            this.LogDateTime = logDateTime;
            this.TLevel = TLevel;
            this.ItemKey = itemKey;
            this.EventMode = eventMode;
        }

        /// <summary>
        /// obj will hold the field and newValue
        /// sqlconnection and sqltransaction will be started from the calling method
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="con"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool WriteAuditLog(object newObj, object oldObj, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                if (newObj != null)
                {
                    //use linq to get new obj property name and values
                    var newObjPropertyNamesAndValues = newObj.GetType().GetProperties()
                        .Where(pi =>  pi.GetGetMethod() != null && (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(Boolean) || pi.PropertyType == typeof(object)))
                        .Select(pi => new
                        {
                            Name = pi.Name,
                            Value = pi.GetGetMethod().Invoke(newObj, null)
                        });

                    foreach (var propNew in newObjPropertyNamesAndValues)
                    {
                        if (propNew.Value != null)
                        {
                            if (!propNew.Value.Equals(string.Empty))
                            {
                                if (propNew.Name.Contains("JobID")) //for double mount with different job it will be 0
                                {
                                    //this.TrxNo =Convert.ToInt32(propNew.Value);
                                    this.TrxNo = propNew.Value.ToString().Contains("+") ? 0 : Convert.ToInt32(propNew.Value);
                                }
                                if (propNew.Name.Contains("Seq")) //for double mount it will be 0
                                {
                                    //this.ItemKey = Convert.ToInt32(propNew.Value);
                                    this.ItemKey = propNew.Value.ToString().Contains("+") ? 0 : Convert.ToInt32(propNew.Value);
                                }
                            }
                        }
                        if (oldObj != null)
                        {
                            //use linq to get old obj property values
                            var oldObjPropertyNamesAndValues = oldObj.GetType().GetProperties()
                                .Where(pi => pi.GetGetMethod() != null) // && (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(Boolean) || pi.PropertyType == typeof(object)))
                                .Select(pi => new
                                {
                                    Name = pi.Name,
                                    Value = pi.GetGetMethod().Invoke(oldObj, null)
                                });
                            foreach (var propOld in oldObjPropertyNamesAndValues)
                            {  //write only those values being changed
                                if (propOld.Value != null && propNew.Value != null)
                                {
                                    if (propNew.Name.ToString() == propOld.Name.ToString() && propNew.Value.ToString() != propOld.Value.ToString())
                                    {
                                        string value = propNew.Value == null ? string.Empty : propNew.Value.ToString();
                                        LoggerDAL.WriteAuditLog(this, propNew.Name, value, propOld.Value, con, tran);
                                        logMessages.Add(propNew.Name + "\t" + value + "\t" + propOld.Value);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string value = propNew.Value == null ? string.Empty : propNew.Value.ToString();
                            if (value != string.Empty)
                            {
                                LoggerDAL.WriteAuditLog(this, propNew.Name.ToString(), value, string.Empty, con, tran);
                                logMessages.Add(propNew.Name + "\t" + value + "\t" + "Empty");
                            }
                        }
                    }
                }
                else // incase for delete
                {
                    LoggerDAL.WriteAuditLog(this, string.Empty, string.Empty, string.Empty, con, tran);                    
                }
                LogWrite();
            }
            catch (FMException FMEx) { throw FMEx; }
            catch (OverflowException e) { throw new FMException(e.Message.ToString()); }
            catch (FormatException e) { throw new FMException(e.Message.ToString()); }
            catch (ArgumentNullException e) { throw new FMException(e.Message.ToString()); }
            catch (Exception Ex) { throw new FMException(Ex.Message.ToString()); }

            return true;
        }

        public bool WriteAuditLog(string propName, string newValue, string oldValue, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                LoggerDAL.WriteAuditLog(this, propName, newValue, oldValue, con, tran);
                logMessages.Add(propName + "\t" + newValue + "\t" + oldValue);
                LogWrite();
            }
            catch (FMException FMEx) { throw; }
            catch (Exception Ex) { throw new FMException(Ex.Message.ToString()); }

            return true;
        }

        public void LogWrite()
        {
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "Transport.txt"))
                {
                    w.Write("\r\nTransport Log Entry : ");
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                    var ObjPropertyNamesAndValues = this.GetType().GetProperties()
                           .Where(pi => pi.PropertyType == typeof(string) && pi.GetGetMethod() != null)
                           .Select(pi => new
                           {
                               Name = pi.Name,
                               Value = pi.GetGetMethod().Invoke(this, null)
                           });
                    string logMessage = string.Empty;
                    string logHeaderText = string.Empty;
                    foreach (var propNew in ObjPropertyNamesAndValues)
                    {
                        logHeaderText += propNew.Name.ToString() + "\t"; 
                        logMessage += propNew.Value.ToString() + "\t";                        
                    }
                    logHeaderText +=  "FieldName\tNewValue\tOldValue"; 
                    w.WriteLine("  :{0}", logHeaderText);
                    if (logMessages.Count > 0)
                    {
                        foreach (string str in logMessages)
                        {
                            string log = logMessage + str;
                            w.WriteLine("  :{0}", log);
                        }
                    }
                    w.WriteLine(" -----------------------------------------------------------------");
                }
            }
            catch { }
        }

        public void LogError(string errorMessage)
        {
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "TransportErrorLog.txt"))
                {
                    w.Write("\r\nTransport Error Log Entry : ");
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                    w.WriteLine("  :{0}", errorMessage);
                    w.WriteLine(" -----------------------------------------------------------------");
                }
            }
            catch { }
        }
        //write textfile
        public static void EventLog(string eventName, object objChange, DateTime start, DateTime end, string userId, bool isTruck = false)
        {
            string m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                eventName = isTruck ? ("TRK - " + eventName) : ("HL - " + eventName);
                var newObjPropertyNamesAndValues = objChange.GetType().GetProperties()
                    .Where(pi => /*pi.PropertyType == typeof(string) && */ pi.GetGetMethod() != null)
                    .Select(pi => new
                    {
                        Name = pi.Name,
                        Value = pi.GetGetMethod().Invoke(objChange, null)
                    });

                string msg = string.Empty;
                foreach (var propNew in newObjPropertyNamesAndValues)
                {
                    if (propNew.Name.Equals("DriverNumber") || propNew.Name.Equals("Trailer") || propNew.Name.Equals("containerNos") || propNew.Name.Equals("Start") || propNew.Name.Equals("End") || propNew.Name.Equals("Status"))
                    {
                        msg += propNew.Name + ":" + (propNew.Value == null ? " - " : propNew.Value.ToString()) + "\t";
                    }
                }
                //msg += "New StartTime:" + start.ToString("dd/MM/yyyy hh:mm") + "\tNew EndTime:" + end.ToString("dd/MM/yyyy hh:mm") + "\t";
                if (msg != string.Empty)
                {
                    using (StreamWriter w = File.AppendText("C:\\FM\\" + "Transport_Planning_EventLog.txt"))
                    {
                        if (logCount == 0)
                        {
                            w.Write("\r\nTransport Event Log Entry  \n\n");
                            w.WriteLine(DateTime.Now.ToLongDateString());
                        }
                        w.WriteLine("{0} - {1} UserId:{2} Time:{3}", eventName, msg, userId, DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
                    }
                }
                logCount++;
            }
            catch (FMException FMEx) { throw FMEx; }
            catch (Exception Ex) { throw new FMException(Ex.Message.ToString()); }
        }
    }
}
