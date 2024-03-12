using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado")]
    public class FederalTaxController : BaseController
    {
        FederalTaxB FederalTaxB = new FederalTaxB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        // GET: FederalTax
        public ActionResult Edit()
        {
            
            tblFederalTax tblFederalTax = FederalTaxB.GetLastReccord();
            if (tblFederalTax == null)
            {
                tblFederalTax = new tblFederalTax();
            }
            else
            {
                ViewBag.CreatedBy = tblFederalTax.CreatedByName;
                ViewBag.UpdatedBy = tblFederalTax.UpdatedByName;
            }
            return View(tblFederalTax);
        }
        [HttpPost]
        public ActionResult Edit(tblFederalTax tblFederalTax)
        {
            ViewBag.CreatedBy = tblFederalTax.CreatedByName;
            ViewBag.UpdatedBy = tblFederalTax.UpdatedByName;

            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblFederalTax.UpdatedBy = LoggedUser;
                tblFederalTax.UpdatedDate = ServerDateTimeNow;
                tblFederalTax.CreatedBy = LoggedUser;
                tblFederalTax.CreatedDate = ServerDateTimeNow;
                tblFederalTax.Active = true;
                Response response = FederalTaxB.Create(tblFederalTax);

                if (response.Result == Result.OK)
                {
                    ViewBag.Message = "Se actualizaron correctamente los datos";
                    return View(tblFederalTax);
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblFederalTax);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblFederalTax);
            }
        }
    }
}