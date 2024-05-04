using church.backend.Models.catalogue.cathedral;
using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class cathedralDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public cathedralDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public cathedral_response consultCathedrals() 
        {
            try
            {
                cathedral_response response = new cathedral_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:cathedral:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new cathedral()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    name = reader["name"].ToString()!,
                                    city = reader["city"].ToString()!,
                                    city_id = int.Parse(reader["cat_cities_id"].ToString()!),
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new cathedral_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse createCathedrals(create_cathedral_request data, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:cathedral:create"]!
                        , data.name
                        , data.city_id
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