using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class Salesman
    {
        private string salesmanCode;
        private string salesmanName;

        public string SalesmanCode
        {
            get { return salesmanCode; }
        }

        public string SalesmanName
        {
            get { return salesmanName; }
        }

        public Salesman(string salesmanCode, string salesmanName)
        {
            this.salesmanCode = salesmanCode;
            this.salesmanName = salesmanName;
        }


        public static SortableList<Salesman> GetAllSalesman()
        {
            return SalesmanDAL.GetAllSalesman();
        }

    }
}
