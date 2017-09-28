using FM.FMSystem.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TR_ConsignmentNote.BLL;
using TR_ConsignmentNote.DAL;

namespace TR_ConsignmentNote.Facade
{
    public class ConsignmentNoteFacadeOut
    {
        public static List<string> GetConsignmentNoteJobNos(string house_No)
        {
            return ConsignmentNote_Job_DAL.GetConsignmentNoteNos(house_No);
        }
        public static List<string> GetConsignmentNoteHouseNos()
        {
            return ConsignmentNote_Job_DAL.GetConsignmentNoteHouseNos();
        }

        public static ConsignmentNote_Job_Master GetConsignmentNoteJobMaster(string jobNo)
        {
            return ConsignmentNote_Job_DAL.GetConsignmentNoteJobMaster(jobNo);
        }

        public static SortableList<ConsignmentNote_Job_Goods_Details> GetConsignmentNoteJobGoodsDetails(string jobNo)
        {
            return ConsignmentNote_Job_DAL.GetConsignmentNoteJobGoodsDetails(jobNo);
        }

        public static SortableList<ConsignmentNote_Job_Goods_Dimension> GetConsignmentNoteJobGoodsDimensions(int jobID, int seqNo)
        {
            return ConsignmentNote_Job_DAL.GetConsignmentNoteJobGoodsDimensions(jobID, seqNo);
        }
    }
}
