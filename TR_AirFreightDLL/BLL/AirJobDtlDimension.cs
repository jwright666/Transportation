using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TR_AirFreightDLL.BLL
{       
    //use by air import/export jobs
    //tables AI_JOB_DTL_DIMENSION_TBL or AE_JOB_DTL_DIMENSION_TBL
    public class AirJobDtlDimension
    {
        private int jobID;
        private int rateSeqNo;
        private int dimenSeqNo;
        private string uom;
        private int qty;
        private decimal grossWeight;//20170424
        private decimal length;
        private decimal width;
        private decimal height;

        public AirJobDtlDimension()
        {
            this.jobID = 0;
            this.rateSeqNo = 0;
            this.dimenSeqNo = 0;
            this.uom = string.Empty;
            this.qty = 0;
            this.grossWeight = 0;
            this.length = 0;
            this.width = 0;
            this.height = 0;
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
        public int DimenSeqNo
        {
            get { return dimenSeqNo; }
            set { dimenSeqNo = value; }
        }
        public string UOM
        {
            get { return uom; }
            set { uom = value; }
        }
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }        
        public decimal GrossWeight //20170424
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }
        public decimal Length
        {
            get { return length; }
            set { length = value; }
        }
        public decimal Width
        {
            get { return width; }
            set { width = value; }
        }
        public decimal Heigth
        {
            get { return height; }
            set { height = value; }
        }
        #endregion
    }
}
