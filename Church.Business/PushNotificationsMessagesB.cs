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
    public class PushNotificationsMessagesB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());
        public StaticPagedList<dtoPushNotificationsMessages> GetPushNotificationsMessages(string Sorting_Order, string Name, string Message, DateTime Date, string Status, tblUsers tblUsers, int Page)
        {
            SqlConnection sqlConn = new SqlConnection(coneccion);
            string Sort = QueryBuilder.GetSorting("tblPushNotificationsMessages.PushNotificationMessageID", "ASC", Sorting_Order);
            string Select = db.tblPushNotificationsMessages.SelectColumns(x => x.PushNotificationMessageID, x => x.Title, x => x.Message, x => x.Draft, x => x.UpdatedDate);
            string Join = String.Empty;
            List<object> Values = new List<object>() { Name, Message, Date, Status, "1" };
            string Conditions = db.tblPushNotificationsMessages.GetCondition(Values, x => x.Title, x => x.Message, x => x.UpdatedDate, x => x.Draft, x => x.Active);

            var query = QueryBuilder.GetQuery(typeof(tblPushNotificationsMessages).Name, Select, Join, Conditions, Sort, Page, PageSize);
            List<dtoPushNotificationsMessages> lst = sqlConn.Query<dtoPushNotificationsMessages>(query).ToList();
            lst.ForEach(x => { if (x.Draft == true) { x.Estatus = "Borrador"; } else { x.Estatus = "Envíado"; } });

            int totalCount = lst?.FirstOrDefault()?.TotalCount ?? 0;
            sqlConn.Close();

            return new StaticPagedList<dtoPushNotificationsMessages>(lst, Page, PageSize, totalCount);
        }
        public Response Create(tblPushNotificationsMessages model)
        {
            Response response = new Response();

            try
            {
                db.tblPushNotificationsMessages.Attach(model);
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
        public Response Update(tblPushNotificationsMessages model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblPushNotificationsMessages.Attach(model);
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
        public tblPushNotificationsMessages GetById(int id)
        {
            string Query = @"SELECT a.*, b.UserName as CreatedByName, c.UserName as UpdatedByName 
                FROM tblPushNotificationsMessages AS a 
                INNER JOIN tblUsers AS b on b.UserID = a.CreatedBy
                INNER JOIN tblUsers AS c on c.UserID = a.UpdatedBy
                WHERE a.Active = 1 AND PushNotificationMessageID = @id";

            SqlConnection sqlConn = new SqlConnection(coneccion);
            tblPushNotificationsMessages tblPushNotificationsMessages = sqlConn.Query<tblPushNotificationsMessages>(Query, new { id }).FirstOrDefault();
            sqlConn.Close();

            return tblPushNotificationsMessages;
        }
    }
}
