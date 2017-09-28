using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TR_WMSDLL.BLL
{
    public class GINDetail
    {
        public string gin_no { get; set; }
        public int itemTrxKey { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string item_uom { get; set; }
        public int qty { get; set; }
        public string dimension_uom { get; set; }
        public decimal unitWeight { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal totalVolume { get; set; }
        public decimal totalWeight { get; set; }
        public string originCode { get; set; }
        public string originName { get; set; }
        public string originAdd1 { get; set; }
        public string originAdd2 { get; set; }
        public string originAdd3 { get; set; }
        public string originAdd4 { get; set; }
        public string destinationCode { get; set; }
        public string destinationName { get; set; }
        public string destinationAdd1 { get; set; }
        public string destinationAdd2 { get; set; }
        public string destinationAdd3 { get; set; }
        public string destinationAdd4 { get; set; }
    }
}
