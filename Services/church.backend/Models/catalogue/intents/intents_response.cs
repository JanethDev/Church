using church.backend.services.Models;

namespace church.backend.Models.catalogue.intents
{
    public class intents_response:GeneralResponse
    {
        public List<intents> data { get; set; } = new List<intents>();
    }
}
