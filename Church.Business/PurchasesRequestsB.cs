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
using Church.Business.SQLHandlers;

namespace Church.Business
{
    public class PurchasesRequestsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private HelpMethods HelpMethods = new HelpMethods();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public List<tblPurchasesRequests> GetPurchasesRequestNotCheck()
        {
            SqlConnection SqlConnection = new SqlConnection(coneccion);

            string Select = "Select tblPurchasesRequests.*, CONCAT(tblCustomers.Name, ' ', tblCustomers.PSurname, ' ', tblCustomers.MSurname) as Customer,tblCryptsSections.Classification from tblPurchasesRequests ";
            string Join = @" inner join tblCryptsSections ON tblCryptsSections.CryptSectionID = tblPurchasesRequests.CryptSectionID 
                             inner join tblCustomers ON tblCustomers.CustomerID = tblPurchasesRequests.CustomerID 
                             where tblPurchasesRequests.BillingChecked = 0";

            List<tblPurchasesRequests> PurchasesRequest = SqlConnection.Query<tblPurchasesRequests>(Select + Join).ToList();
            return PurchasesRequest;
        }
        public int GetPurchasesRequestNotCheckCount()
        {
            SqlConnection SqlConnection = new SqlConnection(coneccion);

            string Select = "Select Count(PurchasesRequestID) from tblPurchasesRequests where tblPurchasesRequests.BillingChecked = 0";

            int Count = SqlConnection.Query<int>(Select).FirstOrDefault();
            return Count;
        }
        public List<tblPurchasesRequests> GetPurchasesRequestCheckAndNoBillingNumber()
        {
            SqlConnection SqlConnection = new SqlConnection(coneccion);

            string Select = @"Select tblPurchasesRequests.*, CONCAT(tblCustomers.Name, ' ', tblCustomers.PSurname, ' ', tblCustomers.MSurname) as Customer,
                                tblCryptsSections.Classification,tblCustomers.RFCCURP, tblCustomers.CelPhone from tblPurchasesRequests ";
            string Join = @" inner join tblCryptsSections ON tblCryptsSections.CryptSectionID = tblPurchasesRequests.CryptSectionID 
                             inner join tblCustomers ON tblCustomers.CustomerID = tblPurchasesRequests.CustomerID 
                             where tblPurchasesRequests.BillingChecked = 1 /*and tblPurchasesRequests.BillingNumber = 0*/";

            List<tblPurchasesRequests> PurchasesRequest = SqlConnection.Query<tblPurchasesRequests>(Select + Join).ToList();
            return PurchasesRequest;
        }
        public List<tblPurchasesRequests> GetListByCustomerID(int CustomerID)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName
                FROM tblPurchasesRequests AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                inner join tblCryptsSections AS CS ON CS.CryptSectionID = U.CryptSectionID
                WHERE U.Active = 1 AND U.CustomerID = @CustomerID";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblPurchasesRequests> PurchasesRequest = SqlConnection.Query<tblPurchasesRequests>(Query, new { CustomerID }).ToList();
            SqlConnection.Close();
            return PurchasesRequest;
        }
        public Response Create(tblPurchasesRequests model)
        {
            Response response = new Response();

            try
            {
                db.tblPurchasesRequests.Attach(model);
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
        public Response Update(tblPurchasesRequests model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblPurchasesRequests.Attach(model);
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
        public int GetPurchaseRequestID()
        {
            string Query = @"SELECT PurchasesRequestID
                            FROM tblPurchasesRequests
                            WHERE PurchasesRequestID >= 5733
                            ORDER BY PurchasesRequestID DESC";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            int model = SqlConnection.Query<int>(Query).FirstOrDefault();
            SqlConnection.Close();
            return model;
        }
        public tblPurchasesRequests GetByCustomerID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName
                FROM tblPurchasesRequests AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.CustomerID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequests PurchasesRequest = SqlConnection.Query<tblPurchasesRequests>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return PurchasesRequest;
        }
        public tblPurchasesRequests GetByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, M.Cost AS MaintenanceFee, F.Cost AS FederalTax
                FROM tblPurchasesRequests AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                LEFT JOIN tblFederalTax AS F ON F.FederalTaxID = U.FederalTaxID
                LEFT JOIN tblMaintenanceFee AS M ON M.MaintenanceFeeID = U.MaintenanceFeeID
                WHERE U.Active = 1 AND U.PurchasesRequestID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequests PurchasesRequest = SqlConnection.Query<tblPurchasesRequests>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return PurchasesRequest;
        }

        public dtoPurchasesRequest GetByIDForContract(int id)
        {
            string Query = @"SELECT PR.PurchasesRequestID, PR.ContractID, PR.CreatedDate, PR.Nicho, 
                            CONCAT(C.Name, ' ', C.PSurname, ' ', CASE WHEN C.MSurname IS NOT NULL AND C.MSurname != '' THEN C.MSurname ELSE '' END) AS CustomerName, C.Address, C.AddressNumber, C.AdressInteriorNumber,
                            C.Neighborhood, C.ZipCode, PR.OriginalPrice, PR.CryptPrice, PR.Mensualidades, C.Phone, C.CelPhone, T.Name AS Town, S.Name AS State, C.CityOfBirth, C.DateOfBirth, CS.Name AS CivilStatus,
                            C.Occupation, PR.CheckMaintenanceFee, PR.CheckFederalTax
                            FROM tblPurchasesRequests PR
                            LEFT JOIN tblCustomers C ON C.CustomerID = PR.CustomerID
                            LEFT JOIN tblTowns T ON T.TownID = C.TownID
                            LEFT JOIN tblStates S ON S.StateID = T.StateID
                            LEFT JOIN tblCivilStatus CS ON CS.CivilStatusID = C.CivilStatusID
                            WHERE PurchasesRequestID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            dtoPurchasesRequest PurchasesRequest = SqlConnection.Query<dtoPurchasesRequest>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return PurchasesRequest;
        }

        public List<string> GetListCryptsByCryptSectionId(int CryptSectionId)
        {
            string Query = @"
                        SELECT PurReq.CryptKey
                        FROM tblPurchasesRequests PurReq
                        WHERE PurReq.Active = 1 AND PurReq.CryptSectionID = @CryptSectionId";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            var model = SqlConnection.Query<string>(Query, new { CryptSectionId }).ToList();
            SqlConnection.Close();
            return model;
        }

        public bool GetByCryptKeyAndSectionID(string CryptKey, int CryptSectionID, int CryptType)
        {
            string Query = @"
                SELECT COUNT(*)
                FROM tblPurchasesRequests
                WHERE Active = 1 AND CryptKey = @CryptKey AND CryptSectionID = @CryptSectionID AND CryptType = @CryptType";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblPurchasesRequests PurchasesRequest = SqlConnection.Query<tblPurchasesRequests>(Query, new { CryptKey = CryptKey, CryptSectionID = CryptSectionID, CryptType = CryptType }).FirstOrDefault();
            SqlConnection.Close();
            return PurchasesRequest != null ? true : false;
        }

        public List<tblPurchasesRequests> Api_GetByUserId(int UserId, int Filter, int PurReqID = 0)
        {
            string sQuery = @"
                SELECT PurReq.PurchasesRequestID
                        ,PurReq.Nicho
	                    ,PurReq.Level
	                    ,CrySec.Classification
	                    ,CryCla.Name
	                    ,PurReq.PaymentMethod
	                    ,PurReq.CryptPrice
	                    ,(
		                SELECT _PurReqPay.*
			                    ,CASE
				                    WHEN CAST(_PurReqPay.PaymentDate AS DATE) < CAST(@CurrentDate AS DATE) THEN 'true'
				                    ELSE 'false'
			                    END OverduePayment
				                ,CASE
					                  WHEN (_Paym.CreatedDate IS NULL OR _Paym.CreatedDate > _PurReqPay.PaymentDate) AND CAST(_PurReqPay.PaymentDate AS DATE) < CAST(@CurrentDate AS DATE)   THEN 1
					                  ELSE 0
				                END HasInterest
				                ,CASE
					                  WHEN _Paym.PaymentID IS NOT NULL THEN 1
					                  ELSE 0
				                END HasPaymentDone
                               ,_Paym.ReceiptUrl
		                FROM tblPurchaseRequestPayments _PurReqPay
		                LEFT JOIN tblPayments _Paym ON _Paym.PurchaseRequestPaymentID = _PurReqPay.PurchaseRequestPaymentID AND _Paym.IsInterest = 0 AND _Paym.Estatus <> 3 
		                WHERE _PurReqPay.Active = 1 AND _PurReqPay.PurchasesRequestID = PurReq.PurchasesRequestID
			                AND (@Filter = 0 
				                OR (@Filter = 1 AND _PurReqPay.Estatus = 3)
				                OR (@Filter = 2 AND _PurReqPay.Estatus != 3))
		                ORDER BY _PurReqPay.PaymentDate ASC
		                FOR JSON PATH
	                    ) lstPayments
		                ,(
			                SELECT CONCAT(AshDep.Name, ' ', AshDep.PSurname, ' ', AshDep.MSurname) FullName
				                    ,AshDep.DeathDate
			                FROM tblAshDeposits AshDep
			                WHERE AshDep.Active = 1 AND AshDep.PurchasesRequestID = PurReq.PurchasesRequestID
			                FOR JSON PATH
		                ) lstAshDeposits, Currency
                FROM tblPurchasesRequests PurReq
                INNER JOIN tblCustomers Cust ON Cust.CustomerID = PurReq.CustomerID AND Cust.Active = 1
                INNER JOIN tblCryptsSections CrySec ON CrySec.CryptSectionID = PurReq.CryptSectionID
                INNER JOIN tblCryptClassifications CryCla ON CryCla.CryptClassificationID = CrySec.CryptClassificationID
                WHERE PurReq.Active = 1 AND Cust.UserID = @UserId";

            if (PurReqID != 0)
            {
                sQuery += " AND PurReq.PurchasesRequestID = " + PurReqID;
            }

            SqlMapper.AddTypeHandler(new PurchaseRequestPaymentsHandler());
            SqlMapper.AddTypeHandler(new AshDepositsHandler());
            using (IDbConnection dbConnection = new SqlConnection(coneccion))
            {
                var model = dbConnection.Query<tblPurchasesRequests>(sQuery, new { UserId, CurrentDate = HelpMethods.GetDateTime(), Filter }).ToList();
                SqlMapper.ResetTypeHandlers();
                return model;
            }
        }

        public List<tblPurchasesRequests> GetList_ByCustomerID(int CustomerID)
        {
            string Query = @"
            SELECT 
            PurReq.PurchasesRequestID,
            PurReq.Nicho
            FROM tblPurchasesRequests PurReq
            WHERE PurReq.Active = 1 AND PurReq.CustomerID = @CustomerID";

            using (IDbConnection dbConnection = new SqlConnection(coneccion))
            {
                List<tblPurchasesRequests> list = dbConnection.Query<tblPurchasesRequests>(Query, new { CustomerID }).ToList();

                return list;
            }
        }

        public tblPurchasesRequests Get_ByPurchasesRequestID(int PurchasesRequestID, int Filter)
        {
            string sQuery = @"
            SELECT PurReq.Nicho
	        ,PurReq.Level
	        ,CrySec.Classification
	        ,CryCla.Name
	        ,PurReq.PaymentMethod
	        ,PurReq.CryptPrice
	        ,(
		    SELECT _PurReqPay.*
			,CASE
				WHEN CAST(_PurReqPay.PaymentDate AS DATE) < CAST(@CurrentDate AS DATE) THEN 'true'
				ELSE 'false'
			END OverduePayment
			,CASE
				WHEN (_Paym.CreatedDate IS NULL OR _Paym.CreatedDate > _PurReqPay.PaymentDate) AND CAST(_PurReqPay.PaymentDate AS DATE) < CAST(@CurrentDate AS DATE)   THEN 1
				ELSE 0
			END HasInterest
			,CASE
				WHEN _Paym.PaymentID IS NOT NULL THEN 1
				ELSE 0
			END HasPaymentDone
            ,_Paym.ReceiptUrl
		    FROM tblPurchaseRequestPayments _PurReqPay
		    LEFT JOIN tblPayments _Paym ON _Paym.PurchaseRequestPaymentID = _PurReqPay.PurchaseRequestPaymentID AND _Paym.IsInterest = 0 AND _Paym.Estatus <> 3
		    WHERE _PurReqPay.Active = 1 AND _PurReqPay.PurchasesRequestID = PurReq.PurchasesRequestID
			AND (@Filter = 0 
				OR (@Filter = 1 AND _PurReqPay.Estatus = 3)
				OR (@Filter = 2 AND _PurReqPay.Estatus != 3))
		    ORDER BY _PurReqPay.PaymentDate ASC
		    FOR JSON PATH
	        ) lstPayments
		    ,(
			SELECT CONCAT(AshDep.Name, ' ', AshDep.PSurname, ' ', AshDep.MSurname) FullName
				    ,AshDep.DeathDate
			FROM tblAshDeposits AshDep
			WHERE AshDep.Active = 1 AND AshDep.PurchasesRequestID = PurReq.PurchasesRequestID
			FOR JSON PATH
		    ) lstAshDeposits, 
            (Cust.[Name] + ' ' + Cust.[PSurname] + (CASE WHEN Cust.MSurname IS NOT NULL AND Cust.MSurname <> '' THEN ' ' + 
            Cust.MSurname ELSE '' END)) AS Customer, PurReq.Currency
            FROM tblPurchasesRequests PurReq
            INNER JOIN tblCustomers Cust ON Cust.CustomerID = PurReq.CustomerID AND Cust.Active = 1
            INNER JOIN tblCryptsSections CrySec ON CrySec.CryptSectionID = PurReq.CryptSectionID
            INNER JOIN tblCryptClassifications CryCla ON CryCla.CryptClassificationID = CrySec.CryptClassificationID
            WHERE PurReq.Active = 1 AND PurReq.PurchasesRequestID = @PurchasesRequestID";

            SqlMapper.AddTypeHandler(new PurchaseRequestPaymentsHandler());

            SqlMapper.AddTypeHandler(new AshDepositsHandler());

            using (IDbConnection dbConnection = new SqlConnection(coneccion))
            {
                var model = dbConnection.QueryFirstOrDefault<tblPurchasesRequests>(sQuery, new
                {
                    PurchasesRequestID =
                            PurchasesRequestID,
                    CurrentDate = HelpMethods.GetDateTime(),
                    Filter
                });

                SqlMapper.ResetTypeHandlers();

                return model;
            }
        }
    }
}