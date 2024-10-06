using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace church.backend.Models.register
{
    public class BeneficiarieRequest
    {
        [Required]
        public int customerId { get; set; } = new int();
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        public string lastname { get; set; } = string.Empty;
        [Required]
        public string phone { get; set; } = string.Empty;
        [Required]        
        public DateTime birthdate { get; set; } = new DateTime();
        [Required]
        public string relationship { get; set; } = string.Empty;
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int user_id { get; set; } = new int();
    }
}
