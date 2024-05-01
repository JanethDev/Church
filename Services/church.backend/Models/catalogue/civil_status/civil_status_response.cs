using church.backend.services.Models;

namespace church.backend.Models.catalogue.civil_status
{
    public class civil_status_response:GeneralResponse
    {
        public List<civil_status> data { get; set; } = new List<civil_status>();
    }
}
