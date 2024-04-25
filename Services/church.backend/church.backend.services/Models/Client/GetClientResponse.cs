using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace church.backend.services.Models.Client
{
    public class GetClientResponse:GeneralResponse
    {
        public AppClient data { get; set; } = new AppClient();
    }
}
