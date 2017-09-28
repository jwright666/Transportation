using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Text;
using TR_LanguageResource.Resources;
using TR_MessageDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using FM.TR_FMSystemDLL.DAL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_HLPlanDLL.BLL;


namespace FM.TransportPlanning.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

//        static void Main()

        static void Main(string[] args)
        {
             bool createdNew;
            Mutex mutex = new Mutex(true, "TRPlanningUI", out createdNew);

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

                #region For DEBUG
                //args = new string[3];
                //args[0] = "ipl";
                //args[1] = "support-srv\\innosqlmaster";//"PC-5\\SQLEXPRESS";// ; //"MS2012STD\\SQL2012DEV";//"TONI\\FM";//"WIN-V55RV2BO0AE\\SQLEXPRESS";//
                //args[2] = "FM80_SG_Demo"; //"FM80_SG_USS";//"FM81_SG_LHS_TEST";// ;  //"FM80_SG_WYN2KTRANSPORT";//
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
                        //20130523 - Gerry Replaced new parameter added from FM(databaseName)
                        //FMGlobalSettings.TheInstance.SetConnectionString(args[0], args[1]);
                        FMGlobalSettings.TheInstance.SetConnectionString(args, true);
                        // 2014-04-26 Zhou Kai adds a line of code to set the pubConnectionsString also, 
                        // it's needed while license checking
                        FMGlobalSettings.TheInstance.SetPubsConnectionString(args[1]);
                        // 2014-04-26 Zhou Kai ends
                        // 2015-04-04 Zhou Kai adds for web service
                        StringBuilder wsAddress = new StringBuilder(255);
                        //FMGlobalSettings.GetPrivateProfileString("WS", "WSAddress", "", wsAddress, 255, Application.StartupPath + "//System_Manager.ini");
                        FMGlobalSettings.GetPrivateProfileString("TruckCommAPI", "BaseAddress", "", wsAddress, 255, Application.StartupPath + "//System_Manager.ini");
                        FrmHaulierPlanningEntry.WS_URL = wsAddress.ToString();

                        FMGlobalSettings.TheInstance.SetHeaderGridColumns();
                        FMGlobalSettings.TheInstance.SetDetailGridColumns();
                        FMGlobalSettings.TheInstance.SetPlanJobTripColumns();

                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                        RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name);

                        Application.Run(new FrmTransportPlanningMain(args[0], mutex));
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
                //*/
            }
        }
    }
}
