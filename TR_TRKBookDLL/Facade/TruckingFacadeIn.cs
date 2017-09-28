using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TR_AirFreightDLL.BLL;
using TR_AirFreightDLL.Facade;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_MarketDLL.BLL;
using System.Collections;
using TR_LanguageResource.Resources;
using TR_WMSDLL.BLL;
using FM.TR_HLBookDLL.BLL;
using Newtonsoft.Json.Linq;
using FM.TR_SeaFreightDLL.BLL;
using FM.TR_SeaFreightDLL.Facade;
using TR_ConsignmentNoteDLL.BLL;

namespace FM.TR_TRKBookDLL.Facade
{
    public class TruckingFacadeIn
    {  
        //Use this method if you need to separate import from export
        public static AirJob GetAirExportJob(string jobNo, string hawb, string mawb)
        {
            AirJob tempAirExportJob = null;
            try
            {
                tempAirExportJob = AirJobFacadeOut.GetAirJob(jobNo, AirJobType.AirExport, hawb, mawb);
                //tempAirExportJob.AirTruckInfo = GetAirTruckInfo(jobNo, AirJobType.AirExport);
                //tempAirExportJob.airJobRates = GetAirJobRates(tempAirExportJob.JobID, AirJobType.AirExport);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return tempAirExportJob;
        }        
        //Use this method if you need to separate import from export
        public static AirJob GetAirImportJob(string jobNo, string hawb, string mawb)
        {
            AirJob tempAirImportJob = null;
            try
            {
                tempAirImportJob = AirJobFacadeOut.GetAirJob(jobNo, AirJobType.AirImport, hawb, mawb);
                //tempAirImportJob.AirTruckInfo = GetAirTruckInfo(jobNo, AirJobType.AirImport);
                //tempAirImportJob.airJobRates = GetAirJobRates(tempAirImportJob.JobID, AirJobType.AirImport);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return tempAirImportJob;
        }       
        //Or we can use this method for both air import and export jobs  
        public static AirJob GetAirJob(string jobNo, AirJobType jobType, string hawb, string mawb)
        {
            AirJob tempAirJob = null;
            try
            {     
                tempAirJob = AirJobFacadeOut.GetAirJob(jobNo, jobType, hawb, mawb);
                //tempAirJob.AirTruckInfo = GetAirTruckInfo(jobNo, jobType);
                //tempAirJob.airJobRates = GetAirJobRates(tempAirJob.JobID, jobType);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return tempAirJob;
        }    
        public static SortableList<AirTruckInfo> GetAirTruckInfos(string jobNo, AirJobType jobType)
        {
            try
            {
                return AirJobFacadeOut.GetAirTruckInfos(jobNo, jobType);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }  
        //Get the list of air job rates based on jobtype
        public static SortableList<AirJobRate> GetAirJobRates(int jobID, AirJobType jobType)
        {
            SortableList<AirJobRate> tempList = new SortableList<AirJobRate>();
            try
            {
                tempList = AirJobFacadeOut.GetAirJobRates(jobID, jobType);
                
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return tempList;
        }
        //Get the list of air job rates dimension based on jobtype
        public static SortableList<AirJobDtlDimension> GetAirJobDtlDimension(int jobID, int rateSeqNo, AirJobType jobType)
        {
            try
            {
                return AirJobFacadeOut.GetAirJobDtlDimension(jobID, rateSeqNo, jobType);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        //Get list of customers; for Export get all shipper codes, for import get all consignee codes
        public static ArrayList GetCustomerCodes(AirJobType jobType)
        {
            try
            {
                return AirJobFacadeOut.GetCustomerCodes(jobType);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        public static ArrayList GetAllJobNos(AirJobType jobType, string awbNo)
        {
            try
            {
                return AirJobFacadeOut.GetAllJobNos(jobType, awbNo);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }
        public static ArrayList GetAllAWBNos(AirJobType jobType)
        {
            try
            {
                return AirJobFacadeOut.GetAllAWBNos(jobType);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        }          
        public static TruckJob ConvertAirFreightJobToTruckJob(AirJob airJob, AirJobType jobType, string quotationNo, out string rateMsg)
        {
            rateMsg = string.Empty;
            TruckJob tempTruckJob = new TruckJob();
            try
            {
                if (jobType == AirJobType.AirExport && airJob.FlightDate.Hour == 0 && airJob.FlightDate.Minute == 0 && airJob.FlightDate.Second == 0)
                    throw new Exception("Please check the flight time from air freight department. ");
                    //airJob.FlightDate.AddHours(23).AddMinutes(59).AddSeconds(59);          

                tempTruckJob.CustNo = airJob.CustCode;
                tempTruckJob.AWBNo = airJob.AWB;
                tempTruckJob.HAWB = airJob.HAWB;
                tempTruckJob.MAWB = airJob.MAWB;
                tempTruckJob.flightDate = airJob.FlightDate;
                tempTruckJob.flightNo = airJob.FlightNo;
                tempTruckJob.origin = airJob.Origin;
                tempTruckJob.destination = airJob.Destination;
                tempTruckJob.SourceReferenceNo = airJob.JobNo;
                tempTruckJob.shipperCode = airJob.ShipperCode;
                tempTruckJob.shipperName = airJob.ShipperName;
                tempTruckJob.shipperAdd1 = airJob.ShipperAdd1;
                tempTruckJob.shipperAdd2 = airJob.ShipperAdd2;
                tempTruckJob.shipperAdd3 = airJob.ShipperAdd3;
                tempTruckJob.shipperAdd4 = airJob.ShipperAdd4;
                tempTruckJob.consigneeCode = airJob.ConsigneeCode;
                tempTruckJob.consigneeName = airJob.ConsigneeName;
                tempTruckJob.consigneeAdd1 = airJob.ConsigneeAdd1;
                tempTruckJob.consigneeAdd2 = airJob.ConsigneeAdd2;
                tempTruckJob.consigneeAdd3 = airJob.ConsigneeAdd3;
                tempTruckJob.consigneeAdd4 = airJob.ConsigneeAdd4;
                tempTruckJob.JobType = jobType == AirJobType.AirExport ? Job_Type.AE.ToString() : Job_Type.AI.ToString();//20170320
                tempTruckJob.TripType = jobType == AirJobType.AirExport ? Trip_Type.MTO : Trip_Type.OTM;//20170417
                tempTruckJob.QuotationNo = quotationNo;
                tempTruckJob.TptDeptCode = TptDept.GetAllTptDepts(DeptType.Trucking)[0].TptDeptCode.ToString(); //20170320
                
                #region OLD LOGIC
                /*
                foreach (AirJobRate tempAirJobRate in GetAirJobRates(airJob.JobID, jobType))
                {   
                    //Convert airJobRate to truck job trip then add to truck job trip collection
                    tempAirJobRate.airJobDtlDimensions = GetAirJobDtlDimension(tempAirJobRate.JobID, tempAirJobRate.RateSeqNo, jobType);
                    TruckJobTrip jobTrip = ConvertAirJobRateToTruckJobTrip(GetAirTruckInfos(airJob.JobNo, jobType), tempAirJobRate);
                    if (tempAirJobRate.GrossWeight == 0 && tempAirJobRate.airJobDtlDimensions.Count <= 0)
                    {
                        rateMsg += "\n" + string.Format(TptResourceBLL.WarnAirJobRateCBM_MT, airJob.JobNo.ToString(), tempAirJobRate.RateSeqNo.ToString());
                    }
                    else if (jobTrip.StartStop.Code.Equals(string.Empty))                          
                    {
                        rateMsg += "\n" + string.Format(TptResourceBLL.WarnNoAirJobRatePickup, airJob.JobNo.ToString(), tempAirJobRate.RateSeqNo.ToString());
                    }
                    else if (jobTrip.EndStop.Code.Equals(string.Empty))
                    {
                        rateMsg += "\n" + string.Format(TptResourceBLL.WarnNoAirJobRateDelivery, airJob.JobNo.ToString(), tempAirJobRate.RateSeqNo.ToString());
                    }
                    else
                    {
                        tempTruckJob.truckJobTrips.Add(jobTrip);
                    }
                }
                */
                #endregion

                SortableList<AirTruckInfo> airAirTruckJobInfos = GetAirTruckInfos(airJob.JobNo, jobType);
                SortableList<AirJobRate> airTruckJobRates = GetAirJobRates(airJob.JobID, jobType);
                if (airAirTruckJobInfos.Count <= 0)
                    rateMsg = string.Format(TptResourceBLL.WarnNoAirJobPickupAndDelivery, airJob.JobNo);
                else
                {
                    #region OLD logic not cater multiple pickup/delivery
                    //for (int i = 0; i < airAirTruckJobInfos.Count; i++)
                    //{
                    //    bool isFirstAirTruckJobInfo = false;
                    //    if (i == 0)
                    //        isFirstAirTruckJobInfo = true;

                    //    TruckJobTrip tempTruckJobTrip = new TruckJobTrip();
                    //    tempTruckJobTrip = ConvertAirJobRateToTruckJobTrip(tempTruckJob, airAirTruckJobInfos[i], airTruckJobRates, isFirstAirTruckJobInfo);
                    //    tempTruckJob.truckJobTrips.Add(tempTruckJobTrip);
                    //}
                    #endregion
                    //20170421 - for multiple pickup/delivery
                    tempTruckJob.truckJobTrips = ConvertAirJobRatesToTruckJobTrips(tempTruckJob, airAirTruckJobInfos, airTruckJobRates);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
        
            return tempTruckJob;
        }  
        #region 20130502 Removed OLD METHOD ConvertAirJobRateToTruckJobTrip
        /*
        public static TruckJobTrip ConvertAirJobRateToTruckJobTrip(AirTruckInfo airTruckInfo, AirJobRate airJobRate)
        {
            TruckJobTrip tempTruckJobTrip = new TruckJobTrip();
            tempTruckJobTrip.Sequence = airJobRate.RateSeqNo;
            decimal jobTripVol = 0;
            tempTruckJobTrip.BookedWeight = airJobRate.GrossWeight;
            if (airJobRate.airJobDtlDimensions.Count > 0)
            {
                decimal jobTripDetWeight = airJobRate.GrossWeight / airJobRate.airJobDtlDimensions.Count;
                foreach (AirJobDtlDimension tempDimension in airJobRate.airJobDtlDimensions)
                {
                    jobTripVol += (decimal)(tempDimension.Length * tempDimension.Width * tempDimension.Heigth * tempDimension.Qty);
                    tempTruckJobTrip.Sequence = tempDimension.RateSeqNo;
                    tempTruckJobTrip.truckJobTripDetail.Add(FillTruckJobTripDetail(tempDimension, jobTripDetWeight));
                }
                tempTruckJobTrip.billing_UOM = "CBM";
                tempTruckJobTrip.BookedVol = jobTripVol; //Gross cbm taken from the total volumne in TruckTripDetails
            }
            else
            {
                tempTruckJobTrip.billing_UOM = "MT"; //either MT or KGM
                tempTruckJobTrip.BookedVol = 0;
            }

            Stop startStop = new Stop(airTruckInfo.PickupCode,
                                    airTruckInfo.PickupName, 
                                    airTruckInfo.PickupAdd1,
                                    airTruckInfo.PickupAdd2,
                                    airTruckInfo.PickupAdd3,
                                    airTruckInfo.PickupAdd4,
                                    GetStopCity(airTruckInfo.PickupCode.Trim())); //City code is set to blank I'm not sure which field to pull
            Stop endStop = new Stop(airTruckInfo.DeliveryCode,
                                   airTruckInfo.DeliveryName,
                                   airTruckInfo.DeliveryAdd1,
                                   airTruckInfo.DeliveryAdd2,
                                   airTruckInfo.DeliveryAdd3,
                                   airTruckInfo.DeliveryAdd4,
                                   GetStopCity(airTruckInfo.DeliveryCode.Trim()));//City code is set to blank I'm not sure which field to pull 

            tempTruckJobTrip.StartStop = startStop;
            tempTruckJobTrip.StartDate = airTruckInfo.PickupDate.Date;
            string startTime = airTruckInfo.PickupDate.ToShortTimeString().Substring(0, 5).Trim(); //get hh:mm and removed : 
            if (startTime.Length < 5)
                startTime = "0" + startTime;

            tempTruckJobTrip.StartTime = startTime.Replace(":", "");
            tempTruckJobTrip.EndStop = endStop;
            tempTruckJobTrip.EndDate = airTruckInfo.DeliveryDate.Date;
            string endTime = airTruckInfo.DeliveryDate.ToShortTimeString().Substring(0, 5).Trim(); //get hh:mm and removed : 
            if (endTime.Length < 5)
                endTime = "0" + endTime;

            tempTruckJobTrip.EndTime = endTime.Replace(":", "");
            tempTruckJobTrip.CargoDescription = airJobRate.GoodsDescription;

            return tempTruckJobTrip;
        }
        */
        #endregion
        public static TruckJobTrip ConvertAirJobRateToTruckJobTrip(TruckJob truckJob, AirTruckInfo airTruckInfo, SortableList<AirJobRate> airJobRates, bool isFirstAirTruckInfo)
        {
            TruckJobTrip tempTruckJobTrip = new TruckJobTrip();
            tempTruckJobTrip.JobID = truckJob.JobID;
            try
            {
                int SeqNo = 1;
                if (truckJob.truckJobTrips.Count > 0)
                {
                    int index = truckJob.truckJobTrips.Count - 1;
                    SeqNo = truckJob.truckJobTrips[index].Sequence + 1;
                }
                tempTruckJobTrip.Sequence = SeqNo;
                Quotation quotation = null;
                string chargeCode = "TRIP";
                if (!truckJob.QuotationNo.Equals(string.Empty))
                {
                    quotation = Quotation.GetAllQuotationHeader(truckJob.QuotationNo);
                    foreach (TransportRate tempRate in quotation.TransportRates)
                    {
                        if (tempRate.UOM == TruckMovementUOM_WtVol.KGM.ToString())
                        {
                            chargeCode = tempRate.ChargeID;
                            break;
                        }
                    }
                }
                tempTruckJobTrip.ChargeCode = chargeCode;
                #region TruckJobTrip took common properties from airTruckJobInfo
                Stop startStop = new Stop(airTruckInfo.PickupCode,
                                airTruckInfo.PickupName,
                                airTruckInfo.PickupAdd1,
                                airTruckInfo.PickupAdd2,
                                airTruckInfo.PickupAdd3,
                                airTruckInfo.PickupAdd4,
                                GetStopCity(airTruckInfo.PickupCode.Trim())); //City code is set to blank I'm not sure which field to pull
                Stop endStop = new Stop(airTruckInfo.DeliveryCode,
                               airTruckInfo.DeliveryName,
                               airTruckInfo.DeliveryAdd1,
                               airTruckInfo.DeliveryAdd2,
                               airTruckInfo.DeliveryAdd3,
                               airTruckInfo.DeliveryAdd4,
                               GetStopCity(airTruckInfo.DeliveryCode.Trim()));//City code is set to blank I'm not sure which field to pull 

                tempTruckJobTrip.StartStop = startStop;
                tempTruckJobTrip.StartDate = DateTime.Today;
                string startTime = airTruckInfo.PickupDate.ToShortTimeString().Substring(0, 5).Trim(); //get hh:mm and removed : 
                if (startTime.Length < 5)
                    startTime = "0" + startTime;

                tempTruckJobTrip.StartTime = startTime.Replace(":", "");
                tempTruckJobTrip.EndStop = endStop;
                tempTruckJobTrip.EndDate = DateTime.Today;
                string endTime = airTruckInfo.DeliveryDate.ToShortTimeString().Substring(0, 5).Trim(); //get hh:mm and removed : 
                if (endTime.Length < 5)
                    endTime = "0" + endTime;

                tempTruckJobTrip.EndTime = endTime.Replace(":", "");
                #endregion

                tempTruckJobTrip.billing_UOM = TruckMovementUOM_WtVol.KGM.ToString();
                tempTruckJobTrip.BillingQty = 1;
                tempTruckJobTrip.BookedVol = 0;
                tempTruckJobTrip.BookedWeight = Convert.ToDecimal("0.01"); //default not to be 0
                SortableList<AirJobDtlDimension> tempDimensionList = new SortableList<AirJobDtlDimension>();
                if (airJobRates.Count > 0)
                {
                    tempTruckJobTrip.CargoDescription = airJobRates[0].GoodsDescription;
                    if (isFirstAirTruckInfo)
                    {
                        foreach (AirJobRate tempAirJobRate in airJobRates)
                        {
                            tempTruckJobTrip.BookedWeight += tempAirJobRate.GrossWeight;
                            foreach (AirJobDtlDimension tempDimension in tempAirJobRate.airJobDtlDimensions)
                            {
                                tempDimensionList.Add(tempDimension);
                            }
                        }
                        foreach (AirJobDtlDimension tempDimension in tempDimensionList)
                        {
                            TruckJobTripDetail tempTruckJobTripDetail = FillTruckJobTripDetail(tempDimension, tempTruckJobTrip);
                            tempTruckJobTrip.BookedVol += tempTruckJobTripDetail.volume;
                            //tempTruckJobTrip.BillingQty += tempTruckJobTripDetail.quantity;

                            tempTruckJobTrip.truckJobTripDetail.Add(tempTruckJobTripDetail);
                        }
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return tempTruckJobTrip;
        }
        //20170421 - new method to convert from multiple pickup of delivery
        public static SortableList<TruckJobTrip> ConvertAirJobRatesToTruckJobTrips(TruckJob truckJob, SortableList<AirTruckInfo> airTruckInfos, SortableList<AirJobRate> airJobRates)
        {
            SortableList<TruckJobTrip> jobTrips = new SortableList<TruckJobTrip>();
            try
            {
                Quotation quotation = null;
                string chargeCode = "TRIP";
                if (!truckJob.QuotationNo.Equals(string.Empty))
                {
                    quotation = Quotation.GetAllQuotationHeader(truckJob.QuotationNo);
                    foreach (TransportRate tempRate in quotation.TransportRates)
                    {
                        if (tempRate.UOM == TruckMovementUOM_WtVol.KGM.ToString())
                        {
                            chargeCode = tempRate.ChargeID;
                            break;
                        }
                    }
                }
                int SeqNo = 1;
                foreach (AirTruckInfo airTruckInfo in airTruckInfos)
                {
                    TruckJobTrip tempTruckJobTrip = new TruckJobTrip();
                    tempTruckJobTrip.JobID = truckJob.JobID;
                    tempTruckJobTrip.Sequence = SeqNo;
                    tempTruckJobTrip.ChargeCode = chargeCode;
                    tempTruckJobTrip.billing_UOM = "TRIP";
                    tempTruckJobTrip.BillingQty = 1M;
                    //tempTruckJobTrip.timeBound = truckJob.JobType == Job_Type.AE.ToString() ? TIME_BOUND.AM_PICKUP : TIME_BOUND.AM_DELIVERY;
                    #region TruckJobTrip took common properties from airTruckJobInfo
                    Stop startStop = new Stop(airTruckInfo.PickupCode,
                                    airTruckInfo.PickupName,
                                    airTruckInfo.PickupAdd1,
                                    airTruckInfo.PickupAdd2,
                                    airTruckInfo.PickupAdd3,
                                    airTruckInfo.PickupAdd4,
                                    GetStopCity(airTruckInfo.PickupCode.Trim())); //City code is set to blank I'm not sure which field to pull
                    Stop endStop = new Stop(airTruckInfo.DeliveryCode,
                                   airTruckInfo.DeliveryName,
                                   airTruckInfo.DeliveryAdd1,
                                   airTruckInfo.DeliveryAdd2,
                                   airTruckInfo.DeliveryAdd3,
                                   airTruckInfo.DeliveryAdd4,
                                   GetStopCity(airTruckInfo.DeliveryCode.Trim()));//City code is set to blank I'm not sure which field to pull 

                    tempTruckJobTrip.StartStop = startStop;
                    tempTruckJobTrip.StartDate = DateTime.Today;
                    string startTime = airTruckInfo.PickupDate.ToShortTimeString().Substring(0, 5).Trim(); //get hh:mm and removed : 
                    if (startTime.Length < 5)
                        startTime = "0" + startTime;

                    tempTruckJobTrip.StartTime = startTime.Replace(":", "");
                    tempTruckJobTrip.EndStop = endStop;
                    tempTruckJobTrip.EndDate = DateTime.Today;
                    string endTime = airTruckInfo.DeliveryDate.ToShortTimeString().Substring(0, 5).Trim(); //get hh:mm and removed : 
                    if (endTime.Length < 5)
                        endTime = "0" + endTime;

                    tempTruckJobTrip.EndTime = endTime.Replace(":", "");
                    #endregion

                    foreach (AirJobDtlDimension tempDimension in airTruckInfo.airJobDtlDimensions)
                    {
                        tempTruckJobTrip.BookedWeight += tempDimension.GrossWeight;
                        TruckJobTripDetail tempTruckJobTripDetail = FillTruckJobTripDetail(tempDimension, tempTruckJobTrip);
                        tempTruckJobTrip.BookedVol += tempTruckJobTripDetail.volume;
                        tempTruckJobTrip.truckJobTripDetail.Add(tempTruckJobTripDetail);
                    }
                    jobTrips.Add(tempTruckJobTrip);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return jobTrips;
        }

        private static TruckJobTripDetail FillTruckJobTripDetail(AirJobDtlDimension airJobDtlDimension, TruckJobTrip truckJobTrip)
        {
            int detSeqNo = 1;
            if (truckJobTrip.truckJobTripDetail.Count > 0)
            {
                int index = truckJobTrip.truckJobTripDetail.Count - 1;
                detSeqNo = truckJobTrip.truckJobTripDetail[index].detailSequence + 1;
            }
            TruckJobTripDetail tempTruckJobTripDetail = new TruckJobTripDetail();
            tempTruckJobTripDetail.jobTripSequence = truckJobTrip.Sequence; //airJobDtlDimension.RateSeqNo;
            tempTruckJobTripDetail.detailSequence = detSeqNo;//airJobDtlDimension.DimenSeqNo;
            tempTruckJobTripDetail.uom = "PCE"; //default to PIECE(S)
            tempTruckJobTripDetail.quantity = airJobDtlDimension.Qty;
            tempTruckJobTripDetail.length = airJobDtlDimension.Length;
            tempTruckJobTripDetail.width = airJobDtlDimension.Width;
            tempTruckJobTripDetail.height = airJobDtlDimension.Heigth;
            tempTruckJobTripDetail.volume = ((airJobDtlDimension.Length * airJobDtlDimension.Width * airJobDtlDimension.Heigth)  / 1000000)* airJobDtlDimension.Qty;
            //weight details
            tempTruckJobTripDetail.totalWeight = truckJobTrip.BookedWeight;
            tempTruckJobTripDetail.unitWeight = truckJobTrip.BookedWeight / airJobDtlDimension.Qty;
            tempTruckJobTripDetail.balQty = airJobDtlDimension.Qty;
            return tempTruckJobTripDetail;
        }
        #region Removed 20130502
        /*
        public static void CheckPickUpAndDeliveryDetails(AirJob airJob, AirJobType jobType, out string outMsg)
        {
            outMsg = string.Empty;
            SortableList<AirTruckInfo> airTruckJobInfos = GetAirTruckInfos(airJob.JobNo, jobType);
            if (airTruckJobInfos.Count <= 0)
            {
                outMsg = string.Format(TptResourceBLL.WarnNoAirJobPickupAndDelivery, airJob.JobNo);
            }
            else
            {   
                AirTruckInfo airTruckInfo = new AirTruckInfo();
                airTruckInfo = GetAirTruckInfos(airJob.JobNo, jobType);
                if (airTruckInfo != null)
                {
                    if (airTruckInfo.PickupCode.Equals(string.Empty))
                    {
                        outMsg = TptResourceBLL.WarnNoAirJobPickup;
                    }
                    else if (airTruckInfo.DeliveryCode.Equals(string.Empty))
                    {
                        outMsg = TptResourceBLL.WarnNoAirJobDelivery;
                    }
                    else if (airTruckInfo.PickupCode.Equals(string.Empty) || airTruckInfo.DeliveryCode.Equals(string.Empty))
                    {
                        outMsg = TptResourceBLL.WarnNoAirJobPickupAndDelivery;
                    }
                }
                else
                {
                    outMsg = TptResourceBLL.WarnNoAirJobPickupAndDelivery;
                }

                if (!outMsg.Equals(string.Empty))
                {
                    outMsg = string.Format(outMsg, airJob.JobNo);
                }
            }           
        }
          
             */
        #endregion
        private static string GetStopCity(string stopCode)
        {
            try
            {
                return (OperatorDTO.GetOperatorDTO(stopCode)).City.ToString();
            }
            catch{ return "No city found. "; } 
        }

        //20160902 - gerry added to Convert WMS GIN To TruckJob
        public static TruckJob ConvertWMSGINToTruckJob(GINHeader ginHeader)
        {
            TruckJob truckJob = null;
            try
            {
                if (ginHeader != null)
                {
                    truckJob = new TruckJob();
                    truckJob.JobDateTime = ginHeader.issue_date;
                    truckJob.SourceReferenceNo = ginHeader.gin_no;
                    truckJob.BranchCode = ginHeader.branchCode;
                    truckJob.CustNo = ginHeader.custCode;
                    truckJob.JobType = Job_Type.LO.ToString();
                    truckJob.TripType = Trip_Type.OTO;
                    truckJob.warehouseNo = ginHeader.warehouseNo;
                    truckJob.shipperCode = ginHeader.shipperCode;
                    truckJob.shipperName = ginHeader.shipperName;
                    truckJob.shipperAdd1 = ginHeader.shipperAdd1;
                    truckJob.shipperAdd2 = ginHeader.shipperAdd2;
                    truckJob.shipperAdd3 = ginHeader.shipperAdd3;
                    truckJob.shipperAdd4 = ginHeader.shipperAdd4;
                    truckJob.consigneeCode = ginHeader.consigneeCode;
                    truckJob.consigneeName = ginHeader.consigneeName;
                    truckJob.consigneeAdd1 = ginHeader.consigneeAdd1;
                    truckJob.consigneeAdd2 = ginHeader.consigneeAdd2;
                    truckJob.consigneeAdd3 = ginHeader.consigneeAdd3;
                    truckJob.consigneeAdd4 = ginHeader.consigneeAdd4;
                    truckJob.truckJobTrips = new SortableList<TruckJobTrip>();
                    //truck job trip
                    TruckJobTrip truckJobTrip = new TruckJobTrip();
                    foreach (GINDetail ginDetail in ginHeader.ginDetails)
                    {
                        truckJobTrip = new TruckJobTrip();
                        truckJobTrip.legType = Leg_Type.OneLeg;
                        truckJobTrip.IsLaden = true;
                        truckJobTrip.TripStatus = JobTripStatus.Ready; //need flag to autoset to ready status 
                        truckJobTrip.CargoDescription = ginDetail.productName;
                        truckJobTrip.billing_UOM = ginDetail.item_uom;
                        truckJobTrip.BillingQty = 1;
                        truckJobTrip.OwnTransport = true;
                        truckJobTrip.StartDate = DateTime.Now;
                        truckJobTrip.StartTime = DateTime.Now.ToShortTimeString().Replace(":", "");// "0000";
                        truckJobTrip.EndDate = DateTime.Now;
                        truckJobTrip.EndTime = DateTime.Now.AddMinutes(30).ToShortTimeString().Replace(":", "");// "0000";
                        truckJobTrip.StartStop = Stop.GetStop(ginDetail.originCode);//new Stop() { Code = ginDetail.originCode, Description = ginDetail.originName, Address1 = ginDetail.originAdd1, Address2 = ginDetail.originAdd2, Address3 = ginDetail.originAdd3, Address4 = ginDetail.originAdd4 };
                        truckJobTrip.EndStop = Stop.GetStop(ginDetail.destinationCode);
                        truckJobTrip.truckJobTripDetail = new SortableList<TruckJobTripDetail>();
                        //item dimension
                        TruckJobTripDetail dimension = new TruckJobTripDetail();
                        dimension.uom = ginDetail.dimension_uom;
                        dimension.quantity = ginDetail.qty;
                        dimension.totalWeight = ginDetail.totalWeight;
                        dimension.unitWeight = dimension.totalWeight / dimension.quantity;
                        dimension.length = ginDetail.length;
                        dimension.width = ginDetail.width;
                        dimension.height = ginDetail.height;
                        dimension.volume = (dimension.length * dimension.width * dimension.height) / 1000000; //must be in cbm
                        dimension.marking = ginDetail.productCode;
                        dimension.cargoDescription = ginDetail.productName;
                        if (truckJob.truckJobTrips.Count < 1)
                        {
                            //add to dimension
                            truckJobTrip.truckJobTripDetail.Add(dimension);
                            //update weight and volume
                            truckJobTrip.BookedWeight += dimension.totalWeight;
                            truckJobTrip.BookedVol += dimension.volume;
                            //add truckJobTrip to list
                            truckJob.truckJobTrips.Add(truckJobTrip);
                        }
                        else
                        {
                            if ((truckJob.truckJobTrips[truckJob.truckJobTrips.Count - 1]).StartStop.Code == truckJobTrip.StartStop.Code
                                && (truckJob.truckJobTrips[truckJob.truckJobTrips.Count - 1]).EndStop.Code == truckJobTrip.EndStop.Code)
                            {
                                //add to dimension
                                (truckJob.truckJobTrips[truckJob.truckJobTrips.Count - 1]).truckJobTripDetail.Add(dimension);
                                //update weight and volume
                                (truckJob.truckJobTrips[truckJob.truckJobTrips.Count - 1]).BookedWeight += dimension.totalWeight;
                                (truckJob.truckJobTrips[truckJob.truckJobTrips.Count - 1]).BookedVol += dimension.volume;
                            }
                        }
                    }
                    if (truckJob.truckJobTrips.Count > 1)
                        truckJob.TripType = Trip_Type.OTM;
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return truckJob;
        }

        //20170526
        #region Consignment Note
        public static List<String> GetConsignmentNoteToJobNos(JArray jConsignmentNoteNumArr)
        {
            List<String> consignmentNoteNos = new List<String>();
            try
            {
                //TransportSettings transportSettings = TransportSettings.GetTransportSetting();
                for (int i = 1; i < jConsignmentNoteNumArr.Count; i++)
                {
                    JObject jObj = JObject.Parse(jConsignmentNoteNumArr[i].ToString());
                    string consignmentNo = jObj.GetValue("Consignment_Note_No").ToString();
                    consignmentNoteNos.Add(consignmentNo);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return consignmentNoteNos;
        }
        public static List<String> GetCustomerCodes(JArray jConsignmentNoteNumArr)
        {
            List<String> customercodes = new List<String>();
            try
            {
                //TransportSettings transportSettings = TransportSettings.GetTransportSetting();
                for (int i = 1; i < jConsignmentNoteNumArr.Count; i++)
                {
                    JObject jObj = JObject.Parse(jConsignmentNoteNumArr[i].ToString());
                    string consignmentNo = jObj.GetValue("Party_Code").ToString();
                    if (!customercodes.Contains(consignmentNo) && consignmentNo != "")
                        customercodes.Add(consignmentNo);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return customercodes;
        }
        public static List<TruckJob> ConvertConsignmentNoteToTruckJobs(JArray jConsignmentNoteArr)
        {
            List<TruckJob> jobsFromConsolidatedConsignmentNote = new List<TruckJob>();
            try
            {
                TransportSettings transportSettings = TransportSettings.GetTransportSetting();
                for (int i = 2; i < jConsignmentNoteArr.Count; i++)
                { 
                    JObject jObj = JObject.Parse(jConsignmentNoteArr[i].ToString());

                    TruckJob truckJob = new TruckJob();
                    truckJob.JobDateTime = DateTime.Today;
                    truckJob.SourceReferenceNo = jObj.GetValue("Consignment_Note_No").ToString();
                    truckJob.BranchCode = jObj.GetValue("Consignment_Note_No").ToString();

                    JObject jCustomerObj = JObject.Parse(jObj.GetValue("Customer_Info").ToString());
                    truckJob.CustNo = jObj.GetValue("Party_Code").ToString();

                    truckJob.JobType = Job_Type.LO.ToString();
                    truckJob.TripType = Trip_Type.OTM;
                    truckJob.BillByTransport = false;
                    truckJob.truckJobTrips = new SortableList<TruckJobTrip>();
                    //items
                    JArray jDetailArr = JArray.Parse(jObj.GetValue("Goods_Details").ToString());
                    for (int j = 0; j < jDetailArr.Count; j++)
                    {
                        JObject jDetailObj = JObject.Parse(jDetailArr[j].ToString());

                        TruckJobTrip truckJobTrip = new TruckJobTrip();
                        truckJobTrip.IsLaden = true;
                        truckJobTrip.TripStatus = JobTripStatus.Ready; //need flag to autoset to ready status 
                        truckJobTrip.CargoDescription = jDetailObj.GetValue("Description").ToString();
                        truckJobTrip.billing_UOM = jDetailObj.GetValue("UOM").ToString();
                        truckJobTrip.BillingQty = 1;
                        truckJobTrip.OwnTransport = true;
                        truckJobTrip.StartDate = DateTime.Now;
                        truckJobTrip.StartTime = DateTime.Now.ToShortTimeString().Replace(":", "");// "0000";
                        truckJobTrip.EndDate = DateTime.Now;
                        truckJobTrip.EndTime = DateTime.Now.AddMinutes(30).ToShortTimeString().Replace(":", "");// "0000";
                        truckJobTrip.StartStop = Stop.GetStop(transportSettings.DefaultWarehouseCode);//new Stop() { Code = ginDetail.originCode, Description = ginDetail.originName, Address1 = ginDetail.originAdd1, Address2 = ginDetail.originAdd2, Address3 = ginDetail.originAdd3, Address4 = ginDetail.originAdd4 };
                        truckJobTrip.EndStop = Stop.GetStop(truckJob.CustNo);
                        truckJobTrip.truckJobTripDetail = new SortableList<TruckJobTripDetail>();                        
                        //items dimensions
                        JArray jDetailDimensionArr = JArray.Parse(jDetailObj.GetValue("Goods_Dimensions").ToString());
                        for (int k = 0; k < jDetailDimensionArr.Count; k++)
                        {
                            JObject jDetailDimensionObj = JObject.Parse(jDetailDimensionArr[k].ToString());

                            TruckJobTripDetail tripDimension = new TruckJobTripDetail();
                            tripDimension.uom = jDetailObj.GetValue("UOM").ToString();
                            tripDimension.quantity = Convert.ToInt32(jDetailDimensionObj.GetValue("QTY").ToString());
                            tripDimension.unitWeight = Convert.ToInt32(jDetailDimensionObj.GetValue("Gross_Weight").ToString());
                            tripDimension.totalWeight = tripDimension.quantity * tripDimension.unitWeight;
                            tripDimension.length = Convert.ToInt32(jDetailDimensionObj.GetValue("Length").ToString());
                            tripDimension.width = Convert.ToInt32(jDetailDimensionObj.GetValue("Width").ToString());
                            tripDimension.height = Convert.ToInt32(jDetailDimensionObj.GetValue("Height").ToString());
                            tripDimension.volume = (tripDimension.length * tripDimension.width * tripDimension.height) / 1000000; //cbm
                            tripDimension.marking = jDetailDimensionObj.GetValue("Goods_Desc").ToString();
                            tripDimension.cargoDescription = jDetailDimensionObj.GetValue("Cargo_Desc").ToString();
                            tripDimension.remarks = jDetailDimensionObj.GetValue("Goods_Desc").ToString();

                            truckJobTrip.BookedVol += tripDimension.volume;
                            truckJobTrip.BookedWeight += tripDimension.totalWeight;
                            truckJobTrip.truckJobTripDetail.Add(tripDimension);
                        }
                        truckJob.truckJobTrips.Add(truckJobTrip);
                    }
                    jobsFromConsolidatedConsignmentNote.Add(truckJob);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return jobsFromConsolidatedConsignmentNote;
        }
        public static List<ConsignmentNote_Job_Master> GetConsginmentNoteJobMaster(JArray jConsignmentNoteArr)
        {
            List<ConsignmentNote_Job_Master> jobsFromConsolidatedConsignmentNote = new List<ConsignmentNote_Job_Master>();
            try
            {
                TransportSettings transportSettings = TransportSettings.GetTransportSetting();
                Stop pickupPoint = Stop.GetStop(transportSettings.DefaultWarehouseCode);
                for (int i = 2; i < jConsignmentNoteArr.Count; i++)
                {
                    //master
                    ConsignmentNote_Job_Master consignmentNote = new ConsignmentNote_Job_Master();
                    JObject jObjMaster = JObject.Parse(jConsignmentNoteArr[0].ToString());
                    consignmentNote.Consignment_Note_No = jObjMaster.GetValue("Consignment_Note_No").ToString();
                    consignmentNote.Customer_Code = jObjMaster.GetValue("CustomerCode").ToString();
                    consignmentNote.Job_Goods_Details = new SortableList<ConsignmentNote_Job_Goods_Details>();




                    //items
                    JObject jObjDetails = JObject.Parse(jConsignmentNoteArr[i].ToString());
                    ConsignmentNote_Job_Goods_Details good_Details = new ConsignmentNote_Job_Goods_Details();
                    good_Details.Goods_Desc = jObjDetails.GetValue("Goods_Desc").ToString();
                    good_Details.UOM = "PCS";
                    good_Details.Qty = Convert.ToDecimal(jObjDetails.GetValue("Scan_PCS").ToString());
                    good_Details.OriginCode = pickupPoint.Code;
                    good_Details.OriginName = pickupPoint.Description;
                    good_Details.OriginAdd1 = pickupPoint.Address1;
                    good_Details.OriginAdd2 = pickupPoint.Address2;
                    good_Details.OriginAdd3 = pickupPoint.Address3;
                    good_Details.OriginAdd4 = pickupPoint.Address4;
                    good_Details.DestinationCode = consignmentNote.Customer_Code;
                    



                    
                    JArray jDetailArr = JArray.Parse(jObjDetails.GetValue("Goods_Details").ToString());
                    for (int j = 0; j < jDetailArr.Count; j++)
                    {
                        JObject jDetailObj = JObject.Parse(jDetailArr[j].ToString());

                        good_Details.Job_Goods_Dimensions = new SortableList<ConsignmentNote_Job_Goods_Dimension>();
                        //items dimensions
                        JArray jDetailDimensionArr = JArray.Parse(jDetailObj.GetValue("Goods_Dimensions").ToString());
                        JObject jDetailDimensionObj = JObject.Parse(jDetailDimensionArr[0].ToString());
                        good_Details.UnitWeight = Convert.ToDecimal(jDetailDimensionObj.GetValue("Gross_Weight").ToString());
                        good_Details.Length = Convert.ToDecimal(jDetailDimensionObj.GetValue("Length").ToString());
                        good_Details.Width = Convert.ToDecimal(jDetailDimensionObj.GetValue("Width").ToString());
                        good_Details.Height = Convert.ToDecimal(jDetailDimensionObj.GetValue("Height").ToString());

                        good_Details.TotalWeight = good_Details.Qty * good_Details.UnitWeight;
                        good_Details.TotalVolume = (good_Details.Qty * good_Details.Length * good_Details.Width * good_Details.Height) / 1000000;                        
                    }
                    jobsFromConsolidatedConsignmentNote.Add(consignmentNote);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return jobsFromConsolidatedConsignmentNote;
        }
        public static TruckJob ConvertConsignmentNoteToTruckJob(JObject jConsignmentNote, User user, out ArrayList consignmentJob_Ids)
        {
            TruckJob truckJob = new TruckJob();
            consignmentJob_Ids = new ArrayList();
            try
            {
                TransportSettings transportSettings = TransportSettings.GetTransportSetting();
                //get consignment note array
                JArray jArr = JArray.Parse(jConsignmentNote.GetValue("Job_Masters").ToString());
                JObject jObj = JObject.Parse(jArr[0].ToString());
                truckJob.JobDateTime = DateTime.Today;
                JObject jCustomerObj = JObject.Parse(jObj.GetValue("Customer_Info").ToString());
                string opCode = jCustomerObj.GetValue("Party_Code").ToString();
                OperationDetail op = OperationDetail.GetOperationDetail(opCode);
                truckJob.CustNo = op.customerCode;
                truckJob.consigneeCode = opCode;
                truckJob.consigneeName = op.operationName;
                truckJob.consigneeAdd1 = op.opAdd1;
                truckJob.consigneeAdd2 = op.opAdd2;
                truckJob.consigneeAdd3 = op.opAdd3;
                truckJob.consigneeAdd4 = op.opAdd4;

                truckJob.SourceReferenceNo = jCustomerObj.GetValue("Party_Code").ToString();
                truckJob.BranchCode = user.DefaultBranch.Code.ToString();
                truckJob.JobType = Job_Type.LO.ToString();
                truckJob.TripType = Trip_Type.OTO;
                truckJob.BillByTransport = false;

                truckJob.TptDeptCode = TptDept.GetAllTptDepts(DeptType.Trucking)[0].TptDeptCode.ToString();
                truckJob.truckJobTrips = new SortableList<TruckJobTrip>();
                //truck job trip
                TruckJobTrip truckJobTrip = new TruckJobTrip();
                truckJobTrip.Sequence = 1;
                truckJobTrip.IsLaden = true;
                truckJobTrip.TripStatus = JobTripStatus.Ready; //need flag to autoset to ready status 
                truckJobTrip.BillingQty = 1;
                truckJobTrip.OwnTransport = true;
                truckJobTrip.StartDate = DateTime.Now;
                truckJobTrip.StartTime = DateTime.Now.ToShortTimeString().Substring(0, 5).Replace(":", "");// "0000";
                truckJobTrip.EndDate = DateTime.Now;
                truckJobTrip.EndTime = DateTime.Now.AddMinutes(30).ToShortTimeString().Substring(0, 5).Replace(":", "");// "0000";
                truckJobTrip.StartStop = Stop.GetStop(transportSettings.DefaultWarehouseCode);//new Stop() { Code = ginDetail.originCode, Description = ginDetail.originName, Address1 = ginDetail.originAdd1, Address2 = ginDetail.originAdd2, Address3 = ginDetail.originAdd3, Address4 = ginDetail.originAdd4 };
                truckJobTrip.EndStop = Stop.GetStop(opCode);
                truckJobTrip.billing_UOM = "TRIP";
                truckJobTrip.truckJobTripDetail = new SortableList<TruckJobTripDetail>();
                consignmentJob_Ids = new ArrayList();
                for (int i = 0; i < jArr.Count; i++)
                {
                    JObject jObjJobMaster = JObject.Parse(jArr[i].ToString());
                    string id = jObjJobMaster.GetValue("Job_ID").ToString();
                    if (!consignmentJob_Ids.Contains(id))
                        consignmentJob_Ids.Add(id);
                    //items
                    JArray jDetailArr = JArray.Parse(jObjJobMaster.GetValue("Goods_Details").ToString());

                    for (int j = 0; j < jDetailArr.Count; j++)
                    {
                        JObject jDetailObj = JObject.Parse(jDetailArr[j].ToString());

                        TruckJobTripDetail tripDimension = new TruckJobTripDetail();
                        tripDimension.jobTripSequence = truckJobTrip.Sequence;
                        tripDimension.detailSequence = j + 1;
                        tripDimension.uom = jDetailObj.GetValue("Packing_Type").ToString();
                        tripDimension.quantity = Convert.ToInt32(jDetailObj.GetValue("Qty").ToString());
                        tripDimension.marking = jDetailObj.GetValue("Goods_Desc").ToString();
                        tripDimension.cargoDescription = jDetailObj.GetValue("Cargo_Desc").ToString();
                        tripDimension.remarks = jDetailObj.GetValue("Goods_Desc").ToString();
                        tripDimension.ref_No = jObjJobMaster.GetValue("Consignment_Note_No").ToString();
                        tripDimension.unitWeight = Convert.ToInt32(jDetailObj.GetValue("Gross_Weight").ToString());
                        //items dimensions
                        JArray jDetailDimensionArr = JArray.Parse(jDetailObj.GetValue("Goods_Dimensions").ToString());
                        for (int k = 0; k < jDetailDimensionArr.Count; k++)
                        {
                            JObject jDetailDimensionObj = JObject.Parse(jDetailDimensionArr[k].ToString());
                            tripDimension.length = Convert.ToInt32(jDetailDimensionObj.GetValue("Length").ToString());
                            tripDimension.width = Convert.ToInt32(jDetailDimensionObj.GetValue("Width").ToString());
                            tripDimension.height = Convert.ToInt32(jDetailDimensionObj.GetValue("Height").ToString());
                            tripDimension.volume = tripDimension.quantity * ((tripDimension.length * tripDimension.width * tripDimension.height) / 1000000); //cbm
                            tripDimension.totalWeight = tripDimension.quantity * tripDimension.unitWeight;
                        }

                        truckJobTrip.BookedVol += tripDimension.volume;
                        truckJobTrip.BookedWeight += tripDimension.totalWeight;
                        truckJobTrip.truckJobTripDetail.Add(tripDimension);
                    }
                }
                truckJob.truckJobTrips.Add(truckJobTrip);
                truckJob.ValidateAddTruckJobHeader();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return truckJob;
        }

        public static List<TruckJob> ConvertCosolidatedConsignmentNoteToTruckJobs(JArray jConsignmentNoteArr, User user)
        {
            List<TruckJob> jobsFromConsolidatedConsignmentNotes = new List<TruckJob>();
            List<string> unRegisteredCustomers = new List<string>();
            try
            {
                TransportSettings transportSettings = TransportSettings.GetTransportSetting();
                OperationDetail shipper = OperationDetail.GetOperationDetail(transportSettings.DefaultWarehouseCode);
                for (int i = 0; i < jConsignmentNoteArr.Count; i++)
                {
                    JObject jObj = JObject.Parse(jConsignmentNoteArr[i].ToString());
                    //master                    
                    string consigneeCode = jObj.GetValue("CustomerCode").ToString();
                    if (consigneeCode == "")
                        consigneeCode = jObj.GetValue("Consignment_Ref").ToString();
                    //op = OperationDetail.GetOperationDetailBySpecialRef(opCode);                    
                    OperationDetail consignee = OperationDetail.GetOperationDetail(consigneeCode.ToUpper());
                    if (consignee.operationCode == string.Empty && !unRegisteredCustomers.Contains(consigneeCode)) 
                        unRegisteredCustomers.Add(consigneeCode); //list of un-registered customers
                    TruckJob truckJob = ConvertJSONObjectTruckJob(jObj, shipper, consignee, user);
                    //items 
                    TruckJobTrip truckJobTrip = ConvertJSONObjectToTruckJobTrip(jObj, shipper.operationCode, consigneeCode.ToUpper());
                    //dimension
                    TruckJobTripDetail tripDimension = ConvertJSONObjectToTruckJobTripDetail(jObj, 1);
                    bool jobExist = false;
                    bool detailExist = false;
                    bool dimensionExist = false;
                    foreach (TruckJob tempTruckJob in jobsFromConsolidatedConsignmentNotes)
                    {
                        if (tempTruckJob.SourceReferenceNo == truckJob.SourceReferenceNo)
                        {
                            jobExist = true;
                            foreach (TruckJobTrip trip in tempTruckJob.truckJobTrips)
                            {
                                if (trip.JobID == truckJobTrip.JobID)
                                {
                                    detailExist = true;
                                    tripDimension.detailSequence = trip.truckJobTripDetail.Count + 1;
                                    trip.BookedVol += tripDimension.volume;
                                    trip.BookedWeight += tripDimension.totalWeight;
                                    trip.truckJobTripDetail.Add(tripDimension);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (!jobExist && !detailExist)
                    {
                        truckJobTrip.BookedVol += tripDimension.volume;
                        truckJobTrip.BookedWeight += tripDimension.totalWeight;
                        truckJobTrip.truckJobTripDetail.Add(tripDimension);
                        truckJob.truckJobTrips.Add(truckJobTrip);
                    }
                    if (!jobExist) { jobsFromConsolidatedConsignmentNotes.Add(truckJob); }                
                }
                if(unRegisteredCustomers.Count > 0)
                    throw new FMException("Customer(s) " + String.Join(", ", unRegisteredCustomers.ToArray()) + " was not found. Please register customer(s) first."); 
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return jobsFromConsolidatedConsignmentNotes;
        }
        private static TruckJob ConvertJSONObjectTruckJob(JObject jObj,OperationDetail shipper, OperationDetail consignee, User user)
        {
            TruckJob truckJob = new TruckJob();
            truckJob.JobID = Convert.ToInt32(jObj.GetValue("Job_ID").ToString());
            truckJob.JobDateTime = DateTime.Today;
            truckJob.CustNo = consignee.customerCode;
            truckJob.shipperCode = shipper.operationCode;
            truckJob.shipperName = shipper.operationCode;
            truckJob.shipperAdd1 = shipper.opAdd1;
            truckJob.shipperAdd2 = shipper.opAdd2;
            truckJob.shipperAdd3 = shipper.opAdd3;
            truckJob.shipperAdd4 = shipper.opAdd4;
            truckJob.consigneeCode = consignee.operationCode;
            truckJob.consigneeName = consignee.operationName;
            truckJob.consigneeAdd1 = consignee.opAdd1;
            truckJob.consigneeAdd2 = consignee.opAdd2;
            truckJob.consigneeAdd3 = consignee.opAdd3;
            truckJob.consigneeAdd4 = consignee.opAdd4;
            truckJob.SourceReferenceNo = jObj.GetValue("Trx_ID").ToString() +"_" +jObj.GetValue("Job_ID").ToString() + "_" + jObj.GetValue("Consignment_Note_No").ToString();
            truckJob.BranchCode = user.DefaultBranch.Code.ToString();
            truckJob.JobType = Job_Type.LO.ToString();
            truckJob.TripType = Trip_Type.OTO;
            truckJob.BillByTransport = false;
            truckJob.TptDeptCode = TptDept.GetAllTptDepts(DeptType.Trucking)[0].TptDeptCode.ToString();
            truckJob.truckJobTrips = new SortableList<TruckJobTrip>();

            return truckJob;
        }
        private static TruckJobTrip ConvertJSONObjectToTruckJobTrip(JObject jObj, string shipperCode, string consigneeCode)
        {
            TruckJobTrip truckJobTrip = new TruckJobTrip();
            truckJobTrip.JobID = Convert.ToInt32(jObj.GetValue("Job_ID").ToString());
            truckJobTrip.Sequence = Convert.ToInt32(jObj.GetValue("Job_Seq_No").ToString());
            truckJobTrip.IsLaden = true;
            truckJobTrip.TripStatus = JobTripStatus.Ready; //need flag to autoset to ready status 
            truckJobTrip.BillingQty = 1;
            truckJobTrip.OwnTransport = true;
            truckJobTrip.StartDate = DateTime.Now;
            truckJobTrip.StartTime = DateTime.Now.ToShortTimeString().Substring(0, 5).Replace(":", "");// "0000";
            truckJobTrip.EndDate = DateTime.Now;
            truckJobTrip.EndTime = DateTime.Now.AddMinutes(30).ToShortTimeString().Substring(0, 5).Replace(":", "");// "0000";
            truckJobTrip.StartStop = Stop.GetStop(shipperCode);//new Stop() { Code = ginDetail.originCode, Description = ginDetail.originName, Address1 = ginDetail.originAdd1, Address2 = ginDetail.originAdd2, Address3 = ginDetail.originAdd3, Address4 = ginDetail.originAdd4 };
            truckJobTrip.EndStop = Stop.GetStop(consigneeCode);
            truckJobTrip.billing_UOM = "TRIP";
            truckJobTrip.truckJobTripDetail = new SortableList<TruckJobTripDetail>();
            return truckJobTrip;
        }
        private static TruckJobTripDetail ConvertJSONObjectToTruckJobTripDetail(JObject jObj, int detailSequence)
        {
            TruckJobTripDetail tripDimension = new TruckJobTripDetail();
            tripDimension.jobTripSequence = Convert.ToInt32(jObj.GetValue("Job_Seq_No").ToString());
            tripDimension.detailSequence = detailSequence;
            tripDimension.uom = "PCS";
            tripDimension.quantity = Convert.ToInt32(jObj.GetValue("Scan_PCS").ToString());//take the account scanned qty
            tripDimension.marking = jObj.GetValue("Consignment_Ref").ToString();
            tripDimension.cargoDescription = jObj.GetValue("Goods_Desc").ToString();
            tripDimension.remarks = "Pulled from consolidation";
            tripDimension.ref_No = jObj.GetValue("Consignment_Note_No").ToString();
            tripDimension.unitWeight = Convert.ToDecimal(jObj.GetValue("Gross_Weight").ToString());
            tripDimension.length = Convert.ToDecimal(jObj.GetValue("Length").ToString());
            tripDimension.width = Convert.ToDecimal(jObj.GetValue("Width").ToString());
            tripDimension.height = Convert.ToDecimal(jObj.GetValue("Height").ToString());
            tripDimension.volume = tripDimension.quantity * ((tripDimension.length * tripDimension.width * tripDimension.height) / 1000000); //cbm
            tripDimension.totalWeight = tripDimension.quantity * tripDimension.unitWeight;
            return tripDimension;
        }
        #endregion

        #region Sea Freight
        public static List<string> GetAllSeaFreightJobNos(string transporterCode, Job_Type jobType)
        {
            List<string> jobNos = new List<string>();
            try
            {
                if (jobType == Job_Type.SE)
                    jobNos = SeaFreightFacadeOut.GetOutstandingSeaExportJobNos(transporterCode, "LCL");
                if (jobType == Job_Type.SI)
                    jobNos = SeaFreightFacadeOut.GetOutstandingSeaImportJobNos(transporterCode, "LCL");
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }

            return jobNos;
        }
        public static SeaExportJob GetSeaExportJob(string jobNo)
        {
            return SeaFreightFacadeOut.GetSeaExportJob(jobNo);
        }
        public static SeaImportJob GetSeaImportJob(string jobNo)
        {
            return SeaFreightFacadeOut.GetSeaImportJob(jobNo);
        }
        public static List<JobDimension> GetExportDimension(int export_number, int seqNo)
        {
            return SeaFreightFacadeOut.GetExportDimension(export_number, seqNo);
        }
        public static List<JobDimension> GetImportDimension(int export_number, int seqNo)
        {
            return SeaFreightFacadeOut.GetImportDimension(export_number, seqNo);
        }
        public static TruckJob ConvertSeaDataToTruckJob(SeaJob seaJob, string quotationNo, Job_Type jobType)
        {
            TruckJob tempTruckJob = new TruckJob();
            tempTruckJob.CustNo = seaJob.CustomerCode;
            tempTruckJob.oblNo = seaJob.OblNumber;
            tempTruckJob.hblNo = seaJob.HblNumber;
            tempTruckJob.eta = seaJob.Eta;
            tempTruckJob.etd = seaJob.Etd;
            tempTruckJob.vesselNo = seaJob.MVessel;
            tempTruckJob.voyageNo = seaJob.MVoyage;
            tempTruckJob.pod = seaJob.Pod;
            tempTruckJob.pol = seaJob.Pol;
            tempTruckJob.SourceReferenceNo = seaJob.JobNumber;
            tempTruckJob.shipperCode = seaJob.ShipperCode;
            tempTruckJob.shipperName = seaJob.ShipperName;
            tempTruckJob.shipperAdd1 = seaJob.ShipperAdd1;
            tempTruckJob.shipperAdd2 = seaJob.ShipperAdd2;
            tempTruckJob.shipperAdd3 = seaJob.ShipperAdd3;
            tempTruckJob.shipperAdd4 = seaJob.ShipperAdd4;
            tempTruckJob.consigneeCode = seaJob.ConsigneeCode;
            tempTruckJob.consigneeName = seaJob.ConsigneeName;
            tempTruckJob.consigneeAdd1 = seaJob.ConsigneeAdd1;
            tempTruckJob.consigneeAdd2 = seaJob.ConsigneeAdd2;
            tempTruckJob.consigneeAdd3 = seaJob.ConsigneeAdd3;
            tempTruckJob.consigneeAdd4 = seaJob.ConsigneeAdd4;
            tempTruckJob.JobType = jobType.ToString();
            tempTruckJob.TripType = Trip_Type.OTO;
            tempTruckJob.QuotationNo = quotationNo;
            tempTruckJob.TptDeptCode = TptDept.GetAllTptDepts(DeptType.Trucking)[0].TptDeptCode.ToString();
            //job trip
            tempTruckJob.truckJobTrips = ConvertSeaImportContainerDataToTruckJobTrip(seaJob, tempTruckJob, jobType);


            return tempTruckJob;
        }

        public static SortableList<TruckJobTrip> ConvertSeaImportContainerDataToTruckJobTrip(SeaJob seaJob, TruckJob truckJob, Job_Type jobType)
        {
            SortableList<TruckJobTrip> truckJobTrips = new SortableList<TruckJobTrip>();
            foreach (SeaJobContainer container in seaJob.SeaJobContainers)
            {
                if (container.GrossWeight <= 0) { throw new FMException("Gross weight should be greater than 0. "); }
                OperationDetail operationDetail = new OperationDetail();
                operationDetail = OperationDetail.GetOperationDetail(seaJob.ConsigneeCode);
                CustomerDTO customerDTO = new CustomerDTO();
                if (operationDetail == null || operationDetail.customerCode == "")
                    customerDTO = CustomerDTO.GetCustomerByCustCode("CASH");
                else
                    customerDTO = CustomerDTO.GetCustomerByCustCode(operationDetail.customerCode);
                
                TruckJobTrip jobTrip = new TruckJobTrip();
                #region Common Trip properties
                jobTrip.JobID = truckJob.JobID;
                jobTrip.Sequence += 1;
                jobTrip.StartStop.Code = seaJob.ShipperCode;
                jobTrip.StartStop.Description = seaJob.ShipperName;
                jobTrip.StartStop.Address1 = seaJob.ShipperAdd1;
                jobTrip.StartStop.Address2 = seaJob.ShipperAdd2;
                jobTrip.StartStop.Address3 = seaJob.ShipperAdd3;
                jobTrip.StartStop.Address4 = seaJob.ShipperAdd4;
                jobTrip.EndStop.Code = seaJob.StuffingCode;
                jobTrip.EndStop.Description = seaJob.StuffingName;
                jobTrip.EndStop.Address1 = seaJob.StuffingAdd1;
                jobTrip.EndStop.Address2 = seaJob.StuffingAdd2;
                jobTrip.EndStop.Address3 = seaJob.StuffingAdd3;
                jobTrip.EndStop.Address4 = seaJob.StuffingAdd4;
                jobTrip.CustomerCode = customerDTO.Code;
                jobTrip.CustomerName = customerDTO.Name;
                jobTrip.StartDate = truckJob.eta;
                jobTrip.EndDate = truckJob.eta;
                jobTrip.StartTime = "0000";
                jobTrip.EndTime = "0000";
                jobTrip.TripStatus = JobTripStatus.Booked;
                jobTrip.IsNew = true;
                jobTrip.OwnTransport = true;
                jobTrip.subCon = new OperatorDTO();//new CustomerDTO();
                jobTrip.SubContractorReference = "";
                jobTrip.ChargeCode = "";
                jobTrip.Remarks = "";
                jobTrip.IsLaden = true;
                jobTrip.BookedWeight = container.GrossWeight;
                //jobTrip.BookedVol = container.GrossCBM;
                jobTrip.billing_UOM = "TRIP";
                jobTrip.BillingQty = 1;
                jobTrip.CargoDescription = container.CargoDescription;
                #endregion
                jobTrip.truckJobTripDetail = new SortableList<TruckJobTripDetail>();
                //dimension
                List<JobDimension> jobDimensions = GetExportDimension(container.JobID, container.SeqNo);
                if(jobType== Job_Type.SE)
                    jobDimensions = GetExportDimension(container.JobID, container.SeqNo);
                else if (jobType == Job_Type.SI)
                    jobDimensions = GetImportDimension(container.JobID, container.SeqNo);
                
                if (jobDimensions.Count > 0)
                {
                    foreach (JobDimension tempDimension in jobDimensions)
                    { 
                        TruckJobTripDetail dimension = new TruckJobTripDetail();
                        dimension.jobID = jobTrip.JobID;
                        dimension.jobTripSequence = jobTrip.Sequence;
                        dimension.uom = "PCS";
                        dimension.quantity = tempDimension.Qty;
                        dimension.balQty = tempDimension.Qty;
                        dimension.length = tempDimension.Length;
                        dimension.width = tempDimension.Width;
                        dimension.height = tempDimension.Height;
                        dimension.marking = container.ContainerNumber;
                        dimension.cargoDescription = container.CargoDescription;
                        dimension.totalWeight = container.GrossWeight / jobDimensions.Count;
                        dimension.unitWeight = dimension.totalWeight / dimension.quantity;
                        dimension.volume = (tempDimension.Length * tempDimension.Width * tempDimension.Height * tempDimension.Qty) / 1000000; //cbm
                        
                        jobTrip.truckJobTripDetail.Add(dimension);
                        jobTrip.BookedVol += dimension.volume;
                    }
                }
                truckJobTrips.Add(jobTrip);
            }
            return truckJobTrips;
        }
      
        
        #endregion

    }
}
