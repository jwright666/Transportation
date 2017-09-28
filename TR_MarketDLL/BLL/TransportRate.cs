using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MarketDLL.DAL;
using FM.TR_FMSystemDLL.DAL;
using TR_LanguageResource.Resources;
using System.Collections;
using System.Data;

namespace FM.TR_MarketDLL.BLL
{
    public class TransportRate
    {
        public const string frmPriceBreak = "TPT_HAU_E_PRICE_BREAK";
        public const string RateType_R = "R";
        public const string RateType_S = "S";
        public const decimal LAST_PRICEBREAK_END = 999999999;

        private int quotationID;
        private int sequenceNo;
  	    private string chargeID;
        private string description;
  	    private string chargeType;
        private string uom;
        private decimal minimum;
	    private string currency;
        private SortableList<PriceBreaks> priceBreaks;
        private byte [] updateVersion;
        private string remarks;
        private bool isSectorDependent;
        private string startSectorCode;
        private string endSectorCode;
        private bool isStopDependent;
        private string startStop;
        private string endStop;
        private bool isContainerMovement;
        private string containerCode;
        private bool isOverweight;
        private bool isTruckMovement;
        private bool isNew;
        private int noOfLeg; // 0 means non-movement, 1 means haulier single leg, 2 means haulier round trip

        public TransportRate()
        {
            this.quotationID = 0;
            this.sequenceNo = 0;
            this.chargeID = "";
            this.description = "";
            this.chargeType = RateType_R;
            this.uom = "";
            this.minimum = 0;
            this.currency = "";
            this.priceBreaks = new SortableList<PriceBreaks>();
            this.updateVersion = new byte[8];
            this.remarks = "";
            this.isSectorDependent = false;
            this.startSectorCode = "";
            this.endSectorCode = "";
            this.isStopDependent = false;
            this.startStop = "";
            this.endStop = "";
            this.isNew = true;
	        this.isContainerMovement=false;
	        this.containerCode="";
	        this.isOverweight=false;
            this.isTruckMovement=false;
            this.noOfLeg = 0;
        }

        public TransportRate(int quotationID, int sequenceNo, string chargeID, string description,
            string chargeType, string uom, decimal minimum, string currency,
            SortableList<PriceBreaks> priceBreaks, byte[] updateVersion, string remarks,
            bool isSectorDependent, string startSectorCode, string endSectorCode,
            bool isStopDependent, string startStop, string endStop, bool isContainerMovement,
            string containerCode, bool isOverweight, bool isTruckMovement, bool isNew, int noOfLeg)
        {

            this.QuotationID = quotationID;
            this.SequenceNo = sequenceNo;
            this.ChargeID = chargeID;
            this.Description = description;
            this.ChargeType = chargeType;
            this.UOM = uom;
            this.Minimum = minimum;
            this.Currency = currency;
            this.PriceBreaks = priceBreaks;
            this.UpdateVersion = updateVersion;
            this.Remarks = remarks;
            this.IsSectorDependent = isSectorDependent;
            this.StartSectorCode = startSectorCode;
            this.EndSectorCode = endSectorCode;
            this.IsStopDependent = isStopDependent;
            this.StartStop = startStop;
            this.EndStop = endStop;
            this.IsNew = isNew;
            this.IsContainerMovement = isContainerMovement;
            this.ContainerCode = containerCode;
            this.IsOverweight = isOverweight;
            this.IsTruckMovement = isTruckMovement;
            this.NoOfLeg = noOfLeg;
        }
        public int NoOfLeg
        {
            get { return noOfLeg; }
            set { noOfLeg = value; }
        }

        public int QuotationID
        {
            get { return quotationID; }
            set
            {
                string temp = value.ToString();
                temp = temp.Trim();
                if (temp.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrQuotationIDBlank);
                }
                else
                {
                    quotationID = value;
                }
            }
        }

        public int SequenceNo
        {
            get { return sequenceNo; }
            set { sequenceNo = value; }
        }

        public string ChargeID
        {
            get { return chargeID; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrChargeIDBlank);
                }
                else
                {
                    chargeID = value;
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrDescriptionBlank);
                }
                else
                {
                    description = value;
                }
            }
        }

        public string ChargeType
        {
            get { return chargeType; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrChargeTypeBlank);
                }
                else
                {
                    if ((value == RateType_R) || (value == RateType_S))
                    {
                        chargeType = value;
                    }
                    else
                    {
                        throw new FMException(TptResourceBLL.ErrChargeTypeNotValid);
                    }
                }
            }
        }

        public string UOM
        {
            get { return uom; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrUOMBlank);
                }
                else
                {
                    uom = value;
                }
            }
        }

        public decimal Minimum
        {
            get { return minimum; }
            set
            {
                string temp = value.ToString();
                temp = temp.Trim();
                if (temp.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrMinimumBlank);
                }
                else
                {
                    if (value<0) 
                    {
                        throw new FMException(TptResourceBLL.ErrMinimumLessThanZero);
                    } else
                    {
                        minimum = value;
                    }

                }
            }
        }

        public string Currency
        {
            get { return currency; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrCurrencyBlank);
                }
                else
                {
                    currency = value;
                }
            }
        }

        public SortableList<PriceBreaks> PriceBreaks
        {
            get { return priceBreaks; }
            set { priceBreaks = value; }
        }

        public byte[] UpdateVersion
        {
            get { return updateVersion; }
            set { updateVersion = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public bool IsSectorDependent
        {
            get { return isSectorDependent; }
            set { isSectorDependent = value; }
        }

        public string StartSectorCode
        {
            get { return startSectorCode; }
            set { startSectorCode = value; }
        }

        public string EndSectorCode
        {
            get { return endSectorCode; }
            set { endSectorCode = value; }
        }

        public bool IsStopDependent
        {
            get { return isStopDependent; }
            set { isStopDependent = value; }
        }

        public string StartStop
        {
            get { return startStop; }
            set { startStop = value; }
        }

        public string EndStop
        {
            get { return endStop; }
            set { endStop = value; }
        }

        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }

        public bool IsContainerMovement
        {
            get { return isContainerMovement; }
            set { isContainerMovement = value; }
        }

        public string ContainerCode
        {
            get { return containerCode; }
            set { containerCode = value; }
        }

        public bool IsOverweight
        {
            get { return isOverweight; }
            set { isOverweight = value; }
        }

        public bool IsTruckMovement
        {
            get { return isTruckMovement; }
            set { isTruckMovement = value; }
        }   
        public decimal GetTotal(decimal qty)
        {
            #region Original method
            /*
            decimal start = 0;
            decimal balance = qty;
            decimal range = 0;
            decimal total = 0;
            if (qty != 0)
            {
                for (int i = 0; i < priceBreaks.Count; i++)
                {
                    if (qty >= priceBreaks[i].End)
                    {
                        range = priceBreaks[i].End - start;
                        balance = balance - range;
                        total = total + priceBreaks[i].GetSubTotal(range);
                        start = priceBreaks[i].End;
                    }
                    else
                    {
                        total = total + priceBreaks[i].GetSubTotal(balance);
                        balance = 0;
                    }
                    if (balance <= 0)
                        break;
                }
            }
            // throw an exception if there is still a balance left
            if (balance > 0)
            {
                throw new FMException(TptResourceBLL.ErrQtyExceedPriceBreaks);
            }
            if (total < minimum)
                total = minimum;
            return total;
            */
            #endregion
            //20130425 - gerry replaced
            decimal start = 0;
            decimal range = 0;
            decimal total = 0;
            decimal balance = qty;
            if (qty != 0)
            {
                for (int i = 0; i < priceBreaks.Count; i++)
                {
                    bool exitLoop = false;
                    switch (this.chargeType)
                    {
                        case RateType_R:
                            if (qty >= priceBreaks[i].End)
                            {
                                range = priceBreaks[i].End - start;
                                balance = balance - range;
                                total = total + priceBreaks[i].GetSubTotal(range);
                            }
                            else
                            {
                                total = total + priceBreaks[i].GetSubTotal(balance);
                                balance = 0;
                                exitLoop = true;  
                            }                     
                            break;
                        case RateType_S:
                            balance = 0;
                            if (qty > start && qty <= priceBreaks[i].End)
                            {
                                decimal price = priceBreaks[i].GetSubTotal(qty); //.IsLumpSum ? priceBreaks[i].LumpSumValue : priceBreaks[i].Rate * qty;
                                total += price;
                                exitLoop =true;                                  
                            }
                            break;
                    }  
                    start = priceBreaks[i].End;
                    if (exitLoop)
                        break;
                }     
            }
            // throw an exception if there is still a balance left
            if (balance > 0)
            {
                throw new FMException(TptResourceBLL.ErrQtyExceedPriceBreaks);
            }
            if (total < minimum)
                total = minimum;
            return total;
        }
        public static SortableList<TransportRate> GetAllTransportRates(string custID)
        {
            return TransportMarketDAL.GetAllTransportRates(custID);

        }
        //Gerry Add fmModule parameter because it might be Trucking or Haulage
        public bool AddPriceBreakRange(FMModule fmModule, PriceBreaks priceBreakRange, string frmName, string user,
            SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;

            try
            {
                PriceBreaks pricebreakout = new PriceBreaks();
                temp = TransportMarketDAL.AddPriceBreakRange(quotationID, this, priceBreakRange, con, tran, out pricebreakout);

                #region 20131226 - Gerry Removed based on specs 11 Dec 2013
                /*
                // now form the log entry
                DateTime serverDateTime = Logger.GetServerDateTime();

                // form the header
                LogHeader logHeader = new LogHeader(fmModule, frmPriceBreak, serverDateTime,
                    this.quotationID.ToString(), priceBreakRange.Seq_No.ToString(), FormMode.Add, user);

                // This is an example of create the 2 logDetail objects
                LogDetail logDetail2 = new LogDetail("End", priceBreakRange.End.ToString());
                LogDetail logDetail3 = new LogDetail("LumSumValue", priceBreakRange.LumpSumValue.ToString());
                LogDetail logDetail4 = new LogDetail("Rate", priceBreakRange.Rate.ToString());


                // add the 2 logDetails objects to the List collection of logHeader
                logHeader.LogDetails.Add(logDetail2);
                logHeader.LogDetails.Add(logDetail3);
                logHeader.LogDetails.Add(logDetail4);

                // now call the Logger class to write

                Logger.WriteLog(logHeader, con, tran);
                 */
                #endregion
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex)
            {
                throw ex;
            }
            return temp;                   
        }

        /* Not required
        public bool EditPriceBreakRange(PriceBreaks priceBreakRange, string frmName, string user)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                TransportMarketDAL.EditPriceBreakRange(quotationID, this, priceBreakRange, con, tran);

                // now form the log entry
                DateTime serverDateTime = Logger.GetServerDateTime();
                FMModule module = FMModule.Transport;

                // form the header
                LogHeader logHeader = new LogHeader(FMModule.Transport, frmName, serverDateTime,
                    quotationID + chargeID, priceBreakRange.Seq_No.ToString(), FormMode.Edit, user);

                // This is an example of create the 2 logDetail objects
                LogDetail logDetail2 = new LogDetail("End", priceBreakRange.End.ToString());
                LogDetail logDetail3 = new LogDetail("LumSumValue", priceBreakRange.LumpSumValue.ToString());
                LogDetail logDetail4 = new LogDetail("Rate", priceBreakRange.Rate.ToString());


                // add the 2 logDetails objects to the List collection of logHeader
                logHeader.LogDetails.Add(logDetail2);
                logHeader.LogDetails.Add(logDetail3);
                logHeader.LogDetails.Add(logDetail4);

                // now call the Logger class to write

                Logger.WriteLog(logHeader, con, tran);

                tran.Commit();
                temp = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                temp = false;
            }
            finally
            {
                con.Close();
            }
            return temp;
        }
        */
        //Gerry Add fmModule parameter because it might be Trucking or Haulage
        public bool DeletePriceBreakRange(FMModule fmModule, PriceBreaks priceBreakRange, string frmName, string user, 
            SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                temp = TransportMarketDAL.DeletePriceBreakRange(quotationID, this, priceBreakRange, con, tran);
                #region 20131226 - Gerry Removed based on specs 11 Dec 2013
                /*
                // now form the log entry
                DateTime serverDateTime = Logger.GetServerDateTime();

                // form the header
                LogHeader logHeader = new LogHeader(fmModule, frmPriceBreak, serverDateTime,
                   this.quotationID.ToString(), priceBreakRange.Seq_No.ToString(), FormMode.Delete, user);

                // This is an example of create the 2 logDetail objects
                LogDetail logDetail2 = new LogDetail("End", priceBreakRange.End.ToString());
                LogDetail logDetail3 = new LogDetail("LumSumValue", priceBreakRange.LumpSumValue.ToString());
                LogDetail logDetail4 = new LogDetail("Rate", priceBreakRange.Rate.ToString());


                // add the 2 logDetails objects to the List collection of logHeader
                logHeader.LogDetails.Add(logDetail2);
                logHeader.LogDetails.Add(logDetail3);
                logHeader.LogDetails.Add(logDetail4);

                // now call the Logger class to write

                Logger.WriteLog(logHeader, con, tran);
                 */
                #endregion
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
            return temp;            
        }

        public bool DeletePriceBreakRange(FMModule fmModule, PriceBreaks priceBreakRange, string frmName, string user)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                temp = TransportMarketDAL.DeletePriceBreakRange(quotationID, this, priceBreakRange, con, tran);
                #region 20131226 - Gerry Removed based on specs 11 Dec 2013
                /*
                // now form the log entry
                DateTime serverDateTime = Logger.GetServerDateTime();

                // form the header
                LogHeader logHeader = new LogHeader(fmModule, frmPriceBreak, serverDateTime,
                   this.quotationID.ToString(), priceBreakRange.Seq_No.ToString(), FormMode.Delete, user);

                // This is an example of create the 2 logDetail objects
                LogDetail logDetail2 = new LogDetail("End", priceBreakRange.End.ToString());
                LogDetail logDetail3 = new LogDetail("LumSumValue", priceBreakRange.LumpSumValue.ToString());
                LogDetail logDetail4 = new LogDetail("Rate", priceBreakRange.Rate.ToString());


                // add the 2 logDetails objects to the List collection of logHeader
                logHeader.LogDetails.Add(logDetail2);
                logHeader.LogDetails.Add(logDetail3);
                logHeader.LogDetails.Add(logDetail4);

                // now call the Logger class to write

                Logger.WriteLog(logHeader, con, tran);
                 */
                #endregion
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
            return temp;
        }

        public bool UpdatePriceBreakRange(FMModule fmModule, PriceBreaks priceBreakRange, string frmName, string user,
            SqlConnection con, SqlTransaction tran)
        {
            bool temp = false;
            try
            {
                temp = TransportMarketDAL.UpdatePriceBreakRange(quotationID, this, priceBreakRange, con, tran);
                #region 20131226 - Gerry Removed based on specs 11 Dec 2013
                /*
                // now form the log entry
                DateTime serverDateTime = Logger.GetServerDateTime();

                // form the header
                LogHeader logHeader = new LogHeader(fmModule, frmPriceBreak, serverDateTime,
                   this.quotationID.ToString(), priceBreakRange.Seq_No.ToString(), FormMode.Edit, user);

                // This is an example of create the 2 logDetail objects
                LogDetail logDetail2 = new LogDetail("End", priceBreakRange.End.ToString());
                LogDetail logDetail3 = new LogDetail("LumSumValue", priceBreakRange.LumpSumValue.ToString());
                LogDetail logDetail4 = new LogDetail("Rate", priceBreakRange.Rate.ToString());


                // add the 2 logDetails objects to the List collection of logHeader
                logHeader.LogDetails.Add(logDetail2);
                logHeader.LogDetails.Add(logDetail3);
                logHeader.LogDetails.Add(logDetail4);

                // now call the Logger class to write

                Logger.WriteLog(logHeader, con, tran);
                 */
                #endregion

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
            return temp;   
        }

        public SortableList<PriceBreaks> GetAllPriceBreakRange()
        {
            return TransportMarketDAL.GetAllPriceBreakRange(this);
        }

        internal bool ValidateAllProperties()
        {
            //validate by using property methods
            bool temp = false;

            ChargeID = this.chargeID;
            Description = this.description;
            ChargeType = this.chargeType;
            UOM = this.uom;
            Minimum = this.minimum;
            Currency = this.currency;


            if ((IsContainerMovement == false && IsTruckMovement == false) && (IsSectorDependent == true || IsStopDependent == true))
            {
                throw new FMException(TptResourceBLL.ErrInvalidIsSectorOrIsStopDependent);
            }
            if ((IsSectorDependent == true) && (IsStopDependent == true))
            {
                throw new FMException(TptResourceBLL.ErrDependent);
            }
            if (IsSectorDependent == true)
            {
                if ((StartSectorCode == "") || (EndSectorCode == ""))
                {
                    throw new FMException(TptResourceBLL.ErrSectorBlank);
                }
            }
            if (IsStopDependent == true)
            {
                if ((StartStop == "") || (EndStop == ""))
                {
                    throw new FMException(TptResourceBLL.ErrStopBlank);
                }
                else
                {
                    if (StartStop ==  EndStop)
                    {
                        throw new FMException(TptResourceBLL.ErrSameStop);
                    }
                }
            }
            if (PriceBreaks.Count == 0)
            {
                throw new FMException(TptResourceBLL.ErrNoPriceBreak);
            }
            //gerry removed
            //if (ChargeType == RateType_R)
            //{
            //    if (PriceBreaks.Count != 1)
            //    {
            //        throw new FMException(TptResourceBLL.ErrPriceBreakMustBeOne);
            //    }
            //}


            temp = true;
            return temp;

        }

        public bool PriceBreakEndChecking()
        {
            bool temp = true;
            if (PriceBreaks.Count > 1)
            {
                for (int i = 1; i < PriceBreaks.Count ; i++)
                {
                    if (PriceBreaks[i - 1].End >= PriceBreaks[i].End)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrEndPriceBreak);
                    }
                }
            }
            // set the last priceBreak.End to a high value 
            int n = PriceBreaks.Count;
            PriceBreaks[n-1].End = LAST_PRICEBREAK_END;
            
            return temp;

        }

        public bool ValidateAllPriceBreaksProperties()
        {
            bool temp = true;
            if (PriceBreaks.Count > 1)
            {
                for (int i = 0; i < PriceBreaks.Count; i++)
                { 
                    if (PriceBreaks[i].ValidateAllProperties()==false)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrPriceBreakNotValid);
                    }
                }
            }

            return temp;

        }

        // This method is used to reset the Sequence No if
        // 1 priceBreak is deleted from the collection
        // This method should be called before saving the collection into database
        public void ResetSequenceNo()
        {
            if (PriceBreaks.Count > 1)
            {
                for (int i = 0; i < PriceBreaks.Count; i++)
                {
                    PriceBreaks[i].Seq_No = i + 1;
                }
            }

        }


        public static TransportRate GetTransportRate(string QuotationNo, string ChargeCode, int SeqNo)
        {
            return TransportMarketDAL.GetTransportRate(QuotationNo, ChargeCode,SeqNo);
        }

        public static TransportRate GetTransportRate(int QuotationID, string ChargeCode)
        {
            return TransportMarketDAL.GetTransportRate(QuotationID, ChargeCode);
        }
        //for stop dependent or none stop dependent
        public static ArrayList GetTransportRatesUOMs(int quotationID, string chargeCode, bool isTruckMovement, bool isContainerMovement, bool isStopDependent)
        {
            ArrayList temp = new ArrayList();
            try
            {
                temp = TransportMarketDAL.GetTransportRatesUOMs(quotationID, chargeCode, isTruckMovement, isContainerMovement, isStopDependent);
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return temp;
        }
        //without stop dependent or none stop dependent
        public static ArrayList GetTransportRatesUOMs(int quotationID, string chargeCode, bool isTruckMovement, bool isContainerMovement)
        {
            ArrayList temp = new ArrayList();
            try
            {
                temp = TransportMarketDAL.GetTransportRatesUOMs(quotationID, chargeCode, isTruckMovement, isContainerMovement);
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return temp;
        }

        public static TransportRate GetTransportRatesBasedOnQuotationNoAndUOMandStopDependent(string QuotationNo, string uom, string chargeCode, bool stopDependent)
        {
            return TransportMarketDAL.GetTransportRatesBasedOnQuotationNoAndUOMandStopDependent(QuotationNo, uom, chargeCode, stopDependent);
        }

        public static TransportRate GetTransportRatesBasedOnQuotationNoAndUOMForInvoice(string QuotationNo, string uom, string chargeCode)
        {
            return TransportMarketDAL.GetTransportRatesBasedOnQuotationNoAndUOMForInvoice(QuotationNo, uom, chargeCode);
        }
        /*
        public static SortableList<TransportRate> GetTransportRateForInvoice(string QuotationNo, string uom, string chargeCode)
        {
            return TransportMarketDAL.GetTransportRatesBasedOnQuotationNoAndUOM(QuotationNo, uom, chargeCode);
        }
        */

        public static DataTable GetTransportRatesForInvoice(string QuotationNo, string chargeCode, string uom)
        {
            return TransportMarketDAL.GetTransportRatesBasedOnQuotationNoAndUOM(QuotationNo, chargeCode, uom);
        }

        public static SortableList<TransportRate> GetNotMovementTransportRates(string quotationNo, bool isTruckMovement, bool isContainerMovement)
        {
            return TransportMarketDAL.GetNotMovementTransportRates(quotationNo, isTruckMovement, isContainerMovement);
        }
        //Gerry Add validation for pricebreak
        public bool ValidateAddPricebreak(PriceBreaks price)
        {
            if (this.priceBreaks.Count > 0)
            {
                int index = this.PriceBreaks.Count - 1;
                if (this.priceBreaks[index].End >= price.End)
                {
                    throw new FMException(TptResourceBLL.ErrPriceBreakEnd);
                }
            }

            return true;
        }

        /// <summary>
        /// 2014-08-11 Zhou Kai adds this function, to calculate the charge for a certain
        /// billing quantity using this transport rate.
        /// NOTICE: this function is not used and not tested! It's only a try for a clearer 
        /// and shorter version of the function: public decimal GetTotal(decimal qty),
        /// which is tested and actually in use.
        /// </summary>
        /// <param name="billing_quantity">The billing quantity.</param>
        /// <returns>The charge amount for this billing quantity with this transport rate.</returns>
        public decimal CalculateTotalChargeForACertainQuantity(decimal billing_quantity)
        {
            decimal totalCharge = 0M;
            // find the last price break for this billing quantity
            PriceBreaks last_break = this.priceBreaks.First(x => x.End >= billing_quantity);
            if (object.ReferenceEquals(last_break, null))
            { throw new FMException(TptResourceBLL.ErrQtyExceedPriceBreaks); }

            // if the rate type is 'S'
            if (this.ChargeType == RateType_R)
            {
                // calculate the charge amount for the last break
                totalCharge = last_break.GetSubTotal(billing_quantity - last_break.End);
                // calcualte the charge amoutn for the previous breaks
                int last_break_index = last_break.Seq_No;
                for (int i = 0; i < last_break_index; i++)
                {
                    if (i >= 1)
                    {
                        totalCharge +=
                            this.priceBreaks[i].GetSubTotal(priceBreaks[i].End - priceBreaks[i - 1].End);
                    }
                    else
                    {
                        totalCharge += this.priceBreaks[0].GetSubTotal(priceBreaks[0].End);
                    }
                }
            }

            // if the rate type is 'R'
            if (this.ChargeType == RateType_S)
            {
                totalCharge = last_break.GetSubTotal(billing_quantity);
            }

            totalCharge = totalCharge < minimum ? minimum : totalCharge;

            return totalCharge;
        }
    }
}
