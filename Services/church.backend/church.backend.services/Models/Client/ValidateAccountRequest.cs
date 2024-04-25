using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.Models.Client
{
    public class ValidateAccountRequest
    {
        public string email { get; set; }
        public int code { get; set; }
    }
}
