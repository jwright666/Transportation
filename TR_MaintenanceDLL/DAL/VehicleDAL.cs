using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using TR_LicenseDLL.BLL.Transport;
using TR_LanguageResource.Resources;


namespace FM.TR_MaintenanceDLL.DAL
{
    internal class VehicleDAL
    {
        #region unuse code
        #endregion

        public static Vehicle GetVehicles(IDataReader reader)
        {
            bool isAvaiable;
            if ((string)reader["Is_Available"] == "T")
            {
                isAvaiable = true;
            }
            else
            {
                isAvaiable = false;
            }

            Vehicle vehicle = new Vehicle(
                    (VehicleType)reader["Vehicle_Type"],
                    (string)reader["Make"],
                    (string)reader["Model"],
                    (string)reader["Vehicle_No"],
                    (decimal)reader["Max_weight"],
                    isAvaiable,
                    (decimal)reader["Tare_Weight"],
                    (string)reader["Trailer_Type"],
                    (string)reader["Default_Stop"],
                    (string)reader["Remarks"],
                    (byte[])reader["Update_Version"],
                    (decimal)reader["Vol_Capacity"]);

            //20140620 - gerry added new property, VEHICLE_INSPECTION_DATE_DUE default to 6 month after creation
            vehicle.vehicleInspectionDateDue = reader["VEHICLE_INSPECTION_DATE_DUE"] == DBNull.Value ? DateTime.Today.AddMonths(6) : (DateTime)reader["VEHICLE_INSPECTION_DATE_DUE"];
            vehicle.vehicleInspectionDateCompleted = reader["VEHICLE_INSPECTION_DATE_COMPLETED"] == DBNull.Value ? DateUtility.GetSQLDateTimeMinimumValue() : (DateTime)reader["VEHICLE_INSPECTION_DATE_COMPLETED"];
            //20160414 - gerry added
            if (vehicle.Vtype == VehicleType.Truck)
                vehicle.containerOrTruckSize = ContainerOrTruckSize.GetContainerOrTruckSize(vehicle.Number);
            //20160819 - GERRY added
            vehicle.cargoSpace_id = reader["CARGO_SPACE_ID"] == DBNull.Value ? string.Empty : (string)reader["CARGO_SPACE_ID"];
            //20161015 - GERRY added
            vehicle.cloudTrack_device_id = reader["CT_DEVICE_ID"] == DBNull.Value ? string.Empty : ((int)reader["CT_DEVICE_ID"]).ToString();
            //20170801
            vehicle.isRefrigerated = reader["IsRefrigerated"] == DBNull.Value ? false : ((string)reader["IsRefrigerated"] == "T" ? true : false);

            return vehicle;
        }

        internal static SortableList<Vehicle> GetAllVehicles()
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string sqlByvehicleNo = string.Empty;
                string sqlByvehicleType = string.Empty;
                string SQLString = "SELECT * FROM TPT_Vehicle_Tbl";
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllVehicles. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllVehicles. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllVehicles. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicles;
        }

        internal static SortableList<Vehicle> GetAllAvailableVehiclesForDefault()
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string sqlByvehicleNo = string.Empty;
                string sqlByvehicleType = string.Empty;
                string SQLString = @"SELECT * FROM TPT_Vehicle_Tbl
                                where Is_Available = 'T'
                                and Vehicle_Type <> 2
                                and Vehicle_No not in (select Default_Vehicle from TPT_Driver_Tbl)";
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllAvailableVehiclesForDefault. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllAvailableVehiclesForDefault. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllAvailableVehiclesForDefault. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicles;
        }

        internal static SortableList<Vehicle> GetAllTrailers()
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string sqlByvehicleNo = string.Empty;
                    string sqlByvehicleType = string.Empty;
                    string SQLString = "SELECT * FROM TPT_Vehicle_Tbl";
                    SQLString += " WHERE Vehicle_Type=" + VehicleType.Trailer.GetHashCode().ToString();

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllTrailers. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllTrailers. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllTrailers. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicles;
        }

        internal static SortableList<Vehicle> GetAllTrailersForParticularDate(DateTime scheduleDate)
        {
            SortableList<Vehicle> availableTrailers = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
                    string SQLString = "SELECT * FROM TPT_Vehicle_Tbl";
                    SQLString += " WHERE Vehicle_Type=" + VehicleType.Trailer.GetHashCode().ToString();
                    SQLString += " AND Is_Available= 'T'";
                    SQLString += " and vehicle_no not in (select vehicle_no from TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " where schedule_date='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "')";

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    cn.Close();
                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        //Modified 04 July 2011
                        SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                        //SQLString += " inner join TPT_Vehicle_Tbl";
                        //SQLString += " on TPT_Vehicle_Tbl.Vehicle_No = TPT_TRAILER_LOCATION_TBL.TrailerNo";
                        SQLString += " WHERE TPT_TRAILER_LOCATION_TBL.TrailerNo = '" + CommonUtilities.FormatString(vehicles[i].Number) + "'";
                        SQLString += " ORDER BY StartTime Desc";
                        cmd = new SqlCommand(SQLString, cn);
                        cn.Open();
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            if ((int)reader["TrailerStatus"] != TrailerStatus.Assigned.GetHashCode()
                                && (int)reader["TrailerStatus"] != TrailerStatus.Unavailable.GetHashCode())
                            {
                                availableTrailers.Add(vehicles[i]);
                            }
                        }
                        reader.Close();
                    }
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllTrailersForParticularDate. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllTrailersForParticularDate. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllTrailersForParticularDate. " + ex.Message); }
                finally { cn.Close(); }
            }
            return availableTrailers;
        }

        internal static SortableList<Vehicle> GetAllAvailableTrailers()
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string sqlByvehicleNo = string.Empty;
                string sqlByvehicleType = string.Empty;
                string SQLString = "SELECT * FROM TPT_Vehicle_Tbl";
                SQLString += " WHERE Vehicle_Type=" + VehicleType.Trailer.GetHashCode().ToString();
                SQLString += " AND Is_Available= 'T'";

                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllAvailableTrailers. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllAvailableTrailers. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllAvailableTrailers. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicles;
        }

        internal static SortableList<Vehicle> GetAllAvailableTrailersBasedOnDepartment(DateTime scheduleDate, string deptCode)
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string sqlByvehicleNo = string.Empty;
                string sqlByvehicleType = string.Empty;
                try
                {
                    string SQLString = "SELECT * FROM TPT_Vehicle_Tbl";
                    SQLString += " WHERE Vehicle_Type=" + VehicleType.Trailer.GetHashCode().ToString();
                    SQLString += " AND Is_Available= 'T'";
                    SQLString += " AND vehicle_no in (select vehicle_no from TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " WHERE SCHEDULE_DATE='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    SQLString += " AND TPT_DEPT_CODE='" + deptCode + "')";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    if (cn.State == ConnectionState.Closed) { cn.Open(); }
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllAvailableTrailersBasedOnDepartment. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllAvailableTrailersBasedOnDepartment. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllAvailableTrailersBasedOnDepartment. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicles;
        }

        internal static SortableList<Vehicle> GetAllTrailersAssigned(DateTime scheduleDate)
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string sqlByvehicleNo = string.Empty;
                    string sqlByvehicleType = string.Empty;
                    string SQLString = "SELECT * FROM TPT_Vehicle_Tbl";
                    SQLString += " WHERE Vehicle_Type=" + VehicleType.Trailer.GetHashCode().ToString();
                    SQLString += " AND Is_Available= 'T'";
                    SQLString += " AND vehicle_no in (select vehicle_no from TPT_PLAN_DEPT_VEHICLE_TBL";
                    SQLString += " WHERE SCHEDULE_DATE='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "')";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetAllTrailersAssigned. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetAllTrailersAssigned. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetAllTrailersAssigned. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicles;
        }

        internal static SortableList<Vehicle> GetVehicles(string vehicleNo, string vehicleType)
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                try
                {
                    string sqlFiltering = string.Empty;
                    string SQLString = "SELECT * FROM TPT_Vehicle_Tbl where 1=1 ";
                    if (vehicleNo.Trim() != "")
                        sqlFiltering = " AND Vehicle_No = '" + CommonUtilities.FormatString(vehicleNo) + "'";
                    if (vehicleType.Trim() != "")
                        sqlFiltering = " AND Vehicle_Type = '" + CommonUtilities.FormatString(vehicleType) + "'";

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetVehicles. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetVehicles. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetVehicles. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicles;
        }

        internal static SortableList<Vehicle> GetTrailersForTrailerLocationStatus(TrailerStatus trailerStatus, DateTime scheduleDate)
        {
            SortableList<Vehicle> trailers = new SortableList<Vehicle>();
            trailers = GetAllTrailersForParticularDate(scheduleDate);
            SortableList<Vehicle> availableTrailers = new SortableList<Vehicle>();

            SortableList<TrailerLocation> trailerlocations = new SortableList<TrailerLocation>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                for (int i = 0; i < trailers.Count; i++)
                {
                    string SQLString = "SELECT TOP 1 * FROM TPT_TRAILER_LOCATION_TBL";
                    SQLString += " inner join TPT_Vehicle_Tbl";
                    SQLString += " on TPT_Vehicle_Tbl.Vehicle_No = TPT_TRAILER_LOCATION_TBL.TrailerNo";
                    SQLString += " WHERE TrailerNo = '" + CommonUtilities.FormatString(trailers[i].Number) + "'";
                    //SQLString += " AND ScheduleDate = '" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "'";
                    //SQLString += " AND TrailerStatus = " + trailerStatus.GetHashCode();
                    SQLString += " ORDER BY StartTime Desc";
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if ((int)reader["TrailerStatus"] != 1 && (int)reader["TrailerStatus"] != 5)
                        {
                            #region Remove
                            //    int temp = (int)reader["TrailerStatus"];
                            //    TrailerStatus status = TrailerStatus.TrailerPark;
                            //    if (temp == 1)
                            //        status = TrailerStatus.Assigned;
                            //    else if (temp == 2)
                            //        status = TrailerStatus.CustomerStuff;
                            //    else if (temp == 3)
                            //        status = TrailerStatus.CustomerUnstuff;
                            //    else if (temp == 4)
                            //        status = TrailerStatus.TrailerPark;
                            //    else if (temp == 5)
                            //        status = TrailerStatus.Unavailable;
                            //    else if (temp == 6)
                            //        status = TrailerStatus.CompleteStuffUnstuff;

                            //    trailers[i].LocationStatus = status;
                            #endregion
                            availableTrailers.Add(trailers[i]);
                        }
                    }
                    reader.Close();
                }
            }
            catch (FMException ex) { throw ex; }
            catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetTrailersForTrailerLocationStatus. " + ex.Message); }
            catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetTrailersForTrailerLocationStatus. " + ex.Message); }
            catch (Exception ex) { throw new FMException("VehicleDAL Error : GetTrailersForTrailerLocationStatus. " + ex.Message); }
            finally { cn.Close(); }
            return availableTrailers;
        }


        internal static Vehicle GetVehicle(string vehicleNo)
        {
            Vehicle vehicle = new Vehicle();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string sqlFiltering = string.Empty;
            string SQLString = "SELECT * FROM TPT_Vehicle_Tbl ";
            SQLString = SQLString + " Where Vehicle_No = '" + CommonUtilities.FormatString(vehicleNo) + "'";

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vehicle = GetVehicles(reader);
            }
            reader.Close();
            cn.Close();
            return vehicle;
        }
        //20130909 - Gerry added 1 parameter  createddate, used for default trailer location start/end date time 
        internal static bool AddVehicle(Vehicle vehicle, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string isAvailable = vehicle.IsAvailable ? "T" : "F";
                int vehcileType = vehicle.Vtype.GetHashCode();
                //20140620 - gerry modified the query to insert inspection dates
                string SQLInstertString = @"INSERT INTO TPT_Vehicle_Tbl(Vehicle_No,Vehicle_Type,Make,Model,Max_Weight,Is_Available,
                                                            Tare_Weight,Trailer_Type,Default_Stop,Remarks,Vol_Capacity,
                                                            VEHICLE_INSPECTION_DATE_DUE, VEHICLE_INSPECTION_DATE_COMPLETED,
                                                            IsRefrigerated) ";
                SQLInstertString += "VALUES('" + CommonUtilities.FormatString(vehicle.Number.Trim()) + "','"
                                               + vehcileType + "','"
                                               + CommonUtilities.FormatString(vehicle.Make.Trim()) + "','"
                                               + CommonUtilities.FormatString(vehicle.Model.Trim()) + "',"
                                               + vehicle.MaximumLadenWeight + ",'"
                                               + isAvailable + "'," +
                                               +vehicle.TareWeight + ",'"
                                               + CommonUtilities.FormatString(vehicle.TrailerType) + "','"
                                               + CommonUtilities.FormatString(vehicle.DefaultStop) + "','"
                                               + CommonUtilities.FormatString(vehicle.Remarks) + "','"
                                               + vehicle.VolCapacity + "','"
                    //20140620 - gerry added
                                               + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(vehicle.vehicleInspectionDateDue) + "','"
                                               + DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(vehicle.vehicleInspectionDateCompleted) + "','"
                                               + CommonUtilities.FormatString(vehicle.isRefrigerated ? "T" : "F") + "'"
                    //20140620 - gerry end
                                               + ")";
                SqlCommand cmd = new SqlCommand(SQLInstertString, cn);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                if (vehicle.Vtype == VehicleType.Trailer)
                {
                    int countTL = 0;
                    TrailerLocation trailerlocation = new TrailerLocation();
                    trailerlocation.TrailerNo = vehicle.Number;
                    trailerlocation.ScheduleDate = DateTime.Today.Date;
                    trailerlocation.ChangeDate = DateTime.Today.Date;
                    trailerlocation.TrailerStatus = TrailerStatus.TrailerPark;
                    trailerlocation.StopCode = vehicle.DefaultStop;
                    trailerlocation.DriverNo = "";
                    trailerlocation.Remarks = "New Trailer";
                    trailerlocation.StartTime = DateTime.Today.Date;
                    trailerlocation.EndTime = DateTime.Today.Date;
                    //trailerlocation.StartTime = createdDate;
                    //trailerlocation.EndTime = createdDate;
                    trailerlocation.StartStop = vehicle.DefaultStop;
                    trailerlocation.ParkStop = vehicle.DefaultStop;
                    trailerlocation.PlanTripNo = "";


                    SQLInstertString = "INSERT INTO TPT_TRAILER_LOCATION_TBL (ScheduleDate,TrailerNo,StartTime,EndTime,DateChange,TrailerStatus,StartStop,StopCode,ParkStopCode,DriverNo,PlanTripNo,Remarks) VALUES ('"
                    + DateUtility.ConvertDateForSQLPurpose(trailerlocation.ScheduleDate.Date);              //Gerry remove time
                    SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.TrailerNo);
                    SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateForSQLPurpose(trailerlocation.StartTime);  //Gerry remove time
                    SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateForSQLPurpose(trailerlocation.EndTime);  //Gerry remove time
                    SQLInstertString += "'" + "," + "'" + DateUtility.ConvertDateForSQLPurpose(DateTime.Now);              //Gerry remove time
                    SQLInstertString += "'" + "," + "'" + trailerlocation.TrailerStatus.GetHashCode().ToString();
                    SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.StartStop);
                    SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.StopCode);
                    SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.ParkStop);
                    SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.DriverNo);
                    SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.PlanTripNo);
                    SQLInstertString += "'" + "," + "'" + CommonUtilities.FormatString(trailerlocation.Remarks) + "')";

                    cmd = new SqlCommand(SQLInstertString, cn);
                    cmd.Transaction = tran;
                    countTL = cmd.ExecuteNonQuery();

                    if (countTL == 0)
                        throw new FMException("No Trailer Location was created, please contact Innosys staff. ");
                }
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException("Fail to Add in AddVehicle\n" + ex.ToString()); }
            return true;
        }

        internal static bool EditVehicle(Vehicle vehicle, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string isAvailable = vehicle.IsAvailable? "T" : "F";
                int vehcileType = vehicle.Vtype.GetHashCode();
                //20140620 - gerry modified the query to update inspection dates
                string sqlEditString = " Update TPT_Vehicle_Tbl set " +
                                       " Vehicle_Type = '" + vehcileType + "'" +
                                       " , Make ='" + CommonUtilities.FormatString(vehicle.Make.Trim()) + "'" +
                                       " , Model='" + CommonUtilities.FormatString(vehicle.Model.Trim()) + "'" +
                                       " , Max_Weight ='" + vehicle.MaximumLadenWeight + "'" +
                                       " , Is_Available = '" + isAvailable + "'" +
                                       " , Tare_Weight = " + vehicle.TareWeight +
                                       " , Trailer_Type = '" + CommonUtilities.FormatString(vehicle.TrailerType) + "'" +
                                       " , Default_Stop = '" + CommonUtilities.FormatString(vehicle.DefaultStop) + "'" +
                                       " , Remarks = '" + CommonUtilities.FormatString(vehicle.Remarks) + "'" +
                                       " , Vol_Capacity = " + vehicle.VolCapacity +
                                       //20140620  - gerry added
                                       " , VEHICLE_INSPECTION_DATE_DUE = '" + DateUtility.ConvertDateForSQLPurpose(vehicle.vehicleInspectionDateDue.Date) + "'" +
                                       " , VEHICLE_INSPECTION_DATE_COMPLETED = '" + DateUtility.ConvertDateForSQLPurpose(vehicle.vehicleInspectionDateCompleted.Date) + "'" +
                                       " , IsRefrigerated= '" + CommonUtilities.FormatString(vehicle.isRefrigerated ? "T" : "F") + "'" +
                                       //20140620  end 
                                       " where vehicle_No ='" + CommonUtilities.FormatString(vehicle.Number.Trim()) + "'";
                SqlCommand cmd = new SqlCommand(sqlEditString, cn);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;

        }

        internal static bool  DeleteVehicle(string vehicleNo, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string sqlDeleteString = " Delete TPT_Vehicle_Tbl where vehicle_No ='" + CommonUtilities.FormatString(vehicleNo) + "'";
                SqlCommand cmd = new SqlCommand(sqlDeleteString, cn);
                cmd.Transaction = tran;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static bool VerifyAddVehicle(string vehicleNo)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            string SQLSearchString = "SELECT * FROM TPT_Vehicle_Tbl where Vehicle_No = '" + CommonUtilities.FormatString(vehicleNo) + "'";
            SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
            DataSet dsSearchResult = new DataSet();
            daSearchCmd.Fill(dsSearchResult);
            cn.Close();
            if (dsSearchResult.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        internal static bool IsVehicleUsedByDriver(Vehicle vehicle)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            string SQLSearchString = "SELECT * FROM TPT_Driver_Tbl where Default_Vehicle = '" + CommonUtilities.FormatString(vehicle.Number) + "'";
            SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
            DataSet dsSearchResult = new DataSet();
            daSearchCmd.Fill(dsSearchResult);
            cn.Close();
            if (dsSearchResult.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        internal static bool IsVehicleUsedByTrailerLocation(Vehicle vehicle)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            string SQLSearchString = "SELECT * FROM TPT_TRAILER_LOCATION_TBL where TrailerStatus <> 4 AND TrailerNo = '" + CommonUtilities.FormatString(vehicle.Number) + "'";
            SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
            DataSet dsSearchResult = new DataSet();
            daSearchCmd.Fill(dsSearchResult);
            cn.Close();
            if (dsSearchResult.Tables[0].Rows.Count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        internal static bool IsVehicleUsedInPlanning(Vehicle vehicle)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            cn.Open();
            //string SQLSearchString = "SELECT * FROM TPT_PLAN_DEPT_VEHICLE_TBL where VEHICLE_NO = '" + CommonUtilities.FormatString(vehicle.Number) + "'";
            string SQLSearchString = @"SELECT * FROM TPT_PLAN_DEPT_VEHICLE_TBL ts
                                        inner join TPT_TRAILER_LOCATION_TBL tl
                                        on ts.VEHICLE_NO = tl.TrailerNo 
                                        where ts.VEHICLE_NO = '{0}'
                                        and tl.TrailerStatus <> 4";
            SQLSearchString = string.Format(SQLSearchString, vehicle.Number);

            SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
            DataSet dsSearchResult = new DataSet();
            daSearchCmd.Fill(dsSearchResult);
            cn.Close();
            if (dsSearchResult.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        internal static SortableList<Vehicle> GetTrailerFromFirstLeg(int jobid, int seqno)
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();

            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = "select c.* from TPT_PLAN_SUBTRIP_JOB_TBL a";
            SQLString += " inner join TPT_PLAN_SUBTRIP_TBL b on";
            SQLString += " b.Plantrip_no = a.PlanTrip_No and";
            SQLString += " b.seq_no = a.seq_no";
            SQLString += " inner join TPT_Vehicle_Tbl c";
            SQLString += " on b.Trailer_No = c.Vehicle_No";
            SQLString += " where a.job_id="+jobid.ToString();
            SQLString += " and a.sequence_no="+seqno.ToString(); 

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vehicles.Add(GetVehicles(reader));
            }
            cn.Close();
            return vehicles;
        }

        //Feb 21, 2011 Gerry Added for Criteria       
        internal static SortableList<Vehicle> GetVehiclesForCriteria(string vehicleNo, VehicleType vehicleType)
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string sqlFiltering = "";
            string SQLString = "SELECT * FROM TPT_Vehicle_Tbl where 1=1";

            if (vehicleNo.Trim() != "")
            {
                if (vehicleType <= 0)
                    sqlFiltering += @" AND Vehicle_No = '{0}'"; 
                else
                    sqlFiltering += @" AND Vehicle_No = '{0}' AND Vehicle_Type = '{1}'";
            }
            else
            {
                if (vehicleType > 0)
                    sqlFiltering += @" AND Vehicle_Type = '{1}'";

                else
                    sqlFiltering = string.Empty;
            }
            SQLString = string.Format(SQLString + sqlFiltering, CommonUtilities.FormatString(vehicleNo), (int)vehicleType);

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vehicles.Add(GetVehicles(reader));
            }
            cn.Close();
            return vehicles;
        }
        //end

        public static SortableList<Vehicle> GetTrailersInParticularDate(DateTime scheduleDate)
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string SQLString = "SELECT * FROM TPT_Vehicle_Tbl";
                SQLString += " WHERE Vehicle_Type=" + VehicleType.Trailer.GetHashCode().ToString();
                SQLString += " AND Is_Available= 'T'";
                SQLString += " and vehicle_no not in (select vehicle_no from TPT_PLAN_DEPT_VEHICLE_TBL";
                SQLString += " where schedule_date='" + DateUtility.ConvertDateForSQLPurpose(scheduleDate) + "')";
                try
                {
                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        vehicles.Add(GetVehicles(reader));
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetTrailersInParticularDate. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetTrailersInParticularDate. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetTrailersInParticularDate. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicles;
        }
        //20141117 - gerry added method to get trailer from previous leg
        internal static Vehicle GetTrailerFromPreviousLeg(int jobID, string containerNo)
        {
            Vehicle vehicle = new Vehicle();

            using (SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                string SQLString = @"select top 1 jt.CONTAINER_NUMBER, v.* from TPT_PLAN_SUBTRIP_JOB_TBL plstj
                                inner join TPT_PLAN_SUBTRIP_TBL plst
                                on plst.Plantrip_no = plstj.PlanTrip_No and plst.seq_no = plstj.seq_no

                                inner join TPT_JOB_DETAIL_CARGO_Tbl jt
                                on jt.job_id = plstj.job_id and jt.sequence_no = plstj.sequence_no
	
	                            inner join TPT_JOB_MAIN_Tbl jm
	                            on jt.JOB_ID = jm.JOB_ID
	
                                inner join TPT_Vehicle_Tbl v
                                on plst.Trailer_No = v.Vehicle_No
                                where jm.JOB_ID = {0}
                                and jt.CONTAINER_NUMBER = '{1}' 
                                and jt.Leg_Group_Member in (select top 1 tempTrip.Leg_Group_Member from TPT_JOB_DETAIL_CARGO_Tbl tempTrip
								                            where tempTrip.STATUS = 4 and tempTrip.CONTAINER_NUMBER = jt.CONTAINER_NUMBER
                                                            order by LEG_GROUP_MEMBER desc)    
							    order by jt.LEG_GROUP_MEMBER desc   ";
                try
                {
                    SQLString = string.Format(SQLString, jobID, CommonUtilities.FormatString(containerNo));

                    SqlCommand cmd = new SqlCommand(SQLString, cn);
                    cn.Open();
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        vehicle = GetVehicles(reader);
                    }
                    reader.Close();
                }
                catch (FMException ex) { throw ex; }
                catch (SqlException ex) { throw new FMException("VehicleDAL Error : GetTrailerFromPreviousLeg. " + ex.Message); }
                catch (InvalidOperationException ex) { throw new FMException("VehicleDAL Error : GetTrailerFromPreviousLeg. " + ex.Message); }
                catch (Exception ex) { throw new FMException("VehicleDAL Error : GetTrailerFromPreviousLeg. " + ex.Message); }
                finally { cn.Close(); }
            }
            return vehicle;
        }
        internal static Vehicle GetTrailerFromPreviousLeg(int jobID, int seqNo)
        {
            Vehicle vehicle = new Vehicle();

            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string SQLString = @"select v.*  from TPT_PLAN_SUBTRIP_JOB_TBL pstj
                                    inner join TPT_JOB_DETAIL_CARGO_Tbl jt
                                    on jt.JOB_ID = pstj.JOB_ID and jt.SEQUENCE_NO = pstj.SEQUENCE_NO

                                    inner join TPT_PLAN_SUBTRIP_TBL pst
                                    on pst.PLANTRIP_NO = pstj.PLANTRIP_NO and pst.SEQ_NO = pstj.SEQ_NO

                                    inner join TPT_Vehicle_Tbl v
                                    on  v.Vehicle_No =pst.Trailer_No and v.Vehicle_Type = 2

                                    where pstj.JOB_ID = {0}
                                    and pstj.SEQUENCE_NO = {1}
								                            ";
            SQLString = string.Format(SQLString, jobID, seqNo);

            SqlCommand cmd = new SqlCommand(SQLString, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                vehicle = GetVehicles(reader);
            }
            cn.Close();
            return vehicle;
        }


        internal static SortableList<Vehicle> GetAllAvailableVehiclesForLicense()
        {
            SortableList<Vehicle> vehicles = new SortableList<Vehicle>();
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string sqlByvehicleNo = string.Empty;
            string sqlByvehicleType = string.Empty;
            string SQLString = @"SELECT * FROM TPT_Vehicle_Tbl
                                where Is_Available = 'T'
                                and Vehicle_Type <> 2 ";
            try
            {
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vehicles.Add(GetVehicles(reader));
                }
                cn.Close();
            }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return vehicles;
        }

        //20130619 - Licensing by vehicle start
        internal static string GetCompanyName()
        {                                                                  
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            string retValue = string.Empty;
            try
            {   
                string query = @"Select * from CRT_Special_Data_Tbl";

                if (cn.State == ConnectionState.Closed) { cn.Open(); }
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = (string)reader["Company_Name"].ToString().Trim();
                }
                reader.Close();
            }
            catch (FMException fmEx)
            {
                throw fmEx;
            }
            catch (SqlException SQLEx)
            {
                throw new FMException(SQLEx.ToString());
            }
            catch (Exception ex)
            {
                throw new FMException(ex.ToString());
            }
            return retValue;
        }
        //this method was tranferred to company license Class
        ///*
    //    internal static bool HasExceedLicense(DateTime appDate)
    //    {
    //        try
    //        {                 
    //            TPTCompany company = TPTCompany.GetCompanyByName(VehicleDAL.GetCompanyName());
    //            if (company != null)
    //            {
    //                int balLicense = company.GetBalLicense(appDate);
    //                if (balLicense < 0)
    //                    throw new FMException(TptResourceBLL.ErrNoOfLicenseExceed);
    //            }
    //            else
    //                throw new FMException(TptResourceBLL.ErrNoValidLicense);
           
    //        }
    //        catch (FMException fmEx)
    //        {
    //            throw fmEx;
    //        }
    //        catch (SqlException SQLEx)
    //        {
    //            throw new FMException(SQLEx.ToString());
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new FMException(ex.ToString());
    //        }
    //        return false;
    //    }
    //     //*/ 

        public static bool IsVehicleNOExisting(string vehicleNO)
        {
            bool isExisting = false;
            string sqlQuery = "SELECT COUNT(VEHICLE_NO) FROM TPT_VEHICLE_Tbl WHERE " +
                "VEHICLE_NO = '{0}'";
            sqlQuery = String.Format(sqlQuery, vehicleNO);
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                if (cn.State != ConnectionState.Open) { cn.Open(); }
                SqlCommand cmd = new SqlCommand(sqlQuery, cn);
                isExisting = ((int)cmd.ExecuteScalar() == 0 ? false : true);
            }
            catch (SqlException se) { throw se;}
            catch (InvalidOperationException ie) { throw ie; }
            finally { cn.Close(); }

            return isExisting;
        }
        //20160820 - gerry added to update shipment id given from 3D app
        internal static bool UpdateCargoSpaceId(Vehicle truck, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TPT_Vehicle_Tbl
                                            set CARGO_SPACE_ID = '{1}' 
                                        WHERE Vehicle_No = '{0}'   
                                         ";

                updateQuery = string.Format(updateQuery, truck.Number, CommonUtilities.FormatString(truck.cargoSpace_id));

                SqlCommand cmd = new SqlCommand(updateQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
        //20161015 - gerry added to update cloud track device id given from baseride API
        internal static bool UpdateCloudTrackDeviceId(Vehicle truck, SqlConnection cn, SqlTransaction tran)
        {
            bool retValue = false;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }
            try
            {
                string updateQuery = @"update TPT_Vehicle_Tbl
                                            set CT_DEVICE_ID = '{1}' 
                                        WHERE Vehicle_No = '{0}'   
                                         ";

                updateQuery = string.Format(updateQuery, truck.Number, CommonUtilities.FormatString(truck.cloudTrack_device_id));

                SqlCommand cmd = new SqlCommand(updateQuery, cn);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.Message.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.Message.ToString()); }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
    }
}
