using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_FMSystemDLL.BLL 
{
    // 2014-02-27 Zhou Kai adds Clone method to stop class
    public class Stop : IComparable, ICloneable
    {
        private string code;
        private string description;
        private string address1;
        private string address2;
        private string address3;
        private string address4;
        private string city;
        public string operationType { get; set; }
        public string postalCode { get; set; }
        public string countryName { get; set; }

        public Stop()
        {
            this.code = "";
            this.description = "";
            this.address1 = "";
            this.address2 = "";
            this.address3 = "";
            this.address4 = "";
            this.city = "";
            this.operationType = "";
            this.postalCode = "";
            this.countryName = "";
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
            List<OperatorDTO> operators = new List<OperatorDTO>();
            //20130819 - Gerry replaced, to get get all operators
            //operators = OperatorDTO.GetAllLocalAndNotAirlinesOperators();
            operators = OperatorDTO.GetAllOperators();

            Stop stop = new Stop();
            List<Stop> stops = new List<Stop>();
            for (int i = 0; i < operators.Count; i++)
            {
                stop = new Stop();
                stop.Code = operators[i].Code;
                stop.Description = operators[i].Name;
                stop.Address1 = operators[i].Add1;
                stop.Address2 = operators[i].Add2;
                stop.Address3 = operators[i].Add3;
                stop.Address4 = operators[i].Add4;
                stop.City = operators[i].City;
                OperationDetail det =OperationDetail.GetOperationDetail(stop.code);
                stop.operationType = det.operationTypeCode;
                stop.postalCode = det.zipCode;
                stop.countryName = det.countryName;
                stops.Add(stop);
            }
            return stops;
        }

        public static Stop GetStop(string code)
        {
            //List<OperatorDTO> operators = new List<OperatorDTO>();
            //20130819 - Gerry replaced, to get get all operators  
            OperatorDTO operatorDTO = new OperatorDTO();
            //operators = OperatorDTO.GetAllLocalAndNotAirlinesOperators(); 
            operatorDTO = OperatorDTO.GetOperatorDTO(code);
            Stop stop = new Stop();
            //for (int i = 0; i < operators.Count; i++)
            //{
            if (operatorDTO.Code.Trim() == code.Trim())
            {
                stop = new Stop();
                stop.Code = operatorDTO.Code;
                stop.Description = operatorDTO.Name;
                stop.Address1 = operatorDTO.Add1;
                stop.Address2 = operatorDTO.Add2;
                stop.Address3 = operatorDTO.Add3;
                stop.Address4 = operatorDTO.Add4;
                stop.City = operatorDTO.City;
                OperationDetail det = OperationDetail.GetOperationDetail(stop.code);
                stop.operationType = det.operationTypeCode;
                stop.postalCode = det.zipCode;
                stop.countryName = det.countryName;
            }
            //}
            return stop;
        }

        public override string ToString()
        {
            return this.Code + "(" +(this.Description != string.Empty ? ( this.Description ) : string.Empty) + "), " + this.address1;
        }

        public static List<string> GetStopCodes()
        { 
            return OperatorDTO.GetAllOperatorCodes();            
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return this.code.CompareTo(((Stop)obj).code);
        }

        #endregion

        // 2014-02-27 Zhou Kai override Clone method
        public object Clone()
        {
            // 2014-10-23, Zhou Kai clones all 7 properties
            // Stop tmpStop = new Stop();
            // tmpStop = (Stop)this.MemberwiseClone(); //20140314 - gerry change to only 2 properties to be clone
            // tmpStop.code = this.code;
            // tmpStop.description = this.description;

            return (Stop)this.MemberwiseClone(); 
        }
        // 2014-02-27 Zhou Kai ends
    }
}
