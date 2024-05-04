using church.backend.Models.catalogue.civil_status;
using church.backend.Models.catalogue.intents;
using church.backend.services.DataBase;
using church.backend.services.Models;

namespace church.backend.services.Services
{
    public class IntentsServices
    {
        private readonly intentsDB _intentsDB;

        public IntentsServices(intentsDB intentsDB)
        {
            _intentsDB = intentsDB;
        }

        public intents_response consultIntents()
        {
            return _intentsDB.consultIntents();
        }

        public GeneralResponse createIntent(string intent, int user_id)
        {
            if (string.IsNullOrWhiteSpace(intent)) {
                return new GeneralResponse() { 
                    code = -1,
                    message = "Es necesario enviar un motivo de misa"
                };
            }
            return _intentsDB.createIntent(intent, user_id);
        }
    }
}
