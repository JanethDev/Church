using church.backend.services.Models;

namespace church.backend.Models.purchase
{
    public class CurrencyResponse : GeneralResponse
    {
        public List<Currency> data { get; set; } = new List<Currency>();
    }
}
