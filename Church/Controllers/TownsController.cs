using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Ventas,Encargado")]
    public class TownsController : BaseController
    {
        TownsB Towns = new TownsB();
        StatesB StatesB = new StatesB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

        public ActionResult GetByStateID(int StateID)
        {
            var model = Towns.GetListByStateID(StateID);
            return Json(model, JsonRequestBehavior.AllowGet);

        }
        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, "tblTowns");
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Index(string Sorting_Order = "", string Name = "", string StateID = "", string Date = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            ViewBag.Name = Name;
            ViewBag.Date = Date;
            ViewBag.StateID = StateID;
            ViewBag.States = new SelectList(StatesB.GetList(), "StateID", "Name", StateID);
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblTowns.Name" ? "tblTowns.Name_Desc" : "tblTowns.Name";
            ViewBag.SortingDate = Sorting_Order == "tblTowns.CreatedDate" ? "tblTowns.CreatedDate_Desc" : "tblTowns.CreatedDate";
            ViewBag.SortingState = Sorting_Order == "tblStates.Name" ? "tblStates.Name_Desc" : "tblStates.Name";

            var tblTowns = Towns.GetTowns(Name, dtDate, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Towns", tblTowns);
            }
            return View(tblTowns);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Details(int id)
        {
            tblTowns tblTowns = Towns.GetTownsByID(id);
            ViewBag.CreatedBy = tblTowns.CreatedByName;
            ViewBag.UpdatedBy = tblTowns.UpdatedByName;
            if (tblTowns == null)
                return HttpNotFound();
            else
            {
                return View(tblTowns);
            }
        }
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            tblTowns tblTowns = new tblTowns();
            tblTowns.Active = true;
            DateTime ServerDateTimeNow = hm.GetDateTime();
            ViewBag.States = StatesB.GetList();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblTowns.CreatedDate = ServerDateTimeNow;
            tblTowns.UpdatedDate = ServerDateTimeNow;
            tblTowns.CreatedBy = LoggedUser;
            tblTowns.UpdatedBy = LoggedUser;
            return View(tblTowns);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblTowns tblTowns)
        {
            ViewBag.States = StatesB.GetList();
            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                if (hm.CheckIfExists("Name", tblTowns.Name, "TownID", tblTowns.TownID, "tblTowns"))
                {
                    ModelState.AddModelError("", "La ciudad ya está registrada");
                    return View(tblTowns);
                }
                else
                {
                    Response response = Towns.Create(tblTowns);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Data);
                        return View(tblTowns);
                    }
                }
            }
            else
            {
                return View(tblTowns);
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            tblTowns tblTowns = new tblTowns();
            tblTowns = Towns.GetTownsByID(id);

            if (tblTowns == null)
                return HttpNotFound();
            ViewBag.States = StatesB.GetList();
            ViewBag.CreatedBy = tblTowns.CreatedByName;
            ViewBag.UpdatedBy = tblTowns.UpdatedByName;
            return View(tblTowns);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblTowns tblTowns)
        {
            ViewBag.CreatedBy = tblTowns.CreatedByName;
            ViewBag.UpdatedBy = tblTowns.UpdatedByName;
            ViewBag.States = StatesB.GetList();

            if (ModelState.IsValid)
            {
                if (hm.CheckIfExists("Name", tblTowns.Name, "TownID", tblTowns.TownID, "tblTowns"))
                {
                    ModelState.AddModelError("", "La ciudad ya está registrada");
                    return View(tblTowns);
                }

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblTowns.UpdatedBy = LoggedUser;
                tblTowns.UpdatedDate = ServerDateTimeNow;
                Response response = Towns.Update(tblTowns);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblTowns);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblTowns);
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblTowns tblTowns = Towns.GetTownsByID(id);
            tblTowns.DeletedBy = LoggedUser;
            tblTowns.DeletedDate = ServerDateTimeNow;
            tblTowns.Active = false;

            Response response = Towns.Update(tblTowns);
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