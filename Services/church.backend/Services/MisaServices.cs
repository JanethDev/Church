using church.backend.Models.catalogue.discounts;
using church.backend.Models.catalogue.misa;
using church.backend.Models.enums;
using church.backend.services.DataBase;
using church.backend.services.Models;

namespace church.backend.services.Services
{
    public class MisaServices
    {
        private readonly misaDB _misaDB;

        public MisaServices(misaDB misaDB)
        {
            _misaDB = misaDB;
        }

        public misa_response consultAll()
        {
            return _misaDB.consultAll();
        }

        public GeneralResponse createMisa(create_misa_request data, int user_id)
        {
            if (string.IsNullOrWhiteSpace(data.hour)) {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un horario de misa"
                };
            }
            if (data.day_id<0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar el dia de la misa"
                };
            }
            return _misaDB.createMisa(data, user_id);
        }

        public GeneralResponse deleteMisa(int id, int user_id)
        {
            return _misaDB.deleteMisa(id, user_id);
        }
    }
}
