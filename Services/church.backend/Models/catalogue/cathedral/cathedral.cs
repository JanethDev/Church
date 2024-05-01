namespace church.backend.Models.catalogue.cathedral
{
    public class cathedral
    {
        public int id { get; set; } = new int();
        public string name { get; set; } = string.Empty;

        public int city_id { get; set; } = new int();
        public string city { get; set; } = string.Empty;
    }
}
