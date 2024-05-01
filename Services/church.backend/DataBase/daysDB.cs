using church.backend.Models.catalogue.days;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class daysDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public daysDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public days_response consultDays() 
        {
            try
            {
                days_response response = new days_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:days:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new days()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    day = reader["day"].ToString()!
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new days_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }
    }
}