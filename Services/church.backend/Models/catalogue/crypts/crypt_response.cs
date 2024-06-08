using church.backend.services.Models;

namespace church.backend.Models.catalogue.crypts
{
    public class crypt_response:GeneralResponse
    {
        public List<crypt> data { get; set; } = new List<crypt>();
    }
}
