using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using Church.Business;
using Church.Business.Jobs;
using Church.Data;
using Church.Data.Enums;
using Church.Helpers;
using Church.Data.HelperClasses;

namespace Church.Controllers
{
    public class PushNotificationsMessagesController : BaseController
    {
        PushNotificationsMessagesB PushNotificationsMessagesB = new PushNotificationsMessagesB();
        HelpMethods hm = new HelpMethods();
        UsersB UsersB = new UsersB();
        FCMManager FCMManager = new FCMManager();


        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";
        // GET: PushNotificationsMessages
        public ActionResult Index(string Title = "", string Message = "", string Date = "", string Status = "", string Sorting_Order = "", int Page = 1)
        {
            tblUsers tblUsers = UsersB.GetUserByID(hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table));

            ViewBag.Title = Title;
            ViewBag.Message = Message;
            ViewBag.Date = Date;
            ViewBag.Status = Status;
            ViewData["Statuses"] = Enum.GetValues(typeof(PushNotificationsStatusE)).Cast<PushNotificationsStatusE>().ToList();

            ViewBag.Sorting_Order = Sorting_Order;
            ViewBag.SortingID = Sorting_Order == "tblPushNotificationsMessages.PushNotificationMessageID" ? "tblPushNotificationsMessages.PushNotificationMessageID_Desc" : "tblPushNotificationsMessages.PushNotificationMessageID";
            ViewBag.SortingTitle = Sorting_Order == "tblPushNotificationsMessages.Title" ? "tblPushNotificationsMessages.Title_Desc" : "tblPushNotificationsMessages.Title";
            ViewBag.SortingMessage = Sorting_Order == "tblPushNotificationsMessages.Message" ? "tblPushNotificationsMessages.Message_Desc" : "tblPushNotificationsMessages.Message";
            ViewBag.SortingDraft = Sorting_Order == "tblPushNotificationsMessages.Draft" ? "tblPushNotificationsMessages.Draft_Desc" : "tblPushNotificationsMessages.Draft";
            ViewBag.SortingCreatedDate = Sorting_Order == "tblPushNotificationsMessages.CreatedDate" ? "tblPushNotificationsMessages.CreatedDate_Desc" : "tblPushNotificationsMessages.CreatedDate";

            if (!String.IsNullOrEmpty(Status)) { Status = Status == "Borrador" ? "1" : "0"; }
            DateTime dtDate = new DateTime(); if (!String.IsNullOrEmpty(Date)) { dtDate = DateTime.Parse(Date); }

            StaticPagedList<dtoPushNotificationsMessages> tblPushNotificationsMessages = PushNotificationsMessagesB.GetPushNotificationsMessages(Sorting_Order, Title, Message, dtDate, Status, tblUsers, Page);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PushNotificationsMessages", tblPushNotificationsMessages);
            }

            return View(tblPushNotificationsMessages);
        }

        public ActionResult Create()
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblPushNotificationsMessages tblPushNotificationsMessages = new tblPushNotificationsMessages()
            {
                Active = true,
                CreatedBy = LoggedUser,
                CreatedDate = ServerDateTimeNow,
                UpdatedBy = LoggedUser,
                UpdatedDate = ServerDateTimeNow,
            };

            return View(tblPushNotificationsMessages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblPushNotificationsMessages tblPushNotificationsMessages, List<string> Users = null)
        {
            Users = UsersB.GetList().Select(r=>r.UserName).ToList();
            if (ModelState.IsValid)
            {
                Response Response = PushNotificationsMessagesB.Create(tblPushNotificationsMessages);

                if (Response.Result == Result.OK)
                {
                    SendNotificationsAsync(tblPushNotificationsMessages);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Ocurrió un error");
                return View(tblPushNotificationsMessages);
            }
        }

        public ActionResult Details(int id)
        {
            ViewBag.Roles = RolesB.GetList();
            tblPushNotificationsMessages tblPushNotificationsMessages = PushNotificationsMessagesB.GetById(id);
            return View(tblPushNotificationsMessages);
        }

        public ActionResult Delete(int id)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);

            tblPushNotificationsMessages tblPushNotificationsMessages = PushNotificationsMessagesB.GetById(id);
            tblPushNotificationsMessages.DeletedBy = LoggedUser;
            tblPushNotificationsMessages.DeletedDate = ServerDateTimeNow;
            tblPushNotificationsMessages.Active = false;
            Response Response = PushNotificationsMessagesB.Update(tblPushNotificationsMessages);

            if (Response.Result == Result.OK)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Ocurrió un error al actualizar los datos.");
                return RedirectToAction("Details", new { id = id });
            }
        }

        public ActionResult Edit(int id)
        {
            int LoggedUser_Rol = hm.GetIntbyString("UserName", HttpContext.User.Identity.Name, "RolID", "tblUsers");
            tblPushNotificationsMessages tblPushNotificationsMessages = PushNotificationsMessagesB.GetById(id);

            return View(tblPushNotificationsMessages);
        }

        [HttpPost]
        public ActionResult Edit(tblPushNotificationsMessages tblPushNotificationsMessages)
        {
            DateTime ServerDateTimeNow = hm.GetDateTime();
            int LoggedUser = hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table);
            List<string> Users = UsersB.GetList().Select(r => r.UserName).ToList();
            if (ModelState.IsValid)
            {
                Response Response = PushNotificationsMessagesB.Update(tblPushNotificationsMessages);

                if (Response.Result == Result.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar los datos.");
                    return RedirectToAction("Edit", new { id = tblPushNotificationsMessages.PushNotificationMessageID });
                }
            }
            else
            {
                ModelState.AddModelError("", "Ocurrió un error");
                return View(tblPushNotificationsMessages);
            }
        }
        private async Task<ActionResult> SendNotificationsAsync(tblPushNotificationsMessages tblPushNotificationsMessages)
        {
            List<tblUsers> lstUsers = UsersB.GetList().Where(r => r.NotificationKey != null).ToList();

            List<dtoNotificationsByUser> lstNotificationsByUser = new List<dtoNotificationsByUser>();
            foreach (tblUsers U in lstUsers)
            {
                dtoNotificationsByUser NotificationByUser = new dtoNotificationsByUser();
                NotificationByUser = new dtoNotificationsByUser();
                NotificationByUser.NotificationID = tblPushNotificationsMessages.PushNotificationMessageID;
                NotificationByUser.NotificationKey = U.NotificationKey;
                NotificationByUser.UserID = U.UserID;
                NotificationByUser.Message = tblPushNotificationsMessages.Title;
                NotificationByUser.Activity = "Activity_Push_Notifications_Messages";

                lstNotificationsByUser.Add(NotificationByUser);
                break;
            }
            foreach (tblUsers U in lstUsers)
            {
                var vPushNotification = new PushNotification
                {
                    Data = tblPushNotificationsMessages.Message,
                    PushNotificationType = PushNotificationType.Message
                };
                var response = await FCMManager.SendNotificationAsync(vPushNotification, U.NotificationKey);
            }

            return null;
        }

    }
}