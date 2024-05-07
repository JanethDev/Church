using church.backend.Models.catalogue.maintenance_fee;
using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class maintenanceFeeDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public maintenanceFeeDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public maintenance_fee_response consultMaintenanceFee() 
        {
            try
            {
                maintenance_fee_response response = new maintenance_fee_response() { code = -1, message = "error" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:maintenance_fee:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response = new maintenance_fee_response() 
                                { 
                                    code = 1,
                                    message = "",
                                    data = new maintenance_fee()
                                    {
                                        cost = double.Parse(reader["cost"].ToString()!),
                                        shared_cost = double.Parse(reader["cost_shared"].ToString()!),
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
                return new maintenance_fee_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }

        public GeneralResponse updateMaintenanceFee(double sharedCost, double cost, int user_id)
        {
            try
            {
                GeneralResponse response = new GeneralResponse() { code = -1, message = "error" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:maintenance_fee:update"]!,cost,sharedCost,user_id);
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