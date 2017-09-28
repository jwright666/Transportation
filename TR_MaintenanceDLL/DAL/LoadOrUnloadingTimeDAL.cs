using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.TR_MaintenanceDLL.DAL
{
    public class LoadOrUnloadingTimeDAL
    {
        internal static bool AddLoadingTime(LoadOrUnloadingTime loadingTime, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = @"INSERT INTO TPT_Load_Or_Unloading_Time_Tbl(Weight_From,Weight_To,Volume_From,Volume_To,Estimated_Time)
                                                    VALUES('{0}','{1}','{2}','{3}','{4}') ";
                SQLInstertString = string.Format(SQLInstertString,
                                                loadingTime.weight_From,
                                                loadingTime.weight_To,
                                                loadingTime.vol_From,
                                                loadingTime.vol_To,
                                                loadingTime.estimatedTime);
                SqlCommand cmd = new SqlCommand(SQLInstertString, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : AddLoadingTime. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : AddLoadingTime. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : AddLoadingTime. " + ex.Message); }
            return true;
        }
        internal static bool EditLoadingTime(LoadOrUnloadingTime loadingTime, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLEditString = @"update TPT_Load_Or_Unloading_Time_Tbl
                                            set Weight_From = '{0}',
                                            Weight_To='{1}',
                                            Volume_From='{2}',
                                            Volume_To = '{3}',
                                            Estimated_Time = {4}
                                            where Id = {5} ";
                SQLEditString = string.Format(SQLEditString,
                                                loadingTime.weight_From,
                                                loadingTime.weight_To,
                                                loadingTime.vol_From,
                                                loadingTime.vol_To,
                                                loadingTime.estimatedTime,
                                                loadingTime.id);
                SqlCommand cmd = new SqlCommand(SQLEditString, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : EditLoadingTime. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : EditLoadingTime. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : EditLoadingTime. " + ex.Message); }
            return true;
        }
        internal static bool DeleteLoadingTime(LoadOrUnloadingTime loadingTime, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLDeleteString = @"delete from TPT_Load_Or_Unloading_Time_Tbl where Id = {0} ";
                SQLDeleteString = string.Format(SQLDeleteString, loadingTime.id);
                SqlCommand cmd = new SqlCommand(SQLDeleteString, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : DeleteLoadingTime. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : DeleteLoadingTime. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : DeleteLoadingTime. " + ex.Message); }
            return true;
        }
        private static LoadOrUnloadingTime GetLoadingTime(IDataReader reader)
        {
            LoadOrUnloadingTime loadingTime = new LoadOrUnloadingTime();
            loadingTime.id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"]);
            loadingTime.weight_From = reader["Weight_From"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Weight_From"]);
            loadingTime.weight_To = reader["Weight_To"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Weight_To"]);
            loadingTime.vol_From = reader["Volume_From"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Volume_From"]);
            loadingTime.vol_To = reader["Volume_To"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Volume_To"]);
            loadingTime.estimatedTime = reader["Estimated_Time"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Estimated_Time"]);
            return loadingTime;
        }
        internal static LoadOrUnloadingTime GetLoadingTime(int id, SqlConnection con, SqlTransaction tran)
        {
            LoadOrUnloadingTime loadingTime = new LoadOrUnloadingTime();
            try
            {
                string SQLString = @"select * from TPT_Load_Or_Unloading_Time_Tbl
                                    where Id = '{0}'";

                SQLString = string.Format(SQLString, id);
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    loadingTime = GetLoadingTime(reader);
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetLoadingTime. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetLoadingTime. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : GetLoadingTime. " + ex.Message); }

            return loadingTime;
        }
        internal static LoadOrUnloadingTime GetLoadingTime(double weight, double volume, SqlConnection con, SqlTransaction tran)
        {
            LoadOrUnloadingTime loadingTime = new LoadOrUnloadingTime();
            try
            {
                string SQLString = @"select * from TPT_Load_Or_Unloading_Time_Tbl
                                        where Weight_From <= '{0}' and Weight_To > '{0}'
	                                        and Volume_From <= '{1}' and Volume_To > '{1}'";

                SQLString = string.Format(SQLString, weight, volume);
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    loadingTime = GetLoadingTime(reader);
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetLoadingTime. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetLoadingTime. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : GetLoadingTime. " + ex.Message); }

            return loadingTime;
        }
        internal static SortableList<LoadOrUnloadingTime> GetAllLoadingTime(SqlConnection con, SqlTransaction tran)
        {
            SortableList<LoadOrUnloadingTime> loadingTimes = new SortableList<LoadOrUnloadingTime>();
            string sqlByvehicleNo = string.Empty;
            string sqlByvehicleType = string.Empty;
            string SQLString = "SELECT * FROM TPT_Load_Or_Unloading_Time_Tbl";
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    loadingTimes.Add(GetLoadingTime(reader));
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllLoadingTime. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllLoadingTime. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllLoadingTime. " + ex.Message); }
            return loadingTimes;
        }

    }
}
