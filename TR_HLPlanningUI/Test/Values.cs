using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//

namespace FM.TransportPlanDLL.Test
{
    public class Values
    {
        #region "Private members"
        private string id;
        private string _value;
        private string remark;
        private string additional_remark;

        #endregion

        #region "Public members"
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        #endregion

        #region "Constructors"
        public Values()
        {
            // initialize with default values of the type
        }

        public Values(string the_id, string the_value, string the_remark, string the_additional_remark)
        {
            this.id = the_id;
            this._value = the_value;
            this.remark = the_remark;
            this.additional_remark = the_additional_remark;
        }

        #endregion

        #region "Methods"
        public void InsertAValue(Values theValue)
        {
            DAL.InsertAValue(theValue);

            return;
        }

        #endregion
    }
}