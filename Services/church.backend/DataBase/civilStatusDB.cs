using church.backend.Models.catalogue.cathedral;
using church.backend.Models.catalogue.cities;
using church.backend.Models.catalogue.civil_status;
using church.backend.services.Models;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace church.backend.services.DataBase
{
    public class civilStatusDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public civilStatusDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public civil_status_response consultCivilStatus() 
        {
            try
            {
                civil_status_response response = new civil_status_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:civil_status:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new civil_status()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    civilStatus = reader["description"].ToString()!
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new civil_status_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse createCivilStatus(string name, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:civil_status:create"]!
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