using FM.TR_FMSystemDLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TR_WMSDLL.DAL;

namespace TR_WMSDLL.BLL
{
    public class GINHeader
    {
        public string gin_no { get; set; }
        public string custCode { get; set; }
        public string branchCode { get; set; }
        public string delivery_type { get; set; }
        public string shipperCode { get; set; }
        public string shipperName { get; set; }
        public string shipperAdd1 { get; set; }
        public string shipperAdd2 { get; set; }
        public string shipperAdd3 { get; set; }
        public string shipperAdd4 { get; set; }
        public string consigneeCode { get; set; }
        public string consigneeName { get; set; }
        public string consigneeAdd1 { get; set; }
        public string consigneeAdd2 { get; set; }
        public string consigneeAdd3 { get; set; }
        public string consigneeAdd4 { get; set; }
        public string hawb { get; set; }
        public string mawb { get; set; }
        public string awb { get; set; }
        public string oblNumber { get; set; }
        public string hblNumber { get; set; }
        public string shippingLine { get; set; }
        public string mVesselName { get; set; }
        public string mVoyage { get; set; }
        public string pol { get; set; }
        public string polName { get; set; }
        public string pod { get; set; }
        public string podName { get; set; }
        public string warehouseNo { get; set; }
        public string yourRefNo { get; set; }
        public DateTime issue_date { get; set; }
        public string wh_staff_id { get; set; }
        public SortableList<GINDetail> ginDetails { get; set; }

    }
}
