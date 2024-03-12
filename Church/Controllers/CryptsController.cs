using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize]
    public class CryptsController : BaseController
    {
        CryptsB CryptsB = new CryptsB();
        StatesB StatesB = new StatesB();
        TownsB TownsB = new TownsB();
        CryptsSectionsB CryptsSectionsB = new CryptsSectionsB();
        CryptPricesB CryptPricesB = new CryptPricesB();
        HelpMethods hm = new HelpMethods();
        PurchasesRequestsB PurchasesRequestsB = new PurchasesRequestsB();
        CryptPositionTypesB CryptPositionTypesB = new CryptPositionTypesB();
        DiscountsB DiscountsB = new DiscountsB();
        PaymentsB PaymentsB = new PaymentsB();
        PurchaseRequestPaymentsB PurchaseRequestPaymentsB = new PurchaseRequestPaymentsB();
        ExchangeRatesB ExchangeRatesB = new ExchangeRatesB();

        private readonly HelperAzure HelperAzure = new HelperAzure();

        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        //metodos
        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, "tblCrypts");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult acPSurname(string term)
        {
            var model = hm.Autocompletes("PSurname", term, "tblCrypts");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult acEmail(string term)
        {
            var model = hm.Autocompletes("Email", term, "tblCrypts");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(string Sorting_Order = "", string Email = "", string Name = "", string Price = "", string Date = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            ViewBag.Name = Name;
            ViewBag.Price = Price;
            ViewBag.Date = Date;
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblCrypts.Name" ? "tblCrypts.Name_Desc" : "tblCrypts.Name";
            ViewBag.SortingPSurname = Sorting_Order == "tblCrypts.Price" ? "tblCrypts.Price_Desc" : "tblCrypts.Price";
            ViewBag.SortingDate = Sorting_Order == "tblCrypts.CreatedDate" ? "tblCrypts.CreatedDate_Desc" : "tblCrypts.CreatedDate";


            var tblCrypts = CryptsB.GetCrypts(Name, Price, dtDate, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Crypts", tblCrypts);
            }
            return View(tblCrypts);
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Details(int id)
        {
            tblCrypts tblCrypts = CryptsB.GetCryptsByID(id);
            ViewBag.CreatedBy = tblCrypts.CreatedByName;
            ViewBag.UpdatedBy = tblCrypts.UpdatedByName;
            if (tblCrypts == null)
                return HttpNotFound();
            else
            {
                return View(tblCrypts);
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            tblCrypts tblCrypts = new tblCrypts();
            tblCrypts.Active = true;
            ViewBag.States = StatesB.GetList();
            ViewBag.Towns = new List<tblTowns>();
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCrypts.CreatedDate = ServerDateTimeNow;
            tblCrypts.UpdatedDate = ServerDateTimeNow;
            tblCrypts.CreatedBy = LoggedUser;
            tblCrypts.UpdatedBy = LoggedUser;
            return View(tblCrypts);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCrypts tblCrypts)
        {

            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                if (hm.CheckIfExists("Name", tblCrypts.Name, "CryptID", tblCrypts.CryptID, "tblCrypts"))
                {
                    ModelState.AddModelError("", "Ya hay una cripta registrada con ese nombre");
                    return View(tblCrypts);
                }
                else
                {
                    Response response = CryptsB.Create(tblCrypts);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Data);
                        return View(tblCrypts);
                    }
                }
            }
            else
            {
                return View(tblCrypts);
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            tblCrypts tblCrypts = new tblCrypts();
            tblCrypts = CryptsB.GetCryptsByID(id);

            if (tblCrypts == null)
                return HttpNotFound();
            ViewBag.CreatedBy = tblCrypts.CreatedByName;
            ViewBag.UpdatedBy = tblCrypts.UpdatedByName;

            return View(tblCrypts);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCrypts tblCrypts)
        {
            ViewBag.CreatedBy = tblCrypts.CreatedByName;
            ViewBag.UpdatedBy = tblCrypts.UpdatedByName;

            if (ModelState.IsValid)
            {
                if (hm.CheckIfExists("Name", tblCrypts.Name, "CryptID", tblCrypts.CryptID, "tblCrypts"))
                {
                    ModelState.AddModelError("", "Ya hay una cripta registrada con ese nombre");
                    return View(tblCrypts);
                }

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblCrypts.UpdatedBy = LoggedUser;
                tblCrypts.UpdatedDate = ServerDateTimeNow;
                Response response = CryptsB.Update(tblCrypts);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblCrypts);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblCrypts);
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCrypts tblCrypts = CryptsB.GetCryptsByID(id);
            tblCrypts.DeletedBy = LoggedUser;
            tblCrypts.DeletedDate = ServerDateTimeNow;
            tblCrypts.Active = false;

            Response response = CryptsB.Update(tblCrypts);
            if (response.Result == Result.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", response.Data);
                return RedirectToAction("Details", new { id = id });
            }
        }

        #region Solicitud
        [Authorize(Roles = "Administrador,Ventas,Encargado")]
        public ActionResult Solicitud(int CustomerID = 0)
        {
            if (CustomerID != 0)
                TempData["CustomerID"] = CustomerID;
            else
                TempData["CustomerID"] = 0;

            ViewBag.DiscountID = DiscountsB.GetList2();
            return View();
        }
        [Authorize(Roles = "Administrador,Ventas,Encargado")]
        public PartialViewResult GetCryptSectionById(int iId = 0, string sAction = "")
        {
            bool bIsManage = sAction == "Manage";
            var model = CryptsSectionsB.GetById(iId);
            ViewBag.CryptPrices = CryptPricesB.GetList();
            ViewBag.DisabledCrypts = bIsManage ? new List<string>() : PurchasesRequestsB.GetListCryptsByCryptSectionId(iId);
            ViewBag.CryptPositionTypes = CryptPositionTypesB.GetListByCryptSectionId(iId);
            string sView = bIsManage ? "~/Views/CryptType/_SelectCrypto.cshtml" : "_SelectCrypto";
            return PartialView(sView, model);
        }
        [Authorize(Roles = "Administrador,Ventas,Encargado")]
        [HttpPost]
        public ActionResult SendToRequest(string sPosition, int iCryptSectionId, string sPrice, string sOriginalPrice, string sLevel, PaymentMethods sPaymentMethod, string sEnganche, CryptTypes eType, string sMensualidades = "", int DiscountID = 0)
        {
            TempData["CryptPosition"] = sPosition;
            TempData["CryptSectionId"] = iCryptSectionId;
            TempData["Price"] = sPrice;
            TempData["OriginalPrice"] = sOriginalPrice;
            TempData["Level"] = sLevel;
            TempData["PaymentMethod"] = sPaymentMethod;
            TempData["Enganche"] = sEnganche;
            TempData["Mensualidades"] = sMensualidades;
            TempData["Type"] = eType;
            TempData["DiscountID"] = DiscountID;
            return RedirectToAction("PurchaseRequest", "ProspectiveCustomer", new { id = TempData["CustomerID"] });
        }
        #endregion

        public ActionResult CryptPayments()
        {
            return View();
        }

        public ActionResult Ajax_Get_Crypts_By_CustomerID(int CustomerID)
        {
            List<tblPurchasesRequests> List_Crypts = new List<tblPurchasesRequests>();

            if (CustomerID != 0)
            {
                List_Crypts = PurchasesRequestsB.GetList_ByCustomerID(CustomerID);
            }

            return Json(List_Crypts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ajax_Get_CryptDetails(int PurchasesRequestID)
        {
            string error_message = "";
            string html = "";

            try
            {
                tblPurchasesRequests details = PurchasesRequestsB.Get_ByPurchasesRequestID(PurchasesRequestID, 0);

                var vPayments = PaymentsB.GetListByPurchaseRequestPaymentIds(details?.lstPayments?.Select(r => r.PurchaseRequestPaymentID).ToList());

                var newPayments = new List<tblPurchaseRequestPayments>();

                bool nextPaymentToDo = true;

                if (details.lstPayments != null && details.lstPayments.Count > 0)
                {
                    foreach (var item in details.lstPayments)
                    {
                        item.NextPayemntToDo = false;

                        if (item.Estatus == PaymentEstatus.Pending)
                        {
                            item.NextPayemntToDo = nextPaymentToDo;

                            nextPaymentToDo = false;
                        }

                        newPayments.Add(item);

                        if (item.HasInterest && (details.PaymentMethod == PaymentMethods.Meses18 ||
                        details.PaymentMethod == PaymentMethods.Meses24 || (details.PaymentMethod == PaymentMethods.Meses36) && details.Level <= 6))
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
                            itemOverdue.Amount = (item.Amount * (details.PaymentMethod == PaymentMethods.Meses18 ? 0.065M : details.PaymentMethod == PaymentMethods.Meses36 ? 0.145M : 0.1M));

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
                }

                details.lstPayments = newPayments;

                tblExchangeRates exchangeRates = ExchangeRatesB.GetLastReccord();
                details.Rate = exchangeRates.Rate;

                html = ControllerContext.RenderPartialToString("~/Views/Crypts/_CryptDetails.cshtml", details);
            }
            catch (Exception)
            {
                error_message = "Ocurrió un error al obtener los detalles de la cripta";
            }

            return Json(new { html = html, error_message = error_message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Ajax_Save_Payments(tblPayments payment)
        {
            string error_message = "";

            try
            {
                if (payment.File != null)
                {
                    int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);
                    DateTime ServerDateTimeNow = hm.GetDateTime();

                    string extension = Path.GetExtension(payment.File.FileName);

                    string imageName = $"{(payment.IsInterest ? "interest" : "payment")}-ticket-{payment.PurchaseRequestPaymentID}{extension}";

                    //var urlAzure = HelperAzure.SaveTicketFile(payment.File, imageName);

                    var urlAzure = HelperAzure.SaveTicketFile(payment.File);

                    if (!string.IsNullOrEmpty(urlAzure))
                    {
                        if (urlAzure != "")
                        {
                            payment.Estatus = ClientPaymentEnum.Pending;
                            payment.TicketUrl = urlAzure;
                            payment.Active = true;
                            payment.CreatedBy = LoggedUser;
                            payment.CreatedDate = ServerDateTimeNow;
                            payment.UpdatedBy = LoggedUser;
                            payment.UpdatedDate = ServerDateTimeNow;

                            tblExchangeRates exchangeRates = ExchangeRatesB.GetLastReccord();
                            payment.ExchangeRate = exchangeRates.Rate;

                            Response response = PaymentsB.Create(payment);

                            if (response.Result == Result.OK)
                            {
                                if (!payment.IsInterest)
                                {
                                    tblPurchaseRequestPayments purchaseRequestPayments = PurchaseRequestPaymentsB.GetById(
                                    payment.PurchaseRequestPaymentID);

                                    purchaseRequestPayments.Estatus = PaymentEstatus.ValidationProcess;

                                    response = PurchaseRequestPaymentsB.Update(purchaseRequestPayments);

                                    if (response.Result != Result.OK)
                                    {
                                        error_message = "Se subió el ticket pero no se pudo actualizar el pago el pago";
                                    }
                                }
                            }
                            else
                            {
                                error_message = "Ocurrió un error al guardar el pago";
                            }
                        }
                        else
                        {
                            error_message = "Ocurrió un error";
                        }

                        //if (!urlAzure.Contains("Ha ocurrido un Error: -"))
                        //{
                        //    payment.Estatus = ClientPaymentEnum.Pending;
                        //    payment.TicketUrl = urlAzure;
                        //    payment.Active = true;
                        //    payment.CreatedBy = LoggedUser;
                        //    payment.CreatedDate = ServerDateTimeNow;
                        //    payment.UpdatedBy = LoggedUser;
                        //    payment.UpdatedDate = ServerDateTimeNow;

                        //    tblExchangeRates exchangeRates = ExchangeRatesB.GetLastReccord();
                        //    payment.ExchangeRate = exchangeRates.Rate;

                        //    Response response = PaymentsB.Create(payment);

                        //    if (response.Result == Result.OK)
                        //    {
                        //        if (!payment.IsInterest)
                        //        {
                        //            tblPurchaseRequestPayments purchaseRequestPayments = PurchaseRequestPaymentsB.GetById(
                        //            payment.PurchaseRequestPaymentID);

                        //            purchaseRequestPayments.Estatus = PaymentEstatus.ValidationProcess;

                        //            response = PurchaseRequestPaymentsB.Update(purchaseRequestPayments);

                        //            if (response.Result != Result.OK)
                        //            {
                        //                error_message = "Se subió el ticket pero no se pudo actualizar el pago el pago";
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        error_message = "Ocurrió un error al guardar el pago";
                        //    }
                        //}
                        //else
                        //{
                        //    error_message = urlAzure;
                        //}
                    }
                    else
                    {
                        error_message = "No se guardó ticket para el pago";
                    }
                }
                else
                {
                    error_message = "No hay ticket a guardar para el pago";
                }
            }
            catch (Exception)
            {
                error_message = "Ocurrió un error al guardar el ticket para el pago";
            }

            return Json(new { error_message = error_message }, JsonRequestBehavior.AllowGet);
        }
    }
}