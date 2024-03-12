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
    public class CryptPositionTypesB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public tblCryptPositionTypes GetByCryptSectionId_CryptKey(int CryptSectionId, string CryptKey)
        {
            string sQuery = @"
                    SELECT CryPosTyp.*
                    FROM tblCryptPositionTypes CryPosTyp
                    WHERE CryPosTyp.Active = 1  AND CryPosTyp.CryptSectionID = @CryptSectionId AND CryPosTyp.CryptKey = @CryptKey";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            var model = SqlConnection.QueryFirstOrDefault<tblCryptPositionTypes>(sQuery, new { CryptSectionId, CryptKey });
            SqlConnection.Close();
            return model;
        }

        public List<tblCryptPositionTypes> GetListByCryptSectionId(int CryptSectionId)
        {
            string sQuery = @"
                SELECT CryPosTyp.*
                FROM tblCryptPositionTypes CryPosTyp
                WHERE CryPosTyp.Active = 1  AND CryPosTyp.CryptSectionID = @CryptSectionId";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            var model = SqlConnection.Query<tblCryptPositionTypes>(sQuery, new { CryptSectionId }).ToList();
            SqlConnection.Close();
            return model;
        }

        public Response Create(tblCryptPositionTypes model)
        {
            Response response = new Response();

            try
            {
                db.tblCryptPositionTypes.Attach(model);
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
        public Response Update(tblCryptPositionTypes model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                db = new ChurchDB();
                db.tblCryptPositionTypes.Attach(model);
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
    }
}
