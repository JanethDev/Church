using church.backend.services.Models;

namespace church.backend.Models.register
{
    public class BeneficiarieResponse : GeneralResponse
    {
        public List<Beneficiarie> data { get; set; } = new List<Beneficiarie>();
    }
}
