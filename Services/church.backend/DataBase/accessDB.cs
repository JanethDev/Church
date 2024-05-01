using church.backend.services.Models;
using church.backend.services.Models.access;
using church.backend.services.Models.enums;
using church.backend.services.Models.register;
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
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }

        public login_response login(string email,string password)
        {
            try
            {
                login_response response = new login_response();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:employeeLogin"]!
                                                , email
                                                , password
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    response = new login_response()
                                    {
                                        code = int.Parse(reader["code"].ToString()!),
                                        message = reader["message"].ToString()!,
                                        data = new user()
                                        {
                                            id = int.Parse(reader["cat_user_id"].ToString()!),
                                            email = reader["email"].ToString()!,
                                            name = $"{reader["name"]} {reader["psurname"]} {reader["msurname"]}",
                                            role_id = int.Parse(reader["cat_roles_id"].ToString()!),
                                            role = reader["role"].ToString()!,
                                        }
                                    };
                                }
                                catch {
                                    response = new login_response()
                                    {
                                        code = int.Parse(reader["code"].ToString()!),
                                        message = reader["message"].ToString()!
                                    };
                                }
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new login_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse ChangePassword(int user_id, string password)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:updatePassword"]!
                                                , user_id
                                                , password
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) {
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

        public GeneralResponse SetTempPassword(string email, string temp)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:recoverPassword"]!
                                                , email
                                                , temp
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) {
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

        public GeneralResponse createEmployee(create_employee_request data) 
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:createUserEmployee"]!
                        , data.email
                        , data.password
                        , data.role_id
                        , (int)user_status.Activo
                        , data.name
                        , data.father_last_name
                        , data.mother_last_name
                        , data.phone
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
