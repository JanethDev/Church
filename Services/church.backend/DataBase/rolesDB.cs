using church.backend.Models.catalogue.roles;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class rolesDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public rolesDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public roles_response consultRoles() 
        {
            try
            {
                roles_response response = new roles_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:roles:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.data.Add(new roles()
                                {
                                    id = int.Parse(reader["id"].ToString()!),
                                    role = reader["role"].ToString()!
                                });
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new roles_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }
    }
}