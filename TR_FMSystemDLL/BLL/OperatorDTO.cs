using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL 
{
    public class OperatorDTO
    {
        public static String[] PropertiesName = new String[] { "Code", "Name", "Add1", "Add2", "Add3", "Add4", "City" };
        public static int TotalProperties = 7;
                          
        //20140502 - gerry change the acces right for code and name to public
        //private string code;
        //private string name;

        public string Code { get; set; } 
        public string Name { get; set; } 
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Add3 { get; set; }
        public string Add4 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; } //20609117 - gerry added for 
        public string ZoneArea { get; set; }  //20609117 - gerry added for truck plan job grouping

        public OperatorDTO()
        {
            this.Code = "";
            this.Name = "";
            this.Add1 = "";
            this.Add2 = "";
            this.Add3 = "";
            this.Add4 = "";
            this.City = "";
            this.ZipCode = "";
            this.ZoneArea = "";
        }

        public OperatorDTO(string code, string name, string add1, 
            string add2, string add3, string add4, string city)
        {
            this.Code = code.Trim();
            this.Name = name.Trim();
            this.Add1 = add1.Trim();
            this.Add2 = add2.Trim();
            this.Add3 = add3.Trim();
            this.Add4 = add4.Trim();
            this.City = city.Trim();
        }

        // static methods
        public static List<OperatorDTO> GetAllOperators()
        {
            return OperatorDAL.GetAllOperators();
        }

        public static List<OperatorDTO> GetAllLocalAndNotAirlinesOperators()
        {
            return OperatorDAL.GetAllLocalAndNotAirlinesOperators();
        }

        //Chong Chin 22 April 2010 -Start -  Return string collection for faster loading
        //Then return 1 object based on the selection

        public static List<string> GetAllOperatorCodes()
        {
            return OperatorDAL.GetAllOperatorCodes();
        }

        public static List<string> GetAllOperatorNames()
        {
            return OperatorDAL.GetAllOperatorNames();
        }


        public static OperatorDTO GetOperatorDTO(string operatorDTOCode)
        {
            return OperatorDAL.GetOperatorDTO(operatorDTOCode);
        }

        public static OperatorDTO GetOperatorDTOByName(string operatorDTOName)
        {
            return OperatorDAL.GetOperatorDTOByName(operatorDTOName);
        }

        // Chong Chin 22 April 2010 End

        //20140502 - gerry added 
        public object Clone()
        {
            OperatorDTO retValue = new OperatorDTO();
            retValue.Code = this.Code;
            retValue.Name = this.Name;
            return retValue;
        }
        //20140502 - gerry added 
        public override string ToString()
        {
            return this.Code + (this.Name == string.Empty ? string.Empty : (", " + this.Name));
        }
    }
}
