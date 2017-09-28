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
    public class PlanTruckSubTripJob : ICloneable
    {
        // 2014-04-01 Zhou Kai adds comments:
        public int jobID { get; set; } // TruckJob ID
        public int jobSeq { get; set; } // TruckJob Trip Seq No
        public string planTripNo { get; set; } // The seq no of the planTruckTrip in all planTruckTrips in a day
        public int planSubTripSeqNo { get; set; } // The seq no of the planTruckSubTrip which contains this planTruckSubTripJob within a planTruckTrip
        public int planSubTripJobSeqNo { get; set; } // Seq no of the planTruckSubTripJob
        public string custCode { get; set; }
        public string custName { get; set; }
        public Stop startStop { get; set; }
        public Stop endStop { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public decimal volume { get; set; }
        public decimal weight { get; set; }
        public byte[] updateVersion { get; set; }
        public JobTripStatus status { get; set; }
        public SortableList<PlanTruckSubTripJobDetail> planTruckSubTripJobDetails { get; set; } // 20160317 gerry added
        public string cloudTrack_JobId { get; set; }//20160908 - gerry added for cloud track

        public PlanTruckSubTripJob()
        {
            this.planTripNo ="";
            this.planSubTripSeqNo=0;
            this.jobID=0;
            this.jobSeq =0;
            this.custCode ="";
            this.custName ="";
            this.startStop = new Stop();
            this.endStop = new Stop();
            this.startDate = DateTime.Today;
            this.endDate = DateTime.Today;
            this.startTime = "0000";
            this.endTime ="0000";
            this.volume=0;
            this.weight =0;
            this.updateVersion= new byte[8];
            this.status = JobTripStatus.Booked;
            this.planSubTripJobSeqNo = 0;
            this.planTruckSubTripJobDetails = new SortableList<PlanTruckSubTripJobDetail>();
        }

        public PlanTruckSubTripJob(string planTripNo, int planTripSeq, int jobID, int jobSeq,
            string custCode, string custName, Stop startStop, Stop endStop, DateTime startDate,
            DateTime endDate, string startTime, string endTime, decimal volume, decimal weight,
            byte[] updateVersion, JobTripStatus status, int jobTripPlanSeq)
        {
            this.planTripNo = planTripNo;
            this.planSubTripSeqNo = planTripSeq;
            this.jobID = jobID;
            this.jobSeq = jobSeq;
            this.custCode = custCode;
            this.custName = custName;
            this.startStop = startStop;
            this.endStop = endStop;
            this.startDate = startDate;
            this.endDate = endDate;
            this.startTime = startTime;
            this.endTime = endTime;
            this.volume = volume;
            this.weight = weight;
            this.updateVersion = updateVersion;
            this.status = status;
            this.planSubTripJobSeqNo = jobTripPlanSeq;
        }

        public object Clone()
        {
            PlanTruckSubTripJob newPlanTruckSubTripJob = new PlanTruckSubTripJob();
            // only 3 properties are created to save time
            newPlanTruckSubTripJob.planTripNo = this.planTripNo;
            newPlanTruckSubTripJob.planSubTripSeqNo = this.planSubTripSeqNo;
            newPlanTruckSubTripJob.jobID = this.jobID;
            newPlanTruckSubTripJob.jobSeq = this.jobSeq;
            newPlanTruckSubTripJob.custCode = this.custCode;
            newPlanTruckSubTripJob.startStop = this.startStop;
            newPlanTruckSubTripJob.endStop = this.endStop;
            newPlanTruckSubTripJob.startDate = this.startDate;
            newPlanTruckSubTripJob.endDate = this.endDate;
            newPlanTruckSubTripJob.startTime = this.startTime;
            newPlanTruckSubTripJob.endTime = this.endTime;
            newPlanTruckSubTripJob.volume = this.volume;
            newPlanTruckSubTripJob.weight = this.weight;
            newPlanTruckSubTripJob.status = this.status;
            newPlanTruckSubTripJob.planSubTripJobSeqNo = this.planSubTripJobSeqNo;
            return newPlanTruckSubTripJob;


        }

        public bool SetJobTripStatus(JobTripStatus jobTripStatus,SqlConnection con,SqlTransaction tran)
        {
            bool set = false;
            try
            {
                this.status = jobTripStatus;
                PlanTruckDAL.SetPlanTruckSubTripJobStatus(this,con,tran);
                    
                set =  true;
            }
            catch (FMException ex)
            {
                set = false;
                throw ex;
            }
            catch (Exception ex)
            {
                set = false;
                throw ex;
            }
            return set;
        }

        public bool SetJobTripStatus(JobTripStatus jobTripStatus)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {

                this.status = jobTripStatus;
                PlanTruckDAL.SetPlanTruckSubTripJobStatus(this, con, tran);
                tran.Commit();
                return true;
            }
            catch (FMException ex)
            {
                tran.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        // 2013-11-14 Zhou Kai adds
        public bool UpdateAcutalStartEndTimeforTruckJobDetailTrip(int jobID, int jobTripSeqNo,
                                                                  DateTime StartDateTime, DateTime EndDateTime,
                                                                  SqlConnection con, SqlTransaction tran)
        {

            return PlanTruckDAL.UpdateAcutalStartEndTimeforTruckJobDetailTrip(jobID, jobTripSeqNo, StartDateTime, EndDateTime, con, tran);
        }
        public bool UpdateAcutalStartEndTimeforTruckJobDetailTrip(int jobID, int jobTripSeqNo,
                                                          DateTime StartDateTime, DateTime EndDateTime)
        {

            return PlanTruckDAL.UpdateAcutalStartEndTimeforTruckJobDetailTrip(jobID, jobTripSeqNo, StartDateTime, EndDateTime);
        }
        
        //20160317 - gerry added
        public static PlanTruckSubTripJob CreatePlanTruckSubTripJob(string planTripNo, int planTruckSubTripSeqNo, TruckJobTrip truckJobTrip, DateTime startTime, DateTime endTime)
        {
            PlanTruckSubTripJob newPlanTruckSubTripJob = new PlanTruckSubTripJob();
            try
            {
                newPlanTruckSubTripJob.planTripNo = planTripNo;
                newPlanTruckSubTripJob.planSubTripSeqNo = planTruckSubTripSeqNo;
                newPlanTruckSubTripJob.jobID = truckJobTrip.JobID;
                newPlanTruckSubTripJob.jobSeq = truckJobTrip.Sequence;
                newPlanTruckSubTripJob.custCode = truckJobTrip.CustomerCode;
                newPlanTruckSubTripJob.custName = truckJobTrip.CustomerName;
                newPlanTruckSubTripJob.startStop = truckJobTrip.StartStop;
                newPlanTruckSubTripJob.endStop = truckJobTrip.EndStop;
                newPlanTruckSubTripJob.startDate = startTime;
                newPlanTruckSubTripJob.endDate = endTime;
                string startHr = startTime.Hour.ToString().Length == 1 ? "0" + startTime.Hour.ToString() : startTime.Hour.ToString();
                string startMin = startTime.Minute.ToString().Length == 1 ? "0" + startTime.Minute.ToString() : startTime.Minute.ToString();
                string strStartTime = string.Concat(startHr, startMin);

                string endHr = endTime.Hour.ToString().Length == 1 ? "0" + endTime.Hour.ToString() : endTime.Hour.ToString();
                string endMin = endTime.Minute.ToString().Length == 1 ? "0" + endTime.Minute.ToString() : endTime.Minute.ToString();
                string strEndTime = string.Concat(endHr, endMin);

                newPlanTruckSubTripJob.startTime = strStartTime;
                newPlanTruckSubTripJob.endTime = strEndTime;
                //Total weight and volume will depend on the cargo details total weight and volume
                //newPlanTruckSubTripJob.volume = actualVolume == 0 ? TruckJobTripPlan.GetUnassignedTruckJobTripVolume(truckJobTrip) : actualVolume;
                //newPlanTruckSubTripJob.weight = actualWeight == 0 ? TruckJobTripPlan.GetUnAssignedTruckJobTripWeight(truckJobTrip) : actualWeight; 
                newPlanTruckSubTripJob.updateVersion = new byte[8];
                newPlanTruckSubTripJob.status = JobTripStatus.Assigned;
                newPlanTruckSubTripJob.planTruckSubTripJobDetails = new SortableList<PlanTruckSubTripJobDetail>();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw ex; }
            return newPlanTruckSubTripJob;
        }
        //20160908 - gerry added for cloud track
        public static bool UpdatePlanTruckSubTripJobCloudtrack_Job_id(PlanTruckSubTrip planSubTrip, PlanTruckSubTripJob planSubTripJob)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    PlanTruckDAL.UpdatePlanTruckSubTripJobCloudtrack_Job_id(planSubTrip, planSubTripJob, con, tran);
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
                finally { con.Close(); }
            }
            return true;
        }


    }


}
