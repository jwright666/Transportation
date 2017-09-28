using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FM.TR_TRKBookDLL.BLL
{
    public class TruckJobTripDetail
    {
        public int jobID { get; set; }
        public int jobTripSequence { get; set; }
        public int detailSequence { get; set; }
        public string uom { get; set; }
        public int quantity { get; set; }
        public decimal unitWeight { get; set; }
        public decimal totalWeight { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal volume { get; set; }
        public string marking { get; set; }
        public string cargoDescription { get; set; }
        public string remarks { get; set; }
        public int balQty { get; set; }
        public string ref_No { get; set; }

        public TruckJobTripDetail()
        {
            this.jobID = 0;
            this.jobTripSequence = 0;
            this.detailSequence = 1;
            this.uom = "";
            this.quantity = 0;
            this.unitWeight = 0;
            this.totalWeight = 0;
            this.length = 0;
            this.width = 0;
            this.height = 0;
            this.volume = 0;
            this.marking = "";
            this.cargoDescription = "";
            this.remarks = "";
            this.balQty = this.quantity;
            this.ref_No = string.Empty;
        }

        public TruckJobTripDetail(int jobID, int jobTripSequence, int detailSequence, string uom, int quantity, decimal unitWeight,
            decimal totalWeight,decimal length,decimal width,decimal height,decimal volume,string marking,
            string cargoDescription,string remarks)
        {
            this.jobID = jobID;
            this.jobTripSequence = jobTripSequence;
            this.detailSequence = detailSequence;
            this.uom = uom;
            this.quantity = quantity;
            this.unitWeight = unitWeight;
            this.totalWeight = totalWeight;
            this.length = length;
            this.width = width;
            this.height = height;
            this.volume = volume;
            this.marking = marking;
            this.cargoDescription = cargoDescription;
            this.remarks = remarks;
            this.ref_No = string.Empty;
            this.balQty = this.quantity;
        }


    }
}
