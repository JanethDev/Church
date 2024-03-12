using System;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Ventas,Encargado")]

    public class UsersController : BaseController
    {
        UsersB users = new UsersB();
        CustomersB CustomersB = new CustomersB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        
        public ActionResult CheckIfExist(string Email, int UserID)
        {
            tblUsers tblUsers = users.GetUserByEmail(Email);
            if(tblUsers == null  || tblUsers.UserID == UserID)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckIfExistCustomer(string Email, int CustomerID)
        {
            tblCustomers tblCustomers1 = CustomersB.GetCustomersByID(CustomerID);
            tblCustomers tblCustomers2 = CustomersB.GetCustomersByEmail(Email);
            if(CustomerID == 0 && tblCustomers2 != null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult acUsername(string term)
        {
            var model = hm.Autocompletes("Username",term, table);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, table);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult acPSurname(string term)
        {
            var model = hm.Autocompletes("PSurname", term, table);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Administrador,Encargado")]

        public ActionResult Index(string Sorting_Order = "", string Username = "", string Name = "", string PSurname = "", string RolID = "", string Date = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            ViewBag.Username = Username;
            ViewBag.Name = Name;
            ViewBag.PSurname = PSurname;
            ViewBag.RolID = RolID;
            ViewBag.Date = Date;
            ViewBag.Roles = new SelectList(RolesB.GetList(), "RolID", "Name", RolID);
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblUsers.Name" ? "tblUsers.Name_Desc" : "tblUsers.Name";
            ViewBag.SortingPSurname = Sorting_Order == "tblUsers.PSurname" ? "tblUsers.PSurname_Desc" : "tblUsers.PSurname";
            ViewBag.SortingUserName = Sorting_Order == "tblUsers.UserName" ? "tblUsers.UserName_Desc" : "tblUsers.UserName";
            ViewBag.SortingRol = Sorting_Order == "tblRoles.Name" ? "tblRoles.Name_Desc" : "tblRoles.Name";
            ViewBag.SortingDate = Sorting_Order == "tblUsers.CreatedDate" ? "tblUsers.CreatedDate_Desc" : "tblUsers.CreatedDate";


            var tblusers = users.GetUsers(Username, Name, PSurname, RolID, dtDate, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Users", tblusers);
            }
            return View(tblusers);
        }
        [Authorize(Roles = "Administrador,Encargado")]
        public ActionResult Details(int id)
        {
            tblUsers tblusers = users.GetUserByID(id);
            ViewBag.CreatedBy = tblusers.CreatedByName;
            ViewBag.UpdatedBy = tblusers.UpdatedByName;
            if (tblusers == null)
                return HttpNotFound();
            else
            {
                return View(tblusers);
            }
        }
        [Authorize(Roles = "Administrador,Encargado")]
        public ActionResult Create()
        {
            tblUsers tblusers = new tblUsers();
            tblusers.Active = true;
            ViewBag.Roles = RolesB.GetList();
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column,HttpContext.User.Identity.Name, columnid,table);         

            tblusers.CreatedDate = ServerDateTimeNow;
            tblusers.UpdatedDate = ServerDateTimeNow;
            tblusers.CreatedBy = LoggedUser;
            tblusers.UpdatedBy = LoggedUser;
            return View(tblusers);
        }
        [Authorize(Roles = "Administrador,Encargado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblUsers tblusers)
        {
            ViewBag.Roles = RolesB.GetList();

            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);               

                if (hm.CheckIfExists(column, tblusers.UserName, columnid, tblusers.UserID, table))
                {
                    ModelState.AddModelError("", "El usuario ya existe");                    
                    return View(tblusers);
                }
                else
                {
                    tblusers.Password = PasswordSecurity.Encrypt(tblusers.Password);
                    tblusers.ConfirmPassword = PasswordSecurity.Encrypt(tblusers.ConfirmPassword);
                    Response response = users.Create(tblusers);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {                       
                        ModelState.AddModelError("", response.Data);
                        return View(tblusers);
                    }
                }
            }
            else
            {               
                return View(tblusers);
            }
        }
        [Authorize(Roles = "Administrador,Encargado")]
        public ActionResult Edit(int id)
        {
            tblUsers tblusers = new tblUsers();
            tblusers = users.GetUserByID(id);

            if (tblusers == null)
                return HttpNotFound();
            ViewBag.CreatedBy = tblusers.CreatedByName;
            ViewBag.UpdatedBy = tblusers.UpdatedByName;
            ViewBag.Roles = RolesB.GetList();
            tblusers.Password = PasswordSecurity.Decrypt(tblusers.Password);
            tblusers.ConfirmPassword = tblusers.Password;

            return View(tblusers);
        }
        [Authorize(Roles = "Administrador,Encargado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblUsers tblusers)
        {
            ViewBag.CreatedBy = tblusers.CreatedByName;
            ViewBag.UpdatedBy = tblusers.UpdatedByName;
            ViewBag.Roles = RolesB.GetList();

            if (ModelState.IsValid)
            {
                if (hm.CheckIfExists(column, tblusers.UserName, columnid, tblusers.UserID, table))
                {
                    tblusers.ConfirmPassword = tblusers.Password;
                    ModelState.AddModelError("", "El usuario ya existe");                 
                    return View(tblusers);
                }

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);
              
                tblusers.UpdatedBy = LoggedUser;
                tblusers.UpdatedDate = ServerDateTimeNow;
                tblusers.Password = PasswordSecurity.Encrypt(tblusers.Password);
                tblusers.ConfirmPassword = PasswordSecurity.Encrypt(tblusers.ConfirmPassword);
                Response response = users.Update(tblusers);
            
                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    tblusers.ConfirmPassword = tblusers.Password;
                    ModelState.AddModelError("", response.Data);                 
                    return View(tblusers);
                }
            }
            else
            {
                tblusers.ConfirmPassword = tblusers.Password;
                ModelState.AddModelError("", "Ocurrio un error");             
                return View(tblusers);
            }
        }

        [Authorize(Roles = "Administrador,Encargado")]
        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);
         
            tblUsers tblusers = users.GetUserByID(id);
            tblusers.DeletedBy = LoggedUser;
            tblusers.DeletedDate = ServerDateTimeNow;
            tblusers.ConfirmPassword = tblusers.Password;
            tblusers.Active = false;

            Response response = users.Update(tblusers);
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