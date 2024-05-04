using church.backend.Models.catalogue.days;
using church.backend.services.DataBase;

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
