using church.backend.Models.catalogue.days;
using church.backend.Models.catalogue.roles;
using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models.register;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace church.backend.services.Services
{
    public class DaysServices
    {
        private readonly daysDB _daysDB;

        public DaysServices(daysDB daysDB)
        {
            _daysDB = daysDB;
        }

        public days_response consultDays()
        {
            return _daysDB.consultDays();
        }
    }
}
