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
    public class CommissionAgentsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public StaticPagedList<dtoCommissionAgents> GetCommissionAgents(string sName, string sPSurname, string sCathedral, DateTime Date, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblCommissionAgents.Name", "ASC", sSortingOrder);
            string Select = db.tblCommissionAgents.SelectColumns(x => x.Name, x => x.PSurname, x => x.CreatedDate, x => x.CommissionAgentID) + ", tblCathedrals.Name as Cathedral";
            string Join = " JOIN tblCathedrals ON tblCathedrals.CathedralID = tblCommissionAgents.CathedralID ";

            List<object> Values = new List<object>() { sName, sPSurname, Date, 1 };
            string Conditions = db.tblCommissionAgents.GetCondition(Values, x => x.Name, x => x.PSurname, x => x.CreatedDate, x => x.Active) + $" AND ('' = '{sCathedral}' OR tblCathedrals.Name LIKE '{sCathedral}')";

            var query = QueryBuilder.GetQuery(typeof(tblCommissionAgents).Name, Select, Join, Conditions, Sort, iPage, PageSize);
            List<dtoCommissionAgents> lst = sqlConnection.Query<dtoCommissionAgents>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoCommissionAgents>(lst, iPage, PageSize, totalCount);
        }
        public Response Create(tblCommissionAgents model)
        {
            Response response = new Response();

            try
            {
                db.tblCommissionAgents.Attach(model);
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
        public Response Update(tblCommissionAgents model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblCommissionAgents.Attach(model);
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
        public List<tblCommissionAgents> GetList()
        {
            string Query = @"
                SELECT U.*,
                concat(U.Name,' ',U.PSurname,' ',U.MSurname) FullName
                FROM tblCommissionAgents AS U
                WHERE U.Active = 1 ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblCommissionAgents> CommissionAgents = SqlConnection.Query<tblCommissionAgents>(Query).ToList();
            SqlConnection.Close();
            return CommissionAgents;
        }
        public tblCommissionAgents GetCommissionAgentsByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName, C.CityID, C.Name AS Cathedral, CT.Name AS City,
                concat(U.Name,' ',U.PSurname,' ',U.MSurname) FullName
                FROM tblCommissionAgents AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                INNER JOIN tblCathedrals AS C ON C.CathedralID = U.CathedralID
                INNER JOIN tblCities AS CT ON CT.CityID = C.CityID
                WHERE U.Active = 1 AND U.CommissionAgentID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblCommissionAgents CommissionAgents = SqlConnection.Query<tblCommissionAgents>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return CommissionAgents;
        }
    }
}
