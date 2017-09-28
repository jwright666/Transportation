using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_HLPlanDLL.BLL
{
    public class PlanHaulierSubTrips : SortableList<PlanHaulierSubTrip>
    {
        private const string startTime = "Start";

        public void SortByStartTime()
        {
            if (Items.Count != 0)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this.Items[0]);
                PropertyDescriptor propStart = props.Find(startTime, true);
                base.ApplySortCore(propStart, ListSortDirection.Ascending);

                // Now reassign the sequence no
                int seqNo = 1;
                foreach (PlanHaulierSubTrip planHaulierSubTrip in this)
                {
                    planHaulierSubTrip.SeqNo = seqNo;
                    seqNo += 1;
                }
            }
        }

    }
}
