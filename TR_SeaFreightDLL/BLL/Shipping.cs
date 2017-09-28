using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_SeaFreightDLL.DAL;

namespace FM.TR_SeaFreightDLL.BLL
{
    /// <summary>
    /// 2014-06-23 Zhou Kai creates this class
    /// </summary>
    public class Shipping
    {
        #region "Properties"
        public string shippingCode {get;set;}
        public string shippingName {get;set;}
        #endregion

        #region "Constructors"
        public Shipping()
        {
            shippingCode = String.Empty;
            shippingName = String.Empty;
        }
        public Shipping(string shippingCode, string shippingName)
        {
            this.shippingCode = shippingCode;
            this.shippingName = shippingName;
        }
        #endregion

        #region "Methods"
        public SortableList<Shipping> GetAllShipping()
        {
            return SeaFreightDAL.GetAllShippings();
        }

        #endregion
    }
}
