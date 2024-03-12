using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Church.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Ventas,Encargado,Facturacion")]
    public class AshDepositsController : BaseController
    {

        AshDepositsB AshDepositsB = new AshDepositsB();
        HelpMethods hm = new HelpMethods();
        PurchasesRequestsB PurchasesRequestsB = new PurchasesRequestsB();
        FederalTaxB FederalTaxB = new FederalTaxB();
        HelperAzure HelperAzure = new HelperAzure();
        CustomersB CustomersB = new CustomersB();
        PurchasesRequestsAshDepositsB PurchasesRequestsAshDepositsB = new PurchasesRequestsAshDepositsB();
        CryptsSectionsB CryptsSectionsB = new CryptsSectionsB();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

        public ActionResult Index(int id)
        {
            var tblAshDeposits = AshDepositsB.GetListByPurchasesRequestID(id);
            tblPurchasesRequests tblPurchasesRequests = PurchasesRequestsB.GetByID(id);
            ViewBag.Nicho = tblPurchasesRequests.Nicho;
            ViewBag.PurchasesRequestID = id;
            ViewBag.CryptType = PurchasesRequestsB.GetByID(id).CryptType.ToString();
            return View(tblAshDeposits);
        }
        public ActionResult IndexCalendar(string Sorting_Order = "", string Date = "", string AshDepositDate = "", int page = 1)
        {
            //Parsing Dates
            DateTime dtAshDeposits = new DateTime();
            if (Date != "" || AshDepositDate != "")
            {
                if (AshDepositDate != "")
                    dtAshDeposits = DateTime.Parse(AshDepositDate);
                else
                    dtAshDeposits = DateTime.Parse(Date);
            }

            ViewBag.Date = dtAshDeposits.ToShortDateString();

            //Sortings
            ViewBag.Sorting_Order = Sorting_Order;

            //Selects lists
            var AshDeposits = AshDepositsB.GetAshDeposits(dtAshDeposits, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_IndexCalendar", AshDeposits);
            }
            return View(AshDeposits);
        }
        public ActionResult Formato(int id)
        {
            var List = AshDepositsB.GetListByPurchasesRequestID(id);

            string CryptType = PurchasesRequestsB.GetByID(id).CryptType.ToString();
            if ((List.Count == 1 && CryptType == "Individual") || (List.Count == 4 && CryptType == "Familiar"))
                return RedirectToAction("Index", new { id = id });
            tblAshDeposits tblAshDeposits = new tblAshDeposits();
            tblAshDeposits.Active = true;
            ViewBag.Towns = new List<tblTowns>();
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblPurchasesRequests tblPurchasesRequests = PurchasesRequestsB.GetByID(id);
            tblCustomers tblCustomers = CustomersB.GetCustomersByID(tblPurchasesRequests.CustomerID);

            tblAshDeposits.Customer = tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname;
            tblAshDeposits.PurchasesRequestID = id;
            tblAshDeposits.CustomerID = tblCustomers.CustomerID;
            tblAshDeposits.Date = ServerDateTimeNow;
            tblAshDeposits.DeathDate = ServerDateTimeNow;
            tblAshDeposits.AshDepositDate = ServerDateTimeNow;
            tblAshDeposits.BirthDate = ServerDateTimeNow;
            tblAshDeposits.FederalTaxID = tblPurchasesRequests.FederalTaxID != 0 ? tblPurchasesRequests.FederalTaxID : FederalTaxB.GetLastReccord().FederalTaxID;
            tblAshDeposits.FederalTax = tblPurchasesRequests.FederalTax != 0 ? tblPurchasesRequests.FederalTax : FederalTaxB.GetLastReccord().Cost;
            tblAshDeposits.CreatedDate = ServerDateTimeNow;
            tblAshDeposits.UpdatedDate = ServerDateTimeNow;
            tblAshDeposits.CreatedBy = LoggedUser;
            tblAshDeposits.UpdatedBy = LoggedUser;

            tblAshDeposits.Nicho = tblPurchasesRequests.Nicho;
            tblAshDeposits.ContractID = tblPurchasesRequests.ContractID;
            tblAshDeposits.Level = tblPurchasesRequests.Level;
            tblAshDeposits.Phone = tblCustomers.CelPhone;
            tblAshDeposits.MisaDate = DateTime.Now;

            return View(tblAshDeposits);
        }
        [HttpPost]
        public ActionResult Formato(tblAshDeposits model, HttpPostedFileBase DeathCertificate, HttpPostedFileBase CremationCertificate, HttpPostedFileBase Ticket, string[] ASon)
        {

            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            Response response = AshDepositsB.Create(model);
            if (response.Result == Result.OK)
            {

                //string File = HelperAzure.SaveFile(DeathCertificate);
                //model.DeathCertificate = File;
                //File = HelperAzure.SaveFile(CremationCertificate);
                //model.CremationCertificate = File;
                //File = HelperAzure.SaveFile(Ticket);
                //model.Ticket = File;

                int idTabla = model.AshDepositID;
                string File = HelperAzure.SaveTicketFileFormatoActa(DeathCertificate, idTabla);
                model.DeathCertificate = File;
                File = HelperAzure.SaveTicketFileFormatoCremacion(CremationCertificate, idTabla);
                model.CremationCertificate = File;
                File = HelperAzure.SaveTicketFileFormatoRecibo(Ticket, idTabla);
                model.Ticket = File;

                AshDepositsB.Update(model);

                tblPurchasesRequestsAshDeposits tblPurchasesRequestsAshDeposits = new tblPurchasesRequestsAshDeposits();
                tblPurchasesRequestsAshDeposits.AshDepositID = model.AshDepositID;
                tblPurchasesRequestsAshDeposits.CustomerID = model.CustomerID;
                tblPurchasesRequestsAshDeposits.ContractID = model.ContractID;
                tblPurchasesRequestsAshDeposits.Phone = model.Phone;
                tblPurchasesRequestsAshDeposits.Name = model.Name + " " + model.PSurname + " " + model.MSurname;
                tblPurchasesRequestsAshDeposits.Age = model.DeathDate.AddTicks(-model.BirthDate.Ticks).Year - 1;
                tblPurchasesRequestsAshDeposits.Date = DateTime.Now;
                tblPurchasesRequestsAshDeposits.Hour = model.Hour;
                tblPurchasesRequestsAshDeposits.Applicant = model.Applicant;
                tblPurchasesRequestsAshDeposits.CivilStatus = model.CivilStatus;
                tblPurchasesRequestsAshDeposits.Religion = model.Religion;
                tblPurchasesRequestsAshDeposits.Sacrament = model.Sacrament;
                tblPurchasesRequestsAshDeposits.MisaHour = model.MisaHour;
                tblPurchasesRequestsAshDeposits.MisaDate = DateTime.Now;
                tblPurchasesRequestsAshDeposits.DeathCause = model.DeathCause;
                tblPurchasesRequestsAshDeposits.DeathCertificate = model.DeathCertificate;
                tblPurchasesRequestsAshDeposits.CremationCertificate = model.CremationCertificate;
                tblPurchasesRequestsAshDeposits.Ticket = model.Ticket;
                tblPurchasesRequestsAshDeposits.ProvicionalNicho = model.ProvicionalNicho;
                tblPurchasesRequestsAshDeposits.Nicho = model.Nicho;
                tblPurchasesRequestsAshDeposits.Level = model.Level;
                tblPurchasesRequestsAshDeposits.CreatedDate = ServerDateTimeNow;
                tblPurchasesRequestsAshDeposits.UpdatedDate = ServerDateTimeNow;
                tblPurchasesRequestsAshDeposits.CreatedBy = LoggedUser;
                tblPurchasesRequestsAshDeposits.UpdatedBy = LoggedUser;
                tblPurchasesRequestsAshDeposits.Wife = model.Wife;
                tblPurchasesRequestsAshDeposits.Active = true;
                string Sons = "";
                for (int i = 0; i < ASon.Length; i++)
                {
                    Sons += ASon[i];
                    Sons += (i + 1) < ASon.Length ? "," : "";
                }
                tblPurchasesRequestsAshDeposits.Sons = Sons;
                response = PurchasesRequestsAshDepositsB.Create(tblPurchasesRequestsAshDeposits);

                DateTime CelebrationDate = Convert.ToDateTime(model.DeathDate.Day + "/" + model.DeathDate.Month + "/" + ServerDateTimeNow.Year);
                if (ServerDateTimeNow > CelebrationDate)
                    CelebrationDate = CelebrationDate.AddYears(1);

                tblAnniversary tblAnniversary = new tblAnniversary();
                tblAnniversary.Name = $"Aniversario de {model.Name} {model.PSurname} {model.MSurname}";
                tblAnniversary.Date = CelebrationDate;
                tblAnniversary.Hour = model.MisaHour.Substring(0, 5);
                tblAnniversary.Description = $"<p>se celebra el aniversario luctuoso de {model.Name} {model.PSurname} {model.MSurname}</p>";
                tblAnniversary.Active = true;
                tblAnniversary.CreatedDate = ServerDateTimeNow;
                tblAnniversary.UpdatedDate = ServerDateTimeNow;
                tblAnniversary.CreatedBy = LoggedUser;
                tblAnniversary.UpdatedBy = LoggedUser;

                AnniversaryB AnniversaryB = new AnniversaryB();
                AnniversaryB.Create(tblAnniversary);

                return RedirectToAction("Index", new { id = model.PurchasesRequestID });
            }
            else
            {
                ModelState.AddModelError("", response.Data);
                return View(model);
            }
        }

        public FileResult Download(string file)
        {
            string filepathfinal = Server.MapPath("~");
            return File("C:/ChurchArchive/" + file + "", System.Net.Mime.MediaTypeNames.Application.Octet, file);
        }

        public ActionResult SearchArchiveAshDeposits(string billpath)
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
                        pathbill = "/AshDeposits/Download/?File=" + billpath + "";
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
                            pathbill = "/AshDeposits/Download/?File=" + billpath + "";
                        }
                        else if (url.ToLower().Contains("jpg"))
                        {
                            base64ImageRepresentation = "data:image/jpg;base64," + base64ImageRepresentation;
                            pathbill = "/AshDeposits/Download/?File=" + billpath + "";
                        }
                        else if (url.ToLower().Contains("png"))
                        {
                            base64ImageRepresentation = "data:image/png;base64," + base64ImageRepresentation;
                            pathbill = "/AshDeposits/Download/?File=" + billpath + "";
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

        public ActionResult Details(int id)
        {
            tblAshDeposits tblAshDeposits = AshDepositsB.GetAshDepositsByID(id);
            ViewBag.CreatedBy = tblAshDeposits.CreatedByName;
            ViewBag.UpdatedBy = tblAshDeposits.UpdatedByName;
            tblPurchasesRequestsAshDeposits tblPurchasesRequestsAshDeposits = PurchasesRequestsAshDepositsB.GetByAshDepositID(id);
            tblCustomers tblCustomers = CustomersB.GetCustomersByID(tblPurchasesRequestsAshDeposits.CustomerID);
            tblAshDeposits.CustomerID = tblPurchasesRequestsAshDeposits.CustomerID;
            tblAshDeposits.Customer = tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname;
            tblAshDeposits.ContractID = tblPurchasesRequestsAshDeposits.ContractID;
            tblAshDeposits.Phone = tblPurchasesRequestsAshDeposits.Phone;
            tblAshDeposits.Name = tblPurchasesRequestsAshDeposits.Name + " " + tblPurchasesRequestsAshDeposits.PSurname + " " + tblPurchasesRequestsAshDeposits.MSurname;
            tblAshDeposits.Age = tblPurchasesRequestsAshDeposits.Age;
            tblAshDeposits.Date = tblPurchasesRequestsAshDeposits.Date;
            tblAshDeposits.Hour = tblPurchasesRequestsAshDeposits.Hour;
            tblAshDeposits.Applicant = tblPurchasesRequestsAshDeposits.Applicant;
            tblAshDeposits.CivilStatus = tblPurchasesRequestsAshDeposits.CivilStatus;
            tblAshDeposits.Religion = tblPurchasesRequestsAshDeposits.Religion;
            tblAshDeposits.Sacrament = tblPurchasesRequestsAshDeposits.Sacrament;
            tblAshDeposits.MisaHour = tblPurchasesRequestsAshDeposits.MisaHour;
            tblAshDeposits.MisaDate = tblPurchasesRequestsAshDeposits.MisaDate;
            tblAshDeposits.DeathCause = tblPurchasesRequestsAshDeposits.DeathCause;
            tblAshDeposits.ProvicionalNicho = tblPurchasesRequestsAshDeposits.ProvicionalNicho;
            tblAshDeposits.Nicho = tblPurchasesRequestsAshDeposits.Nicho;
            tblAshDeposits.Level = tblPurchasesRequestsAshDeposits.Level;
            tblAshDeposits.Wife = tblPurchasesRequestsAshDeposits.Wife;
            tblAshDeposits.Sons = tblPurchasesRequestsAshDeposits.Sons;
            if (tblAshDeposits == null)
                return HttpNotFound();
            else
            {
                return View(tblAshDeposits);
            }
        }
        public ActionResult Edit(int id)
        {
            tblAshDeposits tblAshDeposits = AshDepositsB.GetAshDepositsByID(id);
            ViewBag.CreatedBy = tblAshDeposits.CreatedByName;
            ViewBag.UpdatedBy = tblAshDeposits.UpdatedByName;
            tblPurchasesRequestsAshDeposits tblPurchasesRequestsAshDeposits = PurchasesRequestsAshDepositsB.GetByAshDepositID(id);
            tblCustomers tblCustomers = CustomersB.GetCustomersByID(tblPurchasesRequestsAshDeposits.CustomerID);
            tblAshDeposits.CustomerID = tblPurchasesRequestsAshDeposits.CustomerID;
            tblAshDeposits.Customer = tblCustomers.Name + " " + tblCustomers.PSurname + " " + tblCustomers.MSurname;
            tblAshDeposits.ContractID = tblPurchasesRequestsAshDeposits.ContractID;
            tblAshDeposits.Phone = tblPurchasesRequestsAshDeposits.Phone;
            tblAshDeposits.Name = tblPurchasesRequestsAshDeposits.Name;
            tblAshDeposits.Age = tblPurchasesRequestsAshDeposits.Age;
            tblAshDeposits.Date = tblPurchasesRequestsAshDeposits.Date;
            tblAshDeposits.Hour = tblPurchasesRequestsAshDeposits.Hour;
            tblAshDeposits.Applicant = tblPurchasesRequestsAshDeposits.Applicant;
            tblAshDeposits.CivilStatus = tblPurchasesRequestsAshDeposits.CivilStatus;
            tblAshDeposits.Religion = tblPurchasesRequestsAshDeposits.Religion;
            tblAshDeposits.Sacrament = tblPurchasesRequestsAshDeposits.Sacrament;
            tblAshDeposits.MisaHour = tblPurchasesRequestsAshDeposits.MisaHour;
            tblAshDeposits.MisaDate = tblPurchasesRequestsAshDeposits.MisaDate;
            tblAshDeposits.DeathCause = tblPurchasesRequestsAshDeposits.DeathCause;
            tblAshDeposits.ProvicionalNicho = tblPurchasesRequestsAshDeposits.ProvicionalNicho;
            tblAshDeposits.Nicho = tblPurchasesRequestsAshDeposits.Nicho;
            tblAshDeposits.Level = tblPurchasesRequestsAshDeposits.Level;
            tblAshDeposits.Wife = tblPurchasesRequestsAshDeposits.Wife;
            tblAshDeposits.Sons = tblPurchasesRequestsAshDeposits.Sons;

            return View(tblAshDeposits);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAshDeposits model, HttpPostedFileBase FileDeathCertificate, HttpPostedFileBase FileCremationCertificate, HttpPostedFileBase FileTicket, string[] ASon)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            Response response = AshDepositsB.Update(model);
            if (response.Result == Result.OK)
            {
                string File = FileDeathCertificate != null ? HelperAzure.SaveFile(FileDeathCertificate) : "";
                if (File != "")
                    model.DeathCertificate = File;
                File = FileCremationCertificate != null ? HelperAzure.SaveFile(FileCremationCertificate) : "";
                if (File != "")
                    model.CremationCertificate = File;
                File = FileTicket != null ? HelperAzure.SaveFile(FileTicket) : "";
                if (File != "")
                    model.Ticket = File;

                AshDepositsB.Update(model);

                tblPurchasesRequestsAshDeposits tblPurchasesRequestsAshDeposits = PurchasesRequestsAshDepositsB.GetByAshDepositID(model.AshDepositID);
                tblPurchasesRequestsAshDeposits.Phone = model.Phone;
                tblPurchasesRequestsAshDeposits.Name = model.Name + " " + model.PSurname + " " + model.MSurname;
                tblPurchasesRequestsAshDeposits.Age = model.DeathDate.AddTicks(-model.BirthDate.Ticks).Year - 1;
                tblPurchasesRequestsAshDeposits.Date = DateTime.Now;
                tblPurchasesRequestsAshDeposits.Hour = model.Hour;
                tblPurchasesRequestsAshDeposits.Applicant = model.Applicant;
                tblPurchasesRequestsAshDeposits.CivilStatus = model.CivilStatus;
                tblPurchasesRequestsAshDeposits.Religion = model.Religion;
                tblPurchasesRequestsAshDeposits.Sacrament = model.Sacrament;
                tblPurchasesRequestsAshDeposits.MisaHour = model.MisaHour;
                tblPurchasesRequestsAshDeposits.MisaDate = DateTime.Now;
                tblPurchasesRequestsAshDeposits.DeathCause = model.DeathCause;
                tblPurchasesRequestsAshDeposits.DeathCertificate = model.DeathCertificate;
                tblPurchasesRequestsAshDeposits.CremationCertificate = model.CremationCertificate;
                tblPurchasesRequestsAshDeposits.Ticket = model.Ticket;
                tblPurchasesRequestsAshDeposits.ProvicionalNicho = model.ProvicionalNicho;
                tblPurchasesRequestsAshDeposits.Nicho = model.Nicho;
                tblPurchasesRequestsAshDeposits.Level = model.Level;
                tblPurchasesRequestsAshDeposits.UpdatedDate = ServerDateTimeNow;
                tblPurchasesRequestsAshDeposits.UpdatedBy = LoggedUser;
                tblPurchasesRequestsAshDeposits.Wife = model.Wife;
                string Sons = "";
                for (int i = 0; i < ASon.Length; i++)
                {
                    Sons += ASon[i];
                    Sons += (i + 1) < ASon.Length ? "," : "";
                }
                tblPurchasesRequestsAshDeposits.Sons = Sons;
                response = PurchasesRequestsAshDepositsB.Update(tblPurchasesRequestsAshDeposits);

                return RedirectToAction("Index", new { id = model.PurchasesRequestID });
            }
            else
            {
                ModelState.AddModelError("", response.Data);
                return View(model);
            }
        }
        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblAshDeposits tblAshDeposits = AshDepositsB.GetAshDepositsByID(id);
            tblAshDeposits.DeletedBy = LoggedUser;
            tblAshDeposits.DeletedDate = ServerDateTimeNow;
            tblAshDeposits.Active = false;

            Response response = AshDepositsB.Update(tblAshDeposits);
            if (response.Result == Result.OK)
            {
                return RedirectToAction("Index", new { id = tblAshDeposits.PurchasesRequestID });
            }
            else
            {
                ModelState.AddModelError("", response.Data);
                return RedirectToAction("Details", new { id = id });
            }
        }
        public ActionResult PDF(string Sorting_Order = "", string Date = "", string AshDepositDate = "", int page = 1)
        {
            //Parsing Dates
            DateTime dtAshDeposits = new DateTime();
            if (Date != "" || AshDepositDate != "")
            {
                if (AshDepositDate != "")
                    dtAshDeposits = DateTime.Parse(AshDepositDate);
                else
                    dtAshDeposits = DateTime.Parse(Date);
            }

            ViewBag.Date = dtAshDeposits.ToShortDateString();

            //Sortings
            ViewBag.Sorting_Order = Sorting_Order;

            //Selects lists
            var AshDeposits = AshDepositsB.GetAshDeposits(dtAshDeposits, page, Sorting_Order);

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

                //SUBTITULOS
                PdfPCell TitleCell = new PdfPCell();
                PdfPTable TitleTable = new PdfPTable(1);

                PdfPCell MainTitleHeader = new PdfPCell(new Phrase("Depositos de ceniza", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLD)));
                MainTitleHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                MainTitleHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;

                TitleTable.AddCell(MainTitleHeader);
                TitleTable.AddCell(LineCell);

                TitleTable.TotalWidth = 300f;
                TitleTable.LockedWidth = true;
                TitleTable.HorizontalAlignment = 0;

                TitleCell.Colspan = 2;
                TitleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                TitleCell.AddElement(TitleTable);

                ImageTable.AddCell(ImageCell);
                ImageTable.AddCell(TitleCell);

                ImageTable.TotalWidth = 500f;
                ImageTable.LockedWidth = true;
                ImageTable.HorizontalAlignment = 0;

                ImageTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImageTableCell.Colspan = 3;
                ImageTableCell.AddElement(ImageTable);

                //PAGINA 1
                MainTable.AddCell(ImageTableCell);

                //MINI TABLE APARTADO B
                PdfPCell MiniTableCell = new PdfPCell();
                PdfPTable MiniTable = new PdfPTable(4);
                MiniTable.AddCell(new Paragraph("Nombre", new Font(Font.FontFamily.UNDEFINED, 7f, Font.BOLD)));
                MiniTable.AddCell(new Paragraph("Fecha de nacimiento", new Font(Font.FontFamily.UNDEFINED, 7f, Font.BOLD)));
                MiniTable.AddCell(new Paragraph("Fecha de defunción", new Font(Font.FontFamily.UNDEFINED, 7f, Font.BOLD)));
                MiniTable.AddCell(new Paragraph("Fecha de deposito", new Font(Font.FontFamily.UNDEFINED, 7f, Font.BOLD)));

                foreach (var item in AshDeposits)
                {
                    MiniTable.AddCell(new Paragraph(item.Name, new Font(Font.FontFamily.UNDEFINED, 7f, Font.NORMAL)));
                    MiniTable.AddCell(new Paragraph(item.BirthDate.ToShortDateString(), new Font(Font.FontFamily.UNDEFINED, 7f, Font.NORMAL)));
                    MiniTable.AddCell(new Paragraph(item.DeathDate.ToShortDateString(), new Font(Font.FontFamily.UNDEFINED, 7f, Font.NORMAL)));
                    MiniTable.AddCell(new Paragraph(item.AshDepositDate.ToShortDateString(), new Font(Font.FontFamily.UNDEFINED, 7f, Font.NORMAL)));
                }
                MiniTable.TotalWidth = 500f;
                MiniTable.LockedWidth = true;
                MiniTable.HorizontalAlignment = 0;
                MiniTable.HorizontalAlignment = Element.ALIGN_CENTER;

                MiniTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                MiniTableCell.HorizontalAlignment = Element.ALIGN_CENTER;
                MiniTableCell.Colspan = 5;
                MiniTableCell.AddElement(MiniTable);
                MainTable.AddCell(MiniTableCell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                doc.Add(MainTable);

                writer.CloseStream = false;
                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Depositos de ceniza.pdf");
                Response.Write(doc);
                Response.End();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View();
        }
    }
}