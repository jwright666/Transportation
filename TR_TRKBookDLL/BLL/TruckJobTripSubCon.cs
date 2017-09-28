using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 2013-09-30 Zhou Kai adds
using System.Data;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
// 2013-10-01 Zhou Kai adds
using FM.TR_MarketDLL.BLL;
using FM.TR_TRKBookDLL.DAL;
using TR_LanguageResource.Resources;

namespace FM.TR_TRKBookDLL.BLL
{
    /*
     * 2013-09-20 Zhou Kai adds class TruckJobTripSubCon.
     * TruckJobTripSubCon inherites from TruckJobTrip
     */
    public class TruckJobTripSubCon : TruckJobTrip
    {
        #region " Private members"

        private string waitTimeFrom; // when the truck arrives
        private string waitTimeTo; // when the customer cargo arrives
        private DateTime dateTimeFromAct; // the actual start time of the job trip
        private DateTime dateTimeToAct; // the actual end time of the job trip
        private string driver; // the driver code the sub con used
        private string vehicle; // the vechicle number the sub con used
        private int noPackage; // sum of packages of job trips
        private int noPallets; // sum of pallets of job trips
        private string reqCustTruckCapacity; // the truck capacity customer required
        private string operatorTruckCapacity; // the truck capacity that operator actually provided
        private string remarks1;
        private string remarks2;
        private string remarks3;
        private string remarks4;
        private string remarks5;
        
        #endregion

        #region " Constructors"
        // constructs an empty TruckJobTripSubCon object
        public TruckJobTripSubCon()
            : base()
        {
            waitTimeFrom = String.Empty;
            waitTimeTo = String.Empty;
            dateTimeFromAct = DateTime.Now;
            dateTimeToAct = DateTime.Now;
            driver = String.Empty;
            vehicle = String.Empty;
            noPackage = 0;
            noPallets = 0;
            reqCustTruckCapacity = String.Empty;
            operatorTruckCapacity = String.Empty;
            remarks1 = String.Empty;
            remarks2 = String.Empty;
            remarks3 = String.Empty;
            remarks4 = String.Empty;
            remarks5 = String.Empty;

            // 2014-03-21 Zhou Kai adds
            SubConCode = String.Empty;
            UpdateVersion = new byte[8];
        }

        // construcs a TruckJobTripSubCon class with all properties initialized
        public TruckJobTripSubCon(int jobId, int jobTripSeq, string waitTimeFrom, string waitTimeTo,
                                  DateTime dateTimeFromAct, DateTime dateTimeToAct, string driver, string vehicle,
                                  decimal actVolume, decimal actWeight,
                                  int noPackage, int noPallets, string reqCustTruckCapacity, string operatorTruckCapacity,
                                  string remarks1, string remarks2, string remarks3, string remarks4, string remarks5, 
                                 string subConCode, byte[] updateVersion)
        {
            this.JobID = jobId;
            this.Sequence = jobTripSeq;
            this.waitTimeFrom = waitTimeFrom;
            this.waitTimeTo = waitTimeTo;
            this.dateTimeFromAct = dateTimeFromAct;
            this.dateTimeToAct = dateTimeToAct;
            this.driver = driver;
            this.vehicle = vehicle;
            this.noPackage = noPackage;
            this.noPallets = noPallets;
            this.reqCustTruckCapacity = reqCustTruckCapacity;
            this.operatorTruckCapacity = operatorTruckCapacity;
            this.remarks1 = remarks1;
            this.remarks2 = remarks2;
            this.remarks3 = remarks3;
            this.remarks4 = remarks4;
            this.remarks5 = remarks5;
            this.SubConCode = subConCode;
            // 2014-03-24 Zhou Kai modifies, removes the truckJobTrip reference from "this" class
            this.actualVol = Math.Round(actVolume, 3);
            this.actualWeight = Math.Round(actWeight, 3);
            UpdateVersion = updateVersion; // need to synchronize from database
            // 2014-03-24 Zhou Kai ends
        }
        #endregion

        #region " Public Properties"
        public string WaitTimeFrom
        {
            get {return waitTimeFrom;}
            set {waitTimeFrom = value;}
        }
        public string WaitTimeTo
        {
            get {return waitTimeTo;}
            set {waitTimeTo = value;}
        }
        public DateTime DateTimeFromAct
        {
            get {return dateTimeFromAct;}
            set {dateTimeFromAct = value;}
        }
        public DateTime DateTimeToAct
        {
            get {return dateTimeToAct;}
            set {dateTimeToAct = value;}
        }
        public string Driver
        {
            get {return driver;}
            set {driver = value;}
        }
        public string Vehicle
        {
            get {return vehicle;}
            set {vehicle = value;}
        }
        public int NoPackage
        {
            get {return noPackage;}
            set {noPackage = value;}
        }
        public int NoPallets
        {
            get {return noPallets;}
            set {noPallets = value;}
        }
        public string ReqCustTruckCapacity
        {
            get {return reqCustTruckCapacity;}
            set {reqCustTruckCapacity = value;}
        }
        public string OperatorTruckCapacity
        {
            get {return operatorTruckCapacity;}
            set {operatorTruckCapacity = value;}
        }
        public string Remarks1
        {
            get {return remarks1;}
            set {remarks1 = value;}
        }
        public string Remarks2
        {
            get {return remarks2;}
            set {remarks2 = value;}
        }
        public string Remarks3
        {
            get {return remarks3;}
            set {remarks3 = value;}
        }
        public string Remarks4
        {
            get {return remarks4;}
            set {remarks4 = value;}
        }
        public string Remarks5
        {
            get {return remarks5;}
            set {remarks4 = value;}
        }

        // 2014-03-20 Zhou Kai adds properties for sub-contractor
        public string SubConCode
        {
            get { return subCon.Code; }
            private set{subCon.Code = value;}
        }
        public string SubConFullName
        {
            get { return subCon.Name; }
            private set { subCon.Name = value; }
        }
        // 2014-03-20 Zhou Kai ends
        #endregion

        #region " Functions"
        public int GetNoOfPackageWhenUOMIsNotPkg(int jobId, int jobTripSeq)
        {
            return TruckJobDAL.GetNoOfPackageWhenUOMIsNotPkg(jobId, jobTripSeq);
        }

        public int GetNoOfPackageWhenUOMIsPkg(int jobId, int jobTripSeq)
        {   // for JVL, When UOM is PKG, the UOM is default to PLT
            return TruckJobDAL.GetNoOfPackageWhenUOMIsPkg(jobId, jobTripSeq);
        }

        public string GetReqCustTruckCapacity(int jobId, int jobTripSeq)
        {
            return TruckJobDAL.GetReqCusomerTruckCapacity(jobId, jobTripSeq);
        }

        // 2013-10-28 Zhou Kai adds
        public static SortableList<TruckJobTripSubCon> GetAllTruckJobTripSubCon(int jobId)
        {
            return TruckJobDAL.GetAllTruckJobTripSubCon(jobId);
        }
        public SortableList<TruckJobTripSubCon> GetAllTruckJobTripSubCon()
        {
            return TruckJobDAL.GetAllTruckJobTripSubCon(this.JobID);
        }

        // 2013-10-28 Zhou Kai ends
        public static TruckJobTripSubCon GetTruckJobTripSubCon(int jobId, int jobTripSeq)
        {
            return TruckJobDAL.GetTruckJobTripSubCon(jobId, jobTripSeq); 
        }

        public bool EditTruckJobTripSubConData(int jobId, int jobTripSeqNo,
                                                 string driver, string vehicle,
                                                 string pickUpTime, string deliverTime,
                                                 string waitTimeFrom, string waitTimeTo,
                                                 string operatorTruckCapacity,
                                                 string rm1, string rm2, string rm3,
                                                 string rm4, string rm5)
        {
            return TruckJobDAL.EditTruckJobTripSubConData(jobId, jobTripSeqNo, driver, vehicle,
                                                            pickUpTime, deliverTime, waitTimeFrom,
                                                            waitTimeTo, operatorTruckCapacity, rm1,
                                                            rm2, rm3, rm4, rm5);
        }

        public bool DeleteTruckJobTripSubCon(SqlConnection cn, SqlTransaction tran)
        {
            return TruckJobDAL.DeleteTruckJobTripSubCon(JobID, Sequence, cn, tran);
        }

        private string GetDateTimeToString(DateTime datePart, string timePart)
        {
            return DateUtility.ConvertDateForSQLPurpose(datePart) + " " + timePart.Insert(2, ":");
        }
        private DateTime GetDateTime(DateTime datePart, string timePart)
        {
            return Convert.ToDateTime(DateUtility.ConvertDateForSQLPurpose(datePart)
                                      + " " + timePart.Insert(2, ":"));
        }

        private byte[] GetUpdateVersionFromDb(int jobId, int jobTripSeqNo)
        {
            return TruckJobDAL.GetSubConTripUpdateVersionFromDb(jobId, jobTripSeqNo);
        }

        #endregion
    }

    //2013-10-04 Zhou Kai ends
}
