using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.Enums
{
    public enum PaymentEstatus
    {
        [Display(Name = "Pendiente")]
        Pending = 1,
        [Display(Name = "En proceso")]
        ValidationProcess = 2,
        [Display(Name = "Pagado")]
        Paid = 3
    }
}