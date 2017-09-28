using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;

namespace FM.TR_SeaFreightDLL.BLL
{
    public abstract class SeaJob
    {

        private string branchCode;
        private string jobNumber;
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
        private string oblNumber;
        private string hblNumber;
        private string mVessel;
        private string mVesselName; // 2014-07-11 Zhou Kai adds
        private string mVoyage;
        private string pol;
        private string polName; // 2014-07-04 Zhou Kai adds
        private string pod;
        private string podName; // 2014-07-04 Zhou Kai adds
        private DateTime eta;
        private DateTime etd;
        private string customerInvRefNo; // 2014-07-24 Zhou Kai adds
        public const string TWENTYFOOT = "20'";
        public const string FORTYFOOT = "40'";
        public const string FORTYFIVEFOOT = "45'";
        public const string OTHERSIZE = "OTH";
        private string transportCode;
        private SortableList<SeaJobContainer> seaJobContainers;

        // 2014-09-01 Zhou Kai adds
        private string customerCode; // corresponds to bill_to_code
        private string customerName; // corresponds to bill_to_name
        private string cargoDescription;
        private string shipping_Line_Code;
        private string shipping_Line_Name;
        // 2014-06-26 Zhou Kai ends

        #region 20150803 - Gerry removed and replaced
        /*
        // 2014-06-26 Zhou Kai rename the members below:
        //private string deliverToCode;
        //private string deliverToName;
        //private string deliverToAdd1;
        //private string deliverToAdd2;
        //private string deliverToAdd3;
        //private string deliverToAdd4;

        // 2014-06-30 Zhou Kai changes private to protected
        // These properties are shared by SE and SI,
        // for SE, they're StuffingAt,
        // for SI, they're Container Yard
        protected string emptyContainerToCode;
        protected string emptyContainerToName;
        protected string emptyContainerToAdd1;
        protected string emptyContainerToAdd2;
        protected string emptyContainerToAdd3;
        protected string emptyContainerToAdd4;
        protected string emptyContainerToCityCode; // not in specs
        // 2014-06-26 Zhou Kai ends
        */
        #endregion

    
        //20150803 - Gerry modified properties
        //reason - to have common field names in both import and export for container yard, pickup and delivery
        //container yard
        public string StuffingCode { get; set; }
        public string StuffingName { get; set; }
        public string StuffingAdd1 { get; set; }
        public string StuffingAdd2 { get; set; }
        public string StuffingAdd3 { get; set; }
        public string StuffingAdd4 { get; set; }
        public string StuffingCityCode { get; set; }
        //pickup place
        public string PickUpPlaceCode { get; set; }
        public string PickUpPlaceName { get; set; }
        public string PickUpPlaceAdd1 { get; set; }
        public string PickUpPlaceAdd2 { get; set; }
        public string PickUpPlaceAdd3 { get; set; }
        public string PickUpPlaceAdd4 { get; set; }
        public string PickUpPlaceCityCode { get; set; }
        //delivery place
        public string DeliveryCode { get; set; }
        public string DeliveryName { get; set; }
        public string DeliveryAdd1 { get; set; }
        public string DeliveryAdd2 { get; set; }
        public string DeliveryAdd3 { get; set; }
        public string DeliveryAdd4 { get; set; }
        public string DeliveryCityCode { get; set; }
        public string BookingRefNo { get; set; }
        public string JobCreatorId { get; set; }
        public DateTime JobCreatedDateTime { get; set; }
        public string Shipment_Type { get; set; }


        public string CargoDescription
        {
            get { return cargoDescription; }
            set { cargoDescription = value; }
        }
        public string Shipping_Line_Code
        {
            get { return shipping_Line_Code; }
            set { shipping_Line_Code = value; }
        }
        public string Shipping_Line_Name
        {
            get { return shipping_Line_Name; }
            set { shipping_Line_Name = value; }
        }
        public string POLName
        {
            get { return polName; }
            set { polName = value; }
        }
        public string PODName
        {
            get { return podName; }
            set { podName = value; }
        }
    
        public string BranchCode
        {
            get { return branchCode; }
            set
            {
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrorBranchCodeEmpty);
                }
                else
                {
                    branchCode = value;
                }
            }
        }

        public string JobNumber
        {
            get { return jobNumber; }
            set 
            {
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrorJobNoEmpty);
                }
                else
                {
                    jobNumber = value;
                }
            }
        }

        public string ShipperCode
        {
            get { return shipperCode; }
            set 
            {
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrEmptyShipperCode);
                }
                else
                {
                    shipperCode = value;
                }
            }
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
            set 
            {
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrEmptyConsigneeCode);
                }
                else
                {
                    consigneeCode = value;
                }
            }
        }

        public string ConsigneeName
        {
            get { return consigneeName; }
            set 
            {
                    consigneeName = value;
            }
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

        public string OblNumber
        {
            get { return oblNumber; }
            set { oblNumber = value; }
        }


        public string HblNumber
        {
            get { return hblNumber; }
            set { hblNumber = value; }
        }

        public string MVessel
        {
            get { return mVessel; }
            set { mVessel = value; }
        }

        // 2014-07-11 Zhou Kai adds
        public string MVesselName
        {
            get { return mVesselName; }
            set { mVesselName = value; }
        }
        // 2014-07-11 Zhou Kai ends

        public string MVoyage
        {
            get { return mVoyage; }
            set { mVoyage = value; }
        }

        public string Pol
        {
            get { return pol; }
            set { pol = value; }
        }

        public string Pod
        {
            get { return pod; }
            set { pod = value; }
        }

        public DateTime Eta
        {
            get { return eta; }
            set { eta = value; }
        }

        public DateTime Etd
        {
            get { return etd; }
            set { etd = value; }
        }
        
        public string TransportCode
        {
            get { return transportCode; }
            set 
            {
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrEmptyTransportCode);
                }
                else
                {
                    transportCode = value;
                }
            }
        }

        public SortableList<SeaJobContainer> SeaJobContainers
        {
            get { return seaJobContainers; }
            set { seaJobContainers = value; }
        }

        public string CustomerRefNo
        {
            get { return this.customerInvRefNo; }
            set { this.customerInvRefNo = value; }
        }

        // 2014-09-01 Zhou Kai adds
        public string CustomerCode
        {
            get { return customerCode; }
            set { customerCode = value; }
        }
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }
        // 2014-09-01 Zhou Kai ends 

        public SeaJob()
        {
            this.branchCode = "";
            this.jobNumber = "";
            this.shipperCode = "";
            this.shipperName = "";
            this.shipperAdd1 = "";
            this.shipperAdd2 = "";
            this.shipperAdd3 = "";
            this.shipperAdd4 = "";
            this.consigneeCode = "";
            this.consigneeName = "";
            this.consigneeAdd1 = "";
            this.consigneeAdd2 = "";
            this.consigneeAdd3 = "";
            this.consigneeAdd4 = "";
            this.oblNumber = "";
            this.hblNumber = "";
            this.mVessel = "";
            this.mVesselName = String.Empty; // 2014-07-11 Zhou Kai adds
            this.mVoyage = "";
            this.pol = "";
            this.pod = "";
            this.eta = DateTime.Today;
            this.etd = DateTime.Today;
            // 2014-06-30 Zhou Kai adds
            this.cargoDescription = "";
            this.Shipping_Line_Code = String.Empty;
            this.shipping_Line_Name = String.Empty;
            this.polName = String.Empty;
            this.podName = String.Empty;
            // 2014-06-30 Zhou Kai ends
            this.transportCode = "";
            this.customerInvRefNo = String.Empty; // 2014-07-24 Zhou Kai adds
            this.seaJobContainers = new SortableList<SeaJobContainer>();

            // 2014-09-01 Zhou Kai starts 
            this.customerCode = String.Empty;
            this.customerName = String.Empty;
            // 2014-09-01 Zhou Kai ends 

            //20150803 - gerry added
            this.StuffingCode = string.Empty;
            this.StuffingName = string.Empty;
            this.StuffingAdd1 = string.Empty;
            this.StuffingAdd2 = string.Empty;
            this.StuffingAdd3 = string.Empty;
            this.StuffingAdd4 = string.Empty;
            this.StuffingCityCode = string.Empty;
            this.PickUpPlaceCode = string.Empty;
            this.PickUpPlaceName = string.Empty;
            this.PickUpPlaceAdd1 = string.Empty;
            this.PickUpPlaceAdd2 = string.Empty;
            this.PickUpPlaceAdd3 = string.Empty;
            this.PickUpPlaceAdd4 = string.Empty;
            this.PickUpPlaceCityCode = string.Empty;
            this.DeliveryCode = string.Empty;
            this.DeliveryName = string.Empty;
            this.DeliveryAdd1 = string.Empty;
            this.DeliveryAdd2 = string.Empty;
            this.DeliveryAdd3 = string.Empty;
            this.DeliveryAdd4 = string.Empty;
            this.DeliveryCityCode = string.Empty;
            this.BookingRefNo = string.Empty;
            this.JobCreatorId = string.Empty;
            this.JobCreatedDateTime = DateTime.Now;
        }

        // 2014-06-30 Zhou Kai renames parameters from deliverToXXX to emptyContainerToXXX
        public SeaJob
            (string branchCode,
            string jobNumber,
            string shipperCode,
            string shipperName,
            string shipperAdd1,
            string shipperAdd2,
            string shipperAdd3,
            string shipperAdd4,
            string consigneeCode,
            string consigneeName,
            string consigneeAdd1,
            string consigneeAdd2,
            string consigneeAdd3,
            string consigneeAdd4,
            string oblNumber,
            string hblNumber,
            string mVessel,
            string mVesselName, // 2014-07-11 Zhou Kai adds
            string mVoyage,
            string pol,
            string polName, // 2014-07-04 Zhou Kai adds
            string pod,
            string podName, // 2014-07-04 Zhou Kai adds
            DateTime eta,
            DateTime etd,
            string transportCode,
            string cargoDescription,
            string shippingLineCode,
            string shippingLineName,
            string customerInvRefNo, // 2014-07-24 Zhou Kai adds
            SortableList<SeaJobContainer> seaJobContainers
            /*2014-09-01 Zhou Kai adds*/, string customerCode, string customerName)
        {
            this.branchCode = branchCode;
            this.jobNumber = jobNumber;
            this.shipperCode = shipperCode;
            this.shipperName = shipperName;
            this.shipperAdd1 = shipperAdd1;
            this.shipperAdd2 = shipperAdd2;
            this.shipperAdd3 = shipperAdd3;
            this.shipperAdd4 = shipperAdd4;
            this.consigneeCode = consigneeCode;
            this.consigneeName = consigneeName;
            this.consigneeAdd1 = consigneeAdd1;
            this.consigneeAdd2 = consigneeAdd2;
            this.consigneeAdd3 = consigneeAdd3;
            this.consigneeAdd4 = consigneeAdd4;
            this.oblNumber = oblNumber;
            this.hblNumber = hblNumber;
            this.mVessel = mVessel;
            this.mVesselName = mVesselName; // 2014-07-11 Zhou Kai adds
            this.mVoyage = mVoyage;
            this.pol = pol;
            this.polName = polName;
            this.pod = pod;
            this.podName = podName;
            this.eta = eta;
            this.etd = etd;
            // 2014-06-30 Zhou Kai renames these parameters
            this.shipping_Line_Code = shippingLineCode;
            this.shipping_Line_Name = shippingLineName;
            this.cargoDescription = cargoDescription;
            // 2014-06-30 Zhou Kai ends
            this.transportCode = transportCode;
            this.customerInvRefNo = customerInvRefNo; // 2014-07-24 Zhou Kai adds
            this.seaJobContainers = seaJobContainers;

            // 2014-09-01 Zhou Kai adds
            this.customerCode = customerCode;
            this.customerName = customerName;
            // 2014-09-01 Zhou Kai ends

            //20150803 - gerry added
            this.PickUpPlaceCode = string.Empty;
            this.PickUpPlaceName = string.Empty;
            this.PickUpPlaceAdd1 = string.Empty;
            this.PickUpPlaceAdd2 = string.Empty;
            this.PickUpPlaceAdd3 = string.Empty;
            this.PickUpPlaceAdd4 = string.Empty;
            this.StuffingCode = string.Empty;
            this.StuffingName = string.Empty;
            this.StuffingAdd1 = string.Empty;
            this.StuffingAdd2 = string.Empty;
            this.StuffingAdd3 = string.Empty;
            this.StuffingAdd4 = string.Empty;
            this.DeliveryCode = string.Empty;
            this.DeliveryName = string.Empty;
            this.DeliveryAdd1 = string.Empty;
            this.DeliveryAdd2 = string.Empty;
            this.DeliveryAdd3 = string.Empty;
            this.DeliveryAdd4 = string.Empty;
            this.DeliveryCityCode = string.Empty;
            this.BookingRefNo = string.Empty;
        }

        /// <summary>
        /// Get the container quantity of each container sizes, currently there are
        /// 4 different sizes: 20', 40', 45', OTH(other)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetContainerQuantityOfEachSize()
        {
            Dictionary<string, string> dicContainerQuantities = new Dictionary<string, string>();
            int n20 = 0, n40 = 0, n45 = 0, nOTH = 0;

            foreach (SeaJobContainer sjc in this.SeaJobContainers)
            {
                #region Gerry replaced 20150519
                /*
                if (sjc.ContainerSize.Equals(TWENTYFOOT)) { n20++; }
                if (sjc.ContainerSize.Equals(FORTYFOOT)) { n40++; }
                if (sjc.ContainerSize.Equals(FORTYFIVEFOOT)) { n45++; }
                if (sjc.ContainerSize.Equals(OTHERSIZE)) { nOTH++; }
                */
                #endregion
                //reason : previously 40HC consider as OTH size, so 40HC will not be captured
                if (sjc.ContainerCode.Contains(TWENTYFOOT.Substring(0,2))) { n20++; }
                if (sjc.ContainerCode.Contains(FORTYFOOT.Substring(0, 2))) { n40++; }
                if (sjc.ContainerCode.Contains(FORTYFIVEFOOT.Substring(0, 2))) { n45++; }
                if (sjc.ContainerCode.Equals(OTHERSIZE)) { nOTH++; }
            }

            dicContainerQuantities.Add(TWENTYFOOT, n20.ToString());
            dicContainerQuantities.Add(FORTYFOOT, n40.ToString());
            dicContainerQuantities.Add(FORTYFIVEFOOT, n45.ToString());
            dicContainerQuantities.Add(OTHERSIZE, nOTH.ToString());

            return dicContainerQuantities;
        }
    }

}
