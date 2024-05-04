using church.backend.Models.catalogue.civil_status;
using church.backend.services.DataBase;
using church.backend.services.Models;

namespace church.backend.services.Services
{
    public class CivilStatusServices
    {
        private readonly civilStatusDB _civilStatusDB;

        public CivilStatusServices(civilStatusDB civilStatusDB)
        {
            _civilStatusDB = civilStatusDB;
        }

        public civil_status_response consultCivilStatus()
        {
            return _civilStatusDB.consultCivilStatus();
        }

        public GeneralResponse createCivilStatus(string name, int user_id)
        {
            if (string.IsNullOrWhiteSpace(name)) {
                return new GeneralResponse() { 
                    code = -1,
                    message = "Es necesario enviar el nombre del estatus civil"
                };
            }
            return _civilStatusDB.createCivilStatus(name, user_id);
        }
    }
}
