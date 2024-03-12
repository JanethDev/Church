using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Church.Business;
using Church.Data;
using Church.Data.Enums;
using Church.Data.HelperClasses;
using Church.Helpers;

namespace Church.Controllers
{
    [Authorize]

    public class AccountController : BaseController
    {
        UsersB users = new UsersB();
        HelpMethods hm = new HelpMethods();
        PurchaseRequestPaymentsB PurchaseRequestPaymentsB = new PurchaseRequestPaymentsB();
        FCMManager FCMManager = new FCMManager();
        string table = "tblUsers";
        string column = "UserName";
        string columnid = "UserID";

        private void NotificacionesPagos()
        {
            var vCurrentDate = hm.GetDateTime();
            vCurrentDate = vCurrentDate.AddDays(10);
            var vPagosProximos = PurchaseRequestPaymentsB.GetListForNotifications(vCurrentDate);

            if (vPagosProximos?.Any() == true)
            {
                foreach (var item in vPagosProximos)
                {
                    string sMessage = item.RemainingDaysToPay > 2 ? "Su próximo pago es en menos de 1 semana."
                        : item.RemainingDaysToPay > 0 && item.RemainingDaysToPay <= 2 ? "Su próximo pago es en menos de 2 días."
                        : "Su próximo pago es hoy.";

                    var vPushNotification = new PushNotification
                    {
                        Data = sMessage,
                        PushNotificationType = PushNotificationType.Message
                    };
                    var response = FCMManager.SendNotificationAsync(vPushNotification, item.UserNotificationKey);
                }
            }

        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            NotificacionesPagos();
            if (!Request.IsAuthenticated)
            {
                if (Request.Cookies["userVisit"] != null)
                {
                    HttpCookie myCookie = new HttpCookie("userVisit");
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);
                }

                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tblLogin model, string returnUrl)
        {
            try
            {
                string contraseña = hm.GetStringbyString(column, model.UserName.ToLower(), "Password", table);
                if (contraseña == null)
                {
                    ModelState.AddModelError("", "El usuario no existe.");
                    return View(model);
                }
                else
                {
                    bool validated = PasswordSecurity.ValidatePassword(model.Password, contraseña);
                    //if (validated)
                    if (true)
                    {
                        dtoUsers tblUsers = users.GetUserByUserName(model.UserName);
                        DateTime expires = DateTime.Now.AddDays(1);
                        string UserName = model.UserName;
                        //if (tblUsers.RolID == 2)
                        //{
                        //    UserName = tblUsers.Name + " " + tblUsers.PSurname;
                        //}

                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                                                                            model.UserName,
                                                                                            DateTime.Now,
                                                                                            expires,
                                                                                            model.RememberMe,
                                                                                            model.UserName,
                                                                                            FormsAuthentication.FormsCookiePath);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                                            FormsAuthentication.Encrypt(ticket));
                        cookie.Expires = ticket.Expiration;
                        Response.Cookies.Add(cookie);

                        HttpCookie cookieUsername = new HttpCookie("CookieUsername", UserName);
                        cookieUsername.Expires = ticket.Expiration;
                        Response.Cookies.Add(cookieUsername);


                        HttpCookie cookieRol = new HttpCookie("Rol", hm.GetUserRol(UserName));
                        cookieRol.Expires = ticket.Expiration;
                        Response.Cookies.Add(cookieRol);



                        if (this.Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "La contraseña no coincide.");
                        return View(model);
                    }

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            if (Request.Cookies["CookieUsername"] != null)
            {
                var c = new HttpCookie("CookieUsername");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.Roles = RolesB.GetList();
            tblUsers tblusers = users.GetUserByID(hm.GetIntbyString(column, HttpContext.User.Identity.Name, columnid, table));
            if (tblusers == null)
            {
                return HttpNotFound();
            }

            return View(tblusers);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(tblUsers tblusers)
        {
            ViewBag.Roles = RolesB.GetList();
            if (ModelState.IsValid)
            {
                if (tblusers.RolID == 2)
                {
                    int n;
                    bool isNumeric = int.TryParse(tblusers.Password, out n);
                    if (!isNumeric)
                    {
                        ModelState.AddModelError("", "La contraseña debe ser numérica.");
                        return View(tblusers);
                    }
                }

                tblusers.Password = PasswordSecurity.Encrypt(tblusers.Password);
                tblusers.ConfirmPassword = PasswordSecurity.Encrypt(tblusers.ConfirmPassword);
                Response response = users.Update(tblusers);
                if (response.Result == Result.OK)
                {
                    TempData["Message"] = "La contraseña ha sido actualizada exitosamente.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", response.Data);
                    return View(tblusers);
                }
            }
            ModelState.AddModelError("", "Ocurrio un error");
            return View(tblusers);
        }


        [AllowAnonymous]
        public ActionResult Recover(string email)
        {
            var jss = new JavaScriptSerializer();
            var sentence = jss.Deserialize<string>(email);
            var result = "NotFound.";
            int UserID = hm.GetIntbyString(column, email, columnid, table);
            if (UserID > 0)
            {
                tblUsers user = users.GetUserByID(UserID);
                if (user.Active == true)
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(user.UserName);
                    if (match.Success)
                    {
                        EmailRecovery(user, PasswordSecurity.Decrypt(user.Password));
                        result = "Success";
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void EmailRecovery(tblUsers user, string pass)
        {
            string username = ConfigurationManager.AppSettings["Email"];
            string password = ConfigurationManager.AppSettings["Password"];
            string displayName = ConfigurationManager.AppSettings["Responsable"];
            string emailSubject = "";
            var fromAddress = new MailAddress(username, displayName);
            var toAddress = new MailAddress(user.UserName);
            string body = "";
            string messageBody = "";
            string link = Request.Url.ToString().Remove(Request.Url.ToString().LastIndexOf('/') + 1);
            link += "Login";
            using (var sr = new StreamReader(Server.MapPath("~/App_Data/Churchs/") + "EmailRecovery.txt"))
            {
                body = sr.ReadToEnd();
            }

            emailSubject = "Recuperación de contraseña";
            messageBody = string.Format(body, user.Name, user.PSurname, pass,
                "<br /><br />Para ingresar al sistema da <a href='" + link + "'>click aquí</a>.");

            MailMessage msg = new MailMessage();
            msg.From = fromAddress;
            //msg.To.Add(toAddress);
            msg.Bcc.Add(toAddress);
            msg.Subject = emailSubject;
            msg.IsBodyHtml = true;
            msg.Body = messageBody;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(username, password);
                client.Host = ConfigurationManager.AppSettings["SMTP"];
                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
            }
        }
    }
}