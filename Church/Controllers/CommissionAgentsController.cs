using System;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado")]
    public class CommissionAgentsController : BaseController
    {
        CathedralsB CathedralsB = new CathedralsB();
        CitiesB CitiesB = new CitiesB();
        CommissionAgentsB CommissionAgents = new CommissionAgentsB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, table);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCathedralByCityID(int iCityID = 0)
        {
            var vModel = CathedralsB.GetListByCityID(iCityID);
            return Json(vModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(string Sorting_Order = "", string Name = "", string PSurname = "", string CathedralID = "", string Date = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            ViewBag.Name = Name;
            ViewBag.PSurname = PSurname;
            ViewBag.Cathedral = CathedralID;
            ViewBag.Date = Date;
            ViewBag.Cathedrals = new SelectList(CathedralsB.GetListByCityID(), "Name", "Name", CathedralID);
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblCommissionAgents.Name" ? "tblCommissionAgents.Name_Desc" : "tblCommissionAgents.Name";
            ViewBag.SortingPSurname = Sorting_Order == "tblUsers.PSurname" ? "tblUsers.PSurname_Desc" : "tblUsers.PSurname";
            ViewBag.SortingCathedral = Sorting_Order == "tblCathedrals.Name" ? "tblCathedrals.Name_Desc" : "tblCathedrals.Name";
            ViewBag.SortingDate = Sorting_Order == "tblCommissionAgents.CreatedDate" ? "tblCommissionAgents.CreatedDate_Desc" : "tblCommissionAgents.CreatedDate";


            var tblCommissionAgents = CommissionAgents.GetCommissionAgents(Name, PSurname, CathedralID, dtDate, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CommissionAgents", tblCommissionAgents);
            }
            return View(tblCommissionAgents);
        }

        public ActionResult Details(int id)
        {
            tblCommissionAgents tblCommissionAgents = CommissionAgents.GetCommissionAgentsByID(id);
            ViewBag.CreatedBy = tblCommissionAgents.CreatedByName;
            ViewBag.UpdatedBy = tblCommissionAgents.UpdatedByName;
            if (tblCommissionAgents == null)
                return HttpNotFound();
            else
            {
                return View(tblCommissionAgents);
            }
        }
        public ActionResult Create()
        {
            tblCommissionAgents tblCommissionAgents = new tblCommissionAgents();
            tblCommissionAgents.Active = true;
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCommissionAgents.CreatedDate = ServerDateTimeNow;
            tblCommissionAgents.UpdatedDate = ServerDateTimeNow;
            tblCommissionAgents.CreatedBy = LoggedUser;
            tblCommissionAgents.UpdatedBy = LoggedUser;
            ViewBag.Cities = CitiesB.GetList();
            return View(tblCommissionAgents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCommissionAgents tblCommissionAgents)
        {
            ViewBag.Cities = CitiesB.GetList();
            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                    Response response = CommissionAgents.Create(tblCommissionAgents);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Data);
                        return View(tblCommissionAgents);
                    }
                
            }
            else
            {
                return View(tblCommissionAgents);
            }
        }

        public ActionResult Edit(int id)
        {
            tblCommissionAgents tblCommissionAgents = new tblCommissionAgents();
            tblCommissionAgents = CommissionAgents.GetCommissionAgentsByID(id);

            if (tblCommissionAgents == null)
                return HttpNotFound();
            ViewBag.CreatedBy = tblCommissionAgents.CreatedByName;
            ViewBag.UpdatedBy = tblCommissionAgents.UpdatedByName;
            ViewBag.Cities = CitiesB.GetList();
            ViewBag.Cathedrals = CathedralsB.GetListByCityID(tblCommissionAgents.CityID);
            return View(tblCommissionAgents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCommissionAgents tblCommissionAgents)
        {
            ViewBag.Cities = CitiesB.GetList();
            ViewBag.Cathedrals = CathedralsB.GetListByCityID(tblCommissionAgents.CityID);
            ViewBag.CreatedBy = tblCommissionAgents.CreatedByName;
            ViewBag.UpdatedBy = tblCommissionAgents.UpdatedByName;

            if (ModelState.IsValid)
            {

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblCommissionAgents.UpdatedBy = LoggedUser;
                tblCommissionAgents.UpdatedDate = ServerDateTimeNow;
                Response response = CommissionAgents.Update(tblCommissionAgents);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblCommissionAgents);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblCommissionAgents);
            }
        }

        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCommissionAgents tblCommissionAgents = CommissionAgents.GetCommissionAgentsByID(id);
            tblCommissionAgents.DeletedBy = LoggedUser;
            tblCommissionAgents.DeletedDate = ServerDateTimeNow;
            tblCommissionAgents.Active = false;

            Response response = CommissionAgents.Update(tblCommissionAgents);
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