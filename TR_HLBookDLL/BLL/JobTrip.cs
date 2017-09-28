using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_HLBookDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using System.Data.SqlClient;
//using FM.TransportBook.Resources;
using FM.TR_MaintenanceDLL.BLL;
using TR_LanguageResource.Resources;

namespace FM.TR_HLBookDLL.BLL
{
    public abstract class JobTrip
    {
        private int jobID;
        private string jobNo;
        private int sequence;
        private Stop startStop;
        private Stop endStop;
        private DateTime endDate; 
        private DateTime startDate;
        private string startTime;
        private string endTime;
        private JobTripStatus tripStatus;
        private SortableList<JobTripState> jobTripStates;
        private byte[] updateVersion;
        private bool isNew;
        private string customerCode;
        private string customerName;
        protected bool ownTransport;
        //private string subContractorCode;   //201403012 -gerry Replaced with stop object to cater address and city
        private string subContractorReference;
        private string chargeCode;
        private string remarks;

        //20130522 -gerry added
        public string jobType { get; set; }
        //20130522 end                           
        //201403012 - gerry added Subcon changes
        //20140402 - gerry changed Subcon to operationDTO instead of customerDTO
       // public CustomerDTO subCon { get; set; }
        public OperatorDTO subCon { get; set; }
        //201403012 end
        //20150730 - gerry added
        public bool isMovementJob { get; set; }
        //20161008 - gerry added
        public string JobCreatorId { get; set; }
        public DateTime JobCreatedDateTime { get; set; }
        public string LastModified_By { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        //20161209 - gerry added
        public bool isFixedTime { get; set; }
        public TIME_BOUND timeBound { get; set; }


        public JobTrip()
        {
            this.jobID = 0;
            this.jobNo = "";
            this.sequence = 0;
            this.startStop = new Stop();
            this.endStop = new Stop();
            this.startDate = DateTime.Now;
            this.endDate = DateTime.Now;
            this.tripStatus = JobTripStatus.Booked;
            this.updateVersion = new byte[8]; 
            this.isNew=true;
            this.startTime="0000";
            this.endTime="0000";
            this.jobTripStates = new SortableList<JobTripState>();
            this.ownTransport = true;
            //this.subContractorCode = "";
            this.subContractorReference = "";
            this.chargeCode = "";
            this.remarks = "";
            this.jobType = "";
            this.subCon = new OperatorDTO(); //201040502 - gerry change to operatorDTO,   new CustomerDTO(); // 20140312 -gerry added
            this.isMovementJob = true;
            this.JobCreatorId = string.Empty;
            this.JobCreatedDateTime = DateUtility.GetSQLDateTimeMinimumValue();
            this.LastModified_By = string.Empty;
            this.LastModifiedDateTime = DateUtility.GetSQLDateTimeMinimumValue();
            this.isFixedTime = false; //20161209
            this.timeBound = TIME_BOUND.NONE; //20161209
        }

        // constructor
        //20140312 - gerry change parameter subcontractor code to subcon object
        //201040502 - gerry change subCon from CustomerDTO to operatorDTO
        public JobTrip(int jobID,int sequence,DateTime startDate, Stop startStop, DateTime endDate, Stop endStop,
            string startTime, string endTime, JobTripStatus status, SortableList<JobTripState> jobTripStates,
            byte[] updateVersion, bool isNew, bool ownTransport,
            OperatorDTO subCon, string subContractorReference, string chargeCode, string remarks)
        {
            this.JobID = jobID;
            this.Sequence = sequence;
            this.StartDate = startDate;
            this.StartStop = startStop;
            this.EndDate = endDate;
            this.EndStop = endStop;
            this.TripStatus = status;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.UpdateVersion = updateVersion;
            this.IsNew = isNew;
            this.jobTripStates = jobTripStates;
            this.ownTransport = ownTransport;
            this.subCon = subCon;     //20140312 - gerry change parameter subcontractor code to subcon object
            this.subContractorReference = subContractorReference;
            this.chargeCode = chargeCode;
            this.Remarks = remarks;
            this.isMovementJob = true;
            this.JobCreatorId = string.Empty;
            this.JobCreatedDateTime = DateUtility.GetSQLDateTimeMinimumValue();
            this.LastModified_By = string.Empty;
            this.LastModifiedDateTime = DateUtility.GetSQLDateTimeMinimumValue();
            this.isFixedTime = false; //20161209
            this.timeBound = TIME_BOUND.NONE; //20161209
        }

        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }

        public string JobNo
        {
            get { return jobNo; }
            set { jobNo = value; }
        }

        public int Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }
	
        public JobTripStatus TripStatus
        {
            get { return tripStatus; }
            set { tripStatus = value; }
        }
	            
	    public DateTime EndDate
	    {
		    get { return endDate;}
		    set {
                        endDate = value.Date;
                }
	    }
    	
        public DateTime StartDate
        {
            get { return startDate; }
            set {
                        startDate = value.Date;
                }
        }

        public string EndTime
        {
            get { return endTime; }
            set {
                        endTime = value;
                }
        }

        public string StartTime
        {
            get { return startTime; }
            set {
                        startTime = value;
                }
        }

        public Stop EndStop
        {
            get { return endStop; }
            set
            {
                    endStop = value;
            }
        }
	
        public Stop StartStop
        {
            get { return startStop; }
            set 
            {

                    startStop = value;
            }
        }

        public SortableList<JobTripState> JobTripStates
        {
            get { return jobTripStates; }
            set { jobTripStates = value; }
        }

        public byte[] UpdateVersion
        {
            get { return updateVersion; }
            set { updateVersion = value; }
        }

        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }

        public string CustomerCode
        {
            get { return customerCode; }
//            internal set { customerCode = value; }
            set { customerCode = value; }
        }

        public string CustomerName
        {
            get { return customerName; }
//            internal set { customerName = value; }
            set { customerName = value; }
        }

        public virtual bool OwnTransport
        {
            get { return ownTransport; }
            set { ownTransport = value; }
        }
         //20140312 -gerry removed
        //public string SubContractorCode
        //{
        //    get { return subContractorCode; }
        //    set { subContractorCode = value; }
        //}

        public string SubContractorReference
        {
            get { return subContractorReference; }
            set { subContractorReference = value; }
        }

        public string ChargeCode
        {
            get { return chargeCode; }
            set { chargeCode = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }



        public bool ValidateJobTripStateForAdd(JobTripStatus jobTripStatus)
        {
            bool temp = true;
            if (jobTripStates.Count == 0)
            {
                ApplicationOption option = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_SET_JOBTRIP_TO_READY);
                if (option.setting_Value != "T" && jobTripStatus != JobTripStatus.Booked)
                {
                    temp = false;
                    throw new FMException(TptResourceBLL.ErrJobTripMustBeBooked);                    
                } 
            }
            else
            {
                if (jobTripStates[jobTripStates.Count - 1].Status == JobTripStatus.Booked)
                {
                    if ((jobTripStatus != JobTripStatus.Ready) && (jobTripStatus != JobTripStatus.Assigned))
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrJobTripMustBeReadyOrAssigned);
                        
                    }
                }
                /*
                                if (jobTripStates[jobTripStates.Count - 1].Status == JobTripStatus.Ready)
                                {
                                    if ((jobTripStatus != JobTripStatus.Assigned) && (jobTripStatus != JobTripStatus.Completed))
                                    {
                                        temp = false;
                                        throw new FMException(ResJobTrip.ErrorJobTripMustBeAssignedOrCompleted);
                        
                                    }
                                }
                
                if (jobTripStates[jobTripStates.Count - 1].Status == JobTripStatus.Assigned)
                {
                    if (jobTripStatus != JobTripStatus.Completed)
                    {
                        temp = false;
                        throw new FMException(ResJobTrip.ErrorJobTripMustBeCompleted);                        
                    }
                }
                */
            }

            return temp;
        }

        public bool ValidateJobTripStateForEdit(JobTripState jobTripState)
        {
            bool found = false;
            for (int i = 0; i < jobTripStates.Count; i++)
            {
                if (jobTripStates[i].Status == jobTripState.Status)
                {
                    found = true;
                }
            }
            return found;
        }


        public bool ValidateStartDateAndEndDate()
        {
            bool temp = true;
            if (DateTime.Compare(endDate, startDate) == 0)
            {
                temp = false;
            }
            if (DateTime.Compare(endDate, startDate) < 0)
            {
                temp = false;
            }
            return temp;
        }


    }
}
