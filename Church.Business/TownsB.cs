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
    public class TownsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public StaticPagedList<dtoTowns> GetTowns(string sName, DateTime Date, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblTowns.Name", "ASC", sSortingOrder);
            string Select = db.tblTowns.SelectColumns(x => x.Name, x => x.CreatedDate, x => x.TownID) + ", tblStates.Name as State ";
            string Join = db.tblTowns.GetJoin(x => x.StateID, JoinType.JOIN, typeof(tblStates).Name);

            List<object> Values = new List<object>() { sName, Date };
            string Conditions = db.tblTowns.GetCondition(Values, x => x.Name, x => x.CreatedDate);

            var query = QueryBuilder.GetQuery(typeof(tblTowns).Name, Select, Join, Conditions, Sort, iPage, PageSize);
            List<dtoTowns> lst = sqlConnection.Query<dtoTowns>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoTowns>(lst, iPage, PageSize, totalCount);
        }
        public Response Create(tblTowns model)
        {
            Response response = new Response();

            try
            {
                db.tblTowns.Attach(model);
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
        public Response Update(tblTowns model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblTowns.Attach(model);
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
        public List<tblTowns> GetListByStateID(int StateID)
        {
            string Query = @"
                SELECT U.*
                FROM tblTowns AS U
                WHERE U.Active = 1 and U.StateID = @StateID";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblTowns> Towns = SqlConnection.Query<tblTowns>(Query, new { StateID }).ToList();
            SqlConnection.Close();
            return Towns;
        }
        public List<tblTowns> GetList()
        {
            string Query = @"
                SELECT U.*
                FROM tblTowns AS U
                WHERE U.Active = 1 ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblTowns> Towns = SqlConnection.Query<tblTowns>(Query).ToList();
            SqlConnection.Close();
            return Towns;
        }
        public tblTowns GetTownsByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, S.Name AS State 
                FROM tblTowns AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                INNER JOIN tblStates AS S ON S.StateID = U.StateID
                WHERE U.Active = 1 AND U.TownID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblTowns Towns = SqlConnection.Query<tblTowns>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return Towns;
        }
    }
}
