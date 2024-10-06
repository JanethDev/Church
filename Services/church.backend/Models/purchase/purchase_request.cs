
using Newtonsoft.Json;

namespace church.backend.Models.purchase
{
    public class purchase_request
    {
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int userId {  get; set; } = new int();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int purchaseId {  get; set; } = new int();
        public string tuition { get; set; } = string.Empty;
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime datePurchase { get; set; } = new DateTime();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int statusId {  get; set; } = new int();
        public int cryptId {  get; set; } = new int();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public double cryptPrice {  get; set; } = new double();
        public int cryptSpaces {  get; set; } = new int();
        public double maintenanceFee {  get; set; } = new double();
        public double federalTax {  get; set; } = new double();
        public int discountId {  get; set; } = new int();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public double discountAmount {  get; set; } = new double();
        public double ashDeposit {  get; set; } = new double();
        public int customerId {  get; set; } = new int();
        public int monthlyPayments {  get; set; } = new int();
        public string referencePerson1 { get; set; } = string.Empty;
        public string referencePersonPhone1 { get; set; } = string.Empty;
        public string referencePerson2 { get; set; } = string.Empty;
        public string referencePersonPhone2 { get; set; } = string.Empty;
        public List<payment_request> payments { get; set; } = new List<payment_request>();
    }
}
