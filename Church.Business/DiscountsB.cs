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
    public class DiscountsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public StaticPagedList<dtoDiscounts> GetDiscounts(string sName, string InitDate, string EndDate, int iPage, string sSortingOrder)
        {
            EndDate = EndDate != "" ? EndDate.Split('/')[2] + "-" + EndDate.Split('/')[1] + "-" + EndDate.Split('/')[0] + " 23:59:59" : "";
            InitDate = InitDate != "" ? InitDate.Split('/')[2] + "-" + InitDate.Split('/')[1] + "-" + InitDate.Split('/')[0] : "";

            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblDiscounts.Percentage", "ASC", sSortingOrder);
            string Select = db.tblDiscounts.SelectColumns(x => x.Percentage, x => x.InitDate, x => x.EndDate, x => x.CreatedDate, x => x.DiscountID);
            string wheredate = "";
            if (InitDate != "" && EndDate != "")
            {
                wheredate = " AND tblDiscounts.InitDate >= @InitDate AND tblDiscounts.EndDate <= @EndDate ";
            }
            else if (InitDate != "" && EndDate == "")
            {
                wheredate = " AND tblDiscounts.InitDate >= @InitDate ";
            }
            else if (InitDate == "" && EndDate != "")
            {
                wheredate = " AND tblDiscounts.EndDate <= @EndDate ";
            }
            List<object> Values = new List<object>() { sName, 1};
            string Conditions = db.tblDiscounts.GetCondition(Values, x => x.Percentage, x => x.Active) + wheredate;

            var query = QueryBuilder.GetQuery(typeof(tblDiscounts).Name, Select, "", Conditions, Sort, iPage, PageSize);
            List<dtoDiscounts> lst = sqlConnection.Query<dtoDiscounts>(query, new { InitDate, EndDate }).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoDiscounts>(lst, iPage, PageSize, totalCount);
        }

        public Response Create(tblDiscounts model)
        {
            Response response = new Response();

            try
            {
                db.tblDiscounts.Attach(model);
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
        public Response Update(tblDiscounts model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblDiscounts.Attach(model);
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
        public List<tblDiscounts> GetList()
        {
            string Query = @"
                SELECT U.*
                FROM tblDiscounts AS U
                WHERE U.Active = 1 ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblDiscounts> Discounts = SqlConnection.Query<tblDiscounts>(Query).ToList();
            SqlConnection.Close();
            return Discounts;
        }
        public List<tblDiscounts> GetList2()
        {
            string Query = @"
                SELECT U.*, CONCAT(U.Percentage,'% ',U.Description) as Description
                FROM tblDiscounts AS U
                WHERE U.Active = 1 ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblDiscounts> Discounts = SqlConnection.Query<tblDiscounts>(Query).ToList();
            SqlConnection.Close();
            return Discounts;
        }
        public tblDiscounts GetDiscountsByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblDiscounts AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.DiscountID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblDiscounts Discounts = SqlConnection.Query<tblDiscounts>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return Discounts;
        }

        public tblDiscounts GetByDate(DateTime DateNow)
        {
            string Query = @"
                SELECT Disc.*
                FROM tblDiscounts Disc
                WHERE Disc.Active = 1 
	                AND CAST(@DateNow AS DATE) >= CAST(Disc.InitDate AS DATE)
	                AND CAST(@DateNow AS DATE) <= CAST(Disc.EndDate AS DATE)";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblDiscounts Discounts = SqlConnection.QueryFirstOrDefault<tblDiscounts>(Query, new { DateNow });
            SqlConnection.Close();
            return Discounts;
        }
    }
}
