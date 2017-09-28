using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using FM.FMSystem.BLL;
using FM.FMSystem.DAL;
using System.Resources;
using System.Globalization;
using System.Reflection;
using FM.TransportMaintenanceDLL.BLL;
using TR_LanguageResource.Resources;

namespace FM.TruckPlanning.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
        {
            bool createdNew;
            Mutex mutex = new Mutex(true, "TruckPlanningUI", out createdNew);

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
                    String userID = "wilson";
                    //String server = "INNOSYS-TR1\\LOCALDB";  
                    String server = "supportsrv\\innosqlmaster";
                    FMGlobalSettings.TheInstance.SetConnectionString(userID, server);
                    TransportSettings settings = TransportSettings.GetTransportSetting();

                    ResourceManager resourceManager;
                    resourceManager = new ResourceManager("TR_LanguageResource", Assembly.GetExecutingAssembly());

                    Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Culture);
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                    RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name);

                    Application.Run(new FrmTruckPlanningMain(userID, mutex));
                }
                catch (FMException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                */
                #endregion

                ///*                                                      
             if (args.Length == 0 || User.ValidateUser(args[0], args[1]) == false)
             {
                 MessageBox.Show("Please start this application from Freight Master Menu");
                
                 Application.Exit();
             }
             else
             {
                 try
                 {
                     ResourceManager resourceManager = new ResourceManager("TR_LanguageResource", Assembly.GetExecutingAssembly());

                     FMGlobalSettings.TheInstance.SetConnectionString(args[0], args[1]);
                     TransportSettings settings = TransportSettings.GetTransportSetting(); 
                  
                     Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Culture);
                     Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                     RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name);

                     Application.Run(new FrmTruckPlanningMain(args[0], mutex));
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
                //*/

            }
               
        }

    }
}
