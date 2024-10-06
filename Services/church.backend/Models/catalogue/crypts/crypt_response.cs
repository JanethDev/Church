using church.backend.services.Models;

namespace church.backend.Models.catalogue.crypts
{
    public class crypt_response:GeneralResponse
    {
        public List<crypt> data { get; set; } = new List<crypt>();
    }

    public class crypt_responseV2 :GeneralResponse
    {
        public crypt_with_level data {get;set;} = new crypt_with_level();
    }

    public class crypt_with_level :GeneralResponse
    {
        public List<crypt> crypts { get; set; } = new List<crypt>();
        public List<string> levels {get;set;} = new List<string>(); 
    }
}
