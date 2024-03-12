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
    public class PurchasesRequestsQuotationB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public List<tblPurchasesRequestsQuotation> GetPurchasesRequestNotCheck()
        {
            SqlConnection SqlConnection = new SqlConnection(coneccion);

            string Select = "Select tblPurchasesRequestsQuotation.*, CONCAT(tblCustomers.Name, ' ', tblCustomers.PSurname, ' ', tblCustomers.MSurname) as Customer,tblCryptsSections.Classification from tblPurchasesRequestsQuotation ";
            string Join = @" inner join tblCryptsSections ON tblCryptsSections.CryptSectionID = tblPurchasesRequestsQuotation.CryptSectionID 
                             inner join tblCustomers ON tblCustomers.CustomerID = tblPurchasesRequestsQuotation.CustomerID 
                             where tblPurchasesRequestsQuotation.BillingChecked = 0";

            List<tblPurchasesRequestsQuotation> PurchasesRequest = SqlConnection.Query<tblPurchasesRequestsQuotation>(Select + Join).ToList();
            return PurchasesRequest;
        }
        public List<tblPurchasesRequestsQuotation> GetPurchasesRequestCheckAndNoBillingNumber()
        {
            SqlConnection SqlConnection = new SqlConnection(coneccion);

            string Select = @"Select tblPurchasesRequestsQuotation.*, CONCAT(tblCustomers.Name, ' ', tblCustomers.PSurname, ' ', tblCustomers.MSurname) as Customer,
                                tblCryptsSections.Classification,tblCustomers.RFCCURP, tblCustomers.CelPhone from tblPurchasesRequestsQuotation ";
            string Join = @" inner join tblCryptsSections ON tblCryptsSections.CryptSectionID = tblPurchasesRequestsQuotation.CryptSectionID 
                             inner join tblCustomers ON tblCustomers.CustomerID = tblPurchasesRequestsQuotation.CustomerID 
                             where tblPurchasesRequestsQuotation.BillingChecked = 1 /*and tblPurchasesRequestsQuotation.BillingNumber = 0*/";

            List<tblPurchasesRequestsQuotation> PurchasesRequest = SqlConnection.Query<tblPurchasesRequestsQuotation>(Select + Join).ToList();
            return PurchasesRequest;
        }
        public List<tblPurchasesRequests>GetListByCustomerID(int CustomerID)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, U.Active as Quotation
                FROM tblPurchasesRequestsQuotation AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                inner join tblCryptsSections AS CS ON CS.CryptSectionID = U.CryptSectionID
                WHERE U.Active = 1 AND U.CustomerID = @CustomerID";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblPurchasesRequests> PurchasesRequest = SqlConnection.Query<tblPurchasesRequests>(Query, new { CustomerID }).ToList();
            SqlConnection.Close();
            return PurchasesRequest;
        }
        public Response Create(tblPurchasesRequestsQuotation model)
        {
            Response response = new Response();

            try
            {
                db.tblPurchasesRequestsQuotation.Attach(model);
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
        public Response Update(tblPurchasesRequestsQuotation model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblPurchasesRequestsQuotation.Attach(model);
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
        public tblPurchasesRequestsQuotation GetByCustomerID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName
                FROM tblPurchasesRequestsQuotation AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.CustomerID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequestsQuotation PurchasesRequest = SqlConnection.Query<tblPurchasesRequestsQuotation>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return PurchasesRequest;
        }
        public tblPurchasesRequestsQuotation GetByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, M.Cost AS MaintenanceFee, F.Cost AS FederalTax
                FROM tblPurchasesRequestsQuotation AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                LEFT JOIN tblFederalTax AS F ON F.FederalTaxID = U.FederalTaxID
                LEFT JOIN tblMaintenanceFee AS M ON M.MaintenanceFeeID = U.MaintenanceFeeID
                WHERE U.Active = 1 AND U.PurchasesRequestQuotationID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequestsQuotation PurchasesRequest = SqlConnection.Query<tblPurchasesRequestsQuotation>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return PurchasesRequest;
        }
        public tblPurchasesRequests GetByIDToPurchasesRequests(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, M.Cost AS MaintenanceFee, F.Cost AS FederalTax, U.Active as Quotation
                FROM tblPurchasesRequestsQuotation AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                LEFT JOIN tblFederalTax AS F ON F.FederalTaxID = U.FederalTaxID
                LEFT JOIN tblMaintenanceFee AS M ON M.MaintenanceFeeID = U.MaintenanceFeeID
                WHERE U.Active = 1 AND U.PurchasesRequestQuotationID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequests PurchasesRequest = SqlConnection.Query<tblPurchasesRequests>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return PurchasesRequest;
        }

        public List<string> GetListCryptsByCryptSectionId(int CryptSectionId)
        {
            string Query = @"
                        SELECT PurReq.CryptKey
                        FROM tblPurchasesRequestsQuotation PurReq
                        WHERE PurReq.Active = 1 AND PurReq.CryptSectionID = @CryptSectionId";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            var model = SqlConnection.Query<string>(Query, new { CryptSectionId }).ToList();
            SqlConnection.Close();
            return model;
        }
    }
}
