using church.backend.services.Models;

namespace church.backend.Models.misa_intents
{
    public class misa_intents_response:GeneralResponse
    {
        public List<misa_intents> data { get; set; } = new List<misa_intents>();
    }
}
