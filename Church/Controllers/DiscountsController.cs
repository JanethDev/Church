using System;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Ventas,Encargado")]
    public class DiscountsController : BaseController
    {
        DiscountsB DiscountsB = new DiscountsB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, table);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Administrador")]
        public ActionResult Index(string Sorting_Order = "", string Name = "", string InitDate = "", string EndDate = "", int page = 1)
        {
            ViewBag.Name = Name;
            ViewBag.InitDate = InitDate;
            ViewBag.EndDate = EndDate;
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblDiscounts.Percentage" ? "tblDiscounts.Percentage_Desc" : "tblDiscounts.Percentage";
            ViewBag.SortingInitDate = Sorting_Order == "tblDiscounts.InitDate" ? "tblDiscounts.InitDate_Desc" : "tblDiscounts.InitDate";
            ViewBag.SortingEndDate = Sorting_Order == "tblDiscounts.EndDate" ? "tblDiscounts.EndDate_Desc" : "tblDiscounts.EndDate";


            var tblDiscounts = DiscountsB.GetDiscounts(Name, InitDate, EndDate, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Discounts", tblDiscounts);
            }
            return View(tblDiscounts);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Details(int id)
        {
            tblDiscounts tblDiscounts = DiscountsB.GetDiscountsByID(id);
            ViewBag.CreatedBy = tblDiscounts.CreatedByName;
            ViewBag.UpdatedBy = tblDiscounts.UpdatedByName;
            if (tblDiscounts == null)
                return HttpNotFound();
            else
            {
                return View(tblDiscounts);
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            tblDiscounts tblDiscounts = new tblDiscounts();
            tblDiscounts.Active = true;
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblDiscounts.CreatedDate = ServerDateTimeNow;
            tblDiscounts.UpdatedDate = ServerDateTimeNow;
            tblDiscounts.CreatedBy = LoggedUser;
            tblDiscounts.UpdatedBy = LoggedUser;
            tblDiscounts.InitDate = ServerDateTimeNow;
            tblDiscounts.EndDate= ServerDateTimeNow;
            return View(tblDiscounts);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblDiscounts tblDiscounts)
        {
            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                Response response = DiscountsB.Create(tblDiscounts);
                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblDiscounts);
                }
            }
            else
            {
                return View(tblDiscounts);
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            tblDiscounts tblDiscounts = new tblDiscounts();
            tblDiscounts = DiscountsB.GetDiscountsByID(id);

            if (tblDiscounts == null)
                return HttpNotFound();
            ViewBag.CreatedBy = tblDiscounts.CreatedByName;
            ViewBag.UpdatedBy = tblDiscounts.UpdatedByName;
            return View(tblDiscounts);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblDiscounts tblDiscounts)
        {
            ViewBag.CreatedBy = tblDiscounts.CreatedByName;
            ViewBag.UpdatedBy = tblDiscounts.UpdatedByName;

            if (ModelState.IsValid)
            {

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblDiscounts.UpdatedBy = LoggedUser;
                tblDiscounts.UpdatedDate = ServerDateTimeNow;
                Response response = DiscountsB.Update(tblDiscounts);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblDiscounts);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblDiscounts);
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblDiscounts tblDiscounts = DiscountsB.GetDiscountsByID(id);
            tblDiscounts.DeletedBy = LoggedUser;
            tblDiscounts.DeletedDate = ServerDateTimeNow;
            tblDiscounts.Active = false;

            Response response = DiscountsB.Update(tblDiscounts);
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

        public JsonResult GetCurrentDiscount(decimal dCurrentPrice = 0)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            var vDiscount = DiscountsB.GetByDate(ServerDateTimeNow);

            decimal dDiscount = vDiscount != null ? (dCurrentPrice * vDiscount.Percentage) / 100 : 0;
            dCurrentPrice = dCurrentPrice - dDiscount;

            object oResponse = new
            {
                Percentage = vDiscount?.Percentage,
                NewPrice = dCurrentPrice,
                NewPriceFormat = dCurrentPrice.ToString("C2"),
                Found = vDiscount != null
            };
            return Json(oResponse, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDiscountByID(decimal dCurrentPrice = 0, int DiscountID = 0)
        {
            var vDiscount = DiscountsB.GetDiscountsByID(DiscountID);

            decimal dDiscount = vDiscount != null ? (dCurrentPrice * vDiscount.Percentage) / 100 : 0;
            decimal dTotal = dCurrentPrice;
            dCurrentPrice = dCurrentPrice - dDiscount;

            object oResponse = new
            {
                Percentage = vDiscount?.Percentage,
                NewPrice = dCurrentPrice,
                NewPriceFormat = dCurrentPrice.ToString("C2"),
                Found = vDiscount != null,
                Total = dTotal
            };
            return Json(oResponse, JsonRequestBehavior.AllowGet);
        }
    }
}