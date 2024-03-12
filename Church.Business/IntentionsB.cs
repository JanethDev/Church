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
    public class IntentionsB
    {
        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private ChurchDB db = new ChurchDB();
        private QueryBuilder QueryBuilder = new QueryBuilder();
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        HelpMethods HelpMethods = new HelpMethods();

        [MethodDescription(DescriptionES.INDEX_OF_NOF)]
        public StaticPagedList<dtoIntentions> GetIntentions(string Name, string MentionPerson, int IntentionTypeID, DateTime ApplicationDate, DateTime IntentionDate, string Hour, int iPage, string sSortingOrder)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            DateTime dtStarDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0); 
            DateTime dtEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59); 

            string ConditionDate = ApplicationDate.Year == 1 && IntentionDate.Year == 1 ?  $" AND ( tblIntentions.ApplicationDate >= '{dtStarDate.ToString("yyyy-MM-dd")}' ) ":"";
            ConditionDate += ApplicationDate.Year == 1 && IntentionDate.Year == 1 ? $" AND ( tblIntentions.ApplicationDate <= '{dtEndDate.ToString("yyyy-MM-dd")}') " : "";

            string sIntentionDate = IntentionDate.Year == 1 && IntentionDate.Year == 1 ?  $" AND ( tblIntentions.IntentionDate >= '{dtStarDate.ToString("yyyy-MM-dd")}' ) ":"";
            sIntentionDate += IntentionDate.Year == 1 && IntentionDate.Year == 1 ? $" AND ( tblIntentions.IntentionDate <= '{dtEndDate.ToString("yyyy-MM-dd")}') " : "";

            string Sort = QueryBuilder.GetSorting("tblIntentions.Name", "ASC", sSortingOrder);
            string Select = db.tblIntentions.SelectColumns(x => x.IntentionID, x => x.Name,x => x.MentionPerson, x => x.tblIntentionsType.Name, x => x.ScheduleID, x => x.ApplicationDate, x => x.IntentionDate) + ",tblSchedule.Hour";
            string Join = db.tblIntentions.GetJoin(x => x.IntentionTypeID, JoinType.JOIN, typeof(tblIntentionsType).Name);
            Join += db.tblIntentions.GetJoin(x => x.ScheduleID, JoinType.JOIN, typeof(tblSchedule).Name);

            List<object> Values = new List<object>() { Name, MentionPerson, IntentionTypeID, ApplicationDate, IntentionDate, 1 };
            string Conditions = db.tblIntentions.GetCondition(Values, x => x.Name,x => x.MentionPerson, x => x.IntentionTypeID, x => x.ApplicationDate, X => X.IntentionDate, x=> x.Active) +  $" AND ('' = '{Hour}' OR tblSchedule.Hour = '{ Hour}' ) ";

            var query = QueryBuilder.GetQuery(typeof(tblIntentions).Name, Select, Join, Conditions + ConditionDate, Sort, iPage, PageSize);
            List<dtoIntentions> lst = sqlConnection.Query<dtoIntentions>(query).ToList();

            int totalCount = lst.Count > 0 ? lst.First().TotalCount : 0;
            sqlConnection.Close();
            return new StaticPagedList<dtoIntentions>(lst, iPage, PageSize, totalCount);
        }

        public tblIntentions GetByID(int IntentionID)
        {
            string Query = @"
                SELECT I.*, S.Hour Schedule,
                    IT.Name AS IntentionTypeName,
                    CONCAT(UC.Name, ' ', UC.PSurname, ' ', UC.MSurname) AS CreatedByName,
                    CONCAT(UU.Name, ' ', UU.PSurname, ' ', UU.MSurname) AS UpdatedByName
                FROM tblIntentions I
                JOIN tblIntentionsType IT ON IT.IntentionTypeID = I.IntentionTypeID
                JOIN tblSchedule S ON S.ScheduleID = I.ScheduleID
                JOIN tblUsers UC ON UC.UserID = I.CreatedBy
                JOIN tblUsers UU ON UU.UserID = I.UpdatedBy
                WHERE I.IntentionID = @IntentionID
                    AND I.Active = 1";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            tblIntentions tblIntentions = SqlConnection.Query<tblIntentions>(Query, new { IntentionID }).FirstOrDefault();
            SqlConnection.Close();
            return tblIntentions;
        }
 
        public List<tblIntentions> GetListByYearAndMonth(int Year, int Month)
        {
            string Query = @"SELECT MONTH(IntentionDate) Month,YEAR(IntentionDate ) Year, DAY(IntentionDate ) Day  
                                FROM tblIntentions WHERE MONTH(IntentionDate) = @Month AND YEAR(IntentionDate ) = @Year
                                GROUP BY MONTH(IntentionDate),YEAR(IntentionDate ), DAY(IntentionDate ) ";

            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblIntentions> List = SqlConnection.Query<tblIntentions>(Query, new { Year, Month }).ToList();
            SqlConnection.Close();
            return List;
        }
        public List<tblIntentions> GetListForPDF(string Name, int IntentionTypeID, DateTime? ApplicationDate, DateTime? IntentionDate)
        {
            string QDate = "";
            if (ApplicationDate.HasValue)
                QDate += HelpMethods.QueryDate("ApplicationDate", "=", ApplicationDate.Value);
            if (IntentionDate.HasValue)
                QDate += HelpMethods.QueryDate("IntentionDate", "=", IntentionDate.Value);

            string Query = @"SELECT *
                            FROM tblIntentions
                            WHERE Active = 1
                                 AND ('' = @Name OR Name = @Name)
                                 AND (0 = @IntentionTypeID OR IntentionTypeID = @IntentionTypeID)"
                                 + QDate;

            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblIntentions> List = SqlConnection.Query<tblIntentions>(Query, new { Name, IntentionTypeID }).ToList();
            SqlConnection.Close();
            return List;
        }

        public Response Create(tblIntentions model)
        {
            Response response = new Response();

            try
            {
                db.tblIntentions.Attach(model);
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

        public Response Update(tblIntentions model)
        {
            Response response = new Response();

            try
            {
                if (db != null)
                    db.Dispose();

                model.UpdatedDate = DateTime.Now;
                db = new ChurchDB();
                db.tblIntentions.Attach(model);
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
