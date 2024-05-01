using church.backend.Models.catalogue.cathedral;
using church.backend.services.DataBase;
using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.Services
{
    public class CathedralServices
    {
        private readonly cathedralDB _cathedralDB;

        public CathedralServices(cathedralDB cathedralDB)
        {
            _cathedralDB = cathedralDB;
        }

        public cathedral_response consultCathedrals()
        {
            return _cathedralDB.consultCathedrals();
        }

        public GeneralResponse createCathedrals(create_cathedral_request data, int user_id)
        {
            if (string.IsNullOrWhiteSpace(data.name)) {
                return new GeneralResponse() { 
                    code = -1,
                    message = "Es necesario enviar el nombre de la catedral"
                };
            }
            if (data.city_id <= 0)
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Es necesario enviar un id válido de la cuidad"
                };
            }
            return _cathedralDB.createCathedrals(data,user_id);
        }
    }
}
