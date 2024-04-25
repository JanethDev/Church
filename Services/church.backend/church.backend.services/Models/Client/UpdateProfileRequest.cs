using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.Models.Client
{
    public class UpdateProfileRequest
    {
        public string name { get; set; }
        public string email { get; set; }
        public int clientId { get; set; }
    }
}
