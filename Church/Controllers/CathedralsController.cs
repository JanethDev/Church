using System;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado")]
    public class CathedralsController : BaseController
    {
        CathedralsB Cathedrals = new CathedralsB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, table);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Index(string Sorting_Order = "", string Name = "", string Date = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            ViewBag.Name = Name;
            ViewBag.Date = Date;
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblCathedrals.Name" ? "tblCathedrals.Name_Desc" : "tblCathedrals.Name";
            ViewBag.SortingDate = Sorting_Order == "tblCathedrals.CreatedDate" ? "tblCathedrals.CreatedDate_Desc" : "tblCathedrals.CreatedDate";


            var tblCathedrals = Cathedrals.GetCathedrals(Name, dtDate, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Cathedrals", tblCathedrals);
            }
            return View(tblCathedrals);
        }

        public ActionResult Details(int id)
        {
            tblCathedrals tblCathedrals = Cathedrals.GetCathedralsByID(id);
            ViewBag.CreatedBy = tblCathedrals.CreatedByName;
            ViewBag.UpdatedBy = tblCathedrals.UpdatedByName;
            if (tblCathedrals == null)
                return HttpNotFound();
            else
            {
                return View(tblCathedrals);
            }
        }
        public ActionResult Create()
        {
            tblCathedrals tblCathedrals = new tblCathedrals();
            tblCathedrals.Active = true;
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCathedrals.CreatedDate = ServerDateTimeNow;
            tblCathedrals.UpdatedDate = ServerDateTimeNow;
            tblCathedrals.CreatedBy = LoggedUser;
            tblCathedrals.UpdatedBy = LoggedUser;
            return View(tblCathedrals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCathedrals tblCathedrals)
        {
            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                if (hm.CheckIfExists("Name", tblCathedrals.Name, "CathedralID", tblCathedrals.CathedralID, "tblCathedrals"))
                {
                    ModelState.AddModelError("", "Ya se encuentra un registro con el mismo nombre");
                    return View(tblCathedrals);
                }
                else
                {
                    Response response = Cathedrals.Create(tblCathedrals);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Data);
                        return View(tblCathedrals);
                    }
                }
            }
            else
            {
                return View(tblCathedrals);
            }
        }

        public ActionResult Edit(int id)
        {
            tblCathedrals tblCathedrals = new tblCathedrals();
            tblCathedrals = Cathedrals.GetCathedralsByID(id);

            if (tblCathedrals == null)
                return HttpNotFound();
            ViewBag.CreatedBy = tblCathedrals.CreatedByName;
            ViewBag.UpdatedBy = tblCathedrals.UpdatedByName;
            return View(tblCathedrals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCathedrals tblCathedrals)
        {
            ViewBag.CreatedBy = tblCathedrals.CreatedByName;
            ViewBag.UpdatedBy = tblCathedrals.UpdatedByName;

            if (ModelState.IsValid)
            {
                if (hm.CheckIfExists("Name", tblCathedrals.Name, "CathedralID", tblCathedrals.CathedralID, "tblCathedrals"))
                {
                    ModelState.AddModelError("", "Ya se encuentra un registro con el mismo nombre");
                    return View(tblCathedrals);
                }

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblCathedrals.UpdatedBy = LoggedUser;
                tblCathedrals.UpdatedDate = ServerDateTimeNow;
                Response response = Cathedrals.Update(tblCathedrals);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblCathedrals);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblCathedrals);
            }
        }

        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCathedrals tblCathedrals = Cathedrals.GetCathedralsByID(id);
            tblCathedrals.DeletedBy = LoggedUser;
            tblCathedrals.DeletedDate = ServerDateTimeNow;
            tblCathedrals.Active = false;

            Response response = Cathedrals.Update(tblCathedrals);
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
    }
}