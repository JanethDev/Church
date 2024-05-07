using church.backend.services.Models;

namespace church.backend.Models.catalogue.maintenance_fee
{
    public class maintenance_fee_response : GeneralResponse
    {
        public maintenance_fee data {  get; set; } = new maintenance_fee();
    }
}
