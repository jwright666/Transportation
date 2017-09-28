using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using TR_LanguageResource.Resources;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MarketDLL.DAL;

namespace FM.TR_MarketDLL.BLL
{
    public class Uom
    {
        private string uomCode;
        private string uomDescription;
        //20130402 gerry added also in constractors
        private UOM_TYPE uom_type;
        //20130402 end

        public string UomCode
        {
            get { return uomCode; }
        }

        public string UomDescription
        {
            get { return uomDescription; }
        }    
        public UOM_TYPE UOM_Type
        {
            get { return uom_type; }
        }

        public Uom()
        {
            this.uomCode = "";
            this.uomDescription = "";
            this.uom_type = UOM_TYPE.Others;
            
        }

        public Uom(string uomCode, string uomDescription, UOM_TYPE uom_type)
        {
            this.uomCode = uomCode;
            this.uomDescription = uomDescription;
            this.uom_type = uom_type;
        }

        public static Uom GetUomFromFMbyCode(string code)
        {
            return UomDAL.GetUomFromFMbyCode(code);
        }
        public static SortableList<Uom> GetAllUoms()
        {
            return UomDAL.GetAllUoms();
        }

        public static List<string> GetUOMs()
        {
            return UomDAL.GetAllUOMs();
        }

        public static SortableList<Uom> GetValidTRUomsByChargeCode(Charge chargeCode)
        {
            return UomDAL.GetValidTRUomForChargeCode(chargeCode);
        }

        public bool AddValidUOM(Charge charge, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = true;
            if (con.State == ConnectionState.Closed) { con.Open(); }
            try
            {
                UomDAL.AddValidUOM(charge, this, con, tran);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }
        public bool DeleteValidUOM(Charge charge, string uomCode, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = true;
            if (con.State == ConnectionState.Closed) { con.Open(); }
            try
            {
                if (IsUOMvalidToDelete(charge, uomCode))
                    UomDAL.DeleteValidUOM(charge, uomCode, con, tran);
                else
                    throw new FMException(TptResourceBLL.ErrCantDeleteInUsedCharge);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }

        public bool IsUOMvalidToDelete(Charge charge, string uomCode)
        {
            return UomDAL.IsUomValidToDelete(charge, uomCode);
        }
    }
}
