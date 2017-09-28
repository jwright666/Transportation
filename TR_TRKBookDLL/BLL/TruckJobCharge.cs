using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_HLBookDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using FM.TR_TRKBookDLL.DAL;
using TR_LanguageResource.Resources;
using FM.TR_FMSystemDLL.DAL;
using System.Collections;
using FM.TR_MarketDLL.BLL;

namespace FM.TR_TRKBookDLL.BLL
{
    public class TruckJobCharge : JobCharge
    {
        // 2014-03-20 Zhou Kai adds comments on charges "S" and "T":
        // Charge of type "S" is for service charges those are other than Truck Movements charges, 
        // when a user adds/edits/deletes a charge of type "S", it will be logged.
        // Charge of type "T" is for Truck Movement charges, they're added into system programmatically,
        // when a user adds/edits/deletes a charge of type "T", no logs written
        // 2014-03-20 Zhou Kai ends
        public const string TRUCK_JOB_OTHER_CHARGES = "S"; 
        public const string TRUCK_JOB_MOVE_CHARGES = "T";
        public const string QUOTE_RATE_TYPE_R = "R";
        public const string QUOTE_RATE_TYPE_S = "S";

        public TruckJobCharge()
            : base()
        {
        }

        public TruckJobCharge(int invTrxNo, int invItemSeqNo, int jobId, int sequenceNo, string chargeCode, string chargeDescription, string uom,
            decimal quantity, decimal unitRateFC, decimal totalAmountHC, decimal totalAmountFC, string gstType,
            decimal gstRate, decimal gstAmountHC, string currency, decimal exchangeRate,
            string remarks, string quotationNo, int quotationID, int sequenceNoRate, string jobChargeType,
            DateTime completedDate, JobChargeStatus jobChargeStatus, string rateType,
            byte[] updateVersion)
            : base(invTrxNo, invItemSeqNo, jobId, sequenceNo, chargeCode, chargeDescription, uom,
            quantity, unitRateFC, totalAmountHC, totalAmountFC, gstType,
            gstRate, gstAmountHC, currency, exchangeRate,
            remarks, quotationNo, quotationID, sequenceNoRate, jobChargeType,
            completedDate, jobChargeStatus, rateType,
            updateVersion)
        {
        }



        public static String ValidateTruckJobsWithJobCharges(TruckJob truckJob, List<TruckJobCharge> truckJobCharges,
            SortableList<TruckJobTrip> truckJobTrips)
        {
            String temp = "";
            if (truckJob.TruckJobChargeType == ChargeType.StopDependent)
            {
                for (int i = 0; i < truckJobTrips.Count; i++)
                {
                    bool find = false;
                    for (int j = 0; j < truckJobCharges.Count; j++)
                    {
                        if (truckJobCharges[j].QuotationNo != "")
                        {

                            TransportRate transportRate = new TransportRate();
                            transportRate = TransportRate.GetTransportRate(truckJobCharges[j].QuotationNo, truckJobCharges[j].ChargeCode, truckJobCharges[j].SequenceNoRate);

                            if ((truckJobTrips[i].StartStop.Code == transportRate.StartStop) &&
                                (truckJobTrips[i].EndStop.Code == transportRate.EndStop) &&
                                (truckJobTrips[i].ChargeCode == transportRate.ChargeID) &&
                                (transportRate.IsTruckMovement == true))
                            {
                                if (transportRate.UOM == TruckMovementUOM_WtVol.KGM.ToString() || transportRate.UOM == TruckMovementUOM_WtVol.MT.ToString())
                                {
                                    if (truckJobTrips[i].BookedWeight != 0)
                                        find = true;
                                    else
                                        temp = temp + string.Format(TptResourceBLL.ErrZeroWtVol, CommonResource.Weight, string.Format(TptResourceUI.BookedWeight, string.Empty)) + " " + truckJobTrips[i].Sequence.ToString() + "\n";
                                }
                                if (transportRate.UOM == TruckMovementUOM_WtVol.CBM.ToString())
                                {
                                    if (truckJobTrips[i].BookedVol != 0)
                                        find = true;
                                    else
                                        temp = temp + string.Format(TptResourceBLL.ErrZeroWtVol, CommonResource.Volume, string.Format(TptResourceUI.BookedVol, string.Empty)) + " " + truckJobTrips[i].Sequence.ToString() + "\n";
                                }
                            }
                        }
                    }
                    if (!find)
                    {
                        temp = temp + TptResourceBLL.ErrNoChargeFound + " " + truckJobTrips[i].Sequence.ToString() + "\n";
                    }
                }
            }
            else
            {
                bool find = false;
                for (int j = 0; j < truckJobCharges.Count; j++)
                {
                    if (truckJobCharges[j].QuotationNo != "")
                    {

                        TransportRate transportRate = new TransportRate();
                        transportRate = TransportRate.GetTransportRate(truckJobCharges[j].QuotationNo, truckJobCharges[j].ChargeCode, truckJobCharges[j].SequenceNoRate);

                        if (transportRate.IsTruckMovement)
                        {
                            find = true;
                            if (truckJobCharges[j].Uom == "")
                            {
                                temp = temp + TptResourceBLL.ErrNoUOM + " " + truckJobCharges[j].ChargeCode + "\n";
                            }
                        }
                    }
                }
                if (find == false)
                {
                    temp = temp + TptResourceBLL.ErrNoTMCCharge + "\n";
                }
            }

            for (int i = 0; i < truckJobCharges.Count; i++)
            {
                TransportRate transportRate = new TransportRate();
                transportRate = TransportRate.GetTransportRate(truckJobCharges[i].QuotationNo, truckJobCharges[i].ChargeCode, truckJobCharges[i].SequenceNoRate);

                if (transportRate.IsTruckMovement == true)
                {

                    bool find = false;

                    for (int j = 0; j < truckJobTrips.Count; j++)
                    {
                        if ((truckJobTrips[j].StartStop.Code == transportRate.StartStop) &&
                            (truckJobTrips[j].EndStop.Code == transportRate.EndStop) &&
                            (truckJobTrips[j].ChargeCode == transportRate.ChargeID))
                        {
                            if (transportRate.UOM == TruckMovementUOM_WtVol.KGM.ToString() || transportRate.UOM == TruckMovementUOM_WtVol.MT.ToString())
                            {
                                if (truckJobTrips[i].BookedWeight != 0)
                                    find = true;
                            }

                            else if (transportRate.UOM == TruckMovementUOM_WtVol.CBM.ToString())
                            {
                                if (truckJobTrips[i].BookedVol != 0)
                                    find = true;
                            }
                        }
                    }

                    if (find == false)
                    {
                        temp = temp + TptResourceBLL.ErrNoJobTrip + " " + truckJobCharges[i].ChargeCode + "\n";
                    }
                }
            }
            return temp;


        }

        //Gerry Modify this method
        //Allow to add duplication job charge code and UOM but atleast different Description
        public bool ValidateAddTruckJobCharge(TruckJob truckJob, TruckJobCharge truckJobCharge)
        {
            SortableList<TruckJobCharge> truckJobCharges = new SortableList<TruckJobCharge>();
            truckJobCharges = TruckJobDAL.GetAllTruckJobChargesByType(truckJob, TRUCK_JOB_OTHER_CHARGES);
            bool temp = true;
            //string message = "";
            try
            {
                if (QuotationNo != "")
                {
                    transportRate = TransportRate.GetTransportRate(QuotationNo, ChargeCode, SequenceNoRate);
                    Currency = transportRate.Currency;
                    TotalAmountFC = transportRate.GetTotal(Quantity);
                    UnitRateFC = TotalAmountFC / Quantity;
                }
                else
                {
                    if (truckJobCharges.Count > 0)
                    {
                        for (int i = 0; i < truckJobCharges.Count; i++)
                        {
                            if (this.JobID == truckJobCharges[i].JobID
                                && this.Uom == truckJobCharges[i].Uom
                                && this.ChargeDescription.Trim() == truckJobCharges[i].ChargeDescription.Trim())
                            {
                                temp = false;
                                throw new FMException(CommonResource.ValidateNewJobCharge);
                            }
                        }
                    }
                }
                temp = true;
            }
            catch (FMException fmEx)
            {
                temp = false;
                throw fmEx;
            }
            catch (Exception ex)
            {
                temp = false;
                throw ex;
            }

            return temp;
        }

        // 2013-12-24 Zhou Kai uncomments and modifies it
        public static List<string> GetAllTruckJobChargeSeqNos(string jobNumber)
        {
            return TruckJobDAL.GetAllTruckJobChargeSeqNos(jobNumber);
        }

        public static List<string> GetAllDeletedTruckJobTripIDs(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllDeletedTruckJobTripIDs(dict);
        }

        public static List<string> GetAllDeletedTruckJobChargeSeqNos(Dictionary<string, string> dict)
        {
            return TruckJobDAL.GetAllDeletedTruckJobChargeSeqNos(dict);
        }
        // 2013-12-24

        public static SortableList<TruckJobCharge> GetInvoiceTruckJobCharges(TruckJob truckJob, JobChargeStatus status, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.GetInvoiceTruckJobCharges(truckJob, status, con, tran);
        }

        public static ArrayList GetChargeUOMs(int jobID, string chargeCode)
        {
            return TruckJobDAL.GetChargeUOMs(jobID, chargeCode);
        }
    }
}
