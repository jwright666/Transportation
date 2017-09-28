using FM.TR_FMSystemDLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TR_ConsignmentNoteDLL.BLL
{
    public class ConsignmentNote_Job_Master
    {
        public string Consignment_Note_No { get; set; }
        public string Customer_Code { get; set; }
        public string ShipperCode { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAdd1 { get; set; }
        public string ShipperAdd2 { get; set; }
        public string ShipperAdd3 { get; set; }
        public string ShipperAdd4 { get; set; }
        public string ConsigneeCode { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAdd1 { get; set; }
        public string ConsigneeAdd2 { get; set; }
        public string ConsigneeAdd3 { get; set; }
        public string ConsigneeAdd4 { get; set; }
        public SortableList<ConsignmentNote_Job_Goods_Details> Job_Goods_Details { get; set; }

        public ConsignmentNote_Job_Master()
        {
            this.Consignment_Note_No = string.Empty;
            this.Customer_Code = string.Empty;
            this.ShipperCode = string.Empty;
            this.ShipperName = string.Empty;
            this.ShipperAdd1 = string.Empty;
            this.ShipperAdd2 = string.Empty;
            this.ShipperAdd3 = string.Empty;
            this.ShipperAdd4 = string.Empty;
            this.ConsigneeCode = string.Empty;
            this.ConsigneeName = string.Empty;
            this.ConsigneeAdd1 = string.Empty;
            this.ConsigneeAdd2 = string.Empty;
            this.ConsigneeAdd3 = string.Empty;
            this.ConsigneeAdd4 = string.Empty;
            this.Job_Goods_Details = new SortableList<ConsignmentNote_Job_Goods_Details>();
        }
    }
}
