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
    public class PurchasesRequestsAshDepositsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public Response Create(tblPurchasesRequestsAshDeposits model)
        {
            Response response = new Response();

            try
            {
                db.tblPurchasesRequestsAshDeposits.Attach(model);
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
        public Response Update(tblPurchasesRequestsAshDeposits model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblPurchasesRequestsAshDeposits.Attach(model);
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
        public tblPurchasesRequestsAshDeposits GetByAshDepositID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName
                FROM tblPurchasesRequestsAshDeposits AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.AshDepositID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequestsAshDeposits model = SqlConnection.Query<tblPurchasesRequestsAshDeposits>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return model;
        }
        public tblPurchasesRequestsAshDeposits GetByCustomerID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName
                FROM tblPurchasesRequestsAshDeposits AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.CustomerID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequestsAshDeposits model = SqlConnection.Query<tblPurchasesRequestsAshDeposits>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return model;
        }
        public tblPurchasesRequestsAshDeposits GetByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, M.Cost AS MaintenanceFee, F.Cost AS FederalTax
                FROM tblPurchasesRequests AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.PurchasesRequestID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequestsAshDeposits model = SqlConnection.Query<tblPurchasesRequestsAshDeposits>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return model;
        }
    }
}
