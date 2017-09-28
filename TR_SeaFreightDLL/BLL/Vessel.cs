using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_SeaFreightDLL.DAL;

namespace FM.TR_SeaFreightDLL.BLL
{
    /// <summary>
    /// 2014-06-23 Zhou Kai creates this class
    /// </summary>
    public class Vessel
    {
        #region "properties"
        public string vesselCode {get;set;}
        public string vesselName {get;set;}
        #endregion

        #region "Constructors"
        public Vessel()
        {
            vesselCode = String.Empty;
            vesselName = String.Empty;
        }

        public Vessel(string vesselCode, string vesselName)
        {
            this.vesselCode = vesselCode;
            this.vesselName = vesselName;
        }
        #endregion

        #region "Methods"
        public SortableList<Vessel> GetAllvessels()
        {
            return SeaFreightDAL.GetAllVessels();
        }
        #endregion
    }
}
