using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;


namespace FM.TR_FMSystemDLL.BLL
{
    public class RateType
    {
        private string measurementCode;
        private string measurementDesc;

        public RateType()
        { 
           this.measurementCode=string.Empty; 
           this.measurementDesc=string.Empty; 
        }

        public RateType(string MeasurementCode, string MeasurementDesc)
        {
            measurementCode = MeasurementCode;
            measurementDesc = MeasurementDesc; 
        }

        public string MeasurementCode
        {
            get { return measurementCode; }
        }

        public string MeasurementDesc
        {
            get { return measurementDesc; }
        }

        public static SortableList<RateType> GetAllRateType()
        {
            try
            {
                return RateTypeDAL.GetAllRateType();
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
    }
}
