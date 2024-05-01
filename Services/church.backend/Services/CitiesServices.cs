using church.backend.Models.catalogue.cathedral;
using church.backend.Models.catalogue.cities;
using church.backend.services.DataBase;
using church.backend.services.Models;
using System.Data.SqlClient;

namespace church.backend.services.Services
{
    public class CitiesServices
    {
        private readonly citiesDB _citiesDB;

        public CitiesServices(citiesDB citiesDB)
        {
            _citiesDB = citiesDB;
        }

        public cities_response consultCities()
        {
            return _citiesDB.consultCities();
        }

        public GeneralResponse createCity(string city, int user_id)
        {
            if (string.IsNullOrWhiteSpace(city)) {
                return new GeneralResponse() { 
                    code = -1,
                    message = "Es necesario enviar el nombre de la ciudad"
                };
            }
            return _citiesDB.createCity(city, user_id);
        }
    }
}
