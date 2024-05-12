namespace church.backend.Models.misa_intents
{
    public class create_misa_intent_request
    {
        public int intent_id { get; set; } = new int();

        public int misa_id { get; set; } = new int();
        public DateTime date { get; set; } = new DateTime();
        public string mention_person { get; set; } = string.Empty;
        public string applicant { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public double donation { get; set; } = new double();
        public double exchange_rate { get; set; } = new double();
        public string decription { get; set; } = string.Empty;
    }
}
