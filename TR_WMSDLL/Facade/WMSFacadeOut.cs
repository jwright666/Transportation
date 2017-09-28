using FM.TR_FMSystemDLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TR_WMSDLL.BLL;
using TR_WMSDLL.DAL;

namespace TR_WMSDLL.Facade
{
    public class WMSFacadeOut
    {
        public static SortableList<GINHeader> getGINOutStandingGINs()
        {
            return GINDAL.GetOutStandingGINFromWMS();
        }
        public static SortableList<GINDetail> getGINDetails(string ginNo)
        {
            return GINDAL.GetOutStandingGINDetailsFromWMS(ginNo);
        }
    }
}
