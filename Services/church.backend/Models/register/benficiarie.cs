using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace church.backend.Models.register
{
    public class Beneficiarie
    {
        public int id { get; set; } = new int();
        public string name { get; set; } = string.Empty;
        public string lastname { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public DateTime birthdate { get; set; } = new DateTime();
        public string relationship { get; set; } = string.Empty;
    }
}
