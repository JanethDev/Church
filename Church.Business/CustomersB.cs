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
    public class CustomersB
    {

        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public struct sCustomers
        {
            public string label { get; set; }
            public string value { get; set; }
        }
        public StaticPagedList<dtoCustomers> GetCustomers(string sName, string sPSurname, string sEmail, string sCelPhone, DateTime Date, bool Prospective, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblCustomers.Name", "ASC", sSortingOrder);
            string Select = db.tblCustomers.SelectColumns(x => x.Name, x => x.PSurname, x => x.Email, x => x.CelPhone, x => x.CreatedDate, x => x.CustomerID) + ", (select COUNT( *) from tblPurchasesRequestsQuotation where tblPurchasesRequestsQuotation.CustomerID = tblCustomers.CustomerID) NumberQuotation";

            List<object> Values = new List<object>() { sName, sPSurname, sEmail, sCelPhone, Prospective, Date };
            string Conditions = db.tblCustomers.GetCondition(Values, x => x.Name, x => x.PSurname, x => x.Email, x => x.CelPhone, x => x.Prospective, x => x.CreatedDate);

            var query = QueryBuilder.GetQuery(typeof(tblCustomers).Name, Select, "", Conditions, Sort, iPage, PageSize);
            List<dtoCustomers> lst = sqlConnection.Query<dtoCustomers>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoCustomers>(lst, iPage, PageSize, totalCount);
        }
        public object SearchByName(string term)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            var vQuery = @"SELECT CONCAT ( tblCustomers.PSurname, ' ', tblCustomers.MSurname, ' ', tblCustomers.Name ) as value, CustomerID as label, tblCustomers.*, format(tblCustomers.DateOfBirth,'dd-MM-yyyy') as sDateOfBirth 
                         ,tblStates.StateID, S.StateID as StateAddressCompanyID  FROM tblCustomers 
                         left join tblTowns on tblTowns.TownID = tblCustomers.TownID
                         left join tblStates on tblStates.StateID = tblTowns.StateID 
                         left join tblTowns T on T.TownID = tblCustomers.TownAddressCompanyID
                         left join tblStates S on S.StateID = tblTowns.StateID";
            vQuery += " WHERE CONCAT ( tblCustomers.PSurname, ' ', tblCustomers.MSurname, ' ', tblCustomers.Name ) LIKE '%" + term + "%' AND tblCustomers.Active = 1 ";
            List<tblCustomers> lstCustomers = sqlConnection.Query<tblCustomers>(vQuery).ToList();
            sqlConnection.Close();
            return lstCustomers;
        }
        public Response Create(tblCustomers model)
        {
            Response response = new Response();

            try
            {
                db.tblCustomers.Attach(model);
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
        public Response Update(tblCustomers model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblCustomers.Attach(model);
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
        public tblCustomers GetCustomersByEmail(string Email)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, S.StateID, S.Name as State, T.Name as Town
                , S2.Name as StateAddressCompany, T2.Name as TownAddressCompany, C.Name as CivilStatus
                FROM tblCustomers AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                LEFT JOIN tblTowns AS T ON T.TownID = U.TownID
                LEFT JOIN tblStates AS S ON S.StateID = T.StateID
                LEFT JOIN tblTowns AS T2 ON T2.TownID = U.TownAddressCompanyID
                LEFT JOIN tblStates AS S2 ON S2.StateID = T2.StateID
                LEFT JOIN tblCivilStatus AS C ON C.CivilStatusID = U.CivilStatusID
                WHERE U.Active = 1 AND U.Email = @Email";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblCustomers Customers = SqlConnection.Query<tblCustomers>(Query, new { Email }).FirstOrDefault();
            SqlConnection.Close();
            return Customers;
        }
        public tblCustomers GetCustomersByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, S.StateID, S.Name as State, T.Name as Town
                , S2.Name as StateAddressCompany, T2.Name as TownAddressCompany, C.Name as CivilStatus
                FROM tblCustomers AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                LEFT JOIN tblTowns AS T ON T.TownID = U.TownID
                LEFT JOIN tblStates AS S ON S.StateID = T.StateID
                LEFT JOIN tblTowns AS T2 ON T2.TownID = U.TownAddressCompanyID
                LEFT JOIN tblStates AS S2 ON S2.StateID = T2.StateID
                LEFT JOIN tblCivilStatus AS C ON C.CivilStatusID = U.CivilStatusID
                WHERE U.Active = 1 AND U.CustomerID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblCustomers Customers = SqlConnection.Query<tblCustomers>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return Customers;
        }

        public int GetCustomerID()
        {
            string Query = @"SELECT CustomerID
                            FROM tblCustomers
                            WHERE CustomerID >= 35270
                            ORDER BY CustomerID DESC";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            int model = SqlConnection.Query<int>(Query).FirstOrDefault();
            SqlConnection.Close();
            return model;
        }
    }
}
