using church.backend.Models.catalogue.civil_status;
using church.backend.Models.catalogue.intents;
using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class intentsDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public intentsDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public intents_response consultIntents() 
        {
            try
            {
                intents_response response = new intents_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:intents_type:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new intents()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    intent = reader["intent"].ToString()!
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new intents_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse createIntent(string name, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:intents_type:create"]!
                        , name
                        , user_id
                    );
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