using System;
using System.Collections.Generic;
using System.Text;

namespace FM.TR_SeaFreightDLL.BLL
{
    public class SeaJobContainer
    {

        private string containerCode;
        private string containerNumber;
        private string sealNumber;
        private decimal grossCBM;
        private decimal grossWeight;
        // 2014-06-27 Zhou Kai adds
        private string cargoDescription;
        private string containerSize;

        public int JobID { get; set; }
        public int SeqNo { get; set; }
        public JobDimension ContainerDimension { get; set; } //20170807

        public string CargoDescription 
        { 
            get { return cargoDescription; } 
            set { cargoDescription = value; } 
        }
        public string ContainerSize
        {
            get { return containerSize; }
            set { containerSize = value; }
        }
        // 2014-06-27 Zhou Kai ends

        public string ContainerCode
        {
            get { return containerCode; }
            set { containerCode = value; }
        }

        public string ContainerNumber
        {
            get { return containerNumber; }
            set { containerNumber = value; }
        }

        public string SealNumber
        {
            get { return sealNumber; }
            set { sealNumber = value; }
        }

        public decimal GrossCBM
        {
            get { return grossCBM; }
            set { grossCBM = value; }
        }

        public decimal GrossWeight
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }
        
        public SeaJobContainer()
        {
            this.containerCode="";
            this.containerNumber = "";
            this.sealNumber = "";
            this.grossCBM=0;
            this.grossWeight=0;
            // 2014-07-01 Zhou Kai adds
            this.cargoDescription = String.Empty;
            this.containerSize = String.Empty;
            // 2014-07-01 Zhou Kai ends
        }

        // 2014-06-30 Zhou Kai modifies this constructor
        public SeaJobContainer(string containerCode,string containerNumber,string sealNumber,
            decimal grossCBM, decimal grossWeight, string cargoDescription, string containerSize)
        {

            this.containerCode = containerCode;
            this.containerNumber = containerNumber;
            this.sealNumber = sealNumber;
            this.grossCBM = grossCBM;
            this.grossWeight = grossWeight;
            // 2014-06-30 Zhou Kai adds
            this.cargoDescription = cargoDescription;
            this.containerSize = containerSize;
            // 2014-06-30 Zhou Kai ends
        }

        
    }
}
