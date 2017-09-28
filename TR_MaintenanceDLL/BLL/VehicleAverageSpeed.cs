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
    public class VehicleAverageSpeed
    {
        public int id { get; set; }
        public DateTime time_From { get; set; }
        public DateTime time_To { get; set; }
        public double ave_Speed { get; set; }

        public VehicleAverageSpeed()
        {
            this.id = 0;
            this.time_From = DateUtility.GetSQLDateTimeMinimumValue();
            this.time_To = DateUtility.GetSQLDateTimeMinimumValue();
            this.ave_Speed = 0;
        }

        public static double GetAveSpeed(DateTime time)
        {
            double retValue = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    retValue = VehicleAverageSpeedDAL.GetAverageSpeed(time, con, tran).ave_Speed;
                    retValue = retValue == 0 ? 50 : retValue; //default speed is 50
                    tran.Rollback();
                }
            }
            catch (FMException fmEx) { throw; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        public static SortableList<VehicleAverageSpeed> GetAveSpeeds()
        {
            SortableList<VehicleAverageSpeed> speeds = new SortableList<VehicleAverageSpeed>();
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    speeds = VehicleAverageSpeedDAL.GetAllAverageSpeed(con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return speeds;
        }

        public static bool AddAverageSpeed(VehicleAverageSpeed aveSpeed)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    if (!IsTimeRangeExist(aveSpeed))
                        VehicleAverageSpeedDAL.AddAverageSpeed(aveSpeed, con, tran);

                    tran.Commit();
                }
                catch (FMException fmEx) { tran.Rollback(); throw; }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                return true;
            }
        }
        public static bool EditAverageSpeed(VehicleAverageSpeed aveSpeed)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    //VehicleAverageSpeed orig = VehicleAverageSpeedDAL.GetAverageSpeed(aveSpeed.id, con, tran);
                    //if (orig == aveSpeed)
                    //{
                    if (!IsTimeRangeExist(aveSpeed))
                        VehicleAverageSpeedDAL.EditAverageSpeed(aveSpeed, con, tran);
                    //}
                    tran.Commit();
                }
                catch (FMException fmEx) { tran.Rollback(); throw; }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                return true;
            }
        }
        public static bool DeleteAverageSpeed(VehicleAverageSpeed aveSpeed)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    VehicleAverageSpeedDAL.DeleteAverageSpeed(aveSpeed, con, tran);
                    tran.Commit();
                }
                catch (FMException fmEx) { tran.Rollback(); throw; }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                return true;
            }
        }
        public static bool IsTimeRangeExist(VehicleAverageSpeed aveSpeed)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    VehicleAverageSpeedDAL.IsTimeRangeExist(aveSpeed, con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException fmEx) { throw; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return false;
        }
    }
}
