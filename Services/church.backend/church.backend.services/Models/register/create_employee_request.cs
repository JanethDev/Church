using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.Models.register
{
    public class create_employee_request
    {
        public string email { get; set; } = string.Empty;
        [JsonIgnore]
        public string password { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string father_last_name { get; set; } = string.Empty;
        public string mother_last_name { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public int role_id { get; set; } = new int();
        [JsonIgnore]
        public int user_id { get; set; } = new int();
    }
}
