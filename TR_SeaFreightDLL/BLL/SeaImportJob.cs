using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_SeaFreightDLL.BLL
{
    public class SeaImportJob:SeaJob
    {
        public SeaImportJob() : base() { }

        // 2014-06-30 Zhou Kai modifies this constructor:
        public SeaImportJob(string branchCode, string jobNumber, string shipperCode, string shipperName,
            string shipperAdd1, string shipperAdd2, string shipperAdd3, string shipperAdd4,
            string consigneeCode, string consigneeName, string consigneeAdd1, string consigneeAdd2,
            string consigneeAdd3, string consigneeAdd4, string oblNumber, string hblNumber, string mVessel,
            string mVesselName, // 2014-07-11 Zhou Kai adds
            string mVoyage, string pol, string polName, string pod, string podName, DateTime eta, DateTime etd, 
            // 2014-06-30 Zhou Kai adds:
            string transportCode, 
            string cargoDescription, 
            string shippingLineCode,
            string shippingLineName,
            string customerInvRefNo, // 2014-07-24 Zhou Kai adds
            SortableList<SeaJobContainer> seaJobContainers
            /*2014-09-01 Zhou Kai adds*/, string customerCode, string customerName
            )
            : base(branchCode, jobNumber, shipperCode, shipperName,
            shipperAdd1, shipperAdd2, shipperAdd3, shipperAdd4,
            consigneeCode, consigneeName, consigneeAdd1, consigneeAdd2,
            consigneeAdd3, consigneeAdd4, oblNumber, hblNumber, mVessel,
            mVesselName, // 2014-07-11 Zhou Kai adds
            mVoyage, pol, polName, pod, podName, eta, etd,
            transportCode,  cargoDescription,
            shippingLineCode, shippingLineName,customerInvRefNo, // 2014-07-24 Zhou Kai adds
            seaJobContainers
                /*2014-09-01 Zhou Kai adds*/ , customerCode, customerName)
        {
        }

        /// <summary>
        /// 2014-07-23 Zhou Kai adds, the function is used when importing sea freight data to haulier job,
        /// to check if there're compulsory fields left blank; if so, add the field names to the string list
        /// </summary>
        /// <param name="noOfLegs">When checking the compulsory fileds for an
        /// sea-import job, it's necessary to know the noOfLegs this job is going to be</param>
        /// <returns>The string list of field names which are compulsory but left blank</returns>
        public String ValidateCompulsoryFieldsComplete(int noOfLegs)
        {
            string errMessage = String.Empty;
            List<String> errList = new List<string>();

            if (this.MVessel.Equals(String.Empty))
            {
                errList.Add("Mother Vessel");
            }

            if(MVoyage.Equals(String.Empty))
            {
                errList.Add("Mother Voyage");
            }

            // 2014-08-16 Zhou Kai comments out this checking,
            // if it's 1-leg trip, the pkup Place is then not compulsory
            if (PickUpPlaceCode.Equals(String.Empty) &&
                noOfLegs == 2)
            {
                errList.Add("PickUp Place");
            }

            // as the sea-import job class has currently no port property, so
            // the validation of the port property is left at UI level
            
             if(StuffingCode.Equals(String.Empty))
            {
                errList.Add("Empty Container To");
            }

             if (errList.Count > 0)
             {
                 foreach (string t in errList)
                 {
                     errMessage += " " + t + " ";
                 }
             }
             else { return String.Empty; }
            
            return errMessage;
        }

    }
}
