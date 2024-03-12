using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.Enums
{
    public enum ExchangeTypes
    {
        [Display(Name = "MXN")]
        MXN = 1,
        [Display(Name = "DLLS")]
        DLLS = 2
    }
}
