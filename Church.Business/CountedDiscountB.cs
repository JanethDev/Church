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
    public class CountedDiscountB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public Response Create(tblCountedDiscount model)
        {
            Response response = new Response();
            try
            {
                db.tblCountedDiscount.Attach(model);
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
        public tblCountedDiscount GetLastReccord()
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblCountedDiscount AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy ORDER BY CountedDiscountID DESC";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblCountedDiscount tblCountedDiscount = SqlConnection.Query<tblCountedDiscount>(Query).FirstOrDefault();
            SqlConnection.Close();
            return tblCountedDiscount;
        }
    }
}
