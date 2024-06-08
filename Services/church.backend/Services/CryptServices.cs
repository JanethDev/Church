using church.backend.Models.catalogue.crypts;
using church.backend.Models.catalogue.discounts;
using church.backend.Models.enums;
using church.backend.services.DataBase;
using church.backend.services.Models;

namespace church.backend.services.Services
{
    public class CryptServices
    {
        private readonly cryptDB _cryptDB;

        public CryptServices(cryptDB cryptDB)
        {
            _cryptDB = cryptDB;
        }

        public crypt_response consultByZone(string zone)
        {
            if (string.IsNullOrWhiteSpace(zone))
            {
                return new crypt_response()
                {
                    code = -1,
                    message = "Es necesario enviar la zona"
                };
            }
            return _cryptDB.consultByZone(zone);
        }

        public crypt_response consultById(string idString)
        {
            int id = 0;
            try
            {
                id = int.Parse(idString);
            }
            catch{
                return new crypt_response()
                {
                    code= -1,
                    message ="es necesario enviar un id válido"
                };
            }
            return _cryptDB.consultById(id);
        }

        public crypt_zones consultZones()
        {
            return _cryptDB.consultZones();
        }

        public GeneralResponse updateCrypt(update_crypt_request data, int user_id)
        {
            return _cryptDB.updateCrypt(data, user_id);
        }

        /*public void addc()
        {
            _cryptDB.addCrypt();
        }*/
    }
}
