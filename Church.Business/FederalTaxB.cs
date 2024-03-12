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
    public class FederalTaxB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString()); 
        public Response Create(tblFederalTax model)
        {
            Response response = new Response();
            try
            {
                db.tblFederalTax.Attach(model);
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
        public tblFederalTax GetLastReccord()
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblFederalTax AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy ORDER BY FederalTaxID DESC";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblFederalTax tblFederalTax = SqlConnection.Query<tblFederalTax>(Query).FirstOrDefault();
            SqlConnection.Close();
            return tblFederalTax;
        }

        public decimal GetFedetalTaxCost(int ID)
        {
            string Query = @"SELECT Cost
                                FROM tblFederalTax
                                WHERE FederalTaxID = " + ID;
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            decimal cost = SqlConnection.Query<decimal>(Query).FirstOrDefault();
            SqlConnection.Close();
            return cost;
        }
    }
}
