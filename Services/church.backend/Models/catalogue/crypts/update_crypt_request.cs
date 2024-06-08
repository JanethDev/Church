namespace church.backend.Models.catalogue.crypts
{
    public class update_crypt_request
    {
        public int id { get; set; } = new int();
        public int status_id { get; set; } = new int();
        public bool is_shared { get; set; } = new bool();
        public int places_shared { get; set; } = new int();
        public double price { get; set; } = new double();
        public double price_shared { get; set; } = new double();
    }
}
