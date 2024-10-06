namespace church.backend.Models.catalogue.crypts
{
    public class crypt
    {
        public int id { get; set; } = new int();
        public int status_id { get; set; } = new int();
        public string status {  get; set; } = string.Empty;
        public string full_position { get; set; } = string.Empty;
        public string zone { get; set; } = string.Empty;
        public string level { get; set; } = string.Empty;
        public string position { get; set; } = string.Empty;
        public string aisle { get; set; } = string.Empty;
        public bool is_shared { get; set; } = new bool();
        public int places_shared { get; set; } = new int();
        public double price { get; set; } = new double();
        public double price_shared { get; set; } = new double();
    }
}
