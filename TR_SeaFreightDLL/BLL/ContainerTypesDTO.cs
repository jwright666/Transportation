using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_SeaFreightDLL.DAL;
using FM.TR_FMSystemDLL.BLL;

namespace FM.TR_SeaFreightDLL.BLL
{
    public class ContainerTypesDTO
    {
        private string  code;
        private string  description;
        private string size;
        private decimal maxLadenWeight;
        private decimal tareWeight;
        private decimal  tareCBM;
        private decimal  length;
        private decimal  height;
        private decimal  width;

        public ContainerTypesDTO()
        {
            this.code = "";
            this.description = "";
            this.size = "";
            this.maxLadenWeight = 0;
            this.tareWeight = 0;
            this.tareCBM = 0;
            this.length = 0;
            this.height = 0;
            this.width = 0;
        }

        public ContainerTypesDTO(string code, string description, string size, decimal maxLadenWeight,
            decimal tareWeight, decimal tareCBM, decimal length, decimal height, decimal width)
        {
            this.code = code;
            this.description = description;
            this.size = size;
            this.maxLadenWeight = maxLadenWeight;
            this.tareWeight = tareWeight;
            this.tareCBM = tareCBM;
            this.length = length;
            this.height = height;
            this.width = width;

        }

        public decimal  Width
        {
            get { return width; }
        }
	     
        public decimal  Height
        {
            get { return height; }
        }
	
        public decimal Length
        {
            get { return length; }
        }
	 
        public decimal  TareCBM
        {
            get { return tareCBM; }
        }
	
        public decimal  TareWeight
        {
            get { return tareWeight; }
        }

        public decimal MaxLadenWeight
        {
            get { return maxLadenWeight; }
        }
	
        public string Size
        {
            get { return size; }
        }
	

        public string  Description
        {
            get { return description; }
        }
	


        public string  Code
        {
            get { return code; }
        }
	
        // static methods

        public static List<ContainerTypesDTO> GetAllContainerDTO()
        {
            //delegates method call to SeaFreightDAL
            return SeaFreightDAL.GetAllContainerTypes();
        }

        public static ContainerTypesDTO GetContainerDTO(string code)
        {
            //delegates method call to SeaFreightDAL
            return SeaFreightDAL.GetContainerType(code);
        }

        /// <summary>
        /// 2014-08-06 Zhou Kai adds this function to get container size
        /// from container code
        /// </summary>
        /// <param name="code">The container code</param>
        /// <returns>The container size</returns>
        public static string GetContainerSizeFromContainerCode(string code)
        {
            return SeaFreightDAL.GetContainerSizeFromContainerCode(code);
        }

        // 2014-07-30 Zhou Kai adds parameter noOfLegs to this function
        //20140124 - gerry added 
        public static List<ContainerTypesDTO> GetContainerDTOBySizeUsedInQuotationAndJobTrips(int quotationID, bool stopDependent, int noOfLegs)
        {
            //used in Quotations and Job Trips
            return SeaFreightDAL.GetContainerDTOBySizeUsedInQuotationAndJobTrips(quotationID, stopDependent, noOfLegs);
        }

        // 2014-07-30 Zhou Kai adds the parameter noOfLegs
        //16 Aug, 2011 Gerry Add
        public static List<string> GetContainerCodesBySizeUsedInQuotationAndJobTrips(int quotationID, bool stopDependent, int noOfLegs)
        {
            //used in Quotations and Job Trips
            return SeaFreightDAL.GetContainerCodesBySizeUsedInQuotationAndJobTrips(quotationID, stopDependent, noOfLegs);
        }
        public static List<string> GetContainerSizes()
        {
            //used in Quotations and Job Trips
            return SeaFreightDAL.GetContainerSizes();
        }

        public static List<string> GetAllContainerCodes()
        {
            return SeaFreightDAL.GetAllContainerCodes();
        }
        //16 Aug, 2011 End

        //20150813 - gerry added for container checking
        //logic based from delphi code
        public static bool CheckValidContainerNo(string containerNo)
        {
            //container number ISO checking options
            //
            bool needToCheck = ApplicationOption.GetApplicationOption(ApplicationOption.HAULAGE_SETTINGS_ID, ApplicationOption.SETTINGS_CHECK_CONTAINER_ISO).setting_Value == "T" ? true : false;
            if (needToCheck)
            {
                //removed space before and after container number; space in the middle will be counted
                containerNo = containerNo.Trim();
                if (!containerNo.Equals(string.Empty))
                {
                    int[] iMult = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512 };
                    int iCDTot = 0;
                    int iCD = 0;

                    if (containerNo.Length != 11) { return false; }

                    for (int i = 0; i < 10; i++)
                    {
                        iCDTot += GetCharValue(containerNo[i]) * iMult[i];
                    }
                    iCD = iCDTot % 11;
                    int lastDigit = Convert.ToInt32(containerNo[containerNo.Length - 1].ToString());
                    if (lastDigit != 0)
                    {
                        if (iCD != lastDigit) { return false; }
                    }
                }
            }
            return true;
        }
        private static int GetCharValue(char c)
        {
            int iValue = 0;
            switch (c)
            {
                case '0': iValue = 0; break;
                case '1': iValue = 1; break;
                case '2': iValue = 2; break;
                case '3': iValue = 3; break;
                case '4': iValue = 4; break;
                case '5': iValue = 5; break;
                case '6': iValue = 6; break;
                case '7': iValue = 7; break;
                case '8': iValue = 8; break;
                case '9': iValue = 9; break;
                case 'A': iValue = 10; break;
                case 'B': iValue = 12; break;
                case 'C': iValue = 13; break;
                case 'D': iValue = 14; break;
                case 'E': iValue = 15; break;
                case 'F': iValue = 16; break;
                case 'G': iValue = 17; break;
                case 'H': iValue = 18; break;
                case 'I': iValue = 19; break;
                case 'J': iValue = 20; break;
                case 'K': iValue = 21; break;
                case 'L': iValue = 23; break;
                case 'M': iValue = 24; break;
                case 'N': iValue = 25; break;
                case 'O': iValue = 26; break;
                case 'P': iValue = 27; break;
                case 'Q': iValue = 28; break;
                case 'R': iValue = 29; break;
                case 'S': iValue = 30; break;
                case 'T': iValue = 31; break;
                case 'U': iValue = 32; break;
                case 'V': iValue = 34; break;
                case 'W': iValue = 35; break;
                case 'X': iValue = 36; break;
                case 'Y': iValue = 37; break;
                case 'Z': iValue = 38; break;
            }
            return iValue;
        }

    }
}
