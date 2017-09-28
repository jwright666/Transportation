using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class DriverUnavailable
    {
        public string driverCode { get; set; }
        public int seqNo { get; set; }         //20140701 - added
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public string remarks { get; set; }

        public DriverUnavailable()
        {
            this.driverCode = string.Empty;
            this.seqNo = 0;
            this.startDateTime = DateTime.Now;
            this.endDateTime = DateTime.Now;
            this.remarks = string.Empty;
        }

    }
}
