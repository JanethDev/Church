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
    public class StatesB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public StaticPagedList<dtoStates> GetStates(string sName, DateTime Date, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblStates.Name", "ASC", sSortingOrder);
            string Select = db.tblStates.SelectColumns(x => x.Name, x => x.CreatedDate, x => x.StateID);

            List<object> Values = new List<object>() { sName, Date };
            string Conditions = db.tblStates.GetCondition(Values, x => x.Name, x => x.CreatedDate);

            var query = QueryBuilder.GetQuery(typeof(tblStates).Name, Select, "", Conditions, Sort, iPage, PageSize);
            List<dtoStates> lst = sqlConnection.Query<dtoStates>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoStates>(lst, iPage, PageSize, totalCount);
        }
        public Response Create(tblStates model)
        {
            Response response = new Response();

            try
            {
                db.tblStates.Attach(model);
                db.Entry(model).State = EntityState.Added;
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
        public Response Update(tblStates model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblStates.Attach(model);
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
        public List<tblStates> GetList()
        {
            string Query = @"
                SELECT U.*
                FROM tblStates AS U
                WHERE U.Active = 1 ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblStates> States = SqlConnection.Query<tblStates>(Query).ToList();
            SqlConnection.Close();
            return States;
        }
        public tblStates GetStatesByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblStates AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.StateID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblStates States = SqlConnection.Query<tblStates>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return States;
        }
    }
}
