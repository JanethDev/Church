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
    public class ContractsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public Response Create(tblContracts model)
        {
            Response response = new Response();

            try
            {
                db.tblContracts.Attach(model);
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

        public int GetContractID(int ContractType, int ContractID)
        {
            string Query = @"SELECT *
                            FROM tblContracts
                            WHERE ContractType = " + ContractType + " AND ContractID >= " + ContractID + " ORDER BY ContractID DESC";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            int model = SqlConnection.Query<int>(Query).FirstOrDefault();
            SqlConnection.Close();
            return model;
        }
    }
}
