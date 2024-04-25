
namespace church.backend.services.Models.Client
{
    public class AppClient
    {
        public int clientId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int postal_code { get; set; }
        public string rfc { get; set; }
        public string picture { get; set; }
        public bool isMoral { get; set; }
        public bool isValidated { get; set; }
        public bool isEmailValidated { get; set; }
    }
}
