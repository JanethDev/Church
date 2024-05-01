using church.backend.services.Models;

namespace church.backend.Models.catalogue.cities
{
    public class cities_response:GeneralResponse
    {
        public List<cities> data {  get; set; } = new List<cities> ();
    }
}
