using Church.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Church.Helpers
{
    public class HelperAzure
    {
        string conStr = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
        string StorageRoot = ConfigurationManager.AppSettings["urlAzureStorage"];
        string sContainer = ConfigurationManager.AppSettings["AzureContainer"];

        public string SaveFile(HttpPostedFileBase File)
        {
            string sFileName = Guid.NewGuid() + Path.GetExtension(File.FileName);
            Storage storage = new Storage(conStr, StorageRoot);
            string result = storage.createBlobFromStream(sContainer, sFileName, File.InputStream, File.ContentType);
            if (!result.Contains("Ha ocurrido un Error: -"))
            {
                result.Replace("http", "https");
                return result;
            }
            return "";
        }

        public string SaveFile2(HttpPostedFileBase File, int PaymentID)
        {
            string respuesta = "";
            string namefile = "";
            string returnrespuesta = "";
            char[] delimiterChars = { '.' };

            try
            {
                string[] words = File.FileName.Split(delimiterChars);
                string algo = words[1];
                namefile = Path.GetFileName("Recibopago" + PaymentID + "." + algo);
                string path = Path.Combine(@"c:\ChurchArchive", namefile);
                File.SaveAs(path);
            }
            catch (Exception ex)
            {
                respuesta = "Ha ocurrido un Error";
            }

            if (respuesta == "")
            {
                return returnrespuesta = namefile;
            }

            return returnrespuesta;
        }

        public string SaveFileBilling(HttpPostedFileBase File, int BillingNumber)
        {
            string respuesta = "";
            string namefile = "";
            string returnrespuesta = "";
            char[] delimiterChars = { '.' };

            if (File != null)
            {
                try
                {
                    string[] words = File.FileName.Split(delimiterChars);
                    string algo = words[1];
                    namefile = Path.GetFileName(BillingNumber + "." + algo);
                    string path = Path.Combine(@"c:\ChurchArchive", namefile);
                    File.SaveAs(path);
                }
                catch (Exception ex)
                {
                    respuesta = "Ha ocurrido un Error";
                }

                if (respuesta == "")
                {
                    return returnrespuesta = namefile;
                }
                else
                {
                    return returnrespuesta = null;
                }
            }
            
            return returnrespuesta = null;
        }

        //public string SaveTicketFile(HttpPostedFileBase File, string FileName)
        //{
        //    Storage storage = new Storage(conStr, StorageRoot);
        //    string result = storage.createBlobFromStream(sContainer, FileName, File.InputStream, File.ContentType);

        //    if (!result.Contains("Ha ocurrido un Error: -"))
        //    {
        //        result.Replace("http", "https");

        //        return result;
        //    }

        //    return "";
        //}

        public string SaveTicketFile(HttpPostedFileBase File)
        {
            string respuesta = "";
            string namefile = "";
            string returnrespuesta = "";
            char[] delimiterChars = { '.' };

            try
            {
                namefile = Path.GetFileName(File.FileName);
                string path = Path.Combine(@"c:\ChurchArchive", namefile);
                File.SaveAs(path);
            }
            catch (Exception ex)
            {
                respuesta = "Ha ocurrido un Error";
            }

            if (respuesta == "")
            {
                return returnrespuesta = namefile;
            }

            return returnrespuesta;
        }

        public string SaveTicketFileFormatoRecibo(HttpPostedFileBase File, int idTabla)
        {
            string respuesta = "";
            string namefile = "";
            string returnrespuesta = "";
            char[] delimiterChars = { '.' };

            try
            {
                string[] words = File.FileName.Split(delimiterChars);
                string algo = words[1];
                namefile = Path.GetFileName("ReciboPago" + idTabla + "." + algo);
                string path = Path.Combine(@"c:\ChurchArchive", namefile);
                File.SaveAs(path);
            }
            catch (Exception ex)
            {
                respuesta = "Ha ocurrido un Error";
            }

            if (respuesta == "")
            {
                return returnrespuesta = namefile;
            }

            return returnrespuesta;
        }

        public string SaveTicketFileFormatoActa(HttpPostedFileBase File, int idTabla)
        {
            string respuesta = "";
            string namefile = "";
            string returnrespuesta = "";
            char[] delimiterChars = { '.' };

            try
            {
                string[] words = File.FileName.Split(delimiterChars);
                string algo = words[1];
                namefile = Path.GetFileName("ActaDefuncion" + idTabla + "." + algo);
                string path = Path.Combine(@"c:\ChurchArchive", namefile);
                File.SaveAs(path);
            }
            catch (Exception ex)
            {
                respuesta = "Ha ocurrido un Error";
            }

            if (respuesta == "")
            {
                return returnrespuesta = namefile;
            }

            return returnrespuesta;
        }

        public string SaveTicketFileFormatoCremacion(HttpPostedFileBase File, int idTabla)
        {
            string respuesta = "";
            string namefile = "";
            string returnrespuesta = "";
            char[] delimiterChars = { '.' };

            try
            {
                string[] words = File.FileName.Split(delimiterChars);
                string algo = words[1];
                namefile = Path.GetFileName("Cremacion" + idTabla + "." + algo);
                string path = Path.Combine(@"c:\ChurchArchive", namefile);
                File.SaveAs(path);
            }
            catch (Exception ex)
            {
                respuesta = "Ha ocurrido un Error";
            }

            if (respuesta == "")
            {
                return returnrespuesta = namefile;
            }

            return returnrespuesta;
        }

    }
}