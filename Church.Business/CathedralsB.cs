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
    public class CathedralsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public StaticPagedList<dtoCathedrals> GetCathedrals(string sName, DateTime Date, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblCathedrals.Name", "ASC", sSortingOrder);
            string Select = db.tblCathedrals.SelectColumns(x => x.Name, x => x.CreatedDate, x=>x.CathedralID);

            List<object> Values = new List<object>() { sName, Date, 1 };
            string Conditions = db.tblCathedrals.GetCondition(Values, x => x.Name, x => x.CreatedDate, x => x.Active);

            var query = QueryBuilder.GetQuery(typeof(tblCathedrals).Name, Select, "", Conditions, Sort, iPage, PageSize);
            List<dtoCathedrals> lst = sqlConnection.Query<dtoCathedrals>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoCathedrals>(lst, iPage, PageSize, totalCount);
        }
        public Response Create(tblCathedrals model)
        {
            Response response = new Response();

            try
            {
                db.tblCathedrals.Attach(model);
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
        public Response Update(tblCathedrals model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblCathedrals.Attach(model);
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
        public List<tblCathedrals> GetListByCityID(int CityID = 0)
        {
            string Query = @"
                SELECT *
                FROM tblCathedrals
                WHERE Active = 1 
                    AND (@CityID = 0 OR CityID = @CityID)";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblCathedrals> Cathedrals = SqlConnection.Query<tblCathedrals>(Query, new { CityID }).ToList();
            SqlConnection.Close();
            return Cathedrals;
        }
        public tblCathedrals GetCathedralsByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblCathedrals AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.CathedralID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblCathedrals Cathedrals = SqlConnection.Query<tblCathedrals>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return Cathedrals;
        }
    }
}
