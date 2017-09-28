using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_HLBookDLL.DAL;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
//using FM.TransportBook.Resources;
using FM.TR_MaintenanceDLL.BLL;
using TR_LanguageResource.Resources;
using System.Data;
using System.Collections;
using FM.TR_MarketDLL.BLL;
using FM.TR_SeaFreightDLL.BLL;
using System.Linq;
//
using System.Runtime.Serialization;
using FM.TR_HLBookDLL.Facade;

namespace FM.TR_HLBookDLL.BLL
{
    public class HaulierJobTrip : JobTrip, ICloneable, IComparable
    {
        public const string LOGDETAIL_CONTAINER_NO = "CONTAINER NUMBER";
        public const string LOGDETAIL_SEAL_NO = "SEAL NUMBER";

        private string containerNo;
        private string sealNo;
        private decimal grossWeight;
        private decimal grossCBM;
        private bool isLaden;
        private string containerCode;
        // 2014-08-13 Zhou Kai adds, it depends on container code, and it's 
        // read only
        private string containerSize; 
        private string cargoDescription;
        private bool isDangerousGoods;
        private string dGRemarks;
        private bool isOversize;
        private string oversizeRemarks;
        private bool isDirectDelivery;
        private Leg_Type legType;
        private int partnerLeg;
        //20120323 - add
        private string portRemark;
        private string yard;
        public string permitNo { get; set; }
        public string vesselNo { get; set; }
        public string vesselName { get; set; }
        public string voyageNo { get; set; }
        //20120813
        public string oldPermitNo { get; set; }
        //20120813 end
        public decimal maxCBM { get; set; }
        public decimal maxWeight { get; set; }

        public bool isMulti_leg { get; set; }
        //20130417
        public string oldContainerNo { get; set; }
        public string oldSealNo { get; set; }
        //20130417end
        //20140120 - Gerry added
        public string oblNo { get; set; }
        //end 20140120
        public bool isBillable { get; set; } // 2014-07-25 Zhou Kai adds
        //20150620 - gerry added to display in Planning
        public string BookRefNo { get; set; }
        public string SourceRefNo { get; set; }
        //20150620 end
        //20161017
        public bool Processed { get; set; }
        public bool Released { get; set; }
        public bool ESN { get; set; }

        #region "Properties for Union Changes" 
        // 2014-10-04 Zhou Kai adds.
        // You need to set partnerJobNo first, then
        // legGroup, then legGroupMember.
        private int legGroup;
        public int LegGroup
        { get { return legGroup; }
            set
            {
                legGroup = value; 
            }
        }
        public void SetLegGroup(int n)
        { this.legGroup = n; }
        private int legGroupMember;
        public int LegGroupMember
        { get { return legGroupMember; }
            internal set
            {
                if (legGroup == -1)
                {
                    throw new FMException(
                        "Please set legGroup first."
                        );
                }
                else { legGroupMember = value; }
            }
        }
        public void SetLegGroupMember(int n)
        { this.legGroupMember = n; }

        // The PartnerJob is set only when creating the
        // trip, so the setter is private.
        public string PartnerJobNo { get; internal set; }
        public void SetPartnerJobNo (string s)
        { this.PartnerJobNo = s; }

        private string legTypeCustomized; // don't use this field, it's customized only for USS
        public string LegTypeCustomized
        {
            get { return legTypeCustomized; }
            set { legTypeCustomized = value; }
        }
        #endregion

        public HaulierJobTrip()
            : base()
        {
            this.containerNo = string.Empty;
            this.sealNo = string.Empty;
            this.grossWeight = 0;
            this.grossCBM = 0;
            this.isLaden = false;
            this.containerCode = string.Empty;
            this.cargoDescription = string.Empty;
            this.maxCBM = 0;
            this.maxWeight = 0;
            this.isDangerousGoods = false;
            this.dGRemarks = string.Empty;
            this.isOversize = false;
            this.oversizeRemarks = string.Empty;
            this.isDirectDelivery = false;
            this.legType = Leg_Type.OneLeg;
            this.partnerLeg = 0;
            this.portRemark = string.Empty;
            this.yard = string.Empty;
            this.oldPermitNo = string.Empty;
            this.isMulti_leg = false;
            this.oldContainerNo = string.Empty;
            this.oldSealNo = string.Empty;
            this.isBillable = false; // 2014-07-25 Zhou Kai adds
            this.containerSize = String.Empty; // 2014-08-13 Zhou Kai adds

            // 2014-10-04 Zhou Kai adds
            this.PartnerJobNo = String.Empty;
            this.legGroup = -1;
            this.legGroupMember = -1;
            // 2014-10-04 Zhou Kai ends
            this.Processed = false;
            this.Released = false;
            this.ESN = false;
        }
        //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
        public HaulierJobTrip(int jobID, int sequence, DateTime startDate, Stop startStop, DateTime endDate, Stop endStop,
            string startTime, string endTime, JobTripStatus status, SortableList<JobTripState> jobTripStates, byte[] updateVersion, bool isNew,
            bool ownTransport, OperatorDTO subContractorCode, string subContractorReference, string chargeCode, string remarks,
            string containerNo, string sealNo, decimal grossWeight, decimal grossCBM, bool isLaden,
            string containerCode, string cargoDescription, decimal maxCBM, decimal maxWeight,
            bool isDangerousGoods, string dGRemarks, bool isOversize, string oversizeRemarks,
            bool isDirectDelivery, Leg_Type legType, int partnerLeg, string portRemark, string yard
            /*2014-07-25 Zhou Kai adds*/, bool isBillable
            // 2014-10-04 Zhou Kai adds
            , string partnerJobNo
            , int legGroup
            , int legGroupMember
            // 2014-10-04 Zhou Kai ends
            )
            : base(jobID, sequence, startDate, startStop, endDate, endStop, startTime, endTime, status,
            jobTripStates, updateVersion, isNew, ownTransport,
            subContractorCode, subContractorReference, chargeCode, remarks)
        {
            this.ContainerCode = containerCode;
            this.SealNo = sealNo;
            this.GrossWeight = grossWeight;
            this.GrossCBM = grossCBM;
            this.IsLaden = isLaden;
            this.ContainerNo = containerNo;
            this.CargoDescription = cargoDescription;
            this.maxCBM = maxCBM;
            this.maxWeight = maxWeight;
            this.isDangerousGoods = isDangerousGoods;
            this.dGRemarks = dGRemarks;
            this.isOversize = isOversize;
            this.oversizeRemarks = oversizeRemarks;
            this.isDirectDelivery = isDirectDelivery;
            this.LegType = legType;
            this.PartnerLeg = partnerLeg;
            this.PortRemark = portRemark;
            this.Yard = yard;
            this.oldContainerNo = containerNo;
            this.oldSealNo = sealNo;
            this.isBillable = isBillable;
            SetContainerSizeFromCode(); // "2014-08-13 Zhou Kai adds"

            // 2014-10-04 Zhou Kai adds
            this.PartnerJobNo = partnerJobNo;
            this.legGroup = legGroup;
            this.legGroupMember = legGroupMember;
            // 2014-10-04 Zhou Kai ends
            this.Processed = false;
            this.Released = false;
            this.ESN = false;
        }
        /// <summary>
        /// 2014-08-13 Zhou Kai adds this function, the container size property of
        /// this class is private and read only, the only way to set it is via this
        /// function.
        /// </summary>
        public void SetContainerSizeFromCode()
        {
            if (HaulierJob.containerCodeSizeMapping.Count == 0)
            {
                HaulierJob.containerCodeSizeMapping =
                    TransportFacadeIn.GetContainerCodeSizeMapping();
                containerSize =
                    HaulierJob.containerCodeSizeMapping[containerCode];
            }
            else
            {
                containerSize =
                    HaulierJob.containerCodeSizeMapping[containerCode];
            }
        }

        public string PortRemark
        {
            get { return portRemark; }
            set { portRemark = value; }
        }
        public string Yard
        {
            get { return yard; }
            set { yard = value; }
        }

        public string CargoDescription
        {
            get { return cargoDescription; }
            set { cargoDescription = value; }
        }

        public string ContainerCode
        {
            get { return containerCode; }
            // 2014-08-13 Zhou Kai adds logic to update containerSize when the 
            // containerCode is changed.
            set 
            {
                containerCode = value;
                SetContainerSizeFromCode();
            }
            // 2014-08-13 Zhou Kai ends
        }

        public bool IsLaden
        {
            get { return isLaden; }
            set { isLaden = value; }
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

        public string SealNo
        {
            get { return sealNo; }
            set { sealNo = value; }
        }

        public string ContainerNo
        {
            get { return containerNo; }
            set
            {
                value = value.Trim();
                containerNo = value;
            }
        }

        public bool IsDangerousGoods
        {
            get { return isDangerousGoods; }
            set { isDangerousGoods = value; }
        }

        public string DGRemarks
        {
            get { return dGRemarks; }
            set { dGRemarks = value; }
        }

        public bool IsOversize
        {
            get { return isOversize; }
            set { isOversize = value; }
        }

        public string OversizeRemarks
        {
            get { return oversizeRemarks; }
            set { oversizeRemarks = value; }
        }

        public bool IsDirectDelivery
        {
            get { return isDirectDelivery; }
            set { isDirectDelivery = value; }
        }


        public Leg_Type LegType
        {
            get { return legType; }
            set { legType = value; }
        }

        public int PartnerLeg
        {
            get { return partnerLeg; }
            set { partnerLeg = value; }
        }


        public static SortableList<HaulierJobTrip> GetAllHaulierJobTrips(HaulierJob haulierJob)
        {
            return HaulierJobDAL.GetAllHaulierJobTrips(haulierJob);

        }

        public static SortableList<HaulierJobTrip> GetAllReadyHaulierJobTrips()
        {
            return HaulierJobDAL.GetAllReadyHaulierJobTrips();
        }
        public static SortableList<HaulierJobTrip> GetAllJobTripInReadyStatusByDept(string deptCode, DateTime scheduleDate, bool ownTransport)
        {
            return HaulierJobDAL.GetAllJobTripInReadyStatusByDept(deptCode, scheduleDate, ownTransport);
        }
        public static SortableList<HaulierJobTrip> GetAllJobTripInReadyStatusByDept(string deptCode, DateTime scheduleDate, bool ownTransport,
                            string jobNo, string containerNo, string sourceRefNo, string rowCount, DateTime? advanceJobDate)
        {
            return HaulierJobDAL.GetAllJobTripInReadyStatusByDept(deptCode, scheduleDate, ownTransport, jobNo, containerNo, sourceRefNo, rowCount, advanceJobDate);
        }

        public static SortableList<HaulierJobTrip> GetAllReadyHaulierJobTripsAndNotSubCon()
        {
            return HaulierJobDAL.GetAllReadyHaulierJobTripsAndNotSubCon();
        }

        public static SortableList<HaulierJobTrip> GetAllHaulierJobTrips()
        {
            return HaulierJobDAL.GetAllHaulierJobTrips();
        }

        // 2014-12-01 Zhou Kai adds the version with dbCon and dbTran
        public static SortableList<HaulierJobTrip> GetAllHaulierJobTrips(HaulierJob hJob,
            SqlConnection dbCon, SqlTransaction dbTran)
        {
            return HaulierJobDAL.GetAllHaulierJobTrips(hJob, dbCon, dbTran);
        }

        public static HaulierJobTrip GetHaulierJobTrip(int jobid, int sequence)
        {
            return HaulierJobDAL.GetHaulierJobTrip(jobid, sequence);
        }
        public static HaulierJobTrip GetHaulierJobTrip(int jobid, int sequence, SqlConnection con, SqlTransaction tran)
        {
            return HaulierJobDAL.GetHaulierJobTrip(jobid, sequence, con, tran);
        }



        //20141024 - gerry added optional parameter for the shifting jobs
        public bool ValidateAddHaulierJobTrip(bool isShiftingJob = false)
        {
            string error = "";
            bool status = true;

            if (OwnTransport == false)
            {
                if (subCon.Code == "")
                {
                    error += TptResourceBLL.ErrSubContractorBlank;
                    status = false;
                }
            }
            else
            {
                //SubContractorCode = "";   //20140312 - gerry replaced    
                //201040502 - gerry change subContractorCode from CustomerDTO to OperatorDTO
                if (subCon == null) { subCon = new OperatorDTO(); }//new CustomerDTO(); // 2014-10-30 Zhou Kai
            }
            //20140708 - gerry added validation   containerCode
            if (containerCode.Length == 0 && this.isBillable)
            {
                error += TptResourceBLL.ErrContainerCodeBlank;
                status = false;
            }
            if (StartTime == "")
            {
                error += TptResourceBLL.ErrStartTimeBlank;
                status = false;
            }
            if (EndTime == "")
            {
                error += TptResourceBLL.ErrEndTimeBlank;
                status = false;
            }
            if (StartDate.CompareTo(EndDate) == 0)
            {
                if ((StartTime != "") && (EndTime != ""))
                {
                    if ((StartTime != "0000") && (EndTime != "0000"))
                    {
                        if (Convert.ToInt32(StartTime) >= Convert.ToInt32(EndTime))
                        {
                            error += TptResourceBLL.ErrStartTimenEndTime;
                            status = false;
                        }
                    }
                }
            }
            else
            {
                if (StartDate.CompareTo(EndDate) > 0)
                {
                    error += TptResourceBLL.ErrStartDatenEndDate;
                    status = false;
                }
            }

            if (grossWeight > maxWeight)
            {
                error += TptResourceBLL.ErrGrossWeightExceedMax;
                status = false;
            }

            if (grossCBM > maxCBM)
            {
                error += TptResourceBLL.ErrGrossCBMExceedMaxCBM;
                status = false;
            }
            //20141023 -gerry added isShifting
            if (!isShiftingJob && String.Compare(StartStop.Code, EndStop.Code) == 0)
            {
                error += TptResourceBLL.ErrStartStopnEndStop;
                status = false;
            }
            if (StartStop.Code == string.Empty || EndStop.Code == string.Empty)
            {
                error += TptResourceBLL.ErrStopBlank;
                status = false;
            }

            if (status == false) { throw new FMException(error); }



            return status;
        }
        //20141024 - gerry added optional parameter for the shifting jobs
        public bool ValidateEditHaulierJobTrip(bool isShiftingJob = false)
        {
            string error = "";
            bool status = true;

            if (OwnTransport == false)
            {
                if (subCon.Code == "")
                {
                    error += TptResourceBLL.ErrSubContractorBlank;
                    status = false;
                }

            }
            else
            {
                //SubContractorCode = "";   //20140312 - gerry replaced   
                //201040502 - gerry change subCon from CustomerDTO to operatorDTO
                // subCon = new OperatorDTO(); //new CustomerDTO(); // *******************2014-05-23 Zhou Kai comments out this line
            }

            /**
            if ((containerNo.Length > 0) && (containerNo.Length != 11))
            {
                error += ResHaulierJob.ErrorContainerNoLessThan11Char;
                status = false;
            }
             **/
            if (TripStatus == JobTripStatus.Completed && this.PartnerJobNo.Equals(string.Empty))
            {
                error += TptResourceBLL.ErrCannotEditBecauseJobTripAlreadyComplete;
                status = false;
            }
            //20141023 -gerry added isShifting
            if (!isShiftingJob && String.Compare(StartStop.Code, EndStop.Code) == 0)
            {
                error += TptResourceBLL.ErrStartStopnEndStop;
                status = false;
            } 
            if (StartStop.Code == string.Empty || EndStop.Code == string.Empty)
            {
                error += TptResourceBLL.ErrStopBlank;
                status = false;
            }
            if (grossWeight > maxWeight)
            {
                error += TptResourceBLL.ErrGrossWeightExceedMax;
                status = false;
            }

            if (grossCBM > maxCBM)
            {
                error += TptResourceBLL.ErrGrossCBMExceedMaxCBM;
                status = false;
            }

            if (status == false)
            {
                throw new FMException(error);
            }
            return status;

        }


        public bool ValidateDeleteHaulierJobTrip()
        {
            bool status = true;
            #region Removed 20140214
            /*
            if (TripStatus == JobTripStatus.Assigned)
            {
                error += TptResourceBLL.ErrCantDeleteBecauseJobTripAlreadyAssigned;
                status = false;
            }
            if (TripStatus == JobTripStatus.Completed)
            {
                error += TptResourceBLL.ErrCannotEditBecauseJobTripAlreadyComplete;
                status = false;
            }
            if (status == false)
            {
                throw new FMException(error);
            }
            */
            #endregion
            //if (TripStatus != JobTripStatus.Booked)
            if (isMovementJob)
            {
                if ((TripStatus == JobTripStatus.Assigned) || (TripStatus == JobTripStatus.Completed))
                {
                    throw new FMException("Assigned and Completed jobs are not allowed to delete. ");
                }
            }
            return status;
        }

        //Renamed From UpdateContainerNo
        //add parameter planJobTrips
        public bool UpdateHaulierJobTripFromPlanning(string formName, string userID, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                bool update = HaulierJobDAL.UpdateHaulierJobTripFromPlanning(this, cn, tran);
                this.UpdateVersion = GetHaulierJobTripUpdateVersion(cn,tran);     

                return update;
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool UpdateSubContractor(SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                return HaulierJobDAL.UpdateSubContractor(this, cn, tran);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Deleted Methods
        #region Un-Necessary Methods
        //Feb. 14, 2011 Remove by Gerry - no need to create this methods
        //You can directly call this methods because it was inherited from the base class

        //public bool AddJobTripState(JobTripState jobTripState)
        //{
        //    return base.AddJobTripState(jobTripState);
        //}

        //public bool AddJobTripState(JobTripState jobTripState, SqlConnection cn, SqlTransaction tran)
        //{
        //    try
        //    {
        //        return base.AddJobTripState(jobTripState, cn, tran);
        //    }
        //    catch (FMException ex)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public bool EditJobTripState(JobTripState jobTripState)
        //{
        //    return base.EditJobTripState(jobTripState);
        //}

        //public bool DeleteJobTripState(JobTripState jobTripState)
        //{
        //    return base.DeleteJobTripState(jobTripState);
        //}

        //public bool DeleteJobTripState(JobTripState jobTripState, SqlConnection cn, SqlTransaction tran)
        //{
        //    return base.DeleteJobTripState(jobTripState, cn, tran);
        //}
        #endregion

        // method to copy to another HaulierJobTrip
        // purpose is for the Planning Module where the old and new HaulierJobTrips have to be compared
        public object Clone()
        {
            HaulierJobTrip newHaulierJobTrip = new HaulierJobTrip();
            // only 3 properties are created to save time
            newHaulierJobTrip.JobID = this.JobID;
            newHaulierJobTrip.JobNo = this.JobNo;
            newHaulierJobTrip.Sequence = this.Sequence;
            newHaulierJobTrip.ContainerCode = this.ContainerCode;
            newHaulierJobTrip.TripStatus = this.TripStatus;
            newHaulierJobTrip.ContainerNo = this.ContainerNo;
            newHaulierJobTrip.sealNo = this.SealNo;
            newHaulierJobTrip.isMulti_leg = this.isMulti_leg;
            //end 20140213
            // 2014-10-15 Zhou Kai modifies / adds
            newHaulierJobTrip.StartStop = (Stop)this.StartStop.Clone();
            newHaulierJobTrip.EndStop = (Stop)this.EndStop.Clone();
            newHaulierJobTrip.isBillable = this.isBillable;
            newHaulierJobTrip.isLaden = this.isLaden;
            newHaulierJobTrip.LegType = this.LegType;
            newHaulierJobTrip.CustomerCode = this.CustomerCode;
            newHaulierJobTrip.CustomerName = this.CustomerName;
            newHaulierJobTrip.jobType = this.jobType;
            newHaulierJobTrip.StartDate = this.StartDate;
            newHaulierJobTrip.StartDate = this.EndDate;
            newHaulierJobTrip.legGroup = this.legGroup;
            newHaulierJobTrip.legGroupMember = this.legGroupMember;
            // 2014-10-15 Zhou Kai ends
            // 2014-10-16 Zhou Kai deep-clone the JobTripStates property
            // newHaulierJobTrip.JobTripStates = this.JobTripStates;   //need to clone the job states
            SortableList<JobTripState> newStates = new SortableList<JobTripState>();
            foreach (JobTripState jts in this.JobTripStates) { newStates.Add((JobTripState)jts.Clone()); }
            newHaulierJobTrip.JobTripStates = newStates;
            // 2014-10-16 Zhou Kai ends
            newHaulierJobTrip.UpdateVersion = this.UpdateVersion; //need to clone the updated version for the old job trips in planning
            newHaulierJobTrip.isMovementJob = this.isMovementJob; //20150805 - gerry added
            //20151214
            newHaulierJobTrip.BookRefNo = this.BookRefNo;
            newHaulierJobTrip.SourceRefNo = this.SourceRefNo;
            newHaulierJobTrip.legTypeCustomized = this.legTypeCustomized;
            newHaulierJobTrip.permitNo = this.permitNo;
            newHaulierJobTrip.Yard = this.yard;
            newHaulierJobTrip.PortRemark = this.portRemark;
            newHaulierJobTrip.vesselName = this.vesselName;
            newHaulierJobTrip.vesselNo = this.vesselNo;
            newHaulierJobTrip.voyageNo = this.voyageNo;
            newHaulierJobTrip.PartnerJobNo = this.PartnerJobNo;

            return newHaulierJobTrip;
        }

        public static SortableList<HaulierJobTrip> GetSubconJobTrips(SortableList<HaulierJobTrip> haulierJobTrips, bool flag)
        {
            SortableList<HaulierJobTrip> h = new SortableList<HaulierJobTrip>();

            for (int i = 0; i < haulierJobTrips.Count; i++)
            {
                if (haulierJobTrips[i].OwnTransport == flag)
                {
                    h.Add(haulierJobTrips[i]);
                }
            }
            return h;
        }

        public static SortableList<HaulierJobTrip> GetJobTrips(JobTripStatus status)
        {
            return HaulierJobDAL.GetJobTrips(status);
        }

        public static SortableList<HaulierJobTrip> GetOwnBookJobTrips(JobTripStatus status, DateTime date1, string tptDeptCode)
        {
            return HaulierJobDAL.GetOwnBookJobTrips(status, date1, tptDeptCode);
        }

        public static SortableList<HaulierJobTrip> GetJobTrips(int startJobID, int endJobID)
        {
            return HaulierJobDAL.GetJobTrips(startJobID, endJobID);
        }

        public static SortableList<HaulierJobTrip> GetJobTrips(string startJobNo, string endJobNo, JobTripStatus status, string tptDept)
        {
            return HaulierJobDAL.GetJobTrips(startJobNo, endJobNo, status, tptDept);
        }

        public static SortableList<HaulierJobTrip> GetJobTrips(DateTime startJobDate, DateTime endJobDate, JobTripStatus status, string tptDept)
        {
            return HaulierJobDAL.GetJobTrips(startJobDate, endJobDate, status, tptDept);
        }

        public static SortableList<HaulierJobTrip> GetJobTrips(DateTime startJobDate, DateTime endJobDate)
        {
            return HaulierJobDAL.GetJobTrips(startJobDate, endJobDate);
        }

        public static SortableList<HaulierJobTrip> GetJobTrips(bool ownTransport)
        {
            return HaulierJobDAL.GetJobTrips(ownTransport);
        }

        // Chong Chin 9 Dec 09 - add in parameter, planDate. Only job trips on or before this date
        // should be returned to planning *** Start
        public static SortableList<HaulierJobTrip> GetJobTrips(JobTripStatus status, bool owntransport, string tptDeptCode, DateTime planDate, string jobType)
        {
            return HaulierJobDAL.GetJobTrips(status, owntransport, tptDeptCode, planDate, jobType);
            //************ 19 Dec 09 ** End ***************************
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsSetBookedToReady(string jobType, string deptCode, DateTime startDate, DateTime endDate)
        {
            //20130516 - Gerry modify this method to hide other remaining legs if the same container number
            SortableList<HaulierJobTrip> retValue = new SortableList<HaulierJobTrip>();
            try
            {
                SortableList<HaulierJobTrip> list = HaulierJobDAL.GetHaulierJobTripsSetBookedToReady(jobType, deptCode, startDate, endDate);
                foreach (HaulierJobTrip tempJobTrip in list)
                {
                    if (!HaulierJobDAL.IsHaulierJobTripHasPendingPreviousLeg(tempJobTrip))
                    {
                        retValue.Add(tempJobTrip);
                    }
                    //bool found = false;
                    //if (retValue.Count > 0)
                    //{
                    //    for (int i = 0; i < retValue.Count; i++)
                    //    {
                    //        //if (retValue[i].containerNo.Equals(tempJobTrip.containerNo))
                    //        //{
                    //            if (HaulierJobDAL.IsHaulierJobTripHasPendingPreviousLeg(tempJobTrip))
                    //            {
                    //                found = true;
                    //                break;
                    //            }
                    //        //}
                    //    }
                    //}
                    //if (!found)
                    //    retValue.Add(tempJobTrip);
                }
            }
            catch (FMException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }

        public byte[] GetHaulierJobTripUpdateVersion(SqlConnection cn, SqlTransaction tran)
        {
            return HaulierJobDAL.GetHaulierJobTripUpdateVersion(this, cn, tran);
        }

        public byte[] GetHaulierJobTripUpdateVersion()
        {
            byte[] version = null;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                version = HaulierJobDAL.GetHaulierJobTripUpdateVersion(this, cn, tran);
            }
            catch (Exception ex)
            { throw ex; }

            cn.Close();
            return version;
        }

        public bool UpdateContainerAndSealNoForSecondLeg(string entryFormPrefix, string userID, string frmName, SqlConnection con, SqlTransaction tran)
        {
            if (legType == Leg_Type.FirstOfTwoLeg)
            {
                bool temp = false;
                try
                {
                    temp = HaulierJobDAL.UpdateContainerSealNoForLegsOfSameLegGroup(this, con, tran);
                    this.UpdateVersion = GetHaulierJobTripUpdateVersion(con, tran);

                    HaulierJobTrip nextLegTrip = HaulierJobDAL.GetNextLegJobTrip(this.JobID, this.Sequence, this.legGroup, con, tran);
                    //20151102 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.JobNo, "TR", entryFormPrefix, this.JobID, userID, DateTime.Now, frmName, this.legGroup, "EDT");
                    auditLog.WriteAuditLog("ContainerNo", this.containerNo.ToString(), nextLegTrip.containerNo.ToString(), con, tran);
                    auditLog.WriteAuditLog("SealNo", this.sealNo.ToString(), nextLegTrip.sealNo.ToString(), con, tran);
                    auditLog.WriteAuditLog("ContainerCode", this.containerCode.ToString(), nextLegTrip.containerCode.ToString(), con, tran);
                }
                catch (FMException e)
                {
                    throw e;
                }
                catch (Exception ex)
                {
                    throw new FMException(ex.ToString());
                }

            }
            return true;
        }

        /// <summary>
        /// 2014-08-27 Zhou Kai adds, to update the partner leg of this leg, of the 
        /// IsBillable and ContainerCode properties.
        /// </summary>
        /// <param name="frmName"></param>
        /// <param name="cn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool UpdateBillableConCodeForPartnerTrip(string frmName, SqlConnection cn, SqlTransaction tran)
        {
                bool temp = false;
                try
                {
                    temp = HaulierJobDAL.UpdateBillableNConCodeForPartnerLeg(this, cn, tran);
                    this.UpdateVersion = GetHaulierJobTripUpdateVersion(cn, tran);
                }
                catch (FMException e) { throw e; }
                catch (Exception ex) { throw new FMException(ex.ToString()); }

            return true;
        }

        //20130417 - gerry added
        public bool UpdateContainerSealNoForRemainingLeg(string entryFormPrefix, string userID, string frmName, SqlConnection con, SqlTransaction tran)
        {
            //20160513 - gerry allow to update container and seal number in any legs
            //if (legType == LegType.FirstOfTwoLeg)
            //{
            bool temp = true;
            try
            {
                temp = HaulierJobDAL.UpdateContainerSealNoForAllRemainingLeg(this, con, tran);
                this.UpdateVersion = GetHaulierJobTripUpdateVersion(con, tran);

                HaulierJobTrip nextLegTrip = HaulierJobDAL.GetNextLegJobTrip(this.JobID, this.Sequence, this.legGroup, con, tran);
                AuditLog auditLog = new AuditLog(this.JobNo, "TR", entryFormPrefix, this.JobID, userID, DateTime.Now, frmName, this.legGroup, "EDT");
                auditLog.WriteAuditLog("ContainerNo", this.containerNo.ToString(), nextLegTrip.containerNo.ToString(), con, tran);
                auditLog.WriteAuditLog("SealNo", this.sealNo.ToString(), nextLegTrip.sealNo.ToString(), con, tran);
                auditLog.WriteAuditLog("ContainerCode", this.containerCode.ToString(), nextLegTrip.containerCode.ToString(), con, tran);
            }
            catch (FMException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }

            //}
            return temp;
        }
        public static bool SetAllJobTripsFromBookToReady(HaulierJobTrip haulierJobTrip, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                JobTripState jobTripState = new JobTripState();
                jobTripState.Seq_No = 1;
                jobTripState.Status = JobTripStatus.Ready;
                jobTripState.StatusDate = DateTime.Now;
                jobTripState.IsNew = true;

                haulierJobTrip.AddJobTripState(jobTripState, con, tran);
                return true;
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }

        public static bool SetAllJobTripsFromBookToReady(SortableList<HaulierJobTrip> haulierJobTrips)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                JobTripState jobTripState = new JobTripState();
                jobTripState.Seq_No = 1;
                jobTripState.Status = JobTripStatus.Ready;
                jobTripState.StatusDate = DateTime.Now;
                jobTripState.IsNew = true;


                for (int i = 0; i < haulierJobTrips.Count; i++)
                {
                    haulierJobTrips[i].AddJobTripState(jobTripState, con, tran);


                }
                tran.Commit();
                return true;
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

        //Feb 14, 2011 - Gerry Added Methods to Validate change job trip status and filter criteria
        #region
        public bool CanChangeStatus(JobTripStatus oldTripStatus, JobTripStatus newTripStatus, 
            bool ownTransport, out string message)
        {
            bool validationCheck = true;
            message = "";
            if (ownTransport)
            {
                if (oldTripStatus == JobTripStatus.Booked)
                {
                    if (newTripStatus != JobTripStatus.Ready)
                    {
                        message = TptResourceBLL.ErrCanSetOnlyToReady;
                        validationCheck = false;
                    }
                }
                else if (oldTripStatus == JobTripStatus.Ready)
                {
                    if (newTripStatus != JobTripStatus.Booked)
                    {
                        message = TptResourceBLL.ErrCanSetOnlyToBooked;
                        validationCheck = false;
                    }
                }
                else
                {
                    message = TptResourceBLL.ErrOwnTransCanSetBookedOrReady;
                    validationCheck = false;
                }
            }
            else if(!ownTransport) // 2014-11-17 Zhou Kai adds
            {
                //allow to change status for subcon job trip
                bool hasPrevLeg = this.legGroupMember!=1;
                JobTripStatus prevLegStatus = JobTripStatus.Completed;
                if (hasPrevLeg)
                {
                    HaulierJobTrip prevLeg = this.GetPreviousLegTrip();
                    prevLegStatus = prevLeg.TripStatus;
                }
                ValidateChangingSubConTripStatus(hasPrevLeg, prevLegStatus, oldTripStatus, newTripStatus);
                #region removed
                //if (oldTripStatus == JobTripStatus.Booked)
                //{
                //    if (newTripStatus != JobTripStatus.Ready)
                //    {
                //        message = TptResourceBLL.ErrCanSetOnlyToReady;
                //        validationCheck = false;
                //    }
                //}
                //else if (oldTripStatus == JobTripStatus.Ready)
                //{
                //    //20130416 - gerry change the condition
                //    //if (newTripStatus != JobTripStatus.Assigned || newTripStatus != JobTripStatus.Completed)
                //    if (newTripStatus == JobTripStatus.Booked || newTripStatus == JobTripStatus.Ready)
                //    {
                //        message = TptResourceBLL.ErrCanSetOnlyToAssignOrComplete;
                //        validationCheck = false;
                //    }
                //}
                //else if (oldTripStatus == JobTripStatus.Assigned)
                //{
                //    if (newTripStatus != JobTripStatus.Completed)
                //    {
                //        message = TptResourceBLL.ErrCanSetOnlyToComplete;
                //        validationCheck = false;
                //    }
                //}
                #endregion
            }
            return validationCheck;
        }

        #region Methods For Criteria
        //Start      
        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status)
        {
            return HaulierJobDAL.GetHaulierJobTripsByStatus(status);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(bool ownTransport)
        {
            return HaulierJobDAL.GetHaulierJobTripsByTransport(ownTransport);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, bool ownTransport)
        {
            return HaulierJobDAL.GetHaulierJobTripsByStatusAndTransport(status, ownTransport);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, bool ownTransport, string deptCode, string jobNoFrom, string jobNoTo)
        {
            return HaulierJobDAL.GetHaulierJobTrips(status, ownTransport, jobNoFrom, jobNoTo, deptCode);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, string deptCode, string jobNoFrom, string jobNoTo)
        {
            return HaulierJobDAL.GetHaulierJobTrips(status, jobNoFrom, jobNoTo, deptCode);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, bool ownTransport, string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return HaulierJobDAL.GetHaulierJobTrips(status, ownTransport, planDateFrom, planDateTo, deptCode);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return HaulierJobDAL.GetHaulierJobTrips(status, planDateFrom, planDateTo, deptCode);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(JobTripStatus status, string deptCode, bool ownTransport)
        {
            return HaulierJobDAL.GetHaulierJobTrips(status, deptCode, ownTransport);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTrips(string deptCode, bool ownTransport)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDeptAndTransport(deptCode, ownTransport);
        }
        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByDeptAndStatus(JobTripStatus status, string deptCode)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDeptAndStatus(status, deptCode);
        }
        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByDept(string deptCode)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDept(deptCode);
        }
        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByJobs(string deptCode, string jobIDFrom, string jobIDTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByJob(deptCode, jobIDFrom, jobIDTo);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByJobs(string jobIDFrom, string jobIDTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByJob(jobIDFrom, jobIDTo);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByJobs(bool ownTransport, string jobIDFrom, string jobIDTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByJob(ownTransport, jobIDFrom, jobIDTo);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByJobs(bool ownTransport, string deptCode, string jobNoFrom, string jobNoTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByJob(ownTransport, jobNoFrom, jobNoTo, deptCode);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(DateTime jobDateFrom, DateTime jobDateTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDate(jobDateFrom, jobDateTo);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(JobTripStatus status, DateTime jobDateFrom, DateTime jobDateTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDate(status, jobDateFrom, jobDateTo);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDate(ownTransport, jobDateFrom, jobDateTo);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(JobTripStatus status, bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDate(status, ownTransport, jobDateFrom, jobDateTo);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDate(deptCode, planDateFrom, planDateTo);
        }
        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByDate(bool onwTransport, string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return HaulierJobDAL.GetHaulierJobTripsByDate(onwTransport, deptCode, planDateFrom, planDateTo);
        }
        //end for criteria
        #endregion


        #endregion
        //20130522 - Gerry Added method to refresh jobtrips in planning
        public static SortableList<HaulierJobTrip> RefreshHaulierJobTripReadyStatus(SortableList<HaulierJobTrip> jobTrips, string jobType)
        {
            SortableList<HaulierJobTrip> retValue = new SortableList<HaulierJobTrip>();
            try
            {
                if (!jobType.Equals(string.Empty))
                {
                    foreach (HaulierJobTrip tempJobTrip in jobTrips)
                    {
                        if (jobType.ToString().Trim().Equals(tempJobTrip.jobType.ToString().Trim()))
                        {
                            retValue.Add(tempJobTrip);
                        }
                    }
                }
                else
                    retValue = jobTrips;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }


            return retValue;
        }
        //end 20130522
        //20150824 - gerry added
        public static bool IsContainerNoFreeToUse(string containerNo, int jobID, int legGroup)
        {
            return HaulierJobDAL.IsContainerNoFreeToUse(containerNo, jobID, legGroup);
        }
        public static bool IsContainerNoFreeToUse(string containerNo, int jobID, int legGroup, SqlConnection cn, SqlTransaction tran)
        {
            return HaulierJobDAL.IsContainerNoFreeToUse(containerNo, jobID, legGroup, cn, tran);
        }
        public static bool IsSealNoFreeToUse(string sealNo, int jobID, int legGroup)
        {
            return HaulierJobDAL.IsSealNoFreeToUse(sealNo, jobID, legGroup);
        }
        public static bool IsSealNoFreeToUse(string sealNo, int jobID, int legGroup, SqlConnection cn, SqlTransaction tran)
        {
            return HaulierJobDAL.IsSealNoFreeToUse(sealNo, jobID, legGroup, cn, tran);
        }
        //end 20150824
        public static SortableList<HaulierJobTrip> GetTwoLegsHaulierJobTrips(int jobID)
        {
            return HaulierJobDAL.GetTwoLegsHaulierJobTrips(jobID);
        }

        public static SortableList<HaulierJobTrip> GetOneLeghaulierJobTrips(int jobID)
        {
            return HaulierJobDAL.GetOneLegHaulierJobTrips(jobID);
        }

        public static SortableList<HaulierJobTrip> GetHaulierJobTripsByContainerNo(int jobID, string contNo, bool sorted)
        {
            //List<HaulierJobTrip> list = HaulierJobDAL.GetHaulierJobTripsByContainerNo(jobID, contNo);
            //SortByContainerSeqNoAsc sortByContSeqNo = new SortByContainerSeqNoAsc();
            //list.Sort(sortByContSeqNo);
            //return list
            return HaulierJobDAL.GetHaulierJobTripsByContainerNo(jobID, contNo, sorted);
        }
        //update from booking
        public bool UpdatePortAndYardFromBooking()
        {
            bool retValue = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                retValue = HaulierJobDAL.UpdatePortAndYard(this, con, tran);

                tran.Commit();
                this.UpdateVersion = GetHaulierJobTripUpdateVersion();

            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            catch (Exception fmEx)
            {
                tran.Rollback();
                throw fmEx;
            }
            finally { con.Close(); }
            return retValue;
        }

        public void LogJobTripChangesFromPlanning(FMModule fmModule, string formName, FormMode formMode, string userName, SqlConnection cn, SqlTransaction trans)
        {
            try
            {
                DateTime logDateTime = Logger.GetServerDateTime();
                //20140110 - gerry modified logging, replaced jobId to jobNo
                LogHeader objLogHeader = new LogHeader(fmModule, formName, logDateTime,/*this.JobID.ToString()*/ this.JobNo.ToString(), this.Sequence.ToString(), formMode, userName);
                LogDetail objLogDetail0 = new LogDetail(LOGDETAIL_CONTAINER_NO, this.containerNo.ToString());
                LogDetail objLogDetail1 = new LogDetail(LOGDETAIL_SEAL_NO, this.sealNo.ToString());
                LogDetail objLogDetail2 = new LogDetail(HaulierJob.LOGDETAIL_START_STOP, this.StartStop.Code.ToString());
                LogDetail objLogDetail3 = new LogDetail(HaulierJob.LOGDETAIL_END_STOP, this.EndStop.Code.ToString());
                objLogHeader.LogDetails.Add(objLogDetail0);
                objLogHeader.LogDetails.Add(objLogDetail1);
                objLogHeader.LogDetails.Add(objLogDetail2);
                objLogHeader.LogDetails.Add(objLogDetail3);
                Logger.WriteLog(objLogHeader, cn, trans);
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
        //Sub Class for custom sorting
        #region Sub Class for custom sorting
        public class SortByContainerSeqNoAsc : IComparer<HaulierJobTrip>
        {
            #region IComparer<HaulierJobTrip> Members

            public int Compare(HaulierJobTrip jt1, HaulierJobTrip jt2)
            {
                return string.Compare(jt1.ContainerNo, jt2.ContainerNo);
            }

            #endregion
        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion


        // 2013-12-02 Zhou Kai adds methods to get the status record list for a haulier job trip
        public List<JobTripState> GetHaulierJobTripStatusHistory(int jobId, int jobTripSeqNo)
        {
            return HaulierJobDAL.GetHaulierJobTripStatusHistory(jobId, jobTripSeqNo);
        }
        // 2013-12-02 Zhou Kai ends

        // 2013-12-24 Zhou Kai adds
        public static List<string> GetAllHaulageJobTripSeqNoByJobNumber(string jobNumber)
        {
            List<string> arrListHaulierJobTripNo = new List<string>();
            SortableList<HaulierJobTrip> haulierJobTrips = new SortableList<HaulierJobTrip>();
            haulierJobTrips = HaulierJobTrip.GetHaulierJobTripsByJobs(jobNumber, jobNumber);
            foreach (HaulierJobTrip hjt in haulierJobTrips)
            {
                arrListHaulierJobTripNo.Add(hjt.Sequence.ToString());
            }

            return arrListHaulierJobTripNo;
        }
        // 2013-12-24 Zhou Kai ends

       

        #region "Gerry's functions"
        //20140124 - Gerry added to get the previous job trip(last leg)
        public static HaulierJobTrip GetPreviousLegJobTrip(int jobID, string containerNo)
        {
            return HaulierJobDAL.GetPreviousLegJobTrip(jobID, containerNo);
        }
        //20140520 - Gerry overload
        //this method is being from planning to validate the sub contractor assignement
        public static HaulierJobTrip GetPreviousLegJobTrip(int jobID, int seqNo, string containerNo)
        {
            return HaulierJobDAL.GetPreviousLegJobTrip(jobID, seqNo, containerNo);
        }
        //20141124 - Gerry overload
        //to get the previous job trip including the transfer out job
        public static HaulierJobTrip GetPreviousLegJobTrip(int jobID, string containerNo, int legGroupMember)
        {
            return HaulierJobDAL.GetPreviousLegJobTrip(jobID, containerNo,legGroupMember);
        }

        //20140521 - gerry added to get jobNos for subcontractor jobs
        public static ArrayList GetSubContractedHaulierTripJobNo(int status)
        {
            return HaulierJobDAL.GetSubContractedHaulierTripJobNo(status);
        }
        //20140521 - gerry added to get jobtrip seqNos for job id
        public static ArrayList GetSubContractedHaulierTripSeqNo(int jobId, int status)
        {
            return HaulierJobDAL.GetSubContractedHaulierTripSeqNo(jobId, status);
        }
        //20151201 - gerry added to cater the previous leg of the hubbing job
        public HaulierJobTrip GetPreviousLegJobTrip()
        {
            return HaulierJobDAL.GetPreviousLegJobTrip(this);
        }

        ///20140609 - gerry added separate method
        ///this is called from Planning Main menu to change sub contractor
        ///in the specs need an Operator parameter which is not necessary the it is an instance method, 
        ///we can use the subCon property
        public bool ChangeSubContractorFromPlanning()
        {
            if (ValidateAddSubContractorFromPlanning())
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                SqlTransaction tran = null;
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    HaulierJobDAL.AddorChangeSubContractorFromPlanning(this, con, tran);
                    //get update version
                    this.UpdateVersion = HaulierJobDAL.GetHaulierJobTripUpdateVersion(this, con, tran);
                    //we only add job trip state when sub contractor was set to blank
                    //because status will be changed either Booked or Ready
                    if (this.subCon.Code.Equals(string.Empty))
                    {
                        //add jobtrip state
                        JobTripState jobTripState = new JobTripState(1, this.TripStatus, DateTime.Now, "Added from planning.", true);
                        this.AddJobTripState(jobTripState, con, tran);
                    }
                    tran.Commit();
                }
                catch (FMException fmEx)
                {
                    tran.Rollback();
                    throw fmEx;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new FMException(ex.ToString());
                }
                return true;
            }
            else
                return false;
        }

        ///20140609 - gerry renamed method from AddOrChangeSubContractorFromPlanning
        ///and added separate method to Change SubContractorFromPlanning.   
        ///In the specs need an Operator parameter which is not necessary the it is an instance method, 
        ///we can use the subCon property.
        public bool AddSubContractorFromPlanning(SqlConnection con, SqlTransaction tran)
        {
            if (ValidateAddSubContractorFromPlanning())
            {
                if (con == null) { con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()); }
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    if (tran == null) { tran = con.BeginTransaction(); }
                    HaulierJobDAL.AddorChangeSubContractorFromPlanning(this, con, tran);
                    //get update version
                    this.UpdateVersion = HaulierJobDAL.GetHaulierJobTripUpdateVersion(this, con, tran);
                    //add jobtrip state
                    JobTripState jobTripState = new JobTripState(1, this.TripStatus, DateTime.Now, "Added from planning. ", true);
                    this.AddJobTripState(jobTripState, con, tran);
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (Exception ex) { throw new FMException(ex.ToString()); }
                return true;
            }
            else
                return false;
        }
        //20140521 - gerry added to validate adding subcon from planning
        public bool ValidateAddSubContractorFromPlanning()
        {
            if (this.subCon.Code != string.Empty)
            {
                if (this.TripStatus != JobTripStatus.Assigned)
                    throw new FMException(TptResourceBLL.ErrSetSubconTripNotAssignedStatus); //"The job trip was being assigned to a subcon the status must be ASSIGNED. ");
            }
            return true;
        }
        #endregion

        //20140620 - gerry added to set next leg status to ready after completing the previous leg
        #region gerry removed this method and combined to UpdateNextLegStartStopAndStatus
        /*
        public void SetNextLegToReady(SqlConnection con, SqlTransaction tran)
        {
            try
            {
                HaulierJobTrip nextLegTrip = HaulierJobDAL.GetNextLegJobTrip(this.JobID, this.Sequence, this.containerNo, con, tran);
                //Set status for the next leg if any
                if (nextLegTrip.JobID != 0 && nextLegTrip.Sequence != 0 &&
                    nextLegTrip.TripStatus == JobTripStatus.Booked)
                {
                    HaulierJobDAL.SetNextLegToReady(nextLegTrip, con, tran);
                    //get update version
                    nextLegTrip.UpdateVersion = nextLegTrip.GetHaulierJobTripUpdateVersion(con, tran);
                    //add jobtrip state to ready
                    JobTripState jobTripState = new JobTripState(1, JobTripStatus.Ready, DateTime.Now, "Auto set from planning", true);
                    nextLegTrip.AddJobTripState(jobTripState, con, tran);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
         */ 
        #endregion

        //20140729 - gerry added to set next leg trip start stop
        public void UpdateNextLegStartStopAndStatus(bool autoSetNextLegToReady, string entryFormPrefix, string userID, string frmName, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
                //AND CHANGE PARAMETER
                //HaulierJobTrip nextLegTrip = HaulierJobDAL.GetNextLegJobTrip(this, con, tran);
                HaulierJobTrip nextLegTrip = HaulierJobDAL.GetNextLegJobTrip(this.JobID, this.Sequence, this.legGroup/*this.containerNo*/, con, tran);
                if (this.EndStop.Code != nextLegTrip.EndStop.Code)
                {
                    //Set start stop for the next leg if any
                    if (nextLegTrip.JobID != 0 && nextLegTrip.Sequence != 0)
                    {
                        string oldStartStopCode = nextLegTrip.EndStop.Code;
                        string oldStatus = nextLegTrip.TripStatus.ToString();
                        HaulierJobDAL.UpdateNextLegStartStopAndStatus(this.TripStatus, this, nextLegTrip, autoSetNextLegToReady, con, tran);
                        //20151102 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(nextLegTrip.JobNo, "TR", entryFormPrefix, nextLegTrip.JobID, userID, DateTime.Now, frmName, nextLegTrip.Sequence, "EDT");
                        auditLog.WriteAuditLog("ContainerNo", this.containerNo.ToString(), nextLegTrip.containerNo.ToString(), con, tran);
                        auditLog.WriteAuditLog("StartStop", this.EndStop.Code.ToString(), oldStartStopCode.ToString(), con, tran);
                        auditLog.WriteAuditLog("TripStatus", JobTripStatus.Ready.ToString(), oldStatus.ToString(), con, tran);
                    }
                }
                else
                    throw new FMException("NextLegTrip " + TptResourceBLL.ErrStartStopnEndStop);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        } 
        //20140729 - gerry added to set next leg trip start stop
        public void UpdateNextLegToReadyStatus(bool autoSetNextLegToReady, string entryFormPrefix, string userID, string frmName, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                //20141106 - GERRY MODIFIED QUERY BECAUSE IT WILL FAIL IF ALL HAS NO CONTAINER NO YET AND 1 LEG
                //AND CHANGE PARAMETER
                //HaulierJobTrip nextLegTrip = HaulierJobDAL.GetNextLegJobTrip(this, con, tran);
                HaulierJobTrip nextLegTrip = HaulierJobDAL.GetNextLegJobTrip(this.JobID, this.Sequence, this.legGroup/*this.containerNo*/, con, tran);
                if (this.EndStop.Code != nextLegTrip.EndStop.Code)
                {
                    //Set start stop for the next leg if any
                    if (nextLegTrip.JobID != 0 && nextLegTrip.Sequence != 0)
                    {
                        string oldStartStopCode = nextLegTrip.EndStop.Code;
                        string oldStatus = nextLegTrip.TripStatus.ToString();
                        HaulierJobDAL.UpdateNextLegStartStopAndStatus(this.TripStatus, this, nextLegTrip, autoSetNextLegToReady, con, tran);
                        //20151102 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(nextLegTrip.JobNo, "TR", entryFormPrefix, nextLegTrip.JobID, userID, DateTime.Now, frmName, nextLegTrip.Sequence, "EDT");
                        auditLog.WriteAuditLog("ContainerNo", this.containerNo.ToString(), nextLegTrip.containerNo.ToString(), con, tran);
                        auditLog.WriteAuditLog("StartStop", this.EndStop.Code.ToString(), oldStartStopCode.ToString(), con, tran);
                        auditLog.WriteAuditLog("TripStatus", JobTripStatus.Ready.ToString(), oldStatus.ToString(), con, tran);
                    }
                }
                else
                    throw new FMException("NextLegTrip " + TptResourceBLL.ErrStartStopnEndStop);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        public void UpdatePrevLegEndStop(string entryFormPrefix, string userID, string frmName, SqlConnection con, SqlTransaction tran)
        {
            HaulierJobTrip prevLegTrip = HaulierJobDAL.GetPreviousLegJobTrip(this, con, tran); //GetPreviousLegJobTrip(this.JobID, this.Sequence, this.legGroup, con, tran);
            if (prevLegTrip.TripStatus == JobTripStatus.Booked || prevLegTrip.TripStatus == JobTripStatus.Ready)
            {
                if (this.StartStop.Code != prevLegTrip.StartStop.Code)
                {
                    string oldEndStopCode = prevLegTrip.EndStop.Code;
                    //Set end stop for the previous leg if any
                    if (prevLegTrip.JobID != 0 && prevLegTrip.Sequence != 0)
                    {
                        HaulierJobDAL.UpdatePreviousLegEndStop(this, prevLegTrip, con, tran);

                        //20151102 - gerry added new audit log
                        AuditLog auditLog = new AuditLog(prevLegTrip.JobNo, "TR", entryFormPrefix, prevLegTrip.JobID, userID, DateTime.Now, frmName, prevLegTrip.Sequence, "EDT");
                        auditLog.WriteAuditLog("ContainerNo", this.containerNo.ToString(), prevLegTrip.containerNo.ToString(), con, tran);
                        auditLog.WriteAuditLog("EndStop", this.StartStop.Code.ToString(), oldEndStopCode.ToString(), con, tran);
                    }
                }
                else
                    throw new FMException("PrevLegTrip " + TptResourceBLL.ErrStartStopnEndStop);
            }
        }
       
        //20140829 - gerry added method to get nextLegTrip
        public HaulierJobTrip GetNextLegTrip()
        {
            return HaulierJobDAL.GetNextLegJobTrip(this.JobID, this.Sequence, this.legGroup);
        }
        public HaulierJobTrip GetPreviousLegTrip()
        {
            return HaulierJobDAL.GetPreviousLegJobTrip(this.JobID, this.legGroupMember, this.legGroup);
        }
        /// <summary>
        /// 2014-07-25 Zhou Kai adds this function to:
        /// Get all valid UOMs for a haulier job trip going to be added.
        /// The haulier job trip to be added should have its properties: customerCode, isStopDependent,
        /// bookingDateTime and LegType set.
        /// Based on those properties, this function returns all valid UOMs.
        /// </summary>
        /// <param name="customerCode">for whom the quotation is</param>
        /// <param name="dateTime">the booking date of this job(trip)</param>
        /// <param name="legType">the leg type of this job trip</param>
        /// <returns>a HashSet contains valid uoms</returns>
        public static HashSet<string> GetHaulierUOMFromQuotation(string customerCode,
            bool isStopDependent, DateTime dateTime, Leg_Type legType)
        {
            HashSet<string> setUOMs = new HashSet<string>();
            string message = String.Empty;
            string quotationNo = Quotation.GetValidQuotationNo(customerCode, dateTime, out message);
            if (quotationNo.Equals(String.Empty)) { return setUOMs; }

            int noOfLegs = 0;
            if (legType == Leg_Type.OneLeg) { noOfLegs = 1; }
            if (legType == Leg_Type.FirstOfTwoLeg) { noOfLegs = 2; }
            if (legType == Leg_Type.SecondOfTwoLeg) { noOfLegs = 2; }
            SortableList<TransportRate> lstRates = Quotation.GetTransportRatesBasedOnQuotationNo(quotationNo);
            foreach (TransportRate tr in lstRates)
            {
                if (tr.IsStopDependent == isStopDependent &&
                    tr.NoOfLeg == noOfLegs)
                {
                    setUOMs.Add(tr.UOM);
                }
            }

            return setUOMs;
        }

        /// <summary>
        /// 2014-07-31 Zhou Kai adds this method. To get the possible container codes when adding a trip
        /// based on quotationNo, chargeType, isStopDependent and noOfLegs
        /// </summary>
        /// <param name="quotationNo">The quotationNo of the haulier job</param>
        /// <param name="chargeType">the haulierChargeType property of the trip to be added, 1 means None, 2 means IsStopDependent</param>
        /// <param name="noOfLegs">1 means the trip to be added is a single leg trip, 2 means it's a 2-leg trip</param>
        /// <param name="msg">The error message, if it's empty, no errors.</param>
        public static List<ContainerTypesDTO> GetValidContainerTypesDTOsForAddingATrip(string quotationNo, ChargeType chargeType, int noOfLegs,
            bool isBillable, out string msg)
        {
            List<ContainerTypesDTO> validContainers = new List<ContainerTypesDTO>();
            msg = String.Empty;

            if (!quotationNo.Equals(String.Empty)) // have quotation, then all then container types should be depending on quotation, stop_dependent, and noOfLegs
            {
                Quotation quotation = Quotation.GetAllQuotationHeader(quotationNo);
                if (chargeType.GetHashCode() == 2)
                    validContainers =
                        TransportFacadeIn.GetContainerDTOBySizeUsedInQuotation(quotation.QuotationID, true, noOfLegs);
                else
                    validContainers =
                        TransportFacadeIn.GetContainerDTOBySizeUsedInQuotation(quotation.QuotationID, false, noOfLegs);
            
                if (validContainers.Count == 0)
                {
                    msg = TptResourceBLL.ErrNoContainerForQuotation;
                    msg = string.Format(msg, chargeType.ToString());
                }
            }

            // don't have quotation, just get all container types
            //if (quotationNo == "" || !isBillable) 
            //{ validContainers = TransportFacadeIn.GetAllContainerTypes(); }

            // 2014-08-27 Zhou Kai removes the condition of isBillable from the block above
            if (quotationNo.Equals(String.Empty))
            { validContainers = TransportFacadeIn.GetAllContainerTypes(); }

            return validContainers;
        }

        /// <summary>
        /// 2014-11-17 Zhou Kai.
        /// Get the previous leg of the current leg, if the current leg, return itself.
        /// </summary>
        /// <param name="currentLeg">The current leg</param>
        /// <returns>The previous leg of the current leg, if the current leg is the first leg, bool = false
        /// else, bool = true</returns>
        public static Dictionary<HaulierJobTrip, bool> GetPreviousLeg(HaulierJobTrip currentLeg)
        {
            Dictionary<HaulierJobTrip, bool> rtn  = new Dictionary<HaulierJobTrip,bool>();
            if (currentLeg.legGroupMember == 1) { rtn.Add(currentLeg, false);}

            HaulierJob hJob = HaulierJob.GetHaulierJob(currentLeg.JobNo);
            SortableList<HaulierJobTrip> hTrips = HaulierJobTrip.GetAllHaulierJobTrips(hJob);
            int nIndex = hTrips.Find("LegGroupMember", currentLeg.LegGroupMember - 1);
            HaulierJobTrip previousLeg = hTrips[nIndex]; // exception may occur here!!!
            rtn.Add(previousLeg, true);

            return rtn;

        }

        /// <summary>
        /// 2014-11-17 Zhou Kai.
        /// Get the previous leg of the current leg, if the current leg is the first leg of the leg_group,
        /// return itself, and set the bool to false as well.
        /// </summary>
        /// <param name="currentJob">The job header of the current leg</param>
        /// <param name="currentLeg">The current leg</param>
        /// <returns>The previous leg of the current leg, if the current leg is the first leg, bool = false
        /// else, bool = true</returns>
        public static KeyValuePair<HaulierJobTrip, bool> GetPreviousLeg(HaulierJob currentJob,
            HaulierJobTrip currentLeg)
        {
            KeyValuePair<HaulierJobTrip, bool> rtn = new KeyValuePair<HaulierJobTrip, bool>();
            if (currentLeg.legGroupMember == 1)
            { rtn = new KeyValuePair<HaulierJobTrip, bool>(currentLeg, false); return rtn; }

            SortableList<HaulierJobTrip> hTrips = HaulierJobTrip.GetAllHaulierJobTrips(currentJob);
            int nPreviousSeq = hTrips.First(x => x.LegGroup == currentLeg.LegGroup &&
                x.LegGroupMember == currentLeg.LegGroupMember - 1).Sequence;
                // hTrips.Find("LegGroupMember", currentLeg.LegGroupMember - 1);
            HaulierJobTrip previousLeg = hTrips.First(x => x.Sequence == nPreviousSeq); 
            rtn = new KeyValuePair<HaulierJobTrip,bool>(previousLeg, true);

            return rtn;
        }

        #region Transferred from the based class  
        public bool AddJobTripState(JobTripState jobTripState, SqlConnection con, SqlTransaction tran)
        {
            bool temp = true;
            try
            {
                if (ValidateJobTripStateForAdd(jobTripState.Status))
                {
                    temp = HaulierJobDAL.AddJobTripState(this, jobTripState, con, tran);
                    TripStatus = jobTripState.Status;
                    this.JobTripStates.Add(jobTripState);
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
        public bool AddJobTripState(JobTripState jobTripState, string entryFormPrefix, string userID, string frmName)
        {
            bool temp = true;
            try
            {
                if (ValidateJobTripStateForAdd(jobTripState.Status) == true)
                {
                    SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    JobTripStatus origStatus = this.JobTripStates[this.JobTripStates.Count - 1].Status;
                    temp = AddJobTripState(jobTripState, con, tran);

                    //20151102 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.JobNo, "TR", entryFormPrefix, this.JobID, userID, DateTime.Now, frmName, this.Sequence, "EDT");
                    auditLog.WriteAuditLog("TripStatus", jobTripState.Status.ToString(), origStatus.ToString(), con, tran);

                    tran.Commit();
                    TripStatus = jobTripState.Status;
                    this.JobTripStates.Add(jobTripState);
                }
            }
            catch (Exception ex)
            {
                temp = false;
                throw;
            }
            return temp;
        }
        public bool AddJobTripState(JobTripState jobTripState, string entryFormPrefix, string userID, string frmName, SqlConnection con, SqlTransaction tran)
        {
            bool temp = true;
            try
            {
                if (ValidateJobTripStateForAdd(jobTripState.Status))
                {
                    JobTripStatus origStatus = this.JobTripStates[this.JobTripStates.Count - 1].Status;
                    temp = AddJobTripState(jobTripState, con, tran);

                    //20151102 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.JobNo, "TR", entryFormPrefix, this.JobID, userID, DateTime.Now, frmName, this.Sequence, "EDT");
                    auditLog.WriteAuditLog("TripStatus", jobTripState.Status.ToString(), origStatus.ToString(), con, tran);
                    
                    //////get next leg trip
                    ////HaulierJobTrip nextLegTrip = HaulierJobDAL.GetNextLegJobTrip(this.JobID, this.Sequence, this.legGroup/*this.containerNo*/, con, tran);
                    //////TODO set next leg status to Ready if current leg is completed
                    ////if (!nextLegTrip.Equals(new HaulierJobTrip()))
                    ////{
                    ////    JobTripStatus nextLegOrigStatus = nextLegTrip.TripStatus;
                    ////    JobTripState newJobTripState = new JobTripState(nextLegTrip.Sequence, JobTripStatus.Ready, DateTime.Now, "Set_Status_for_Multiple_SubConTrips", true);
                    ////    if (ValidateJobTripStateForAdd(newJobTripState.Status))
                    ////        AddJobTripState(newJobTripState, con, tran);
                    ////    //20151102 - gerry added new audit log
                    ////    auditLog = new AuditLog(nextLegTrip.JobNo, "TR", entryFormPrefix, nextLegTrip.JobID, userID, DateTime.Now, frmName, nextLegTrip.Sequence, FormMode.Edit.ToString());
                    ////    auditLog.WriteAuditLog("TripStatus", newJobTripState.Status.ToString(), nextLegOrigStatus.ToString(), con, tran);
                    ////}
                    //TODO logging


                    TripStatus = jobTripState.Status;
                    this.JobTripStates.Add(jobTripState);
                }
            }
            catch (Exception ex)
            {
                temp = false;
                throw;
            }
            return temp;
        }       
        public bool DeleteJobTripState(JobTripState jobTripState)
        {
            bool temp = true;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                temp = DeleteJobTripStateUpdateHaulierJobTripStatus(jobTripState, con, tran);
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                temp = false;
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return temp;
        }  
        public bool DeleteJobTripStateUpdateHaulierJobTripStatus(JobTripState jobTripState, SqlConnection con, SqlTransaction tran)
        {
            bool temp = true;
            try
            {
                temp = HaulierJobDAL.DeleteJobTripState(this, jobTripState, con, tran);

            }
            catch (Exception ex)
            {
                temp = false;
                throw ex;
            }
            return temp;
        }

        #endregion

        #region "2014-11-18 Zhou Kai"
        public static string ValidateChangingSubConTripStatus(bool hasPreviousLeg, 
            JobTripStatus previousLegStatus,
            JobTripStatus currentLegStatus, JobTripStatus newStatus)
        {
            string msg = String.Empty;
            if (hasPreviousLeg) // for case the current leg has a previous leg
            {
                // the previous leg must be 
                // completed before the current leg can be set to any other status
                if (previousLegStatus != JobTripStatus.Completed)
                {
                    msg = String.Format(TptResourceBLL.ErrCantSetStatusTo,
                        currentLegStatus.ToString(), newStatus.ToString());
                    msg += String.Format(TptResourceBLL.ErrPreviousLegStatusIs,
                            previousLegStatus.ToString());

                    return msg;
                }
            }
            #region 20150917 - Gerry removed to allow subcontracted jobtrips to set status to Assigned or complete
            // for both cases: have or don't have previous leg:
            //switch (currentLegStatus)
            //{
            //    case JobTripStatus.Booked:
            //        {
            //            // the current status can be set to ready only
            //            if (newStatus != JobTripStatus.Ready)
            //            {
            //                msg = String.Format(TptResourceBLL.ErrCantSetStatusTo,
            //                    currentLegStatus.ToString(), newStatus.ToString());
            //            }
            //            break;
            //        }
            //    case JobTripStatus.Ready:
            //        { // the current status can be set to assigned or completed only
            //            if (!(newStatus == JobTripStatus.Assigned ||
            //                newStatus == JobTripStatus.Completed))
            //            {
            //                msg = String.Format(TptResourceBLL.ErrCantSetStatusTo,
            //                                                       currentLegStatus.ToString(), newStatus.ToString());
            //            }
            //            break;
            //        }
            //    case JobTripStatus.Assigned:
            //        {
            //            // the current status can be set to completed only
            //            if (newStatus != JobTripStatus.Completed)
            //            {
            //                msg = String.Format(TptResourceBLL.ErrCantSetStatusTo,
            //                                                    currentLegStatus.ToString(), newStatus.ToString());
            //            }
            //            break;
            //        }
            //    default:
            //        {
            //            // besides the status above, there're Completed and Invoiced
            //            // we don't allow the status to be changed if the current status is Completed
            //            // and we don't consider invoiced status here.
            //            msg = String.Format(TptResourceBLL.ErrCantSetStatusTo,
            //                                                    currentLegStatus.ToString(), newStatus.ToString());
            //            break;
            //        }
            //}
            #endregion

            return msg;
        }

        /// <summary>
        /// 2014-11-18 Zhou Kai
        /// </summary>
        /// <param name="selectedTrips"></param>
        public static string  SaveMultipleSubConTripStatus(List<HaulierJobTrip> trips
            /*Dictionary<HaulierJobTrip, HaulierJobTrip> selectedTripsWithPreviousLeg*/,
            JobTripStatus newStatus, string entryFormPrefix, string frmName, string userID)
        {
            string msg = String.Empty;
            #region 20150917 - gerry removed
            //List<HaulierJobTrip> validTrips = new List<HaulierJobTrip>();
            //foreach (KeyValuePair<HaulierJobTrip, HaulierJobTrip> kvp in selectedTripsWithPreviousLeg)
            //{
            //    bool hasPreviousLeg = kvp.Value != null;
            //    string tmp = HaulierJobTrip.ValidateChangingSubConTripStatus(hasPreviousLeg,
            //        hasPreviousLeg ? kvp.Value.TripStatus : JobTripStatus.Booked, kvp.Key.TripStatus, newStatus);
            //    if (tmp != String.Empty)
            //    {
            //        msg += Environment.NewLine + tmp + " >>> JobNo: " + kvp.Key.JobNo + " LegGroup: " +
            //            kvp.Key.LegGroup.ToString() + " LegGroupMember: " + 
            //            kvp.Key.LegGroupMember.ToString();
            //    }
            //}
            #endregion
            foreach (HaulierJobTrip trip in trips)
            {
                bool hasPrevLeg = trip.legGroupMember > 1;
                JobTripStatus prevLegStatus = JobTripStatus.Completed;
                if (hasPrevLeg)
                {
                    HaulierJobTrip prevLeg = trip.GetPreviousLegJobTrip(); //trip.GetPreviousLegTrip();
                    prevLegStatus = prevLeg.TripStatus;
                    //hasPrevLeg = prevLeg.OwnTransport; //if previous leg is subcon it considered
                }
                string tmp = ValidateChangingSubConTripStatus(hasPrevLeg, prevLegStatus, trip.TripStatus, newStatus);
                if (tmp != String.Empty)
                {
                    msg += Environment.NewLine + tmp + " >>> JobNo: " + trip.JobNo + " LegGroup: " +
                        trip.LegGroup.ToString() + " LegGroupMember: " +
                        trip.LegGroupMember.ToString();
                }
            }
            
            if (msg == String.Empty)
            {
                // no errors, so go on changing status
                string strDBCon = FMGlobalSettings.TheInstance.getConnectionString();
                using (SqlConnection dbCon = new SqlConnection(strDBCon))
                {
                    SqlTransaction dbTran = null;
                    try
                    {
                        if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                        dbTran = dbCon.BeginTransaction();
                        #region 20150917 - gerry replaced
                        //foreach (KeyValuePair<HaulierJobTrip, HaulierJobTrip> kvp in selectedTripsWithPreviousLeg)
                        //{
                        //    HaulierJobTrip currentTrip = kvp.Key;
                        //    JobTripState newState = new JobTripState(currentTrip.Sequence,
                        //       newStatus, DateTime.Now, "Set_Status_for_Multiple_SubConTrips", true );
                        //    kvp.Key.AddJobTripState(newState, dbCon, dbTran);
                        //}
                        #endregion
                        foreach (HaulierJobTrip trip in trips)
                        {
                            //20151103 - gerry added validation cannot set status to complete if containerNo is blank
                            if (trip.containerNo.Equals(string.Empty) && newStatus == JobTripStatus.Completed)
                            {
                                throw new FMException("Cannot set to complete without containerNo. Please change from booking. ");
                            }
                            JobTripState newJobTripState = new JobTripState(trip.Sequence, newStatus, DateTime.Now, "Set_Status_for_Multiple_SubConTrips", true);
                            trip.AddJobTripState(newJobTripState, entryFormPrefix, userID, frmName, dbCon, dbTran);
                       
                            //2015113 - gerry added to set next leg to ready
                            if(newStatus == JobTripStatus.Completed)
                            {
                                ApplicationOption settings = ApplicationOption.GetApplicationOption(ApplicationOption.HAULAGE_SETTINGS_ID, ApplicationOption.SETTINGS_AUTO_UPDATE_STATUS_NEXT_LEG);
                                bool autoSetToReady = settings.setting_Value == "T" ? true: false;
                                trip.UpdateNextLegToReadyStatus(autoSetToReady, "PL", userID, frmName, dbCon, dbTran);
                            }
                        }
                        dbTran.Commit();
                    }
                    catch (FMException fmEx) { if (dbTran != null) { dbTran.Rollback(); } throw fmEx; }
                    catch (SqlException ex)
                    { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                    catch (InvalidOperationException ex)
                    { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                    catch (Exception ex)
                    { if (dbTran != null) { dbTran.Rollback(); } throw new FMException(ex.Message); }
                }
            }

            return msg;
        }

        ///// <summary>
        /// 2014-11-17 Zhou Kai.
        /// Get the previous legs of the current legs, only when there is a previous leg.
        /// </summary>
        /// <param name="currentTrips">The current legs</param>
        /// <param name="returnCurrentTrip">true: when there is no previous leg, add the current leg to
        /// the returning dictionary as value, false: add null instead</param>
        /// <returns>In the return dictionary, the key is the current leg, the value is the previous leg.</returns>
        public static Dictionary<HaulierJobTrip, HaulierJobTrip> GetPreviousTrips(
            bool returnCurrentTrip,
            List<HaulierJobTrip> currentTrips)
        {
            Dictionary<HaulierJobTrip, HaulierJobTrip> rtn =
                new Dictionary<HaulierJobTrip, HaulierJobTrip>();
            // classify the current trips by job_id
            IEnumerable<IGrouping<string, HaulierJobTrip>> groups =
                currentTrips.GroupBy(x => x.JobNo);
            foreach (IGrouping<string, HaulierJobTrip> group in groups)
            {
                HaulierJob hJob = HaulierJob.GetHaulierJob(group.Key);
                foreach (HaulierJobTrip t in group)
                {
                    KeyValuePair<HaulierJobTrip, bool> tmp = HaulierJobTrip.GetPreviousLeg(hJob, t);
                    // when there is a previous leg, insert it to the returning dictionary
                    // otherwise, insert null the previous(only when specified to)
                    if (tmp.Value) { rtn.Add(t, tmp.Key); }
                    else { if (returnCurrentTrip) { rtn.Add(t, null); } }
                }
            }

            return rtn;
        }

        #endregion

       

        public ContainerMovementInfo GetContainerMovementInfo()
        {
            return HaulierJobDAL.GetContainerMovementInfo(this.JobID, this.Sequence);
        }
        //20161201
        public bool UpdateProcessedReleasedESNForAllLegs()
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    HaulierJobDAL.UpdateProcessedReleasedESNForAllLegs(this, con, tran);
                    tran.Commit();
                    return true;
                }
                catch (FMException fmEx) { tran.Rollback(); throw; }
                catch (Exception fmEx) { tran.Rollback(); throw; }
                finally { con.Close(); }
            }
        }
    }
}
