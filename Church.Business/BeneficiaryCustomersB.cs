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
    public class BeneficiaryCustomersB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public Response Create(tblBeneficiaryCustomers model)
        {
            Response response = new Response();

            try
            {
                db.tblBeneficiaryCustomers.Attach(model);
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
        public Response Update(tblBeneficiaryCustomers model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblBeneficiaryCustomers.Attach(model);
                db.Entry(model).State = EntityState.Modified;
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
        public tblBeneficiaryCustomers GetByBeneficiaryCustomerID(int BeneficiaryCustomerID)
        {
            string Query = @"
                SELECT U.*
                FROM tblBeneficiaryCustomers AS U
                WHERE U.Active = 1 AND U.BeneficiaryCustomerID = @BeneficiaryCustomerID";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblBeneficiaryCustomers BeneficiaryCustomers = SqlConnection.Query<tblBeneficiaryCustomers>(Query, new { BeneficiaryCustomerID }).FirstOrDefault();
            SqlConnection.Close();
            return BeneficiaryCustomers;
        }
        public List<tblBeneficiaryCustomers> GetListByPurchasesRequestID(int PurchasesRequestID)
        {
            string Query = @"
                SELECT U.*
                FROM tblBeneficiaryCustomers AS U
                WHERE U.Active = 1 AND U.PurchasesRequestID = @PurchasesRequestID";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblBeneficiaryCustomers> BeneficiaryCustomers = SqlConnection.Query<tblBeneficiaryCustomers>(Query, new { PurchasesRequestID }).ToList();
            SqlConnection.Close();
            return BeneficiaryCustomers;
        }
        public List<tblBeneficiaryCustomers> GetListByCustomerID(int CustomerID)
        {
            string Query = @"
                SELECT U.*
                FROM tblBeneficiaryCustomers AS U
                WHERE U.Active = 1 AND U.CustomerID = @CustomerID";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblBeneficiaryCustomers> BeneficiaryCustomers = SqlConnection.Query<tblBeneficiaryCustomers>(Query, new { CustomerID }).ToList();
            SqlConnection.Close();
            return BeneficiaryCustomers;
        }
    }
}
