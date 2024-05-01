namespace church.backend.Models.catalogue.discounts
{
    public class create_discount_request
    {
        public string description { get; set; } = string.Empty;
        public double percentage { get; set; } = new double();
        public DateTime start_date { get; set; } = new DateTime();
        public DateTime end_date { get; set; } = new DateTime();
    }
}
