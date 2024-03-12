using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Church.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado,Caja")]
    public class IntentionsController : BaseController
    {
        //Bussines objects
        AshDepositsB AshDepositsB = new AshDepositsB();
        IntentionsB IntentionsB = new IntentionsB();
        IntentionsTypeB IntentionsTypeB = new IntentionsTypeB();
        ScheduleB ScheduleB = new ScheduleB();
        UsersB UsersB = new UsersB();
        HelpMethods HM = new HelpMethods();

        //Autocompletes
        public ActionResult acName(string term)
        {
            var model = HM.Autocompletes("Name", term, "tblIntentions");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult acMentionPerson(string term)
        {
            var model = HM.Autocompletes("MentionPerson", term, "tblIntentions");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getHourByDay(string Date)
        {
            DateTime IntentionDate = Convert.ToDateTime(Date);
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            string DayOfWeek = dtinfo.GetDayName(IntentionDate.DayOfWeek);
            DayOfWeek = DayOfWeek.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            DayOfWeek = reg.Replace(DayOfWeek, "");

            var lstSchedule = ScheduleB.GetListByDay(DayOfWeek);

            return Json(lstSchedule, JsonRequestBehavior.AllowGet);
        }

        // GET: Intentions
        public ActionResult Index(string Sorting_Order = "", string Name = "", string MentionPerson = "", string IntentionTypeID = "", string ApplicationDate = "", string IntentionDate = "",string Hour = "", int page = 1)
        {
            //Parsing Dates
            DateTime dtApplicationDate = new DateTime();
            if (ApplicationDate != "")
                dtApplicationDate = DateTime.Parse(ApplicationDate);
            
            DateTime dtIntentionDate = new DateTime();
            if (IntentionDate != "")
                dtIntentionDate = DateTime.Parse(IntentionDate);

            //Filters
            ViewBag.Name = Name;
            ViewBag.MentionPerson = MentionPerson;
            ViewBag.Hour = Hour;
            ViewBag.IntentionTypeID = IntentionTypeID;
            ViewBag.ApplicationDate = ApplicationDate;
            ViewBag.IntentionDate = IntentionDate;
            //Sortings
            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "tblIntentions.Name" ? "tblIntentions.Name_Desc" : "tblIntentions.Name";
            ViewBag.SortingMentionPerson = Sorting_Order == "tblIntentions.MentionPersonName" ? "tblIntentions.MentionPerson_Desc" : "tblIntentions.MentionPerson";
            ViewBag.SortingHour = Sorting_Order == "tblIntentions.ScheduleID" ? "tblIntentions.Hour_Desc" : "tblIntentions.ScheduleID";
            ViewBag.SortingIntentionType = Sorting_Order == "tblIntentionsType.Name" ? "tblIntentionsType.Name_Desc" : "tblIntentionsType.Name";
            ViewBag.SortingApplicationDate = Sorting_Order == "tblIntentions.ApplicationDate" ? "tblIntentions.ApplicationDate_Desc" : "tblIntentions.ApplicationDate";
            ViewBag.SortingIntentionDate = Sorting_Order == "tblIntentions.IntentionDate" ? "tblIntentions.IntentionDate_Desc" : "tblIntentions.IntentionDate";

            int IntentionTypeIDInt = 0;

            if (IntentionTypeID != "")
            {
                IntentionTypeIDInt = Convert.ToInt32(IntentionTypeID);
            }
            //Selects lists
            ViewBag.IntentionTypes = new SelectList(IntentionsTypeB.GetList(), "IntentionTypeID", "Name", IntentionTypeID);

            var tblIntentions = IntentionsB.GetIntentions(Name, MentionPerson, IntentionTypeIDInt, dtApplicationDate, dtIntentionDate, Hour, page, Sorting_Order);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Intentions", tblIntentions);
            }
            return View(tblIntentions);
        }

        // GET: Intentions/Details/5
        public ActionResult Details(int id)
        {
            tblIntentions tblIntentions = IntentionsB.GetByID(id);
            if (tblIntentions == null)
                return HttpNotFound();
            else
                return View(tblIntentions);
        }

        // GET: Intentions/Create
        public ActionResult Create()
        {
            tblIntentions tblIntentions = new tblIntentions();

            ViewBag.IntentionTypes = IntentionsTypeB.GetList();
            ViewBag.ExchangeTypes = new SelectList(Enum.GetValues(typeof(ExchangeTypes)));

            return View(tblIntentions);
        }

        // POST: Intentions/Create
        [HttpPost]
        public ActionResult Create(tblIntentions tblIntentions)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            string DayOfWeek = dtinfo.GetDayName(tblIntentions.IntentionDate.DayOfWeek);
            DayOfWeek = DayOfWeek.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            DayOfWeek = reg.Replace(DayOfWeek, "");
            ViewBag.Schedules = ScheduleB.GetListByDay(DayOfWeek);
            #region if return to view
            ViewBag.IntentionTypes = IntentionsTypeB.GetList();
            ViewBag.ExchangeTypes = new SelectList(Enum.GetValues(typeof(ExchangeTypes)));
            #endregion
            try
            {
                // TODO: Add insert logic here
                if (HM.CheckIfExists("Name", tblIntentions.Name, "IntentionID", tblIntentions.IntentionID, "tblIntentions"))
                {
                    ModelState.AddModelError("", "El nombre de la intención ya existe.");
                    return View(tblIntentions);
                }
                else
                {
                    DateTime ServerDateTimeNow = HM.GetDateTime();
                    int LoggedUser = HM.GetIntbyString("UserName", HttpContext.User.Identity.Name, "UserID", "tblUsers");
                    Enum.TryParse(tblIntentions.ExchangeType.ToString(), out ExchangeTypes exchangeTypes);

                    //Audit fields
                    tblIntentions.Active = true;
                    tblIntentions.CreatedBy = LoggedUser;
                    tblIntentions.CreatedDate = ServerDateTimeNow;
                    tblIntentions.UpdatedBy = LoggedUser;
                    tblIntentions.UpdatedDate = ServerDateTimeNow;
                    //Model fields
                    tblIntentions.ApplicationDate = tblIntentions.ApplicationDate;
                    tblIntentions.ExchangeType = exchangeTypes;
                    Response response = IntentionsB.Create(tblIntentions);
                    if (response.Result == Result.OK)
                        return RedirectToAction("Index");
                    else
                    {
                        ModelState.AddModelError("", "Ocurrió un error al guardar: " + response.Data);
                        return View(tblIntentions);
                    }
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "Ocurrió un error: " + e.Message);
                return View(tblIntentions);
            }
        }

        // GET: Intentions/Edit/5
        public ActionResult Edit(int id)
        {
            tblIntentions tblIntentions = IntentionsB.GetByID(id);

            ViewBag.IntentionTypes = IntentionsTypeB.GetList();

            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            string DayOfWeek = dtinfo.GetDayName(tblIntentions.IntentionDate.DayOfWeek);
            DayOfWeek = DayOfWeek.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            DayOfWeek = reg.Replace(DayOfWeek, "");
            ViewBag.Schedules = ScheduleB.GetListByDay(DayOfWeek);
            ViewBag.ExchangeTypes = new SelectList(Enum.GetValues(typeof(ExchangeTypes)));

            return View(tblIntentions);
        }

        // POST: Intentions/Edit/5
        [HttpPost]
        public ActionResult Edit(tblIntentions tblIntentions)
        {

            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            string DayOfWeek = dtinfo.GetDayName(tblIntentions.IntentionDate.DayOfWeek);
            DayOfWeek = DayOfWeek.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            DayOfWeek = reg.Replace(DayOfWeek, "");
            ViewBag.Schedules = ScheduleB.GetListByDay(DayOfWeek);

            #region if return to view
            ViewBag.IntentionTypes = IntentionsTypeB.GetList();
            ViewBag.ExchangeTypes = new SelectList(Enum.GetValues(typeof(ExchangeTypes)));
            #endregion
            try
            {
                // TODO: Add update logic here
                if (HM.CheckIfExists("Name", tblIntentions.Name, "IntentionID", tblIntentions.IntentionID, "tblIntentions"))
                {
                    ModelState.AddModelError("", "El nombre de la intención ya existe.");
                    return View(tblIntentions);
                }
                else
                {
                    Enum.TryParse(tblIntentions.ExchangeType.ToString(), out ExchangeTypes exchangeTypes);

                    //Audit fields
                    tblIntentions.UpdatedBy = HM.GetIntbyString("UserName", HttpContext.User.Identity.Name, "UserID", "tblUsers");
                    tblIntentions.UpdatedDate = HM.GetDateTime();
                    //Model fields
                    tblIntentions.ExchangeType = exchangeTypes;
                    Response response = IntentionsB.Update(tblIntentions);
                    if (response.Result == Result.OK)
                        return RedirectToAction("Index");
                    else
                    {
                        ModelState.AddModelError("", "Ocurrió un error al guardar: " + response.Data);
                        return View(tblIntentions);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Ocurrió un error: " + e.Message);
                return View(tblIntentions);
            }
        }

        // GET: Intentions/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Intentions/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult GetMentionPersonByAshDepositID(string term)
        {
            List<tblAshDeposits> AshDeposits = AshDepositsB.GetListAutocompleteByID(term);
            return Json(AshDeposits, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PDF(string Name = "", int IntentionTypeID = 0, DateTime? ApplicationDate = null, DateTime? IntentionDate = null)
        {
            var lstSchedule = ScheduleB.GetList();
            List<tblIntentions> ListIntentions = IntentionsB.GetListForPDF(Name, IntentionTypeID, ApplicationDate, IntentionDate);
            //Varaibles para lista de intenciones
            string sAnniversaryList = "";
            string sThanksgivingList = "";
            string sVariousNeeds = "";
            string sHealth = "";
            string sEspecialCelebrate = "";
            string sDeceased = "";
            string sIntentionDate = IntentionDate != null ? "Intenciones del " + IntentionDate.Value.ToShortDateString() : "Sin fecha de intención definida";

            if (ListIntentions != null && ListIntentions.Count > 0)
            {
                //Variables que obtienen total de intenciones por tipo
                int iAnniversaryCount = ListIntentions.Where(r => r.IntentionTypeID == 1).ToList().Count;
                int iThanksgivingCount = ListIntentions.Where(r => r.IntentionTypeID == 2).ToList().Count;
                int iHealthCount = ListIntentions.Where(r => r.IntentionTypeID == 3).ToList().Count;
                int iVariousNeedsCount = ListIntentions.Where(r => r.IntentionTypeID == 4).ToList().Count;
                int iEspecialCelebrate = ListIntentions.Where(r => r.IntentionTypeID == 1002).ToList().Count;
                int iDeceased = ListIntentions.Where(r => r.IntentionTypeID == 1003).ToList().Count;
                int index = 1;

                //Concatenación de valores de lista
                foreach (var item in ListIntentions.Where(r => r.IntentionTypeID == 1))
                {
                    tblSchedule tblSchedule = lstSchedule.Where(r => r.ScheduleID == item.ScheduleID).FirstOrDefault();
                    
                    sAnniversaryList += index + ") " + item.MentionPerson + "   Hora: " + tblSchedule.Hour + System.Environment.NewLine + "   Asunto: " + item.Description + System.Environment.NewLine;
                    index++;
                }
                //Concatenación de espacios en blanco de máximo 10 renglones por tipo
                for (int i = 0; i < (7 - iAnniversaryCount); i++)
                {
                    sAnniversaryList += System.Environment.NewLine;
                }
                index = 1;
                //Concatenación de valores de lista
                foreach (var item in ListIntentions.Where(r => r.IntentionTypeID == 2))
                {
                    tblSchedule tblSchedule = lstSchedule.Where(r => r.ScheduleID == item.ScheduleID).FirstOrDefault();
                    sThanksgivingList += index + ") " + item.MentionPerson + "   Hora: " + tblSchedule.ScheduleID + System.Environment.NewLine + "   Asunto: " + item.Description + System.Environment.NewLine;
                    index++;
                }
                //Concatenación de espacios en blanco de máximo 10 renglones por tipo
                for (int i = 0; i < (7 - iThanksgivingCount); i++)
                {
                    sThanksgivingList += System.Environment.NewLine;
                }
                index = 1;
                //Concatenación de valores de lista
                foreach (var item in ListIntentions.Where(r => r.IntentionTypeID == 3))
                {
                    tblSchedule tblSchedule = lstSchedule.Where(r => r.ScheduleID == item.ScheduleID).FirstOrDefault();
                    sHealth += index + ") " + item.MentionPerson + "   Hora: " + tblSchedule.ScheduleID + System.Environment.NewLine + "   Asunto: " + item.Description + System.Environment.NewLine;
                    index++;
                }
                //Concatenación de espacios en blanco de máximo 10 renglones por tipo
                for (int i = 0; i < (7 - iHealthCount); i++)
                {
                    sHealth += System.Environment.NewLine;
                }
                index = 1;
                //Concatenación de valores de lista
                foreach (var item in ListIntentions.Where(r => r.IntentionTypeID == 4))
                {
                    tblSchedule tblSchedule = lstSchedule.Where(r => r.ScheduleID == item.ScheduleID).FirstOrDefault();
                    sVariousNeeds += index + ") " + item.MentionPerson + "   Hora: " + tblSchedule.ScheduleID + System.Environment.NewLine + "   Asunto: " + item.Description + System.Environment.NewLine;
                    index++;
                }
                //Concatenación de espacios en blanco de máximo 10 renglones por tipo
                for (int i = 0; i < (7 - iVariousNeedsCount); i++)
                {
                    sVariousNeeds += System.Environment.NewLine;
                }
                index = 1;
                //Concatenación de valores de lista
                foreach (var item in ListIntentions.Where(r => r.IntentionTypeID == 1002))
                {
                    tblSchedule tblSchedule = lstSchedule.Where(r => r.ScheduleID == item.ScheduleID).FirstOrDefault();
                    sEspecialCelebrate += index + ") " + item.MentionPerson + "   Hora: " + tblSchedule.Hour + System.Environment.NewLine + "   Asunto: " + item.Description + System.Environment.NewLine;
                    index++;
                }
                //Concatenación de espacios en blanco de máximo 10 renglones por tipo
                for (int i = 0; i < (7 - iEspecialCelebrate); i++)
                {
                    sEspecialCelebrate += System.Environment.NewLine;
                }
                index = 1;
                //Concatenación de valores de lista
                foreach (var item in ListIntentions.Where(r => r.IntentionTypeID == 1003))
                {
                    tblSchedule tblSchedule = lstSchedule.Where(r => r.ScheduleID == item.ScheduleID).FirstOrDefault();
                    sDeceased += index + ") " + item.MentionPerson + "   Hora: " + tblSchedule.Hour + System.Environment.NewLine + "   Asunto: " + item.Description + System.Environment.NewLine;
                    index++;
                }
                //Concatenación de espacios en blanco de máximo 10 renglones por tipo
                for (int i = 0; i < (7 - iDeceased); i++)
                {
                    sDeceased += System.Environment.NewLine;
                }
            }

            var doc = new Document(iTextSharp.text.PageSize.LETTER);
            try 
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);

                doc.Open();
                doc.NewPage();

                PdfPTable MainTable = new PdfPTable(5);
                MainTable.TotalWidth = 500f;
                MainTable.LockedWidth = true;
                MainTable.HorizontalAlignment = 0;

                //CELDAS EN BLANCO
                Paragraph Line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 10)));
                PdfPCell LineCell = new PdfPCell();
                LineCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                LineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                LineCell.AddElement(Line);
                LineCell.Colspan = 5;

                //SALTO DE LINEA
                PdfPCell CellRow = new PdfPCell(new Paragraph("\n\n"));
                CellRow.Colspan = 5;
                CellRow.Border = iTextSharp.text.Rectangle.NO_BORDER;

                //COMILLAS PARA TEXTO
                const string quote = "\"";

                //IMAGEN
                PdfPCell ImageTableCell = new PdfPCell();
                PdfPTable ImageTable = new PdfPTable(5);

                string imageURL = Server.MapPath("~/Content/Images/ChurchLogo.jpg");
                Image img = Image.GetInstance(imageURL);
                img.ScaleAbsolute(75f, 50f);

                PdfPCell ImageCell = new PdfPCell(img);
                ImageCell.Colspan = 2;
                ImageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImageCell.HorizontalAlignment = Element.ALIGN_CENTER;

                //SUBTITULOS
                PdfPCell TitleCell = new PdfPCell();
                PdfPTable TitleTable = new PdfPTable(2);


                PdfPCell MainTitleHeader = new PdfPCell(new Phrase("LISTA DE INTENCIONES", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC)));
                MainTitleHeader.Colspan = 2;
                MainTitleHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                //MainTitleHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;

                TitleTable.AddCell(MainTitleHeader);
                TitleTable.AddCell(LineCell);

                TitleTable.TotalWidth = 300f;
                TitleTable.LockedWidth = true;
                TitleTable.HorizontalAlignment = 0;

                TitleCell.Colspan = 3;
                TitleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                TitleCell.AddElement(TitleTable);

                ImageTable.AddCell(ImageCell);
                ImageTable.AddCell(TitleCell);

                ImageTable.TotalWidth = 500f;
                ImageTable.LockedWidth = true;
                ImageTable.HorizontalAlignment = 0;

                ImageTableCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImageTableCell.Colspan = 5;
                ImageTableCell.AddElement(ImageTable);
                MainTable.AddCell(ImageTableCell);

                //CONTENIDO
                PdfPCell ContentTableCell = new PdfPCell();
                PdfPTable ContentTable = new PdfPTable(6);

                PdfPCell FirstRow = new PdfPCell();
                FirstRow.Colspan = 3;
                FirstRow.HorizontalAlignment = Element.ALIGN_CENTER;
                FirstRow.Border = iTextSharp.text.Rectangle.NO_BORDER;

                PdfPCell SecondRow = new PdfPCell();
                SecondRow.Colspan = 3;
                SecondRow.HorizontalAlignment = Element.ALIGN_CENTER;
                SecondRow.Border = iTextSharp.text.Rectangle.NO_BORDER;

                Paragraph DateParagraph = new Paragraph(sIntentionDate, new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC));

                Paragraph AnniversaryParagraph = new Paragraph();
                AnniversaryParagraph.Add(new Chunk("Aniversario luctuoso: " + "\n", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC)));
                AnniversaryParagraph.Add(new Chunk(sAnniversaryList, new Font(Font.FontFamily.UNDEFINED, 9f, Font.NORMAL)));

                Paragraph ThanksgivingParagraph = new Paragraph();
                ThanksgivingParagraph.Add(new Chunk("Acción de gracias: " + "\n", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC)));
                ThanksgivingParagraph.Add(new Chunk(sThanksgivingList, new Font(Font.FontFamily.UNDEFINED, 9f, Font.NORMAL)));

                Paragraph VariousNeedsParagraph = new Paragraph();
                VariousNeedsParagraph.Add(new Chunk("Diversas necesidades: " + "\n", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC)));
                VariousNeedsParagraph.Add(new Chunk(sVariousNeeds, new Font(Font.FontFamily.UNDEFINED, 9f, Font.NORMAL)));

                Paragraph HealthParagraph = new Paragraph();
                HealthParagraph.Add(new Chunk("\n" + "Salud: " + "\n", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC)));
                HealthParagraph.Add(new Chunk(sHealth, new Font(Font.FontFamily.UNDEFINED, 9f, Font.NORMAL)));

                Paragraph EspecialCelebrateParagraph = new Paragraph();
                HealthParagraph.Add(new Chunk("\n" + "Celebración especial: " + "\n", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC)));
                HealthParagraph.Add(new Chunk(sEspecialCelebrate, new Font(Font.FontFamily.UNDEFINED, 9f, Font.NORMAL)));

                Paragraph DeceasedParagraph = new Paragraph();
                HealthParagraph.Add(new Chunk("\n" + "Difunto: " + "\n", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC)));
                HealthParagraph.Add(new Chunk(sDeceased, new Font(Font.FontFamily.UNDEFINED, 9f, Font.NORMAL)));

                FirstRow.AddElement(DateParagraph);
                FirstRow.AddElement(AnniversaryParagraph);
                FirstRow.AddElement(ThanksgivingParagraph);
                FirstRow.AddElement(VariousNeedsParagraph);
                SecondRow.AddElement(HealthParagraph);
                SecondRow.AddElement(EspecialCelebrateParagraph);
                SecondRow.AddElement(DeceasedParagraph);

                ContentTable.AddCell(FirstRow);
                ContentTable.AddCell(SecondRow);

                ContentTable.TotalWidth = 500f;
                ContentTable.LockedWidth = true;
                ContentTable.HorizontalAlignment = 0;

                ContentTableCell.Colspan = 5;
                ContentTableCell.AddElement(ContentTable);
                MainTable.AddCell(ContentTableCell);

                doc.Add(MainTable);

                writer.CloseStream = false;
                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ListaIntenciones.pdf");
                Response.Write(doc);
                Response.End();
                return RedirectToAction("Index", "Intentions");
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return null;
        }
    }
}
