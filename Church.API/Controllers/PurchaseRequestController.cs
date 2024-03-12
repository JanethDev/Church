using Church.API.Filters;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Church.API.Controllers
{
    //[HMACAuthentication]
    [RoutePrefix("api/PurchaseRequest")]
    public class PurchaseRequestController : ApiController
    {
        private readonly PurchasesRequestsB PurchasesRequestsB = new PurchasesRequestsB();
        private readonly PaymentsB PaymentsB = new PaymentsB();
        ExchangeRatesB ExchangeRatesB = new ExchangeRatesB();

        [Route("GetByUserId/{UserId}/{Filter}/{PurReq}")]
        public IHttpActionResult GetByUserId(int UserId, int Filter, int PurReq = 0)
        {
            Response response = new Response();

            var models = PurchasesRequestsB.Api_GetByUserId(UserId, Filter, PurReq);

            foreach (var model in models)
            {
                if (model?.lstPayments?.Any() == true && model.lstPayments.Any(r => r.OverduePayment))
                {
                    var lstPurchaseRequestPaymentIds = model?.lstPayments?.Select(r => r.PurchaseRequestPaymentID).ToList();
                    var vPayments = PaymentsB.GetListByPurchaseRequestPaymentIds(lstPurchaseRequestPaymentIds);
                    var newPayments = new List<tblPurchaseRequestPayments>();

                    bool nextPaymentToDo = true;

                    foreach (var item in model.lstPayments)
                    {
                        item.NextPayemntToDo = false;
                        if (item.Estatus == PaymentEstatus.Pending)
                        {
                            item.NextPayemntToDo = nextPaymentToDo;
                            nextPaymentToDo = false;
                        }

                        newPayments.Add(item);

                        if (item.HasInterest && (model.PaymentMethod == PaymentMethods.Meses18 || model.PaymentMethod == PaymentMethods.Meses24 || (model.PaymentMethod == PaymentMethods.Meses36 && model.Level <= 6)))
                        {
                            var vPayment = vPayments?.FirstOrDefault(r => r.PurchaseRequestPaymentID == item.PurchaseRequestPaymentID);

                            var itemOverdue = new tblPurchaseRequestPayments();

                            itemOverdue.IsOverduePayment = true;

                            itemOverdue.NumPayment = item.NumPayment;

                            itemOverdue.OverduePayment = item.OverduePayment;

                            itemOverdue.PaymentDate = item.PaymentDate;

                            itemOverdue.PurchaseRequestPaymentID = item.PurchaseRequestPaymentID;

                            itemOverdue.PurchasesRequestID = item.PurchasesRequestID;

                            itemOverdue.TicketUrl = item.TicketUrl;

                            itemOverdue.ReceiptUrl = vPayment?.ReceiptUrl;

                            //0.02M
                            itemOverdue.Amount = (item.Amount * (model.PaymentMethod == PaymentMethods.Meses18 ? 0.065M : (model.PaymentMethod == PaymentMethods.Meses36 ? 0.145M : 0.1M)));

                            itemOverdue.HasInterest = false;

                            itemOverdue.HasPaymentDone = vPayment != null;

                            itemOverdue.Estatus = vPayment == null ? PaymentEstatus.Pending
                            : vPayment.Estatus == ClientPaymentEnum.Pending ? PaymentEstatus.ValidationProcess
                            : vPayment.Estatus == ClientPaymentEnum.Approved ? PaymentEstatus.Paid
                            : vPayment.Estatus == ClientPaymentEnum.Rejected ? PaymentEstatus.Pending
                            : PaymentEstatus.Pending;

                            if (itemOverdue.Estatus == PaymentEstatus.Pending)
                            {
                                itemOverdue.NextPayemntToDo = nextPaymentToDo;
                                nextPaymentToDo = false;
                            }

                            newPayments.Add(itemOverdue);
                        }
                    }

                    model.lstPayments = newPayments;
                }
            }

            response.Result = Result.OK;
            response.Data = JsonConvert.SerializeObject(models);

            return Ok(response);
        }

        [Route("GetExchangeRate")]
        public IHttpActionResult GetExchangeRate()
        {
            Response response = new Response();
            tblExchangeRates exchangeRates = ExchangeRatesB.GetLastReccord();

            response.Result = Result.OK;
            response.Decimal = exchangeRates.Rate;

            return Ok(response);
        }
    }
}