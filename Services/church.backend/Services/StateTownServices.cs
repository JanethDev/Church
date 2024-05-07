using church.backend.Models.catalogue.days;
using church.backend.Models.catalogue.states_towns;
using church.backend.services.DataBase;

namespace church.backend.services.Services
{
    public class StateTownServices
    {
        private readonly stateTownsDB _stateTownsDB;

        public StateTownServices(stateTownsDB stateTownsDB)
        {
            _stateTownsDB = stateTownsDB;
        }

        public state_town_response consultStateTowns()
        {
            return _stateTownsDB.consultStateTowns();
        }
    }
}
