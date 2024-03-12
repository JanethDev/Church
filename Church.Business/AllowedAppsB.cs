using Church.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Business
{
  public  class AllowedAppsB
    {

        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();


        public bool Validate(string AppId , string AppKey)
        {
            string Query = @"
                SELECT count(0)
                FROM tblAllowedApps 
                WHERE Active = 1 AND AppId = @AppId and AppKey=@AppKey";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            int count = SqlConnection.Query<int>(Query, new { AppId, AppKey }).FirstOrDefault();
            SqlConnection.Close();
            return count>0;
        }
        public tblAllowedApps ValidAppId(string AppId)
        {
            string Query = @"
                SELECT *
                FROM tblAllowedApps 
                WHERE  AppId = @AppId ";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblAllowedApps tblAllowedApps = SqlConnection.Query<tblAllowedApps>(Query, new { AppId }).FirstOrDefault();
            SqlConnection.Close();
            return tblAllowedApps;
        }


    }     
}
