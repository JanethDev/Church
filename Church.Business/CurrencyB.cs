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
    public class CurrencyB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public Response Create(tblCurrency model)
        {
            Response response = new Response();
            try
            {
                db.tblCurrency.Attach(model);
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
        public List<tblCurrency> GetList()
        {
            string Query = @"
                            SELECT U.*
                            FROM tblCurrency AS U ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblCurrency> tblCurrency = SqlConnection.Query<tblCurrency>(Query).ToList();
            SqlConnection.Close();
            return tblCurrency;
        }
        public tblCurrency GetByName(string Name)
        {
            string Query = "SELECT * FROM tblCurrency WHERE Name = @Name ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblCurrency tblCurrency = SqlConnection.Query<tblCurrency>(Query, new { Name }).FirstOrDefault();
            SqlConnection.Close();
            return tblCurrency;
        }
    }
}
