using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FM.TR_SeaFreightDLL.BLL
{       
    public class JobDimension
    {
        private int jobID;
        private int seqNo;
        private int dimenSeqNo;
        private string uom;
        private int qty;
        private decimal unitWeight;//20170424
        private decimal length;
        private decimal width;
        private decimal height;

        public JobDimension()
        {
            this.jobID = 0;
            this.seqNo = 0;
            this.dimenSeqNo = 0;
            this.uom = string.Empty;
            this.qty = 0;
            this.unitWeight = 0;
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
        public int SeqNo
        {
            get { return seqNo; }
            set { seqNo = value; }
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
        public decimal UnitWeight //20170424
        {
            get { return unitWeight; }
            set { unitWeight = value; }
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
        public decimal Height
        {
            get { return height; }
            set { height = value; }
        }
        #endregion
    }
}
