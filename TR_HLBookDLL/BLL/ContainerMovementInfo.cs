using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_HLBookDLL.BLL
{
    /// <summary>
    /// this class is use to display planning information in booking
    /// under job trip
    /// </summary>
    public class ContainerMovementInfo
    {
        public int JobID { get; set; }
        public int SeqNo { get; set; }
        public string DriverName { get; set; }
        public string PrimeMover { get; set; }
        public string TrailerNo { get; set; }
        public DateTime ScheduleStarted { get; set; }
        public DateTime ScheduleFinished { get; set; }

        public ContainerMovementInfo()
        {
            this.JobID = 0;
            this.SeqNo = 0;
            this.DriverName = string.Empty;
            this.PrimeMover = string.Empty;
            this.TrailerNo = string.Empty;
            this.ScheduleStarted = DateTime.Today;
            this.ScheduleFinished = DateTime.Today;
        }
        
    }
}
