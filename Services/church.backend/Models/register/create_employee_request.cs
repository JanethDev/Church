using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.Models.register
{
    public class create_employee_request
    {
        [Required]
        public string email { get; set; } = string.Empty;
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public string password { get; set; } = string.Empty;
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        public string father_last_name { get; set; } = string.Empty;
        [Required]
        public string mother_last_name { get; set; } = string.Empty;
        [Required]
        public string phone { get; set; } = string.Empty;
        [Required]
        public int role_id { get; set; } = new int();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public int user_id { get; set; } = new int();
    }
}
