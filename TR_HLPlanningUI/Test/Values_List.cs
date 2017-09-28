using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FM.TransportPlanDLL.Test
{
    public class Values_List
    {
        public static List<Values> AllValues = new List<Values>
        {
            new Values("1", "100", "the first values", "NA"),
            new Values("2", "101", "the second values", "NA"),
            new Values("3", "102", "the third values", "NA"),
            new Values("4", "103", "the fourth values", "NA"),
            new Values("5", "104", "the fifth values", "NA"),
            new Values("6", "105", "the sixth values", "NA"),
            new Values("7", "106", "the seventh values", "NA"),
            new Values("8", "107", "the eighth values", "NA"),
        };

        public Values_List()
        {
            // do nothing here
        }
    }
}