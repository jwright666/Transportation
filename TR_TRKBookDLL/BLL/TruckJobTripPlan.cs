using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_TRKBookDLL.DAL;
using System.Data.SqlClient;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_HLBookDLL.BLL;

namespace FM.TR_TRKBookDLL.BLL
{
    public class TruckJobTripPlan : ICloneable
    {
        public DateTime scheduleDate  { get; set; }
        public string planTripNo  { get; set; }
        public string driver  { get; set; }
        public string truckNo  { get; set; }
        public decimal volume  { get; set; }
        public decimal weight  { get; set; }
        public byte[] updateVersion { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int jobID { get; set; }
        public int sequence { get; set; }
        public JobTripStatus status { get; set; }
        public int planSubTripSeqNo { get; set; }
        public int planSubTripJobSeqNo { get; set; }

        public TruckJobTripPlan()
        {
            this.scheduleDate=DateTime.Today;
            this.planTripNo="";
            this.driver="";
            this.truckNo="";
            this.volume=0;
            this.weight=0;
            this.updateVersion = new byte[8];
            this.start = DateTime.Now;
            this.end = DateTime.Now;
            this.status = JobTripStatus.Assigned;
            this.planSubTripJobSeqNo = 0;
            this.planSubTripSeqNo = 0;
        }

        public TruckJobTripPlan(DateTime scheduleDate, string planTripNo, string driver,
            string truckNo, decimal volume, decimal weight, byte[] updateVersion,
            DateTime start, DateTime end, JobTripStatus status, int jobTripPlanSeq)
        {
            this.scheduleDate = scheduleDate;
            this.planTripNo = planTripNo;
            this.driver = driver;
            this.truckNo = truckNo;
            this.volume = volume;
            this.weight = weight;
            this.updateVersion = updateVersion;
            this.start = start;
            this.end = end;
            this.status = status;
            this.planSubTripJobSeqNo = jobTripPlanSeq;
        }

        public object Clone()
        {
            TruckJobTripPlan newTruckJobTripPlan = new TruckJobTripPlan();
            // only 3 properties are created to save time
            newTruckJobTripPlan.scheduleDate = this.scheduleDate;
            newTruckJobTripPlan.planTripNo = this.planTripNo;
            newTruckJobTripPlan.jobID = this.jobID;
            newTruckJobTripPlan.sequence = this.sequence;
            newTruckJobTripPlan.driver = this.driver;
            newTruckJobTripPlan.truckNo = this.truckNo;
            newTruckJobTripPlan.volume = this.volume;
            newTruckJobTripPlan.weight = this.weight;
            newTruckJobTripPlan.start = this.start;
            newTruckJobTripPlan.end = this.end;
            newTruckJobTripPlan.status = this.status;
          return newTruckJobTripPlan;



        }

        public static bool ChangeDrivers(string planTripNo, Driver newDriver, SqlConnection cn, SqlTransaction tran)
        {
            bool flag = false;
            try
            {
                flag = TruckJobDAL.ChangeDrivers(planTripNo, newDriver, cn, tran);
            }
            catch (FMException ex)
            {
                throw ex;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public static bool ChangeDriver(string planTripNo, int jobId, int jobSeq, int jobtripPlanSeq,string newPlanTripNo, Driver newDriver, SqlConnection cn, SqlTransaction tran)
        {
            bool flag = false;
            try
            {

                flag = TruckJobDAL.ChangeDriver(planTripNo, jobId, jobSeq, jobtripPlanSeq, newPlanTripNo,newDriver, cn, tran);
            }
            catch (FMException ex)
            {
                throw ex;
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
                return flag;
            }


            return flag;
        }

        public bool ChangeWeightVolume(decimal newWeight, decimal newVolume)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                TruckJobDAL.ChangeWeightVolume(newWeight, newVolume, this, con, tran);
                tran.Commit();
                return true;
            }
            catch (FMException ex) { tran.Rollback(); throw ex; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.ToString()); }
        }

        public bool SetJobTripPlanStatus(JobTripStatus status)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                TruckJobDAL.SetJobTripPlanStatus(status,this, con, tran);
                this.status = status;
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

        public bool SetJobTripPlanStatus(JobTripStatus status,SqlConnection con,SqlTransaction tran)
        {

            try
            {
                TruckJobDAL.SetJobTripPlanStatus(status, this, con, tran);
                this.status = status;

                return true;
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SortableList<TruckJobTripPlan> GetTruckTripPlanByDriver(string driverCode, DateTime schedDate, string deptCode)
        {
            return TruckJobDAL.GetTruckJobTripPlanByDriver(driverCode, schedDate,deptCode);
        }

        public static bool UpdateWeightAndVol(TruckJobTripPlan truckJobTripPlan, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {              
                TruckJobDAL.UpdateTruckJobTripPlanWeightVol(truckJobTripPlan, con, tran);
               
                retValue = true;
            }
            catch (FMException fmex) { throw fmex; }
            catch (Exception ex) { throw ex; }       
            return retValue;
        }
        //20160129 - gerry modified query add parameter planSubTripSeqNo
        public static TruckJobTripPlan GetTruckJobTripPlan(int jobid, int jobtripseq,string plantripNo,int planSubTripSeqNo, int jobTripPlanSeq,SqlConnection con,SqlTransaction tran)
        {
            return TruckJobDAL.GetTruckJobTripPlan(jobid, jobtripseq, plantripNo, planSubTripSeqNo, jobTripPlanSeq, con, tran);
        }
        //20151222 - gerry start modify
        public static Decimal GetUnAssignedTruckJobTripWeight(TruckJobTrip jobTrip)
        {
            return TruckJobDAL.GetUnAssignedTruckJobTripWeight(jobTrip);
        }
        //20160420 gerry overload
        public static Decimal GetUnAssignedTruckJobTripWeight(TruckJobTrip jobTrip, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.GetUnAssignedTruckJobTripWeight(jobTrip, con, tran);
        }
        public static Decimal GetUnassignedTruckJobTripVolume(TruckJobTrip jobTrip)
        {
            return TruckJobDAL.GetUnassignedTruckJobTripVolume(jobTrip);
        }
        //20160420 gerry overload
        public static Decimal GetUnassignedTruckJobTripVolume(TruckJobTrip jobTrip, SqlConnection con, SqlTransaction tran)
        {
            return TruckJobDAL.GetUnassignedTruckJobTripVolume(jobTrip, con, tran);
        }

    }
}
