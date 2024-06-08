using church.backend.Models.catalogue.crypts;
using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class cryptDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public cryptDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public crypt_response consultByZone(string zone) 
        {
            try
            {
                crypt_response response = new crypt_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:crypts:consultByZone"]!, zone);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new crypt()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    places_shared = int.Parse(reader["places_shared"].ToString()!),
                                    price = double.Parse(reader["price"].ToString()!),
                                    price_shared = double.Parse(reader["price_shared"].ToString()!),
                                    full_position = reader["full_position"].ToString()!,
                                    zone = reader["zone"].ToString()!,
                                    position = reader["position"].ToString()!,
                                    is_shared = reader["is_shared"].ToString()!.ToLower()=="true",
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
                return new crypt_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public crypt_response consultById(int id)
        {
            try
            {
                crypt_response response = new crypt_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:crypts:consultById"]!, id);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new crypt()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    places_shared = int.Parse(reader["places_shared"].ToString()!),
                                    price = double.Parse(reader["price"].ToString()!),
                                    price_shared = double.Parse(reader["price_shared"].ToString()!),
                                    full_position = reader["full_position"].ToString()!,
                                    zone = reader["zone"].ToString()!,
                                    position = reader["position"].ToString()!,
                                    is_shared = reader["is_shared"].ToString()!.ToLower() == "true",
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
                return new crypt_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public crypt_zones consultZones()
        {
            try
            {
                crypt_zones response = new crypt_zones() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:crypts:consultZones"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(reader["zone"].ToString()!);
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new crypt_zones()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse updateCrypt(update_crypt_request data, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:crypts:update"]!
                        , data.places_shared
                        , data.price
                        , data.price_shared
                        , data.is_shared?1:0
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

        /*public void addCrypt()
        {
            List< cprices > sections = new List<cprices>()
            {
                new cprices(){letter="A",price= 85168.00}
                , new cprices(){letter="B",price= 87760.00}
                , new cprices(){letter="C",price= 90352.00}
                , new cprices(){letter="D",price= 91200.00}
                , new cprices(){letter="E",price= 93120.00}
                , new cprices(){letter="F",price= 96000.00}
                , new cprices(){letter="G",price= 96000.00}
                , new cprices(){letter="H",price= 96000.00}
                , new cprices(){letter="I",price= 96000.00}
            };
            List<cprices> sections = new List<cprices>()
            {
                new cprices(){letter="A",price= 78040.00}
                , new cprices(){letter="B",price= 80308.00}
                , new cprices(){letter="C",price= 79800.00}
                , new cprices(){letter="D",price= 81480.00}
                , new cprices(){letter="E",price= 84000.00}
                , new cprices(){letter="F",price= 84000.00}
                , new cprices(){letter="G",price= 84000.00}
                , new cprices(){letter="H",price= 84000.00}
            };
            List<cprices> sections = new List<cprices>()
            {
                new cprices(){letter="Z",price= 66376.00}
                , new cprices(){letter="A",price= 68320.00}
                , new cprices(){letter="B",price= 70264.00}
                , new cprices(){letter="C",price= 68400.00}
                , new cprices(){letter="D",price= 69840.00}
                , new cprices(){letter="E",price= 72000.00}
                , new cprices(){letter="F",price= 72000.00}
                , new cprices(){letter="G",price= 72000.00}
                , new cprices(){letter="H",price= 72000.00}
            };
            List<cprices> sections = new List<cprices>()
            {
                new cprices(){letter="Z",price= 91200.00}
                , new cprices(){letter="A",price= 93120.00}
                , new cprices(){letter="B",price= 96000.00}
                , new cprices(){letter="C",price= 96000.00}
                , new cprices(){letter="D",price= 96000.00}
                , new cprices(){letter="E",price= 96000.00}
            };
            List<cprices> sections = new List<cprices>()
            {
                new cprices(){letter="B",price= 78040.00}
                , new cprices(){letter="C",price= 80308.00}
                , new cprices(){letter="D",price= 79800.00}
                , new cprices(){letter="E",price= 81480.00}
                , new cprices(){letter="F",price= 84000.00}
                , new cprices(){letter="G",price= 84000.00}
                , new cprices(){letter="H",price= 84000.00}
                , new cprices(){letter="I",price= 84000.00}
            };
            List<cprices> sections = new List<cprices>()
            {
                new cprices(){letter="A",price= 79800.00}
                , new cprices(){letter="B",price= 81480.00}
                , new cprices(){letter="C",price= 84000.00}
                , new cprices(){letter="D",price= 84000.00}
                , new cprices(){letter="E",price= 84000.00}
                , new cprices(){letter="F",price= 84000.00}
            };
            foreach (cprices cpr in sections)
            {
                for (int index = 1; index <= 30; index++)
                {
                    string position = cpr.letter + index;
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(DataBaseConection))
                        {
                            string query = @"declare @position varchar(20)= '{0}'
                        declare @zone varchar(20)= 'AG02U03'
                        declare @status int = '1008'
                        declare @fullPosition varchar(100) = @zone + @position
                        declare @price decimal(10,2) = '{1}'

                        INSERT into [cat_crypts] ([cat_status_id],[full_position],[zone],[position],[is_shared],[places_shared],[price],[price_shared]) VALUES
                        (@status, @fullPosition, @zone, @position, '0','0',@price,'0.00')";

                            query = string.Format(query, position, cpr.price);
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                connection.Open();
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }*/
    }
}

/*class cprices()
{
    public string letter { get; set; }
    public double price { get; set; }
}*/