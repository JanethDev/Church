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
    public class PurchaseRequestPaymentsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public tblPurchaseRequestPayments GetById(int Id)
        {
            string Query = @"
                SELECT PurReqPay.*
                FROM tblPurchaseRequestPayments PurReqPay
                WHERE PurReqPay.Active = 1 AND PurReqPay.PurchaseRequestPaymentID = @Id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            var model = SqlConnection.Query<tblPurchaseRequestPayments>(Query, new { Id }).FirstOrDefault();
            SqlConnection.Close();

            return model;
        }

        public List<AuxCustomerNotifications> GetListForNotifications(DateTime CurrentDate)
        {
            string Query = @"
                SELECT DATEDIFF(DAY, CAST(@CurrentDate AS DATE), CAST(PurReqPay.PaymentDate AS DATE)) RemainingDaysToPay
	                  ,Users.NotificationKey UserNotificationKey
                FROM tblPurchaseRequestPayments PurReqPay
                INNER JOIN tblPurchasesRequests PurReq ON PurReq.PurchasesRequestID = PurReqPay.PurchasesRequestID
                INNER JOIN tblCustomers Cust ON Cust.CustomerID = PurReq.CustomerID
                INNER JOIN tblUsers Users ON Users.UserID = Cust.UserID
                WHERE PurReqPay.Active = 1 AND PurReqPay.Estatus != 3 AND Users.NotificationKey IS NOT NULL AND Users.NotificationKey != ''
	                AND CAST(PurReqPay.PaymentDate AS DATE) >= CAST(@CurrentDate AS DATE)
	                AND DATEDIFF(DAY, CAST(@CurrentDate AS DATE), CAST(PurReqPay.PaymentDate AS DATE)) <= 7";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            var model = SqlConnection.Query<AuxCustomerNotifications>(Query, new { CurrentDate }).ToList();
            SqlConnection.Close();

            return model;
        }
        public Response Create(tblPurchaseRequestPayments model)
        {
            Response response = new Response();

            try
            {
                db.tblPurchaseRequestPayments.Attach(model);
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

        public Response Update(tblPurchaseRequestPayments model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                db = new ChurchDB();
                db.tblPurchaseRequestPayments.Attach(model);
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