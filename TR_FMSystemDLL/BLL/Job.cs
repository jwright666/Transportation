using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class Job
    {
        public string jobNumber { get; set; }
        public string source { get; set; }
        public string masterNumber { get; set; }
        public string houseNumber { get; set; }
        public string branchCode { get; set; }
        public string systemJobRelation { get; set; }
        public decimal totalBilledAmount { get; set; }
        public decimal totalReceviedAmount { get; set; }
        public decimal totalInvoicedAmount { get; set; }
        public decimal totalPaidAmount { get; set; }
        public string openBy { get; set; }
        public DateTime openDateTime { get; set; }
        public int jobMonth { get; set; }
        public int jobYear { get; set; }
        public string costCentre { get; set; }
        public string miscFlag { get; set; }
        public string salesMan { get; set; }
        public string changeByWho { get; set; }
        public string changeDateTime { get; set; }

        //public string miscFlag { get; set; }
        //public string salesManCode { get; set; }
        public SortableList<OperatorDTO> operatorDTO;

        //public SortableList<OperatorDTO> OperatorDTO
        //{
        //    get { return OperatorDTO; }
        //    set { OperatorDTO = value; }
        //}
        public Job() 
        {
            this.branchCode  = string.Empty;
            this.jobNumber = string.Empty;
            this.source =string.Empty; 
            this.systemJobRelation =string.Empty;
            this.houseNumber =string.Empty;
            this.masterNumber = string.Empty;
            this.totalBilledAmount = 0;
            this.totalInvoicedAmount = 0;
            this.totalPaidAmount = 0;
            this.totalReceviedAmount = 0;
            this.salesMan = string.Empty;
            this.openDateTime = DateTime.Now;
            this.miscFlag = string.Empty;
            this.jobMonth = DateTime.Now.Month;
            this.jobYear = DateTime.Now.Year;
            this.costCentre = string.Empty; 
           
            //this.miscFlag = string.Empty;
            //this.salesManCode = string.Empty;
            //this.OperatorDTO = new SortableList<OperatorDTO>();
            
           
           
        }

        public Job(string branchCode ,string jobNumber , 
                   string systemJobRelation, string houseNumber,
                   string masterNumber)
        {
            this.branchCode = branchCode;
            this.houseNumber = houseNumber;
            this.jobNumber = jobNumber;
            this.masterNumber = masterNumber;
            //this.miscFlag = miscFlag;
            //this.salesManCode = salesManCode;
            //this.source = source;
            this.systemJobRelation = systemJobRelation;
            //this.OperatorDTO = operatorDTo;
        }

        public static SortableList<Job> GetJobNoListByBranchCodeAndSystemJobRelation(string branchCode, string systemJobRelation)
        {
            return JobDAL.GetJobByBranchCodeAndSystemJobRelation(branchCode, systemJobRelation);
        }

        public static SortableList<Job> GetJobNoListByBranchCodeAndJobPrefix(string branchCode, string jobprefix)
        {
            return JobDAL.GetJobNoListByBranchCodeAndJobPrefix(branchCode, jobprefix);
        }

        public static List<string> GetJobNosByBranchCodeAndJobPrefix(string branchCode, string jobprefix)
        {
            return JobDAL.GetJobNosByBranchCodeAndJobPrefix(branchCode, jobprefix);
        }

        public static List<string> GetMasterNosByBranchCodeAndJobPrefix(string branchCode, string jobprefix)
        {
            return JobDAL.GetMasterNosByBranchCodeAndJobPrefix(branchCode, jobprefix);
        }

        public static List<string> GetHouseNosByBranchCodeAndJobPrefix(string branchCode, string jobprefix)
        {
            return JobDAL.GetHouseNosByBranchCodeAndJobPrefix(branchCode, jobprefix);
        }
     }
}
