using church.backend.services.Models;

namespace church.backend.Models.register
{
    public class list_customer:GeneralResponse
    {
        public List<customer_response> customers { get; set; } = new List<customer_response>();
    }
}
