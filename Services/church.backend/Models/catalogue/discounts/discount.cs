namespace church.backend.Models.catalogue.discounts
{
    public class discount
    {
        public int id {  get; set; } = new int();
        public string description { get; set; } = string.Empty;
        public double percentage { get; set; } = new double();
        public DateTime start_date { get; set; } = new DateTime();
        public DateTime end_date { get; set; } = new DateTime();
        public int status_id { get; set; } = new int();
        public string status { get; set; } = string.Empty;
    }
}
