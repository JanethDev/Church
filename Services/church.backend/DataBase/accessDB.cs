using church.backend.Models.register;
using church.backend.services.Models;
using church.backend.services.Models.access;
using church.backend.services.Models.enums;
using church.backend.services.Models.register;
using church.backend.Utilities;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class accessDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        private readonly NullValues _nv;
        public accessDB(IConfiguration configuration, NullValues nv)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
            _nv = nv;
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

        public GeneralResponse createCustomer(create_customer_request data)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:createCustomer"]!
                        , data.email
                        , data.password
                        , 7 //customer role
                        , (int)user_status.Activo
                        , data.name
                        , data.father_last_name
                        , data.mother_last_name
                        , data.phone
                        , data.rfc
                        , data.zip_code
                        , data.address
                        , data.catStatesId
                        , data.catTownsId
                        , data.social_reason
                        , data.birthdate.ToString("yyyy-MM-dd")
                        , data.birth_place
                        , data.civil_status
                        , data.occupation
                        , data.business_name
                        , data.business_address
                        , data.business_phone
                        , data.business_ext
                        , data.deputation
                        , data.average_income
                        , data.business_city
                        , data.business_municipality
                        , data.business_state
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

        public GeneralResponse createBeneficiaries(BeneficiarieRequest data)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:createBeneficiaries"]!
                        , data.customerId
                        , data.name
                        , data.lastname                       
                        , data.birthdate.ToString("yyyy-MM-dd")
                        , data.phone
                        , data.relationship
                        , data.user_id
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

        public GeneralResponse deleteBeneficiaries(int beneficiarieId, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse();
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:deleteBeneficiaries"]!
                        , beneficiarieId
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

        public BeneficiarieResponse consultBeneficiaries(int customerId)
        {
            try
            {
                BeneficiarieResponse response = new BeneficiarieResponse(){code = 1};
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:access:beneficiariesByCustomer"]!
                        , customerId
                    );
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new Beneficiarie(){
                                    id = _nv.nullInt(reader["id"].ToString()!),
                                    name = reader["name"].ToString()!,
                                    lastname = reader["lastname"].ToString()!,
                                    birthdate = _nv.nullDate(reader["birthdate"].ToString()!),
                                    phone = reader["phone"].ToString()!,
                                    relationship = reader["relationship"].ToString()!,
                                });                             
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new BeneficiarieResponse()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public list_customer SearchCustomer(string value)
        {
            try
            {
                list_customer response = new list_customer() { code = 1};
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = @"SELECT 
	                     cus.*
	                     , st.state
	                     , tw.town
                      FROM [cat_customers] cus
                      LEFT JOIN cat_states st ON st.id = cus.cat_states_id
                      LEFT JOIN cat_towns tw ON tw.id = cus.cat_towns_id
                      WHERE email like '%{0}%'
                      OR cus.phone like '%{0}%'
                      OR cus.[name] like '%{0}%'
                      OR cus.msurname like '%{0}%'
                      OR cus.psurname like '%{0}%'
                      AND cus.cat_status_id = '{1}'";
                    query = string.Format(query, value, (int)user_status.Activo);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.customers.Add(new customer_response()
                                {
                                    id = _nv.nullInt(reader["id"].ToString()!),
                                    customerNumber = _nv.nullInt(reader["customer_number"].ToString()!),
                                    name = reader["name"].ToString()!,
                                    father_last_name = reader["psurname"].ToString()!,
                                    mother_last_name = reader["msurname"].ToString()!,
                                    phone = reader["phone"].ToString()!,
                                    email = reader["email"].ToString()!,
                                    rfc = reader["rfc"].ToString()!,
                                    zip_code = _nv.nullInt(reader["cp_code"].ToString()!),
                                    address = reader["address"].ToString()!,
                                    catStatesId = _nv.nullInt(reader["cat_states_id"].ToString()!),
                                    state = reader["state"].ToString()!,
                                    catTownsId = _nv.nullInt(reader["town"].ToString()!),
                                    town = reader["town"].ToString()!,
                                    social_reason = reader["social_reason"].ToString()!,
                                    birthdate = _nv.nullDate(reader["birthdate"].ToString()!),
                                    birth_place  = reader["birth_place"].ToString()!,
                                    civil_status = reader["civil_status"].ToString()!,
                                    occupation = reader["occupation"].ToString()!,
                                    business_name = reader["business_name"].ToString()!,
                                    business_address = reader["business_address"].ToString()!,
                                    business_city = reader["business_city"].ToString()!,
                                    business_municipality = reader["business_ municipality"].ToString()!,
                                    business_state = reader["business_state"].ToString()!,
                                    business_phone = reader["business_phone"].ToString()!,
                                    business_ext = reader["business_ext"].ToString()!,
                                    deputation = reader["deputation"].ToString()!,
                                    average_income = _nv.nullDouble(reader["average_income"].ToString()??"0"),
                                    house_number = reader["house_number"].ToString()!,
                                    apt_number = reader["apt_number"].ToString()!,
                                    customer_municipality = reader["customer_municipality"].ToString()!,
                                    neighborhood = reader["neighborhood"].ToString()!,
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new list_customer()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }
    }
}
