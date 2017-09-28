using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class BranchDTO
    {
        private string code;
        private string name;

        public BranchDTO()
        {
            this.code = "";
            this.name = ""; 
        }
        public BranchDTO(string code, string name)
        {
            this.code = code;
            this.name = name;
        }
        
        public string Name
        {
            get { return name; }
        }
	
        public string Code
        {
            get { return code; }
        }
	
        // static methods
        public static List<BranchDTO> GetAllBranches()
        {
            return BranchDAL.GetAllBranches();
        }

        public static SortableList<BranchDTO> GetBranchCodeByUser(string userID)
        {
            return BranchDAL.GetBranchsByUserName(userID);
        }

        public static BranchDTO GetBranch(string branch_code)
        {
            return BranchDAL.GetBranch(branch_code);
        }
        //20121122 - Gerry added to display as string
        public override string ToString()
        {
            return this.code.ToString();
        }
    }

}
