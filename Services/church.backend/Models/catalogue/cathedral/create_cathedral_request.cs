using System.ComponentModel.DataAnnotations;

namespace church.backend.Models.catalogue.cathedral
{
    public class create_cathedral_request
    {
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        public int city_id { get; set; } = new int();
    }
}
