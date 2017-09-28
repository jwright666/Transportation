using System;
using System.Collections.Generic;
using System.Text;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_FMSystemDLL.BLL
{
    public class CurrencyRate
    {
        private string currencyCode;
        private string currencyName;
        private decimal rate;
        private decimal variance;
        private bool defaultCurrency;
        private int currFactor;
        private bool showDecimal;
        private bool roundFlag;

        public string CurrencyCode
        {
            get { return currencyCode; }
        }

        public string CurrencyName
        {
            get { return currencyName; }
        }

        public decimal Rate
        {
            get { return rate; }
        }

        public decimal Variance
        {
            get { return variance; }
        }

        public bool DefaultCurrency
        {
            get { return defaultCurrency; }
        }

        public int CurrFactor
        {
            get { return currFactor;}
            set { currFactor = value; }
        }
        public bool ShowDecimal
        {
            get { return showDecimal; }
            set { showDecimal = value; }
        }
        public bool RoundFlag
        {
            get { return roundFlag; }
            set { roundFlag = value; }
        }

        public CurrencyRate()
        {
            this.currencyCode = "";
            this.currencyName = "";
            this.rate = 0;
            this.variance = 0;
            this.defaultCurrency = false;
            this.currFactor = 0;
            this.showDecimal = false;
            this.roundFlag = false; ;
        }


        public CurrencyRate(string currencyCode, string currencyName, decimal rate, decimal variance,bool defaultCurrency,
                                int currFactor, bool showDecimal, bool roundFlag)
        {
            this.currencyCode = currencyCode;
            this.currencyName = currencyName;
            this.rate = rate;
            this.variance = variance;
            this.defaultCurrency = defaultCurrency;
            this.currFactor = currFactor;
            this.showDecimal = showDecimal;
            this.roundFlag = roundFlag;
        }       

        public static SortableList<CurrencyRate> GetAllCurrencyRates(DateTime requiredDate)
        {
            return CurrencyRateDAL.GetAllCurrencyRates(requiredDate);

        }

        public static SortableList<CurrencyRate> GetAllCurrencyRatesForCustomer(string custCode,DateTime requiredDate)
        {
            SortableList<CurrencyRate> currencyRates = new SortableList<CurrencyRate>();
            currencyRates = CurrencyRateDAL.GetAllCurrencyRatesForCustomer(custCode, requiredDate);
            return currencyRates;

        }

        public static CurrencyRate GetCurrencyRate(string currencyCode,DateTime requiredDate)
        {
            return CurrencyRateDAL.GetCurrencyRate(currencyCode,requiredDate);

        }

        public static bool GetValidCurrencyRateVarianceRange(string currencyCode, DateTime requiredDate, decimal compareCurrencyRate)
        {
            try
            {
               return CurrencyRateDAL.GetValidCurrencyRateVarianceRange(currencyCode, requiredDate, compareCurrencyRate);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
        
        public static string GetCurrencyName(string code)
        {
            return CurrencyRateDAL.GetCurrencyName(code);
        }

        //14 March 2012 - Gerry added this method
        public static Decimal RoundAndShowDecimal(CurrencyRate curr, decimal amount)
        {
            if (curr.ShowDecimal)
            {
                if (curr.RoundFlag)
                    amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
                else
                    amount = decimal.Round(amount, 2, MidpointRounding.ToEven);
            }
            else
                amount = decimal.Truncate(amount);

            return amount;
        }



    }
}
