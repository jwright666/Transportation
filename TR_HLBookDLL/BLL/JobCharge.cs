using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MarketDLL.BLL;
//using FM.TransportBook.Resources;
using FM.TR_HLBookDLL.DAL;
using TR_LanguageResource.Resources;
using FM.TR_MaintenanceDLL.BLL;
using System.Collections;
using System.Globalization;



namespace FM.TR_HLBookDLL.BLL
{
    public abstract class JobCharge
    {
        #region Private Properties
        private int invTranNo;
        private int invItemSeqNo; 
        private int jobID;
        private int sequenceNo;
        private string chargeCode;
        private string chargeDescription;
        private string uom;
        private decimal quantity;
        private decimal unitRateFC;
        private decimal totalAmountHC;
        private decimal totalAmountFC;
        private string gstType;
        private decimal gstRate;
        private decimal gstAmountHC;
        private decimal gstAmountFC;    //20121228 - Gerry added for gstamount fc 
        private string currency;
        private decimal exchangeRate;
        private string remarks;
        private string quotationNo;
        private int quotationID;
        private int sequenceNoRate;
        private string jobChargeType;
        private DateTime completedDate;
        private byte[] updateVersion;
        private JobChargeStatus jobChargeStatus;
        public TransportRate transportRate;
        private JobChargeStatus oldJobChargeStatus;
        private string rateType;
        #endregion
        #region Getter and Setter Methods
        public int InvTranNo
        {
            get { return invTranNo; }
            set { invTranNo = value; }
        }
        public int InvItemSeqNo
        {
            get { return invItemSeqNo; }
            set { invItemSeqNo = value; }
        }        
        public int JobID
        {
            get { return jobID; }
            set { jobID = value; }
        }
        public int SequenceNo
        {
            get { return sequenceNo; }
            set { sequenceNo = value; }
        }
        public string ChargeCode
        {
            get { return chargeCode; }
            set { chargeCode = value.Trim(); }
        }
        public string ChargeDescription
        {
            get { return chargeDescription; }
            set { chargeDescription = value.Trim(); }
        }
        public string Uom
        {
            get { return uom; }
            set {                             
                uom = value;                      
            }
        }
        public string RateType
        {
            get { return rateType; }
            set { rateType = value; }
        }
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        public decimal ExchangeRate
        {
            get { return exchangeRate; }
            set
            {
                try
                {
                    //if (CurrencyRate.GetValidCurrencyRateVarianceRange(Currency, DateTime.Today, value))
                    exchangeRate = decimal.Round(value, 6);
                    //else
                    //    throw new FMException(Currency + " " + TptResourceBLL.ErrExceedVariance);
                }
                catch (FMException fmEx) { throw fmEx; }
                catch (Exception ex) { throw ex; }
            }
        }
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                TotalAmountFC = value * unitRateFC;
            }
        }
        public decimal UnitRateFC
        {
            get { return unitRateFC; }
            set
            {
                unitRateFC = value;
                TotalAmountFC = value * quantity;
            }
        }
        public decimal TotalAmountHC
        {
            get { return totalAmountHC; }
            set
            {
                totalAmountHC = ConvertHomeCurrenRate(value);
            }
            
        }
        public decimal TotalAmountFC
        {
            get { return totalAmountFC; }
            set {
                totalAmountFC = value;
                TotalAmountHC = totalAmountFC;      
                GstAmountFC = value * (GstRate / 100);
            }
        }
        public string GstType
        {
            get { return gstType; }
            set { gstType = value; }
        }
        public decimal GstRate
        {
            get { return gstRate; }
            set
            {
                gstRate = value;
                GstAmountFC = totalAmountFC * (value / 100);
            }
        }
        public decimal GstAmountHC
        {
            get { return gstAmountHC; }
            set {
                if (GstAmountFC != 0 & ExchangeRate != 0)
                {
                    value = ConvertHomeCurrenRate(GstAmountFC);
                }
                gstAmountHC = value; 
            }
        }
        public decimal GstAmountFC
        {
            get { return gstAmountFC; }
            set {
               //gstAmountFC = value;

                gstAmountFC = value;
                GstAmountHC = gstAmountFC;
            }
        }
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        public string QuotationNo
        {
            get { return quotationNo; }
            set { quotationNo = value; }
        }
        public int QuotationID
        {
            get { return quotationID; }
            set { quotationID = value; }
        }
        public int SequenceNoRate
        {
            get { return sequenceNoRate; }
            set { sequenceNoRate = value; }
        }
        public string JobChargeType
        {
            get { return jobChargeType; }
            set { jobChargeType = value; }
        }
        public DateTime CompletedDate
        {
            get { return completedDate; }
            set { completedDate = value; }
        }
        public JobChargeStatus JobChargeStatus
        {
            get { return jobChargeStatus; }
            set { jobChargeStatus = value; }
        }
        public JobChargeStatus OldJobChargeStatus
        {
            get { return oldJobChargeStatus; }
            set { oldJobChargeStatus = value; }
        }
        public byte[] UpdateVersion
        {
            get { return updateVersion; }
            set { updateVersion = value; }
        }
        //20131010 -Gerry Added for jobtrip seqNo to link between job trip ang job charge
        public int JobTripSeqNo { get; set; }


        #endregion
        public JobCharge()
        {
            this.completedDate = DateTime.Today;
            this.invTranNo = 0;
            this.invItemSeqNo = 0;
            this.jobID = 0;
            this.sequenceNo = 0;
            this.chargeCode = "";
            this.chargeDescription = "";
            this.uom = "";
            this.quantity = 0;
            this.currency = "";
            this.exchangeRate = 0;
            this.unitRateFC = 0;
            this.totalAmountHC = 0;
            this.totalAmountFC = 0;
            this.gstRate = 0;
            this.gstAmountHC = 0;
            this.gstAmountFC = 0;
            this.remarks = "";
            this.quotationNo = "";
            this.quotationID = 0;
            this.jobChargeType = "S";
            this.jobChargeStatus = JobChargeStatus.Booked;
            this.sequenceNoRate = 0;
            this.gstType = "";
            this.oldJobChargeStatus = JobChargeStatus.Booked;
            this.rateType = "R";
            this.JobTripSeqNo = 0;
        }

        public JobCharge(int invTrxNo, int invItemSeqNo, int jobId, int sequenceNo,string chargeCode,string chargeDescription,string uom,
            decimal quantity,decimal unitRateFC,decimal totalAmountHC,decimal totalAmountFC, string gstType,
            decimal gstRate,decimal gstAmountFC,string currency,decimal exchangeRate,
            string remarks, string quotationNo, int quotationID, int sequenceNoRate, string jobChargeType, 
            DateTime completedDate, JobChargeStatus jobChargeStatus, string rateType,
            byte[] updateVersion)
        {
            this.CompletedDate = completedDate;
            this.InvTranNo = invTrxNo;
            this.InvItemSeqNo = invItemSeqNo;
            this.JobID = jobId;
            this.SequenceNo = sequenceNo;
            this.ChargeCode = chargeCode;
            this.ChargeDescription = chargeDescription;
            this.Uom = uom;
            this.Currency = currency;
            this.ExchangeRate = exchangeRate;
            this.Quantity = quantity;
            this.UnitRateFC = unitRateFC;
            //this.TotalAmountHC = totalAmountHC;   //Gerry remove the assigment of totalAmountHC because it will recalculate in setter method
            this.TotalAmountFC = totalAmountFC;
            this.GstType = gstType;
            this.GstRate = gstRate;
            //this.GstAmountHC = gstAmountHC;   //Gerry remove the assigment of GstAmountHC because it will recalculate in setter method
            this.GstAmountFC = gstAmountFC;
            this.Remarks = remarks;
            this.QuotationNo = quotationNo;
            this.QuotationID = quotationID;
            this.UpdateVersion = updateVersion;
            this.JobChargeType = jobChargeType;
            this.JobChargeStatus = jobChargeStatus;
            this.SequenceNoRate = sequenceNoRate;
            this.RateType = rateType;
        }

        public bool ValidationCheck()
        {
            bool temp = true;
            if (chargeDescription == "")
            {  
                temp = false;
                throw new FMException(TptResourceBLL.ErrMissingChargeDesc);
                
            }
            if (gstAmountHC != CalculateGSTAmountHC())
            {   
                temp = false;
                throw new FMException(TptResourceBLL.ErrGstAmountHCNotCorrect);
                
            }
            return temp;
        }

        public decimal CalculateGSTAmountHC()
        {
            decimal temp = 0;
            temp = totalAmountFC * exchangeRate * gstRate;
            return temp;
        }

        public string GetUOM()
        {
            string temp = "";
            
            return temp;

        }

        public bool ValidateAddJobCharge()
        {
            bool temp = true;
            if (HaulierJobDAL.FindDuplicateJobCharge(jobID, chargeCode) == true)
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrDuplicateJobCharge);                
            }
            else
            {

                if (quotationNo == "")
                {
                    if (currency == "")
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrMissingCurrency);                        
                    }
                    if (unitRateFC == 0)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrMissingUnitRateFC);                        
                    }
                    if (quantity == 0)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrorQuantityEmpty);
                    }

                }
                else
                {
                    transportRate = TransportRate.GetTransportRate(quotationNo, chargeCode, sequenceNoRate);
                    currency = transportRate.Currency;
                    totalAmountFC = transportRate.GetTotal(quantity);
                    unitRateFC = totalAmountFC / quantity;

                }
            }
            return temp;
        }

        public bool ValidateEditJobCharge()
        {
            bool temp = true;

            if (oldJobChargeStatus == JobChargeStatus.Invoiced)
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrCantEdit);                
            }
            else
            {
                if (quotationNo != "")
                {
                    transportRate = new TransportRate();
                    transportRate = TransportRate.GetTransportRate(quotationNo, chargeCode, sequenceNoRate);

                    //if (totalAmountFC != transportRate.GetTotal(quantity))
                    //{
                    //    temp = false;
                    //    throw new FMException(TptResourceBLL.ErrTotalAmountFCNotCorrect);
                    //}
                }
                else
                {
                    if (currency == "")
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrMissingCurrency);
                    }
                    if (unitRateFC == 0)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrMissingUnitRateFC);
                    }
                    if (quantity == 0)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrorQuantityEmpty);
                    }
                }

            }
            return temp;
        }

        public bool ValidateDeleteJobCharge()
        {
            bool temp = true;
            if (jobChargeStatus == JobChargeStatus.Invoiced)
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrCantDeleteInvoiced);
            }
            else
            {
                if (jobChargeStatus == JobChargeStatus.Invoiced)
                {
                    temp = false;
                    throw new FMException(TptResourceBLL.ErrCantDeleteCompleted);
                }
            }
            return temp;
        }

        public bool ValidateFields()
        {
            bool temp = true;
            //msg = "";
            if (ChargeCode == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrMissingChargeCode);
            }
            if (Currency == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrMissingCurrency);
            }
            if (Quantity == 0)
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrorQuantityEmpty);
            }
            if (UnitRateFC == 0)
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrMissingUnitRateFC);
            }

            return temp;
        }



        public Decimal ConvertHomeCurrenRate(decimal amount)
        {
            try
            {
                CurrencyRate curr = CurrencyRate.GetCurrencyRate(currency, completedDate);
                decimal exchRate = curr.CurrFactor / ExchangeRate;

                amount = amount / exchRate;

                TransportSettings transportSettings = TransportSettings.GetTransportSetting();
                CurrencyRate homeCurrRate = CurrencyRate.GetCurrencyRate(transportSettings.HomeCurrency.ToString().Trim(), DateTime.Today);
                amount = CurrencyRate.RoundAndShowDecimal(homeCurrRate, amount);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
            return amount;
        }
        //this method is to display the currency equivalent rate amount
        public string DisplayEquivRate()
        {
            string retValue = string.Empty;
            try
            {
                CurrencyRate currencyRate = CurrencyRate.GetCurrencyRate(Currency, DateTime.Today);
                decimal tempLocEquivRate = 0;
                tempLocEquivRate = currencyRate.CurrFactor / ExchangeRate;
                
                TransportSettings transportSettings = TransportSettings.GetTransportSetting();
                CurrencyRate homeCurrRate = CurrencyRate.GetCurrencyRate(transportSettings.HomeCurrency.ToString().Trim(), DateTime.Today);

                if (homeCurrRate.CurrencyCode.Trim() != currencyRate.CurrencyCode.Trim())
                    retValue = String.Format("1 {0} = {1} {2}", homeCurrRate.CurrencyCode, tempLocEquivRate.ToString("N6", NumberFormatInfo.CurrentInfo), currencyRate.CurrencyCode);
                else
                    retValue = string.Empty;
            }
            catch { ;}
            return retValue;
        }

        public decimal CalculateGst()
        {
            decimal value = 0;
            CurrencyRate curr = CurrencyRate.GetCurrencyRate(currency, completedDate);
            decimal exchRate = curr.CurrFactor / ExchangeRate;

            value = (TotalAmountFC * exchRate) * (GstRate / 100);

            return value;
        }
    }
}
