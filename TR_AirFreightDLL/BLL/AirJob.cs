using FM.TR_FMSystemDLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TR_AirFreightDLL.BLL
{
    //base class for Air jobs
    public class AirJob
    {
        #region Class private properties
        private int jobID;
        private string jobNo;
        private string custCode;
        private string bookingNo;
        private string flightNo;
        private string hawb;
        private string mawb;
        private string awb;
        private string origin;
        private string destination;
        private DateTime flightDate;
        private string shipperCode;
        private string shipperName;
        private string shipperAdd1;
        private string shipperAdd2;
        private string shipperAdd3;
        private string shipperAdd4;
        private string consigneeCode;
        private string consigneeName;
        private string consigneeAdd1;
        private string consigneeAdd2;
        private string consigneeAdd3;
        private string consigneeAdd4;
        private AirTruckInfo airTruckInfo;
        #endregion    
        public SortableList<AirJobRate> airJobRates { get; set; }

        #region Constructor
        public AirJob()
        {
            this.jobID = 0;
            this.jobNo = string.Empty;
            this.custCode = string.Empty;
            this.bookingNo = string.Empty;
            this.flightNo = string.Empty;
            this.hawb = string.Empty;
            this.mawb = string.Empty;
            this.awb = string.Empty;
            this.origin = string.Empty;
            this.destination = string.Empty;
            this.flightDate = DateTime.Today;
            this.shipperCode = string.Empty;
            this.shipperName = string.Empty;
            this.shipperAdd1 = string.Empty;
            this.shipperAdd2 = string.Empty;
            this.shipperAdd3 = string.Empty;
            this.shipperAdd4 = string.Empty;
            this.consigneeCode = string.Empty;
            this.consigneeName = string.Empty;
            this.consigneeAdd1 = string.Empty;
            this.consigneeAdd2 = string.Empty;
            this.consigneeAdd3 = string.Empty;
            this.consigneeAdd4 = string.Empty;
            this.airTruckInfo = new AirTruckInfo();
            this.airJobRates = new SortableList<AirJobRate>();
        }
        #endregion  

        #region Getter/Setter Methods
        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }
        public string JobNo
        {
            get { return jobNo; }
            set { jobNo = value; }
        }
        public string CustCode
        {
            get { return custCode; }
            set { custCode = value; }
        }
        public string BookingNo
        {
            get { return bookingNo; }
            set { bookingNo = value; }
        }
        public string FlightNo
        {
            get { return flightNo; }
            set { flightNo = value; }
        }
        public string HAWB
        {
            get { return hawb; }
            set { hawb = value; }
        }
        public string MAWB
        {
            get { return mawb; }
            set { mawb = value; }
        }
        public string AWB
        {
            get { return awb; }
            set { awb = value; }
        }
        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }
        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }
        public DateTime FlightDate
        {
            get { return flightDate; }
            set { flightDate = value; }
        }
        public string ShipperCode
        {
            get { return shipperCode; }
            set { shipperCode = value; }
        }
        public string ShipperName
        {
            get { return shipperName; }
            set { shipperName = value; }
        }  
        public string ShipperAdd1
        {
            get { return shipperAdd1; }
            set { shipperAdd1 = value; }
        } 
        public string ShipperAdd2
        {
            get { return shipperAdd2; }
            set { shipperAdd2 = value; }
        } 
        public string ShipperAdd3
        {
            get { return shipperAdd3; }
            set { shipperAdd3 = value; }
        } 
        public string ShipperAdd4
        {
            get { return shipperAdd4; }
            set { shipperAdd4 = value; }
        } 
        public string ConsigneeCode
        {
            get { return consigneeCode; }
            set { consigneeCode = value; }
        }
        public string ConsigneeName
        {
            get { return consigneeName; }
            set{ consigneeName = value; }
        } 
        public string ConsigneeAdd1
        {
            get { return consigneeAdd1; }
            set { consigneeAdd1 = value; }
        }     
        public string ConsigneeAdd2
        {
            get { return consigneeAdd2; }
            set { consigneeAdd2 = value; }
        }
        public string ConsigneeAdd3
        {
            get { return consigneeAdd3; }
            set { consigneeAdd3 = value; }
        }
        public string ConsigneeAdd4
        {
            get { return consigneeAdd4; }
            set { consigneeAdd4 = value; }
        }
        public AirTruckInfo AirTruckInfo
        {
            get { return airTruckInfo; }
            set { airTruckInfo = value; }
        }
        #endregion




    }
}
