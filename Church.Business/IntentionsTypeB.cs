using System.Collections.Generic;
using System.Linq;
using Church.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace Church.Business
{
    public class IntentionsTypeB
    {
        private string coneccion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public List<tblIntentionsType> GetList()
        {
            string Query = @"
                SELECT *
                FROM tblIntentionsType";
            SqlConnection SqlConnection = new SqlConnection(coneccion);
            List<tblIntentionsType> lstIntentions = SqlConnection.Query<tblIntentionsType>(Query).ToList();
            SqlConnection.Close();
            return lstIntentions;
        }
    }
}
