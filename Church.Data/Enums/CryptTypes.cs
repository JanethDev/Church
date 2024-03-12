using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.Enums
{
    public enum CryptTypes
    {
        [Display(Name = "Familiar")]
        Familiar = 1,
        [Display(Name = "Individual")]
        Individual = 2
    }
}
