using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Church.Data;

namespace Church.Business
{
    public class CustomRoleProvider : RoleProvider
    {

        HelpMethods hm = new HelpMethods();
 

        public override string ApplicationName { get; set; }
        public override bool IsUserInRole(string id, string roleName)
        {

            return false;
            //try
            //{
            //    using (var db = new ChurchDB())
            //    {
            //        var user = db.tblUsers.Find(int.Parse(id));
            //        if (user == null)
            //        {
            //            return false;
            //        }
            //        else
            //        {
            //            var rol = (from r in db.tblRoles where roleName == r.Name select r).FirstOrDefault();
            //            if (rol == null)
            //            {
            //                return false;
            //            }
            //            else
            //            {
            //                if (user.RolID == rol.RolID)
            //                {
            //                    return true;
            //                }
            //                else
            //                {
            //                    return false;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }

        public override string[] GetRolesForUser(string username)
        {
            string rol = "";
            if (HttpContext.Current.Request.Cookies["Rol"] != null)
            {

                rol = HttpContext.Current.Request.Cookies["Rol"].Value;
            }
            else
            { 
            HttpCookie cookieRol = new HttpCookie("Rol", hm.GetUserRol(username));
            DateTime expires = DateTime.Now.AddDays(1);
            cookieRol.Expires = expires;
            HttpContext.Current.Response.Cookies.Add(cookieRol);
        }

            if (rol == null)
            {
                return new string[] { };
            }
            else
            {
                return new string[] { rol };
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
    }
}
