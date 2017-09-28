using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_HLPlanDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;


namespace FM.TR_HLPlanDLL.BLL
{
    public abstract class PlanTrip
    {
        private string planTripNo;
        private DateTime scheduleDate;
        private Vehicle vehicle;
        private Driver driver;
        private string vehicleNumber;
        private byte[] updateVersion;
        public string dept { get; set; }
        public string DisplayForPlanning_PrimeMover { get; set; }
        public string DisplayForPlanning_DriverCode { get; set; }
        public string DisplayForPlanning_Both { get; set; }

        public string PlanTripNo
        {
            get { return planTripNo; }
            set { planTripNo = value; }
        }

        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
        }

        public Vehicle Vehicle
        {
            get { return vehicle; }
            set { vehicle = value; }
        }

        public Driver Driver
        {
            get { return driver; }
            set { driver = value; }
        }

        public string VehicleNumber
        {
            get { return vehicleNumber; }
            set { vehicleNumber = value; }
        }

        public byte[] UpdateVersion
        {
            get { return updateVersion; }
            set { updateVersion = value; }
        }

        public PlanTrip()
        {
            this.dept = string.Empty;
            this.planTripNo = "";
            this.scheduleDate = DateTime.Today;
            this.vehicle = new Vehicle();
            this.driver = new Driver();
            this.vehicleNumber = "";
            this.updateVersion = new byte[8];
            this.DisplayForPlanning_PrimeMover = string.Empty;
            this.DisplayForPlanning_DriverCode = string.Empty;
            this.DisplayForPlanning_Both = string.Empty;
        }

        public PlanTrip(string planTripNo, DateTime scheduleDate, Vehicle vehicle, 
            Driver driver, byte[] updateVersion)
        {
            this.PlanTripNo = planTripNo;
            this.ScheduleDate = scheduleDate;
            this.Vehicle = vehicle;
            this.Driver = driver;
            this.VehicleNumber = vehicle.Number;
            this.UpdateVersion = updateVersion;
            this.DisplayForPlanning_PrimeMover = driver.defaultVehicleNumber;
            this.DisplayForPlanning_DriverCode = driver.Code;
            this.DisplayForPlanning_Both = driver.defaultVehicleNumber + "(" + driver.Code + ")";
        }

/*
        public static SortableList<DateTime> GetPlanTripDates(DateTime fromDate, DateTime toDate)
        {
            return PlanTransportDAL.GetPlanTripDates(fromDate, toDate);
        }
        */


    }


}
