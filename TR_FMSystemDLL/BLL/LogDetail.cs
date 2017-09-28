using System;
using System.Collections.Generic;
using System.Text;

namespace FM.TR_FMSystemDLL.BLL
{
    public class LogDetail
    {
        private int logID;
        private string propertyName;
        private string propertyValue;

        public LogDetail(string propertyName, string propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public LogDetail(int logId, string propertyName, string propertyValue)
        {
            LogID = logId;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public int LogID
        { 
            get { return logID; }
            set { logID = value; }
            
        }

        public string PropertyName
        {
            get { return propertyName; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(FMException.REQUIRED_FIELD + " Property Name");
                }
                propertyName = value;
            }
        }

        public string PropertyValue
        {
            get { return propertyValue; }
            set
            {
                value = value.Trim();
                /*
                if (value.Length == 0)
                {
                    throw new FMException(FMException.REQUIRED_FIELD + " Property Value");
                }
                 */
                propertyValue = value;
            }
        }

    }
}
