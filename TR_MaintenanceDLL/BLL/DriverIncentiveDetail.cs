using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.FMSystem.BLL;
using FM.TransportMaintenanceDLL.DAL;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using FM.FMSystem.DAL;

namespace FM.TransportMaintenanceDLL.BLL
{
    public class DriverIncentiveDetail
    {
        private DriverIncentiveType incentiveType;
        private bool isLaden;
        private string containerSizeCode;
        private string incentiveDescription;
        private decimal incentivePerTrip;

        public DriverIncentiveDetail()
        {
            this.incentiveType = DriverIncentiveType.Haulage;
            this.isLaden = true;
            this.containerSizeCode = "";
            this.incentiveDescription = "";
            this.incentivePerTrip = 0;
        }

        public DriverIncentiveDetail(DriverIncentiveType incentiveType, bool isLaden,
            string containerSizeCode, string incentiveDescription, decimal incentivePerTrip)
        {
            this.incentiveType = incentiveType;
            this.isLaden = isLaden;
            this.containerSizeCode = containerSizeCode;
            this.incentiveDescription = incentiveDescription;
            this.incentivePerTrip = incentivePerTrip;
        }

        public DriverIncentiveType IncentiveType
        {
            get { return incentiveType; }
            set { incentiveType = value; }
        }

        public bool IsLaden
        {
            get { return isLaden; }
            set { isLaden = value; }
        }

        public string ContainerSizeCode
        {
            get { return containerSizeCode; }
            set { containerSizeCode = value; }
        }

        public string IncentiveDescription
        {
            get { return incentiveDescription; }
            set { incentiveDescription = value; }
        }

        public decimal IncentivePerTrip
        {
            get { return incentivePerTrip; }
            set { incentivePerTrip = value; }
        }




    }
}
