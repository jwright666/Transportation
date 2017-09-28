using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;
//using FM.FMSystem.Resources;

namespace FM.TR_FMSystemDLL.BLL
{
    public class User
    {
        private string userID;
        private string firstName;
        private string lastName;
        private BranchDTO defaultBranch;
        private int defaultLanguage;

        public string UserID
        {
            get { return userID; }
        }

        public string FirstName
        {
            get { return firstName; }
        }

        public string LastName
        {
            get { return lastName; }
        }

        public BranchDTO DefaultBranch
        {
            get { return defaultBranch; }
        }

        public int DefaultLanguage
        {
            get { return defaultLanguage; }
        }

        public User()
        {
            this.userID = "";
            this.firstName = "";
            this.lastName = "";
            this.defaultBranch = new BranchDTO();
            this.defaultLanguage = 1;
        }

        public User(string userID, string firstName, string lastName, BranchDTO defaultBranch, int defaultLanguage)
        {
            this.userID = userID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.defaultBranch = defaultBranch;
            this.defaultLanguage = defaultLanguage;
        }


        public static User GetUser(string userID)
        {
            User temp = new User();
            try
            {
                temp = UserDAL.GetUser(userID);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FMException("Error accesing User table for user "+userID + ex.Message.ToString());
            }
            return temp;
        }

        public static bool ValidateUser(string userID, string serverName)
        {
            if (userID.Length == 0 || serverName.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
