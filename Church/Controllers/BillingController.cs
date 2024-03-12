using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize(Roles = "Facturacion,Administrador")]
    public class BillingController : BaseController
    {
        CustomersB CustomersB = new CustomersB();
        PurchasesRequestsB PurchasesRequestsB = new PurchasesRequestsB();
        CryptsSectionsB CryptsSectionsB = new CryptsSectionsB();
        BeneficiaryCustomersB BeneficiaryCustomersB = new BeneficiaryCustomersB();
        StatesB StatesB = new StatesB();
        TownsB TownsB = new TownsB();
        HelpMethods hm = new HelpMethods();
        HelperAzure HelperAzure = new HelperAzure();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        // GET: Billing
        public ActionResult Solicitudes()
        {
            var PurchasesRequest = PurchasesRequestsB.GetPurchasesRequestNotCheck();
            for (int i = 0; i < PurchasesRequest.Count; i++)
            {
                PurchasesRequest[i].tblCryptsSections = CryptsSectionsB.GetById(PurchasesRequest[i].CryptSectionID);
            }
            return View(PurchasesRequest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Solicitudes(bool[] Check, int[] PurchasesRequestID)
        {
            var ListPurchasesRequest = PurchasesRequestsB.GetPurchasesRequestNotCheck();

            for (int i = 0; i < PurchasesRequestID.Length; i++)
            {
                var PurchasesRequest = ListPurchasesRequest.Where(r => r.PurchasesRequestID == PurchasesRequestID[i]).FirstOrDefault();
                PurchasesRequest.BillingChecked = Check[i];
                PurchasesRequestsB.Update(PurchasesRequest);
            }
            return RedirectToAction("Facturacion");
        }
        public ActionResult Facturacion()
        {
            var PurchasesRequest = PurchasesRequestsB.GetPurchasesRequestCheckAndNoBillingNumber();
            return View(PurchasesRequest);
        }

        public ActionResult Factura(int id)
        {
            var PurchasesRequest = PurchasesRequestsB.GetByID(id);
            tblCustomers tblCustomers = CustomersB.GetCustomersByID(PurchasesRequest.CustomerID);
            PurchasesRequest.RFCCURP = tblCustomers.RFCCURP;
            PurchasesRequest.Customer = tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname;
            return View(new Tuple<tblPurchasesRequests, tblCustomers>(PurchasesRequest, tblCustomers));
        }

        public ActionResult SearchArchive(string billpath)
        {
            string pathbill = "";
            string url = Path.Combine("C:\\ChurchArchive", billpath);
            char[] delimiterChars = { '.' };

            string base64ImageRepresentation = "";
            string base64StringPDF = "";
            string respuesta = "";

            byte[] imageArray;

            byte[] buffer;

            if (url.ToLower().Contains("pdf"))
            {
                try
                {
                    using (Stream stream = new System.IO.FileStream(url, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        buffer = new byte[stream.Length - 1];
                        stream.Read(buffer, 0, buffer.Length);
                    }

                    base64StringPDF = Convert.ToBase64String(buffer, 0, buffer.Length);

                    if (url != null && url.Length > 0)
                    {
                        base64StringPDF = "data:application/pdf;base64," + base64StringPDF;
                        //pathbill = "/Billing/Download/?File=" + billpath + "";
                        pathbill = "/Parroquia/Billing/Download/?File=" + billpath + "";
                    }
                }
                catch (Exception ex)
                {
                    respuesta = "No hay archivo";
                }
            }
            else
            {
                try
                {
                    imageArray = System.IO.File.ReadAllBytes(url);
                    base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    if (url != null && url.Length > 0)
                    {
                        if (url.ToLower().Contains("jpeg"))
                        {
                            base64ImageRepresentation = "data:image/jpeg;base64," + base64ImageRepresentation;
                            pathbill = "/Billing/Download/?File=" + billpath + "";
                        }
                        else if (url.ToLower().Contains("jpg"))
                        {
                            base64ImageRepresentation = "data:image/jpg;base64," + base64ImageRepresentation;
                            pathbill = "/Billing/Download/?File=" + billpath + "";
                        }
                        else if (url.ToLower().Contains("png"))
                        {
                            base64ImageRepresentation = "data:image/png;base64," + base64ImageRepresentation;
                            pathbill = "/Billing/Download/?File=" + billpath + "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    respuesta = "No hay archivo";
                }
            }

            return Json(new
            {
                respuesta = respuesta,
                base64ImageRepresentation = base64ImageRepresentation,
                base64StringPDF = base64StringPDF,
                pathbill = pathbill
            });
        }

        public FileResult Download(string file)
        {
            return File("C:/ChurchArchive/" + file + "", System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        [HttpPost]
        public ActionResult Factura(int id, int BillingNumber, HttpPostedFileBase Billing)
        {
            var PurchasesRequest = PurchasesRequestsB.GetByID(id);
            tblCustomers tblCustomers = CustomersB.GetCustomersByID(PurchasesRequest.CustomerID);
            if (hm.CheckIfExists("BillingNumber", Convert.ToString(BillingNumber), "PurchasesRequestID", id, "tblPurchasesRequests"))
            {
                ModelState.AddModelError("", "El número de factura ya existe");
                //return View(PurchasesRequest);
                return View(new Tuple<tblPurchasesRequests, tblCustomers>(PurchasesRequest, tblCustomers));
            }
            else
            {
                //tblCustomers tblCustomers = CustomersB.GetCustomersByID(PurchasesRequest.CustomerID);
                tblCustomers = CustomersB.GetCustomersByID(PurchasesRequest.CustomerID);
                PurchasesRequest.RFCCURP = tblCustomers.RFCCURP;
                PurchasesRequest.Customer = tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname;

                PurchasesRequest.BillingNumber = BillingNumber;
                string File = HelperAzure.SaveFileBilling(Billing, BillingNumber);
                PurchasesRequest.Billing = File;

                PurchasesRequestsB.Update(PurchasesRequest);

                return RedirectToAction("Facturacion");
            }
        }
        public ActionResult SolicitudDetails(int id)
        {

            tblPurchasesRequests tblPurchasesRequests = PurchasesRequestsB.GetByID(id);
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
            tblPurchasesRequests.CivilStatusID = tblCustomers.CivilStatusID;
            tblPurchasesRequests.Occupation = tblCustomers.Occupation;
            tblPurchasesRequests.Company = tblCustomers.Company;
            tblPurchasesRequests.PhoneCompany = tblCustomers.PhoneCompany;
            tblPurchasesRequests.AddressCompany = tblCustomers.AddressCompany;
            tblPurchasesRequests.ExtPhoneCompany = tblCustomers.ExtPhoneCompany;
            tblPurchasesRequests.TownAddressCompanyID = tblCustomers.TownAddressCompanyID;
            tblPurchasesRequests.Income = tblCustomers.Income;
            tblPurchasesRequests.TownID = tblCustomers.TownID;
            tblPurchasesRequests.UserID = tblCustomers.UserID;
            tblPurchasesRequests.Town = TownsB.GetTownsByID((int)tblPurchasesRequests.TownID).Name;
            tblPurchasesRequests.State = StatesB.GetStatesByID(tblCustomers.StateID).Name;

            tblPurchasesRequests.tblCryptsSections = CryptsSectionsB.GetById(tblPurchasesRequests.CryptSectionID);

            ViewBag.States = StatesB.GetList();
            ViewBag.Towns = TownsB.GetListByStateID(tblCustomers.StateID);
            ViewBag.BeneficiaryCustomers = BeneficiaryCustomersB.GetListByPurchasesRequestID(tblPurchasesRequests.PurchasesRequestID);
            return View(tblPurchasesRequests);
        }
    }
}