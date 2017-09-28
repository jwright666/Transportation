using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using FM.TR_HLBookUI.UI;
using FM.TR_FMSystemDLL.BLL;
using TR_FormBaseLibrary;
using FM.TR_HLBookDLL.BLL;
using FM.TR_MarketDLL.BLL;
using FM.TR_SeaFreightDLL.BLL;
using TR_LanguageResource.Resources;
using FM.TR_FMSystemDLL.DAL;

namespace TransportUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
        {
            //********** Chong Chin 21 April 2010 Start *************   
            // Use Mutex object to create inter process lock 

            bool createdNew;
            Mutex mutex = new Mutex(false, "TRBookUI", out createdNew) ;

            // Wait for 1 minute first - allow time to reset the mutex

            if ((!mutex.WaitOne(1000, false)) & createdNew == false)
            {
                MessageBox.Show("Only one instance of this application can be started");
                Application.Exit();
            }
            else
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    #region For DEBUG
                    //args = new string[3];
                    //args[0] = "ipl";
                    //args[1] = "support-srv\\innosqlmaster";//"42.1.62.183\\FREIGHTMASTER";//"PC-5\\SQLEXPRESS";// ; //"MS2012STD\\SQL2012DEV";//"TONI\\FM";//"WIN-V55RV2BO0AE\\SQLEXPRESS";//
                    //args[2] = "FM80_SG_Demo"; //;  //"FM80_SG_WYN2KTRANSPORT";//"FM80_SG_USS";//"FM81_SG_LHS_TEST";//
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
                        //TransportSettings settings = TransportSettings.GetTransportSetting();

                        ResourceManager resourceManager;
                        resourceManager = new ResourceManager("TR_LanguageResource", Assembly.GetExecutingAssembly());

                        FMGlobalSettings.TheInstance.SetHeaderGridColumns();
                        FMGlobalSettings.TheInstance.SetDetailGridColumns();
                        FMGlobalSettings.TheInstance.SetPlanJobTripColumns();
                        
                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                        RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name);
                        Application.Run(new frmTransportJobMain(args[0], mutex));
                    }
                    //*/
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