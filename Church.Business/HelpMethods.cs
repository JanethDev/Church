using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Church.Business
{
    public class HelpMethods
    {
        
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            string algo = "XXXX-AAAA-ADSADSADAS";

            algo.RemoveXChar();

            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && column.ColumnName != "DeletedDate" && column.ColumnName != "DeletedBy")
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public object Autocompletes(string Column, string Filter, string Table)
        {
            List<string> value = new List<string>();

            if (Filter.Length > 50)
            {
                Filter = Truncate(Filter, 50);
            }

            SqlConnection sqlConnection = new SqlConnection(coneccion);

            value = sqlConnection.Query<string>("Autocompletes", new
            {
                Column = Column,
                Filter = Filter,
                Table = Table
            },
            commandType: CommandType.StoredProcedure).ToList();

            sqlConnection.Close();
            return value;
        }

        private string Truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public bool CheckIfExists(string Column, string String, string ColumnID, int Int, string Table)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string value = sqlConnection.Query<string>("CheckIfExists", new
            {
                Column = Column,
                String = String,
                ColumnID = ColumnID,
                Int = Int,
                Table = Table
            },
            commandType: CommandType.StoredProcedure).SingleOrDefault();
            sqlConnection.Close();
            return value == "true";
        }

        public string GetStringbyString(string Column, string Value, string ColumntoGet, string Table)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string value = sqlConnection.Query<string>("GetStringbyString", new
            {
                Column = Column,
                Value = Value,
                ColumntoGet = ColumntoGet,
                Table = Table
            },
            commandType: CommandType.StoredProcedure).SingleOrDefault();
            sqlConnection.Close();
            return value;
        }

        public int GetIntbyString(string Column, string Value, string ColumntoGet, string Table)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            int value = sqlConnection.Query<int>("GetIntbyString", new
            {
                Column = Column,
                Value = Value,
                ColumntoGet = ColumntoGet,
                Table = Table
            },
            commandType: CommandType.StoredProcedure).SingleOrDefault();

            sqlConnection.Close();
            return value;
        }

        public string GetUserRol(string UserName)
        {
            SqlConnection sqlConnection = new SqlConnection(coneccion);

            string value = sqlConnection.Query<string>("GetUserRol", new
            {
                UserName = UserName
            },
            commandType: CommandType.StoredProcedure).SingleOrDefault();

            sqlConnection.Close();
            return value;
        }

        public DateTime GetDateTime()
        {
            //    DateTime ServerDateTimeNow = new DateTime();
            //    if (Boolean.Parse(ConfigurationManager.AppSettings["Azure"]) == true)
            //    {
            //        int hours = int.Parse(ConfigurationManager.AppSettings["Hours"]);
            //        ServerDateTimeNow = DateTime.Now.AddHours(hours);
            //    }
            //    else
            //    {
            //        ServerDateTimeNow = DateTime.Now;
            //    }

            //    return ServerDateTimeNow;

            DateTime UtcNow = DateTime.UtcNow;

            DateTime utcTime = new DateTime(UtcNow.Year, UtcNow.Month, UtcNow.Day, UtcNow.Hour, UtcNow.Minute, UtcNow.Second);
            string sTimeZone = "Pacific Standard Time";
            DateTime dtNow;
            try
            {
                TimeZoneInfo PSTZone = TimeZoneInfo.FindSystemTimeZoneById(sTimeZone);
                dtNow = TimeZoneInfo.ConvertTimeFromUtc(utcTime, PSTZone);

            }
            catch (Exception)
            {
                return DateTime.Now;
            }
            return dtNow;

        }

        public string QueryDate(string Prop, string Cond, DateTime Filter)
        {
            return string.Format(" AND CAST({0} AS DATE) {1} CAST('{2}' AS DATE) ", Prop, Cond, Filter.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
