using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
//using FM.TransportBook.Resources;
using FM.TR_HLBookDLL.DAL;
using System.Data.SqlClient;
using TR_LanguageResource.Resources;
using FM.TR_MarketDLL.BLL;

namespace FM.TR_HLBookDLL.BLL
{
    public class HaulierJobCharge:JobCharge
    {
        private string containerNo;

        public string ContainerNo
        {
            get { return containerNo; }
            set { containerNo = value; }
        }

        public HaulierJobCharge()
            : base()
        {
            this.containerNo = "";
        }



        public HaulierJobCharge(int invTrxNo, int invItemSeqNo, int jobId, int sequenceNo, string chargeCode, string chargeDescription, string uom,
            decimal quantity, decimal unitRateFC, decimal totalAmountHC, decimal totalAmountFC,
            string gstType, decimal gstRate, decimal gstAmountHC, string currency, decimal exchangeRate,
            string remarks, string quotationNo, int quotationID, int sequenceNoRate,string jobChargeType,
            DateTime completedDate, JobChargeStatus jobChargeStatus, string rateType,
            byte[] updateVersion, string containerNo)
            : base(invTrxNo, invItemSeqNo, jobId, sequenceNo, chargeCode, chargeDescription, uom, quantity, unitRateFC, totalAmountHC,
            totalAmountFC, gstType, gstRate, gstAmountHC, currency, exchangeRate, remarks, quotationNo,
            quotationID, sequenceNoRate, jobChargeType, completedDate, jobChargeStatus, rateType,updateVersion)
        {
            this.containerNo = containerNo;
        }

        //Gerry Modify this method
        //Allow to add duplication job charge code and UOM but atleast different Description
        public bool ValidateAddHaulierJobCharge(HaulierJob haulierJob, HaulierJobCharge haulierJobCharge)
        {
            SortableList<HaulierJobCharge> haulierJobCharges = new SortableList<HaulierJobCharge>();
            haulierJobCharges = HaulierJobDAL.GetAllHaulierJobCharges(haulierJob);
            bool temp = true;
            string message = "";
            try
            {
                if (ValidateFields(out message))
                {
                    if (QuotationNo == "")
                    {
                        if (haulierJobCharges.Count > 0)
                        {
                            for (int i = 0; i < haulierJobCharges.Count; i++)
                            {
                                if (this.JobID == haulierJobCharges[i].JobID
                                    && this.Uom == haulierJobCharges[i].Uom
                                    && this.ChargeDescription.Trim() == haulierJobCharges[i].ChargeDescription.Trim())
                                {
                                    temp = false;
                                    throw new FMException(CommonResource.ValidateNewJobCharge);
                                }
                            }
                        }
                    }
                    temp = true;
                }
                else
                {
                    temp = false;
                    throw new FMException(message);
                }
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

        public bool ValidateEditHaulierJobCharge()
        {
            bool temp = true;

            if (base.OldJobChargeStatus == JobChargeStatus.Invoiced)
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrCantEdit);
            }
            else
            {
                if (base.QuotationNo != "")
                {
                    transportRate = new TransportRate();
                    transportRate = TransportRate.GetTransportRate(base.QuotationNo, base.ChargeCode, base.SequenceNoRate);

                    //if (totalAmountFC != transportRate.GetTotal(quantity))
                    //{
                    //    temp = false;
                    //    throw new FMException(TptResourceBLL.ErrTotalAmountFCNotCorrect);
                    //}
                }
                else
                {
                    if (base.Currency == "")
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrMissingCurrency);
                    }
                    if (base.UnitRateFC == 0)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrMissingUnitRateFC);
                    }
                    if (base.Quantity == 0)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrorQuantityEmpty);
                    }
                }

            }
            return temp;
        }

        public bool ValidateFields(out string msg)
        {
            bool temp = true;
            msg = "";
            if (Currency == "")
            {
                temp = false;
                msg = TptResourceBLL.ErrMissingCurrency;
            }
            if (UnitRateFC == 0)
            {
                temp = false;
                msg = TptResourceBLL.ErrMissingUnitRateFC;
            }
            if (Quantity == 0)
            {
                temp = false;
                msg = TptResourceBLL.ErrorQuantityEmpty;
            }

            return temp;
        }

        public bool ValidateContainerExists()
        {
            bool temp = true;
            if (containerNo == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrMissingContainerNo);
                
            }
            return temp;
        }

        public static SortableList<HaulierJobCharge> GetAllHaulierJobCharges(HaulierJob haulierJob)
        {
            return HaulierJobDAL.GetAllHaulierJobCharges(haulierJob);
        }

        public static SortableList<HaulierJobCharge> GetInvoiceHaulierJobCharges(HaulierJob haulierJob, SqlConnection cn, SqlTransaction tran)
        {
            return HaulierJobDAL.GetInvoiceHaulierJobCharges(haulierJob, cn, tran);
        }

        //deleted from invoice item
        public static bool SetHaulierJobChargeStatus(HaulierJobCharge hauleirJobCharge, int invTrxNo, int invItemSeqNo, int jobID, bool deleteAll, SqlConnection con, SqlTransaction tran)
        {
            return HaulierJobDAL.SetHaulierJobChargeStatus(hauleirJobCharge, invTrxNo, invItemSeqNo, jobID, deleteAll, con, tran);
        }

        //added to invoice Item
        public static bool SetHaulierJobChargeStatus(HaulierJobCharge haulierJobCharge, SqlConnection con, SqlTransaction tran)
        {
            return HaulierJobDAL.SetHaulierJobChargeStatus(haulierJobCharge, con, tran);
        }

        // 2013-12-24 Zhou Kai adds
        public static List<string> GetAllHaulierJobCharges(string jobNumber)
        {
            return HaulierJobDAL.GetAllHaulierJobChargeSeqNos(jobNumber);
        }
        // 2013-12-24 Zhou Kai ends

        public static List<string> GetAllDeletedJobChargeSeqNos(Dictionary<string, string> dict)
        {
            return HaulierJobDAL.GetAllDeletedJobChargeSeqNos(dict);
        }
    }
}
