using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Globalization;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_TRKBookUI.UI;
using FM.TR_MaintenanceDLL.BLL;
using TR_FormBaseLibrary;
using TR_LanguageResource.Resources;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TransportMarket.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        // FM72 calls this TransportMarketUI.exe passing in 1 string, the name of the userID e.g "ipl"
        static void Main(string[] args)
        {
            bool createdNew;
            Mutex mutex = new Mutex(true, "TruckBookUI", out createdNew);

            // Wait for 1 minute first - allow time to reset the mutex

            if ((!mutex.WaitOne(1000, false)) & createdNew == false)
            {
                MessageBox.Show("Only one instance of this application can be started");
                Application.Exit();
            }
            // 21 April 2010  End
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);   
                try
                {
                    #region For DEBUG
                    //args = new string[3];
                    //args[0] = "ipl";
                    //args[1] = "support-srv\\innosqlmaster";//"180.240.231.11\\SQLEXPRESS";//"PC-5\\SQLEXPRESS";//"42.1.62.183\\FREIGHTMASTER";// ; //"MS2012STD\\SQL2012DEV";//"TONI\\FM";//
                    //args[2] = "FM81_SG_LOMING_TEST";//"FM80_SG_Demo"; // ;  //"FM80_SG_WYN2KTRANSPORT";//"FM80_SG_USS";//"FM81_SG_LHS_TEST";//
                    #endregion
                    if (args.Length == 0 || User.ValidateUser(args[0], args[1]) == false)
                    {
                        MessageBox.Show("Please start this application from Freight Master Menu");
                        Application.Exit();
                    }
                    else
                    {
                        //20130523 - Gerry Replaced new parameter added from FM(databaseName)
                        //FMGlobalSettings.TheInstance.SetConnectionString(args[0], args[1]);
                        FMGlobalSettings.TheInstance.SetConnectionString(args);
                        FMGlobalSettings.TheInstance.SetPubsConnectionString(args[1]);
                       
                        ResourceManager resourceManager;
                        resourceManager = new ResourceManager("TR_LanguageResource", Assembly.GetExecutingAssembly());

                        //20131004 - Gerry removed, not it will not depend on the transport setting, 
                        //TransportSettings settings = TransportSettings.GetTransportSetting(); 
                        //it will directly get the the PC CurrentCulture
                        //Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Culture);
                        //20131004 end
                        
                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                        RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name); 

                        Application.Run(new FrmTruckJobMain(args[0], mutex));      
                    }
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ex.Message.ToString(), CommonResource.Error);
                }
                catch (FMException ex)
                {
                    MessageBox.Show(ex.Message.ToString(), CommonResource.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), CommonResource.Error);
                }   
            }
        }
         

    }
}
