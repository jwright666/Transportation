using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_HLBookDLL.DAL;

namespace FM.TR_HLBookDLL.BLL
{
    public abstract class VehicleJob
    {
        protected string custNo;
        protected string jobType;
        protected DateTime jobDateTime;
        protected string quotationNo;
        private int jobID;
        private string jobNo;
        private string sourceReference;
        private JobStatus jobStatus;
        private string branchCode;
        private byte[] updateVersion;
        private bool isNew;
        private JobUrgencyType jobUrgencyType;
        private bool billByTransport;
        private string tptDeptCode;
        private string oldTptDeptCode;
        // 2014-06-18 Zhou Kai adds member
        private bool isBillable;
        // 2014-06-18 Zhou Kai ends
        //20151001 - gerry added
        public string JobCreatorId { get; set; }
        public DateTime JobCreatedDateTime { get; set; }
        public bool isFlexibleTime { get; set; }
        public string LastModified_By { get; set; }
        public DateTime LastModifiedDateTime { get; set; }


        public VehicleJob()
        {
            this.jobID = 0;
            this.jobNo = "";
            this.custNo = "";
            this.sourceReference = "";
            this.jobStatus = JobStatus.Booked;
            this.jobType = "";
            this.jobDateTime = DateTime.Now;
            this.branchCode = "";
            this.quotationNo = "";
            this.updateVersion = new byte[8]; 
            this.isNew = true;
            this.jobUrgencyType = JobUrgencyType.Normal;
            this.billByTransport = true;
            this.tptDeptCode = "";
            this.oldTptDeptCode = "";
            // 2014-06-18 Zhou Kai adds
            this.isBillable = true;
            // 2014-06-18 Zhou Kai ends
            this.JobCreatorId = string.Empty;
            this.JobCreatedDateTime = DateTime.Now;
            this.isFlexibleTime = true;
            this.LastModified_By = string.Empty;
            this.LastModifiedDateTime = DateUtility.GetSQLDateTimeMinimumValue();

        }

        public VehicleJob(int jobID, string jobNo, string custNo, string sourceReference,
            JobStatus jobStatus, string jobType, string bookingNo, DateTime bookingDate,
            string branchCode, byte[] updateVersion, bool isNew, string quotationNo,
            JobUrgencyType jobUrgencyType, bool billByTransport, string tptDeptCode, string oldTptDeptCode
            /*2014-06-18 Zhou Kai adds*/, bool isBillable)
        {
            this.JobID = jobID;
            this.SourceReferenceNo = sourceReference;
            this.JobNo = jobNo;
            this.JobStatus = jobStatus;
            this.BranchCode = branchCode;
            this.UpdateVersion = updateVersion;
            this.IsNew = isNew;
            this.QuotationNo = quotationNo;
            this.jobUrgencyType = jobUrgencyType;
            this.BillByTransport = billByTransport;
            this.TptDeptCode = tptDeptCode;
            this.OldTptDeptCode = oldTptDeptCode;
            this.isBillable = isBillable;
            this.JobCreatorId = string.Empty;
            this.JobCreatedDateTime = DateTime.Now;
            this.isFlexibleTime = true;
            this.LastModified_By = string.Empty;
            this.LastModifiedDateTime = DateUtility.GetSQLDateTimeMinimumValue();
        }

        // because isBillable is public scoped, so no public property for it

        public virtual string CustNo
        {
            get { return custNo; }
            set
            {
                if (value.Length == 0)
                {
                    throw new FMException(FMException.MANDATORY_FIELDS + ", Customer No");
                }
                custNo = value;
            }
        }

        public virtual string JobType
        {
            get { return jobType; }
            set { jobType = value; }
        }

        public virtual DateTime JobDateTime
        {
            get { return jobDateTime; }
            set { jobDateTime = value; }
        }

        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }

        public JobStatus JobStatus
        {
            get { return jobStatus; }
            set
            {
                if (value != JobStatus.Booked & value != JobStatus.Cancelled 
                    & value != JobStatus.Billed & value != JobStatus.Completed
                    & value != JobStatus.Closed)
                {
                    throw new FMException("Job Status not Valid");
                }
                if (jobStatus == JobStatus.Cancelled)
                {
                    if (value == JobStatus.Billed)
                    {
                        throw new FMException("Cannot billed a job that is cancelled");
                    }
                }

                if (jobStatus == JobStatus.Billed)
                {
                    if ((value == JobStatus.Booked) | (value == JobStatus.Cancelled) )
                    {
                        throw new FMException("Cannot Edit Job that Already Billed");
                    }
                }
                jobStatus = value;
            }
        }


        public string SourceReferenceNo
        {
            get { return sourceReference; }
            set { sourceReference = value; }
        }

        public string JobNo
        {
            get { return jobNo; }
            set { jobNo = value; }
        }

        public string TptDeptCode
        {
            get { return tptDeptCode; }
            set {
                    if (value.Length == 0)
                    {
                        throw new FMException("Dept Code Can't be Blank");
                    }                
                
                    tptDeptCode = value; 
                }
        }

        public string OldTptDeptCode
        {
            get { return oldTptDeptCode; }
            set { oldTptDeptCode = value; }
        }

        public string BranchCode
        {
            get { return branchCode; }
            set
            {
                if (value.Length == 0)
                {
                    throw new FMException("Branch Code Can't be Blank");
                }
                branchCode = value;
            }
        }

        public bool IsBillable
        {
            get { return isBillable; }
            set { isBillable = value; }
        }

        protected bool ValidateJobHeader()
        {
            JobNo = this.jobNo;    
            CustNo = this.custNo;    
            SourceReferenceNo = this.sourceReference;    
            JobStatus = this.jobStatus;    
            JobType = this.jobType;    
            JobDateTime = this.jobDateTime;    
            BranchCode = this.branchCode;    
            IsNew = this.isNew;    
            QuotationNo = this.quotationNo;
            BillByTransport = this.billByTransport;
            TptDeptCode = this.tptDeptCode;
            OldTptDeptCode = this.oldTptDeptCode;
            return true;
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

        public string QuotationNo
        {
            get { return quotationNo; }
            set { quotationNo = value; }
        }

        public JobUrgencyType JobUrgencyType
        {
            get { return jobUrgencyType; }
            set { jobUrgencyType = value; }
        }

        public bool BillByTransport
        {
            get { return billByTransport; }
            set { billByTransport = value; }
        }


        public bool IsJobBilled()
        {
            bool temp = false;

            temp = HaulierJobDAL.IsJobBilled(this.jobNo);

            return temp;
        }
        public bool IsJobPosted()
        {
            bool temp = false;

            temp = HaulierJobDAL.IsJobPosted(this.jobNo);

            return temp;
        }
    }
}
