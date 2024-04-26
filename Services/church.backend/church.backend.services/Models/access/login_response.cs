using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.Models.access
{
    public class login_response:GeneralResponse
    {
        public user data { get; set; } = new user();
    }
}
