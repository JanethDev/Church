using System;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado")]
    public class StatesController : BaseController
    {

        StatesB States = new StatesB();
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
            ViewBag.SortingName = Sorting_Order == "tblStates.Name" ? "tblStates.Name_Desc" : "tblStates.Name";
            ViewBag.SortingDate = Sorting_Order == "tblStates.CreatedDate" ? "tblStates.CreatedDate_Desc" : "tblStates.CreatedDate";


            var tblStates = States.GetStates(Name, dtDate, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_States", tblStates);
            }
            return View(tblStates);
        }

        public ActionResult Details(int id)
        {
            tblStates tblStates = States.GetStatesByID(id);
            ViewBag.CreatedBy = tblStates.CreatedByName;
            ViewBag.UpdatedBy = tblStates.UpdatedByName;
            if (tblStates == null)
                return HttpNotFound();
            else
            {
                return View(tblStates);
            }
        }
        public ActionResult Create()
        {
            tblStates tblStates = new tblStates();
            tblStates.Active = true;
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblStates.CreatedDate = ServerDateTimeNow;
            tblStates.UpdatedDate = ServerDateTimeNow;
            tblStates.CreatedBy = LoggedUser;
            tblStates.UpdatedBy = LoggedUser;
            return View(tblStates);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblStates tblStates)
        {
            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                if (hm.CheckIfExists("Name", tblStates.Name, "StateID", tblStates.StateID, "tblStates"))
                {
                    ModelState.AddModelError("", "El estado ya existe");
                    return View(tblStates);
                }
                else
                {
                    Response response = States.Create(tblStates);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Data);
                        return View(tblStates);
                    }
                }
            }
            else
            {
                return View(tblStates);
            }
        }

        public ActionResult Edit(int id)
        {
            tblStates tblStates = new tblStates();
            tblStates = States.GetStatesByID(id);

            if (tblStates == null)
                return HttpNotFound();
            ViewBag.CreatedBy = tblStates.CreatedByName;
            ViewBag.UpdatedBy = tblStates.UpdatedByName;
            return View(tblStates);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblStates tblStates)
        {
            ViewBag.CreatedBy = tblStates.CreatedByName;
            ViewBag.UpdatedBy = tblStates.UpdatedByName;

            if (ModelState.IsValid)
            {
                if (hm.CheckIfExists("Name", tblStates.Name, "StateID", tblStates.StateID, "tblStates"))
                {
                    ModelState.AddModelError("", "El estado ya existe");
                    return View(tblStates);
                }

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblStates.UpdatedBy = LoggedUser;
                tblStates.UpdatedDate = ServerDateTimeNow;
                Response response = States.Update(tblStates);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblStates);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblStates);
            }
        }

        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblStates tblStates = States.GetStatesByID(id);
            tblStates.DeletedBy = LoggedUser;
            tblStates.DeletedDate = ServerDateTimeNow;
            tblStates.Active = false;

            Response response = States.Update(tblStates);
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
