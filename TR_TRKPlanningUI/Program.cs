using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Resources;
using System.Globalization;
using System.Reflection;
using FM.TR_TRKBookDLL.BLL;
using FM.TR_MaintenanceDLL.BLL;
using FM.TR_TRKPlanDLL.BLL;
using FM.TR_FMSystemDLL.BLL;
using TR_LanguageResource.Resources;
using TR_FormBaseLibrary;
using FM.TR_FMSystemDLL.DAL;

namespace FM.TR_TRKPlanningUI.UI
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

                #region For DEBUG
                //args = new string[3];
                //args[0] = "ipl";
                //args[1] = "support-srv\\innosqlmaster";//"LAPTOP-P5S6NM39\\SQLEXPRESS"; // "PC-5\\SQLEXPRESS";//"42.1.62.183\\FREIGHTMASTER";// ; //"MS2012STD\\SQL2012DEV";//"TONI\\FM";//"WIN-V55RV2BO0AE\\SQLEXPRESS";//
                //args[2] = "FM80_SG_Demo"; //"FM81_SG_LOMING_TEST";// ;  //"FM80_SG_WYN2KTRANSPORT";//"FM80_SG_USS";//"FM81_SG_LHS_TEST";//
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
                        FMGlobalSettings.TheInstance.SetPubsConnectionString(args[1]);
                        //20131004 - Gerry removed, not it will not depend on the transport setting, 
                        //it will directly get the the PC CurrentCulture
                        //TransportSettings settings = TransportSettings.GetTransportSetting();                   
                        //Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Culture);
                        //20131004 end

                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                        RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name);

                        Application.Run(new FrmTruckPlanningMain(args[0], mutex));
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
