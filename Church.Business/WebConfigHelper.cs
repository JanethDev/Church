using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Church.Business
{
    public class WebConfigHelper
    {

        public static string FCMApiKey = ConfigurationManager.AppSettings["FCM.ApiKey"];

        public static string GetAppSettingsString(string key)
        {
            Configuration Configuration = null;
            if (System.Web.HttpContext.Current != null)
            {
                Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            }
            else
            {
                Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }

            AppSettingsSection appSettingsSection = Configuration.GetSection("appSettings") as AppSettingsSection;
            return appSettingsSection.Settings[key].Value;
        }

        public static string GetConnectionString(string key)
        {
            Configuration Configuration = null;
            if (System.Web.HttpContext.Current != null)
            {
                Configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            }
            else
            {
                Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            ConnectionStringsSection connectionStringsSection = Configuration.GetSection("connectionStrings") as ConnectionStringsSection;
            return connectionStringsSection.ConnectionStrings[key].ConnectionString;
        }
    }


}
