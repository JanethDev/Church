using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.Enums
{
    public enum PaymentMethods
    {
        [Display(Name = "Contado")]
        Contado = 1,
        [Display(Name = "12 meses")]
        Meses12 = 2,
        [Display(Name = "18 meses")]
        Meses18 = 5,
        [Display(Name = "24 meses")]
        Meses24 = 3,
        [Display(Name = "36 meses")]
        Meses36 = 6,
        [Display(Name = "48 meses")]
        Meses48 = 7,
        [Display(Name = "Uso inmediato 50%")]
        UsoInmediato = 4
    }
}
