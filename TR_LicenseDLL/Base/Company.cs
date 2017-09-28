using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_FMSystemDLL.BLL;

namespace TR_LicenseDLL.Base
{
    public abstract class Company
    {
        public string GroupName { get; set; }
        public string CompanyName { get; set; }
        public string GroupNameLocal { get; set; }
        public string CompanyNameLocal { get; set; }
        public string DatabaseName { get; set; }
        public string SQLServer { get; set; }
        public bool ShareWithGoup { get; set; }

        public Company()
        {
            this.GroupName = "";
            this.CompanyName = "";
            this.GroupNameLocal = "";
            this.CompanyNameLocal = "";
            this.DatabaseName = "";
            this.SQLServer = "";
            this.ShareWithGoup = false;
        }

        public virtual void ValidateFields()
        {
            string errMsg = string.Empty;
            if (this.DatabaseName.Trim() == string.Empty)
                errMsg += "\nDatabase name cannot be empty. ";
            if (this.CompanyName.Trim() == string.Empty)
                errMsg += "\nCompany name cannot be empty. ";
            if (this.GroupName.Trim() == string.Empty)
                errMsg += "\nGroup name cannot be empty. ";

            if (errMsg != string.Empty)
                throw new FMException(errMsg);
        }
    }
}
