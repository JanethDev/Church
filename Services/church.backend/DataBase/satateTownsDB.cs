using church.backend.Models.catalogue.days;
using church.backend.Models.catalogue.states_towns;
using System.Data.SqlClient;

namespace church.backend.services.DataBase
{
    public class stateTownsDB
    {
        private readonly string DataBaseConection;
        private readonly IConfiguration _configuration;
        public stateTownsDB(IConfiguration configuration)
        {
            DataBaseConection = configuration["connectionStrings:database:dev"]!;
            _configuration = configuration;
        }
        public state_town_response consultStateTowns() 
        {
            try
            {
                state_town_response response = new state_town_response() { code = 1, message = "success" };
                using (SqlConnection connection = new SqlConnection(DataBaseConection))
                {
                    string query = string.Format(_configuration["queries:state_towns:consultAll"]!);
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int index = response.data.FindIndex(element => element.state.id == int.Parse(reader["state_id"].ToString()!));
                                
                                if(index == -1)
                                {
                                    response.data.Add(new state_town_response_data()
                                    {
                                        state = new state()
                                        {
                                            id = int.Parse(reader["state_id"].ToString()!),
                                            state_name = reader["state"].ToString()!
                                        }
                                    });

                                    index = response.data.FindIndex(element => element.state.id == int.Parse(reader["state_id"].ToString()!));
                                    try {
                                        response.data[index].towns_list.Add(new towns()
                                        {
                                            id = int.Parse(reader["town_id"].ToString()!),
                                            town_name = reader["town"].ToString()!
                                        });
                                    }
                                    catch { }
                                }
                                else
                                {
                                    try
                                    {
                                        response.data[index].towns_list.Add(new towns()
                                        {
                                            id = int.Parse(reader["town_id"].ToString()!),
                                            town_name = reader["town"].ToString()!
                                        });
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new state_town_response()
                {
                    code = -1,
                    message = ex.Message
                };
            }
        }
    }
}