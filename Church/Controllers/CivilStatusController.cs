using System;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado")]
    public class CivilStatusController : BaseController
    {
        CivilStatusB CivilStatus = new CivilStatusB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, table);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Index(string Sorting_Order = "", string Name = "",  string Date = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            ViewBag.Name = Name;
            ViewBag.Date = Date;
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblCivilStatus.Name" ? "tblCivilStatus.Name_Desc" : "tblCivilStatus.Name";
            ViewBag.SortingDate = Sorting_Order == "tblCivilStatus.CreatedDate" ? "tblCivilStatus.CreatedDate_Desc" : "tblCivilStatus.CreatedDate";


            var tblCivilStatus = CivilStatus.GetCivilStatus(Name, dtDate, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CivilStatus", tblCivilStatus);
            }
            return View(tblCivilStatus);
        }

        public ActionResult Details(int id)
        {
            tblCivilStatus tblCivilStatus = CivilStatus.GetCivilStatusByID(id);
            ViewBag.CreatedBy = tblCivilStatus.CreatedByName;
            ViewBag.UpdatedBy = tblCivilStatus.UpdatedByName;
            if (tblCivilStatus == null)
                return HttpNotFound();
            else
            {
                return View(tblCivilStatus);
            }
        }
        public ActionResult Create()
        {
            tblCivilStatus tblCivilStatus = new tblCivilStatus();
            tblCivilStatus.Active = true;
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCivilStatus.CreatedDate = ServerDateTimeNow;
            tblCivilStatus.UpdatedDate = ServerDateTimeNow;
            tblCivilStatus.CreatedBy = LoggedUser;
            tblCivilStatus.UpdatedBy = LoggedUser;
            return View(tblCivilStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCivilStatus tblCivilStatus)
        {
            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                if (hm.CheckIfExists("Name", tblCivilStatus.Name, "CivilStatusID", tblCivilStatus.CivilStatusID, "tblCivilStatus"))
                {
                    ModelState.AddModelError("", "El estado civil ya existe");
                    return View(tblCivilStatus);
                }
                else
                {
                    Response response = CivilStatus.Create(tblCivilStatus);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Data);
                        return View(tblCivilStatus);
                    }
                }
            }
            else
            {
                return View(tblCivilStatus);
            }
        }

        public ActionResult Edit(int id)
        {
            tblCivilStatus tblCivilStatus = new tblCivilStatus();
            tblCivilStatus = CivilStatus.GetCivilStatusByID(id);

            if (tblCivilStatus == null)
                return HttpNotFound();
            ViewBag.CreatedBy = tblCivilStatus.CreatedByName;
            ViewBag.UpdatedBy = tblCivilStatus.UpdatedByName;
            return View(tblCivilStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCivilStatus tblCivilStatus)
        {
            ViewBag.CreatedBy = tblCivilStatus.CreatedByName;
            ViewBag.UpdatedBy = tblCivilStatus.UpdatedByName;

            if (ModelState.IsValid)
            {
                if (hm.CheckIfExists("Name", tblCivilStatus.Name, "CivilStatusID", tblCivilStatus.CivilStatusID, "tblCivilStatus"))
                {
                    ModelState.AddModelError("", "El estado civil ya existe");
                    return View(tblCivilStatus);
                }

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblCivilStatus.UpdatedBy = LoggedUser;
                tblCivilStatus.UpdatedDate = ServerDateTimeNow;
                Response response = CivilStatus.Update(tblCivilStatus);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblCivilStatus);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblCivilStatus);
            }
        }

        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCivilStatus tblCivilStatus = CivilStatus.GetCivilStatusByID(id);
            tblCivilStatus.DeletedBy = LoggedUser;
            tblCivilStatus.DeletedDate = ServerDateTimeNow;
            tblCivilStatus.Active = false;

            Response response = CivilStatus.Update(tblCivilStatus);
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
