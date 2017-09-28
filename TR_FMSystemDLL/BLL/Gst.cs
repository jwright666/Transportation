using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class Gst
    {
        public const string EXM = "EXM";
        public const string OUT = "OUT";
        public const string STD = "STD";
        public const string ZER = "ZER";

        private string gstType;
        private int gstRate;
        private DateTime effectiveDate;

        public string GstType
        {
            get { return gstType; }
            set { gstType = value; }
        }

        public int GstRate
        {
            get { return gstRate; }
            set { gstRate = value; }
        }

        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set { effectiveDate = value; }
        }

        public Gst()
        {
            this.gstRate = 0;
            this.gstType = "";
            this.effectiveDate = DateTime.Today;
        }

        public Gst(string gstType, int gstRate, DateTime effectiveDate)
        {
            this.GstRate = gstRate;
            this.GstType = gstType;
            this.EffectiveDate = effectiveDate;
        }

        public static SortableList<Gst> GetAllGst(DateTime requiredDate)
        {
            return GstDAL.GetAllGst(requiredDate);
        }

        public static Gst GetGstRate(string GstType, DateTime invoiceDate)
        {
            return GstDAL.GetGstRate( GstType,  invoiceDate);
        }


    }
}
