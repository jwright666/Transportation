using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TR_LicenseDLL.Base;
using FM.TR_FMSystemDLL.BLL;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.DAL;
using System.Data;
using TR_LicenseDLL.DAL.Transport;
using System.Collections;

namespace TR_LicenseDLL.BLL.Transport
{
    public class TPTVehicleNoOfLicense : NoOfLicense
    {   
        //in future we may add more properties to this class

        public TPTVehicleNoOfLicense() : base() { }
    }
}
