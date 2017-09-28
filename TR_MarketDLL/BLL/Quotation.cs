using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MarketDLL.DAL;
using TR_LanguageResource.Resources;
using System.Collections;
using System.Data;

namespace FM.TR_MarketDLL.BLL
{
    public class Quotation
    {
        //used to log pricebreak
        public const string frmPriceBreak = "TPT_HAU_E_PRICE_BREAK";
        //20131226 - gerry added constants string for log details
        #region LogDetails constants for QUOTATION Header
        public const string LOGDETAIL_SALESMAN = "Salesman";
        public const string LOGDETAIL_VALIDFROM = "ValidFrom";
        public const string LOGDETAIL_VALIDTO = "ValidTo";
        public const string LOGDETAIL_DATECHANGED = "DateChanged";
        #endregion
        #region LogDetails constants for TransportRates
        public const string LOGDETAIL_CHARGE_ID = "ChargeID";
        public const string LOGDETAIL_UOM = "UOM";
        #endregion
        //20131226 end         
        public const string LOGDETAIL_DELETED_DATETIME = "Deleted DateTime";

        private int quotationID;
        private string quotationNo;
        private QuotationType quotationType;
        private string custNo;
        private string customerGroup;
        private DateTime quotationDate;
        private DateTime validFrom;
        private DateTime validTo;
        private int creditDays;
        private string salesman;
        private string conditions;
        private string paymentTerms;
        private byte[] updateVersion;
        private string remarks;
        private string remarks2;
        private string remarks3;
        private string remarks4;
        private string remarks5;
        private SortableList<TransportRate> transportRates;
        private bool isValid;
        private DateTime oldValidTo;
        private string branchCode;

        public Quotation()
        {
            this.quotationID = 0;
            this.quotationNo = "";
            this.quotationType = QuotationType.Customer;
            this.custNo = "";
            this.customerGroup = "";
            this.quotationDate = DateTime.Today;
            this.validFrom = DateTime.Today;
            this.validTo = DateTime.Today;
            this.creditDays = 0;
            this.salesman = "";
            this.conditions = "";
            this.paymentTerms = "";
            //            this.updateVersion = updateVersion;
            this.remarks = "";
            this.remarks2 = "";
            this.remarks3 = "";
            this.remarks4 = "";
            this.remarks5 = "";
            this.transportRates = new SortableList<TransportRate>();
            this.isValid = true;
            this.branchCode = "";

        }

        public Quotation(int quotationID, string quotationNo, QuotationType quotationType,
            string custNo, string customerGroup, DateTime quotationDate, DateTime validFrom,
            DateTime validTo, int creditDays, string salesman, string conditions,
            string paymentTerms, byte[] updateVersion, string remarks,
            string remarks2, string remarks3, string remarks4, string remarks5,
            SortableList<TransportRate> transportRates, bool isValid, string branchCode)
        {
            this.QuotationID = quotationID;
            this.QuotationNo = quotationNo;
            this.QuotationType = quotationType;
            this.CustNo = custNo;
            this.CustomerGroup = customerGroup;
            this.QuotationDate = quotationDate;
            this.ValidTo = validTo;
            this.ValidFrom = validFrom;
            this.CreditDays = creditDays;
            this.Salesman = salesman;
            this.Conditions = conditions;
            this.PaymentTerms = paymentTerms;
            this.UpdateVersion = updateVersion;
            this.Remarks = remarks;
            this.Remarks2 = remarks2;
            this.Remarks3 = remarks3;
            this.Remarks4 = remarks4;
            this.Remarks5 = remarks5;
            this.TransportRates = transportRates;
            this.IsValid = isValid;
            this.BranchCode = branchCode;
        }

        public int QuotationID
        {
            get { return quotationID; }
            set { quotationID = value; }
        }

        public string QuotationNo
        {
            get { return quotationNo; }
            set
            {
                quotationNo = value;
            }
        }

        public QuotationType QuotationType
        {
            get { return quotationType; }
            set
            {
                string temp = value.ToString();
                temp = temp.Trim();
                if (temp.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrQuotationTypeBlank);
                }
                else if ((value != QuotationType.Customer) && (value != QuotationType.CustomerGroup))
                {
                    throw new FMException(TptResourceBLL.ErrQuotationTypeInvalid);
                }
                else
                {
                    quotationType = value;

                }
            }
        }

        public string CustNo
        {
            get { return custNo; }
            set
            {
                if (quotationType == QuotationType.Customer)
                {
                    value = value.Trim();
                    if (value.Length == 0)
                    {
                        throw new FMException(TptResourceBLL.ErrCustNoBlank);
                    }
                    else
                    {
                        custNo = value;
                    }
                }
                else
                {
                    custNo = "";
                }
            }
        }

        public string CustomerGroup
        {
            get { return customerGroup; }
            set
            {
                if (quotationType == QuotationType.CustomerGroup)
                {
                    value = value.Trim();
                    if (value.Length == 0)
                    {
                        throw new FMException(TptResourceBLL.ErrCustomerGroupBlank);
                    }
                    else
                    {
                        customerGroup = value;
                    }
                }
                else
                {
                    customerGroup = "";
                }

            }
        }

        public DateTime QuotationDate
        {
            get { return quotationDate; }
            set
            {
                quotationDate = value;
            }
        }

        public DateTime ValidFrom
        {
            get { return validFrom; }
            set
            {
                string temp = validTo.ToString();
                temp = temp.Trim();
                if (temp.Length != 0)
                {
                    validFrom = value;
                }

            }
        }

        public DateTime ValidTo
        {
            get { return validTo; }
            set
            {
                string temp = validFrom.ToString();
                temp = temp.Trim();
                if (temp.Length != 0)
                {
                    validTo = value;
                }
            }
        }

        public int CreditDays
        {
            get { return creditDays; }
            set
            {
                if (value < 0)
                {
                    throw new FMException(TptResourceBLL.ErrQuotationDateBlank);
                }
                else
                {
                    creditDays = value;
                }
            }

        }

        public string Salesman
        {
            get { return salesman; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrSalesmanBlank);
                }
                else
                {
                    salesman = value;
                }
            }
        }

        public string Conditions
        {
            get { return conditions; }
            set
            {
                conditions = value;
            }
        }

        public string PaymentTerms
        {
            get { return paymentTerms; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrPaymentTermsBlank);
                }
                else
                {
                    paymentTerms = value;
                }
            }
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

        public string Remarks2
        {
            get { return remarks2; }
            set { remarks2 = value; }
        }

        public string Remarks3
        {
            get { return remarks3; }
            set { remarks3 = value; }
        }

        public string Remarks4
        {
            get { return remarks4; }
            set { remarks4 = value; }
        }

        public string Remarks5
        {
            get { return remarks5; }
            set { remarks5 = value; }
        }


        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }

        internal DateTime OldValidTo
        {
            get { return oldValidTo; }
            set { oldValidTo = value; }
        }


        public SortableList<TransportRate> TransportRates
        {
            get { return transportRates; }
            set { transportRates = value; }
        }

        public string BranchCode
        {
            get { return branchCode; }
            set
            {
                value = value.Trim();
                if (value.Length == 0)
                {
                    throw new FMException(TptResourceBLL.ErrBranchCodeBlank);
                }
                else
                {
                    branchCode = value;
                }
            }
        }
        //Gerry Add fmModule parameter because it might be Trucking or Haulage
        public bool AddTransportRate(FMModule fmModule, TransportRate transportRate, string frmName, string user)
        {
            try
            {
                bool temp = false;
                if (ValidateTransportRateAndPriceBreaks(transportRate))
                {
                    SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    try
                    {
                        TransportRate transportRateOut = new TransportRate();
                        AuditLog auditLog = new AuditLog();

                        TransportMarketDAL.AddTransportRate(this, transportRate, con, tran, out transportRateOut);
                        transportRate.QuotationID = this.quotationID;
                        for (int i = 0; i < transportRate.PriceBreaks.Count; i++)
                        {
                            transportRate.AddPriceBreakRange(fmModule, transportRate.PriceBreaks[i], frmPriceBreak, user, con, tran);
                            //20151028 - gerry added new audit log
                            auditLog = new AuditLog(this.quotationNo, "TR", "MK", this.quotationID, user, DateTime.Now, frmName, i, FormMode.Add.ToString());
                            auditLog.WriteAuditLog((PriceBreaks)transportRate.PriceBreaks[i], null, con, tran);
                        }
                        #region OLD logging removed
                        //// now form the log entry
                        //DateTime serverDateTime = Logger.GetServerDateTime();

                        //// form the header
                        ////20131226 - Gerry replaced parent identifier to quotaion no
                        //LogHeader logHeader = new LogHeader(fmModule, frmName, serverDateTime,
                        //    quotationNo.ToString(), transportRate.SequenceNo.ToString(), FormMode.Add, user);

                        //// This is an example of create the 2 logDetail objects
                        ////20131226 - Gerry removed other logging fields
                        ////LogDetail logDetail1 = new LogDetail("Minimum", transportRate.Minimum.ToString());
                        ////LogDetail logDetail2 = new LogDetail("Currency", transportRate.Currency);
                        //LogDetail logDetail3 = new LogDetail(LOGDETAIL_CHARGE_ID, transportRate.ChargeID);
                        ////LogDetail logDetail4 = new LogDetail("Charge Type",transportRate.ChargeType);
                        //LogDetail logDetail5 = new LogDetail(LOGDETAIL_UOM, transportRate.UOM);
                        ////LogDetail logDetail6 = new LogDetail("IsStopDependent",transportRate.IsStopDependent.ToString());
                        //// add the 2 logDetails objects to the List collection of logHeader
                        //logHeader.LogDetails.Add(logDetail3);
                        ////logHeader.LogDetails.Add(logDetail4);
                        //logHeader.LogDetails.Add(logDetail5);
                        ////logHeader.LogDetails.Add(logDetail6);
                        ////logHeader.LogDetails.Add(logDetail1);
                        ////logHeader.LogDetails.Add(logDetail2);
                        ////20131226 end

                        //// now call the Logger class to write

                        //Logger.WriteLog(logHeader, con, tran);
                        #endregion
                        //20151028 - gerry added new audit log
                        auditLog = new AuditLog(this.quotationNo, "TR", "MK", this.quotationID, user, DateTime.Now, frmName, transportRate.SequenceNo, FormMode.Add.ToString());
                        auditLog.WriteAuditLog(transportRate, null, con, tran);

                        tran.Commit();
                        transportRates.Add(transportRate);
                        temp = true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        temp = false;
                        throw;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return temp;
            }
            catch (FMException ex)
            {
                throw ex;
            }

        }
        public bool EditTransportRate(FMModule fmModule, TransportRate transportRate, string frmName, string user)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                TransportRate oldRate = TransportMarketDAL.GetTransportRate(this, transportRate.ChargeID, transportRate.SequenceNo, con, tran);
                if (ValidateTransportRateAndPriceBreaks(transportRate))
                {
                    TransportRate transportRateOut = new TransportRate();
                    TransportMarketDAL.EditTranportRate(this, transportRate, con, tran, out transportRateOut);
                    for (int i = 0; i < transportRate.PriceBreaks.Count; i++)
                    {
                        transportRate.DeletePriceBreakRange(fmModule, transportRate.PriceBreaks[i], frmPriceBreak, user, con, tran);
                    }
                    for (int i = 0; i < transportRate.PriceBreaks.Count; i++)
                    {
                        transportRateOut.AddPriceBreakRange(fmModule, transportRate.PriceBreaks[i], frmPriceBreak, user, con, tran);
                        transportRateOut.PriceBreaks.Add(transportRate.PriceBreaks[i]);
                    }
                    transportRate = transportRateOut;
                    #region OLD logging removed
                    //// now form the log entry
                    //DateTime serverDateTime = Logger.GetServerDateTime();
                    //// form the header
                    ////20131226 - Gerry replaced parent identifier to quotaion no
                    //LogHeader logHeader = new LogHeader(fmModule, frmName, serverDateTime,
                    //    quotationNo.ToString(), transportRate.SequenceNo.ToString(), FormMode.Edit, user);

                    //// This is an example of create the 2 logDetail objects
                    ////20131226 - Gerry removed other logging fields
                    ////LogDetail logDetail1 = new LogDetail("Minimum", transportRate.Minimum.ToString());
                    ////LogDetail logDetail2 = new LogDetail("Currency", transportRate.Currency);
                    //LogDetail logDetail3 = new LogDetail(LOGDETAIL_CHARGE_ID, transportRate.ChargeID);
                    ////LogDetail logDetail4 = new LogDetail("Charge Type",transportRate.ChargeType);
                    //LogDetail logDetail5 = new LogDetail(LOGDETAIL_UOM, transportRate.UOM);
                    ////LogDetail logDetail6 = new LogDetail("IsStopDependent",transportRate.IsStopDependent.ToString());
                    //// add the 2 logDetails objects to the List collection of logHeader
                    //logHeader.LogDetails.Add(logDetail3);
                    ////logHeader.LogDetails.Add(logDetail4);
                    //logHeader.LogDetails.Add(logDetail5);
                    ////logHeader.LogDetails.Add(logDetail6);
                    ////logHeader.LogDetails.Add(logDetail1);
                    ////logHeader.LogDetails.Add(logDetail2);
                    ////20131226 end
                    //// now call the Logger class to write
                    //Logger.WriteLog(logHeader, con, tran);
                    #endregion
                    //20151028 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.quotationNo, "TR", "MK", this.quotationID, user, DateTime.Now, frmName, transportRate.SequenceNo, FormMode.Edit.ToString());
                    auditLog.WriteAuditLog(transportRate, oldRate, con, tran);

                    tran.Commit();

                    temp = true;
                }
                return temp;
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                temp = false;
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public bool DeleteTransportRate(FMModule fmModule, TransportRate transportRate, string frmName, string user)
        {
            bool temp = false;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < transportRate.PriceBreaks.Count; i++)
                {
                    transportRate.DeletePriceBreakRange(fmModule, transportRate.PriceBreaks[i], frmPriceBreak, user, con, tran);
                }

                TransportMarketDAL.DeleteTransportRate(this, transportRate, con, tran);
                #region OLD logging removed
                //// now form the log entry
                //DateTime serverDateTime = Logger.GetServerDateTime();

                //// form the header
                ////20131226 - Gerry replaced parent identifier to quotaion no
                //LogHeader logHeader = new LogHeader(fmModule, frmName, serverDateTime,
                //    quotationNo.ToString(), transportRate.SequenceNo.ToString(), FormMode.Delete, user);

                ////20140217 - Gerry removed logdetails for delete
                //LogDetail logDetail = new LogDetail(LOGDETAIL_DELETED_DATETIME, serverDateTime.ToString());
                //logHeader.LogDetails.Add(logDetail);
                ////20140217 end  
                //Logger.WriteLog(logHeader, con, tran);
                #endregion

                //20151028 - gerry added new audit log
                AuditLog auditLog = new AuditLog(this.quotationNo, "TR", "MK", this.quotationID, user, DateTime.Now, frmName, transportRate.SequenceNo, FormMode.Delete.ToString());
                auditLog.WriteAuditLog(null, null, con, tran);

                tran.Commit();
                temp = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                temp = false;
                throw;
            }
            finally
            {
                this.TransportRates = GetTransportRatesForThisQuotation();
                con.Close();
            }
            return temp;
        }

        public bool AddQuotationHeader(FMModule fmModule, string frmName, string user)
        {
            bool temp = false;
            string msg = "";

            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            if (con.State == ConnectionState.Closed) { con.Open(); }
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (ValidateAllPropertiesInHeader() && ValidateValidFromAndValidTo(out msg)
                    && ValidatePeriod())
                {
                    Quotation qoutationOut = new Quotation();
                    temp = TransportMarketDAL.AddQuotationHeader(this, con, tran, out qoutationOut);
                    #region OLD logging removed
                    //// now form the log entry
                    //DateTime serverDateTime = Logger.GetServerDateTime();

                    //// form the header
                    ////20131226 - Gerry replaced child identifier to empty string
                    //LogHeader logHeader = new LogHeader(fmModule, frmName, serverDateTime,
                    //    this.quotationNo.ToString(), string.Empty, FormMode.Add, user);

                    //// This is an example of create the 2 logDetail objects
                    ////20131226 - gerry replaced the prop name to constant string instead of hard coded strings
                    //LogDetail logDetail1 = new LogDetail(LOGDETAIL_SALESMAN, salesman);
                    //LogDetail logDetail2 = new LogDetail(LOGDETAIL_VALIDFROM, validFrom.ToShortDateString());
                    //LogDetail logDetail3 = new LogDetail(LOGDETAIL_VALIDTO, validTo.ToShortDateString());

                    //// add the 2 logDetails objects to the List collection of logHeader
                    //logHeader.LogDetails.Add(logDetail1);
                    //logHeader.LogDetails.Add(logDetail2);
                    //logHeader.LogDetails.Add(logDetail3);

                    //// now call the Logger class to write

                    //Logger.WriteLog(logHeader, con, tran);
                    #endregion

                    //20151028 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.quotationNo, "TR", "MK", this.quotationID, user, DateTime.Now, frmName, 0, FormMode.Add.ToString());
                    auditLog.WriteAuditLog(this, null, con, tran);

                    tran.Commit();

                }
            }
            catch (FMException fmEx)
            {
                tran.Rollback();
                temp = false;
                throw fmEx;
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

        public bool EditQuotationHeader(FMModule fmModule, string frmName, string user)
        {
            bool temp = false;
            string msg = "";
            //if (ValidateValidToAndOldValidTo(out msg))

            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Quotation oldQuotaion = TransportMarketDAL.GetQuotation(this.quotationNo, con, tran);
                if (ValidateValidFromAndValidTo(out msg))
                {
                    Quotation quotationOut = new Quotation();
                    //Get the latest updateVersion
                    this.updateVersion = quotationOut.updateVersion;

                    temp = TransportMarketDAL.EditQuotationHeader(this, con, tran, out quotationOut);
                    #region OLD logging removed
                    //// now form the log entry
                    //DateTime serverDateTime = Logger.GetServerDateTime();

                    //// form the header   
                    ////20131226 - Gerry replaced child identifier to empty string
                    //LogHeader logHeader = new LogHeader(fmModule, frmName, serverDateTime,
                    //    quotationNo.ToString(), string.Empty, FormMode.Edit, user);

                    //// This is an example of create the 2 logDetail objects
                    ////20131226 - gerry replaced the prop name to constant string instead of hard coded strings
                    //LogDetail logDetail1 = new LogDetail(LOGDETAIL_SALESMAN, salesman);
                    //LogDetail logDetail2 = new LogDetail(LOGDETAIL_VALIDFROM, validFrom.ToShortDateString());
                    //LogDetail logDetail3 = new LogDetail(LOGDETAIL_VALIDTO, validTo.ToShortDateString());
                    //LogDetail logDetail4 = new LogDetail(LOGDETAIL_DATECHANGED, DateTime.Now.ToString());

                    //// add the 2 logDetails objects to the List collection of logHeader
                    //logHeader.LogDetails.Add(logDetail1);
                    //logHeader.LogDetails.Add(logDetail2);
                    //logHeader.LogDetails.Add(logDetail3);
                    //logHeader.LogDetails.Add(logDetail4);
                    ////20131226 end
                    //// now call the Logger class to write

                    //Logger.WriteLog(logHeader, con, tran);
                    #endregion
                    //20151028 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.quotationNo, "TR", "MK", this.quotationID, user, DateTime.Now, frmName, 0, FormMode.Edit.ToString());
                    auditLog.WriteAuditLog(this, oldQuotaion, con, tran);

                    tran.Commit();
                }
                else
                {
                    //                throw new FMException(ResQuotation.ErrorValidToLaterTanOldValidTo+" "+validTo.ToString()+" "+oldValidTo.ToString());
                    throw new FMException(msg);
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

        public bool DeleteQuotationHeader(FMModule fmModule, string frmName, string user)
        {
            bool temp = false;
            if (CanDeleteQuotationHeader() == true)
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    temp = TransportMarketDAL.DeleteQuotationHeader(this, con, tran);
                    #region OLD logging removed
                    //// now form the log entry
                    //DateTime serverDateTime = Logger.GetServerDateTime();

                    //// form the header
                    ////20131226 - Gerry replaced child identifier to empty string
                    //LogHeader logHeader = new LogHeader(fmModule, frmName, serverDateTime,
                    //    quotationNo.ToString(), string.Empty, FormMode.Delete, user);

                    ////20140217 - Gerry removed logdetails for delete
                    //LogDetail logDetail = new LogDetail(LOGDETAIL_DELETED_DATETIME, serverDateTime.ToString());
                    //logHeader.LogDetails.Add(logDetail);
                    ////20140217 end  

                    //// now call the Logger class to write     
                    //Logger.WriteLog(logHeader, con, tran);
                    #endregion
                    //20151028 - gerry added new audit log
                    AuditLog auditLog = new AuditLog(this.quotationNo, "TR", "MK", this.quotationID, user, DateTime.Now, frmName, 0, FormMode.Delete.ToString());
                    auditLog.WriteAuditLog(null, null, con, tran);
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
            }
            return temp;
        }

        public bool ValidationCheckForTransportRate()
        {
            if (this.transportRates.Count == 0)
            {
                return false;
            }
            else
            {

                return true;
            }

        }

        internal static bool FindCustomerGroupInsideQuotation(CustomerGroup customerGroup)
        {
            if (TransportMarketDAL.FindCustomerGroupInsideQuotation(customerGroup) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool ValidateAllPropertiesInHeader()
        {

            //validate by using property methods
            bool temp = false;
            try
            {
                QuotationType = this.quotationType;
                //if it is for a customer
                if (QuotationType == QuotationType.Customer)
                {
                    CustNo = this.custNo;
                }
                // if quotation is for customerGroup
                else if (QuotationType == QuotationType.CustomerGroup)
                {
                    CustomerGroup = this.customerGroup;
                }
                QuotationDate = this.quotationDate;
                ValidFrom = this.validFrom;
                ValidTo = this.validTo;
                CreditDays = this.creditDays;
                Salesman = this.salesman;
                Conditions = this.conditions;
                PaymentTerms = this.paymentTerms;
                Remarks = this.remarks;

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

        internal bool ValidateTransportRateAndPriceBreaks(TransportRate transportRate)
        {
            bool temp = true;
            temp = transportRate.ValidateAllProperties();

            try
            {
                if (!VerifyIfTransportRateCanbeSave(transportRate))
                    temp = false;
                if (!transportRate.PriceBreakEndChecking())
                    temp = false;
                if (!transportRate.ValidateAllPriceBreaksProperties())
                    temp = false;
                // Reset sequence numbers, in case 1 priceBreak deleted
                if (temp == true)
                {
                    transportRate.ResetSequenceNo();
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

            return temp;
        }

        /// <summary>
        /// 2014-07-25 Zhou Kai adds logic to this function:
        /// The combination of (UOM, NoOfLeg, ChargeID)
        /// can't be duplicated.
        /// </summary>
        /// <param name="transportRate">A TransportRate instance</param>
        /// <returns>true: valid; false: invalid</returns>
        private bool VerifyIfTransportRateCanbeSave(TransportRate transportRate)
        {
            bool temp = true;
            if (this.transportRates.Count > 0)
            {
                for (int i = 0; i < this.transportRates.Count; i++)
                {
                    if (transportRate.IsStopDependent)
                    {
                        if (
                            (transportRate.SequenceNo != this.transportRates[i].SequenceNo) && // means it's a new transportRate
                            (transportRate.StartStop == this.transportRates[i].StartStop) &&  
                            (transportRate.EndStop == this.transportRates[i].EndStop) &&
                            (transportRate.UOM == this.transportRates[i].UOM) &&
                            (transportRate.NoOfLeg == this.transportRates[i].NoOfLeg) && 
                            (transportRate.ChargeID == this.transportRates[i].ChargeID)
                            )
                        {
                            temp = false;
                            //this.transportRates = GetTransportRatesForThisQuotation();
                            throw new FMException(TptResourceBLL.ErrCantEnterDuplicateMovementCharge);
                        }
                    }
                    else if (!transportRate.IsStopDependent)
                    {
                        if (
                            (transportRate.SequenceNo != this.transportRates[i].SequenceNo) &&
                            (transportRate.UOM == this.transportRates[i].UOM) &&
                            (transportRate.ChargeID == this.transportRates[i].ChargeID) &&
                            (transportRate.IsStopDependent == this.TransportRates[i].IsStopDependent) &&
                            (transportRate.NoOfLeg == this.transportRates[i].NoOfLeg)
                            )
                        {
                            temp = false;
                            //this.transportRates = GetTransportRatesForThisQuotation();
                            throw new FMException(TptResourceBLL.ErrCantEnterDuplicateNonMovementCharge);
                        }
                    }

                }
            }
            return temp;
        }

        internal bool ValidateCanDeleteTransportRate(TransportRate transportRate)
        {
            bool temp = true;
            if (transportRate.IsNew == false)
            {
                throw new FMException(TptResourceBLL.ErrCannotDeleteTransportRate);
            }
            return temp;
        }

        internal bool CanDeleteQuotationHeader()
        {
            try
            {
                if (TransportMarketDAL.FindQuotationInsideBooking(this))
                {
                    throw new FMException(TptResourceBLL.ErrCannotDeleteQuotationUsedInBooking);
                }
                else
                {
                    //FIND QUOTATION INSIDE INVOICE
                    return true;
                }
            }
            catch (FMException ex)
            {
                throw;
            }
        }

        internal bool ValidateAtLeastOneTransportRate()
        {
            if (transportRates.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static SortableList<Quotation> GetAllQuotationHeader()
        {
            return TransportMarketDAL.GetAllQuotationHeader();
        }

        public static SortableList<Quotation> GetAllQuotationHeaderExcept(int quotationID)
        {
            return TransportMarketDAL.GetAllQuotationHeaderExcept(quotationID);
        }

        public static Quotation GetAllQuotationHeader(string quotationNo)
        {
            return TransportMarketDAL.GetAllQuotation(quotationNo);
        }

        public static SortableList<Quotation> GetQuotationHeaders(Quotation startQuotation, Quotation endQuotation)
        {
            return TransportMarketDAL.GetQuotationHeaders(startQuotation, endQuotation);
        }

        public SortableList<TransportRate> GetTransportRatesForThisQuotation()
        {
            return TransportMarketDAL.GetTransportRatesAndPriceBreaksBasedOnQuotationID(this);
        }

        public static SortableList<TransportRate> GetTransportRatesBasedOnQuotationNo(string QuotationNo)
        {
            return TransportMarketDAL.GetTransportRatesBasedOnQuotationNo(QuotationNo);
        }



        public static SortableList<TransportRate> GetNotContainerMovementTransportRatesBasedOnQuotationNo(string QuotationNo)
        {
            return TransportMarketDAL.GetNotContainerMovementTransportRatesBasedOnQuotationNo(QuotationNo);
        }

        public void TransferTransportRates(FMModule fmModule, Quotation oldQuotation, string frmName, string user)
        {
            if (this.transportRates.Count == 0)
            {
                if (this.quotationType == QuotationType.Customer)
                {
                    CustomerDTO cust = new CustomerDTO();
                    cust = CustomerDTO.GetCustomerByCustCode(this.custNo);

                    if (cust.CurrencyRates.Count != 0)
                    {
                        bool found = false;
                        for (int i = 0; i < oldQuotation.transportRates.Count; i++)
                        {
                            for (int j = 0; j < cust.CurrencyRates.Count; j++)
                            {
                                if (oldQuotation.transportRates[i].Currency == cust.CurrencyRates[j].CurrencyCode)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found == false)
                        {
                            throw new FMException(TptResourceBLL.ErrCannotCopyTransportRateBecauseCurrCodeNotMatch);
                        }
                    }
                }
                for (int i = 0; i < oldQuotation.transportRates.Count; i++)
                {

                    this.AddTransportRate(fmModule, oldQuotation.transportRates[i], frmName, user);

                    transportRates[i].QuotationID = this.quotationID;
                    transportRates[i].IsNew = true;
                    transportRates[i].UpdateVersion = new byte[8];
                }
            }
            else
            {
                throw new FMException(TptResourceBLL.ErrCannotCopyTransportRateBecauseTransportRateNotEmpty);
            }

        }

        public bool ValidatePeriod()
        {
            bool valid = false;
            ArrayList validPeriods = new ArrayList();
            try
            {
                validPeriods = TransportMarketDAL.GetValidQuotationPeriod(this);
                if (validPeriods.Count == 0)
                    valid = true;
                else
                    valid = false;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }

            return valid;
        }

        #region Removed
        //public bool ValidateValidToAndOldValidTo(out string message)
        //{
        //    bool valid = true;
        //    message = "";
        //    //This method will not be used if we don't allowed to edit previous customer quotation
        //    
        //    /*
        //    SortableList<Quotation> quotations = new SortableList<Quotation>();
        //    quotations = TransportMarketDAL.GetQuotationByCustomer(this.custNo);
        //    for (int i = 0; i < quotations.Count; i++)
        //    {
        //        if (this.validFrom.Date > quotations[i].validTo.Date && quotations[i].isValid)
        //        {
        //            valid = false;
        //            message = TptResourceBLL.ErrValidToOverlapped;
        //        }
        //        else if (this.validTo.Date < quotations[i].validFrom.Date && quotations[i].isValid)
        //        {
        //            valid = false;
        //            message = TptResourceBLL.ErrValidFromLaterTanValidTo;
        //        }
        //    }
        //   */

        //    if (DateTime.Compare(validTo, validFrom) >= 0)
        //    {
        //        valid = false;
        //        message = TptResourceBLL.ErrValidFromLaterTanValidTo;
        //    }

        //    return valid;
        //}
        #endregion

        public bool ValidateNewValidFromAndValidTo(DateTime newValidFrom, DateTime newValidTo, out string msg)
        {
            bool valid = true;
            msg = "";
            if (newValidTo.Date < newValidFrom.Date)
            {
                valid = false;
                msg = TptResourceBLL.ErrValidFromLaterTanValidTo;
            }
            return valid;

        }

        private bool ValidateValidFromAndValidTo(out string message)
        {
            bool valid = true;
            message = "";
            //This method will not be used if we don't allowed to edit previous customer quotation
            #region Removed
            /*
            SortableList<Quotation> quotations = new SortableList<Quotation>();
            quotations = TransportMarketDAL.GetQuotationByCustomer(this.custNo);
            for (int i = 0; i < quotations.Count; i++)
            {
                if (this.validFrom.Date > quotations[i].validTo.Date && quotations[i].isValid)
                {
                    valid = false;
                    message = TptResourceBLL.ErrValidToOverlapped;
                }
                else if (this.validTo.Date < quotations[i].validFrom.Date && quotations[i].isValid)
                {
                    valid = false;
                    message = TptResourceBLL.ErrValidFromLaterTanValidTo;
                }
            }
           */
            #endregion

            if (DateTime.Compare(validTo, validFrom) < 0)
            {
                valid = false;
                message = TptResourceBLL.ErrValidFromLaterTanValidTo;
            }

            return valid;
        }

        #region Not yet implemented
        /*
        private bool ValidateValidFromAndValidTo(out string message)
        {
            bool valid = true;
            message = "";

            SortableList<Quotation> quotations = new SortableList<Quotation>();
            quotations = TransportMarketDAL.GetQuotationByCustomer(this.custNo);
            Quotation latestQuotation = TransportMarketDAL.GetLatestQuotation(this.custNo);
            if (this.validFrom.Date > latestQuotation.validTo.Date)
            {
                if (DateTime.Compare(validTo, validFrom) >= 0)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }

            }
            else
            {
                for (int i = 0; i < quotations.Count; i++)
                {
                    string dateValidFrom = quotations[i].validFrom.ToString();
                    string dateValidTo = quotations[i].validTo.ToString();
                    string quotationNo = "Quotation No:" + quotations[i].quotationNo.ToString() + "\t" + dateValidFrom.Substring(0, 10) + " - " + dateValidTo.Substring(0, 8) + "\n";

                    if (quotations[i].validFrom == latestQuotation.validFrom)
                    {
                        if (this.validTo.Date >= quotations[i].validFrom.Date)
                        {
                            valid = false;
                            message = TptResourceBLL.ErrValidToOverlapped + "\n" + string.Format(quotationNo);
                            message = string.Format(message);
                        }
                    }
                    else
                    {
                        if (this.validFrom.Date <= quotations[i].validTo.Date)
                        {
                            valid = false;
                            message = TptResourceBLL.ErrValidToOverlapped + "\n" + string.Format(quotationNo);
                            message = string.Format(message);
                        }
                    }
                }
            }

            return valid;          
        }
         */
        #endregion

        public static SortableList<Quotation> GetQuotationBasedOnCustNo(string custno, string quotationNo)
        {
            return TransportMarketDAL.GetQuotationBasedOnCustNo(custno, quotationNo);
        }

        public static SortableList<Quotation> GetQuotationBasedOnCustomerGroup(string custgroupcode, string quotationNo)
        {
            return TransportMarketDAL.GetQuotationBasedOnCustomerGroup(custgroupcode, quotationNo);
        }

        public static SortableList<Quotation> GetQuotationBasedOnCustNo(string custno, string quotationNo, DateTime date)
        {
            return TransportMarketDAL.GetQuotationBasedOnCustNo(custno, quotationNo, date);
        }

        public static SortableList<Quotation> GetQuotationBasedOnCustomerGroup(string custgroupcode, string quotationNo, DateTime date)
        {
            return TransportMarketDAL.GetQuotationBasedOnCustomerGroup(custgroupcode, quotationNo, date);
        }

        public static SortableList<TransportRate> GetTruckMovementTransportRatesBasedOnQuotationNoAndStopDependent(string QuotationNo, bool stopDependent)
        {
            return TransportMarketDAL.GetTruckMovementTransportRatesBasedOnQuotationNoAndStopDependent(QuotationNo, stopDependent);
        }

        public static SortableList<TransportRate> GetNonTruckMovementTransportRatesBasedOnQuotationNoAndStopDependent(string QuotationNo, bool stopDependent)
        {
            return TransportMarketDAL.GetNonTruckMovementTransportRatesBasedOnQuotationNoAndStopDependent(QuotationNo, stopDependent);
        }
        //Feb. 25, 2011 - Gerry Added to fetch the latest quotation
        public static SortableList<Quotation> GetLatestQuotationHeader(string startQuotation, string endQuotation, bool latest)
        {
            return TransportMarketDAL.GetLatestQuotationHeader(startQuotation, endQuotation, latest);
        }

        public static List<string> GetLatestQuotationNo(bool latest)
        {
            return TransportMarketDAL.GetQuotationNoForCriteria(latest);
        }
        public static List<string> GetQuotationNoToCopy(string quotationNo)
        {
            return TransportMarketDAL.GetQuotationNoToCopy(quotationNo);
        }

        public static SortableList<TransportRate> GetContainerMovementRatesBasedOnQuotationQuotationIDStopendent(string quotationNo, bool stopDependent)
        {
            return TransportMarketDAL.GetContainerMovementRatesBasedOnQuotationQuotationIDStopendent(quotationNo, stopDependent);
        }

        public static string GetValidQuotationNo(string custCode, DateTime date, out string msg)
        {
            return TransportMarketDAL.GetValidQuotationNo(custCode, date, out msg);
        }

        public ArrayList GetJobsAndInvoicesForQuotation(DateTime newValidTo)
        {
            return TransportMarketDAL.GetJobsAndInvoicesForQuotation(this, newValidTo);
        }

        public ArrayList GetJobsForQuotation()
        {
            return TransportMarketDAL.GetJobsForQuotation(this);
        }
        public static bool HaveTruckMovementTransportRatesForMT_CBM(string quotationNo, out string outMsg)
        {
            return TransportMarketDAL.HaveTruckMovementTransportRatesForMT_CBM(quotationNo, out outMsg);
        }

        public bool ValidateEditDeleteRate()
        {
            ArrayList jobList = new ArrayList();
            jobList = this.GetJobsForQuotation();
            if (jobList.Count > 0)
            {
                string jobs = "";
                for (int i = 0; i < jobList.Count; i++)
                {
                    jobs += jobList[i].ToString() + "\n";
                }
                jobs = string.Format(jobs);
                throw new FMException(TptResourceBLL.WarnCantEditTransportRateWithJobs + "\n" + jobs);
            }
            return true;
        }

        #region "2013-12-24 Zhou Kai adds code blocks"
        public static List<string> GetAllQuotationNumbers()
        {
            List<string> lstAllQuotationNumber = new List<string>();
            SortableList<Quotation> allQuotation = new SortableList<Quotation>();
            allQuotation = Quotation.GetAllQuotationHeader();
            foreach (Quotation q in allQuotation)
            {
                lstAllQuotationNumber.Add(q.QuotationNo);
            }

            return lstAllQuotationNumber;
        }

        // Get all deleted quotation numbers from log tables
        public static List<string> GetAllDeletedQuotationNumbers(Dictionary<string, string> dict)
        {
            return TransportMarketDAL.GetAllDeletedQuotationNumbers(dict);
        }

        // 2014-03-14 Zhou Kai adds function to get all transport rate sequence numbers by QuotationNo
        public static List<string> GetTransportRateSeqNosBasedOnQuotationNo(string QuotationNo)
        {
            List<string> tmpList = new List<string>();
            SortableList<TransportRate> tmpRateList = TransportMarketDAL.GetTransportRatesBasedOnQuotationNo(QuotationNo);
            foreach (TransportRate iterator in tmpRateList)
            {
                tmpList.Add(iterator.SequenceNo.ToString());
            }

            return tmpList;
        }
        // 2014-03-14 Zhou Kai ends

        // 2014-03-12 Zhou Kai adds function to get deleted transport rate by quotation number
        public static List<string> GetAllDeletedTransportRatesByQuotationNo(Dictionary<string, string> dict)
        {
            return TransportMarketDAL.GetAllDeletedTransportRatesByQuotationNo(dict);
        }
        // 2014-03-12 Zhou Kai ends

        public static List<string> GetAllQuotationNosWhichHasDeletedRates(Dictionary<string, string> dict)
        {
            return TransportMarketDAL.GetAllQuotationNosWhichHasDeletedRates(dict);
        }
        #endregion
    }
}
