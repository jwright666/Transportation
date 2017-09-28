using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using TR_LanguageResource.Resources;
using System.Data;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MarketDLL.DAL;


namespace FM.TR_MarketDLL.BLL
{
    public class Charge
    {
        private string chargeCode;
        private string chargeDescription;
        private string uom;
        private string salesAccountCode;
        private bool isContainerMovement;
        private string containerCode;
        private bool isOverweight;
        private bool isTruckMovement;
        private string taxCode;
        private string branchCode;
        private string custVendorType;
        //20130402 Gerry added
        //private bool isInvoiceAmtDependentOnWtVol;
        //private bool isHigherOfActWtVol;
        public SortableList<Uom> validUoms { get; set; }
        //end 20130402
        //20130827 Gerry added for package type
        public string packType { get; set; }
        public InvoiceChargeType invoiceChargeType { get; set; }
        //end 20130402


        public string ChargeCode
        {
            get { return chargeCode; }
            set
            {
                //if (value == "")
                //{
                //    throw new FMException(ResClassCharge.ErrorMissingChargeCode);
                //}
                //else
                //{
                    chargeCode = value;
                //}
            }    
        }

        public string ChargeDescription
        {
            get { return chargeDescription; }
            set
            {
                //if (value == "")
                //{
                //    throw new FMException(ResClassCharge.ErrorMissingChargeDesc);
                //}
                //else
                //{
                    chargeDescription = value;
                //}
            }
        }

        public string UOM
        {
            get { return uom; }
            set {
                        uom = value;
                }

        }

        public string SalesAccountCode
        {
            get { return salesAccountCode; }
            set
            {
                //if (value == "")
                //{
                //    throw new FMException(ResClassCharge.ErrorMissingSalesAccntCode);
                //}
                //else
                //{
                    salesAccountCode = value;
                //}
            }
        }

        public bool IsContainerMovement
        {
            get { return isContainerMovement; }
            set { isContainerMovement = value; }

        }

        public bool IsOverweight
        {
            get { return isOverweight; }
            set { isOverweight = value; }
        }

        public string ContainerCode
        {
            get { return containerCode; }
            set { containerCode = value; }

        }

        public bool IsTruckMovement
        {
            get { return isTruckMovement; }
            set { isTruckMovement = value; }

        }     
     
        public string TaxCode
        {
            get { return taxCode; }
            set { taxCode = value; }
        }

        public string BranchCode
        {
            get { return branchCode; }
            set { branchCode = value; }

        }

        public string CustVendorType
        {
            get { return custVendorType; }
            set { custVendorType = value; }

        }

        //public bool IsInvoiceAmtDependentOnWtVol
        //{
        //    get { return isInvoiceAmtDependentOnWtVol; }
        //    set { isInvoiceAmtDependentOnWtVol = value; }

        //}
        //public bool IsHigherOfActWtVol
        //{
        //    get { return isHigherOfActWtVol; }
        //    set { isHigherOfActWtVol = value; }

        //}


        public Charge()
        {
            this.chargeCode = "";
            this.chargeDescription = "";
            this.uom = "";
            this.salesAccountCode = "";
            this.isContainerMovement = false;
            this.containerCode = "";
            this.isOverweight = false;
            this.isTruckMovement = false;
            this.taxCode = "";
            this.branchCode = "";
            this.custVendorType = "";
            //this.isInvoiceAmtDependentOnWtVol = true;
            //this.isHigherOfActWtVol = false;
            this.validUoms = new SortableList<Uom>();
            this.invoiceChargeType = InvoiceChargeType.Others;
            this.packType = string.Empty;
        }

        public Charge(string chargeCode, string chargeDescription, string uom, string salesAccountCode,
            bool isContainerMovement, string containerCode, bool isOverweight,
            bool isTruckMovement, string taxCode, string branchCode, string custVendorType)
        {
            this.ChargeCode = chargeCode;
            this.ChargeDescription = chargeDescription;
            this.UOM = uom;
            this.SalesAccountCode = salesAccountCode;
            this.IsContainerMovement = isContainerMovement;
            this.ContainerCode = containerCode;
            this.IsOverweight = isOverweight;
            this.IsTruckMovement = isTruckMovement;
            this.taxCode = taxCode;
            this.BranchCode = branchCode;
            this.CustVendorType = custVendorType;
        }

        public static SortableList<Charge> GetAllCharges(string branchCode, string custVendType)
        {
            return ChargeDAL.GetAllCharges(branchCode, custVendType);
        }

        public static Charge GetCharge(string chargeCode, string branchCode, string custVend_Type)
        {
            return ChargeDAL.GetCharge(chargeCode, branchCode, custVend_Type);
        }

        public static Charge GetCharge(string chargeCode, string branchCode)
        {
            return ChargeDAL.GetCharge(chargeCode, branchCode);
        }

        public static SortableList<Charge> GetAllChargesWhichNotContainerMovement()
        {
            return ChargeDAL.GetAllChargesWhichNotContainerMovement();
        }

        public bool ValidationCheck()
        {
            bool temp = true;

            if (branchCode == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrorMissingBranch);
            }
            else if (custVendorType == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrorMissingCustVendorType);
            }
            else if (chargeCode == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrMissingChargeCode);
            }
            else if (chargeDescription == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrMissingChargeDesc);
            }
            else if (uom == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrorMissingUOM);
            }
            else if (salesAccountCode == "")
            {
                temp = false;
                throw new FMException(TptResourceBLL.ErrorMissingSalesAccntCode);
            }

            if (isContainerMovement == true)
            {
                if (containerCode == "")
                {
                    temp = false;
                    throw new FMException(TptResourceBLL.ErrorContainerCodeBlank);
                }
            }
            else
            {
                if (isOverweight == true)
                {
                    temp = false;
                    throw new FMException(TptResourceBLL.ErrorIsOverweight);
                }
            }

            return temp;
        }

        public bool CheckCodeIfNotExist(SortableList<Charge> charges)
        {
            bool temp = true;
            if (charges.Count > 0)
            {
                for (int i = 0; i < charges.Count; i++)
                {
                    if (this.chargeCode == charges[i].chargeCode)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrCantAddDupChargeCode);
                    }
                        //20130406 gerry added
                    else if (this.isTruckMovement && this.validUoms.Count <= 0)
                    {
                        temp = false;
                        throw new FMException(TptResourceBLL.ErrCantSaveTruckMovementChargeWOValidUOM);                        
                    }
                    //20130406 end
                    else
                    {
                        ValidationCheck();
                    }
                }
            }

            return temp;
            
        }

        public static bool IsContainerMovementorNot(string chargecode, string branchCode, string custVend_Type)
        {
            Charge c = new Charge();
            c = ChargeDAL.GetCharge(chargecode,  branchCode, custVend_Type);

            if (c.isContainerMovement == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static SortableList<Charge> GetAllChargesFromSalesCostItem()
        {
            return ChargeDAL.GetAllChargesFromSalesCostItem();
        }

        public static SortableList<Charge> GetAllChargesFromSalesCostItem(string branchCode, string CustVendType)
        {
            return ChargeDAL.GetAllChargesFromSalesCostItem(branchCode, CustVendType);
        }

        public bool AddCharge()
        {
            if(CheckCodeIfNotExist(GetAllCharges(this.branchCode.Trim(), this.custVendorType.Trim())))
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                { 
                    if (this.isTruckMovement && this.validUoms.Count <= 0)
                    {
                        throw new FMException(TptResourceBLL.ErrCantSaveTruckMovementChargeWOValidUOM);
                    }
                    if (ChargeDAL.AddCharge(this, con, tran))
                    {
                        //201300405 gerry added
                        if (this.validUoms.Count > 0)
                        {
                            foreach (Uom tempValidUOM in this.validUoms)
                            {
                                if (IsUOMValidToAdd(tempValidUOM))
                                    tempValidUOM.AddValidUOM(this, con, tran);                                  
                            }
                        }
                        //end 20130405
                    }

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
            return true;

        }

        public bool EditCharge()
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (this.isTruckMovement && this.validUoms.Count <= 0)
                {
                    throw new FMException(TptResourceBLL.ErrCantSaveTruckMovementChargeWOValidUOM);
                }
                //201300405 gerry modify
                foreach (Uom tempUOM in this.validUoms)
                {
                    UomDAL.DeleteValidUOM(this, tempUOM.UomCode, con, tran);
                }

                if (ChargeDAL.EditCharge(this, con, tran))
                {
                    foreach (Uom tempValidUOM in this.validUoms)
                    {
                        if (IsUOMValidToAdd(tempValidUOM))
                        {
                            tempValidUOM.AddValidUOM(this, con, tran);
                        }
                    }
                    //end 20130405                    
                }  
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
            return true;

        }

        public bool DeleteCharge()
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (this.validUoms.Count > 0)
                {
                    foreach (Uom tempUOM in this.validUoms)
                    {
                        if (tempUOM.IsUOMvalidToDelete(this, tempUOM.UomCode))
                            UomDAL.DeleteValidUOM(this, tempUOM.UomCode, con, tran);
                        else
                            throw new FMException(TptResourceBLL.ErrCantDeleteInUsedCharge);
                    }
                }
                ChargeDAL.DeleteCharge(this, con, tran);

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
            return true;

        }

        //Gerry Added get all charges for transport

        public static SortableList<Charge> GetAllTransportCharges()
        {
            return ChargeDAL.GetAllTransportCharges();
        }

        public static SortableList<Charge> GetAllTransportNotMovementChages()
        {
            return ChargeDAL.GetAllTransportNoneMovementCharges();
        }

        public static SortableList<Charge> GetAllTruckMovementCharges(string branchCode, string custVendType)
        {
            return ChargeDAL.GetAllTruckMovementCharges(branchCode, custVendType);
        }

        public static ArrayList ChargeUOMs(string chargeCode)
        {
            return ChargeDAL.ChargeUOMs(chargeCode);
        }        
        //remove valid uom from memory
        public bool DeleteValidUOM(string uomCode)
        {  
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                if (this.validUoms.Count > 0)
                {
                    for (int i = 0; i < this.validUoms.Count; i++)
                    {
                        if (uomCode.Trim().Equals(this.validUoms[i].UomCode.Trim()))
                        {
                            this.validUoms[i].DeleteValidUOM(this, uomCode, con, tran);

                            tran.Commit();
                            this.validUoms.RemoveAt(i);
                        }
                    }
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
                throw new FMException(ex.ToString());
            }
            finally { con.Close(); }

            return true;
        }
        public bool IsUOMValidToAdd(Uom uom)
        {
            bool isValid = false;
            try
            {
                if (this.invoiceChargeType == InvoiceChargeType.DependOnHigherWeightOrVolume)
                {
                    if (uom.UomCode == TruckMovementUOM_WtVol.CBM.ToString() ||
                        uom.UomCode == TruckMovementUOM_WtVol.KGM.ToString() ||
                        uom.UomCode == TruckMovementUOM_WtVol.MT.ToString())
                    {
                        isValid = true;
                    }
                    else
                        throw new FMException(TptResourceBLL.WarnValidUOMForJobTripDependentCharge);

                }
                else
                    isValid = true;

            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return isValid;
        }

    }
}
