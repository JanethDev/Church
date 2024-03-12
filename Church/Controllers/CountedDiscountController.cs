using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado,Ventas")]
    public class CountedDiscountController : BaseController
    {
        CountedDiscountB CountedDiscountB = new CountedDiscountB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        // GET: CountedDiscount

        [Authorize(Roles = "Administrador,Encargado")]
        public ActionResult Edit()
        {
            tblCountedDiscount tblCountedDiscount = CountedDiscountB.GetLastReccord();
            if (tblCountedDiscount == null)
            {
                tblCountedDiscount = new tblCountedDiscount();
            }
            else
            {
                ViewBag.CreatedBy = tblCountedDiscount.CreatedByName;
                ViewBag.UpdatedBy = tblCountedDiscount.UpdatedByName;
            }
            return View(tblCountedDiscount);
        }

        [Authorize(Roles = "Administrador,Encargado")]
        [HttpPost]
        public ActionResult Edit(tblCountedDiscount tblCountedDiscount)
        {
            ViewBag.CreatedBy = tblCountedDiscount.CreatedByName;
            ViewBag.UpdatedBy = tblCountedDiscount.UpdatedByName;

            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblCountedDiscount.UpdatedBy = LoggedUser;
                tblCountedDiscount.UpdatedDate = ServerDateTimeNow;
                tblCountedDiscount.CreatedBy = LoggedUser;
                tblCountedDiscount.CreatedDate = ServerDateTimeNow;
                tblCountedDiscount.Active = true;
                Response response = CountedDiscountB.Create(tblCountedDiscount);

                if (response.Result == Result.OK)
                {
                    ViewBag.Message = "Se actualizaron correctamente los datos";
                    return View(tblCountedDiscount);
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblCountedDiscount);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblCountedDiscount);
            }
        }

        public ActionResult GetCountedDiscount(decimal Price = 0)
        {
            var model = CountedDiscountB.GetLastReccord();
            if(model != null && Price != 0)
            {
                decimal dDiscount = Convert.ToDecimal(model.Amount) / 100;
                dDiscount = Price * dDiscount;
                Price = Price - dDiscount;
            }
            return Json(Price.ToString("C2"), JsonRequestBehavior.AllowGet);
        }
    }
}