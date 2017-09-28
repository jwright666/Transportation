using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;

namespace FM.TR_TRKBookDLL.Facade
{
    public class TransportFacadeOut
    {
        public static void AddTptJobToSpecialJobDetail(TruckJob truckJob,SqlConnection con,
            SqlTransaction tran,string prefix)
        {
            SpecialJobDetail specialJobDetail = new SpecialJobDetail();
            specialJobDetail.jobNo = truckJob.JobNo;
            specialJobDetail.jobTypeCode = prefix;
            specialJobDetail.branchCode = truckJob.BranchCode;
            specialJobDetail.shipperCode = truckJob.shipperCode;
            specialJobDetail.shipperName = truckJob.shipperName;
            specialJobDetail.consigneeCode = truckJob.consigneeCode;
            specialJobDetail.consigneeName = truckJob.consigneeName;
            specialJobDetail.portOfLoading = truckJob.pol;
            specialJobDetail.portOfDischarge = truckJob.pod;
            //specialJobDetail.remarks1 = truckJob.remarks;
            //specialJobDetail.remarks2 = truckJob.remarks2;
            //specialJobDetail.remarks3 = truckJob.remarks3;
            //specialJobDetail.remarks4 = truckJob.remarks4;
            //specialJobDetail.remarks5 = truckJob.remarks5;
            specialJobDetail.content1 = truckJob.remarks;
            specialJobDetail.content2 = truckJob.remarks2;
            specialJobDetail.content3 = truckJob.remarks3;
            specialJobDetail.content4 = truckJob.remarks4;
            specialJobDetail.content5 = truckJob.remarks5;
            specialJobDetail.jobMonth = truckJob.JobDateTime.Month;
            specialJobDetail.jobYear = truckJob.JobDateTime.Year;
            specialJobDetail.changeDate = Logger.GetServerDateTime();
            //specialJobDetail.uomCode = haulierJob.jobTrips

            //20130503 - Gerry added to Flight_Date and Arrival_Date for AM_Special_Job_Detail_Tbl table
            bool isRetreiveFormFM = truckJob.SourceReferenceNo == string.Empty ? false : true;
            if (isRetreiveFormFM)
            {
                //Retrieve from AirFreight
                if (truckJob.SourceReferenceNo[0].ToString() == "A")  //"A" is the first character of the air freight job number
                {
                    specialJobDetail.flightDate = truckJob.flightDate;
                    specialJobDetail.arrivalDate = truckJob.flightDate;
                }
                //add code here for future integration from other module like warehouse....; 
                //for now only Integration from AirFreight
            }
            else
            {   
                //if the job from truck module just set to minimum value
                specialJobDetail.flightDate = DateUtility.GetSQLDateTimeMinimumValue();
                specialJobDetail.arrivalDate = DateUtility.GetSQLDateTimeMinimumValue(); //no trucking field for this so just assign the flight date 
            }
            
            specialJobDetail.openDateTime = truckJob.JobDateTime;  // 20130322 - gerry added

            specialJobDetail.motherVesselAlpha = truckJob.vesselNo;
            specialJobDetail.motherVoyageNumber = truckJob.voyageNo;
            specialJobDetail.grossWeight = truckJob.GetTotalBookedWeight();
            specialJobDetail.cbm = truckJob.GetTotalBookedVolume();
            //end 20121122
            #region 20160408 - gerry replaced code below
            /*
            // 2014-04-09 Zhou Kai adds
            if (truckJob.Freight_Type == FreightType.AF)
            {
                specialJobDetail.MasterNo = truckJob.MAWB;
                specialJobDetail.HouseNo = truckJob.HAWB;
            }
            else if (truckJob.Freight_Type == FreightType.SF)
            {
                specialJobDetail.MasterNo = truckJob.oblNo;
                specialJobDetail.HouseNo = truckJob.hblNo;
            }
            else if (truckJob.Freight_Type == FreightType.LO)
            {
                specialJobDetail.MasterNo = String.Empty;
                specialJobDetail.HouseNo = String.Empty;
            }
            // 2014-04-09 Zhou Kai ends
             * */
            #endregion
            if (truckJob.JobType == Job_Type.LO.ToString())
            {
                specialJobDetail.MasterNo = String.Empty;
                specialJobDetail.HouseNo = String.Empty;
            }
            else if (truckJob.JobType == Job_Type.SE.ToString() || truckJob.JobType == Job_Type.SI.ToString())
            {
                specialJobDetail.MasterNo = truckJob.oblNo;
                specialJobDetail.HouseNo = truckJob.hblNo;
            }
            else if (truckJob.JobType == Job_Type.AE.ToString() || truckJob.JobType == Job_Type.AI.ToString())
            {
                specialJobDetail.MasterNo = truckJob.MAWB;
                specialJobDetail.HouseNo = truckJob.HAWB;
            }

            try { specialJobDetail.AddTptJobToSpecialJobDetail(con, tran); }
            catch (Exception ex) { throw; }
        }

        public static void EditTptJobToSpecialJobDetail(TruckJob truckJob, SqlConnection con,
            SqlTransaction tran, string prefix)
        {
            SpecialJobDetail specialJobDetail = new SpecialJobDetail();
            specialJobDetail = SpecialJobDetail.GetSpecialJobDetail(truckJob.JobNo, truckJob.BranchCode);

            specialJobDetail.jobTypeCode = prefix;
            specialJobDetail.branchCode = truckJob.BranchCode;
            specialJobDetail.shipperCode = truckJob.shipperCode;
            specialJobDetail.shipperName = truckJob.shipperName;
            specialJobDetail.consigneeCode = truckJob.consigneeCode;
            specialJobDetail.consigneeName = truckJob.consigneeName;
            //specialJobDetail.remarks1 = truckJob.remarks;
            //specialJobDetail.remarks2 = truckJob.remarks2;
            //specialJobDetail.remarks3 = truckJob.remarks3;
            //specialJobDetail.remarks4 = truckJob.remarks4;
            //specialJobDetail.remarks5 = truckJob.remarks5;
            specialJobDetail.content1 = truckJob.remarks;
            specialJobDetail.content2 = truckJob.remarks2;
            specialJobDetail.content3 = truckJob.remarks3;
            specialJobDetail.content4 = truckJob.remarks4;
            specialJobDetail.content5 = truckJob.remarks5;
            specialJobDetail.jobMonth = truckJob.JobDateTime.Month;
            specialJobDetail.jobYear = truckJob.JobDateTime.Year;
            specialJobDetail.changeDate = Logger.GetServerDateTime();
            //specialJobDetail.uomCode = haulierJob.jobTrips

            //20130503 - Gerry added to Flight_Date and Arrival_Date for AM_Special_Job_Detail_Tbl table
            bool isRetreiveFormFM = truckJob.SourceReferenceNo == string.Empty ? false : true; 
            if (isRetreiveFormFM)
            {
                //Retrieve from AirFreight
                if (truckJob.SourceReferenceNo[0].ToString() == "A")  //"A" is the first character of the air freight job number
                {
                    specialJobDetail.flightDate = truckJob.flightDate;
                    specialJobDetail.arrivalDate = truckJob.flightDate;                    
                }
                //add code here for future integration from other module like warehouse....; 
                //for now only Integration from AirFreight
            }
            else
            {   //if the job from truck module just set to minimum value
                specialJobDetail.flightDate = DateUtility.GetSQLDateTimeMinimumValue();
                specialJobDetail.arrivalDate = DateUtility.GetSQLDateTimeMinimumValue(); //no trucking field for this so just assign the flight date 
            }
            
            specialJobDetail.openDateTime = truckJob.JobDateTime;    // 20130322 - gerry added  

            specialJobDetail.motherVesselAlpha = truckJob.vesselNo;
            specialJobDetail.motherVoyageNumber = truckJob.voyageNo;
            //end 20121122                                             

            specialJobDetail.grossWeight = truckJob.GetTotalBookedWeight();
            specialJobDetail.cbm = truckJob.GetTotalBookedVolume();
            #region 20160408 - gerry replaced code below
            /*// 2014-04-09 Zhou Kai adds two parameters:
            if (truckJob.Freight_Type == FreightType.AF)
            {
                specialJobDetail.MasterNo = truckJob.MAWB;
                specialJobDetail.HouseNo = truckJob.HAWB;
            }
            else if (truckJob.Freight_Type == FreightType.SF)
            {
                specialJobDetail.MasterNo = truckJob.oblNo;
                specialJobDetail.HouseNo = truckJob.hblNo;
            }
            else if (truckJob.Freight_Type == FreightType.LO)
            {
                specialJobDetail.MasterNo = String.Empty;
                specialJobDetail.HouseNo = String.Empty;
            }
            // 2014-04-09 Zhou Kai ends
            */
            #endregion

            if (truckJob.JobType == Job_Type.LO.ToString())
            {
                specialJobDetail.MasterNo = String.Empty;
                specialJobDetail.HouseNo = String.Empty;
            }
            else if (truckJob.JobType == Job_Type.SE.ToString() || truckJob.JobType == Job_Type.SI.ToString())
            {
                specialJobDetail.MasterNo = truckJob.oblNo;
                specialJobDetail.HouseNo = truckJob.hblNo;
            }
            else if (truckJob.JobType == Job_Type.AE.ToString() || truckJob.JobType == Job_Type.AI.ToString())
            {
                specialJobDetail.MasterNo = truckJob.MAWB;
                specialJobDetail.HouseNo = truckJob.HAWB;
            }
            try { specialJobDetail.EditTptJobToSpecialJobDetail(con, tran); }
            catch (Exception ex) { throw ex; }
        }

        public static void DeleteTptJobToSpecialJobDetail(TruckJob truckJob, SqlConnection con,
            SqlTransaction tran)
        {               
            try
            {
                SpecialJobDetail specialJobDetail = new SpecialJobDetail();
                specialJobDetail = SpecialJobDetail.GetSpecialJobDetail(truckJob.JobNo, truckJob.BranchCode);

                specialJobDetail.DeleteTptJobToSpecialJobDetail(truckJob.JobNo, con, tran);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
