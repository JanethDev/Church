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
    public class CryptsSectionsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        
        public tblCryptsSections GetById(int Id)
        {
            string Query = @"
                    SELECT CrySec.*
                    FROM tblCryptsSections CrySec
                    WHERE CrySec.Active = 1 AND CrySec.CryptSectionID = @Id";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            var model = SqlConnection.Query<tblCryptsSections>(Query, new { Id }).FirstOrDefault();
            SqlConnection.Close();
            return model;
        }
    }
}
