using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado,Facturacion")]
    public class MaintenanceFeeController : BaseController
    {
        MaintenanceFeeB MaintenanceFeeB = new MaintenanceFeeB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        // GET: MaintenanceFee
        public ActionResult Edit()
        {
            tblMaintenanceFee tblMaintenanceFee = new tblMaintenanceFee();
            tblMaintenanceFee tblMaintenanceFeeShared = MaintenanceFeeB.GetLastReccord(1);
            tblMaintenanceFee tblMaintenanceFeeFamiliar = MaintenanceFeeB.GetLastReccord(0);
            if (tblMaintenanceFeeShared == null && tblMaintenanceFeeFamiliar == null)
            {
                tblMaintenanceFee = new tblMaintenanceFee();
            }
            else
            {
                tblMaintenanceFee.SharedCost = tblMaintenanceFeeShared == null? 0: tblMaintenanceFeeShared.Cost;
                tblMaintenanceFee.FamiliarCost = tblMaintenanceFeeFamiliar == null ? 0 : tblMaintenanceFeeFamiliar.Cost;
                ViewBag.CreatedBy = tblMaintenanceFee.CreatedByName;
                ViewBag.UpdatedBy = tblMaintenanceFee.UpdatedByName;
            }
            return View(tblMaintenanceFee);
        }
        [HttpPost]
        public ActionResult Edit(tblMaintenanceFee model)
        {
            ViewBag.CreatedBy = model.CreatedByName;
            ViewBag.UpdatedBy = model.UpdatedByName;

            tblMaintenanceFee tblMaintenanceFeeShared = MaintenanceFeeB.GetLastReccord(1);
            tblMaintenanceFee tblMaintenanceFeeFamiliar = MaintenanceFeeB.GetLastReccord(0);

            if (ModelState.IsValid && 
            ((tblMaintenanceFeeShared == null || tblMaintenanceFeeShared.Cost != model.SharedCost) || 
            (tblMaintenanceFeeFamiliar == null || tblMaintenanceFeeFamiliar.Cost != model.FamiliarCost)))
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);
                Response response1 = null;
                Response response2 = null;
                if (tblMaintenanceFeeShared == null || tblMaintenanceFeeShared.Cost != model.SharedCost)
                {
                    tblMaintenanceFee tblMaintenanceFee = new tblMaintenanceFee();
                    tblMaintenanceFee.Cost = model.SharedCost;
                    tblMaintenanceFee.IsShared = true;
                    tblMaintenanceFee.UpdatedBy = LoggedUser;
                    tblMaintenanceFee.UpdatedDate = ServerDateTimeNow;
                    tblMaintenanceFee.CreatedBy = LoggedUser;
                    tblMaintenanceFee.CreatedDate = ServerDateTimeNow;
                    tblMaintenanceFee.Active = true;
                    response1 = MaintenanceFeeB.Create(tblMaintenanceFee);
                }
                if(tblMaintenanceFeeFamiliar == null || tblMaintenanceFeeFamiliar.Cost != model.FamiliarCost)
                {
                    tblMaintenanceFee tblMaintenanceFee = new tblMaintenanceFee();
                    tblMaintenanceFee.Cost = model.FamiliarCost;
                    tblMaintenanceFee.IsShared = false;
                    tblMaintenanceFee.UpdatedBy = LoggedUser;
                    tblMaintenanceFee.UpdatedDate = ServerDateTimeNow;
                    tblMaintenanceFee.CreatedBy = LoggedUser;
                    tblMaintenanceFee.CreatedDate = ServerDateTimeNow;
                    tblMaintenanceFee.Active = true;
                    response2 = MaintenanceFeeB.Create(tblMaintenanceFee);
                }

                if ((response1 == null || response1.Result == Result.OK) && (response2 == null || response2.Result == Result.OK))
                {
                    ViewBag.Message = "Se actualizaron correctamente los datos";
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", response1.Data);
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(model);
            }
        }
    }
}