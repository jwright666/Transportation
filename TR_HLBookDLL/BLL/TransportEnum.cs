using System;
using System.Collections.Generic;
using System.Text;

namespace FM.TR_HLBookDLL.BLL
{


    public enum JobChargeStatus : int
    {
        Booked = 1,
        Completed = 2,
        Invoiced = 3
    }

    public enum JobUrgencyType : int
    {
        Normal = 1,
        Important = 2,
        Urgent = 3
    }

  

    /// <summary>
    /// 2014-10-04 Zhou Kai adds new enum for 
    /// describing the transfer type of a haulier job.
    /// </summary>
    public enum JobTransferType : int
    {
        None = 1, // which is the default value
        TransferIn = 2, //bubbing job after transfer
        TransferOut = 3 //hubbing job before transfer
        
    }

    public enum ChargeType : int
    {
        NotStopDependent = 1,
        StopDependent = 2
    }

    // 2013-10-23 Zhou Kai adds
    public enum SUBCONCHANGES
    {
        // 2014-05-22 Zhou Kai adds
        UNKNOWN = 0, // represents an un-initialized value, it's un-usable
        // 2014-05-22 Zhou Kai ends
        BLANKTOA = 1,  // eidt truck job from by OWNTRANSPORT to by Sub-ContractorA
        ATOB = 2, // edit truck job from by Sub-ContractorA to by Sub-ContractorB
        ATOBLANK = 3, // edit truck job from by Sub-ContractorA to by OWNTRANSPORT
        NOCHANGES = 4 // no changes of sub-contractor 
    }
    // 2013-10-23 Zhou Kai ends   


}
