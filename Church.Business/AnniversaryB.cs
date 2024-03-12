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
    public class AnniversaryB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public StaticPagedList<dtoAnniversary> GetAnniversary(string sName, DateTime Date, string Hour, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblAnniversary.Name", "ASC", sSortingOrder);
            string Select = db.tblAnniversary.SelectColumns(x => x.Name, x => x.Date, x => x.Hour, x => x.AnniversaryID);

            List<object> Values = new List<object>() { sName, Date, Hour };
            string Conditions = db.tblAnniversary.GetCondition(Values, x => x.Name, x => x.Date, x => x.Hour);

            var query = QueryBuilder.GetQuery(typeof(tblAnniversary).Name, Select, "", Conditions, Sort, iPage, PageSize);
            List<dtoAnniversary> lst = sqlConnection.Query<dtoAnniversary>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoAnniversary>(lst, iPage, PageSize, totalCount);
        }
        public List<tblAnniversary> GetListByYearAndMonth(int Year, int Month)
        {
            string Query = @"SELECT MONTH(Date) Month,YEAR(Date) Year, DAY(Date) Day  
                                FROM tblAnniversary WHERE MONTH(Date) = @Month AND YEAR(Date) = @Year
                                GROUP BY MONTH(Date),YEAR(Date), DAY(Date) ";

            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblAnniversary> List = SqlConnection.Query<tblAnniversary>(Query, new { Year, Month }).ToList();
            SqlConnection.Close();
            return List;
        }
        public Response Create(tblAnniversary model)
        {
            Response response = new Response();

            try
            {
                db.tblAnniversary.Attach(model);
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
        public Response Update(tblAnniversary model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblAnniversary.Attach(model);
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
        public tblAnniversary GetAnniversaryByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblAnniversary AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.AnniversaryID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblAnniversary Anniversary = SqlConnection.Query<tblAnniversary>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return Anniversary;
        }
    }
}
