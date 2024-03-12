using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Church.Data;

namespace Church.Business
{
    public class RolesB
    {
        public static List<tblRoles> GetList()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            sqlConnection.Open();
            List<tblRoles> lst = sqlConnection.Query<tblRoles>("GetRoles", commandType: CommandType.StoredProcedure).ToList();
            sqlConnection.Close();
            return lst;
        }
    }
}
