using church.backend.services.Models;

namespace church.backend.Models.catalogue.states_towns
{
    public class state_town_response : GeneralResponse
    {
        public List<state_town_response_data> data { get; set; } = new List<state_town_response_data>();
    }

    public class state_town_response_data
    {
        public state state { get; set; } = new state();
        public List<towns> towns_list { get; set; } = new List<towns>();
    }
}
