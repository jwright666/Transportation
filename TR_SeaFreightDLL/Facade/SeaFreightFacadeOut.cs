using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_SeaFreightDLL.BLL;
using FM.TR_SeaFreightDLL.DAL;

namespace FM.TR_SeaFreightDLL.Facade
{
    public class SeaFreightFacadeOut
    {
        public static List<ContainerTypesDTO> GetAllContainerTypes()
        {
            return ContainerTypesDTO.GetAllContainerDTO();
        }

        public static ContainerTypesDTO GetContainerType(string code)
        {
            return ContainerTypesDTO.GetContainerDTO(code);
        }

        public static List<string> GetOutstandingSeaExportJobNos(string transporterCode, string shipmentType = "FCL")
        {
            return SeaFreightDAL.GetOutstandingSeaExportJobNos(transporterCode, shipmentType);
        }

        public static List<string> GetOutstandingSeaImportJobNos(string transporterCode, string shipmentType = "FCL")
        {
            return SeaFreightDAL.GetOutstandingSeaImportJobNos(transporterCode, shipmentType);
        }

        public static SeaExportJob GetSeaExportJob(string jobNo)
        {
            return SeaFreightDAL.GetSeaExportJob(jobNo);
        }

        public static SeaImportJob GetSeaImportJob(string jobNo)
        {
            return SeaFreightDAL.GetSeaImportJob(jobNo);
        }
        //20170807
        public static List<JobDimension> GetExportDimension(int export_number, int seqNo)
        {
            return SeaFreightDAL.GetExportDimension(export_number, seqNo);
        }
        public static List<JobDimension> GetImportDimension(int export_number, int seqNo)
        {
            return SeaFreightDAL.GetImportDimension(export_number, seqNo);
        }


        /// <summary>
        /// 2014-08-14 Zhou Kai adds this function to initialize the container code to 
        /// size mapping from SRT_Container_Tbl
        /// </summary>
        /// <returns>The initialized dictionary object, the key is the container code
        /// and the value is the container size</returns>
        public static Dictionary<string, string> GetContainerCodeSizeMapping()
        {
            return SeaFreightDAL.GetContainerCodeSizeMapping();
        }

        /// <summary>
        /// 2014-08-29 Zhou Kai adds
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ContainerTypesDTO GetContainerDTO(string code)
        {
            //delegates method call to SeaFreightDAL
            return SeaFreightDAL.GetContainerType(code);
        }

    }
}
