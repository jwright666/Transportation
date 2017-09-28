using System;
using System.Collections.Generic;
using FM.TR_FMSystemDLL.BLL;
using System.Text;
using TR_LanguageResource.Resources;

namespace FM.TR_HLBookDLL.BLL
{
    public class JobTripState
    {
        private int seq_no;
        private JobTripStatus status;
        private DateTime statusDate;
        private string remarks;
        private bool isNew;

        public JobTripState()
        {
            this.isNew = true;
            this.seq_no = 0;
            this.status = JobTripStatus.Booked;
            this.statusDate = DateTime.Today;
            this.remarks = "";
        }


        public JobTripState(int seq_no,JobTripStatus status,DateTime statusDate,string remarks, bool isNew)
        {
            this.isNew = isNew;
            this.Seq_No = seq_no;
            this.Status= status;
            this.StatusDate = statusDate;
            this.Remarks = remarks;
        }

        public int Seq_No
        {
            get { return seq_no; }
            set
            {
                if (isNew == true)
                {
                    seq_no = value;
                }
                else
                {
                    throw new FMException(TptResourceBLL.ErrSequenceNoCannotEdit);
                }
            }
        }

        public JobTripStatus Status
        {
            get { return status; }
            set 
            {
                if (isNew == true)
                {
                    status = value;
                }
                else
                {
                    throw new FMException(TptResourceBLL.ErrJobTripStateCannotEdit);
                }
            }
        }

        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }
        //20140807 - gerry added to clone the states
        public JobTripState Clone()
        {
            JobTripState retValue = new JobTripState();
            retValue.seq_no = this.seq_no;
            retValue.status = this.status;
            return retValue;
        }
    }
}
