using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace TR_LicenseDLL.Base
{
    public abstract class NoOfLicense
    {              
        public string GroupName { get; set; }
        public string CompanyName { get; set; }
        public int numOfLicense { get; set; }
        public DateTime dateAdded { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime? validTo { get; set; }

        public NoOfLicense()
        {
            this.GroupName = string.Empty;
            this.CompanyName = string.Empty;
            this.numOfLicense = 0;
            this.dateAdded = DateTime.Now;
            this.validFrom = DateTime.Now;
            this.validTo = null;
        }

        public virtual void IsValidToAddLicense(NoOfLicense newLicense)
        {
            if (this.validFrom.Date >= newLicense.validFrom.Date)
            {
                throw new FMException("Effective date must be after the effective date of the previous license. ");
            }
            //Add more validation in future
        }
        public virtual void IsValidToEditLicense(NoOfLicense previousLicense)
        {
            if (this.validFrom.Date <= previousLicense.validFrom.Date)
            {
                throw new FMException("Effective date must be after the effective date of the previous license. ");
            }
            //Add more validation in future
        }
    }
}
