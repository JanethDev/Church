using church.backend.services.Models;

namespace church.backend.Models.catalogue.crypts
{
    public class crypt_zones:GeneralResponse
    {
        public List<string> data { get; set; } = new List<string>();
    }
}
