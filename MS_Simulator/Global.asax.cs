using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
//
using FM.FMSystem.DAL;
// for mvc
using System.Web.Mvc;
using System.Web.Optimization;
using System.Reflection;
using System.IO;
using System.Text;

namespace MS_Simulator
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            // get initial info from .ini file
            StringBuilder userId = new StringBuilder(255);
            StringBuilder password = new StringBuilder(255); 
            StringBuilder wsAddress = new StringBuilder(255); 
            StringBuilder sql_server_addr = new StringBuilder(255);
            StringBuilder db_name = new StringBuilder(255);

            #region "For testing"
            //string iniPath = @"D:\message_system_workCopy\MS_Simulator\bin\System_Manager.ini";
            #endregion

            /*
             * 2015-04-28 Gerry recommended: integrate the WS section into Company section.
             * For the moment, we still keep the WS section. We will look back into this
             * issue in future.
             */
            string iniPath = HttpRuntime.AppDomainAppPath + "\\System_Manager.ini";
            FMGlobalSettings.GetPrivateProfileString("User", "UserName", "", userId, 255, iniPath);
            FMGlobalSettings.GetPrivateProfileString("User", "Password", "", password, 255, iniPath);
            FMGlobalSettings.GetPrivateProfileString("WS", "DBName", "", db_name, 255, iniPath);
            FMGlobalSettings.GetPrivateProfileString("WS", "DBServer", "", sql_server_addr, 255, iniPath);
            FMGlobalSettings.GetPrivateProfileString("WS", "WSAddress", "", wsAddress, 255, iniPath);

            //20150515 - gerr modified setting of connection
            FMGlobalSettings.TheInstance.SetConnectionString(sql_server_addr.ToString(),db_name.ToString(), userId.ToString(), password.ToString());
            FMGlobalSettings.WS_Address = wsAddress.ToString();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            // for mvc
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // end of for mvc
                        
        }
    }
}
