using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace TR_AirFreightDLL.BLL
{
    //use by air import/export jobs
    //tables AI_JOB_DTL_RATE_TBL or AE_JOB_DTL_RATE_TBL
    public class AirJobRate
    {
        private int jobID;
        private int rateSeqNo;
        private decimal grossWeight;
        private string goodsDescription;
        public SortableList<AirJobDtlDimension> airJobDtlDimensions;

        public AirJobRate()
        {
            this.jobID = 0;
            this.rateSeqNo = 0;
            this.GrossWeight = 0;
            this.goodsDescription = string.Empty;
            this.airJobDtlDimensions = new SortableList<AirJobDtlDimension>();
        }

        #region Getter/Setter Methods
        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }
        public int RateSeqNo
        {
            get { return rateSeqNo; }
            set { rateSeqNo = value; }
        }
        public decimal GrossWeight
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }
        public string GoodsDescription
        {
            get { return goodsDescription; }
            set { goodsDescription = value; }
        }
        #endregion
    }
}
