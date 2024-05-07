using church.backend.Models.catalogue.maintenance_fee;
using church.backend.services.DataBase;
using church.backend.services.Models;

namespace church.backend.services.Services
{
    public class MaintenanceFeeServices
    {
        private readonly maintenanceFeeDB _maintenanceFeeDB;

        public MaintenanceFeeServices(maintenanceFeeDB maintenanceFeeDB)
        {
            _maintenanceFeeDB = maintenanceFeeDB;
        }

        public maintenance_fee_response consultMaintenanceFee()
        {
            return _maintenanceFeeDB.consultMaintenanceFee();
        }

        public GeneralResponse updateMaintenanceFee(string sharedCostString, string costString, int user_id)
        {
            double cost = 0;
            try { 
                cost = double.Parse(costString);
            }
            catch {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Envie un valor válido"
                };
            }

            double sharedCost = 0;
            try
            {
                sharedCost = double.Parse(sharedCostString);
            }
            catch
            {
                return new GeneralResponse()
                {
                    code = -1,
                    message = "Envie un valor válido"
                };
            }
            return _maintenanceFeeDB.updateMaintenanceFee(sharedCost,cost, user_id);
        }
    }
}
