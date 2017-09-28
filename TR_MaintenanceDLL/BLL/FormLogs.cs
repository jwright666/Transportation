using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FM.TR_MaintenanceDLL.DAL;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_MaintenanceDLL.BLL
{
    public class FormLogs
    {
        private string parentForm;
        private string parentFormDescription;
        private string childForm;
        private string childFormDescription;

        public FormLogs()
        {
            parentForm = "";
            parentFormDescription = "";
            childForm = "";
            childFormDescription = "";
        }

        public FormLogs(string parentForm, string parentFormDescription,
                            string childForm, string childFormDescription)
        {
            this.ParentForm = parentForm;
            this.ParentFormDescription = parentFormDescription;
            this.ChildForm = childForm;
            this.ChildFormDescription = childFormDescription;
        }

        public string ParentForm
        {
            get { return parentForm; }
            set { parentForm = value; }
        }

        public string ParentFormDescription
        {
            get { return parentFormDescription; }
            set { parentFormDescription = value; }
        }

        public string ChildForm
        {
            get { return childForm; }
            set { childForm = value; }
        }

        public string ChildFormDescription
        {
            get { return childFormDescription; }
            set { childFormDescription = value; }
        }

        public static SortableList<FormLogs> GetAllHeaderForms()
        {
            return FormLogsDAL.GetAllHeaderForms();
        }

        public static SortableList<FormLogs> GetChildForms(string parentForm)
        {
            return FormLogsDAL.GetChildForms(parentForm);
        }
       
    }
}
