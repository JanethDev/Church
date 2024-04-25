using church.backend.services.Models;
using church.backend.services.Models.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.DataBase
{
    public class accessDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public accessDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:EFTFuel:dev"];
            _configuration = configuration;
        }

        public GeneralResponse ValidateAccount(ValidateAccountRequest request)
        {
            try
            {
                GeneralResponse response = new GeneralResponse()
                {
                    code = -1,
                    message = "El código no coincide"
                };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {                 
                    string query = string.Format(_configuration["queries:access:ValidateAccount"]
                                                , request.email
                                                , request.code
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
                                    code = 1,
                                    message = "success"
                                };
                            }
                        }
                    }
                }
                return response;
            }
            catch
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "No se logro validar la cuenta, por favor intente más tarde."
                };
            }
        }

        public GeneralResponse GetEmailValidationCode(string email, int brand)
        {
            try
            {
                GeneralResponse response = new GeneralResponse()
                {
                    code = -1,
                    message = ""
                };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:GetEmailValidationCode"]
                                                , email
                                                , brand
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
                                    code = 1,
                                    message = reader["success"].ToString()
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

        public GetClientResponse GetClient(string email,string password, string oldEncryptedPassword, string tempPassword)
        {
            try
            {
                GetClientResponse response = new GetClientResponse()
                {
                    code = -1,
                    message = "usuario no encontrado"
                };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:showClient"]
                                                , email
                                                , password
                                                , oldEncryptedPassword
                                                , tempPassword
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response = new GetClientResponse()
                                {
                                    code = 1,
                                    message = "success",
                                    data = new AppClient() {
                                        clientId = int.Parse(reader["clientId"].ToString()),
                                        name = reader["name"].ToString(),
                                        email = reader["email"].ToString(),
                                        phoneNumber = string.IsNullOrWhiteSpace(reader["phoneNumber"].ToString())? "0" : reader["phoneNumber"].ToString(),
                                        rfc = reader["rfc"].ToString(),
                                        isMoral = Convert.ToBoolean(reader["isMoral"].ToString()),
                                        city = reader["city"].ToString(),
                                        state = reader["state"].ToString(),
                                        postal_code = int.Parse(reader["postal_code"].ToString()),
                                        isValidated = bool.Parse(reader["isValidated"].ToString()),
                                        isEmailValidated = true//bool.Parse(reader["isEmailValidated"].ToString()),
                                    }
                                };
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new GetClientResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }
    
        public GeneralResponse SaveClient(SaveClientRequest request, string code)
        {
            try
            {
                GeneralResponse response = new GeneralResponse()
                {
                    code = -1,
                    message = "usuario no encontrado"
                };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:saveClient"]
                                                , request.name
                                                , request.email
                                                , request.password
                                                , request.phoneNumber
                                                , request.city
                                                , request.state
                                                , request.postal_code
                                                , request.rfc
                                                , request.isMoral
                                                , request.reference
                                                , code
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
                                    code = 1,
                                    message = reader["clientId"].ToString()
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

        public GetClientResponse GetClientById(int clientId)
        {
            try
            {
                GetClientResponse response = new GetClientResponse()
                {
                    code = -1,
                    message = "usuario no encontrado"
                };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:showClientItem"], clientId);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response = new GetClientResponse()
                                {
                                    code = 1,
                                    message = "success",
                                    data = new AppClient()
                                    {
                                        clientId = int.Parse(reader["clientId"].ToString()),
                                        name = reader["name"].ToString(),
                                        email = reader["email"].ToString(),
                                        phoneNumber = reader["phoneNumber"].ToString(),
                                        picture = Convert.ToBase64String(reader["picture"] as byte[])
                                    }
                                };
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new GetClientResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse UpdateClientPhoto(int clientId, byte[] photo)
        {
            try
            {               
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:updateClientPhoto"], clientId);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@img", photo));
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()){}
                        }
                    }
                }
                return new GeneralResponse() { 
                    code = 1,
                    message ="success"
                };
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

        public GeneralResponse ChangePassword(int clientId, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    //string newpassword = new CryptoService().Encrypt(password);
                    string query = string.Format(_configuration["queries:access:changePassword"]
                                                , clientId
                                                //, newpassword
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) { }
                        }
                    }
                }
                return new GeneralResponse()
                {
                    code = 1,
                    message = "success"
                };
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

        public GeneralResponse SetTempPassword(string email, string temp)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:setTemporalPassword"]
                                                , email
                                                , temp
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) { }
                        }
                    }
                }
                return new GeneralResponse()
                {
                    code = 1,
                    message = "success"
                };
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

        public GetClientResponse UpdateProfile(UpdateProfileRequest data)
        {
            try
            {
                GetClientResponse response = new GetClientResponse()
                {
                    code = -1,
                    message = "usuario no encontrado"
                };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:updateProfile"]
                                                , data.clientId
                                                , data.name
                                                , data.email
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response = new GetClientResponse()
                                {
                                    code = 1,
                                    message = "success",
                                    data = new AppClient()
                                    {
                                        //clientId = int.Parse(reader["clientId"].ToString()),
                                        name = reader["name"].ToString(),
                                        email = reader["email"].ToString(),
                                    }
                                };
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new GetClientResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }
    }
}
