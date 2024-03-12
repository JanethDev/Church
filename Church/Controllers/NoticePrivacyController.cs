using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Church.Controllers
{
    public class NoticePrivacyController : BaseController
    {
        // GET: NoticePrivacy
        public ActionResult Index()
        {
            var doc = new Document(iTextSharp.text.PageSize.LETTER);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);

                doc.Open();
                doc.NewPage();

                PdfPTable MainTable = new PdfPTable(5);
                MainTable.TotalWidth = 550f;
                MainTable.LockedWidth = true;
                MainTable.HorizontalAlignment = 0;

                //CELDAS EN BLANCO
                Paragraph Line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 10)));
                PdfPCell LineCell = new PdfPCell();
                LineCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                LineCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
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

                string imageURL = Server.MapPath("~/Content/Images/LogoParroquia.png");
                Image img = Image.GetInstance(imageURL);
                img.ScaleAbsolute(75f, 80f);

                PdfPCell ImageCell = new PdfPCell(img);
                ImageCell.Colspan = 2;
                ImageCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ImageCell.HorizontalAlignment = Element.ALIGN_CENTER;

                //SUBTITULOS
                PdfPCell TitleCell = new PdfPCell();
                PdfPTable TitleTable = new PdfPTable(2);

                PdfPCell MainTitleHeader = new PdfPCell(new Phrase("AVISO DE PRIVACIDAD", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLDITALIC)));
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

                //PRIMER PARRAFO
                PdfPCell ParagraphCell = new PdfPCell(new Paragraph("Al fin de dar cumpliemiento a lo dispuesto en la Ley Federal de Datos Personales en Posesión de los Particulares" + 
                " (en adelante la " + quote + "Ley" + quote + "), " + quote + "Parroquia Iglesia Guadalupe del Río en Tijuana, A.R." + quote + " a quien en lo sucesivo se le denominará" +
                " " + quote + "LA ASOCIACIÓN" + quote + ", establece el siguiente " + quote + "aviso de privacidad " + quote + " para confirmar cuales son las caracteristicas bajo las cuales" +
                " sus datos personales (aun los sensibles) son resguardados, los cuales " + quote + "LA ASOCIACIÓN" + quote + " ha obtenido de las solicitudes por la adquisición de nichos y " +
                " otros medios con motivo de la venta de los mismos.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphCell.Colspan = 5;
                ParagraphCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ParagraphCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(ParagraphCell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //AVISO DE PRIVACIDAD
                PdfPCell CompanyHeader = new PdfPCell(new Phrase("AVISO DE PRIVACIDAD", new Font(Font.FontFamily.UNDEFINED, 14f, Font.BOLDITALIC)));
                CompanyHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                CompanyHeader.Colspan = 5;
                MainTable.AddCell(CompanyHeader);

                PdfPCell SubParagraphACell = new PdfPCell();
                Paragraph PhraseParagraphA = new Paragraph();

                PhraseParagraphA.Add(new Chunk("A.- En todo momemto usted podrá revocar el consentimiento que nos ha otorgado para el tratamiento de sus datos personales" +
                ", a fin que dejemos de hacer uso de los mismos. Para ello, es necesario que presente su petición por escrito al Director de la ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                PhraseParagraphA.Add(new Chunk("LA ASOCIACIÓN", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                PhraseParagraphA.Add(new Chunk(" en las oficinas ubicadas en Ave Paseo del Centenario No. 10150, Zona Río, en la ciudad de Tijuana, Baja California, México. Su petición deberá ir acompañada de la siguiente información: el nombre del titular y" +
                " domicilio u otro medio para comunicarle la respuesta a su solicitud, los documentos que acrediten la identidad, o en su caso, la representación legal del titular, la descripción clara" +
                " y o documento que facilite la localización de los datos personales. Tendremos un plazo de 20 días hábiles para atender su petición y le informaremos vía correo electrónico." +
                " Siendo el departamento responsable de atender su derecho el área de cobranza cuyo télefono es (01-664) 607-37-75.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));

                PhraseParagraphA.SetLeading(1, 1);
                SubParagraphACell.AddElement(PhraseParagraphA);

                PdfPCell SubParagraphBCell = new PdfPCell(new Paragraph("B.- Los datos personales que recabemos directamente de Ud. en la solicitud son: nombre del Títular, domicilio, télefono partícular" +
                " correo electrónico, R.F.C./C.U.R.P., fecha de nacimiento, lugar de nacimiento, estado civil, ocupación, datos de la empresa donde presta sus servicios, referencias y beneficiarios." +
                " En caso de no contar con esta información no estariamos en posibilidad de entregar un informe de resultados correcto.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));

                PdfPCell SubParagraphCCell = new PdfPCell(new Paragraph("C.- " + quote + "LA ASOCIACIÓN" + quote + " está comprometido a salvaguardar la confidencialidad de los datos personales de tal manera" +
                " que su privacidad está protegida en terminos de la Ley. Los datos personales serán utilizados para el cumplimiento de las obligaciones contraídas con todos los usuarios de esta parroquia" +
                ", así como para informar los avances que tengamos en la construcción de la Nueva Catedral.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));

                PdfPCell SubParagraphDCell = new PdfPCell(new Paragraph("D.- " + quote + "LA ASOCIACIÓN" + quote + " es responsable del tratamiento de sus datos personales para solicitar cualquier información" +
                " comunicarse al télefono (01-664) 607-37-75 con el responsable del área de Ventas y Cobranza.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));

                PdfPCell SubParagraphECell = new PdfPCell();
                Paragraph PhraseParagraphE = new Paragraph();
                PhraseParagraphE.Add(new Chunk("E.- ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                PhraseParagraphE.Add(new Chunk("Por ningún motivo" + quote + "LA ASOCIACIÓN" + quote + " proporcionará información de los adquirientes a terceros, ya sean personas fisicas" +
                " o morales, salvo la petición expresa de las autoridades. Nuestra base de datos está resguardada en el servidor y es utilizada solo por personal autorizado.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));

                PhraseParagraphE.SetLeading(1, 1);
                SubParagraphECell.AddElement(PhraseParagraphE);

                PdfPCell SubParagraphFCell = new PdfPCell(new Paragraph("E.- Nos reservamos el derecho de efectuar en cualquier momento modificaciones o actualizaciones al presente aviso de privacidad" +
                ", para la atención de las actualizaciones legislativas o jurisprudenciales, políticas internas, nuevos requerimientos para la prestación u ofrecimiento del servicio. Estas modificaciones" +
                " estarán disponibles al público en nuestro sitio de internet www.nuevacatedraltijuana.org, siendo la fecha de la última actualización al presente aviso de privacidad el 12 de diciembre del 2011.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));

                PdfPCell SubParagraphGCell = new PdfPCell(new Paragraph("E.- Si no está de acuerdo en el consentimiento de que sus datos personales sean tratados conforme al presente aviso de privacidad favor de" +
                " de notificarlo al télefono mencionado en el presente documento.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));

                SubParagraphACell.Colspan = 5;
                SubParagraphACell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubParagraphACell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                SubParagraphBCell.Colspan = 5;
                SubParagraphBCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubParagraphBCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                SubParagraphCCell.Colspan = 5;
                SubParagraphCCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubParagraphCCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                SubParagraphDCell.Colspan = 5;
                SubParagraphDCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubParagraphDCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                SubParagraphECell.Colspan = 5;
                SubParagraphECell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubParagraphECell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                SubParagraphFCell.Colspan = 5;
                SubParagraphFCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubParagraphFCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                SubParagraphGCell.Colspan = 5;
                SubParagraphGCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SubParagraphGCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                MainTable.AddCell(SubParagraphACell);
                MainTable.AddCell(SubParagraphBCell);
                MainTable.AddCell(SubParagraphCCell);
                MainTable.AddCell(SubParagraphDCell);
                MainTable.AddCell(SubParagraphECell);
                MainTable.AddCell(SubParagraphFCell);
                MainTable.AddCell(SubParagraphGCell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //FOOTER
                PdfPCell FooterCell = new PdfPCell(new Phrase("Ave. Paseo Tijuana Número 10150 Zona Río CP 22130" + "\n" + "Tijuana, Baja California, México" + "\n" + "Télefono 607-37-75", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                FooterCell.HorizontalAlignment = Element.ALIGN_CENTER;
                FooterCell.Colspan = 5;
                MainTable.AddCell(FooterCell);

                doc.Add(MainTable);

                writer.CloseStream = false;
                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=AvisoDePrivacidad.pdf");
                Response.Write(doc);
                Response.End();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View();
        }

        // GET: NoticePrivacy/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NoticePrivacy/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NoticePrivacy/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: NoticePrivacy/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NoticePrivacy/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: NoticePrivacy/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NoticePrivacy/Delete/5
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
    }
}
