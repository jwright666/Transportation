
using FM.TR_FMSystemDLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TR_AirFreightDLL.BLL
{
    public class AirTruckInfo
    {
        #region Private Properties
        private int jobID;
        private string pickupCode;
        private string pickupName;
        private string pickupAdd1;
        private string pickupAdd2;
        private string pickupAdd3;
        private string pickupAdd4;
        private DateTime pickupDate;
        private string deliveryCode;
        private string deliveryName;
        private string deliveryAdd1;
        private string deliveryAdd2;
        private string deliveryAdd3;
        private string deliveryAdd4;
        private DateTime deliveryDate;
        private string remarks;
        public SortableList<AirJobDtlDimension> airJobDtlDimensions;

        #endregion       
        #region Constuctor
        public AirTruckInfo()
        {
            this.jobID = 0;
            this.pickupCode = string.Empty;
            this.pickupName = string.Empty;
            this.pickupAdd1 = string.Empty;
            this.pickupAdd2 = string.Empty;
            this.pickupAdd3 = string.Empty;
            this.pickupAdd4 = string.Empty;
            this.pickupDate = DateTime.Now;
            this.deliveryDate = DateTime.Now;
            this.deliveryCode = string.Empty;
            this.deliveryName = string.Empty;
            this.deliveryAdd1 = string.Empty;
            this.deliveryAdd2 = string.Empty;
            this.deliveryAdd3 = string.Empty;
            this.deliveryAdd4 = string.Empty;
            this.remarks = string.Empty;
            this.airJobDtlDimensions = new SortableList<AirJobDtlDimension>();
        }
        #endregion              
        #region Getter/Setter Methods
        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }
        public string PickupCode
        {
            get { return pickupCode; }
            set { pickupCode = value; }
        } 
        public string PickupName
        {
            get { return pickupName; }
            set { pickupName = value; }
        }
        public string PickupAdd1
        {
            get { return pickupAdd1; }
            set { pickupAdd1 = value; }
        }
        public string PickupAdd2
        {
            get { return pickupAdd2; }
            set { pickupAdd2 = value; }
        }
        public string PickupAdd3
        {
            get { return pickupAdd3; }
            set { pickupAdd3 = value; }
        }
        public string PickupAdd4
        {
            get { return pickupAdd4; }
            set { pickupAdd4 = value; }
        }
        public DateTime PickupDate
        {
            get { return pickupDate; }
            set { pickupDate = value; }
        }
        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set { deliveryDate = value; }
        }

        public string DeliveryCode
        {
            get { return deliveryCode; }
            set { deliveryCode = value; }
        }
        public string DeliveryName
        {
            get { return deliveryName; }
            set { deliveryName = value; }
        }
        public string DeliveryAdd1
        {
            get { return deliveryAdd1; }
            set { deliveryAdd1 = value; }
        }
        public string DeliveryAdd2
        {
            get { return deliveryAdd2; }
            set { deliveryAdd2 = value; }
        }
        public string DeliveryAdd3
        {
            get { return deliveryAdd3; }
            set { deliveryAdd3 = value; }
        }
        public string DeliveryAdd4
        {
            get { return deliveryAdd4; }
            set { deliveryAdd4 = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        #endregion




    }
}
