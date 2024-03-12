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
    public class ScheduleB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        public List<tblSchedule> GetListByDayNumber(int DayNumber)
        {
            string Query = @"select tblSchedule.*
                            FROM tblSchedule WHERE DayNumber = @DayNumber AND Active = 1";

            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblSchedule> List = SqlConnection.Query<tblSchedule>(Query, new { DayNumber }).ToList();
            SqlConnection.Close();
            return List;
        }
        public List<tblSchedule> GetListByDay(string Day)
        {
            string Query = @"select tblSchedule.*
                            FROM tblSchedule WHERE Day = @Day AND Active = 1";

            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblSchedule> List = SqlConnection.Query<tblSchedule>(Query, new { Day }).ToList();
            SqlConnection.Close();
            return List;
        }
        public List<tblSchedule> GetList()
        {
            string Query = @"select tblSchedule.*
                            FROM tblSchedule WHERE Active = 1";

            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblSchedule> List = SqlConnection.Query<tblSchedule>(Query).ToList();
            SqlConnection.Close();
            return List;
        }
        public Response Create(tblSchedule model)
        {
            Response response = new Response();

            try
            {
                db.tblSchedule.Attach(model);
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
        public Response Update(tblSchedule model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblSchedule.Attach(model);
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
