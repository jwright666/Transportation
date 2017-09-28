using System;
using System.Collections.Generic;
using System.Text;

namespace FM.TR_MarketDLL.BLL
{
    public enum QuotationType : int
    {
        Customer = 1,
        CustomerGroup = 2
    }

    public enum InvoiceChargeType : int 
    {
        Others=1, //for non truck movement charge
        DependOnWeightVolume =2,
        DependOnTruckCapacity =3,
        DependOnPackagetype =4,
        DependOnHigherWeightOrVolume =5
    }




}
