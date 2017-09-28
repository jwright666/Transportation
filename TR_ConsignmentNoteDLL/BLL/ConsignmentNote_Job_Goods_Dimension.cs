using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TR_ConsignmentNoteDLL.BLL
{
    public class ConsignmentNote_Job_Goods_Dimension
    {
        public int Job_ID { get; set; }
        public int Seq_No { get; set; }
        public int Sub_Seq_No { get; set; }
        public decimal Qty { get; set; }
        public decimal Gross_Weight { get; set; }
        public decimal Nett_Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal CBM { get; set; }

        public ConsignmentNote_Job_Goods_Dimension()
        {
            this.Job_ID = 0;
            this.Seq_No = 0;
            this.Sub_Seq_No = 0;
            this.Qty = 0;
            this.Gross_Weight = 0;
            this.Nett_Weight = 0;
            this.Length = 0;
            this.Width = 0;
            this.Height = 0;
            this.CBM = 0;
        }
    }
}
