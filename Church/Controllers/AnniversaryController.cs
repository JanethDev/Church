using System;
using System.Data;
using System.IO;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;
using Microsoft.Reporting.WebForms;
using MvcRazorToPdf;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado,Caja")]
    public class AnniversaryController : BaseController
    {
        AnniversaryB Anniversary = new AnniversaryB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, table);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(string Sorting_Order = "", string Name = "", string Date = "", string Hour = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            else if (!Request.IsAjaxRequest())
            {
                dtDate = hm.GetDateTime();
                Date = dtDate.ToShortDateString();
            }

            ViewBag.Name = Name;
            ViewBag.Date = Date;
            ViewBag.Hour = Hour;
            ViewBag.Page = page;
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblAnniversary.Name" ? "tblAnniversary.Name_Desc" : "tblAnniversary.Name";
            ViewBag.SortingName = Sorting_Order == "tblAnniversary.Hour" ? "tblAnniversary.Hour_Desc" : "tblAnniversary.Hour";
            ViewBag.SortingDate = Sorting_Order == "tblAnniversary.CreatedDate" ? "tblAnniversary.CreatedDate_Desc" : "tblAnniversary.CreatedDate";

            string filepathfinal = Server.MapPath("~");

            var tblAnniversary = Anniversary.GetAnniversary(Name, dtDate, Hour, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Anniversary", tblAnniversary);
            }
            return View(tblAnniversary);
        }

        public ActionResult Details(int id)
        {
            tblAnniversary tblAnniversary = Anniversary.GetAnniversaryByID(id);
            ViewBag.CreatedBy = tblAnniversary.CreatedByName;
            ViewBag.UpdatedBy = tblAnniversary.UpdatedByName;
            if (tblAnniversary == null)
                return HttpNotFound();
            else
            {
                return View(tblAnniversary);
            }
        }
        public ActionResult Create()
        {
            tblAnniversary tblAnniversary = new tblAnniversary();
            tblAnniversary.Active = true;
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblAnniversary.CreatedDate = ServerDateTimeNow;
            tblAnniversary.UpdatedDate = ServerDateTimeNow;
            tblAnniversary.CreatedBy = LoggedUser;
            tblAnniversary.UpdatedBy = LoggedUser;
            return View(tblAnniversary);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblAnniversary tblAnniversary)
        {

            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                if (hm.CheckIfExists("Name", tblAnniversary.Name, "AnniversaryID", tblAnniversary.AnniversaryID, "tblAnniversary"))
                {
                    ModelState.AddModelError("", "El aniversario luctuoso ya existe");
                    return View(tblAnniversary);
                }
                else
                {
                    Response response = Anniversary.Create(tblAnniversary);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Data);
                        return View(tblAnniversary);
                    }
                }
            }
            else
            {
                return View(tblAnniversary);
            }
        }

        public ActionResult Edit(int id)
        {
            tblAnniversary tblAnniversary = new tblAnniversary();
            tblAnniversary = Anniversary.GetAnniversaryByID(id);

            if (tblAnniversary == null)
                return HttpNotFound();
            ViewBag.CreatedBy = tblAnniversary.CreatedByName;
            ViewBag.UpdatedBy = tblAnniversary.UpdatedByName;
            return View(tblAnniversary);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAnniversary tblAnniversary)
        {
            ViewBag.CreatedBy = tblAnniversary.CreatedByName;
            ViewBag.UpdatedBy = tblAnniversary.UpdatedByName;

            if (ModelState.IsValid)
            {
                if (hm.CheckIfExists("Name", tblAnniversary.Name, "AnniversaryID", tblAnniversary.AnniversaryID, "tblAnniversary"))
                {
                    ModelState.AddModelError("", "El aniversario luctuoso ya existe");
                    return View(tblAnniversary);
                }

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblAnniversary.UpdatedBy = LoggedUser;
                tblAnniversary.UpdatedDate = ServerDateTimeNow;
                Response response = Anniversary.Update(tblAnniversary);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblAnniversary);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblAnniversary);
            }
        }

        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblAnniversary tblAnniversary = Anniversary.GetAnniversaryByID(id);
            tblAnniversary.DeletedBy = LoggedUser;
            tblAnniversary.DeletedDate = ServerDateTimeNow;
            tblAnniversary.Active = false;

            Response response = Anniversary.Update(tblAnniversary);
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

        public ActionResult GeneratePDF(string Sorting_Order = "", string Name = "", string Date = "", string Hour = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            var tblAnniversary = Anniversary.GetAnniversary(Name, dtDate, Hour, page, Sorting_Order);


            if (tblAnniversary is null)
                return HttpNotFound();

            return new PdfActionResult("AnniversaryPDF", tblAnniversary);

        }
    }
}