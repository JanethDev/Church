namespace church.backend.Models.misa_intents
{
    public class misa_intents
    {
        public int id { get; set; } = new int();
        public int intent_id { get; set; } = new int();
        public string intent { get;set; } = string.Empty;

        public int misa_id { get; set; } = new int();
        public string misa_day { get; set; } = string.Empty;
        public string misa_hour { get; set; } = string.Empty;
        public DateTime date { get; set; } = new DateTime();
        public string mention_person { get; set; } = string.Empty;
        public string applicant { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public double donation { get; set; } = new double();
        public double exchange_rate { get; set; } = new double();
        public string decription { get; set; } = string.Empty;
        public int status_id { get; set; } = new int();
        public string status { get; set; } = string.Empty;
    }
}
