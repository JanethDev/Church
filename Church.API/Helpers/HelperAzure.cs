using Church.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Church.API.Helpers
{
    public class HelperAzure
    {
        string conStr = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
        string StorageRoot = ConfigurationManager.AppSettings["urlAzureStorage"];
        string sContainer = ConfigurationManager.AppSettings["AzureContainer"];

        public string SaveFile(Stream stream, string fileName, string contentType)
        {
            Storage storage = new Storage(conStr, StorageRoot);
            string result = storage.createBlobFromStream(sContainer, fileName, stream, contentType);
            if (!result.Contains("Ha ocurrido un Error: -"))
            {
                return result.Replace("http", "https");
            }
            return "";
        }
    }
}