using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;
using System.Data.SqlClient;

namespace FM.TR_FMSystemDLL.BLL
{
    public class SpecialJobDetail
    {  
        const string ConstSystemJobRelation = "TR";
        public string jobNo { get; set; }
        public string jobTypeCode { get; set; }
        public string branchCode { get; set; }
        public string shipperCode { get; set; }
        public string shipperName { get; set; }
        public string consigneeCode { get; set; }
        public string consigneeName { get; set; }
        public string overseaAgentCode { get; set; }
        public string overseaAgentName { get; set; }
        public string routingAgentCode { get; set; }
        public string routingAgentName { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string viaCity { get; set; }
        public string portOfLoading {get;set;}
        public string portOfDischarge { get; set; }
        public DateTime flightDate { get; set; }   
        public DateTime arrivalDate { get; set; }    
        public int PCS { get; set; }
        public decimal grossWeight { get; set; }
        public decimal chargeWeight { get; set; }
        public decimal cbm { get; set; }
        public decimal container20 { get; set; }
        public decimal container40 { get; set; }
        public decimal container45 { get; set; }
        public string airLine { get; set; }
        public string coloader { get; set; }
        public string remarks1 { get; set; }
        public string remarks2 { get; set; }
        public string content1 { get; set; }
        public string content2 { get; set; }
        public string content3 { get; set; }
        public string content4 { get; set; }
        public string content5 { get; set; }
        public string feederVesselAlpha { get; set; }
        public string feederVoyageNumber { get; set; }
        public string motherVesselAlpha { get; set; }
        public string motherVoyageNumber { get; set; }
        public string shippingLingCode { get; set; }
        public string remarks3 { get; set; }
        public string remarks4 { get; set; }
        public string remarks5 { get; set; }
        public decimal containerOther { get; set; }
        public string termCode {get;set;}
        public string uomCode {get;set;}
        public string systemJobRelation { get; set; }
        public int jobMonth { get; set; }
        public int jobYear { get; set; }
        public DateTime changeDate { get; set; }
        public DateTime openDateTime { get; set; }   //20130322 - Gerry added

        // 2014-04-08 Zhou Kai adds the two properties
        public string MasterNo { get; set; }
        public string HouseNo { get; set; }
        // 2014-04-08 Zhou Kai ends

        public SpecialJobDetail()
        {
            this.airLine = string.Empty;
            this.arrivalDate = DateUtility.GetSQLDateTimeMinimumValue(); //DateTime.Today;   //gerry replaced default value 
            this.branchCode = string.Empty;
            this.cbm = 0;
            this.chargeWeight = 0;
            this.coloader = string.Empty;
            this.consigneeCode = string.Empty; ;
            this.consigneeName = string.Empty;
            this.container20 = 0;
            this.container40 = 0;
            this.container45 = 0;
            this.containerOther = 0;
            this.content1 = string.Empty ;
            this.content2 = string.Empty;
            this.content3 = string.Empty;
            this.content4 = string.Empty;
            this.content5 = string.Empty;
            this.destination = string.Empty;
            this.feederVesselAlpha = string.Empty;
            this.feederVoyageNumber = string.Empty;
            this.flightDate = DateUtility.GetSQLDateTimeMinimumValue(); //DateTime.Today;   //gerry replaced default value 
            this.grossWeight = 0;
            this.jobNo = string.Empty;
            this.jobTypeCode = string.Empty;
            this.motherVesselAlpha = string.Empty;
            this.motherVoyageNumber = string.Empty;
            this.origin = string.Empty;
            this.overseaAgentCode = string.Empty;
            this.overseaAgentName = string.Empty;
            this.PCS = 0; 
            this.portOfDischarge = string.Empty;
            this.portOfLoading = string.Empty;
            this.remarks1 = string.Empty;
            this.remarks2 = string.Empty;
            this.remarks3 = string.Empty;
            this.remarks4 = string.Empty;
            this.remarks5 = string.Empty; 
            this.routingAgentCode = string.Empty ; 
            this.routingAgentName =string.Empty ;
            this.shipperCode = string.Empty;
            this.shipperName = string.Empty;
            this.shippingLingCode = string.Empty;
            this.termCode = string.Empty;
            this.uomCode = string.Empty;
            this.viaCity = string.Empty;
            this.systemJobRelation = ConstSystemJobRelation;
            this.changeDate = Logger.GetServerDateTime();
            this.openDateTime = DateTime.Now;   //20130322 - Gerry added

            // 2014-04-08 Zhou Kai adds
            this.MasterNo = String.Empty;
            this.HouseNo = String.Empty;
            // 2014-04-09 Zhou Kai ends
        }

        public SpecialJobDetail(string jobNumber, string jobTypeCode, string branchCode, string shipperCode, string shipperName,
                                string consigneeCode, string consigneeName, string overseaAgentCode, string overseaAgentName,
                                string routingAgentCode, string routingAgentName, string origin, string destination, string viaCity,
                                string portOfLoading, string portOfDischarge, DateTime flightDate, DateTime arrivalDate, int PCS,
                                decimal Gross_Weight, decimal Charge_Weight, decimal CBM, decimal container20, decimal container40,
                                decimal container45, string airLine, string coloader, string remarks1, string remarks2, string Content1,
                                string Content2, string Content3, string Content4, string Content5, string feederVesselAlpha,
                                string feederVoyageNumber, string motherVesselAlpha, string motherVoyageNumber, string shippingLingCode,
                                string remarks3, string remarks4, string remarks5, decimal ContainerOther, string Termcoode, string UOMCode,string SystemJobRelation,
                                int jobMonth, int jobYear, DateTime changeDate, DateTime openDateTime //20130322 - gerry added 1 parameter openDateTime
                                /*2014-04-09 Zhou Kai adds*/, string masterNo, string houseNo) 
        {
            this.airLine = airLine;
            this.arrivalDate = arrivalDate;
            this.branchCode = branchCode;
            this.cbm =CBM;
            this.chargeWeight = Charge_Weight;
            this.coloader = coloader;
            this.consigneeCode = consigneeCode;
            this.consigneeName = consigneeName;
            this.container20 = container20;
            this.container40 = container40;
            this.container45 = container45;
            this.containerOther = ContainerOther;
            this.content1 = Content1;
            this.content2 = Content2;
            this.content3 = Content3;
            this.content4 = Content4;
            this.content5 = Content5;
            this.destination = destination;
            this.feederVesselAlpha = feederVesselAlpha;
            this.feederVoyageNumber = feederVoyageNumber;
            this.flightDate = flightDate;
            this.jobNo = jobNumber;
            this.jobTypeCode = jobTypeCode;
            this.motherVesselAlpha = motherVesselAlpha;
            this.motherVoyageNumber = motherVoyageNumber;
            this.origin = origin;
            this.overseaAgentCode = overseaAgentCode;
            this.overseaAgentName = overseaAgentName;
            this.PCS = PCS;
            this.portOfDischarge = portOfDischarge;
            this.portOfLoading = portOfLoading;
            this.remarks1 = remarks1;
            this.remarks2 = remarks2;
            this.remarks3 = remarks3;
            this.remarks4 = remarks4;
            this.remarks5 = remarks5;
            this.routingAgentCode = routingAgentCode;
            this.routingAgentName = routingAgentName;
            this.shipperCode = shipperCode;
            this.shipperName = shipperName;
            this.shippingLingCode = shippingLingCode;
            this.termCode = Termcoode;
            this.uomCode = UOMCode;
            this.viaCity = viaCity;
            this.systemJobRelation = SystemJobRelation;
            this.jobMonth = jobMonth;
            this.jobYear = jobYear;
            this.changeDate = changeDate;
            this.openDateTime = openDateTime;   //20130322 - Gerry added
            // 2014-04-09 Zhou Kai adds
            this.MasterNo = masterNo;
            this.HouseNo = houseNo;
            // 2014-04-09 Zhou Kai ends
        }
       
        public static SpecialJobDetail GetSpecialJobDetail(string jobNumber, string branchCode)
        {
            return SpecialJobDetailDAL.GetSpecialJobDetail(jobNumber, branchCode);
        }

        public static string GetJobTypeCode(string jobNumber, string branchCode)
        {
            try
            {
                return SpecialJobDetailDAL.GetJobTypeCode(jobNumber, branchCode); 
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public bool AddTptJobToSpecialJobDetail(SqlConnection con, SqlTransaction tran)
        {
            try { return SpecialJobDetailDAL.AddTptJobToSpecialJobDetail(this, con, tran); }
            catch (Exception ex) { throw ex; }
        }

        public bool EditTptJobToSpecialJobDetail(SqlConnection con, SqlTransaction tran)
        {
            try { return SpecialJobDetailDAL.EditTptJobToSpecialJobDetail(this, con, tran); }
            catch (Exception ex) { throw ex; }
        }

        public bool DeleteTptJobToSpecialJobDetail(string jobNo, SqlConnection con,
            SqlTransaction tran)
        {
            try
            {
                return SpecialJobDetailDAL.DeleteTptJobToSpecialJobDetail(jobNo, con, tran);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static bool EditSpecialJobDetailGrossWeightVolume(string jobNo, decimal newGrossWeight, decimal newGrossCBM, SqlConnection con,
            SqlTransaction tran)
        {            
            try
            {
                return SpecialJobDetailDAL.EditSpecialJobDetailGrossWeightVolume(jobNo, newGrossWeight, newGrossCBM, con, tran);
            }
            catch (Exception ex) { throw ex; }
        }
    }

}
