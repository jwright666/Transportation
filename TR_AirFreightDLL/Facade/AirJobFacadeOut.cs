using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TR_AirFreightDLL.BLL;
using TR_AirFreightDLL.DAL;
using System.Collections;
using FM.TR_FMSystemDLL.BLL;

namespace TR_AirFreightDLL.Facade
{
    public class AirJobFacadeOut
    {
        // Create AirJob base on the criteria
        public static AirJob GetAirJob(string jobNo, AirJobType jobType, string hawb, string mawb)
        {
            AirJob temp = null;
            try
            {
                temp = AirJobDAL.GetAirJob(jobNo, jobType, hawb, mawb);  
                //temp.airJobRates = GetAirJobRates(temp.JobID, jobType);
                //temp.AirTruckInfo = GetAirTruckInfo(temp.JobNo, jobType);
            }
            catch (FMException fmEx) { throw fmEx; }
            catch (Exception ex) { throw new FMException(ex.Message.ToString()); }


            return temp; 
        }

        //Create AirJob base on the jobType(import/export)
        public static SortableList<AirTruckInfo> GetAirTruckInfos(string jobNo, AirJobType jobType)
        {
            return AirJobDAL.GetAirTruckInfos(jobNo, jobType);
        }
        //Get the list of air job rates based on jobtype
        public static SortableList<AirJobRate> GetAirJobRates(int jobID, AirJobType jobType)
        {
            return AirJobDAL.GetAirJobRates(jobID, jobType);
        }
        //Get the list of air job rates dimension based on jobtype
        public static SortableList<AirJobDtlDimension> GetAirJobDtlDimension(int jobID, int rateSeqNo, AirJobType jobType)
        {
            return AirJobDAL.GetAirJobDtlDimension(jobID, rateSeqNo, jobType);
        }
        public static ArrayList GetCustomerCodes(AirJobType jobType)
        {
            return AirJobDAL.GetAllCustomerCodes(jobType);
        }                                                                      
        public static ArrayList GetAllJobNos(AirJobType jobType, string awbNo)
        {
            return AirJobDAL.GetAllJobNos(jobType, awbNo);
        }
        public static ArrayList GetAllAWBNos(AirJobType jobType)
        {
            return AirJobDAL.GetAllAWBNos(jobType);
        }
    }
}
