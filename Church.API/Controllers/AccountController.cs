using Church.API.Filters;
using Church.Business;
using Church.Data;
using Church.Data.DTO.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Church.API.Controllers
{
    [HMACAuthentication]
    [RoutePrefix("api/Account")]
    public class AccountController :  ApiController
    {
        private readonly UsersB UsersB = new UsersB();
        private readonly HelpMethods HelpMethods = new HelpMethods();

        [HttpPost]
        [Route("user/Login/")]
        public IHttpActionResult Post(tblLogin login)
        {
            Response response = new Response();

            try
            {
                HelpMethods hm = new HelpMethods();
                string contraseña = UsersB.GetByUsername_RolId(login.UserName, 7)?.Password;
                if (contraseña == null)
                {
                    ModelState.AddModelError("", "El usuario no existe.");
                    response.Result = Result.INVALID_PASSWORD;
                }
                else
                {
                    bool validated = PasswordSecurity.ValidatePassword(login.Password, contraseña);

                    if (validated)
                    {
                        dtoUsers user = UsersB.GetUserByUserName(login.UserName);

                        response.Result = Result.OK;
                        response.Data = JsonConvert.SerializeObject(user);

                    }
                    else
                    {
                        //Contrasena Incorrecta
                        response.Result = Result.INVALID_PASSWORD;
                    }
                }

            }
            catch (Exception ex)
            {
                response.Result = Result.EXCEPTION; //Exception
                response.Data = ex.Message;
            }

            return Ok(response);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        [HttpPut]
        [Route("UpdateNotificationKeys/{id}")]
        public IHttpActionResult Put(int id, tblUsersDTO model)
        {
            tblUsers CurrentUser = new tblUsers();
            Response r = new Response();
            try
            {
                CurrentUser = UsersB.GetUserByID(id);

                if (CurrentUser != null && model != null)
                {
                    CurrentUser.UpdatedDate = HelpMethods.GetDateTime();
                    CurrentUser.NotificationKey = model.NotificationKey;
                    CurrentUser.NotificationKeyName = model.NotificationKeyName;
                    CurrentUser.ConfirmPassword = CurrentUser.Password;
                    Response response = UsersB.Update(CurrentUser);
                    response.Result = response.Data == "" ? Result.OK : Result.ERROR;
                }
            }
            catch (Exception ex)
            {
                r.Result = Result.EXCEPTION;
                r.Data = ex.Message;
            }

            return Ok(r);
        }
    }
}