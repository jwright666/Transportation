using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace FM.TR_FMSystemDLL.BLL
{
    public class DateUtility
    {
        public static string ConvertDateAndTimeForSQLPurpose(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string hour = date.Hour.ToString();
            string min = date.Minute.ToString();
            string sec = date.Second.ToString();
            string tempdate = "";
            tempdate = year +"-"+ month +"-"+ day+" "+hour+":"+min+":"+sec;
            return tempdate;
        }

        public static string ConvertDateForSQLPurpose(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string tempdate = "";
            tempdate = year + "-" + month + "-" + day;
            return tempdate;
        }

        public static string ConvertDateAndTimeWithZeroSecForSQLPurpose(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string hour = date.Hour.ToString();
            string min = date.Minute.ToString();
            string sec = date.Second.ToString();
            string tempdate = "";
            tempdate = year + "-" + month + "-" + day + " " + hour + ":" + min + ":00";
            return tempdate;
        }

        //20130503 - Gerry added method to get sql smalldatetime minimum value
        //reason some sql table fields are smalldatetime datatype range(January 1, 1900 to June 6, 2079)
        public static DateTime GetSQLDateTimeMinimumValue()
        {
            return new DateTime(1900, 1, 1, 0, 0, 0); // January 1, 1900 00:00:00
        }
        //20130503 - Gerry added method to get sql smalldatetime maximum value  
        //reason some sql table fields are smalldatetime datatype
        //this can be use in search  end  criteria
        public static DateTime GetSQLDateTimeMaximumValue()
        {
            return new DateTime(2079, 6,6, 0, 0, 0); // June 6, 2079 00:00:00
        }
/*
        public DateTime ConvertBritishToAmericanDate(DateTime date)
        {
            
        }

        public DateTime ConvertAmericanToBritishDate(DateTime date)
        {
        }
*/
    }
}
