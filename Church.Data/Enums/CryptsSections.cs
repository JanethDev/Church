using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.Enums
{
    public enum CryptsSections
    {
        [Display(Name = "Sección A")]
        SeccionA = 1,
        [Display(Name = "Sección B")]
        SeccionB = 2,
        [Display(Name = "Sección C")]
        SeccionC = 3
    }
}
