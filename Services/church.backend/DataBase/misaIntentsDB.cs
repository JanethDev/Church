using church.backend.Models.enums;
using church.backend.Models.misa_intents;
using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class misaIntentsDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public misaIntentsDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public misa_intents_response consultAll() 
        {
            try
            {
                misa_intents_response response = new misa_intents_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:misa_intents:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new misa_intents()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    intent_id = int.Parse(reader["cat_intents_type_id"].ToString()!),
                                    intent = reader["intent"].ToString()!,
                                    misa_id = int.Parse(reader["cat_misa_id"].ToString()!),
                                    misa_day = reader["day"].ToString()!,
                                    misa_hour = reader["hour"].ToString()!,
                                    date = DateTime.Parse(reader["date"].ToString()!),
                                    mention_person = reader["mention_person"].ToString()!,
                                    applicant = reader["applicant"].ToString()!,
                                    phone = reader["phone"].ToString()!,
                                    donation = double.Parse(reader["donation"].ToString()!),
                                    exchange_rate = double.Parse(reader["exchange_rate"].ToString()!),
                                    decription = reader["decription"].ToString()!,
                                    status_id = int.Parse(reader["cat_status_id"].ToString()!),
                                    status = reader["description"].ToString()!,
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new misa_intents_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public misa_intents_response consultByStatus(misa_intents_status status)
        {
            try
            {
                misa_intents_response response = new misa_intents_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:misa_intents:consultByStatus"]!, (int)status);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new misa_intents()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    intent_id = int.Parse(reader["cat_intents_type_id"].ToString()!),
                                    intent = reader["intent"].ToString()!,
                                    misa_id = int.Parse(reader["cat_misa_id"].ToString()!),
                                    misa_day = reader["day"].ToString()!,
                                    misa_hour = reader["hour"].ToString()!,
                                    date = DateTime.Parse(reader["date"].ToString()!),
                                    mention_person = reader["mention_person"].ToString()!,
                                    applicant = reader["applicant"].ToString()!,
                                    phone = reader["phone"].ToString()!,
                                    donation = double.Parse(reader["donation"].ToString()!),
                                    exchange_rate = double.Parse(reader["exchange_rate"].ToString()!),
                                    decription = reader["decription"].ToString()!,
                                    status_id = int.Parse(reader["cat_status_id"].ToString()!),
                                    status = reader["description"].ToString()!,
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new misa_intents_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse createMisaIntent(create_misa_intent_request data, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:misa_intents:create"]!
                        , data.intent_id
                        , data.misa_id
                        , data.date.ToString("yyyy-MM-dd")
                        , data.mention_person
                        , data.applicant
                        , data.phone
                        , data.donation
                        , data.exchange_rate
                        , data.decription
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

        public GeneralResponse updateMisaIntent(update_misa_intent_request data, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:misa_intents:update"]!
                        , data.intent_id
                        , data.misa_id
                        , data.date.ToString("yyyy-MM-dd")
                        , data.mention_person
                        , data.applicant
                        , data.phone
                        , data.donation
                        , data.exchange_rate
                        , data.decription
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