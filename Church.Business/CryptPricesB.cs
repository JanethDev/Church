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
    public class CryptPricesB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        
        public List<tblCryptPrices> GetList()
        {
            string Query = @"
                    SELECT CryPri.*
                    FROM tblCryptPrices CryPri";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            var model = SqlConnection.Query<tblCryptPrices>(Query).ToList();
            SqlConnection.Close();
            return model;
        }
    }
}
