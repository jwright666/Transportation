using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_HLBookDLL.BLL;
using FM.TR_TRKBookDLL.BLL;
using System.Data;
using System.Data.SqlClient;
using FM.TR_TRKPlanDLL.DAL;

namespace FM.TR_TRKPlanDLL.BLL
{
    public class PlanTruckSubTripJobDetail 
    {
        public int jobID { get; set; } // TruckJob ID
        public int jobSeq { get; set; } // TruckJob Trip Seq No
        public string planTripNo { get; set; } // The seq no of the planTruckTrip in all planTruckTrips in a day
        public int planSubTripSeqNo { get; set; } // The seq no of the planTruckSubTrip which contains this planTruckSubTripJob within a planTruckTrip
        public int planSubTripJobSeqNo { get; set; } // Seq no of the planTruckSubTripJob
        public int planSubTripJobDetailSeqNo { get; set; } // Seq no of the planTruckSubTripJob
        public string marking { get; set; }
        public string uom { get; set; }
        public int qty { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal unitWeight { get; set; }
        public int actQty { get; set; }
        public string ref_No { get; set; }


        public PlanTruckSubTripJobDetail()
        {
            this.planTripNo = "";
            this.planSubTripSeqNo = 0;
            this.jobID = 0;
            this.jobSeq = 0;
            this.planSubTripJobDetailSeqNo = 0;
            this.marking = "";
            this.uom = "";
            this.qty = 0;
            this.length = 0;
            this.width = 0;
            this.height = 0;
            this.unitWeight = 0;
            this.actQty = 0;
            this.ref_No = string.Empty;
        }

        public static SortableList<PlanTruckSubTripJobDetail> GetPlanTruckSubTripJobDetails(PlanTruckSubTripJob planTruckSubTripJob)
        {
            return PlanTruckDAL.GetPlanTruckSubTripJobDetails(planTruckSubTripJob);
        }

        public bool AddPlanTruckSubTripJobDetail()
        {
            bool retValue = false;
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    tran = con.BeginTransaction();

                    return PlanTruckDAL.AddPlanTruckSubTripJobDetail(this, con, tran);
                }
                catch (FMException ex) { if (tran != null) tran.Rollback(); throw ex; }
                catch (Exception ex) { if (tran != null) tran.Rollback(); throw ex; }
            }
            return retValue;            
        }
        public bool AddPlanTruckSubTripJobDetail(SqlConnection con, SqlTransaction tran)
        {
            try
            {
                return PlanTruckDAL.AddPlanTruckSubTripJobDetail(this, con, tran);
            }
            catch (FMException ex) { if (tran != null) tran.Rollback(); throw ex; }
            catch (Exception ex) { if (tran != null) tran.Rollback(); throw ex; }
            return true;
        }

        public static PlanTruckSubTripJobDetail CreatePlanTruckSubTripJobDetail(int qty, PlanTruckSubTripJob planTruckSubTripJob, TruckJobTripDetail truckJobTripDetail)
        {
            PlanTruckSubTripJobDetail retValue = new PlanTruckSubTripJobDetail();
            try
            {
                retValue.planTripNo = planTruckSubTripJob.planTripNo;
                retValue.planSubTripSeqNo = planTruckSubTripJob.planSubTripSeqNo;
                retValue.planSubTripJobSeqNo = planTruckSubTripJob.planSubTripJobSeqNo;
                retValue.planSubTripJobDetailSeqNo = truckJobTripDetail.detailSequence;
                retValue.jobID = planTruckSubTripJob.jobID;
                retValue.jobSeq = planTruckSubTripJob.jobSeq;
                retValue.marking = truckJobTripDetail.marking;
                retValue.uom = truckJobTripDetail.uom;
                retValue.qty = qty;
                retValue.length = truckJobTripDetail.length;
                retValue.width = truckJobTripDetail.width;
                retValue.height = truckJobTripDetail.height;
                retValue.unitWeight = truckJobTripDetail.unitWeight;
                retValue.ref_No = truckJobTripDetail.ref_No;
                //
                //planTruckSubTripJob.weight = qty * retValue.unitWeight;
                //planTruckSubTripJob.volume = qty * ((retValue.length * retValue.width * retValue.height) / 1000000);
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            return retValue;
        }
    }
}
