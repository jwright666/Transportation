using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FM.TR_MaintenanceDLL.DAL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using System.Data.SqlClient;

namespace FM.TR_MaintenanceDLL.BLL
{
    public sealed class TransportSettings
    {
        static TransportSettings instance = null;
        static readonly object padlock = new object();

        private string tpt_code;
        private string tpt_name;
        private string address1;
        private string address2;
        private string address3;
        private string address4;
        private int booking_no;
        private int job_no;
        private string booking_no_prefix;
        private string job_no_prefix;
        private int log_no;
        private bool arePricesVisibleInJob; 
        private int quantityDecimals;
        private int foreignCurrencyDecimals;
        private int totalAmountDecimals;
        private int unitAmountDecimals;
        private int quotation_no;
        private string company_reg_no;
        private string gst_reg_no;
        private string homeCurrency;
        private string localEquivCurrency;
        private string culture;
        private string airportOfficeCode;
        public int planningCheckTime { get; set; } //20141031 - gerry added
        public string location { get; set; } //20161011 -gerry added

        // 2014-07-11 Zhou Kai adds
        private string defaultPortCode;
        private string defaultPortName;
        public string DefaultPortCode
        {
            get { return defaultPortCode; }
            set { defaultPortCode = value; }
        }
        public string DefaultPortName
        {
            get { return defaultPortName; }
            set { defaultPortName = value; }
        }
        //20170526 - gerry added
        public string DefaultWarehouseCode { get; set; }
        public string DefaultWarehouseName { get; set; }


        // 2014-07-11 Zhou Kai ends

        //public static TransportSettings Instance
        //{
        //    get
        //    {
        //        lock (padlock)
        //        {
        //            if (instance == null)
        //            {
        //                instance = TransportSettingsDAL.GetTransportSetting();
        //            }
        //            else
        //            {
        //                instance = TransportSettingsDAL.GetTransportSettingForInvoicing();
        //            }
        //            return instance;
        //        }
        //    }
        //}       

        #region Public Propertis

        public string Tpt_code
        {
            get { return tpt_code; }
            set { tpt_code = value; }
        }

        public string Tpt_name
        {
            get { return tpt_name; }
            set { tpt_name = value; }
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


        public int Booking_no
        {
            get { return booking_no; }
            set { booking_no = value; }
        }

        public int Job_no
        {
            get { return job_no; }
            set { job_no = value; }
        }

        public string Booking_no_prefix
        {
            get { return booking_no_prefix; }
            set {
                    if (value.Length > 2)
                    {
                        throw new FMException(TptResourceBLL.ErrorBookingNoPrefix);
                    }                

                    booking_no_prefix = value; 
                }
        }

        public string Job_no_prefix
        {
            get { return job_no_prefix; }
            set {

                    if (value.Length > 2)
                    {
                        throw new FMException(TptResourceBLL.ErrorJobNoPrefix);
                    }                
                    job_no_prefix = value; 

                }
        }

        public int Log_no
        {
            get { return log_no; }
            set { log_no = value; }
        }       

        public bool ArePricesVisibleInJob
        {
            get { return arePricesVisibleInJob; }
            set { arePricesVisibleInJob = value; }
        }

        public int QuantityDecimals
        {
            get { return quantityDecimals; }
            set { quantityDecimals = value; }
        }

        public int ForeignCurrencyDecimals
        {
            get { return foreignCurrencyDecimals; }
            set { foreignCurrencyDecimals = value; }
        }

        public int TotalAmountDecimals
        {
            get { return totalAmountDecimals; }
            set { totalAmountDecimals = value; }
        }

        public int UnitAmountDecimals
        {
            get { return unitAmountDecimals; }
            set { unitAmountDecimals = value; }
        }

        public int Quotation_no
        {
            get { return quotation_no; }
            set { quotation_no = value; }
        }

        public string Company_reg_no
        {
            get { return company_reg_no; }
            set { company_reg_no = value; }
        }

        public string Gst_reg_no
        {
            get { return gst_reg_no; }
            set { gst_reg_no = value; }
        }

        public string HomeCurrency
        {
            get { return homeCurrency; }
            set { homeCurrency = value; }
        }
        public string LocalEquivCurrency
        {
            get { return localEquivCurrency; }
            set { localEquivCurrency = value.Trim(); }
        }
        public string Culture
        {
            get { return culture; }
            set { culture = value.Trim(); }
        }
        public string AirportOfficeCode
        {
            get { return airportOfficeCode; }
            set { airportOfficeCode = value.Trim(); }
        }

        #endregion

        //Gerry Added 2 parameters for home currency and localequivalent currency
        public TransportSettings(string tpt_code,string tpt_name,string address1,
            string address2,string address3,string address4,
            int booking_no, int job_no, string booking_no_prefix, string job_no_prefix, int log_no,
            int quantityDecimals, int foreignCurrencyDecimals,
            int totalAmountDecimals, int unitAmountDecimals, bool arePricesVisibleInJob, int quotation_no,
            string company_reg_no, string gst_reg_no, string homeCurrency, string localEquivCurrency, string culture
            /*2014-07-11 Zhou Kai adds*/, string defaultPortCode, string defaultPortName)
        {
            this.Tpt_code=tpt_code;
            this.Tpt_name=tpt_name;
            this.Address1=address1;
            this.Address2=address2;
            this.Address3=address3;
            this.Address4=address4;
            this.Booking_no=booking_no;
            this.Job_no=job_no;
            this.Booking_no_prefix=booking_no_prefix;
            this.Job_no_prefix=job_no_prefix;
            this.Log_no=log_no;
            this.QuantityDecimals = quantityDecimals;
            this.ForeignCurrencyDecimals = foreignCurrencyDecimals;
            this.TotalAmountDecimals = totalAmountDecimals;
            this.UnitAmountDecimals = unitAmountDecimals;
            this.ArePricesVisibleInJob = arePricesVisibleInJob;
            this.Quotation_no = quotation_no;
            this.Company_reg_no = company_reg_no;
            this.Gst_reg_no = gst_reg_no;
            this.HomeCurrency = homeCurrency;
            this.LocalEquivCurrency = localEquivCurrency;
            this.Culture = culture;
            // 2014-07-11 Zhou Kai adds
            this.defaultPortCode = defaultPortCode;
            this.defaultPortName = defaultPortName;
            // 2014-07-11 Zhou Kai ends
            this.location = string.Empty;
            this.DefaultWarehouseCode = string.Empty;
            this.DefaultWarehouseName = string.Empty;

        }
        

        public TransportSettings()
        {
            this.tpt_code = "";
            this.tpt_name = "";
            this.address1 = "";
            this.address2 = "";
            this.address3 = "";
            this.address4 = "";
            this.booking_no = 0;
            this.job_no = 0;
            this.booking_no_prefix = "TR";
            this.job_no_prefix = "TR";
            this.log_no = 0;
            this.quantityDecimals = 0;
            this.foreignCurrencyDecimals = 0;
            this.totalAmountDecimals = 0;
            this.unitAmountDecimals = 0;
            this.arePricesVisibleInJob = true;
            this.quotation_no = 0;
            this.company_reg_no = "";
            this.gst_reg_no = "";
            this.homeCurrency = "";
            this.localEquivCurrency = "";
            this.culture = "";
            this.airportOfficeCode = "";
            // 2014-07-11 Zhou Kai adds
            this.defaultPortCode = String.Empty;
            this.defaultPortName = String.Empty;
            // 2014-07-11 Zhou Kai ends
            //20141031 - gerry added
            this.planningCheckTime = 30; //default to 30 sec(note timer measured milliseconds)
            this.location = string.Empty;
            //20141031 - gerry end
            this.DefaultWarehouseCode = string.Empty;
            this.DefaultWarehouseName = string.Empty;
        }

        public static bool GetArePricesVisibleInJob()
        {
            return TransportSettingsDAL.GetArePricesVisibleInJob();
        }

        public static SortableList<TransportSettings> GetTransportSettings()
        {
            SortableList<TransportSettings> t = new SortableList<TransportSettings>();
            TransportSettings ts = TransportSettingsDAL.GetTransportSetting();
            if (ts.tpt_code != "")
            {
                t.Add(ts);
            }
            return t;
        }

        public bool EditTransportSetting()
        {
            try
            {
                TransportSettingsDAL.EditTransportSetting(this);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
            
        }

        public bool AddTransportSetting()
        {
            try
            {
                TransportSettingsDAL.AddTransportSetting(this);
            }
            catch (FMException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static TransportSettings GetTransportSetting()
        {
            return TransportSettingsDAL.GetTransportSetting();
        }

        public static TransportSettings GetTransportSettingForInvoicing(SqlConnection cn)
        {
            return TransportSettingsDAL.GetTransportSettingForInvoice(cn);
        }

    }
}
