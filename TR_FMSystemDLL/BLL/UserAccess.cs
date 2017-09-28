using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;
//using FM.FMSystem.Resources;

namespace FM.TR_FMSystemDLL.BLL
{
    public class UserAccess
    {
        private User user;
        private SortableList<ModuleItemAccess> moduleItemAccess;

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public SortableList<ModuleItemAccess> ModuleItemAccess
        {
            get { return moduleItemAccess; }
            set { moduleItemAccess = value; }
        }

        public UserAccess()
        {
            this.user = new User();
            this.moduleItemAccess = new SortableList<ModuleItemAccess>();
        }

        public static UserAccess GetUserAccess(User user)
        {
            return UserAccessDAL.GetUserAccess(user);
        }

        public static ModuleItemAccess GetUserAccessRightsForModuleItem(User user, string moduleItem)
        {
            return UserAccessDAL.GetUserAccessRightsForModuleItem(user, moduleItem);
        }

        public static bool IsModuleItemAllowed(User user, string moduleItem)
        {
            return UserAccessDAL.IsModuleItemAllowed(user, moduleItem);
        }
    }
}
