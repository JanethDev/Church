using church.backend.services.Models;

namespace church.backend.Models.purchase
{
    public class PurchaseResponse : GeneralResponse
    {
        public List<Purchase> data { get; set; } = new List<Purchase>();
    }
}