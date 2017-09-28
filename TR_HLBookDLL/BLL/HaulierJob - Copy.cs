using System;
using System.Collections.Generic;
using System.Text;
using FM.FMSystem.BLL;
using FM.TransportBook.DAL;
using FM.FMSystem.DAL;
using System.Data.SqlClient;
//using FM.TransportBook.Resources;
using FM.TransportBook.Facade;
using FM.TransportMarket.BLL;
using TR_LanguageResource.Resources;
using System.Collections;
using System.Data;
using System.Globalization;
// 2014-06-18 Zhou Kai adds for using Stop class
using FM.TransportMaintenanceDLL.BLL;
using FM.SeaFreight.BLL;
using System.Linq;
using FM.Facade;
// 2014-06-18 Zhou Kai ends
using System.Diagnostics;
using System.Runtime.Serialization;

namespace FM.TransportBook.BLL
{
    public class HaulierJob : VehicleJob
    {
        //20131226, Gerry, string constants for logging   
        public const string MULTI_LEG_FORM = "TPT_HAU_E_HAULIER_MULT_2CONT";

        public const string LOGDETAIL_DATE_FROM = "From Date";
        public const string LOGDETAIL_DATE_TO = "To Date";
        public const string LOGDETAIL_START_STOP = "Start Stop";
        public const string LOGDETAIL_END_STOP = "End Stop";
        public const string LOGDETAIL_IS_MULTI_LEG = "Is Multi-Leg";
        public const string LOGDETAIL_CONTAINER_CODE = "ContainerCode";
        // 2014-04-01 Zhou Kai adds the variable below, to log quotation number
        public const string LOGDETAIL_SUBCON = "Subcon";
        public const string LOGDETAIL_QUOTATION_NO = "QuotationNo";
        public const string LOGDETAIL_CHARGECODE = "ChargeCode";
        public const string LOGDETAIL_QTY = "Qty";
        public const string LOGDETAIL_UNIT_PRICE = "Unit Price";
        public const string LOGDETAIL_DELETED_DATETIME = "Deleted DateTime";
        public const string LOGDETAIL_SEAL_NO = "Seal No";
        public const string LOGDETAIL_CONTAINER_NO = "Container No";
        public const string LOGDETAIL_LEGTYPE = "LegType";
        public const string LOGDETAIL_PARTNER_LEG = "Partner Leg";
        public const string LOGDETAIL_GROSS_WEIGHT = "Gross Weight";
        //20131226 End

        public const string HAULIER_LOCAL = "HLLO";
        public const string HAULIER_SEA_EXPORT = "HLSE";
        public const string HAULIER_SEA_IMPORT = "HLSI";

        private ChargeType haulierChargeType;
        private SortableList<HaulierJobTrip> jobTrips;

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
        // 2014-05-22 Zhou Kai adds new property
        public string subcontractorCityCode { get; set; }
        // 2014-05-22 Zhou Kai ends
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
        public string carrierAgent { get; set; }
        public string oblNo { get; set; }
        public string hblNo { get; set; }

        public string voyageNo { get; set; }
        public DateTime eta { get; set; }
        public DateTime etd { get; set; }

        #region "2014-06-18 Zhou Kai adds properties"
        // 2014-06-23 Zhou Kai adds logic to this property, because the absence of Name property
        private string shippingLine; // note it should be named as shippingLineCode, but 
        // as to be consistent with the existing BLL and database table design
        // such as POL, POD, we use shippingLine instead of shippingLineCode
        private string shippingLineName;
        public string ShippingLine
        {
            get { return shippingLine; }
            set { shippingLine = value; }
        }
        public string ShippingLineName
        {
            get { return shippingLineName; }
            set { shippingLineName = value; }
        }
        private string vesselNo;
        private string vesselName;
        public string VesselNo
        {
            get { return vesselNo; }
            set { vesselNo = value; }
        }
        public string VesselName
        {
            get { return vesselName; }
            set { vesselName = value; }
        }
        private string pol;
        private string polName;
        public string POL
        {
            get { return pol; }
            set { pol = value; }
        }
        public string PolName
        {
            get { return polName; }
            set { polName = value; }
        }
        private string pod;
        private string podName;
        public string POD
        {
            get { return pod; }
            set { pod = value; }
        }
        public string PodName
        {
            get { return podName; }
            set { podName = value; }
        }

        public string CustomerRef { get; set; }
        public string BookingRef { get; set; }
        public Stop StuffingAt { get; set; }
        public Stop UnstuffingAt { get; set; }
        public string PortNetRef { get; set; }
        public Stop Port { get; set; }
        public Stop ContainerYard { get; set; }
        private string cargoDescription;
        public string CargoDescription
        {
            get { return cargoDescription; }
            set
            {
                //if (value.Length > 120)
                //{
                //    throw new FMException(CommonResource.MaxLengthExceeded);
                //}
                cargoDescription = value;
            }
        }
        #endregion

        #region "Properties for Union Services"
        /*
         * 2014-10-04 Zhou Kai adds new properties
         * for union service changes
         */
        public string PartnerJobNo { get; set; }
        private JobTransferType jobTransferType;
        public JobTransferType JobTransferType
        {
            get { return jobTransferType; }
            set
            {
                if (value == JobTransferType.TransferIn)
                {
                    if (PartnerJobNo == String.Empty)
                    {
                        throw new FMException("Set partnerJobNo first.");
                    }
                    else { jobTransferType = value; }
                }
                else { jobTransferType = value; }

                if (value == JobTransferType.TransferOut)
                {
                    PartnerJobNo = String.Empty;
                    // PartnerJobTrips.Clear();
                    PartnerJobTrips = new SortableList<HaulierJobTrip>();
                }
            }
        }
        public SortableList<HaulierJobTrip> PartnerJobTrips { get; set; }

        #endregion

        // 2014-08-13 Zhou Kai adds 
        public static Dictionary<string, string> containerCodeSizeMapping =
                                           new Dictionary<string, string>();
        public HaulierJob()
            : base()
        {
            this.haulierChargeType = ChargeType.NotStopDependent;
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
            // 2014-05-22 Zhou Kai adds
            this.subcontractorCityCode = String.Empty;
            // 2014-05-22 Zhou Kai ends
            this.warehouseNo = "";
            this.yourRefNo = "";
            this.ucrNo = "";
            this.permitNo1 = "";
            this.datePermitNo1 = DateTime.Today;
            this.permitNo2 = "";
            this.datePermitNo2 = DateTime.Today;
            this.remarks = "";
            this.remarks2 = "";
            this.remarks3 = "";
            this.remarks4 = "";
            this.remarks5 = "";

            this.carrierAgent = "";
            this.oblNo = "";
            this.hblNo = "";
            this.vesselNo = "";
            this.voyageNo = "";
            this.pol = "";
            this.pod = "";
            this.eta = DateTime.Today;
            this.etd = DateTime.Today;
            this.jobTrips = new SortableList<HaulierJobTrip>();

            #region "2014-06-18 Zhou Kai adds"
            this.shippingLine = String.Empty;
            this.shippingLineName = String.Empty;
            this.CustomerRef = String.Empty;
            this.BookingRef = String.Empty;
            this.StuffingAt = new Stop();
            this.UnstuffingAt = new Stop();
            this.PortNetRef = String.Empty;
            this.Port = new Stop();
            this.ContainerYard = new Stop();
            this.cargoDescription = String.Empty;
            this.polName = String.Empty;
            this.podName = String.Empty;
            this.vesselNo = String.Empty;
            this.VesselName = String.Empty;
            #endregion

            GetContainerCodeSizeMapping(); // 2014-08-14 Zhou Kai adds

            // 2014-10-04 Zhou Kai adds
            this.PartnerJobNo = String.Empty;
            this.PartnerJobTrips =
                new SortableList<HaulierJobTrip>();
            this.jobTransferType = JobTransferType.None;
            // 2014-10-04 Zhou Kai ends
        }

        public HaulierJob(SortableList<HaulierJobTrip> jobTrips, int jobID, string jobNo, string custNo, string sourceRef, JobStatus jobstatus,
            string jobType, string bookingNo, DateTime bookingDate, string quotationNo, JobUrgencyType jobUrgencyType, bool billByTransport, string tptDeptCode, string oldTptDeptCode,
            string shipperCode, string shipperName, string shipperAdd1, string shipperAdd2, string shipperAdd3, string shipperAdd4,
            string consigneeCode, string consigneeName, string consigneeAdd1, string consigneeAdd2, string consigneeAdd3, string consigneeAdd4,
            string branchCode, byte[] updateVersion, bool isNew, string psaAccount,
            string subcontractorCode, string subcontractorName, string subcontractorAdd1, string subcontractorAdd2, string subcontractorAdd3, string subcontractorAdd4,
            /*2014-05-22 Zhou Kai adds*/string subcontractorCityCode,
            string warehouseNo,
            string yourRefNo, string ucrNo, string permitNo1, DateTime datePermitNo1, string permitNo2, DateTime datePermitNo2,
            string remarks, string remarks2, string remarks3, string remarks4, string remarks5,
            string carrierAgent, string oblNo, string hblNo, string vesselNo, string vesselName, string voyageNo, string pol, string polName,
            string pod, string podName, DateTime eta, DateTime etd, ChargeType haulierChargeType
            /*2014-06-18 Zhou Kai adds*/, bool isBillable, string cargoDescription,
            string shippingLine, string shippingLineName, string customerRef, string bookingRef, Stop stuffingAt,
            Stop unstuffingAt, string portNetRef, Stop port, Stop containerYard
            // 2014-10-04 Zhou Kai adds
            , string partnerJobNo,
            JobTransferType jobTransType,
            SortableList<HaulierJobTrip> partnerTrips
            // 2014-10-04 Zhou Kai ends
            )
            : base(jobID, jobNo, custNo, sourceRef, jobstatus, jobType, bookingNo, bookingDate, branchCode,
            updateVersion, isNew, quotationNo, jobUrgencyType, billByTransport, tptDeptCode, oldTptDeptCode, isBillable)
        {
            this.jobTrips = jobTrips;
            this.haulierChargeType = haulierChargeType;
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
            // 2014-05-22 Zhou Kai adds 
            this.subcontractorCityCode = subcontractorCityCode;
            // 2014-05-22 Zhou Kai ends
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
            this.polName = polName; // 2014-07-02 Zhou Kai adds
            this.pod = pod;
            this.podName = podName; // 2014-07-02 Zhou Kai adds
            this.eta = eta;
            this.etd = etd;
            this.BillByTransport = billByTransport;
            base.CustNo = custNo;
            base.BookingDate = bookingDate;
            base.JobType = jobType;

            #region "2014-06-18 Zhou Kai adds"
            this.shippingLine = shippingLine;
            this.shippingLineName = shippingLineName;
            this.CustomerRef = customerRef;
            this.BookingRef = bookingRef;
            this.StuffingAt = stuffingAt;
            this.UnstuffingAt = unstuffingAt;
            this.PortNetRef = portNetRef;
            this.Port = port;
            this.ContainerYard = containerYard;
            this.cargoDescription = cargoDescription;
            this.vesselName = vesselName;
            #endregion

            GetContainerCodeSizeMapping(); // 2014-08-14 Zhou Kai adds

            // 2014-10-04 Zhou Kai adds
            this.PartnerJobNo = partnerJobNo;
            this.JobTransferType = jobTransType;
            this.PartnerJobTrips = partnerTrips;
            // 2014-10-04 Zhou Kai ends
        }

        public SortableList<HaulierJobTrip> JobTrips
        {
            get { return jobTrips; }
            set { jobTrips = value; }
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
                        if (this.jobTrips.Count > 0)
                            throw new FMException("Customer Code cannot be change, some JobTrips exist");

                        if (GetAllHaulierJobCharges(this).Count > 0)
                            throw new FMException("Customer Code cannot be change, some JobCharges exist");
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
                        if (this.JobTrips.Count > 0)
                            throw new FMException("Job Type cannot be change, some JobTrips exist");

                        if (GetAllHaulierJobCharges(this).Count > 0)
                            throw new FMException("Job Type cannot be change, some JobCharges exist");
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

        public override DateTime BookingDate
        {
            get { return base.bookingDate; }
            set
            {
                try
                {
                    string msg = "";
                    if (value.Date != base.bookingDate.Date)
                    {
                        if (!base.QuotationNo.Equals(string.Empty))
                        {
                            string tempQuotationNo = Quotation.GetValidQuotationNo(this.CustNo, value, out msg);
                            if (msg != "")
                                throw new FMException(msg.ToString());

                            if (isQuotationValid(value, tempQuotationNo, base.custNo))
                            {
                                base.BookingDate = value;
                            }
                        }
                    }
                    base.bookingDate = value;
                }
                catch (FMException fmEx)
                {
                    value = base.bookingDate;
                    throw new FMException(fmEx.ToString());
                }
                catch (Exception ex)
                {
                    value = base.bookingDate;
                    throw new FMException(ex.ToString());
                }
            }
        }

        public ChargeType HaulierChargeType
        {
            get { return haulierChargeType; }
            set
            {
                try
                {
                    if (value != haulierChargeType)
                    {
                        if (this.JobTrips.Count > 0)
                            throw new FMException("HaulierChargeType cannot be change, some JobTrips exist, quotation might be affected. ");
                    }
                    haulierChargeType = value;
                }
                catch (FMException fmEx)
                {
                    value = haulierChargeType;
                    throw new FMException(fmEx.ToString());
                }
                catch (Exception ex)
                {
                    value = haulierChargeType;
                    throw new FMException(ex.ToString());
                }
            }
        }

        public SortableList<HaulierJobTrip> GetPartnerTrips()
        {
            return HaulierJobDAL.GetPartnerTrips(this);
        }

        public bool isQuotationValid(DateTime date, string newQuotationNo, string oldQuotationNo)
        {
            if (newQuotationNo == oldQuotationNo)
            {
                return true;
            }
            else
            {
                if (this.JobTrips.Count > 0)
                    throw new FMException("BookingDate cannot be change, some JobTrips exist, quotation might be affected. ");

                if (GetAllHaulierJobCharges(this).Count > 0)
                    throw new FMException("BookingDate cannot be change, some JobCharges exist, quotation might be affected. ");

                return false;
            }
        }

        public static SortableList<HaulierJobCharge> GetAllHaulierJobCharges(HaulierJob haulierJob)
        {
            return HaulierJobDAL.GetAllHaulierJobCharges(haulierJob);
        }

        public static SortableList<HaulierJobCharge> GetAllHaulierJobCharges(int JobID, bool isContainerMovement, SortableList<HaulierJobTrip> haulierjobTrips)
        {
            SortableList<HaulierJobCharge> tempjobcharges = new SortableList<HaulierJobCharge>();
            tempjobcharges = HaulierJobDAL.GetAllHaulierJobCharges(JobID, isContainerMovement);
            for (int i = 0; i < haulierjobTrips.Count; i++)
            {

            }
            return tempjobcharges;
        }

        public static SortableList<HaulierJobCharge> GetAllHaulierJobCharges(int JobID, bool isContainerMovement)
        {
            return HaulierJobDAL.GetAllHaulierJobCharges(JobID, isContainerMovement);
        }

        public static SortableList<HaulierJobCharge> GetAllHaulierJobChargesBasedOnType(HaulierJob haulierJob, string jobChargeType)
        {
            return HaulierJobDAL.GetAllHaulierJobChargesBasedOnType(haulierJob, jobChargeType);
        }

        public static SortableList<HaulierJob> GetAllHaulierJobs()
        {
            return HaulierJobDAL.GetAllHaulierJobs();
        }

        //Gerry Added
        public static List<string> GetAllHaulierJobNo()
        {
            return HaulierJobDAL.GetAllHaulierJobNo();
        }

        public static List<string> GetAllHaulierCustCode()
        {
            return HaulierJobDAL.GetAllHaulierCustCode();
        }

        public static HaulierJob GetHaulierJob(string JobNo)
        {
            return HaulierJobDAL.GetHaulierJob(JobNo);
        }
        //20140818 - gerry added
        public static HaulierJob GetHaulierJob(string JobNo, SqlConnection con, SqlTransaction tran)
        {
            return HaulierJobDAL.GetHaulierJob(JobNo, con, tran);
        }

        public static SortableList<HaulierJob> GetAllBookedHaulierJobsFromDatabase()
        {
            return HaulierJobDAL.GetAllBookedHaulierJobsFromDatabase();
        }

        public static SortableList<HaulierJob> GetAllBookedHaulierJobsFromObject(SortableList<HaulierJob> haulierJobs)
        {
            SortableList<HaulierJob> temphaulierJobs = new SortableList<HaulierJob>();

            for (int i = 0; i < haulierJobs.Count; i++)
            {
                if (haulierJobs[i].JobStatus == JobStatus.Booked)
                {
                    temphaulierJobs.Add(haulierJobs[i]);
                }
            }
            return temphaulierJobs;


        }

        public static SortableList<HaulierJob> GetHaulierJobs(HaulierJob startJob, HaulierJob endJob)
        {
            return HaulierJobDAL.GetHaulierJobs(startJob, endJob);
        }

        public static SortableList<HaulierJob> GetHaulierJobs(CustomerDTO startCust, CustomerDTO endCust)
        {
            return HaulierJobDAL.GetHaulierJobs(startCust, endCust);
        }

        public static SortableList<HaulierJob> GetHaulierJobs(HaulierJob startJob, HaulierJob endJob, CustomerDTO startCust, CustomerDTO endCust)
        {
            return HaulierJobDAL.GetHaulierJobs(startJob, endJob, startCust, endCust);
        }

        public static List<String> GetHaulierJobTypes()
        {
            List<String> haulierJobTypes = new List<String>();
            haulierJobTypes.Add(HAULIER_LOCAL);
            haulierJobTypes.Add(HAULIER_SEA_EXPORT);
            haulierJobTypes.Add(HAULIER_SEA_IMPORT);
            return haulierJobTypes;
        }

        public bool ValidateAddHaulierJobHeader()
        {
            /*
                        if (base.JobType != "HLLO")
                        {
                            if (shipperCode == "")
                            {
                                throw new FMException(classHaulierJobResource.ErrorMissingShipperCode);
                            }
                        }
            */
            if (base.custNo.Equals(string.Empty))
            {
                throw new FMException("Missing Customer Code. Please Fill in. ");
            }

            if (shipperCode != "")
            {
                if (shipperName == "")
                {
                    throw new FMException(TptResourceBLL.ErrMissingShipperName);
                }
                //if (shipperAdd1 == "")
                //{
                //    throw new FMException(TptResourceBLL.ErrMissingShipperAddress);
                //}
            }
            if (consigneeCode != "")
            {
                if (consigneeName == "")
                {
                    throw new FMException(TptResourceBLL.ErrMissingConsigneeName);
                }
                //if (consigneeAdd1 == "")
                //{
                //    throw new FMException(TptResourceBLL.ErrMissingConsigneeAddress);
                //}
            }
            return true;

        }

        public bool ValidateDeleteJob()
        {
            if (this.JobStatus == JobStatus.Complete)
            {
                throw new FMException(TptResourceBLL.ErrCantDelteCompletedorAssignedJob);
            }
            for (int i = 0; i < JobTrips.Count; i++)
            {
                if ((JobTrips[i].TripStatus == JobTripStatus.Assigned)
                    || (JobTrips[i].TripStatus == JobTripStatus.Completed))
                {
                    throw new FMException(TptResourceBLL.ErrCantDeleteBecauseJobTripAlreadyAssigned);
                }
            }
            if (HaulierJobDAL.IsJobBilled(this.JobNo))
            {
                throw new FMException(TptResourceBLL.ErrCantDeleteBecauseJobAlreadyBilled);
            }
            if (JobStatus == JobStatus.Cancel)
            {
                throw new FMException(TptResourceBLL.ErrCantDeleteBecauseJobAlreadyCanceled);
            }

            if (HaulierJobDAL.AreThereInvoices(JobNo) == true)
            {
                throw new FMException(TptResourceBLL.ErrCantDeleteBecauseInvoice);
            }

            SortableList<HaulierJobCharge> haulierJobCharges = new SortableList<HaulierJobCharge>();
            haulierJobCharges = HaulierJob.GetAllHaulierJobCharges(this);

            for (int i = 0; i < haulierJobCharges.Count; i++)
            {
                if ((haulierJobCharges[i].JobChargeStatus == JobChargeStatus.Completed) || (haulierJobCharges[i].JobChargeStatus == JobChargeStatus.Invoiced))
                {
                    throw new FMException(TptResourceBLL.ErrCantDeleteBecauseJobChargeAlreadyCompleteorInvoiced);
                    break;
                }
            }
            return true;

        }


        private bool ValidateEditHaulierJobHeader()
        {
            return true;
        }

        public static string GetPrefix()
        {
            return HaulierJobDAL.GetPrefix();
        }

        public string AddHaulierJobHeader(HaulierJob haulierJob, SqlConnection con,
            SqlTransaction tran, string formName, string user)
        {
            if (base.ValidateJobHeader() == true)
            {
                if (ValidateAddHaulierJobHeader() == true)
                {
                    string prefix = "";
                    prefix = HaulierJobDAL.GetPrefix();
                    try
                    {
                        this.JobNo = HaulierJobDAL.AddHaulierHeader(haulierJob, con, tran, prefix, user);
                        TransportFacadeOut.AddTptJobToSpecialJobDetail(haulierJob, con, tran, prefix);

                        //// logs
                        //DateTime serverDateTime = Logger.GetServerDateTime();
                        //LogHeader logHeader = new LogHeader(FMModule.Transport, formName,
                        //    serverDateTime, JobNo, string.Empty, FormMode.Add, user);
                        //LogDetail logDetail1 = new LogDetail(LOGDETAIL_QUOTATION_NO,
                        //    this.QuotationNo);
                        //logHeader.LogDetails.Add(logDetail1);
                        //Logger.WriteLog(logHeader, con, tran);

                        ////20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(haulierJob.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, formName, 0, FormMode.Add.ToString());
                        auditLog.WriteAuditLog("JobNumber", this.JobNo,string.Empty, con, tran);

                        return JobNo;
                    }
                    catch (FMException ex) { throw ex; }
                    catch (Exception ex) { throw new FMException(ex.ToString()); }
                }
            }

            return FMException.CLASS_VALIDATION_ERROR + "HaulierJob.addHaulierJob";
        }
        //20150807 - gerry added
        public string AddHaulierJobHeader(string formName, string user)
        {
            string strDbCon = FMGlobalSettings.TheInstance.getConnectionString();
            using (SqlConnection dbCon = new SqlConnection(strDbCon))
            {
                SqlTransaction dbTran = null;
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbTran = dbCon.BeginTransaction();
                    string jobNo = AddHaulierJobHeader(this, dbCon, dbTran, formName, user);
                    dbTran.Commit();
                    this.UpdateVersion = GetHeaderUpdateVersion();

                    return jobNo;
                }
                catch (FMException ex) { if (dbTran != null) { dbTran.Rollback(); } throw ex; }
                catch (InvalidOperationException ioe)
                {
                    if (dbTran != null) { dbTran.Rollback(); }
                    throw new FMException(ioe.Message);
                }
                catch (Exception ex)
                {
                    if (dbTran != null) { dbTran.Rollback(); }
                    throw new FMException(ex.ToString());
                }
            }
        }

        public byte[] GetHeaderUpdateVersion()
        {
            return HaulierJobDAL.GetHeaderUpdateVersion(this.JobID);
        }

        // 2014-05-21 Zhou Kai adds logic to this methods:
        // when the haulier job is sub-contracted by whole job,
        // also change the sub-contractor fields of all the haulier job trips.
        // Notice: one more parameter: subConChanges is also added.
        public bool EditHaulierJobHeader(string formName, SUBCONCHANGES subConChanges, string user)
        {
            bool temp = false;

            if ((TptDeptCode == OldTptDeptCode) | ((TptDeptCode != OldTptDeptCode) & (CanChangeTptDeptCode())))
            {

                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    HaulierJob oldHaulierJob = GetHaulierJob(this.JobNo, con, tran);
                    if (this.JobStatus == JobStatus.Complete)
                    {
                        throw new FMException(TptResourceBLL.ErrCantEditCompletedJob);
                    }

                    // 2014-05-22 Zhou Kai adds logic:
                    string tempStatus =  ApplicationOption.GetApplicationOption(ApplicationOption.HAULAGE_SETTINGS_ID, ApplicationOption.SETTINGS_FLAG_HAULIER_SUBCONTRACT_BY_JOB).setting_Value;
                    
                    bool isSubConByWholeJob = false;
                    if (tempStatus == "T") { isSubConByWholeJob = true; }
                    if (isSubConByWholeJob && (subConChanges != SUBCONCHANGES.NOCHANGES))
                    {
                        foreach (HaulierJobTrip tmp in this.jobTrips)
                        {
                            if (tmp.TripStatus > JobTripStatus.Ready)
                            {
                                throw new FMException(TptResourceBLL.CannotChangeSubConAtJobLevelWhenSubConByJobAndSomeJobTripsAssigned);
                            }
                        }
                    }
                    temp = HaulierJobDAL.EditHaulierHeader(this, isSubConByWholeJob, con, tran); // adds a bool as a flag
                    // 2014-05-22 Zhou Kai ends

                    string prefix = "";
                    prefix = HaulierJobDAL.GetPrefix();

                    TransportFacadeOut.EditTptJobToSpecialJobDetail(this, con, tran, prefix);

                    //// now form the log entry
                    //DateTime serverDateTime = Logger.GetServerDateTime();

                    //// form the header    
                    ////20131226 - Gerry replaced child identifier to empty string
                    //LogHeader logHeader = new LogHeader(FMModule.Transport, formName, serverDateTime,
                    //    JobNo, string.Empty, FormMode.Edit, user);

                    //// This is an example of create the 2 logDetail objects
                    ////20131226 - Gerry replace property to be log
                    ////LogDetail logDetail1 = new LogDetail("CustNo", CustNo);
                    ////LogDetail logDetail2 = new LogDetail("JobType", JobType);
                    //// 2014-04-01 Zhou Kai modifies the line below, to log the quotation number instead
                    //LogDetail logDetail1 = new LogDetail(LOGDETAIL_QUOTATION_NO, this.QuotationNo);

                    //// add the 2 logDetails objects to the List collection of logHeader
                    //logHeader.LogDetails.Add(logDetail1);
                    ////logHeader.LogDetails.Add(logDetail2);

                    //// now call the Logger class to write

                    //Logger.WriteLog(logHeader, con, tran);


                    //20151028 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, formName, 0, FormMode.Edit.ToString());
                    auditLog.WriteAuditLog(this, oldHaulierJob, con, tran);

                    tran.Commit();
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
                    throw new FMException(ex.Message.ToString());
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
            return temp;
        }
        //20150805 - gerry added method to be use when deleting shifting job from planning
        public bool DeleteHaulierJobFromPlanning(string formName, string user, SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            if (ValidateDeleteJob() == true)
            {
                try
                {
                    temp = HaulierJobDAL.DeleteHaulierJob(this, con, tran);
                    // 2014-10-31 Zhou Kai adds, remove the partnerJobNo information from the transferred
                    // in job
                    ErasePartnerJobNo(this.PartnerJobNo, this.JobNo, con, tran);
                    // 2014-10-31 Zhou Kai ends

                    TransportFacadeOut.DeleteTptJobToSpecialJobDetail(this, con, tran);

                    // now form the log entry
                    DateTime serverDateTime = Logger.GetServerDateTime();


                    // 2014-08-20 Zhou Kai adds
                    HaulierJob.DeleteAllContainerMovementCharges(this, con, tran);
                    
                    //for now no looging for planning
                }
                catch (FMException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new FMException(ex.ToString());
                }
            }
            return temp;
        }
        public bool DeleteHaulierJob(string formName, string user)
        {
            bool temp = false;
            if (ValidateDeleteJob() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    temp = HaulierJobDAL.DeleteHaulierJob(this, con, tran);
                    // 2014-10-31 Zhou Kai adds, remove the partnerJobNo information from the transferred
                    // in job
                    ErasePartnerJobNo(this.PartnerJobNo, this.JobNo, con, tran);
                    // 2014-10-31 Zhou Kai ends

                    TransportFacadeOut.DeleteTptJobToSpecialJobDetail(this, con, tran);

                    //// now form the log entry
                    //DateTime serverDateTime = Logger.GetServerDateTime();

                    //// form the header             
                    ////20131226 - Gerry replaced child identifier to empty string
                    //LogHeader logHeader = new LogHeader(FMModule.Transport, formName, serverDateTime,
                    //    JobNo, string.Empty, FormMode.Delete, user);

                    ////20140217 - Gerry removed logdetails for delete
                    //LogDetail logDetail = new LogDetail(LOGDETAIL_DELETED_DATETIME, serverDateTime.ToString());
                    //logHeader.LogDetails.Add(logDetail);
                    ////20140217 end  

                    //// now call the Logger class to write   
                    //Logger.WriteLog(logHeader, con, tran);

                    // 2014-08-20 Zhou Kai adds
                    HaulierJob.DeleteAllContainerMovementCharges(this, con, tran);
                    // 2014-08-20 Zhou Kai ends


                    //20151028 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, formName, 0, FormMode.Delete.ToString());
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
        //20140218 - Gerry changed the logic to check duplicate Container and Seal number
        //20140920 - Gerry added sql parameter to be in single connection
        public void CheckDoubleContainerSealNos(HaulierJobTrip haulierJobTrip, SqlConnection con, SqlTransaction tran)
        {
            #region OLD logic
            /*
                if (haulierJobTrip.ContainerNo != "")
                {
                    if (haulierJobTrip.ContainerNo == JobTrips[i].ContainerNo)
                    {
                        temp = true;
                        throw new FMException(TptResourceBLL.ErrDoubleContainerNo);
                    }
                }
                 */
            #endregion
            //20121214 - Gerry Replaced   
            if (!haulierJobTrip.ContainerNo.Equals(string.Empty) && !haulierJobTrip.ContainerNo.Equals("-"))
                HaulierJobTrip.IsContainerNoFreeToUse(haulierJobTrip.ContainerNo, haulierJobTrip.JobID, haulierJobTrip.LegGroup, con, tran);
            if (!haulierJobTrip.SealNo.Equals(string.Empty) && !haulierJobTrip.SealNo.Equals("-") && !haulierJobTrip.SealNo.Equals("NA"))
                HaulierJobTrip.IsSealNoFreeToUse(haulierJobTrip.SealNo, haulierJobTrip.JobID, haulierJobTrip.LegGroup, con, tran);

            if (!haulierJobTrip.isMulti_leg)
            {

                //20140218 - Gerry modify, loop all jobtrips if container and seal numbers
                foreach (HaulierJobTrip trip in this.jobTrips)
                {
                    if (haulierJobTrip.Sequence != trip.Sequence)
                    {
                        //Check Container No
                        if (haulierJobTrip.ContainerNo != "" && haulierJobTrip.ContainerNo == trip.ContainerNo)
                            throw new FMException("Container number " + haulierJobTrip.ContainerNo.ToUpper().ToString() + " is being used in the same job with seqNo " + haulierJobTrip.Sequence.ToString() + ". ");
                        //Check Seal No
                        else if (haulierJobTrip.SealNo != "" && haulierJobTrip.SealNo == trip.SealNo)
                            throw new FMException("Seal number " + haulierJobTrip.SealNo.ToUpper().ToString() + " is being used in the same job with seqNo " + haulierJobTrip.Sequence.ToString() + ". ");
                            //throw new FMException(TptResourceBLL.ErrDoubleSealNo);
                    }
                }
            }
            else
            {
                ArrayList contNoList = GetAllContainerNoForJob(con, tran);
                contNoList.Remove("");
                ArrayList sealNoList = GetAllSealNoForJob(con, tran);
                sealNoList.Remove("");
                switch (haulierJobTrip.LegType)
                {
                    case LegType.OneLeg:
                        if (haulierJobTrip.ContainerNo == "")
                            throw new FMException(TptResourceBLL.ErrContainerNoBlank);
                        if (haulierJobTrip.ContainerNo != "" && !contNoList.Contains(haulierJobTrip.ContainerNo.ToUpper()))
                            throw new FMException(TptResourceBLL.ErrDifferentContainerNo);
                        if (haulierJobTrip.SealNo != "" && !sealNoList.Contains(haulierJobTrip.SealNo.ToUpper()))
                            throw new FMException(TptResourceBLL.ErrDifferentSealNo);

                        break;
                    default:
                        if (haulierJobTrip.ContainerNo != haulierJobTrip.oldContainerNo || haulierJobTrip.SealNo != haulierJobTrip.oldSealNo)
                        {
                            //remove contNo and seal list for the current trip
                            foreach (JobTrip trip in this.jobTrips)
                            {
                                if (trip.JobID == haulierJobTrip.JobID && trip.Sequence == haulierJobTrip.Sequence)
                                {
                                    contNoList.Remove(haulierJobTrip.oldContainerNo);
                                    sealNoList.Remove(haulierJobTrip.oldSealNo);
                                }

                                if (contNoList.Contains(haulierJobTrip.ContainerNo.ToUpper()))
                                    throw new FMException("Container number " + haulierJobTrip.ContainerNo.ToUpper().ToString() + " is being used in the same job with seqNo " + haulierJobTrip.Sequence.ToString() + ". ");
                                //throw new FMException(string.Format(TptResourceBLL.ErrDoubleContainerNo, haulierJobTrip.ContainerNo));
                                if (sealNoList.Contains(haulierJobTrip.SealNo.ToUpper()))
                                    throw new FMException("Seal number " + haulierJobTrip.SealNo.ToUpper().ToString() + " is being used in the same job with seqNo " + haulierJobTrip.Sequence.ToString() + ". ");
                                //  throw new FMException(string.Format(TptResourceBLL.ErrDoubleSealNo, haulierJobTrip.SealNo));
                            }
                            break;
                        }
                        break;
                }
            }
        }
        public void CheckDoubleContainerSealNos(HaulierJobTrip haulierJobTrip)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //20160205 - gerry added
                if (!haulierJobTrip.ContainerNo.Equals(string.Empty) && !haulierJobTrip.ContainerNo.Equals("-"))
                    HaulierJobTrip.IsContainerNoFreeToUse(haulierJobTrip.ContainerNo, haulierJobTrip.JobID, haulierJobTrip.LegGroup, con, tran);
                if (!haulierJobTrip.SealNo.Equals(string.Empty) && !haulierJobTrip.SealNo.Equals("-") && !haulierJobTrip.SealNo.Equals("NA"))
                    HaulierJobTrip.IsSealNoFreeToUse(haulierJobTrip.SealNo, haulierJobTrip.JobID, haulierJobTrip.LegGroup, con, tran);

                if (!haulierJobTrip.isMulti_leg)
                {
                    //20140218 - Gerry modify, loop all jobtrips if container and seal numbers
                    foreach (HaulierJobTrip trip in this.jobTrips)
                    {
                        if (haulierJobTrip.Sequence != trip.Sequence)
                        {
                            //Check Container No
                            if (haulierJobTrip.ContainerNo != "" && haulierJobTrip.ContainerNo == trip.ContainerNo)
                                throw new FMException("Container number " + haulierJobTrip.ContainerNo.ToUpper().ToString() + " is being used in the same job with seqNo " + haulierJobTrip.Sequence.ToString() + ". ");
                            //throw new FMException(TptResourceBLL.ErrDoubleContainerNo);
                            //Check Seal No
                            else if (haulierJobTrip.SealNo != "" && haulierJobTrip.SealNo == trip.SealNo)
                                throw new FMException("Seal number " + haulierJobTrip.SealNo.ToUpper().ToString() + " is being used in the same job with seqNo " + haulierJobTrip.Sequence.ToString() + ". ");
                            //throw new FMException(TptResourceBLL.ErrDoubleSealNo);
                        }
                    }
                }
                else
                {
                    ArrayList contNoList = GetAllContainerNoForJob(con, tran);
                    contNoList.Remove("");
                    ArrayList sealNoList = GetAllSealNoForJob(con, tran);
                    sealNoList.Remove("");
                    switch (haulierJobTrip.LegType)
                    {
                        case LegType.OneLeg:
                            if (haulierJobTrip.ContainerNo == "")
                                throw new FMException(TptResourceBLL.ErrContainerNoBlank);
                            if (haulierJobTrip.ContainerNo != "" && !contNoList.Contains(haulierJobTrip.ContainerNo.ToUpper()))
                                throw new FMException(TptResourceBLL.ErrDifferentContainerNo);
                            if (haulierJobTrip.SealNo != "" && !sealNoList.Contains(haulierJobTrip.SealNo.ToUpper()))
                                throw new FMException(TptResourceBLL.ErrDifferentSealNo);

                            break;
                        default:
                            if (haulierJobTrip.ContainerNo != haulierJobTrip.oldContainerNo || haulierJobTrip.SealNo != haulierJobTrip.oldSealNo)
                            {
                                //remove contNo and seal list for the current trip
                                foreach (JobTrip trip in this.jobTrips)
                                {
                                    if (trip.JobID == haulierJobTrip.JobID && trip.Sequence == haulierJobTrip.Sequence)
                                    {
                                        contNoList.Remove(haulierJobTrip.oldContainerNo);
                                        sealNoList.Remove(haulierJobTrip.oldSealNo);
                                    }

                                    if (contNoList.Contains(haulierJobTrip.ContainerNo.ToUpper()))
                                        throw new FMException("Container number " + haulierJobTrip.ContainerNo.ToUpper().ToString() + " is being used in the same job with seqNo " + haulierJobTrip.Sequence.ToString() + ". ");
                                        //throw new FMException(string.Format(TptResourceBLL.ErrDoubleContainerNo, haulierJobTrip.ContainerNo));
                                    if (sealNoList.Contains(haulierJobTrip.SealNo.ToUpper()))
                                        throw new FMException("Seal number " + haulierJobTrip.SealNo.ToUpper().ToString() + " is being used in the same job with seqNo " + haulierJobTrip.Sequence.ToString() + ". ");
                                        //  throw new FMException(string.Format(TptResourceBLL.ErrDoubleSealNo, haulierJobTrip.SealNo));
                                }
                                break;
                            }
                            break;
                    }
                }
                tran.Commit();
            }
            catch (FMException e) { tran.Rollback(); throw e; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
            finally { con.Close(); }
        }

        public bool AddHaulierJobTrip(HaulierJobTrip haulierJobTrip, string frmName, string user)
        {
            bool temp = true;
            // validate business logic before calling HaulierJobDAL to save
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                //if (haulierJobTrip.LegType != LegType.SecondOfTwoLeg) //20131127 - Gerry added condition, no need to check for 2nd leg because of the SE/SI jobs
                //    CheckDoubleContainerSealNos(haulierJobTrip, con, tran);  //20140920 - Gerry added sql parameter
                if (haulierJobTrip.LegType == LegType.OneLeg)
                    CheckDoubleContainerSealNos(haulierJobTrip, con, tran); 

                if (haulierJobTrip.ValidateAddHaulierJobTrip())
                {
                    int oldTripQty = this.jobTrips.Count;

                    #region "LegGroup, LegGroupMember"
                    // 2014-10-09 Zhou Kai adds logic to
                    // assign legGroup / legGroupMember
                    if (haulierJobTrip.LegGroup == -1)
                    {
                        haulierJobTrip.LegGroup =
                            haulierJobTrip.ContainerNo == String.Empty ?
                                HaulierJobDAL.GetNextLegGroup(haulierJobTrip.JobID)
                                 :
                                HaulierJobDAL.GetNextLegGroupByContainerNo(haulierJobTrip.JobID,
                                    haulierJobTrip.ContainerNo);

                    }
                    if (haulierJobTrip.LegGroupMember == -1)
                    {
                        haulierJobTrip.LegGroupMember =
                            HaulierJobDAL.GetNextLegGroupMember(
                                                                haulierJobTrip.JobID,
                                                                haulierJobTrip.LegGroup);
                    }

                    // 2014-10-09 Zhou Kai ends
                    #endregion
                    if (HaulierJobDAL.AddHaulierJobTrip(this, haulierJobTrip, con, tran) == true)
                    {
                        // Create a JobTripState and write to DB
                        JobTripState jobTripState = new JobTripState(1, JobTripStatus.Booked, this.BookingDate, "Trip added by " + user, true);
                        haulierJobTrip.AddJobTripState(jobTripState, con, tran);

                        // 2014-08-20 Zhou Kai adds
                        if (jobTrips.Count == oldTripQty) { this.jobTrips.Add(haulierJobTrip); }
                        HaulierJob.ChangeContainerMovementCharge(this, con, tran);
                        // 2014-08-20 Zhou Kai ends

                        //// Do logging   20140216
                        //DoAddOrEditLogging(haulierJobTrip, frmName, user, FormMode.Add, con, tran);

                        ////20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(haulierJobTrip.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, haulierJobTrip.Sequence, FormMode.Add.ToString());
                        auditLog.WriteAuditLog("JobNumber", haulierJobTrip.JobNo.ToString(), string.Empty, con, tran);

                        tran.Commit();
                    }
                }
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

            return temp;
        }

        /// <summary>
        /// 2014-10-10 Zhou Kai cloned this function by adding extra parameters
        /// of database connection and transaction
        /// </summary>
        /// <param name="haulierJobTrip">The trip to be added to "this"</param>
        /// <param name="frmName">The form name from where this
        ///  function is invoked</param>
        /// <param name="user">The current login user </param>
        /// <returns>true: successed; false: failed</returns>
        /// 
        // 20141027 - gerry added 1 optional parameter to check c=the duplicate containerNo which is not needed in transfering Job
        public bool AddHaulierJobTrip(HaulierJobTrip haulierJobTrip, string frmName, string user, SqlConnection con, SqlTransaction tran, bool isTransferJob = false)
        {
            bool temp = true;
            try
            {
                // 20141027 - gerry modified to check container only if not transferred job
                if (haulierJobTrip.LegType == LegType.OneLeg && !isTransferJob)
                    CheckDoubleContainerSealNos(haulierJobTrip, con, tran);

                if (haulierJobTrip.ValidateAddHaulierJobTrip(isTransferJob))
                {
                    int oldTripQty = this.jobTrips.Count;

                    #region "LegGroup, LegGroupMember"
                    // 2014-10-09 Zhou Kai adds logic to
                    // assign legGroup / legGroupMember
                    if (haulierJobTrip.LegGroup == -1)
                    {
                        haulierJobTrip.LegGroup = HaulierJobDAL.GetNextLegGroup(haulierJobTrip.JobID, con, tran);

                    }
                    if (haulierJobTrip.LegGroupMember == -1)
                    {
                        haulierJobTrip.LegGroupMember = HaulierJobDAL.GetNextLegGroupMember(
                                                                                                                          haulierJobTrip.JobID,
                                                                                                                          haulierJobTrip.LegGroup,
                                                                                                                          con,
                                                                                                                          tran);
                    }

                    // 2014-10-09 Zhou Kai ends
                    #endregion
                    if (HaulierJobDAL.AddHaulierJobTrip(this, haulierJobTrip, con, tran) == true)
                    {
                        // Create a JobTripState and write to DB
                        JobTripState jobTripState = new JobTripState(1, JobTripStatus.Booked, this.BookingDate, "Trip added by " + user, true);
                        haulierJobTrip.AddJobTripState(jobTripState, con, tran);

                        // 2014-08-20 Zhou Kai adds
                        if (jobTrips.Count == oldTripQty) { this.jobTrips.Add(haulierJobTrip); }
                        HaulierJob.ChangeContainerMovementCharge(this, con, tran);
                        // 2014-08-20 Zhou Kai ends

                        //// Do logging   20140216
                        //DoAddOrEditLogging(haulierJobTrip, frmName, user, FormMode.Add, dbCon, dbTran);

                        ////20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(haulierJobTrip.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, haulierJobTrip.Sequence, FormMode.Add.ToString());
                        auditLog.WriteAuditLog("JobNumber", haulierJobTrip.JobNo.ToString(), string.Empty, con, tran);
                    }
                }
            }
            catch (FMException ex)
            {
                //dbTran.Rollback(); 
                throw ex;
            }
            catch (Exception ex)
            {
                //dbTran.Rollback();
                throw new FMException(ex.ToString());
            }

            return temp;
        }

        //20140216 - Gerry added 2 parameters sqlconnection and sql transaction
        public bool EditHaulierJobTrip(HaulierJobTrip haulierJobTrip, string entryFormPrefix, string frmName, string user)
        {
            bool temp = false;
            HaulierJobTrip oldHaulierJobTrip = HaulierJobTrip.GetHaulierJobTrip(haulierJobTrip.JobID, haulierJobTrip.Sequence);
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (haulierJobTrip.LegGroupMember == 1 && haulierJobTrip.PartnerJobNo.Equals(string.Empty)) // can only edit first leg
                    CheckDoubleContainerSealNos(haulierJobTrip, con, tran);  //20140920 - Gerry added sql parameter
                if (haulierJobTrip.ValidateEditHaulierJobTrip())
                {
                    temp = HaulierJobDAL.EditHaulierJobTrip(haulierJobTrip, con, tran);
                    if (haulierJobTrip.JobTripStates[haulierJobTrip.JobTripStates.Count - 1].Status != haulierJobTrip.TripStatus)
                    {
                        haulierJobTrip.UpdateVersion = HaulierJobDAL.GetHaulierJobTripUpdateVersion(haulierJobTrip, con, tran);
                        JobTripState jobTripState = new JobTripState(1, haulierJobTrip.TripStatus, this.BookingDate, " new state" + user, true);
                        haulierJobTrip.AddJobTripState(jobTripState, con, tran);
                    }
                    //20140110 - gerry added 1 condition to only update 2nd leg if not from multi leg container form
                    if (frmName != MULTI_LEG_FORM)
                    {
                        //20150909 - gerry added to set the start of the next leg
                        haulierJobTrip.UpdateNextLegStartStopAndStatus(true,entryFormPrefix, user, frmName, con, tran);
                        haulierJobTrip.UpdatePrevLegEndStop(entryFormPrefix, user, frmName,con, tran);

                        //this condition is for case where containerNo and SealNo were updated from first leg
                        if (haulierJobTrip.LegType == LegType.FirstOfTwoLeg)
                        {   //gerry modified if old containerNo is blank or not
                            if (haulierJobTrip.oldContainerNo == string.Empty)
                                haulierJobTrip.UpdateContainerAndSealNoForSecondLeg(entryFormPrefix, user, frmName, con, tran);
                            else
                                haulierJobTrip.UpdateContainerSealNoForRemainingLeg(entryFormPrefix, user, frmName, con, tran);
                        }
                        // 2014-08-27 Zhou Kai adds, to update the IsBillable, ContainerCode of the partner 
                        // trip of the current trip
                        haulierJobTrip.UpdateBillableConCodeForPartnerTrip(frmName, con, tran);
                        this.jobTrips.First(x => x.Sequence == haulierJobTrip.Sequence).isBillable =
                            haulierJobTrip.isBillable;
                        // when there is partner leg, update the isbillable of the partner leg also
                        if (haulierJobTrip.PartnerLeg != 0)
                        {
                            this.jobTrips.First(x => x.Sequence == haulierJobTrip.PartnerLeg).isBillable =
                                                       haulierJobTrip.isBillable;
                        }

                        // 2014-08-27 Zhou Kai ends

                        // 2014-08-20 Zhou Kai adds, the calculating of charges has to be 
                        // put here before the logging, because the logging writting will
                        // update the tpt_special_data_tbl, which the calculating need to 
                        // depend on.
                        HaulierJob.ChangeContainerMovementCharge(this, con, tran);
                        // 2014-08-20 Zhou Kai ends

                        //// Do logging    20140216
                        //DoAddOrEditLogging(haulierJobTrip, frmName, user, FormMode.Edit, con, tran);


                        //20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(haulierJobTrip.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, haulierJobTrip.Sequence, FormMode.Edit.ToString());
                        auditLog.WriteAuditLog(haulierJobTrip, oldHaulierJobTrip, con, tran);
                    }

                    tran.Commit();
                }
            }
            catch (FMException e) { tran.Rollback(); throw e; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
            finally { con.Close(); }

            return temp;
        }
        #region OLD logging removed
        ////20140314 - Gerry add 1 more parameter formMode, either add or edit mode
        //private void DoAddOrEditLogging(HaulierJobTrip haulierJobTrip, string frmName, string user, FormMode frmMode, SqlConnection con, SqlTransaction tran)
        //{
        //    try
        //    {// now form the log entry
        //        DateTime serverDateTime = Logger.GetServerDateTime();

        //        // form the header
        //        // 2014-02-01 Zhou Kai modifies from jobNo to haulierJobTrip.Sequence.ToString()
        //        LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
        //            JobNo, haulierJobTrip.Sequence.ToString(), frmMode, user);

        //        // This is an example of create the 2 logDetail objects
        //        //20131226 - Gerry replaced properties to be log
        //        //LogDetail logDetail2 = new LogDetail("GrossWeight", haulierJobTrip.GrossWeight.ToString());
        //        //LogDetail logDetail3 = new LogDetail("GrossCBM", haulierJobTrip.GrossCBM.ToString());
        //        LogDetail logDetail1 = new LogDetail(LOGDETAIL_CONTAINER_CODE, haulierJobTrip.ContainerCode);
        //        LogDetail logDetail2 = new LogDetail(LOGDETAIL_START_STOP, haulierJobTrip.StartStop.Code.ToString());
        //        LogDetail logDetail3 = new LogDetail(LOGDETAIL_END_STOP, haulierJobTrip.EndStop.Code.ToString());
        //        // 2014-01-02 Zhou Kai modifies from haulierJobTrip.StartDate.ToShortDateString() to ToString("yyyy-MM-dd HH:mm:ss")
        //        LogDetail logDetail4 = new LogDetail(LOGDETAIL_DATE_FROM, haulierJobTrip.StartDate.ToShortDateString());
        //        LogDetail logDetail5 = new LogDetail(LOGDETAIL_DATE_TO, haulierJobTrip.EndDate.ToShortDateString());
        //        LogDetail logDetail6 = new LogDetail(LOGDETAIL_IS_MULTI_LEG, haulierJobTrip.isMulti_leg ? "Yes" : "No");
        //        //20140216 -gerry added
        //        LogDetail logDetail7 = new LogDetail(LOGDETAIL_CONTAINER_NO, haulierJobTrip.ContainerNo);
        //        LogDetail logDetail8 = new LogDetail(LOGDETAIL_SEAL_NO, haulierJobTrip.SealNo);
        //        LogDetail logDetail9 = new LogDetail(LOGDETAIL_LEGTYPE, haulierJobTrip.LegType.ToString());
        //        LogDetail logDetail10 = new LogDetail(LOGDETAIL_PARTNER_LEG, haulierJobTrip.PartnerLeg.ToString());
        //        LogDetail logDetail11 = new LogDetail(LOGDETAIL_GROSS_WEIGHT, haulierJobTrip.GrossWeight.ToString());
        //        // add the 2 logDetails objects to the List collection of logHeader  
        //        logHeader.LogDetails.Add(logDetail1);
        //        logHeader.LogDetails.Add(logDetail2);
        //        logHeader.LogDetails.Add(logDetail3);
        //        logHeader.LogDetails.Add(logDetail4);
        //        logHeader.LogDetails.Add(logDetail5);
        //        logHeader.LogDetails.Add(logDetail6);
        //        logHeader.LogDetails.Add(logDetail7);
        //        logHeader.LogDetails.Add(logDetail8);
        //        logHeader.LogDetails.Add(logDetail9);
        //        logHeader.LogDetails.Add(logDetail10);
        //        logHeader.LogDetails.Add(logDetail11);

        //        Logger.WriteLog(logHeader, con, tran);
        //    }
        //    catch (FMException e) { throw e; }
        //    catch (Exception ex) { throw new FMException(ex.ToString()); }
        //}
        #endregion


        public bool DeleteHaulierJobTrips(List<HaulierJobTrip> haulierJobTrips, string frmName, string user)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                foreach (HaulierJobTrip trip in haulierJobTrips)
                {
                    if (trip.ValidateDeleteHaulierJobTrip())
                    {
                        if (!ValidateDeletingHaulierJobTrip(trip, trip.LegType == LegType.OneLeg ? 1 : 2, quotationNo))
                        {
                            throw new FMException(TptResourceBLL.ErrNo1LegRate);
                        }
                        for (int i = 0; i < trip.JobTripStates.Count; i++)
                            trip.DeleteJobTripStateUpdateHaulierJobTripStatus(trip.JobTripStates[i], con, tran);

                        //for more than 2 legs dont allow to delete the first or 2nd legs
                        int legCount = this.JobTrips.Where(jt => jt.LegGroup == trip.LegGroup).ToList().Count;
                        if (legCount > 2)
                        {
                            if (trip.LegGroupMember <= 2) { throw new FMException("For more than two legs job, you only allow to delete the after 2nd leg. "); }
                        }
                        //20140828 - gerry added to update trip with the same leg group
                        if (trip.isMulti_leg && trip.LegGroupMember <= 2) // && !trip.ContainerNo.Equals(string.Empty))
                        {
                            var tripListSameLegGroup = this.jobTrips.Where(jt => jt.LegGroup == trip.LegGroup);
                            foreach (HaulierJobTrip tempJobTrip in tripListSameLegGroup)
                            {
                                HaulierJobTrip clonedTrip = HaulierJobDAL.GetHaulierJobTrip(tempJobTrip.JobID, tempJobTrip.Sequence, con, tran);
                                if (tempJobTrip.TripStatus != JobTripStatus.Completed && tempJobTrip.Sequence != trip.Sequence && tempJobTrip.LegGroupMember <= 2)
                                {
                                    //tempJobTrip.ContainerNo = string.Empty;
                                    //tempJobTrip.SealNo = string.Empty;
                                    tempJobTrip.isMulti_leg = false;
                                    tempJobTrip.LegType = LegType.OneLeg;
                                    tempJobTrip.PartnerLeg = 0;
                                    //Do edit
                                    HaulierJobDAL.EditHaulierJobTrip(tempJobTrip, con, tran);
                                    //20151028 - gerry added new audit log
                                    AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, tempJobTrip.Sequence, FormMode.Edit.ToString());
                                    auditLog.WriteAuditLog(tempJobTrip, clonedTrip, con, tran);
                                }
                            }
                        }
                        //20140216 
                        if (HaulierJobDAL.DeleteHaulierJobTrip(trip, con, tran))
                        {
                            // 2014-08-20 Zhou Kai adds
                            this.jobTrips.Remove(trip);
                            HaulierJob.ChangeContainerMovementCharge(this, con, tran);
                            // 2014-08-20 Zhou Kai ends
                            //20151028 - gerry added new audit log
                            AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, trip.Sequence, FormMode.Edit.ToString());
                            auditLog.WriteAuditLog(trip, null, con, tran);

                        }
                        //20160216 - update the partner job trip
                        if (!trip.PartnerJobNo.Equals(string.Empty))
                        {
                            HaulierJob outJob = HaulierJob.GetHaulierJob(trip.PartnerJobNo, con, tran);
                            List<string> contNoList = new List<string>();
                            contNoList.Add(trip.ContainerNo);
                            HaulierJob.UpdatePartnerJobNos(outJob.JobID, contNoList, string.Empty, con, tran);
                        }
                    }
                }

                tran.Commit();
                retValue = true;
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
            finally { con.Close(); }
            return retValue;
        }
        public bool EditHaulierJobTrips(List<HaulierJobTrip> haulierJobTrips, string frmName, string user)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                foreach (HaulierJobTrip trip in haulierJobTrips)
                {
                    HaulierJobTrip oldJobTrip = HaulierJobDAL.GetHaulierJobTrip(trip.JobID, trip.Sequence, con, tran);
                    if (trip.PartnerJobNo.Equals(string.Empty))
                        CheckDoubleContainerSealNos(trip, con, tran);  //20140920 - Gerry added sql parameter
                    if (trip.ValidateEditHaulierJobTrip())
                    {
                        temp = HaulierJobDAL.EditHaulierJobTrip(trip, con, tran);
                        if (trip.JobTripStates[trip.JobTripStates.Count - 1].Status != trip.TripStatus)
                        {
                            trip.UpdateVersion = HaulierJobDAL.GetHaulierJobTripUpdateVersion(trip, con, tran);
                            // Create a JobTripState and write to DB
                            JobTripState jobTripState = new JobTripState(1, trip.TripStatus, this.BookingDate, " new state" + user, true);
                            trip.AddJobTripState(jobTripState, con, tran);
                        }
                        //DoAddOrEditLogging(trip, frmName, user, FormMode.Edit, con, tran);

                        //20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(trip.JobNo, "TR", "BK", trip.JobID, user, DateTime.Now, frmName, trip.Sequence, FormMode.Edit.ToString());
                        auditLog.WriteAuditLog(trip, oldJobTrip, con, tran);
                    }
                }
                tran.Commit();
            }
            catch (FMException e) { tran.Rollback(); throw e; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
            finally { con.Close(); }

            return temp;
        }


        /// <summary>
        /// 2014-09-09 Zhou Kai adds function to:
        /// Verify if a trip can be deleted. 
        /// Scenario: If a leg of a 2-leg trip is deleted, the other leg will become a 1-leg trip,
        /// so if the job is under quotation, we need to check if there is rate for 1-leg trip.
        /// If not, we can't delete it.If yes, the 1-leg left will be billed under the rate for 1-leg trip.
        /// </summary>
        /// <param name="tripToDelete">The trip going to be deleted</param>
        /// <param name="noOfLegs">If noOfLegs = 2, it's a leg of a 2-leg trips, if noOfLegs = 1, it's a 1-leg trip</param>
        /// <param name="quotationNo">The quotation this job trip is booked under</param>
        /// <returns>true, the trip can be deleted; false, the trip cannot be deleted</returns>
        public bool ValidateDeletingHaulierJobTrip(HaulierJobTrip tripToDelete, int noOfLegs, string quotationNo)
        {
            if (quotationNo.Equals(String.Empty) || noOfLegs < 2) { return true; }

            Quotation quotation = Quotation.GetAllQuotationHeader(quotationNo);
            string uom = TransportFacadeIn.GetContainerSizeByContainerCode(tripToDelete.ContainerCode).ToUpper();
            try
            {
                return quotation.TransportRates.First(x => x.NoOfLeg == 1 && x.UOM.ToUpper().Equals(uom)) != null;
            }
            catch (System.ArgumentNullException) { return false; }
            catch (System.InvalidOperationException) { return false; }

        }

        public bool DeleteHaulierJobTrip(HaulierJobTrip haulierJobTrip, string frmName, string user)
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (haulierJobTrip.ValidateDeleteHaulierJobTrip())
                {
                    // 2014-09-09 Zhou Kai adds another checking:
                    HaulierJob haulierJob = HaulierJob.GetHaulierJob(haulierJobTrip.JobNo);
                    string quotationNo = Quotation.GetAllQuotationHeader(haulierJob.quotationNo).QuotationNo;
                    if (!ValidateDeletingHaulierJobTrip(haulierJobTrip, haulierJobTrip.LegType == LegType.OneLeg ? 1 : 2, quotationNo))
                    {
                        throw new FMException(TptResourceBLL.ErrNo1LegRate);
                    }
                    //for more than 2 legs dont allow to delete the first or 2nd legs
                    int legCount = this.JobTrips.Where(jt => jt.LegGroup == haulierJobTrip.LegGroup).ToList().Count;
                    if (legCount > 2)
                    {
                        if (haulierJobTrip.LegGroupMember <= 2) { throw new FMException("For more than two legs job, you only allow to delete the after 2nd leg. "); }
                    }

                    for (int i = 0; i < haulierJobTrip.JobTripStates.Count; i++)
                        haulierJobTrip.DeleteJobTripStateUpdateHaulierJobTripStatus(haulierJobTrip.JobTripStates[i], con, tran);

                    //20140216 - gerry added to update trip with the same container No
                    //20140828 - gerry added to update trip with the same leg group
                    if (haulierJobTrip.isMulti_leg && haulierJobTrip.LegGroupMember <= 2) // && !trip.ContainerNo.Equals(string.Empty))
                    {
                        var tripListSameLegGroup = this.jobTrips.Where(jt => jt.LegGroup == haulierJobTrip.LegGroup);
                        foreach (HaulierJobTrip tempJobTrip in tripListSameLegGroup)
                        {
                            HaulierJobTrip clonedTrip = HaulierJobDAL.GetHaulierJobTrip(tempJobTrip.JobID, tempJobTrip.Sequence, con, tran);
                            if (tempJobTrip.TripStatus != JobTripStatus.Completed && tempJobTrip.Sequence != haulierJobTrip.Sequence && tempJobTrip.LegGroupMember <= 2)
                            {
                                //tempJobTrip.ContainerNo = string.Empty;
                                //tempJobTrip.SealNo = string.Empty;
                                tempJobTrip.isMulti_leg = false;
                                tempJobTrip.LegType = LegType.OneLeg;
                                tempJobTrip.PartnerLeg = 0;
                                //Do edit
                                HaulierJobDAL.EditHaulierJobTrip(tempJobTrip, con, tran);
                                //20151028 - gerry added new audit log
                                AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, tempJobTrip.Sequence, FormMode.Edit.ToString());
                                auditLog.WriteAuditLog(tempJobTrip, clonedTrip, con, tran);
                            }
                        }
                    }
                    //20140216 
                    if (HaulierJobDAL.DeleteHaulierJobTrip(haulierJobTrip, con, tran))
                    {
                        // 2014-08-20 Zhou Kai adds
                        this.jobTrips.Remove(haulierJobTrip);
                        HaulierJob.ChangeContainerMovementCharge(this, con, tran);
                        // 2014-08-20 Zhou Kai ends
                        //20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, haulierJobTrip.Sequence, FormMode.Edit.ToString());
                        auditLog.WriteAuditLog(haulierJobTrip, null, con, tran);

                        tran.Commit();
                        retValue = true;
                    }
                    //20160216 - update the partner job trip
                    if (!haulierJobTrip.PartnerJobNo.Equals(string.Empty))
                    {
                        HaulierJob outJob = HaulierJob.GetHaulierJob(haulierJobTrip.PartnerJobNo, con, tran);
                        List<string> contNoList = new List<string>();
                        contNoList.Add(haulierJobTrip.ContainerNo);
                        HaulierJob.UpdatePartnerJobNos(outJob.JobID, contNoList, string.Empty, con, tran);
                    }
                }
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
            finally { con.Close(); }
            return retValue;
        }
       
        public bool AddHaulierJobCharges(HaulierJobCharge haulierJobCharge, string frmName, string user)
        {
            //if (haulierJobCharge.ValidateAddJobCharge() == true)
            if (haulierJobCharge.ValidateAddHaulierJobCharge(this, haulierJobCharge))
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    if (HaulierJobDAL.AddHaulierJobCharges(haulierJobCharge, con, tran) == true)
                    {
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Booked;
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Completed;
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Invoiced;


                        //// now form the log entry
                        //DateTime serverDateTime = Logger.GetServerDateTime();

                        //// form the header              
                        ////20131226 - Gerry replaced parent identifier to JobNo and child identifier to sequence no
                        //LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
                        //    this.JobNo.ToString(), haulierJobCharge.SequenceNo.ToString(), FormMode.Add, user);
                        ////20131226 - gerry added properties to be log
                        //LogDetail logDetail1 = new LogDetail(LOGDETAIL_CHARGECODE, haulierJobCharge.ChargeCode);
                        //LogDetail logDetail2 = new LogDetail(LOGDETAIL_QTY, haulierJobCharge.Quantity.ToString());
                        //LogDetail logDetail3 = new LogDetail(LOGDETAIL_UNIT_PRICE, haulierJobCharge.UnitRateFC.ToString());

                        //logHeader.LogDetails.Add(logDetail1);
                        //logHeader.LogDetails.Add(logDetail2);
                        //logHeader.LogDetails.Add(logDetail3);
                        ////20131226 end
                        //// now call the Logger class to write

                        //Logger.WriteLog(logHeader, con, tran);


                        ////20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(haulierJobCharge.ChargeCode, "TR", "BK", this.JobID, user, DateTime.Now, frmName, haulierJobCharge.SequenceNo, FormMode.Add.ToString());
                        auditLog.WriteAuditLog("JobID", haulierJobCharge.JobID.ToString(), string.Empty, con, tran);

                        tran.Commit();
                        return true;
                    }
                    else return false;
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
            else return false;
        }

        /// <summary>
        /// 2014-08-14 Zhou Kai adds a static version of this function
        /// </summary>
        /// <returns></returns>
        public static bool AddHaulierJobCharges(HaulierJob haulierJob, HaulierJobCharge haulierJobCharge,
            string frmName, string user)
        {
            //if (haulierJobCharge.ValidateAddJobCharge() == true)
            if (haulierJobCharge.ValidateAddHaulierJobCharge(haulierJob, haulierJobCharge))
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    if (HaulierJobDAL.AddHaulierJobCharges(haulierJobCharge, con, tran) == true)
                    {
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Booked;
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Completed;
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Invoiced;

                        #region OLD Logging removed
                        //// now form the log entry
                        //DateTime serverDateTime = Logger.GetServerDateTime();

                        //// form the header              
                        ////20131226 - Gerry replaced parent identifier to JobNo and child identifier to sequence no
                        //LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
                        //    haulierJob.JobNo.ToString(), haulierJobCharge.SequenceNo.ToString(), FormMode.Add, user);
                        ////20131226 - gerry added properties to be log
                        //LogDetail logDetail1 = new LogDetail(LOGDETAIL_CHARGECODE, haulierJobCharge.ChargeCode);
                        //LogDetail logDetail2 = new LogDetail(LOGDETAIL_QTY, haulierJobCharge.Quantity.ToString());
                        //LogDetail logDetail3 = new LogDetail(LOGDETAIL_UNIT_PRICE, haulierJobCharge.UnitRateFC.ToString());

                        //logHeader.LogDetails.Add(logDetail1);
                        //logHeader.LogDetails.Add(logDetail2);
                        //logHeader.LogDetails.Add(logDetail3);
                        ////20131226 end
                        //// now call the Logger class to write
                        //Logger.WriteLog(logHeader, con, tran);
                        #endregion

                        ////20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(haulierJob.JobNo, "TR", "BK", haulierJob.JobID, user, DateTime.Now, frmName, haulierJobCharge.SequenceNo, FormMode.Add.ToString());
                        auditLog.WriteAuditLog("JobID", haulierJobCharge.JobID.ToString(), string.Empty, con, tran);

                        tran.Commit();
                        return true;
                    }
                    else return false;
                }
                catch (FMException ex) { tran.Rollback(); throw ex; }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
                finally { con.Close(); }
            }
            else return false;
        }

        public bool EditHaulierJobCharges(HaulierJobCharge haulierJobCharge, string frmName, string user)
        {
            if (haulierJobCharge.ValidateEditJobCharge() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    //HaulierJobCharge oldHaulierJobTrip = gethaulier
                    if (HaulierJobDAL.EditHaulierJobCharges(haulierJobCharge, con, tran) == true)
                    {
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Booked;
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Completed;
                        if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                            haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Invoiced;

                        #region OLD Logging removed
                        //// now form the log entry
                        //DateTime serverDateTime = Logger.GetServerDateTime();

                        //// form the header   
                        ////20131226 - Gerry replaced parent identifier to JobNo and child identifier to sequence no
                        //LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
                        //    this.JobNo.ToString(), haulierJobCharge.SequenceNo.ToString(), FormMode.Edit, user);

                        ////20131226 - gerry added properties to be log
                        //LogDetail logDetail1 = new LogDetail(LOGDETAIL_CHARGECODE, haulierJobCharge.ChargeCode);
                        //LogDetail logDetail2 = new LogDetail(LOGDETAIL_QTY, haulierJobCharge.Quantity.ToString());
                        //LogDetail logDetail3 = new LogDetail(LOGDETAIL_UNIT_PRICE, haulierJobCharge.UnitRateFC.ToString());

                        //logHeader.LogDetails.Add(logDetail1);
                        //logHeader.LogDetails.Add(logDetail2);
                        //logHeader.LogDetails.Add(logDetail3);
                        ////20131226 end

                        //// now call the Logger class to write

                        //Logger.WriteLog(logHeader, con, tran);
                        #endregion

                        //20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, haulierJobCharge.SequenceNo, FormMode.Edit.ToString());
                        auditLog.WriteAuditLog(haulierJobCharge, null, con, tran);

                        tran.Commit();
                        return true;
                    }
                    else return false;
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
            else return false;
        }

        public static bool EditHaulierJobCharges(HaulierJobCharge haulierJobCharge,
            string frmName, string user, SqlConnection con, SqlTransaction tran)
        {
            if (haulierJobCharge.ValidateEditJobCharge() == true)
            {
                try
                {
                    HaulierJobDAL.EditHaulierJobCharges(haulierJobCharge, con, tran);
                    if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                        haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Booked;
                    if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                        haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Completed;
                    if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                        haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Invoiced;

                    return true;
                }
                catch (FMException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new FMException(ex.ToString());
                }
            }
            else return false;
        }


        public static bool ChangeJobStatusFromInvoice(HaulierJobCharge haulierJobCharge,
            string frmName, string user, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                haulierJobCharge.JobChargeStatus = JobChargeStatus.Booked;
                HaulierJobDAL.EditHaulierJobCharges(haulierJobCharge, con, tran);
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Booked)
                    haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Booked;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Completed)
                    haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Completed;
                if (haulierJobCharge.JobChargeStatus == JobChargeStatus.Invoiced)
                    haulierJobCharge.OldJobChargeStatus = JobChargeStatus.Invoiced;

                return true;
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
        }



        public bool DeleteHaulierJobCharges(HaulierJobCharge haulierJobCharge, string frmName, string user)
        {
            if (haulierJobCharge.ValidateDeleteJobCharge() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    if (HaulierJobDAL.DeleteHaulierJobCharges(haulierJobCharge, con, tran) == true)
                    {
                        #region OLD logging removed
                        //// now form the log entry
                        //DateTime serverDateTime = Logger.GetServerDateTime();

                        //// form the header      
                        ////20131226 - Gerry replaced parent identifier to JobNo and child identifier to sequence no
                        //LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
                        //    this.JobNo.ToString(), haulierJobCharge.SequenceNo.ToString(), FormMode.Delete, user);
                        ////20131226 - Gerry removed logdetails for delete
                        //LogDetail logDetail = new LogDetail(LOGDETAIL_DELETED_DATETIME, serverDateTime.ToString());
                        //logHeader.LogDetails.Add(logDetail);
                        ////20131226 end  

                        //// now call the Logger class to write    
                        //Logger.WriteLog(logHeader, con, tran);
                        #endregion

                        //20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, frmName, haulierJobCharge.SequenceNo, FormMode.Delete.ToString());
                        auditLog.WriteAuditLog(this, null, con, tran);

                        tran.Commit();
                        return true;
                    }
                    else return false;
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
            else return false;
        }

        public bool AreAllJobTripsInBookedState()
        {
            bool temp = true;
            for (int i = 0; i < JobTrips.Count; i++)
            {
                if (JobTrips[i].TripStatus != JobTripStatus.Booked)
                {
                    temp = false;
                    break;
                }
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
        public static SortableList<HaulierJob> GetHaulierJobs(int rowCount)
        {
            return HaulierJobDAL.GetHaulierJobs(rowCount);
        }
        public static SortableList<HaulierJob> GetHaulierJobs(string jobNoFrom, string jobNoTo, string custCodeFrom, string custCodeTo, string containerNo, string sourceRefNo, int rowCount)
        {
            return HaulierJobDAL.GetHaulierJobs(jobNoFrom, jobNoTo, custCodeFrom, custCodeTo, containerNo,sourceRefNo, rowCount);
        }
        public static SortableList<HaulierJob> GetHaulierJobs(string jobNo, string custCode, string custRefNo, string bookRefNo, string sourceRefNo, int rowCount, string containerNo,string shipperCode, string consigneeCode)
        {
            return HaulierJobDAL.GetHaulierJobs(jobNo, custCode, custRefNo, bookRefNo, sourceRefNo, rowCount, containerNo, shipperCode, consigneeCode);
        }

        public bool CanEditJobType()
        {
            bool temp = true;
            if (jobTrips.Count > 0 || GetAllHaulierJobCharges(this).Count > 0)
            {
                temp = false;
            }
            return temp;
        }

        public bool CanEditCustCode()
        {
            bool temp = true;
            if (jobTrips.Count > 0 || GetAllHaulierJobCharges(this).Count > 0)
            {
                temp = false;
            }
            return temp;
        }

        public bool CanEditDept()
        {
            bool temp = true;
            if (jobTrips.Count > 0 || GetAllHaulierJobCharges(this).Count > 0)
            {
                temp = false;
            }
            return temp;
        }
        public bool CanEditSubContractor()
        {
            bool temp = true;
            if (jobTrips.Count > 0)
            {
                temp = false;
            }
            return temp;
        }
        //20140731 - gerry added to get container codes
        public ArrayList GetAllContainerCodesForJob()
        {
            return HaulierJobDAL.GetAllContainerCodesForJob(this.JobID);
        }
        //20140920 - gerry added sql parameter, to be in a single connection with the calling method
        public ArrayList GetAllContainerNoForJob(SqlConnection con, SqlTransaction tran)
        {
            return HaulierJobDAL.GetAllContainerNoForJob(this.JobID, con, tran);
        }

        public ArrayList GetAllContainerNoForJob()
        {
            return HaulierJobDAL.GetAllContainerNoForJob(this.JobID);
        }
        //20140920 - gerry added sql parameter, to be in a single connection with the calling method
        public ArrayList GetAllSealNoForJob(SqlConnection con, SqlTransaction tran)
        {
            return HaulierJobDAL.GetAllSealNoForJob(this.JobID, con, tran);
        }
        //20140805 - gerry change to instance method instead of static
        public bool UpdatehaulierJobFromPlanning(SqlConnection con, SqlTransaction tran)
        {
            return HaulierJobDAL.UpdateHaulierJobFromPlanning(this, con, tran);
        }

        //20131226 - gerry added parameters for logging
        public bool SetHaulierJobToComplete(string formName, string user)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                string outMsg = "";
                if (IsHaulierCanSetToComplete(out outMsg))
                {
                    string remarkForCompleted = string.Format(CommonResource.ComplationDateRemark, DateTime.Today.Date.ToString(CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern));
                    this.remarks5 = remarkForCompleted;
                    HaulierJobDAL.SetHaulierJobStatus(this.JobID, this.JobStatus, this.remarks5, con, tran);

                    TransportFacadeOut.EditTptJobToSpecialJobDetail(this, con, tran, HaulierJobDAL.GetPrefix());

                    #region OLD logging removed
                    //// now form the log entry
                    //DateTime serverDateTime = Logger.GetServerDateTime();

                    //// form the header  
                    ////20131226 - Gerry replaced child identifier to empty string
                    //LogHeader logHeader = new LogHeader(FMModule.Transport, formName, serverDateTime,
                    //    JobNo, string.Empty, FormMode.Edit, user);

                    //// This is an example of create the 2 logDetail objects
                    ////20131226 - Gerry replace property to be log
                    ////LogDetail logDetail1 = new LogDetail("CustNo", CustNo);
                    ////LogDetail logDetail2 = new LogDetail("JobType", JobType);
                    //LogDetail logDetail1 = new LogDetail(LOGDETAIL_SUBCON, subcontractorCode);

                    //// add the 2 logDetails objects to the List collection of logHeader
                    //logHeader.LogDetails.Add(logDetail1);
                    ////logHeader.LogDetails.Add(logDetail2);

                    //// now call the Logger class to write

                    //Logger.WriteLog(logHeader, con, tran);
                    #endregion

                    //20151028 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, formName, 0, FormMode.Edit.ToString());
                    auditLog.WriteAuditLog("JobStatus", JobTripStatus.Booked.ToString(), JobTripStatus.Completed.ToString(), con, tran);

                    tran.Commit();

                    this.UpdateVersion = GetHeaderUpdateVersion();
                }
                else
                {
                    throw new FMException(outMsg.ToString());
                }
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new FMException(ex.Message.ToString());
            }
            finally { con.Close(); }
            return true;
        }

        private bool IsHaulierCanSetToComplete(out string errMsg)
        {
            bool retValue = true;
            errMsg = TptResourceBLL.ErrNotCompletedJobTrips;
            string incompleteJobTrips = "";
            foreach (HaulierJobTrip tempJobTrip in this.jobTrips)
            {
                if (tempJobTrip.TripStatus != JobTripStatus.Completed)
                {
                    incompleteJobTrips += tempJobTrip.Sequence.ToString() + ", ";
                }
            }
            if (incompleteJobTrips != "")
            {
                retValue = false;
                errMsg = string.Format(errMsg, incompleteJobTrips);
            }

            return retValue;
        }

        #region "2014-03-14 Zhou Kai adds the code bolck for haulage logging related functions"
        public static List<string> GetAllDeletedHaulageJobNumbers(Dictionary<string, string> dict)
        {
            return HaulierJobDAL.GetAllDeletedHaulageJobNumbers(dict);
        }

        public static List<string> GetAllJobNoWhichHasDeletedJobTrips(Dictionary<string, string> dict)
        {
            return HaulierJobDAL.GetAllJobNoWhichHasDeletedJobTrips(dict);
        }

        public static List<string> GetAllDeletedJobTripSeqNos(Dictionary<string, string> dict)
        {
            return HaulierJobDAL.GetAllDeletedJobTripSeqNos(dict);
        }

        public static List<string> GetAllJobNoWhichHasDeletedJobCharges(Dictionary<string, string> dict)
        {
            return HaulierJobDAL.GetAllJobNoWhichHasDeletedJobCharges(dict);
        }

        #endregion

        //20140617 - gerry added to create haulier job and job trip from planning
        //SE, SI can also use this method 
        // bPlanningOrSeaFreightSide: true, add / edit from planning side; false, add / edit from sea freight side
        //20140724 - gerry added 1 parameter if job exist
        public bool AddHaulierJobAndTripFromPlanningOrSeaFreightSide(string formName, string user, bool bPlanningOrSeaFreightSide, bool existingJob, bool isShiftingJob = false)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (this.jobTrips.Count < 1)
                    throw new FMException(TptResourceBLL.ErrNoJobTripFound);

                if (ValidateAddHaulierJobHeader())   //job trip validation was transfered in the job trip loop
                {
                    string prefix = HaulierJobDAL.GetPrefix();
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    if (!existingJob)
                    {
                        //insert haulier job to database
                        HaulierJobDAL.AddHaulierHeader(this, con, tran, prefix, user);
                        //20140909 - gerry added to write into am_special_data table if job is billable for invoicing
                        //only write in if the job is a billable job and it is bill by transport 
                        if (this.BillByTransport && this.IsBillable)
                            TransportFacadeOut.AddTptJobToSpecialJobDetail(this, con, tran, prefix);
                        //20140909 end

                        // 2014-08-20 Zhou Kai adds, once we inserted the job_header
                        // we got the jID and jNO
                        foreach (HaulierJobTrip jobTrip in this.jobTrips) 
                        { jobTrip.JobID = this.JobID; }
                        foreach (HaulierJobTrip jobTrip in this.PartnerJobTrips)
                        { jobTrip.PartnerJobNo = this.JobNo; }
                        // 2014-11-14 Zhou Kai ends
                        HaulierJob.ChangeContainerMovementCharge(this, con, tran);
                        // 2014-08-20 Zhou Kai ends
                        #region OLD logging removed
                        ////log job header
                        //LogHeader logHeader = new LogHeader(FMModule.Transport, formName, DateTime.Now,
                        //       JobNo, string.Empty, FormMode.Add, user);
                        //LogDetail logDetail1 = new LogDetail(LOGDETAIL_QUOTATION_NO, this.QuotationNo);
                        //logHeader.LogDetails.Add(logDetail1);
                        //Logger.WriteLog(logHeader, con, tran);
                        #endregion

                        //20151028 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(this.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, formName, 0, FormMode.Add.ToString());
                        auditLog.WriteAuditLog("JobNumber", this.JobNo, string.Empty, con, tran);
                    }

                    // 2014-11-11 Zhou Kai, update partner_job_no for transfer out job in transfer out job via sea-freight scenairo
                    if (!bPlanningOrSeaFreightSide)
                    {
                        if (this.PartnerJobNo != String.Empty)
                        {
                            HaulierJob outJob = HaulierJob.GetHaulierJob(this.PartnerJobNo, con, tran);
                            List<string> conNos = new List<string>();
                            foreach (HaulierJobTrip t in this.JobTrips) { conNos.Add(t.ContainerNo); }
                            HaulierJob.UpdatePartnerJobNos(outJob.JobID, conNos, this.JobNo, con, tran);
                        }
                    }
                    // 2014-11-11 Zhou Kai ends

                    //cater the case for multiple job trips
                    //this method can be use in se import or export
                    foreach (HaulierJobTrip jobTrip in this.jobTrips)
                    {   //assign job header values to job trip
                        jobTrip.JobID = this.JobID;
                        jobTrip.JobNo = this.JobNo;

                        //20140913 - gerry replaced job trip status
                        jobTrip.TripStatus = JobTripStatus.Booked;
                        //20140913 end

                        if (jobTrip.ValidateAddHaulierJobTrip(isShiftingJob))
                        {
                            if (jobTrip.Sequence == 0 || !bPlanningOrSeaFreightSide)
                            {
                                //20140724 - gerry added to validate if can add job trip to existing job from jobtrip
                                if (existingJob)
                                {
                                    CheckDoubleContainerSealNos(jobTrip, con, tran);  //20140920 - Gerry added sql parameter
                                    CanAddTripToExistingFromPlanning(jobTrip);
                                }
                                //TODO validate if quotation has a single leg trip
                                if (!this.quotationNo.Equals(string.Empty) && jobTrip.isBillable)
                                {
                                    bool isStopDependent =
                                        this.haulierChargeType == ChargeType.StopDependent ? true : false;
                                    HashSet<string> rateUoms =
                                        HaulierJobTrip.GetHaulierUOMFromQuotation(jobTrip.CustomerCode,
                                                                                                                  isStopDependent,
                                                                                                                  this.bookingDate,
                                                                                                                  jobTrip.LegType);
                                    if (rateUoms.Count > 0)
                                    {
                                        ContainerTypesDTO containerDTO = ContainerTypesDTO.GetContainerDTO(jobTrip.ContainerCode.ToString().Trim());
                                        if (containerDTO != null)
                                        {
                                            if (!rateUoms.Contains(containerDTO.Size))
                                                throw new FMException(string.Format(TptResourceBLL.ErrNoValid1LegRateUomForContainerCode, jobTrip.ContainerCode));
                                        }
                                    }
                                    else
                                        throw new FMException(string.Format(TptResourceBLL.ErrNoContainerForQuotation, this.haulierChargeType));
                                }
                                //Add Job Trip
                                // 2014-10-17 Zhou Kai uses back the DAL function,
                                // but notice that now there's business logic of Generating LegGroup
                                // and LegGroupMember inside the DAL function(which is not a good
                                // practice), but we have to do because the BLL function will write
                                // logs and calculate charges per trip(which will have a transaction conflict)
                                HaulierJobDAL.AddHaulierJobTrip(this, jobTrip, con, tran);
                                // AddHaulierJobTrip(jobTrip, formName, user, con, tran);
                                // 2

                                //add job trip state 
                                //this case 2 states will be create because we can't save state directly to Ready, it must have Boooked status  
                                //first job state is Booked
                                // 2014-07-14 Zhou Kai modifies
                                JobTripState jobTripBookedState = new JobTripState(1, JobTripStatus.Booked, this.BookingDate,
                                    (bPlanningOrSeaFreightSide ? TptResourceUI.AddFromPlanning : TptResourceUI.ImportedFromSeaFreight) + " by " + user, true);

                                jobTrip.AddJobTripState(jobTripBookedState, con, tran);
                                #region  "2015-02-05 Zhou Kai adds logic to auto set all trips to ready(with conditions)"
                                if (jobTrip.LegType == LegType.FirstOfTwoLeg ||
                                        (
                                        jobTrip.LegType == LegType.OneLeg && 
                                        jobTrip.isMulti_leg == false
                                        )
                                    )
                                {

                                    string tempStatus =ApplicationOption.GetApplicationOption(ApplicationOption.HAULAGE_SETTINGS_ID,
                                    ApplicationOption.AUTO_SET_TRIPS_TO_READY_AFTER_TRANSFERRING).setting_Value;
                                    bool isReadyAfterTransfer = false;
                                    if (tempStatus == "T") { isReadyAfterTransfer = true; }

                                    if (!bPlanningOrSeaFreightSide && isReadyAfterTransfer)
                                    {
                                        // go on setting the trip to ready
                                        JobTripState jobTripReadyState = new JobTripState(2, JobTripStatus.Ready, this.BookingDate,
                                         "auto set the trip to ready from SF to HL by " + user, true);
                                        jobTrip.UpdateVersion = jobTrip.GetHaulierJobTripUpdateVersion(con, tran);
                                        jobTrip.AddJobTripState(jobTripReadyState, con, tran);
                                    }
                                }
                                #endregion

                                //get update version
                                jobTrip.UpdateVersion = jobTrip.GetHaulierJobTripUpdateVersion(con, tran);
                                if (bPlanningOrSeaFreightSide)
                                {
                                    //2nd job state is Assigned to create plan haulier sub trip
                                    //20140912 - gerry modified the 2nd job state is Ready to show in planning haulierJobTrips in case user will not save the whole plan
                                    JobTripState jobTripAssignedState = new JobTripState(3, JobTripStatus.Ready, this.BookingDate,
                                       (bPlanningOrSeaFreightSide ? TptResourceUI.AddFromPlanning : TptResourceUI.ImportedFromSeaFreight) + " by "+user, true);
                                    jobTrip.AddJobTripState(jobTripAssignedState, con, tran);
                                    jobTrip.UpdateVersion = jobTrip.GetHaulierJobTripUpdateVersion(con, tran);

                                    //20140912 - gerry add - jobTrip is Assigned to create plan haulier sub trip
                                    //only in memory in db 
                                    jobTrip.TripStatus = JobTripStatus.Assigned;
                                    //20140912 end
                                }
                                ////log job trip
                                //DoAddOrEditLogging(jobTrip, formName, user, FormMode.Add, con, tran);


                                ////20151028 - gerry added new audit log
                                AuditLog auditLog = new AuditLog(jobTrip.JobNo, "TR", "BK", this.JobID, user, DateTime.Now, formName, jobTrip.Sequence, FormMode.Add.ToString());
                                auditLog.WriteAuditLog("JobNumber", jobTrip.JobNo, string.Empty, con, tran);
                            }
                        }
                    }

                    tran.Commit();
                }
            }
            catch (FMException fmEx)
            {
                if (tran != null) { tran.Rollback(); }
                throw fmEx;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw new FMException(ex.Message.ToString());
            }
            finally { con.Close(); }
            return true;
        }
        //20140724 - gerry added to validate if can add job trip
        public bool CanAddTripToExistingFromPlanning(HaulierJobTrip jobTrip)
        {
            if (jobTrip.isMulti_leg)
            {
                HaulierJobTrip prevTrip = HaulierJobTrip.GetPreviousLegJobTrip(jobTrip.JobID, jobTrip.ContainerNo);
                if (prevTrip.TripStatus != JobTripStatus.Completed)
                    throw new FMException(TptResourceBLL.ErrCantAddJobTripNotCompletedPreviousLeg);
            }
            return true;
        }

        // 2014-07-07 Zhou Kai adds
        /// <summary>
        /// This function is not used to add a new trip based on quotation of the normal flow.
        /// It's used in case importing a haulier job from the sea freight side.
        /// In that case, a haulier job trip may or may not have a valid quotation, this function
        /// is used to do this checking.
        /// And in that case, the NOofLegs of all the haulier job trips are decided, hence, we nned
        /// to get the valid UOMs in quotation also based on NO of legs.
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckIfAllTripsHaveValidUOMInQuotation(Quotation quotation, SeaJob seaJob, int noOfLegs)
        {
            // (1) Select the uoms from the quotation, whose transport rate has the 
            // wanted value of (StopDependent, NoOfLegs) of the SeaFreight side
            List<string> uoms = new List<string>();
            if (this.HaulierChargeType == ChargeType.StopDependent)
            {
                foreach (TransportRate tr in quotation.TransportRates)
                {
                    if (tr.IsStopDependent && (noOfLegs == tr.NoOfLeg))
                    {
                        uoms.Add(tr.UOM);
                    }
                }
            }
            else
            {
                foreach (TransportRate tr in quotation.TransportRates)
                {
                    if (!tr.IsStopDependent && (noOfLegs == tr.NoOfLeg))
                    {
                        uoms.Add(tr.UOM);
                    }
                }
            }

            if (uoms.Count == 0)
            {
                return false;
            }

            // (2) Get the container size(which corresponds to uoms in quotation) from seaJob
            HashSet<string> containerSizes = new HashSet<string>();
            foreach (SeaJobContainer sjc in seaJob.SeaJobContainers)
            {
                containerSizes.Add(sjc.ContainerSize);
            }

            // (3) Compare the uoms and containerSizes
            foreach (string t in containerSizes)
            {
                if (uoms.Find(x => x.Equals(t)) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 2014-07-29 Zhou Kai adds, to get the quantity of containers for each type
        /// </summary>
        /// <param name="jobId">The id of the haulier job</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetContainerQtyByJobID(int jobId)
        {
            return HaulierJobDAL.GetContainerQtyByJobID(jobId);
        }

        /// <summary>
        /// 2014-08-06, Zhou Kai writes this function to re-calculate the container
        /// movement charges, when add / edit / delete a haulier job trip or haulier 
        /// job
        /// </summary>
        /// <param name="haulierJob">The haulier job whose containerMovement
        /// charges is going to be recalcualted</param>
        /// <param name="con">The database connection object</param>
        /// <param name="tran">The database operation transaction</param>
        /// <returns>The sortableList of recaculated HaulierJobCharge belonging
        /// to haulierJob</returns>
        public static SortableList<HaulierJobCharge> ChangeContainerMovementCharge
            (HaulierJob haulierJob, SqlConnection con, SqlTransaction tran)
        {
            SortableList<HaulierJobCharge> containerMovementCharges =
                new SortableList<HaulierJobCharge>(); // for return

            try
            {
                // delete existings first
                HaulierJobDAL.DeleteAllContainerMovementCharges(haulierJob.JobID, con, tran);
                // get the setting of how to handle the charges, is it per trip or group by transport rate
                ApplicationOption appOption = ApplicationOption.GetApplicationOption(
                                    ApplicationOption.HAULAGE_SETTINGS_ID,
                                    ApplicationOption.SETTINGS_HAULAGE_INVOCE_BY_TPTRATE_OR_PERTRIP
                                     );

                // case: job is not under quotation: no charges
                if (haulierJob.quotationNo.Equals(string.Empty)) { return containerMovementCharges; }

                // case job is under quotation: to calculate charges according to the transport rates under the quotation
                Quotation quotation = Quotation.GetAllQuotationHeader(haulierJob.quotationNo);
                // trips collection combining own trips and transferred in trips
                SortableList<HaulierJobTrip> combinedTrips = new SortableList<HaulierJobTrip>();
                // -----> partner trips(if applicable)
                if (haulierJob.JobTransferType == JobTransferType.TransferIn &&
                    haulierJob.PartnerJobNo != String.Empty &&
                    haulierJob.PartnerJobTrips.Count == 0
                    )
                {
                    combinedTrips = HaulierJobDAL.GetPartnerTripsForInJobFromOutJob(haulierJob.JobNo,
                                       con, tran);
                }
                else
                {
                    foreach (HaulierJobTrip t in haulierJob.PartnerJobTrips)
                    { combinedTrips.Add(t); Debug.WriteLine(
                        " Partners: JNO:" + t.JobNo + " Seq: " + t.Sequence + " ConNO:" + t.ContainerNo +
                        " PaerterJNO: " + t.PartnerJobNo); }
                }
                // -----> existing trips
                foreach (HaulierJobTrip t in haulierJob.JobTrips) { combinedTrips.Add(t);
                Debug.WriteLine(
                    " Own: JNO:" + t.JobNo + " Seq: " + t.Sequence + " ConNO:" + t.ContainerNo +
                    " PartnerJNO: " + t.PartnerJobNo);
                }

                // simple sorting of trips
                combinedTrips.OrderBy(x => x.LegGroup);
                // end of sorting

                // foreach (HaulierJobTrip haulierTrip in haulierJob.jobTrips)
                foreach (HaulierJobTrip t in combinedTrips)
                {
                    // the "if" statement below is only a in-memory assignment, 
                    // to make the charge of a partner trip be under the transferred_in_job
                    if (haulierJob.JobTransferType == JobTransferType.TransferIn &&
                        t.PartnerJobNo != String.Empty && 
                        t.JobID != haulierJob.JobID)
                    {
                        Debug.WriteLine("Bf modify JID: " + t.JobID + " JNO: " + t.JobNo + " Seq: " + t.Sequence +
    " ConCode:" + t.ContainerCode + " ConNO:" + t.ContainerNo + " PartnerJNO: " + t.PartnerJobNo);

                        t.JobID = haulierJob.JobID; 
                        t.JobNo = haulierJob.JobNo;
                        t.isBillable = true;

                        Debug.WriteLine("Af modify JID: " + t.JobID + " JNO: " + t.JobNo + " Seq: " + t.Sequence +
  " ConCode:" + t.ContainerCode + " ConNO:" + t.ContainerNo + " PartnerJNO: " + t.PartnerJobNo);
                    }

                    if (!t.isBillable) { continue; }
                    // create a charge for this trip
                    HaulierJobCharge haulierJobCharge = new HaulierJobCharge();
                    // get the uom(size) from container code
                    string uom = ContainerTypesDTO.GetContainerSizeFromContainerCode(
                        t.ContainerCode).ToUpper();
                    // look for the correct transport rate for the trip
                    TransportRate transportRate = new TransportRate();
                    try // 2014-10-20 Zhou Kai adds try catch
                    {
                        transportRate =
                           quotation.TransportRates.First(x => x.UOM.ToUpper().Equals(uom) &&
                               // 2014-08-20 Zhou Kai comments the one line below
                               // x.ChargeID.ToUpper().Equals(haulierTrip.ChargeCode.ToUpper()) && 
                               x.NoOfLeg == (t.LegType == LegType.OneLeg ? 1 : 2));
                    }
                    catch (ArgumentNullException)
                    { throw new FMException(TptResourceBLL.NoSuitableRate); }
                    catch (InvalidOperationException)
                    { throw new FMException(TptResourceBLL.NoSuitableRate); }
                    // single leg
                    if (t.LegType == LegType.OneLeg)
                    {
                        haulierJobCharge = FillContainerMovementChargeFromTrip(haulierJob, transportRate, t);
                        containerMovementCharges.Add(haulierJobCharge);
                    }
                    // 1st leg of 2-leg trips
                    if (t.isMulti_leg && (t.LegType == LegType.FirstOfTwoLeg))
                    {
                        // each first leg of two legs corresponds to a haulierJobCharge
                        haulierJobCharge = FillContainerMovementChargeFromTrip(haulierJob, transportRate, t);
                        containerMovementCharges.Add(haulierJobCharge);
                    }
                    else { /* It's the second leg of 2-legs trip, just ignore it.*/}
                }

                // re-group the container movement charges by (ChargeID, UOM and NoOfLegs)
                foreach(HaulierJobCharge c in containerMovementCharges)
                { Debug.WriteLine("JID: " + c.JobID +  " TSeq: " + c.JobTripSeqNo +" ConNO: " + c.ContainerNo) ; }

                bool isRatePerTrip = appOption.setting_Value == "T" ? true : false;
                containerMovementCharges = ReGroupHaulierJobCharges(quotation, containerMovementCharges, !isRatePerTrip); //!appOption.setting_Value);
                // write into database
                foreach (HaulierJobCharge tmp in containerMovementCharges)
                {
                    HaulierJobDAL.AddHaulierJobCharges(tmp, con, tran);
                }

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return containerMovementCharges;
        }

        /// <summary>
        /// This function is the same version with no dbCon and dbTran as params
        /// </summary>
        /// <param name="haulierJob"></param>
        /// <returns></returns>
        public static SortableList<HaulierJobCharge> ChangeContainerMovementCharge
            (HaulierJob haulierJob, string formName, string userId)
        {
            using (SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction dbTran = null;
                try
                {
                    if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                    dbTran = dbCon.BeginTransaction();

                    return ChangeContainerMovementCharge(haulierJob, dbCon, dbTran);

                }
                catch (FMException fmEx) { throw fmEx; }
                catch (SqlException ex) { throw new FMException(ex.ToString()); }
                catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
                catch (Exception ex) { throw new FMException(ex.ToString()); }
            }

        }

        /// <summary>
        /// 2014-08-06 Zhou Kai adds this function to re-group the haulierJobCharges
        /// If byGroup is true, group the charges by transport rate(the charges of the
        /// same rate will be grouped together)
        /// If byGroup is false, return the containerMovement charges as it is(per 
        /// trip per charge).
        /// </summary>
        /// <returns>The re-grouped containerMovement charges</returns>
        private static SortableList<HaulierJobCharge> ReGroupHaulierJobCharges(
            Quotation quotation,
            SortableList<HaulierJobCharge> haulierJobCharges,
            bool byGroup)
        {
            if (!byGroup) { return haulierJobCharges; }
            else
            {
                SortableList<HaulierJobCharge> rtn = new SortableList<HaulierJobCharge>();
                // group by transport rate(which the haulier job charges belong to)
                IEnumerable<IGrouping<int, HaulierJobCharge>> groups =
                    haulierJobCharges.GroupBy(x => x.SequenceNoRate);

                foreach (IGrouping<int, HaulierJobCharge> group in groups)
                {
                    // use the first charge of the group to represent the whole group
                    // (namely to combine multiple charges)
                    HaulierJobCharge firstChargeInGroup = (HaulierJobCharge)group.ElementAt(0);

                    int billing_qty = group.Count();
                    // Notice the transportRateSeqNo is 1-based
                    // decimal totalAmountFC = quotation.TransportRates[firstChargeInGroup.SequenceNoRate - 1].GetTotal(billing_qty);
                    // 2014-11-10 Zhou Kai modifies the one line above
                    decimal totalAmountFC = quotation.TransportRates.First(
                        x => x.SequenceNo == firstChargeInGroup.SequenceNoRate).GetTotal(billing_qty);
                    // 2014-11-10 Zhou Kai ends
                    firstChargeInGroup.TotalAmountFC = totalAmountFC;
                    firstChargeInGroup.Quantity = billing_qty;
                    // No DivideByZero exception should happen here
                    firstChargeInGroup.UnitRateFC = totalAmountFC / billing_qty;

                    rtn.Add((HaulierJobCharge)group.ElementAt(0));
                }

                return rtn;
            }
        }

        /// <summary>
        /// 2014-08-06 Zhou Kai adds this function, to form the containerMovement charger
        /// for haulier job trip under the haulier job.
        /// </summary>
        /// <param name="haulierJob">The haulier job the calculated haulier job tirp is under.</param>
        /// <param name="trip">The haulier job trip being calculated</param>
        /// <param name="rate">The transport rate</param>
        /// <returns>The containerMovement charge for this haulier job trip</returns>
        private static HaulierJobCharge FillContainerMovementChargeFromTrip
            (HaulierJob haulierJob, TransportRate transportRate, HaulierJobTrip haulierJobTrip)
        {
            try
            {
                // get charge object for gst information only
                Charge charge = Charge.GetCharge(transportRate.ChargeID, haulierJob.BranchCode);
                // create a new haulierJobCharge
                HaulierJobCharge haulierJobCharge = new HaulierJobCharge();
                // initialize the haulierJobCharge with transportRate haulier job and haulier job trip
                haulierJobCharge.QuotationID = transportRate.QuotationID;
                haulierJobCharge.QuotationNo = haulierJob.QuotationNo;
                haulierJobCharge.RateType = transportRate.ChargeType;
                haulierJobCharge.ChargeCode = transportRate.ChargeID;
                haulierJobCharge.ChargeDescription = transportRate.Description;
                haulierJobCharge.Uom = ContainerTypesDTO.GetContainerSizeFromContainerCode(haulierJobTrip.ContainerCode);
                haulierJobCharge.JobID = haulierJobTrip.JobID;
                haulierJobCharge.Currency = transportRate.Currency;
                haulierJobCharge.ExchangeRate = (CurrencyRate.GetCurrencyRate(transportRate.Currency, haulierJob.bookingDate)).Rate;
                haulierJobCharge.JobChargeType = "T";
                Gst tempGst = Gst.GetGstRate(charge.TaxCode, haulierJob.BookingDate);
                haulierJobCharge.GstRate = tempGst.GstRate;
                haulierJobCharge.GstType = charge.TaxCode;
                haulierJobCharge.JobChargeStatus = JobChargeStatus.Booked;
                haulierJobCharge.JobTripSeqNo = haulierJobTrip.Sequence;
                haulierJobCharge.ContainerNo = haulierJobTrip.ContainerNo; // special for haulier trip
                #region "for a haulier job trip, the billing quantity is the container quantity, which is always 1"
                haulierJobCharge.Quantity = 1M;
                haulierJobCharge.TotalAmountFC = transportRate.GetTotal(1);
                haulierJobCharge.UnitRateFC = haulierJobCharge.TotalAmountFC;
                #endregion
                // The link between charge and rate,
                // DO notice this seq no is 1-based, no 0-based
                haulierJobCharge.SequenceNoRate = transportRate.SequenceNo;

                return haulierJobCharge;
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

        }

        /// <summary>
        /// 2014-08-08 Zhou Kai adds function to re-calcualte the haulier job charges
        /// (namely the container movement charges) one by one.
        /// Please notice that, this function does not consider the price-break, menas
        /// the charges are not combined by transport rate, instead, it's calculated
        /// per haulier job trip(or per container movement charge)
        /// </summary>
        /// <param name="containerMovementCharges">The SortableList of HaulierJobCharge(partially) under a haulier job</param>
        /// <param name="quotation">The quotation the haulier job is under</param>
        public static void CalculateAmtForContainerMovementCharges(
            SortableList<HaulierJobCharge> containerMovementCharges, Quotation quotation)
        {
            try
            {
                foreach (HaulierJobCharge haulierJobCharge in containerMovementCharges)
                {
                    // look for the correct transport rate
                    TransportRate transportRate = quotation.TransportRates.First(x =>
                                          x.UOM.ToUpper().Equals(haulierJobCharge.Uom.ToUpper()) &&
                                          x.ChargeID.ToUpper().Equals(haulierJobCharge.ChargeCode.ToUpper())
                                           );
                    // calculate the charge according to the transport rate and billing quantity
                    haulierJobCharge.TotalAmountFC = transportRate.GetTotal(1); // for foreign currency

                    // calculate unit rate(the unit rate equals to that of the total amount here):
                    haulierJobCharge.UnitRateFC = haulierJobCharge.TotalAmountFC;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return;
        }

        /// <summary>
        /// 2014-08-08 Zhou Kai adds
        /// </summary>
        /// <param name="haulierJob">The haulier job whose charges will be removed</param>
        /// <param name="con">The database connection object</param>
        /// <param name="tran">The database operation transaction</param>
        /// <returns>True, operation successfully; False, operation failed.</returns>
        public static bool DeleteAllContainerMovementCharges(HaulierJob haulierJob, SqlConnection con, SqlTransaction tran)
        {
            if (haulierJob.JobStatus == JobStatus.Complete) { throw new FMException(TptResourceUI.CantModifyCompleteJob); }

            bool retValue = false;
            try
            {
                retValue = HaulierJobDAL.DeleteAllContainerMovementCharges(haulierJob.JobID, con, tran);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;
        }

        /// <summary>
        /// 2014-08-14 Zhou Kai adds, the same version of the above function, with no
        /// dbCon and dbTran as params
        /// </summary>
        /// <returns></returns>
        public static bool DeleteAllContainerMovementCharges(int jobId)
        {
            bool retValue = false;
            try
            {
                retValue = HaulierJobDAL.DeleteAllContainerMovementCharges(jobId);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;
        }

        /// <summary>
        /// 2014-08-14 Zhou Kai adds
        /// </summary>
        private void GetContainerCodeSizeMapping()
        {
            if (HaulierJob.containerCodeSizeMapping.Count == 0)
            {
                containerCodeSizeMapping = TransportFacadeIn.GetContainerCodeSizeMapping();
            }
            else
            { /* don't get again */}
        }

        #region "For Union Services"

        /// <summary>
        /// 2014-10-09 Zhou Kai.
        /// Get the next legGroup id from db
        /// </summary>
        /// <param name="jobId">The id of the haulier
        /// job this function works on</param>
        /// <returns>The next leg group id</returns>
        public static int GetNextLegGroup(int jobId)
        {
            return HaulierJobDAL.GetNextLegGroup(jobId);
        }

        public static int GetNextLegGroup(int jobId, SqlConnection dbCon, SqlTransaction dbTran)
        {
            return HaulierJobDAL.GetNextLegGroup(jobId, dbCon, dbTran);
        }

        /// <summary>
        /// 2014-10-09 Zhou Kai
        /// Get the next legGroup id from db
        /// </summary>
        /// <param name="job">The haulier job trip this function works on</param>
        /// <returns>The next leg group id</returns>
        public static int GetNextLegGroupMember(int jobId, int legGroup)
        {
            return HaulierJobDAL.GetNextLegGroupMember(jobId, legGroup);
        }

        public static int GetNextLegGroupMember(int jobId, int legGroup, SqlConnection dbCon,
            SqlTransaction dbTran)
        {
            return HaulierJobDAL.GetNextLegGroupMember(jobId, legGroup, dbCon, dbTran);
        }

        /// <summary>
        /// 2014-10-20 Get LegGroupMember of a new job trip by its jobId and containerNo.
        /// Notice: this function is not implemented yet
        /// </summary>
        /// <param name="jobId">The id of the job that the new trip is going to be added under</param>
        /// <param name="containerNo">The container no of the new job trip that going to be added
        /// </param>
        /// <returns>The LegGroupMember of the new haulier job trip</returns>
        public static int GetLegGroupMemberByContainerNo(int jobId, string containerNo)
        {
            NotImplementedException ne = new NotImplementedException();

            throw new FMException(ne.Message);
        }

        /// <summary>
        /// 2014-10-09 Zhou Kai adds.
        /// Update the legGroupMember of the following
        /// trips when one of the trip in a legGroup is deleted.
        /// </summary>
        /// <param name="trip">The trip to be deleted</param>
        /// <returns>true: successfull; false: failed</returns>
        public static bool ChangeLegGroupMember(HaulierJobTrip trip)
        {
            return HaulierJobDAL.ChangeLegGroupMember(trip);
        }

        public static bool ChangeLegGroupMember(HaulierJobTrip trip,
                                                                   SqlConnection dbCon,
                                                                   SqlTransaction dbTran)
        {
            return HaulierJobDAL.ChangeLegGroupMember(trip, dbCon, dbTran);
        }

        public static bool AddHaulierJobTrips(HaulierJob job, SortableList<HaulierJobTrip> trips,
            string formName, string usrId)
        {
            return HaulierJobDAL.AddHaulierJobTrips(job, trips, formName, usrId);
        }

        public static bool AddHaulierJobTrips(HaulierJob job,
                                                         SortableList<HaulierJobTrip> newTrips,
                                                         string formName,
                                                         string usrId,
                                                         SqlConnection dbCon,
                                                         SqlTransaction dbTran)
        {
            try
            {
                // calculate the charges first:
                SortableList<HaulierJobTrip> allTrips = new SortableList<HaulierJobTrip>();
                // merge new trips with existing trips
                foreach (HaulierJobTrip t in job.JobTrips) { allTrips.Add(t); }
                // assign the allTrips to job in-memory temporarily only for charge_calculating
                job.JobTrips = allTrips;
                // calculate charges based on all trips(including new trips, existing trips, partner trips)
                // the partner_trips will be merged with all trips inside the ChangeContainerXXX function
                ChangeContainerMovementCharge(job, dbCon, dbTran);
                // insert only new trips into database, and write logs
                return HaulierJobDAL.AddHaulierJobTrips(job, newTrips, formName, usrId, dbCon, dbTran);
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }

        /// <summary>
        /// 2014-10-17 Zhou Kai adds the function to generate sampleTrips from the sea job: sJob
        /// for the haulierJob: hJob, according to the stops.
        /// The containers in the sJob are grouped by the container code, and each container code
        ///will have a set of sample trips. Per container code per sample trips set, per leg(2 stops) per sample trip
        /// </summary>
        /// <param name="sJob">The sea job</param>
        /// <param name="inJob">The transfer in haulier job</param>
        /// <param name="outJob">The transfer out job, it can be null if the inJob has no TO job</param>
        /// <param name="stops">The stops collection</param>
        /// <returns>The sample trips</returns>
        public static List<HaulierJobTrip> CreateSampleTripsForSeaJob(SeaJob sJob, HaulierJob inJob, HaulierJob outJob, List<Stop> stops)
        {
            List<HaulierJobTrip> sampleTrips = new List<HaulierJobTrip>();
            try
            {
                bool isSeaExport = false;
                if (sJob is SeaExportJob) { isSeaExport = true; }
                if (sJob is SeaImportJob) { isSeaExport = false; }

                //// 2014-11-12 Zhou Kai adds logic, if the inJob has an outJob, insert an blank 
                //// stop at the head of the stop chain
                //if (outJob != null) { stops.Insert(0, new Stop()); }
                //// 2014-11-12 Zhou Kai ends


                // data which is job-header related

                OperationDetail od = isSeaExport ?
                                        OperationDetail.GetOperationDetail(sJob.ShipperCode) :
                                        OperationDetail.GetOperationDetail(sJob.ConsigneeCode);
                CustomerDTO cud = (od == null || od.customerCode == String.Empty) ?
                                        CustomerDTO.GetCustomerByCustCode("CASH") :
                                        CustomerDTO.GetCustomerByCustCode(od.customerCode);
                /*
                 * 2014-10-24 Zhou Kai comments out this line, instead of group by container_code,
                 * we group by container_size now
                IEnumerable<IGrouping<string, SeaJobContainer>> containerGrps =
                    sJob.SeaJ
                 * obContainers.GroupBy(x => x.ContainerCode);
                */
                Stop lastStopOfHubbingJob = new Stop();
                if (outJob != null)
                    lastStopOfHubbingJob = HaulierJobTrip.GetAllHaulierJobTrips(outJob).Last().EndStop;

                IEnumerable<IGrouping<string, SeaJobContainer>> containerGrps = sJob.SeaJobContainers.GroupBy(x => x.ContainerSize);
                for (int i = 0; i < stops.Count - 1; i++)
                {
                    bool isLaden = isSeaExport ? (i == 0 ? false : true) : (i == stops.Count - 2 ? false : true);
                    foreach (IGrouping<string, SeaJobContainer> g in containerGrps)
                    {
                        HaulierJobTrip t = new HaulierJobTrip()
                        {
                            JobID = inJob.JobID, // uninitialized yet
                            Sequence = i, // uninitialized yet
                            JobNo = inJob.JobNo, // uninitialized yet
                            isBillable = true,
                            isMulti_leg = stops.Count > 2 ? true : false,
                            IsNew = true,
                            IsLaden = isLaden,
                            StartStop = outJob == null ? (Stop)stops[i].Clone() : (i == 0 ? lastStopOfHubbingJob : (Stop)stops[i].Clone()),
                            EndStop = (Stop)stops[i + 1].Clone(),

                            DGRemarks = g.ElementAt(0).ContainerSize,
                            LegType = (outJob != null) ? LegType.OneLeg : // has outJob, always 1-leg
                                // doesn't have outJob, depends on if > 2 legs
                                         (stops.Count > 2 && i == 0 ? LegType.FirstOfTwoLeg :
                                         (stops.Count > 2 && i == 1 ? LegType.SecondOfTwoLeg : LegType.OneLeg)),
                            PartnerLeg = (outJob != null) ? 0 : //has outJob, always no partnerLeg
                              (stops.Count == 2) ? 0 : (i == 0 ? 2 : (i == 1 ? 1 : 0)), // doesn't have outJob, depends on if > 2 legs
                            TripStatus = JobTripStatus.Booked,
                            CustomerCode = inJob.CustNo,
                            CustomerName = inJob.JobTrips.Count > 0 ? inJob.JobTrips[0].CustomerName : String.Empty,
                            jobType = inJob.jobType,
                            StartDate = DateTime.Today,// inJob.eta,
                            EndDate = DateTime.Today,
                            StartTime = "0000",
                            EndTime = "0000",
                            // assume the GrossWeight / GrossCBM of a same container code is always the same
                            ContainerCode = g.ElementAt(0).ContainerCode,
                            GrossWeight = g.ElementAt(0).GrossWeight,
                            GrossCBM = g.ElementAt(0).GrossCBM,
                            maxWeight = g.ElementAt(0).GrossWeight,
                            maxCBM = g.ElementAt(0).GrossCBM,
                            OwnTransport = true,
                        };
                        t.CustomerCode = cud.Code; t.CustomerName = cud.Name;
                        // default the first leg of sea export to not laden
                        // 2014-11-20 Zhou Kai adds condition: outJob == null
                        if (isSeaExport && i == 0 && outJob == null) { t.IsLaden = false; }
                        // cannot default the second leg of sea import to not laden
                        // if (!isSeaExport && i == 1) { t.IsLaden = false; }            


                        if (t.StartStop.Code.Equals(t.EndStop.Code))
                            throw new FMException(TptResourceBLL.ErrSameStop);

                        if (!sampleTrips.Contains(t))
                            sampleTrips.Add(t);

                    }
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return sampleTrips;
        }
   

        /// <summary>
        /// 2014-10-10 Zhou Kai.
        /// Create haulier job trips for multi-containers and more than 2 legs under job.
        /// The standalone scenario is different from the seafreight one, because the in seafreight
        /// scenario the job header is not created yet. So there is another function created solely
        /// for sea-freight scenario: CreateConcreteTripsFromSampleTripsForSeaFreight()
        /// </summary>
        /// <param name="job">The job for the new trips</param>
        /// <param name="containerSizeToCode">The mapping between container code
        /// and the container quantity</param>
        /// <param name="stops">The collection of stops</param>
        /// <param name="sampleTrips">Sample trips are a sub-set of all the new 
        /// haulier job trips represented by the collection of stops. It's the collection
        /// of all the legs of the first container. It's for display, and for the convenient of 
        /// setting the properties of all the containers of the same leg.</param>
        /// <returns></returns>
        public static SortableList<HaulierJobTrip> CreateConcreteTripsFromSampleTripsForStandaloneJob(
                                                                       HaulierJob job,
                                                                       Dictionary<string, int> containerCodeToQty,
                                                                       List<HaulierJobTrip> sampleTrips)
        {
            SortableList<HaulierJobTrip> allTrips = new SortableList<HaulierJobTrip>();

            try
            {
                List<string> containerCodes = new List<string>();
                List<int> containerQty = new List<int>();
                int nContainerQty = 0;
                // assume there are only 2 key value pairs in the dictionary
                foreach (KeyValuePair<string, int> kvp in containerCodeToQty)
                {
                    containerCodes.Add(kvp.Key);
                    containerQty.Add(kvp.Value);
                    nContainerQty += kvp.Value;
                }

                // all the trips
                int LegGroup = GetNextLegGroup(job.JobID);
                int nSeq = job.JobTrips.Count > 0 ? job.JobTrips.Max(x => x.Sequence) + 1 : 1;
                if (nContainerQty > containerQty[0]) // the sample trips are duplicated(for two types container code)
                {
                    for (int i = 0; i < containerQty[0]; i++) // the first part of sample trips
                    {
                        ContainerTypesDTO container = ContainerTypesDTO.GetContainerDTO(containerCodes[0]);
                        // all trips under the same container shares the same LegGroup
                        for (int j = 0; j < sampleTrips.Count / 2; j++)
                        {
                            HaulierJobTrip t = (HaulierJobTrip)sampleTrips[j].Clone();
                            t.Sequence = nSeq++;
                            t.LegGroup = LegGroup;
                            t.PartnerLeg = sampleTrips[j].PartnerLeg; // 2014-10-27 Zhou Kai
                            switch (t.PartnerLeg) // 2014-11-05 ZK add switch
                            {
                                case 0: { /*do nothing*/ break; }
                                case 1: { t.PartnerLeg = t.Sequence - 1; break; }
                                case 2: { t.PartnerLeg = t.Sequence + 1; break; }
                                default: { break; }
                            }
                            OperatorDTO o = sampleTrips[j].subCon;
                            t.subCon = new OperatorDTO(o.Code, o.Name, o.Add1, o.Add2, o.Add3,
                                o.Add4, o.City); // 2014-10-29 Zhou Kai
                            t.OwnTransport = o.Code == String.Empty;
                            t.GrossWeight = sampleTrips[j].GrossWeight == 0 ? container.TareWeight : sampleTrips[j].GrossWeight;
                            t.GrossCBM = container.TareCBM;
                            t.maxWeight = sampleTrips[j].GrossWeight == 0 ? container.TareWeight : sampleTrips[j].GrossWeight;
                            t.maxCBM = container.TareCBM;
                            allTrips.Add(t);
                        }
                        LegGroup++;
                    }
                    for (int i = 0; i < containerQty[1]; i++) // the second part of sample trips
                    {
                        ContainerTypesDTO container = ContainerTypesDTO.GetContainerDTO(containerCodes[1]);
                        for (int j = sampleTrips.Count / 2; j < sampleTrips.Count; j++)
                        {
                            HaulierJobTrip t = (HaulierJobTrip)sampleTrips[j].Clone();
                            t.Sequence = nSeq++;
                            t.LegGroup = LegGroup;
                            t.PartnerLeg = sampleTrips[j].PartnerLeg;  // 2014-10-27 Zhou Kai
                            switch (t.PartnerLeg) // 2014-11-05 ZK add switch
                            {
                                case 0: { /*do nothing*/ break; }
                                case 1: { t.PartnerLeg = t.Sequence - 1; break; }
                                case 2: { t.PartnerLeg = t.Sequence + 1; break; }
                                default: { break; }
                            }
                            OperatorDTO o = sampleTrips[j].subCon;
                            t.subCon = new OperatorDTO(o.Code, o.Name, o.Add1, o.Add2, o.Add3,
                                o.Add4, o.City); // 2014-10-29 Zhou Kai
                            t.OwnTransport = o.Code == String.Empty;
                            t.GrossWeight = sampleTrips[j].GrossWeight == 0 ? container.TareWeight : sampleTrips[j].GrossWeight;
                            t.GrossCBM = container.TareCBM;
                            t.maxWeight = sampleTrips[j].GrossWeight == 0 ? container.TareWeight : sampleTrips[j].GrossWeight;
                            t.maxCBM = container.TareCBM;
                            allTrips.Add(t);
                        }
                        LegGroup++;
                    }
                }
                else
                {
                    // there are only one type of containerCode
                    for (int i = 0; i < nContainerQty; i++)
                    {
                        ContainerTypesDTO container = ContainerTypesDTO.GetContainerDTO(containerCodes[0]);
                        // need to generate the Leg_Group here
                        for (int j = 0; j < sampleTrips.Count; j++)
                        {
                            HaulierJobTrip t = (HaulierJobTrip)sampleTrips[j].Clone();
                            t.Sequence = nSeq++;
                            t.LegGroup = LegGroup;
                            t.PartnerLeg = sampleTrips[j].PartnerLeg; // 2014-10-27 Zhou Kai
                            switch (t.PartnerLeg) // 2014-11-05 ZK add switch
                            {
                                case 0: { /*do nothing*/ break; }
                                case 1: { t.PartnerLeg = t.Sequence - 1; break; }
                                case 2: { t.PartnerLeg = t.Sequence + 1; break; }
                                default: { break; }
                            }
                            OperatorDTO o = sampleTrips[j].subCon;
                            t.subCon = new OperatorDTO(o.Code, o.Name, o.Add1, o.Add2, o.Add3,
                                o.Add4, o.City); // 2014-10-29 Zhou Kai
                            t.OwnTransport = o.Code == String.Empty;
                            t.GrossWeight = sampleTrips[j].GrossWeight == 0 ? container.TareWeight : sampleTrips[j].GrossWeight;
                            t.GrossCBM = container.TareCBM;
                            t.maxWeight = sampleTrips[j].GrossWeight == 0 ? container.TareWeight : sampleTrips[j].GrossWeight;
                            t.maxCBM = container.TareCBM;
                            allTrips.Add(t);
                        }
                        LegGroup++;
                    }
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return allTrips;
        }

        /// <summary>
        /// 2014-10-17 Zhou Kai creates function to generate all trips based on SeaFreight job
        /// and the sample trips.
        /// </summary>
        /// <param name="hJob">The haulier job, can be used as stand-alone job or inJob, 
        /// if outJob == null, it's the inJob, otherwise, it's the stand-alone job.</param>
        /// <param name="sJob">The sea freight job the hJob imported from</param>
        /// <param name="sampleTrips">The sample trips</param>
        /// <returns>All the trips</returns>
        public static SortableList<HaulierJobTrip> CreateConcreteTripsFromSampleTripsForSeaFreight(
            HaulierJob hJob, HaulierJob outJob, SeaJob sJob, List<HaulierJobTrip> sampleTrips,
            Dictionary<string, int> dicConNoToLegGroup)
        {
            SortableList<HaulierJobTrip> allTrips = new SortableList<HaulierJobTrip>();
            try
            {
                // generate all trips according to the sample trips 
                // 2014-11-03 Zhou Kai, assign seq / partnerLeg also
                int nSeq = hJob.JobTrips.Count > 0 ? hJob.JobTrips.Max(x => x.Sequence) + 1 : 1;
                int nConIndex = 1;
                foreach (SeaJobContainer sc in sJob.SeaJobContainers)
                {
                    int nLegGroup, nLegGroupMember;
                    if (outJob != null)
                    {
                        var outTrips = outJob.JobTrips.Where(x => x.ContainerNo == sc.ContainerNumber).ToList();
                        if (outTrips.Count > 0)
                        {
                            nLegGroup = dicConNoToLegGroup[sc.ContainerNumber];
                            nLegGroupMember = outTrips.Max(x => x.LegGroupMember) + 1;
                        }
                        else
                        {
                            nLegGroup = nConIndex++;
                            nLegGroupMember = 1;
                        }
                    }
                    else
                    {
                        nLegGroup = nConIndex++;
                        nLegGroupMember = 1;
                    }

                    // 2014-11-12 for each container, inherits the end stop of the last leg for that container
                    // in the TO job as the start stop of the 1st leg of the TI job
                    if (outJob != null)
                    {
                        Stop inheritedStop = new Stop();
                        //int seqNoOfTOLastLeg = outJob.JobTrips.Where(x => x.ContainerNo == sc.ContainerNumber).Max(x => x.Sequence);
                        //inheritedStop = (Stop)outJob.JobTrips.First(x => x.Sequence == seqNoOfTOLastLeg).EndStop.Clone();

                        var outJobTrip = outJob.JobTrips.Where(jt => jt.ContainerNo == sc.ContainerNumber).ToList();
                        if (outJobTrip.Count > 0)
                        {
                            inheritedStop = (Stop)outJobTrip[outJobTrip.Count - 1].EndStop.Clone();
                            // DGRemarks is used to store the container_size for sea-freight scenario
                            sampleTrips.Where(x => x.DGRemarks == sc.ContainerSize).OrderBy(x => x.Sequence).ElementAt(0).StartStop = inheritedStop;
                        }
                        else //create partner leg
                        {
                            HaulierJobTrip firstLegTrip = sampleTrips[0].Clone() as HaulierJobTrip;
                            firstLegTrip.PartnerJobNo = string.Empty;
                            firstLegTrip.IsLaden = false;
                            firstLegTrip.GrossWeight = 0;
                            firstLegTrip.GrossCBM = 0;
                            firstLegTrip.StartStop = Stop.GetStop(sJob.PickUpPlaceCode);
                            firstLegTrip.EndStop = Stop.GetStop(sJob.StuffingCode);
                            firstLegTrip.PartnerLeg = sampleTrips[0].Sequence;
                            firstLegTrip.Sequence = sampleTrips[0].Sequence + 1;
                            firstLegTrip.DGRemarks = sc.ContainerSize;
                            //modify the sample jobtrip
                            sampleTrips[0].PartnerLeg = firstLegTrip.Sequence;
                            sampleTrips[0].IsLaden = true;
                            sampleTrips[0].StartStop = Stop.GetStop(firstLegTrip.EndStop.Code);

                            sampleTrips.Insert(0, firstLegTrip);
                        }
                    }
                    // 2014-11-12 Zhou Kai ends
                    ContainerTypesDTO container = ContainerTypesDTO.GetContainerDTO(sc.ContainerCode);

                    foreach (HaulierJobTrip t in sampleTrips)
                    {
                        // change grouping from by container_code to by container_size
                        //if (sc.ContainerCode == t.ContainerCode)
                        // notice here since t has no conSize property, so t.DGRemarks
                        // is used to store conSize in sample trips temporarily
                        if (sc.ContainerSize == t.DGRemarks)
                        {                            
                            HaulierJobTrip newTrip = (HaulierJobTrip)t.Clone();

                            newTrip.Sequence = nSeq++;
                            newTrip.PartnerLeg = t.PartnerLeg;
                            switch (newTrip.PartnerLeg) // 2014-11-04 ZK add switch
                            {
                                case 1: { newTrip.PartnerLeg = newTrip.Sequence - 1; break; }
                                case 2: { newTrip.PartnerLeg = newTrip.Sequence + 1; break; }
                                default: { break; }
                            }
                            // data shared by LegGroup
                            newTrip.ContainerNo = sc.ContainerNumber;
                            newTrip.ContainerCode = sc.ContainerCode;
                            newTrip.SealNo = sc.SealNumber;
                            newTrip.DGRemarks = String.Empty;

                            newTrip.LegGroup = nLegGroup;
                            newTrip.LegGroupMember = nLegGroupMember++;
                            newTrip.subCon = new OperatorDTO(t.subCon.Code,
                                t.subCon.Name, t.subCon.Add1, t.subCon.Add2, t.subCon.Add3,
                                t.subCon.Add4, t.subCon.City); // 2014-10-29 Zhou Kai
                            newTrip.OwnTransport = t.subCon.Code == String.Empty;
                            newTrip.PartnerJobNo = outJob == null ? String.Empty : outJob.JobNo; // 2014-11-11 Zhou Kai 
                            //20150707 -gerry added to fix the gross weight bugs
                            newTrip.GrossWeight = t.GrossWeight; // t.GrossWeight;
                            newTrip.GrossCBM = t.GrossCBM == 0 ? container.TareCBM : sc.GrossCBM;
                            //ContainerTypesDTO ct = ContainerTypesDTO.GetContainerDTO(sc.ContainerCode);
                            newTrip.maxWeight = t.GrossWeight;
                            newTrip.maxCBM = t.GrossCBM == 0 ? container.TareCBM : sc.GrossCBM;
                            
                            allTrips.Add(newTrip);
                        }
                    }

                    nLegGroup++;
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return allTrips;
        }

        /// <summary>
        /// 2014-10-20 Zhou Kai. To create sample trips from stop-chain, and the container-code
        /// to quantity mapping for the given haulier job.
        /// </summary>
        /// <param name="hJob">The given haulier job</param>
        /// <param name="stopChian">The provided stop chain</param>
        /// <param name="conCodeToQty">The container code to quantity mapping</param>
        /// <returns>The sample trips</returns>
        public static List<HaulierJobTrip> CreateSampleTripsForStandaloneJob(HaulierJob hJob,
            List<Stop> stopChian, Dictionary<string, int> conCodeToQty, Dictionary<string, decimal> gwByContCode)
        {
            List<HaulierJobTrip> sampleTrips = new List<HaulierJobTrip>();
            try
            {
                foreach (KeyValuePair<string, int> kvp in conCodeToQty)
                {
                    decimal grossWt = gwByContCode[kvp.Key];
                    if (kvp.Key != String.Empty && kvp.Value > 0)
                    {
                        ContainerTypesDTO ct = ContainerTypesDTO.GetContainerDTO(kvp.Key.ToString());
                        // create a sample trip for each leg
                        for (int i = 0; i < stopChian.Count - 1; i++)
                        {
                            sampleTrips.Add(new HaulierJobTrip()
                            {
                                JobID = hJob.JobID,
                                JobNo = hJob.JobNo,
                                isBillable = true,
                                isMulti_leg = stopChian.Count > 2 ? true : false,
                                IsNew = true,
                                IsLaden = true,
                                StartStop = (Stop)stopChian[i].Clone(),
                                EndStop = (Stop)stopChian[i + 1].Clone(),
                                ContainerCode = kvp.Key,
                                OwnTransport = true,
                                LegType = stopChian.Count == 2 ? LegType.OneLeg :
                                   (i == 0 ? LegType.FirstOfTwoLeg : (i == 1 ? LegType.SecondOfTwoLeg :
                                    /*LegType.MoreThanTwoLegs*/ LegType.OneLeg)),
                                PartnerLeg = stopChian.Count == 2 ? 0 : (i == 0 ? 2 : (i == 1 ? 1 : 0)),
                                TripStatus = JobTripStatus.Booked,
                                CustomerCode = hJob.CustNo,
                                CustomerName = hJob.JobTrips.Count > 0 ? hJob.JobTrips[0].CustomerName : String.Empty,
                                jobType = hJob.JobType,
                                StartDate = DateTime.Today,
                                EndDate = DateTime.Today,
                                StartTime = "0000",
                                EndTime = "0000",
                                GrossWeight = grossWt == 0 ? ct.TareWeight : grossWt,
                                GrossCBM = ct.TareCBM,
                                maxWeight = grossWt == 0 ? ct.TareWeight : grossWt,
                                maxCBM = ct.TareCBM
                            }
                            );
                        }
                    }
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }

            return sampleTrips;
        }

        /// <summary>
        /// 2014-10-20 Zhou Kai, get the stops defined at sea-freight side.
        /// </summary>
        /// <param name="sJob">The sea job which is going to be transferred to haulage</param>
        /// <returns>The collection of predefined stops(in sequence depending on the job type)</returns>
        public static List<Stop> GetPreDefinedStopsFromSeaFreightSide(SeaJob sJob,  bool bHasTransferOutJob)
        {
            // carry on the stops defined  at seafreight side
            List<Stop> preDefinedStops = new List<Stop>();
            try
            {
                // 2014-11-11 Zhou Kai, only inherits one stop: the deliver to
                if (sJob is SeaExportJob)
                {
                    SeaExportJob se = (SeaExportJob)sJob;
                    if (!bHasTransferOutJob)
                    {
                        if (se.PickUpPlaceCode != String.Empty)
                        {
                            preDefinedStops.Add(new Stop()
                            {
                                Code = se.PickUpPlaceCode,
                                Description = se.PickUpPlaceName,
                                Address1 = se.PickUpPlaceAdd1,
                                Address2 = se.PickUpPlaceAdd2,
                                Address3 = se.PickUpPlaceAdd3,
                                Address4 = se.PickUpPlaceAdd4,
                                City = se.PickUpPlaceCityCode
                            });
                        }
                        if (sJob.StuffingCode != String.Empty)
                        {
                            preDefinedStops.Add(new Stop()
                            {
                                Code = sJob.StuffingCode,
                                Description = sJob.StuffingName,
                                Address1 = sJob.StuffingAdd1,
                                Address2 = sJob.StuffingAdd2,
                                Address3 = sJob.StuffingAdd3,
                                Address4 = sJob.StuffingAdd4,
                                City = sJob.StuffingCityCode
                            });
                        }
                    }
                    if (se.DeliveryCode != String.Empty)
                    {
                        preDefinedStops.Add(new Stop()
                        {
                            Code = se.DeliveryCode,
                            Description = se.DeliveryName,
                            Address1 = se.DeliveryAdd1,
                            Address2 = se.DeliveryAdd2,
                            Address3 = se.DeliveryAdd3,
                            Address4 = se.DeliveryAdd4,
                            City = se.DeliveryCityCode
                        });
                    }
                }
                if (sJob is SeaImportJob)
                {
                    SeaImportJob si = (SeaImportJob)sJob;

                    //20150805 - gerry removed the default port
                    // the stop of port is get from transport setting for SeaImport job
                    //OperatorDTO tmp =  OperatorDTO.GetOperatorDTO(TransportSettings.GetTransportSetting().DefaultPortCode);
                    //preDefinedStops.Add(new Stop()
                    //{
                    //    Code = tmp.Code,
                    //    Description = tmp.Name,
                    //    Address1 = tmp.Add1,
                    //    Address2 = tmp.Add2,
                    //    Address3 = tmp.Add3,
                    //    Address4 = tmp.Add4,
                    //    City = tmp.City
                    //});
                    if (si.PickUpPlaceCode != String.Empty)
                    {
                        preDefinedStops.Add(new Stop()
                        {
                            Code = si.PickUpPlaceCode,
                            Description = si.PickUpPlaceName,
                            Address1 = si.PickUpPlaceAdd1,
                            Address2 = si.PickUpPlaceAdd2,
                            Address3 = si.PickUpPlaceAdd3,
                            Address4 = si.PickUpPlaceAdd4,
                            City = si.PickUpPlaceCityCode
                        });
                    }
                    if (sJob.StuffingCode != String.Empty)
                    {
                        preDefinedStops.Add(new Stop()
                        {
                            Code = sJob.StuffingCode,
                            Description = sJob.StuffingName,
                            Address1 = sJob.StuffingAdd1,
                            Address2 = sJob.StuffingAdd2,
                            Address3 = sJob.StuffingAdd3,
                            Address4 = sJob.StuffingAdd4,
                            City = sJob.StuffingCityCode
                        });
                    }
                    if (sJob.DeliveryCode != string.Empty)
                    {
                        preDefinedStops.Add(new Stop()
                        {
                            Code = sJob.DeliveryCode,
                            Description = sJob.DeliveryName,
                            Address1 = sJob.DeliveryAdd1,
                            Address2 = sJob.DeliveryAdd2,
                            Address3 = sJob.DeliveryAdd3,
                            Address4 = sJob.DeliveryAdd4,
                            City = sJob.DeliveryCityCode
                        });
                    }
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return preDefinedStops;
        }

        #endregion

        #region UNION transfer Job
        //Get container numbers
        private static List<string> GetContainerNos(List<ContainerTypesDTO> containers)
        {
            List<string> contNos = new List<string>();
            try
            {
                foreach (ContainerTypesDTO temp in containers)
                    contNos.Add(temp.Code);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return contNos;
        }
        //Get job trip in the same container number
        private static SortableList<HaulierJobTrip> GetJobTripsInTheSameContainerNo(SortableList<HaulierJobTrip> partnerTrips, string containerNo)
        {
            SortableList<HaulierJobTrip> jobTripsInTheSameContainer = new SortableList<HaulierJobTrip>();
            try
            {
                foreach (HaulierJobTrip temp in partnerTrips)
                {
                    if (temp.ContainerNo.Equals(containerNo))
                        jobTripsInTheSameContainer.Add(temp);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return jobTripsInTheSameContainer;
        }

        //20141023 - gerry modified and changed parameters
        //containerNos = list of containerDTOs because we need the container size
        //additionalStops = list of addtional stops
        //sampleTrips = we assigned the isbillable and sub-con properties
        //201421101 - geryy added parameters isBillByTransport quotationNo
        public static bool TransferOutJobTripsToNewJob(HaulierJob oldHaulierJob, List<string> selectedContainerNos, List<Stop> additionalStops, SortableList<HaulierJobTrip> sampleTrips, string frmName, string userID, string quotationNo, bool isBillByTransport)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (selectedContainerNos.Count > 0 && additionalStops.Count > 0)
                {
                    //List<string> containerNos = GetContainerNos(containers); //get the container numbers
                    HaulierJob newHaulerJob = new HaulierJob(); //(HaulierJob)oldHaulierJob.MemberwiseClone();
                    //set some properties to new job based on old job
                    newHaulerJob.CustNo = oldHaulierJob.custNo;
                    newHaulerJob.quotationNo = quotationNo; //20141101 gerry added
                    newHaulerJob.BillByTransport = isBillByTransport;//20141101 gerry added
                    newHaulerJob.BranchCode = oldHaulierJob.BranchCode;
                    newHaulerJob.TptDeptCode = oldHaulierJob.TptDeptCode;
                    //20141029 - gerry removed assigned of vessel/vogaye are in the new job
                    //newHaulerJob.vesselNo = oldHaulierJob.vesselNo;
                    //newHaulerJob.voyageNo = oldHaulierJob.voyageNo;
                    newHaulerJob.haulierChargeType = oldHaulierJob.haulierChargeType;
                    newHaulerJob.jobType = oldHaulierJob.jobType;
                    newHaulerJob.IsBillable = true;
                    newHaulerJob.jobTransferType = JobTransferType.TransferIn;
                    newHaulerJob.PartnerJobNo = oldHaulierJob.JobNo;
                    foreach (HaulierJobTrip trip in oldHaulierJob.jobTrips)
                    {
                        foreach (string contNo in selectedContainerNos)
                        {
                            if (trip.ContainerNo.Equals(contNo))
                            {
                                newHaulerJob.PartnerJobTrips.Add((HaulierJobTrip)trip.Clone());
                                break;
                            }
                        }
                    }
                    //validate HaulierJob
                    if (newHaulerJob.ValidateAddHaulierJobHeader())   //job trip validation was transfered in the job trip loop
                    {
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        if (tran == null) { tran = con.BeginTransaction(); }
                        //TODO add new haulierJob

                        // 2014-10-31 Zhou Kai use DAL function to insert job header instead of BLL function
                        string prefix = HaulierJobDAL.GetPrefix();
                        HaulierJobDAL.AddHaulierHeader(newHaulerJob, con, tran, prefix, userID);
                        if (newHaulerJob.BillByTransport && newHaulerJob.IsBillable)
                            TransportFacadeOut.AddTptJobToSpecialJobDetail(newHaulerJob, con, tran, prefix);
                        foreach (HaulierJobTrip t in newHaulerJob.PartnerJobTrips)
                        { t.PartnerJobNo = newHaulerJob.JobNo; }
                        // 2014-10-31 Zhou Kai ends
                        //
                        //update parnerJobNo of the partnerlegs/oldtrips
                        HaulierJobDAL.UpdatePartnerTrips(oldHaulierJob.JobID, selectedContainerNos, newHaulerJob.JobNo, con, tran);
                        // 2014-10-31 Zhou Kai adds new in-memory variable: allNewTrips
                        //SortableList<HaulierJobTrip> allNewTrips = new SortableList<HaulierJobTrip>();//20141103 - gerry removed no need new object instead use newHualierJob.JobTrips
                        foreach (Stop stop in additionalStops)
                        {
                            foreach (string containerNo in selectedContainerNos)
                            {   //get the list of job trips based on conatinerNo or the legGroup(I used containerNo)
                                SortableList<HaulierJobTrip> tripsInSameContainerNo = GetJobTripsInTheSameContainerNo(newHaulerJob.PartnerJobTrips, containerNo);
                                //create haulier job trip
                                HaulierJobTrip newLegTrip = (HaulierJobTrip)tripsInSameContainerNo[0].Clone();//jobtrips is in descending order based on the sql query
                                newLegTrip.JobID = newHaulerJob.JobID;
                                newLegTrip.Sequence = 0; //auto increment in SP
                                newLegTrip.PartnerJobNo = newHaulerJob.PartnerJobNo;
                                newLegTrip.ContainerNo = containerNo;
                                newLegTrip.ContainerCode = ((HaulierJobTrip)tripsInSameContainerNo[0].Clone()).ContainerCode;
                                newLegTrip.LegGroup = ((HaulierJobTrip)tripsInSameContainerNo[0].Clone()).LegGroup;
                                newLegTrip.LegGroupMember = tripsInSameContainerNo.Count + 1;
                                newLegTrip.StartStop = ((HaulierJobTrip)tripsInSameContainerNo[0].Clone()).EndStop; //20141103 - gerry modified I used index 0 because the collection of job trips is in decsending order
                                newLegTrip.EndStop = stop;
                                newLegTrip.TripStatus = JobTripStatus.Booked;
                                newLegTrip.JobTripStates = new SortableList<JobTripState>();
                                //20141101 - gerry added
                                newLegTrip.LegType = LegType.OneLeg; //set to 1 leg
                                newLegTrip.isMulti_leg = true; //set to multileg because same container
                                //20141101 - gerry added
                                //20141104 -gerry modified to assigned the clone object
                                if (newHaulerJob.jobTrips.Count > 0)
                                {
                                    //20141028 - gerry modified to assign the start stop
                                    if (newHaulerJob.jobTrips[newHaulerJob.jobTrips.Count - 1].EndStop == stop)
                                    {
                                        if (newHaulerJob.jobTrips[newHaulerJob.jobTrips.Count - 1].StartStop != tripsInSameContainerNo[tripsInSameContainerNo.Count - 1].EndStop)
                                            newLegTrip.StartStop = ((HaulierJobTrip)newHaulerJob.jobTrips[newHaulerJob.jobTrips.Count - 1].Clone()).StartStop;
                                        else
                                            newLegTrip.StartStop = ((HaulierJobTrip)newHaulerJob.jobTrips[newHaulerJob.jobTrips.Count - 1].Clone()).EndStop;
                                    }
                                    else
                                    {
                                        newLegTrip.StartStop = ((HaulierJobTrip)newHaulerJob.jobTrips[newHaulerJob.jobTrips.Count - 1].Clone()).EndStop;
                                    }
                                    //get last legmember from the collection of new trips
                                    SortableList<HaulierJobTrip> newTripsSameConatiner = GetJobTripsInTheSameContainerNo(newHaulerJob.JobTrips, containerNo);
                                    if (newTripsSameConatiner.Count > 0)
                                    {
                                        if (newTripsSameConatiner[newTripsSameConatiner.Count - 1].LegGroup == newLegTrip.LegGroup)
                                            newLegTrip.LegGroupMember = ((HaulierJobTrip)newTripsSameConatiner[newTripsSameConatiner.Count - 1].Clone()).LegGroupMember + 1;
                                    }
                                }
                                //20141104
                                //20141029 - gerry added - get the container size of the new trip based on container code
                                string newTripContSize = TransportFacadeIn.GetContainerSizeByContainerCode(newLegTrip.ContainerCode);
                                //TODO loop to sampleTrips then asign subcon
                                foreach (HaulierJobTrip tempTrip in sampleTrips)
                                {
                                    //20141029 - gerry added - get the container size of the sample based on container code
                                    string tempTripContSize = TransportFacadeIn.GetContainerSizeByContainerCode(tempTrip.ContainerCode);
                                    //gerry modified the validation to assigned subcon and isbillable properties based container size regardless the containerNo
                                    if (tempTrip.EndStop.Code == stop.Code && tempTripContSize == newTripContSize)
                                    {
                                        newLegTrip.OwnTransport = tempTrip.OwnTransport;
                                        newLegTrip.isBillable = tempTrip.isBillable;
                                        newLegTrip.subCon = tempTrip.subCon;
                                        break;
                                    }
                                }
                                //end subcon & isbillable
                                // 2014-10-31 Zhou Kai insert all trips in one go, as well as calculate charges, write logs
                                //allNewTrips.Add(newLegTrip);//20141103 - gerry removed no need new object instead use newHualierJob.JobTrips

                                //TODO insert newLegTrip to database
                                // newHaulerJob.AddHaulierJobTrip(newLegTrip, frmName, userID, con, tran, true); //last parameter is optional parameter to check containerNO

                                //20141103 - gerry removed no need new object instead use newHualierJob.JobTrips
                                //and add quotation checking 1 more time
                                if (isBillByTransport && !quotationNo.Equals(string.Empty))
                                {
                                    Quotation quotation = Quotation.GetAllQuotationHeader(quotationNo);
                                    CheckValidQuotaionRates(quotation, newTripContSize, newLegTrip);
                                }
                                newHaulerJob.JobTrips.Add(newLegTrip);
                            }
                        }

                        // 2014-10-31 Zhou Kai insert all trips in one go, as well as calculate charges, write logs
                        // notice that service_charges are not transferred currently, logic to be clarified
                        //newHaulerJob.JobTrips = allNewTrips;
                        //AddHaulierJobTrips(newHaulerJob, allNewTrips, frmName, userID, con, tran);
                        // 2014-10-31 Zhou Kai ends

                        //20141103 - gerry removed no need new object(allNewTrips) instead use newHaulierJob.JobTrips
                        AddHaulierJobTrips(newHaulerJob, newHaulerJob.JobTrips, frmName, userID, con, tran);

                        tran.Commit();
                    }
                    else
                        throw new FMException(TptResourceBLL.ErrNoSelectedContainerToTransfer); //"Please select containers to be transferred and where is the next stop. ");
                }
            }
            catch (FMException fmEx)
            {
                if (tran != null) { tran.Rollback(); }
                throw fmEx;
            }
            catch (Exception ex)
            {
                if (tran != null) { tran.Rollback(); }
                throw new FMException(ex.Message.ToString());
            }
            finally { con.Close(); }
            return true;
        }

        public List<string> GetCompletedContainers()
        {
            List<string> containerNos = new List<string>();
            try
            {
                //get job trips by containers
                foreach (HaulierJobTrip groupTrip in this.jobTrips)
                {
                    if (!containerNos.Contains(groupTrip.ContainerNo))
                        containerNos.Add(groupTrip.ContainerNo);
                }
                List<string> notCompletedContainers = new List<string>();
                //after having all containerNos now loop each container and check if all jobtrips are completed
                //20141029 - gerry modified no need to combined with partner trips
                //we already have the partner job number
                //SortableList<HaulierJobTrip> partnerTrips = HaulierJobDAL.GetPartnerTripsForInJobFromOutJob(this.JobNo);
                //List<HaulierJobTrip> combinedTrips = this.jobTrips.Union(partnerTrips).OrderBy(jt => jt.LegGroup).ThenBy(jt => jt.LegGroupMember).ToList();
                foreach (string containerNo in containerNos)
                {
                    foreach (HaulierJobTrip tempTrip in this.jobTrips)
                    {
                        if (containerNo.Equals(tempTrip.ContainerNo))
                        {
                            if (tempTrip.TripStatus != JobTripStatus.Completed || tempTrip.PartnerJobNo != "")
                            {
                                notCompletedContainers.Add(tempTrip.ContainerNo);
                                break;
                            }
                        }
                    }
                }
                //20141028 end
                //now remove from containerNos collection those container have not yet completed job
                foreach (string cont in notCompletedContainers)
                    containerNos.Remove(cont);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return containerNos;
        }

        //20141101 - gerry added 2 parameter(isbillByTransport and quotNo) to check quotation
        public static SortableList<HaulierJobTrip> AddSampleJobTrips(HaulierJob oldHaulierJob, List<string> containerNos, List<Stop> additionalStops, bool isBillByTransport, string quotNo)
        {
            SortableList<HaulierJobTrip> sampleTrips = new SortableList<HaulierJobTrip>();
            try
            {
                if (containerNos.Count > 0 && additionalStops.Count > 0)
                {
                    Quotation quotation = Quotation.GetAllQuotationHeader(quotNo);
                    //20141101 end
                    foreach (Stop stop in additionalStops)
                    {
                        List<string> containerSizes = new List<string>();
                        HaulierJobTrip sampleTrip = null;
                        foreach (string contNo in containerNos)
                        {
                            //get trips in the same container number
                            SortableList<HaulierJobTrip> tripsInSameContainerNo = GetJobTripsInTheSameContainerNo(oldHaulierJob.jobTrips, contNo);
                            //get container size
                            string contSize = TransportFacadeIn.GetContainerSizeByContainerCode(tripsInSameContainerNo[0].ContainerCode);
                            //create sample job trip
                            Stop defaultStartStop = new Stop("Unknow", " From previous leg of the container", "", "", "", "", "");
                            sampleTrip = (HaulierJobTrip)tripsInSameContainerNo[0].Clone(); // just to get other properties
                            sampleTrip.StartStop = defaultStartStop;
                            sampleTrip.EndStop = stop;
                            sampleTrip.isBillable = true;
                            //20141101 - gerry added
                            sampleTrip.LegType = LegType.OneLeg; //set to 1 leg
                            sampleTrip.isMulti_leg = true; //set to multileg because same container
                            //20141101 - gerry added
                            //for display the size in the sample trips grid I use remark field
                            sampleTrip.Remarks = contSize;
                            if (sampleTrips.Count > 0)
                            {
                                //20141027 - gerry modified to assign the start stop
                                if (sampleTrips[sampleTrips.Count - 1].EndStop == stop)
                                {
                                    if (sampleTrips[sampleTrips.Count - 1].StartStop != defaultStartStop)
                                        sampleTrip.StartStop = sampleTrips[sampleTrips.Count - 1].StartStop;
                                    else
                                        sampleTrip.StartStop = sampleTrips[sampleTrips.Count - 1].EndStop;
                                }
                                else
                                {
                                    sampleTrip.StartStop = sampleTrips[sampleTrips.Count - 1].EndStop;
                                }
                                //20141027 end
                            }
                            //add to collect of container sizes
                            if (!containerSizes.Contains(contSize))
                            {
                                //20141101 - gery added to check the quotation
                                if (isBillByTransport && !quotNo.Equals(string.Empty))
                                {
                                    //first check the old trip if there is valid rate
                                    CheckValidQuotaionRates(quotation, contSize, tripsInSameContainerNo[0]);
                                    //2nd check the sample trip or the additional trip if there is valid rate
                                    CheckValidQuotaionRates(quotation, contSize, sampleTrip);
                                }
                                //20141101 end
                                sampleTrips.Add(sampleTrip);
                                containerSizes.Add(contSize);
                            }
                        }
                    }
                }
                else
                    throw new FMException(TptResourceBLL.ErrNoSelectedContainerToTransfer);//"Please select containers to be transferred and where is the next stop. ");
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return sampleTrips;
        }
        private static void CheckValidQuotaionRates(Quotation quotation, string contSize, HaulierJobTrip sampleTrip)
        {
            try
            {
                //20141101 - gery added to check the quotation
                //20141103 - gerry modified find("", "") function not functioning
                List<TransportRate> validRates = quotation.TransportRates.Where(r => r.UOM == contSize).ToList();
                //if (quotation.TransportRates.Find("UOM", contSize) == 0)
                //    throw new FMException(string.Format(TptResourceBLL.ErrNoValidRateForSize, contSize));

                if (validRates.Count == 0)
                    throw new FMException(string.Format(TptResourceBLL.ErrNoValidRateForSize, contSize));
                else
                {
                    if (sampleTrip.LegType != LegType.OneLeg)
                    {
                        List<TransportRate> rates = validRates.Where(r => r.UOM == contSize && r.NoOfLeg == 2).ToList();
                        if (rates.Count == 0) //no round trip rate
                            throw new FMException(string.Format(TptResourceBLL.ErrNo2LegRateForSize, contSize));
                    }
                    else
                    {
                        List<TransportRate> rates = validRates.Where(r => r.UOM == contSize && r.NoOfLeg == 1).ToList();
                        if (rates.Count == 0) //no single trip rate
                            throw new FMException(string.Format(TptResourceBLL.ErrNo1LegRateForSize, contSize));
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        #endregion

        #region "Zhou Kai, service_charge and additional logic after deleting whole haulier job"
        /// <summary>
        /// 2014-10-30 Zhou Kai.
        /// This function is used to transfer service charges from the transfer_in job to the transfer_out job.
        /// Pay attention that this function CAN'T be used, the logic need clarification.
        /// </summary>
        /// <param name="outJobId"></param>
        /// <param name="inJobId"></param>
        private void TransferServiceCharges(int outJobId, int inJobId)
        {
            HaulierJobDAL.TransferServiceCharges(outJobId, inJobId);
        }

        /// <summary>
        /// 2014-10-30 Zhou Kai.
        /// This function is used to transfer service charges from the transfer_in job to the transfer_out job.
        /// Pay attention that this function CAN'T be used, the logic need clarification.
        /// </summary>
        /// <param name="outJobId"></param>
        /// <param name="inJobId"></param>
        private void TransferServiceCharges(int outJobId, int inJobId, SqlConnection dbCon,
            SqlTransaction dbTran)
        {
            HaulierJobDAL.TransferServiceCharges(outJobId, inJobId, dbCon, dbTran);
        }

        /// <summary>
        /// 2014-10-31 Zhou Kai.
        /// This function is used to erase the partner_job_no of a transferred_in job, when the corresponding
        /// transferred_out job is deleted.
        /// </summary>
        /// <param name="inJobNo">The number of the transferred in job</param>
        /// <param name="outJobNo">The number of the deleted transferred out job</param>
        /// <returns>true: succeeded, false: failed</returns>
        private bool ErasePartnerJobNo(string inJobNo, string outJobNo, SqlConnection dbCon,
            SqlTransaction dbTran)
        {
            return HaulierJobDAL.ErasePartnerJobNo(inJobNo, outJobNo, dbCon, dbTran);
        }

        /// <summary>
        /// 2014-10-31 Zhou Kai.
        /// This function is used to erase the partner_job_no of a transferred_in job, when the corresponding
        /// transferred_out job is deleted.
        /// </summary>
        /// <param name="inJobNo">The number of the transferred in job</param>
        /// <param name="outJobNo">The number of the deleted transferred out job</param>
        /// <returns>true: succeeded, false: failed</returns>
        private bool ErasePartnerJobNo(string inJobNo, string outJobNo)
        {
            return HaulierJobDAL.ErasePartnerJobNo(inJobNo, outJobNo);
        }

        #endregion

        #region "2014-11-11 Zhou Kai"
        /// <summary>
        /// 2014-11-11 Zhou Kai.
        /// Get the leg_group in the transferOut haulier job for each container number.
        /// </summary>
        /// <param name="seaJob">the Sea-Export job, containing all containers(with container_numbers)
        /// </param>
        /// <param name="transferOutJob">the transfer out haulier job</param>
        /// <returns>the mapping of container_number to leg_group</returns>
        public static Dictionary<string, int> GetLegGroupForTransferInJob(SeaJob seaJob,
            HaulierJob transferOutJob)
        {
            Dictionary<string, int> rtn = new Dictionary<string, int>();
            List<HaulierJobTrip> tripsLeft = new List<HaulierJobTrip>();
            foreach (HaulierJobTrip t in transferOutJob.JobTrips)
            {
                if (t.PartnerJobNo == String.Empty) { tripsLeft.Add(t); }
            }
            string err = String.Empty;
            foreach (SeaJobContainer sjc in seaJob.SeaJobContainers)
            {
                //if (sjc.ContainerNumber == String.Empty)
                //{ throw new FMException(TptResourceBLL.ErrSomeConNoNotFilled); }
                try
                {
                    Debug.WriteLine(sjc.ContainerNumber);
                    var transferContainer = tripsLeft.Where(x => x.ContainerNo == sjc.ContainerNumber && HaulierJob.containerCodeSizeMapping[x.ContainerCode] == sjc.ContainerSize).ToList();
                    // 2014-11-21 Zhou Kai adds additional condition checking: containerSize must match
                    if (transferContainer.Count > 0)
                    {
                        rtn.Add(sjc.ContainerNumber, ((HaulierJobTrip)transferContainer[0]).LegGroup);
                        //rtn.Add(sjc.ContainerNumber, tripsLeft.First(x => x.ContainerNo == sjc.ContainerNumber &&
                        //                       HaulierJob.containerCodeSizeMapping[x.ContainerCode] == sjc.ContainerSize).LegGroup);
                        // 2014-11-21 Zhou Kai ends
                    }
                }
                catch (ArgumentNullException ane) { err += " " + sjc.ContainerNumber + ","; }
                catch (InvalidOperationException ioe) { err += " " + sjc.ContainerNumber + ","; }
            }
            if (err != String.Empty)
            {
                throw new FMException(String.Format(TptResourceBLL.ContainerNoNoInOutJob,
                                     err) + Environment.NewLine);
            }

            return rtn;
        }

        public static bool UpdatePartnerJobNos(int outJobId, List<string> containerNos, string inJobNo,
            SqlConnection dbCon, SqlTransaction dbTran)
        {
            // update the partner_job_no of the transferred out trips of the transfer out job
            return HaulierJobDAL.UpdatePartnerTrips(outJobId, containerNos, inJobNo, dbCon, dbTran);
        }

        public static List<string> GetAllTransferOutJobNo()
        {
            return HaulierJobDAL.GetAllTransferOutJobNo();
        }

        public static List<string> GetAllTransferOutJobNo(string customerCode)
        {
            return HaulierJobDAL.GetAllTransferOutJobNo(customerCode);
        }


        #endregion

        /// <summary>
        /// 2014-12-04 Zhou Kai.
        /// Get all haulier jobs whose booking date is between startDate and endDate
        /// </summary>
        /// <param name="startDate">The earlist booking date</param>
        /// <param name="endDate">The latest booking date</param>
        /// <returns>All haulier jobs whose booking date is between startDate and endDate</returns>
        public static List<HaulierJob> GetHaulierJobsByDate(DateTime startDate, DateTime endDate)
        {
            return HaulierJobDAL.GetHaulierJobsByDate(startDate, endDate);
        }

        public static List<HaulierJob> GetHaulierJobsByDate(DateTime startDate, DateTime endDate,
            SqlConnection dbCon, SqlTransaction dbTran)
        {
            return HaulierJobDAL.GetHaulierJobsByDate(startDate, endDate, dbCon, dbTran);
        }

        public static List<string> GetAllHaulierJobSourceRefNo()
        {
            return HaulierJobDAL.GetAllHaulierJobSourceRefNo();
        }

        public bool CanUpdateLocationFromHeader(out string warnMsg)
        {
            warnMsg = string.Empty;
            try
            {
                //check if there is assigned or complete trips
                var listAssignedOrCompleteTrips = this.JobTrips.Where(jt => jt.TripStatus == JobTripStatus.Assigned || jt.TripStatus == JobTripStatus.Completed).ToList();
                if (listAssignedOrCompleteTrips.Count > 0)
                {
                    warnMsg = "Some containers are already assigned or completed. ";
                    return false;
                }
                //check if there is more than 2 legs trips
                var listMoreThan2LegsTrips = this.JobTrips.Where(jt => jt.LegGroupMember == 3).ToList();
                if (listMoreThan2LegsTrips.Count > 0)
                {
                    warnMsg = "Some container have more than 2 legs. ";
                    return false;
                }
                //check if there is container with not multiple leg
                var listSingleLegTrip = this.JobTrips.Where(jt => !jt.isMulti_leg).ToList();
                if (listSingleLegTrip.Count > 0)
                {
                    warnMsg = "Some container is only single leg. ";
                    return false;
                }
                //check if there is container with different location for first legs
                var list1stLegTripsStartLocations = this.JobTrips.Where(jt => jt.isMulti_leg && jt.LegGroupMember == 1).GroupBy(jt => jt.StartStop.Code).ToList();
                var list1stLegTripsEndLocations = this.JobTrips.Where(jt => jt.isMulti_leg && jt.LegGroupMember == 1).GroupBy(jt => jt.EndStop.Code).ToList();
                if (list1stLegTripsStartLocations.Count > 1 || list1stLegTripsEndLocations.Count > 1)
                {
                    warnMsg = "Some container first leg have different locations. ";
                    return false;                
                }
                var list2ndLegTripStartLocations = this.JobTrips.Where(jt => jt.LegGroupMember == 2).GroupBy(jt => jt.StartStop.Code).ToList();
                var list2ndLegTripEndLocations = this.JobTrips.Where(jt => jt.LegGroupMember == 2).GroupBy(jt => jt.EndStop.Code).ToList();
                if (list2ndLegTripStartLocations.Count > 1 || list2ndLegTripEndLocations.Count > 1)
                {
                    warnMsg = "Some container first leg have different locations. ";
                    return false;
                }

                //Dictionary<int, string> startLocations = new Dictionary<int, string>();
                //Dictionary<int, string> endLocations = new Dictionary<int, string>();
                //foreach (HaulierJobTrip trip in list1stLegTrips)
                //{
                //    startLocations.Add(trip.LegGroup, trip.StartStop.Code);
                //    endLocations.Add(trip.LegGroup, trip.EndStop.Code);
                //}
                //if (startLocations.Count != startLocations.Distinct().Count() || endLocations.Count != endLocations.Distinct().Count())
                //{
                //    warnMsg = "Some container first leg have different locations. ";
                //    return false;
                //}
                ////check if there is container with different location for 2nd legs
                //var list2ndLegTrips = this.JobTrips.Where(jt => jt.LegGroupMember == 2).ToList();
                //startLocations = new Dictionary<int, string>();
                //endLocations = new Dictionary<int, string>();
                //foreach (HaulierJobTrip trip in list2ndLegTrips)
                //{
                //    startLocations.Add(trip.LegGroup, trip.StartStop.Code);
                //    endLocations.Add(trip.LegGroup, trip.EndStop.Code);
                //}
                //if (startLocations.Count != startLocations.Distinct().Count() || endLocations.Count != endLocations.Distinct().Count())
                //{
                //    warnMsg = "Some container 2nd leg have different locations. ";
                //    return false;
                //}
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return true;
        }
    }
}
