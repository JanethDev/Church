using church.backend.Models.catalogue.discounts;
using church.backend.Models.enums;
using church.backend.Models.misa_intents;
using church.backend.services.DataBase;
using church.backend.services.Models;

namespace church.backend.services.Services
{
    public class MisaIntentsServices
    {
        private readonly misaIntentsDB _misaIntentsDB;

        public MisaIntentsServices(misaIntentsDB misaIntentsDB)
        {
            _misaIntentsDB = misaIntentsDB;
        }

        public misa_intents_response consultAll()
        {
            return _misaIntentsDB.consultAll();
        }

        public misa_intents_response consultByStatus(misa_intents_status status)
        {
            return _misaIntentsDB.consultByStatus(status);
        }

        public GeneralResponse createMisaIntent(create_misa_intent_request data, int user_id)
        {
            if (data.intent_id <= 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar una intención de misa"
                };
            }
            if (data.misa_id <= 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id de misa"
                };
            }
            if (string.IsNullOrWhiteSpace(data.applicant))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar persona que aplica"
                };
            }
            if (string.IsNullOrWhiteSpace(data.mention_person))
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar persona a mencionar"
                };
            }
            if(data.date == new DateTime())
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar una fecha de misa"
                };
            }
            if (data.donation <= 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar una donación"
                };
            }
            return _misaIntentsDB.createMisaIntent(data, user_id);
        }

        public GeneralResponse updateMisaIntent(update_misa_intent_request data, int user_id)
        {
            return _misaIntentsDB.updateMisaIntent(data, user_id);
        }
    }
}
