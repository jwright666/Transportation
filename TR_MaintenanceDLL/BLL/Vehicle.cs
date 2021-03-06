﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_MaintenanceDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using FM.TR_FMSystemDLL.DAL;
using TR_LanguageResource.Resources;
using TR_LicenseDLL.BLL.Transport;
using System.Data;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class Vehicle
    {
        // private int vehicleID;
        private string number;
        private VehicleType type;
        private string make; 
        private string model; 
        private decimal maximumLadenWeight; 
        private bool isAvailable; 
        private decimal tareWeight; 
        private string trailerType; 
        private string defaultStop; 
        private string remarks; 
        private byte[] updateVersion;
        private decimal volCapacity;
        //20140620 - gerry added
        public DateTime vehicleInspectionDateDue { get; set; }
        public DateTime vehicleInspectionDateCompleted { get; set; }
        //20140620 gerry end
        //public string DescriptionForPlanningPurpose { get; set; }

        //20160414 - gerry added for truck size
        public ContainerOrTruckSize containerOrTruckSize { get; set; }
        //20160820 - gerry added for visual loading (3D app)
        public string cargoSpace_id { get; set; } //for haulage = container; for trucking = lorry
        public string cloudTrack_device_id { get; set; } //for haulage = container; for trucking = lorry
        //20170801
        public bool isRefrigerated { get; set; }



        //20140127 - added for display
        public string NumberAndCapacity
        {
            get { return number == string.Empty ? string.Empty 
                        : number + " - " + ("Wt."+Decimal.Round(maximumLadenWeight, 2).ToString().Trim()
                                        + "Vol." + Decimal.Round(volCapacity, 2).ToString().Trim());
            }
        }

        public string Number
        {
            get { return number; }
            set { number = value; }
        }

        public VehicleType Vtype
        {
            get { return type; }
            set { type = value; }
        }

        public string Make
        {
            get { return make; }
            set { make = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }


        public decimal MaximumLadenWeight
        {
            get { return maximumLadenWeight; }
            set { maximumLadenWeight = value; }
        }

        public bool IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value; }
        }

        public decimal TareWeight
        {
            get { return tareWeight; }
            set { tareWeight = value; }
        }

        public string TrailerType
        {
            get { return trailerType; }
            set { trailerType = value; }
        }

        public string DefaultStop
        {
            get { return defaultStop; }
            set { defaultStop = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public byte[] UpdateVersion
        {
            get { return updateVersion; }
            set { updateVersion = value; }
        }

        public decimal VolCapacity
        {
            get { return volCapacity; }
            set { volCapacity = value; }
        }

        public Vehicle()
        {
            //this.vehicleID = 0;
            this.type = VehicleType.PrimeMover;
            this.make = "";
            this.model = "";
            this.number = "";
            this.maximumLadenWeight = 0;
            this.isAvailable = false;
            this.tareWeight = 0;
            this.trailerType = "";
            this.defaultStop = "";
            this.remarks = "";
            this.updateVersion = new byte[8];
            this.volCapacity = 0;
            //20140620 - gerry added
            this.vehicleInspectionDateDue = DateTime.Today.AddMonths(6); // default to 6 month after creation
            this.vehicleInspectionDateCompleted = DateUtility.GetSQLDateTimeMinimumValue(); // set to minimun value for SQL, to display blank if not set
            this.cargoSpace_id = string.Empty;
            this.cloudTrack_device_id = string.Empty;
         }

        public Vehicle(VehicleType type, string make, string model, string number,
            decimal maximumLadenWeight, bool isAvailable, 
            decimal tareWeight, 
            string trailerType, 
            string defaultStop, 
            string remarks, 
            byte[] updateVersion,
            decimal volCapacity
            )
        {
            // this.VehicleID = vehicleID;
            this.type = type;
            this.make = make;
            this.model = model;
            this.number = number;
            this.maximumLadenWeight = maximumLadenWeight;
            this.isAvailable = isAvailable;
            this.tareWeight = tareWeight;
            this.trailerType = trailerType;
            this.defaultStop = defaultStop;
            this.remarks = remarks;
            this.updateVersion = updateVersion;
            this.volCapacity = volCapacity;
            this.cargoSpace_id = string.Empty;
            this.cloudTrack_device_id = string.Empty;
        }

        public override string ToString()
        {
            if (type == VehicleType.Trailer)
                return "Number:" + number + "; Type:" + trailerType;
            else if (type == VehicleType.Truck)
                return number.ToString() + "(Wt." + maximumLadenWeight + ")";
            else
                return number;

        }

        //public int VehicleID
        //{
        //    get { return vehicleID; }
        //    set { vehicleID = value; }
        //}

        //public string ToString()
        //{
        //    return number;
        //}

        //public static SortableList<Vehicle> GetAllVehicles()
        //{
        //    return VehicleDAL.GetAllVehicles();
        //}
        public static Vehicle GetVehicle(string vehicleNo)
        {
            return VehicleDAL.GetVehicle(vehicleNo);
        }        

        public static SortableList<Vehicle> GetVehicles(string vehicleNo, string vehicleType)
        {
            return VehicleDAL.GetVehicles(vehicleNo, vehicleType);
        }

        public static SortableList<Vehicle> GetAllVehicles()
        {
            return VehicleDAL.GetAllVehicles();
        }

        public static SortableList<Vehicle> GetAllTrailers()
        {
            return VehicleDAL.GetAllTrailers();
        }

        public static SortableList<Vehicle> GetAllTrailersAssigned(DateTime scheduleDate)
        {
            return VehicleDAL.GetAllTrailersAssigned(scheduleDate);
        }


        public static SortableList<Vehicle> GetAllTrailersForEditTptPlanDeptDate(DateTime scheduleDate)
        {
            SortableList<Vehicle> trailersavailable = new SortableList<Vehicle>();

            SortableList<Vehicle> a = new SortableList<Vehicle>();
            a = VehicleDAL.GetAllTrailersForParticularDate(scheduleDate);

            if (scheduleDate.Date==DateTime.Today.Date)
            {
                trailersavailable = a;
            } 
            else
            {
                SortableList<Vehicle> b = new SortableList<Vehicle>();
                b = GetAllTrailersAssigned(DateTime.Today.Date);

                for (int i = 0; i < a.Count; i++)
                {
                    bool find = false;
                    for (int j = 0; j < b.Count; j++)
                    {
                        if (a[i].number == b[j].number)
                        {
                            find = true;
                        }
                    }
                    if (find == false)
                    {
                        trailersavailable.Add(a[i]);
                    }

                }

            }
            return trailersavailable;
        }

        public static SortableList<Vehicle> GetAllTrailersForNewTptPlanDeptDate(TrailerStatus trailerStatus, DateTime scheduleDate)
        {
            SortableList<Vehicle> trailersavailable = new SortableList<Vehicle>();

            SortableList<Vehicle> a = new SortableList<Vehicle>();
            a = VehicleDAL.GetTrailersForTrailerLocationStatus(trailerStatus, scheduleDate);


            if (scheduleDate.Date == DateTime.Today.Date)
            {
                trailersavailable = a;
            }
            else
            {
                SortableList<Vehicle> b = new SortableList<Vehicle>();
                b = GetAllTrailersAssigned(DateTime.Today.Date);

                for (int i = 0; i < a.Count; i++)
                {
                    bool find = false;
                    for (int j = 0; j < b.Count; j++)
                    {
                        if (a[i].number == b[j].number)
                        {
                            find = true;
                        }
                    }
                    if (find == false)
                    {
                        trailersavailable.Add(a[i]);
                    }

                }

            }
            return trailersavailable;

        }


        public static SortableList<Vehicle> GetAllTrailersForParticularDate(DateTime scheduleDate)
        {
            return VehicleDAL.GetAllTrailersForParticularDate(scheduleDate);
        }

        public static SortableList<Vehicle> GetAllAvailableTrailers()
        {
            return VehicleDAL.GetAllAvailableTrailers();
        }

        public static SortableList<Vehicle> GetTrailersForTrailerLocationStatus(TrailerStatus trailerStatus,DateTime scheduleDate)
        {
            return VehicleDAL.GetTrailersForTrailerLocationStatus(trailerStatus, scheduleDate);
        }

        public static SortableList<Vehicle> GetTrailerFromFirstLeg(int jobid, int seqno)
        {
            return VehicleDAL.GetTrailerFromFirstLeg(jobid, seqno);
        }


        public static SortableList<Vehicle> GetAllAvailableTrailersBasedOnDepartment(DateTime scheduledate, string deptcode)
        {
            return VehicleDAL.GetAllAvailableTrailersBasedOnDepartment(scheduledate, deptcode);
        }

        public bool AddVehicle(string formName, string userID)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {
                if (this.ValidateAddVehicle())
                {
                    // 2014-02-21 Zhou Kai adds checking of duplicated primary key
                    if (IsVehicleNOExisting(this.Number)) 
                    {
                        throw new FMException(TptResourceUI.VehicleIDAlreadyExists); 
                    }

                    //20130619- gerry added 1 more validation for vehicle license
                    //if (IsLicenseEnough(DateTime.Now))
                    //{
                    VehicleDAL.AddVehicle(this, con, tran);
                    //}
                    if (this.Vtype == VehicleType.Truck)
                    {
                        if (this.containerOrTruckSize != null)
                            ContainerOrTruckSize.AddContainerOrTruckSize(this.containerOrTruckSize, con, tran);
                    }

                    //audit log
                    AuditLog auditLog = new AuditLog(this.Number, "TR", "SETUP", 0, userID, DateTime.Now, "VEHICLE_SETUP", 0, FormMode.Add.ToString());
                    auditLog.WriteAuditLog(this, null, con, tran);

                    tran.Commit();
                }
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
            return true;
        }

        public bool DeleteVehicle(string formName, string userID)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();  
            try
            {
                if (this.ValidateDeleteVehicle() == true)
                {
                    VehicleDAL.DeleteVehicle(this.Number, con, tran);
                    //20160414 - gerry added
                    if (this.Vtype == VehicleType.Truck)
                        ContainerOrTruckSize.DeleteContainerOrTruckSize(this.number, con, tran);

                    //audit log
                    AuditLog auditLog = new AuditLog(this.Number, "TR", "SETUP", 0, userID, DateTime.Now, "VEHICLE_SETUP", 0, FormMode.Delete.ToString());
                    auditLog.WriteAuditLog(this, null, con, tran);
                }


                tran.Commit();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public bool EditVehicle(string formName, string userID)
        {
            SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                Vehicle oldVehicle = VehicleDAL.GetVehicle(this.number);
                if (this.ValidateAddVehicle())
                {
                    //20130619- gerry added 1 more validation for vehicle license
                    //if (IsLicenseEnough(DateTime.Now))
                    //{
                    VehicleDAL.EditVehicle(this, con, tran);
                    //}
                    if (this.Vtype == VehicleType.Truck)
                    {
                        ContainerOrTruckSize truckSize = ContainerOrTruckSize.GetContainerOrTruckSize(this.number, con, tran);
                        if (truckSize == null)
                            ContainerOrTruckSize.AddContainerOrTruckSize(this.containerOrTruckSize, con, tran);
                        else
                            ContainerOrTruckSize.EditContainerOrTruckSize(this.containerOrTruckSize, con, tran);
                    }

                    //audit log
                    AuditLog auditLog = new AuditLog(this.Number, "TR", "SETUP", 0, userID, DateTime.Now, "VEHICLE_SETUP", 0, FormMode.Edit.ToString());
                    auditLog.WriteAuditLog(this, oldVehicle, con, tran);
                }
                tran.Commit();
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { con.Close(); }
            return true;
        }

        public static bool VerifyVehicle(string vehicleID)
        {
            return VehicleDAL.VerifyAddVehicle(vehicleID);
        }

        public bool ValidateAddVehicle()
        {
            if (type == VehicleType.Trailer)
            {
                if (trailerType == "")
                {
                    throw new FMException(TptResourceBLL.ErrTrailerTypeBlank);
                }
            }

            if (tareWeight <= 0)
            {
                throw new FMException(TptResourceBLL.ErrTareWeightLessThanZero);
            }

            if (maximumLadenWeight <= 0)
            {
                throw new FMException(TptResourceBLL.ErrMaxLadenWeightLessThanZero);
            }

            if (tareWeight > maximumLadenWeight)
            {
                throw new FMException(TptResourceBLL.ErrTareWeightMoreThanMaxLadenWeight);
            }

            if ((type == VehicleType.Truck) && (volCapacity<=0))
            {
                throw new FMException(TptResourceBLL.ErrVolCapacityisZero);
            }

            return true;
        }

        public bool ValidateDeleteVehicle()
        {
            bool flag = true;
            try
            {

                flag = this.IsVehicleUsedByDriver();
                if (this.IsVehicleUsedInPlanning() == false)
                {
                    flag = false;
                }
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
        

        public bool IsVehicleUsedByDriver()
        {
            bool flag = true;
            flag = VehicleDAL.IsVehicleUsedByDriver(this);
            if (flag == true)
            {
                throw new FMException(TptResourceBLL.ErrVehicleUsedInsideDriver);
            }
            return flag;
        }

        public bool IsVehicleUsedByTrailerLocation()
        {
            bool flag = true;
            flag = VehicleDAL.IsVehicleUsedByTrailerLocation(this);
            if (flag == true)
            {
                throw new FMException("Trailer is still attached to prime mover. ");
            }
            return flag;
        }

        public bool IsVehicleUsedInPlanning()
        {
            bool flag =  VehicleDAL.IsVehicleUsedInPlanning(this);
            if (flag == true)
            {
                throw new FMException(TptResourceBLL.ErrVehicleUsedInPlanning);
            }
            return flag;
        }
        //start Feb 21, 2011 - Gerry Added Methods for  criteria and new deleted
        public bool DeleteVehicleNew(out string message, string formName, string userID)
        {
            try
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                message = "";
                if (IsVehicleUsedByDriver())
                    return false;
                else if (IsVehicleUsedInPlanning())
                    return false;
                else
                {
                    VehicleDAL.DeleteVehicle(this.Number, con, tran);

                    //audit log
                    AuditLog auditLog = new AuditLog(this.Number, "TR", "SETUP", 0, userID, DateTime.Now, "VEHICLE_SETUP", 0, FormMode.Delete.ToString());
                    auditLog.WriteAuditLog(null, null, con, tran);

                    tran.Commit();
                }
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return true;
        }

        public static SortableList<Vehicle> GetVehiclesForCriteria(string vehicleNo, VehicleType VehicleType)
        {
            return VehicleDAL.GetVehiclesForCriteria(vehicleNo, VehicleType);
        }
        //end

        public static SortableList<Vehicle> GetAllAvailableVehiclesForDefault()
        {
            return VehicleDAL.GetAllAvailableVehiclesForDefault();
        }
        //20141117 - gerry added method to get trailer from previous leg
        public static Vehicle GetTrailerFromPreviousLeg(int jobid, string containerNo)
        {
            return VehicleDAL.GetTrailerFromPreviousLeg(jobid, containerNo);
        }
        public static Vehicle GetTrailerFromPreviousLeg(int jobid, int seqNo)
        {
            return VehicleDAL.GetTrailerFromPreviousLeg(jobid, seqNo);
        }
        //20130619 - Licensing by vehicle start

        public static SortableList<Vehicle> GetAllAvailableVehiclesForLicense()
        {
            return VehicleDAL.GetAllAvailableVehiclesForLicense();
        }   

        // 2014-02-20 Zhou Kai comments out this function to disable license check
        //this method user when add/edit vehicle which type is not trailer
        public bool IsLicenseEnough(DateTime appDate)
        {
            bool retValue = true;
            if (this.type != VehicleType.Trailer)
            {
                try
                {
                    Vehicle origVehicle = VehicleDAL.GetVehicle(this.number);
                    SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                    TPTCompany company = TPTCompany.GetCompanyByName(VehicleDAL.GetCompanyName(), con.Database);
                    if (company != null)
                    {
                        if (company.HasValidLicense(appDate))
                        {
                            int balLicense = company.GetBalLicense(appDate);
                            //this case for edit available vehicle
                            if (this.number.Trim().ToUpper().Equals(origVehicle.number.Trim().ToUpper()) && origVehicle.isAvailable)
                            {
                                if (balLicense < 0)
                                    throw new FMException(TptResourceBLL.ErrNoOfLicenseExceed);   
                            }
                            else //case for add new vehicle or edit from unavailable to available
                            {
                                if (balLicense <= 0)
                                    throw new FMException(TptResourceBLL.ErrNoOfLicenseExceed);
                            }
                        }
                        else
                            throw new FMException(TptResourceBLL.ErrNoValidLicense);
                    }
                    else
                        throw new FMException(TptResourceBLL.ErrNoValidLicense);
                }
                catch (FMException fmEx)
                {
                    throw fmEx;
                }
                catch (Exception ex)
                {
                    throw new FMException(ex.ToString());
                }
            }
            return retValue;
        }
        // 2014-02-20 Zhou Kai ends

        //20140320 - gerry added to check if license exceed before starting
        public static bool HasExceedLicense(DateTime appDate)
        {
            bool retValue = false;
            try
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                TPTCompany company = TPTCompany.GetCompanyByName(VehicleDAL.GetCompanyName(), con.Database);
                if (company.GetBalLicense(appDate.Date) <= 0)
                    throw new FMException(TptResourceBLL.ErrVehicleCountExceedAllowedLicense); 
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;
        }
        //20140619 - gerry added 1 more parameter, for the case of auto select all drivers
        //all drivers must have default vehicle
        public static bool HasExceedLicenseForTotalVehicles(DateTime appDate, int totalVehicles)
        {
            bool retValue = false;
            try
            {
                SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
                TPTCompany company = TPTCompany.GetCompanyByName(VehicleDAL.GetCompanyName(), con.Database);
                int balLicense = company.GetBalLicense(appDate.Date);
                if (balLicense < totalVehicles)
                    throw new FMException(String.Format(TptResourceBLL.ErrLicenseExceedTotalVehicle, balLicense, totalVehicles));   
            }
            catch (FMException ex) { throw ex; }
            catch (Exception ex) { throw new FMException(ex.ToString()); }
            return retValue;
        }


        //2014-02-21 Zhou Kai adds function to check if the given vehicle already exists
        bool IsVehicleNOExisting(string vehicleNO)
        {
            return VehicleDAL.IsVehicleNOExisting(vehicleNO);
        }
        // 2014-02-21 Zhou Kai ends

        public static void UpdateCargoSpaceId(Vehicle truck)
        {
            using (SqlConnection con = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString()))
            {
                SqlTransaction tran = null;
                try
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }
                    tran = con.BeginTransaction();

                    VehicleDAL.UpdateCargoSpaceId(truck, con, tran);

                    tran.Commit();
                }
                catch (FMException fmEx) { if (tran != null) { tran.Rollback(); } throw fmEx; }
                catch (Exception ex) { if (tran != null) { tran.Rollback(); } throw ex; }
                finally { con.Close(); }
            }        
        }
        
        ///<Sunmmary>
        ///Update the location of the truck or prime mover where is the last parking place
        ///
        /// 
        ///



    }
}
