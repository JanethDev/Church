using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Church.Business;
using Church.Data;
using Church.Helpers;
using System.Linq;

namespace Church.Controllers
{
    [Authorize(Roles = "Administrador,Encargado")]
    public class ScheduleController : BaseController
    {
        ScheduleB ScheduleB = new ScheduleB();
        HelpMethods hm = new HelpMethods();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        // GET: Schedule
        public ActionResult Details()
        {
            var ListSchedules = ScheduleB.GetList();
            return View(ListSchedules);
        }
        public ActionResult Edit(int DayNumber)
        {
            ViewBag.DayNumber = DayNumber;
            var ListSchedules = ScheduleB.GetListByDayNumber(DayNumber);
            return View(ListSchedules);
        }
        [HttpPost]
        public ActionResult Edit(int DayNumber, string Day, int[] ScheduleID, string[] Hour)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);
            var ListSchedules = ScheduleB.GetListByDayNumber(DayNumber);
            for(int i =0; i < ScheduleID.Length; i++)
            {
                if(ScheduleID[i] > 0)
                {
                    tblSchedule tblSchedule = ListSchedules.Where(r => r.ScheduleID == ScheduleID[i]).FirstOrDefault();
                    if (Hour[i] != ListSchedules[i].Hour)
                    {
                        tblSchedule.Hour = Hour[i];
                        tblSchedule.UpdatedBy = LoggedUser;
                        tblSchedule.UpdatedDate = ServerDateTimeNow;
                        ScheduleB.Update(tblSchedule);
                    }
                }
                else
                {
                    tblSchedule tblSchedule = new tblSchedule();
                    tblSchedule.DayNumber = DayNumber;
                    tblSchedule.Day = Day;
                    tblSchedule.Hour = Hour[i];
                    tblSchedule.Active = true;
                    tblSchedule.CreatedBy = LoggedUser;
                    tblSchedule.CreatedDate = ServerDateTimeNow;
                    tblSchedule.UpdatedBy = LoggedUser;
                    tblSchedule.UpdatedDate = ServerDateTimeNow;
                    ScheduleB.Create(tblSchedule);
                }
            }
            foreach (var Schedule in ListSchedules)
            {
                if (!ScheduleID.Where(r => r == Schedule.ScheduleID).Any())
                {
                    Schedule.Active = false;
                    Schedule.UpdatedBy = LoggedUser;
                    Schedule.UpdatedDate = ServerDateTimeNow;
                    ScheduleB.Update(Schedule);
                }
            }
            return RedirectToAction("Details");
        }
    }
}