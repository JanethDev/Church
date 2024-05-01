using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.Models.access
{
    public class user
    {
        public int id { get; set; } = new int();
        public string email { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public int role_id { get; set; } = new int();
        public string role { get; set; } = string.Empty;
    }
}
