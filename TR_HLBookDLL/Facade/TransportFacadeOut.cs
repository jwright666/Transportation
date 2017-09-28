using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_HLBookDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;

namespace FM.TR_HLBookDLL.Facade
{
    public class TransportFacadeOut
    {
        public const string TWENTYFOOT = "20'";
        public const string FORTYFOOT = "40'";
        public const string FORTYFIVEFOOT = "45'";
        public const string OTHERSIZE = "OTH";

        public static void AddTptJobToSpecialJobDetail(HaulierJob haulierJob,SqlConnection con,
            SqlTransaction tran, string prefix, Dictionary<string, Decimal> dictContainerQty)
        {
            SpecialJobDetail specialJobDetail = new SpecialJobDetail();
            specialJobDetail.jobNo = haulierJob.JobNo;
            specialJobDetail.jobTypeCode = prefix;
            specialJobDetail.branchCode = haulierJob.BranchCode;
            specialJobDetail.shipperCode = haulierJob.shipperCode;
            specialJobDetail.shipperName = haulierJob.shipperName;
            specialJobDetail.consigneeCode = haulierJob.consigneeCode;
            specialJobDetail.consigneeName = haulierJob.consigneeName;
            specialJobDetail.portOfLoading = haulierJob.POL;
            specialJobDetail.portOfDischarge = haulierJob.POD;
            specialJobDetail.remarks1 = haulierJob.remarks;
            specialJobDetail.remarks2 = haulierJob.remarks2;
            specialJobDetail.remarks3 = haulierJob.remarks3;
            specialJobDetail.remarks4 = haulierJob.remarks4;
            specialJobDetail.remarks5 = haulierJob.remarks5;
            specialJobDetail.jobMonth = haulierJob.JobDateTime.Month;
            specialJobDetail.jobYear = haulierJob.JobDateTime.Year;
            specialJobDetail.changeDate = Logger.GetServerDateTime();
            //specialJobDetail.uomCode = haulierJob.jobTrips

            //20121122 - Gerry added to Flight_Date and Arrival_Date for AM_Special_Job_Detail_Tbl table
            specialJobDetail.flightDate = haulierJob.JobDateTime;
            specialJobDetail.arrivalDate = haulierJob.JobDateTime;

            specialJobDetail.motherVesselAlpha = haulierJob.VesselNo;
            specialJobDetail.motherVoyageNumber = haulierJob.voyageNo;
            //end 20121122
            //20170726 - gerry added
            specialJobDetail.container20 = dictContainerQty[TWENTYFOOT];
            specialJobDetail.container40 = dictContainerQty[FORTYFOOT];
            specialJobDetail.container45 = dictContainerQty[FORTYFIVEFOOT];
            specialJobDetail.containerOther = dictContainerQty[OTHERSIZE];
            //end 20170726
            try
            {
                specialJobDetail.AddTptJobToSpecialJobDetail(con, tran);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.Message.ToString());
            }
        }

        public static void EditTptJobToSpecialJobDetail(HaulierJob haulierJob, SqlConnection con,
            SqlTransaction tran, string prefix, Dictionary<string, Decimal> dictContainerQty)
        {

            SpecialJobDetail specialJobDetail = new SpecialJobDetail();
            specialJobDetail = SpecialJobDetail.GetSpecialJobDetail(haulierJob.JobNo, haulierJob.BranchCode); 

            specialJobDetail.jobTypeCode = prefix;
            specialJobDetail.branchCode = haulierJob.BranchCode;
            specialJobDetail.shipperCode = haulierJob.shipperCode;
            specialJobDetail.shipperName = haulierJob.shipperName;
            specialJobDetail.consigneeCode = haulierJob.consigneeCode;
            specialJobDetail.consigneeName = haulierJob.consigneeName;
            specialJobDetail.remarks1 = haulierJob.remarks;
            specialJobDetail.remarks2 = haulierJob.remarks2;
            specialJobDetail.remarks3 = haulierJob.remarks3;
            specialJobDetail.remarks4 = haulierJob.remarks4;
            specialJobDetail.remarks5 = haulierJob.remarks5;
            specialJobDetail.jobMonth = haulierJob.JobDateTime.Month;
            specialJobDetail.jobYear = haulierJob.JobDateTime.Year;
            specialJobDetail.changeDate = Logger.GetServerDateTime();
            //specialJobDetail.uomCode = haulierJob.jobTrips

            //20121122 - Gerry added to Flight_Date and Arrival_Date for AM_Special_Job_Detail_Tbl table
            specialJobDetail.flightDate = haulierJob.JobDateTime;
            specialJobDetail.arrivalDate = haulierJob.JobDateTime;          

            specialJobDetail.motherVesselAlpha = haulierJob.VesselNo;
            specialJobDetail.motherVoyageNumber = haulierJob.voyageNo;
            //end 20121122
            //20170726 - gerry added
            specialJobDetail.container20 = dictContainerQty[TWENTYFOOT];
            specialJobDetail.container40 = dictContainerQty[FORTYFOOT];
            specialJobDetail.container45 = dictContainerQty[FORTYFIVEFOOT];
            specialJobDetail.containerOther = dictContainerQty[OTHERSIZE];
            //end 20170726
            try
            {
                specialJobDetail.EditTptJobToSpecialJobDetail(con, tran);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.Message.ToString());
            }
        }

        public static void DeleteTptJobToSpecialJobDetail(HaulierJob haulierJob, SqlConnection con,
            SqlTransaction tran)
        {
            try
            {
                SpecialJobDetail specialJobDetail = new SpecialJobDetail();
                specialJobDetail = SpecialJobDetail.GetSpecialJobDetail(haulierJob.JobNo, haulierJob.BranchCode);

                specialJobDetail.DeleteTptJobToSpecialJobDetail(haulierJob.JobNo,con, tran);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException(ex.Message.ToString());
            }
        }


    }
}
