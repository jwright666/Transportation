using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class ModuleItemAccess
    {
        private string moduleItem;
        private bool add;
        private bool edit;
        private bool delete;
        private bool browse;
        private bool print;
        private bool post;

        public string ModuleItem
        {
            get { return moduleItem; }
        }

        public bool Add
        {
            get { return add; }
        }

        public bool Edit
        {
            get { return edit; }
        }

        public bool Delete
        {
            get { return delete; }
        }

        public bool Browse
        {
            get { return browse; }
        }

        public bool Print
        {
            get { return print; }
        }

        public bool Post
        {
            get { return post; }
        }

        public ModuleItemAccess()
        {
            this.moduleItem = "";
            this.add = false;
            this.edit = false;
            this.delete = false;
            this.browse = false;
            this.print = false;
            this.post = false;
        }

        public ModuleItemAccess(string moduleItem, bool add, bool edit,
            bool delete, bool browse, bool print, bool post)
        {
            this.moduleItem = moduleItem;
            this.add = add;
            this.edit = edit;
            this.delete = delete;
            this.browse = browse;
            this.print = print;
            this.post = post;
        }



    }
}
