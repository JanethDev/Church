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
    public class CivilStatusB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public StaticPagedList<dtoCivilStatus> GetCivilStatus(string sName, DateTime Date, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string Sort = QueryBuilder.GetSorting("tblCivilStatus.Name", "ASC", sSortingOrder);
            string Select = db.tblCivilStatus.SelectColumns(x => x.Name, x => x.CreatedDate, x => x.CivilStatusID);

            List<object> Values = new List<object>() { sName, Date };
            string Conditions = db.tblCivilStatus.GetCondition(Values, x => x.Name, x => x.CreatedDate);

            var query = QueryBuilder.GetQuery(typeof(tblCivilStatus).Name, Select, "", Conditions, Sort, iPage, PageSize);
            List<dtoCivilStatus> lst = sqlConnection.Query<dtoCivilStatus>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoCivilStatus>(lst, iPage, PageSize, totalCount);
        }
        public List<tblCivilStatus> GetList()
        {
            string Query = @"
                SELECT U.*
                FROM tblCivilStatus AS U
                WHERE U.Active = 1 ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblCivilStatus> model = SqlConnection.Query<tblCivilStatus>(Query).ToList();
            SqlConnection.Close();
            return model;
        }
        public Response Create(tblCivilStatus model)
        {
            Response response = new Response();

            try
            {
                db.tblCivilStatus.Attach(model);
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
        public Response Update(tblCivilStatus model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblCivilStatus.Attach(model);
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
        public tblCivilStatus GetCivilStatusByID(int id)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblCivilStatus AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy
                WHERE U.Active = 1 AND U.CivilStatusID = @id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblCivilStatus CivilStatus = SqlConnection.Query<tblCivilStatus>(Query, new { id }).FirstOrDefault();
            SqlConnection.Close();
            return CivilStatus;
        }
    }
}
