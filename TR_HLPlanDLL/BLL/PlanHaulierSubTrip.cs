using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_HLBookDLL.BLL;
using FM.TR_HLPlanDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using System.Data;
using System.Data.SqlClient;
using TR_MessageDLL.BLL;

namespace FM.TR_HLPlanDLL.BLL
{
    public class PlanHaulierSubTrip : PlanSubTrip, ICloneable
    {
        private Vehicle trailer;
        private SortableList<HaulierJobTrip> jobTrips;
        //20140120 - Gerry added
        public string containerNos { get; set; } 
        //end 20140120
        public string IncentiveCode { get; set; }
        public decimal IncentiveQty { get; set; }
        public string IncentiveRemarks { get; set; }
        public bool HasOT { get; set; }
        public List<Incentive> OtherIncentives { get; set; }

        //2 properties added because we cannot pass object inside databinding source
        //public String TrailerType { get; set; }
        //public String TrailerNumber { get; set; }      

        public Vehicle Trailer
        {
            get { return trailer; }
            set { trailer = value; 
                  //TrailerType = trailer.TrailerType.ToString();
                  //TrailerNumber = trailer.Number.ToString();
            }
        }

        public SortableList<HaulierJobTrip> JobTrips
        {
            get { return jobTrips; }
            set { jobTrips = value; }
        }

        public PlanHaulierSubTrip()
            : base()
        {
            this.trailer = new Vehicle();
            this.jobTrips = new SortableList<HaulierJobTrip>();
            this.IncentiveCode = string.Empty;
            this.IncentiveRemarks = string.Empty;
            this.IncentiveQty= 1;
            this.OtherIncentives = new List<Incentive>();
            this.HasOT = false;
        }
        

        public PlanHaulierSubTrip(int seqNo, DateTime start, DateTime end,
            Stop startStop, Stop endStop, bool isBillableTrip, string description,
            SortableList<HaulierJobTrip> jobTrips,
            Vehicle trailer, PlanHaulierTrip trip, JobTripStatus status)
            : base(seqNo, start, end, startStop, endStop, isBillableTrip, description, trip, status)
        {
            this.JobTrips = jobTrips;
            this.Trailer = trailer;
            this.IncentiveCode = string.Empty;
            this.IncentiveRemarks = string.Empty;
            this.IncentiveQty = 1;
            this.OtherIncentives = new List<Incentive>();
            this.HasOT = false;
        }


        public bool AreSubTripsOverlapping()
        {
            return false;
        }


        public static SortableList<PlanHaulierSubTrip> GetAllPlanHaulierSubTrips(SortableList<PlanHaulierTrip> planHaulierTrips, DateTime date)
        {
            SortableList<PlanHaulierSubTrip> planHaulierSubTrips = new SortableList<PlanHaulierSubTrip>();

            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                SortableList<PlanHaulierSubTrip> subTrips = planHaulierTrips[i].HaulierSubTrips;
                for (int j = 0; j < subTrips.Count; j++)
                {
                    subTrips[j].containerNos = SetContainerNos(subTrips[j]);   //add 20140120
                    planHaulierSubTrips.Add(subTrips[j]);
                }
            }
            return planHaulierSubTrips;
        }
        public static PlanHaulierSubTrips GetAllPlanHaulierSubTripsByDateAndVehicleNumber(SortableList<PlanHaulierTrip> planHaulierTrips, DateTime date, string vehicleNo)
        {
            PlanHaulierSubTrips planHaulierSubTrips = new PlanHaulierSubTrips();

            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                SortableList<PlanHaulierSubTrip> subTrips = planHaulierTrips[i].HaulierSubTrips;
                for (int j = 0; j < subTrips.Count; j++)
                {
                    if (planHaulierTrips[i].Vehicle.Number == vehicleNo)
                    {
                        subTrips[j].containerNos = SetContainerNos(subTrips[j]);   //add 20150515
                        planHaulierSubTrips.Add(subTrips[j]);
                    }
                }
            }

            return planHaulierSubTrips;
        }

        public static PlanHaulierSubTrips GetAllPlanHaulierSubTripsByDateAndVehicle(SortableList<PlanHaulierTrip> planHaulierTrips, DateTime date, Vehicle vehicle)
        {
            PlanHaulierSubTrips planHaulierSubTrips = new PlanHaulierSubTrips();

            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                SortableList<PlanHaulierSubTrip> subTrips = planHaulierTrips[i].HaulierSubTrips;
                for (int j = 0; j < subTrips.Count; j++)
                {
                    if (planHaulierTrips[i].Vehicle.Number == vehicle.Number)
                    {
                        subTrips[j].containerNos = SetContainerNos(subTrips[j]);   //add 20140120
                        planHaulierSubTrips.Add(subTrips[j]);
                    }
                }
            }

            return planHaulierSubTrips;
        }

        public static PlanHaulierSubTrips GetAllPlanHaulierSubTripsByDateAndDriver(SortableList<PlanHaulierTrip> planHaulierTrips, DateTime date, Driver driver)
        {
            PlanHaulierSubTrips planHaulierSubTrips = new PlanHaulierSubTrips();

            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                SortableList<PlanHaulierSubTrip> subTrips = planHaulierTrips[i].HaulierSubTrips;
                for (int j = 0; j < subTrips.Count; j++)
                {
                    if (planHaulierTrips[i].Driver.Code == driver.Code)
                    {
                        subTrips[j].containerNos = SetContainerNos(subTrips[j]);   //add 20140120
                        planHaulierSubTrips.Add(subTrips[j]);
                    }
                }
            }

            return planHaulierSubTrips;
        }

        public static PlanHaulierSubTrips GetAssignedPlanHaulierSubTripsByDriver(PlanHaulierTrip currentPlanTrip)
        {
            PlanHaulierSubTrips planHaulierSubTrips = new PlanHaulierSubTrips();
            foreach (PlanHaulierSubTrip subTrip in currentPlanTrip.HaulierSubTrips)
            {
                if (subTrip.Status == JobTripStatus.Assigned)
                {
                    subTrip.containerNos = SetContainerNos(subTrip);
                    planHaulierSubTrips.Add(subTrip);
                }
            }         

            return planHaulierSubTrips;
        }
        public static PlanHaulierSubTrips GetCompletedPlanHaulierSubTripsByDriver(SortableList<PlanHaulierTrip> planHaulierTrips, Driver driver)
        {
            PlanHaulierSubTrips planHaulierSubTrips = new PlanHaulierSubTrips();

            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                SortableList<PlanHaulierSubTrip> subTrips = planHaulierTrips[i].HaulierSubTrips;
                for (int j = 0; j < subTrips.Count; j++)
                {
                    if (planHaulierTrips[i].Driver.Code == driver.Code && subTrips[j].Status == JobTripStatus.Completed)
                    {
                        subTrips[j].containerNos = SetContainerNos(subTrips[j]);   //add 20140120
                        planHaulierSubTrips.Add(subTrips[j]);
                    }
                }
            }

            return planHaulierSubTrips;
        }
       
        
        
        public bool IsVehicleAlreadyUsed(SortableList<PlanHaulierTrip> planHaulierTrips, Vehicle vehicle, DateTime start)
        {
            for (int i = 0; i < planHaulierTrips.Count; i++)
            {
                for (int j = 0; j < planHaulierTrips[i].HaulierSubTrips.Count; i++)
                {
                    if ((DateTime.Compare(start, planHaulierTrips[i].HaulierSubTrips[j].Start) > 0) & (DateTime.Compare(start, planHaulierTrips[i].HaulierSubTrips[j].End) < 0))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Method is used to copy this to another PlanHaulierSubTrip
        public object Clone()
        {
            PlanHaulierSubTrip newPlanHaulierSubTrip = new PlanHaulierSubTrip();
            // to save time, will copy only properties required for comparing 
            // the List<HaulierJobTrips> 
            newPlanHaulierSubTrip.SeqNo = this.SeqNo;
            //20150814 - gerry added
            newPlanHaulierSubTrip.Status = this.Status;
            newPlanHaulierSubTrip.DriverNumber = this.DriverNumber;
            newPlanHaulierSubTrip.VehicleNumber = this.VehicleNumber;
            newPlanHaulierSubTrip.Trailer = this.trailer;
            newPlanHaulierSubTrip.StartStop =(Stop)this.StartStop.Clone();
            newPlanHaulierSubTrip.EndStop = (Stop)this.EndStop.Clone();
            newPlanHaulierSubTrip.Start = this.Start;
            newPlanHaulierSubTrip.End = this.End;
            newPlanHaulierSubTrip.IncentiveCode = this.IncentiveCode;
            newPlanHaulierSubTrip.OtherIncentives = this.OtherIncentives;
            //end 20150814

            foreach (HaulierJobTrip haulierJobTrip in JobTrips)
            {
                HaulierJobTrip newHaulierJobTrip = new HaulierJobTrip();
                newHaulierJobTrip = (HaulierJobTrip) haulierJobTrip.Clone();
                newPlanHaulierSubTrip.JobTrips.Add(newHaulierJobTrip);
            }
            return newPlanHaulierSubTrip;
        }

        //20140120 - WYN2000 change to display container nos.
        public static string SetContainerNos(PlanHaulierSubTrip planSubTrip)
        {
            string retValue = "-";
            try
            {
                if (planSubTrip.jobTrips.Count > 0)
                {
                    if (planSubTrip.jobTrips.Count == 1 && !planSubTrip.jobTrips[0].ContainerNo.Equals(string.Empty))
                        retValue = planSubTrip.jobTrips[0].ContainerNo.ToString();
                    else if (planSubTrip.jobTrips.Count > 1)
                    {
                        int count = 0;
                        foreach (HaulierJobTrip jobTrip in planSubTrip.jobTrips)
                        {
                            if (!jobTrip.ContainerNo.Equals(string.Empty))
                            {
                                if (count == 0)
                                    retValue = jobTrip.ContainerNo.ToString();
                                else
                                    retValue += "+" + jobTrip.ContainerNo.ToString();
                            }
                            count++;
                        }
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
        //20140128 - gerry added to check if container weight exceed in trailer weight capacity
        public bool IsTrailerOverload(decimal trailerCapacity, out decimal exceedWeight)
        {
            bool retValue = false;
            exceedWeight = 0;
            decimal totalCargoWt = 0;
            try
            {
                foreach (HaulierJobTrip jobTrip in this.JobTrips)
                {
                    HaulierJob job = HaulierJob.GetHaulierJob(jobTrip.JobNo);
                    if (jobTrip.LegType == Leg_Type.FirstOfTwoLeg && job.JobType == HaulierJob.HAULIER_SEA_EXPORT)
                        totalCargoWt += HaulierJobTrip.GetHaulierJobTrip(jobTrip.JobID, jobTrip.PartnerLeg).GrossWeight;
                    else
                        totalCargoWt += jobTrip.GrossWeight;
                }
                exceedWeight = totalCargoWt - trailerCapacity;
                retValue = exceedWeight > 0 ? true : false;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message); }
            return retValue;
        }

        #region "Codes for Messaging System"
        //// 2015-04-15 update
        public static TripInstruction02 CreateTripInstruction_OLD(PlanHaulierSubTrip subPlanTripToSend, HaulierJobTrip tripToBeSent, string plannerId,
            string remark)
        {
            /*
             * (1) check the sub plan trip:
             *   a. if it does not contain any job trips, throw exception
             *   b. if it contains more than 2 trips, throw exception
             *   c. if it contains 2 trips, check if they share the same start and end stop, and the each of them
             *     carries a 20 foot container, otherwise, throw exception
             * (2) if the sub plan trip is valid to send trip instruction, generate a trip instruction:
             *   a. generate the trip instruction according to the configuration file Message.json 
             */

            TripInstruction02 instruction = new TripInstruction02();
            // (1) check if the plan haulier sub trip contains only 1 job trip, now the system can only handle one trip per message
            //if (subPlanTripToSend.JobTrips.Count != 1)
            //    throw new FMException("The current messaging system only supports 1 trip each message.");

            HaulierJob jobTheSentTripBelongsTo = HaulierJob.GetHaulierJob(tripToBeSent.JobNo);
            instruction.MessageCode = "TI01";
            // (2) get prime mover number and driver code, and more
            instruction.PrimeMover = subPlanTripToSend.VehicleNumber;
            // bug here: for USS HeadNo is sourcerefNo, but for other companies, it may be JobNo
            //instruction.HeadNo = jobTheSentTripBelongsTo.SourceReferenceNo == string.Empty ? tripToBeSent.JobNo : jobTheSentTripBelongsTo.SourceReferenceNo;
            instruction.HeadNo = tripToBeSent.JobNo; // now send the job number to be standardize
            instruction.SeqNo = tripToBeSent.Sequence.ToString();
            //20150519 - gerry added
            instruction.BookRefNo = jobTheSentTripBelongsTo.BookingRef;
            //instruction.PlannerID = plannerId;
            //instruction.PlanTripSeqNo = subPlanTripToSend.SeqNo.ToString();
            //instruction.DriverCode = subPlanTripToSend.DriverNumber;
            instruction.MsgTripStatus = MessageTripStatus.Received;
            instruction.Sender = plannerId;
            instruction.Remark = remark;

            //201506 - gerry standardized message body
            if (tripToBeSent.LegGroupMember == 1)
                instruction.MessageBody = "COL ";
            else
                instruction.MessageBody = "DEL ";
            //standardized the message body both Export,Import or Local Jobs
            instruction.MessageBody += subPlanTripToSend.Start.ToString("yyyy-MM-dd") + " " + // ScheduleDate
                        subPlanTripToSend.Start.ToString("HH:mm") + " " + // TripStartTime
                        subPlanTripToSend.End.ToString("HH:mm") + " " + // TripEndTime
                        jobTheSentTripBelongsTo.VesselNo + " " + // Vessel
                        jobTheSentTripBelongsTo.voyageNo + " " +  // Voyage
                        jobTheSentTripBelongsTo.SourceReferenceNo + " " + // BookRefNo
                        jobTheSentTripBelongsTo.ShippingLine + " " +  // ShippingLine
                        jobTheSentTripBelongsTo.POD + " " +  // PortOfDestination
                        "1 " + // ContainerQty, 
                        HaulierJob.containerCodeSizeMapping[tripToBeSent.ContainerCode] + " " + // ContainerSize
                        tripToBeSent.ContainerCode + " " + // ContainerCode
                        tripToBeSent.StartStop.Code + " " + // FromLoacation
                        tripToBeSent.EndStop.Code + " " + // ToLocation
                        plannerId + " " + // plannerId,
                        remark; // remark

            #region 20160519 - Gerry Removed and replaced code above for Standard message body
            /*
            // (3) detect the job trip type
            // bug here: Mr. Lam proposes to add short labels before each property value
            //if (jobTheSentTripBelongsTo.JobType.Equals("HLSE"))
            //{
                if (tripToBeSent.LegGroupMember == 1)
                {
                    // Sea-Export 1st Leg
                    instruction.MessageBody = "COL " +  // MsgSubCode
                        subPlanTripToSend.Start.ToString("yyyy-MM-dd") + " " + // ScheduleDate
                        subPlanTripToSend.Start.ToString("HH:mm") + " " + // TripStartTime
                        subPlanTripToSend.End.ToString("HH:mm") + " " + // TripEndTime
                        jobTheSentTripBelongsTo.VesselNo + " " + // Vessel
                        jobTheSentTripBelongsTo.voyageNo + " " +  // Voyage
                        jobTheSentTripBelongsTo.SourceReferenceNo +  // BookRefNo
                        jobTheSentTripBelongsTo.ShippingLine + " " +  // ShippingLine
                        jobTheSentTripBelongsTo.POD + " " +  // PortOfDestination
                        "1 " + // ContainerQty, bug here, double mount is not considered
                        HaulierJob.containerCodeSizeMapping[tripToBeSent.ContainerCode] +
                        " " + // ContainerSize
                        tripToBeSent.ContainerCode + " " + // ContainerCode
                        tripToBeSent.StartStop.Code + " " + // FromLoacation
                        tripToBeSent.EndStop.Code + " " + // ToLocation
                        plannerId + " " + // plannerId, bug here: the planner_id shoud be
                                          // put in message head
                        remark; // remark
                }
                else if (tripToBeSent.LegGroupMember > 1)
                {
                    // Sea-Export 2nd Leg
                    instruction.MessageBody = "EXP " + // MsgSubCode
                        subPlanTripToSend.Start.ToString("yyyy-MM-dd") + " " + // ScheduleDate
                        subPlanTripToSend.Start.ToString("HH:mm") + " " +  // TripStartTime
                        subPlanTripToSend.End.ToString("HH:mm") + " " + // TripEndTime
                        subPlanTripToSend.containerNos + " " + // ContainerNo1
                        "NA " + // ContainerNo2
                        tripToBeSent.SealNo + " " + // SealNo1 
                        "NA " + // SealNo2 
                        tripToBeSent.StartStop.Code + " " +
                        tripToBeSent.EndStop.Code + " " +
                        plannerId + " " + // bug here, plannerId should be in msg head
                        remark;
                }
                //else
                //{
                //    throw new FMException("Please check the leg group member, "
                //    + "the messaging system now supports only 1 and 2.");
                //}
            //}
            //if (jobTheSentTripBelongsTo.JobType.Equals("HLSI"))
            //{
             //   throw new FMException("Not implemented.");
            //}
            //else if (jobTheSentTripBelongsTo.JobType.Equals("HLLO"))
            //{
            //    throw new FMException("Not implemented.");
            //}
            //else
            //{
            //    throw new FMException("The job type is wrongly set, thus, " +
            //        "cannot generate a correct trip instruction for it.");
                //}

            */
            #endregion

            return instruction;
        }
        //20150908 - Gerry overload the method to create instruction for double mount
        public static TripInstruction02 CreateTripInstruction(PlanHaulierSubTrip planSubTripToSend, string plannerId, string remark)
        {
            TripInstruction02 instruction = new TripInstruction02();
            try
            {
                instruction.MessageCode = "TI01";
                instruction.PrimeMover = planSubTripToSend.VehicleNumber;
                instruction.MsgTripStatus = MessageTripStatus.Received;
                instruction.Sender = plannerId;
                instruction.Remark = remark.Length > 50 ? remark.Substring(0,50) : remark;
                string body = @"BK:{0} VSL:{1} VOY:{2} {3} FRM:{4} TO:{5} AGNT:{6} QTY:{7} RMK:{8}";
                //for the same JOB order as default
                HaulierJob jobTheSentTripBelongsTo = HaulierJob.GetHaulierJob(planSubTripToSend.jobTrips[0].JobNo);
                instruction.SeqNo = planSubTripToSend.jobTrips[0].Sequence.ToString();
                instruction.HeadNo = planSubTripToSend.jobTrips[0].JobNo;
                instruction.BookRefNo = planSubTripToSend.jobTrips[0].BookRefNo;
                string VSL = jobTheSentTripBelongsTo.VesselName;
                string VOY = jobTheSentTripBelongsTo.voyageNo;
                string PRT = jobTheSentTripBelongsTo.JobType == "HLSE" ? "POD:" : "POL:";
                PRT += jobTheSentTripBelongsTo.JobType == "HLSE" ? jobTheSentTripBelongsTo.PodName : (jobTheSentTripBelongsTo.JobType == "HLSI" ? jobTheSentTripBelongsTo.PolName : string.Empty);
                string AGENT = jobTheSentTripBelongsTo.ShippingLine;
                string LOC_FRM = planSubTripToSend.jobTrips[0].StartStop.Description;
                string LOC_TO = planSubTripToSend.jobTrips[0].EndStop.Description;
                string strQty = "1x" + planSubTripToSend.jobTrips[0].ContainerCode;
                //20170729 for SEA IMPORT
                string CONT_NO = string.Empty;
                string SEAL_NO = string.Empty;
                if (jobTheSentTripBelongsTo.JobType == "HLSI")
                {
                    CONT_NO = planSubTripToSend.jobTrips[0].ContainerNo;
                    SEAL_NO = planSubTripToSend.jobTrips[0].SealNo;
                }
                //20170729 for SEA IMPORT end
                //for double mounted and different job order
                if (planSubTripToSend.jobTrips.Count > 1)
                {
                    instruction.SeqNo += "+" + planSubTripToSend.jobTrips[1].Sequence.ToString();
                    if (planSubTripToSend.jobTrips[0].JobNo != planSubTripToSend.jobTrips[1].JobNo)
                    {
                        instruction.HeadNo += "+" + planSubTripToSend.jobTrips[1].JobNo;
                        instruction.BookRefNo += "+" + planSubTripToSend.jobTrips[1].BookRefNo;
                        jobTheSentTripBelongsTo = HaulierJob.GetHaulierJob(planSubTripToSend.jobTrips[1].JobNo);
                        //message body
                        if (VSL != jobTheSentTripBelongsTo.VesselName)
                            VSL += " " + jobTheSentTripBelongsTo.VesselName;
                        if (VOY != jobTheSentTripBelongsTo.voyageNo)
                            VOY += " " + jobTheSentTripBelongsTo.voyageNo;
                        if (AGENT != jobTheSentTripBelongsTo.ShippingLine)
                            AGENT += " " + jobTheSentTripBelongsTo.ShippingLine;

                        if (jobTheSentTripBelongsTo.JobType == "HLSE" && PRT != jobTheSentTripBelongsTo.PodName)
                            PRT += " " + jobTheSentTripBelongsTo.PodName;
                        else if (jobTheSentTripBelongsTo.JobType == "HLSI" && PRT != jobTheSentTripBelongsTo.PolName)
                            PRT += " " + jobTheSentTripBelongsTo.PolName;

                    }
                    //20170315 - gerry changed stop code to description.
                    LOC_FRM += planSubTripToSend.jobTrips[1].StartStop.Description.Equals(LOC_FRM) ? string.Empty : (" " + planSubTripToSend.jobTrips[1].StartStop.Code);
                    LOC_TO += planSubTripToSend.jobTrips[1].EndStop.Description.Equals(LOC_TO) ? string.Empty : (" " + planSubTripToSend.jobTrips[1].EndStop.Code);
                    strQty = "2x" + planSubTripToSend.jobTrips[0].ContainerCode;
                    //20170720 for SEA IMPORT
                    if (jobTheSentTripBelongsTo.JobType == "HLSI")
                    {
                        CONT_NO += planSubTripToSend.jobTrips[1].ContainerNo.Equals(CONT_NO) ? string.Empty : (" " + planSubTripToSend.jobTrips[1].ContainerNo);
                        SEAL_NO += planSubTripToSend.jobTrips[1].SealNo.Equals(SEAL_NO) ? string.Empty : (" " + planSubTripToSend.jobTrips[1].SealNo);
                    }
                    //20170729 for SEA IMPORT end
                }
                //TODO make some switch for Message body to display in MDT
                //for now Union dont want to display job number
                body = string.Format(body,instruction.BookRefNo.Replace("+"," "), /*instruction.HeadNo,*/ VSL, VOY, PRT, LOC_FRM, LOC_TO, AGENT, strQty, remark);
                
                //20170720 for SEA IMPORT
                if (jobTheSentTripBelongsTo.JobType == "HLSI")
                {
                    body = "CONT#:" + CONT_NO + " SEAL#:" + SEAL_NO + " " + body;
                }
                //20170729 for SEA IMPORT end
                
                instruction.MessageBody = body;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message); }
            return instruction;            
        }

        public bool IsTripStillInVehicle()
        {
            bool retValue = false;
            try
            {
                bool isDoubleMount = this.jobTrips.Count > 1;
                string jobNo = this.jobTrips[0].JobNo;
                string seqNo = this.jobTrips[0].Sequence.ToString();

                if (isDoubleMount)
                {
                    seqNo += "+" + this.jobTrips[1].Sequence.ToString();
                    if (!jobNo.Equals(this.jobTrips[1].JobNo))
                    {
                        jobNo += "+" + this.jobTrips[1].JobNo.ToString();                        
                    }
                }
                retValue = TripInstruction02.IsTripStillInVehicle(this.VehicleNumber, jobNo, seqNo, this.Task_ID);
                //foreach (HaulierJobTrip trip in this.jobTrips)
                //{
                //    //HaulierJob job = HaulierJob.GetHaulierJob(trip.JobNo);
                //    //string SourceRefNo = job.SourceReferenceNo == string.Empty ? job.JobNo : job.SourceReferenceNo;
                //    retValue = TripInstruction01.IsTripStillInVehicle(this.VehicleNumber, trip.JobNo, trip.Sequence.ToString());
                //    if (retValue) { return true; }
                //}
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message); }
            return retValue;
        }

        public bool IsTripStillInVehicle(SqlConnection dbCon, SqlTransaction dbTran)
        {
            bool retValue = false;
            try
            {
                bool isDoubleMount = this.jobTrips.Count > 1;
                string jobNo = this.jobTrips[0].JobNo;
                string seqNo = this.jobTrips[0].Sequence.ToString();

                if (isDoubleMount)
                {
                    seqNo += "+" + this.jobTrips[1].Sequence.ToString();
                    if (!jobNo.Equals(this.jobTrips[1].JobNo))
                    {
                        jobNo += "+" + this.jobTrips[1].JobNo.ToString();
                    }
                }
                retValue = TripInstruction02.IsTripStillInVehicle(this.VehicleNumber, jobNo, seqNo,this.Task_ID, dbCon, dbTran);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message); }
            return retValue;
        }

        public static PlanHaulierSubTrip GetNextHaulierPlanSubTripByVehicle(SortableList<PlanHaulierTrip> planHaulierTrips, TripUpdate02 tripUpdate)
        {
            PlanHaulierSubTrip retValue = null;
            try
            {
                Vehicle vehicle = Vehicle.GetVehicle(tripUpdate.PrimeMover);
                PlanHaulierTrip planTrip = PlanHaulierTrip.FindPlanHaulierTripByVehicle(planHaulierTrips, vehicle);
                string jobNo = tripUpdate.HeadNo;
                string seqNo = tripUpdate.SeqNo;
                if (tripUpdate.HeadNo.Contains("+"))
                {
                    string[] jobNos = tripUpdate.HeadNo.Split('+');
                    jobNo = jobNos[0].ToString().Trim();
                }
                if (tripUpdate.SeqNo.Contains("+")) 
                {
                    string[] seqNos = tripUpdate.SeqNo.Split('+');
                    seqNo = seqNos[0].ToString().Trim();
                }

                foreach (PlanHaulierSubTrip planSubTrip in planTrip.HaulierSubTrips)
                {
                    if (planSubTrip.jobTrips[0].JobNo == jobNo && planSubTrip.jobTrips[0].Sequence == Convert.ToInt32(seqNo))
                    {
                        int indxForNextPlanSubTrip = planTrip.FindNextIndexPlanHaulierSubTrip(planSubTrip.End);
                        if (indxForNextPlanSubTrip < 99)
                            retValue = planTrip.HaulierSubTrips[indxForNextPlanSubTrip];

                        break;
                    }
                }                          
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message); }
            return retValue;
        }
        //20150916 - gerry added to get plan subtrip based ug job number and seqno of job trip 
        public static PlanHaulierSubTrip GetHaulierPlanSubTripBaseOnMessage(PlanHaulierTrip planHaulierTrip, TripUpdate02 tripUpdate)
        {
            PlanHaulierSubTrip retValue = null;
            try
            {
                string jobNo = tripUpdate.HeadNo;
                string seqNo = tripUpdate.SeqNo;
                if (tripUpdate.HeadNo.Contains("+"))
                {
                    string[] jobNos = tripUpdate.HeadNo.Split('+');
                    jobNo = jobNos[0].ToString().Trim();
                }
                if (tripUpdate.SeqNo.Contains("+"))
                {
                    string[] seqNos = tripUpdate.SeqNo.Split('+');
                    seqNo = seqNos[0].ToString().Trim();
                }
                foreach (PlanHaulierSubTrip planSubTrip in planHaulierTrip.HaulierSubTrips)
                {
                    if (planSubTrip.jobTrips[0].JobNo == jobNo && planSubTrip.jobTrips[0].Sequence == Convert.ToInt32(seqNo))
                    {
                        retValue = planSubTrip;
                        break;
                    }
                }

                if (retValue == null)
                {
                    throw new FMException(string.Format("Job trip was not sent to this vehicle for this plan date, please open {0} plan date  to confirm the message. "
                                                        ,tripUpdate.MsgDateTime.ToShortDateString()));
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message); }
            return retValue;
        }

        #endregion

        #region TruckComm messaging
        //20170303 - gerry added method to capture the task id used in TruckComm
        public static bool UpdatePlanHaulierSubTripTaskID(PlanHaulierSubTrip planSubTrip)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    PlanTransportDAL.UpdatePlanHaulierSubTripTaskID(planSubTrip, con, tran);
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
                finally { con.Close(); }
            }
            return true;
        }
        #endregion
    }

    public class Incentive
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Rate { get; set; }
        public decimal PayableQty { get; set; }
        public decimal IncentiveAmount { get; set; } 
        public decimal FromPremiumAmount { get; set; }
        public decimal ToPremiumAmount { get; set; }
        public string Remarks { get; set; }

        public Incentive()
        {
            this.Code = string.Empty;
            this.Desc = string.Empty;
            this.Type = string.Empty;
            this.Currency = string.Empty;
            this.EffectiveDate = DateTime.Today;
            this.Rate = 0;
            this.PayableQty = 1;
            this.FromPremiumAmount = 0;
            this.ToPremiumAmount = 0;
            this.IncentiveAmount = 0;
            this.Remarks= string.Empty;        
        }

        //start driver incentives
        public static string GenerateDriverIncentive(string planTripNo, PlanHaulierSubTrip planHaulierSubTrip, string deptCode, string frmName, string userID)
        {
            return PlanTransportDAL.GenerateDriverIncentive(planTripNo, planHaulierSubTrip, deptCode, frmName, userID);
        }
        public static bool GenerateOtherIncentive(string planTripNo, PlanHaulierSubTrip planHaulierSubTrip, Incentive otherIncentive, string deptCode, string frmName, string userID)
        {
            return PlanTransportDAL.GenerateDriverOtherIncentive(planTripNo, planHaulierSubTrip, otherIncentive, deptCode, frmName, userID);
        }
        public static List<string> GetIncentiveCodes(string incentiveType)
        {
            return PlanTransportDAL.GetIncentiveCodes(incentiveType);
        }
        public static Incentive GetActualIncentive(string planTripNo, PlanHaulierSubTrip planSubTrip)
        {
            return PlanTransportDAL.GetActualIncentive(planSubTrip);
        }      
        public static Incentive GetIncentiveWithRateDetails(string incentiveCode, DateTime scheduleDate)
        {
            return PlanTransportDAL.GetIncentiveWithRateDetails(incentiveCode, scheduleDate);
        }
        public static List<Incentive> GetOtherIncentives(PlanHaulierSubTrip planSubTrip)
        {
            return PlanTransportDAL.GetOtherIncentives(planSubTrip);
        }
        public static bool DeleteIncentiveByCode(PlanHaulierSubTrip planSubTrip, Incentive otherIncentive, string frmName, string userID)
        {
            return PlanTransportDAL.DeleteIncentiveByCode(planSubTrip, otherIncentive, frmName, userID);
        }
        public static bool DeleteIncentive(PlanHaulierSubTrip planSubTrip, string frmName, string userID)
        {
            return PlanTransportDAL.DeleteIncentive(planSubTrip, frmName, userID);
        }
        public static void GetPremiumIncentives(PlanHaulierSubTrip planSubTrip, Incentive incentive)
        {
            try
            {
                Incentive premiumIncentive = null;
                if (incentive.FromPremiumAmount > 0)
                {
                    premiumIncentive = new Incentive();
                    premiumIncentive.Code = planSubTrip.StartStop.Code;
                    premiumIncentive.Rate = incentive.FromPremiumAmount;
                    premiumIncentive.PayableQty = 1;
                    premiumIncentive.IncentiveAmount = incentive.FromPremiumAmount;
                    premiumIncentive.Remarks = planSubTrip.StartStop.Code + " premium incentive";
                    planSubTrip.OtherIncentives.Add(premiumIncentive);
                }
                if (incentive.ToPremiumAmount > 0)
                {
                    premiumIncentive = new Incentive();
                    premiumIncentive.Code = planSubTrip.EndStop.Code;
                    premiumIncentive.Rate = incentive.ToPremiumAmount;
                    premiumIncentive.PayableQty = 1;
                    premiumIncentive.IncentiveAmount = incentive.ToPremiumAmount;
                    premiumIncentive.Remarks = planSubTrip.EndStop.Code + " premium incentive";
                    planSubTrip.OtherIncentives.Add(premiumIncentive);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message); }
        }
    }
}

