using church.backend.services.Models;

namespace church.backend.Models.catalogue.discounts
{
    public class discount_response:GeneralResponse
    {
        public List<discount> data { get; set; } = new List<discount>();
    }
}
