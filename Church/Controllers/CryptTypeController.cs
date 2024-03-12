using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CryptTypeController : BaseController
    {
        HelpMethods HelpMethods = new HelpMethods();
        CryptPositionTypesB CryptPositionTypesB = new CryptPositionTypesB();
        public ActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Manage(tblCryptPositionTypes model)
        {
            DateTime dtNow = HelpMethods.GetDateTime();
            int iLoggedUser = HelpMethods.GetIntbyString("UserName", HttpContext.User.Identity.Name, "UserID", "tblUsers");
            var vModelOriginal = CryptPositionTypesB.GetByCryptSectionId_CryptKey(model.CryptSectionID, model.CryptKey);
            bool bIsCreate = vModelOriginal == null;
            vModelOriginal = vModelOriginal ?? model;

            vModelOriginal.Type = model.Type;
            vModelOriginal.Active = true;
            vModelOriginal.CreatedBy = bIsCreate ? iLoggedUser : vModelOriginal.CreatedBy;
            vModelOriginal.UpdatedBy = iLoggedUser;
            vModelOriginal.CreatedDate = bIsCreate ? dtNow : vModelOriginal.CreatedDate;
            vModelOriginal.UpdatedDate = dtNow;

            var response = bIsCreate ? CryptPositionTypesB.Create(vModelOriginal) : CryptPositionTypesB.Update(vModelOriginal);
            object oResponse = new
            {
                Message = response.Result == Result.OK ? "Se guardo correctamente la información" : $"Ocurrio un error: {response.Result}",
                Class = response.Result == Result.OK ? "success" : "error",
                TypeName = vModelOriginal.TypeName
            };
            return Json(oResponse, JsonRequestBehavior.AllowGet);
        }
    }
}