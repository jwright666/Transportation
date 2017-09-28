using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_HLBookDLL.BLL;
using FM.TR_TRKBookDLL.DAL;
using FM.TR_TRKBookDLL.Facade;
using System.Collections;
using TR_LanguageResource.Resources;
using FM.TR_MarketDLL.BLL;
using System.Data;
using FM.TR_MaintenanceDLL.BLL;
using TR_WMSDLL.BLL;

namespace FM.TR_TRKBookDLL.BLL
{
    public class TruckJob : VehicleJob
    {
        //public const string ONE_TO_MANY = "OTM"; //create in common enum as Trip_Type
        //public const string ONE_TO_ONE = "OTO";
        //public const string MANY_TO_ONE = "MTO";

        public string is1JobTrip1Invoice { get; set; }
        public string shipperCode { get; set; }
        public string shipperName { get; set; }
        public string shipperAdd1 { get; set; }
        public string shipperAdd2 { get; set; }
        public string shipperAdd3 { get; set; }
        public string shipperAdd4 { get; set; }
        public string consigneeCode { get; set; }
        public string consigneeName { get; set; }
        public string consigneeAdd1 { get; set; }
        public string consigneeAdd2 { get; set; }
        public string consigneeAdd3 { get; set; }
        public string consigneeAdd4 { get; set; }
        public string psaAccount { get; set; }
        public string subcontractorCode { get; set; }
        public string subcontractorName { get; set; }
        public string subcontractorAdd1 { get; set; }
        public string subcontractorAdd2 { get; set; }
        public string subcontractorAdd3 { get; set; }
        public string subcontractorAdd4 { get; set; }
        public string warehouseNo { get; set; }
        public string yourRefNo { get; set; }
        public string ucrNo { get; set; }
        public string permitNo1 { get; set; }
        public DateTime datePermitNo1 { get; set; }
        public string permitNo2 { get; set; }
        public DateTime datePermitNo2 { get; set; }
        public string remarks { get; set; }
        public string remarks2 { get; set; }
        public string remarks3 { get; set; }
        public string remarks4 { get; set; }
        public string remarks5 { get; set; }
        public SortableList<TruckJobTrip> truckJobTrips { get; set; }

        public string carrierAgent { get; set; }
        public string oblNo { get; set; }
        public string hblNo { get; set; }
        public string vesselNo { get; set; }
        public string voyageNo { get; set; }
        public string pol { get; set; }
        public string pod { get; set; }
        public DateTime eta { get; set; }
        public DateTime etd { get; set; }

        public string origin { get; set; }
        public string destination { get; set; }
        public string via { get; set; }
        public string flightNo { get; set; }
        public string HAWB { get; set; }
        public string MAWB { get; set; }
        public string AWBNo { get; set; }
        public DateTime flightDate { get; set; }


        private ChargeType truckChargeType;
        //20130903 - Gerry added
        public string commercialInvoice { get; set; }
        public string customsDeclaration { get; set; }


        // 2013-12-23 Zhou Kai adds const string for writting logs
        // 2014-04-01 Zhou Kai changes the sub-con code to quotation no to be logged
        // private const string LOG_DETAIL_SUBCON_CODE = "Subcontractor Code";
        private const string LOG_DETAIL_QUOTATION_NO = "Quotation No";
        // 2014-04-01 Zhou Kai ends
        // 2014-0-24 Zhou Kai adds
        private const string LOG_DETAIL_SYSTEM_TIME = "System time";
        // 2014-01-17 Zhou Kai modifies what to be logged
        // private const string FROM_TIME = "From Time";
        private const string LOG_DETAIL_START_STOP = "Start Stop";
        // private const string TO_TIME = "To Time";
        private const string LOG_DETAIL_END_STOP = "End Stop";
        private const string LOG_DETAIL_FROM_DATE = "From Date";
        private const string LOG_DETAIL_TO_DATE = "To Date";
        private const string LOG_DETAIL_BILLING_QTY = "Billing Quantity";
        private const string LOG_DETAIL_BILLING_UOM = "Billing UOM";
        private const string LOG_DETAIL_WEIGHT_VOLUME_FROM_DETAILS = "Weight/Volume from details?";
        // private const string WEIGHT_FROM_DETAILS = "Weight";
        // private const string VOLUME_FROM_DETAILS = "Volume";
        private const string LOG_DETAIL_CHARGE_CODE = "Charge Code";
        private const string LOG_DETAIL_QUANTITY = "Quantity";
        private const string LOG_DETAIL_UNITPRICE = "Unit Price";
        private const string LOG_DETAIL_DELETE_DATE = "Deleted DateTime";

        public Trip_Type TripType { get; set; }

        public TruckJob()
            : base()
        {
            this.truckChargeType = ChargeType.NotStopDependent;

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
            this.psaAccount = "";
            this.subcontractorCode = "";
            this.subcontractorName = "";
            this.subcontractorAdd1 = "";
            this.subcontractorAdd2 = "";
            this.subcontractorAdd3 = "";
            this.subcontractorAdd4 = "";
            this.warehouseNo = "";
            this.yourRefNo = "";
            this.ucrNo = "";
            this.permitNo1 = "";
            this.datePermitNo1 = DateUtility.GetSQLDateTimeMinimumValue(); //DateTime.Today;   //gerry replaced default value
            this.permitNo2 = "";
            this.datePermitNo2 = DateUtility.GetSQLDateTimeMinimumValue(); //DateTime.Today;   //gerry replaced default value
            this.remarks = "";
            this.remarks2 = "";
            this.remarks3 = "";
            this.remarks4 = "";
            this.remarks5 = "";
            this.truckJobTrips = new SortableList<TruckJobTrip>();
            this.carrierAgent = "";
            this.oblNo = "";
            this.hblNo = "";
            this.vesselNo = "";
            this.voyageNo = "";
            this.pol = "";
            this.pod = "";
            this.eta = DateUtility.GetSQLDateTimeMinimumValue(); //DateTime.Today;   //gerry replaced default value
            this.etd = DateUtility.GetSQLDateTimeMinimumValue(); //DateTime.Today;   //gerry replaced default value

            this.origin = "";
            this.destination = "";
            this.via = "";
            this.flightNo = "";
            this.HAWB = "";
            this.MAWB = "";
            this.AWBNo = "";
            this.flightDate = DateUtility.GetSQLDateTimeMinimumValue();//gerry replaced default value 
            this.commercialInvoice = string.Empty;
            this.customsDeclaration = string.Empty;

            // 2014-04-09 Zhou Kai adds
            //this.tripType = FreightType.LO;
            // 2014-04-09 Zhou Kai ends
            this.TripType = Trip_Type.OTO;
        }

        public TruckJob(int jobID, string jobNo, string custNo, string sourceRef, JobStatus jobstatus,
            string jobType, string bookingNo, DateTime bookingDate, string quotationNo, JobUrgencyType jobUrgencyType,
            bool billByTransport, string tptDeptCode, string oldTptDeptCode, string branchCode, byte[] updateVersion, bool isNew,
            ChargeType truckChargeType,
            string shipperCode, string shipperName, string shipperAdd1, string shipperAdd2, string shipperAdd3, string shipperAdd4,
            string consigneeCode, string consigneeName, string consigneeAdd1, string consigneeAdd2, string consigneeAdd3, string consigneeAdd4, string psaAccount,
            string subcontractorCode, string subcontractorName, string subcontractorAdd1, string subcontractorAdd2, string subcontractorAdd3, string subcontractorAdd4,
            string warehouseNo,
            string yourRefNo, string ucrNo, string permitNo1, DateTime datePermitNo1, string permitNo2, DateTime datePermitNo2,
            string remarks, string remarks2, string remarks3, string remarks4, string remarks5,
            string carrierAgent, string oblNo, string hblNo, string vesselNo, string voyageNo, string pol,
            string pod, DateTime eta, DateTime etd, string origin, string destination, string via, string flightNo,
            string HAWB, string MAWB, string AWBNo, DateTime flightDate, SortableList<TruckJobTrip> truckJobTrips, /*2014-04-09 Zhou Kai adds: , FreightType freightType, 2014-06-23 Zhou Kai adds*/ Trip_Type tripType, bool isBillable)
            : base(jobID, jobNo, custNo, sourceRef, jobstatus, jobType, bookingNo, bookingDate, branchCode, updateVersion, isNew, quotationNo, jobUrgencyType, billByTransport, tptDeptCode, oldTptDeptCode, isBillable)
        {
            this.truckChargeType = truckChargeType;
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
            this.psaAccount = psaAccount;
            this.subcontractorCode = subcontractorCode;
            this.subcontractorName = subcontractorName;
            this.subcontractorAdd1 = subcontractorAdd1;
            this.subcontractorAdd2 = subcontractorAdd2;
            this.subcontractorAdd3 = subcontractorAdd3;
            this.subcontractorAdd4 = subcontractorAdd4;
            this.warehouseNo = warehouseNo;
            this.yourRefNo = yourRefNo;
            this.ucrNo = ucrNo;
            this.permitNo1 = permitNo1;
            this.datePermitNo1 = datePermitNo1;
            this.permitNo2 = permitNo2;
            this.datePermitNo2 = datePermitNo2;
            this.remarks = remarks;
            this.remarks2 = remarks2;
            this.remarks3 = remarks3;
            this.remarks4 = remarks4;
            this.remarks5 = remarks5;

            this.carrierAgent = carrierAgent;
            this.oblNo = oblNo;
            this.hblNo = hblNo;
            this.vesselNo = vesselNo;
            this.voyageNo = voyageNo;
            this.pol = pol;
            this.pod = pod;
            this.eta = eta;
            this.etd = etd;

            this.origin = origin;
            this.destination = destination;
            this.via = via;
            this.flightNo = flightNo;
            this.HAWB = HAWB;
            this.MAWB = MAWB;
            this.AWBNo = AWBNo;
            this.flightDate = flightDate;

            this.truckJobTrips = truckJobTrips;
            base.CustNo = custNo;
            base.JobDateTime = bookingDate;
            base.JobType = jobType;

            // 2014-04-09 Zhou Kai adds
            //this.tripType = freightType;
            // 2014-04-09 Zhou Kai ends
            this.TripType = tripType; //20160408
        }

        public override string CustNo
        {
            get { return base.custNo; }
            set
            {
                try
                {
                    if (value != base.custNo)
                    {
                        if (this.truckJobTrips.Count > 0)
                            throw new FMException(TptResourceBLL.ErrCantChangeCustomerJobTripExist);

                        if (GetAllTruckJobChargesByType(this, TruckJobCharge.TRUCK_JOB_OTHER_CHARGES).Count > 0)
                            throw new FMException(TptResourceBLL.ErrCantChangeCustomerJobChargeExist);
                    }
                    base.custNo = value;
                }
                catch (FMException fmEx)
                {
                    value = base.custNo;
                    throw new FMException(fmEx.ToString());
                }
                catch (Exception ex)
                {
                    value = base.custNo;
                    throw new FMException(ex.ToString());
                }
            }
        }

        public override string JobType
        {
            get { return base.jobType; }
            set
            {
                try
                {
                    if (value != base.jobType)
                    {
                        if (this.truckJobTrips.Count > 0)
                            throw new FMException(TptResourceBLL.ErrCantChangeJobTypeJobTripExist);

                        if (GetAllTruckJobChargesByType(this, TruckJobCharge.TRUCK_JOB_OTHER_CHARGES).Count > 0)
                            throw new FMException(TptResourceBLL.ErrCantChangeJobTypeJobChargeExist);
                    }
                    base.jobType = value;
                }
                catch (FMException fmEx)
                {
                    value = base.jobType;
                    throw new FMException(fmEx.ToString());
                }
                catch (Exception ex)
                {
                    value = base.jobType;
                    throw new FMException(ex.ToString());
                }
            }
        }

        public override DateTime JobDateTime
        {
            get { return base.jobDateTime; }
            set
            {
                try
                {
                    string msg = "";
                    if (value.Date != base.jobDateTime.Date)
                    {
                        if (!base.quotationNo.Equals(string.Empty))
                        {
                            string tempQuotationNo = Quotation.GetValidQuotationNo(this.CustNo, value, out msg);
                            if (msg != "")
                                throw new FMException(msg.ToString());

                            if (isQuotationValid(value, tempQuotationNo, base.custNo))
                            {
                                base.jobDateTime = value;
                            }
                        }
                    }
                    base.jobDateTime = value;
                }
                catch (FMException fmEx)
                {
                    value = base.jobDateTime;
                    throw fmEx;
                }
                catch (Exception ex)
                {
                    value = base.jobDateTime;
                    throw new FMException(ex.ToString());
                }
            }
        }

        public ChargeType TruckJobChargeType
        {
            get { return truckChargeType; }
            set
            {
                try
                {
                    if (value != truckChargeType)
                    {
                        if (this.truckJobTrips.Count > 0)
                            throw new FMException(TptResourceBLL.ErrCantChangeChargeTypeJobTripExist);

                        if (GetAllTruckJobChargesByType(this, TruckJobCharge.TRUCK_JOB_OTHER_CHARGES).Count > 0)
                            throw new FMException(TptResourceBLL.ErrCantChangeChargeTypeJobChargeExist);
                    }
                    truckChargeType = value;
                }
                catch (FMException fmEx)
                {
                    value = truckChargeType;
                    throw fmEx;
                }
                catch (Exception ex)
                {
                    value = truckChargeType;
                    throw new FMException(ex.ToString());
                }
            }
        }

        // 2014-04-09 Zhou Kai adds property
        //public FreightType Freight_Type
        //{
        //    get { return tripType; }
        //    set { tripType = value; }
        //}
        // 2014-04-09 Zhou Kai ends

        public bool isQuotationValid(DateTime date, string newQuotationNo, string oldQuotationNo)
        {
            if (newQuotationNo == oldQuotationNo)
            {
                return true;
            }
            else
            {
                if (this.truckJobTrips.Count > 0)
                    throw new FMException(TptResourceBLL.ErrCantChangeBookingDateJobTrips);

                if (GetAllTruckJobChargesByType(this, TruckJobCharge.TRUCK_JOB_OTHER_CHARGES).Count > 0)
                    throw new FMException(TptResourceBLL.ErrCantChangeBookingDateCharges);

                return false;
            }
        }

        public bool ValidateAddTruckJobHeader()
        {
            bool valid = true;
            if (CustNo == "")
            {
                valid = false;
                throw new FMException(TptResourceBLL.ErrMissingCustomer);
            }
            if (BranchCode == "")
            {
                valid = false;
                throw new FMException(TptResourceBLL.ErrMissingBranchCode);
            }
            if (shipperCode != "")
            {
                if (shipperName == "")
                {
                    valid = false;
                    throw new FMException(TptResourceBLL.ErrMissingShipperName);
                }
                if (shipperAdd1 == "" && CustNo != "CASH")
                {
                    valid = false;
                    throw new FMException(TptResourceBLL.ErrMissingShipperAddress);
                }
            }
            if (consigneeCode != "")
            {
                if (consigneeName == "")
                {
                    valid = false;
                    throw new FMException(TptResourceBLL.ErrMissingConsigneeName);
                }
                if (consigneeAdd1 == "" && CustNo != "CASH")
                {
                    valid = false;
                    throw new FMException(TptResourceBLL.ErrMissingConsigneeAddress);
                }
            }
            return valid;
        }

        public bool ValidateEditTruckJobHeader()
        {
            bool valid = true;
            TruckJob oldTruckJob = new TruckJob();

            oldTruckJob = TruckJob.GetTruckJob(JobID);

            if (base.IsJobPosted())
            {
                valid = false;
                throw new FMException(TptResourceBLL.ErrCantEditJobPostedInv);
            }

            //if (CustNo != oldTruckJob.CustNo)
            //{
            //}
            oldTruckJob.truckJobTrips = oldTruckJob.GetTruckJobTrips();

            if (oldTruckJob.truckJobTrips.Count > 0)
            {
                if (oldTruckJob.JobType != JobType)
                {
                    valid = false;
                    throw new FMException(TptResourceBLL.ErrCantEditJobType);
                }
                if (oldTruckJob.truckChargeType != truckChargeType)
                {
                    valid = false;
                    throw new FMException(TptResourceBLL.ErrCantEditChargeType);
                }
            }
            return valid;
        }

        public bool ValidateDeleteTruckJobHeader()
        {
            bool valid = true;
            if (base.IsJobBilled()) //TruckJobDAL.IsJobBilled(this.JobNo))// )
            {
                valid = false;
                throw new FMException(TptResourceBLL.ErrCantDeleteBilledJob);
            }

            for (int i = 0; i < truckJobTrips.Count; i++)
            {
                if (truckJobTrips[i].TripStatus == JobTripStatus.Assigned || truckJobTrips[i].TripStatus == JobTripStatus.Completed)
                {
                    valid = false;
                    throw new FMException(TptResourceBLL.ErrCantDeleteAssignedJob);
                }

            }
            return valid;
        }

        public bool AddTruckJobHeader(string formName, string user)
        {
            bool added = false;
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            if (base.ValidateJobHeader() == true)
            {
                if (ValidateAddTruckJobHeader() == true)
                {
                    ApplicationOption appOption = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_SET_JOBTRIP_TO_READY);
                    JobTripStatus tripStatus = appOption.setting_Value == "T" ? JobTripStatus.Ready : JobTripStatus.Booked;
                    
                    string prefix = "";
                    prefix = TruckJobDAL.GetPrefix();

                    SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    try
                    {
                        // Chong chin 25 Aug 2010. Add an output parameter, new TruckJob
                        // This is help in NUNIT database testing
                        TruckJob truckJobOut = new TruckJob();
                        // Chong Chin 27 Aug 10. Add this output parameter needed for NUNIT tests
                        this.JobNo = TruckJobDAL.AddTruckJobHeader(this, con, tran, prefix, out truckJobOut);

                        //Auto add job trip for case retreived from Air freight jobs
                        if (!this.SourceReferenceNo.Equals(string.Empty) && this.truckJobTrips.Count > 0)
                        {
                            TruckJobTrip truckJobTripOut = new TruckJobTrip();
                            foreach (TruckJobTrip tempTrip in this.truckJobTrips)
                            {
                                tempTrip.JobID = this.JobID;
                                TruckJobDAL.AddTruckJobTrip(this, tempTrip, con, tran, out truckJobTripOut);
                                //20130227 add job trip state
                                JobTripState jobTripState = new JobTripState(1, tripStatus, this.JobDateTime, "", true);
                                tempTrip.AddJobTripStateForTruck(jobTripState, con, tran);
                                // 20130227 End

                                ////20161024 - gerry added new audit log
                                AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Job Trip", tempTrip.Sequence, "ADD");
                                auditLog.WriteAuditLog(tempTrip, null, con, tran);

                                if (tempTrip.truckJobTripDetail.Count > 0)
                                {
                                    TruckJobTripDetail outTruckJobTripDetail = new TruckJobTripDetail();
                                    foreach (TruckJobTripDetail tempTripDetail in tempTrip.truckJobTripDetail)
                                    {
                                        tempTripDetail.jobTripSequence = tempTrip.Sequence;
                                        TruckJobDAL.AddTruckJobTripDetail(tempTrip, tempTripDetail, con, tran, out outTruckJobTripDetail);

                                        ////20161024 - gerry added new audit log
                                        auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Job Trip Detail", tempTripDetail.detailSequence, FormMode.Add.ToString());
                                        auditLog.WriteAuditLog(tempTripDetail, null, con, tran);
                                    }
                                }
                            }
                        }
                        //end 20130112
                        TransportFacadeOut.AddTptJobToSpecialJobDetail(this, con, tran, prefix);

                        ////20161024 - gerry added new audit log
                        AuditLog auditLog1 = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Job Header", 0, FormMode.Add.ToString());
                        auditLog1.WriteAuditLog("JobNumber", this.JobNo, string.Empty, con, tran);


                        tran.Commit();
                        //return JobNo;
                        added = true;

                        //always get the update version after successful insert
                        this.UpdateVersion = GetHeaderUpdateVersion();
                    }
                    catch (FMException ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new FMException(ex.ToString());
                    }
                    finally
                    {
                        con.Close();
                    }

                }
            }
            //return FMException.CLASS_VALIDATION_ERROR + "TruckJob.AddTruckJobHeader";
            return added;
        }

        //public static List<Trip_Type> GetTruckJobTypes()
        //{
        //    List<String> truckJobTypes = new List<String>();
        //    truckJobTypes.Add(ONE_TO_ONE);
        //    truckJobTypes.Add(ONE_TO_MANY);
        //    truckJobTypes.Add(MANY_TO_ONE);
        //    return truckJobTypes;
        //}

        public byte[] GetHeaderUpdateVersion()
        {
            return TruckJobDAL.GetHeaderUpdateVersion(this.JobID);
        }
        // 2013-10-23 Zhou Kai adds parameter SUBCONCHAGES
        public bool EditTruckJobHeader(string formName, string user, SUBCONCHANGES subconChanges)
        {
            bool temp = false;
            try
            {
                if (ValidateEditTruckJobHeader())
                {
                    if ((TptDeptCode == OldTptDeptCode) | ((TptDeptCode != OldTptDeptCode) & (CanChangeTptDeptCode())))
                    {

                        TruckJob origTruckJob = GetTruckJob(this.JobID);
                        SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                        con.Open();
                        SqlTransaction tran = con.BeginTransaction();

                        try
                        {
                            // Chong Chin 31 Aug 10. Add TruckJob output parameter for NUNIT test
                            TruckJob truckJobOut = new TruckJob();
                            temp = TruckJobDAL.EditTruckHeader(this, con, tran, out truckJobOut);
                            // 2014-03-22 Zhou Kai comments out the logic below, because no sub-con changes here
                            // 2013-10-23 Zhou Kai adds logic
                            //if (temp)
                            //{
                            //    if (subconChanges == SUBCONCHANGES.ATOB)
                            //    {
                            //        TruckJobDAL.UpdateSubConCodeByJobId(this.JobID, truckJobOut.subcontractorCode, con, tran);
                            //        TruckJobDAL.EraseAllTruckJobTripsSubConData(this.JobID, con, tran);
                            //    }
                            //    else if (subconChanges == SUBCONCHANGES.ATOBLANK)
                            //    { TruckJobDAL.DeleteAllTruckJobTripsSubCon(this.JobID, con, tran); }
                            //    else if (subconChanges == SUBCONCHANGES.BLANKTOA)
                            //    {
                            //        foreach (TruckJobTrip tjt in this.truckJobTrips)
                            //        {
                            //            TruckJobTripSubCon tjts = new TruckJobTripSubCon(tjt);
                            //            tjts.AddTruckJobTripSubCon(con, tran);
                            //        }
                            //    }
                            //}
                            // 2013-10-23 Zhou Kai ends

                            string prefix = "";
                            prefix = TruckJobDAL.GetPrefix();

                            TransportFacadeOut.EditTptJobToSpecialJobDetail(this, con, tran, prefix);
                            /*
                            // now form the log entry
                            DateTime serverDateTime = Logger.GetServerDateTime();
                            
                             * #region "2013-12-23 Zhou Kai creates this code block to replace the block below"

                            LogHeader logHeader = new LogHeader(FMModule.Trucking, formName, serverDateTime,
                                this.JobNo.ToString(), String.Empty, FormMode.Edit, user);
                            LogDetail logDetail1 = new LogDetail(LOG_DETAIL_QUOTATION_NO, truckJobOut.QuotationNo);
                            logHeader.LogDetails.Add(logDetail1);
                            Logger.WriteLog(logHeader, con, tran);
                            #endregion
                           
                            // form the header
                            LogHeader logHeader = new LogHeader(FMModule.Trucking, formName, serverDateTime,
                                this.JobID.ToString(), this.JobID.ToString(), FormMode.Edit, user);

                            // This is an example of create the 2 logDetail objects
                            LogDetail logDetail1 = new LogDetail("CustNo", CustNo);
                            LogDetail logDetail2 = new LogDetail("JobType", JobType);

                            // add the 2 logDetails objects to the List collection of logHeader
                            logHeader.LogDetails.Add(logDetail1);
                            logHeader.LogDetails.Add(logDetail2);

                            // now call the Logger class to write
                            Logger.WriteLog(logHeader, con, tran);
                            */

                            ////20161024 - gerry added new audit log
                            AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Truck Job", 0, "EDT");
                            auditLog.WriteAuditLog(this, origTruckJob, con, tran);

                            tran.Commit();
                            //always get the update version after successful edit
                            this.UpdateVersion = GetHeaderUpdateVersion(); //
                            this.truckJobTrips = GetTruckJobTrips();   //20140322 - gerry added to insure the jobtrips were the latest from db
                        }
                        catch (FMException ex)
                        {
                            tran.Rollback();
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new FMException(ex.ToString());
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                    else
                    {
                        throw new FMException(TptResourceBLL.ErrCantEditJobHeader);
                    }
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return temp;
        }

        public bool CanChangeTptDeptCode()
        {
            if (this.AreAllJobTripsInBookedState() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AreAllJobTripsInBookedState()
        {
            bool temp = true;
            for (int i = 0; i < truckJobTrips.Count; i++)
            {
                if (truckJobTrips[i].TripStatus != JobTripStatus.Booked)
                {
                    temp = false;
                    break;
                }
            }
            return temp;
        }

        public bool DeleteTruckJob(string formName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            bool temp = false;
            if (ValidateDeleteTruckJobHeader() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    // 2013-10-22 Zhou Kai deletes all the truck job trip sub con data 
                    // under this truck job also
                    this.DeleteAllTruckJobTripsSubCon(con, tran);
                    temp = TruckJobDAL.DeleteTruckJob(this, con, tran);

                    TransportFacadeOut.DeleteTptJobToSpecialJobDetail(this, con, tran);
                    //20160902 -gerry added
                    TruckJobDAL.UpdateTRKJOB_FROM_WMS_Tbl(this.SourceReferenceNo, false, con, tran);

                    //20160811 - auditlog for job trip, only capture CargoDescription
                    AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, formName, 0, "DEL");
                    auditLog.WriteAuditLog(this, null, con, tran);

                    tran.Commit();
                }
                catch (FMException ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return temp;
        }

        public static SortableList<TruckJob> GetAllTruckJobs()
        {
            return TruckJobDAL.GetAllTruckJobs();
        }

        public static SortableList<TruckJob> GetTruckJobsWithRangedJobNo(string StartJobNo, string EndJobNo)
        {
            if (EndJobNo.CompareTo(StartJobNo) < 0)
            {
                throw new FMException(TptResourceBLL.ErrEndJobNoLesserThanStartJobNo);
            }
            return TruckJobDAL.GetTruckJobsWithRangedJobNo(StartJobNo, EndJobNo);
        }

        public static SortableList<TruckJob> GetTruckJobsWithRangedCustNo(string StartCustNo, string EndCustNo)
        {
            if (EndCustNo.CompareTo(StartCustNo) < 0)
            {
                throw new FMException(TptResourceBLL.ErrEndCustLesserThanStartCust);
            }

            return TruckJobDAL.GetTruckJobsWithRangedCustNo(StartCustNo, EndCustNo);
        }

        public static SortableList<TruckJob> GetTruckJobs(string StartJobNo, string EndJobNo, string StartCustNo, string EndCustNo)
        {
            if (EndJobNo.CompareTo(StartJobNo) < 0)
            {
                throw new FMException(TptResourceBLL.ErrEndJobNoLesserThanStartJobNo);
            }
            if (EndCustNo.CompareTo(StartCustNo) < 0)
            {
                throw new FMException(TptResourceBLL.ErrEndCustLesserThanStartCust);
            }

            return TruckJobDAL.GetTruckJobs(StartJobNo, EndJobNo, StartCustNo, EndCustNo);
        }

        public static TruckJob GetTruckJob(string JobNo)
        {
            return TruckJobDAL.GetTruckJob(JobNo);
        }

        public static TruckJob GetTruckJob(int JobId)
        {
            return TruckJobDAL.GetTruckJob(JobId);
        }

        public SortableList<TruckJobTrip> GetTruckJobTrips()
        {
            return TruckJobDAL.GetTruckJobTrips(this.JobID);
        }

        public bool CanEditJobType()
        {
            bool temp = true;
            if (truckJobTrips.Count > 0 || GetAllTruckJobChargesByType(this, TruckJobCharge.TRUCK_JOB_OTHER_CHARGES).Count > 0)
            {
                temp = false;
            }
            return temp;
        }

        public bool CanEditCustCode()
        {
            bool temp = true;
            if (truckJobTrips.Count > 0 || GetAllTruckJobChargesByType(this, TruckJobCharge.TRUCK_JOB_OTHER_CHARGES).Count > 0)
            {
                temp = false;
            }
            return temp;
        }

        public bool CanEditDept()
        {
            bool temp = true;
            if (truckJobTrips.Count > 0 || GetAllTruckJobChargesByType(this, TruckJobCharge.TRUCK_JOB_OTHER_CHARGES).Count > 0)
            {
                temp = false;
            }
            return temp;
        }

        public bool ValidateTruckJobTripUOM(TruckJobTrip truckJobTrip)
        {
            bool valid = false;
            try
            {
                if ((!this.QuotationNo.Equals(string.Empty) || this.QuotationNo.Length > 0)
                    && !truckJobTrip.isPackageDependent)
                {
                    SortableList<TransportRate> transportRates = new SortableList<TransportRate>();
                    transportRates = Quotation.GetTransportRatesBasedOnQuotationNo(QuotationNo);
                    //if (this.truckChargeType == TruckChargeType.StopDependent)
                    //    transportRates = Quotation.GetTruckMovementTransportRatesBasedOnQuotationNoAndStopDependent(QuotationNo, true);
                    //else
                    //    transportRates = Quotation.GetNonTruckMovementTransportRatesBasedOnQuotationNoAndStopDependent(QuotationNo, false);

                    for (int i = 0; i < transportRates.Count; i++)
                    {
                        if (truckJobTrip.billing_UOM == transportRates[i].UOM)
                        {
                            valid = true;
                            break;
                        }
                    }
                }
                else
                {
                    valid = true;
                }

                if (!valid)
                    throw new FMException("The selected UOM for this jobtrip is not in the quotation. ");

            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return valid;
        }
        public bool AddTruckJobTrip(TruckJobTrip truckJobTrip, string frmName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            //check for double container no
            bool temp = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ValidateTruckJobTripUOM(truckJobTrip))
                {
                    // validate business logic before calling HaulierJobDAL to save
                    if (truckJobTrip.ValidateTruckJobTrip() == true)
                    {
                        TruckJobTrip truckJobTripOut = new TruckJobTrip();

                        if (TruckJobDAL.AddTruckJobTrip(this, truckJobTrip, con, tran, out truckJobTripOut))
                        {
                            // 2013-10-22 Zhou Kai adds      
                            //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
                            ApplicationOption appOption = ApplicationOption.GetApplicationOption(
                                                                            ApplicationOption.TRUCKING_SETTINGS_ID,
                                                                            ApplicationOption.SETTINGS_TRUKC_JOB_BY_SUBCON);
                            bool isSubCon = appOption.setting_Value == "T" ? true : false;
                            //if (appOption.setting_Value && !truckJobTrip.OwnTransport)
                            if (isSubCon && !truckJobTrip.OwnTransport)
                            {
                                // add a truck job trip sub con also
                                truckJobTrip.AddTruckJobTripSubCon(con, tran);
                            }
                            // 2013-10-22 Zhou Kai ends

                            //20131001 - Gerry added for multi leg
                            if (truckJobTrip.legType != Leg_Type.OneLeg)
                            {
                                TruckJobDAL.UpdatePartnerLeg(truckJobTrip.JobID, truckJobTrip.partnerLeg, Leg_Type.FirstOfTwoLeg.GetHashCode(), truckJobTrip.Sequence, con, tran);
                            }
                            //end 20131001
                            // Chong Chin 13 Feb 2011 - Start - Removed the remarks for next 2 line
                            // Reason is JobTripState for trucks was introduced after booking was completed  
                            JobTripState jobTripState = new JobTripState(1, JobTripStatus.Booked, this.JobDateTime, "", true);
                            truckJobTrip.AddJobTripStateForTruck(jobTripState, con, tran);
                            // Chong Chin 13 Feb 2011 End   

                            //20160219 - gerry added to save truck job trip dimension if any
                            foreach (TruckJobTripDetail dimension in truckJobTrip.truckJobTripDetail)
                            {
                                truckJobTrip.AddTruckJobTripDetail(dimension, frmName, user, this, con, tran);
                            }

                            //ChangeTruckMovementCharges(this, con, tran);
                            //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                            this.truckJobTrips.Add(truckJobTrip);
                            //20131019 end

                            //20121108 - Gerry Added to update special Job detail table
                            decimal totalCBM = this.GetTotalBookedVolume();
                            decimal totalWeight = this.GetTotalBookedWeight();
                            SpecialJobDetail.EditSpecialJobDetailGrossWeightVolume(this.JobNo, totalWeight, totalCBM, con, tran);
                            //end 20121108

                            ////20161024 - gerry added new audit log
                            AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Job Trip", truckJobTrip.Sequence, "ADD");
                            auditLog.WriteAuditLog(truckJobTrip, null, con, tran);

                            tran.Commit();
                        }
                    }
                }
            }
            catch (FMException e)
            {
                tran.Rollback();
                throw e;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return temp;
        }
        public bool AddTruckJobTrip(TruckJobTrip truckJobTrip, SortableList<TruckJobTripDetail> dimensions, string frmName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            //check for double container no
            bool temp = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ValidateTruckJobTripUOM(truckJobTrip))
                {
                    // validate business logic before calling HaulierJobDAL to save
                    if (truckJobTrip.ValidateTruckJobTrip() == true)
                    {
                        ApplicationOption autoSetToReady = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID,
                                                                        ApplicationOption.SETTINGS_AUTO_SET_JOBTRIP_TO_READY);
                        if (autoSetToReady.setting_Value == "T")
                            truckJobTrip.TripStatus = JobTripStatus.Ready;


                        TruckJobTrip truckJobTripOut = new TruckJobTrip();

                        if (TruckJobDAL.AddTruckJobTrip(this, truckJobTrip, con, tran, out truckJobTripOut))
                        {
                            // 2013-10-22 Zhou Kai adds      
                            //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
                            ApplicationOption appOptionForSubCon = ApplicationOption.GetApplicationOption(
                                                                            ApplicationOption.TRUCKING_SETTINGS_ID,
                                                                            ApplicationOption.SETTINGS_TRUKC_JOB_BY_SUBCON);
                            bool isSubCon = appOptionForSubCon.setting_Value == "T" ? true : false;
                            //if (appOption.setting_Value && !truckJobTrip.OwnTransport)
                            if (isSubCon && !truckJobTrip.OwnTransport)
                            {
                                // add a truck job trip sub con also
                                truckJobTrip.AddTruckJobTripSubCon(con, tran);
                            }
                            // 2013-10-22 Zhou Kai ends

                            //20131001 - Gerry added for multi leg
                            if (truckJobTrip.legType != Leg_Type.OneLeg)
                            {
                                TruckJobDAL.UpdatePartnerLeg(truckJobTrip.JobID, truckJobTrip.partnerLeg, Leg_Type.FirstOfTwoLeg.GetHashCode(), truckJobTrip.Sequence, con, tran);
                            }
                            //end 20131001
                            // Chong Chin 13 Feb 2011 - Start - Removed the remarks for next 2 line
                            // Reason is JobTripState for trucks was introduced after booking was completed  
                            JobTripState jobTripState = new JobTripState(1, JobTripStatus.Booked, this.JobDateTime, "", true);
                            truckJobTrip.AddJobTripStateForTruck(jobTripState, con, tran);
                            // Chong Chin 13 Feb 2011 End  
 
                            //20160311 - gerry added
                            if (autoSetToReady.setting_Value == "T")
                            {
                                truckJobTrip.UpdateVersion = TruckJobDAL.GetTruckJobTripUpdateVersion(truckJobTrip, con, tran);
                                jobTripState = new JobTripState(1, JobTripStatus.Ready, this.JobDateTime, "Auto set to ready.", true);
                                truckJobTrip.AddJobTripStateForTruck(jobTripState, con, tran);
                            }
                            //20160219 - gerry added to save truck job trip dimension if any
                            foreach (TruckJobTripDetail dimension in dimensions)
                            {
                                truckJobTrip.AddTruckJobTripDetail(dimension, frmName, user, this, con, tran);
                            }

                            //ChangeTruckMovementCharges(this, con, tran);
                            //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                            this.truckJobTrips.Add(truckJobTrip);
                            //20131019 end

                            //20121108 - Gerry Added to update special Job detail table
                            decimal totalCBM = this.GetTotalBookedVolume();
                            decimal totalWeight = this.GetTotalBookedWeight();
                            SpecialJobDetail.EditSpecialJobDetailGrossWeightVolume(this.JobNo, totalWeight, totalCBM, con, tran);
                            //end 20121108

                            ////20161024 - gerry added new audit log
                            AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Truck Job Trip", truckJobTrip.Sequence, "ADD");
                            auditLog.WriteAuditLog(truckJobTrip, null, con, tran);

                            tran.Commit();
                        }
                    }
                }
            }
            catch (FMException e)
            {
                tran.Rollback();
                throw e;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return temp;
        }

        public bool EditTruckJobTrip(TruckJobTrip truckJobTrip, string frmName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ValidateTruckJobTripUOM(truckJobTrip))
                {
                    if (truckJobTrip.ValidateTruckJobTrip())
                    {
                        TruckJobTrip origTruckJobTrip = TruckJobTrip.GetTruckJobTrip(truckJobTrip.JobID, truckJobTrip.Sequence, con, tran);
                        TruckJobTrip truckJobTripOut = new TruckJobTrip();
                        temp = TruckJobDAL.EditTruckJobTrip(truckJobTrip, con, tran, out truckJobTripOut);
                        // 2014-03-24 Zhou Kai adds           
                        //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
                        ApplicationOption appOption = ApplicationOption.GetApplicationOption(
                                                                            ApplicationOption.TRUCKING_SETTINGS_ID,
                                                                            ApplicationOption.SETTINGS_TRUKC_JOB_BY_SUBCON);
                        bool isSubCon = appOption.setting_Value == "T" ? true : false;
                        //if (appOption.setting_Value && !truckJobTrip.OwnTransport)
                        if (isSubCon && !truckJobTrip.OwnTransport)
                        {
                            // 2014-03-25 Zhou Kai adds comment, put edition of of truckJobTrip 
                            // and truckJobTripSubCon in the same transaction
                            EditTruckJobTripSubConRelatedData(truckJobTrip, con, tran); 
                        }
                        // 2014-03-24 Zhou Kai ends

                        //20160219 - gerry added to save truck job trip dimension if any
                        TruckJobDAL.DeleteTruckJobTripDetails(truckJobTrip, con, tran);
                        //reset the list
                        truckJobTripOut.truckJobTripDetail = new SortableList<TruckJobTripDetail>();
                        foreach (TruckJobTripDetail dimension in truckJobTrip.truckJobTripDetail)
                        {
                            truckJobTripOut.AddTruckJobTripDetail(dimension, frmName, user, this, con, tran);
                        }
                        truckJobTrip.UpdateVersion = truckJobTrip.GetTruckJobTripUpdatedVersion(con, tran);
                        //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                        //ChangeTruckMovementCharges(this, con, tran);
                        //20131019 end

                        //20121108 - Gerry Added to update special Job detail table
                        decimal totalCBM = this.GetTotalBookedVolume();
                        decimal totalWeight = this.GetTotalBookedWeight();
                        temp = SpecialJobDetail.EditSpecialJobDetailGrossWeightVolume(this.JobNo, totalWeight, totalCBM, con, tran);


                        ////20161024 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Truck Job Trip", truckJobTrip.Sequence, "EDT");
                        auditLog.WriteAuditLog(truckJobTrip, origTruckJobTrip, con, tran);

                        tran.Commit();
                    }
                }
            }
            catch (FMException e)
            {
                tran.Rollback();
                throw e;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return temp;
        }
        public bool EditTruckJobTrip(TruckJobTrip truckJobTrip, SortableList<TruckJobTripDetail> dimensions, string frmName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ValidateTruckJobTripUOM(truckJobTrip))
                {
                    if (truckJobTrip.ValidateTruckJobTrip())
                    {
                        TruckJobTrip origTruckJobTrip = TruckJobTrip.GetTruckJobTrip(truckJobTrip.JobID, truckJobTrip.Sequence, con, tran);
                        TruckJobTrip truckJobTripOut = new TruckJobTrip();
                        temp = TruckJobDAL.EditTruckJobTrip(truckJobTrip, con, tran, out truckJobTripOut);
                        // 2014-03-24 Zhou Kai adds           
                        //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
                        ApplicationOption appOption = ApplicationOption.GetApplicationOption(
                                                                            ApplicationOption.TRUCKING_SETTINGS_ID,
                                                                            ApplicationOption.SETTINGS_TRUKC_JOB_BY_SUBCON);
                        bool isSubCon = appOption.setting_Value == "T" ? true : false;
                        //if (appOption.setting_Value && !truckJobTrip.OwnTransport)
                        if (isSubCon && !truckJobTrip.OwnTransport)
                        {
                            // 2014-03-25 Zhou Kai adds comment, put edition of of truckJobTrip 
                            // and truckJobTripSubCon in the same transaction
                            EditTruckJobTripSubConRelatedData(truckJobTrip, con, tran);
                        }
                        // 2014-03-24 Zhou Kai ends

                        //20160219 - gerry added to save truck job trip dimension if any
                        TruckJobDAL.DeleteTruckJobTripDetails(truckJobTrip, con, tran);
                        //reset the list
                        truckJobTripOut.truckJobTripDetail = new SortableList<TruckJobTripDetail>();
                        foreach (TruckJobTripDetail dimension in truckJobTrip.truckJobTripDetail)
                        {
                            truckJobTripOut.AddTruckJobTripDetail(dimension, frmName, user, this, con, tran);
                        }
                        truckJobTrip.UpdateVersion = truckJobTrip.GetTruckJobTripUpdatedVersion(con, tran);
                        //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                        //ChangeTruckMovementCharges(this, con, tran);
                        //20131019 end

                        //20121108 - Gerry Added to update special Job detail table
                        decimal totalCBM = this.GetTotalBookedVolume();
                        decimal totalWeight = this.GetTotalBookedWeight();
                        temp = SpecialJobDetail.EditSpecialJobDetailGrossWeightVolume(this.JobNo, totalWeight, totalCBM, con, tran);


                        ////20161024 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Truck Job Trip", truckJobTrip.Sequence, "EDT");
                        auditLog.WriteAuditLog(truckJobTrip, origTruckJobTrip, con, tran);

                        tran.Commit();
                    }
                }
            }
            catch (FMException e)
            {
                tran.Rollback();
                throw e;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return temp;
        }

        #region "2014-03-21 Zhou Kai adds"
        public bool EditTruckJobTrip(TruckJobTrip truckJobTrip, SortableList<TruckJobTripDetail> dimensions, string frmName, string user, SUBCONCHANGES subConChanges)
        {
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }

            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ValidateTruckJobTripUOM(truckJobTrip))
                {
                    if (truckJobTrip.ValidateTruckJobTrip())
                    {
                        TruckJobTrip origTruckJobTrip = TruckJobTrip.GetTruckJobTrip(truckJobTrip.JobID, truckJobTrip.Sequence, con, tran);
                        TruckJobTrip truckJobTripOut = new TruckJobTrip();
                        temp = TruckJobDAL.EditTruckJobTrip(truckJobTrip, con, tran, out truckJobTripOut);

                        //20140502 - gerry added condition   if (appOption.setting_Value)     
                        //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
                        ApplicationOption appOption = ApplicationOption.GetApplicationOption(
                                                                            ApplicationOption.TRUCKING_SETTINGS_ID,
                                                                            ApplicationOption.SETTINGS_TRUKC_JOB_BY_SUBCON);
                        bool isSubCon = appOption.setting_Value == "T" ? true : false;
                        if (isSubCon)
                        {

                            // 2014-03-25 Zhou Kai adds comment, put edition of of truckJobTrip 
                            // and truckJobTripSubCon in the same transaction
                            if (subConChanges == SUBCONCHANGES.NOCHANGES)
                            {
                                // 2014-04-11 Gerry detected a bug here:
                                // when subConChanges == NOCHANGES, the subContractor trip may also be not exist,
                                // then null reference bug will occur
                                // 
                                if (!(truckJobTrip.subCon.Equals(String.Empty) || truckJobTrip.OwnTransport))
                                {
                                    EditTruckJobTripSubConRelatedData(truckJobTrip, con, tran);
                                }
                                // 2014-04-11 Zhou Kai ends
                            }
                            if (subConChanges == SUBCONCHANGES.ATOB) { EditTruckJobTripSubConRelatedData(truckJobTrip, con, tran); }
                            if (subConChanges == SUBCONCHANGES.BLANKTOA)
                            {
                                // add the truck job trip by sub-contractor
                                truckJobTrip.AddTruckJobTripSubCon(con, tran);
                            }
                            if (subConChanges == SUBCONCHANGES.ATOBLANK)
                            {
                                // delete the corrsoponding truck job trip by sub-contractor
                                truckJobTrip.DeleteTruckJobTripBySubCon(con, tran);
                            }

                            // 2013-11-08 Zhou Kai ends
                        }
                        //20140502 gerry end

                        //20160219 - gerry added to save truck job trip dimension if any
                        TruckJobDAL.DeleteTruckJobTripDetails(truckJobTrip, con, tran);
                        //reset the list
                        truckJobTripOut.truckJobTripDetail = new SortableList<TruckJobTripDetail>();
                        foreach (TruckJobTripDetail dimension in truckJobTrip.truckJobTripDetail)
                        {
                            truckJobTripOut.AddTruckJobTripDetail(dimension, frmName, user, this, con, tran);
                        }
                        truckJobTrip.UpdateVersion = truckJobTrip.GetTruckJobTripUpdatedVersion(con, tran);
                        //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                        //ChangeTruckMovementCharges(this, con, tran);
                        //20131019 end

                        //20121108 - Gerry Added to update special Job detail table
                        decimal totalCBM = this.GetTotalBookedVolume();
                        decimal totalWeight = this.GetTotalBookedWeight();
                        temp = SpecialJobDetail.EditSpecialJobDetailGrossWeightVolume(this.JobNo, totalWeight, totalCBM, con, tran);


                        ////20161024 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Truck Job Trip", truckJobTrip.Sequence, "EDT");
                        auditLog.WriteAuditLog(truckJobTrip, origTruckJobTrip, con, tran);

                        tran.Commit();
                    }
                }
            }
            catch (FMException e)
            {
                tran.Rollback();
                throw e;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return temp;
        }
        #endregion

        public bool DeleteTruckJobTrip(TruckJobTrip truckJobTrip, string frmName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            // validate business logic before calling HaulierJobDAL to save
            if (truckJobTrip.ValidateDeleteTruckJobTrip() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    // 2013-10-22 Zhou Kai deletes truck job trips by sub con data also      
                    //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
                    ApplicationOption appOption = ApplicationOption.GetApplicationOption(
                                                                    ApplicationOption.TRUCKING_SETTINGS_ID,
                                                                    ApplicationOption.SETTINGS_TRUKC_JOB_BY_SUBCON);
                    bool isSubCon = appOption.setting_Value == "T" ? true : false;
                    //if (appOption.setting_Value && !truckJobTrip.OwnTransport)
                    if (isSubCon && !truckJobTrip.OwnTransport)
                    {
                        truckJobTrip.DeleteTruckJobTripBySubCon(con, tran);
                    }
                    // 2013-10-22 Zhou Kai ends

                    if (TruckJobDAL.DeleteTruckJobTrip(truckJobTrip, con, tran))
                    {
                        //20131001 - Gerry added for multi leg
                        if (truckJobTrip.legType != Leg_Type.OneLeg)
                        {
                            TruckJobDAL.UpdatePartnerLeg(truckJobTrip.JobID, truckJobTrip.partnerLeg, Leg_Type.OneLeg.GetHashCode(), 0, con, tran);
                        }
                        //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                        this.truckJobTrips.Remove(truckJobTrip);
                        ChangeTruckMovementCharges(this, con, tran);
                        //20131019 end

                        //end  20131001
                        //20121108 - Gerry Added to update special Job detail table
                        decimal totalCBM = this.GetTotalBookedVolume();
                        decimal totalWeight = this.GetTotalBookedWeight();
                        SpecialJobDetail.EditSpecialJobDetailGrossWeightVolume(this.JobNo, totalWeight, totalCBM, con, tran);
                        //end 20121108

                        #region 20160811 - gerry replaced auditlog
                        /*
                        #region "2014-02-03 Zhou Kai modifies logs for deletion"
                        // now form the log entry
                        DateTime serverDateTime = Logger.GetServerDateTime();
                        // form the header
                        LogHeader logHeader = new LogHeader(FMModule.Trucking, frmName, serverDateTime,
                            truckJobTrip.JobNo.ToString(), truckJobTrip.Sequence.ToString(), FormMode.Delete, user);
                        // This is an example of create the 2 logDetail objects
                        LogDetail logDetail2 = new LogDetail(LOG_DETAIL_DELETE_DATE, serverDateTime.ToString("yyyy-MM-dd HH:mm"));
                        //LogDetail logDetail3 = new LogDetail("GrossCBM", truckJobTrip.BookedVol.ToString());
                        //// add the 2 logDetails objects to the List collection of logHeader
                        logHeader.LogDetails.Add(logDetail2);
                        //logHeader.LogDetails.Add(logDetail3);
                        //// now call the Logger class to write
                        Logger.WriteLog(logHeader, con, tran);
                        #endregion
                        */
                        #endregion

                        //20160811 - auditlog for job trip, only capture CargoDescription
                        AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, frmName, truckJobTrip.Sequence, "DEL");
                        auditLog.WriteAuditLog(truckJobTrip, null, con, tran);

                        tran.Commit();
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            else return false;
        }

        public ArrayList GetTotalPieces()
        {
            ArrayList uomlist = new ArrayList();
            ArrayList list = new ArrayList();

            uomlist = GetUoms();
            for (int i = 0; i < uomlist.Count; i++)
            {

                int total = 0;
                for (int k = 0; k < truckJobTrips.Count; k++)
                {
                    for (int j = 0; j < truckJobTrips[k].truckJobTripDetail.Count; j++)
                    {
                        if ((string)uomlist[i] == (string)truckJobTrips[k].truckJobTripDetail[j].uom.ToString())
                        {
                            total = total + truckJobTrips[k].truckJobTripDetail[j].quantity;
                        }
                    }
                }
                list.Add(total);

            }
            return list;
        }


        public ArrayList GetUoms()
        {
            ArrayList list = new ArrayList();

            for (int k = 0; k < truckJobTrips.Count; k++)
            {
                for (int i = 0; i < truckJobTrips[k].truckJobTripDetail.Count; i++)
                {
                    if (list.Count == 0)
                    {
                        list.Add(truckJobTrips[k].truckJobTripDetail[i].uom);
                    }
                    else
                    {
                        bool found = false;
                        for (int j = 0; j < list.Count; j++)
                        {
                            if ((string)list[j] == (string)truckJobTrips[k].truckJobTripDetail[i].uom)
                            {
                                found = true;
                            }
                        }
                        if (found == false)
                        {
                            list.Add(truckJobTrips[k].truckJobTripDetail[i].uom);
                        }
                    }
                }
            }
            return list;

        }

        public decimal GetTotalVolume()
        {

            decimal total = 0;
            for (int i = 0; i < truckJobTrips.Count; i++)
            {
                for (int j = 0; j < truckJobTrips[i].truckJobTripDetail.Count; j++)
                {
                    total = total + truckJobTrips[i].truckJobTripDetail[j].volume;
                }
            }
            return total;
        }

        public decimal GetTotalWeight()
        {
            decimal total = 0;
            for (int i = 0; i < truckJobTrips.Count; i++)
            {
                for (int j = 0; j < truckJobTrips[i].truckJobTripDetail.Count; j++)
                {
                    total = total + truckJobTrips[i].truckJobTripDetail[j].totalWeight;
                }
            }
            return total;
        }

        public static ArrayList GetAllTruckJobNo()
        {
            return TruckJobDAL.GetAllTruckJobNo();
        }

        // 2014-01-10 Zhou Kai adds
        public static List<string> GetAllTruckJobNoListString()
        {
            return TruckJobDAL.GetAllTruckJobNoListString();
        }

        public static List<string> GetTruckJobNos()
        {
            return TruckJobDAL.GetTruckJobNos();
        }

        public bool AddTruckJobCharges(TruckJobCharge truckJobCharge, string frmName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            if (truckJobCharge.ValidateAddTruckJobCharge(this, truckJobCharge))
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    if (TruckJobDAL.AddTruckJobCharges(truckJobCharge, con, tran) == true)
                    {
                        if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                            truckJobCharge.OldJobChargeStatus = JobChargeStatus.Booked;
                        if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                            truckJobCharge.OldJobChargeStatus = JobChargeStatus.Completed;
                        if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                            truckJobCharge.OldJobChargeStatus = JobChargeStatus.Invoiced;


                        ////20161024 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Job Charge", truckJobCharge.SequenceNo, "ADD");
                        auditLog.WriteAuditLog(truckJobCharge, null, con, tran);

                        tran.Commit();
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            else return false;
        }

        public static bool EditTruckJobCharges(TruckJobCharge truckJobCharge,
            string frmName, string user, SqlConnection con, SqlTransaction tran)
        {
            bool success = true;
            if (truckJobCharge.ValidateEditJobCharge() == true)
            {
                try
                {
                    TruckJobDAL.EditTruckJobCharges(truckJobCharge, con, tran);
                    if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                        truckJobCharge.OldJobChargeStatus = JobChargeStatus.Booked;
                    if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                        truckJobCharge.OldJobChargeStatus = JobChargeStatus.Completed;
                    if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                        truckJobCharge.OldJobChargeStatus = JobChargeStatus.Invoiced;

                }
                catch (Exception ex)
                {
                    success = false;
                    throw ex;
                }
            }
            return success;
        }

        public bool EditTruckJobCharges(TruckJobCharge truckJobCharge, string frmName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            //if (truckJobCharge.ValidateAddTruckJobCharge(this, truckJobCharge))
            string msg = "";
            bool success = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                TruckJobCharge origCharge = TruckJobDAL.GetTruckJobCharge(truckJobCharge);   
                if (TruckJobDAL.EditTruckJobCharges(truckJobCharge, con, tran) == true)
                {
                    if (truckJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                        truckJobCharge.OldJobChargeStatus = JobChargeStatus.Booked;
                    if (truckJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                        truckJobCharge.OldJobChargeStatus = JobChargeStatus.Completed;
                    if (truckJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                        truckJobCharge.OldJobChargeStatus = JobChargeStatus.Invoiced;

                    ////20161024 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Truck Job Trip", truckJobCharge.SequenceNo, "EDT");
                    auditLog.WriteAuditLog(truckJobCharge, origCharge, con, tran);

                    tran.Commit();
                }
            }
            catch (FMException fmEx)
            {
                success = false;
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                success = false;
                tran.Rollback();
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return success;
        }
        //20130423 - gerry added parameter for job charge type "T"(truckmovement) or "S"(other charges)
        public static SortableList<TruckJobCharge> GetAllTruckJobChargesByType(TruckJob truckJob, string jobChargeType)
        {
            return TruckJobDAL.GetAllTruckJobChargesByType(truckJob, jobChargeType);
        }

        public static string GetPrefix()
        {
            return TruckJobDAL.GetPrefix();
        }

        public bool DeleteTruckJobCharges(TruckJobCharge truckJobCharge, string frmName, string user)
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            if (truckJobCharge.ValidateDeleteJobCharge() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    if (TruckJobDAL.DeleteTruckJobCharges(truckJobCharge, con, tran) == true)
                    {
                        #region 20160811 - gerry replaced auditlog
                        /*
                        #region "2014-01-23 Zhou Kai comments out this block"
                        // now form the log entry
                        DateTime serverDateTime = Logger.GetServerDateTime();
                        LogHeader logHeader = new LogHeader(FMModule.Trucking, frmName, serverDateTime,
                           this.JobNo, truckJobCharge.SequenceNo.ToString(), FormMode.Delete, user);

                        LogDetail logDetail1 = new LogDetail(LOG_DETAIL_DELETE_DATE, serverDateTime.ToString("yyyy-MM-dd HH:mm"));
                        logHeader.LogDetails.Add(logDetail1);
                        Logger.WriteLog(logHeader, con, tran);
                        #endregion
                        */
                        #endregion

                        //20160811 - auditlog for job trip, only capture CargoDescription
                        AuditLog auditLog = new AuditLog(truckJobCharge.ChargeCode, "TRK", "BK", this.JobID, user, DateTime.Now, frmName, truckJobCharge.SequenceNo, "DEL");
                        auditLog.WriteAuditLog(truckJobCharge, null, con, tran);
                        tran.Commit();
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            else return false;
        }

        //use in invoicing
        public static bool SetTruckJobChargeStatus(TruckJobCharge truckJobCharge, int invTrxNo, int jobID, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.SetTruckJobChargeStatus(truckJobCharge, invTrxNo, jobID, con, tran);
        }
        //use in invoicing
        public static bool SetTruckJobChargeStatus(TruckJobCharge truckJobCharge, int invTrxNo, int invItemSeqNo, int jobNo, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.SetTruckJobChargeStatus(truckJobCharge, invTrxNo, invItemSeqNo, jobNo, con, tran);
        }
        //use in invoicing
        public static bool SetTruckJobChargeStatus(TruckJobCharge truckJobCharge, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.SetTruckJobChargeStatus(truckJobCharge, con, tran);
        }

        public decimal GetTotalBookedVolume()
        {
            decimal totalVol = 0;
            try
            {
                //return TruckJobDAL.GetTotalCBM(this.JobID, con, tran);
                foreach (TruckJobTrip tempTrip in this.truckJobTrips)
                {
                    totalVol += tempTrip.BookedVol;
                }
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return totalVol;
        }
        public decimal GetTotalBookedWeight()
        {
            decimal bookedWeight = 0;
            try
            {
                //return TruckJobDAL.GetTotalWeight(this.JobID, con, tran);
                foreach (TruckJobTrip tempTrip in this.truckJobTrips)
                {
                    bookedWeight += tempTrip.BookedWeight;
                }
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return bookedWeight;
        }
        //Check if AirJob Already exist
        public static bool DoesAirJobNumberExist(string jobNo, out string outTruckJobNo)
        {
            try
            {
                return TruckJobDAL.DoesAirJobNumberExist(jobNo, out outTruckJobNo);
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
        }

        public static SortableList<TruckJobCharge> ChangeTruckMovementCharges(TruckJob truckJob, SqlConnection con, SqlTransaction tran)
        {
            SortableList<TruckJobCharge> truckMovementCharges = new SortableList<TruckJobCharge>();
            //SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            //if (con.State == ConnectionState.Closed) { con.Open(); }
            //SqlTransaction tran = con.BeginTransaction();
            try
            {
                //get the special settings if invoice is by each trip or not  
                //20140522 - gerry changed from ApplicationOption.SETTINGS_ID
                ApplicationOption appOption = ApplicationOption.GetApplicationOption(ApplicationOption.TRUCKING_SETTINGS_ID, ApplicationOption.SETTINGS_RETRIEVE_JOB_TRIP_TO_INVOICE_NAME);
                //bool isBilledEachTrip = TransportSettings.GetSpecialSettingValue(RETRIEVE_JOB_TRIP_TO_INVOICE);
                if (!truckJob.quotationNo.Equals(string.Empty))
                {
                    Quotation quotation = Quotation.GetAllQuotationHeader(truckJob.quotationNo);
                    //TODO delete all truck movement charges
                    TruckJobDAL.DeleteAllTruckMovementCharges(truckJob.JobID, con, tran);
                    //Create new collection of truck movement charges
                    if (truckJob.truckJobTrips.Count > 0)
                    {
                        foreach (TruckJobTrip trip in truckJob.truckJobTrips)
                        {
                            TruckJobCharge tempTruckJobCharge = null;
                            if (trip.isPackageDependent && trip.isMultiplePackage)
                            {
                                #region Multiple packages
                                foreach (TruckJobTripDetail tripDetail in trip.truckJobTripDetail)
                                {
                                    foreach (TransportRate rate in quotation.TransportRates)
                                    {
                                        if (tripDetail.uom.ToUpper().Equals(rate.UOM.ToUpper())
                                            && trip.ChargeCode.ToUpper().Equals(rate.ChargeID.ToUpper()))
                                        {
                                            tempTruckJobCharge = FillTruckJobChargeFromTripDetail(truckJob, rate, tripDetail);
                                            truckMovementCharges.Add(tempTruckJobCharge);
                                            break;

                                        }
                                    }
                                }
                                #endregion
                            }
                            else     //if not package dependent or package dependent but single package size inside
                            {
                                bool exist = false;  //20140430 - gery tranfer from below
                                foreach (TransportRate rate in quotation.TransportRates)
                                {
                                    if (!trip.isMultiplePackage
                                        && trip.billing_UOM.ToUpper().Equals(rate.UOM.ToUpper())
                                        && trip.ChargeCode.ToUpper().Equals(rate.ChargeID.ToUpper()))
                                    {
                                        tempTruckJobCharge = FillTruckJobChargeFromTrip(truckJob, rate, trip);
                                        //truckMovementCharges.Add(tempTruckJobCharge);  //20140430 - gerry removed
                                        break;
                                    }
                                }
                                //insert truckjobcharge if not exist and calculate the quantity
                                bool isretriveJobByIncvoice = appOption.setting_Value == "T" ? true : false;
                                if (tempTruckJobCharge != null && !isretriveJobByIncvoice)
                                {
                                    //bool exist = false; //20140430 - gery tranfer above
                                    for (int i = 0; i < truckMovementCharges.Count; i++)
                                    {
                                        if (!trip.isMultiplePackage
                                            && truckMovementCharges[i].ChargeCode == tempTruckJobCharge.ChargeCode
                                            && truckMovementCharges[i].Uom == tempTruckJobCharge.Uom)
                                        {
                                            truckMovementCharges[i].Quantity += tempTruckJobCharge.Quantity;
                                            exist = true;
                                            break;
                                        }
                                    }
                                }
                                //20140430 - gerry transfer outside the considation if (tempTruckJobCharge != null && !appOption.setting_Value)
                                if (!exist)
                                    truckMovementCharges.Add(tempTruckJobCharge);
                            }
                        }
                    }
                    if (truckMovementCharges.Count > 0)
                    {
                        //calculate each truckjobcharge amount
                        CalculateAmtForTruckMovementCharges(truckMovementCharges, quotation);
                        foreach (TruckJobCharge temp in truckMovementCharges)
                        {  //insert to database
                             TruckJobDAL.AddTruckJobCharges(temp, con, tran);
                            // 2014-03-20 Zhou Kai adds comments:
                            // No logs written for the truckMovementCharges, because the truckMovementCharges
                            // which are "T" type charges are automatically added(not by the user) when a truck job/
                            // truck job trip is added/edited/deleted.
                        }
                    }
                }
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return truckMovementCharges;
        }
        public static SortableList<TruckJobCharge> CalculateAmtForTruckMovementCharges(SortableList<TruckJobCharge> truckMovementCharges, Quotation quotation)
        {
            try
            {
                foreach (TruckJobCharge truckJobCharge in truckMovementCharges)
                {
                    foreach (TransportRate rate in quotation.TransportRates)
                    {
                        if (truckJobCharge.ChargeCode == rate.ChargeID && truckJobCharge.Uom == rate.UOM)
                        {
                            Charge tempTPTCharge = Charge.GetCharge(rate.ChargeID, quotation.BranchCode);
                            //20130903 - Gerry replaced the options
                            //if (tempTPTCharge.IsInvoiceAmtDependentOnWtVol)
                            if (tempTPTCharge.invoiceChargeType == InvoiceChargeType.DependOnWeightVolume
                                || tempTPTCharge.invoiceChargeType == InvoiceChargeType.DependOnPackagetype)
                            {
                                //TODO calculate price break based on Charge type (R or S)
                                truckJobCharge.TotalAmountFC = rate.GetTotal(truckJobCharge.Quantity);
                                //gstAmount is assign in setter method when totalAmountFC set
                                if (truckJobCharge.Quantity != 0)
                                {
                                    truckJobCharge.UnitRateFC = truckJobCharge.TotalAmountFC / truckJobCharge.Quantity;
                                }
                            }
                            else
                            {
                                truckJobCharge.UnitRateFC = rate.PriceBreaks[0].IsLumpSum ? rate.PriceBreaks[0].LumpSumValue : rate.PriceBreaks[0].Rate;
                                truckJobCharge.TotalAmountFC = truckJobCharge.Quantity * truckJobCharge.UnitRateFC;
                            }
                        }
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return null;
        }
        public static bool DeleteAllTruckMovementCharges(TruckJob truckJob, SqlConnection con, SqlTransaction tran)
        {
            // 2013-11-01 Zhou Kai adds
            if (truckJob.JobStatus == JobStatus.Completed) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            bool retValue = false;
            try
            {
                retValue = TruckJobDAL.DeleteAllTruckMovementCharges(truckJob.JobID, con, tran);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;
        }
        private static TruckJobCharge FillTruckJobChargeFromTrip(TruckJob truckJob, TransportRate rate, TruckJobTrip trip)
        {
            try
            {
                Charge tempTPTCharge = Charge.GetCharge(rate.ChargeID, truckJob.BranchCode);
                TruckJobCharge tempTruckJobCharge = new TruckJobCharge();
                tempTruckJobCharge.QuotationID = rate.QuotationID;
                tempTruckJobCharge.QuotationNo = truckJob.QuotationNo;
                tempTruckJobCharge.RateType = rate.ChargeType;
                tempTruckJobCharge.ChargeCode = rate.ChargeID;
                tempTruckJobCharge.ChargeDescription = rate.Description;
                tempTruckJobCharge.Uom = trip.billing_UOM;
                tempTruckJobCharge.JobID = trip.JobID;
                tempTruckJobCharge.Currency = rate.Currency;
                tempTruckJobCharge.ExchangeRate = (CurrencyRate.GetCurrencyRate(rate.Currency, truckJob.jobDateTime)).Rate;
                tempTruckJobCharge.JobChargeType = "T";
                Gst tempGst = Gst.GetGstRate(tempTPTCharge.TaxCode, truckJob.JobDateTime);
                tempTruckJobCharge.GstRate = tempGst.GstRate;
                tempTruckJobCharge.GstType = tempTPTCharge.TaxCode;
                tempTruckJobCharge.JobChargeStatus = JobChargeStatus.Booked;
                tempTruckJobCharge.Quantity = trip.BillingQty;
                //201310 - gerry added for job trip seqNo
                tempTruckJobCharge.JobTripSeqNo = trip.Sequence;

                return tempTruckJobCharge;
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

        }
        private static TruckJobCharge FillTruckJobChargeFromTripDetail(TruckJob truckJob, TransportRate rate, TruckJobTripDetail tripDetail)
        {
            try
            {
                Charge tempTPTCharge = Charge.GetCharge(rate.ChargeID, truckJob.BranchCode);
                TruckJobCharge tempTruckJobCharge = new TruckJobCharge();
                tempTruckJobCharge.QuotationID = rate.QuotationID;
                tempTruckJobCharge.QuotationNo = truckJob.QuotationNo;
                tempTruckJobCharge.RateType = rate.ChargeType;
                tempTruckJobCharge.ChargeCode = rate.ChargeID;
                tempTruckJobCharge.ChargeDescription = rate.Description;
                tempTruckJobCharge.Uom = tripDetail.uom;
                tempTruckJobCharge.JobID = truckJob.JobID;
                tempTruckJobCharge.Currency = rate.Currency;
                tempTruckJobCharge.ExchangeRate = (CurrencyRate.GetCurrencyRate(rate.Currency, truckJob.jobDateTime)).Rate;
                tempTruckJobCharge.JobChargeType = "T";
                Gst tempGst = Gst.GetGstRate(tempTPTCharge.TaxCode, truckJob.JobDateTime);
                tempTruckJobCharge.GstRate = tempGst.GstRate;
                tempTruckJobCharge.GstType = tempTPTCharge.TaxCode;
                tempTruckJobCharge.JobChargeStatus = JobChargeStatus.Booked;
                tempTruckJobCharge.Quantity = tripDetail.quantity;
                //201310 - gerry added for job trip seqNo
                tempTruckJobCharge.JobTripSeqNo = tripDetail.jobTripSequence;

                return tempTruckJobCharge;
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

        }

        /*
         * 2013-09-26 Zhou Kai adds this function to:
         * (1) Get all truckJobTripsSubCon
         */
        public SortableList<TruckJobTripSubCon> GetAllTruckJobTripsSubCon()
        {
            return TruckJobDAL.GetAllTruckJobTripSubCon(this.JobID);
        }
        public SortableList<TruckJobTripSubCon> GetAllSubConDataWithTruckJobTripCompleted()
        {
            return TruckJobDAL.GetAllSubConDataWithTruckJobTripCompleted(this.JobID);
        }

        /*
         * 2013-09-25 Zhou Kai adds this function to:
         * (1) Call AreAllJobTripsSetToComplete()
         * (2) Call AreAllSubConDateComplete()
         * (3) If above two are both true, start a sql connection and 
         *     a sql transaction
         * (4) Change the status of this job to "Complete" by
         *     calling TruckJobDAL.SetJobStatus()
         * (5) Add a LogHeader and 2 LogDetails
         * 
         */
        public bool SetJobStatus(JobStatus jobStatus, string frmName,
                                 string user, out string message)
        {
            bool retValue = false;
            message = string.Empty;


            return retValue;
        }
        // 2013-09-25

        /*
         * 2013-09-25 Zhou Kai adds this function to:
         * 
         */
        public bool DoTruckTripSubConExist()
        {
            return TruckJobDAL.DoTruckJobTripSubConExist(this.JobID);
        }
        // 2013-10-16 ends

        //20130930 - Gerry added for multiple trips
        public TruckJobTrip GetJobTripForSecondLeg(TruckJobTrip firstTrip)
        {
            TruckJobTrip retValue = new TruckJobTrip();
            try
            {
                //form the property values
                retValue.JobID = firstTrip.JobID;
                retValue.JobNo = firstTrip.JobNo;
                retValue.Sequence = this.truckJobTrips[0].Sequence + 1;
                retValue.partnerLeg = firstTrip.Sequence;
                retValue.legType = Leg_Type.SecondOfTwoLeg;
                retValue.ChargeCode = firstTrip.ChargeCode;
                retValue.billing_UOM = firstTrip.billing_UOM;
                retValue.wtVolFrDetails = firstTrip.wtVolFrDetails;
                retValue.isPackageDependent = firstTrip.isPackageDependent;
                retValue.isMultiplePackage = firstTrip.isMultiplePackage;
                retValue.StartStop = firstTrip.EndStop;
                retValue.EndStop = firstTrip.StartStop;
                retValue.StartDate = firstTrip.EndDate;
                retValue.EndDate = firstTrip.StartDate;
                //need to process time
                int startHour = Convert.ToInt32(firstTrip.EndTime.Substring(0, 2)) + 2;
                int startMin = Convert.ToInt32(firstTrip.EndTime.Substring(2, 2));
                startHour = startHour >= 24 ? startHour - 24 : startHour;
                startMin = startMin >= 60 ? 0 : startMin;
                string startTime = (startHour.ToString().Length < 2 ? "0" + startHour.ToString() : startHour.ToString())
                                    + (startMin.ToString().Length < 2 ? "0" + startMin.ToString() : startMin.ToString());

                int endHour = startHour + 2;
                endHour = endHour >= 24 ? endHour - 24 : endHour;
                string endTime = (endHour.ToString().Length < 2 ? "0" + endHour.ToString() : endHour.ToString())
                                    + (startMin.ToString().Length < 2 ? "0" + startMin.ToString() : startMin.ToString());

                retValue.StartTime = startTime;
                retValue.EndTime = endTime;

                retValue.BookedWeight = 0;
                retValue.BookedVol = 0;
                retValue.actualWeight = 0;
                retValue.actualVol = 0;
                Charge charge = Charge.GetCharge(firstTrip.ChargeCode, this.BranchCode);
                switch (charge.invoiceChargeType)
                {
                    case InvoiceChargeType.DependOnWeightVolume:
                        retValue.BillingQty = 1;
                        break;
                    case InvoiceChargeType.DependOnTruckCapacity:
                        retValue.BillingQty = 1;
                        break;
                    case InvoiceChargeType.DependOnPackagetype:
                        retValue.BillingQty = firstTrip.isMultiplePackage ? 0 : 1;
                        break;
                    case InvoiceChargeType.DependOnHigherWeightOrVolume:
                        //option is not yet ready
                        break;
                    default:
                        break;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
            return retValue;
        }

        #region "2013-10-16 Zhou Kai adds"
        public bool AreAllJobTripsComplete(out string message)
        {
            bool bBySubCon = true; // if bBySubCon == true, then this truck job is by sub-con
            if (this.subcontractorCode.Equals(String.Empty)) { bBySubCon = false; }
            if (bBySubCon) { return TruckJobDAL.AreAllJobTripsCompleteForSubConJob(JobID, out message); }
            else { return TruckJobDAL.AreAllJobTripsCompleteForOwnTransportJob(JobID, out message); }
        }

        // 2013-10-24 Zhou Kai adds
        public bool SetJobStatusToComplete(FMModule module, string formName, DateTime dateTime, string parentId, string childId, FormMode fMode, string user)
        {
            if (!TruckJobDAL.SetJobStatusToComplete(this.JobID)) { return false; }
            
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State != ConnectionState.Open) { con.Open(); };
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //DateTime systemTime = DateTime.Now;
                //LogHeader logHeader = new LogHeader(module, formName, systemTime, parentId, childId, fMode, user.ToString());
                //LogDetail logDetail = new LogDetail(LOG_DETAIL_SYSTEM_TIME, systemTime.ToString("yyyy-MM-dd HH:mm:ss"));
                //logHeader.LogDetails.Add(logDetail);
                //Logger.WriteLog(logHeader, con, tran);

                //20160811 - auditlog for job trip, only capture CargoDescription
                AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, formName, 0, "EDT");
                auditLog.WriteAuditLog("Status", "Booked", "Completed", con, tran);
                tran.Commit();



                tran.Commit();
            }
            catch (SqlException e) { tran.Rollback(); }
            finally { con.Close(); }

            return true;
        }
        // 2014-01-24 Zhou Kai ends

        public bool DoTruckJobTripSubConExist()
        {
            return TruckJobDAL.DoTruckJobTripSubConExist(JobID);
        }

        public bool DeleteAllTruckJobTripsSubCon()
        {
            // 2013-11-01 Zhou Kai adds
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            // 2013-11-01 Zhou Kai ends
            return TruckJobDAL.DeleteAllTruckJobTripsSubCon(JobID);
        }

        public bool DeleteAllTruckJobTripsSubCon(SqlConnection cn, SqlTransaction tran)
        {
            if (IsThisJobComplete()) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }
            return TruckJobDAL.DeleteAllTruckJobTripsSubCon(JobID, cn, tran);
        }

        #endregion

        // 2013-11-01 Zhou Kai adds
        public bool IsThisJobComplete() { return this.JobStatus == JobStatus.Completed; }
        public static bool EditTruckJobTripSubConRelatedData(TruckJobTrip truckJobTrip, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.EditTruckJobTripSubConRelatedData(truckJobTrip, con, tran);
        }
        // 2013-11-08 Zhou Kai ends

        // 2014-01-30 Zhou Kai adds
        public static List<string> GetAllDeletedTruckJobNumbers(Dictionary<string, string> dictCriteria)
        {
            return TruckJobDAL.GetAllDeletedTruckJobNumbers(dictCriteria);
        }

        public static List<string> GetAllTruckJobNoWhichHasDeletedJobTrips(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllTruckJobNoWhichHasDeletedJobTrips(dict);
        }

        public static List<string> GetAllTruckJobNoWhichHasDeletedJobCharges(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllTruckJobNoWhichHasDeletedJobCharges(dict);
        }

        public static List<string> GetAllTruckJobNoWhichHasDeletedJobTripDetails(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllJobNoWhichHasDeletedDetails(dict);
        }

        public static List<string> GetAllDeletedTruckJobTripIDs(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllDeletedTruckJobTripIDs(dict);
        }
        // 2014-02-03 Zhou Kai ends

        // 2014-03-17 Zhou Kai adds
        public static List<string> GetAllDeletedDetailSeqNoByJobNoAndTripSeqNo(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllDeletedDetailSeqNoByJobNoAndTripSeqNo(dict);
        }

        public static List<string> GetAllTripSeqNosWhichHasDeletedDetails(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllTripSeqNosWhichHasDeletedDetails(dict);
        }
        // 2014-03-17 Zhou Kai ends

        // 2014-03-19 Zhou Kai adds function to check if there're truck job trips under sub-con
        // Get the quantity of completed sub-con job trips under this truck job.
        // We can also use GetCompletedSubConJobTrips() instead of this method
        public int GetQtyOfCompletedSubConJobTrips()
        {
            int nCount = 0;

            foreach(TruckJobTrip tjt in this.truckJobTrips)
            {
                if (!string.IsNullOrEmpty(tjt.subCon.Code) &&
                    tjt.TripStatus == JobTripStatus.Completed)
                {
                    nCount++;
                }
            }

            return nCount;
        }
        // Get a list of completed sub-con job trips under this truck job 
        public List<TruckJobTrip> GetCompletedSubConJobTrips()
        {
            List<TruckJobTrip> lstTruckJobTrip = new List<TruckJobTrip>();
            // Get truckJobTrips again(they may be updated at planning side)
            SortableList<TruckJobTrip> updatedTruckJobTrips = this.GetTruckJobTrips();

            foreach (TruckJobTrip tjt in updatedTruckJobTrips)
            {
                if (!string.IsNullOrEmpty(tjt.subCon.Code) &&
                    tjt.TripStatus == JobTripStatus.Completed)
                {
                    lstTruckJobTrip.Add(tjt);
                }
            }

            return lstTruckJobTrip;
        }
        // 2014-03-19 Zhou Kai ends

        //20160902 - gerry added to Convert WMS GIN To TruckJob
        public static TruckJob ConvertWMSGINToTruckJob(GINHeader ginHeader)
        {
            return TruckingFacadeIn.ConvertWMSGINToTruckJob(ginHeader);
        }
        public static bool AddTruckJobs(SortableList<TruckJob> truckJobs, string formName, string userID)
        {
            string prefix = TruckJobDAL.GetPrefix();
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                ApplicationOption appOption = ApplicationOption.GetApplicationOption(ApplicationOption.BOTH_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_SET_JOBTRIP_TO_READY);
                JobTripStatus tripStatus = appOption.setting_Value == "T" ? JobTripStatus.Ready : JobTripStatus.Booked;
                con.Open();
                string customer = string.Empty;
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    foreach (TruckJob truckJob in truckJobs)
                    {
                        customer = truckJob.CustNo;
                        if (customer == string.Empty) { throw new FMException("Consignment note number "+ truckJob.SourceReferenceNo + " has no customer found."); }
                        TruckJob truckJobOut = new TruckJob();
                        truckJob.TripType = Trip_Type.OTM;
                        truckJob.JobNo = TruckJobDAL.AddTruckJobHeader(truckJob, con, tran, prefix, out truckJobOut);
                        if (!truckJob.SourceReferenceNo.Equals(string.Empty) && truckJob.truckJobTrips.Count > 0)
                        {
                            TruckJobTrip truckJobTripOut = new TruckJobTrip();
                            foreach (TruckJobTrip tempTrip in truckJob.truckJobTrips)
                            {
                                tempTrip.JobID = truckJobOut.JobID;
                                tempTrip.tripType = truckJob.TripType;

                                TruckJobDAL.AddTruckJobTrip(truckJobOut, tempTrip, con, tran, out truckJobTripOut);
                                //20160903 added 2 job trip states bec
                                //JobTripState jobTripState = new JobTripState(1, JobTripStatus.Booked, truckJobOut.JobDateTime, "", true);
                                //tempTrip.AddJobTripStateForTruck(jobTripState, con, tran);
                                JobTripState jobTripState1 = new JobTripState(2, tripStatus, truckJobOut.JobDateTime, "", true);
                                tempTrip.AddJobTripStateForTruck(jobTripState1, con, tran);
                                // 20130227 End

                                //20160811 - auditlog for job trip, only capture CargoDescription
                                AuditLog auditLog1 = new AuditLog(truckJobOut.JobNo, "TR", "BK", truckJobOut.JobID, userID, DateTime.Now, formName, tempTrip.Sequence, "ADD");
                                auditLog1.WriteAuditLog(tempTrip, null, con, tran);

                                if (tempTrip.truckJobTripDetail.Count > 0)
                                {
                                    TruckJobTripDetail outTruckJobTripDetail = new TruckJobTripDetail();
                                    foreach (TruckJobTripDetail tempTripDetail in tempTrip.truckJobTripDetail)
                                    {
                                        tempTripDetail.jobTripSequence = tempTrip.Sequence;
                                        TruckJobDAL.AddTruckJobTripDetail(tempTrip, tempTripDetail, con, tran, out outTruckJobTripDetail);
                                        //20160811 - auditlog for job trip, only capture CargoDescription
                                        AuditLog auditLog2 = new AuditLog(truckJobOut.JobNo, "TR", "BK", truckJobOut.JobID, userID, DateTime.Now, formName, tempTripDetail.detailSequence, "ADD");
                                        auditLog2.WriteAuditLog(tempTripDetail, null, con, tran);
                                    }
                                }
                            }
                        }
                        TransportFacadeOut.AddTptJobToSpecialJobDetail(truckJobOut, con, tran, prefix);
                        //update the temporary table
                        TruckJobDAL.UpdateTRKJOB_FROM_WMS_Tbl(truckJobOut.SourceReferenceNo, true, con, tran);

                        //201608 - auditlog for job header, only capture job number
                        AuditLog auditLog = new AuditLog(truckJobOut.JobNo, "TR", "BK", truckJobOut.JobID, userID, DateTime.Now, formName, 0, "ADD");
                        auditLog.WriteAuditLog("JobNumber", truckJobOut.JobNo, string.Empty, con, tran);
                    }
                    tran.Commit();
                }
                catch (FMException fmEx)
                {
                    if (tran != null) { tran.Rollback(); }
                    AuditLog auditLog = new AuditLog(customer, "TR", "BK", 0, userID, DateTime.Now, formName, 0, "ADD");
                    auditLog.WriteAuditLog("Error", fmEx.Message.ToString(), string.Empty, con, tran);
                    throw fmEx; 
                }
                catch (Exception ex)
                {
                    if (tran != null) { tran.Rollback(); }
                    AuditLog auditLog = new AuditLog(customer, "TR", "BK", 0, userID, DateTime.Now, formName, 0, "ADD");
                    auditLog.WriteAuditLog("Error", ex.Message.ToString(), string.Empty, con, tran); 
                    throw ex;
                }
                finally { if (con.State != ConnectionState.Closed) { con.Close(); } }
            }
            return true;
        }

    }
}
