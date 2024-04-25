using Newtonsoft.Json;

namespace church.backend.services.Models.Client
{
    public class SaveClientRequest
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("password")]
        public string password { get; set; }
        [JsonProperty("phoneNumber")]
        public string phoneNumber { get; set; }
        [JsonProperty("city")]
        public string city { get; set; }
        [JsonProperty("state")]
        public string state { get; set; }
        [JsonProperty("cp")]
        public int postal_code { get; set; }
        [JsonProperty("rfc")]
        public string rfc { get; set; }
        [JsonProperty("isMoral")]
        public bool isMoral { get; set; }
        [JsonProperty("reference")]
        public string reference { get; set; }

        [JsonProperty("brand")]
        public int brand { get; set; } = 0;
    }
}
