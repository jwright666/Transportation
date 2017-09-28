using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FM.TransportMarket.BLL;
using FM.TransportMarket.DAL;
using FM.FMSystem.DAL;
using FM.FMSystem.BLL;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Globalization;
using FM.TransportMaintenanceDLL.BLL;
using TR_LanguageResource.Resources;

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
            Mutex mutex = new Mutex(true, "TR_MarketUI", out createdNew);

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


                #region For Testing
                /*
                try
                {     
                    ResourceManager resourceManager;
                    resourceManager = new ResourceManager("TR_LanguageResource", Assembly.GetExecutingAssembly());

                    //String userID = "ipl";
                    //String server = "INNOSYS-TR1\\LOCALDB";
                    String userID = "ipl";
                    String server = "SUPPORTSRV\\INNOSQLMASTER";
                    FMGlobalSettings.TheInstance.SetConnectionString(userID, server);
                    TransportSettings settings = TransportSettings.GetTransportSetting();

                   
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Culture);
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                    RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name);

                    Application.Run(new frmMarketMain(userID, mutex, "HAU"));
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ex.ToString(), CommonResource.Error);
                }
                catch (FMException ex)
                {
                    MessageBox.Show(ex.ToString(), CommonResource.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), CommonResource.Error);
                }
                */
                #endregion
                ///*      
                try
                {
                    if (args.Length == 0 || User.ValidateUser(args[0], args[1]) == false)
                    {
                        MessageBox.Show("Please start this application from Freight Master Menu");
                        Application.Exit();
                    }
                    else
                    {
                        FMGlobalSettings.TheInstance.SetConnectionString(args[0], args[1]);

                        TransportSettings settings = TransportSettings.GetTransportSetting();

                        ResourceManager resourceManager;
                        resourceManager = new ResourceManager("TR_LanguageResource", Assembly.GetExecutingAssembly());

                        Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Culture);
                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                        RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name);

                        Application.Run(new frmMarketMain(args[0], mutex, "HAU"));

                    }
                    //*/
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show(ex.ToString(), CommonResource.Error);
                }
                catch (FMException ex)
                {
                    MessageBox.Show(ex.ToString(), CommonResource.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), CommonResource.Error);
                }
            }
        }
         

    }
}
