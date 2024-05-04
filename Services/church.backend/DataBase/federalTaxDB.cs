using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class federalTaxDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public federalTaxDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public GeneralResponse consultFederalTax() 
        {
            try
            {
                GeneralResponse response = new GeneralResponse() { code = -1, message = "error" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:federal_tax:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response = new GeneralResponse() 
                                { 
                                    code = int.Parse(reader["id"].ToString()!),
                                    message = reader["cost"].ToString()!
                                };
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse updateFederalTax(double cost, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse() { code = -1, message = "error" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:federal_tax:update"]!,cost,user_id);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response = new GeneralResponse()
                                {
                                    code = int.Parse(reader["code"].ToString()!),
                                    message = reader["message"].ToString()!
                                };
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }
    }
}