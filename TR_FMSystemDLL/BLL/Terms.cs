using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class Terms
    {
        public int termCode { get; set; }
        public string termDesc { get; set; }
        public int discount { get; set; }
        public string discAsofInvDate { get; set; }
        public string discAsofEOM { get; set; }
        public string netDueAsofInvDate { get; set; }
        public string netDueAsofEOM { get; set; }
        public decimal serviceChargeRate { get; set; }
        public decimal serviceChargeMin { get; set; }

        public Terms()
        {
            this.termCode = 0;
            this.termDesc = string.Empty;
            //this.discount = 0;
            //this.netDueAsofEOM = string.Empty;
            //this.netDueAsofInvDate = string.Empty;
            //this.serviceChargeMin = 0;
            //this.serviceChargeRate = 0; 
        }
        public Terms(int termCode,string termDesc)
        {
            //this.discAsofEOM = discAsofEOM;
            //this.discAsofInvDate = discAsofInvDate;
            //this.discount = discount; 
            //this.netDueAsofEOM = netDueAsofEOM;
            //this.netDueAsofInvDate = netDueAsofInvDate;
            //this.serviceChargeMin = serviceChargeMin;
            //this.serviceChargeRate = serviceChargeRate;
            this.termCode = termCode;
            this.termDesc = termDesc;
        }

        public static SortableList<Terms> GetALLTerms()
        {
            return TermsDAL.GetALLTerms();
        }
    }
}
