using Newtonsoft.Json;

namespace church.backend.Models.purchase
{
    public class payment_request
    {
        public double paymentAmount { get; set; } = new double();
        public string concept { get; set; } = string.Empty;
        public int typePaymentId { get; set; } = new int();
        public int currencyId { get; set; } = new int();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int number { get; set; } = new int();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime date { get; set; } = new DateTime();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int statusId { get; set; } = new int();
    }
}
