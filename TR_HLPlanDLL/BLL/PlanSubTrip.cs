using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_HLBookDLL.BLL;
using TR_MessageDLL.BLL;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_HLPlanDLL.BLL
{
    public abstract class PlanSubTrip
    {
        private string planTripNo;
        private int seqNo; 
        private DateTime start;
        private DateTime end;
        private Stop startStop;
        private Stop endStop;
        private bool isBillableTrip;
        private string description;
        private string vehicleNumber;
        private string driverNumber;
        private JobTripStatus status;
        public byte[] updateVersion;
        public string Task_ID { get; set; } //20170302 fro truckcomm messaging
        public MessageTripStatus messageTripStatus { get; set; }

        public string PlanTripNo
        {
            get { return planTripNo; }
            set { planTripNo = value; }
        }

        public int SeqNo
        {
            get { return seqNo; }
            set { seqNo = value; }
        }

        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        public DateTime End
        {
            get { return end; }
            set { end = value; }
        }

        public Stop StartStop
        {
            get { return startStop; }
            set { startStop = value; }
        }

        public Stop EndStop
        {
            get { return endStop; }
            set { endStop = value; }
        }

        public bool IsBillableTrip
        {
            get { return isBillableTrip; }
            set { isBillableTrip = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string VehicleNumber
        {
            get { return vehicleNumber; }
            set { vehicleNumber = value; }
        }

        public string DriverNumber
        {
            get { return driverNumber; }
            set { driverNumber = value; }
        }

        public JobTripStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public PlanSubTrip()
        {
            this.planTripNo = string.Empty;
            this.seqNo = 0;
            this.start = DateTime.Today;
            this.end = DateTime.Today;
            this.isBillableTrip = true;
            this.description = "";
            this.vehicleNumber = "";
            this.status = JobTripStatus.Assigned;
            this.startStop = new Stop();
            this.endStop = new Stop();
            this.updateVersion = new byte[8];
            this.Task_ID = "0"; //default to "0"
            this.messageTripStatus = MessageTripStatus.None;
        }

        public PlanSubTrip(int seqNo, DateTime start, DateTime end, Stop startStop,
            Stop endStop, bool isBillableTrip, string description, PlanTrip trip, JobTripStatus status)
        {
            this.planTripNo = string.Empty;
            this.seqNo = seqNo;
            this.start = start;
            this.end = end;
            this.startStop = startStop;
            this.endStop = endStop;
            this.isBillableTrip = isBillableTrip;
            this.description = description;
            this.vehicleNumber = trip.Vehicle.Number;
            this.driverNumber = trip.Driver.Code;
            this.status = status;
            this.Task_ID = "0"; //default to "0"
            this.messageTripStatus = MessageTripStatus.None;
        }


    }


}
