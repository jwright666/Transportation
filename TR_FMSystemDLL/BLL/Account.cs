using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class Account
    {
        private string accountCode;
        private string accountDesc;

        public Account()
        {
            this.accountCode =AccountCode;
            this.accountDesc = AccountDesc;
        }
        public Account(string AccountCode, string AccountDesc)
        {
            this.accountCode = AccountCode;
            this.accountDesc = AccountDesc;
        }

        public string AccountCode
        {
            get { return accountCode; }
        }

        public string AccountDesc
        {
            get { return accountDesc; }
        }

        // static methods
        public static SortableList<Account> GetAllAccounts()
        {
            return AccountDAL.GetAccountInfo(); 
            //return BranchDAL.GetAllBranches();
        }

        //public static SortableList<BranchDTO> GetBranchCodeByUser(string userID)
        //{
        //    return BranchDAL.GetBranchsByUserName(userID);
        //}
    }
}
