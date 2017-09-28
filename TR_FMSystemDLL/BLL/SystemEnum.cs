using System;
using System.Collections.Generic;
using System.Text;

namespace FM.TR_FMSystemDLL.BLL
{
    public enum FMModule : int
    {
        SeaFreight = 1,
        AirFreight = 2,
        Accounts = 3,
        Warehouse = 4,
        Transport = 5,
        Invoicing = 6,
        Trucking = 7
    }

    public enum FormMode : int
    {
        Add = 1,
        Edit = 2,
        Delete = 3
    }

    #region "2013-12-16 Zhou Kai adds codes for Log by Property for FM82"
    public enum PropertyLevel : int
    {
        JobLevel = 1,
        JobTripLevel = 2,
        JobTripDetailLevel = 3,
        JobChargeLevel = 4,
        QuotationLevel = 5,
        TransportRateLevel = 6,
        NotSet = 7
    }
    #endregion
}
