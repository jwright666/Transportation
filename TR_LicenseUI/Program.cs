using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FMLicenseUI.Common;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Globalization;
using FM.FMSystem.BLL;

namespace FMLicenseUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool createdNew;
            Mutex mutex = new Mutex(false, "FMLicenseUI", out createdNew);

            // Wait for 1 minute first - allow time to reset the mutex  
            if ((!mutex.WaitOne(1000, false)) & createdNew == false)
            {
                MessageBox.Show("Only one instance of this application can be started");
                Application.Exit();
            }
             try
             {
                 ResourceManager resourceManager;
                 resourceManager = new ResourceManager("TR_LanguageResource", Assembly.GetExecutingAssembly());

                 //Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Culture);
                 Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                 RegionInfo regionInfo = new RegionInfo(Thread.CurrentThread.CurrentCulture.Name);

                 Application.Run(new FrmLicenseMenu());
             }
             catch (FMException ex)
             {
                 MessageBox.Show(ex.ToString());
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.ToString());
             }  
        }
    }
}
