using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using System.Collections;
using TR_LanguageResource.Resources;

namespace FM.TR_MaintenanceDLL.DAL
{
    internal class DriverDAL
    {
        public const string MIN_DAY_TIME = "0000";
        public const string MAX_DAY_TIME = "2359";

        internal static SortableList<Driver> GetAllDriverThatNotInThePlan(DateTime date)
        {
            SortableList<Driver> drivers = new SortableList<Driver>();     
            SortableList<Driver> drivers2 = new SortableList<Driver>();
            try
            {   
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                //20140702 - gerry modified query to include partially unavailable drivers
                string SQLString = "SELECT * FROM TPT_Driver_Tbl";
                SQLString += " WHERE Employ_Status = 'A'";
                SQLString += @" and Driver_Code not in (select Distinct Driver_Code from Tpt_driver_unavailable_tbl where 1=1
                                                            and (DateFrom  = '{0}' and TimeFrom = '{1}' and DateTo = '{0}' and TimeTo = '{2}')
                                                            OR (DateFrom  = '{0}' and TimeFrom = '{1}' and DateTo > '{0}')
                                                            OR (DateFrom  < '{0}' and DateTo > '{0}')
                                                            OR (DateFrom  < '{0}' and DateTo = '{0}' and TimeTo = '{2}') 
                                                        )";
                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(date), MIN_DAY_TIME, MAX_DAY_TIME);

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    drivers.Add(GetDriver(reader));
                }
                cn.Close();
                for (int i = 0; i < drivers.Count; i++)
                {
                    cn.Open();
                    string SQLSearchString = "SELECT * FROM TPT_PLAN_TRIP_TBL ";
                    SQLSearchString += "where DRIVER_NO = '" + CommonUtilities.FormatString(drivers[i].Code) + "'";
                    SQLSearchString += " AND SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
                    SQLSearchString += " AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";
                    SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
                    DataSet dsSearchResult = new DataSet();
                    daSearchCmd.Fill(dsSearchResult);
                    cn.Close();
                    if (dsSearchResult.Tables[0].Rows.Count > 0)
                    {
                    }
                    else
                    {
                        drivers2.Add(drivers[i]);
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }  

            return drivers2;
        }

        internal static SortableList<Driver> GetAllDriverThatNotInBothTruckAndHaulierPlan(DateTime date)
        {
            SortableList<Driver> drivers3 = new SortableList<Driver>();
            try
            {   
                SortableList<Driver> drivers = new SortableList<Driver>();
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                //20140630 - gerry modified the query to include partially unavailable drivers
                //string SQLString = "SELECT * FROM TPT_Driver_Tbl";
                //SQLString += " WHERE Is_Available = 'T' and Employ_Status = 'A'";
                string SQLString = @"SELECT * FROM TPT_Driver_Tbl WHERE 1=1
                                    and Employ_Status = 'A'
                                    and Driver_Code not in (select Distinct Driver_Code from Tpt_driver_unavailable_tbl where 1=1
                                                                and (DateFrom  = '{0}' and TimeFrom = '{1}' and DateTo = '{0}' and TimeTo = '{2}')
                                                                OR (DateFrom  = '{0}' and TimeFrom = '{1}' and DateTo > '{0}')
                                                                OR (DateFrom  < '{0}' and DateTo > '{0}')
                                                                OR (DateFrom  < '{0}' and DateTo = '{0}' and TimeTo = '{2}') 
                                                            )";
                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(date),MIN_DAY_TIME, MAX_DAY_TIME);

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                { 
                    drivers.Add(GetDriver(reader));
                }
                cn.Close();

                SortableList<Driver> drivers2 = new SortableList<Driver>();   
                for (int i = 0; i < drivers.Count; i++)
                {
                    cn.Open();
                    string SQLSearchString = "SELECT * FROM TPT_PLAN_TRIP_TBL ";
                    SQLSearchString += "where DRIVER_NO = '" + CommonUtilities.FormatString(drivers[i].Code) + "'";
                    SQLSearchString += " AND SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
                    SQLSearchString += " AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";
                    SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
                    DataSet dsSearchResult = new DataSet();
                    daSearchCmd.Fill(dsSearchResult);
                    cn.Close();
                    if (dsSearchResult.Tables[0].Rows.Count > 0)
                    {
                    }
                    else
                    {
                        drivers2.Add(drivers[i]);
                    }
                }

                for (int i = 0; i < drivers2.Count; i++)
                {
                    cn.Open();
                    string SQLSearchString = "SELECT * FROM TRK_PLAN_TRIP_TBL ";
                    SQLSearchString += "where DRIVER_NO = '" + CommonUtilities.FormatString(drivers2[i].Code) + "'";
                    SQLSearchString += " AND SCHEDULE_DATE >= '" + DateUtility.ConvertDateForSQLPurpose(date) + "'";
                    SQLSearchString += " AND SCHEDULE_DATE < '" + DateUtility.ConvertDateForSQLPurpose(date + TimeSpan.FromDays(1)) + "'";
                    SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
                    DataSet dsSearchResult = new DataSet();
                    daSearchCmd.Fill(dsSearchResult);
                    cn.Close();
                    if (dsSearchResult.Tables[0].Rows.Count > 0)
                    {
                    }
                    else
                    {
                        drivers3.Add(drivers[i]);
                    }
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers3;
        }

        internal static SortableList<Driver> GetAllDrivers()
        {
            SortableList<Driver> drivers = new SortableList<Driver>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_Driver_Tbl";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drivers.Add(GetDriver(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }  
            return drivers;
        }

        internal static SortableList<Driver> GetAllAvailableDrivers()
        {
            SortableList<Driver> drivers = new SortableList<Driver>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_Driver_Tbl ";
                SQLString += "WHERE Is_Available = 'T' AND EMPLOY_STATUS ='A' ";
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drivers.Add(GetDriver(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }

        internal static SortableList<Driver> GetAllAvailableDriversForParticularDateForHaulage(DateTime scheduleDate)
        {
            SortableList<Driver> drivers = new SortableList<Driver>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                //20131120 - gerry modify the query to only driver drivers with default vehicle is available
                //and those drivers with no default vehicle will not be shown.
                //20140630 - gerry modified
                string SQLString = @"SELECT * FROM TPT_Driver_Tbl as dr
                                    inner join TPT_Vehicle_Tbl as vh
                                    on vh.Vehicle_No = dr.Default_Vehicle
                                    WHERE dr.EMPLOY_STATUS ='A' 
                                    and dr.Driver_Code not in (select driver_code 
							                                    from TPT_PLAN_DEPT_DRIVER_TBL 
							                                    where schedule_date='{0}')
                                    and dr.Default_Vehicle <> ''
                                    and vh.Vehicle_Type = 1 --primemover only
                                    ";
                                    
                #region REmoved
//                and dr.Driver_Code not in (select Distinct Driver_Code from Tpt_driver_unavailable_tbl 
//                                                                    where DateFrom  <= '{0}' and DateTo > '{0}')
//                                    ";


                //and dr.Driver_Code not in (select Distinct Driver_Code from Tpt_driver_unavailable_tbl where 1=1
                                    //                            --and (DateFrom  = '{0}' and TimeFrom = '{1}' and DateTo = '{0}' and TimeTo = '{2}')
                                    //                            AND (DateFrom  = '{0}' and TimeFrom = '{1}' and DateTo > '{0}')
                                    //                            OR (DateFrom  < '{0}' and DateTo > '{0}')
                                    //                            OR (DateFrom  < '{0}' and DateTo = '{0}' and TimeTo = '{2}') 
                //                            )";
                #endregion
                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate));
                //SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), MIN_DAY_TIME, MAX_DAY_TIME);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drivers.Add(GetDriver(reader));
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }
        //20160226 - gerry added
        internal static SortableList<Driver> GetAllAvailableDriversForParticularDateForTrucking(DateTime scheduleDate)
        {
            SortableList<Driver> drivers = new SortableList<Driver>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                //20131120 - gerry modify the query to only driver drivers with default vehicle is available
                //and those drivers with no default vehicle will not be shown.
                //20140630 - gerry modified
                string SQLString = @"SELECT * FROM TPT_Driver_Tbl as dr
                                    inner join TPT_Vehicle_Tbl as vh
                                    on vh.Vehicle_No = dr.Default_Vehicle
                                    WHERE dr.EMPLOY_STATUS ='A' 
                                    and dr.Driver_Code not in (select driver_code 
							                                    from TPT_PLAN_DEPT_DRIVER_TBL 
							                                    where schedule_date='{0}')

                                    and dr.Default_Vehicle <> ''
                                    and vh.Vehicle_Type = 3
                                    ";

                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate));
                //SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), MIN_DAY_TIME, MAX_DAY_TIME);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drivers.Add(GetDriver(reader));
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }



        internal static Driver GetDriver(IDataReader reader)
        {
            try
            {
                bool isavailable = true;
                EmployeeStatus driverEmployeeStatus;
                string temp = (string)reader["Is_Available"];
                if (temp == "F")
                {
                    isavailable = false;
                }

                string tempEmployeeStatus = (string)reader["Employ_Status"];
                if (tempEmployeeStatus == "A")
                {
                    driverEmployeeStatus = EmployeeStatus.Available;
                }
                else
                {
                    driverEmployeeStatus = EmployeeStatus.Resigned;
                }


                Driver driver = new Driver(
                    (string)reader["Driver_Code"],
                    (string)reader["Driver_Name"],
                    (string)reader["NRIC_No"],
                    (string)reader["Nationality"],
                    (string)reader["Driving_Licence_No"],
                    (string)reader["Driving_Class"],
                    (DateTime)reader["Licence_Expiry_Date"],
                    (string)reader["Default_Vehicle"],
                    isavailable,
                    driverEmployeeStatus
                    );
                return driver;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static SortableList<Driver> GetDrivers(string driverCode)
        {
            try
            {
                SortableList<Driver> drivers = new SortableList<Driver>();
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());

                //Gerry Modified - to show drivers either available or not, because it cause duplicate driver code if we not show resigned or not available drivers
                //string SQLString = "SELECT * FROM TPT_Driver_Tbl where Employ_status ='A' ";
                string SQLString = "SELECT * FROM TPT_Driver_Tbl where 1=1 ";

                if (driverCode.Trim() != "")
                {
                    SQLString += " and Driver_Code= '" + CommonUtilities.FormatString(driverCode.Trim()) + " ' ";
                }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drivers.Add(GetDriver(reader));
                }
                cn.Close();
                return drivers;



                //return drivers;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static Driver GetDriver(string driverCode, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                Driver driver = new Driver();
                string SQLString = "SELECT * FROM TPT_Driver_Tbl where Driver_Code = '{0}' ";
                SQLString = string.Format(SQLString, CommonUtilities.FormatString(driverCode.Trim()));
                
                SqlCommand cmd = new SqlCommand(SQLString, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    driver = GetDriver(reader);
                }
                reader.Close();
                return driver;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static Driver GetDriver(string driverCode)
        {
            try
            {
                Driver driver = new Driver();
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_Driver_Tbl where employ_status = 'A' ";
                if (driverCode.Trim() != "")
                {
                    SQLString += " and Driver_Code= '" + CommonUtilities.FormatString(driverCode.Trim()) + " ' ";
                }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    driver = GetDriver(reader);
                }
                cn.Close();
                return driver;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static Driver GetDriverByVehicle(string vehicleNo)
        {
            try
            {
                Driver driver = new Driver();
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT * FROM TPT_Driver_Tbl where employ_status = 'A' ";
                if (vehicleNo.Trim() != "")
                {
                    SQLString += " and Default_Vehicle= '" + CommonUtilities.FormatString(vehicleNo.Trim()) + " ' ";
                }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    driver = GetDriver(reader);
                }
                cn.Close();
                return driver;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static string GetDriverName(string driverCode)
        {
            try
            {
                string driverName = "";
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = "SELECT Driver_Name FROM TPT_Driver_Tbl ";
                if (driverCode.Trim() != "")
                {
                    SQLString += " where Driver_Code= '" + CommonUtilities.FormatString(driverCode.Trim()) + " ' ";
                }
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //driverName = (string)reader["Driver_Name"];
                    driverName = reader.GetString(0).ToString().Trim();
                }
                cn.Close();
                return driverName;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static bool AddDriver(Driver driver, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string Is_Available = driver.isAvailable ? "T" : "F";
                string driverEmployeeStatus = driver.EmployeeStatus == EmployeeStatus.Available ? "A" : "R";
                string SQLInsertDriverString = " Insert into TPT_Driver_Tbl(Driver_Code,Driver_Name,NRIC_No,Nationality,Driving_Licence_No,Driving_Class,Licence_Expiry_Date,Default_Vehicle,Is_Available,Employ_Status) " +
                                               " Values ('" + CommonUtilities.FormatString(driver.Code) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.Name) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.nirc) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.nationality) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.DrivingLicence) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.DrivingClass) + "'" + "," +
                                               "'" + DateUtility.ConvertDateForSQLPurpose(driver.licenceExpiryDate) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.defaultVehicleNumber) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(Is_Available.Trim()) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driverEmployeeStatus) + "'" +
                                               ")";

                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(SQLInsertDriverString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static bool AddDriver(Driver driver)
        {
            try
            {
                string Is_Available = driver.isAvailable ? "T" : "F";
                string driverEmployeeStatus = driver.EmployeeStatus == EmployeeStatus.Available ? "A" : "R";
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLInsertDriverString = " Insert into TPT_Driver_Tbl(Driver_Code,Driver_Name,NRIC_No,Nationality,Driving_Licence_No,Driving_Class,Licence_Expiry_Date,Default_Vehicle,Is_Available,Employ_Status) " +
                                               " Values ('" + CommonUtilities.FormatString(driver.Code) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.Name) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.nirc) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.nationality) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.DrivingLicence) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.DrivingClass) + "'" + "," +
                                               "'" + DateUtility.ConvertDateForSQLPurpose(driver.licenceExpiryDate) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driver.defaultVehicleNumber) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(Is_Available.Trim()) + "'" + "," +
                                               "'" + CommonUtilities.FormatString(driverEmployeeStatus) + "'" +
                                               ")";

                SqlCommand cmd = new SqlCommand(SQLInsertDriverString, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static bool EditDriver(Driver driver, SqlConnection con, SqlTransaction tran)
        {
            bool retValue = false;
            try
            {
                string Is_Available = driver.isAvailable ? "T" : "F";

                string driverEmployeeStatus = driver.EmployeeStatus == EmployeeStatus.Available?"A": "R";

                string SQLUpdateDriverString = " Update TPT_Driver_Tbl set " +
                                               " Driver_Name = '" + CommonUtilities.FormatString(driver.Name) + "'" + "," +
                                               " NRIC_No  ='" + CommonUtilities.FormatString(driver.nirc) + "'" + "," +
                                               " Nationality ='" + CommonUtilities.FormatString(driver.nationality) + "'" + "," +
                                               " Driving_Licence_No ='" + CommonUtilities.FormatString(driver.DrivingLicence) + "'" + "," +
                                               " Driving_Class ='" + CommonUtilities.FormatString(driver.DrivingClass) + "'" + "," +
                                               " Licence_Expiry_Date ='" + DateUtility.ConvertDateForSQLPurpose(driver.licenceExpiryDate) + "'" + "," +
                                               " Default_Vehicle = '" + CommonUtilities.FormatString(driver.defaultVehicleNumber) + "'" + "," +
                                               " Employ_Status ='" + CommonUtilities.FormatString(driverEmployeeStatus) + "'" + "," +
                                               " Is_Available ='" + CommonUtilities.FormatString(Is_Available) + "'" +
                                               " Where Driver_Code = '" + CommonUtilities.FormatString(driver.Code) + "'";

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;

        }
        internal static bool EditDriver(Driver driver)
        {
            bool retValue = false;
            try
            {
                string Is_Available;
                if (driver.isAvailable)
                {
                    Is_Available = "T";
                }
                else
                {
                    Is_Available = "F";
                }

                string driverEmployeeStatus = string.Empty;
                if (driver.EmployeeStatus == EmployeeStatus.Available)
                {
                    driverEmployeeStatus = "A";
                }
                else
                {
                    driverEmployeeStatus = "R";
                }
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLUpdateDriverString = " Update TPT_Driver_Tbl set " +
                                               " Driver_Name = '" + CommonUtilities.FormatString(driver.Name) + "'" + "," +
                                               " NRIC_No  ='" + CommonUtilities.FormatString(driver.nirc) + "'" + "," +
                                               " Nationality ='" + CommonUtilities.FormatString(driver.nationality) + "'" + "," +
                                               " Driving_Licence_No ='" + CommonUtilities.FormatString(driver.DrivingLicence) + "'" + "," +
                                               " Driving_Class ='" + CommonUtilities.FormatString(driver.DrivingClass) + "'" + "," +
                                               " Licence_Expiry_Date ='" + DateUtility.ConvertDateForSQLPurpose(driver.licenceExpiryDate) + "'" + "," +
                                               " Default_Vehicle = '" + CommonUtilities.FormatString(driver.defaultVehicleNumber) + "'" + "," +
                                               " Employ_Status ='" + CommonUtilities.FormatString(driverEmployeeStatus) + "'" + "," +
                                               " Is_Available ='" + CommonUtilities.FormatString(Is_Available) + "'" +
                                               " Where Driver_Code = '" + CommonUtilities.FormatString(driver.Code) + "'";

                SqlCommand cmd = new SqlCommand(SQLUpdateDriverString, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                retValue = true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;

        }
        internal static bool DeleteDriver(string driverNo, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string SQLDeleteDriverString = @"DELETE TPT_PLAN_DRIVER_PRIORITY_Tbl
                                                    where DriverCode = '{0}'
                                                    DELETE TPT_Driver_Tbl 
                                                    where Driver_Code = '{0}'";

                SQLDeleteDriverString = string.Format(SQLDeleteDriverString, CommonUtilities.FormatString(driverNo));
                SqlCommand cmd = new SqlCommand(SQLDeleteDriverString, con);
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }
        internal static bool DeleteDriver(string driverNo)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLDeleteDriverString = @"DELETE TPT_PLAN_DRIVER_PRIORITY_Tbl
                                                    where DriverCode = '{0}'
                                                    DELETE TPT_Driver_Tbl 
                                                    where Driver_Code = '{0}'";
                    
                SQLDeleteDriverString = string.Format(SQLDeleteDriverString, CommonUtilities.FormatString(driverNo));
                SqlCommand cmd = new SqlCommand(SQLDeleteDriverString, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static bool VerifyExixtingDriver(string driverCode)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLSearchString = "SELECT * FROM TPT_Driver_Tbl where Driver_Code = '" + CommonUtilities.FormatString(driverCode) + "'";
                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLSearchString, cn);
                DataSet dsSearchResult = new DataSet();
                daSearchCmd.Fill(dsSearchResult);
                cn.Close();
                if (dsSearchResult.Tables[0].Rows.Count > 0)
                    return false;
                else
                    return true;
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static bool IsJobTripUsedByDriver(string driverCode)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                //string SQLSearchString = "SELECT DRIVER_NO FROM TPT_PLAN_TRIP_TBL where DRIVER_NO = '" + CommonUtilities.FormatString(driverCode) + "'";
                string SQLString = @" SELECT DRIVER_NO FROM TPT_PLAN_TRIP_TBL where DRIVER_NO = '{0}'";
                SQLString = string.Format(SQLString, CommonUtilities.FormatString(driverCode));
                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
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
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static bool IsDriverHasAssignedPriority(string driverCode)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLString = @" SELECT DRIVERCode FROM TPT_PLAN_DRIVER_PRIORITY_Tbl where DRIVERCode = '{0}' ";
                SQLString = string.Format(SQLString, CommonUtilities.FormatString(driverCode));
                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
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
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

        internal static bool IsVehicleAssigned(string vehicleno)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLSearchString = "SELECT * FROM TPT_Driver_Tbl where Default_Vehicle = '" + CommonUtilities.FormatString(vehicleno) + "'";
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
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }


        internal static bool IsVehicleAssignedForEditMode(string vehicleno, string driver)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLSearchString = "SELECT * FROM TPT_Driver_Tbl where Default_Vehicle = '" + CommonUtilities.FormatString(vehicleno) + "'";
                SQLSearchString += " AND Driver_Code <> '" + driver + "'";
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
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }


        internal static List<string> GetDriverInPlanning(DateTime scheduleDate, string deptCode)
        {
            List<string> drivers = new List<string>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT TPT_Driver_Tbl.Driver_Name FROM TPT_Driver_Tbl 
                                  inner join TPT_PLAN_DEPT_DRIVER_TBL 
                                        ON TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE = TPT_Driver_Tbl.Driver_Code
                                    WHERE TPT_Driver_Tbl.Is_Available = 'T' 
                                       AND TPT_Driver_Tbl.EMPLOY_STATUS ='A'
                                       AND TPT_PLAN_DEPT_DRIVER_TBL.schedule_date = '{0}'
                                       AND TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE = '{1}'
                                    order by TPT_PLAN_DEPT_DRIVER_TBL.Driver_Code";

                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), CommonUtilities.FormatString(deptCode));

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drivers.Add(reader.GetString(0));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }
        internal static List<Driver> GetDriversInPlanning(DateTime scheduleDate, string deptCode)
        {
            List<Driver> drivers = new List<Driver>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                string SQLString = @"SELECT TPT_Driver_Tbl.* FROM TPT_Driver_Tbl 
                                  inner join TPT_PLAN_DEPT_DRIVER_TBL 
                                        ON TPT_PLAN_DEPT_DRIVER_TBL.DRIVER_CODE = TPT_Driver_Tbl.Driver_Code
                                    WHERE TPT_Driver_Tbl.Is_Available = 'T' 
                                       AND TPT_Driver_Tbl.EMPLOY_STATUS ='A'
                                       AND TPT_PLAN_DEPT_DRIVER_TBL.schedule_date = '{0}'
                                       AND TPT_PLAN_DEPT_DRIVER_TBL.TPT_DEPT_CODE = '{1}'
                                    order by TPT_PLAN_DEPT_DRIVER_TBL.Driver_Code";

                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), CommonUtilities.FormatString(deptCode));

                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drivers.Add(GetDriver(reader));
                }
                cn.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }

        #region "2014-05-14 Zhou Kai adds functions"
        internal static List<Driver> GetUnAllocatedDriverForDate(DateTime date)
        {
            List<Driver> tmp1 = GetAllDriversInScheduleButHasNoPlanTripsByDate(date);
            List<Driver> tmp2 = GetAllDriversInScheduleButHasNoPlanSubTripsByDate(date);

            tmp1.AddRange(tmp2);

            return tmp1;
        }

        internal static List<Driver> GetAllDriversInScheduleButHasNoPlanTripsByDate(DateTime date)
        {
            List<Driver> tmp = new List<Driver>();
            string sqlQuery = "SELECT * FROM [dbo].[TPT_Driver_Tbl] WHERE " + // select all drivers
                "[Driver_Code] IN " +  // select all drivers on tpt_plan_dept_driver_tbl on a certain date
                "(SELECT [Driver_Code] FROM [dbo].[TPT_PLAN_DEPT_DRIVER_TBL] WHERE " +
                "[dbo].[TPT_PLAN_DEPT_DRIVER_TBL].[SCHEDULE_DATE] = '{0}' AND " +
                "[Driver_Code] NOT IN " + // select all drivers on trk_plan_trip_tbl on that same date
                "(SELECT [Driver_No] FROM [dbo].[TRK_PLAN_TRIP_TBL] WHERE " +
                "[dbo].[TRK_PLAN_TRIP_TBL].[SCHEDULE_DATE] = '{0}') AND " +
                "[Driver_Code] NOT IN " + // select all drivers on tpt_plan_trip_tbl on that same date
                "(SELECT [Driver_No] FROM [dbo].[TPT_PLAN_TRIP_TBL] WHERE " +
                "[dbo].[TPT_PLAN_TRIP_TBL].[SCHEDULE_DATE] = '{0}'))";
            string strDate = date.ToString("yyyy-MM-dd");
            sqlQuery = String.Format(sqlQuery, strDate);
            SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand dbCmd = new SqlCommand();
            SqlDataReader dbRder;
            try
            {
                if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = sqlQuery;
                dbCmd.Connection = dbCon;

                dbRder = dbCmd.ExecuteReader();
                while (dbRder.Read())
                {
                    Driver dr = GetDriver(dbRder);
                    dr.unAvailableDates = GetDriverUnavailableDates(dr.Code, date.Date);
                    tmp.Add(dr);
                    //tmp.Add(GetDriver(dbRder));
                }
                dbRder.Close();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException se) { throw se; }
            catch (Exception e) { throw e; }
            finally { dbCon.Close(); }

            return tmp;
        }

        internal static List<Driver> GetAllDriversInScheduleButHasNoPlanSubTripsByDate(DateTime date)
        {
            List<Driver> tmp = new List<Driver>();

            string sqlQuery = "SELECT * FROM [dbo].[TPT_Driver_Tbl] WHERE " + // select all drivers
                "[Driver_Code] IN " +  // select all drivers from trk_plan_trip_tbl on certain date
                "(SELECT [Driver_No] FROM [dbo].[TRK_PLAN_TRIP_TBL] WHERE " +
                "[dbo].[TRK_PLAN_TRIP_TBL].[SCHEDULE_DATE] = '{0}' AND " +
                "[dbo].[TRK_PLAN_TRIP_TBL].[PLANTRIP_NO] NOT IN " + // select those planTrips which has no subTrips
                "(SELECT [PLANTRIP_NO] FROM [dbo].[TRK_PLAN_SUB_TRIP_TBL]))";
            string strDate = date.ToString("yyyy-MM-dd");
            sqlQuery = String.Format(sqlQuery, strDate);
            SqlConnection dbCon = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SqlCommand dbCmd = new SqlCommand();
            SqlDataReader dbRder;
            try
            {
                if (dbCon.State == ConnectionState.Closed) { dbCon.Open(); }
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = sqlQuery;
                dbCmd.Connection = dbCon;

                dbRder = dbCmd.ExecuteReader();
                while (dbRder.Read())
                {
                    Driver dr = GetDriver(dbRder);
                    dr.unAvailableDates = GetDriverUnavailableDates(dr.Code, date.Date);
                    tmp.Add(dr);
                    //tmp.Add(GetDriver(dbRder));
                }
                dbRder.Close();
            }
            catch (FMException fme) { throw fme; }
            catch (SqlException se) { throw se; }
            catch (Exception e) { throw e; }
            finally { dbCon.Close(); }

            return tmp;
        }

        #endregion
        //20140625 - gerry added to get unavailable drivers based on schedule date
        //this filter can be use in getting unvailable dates from planning 
        internal static SortableList<DriverUnavailable> GetDriverUnavailableDates(string driveCode, DateTime scheduleDate, SqlConnection con, SqlTransaction tran)
        {
            SortableList<DriverUnavailable> retValues = new SortableList<DriverUnavailable>();
            try
            {
                string query = @"select * from TPT_DRIVER_UNAVAILABLE_TBL where 1=1 ";

                //if start date is not the minimum date
                //this filter can be use in getting unvailable dates from planning 
                if (scheduleDate != DateUtility.GetSQLDateTimeMinimumValue())
                {
                    query += @" and DateFrom <= '{0}' and DateTo >= '{0}'";     //20140813 - gerry modified to DateFrom <= scheduleDate 
                }
                query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(scheduleDate));
                //add filter
                if (!driveCode.Equals(string.Empty))
                    query += @" and Driver_Code = '" + driveCode + "'";

                                                                        
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DriverUnavailable unavailableDriver = GetUnavailableDate(reader);
                    if (unavailableDriver != null)
                        retValues.Add(unavailableDriver);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValues;
        }
        internal static SortableList<DriverUnavailable> GetDriverUnavailableDates(string driveCode, DateTime scheduleDate)
        {
            SortableList<DriverUnavailable> retValues = new SortableList<DriverUnavailable>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string query = @"select * from TPT_DRIVER_UNAVAILABLE_TBL where 1=1 ";

                //if start date is not the minimum date
                //this filter can be use in getting unvailable dates from planning 
                if (scheduleDate != DateUtility.GetSQLDateTimeMinimumValue())
                {
                    query += @" and DateFrom <= '{0}' and DateTo >= '{0}'";     //20140813 - gerry modified to DateFrom <= scheduleDate 
                }
                query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(scheduleDate));
                //add filter
                if (!driveCode.Equals(string.Empty))
                    query += @" and Driver_Code = '" + driveCode + "'";


                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmd = new SqlCommand(query, con);
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DriverUnavailable unavailableDriver = GetUnavailableDate(reader);
                    if (unavailableDriver != null)
                        retValues.Add(unavailableDriver);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValues;
        }
  
        //20140625 - gerry added to get unavailable drivers from the range
        internal static SortableList<DriverUnavailable> GetDriverUnavailableDates(string driveCode, DateTime dateTimeStartFrom, DateTime dateTimeStartTo)
        {
            SortableList<DriverUnavailable> retValues = new SortableList<DriverUnavailable>();
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                //date time filter is in UI using LINQ inside btnUnavailableDatesQuery
                string query = @"select * from TPT_DRIVER_UNAVAILABLE_TBL where 1=1 ";
                 //add filter
                if (!driveCode.Equals(string.Empty))
                    query += @" and Driver_Code = '" + driveCode + "'";

                query += @" and DateFrom >= '{0}' and TimeFrom >= '{1}'
                            and DateTo <= '{2}' and TimeTo <='{3}'";

                //string startTime = (dateTimeStartFrom.Hour.ToString().Length == 2 ? dateTimeStartFrom.Hour.ToString() : ("0" + dateTimeStartFrom.Hour.ToString())) +
                //                   (dateTimeStartFrom.Minute.ToString().Length == 2 ? dateTimeStartFrom.Minute.ToString() : ("0" + dateTimeStartFrom.Minute.ToString()));
                //string endTime = (dateTimeStartTo.Hour.ToString().Length == 2 ? dateTimeStartTo.Hour.ToString() : ("0" + dateTimeStartTo.Hour.ToString())) +
                //                   (dateTimeStartTo.Minute.ToString().Length == 2 ? dateTimeStartTo.Minute.ToString() : ("0" + dateTimeStartTo.Minute.ToString()));

                query = string.Format(query, DateUtility.ConvertDateForSQLPurpose(dateTimeStartFrom), MIN_DAY_TIME,
                                             DateUtility.ConvertDateForSQLPurpose(dateTimeStartTo), MAX_DAY_TIME);

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DriverUnavailable unavailableDriver = GetUnavailableDate(reader);
                    if(unavailableDriver != null)
                        retValues.Add(unavailableDriver);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { con.Close(); }
            return retValues;
        }
        //20140625 - gerry added to reader unavailable drivers from reader
        internal static DriverUnavailable GetUnavailableDate(IDataReader reader)
        {
            DriverUnavailable retValue = null;
            try
            {
                retValue = new DriverUnavailable();
                int sHr = Convert.ToInt32(((string)reader["TimeFrom"]).Substring(0, 2));
                int sMin = Convert.ToInt32(((string)reader["TimeFrom"]).Substring(2, 2));
                int eHr = Convert.ToInt32(((string)reader["TimeTo"]).Substring(0, 2));
                int eMin = Convert.ToInt32(((string)reader["TimeTo"]).Substring(2, 2));
                retValue.driverCode = reader["Driver_Code"] == DBNull.Value ? string.Empty : (string)reader["Driver_Code"];
                retValue.seqNo = reader["Seq_No"] == DBNull.Value ? 0 : (int)reader["Seq_No"];
                retValue.startDateTime = ((DateTime)reader["DateFrom"]).Date.AddHours(sHr).AddMinutes(sMin);
                retValue.endDateTime = ((DateTime)reader["DateTo"]).Date.AddHours(eHr).AddMinutes(eMin);
                //retValue.isWholeUnavailable = (string)reader["Is_WholeDay"] == "T" ? true : false; 
                retValue.remarks = reader["Remarks"] == DBNull.Value ? string.Empty : (string)reader["Remarks"];
            }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }
            return retValue;
        }
       
        //20140625 - gerry added to get unavailable drivers from the range
        internal static DriverUnavailable GetDriverUnavailableDate(string driveCode, int seqNo)
        {
            DriverUnavailable retValue = null;
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string query = @"select * from TPT_DRIVER_UNAVAILABLE_TBL where 1=1
                                    and driver_code ='{0}'
                                    and [Seq_No] ={1}";
                query = string.Format(query, driveCode, seqNo);

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = GetUnavailableDate(reader);
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { con.Close(); }
            return retValue;
        }
        internal static int GetNextUnavailableDateSeqNo(string driverCode, SqlConnection con, SqlTransaction tran)
        {
            int retValue = 0;
            try
            {
                string query = @"select MAX(Seq_No) from TPT_DRIVER_UNAVAILABLE_TBL where driver_code ='"+driverCode+"'";
                                                                                       
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    retValue = reader[0] == DBNull.Value? 0 : (int)reader[0];
                }
                reader.Close();               
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue+1;
        }
        //20140625 - gerry added to insert unavailable driver
        internal static bool AddDriverUnavailable(DriverUnavailable driverUnavailable, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                int SeqNo = GetNextUnavailableDateSeqNo(driverUnavailable.driverCode, con, tran);
                string startTime = driverUnavailable.startDateTime.TimeOfDay.ToString();
                startTime = startTime.Replace(":", "").Substring(0, 4);
                string endTime = driverUnavailable.endDateTime.TimeOfDay.ToString();
                endTime = endTime.Replace(":", "").Substring(0, 4);
                string query = @"INSERT INTO TPT_DRIVER_UNAVAILABLE_TBL
                                    VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                query = string.Format(query, driverUnavailable.driverCode, SeqNo,
                                       DateUtility.ConvertDateForSQLPurpose(driverUnavailable.startDateTime),
                                       startTime,
                                       DateUtility.ConvertDateForSQLPurpose(driverUnavailable.endDateTime),
                                       endTime,
                                       driverUnavailable.remarks);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }
        //20140625 - gerry added to edit unavailable driver
        internal static bool EditDriverUnavailable(DriverUnavailable driverUnavailable, SqlConnection con, SqlTransaction tran)
        {
            try
            {
                string startTime = driverUnavailable.startDateTime.TimeOfDay.ToString();
                startTime = startTime.Replace(":", "").Substring(0, 4);
                string endTime = driverUnavailable.endDateTime.TimeOfDay.ToString();
                endTime = endTime.Replace(":", "").Substring(0, 4);
                string query = @"UPDATE TPT_DRIVER_UNAVAILABLE_TBL 
                                    set DateFrom ='{2}',
                                        TimeFrom = '{3}',
	                                    DateTo = '{4}',
	                                    TimeTo = '{5}',   
	                                    Remarks = '{6}'
                                    where Driver_Code = '{0}' and Seq_No = '{1}'";
                query = string.Format(query, driverUnavailable.driverCode, driverUnavailable.seqNo,
                                       DateUtility.ConvertDateForSQLPurpose(driverUnavailable.startDateTime.Date),
                                       startTime,
                                       DateUtility.ConvertDateForSQLPurpose(driverUnavailable.endDateTime.Date),
                                       endTime,
                                       driverUnavailable.remarks);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }
        //20140625 - gerry added to delete unavailable driver
        internal static bool DeleteDriverUnavailable(DriverUnavailable driverUnavailable, SqlConnection con, SqlTransaction tran)
        {
            try
            {

                string startTime = driverUnavailable.startDateTime.TimeOfDay.ToString();
                startTime = startTime.Replace(":", "").Substring(0, 4);
                string query = @"DELETE FROM TPT_DRIVER_UNAVAILABLE_TBL 
                                    where Driver_Code = '{0}'
                                    and DateFrom = '{1}'
	                                and TimeFrom = '{2}'";
                query = string.Format(query, driverUnavailable.driverCode,
                                       DateUtility.ConvertDateForSQLPurpose(driverUnavailable.startDateTime.Date),
                                       startTime);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                if (tran == null) { tran = con.BeginTransaction(); }
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }

        internal static bool DoDatesOverlapped(DriverUnavailable driverUnavailable)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            try
            {
                string query = @"IF EXISTS(SELECT * FROM TEMPDB.SYS.TABLES WHERE NAME like '#temp1%')
                                DROP TABLE #temp1
                               
                                select (DateFrom + CAST((SubString(TimeFrom,1, 2) +':' +SubString(TimeFrom,3, 2)+':00') as DATETIME)) as startDateTime,
                                (DateTo + CAST((SubString(TimeTo,1, 2) +':' +SubString(TimeTo,3, 2)+':00') as DATETIME)) as endDateTime
                                 , * into #temp1 
                                 from TPT_DRIVER_UNAVAILABLE_TBL where 1=1
                                
                                select COUNT(*) from #temp1
                                where 1=1
                                 and Driver_Code = '{2}' and Seq_No <> {3} 
                                 and ((startDateTime <='{0}' and endDateTime >='{0}')
                                         OR (startDateTime > '{0}' and endDateTime <'{1}')
                                         OR (startDateTime > '{0}' and startDateTime <'{1}')
                                         OR (endDateTime > '{1}' and endDateTime <'{1}'))

                                DROP TABLE #temp1
                                ";

                query = string.Format(query, DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(driverUnavailable.startDateTime)
                                            , DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(driverUnavailable.endDateTime)
                                            , CommonUtilities.FormatString(driverUnavailable.driverCode)
                                            , driverUnavailable.seqNo);

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                object obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    if ((int)obj == 0)
                        return false;
                    else
                        throw new FMException(TptResourceBLL.ErrUnavailableDateTimeOverlapped);
                }
                else
                    throw new FMException(TptResourceBLL.ErrUnavailableDateTimeOverlapped);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            finally { con.Close(); }
        }
        internal static SortableList<Driver> GetDriversUnavailableForTheDay(DateTime scheduleDate)
        {
            SortableList<Driver> drivers = new SortableList<Driver>();
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                //20141103 - gerry modified query
                string SQLString = @"SELECT * FROM TPT_Driver_Tbl as dr
                                        inner join TPT_Vehicle_Tbl as vh
                                        on vh.Vehicle_No = dr.Default_Vehicle
                                    WHERE dr.EMPLOY_STATUS ='R' 
                                    OR (dr.EMPLOY_STATUS ='A' 
	                                    and dr.Driver_Code in (select Distinct Driver_Code from Tpt_driver_unavailable_tbl where 1=1
                                                                and (DateFrom  = '{0}' and TimeFrom = '{1}' and DateTo = '{0}' and TimeTo = '{2}')
                                                                OR (DateFrom  = '{0}' and TimeFrom = '{1}' and DateTo > '{0}')
                                                                OR (DateFrom  < '{0}' and DateTo > '{0}')
                                                                OR (DateFrom  < '{0}' and DateTo = '{0}' and TimeTo = '{2}') 
                                                                )   
                                    )";
                //+ + "')";
                //SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate));
                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(scheduleDate), MIN_DAY_TIME, MAX_DAY_TIME);
                SqlCommand cmd = new SqlCommand(SQLString, cn);
                cn.Open();
                SqlTransaction tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Driver dr = GetDriver(reader);
                    if (GetDriverUnavailableDates(dr.Code, scheduleDate.Date, scheduleDate.AddHours(23).AddMinutes(59)).Count > 0)
                    {
                        DriverUnavailable unAvailable = GetDriverUnavailableDates(dr.Code, scheduleDate.Date, scheduleDate.AddHours(23).AddMinutes(59))[0];
                        dr.unAvailableDisplayTime = unAvailable.remarks + " from " + unAvailable.startDateTime.ToString("dd/MM/yyyy HH:mm") + " - " + unAvailable.endDateTime.ToString("dd/MM/yyyy HH:mm");

                    }
                    drivers.Add(dr);
                }
                reader.Close();
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return drivers;
        }

        internal static bool IsDriverAssignJob(string DriverCode, DateTime ScheduleDate, DateTime StartTime, DateTime EndTime)
        {
            try
            {
                SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                cn.Open();
                string SQLString = @" SELECT * FROM TPT_PLAN_SUBTRIP_TBL PST
                                             INNER JOIN TPT_PLAN_TRIP_TBL PT
                                             ON PST.PLANTRIP_NO = PT.PLANTRIP_NO
                                             WHERE PT.SCHEDULE_DATE = '{0}'
                                             AND DRIVER_NO = '{1}'
                                             AND ((START_TIME < '{3}') and ('{2}' < END_TIME))";
                SQLString = string.Format(SQLString, DateUtility.ConvertDateForSQLPurpose(ScheduleDate),
                                                    CommonUtilities.FormatString(DriverCode),
                                                    DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(StartTime),
                                                    DateUtility.ConvertDateAndTimeWithZeroSecForSQLPurpose(EndTime));
                SqlDataAdapter daSearchCmd = new SqlDataAdapter(SQLString, cn);
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
            catch (FMException fmEx) { throw fmEx; }
            catch (SqlException ex) { throw new FMException(ex.ToString()); }
            catch (InvalidOperationException ex) { throw new FMException(ex.ToString()); }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
        }

    }
}
