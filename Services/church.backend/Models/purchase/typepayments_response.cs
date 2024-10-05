using church.backend.services.Models;

namespace church.backend.Models.purchase
{
    public class TypePaymentsResponse : GeneralResponse
    {
        public List<TypePayment> data { get; set; } = new List<TypePayment>();
    }
}
