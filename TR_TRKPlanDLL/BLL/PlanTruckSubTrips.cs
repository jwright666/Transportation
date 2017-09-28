using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_TRKPlanDLL.BLL
{
    public class PlanTruckSubTrips : SortableList<PlanTruckSubTrip>
    {

        private const string startTime = "Start";

        public void SortByStartTime()
        {
            if (Items.Count != 0)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this.Items[0]);
                PropertyDescriptor propStart = props.Find(startTime, true);
                base.ApplySortCore(propStart, ListSortDirection.Ascending);
            }
        }


    }
}
