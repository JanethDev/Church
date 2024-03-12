using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;


namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado")]
    public class ExchangeRatesController : BaseController
    {
        ExchangeRatesB ExchangeRatesB = new ExchangeRatesB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        // GET: ExchangeRates
        public ActionResult Edit()
        {

            tblExchangeRates tblExchangeRates = ExchangeRatesB.GetLastReccord();
            if (tblExchangeRates == null)
            {
                tblExchangeRates = new tblExchangeRates();
            }
            else
            {
                ViewBag.CreatedBy = tblExchangeRates.CreatedByName;
                ViewBag.UpdatedBy = tblExchangeRates.UpdatedByName;
            }
            return View(tblExchangeRates);
        }
        [HttpPost]
        public ActionResult Edit(tblExchangeRates tblExchangeRates)
        {
            ViewBag.CreatedBy = tblExchangeRates.CreatedByName;
            ViewBag.UpdatedBy = tblExchangeRates.UpdatedByName;

            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblExchangeRates.UpdatedBy = LoggedUser;
                tblExchangeRates.UpdatedDate = ServerDateTimeNow;
                tblExchangeRates.CreatedBy = LoggedUser;
                tblExchangeRates.CreatedDate = ServerDateTimeNow;
                tblExchangeRates.Active = true;
                Response response = ExchangeRatesB.Create(tblExchangeRates);

                if (response.Result == Result.OK)
                {
                    ViewBag.Message = "Se actualizaron correctamente los datos";
                    return View(tblExchangeRates);
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblExchangeRates);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblExchangeRates);
            }
        }
    }
}