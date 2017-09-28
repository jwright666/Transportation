using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FM.TR_MaintenanceDLL.BLL
{
    public enum DriverIncentiveType : int
    {
        Haulage = 1,
        Truck = 2
    }

    public enum TrailerStatus : int
    {
        Assigned = 1,
        CustomerStuff = 2,
        CustomerUnstuff = 3,
        TrailerPark = 4,
        Unavailable = 5,
        CompleteStuffUnstuff = 6
    }

    public enum VehicleType : int
    {
        PrimeMover = 1,
        Trailer = 2,
        Truck = 3,
    }

    public enum EmployeeStatus : int
    {
        Available = 1,
        Resigned = 2,
       
    }

    public enum DeptType : int
    { 
        Haulage = 1,
        Trucking = 2
    }
    public enum VehiclePriority : int
    {
        Main_Land = 1,
        DirectLoading = 2,
        ByCustomer = 3,
        ByLocation = 4,
        UnAvailable = 5
    }

}
