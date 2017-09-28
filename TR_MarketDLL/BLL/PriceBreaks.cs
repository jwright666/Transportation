using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_MarketDLL.DAL;
using FM.TR_FMSystemDLL.DAL;
using TR_LanguageResource.Resources;

namespace FM.TR_MarketDLL.BLL
{
    public class PriceBreaks
    {
        

        private int seq_No;
        private decimal end;
 	  	private bool isLumpSum;
	  	private decimal lumpSumValue;
	  	private decimal rate;
        private byte[] updateVersion;
        private string remarks;

        public PriceBreaks()
        {
            this.seq_No = 0;
            this.end = 0;
            this.isLumpSum = false;
            this.lumpSumValue = 0;
            this.rate = 0;
//            this.updateVersion
            this.remarks = "";
        }

        public PriceBreaks(int seq_No, decimal end,
            bool isLumpSum, decimal lumpSumValue, decimal rate,
            byte[] updateVersion, string remarks)
        {
            this.Seq_No = seq_No;
            this.End = end;
            this.IsLumpSum = isLumpSum;
            this.LumpSumValue = lumpSumValue;
            this.Rate = rate;
            this.UpdateVersion = updateVersion;
            this.Remarks = remarks;
        }

        public int Seq_No
        {
            get { return seq_No; }
            set { seq_No = value; }
        }

        public decimal End
        {
            get { return end; }
            set {
                    if (value <= 0)
                    {
                        throw new FMException(TptResourceBLL.ErrEndZero);
                    }
                    else
                    {
                        end = value;
                    }
                }
        }

        public bool IsLumpSum
        {
            get { return isLumpSum; }
            set {
                    string temp = value.ToString();
                    temp = temp.Trim();
                    if (temp.Length == 0)
                    {
                        throw new FMException(TptResourceBLL.ErrIsLumpSumBlank);
                    }
                    else
                    {
                        isLumpSum = value;
                    }
                }
        }

        public decimal LumpSumValue
        {
            get { return lumpSumValue; }
            set {
                    if (isLumpSum == true)
                    {
                        if (value <= 0)
                        {
                            throw new FMException(TptResourceBLL.ErrLumpSumValueLessThanZero);
                        }
                        else
                        {
                            lumpSumValue = value;
                        }
                    }
                    else
                    {
                        lumpSumValue = 0;
                    }
                }
        }

        public decimal Rate
        {
            get { return rate; }
            set {
                    if (isLumpSum == false)
                    {
                        if (value <= 0)
                        {
                            throw new FMException(TptResourceBLL.ErrLumpSumValueLessThanZero);
                        }
                        else
                        {
                            rate = value;
                        }
                    }
                    else
                    {
                        rate = 0;
                    }
                }
        }

        public byte[] UpdateVersion
        {
            get { return updateVersion; }
            set { updateVersion = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public decimal GetSubTotal(decimal qty)
        {
            if (isLumpSum == true)
            {
                return lumpSumValue;
            }
            else
            {
                decimal temp = 0;
                temp = qty * rate;
                return temp;
            }
        }

        public bool ValidateAllProperties()
        {
            //validate by using property methods
            bool temp = false;

            End = this.end;
            IsLumpSum = this.isLumpSum;
            LumpSumValue = this.lumpSumValue;
            Rate = this.rate;

            temp = true;
            return temp;

        }

    }
}
