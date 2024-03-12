using Church.API.Filters;
using Church.API.Helpers;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Church.API.Controllers
{
    //[HMACAuthentication]
    [RoutePrefix("api/Payments")]
    public class PaymentsController :  ApiController
    {
        private readonly PurchaseRequestPaymentsB PurchaseRequestPaymentsB = new PurchaseRequestPaymentsB();
        private readonly PaymentsB PaymentsB = new PaymentsB();
        private readonly HelperAzure HelperAzure = new HelperAzure();
        private readonly HelpMethods HelpMethods = new HelpMethods();

        [HttpPost]
        [Route("Create/")]
        public IHttpActionResult Post(tblPayments model)
        {
            Response response = new Response();

            try
            {
                string imageName = $"{(model.IsInterest ? "interest" : "payment")}-ticket-{model.PurchaseRequestPaymentID}.png";
                Stream stream = new MemoryStream(model.FileBytes);

                var urlAzure = HelperAzure.SaveFile(stream, imageName, "image/png");

                model.Estatus = ClientPaymentEnum.Pending;
                model.TicketUrl = urlAzure;
                model.Active = true;
                model.CreatedDate = model.UpdatedDate = HelpMethods.GetDateTime();

                response = PaymentsB.Create(model);
                if(response.Result == Result.OK && !model.IsInterest)
                {
                    tblPurchaseRequestPayments purchaseRequestPayments = PurchaseRequestPaymentsB.GetById(model.PurchaseRequestPaymentID);
                    purchaseRequestPayments.Estatus = PaymentEstatus.ValidationProcess;

                    response = PurchaseRequestPaymentsB.Update(purchaseRequestPayments);
                }

            }
            catch (Exception ex)
            {
                response.Result = Result.EXCEPTION; //Exception
                response.Data = ex.Message;
            }

            return Ok(response);
        }
    }
}