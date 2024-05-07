using church.backend.services.Models;

namespace church.backend.Models.catalogue.misa
{
    public class misa_response:GeneralResponse
    {
        public List<misa> data { get; set; } = new List<misa>();
    }
}
