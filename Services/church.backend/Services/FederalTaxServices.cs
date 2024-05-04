using church.backend.services.DataBase;
using church.backend.services.Models;

namespace church.backend.services.Services
{
    public class FederalTaxServices
    {
        private readonly federalTaxDB _federalTaxDB;

        public FederalTaxServices(federalTaxDB federalTaxDB)
        {
            _federalTaxDB = federalTaxDB;
        }

        public GeneralResponse consultFederalTax()
        {
            return _federalTaxDB.consultFederalTax();
        }

        public GeneralResponse updateFederalTax(string costString, int user_id)
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
            return _federalTaxDB.updateFederalTax(cost, user_id);
        }
    }
}
