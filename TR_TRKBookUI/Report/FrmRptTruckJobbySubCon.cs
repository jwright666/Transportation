using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FM.FMSystem.BLL;
using FormBaseLibrary;
using TR_LanguageResource.Resources;
using FM.TruckBook.BLL;
using FM.TruckBook.DAL;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace FM.TruckBook.UI
{
    public partial class FrmRptTruckJobbySubCon : AbstractForm
    {
        /*
         * 2013-10-14 Zhou Kai adds comments:
         * (1) FrmRptTruckJobbySubCon is to display and print the truck jobs by sub-contractor
         * (2) Because the current database don't have a table just for this report, we 
         *     need to use complex logic to select and check to get the info the report needs,
         *     so we add a new table TRK_JOB_BY_SUBCON_REPORT_Tbl
         * (3) When the user fill in the criteria and click button "Preview" ,
         *     the C# code will clear the data in TRK_JOB_BY_SUBCON_REPORT_Tbl,
         *     and then fill in with selected data
         * (4) The report control will then display the data in TRK_JOB_BY_SUBCON_REPORT_Tbl
         */

        public FrmRptTruckJobbySubCon()
        {
            InitializeComponent();
        }

        private void FrmRptTruckJobbySubCon_Load(object sender, EventArgs e)
        {
            // this.reportViewer1.RefreshReport();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            // get the creteria and select the data
            DateTime dtStart = dtpStart.Value;
            DateTime dtEnd = dtpTo.Value;
            if (dtStart > dtEnd) { MessageBox.Show(TptResourceUI.StartTimeLaterThanEndTime); return; }

            try { if (DeleteAllDataFromTRK_JOB_BY_SUBCON_REPORT_Tbl()) { UpdateTRKJobsbySubCon(dtpStart.Value, dtpTo.Value); } }
            catch (SqlException se) { throw se; }

            // 2013-10-29 Gerry adds
            ReportParameterInfoCollection parameterInfos = reportViewer1.LocalReport.GetParameters();
            List<ReportParameter> parameters = new List<ReportParameter>();
            if (parameterInfos.Count > 0)
            {
                parameters.Add(new ReportParameter("startDate", dtStart.ToShortDateString(), true));
                parameters.Add(new ReportParameter("endDate", dtEnd.ToShortDateString(), true));
                // Add report parameters
                reportViewer1.LocalReport.SetParameters(parameters);
            }
            // 2013-10-29 Gerry ends
            reportViewer1.RefreshReport();
        }

        private bool UpdateTRKJobsbySubCon(DateTime start_date, DateTime end_date)
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FM.FMSystem.DAL.FMGlobalSettings.TheInstance.getConnectionString());
            if (cn.State != ConnectionState.Open) { cn.Open(); }
            // 2013-11-08 Zhou Kai modifies
            string str_start_date = DateUtility.ConvertDateForSQLPurpose(start_date) + " 00:00";
            string str_end_date = DateUtility.ConvertDateForSQLPurpose(end_date) + " 23:59";
            // 2013-11-08 Zhou Kai ends
            try
            {
                // Get the job id and job trip seq no
                string strSql = "SELECT TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID AS JOB_ID, TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO " +
                                "AS JOBTRIP_SEQ_NO " + 
                                "FROM TRK_JOB_DETAIL_TRIP_Tbl INNER JOIN TRK_JOB_DETAIL_TRIP_SUBCON_Tbl  ON " +
                                "(TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.JOB_ID AND " +
                                "TRK_JOB_DETAIL_TRIP_Tbl.JOBTRIP_SEQ_NO = TRK_JOB_DETAIL_TRIP_SUBCON_Tbl.JOBTRIP_SEQ_NO) " +
                                "INNER JOIN TRK_JOB_MAIN_Tbl ON " +
                                "(TRK_JOB_DETAIL_TRIP_Tbl.JOB_ID = TRK_JOB_MAIN_Tbl.JOB_ID) " +
                                "AND TRK_JOB_MAIN_Tbl.BOOKING_DATE >= @start_date AND TRK_JOB_MAIN_Tbl.BOOKING_DATE <= @end_date AND " +
                                "TRK_JOB_DETAIL_TRIP_Tbl.STATUS > @status";
                                
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(strSql, cn);
                cmd.Parameters.AddWithValue("start_date", str_start_date);
                cmd.Parameters.AddWithValue("end_date", str_end_date);
                cmd.Parameters.AddWithValue("status", (int)TransportBook.BLL.JobTripStatus.Booked);
                SqlDataReader sr = cmd.ExecuteReader();
                List<JobIdJobTripSeq> listJobIdJobTripSeq = new List<JobIdJobTripSeq>();

                while (sr.Read())
                {
                    listJobIdJobTripSeq.Add(new JobIdJobTripSeq(Convert.ToInt32(sr["JOB_ID"]),
                                                                Convert.ToInt32(sr["JOBTRIP_SEQ_NO"])));
                }
                sr.Close();

                // Get full info for each report record
                if (listJobIdJobTripSeq.Count > 0)
                {
                    List<RcdTruckJobTripSubCon> listRcdTruckJobTripSubCon = new List<RcdTruckJobTripSubCon>();
                    foreach (JobIdJobTripSeq jijts in listJobIdJobTripSeq)
                    {
                        listRcdTruckJobTripSubCon.Add(new RcdTruckJobTripSubCon(jijts.jobId, jijts.jobTripSeqNo));
                    }
                    
                    foreach (RcdTruckJobTripSubCon rcdTruckJobSubTripCon in listRcdTruckJobTripSubCon)
                    {

                        adptTRK_JOB_BY_SUBCON_REPORT_Tbl.Insert(rcdTruckJobSubTripCon.jobId,
                                                                rcdTruckJobSubTripCon.seqNo,
                                                                rcdTruckJobSubTripCon.jobNo,
                                                                rcdTruckJobSubTripCon.bookingDate,
                                                                rcdTruckJobSubTripCon.customerNo,
                                                                rcdTruckJobSubTripCon.shipperCode,
                                                                rcdTruckJobSubTripCon.consigneeCode,
                                                                rcdTruckJobSubTripCon.vendorName,
                                                                rcdTruckJobSubTripCon.startStopCode,
                                                                rcdTruckJobSubTripCon.endStopCode,
                                                                rcdTruckJobSubTripCon.AWBBLNo,
                                                                rcdTruckJobSubTripCon.CommercialInvoiceNo,
                                                                rcdTruckJobSubTripCon.cusDeclarationNo,
                                                                rcdTruckJobSubTripCon.NoofPackage,
                                                                rcdTruckJobSubTripCon.NoofPallet,
                                                                rcdTruckJobSubTripCon.kgm,
                                                                rcdTruckJobSubTripCon.cbm,
                                                                rcdTruckJobSubTripCon.pkUptime,
                                                                rcdTruckJobSubTripCon.deliverTime,
                                                                rcdTruckJobSubTripCon.wait_fromTime,
                                                                rcdTruckJobSubTripCon.wait_toTime,
                                                                rcdTruckJobSubTripCon.driverName,
                                                                rcdTruckJobSubTripCon.vechicle,
                                                                rcdTruckJobSubTripCon.reqCusTruckCapacity,
                                                                rcdTruckJobSubTripCon.operTruckCapacity,
                                                                rcdTruckJobSubTripCon.remarks,
                                                                rcdTruckJobSubTripCon.SubConCode);
                    }
                }
                dstRptTruckJobsBySubCon.Clear();
                adptTRK_JOB_BY_SUBCON_REPORT_Tbl.Fill(dstRptTruckJobsBySubCon.TRK_JOB_BY_SUBCON_REPORT_Tbl, 
                                                      Convert.ToDateTime(str_start_date), Convert.ToDateTime(str_end_date));
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally
            {
                cn.Close();
            }
            

            return retValue;
        }

        private bool DeleteAllDataFromTRK_JOB_BY_SUBCON_REPORT_Tbl()
        {
            bool retValue = false;
            string strSql = "DELETE FROM TRK_JOB_BY_SUBCON_REPORT_Tbl";
            SqlConnection cn = new SqlConnection(FM.FMSystem.DAL.FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(strSql, cn);
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { cn.Close(); }
            
            return retValue;
        }

    }

    internal struct JobIdJobTripSeq
    {
        public int jobId;
        public int jobTripSeqNo;

        public JobIdJobTripSeq(int jobId, int jobTripSeqNo)
        {
            this.jobId = jobId;
            this.jobTripSeqNo = jobTripSeqNo;
        }
    }

    internal class RcdTruckJobTripSubCon
    {
        public int jobId { get; set; }
        public DateTime bookingDate{get;set;}
        public string jobNo{get;set;}
        public int seqNo{get;set;}
        public string customerNo{get;set;}
        public string shipperCode{get;set;}
        public string consigneeCode{get;set;}
        public string startStopCode{get;set;}
        public string endStopCode{get;set;}
        public string AWBBLNo{get;set;}
        public string CommercialInvoiceNo { get; set; }
        public string cusDeclarationNo { get; set; }
        public int NoofPackage { get; set; }
        public int NoofPallet { get; set; }
        public decimal kgm { get; set; }
        public decimal cbm { get; set; }
        public DateTime pkUptime { get; set; }
        public DateTime deliverTime { get; set; }
        public DateTime wait_fromTime { get; set; }
        public DateTime wait_toTime { get; set; }
        public string reqCusTruckCapacity { get; set; }
        public string operTruckCapacity { get; set; }
        public string driverName { get; set; }
        public string vechicle { get; set; }
        public string plateNo { get; set; }
        public string vendorName { get; set; }
        public string remarks { get; set; }
        // 2014-03-21 Zhou Kai adds property SubConCode
        public string SubConCode { get; set; }


        public RcdTruckJobTripSubCon(int jobId, int jobTripSeqNo)
        {
            TruckJob truckJob = TruckJob.GetTruckJob(jobId);
            TruckJobTripSubCon truckJobTripSubCon = TruckJobTripSubCon.GetTruckJobTripSubCon(jobId, jobTripSeqNo);
            this.jobId = jobId;
            this.bookingDate = truckJob.BookingDate;
            this.jobNo = truckJob.JobNo;
            this.seqNo = truckJobTripSubCon.Sequence;
            this.customerNo = truckJob.CustNo;
            this.shipperCode = truckJob.shipperCode;
            this.consigneeCode = truckJob.consigneeCode;
            this.startStopCode = truckJobTripSubCon.truckJobTrip.StartStop.Code;
            this.endStopCode = truckJobTripSubCon.truckJobTrip.EndStop.Code;
            // 2013-11-08 Zhou Kai modifies
            this.AWBBLNo = truckJob.AWBNo.Equals(String.Empty) ? truckJob.oblNo :
                                 (truckJob.AWBNo + (truckJob.oblNo.Equals(String.Empty) ?
                                                    String.Empty : ", " + truckJob.oblNo)); ;
            // 2013-11-08 Zhou Kai ends
            this.CommercialInvoiceNo = truckJob.commercialInvoice;
            this.cusDeclarationNo = truckJob.customsDeclaration;
            this.NoofPackage = truckJobTripSubCon.NoPackage;
            this.NoofPallet = truckJobTripSubCon.NoPallets;
            this.kgm = truckJobTripSubCon.truckJobTrip.actualWeight;
            this.cbm = truckJobTripSubCon.truckJobTrip.actualVol;
            this.pkUptime = truckJobTripSubCon.DateTimeFromAct;
            this.deliverTime = truckJobTripSubCon.DateTimeToAct;
            DateTime tmp_wait_from;
            if (DateTime.TryParse(truckJobTripSubCon.WaitTimeFrom, out tmp_wait_from))
            { 
                this.wait_fromTime = tmp_wait_from;
            }
            else 
            { 
                this.wait_fromTime = DateTime.Now; 
            }
            DateTime tmp_wait_to;
            if (DateTime.TryParse(truckJobTripSubCon.WaitTimeTo, out tmp_wait_to))
            { 
                this.wait_toTime = tmp_wait_to; 
            }
            else 
            {
                this.wait_toTime = DateTime.Now;
            }

            this.reqCusTruckCapacity = truckJobTripSubCon.ReqCustTruckCapacity;
            this.operTruckCapacity = truckJobTripSubCon.OperatorTruckCapacity;
            this.driverName = truckJobTripSubCon.Driver;
            this.vechicle = truckJobTripSubCon.Vehicle;
            this.vendorName = truckJob.subcontractorName;
            // 2013-11-08 Zhou Kai modifies, truckJob --> truckJobTripSubCon
            if (!(truckJobTripSubCon.Remarks1.Equals(String.Empty) ||
                truckJobTripSubCon.Remarks2.Equals(String.Empty)))
            {
                this.remarks = truckJobTripSubCon.Remarks1 + "," + truckJobTripSubCon.Remarks2;
            }
            else
            {
                this.remarks = truckJobTripSubCon.Remarks1;
            }

            //  2013-11-08 Zhou Kai ends modifying

            // 2014-03-21 Zhou Kai adds SubConCode
            this.SubConCode = truckJobTripSubCon.SubConCode;
            // 2014-03-21 Zhou Kai ends
        }
    }
}
