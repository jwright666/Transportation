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
    public class LoadOrUnloadingTime
    {
        public int id { get; set; }
        public double weight_From { get; set; }
        public double weight_To { get; set; }
        public double vol_From { get; set; }
        public double vol_To { get; set; }
        public double estimatedTime { get; set; }

        public LoadOrUnloadingTime() 
        {
            this.id = 0;
            this.weight_From = 0;
            this.weight_To = 0;
            this.vol_From = 0;
            this.vol_To = 0;
            this.estimatedTime = 1;
        }


        public static bool AddLoadOrUnloadingTime(LoadOrUnloadingTime loadingTime)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    LoadOrUnloadingTimeDAL.AddLoadingTime(loadingTime, con, tran);

                    tran.Commit();
                }
                catch (FMException fmEx) { tran.Rollback(); throw; }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                return true;
            }
        }
        public static bool EditLoadOrUnloadingTime(LoadOrUnloadingTime loadingTime)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    LoadOrUnloadingTimeDAL.EditLoadingTime(loadingTime, con, tran);

                    tran.Commit();
                }
                catch (FMException fmEx) { tran.Rollback(); throw; }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                return true;
            }
        }
        public static bool DeleteLoadOrUnloadingTime(LoadOrUnloadingTime loadingTime)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    LoadOrUnloadingTimeDAL.DeleteLoadingTime(loadingTime, con, tran);
                    tran.Commit();
                }
                catch (FMException fmEx) { tran.Rollback(); throw; }
                catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }
                return true;
            }
        }

        public static LoadOrUnloadingTime GetLoadingTime(int id)
        {
            LoadOrUnloadingTime retValue = null;
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    retValue = LoadOrUnloadingTimeDAL.GetLoadingTime(id, con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException fmEx) { throw; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        public static LoadOrUnloadingTime GetLoadingTime(double weight, double volume)
        {
            LoadOrUnloadingTime retValue = null;
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    retValue = LoadOrUnloadingTimeDAL.GetLoadingTime(weight, volume, con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException fmEx) { throw; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        public static LoadOrUnloadingTime GetLoadingTime(double weight, double volume, SqlConnection con, SqlTransaction tran)
        {
            return LoadOrUnloadingTimeDAL.GetLoadingTime(weight, volume, con, tran);
        }
        public static SortableList<LoadOrUnloadingTime> GetAllLoadingOrUnloadingTime()
        {
            SortableList<LoadOrUnloadingTime> speeds = new SortableList<LoadOrUnloadingTime>();
            try
            {
                using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    speeds = LoadOrUnloadingTimeDAL.GetAllLoadingTime(con, tran);
                    tran.Rollback();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return speeds;
        }
    }
}
