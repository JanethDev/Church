using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Church.Helpers;
using Church.HtmlHelpers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Web;
using AutoMapper;
using System.Data;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Ventas,Encargado")]
    public class ProspectiveCustomerController : BaseController
    {
        FederalTaxB FederalTaxB = new FederalTaxB();
        MaintenanceFeeB MaintenanceFeeB = new MaintenanceFeeB();
        CustomersB CustomersB = new CustomersB();
        UsersB UsersB = new UsersB();
        StatesB StatesB = new StatesB();
        TownsB TownsB = new TownsB();
        BeneficiaryCustomersB BeneficiaryCustomersB = new BeneficiaryCustomersB();
        PurchasesRequestsB PurchasesRequestsB = new PurchasesRequestsB();
        HelpMethods hm = new HelpMethods();
        CryptsSectionsB CryptsSectionsB = new CryptsSectionsB();
        CivilStatusB CivilStatusB = new CivilStatusB();
        HelperAzure HelperAzure = new HelperAzure();
        DiscountsB DiscountsB = new DiscountsB();
        PurchasesRequestsQuotationB PurchasesRequestsQuotationB = new PurchasesRequestsQuotationB();
        PurchaseRequestPaymentsB PurchaseRequestPaymentsB = new PurchaseRequestPaymentsB();
        ContractsB ContractsB = new ContractsB();
        CommissionAgentsB CommissionAgentsB = new CommissionAgentsB();

        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

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
        public ActionResult acCelphone(string term)
        {
            var model = hm.Autocompletes("Celphone", term, "tblCustomers");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult acCustomers(string term)
        {
            var model = CustomersB.SearchByName(term);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index(string Sorting_Order = "", string CelPhone = "", string Name = "", string PSurname = "", string Date = "", int page = 1)
        {
            DateTime dtDate = new DateTime();
            if (Date != "")
                dtDate = DateTime.Parse(Date);
            ViewBag.CelPhone = CelPhone;
            ViewBag.Name = Name;
            ViewBag.PSurname = PSurname;
            ViewBag.Date = Date;
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblCustomers.Name" ? "tblCustomers.Name_Desc" : "tblCustomers.Name";
            ViewBag.SortingPSurname = Sorting_Order == "tblCustomers.PSurname" ? "tblCustomers.PSurname_Desc" : "tblCustomers.PSurname";
            ViewBag.SortingCelPhone = Sorting_Order == "tblCustomers.CelPhone" ? "tblCustomers.CelPhone_Desc" : "tblCustomers.CelPhone";
            ViewBag.SortingDate = Sorting_Order == "tblCustomers.CreatedDate" ? "tblCustomers.CreatedDate_Desc" : "tblCustomers.CreatedDate";


            var tblCustomers = CustomersB.GetCustomers(Name, PSurname, "", CelPhone, dtDate, true, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProspectiveCustomers", tblCustomers);
            }
            return View(tblCustomers);
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
        public ActionResult Quotation(int id)
        {
            tblCustomers tblCustomers = CustomersB.GetCustomersByID(id);
            tblPurchasesRequestsQuotation tblPurchasesRequestsQuotation = PurchasesRequestsQuotationB.GetByCustomerID(id);
            tblPurchasesRequestsQuotation.Customer = tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname;
            tblPurchasesRequestsQuotation.CelPhone = tblCustomers.Phone;
            tblDiscounts tblDiscounts = tblPurchasesRequestsQuotation.DiscountID != 0 ? DiscountsB.GetDiscountsByID(tblPurchasesRequestsQuotation.DiscountID) : null;
            tblPurchasesRequestsQuotation.Discount = tblDiscounts != null ? tblDiscounts.Percentage.ToString() + "% " : "";
            tblPurchasesRequestsQuotation.DiscountDescription = tblDiscounts != null ? tblDiscounts.Description : "";
            var vCryptSection = CryptsSectionsB.GetById(tblPurchasesRequestsQuotation.CryptSectionID);
            ViewBag.CryptSection = vCryptSection;
            ViewBag.CreatedBy = tblCustomers.CreatedByName;
            ViewBag.UpdatedBy = tblCustomers.UpdatedByName;
            if (tblCustomers == null)
                return HttpNotFound();
            else
            {
                return View(tblPurchasesRequestsQuotation);
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
            tblCustomers.Prospective = true;
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
        public ActionResult PurchaseRequest(int id = 0, bool Adquire = false)
        {
            ViewBag.CivilStatus = CivilStatusB.GetList();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);
            tblCustomers tblCustomers = new tblCustomers();
            tblPurchasesRequests tblPurchasesRequests = new tblPurchasesRequests();
            tblPurchasesRequestsQuotation tblPurchasesRequestsQuotation = new tblPurchasesRequestsQuotation();

            tblFederalTax tblFederalTax = FederalTaxB.GetLastReccord();
            tblMaintenanceFee tblMaintenanceFee = TempData["Type"].ToString() == "Familiar" ? MaintenanceFeeB.GetLastReccord(1) : MaintenanceFeeB.GetLastReccord(0);

            tblPurchasesRequests.MaintenanceFeeID = tblMaintenanceFee.MaintenanceFeeID;
            tblPurchasesRequests.MaintenanceFee = tblMaintenanceFee.Cost;
            tblPurchasesRequests.FederalTaxID = tblFederalTax.FederalTaxID;
            tblPurchasesRequests.FederalTax = tblFederalTax.Cost;

            if (id != 0)
            {
                //Entra aquí si se adquiere desde vista cotización (Vista Quotation)
                if (Adquire)
                {
                    tblPurchasesRequestsQuotation = PurchasesRequestsQuotationB.GetByID(id);
                    var config = new AutoMapper.MapperConfiguration(cfg => { cfg.CreateMap<tblPurchasesRequestsQuotation, tblPurchasesRequests>(); });
                    IMapper imapper = config.CreateMapper();
                    tblPurchasesRequests = imapper.Map<tblPurchasesRequestsQuotation, tblPurchasesRequests>(tblPurchasesRequestsQuotation);
                    tblCustomers = CustomersB.GetCustomersByID(tblPurchasesRequests.CustomerID);
                    ViewBag.CryptSection = CryptsSectionsB.GetById(tblPurchasesRequests.CryptSectionID);
                }
                else
                {
                    tblCustomers = CustomersB.GetCustomersByID(id);
                    tblPurchasesRequests = PurchasesRequestsB.GetByCustomerID(id);
                }
                if (tblPurchasesRequests == null)
                {
                    tblPurchasesRequests = new tblPurchasesRequests();
                    DateTime ServerDateTimeNow = hm.GetDateTime();
                    tblPurchasesRequests.CreatedDate = ServerDateTimeNow;
                    tblPurchasesRequests.UpdatedDate = ServerDateTimeNow;
                    tblPurchasesRequests.CreatedBy = LoggedUser;
                    tblPurchasesRequests.UpdatedBy = LoggedUser;
                    tblPurchasesRequests.Cancel = false;
                    tblPurchasesRequests.Active = true;
                    tblPurchasesRequests.Date = ServerDateTimeNow;
                }
                tblPurchasesRequests.Customer = tblCustomers.PSurname + " " + tblCustomers.MSurname + " " + tblCustomers.Name;
                tblPurchasesRequests.CustomerID = tblCustomers.CustomerID;
                tblPurchasesRequests.CelPhone = tblCustomers.CelPhone;
                tblPurchasesRequests.Email = tblCustomers.Email;
                tblPurchasesRequests.Prospective = tblCustomers.Prospective;

                tblPurchasesRequests.CelPhone = tblCustomers.CelPhone;
                tblPurchasesRequests.Email = tblCustomers.Email;
                tblPurchasesRequests.Address = tblCustomers.Address;
                tblPurchasesRequests.CityOfBirth = tblCustomers.CityOfBirth;
                tblPurchasesRequests.AddressNumber = tblCustomers.AddressNumber;
                tblPurchasesRequests.AdressInteriorNumber = tblCustomers.AdressInteriorNumber;
                tblPurchasesRequests.Neighborhood = tblCustomers.Neighborhood;
                tblPurchasesRequests.ZipCode = tblCustomers.ZipCode;
                tblPurchasesRequests.RFCCURP = tblCustomers.RFCCURP;
                tblPurchasesRequests.BusinessName = tblCustomers.BusinessName;
                tblPurchasesRequests.DateOfBirth = tblCustomers.DateOfBirth;
                tblPurchasesRequests.CivilStatusID = tblCustomers.CivilStatusID;
                tblPurchasesRequests.Occupation = tblCustomers.Occupation;
                tblPurchasesRequests.Company = tblCustomers.Company;
                tblPurchasesRequests.PhoneCompany = tblCustomers.PhoneCompany;
                tblPurchasesRequests.AddressCompany = tblCustomers.AddressCompany;
                tblPurchasesRequests.ExtPhoneCompany = tblCustomers.ExtPhoneCompany;
                tblPurchasesRequests.TownAddressCompanyID = tblCustomers.TownAddressCompanyID;
                tblPurchasesRequests.StateAddressCompanyID = tblCustomers != null && tblCustomers.TownAddressCompanyID != null ? TownsB.GetTownsByID(Convert.ToInt32(tblCustomers.TownAddressCompanyID)).StateID : 0;
                tblPurchasesRequests.DelegationAddressCompany = tblCustomers.DelegationAddressCompany;
                tblPurchasesRequests.Income = tblCustomers.Income;
                tblPurchasesRequests.ReferenceCustomer1 = tblPurchasesRequests != null ? tblPurchasesRequests.ReferenceCustomer1 : tblCustomers.ReferenceCustomer1;
                tblPurchasesRequests.ReferenceCustomerPhone1 = tblPurchasesRequests != null ? tblPurchasesRequests.ReferenceCustomerPhone1 : tblCustomers.ReferenceCustomerPhone1;
                tblPurchasesRequests.ReferenceCustomer2 = tblPurchasesRequests != null ? tblPurchasesRequests.ReferenceCustomer2 : tblCustomers.ReferenceCustomer2;
                tblPurchasesRequests.ReferenceCustomerPhone2 = tblPurchasesRequests != null ? tblPurchasesRequests.ReferenceCustomerPhone2 : tblCustomers.ReferenceCustomerPhone2;
                tblPurchasesRequests.Delegation = tblCustomers.Delegation;
                tblPurchasesRequests.StateID = tblCustomers != null && tblCustomers.TownID != null ? TownsB.GetTownsByID(Convert.ToInt32(tblCustomers.TownID)).StateID : 0;
                tblPurchasesRequests.TownID = tblCustomers.TownID;
                tblPurchasesRequests.UserID = tblCustomers.UserID;
                tblPurchasesRequests.CheckMaintenanceFee = tblPurchasesRequests != null ? tblPurchasesRequests.CheckMaintenanceFee : false;
                tblPurchasesRequests.MaintenanceFeeID = tblMaintenanceFee.MaintenanceFeeID;
                tblPurchasesRequests.MaintenanceFee = tblMaintenanceFee.Cost;
                tblPurchasesRequests.CheckFederalTax = tblPurchasesRequests != null ? tblPurchasesRequests.CheckFederalTax : false;
                tblPurchasesRequests.FederalTaxID = tblFederalTax.FederalTaxID;
                tblPurchasesRequests.FederalTax = tblFederalTax.Cost;
                tblPurchasesRequests.DiscountID = Convert.ToInt32(TempData["DiscountID"]);
                tblDiscounts tblDiscounts = tblPurchasesRequests.DiscountID != 0 ? DiscountsB.GetDiscountsByID(tblPurchasesRequests.DiscountID) : null;
                tblPurchasesRequests.Discount = tblDiscounts != null ? tblDiscounts.Percentage.ToString() + "% " : "";
                tblPurchasesRequests.DiscountDescription = tblDiscounts != null ? tblDiscounts.Description : "";

                tblPurchasesRequests.CheckPaymentAmount = tblPurchasesRequests != null ? tblPurchasesRequests.CheckPaymentAmount : null;
                tblPurchasesRequests.CheckPaymentNo = tblPurchasesRequests != null ? tblPurchasesRequests.CheckPaymentNo : "";
                tblPurchasesRequests.CheckPaymentAccount = tblPurchasesRequests != null ? tblPurchasesRequests.CheckPaymentAccount : "";
                tblPurchasesRequests.CheckPaymentBank = tblPurchasesRequests != null ? tblPurchasesRequests.CheckPaymentBank : "";

                tblPurchasesRequests.CreditCardPaymentAmount = tblPurchasesRequests != null ? tblPurchasesRequests.CreditCardPaymentAmount : null;
                tblPurchasesRequests.CreditCardPaymentNo = tblPurchasesRequests != null ? tblPurchasesRequests.CreditCardPaymentNo : "";
                tblPurchasesRequests.CreditCardPaymentAccount = tblPurchasesRequests != null ? tblPurchasesRequests.CreditCardPaymentAccount : "";
                tblPurchasesRequests.CreditCardPaymentBank = tblPurchasesRequests != null ? tblPurchasesRequests.CreditCardPaymentBank : "";

                tblPurchasesRequests.CashPaymentAmountOrTransfer = tblPurchasesRequests != null ? tblPurchasesRequests.CashPaymentAmountOrTransfer : null;

                tblPurchasesRequests.CashDeposit = tblPurchasesRequests != null ? tblPurchasesRequests.CashDeposit : null;

                tblPurchasesRequests.CashPaymentAmount = tblPurchasesRequests != null ? tblPurchasesRequests.CashPaymentAmount : null;

                ViewBag.States = StatesB.GetList();
                ViewBag.Towns = TownsB.GetListByStateID(tblCustomers.StateID);
                ViewBag.CommissionAgents = CommissionAgentsB.GetList();

                ViewBag.StateAddressCompany = StatesB.GetList();
                if (tblPurchasesRequests.StateAddressCompanyID != null)
                    ViewBag.TownAddressCompany = TownsB.GetListByStateID((int)tblPurchasesRequests.StateAddressCompanyID);
                else
                    ViewBag.TownAddressCompany = new List<tblTowns>();

                ViewBag.BeneficiaryCustomers = BeneficiaryCustomersB.GetListByPurchasesRequestID(tblPurchasesRequests.PurchasesRequestID);
            }
            else
            {
                ViewBag.States = StatesB.GetList();
                ViewBag.Towns = new List<tblTowns>();

                ViewBag.StateAddressCompany = StatesB.GetList();
                ViewBag.TownAddressCompany = new List<tblTowns>();
                ViewBag.CommissionAgents = CommissionAgentsB.GetList();

                DateTime ServerDateTimeNow = hm.GetDateTime();
                tblPurchasesRequests.CreatedDate = ServerDateTimeNow;
                tblPurchasesRequests.UpdatedDate = ServerDateTimeNow;
                tblPurchasesRequests.CreatedBy = LoggedUser;
                tblPurchasesRequests.UpdatedBy = LoggedUser;
                tblPurchasesRequests.Cancel = false;
                tblPurchasesRequests.Active = true;
                tblPurchasesRequests.Date = ServerDateTimeNow;

                tblPurchasesRequests.DiscountID = Convert.ToInt32(TempData["DiscountID"]);
                tblDiscounts tblDiscounts = tblPurchasesRequests.DiscountID != 0 ? DiscountsB.GetDiscountsByID(tblPurchasesRequests.DiscountID) : null;
                tblPurchasesRequests.Discount = tblDiscounts != null ? tblDiscounts.Percentage.ToString() + "% " : "";
                tblPurchasesRequests.DiscountDescription = tblDiscounts != null ? tblDiscounts.Description : "";
            }
            bool bIsCryptSectionId = int.TryParse(TempData["CryptSectionId"]?.ToString(), out int iCryptSectionId);

            if (!bIsCryptSectionId && !Adquire)
                return HttpNotFound();

            if (!Adquire)
            {
                TempData["Type"] = TempData["Type"];
                TempData["CryptSectionId"] = TempData["CryptSectionId"];
                TempData["CryptPosition"] = TempData["CryptPosition"];
                TempData["Price"] = TempData["Price"];
                TempData["OriginalPrice"] = TempData["OriginalPrice"];
                TempData["Level"] = TempData["Level"];
                TempData["PaymentMethod"] = TempData["PaymentMethod"];
                TempData["Enganche"] = TempData["Enganche"];
                TempData["Mensualidades"] = TempData["Mensualidades"];

                Enum.TryParse(TempData["PaymentMethod"]?.ToString(), out PaymentMethods paymentMethod);
                Enum.TryParse(TempData["Type"]?.ToString(), out CryptTypes eCryptType);
                var vCryptPosition = TempData["CryptPosition"].ToString().Split('-');
                string sCryptKey = vCryptPosition[0];
                int? sPosition = eCryptType == CryptTypes.Individual ? int.Parse(vCryptPosition[1]) : (int?)null;
                tblPurchasesRequests.PaymentMethod = paymentMethod;
                tblPurchasesRequests.CryptType = eCryptType;
                tblPurchasesRequests.Quantity = eCryptType == CryptTypes.Familiar ? 4 : 1;
                tblPurchasesRequests.CryptKey = sCryptKey;
                tblPurchasesRequests.CryptPosition = sPosition;
                tblPurchasesRequests.CryptSectionID = iCryptSectionId;
                tblPurchasesRequests.Enganche = Math.Round(decimal.Parse(paymentMethod == PaymentMethods.Contado ? TempData["Price"]?.ToString() : TempData["Enganche"]?.ToString()), 2);
                tblPurchasesRequests.Mensualidades = Math.Round(decimal.Parse(paymentMethod == PaymentMethods.Contado ? "0" : TempData["Mensualidades"]?.ToString()), 2);
                tblPurchasesRequests.Level = int.Parse(TempData["Level"].ToString());

                var vCryptPrice = tblPurchasesRequests.Enganche;
                int iMeses = paymentMethod == PaymentMethods.Meses12 ? 11
                    : paymentMethod == PaymentMethods.Meses18 ? 17
                    : paymentMethod == PaymentMethods.Meses24 ? 23
                    : paymentMethod == PaymentMethods.Meses36 ? 35
                    : paymentMethod == PaymentMethods.Meses48 ? 47
                    : paymentMethod == PaymentMethods.UsoInmediato ? 11
                    : 0;

                tblPurchasesRequests.FirstPaymentDate = paymentMethod != PaymentMethods.Contado ? tblPurchasesRequests.Date.AddMonths(1) : (DateTime?)null;
                tblPurchasesRequests.LastPaymentDate = paymentMethod != PaymentMethods.Contado ? tblPurchasesRequests.Date.AddMonths(iMeses) : (DateTime?)null;
                vCryptPrice = vCryptPrice + (tblPurchasesRequests.Mensualidades * iMeses);
                tblPurchasesRequests.CryptPrice = Math.Ceiling(vCryptPrice);
                var vCryptSection = CryptsSectionsB.GetById(iCryptSectionId);
                ViewBag.CryptSection = vCryptSection;
                tblPurchasesRequests.CryptPrice = paymentMethod == PaymentMethods.Contado ? decimal.Parse(TempData["Price"].ToString()) : tblPurchasesRequests.CryptPrice;
                tblPurchasesRequests.OriginalPrice = Convert.ToDecimal(TempData["OriginalPrice"]);


                string CryptKeyNumberAux = tblPurchasesRequests.CryptKey.Substring(1, tblPurchasesRequests.CryptKey.Length - 1);
                string CryptKeyNumber = "";
                if (Convert.ToInt32(CryptKeyNumberAux) > 9 && Convert.ToInt32(CryptKeyNumberAux) < 100)
                {
                    CryptKeyNumber = "0" + CryptKeyNumberAux;
                }
                else if (Convert.ToInt32(CryptKeyNumberAux) < 9)
                {
                    CryptKeyNumber = "00" + CryptKeyNumberAux;
                }
                tblPurchasesRequests.Nicho = vCryptSection.Code + tblPurchasesRequests.CryptKey[0] + CryptKeyNumber;
                tblPurchasesRequests.Nicho += tblPurchasesRequests.CryptType == CryptTypes.Individual ? "-" + tblPurchasesRequests.CryptPosition : "";
            }
            return View(tblPurchasesRequests);
        }
        [HttpPost]
        public ActionResult PurchaseRequest(tblPurchasesRequests model, bool Quotation, string[] BeneficiaryName, string[] BeneficiarySurnames, DateTime[] BeneficiaryBirthdate, string[] BeneficiaryRelationship, string[] BeneficiaryCelPhone, int[] BeneficiaryCustomerID, string windowid, HttpPostedFileBase Ticket, HttpPostedFileBase TicketCashDeposit)
        {
            try
            {
                Response Response = new Response();

                tblPurchasesRequestsQuotation tblPurchasesRequestsQuotation = null;
                if (Quotation)
                {
                    tblPurchasesRequestsQuotation = new tblPurchasesRequestsQuotation();
                }
                tblCustomers tblCustomers = new tblCustomers();
                DateTime ServerDateTimeNow = hm.GetDateTime();
                int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

                if (model.CustomerID != 0)
                {
                    tblCustomers = CustomersB.GetCustomersByID(model.CustomerID);
                }
                else
                {
                    tblCustomers.Name = model.Name;
                    tblCustomers.PSurname = model.PSurname;
                    tblCustomers.MSurname = model.MSurname;
                    tblCustomers.CreatedDate = ServerDateTimeNow;
                    tblCustomers.UpdatedDate = ServerDateTimeNow;
                    tblCustomers.CreatedBy = LoggedUser;
                    tblCustomers.UpdatedBy = LoggedUser;
                }
                tblCustomers.TownID = model.TownID;
                tblCustomers.Delegation = model.Delegation;
                tblCustomers.Email = model.Email;
                tblCustomers.CelPhone = model.CelPhone;
                tblCustomers.Address = model.Address;
                tblCustomers.AddressNumber = model.AddressNumber;
                tblCustomers.AdressInteriorNumber = model.AdressInteriorNumber;
                tblCustomers.Neighborhood = model.Neighborhood;
                tblCustomers.CityOfBirth = model.CityOfBirth;
                tblCustomers.ZipCode = model.ZipCode;
                tblCustomers.RFCCURP = model.RFCCURP;
                tblCustomers.BusinessName = model.BusinessName;
                tblCustomers.DateOfBirth = model.DateOfBirth;
                tblCustomers.CivilStatusID = model.CivilStatusID;
                tblCustomers.Occupation = model.Occupation;
                tblCustomers.Company = model.Company;
                tblCustomers.PhoneCompany = model.PhoneCompany;
                tblCustomers.AddressCompany = model.AddressCompany;
                tblCustomers.ExtPhoneCompany = model.ExtPhoneCompany;
                tblCustomers.TownAddressCompanyID = model.TownAddressCompanyID;
                tblCustomers.DelegationAddressCompany = model.DelegationAddressCompany;
                tblCustomers.Income = model.Income;
                tblCustomers.Name = model.Name != null ? model.Name : tblCustomers.Name;
                tblCustomers.PSurname = model.PSurname != null ? model.PSurname : tblCustomers.PSurname;
                tblCustomers.MSurname = model.MSurname != null ? model.MSurname : tblCustomers.MSurname;
                tblCustomers.Prospective = Quotation && model.Prospective ? true : false;
                tblCustomers.Active = true;

                model.Customer = tblCustomers.PSurname + " " + tblCustomers.MSurname + " " + tblCustomers.Name;

                if (model.CustomerID != 0)
                {
                    Response = CustomersB.Update(tblCustomers);

                    if (Response.Result != Result.OK)
                    {
                        string error = "Update Customers. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }
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

                    Response = CustomersB.Create(tblCustomers);

                    if (Response.Result != Result.OK)
                    {
                        string error = "Create Customers. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }

                    model.CustomerID = tblCustomers.CustomerID;
                }

                //CREATE CONTRACT
                if (model.ContractID == 0)
                {
                    tblContracts tblContracts = new tblContracts();
                    tblContracts.Active = true;
                    tblContracts.CreatedBy = LoggedUser;
                    tblContracts.CreatedDate = ServerDateTimeNow;
                    tblContracts.UpdatedBy = LoggedUser;
                    tblContracts.UpdatedDate = ServerDateTimeNow;

                    int i = 0;
                    int ID = 0;
                    if (model.CryptType == CryptTypes.Individual)
                    {
                        i = 1;
                        ID = 175;
                        tblContracts.ContractType = 1;
                    }
                    else
                    {
                        i = 2;
                        ID = 1142;
                        tblContracts.ContractType = 2;
                    }

                    int contractID = ContractsB.GetContractID(i, ID);

                    if (contractID == 0 && model.CryptType == CryptTypes.Individual)
                    {
                        tblContracts.ContractID = ID;
                    }
                    else if (contractID != 0 && model.CryptType == CryptTypes.Individual)
                    {
                        tblContracts.ContractID = contractID + 1;
                    }

                    if (contractID == 0 && model.CryptType == CryptTypes.Familiar)
                    {
                        tblContracts.ContractID = ID;
                    }
                    else if (contractID != 0 && model.CryptType == CryptTypes.Familiar)
                    {
                        tblContracts.ContractID = contractID + 1;
                    }


                    Response = ContractsB.Create(tblContracts);

                    if (Response.Result != Result.OK)
                    {
                        string error = "Create Contracts. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }

                    model.ContractID = tblContracts.ContractID;
                }

                model.UpdatedDate = ServerDateTimeNow;
                model.UpdatedBy = LoggedUser;
                string File = Ticket != null ? HelperAzure.SaveFile(Ticket) : "";
                string File2 = TicketCashDeposit != null ? HelperAzure.SaveFile(TicketCashDeposit) : "";
                model.Ticket = File != "" ? File : "";
                model.TicketCashDeposit = File2 != "" ? File2 : "";

                model.Currency = true;

                tblPurchasesRequests tblPurchasesRequests = PurchasesRequestsB.GetByID(model.PurchasesRequestID);
                if (tblPurchasesRequests == null && !Quotation)
                {
                    int PurchaseRequestID = PurchasesRequestsB.GetPurchaseRequestID();
                    if (PurchaseRequestID == 0)
                    {
                        model.PurchasesRequestID = 5733;
                    }
                    else
                    {
                        model.PurchasesRequestID = PurchaseRequestID + 1;
                    }

                    Response = PurchasesRequestsB.Create(model);

                    if (Response.Result != Result.OK)
                    {
                        string error = "Create PurchaseRequest. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }

                    tblPurchasesRequests = model;
                }
                else if (!Quotation)
                {
                    Response = PurchasesRequestsB.Update(model);
                    if (Response.Result != Result.OK)
                    {
                        string error = "Update PurchaseRequest. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }
                }
                else
                {
                    var config = new AutoMapper.MapperConfiguration(cfg => { cfg.CreateMap<tblPurchasesRequests, tblPurchasesRequestsQuotation>(); });
                    IMapper imapper = config.CreateMapper();
                    tblPurchasesRequestsQuotation = imapper.Map<tblPurchasesRequests, tblPurchasesRequestsQuotation>(model);

                    Response = PurchasesRequestsQuotationB.Create(tblPurchasesRequestsQuotation);
                    if (Response.Result != Result.OK)
                    {
                        string error = "Create PurchaseRequestsQuotation. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }
                }

                var ListAntBeneficiary = new List<tblBeneficiaryCustomers>();
                if (Quotation)
                    ListAntBeneficiary = BeneficiaryCustomersB.GetListByPurchasesRequestID(tblPurchasesRequestsQuotation.PurchasesRequestQuotationID);
                else
                    ListAntBeneficiary = BeneficiaryCustomersB.GetListByPurchasesRequestID(tblPurchasesRequests.PurchasesRequestID);

                List<tblBeneficiaryCustomers> lstBeneficiaryCustomers = new List<tblBeneficiaryCustomers>();

                for (int i = 0; i < BeneficiaryCustomerID.Length; i++)
                {
                    if (BeneficiaryCustomerID[i] != 0)
                    {
                        var BeneficiaryCustomer = BeneficiaryCustomersB.GetByBeneficiaryCustomerID(BeneficiaryCustomerID[i]);
                        BeneficiaryCustomer.Name = BeneficiaryName[i];
                        BeneficiaryCustomer.Surnames = BeneficiarySurnames[i];
                        BeneficiaryCustomer.Birthdate = BeneficiaryBirthdate[i];
                        BeneficiaryCustomer.Relationship = BeneficiaryRelationship[i];
                        BeneficiaryCustomer.CelPhone = BeneficiaryCelPhone[i];
                        BeneficiaryCustomer.CustomerID = tblCustomers.CustomerID;
                        BeneficiaryCustomer.UpdatedDate = ServerDateTimeNow;
                        BeneficiaryCustomer.UpdatedBy = LoggedUser;
                        BeneficiaryCustomer.PurchasesRequestID = tblPurchasesRequestsQuotation == null ? tblPurchasesRequests.PurchasesRequestID : tblPurchasesRequestsQuotation.PurchasesRequestQuotationID;

                        Response = BeneficiaryCustomersB.Update(BeneficiaryCustomer);
                        if (Response.Result != Result.OK)
                        {
                            string error = "Update BeneficiaryCustomer. " + Response.error;
                            return RedirectToAction("Index", "Home", new { error = error });
                        }

                        lstBeneficiaryCustomers.Add(BeneficiaryCustomer);
                    }
                    else if (BeneficiarySurnames[0] != "")
                    {
                        tblBeneficiaryCustomers tblBeneficiaryCustomers = new tblBeneficiaryCustomers();
                        tblBeneficiaryCustomers.Name = BeneficiaryName[i];
                        tblBeneficiaryCustomers.Surnames = BeneficiarySurnames[i];
                        tblBeneficiaryCustomers.Birthdate = BeneficiaryBirthdate[i];
                        tblBeneficiaryCustomers.Relationship = BeneficiaryRelationship[i];
                        tblBeneficiaryCustomers.CelPhone = BeneficiaryCelPhone[i];
                        tblBeneficiaryCustomers.CustomerID = tblCustomers.CustomerID;
                        tblBeneficiaryCustomers.Active = true;
                        tblBeneficiaryCustomers.CreatedDate = ServerDateTimeNow;
                        tblBeneficiaryCustomers.UpdatedDate = ServerDateTimeNow;
                        tblBeneficiaryCustomers.CreatedBy = LoggedUser;
                        tblBeneficiaryCustomers.UpdatedBy = LoggedUser;
                        tblBeneficiaryCustomers.PurchasesRequestID = tblPurchasesRequestsQuotation == null ? tblPurchasesRequests.PurchasesRequestID : tblPurchasesRequestsQuotation.PurchasesRequestQuotationID;

                        Response = BeneficiaryCustomersB.Create(tblBeneficiaryCustomers);
                        if (Response.Result != Result.OK)
                        {
                            string error = "Create BeneficiaryCustomer. " + Response.error;
                            return RedirectToAction("Index", "Home", new { error = error });
                        }

                        lstBeneficiaryCustomers.Add(tblBeneficiaryCustomers);
                    }
                }

                var arrBeneficiaryCustomerID = BeneficiaryCustomerID.Where(r => r != 0).ToList();

                for (int i = 0; i < ListAntBeneficiary.Count; i++)
                {
                    if (!arrBeneficiaryCustomerID.Where(r => r == ListAntBeneficiary[i].BeneficiaryCustomerID).Any())
                    {
                        ListAntBeneficiary[i].Active = false;

                        Response = BeneficiaryCustomersB.Update(ListAntBeneficiary[i]);
                        if (Response.Result != Result.OK)
                        {
                            string error = "Update BeneficiaryCustomer. " + Response.error;
                            return RedirectToAction("Index", "Home", new { error = error });
                        }
                    }
                }

                ViewBag.States = StatesB.GetList();
                ViewBag.Towns = TownsB.GetListByStateID(tblCustomers.StateID);
                ViewBag.CommissionAgents = CommissionAgentsB.GetList();
                if (Quotation)
                    ViewBag.BeneficiaryCustomers = BeneficiaryCustomersB.GetListByPurchasesRequestID(tblPurchasesRequestsQuotation.PurchasesRequestQuotationID);
                else
                    ViewBag.BeneficiaryCustomers = BeneficiaryCustomersB.GetListByPurchasesRequestID(tblPurchasesRequests.PurchasesRequestID);

                tblUsers tblUsers = UsersB.GetUserByID(model.UserID);
                if (tblUsers == null)
                    tblUsers = UsersB.GetUserByEmail(model.Email);
                if (tblUsers == null)
                {
                    tblUsers = new tblUsers();
                    tblUsers.UserName = tblCustomers.Email;
                    tblUsers.Password = "123123";
                    tblUsers.ConfirmPassword = "123123";

                    tblUsers.Password = PasswordSecurity.Encrypt(tblUsers.Password);
                    tblUsers.ConfirmPassword = PasswordSecurity.Encrypt(tblUsers.ConfirmPassword);
                    tblUsers.Name = tblCustomers.Name;
                    tblUsers.PSurname = tblCustomers.PSurname;
                    tblUsers.MSurname = tblCustomers.MSurname;
                    tblUsers.Active = true;
                    tblUsers.CreatedDate = ServerDateTimeNow;
                    tblUsers.UpdatedDate = ServerDateTimeNow;
                    tblUsers.CreatedBy = LoggedUser;
                    tblUsers.UpdatedBy = LoggedUser;
                    tblUsers.RolID = 7;

                    Response = UsersB.Create(tblUsers);
                    if (Response.Result != Result.OK)
                    {
                        string error = "Create Users. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }

                    tblCustomers.UserID = tblUsers.UserID;

                    Response = CustomersB.Update(tblCustomers);
                    if (Response.Result != Result.OK)
                    {
                        string error = "Update Users. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }
                }
                else
                {
                    tblUsers.UserName = model.Email;
                    tblUsers.Name = tblCustomers.Name;
                    tblUsers.PSurname = tblCustomers.PSurname;
                    tblUsers.MSurname = tblCustomers.MSurname;
                }

                ViewBag.Message = "Se guardaron los cambios";
                string emailSubject = "Parroquia";
                var vEmailData = new AxuEmail { ClientName = model.Customer, Email = model.Email, Password = "123123", ServiceName = "Bienvenido", Date = DateTime.Now, Time = DateTime.Now.ToShortTimeString(), TimeEnd = DateTime.Now.ToShortTimeString() };
                string sMessageBody = ConvertViewToString("EmailTemplates/Welcome", vEmailData);
                HtmlHelperExtensions.SendEmail(emailSubject, sMessageBody, new List<string> { tblCustomers.Email });

                int QtyPayment = 0;
                switch (model.PaymentMethod)
                {
                    case PaymentMethods.Contado:
                        QtyPayment = 1;
                        break;
                    case PaymentMethods.Meses12:
                        QtyPayment = 12;
                        break;
                    case PaymentMethods.Meses24:
                        QtyPayment = 24;
                        break;
                    case PaymentMethods.UsoInmediato:
                        QtyPayment = 12;
                        break;
                    case PaymentMethods.Meses18:
                        QtyPayment = 18;
                        break;
                    case PaymentMethods.Meses36:
                        QtyPayment = 36;
                        break;
                    case PaymentMethods.Meses48:
                        QtyPayment = 48;
                        break;
                }
                DateTime PaymentDate = ServerDateTimeNow;
                decimal CryptPrice = model.CryptPrice;
                for (int i = 0; i < QtyPayment; i++)
                {
                    tblPurchaseRequestPayments tblPurchaseRequestPayments = new tblPurchaseRequestPayments();
                    decimal Mensualidades = ((model.CryptPrice - model.Enganche) / QtyPayment) * 100 / 100;
                    string[] arrMensualidades = Mensualidades.ToString().Split('.');
                    decimal Mensualidad = i == 0 ? 0 : Convert.ToDecimal(arrMensualidades[1]) > 0 ? Convert.ToDecimal(arrMensualidades[0]) + 1 : Mensualidades;
                    if (i > 0)
                    {
                        if (i < (QtyPayment - 1))
                            CryptPrice -= Mensualidad;
                        else
                            Mensualidad = CryptPrice;
                    }
                    else
                    {
                        CryptPrice -= model.Enganche;
                    }

                    switch (model.PaymentMethod)
                    {
                        case PaymentMethods.Contado:
                            tblPurchaseRequestPayments.Amount = i == 0 ? model.Enganche : Mensualidad;
                            break;
                        case PaymentMethods.Meses12:
                            tblPurchaseRequestPayments.Amount = i == 0 ? model.Enganche : Mensualidad;
                            break;
                        case PaymentMethods.Meses24:
                            tblPurchaseRequestPayments.Amount = i == 0 ? model.Enganche : Mensualidad;
                            break;
                        case PaymentMethods.UsoInmediato:
                            tblPurchaseRequestPayments.Amount = i == 0 ? model.Enganche : Mensualidad;
                            break;
                        case PaymentMethods.Meses18:
                            tblPurchaseRequestPayments.Amount = i == 0 ? model.Enganche : Mensualidad;
                            break;
                        case PaymentMethods.Meses36:
                            tblPurchaseRequestPayments.Amount = i == 0 ? model.Enganche : Mensualidad;
                            break;
                        case PaymentMethods.Meses48:
                            tblPurchaseRequestPayments.Amount = i == 0 ? model.Enganche : Mensualidad;
                            break;
                    }
                    tblPurchaseRequestPayments.Estatus = i == 0 ? PaymentEstatus.Paid : PaymentEstatus.Pending;
                    tblPurchaseRequestPayments.NumPayment = (i + 1).ToString();
                    tblPurchaseRequestPayments.PaymentDate = PaymentDate.AddMonths(i);
                    tblPurchaseRequestPayments.PurchasesRequestID = model.PurchasesRequestID;
                    tblPurchaseRequestPayments.CreatedDate = ServerDateTimeNow;
                    tblPurchaseRequestPayments.UpdatedDate = ServerDateTimeNow;
                    tblPurchaseRequestPayments.CreatedBy = LoggedUser;
                    tblPurchaseRequestPayments.UpdatedBy = LoggedUser;
                    tblPurchaseRequestPayments.Active = true;

                    Response = PurchaseRequestPaymentsB.Create(tblPurchaseRequestPayments);
                    if (Response.Result != Result.OK)
                    {
                        string error = "Create PurchaseRequestPayment. " + Response.error;
                        return RedirectToAction("Index", "Home", new { error = error });
                    }
                }

                ReceiptsB ReceiptsB = new ReceiptsB();
                tblReceipts tblReceipts = new tblReceipts();
                tblReceipts.PurchasesRequestID = model.PurchasesRequestID;
                tblReceipts.CreatedDate = ServerDateTimeNow;
                tblReceipts.UpdatedDate = ServerDateTimeNow;
                tblReceipts.CreatedBy = LoggedUser;
                tblReceipts.UpdatedBy = LoggedUser;
                tblReceipts.Active = true;

                Response = ReceiptsB.Create(tblReceipts);
                if (Response.Result != Result.OK)
                {
                    string error = "Create Receipts. " + Response.error;
                    return RedirectToAction("Index", "Home", new { error = error });
                }

                LastID = tblPurchasesRequests != null ? tblPurchasesRequests.PurchasesRequestID : 0;
                return PDF(model, lstBeneficiaryCustomers, windowid);
            }
            catch (Exception ex)
            {
                string error = ex.Message + " Inner Exception: " + ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home", new { error = error });
            }

        }
        static public int LastID = 0;

        #region PDFs designers
        public ActionResult Receipt(int id = 0)
        {
            LastID = id != 0 ? id : LastID;
            if (LastID > 0)
            {
                tblPurchasesRequests tblPurchasesRequests = PurchasesRequestsB.GetByID(LastID);
                tblCustomers tblCustomers = CustomersB.GetCustomersByID(tblPurchasesRequests.CustomerID);
                string Customer = tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname;
                //Selects lists

                var doc = new Document(iTextSharp.text.PageSize.LETTER);
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);

                    doc.Open();
                    doc.NewPage();

                    PdfPTable MainTable = new PdfPTable(3);
                    MainTable.TotalWidth = 500f;
                    MainTable.LockedWidth = true;
                    MainTable.HorizontalAlignment = 0;

                    //CELDAS EN BLANCO
                    PdfPCell WhiteCell = new PdfPCell(new Paragraph(""));
                    WhiteCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    //SALTO DE LINEA
                    PdfPCell CellRow = new PdfPCell(new Paragraph("\n"));
                    CellRow.Colspan = 3;
                    CellRow.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    //IMAGEN
                    PdfPCell ImageTableCell = new PdfPCell();
                    PdfPTable ImageTable = new PdfPTable(3);

                    string imageURL = Server.MapPath("~/Content/Images/LogoParroquia.png");
                    Image img = Image.GetInstance(imageURL);
                    img.ScaleAbsolute(90f, 105f);

                    PdfPCell ImageCell = new PdfPCell(img);
                    ImageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ImageCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    //SUBTITULOS
                    PdfPCell TitleCell = new PdfPCell();
                    PdfPTable TitleTable = new PdfPTable(1);

                    PdfPCell MainTitleHeader = new PdfPCell(new Phrase("Parroquia Iglesia Guadalupe del Rio en Tijuana A.R", new Font(Font.FontFamily.UNDEFINED, 14f, Font.NORMAL, new BaseColor(0, 102, 204))));
                    MainTitleHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTitleHeader.PaddingTop = 35f;
                    MainTitleHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    PdfPCell MainSubTitleHeader = new PdfPCell(new Phrase("Av. Centenario 10150, Zona Urbana Rio Tijuana. 22010", new Font(Font.FontFamily.UNDEFINED, 9f, Font.NORMAL, new BaseColor(0, 102, 204))));
                    MainSubTitleHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainSubTitleHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    PdfPCell MainSubTitleRFCHeader = new PdfPCell(new Phrase("RFC: PG930423EY5", new Font(Font.FontFamily.UNDEFINED, 9f, Font.NORMAL, new BaseColor(0, 102, 204))));
                    MainSubTitleRFCHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainSubTitleRFCHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    TitleTable.AddCell(MainTitleHeader);
                    TitleTable.AddCell(MainSubTitleHeader);
                    TitleTable.AddCell(MainSubTitleRFCHeader);

                    TitleTable.TotalWidth = 400f;
                    TitleTable.LockedWidth = true;
                    TitleTable.HorizontalAlignment = 0;

                    TitleCell.Colspan = 2;
                    TitleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    TitleCell.AddElement(TitleTable);

                    ImageTable.AddCell(ImageCell);
                    ImageTable.AddCell(TitleCell);

                    ImageTable.TotalWidth = 250f;
                    ImageTable.LockedWidth = true;
                    ImageTable.HorizontalAlignment = 0;

                    ImageTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ImageTableCell.Colspan = 3;
                    ImageTableCell.AddElement(ImageTable);

                    //PAGINA 1
                    MainTable.AddCell(ImageTableCell);
                    //primer row
                    PdfPCell ApplicantHeader = new PdfPCell(new Phrase("No. de Cliente", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase("Nombre", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase("No. Contrato", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //segundo row
                    ApplicantHeader = new PdfPCell(new Phrase(tblCustomers.CustomerID.ToString(), new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname, new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    string nom = "";
                    if (tblPurchasesRequests.CryptType == Church.Data.Enums.CryptTypes.Familiar)
                    {
                        nom = "-A/F";
                    }
                    else
                    {
                        nom = "-A/C";
                    }
                    int zeros = 6 - tblPurchasesRequests.ContractID.ToString().Length;
                    string ceros = "";
                    for (int i = 0; i < zeros; i++)
                    {
                        ceros += "0";
                    }
                    ApplicantHeader = new PdfPCell(new Phrase("C-JC-" + ceros + tblPurchasesRequests.ContractID.ToString() + nom, new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase("\n\n", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //tercer row
                    ApplicantHeader = new PdfPCell(new Phrase("Clave de la cripta", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase("Costo", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase("Financiamiento", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //cuarto row
                    ApplicantHeader = new PdfPCell(new Phrase(tblPurchasesRequests.Nicho, new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(tblPurchasesRequests.CryptPrice.ToString("C2") + " MXN.", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(tblPurchasesRequests.PaymentMethodName.ToUpper(), new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase("\n\n", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //quinto row
                    ApplicantHeader = new PdfPCell(new Phrase("Enganche", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.PaddingTop = 10;
                    ApplicantHeader.PaddingBottom = 10;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.PaddingTop = 10;
                    ApplicantHeader.PaddingBottom = 10;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase("Forma de pago", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.PaddingTop = 10;
                    ApplicantHeader.PaddingBottom = 10;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //sexto row
                    ApplicantHeader = new PdfPCell(new Phrase(tblPurchasesRequests.Enganche.ToString("C2") + " MXN.", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    string FormaPago = "";
                    if (tblPurchasesRequests.TypePay == 1)
                        FormaPago = "Cheque";
                    else if (tblPurchasesRequests.TypePay == 2)
                        FormaPago = "Tarjeta de credito/debito";
                    else if (tblPurchasesRequests.TypePay == 3)
                        FormaPago = "Transferencia";
                    else if (tblPurchasesRequests.TypePay == 4)
                        FormaPago = "Efectivo";
                    else if (tblPurchasesRequests.TypePay == 5)
                        FormaPago = "Deposito en efectivo";

                    ApplicantHeader = new PdfPCell(new Phrase(FormaPago, new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase("\n\n", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //septimo row
                    ApplicantHeader = new PdfPCell(new Phrase("Cuota de mantenimiento", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.MinimumHeight = 20;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.MinimumHeight = 20;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase("Depósito de cenizas", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.MinimumHeight = 20;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //octavo row
                    ApplicantHeader = new PdfPCell(new Phrase(tblPurchasesRequests.MaintenanceFee.ToString("C2") + " MXN.", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(tblPurchasesRequests.FederalTax.ToString("C2") + " MXN.", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase("\n\n", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //noveno row
                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.MinimumHeight = 20;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.MinimumHeight = 20;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase("Folio no.", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(180, 197, 231);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.VerticalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.MinimumHeight = 20;
                    MainTable.AddCell(ApplicantHeader);
                    //espacio entre row
                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);
                    //decimo
                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);

                    ApplicantHeader = new PdfPCell(new Phrase(tblPurchasesRequests.PurchasesRequestID.ToString(), new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.BackgroundColor = new BaseColor(255, 217, 102);
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    MainTable.AddCell(ApplicantHeader);



                    ApplicantHeader = new PdfPCell(new Phrase("\n", new Font(Font.FontFamily.UNDEFINED, 12f, Font.BOLD)));
                    ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    ApplicantHeader.Colspan = 5;
                    MainTable.AddCell(ApplicantHeader);


                    ApplicantHeader = new PdfPCell(new Phrase("Este es un documeno provisional, comprobante de su compra, mientras se elabora su factura y contrato.", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLD)));
                    ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                    ApplicantHeader.Colspan = 5;
                    ApplicantHeader.MinimumHeight = 17;
                    MainTable.AddCell(ApplicantHeader);

                    //SALTO DE LINEA
                    MainTable.AddCell(CellRow);

                    doc.Add(MainTable);

                    writer.CloseStream = false;
                    doc.Close();

                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=RECIBO ADQUISICION DE CRIPTA.pdf");
                    Response.Write(doc);
                    Response.End();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            return null;
        }
        //Purchase Request contract
        public ActionResult PDF(tblPurchasesRequests tblPurchasesRequests, List<tblBeneficiaryCustomers> lstBeneficiaryCustomers, string windowid)
        {
            var doc = new Document(iTextSharp.text.PageSize.LETTER);
            try
            {
                tblPurchasesRequests tblPurchasesRequests2 = PurchasesRequestsB.GetByID(tblPurchasesRequests.PurchasesRequestID);

                tblCommissionAgents tblCommissionAgent = tblPurchasesRequests.CommissionAgentID != null ? CommissionAgentsB.GetCommissionAgentsByID((int)tblPurchasesRequests.CommissionAgentID) : null;
                //CAMPOS (SALTO DE LINEA SE USA PARA MOSTRAR COLUMNA VACIA EN DOCUMENTO)
                //FECHA
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string sDateDay = tblPurchasesRequests.Date != DateTime.MinValue ? tblPurchasesRequests.Date.Day.ToString() : "\n";
                string sDateMonth = tblPurchasesRequests.Date != DateTime.MinValue ? formatoFecha.GetMonthName(tblPurchasesRequests.Date.Month) : "\n";
                string sDateYear = tblPurchasesRequests.Date != DateTime.MinValue ? tblPurchasesRequests.Date.Year.ToString() : "\n";
                //INFO SOLICITUD
                string sPurchaseRequestID = tblPurchasesRequests.PurchasesRequestID != 0 ? tblPurchasesRequests.PurchasesRequestID.ToString() : "\n";
                string sContractID = tblPurchasesRequests.ContractID != 0 ? tblPurchasesRequests.ContractID.ToString() : "\n";
                string sCreatedBy = tblPurchasesRequests.CreatedBy != 0 ? UsersB.GetUserByID(tblPurchasesRequests.CreatedBy).UserFullName : "\n";
                string sCommissionAgent = tblCommissionAgent != null ? tblCommissionAgent.FullName : "\n";
                //DATOS DEL SOLICITANTE
                //Arreglo para clientes prospectos
                string[] aCustomer = null;
                if (tblPurchasesRequests.Customer != null)
                {
                    aCustomer = tblPurchasesRequests.Customer.Split(' ');
                }
                string sName = "";
                string sPSurname = tblPurchasesRequests.PSurname != null ? tblPurchasesRequests.PSurname : aCustomer != null ? aCustomer[0] : "\n";
                string sMSurname = tblPurchasesRequests.MSurname != null ? tblPurchasesRequests.MSurname : aCustomer != null ? aCustomer[1] : "\n";
                if (aCustomer != null)
                {
                    for (int i = 2; i < aCustomer.Length; i++)
                    {
                        sName += aCustomer[i] + " ";
                    }
                }
                else if (tblPurchasesRequests.Name != null)
                    sName = tblPurchasesRequests.Name;
                else
                    sName = "\n";
                //DOMICILIO PARTICULAR
                string sAddress = tblPurchasesRequests.Address != null ? tblPurchasesRequests.Address : "\n";
                string sAddressNumber = tblPurchasesRequests.AddressNumber != null ? tblPurchasesRequests.AddressNumber : "\n";
                string sAdressInteriorNumber = tblPurchasesRequests.AdressInteriorNumber != null ? tblPurchasesRequests.AdressInteriorNumber.ToString() : "\n";
                string sNeighborhood = tblPurchasesRequests.Neighborhood != null ? tblPurchasesRequests.Neighborhood : "\n";
                string sZipCode = tblPurchasesRequests.ZipCode != null ? tblPurchasesRequests.ZipCode.ToString() : "\n";
                string sState = tblPurchasesRequests.StateID != null ? StatesB.GetStatesByID(Convert.ToInt32(tblPurchasesRequests.StateID)).Name : "\n";
                string sTown = tblPurchasesRequests.TownID != null ? TownsB.GetTownsByID(Convert.ToInt32(tblPurchasesRequests.TownID)).Name : "\n";
                string sCelPhone = tblPurchasesRequests.CelPhone != null ? tblPurchasesRequests.CelPhone : "\n";
                string sEmail = tblPurchasesRequests.Email != null ? tblPurchasesRequests.Email : "\n";
                string sRFCCURP = tblPurchasesRequests.RFCCURP != null ? tblPurchasesRequests.RFCCURP : "\n";
                string sDateOfBirth = tblPurchasesRequests.DateOfBirth != null ? tblPurchasesRequests.Date.ToString("dd/MMMM/yyyy").ToUpper() : "\n";
                string sCityOfBirth = tblPurchasesRequests.CityOfBirth != null ? tblPurchasesRequests.CityOfBirth : "\n";
                //string sCivilStatus = tblPurchasesRequests.CivilStatus != null ? tblPurchasesRequests.CivilStatus : "\n";
                string sCivilStatus = tblPurchasesRequests.CivilStatusID != null ? CivilStatusB.GetCivilStatusByID(Convert.ToInt32(tblPurchasesRequests.CivilStatusID)).Name : "\n";
                string sOccupation = tblPurchasesRequests.Occupation != null ? tblPurchasesRequests.Occupation : "\n";
                //DATOS DE LA EMPRESA DONDE PRESTA SUS SERVICIOS
                string sCompany = tblPurchasesRequests.Company != null ? tblPurchasesRequests.Company : "\n";
                string sPhoneCompany = tblPurchasesRequests.PhoneCompany != null ? tblPurchasesRequests.PhoneCompany.ToString() : "";
                string sAddressCompany = tblPurchasesRequests.AddressCompany != null ? tblPurchasesRequests.AddressCompany : "\n";
                string sExtPhoneCompany = tblPurchasesRequests.ExtPhoneCompany != null ? tblPurchasesRequests.ExtPhoneCompany.ToString() : "";
                string sTownAddressCompany = tblPurchasesRequests.TownAddressCompanyID != null ? TownsB.GetTownsByID(Convert.ToInt32(tblPurchasesRequests.TownAddressCompanyID)).Name : "\n";
                string sDelegationAddressCompany = tblPurchasesRequests.DelegationAddressCompany != null ? tblPurchasesRequests.DelegationAddressCompany : "\n";
                string sStateAddressCompany = tblPurchasesRequests.StateAddressCompanyID != null ? StatesB.GetStatesByID(Convert.ToInt32(tblPurchasesRequests.StateAddressCompanyID)).Name : "\n";
                string sIncome = tblPurchasesRequests.Income != null ? "$" + tblPurchasesRequests.Income.ToString() : "\n";
                //REFERENCIAS
                string sReferences = tblPurchasesRequests.ReferenceCustomer1 != null ? tblPurchasesRequests.ReferenceCustomer1 : "";
                string sReferences2 = tblPurchasesRequests.ReferenceCustomer2 != null ? tblPurchasesRequests.ReferenceCustomer2 : "";
                string sReferencesTel = tblPurchasesRequests.ReferenceCustomerPhone1 != null ? tblPurchasesRequests.ReferenceCustomerPhone1 : "\n";
                string sReferencesTel2 = tblPurchasesRequests.ReferenceCustomerPhone2 != null ? tblPurchasesRequests.ReferenceCustomerPhone2 : "\n";
                //ADICIONAL
                //string sMaintenanceShare = tblPurchasesRequests.CheckMaintenanceFee ? "APLICA - " + tblPurchasesRequests.MaintenanceFee.ToString("C2") : "NO APLICA";
                //string sAshesDeposit = tblPurchasesRequests.CheckFederalTax ? "APLICA - " + " " + tblPurchasesRequests.FederalTax.ToString("C2") + " MXN" : "NO APLICA";

                string sMaintenanceShare = tblPurchasesRequests.CheckMaintenanceFee ? "APLICA - " + MaintenanceFeeB.GetMaintenanceFeeCost(tblPurchasesRequests.MaintenanceFeeID).ToString("C2") + " MXN." : "NO APLICA";
                string sAshesDeposit = tblPurchasesRequests.CheckFederalTax ? "APLICA - " + " " + FederalTaxB.GetFedetalTaxCost(tblPurchasesRequests.FederalTaxID).ToString("C2") + " MXN." : "NO APLICA";
                //CONDICIONES ECONOMICAS DE LA OPERACION
                string sPaymentMethodName = tblPurchasesRequests.PaymentMethodName != null ? tblPurchasesRequests.PaymentMethodName : "\n";
                string sCriptKey = tblPurchasesRequests.Nicho != null ? tblPurchasesRequests.Nicho : "\n";
                string sLevel = tblPurchasesRequests.Level != 0 ? tblPurchasesRequests.Level.ToString() : "\n";
                string sCryptSection = tblPurchasesRequests.CryptSectionID != 0 ? CryptsSectionsB.GetById(tblPurchasesRequests.CryptSectionID).ClassificationName : "\n";
                string sCryptPrice = tblPurchasesRequests.CryptPrice != 0 ? "$" + tblPurchasesRequests.CryptPrice.ToString() + " MXN." : "\n";
                string sEnganche = tblPurchasesRequests.Enganche != 0 ? "$" + tblPurchasesRequests.Enganche.ToString() + " MXN." : "\n";
                string sSaldo = "$" + Convert.ToString(tblPurchasesRequests.CryptPrice - tblPurchasesRequests.Enganche) + " MXN.";
                int iMeses = tblPurchasesRequests.PaymentMethod == PaymentMethods.Meses12 ? 11
                    : tblPurchasesRequests.PaymentMethod == PaymentMethods.Meses18 ? 17
                    : tblPurchasesRequests.PaymentMethod == PaymentMethods.Meses24 ? 23
                    : tblPurchasesRequests.PaymentMethod == PaymentMethods.UsoInmediato ? 11
                    : 0;
                string sSaldoMeses = "EL SALDO SERA LIQUIDADO EN " + iMeses;
                string sAbonosMeses = " ABONOS DE: " + tblPurchasesRequests.Mensualidades.ToString("C2") + " MXN. C/U";
                string sDateFisrtPaymentDay = tblPurchasesRequests.FirstPaymentDate != null ? tblPurchasesRequests.FirstPaymentDate.Value.Day.ToString() : "\n";
                string sDateFisrtPaymentMonth = tblPurchasesRequests.FirstPaymentDate != null ? formatoFecha.GetMonthName(tblPurchasesRequests.FirstPaymentDate.Value.Month) : "\n";
                string sDateFisrtPaymentYear = tblPurchasesRequests.FirstPaymentDate != null ? tblPurchasesRequests.FirstPaymentDate.Value.Year.ToString() : "\n";
                string sDateLastPaymentDay = tblPurchasesRequests.LastPaymentDate != null ? tblPurchasesRequests.LastPaymentDate.Value.Day.ToString() : "\n";
                string sDateLastPaymentMonth = tblPurchasesRequests.LastPaymentDate != null ? formatoFecha.GetMonthName(tblPurchasesRequests.LastPaymentDate.Value.Month) : "\n";
                string sDateLastPaymentYear = tblPurchasesRequests.LastPaymentDate != null ? tblPurchasesRequests.LastPaymentDate.Value.Year.ToString() : "\n";
                string sCashPaymentAmount = tblPurchasesRequests.CashPaymentAmount != 0 ? "$" + tblPurchasesRequests.CashPaymentAmount.ToString() + " MXN." : "\n";
                string sCheckPaymentAmount = tblPurchasesRequests.CheckPaymentAmount != 0 ? "$" + tblPurchasesRequests.CheckPaymentAmount.ToString() + " MXN." : "\n";
                string sCheckPaymentNo = tblPurchasesRequests.CheckPaymentNo != null ? tblPurchasesRequests.CheckPaymentNo : "\n";
                string sCheckPaymentAccount = tblPurchasesRequests.CheckPaymentAccount != null ? tblPurchasesRequests.CheckPaymentAccount : "\n";
                string sCheckPaymentBank = tblPurchasesRequests.CheckPaymentBank != null ? tblPurchasesRequests.CheckPaymentBank : "\n";
                string sCreditCardAmount = tblPurchasesRequests.CreditCardPaymentAmount != 0 ? "$" + tblPurchasesRequests.CreditCardPaymentAmount.ToString() + " MXN." : "\n";
                string sCreditCardtNo = tblPurchasesRequests.CreditCardPaymentNo != null ? tblPurchasesRequests.CreditCardPaymentNo : "\n";
                string sCreditCardAccount = tblPurchasesRequests.CreditCardPaymentAccount != null ? tblPurchasesRequests.CreditCardPaymentAccount : "\n";
                string sCreditCardBank = tblPurchasesRequests.CreditCardPaymentBank != null ? tblPurchasesRequests.CreditCardPaymentBank : "\n";
                string sTransferAmount = tblPurchasesRequests.CashPaymentAmountOrTransfer != 0 ? "$" + tblPurchasesRequests.CashPaymentAmountOrTransfer.ToString() + " MXN." : "\n";
                string sCashDepositAmount = tblPurchasesRequests.CashDeposit != 0 ? "$" + tblPurchasesRequests.CashDeposit.ToString() + " MXN." : "\n";

                //MemoryStream memoryStream = new MemoryStream();
                //PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
                PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);

                doc.Open();
                doc.NewPage();

                PdfPTable MainTable = new PdfPTable(5);
                MainTable.TotalWidth = 550f;
                MainTable.LockedWidth = true;
                MainTable.HorizontalAlignment = 0;

                //CELDAS EN BLANCO
                Paragraph Line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                PdfPCell LineCell = new PdfPCell();
                LineCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                LineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                LineCell.AddElement(Line);

                PdfPCell Row16WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row12WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row10WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row8WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row7WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row6WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row5WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row4WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row3WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell Row2WhiteCell = new PdfPCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                Row16WhiteCell.Colspan = 16;
                Row12WhiteCell.Colspan = 12;
                Row10WhiteCell.Colspan = 10;
                Row8WhiteCell.Colspan = 8;
                Row7WhiteCell.Colspan = 7;
                Row6WhiteCell.Colspan = 6;
                Row5WhiteCell.Colspan = 5;
                Row4WhiteCell.Colspan = 4;
                Row3WhiteCell.Colspan = 3;
                Row2WhiteCell.Colspan = 2;

                //IMAGEN
                PdfPCell ImageTableCell = new PdfPCell();
                PdfPTable ImageTable = new PdfPTable(5);

                string imageURL = Server.MapPath("~/Content/Images/LogoParroquia.png");
                Image img = Image.GetInstance(imageURL);
                img.ScaleAbsolute(50f, 75f);

                PdfPCell ImageCell = new PdfPCell(img);
                ImageCell.Colspan = 2;
                ImageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImageCell.HorizontalAlignment = Element.ALIGN_CENTER;

                //SUBTITULOS
                PdfPCell TitleCell = new PdfPCell();
                PdfPTable TitleTable = new PdfPTable(2);

                PdfPCell MainTitleHeader = new PdfPCell(new Phrase("PARROQUIA IGLESIA DE GUADALUPE DEL RIO EN TIJUANA, A.R.", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLD)));
                PdfPCell SubTittleHeader1 = new PdfPCell(new Phrase("PASEO CENTENARIO 10150 ZONA RIO U.S.A P.O BOX 431049 SAN YSIDRO CA 92173-9998 TEL 607-37-75 Y 607-38-67", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell SubTittleHeader2 = new PdfPCell(new Phrase("TIJUANA B.C. C.P. 22320 e-mail:criptas@telnor.net", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                MainTitleHeader.Colspan = 2;
                MainTitleHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                MainTitleHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubTittleHeader1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubTittleHeader2.Border = iTextSharp.text.Rectangle.NO_BORDER;

                TitleTable.AddCell(MainTitleHeader);
                TitleTable.AddCell(SubTittleHeader1);
                TitleTable.AddCell(SubTittleHeader2);

                TitleTable.TotalWidth = 300f;
                TitleTable.LockedWidth = true;
                TitleTable.HorizontalAlignment = 0;

                TitleCell.Colspan = 3;
                TitleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                TitleCell.AddElement(TitleTable);

                ImageTable.AddCell(ImageCell);
                ImageTable.AddCell(TitleCell);

                ImageTable.TotalWidth = 530f;
                ImageTable.LockedWidth = true;
                ImageTable.HorizontalAlignment = 0;

                ImageTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImageTableCell.Colspan = 5;
                ImageTableCell.AddElement(ImageTable);
                MainTable.AddCell(ImageTableCell);

                //FECHA
                PdfPCell DateTableCell = new PdfPCell();

                PdfPTable DateTable = new PdfPTable(3);
                PdfPCell DateTitleCell = new PdfPCell();
                PdfPCell DayCell = new PdfPCell();
                PdfPCell MonthCell = new PdfPCell();
                PdfPCell YearCell = new PdfPCell();
                Paragraph DateTitleParagraph = new Paragraph("FECHA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD));
                DateTitleParagraph.Alignment = Element.ALIGN_CENTER;
                DateTitleCell.Colspan = 3;
                Paragraph DayParagraph = new Paragraph("DÍA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD));
                DayParagraph.Alignment = Element.ALIGN_CENTER;
                Paragraph MonthParagraph = new Paragraph("MES", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD));
                MonthParagraph.Alignment = Element.ALIGN_CENTER;
                Paragraph YearParagraph = new Paragraph("AÑO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD));
                YearParagraph.Alignment = Element.ALIGN_CENTER;
                DateTitleCell.AddElement(DateTitleParagraph);
                DayCell.AddElement(DayParagraph);
                MonthCell.AddElement(MonthParagraph);
                YearCell.AddElement(YearParagraph);
                DateTable.AddCell(DateTitleCell);
                DateTable.AddCell(DayCell);
                DateTable.AddCell(MonthCell);
                DateTable.AddCell(YearCell);
                //DateTable.AddCell(new Paragraph("DÍA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                //DateTable.AddCell(new Paragraph("MES", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                //DateTable.AddCell(new Paragraph("AÑO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                DateTable.AddCell(new Paragraph(sDateDay, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                DateTable.AddCell(new Paragraph(sDateMonth, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                DateTable.AddCell(new Paragraph(sDateYear, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));

                DateTable.TotalWidth = 166f;
                DateTable.LockedWidth = true;
                DateTable.HorizontalAlignment = 0;

                DateTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                DateTableCell.Colspan = 2;
                DateTableCell.AddElement(DateTable);
                MainTable.AddCell(DateTableCell);

                //CELDA ESPACIO EN BLANCO
                PdfPCell WhiteCell = new PdfPCell();
                WhiteCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MainTable.AddCell(WhiteCell);

                string nom = "";
                if (tblPurchasesRequests.CryptType == Church.Data.Enums.CryptTypes.Familiar)
                {
                    nom = "-A/F";
                }
                else
                {
                    nom = "-A/C";
                }

                int zeros = 6 - sContractID.Length;
                string ceros = "";
                for (int i = 0; i < zeros; i++)
                {
                    ceros += "0";
                }
                //INFO SOLICITUD
                PdfPCell RequestTableCell = new PdfPCell();
                PdfPTable RequestTable = new PdfPTable(2);
                RequestTable.AddCell(new Paragraph("SOLICITUD No.", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                RequestTable.AddCell(new Paragraph(sPurchaseRequestID.ToString(), new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                RequestTable.AddCell(new Paragraph("CONTRATO No.", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                RequestTable.AddCell(new Paragraph("C-JC-" + ceros + sContractID + nom, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                RequestTable.AddCell(new Paragraph("VENDEDOR No.", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                RequestTable.AddCell(new Paragraph(sCreatedBy, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                RequestTable.AddCell(new Paragraph("PROMOTOR", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                RequestTable.AddCell(new Paragraph(sCommissionAgent, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));

                RequestTable.TotalWidth = 200f;
                RequestTable.LockedWidth = true;
                RequestTable.HorizontalAlignment = 0;

                RequestTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                RequestTableCell.Colspan = 2;
                RequestTableCell.AddElement(RequestTable);
                MainTable.AddCell(RequestTableCell);

                //SALTO DE LINEA
                PdfPCell CellRow = new PdfPCell(new Paragraph(""));
                CellRow.Colspan = 5;
                CellRow.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MainTable.AddCell(CellRow);

                //DATOS DEL SOLICITANTE
                PdfPCell ApplicantHeader = new PdfPCell(new Phrase("DATOS DEL SOLICITANTE", new Font(Font.FontFamily.UNDEFINED, 8f, Font.BOLD)));
                ApplicantHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ApplicantHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                ApplicantHeader.Colspan = 5;
                MainTable.AddCell(ApplicantHeader);

                PdfPCell ApplicantTableCell = new PdfPCell();
                PdfPCell PSurnameCell = new PdfPCell();
                PdfPCell MSurnameCell = new PdfPCell();
                PdfPCell NameCell = new PdfPCell();
                PdfPTable ApplicantTable = new PdfPTable(3);

                Paragraph PSurnameParagraph = new Paragraph("APELLIDO PATERNO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD));
                PSurnameParagraph.Alignment = Element.ALIGN_CENTER;
                Paragraph MSurnameParagraph = new Paragraph("MATERNO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD));
                MSurnameParagraph.Alignment = Element.ALIGN_CENTER;
                Paragraph NameParagraph = new Paragraph("NOMBRE", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD));
                NameParagraph.Alignment = Element.ALIGN_CENTER;
                PSurnameCell.AddElement(PSurnameParagraph);
                MSurnameCell.AddElement(MSurnameParagraph);
                NameCell.AddElement(NameParagraph);
                ApplicantTable.AddCell(PSurnameCell);
                ApplicantTable.AddCell(MSurnameCell);
                ApplicantTable.AddCell(NameCell);
                //ApplicantTable.AddCell(new Paragraph("APELLIDO PATERNO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                //ApplicantTable.AddCell(new Paragraph("MATERNO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                //ApplicantTable.AddCell(new Paragraph("NOMBRES", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                ApplicantTable.AddCell(new Paragraph(sPSurname, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                ApplicantTable.AddCell(new Paragraph(sMSurname, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                ApplicantTable.AddCell(new Paragraph(sName, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));

                ApplicantTable.TotalWidth = 530f;
                //ApplicantTable.HorizontalAlignment = Element.ALIGN_CENTER;
                ApplicantTable.LockedWidth = true;
                ApplicantTable.HorizontalAlignment = 0;


                ApplicantTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ApplicantTableCell.Colspan = 5;
                ApplicantTableCell.AddElement(ApplicantTable);
                MainTable.AddCell(ApplicantTableCell);

                //DOMICILIO PARTICULAR
                PdfPCell AddressHeader = new PdfPCell(new Phrase("DOMICILIO PARTICULAR", new Font(Font.FontFamily.UNDEFINED, 8f, Font.BOLD)));
                AddressHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AddressHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                AddressHeader.Colspan = 5;
                MainTable.AddCell(AddressHeader);

                PdfPCell AddressTableCell = new PdfPCell();
                PdfPTable AddressTable = new PdfPTable(5);

                PdfPCell StreetTitleCell = new PdfPCell(new Paragraph("CALLE, AV., BLVD. CALZ", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                StreetTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell StreetCell = new PdfPCell(new Paragraph(sAddress, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                StreetTitleCell.Colspan = 3;
                StreetCell.Colspan = 3;
                AddressTable.AddCell(StreetTitleCell);
                PdfPCell NumberCell = new PdfPCell(new Paragraph("NUMERO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                NumberCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell InterCell = new PdfPCell(new Paragraph("INTERIOR", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                InterCell.HorizontalAlignment = Element.ALIGN_CENTER;
                AddressTable.AddCell(NumberCell);
                AddressTable.AddCell(InterCell);
                //AddressTable.AddCell(new Paragraph("NUMERO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                //AddressTable.HorizontalAlignment = Element.ALIGN_CENTER;
                //AddressTable.AddCell(new Paragraph("INTERIOR", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                //AddressTable.HorizontalAlignment = Element.ALIGN_CENTER;
                AddressTable.AddCell(StreetCell);
                AddressTable.AddCell(new Paragraph(sAddressNumber, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                AddressTable.AddCell(new Paragraph(sAdressInteriorNumber, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));

                PdfPCell AreaTitleCell = new PdfPCell(new Paragraph("COLONIA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                PdfPCell PostalCodeTitleCell = new PdfPCell(new Paragraph("CODIGO POSTAL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                PdfPCell AreaCell = new PdfPCell(new Paragraph(sNeighborhood, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell PostalCodeCell = new PdfPCell(new Paragraph(sZipCode, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                AreaTitleCell.Colspan = 3;
                AreaTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PostalCodeTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PostalCodeTitleCell.Colspan = 2;
                AreaCell.Colspan = 3;
                PostalCodeCell.Colspan = 2;

                AddressTable.AddCell(AreaTitleCell);
                AddressTable.AddCell(PostalCodeTitleCell);
                AddressTable.AddCell(AreaCell);
                AddressTable.AddCell(PostalCodeCell);

                PdfPCell CityTitleCell = new PdfPCell(new Paragraph("CIUDAD", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                CityTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell DelegationTitleCell = new PdfPCell(new Paragraph("DELEGACION", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                DelegationTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell StateTitleCell = new PdfPCell(new Paragraph("ESTADO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                StateTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell CityCell = new PdfPCell(new Paragraph(sTown, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell StateCell = new PdfPCell(new Paragraph(sState, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                CityTitleCell.Colspan = 2;
                StateTitleCell.Colspan = 2;
                CityCell.Colspan = 2;
                StateCell.Colspan = 2;

                AddressTable.AddCell(CityTitleCell);
                AddressTable.AddCell(DelegationTitleCell);
                AddressTable.AddCell(StateTitleCell);
                AddressTable.AddCell(CityCell);
                AddressTable.AddCell(new Paragraph("\n", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                AddressTable.AddCell(StateCell);

                PdfPCell PhoneTitleCell = new PdfPCell(new Paragraph("TELEFONO PARTICULAR", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                PhoneTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell EmailTitleCell = new PdfPCell(new Paragraph("CORREO ELECTRONICO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                EmailTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell PhoneCell = new PdfPCell(new Paragraph(sCelPhone, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell EmailCell = new PdfPCell(new Paragraph(sEmail, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PhoneTitleCell.Colspan = 3;
                EmailTitleCell.Colspan = 2;
                PhoneCell.Colspan = 3;
                EmailCell.Colspan = 2;

                AddressTable.AddCell(PhoneTitleCell);
                AddressTable.AddCell(EmailTitleCell);
                AddressTable.AddCell(PhoneCell);
                AddressTable.AddCell(EmailCell);

                PdfPCell RFCTitleCell = new PdfPCell(new Paragraph("R.F.C", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                RFCTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell BirthDateTitleCell = new PdfPCell(new Paragraph("FECHA DE NACIMIENTO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                BirthDateTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell BirthPlaceTitleCell = new PdfPCell(new Paragraph("LUGAR DE NACIMIENTO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                BirthPlaceTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell CivilStatusTitleCell = new PdfPCell(new Paragraph("ESTADO CIVIL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                CivilStatusTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell OcupationTitleCell = new PdfPCell(new Paragraph("OCUPACION", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                OcupationTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell RFCCell = new PdfPCell(new Paragraph(sRFCCURP, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell BirthDateCell = new PdfPCell(new Paragraph(sDateOfBirth, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell BirthPlaceCell = new PdfPCell(new Paragraph(sCityOfBirth, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CivilStatusCell = new PdfPCell(new Paragraph(sCivilStatus, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell OcupationCell = new PdfPCell(new Paragraph(sOccupation, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                RFCCell.Colspan = 4;
                BirthDateCell.Colspan = 4;
                BirthPlaceCell.Colspan = 4;
                CivilStatusCell.Colspan = 4;
                OcupationCell.Colspan = 4;

                AddressTable.AddCell(RFCTitleCell);
                AddressTable.AddCell(RFCCell);
                AddressTable.AddCell(BirthDateTitleCell);
                AddressTable.AddCell(BirthDateCell);
                AddressTable.AddCell(BirthPlaceTitleCell);
                AddressTable.AddCell(BirthPlaceCell);
                AddressTable.AddCell(CivilStatusTitleCell);
                AddressTable.AddCell(CivilStatusCell);
                AddressTable.AddCell(OcupationTitleCell);
                AddressTable.AddCell(OcupationCell);

                AddressTable.TotalWidth = 530f;
                AddressTable.LockedWidth = true;
                AddressTable.HorizontalAlignment = 0;

                AddressTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AddressTableCell.Colspan = 5;
                AddressTableCell.AddElement(AddressTable);
                MainTable.AddCell(AddressTableCell);

                //DATOS DE LA EMPRESA DONDE PRESTA SUS SERVICIOS
                PdfPCell CompanyHeader = new PdfPCell(new Phrase("DATOS DE LA EMPRESA DONDE PRESTA SUS SERVICIOS", new Font(Font.FontFamily.UNDEFINED, 8f, Font.BOLD)));
                CompanyHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CompanyHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                CompanyHeader.Colspan = 5;
                MainTable.AddCell(CompanyHeader);

                PdfPCell CompanyTableCell = new PdfPCell();
                PdfPTable CompanyTable = new PdfPTable(3);

                PdfPCell CompanyNameTitleCell = new PdfPCell(new Paragraph("NOMBRE DE LA COMPAÑIA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CompanyNameCell = new PdfPCell(new Paragraph(sCompany, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                CompanyNameCell.Colspan = 2;

                CompanyTable.AddCell(CompanyNameTitleCell);
                CompanyTable.AddCell(CompanyNameCell);

                PdfPCell CompanyAddressTitleCell = new PdfPCell(new Paragraph("DOMICILIO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CompanyPhoneTitleCell = new PdfPCell(new Paragraph("TELEFONO: " + sPhoneCompany, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                CompanyAddressTitleCell.Colspan = 2;

                CompanyTable.AddCell(CompanyAddressTitleCell);
                CompanyTable.AddCell(CompanyPhoneTitleCell);

                PdfPCell CompanyExtTitleCell = new PdfPCell(new Paragraph("EXT: " + sExtPhoneCompany, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CompanyAddressCell = new PdfPCell(new Paragraph(sAddressCompany, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                CompanyAddressCell.Colspan = 2;

                CompanyTable.AddCell(CompanyAddressCell);
                CompanyTable.AddCell(CompanyExtTitleCell);

                PdfPCell CompanyCityTitleCell = new PdfPCell(new Paragraph("CIUDAD", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CompanyTownTitleCell = new PdfPCell(new Paragraph("MUNICIPIO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CompanyStateTitleCell = new PdfPCell(new Paragraph("ESTADO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));

                CompanyTable.AddCell(CompanyCityTitleCell);
                CompanyTable.AddCell(CompanyTownTitleCell);
                CompanyTable.AddCell(CompanyStateTitleCell);
                CompanyTable.AddCell(new Paragraph(sTownAddressCompany, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                CompanyTable.AddCell(new Paragraph(sDelegationAddressCompany, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                CompanyTable.AddCell(new Paragraph(sStateAddressCompany, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));

                PdfPCell CompanyIngressTitleCell = new PdfPCell(new Paragraph("INGRESO PROMEDIO MENSUAL (Incluye a su conyuge)", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CompanyIncomeCell = new PdfPCell(new Paragraph(sIncome, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                CompanyIngressTitleCell.Colspan = 2;

                CompanyTable.AddCell(CompanyIngressTitleCell);
                CompanyTable.AddCell(CompanyIncomeCell);

                CompanyTable.TotalWidth = 530f;
                CompanyTable.LockedWidth = true;
                CompanyTable.HorizontalAlignment = 0;

                CompanyTableCell.AddElement(CompanyTable);
                CompanyTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CompanyTableCell.Colspan = 5;
                MainTable.AddCell(CompanyTableCell);

                //REFERENCIAS
                PdfPCell ReferencesHeader = new PdfPCell(new Phrase("REFERENCIAS", new Font(Font.FontFamily.UNDEFINED, 8f, Font.BOLD)));
                ReferencesHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                ReferencesHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ReferencesHeader.Colspan = 5;
                MainTable.AddCell(ReferencesHeader);

                PdfPCell ReferencesTableCell = new PdfPCell();
                PdfPTable ReferencesTable = new PdfPTable(18);

                PdfPCell ReferenceTitleCell = new PdfPCell(new Paragraph("1) " + sReferences, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell ReferenceTitleCell2 = new PdfPCell(new Paragraph("2) " + sReferences2, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell ReferenceTelTitleCell = new PdfPCell(new Paragraph("TEL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell ReferenceTelTitleCell2 = new PdfPCell(new Paragraph("TEL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell ReferenceTelCell = new PdfPCell(new Paragraph(sReferencesTel, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell ReferenceTel2Cell = new PdfPCell(new Paragraph(sReferencesTel2, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                ReferenceTitleCell.Colspan = 12;
                ReferenceTitleCell2.Colspan = 12;
                ReferenceTelCell.Colspan = 5;
                ReferenceTel2Cell.Colspan = 5;

                ReferencesTable.AddCell(ReferenceTitleCell);
                ReferencesTable.AddCell(ReferenceTelTitleCell);
                ReferencesTable.AddCell(ReferenceTelCell);
                ReferencesTable.AddCell(ReferenceTitleCell2);
                ReferencesTable.AddCell(ReferenceTelTitleCell2);
                ReferencesTable.AddCell(ReferenceTel2Cell);

                ReferencesTable.TotalWidth = 530f;
                ReferencesTable.LockedWidth = true;
                ReferencesTable.HorizontalAlignment = 0;

                ReferencesTableCell.AddElement(ReferencesTable);
                ReferencesTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ReferencesTableCell.Colspan = 5;
                MainTable.AddCell(ReferencesTableCell);

                //BENEFICIARIOS
                PdfPCell BeneficiariesHeader = new PdfPCell(new Phrase("BENEFICIARIOS", new Font(Font.FontFamily.UNDEFINED, 8f, Font.BOLD)));
                BeneficiariesHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                BeneficiariesHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                BeneficiariesHeader.Colspan = 5;
                MainTable.AddCell(BeneficiariesHeader);

                PdfPCell BeneficiariesTableCell = new PdfPCell();
                PdfPTable BeneficiariesTable = new PdfPTable(18);

                PdfPCell BeneficiaryBirthDateTitleCell = new PdfPCell(new Paragraph("FECHA NAC.", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                BeneficiaryBirthDateTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell BeneficiaryRelationshipTitleCell = new PdfPCell(new Paragraph("PARENTESCO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                BeneficiaryRelationshipTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell BeneficiaryPhoneTitleCell = new PdfPCell(new Paragraph("TELEFONO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                BeneficiaryPhoneTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell BeneficiaryNameTitleCell = new PdfPCell(new Paragraph("NOMBRE(S)", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                BeneficiaryNameTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell BeneficiaryLastNameTitleCell = new PdfPCell(new Paragraph("APELLIDOS", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                BeneficiaryLastNameTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                BeneficiaryBirthDateTitleCell.Colspan = 3;
                BeneficiaryRelationshipTitleCell.Colspan = 2;
                BeneficiaryPhoneTitleCell.Colspan = 3;
                BeneficiaryNameTitleCell.Colspan = 5;
                BeneficiaryLastNameTitleCell.Colspan = 5;

                BeneficiariesTable.AddCell(Row10WhiteCell);
                BeneficiariesTable.AddCell(BeneficiaryBirthDateTitleCell);
                BeneficiariesTable.AddCell(BeneficiaryRelationshipTitleCell);
                BeneficiariesTable.AddCell(BeneficiaryPhoneTitleCell);
                BeneficiariesTable.AddCell(BeneficiaryNameTitleCell);
                BeneficiariesTable.AddCell(BeneficiaryLastNameTitleCell);
                BeneficiariesTable.AddCell(new Paragraph("DIA", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));
                BeneficiariesTable.AddCell(new Paragraph("MES", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));
                BeneficiariesTable.AddCell(new Paragraph("AÑO", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));
                BeneficiariesTable.AddCell(Row2WhiteCell);
                BeneficiariesTable.AddCell(Row3WhiteCell);

                //Lista de benericiarios
                string sBeneficiaryName = "";
                string sBeneficiaryLastName = "";
                string sBeneficiaryRelationship = "";
                string sBeneficiaryDateBirthDay = "";
                string sBeneficiaryDateBirthMonth = "";
                string sBeneficiaryDateBirthYear = "";
                string sBeneficiaryPhone = "";

                foreach (var item in lstBeneficiaryCustomers ?? new List<tblBeneficiaryCustomers>())
                {
                    sBeneficiaryName = item.Name != null ? item.Name : "\n";
                    sBeneficiaryLastName = item.Surnames != null ? item.Surnames : "\n";
                    sBeneficiaryRelationship = item.Relationship != null ? item.Relationship : "\n";
                    sBeneficiaryDateBirthDay = item.Birthdate != DateTime.MinValue ? item.Birthdate.Day.ToString() : "\n";
                    sBeneficiaryDateBirthMonth = item.Birthdate != DateTime.MinValue ? formatoFecha.GetMonthName(item.Birthdate.Month) : "\n";
                    sBeneficiaryDateBirthYear = item.Birthdate != DateTime.MinValue ? item.Birthdate.Year.ToString() : "\n";
                    sBeneficiaryPhone = item.CelPhone != null ? item.CelPhone : "\n";

                    PdfPCell BeneficiaryNameCell = new PdfPCell(new Paragraph(sBeneficiaryName, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell BeneficiaryLastNameCell = new PdfPCell(new Paragraph(sBeneficiaryLastName, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell BeneficiaryRelationshipCell = new PdfPCell(new Paragraph(sBeneficiaryRelationship, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell BeneficiaryCelPhoneCell = new PdfPCell(new Paragraph(sBeneficiaryPhone, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    BeneficiaryNameCell.Colspan = 5;
                    BeneficiaryLastNameCell.Colspan = 5;
                    BeneficiaryRelationshipCell.Colspan = 2;
                    BeneficiaryCelPhoneCell.Colspan = 3;

                    BeneficiariesTable.AddCell(BeneficiaryNameCell);
                    BeneficiariesTable.AddCell(BeneficiaryLastNameCell);
                    BeneficiariesTable.AddCell(new Paragraph(sBeneficiaryDateBirthDay, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    BeneficiariesTable.AddCell(new Paragraph(sBeneficiaryDateBirthMonth, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    BeneficiariesTable.AddCell(new Paragraph(sBeneficiaryDateBirthYear, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    BeneficiariesTable.AddCell(BeneficiaryRelationshipCell);
                    BeneficiariesTable.AddCell(BeneficiaryCelPhoneCell);
                }

                BeneficiariesTable.TotalWidth = 530f;
                BeneficiariesTable.LockedWidth = true;
                BeneficiariesTable.HorizontalAlignment = 0;

                BeneficiariesTableCell.AddElement(BeneficiariesTable);
                BeneficiariesTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                BeneficiariesTableCell.Colspan = 5;
                MainTable.AddCell(BeneficiariesTableCell);

                //CONDICIONES ECONOMICAS DE LA OPERACION
                PdfPCell ConditionsHeader = new PdfPCell(new Phrase("CONDICIONES ECONOMICAS DE LA OPERACION", new Font(Font.FontFamily.UNDEFINED, 8f, Font.BOLD)));
                ConditionsHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ConditionsHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                ConditionsHeader.Colspan = 5;
                MainTable.AddCell(ConditionsHeader);

                PdfPCell ConditionsTableCell = new PdfPCell();
                PdfPTable ConditionsTable = new PdfPTable(18);

                PdfPCell SalesPlanTitleCell = new PdfPCell(new Paragraph("PLAN DE VENTA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                SalesPlanTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell CryptKeyTitleCell = new PdfPCell(new Paragraph("CLAVE DE LA CRIPTA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                CryptKeyTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell LevelTitleCell = new PdfPCell(new Paragraph("NIVEL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                LevelTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell CryptSectionTitleCell = new PdfPCell(new Paragraph("AREA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                CryptSectionTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell ConditionsZoneTitleCell = new PdfPCell(new Paragraph("ZONA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                ConditionsZoneTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                SalesPlanTitleCell.Colspan = 3;
                CryptKeyTitleCell.Colspan = 3;
                LevelTitleCell.Colspan = 3;
                CryptSectionTitleCell.Colspan = 6;
                ConditionsZoneTitleCell.Colspan = 3;

                ConditionsTable.AddCell(SalesPlanTitleCell);
                ConditionsTable.AddCell(CryptKeyTitleCell);
                ConditionsTable.AddCell(LevelTitleCell);
                ConditionsTable.AddCell(CryptSectionTitleCell);
                ConditionsTable.AddCell(ConditionsZoneTitleCell);

                PdfPCell SalesPlanCell = new PdfPCell(new Paragraph(sPaymentMethodName, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CryptKeyCell = new PdfPCell(new Paragraph(sCriptKey, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell LevelCell = new PdfPCell(new Paragraph(sLevel, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CryptSectionCell = new PdfPCell(new Paragraph(sCryptSection, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                SalesPlanCell.Colspan = 3;
                CryptKeyCell.Colspan = 3;
                LevelCell.Colspan = 3;
                CryptSectionCell.Colspan = 6;

                ConditionsTable.AddCell(SalesPlanCell);
                ConditionsTable.AddCell(CryptKeyCell);
                ConditionsTable.AddCell(LevelCell);
                ConditionsTable.AddCell(CryptSectionCell);
                ConditionsTable.AddCell(Row3WhiteCell);

                PdfPCell CryptPriceTitleCell = new PdfPCell(new Paragraph("IMPORTE INICIAL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                CryptPriceTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell EngancheTitleCell = new PdfPCell(new Paragraph("PAGO INICIAL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                EngancheTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell SaldoTitleCell = new PdfPCell(new Paragraph("SALDO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                SaldoTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                CryptPriceTitleCell.Colspan = 6;
                EngancheTitleCell.Colspan = 6;
                SaldoTitleCell.Colspan = 6;

                PdfPCell CryptPriceCell = new PdfPCell(new Paragraph(sCryptPrice, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell EngancheCell = new PdfPCell(new Paragraph(sEnganche, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell SaldoCell = new PdfPCell(new Paragraph(sSaldo, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                CryptPriceCell.Colspan = 6;
                EngancheCell.Colspan = 6;
                SaldoCell.Colspan = 6;

                ConditionsTable.AddCell(CryptPriceTitleCell);
                ConditionsTable.AddCell(EngancheTitleCell);
                ConditionsTable.AddCell(SaldoTitleCell);
                ConditionsTable.AddCell(CryptPriceCell);
                ConditionsTable.AddCell(EngancheCell);
                ConditionsTable.AddCell(SaldoCell);

                if (sPaymentMethodName != "Contado")
                {
                    PdfPCell BalanceSoldOutMonthPaymentTitleCell = new PdfPCell(new Paragraph(sSaldoMeses + sAbonosMeses, new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                    BalanceSoldOutMonthPaymentTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    BalanceSoldOutMonthPaymentTitleCell.Colspan = 18;

                    ConditionsTable.AddCell(BalanceSoldOutMonthPaymentTitleCell);

                    ConditionsTable.AddCell(Row7WhiteCell);
                    ConditionsTable.AddCell(new Paragraph("DIA", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));
                    ConditionsTable.AddCell(new Paragraph("MES", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));
                    ConditionsTable.AddCell(new Paragraph("AÑO", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));
                    ConditionsTable.AddCell(Row5WhiteCell);
                    ConditionsTable.AddCell(new Paragraph("DIA", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));
                    ConditionsTable.AddCell(new Paragraph("MES", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));
                    ConditionsTable.AddCell(new Paragraph("AÑO", new Font(Font.FontFamily.UNDEFINED, 8f, Font.NORMAL)));

                    PdfPCell DateFirstPaymentTitleCell = new PdfPCell(new Paragraph("SIENDO EL PRIMERO DE ELLOS EN", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                    DateFirstPaymentTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell DateLastPaymentTitleCell = new PdfPCell(new Paragraph("Y EL ULTIMO EN", new Font(Font.FontFamily.UNDEFINED, 6f, Font.BOLD)));
                    DateLastPaymentTitleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    DateFirstPaymentTitleCell.Colspan = 7;
                    DateLastPaymentTitleCell.Colspan = 5;

                    ConditionsTable.AddCell(DateFirstPaymentTitleCell);
                    ConditionsTable.AddCell(new Paragraph(sDateFisrtPaymentDay, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    ConditionsTable.AddCell(new Paragraph(sDateFisrtPaymentMonth, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    ConditionsTable.AddCell(new Paragraph(sDateFisrtPaymentYear, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    ConditionsTable.AddCell(DateLastPaymentTitleCell);
                    ConditionsTable.AddCell(new Paragraph(sDateLastPaymentDay, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    ConditionsTable.AddCell(new Paragraph(sDateLastPaymentMonth, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    ConditionsTable.AddCell(new Paragraph(sDateLastPaymentYear, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                }

                ConditionsTable.TotalWidth = 530f;
                ConditionsTable.LockedWidth = true;
                ConditionsTable.HorizontalAlignment = 0;

                ConditionsTableCell.AddElement(ConditionsTable);
                ConditionsTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ConditionsTableCell.Colspan = 5;
                MainTable.AddCell(ConditionsTableCell);


                //ADICIONAL
                PdfPCell AdditionalHeader = new PdfPCell(new Phrase("ADICIONAL", new Font(Font.FontFamily.UNDEFINED, 8f, Font.BOLD)));
                AdditionalHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AdditionalHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                AdditionalHeader.Colspan = 5;
                MainTable.AddCell(AdditionalHeader);

                PdfPCell AdditionalTableCell = new PdfPCell();
                PdfPTable AdditionalTable = new PdfPTable(18);

                PdfPCell MaintenanceShareTitleCell = new PdfPCell(new Paragraph("CUOTA DE MANTENIMIENTO ANUAL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell AshesDepositeTitleCell = new PdfPCell(new Paragraph("DEPÓSITO DE CENÍZAS", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                MaintenanceShareTitleCell.Colspan = 9;
                AshesDepositeTitleCell.Colspan = 9;

                PdfPCell MaintenanceShareCell = new PdfPCell(new Paragraph(sMaintenanceShare, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell AshesDepositeCell = new PdfPCell(new Paragraph(sAshesDeposit, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                MaintenanceShareCell.Colspan = 9;
                AshesDepositeCell.Colspan = 9;

                AdditionalTable.AddCell(MaintenanceShareTitleCell);
                AdditionalTable.AddCell(AshesDepositeTitleCell);
                AdditionalTable.AddCell(MaintenanceShareCell);
                AdditionalTable.AddCell(AshesDepositeCell);

                AdditionalTable.TotalWidth = 530f;
                AdditionalTable.LockedWidth = true;
                AdditionalTable.HorizontalAlignment = 0;

                AdditionalTableCell.AddElement(AdditionalTable);
                AdditionalTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AdditionalTableCell.Colspan = 5;
                MainTable.AddCell(AdditionalTableCell);

                //TABLA DE PAGOS
                PdfPCell PaysTableCell = new PdfPCell();
                PdfPTable PaysTable = new PdfPTable(18);

                PdfPCell InitialPayCell = new PdfPCell(new Paragraph("FORMA DEL PAGO INICIAL", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell InitialPayFormCell = new PdfPCell();
                PdfPTable InitialPayTable = new PdfPTable(20);

                PdfPCell QuantityTitleCell = new PdfPCell(new Paragraph("CANTIDAD", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell CheckNumberTitleCell = new PdfPCell(new Paragraph("No. DE CHEQUE", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell AccountNumberTitleCell = new PdfPCell(new Paragraph("No. DE CUENTA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell BankTitleCell = new PdfPCell(new Paragraph("BANCO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                QuantityTitleCell.Colspan = 4;
                CheckNumberTitleCell.Colspan = 4;
                AccountNumberTitleCell.Colspan = 4;
                BankTitleCell.Colspan = 4;

                InitialPayTable.AddCell(Row4WhiteCell);
                InitialPayTable.AddCell(QuantityTitleCell);
                InitialPayTable.AddCell(CheckNumberTitleCell);
                InitialPayTable.AddCell(AccountNumberTitleCell);
                InitialPayTable.AddCell(BankTitleCell);

                //Si tipo de pago es cheque
                if (tblPurchasesRequests.CheckPaymentAmount != null)
                {
                    PdfPCell CheckTitleCell = new PdfPCell(new Paragraph("CHEQUE", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CheckPaymentAmountCell = new PdfPCell(new Paragraph(sCheckPaymentAmount, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CheckPaymentNoCell = new PdfPCell(new Paragraph(sCheckPaymentNo, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CheckPaymentAccountCell = new PdfPCell(new Paragraph(sCheckPaymentAccount, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CheckPaymentBankCell = new PdfPCell(new Paragraph(sCheckPaymentBank, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    CheckTitleCell.Colspan = 4;
                    CheckPaymentAmountCell.Colspan = 4;
                    CheckPaymentNoCell.Colspan = 4;
                    CheckPaymentAccountCell.Colspan = 4;
                    CheckPaymentBankCell.Colspan = 4;

                    InitialPayTable.AddCell(CheckTitleCell);
                    InitialPayTable.AddCell(CheckPaymentAmountCell);
                    InitialPayTable.AddCell(CheckPaymentNoCell);
                    InitialPayTable.AddCell(CheckPaymentAccountCell);
                    InitialPayTable.AddCell(CheckPaymentBankCell);
                }

                //Tipo de pago es tarjeta de credito
                if (tblPurchasesRequests.CreditCardPaymentAmount != null)
                {
                    PdfPCell CreditCardTitleCell = new PdfPCell(new Paragraph("T. DE CREDITO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CreditCardAmountCell = new PdfPCell(new Paragraph(sCreditCardAmount, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CreditCardNoCell = new PdfPCell(new Paragraph(sCreditCardtNo, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CreditCardAccountCell = new PdfPCell(new Paragraph(sCreditCardAccount, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CreditCardBankCell = new PdfPCell(new Paragraph(sCreditCardBank, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    CreditCardTitleCell.Colspan = 4;
                    CreditCardAmountCell.Colspan = 4;
                    CreditCardNoCell.Colspan = 4;
                    CreditCardAccountCell.Colspan = 4;
                    CreditCardBankCell.Colspan = 4;

                    InitialPayTable.AddCell(CreditCardTitleCell);
                    InitialPayTable.AddCell(CreditCardAmountCell);
                    InitialPayTable.AddCell(CreditCardNoCell);
                    InitialPayTable.AddCell(CreditCardAccountCell);
                    InitialPayTable.AddCell(CreditCardBankCell);
                }

                //Si tipo de pago es trasnferencia
                if (tblPurchasesRequests.CashPaymentAmountOrTransfer != null)
                {
                    PdfPCell TransferTitleCell = new PdfPCell(new Paragraph("TRANSFERENCIA", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell TrasnferCell = new PdfPCell(new Paragraph(sTransferAmount, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    TransferTitleCell.Colspan = 4;
                    TrasnferCell.Colspan = 16;

                    InitialPayTable.AddCell(TransferTitleCell);
                    InitialPayTable.AddCell(TrasnferCell);
                }

                //Si tipo de pago es deposito
                if (tblPurchasesRequests.CashDeposit != null)
                {
                    PdfPCell CashDepositTitleCell = new PdfPCell(new Paragraph("DÉPOSITO EFECTIVO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CashDepositCellCell = new PdfPCell(new Paragraph(sCashDepositAmount, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    CashDepositTitleCell.Colspan = 4;
                    CashDepositCellCell.Colspan = 16;

                    InitialPayTable.AddCell(CashDepositTitleCell);
                    InitialPayTable.AddCell(CashDepositCellCell);
                }

                //Si tipo de pago es efectivo
                if (tblPurchasesRequests.CashPaymentAmount != null)
                {
                    PdfPCell CashTitleCell = new PdfPCell(new Paragraph("EFECTIVO", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    PdfPCell CashCellCell = new PdfPCell(new Paragraph(sCashPaymentAmount, new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    CashTitleCell.Colspan = 4;
                    CashCellCell.Colspan = 16;

                    InitialPayTable.AddCell(CashTitleCell);
                    InitialPayTable.AddCell(CashCellCell);
                }

                InitialPayTable.TotalWidth = 410f;
                InitialPayTable.LockedWidth = true;
                InitialPayTable.HorizontalAlignment = 0;

                InitialPayFormCell.AddElement(InitialPayTable);
                InitialPayFormCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                InitialPayFormCell.Colspan = 14;
                InitialPayCell.Colspan = 4;

                PaysTable.AddCell(InitialPayCell);
                PaysTable.AddCell(InitialPayFormCell);

                PaysTable.TotalWidth = 530f;
                PaysTable.LockedWidth = true;
                PaysTable.HorizontalAlignment = 0;

                PaysTableCell.AddElement(PaysTable);
                PaysTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PaysTableCell.Colspan = 5;
                MainTable.AddCell(PaysTableCell);

                //FIRMAS REPRESENTANTE Y VENDEDOR
                PdfPCell SignatureCell = new PdfPCell();
                PdfPTable SignatureTable = new PdfPTable(2);

                PdfPCell SignatureHeader = new PdfPCell(new Phrase("FIRMAS DEL REPRESENTANTE Y VENDEDOR", new Font(Font.FontFamily.UNDEFINED, 8f, Font.BOLD)));
                SignatureHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SignatureHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                SignatureHeader.Colspan = 2;

                PdfPCell SignatureParagraph = new PdfPCell(new Paragraph("Manifiesto que he recibido el pago inicial correspondiente a la presente operacion de acuerdo a la forma que se especifica en la presente solicitud.", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                SignatureParagraph.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SignatureParagraph.HorizontalAlignment = Element.ALIGN_CENTER;
                SignatureParagraph.Colspan = 2;

                SignatureTable.AddCell(SignatureHeader);
                SignatureTable.AddCell(SignatureParagraph);

                PdfPCell SalesManSignature = new PdfPCell(new Paragraph("NOMBRE Y FIRMA DEL VENDEDOR", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell SalesManagerSignature = new PdfPCell(new Paragraph("Vo. Bo. GERENCIA DE VENTAS", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                SalesManSignature.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SalesManSignature.HorizontalAlignment = Element.ALIGN_CENTER;
                SalesManagerSignature.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SalesManagerSignature.HorizontalAlignment = Element.ALIGN_CENTER;

                SignatureTable.AddCell(LineCell);
                SignatureTable.AddCell(LineCell);
                SignatureTable.AddCell(SalesManSignature);
                SignatureTable.AddCell(SalesManagerSignature);

                SignatureCell.AddElement(SignatureTable);
                SignatureCell.Colspan = 5;
                MainTable.AddCell(SignatureCell);

                //PARRAFO
                PdfPCell ParagraphCell = new PdfPCell(new Paragraph("Con la aceptacion de la presente solicitud me comprometo a firmar el contrato correspondiente una vez transcurrido el plazo de cinco" +
                    " dias habiles contados a partir de la firma de esta solicitud y no habiendo hecho el uso del derecho de revocar mi consentiemiento por la firma del contrato de cesión de derechos de uso mortuorio a perpetuidad que ampara" +
                    " la presente operacion, cuyos principales terminos y condiciones son los establecidos en esta solicitud. Lo anterior sin responsabilidad alguna que establece el Art. 48 de la Ley Federal de Protección al Consumidor." +
                    " Igualmente me obligo al suscribir al momento de firmar el mencionado contrato, el pagaré correspondiente a la presente operación para garantizar el saldo del valor o precio convenido" +
                    " Declaro estar de acuerdo con la presente solicitud, así como la información que he proporcionado es real y verídica.", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                ParagraphCell.Colspan = 5;
                MainTable.AddCell(ParagraphCell);

                //FIRMA ADQUIRIENTE
                PdfPCell FinalSignatureHeader = new PdfPCell(new Phrase("NOMBRE Y FIRMA DEL ADQUIRIENTE", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLD)));
                FinalSignatureHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FinalSignatureHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                FinalSignatureHeader.Colspan = 5;
                LineCell.Colspan = 5;
                Row5WhiteCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                MainTable.AddCell(Row5WhiteCell);
                MainTable.AddCell(LineCell);
                MainTable.AddCell(FinalSignatureHeader);

                PdfPCell FinalSignatureCell = new PdfPCell();
                PdfPTable FinalSignatureTable = new PdfPTable(3);

                PdfPCell OriginalFile = new PdfPCell(new Paragraph("Original: Expediente", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell BlueCopy = new PdfPCell(new Paragraph("Copia Azul: Adquiriente", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                PdfPCell PinkCopy = new PdfPCell(new Paragraph("Copia Rosa: Contabilidad", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                OriginalFile.Border = iTextSharp.text.Rectangle.NO_BORDER;
                OriginalFile.HorizontalAlignment = Element.ALIGN_CENTER;
                BlueCopy.Border = iTextSharp.text.Rectangle.NO_BORDER;
                BlueCopy.HorizontalAlignment = Element.ALIGN_CENTER;
                PinkCopy.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PinkCopy.HorizontalAlignment = Element.ALIGN_CENTER;

                FinalSignatureTable.AddCell(OriginalFile);
                FinalSignatureTable.AddCell(BlueCopy);
                FinalSignatureTable.AddCell(PinkCopy);

                FinalSignatureCell.AddElement(FinalSignatureTable);
                FinalSignatureCell.Colspan = 5;
                FinalSignatureCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MainTable.AddCell(FinalSignatureCell);

                doc.Add(MainTable);

                writer.CloseStream = false;
                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=PurchaseRequest.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.Cookies.Add(new HttpCookie(windowid, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ff")));
                Response.Cookies.Add(new HttpCookie("sPurchaseRequestID", sPurchaseRequestID));
                Response.Cookies.Add(new HttpCookie("bQuotation", tblPurchasesRequests.Quotation.ToString()));
                Response.End();
                //memoryStream.Position = 0;
                //return File(memoryStream, "application/pdf");
                //return RedirectToAction("Index", "Customers");
            }
            catch (Exception ex)
            {
                string error = ex.Message + " Inner Exception: " + ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home", new { error = error });
            }
            return null;
        }

        //Contract familiar crypt
        public ActionResult Contract(string window_contract_id, string sPaymentMethod)
        {
            HttpCookie purchaseID = Request.Cookies["sPurchaseRequestID"];
            HttpCookie quotation = Request.Cookies["bQuotation"];
            var doc = new Document(iTextSharp.text.PageSize.LETTER);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);

                int PurchaseID = 0;
                if (quotation.Value == "False")
                {
                    PurchaseID = Convert.ToInt32(purchaseID.Value);
                }

                dtoPurchasesRequest dtoPurchasesRequest = PurchasesRequestsB.GetByIDForContract(PurchaseID);

                doc.Open();
                doc.NewPage();

                PdfPTable MainTable = new PdfPTable(3);
                MainTable.TotalWidth = 550f;
                MainTable.LockedWidth = true;
                MainTable.HorizontalAlignment = 0;

                //CELDAS EN BLANCO
                PdfPCell WhiteCell = new PdfPCell(new Paragraph(""));
                WhiteCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                Paragraph Line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 10)));
                PdfPCell LineCell = new PdfPCell();
                LineCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                LineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                LineCell.AddElement(Line);
                LineCell.Colspan = 3;

                //SALTO DE LINEA
                PdfPCell CellRow = new PdfPCell(new Paragraph("\n\n"));
                CellRow.Colspan = 3;
                CellRow.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //COMILLAS PARA TEXTO
                const string quote = "\"";

                //IMAGEN
                PdfPCell ImageTableCell = new PdfPCell();
                PdfPTable ImageTable = new PdfPTable(3);

                string imageURL = Server.MapPath("~/Content/Images/LogoParroquia.png");
                Image img = Image.GetInstance(imageURL);
                img.ScaleAbsolute(50f, 75f);

                PdfPCell ImageCell = new PdfPCell(img);
                ImageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImageCell.HorizontalAlignment = Element.ALIGN_CENTER;

                //INFO CONTRATO
                PdfPCell TitleInfoCell = new PdfPCell();
                Paragraph TitleInfoParagraph = new Paragraph();
                if (quotation.Value == "True")
                {
                    TitleInfoParagraph.Add(new Chunk("CONTRATO No. ------------- \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.Add(new Chunk("SOLICITUD No. ------------- \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.Add(new Chunk("TIPO: PERPETUIDAD. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.SetLeading(1, 1);
                    TitleInfoCell.AddElement(TitleInfoParagraph);
                    TitleInfoCell.Colspan = 5;
                    TitleInfoCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    TitleInfoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    MainTable.AddCell(ImageCell);
                    MainTable.AddCell(WhiteCell);
                    MainTable.AddCell(TitleInfoCell);
                }
                else
                {
                    int zeros = 6 - dtoPurchasesRequest.ContractID.ToString().Length;
                    string ceros = "";
                    for (int i = 0; i < zeros; i++)
                    {
                        ceros += "0";
                    }
                    TitleInfoParagraph.Add(new Chunk("CONTRATO No. C-JC-" + ceros + dtoPurchasesRequest.ContractID + "-A/F\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.Add(new Chunk("SOLICITUD No. " + dtoPurchasesRequest.PurchasesRequestID + "\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.Add(new Chunk("TIPO: PERPETUIDAD. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.SetLeading(1, 1);
                    TitleInfoCell.AddElement(TitleInfoParagraph);
                    TitleInfoCell.Colspan = 5;
                    TitleInfoCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    TitleInfoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    MainTable.AddCell(ImageCell);
                    MainTable.AddCell(WhiteCell);
                    MainTable.AddCell(TitleInfoCell);
                }


                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PRIMER PARRAFO
                PdfPCell ParagraphCell = new PdfPCell(new Paragraph("CONTRATO DE ADHESIÓN PARA ENAJENACIÓN SOBRE PRODUCTOS Y SERVICIOS FUNERARIOS DE LOS DERECHOS DE USO DE CRIPTAS, NICHOS U OSARIOS PARA EL DEPÓSITO DE CENIZAS y RESTOS HUMANOS ÁRIDOS MORTUORIOS.",
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphCell.Colspan = 3;
                ParagraphCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ParagraphCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(ParagraphCell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //SEGUNDO PARRAFO
                PdfPCell ParagraphCell2 = new PdfPCell(new Paragraph("En el municipio de Tijuana, Baja California; siendo las ____ horas, del día ____ del mes de ______ estando presentes, las siguientes partes:",
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphCell2.Colspan = 3;
                ParagraphCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ParagraphCell2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(ParagraphCell2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TERCER PARRAFO
                PdfPCell ParagraphCell3 = new PdfPCell(new Paragraph("Por un lado “LA PARROQUIA IGLESIA DE GUADALUPE DEL RÍO EN TIJUANA A.R.” representada por C. PBRO. JESÚS CÁRDENAS NÚÑEZ a quien en lo sucesivo y para efectos de este contrato se le denominara “LA ASOCIACIÓN”.",
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphCell3.Colspan = 3;
                ParagraphCell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ParagraphCell3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(ParagraphCell3);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //CUARTO PARRAFO
                if (quotation.Value == "True")
                {
                    PdfPCell ParagraphCell4 = new PdfPCell(new Paragraph("Y por la otra parte, la (él) C. ______________________ a quien en lo sucesivo se le denominara “EL ADQUIRIENTE”; de conformidad con lo previsto en los artículos " +
                        "1679, 1680, 1681, 1682, 1683, 1685, 1690, 1699, 1700, 1701, 1702, 1703, 1704, 1705, 1706 y demás relativos y aplicables del Código Civil del Estado de Baja California vigente, conviene celebrar el presente contrato al tenor de las siguientes: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphCell4.Colspan = 3;
                    ParagraphCell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ParagraphCell4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(ParagraphCell4);
                }
                else
                {
                    PdfPCell ParagraphCell4 = new PdfPCell(new Paragraph("Y por la otra parte, la (él) C. " + dtoPurchasesRequest.CustomerName + " a quien en lo sucesivo se le denominara “EL ADQUIRIENTE”; de conformidad con lo previsto en los artículos " +
                        "1679, 1680, 1681, 1682, 1683, 1685, 1690, 1699, 1700, 1701, 1702, 1703, 1704, 1705, 1706 y demás relativos y aplicables del Código Civil del Estado de Baja California vigente, conviene celebrar el presente contrato al tenor de las siguientes: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphCell4.Colspan = 3;
                    ParagraphCell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ParagraphCell4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(ParagraphCell4);

                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECLARACIONES
                PdfPCell DeclarationTitle = new PdfPCell(new Phrase("DECLARACIONES", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                DeclarationTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                DeclarationTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                DeclarationTitle.Colspan = 3;
                MainTable.AddCell(DeclarationTitle);

                //DECLARACIONES 1
                PdfPCell DeclarationCell = new PdfPCell();
                Paragraph DeclarationParagraph = new Paragraph();

                DeclarationParagraph.Add(new Chunk("1. LA ASOCIACIÓN ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                DeclarationParagraph.Add(new Chunk("a través de su representante declara:", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                DeclarationParagraph.SetLeading(1, 1);
                DeclarationCell.AddElement(DeclarationParagraph);
                DeclarationCell.Colspan = 5;
                DeclarationCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                DeclarationCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(DeclarationCell);

                //1.1
                PdfPCell Cell1_1 = new PdfPCell();
                Paragraph Paragraph1_1 = new Paragraph();

                Paragraph1_1.Add(new Chunk("1.1.    Que es una Asociación Religiosa debidamente constituida y existente de conformidad con las leyes de los Estados Unidos Mexicanos, inscrita en la Secretaría de Gobernación con registro constitutivo No. SGAR/48:68/95 " +
                    "y que de igual forma es una división autónoma dentro de la Asociación Religiosa denominada “Diócesis de Tijuana, A.R. y cuyos estatutos han sido protocolizados en la ciudad de Tijuana, según consta en la escritura pública no. 42, 918, del 18 de febrero de 1994 del volumen 848; pág. 235, " +
                    "del protocolo de la notaría pública número 6 de la ciudad de Tijuana, a cargo de Lic. J. Eduardo Illades Moreno, que quedó debidamente inscrita en la ciudad de Tijuana",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_1.SetLeading(1, 1);
                Cell1_1.AddElement(Paragraph1_1);
                Cell1_1.Colspan = 5;
                Cell1_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.2
                PdfPCell Cell1_2 = new PdfPCell();
                Paragraph Paragraph1_2 = new Paragraph();
                Paragraph1_2.Add(new Chunk("1.2.    Que ante la Notaría No. 6, a cargo de su titular Lic. J. Eduardo Illades Moreno, con domicilio en esta ciudad de Tijuana, Baja California, se evidencia el de nombre Pbro. Lic. Jesús Cárdenas Núñez," +
                " como apoderado legal y/o representante legal de LA ASOCIACIÓN, según consta al tenor del canon 523 del Código de Derecho Canónico.",
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_2.SetLeading(1, 1);
                Cell1_2.AddElement(Paragraph1_2);
                Cell1_2.Colspan = 5;
                Cell1_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.3
                PdfPCell Cell1_3 = new PdfPCell();
                Paragraph Paragraph1_3 = new Paragraph();

                Paragraph1_3.Add(new Chunk("1.3.    Que dentro de su objeto social se encuentra, entre otros, el celebrar contratos relativos a la comercialización de productos y servicios para uso inmediato o a futuro, de los derechos de uso de espacios por tiempo determinado y/o a perpetuidad conocidos como criptas, nichos, osarios, o gavetas, para restos mortuorios " +
                    "y en específico aquellos clasificados como restos humanos áridos y cenizas de acuerdo al numeral 5, sub numerales en concordancia 5.2 y 5.2.9 de la Norma Oficial Mexicana NOM-036-SCFI-2016, Prácticas comerciales, requisitos de información y disposiciones generales en la prestación de servicios funerarios, publicada el 05 de enero del 2017 en el Diario Oficial de la Federación.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_3.SetLeading(1, 1);
                Cell1_3.AddElement(Paragraph1_3);
                Cell1_3.Colspan = 5;
                Cell1_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_3);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.4
                PdfPCell Cell1_4 = new PdfPCell();
                Paragraph Paragraph1_4 = new Paragraph();

                Paragraph1_4.Add(new Chunk("1.4.    Que de acuerdo a lo dispuesto en la Norma Oficial Mexicana NOM-036-SCFI-2016, Prácticas comerciales, requisitos de información y disposiciones generales en la prestación de servicios funerarios; cuenta con la adecuación jurídica correspondiente para los efectos, de donde se colige  y se apega a los dispuesto por la NOM citada en lo que respecta a los elementos informativos y de la publicidad, " +
                    "cumpliendo con los principios de legalidad, honestidad, veracidad, dignidad y libre competencia, entre otros.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_4.SetLeading(1, 1);
                Cell1_4.AddElement(Paragraph1_4);
                Cell1_4.Colspan = 5;
                Cell1_4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_4);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PAGINA 2

                //1.5
                PdfPCell Cell1_5 = new PdfPCell();
                Paragraph Paragraph1_5 = new Paragraph();

                Paragraph1_5.Add(new Chunk("1.5.    En concordancia legal de lo anterior, se desprende que por razones de jurisdicción territorial LA ASOCIACIÓN, cumple con lo dispuesto en su aplicabilidad con el Reglamento de Panteones en el Municipio de Tijuana, vigente para los efectos a que haya lugar, derivado del mandato expreso y facultativo hacia los municipios del país constreñido en el artículo 115 de la Constitución Política de los Estados Unidos Mexicanos.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_5.SetLeading(1, 1);
                Cell1_5.AddElement(Paragraph1_5);
                Cell1_5.Colspan = 5;
                Cell1_5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_5.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_5);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.6
                PdfPCell Cell1_6 = new PdfPCell();
                Paragraph Paragraph1_6 = new Paragraph();

                Paragraph1_6.Add(new Chunk("1.6.    Asimismo, LA ASOCIACIÓN cumple con su reglamento interior que norma las acciones inherentes a la prestación de servicios, así como toda aquella regla que le permite a LA ASOCIACIÓN responsabilizarse y lleva a cabo su operación cotidiana de forma interna que pudiera impactar en su vida interior, pero a la vez, frente a terceros (consumidores), por lo que sabedora de ello en su actuar procedió a registrar en su momento el presente contrato ante la " +
                    "Procuraduría Federal del Consumidor toda vez que para los efectos se rige, entre otras legislaciones con la Ley Federal del Consumidor en la parte que le resulta aplicativa. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_6.SetLeading(1, 1);
                Cell1_6.AddElement(Paragraph1_6);
                Cell1_6.Colspan = 5;
                Cell1_6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_6.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_6);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.7
                PdfPCell Cell1_7 = new PdfPCell();
                Paragraph Paragraph1_7 = new Paragraph();

                Paragraph1_7.Add(new Chunk("1.7.    De todo lo anteriormente se evidencia que LA ASOCIACIÓN cuenta con personalidad jurídica propia y cumple con la legislación aplicable correspondiente, atribuyéndole las facultades bastas y necesarias para celebrar el presente contrato de adhesión, de acuerdo a la base legal en los artículos 85, 86, 86 BIS, 86 TER de la Ley Federal de Protección al Consumidor. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_7.SetLeading(1, 1);
                Cell1_7.AddElement(Paragraph1_7);
                Cell1_7.Colspan = 5;
                Cell1_7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_7.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_7);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ADQUIRIENTE 2
                PdfPCell AdquirientCell = new PdfPCell();
                Paragraph AdquirientParagraph = new Paragraph();

                AdquirientParagraph.Add(new Chunk("2. EL ADQUIRIENTE ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                AdquirientParagraph.Add(new Chunk("declara:", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                AdquirientParagraph.SetLeading(1, 1);
                AdquirientCell.AddElement(AdquirientParagraph);
                AdquirientCell.Colspan = 5;
                AdquirientCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AdquirientCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(AdquirientCell);

                //2.1
                PdfPCell Cell2_1 = new PdfPCell();
                Paragraph Paragraph2_1 = new Paragraph();
                if (quotation.Value == "True")
                {
                    Paragraph2_1.Add(new Chunk("2.1.    Que es una persona física y/o moral de nombre (razón social) que cuenta con personalidad jurídica propia según consta-------------------------------------------------------------------------------------------------------------------------------------------------------- " +
                        "y se encuentra facultado para celebrar el presente contrato de adhesión.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph2_1.SetLeading(1, 1);
                    Cell2_1.AddElement(Paragraph2_1);
                    Cell2_1.Colspan = 5;
                    Cell2_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell2_1);
                }
                else
                {
                    Paragraph2_1.Add(new Chunk("2.1.    Que es una persona física y/o moral de nombre (razón social) que cuenta con personalidad jurídica propia según consta ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph2_1.Add(new Chunk(dtoPurchasesRequest.CustomerName + " ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Paragraph2_1.Add(new Chunk("y se encuentra facultado para celebrar el presente contrato de adhesión.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph2_1.SetLeading(1, 1);
                    Cell2_1.AddElement(Paragraph2_1);
                    Cell2_1.Colspan = 5;
                    Cell2_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell2_1);
                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //2.2
                PdfPCell Cell2_2 = new PdfPCell();
                Paragraph Paragraph2_2 = new Paragraph();
                if (quotation.Value == "True")
                {
                    Paragraph2_2.Add(new Chunk("2.2.    Que tiene su domicilio en ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- " +
                        "y que lo puede probar a través de documental, " +
                        "y que de lo manifestado junto con la presente declaración y la que le antecede se sirve adjuntar en paquete de documentales como ANEXO 2. (INE, CRED. USA)",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph2_2.SetLeading(1, 1);
                    Cell2_2.AddElement(Paragraph2_2);
                    Cell2_2.Colspan = 5;
                    Cell2_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell2_2);
                }
                else
                {
                    Paragraph2_2.Add(new Chunk("2.2.    Que tiene su domicilio en ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph2_2.Add(new Chunk(dtoPurchasesRequest.Address + ", No. " + dtoPurchasesRequest.AdressInteriorNumber + ", No. Interior " + dtoPurchasesRequest.AdressInteriorNumber + ", Colonia " + dtoPurchasesRequest.Neighborhood + ", C.P. " + dtoPurchasesRequest.ZipCode,
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Paragraph2_2.Add(new Chunk(" y que lo puede probar a través de documental, " +
                        "y que de lo manifestado junto con la presente declaración y la que le antecede se sirve adjuntar en paquete de documentales como ANEXO 2. (INE, CRED. USA)",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph2_2.SetLeading(1, 1);
                    Cell2_2.AddElement(Paragraph2_2);
                    Cell2_2.Colspan = 5;
                    Cell2_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell2_2);
                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //2.3
                PdfPCell Cell2_3 = new PdfPCell();
                Paragraph Paragraph2_3 = new Paragraph();
                if (quotation.Value == "True")
                {
                    Paragraph2_3.Add(new Chunk("2.3.    Que es interés de EL ADQUIRENTE celebrar el presente contrato de adhesión con LA ASOCIACIÓN, haciéndose el primero acreedor, previa contraprestación pecuniaria pactada en numerario, según lo estipulado en el ANEXO 3 denominado: " +
                        "CONDICIONES COMERCIALES, de una cripta y/o nicho que se localiza en la PARROQUIA IGLESIA GUADALUPE DEL RÍO, EN TIJUANA A.R, ubicada en Av. Paseo Centenario No. 10150, Zona Río, Tijuana, Baja California, México, Código Postal 2210, ubicada en el ÁREA ------ ZONA --------- UBICACIÓN ---- NICHO No. --------identificado con la clave No. ------------------------------.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph2_3.SetLeading(1, 1);
                    Cell2_3.AddElement(Paragraph2_3);
                    Cell2_3.Colspan = 5;
                    Cell2_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2_3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell2_3);
                }
                else
                {
                    string first4 = dtoPurchasesRequest.Nicho.Substring(0, 4);
                    string fiveTo7 = dtoPurchasesRequest.Nicho.Substring(4, 3);
                    string last3 = dtoPurchasesRequest.Nicho.Substring(8, 3);

                    Paragraph2_3.Add(new Chunk("2.3.    Que es interés de EL ADQUIRENTE celebrar el presente contrato de adhesión con LA ASOCIACIÓN, haciéndose el primero acreedor, previa contraprestación pecuniaria pactada en numerario, según lo estipulado en el ANEXO 3 denominado: " +
                        "CONDICIONES COMERCIALES, de una cripta y/o nicho que se localiza en la PARROQUIA IGLESIA GUADALUPE DEL RÍO, EN TIJUANA A.R, ubicada en Av. Paseo Centenario No. 10150, Zona Río, Tijuana, Baja California, México, Código Postal 2210, ubicada en el ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph2_3.Add(new Chunk("ÁREA " + first4 + " ZONA " + fiveTo7 + " UBICACIÓN " + dtoPurchasesRequest.Nicho[7] + " NICHO No. " + last3 + " identificado con la clave No. " + dtoPurchasesRequest.Nicho + ".",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Paragraph2_3.SetLeading(1, 1);
                    Cell2_3.AddElement(Paragraph2_3);
                    Cell2_3.Colspan = 5;
                    Cell2_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2_3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell2_3);
                }

                //2.4 (3)
                PdfPCell Cell2_4 = new PdfPCell();
                Paragraph Paragraph2_4 = new Paragraph();

                Paragraph2_4.Add(new Chunk("3.  Las partes declaran: \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph2_4.Add(new Chunk("Que es su interés sujetarse a lo estipulado en el presente CONTRATO DE ADHESIÓN PARA ENAJENACIÓN en el más amplio sentido del derecho y conforme a sus declaraciones, cláusulas y anexos que conforman de forma integral el presente instrumento jurídico.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph2_4.SetLeading(1, 1);
                Cell2_4.AddElement(Paragraph2_4);
                Cell2_4.Colspan = 5;
                Cell2_4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell2_4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell2_4);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                PdfPCell Cell2_5 = new PdfPCell();
                Paragraph Paragraph2_5 = new Paragraph();

                Paragraph2_5.Add(new Chunk("Dadas las anteriores declaraciones, las partes otorgan lo contenido en las siguientes ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph2_5.SetLeading(1, 1);
                Cell2_5.AddElement(Paragraph2_5);
                Cell2_5.Colspan = 5;
                Cell2_5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell2_5.HorizontalAlignment = Element.ALIGN_CENTER;
                MainTable.AddCell(Cell2_5);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //CLÁUSULAS
                PdfPCell ConclusionsTitle = new PdfPCell(new Phrase("CLÁUSULAS", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ConclusionsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ConclusionsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ConclusionsTitle.Colspan = 3;
                MainTable.AddCell(ConclusionsTitle);

                //1
                PdfPCell Cell1 = new PdfPCell();
                Paragraph Paragraph1 = new Paragraph();

                Paragraph1.Add(new Chunk("1.    DEFINICIONES \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph1.Add(new Chunk("En el presente contrato, cuando el contexto lo permita, se sujetará textualmente a las definiciones contenidas en la Norma Oficial Mexicana NOM-036-SCFI-2016, Prácticas comerciales, requisitos de información y disposiciones generales en la prestación de servicios funerarios, publicada el 05 de enero del 2017 en el Diario Oficial de la Federación " +
                    "y los criterios legales procurados por la Ley Federal de Protección al Consumidor.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1.SetLeading(1, 1);
                Cell1.AddElement(Paragraph1);
                Cell1.Colspan = 5;
                Cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //Pagina 3

                //2
                PdfPCell Cell2 = new PdfPCell();
                Paragraph Paragraph2 = new Paragraph();

                Paragraph2.Add(new Chunk("2.    RECONOCIMIENTO DE EL ADQUIRENTE. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph2.Add(new Chunk("LA ASOCIACIÓN reconoce a EL ADQUIRENTE como una persona de buena fe y de buenas costumbres en un proceder lícito y apegado a la verdad en su actuar, a quien, en este acto, para efectos de hacer evidente ello, EL ADQUIRENTE manifiesta que se dedica o dedicado (oficio y/o profesión) de acuerdo al derecho humano que le confiere el artículo 5to. De la Constitución Política de los Estados Unidos Mexicanos. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph2.SetLeading(1, 1);
                Cell2.AddElement(Paragraph2);
                Cell2.Colspan = 5;
                Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //3
                PdfPCell Cell3 = new PdfPCell();
                Paragraph Paragraph3 = new Paragraph();

                Paragraph3.Add(new Chunk("3.    DERECHOS Y OBLIGACIONES DE LA ASOCIACIÓN. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk("3.1.  LA ASOCIACIÓN podrá:",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.SetLeading(1, 1);
                Cell3.AddElement(Paragraph3);
                Cell3.Colspan = 5;
                Cell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell3);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //3.1.1
                PdfPCell Cell3_1_1 = new PdfPCell();
                Paragraph Paragraph3_1_1 = new Paragraph();

                Paragraph3_1_1.Add(new Chunk("3.1.1.    LA ASOCIACIÓN cede y EL ADQUIRENTE obtiene el derecho de uso mortuorio del nicho a PERPETUIDAD en la PARROQUIA IGLESIA GUADALUPE DEL RIO, EN TIJUANA, A.R.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3_1_1.SetLeading(1, 1);
                Cell3_1_1.AddElement(Paragraph3_1_1);
                Cell3_1_1.Colspan = 5;
                Cell3_1_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell3_1_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell3_1_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //3.1.2
                PdfPCell Cell3_1_2 = new PdfPCell();
                Paragraph Paragraph3_1_2 = new Paragraph();

                Paragraph3_1_2.Add(new Chunk("3.1.2.    LA ASOCIACIÓN se reserva el derecho de verificar el contenido de las urnas a depositar, con el propósito de verificar precisamente que su contenido sea el de restos humanos áridos, secos e incinerados.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3_1_2.SetLeading(1, 1);
                Cell3_1_2.AddElement(Paragraph3_1_2);
                Cell3_1_2.Colspan = 5;
                Cell3_1_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell3_1_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell3_1_2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                if (sPaymentMethod != "Contado")
                {
                    //3.1.3
                    PdfPCell Cell3_1_3 = new PdfPCell();
                    Paragraph Paragraph3_1_3 = new Paragraph();

                    Paragraph3_1_3.Add(new Chunk("3.1.3.    LA ASOCIACIÓN puede y debe recibir pagos anticipados por parte de EL ADQUIRENTE, de acuerdo a lo estipulado en el ANEXO 3, pero a la vez LA ASOCIACIÓN exigirá EL ADQUIRENTE el cumplimiento de sus obligaciones de pago asumidas por éste, " +
                        "en el entendido que a falta de tres (3) mensualidades (comercialización en pagos o abonos) de forma ininterrumpida y subsecuentes, SIN QUE MEDIE RESOLUCIÓN ADMINISTRATIVA O JUDICIAL ( JURISIDICCIONAL) LA ASOCIACIÓN dará por rescindido y terminado de manera anticipada el presente contrato de adhesión, " +
                        "al día siguiente de haberse cumplimentado las tres mensualidades adeudadas, de donde se desprende que NO HABRÁ DEVOLUCIÓN ALGUNA DE LO YA PAGADO A LA ASOCIACIÓN.",
                     new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph3_1_3.SetLeading(1, 1);
                    Cell3_1_3.AddElement(Paragraph3_1_3);
                    Cell3_1_3.Colspan = 5;
                    Cell3_1_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell3_1_3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell3_1_3);

                    //SALTO DE LINEA
                    MainTable.AddCell(CellRow);

                    //3.1.4
                    PdfPCell Cell3_1_4 = new PdfPCell();
                    Paragraph Paragraph3_1_4 = new Paragraph();

                    Paragraph3_1_4.Add(new Chunk("3.1.4.    De lo anterior se colige que, si dentro del plazo en que llegase a acumularse dichas tres mensualidades, a cada vencimiento de mes no pagado se le gravará un ",
                     new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph3_1_4.Add(new Chunk("3.5% como mora sobre el valor total de la mensualidad atrasada. ",
                     new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Paragraph3_1_4.SetLeading(1, 1);
                    Cell3_1_4.AddElement(Paragraph3_1_4);
                    Cell3_1_4.Colspan = 5;
                    Cell3_1_4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell3_1_4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell3_1_4);

                    //SALTO DE LINEA
                    MainTable.AddCell(CellRow);

                    //3.1.5
                    PdfPCell Cell3_1_5 = new PdfPCell();
                    Paragraph Paragraph3_1_5 = new Paragraph();

                    Paragraph3_1_5.Add(new Chunk("3.1.5.	De cumplimentarse lo referido en el sub numeral 3.1.4. LA ASOCIACIÓN notificará por correo electrónico o por vía telefónica a EL ADQUIRENTE, siempre y cuando éste se encuentre en el municipio de Tijuana, Baja California, " +
                        "por lo que si se encuentra fuera de las ciudad de Tijuana el domicilio de EL ADQUIRENTE, LA ASOCIACIÓN utilizará las modalidades de notificación con antelación descritas, por lo que bastará que LA ASOCIACIÓN anote y asiente en un libro de actas interno ello, " +
                        "para que de forma inmediata el nicho o nichos que EL ADQUIRENTE dejó de cumplimentar en sus pagos y por lo tanto dieron pie a la recisión contractual y terminación anticipada de lo pactado, regresen a formar parte del uso en el patrimonio de LA ASOCIACIÓN, " +
                        "pudiendo la misma ofertarla para su comercialización sin mayor trámite y sin que MEDIE RESOLUCIÓN ADMINISTRATIVA y/o JUDICIAL.",
                     new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph3_1_5.SetLeading(1, 1);
                    Cell3_1_5.AddElement(Paragraph3_1_5);
                    Cell3_1_5.Colspan = 5;
                    Cell3_1_5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell3_1_5.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell3_1_5);

                    //SALTO DE LINEA
                    MainTable.AddCell(CellRow);
                }

                //3.1.6
                PdfPCell Cell3_1_6 = new PdfPCell();
                Paragraph Paragraph3_1_6 = new Paragraph();

                Paragraph3_1_6.Add(new Chunk("3.1.6.	Con el propósito de proporcionar el servicio de depósito de urnas en los nichos, LA ASOCIACIÓN dispone de nichos que forman parte de la zona central o área atrial del columbario que se encuentra en el predio de LA ASOCIACIÓN, " +
                    "mismas que podrán ser utilizadas por el ADQUIRENTE provisionalmente si aún no ha sido construido el nicho y/o la cripta adquirida, es decir, está en lo futuro sujeto a la concreción de su construcción y habilitación para tal efecto.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3_1_6.SetLeading(1, 1);
                Cell3_1_6.AddElement(Paragraph3_1_6);
                Cell3_1_6.Colspan = 5;
                Cell3_1_6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell3_1_6.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell3_1_6);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);

                //PAGINA 4

                //4
                PdfPCell Cell4 = new PdfPCell();
                Paragraph Paragraph4 = new Paragraph();

                Paragraph4.Add(new Chunk("4.    DERECHOS Y OBLIGACIONES DE EL ADQUIRENTE. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph4.Add(new Chunk("4.1.  EL ADQUIRENTE se hace sabedor y:",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4.SetLeading(1, 1);
                Cell4.AddElement(Paragraph4);
                Cell4.Colspan = 5;
                Cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //4.1.1
                PdfPCell Cell4_1_1 = new PdfPCell();
                Paragraph Paragraph4_1_1 = new Paragraph();

                Paragraph4_1_1.Add(new Chunk("4.1.1.    Manifiesta de manera expresa que conoce el proyecto de la construcción de nichos y está de acuerdo en la celebración del presente contrato de adhesión, " +
                    "aun cuando la ejecución de tal proyecto no haya sido del todo concluida.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_1.SetLeading(1, 1);
                Cell4_1_1.AddElement(Paragraph4_1_1);
                Cell4_1_1.Colspan = 5;
                Cell4_1_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //4.1.2
                PdfPCell Cell4_1_2 = new PdfPCell();
                Paragraph Paragraph4_1_2 = new Paragraph();

                Paragraph4_1_2.Add(new Chunk("4.1.2.    Manifiesta que ha seleccionado el nicho o la cripta que ampara el derecho de uso mortuorio a PERPETUIDAD objeto de este contrato y la ha identificado plenamente de acuerdo a la memoria descriptiva de la cual, " +
                    "EL ADQUIRENTE afirma haber observado el plano de la distribución donde se localiza el nicho en referencia y que corresponde con la selección que éste hizo, de donde se desprende que la dimensión del nicho y/o cripta que se ha ofertado tiene una dimensión de " +
                    "50 centímetros x 40 centímetros x 55 centímetros EXTERIOR ( ancho X alto X fondo). En este nicho podrán ser depositadas hasta cuatro (4) urnas de dimensiones estándares, con las siguientes medidas no mayor a 30 centímetros x 15 centímetros x 20 centímetros (alto X ancho X fondo).",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_2.SetLeading(1, 1);
                Cell4_1_2.AddElement(Paragraph4_1_2);
                Cell4_1_2.Colspan = 5;
                Cell4_1_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //4.1.3
                PdfPCell Cell4_1_3 = new PdfPCell();
                Paragraph Paragraph4_1_3 = new Paragraph();

                Paragraph4_1_3.Add(new Chunk("4.1.3.    Manifiesta y se hace sabedor que los ",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_3.Add(new Chunk("precios son en pesos mexicanos, moneda nacional, o su equivalente en dólar al tipo de cambio que impere y rija el Banco de México al día en que se realice el pago o pagos correspondientes, ",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph4_1_3.Add(new Chunk("debiéndose hacer dentro de los tres primeros días hábiles a la fecha de vencimiento.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_3.SetLeading(1, 1);
                Cell4_1_3.AddElement(Paragraph4_1_3);
                Cell4_1_3.Colspan = 5;
                Cell4_1_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_3);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //4.1.4
                PdfPCell Cell4_1_4 = new PdfPCell();
                Paragraph Paragraph4_1_4 = new Paragraph();

                Paragraph4_1_4.Add(new Chunk("4.1.4.    Se hace sabedor a fin de cumplimentar ante LA ASOCIACIÓN lo descrito en la cláusula 3 (TERCERA) del presente contrato de adhesión.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_4.SetLeading(1, 1);
                Cell4_1_4.AddElement(Paragraph4_1_4);
                Cell4_1_4.Colspan = 5;
                Cell4_1_4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_4);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //4.1.5
                PdfPCell Cell4_1_5 = new PdfPCell();
                Paragraph Paragraph4_1_5 = new Paragraph();

                Paragraph4_1_5.Add(new Chunk("4.1.5.    EL ADQUIRENTE está de acuerdo y manifiesta su conformidad en torno a la colocación y/o extracción de urnas, sea función única y exclusivamente por el personal autorizado por LA ASOCIACIÓN " +
                    "previa solicitud el ADQUIRIENTE debe presentar el contrato y el último recibo de pago, que acredite que ha cubierto la totalidad del valor de la cripta y/o nicho.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_5.SetLeading(1, 1);
                Cell4_1_5.AddElement(Paragraph4_1_5);
                Cell4_1_5.Colspan = 5;
                Cell4_1_5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_5.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_5);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //4.1.6
                PdfPCell Cell4_1_6 = new PdfPCell();
                Paragraph Paragraph4_1_6 = new Paragraph();

                Paragraph4_1_6.Add(new Chunk("4.1.6.    EL ADQUIRENTE no podrá ceder parcial ni totalmente los derechos que obtiene y ejerce en el presente contrato, salvo la autorización expresa y por escrito de LA ASOCIACIÓN.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_6.SetLeading(1, 1);
                Cell4_1_6.AddElement(Paragraph4_1_6);
                Cell4_1_6.Colspan = 5;
                Cell4_1_6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_6.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_6);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //4.1.7
                PdfPCell Cell4_1_7 = new PdfPCell();
                Paragraph Paragraph4_1_7 = new Paragraph();

                Paragraph4_1_7.Add(new Chunk("4.1.7.    Se compromete a dar aviso a LA ASOCIACIÓN de su cambio de domicilio dentro de un plazo que no excederá de quince (15) días naturales a partir de la fecha en que ocurra, con acuse de recibo por parte de LA ASOCIACIÓN, así como en cada ocasión que EL ADQUIRENTE cambié de domicilio. " +
                    "El incumplimiento a la presente cláusula por parte de EL ADQUIRENTE, liberará a LA ASOCIACIÓN de sus obligaciones inherentes al presente contrato y será causal de rescisión del presente contrato de adhesión con responsabilidad para EL ADQUIRENTE, en el entendido que la presente cláusula no aplicará si el pago total " +
                    "sobre el precio pactado de la operación se haya o se encuentra liquidado.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_7.SetLeading(1, 1);
                Cell4_1_7.AddElement(Paragraph4_1_7);
                Cell4_1_7.Colspan = 5;
                Cell4_1_7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_7.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_7);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //4.1.8
                PdfPCell Cell4_1_8 = new PdfPCell();
                Paragraph Paragraph4_1_8 = new Paragraph();

                Paragraph4_1_8.Add(new Chunk("4.1.8.    EL ADQUIRENTE faculta de manera expresa a LA ASOCIACIÓN para ceder, descontar o negociar, ante cualquier Institución de Crédito o empresa, sea del país (México) o del extranjero, el o los derechos suscritos al amparo de este contrato de adhesión.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_8.SetLeading(1, 1);
                Cell4_1_8.AddElement(Paragraph4_1_8);
                Cell4_1_8.Colspan = 5;
                Cell4_1_8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_8.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_8);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PAGINA 5

                //4.1.9
                tblMaintenanceFee tblMaintenance = MaintenanceFeeB.GetLastReccord(1);
                string maintenanceFee = ConvertirNumeroALetras(tblMaintenance.Cost);
                PdfPCell Cell4_1_9 = new PdfPCell();
                Paragraph Paragraph4_1_9 = new Paragraph();

                Paragraph4_1_9.Add(new Chunk("4.1.9.    El adquiriente está de acuerdo en realizar un pago anual por concepto de CUOTA DE MANTENIMIENTO, a partir de la fecha de compra, por la cantidad de " + tblMaintenance.Cost.ToString("C2") + " (" + maintenanceFee + ") por tiempo indefinido, con el propósito de garantizar el mantenimiento vitalicio de la totalidad del área de nichos.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4_1_9.SetLeading(1, 1);
                Cell4_1_9.AddElement(Paragraph4_1_9);
                Cell4_1_9.Colspan = 5;
                Cell4_1_9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell4_1_9.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell4_1_9);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //5
                PdfPCell Cell5 = new PdfPCell();
                Paragraph Paragraph5 = new Paragraph();

                Paragraph5.Add(new Chunk("5. TITULAR Y BENEFICIARIOS. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph5.SetLeading(1, 1);
                Cell5.AddElement(Paragraph5);
                Cell5.Colspan = 5;
                Cell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell5.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell5);

                //5.1.
                PdfPCell Cell5_1 = new PdfPCell();
                Paragraph Paragraph5_1 = new Paragraph();

                Paragraph5_1.Add(new Chunk("5.1.    EL ADQUIRENTE en su calidad de TITULAR designa a los siguientes beneficiarios, manifestando que la relación con el adquirente es en primer grado ascendente (padres), descendente (hijos) y lateral (esposa), o concubina o en su defecto podrá, sin ningún impedimento, designar de beneficiario (a), aquella persona que sin tener parentesco con el TITULAR sea la voluntad manifiesta simple y expresa de éste para inscribirlo desde el concepto de una relación de amistad.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph5_1.SetLeading(1, 1);
                Cell5_1.AddElement(Paragraph5_1);
                Cell5_1.Colspan = 5;
                Cell5_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell5_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell5_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //5.2.
                PdfPCell Cell5_2 = new PdfPCell();
                Paragraph Paragraph5_2 = new Paragraph();

                Paragraph5_2.Add(new Chunk("5.2.    Los beneficiarios podrán ser sustituidos a voluntad de EL ADQUIRENTE (Titular) previo aviso por escrito a LA ASOCIACIÓN con acuse de recibo, así como los beneficiarios que no sean descendientes o ascendientes directos al TITULAR.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph5_2.SetLeading(1, 1);
                Cell5_2.AddElement(Paragraph5_2);
                Cell5_2.Colspan = 5;
                Cell5_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell5_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell5_2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //5.3.
                PdfPCell Cell5_3 = new PdfPCell();
                Paragraph Paragraph5_3 = new Paragraph();

                Paragraph5_3.Add(new Chunk("5.3.    Será facultad única y exclusiva de EL ADQUIRENTE decidir extraer alguna de las urnas depositadas en la cripta y/o nicho adquirido, y esto diera lugar a ocupar ese espacio, éste podrá utilizarse de acuerdo a la petición que por escrito respectivo realice EL ADQUIRENTE a LA ASOCIACIÓN, quien tendrá, ésta última, la facultad de autorizar o no tal movimiento. Esta facultad NO es transferible a ninguno de los beneficiarios. ",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph5_3.SetLeading(1, 1);
                Cell5_3.AddElement(Paragraph5_3);
                Cell5_3.Colspan = 5;
                Cell5_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell5_3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell5_3);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //5.4.
                PdfPCell Cell5_4 = new PdfPCell();
                Paragraph Paragraph5_4 = new Paragraph();

                Paragraph5_4.Add(new Chunk("5.4.    EL ADQUIRENTE está de acuerdo que por discapacidad mental diagnosticada por profesional médico o estado de interdicción decretada por juez competente o muerte cerebral o evidencia de estado de coma o muerte (fallecimiento), sean cedidos al primer beneficiario, y así sucesivamente, siempre y cuando en testamento inscrito ante notario público se determine a otro para ello. Los derechos no son aplicables al concepto de sustitución " +
                    "y extracción de urnas, por lo que será en todo momento la voluntad que por escrito haya presentado EL ADQUIRENTE a LA ASOCIACIÓN.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph5_4.SetLeading(1, 1);
                Cell5_4.AddElement(Paragraph5_4);
                Cell5_4.Colspan = 5;
                Cell5_4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell5_4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell5_4);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //5.5.
                PdfPCell Cell5_5 = new PdfPCell();
                Paragraph Paragraph5_5 = new Paragraph();
                if (quotation.Value == "True")
                {
                    Paragraph5_5.Add(new Chunk("5.5.    EL ADQUIRENTE (TITULAR) DESIGNA A LOS SIGUIENTES BENEFICIARIOS): \n",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Paragraph5_5.SetLeading(1, 1);
                    Cell5_5.AddElement(Paragraph5_5);
                    Cell5_5.Colspan = 5;
                    Cell5_5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell5_5.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell5_5);

                    //TABLA DE BENEFICIARIOS
                    PdfPTable MiniTable = new PdfPTable(4);

                    // Estilo de la tabla
                    MiniTable.TotalWidth = 550;
                    MiniTable.LockedWidth = true;
                    MiniTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    MiniTable.WidthPercentage = 100;
                    float[] columnWidths = new float[] { 240, 100, 100, 120 };
                    MiniTable.SetWidths(columnWidths);

                    // Estilo de las celdas de la tabla
                    PdfPCell cellStyle = new PdfPCell();
                    cellStyle.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellStyle.Padding = 5;
                    cellStyle.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cellStyle.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    cellStyle.BorderColor = BaseColor.BLACK;
                    cellStyle.BorderWidth = 0.5f;

                    // Agregar títulos a la tabla con el estilo
                    MiniTable.AddCell(new PdfPCell(new Phrase("Nombre y apellidos", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                    MiniTable.AddCell(new PdfPCell(new Phrase("Fecha de nac.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                    MiniTable.AddCell(new PdfPCell(new Phrase("Parentesco", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                    MiniTable.AddCell(new PdfPCell(new Phrase("Número de teléfono", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));

                    // Llenar la tabla con la información de los beneficiarios
                    MiniTable.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL))));
                    MiniTable.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL))));
                    MiniTable.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL))));
                    MiniTable.AddCell(new PdfPCell(new Phrase(" ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL))));

                    PdfPCell MiniTableCell = new PdfPCell();
                    MiniTableCell.Border = PdfPCell.NO_BORDER;
                    MiniTableCell.Colspan = 4;
                    MiniTableCell.AddElement(MiniTable);
                    MainTable.AddCell(MiniTableCell);
                }
                else
                {
                    Paragraph5_5.Add(new Chunk("5.5.    EL ADQUIRENTE (TITULAR) DESIGNA A LOS SIGUIENTES BENEFICIARIOS): \n",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Paragraph5_5.SetLeading(1, 1);
                    Cell5_5.AddElement(Paragraph5_5);
                    Cell5_5.Colspan = 5;
                    Cell5_5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell5_5.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell5_5);

                    //TABLA DE BENEFICIARIOS
                    PdfPTable MiniTable = new PdfPTable(4);

                    // Estilo de la tabla
                    MiniTable.TotalWidth = 550;
                    MiniTable.LockedWidth = true;
                    MiniTable.HorizontalAlignment = Element.ALIGN_CENTER;
                    MiniTable.WidthPercentage = 100;
                    float[] columnWidths = new float[] { 230, 110, 100, 110 };
                    MiniTable.SetWidths(columnWidths);

                    // Estilo de las celdas de la tabla
                    PdfPCell cellStyle = new PdfPCell();
                    cellStyle.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellStyle.Padding = 5;
                    cellStyle.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cellStyle.Border = PdfPCell.BOTTOM_BORDER | PdfPCell.TOP_BORDER;
                    cellStyle.BorderColor = BaseColor.BLACK;
                    cellStyle.BorderWidth = 0.5f;

                    // Agregar títulos a la tabla con el estilo
                    MiniTable.AddCell(new PdfPCell(new Phrase("Nombre y apellidos", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                    MiniTable.AddCell(new PdfPCell(new Phrase("Fecha de nac.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                    MiniTable.AddCell(new PdfPCell(new Phrase("Parentesco", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                    MiniTable.AddCell(new PdfPCell(new Phrase("Número de teléfono", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));


                    var model = BeneficiaryCustomersB.GetListByPurchasesRequestID(PurchaseID);
                    // Llenar la tabla con la información de los beneficiarios
                    foreach (var item in model)
                    {
                        string formatedDate = item.Birthdate.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-ES")).ToUpper();
                        MiniTable.AddCell(new PdfPCell(new Phrase(item.Name + " " + item.Surnames, new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                        MiniTable.AddCell(new PdfPCell(new Phrase(formatedDate, new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                        MiniTable.AddCell(new PdfPCell(new Phrase(item.Relationship, new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                        MiniTable.AddCell(new PdfPCell(new Phrase(item.CelPhone, new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD))));
                    }

                    PdfPCell MiniTableCell = new PdfPCell();
                    MiniTableCell.Border = PdfPCell.NO_BORDER;
                    MiniTableCell.Colspan = 4;
                    MiniTableCell.AddElement(MiniTable);
                    MainTable.AddCell(MiniTableCell);
                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //6
                PdfPCell Cell6 = new PdfPCell();
                Paragraph Paragraph6 = new Paragraph();

                Paragraph6.Add(new Chunk("6.    DEPÓSITO DE URNAS. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph6.SetLeading(1, 1);
                Cell6.AddElement(Paragraph6);
                Cell6.Colspan = 5;
                Cell6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell6.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell6);

                //6.1.
                PdfPCell Cell6_1 = new PdfPCell();
                Paragraph Paragraph6_1 = new Paragraph();

                Paragraph6_1.Add(new Chunk("6.1.    Las URNAS deberán contener cenizas y/o restos humanos áridos mortuorios, secos e incinerados.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph6_1.SetLeading(1, 1);
                Cell6_1.AddElement(Paragraph6_1);
                Cell6_1.Colspan = 5;
                Cell6_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell6_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell6_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //6.2.
                PdfPCell Cell6_2 = new PdfPCell();
                Paragraph Paragraph6_2 = new Paragraph();

                Paragraph6_2.Add(new Chunk("6.2.    EL ADQUIRENTE (Titular) o beneficiarios deberán presentar el acta de defunción, el comprobante de cremación de los restos mortuorios expedido por la funeraria, cuando menos dentro de las 24 horas del día anterior a la fecha en que se requiera el uso de la cripta y/o nicho. ",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph6_2.SetLeading(1, 1);
                Cell6_2.AddElement(Paragraph6_2);
                Cell6_2.Colspan = 5;
                Cell6_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell6_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell6_2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //6.3.
                PdfPCell Cell6_3 = new PdfPCell();
                Paragraph Paragraph6_3 = new Paragraph();

                Paragraph6_3.Add(new Chunk("6.3.    Mostrar el presente contrato de adhesión, así como el último recibo de pago que finiquitó la contraprestación pactada.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph6_3.SetLeading(1, 1);
                Cell6_3.AddElement(Paragraph6_3);
                Cell6_3.Colspan = 5;
                Cell6_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell6_3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell6_3);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PAGINA 6

                //6.4.
                PdfPCell Cell6_4 = new PdfPCell();
                Paragraph Paragraph6_4 = new Paragraph();

                Paragraph6_4.Add(new Chunk("6.4.    LA ASOCIACIÓN proporcionará el REGLAMENTO INTERNO del caso en comento correspondiente a EL ADQUIRENTE a fin de que éste lo observe y cumplimente en todo momento y que se adjunta al presente instrumento jurídico como ANEXO 4.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph6_4.SetLeading(1, 1);
                Cell6_4.AddElement(Paragraph6_4);
                Cell6_4.Colspan = 5;
                Cell6_4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell6_4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell6_4);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //7
                PdfPCell Cell7 = new PdfPCell();
                Paragraph Paragraph7 = new Paragraph();

                Paragraph7.Add(new Chunk("RESICIÓN CONTRACTUAL Y TERMINACIÓN ANTICIPADA. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph7.SetLeading(1, 1);
                Cell7.AddElement(Paragraph7);
                Cell7.Colspan = 5;
                Cell7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell7.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell7);

                //7.1.
                PdfPCell Cell7_1 = new PdfPCell();
                Paragraph Paragraph7_1 = new Paragraph();

                Paragraph7_1.Add(new Chunk("7.1.    Sumado a los hipotéticos jurídicos aquí planteados para los casos en que se pueda presentar la recisión contractual y terminación anticipada del presente contrato de adhesión, se encuentran en plena concordancia y correlación aquellos inscritos que para su consulta se focalizan en el ANEXO 3, " +
                    "a efecto de que las partes se remitan al mismo para mejor comprensión.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph7_1.SetLeading(1, 1);
                Cell7_1.AddElement(Paragraph7_1);
                Cell7_1.Colspan = 5;
                Cell7_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell7_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell7_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //8
                PdfPCell Cell8 = new PdfPCell();
                Paragraph Paragraph8 = new Paragraph();

                Paragraph8.Add(new Chunk("8. NOTIFICACIONES. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph8.SetLeading(1, 1);
                Cell8.AddElement(Paragraph8);
                Cell8.Colspan = 5;
                Cell8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell8.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell8);

                //8.1.
                PdfPCell Cell8_1 = new PdfPCell();
                Paragraph Paragraph8_1 = new Paragraph();

                Paragraph8_1.Add(new Chunk(" Las partes convienen que toda notificación y demás comunicaciones que sean requeridas o que se deseen dar conforme a este contrato, deberán ser de acuerdo al numeral 3.1.5 descrito con anterioridad en el presente contrato de adhesión.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8_1.SetLeading(1, 1);
                Cell8_1.AddElement(Paragraph8_1);
                Cell8_1.Colspan = 5;
                Cell8_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell8_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell8_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //9
                PdfPCell Cell9 = new PdfPCell();
                Paragraph Paragraph9 = new Paragraph();

                Paragraph9.Add(new Chunk("9. JURISDICCIÓN Y LEGISLACIÓN APLICABLE \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph9.SetLeading(1, 1);
                Cell9.AddElement(Paragraph9);
                Cell9.Colspan = 5;
                Cell9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell9.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell9);

                //9.1.
                PdfPCell Cell9_1 = new PdfPCell();
                Paragraph Paragraph9_1 = new Paragraph();

                Paragraph9_1.Add(new Chunk("Las partes convienen en que este contrato es de naturaleza mercantil y que el mismo se regirá y se interpretará de conformidad con las leyes de los Estados Unidos Mexicanos. \n" +
                    "Para todo lo relacionado con la interpretación y cumplimiento del presente contrato, las partes se someten a la jurisdicción y competencia de los tribunales de la Ciudad de Tijuana, Baja California, México, expresamente renunciando a cualquier otro fuero que pudiera llegar a corresponderles por razón de sus domicilios presentes o futuros o por cualquier otra causa.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9_1.SetLeading(1, 1);
                Cell9_1.AddElement(Paragraph9_1);
                Cell9_1.Colspan = 5;
                Cell9_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell9_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell9_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //10
                PdfPCell Cell10 = new PdfPCell();
                Paragraph Paragraph10 = new Paragraph();

                Paragraph10.Add(new Chunk("10.  PROTECCIÓN DE DATOS PERSONALES y AVISO DE PRIVACIDAD. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph10.SetLeading(1, 1);
                Cell10.AddElement(Paragraph10);
                Cell10.Colspan = 5;
                Cell10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell10.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell10);

                //10.1.
                PdfPCell Cell10_1 = new PdfPCell();
                Paragraph Paragraph10_1 = new Paragraph();

                Paragraph10_1.Add(new Chunk("Este Contrato se celebra entre contratistas independientes, sobre una base de igualdad, por lo que " +
                    "LA ASOCIACIÓN informa que el tratamiento de sus datos personales que proporciona se lleva a cabo y se cumplimenta a través de las disposiciones de la " +
                    "Ley Federal de Protección de Datos Personales en Posesión de Particulares (LFPDPPP) y demás legislación aplicable vigente en los Estados Unidos Mexicanos. \n" +
                    "Ambas partes manifiestan y reconocen tener la capacidad jurídica para celebrar el presente contrato, en términos del artículo 1679 del Código Civil del Estado de Baja California vigente. \n " +
                    "Es objeto del presente contrato, en términos de los artículos 1711, 1712, 1713, 1714 del Código Civil referido, el contrato de adhesión, citado en título del presente documento, con las características y particularidades señaladas.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph10_1.SetLeading(1, 1);
                Cell10_1.AddElement(Paragraph10_1);
                Cell10_1.Colspan = 5;
                Cell10_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell10_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell10_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //10.1.
                PdfPCell Cell10_2 = new PdfPCell();
                Paragraph Paragraph10_2 = new Paragraph();
                if (quotation.Value == "True")
                {
                    Paragraph10_2.Add(new Chunk("EN TESTIMONIO DE LO ANTERIOR, las partes del presente Contrato, lo suscriben por conducto de sus representantes, debidamente autorizados para ello, el ___ de _________ del año 20---- en la ciudad de Tijuana, Baja California, México.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph10_2.SetLeading(1, 1);
                    Cell10_2.AddElement(Paragraph10_2);
                    Cell10_2.Colspan = 5;
                    Cell10_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell10_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell10_2);
                }
                else
                {
                    Paragraph10_2.Add(new Chunk("EN TESTIMONIO DE LO ANTERIOR, las partes del presente Contrato, lo suscriben por conducto de sus representantes, debidamente autorizados para ello, " + dtoPurchasesRequest.CreatedDate.ToString("'el 'dd' de 'MMMM' del año 'yyyy") + " en la ciudad de Tijuana, Baja California, México.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Paragraph10_2.SetLeading(1, 1);
                    Cell10_2.AddElement(Paragraph10_2);
                    Cell10_2.Colspan = 5;
                    Cell10_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell10_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Cell10_2);
                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                PdfPCell MiddleSignatureCell = new PdfPCell();
                PdfPTable MiddleSignatureTable = new PdfPTable(2);

                Paragraph ParagraphMiddleSing1 = new Paragraph();
                Paragraph ParagraphMiddleSing2 = new Paragraph();

                ParagraphMiddleSing1.Add(new Chunk("____________________________ \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphMiddleSing1.Add(new Chunk("“LA ASOCIACIÓN”  \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphMiddleSing1.Add(new Chunk("Representante legal \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));

                ParagraphMiddleSing2.Add(new Chunk("____________________________ \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphMiddleSing2.Add(new Chunk("EL ADQUIRIENTE   \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));

                PdfPCell SignCell1 = new PdfPCell();
                PdfPCell SignCell2 = new PdfPCell();

                SignCell1.AddElement(ParagraphMiddleSing1);
                SignCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SignCell1.HorizontalAlignment = Element.ALIGN_CENTER;

                SignCell2.AddElement(ParagraphMiddleSing2);
                SignCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SignCell2.HorizontalAlignment = Element.ALIGN_CENTER;

                MiddleSignatureTable.AddCell(SignCell1);
                MiddleSignatureTable.AddCell(SignCell2);

                MiddleSignatureCell.AddElement(MiddleSignatureTable);
                MiddleSignatureCell.Colspan = 5;
                MiddleSignatureCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MainTable.AddCell(MiddleSignatureCell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);

                //PAGINA 7

                //ANEXO 3
                PdfPCell Annexed3Title = new PdfPCell(new Phrase("ANEXO 3 \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Annexed3Title.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Annexed3Title.HorizontalAlignment = Element.ALIGN_CENTER;
                Annexed3Title.Colspan = 3;
                MainTable.AddCell(Annexed3Title);

                //CONDICIONES COMERCIALES
                PdfPCell CommercialConditionsTitle = new PdfPCell(new Phrase("CONDICIONES COMERCIALES", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                CommercialConditionsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CommercialConditionsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                CommercialConditionsTitle.Colspan = 3;
                MainTable.AddCell(CommercialConditionsTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //APARTADO A
                PdfPCell ParACell = new PdfPCell();
                Paragraph ParAParagraph = new Paragraph();
                if (quotation.Value == "True")
                {
                    ParAParagraph.Add(new Chunk("APARTADO A.- \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParAParagraph.Add(new Chunk("El precio de la presente operación (contraprestación) en una sola exhibición AL CONTADO será por la cantidad de --------------------------- pesos mexicanos, moneda nacional --------------------------------- o el equivalente dólares de los Estados Unidos de América, " +
                        "de acuerdo al tipo de cambio que sea publicado por el Banco de México el día del cierre de la operación.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParAParagraph.SetLeading(1, 1);
                    ParACell.AddElement(ParAParagraph);
                    ParACell.Colspan = 5;
                    ParACell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ParACell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(ParACell);
                }
                else
                {
                    string originalPriceText = ConvertirNumeroALetras(dtoPurchasesRequest.OriginalPrice);
                    string cryptPriceText = ConvertirNumeroALetras(dtoPurchasesRequest.CryptPrice);

                    ParAParagraph.Add(new Chunk("APARTADO A.- \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParAParagraph.Add(new Chunk("El precio de la presente operación (contraprestación) en una sola exhibición",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParAParagraph.Add(new Chunk(" AL CONTADO",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParAParagraph.Add(new Chunk(" será por la cantidad de ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParAParagraph.Add(new Chunk("$" + dtoPurchasesRequest.OriginalPrice + " pesos mexicanos,",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParAParagraph.Add(new Chunk(" moneda nacional ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParAParagraph.Add(new Chunk("(" + originalPriceText + ")",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParAParagraph.Add(new Chunk(" o el equivalente dólares de los Estados Unidos de América, " +
                        "de acuerdo al tipo de cambio que sea publicado por el Banco de México el día del cierre de la operación.\n\n",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParAParagraph.SetLeading(1, 1);
                    ParACell.AddElement(ParAParagraph);
                    ParACell.Colspan = 5;
                    ParACell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ParACell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(ParACell);
                }


                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                if (sPaymentMethod != "Contado")
                {
                    string MensualidadesText = "";
                    //APARTADO B
                    if (quotation.Value != "True")
                    {
                        MensualidadesText = ConvertirNumeroALetras(dtoPurchasesRequest.Mensualidades);
                    }
                    PdfPCell ParBCell = new PdfPCell();
                    Paragraph ParBParagraph = new Paragraph();

                    if (quotation.Value == "True")
                    {
                        ParBParagraph.Add(new Chunk("APARTADO B.- \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                        ParBParagraph.Add(new Chunk("El precio de la presente operación (contraprestación) A PLAZOS o EN ABONOS no rebasará el término de 12, 24, 36 o 48 mensualidades, es decir, en uno, dos, tres y cuatro años deberá ser liquidado en su totalidad por parte de EL ADQUIRENTE, de acuerdo al financiamiento que elija. \n\n",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("Las mensualidades deberán ser por la cantidad de $------ pesos mexicanos, moneda nacional (------) o el equivalente, dólares de los Estados Unidos de América de acuerdo al tipo de cambio que sea publicado por el Banco de México el día del pago de la mensualidad. \n\n",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("Independiente del cumplimiento por parte de EL ADQUIRENTE en lo que toca en su aplicativa concerniente de las cláusulas 3 (TERCERA), 4 (CUARTA), 5 (QUINTA), 6 (SEXTA) Y 7 (SÉPTIMA), respectivamente, cabe destacar que, respecto de PAGOS ANTICIPADOS, cuando estos sean abonos parciales se recibirán siempre y cuando sean por cantidades que cubran íntegramente dos o más pagos mensuales. " +
                          "Estos últimos podrán aplicarse a los vencimientos subsecuentes, o a elección de EL ADQUIRENTE estos pagos se aplicarán al último vencimiento y continuando en ese orden, es decir, del último al primero. \n\n",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("Por otro lado, cabe distinguir, que el hecho que éste efectúe amortizaciones anticipadas, no lo exime de la obligación de seguir pagando los abonos mensuales que sigan a la fecha del pago anticipado, pues con ello solo se reduce el plazo, y los pagos normales subsecuentes seguirán cubriendo el orden normal del calendario en el presente contrato de adhesión. ",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("Ahora bien, cuando el pago anticipado sea por la totalidad del saldo, EL ADQUIRENTE tendrá un beneficio que será la aplicación del descuento sobre el saldo insoluto de acuerdo a la siguiente tabla de porcentajes, según el mes en que se esté realizando el pago: \n\n",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.SetLeading(1, 1);
                        ParBCell.AddElement(ParBParagraph);
                        ParBCell.Colspan = 5;
                        ParBCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        ParBCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        MainTable.AddCell(ParBCell);
                    }
                    else
                    {
                        ParBParagraph.Add(new Chunk("APARTADO B.- \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                        ParBParagraph.Add(new Chunk("El precio de la presente operación (contraprestación) A PLAZOS o EN ABONOS no rebasará el término de 12, 24, 36 o 48 mensualidades, es decir, en uno, dos, tres y cuatro años deberá ser liquidado en su totalidad por parte de EL ADQUIRENTE, de acuerdo al financiamiento que elija. \n\n",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("Las mensualidades deberán ser por la cantidad de ",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("$" + dtoPurchasesRequest.Mensualidades + " pesos mexicanos, ",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                        ParBParagraph.Add(new Chunk("moneda nacional ",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("(" + MensualidadesText + ")",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                        ParBParagraph.Add(new Chunk(" o el equivalente, dólares de los Estados Unidos de América de acuerdo al tipo de cambio que sea publicado por el Banco de México el día del pago de la mensualidad. \n\n",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("Independiente del cumplimiento por parte de EL ADQUIRENTE en lo que toca en su aplicativa concerniente de las cláusulas 3 (TERCERA), 4 (CUARTA), 5 (QUINTA), 6 (SEXTA) Y 7 (SÉPTIMA), respectivamente, cabe destacar que, respecto de PAGOS ANTICIPADOS, cuando estos sean abonos parciales se recibirán siempre y cuando sean por cantidades que cubran íntegramente dos o más pagos mensuales. " +
                          "Estos últimos podrán aplicarse a los vencimientos subsecuentes, o a elección de EL ADQUIRENTE estos pagos se aplicarán al último vencimiento y continuando en ese orden, es decir, del último al primero. \n\n",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("Por otro lado, cabe distinguir, que el hecho que éste efectúe amortizaciones anticipadas, no lo exime de la obligación de seguir pagando los abonos mensuales que sigan a la fecha del pago anticipado, pues con ello solo se reduce el plazo, y los pagos normales subsecuentes seguirán cubriendo el orden normal del calendario en el presente contrato de adhesión.",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.Add(new Chunk("Ahora bien, cuando el pago anticipado sea por la totalidad del saldo, EL ADQUIRENTE tendrá un beneficio que será la aplicación del descuento sobre el saldo insoluto de acuerdo a la siguiente tabla de porcentajes, según el mes en que se esté realizando el pago: \n\n",
                            new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                        ParBParagraph.SetLeading(1, 1);
                        ParBCell.AddElement(ParBParagraph);
                        ParBCell.Colspan = 5;
                        ParBCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        ParBCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        MainTable.AddCell(ParBCell);
                    }

                    //MINI TABLE APARTADO B
                    //PdfPCell MiniTableCell = new PdfPCell();
                    //PdfPTable MiniTable = new PdfPTable(2);
                    //MiniTable.AddCell(new Paragraph("Mes", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    //MiniTable.AddCell(new Paragraph("1er.", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    //MiniTable.AddCell(new Paragraph("Interés", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));
                    //MiniTable.AddCell(new Paragraph("5%", new Font(Font.FontFamily.UNDEFINED, 6f, Font.NORMAL)));

                    //MiniTable.TotalWidth = 200f;
                    //MiniTable.LockedWidth = true;
                    //MiniTable.HorizontalAlignment = 0;
                    //MiniTable.HorizontalAlignment = Element.ALIGN_CENTER;

                    //MiniTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //MiniTableCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //MiniTableCell.Colspan = 5;
                    //MiniTableCell.AddElement(MiniTable);
                    //MainTable.AddCell(MiniTableCell);

                    //APARTADO B PARRAFO FINAL
                    PdfPCell ParBFinalCell = new PdfPCell();
                    Paragraph ParBFinalParagraph = new Paragraph();

                    ParBFinalParagraph.Add(new Chunk("EL ADQUIRENTE pagará por su cuenta, todos los gastos, derechos e impuestos, servicios federales, estatales y/o municipales que se llegaran a causar con motivo de la celebración de este contrato de adhesión, así como por la expedición del título correspondiente.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParBFinalCell.SetLeading(1, 1);
                    ParBFinalCell.AddElement(ParBFinalParagraph);
                    ParBFinalCell.Colspan = 5;
                    ParBFinalCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ParBFinalCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(ParBFinalCell);

                    //SALTO DE LINEA
                    MainTable.AddCell(CellRow);
                    MainTable.AddCell(CellRow);
                    MainTable.AddCell(CellRow);
                    MainTable.AddCell(CellRow);
                    MainTable.AddCell(CellRow);
                    MainTable.AddCell(CellRow);
                    MainTable.AddCell(CellRow);
                }


                //PAGINA 8

                //ANEXO 4
                PdfPCell Annexed4Title = new PdfPCell(new Phrase("ANEXO 4 \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Annexed4Title.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Annexed4Title.HorizontalAlignment = Element.ALIGN_CENTER;
                Annexed4Title.Colspan = 3;
                MainTable.AddCell(Annexed4Title);

                //REGLAMENTO DEL COLUMBARIO
                PdfPCell ColumbariumRegulationTitle = new PdfPCell(new Phrase("REGLAMENTO DEL COLUMBARIO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ColumbariumRegulationTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ColumbariumRegulationTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ColumbariumRegulationTitle.Colspan = 3;
                MainTable.AddCell(ColumbariumRegulationTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //APARTADO B
                PdfPCell CRCell1 = new PdfPCell();
                Paragraph CRParagraph1 = new Paragraph();

                CRParagraph1.Add(new Chunk("Con el propósito de conservar siempre en buen estado las instalaciones, muebles, áreas verdes, capillas, estacionamiento, etc., " +
                    "y todo lo relacionado con la custodia, depósito y extracción de urnas cuyo contenido sea única y exclusivamente el de restos humanos incinerados o cremados, se creó el presente reglamento, con el cual se sujetarán a las siguientes: \n\n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                CRParagraph1.Add(new Chunk("Que, en virtud de la autorización concedida por el Arzobispo de Tijuana mediante decreto y acta constitutiva del día 12 de marzo del 2002 (18-09-2017), he tenido a bien expedir el siguiente Reglamento que está constituido por los siguientes Títulos. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                CRParagraph1.SetLeading(1, 1);
                CRCell1.AddElement(CRParagraph1);
                CRCell1.Colspan = 5;
                CRCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CRCell1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CRCell1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO PREELIMINAR
                PdfPCell PreeliminarTitle = new PdfPCell(new Phrase("TÍTULO PREELIMINAR \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                PreeliminarTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PreeliminarTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                PreeliminarTitle.Colspan = 3;
                MainTable.AddCell(PreeliminarTitle);

                //DISPOSICIONES GENERALES
                PdfPCell GeneralDispositionTitle = new PdfPCell(new Phrase("DISPOSICIONES GENERALES", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                GeneralDispositionTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                GeneralDispositionTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                GeneralDispositionTitle.Colspan = 3;
                MainTable.AddCell(GeneralDispositionTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 1
                PdfPCell Art1Cell = new PdfPCell();
                Paragraph Art1Paragraph = new Paragraph();

                Art1Paragraph.Add(new Chunk("Artículo 1. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art1Paragraph.Add(new Chunk("El presente reglamento tiene por objeto regular el uso en participación, funcionamiento, conservación y vigilancia del Columbario (zona de criptas) con que se cuenta.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art1Paragraph.SetLeading(1, 1);
                Art1Cell.AddElement(Art1Paragraph);
                Art1Cell.Colspan = 5;
                Art1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art1Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art1Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 2
                PdfPCell Art2Cell = new PdfPCell();
                Paragraph Art2Paragraph = new Paragraph();

                Art2Paragraph.Add(new Chunk("Artículo 2. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art2Paragraph.Add(new Chunk("A él deberán sujetarse todas aquellas personas o familias que hayan adquirido una o más criptas presentando su contrato firmado por el Adquiriente y por el Representante Legal con la Asociación.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art2Paragraph.SetLeading(1, 1);
                Art2Cell.AddElement(Art2Paragraph);
                Art2Cell.Colspan = 5;
                Art2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art2Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 3
                PdfPCell Art3Cell = new PdfPCell();
                Paragraph Art3Paragraph = new Paragraph();

                Art3Paragraph.Add(new Chunk("Artículo 3. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3Paragraph.Add(new Chunk("El reglamento Interno de la Parroquia Nuestra Señora de Guadalupe, a fin de comprender la diversidad de conceptos utilizados durante esta normatividad expone las siguientes definiciones, entendiéndose como: \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3Paragraph.SetLeading(1, 1);
                Art3Cell.AddElement(Art3Paragraph);
                Art3Cell.Colspan = 5;
                Art3Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3Cell);

                //ARTICULO 3 Apartado 1
                PdfPCell Art3_1Cell = new PdfPCell();
                Paragraph Art3_1Paragraph = new Paragraph();

                Art3_1Paragraph.Add(new Chunk("I. Acta de defunción: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_1Paragraph.Add(new Chunk("es un documento oficial emitido por el Registro Civil en donde se registra el momento exacto de la Muerte de la persona. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_1Paragraph.SetLeading(1, 1);
                Art3_1Cell.AddElement(Art3_1Paragraph);
                Art3_1Cell.Colspan = 5;
                Art3_1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_1Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_1Cell);

                //ARTICULO 3 Apartado 2
                PdfPCell Art3_2Cell = new PdfPCell();
                Paragraph Art3_2Paragraph = new Paragraph();

                Art3_2Paragraph.Add(new Chunk("II. Beneficiarios: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_2Paragraph.Add(new Chunk("personas que el titular establece al momento de la compra de su cripta, mismos que pueden ser modificados al momento que el Titular lo decida. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_2Paragraph.SetLeading(1, 1);
                Art3_2Cell.AddElement(Art3_2Paragraph);
                Art3_2Cell.Colspan = 5;
                Art3_2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_2Cell);

                //ARTICULO 3 Apartado 3
                PdfPCell Art3_3Cell = new PdfPCell();
                Paragraph Art3_3Paragraph = new Paragraph();

                Art3_3Paragraph.Add(new Chunk("III. Cadáver/difunto: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_3Paragraph.Add(new Chunk("cuerpo humano en el que se haya comprobado la perdida de la vida. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_3Paragraph.SetLeading(1, 1);
                Art3_3Cell.AddElement(Art3_3Paragraph);
                Art3_3Cell.Colspan = 5;
                Art3_3Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_3Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_3Cell);

                //ARTICULO 3 Apartado 4
                PdfPCell Art3_4Cell = new PdfPCell();
                Paragraph Art3_4Paragraph = new Paragraph();

                Art3_4Paragraph.Add(new Chunk("IV. Cenizas: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_4Paragraph.Add(new Chunk("resultado del proceso de cremación de restos humanos. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_4Paragraph.SetLeading(1, 1);
                Art3_4Cell.AddElement(Art3_4Paragraph);
                Art3_4Cell.Colspan = 5;
                Art3_4Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_4Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_4Cell);

                //ARTICULO 3 Apartado 5
                PdfPCell Art3_5Cell = new PdfPCell();
                Paragraph Art3_5Paragraph = new Paragraph();

                Art3_5Paragraph.Add(new Chunk("V. Columbario: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_5Paragraph.Add(new Chunk("conjunto de nichos destinados al depósito de restos humanos áridos o cremados. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_5Paragraph.SetLeading(1, 1);
                Art3_5Cell.AddElement(Art3_5Paragraph);
                Art3_5Cell.Colspan = 5;
                Art3_5Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_5Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_5Cell);

                //ARTICULO 3 Apartado 6
                PdfPCell Art3_6Cell = new PdfPCell();
                Paragraph Art3_6Paragraph = new Paragraph();

                Art3_6Paragraph.Add(new Chunk("VI. Consumidor: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_6Paragraph.Add(new Chunk("persona física o moral que adquiere o utiliza bienes y servicios funerales. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_6Paragraph.SetLeading(1, 1);
                Art3_6Cell.AddElement(Art3_6Paragraph);
                Art3_6Cell.Colspan = 5;
                Art3_6Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_6Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_6Cell);

                //ARTICULO 3 Apartado 7
                PdfPCell Art3_7Cell = new PdfPCell();
                Paragraph Art3_7Paragraph = new Paragraph();

                Art3_7Paragraph.Add(new Chunk("VII. Cremación: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_7Paragraph.Add(new Chunk("proceso mediante el cual un cadáver, restos humanos o restos áridos, se someten a técnicas y procedimientos adecuados con la finalidad de reducirlos a cenizas. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_7Paragraph.SetLeading(1, 1);
                Art3_7Cell.AddElement(Art3_7Paragraph);
                Art3_7Cell.Colspan = 5;
                Art3_7Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_7Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_7Cell);

                //ARTICULO 3 Apartado 8
                PdfPCell Art3_8Cell = new PdfPCell();
                Paragraph Art3_8Paragraph = new Paragraph();

                Art3_8Paragraph.Add(new Chunk("VIII. Cripta: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_8Paragraph.Add(new Chunk("lugar destinado para depositar cenizas. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_8Paragraph.SetLeading(1, 1);
                Art3_8Cell.AddElement(Art3_8Paragraph);
                Art3_8Cell.Colspan = 5;
                Art3_8Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_8Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_8Cell);

                //ARTICULO 3 Apartado 9
                PdfPCell Art3_9Cell = new PdfPCell();
                Paragraph Art3_9Paragraph = new Paragraph();

                Art3_9Paragraph.Add(new Chunk("IX. Cripta comunitaria: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_9Paragraph.Add(new Chunk("estructura construida destinada al depósito de cadáveres, restos humanos áridos o cremados, que es compartida con otras 3 (tres) urnas totalmente independientes al adquiriente. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_9Paragraph.SetLeading(1, 1);
                Art3_9Cell.AddElement(Art3_9Paragraph);
                Art3_9Cell.Colspan = 5;
                Art3_9Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_9Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_9Cell);


                //ARTICULO 3 Apartado 10
                PdfPCell Art3_10Cell = new PdfPCell();
                Paragraph Art3_10Paragraph = new Paragraph();

                Art3_10Paragraph.Add(new Chunk("X. Cripta familiar: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_10Paragraph.Add(new Chunk("estructura construida destinada al depósito de cadáveres, restos humanos áridos o cremados. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_10Paragraph.SetLeading(1, 1);
                Art3_10Cell.AddElement(Art3_10Paragraph);
                Art3_10Cell.Colspan = 5;
                Art3_10Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_10Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_10Cell);

                //ARTICULO 3 Apartado 11
                PdfPCell Art3_11Cell = new PdfPCell();
                Paragraph Art3_11Paragraph = new Paragraph();

                Art3_11Paragraph.Add(new Chunk("XI. Exhumación: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_11Paragraph.Add(new Chunk("extradición de un cadáver sepultado. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_11Paragraph.SetLeading(1, 1);
                Art3_11Cell.AddElement(Art3_11Paragraph);
                Art3_11Cell.Colspan = 5;
                Art3_11Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_11Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_11Cell);

                //ARTICULO 3 Apartado 12
                PdfPCell Art3_12Cell = new PdfPCell();
                Paragraph Art3_12Paragraph = new Paragraph();

                Art3_12Paragraph.Add(new Chunk("XII. Inhumación: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_12Paragraph.Add(new Chunk("sepultar un cadáver. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_12Paragraph.SetLeading(1, 1);
                Art3_12Cell.AddElement(Art3_12Paragraph);
                Art3_12Cell.Colspan = 5;
                Art3_12Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_12Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_12Cell);

                //ARTICULO 3 Apartado 13
                PdfPCell Art3_13Cell = new PdfPCell();
                Paragraph Art3_13Paragraph = new Paragraph();

                Art3_13Paragraph.Add(new Chunk("XIII. Nichos: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_13Paragraph.Add(new Chunk("espacio destinado al depósito de restos humanos áridos o cremados. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_13Paragraph.SetLeading(1, 1);
                Art3_13Cell.AddElement(Art3_13Paragraph);
                Art3_13Cell.Colspan = 5;
                Art3_13Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_13Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_13Cell);

                //ARTICULO 3 Apartado 14
                PdfPCell Art3_14Cell = new PdfPCell();
                Paragraph Art3_14Paragraph = new Paragraph();

                Art3_14Paragraph.Add(new Chunk("XIV. Osario: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_14Paragraph.Add(new Chunk("lugar destinado para depósito de restos humanos áridos. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_14Paragraph.SetLeading(1, 1);
                Art3_14Cell.AddElement(Art3_14Paragraph);
                Art3_14Cell.Colspan = 5;
                Art3_14Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_14Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_14Cell);

                //ARTICULO 3 Apartado 15
                PdfPCell Art3_15Cell = new PdfPCell();
                Paragraph Art3_15Paragraph = new Paragraph();

                Art3_15Paragraph.Add(new Chunk("XV. Panteón: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_15Paragraph.Add(new Chunk("lugar destinado a recibir cadáveres, restos humanos y restos humanos áridos o cremados. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_15Paragraph.SetLeading(1, 1);
                Art3_15Cell.AddElement(Art3_15Paragraph);
                Art3_15Cell.Colspan = 5;
                Art3_15Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_15Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_15Cell);

                //ARTICULO 3 Apartado 16
                PdfPCell Art3_16Cell = new PdfPCell();
                Paragraph Art3_16Paragraph = new Paragraph();

                Art3_16Paragraph.Add(new Chunk("XVI. Permiso de cremación: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_16Paragraph.Add(new Chunk("documento expedido por la oficialía de registro civil donde se autoriza el trámite de cremación. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_16Paragraph.SetLeading(1, 1);
                Art3_16Cell.AddElement(Art3_16Paragraph);
                Art3_16Cell.Colspan = 5;
                Art3_16Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_16Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_16Cell);

                //PAGINA 9

                //ARTICULO 3 Apartado 17
                PdfPCell Art3_17Cell = new PdfPCell();
                Paragraph Art3_17Paragraph = new Paragraph();

                Art3_17Paragraph.Add(new Chunk("XVII. Restos áridos: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_17Paragraph.Add(new Chunk("osamenta de un cadáver como resultado del proceso natural de descomposición de un cuerpo. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_17Paragraph.SetLeading(1, 1);
                Art3_17Cell.AddElement(Art3_17Paragraph);
                Art3_17Cell.Colspan = 5;
                Art3_17Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_17Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_17Cell);

                //ARTICULO 3 Apartado 18
                PdfPCell Art3_18Cell = new PdfPCell();
                Paragraph Art3_18Paragraph = new Paragraph();

                Art3_18Paragraph.Add(new Chunk("XVIII. Titular: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_18Paragraph.Add(new Chunk("Persona con capacidad jurídica para adquirir una cripta en el columbario, mediante contrato firmado por las dos partes. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_18Paragraph.SetLeading(1, 1);
                Art3_18Cell.AddElement(Art3_18Paragraph);
                Art3_18Cell.Colspan = 5;
                Art3_18Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_18Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_18Cell);

                //ARTICULO 3 Apartado 19
                PdfPCell Art3_19Cell = new PdfPCell();
                Paragraph Art3_19Paragraph = new Paragraph();

                Art3_19Paragraph.Add(new Chunk("XIX. Usuario: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_19Paragraph.Add(new Chunk("la(s) persona(s) designadas por el titular, o en su caso por el beneficiario, para que al fallecer se les proporcione un espacio en la cripta adquirida. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_19Paragraph.SetLeading(1, 1);
                Art3_19Cell.AddElement(Art3_19Paragraph);
                Art3_19Cell.Colspan = 5;
                Art3_19Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_19Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_19Cell);

                //ARTICULO 3 Apartado 20
                PdfPCell Art3_20Cell = new PdfPCell();
                Paragraph Art3_20Paragraph = new Paragraph();

                Art3_20Paragraph.Add(new Chunk("XX. Velatorio: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art3_20Paragraph.Add(new Chunk("lugar destinado a la velación. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art3_20Paragraph.SetLeading(1, 1);
                Art3_20Cell.AddElement(Art3_20Paragraph);
                Art3_20Cell.Colspan = 5;
                Art3_20Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3_20Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3_20Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO PRIMERO
                PdfPCell FirstTitle = new PdfPCell(new Phrase("TÍTULO PRIMERO \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                FirstTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FirstTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                FirstTitle.Colspan = 3;
                MainTable.AddCell(FirstTitle);

                //PARTES
                PdfPCell PartsTitle = new PdfPCell(new Phrase("PARTES \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                PartsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PartsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                PartsTitle.Colspan = 3;
                MainTable.AddCell(PartsTitle);

                //ASOCIACIÓN Y ADQUIRIENTE
                PdfPCell AssociationandacquirerTitle = new PdfPCell(new Phrase("ASOCIACIÓN Y ADQUIRIENTE", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                AssociationandacquirerTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AssociationandacquirerTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                AssociationandacquirerTitle.Colspan = 3;
                MainTable.AddCell(AssociationandacquirerTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 4
                PdfPCell Art4Cell = new PdfPCell();
                Paragraph Art4Paragraph = new Paragraph();

                Art4Paragraph.Add(new Chunk("Artículo 4. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art4Paragraph.Add(new Chunk("El columbario de la Nueva Catedral de Tijuana es propiedad de la Parroquia Iglesia Guadalupe del Rio en Tijuana, A.R., quien además es la Titular para prestar y administrar el servicio público de depósito y extracción de urnas cuyo contenido sea únicamente el de restos humanos secos e incinerados, la administración determinará las modalidades y forma de uso de los muebles e inmuebles del Columbario. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art4Paragraph.SetLeading(1, 1);
                Art4Cell.AddElement(Art4Paragraph);
                Art4Cell.Colspan = 5;
                Art4Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art4Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art4Cell);

                //ARTICULO 5
                PdfPCell Art5Cell = new PdfPCell();
                Paragraph Art5Paragraph = new Paragraph();

                Art5Paragraph.Add(new Chunk("Artículo 5. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art5Paragraph.Add(new Chunk("Los usuarios de los servicios del columbario se sujetarán a lo dispuesto en el presente reglamento, obligándose a su cumplimiento. La administración del columbario se reserva el derecho de rehusar la admisión al mismo de cualquier persona, a excepción de los titulares de derechos de uso de las criptas, sus familiares y allegados. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art5Paragraph.SetLeading(1, 1);
                Art5Cell.AddElement(Art5Paragraph);
                Art5Cell.Colspan = 5;
                Art5Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art5Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art5Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO SEGUNDO
                PdfPCell SecondTitle = new PdfPCell(new Phrase("TÍTULO SEGUNDO \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                SecondTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SecondTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                SecondTitle.Colspan = 3;
                MainTable.AddCell(SecondTitle);

                //HORARIO DE APERTURA Y CIERRE DEL COLUMBARIO
                PdfPCell OpenScheduleTitle = new PdfPCell(new Phrase("HORARIO DE APERTURA Y CIERRE DEL COLUMBARIO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                OpenScheduleTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                OpenScheduleTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                OpenScheduleTitle.Colspan = 3;
                MainTable.AddCell(OpenScheduleTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 6
                PdfPCell Art6Cell = new PdfPCell();
                Paragraph Art6Paragraph = new Paragraph();

                Art6Paragraph.Add(new Chunk("Artículo 6. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art6Paragraph.Add(new Chunk("Las puertas de acceso al columbario se abrirán a las 7:00 (siete) horas y cerrarán a las 20:00 (veinte) horas, en cuyo lapso deberán celebrarse el depósito de urnas con cenizas de restos humanos, en la inteligencia de que los servicios se harán de acuerdo con lo establecido en el Artículo 16 de este reglamento.  \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art6Paragraph.SetLeading(1, 1);
                Art6Cell.AddElement(Art6Paragraph);
                Art6Cell.Colspan = 5;
                Art6Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art6Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art6Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 7
                PdfPCell Art7Cell = new PdfPCell();
                Paragraph Art7Paragraph = new Paragraph();

                Art7Paragraph.Add(new Chunk("Artículo 7. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art7Paragraph.Add(new Chunk("¿Por causas de fuerza mayor, por caso fortuito, por indicaciones de autoridades competentes o actividades arquidiocesanas la asociación se reserva el derecho de modificación del horario, en las instalaciones de la Parroquia Iglesia Nuestra Señora de Guadalupe en Zona Rio A. R. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art7Paragraph.SetLeading(1, 1);
                Art7Cell.AddElement(Art7Paragraph);
                Art7Cell.Colspan = 5;
                Art7Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art7Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art7Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO TERCERO
                PdfPCell ThirdTitle = new PdfPCell(new Phrase("TÍTULO TERCERO \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ThirdTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ThirdTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ThirdTitle.Colspan = 3;
                MainTable.AddCell(ThirdTitle);

                //BENEFICIOS Y OBLIGACIONES DEL ADQUIRIENTE
                PdfPCell BenefitsAndObligationsTitle = new PdfPCell(new Phrase("BENEFICIOS Y OBLIGACIONES DEL ADQUIRIENTE", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                BenefitsAndObligationsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                BenefitsAndObligationsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                BenefitsAndObligationsTitle.Colspan = 3;
                MainTable.AddCell(BenefitsAndObligationsTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 8
                PdfPCell Art8Cell = new PdfPCell();
                Paragraph Art8Paragraph = new Paragraph();

                Art8Paragraph.Add(new Chunk("Artículo 8. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art8Paragraph.Add(new Chunk("Este título corresponde a todos los beneficios que el adquirente contrae al momento de la firma del contrato de adhesión a perpetuidad con la Asociación. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8Paragraph.SetLeading(1, 1);
                Art8Cell.AddElement(Art8Paragraph);
                Art8Cell.Colspan = 5;
                Art8Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8Cell);

                //ARTICULO 8 APARTADO 1
                PdfPCell Art8_1Cell = new PdfPCell();
                Paragraph Art8_1Paragraph = new Paragraph();

                Art8_1Paragraph.Add(new Chunk("I.   El adquiriente tendrá derecho a un Nicho a perpetuidad. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8_1Paragraph.SetLeading(1, 1);
                Art8_1Cell.AddElement(Art8_1Paragraph);
                Art8_1Cell.Colspan = 5;
                Art8_1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8_1Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8_1Cell);

                //ARTICULO 8 APARTADO 2
                PdfPCell Art8_2Cell = new PdfPCell();
                Paragraph Art8_2Paragraph = new Paragraph();

                Art8_2Paragraph.Add(new Chunk("II.  Se tiene como beneficio la celebración de Aniversario luctuoso sin costo, que consiste en ofrecer una intención en la Santa Misa del día. La notificación será al Titular, primeramente, si este ya falleció se le notificara al beneficiario en turno. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8_2Paragraph.SetLeading(1, 1);
                Art8_2Cell.AddElement(Art8_2Paragraph);
                Art8_2Cell.Colspan = 5;
                Art8_2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8_2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8_2Cell);

                //ARTICULO 8 APARTADO 3
                PdfPCell Art8_3Cell = new PdfPCell();
                Paragraph Art8_3Paragraph = new Paragraph();

                Art8_3Paragraph.Add(new Chunk("III.	El adquirente y su familia tendrán talleres de duelo gratuitos, siendo canalizados con personas especializadas en situaciones del proceso de duelo. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8_3Paragraph.SetLeading(1, 1);
                Art8_3Cell.AddElement(Art8_3Paragraph);
                Art8_3Cell.Colspan = 5;
                Art8_3Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8_3Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8_3Cell);

                //ARTICULO 8 APARTADO 4
                PdfPCell Art8_4Cell = new PdfPCell();
                Paragraph Art8_4Paragraph = new Paragraph();

                Art8_4Paragraph.Add(new Chunk("IV.  El adquirente y su familia tendrán el beneficio de la Dirección espiritual por un sacerdote, religioso o religiosa, asignado por la administración de la parroquia, previamente solicitándolo ante la oficina parroquial. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8_4Paragraph.SetLeading(1, 1);
                Art8_4Cell.AddElement(Art8_4Paragraph);
                Art8_4Cell.Colspan = 5;
                Art8_4Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8_4Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8_4Cell);

                //ARTICULO 8 APARTADO 5
                PdfPCell Art8_5Cell = new PdfPCell();
                Paragraph Art8_5Paragraph = new Paragraph();

                Art8_5Paragraph.Add(new Chunk("V.   El adquiriente y su familia podrá tener el sacramento de la confesión previa cita en la oficina parroquial o los días establecidos por la administración, con los requisitos que obliga este sacramento. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8_5Paragraph.SetLeading(1, 1);
                Art8_5Cell.AddElement(Art8_5Paragraph);
                Art8_5Cell.Colspan = 5;
                Art8_5Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8_5Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8_5Cell);

                //PAGINA 10

                //ARTICULO 8 APARTADO 6
                PdfPCell Art8_6Cell = new PdfPCell();
                Paragraph Art8_6Paragraph = new Paragraph();

                Art8_6Paragraph.Add(new Chunk("VI.  El adquiriente tiene la capacidad para designar los espacios específicos que él considere en la cripta que adquirió, a través de un documento a mano, firmado por él mismo y presentado en original a la oficina del departamento de Criptas, sin modificación durante o después de vida, siempre que existan espacios disponibles. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8_6Paragraph.SetLeading(1, 1);
                Art8_6Cell.AddElement(Art8_6Paragraph);
                Art8_6Cell.Colspan = 5;
                Art8_6Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8_6Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8_6Cell);

                //ARTICULO 8 APARTADO 7
                PdfPCell Art8_7Cell = new PdfPCell();
                Paragraph Art8_7Paragraph = new Paragraph();

                Art8_7Paragraph.Add(new Chunk("VII. El adquiriente tendrá la certeza de que toda urna será cuidada, salvaguardada, custodiada y se le dará un lugar digno para su protección. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8_7Paragraph.SetLeading(1, 1);
                Art8_7Cell.AddElement(Art8_7Paragraph);
                Art8_7Cell.Colspan = 5;
                Art8_7Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8_7Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8_7Cell);

                //ARTICULO 8 APARTADO 8
                PdfPCell Art8_8Cell = new PdfPCell();
                Paragraph Art8_8Paragraph = new Paragraph();

                Art8_8Paragraph.Add(new Chunk("VIII.    El titular es el único que tiene toda capacidad jurídica para hacer valer los beneficios del contrato. Los demás se apegan a las normas vigentes y establecidas por la Asociación. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art8_8Paragraph.SetLeading(1, 1);
                Art8_8Cell.AddElement(Art8_8Paragraph);
                Art8_8Cell.Colspan = 5;
                Art8_8Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8_8Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8_8Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 9
                PdfPCell Art9Cell = new PdfPCell();
                Paragraph Art9Paragraph = new Paragraph();

                Art9Paragraph.Add(new Chunk("Artículo 9. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art9Paragraph.Add(new Chunk("Este título corresponde a las obligaciones que el adquirente contrae al momento de la firma del contrato de adhesión a perpetuidad con la Asociación. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9Paragraph.SetLeading(1, 1);
                Art9Cell.AddElement(Art9Paragraph);
                Art9Cell.Colspan = 5;
                Art9Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9Cell);

                //ARTICULO 9 APARTADO 1
                PdfPCell Art9_1Cell = new PdfPCell();
                Paragraph Art9_1Paragraph = new Paragraph();

                Art9_1Paragraph.Add(new Chunk("I.   A partir de la fecha de la firma del contrato, el titular se compromete a aportar una cuota anual por tiempo indefinido, que servirá para el mantenimiento de áreas internas y externas de las instalaciones del columbario. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9_1Paragraph.SetLeading(1, 1);
                Art9_1Cell.AddElement(Art9_1Paragraph);
                Art9_1Cell.Colspan = 5;
                Art9_1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9_1Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9_1Cell);

                //ARTICULO 9 APARTADO 2
                PdfPCell Art9_2Cell = new PdfPCell();
                Paragraph Art9_2Paragraph = new Paragraph();

                Art9_2Paragraph.Add(new Chunk("II.  En caso de Muerte del titular, dicha obligación será adquirida por el primer beneficiario en la lista de los registrados por el titular, si hay negación se pasará al que sigue, hasta terminar con los beneficiarios. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9_2Paragraph.SetLeading(1, 1);
                Art9_2Cell.AddElement(Art9_2Paragraph);
                Art9_2Cell.Colspan = 5;
                Art9_2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9_2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9_2Cell);

                //ARTICULO 9 APARTADO 3
                tblMaintenanceFee tblMaintenance2 = MaintenanceFeeB.GetLastReccord(0);
                PdfPCell Art9_3Cell = new PdfPCell();
                Paragraph Art9_3Paragraph = new Paragraph();

                Art9_3Paragraph.Add(new Chunk("III.	Las aportaciones por mantenimiento según la cláusula 4.1.9 del contrato de adhesión a perpetuidad, los precios de una cripta familiar serán de " + tblMaintenance.Cost.ToString("C2") + " pesos, moneda nacional o su equivalente en dólar, de acuerdo al tipo de cambio que impere y rija el Banco de México al día en que se realice el pago o pagos correspondientes. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9_3Paragraph.SetLeading(1, 1);
                Art9_3Cell.AddElement(Art9_3Paragraph);
                Art9_3Cell.Colspan = 5;
                Art9_3Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9_3Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9_3Cell);

                //ARTICULO 9 APARTADO 4
                PdfPCell Art9_4Cell = new PdfPCell();
                Paragraph Art9_4Paragraph = new Paragraph();

                Art9_4Paragraph.Add(new Chunk("IV.  Cubrir el pago por depósito, extracción o traslado de urna. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9_4Paragraph.SetLeading(1, 1);
                Art9_4Cell.AddElement(Art9_4Paragraph);
                Art9_4Cell.Colspan = 5;
                Art9_4Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9_4Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9_4Cell);

                //ARTICULO 9 APARTADO 5
                PdfPCell Art9_5Cell = new PdfPCell();
                Paragraph Art9_5Paragraph = new Paragraph();

                Art9_5Paragraph.Add(new Chunk("V.   Para el resguardo de una primera urna se dará el depósito del 50% del total del precio de la cripta y el 100% para el uso subsecuente de los demás beneficiarios. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9_5Paragraph.SetLeading(1, 1);
                Art9_5Cell.AddElement(Art9_5Paragraph);
                Art9_5Cell.Colspan = 5;
                Art9_5Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9_5Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9_5Cell);

                //ARTICULO 9 APARTADO 6
                PdfPCell Art9_6Cell = new PdfPCell();
                Paragraph Art9_6Paragraph = new Paragraph();

                Art9_6Paragraph.Add(new Chunk("VI.  En caso de modificación, corrección o recisión al contrato de adhesión a perpetuidad por parte del ADQUIRIENTE, los gastos correrán por la parte interesada. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9_6Paragraph.SetLeading(1, 1);
                Art9_6Cell.AddElement(Art9_6Paragraph);
                Art9_6Cell.Colspan = 5;
                Art9_6Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9_6Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9_6Cell);

                //ARTICULO 9 APARTADO 7
                PdfPCell Art9_7Cell = new PdfPCell();
                Paragraph Art9_7Paragraph = new Paragraph();

                Art9_7Paragraph.Add(new Chunk("VII. En caso del fallecimiento del titular, los beneficiarios asumen las obligaciones del titular y no sus beneficios que cambien su voluntad. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9_7Paragraph.SetLeading(1, 1);
                Art9_7Cell.AddElement(Art9_7Paragraph);
                Art9_7Cell.Colspan = 5;
                Art9_7Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9_7Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9_7Cell);

                //ARTICULO 9 APARTADO 8
                PdfPCell Art9_8Cell = new PdfPCell();
                Paragraph Art9_8Paragraph = new Paragraph();

                Art9_8Paragraph.Add(new Chunk("VIII.    En caso de que los beneficios no asuman las obligaciones del titular, mencionadas en las fracciones anteriores, el destino de las urnas contenidas en la cripta correspondiente, queda reservada a la administración de la Parroquia. \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art9_8Paragraph.SetLeading(1, 1);
                Art9_8Cell.AddElement(Art9_8Paragraph);
                Art9_8Cell.Colspan = 5;
                Art9_8Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9_8Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9_8Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO CUARTO
                PdfPCell FourthTitle = new PdfPCell(new Phrase("TÍTULO CUARTO \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                FourthTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FourthTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                FourthTitle.Colspan = 3;
                MainTable.AddCell(FourthTitle);

                //BENEFICIOS Y OBLIGACIONES DEL ADQUIRIENTE
                PdfPCell AssociationObligationsTitle = new PdfPCell(new Phrase("OBLIGACIONES DE LA ASOCIACIÓN", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                AssociationObligationsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AssociationObligationsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                AssociationObligationsTitle.Colspan = 3;
                MainTable.AddCell(AssociationObligationsTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 10
                PdfPCell Art10Cell = new PdfPCell();
                Paragraph Art10Paragraph = new Paragraph();

                Art10Paragraph.Add(new Chunk("Artículo 10. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art10Paragraph.Add(new Chunk("Son obligaciones de la Parroquia Nuestra Señora de Guadalupe A.R, en Zona Rio, Tijuana, Baja California: \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art10Paragraph.SetLeading(1, 1);
                Art10Cell.AddElement(Art10Paragraph);
                Art10Cell.Colspan = 5;
                Art10Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art10Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art10Cell);

                //ARTICULO 10 APARTADO 1
                PdfPCell Art10_1Cell = new PdfPCell();
                Paragraph Art10_1Paragraph = new Paragraph();

                Art10_1Paragraph.Add(new Chunk("I.  Llevar un registro (expediente) donde aparezca: Información de adquiriente, beneficiarios, póliza de mantenimiento, registro de depósitos (cenizas) que allí se tengan, donde este identificado con los siguientes datos: nombre completo, fecha de nacimiento, fecha de defunción, copia del acta de defunción y copia del permiso de cremación. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art10_1Paragraph.SetLeading(1, 1);
                Art10_1Cell.AddElement(Art10_1Paragraph);
                Art10_1Cell.Colspan = 5;
                Art10_1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art10_1Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art10_1Cell);

                //ARTICULO 10 APARTADO 2
                PdfPCell Art10_2Cell = new PdfPCell();
                Paragraph Art10_2Paragraph = new Paragraph();

                Art10_2Paragraph.Add(new Chunk("II. Mantener y conservar en condiciones de higiene y limpieza del columbario, así como la seguridad de la zona donde se encuentren las criptas. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art10_2Paragraph.SetLeading(1, 1);
                Art10_2Cell.AddElement(Art10_2Paragraph);
                Art10_2Cell.Colspan = 5;
                Art10_2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art10_2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art10_2Cell);

                //ARTICULO 10 APARTADO 3
                PdfPCell Art10_3Cell = new PdfPCell();
                Paragraph Art10_3Paragraph = new Paragraph();

                Art10_3Paragraph.Add(new Chunk("III.    Mantener la zona del columbario iluminada, segura, monitoreada por Sistema de vigilancia de circuito cerrado las 24 horas.  \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art10_3Paragraph.SetLeading(1, 1);
                Art10_3Cell.AddElement(Art10_3Paragraph);
                Art10_3Cell.Colspan = 5;
                Art10_3Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art10_3Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art10_3Cell);

                //ARTICULO 10 APARTADO 4
                PdfPCell Art10_4Cell = new PdfPCell();
                Paragraph Art10_4Paragraph = new Paragraph();

                Art10_4Paragraph.Add(new Chunk("IV. Y las demás naturales a las obligaciones de la Asociación Iglesia de Guadalupe del Rio en Tijuana A.R. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art10_4Paragraph.SetLeading(1, 1);
                Art10_4Cell.AddElement(Art10_4Paragraph);
                Art10_4Cell.Colspan = 5;
                Art10_4Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art10_4Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art10_4Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);

                //PAGINA 11

                //TÍTULO QUINTO
                PdfPCell FifthTitle = new PdfPCell(new Phrase("TÍTULO QUINTO \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                FifthTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FifthTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                FifthTitle.Colspan = 3;
                MainTable.AddCell(FifthTitle);

                //DEPOSITO Y RETIRO DE URNAS
                PdfPCell DepositOfUrnsTitle = new PdfPCell(new Phrase("DEPOSITO Y RETIRO DE URNAS", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                DepositOfUrnsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                DepositOfUrnsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                DepositOfUrnsTitle.Colspan = 3;
                MainTable.AddCell(DepositOfUrnsTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 11
                PdfPCell Art11Cell = new PdfPCell();
                Paragraph Art11Paragraph = new Paragraph();

                Art11Paragraph.Add(new Chunk("Artículo 11. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art11Paragraph.Add(new Chunk("El servicio de depósito de urnas con los restos incinerados de las personas fallecidas, se solicitará a la administración con una anticipación no menor de 48 de horas, en el entendido de que este servicio no deberá realizarse en ningún caso antes de las 9:15 am (nueve) horas y después de las 20:00 (veinte) horas. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art11Paragraph.SetLeading(1, 1);
                Art11Cell.AddElement(Art11Paragraph);
                Art11Cell.Colspan = 5;
                Art11Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art11Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art11Cell);

                //ARTICULO 12
                PdfPCell Art12Cell = new PdfPCell();
                Paragraph Art12Paragraph = new Paragraph();

                Art12Paragraph.Add(new Chunk("Artículo 12. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art12Paragraph.Add(new Chunk("El DEPÓSITO Y RETIRO de urnas, solo se podrá efectuarse mediante solicitud expresa del TITULAR del CONTRATO DE CESIÓN DE DERECHOS de uso Mortuorio a Perpetuidad o la(s) personas (s) que este haya autorizado ESPECIFICAMENTE por escrito a su fallecimiento, además para autorizar el depósito de urnas, la administración del columbario le exigirá al Titular comprobar que están al corriente del pago, de presentar una petición por escrito y en su caso de exhibir el título de derecho de uso mortuorio a perpetuidad y estar apegado a los términos y condiciones que fijen las autoridades de Salubridad y Municipales respectivamente. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art12Paragraph.SetLeading(1, 1);
                Art12Cell.AddElement(Art12Paragraph);
                Art12Cell.Colspan = 5;
                Art12Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art12Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art12Cell);

                //ARTICULO 13
                PdfPCell Art13Cell = new PdfPCell();
                Paragraph Art13Paragraph = new Paragraph();

                Art13Paragraph.Add(new Chunk("Artículo 13. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art13Paragraph.Add(new Chunk("EL DEPOÓSITO Y RETIRO de cenizas, ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art13Paragraph.Add(new Chunk("solo podrá ser efectuado por el personal autorizado por el director del Proyecto de la Nueva Catedral Metropolitana de Tijuana, ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art13Paragraph.Add(new Chunk("bajo su control y vigilancia, ante la presencia del titular o de quien éste haya designado por escrito como titular y/o cuando medie y/o presente a la administración del columbario PODER NOTARIAL.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art13Paragraph.SetLeading(1, 1);
                Art13Cell.AddElement(Art13Paragraph);
                Art13Cell.Colspan = 5;
                Art13Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art13Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art13Cell);

                //ARTICULO 14
                PdfPCell Art14Cell = new PdfPCell();
                Paragraph Art14Paragraph = new Paragraph();

                Art14Paragraph.Add(new Chunk("Artículo 14. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art14Paragraph.Add(new Chunk("El servicio de depósito de urnas consistirá en lo siguiente: La colocación de la tapa de la cripta, placa grabada con el nombre y fechas correspondientes de nacimiento y deceso de cada persona fallecida depositada en la cripta, este grabado será de acuerdo a las normas y especificaciones aprobadas por la administración del Columbario. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art14Paragraph.SetLeading(1, 1);
                Art14Cell.AddElement(Art14Paragraph);
                Art14Cell.Colspan = 5;
                Art14Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art14Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art14Cell);

                //ARTICULO 15
                PdfPCell Art15Cell = new PdfPCell();
                Paragraph Art15Paragraph = new Paragraph();

                Art15Paragraph.Add(new Chunk("Artículo 15. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art15Paragraph.Add(new Chunk("Cuando un servicio de depósito de urna no pudiera realizarse por causas de fuerza mayor o por caso fortuito, en el lugar escogido por el titular o sus beneficiarios de acuerdo con el contrato original, aun cuando ya se hubieran liquidado los pagos respectivos, la administración señalará la cripta que deba hacerse a fin de no demorar el evento, si así lo disponen los familiares. ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art15Paragraph.Add(new Chunk("Dicha cripta será de carácter “provisional” y no será inferior al valor del contrato original. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art15Paragraph.SetLeading(1, 1);
                Art15Cell.AddElement(Art15Paragraph);
                Art15Cell.Colspan = 5;
                Art15Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art15Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art15Cell);

                //ARTICULO 16
                PdfPCell Art16Cell = new PdfPCell();
                Paragraph Art16Paragraph = new Paragraph();

                Art16Paragraph.Add(new Chunk("Artículo 16. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art16Paragraph.Add(new Chunk("La administración no será responsable respecto a la identidad de la persona de quien se deposita los restos incinerados. La persona o personas que hayan gestionado el servicio de depósito de la urna serán las únicas responsables de la identidad de las personas fallecidas. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art16Paragraph.SetLeading(1, 1);
                Art16Cell.AddElement(Art16Paragraph);
                Art16Cell.Colspan = 5;
                Art16Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art16Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art16Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO SEXTO
                PdfPCell SixthTitle = new PdfPCell(new Phrase("TÍTULO SEXTO \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                SixthTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SixthTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                SixthTitle.Colspan = 3;
                MainTable.AddCell(SixthTitle);

                //DEPOSITO Y RETIRO DE URNAS
                PdfPCell ColumbariumCareTitle = new PdfPCell(new Phrase("CUIDADO DEL COLUMBARIO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ColumbariumCareTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ColumbariumCareTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ColumbariumCareTitle.Colspan = 3;
                MainTable.AddCell(ColumbariumCareTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 17
                PdfPCell Art17Cell = new PdfPCell();
                Paragraph Art17Paragraph = new Paragraph();

                Art17Paragraph.Add(new Chunk("Artículo 17. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art17Paragraph.Add(new Chunk("La Parroquia Iglesia Guadalupe del Rio en Tijuana, A.R., como prestación a la cuota de mantenimiento estipulada en el contrato original en la cláusula 4.1.9, La Asociación garantiza los gastos de mantenimiento del columbario, así como todos los servicios de depósito y extracción de urnas, con los servicios colaterales a estos y los que fueron vendidos según el contrato correspondiente y debidamente firmado por los interesados. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art17Paragraph.SetLeading(1, 1);
                Art17Cell.AddElement(Art17Paragraph);
                Art17Cell.Colspan = 5;
                Art17Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art17Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art17Cell);

                //ARTICULO 18
                PdfPCell Art18Cell = new PdfPCell();
                Paragraph Art18Paragraph = new Paragraph();

                Art18Paragraph.Add(new Chunk("Artículo 18. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art18Paragraph.Add(new Chunk("La administración se compromete a mantener un cuerpo de vigilancia diurna y nocturna del columbario. Su objeto es la preservación del orden de las criptas, así como la iglesia y capillas, incluyendo el edificio administrativo. Sin embargo, la administración no se hace responsable de los daños que pudieran resultar por causas ajenas a su control, y en especial, los que se deriven por caso fortuito o fuerza mayor, como desastres naturales, incendios, huelgas, tumultos, terremotos, ciclones, o de actos u órdenes de las autoridades. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art18Paragraph.SetLeading(1, 1);
                Art18Cell.AddElement(Art18Paragraph);
                Art18Cell.Colspan = 5;
                Art18Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art18Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art18Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);

                //PAGINA 12

                //TÍTULO SÉPTIMO
                PdfPCell SeventhTitle = new PdfPCell(new Phrase("TÍTULO SÉPTIMO \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                SeventhTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SeventhTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                SeventhTitle.Colspan = 3;
                MainTable.AddCell(SeventhTitle);

                //PROHIBICIONES
                PdfPCell ProhibitionsTitle = new PdfPCell(new Phrase("PROHIBICIONES", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ProhibitionsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ProhibitionsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ProhibitionsTitle.Colspan = 3;
                MainTable.AddCell(ProhibitionsTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 19
                PdfPCell Art19Cell = new PdfPCell();
                Paragraph Art19Paragraph = new Paragraph();

                Art19Paragraph.Add(new Chunk("Artículo 19. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art19Paragraph.Add(new Chunk("En la etapa de la cripta solo podrá escribirse el nombre de la persona fallecida con su fecha de nacimiento y fecha de defunción con el tipo de letra autorizado y que todas las placas, serán iguales, quedando excluido cualquier añadido de carácter personal. En la tapa de la cripta no podrán colocarse floreros, veladoras, imágenes, adornos, etc., tanto en la parte exterior, así como en la interior de la misma. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art19Paragraph.SetLeading(1, 1);
                Art19Cell.AddElement(Art19Paragraph);
                Art19Cell.Colspan = 5;
                Art19Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art19Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art19Cell);

                //ARTICULO 20
                PdfPCell Art20Cell = new PdfPCell();
                Paragraph Art20Paragraph = new Paragraph();

                Art20Paragraph.Add(new Chunk("Artículo 20. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art20Paragraph.Add(new Chunk("Se prohíbe la introducción y consumo de bebidas alcohólicas, de droga, cigarros y tóxicos de toda clase o especie dentro del columbario, así como la entrada al mismo de toda persona en estado de ebriedad o intoxicación y se procederá a expulsar a toda aquella persona que con hechos o palabras causare algún escándalo o perjuicio en el interior del columbario. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art20Paragraph.SetLeading(1, 1);
                Art20Cell.AddElement(Art20Paragraph);
                Art20Cell.Colspan = 5;
                Art20Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art20Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art20Cell);

                //ARTICULO 21
                PdfPCell Art21Cell = new PdfPCell();
                Paragraph Art21Paragraph = new Paragraph();

                Art21Paragraph.Add(new Chunk("Artículo 21. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art21Paragraph.Add(new Chunk("Se prohíbe arrojar basura o desperdicios en los pasillos del columbario, lo mismo colocar sobre las tapas de las criptas imágenes, porta floreros, letreros o cualquier otro artículo que ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art21Paragraph.Add(new Chunk("NO ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art21Paragraph.Add(new Chunk("esté autorizado por la administración. La administración colocará recipientes para su depósito en los lugares que estime conveniente. \n",
                  new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art21Paragraph.SetLeading(1, 1);
                Art21Cell.AddElement(Art21Paragraph);
                Art21Cell.Colspan = 5;
                Art21Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art21Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art21Cell);

                //ARTICULO 22
                PdfPCell Art22Cell = new PdfPCell();
                Paragraph Art22Paragraph = new Paragraph();

                Art22Paragraph.Add(new Chunk("Artículo 22. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art22Paragraph.Add(new Chunk("No se permitirá el acceso al columbario a niños menores de diez años sin la compañía de personas mayores que cuiden de ellos. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art22Paragraph.SetLeading(1, 1);
                Art22Cell.AddElement(Art22Paragraph);
                Art22Cell.Colspan = 5;
                Art22Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art22Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art22Cell);

                //ARTICULO 23
                PdfPCell Art23Cell = new PdfPCell();
                Paragraph Art23Paragraph = new Paragraph();

                Art23Paragraph.Add(new Chunk("Artículo 23. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art23Paragraph.Add(new Chunk("Se prohíbe a toda persona ajena a recoger flores ya sean silvestres o cultivadas, o trozar ramaje de los árboles, arbustos o plantas o alimentar a los pájaros u otra forma de vida animal que se encuentre dentro del terreno del columbario. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art23Paragraph.SetLeading(1, 1);
                Art23Cell.AddElement(Art23Paragraph);
                Art23Cell.Colspan = 5;
                Art23Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art23Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art23Cell);

                //ARTICULO 24
                PdfPCell Art24Cell = new PdfPCell();
                Paragraph Art24Paragraph = new Paragraph();

                Art24Paragraph.Add(new Chunk("Artículo 24. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art24Paragraph.Add(new Chunk("No se permitirá dentro del columbario y jardines, el tránsito de bicicletas, motocicletas, patinetas, etc. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art24Paragraph.SetLeading(1, 1);
                Art24Cell.AddElement(Art24Paragraph);
                Art24Cell.Colspan = 5;
                Art24Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art24Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art24Cell);

                //ARTICULO 25
                PdfPCell Art25Cell = new PdfPCell();
                Paragraph Art25Paragraph = new Paragraph();

                Art25Paragraph.Add(new Chunk("Artículo 25. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art25Paragraph.Add(new Chunk("No se permitirá dentro del columbario y jardines o áreas circundantes avisos, letreros, leyendas o anuncios de cualquier índole, excepto por los autorizados por la administración y autoridades competentes. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art25Paragraph.SetLeading(1, 1);
                Art25Cell.AddElement(Art25Paragraph);
                Art25Cell.Colspan = 5;
                Art25Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art25Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art25Cell);

                //ARTICULO 26
                PdfPCell Art26Cell = new PdfPCell();
                Paragraph Art26Paragraph = new Paragraph();

                Art26Paragraph.Add(new Chunk("Artículo 26. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art26Paragraph.Add(new Chunk("No se permitirán el acceso de animales al columbario ni a los jardines. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art26Paragraph.SetLeading(1, 1);
                Art26Cell.AddElement(Art26Paragraph);
                Art26Cell.Colspan = 5;
                Art26Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art26Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art26Cell);

                //ARTICULO 27
                PdfPCell Art27Cell = new PdfPCell();
                Paragraph Art27Paragraph = new Paragraph();

                Art27Paragraph.Add(new Chunk("Artículo 27. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art27Paragraph.Add(new Chunk("No se permitirá el acceso a orquestas, bandas, grupos musicales, radio fusiones, altoparlantes, celulares, dispositivos electrónicos o amplificadores, si no se obtiene permiso de la administración. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art27Paragraph.SetLeading(1, 1);
                Art27Cell.AddElement(Art27Paragraph);
                Art27Cell.Colspan = 5;
                Art27Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art27Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art27Cell);

                //ARTICULO 28
                PdfPCell Art28Cell = new PdfPCell();
                Paragraph Art28Paragraph = new Paragraph();

                Art28Paragraph.Add(new Chunk("Artículo 28. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art28Paragraph.Add(new Chunk("Los visitantes al columbario que utilicen vehículos, los deberán estacionar en las zonas de estacionamiento que para tal efecto estarán marcadas por la administración y de acuerdo con los lineamientos de las autoridades competentes. Si el usuario invade una zona prohibida o que no le corresponde, la administración llamara a la empresa de grúas para levantar el vehículo, y los gastos que se originen serán cubiertos por el propietario o usuario del vehículo. \n",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art28Paragraph.SetLeading(1, 1);
                Art28Cell.AddElement(Art28Paragraph);
                Art28Cell.Colspan = 5;
                Art28Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art28Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art28Cell);

                //ARTICULO 29
                PdfPCell Art29Cell = new PdfPCell();
                Paragraph Art29Paragraph = new Paragraph();

                Art29Paragraph.Add(new Chunk("Artículo 29. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art29Paragraph.Add(new Chunk("Cualquier caso o evento que no estuviere contemplado en este reglamento, y por consecuencia origine duda, problema, contrariedad, perturbación, molestia, etc., será resuelto por el Consejo de la Parroquia Iglesia de Guadalupe del Rio en Tijuana, A.R., o por las autoridades competentes, según sea el caso. (Por mediación a través de un acuerdo entre las partes)",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art29Paragraph.SetLeading(1, 1);
                Art29Cell.AddElement(Art29Paragraph);
                Art29Cell.Colspan = 5;
                Art29Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art29Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art29Cell);

                //ARTICULO 29
                PdfPCell Art30Cell = new PdfPCell();
                Paragraph Art30Paragraph = new Paragraph();

                Art30Paragraph.Add(new Chunk("Artículo 30. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Art30Paragraph.Add(new Chunk("La resolución a la que se llegue, será inapelable y aplicable a todos los adquirientes, o beneficiarios de las criptas.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Art30Paragraph.SetLeading(1, 1);
                Art30Cell.AddElement(Art30Paragraph);
                Art30Cell.Colspan = 5;
                Art30Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art29Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art30Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TITULAR
                PdfPCell FinalSignatureHeader = new PdfPCell(new Phrase("TITULAR", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLD)));
                FinalSignatureHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FinalSignatureHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                FinalSignatureHeader.Colspan = 5;
                LineCell.Colspan = 5;

                MainTable.AddCell(LineCell);
                MainTable.AddCell(FinalSignatureHeader);

                doc.Add(MainTable);

                writer.CloseStream = false;
                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ContratoDeAdhesion.pdf");
                Response.Write(doc);
                Response.Cookies.Add(new HttpCookie(window_contract_id, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ff")));
                Response.End();

            }
            catch (Exception ex)
            {
                string error = ex.Message + " Inner Exception: " + ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home", new { error = error });
            }
            return null;
        }

        //Contract individual crypt
        public ActionResult ContractCommunityCrypt(string window_contract_id)
        {
            try
            {
                HttpCookie purchaseID = Request.Cookies["sPurchaseRequestID"];
                HttpCookie quotation = Request.Cookies["bQuotation"];
                var doc = new Document(iTextSharp.text.PageSize.LETTER);

                PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);

                int PurchaseID = 0;
                if (quotation.Value == "False")
                {
                    PurchaseID = Convert.ToInt32(purchaseID.Value);
                }

                dtoPurchasesRequest dtoPurchasesRequest = PurchasesRequestsB.GetByIDForContract(PurchaseID);

                doc.Open();
                doc.NewPage();

                PdfPTable MainTable = new PdfPTable(3);
                MainTable.TotalWidth = 550f;
                MainTable.LockedWidth = true;
                MainTable.HorizontalAlignment = 0;

                //CELDAS EN BLANCO
                PdfPCell WhiteCell = new PdfPCell(new Paragraph(""));
                WhiteCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                Paragraph Line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 10)));
                PdfPCell LineCell = new PdfPCell();
                LineCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                LineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                LineCell.AddElement(Line);
                LineCell.Colspan = 3;

                //SALTO DE LINEA
                PdfPCell CellRow = new PdfPCell(new Paragraph("\n\n"));
                CellRow.Colspan = 3;
                CellRow.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //COMILLAS PARA TEXTO
                const string quote = "\"";

                //IMAGEN
                PdfPCell ImageTableCell = new PdfPCell();
                PdfPTable ImageTable = new PdfPTable(3);

                string imageURL = Server.MapPath("~/Content/Images/LogoParroquia.png");
                Image img = Image.GetInstance(imageURL);
                img.ScaleAbsolute(50f, 75f);

                PdfPCell ImageCell = new PdfPCell(img);
                ImageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImageCell.HorizontalAlignment = Element.ALIGN_CENTER;

                //INFO CONTRATO
                PdfPCell TitleInfoCell = new PdfPCell();
                Paragraph TitleInfoParagraph = new Paragraph();
                if (quotation.Value == "True")
                {
                    TitleInfoParagraph.Add(new Chunk("CONTRATO No. ------------- \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.Add(new Chunk("SOLICITUD No. ------------- \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.Add(new Chunk("TIPO: PERPETUIDAD. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.SetLeading(1, 1);
                    TitleInfoCell.AddElement(TitleInfoParagraph);
                    TitleInfoCell.Colspan = 5;
                    TitleInfoCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    TitleInfoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    MainTable.AddCell(ImageCell);
                    MainTable.AddCell(WhiteCell);
                    MainTable.AddCell(TitleInfoCell);
                }
                else
                {
                    int zeros = 6 - dtoPurchasesRequest.ContractID.ToString().Length;
                    string ceros = "";
                    for (int i = 0; i < zeros; i++)
                    {
                        ceros += "0";
                    }
                    TitleInfoParagraph.Add(new Chunk("CONTRATO No. C-JC-" + ceros + dtoPurchasesRequest.ContractID + "-A/C\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.Add(new Chunk("SOLICITUD No. " + dtoPurchasesRequest.PurchasesRequestID + "\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.Add(new Chunk("TIPO: PERPETUIDAD. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    TitleInfoParagraph.SetLeading(1, 1);
                    TitleInfoCell.AddElement(TitleInfoParagraph);
                    TitleInfoCell.Colspan = 5;
                    TitleInfoCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    TitleInfoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    MainTable.AddCell(ImageCell);
                    MainTable.AddCell(WhiteCell);
                    MainTable.AddCell(TitleInfoCell);
                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PRIMER PARRAFO
                PdfPCell ParagraphCell = new PdfPCell(new Paragraph("CONTRATO DE ADHESIÓN PARA ENAJENACIÓN SOBRE PRODUCTOS Y SERVICIOS FUNERARIOS DE LOS DERECHOS DE USO DE CRIPTAS, NICHOS U OSARIOS PARA EL DEPÓSITO DE CENIZAS y RESTOS HUMANOS ÁRIDOS MORTUORIOS.",
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphCell.Colspan = 3;
                ParagraphCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ParagraphCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(ParagraphCell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //SEGUNDO PARRAFO
                PdfPCell ParagraphCell2 = new PdfPCell(new Paragraph("En el municipio de Tijuana, Baja California; siendo las ____ horas, del día ____ del mes de ______ estando presentes, las siguientes partes:",
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphCell2.Colspan = 3;
                ParagraphCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ParagraphCell2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(ParagraphCell2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TERCER PARRAFO
                PdfPCell ParagraphCell3 = new PdfPCell(new Paragraph("Por un lado “LA PARROQUIA IGLESIA DE GUADALUPE DEL RÍO EN TIJUANA A.R.” representada por C. PBRO. JESÚS CÁRDENAS NÚÑEZ a quien en lo sucesivo y para efectos de este contrato se le denominara “LA ASOCIACIÓN”.",
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphCell3.Colspan = 3;
                ParagraphCell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ParagraphCell3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(ParagraphCell3);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //CUARTO PARRAFO
                if (quotation.Value == "True")
                {
                    PdfPCell ParagraphCell4 = new PdfPCell(new Paragraph("Y por la otra parte, la (él) C. ______________________ a quien en lo sucesivo se le denominara “EL ADQUIRIENTE”; de conformidad con lo previsto en los artículos " +
                        "1679, 1680, 1681, 1682, 1683, 1685, 1690, 1699, 1700, 1701, 1702, 1703, 1704, 1705, 1706 y demás relativos y aplicables del Código Civil del Estado de Baja California vigente, conviene celebrar el presente contrato al tenor de las siguientes: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphCell4.Colspan = 3;
                    ParagraphCell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ParagraphCell4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(ParagraphCell4);
                }
                else
                {
                    PdfPCell ParagraphCell4 = new PdfPCell(new Paragraph("Y por la otra parte, la (él) C. " + dtoPurchasesRequest.CustomerName + " a quien en lo sucesivo se le denominara “EL ADQUIRIENTE”; de conformidad con lo previsto en los artículos " +
                        "1679, 1680, 1681, 1682, 1683, 1685, 1690, 1699, 1700, 1701, 1702, 1703, 1704, 1705, 1706 y demás relativos y aplicables del Código Civil del Estado de Baja California vigente, conviene celebrar el presente contrato al tenor de las siguientes: ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphCell4.Colspan = 3;
                    ParagraphCell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ParagraphCell4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(ParagraphCell4);

                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECLARACIONES
                PdfPCell DeclarationTitle = new PdfPCell(new Phrase("DECLARACIONES", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                DeclarationTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                DeclarationTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                DeclarationTitle.Colspan = 3;
                MainTable.AddCell(DeclarationTitle);

                //DECLARACIONES 1
                PdfPCell DeclarationCell = new PdfPCell();
                Paragraph DeclarationParagraph = new Paragraph();

                DeclarationParagraph.Add(new Chunk("1. La ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                DeclarationParagraph.Add(new Chunk("“PARROQUIA IGLESIA GUADALUPE DEL RÍO EN TIJUANA, A.R.” ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                DeclarationParagraph.Add(new Chunk("a quien en lo sucesivo se le denominará ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                DeclarationParagraph.Add(new Chunk("“LA ASOCIACIÓN”", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                DeclarationParagraph.Add(new Chunk(", declara lo siguiente: \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                DeclarationParagraph.Add(new Chunk("Que es una Asociación Religiosa inscrita en la Secretaría de Gobernación con registro constitutivo No. SGAR/48:68/95 y que, de igual forma, es una entidad  o división autónoma, " +
                    "dentro de la Asociación Religiosa denominada “Diócesis de Tijuana, A.R.” cuyo registro constitutivo en la Secretaría de Gobernación es el No. SGAR/48/93 y cuyos estatutos han sido protocolizados en la " +
                    "ciudad de Tijuana mediante escritura no. 42,918 del 18 de febrero de 1994 ante la fe del Lic. J. Eduardo Illades Moreno, Notario Público No. 6 de esta ciudad de Tijuana.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                DeclarationParagraph.SetLeading(1, 1);
                DeclarationCell.AddElement(DeclarationParagraph);
                DeclarationCell.Colspan = 5;
                DeclarationCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                DeclarationCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(DeclarationCell);

                //1.1
                PdfPCell Cell1_1 = new PdfPCell();
                Paragraph Paragraph1_1 = new Paragraph();

                Paragraph1_1.Add(new Chunk("1.1.    Que dentro de su objeto social se encuentra, entre otros, el celebrar contratos relativos a la comercialización de productos y servicios para uso inmediato o a futuro, de los derechos de uso de espacios por tiempo determinado " +
                    "y/o a perpetuidad conocidos como criptas, nichos, osarios, o gavetas, para restos mortuorios y en específico aquellos clasificados como restos humanos áridos y cenizas de acuerdo al numeral 5, sub numerales en concordancia 5.2 y 5.2.9 de la ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_1.Add(new Chunk("Norma Oficial Mexicana NOM-036-SCFI-2016, Prácticas comerciales, requisitos de información y disposiciones generales en la prestación de servicios funerarios",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLDITALIC)));
                Paragraph1_1.Add(new Chunk(", publicada el 05 de enero del 2017 en el Diario Oficial de la Federación.",
                 new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_1.SetLeading(1, 1);
                Cell1_1.AddElement(Paragraph1_1);
                Cell1_1.Colspan = 5;
                Cell1_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_1.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_1);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.2
                PdfPCell Cell1_2 = new PdfPCell();
                Paragraph Paragraph1_2 = new Paragraph();

                Paragraph1_2.Add(new Chunk("1.2.    Que de acuerdo a lo dispuesto en la ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_2.Add(new Chunk("Norma Oficial Mexicana NOM-036-SCFI-2016, Prácticas comerciales, requisitos de información y disposiciones generales en la prestación de servicios funerarios; ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.ITALIC)));
                Paragraph1_2.Add(new Chunk("cuenta con la adecuación jurídica correspondiente para los efectos, de donde se colige  y se apega a los dispuesto por la NOM citada en lo que respecta a los elementos informativos y de la publicidad, cumpliendo con los principios de " +
                    "legalidad, honestidad, veracidad, dignidad y libre competencia, entre otros.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_2.SetLeading(1, 1);
                Cell1_2.AddElement(Paragraph1_2);
                Cell1_2.Colspan = 5;
                Cell1_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_2);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.3
                PdfPCell Cell1_3 = new PdfPCell();
                Paragraph Paragraph1_3 = new Paragraph();

                Paragraph1_3.Add(new Chunk("1.3.    En concordancia legal de lo anterior, se desprende que por razones de jurisdicción territorial LA ASOCIACIÓN, cumple con lo dispuesto en su aplicabilidad con el ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_3.Add(new Chunk("Reglamento de Panteones en el Municipio de Tijuana, ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.ITALIC)));
                Paragraph1_3.Add(new Chunk("vigente para los efectos a que haya lugar, derivado del mandato expreso y facultativo hacia los municipios del país constreñido en el artículo 115 de la Constitución Política de los Estados Unidos Mexicanos.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.ITALIC)));
                Paragraph1_3.SetLeading(1, 1);
                Cell1_3.AddElement(Paragraph1_3);
                Cell1_3.Colspan = 5;
                Cell1_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_3.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_3);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.4
                PdfPCell Cell1_4 = new PdfPCell();
                Paragraph Paragraph1_4 = new Paragraph();

                Paragraph1_4.Add(new Chunk("1.4.    Asimismo, LA ASOCIACIÓN cumple con su reglamento interior que norma las acciones inherentes a la prestación de servicios, así como toda aquella regla que le permite a LA ASOCIACIÓN responsabilizarse y lleva a cabo su operación " +
                    "cotidiana de forma interna que pudiera impactar en su vida interior, pero a la vez, frente a terceros (consumidores), por lo que sabedora de ello en su actuar procedió a registrar en su momento el presente contrato ante la Procuraduría Federal del Consumidor " +
                    "toda vez que para los efectos se rige, entre otras legislaciones con la Ley Federal del Consumidor en la parte que le resulta aplicativa.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_4.SetLeading(1, 1);
                Cell1_4.AddElement(Paragraph1_4);
                Cell1_4.Colspan = 5;
                Cell1_4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_4.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_4);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //1.5
                PdfPCell Cell1_5 = new PdfPCell();
                Paragraph Paragraph1_5 = new Paragraph();

                Paragraph1_5.Add(new Chunk("1.5.    De todo lo anteriormente se evidencia que LA ASOCIACIÓN cuenta con personalidad jurídica propia y cumple con la legislación aplicable correspondiente, atribuyéndole las facultades bastas y necesarias para celebrar el presente contrato de adhesión, " +
                    "de acuerdo a la base legal en los artículos 85, 86, 86 BIS, 86 TER de la Ley Federal de Protección al Consumidor. ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1_5.SetLeading(1, 1);
                Cell1_5.AddElement(Paragraph1_5);
                Cell1_5.Colspan = 5;
                Cell1_5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell1_5.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Cell1_5);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECLARACIONES 2
                PdfPCell Declaration2Cell = new PdfPCell();
                Paragraph Declaration2Paragraph = new Paragraph();
                if (quotation.Value == "True")
                {
                    Declaration2Paragraph.Add(new Chunk("LA SRA. (SR.) ____________________con domicilio en la calle ________. No._____ Interior __ en el Colonia: _______ C.P. ______, teléfono _______, de la ciudad de _______ Estado de ____________, país ______, " +
                    "manifiesta que tiene capacidad jurídica para  celebrar el presente contrato, y declara que es su deseo",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Declaration2Paragraph.Add(new Chunk(" adquirir la cesión de derechos de uso mortuorio a perpetuidad de un solo espacio ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Declaration2Paragraph.Add(new Chunk("en el nicho comunitario que se localizará en la nueva Catedral de Tijuana, ubicada en Av. Paseo Centenario No. 10150, Zona Río, Tijuana, B.C. C.P. 22320, ubicada en el ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Declaration2Paragraph.Add(new Chunk("AREA ATRIAL ZONA ____ UBICACIÓN _____  NICHO No. _____ ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Declaration2Paragraph.Add(new Chunk("identificada con la clave No. ___________.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Declaration2Paragraph.SetLeading(1, 1);
                    Declaration2Cell.AddElement(Declaration2Paragraph);
                    Declaration2Cell.Colspan = 5;
                    Declaration2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Declaration2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Declaration2Cell);
                }
                else
                {
                    string phone = "";
                    if (dtoPurchasesRequest.Phone != null)
                    {
                        phone = dtoPurchasesRequest.Phone;
                    }
                    else if (dtoPurchasesRequest.CelPhone != null)
                    {
                        phone = dtoPurchasesRequest.CelPhone;
                    }
                    else
                    {
                        phone = "N/A";
                    }

                    string first4 = dtoPurchasesRequest.Nicho.Substring(0, 4);
                    string fiveTo7 = dtoPurchasesRequest.Nicho.Substring(4, 3);
                    string last5 = dtoPurchasesRequest.Nicho.Substring(8, 5);

                    Declaration2Paragraph.Add(new Chunk("LA SRA. (SR.) ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Declaration2Paragraph.Add(new Chunk(dtoPurchasesRequest.CustomerName,
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Declaration2Paragraph.Add(new Chunk(" con domicilio en la calle ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Declaration2Paragraph.Add(new Chunk(dtoPurchasesRequest.Address + ". No. " + dtoPurchasesRequest.AddressNumber + " Interior " + dtoPurchasesRequest.AdressInteriorNumber + " en el Colonia: " + dtoPurchasesRequest.Neighborhood + " C.P. " + dtoPurchasesRequest.ZipCode + ", teléfono " + phone + ", de la ciudad de " + dtoPurchasesRequest.Town + " Estado de " + dtoPurchasesRequest.State + ", país México, ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Declaration2Paragraph.Add(new Chunk("manifiesta que tiene capacidad jurídica para  celebrar el presente contrato, y declara que es su deseo",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Declaration2Paragraph.Add(new Chunk(" adquirir la cesión de derechos de uso mortuorio a perpetuidad de un solo espacio ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Declaration2Paragraph.Add(new Chunk("en el nicho comunitario que se localizará en la nueva Catedral de Tijuana, ubicada en Av. Paseo Centenario No. 10150, Zona Río, Tijuana, B.C. C.P. 22320, ubicada en el ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Declaration2Paragraph.Add(new Chunk("AREA ATRIAL ZONA " + fiveTo7 + " UBICACIÓN " + dtoPurchasesRequest.Nicho[7] + " NICHO No. " + last5 + " ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Declaration2Paragraph.Add(new Chunk("identificada con la clave No. ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Declaration2Paragraph.Add(new Chunk(dtoPurchasesRequest.Nicho + ".",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Declaration2Paragraph.SetLeading(1, 1);
                    Declaration2Cell.AddElement(Declaration2Paragraph);
                    Declaration2Cell.Colspan = 5;
                    Declaration2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Declaration2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Declaration2Cell);

                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECLARACIONES 3
                PdfPCell Declaration3Cell = new PdfPCell();
                Paragraph Declaration3Paragraph = new Paragraph();

                Declaration3Paragraph.Add(new Chunk("III.   “EL ADQUIRIENTE” manifiesta de manera expresa que conoce el proyecto de la construcción de los nichos y está de acuerdo en la celebración de este contrato, " +
                    "aun cuando la ejecución de tal proyecto no haya sido concluida.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Declaration3Paragraph.SetLeading(1, 1);
                Declaration3Cell.AddElement(Declaration3Paragraph);
                Declaration3Cell.Colspan = 5;
                Declaration3Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Declaration3Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Declaration3Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECLARACIONES 4
                PdfPCell Declaration4Cell = new PdfPCell();
                Paragraph Declaration4Paragraph = new Paragraph();

                Declaration4Paragraph.Add(new Chunk("IV.    “EL ADQUIRIENTE” manifiesta que ha seleccionado UN SOLO ESPACIO del nicho que ampara el derecho de uso mortuorio a perpetuidad y objeto de este contrato, " +
                    "y la ha identificado plenamente de acuerdo a la memoria descriptiva de la cual, “EL ADQUIRIENTE” afirma haber observado plano de distribución donde se localiza el nicho en referencia y que corresponde a la indicada en la ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Declaration4Paragraph.Add(new Chunk("Declaración II.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Declaration4Paragraph.SetLeading(1, 1);
                Declaration4Cell.AddElement(Declaration4Paragraph);
                Declaration4Cell.Colspan = 5;
                Declaration4Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Declaration4Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Declaration4Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECLARACIONES 5
                PdfPCell Declaration5Cell = new PdfPCell();
                Paragraph Declaration5Paragraph = new Paragraph();

                Declaration5Paragraph.Add(new Chunk("V.     El nicho al que se refiere en la Declaración II, tiene una dimensión de 50cm X 40cm X 55cm EXTERIOR (alto X ancho X fondo). En este nicho podrán ser depositadas hasta 5 urnas, ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Declaration5Paragraph.Add(new Chunk("por el cual está de acuerdo en compartirla con otras 4 urnas.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Declaration5Paragraph.SetLeading(1, 1);
                Declaration5Cell.AddElement(Declaration5Paragraph);
                Declaration5Cell.Colspan = 5;
                Declaration5Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Declaration5Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Declaration5Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECLARACIONES 6
                PdfPCell Declaration6Cell = new PdfPCell();
                Paragraph Declaration6Paragraph = new Paragraph();

                Declaration6Paragraph.Add(new Chunk("VI.    “EL ADQUIRIENTE” está de acuerdo en instruir a “EL ADQUIRIENTE” que la urna para depositar sus restos incinerados debe tener una ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Declaration6Paragraph.Add(new Chunk("medida no mayor ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Declaration6Paragraph.Add(new Chunk("a  20cm X 15cm X 15cm (alto X ancho X largo).",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Declaration6Paragraph.SetLeading(1, 1);
                Declaration6Cell.AddElement(Declaration6Paragraph);
                Declaration6Cell.Colspan = 5;
                Declaration6Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Declaration6Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Declaration6Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECLARACIONES 7
                PdfPCell Declaration7Cell = new PdfPCell();
                Paragraph Declaration7Paragraph = new Paragraph();

                Declaration7Paragraph.Add(new Chunk("VII.   Los precios son en pesos mexicanos, moneda nacional, o su equivalente en dólar al tipo de cambio que impere y rija el Banco de México al día en que se realice el pago.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Declaration7Cell.AddElement(Declaration7Paragraph);
                Declaration7Cell.Colspan = 5;
                Declaration7Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Declaration7Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Declaration7Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //LEIDO LO ANTERIOR
                PdfPCell TextCell = new PdfPCell();
                Paragraph TextParagraph = new Paragraph();

                TextParagraph.Add(new Chunk("Leído lo anterior, los contratantes se sujetan a las siguientes: \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                TextCell.AddElement(TextParagraph);
                TextCell.Colspan = 5;
                TextCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                TextCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(TextCell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);
                MainTable.AddCell(CellRow);

                //PERSONALIDAD
                PdfPCell PersonalityTitle = new PdfPCell(new Phrase("PERSONALIDAD", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                PersonalityTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PersonalityTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                PersonalityTitle.Colspan = 3;
                MainTable.AddCell(PersonalityTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PERSONALIDAD PARRAFO 1
                PdfPCell Personality1Cell = new PdfPCell();
                Paragraph Personality1Paragraph = new Paragraph();

                Personality1Paragraph.Add(new Chunk("PARROQUIA IGLESIA GUADALUPE DEL RÍO EN TIJUANA, AR, ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Personality1Paragraph.Add(new Chunk("es representada en este acto por el ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Personality1Paragraph.Add(new Chunk("Pbro. Lic. JESÚS CÁRDENAS NUÑEZ, ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Personality1Paragraph.Add(new Chunk("a quien se le ha concedido el NOMBRAMIENTO COMO PÁRROCO el 24/05/2017 con el uso de facultades ministeriales vigentes en la Arquidiócesis por el Excmo. ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Personality1Paragraph.Add(new Chunk("Dr. DON FRANCISCO MORENO BARRÓN, ARZOBISPO DE TIJUANA, ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Personality1Paragraph.Add(new Chunk("al tenor del canon 523 del Código de Derecho Canónico.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Personality1Paragraph.SetLeading(1, 1);
                Personality1Cell.AddElement(Personality1Paragraph);
                Personality1Cell.Colspan = 5;
                Personality1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Personality1Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Personality1Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PERSONALIDAD PARRAFO 2
                PdfPCell Personality2Cell = new PdfPCell();
                Paragraph Personality2Paragraph = new Paragraph();
                if (quotation.Value == "True")
                {
                    Personality2Paragraph.Add(new Chunk("“EL ADQUIRIENTE,” ____________ ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Personality2Paragraph.Add(new Chunk("- es originario (a) de _________. Donde nació el ____ del mes de ______ en el año de _____, " +
                        "estado civil, _________, de profesión ________ y con domicilio manifestado en la Declaración II de este contrato.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Personality2Cell.AddElement(Personality2Paragraph);
                    Personality2Cell.Colspan = 5;
                    Personality2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Personality2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Personality2Cell);
                }
                else
                {
                    Personality2Paragraph.Add(new Chunk("“EL ADQUIRIENTE,” " + dtoPurchasesRequest.CustomerName + " ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Personality2Paragraph.Add(new Chunk("- es originario (a) de ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Personality2Paragraph.Add(new Chunk(dtoPurchasesRequest.CityOfBirth,
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Personality2Paragraph.Add(new Chunk(". Donde nació ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Personality2Paragraph.Add(new Chunk(dtoPurchasesRequest.DateOfBirth.ToString("'el 'dd' de 'MMMM' del año 'yyyy"),
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Personality2Paragraph.Add(new Chunk(", estado civil, ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Personality2Paragraph.Add(new Chunk(dtoPurchasesRequest.CivilStatus,
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Personality2Paragraph.Add(new Chunk(", de profesión ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Personality2Paragraph.Add(new Chunk(dtoPurchasesRequest.Occupation,
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    Personality2Paragraph.Add(new Chunk(" y con domicilio manifestado en la Declaración II de este contrato.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    Personality2Cell.AddElement(Personality2Paragraph);
                    Personality2Cell.Colspan = 5;
                    Personality2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Personality2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(Personality2Cell);
                }


                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //CLAUSULAS
                PdfPCell ClausesTitle = new PdfPCell(new Phrase("CLAUSULAS", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ClausesTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ClausesTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ClausesTitle.Colspan = 3;
                MainTable.AddCell(ClausesTitle);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PRIMERA
                PdfPCell CellFirst = new PdfPCell();
                Paragraph ParagraphFirst = new Paragraph();

                ParagraphFirst.Add(new Chunk("PRIMERA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphFirst.Add(new Chunk("“LA ASOCIACIÓN” cede y “EL ADQUIRIENTE” obtiene el derecho de uso mortuorio de un espacio del nicho comunitario a perpetuidad en la Nueva Catedral de Tijuana, cuyo domicilio se describe en la ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphFirst.Add(new Chunk("Declaración II ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphFirst.Add(new Chunk(", y en el cual se llevará a cabo su construcción.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphFirst.SetLeading(1, 1);
                CellFirst.AddElement(ParagraphFirst);
                CellFirst.Colspan = 5;
                CellFirst.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellFirst.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellFirst);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //SEGUNDA
                PdfPCell CellSecond = new PdfPCell();
                Paragraph ParagraphSecond = new Paragraph();
                if (quotation.Value == "True")
                {
                    ParagraphSecond.Add(new Chunk("SEGUNDA. - ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphSecond.Add(new Chunk("- El precio de la presente operación será de contado por la cantidad de ",
                       new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParagraphSecond.Add(new Chunk("$______ pesos mexicanos (________________________ PESOS MXN 00/100) ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphSecond.Add(new Chunk("pagaderos en una sola exhibición amparado con la factura No. ______de fecha ___ de ________ del _____, por la cantidad de ",
                       new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParagraphSecond.Add(new Chunk("$______ pesos mexicanos (________________________ PESOS MXN 00/100). \n",
                       new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphSecond.Add(new Chunk("Pero además se efectúo el pago de cuota de Mantenimiento Anual por el período de ________ a ________ por la cantidad de ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParagraphSecond.Add(new Chunk("$______ pesos (________________________ PESOS MXN 00/100). ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphSecond.Add(new Chunk("Indicado en la misma factura.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParagraphSecond.SetLeading(1, 1);
                    CellSecond.AddElement(ParagraphSecond);
                    CellSecond.Colspan = 5;
                    CellSecond.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellSecond.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(CellSecond);
                }
                else
                {
                    tblMaintenanceFee tblMaintenance = MaintenanceFeeB.GetLastReccord(0);

                    string originalPriceText = ConvertirNumeroALetras(dtoPurchasesRequest.OriginalPrice);
                    string cryptPriceText = ConvertirNumeroALetras(dtoPurchasesRequest.CryptPrice);
                    string maintenanceFee = ConvertirNumeroALetras(tblMaintenance.Cost);
                    string formattedDate = dtoPurchasesRequest.CreatedDate.AddYears(1).ToString("dd/MM/yyyy");

                    ParagraphSecond.Add(new Chunk("SEGUNDA. - ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphSecond.Add(new Chunk("- El precio de la presente operación será de contado por la cantidad de ",
                       new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParagraphSecond.Add(new Chunk("$" + dtoPurchasesRequest.CryptPrice + " pesos mexicanos (" + cryptPriceText + ") ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphSecond.Add(new Chunk("pagaderos en una sola exhibición amparado con la factura No. ______ de fecha " + dtoPurchasesRequest.CreatedDate.ToString("dd' de 'MMMM' del año 'yyyy") + ", por la cantidad de ",
                       new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParagraphSecond.Add(new Chunk("$" + dtoPurchasesRequest.CryptPrice + " pesos mexicanos (" + cryptPriceText + "). \n",
                       new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphSecond.Add(new Chunk("Pero además se efectúo el pago de cuota de Mantenimiento Anual por el período de " + dtoPurchasesRequest.CreatedDate.ToString("dd/MM/yyyy") + " a " + formattedDate + " por la cantidad de ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParagraphSecond.Add(new Chunk(tblMaintenance.Cost.ToString("C2") + " pesos (" + maintenanceFee + "). ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                    ParagraphSecond.Add(new Chunk("Indicado en la misma factura.",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    ParagraphSecond.SetLeading(1, 1);
                    CellSecond.AddElement(ParagraphSecond);
                    CellSecond.Colspan = 5;
                    CellSecond.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellSecond.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(CellSecond);
                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TERCERA
                PdfPCell CellThird = new PdfPCell();
                Paragraph ParagraphThird = new Paragraph();

                ParagraphThird.Add(new Chunk("TERCERA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphThird.Add(new Chunk("“EL ADQUIRIENTE” pagará por su exclusiva cuenta, todos los gastos, derechos e impuestos, servicios Federales, Estatales o Municipales que se llegaran a causar con motivo de la celebración de este contrato.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphThird.SetLeading(1, 1);
                CellThird.AddElement(ParagraphThird);
                CellThird.Colspan = 5;
                CellThird.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellThird.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellThird);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //CUARTA
                PdfPCell CellFourth = new PdfPCell();
                Paragraph ParagraphFourth = new Paragraph();

                ParagraphFourth.Add(new Chunk("CUARTA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphFourth.Add(new Chunk("“LA ASOCIACIÓN” como contraprestación al precio convenido en su Cláusula Segunda, se compromete a proporcionar a “EL ADQUIRIENTE” lo siguiente: \n " +
                    "A)  El derecho de uso mortuorio a perpetuidad de un solo espacio en el nicho descrita en la Declaración II de este   contrato. \n " +
                    "B)   Proporcionar las facilidades y auxilio en la colocación o extracción de urnas.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphFourth.SetLeading(1, 1);
                CellFourth.AddElement(ParagraphFourth);
                CellFourth.Colspan = 5;
                CellFourth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellFourth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellFourth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //QUINTA
                PdfPCell CellFifth = new PdfPCell();
                Paragraph ParagraphFifth = new Paragraph();

                ParagraphFifth.Add(new Chunk("QUINTA. - PARA EL DEPÓSITO DE URNAS SERÁ ESTRICTAMENTE NECESARIO LO SIGUIENTE: \n",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphFifth.Add(new Chunk("“LA ASOCIACIÓN” como contraprestación al precio convenido en su Cláusula Segunda, se compromete a proporcionar a “EL ADQUIRIENTE” lo siguiente: \n " +
                    "A) Las urnas deberán contener restos humanos totalmente secos, áridos e incinerados. \n " +
                    "B) Los encomendados, mismos que se enuncian en la Cláusula Décima Segunda deberán presentar el acta de defunción, el comprobante de cremación de los restos expedido por la funeraria, cuando menos dentro de las 72 horas del día anterior a la fecha en que se requiera el uso del nicho.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphFifth.SetLeading(1, 1);
                CellFifth.AddElement(ParagraphFifth);
                CellFifth.Colspan = 5;
                CellFifth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellFifth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellFifth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //SEXTA
                PdfPCell CellSixth = new PdfPCell();
                Paragraph ParagraphSixth = new Paragraph();

                ParagraphSixth.Add(new Chunk("SEXTA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphSixth.Add(new Chunk("“EL ADQUIRIENTE” está de acuerdo que la colocación y/o extracción de urnas, sea función única y exclusivamente de personal autorizado por “LA ASOCIACIÓN” previa solicitud, y para tal efecto el o los encomendados deberán mostrar, el presente contrato.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphSixth.SetLeading(1, 1);
                CellSixth.AddElement(ParagraphSixth);
                CellSixth.Colspan = 5;
                CellSixth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellSixth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellSixth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //SEPTIMA
                PdfPCell CellSeventh = new PdfPCell();
                Paragraph ParagraphSeventh = new Paragraph();

                ParagraphSeventh.Add(new Chunk("SEPTIMA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphSeventh.Add(new Chunk("“EL ADQUIRIENTE” está de acuerdo en acatar los siguientes impedimentos: \n " +
                    "A) No sobreponer ningún tipo de ornamentos, floreros, placas de imagen con fotografía, y otras inscripciones que no sean acordes al lugar donde se encuentran depositados los restos de su familiar. \n" +
                    "B) La cubierta del nicho no podrá ser reemplazada por otro material distinto al que instalara “LA ASOCIACIÓN”, y solo se escribirán los nombres con la fecha de nacimiento y fallecimiento de las personas cuyos restos estarán ahí depositados. \n" +
                    "C) Visitar el nicho fuera de los horarios que para tal efecto tendrá establecido “LA ASOCIACIÓN”. \n" +
                    "D)	Depositar urnas vacías.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphSeventh.SetLeading(1, 1);
                CellSeventh.AddElement(ParagraphSeventh);
                CellSeventh.Colspan = 5;
                CellSeventh.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellSeventh.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellSeventh);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //OCATAVA
                PdfPCell CellEighth = new PdfPCell();
                Paragraph ParagraphEighth = new Paragraph();

                ParagraphEighth.Add(new Chunk("OCATAVA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphEighth.Add(new Chunk("“LA ASOCIACIÓN” podrá rescindir el presente contrato o dar por anticipado su vencimiento cuando “EL ADQUIRIENTE” ha incumplido cuales quiera de las obligaciones estipuladas en este contrato.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphEighth.SetLeading(1, 1);
                CellEighth.AddElement(ParagraphEighth);
                CellEighth.Colspan = 5;
                CellEighth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellEighth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellEighth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //NOVENA
                PdfPCell CellNineth = new PdfPCell();
                Paragraph ParagraphNineth = new Paragraph();

                ParagraphNineth.Add(new Chunk("NOVENA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphNineth.Add(new Chunk("Como consecuencia de la cancelación del presente contrato por incurrir en violaciones a la Cláusula Séptima y si el “ADQURIENTE”, por voluntad propia llegara a cancelar el contrato, “LA ASOCIACIÓN” no devolverá a “EL ADQUIRIENTE” " +
                    "ningún reembolso, ese importe se aplique como pena a favor de “LA ASOCIACIÓN”, por daños y perjuicios causados a ésta, debido al incumplimiento por parte de “EL ADQUIRIENTE”, de las obligaciones de pago contraídas en el presente contrato, todo esto sin considerar " +
                    "las cantidades cubiertas por impuestos, derechos, intereses moratorios, o cualquier otra cantidad diferente de lo señalado en la Cláusula Segunda.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphNineth.SetLeading(1, 1);
                CellNineth.AddElement(ParagraphNineth);
                CellNineth.Colspan = 5;
                CellNineth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellNineth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellNineth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECIMA
                PdfPCell CellTenth = new PdfPCell();
                Paragraph ParagraphTenth = new Paragraph();

                ParagraphTenth.Add(new Chunk("DECIMA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphTenth.Add(new Chunk("“EL ADQUIRIENTE” no podrá ceder los derechos que obtiene en el presente contrato ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphTenth.Add(new Chunk(" salvo autorización expresa y por escrito de “LA ASOCIACIÓN”.",
                  new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphTenth.SetLeading(1, 1);
                CellTenth.AddElement(ParagraphTenth);
                CellTenth.Colspan = 5;
                CellTenth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellTenth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellTenth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECIMA PRIMERA
                PdfPCell CellEleventh = new PdfPCell();
                Paragraph ParagraphEleventh = new Paragraph();

                ParagraphEleventh.Add(new Chunk("DECIMA PRIMERA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphEleventh.Add(new Chunk("“EL ADQUIRIENTE” se compromete a dar aviso a “LA ASOCIACIÓN” de su cambio de domicilio, número telefónico dentro de un plazo que no excederá de 15 (quince) días naturales a partir de la fecha en que ocurra, " +
                    "con acuse de recibo por parte de “LA ASOCIACION”, así como en cada ocasión que “EL ADQUIRIENTE” cambie de domicilio. El incumplimiento a la presente Cláusula por parte de “EL ADQUIRIENTE”, liberará a” LA ASOCIACIÓN” de sus obligaciones inherentes al presente contrato.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphEleventh.SetLeading(1, 1);
                CellEleventh.AddElement(ParagraphEleventh);
                CellEleventh.Colspan = 5;
                CellEleventh.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellEleventh.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellEleventh);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECIMA SEGUNDA
                PdfPCell CellTwelfth = new PdfPCell();
                Paragraph ParagraphTwelfth = new Paragraph();

                ParagraphTwelfth.Add(new Chunk("DECIMA SEGUNDA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphTwelfth.Add(new Chunk("El pago de todos los gastos, derechos e impuestos, que se originen con motivo de la celebración del presente contrato, su ejecución, rescisión, " +
                    "y demás consecuencias serán por cuenta de “EL ADQUIRIENTE”.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphTwelfth.SetLeading(1, 1);
                CellTwelfth.AddElement(ParagraphTwelfth);
                CellTwelfth.Colspan = 5;
                CellTwelfth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellTwelfth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellTwelfth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECIMA TERCERA
                PdfPCell CellThirteenth = new PdfPCell();
                Paragraph ParagraphThirteenth = new Paragraph();

                ParagraphThirteenth.Add(new Chunk("DECIMA TERCERA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphThirteenth.Add(new Chunk("“LA ASOCIACIÓN” se reserva el derecho de verificar el contenido de las urnas a depositar, con el propósito de verificar precisamente que su " +
                    "contenido sea el de restos humanos secos e incinerados.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphThirteenth.SetLeading(1, 1);
                CellThirteenth.AddElement(ParagraphThirteenth);
                CellThirteenth.Colspan = 5;
                CellThirteenth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellThirteenth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellThirteenth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECIMA CUARTA
                PdfPCell CellFourteenth = new PdfPCell();
                Paragraph ParagraphFourteenth = new Paragraph();

                ParagraphFourteenth.Add(new Chunk("DECIMA CUARTA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphFourteenth.Add(new Chunk("Con el propósito de proporcionar el servicio de depósito de urnas en los nichos, “LA ASOCIACION” " +
                    "dispone de nichos que forman parte de la zona atrial del columbario que se encuentra en el predio de “LA ASOCIACIÓN”.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphFourteenth.SetLeading(1, 1);
                CellFourteenth.AddElement(ParagraphFourteenth);
                CellFourteenth.Colspan = 5;
                CellFourteenth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellFourteenth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellFourteenth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECIMA QUINTA
                PdfPCell CellFifteenth = new PdfPCell();
                Paragraph ParagraphFifteenth = new Paragraph();

                ParagraphFifteenth.Add(new Chunk("DECIMA QUINTA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphFifteenth.Add(new Chunk("“EL ADQUIRIENTE” está de acuerdo en realizar un pago anual por la cantidad de $ 1,000.00 pesos (UN MIL PESOS MXN/100) por tiempo indefinido, a partir de la fecha de compra, con el propósito de ",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphFifteenth.Add(new Chunk("garantizar el mantenimiento vitalicio ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphFifteenth.Add(new Chunk("de la totalidad del área de nichos.",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphFifteenth.SetLeading(1, 1);
                CellFifteenth.AddElement(ParagraphFifteenth);
                CellFifteenth.Colspan = 5;
                CellFifteenth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellFifteenth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellFifteenth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //DECIMA SEXTA
                PdfPCell CellSixteenth = new PdfPCell();
                Paragraph ParagraphSixteenth = new Paragraph();

                ParagraphSixteenth.Add(new Chunk("DECIMA SEXTA. - ",
                    new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ParagraphSixteenth.Add(new Chunk("En el caso de existir controversia relativa al contenido o alcances de este contrato, las partes se someten expresamente a los Tribunales Judiciales establecidos en esta ciudad de Tijuana, B.C. renunciando a cualquier fuero que por razón de domicilio presente o futuro pudiese corresponderle.",
                   new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphSixteenth.SetLeading(1, 1);
                CellSixteenth.AddElement(ParagraphSixteenth);
                CellSixteenth.Colspan = 5;
                CellSixteenth.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CellSixteenth.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(CellSixteenth);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //EL PRESENTE CONTRATO
                PdfPCell TextCell2 = new PdfPCell();
                Paragraph TextParagraph2 = new Paragraph();
                if (quotation.Value == "True")
                {
                    TextParagraph2.Add(new Chunk("El presente contrato se firma en dos tantos, en la ciudad de Tijuana, B. C. el día _____ de ______          de   ______. ",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    TextCell2.AddElement(TextParagraph2);
                    TextCell2.Colspan = 5;
                    TextCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    TextCell2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(TextCell2);
                }
                else
                {
                    TextParagraph2.Add(new Chunk("El presente contrato se firma en dos tantos, en la ciudad de Tijuana, B. C. el día " + dtoPurchasesRequest.CreatedDate.ToString("dd' de 'MMMM' de 'yyyy") + ".",
                        new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                    TextCell2.AddElement(TextParagraph2);
                    TextCell2.Colspan = 5;
                    TextCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    TextCell2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    MainTable.AddCell(TextCell2);
                }

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                PdfPCell MiddleSignatureCell = new PdfPCell();
                PdfPTable MiddleSignatureTable = new PdfPTable(2);

                Paragraph ParagraphMiddleSing1 = new Paragraph();
                Paragraph ParagraphMiddleSing2 = new Paragraph();

                ParagraphMiddleSing1.Add(new Chunk("Pbro. JESUS CARDENAS NUÑEZ \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphMiddleSing1.Add(new Chunk("“LA ASOCIACIÓN”  \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));

                ParagraphMiddleSing2.Add(new Chunk("____________________________ \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphMiddleSing2.Add(new Chunk("EL ADQUIRIENTE   \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));

                PdfPCell SignCell1 = new PdfPCell();
                PdfPCell SignCell2 = new PdfPCell();

                SignCell1.AddElement(ParagraphMiddleSing1);
                SignCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SignCell1.HorizontalAlignment = Element.ALIGN_CENTER;

                SignCell2.AddElement(ParagraphMiddleSing2);
                SignCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SignCell2.HorizontalAlignment = Element.ALIGN_CENTER;

                MiddleSignatureTable.AddCell(SignCell1);
                MiddleSignatureTable.AddCell(SignCell2);

                MiddleSignatureCell.AddElement(MiddleSignatureTable);
                MiddleSignatureCell.Colspan = 5;
                MiddleSignatureCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MainTable.AddCell(MiddleSignatureCell);

                doc.Add(MainTable);

                writer.CloseStream = false;
                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ContratoDeAdhesion.pdf");
                Response.Write(doc);
                Response.Cookies.Add(new HttpCookie(window_contract_id, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ff")));
                Response.End();
            }
            catch (Exception ex)
            {
                string error = ex.Message + " Inner Exception: " + ex.InnerException.InnerException.Message;
                return RedirectToAction("Index", "Home", new { error = error });
            }
            return null;
        }

        #endregion

        public ActionResult PurchaseQuotation(string CryptKey = "", int CryptSectionID = 0, int CryptType = 0)
        {
            bool bCheckInPurchaseRequest = PurchasesRequestsB.GetByCryptKeyAndSectionID(CryptKey, CryptSectionID, CryptType);
            return Json(bCheckInPurchaseRequest, JsonRequestBehavior.AllowGet);
        }

        public static string ConvertirNumeroALetras(decimal numero)
        {
            string[] unidades = { "", "UN", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE" };
            string[] decenas = { "", "DIEZ", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
            string[] centenas = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };

            if (numero == 0)
            {
                return "CERO";
            }

            string cantidadTexto = string.Empty;

            // Obtener la parte entera del número
            long parteEntera = (long)numero;

            // Obtener los centavos (parte decimal)
            int centavos = (int)((numero - parteEntera) * 100);

            // Convertir la parte entera a texto
            if (parteEntera > 0)
            {
                cantidadTexto = ConvertirEnteroALetras(parteEntera);
            }

            // Convertir los centavos a texto
            if (centavos > 0)
            {
                cantidadTexto += " CON " + ConvertirEnteroALetras(centavos) + " CENTAVOS";
            }

            cantidadTexto += " PESOS MXN/100";

            return cantidadTexto.ToUpper();
        }

        public static string ConvertirEnteroALetras(long numero)
        {
            string[] unidades = { "", "UN", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE" };
            string[] decenas = { "", "DIEZ", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
            string[] centenas = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };

            if (numero < 10)
            {
                return unidades[numero];
            }
            else if (numero < 20)
            {
                return unidades[numero - 10] + " DIECI";
            }
            else if (numero < 100)
            {
                return decenas[numero / 10] + (numero % 10 > 0 ? " Y " + unidades[numero % 10] : "");
            }
            else if (numero < 1000)
            {
                return centenas[numero / 100] + (numero % 100 > 0 ? " " + ConvertirEnteroALetras(numero % 100) : "");
            }
            else if (numero < 1000000)
            {
                return ConvertirEnteroALetras(numero / 1000) + " MIL" + (numero % 1000 > 0 ? " " + ConvertirEnteroALetras(numero % 1000) : "");
            }
            else if (numero < 1000000000)
            {
                return ConvertirEnteroALetras(numero / 1000000) + " MILLON" + (numero % 1000000 > 0 ? " " + ConvertirEnteroALetras(numero % 1000000) : "");
            }
            else
            {
                // Solo se soportan números hasta mil millones, ajustar según necesidad
                throw new ArgumentException("El número es demasiado grande para convertirlo a letras.");
            }
        }
    }
}