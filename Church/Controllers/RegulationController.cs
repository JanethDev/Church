using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Church.Controllers
{
    public class RegulationController : BaseController
    {
        // GET: Regulation
        public ActionResult Index()
        {
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

                PdfPCell MainTitleHeader = new PdfPCell(new Phrase("REGLAMENTO DEL COLUMBARIO", new Font(Font.FontFamily.UNDEFINED, 9f, Font.BOLD)));
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

                //PRIMER PARRAFO
                PdfPCell ParagraphCell = new PdfPCell(new Paragraph("Con el propósito de conservar siempre en buen estado las instalaciones, muebles, áreas verdes, capillas, estacionamiento, etc., y todo lo relacionado con la custodia, " +
                "depósito y extracción de urnas cuyo contenido sea única y exclusivamente el de restos humanos incinerados o cremados, se creó el presente reglamento, con el cual se sujetarán a las siguientes: \n\n" + 
                "Que, en virtud de la autorización concedida por el Arzobispo de Tijuana mediante decreto y acta constitutiva del día 18-septiembre-2017), he tenido a bien expedir el siguiente Reglamento que está constituido por los siguientes Títulos.", 
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                ParagraphCell.Colspan = 3;
                ParagraphCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ParagraphCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(ParagraphCell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO PRELIMINAR
                PdfPCell PreliminarTitle = new PdfPCell(new Phrase("TÍTULO PRELIMINAR", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                PreliminarTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PreliminarTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                PreliminarTitle.Colspan = 3;
                MainTable.AddCell(PreliminarTitle);

                //DISPOSICIONES GENERALES
                PdfPCell GeneralDispositionTitle = new PdfPCell(new Phrase("DISPOSICIONES GENERALES", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                GeneralDispositionTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                GeneralDispositionTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                GeneralDispositionTitle.Colspan = 3;
                MainTable.AddCell(GeneralDispositionTitle);

                //ARTICULO 1
                PdfPCell Art1Cell = new PdfPCell();
                Paragraph Paragraph1 = new Paragraph();

                Paragraph1.Add(new Chunk("Artículo 1.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph1.Add(new Chunk(" El presente reglamento tiene por objeto regular el uso en participación, funcionamiento, conservación y vigilancia del Columbario (zona de criptas) con que se cuenta.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph1.SetLeading(1, 1);
                Art1Cell.AddElement(Paragraph1);
                Art1Cell.Colspan = 5;
                Art1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art1Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art1Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 2
                PdfPCell Art2Cell = new PdfPCell();
                Paragraph Paragraph2 = new Paragraph();

                Paragraph2.Add(new Chunk("Artículo 2.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph2.Add(new Chunk(" A él deberán sujetarse todas aquellas personas o familias que hayan adquirido una o más criptas presentando su contrato firmado por el Adquiriente y por el Representante Legal con la Asociación.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph2.SetLeading(1, 1);
                Art2Cell.AddElement(Paragraph2);
                Art2Cell.Colspan = 5;
                Art2Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art2Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art2Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 3
                PdfPCell Art3Cell = new PdfPCell();
                Paragraph Paragraph3 = new Paragraph();

                Paragraph3.Add(new Chunk("Artículo 3.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" El reglamento Interno de la Parroquia Nuestra Señora de Guadalupe, a fin de comprender la diversidad de conceptos utilizados durante esta normatividad expone las siguientes definiciones, entendiéndose como: \n\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("I.     Acta de defunción: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" es un documento oficial emitido por el Registro Civil en donde se registra el momento exacto de la Muerte de la persona. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("II.    Beneficiarios: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" personas que el titular establece al momento de la compra de su cripta, mismos que pueden ser modificados al momento que el Titular lo decida. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("III    Cádaver/difunto: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" cuerpo humano en el que se haya comprobado la perdida de la vida. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("IV.    Cenizas: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" resultado del proceso de cremación de restos humanos. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("V.     Columbario: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" conjunto de nichos destinados al depósito de restos humanos áridos o cremados. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("VI.    Consumidor: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" persona física o moral que adquiere o utiliza bienes y servicios funerales. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("VII.   Cremación: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" proceso mediante el cual un cadaver, restos humanos o restos aridos, se someten a tecnicas y procedimientos adecuados con la finalidad de reducirlos a cenizas. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("VIII.  Cripta: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" lugar destinado para enterrar muertos. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("IX.    Cripta comunitaria: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" estructura construida destinada al depósito de cadáveres, restos humanos áridos o cremados, que es compartida con otras 3 (tres) urnas totalmente independientes al adquiriente. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("X.     Cripta familiar: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" estructura construida destinada al depósito de cadáveres, restos humanos áridos o cremados. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XI.    Exhumación: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" extradición de un cadáver sepultado. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XII.   Inhumación: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" sepultar un cadáver. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XIII.  Nichos: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" espacio destinado al depósito de restos humanos áridos o cremados. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XIV.   Osorio: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" lugar destinado para depósito de restos humanos áridos. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XV.    Panteón: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" lugar destinado a recibir cadáveres, restos humanos y restos humanos áridos o cremados. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XVI.   Permiso de cremación: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" documento expedido por la oficialía de registro civil donde se autoriza el trámite de cremación. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XVII.  Restos áridos: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" osamenta de un cadáver como resultado del proceso natural de descomposición de un cuerpo. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XVIII. Restos áridos: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" a la(s) persona(s) designadas por el consumidor/cliente, o en su caso por el beneficiario, para que al fallecer les proporcionen los servicios funerarios contratados a futuro. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.Add(new Chunk("XIX.   Restos áridos: ", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph3.Add(new Chunk(" lugar destinado a la velación de cadáveres. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph3.SetLeading(1, 1);
                Art3Cell.AddElement(Paragraph3);
                Art3Cell.Colspan = 5;
                Art3Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art3Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art3Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PAGINA 2
                MainTable.AddCell(ImageTableCell);

                //TÍTULO PRIMERO
                PdfPCell FirstTitle = new PdfPCell(new Phrase("TÍTULO PRIMERO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                FirstTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FirstTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                FirstTitle.Colspan = 3;
                MainTable.AddCell(FirstTitle);

                //PARTES
                PdfPCell PartsTitle = new PdfPCell(new Phrase("PARTES", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                PartsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PartsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                PartsTitle.Colspan = 3;
                MainTable.AddCell(PartsTitle);

                //ASOCIACION Y ADQUIRIENTE
                PdfPCell AssociationPurcharserTitle = new PdfPCell(new Phrase("ASOCIACION Y ADQUIRIENTE", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                AssociationPurcharserTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AssociationPurcharserTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                AssociationPurcharserTitle.Colspan = 3;
                MainTable.AddCell(AssociationPurcharserTitle);

                //ARTICULO 4
                PdfPCell Art4Cell = new PdfPCell();
                Paragraph Paragraph4 = new Paragraph();

                Paragraph4.Add(new Chunk("Artículo 4.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph4.Add(new Chunk(" El columbario de la Nueva Catedral de Tijuana es propiedad de la Parroquia Iglesia Guadalupe del Rio en Tijuana, A.R., quien además es la Titular para prestar y administrar el servicio público de depósito " +
                "y extracción de urnas cuyo contenido sea únicamente el de restos humanos secos e incinerados, la administración determinara las modalidades y forma de uso de los muebles e inmuebles del Columbario.", 
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph4.SetLeading(1, 1);
                Art4Cell.AddElement(Paragraph4);
                Art4Cell.Colspan = 5;
                Art4Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art4Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art4Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 5
                PdfPCell Art5Cell = new PdfPCell();
                Paragraph Paragraph5 = new Paragraph();

                Paragraph5.Add(new Chunk("Artículo 5.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph5.Add(new Chunk(" Los usuarios de los servicios del columbario se sujetarán a lo dispuesto en el presente reglamento, obligándose a su cumplimiento. La administración del columbario se reserva el derecho de rehusar la admisión al mismo de cualquier persona, " +
                "a excepción de los titulares de derechos de uso de las criptas, sus familiares y allegados.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph5.SetLeading(1, 1);
                Art5Cell.AddElement(Paragraph5);
                Art5Cell.Colspan = 5;
                Art5Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art5Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art5Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO SEGUNDO
                PdfPCell SecondTitle = new PdfPCell(new Phrase("TÍTULO SEGUNDO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
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

                //ARTICULO 6
                PdfPCell Art6Cell = new PdfPCell();
                Paragraph Paragraph6 = new Paragraph();

                Paragraph6.Add(new Chunk("Artículo 6.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph6.Add(new Chunk(" Las puertas de acceso al columbario se abrirán a las 8:00 (ocho) horas y cerrarán a las 19:00 (diecinueve) horas, en cuyo lapso deberán " +
                "celebrarse el depósito de urnas con cenizas de restos humanos, en la inteligencia de que los servicios se harán de acuerdo con lo establecido en el Articulo 16 de este reglamento.", 
                new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph6.SetLeading(1, 1);
                Art6Cell.AddElement(Paragraph6);
                Art6Cell.Colspan = 5;
                Art6Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art6Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art6Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 7
                PdfPCell Art7Cell = new PdfPCell();
                Paragraph Paragraph7 = new Paragraph();

                Paragraph7.Add(new Chunk("Artículo 7.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph7.Add(new Chunk(" ¿Por causas de fuerza mayor, por caso fortuito, por indicaciones de autoridades competentes o actividades arquidiocesanas la asociación se reserva el derecho de modificación del horario, " +
                "en las instalaciones de la Parroquia Iglesia Nuestra Señora de Guadalupe en Zona Rio A. R.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph7.SetLeading(1, 1);
                Art7Cell.AddElement(Paragraph7);
                Art7Cell.Colspan = 5;
                Art7Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art7Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art7Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO TERCERO
                PdfPCell ThirdTitle = new PdfPCell(new Phrase("TÍTULO TERCERO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ThirdTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ThirdTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ThirdTitle.Colspan = 3;
                MainTable.AddCell(ThirdTitle);

                //BENEFICIOS Y OBLIGACIONES DEL ADQUIRIENTE
                PdfPCell BenefitsObligationsTitle = new PdfPCell(new Phrase("BENEFICIOS Y OBLIGACIONES DEL ADQUIRIENTE", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                BenefitsObligationsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                BenefitsObligationsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                BenefitsObligationsTitle.Colspan = 3;
                MainTable.AddCell(BenefitsObligationsTitle);

                //ARTICULO 8
                PdfPCell Art8Cell = new PdfPCell();
                Paragraph Paragraph8 = new Paragraph();

                Paragraph8.Add(new Chunk("Artículo 8.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph8.Add(new Chunk(" Este título corresponde a todos los beneficios que el adquirente contrae al momento de la firma del contrato de adhesión a perpetuidad con la Asociación. \n\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.Add(new Chunk("I.    Acta de defunción: El adquiriente tendrá derecho a un Nicho a perpetuidad. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.Add(new Chunk("II.   Se tiene como beneficio la celebración de Aniversario luctuoso sin costo, que consiste en ofrecer una intención en la Santa Misa del día. La notificación será al Titular, primeramente, si este ya falleció se le notificara al beneficiario en turno. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.Add(new Chunk("III.  El adquirente y su familia tendrán talleres de duelo gratuitos, siendo canalizados con personas especializadas en situaciones del proceso de duelo. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.Add(new Chunk("IV.   El adquirente y su familia tendrán el beneficio de la Dirección espiritual por un sacerdote, religioso o religiosa, asignado por la administración de la parroquia, previamente solicitándolo ante la oficina parroquial. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.Add(new Chunk("V.    El adquiriente y su familia podrá tener el sacramento de la confesión previa cita en la oficina parroquial o los días establecidos por la administración, con los requisitos que obliga este sacramento. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.Add(new Chunk("VI.   El adquiriente tiene la capacidad para designar los espacios específicos que él considere en la cripta que adquirió, a través de un documento a mano, firmado por él mismo y presentado en original a la oficinas del departamento de Criptas, sin modificación durante o después de vida, siempre que existan espacios disponibles. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.Add(new Chunk("VII.  El adquiriente tendrá la certeza de que toda urna será cuidada, salvaguardada, custodiada y se le dará un lugar digno para su protección. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.Add(new Chunk("VIII. El titular tiene capacidad previo acuerdo con la administración sobre las urnas y la cripta que el designe, y no los beneficiarios para extraer urna con cenizas o modificaciones, correcciones o recisiones al contrato original. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph8.SetLeading(1, 1);
                Art8Cell.AddElement(Paragraph8);
                Art8Cell.Colspan = 5;
                Art8Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art8Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art8Cell);

                //PAGINA 3
                MainTable.AddCell(ImageTableCell);

                //ARTICULO 9
                PdfPCell Art9Cell = new PdfPCell();
                Paragraph Paragraph9 = new Paragraph();

                Paragraph9.Add(new Chunk("Artículo 9.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph9.Add(new Chunk(" Este título corresponde a las obligaciones que el adquirente contrae al momento de la firma del contrato de adhesión a perpetuidad con la Asociación. \n\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.Add(new Chunk("I.    A partir de la fecha de la firma del contrato, el titular se compromete a aportar una cuota anual por tiempo indefinido, que servirá para el mantenimiento de áreas internas y externas de las instalaciones del columbario. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.Add(new Chunk("II.   En caso de Muerte del titular, dicha obligación será adquirida por el primer beneficiario en la lista de los registrados por el titular, si hay negación se pasará al que sigue, hasta terminar con los beneficiarios. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.Add(new Chunk("III.  Las aportaciones por mantemiento según la clausula 4.1.9 del contrato del adhesión a perpetuidad, los precios de una cripta familiar será de $50.00 dólares y $30.00 dólares en caso de una cripta comunitaria o sus equivalentes en moneda nacional. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.Add(new Chunk("IV.   Cubrir el pago por deposito, extracción o traslado de urna. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.Add(new Chunk("V.    Para el resguardo de una primera urna se dará el depósito del 50% del total del precio de la cripta y el 100% para el uso subsecuente de los demás beneficiarios. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.Add(new Chunk("VI.   En caso de modificación, corrección o recisión al contrato de adhesión a perpetuidad por parte del ADQUIRIENTE, los gastos correrán por la parte interesada. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.Add(new Chunk("VII.  En caso del fallecimiento del titular, los beneficiarios asumen las obligaciones del titular y no sus beneficios que cambien su voluntad. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.Add(new Chunk("VIII. En caso de que nos beneficios no asuman las obligaciones del titular, mencionadas en las fracciones anteriores, el destino de las urnas contenidas en la cripta correspondiente, queda reservada a la administración de la Parroquia. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph9.SetLeading(1, 1);
                Art9Cell.AddElement(Paragraph9);
                Art9Cell.Colspan = 5;
                Art9Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art9Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art9Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO CUARTO
                PdfPCell FourthTitle = new PdfPCell(new Phrase("TÍTULO CUARTO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                FourthTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FourthTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                FourthTitle.Colspan = 3;
                MainTable.AddCell(FourthTitle);

                //OBLIGACIONES DE LA ASOCIACIÓN
                PdfPCell AssociationObligationsTitle = new PdfPCell(new Phrase("OBLIGACIONES DE LA ASOCIACIÓN", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                AssociationObligationsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                AssociationObligationsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                AssociationObligationsTitle.Colspan = 3;
                MainTable.AddCell(AssociationObligationsTitle);

                //ARTICULO 10
                PdfPCell Art10Cell = new PdfPCell();
                Paragraph Paragraph10 = new Paragraph();

                Paragraph10.Add(new Chunk("Artículo 10.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph10.Add(new Chunk(" Son obligaciones de la Parroquia Nuestra Señora de Guadalupe A.R, en Zona Rio, Tijuana, Baja California: \n\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph10.Add(new Chunk("I.   Llevar un registro (expediente) donde aparezca: Información de adquiriente, beneficiarios, póliza de mantenimiento, registro de depósitos (cenizas) que allí se tengan, donde este identificado con los siguientes datos: nombre completo, fecha de nacimiento, fecha de defunción, copia del acta de defunción y copia del permiso de cremación.  \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph10.Add(new Chunk("II.  Mantener y conservar en condiciones de higiene y limpieza del columbario, así como la seguridad de la zona donde se encuentren las criptas. \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph10.Add(new Chunk("III. Mantener la zona del columbario iluminada, segura, monitoreada por Sistema de vigilancia de circuito cerrado las 24 horas.  \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph10.Add(new Chunk("IV.  Y las demás naturales a las obligaciones de la Asociación Iglesia de Guadalupe del Rio en Tijuana A.R.  \n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph10.SetLeading(1, 1);
                Art10Cell.AddElement(Paragraph10);
                Art10Cell.Colspan = 5;
                Art10Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art10Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art10Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO QUINTO
                PdfPCell FifthTitle = new PdfPCell(new Phrase("TÍTULO QUINTO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                FifthTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                FifthTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                FifthTitle.Colspan = 3;
                MainTable.AddCell(FifthTitle);

                //DEPOSITO Y RETIRO DE URNAS
                PdfPCell DepositRetiredUrnsTitle = new PdfPCell(new Phrase("DEPOSITO Y RETIRO DE URNAS", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                DepositRetiredUrnsTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                DepositRetiredUrnsTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                DepositRetiredUrnsTitle.Colspan = 3;
                MainTable.AddCell(DepositRetiredUrnsTitle);

                //ARTICULO 11
                PdfPCell Art11Cell = new PdfPCell();
                Paragraph Paragraph11 = new Paragraph();

                Paragraph11.Add(new Chunk("Artículo 11.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph11.Add(new Chunk(" El servicio de depósito de urnas con los restos incinerados de las personas fallecidas, se solicitará a la administración con una anticipación no menor de 48 de horas, " +
                "en el entendido de que este servicio no deberá realizarse en ningún caso antes de las 10:00 (diez) horas y después de las 19:00 (diecinueve) horas. \n\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph11.SetLeading(1, 1);
                Art11Cell.AddElement(Paragraph11);
                Art11Cell.Colspan = 5;
                Art11Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art11Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art11Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PAGINA 4
                MainTable.AddCell(ImageTableCell);

                //ARTICULO 12
                PdfPCell Art12Cell = new PdfPCell();
                Paragraph Paragraph12 = new Paragraph();

                Paragraph12.Add(new Chunk("Artículo 12.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph12.Add(new Chunk(" El DEPOSITO Y RETIRO de urnas, solo se podrá efectuarse mediante solicitud expresa del TITULAR del CONTRATO DE CESIÓN DE DERECHOS de uso Mortuorio a Perpetuidad o la(s) personas (s) que este haya autorizado ESPECIFICAMENTE por escrito a su fallecimiento, " +
                "además para autorizar el depósito de urnas, la administración del columbario le exigirá al Titular comprobar que están al corriente del pago, de presentar una petición por escrito y en su caso de exhibir el título de derecho de uso mortuorio a perpetuidad y estar apegado a los términos " +
                "y condiciones que fijen las autoridades de Salubridad y Municipales respectivamente. \n\n", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph12.SetLeading(1, 1);
                Art12Cell.AddElement(Paragraph12);
                Art12Cell.Colspan = 5;
                Art12Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art12Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art12Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 13
                PdfPCell Art13Cell = new PdfPCell();
                Paragraph Paragraph13 = new Paragraph();

                Paragraph13.Add(new Chunk("Artículo 13.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph13.Add(new Chunk(" EL DEPOSITO Y RETIRO de cenizas,", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph13.Add(new Chunk(" solo podrá ser efectuado por el personal autorizado por el director del Proyecto de la Nueva Catedral Metropolitana de Tijuana,", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph13.Add(new Chunk(" bajo su control y vigilancia, ante la presencia del titular o de quien éste haya designado por escrito como titular y/o cuando medie y/o presente a la administración del columbario PODER NOTARIAL.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph13.SetLeading(1, 1);
                Art13Cell.AddElement(Paragraph13);
                Art13Cell.Colspan = 5;
                Art13Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art13Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art13Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 14
                PdfPCell Art14Cell = new PdfPCell();
                Paragraph Paragraph14 = new Paragraph();

                Paragraph14.Add(new Chunk("Artículo 14.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph14.Add(new Chunk(" El servicio de depósito de urnas consistirá en lo siguiente: La colocación de la tapa de la cripta, placa grabada con el nombre y fechas correspondientes de nacimiento y deceso de cada persona fallecida depositada en la cripta, este grabado será de acuerdo a las normas y especificaciones aprobadas por la administración del Columbario.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph14.SetLeading(1, 1);
                Art14Cell.AddElement(Paragraph14);
                Art14Cell.Colspan = 5;
                Art14Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art14Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art14Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 15
                PdfPCell Art15Cell = new PdfPCell();
                Paragraph Paragraph15 = new Paragraph();

                Paragraph15.Add(new Chunk("Artículo 15.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph15.Add(new Chunk(" Cuando un servicio de depósito de urna no pudiera realizarse por causas de fuerza mayor o por caso fortuito, en el lugar escogido por el titular o sus beneficiarios de acuerdo con el contrato original, aun cuando ya se hubieran liquidado los pagos respectivos, la administración señalara la cripta que deba hacerse a fin de no demorar el evento, si así lo disponen los familiares.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph15.Add(new Chunk(" Dicha cripta será de carácter “provisional” y no será inferior al valor del contrato original.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph15.SetLeading(1, 1);
                Art15Cell.AddElement(Paragraph15);
                Art15Cell.Colspan = 5;
                Art15Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art15Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art15Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 16
                PdfPCell Art16Cell = new PdfPCell();
                Paragraph Paragraph16 = new Paragraph();

                Paragraph16.Add(new Chunk("Artículo 16.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph16.Add(new Chunk(" La administración no será responsable respecto a la identidad de la persona de quien se deposita los restos incinerados. La persona o personas que hayan gestionado el servicio de depósito de la urna serán las únicas responsables de la identidad de las personas fallecidas.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph16.SetLeading(1, 1);
                Art16Cell.AddElement(Paragraph16);
                Art16Cell.Colspan = 5;
                Art16Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art16Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art16Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //TÍTULO SEXTO
                PdfPCell SixthTitle = new PdfPCell(new Phrase("TÍTULO SEXTO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                SixthTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SixthTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                SixthTitle.Colspan = 3;
                MainTable.AddCell(SixthTitle);

                //CUIDADO DEL COLUMBARIO
                PdfPCell ColumbariumCareTitle = new PdfPCell(new Phrase("CUIDADO DEL COLUMBARIO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                ColumbariumCareTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
                ColumbariumCareTitle.HorizontalAlignment = Element.ALIGN_CENTER;
                ColumbariumCareTitle.Colspan = 3;
                MainTable.AddCell(ColumbariumCareTitle);

                //ARTICULO 17
                PdfPCell Art17Cell = new PdfPCell();
                Paragraph Paragraph17 = new Paragraph();

                Paragraph17.Add(new Chunk("Artículo 17.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph17.Add(new Chunk(" La Parroquia Iglesia Guadalupe del Rio en Tijuana, A.R., como prestación a la cuota de mantenimiento estipulada en el contrato original en la cláusula 4.1.9, La Asociación garantiza los gastos de mantenimiento del columbario, así como todos los servicios de depósito y extracción de urnas, " +
                "con los servicios colaterales a estos y los que fueron vendidos según el contrato correspondiente y debidamente firmado por los interesados.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph17.SetLeading(1, 1);
                Art17Cell.AddElement(Paragraph17);
                Art17Cell.Colspan = 5;
                Art17Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art17Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art17Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 18
                PdfPCell Art18Cell = new PdfPCell();
                Paragraph Paragraph18 = new Paragraph();

                Paragraph18.Add(new Chunk("Artículo 18.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph18.Add(new Chunk(" La administración se compromete a mantener un cuerpo de vigilancia diurna y nocturna del columbario. Su objeto es la preservación del orden de las criptas, así como la iglesia y capillas, incluyendo el edificio administrativo. Sin embargo, " +
                "la administración no se hace responsable de los daños que pudieran resultar por causas ajenas a su control, y en especial, los que se deriven por caso fortuito o fuerza mayor, como desastres naturales, incendios, huelgas, tumultos, terremotos, ciclones, o de actos u órdenes de las autoridades.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph18.SetLeading(1, 1);
                Art18Cell.AddElement(Paragraph18);
                Art18Cell.Colspan = 5;
                Art18Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art18Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art18Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //PAGINA 5
                MainTable.AddCell(ImageTableCell);

                //TÍTULO SÉPTIMO
                PdfPCell SeventhTitle = new PdfPCell(new Phrase("TÍTULO SÉPTIMO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
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

                //ARTICULO 19
                PdfPCell Art19Cell = new PdfPCell();
                Paragraph Paragraph19 = new Paragraph();

                Paragraph19.Add(new Chunk("Artículo 19.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph19.Add(new Chunk(" En la etapa de la cripta solo podrá escribirse el nombre de la persona fallecida con su fecha de nacimiento y fecha de defunción con el tipo de letra autorizado y que todas las placas, serán iguales, quedando excluido cualquier añadido de carácter personal. " +
                "En la tapa de la cripta no podrán colocarse floreros, veladoras, imágenes, adornos, etc., tanto en la parte exterior, así como en la interior de la misma.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph19.SetLeading(1, 1);
                Art19Cell.AddElement(Paragraph19);
                Art19Cell.Colspan = 5;
                Art19Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art19Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art19Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 20
                PdfPCell Art20Cell = new PdfPCell();
                Paragraph Paragraph20 = new Paragraph();

                Paragraph20.Add(new Chunk("Artículo 20.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph20.Add(new Chunk(" Se prohíbe la introducción y consumo de bebidas alcohólicas, de droga, cigarros y tóxicos de toda clase o especie dentro del columbario, así como la entrada al mismo de toda persona en estado de ebriedad o intoxicación y se procederá a expulsar a toda aquella persona que con hechos o " +
                "palabras causare algún escandalo o perjuicio en el interior del columbario.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph20.SetLeading(1, 1);
                Art20Cell.AddElement(Paragraph20);
                Art20Cell.Colspan = 5;
                Art20Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art20Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art20Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 21
                PdfPCell Art21Cell = new PdfPCell();
                Paragraph Paragraph21 = new Paragraph();

                Paragraph21.Add(new Chunk("Artículo 21.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph21.Add(new Chunk(" Se prohíbe arrojar basura o desperdicios en los pasillos del columbario, lo mismo colocar sobre las tapas de las criptas imágenes, porta floreros, letreros o cualquier otro artículo que", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph21.Add(new Chunk(" NO", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph21.Add(new Chunk(" este autorizado por la administración. La administración colocara recipientes para su depósito en los lugares que estime conveniente.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph21.SetLeading(1, 1);
                Art21Cell.AddElement(Paragraph21);
                Art21Cell.Colspan = 5;
                Art21Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art21Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art21Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 22
                PdfPCell Art22Cell = new PdfPCell();
                Paragraph Paragraph22 = new Paragraph();

                Paragraph22.Add(new Chunk("Artículo 22.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph22.Add(new Chunk(" No se permitirá el acceso al columbario a niños menores de diez años sin la compañía de personas mayores que cuiden de ellos.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph22.SetLeading(1, 1);
                Art22Cell.AddElement(Paragraph22);
                Art22Cell.Colspan = 5;
                Art22Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art22Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art22Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 23
                PdfPCell Art23Cell = new PdfPCell();
                Paragraph Paragraph23 = new Paragraph();

                Paragraph23.Add(new Chunk("Artículo 23.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph23.Add(new Chunk(" Se prohíbe a toda persona ajena a recoger flores ya sean silvestres o cultivadas, o trozar ramaje de los árboles, arbustos o plantas o alimentar a los pájaros u otra forma de vida animal que se encuentre dentro del terreno del columbario.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph23.SetLeading(1, 1);
                Art23Cell.AddElement(Paragraph23);
                Art23Cell.Colspan = 5;
                Art23Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art23Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art23Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 24
                PdfPCell Art24Cell = new PdfPCell();
                Paragraph Paragraph24 = new Paragraph();

                Paragraph24.Add(new Chunk("Artículo 24.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph24.Add(new Chunk(" No se permitirá dentro del columbario y jardines, el tránsito de bicicletas, motocicletas, patinetas, etc.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph24.SetLeading(1, 1);
                Art24Cell.AddElement(Paragraph24);
                Art24Cell.Colspan = 5;
                Art24Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art24Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art24Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 25
                PdfPCell Art25Cell = new PdfPCell();
                Paragraph Paragraph25 = new Paragraph();

                Paragraph25.Add(new Chunk("Artículo 25.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph25.Add(new Chunk(" No se permitirá dentro del columbario y jardines o áreas circundantes avisos, letreros, leyendas o anuncios de cualquier índole, excepto por los autorizados por la administración y autoridades competentes.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph25.SetLeading(1, 1);
                Art25Cell.AddElement(Paragraph25);
                Art25Cell.Colspan = 5;
                Art25Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art25Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art25Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 26
                PdfPCell Art26Cell = new PdfPCell();
                Paragraph Paragraph26 = new Paragraph();

                Paragraph26.Add(new Chunk("Artículo 26.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph26.Add(new Chunk(" No se permitirán el acceso de animales al columbario ni a los jardines.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph26.SetLeading(1, 1);
                Art26Cell.AddElement(Paragraph26);
                Art26Cell.Colspan = 5;
                Art26Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art26Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art26Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 27
                PdfPCell Art27Cell = new PdfPCell();
                Paragraph Paragraph27 = new Paragraph();

                Paragraph27.Add(new Chunk("Artículo 27.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph27.Add(new Chunk(" No se permitirá el acceso a orquestas, bandas, grupos musicales, radio fusiones, altoparlantes, celulares, dispositivos electrónicos o amplificadores, si no se obtiene permiso de la administración.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph27.SetLeading(1, 1);
                Art27Cell.AddElement(Paragraph27);
                Art27Cell.Colspan = 5;
                Art27Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art27Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art27Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //ARTICULO 28
                PdfPCell Art28Cell = new PdfPCell();
                Paragraph Paragraph28 = new Paragraph();

                Paragraph28.Add(new Chunk("Artículo 28.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph28.Add(new Chunk(" Los visitantes al columbario que utilicen vehículos, los deberán estacionar en las zonas de estacionamiento que para tal efecto estarán marcadas por la administración y de acuerdo con los lineamientos de las autoridades competentes. " +
                "Si el usuario invade una zona prohibida o que no le corresponde, la administración llamara a la empresa de grúas para levantar el vehículo, y los gastos que se originen serán cubiertos por el propietario o usuario del vehículo.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph28.SetLeading(1, 1);
                Art28Cell.AddElement(Paragraph28);
                Art28Cell.Colspan = 5;
                Art28Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art28Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art28Cell);

                //PAGINA 6
                MainTable.AddCell(ImageTableCell);

                //ARTICULO 29
                PdfPCell Art29Cell = new PdfPCell();
                Paragraph Paragraph29 = new Paragraph();

                Paragraph29.Add(new Chunk("Artículo 29.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph29.Add(new Chunk(" Cualquier caso o evento que no estuviere contemplado en este reglamento, y por consecuencia origine duda, problema, contrariedad, perturbación, molestia, etc., " +
                "será resuelto por el Consejo de la Parroquia Iglesia de Guadalupe del Rio en Tijuana, A.R., o por las autoridades competentes, según sea el caso.", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                Paragraph29.Add(new Chunk(" (Por mediación a través de un acuerdo firmado entre las partes).", new Font(Font.FontFamily.UNDEFINED, 10f, Font.BOLD)));
                Paragraph29.SetLeading(1, 1);
                Art29Cell.AddElement(Paragraph29);
                Art29Cell.Colspan = 5;
                Art29Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Art29Cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                MainTable.AddCell(Art29Cell);

                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                //SALTO DE LINEA
                MainTable.AddCell(CellRow);
                //SALTO DE LINEA
                MainTable.AddCell(CellRow);

                //FIRMA
                PdfPCell SigningCell = new PdfPCell();
                PdfPTable SigningTable = new PdfPTable(3);

                Line.Alignment = Element.ALIGN_CENTER;
                PdfPCell SingingLineCell = new PdfPCell();
                SingingLineCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SingingLineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                SingingLineCell.AddElement(Line);
                SingingLineCell.Colspan = 3;

                PdfPCell SigningPhraseCell = new PdfPCell(new Phrase("EL TITULAR" + "\n" + "(Nombre y Firma)", new Font(Font.FontFamily.UNDEFINED, 10f, Font.NORMAL)));
                SigningPhraseCell.Colspan = 3;
                SigningPhraseCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SigningPhraseCell.HorizontalAlignment = Element.ALIGN_CENTER;

                SigningTable.AddCell(SingingLineCell);
                SigningTable.AddCell(SigningPhraseCell);

                SigningCell.AddElement(SigningTable);
                SigningCell.Colspan = 3;
                SigningCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SigningCell.HorizontalAlignment = Element.ALIGN_CENTER;

                MainTable.AddCell(SigningCell);

                doc.Add(MainTable);

                writer.CloseStream = false;
                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ReglamentoDelColumbario.pdf");
                Response.Write(doc);
                Response.End();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View();
        }

        // GET: Regulation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Regulation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Regulation/Create
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

        // GET: Regulation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Regulation/Edit/5
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

        // GET: Regulation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Regulation/Delete/5
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
