using church.backend.services.Models;

namespace church.backend.Models.catalogue.days
{
    public class days_response:GeneralResponse
    {
        public List<days> data { get; set; } = new List<days>();
    }
}
