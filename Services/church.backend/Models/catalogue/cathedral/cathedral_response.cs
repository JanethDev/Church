using church.backend.services.Models;

namespace church.backend.Models.catalogue.cathedral
{
    public class cathedral_response : GeneralResponse
    {
        public List<cathedral> data { get; set; } = new List<cathedral>();
    }
}
