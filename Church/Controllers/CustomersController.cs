using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Ventas,Encargado,Facturacion")]
    public class CustomersController : BaseController
    {
        CustomersB CustomersB = new CustomersB();
        PurchasesRequestsB PurchasesRequestsB = new PurchasesRequestsB();
        PurchasesRequestsQuotationB PurchasesRequestsQuotationB = new PurchasesRequestsQuotationB();

        CryptsSectionsB CryptsSectionsB = new CryptsSectionsB();
        BeneficiaryCustomersB BeneficiaryCustomersB = new BeneficiaryCustomersB();
        StatesB StatesB = new StatesB();
        TownsB TownsB = new TownsB();
        HelpMethods hm = new HelpMethods();
        DiscountsB DiscountsB = new DiscountsB();
        CommissionAgentsB CommissionAgentsB = new CommissionAgentsB();
        HelperAzure HelperAzure = new HelperAzure();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        CivilStatusB CivilStatusB = new CivilStatusB();

        public ActionResult acName(string term)
        {
            var model = hm.Autocompletes("Name", term, "tblCustomers");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult acPSurname(string term)
        {
            var model = hm.Autocompletes("PSurname", term, "tblCustomers");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult acEmail(string term)
        {
            var model = hm.Autocompletes("Email", term, "tblCustomers");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(string Sorting_Order = "", string Email = "", string Name = "", string PSurname = "", string Date = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            ViewBag.Email = Email;
            ViewBag.Name = Name;
            ViewBag.PSurname = PSurname;
            ViewBag.Date = Date;
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblCustomers.Name" ? "tblCustomers.Name_Desc" : "tblCustomers.Name";
            ViewBag.SortingPSurname = Sorting_Order == "tblCustomers.PSurname" ? "tblCustomers.PSurname_Desc" : "tblCustomers.PSurname";
            ViewBag.SortingEmail = Sorting_Order == "tblCustomers.Email" ? "tblCustomers.Email_Desc" : "tblCustomers.Email";
            ViewBag.SortingDate = Sorting_Order == "tblCustomers.CreatedDate" ? "tblCustomers.CreatedDate_Desc" : "tblCustomers.CreatedDate";


            var tblCustomers = CustomersB.GetCustomers(Name, PSurname, Email, "", dtDate, false, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Customers", tblCustomers);
            }
            return View(tblCustomers);
        }
        public ActionResult SolicitudDetails(int id, bool Quotation)
        {
            tblPurchasesRequests tblPurchasesRequests = null;
            if (Quotation)
                tblPurchasesRequests = PurchasesRequestsQuotationB.GetByIDToPurchasesRequests(id);
            else
                tblPurchasesRequests = PurchasesRequestsB.GetByID(id);

            tblCustomers tblCustomers = CustomersB.GetCustomersByID(tblPurchasesRequests.CustomerID);
            tblPurchasesRequests.Customer = tblCustomers.PSurname + " " + tblCustomers.MSurname + " " + tblCustomers.Name;
            tblPurchasesRequests.CustomerID = tblCustomers.CustomerID;
            tblPurchasesRequests.CelPhone = tblCustomers.CelPhone;
            tblPurchasesRequests.Email = tblCustomers.Email;
            tblPurchasesRequests.Name = tblCustomers.Name;
            tblPurchasesRequests.PSurname = tblCustomers.PSurname;
            tblPurchasesRequests.MSurname = tblCustomers.MSurname;

            tblPurchasesRequests.CelPhone = tblCustomers.CelPhone;
            tblPurchasesRequests.Email = tblCustomers.Email;
            tblPurchasesRequests.Address = tblCustomers.Address;
            tblPurchasesRequests.CityOfBirth = tblCustomers.CityOfBirth;
            tblPurchasesRequests.AddressNumber = tblCustomers.AddressNumber;
            tblPurchasesRequests.AdressInteriorNumber = tblCustomers.AdressInteriorNumber;
            tblPurchasesRequests.Neighborhood = tblCustomers.Neighborhood;
            tblPurchasesRequests.ZipCode = tblCustomers.ZipCode;
            tblPurchasesRequests.RFCCURP = tblCustomers.RFCCURP;
            tblPurchasesRequests.DateOfBirth = tblCustomers.DateOfBirth;
            tblPurchasesRequests.CivilStatus = tblCustomers.CivilStatus;
            tblPurchasesRequests.Occupation = tblCustomers.Occupation;
            tblPurchasesRequests.Company = tblCustomers.Company;
            tblPurchasesRequests.PhoneCompany = tblCustomers.PhoneCompany;
            tblPurchasesRequests.AddressCompany = tblCustomers.AddressCompany;
            tblPurchasesRequests.ExtPhoneCompany = tblCustomers.ExtPhoneCompany;
            tblPurchasesRequests.TownAddressCompany = tblCustomers.TownAddressCompany;
            tblPurchasesRequests.DelegationAddressCompany = tblCustomers.DelegationAddressCompany;
            tblPurchasesRequests.StateAddressCompany = tblCustomers.StateAddressCompany;
            tblPurchasesRequests.Income = tblCustomers.Income;
            tblPurchasesRequests.TownID = tblCustomers.TownID;
            tblPurchasesRequests.Delegation = tblCustomers.Delegation;
            tblPurchasesRequests.UserID = tblCustomers.UserID;
            tblPurchasesRequests.Town = tblPurchasesRequests.TownID != null ? TownsB.GetTownsByID((int)tblPurchasesRequests.TownID).Name : "";
            tblPurchasesRequests.State = tblPurchasesRequests.StateID != null ? StatesB.GetStatesByID(tblCustomers.StateID).Name : "";

            tblPurchasesRequests.tblCryptsSections = CryptsSectionsB.GetById(tblPurchasesRequests.CryptSectionID);
            tblDiscounts tblDiscounts = DiscountsB.GetDiscountsByID(tblPurchasesRequests.DiscountID);
            tblPurchasesRequests.DiscountDescription = tblDiscounts != null ? tblDiscounts.Description : "";
            tblPurchasesRequests.Discount = tblDiscounts != null ? tblDiscounts.DiscountID.ToString() : "";
            tblCommissionAgents tblCommissionAgents = tblPurchasesRequests.CommissionAgentID != null ? CommissionAgentsB.GetCommissionAgentsByID((int)tblPurchasesRequests.CommissionAgentID) : null;
            tblPurchasesRequests.CommissionAgent = tblCommissionAgents != null ? tblCommissionAgents.FullName : "";

            ViewBag.States = StatesB.GetList();
            ViewBag.Towns = TownsB.GetListByStateID(tblCustomers.StateID);
            ViewBag.BeneficiaryCustomers = BeneficiaryCustomersB.GetListByPurchasesRequestID(tblPurchasesRequests.PurchasesRequestID);
            return View(tblPurchasesRequests);
        }
        public ActionResult Solicitudes(int id)
        {
            List<tblPurchasesRequests> Solicitudes = PurchasesRequestsB.GetListByCustomerID(id);
            List<tblPurchasesRequests> Cotizaciones = PurchasesRequestsQuotationB.GetListByCustomerID(id);

            Solicitudes.AddRange(Cotizaciones);

            var tblCustomers = CustomersB.GetCustomersByID(id);
            ViewBag.Customer = (tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname).ToUpper();
            for (int i = 0; i < Solicitudes.Count; i++)
            {
                Solicitudes[i].tblCryptsSections = CryptsSectionsB.GetById(Solicitudes[i].CryptSectionID);
            }

            return View(Solicitudes);
        }
        public ActionResult Details(int id)
        {
            tblCustomers tblCustomers = CustomersB.GetCustomersByID(id);
            ViewBag.CreatedBy = tblCustomers.CreatedByName;
            ViewBag.UpdatedBy = tblCustomers.UpdatedByName;
            if (tblCustomers == null)
                return HttpNotFound();
            else
            {
                return View(tblCustomers);
            }
        }
        public ActionResult Create()
        {
            tblCustomers tblCustomers = new tblCustomers();
            tblCustomers.Active = true;
            ViewBag.States = StatesB.GetList();
            ViewBag.Towns = new List<tblTowns>();
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCustomers.CreatedDate = ServerDateTimeNow;
            tblCustomers.UpdatedDate = ServerDateTimeNow;
            tblCustomers.CreatedBy = LoggedUser;
            tblCustomers.UpdatedBy = LoggedUser;
            return View(tblCustomers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCustomers tblCustomers)
        {
            ViewBag.States = StatesB.GetList();
            ViewBag.Towns = TownsB.GetListByStateID(tblCustomers.StateID);

            if (ModelState.IsValid)
            {
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                if (hm.CheckIfExists("Email", tblCustomers.Email, "CustomerID", tblCustomers.CustomerID, "tblCustomers"))
                {
                    ModelState.AddModelError("", "El usuario ya existe");
                    return View(tblCustomers);
                }
                else
                {
                    int customerID = CustomersB.GetCustomerID();
                    if (customerID == 0)
                    {
                        tblCustomers.CustomerID = 35270;
                    }
                    else
                    {
                        tblCustomers.CustomerID = customerID + 8;
                    }
                    Response response = CustomersB.Create(tblCustomers);
                    if (response.Result == Result.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Data);
                        return View(tblCustomers);
                    }
                }
            }
            else
            {
                return View(tblCustomers);
            }
        }
        public ActionResult Edit(int id)
        {
            tblCustomers tblCustomers = new tblCustomers();
            tblCustomers = CustomersB.GetCustomersByID(id);
            ViewBag.CivilStatus = CivilStatusB.GetList();

            if (tblCustomers == null)
                return HttpNotFound();
            ViewBag.States = StatesB.GetList();
            ViewBag.Towns = TownsB.GetListByStateID(tblCustomers.StateID);
            ViewBag.CreatedBy = tblCustomers.CreatedByName;
            ViewBag.UpdatedBy = tblCustomers.UpdatedByName;

            return View(tblCustomers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCustomers tblCustomers)
        {
            ViewBag.CreatedBy = tblCustomers.CreatedByName;
            ViewBag.UpdatedBy = tblCustomers.UpdatedByName;
            ViewBag.States = StatesB.GetList();
            ViewBag.Towns = TownsB.GetListByStateID(tblCustomers.StateID);
            ViewBag.CivilStatus = CivilStatusB.GetList();

            if (ModelState.IsValid)
            {
                if (hm.CheckIfExists("Email", tblCustomers.Email, "CustomerID", tblCustomers.CustomerID, "tblCustomers"))
                {
                    ModelState.AddModelError("", "El usuario ya existe");
                    return View(tblCustomers);
                }

                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                tblCustomers.UpdatedBy = LoggedUser;
                tblCustomers.UpdatedDate = ServerDateTimeNow;

                tblCustomers tblCustomers2 = CustomersB.GetCustomersByID(tblCustomers.CustomerID);
                if (tblCustomers.CFDIFile != null)
                {
                    HttpPostedFileBase document = tblCustomers.CFDIFile;
                    string File = HelperAzure.SaveFile(document);
                    tblCustomers.CFDI = File;
                }
                else if (tblCustomers2 != null)
                {
                    tblCustomers.CFDI = tblCustomers2.CFDI;
                }


                Response response = CustomersB.Update(tblCustomers);

                if (response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblCustomers);
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrio un error");
                return View(tblCustomers);
            }
        }
        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblCustomers tblCustomers = CustomersB.GetCustomersByID(id);
            tblCustomers.DeletedBy = LoggedUser;
            tblCustomers.DeletedDate = ServerDateTimeNow;
            tblCustomers.Active = false;

            Response response = CustomersB.Update(tblCustomers);
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
