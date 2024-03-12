using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Church.Helpers;
using Church.HtmlHelpers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Web;
using AutoMapper;
using Church.Data.HelperClasses;
using System.Threading.Tasks;

namespace Church.Controllers
{
    public class PaymentsController : BaseController
    {
        HelpMethods hm = new HelpMethods();
        PaymentsB PaymentsB = new PaymentsB();
        PurchaseRequestPaymentsB PurchaseRequestPaymentsB = new PurchaseRequestPaymentsB();
        HelperAzure HelperAzure = new HelperAzure();
        FCMManager FCMManager = new FCMManager();
        public ActionResult Validate()
        {
            ViewBag.ClientPaymentEnum = GetClientPayments();
            var PurchasesPayments = PaymentsB.GetListByEstatusPendiente();
            return View(PurchasesPayments);
        }
        [HttpPost]
        public async Task<ActionResult> Validate(List<tblPayments> LstPayments)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            LstPayments = LstPayments.Where(r => r.Validate).ToList();
            int LoggedUser = hm.GetIntbyString("UserName", HttpContext.User.Identity.Name, "UserID", "tblUsers");
            foreach (var item in LstPayments)
            {
                tblPayments tblPayments = PaymentsB.GetById(item.PaymentID);

                //string File = HelperAzure.SaveFile(item.File);
                string File = HelperAzure.SaveFile2(item.File, item.PaymentID);
                tblPayments.ReceiptUrl = File;
                tblPayments.Estatus = item.Estatus;
                tblPayments.UpdatedBy = LoggedUser;
                tblPayments.UpdatedDate = ServerDateTimeNow;
                tblPayments.ApprovedRejectedDate = ServerDateTimeNow;

                var response = PaymentsB.Update(tblPayments);

                if(response.Result == Result.OK)
                {
                    var vPurchaseRequest = PurchaseRequestPaymentsB.GetById(tblPayments.PurchaseRequestPaymentID);
                    vPurchaseRequest.Estatus = tblPayments.Estatus == ClientPaymentEnum.Approved ? PaymentEstatus.Paid : PaymentEstatus.Pending;
                    response = PurchaseRequestPaymentsB.Update(vPurchaseRequest);
                }

                string sMessage = tblPayments.Estatus == ClientPaymentEnum.Approved ? $"Se aprobó su pago #{tblPayments.NumPayment}." : $"Se rechazo su pago #{tblPayments.NumPayment}";
                
                var vPushNotification = new PushNotification
                {
                    Data = sMessage,
                    PushNotificationType = PushNotificationType.Message
                };

                //bool bResponse = await FCMManager.SendNotificationAsync(vPushNotification, tblPayments.UserNotificationKey);
            }

            ViewBag.Save = 1;
            ViewBag.ClientPaymentEnum = GetClientPayments();
            var PurchasesPayments = PaymentsB.GetListByEstatusPendiente();
            return View(PurchasesPayments);
        }
        private IEnumerable<SelectListItem> GetClientPayments()

        {

            var vItems = Enum.GetValues(typeof(ClientPaymentEnum)).Cast<ClientPaymentEnum>()

                  .Where(r => r != ClientPaymentEnum.Pending)

                  .Select(r => new SelectListItem { Value = ((int)r).ToString(), Text = EnumHelper<ClientPaymentEnum>.GetDisplayValue(r) });

            return vItems;

        }

        public ActionResult SearchArchivePayment(string billpath)
        {
            string url = Path.Combine("C:\\ChurchArchive", billpath);
            char[] delimiterChars = { '.' };

            string base64ImageRepresentation = "";
            string base64StringPDF = "";

            string respuesta = "";

            byte[] imageArray;

            byte[] buffer;

            if (url.ToLower().Contains("pdf"))
            {
                try
                {
                    using (Stream stream = new System.IO.FileStream(url, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        buffer = new byte[stream.Length - 1];
                        stream.Read(buffer, 0, buffer.Length);
                    }

                    base64StringPDF = Convert.ToBase64String(buffer, 0, buffer.Length);

                    if (url != null && url.Length > 0)
                    {
                        base64StringPDF = "data:application/pdf;base64," + base64StringPDF;
                    }
                }
                catch (Exception ex)
                {
                    respuesta = "No hay archivo";
                }
            }
            else
            {
                try
                {
                    imageArray = System.IO.File.ReadAllBytes(url);
                    base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    if (url != null && url.Length > 0)
                    {
                        if (url.ToLower().Contains("jpeg"))
                        {
                            base64ImageRepresentation = "data:image/jpeg;base64," + base64ImageRepresentation;
                        }
                        else if (url.ToLower().Contains("jpg"))
                        {
                            base64ImageRepresentation = "data:image/jpg;base64," + base64ImageRepresentation;
                        }
                        else if (url.ToLower().Contains("png"))
                        {
                            base64ImageRepresentation = "data:image/png;base64," + base64ImageRepresentation;
                        }
                    }
                }
                catch (Exception ex)
                {
                    respuesta = "No hay archivo";
                }
            }


            return Json(new
            {
                respuesta = respuesta,
                base64ImageRepresentation = base64ImageRepresentation,
                base64StringPDF = base64StringPDF
            });
        }
    }
}