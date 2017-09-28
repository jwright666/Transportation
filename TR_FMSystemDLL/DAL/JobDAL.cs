using System;
using System.Data;
using System.Data.SqlClient; 
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL; 

namespace FM.TR_FMSystemDLL.DAL
{
    internal class JobDAL
    {

        internal static SortableList<Job> GetJobByBranchCodeAndSystemJobRelation(string branchCode,string systemRelationKey)
        { 
            SqlConnection cn =new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SortableList<Job> jobs = new SortableList<Job>();
            string sqlGetJob = " Select M.Job_Number as Job_Number,M.Master_Number as Master_Number,M.House_Number as House_Number,M.Branch_Code as Branch_Code,M.System_Job_Relation as System_Job_Relation from AM_Job_Master_Tbl M " + 
                               " where Branch_Code = '" + branchCode + "'" +
                               " and System_Job_Relation like '" + systemRelationKey +  "%'"  ;  
            SqlCommand cmd = new SqlCommand(sqlGetJob,cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            { 
                jobs.Add(BindingJobDetail(reader));
            }
            cn.Close();
            return jobs; 
        }

        internal static SortableList<Job> GetJobNoListByBranchCodeAndJobPrefix(string branchCode, string jobprefix)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            SortableList<Job> jobs = new SortableList<Job>();
            string sqlGetJob = " Select M.Job_Number as Job_Number,M.Master_Number as Master_Number,M.House_Number as House_Number,M.Branch_Code as Branch_Code,M.System_Job_Relation as System_Job_Relation from AM_Job_Master_Tbl M " +
                               " where Branch_Code = '" + branchCode + "'" +
                               " and Job_number like '" + jobprefix + "%'";
            SqlCommand cmd = new SqlCommand(sqlGetJob, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                jobs.Add(BindingJobDetail(reader));
            }
            cn.Close();
            return jobs;
        }

        internal static Job BindingJobDetail(IDataReader reader)
        {
            Job job = new Job((string)reader["Branch_Code"],(string)reader["Job_Number"],
                              (string)reader["System_Job_Relation"], (string)reader["House_Number"], 
                              (string)reader["Master_Number"]);
           
            return job;
        }

        //22 Aug, 2011 - Gerry Added this method for Job Number sources
        //List string instead of List Job object

        internal static List<string> GetJobNosByBranchCodeAndJobPrefix(string branchCode, string jobprefix)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            List<string> jobs = new List<string>();
            string sqlGetJob = " Select Job_Number from AM_Job_Master_Tbl " +
                               " where Branch_Code = '" + branchCode + "'" +
                               " and Job_number like '" + jobprefix + "%'";
            SqlCommand cmd = new SqlCommand(sqlGetJob, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string jobNo = reader.GetString(0).Trim();
                if (!jobNo.Equals(string.Empty))
                    jobs.Add(jobNo);
            }
            cn.Close();
            return jobs;
        }
        internal static List<string> GetMasterNosByBranchCodeAndJobPrefix(string branchCode, string jobprefix)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            List<string> masterNoList = new List<string>();
            string sqlGetJob = " Select Master_Number from AM_Job_Master_Tbl " +
                               " where Branch_Code = '" + branchCode + "'" +
                               " and Job_number like '" + jobprefix + "%'";
            SqlCommand cmd = new SqlCommand(sqlGetJob, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string masterNo = reader.GetString(0).Trim();
                if(!masterNo.Equals(string.Empty))
                    masterNoList.Add(masterNo);
            }
            cn.Close();
            return masterNoList;
        }
        internal static List<string> GetHouseNosByBranchCodeAndJobPrefix(string branchCode, string jobprefix)
        {
            SqlConnection cn = new SqlConnection(FMGlobalSettings.TheInstance.getConnectionString());
            List<string> houseNoList = new List<string>();
            string sqlGetJob = " Select House_Number from AM_Job_Master_Tbl " +
                               " where Branch_Code = '" + branchCode + "'" +
                               " and Job_number like '" + jobprefix + "%'";
            SqlCommand cmd = new SqlCommand(sqlGetJob, cn);
            cn.Open();
            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string houseNo = reader.GetString(0).Trim();
                if (!houseNo.Equals(string.Empty))
                    houseNoList.Add(houseNo);
            }
            cn.Close();
            return houseNoList;
        }
        
        //End Gerry Added

      
    }
}
