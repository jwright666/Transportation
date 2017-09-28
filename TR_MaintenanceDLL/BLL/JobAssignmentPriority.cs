using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class JobAssignmentPriority
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int sched_Priority { get; set; }
        public Color priorityColor { get; set; }

        public JobAssignmentPriority()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.sched_Priority = 999;
            this.priorityColor = Color.White;
        }

        public bool AddJobAssignment()
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                retValue = PriorityDAL.AddJobAssignmentPriority(this, cn, tran);

                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }
        public bool AddJobAssignment(SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                retValue = PriorityDAL.AddJobAssignmentPriority(this, cn, tran);

                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }
        public bool EditJobAssignment()
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                retValue = PriorityDAL.EditJobAssignmentPriority(this, cn, tran);

                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }
        public bool DeleteJobAssignment()
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                retValue = PriorityDAL.DeleteJobAssignmentPriority(this, cn, tran);

                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }

        public static bool ReArrangePriorities(List<JobAssignmentPriority> list)
        {
            bool retValue = false;
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            SqlTransaction tran = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    retValue = PriorityDAL.ReArrangePriorities(list[i], cn, tran);
                }
                tran.Commit();
            }
            catch (FMException fmEx) { tran.Rollback(); throw fmEx; }
            catch (Exception ex) { tran.Rollback(); throw new FMException(ex.Message.ToString()); }

            return retValue;
        }

        public static SortableList<JobAssignmentPriority> GetDriverJobAssignments()
        {
            return PriorityDAL.GetAllJobAssignmentPriority();
        }








    }
}
