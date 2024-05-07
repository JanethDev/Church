namespace church.backend.Models.catalogue.misa
{
    public class misa
    {
        public int id { get; set; } = new int();
        public int day_id { get; set; } = new int();
        public string day { get; set; } = string.Empty;
        public string hour { get; set; } = string.Empty;
    }
}
