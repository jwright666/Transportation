using FM.TR_FMSystemDLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TR_ConsignmentNoteDLL.BLL
{
    public class ConsignmentNote_Job_Goods_Details
    {
        public int Job_ID { get; set; }
        public int Seq_No { get; set; }
        public string Goods_Desc { get; set; }
        public string UOM { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitWeight { get; set; }
        public decimal Gross_Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal CBM { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal TotalWeight { get; set; }
        public string OriginCode { get; set; }
        public string OriginName { get; set; }
        public string OriginAdd1 { get; set; }
        public string OriginAdd2 { get; set; }
        public string OriginAdd3 { get; set; }
        public string OriginAdd4 { get; set; }
        public string DestinationCode { get; set; }
        public string DestinationName { get; set; }
        public string DestinationAdd1 { get; set; }
        public string DestinationAdd2 { get; set; }
        public string DestinationAdd3 { get; set; }
        public string DestinationAdd4 { get; set; }
        public SortableList<ConsignmentNote_Job_Goods_Dimension> Job_Goods_Dimensions { get; set; }
         

    }
}
