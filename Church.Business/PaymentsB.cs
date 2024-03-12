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
    public class PaymentsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public List<tblPayments> GetListByEstatusPendiente()
        {
            SqlConnection SqlConnection = new SqlConnection(coneccion);

            string Select = @"SELECT tblPayments.*, tblPurchaseRequestPayments.NumPayment, 
                                    tblPurchaseRequestPayments.PaymentDate, tblPurchasesRequests.Nicho, tblPurchasesRequests.Currency
                                FROM tblPayments
                                JOIN tblPurchaseRequestPayments ON tblPurchaseRequestPayments.PurchaseRequestPaymentID = tblPayments.PurchaseRequestPaymentID
                                JOIN tblPurchasesRequests ON tblPurchasesRequests.PurchasesRequestID = tblPurchaseRequestPayments.PurchasesRequestID
                                WHERE tblPayments.Estatus = 1 AND tblPayments.Active = 1";

            List<tblPayments> Payments = SqlConnection.Query<tblPayments>(Select).ToList();
            return Payments;
        }
        public int GetListByEstatusPendienteCount()
        {
            SqlConnection SqlConnection = new SqlConnection(coneccion);

            string Select = @"SELECT Count(tblPayments.PaymentID)
                                FROM tblPayments
                                WHERE tblPayments.Estatus = 1 AND tblPayments.Active = 1 ";
            int Count = SqlConnection.Query<int>(Select).FirstOrDefault();
            return Count;
        }

        public List<tblPayments> GetListByPurchaseRequestPaymentIds(List<int> lstPurchaseRequestPaymentIds)
        {
            SqlConnection SqlConnection = new SqlConnection(coneccion);

            string sQuery = @"
                SELECT Paym.*
                FROM tblPayments Paym
                WHERE Paym.Active = 1 AND Paym.IsInterest = 1 AND Paym.PurchaseRequestPaymentID IN @lstPurchaseRequestPaymentIds";

            List<tblPayments> Payments = SqlConnection.Query<tblPayments>(sQuery, new { lstPurchaseRequestPaymentIds }).ToList();
            return Payments;
        }

        public tblPayments GetById(int id)
        {
            string Query = @"
                SELECT Paym.*
	                  ,Users.NotificationKey UserNotificationKey
	                  ,PurReqPay.NumPayment
                FROM tblPayments Paym
                INNER JOIN tblPurchaseRequestPayments PurReqPay ON PurReqPay.PurchaseRequestPaymentID = Paym.PurchaseRequestPaymentID
                INNER JOIN tblPurchasesRequests PurReq ON PurReq.PurchasesRequestID = PurReqPay.PurchasesRequestID
                INNER JOIN tblCustomers Cust ON Cust.CustomerID = PurReq.CustomerID
                INNER JOIN tblUsers Users ON Users.UserID = Cust.UserID
                WHERE Paym.Active = 1 AND Paym.PaymentID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPayments Payments = SqlConnection.Query<tblPayments>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return Payments;
        }

        public Response Create(tblPayments model)
        {
            Response response = new Response();

            try
            {
                db.tblPayments.Attach(model);
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
        public Response Update(tblPayments model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblPayments.Attach(model);
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
    }
}
