using System;
using System.Collections.Generic;
using System.Text;
using FM.TransportBook.DAL;

namespace FM.TransportBook.BLL 
{
    public class Stop
    {
        private string code;
        private string description;
        private string address1;
        private string address2;
        private string address3;
        private string address4;
        private string city;

        public Stop()
        {
            this.code = "";
            this.description = "";
            this.address1 = "";
            this.address2 = "";
            this.address3 = "";
            this.address4 = "";
            this.city = "";
        }
        
        public Stop(string code, string description, string address1, string address2, string address3, string address4, string city)
        {
            this.Code = code;
            this.Description = description;
            this.Address1 = address1;
            this.Address2 = address2;
            this.Address3 = address3;
            this.Address4 = address4;
            this.City = city;
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }
	

        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }

        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }

        public string Address3
        {
            get { return address3; }
            set { address3 = value; }
        }

        public string Address4
        {
            get { return address4; }
            set { address4 = value; }
        }


        public string Description
        {
            get { return description; }
            set { description = value; }
        }
	

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public static List<Stop> GetAllStops()
        {
            return StopDAL.GetAllStops();
 
        }

        public static Stop GetStop(string code)
        {
            return StopDAL.GetStop(code);

        }

        public override string ToString()
        {
            return this.Code + ", " + this.Description;
        }
    }
}
