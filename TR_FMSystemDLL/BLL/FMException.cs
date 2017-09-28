using System;
using System.Collections.Generic;
using System.Text;

namespace FM.TR_FMSystemDLL.BLL
{
    public class FMException:Exception
    {
        private string exception;
        public const string MANDATORY_FIELDS = "Please fill in the mandatory fields in blue";
        public const string NUMERIC_FIELDS = "Please check that numeric fields are fill in with numbers only ";
        public const string CLASS_VALIDATION_ERROR = "A class validation error occurred in: ";
        public const string WRONG_VALUE = "A required field has the wrong value";
        public const string REQUIRED_FIELD = "A required field is blank"; 


        public FMException(string exception)
        {
            this.exception = exception;
        }

        // 2014-03-18 Zhou Kai adds keyword override
        public override string ToString()  
        {
            return exception;
        }

        // 2014-03-18 Zhou Kai adds
        public override string Message
        {
            get{return this.exception;}
        }
        // 2014-03-18 Zhou Kai ends
    }
}
