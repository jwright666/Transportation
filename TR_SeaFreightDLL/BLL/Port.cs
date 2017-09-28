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
    public class Port
    {
        #region "properties"
        public string portCode {get;set;}
        public string portName {get;set;}
        #endregion

        #region "Constructors"
        public Port()
        {
            portCode = String.Empty;
            portName = String.Empty;
        }

        public Port(string portCode, string portName)
        {
            this.portCode = portCode;
            this.portName = portName;
        }
        #endregion

        #region "Methods"
        public SortableList<Port> GetAllPorts()
        {
            return SeaFreightDAL.GetAllPorts();
        }
        #endregion
    }
}
