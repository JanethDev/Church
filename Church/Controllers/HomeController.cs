using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Docs;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize]

    public class HomeController : BaseController
    {
        AnniversaryB AnniversaryB = new AnniversaryB();
        IntentionsB IntentionsB = new IntentionsB();
        AshDepositsB AshDepositsB = new AshDepositsB();
        public ActionResult GetListIntentionsByYearAndMonth(int Year, int Month)
        {
            List<tblIntentions> lst = IntentionsB.GetListByYearAndMonth(Year, Month);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAnniversaryByYearAndMonth(int Year, int Month)
        {
            var model = AnniversaryB.GetListByYearAndMonth(Year, Month);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAshDepositByYearAndMonth(int Year, int Month)
        {
            var model = AshDepositsB.GetListByYearAndMonth(Year, Month);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //[MethodDescription( DescriptionES.INDEX_SHOWS_LIST_OF_NOF, DescriptionES.USERS)]
        public ActionResult Index(string error = "")
        {
            DateTime now = DateTime.Now;
            //TechnicalDocumentation.GenerateDoc("Church", AppDomain.CurrentDomain.BaseDirectory, "Controllers");
            //TechnicalDocumentation.GenerateDoc("Church.Business", AppDomain.CurrentDomain.BaseDirectory, string.Empty);
            var calendarToday = now.Year.ToString() + "/" + now.Month.ToString() + "/" + now.Day.ToString();
            ViewBag.ErrorMessage = error;
            ViewBag.today = calendarToday;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}