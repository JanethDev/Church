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
    public class MaintenanceFeeB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public Response Create(tblMaintenanceFee model)
        {
            Response response = new Response();
            try
            {
                db.tblMaintenanceFee.Attach(model);
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
        public tblMaintenanceFee GetLastReccord(int IsShared)
        {
            string Query = @"
                SELECT U.*, UC.UserName AS CreatedByName, UU.UserName AS UpdatedByName 
                FROM tblMaintenanceFee AS U
                INNER JOIN tblUsers AS UC ON UC.UserID = U.CreatedBy
                INNER JOIN tblUsers AS UU ON UU.UserID = U.UpdatedBy 
                WHERE IsShared = @IsShared
                ORDER BY MaintenanceFeeID DESC";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblMaintenanceFee tblMaintenanceFee = SqlConnection.Query<tblMaintenanceFee>(Query, new { IsShared }).FirstOrDefault();
            SqlConnection.Close();
            return tblMaintenanceFee;
        }

        public decimal GetMaintenanceFeeCost(int ID) {
            string Query = @"SELECT Cost
                                FROM tblMaintenanceFee
                                WHERE MaintenanceFeeID = " + ID;
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            decimal cost = SqlConnection.Query<decimal>(Query).FirstOrDefault();
            SqlConnection.Close();
            return cost;
        }
    }
}
