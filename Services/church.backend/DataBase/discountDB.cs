using church.backend.Models.catalogue.cathedral;
using church.backend.Models.catalogue.discounts;
using church.backend.Models.enums;
using church.backend.services.Models;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace church.backend.services.DataBase
{
    public class discountDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public discountDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public discount_response consultAll() 
        {
            try
            {
                discount_response response = new discount_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:discounts:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new discount()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    description = reader["description"].ToString()!,
                                    start_date = DateTime.Parse(reader["start_date"].ToString()!),
                                    end_date = DateTime.Parse(reader["end_date"].ToString()!),
                                    percentage = double.Parse(reader["percentage"].ToString()!),
                                    status = reader["status"].ToString()!,
                                    status_id = int.Parse(reader["cat_status_id"].ToString()!),
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new discount_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public discount_response consultByStatus(discount_status status)
        {
            try
            {
                discount_response response = new discount_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:discounts:consultByStatus"]!, (int)status);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new discount()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    description = reader["description"].ToString()!,
                                    start_date = DateTime.Parse(reader["start_date"].ToString()!),
                                    end_date = DateTime.Parse(reader["end_date"].ToString()!),
                                    percentage = double.Parse(reader["percentage"].ToString()!),
                                    status = reader["status"].ToString()!,
                                    status_id = int.Parse(reader["cat_status_id"].ToString()!),
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new discount_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse createDiscount(create_discount_request data, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:discounts:create"]!
                        , data.description
                        , data.percentage
                        , data.start_date.ToString("yyyy-MM-dd")
                        , data.end_date.ToString("yyyy-MM-dd")
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

        public GeneralResponse updateDiscount(update_discount_request data, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:discounts:create"]!
                        , data.description
                        , data.percentage
                        , data.start_date.ToString("yyyy-MM-dd")
                        , data.end_date.ToString("yyyy-MM-dd")
                        , data.status_id
                        , data.id
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