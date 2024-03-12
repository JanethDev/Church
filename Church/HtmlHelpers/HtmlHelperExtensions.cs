using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Church.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {

        public static List<string> lstAbecedario = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "Ñ",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
        };

        public static MvcHtmlString Nl2Br(this HtmlHelper htmlHelper, string text)
        {
            if (string.IsNullOrEmpty(text))
                return MvcHtmlString.Create(text);
            else
            {
                StringBuilder builder = new StringBuilder();
                string[] lines = text.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i > 0)
                        builder.Append("<br/>\n");
                    builder.Append(HttpUtility.HtmlEncode(lines[i]));
                }
                return MvcHtmlString.Create(builder.ToString());
            }
        }


        public static string ActiveAction(this HtmlHelper helper, string controller, string action)
        {
            string classValue = "";

            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

            if (currentController == controller && currentAction == action)
            {
                classValue = "selected a";
            }

            return classValue;
        }

        public static string ActiveController(this HtmlHelper helper, string controller)
        {
            string classValue = "";

            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            if (currentController == controller)
            {
                classValue = "selected a";
            }

            return classValue;
        }

        public static string SendEmail(string sSubject = "", string sMessageBody = "", List<string> lstToAddress = null, List<Attachment> Archivo = null)
        {
            string sResponse = "";
            try
            {
                string sEmailSubject = sSubject;
                string sCorreo = WebConfigurationManager.AppSettings["Email"];
                string sContrasena = WebConfigurationManager.AppSettings["Password"];
                string sDisplayName = WebConfigurationManager.AppSettings["Responsable"];
                var vFromAddress = new MailAddress(sCorreo, sDisplayName);

                MailMessage msg = new MailMessage();
                msg.From = vFromAddress;
                foreach (var item in lstToAddress)
                    msg.To.Add(new MailAddress(item));
                if (Archivo?.Any() == true)
                    foreach (var item in Archivo)
                        msg.Attachments.Add(item);

                msg.Subject = sEmailSubject;
                msg.IsBodyHtml = true;
                msg.Body = sMessageBody;

                using (SmtpClient client = new SmtpClient())
                {
                    string sHost = WebConfigurationManager.AppSettings["SMTP"];
                    int iPort = int.Parse(WebConfigurationManager.AppSettings["Port"]);
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(sCorreo, sContrasena);
                    client.Host = sHost;
                    client.Port = iPort;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                sResponse = ex.Message;
            }
            return sResponse;
        }
    }
}