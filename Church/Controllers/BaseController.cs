using Church.Helpers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Church.Business;
using Church.Data;

namespace Church.Controllers
{
    public class BaseController : Controller
    {
        PurchasesRequestsB PurchasesRequestsB = new PurchasesRequestsB();
        PaymentsB PaymentsB = new PaymentsB();
        public string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.PurchasesPendings = PurchasesRequestsB.GetPurchasesRequestNotCheckCount();
            ViewBag.PaymentsPendings = PaymentsB.GetListByEstatusPendienteCount();
        }
        //public byte[] GetReport(string title, string reportType, Dictionary<string, DataTable> dictDataTables, string reportRDLCName, bool Descargar = true)
        //{
        //    byte[] renderedBytes = new byte[0];

        //    string path = Path.Combine(Server.MapPath("~/Reports"), $"{reportRDLCName}.rdlc");
        //    ReportViewer rpv = new ReportViewer();
        //    rpv.LocalReport.EnableExternalImages = true;
        //    if (System.IO.File.Exists(path))
        //    {
        //        rpv.LocalReport.ReportPath = path;
        //    }


        //    foreach (var item in dictDataTables)
        //    {
        //        rpv.LocalReport.DataSources.Add(new ReportDataSource(item.Key, item.Value));
        //    }

        //    string mimeType = "";
        //    string encoding = "";
        //    string fileNameExtension = "";
        //    string deviceInfo =
        //                "<DeviceInfo>" +
        //                "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //                "  <PageWidth>8.27in</PageWidth>" +
        //                "  <PageHeight>11.69in</PageHeight>" +
        //                "  <MarginTop>0.6in</MarginTop>" +
        //                "  <MarginLeft>0.01n</MarginLeft>" +
        //                "  <MarginRight>0.01in</MarginRight>" +
        //                "  <MarginBottom>0.6in</MarginBottom>" +
        //                "  <HumanReadablePDF>True</HumanReadablePDF>" +
        //                "</DeviceInfo>";

        //    Warning[] warnings = null;
        //    string[] streams = null;
        //    renderedBytes = rpv.LocalReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        //    if (!Descargar)
        //        return renderedBytes;

        //    Response.ClearContent();
        //    Response.Buffer = true;

        //    if (reportType == ReportType.PDF)
        //    {
        //        Response.AddHeader("content-disposition", "inline; filename=Report_ " + title + ".pdf");
        //        Response.ContentType = "application/pdf";
        //    }
        //    else
        //    {
        //        Response.AddHeader("content-disposition", "inline; filename=Report_" + title + ".xls");
        //        Response.ContentType = "application/vnd.ms-excel";
        //    }

        //    Response.Charset = "UTF-8";
        //    Response.BinaryWrite(renderedBytes);
        //    Response.End();

        //    return null;
        //}

        //public DataTable GetdtColumnNames(List<ColumnNamesReport> lstColumnNames)
        //{
        //    DataTable DTHeader = new DataTable();
        //    foreach (var item in lstColumnNames)
        //        DTHeader.Columns.Add(new DataColumn(item.ColumnName, typeof(string)));

        //    DataRow row = DTHeader.NewRow();
        //    int i = 0;
        //    foreach (var item in lstColumnNames)
        //    {
        //        row[i] = item.ColumnValue;
        //        i++;
        //    }
        //    DTHeader.Rows.Add(row);
        //    return DTHeader;
        //}
    }
}