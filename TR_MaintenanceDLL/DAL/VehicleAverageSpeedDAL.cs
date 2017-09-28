using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
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
    public class VehicleAverageSpeedDAL
    {
        internal static bool AddAverageSpeed(VehicleAverageSpeed aveSpeed, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = @"INSERT INTO TPT_Vehicle_Ave_Speed_Tbl(Time_From,Time_To,Average_Speed)
                                                    VALUES('{0}','{1}',{2}) ";
                SQLInstertString = string.Format(SQLInstertString,
                                                DateUtility.ConvertDateAndTimeForSQLPurpose(aveSpeed.time_From),
                                                DateUtility.ConvertDateAndTimeForSQLPurpose(aveSpeed.time_To),
                                                aveSpeed.ave_Speed);
                SqlCommand cmd = new SqlCommand(SQLInstertString, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : AddAverageSpeed. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : AddAverageSpeed. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : AddAverageSpeed. " + ex.Message); }
            return true;
        }
        internal static bool EditAverageSpeed(VehicleAverageSpeed aveSpeed, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = @"update TPT_Vehicle_Ave_Speed_Tbl
                                            set Time_From = '{0}',
                                            Time_To = '{1}',
                                            Average_Speed = {2}
                                            where Id = {3}";
                SQLInstertString = string.Format(SQLInstertString,
                                                DateUtility.ConvertDateAndTimeForSQLPurpose(aveSpeed.time_From),
                                                DateUtility.ConvertDateAndTimeForSQLPurpose(aveSpeed.time_To),
                                                aveSpeed.ave_Speed,
                                                aveSpeed.id);
                SqlCommand cmd = new SqlCommand(SQLInstertString, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : EditAverageSpeed. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : EditAverageSpeed. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : EditAverageSpeed. " + ex.Message); }
            return true;
        }
        internal static bool DeleteAverageSpeed(VehicleAverageSpeed aveSpeed, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = @"delete from TPT_Vehicle_Ave_Speed_Tbl where Id = {0}";
                SQLInstertString = string.Format(SQLInstertString, aveSpeed.id);
                SqlCommand cmd = new SqlCommand(SQLInstertString, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : DeleteAverageSpeed. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : DeleteAverageSpeed. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : DeleteAverageSpeed. " + ex.Message); }
            return true;
        }
        internal static bool DeleteAverageSpeed(int id, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLInstertString = @"delete from TPT_Vehicle_Ave_Speed_Tbl where Id = {0}";
                SQLInstertString = string.Format(SQLInstertString, id);
                SqlCommand cmd = new SqlCommand(SQLInstertString, con);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : DeleteAverageSpeed. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : DeleteAverageSpeed. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : DeleteAverageSpeed. " + ex.Message); }
            return true;
        }
        private static VehicleAverageSpeed GetVehicleAveSpeed(IDataReader reader)
        {
            VehicleAverageSpeed aveSpeed = new VehicleAverageSpeed();
            aveSpeed.id = reader["Id"] == DBNull.Value ? 0 : (Int32)reader["Id"];
            aveSpeed.time_From = reader["Time_From"] == DBNull.Value ? DateTime.Now : (DateTime)reader["Time_From"];
            aveSpeed.time_To = reader["Time_To"] == DBNull.Value ? DateTime.Now : (DateTime)reader["Time_To"];
            aveSpeed.ave_Speed = reader["Average_Speed"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Average_Speed"]);

            return aveSpeed;
        }
        internal static VehicleAverageSpeed GetAverageSpeed(DateTime time, SqlConnection con, SqlTransaction tran)
        {
            VehicleAverageSpeed vehicleSpeed = new VehicleAverageSpeed();
            try
            {
                string SQLString = @"select * from TPT_Vehicle_Ave_Speed_Tbl
                                    where Time_From < '{0}' 
                                        and Time_To > '{0}'";

                SQLString = string.Format(SQLString, DateUtility.ConvertDateAndTimeForSQLPurpose(time));
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vehicleSpeed = GetVehicleAveSpeed(reader);
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAverageSpeed. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAverageSpeed. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAverageSpeed. " + ex.Message); }

            return vehicleSpeed;
        }
        internal static VehicleAverageSpeed GetAverageSpeed(int id, SqlConnection con, SqlTransaction tran)
        {
            VehicleAverageSpeed vehicleSpeed = new VehicleAverageSpeed();
            try
            {
                string SQLString = @"select * from TPT_Vehicle_Ave_Speed_Tbl
                                    where Id = '{0}' ";

                SQLString = string.Format(SQLString, id);
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vehicleSpeed = GetVehicleAveSpeed(reader);
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAverageSpeed. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAverageSpeed. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAverageSpeed. " + ex.Message); }

            return vehicleSpeed;
        }
        internal static SortableList<VehicleAverageSpeed> GetAllAverageSpeed()
        {
            SortableList<VehicleAverageSpeed> vehicleSpeeds = new SortableList<VehicleAverageSpeed>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string sqlByvehicleNo = string.Empty;
                string sqlByvehicleType = string.Empty;
                string SQLString = "SELECT * FROM TPT_Vehicle_Ave_Speed_Tbl";
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vehicleSpeeds.Add(GetVehicleAveSpeed(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllAverageSpeed. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllAverageSpeed. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllAverageSpeed. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicleSpeeds;
        }
        internal static SortableList<VehicleAverageSpeed> GetAllAverageSpeed(SqlConnection con, SqlTransaction tran)
        {
            SortableList<VehicleAverageSpeed> vehicleSpeeds = new SortableList<VehicleAverageSpeed>();
            string sqlByvehicleNo = string.Empty;
            string sqlByvehicleType = string.Empty;
            string SQLString = "SELECT * FROM TPT_Vehicle_Ave_Speed_Tbl";
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vehicleSpeeds.Add(GetVehicleAveSpeed(reader));
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllAverageSpeed. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllAverageSpeed. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllAverageSpeed. " + ex.Message); }
            return vehicleSpeeds;
        }
        internal static bool IsTimeRangeExist(VehicleAverageSpeed aveSpeed, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLString = @"select * from TPT_Vehicle_Ave_Speed_Tbl
                                    where ((Time_From <= '{0}' and Time_To > '{0}') OR (Time_From < '{1}' and Time_To >= '{1}'))
                                        AND Id <> {2}";

                SQLString = string.Format(SQLString, DateUtility.ConvertDateAndTimeForSQLPurpose(aveSpeed.time_From)
                                                    , DateUtility.ConvertDateAndTimeForSQLPurpose(aveSpeed.time_To)
                                                    , aveSpeed.id);
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.CommandTimeout = 0;
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    throw new FMException("Time range already exist. ");
                }
                reader.Close();
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : IsTimeRangeExist. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : IsTimeRangeExist. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : IsTimeRangeExist. " + ex.Message); }

            return false;
        }

    }
}
