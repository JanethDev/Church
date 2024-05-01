using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using church.backend.services.Models;

namespace church.backend.Models.catalogue.roles
{
    public class roles_response : GeneralResponse
    {
        public List<roles> data { get; set; } = new List<roles>();
    }
}
