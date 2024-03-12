using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Church.Data;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using static Church.Business.QueryHelper;
using Dapper;
using Church.Docs;

namespace Church.Business
{
    public class UsersB : DbContext
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        [MethodDescription(DescriptionES.INDEX_OF_NOF)]
        public StaticPagedList<dtoUsers> GetUsers(string sUsername, string sName, string sPSurname, string sRolId, DateTime Date, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);
            int RolId = string.IsNullOrEmpty(sRolId) ? 0 : int.Parse(sRolId);

            string Sort = QueryBuilder.GetSorting("tblUsers.UserName", "ASC", sSortingOrder);
            string Select = db.tblUsers.SelectColumns(x => x.UserName, x => x.Name, x => x.PSurname, x => x.CreatedDate, x => x.UserID, x => x.tblRoles.Name);
            string Join = db.tblUsers.GetJoin(x => x.RolID, JoinType.JOIN, typeof(tblRoles).Name);

            List<object> Values = new List<object>() { sUsername, sName, sPSurname, RolId,Date };
            string Conditions = db.tblUsers.GetCondition(Values, x => x.UserName, x => x.Name, x => x.PSurname, X => X.RolID, x => x.CreatedDate);

            var query = QueryBuilder.GetQuery(typeof(tblUsers).Name, Select, Join, Conditions, Sort, iPage, PageSize);
            List<dtoUsers> lst = sqlConnection.Query<dtoUsers>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoUsers>(lst, iPage, PageSize, totalCount);
        }

        [MethodDescription(DescriptionES.GET_DATA_BY_ID, DescriptionES.USER)]
        public dtoUsers GetUserByUserName(string UserName)
        {
            dtoUsers value = new dtoUsers(); 
            SqlConnection sqlConnection = new SqlConnection(coneccion);
            value = sqlConnection.Query<dtoUsers>("GetUserbyUserName", new { UserName = UserName },commandType: CommandType.StoredProcedure).SingleOrDefault();
            sqlConnection.Close();
            return value;
        }
        public List<tblUsers> GetList()
        {
            string Query = @"
                SELECT *
                FROM tblUsers";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblUsers> Users = SqlConnection.Query<tblUsers>(Query).ToList();
            SqlConnection.Close();
            return Users;
        }
        public tblUsers GetUserByEmail(string Email)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName
                FROM tblUsers AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.Username = @Email";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblUsers tblUsers = SqlConnection.Query<tblUsers>(Query, new { Email }).FirstOrDefault();
            SqlConnection.Close();
            return tblUsers;
        }
        [MethodDescription(DescriptionES.POST_CREATE)]
        public Response Create(tblUsers model)
        {
            Response response = new Response();

            try
            {
                db.tblUsers.Attach(model);
                db.Entry(model).State = EntityState.Added;
                db.SaveChanges();
                response.Result = Result.OK;
            }
            catch (Exception ex)
            {
                response.Data = ex.Message;
                response.error = ex.Message + " Inner exception: " + ex.InnerException.InnerException.Message;
                response.Result = Result.EXCEPTION;
            }
            return response;
        }

        public Response Update(tblUsers model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblUsers.Attach(model);
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                response.Result = Result.OK;
            }
            catch (Exception ex)
            {
                response.Data = ex.Message;
                response.Result = Result.EXCEPTION;
            }
            return response;
        }

        public tblUsers GetByUsername_RolId(string UserName, int RolId)
        {
            string sQuery = @"
                SELECT Users.*
                FROM tblUsers Users
                WHERE Users.Active = 1 AND Users.UserName = @UserName AND Users.RolID = @RolId";
            using(IDbConnection dbConnection = new SqlConnection(coneccion))
            {
                var model = dbConnection.QueryFirstOrDefault<tblUsers>(sQuery, new { UserName, RolId });
                return model;
            }
        }
        public tblUsers GetUserByID(int UserID)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);
            tblUsers users;
            using (var multi = sqlConnection.QueryMultiple("GetUserbyID",  new { UserID = UserID }, commandType: CommandType.StoredProcedure))
            {
                users = multi.Read<tblUsers>().FirstOrDefault();
            }
            sqlConnection.Close();
            return users;
        }
    }
}
