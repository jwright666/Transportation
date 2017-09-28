using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_HLBookDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_TRKBookDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using System.Collections;
using TR_LanguageResource.Resources;
using System.Globalization;
using FM.TR_MarketDLL.BLL;
using System.Data;

namespace FM.TR_TRKBookDLL.BLL
{
    public class TruckJobTrip : JobTrip, ICloneable
    {
        private decimal bookedWeight;   //renamed from grossWeight
        private decimal bookedVol;     //renamed from grossCBM
        private bool isLaden;
        private string cargoDescription;
        private bool isDangerousGoods;
        private string dGRemarks;
        private bool isOversize;
        private string oversizeRemarks;
        private bool isDirectDelivery;
        public decimal BillingQty { get; set; }   //renamed from TotalCBM
        public decimal TotalWeight { get; set; }
        public SortableList<TruckJobTripDetail> truckJobTripDetail;

        public decimal actualVol { get; set; }     //renamed from maxCBM
        public decimal actualWeight { get; set; }    //renamed from maxWeight

        public SortableList<TruckJobTripPlan> truckJobTripPlans;
        public string billing_UOM { get; set; }
        public decimal weightRequired { get; set; }
        public bool wtVolFrDetails { get; set; }
        //20130903 - Gerry added package feature
        public bool isPackageDependent { get; set; }
        public bool isMultiplePackage { get; set; }
        public string packType { get; set; }
        //20130930 - Gerry added for multiple trips
        public int partnerLeg { get; set; }
        public Leg_Type legType { get; set; }

        // 2013-12-23 Zhou Kai adds const string for log writting
        private const string LOG_DETAIL_UOM = "Uom";
        private const string LOG_DETAIL_QTY = "Qty";
        private const string LOG_DETAIL_DELETED_DATETIME = "Deleted DateTime";
        // 2013-12-24 Zhou Kai ends

        // 2014-03-24 Zhou Kai adds, if this truck job trip is by OwnTransport,
        // the currentTruckJobTripSubCon should be null,
        // otherwise, a currentTruckJobTripSubCon object will be created
        public TruckJobTripSubCon currentTruckJobTripSubCon;
        //private bool ownTransport;
        public override bool OwnTransport
        {
            get { return ownTransport; }
            set
            {
                ownTransport = value;
                // Set the currentTruckJobTripSubCon to null in any cases
                // when the OwnTransport is set to True
                if (ownTransport) { currentTruckJobTripSubCon = null; }
                else
                {
                    // 2014-03-27 Zhou Kai adds comments
                    // Consider two cases:
                    // (1) Set OwnTransport from True to False
                    // (2) Add a NEW truckJobTrip which is under sub-contractor
                    // Only in the above two cases, a TruckJobTripSubCon object will be created
                    if (object.ReferenceEquals(currentTruckJobTripSubCon, null))
                    {
                        currentTruckJobTripSubCon = new TruckJobTripSubCon();
                    }
                    else { currentTruckJobTripSubCon = GetTruckJobTripSubCon(); }
                }
                // 2014-03-24 Zhou Kai ends
            }
        }
        // 2014-03-24 Zhou Kai ends
        public Trip_Type tripType { get; set; }
        //20170801
        public bool isRefrigeratedGoods { get; set; }

        public TruckJobTrip()
            : base()
        {
            this.bookedWeight = 0;
            this.bookedVol = 0;
            this.isLaden = false;
            this.cargoDescription = "";
            this.isDangerousGoods = false;
            this.dGRemarks = "";
            this.isOversize = false;
            this.oversizeRemarks = "";
            this.isDirectDelivery = false;
            this.BillingQty = 0;
            this.TotalWeight = 0;
            this.truckJobTripDetail = new SortableList<TruckJobTripDetail>();
            this.actualVol = 0;
            this.actualWeight = 0;
            this.truckJobTripPlans = new SortableList<TruckJobTripPlan>();
            this.billing_UOM = "";
            this.weightRequired = 0;
            this.wtVolFrDetails = true;
            this.packType = string.Empty;
            this.isPackageDependent = false;
            this.isMultiplePackage = false;
            this.legType = Leg_Type.OneLeg;
            this.partnerLeg = 0;
            // 2014-03-24 Zhou Kai adds
            this.currentTruckJobTripSubCon = null;
            this.isRefrigeratedGoods = false;
        }

        //201040502 - gerry change subCon from CustomerDTO to OperatorDTO
        public TruckJobTrip(int jobID, int sequence, DateTime startDate, Stop startStop, DateTime endDate, Stop endStop,
            string startTime, string endTime, JobTripStatus status, SortableList<JobTripState> jobTripStates, byte[] updateVersion, bool isNew,
            bool ownTransport, OperatorDTO subCon, string subContractorReference, string chargeCode, string remarks,
            decimal bookedWeight, decimal bookedVol, decimal actualWeight, decimal actualVol, bool isLaden, string cargoDescription, bool isDangerousGoods,
            string dGRemarks, bool isOversize, string oversizeRemarks, bool isDirectDelivery, decimal billingQty,
            decimal TotalWeight, SortableList<TruckJobTripDetail> truckJobTripDetail, SortableList<TruckJobTripPlan> truckJobTripPlans,
            string billing_UOM, decimal weightRequired, bool wtVolFrDetails
            )
            : base(jobID, sequence, startDate, startStop, endDate, endStop, startTime, endTime, status,
            jobTripStates, updateVersion, isNew, ownTransport,
            subCon, subContractorReference, chargeCode, remarks)
        {
            this.bookedWeight = bookedWeight;
            this.bookedVol = bookedVol;
            this.isLaden = isLaden;
            this.cargoDescription = cargoDescription;
            this.isDangerousGoods = isDangerousGoods;
            this.dGRemarks = dGRemarks;
            this.isOversize = isOversize;
            this.oversizeRemarks = oversizeRemarks;
            this.isDirectDelivery = isDirectDelivery;
            this.BillingQty = billingQty;
            this.TotalWeight = TotalWeight;
            this.truckJobTripDetail = truckJobTripDetail;
            this.actualWeight = actualWeight;
            this.actualVol = actualVol;
            this.truckJobTripPlans = truckJobTripPlans;
            this.billing_UOM = billing_UOM;
            this.weightRequired = weightRequired;
            this.wtVolFrDetails = wtVolFrDetails;
            // 2014-03-24 Zhou Kai adds
            this.OwnTransport = ownTransport; // hide the base.OwnTransport
            this.isRefrigeratedGoods = false;
        }

        public decimal BookedVol
        {
            get { return bookedVol; }
            set { bookedVol = value; }
        }


        public decimal BookedWeight
        {
            get { return bookedWeight; }
            set { bookedWeight = value; }
        }

        public string CargoDescription
        {
            get { return cargoDescription; }
            set { cargoDescription = value; }
        }

        public bool IsLaden
        {
            get { return isLaden; }
            set { isLaden = value; }
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

        public bool ValidateTruckJobTrip()
        {
            string error = "";
            bool isValid = true;
            if (BillingQty <= 0 && !isMultiplePackage)
            {
                error += TptResourceBLL.ErrQtyZero + "\n";
                isValid = false;
            }
            if (String.Compare(StartStop.Code, EndStop.Code) == 0)
            {
                error += TptResourceBLL.ErrStartStopnEndStop + "\n";
                isValid = false;
            }
            if (StartTime == "")
            {
                error += TptResourceBLL.ErrStartTimeBlank + "\n";
                isValid = false;
            }
            if (EndTime == "")
            {
                error += TptResourceBLL.ErrEndTimeBlank + "\n";
                isValid = false;
            }
            if (StartDate.CompareTo(EndDate) == 0)
            {
                if ((StartTime != "") && (EndTime != ""))
                {
                    if ((StartTime != "0000") && (EndTime != "0000"))
                    {
                        //20131016 - gerry change to allow equal start/end time 
                        //if (Convert.ToInt32(StartTime) >= Convert.ToInt32(EndTime))
                        if (Convert.ToInt32(StartTime) > Convert.ToInt32(EndTime))
                        {
                            error += TptResourceBLL.ErrStartTimenEndTime + "\n";
                            isValid = false;
                        }
                    }
                }
            }
            else
            {
                if (StartDate.CompareTo(EndDate) > 0)
                {
                    error += TptResourceBLL.ErrStartDatenEndDate + "\n";
                    isValid = false;
                }
            }
            /*
            if (!wtVolFrDetails)
            {
                if (actualVol <= 0 && billing_UOM == TruckMovementUOM_WtVol.CBM.ToString())
                {
                    error += string.Format(TptResourceBLL.ErrZeroWtVol, CommonResource.Volume, string.Format(TptResourceUI.ActualVol, string.Empty)) + "\n";
                    isValid = false;
                }
                if (actualWeight <= 0 && (billing_UOM == TruckMovementUOM_WtVol.KGM.ToString() || billing_UOM == TruckMovementUOM_WtVol.MT.ToString()))
                {
                    error += string.Format(TptResourceBLL.ErrZeroWtVol, CommonResource.Weight, string.Format(TptResourceUI.ActualWeight, string.Empty)) + "\n";
                    isValid = false;
                }
            }
             */ 
            //2013 - gerry added validation for package dependent
            if (this.isPackageDependent && !this.isMultiplePackage)
            {
                if (this.billing_UOM.Equals(string.Empty))
                {
                    error += string.Format(TptResourceBLL.ErrNoUOM);
                    isValid = false;
                }
            }
            if (isValid == false)
            {
                throw new FMException(error);
            }
            return isValid;
        }
        #region 20130429 Gerry Removed the same validation with add
        /*
        public bool ValidateEditTruckJobTrip()
        {
            string error = "";
            bool status = true;
            //20130412 - gerry removed allowed edit actual qty
            //if (TripStatus != JobTripStatus.Booked)
            //{
            //    error += TptResourceBLL.ErrCantEditBesideBooked + "\n";
            //    status = false;
            //}
            //20130412 end 
            if (String.Compare(StartStop.Code, EndStop.Code) == 0)
            {
                error += TptResourceBLL.ErrStartStopnEndStop + "\n";
                status = false;
            }
            if (StartTime == "")
            {
                error += TptResourceBLL.ErrStartTimeBlank + "\n";
                status = false;
            }
            if (EndTime == "")
            {
                error += TptResourceBLL.ErrEndTimeBlank + "\n";
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
                            error += TptResourceBLL.ErrStartTimenEndTime + "\n";
                            status = false;
                        }
                    }
                }
            }
            else
            {
                if (StartDate.CompareTo(EndDate) > 0)
                {
                    error += TptResourceBLL.ErrStartDatenEndDate + "\n";
                    status = false;
                }
            }

            if (status == false)
            {
                throw new FMException(error);
            }
            return status;

        }
         */
        #endregion
        public bool ValidateDeleteTruckJobTrip()
        {
            if ((TripStatus == JobTripStatus.Assigned) || (TripStatus == JobTripStatus.Completed))
            {
                throw new FMException(TptResourceBLL.ErrCantDelteCompletedorAssignedJob);

            }

            return true;
        }

        public bool AddTruckJobTripDetail(TruckJobTripDetail truckJobTripDetail, string frmName, string user, TruckJob truckJob)
        {
            bool temp = true;
            try
            {
                if (IsTruckJobTripDetailUOMNotExist(truckJobTripDetail) && ValidateTruckJobTripDetail(truckJobTripDetail))
                {
                    SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    try
                    {
                        //TruckJobTripDetail origTruckJobTrip = TruckJobDAL.GetTruckJobTripDetail(truckJobTripDetail.jobID, truckJobTripDetail.jobTripSequence, truckJobTripDetail.detailSequence, con, tran);
                        TruckJobTripDetail truckJobTripDetailOut = new TruckJobTripDetail();
                        if (TruckJobDAL.AddTruckJobTripDetail(this, truckJobTripDetail, con, tran, out truckJobTripDetailOut))
                        {
                            //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                            this.truckJobTripDetail.Add(truckJobTripDetail);
                            if (this.wtVolFrDetails) { UpdateGrossWeightAndVolume(truckJob.BranchCode, con, tran); }
                            TruckJob.ChangeTruckMovementCharges(truckJob, con, tran);
                            //20131019 end


                            ////20161024 - gerry added new audit log
                            AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, this.LastModified_By, DateTime.Now, "Job Trip Detail ", truckJobTripDetail.detailSequence, "ADD");
                            auditLog.WriteAuditLog(truckJobTripDetail, null, con, tran);

                            tran.Commit();
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
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            return temp;

        }
        public bool AddTruckJobTripDetail(TruckJobTripDetail truckJobTripDetail, string frmName, string user, TruckJob truckJob, SqlConnection con, SqlTransaction tran)
        {
            bool temp = true;
            try
            {
                if (IsTruckJobTripDetailUOMNotExist(truckJobTripDetail) && ValidateTruckJobTripDetail(truckJobTripDetail))
                {
                    try
                    {
                        TruckJobTripDetail truckJobTripDetailOut = new TruckJobTripDetail();
                        if (TruckJobDAL.AddTruckJobTripDetail(this, truckJobTripDetail, con, tran, out truckJobTripDetailOut))
                        {
                            //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                            this.truckJobTripDetail.Add(truckJobTripDetail);
                            this.UpdateVersion = GetTruckJobTripUpdatedVersion(con, tran);
                            if (this.wtVolFrDetails) { UpdateGrossWeightAndVolume(truckJob.BranchCode, con, tran); }
                            TruckJob.ChangeTruckMovementCharges(truckJob, con, tran);
                            //20131019 end

                            ////20161024 - gerry added new audit log
                            AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, this.LastModified_By, DateTime.Now, "Job Trip Detail ", truckJobTripDetail.detailSequence, "ADD");
                            auditLog.WriteAuditLog(truckJobTripDetail, null, con, tran);
                        }
                    }
                    catch (FMException ex) { throw ex; }
                    catch (Exception ex) { throw new FMException(ex.ToString()); }
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            return temp;

        }

        public bool AddTruckJobTripPlan(TruckJobTripPlan truckJobTripPlan, SqlConnection con, SqlTransaction tran, out TruckJobTripPlan outTruckJobTripPlan, string user, string frmName)
        {
            bool temp = true;

            try
            {
                outTruckJobTripPlan = new TruckJobTripPlan();
                TruckJobDAL.AddTruckJobTripPlan(truckJobTripPlan, con, tran, out outTruckJobTripPlan);

                //Chong Chin, 13 March 2011, start
                //When a new JobTrip is added in FrmPlanSubTrip, the status of the 
                //the TruckJobTrip status should be set to Assigned, if it has not been set

                if (this.TripStatus != JobTripStatus.Assigned)
                {
                    // Create a JobTripState
                    JobTripState jobTripState = new JobTripState();
                    jobTripState.IsNew = true;
                    jobTripState.StatusDate = DateTime.Today;
                    // 2014-02-26 Zhou Kai adds the line below
                    jobTripState.Status = JobTripStatus.Assigned;
                    // Call 
                    // 2014-02-26 Zhou Kai modifies the line below, to make sure the old status is correct
                    SetJobTripStatusForTruck(jobTripState, con, tran, user, frmName, this.TripStatus.ToString());
                    this.TripStatus = JobTripStatus.Assigned;
                }
                // Chong Chin 13 March 11, End

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

        private bool IsTruckJobTripDetailUOMNotExist(TruckJobTripDetail truckJobTripDetail)
        {
            foreach (TruckJobTripDetail tripDetail in this.truckJobTripDetail)
            {
                //20160111 -gerry modified validation that description should be unique if the same UOM
                if (tripDetail.jobTripSequence == truckJobTripDetail.jobTripSequence
                    && tripDetail.detailSequence != truckJobTripDetail.detailSequence
                    && tripDetail.uom.Equals(truckJobTripDetail.uom) 
                    && tripDetail.marking.Trim().Equals(truckJobTripDetail.marking.Trim()))
                {
                    throw new FMException(TptResourceBLL.ErrUOMExist + "");
                }
            }
            return true;
        }

        public bool ValidateTruckJobTripDetail(TruckJobTripDetail truckJobTripDetail)
        {
            if ((truckJobTripDetail.totalWeight == 0) && (truckJobTripDetail.volume == 0))
                throw new FMException(TptResourceBLL.ErrTotalWeightOrVolZero);
            //20131021 - gerry added to filter duplicate uom and marking
            foreach (TruckJobTripDetail temp in this.truckJobTripDetail)
            {
                if (temp.detailSequence != truckJobTripDetail.detailSequence
                    && temp.uom == truckJobTripDetail.uom
                    && temp.marking.Trim().Equals(truckJobTripDetail.marking.Trim()))
                {
                    throw new FMException(TptResourceBLL.ErrUOMExist);
                }
            }
            //20131021 end
            return true;
        }
        public bool EditTruckJobTripDetailMemory(TruckJobTripDetail truckJobTripDetail, string frmName, string user, TruckJob truckJob)
        {
            try
            {

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }

        public bool EditTruckJobTripDetail(TruckJobTripDetail truckJobTripDetail, string frmName, string user, TruckJob truckJob)
        {
            bool temp = true;
            try
            {
                if (ValidateTruckJobTripDetail(truckJobTripDetail))
                {

                    SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    try
                    {
                        TruckJobTripDetail origTruckJobTripDetail = TruckJobDAL.GetTruckJobTripDetail(truckJobTripDetail.jobID,truckJobTripDetail.jobTripSequence, truckJobTripDetail.jobTripSequence, con, tran);
                        TruckJobTripDetail truckJobTripDetailOut = new TruckJobTripDetail();
                        if (TruckJobDAL.EditTruckJobTripDetail(this, truckJobTripDetail, con, tran, out truckJobTripDetailOut) == true)
                        {
                            //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                            if (this.wtVolFrDetails) { UpdateGrossWeightAndVolume(truckJob.BranchCode, con, tran); }
                            TruckJob.ChangeTruckMovementCharges(truckJob, con, tran);
                            //20131019 end

                            ////20161024 - gerry added new audit log
                            AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, user, DateTime.Now, "Job Trip Detail", truckJobTripDetail.detailSequence, "EDT");
                            auditLog.WriteAuditLog(truckJobTripDetail, origTruckJobTripDetail, con, tran);

                            tran.Commit();
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
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            return temp;

        }


        public bool DeleteTruckJobTripDetail(TruckJobTrip truckJobTrip, TruckJobTripDetail truckJobTripDetail, string frmName, string user, TruckJob truckJob)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    if (TruckJobDAL.DeleteTruckJobTripDetail(truckJobTrip, truckJobTripDetail, con, tran))
                    {
                        //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                        this.truckJobTripDetail.Remove(truckJobTripDetail);
                        if (this.wtVolFrDetails) { UpdateGrossWeightAndVolume(truckJob.BranchCode, con, tran); }
                        TruckJob.ChangeTruckMovementCharges(truckJob, con, tran);
                        //20131019 end
                        #region 20160811 - gerry replaced auditlog
                        /*
                        #region "2014-01-23 Zhou Kai comments out this bolck"
                        //June 20, 2011 - Gerry Modify to form LogHeader and log Details
                        DateTime serverDateTime = Logger.GetServerDateTime();
                        LogHeader logHeader = new LogHeader(FMModule.Trucking, frmName, serverDateTime,
                                                                                        truckJobTrip.JobNo.ToString() + "&" + truckJobTripDetail.jobTripSequence.ToString(),
                                                                                        truckJobTripDetail.detailSequence.ToString(),
                                                                                        FormMode.Delete, user);

                        LogDetail logDetail1 = new LogDetail(LOG_DETAIL_DELETED_DATETIME, serverDateTime.ToString("yyyy-MM-dd HH:mm"));
                        logHeader.LogDetails.Add(logDetail1);
                        Logger.WriteLog(logHeader, con, tran);
                        #endregion
                        */
                        #endregion

                        //20160811 - auditlog for job trip, only capture CargoDescription
                        AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.Sequence, user, DateTime.Now, frmName, truckJobTripDetail.detailSequence, "DEL");
                        auditLog.WriteAuditLog(truckJobTripDetail, null, con, tran);

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
                finally { con.Close(); }
            }

        }
        public bool DeleteTruckJobTripDetails(TruckJobTrip truckJobTrip, TruckJobTripDetail truckJobTripDetail, string frmName, string user, TruckJob truckJob,
            SqlConnection con, SqlTransaction tran)
        {
            try
            {
                if (TruckJobDAL.DeleteTruckJobTripDetail(truckJobTrip, truckJobTripDetail, con, tran))
                {
                    //20131019 - gerry added to cater for single transaction when when display calculating the truckmovement charges
                    this.truckJobTripDetail.Remove(truckJobTripDetail);
                    if (this.wtVolFrDetails) { UpdateGrossWeightAndVolume(truckJob.BranchCode, con, tran); }
                    TruckJob.ChangeTruckMovementCharges(truckJob, con, tran);
                    //20131019 end
                    #region 20160811 - gerry replaced auditlog
                    /*
                    #region "2014-01-23 Zhou Kai comments out this bolck"
                    //June 20, 2011 - Gerry Modify to form LogHeader and log Details
                    DateTime serverDateTime = Logger.GetServerDateTime();
                    LogHeader logHeader = new LogHeader(FMModule.Trucking, frmName, serverDateTime,
                                                                                    truckJobTrip.JobNo.ToString() + "&" + truckJobTripDetail.jobTripSequence.ToString(),
                                                                                    truckJobTripDetail.detailSequence.ToString(),
                                                                                    FormMode.Delete, user);

                    LogDetail logDetail1 = new LogDetail(LOG_DETAIL_DELETED_DATETIME, serverDateTime.ToString("yyyy-MM-dd HH:mm"));
                    logHeader.LogDetails.Add(logDetail1);
                    Logger.WriteLog(logHeader, con, tran);
                    #endregion
                    */
                    #endregion

                    //20160811 - auditlog for job trip, only capture CargoDescription
                    AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.Sequence, user, DateTime.Now, frmName, truckJobTripDetail.detailSequence, "DEL");
                    auditLog.WriteAuditLog(truckJobTripDetail, null, con, tran);

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
            finally { con.Close(); }
        }

        public bool DeleteTruckJobTripPlan(TruckJobTripPlan truckJobTripPlan, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                if (TruckJobDAL.DeleteTruckJobTripPlan(truckJobTripPlan, con, tran) /*== true*/)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
        }


        public static SortableList<TruckJobTrip> GetAllTruckJobTrips()
        {
            return TruckJobDAL.GetAllTruckJobTrips();
        }

        public static TruckJobTrip GetTruckJobTrip(int jobid, int sequence)
        {
            return TruckJobDAL.GetTruckJobTrip(jobid, sequence);
        }
        public static TruckJobTrip GetTruckJobTrip(int jobid, int sequence, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.GetTruckJobTrip(jobid, sequence, con, tran);
        }

        public ArrayList GetTotalPieces()
        {
            ArrayList uomlist = new ArrayList();
            ArrayList list = new ArrayList();

            uomlist = GetUoms();
            for (int i = 0; i < uomlist.Count; i++)
            {
                int total = 0;
                for (int j = 0; j < truckJobTripDetail.Count; j++)
                {
                    if (truckJobTripDetail[j].uom == (string)uomlist[i])
                    {
                        total = total + truckJobTripDetail[j].quantity;
                    }
                }
                list.Add(total);

            }
            return list;
        }


        public ArrayList GetUoms()
        {
            ArrayList list = new ArrayList();

            for (int i = 0; i < truckJobTripDetail.Count; i++)
            {
                if (!list.Contains((string)truckJobTripDetail[i].uom))
                {
                    list.Add(truckJobTripDetail[i].uom);
                }
                /*
                if (list.Count == 0)
                {
                }
                else
                {
                    bool found = false;
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j] == truckJobTripDetail[i].uom)
                        {
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        list.Add(truckJobTripDetail[i].uom);
                    }
                }
                */
            }
            return list;

        }

        public decimal GetTotalVolume()
        {

            decimal total = 0;
            for (int j = 0; j < truckJobTripDetail.Count; j++)
            {
                total = total + truckJobTripDetail[j].volume;
            }
            return total;
        }

        public decimal GetTotalWeight()
        {
            decimal total = 0;
            for (int j = 0; j < this.truckJobTripDetail.Count; j++)
            {
                total = total + this.truckJobTripDetail[j].totalWeight;
            }
            return total;
        }

        public SortableList<TruckJobTripDetail> GetTruckJobTripDetails()
        {
            SortableList<TruckJobTripDetail> t = new SortableList<TruckJobTripDetail>();
            t = TruckJobDAL.GetTruckJobTripDetails(this.JobID, this.Sequence);
            foreach (TruckJobTripDetail jobTripDet in t)
            {
                jobTripDet.balQty = TruckJobDAL.GetAvailableBalQtyForCargoDetail(jobTripDet);
            }
            this.truckJobTripDetail = t;
            return t;
        }

        // 2013-12-18 Zhou Kai adds function to get truck job trip
        // details from truck job number and truck job trip seq no.
        public static SortableList<TruckJobTripDetail> GetTruckJobTripDetails(string jobNo, int tripSeq)
        {
            return TruckJobDAL.GetTruckJobTripDetailsStatic(jobNo, tripSeq);
        }
        // 2013-12-18 Zhou Kai ends

        public bool UpdateGrossWeightAndVolume(Charge charge)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {
                CalculateGrossWeightAndVolume();
                this.BillingQty = GetBillingQty(charge, this.billing_UOM);
                if (TruckJobDAL.UpdateWeightAndVolume(this, con, tran) == true)
                {
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
        //20131019 - gerry added to cater a single transaction
        public bool UpdateGrossWeightAndVolume(string branchCode, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                Charge tempCharge = Charge.GetCharge(this.ChargeCode, branchCode);
                CalculateGrossWeightAndVolume();
                this.BillingQty = GetBillingQty(tempCharge, this.billing_UOM);
                TruckJobDAL.UpdateWeightAndVolume(this, con, tran);

            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }

        internal void CalculateGrossWeightAndVolume()
        {
            if (wtVolFrDetails == true)
            {
                BookedVol = GetTotalVolume();
                BookedWeight = GetTotalWeight();

                actualVol = GetTotalVolume();
                actualWeight = GetTotalWeight();
            }
            else
            {
                if ((BookedVol == 0) && (BookedWeight == 0))
                {
                    throw new FMException(TptResourceBLL.ErrGrossWeightAndGrossCBMisZero);
                }
            }
        }

        public decimal GetAllocatedVolToPlan()
        {
            decimal vol = 0;
            for (int i = 0; i < truckJobTripPlans.Count; i++)
            {
                vol = vol + truckJobTripPlans[i].volume;
            }
            return vol;
        }

        public decimal GetAllocatedWeightToPlan()
        {
            decimal weight = 0;
            for (int i = 0; i < truckJobTripPlans.Count; i++)
            {
                weight = weight + truckJobTripPlans[i].weight;
            }
            return weight;
        }

        public decimal GetBalanceVolForPlan()
        {
            return (this.bookedVol - this.GetAllocatedVolToPlan());
        }

        public decimal GetBalanceWeightForPlan()
        {
            return (this.bookedWeight - this.GetAllocatedWeightToPlan());
        }

        public object Clone()
        {
            TruckJobTrip newTruckJobTrip = new TruckJobTrip();
            newTruckJobTrip.JobID = this.JobID;
            newTruckJobTrip.Sequence = this.Sequence;
            foreach (TruckJobTripPlan t in truckJobTripPlans)
            {
                TruckJobTripPlan newplan = new TruckJobTripPlan();

                newplan = (TruckJobTripPlan)t.Clone();
                newTruckJobTrip.truckJobTripPlans.Add(newplan);
            }
            //20140314 - gerry added to clone 3 objects
            newTruckJobTrip.StartStop = (Stop)this.StartStop.Clone();
            newTruckJobTrip.EndStop = (Stop)this.EndStop.Clone();
            //201040502 - gerry change subCon from CustomerDTO to operatorDTO
            newTruckJobTrip.subCon = (OperatorDTO)this.subCon.Clone();

            return newTruckJobTrip;
        }

        public static SortableList<TruckJobTrip> GetJobTrips(JobTripStatus status, bool owntransport, string tptDeptCode, DateTime planDate)
        {
            return TruckJobDAL.GetJobTrips(status, owntransport, tptDeptCode, planDate);
        }

        public static SortableList<TruckJobTrip> GetJobTripsForPlanning(bool owntransport, string tptDeptCode, DateTime planDate, string jobNo)
        {
            return TruckJobDAL.GetJobTripsForPlanning(owntransport, tptDeptCode, planDate, jobNo);
        }
        //20161006 - jana - add date range filter
        public static SortableList<TruckJobTrip> GetJobTripsForPlanningFilterDateRange(bool owntransport, string tptDeptCode, DateTime planDate, DateTime ToDate, string jobNo)
        {
            return TruckJobDAL.GetJobTripsForPlanningFilterDateRange(owntransport, tptDeptCode, planDate, ToDate, jobNo);
        }

        public SortableList<TruckJobTripPlan> GetTruckJobTripPlans()
        {
            return TruckJobDAL.GetTruckJobTripPlans(this.JobID, this.Sequence);
        }
        public SortableList<TruckJobTripPlan> GetTruckJobTripPlans(SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.GetTruckJobTripPlans(this.JobID, this.Sequence, con, tran);
        }
        public bool SetJobTripStatusForTruck(JobTripState jobTripState, SqlConnection con, SqlTransaction tran, string user, string frmName, string oldStatus)
        {
            bool isSetted = false;
            try
            {
                this.AddJobTripStateForTruck(jobTripState, con, tran);
                if (TruckJobDAL.SetJobTripStatusForTruck(jobTripState.Status, this, con, tran))
                {
                    #region "2014-04-01 Zhou Kai comments out the logging block"
                    /*
                    //form Logheader and logdetails
                    DateTime serverDateTime = Logger.GetServerDateTime();

                    LogHeader logHeader = new LogHeader(FMModule.Trucking, frmName, serverDateTime, this.JobID.ToString(), this.Sequence.ToString(), FormMode.Edit, user);
                    LogDetail objLogDetail0 = new LogDetail("Changed Date", serverDateTime.ToShortDateString() + " " + serverDateTime.ToShortTimeString());
                    LogDetail objLogDetail1 = new LogDetail("Old Status", oldStatus);
                    LogDetail objLogDetail2 = new LogDetail("New Status", jobTripState.Status.ToString());
                    logHeader.LogDetails.Add(objLogDetail0);
                    logHeader.LogDetails.Add(objLogDetail1);
                    logHeader.LogDetails.Add(objLogDetail2);
                    Logger.WriteLog(logHeader, con, tran);
                    //tran.Commit();
                    
                     */
                    #endregion


                    isSetted = true;
                }
            }
            catch (FMException ex)
            {
                isSetted = false;
                throw ex;
            }
            catch (Exception ex)
            {
                isSetted = false;
                throw ex;
            }
            return isSetted;
        }

        public bool SetJobTripStatusForTruck(JobTripState jobTripState, string user, string frmName, string oldStatus)
        {
            bool setted = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                AddJobTripStateForTruck(jobTripState, con, tran);
                if (TruckJobDAL.SetJobTripStatusForTruck(jobTripState.Status, this, con, tran))
                {
                    #region "2014-04-02 Zhou Kai comments out the logging block, now the truckJobTrip status history is viewed by UI not by logs"
                    //form Logheader and logdetails
                    //DateTime serverDateTime = Logger.GetServerDateTime();

                    //LogHeader logHeader = new LogHeader(FMModule.Trucking, frmName, serverDateTime, this.JobID.ToString(), this.Sequence.ToString(), FormMode.Edit, user);
                    //LogDetail objLogDetail = new LogDetail("Job No", this.JobNo);
                    //LogDetail objLogDetail0 = new LogDetail("Changed Date", serverDateTime.ToShortDateString() + " " + serverDateTime.ToShortTimeString());
                    //LogDetail objLogDetail1 = new LogDetail("Old Status", oldStatus);
                    //LogDetail objLogDetail2 = new LogDetail("New Status", jobTripState.Status.ToString());
                    //logHeader.LogDetails.Add(objLogDetail0);
                    //logHeader.LogDetails.Add(objLogDetail1);
                    //logHeader.LogDetails.Add(objLogDetail2);
                    //Logger.WriteLog(logHeader, con, tran);
                    #endregion

                    tran.Commit();
                    setted = true;
                }
            }
            catch (FMException ex)
            {
                setted = false;
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                setted = false;
                tran.Rollback();
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return setted;
        }

        internal bool AddJobTripStateForTruck(JobTripState jobTripState)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ValidateJobTripStateForAdd(jobTripState.Status) == true)
                {
                        TruckJobDAL.AddTruckJobTripState(this, jobTripState, con, tran);
                        tran.Commit();
                        this.TripStatus = jobTripState.Status;
                        this.JobTripStates.Add(jobTripState);
                }
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        internal bool AddJobTripStateForTruck(JobTripState jobTripState, SqlConnection con, SqlTransaction tran)
        {
            bool temp = true;
            try
            {
                //25 June 2011 - Gerry Added
                //Add the new jobtripstate in memory then remove after validation even if valid or not
                //this.JobTripStates.Add(jobTripState);
                //end 25 June 2011
                if (ValidateJobTripStateForAdd(jobTripState.Status) == true)
                {
                    temp = TruckJobDAL.AddTruckJobTripState(this, jobTripState, con, tran);
                    this.TripStatus = jobTripState.Status;
                    this.UpdateVersion = TruckJobDAL.GetTruckJobTripUpdateVersion(this, con, tran);
                    this.JobTripStates.Add(jobTripState);
                }
            }
            catch (FMException ex)
            {
                temp = false;
                throw ex;
            }
            catch (Exception ex)
            {
                temp = false;
                throw ex;
            }
            return temp;
        }

        public bool CanChangeStatusToComplete(out string message)
        {
            bool temp = true;
            message = "";

            if (this.TripStatus != JobTripStatus.Assigned)
            {
                temp = false;
                message = message + TptResourceBLL.ErrCantSetStatusToCompleteIfCurrentStatusNotAssigned + "\n";
            }
            if (this.billing_UOM == "MT")
            {
                if (this.GetAllocatedWeightToPlan() < this.BookedWeight)
                {
                    temp = false;
                    message = message + TptResourceBLL.ErrTotalWeightNotFullyAllocated + "\n";
                }
            }
            if (this.billing_UOM == "CBM")
            {
                if (this.GetAllocatedVolToPlan() < this.BookedVol)
                {
                    temp = false;
                    message = message + TptResourceBLL.ErrTotalVolumeNotFullyAllocated;
                }
            }
            bool found = false;
            for (int i = 0; i < this.truckJobTripPlans.Count; i++)
            {
                if (this.truckJobTripPlans[i].status != JobTripStatus.Completed)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                temp = false;
                message = message + TptResourceBLL.ErrTruckJobTripPlansNotCompleted;
            }

            return temp;
        }

        public bool IsJobTripIsFullyAllocatedToPlan()
        {
            if (this.truckJobTripPlans.Count == 0)
            {
                return false;
            }
            else
            {
                //18 Jan 2012 - Gerry Modified <= because we allowed overload maybe planning weight/vol will exceed to jobtrip
                //if ((this.GetBalanceVolForPlan() == 0) && (this.GetBalanceWeightForPlan() == 0))
                if ((this.GetBalanceVolForPlan() <= 0) && (this.GetBalanceWeightForPlan() <= 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #region Methods For Criteria
        //Start      
        public static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status)
        {
            return TruckJobDAL.GetTruckJobTripsByStatus(status);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, bool ownTransport)
        {
            return TruckJobDAL.GetTruckJobTripsByStatusAndTransport(status, ownTransport);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, bool ownTransport, string deptCode, string jobNoFrom, string jobNoTo)
        {
            return TruckJobDAL.GetTruckJobTrips(status, ownTransport, jobNoFrom, jobNoTo, deptCode);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, string deptCode, string jobNoFrom, string jobNoTo)
        {
            return TruckJobDAL.GetTruckJobTrips(status, jobNoFrom, jobNoTo, deptCode);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, bool ownTransport, string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return TruckJobDAL.GetTruckJobTrips(status, ownTransport, planDateFrom, planDateTo, deptCode);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return TruckJobDAL.GetTruckJobTrips(status, planDateFrom, planDateTo, deptCode);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTrips(JobTripStatus status, string deptCode, bool ownTransport)
        {
            return TruckJobDAL.GetTruckJobTrips(status, deptCode, ownTransport);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTrips(bool ownTransport)
        {
            return TruckJobDAL.GetTruckJobTripsByTransport(ownTransport);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTrips(string deptCode, bool ownTransport)
        {
            return TruckJobDAL.GetTruckJobTripsByDeptAndTransport(deptCode, ownTransport);
        }
        public static SortableList<TruckJobTrip> GetTruckJobTripsByDeptAndStatus(JobTripStatus status, string deptCode)
        {
            return TruckJobDAL.GetTruckJobTripsByDeptAndStatus(status, deptCode);
        }
        public static SortableList<TruckJobTrip> GetTruckJobTripsByDept(string deptCode)
        {
            return TruckJobDAL.GetTruckJobTripsByDept(deptCode);
        }
        public static SortableList<TruckJobTrip> GetTruckJobTripsByJobs(string deptCode, string jobIDFrom, string jobIDTo)
        {
            return TruckJobDAL.GetTruckJobTripsByJob(deptCode, jobIDFrom, jobIDTo);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsByJobs(string jobIDFrom, string jobIDTo)
        {
            return TruckJobDAL.GetTruckJobTripsByJob(jobIDFrom, jobIDTo);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsByJobs(bool ownTransport, string jobIDFrom, string jobIDTo)
        {
            return TruckJobDAL.GetTruckJobTripsByJob(ownTransport, jobIDFrom, jobIDTo);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsByJobs(bool ownTransport, string deptCode, string jobNoFrom, string jobNoTo)
        {
            return TruckJobDAL.GetTruckJobTripsByJob(ownTransport, jobNoFrom, jobNoTo, deptCode);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsByDate(DateTime jobDateFrom, DateTime jobDateTo)
        {
            return TruckJobDAL.GetTruckJobTripsByDate(jobDateFrom, jobDateTo);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsByDate(JobTripStatus status, DateTime jobDateFrom, DateTime jobDateTo)
        {
            return TruckJobDAL.GetTruckJobTripsByDate(status, jobDateFrom, jobDateTo);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsByDate(bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            return TruckJobDAL.GetTruckJobTripsByDate(ownTransport, jobDateFrom, jobDateTo);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsByDate(JobTripStatus status, bool ownTransport, DateTime jobDateFrom, DateTime jobDateTo)
        {
            return TruckJobDAL.GetTruckJobTripsByDate(status, ownTransport, jobDateFrom, jobDateTo);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsByDate(string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return TruckJobDAL.GetTruckJobTripsByDate(deptCode, planDateFrom, planDateTo);
        }
        public static SortableList<TruckJobTrip> GetTruckJobTripsByDate(bool ownTransport, string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return TruckJobDAL.GetTruckJobTripsByDate(ownTransport, deptCode, planDateFrom, planDateTo);
        }
        public static SortableList<TruckJobTrip> GetTruckJobTripsForSetBookedToReady(bool hasSubCon, string deptCode, DateTime planDateFrom, DateTime planDateTo)
        {
            return TruckJobDAL.GetTruckJobTripsForSetBookedToReady(hasSubCon, deptCode, planDateFrom, planDateTo);
        }
        //end for criteria
        #endregion

        public bool CanChangeStatus(JobTripStatus oldTripStatus, JobTripStatus newTripStatus, bool ownTransport, out string message)
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
            else
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
                    if (newTripStatus != JobTripStatus.Assigned || newTripStatus != JobTripStatus.Completed)
                    {
                        message = TptResourceBLL.ErrCanSetOnlyToAssignOrComplete;
                        validationCheck = false;
                    }
                }
                else if (oldTripStatus == JobTripStatus.Assigned)
                {
                    if (newTripStatus != JobTripStatus.Completed)
                    {
                        message = TptResourceBLL.ErrCanSetOnlyToComplete;
                        validationCheck = false;
                    }
                }
            }
            return validationCheck;
        }

        public static bool SetAllJobTripsFromBookedToReady(SortableList<TruckJobTrip> truckJobTrips, string user, string frmName, string oldStatus)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try
            {
                JobTripState truckJobTripState = new JobTripState();
                truckJobTripState.Seq_No = 1;
                truckJobTripState.Status = JobTripStatus.Ready;
                truckJobTripState.StatusDate = DateTime.Now.Date;
                truckJobTripState.IsNew = true;

                for (int i = 0; i < truckJobTrips.Count; i++)
                {
                    if (truckJobTrips[i].AddJobTripStateForTruck(truckJobTripState))
                    {
                        /* 2014-04-07 Zhou Kai adds
                        //form Logheader and logdetails
                        DateTime serverDateTime = Logger.GetServerDateTime();

                        LogHeader logHeader = new LogHeader(FMModule.Trucking, frmName, serverDateTime, truckJobTrips[i].JobID.ToString(), truckJobTrips[i].Sequence.ToString(), FormMode.Edit, user);
                        LogDetail objLogDetail0 = new LogDetail("Changed Date", serverDateTime.ToShortDateString() + " " + serverDateTime.ToShortTimeString());
                        LogDetail objLogDetail1 = new LogDetail("Old Status", oldStatus);
                        LogDetail objLogDetail2 = new LogDetail("New Status", truckJobTripState.Status.ToString());
                        logHeader.LogDetails.Add(objLogDetail0);
                        logHeader.LogDetails.Add(objLogDetail1);
                        logHeader.LogDetails.Add(objLogDetail2);
                        Logger.WriteLog(logHeader, con, trans);
                         */ 
                    }
                }

                trans.Commit();
                return true;
            }
            catch (FMException ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public byte[] GetTruckJobTripUpdatedVersion(SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.GetTruckJobTripUpdateVersion(this, con, tran);
        }
        public byte[] GetTruckJobTripUpdatedVersion()
        {
            byte[] updatedVersion;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State != ConnectionState.Open) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                updatedVersion = TruckJobDAL.GetTruckJobTripUpdateVersion(this, con, tran);

                tran.Commit();
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw new FMException(ex.Message.ToString());
            }
            finally { if (con.State != ConnectionState.Closed) { con.Close(); } }
            return updatedVersion;
        }

        public static ArrayList GetJobNosForPlanningReport(string driver, DateTime scheduleDate, string deptCode)
        {
            return TruckJobDAL.GetJobNosForPlanningReport(driver, scheduleDate, deptCode);
        }

        public static ArrayList GetJobSeqNosForPlanningReport(string driver, DateTime scheduleDate, int jobID, string deptCode)
        {
            return TruckJobDAL.GetJobTripSeqNosForPlanningReport(driver, scheduleDate, jobID, deptCode);
        }
        //20130406 gerry added
        public decimal GetBillingQty(Charge charge, string uom)
        {
            //Do logic here;
            //logic not yet completed, need to take quotation or not
            decimal billQty = 1;
            if (charge != null)
            {
                //if (charge.IsInvoiceAmtDependentOnWtVol)
                if (charge.invoiceChargeType == InvoiceChargeType.DependOnWeightVolume)
                {
                    if (uom.Equals(TruckMovementUOM_WtVol.CBM.ToString()))
                    {
                        billQty = this.actualVol;
                    }
                    else if (uom.Equals(TruckMovementUOM_WtVol.MT.ToString())
                        || uom.Equals(TruckMovementUOM_WtVol.KGM.ToString()))
                    {
                        billQty = this.actualWeight;
                    }
                }
            }
            return billQty;
        }
        //20130406 end

        // 2013-09-09 Zhou Kai adds function
        public static bool UpdateAcutalWeightVolumeforTruckJobTrip(int jobID, int jobTripSeqNo,
                                                                   SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            retValue = TruckJobDAL.UpdateAcutalWeightVolumeforTruckJobTrip(jobID, jobTripSeqNo, con, tran);
            return retValue;
        }
        public static bool UpdateAcutalWeightVolumeforTruckJobTrip(int jobID, int jobTripSeqNo)
        {
            bool retValue = false;
            retValue = TruckJobDAL.UpdateAcutalWeightVolumeforTruckJobTrip(jobID, jobTripSeqNo);
            return retValue;
        }
        // 2013-09-09 Zhou Kai ends

        /*
         * 2013-10-24 Zhou Kai starts to modify
         */
        public bool GetActualStartEndTime(string earliestStartTime, string latestEndTime,
                                                 SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            retValue = TruckJobDAL.GetActualStartEndTime(this.JobID, this.Sequence, earliestStartTime, latestEndTime, con, tran);
            return retValue;
        }
        public bool GetActualStartEndTime(out string earliestStartTime, out string latestEndTime)
        {
            return TruckJobDAL.GetActualStartEndTime(this.JobID, this.Sequence, out earliestStartTime, out latestEndTime);
        }
        // 2013-09-12 ends

        /*
         * 2013-09-12 Zhou Kai adds function
         */
        public static string To4BitTimeFormat(DateTime origDateTime)
        {
            return TruckJobDAL.To4BitTimeFormat(origDateTime);
        }

        public static SortableList<TruckJobTrip> GetTruckJobTripsToChangeStatus(int currentStatus, bool? ownTransport, DateTime? jobDateFrom, DateTime? jobDateTo, string jobNoFrom, string jobNoTo, string deptCode)
        {
            return TruckJobDAL.GetTruckJobTripsToChangeStatus(currentStatus, ownTransport, jobDateFrom, jobDateTo, jobNoFrom, jobNoTo, deptCode);
        }
        //20131009 - Gerry added to update job trip status
        //20140314 - gerry tranfer the sql transaction to DAL method for individual job trips
        public static bool UpdateJobTripStatus(SortableList<TruckJobTrip> jobTrips)
        {
            try
            {
                if (jobTrips.Count > 0)
                {
                    foreach (TruckJobTrip temp in jobTrips)
                    {
                        //TruckJobDAL.UpdateJobTripStatus(temp.JobID, temp.Sequence, temp.TripStatus);
                        TruckJobDAL.UpdateJobTripStatus(temp);
                    }
                }
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        // 2013-10-16 Zhou Kai adds
        public bool DoTruckJobTripSubConExist()
        {
            return TruckJobDAL.DoTruckJobTripSubConExist(JobID, Sequence);
        }
        // 2013-10-16 Zhou Kai ends

        // 2013-11-29 Zhou Kai adds methods to get the status record list for a truck job trip
        public List<JobTripState> GetTruckJobTripStatusHistory(int jobId, int jobTripSeqNo)
        {
            return TruckJobDAL.GetTruckJobTripStatusHistory(jobId, jobTripSeqNo);
        }
        // 2013-12-02 Zhou Kai ends

        // 2013-12-24 Zhou Kai adds
        public static List<string> GetAllTruckJobTripSeqNoByJobNumber(string jobNumber)
        {
            List<string> lstTruckJobTripNo = new List<string>();
            SortableList<TruckJobTrip> truckJobTrips = new SortableList<TruckJobTrip>();
            truckJobTrips = TruckJobTrip.GetTruckJobTripsByJobs(jobNumber, jobNumber);
            foreach (TruckJobTrip tjt in truckJobTrips)
            {
                lstTruckJobTripNo.Add(tjt.Sequence.ToString());
            }

            return lstTruckJobTripNo;
        }

        public static SortableList<string> GetTruckJobTripDetailSeqNo(string jobNo, string jobTripSeqNo)
        {
            int nJobTripSeqNo = 0;
            SortableList<string> lstTruckJobTripDetailSeqNo = new SortableList<string>();
            SortableList<TruckJobTripDetail> lstTruckJobTripDetails = new SortableList<TruckJobTripDetail>();
            if (Int32.TryParse(jobTripSeqNo, out nJobTripSeqNo))
            {
                lstTruckJobTripDetails = TruckJobTrip.GetTruckJobTripDetails(jobNo, nJobTripSeqNo);
                foreach (TruckJobTripDetail tjtd in lstTruckJobTripDetails)
                {
                    lstTruckJobTripDetailSeqNo.Add(tjtd.detailSequence.ToString());
                }
            }

            return lstTruckJobTripDetailSeqNo;
        }

        public static List<string> GetTruckJobTripDeletedDetailSeqNo(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllDeletedDetailSeqNoByJobNoAndTripSeqNo(dict);
        }
        // 2013-12-24 Zhou Kai ends

        //20140304 - gerry added
        public bool AddorChangeSubContractorFromPlanning()
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (ValidateAddSubContractorFromPlanning())
                {
                    TruckJobDAL.AddorChangeSubContractorFromPlanning(this, con, tran);
                    //get update version
                    this.UpdateVersion = TruckJobDAL.GetTruckJobTripUpdateVersion(this, con, tran);
                    //add jobtrip state
                    JobTripState jobTripState = new JobTripState(1, JobTripStatus.Assigned, DateTime.Now, "", true);
                    this.AddJobTripStateForTruck(jobTripState, con, tran);
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

        #region ""
        public bool AddorChangeSubContractorFromPlanning(SUBCONCHANGES subConCodeChanges)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlTransaction tran = null;
            try
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                if (ValidateAddSubContractorFromPlanning())
                {
                    TruckJobTrip origTruckJobTrip = TruckJobDAL.GetTruckJobTrip(this.JobID, this.Sequence, con, tran);
                    TruckJobDAL.AddorChangeSubContractorFromPlanning(this, con, tran);
                    #region "Logic for sub-contractor changes"
                    if (subConCodeChanges == SUBCONCHANGES.BLANKTOA)
                    {
                        // add the truck job trip by sub-contractor
                        this.OwnTransport = false; // 2014-03-26 Zhou Kai adds the line on the left
                        this.AddTruckJobTripSubCon(con, tran);
                    }
                    if (subConCodeChanges == SUBCONCHANGES.ATOBLANK)
                    {
                        // delete the corrsoponding truck job trip by sub-contractor
                        this.OwnTransport = true; // 2014-03-26 Zhou Kai adds the line on the left
                        this.DeleteTruckJobTripBySubCon(con, tran);
                    }
                    #endregion

                    //get update version
                    this.UpdateVersion = TruckJobDAL.GetTruckJobTripUpdateVersion(this, con, tran);

                    //add jobtrip state
                    JobTripState jobTripState = new JobTripState(1, /*TrailerStatus.Assigned - 20140324 gerry replaced with the status*/ this.TripStatus, DateTime.Now, "", true);
                    this.AddJobTripStateForTruck(jobTripState, con, tran);


                    ////20161024 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.JobNo, "TRK", "BK", this.JobID, this.LastModified_By, DateTime.Now, "Job Trip", this.Sequence, "ADD");
                    auditLog.WriteAuditLog("Subcon", this.subCon.Code,origTruckJobTrip.subCon.Code, con, tran);
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
        #endregion

        //20140314 - gerry added to validate adding subcon
        public bool ValidateAddSubContractorFromPlanning()
        {
            //20140320 - gerry remove this validation because assigned status can be assigne to different sub con
            //it is being filtered from the UI
            //if (this.TripStatus != JobTripStatus.Ready)
            //    throw new FMException(TptResourceBLL.ErrCantAddSubConStatusNotReady);
            if (AreThereAnyTruckJobTripPlan())
                throw new FMException(TptResourceBLL.ErrCantAddSubConAlreadyInPlan);
            //20140324 - gerry added validation
            if (this.subCon.Code != string.Empty)
            {
                if (this.TripStatus != JobTripStatus.Assigned)
                    throw new FMException(TptResourceBLL.ErrSetSubconTripNotAssignedStatus); //"The job trip was being assigned to a subcon the status must be ASSIGNED. ");
            }

            return true;
        }
        //20140314 - gerry added to validate adding subcon
        public bool AreThereAnyTruckJobTripPlan()
        {
            return TruckJobDAL.AreThereAnyTruckJobTripPlan(this);
        }
        //20140319 - gerry added to get jobNos for subcontractor jobs
        public static ArrayList GetSubContractedTruckTripJobNo(int status)
        {
            return TruckJobDAL.GetSubContractedTruckTripJobNo(status);
        }
        //20140319 - gerry added to get jobtrip seqNos for job id
        public static ArrayList GetSubContractedTruckTripSeqNo(int jobId, int status)
        {
            return TruckJobDAL.GetSubContractedTruckTripSeqNo(jobId, status);
        }

        // 2014-03-21 Zhou Kai adds functions
        public bool DeleteTruckJobTripBySubCon(SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.DeleteTruckJobTripBySubCon(this.JobID, this.Sequence, con, tran);
        }
        private DateTime GetDateTime(DateTime datePart, string timePart)
        {
            return Convert.ToDateTime(DateUtility.ConvertDateForSQLPurpose(datePart)
                                      + " " + timePart.Insert(2, ":"));
        }

        public bool AddTruckJobTripSubCon(SqlConnection cn, SqlTransaction tran)
        {
            return TruckJobDAL.AddTruckJobTripSubCon(this, cn, tran);
        }

        public TruckJobTripSubCon GetTruckJobTripSubCon()
        {
            return TruckJobDAL.GetTruckJobTripSubCon(this.JobID, this.Sequence);
        }

        public TruckJobTripSubCon GetTruckJobTripSubCon(int jobId, int jobTripSeqNo)
        {
            return TruckJobDAL.GetTruckJobTripSubCon(jobId, jobTripSeqNo);
        }
        // 2014-03-24 Zhou Kai ends

        //20160224 - gerry added
        public static JobTripStatus GetTruckJobTripStatus(int jobID, int SeqNo, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.GetTruckJobTripStatus(jobID, SeqNo, con, tran);
        }
        public static SortableList<JobTripState> GetAllTruckJobTripStates(TruckJobTrip truckJobTrip, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.GetAllTruckJobTripStates(truckJobTrip, con, tran);
        }
    }
}
