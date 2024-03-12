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
    public class CryptsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public StaticPagedList<dtoCrypts> GetCrypts(string sName, string sPrice, DateTime Date, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblCrypts.Name", "ASC", sSortingOrder);
            string Select = db.tblCrypts.SelectColumns(x => x.Name, x => x.Price, x => x.CreatedDate, x => x.CryptID);

            List<object> Values = new List<object>() { sName, sPrice, Date };
            string Conditions = db.tblCrypts.GetCondition(Values, x => x.Name, x => x.Price, x => x.CreatedDate);

            var query = QueryBuilder.GetQuery(typeof(tblCrypts).Name, Select, "", Conditions, Sort, iPage, PageSize);
            List<dtoCrypts> lst = sqlConnection.Query<dtoCrypts>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoCrypts>(lst, iPage, PageSize, totalCount);
        }
        public Response Create(tblCrypts model)
        {
            Response response = new Response();

            try
            {
                db.tblCrypts.Attach(model);
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
        public Response Update(tblCrypts model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblCrypts.Attach(model);
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
        public tblCrypts GetCryptsByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblCrypts AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.CryptID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblCrypts Crypts = SqlConnection.Query<tblCrypts>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return Crypts;
        }
    }
}