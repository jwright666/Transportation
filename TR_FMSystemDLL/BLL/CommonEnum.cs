using System;
using System.Collections.Generic;
using System.Text;

namespace FM.TR_FMSystemDLL.BLL
{
    public enum JobStatus : int
    {
        Booked = 1,
        Cancelled = 2,
        Billed = 3,
        Completed = 4,
        Closed = 5
    }
    public enum JobTripStatus : int
    {
        Booked = 1,
        Ready = 2,
        Assigned = 3,
        Completed = 4,
        Invoiced = 5
    }
    public enum UOM_TYPE : int 
    {
        Weight =1,
        Volume =2, 
        Others =3
    }
    public enum TruckMovementUOM_WtVol : int 
    {
        KGM = 1,    //weight
        MT = 2,     //weight
        CBM = 3   //volume
        
    }
    public enum Trip_Type : int
    { 
        OTO = 1, // one origin to one destination
        OTM = 2, // one origin to multiple destination
        MTO = 3  // many origin to one destination
    }
    public enum Job_Type : int
    {
        LO = 1,  //for local job
        SE = 21, //for sea export job
        SI = 22, //for sea import job
        AE = 31, //for air export job
        AI = 32, //for air import job
        CN = 4   //for Consignment Note
    }
    public enum Leg_Type : int
    {
        OneLeg = 1, //for one stop job
        TwoLegs = 2, //
        FirstOfTwoLeg = 21,
        SecondOfTwoLeg = 22,
        MoreThanTwoLegs = 3
    }
    //20161209 - gerry added
    public enum TIME_BOUND
    {
        NONE = 0, //no specific
        FIXED_PICKUP = 1, //specific time for pickup
        FIXED_DELIVERY = 2, //specific time for delivery
        AM_PICKUP = 3, //00:00:00 - 11:59:00 must pickup
        AM_DELIVERY = 4, //00:00:00 - 11:59:00 must deliver
        PM_PICKUP = 5, //12:00:00 - 23:59:59 must pickup
        PM_DELIVERY = 6 //12:00:00 - 23:59:59 must deliver
    }

}
