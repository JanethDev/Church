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
    public class CitiesB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

         public List<tblCities> GetList()
        {
            string Query = @"
                SELECT *
                FROM tblCities";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblCities> Cities = SqlConnection.Query<tblCities>(Query).ToList();
            SqlConnection.Close();
            return Cities;
        }
    }
}
