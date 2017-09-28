using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class DriverPriority
    {
        public string DriverCode { get; set; }
        public string DriverName { get; set; }
        public string DefaultVehicle { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string Prior_Code { get; set; }
        public string Prior_Desc { get; set; }

        public DriverPriority()
        {
            this.DriverCode = string.Empty;
            this.DriverName = string.Empty;
            this.DefaultVehicle = string.Empty;
            this.ScheduleDate = DateTime.Now;
            this.Prior_Code = string.Empty;
            this.Prior_Desc = string.Empty;
        }

        public bool AddDriverPriority()
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                if (this.HasDriverPriorityForTheDay())
                    PriorityDAL.DeleteDriverPriority(this, cn, tran);

                retValue = PriorityDAL.AddDriverPriority(this, cn, tran);
                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }

        public bool AddDriverPriority(DateTime scheduleDateFrom, DateTime scheduleDateTo)
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                DateTime priorDate = scheduleDateFrom.Date;
                while (priorDate <= scheduleDateTo.Date)
                {
                    this.ScheduleDate = priorDate;
                    if (this.HasDriverPriorityForTheDay())
                        PriorityDAL.DeleteDriverPriority(this, cn, tran);

                    retValue = PriorityDAL.AddDriverPriority(this, cn, tran);
                    priorDate = priorDate.AddDays(1);
                }
                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }
        public bool HasDriverPriorityForTheDay()
        {
            return PriorityDAL.HasDriverPriorityForTheDay(this);
        }
        public static bool AddPriorities(List<Driver> drivers,  DateTime scheduleDateFrom, DateTime scheduleDateTo)
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                DriverPriority driverPriority = new DriverPriority();
                foreach (Driver dr in drivers)
                {
                    driverPriority.DriverCode = dr.Code;
                    driverPriority.DriverName = dr.Name;
                    driverPriority.DefaultVehicle = dr.defaultVehicleNumber;

                    DateTime priorDate = scheduleDateFrom.Date;
                    while (priorDate <= scheduleDateTo.Date)
                    {
                        driverPriority.ScheduleDate = priorDate;
                        if (driverPriority.HasDriverPriorityForTheDay())
                            PriorityDAL.DeleteDriverPriority(driverPriority, cn, tran);

                        retValue = PriorityDAL.AddDriverPriority(driverPriority, cn, tran);
                        priorDate = priorDate.AddDays(1);
                    }

                }
                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }
     

        public bool EditDriverPriority(string newPriorCode)
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                retValue = PriorityDAL.EditDriverPriority(this, newPriorCode, cn, tran);

                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }

        public static SortableList<DriverPriority> GetDriverPriority(string driverCode, DateTime scheduleDate)
        {
            return PriorityDAL.GetAllJobAssignmentPriority(driverCode, scheduleDate);
        }

        public static SortableList<DriverPriority> GetDriverPriority(string driverCode, DateTime scheduleDateFrom, DateTime scheduleDateTo)
        {
            return PriorityDAL.GetAllJobAssignmentPriority(driverCode, scheduleDateFrom, scheduleDateTo);
        }

        public static SortableList<DriverPriority> GetJobAssignmentPriorityByDate(DateTime scheduleDateFrom, DateTime scheduleDateTo)
        {
            return PriorityDAL.GetJobAssignmentPriorityByDate(scheduleDateFrom, scheduleDateTo);
        }
    }
}
