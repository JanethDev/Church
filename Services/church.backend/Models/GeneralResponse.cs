using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.Models
{
    public class GeneralResponse
    {
        public int code { get; set; } = new int();
        public string message { get; set; } = string.Empty;
    }


}
