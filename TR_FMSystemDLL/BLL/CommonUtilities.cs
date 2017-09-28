using System;
using System.Collections.Generic;
using System.Text;

namespace FM.TR_FMSystemDLL.BLL
{
    
    public class CommonUtilities
    {
        //We can add more utilities here for future projects


        //20121109 - Gerry Added this method to format strings with ' for sql query purpose.
        public static String FormatString(string stringToFormat)
        {
            string retValue = "";
            try
            {
                if (stringToFormat != null)
                {
                    retValue = stringToFormat.Replace("'", "''");
                }
            }
            catch (Exception ex) { throw ex; }

            return retValue;
        }
        //end 20121109
    }
}
