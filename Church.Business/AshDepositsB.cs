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
    public class AshDepositsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public List<tblAshDeposits> GetListByPurchasesRequestID(int PurchasesRequestID)
        {
            string Query = @"select tblAshDeposits.*,
                            Concat(tblAshDeposits.Name, ' ', tblAshDeposits.PSurname, ' ', tblAshDeposits.MSurname) as Name, tblFederalTax.Cost as FederalTax
                            FROM tblAshDeposits
                            LEFT JOIN tblFederalTax ON tblFederalTax.FederalTaxID = tblAshDeposits.FederalTaxID
                            WHERE tblAshDeposits.PurchasesRequestID = @PurchasesRequestID AND tblAshDeposits.Active = 1 ";

            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List <tblAshDeposits> List = SqlConnection.Query<tblAshDeposits>(Query, new { PurchasesRequestID }).ToList();
            SqlConnection.Close();
            return List;
        }
        public StaticPagedList<dtoAshDeposits> GetAshDeposits(DateTime AshDepositDate, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            DateTime dtStarDate = AshDepositDate.Year == 1 ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0) : AshDepositDate;
            DateTime dtEndDate = AshDepositDate.Year == 1 ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59) : AshDepositDate.AddDays(1);

            string ConditionDate = $" AND ( tblAshDeposits.AshDepositDate >= '{dtStarDate.ToString("yyyy-MM-dd")}' ) ";
            ConditionDate += $" AND ( tblAshDeposits.AshDepositDate < '{dtEndDate.ToString("yyyy-MM-dd")}') ";
            string Sort = QueryBuilder.GetSorting("tblAshDeposits.Name", "ASC", sSortingOrder);
            string Select = @" tblAshDeposits.BirthDate ,tblAshDeposits.DeathDate ,tblAshDeposits.AshDepositDate,tblAshDeposits.AshDepositID ,
                                tblAshDeposits.DeathCertificate ,tblAshDeposits.CremationCertificate ,tblAshDeposits.Ticket,
                            Concat(tblAshDeposits.Name, ' ', tblAshDeposits.PSurname, ' ', tblAshDeposits.MSurname) as Name, tblFederalTax.Cost as FederalTax";
            string Join = " LEFT JOIN tblFederalTax ON tblFederalTax.FederalTaxID = tblAshDeposits.FederalTaxID";

            List<object> Values = new List<object>() { 1 };
            string Conditions = db.tblAshDeposits.GetCondition(Values, x => x.Active);

            var query = QueryBuilder.GetQuery(typeof(tblAshDeposits).Name, Select, Join, Conditions + ConditionDate, Sort, iPage, PageSize);
            List<dtoAshDeposits> lst = sqlConnection.Query<dtoAshDeposits>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoAshDeposits>(lst, iPage, PageSize, totalCount);
        }
        public List<tblAshDeposits> GetListByYearAndMonth(int Year, int Month)
        {
            string Query = @"SELECT MONTH(AshDepositDate) Month,YEAR(AshDepositDate ) Year, DAY(AshDepositDate ) Day  
                                FROM tblAshDeposits WHERE MONTH(AshDepositDate) = @Month AND YEAR(AshDepositDate ) = @Year
                                GROUP BY MONTH(AshDepositDate),YEAR(AshDepositDate ), DAY(AshDepositDate ) ";

            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblAshDeposits> List = SqlConnection.Query<tblAshDeposits>(Query, new { Year, Month }).ToList();
            SqlConnection.Close();
            return List;
        }


        public Response Create(tblAshDeposits model)
        {
            Response response = new Response();

            try
            {
                db.tblAshDeposits.Attach(model);
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
        public Response Update(tblAshDeposits model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblAshDeposits.Attach(model);
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
        public tblAshDeposits GetAshDepositsByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, F.Cost as FederalTax
                FROM tblAshDeposits AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                LEFT JOIN tblFederalTax AS F ON F.FederalTaxID = U.FederalTaxID
                WHERE U.Active = 1 AND U.AshDepositID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblAshDeposits AshDeposits = SqlConnection.Query<tblAshDeposits>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return AshDeposits;
        }

        public List<tblAshDeposits> GetListAutocompleteByID(string id)
        {
            string Query = @"
                SELECT TOP (10) AshDepositID, CONCAT(Name, ' ', PSurname, ' ', MSurname) AS FullName
                FROM tblAshDeposits
                WHERE Active = 1 AND AshDepositID LIKE '%' + @id + '%'";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblAshDeposits> AshDeposits = SqlConnection.Query<tblAshDeposits>(Query, new { id }).ToList();
            SqlConnection.Close();
            return AshDeposits;
        }
    }
}
